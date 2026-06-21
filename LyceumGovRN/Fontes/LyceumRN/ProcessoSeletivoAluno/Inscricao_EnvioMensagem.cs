using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno
{
    public class Inscricao_EnvioMensagem
    {
        /// <summary>
        /// Preapara a mensagem de email para envio ao candidato
        /// </summary>
        /// <param name="agendaId"></param>
        /// <param name="USUARIOID"></param>
        /// <returns></returns>
        public int PreparaEmail(int agendaId, string USUARIOID)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            int ret;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"INSERT INTO [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO_ENVIOMENSAGEM]
                                          (
                                              ENVIOMENSAGEMID
                                            , INSCRICAOID
                                            , ENVIADO
                                            , USUARIOID
                                          )
                                          SELECT E.ENVIOMENSAGEMID
                                               , I.INSCRICAOID
                                               , 0 AS ENVIADO
                                               , @USUARIO AS USUARIOID
                                            FROM [LYCEUM].[dbo].[ENVIOMENSAGEM] E
                                           INNER JOIN [LYCEUM].[Agenda].[AGENDA_ENVIOMENSAGEM] AE
                                              ON E.ENVIOMENSAGEMID = AE.ENVIOMENSAGEMID
                                           INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] PS
                                              ON AE.AGENDAID = PS.AGENDAID
                                           INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO] I
                                              ON I.PROCESSOSELETIVOID = PS.PROCESSOSELETIVOID
                                           WHERE I.[SITUACAO] = @SITUACAO
                                             AND AE.AGENDAID = @AGENDAID
                                             AND E.TIPOMENSAGEMID =  @TIPOMENSAGEMID
                                             AND E.TIPOOPERACAOID = @TIPOOPERACAOID
                                             AND E.TIPODESTINATARIOID =  @TIPODESTINATARIOID";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@TIPOMENSAGEMID", 1);
                contextQuery.Parameters.Add("@TIPOOPERACAOID", 2);
                contextQuery.Parameters.Add("@TIPODESTINATARIOID", 2);
                contextQuery.Parameters.Add("@SITUACAO", 1);
                contextQuery.Parameters.Add("@USUARIO", USUARIOID);

                ret = contexto.ApplyModifications(contextQuery);
            }
            catch (Exception exception)
            {
                if (contexto != null)
                    contexto.Abandon();

                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return ret;
        }

        /// <summary>
        /// Lista dados dos destinatários do email
        /// </summary>
        /// <param name="idMensagEnvio"></param>
        /// <returns></returns>
        public DataTable ListaDestinatarioEmail(int idMensagEnvio)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT IE.INSCRICAO_ENVIOMENSAGEM_ID
                                              , C.*
                                              , I.NUMEROINSCRICAO
                                              , UCT.CURSOID
                                              , CU.NOME AS CURSONOME
                                              , UCT.TURNOID
                                              , T.DESCRICAO AS NOMETURNO
                                              , UCT.UNIDADEENSINOID
                                              , UE.NOME_COMP AS NOMEUNIDADEENSINO
                                           FROM [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO_ENVIOMENSAGEM] IE
                                          INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO] I
                                             ON IE.INSCRICAOID = I.INSCRICAOID
                                          INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[UNIDADEENSINO_CURSO_TURNO_INSCRICAO] UCT
                                             ON UCT.INSCRICAOID = I.INSCRICAOID
                                          INNER JOIN [LYCEUM].[dbo].[LY_CURSO] CU
                                             ON UCT.CURSOID = CU.CURSO
                                          INNER JOIN [LYCEUM].[dbo].[LY_TURNO] T
                                             ON UCT.TURNOID = T.TURNO
                                          INNER JOIN [LYCEUM].[dbo].[LY_UNIDADE_ENSINO] UE
                                             ON UCT.UNIDADEENSINOID = UE.UNIDADE_ENS
                                          INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[CANDIDATO] C
                                             ON I.CANDIDATOID = C.CANDIDATOID
                                          WHERE IE.ENVIOMENSAGEMID = @ENVIOMENSAGEMID
                                            AND IE.ENVIADO = @ENVIADO 
                                            AND EMAIL <> '' AND EMAIL IS NOT NULL";

                contextQuery.Parameters.Add("@ENVIOMENSAGEMID", idMensagEnvio);
                contextQuery.Parameters.Add("@ENVIADO", 0);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// Atualiza a situação de envio da mensagem de email para o candidato
        /// </summary>
        /// <param name="INSCRICAO_ENVIOMENSAGEM_ID"></param>
        /// <returns></returns>
        public int AtualizaSituacaoEnvio(int INSCRICAO_ENVIOMENSAGEM_ID)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            int ret;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"UPDATE [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO_ENVIOMENSAGEM]
                                            SET ENVIADO = 1
                                              , DATAENVIO = GETDATE()
                                              , MENSAGEMERRO = ''
                                          WHERE INSCRICAO_ENVIOMENSAGEM_ID = @INSCRICAO_ENVIOMENSAGEM_ID";

                contextQuery.Parameters.Add("@INSCRICAO_ENVIOMENSAGEM_ID", INSCRICAO_ENVIOMENSAGEM_ID);

                ret = contexto.ApplyModifications(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return ret;
        }

        /// <summary>
        /// Produz o log da mensagem de erro de envio da mensagem
        /// </summary>
        /// <param name="INSCRICAO_ENVIOMENSAGEM_ID"></param>
        /// <param name="MENSAGEM"></param>
        /// <returns></returns>
        public int MensagemErroEnvio(int INSCRICAO_ENVIOMENSAGEM_ID, string MENSAGEM)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            int ret;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"UPDATE [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO_ENVIOMENSAGEM]
                                            SET MENSAGEMERRO = @MENSAGEM
                                          WHERE INSCRICAO_ENVIOMENSAGEM_ID = @INSCRICAO_ENVIOMENSAGEM_ID";

                contextQuery.Parameters.Add("@INSCRICAO_ENVIOMENSAGEM_ID", INSCRICAO_ENVIOMENSAGEM_ID);
                contextQuery.Parameters.Add("@MENSAGEM", MENSAGEM);
                ret = contexto.ApplyModifications(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }

            return ret;
        }
    }
}
