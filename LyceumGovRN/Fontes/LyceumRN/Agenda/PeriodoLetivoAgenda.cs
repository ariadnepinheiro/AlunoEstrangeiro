using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Agenda
{
    public class PeriodoLetivoAgenda : RNBase
    {
        public static DataTable ListaPeriodoLetivoAgenda()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOLETIVOAGENDAID,
                                                 AGENDAID             ,
                                                 ANO                  ,
                                                 PERIODO               
                                            FROM Agenda.PERIODOLETIVOAGENDA
                                           ORDER BY PERIODOLETIVOAGENDAID ";

                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        public static DataTable ListaPeriodoLetivoPorAgenda(int AgendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT PERIODOLETIVOAGENDAID,
                                                 AGENDAID             ,
                                                 ANO                  ,
                                                 PERIODO               
                                            FROM Agenda.PERIODOLETIVOAGENDA
                                           WHERE AGENDAID = @AGENDAID
                                           ORDER BY PERIODOLETIVOAGENDAID ";

                contextQuery.Parameters.Add("@AGENDAID", AgendaId);

                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        public static DataTable ListaPeriodoLetivoPorAgendaConcatenado(int AgendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" 
                                        SELECT CONVERT(VARCHAR(4), ano) + '-' 
                                               + CONVERT(VARCHAR(1), periodo) AS ANOPERIODO 
                                        FROM   agenda.periodoletivoagenda 
                                        WHERE  agendaid = @AGENDAID 
                                        ORDER  BY ANOPERIODO";

                contextQuery.Parameters.Add("@AGENDAID", AgendaId);

                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        public static void InserePeriodoLetivoAgenda(Entidades.PeriodoLetivoAgenda PeriodoLetivoAgenda)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO Agenda.PERIODOLETIVOAGENDA(
                                        AGENDAID             ,
                                        ANO                  ,
                                        PERIODO               
                                    ) VALUES ( @AGENDAID ,
                                               @ANO      ,
                                               @PERIODO               
                                             )"
                };

                contextQuery.Parameters.Add("@AGENDAID", PeriodoLetivoAgenda.AgendaId);
                contextQuery.Parameters.Add("@ANO", PeriodoLetivoAgenda.Ano);
                contextQuery.Parameters.Add("@PERIODO", PeriodoLetivoAgenda.Periodo);

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static void AlteraPeriodoLetivoAgenda(Entidades.PeriodoLetivoAgenda PeriodoLetivoAgenda)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"UPDATE Agenda.PERIODOLETIVOAGENDA
                                   SET AGENDAID = @AGENDAID,
	                                   ANO      = @ANO     ,
	                                   PERIODO  = @PERIODO               
                                 WHERE PERIODOLETIVOAGENDAID = @PERIODOLETIVOAGENDAID "
                };

                contextQuery.Parameters.Add("@PERIODOLETIVOAGENDAID", PeriodoLetivoAgenda.PeriodoLetivoAgendaId);
                contextQuery.Parameters.Add("@AGENDAID", PeriodoLetivoAgenda.AgendaId);
                contextQuery.Parameters.Add("@ANO", PeriodoLetivoAgenda.Ano);
                contextQuery.Parameters.Add("@PERIODO", PeriodoLetivoAgenda.Periodo);

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static void RemovePeriodoLetivoAgenda(int PeriodoLetivoAgendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE Agenda.PERIODOLETIVOAGENDA
                                  WHERE PERIODOLETIVOAGENDAID = @PERIODOLETIVOAGENDAID "
                };

                contextQuery.Parameters.Add("@PERIODOLETIVOAGENDAID", PeriodoLetivoAgendaId);

                ctx.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static DataTable ListaAnoPeriodoReferenciaPorAgenda(int AgendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = string.Format(@"
                SELECT R.ano     ANO_REF, 
                       R.periodo PERIODO_REF 
                FROM   agenda.periodoreferencia R 
                       INNER JOIN agenda.periodoletivoagenda P 
                               ON R.periodoletivoagendaid = P.periodoletivoagendaid 
                WHERE  P.agendaid = {0} 
                ORDER BY PERIODO_REF", AgendaId);

                agenda = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return agenda;
        }

        public DataTable ListaAnosPeriodoLetivoAgenda( int tipoEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable anos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                                    p.[ANO]
                                            FROM    [LYCEUM].[Agenda].[PERIODOLETIVOAGENDA] p
                                                    INNER JOIN LYCEUM.Agenda.AGENDA a ON a.AGENDAID = p.AGENDAID
                                                    INNER JOIN LYCEUM.Agenda.EVENTO e ON e.AGENDAID = a.AGENDAID
                                                                                         AND TIPOEVENTOID = @TIPOEVENTOID
                                            ORDER BY ANO DESC";

                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);

                anos = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }

            return anos;
        }


        public DataTable ListaAnoAbertoPor(int idEventoReaberturaMatricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable ano = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                PLA.[ANO]
                        FROM    [LYCEUM].[AGENDA].[PERIODOLETIVOAGENDA] PLA
                                INNER JOIN [LYCEUM].[AGENDA].[EVENTO] EV ON EV.AGENDAID = PLA.AGENDAID
                        WHERE   EV.DATAFIM >= CONVERT(DATE, GETDATE())
                                AND EV.DATAINICIO <= CONVERT(DATE, GETDATE())
                                AND EV.[TIPOEVENTOID] = @TIPOEVENTOID
                        ORDER BY ANO DESC ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", idEventoReaberturaMatricula);

                ano = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return ano;
        }

        public DataTable ListaPeriodoAbertoPor(int idEventoReaberturaMatricula, int ano)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable periodo = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                PLA.[PERIODO]
                        FROM    [LYCEUM].[AGENDA].[PERIODOLETIVOAGENDA] PLA
                                INNER JOIN [LYCEUM].[AGENDA].[EVENTO] EV ON EV.AGENDAID = PLA.AGENDAID
                        WHERE   EV.DATAFIM >= CONVERT(DATE, GETDATE())
                                AND EV.DATAINICIO <= CONVERT(DATE, GETDATE())
                                AND EV.[TIPOEVENTOID] = @TIPOEVENTOID
                                AND PLA.ANO = @ANO
                        ORDER BY PERIODO DESC ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", idEventoReaberturaMatricula);
                contextQuery.Parameters.Add("@ANO", ano);

                periodo = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return periodo;
        }
    }
}
