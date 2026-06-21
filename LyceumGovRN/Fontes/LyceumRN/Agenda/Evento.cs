using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;

namespace Techne.Lyceum.RN.Agenda
{
    public class Evento : RNBase
    {
        public static DataTable ListaEvento()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT EVENTOID     ,
                                                 AGENDAID     ,
                                                 DATAINICIO   ,
                                                 DATAFIM      ,
                                                 TIPOEVENTOID ,
                                                 DATACADASTRO ,
                                                 DATAALTERACAO,
                                                 USUARIOID     
                                            FROM agenda.EVENTO     
                                        ORDER BY EVENTOID ";

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

        public static DataTable ListaEventoPorAgenda(int AgendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT EVENTOID     ,
                                                 AGENDAID     ,
                                                 DATAINICIO   ,
                                                 DATAFIM      ,
                                                 TIPOEVENTOID ,
                                                 DATACADASTRO ,
                                                 DATAALTERACAO,
                                                 USUARIOID     
                                            FROM agenda.EVENTO
                                           WHERE AGENDAID = @AGENDAID
                                        ORDER BY EVENTOID ";

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

        /// <summary>
        /// Lista todos os eventos com descrição de acordo com a agenda para utilização na grid
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable ListaEventosPorAgendaComDescricaoTipoEvento(int agendaId)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();
                contextQuery.Command = @"SELECT TE.[NOME],
                                                E.[DATAINICIO],
                                                E.[DATAFIM]
                                           FROM [LYCEUM].[Agenda].[EVENTO] E
                                          INNER JOIN [LYCEUM].[Agenda].[TIPOEVENTO] TE
                                             ON E.[TIPOEVENTOID] = TE.[TIPOEVENTOID]
                                          INNER JOIN [LYCEUM].[Agenda].[AGENDA] A
                                             ON A.[AGENDAID] = E.[AGENDAID]
                                          INNER JOIN [LYCEUM].[Agenda].[TIPOEVENTOCORRELATO] TEC
                                             ON TEC.[TIPOEVENTOID] = E.[TIPOEVENTOID]
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

        public static DataTable ListaEventoPorTipoEvento(int TipoEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT EVENTOID     ,
                                                 AGENDAID     ,
                                                 DATAINICIO   ,
                                                 DATAFIM      ,
                                                 TIPOEVENTOID ,
                                                 DATACADASTRO ,
                                                 DATAALTERACAO,
                                                 USUARIOID     
                                            FROM agenda.EVENTO
                                           WHERE TIPOEVENTOID = @TIPOEVENTOID
                                        ORDER BY EVENTOID ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", TipoEventoId);

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

        public static DataTable ListaEventoPorDataEvento(DateTime DataEvento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT EVENTOID     ,
                                                 AGENDAID     ,
                                                 DATAINICIO   ,
                                                 DATAFIM      ,
                                                 TIPOEVENTOID ,
                                                 DATACADASTRO ,
                                                 DATAALTERACAO,
                                                 USUARIOID     
                                            FROM agenda.EVENTO
                                           WHERE @DATAEVENTO BETWEEN DATAINICIO AND DATAFIM
                                        ORDER BY EVENTOID ";

                contextQuery.Parameters.Add("@DATAEVENTO", DataEvento);

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

        public static DataTable ListaEventoPorTipoEventoEPorDataEvento(int TipoEventoId, DateTime DataEvento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT EVENTOID     ,
                                                 AGENDAID     ,
                                                 DATAINICIO   ,
                                                 DATAFIM      ,
                                                 TIPOEVENTOID ,
                                                 DATACADASTRO ,
                                                 DATAALTERACAO,
                                                 USUARIOID     
                                            FROM agenda.EVENTO
                                           WHERE TIPOEVENTOID = @TIPOEVENTOID
                                             AND @DATAEVENTO BETWEEN DATAINICIO AND DATAFIM
                                        ORDER BY EVENTOID ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", TipoEventoId);
                contextQuery.Parameters.Add("@DATAEVENTO", DataEvento);

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

        public List<RN.Agenda.Entidades.Evento> ListaEventoAbertoPor(int tipoEventoId, DateTime dataEvento)
        {
            List<Entidades.Evento> listaEventos = new List<Entidades.Evento>();
            Entidades.Evento evento = new Entidades.Evento();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  EVENTOID ,
                                                  AGENDAID ,
                                                  DATAINICIO ,
                                                  DATAFIM ,
                                                  TIPOEVENTOID ,
                                                  DATACADASTRO ,
                                                  DATAALTERACAO ,
                                                  USUARIOID
                                          FROM    agenda.EVENTO
                                          WHERE   TIPOEVENTOID = @TIPOEVENTOID
                                                  AND CAST(@DATAEVENTO AS DATE) BETWEEN CAST(DATAINICIO AS DATE)
                                                                                AND     CAST(DATAFIM AS DATE)
                                          ORDER BY EVENTOID ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);
                contextQuery.Parameters.Add("@DATAEVENTO", dataEvento);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    evento.EventoId = Convert.ToInt32(reader["EVENTOID"]);
                    evento.AgendaId = Convert.ToInt32(reader["AGENDAID"]);
                    evento.DataInicio = Convert.ToDateTime(reader["DATAINICIO"]);
                    evento.DataFim = Convert.ToDateTime(reader["DATAFIM"]);
                    evento.TipoEventoId = Convert.ToInt32(reader["TIPOEVENTOID"]);
                    evento.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    if (reader["DATAALTERACAO"] != DBNull.Value)
                    {
                        evento.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                    }
                    evento.UsuarioId = Convert.ToString(reader["USUARIOID"]);

                    listaEventos.Add(evento);
                }

                return listaEventos;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public bool ExisteEventoAbertoPor(int tipoEventoId, DateTime dataEvento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                    FROM    agenda.EVENTO
                                    WHERE   TIPOEVENTOID = @TIPOEVENTOID
                                            AND CAST(@DATAEVENTO AS DATE) BETWEEN CAST(DATAINICIO AS DATE)
                                                                          AND     CAST(DATAFIM AS DATE) ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);
                contextQuery.Parameters.Add("@DATAEVENTO", dataEvento);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
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

        public static void InsereEvento(Entidades.Evento evento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO agenda.EVENTO( 
                                        AGENDAID     ,
                                        DATAINICIO   ,
                                        DATAFIM      ,
                                        TIPOEVENTOID ,
                                        DATACADASTRO ,
                                        DATAALTERACAO,
                                        USUARIOID     
                                    ) VALUES ( @AGENDAID     ,
                                               @DATAINICIO   ,
                                               @DATAFIM      ,
                                               @TIPOEVENTOID ,
                                               @DATACADASTRO ,
                                               @DATAALTERACAO,
                                               @USUARIOID         
                                             )"
                };

                contextQuery.Parameters.Add("@AGENDAID", evento.AgendaId);
                contextQuery.Parameters.Add("@DATAINICIO", evento.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", evento.DataFim);
                contextQuery.Parameters.Add("@TIPOEVENTOID", evento.TipoEventoId);
                contextQuery.Parameters.Add("@DATACADASTRO", evento.DataCadastro);
                contextQuery.Parameters.Add("@DATAALTERACAO", evento.DataAlteracao);
                contextQuery.Parameters.Add("@USUARIOID", evento.UsuarioId.Trim());

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

        public static void AlteraEvento(Entidades.Evento evento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"UPDATE agenda.EVENTO
                                   SET AGENDAID      = @AGENDAID     ,
                                       DATAINICIO    = @DATAINICIO   ,
                                       DATAFIM       = @DATAFIM      ,
                                       TIPOEVENTOID  = @TIPOEVENTOID ,
                                       DATACADASTRO  = @DATACADASTRO ,
                                       DATAALTERACAO = @DATAALTERACAO,
                                       USUARIOID     = @USUARIOID    
                                 WHERE EVENTOID = @EVENTOID "
                };

                contextQuery.Parameters.Add("@EVENTOID", evento.EventoId);
                contextQuery.Parameters.Add("@AGENDAID", evento.AgendaId);
                contextQuery.Parameters.Add("@DATAINICIO", evento.DataInicio);
                contextQuery.Parameters.Add("@DATAFIM", evento.DataFim);
                contextQuery.Parameters.Add("@TIPOEVENTOID", evento.TipoEventoId);
                contextQuery.Parameters.Add("@DATACADASTRO", evento.DataCadastro);
                contextQuery.Parameters.Add("@DATAALTERACAO", evento.DataAlteracao);
                contextQuery.Parameters.Add("@USUARIOID", evento.UsuarioId.Trim());

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

        public static void RemoveEvento(int EventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE agenda.EVENTO
                                  WHERE EVENTOID = @EVENTOID "
                };

                contextQuery.Parameters.Add("@EVENTOID", EventoId);

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

        public DateTime ObtemDataFimPor(int ano, int agendaId, int tipoEventoId, int codPerfil)
        {
            DateTime dataFim = DateTime.MinValue;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery();

                //Verifica se o usuario é privilegiado
                if (codPerfil == 0)
                {
                    contextQuery.Command = @" SELECT  MAX(DT_FIM_CONF_TURNO) AS DATAFIM
                                              FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA
                                              WHERE   AGENDAID = @AGENDAID
                                                      AND ANO = @ANO ";
                }
                else
                {
                    contextQuery.Command = @" SELECT AE.DATAFIM
                                              FROM    AGENDA.AGENDA AA
                                                      INNER JOIN AGENDA.EVENTO AE ON AA.AGENDAID = AE.AGENDAID
                                                      INNER JOIN AGENDA.EVENTO_PERFIL AEP ON AEP.EVENTOID = AE.EVENTOID
                                                      INNER JOIN HADES.DBO.TCE_PERFIL AP ON AP.ID_PERFIL = AEP.PERFILID
                                                      INNER JOIN AGENDA.TIPOEVENTO ATE ON AE.TIPOEVENTOID = ATE.TIPOEVENTOID
                                              WHERE   AA.AGENDAID = @AGENDAID
                                                      AND ATE.TIPOEVENTOID = @TIPOEVENTOID
                                                      AND AP.ID_PERFIL = @ID_PERFIL ";
                }

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);
                contextQuery.Parameters.Add("@ID_PERFIL", codPerfil);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dataFim = Convert.ToDateTime(reader["DATAFIM"]);
                }

                return dataFim;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public DataTable ListaAnoParaReaberturaAberto(DateTime dataEncerramento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable ano = null;
            int idEventoReaberturaMatricula = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ReaberturaMatricula);

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                    LPL.[ANO]
                            FROM    DBO.LY_PERIODO_LETIVO LPL
                                    INNER JOIN [LYCEUM].[Agenda].[PERIODOLETIVOAGENDA] PLA ON LPL.[ANO] = PLA.ANO
                                                                                          AND LPL.[PERIODO] = PLA.PERIODO
                                    INNER JOIN [LYCEUM].[Agenda].[EVENTO] EV ON EV.AGENDAID = PLA.AGENDAID
                            WHERE   CONVERT(DATE, DT_INICIO) <= CONVERT(DATE, GETDATE())
                                    AND CONVERT(DATE, DT_FIM) >= CONVERT(DATE, @DATAENCERRAMENTO)
                                    AND EV.DATAINICIO <= CONVERT(DATE, @DATAENCERRAMENTO)
                                    AND EV.DATAFIM >= CONVERT(DATE, GETDATE())
                                    AND EV.[TIPOEVENTOID] = @TIPOEVENTOID
                            ORDER BY ANO DESC
                             ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", idEventoReaberturaMatricula);
                contextQuery.Parameters.Add("@DATAENCERRAMENTO", dataEncerramento);

                ano = ctx.GetDataTable(contextQuery);
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

            return ano;
        }

        public DataTable ListaPeriodoParaReaberturaAbertoPor(int ano, DateTime dataEncerramento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable periodo = null;
            int idEventoReaberturaMatricula = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ReaberturaMatricula);

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                            LPL.PERIODO
                    FROM    DBO.LY_PERIODO_LETIVO LPL
                            INNER JOIN [LYCEUM].[Agenda].[PERIODOLETIVOAGENDA] PLA ON LPL.[ANO] = PLA.ANO
                                                                                  AND LPL.[PERIODO] = PLA.PERIODO
                            INNER JOIN [LYCEUM].[Agenda].[EVENTO] EV ON EV.AGENDAID = PLA.AGENDAID
                    WHERE   CONVERT(DATE, DT_INICIO) <= CONVERT(DATE, GETDATE())
                            AND CONVERT(DATE, DT_FIM) >= CONVERT(DATE, @DATAENCERRAMENTO)
                            AND LPL.ANO = @ANO
                            AND EV.DATAINICIO <= CONVERT(DATE, @DATAENCERRAMENTO)
                            AND EV.DATAFIM >= CONVERT(DATE, GETDATE())
                            AND EV.[TIPOEVENTOID] = @TIPOEVENTOID ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", idEventoReaberturaMatricula);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@DATAENCERRAMENTO", dataEncerramento);

                periodo = ctx.GetDataTable(contextQuery);
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

            return periodo;
        }

