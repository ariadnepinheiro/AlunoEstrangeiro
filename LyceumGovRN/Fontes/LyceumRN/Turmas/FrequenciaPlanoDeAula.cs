using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Turmas
{
    public class FrequenciaPlanoDeAula
    {
        public Entidades.FrequenciaPlanoDeAula ObtemPor(int ano, int periodo, string turma, string disciplina, DateTime dataFrequencia)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {                
                return this.ObtemPor(contexto, ano, periodo, turma, disciplina, dataFrequencia);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public Entidades.FrequenciaPlanoDeAula ObtemPor(DataContext contexto, int ano, int periodo, string turma, string disciplina, DateTime dataFrequencia)
        {
            Entidades.FrequenciaPlanoDeAula frequenciaPlanoDeAula = new Entidades.FrequenciaPlanoDeAula();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT F.*,
											ISNULL(P.NOME_COMPL, U.NOME) AS NOMEUSUARIO
                                        from Turma.FREQUENCIAPLANODEAULA F (NOLOCK)
											left join hades..HD_USUARIO u (nolock) on f.USUARIOID = u.USUARIO
											left join LY_DOCENTE d (nolock) on f.NUMFUNCLANCAMENTO = d.NUM_FUNC
											left join LY_PESSOA p (nolock) on isnull(d.PESSOA, u.PESSOA) = p.PESSOA
                                        where TURMA = @TURMA
	                                        AND ANO = @ANO
	                                        AND SEMESTRE = @PERIODO
	                                        AND DISCIPLINA = @DISCIPLINA
	                                        AND DATAFREQUENCIA = @DATAFREQUENCIA ";

            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

            frequenciaPlanoDeAula = contexto.TryToBindEntity<Entidades.FrequenciaPlanoDeAula>(contextQuery);

            return frequenciaPlanoDeAula;
        }

        public void Insere(DataContext contexto, Entidades.FrequenciaPlanoDeAula frequenciaPlanoDeAula)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Turma.FREQUENCIAPLANODEAULA
                                           (ANO
                                           ,SEMESTRE
                                           ,TURMA
                                           ,DISCIPLINA
                                           ,DATAFREQUENCIA
                                           ,PLANOAULA
                                           ,NUMFUNCLANCAMENTO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@ANO, 
                                           @SEMESTRE, 
                                           @TURMA, 
                                           @DISCIPLINA, 
                                           @DATAFREQUENCIA, 
                                           @PLANOAULA, 
                                           @NUMFUNCLANCAMENTO, 
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Decimal, frequenciaPlanoDeAula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Decimal, frequenciaPlanoDeAula.Semestre);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, frequenciaPlanoDeAula.Turma);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, frequenciaPlanoDeAula.Disciplina);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, frequenciaPlanoDeAula.DataFrequencia.Date);
            contextQuery.Parameters.Add("@PLANOAULA", SqlDbType.VarChar, frequenciaPlanoDeAula.PlanoAula);
            contextQuery.Parameters.Add("@NUMFUNCLANCAMENTO", SqlDbType.Decimal, DBNull.Value);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, frequenciaPlanoDeAula.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void Atualiza(DataContext contexto, Entidades.FrequenciaPlanoDeAula frequenciaPlanoDeAula)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE Turma.FREQUENCIAPLANODEAULA
                                       SET PLANOAULA = @PLANOAULA, 
                                           NUMFUNCLANCAMENTO = @NUMFUNCLANCAMENTO,
                                           USUARIOID = @USUARIOID,
                                           DATAALTERACAO = @DATAALTERACAO
                                     WHERE FREQUENCIAPLANODEAULAID = @FREQUENCIAPLANODEAULAID ";

            contextQuery.Parameters.Add("@FREQUENCIAPLANODEAULAID", SqlDbType.Int, frequenciaPlanoDeAula.FrequenciaPlanoDeAulaId);
            contextQuery.Parameters.Add("@PLANOAULA", SqlDbType.VarChar, frequenciaPlanoDeAula.PlanoAula);
            contextQuery.Parameters.Add("@NUMFUNCLANCAMENTO", SqlDbType.Decimal, DBNull.Value);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, frequenciaPlanoDeAula.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePor(DataContext contexto, string censo, DateTime dataFrequencia)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE PA
                                    FROM TURMA.FREQUENCIAPLANODEAULA PA
	                                    INNER JOIN LY_TURMA T ON PA.TURMA = T.TURMA AND PA.ANO = T.ANO AND PA.SEMESTRE = T.SEMESTRE AND PA.DISCIPLINA = T.DISCIPLINA
                                    WHERE T.FACULDADE = @CENSO
	                                    AND DATAFREQUENCIA = @DATAFREQUENCIA ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@DATAFREQUENCIA", SqlDbType.DateTime, dataFrequencia.Date);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
