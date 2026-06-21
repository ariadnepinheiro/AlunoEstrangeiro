using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Matriculas
{
    public class PessoaAluno
    {
        public Entidades.PessoaAluno ObtemPor(DataContext contexto, decimal pessoa)
        {
            Entidades.PessoaAluno pessoaAluno = new Entidades.PessoaAluno();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT * 
							FROM MATRICULA.PESSOAALUNO (NOLOCK)
							WHERE PESSOAID = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            pessoaAluno = contexto.TryToBindEntity<Entidades.PessoaAluno>(contextQuery);

            return pessoaAluno;
        }

        public void AtualizaSuspensao(DataContext contexto, int ano, int periodo, int historicoSuspensaoId, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" 	UPDATE MATRICULA.PESSOAALUNO
                                                SET ANOENCERRAMENTO = @ANO,
                                                    PERIODOENCERRAMENTO = @SEMESTRE,
                                                    USUARIOID = @USUARIOID,
                                                    DATAALTERACAO = GETDATE()
                                        FROM MATRICULA.PESSOAALUNO PA
	                                        INNER JOIN Turma.HISTORICOSUSPENSAO h ON h.ALUNO = pa.ALUNO
                                        WHERE HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);

            contexto.ApplyModifications(contextQuery);
        }

        public string ObtemOutraPessoaAlunoPor(decimal pessoa, string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            string matricula = string.Empty;

            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"SELECT  ALUNO
                            FROM   Matricula.PESSOAALUNO                                    
                            WHERE PESSOAID = @PESSOA                          
                            AND ALUNO <> @ALUNO ";

            try
            {
                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                matricula = ctx.GetReturnValue<string>(contextQuery);
            }
            catch (Exception ex)
            {
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
                ctx.Dispose();
            }

            return matricula;
        }

        public string ObtemOutraPessoaAlunoPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            string matricula = string.Empty;

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT PE.ALUNO 
                                    FROM   LY_ALUNO A (NOLOCK) 
                                           INNER JOIN Matricula.PESSOAALUNO PE (NOLOCK) 
                                                   ON A.PESSOA = PE.PESSOAID 
                                                      AND A.ALUNO <> PE.ALUNO 
                                    WHERE  A.ALUNO = @ALUNO ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

                matricula = ctx.GetReturnValue<string>(contextQuery);
            }
            catch (Exception ex)
            {
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
                ctx.Dispose();
            }

            return matricula;
        }

        public void Atualiza(DataContext contexto, string aluno, int anoEncerramento, int periodoEncerramento, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE MATRICULA.PESSOAALUNO
                                           SET ANOENCERRAMENTO = @ANOENCERRAMENTO,
                                               PERIODOENCERRAMENTO = @PERIODOENCERRAMENTO,
                                               USUARIOID = @USUARIOID,
                                               DATAALTERACAO = @DATAALTERACAO
                                    WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@ANOENCERRAMENTO", SqlDbType.Int, anoEncerramento);
            contextQuery.Parameters.Add("@PERIODOENCERRAMENTO", SqlDbType.Int, periodoEncerramento);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaAluno(DataContext contexto, decimal pessoa, string aluno, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE MATRICULA.PESSOAALUNO
                                           SET ALUNO = @ALUNO,
                                               USUARIOID = @USUARIOID,
                                               DATAALTERACAO = @DATAALTERACAO
                                    WHERE PESSOAID = @PESSOAID ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, pessoa);

            contexto.ApplyModifications(contextQuery);
        }       

        public void Insere(DataContext contexto, string aluno, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Matricula.PESSOAALUNO 
                                                (PESSOAID, 
                                                 ALUNO, 
                                                 USUARIOID, 
                                                 SITUACAOALUNO, 
                                                 ANOENCERRAMENTO, 
                                                 PERIODOENCERRAMENTO, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO, 
                                                 ESCOLAALUNO) 
                                    SELECT A.PESSOA, 
                                           A.ALUNO, 
                                           @USUARIO, 
                                           A.SIT_ALUNO, 
                                           T.ANO_ENCERRAMENTO, 
                                           T.SEM_ENCERRAMENTO, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO, 
                                           A.UNIDADE_ENSINO 
                                    FROM   LY_ALUNO A 
                                           LEFT JOIN (SELECT TOP 1 * 
                                                       FROM   LY_H_CURSOS_CONCL CC 
                                                       WHERE  CC.DT_REABERTURA IS NULL 
                                                              AND CC.ALUNO =  @ALUNO
                                                       ORDER  BY CC.ANO_ENCERRAMENTO DESC) T 
                                                   ON A.ALUNO = T.ALUNO 
                                    WHERE  A.ALUNO = @ALUNO  ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE MATRICULA.PESSOAALUNO
                                      WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE MATRICULA.PESSOAALUNO
                                      WHERE PESSOAID = @PESSOAID ";

            contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, pessoa);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePorPessoaOuAluno(DataContext contexto, decimal pessoa, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE MATRICULA.PESSOAALUNO
                                      WHERE PESSOAID = @PESSOAID
                                            OR ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);
            contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, pessoa);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiPessoaAlunoPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM Matricula.PESSOAALUNO 
                                    WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}
