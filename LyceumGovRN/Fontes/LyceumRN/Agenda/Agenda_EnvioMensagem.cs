using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Agenda
{
    public class Agenda_EnvioMensagem
    {
        /// <summary>
        /// Verifica se há mensagem de email produzida para envio ao candidato de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable VerificaMensagemEmail(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT 1
                                           FROM [LYCEUM].[dbo].[ENVIOMENSAGEM] E
                                          INNER JOIN [LYCEUM].[Agenda].[AGENDA_ENVIOMENSAGEM] AE
                                             ON E.ENVIOMENSAGEMID = AE.ENVIOMENSAGEMID
                                          WHERE AE.AGENDAID = @AGENDAID
                                            AND E.TIPOMENSAGEMID = @TIPOMENSAGEMID
                                            AND E.TIPOOPERACAOID = @TIPOOPERACAOID
                                            AND E.TIPODESTINATARIOID = @TIPODESTINATARIOID ";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@TIPOMENSAGEMID", 1);
                contextQuery.Parameters.Add("@TIPOOPERACAOID", 2);
                contextQuery.Parameters.Add("@TIPODESTINATARIOID", 2);

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
        /// Seleciona os dados do remtente do email
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable SelecionaDadosEmail(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT E.ENVIOMENSAGEMID 
                                              , E.REMETENTE
                                              , E.ASSUNTO
                                              , E.MENSAGEM
                                           FROM [LYCEUM].[dbo].[ENVIOMENSAGEM] E
                                          INNER JOIN [LYCEUM].[Agenda].[AGENDA_ENVIOMENSAGEM] AE
                                             ON E.ENVIOMENSAGEMID = AE.ENVIOMENSAGEMID
                                          WHERE AE.AGENDAID = @AGENDAID
                                            AND E.TIPOMENSAGEMID =  @TIPOMENSAGEMID
                                            AND E.TIPOOPERACAOID = @TIPOOPERACAOID
                                            AND E.TIPODESTINATARIOID =  @TIPODESTINATARIOID";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@TIPOMENSAGEMID", 1);
                contextQuery.Parameters.Add("@TIPOOPERACAOID", 2);
                contextQuery.Parameters.Add("@TIPODESTINATARIOID", 2);

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
    }
}
