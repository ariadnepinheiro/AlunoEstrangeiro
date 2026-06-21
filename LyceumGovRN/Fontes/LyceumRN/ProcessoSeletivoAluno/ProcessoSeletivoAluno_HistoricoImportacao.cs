using System;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno
{
    public class ProcessoSeletivo_HistoricoImportacao : RNBase
    {
        public DataTable ListarPorAgendaId(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT H.[HISTORICOIMPORTACAOID],
                                           H.[DATAIMPORTACAO],
                                           H.[NOMEARQUIVO],
                                           H.[STATUSPROCESSAMENTO],
                                           H.[TIPOENTRADASISTEMAID],
                                           H.[TOTALREGISTROIMPORTADO],
                                           PH.[NUMEROCHAMADA]
                                          FROM [LYCEUM].[dbo].[HISTORICOIMPORTACAO] H WITH (NOLOCK)
                                         INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[PROCESSOSELETIVO_HISTORICOIMPORTACAO] PH
                                            ON H.HISTORICOIMPORTACAOID = PH.HISTORICOIMPORTACAOID
                                         INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] PS
                                            ON PH.PROCESSOSELETIVOID = PS.PROCESSOSELETIVOID
                                         WHERE PS.AGENDAID = @AGENDAID
                                         ORDER BY H.[DATAIMPORTACAO] DESC";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

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
        /// REF7. Query para Inclusão do Relacionamento entre Histórico de Importação e Processo Seletivo
        /// </summary>
        /// <param name="ProcessoSeletivo_HistoricoImportacao"></param>
        /// <param name="contexto"></param>
        public int InsereProcessoSeletivo_HistoricoImportacao(Entidades.ProcessoSeletivo_HistoricoImportacao processoSeletivo_HistoricoImportacao, DataContext contexto)
        {
            string strSQL = string.Empty;
            int idProcessoSeletivo_HistoricoImportacao = 0;
            ContextQuery contextQuery = null;

            try
            {
                contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO [LYCEUM].[ProcessoSeletivoAluno].[PROCESSOSELETIVO_HISTORICOIMPORTACAO]
                                (
                                HISTORICOIMPORTACAOID,
                                PROCESSOSELETIVOID,
                                NUMEROCHAMADA
                                )
                                VALUES
                                (
                                @HISTORICOIMPORTACAOID,
                                (SELECT PS.PROCESSOSELETIVOID
                                FROM [Agenda].[PROCESSOSELETIVO] PS
                                WHERE PS.AGENDAID = @AGENDAID),
                                ISNULL(
                                (SELECT MAX(NUMEROCHAMADA) + 1 as NUMEROCHAMADA
                                FROM [LYCEUM].[PROCESSOSELETIVOALUNO].[PROCESSOSELETIVO_HISTORICOIMPORTACAO] PHN
                                INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] PRS
                                ON PHN.PROCESSOSELETIVOID = PRS.PROCESSOSELETIVOID
                                INNER JOIN [LYCEUM].[PROCESSOSELETIVOALUNO].[HISTORICOGERACAOPREMATRICULA] HN
                                ON PHN.PROCESSOSELETIVO_HISTORICOIMPORTACAOID = HN.PROCESSOSELETIVO_HISTORICOIMPORTACAOID
                                WHERE PRS.AGENDAID = @AGENDAID),1))"
                };

                contextQuery.Parameters.Add("@AGENDAID", processoSeletivo_HistoricoImportacao.ProcessoSeletivoId);
                contextQuery.Parameters.Add("@HISTORICOIMPORTACAOID", processoSeletivo_HistoricoImportacao.HistoricoImportacaoId);

                contexto.ApplyModifications(contextQuery);

                //Retorna o último Id do Histórico da Importação
                strSQL = @"SELECT MAX(PROCESSOSELETIVO_HISTORICOIMPORTACAOID) 
                             FROM [LYCEUM].[PROCESSOSELETIVOALUNO].[PROCESSOSELETIVO_HISTORICOIMPORTACAO]
                            WHERE HISTORICOIMPORTACAOID = @HISTORICOIMPORTACAOID";

                //contextQuery.Parameters.Remove(

                object id = contexto.GetReturnValue(new ContextQuery(strSQL, contextQuery.Parameters));

                if (id != null && !(id == DBNull.Value))
                    idProcessoSeletivo_HistoricoImportacao = Convert.ToInt32(id);

            }
            catch (Exception ex)
            {
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }

            return idProcessoSeletivo_HistoricoImportacao;
        }

        public void RemoveProcessoSeletivo_HistoricoImportacao(int historicoImportacaoId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                contextQuery = new ContextQuery
                {
                    Command = @" DELETE [ProcessoSeletivoAluno].[PROCESSOSELETIVO_HISTORICOIMPORTACAO]
                                  WHERE HISTORICOIMPORTACAOID = @HISTORICOIMPORTACAOID"
                };

                contextQuery.Parameters.Add("@HISTORICOIMPORTACAOID", historicoImportacaoId);

                contexto.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                if (contexto != null)
                    contexto.Abandon();

                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }
        }

        public Entidades.ProcessoSeletivo_HistoricoImportacao CriaEntidadeProcessoSeletivo_HistoricoImportacao(int agendaId, int historicoImportacaoId)
        {
            ProcessoSeletivoAluno.Entidades.ProcessoSeletivo_HistoricoImportacao processoSeletivo__HistoricoImportacao = new Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades.ProcessoSeletivo_HistoricoImportacao();
            processoSeletivo__HistoricoImportacao.ProcessoSeletivoId = agendaId;
            processoSeletivo__HistoricoImportacao.HistoricoImportacaoId = historicoImportacaoId;
            return processoSeletivo__HistoricoImportacao;
        }

        /// <summary>
        /// REF14. Query para Verificação se Existe Alguma Importação dos Candidatos Classificados com a Situação “Concluído”
        /// </summary>
        /// <param name="agendaId"></param>
        /// <param name="tipoImportacaoId"></param>
        /// <returns></returns>
        public bool VerificaImportacaoConcluidaPorAgendaETipoImportacao(int agendaId, int tipoImportacaoId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            bool importacaoConcluida = false;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT DISTINCT 1
                                         FROM [LYCEUM].[PROCESSOSELETIVOALUNO].[PROCESSOSELETIVO_HISTORICOIMPORTACAO] PH
                                         INNER JOIN [LYCEUM].[AGENDA].[PROCESSOSELETIVO] PS
                                            ON PS.PROCESSOSELETIVOID = PH.PROCESSOSELETIVOID
                                         INNER JOIN [LYCEUM].[dbo].[HISTORICOIMPORTACAO] H
                                            ON PH.HISTORICOIMPORTACAOID = H.HISTORICOIMPORTACAOID
                                          LEFT OUTER JOIN
                                             (
	                                           SELECT MAX(NUMEROCHAMADA) + 1 as NUMEROCHAMADA, PHN.PROCESSOSELETIVOID
                                                   FROM [LYCEUM].[PROCESSOSELETIVOALUNO].[PROCESSOSELETIVO_HISTORICOIMPORTACAO] PHN
                                                  INNER JOIN [LYCEUM].[PROCESSOSELETIVOALUNO].[HISTORICOGERACAOPREMATRICULA] HN
                                                     ON PHN.PROCESSOSELETIVO_HISTORICOIMPORTACAOID = HN.PROCESSOSELETIVO_HISTORICOIMPORTACAOID
                                                  GROUP BY PHN.PROCESSOSELETIVOID
                                             ) MH ON MH.PROCESSOSELETIVOID = PS.PROCESSOSELETIVOID
                                         WHERE PS.AGENDAID =@AGENDAID
                                           AND H.STATUSPROCESSAMENTO = @STATUSPROCESSAMENTO
                                           AND H.TIPOIMPORTACAOID = @TIPOIMPORTACAOID
                                           AND PH.NUMEROCHAMADA = ISNULL(MH.NUMEROCHAMADA , 1)";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@STATUSPROCESSAMENTO", (int)RN.HistoricoImportacao.StatusProcessamento.Concluido);
                contextQuery.Parameters.Add("@TIPOIMPORTACAOID", tipoImportacaoId);

                object retorno = contexto.GetReturnValue(contextQuery);

                if (retorno != null && !(retorno == DBNull.Value) && retorno.ToString() == "1")
                    importacaoConcluida = true;
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

            return importacaoConcluida;
        }

        /// <summary>
        ///REF16. Query para Enumeração do Histórico de Geração de Pré-Matrícula
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataTable ListaHistoricoGeracaoPreMatricula(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT H.[HISTORICOGERACAOPREMATRICULAID],
                                                H.[DATAGERACAO],
                                                PH.[NUMEROCHAMADA],
                                                COUNT(*) AS TOTALPREMATRICULAGERADA
                                           FROM [LYCEUM].[ProcessoSeletivoAluno].[HISTORICOGERACAOPREMATRICULA] H
                                          INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[PROCESSOSELETIVO_HISTORICOIMPORTACAO] PH
                                             ON H.PROCESSOSELETIVO_HISTORICOIMPORTACAOID = PH.PROCESSOSELETIVO_HISTORICOIMPORTACAOID
                                          INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] PS
                                             ON PH.PROCESSOSELETIVOID = PS.PROCESSOSELETIVOID
                                          INNER JOIN [LYCEUM].[ProcessoSeletivoAluno].[CANDIDATOINSCRITOCLASSIFICADO] C
                                             ON PH.PROCESSOSELETIVO_HISTORICOIMPORTACAOID = C.PROCESSOSELETIVO_HISTORICOIMPORTACAOID
                                          WHERE PS.[AGENDAID] = @AGENDAID
                                          GROUP BY H.[HISTORICOGERACAOPREMATRICULAID],
                                                   H.[DATAGERACAO],
                                                   PH.[NUMEROCHAMADA]";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

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
