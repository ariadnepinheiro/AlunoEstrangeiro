using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Matriculas
{
    public class MatriculaEspecialDisciplina
    {
        public DataTable ObtemListaPor(int ano, string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT M.MATRICULAESPECIALID,
	                       D.MATRICULAESPECIALDISCIPLINAID,
	                       D.DISCIPLINA,
	                       C.NOME AS NOMEDISCIPLINA,
	                       D.TURNO,
	                       T.DESCRICAO AS NOMETURNO,
	                       D.DATACADASTRO,
	                       CASE
			                    WHEN D.DATACONVOCACAO IS  NOT NULL THEN 'Disciplina inscrita'
			                    ELSE 'Fila de espera'
		                    END SITUACAO,
		                    D.DATACONVOCACAO
                    FROM MATRICULA.MATRICULAESPECIAL M (NOLOCK)
	                     INNER JOIN MATRICULA.MATRICULAESPECIALDISCIPLINA D (NOLOCK) 
			                    ON M.MATRICULAESPECIALID = D.MATRICULAESPECIALID
	                     INNER JOIN LY_CURSO C (NOLOCK)
			                    ON C.CURSO = D.DISCIPLINA
	                     INNER JOIN LY_TURNO T (NOLOCK)
			                    ON T.TURNO = D.TURNO
                    WHERE M.ALUNO = @ALUNO
	                      AND M.ANO = @ANO ";

                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

            return dt;
        }

        public ValidacaoDados ValidaRemocao(int matriculaEspecialDisciplinaId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (matriculaEspecialDisciplinaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica ainda existe                    
                    if (!this.ExistePor(contexto, matriculaEspecialDisciplinaId))
                    {
                        mensagens.Add("Esta opção não está mais ativa.");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private bool ExistePor(DataContext ctx, int matriculaEspecialDisciplinaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT  COUNT(1)
                                            FROM   Matricula.MATRICULAESPECIALDISCIPLINA
                            WHERE  MATRICULAESPECIALDISCIPLINAID = @MATRICULAESPECIALDISCIPLINAID  ";

            contextQuery.Parameters.Add("@MATRICULAESPECIALDISCIPLINAID", SqlDbType.Int, matriculaEspecialDisciplinaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void Remove(int matriculaEspecialDisciplinaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Insere Historico
                int motivoExclusaoDisciplinaId = 1; //2	EXCLUÍDO PELA GESTÃO
                this.InsereHistorio(ctx, matriculaEspecialDisciplinaId, motivoExclusaoDisciplinaId);

                //Remove
                this.Remove(ctx, matriculaEspecialDisciplinaId);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private void InsereHistorio(DataContext ctx, int matriculaEspecialDisciplinaId, int motivoExclusaoDisciplinaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Matricula.MATRICULAESPECIALDISCIPLINAHIST
                                                        (MATRICULAESPECIALDISCIPLINAID,
                                                         MATRICULAESPECIALID,
                                                         DISCIPLINA,
                                                         TURNO,
                                                         DATACONVOCACAO,
                                                         MOTIVOEXCLUSAODISCIPLINAID,
                                                         DATAEXCLUSAO,
                                                         USUARIOID,
                                                         DATACADASTRO,
                                                         DATAALTERACAO)
                                            SELECT MATRICULAESPECIALDISCIPLINAID,
                                                   MATRICULAESPECIALID,
                                                   DISCIPLINA,
                                                   TURNO,
                                                   DATACONVOCACAO,
                                                   @MOTIVOEXCLUSAODISCIPLINAID,
                                                   @DATAATUAL,
                                                   USUARIOID,
                                                   DATACADASTRO,
                                                   @DATAATUAL
                                            FROM   Matricula.MATRICULAESPECIALDISCIPLINA
                                            WHERE  MATRICULAESPECIALDISCIPLINAID = @MATRICULAESPECIALDISCIPLINAID  ";

            contextQuery.Parameters.Add("@MATRICULAESPECIALDISCIPLINAID", SqlDbType.Int, matriculaEspecialDisciplinaId);
            contextQuery.Parameters.Add("@MOTIVOEXCLUSAODISCIPLINAID", SqlDbType.Int, motivoExclusaoDisciplinaId);
            contextQuery.Parameters.Add("@DATAATUAL", SqlDbType.DateTime, DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        private void Remove(DataContext ctx, int matriculaEspecialDisciplinaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Matricula.MATRICULAESPECIALDISCIPLINA
                            WHERE  MATRICULAESPECIALDISCIPLINAID = @MATRICULAESPECIALDISCIPLINAID  ";

            contextQuery.Parameters.Add("@MATRICULAESPECIALDISCIPLINAID", SqlDbType.Int, matriculaEspecialDisciplinaId);

            ctx.ApplyModifications(contextQuery);
        }

        public void AtualizaConvocacao(DataContext contexto, int matriculaEspecialDisciplinaId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   UPDATE Matricula.MATRICULAESPECIALDISCIPLINA
                                        SET DATACONVOCACAO = @DATAATUAL,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAATUAL
                                        WHERE MATRICULAESPECIALDISCIPLINAID = @MATRICULAESPECIALDISCIPLINAID ";

            contextQuery.Parameters.Add("@MATRICULAESPECIALDISCIPLINAID", SqlDbType.Int, matriculaEspecialDisciplinaId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAATUAL", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

    }
}