        public DataTable ListaPeriodoEventoPorTipoEventoEPorAno(int TipoEventoId, int ano)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable periodo = null;

            try
            {
                contextQuery.Command = @"  SELECT   DISTINCT
                                                    PERIODO
                                            FROM    AGENDA.PERIODOLETIVOAGENDA P
                                                    INNER JOIN agenda.AGENDA AA ON aa.AGENDAID = P.AGENDAID
                                                    INNER JOIN agenda.EVENTO AE ON AA.AGENDAID = AE.AGENDAID
                                            WHERE   ANO = @ANO
                                                    AND AE.TIPOEVENTOID = @TIPOEVENTOID
                                                    AND GETDATE() BETWEEN DATAINICIO AND DATAFIM ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", TipoEventoId);
                contextQuery.Parameters.Add("@ANO", ano);

                periodo = ctx.GetDataTable(contextQuery);
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

            return periodo;
        }

        public List<RN.Agenda.Entidades.Evento> ListaEventoAbertoPor(int tipoEventoId, DateTime dataEvento, int ano, int periodo)
        {
            List<Entidades.Evento> listaEventos = new List<Entidades.Evento>();
            Entidades.Evento evento = new Entidades.Evento();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  EVENTOID ,
                                                  E.AGENDAID ,
                                                  DATAINICIO ,
                                                  DATAFIM ,
                                                  TIPOEVENTOID ,
                                                  DATACADASTRO ,
                                                  DATAALTERACAO ,
                                                  USUARIOID
                                          FROM    agenda.EVENTO E
                                          INNER JOIN AGENDA.PERIODOLETIVOAGENDA P ON E.AGENDAID = P.AGENDAID
                                          WHERE   TIPOEVENTOID = @TIPOEVENTOID
                                                  AND CAST(@DATAEVENTO AS DATE) BETWEEN CAST(DATAINICIO AS DATE)
                                                                                AND     CAST(DATAFIM AS DATE)
                                                  AND ANO = @ANO
                                                  AND PERIODO = @PERIODO
                                          ORDER BY EVENTOID ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);
                contextQuery.Parameters.Add("@DATAEVENTO", dataEvento);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    evento.EventoId = Convert.ToInt32(reader["EVENTOID"]);
                    evento.AgendaId = Convert.ToInt32(reader["AGENDAID"]);
                    evento.DataInicio = Convert.ToDateTime(reader["DATAINICIO"]);
                    evento.DataFim = Convert.ToDateTime(reader["DATAFIM"]);
                    evento.TipoEventoId = Convert.ToInt32(reader["TIPOEVENTOID"]);
                    evento.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    if (reader["DATAALTERACAO"] != DBNull.Value)
                    {
                        evento.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                    }
                    evento.UsuarioId = Convert.ToString(reader["USUARIOID"]);

                    listaEventos.Add(evento);
                }

                return listaEventos;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

    }
}