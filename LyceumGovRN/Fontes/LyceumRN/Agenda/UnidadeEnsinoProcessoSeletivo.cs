using System;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Data;

namespace Techne.Lyceum.RN.Agenda
{
    public class UnidadeEnsinoProcessoSeletivo : RNBase
    {
        public DataTable ListaPorAgendaId(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT UEP.[UNIDADEENSINOID],
                                                UE.[NOME_COMP] AS NOMEUNIDADEENSINO,
                                                UEP.[MENSAGEM]
                                           FROM [LYCEUM].[Agenda].[UNIDADEENSINO_PROCESSOSELETIVO] UEP
                                          INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] P
                                             ON UEP.PROCESSOSELETIVOID = P.PROCESSOSELETIVOID
                                          INNER JOIN [LYCEUM].[Agenda].[AGENDA] A
                                             ON P.AGENDAID = A.AGENDAID
                                          INNER JOIN [LYCEUM].[dbo].[LY_UNIDADE_ENSINO] UE
                                             ON UEP.UNIDADEENSINOID = UE.UNIDADE_ENS
                                          WHERE A.[AGENDAID] = @AGENDAID";

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

        public static QueryTable ListaUnidadeEnsinoCursoTurnoProcessoSeletivo(Int32 AgendaID)
        {
            string sql = @" SELECT AUE.[UNIDADEENSINOID],
                                   UE.[NOME_COMP] NOMEUNIDADEENSINO,
                                   AC.[CURSOID],
                                   CU.[NOME] NOMECURSO,
                                   AC.[SERIE],
                                   AC.[TURNOID],
                                   TU.[DESCRICAO] NOMETURNO
                              FROM [LYCEUM].[AGENDA].[AGENDA_CURSO__AGENDA_UNIDADEENSINO] AC_AUE
                             INNER JOIN [LYCEUM].[AGENDA].[AGENDA_UNIDADEENSINO] AUE
                                ON AC_AUE.AGENDA_UNIDADEENSINO_ID = AUE.AGENDA_UNIDADEENSINO_ID
                             INNER JOIN [LYCEUM].[DBO].[LY_UNIDADE_ENSINO] UE
                                ON AUE.UNIDADEENSINOID = UE.UNIDADE_ENS
                             INNER JOIN [LYCEUM].[AGENDA].[AGENDA_CURSO] AC
                                ON AC_AUE.AGENDA_CURSO_ID = AC.AGENDA_CURSO_ID
                             INNER JOIN [LYCEUM].[DBO].[LY_CURSO] CU
                                ON AC.CURSOID = CU.CURSO
                             INNER JOIN [LYCEUM].[DBO].[LY_TURNO] TU
                                ON AC.TURNOID = TU.TURNO
                             INNER JOIN [LYCEUM].[AGENDA].[AGENDA] AG
                                ON AG.AGENDAID = AUE.AGENDAID
                               AND AG.AGENDAID = AC.AGENDAID                                      
                             WHERE AG.AGENDAID = ?";

            return RNBase.Consultar(sql, AgendaID);
        }

        public static string MensagemUnidadeEnsino(int unidadeEnsinoId, int processoSeletivoId)
        {
            string mensagemUnidadeEnsino = string.Empty;

            try
            {
                using (DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    ContextQuery contextQuery = new ContextQuery();

                    contextQuery.Command = @" SELECT  UEPS.MENSAGEM
                        FROM    [LYCEUM].[Agenda].[UNIDADEENSINO_PROCESSOSELETIVO] UEPS
                                INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] OS ON UEPS.PROCESSOSELETIVOID = OS.PROCESSOSELETIVOID
                        WHERE   UEPS.UNIDADEENSINOID = @UNIDADEENSINOID
                                AND UEPS.PROCESSOSELETIVOID = @PROCESSOSELETIVOID ";

                    contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsinoId);
                    contextQuery.Parameters.Add("@PROCESSOSELETIVOID", processoSeletivoId);

                    DataTable dt = ctx.GetDataTable(contextQuery);

                    if (dt.Rows.Count > 0)
                        mensagemUnidadeEnsino = dt.Rows[0]["MENSAGEM"].ToString();
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return mensagemUnidadeEnsino;
        }
    }
}
