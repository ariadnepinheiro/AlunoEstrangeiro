using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.Agenda
{
    public class Agenda_Curso : RNBase
    {
        public static DataTable ListaAgenda_Curso()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT CURSOID        ,
                                                 AGENDAID       ,
                                                 AGENDA_CURSO_ID,
                                                 SERIE          ,
                                                 TURNOID        
                                            FROM Agenda.AGENDA_CURSO
                                           ORDER BY AGENDA_CURSO_ID ";

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

        public static DataTable ListaCursoTurnoSeriePorAgenda(int AgendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT CURSOID ,
                                                 TURNOID ,
                                                 SERIE        
                                            FROM Agenda.AGENDA_CURSO
                                           WHERE AGENDAID = @AGENDAID
                                           ORDER BY AGENDA_CURSO_ID ";

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

        public static DataTable ListaCursoTurnoSeriePorAgendaEParticipacao(int AgendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                                 C.CURSO ,
                                                 C.NOME ,
                                                 S.SERIE ,
                                                 S.TURNO
                                            FROM LY_CURSO      c
                                           INNER JOIN LY_SERIE s 
                                              ON s.CURSO       = c.CURSO
                                           WHERE EXISTS ( 
                                                           SELECT 1
                                                             FROM Agenda.AGENDA a
                                                            WHERE a.AGENDAID    = @AGENDAID
                                                              AND ( CASE WHEN a.PARTICIPACURSOID = 0 
                                                                              THEN 1
                                                                         WHEN a.PARTICIPACURSOID = 1
                                                                          AND EXISTS ( 
                                                                                        SELECT 1
                                                                                          FROM Agenda.AGENDA_CURSO ac
                                                                                         WHERE ac.AGENDAID = a.AGENDAID
                                                                                           AND c.CURSO     = ac.CURSOID
                                                                                           AND s.SERIE     = ac.SERIE 
                                                                                           AND S.TURNO     = ac.TURNOID
                                                                                     )
                                                                              THEN 1
                                                                         WHEN a.PARTICIPACURSOID = 2
                                                                          AND NOT EXISTS ( 
                                                                                            SELECT 1
                                                                                              FROM Agenda.AGENDA_CURSO ac
                                                                                             WHERE ac.AGENDAID = a.AGENDAID
                                                                                               AND c.CURSO     = ac.CURSOID
                                                                                               AND S.SERIE     = ac.SERIE
                                                                                               AND S.TURNO     = ac.TURNOID 
                                                                                         )
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

        public bool EhCursoTurnoSerieParticipantePor(int agendaId, string curso, string turno, int serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool participa = false;

            try
            {
                contextQuery.Command = @" SELECT  CASE WHEN a.PARTICIPACURSOID = 0 THEN 1
                                             WHEN a.PARTICIPACURSOID = 1
                                                  AND EXISTS ( SELECT   1
                                                               FROM     Agenda.AGENDA_CURSO ac
                                                               WHERE    ac.AGENDAID = a.AGENDAID
                                                                        AND ac.CURSOID = @CURSOID
                                                                        AND ac.SERIE = @SERIE 
                                                                        AND ac.TURNOID = @TURNOID
                                                                        ) THEN 1
                                             WHEN a.PARTICIPACURSOID = 2
                                                  AND NOT EXISTS ( SELECT   1
                                                                   FROM     Agenda.AGENDA_CURSO ac
                                                                   WHERE    ac.AGENDAID = a.AGENDAID
                                                                            AND ac.CURSOID = @CURSOID
                                                                            AND ac.SERIE = @SERIE 
                                                                            AND ac.TURNOID = @TURNOID
                                                                            ) THEN 1
                                             ELSE 0
                                        END PARTICIPA
                                FROM    Agenda.AGENDA a
                                WHERE   a.AGENDAID = @AGENDAID ";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
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

        public bool EhCursoParticipantePor(int agendaId, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool participa = false;

            try
            {
                contextQuery.Command = @" SELECT  CASE WHEN a.PARTICIPACURSOID = 0 THEN 1
                                             WHEN a.PARTICIPACURSOID = 1
                                                  AND EXISTS ( SELECT   1
                                                               FROM     Agenda.AGENDA_CURSO ac
                                                               WHERE    ac.AGENDAID = a.AGENDAID
                                                                        AND ac.CURSOID = @CURSOID
                                                                        ) THEN 1
                                             WHEN a.PARTICIPACURSOID = 2
                                                  AND NOT EXISTS ( SELECT   1
                                                                   FROM     Agenda.AGENDA_CURSO ac
                                                                   WHERE    ac.AGENDAID = a.AGENDAID
                                                                            AND ac.CURSOID = @CURSOID
                                                                            ) THEN 1
                                             ELSE 0
                                        END PARTICIPA
                                FROM    Agenda.AGENDA a
                                WHERE   a.AGENDAID = @AGENDAID ";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
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

        public static DataTable ListaCursoTurnoSeriePorAgendaEUnidadeEnsino(int AgendaId, int UnidadeEnsinoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT AC.CURSOID ,
                                                 AC.TURNOID ,
                                                 AC.SERIE        
                                            FROM Agenda.AGENDA_CURSO                             AC
                                           INNER JOIN Agenda.AGENDA_CURSO__AGENDA_UNIDADEENSINO ACAU
                                              ON AC.AGENDA_CURSO_ID                              =  ACAU.AGENDA_CURSO_ID
                                           INNER JOIN Agenda.AGENDA_UNIDADEENSINO                AU
                                              ON ACAU.AGENDA_UNIDADEENSINO_ID                    =  AU.AGENDA_UNIDADEENSINO_ID
                                           WHERE AC.AGENDAID        = @AGENDAID
                                             AND AU.UNIDADEENSINOID = @UNIDADEENSINOID
                                           ORDER BY AC.AGENDA_CURSO_ID ";

                contextQuery.Parameters.Add("@AGENDAID", AgendaId);
                contextQuery.Parameters.Add("@UNIDADEENSINOID", UnidadeEnsinoId);

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

        public static void InsereAgenda_Curso(Entidades.Agenda_Curso Agenda_Curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO Agenda.AGENDA_CURSO(
                                        CURSOID        ,
                                        AGENDAID       ,
                                        SERIE          ,
                                        TURNOID
                                    ) VALUES ( @CURSOID        ,
                                               @AGENDAID       ,
                                               @SERIE          ,
                                               @TURNOID
                                             )"
                };

                contextQuery.Parameters.Add("@CURSOID", Agenda_Curso.CursoId.Trim());
                contextQuery.Parameters.Add("@AGENDAID", Agenda_Curso.AgendaId);
                contextQuery.Parameters.Add("@SERIE", Agenda_Curso.Serie.Trim());
                contextQuery.Parameters.Add("@TURNOID", Agenda_Curso.TurnoId.Trim());

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

        public static void AlteraAgenda_Curso(Entidades.Agenda_Curso Agenda_Curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"UPDATE Agenda.AGENDA_CURSO
                                   SET CURSOID  = @CURSOID ,
                                       AGENDAID = @AGENDAID,
                                       SERIE    = @SERIE   ,
                                       TURNOID  = @TURNOID
                                 WHERE AGENDA_CURSO_ID = @AGENDA_CURSO_ID "
                };

                contextQuery.Parameters.Add("@CURSOID", Agenda_Curso.CursoId.Trim());
                contextQuery.Parameters.Add("@AGENDAID", Agenda_Curso.AgendaId);
                contextQuery.Parameters.Add("@AGENDA_CURSO_ID", Agenda_Curso.Agenda_Curso_Id);
                contextQuery.Parameters.Add("@SERIE", Agenda_Curso.Serie.Trim());
                contextQuery.Parameters.Add("@TURNOID", Agenda_Curso.TurnoId.Trim());

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

        public static void RemoveAgenda_Curso(int Agenda_Curso_Id)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE Agenda.AGENDA_CURSO
                                  WHERE AGENDA_CURSO_ID = @AGENDA_CURSO_ID "
                };

                contextQuery.Parameters.Add("@AGENDA_CURSO_ID", Agenda_Curso_Id);

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
        public DataTable ListaCursoPorAgendaEParticipacao(int AgendaId, string unidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                                 C.CURSO ,
                                                 C.NOME                                                 
                                            FROM LY_CURSO      c
                                           INNER JOIN LY_SERIE s 
                                              ON s.CURSO       = c.CURSO
                                           WHERE EXISTS ( 
                                                           SELECT 1
                                                             FROM Agenda.AGENDA a
                                                            WHERE a.AGENDAID    = @AGENDAID
                                                              AND ( CASE WHEN a.PARTICIPACURSOID = 0 
                                                                              THEN 1
                                                                         WHEN a.PARTICIPACURSOID = 1
                                                                          AND EXISTS ( 
                                                                                        SELECT 1
                                                                                          FROM Agenda.AGENDA_CURSO ac
                                                                                         WHERE ac.AGENDAID = a.AGENDAID
                                                                                           AND c.CURSO     = ac.CURSOID
                                                                                           AND s.SERIE     = ac.SERIE 
                                                                                           AND S.TURNO     = ac.TURNOID
                                                                                     )
                                                                              THEN 1
                                                                         WHEN a.PARTICIPACURSOID = 2
                                                                          AND NOT EXISTS ( 
                                                                                            SELECT 1
                                                                                              FROM Agenda.AGENDA_CURSO ac
                                                                                             WHERE ac.AGENDAID = a.AGENDAID
                                                                                               AND c.CURSO     = ac.CURSOID
                                                                                               AND S.SERIE     = ac.SERIE
                                                                                               AND S.TURNO     = ac.TURNOID 
                                                                                         )
                                                                              THEN 1
                                                                    END 
                                                                  ) = 1 
                                                                   AND ( CASE WHEN a.CURSOPORUNIDADE = 0 
                                                                              THEN 1
                                                                         WHEN a.CURSOPORUNIDADE = 1
                                                                          AND EXISTS ( 
                                                                                        SELECT 1
                                                                                        FROM  [LYCEUM].[Agenda].[AGENDA_CURSO] AC
                                                                                        INNER JOIN [AGENDA].[AGENDA_UNIDADEENSINO] UE ON UE.AGENDAID = AC.AGENDAID  
                                                                                        INNER JOIN [Agenda].[AGENDA_CURSO__AGENDA_UNIDADEENSINO] UEA ON UEA.AGENDA_CURSO_ID = AC.AGENDA_CURSO_ID AND UEA.AGENDA_UNIDADEENSINO_ID = UE.AGENDA_UNIDADEENSINO_ID
                                                                                        WHERE AC.AGENDAID= a.AGENDAID
                                                                                          AND c.CURSO     = ac.CURSOID
                                                                                          AND s.SERIE     = ac.SERIE 
                                                                                          AND S.TURNO     = ac.TURNOID
                                                                                          AND UE.UNIDADEENSINOID = @UNIDADE
                                                                                     )
                                                                              THEN 1
                                                                         WHEN a.CURSOPORUNIDADE = 2
                                                                          AND not EXISTS ( 
                                                                                        SELECT 1
                                                                                        FROM  [LYCEUM].[Agenda].[AGENDA_CURSO] AC
                                                                                        INNER JOIN [AGENDA].[AGENDA_UNIDADEENSINO] UE ON UE.AGENDAID = AC.AGENDAID  
                                                                                        INNER JOIN [Agenda].[AGENDA_CURSO__AGENDA_UNIDADEENSINO] UEA ON UEA.AGENDA_CURSO_ID = AC.AGENDA_CURSO_ID AND UEA.AGENDA_UNIDADEENSINO_ID = UE.AGENDA_UNIDADEENSINO_ID
                                                                                        WHERE AC.AGENDAID= a.AGENDAID
                                                                                          AND c.CURSO     = ac.CURSOID
                                                                                          AND s.SERIE     = ac.SERIE 
                                                                                          AND S.TURNO     = ac.TURNOID
                                                                                          
                                                                                     )
                                                                              THEN 1
                                                                    END 
                                                                  ) = 1 

                                                        ) and exists (select * from LY_UNIDADE_ENSINO_CURSOS uc where uc.curso=c.curso and uc.UNIDADE_ENS = @UNIDADE)";

                contextQuery.Parameters.Add("@AGENDAID", AgendaId);
                contextQuery.Parameters.Add("@UNIDADE", unidade);

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


        public DataTable ListaSeriePorAgendaEParticipacao(int AgendaId, string unidade, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                                 S.SERIE                                               
                                            FROM LY_CURSO      c
                                           INNER JOIN LY_SERIE s 
                                              ON s.CURSO       = c.CURSO
                                           WHERE C.CURSO = @CURSO AND EXISTS ( 
                                                           SELECT 1
                                                             FROM Agenda.AGENDA a
                                                            WHERE a.AGENDAID    = @AGENDAID
                                                              AND ( CASE WHEN a.PARTICIPACURSOID = 0 
                                                                              THEN 1
                                                                         WHEN a.PARTICIPACURSOID = 1
                                                                          AND EXISTS ( 
                                                                                        SELECT 1
                                                                                          FROM Agenda.AGENDA_CURSO ac
                                                                                         WHERE ac.AGENDAID = a.AGENDAID
                                                                                           AND c.CURSO     = ac.CURSOID
                                                                                           AND s.SERIE     = ac.SERIE 
                                                                                           AND S.TURNO     = ac.TURNOID
                                                                                     )
                                                                              THEN 1
                                                                         WHEN a.PARTICIPACURSOID = 2
                                                                          AND NOT EXISTS ( 
                                                                                            SELECT 1
                                                                                              FROM Agenda.AGENDA_CURSO ac
                                                                                             WHERE ac.AGENDAID = a.AGENDAID
                                                                                               AND c.CURSO     = ac.CURSOID
                                                                                               AND S.SERIE     = ac.SERIE
                                                                                               AND S.TURNO     = ac.TURNOID 
                                                                                         )
                                                                              THEN 1
                                                                    END 
                                                                  ) = 1 
                                                                   AND ( CASE WHEN a.CURSOPORUNIDADE = 0 
                                                                              THEN 1
                                                                         WHEN a.CURSOPORUNIDADE = 1
                                                                          AND EXISTS ( 
                                                                                        SELECT 1
                                                                                        FROM  [LYCEUM].[Agenda].[AGENDA_CURSO] AC
                                                                                        INNER JOIN [AGENDA].[AGENDA_UNIDADEENSINO] UE ON UE.AGENDAID = AC.AGENDAID  
                                                                                        INNER JOIN [Agenda].[AGENDA_CURSO__AGENDA_UNIDADEENSINO] UEA ON UEA.AGENDA_CURSO_ID = AC.AGENDA_CURSO_ID AND UEA.AGENDA_UNIDADEENSINO_ID = UE.AGENDA_UNIDADEENSINO_ID
                                                                                        WHERE AC.AGENDAID= a.AGENDAID
                                                                                          AND c.CURSO     = ac.CURSOID
                                                                                          AND s.SERIE     = ac.SERIE 
                                                                                          AND S.TURNO     = ac.TURNOID
                                                                                          AND UE.UNIDADEENSINOID = @UNIDADE
                                                                                     )
                                                                              THEN 1
                                                                         WHEN a.CURSOPORUNIDADE = 2
                                                                          AND not EXISTS ( 
                                                                                        SELECT 1
                                                                                        FROM  [LYCEUM].[Agenda].[AGENDA_CURSO] AC
                                                                                        INNER JOIN [AGENDA].[AGENDA_UNIDADEENSINO] UE ON UE.AGENDAID = AC.AGENDAID  
                                                                                        INNER JOIN [Agenda].[AGENDA_CURSO__AGENDA_UNIDADEENSINO] UEA ON UEA.AGENDA_CURSO_ID = AC.AGENDA_CURSO_ID AND UEA.AGENDA_UNIDADEENSINO_ID = UE.AGENDA_UNIDADEENSINO_ID
                                                                                        WHERE AC.AGENDAID= a.AGENDAID
                                                                                          AND c.CURSO     = ac.CURSOID
                                                                                          AND s.SERIE     = ac.SERIE 
                                                                                          AND S.TURNO     = ac.TURNOID
                                                                                          
                                                                                     )
                                                                              THEN 1
                                                                    END 
                                                                  ) = 1 

                                                        ) ";

                contextQuery.Parameters.Add("@AGENDAID", AgendaId);
                contextQuery.Parameters.Add("@UNIDADE", unidade);
                contextQuery.Parameters.Add("@CURSO", curso);

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



    }
}
