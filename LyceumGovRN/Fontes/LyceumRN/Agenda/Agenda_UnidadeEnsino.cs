using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Agenda
{
    public class Agenda_UnidadeEnsino : RNBase
    {
        public static DataTable ListaAgenda_UnidadeEnsino()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT AGENDA_UNIDADEENSINO_ID,
                                                 AGENDAID               ,
                                                 UNIDADEENSINOID        
                                            FROM Agenda.AGENDA_UNIDADEENSINO
                                           ORDER BY AGENDA_UNIDADEENSINO_ID ";

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

        public static DataTable ListaAgenda_UnidadeEnsinoPorAgenda(int AgendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT AGENDA_UNIDADEENSINO_ID,
                                                 AGENDAID               ,
                                                 UNIDADEENSINOID        
                                            FROM Agenda.AGENDA_UNIDADEENSINO
                                           WHERE AGENDAID = @AGENDAID
                                           ORDER BY AGENDA_UNIDADEENSINO_ID ";

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

        public static DataTable ListaAgenda_UnidadeEnsinoPorAgendaEParticipacao(int AgendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT U.UNIDADE_ENS, U.NOME_COMP
                                            FROM LY_UNIDADE_ENSINO U 
                                           WHERE EXISTS(
                                                          SELECT 1
		                                                    FROM Agenda.AGENDA a
  			                                               WHERE a.AGENDAID = @AGENDAID
			                                                 AND (CASE WHEN a.PARTICIPAUNIDADEID = 0                                     
			                                                                THEN 1
			                                                           WHEN a.PARTICIPAUNIDADEID = 1
			                                                            AND EXISTS(SELECT 1
			                                                                         FROM Agenda.AGENDA_UNIDADEENSINO au
			                                                                        WHERE au.AGENDAID                  = a.AGENDAID
			                                                                          AND U.UNIDADE_ENS                = au.UNIDADEENSINOID)     
			                                                                THEN 1
			                                                           WHEN a.PARTICIPAUNIDADEID = 2
			                                                            AND NOT EXISTS(SELECT 1
			                                                                             FROM Agenda.AGENDA_UNIDADEENSINO au
			                                                                            WHERE au.AGENDAID                  = a.AGENDAID
			                                                                              AND U.UNIDADE_ENS                = au.UNIDADEENSINOID) 
			                                                                THEN 1
			                                                      END
			                                                     ) = 1
			                                           ) ";

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

        public bool EhUnidadeParticipantePor(int agendaId, string unidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool participa = false;

            try
            {
                contextQuery.Command = @" SELECT  CASE WHEN a.PARTICIPAUNIDADEID = 0 THEN 1
                                             WHEN a.PARTICIPAUNIDADEID = 1
                                                  AND EXISTS ( SELECT   1
                                                               FROM     Agenda.AGENDA_UNIDADEENSINO au
                                                               WHERE    au.AGENDAID = a.AGENDAID
                                                                        AND au.UNIDADEENSINOID = @UNIDADEENSINOID )
                                                  THEN 1
                                             WHEN a.PARTICIPAUNIDADEID = 2
                                                  AND NOT EXISTS ( SELECT   1
                                                                   FROM     Agenda.AGENDA_UNIDADEENSINO au
                                                                   WHERE    au.AGENDAID = a.AGENDAID
                                                                            AND au.UNIDADEENSINOID = @UNIDADEENSINOID )
                                                  THEN 1
                                             ELSE 0
                                        END PARTICIPA
                                FROM    Agenda.AGENDA a
                                WHERE   a.AGENDAID = @AGENDAID ";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsino);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    participa = true;
                }

                return participa;
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

        public static void InsereAgenda_UnidadeEnsino(Entidades.Agenda_UnidadeEnsino Agenda_UnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO Agenda.AGENDA_UNIDADEENSINO(
                                        AGENDAID        ,
                                        UNIDADEENSINOID
                                    ) VALUES ( @AGENDAID        ,
                                               @UNIDADEENSINOID
                                             )"
                };

                contextQuery.Parameters.Add("@AGENDAID", Agenda_UnidadeEnsino.AgendaId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", Agenda_UnidadeEnsino.UnidadeEnsinoId.Trim());

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

        public static void AlteraAgenda_UnidadeEnsino(Entidades.Agenda_UnidadeEnsino Agenda_UnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"UPDATE Agenda.AGENDA_UNIDADEENSINO
                                   SET AGENDAID        = @AGENDAID       ,
                                       UNIDADEENSINOID = @UNIDADEENSINOID
                                 WHERE AGENDA_UNIDADEENSINO_ID = @AGENDA_UNIDADEENSINO_ID "
                };

                contextQuery.Parameters.Add("@AGENDA_UNIDADEENSINO_ID", Agenda_UnidadeEnsino.Agenda_UnidadeEnsino_Id);
                contextQuery.Parameters.Add("@AGENDAID", Agenda_UnidadeEnsino.AgendaId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", Agenda_UnidadeEnsino.UnidadeEnsinoId.Trim());

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

        public static void RemoveAgenda_UnidadeEnsino(int Agenda_UnidadeEnsino_Id)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE Agenda.AGENDA_UNIDADEENSINO
                                  WHERE AGENDA_UNIDADEENSINO_ID = @AGENDA_UNIDADEENSINO_ID "
                };

                contextQuery.Parameters.Add("@AGENDA_UNIDADEENSINO_ID", Agenda_UnidadeEnsino_Id);

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

    }
}
