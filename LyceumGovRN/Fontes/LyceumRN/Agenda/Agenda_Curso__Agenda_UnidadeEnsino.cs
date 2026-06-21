using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Agenda
{
    public class Agenda_Curso__Agenda_UnidadeEnsino : RNBase
    {
        public static DataTable ListaAgenda_Curso__Agenda_UnidadeEnsino()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT AGENDA_UNIDADEENSINO_ID,
                                                 AGENDA_CURSO_ID        
                                            FROM Agenda.AGENDA_CURSO__AGENDA_UNIDADEENSINO ";

                dt = ctx.GetDataTable(contextQuery);
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

            return dt;
        }

        public DataTable ListaAgenda_Curso__Agenda_UnidadeEnsinoPorAgendaEParticipacao(int tipoEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                                 c.CURSO,
                                                 c.NOME AS NOME_CURSO,
                                                 u.UNIDADE_ENS,
                                                 u.NOME_COMP AS NOME_UNIDADEENSINO,
                                                 s.SERIE,
                                                 s.TURNO
                                            FROM LY_CURSO                   c
                                           INNER JOIN LY_SERIE              s 
                                              ON s.CURSO                    = c.CURSO
                                           INNER JOIN Agenda.AGENDA_CURSO  ac
                                              ON ac.CURSOID                 = c.CURSO
                                             AND ac.SERIE                   = s.SERIE                                        
                                           INNER JOIN Agenda.AGENDA_CURSO__AGENDA_UNIDADEENSINO acau
                                              ON acau.AGENDA_CURSO_ID       = ac.AGENDA_CURSO_ID
                                           INNER JOIN Agenda.AGENDA_UNIDADEENSINO au
                                              ON au.AGENDA_UNIDADEENSINO_ID = acau.AGENDA_UNIDADEENSINO_ID
                                           INNER JOIN LY_UNIDADE_ENSINO     u
                                              ON u.UNIDADE_ENS              = au.UNIDADEENSINOID 
                                           INNER JOIN Agenda.AGENDA         a
                                              ON a.AGENDAID                 = ac.AGENDAID
                                           INNER JOIN agenda.EVENTO E
                                              ON A.AGENDAID         = E.AGENDAID        
                                           WHERE a.CURSOPORUNIDADE = 1
                                             AND E.TIPOEVENTOID        = @TIPOEVENTOID 
                                            AND CAST(getdate() AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE)
                                            AND     CAST(E.DATAFIM AS DATE)";

                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);

                dt = ctx.GetDataTable(contextQuery);
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

            return dt;
        }

        /// <summary>
        /// Enumeração de Cursos por Unidade agrupados por Turno com o Nome do Curso e da Unidade de Ensino
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable ListaCursoPorUnidadePorAgendaAgrupadoPorTurno(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.Command = @"SELECT DISTINCT AUE.[UNIDADEENSINOID],
                                                UE.[NOME_COMP] AS NOMEUNIDADEENSINO,
                                                AC.[CURSOID],
                                                C.[NOME] AS NOMECURSO,
                                                AC.SERIE,
                                                SUBSTRING(TURNOS.TURNOS, 0, LEN(TURNOS.TURNOS)) AS TURNOS
                                           FROM [LYCEUM].[Agenda].[AGENDA_CURSO__AGENDA_UNIDADEENSINO] ACUE
                                          INNER JOIN [LYCEUM].[Agenda].[AGENDA_CURSO] AC
                                             ON ACUE.[AGENDA_CURSO_ID] = AC.[AGENDA_CURSO_ID]
                                          INNER JOIN [LYCEUM].[dbo].[LY_CURSO] C
                                             ON AC.[CURSOID] = C.[CURSO]
                                          INNER JOIN [LYCEUM].[Agenda].[AGENDA_UNIDADEENSINO] AUE
                                             ON ACUE.[AGENDA_UNIDADEENSINO_ID] = AUE.[AGENDA_UNIDADEENSINO_ID]
                                          INNER JOIN [LYCEUM].[dbo].[LY_UNIDADE_ENSINO] UE
                                             ON AUE.[UNIDADEENSINOID] = UE.[UNIDADE_ENS]
                                          INNER JOIN [LYCEUM].[Agenda].[AGENDA] A
                                             ON AC.AGENDAID = A.AGENDAID
                                            AND AUE.AGENDAID = A.AGENDAID
                                          CROSS APPLY (SELECT T.[DESCRICAO] + ', '
                                                         FROM [LYCEUM].[dbo].[LY_TURNO] T
                                                        INNER JOIN [LYCEUM].[Agenda].[AGENDA_CURSO] ACT
                                                           ON T.[TURNO] = ACT.[TURNOID]
                                                        INNER JOIN [LYCEUM].[Agenda].[AGENDA_CURSO__AGENDA_UNIDADEENSINO] ACUET
                                                           ON ACUET.[AGENDA_CURSO_ID] = ACT.[AGENDA_CURSO_ID]
                                                        INNER JOIN [LYCEUM].[Agenda].[AGENDA_UNIDADEENSINO] AUET
                                                           ON ACUET.[AGENDA_UNIDADEENSINO_ID] = AUET.[AGENDA_UNIDADEENSINO_ID]
                                                        WHERE ACT.[CURSOID] = AC.[CURSOID]
                                                          AND ACT.[AGENDAID] = A.[AGENDAID]
                                                          AND ACT.[SERIE] = AC.[SERIE]
                                                          AND AUET.[UNIDADEENSINOID] = AUE.UNIDADEENSINOID
                                                          AND AUET.[AGENDAID] = A.[AGENDAID]
                                                        ORDER BY DESCRICAO
                                                          FOR XML PATH('')) TURNOS (TURNOS)
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

        public bool EhUnidadeCursoTurnoSerieParticipantePor(int agendaId, string unidadeEnsino, string curso, string turno, int serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool participa = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                          FROM    Agenda.AGENDA_CURSO ac
                                                  INNER JOIN Agenda.AGENDA_CURSO__AGENDA_UNIDADEENSINO acau ON acau.AGENDA_CURSO_ID = ac.AGENDA_CURSO_ID
                                                  INNER JOIN Agenda.AGENDA_UNIDADEENSINO au ON au.AGENDA_UNIDADEENSINO_ID = acau.AGENDA_UNIDADEENSINO_ID
                                          WHERE   ac.AGENDAID = @AGENDAID
                                                  AND au.UNIDADEENSINOID = @UNIDADEENSINOID
                                                  AND ac.CURSOID = @CURSOID
                                                  AND ac.TURNOID = @TURNOID
                                                  AND ac.SERIE = @SERIE ";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsino);
                contextQuery.Parameters.Add("@CURSOID", curso);
                contextQuery.Parameters.Add("@TURNOID", turno);
                contextQuery.Parameters.Add("@SERIE", serie);

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

        public bool EhUnidadeCursoParticipantePor(int agendaId, string unidadeEnsino, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool participa = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                          FROM    Agenda.AGENDA_CURSO ac
                                                  INNER JOIN Agenda.AGENDA_CURSO__AGENDA_UNIDADEENSINO acau ON acau.AGENDA_CURSO_ID = ac.AGENDA_CURSO_ID
                                                  INNER JOIN Agenda.AGENDA_UNIDADEENSINO au ON au.AGENDA_UNIDADEENSINO_ID = acau.AGENDA_UNIDADEENSINO_ID
                                          WHERE   ac.AGENDAID = @AGENDAID
                                                  AND au.UNIDADEENSINOID = @UNIDADEENSINOID
                                                  AND ac.CURSOID = @CURSOID ";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", unidadeEnsino);
                contextQuery.Parameters.Add("@CURSOID", curso);

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

        public static void InsereAgenda_Curso__Agenda_UnidadeEnsino(Entidades.Agenda_Curso__Agenda_UnidadeEnsino Agenda_Curso__Agenda_UnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO Agenda.AGENDA_CURSO__AGENDA_UNIDADEENSINO(
                                        AGENDA_UNIDADEENSINO_ID,
                                        AGENDA_CURSO_ID
                                    ) VALUES ( @AGENDA_UNIDADEENSINO_ID,
                                               @AGENDA_CURSO_ID        
                                             )"
                };

                contextQuery.Parameters.Add("@AGENDA_UNIDADEENSINO_ID", Agenda_Curso__Agenda_UnidadeEnsino.Agenda_UnidadeEnsino_Id);
                contextQuery.Parameters.Add("@AGENDA_CURSO_ID", Agenda_Curso__Agenda_UnidadeEnsino.Agenda_Curso_Id);

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

        public static void RemoveAgenda_Curso__Agenda_UnidadeEnsino(Entidades.Agenda_Curso__Agenda_UnidadeEnsino Agenda_Curso__Agenda_UnidadeEnsino)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE Agenda.AGENDA_CURSO__AGENDA_UNIDADEENSINO
                                  WHERE AGENDA_UNIDADEENSINO_ID = @AGENDA_UNIDADEENSINO_ID
                                    AND AGENDA_CURSO_ID         = @AGENDA_CURSO_ID "
                };

                contextQuery.Parameters.Add("@AGENDA_UNIDADEENSINO_ID", Agenda_Curso__Agenda_UnidadeEnsino.Agenda_UnidadeEnsino_Id);
                contextQuery.Parameters.Add("@AGENDA_CURSO_ID", Agenda_Curso__Agenda_UnidadeEnsino.Agenda_Curso_Id);

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

        public DataTable ListaSerieAgenda_Curso__Agenda_UnidadeEnsinoPorAgendaEParticipacao(int tipoEventoId, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT                                               
                                                 s.SERIE
                                              
                                            FROM LY_CURSO                   c
                                           INNER JOIN LY_SERIE              s 
                                              ON s.CURSO                    = c.CURSO
                                           INNER JOIN Agenda.AGENDA_CURSO  ac
                                              ON ac.CURSOID                 = c.CURSO
                                             AND ac.SERIE                   = s.SERIE
                                              AND AC.TURNOID                 = S.TURNO 
                                           INNER JOIN Agenda.AGENDA_CURSO__AGENDA_UNIDADEENSINO acau
                                              ON acau.AGENDA_CURSO_ID       = ac.AGENDA_CURSO_ID
                                           INNER JOIN Agenda.AGENDA_UNIDADEENSINO au
                                              ON au.AGENDA_UNIDADEENSINO_ID = acau.AGENDA_UNIDADEENSINO_ID
                                           INNER JOIN LY_UNIDADE_ENSINO     u
                                              ON u.UNIDADE_ENS              = au.UNIDADEENSINOID 
                                           INNER JOIN Agenda.AGENDA         a
                                              ON a.AGENDAID                 = ac.AGENDAID
                                           INNER JOIN agenda.EVENTO E
                                              ON A.AGENDAID         = E.AGENDAID        
                                           WHERE a.CURSOPORUNIDADE = 1
                                             AND E.TIPOEVENTOID        = @TIPOEVENTOID 
                                             AND AC.CURSOID            = @CURSOID 
                                            AND CAST(getdate() AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE)
                                            AND     CAST(E.DATAFIM AS DATE)";

                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);
                contextQuery.Parameters.Add("@CURSOID", curso);

                dt = ctx.GetDataTable(contextQuery);
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

            return dt;
        }
    }
}
