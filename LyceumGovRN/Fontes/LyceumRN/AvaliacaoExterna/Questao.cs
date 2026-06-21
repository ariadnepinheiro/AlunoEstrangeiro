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
    public class Questao
    {
        public bool PossuiHabilidadePor(int habilidadeId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" SELECT COUNT(0) 
                                FROM [AvaliacaoExterna].[QUESTAO] (NOLOCK)
                                WHERE HABILIDADEID = @HABILIDADEID ";

                contextQuery.Parameters.Add("@HABILIDADEID", SqlDbType.Int, habilidadeId);

                return ctx.GetReturnValue<int>(contextQuery) > 0;
            }
            catch (Exception ex)
            {
                if (ctx != null)
                {
                    ctx.Abandon();
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ctx != null)
                {
                    ctx.Dispose();
                }
            }
        }

        public ICollection<Entidades.Questao> ListaPorQuestaoId(IList<int> listaQuestaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ICollection<Entidades.Questao> matGrades = new List<Entidades.Questao>();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                var questoes = string.Join(", ", listaQuestaoId.Select(s => s.ToString()).ToArray());

                contextQuery.Command = @"  SELECT *
                                           FROM AvaliacaoExterna.QUESTAO q
                                           WHERE q.QUESTAOID in (" + questoes + ")";

                matGrades = contexto.TryToBindEntities<Entidades.Questao>(contextQuery);

                return matGrades;
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

        public DataTable ListaPorProva(int provaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"   SELECT 
                                            Q.QUESTAOID,
                                            Q.PROVAID,
											P.DESCRICAO AS PROVA,
                                            C.COMPONENTEID,
                                            Q.HABILIDADEID,
                                            Q.NUMERO,
                                            C.DESCRICAO, 
                                            H.CODIGO + ' - ' + H.DESCRICAO AS HABILIDADE, 
                                            Q.INDICEDIFICULDADE,
                                            CASE
	                                            WHEN Q.INDICEDIFICULDADE <= 0.1 then 'Muito Difícil'
	                                            WHEN Q.INDICEDIFICULDADE > 0.1 and q.INDICEDIFICULDADE <= 0.3 then 'Difícil'
	                                            WHEN Q.INDICEDIFICULDADE > 0.3 and q.INDICEDIFICULDADE <= 0.7 then 'Mediano'
	                                            WHEN Q.INDICEDIFICULDADE > 0.7 and q.INDICEDIFICULDADE <= 0.9 then 'Fácil'
	                                            WHEN Q.INDICEDIFICULDADE > 0.9 then 'Muito Fácil'
                                            END AS NIVELDIFICULDADE,
                                            Q.QUANTIDADEALTERNATIVAS,
                                            Q.ALTERNATIVACORRETA
						                    FROM AVALIACAOEXTERNA.QUESTAO Q (NOLOCK) 
							                    INNER JOIN AVALIACAOEXTERNA.HABILIDADE H (NOLOCK) 
													                    ON H.HABILIDADEID = Q.HABILIDADEID
							                    INNER JOIN AVALIACAOEXTERNA.COMPONENTE C (NOLOCK)
													                    ON C.COMPONENTEID = H.COMPONENTEID
												inner join AVALIACAOEXTERNA.prova p (nolock)
																		on p.provaid = q.PROVAID
                                            WHERE q.PROVAID = @PROVAID ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

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

        public DataTable ListaPorComponente(int componenteId, int provaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT 
                        q.QUESTAOID,
						q.PROVAID,
                        c.COMPONENTEID,
                        q.HABILIDADEID,
                        q.NUMERO,
                        c.DESCRICAO, 
                        h.CODIGO + ' - ' + h.DESCRICAO AS HABILIDADE, 
                        q.INDICEDIFICULDADE,
                        case
	                        when q.INDICEDIFICULDADE <= 0.1 then 'Muito Difícil'
	                        when q.INDICEDIFICULDADE > 0.1 and q.INDICEDIFICULDADE <= 0.3 then 'Difícil'
	                        when q.INDICEDIFICULDADE > 0.3 and q.INDICEDIFICULDADE <= 0.7 then 'Mediano'
	                        when q.INDICEDIFICULDADE > 0.7 and q.INDICEDIFICULDADE <= 0.9 then 'Fácil'
	                        when q.INDICEDIFICULDADE > 0.9 then 'Muito Fácil'
                        end as NIVELDIFICULDADE,
                        q.QUANTIDADEALTERNATIVAS,
                        q.ALTERNATIVACORRETA
                        FROM AvaliacaoExterna.QUESTAO q
                        inner join AvaliacaoExterna.HABILIDADE h on h.HABILIDADEID = q.HABILIDADEID
						inner join AvaliacaoExterna.COMPONENTE c on c.COMPONENTEID = h.COMPONENTEID
                        WHERE c.COMPONENTEID = @COMPONENTEID
							  and q.PROVAID = @PROVAID ";

                contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, componenteId);
                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

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

        public void Insere(Entidades.Questao questao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            int numero = 0;
            try
            {
                numero = this.IncrementaNumero(ctx, questao.ProvaId);

                contextQuery.Command = @" INSERT INTO AvaliacaoExterna.QUESTAO
                                               (PROVAID
                                               ,HABILIDADEID
                                               ,NUMERO
                                               ,INDICEDIFICULDADE
                                               ,QUANTIDADEALTERNATIVAS
                                               ,ALTERNATIVACORRETA
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO)
                                         VALUES
                                               (@PROVAID, 
                                               @HABILIDADEID, 
                                               @NUMERO, 
                                               @INDICEDIFICULDADE,
                                               @QUANTIDADEALTERNATIVAS, 
                                               @ALTERNATIVACORRETA, 
                                               @USUARIOID, 
                                               @DATACADASTRO, 
                                               @DATAALTERACAO)

                                          SELECT IDENT_CURRENT('AvaliacaoExterna.QUESTAO') ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, questao.ProvaId);
                contextQuery.Parameters.Add("@HABILIDADEID", SqlDbType.Int, questao.HabilidadeId);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.Int, numero);
                contextQuery.Parameters.Add("@INDICEDIFICULDADE", SqlDbType.Decimal, questao.IndiceDificuldade);
                contextQuery.Parameters.Add("@QUANTIDADEALTERNATIVAS", SqlDbType.Int, questao.QuantidadeAlternativas);
                contextQuery.Parameters.Add("@ALTERNATIVACORRETA", SqlDbType.Int, questao.AlternativaCorreta);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, questao.UsuarioID);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                questao.QuestaoId = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
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

        public void Atualiza(Entidades.Questao questao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE AvaliacaoExterna.QUESTAO
                                           SET HABILIDADEID = @HABILIDADEID, 
                                              NUMERO = @NUMERO, 
                                              INDICEDIFICULDADE = @INDICEDIFICULDADE, 
                                              QUANTIDADEALTERNATIVAS = @QUANTIDADEALTERNATIVAS,
                                              ALTERNATIVACORRETA = @ALTERNATIVACORRETA, 
                                              USUARIOID = @USUARIOID, 
                                              DATAALTERACAO = @DATAALTERACAO
                                         WHERE QUESTAOID = @QUESTAOID ";

                contextQuery.Parameters.Add("@QUESTAOID", SqlDbType.Int, questao.QuestaoId);
                contextQuery.Parameters.Add("@HABILIDADEID", SqlDbType.Int, questao.HabilidadeId);
                contextQuery.Parameters.Add("@NUMERO", SqlDbType.Int, questao.Numero);
                contextQuery.Parameters.Add("@INDICEDIFICULDADE", SqlDbType.Decimal, questao.IndiceDificuldade);
                contextQuery.Parameters.Add("@QUANTIDADEALTERNATIVAS", SqlDbType.Int, questao.QuantidadeAlternativas);
                contextQuery.Parameters.Add("@ALTERNATIVACORRETA", SqlDbType.Int, questao.AlternativaCorreta);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, questao.UsuarioID);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                ctx.ApplyModifications(contextQuery);
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

        public ValidacaoDados ValidaRemocao(int questaoId)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (questaoId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
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

        public void Remove(int questaoId)
        {
            RN.AvaliacaoExterna.Resposta rnResposta = new Resposta();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Remove respostas
                rnResposta.RemoveQuestao(ctx, questaoId);

                //Remove questao
                this.Remove(ctx, questaoId);
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

        private void Remove(DataContext ctx, int questaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE AvaliacaoExterna.QUESTAO 
                                          WHERE QUESTAOID = @QUESTAOID ";

            contextQuery.Parameters.Add("@QUESTAOID", SqlDbType.Int, questaoId);

            ctx.ApplyModifications(contextQuery);
        }

        public ValidacaoDados Valida(Entidades.Questao questao, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados();
            DataContext contexto = null;

            if (questao == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {
                if (questao.QuestaoId <= 0)
                {
                    mensagens.Add("Campo CÓDIGO é obrigatório.");
                }

                //Será automatico para cadastro
                if (questao.Numero <= 0)
                {
                    mensagens.Add("Campo NÚMERO é obrigatório.");
                }
            }

            if (questao.ProvaId <= 0)
            {
                mensagens.Add("Campo PROVA é obrigatório.");
            }

            if (questao.HabilidadeId <= 0)
            {
                mensagens.Add("Campo HABILIDADE é obrigatório.");
            }


            if (questao.IndiceDificuldade < 0 || questao.IndiceDificuldade > 1)
            {
                mensagens.Add("Campo INDICE DE DIFICULDADE deve estar entre 0 (muito difícil) e 1 (muito fácil).");
            }

            if (questao.QuantidadeAlternativas < 2 || questao.QuantidadeAlternativas > 5)
            {
                mensagens.Add("A QUANTIDADE DE ALTERNATIVAS deve estar entre 2 e 5.");
            }

            if (questao.AlternativaCorreta < 1 || questao.AlternativaCorreta > 5)
            {
                mensagens.Add("Campo GABARITO pode ser somente uma das letras: A, B, C, D ou E.");
            }

            if (questao.AlternativaCorreta > questao.QuantidadeAlternativas)
            {
                mensagens.Add("Campo GABARITO não pode ser definido com uma letra além da QUANTIDADE DE ALTERNATIVAS");
            }

            if (questao.UsuarioID.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Caso seja um cadastro, verifica se a quantidade permitida de questoes da prova foi atingida
                    if (cadastro && this.AtingiuQtdMaxQuestoes(contexto, questao.ProvaId))
                    {
                        mensagens.Add("Foi atingida a quantidade máxima de questões que a prova permite.");
                    }

                    //Será automatico para cadastro
                    if (!cadastro)
                    {
                        //Verifica se o numero já foi usado para outra questao na mesma prova
                        if (this.NumeroQuestaoJaExisteNestaEtapa(contexto, questao.QuestaoId, questao.Numero, questao.ProvaId))
                        {
                            mensagens.Add("Campo NÚMERO foi definido com um número que já existe cadastrado para esta prova.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private int IncrementaNumero(DataContext contexto, int provaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT ISNULL(MAX(NUMERO), 0) + 1 
                                      FROM AVALIACAOEXTERNA.QUESTAO (NOLOCK)
                                      WHERE PROVAID = @PROVAID ";

            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            return contexto.GetReturnValue<int>(contextQuery);
        }

        private bool NumeroQuestaoJaExisteNestaEtapa(DataContext contexto, int questaoId, int numero, int provaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(0) 
                                        FROM AVALIACAOEXTERNA.QUESTAO (NOLOCK)
                                        WHERE PROVAID = @PROVAID
	                                        AND NUMERO = @NUMERO
	                                        AND QUESTAOID <> @QUESTAOID ";

            contextQuery.Parameters.Add("@QUESTAOID", SqlDbType.Int, questaoId);
            contextQuery.Parameters.Add("@NUMERO", SqlDbType.Int, numero);
            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            return contexto.GetReturnValue<int>(contextQuery) > 0;
        }

        private bool AtingiuQtdMaxQuestoes(DataContext contexto, int provaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  SELECT 
                                    CASE 
	                                    WHEN COUNT(0) >= QUANTIDADEQUESTOES THEN 1
	                                    ELSE 0
                                    END AS EXTRAPOLOUQTDMAX
                                    FROM AVALIACAOEXTERNA.QUESTAO Q
	                                    INNER JOIN AVALIACAOEXTERNA.PROVA P ON Q.PROVAID = P.PROVAID
                                    WHERE Q.PROVAID = @PROVAID
                                    GROUP BY Q.PROVAID, QUANTIDADEQUESTOES ";

            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            int? returnValue = contexto.GetReturnValue<int?>(contextQuery);
            if (!returnValue.HasValue)
            {
                returnValue = 0;
            }

            return returnValue.Value == 1;
        }

        public bool ExisteQuestaoPor(int componenteId, int provaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(0)
                                    FROM AVALIACAOEXTERNA.QUESTAO Q (NOLOCK)
                                    INNER JOIN AVALIACAOEXTERNA.HABILIDADE H (NOLOCK)
	                                    ON Q.HABILIDADEID = H.HABILIDADEID
                                    INNER JOIN AVALIACAOEXTERNA.COMPONENTE C (NOLOCK)
	                                    ON H.COMPONENTEID = C.COMPONENTEID
                                    WHERE Q.PROVAID = @PROVAID
	                                    AND C.COMPONENTEID = @COMPONENTEID ";

                contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);
                contextQuery.Parameters.Add("@COMPONENTEID", SqlDbType.Int, componenteId);


                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
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
                contexto.Dispose();
            }
        }

        public int RetornaConponentePor(int questaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.RetornaConponentePor(contexto, questaoId);
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

        public int RetornaConponentePor(DataContext contexto, int questaoId)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT COMPONENTEID
                            FROM AVALIACAOEXTERNA.QUESTAO Q	
	                            INNER JOIN AVALIACAOEXTERNA.HABILIDADE H 
			                            ON Q.HABILIDADEID = H.HABILIDADEID
                            WHERE Q.QUESTAOID = @QUESTAOID ";

                contextQuery.Parameters.Add("@QUESTAOID", SqlDbType.Int, questaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["COMPONENTEID"]);
                }

                return retorno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public int RetornaProvaIdPor(DataContext contexto, int questaoId)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT PROVAID
                            FROM AVALIACAOEXTERNA.QUESTAO
                            WHERE QUESTAOID = @QUESTAOID ";

                contextQuery.Parameters.Add("@QUESTAOID", SqlDbType.Int, questaoId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["PROVAID"]);
                }

                return retorno;
            }

            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool PossuiProvaPor(DataContext contexto, int provaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM AvaliacaoExterna.QUESTAO (NOLOCK)
                                    WHERE PROVAID = @PROVAID ";

            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public IList<Entidades.Questao> CarregaQuestoes(DataContext ctx, int provaId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"SELECT * 
                                    FROM AVALIACAOEXTERNA.QUESTAO 
                                    WHERE PROVAID = @PROVAID ";

            contextQuery.Parameters.Add("@PROVAID", SqlDbType.Int, provaId);

            return ctx.GetDataTable(contextQuery).ToList<Entidades.Questao>();
        }
    }
}