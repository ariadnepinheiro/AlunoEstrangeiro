using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;


namespace Techne.Lyceum.RN.AvaliacaoExterna
{
    public class Resposta
    {
        public int ObtemIdPor(DataContext contexto, string aluno, int questaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT RESPOSTAID
                                        FROM [AvaliacaoExterna].[RESPOSTA] (NOLOCK)
                                        WHERE ALUNO = @ALUNO
	                                        AND QUESTAOID = @QUESTAOID ";

                contextQuery.Parameters.Add("@QUESTAOID", SqlDbType.Int, questaoId);
                contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["RESPOSTAID"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public int ObtemQuantidadePor(int questaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) AS TOTAL
                                        FROM [AvaliacaoExterna].[RESPOSTA] (NOLOCK)
                                        WHERE QUESTAOID = @QUESTAOID  ";

                contextQuery.Parameters.Add("@QUESTAOID", SqlDbType.Int, questaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TOTAL"]);
                }

                return retorno;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public bool PossuiQuestaoPor(DataContext contexto, int questaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM [AvaliacaoExterna].[RESPOSTA] (NOLOCK)
                                        WHERE QUESTAOID = @QUESTAOID ";

            contextQuery.Parameters.Add("@QUESTAOID", SqlDbType.Int, questaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public IList<Entidades.Resposta> CarregaRespostas(DataContext ctx, int provaId, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT DISTINCT R.*
                                        FROM AVALIACAOEXTERNA.RESPOSTA R 
	                                        INNER JOIN AVALIACAOEXTERNA.QUESTAO Q ON R.QUESTAOID = Q.QUESTAOID
                                        WHERE Q.PROVAID = @PROVAID
	                                        AND ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, aluno);

            return ctx.GetDataTable(contextQuery).ToList<Entidades.Resposta>();
        }

        public void SalvaResposta(DataContext ctx, Entidades.Resposta resposta)
        {
            //Insere novas respostas
           this.Insere(ctx, resposta);
                      
        }

        public void RemoveQuestao(DataContext ctx, int questaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE AvaliacaoExterna.RESPOSTA 
                                          WHERE QUESTAOID = @QUESTAOID ";

            contextQuery.Parameters.Add("@QUESTAOID", SqlDbType.Int, questaoId);

            ctx.ApplyModifications(contextQuery);
        }

        private void Insere(DataContext contexto, Entidades.Resposta resposta)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO AvaliacaoExterna.RESPOSTA
                                               (ALUNO
                                               ,QUESTAOID
                                               ,RESPOSTA
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@ALUNO, 
                                               @QUESTAOID, 
                                               @RESPOSTA, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO)

                                    SELECT IDENT_CURRENT('AvaliacaoExterna.RESPOSTA') ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, resposta.Aluno);
            contextQuery.Parameters.Add("@QUESTAOID", SqlDbType.Int, resposta.QuestaoId);
            contextQuery.Parameters.Add("@RESPOSTA", SqlDbType.Int, resposta.resposta);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, resposta.UsuarioID);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            resposta.RespostaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void Atualiza(DataContext contexto, Entidades.Resposta resposta)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE AvaliacaoExterna.RESPOSTA 
                                        SET RESPOSTA = @RESPOSTA,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        WHERE RESPOSTAID = @RESPOSTAID ";

            contextQuery.Parameters.Add("@RESPOSTAID", SqlDbType.Int, resposta.RespostaId);
            contextQuery.Parameters.Add("@RESPOSTA", SqlDbType.Int, resposta.resposta);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, resposta.UsuarioID);            
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}