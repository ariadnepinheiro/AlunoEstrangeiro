using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.DTOs.Agenda;

namespace Techne.Lyceum.RN.Agenda
{
    public class Agenda : RNBase
    {
        public static DataTable ListaAgenda()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT  AGENDAID          ,
                                                  DESCRICAO         ,
                                                  OBSERVACAO        ,
                                                  PARTICIPAUNIDADEID,
                                                  PARTICIPACURSOID  ,
                                                  CURSOPORUNIDADE   ,
                                                  DATACADASTRO      ,
                                                  DATAALTERACAO     ,
                                                  USUARIOID         
                                            FROM  agenda.AGENDA     
                                        ORDER BY  AGENDAID ";

                agenda = contexto.GetDataTable(contextQuery);
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

            return agenda;
        }

        public static DataTable ListaAgendaPorTipoEvento(int TipoEventoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT A.AGENDAID          ,
                                                 A.DESCRICAO         ,
                                                 A.OBSERVACAO        ,
                                                 A.PARTICIPAUNIDADEID,
                                                 A.PARTICIPACURSOID  ,
                                                 A.CURSOPORUNIDADE   ,
                                                 A.DATACADASTRO      ,
                                                 A.DATAALTERACAO     ,
                                                 A.USUARIOID         
                                            FROM agenda.AGENDA      A
                                           INNER JOIN agenda.EVENTO E
                                              ON A.AGENDAID         = E.AGENDAID
                                           WHERE E.TIPOEVENTOID = @TIPOEVENTOID
                                        ORDER BY A.AGENDAID ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", TipoEventoId);

                agenda = contexto.GetDataTable(contextQuery);
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

            return agenda;
        }

        public static DataTable ListaAgendaPorDataEvento(DateTime DataEvento)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT A.AGENDAID          ,
                                                 A.DESCRICAO         ,
                                                 A.OBSERVACAO        ,
                                                 A.PARTICIPAUNIDADEID,
                                                 A.PARTICIPACURSOID  ,
                                                 A.CURSOPORUNIDADE   ,
                                                 A.DATACADASTRO      ,
                                                 A.DATAALTERACAO     ,
                                                 A.USUARIOID         
                                            FROM agenda.AGENDA      A
                                           INNER JOIN agenda.EVENTO E
                                              ON A.AGENDAID         = E.AGENDAID
                                           WHERE @DATAEVENTO BETWEEN E.DATAINICIO AND E.DATAFIM
                                        ORDER BY A.AGENDAID ";

                contextQuery.Parameters.Add("@DATAEVENTO", DataEvento);

                agenda = contexto.GetDataTable(contextQuery);
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

            return agenda;
        }

        public static DataTable ListaAgendaPorTipoEventoEDataEvento(int TipoEventoId, DateTime DataEvento)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT A.AGENDAID          ,
                                                 A.DESCRICAO         ,
                                                 A.OBSERVACAO        ,
                                                 A.PARTICIPAUNIDADEID,
                                                 A.PARTICIPACURSOID  ,
                                                 A.CURSOPORUNIDADE   ,
                                                 A.DATACADASTRO      ,
                                                 A.DATAALTERACAO     ,
                                                 A.USUARIOID         ,
                                                 E.DATAINICIO        ,
                                                 E.DATAFIM        
                                            FROM agenda.AGENDA      A
                                           INNER JOIN agenda.EVENTO E
                                                ON A.AGENDAID         = E.AGENDAID
                                           WHERE   E.TIPOEVENTOID = @TIPOEVENTOID
                                                AND CAST(@DATAEVENTO AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE)
                                      AND     CAST(E.DATAFIM AS DATE)
                                        ORDER BY A.AGENDAID ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", TipoEventoId);
                contextQuery.Parameters.Add("@DATAEVENTO", DataEvento);

                agenda = contexto.GetDataTable(contextQuery);
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

            return agenda;
        }

        public List<RN.Agenda.Entidades.Agenda> ListaAgendasAbertaPor(int tipoEventoId, DateTime dataEvento, int ano, int periodo)
        {
            List<Entidades.Agenda> listaAgendas = new List<Entidades.Agenda>();
            Entidades.Agenda agenda = new Entidades.Agenda();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT A.AGENDAID ,
                                    A.DESCRICAO ,
                                    A.OBSERVACAO ,
                                    A.PARTICIPAUNIDADEID ,
                                    A.PARTICIPACURSOID ,
                                    A.CURSOPORUNIDADE ,
                                    A.DATACADASTRO ,
                                    A.DATAALTERACAO ,
                                    A.USUARIOID
                            FROM    agenda.AGENDA A
                                    INNER JOIN agenda.EVENTO E ON A.AGENDAID = E.AGENDAID
                                    INNER JOIN Agenda.PERIODOLETIVOAGENDA pl ON E.AGENDAID = pl.AGENDAID
                            WHERE   E.TIPOEVENTOID = @TIPOEVENTOID
                                    AND CAST(@DATAEVENTO AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE)
                                                                   AND     CAST(E.DATAFIM AS DATE)
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                            ORDER BY A.AGENDAID ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);
                contextQuery.Parameters.Add("@DATAEVENTO", dataEvento);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    agenda.AgendaId = Convert.ToInt32(reader["AGENDAID"]);
                    agenda.Descricao = Convert.ToString(reader["DESCRICAO"]);
                    agenda.Observacao = Convert.ToString(reader["OBSERVACAO"]);
                    agenda.ParticipaUnidadeId = Convert.ToInt32(reader["PARTICIPAUNIDADEID"]);
                    agenda.ParticipaCursoId = Convert.ToInt32(reader["PARTICIPACURSOID"]);
                    agenda.CursoPorUnidade = Convert.ToBoolean(reader["CURSOPORUNIDADE"]);
                    agenda.DataCadastro = Convert.ToDateTime(reader["DATACADASTRO"]);
                    if (reader["DATAALTERACAO"] != DBNull.Value)
                    {
                        agenda.DataAlteracao = Convert.ToDateTime(reader["DATAALTERACAO"]);
                    }
                    agenda.UsuarioId = Convert.ToString(reader["USUARIOID"]);

                    listaAgendas.Add(agenda);
                }

                return listaAgendas;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }
      
        public List<RN.DTOs.Agenda.DadosAgendaEvento> ListaAgendasEventosAbertaPor(int tipoEventoId, DateTime dataEvento)
        {
            List<RN.DTOs.Agenda.DadosAgendaEvento> listaAgendas = new List<RN.DTOs.Agenda.DadosAgendaEvento>();
            RN.DTOs.Agenda.DadosAgendaEvento agenda;
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                    A.AGENDAID ,
                                    e.EVENTOID, 
                                    e.TIPOEVENTOID, 
                                    e.DATAINICIO,
                                    e.DATAFIM ,
                                    A.PARTICIPAUNIDADEID ,
                                    A.PARTICIPACURSOID ,
                                    A.CURSOPORUNIDADE 
                            FROM    agenda.AGENDA A
                                    INNER JOIN agenda.EVENTO E ON A.AGENDAID = E.AGENDAID
                            WHERE   E.TIPOEVENTOID = @TIPOEVENTOID
                                    AND CAST(@DATAEVENTO AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE)
                                                                  AND     CAST(E.DATAFIM AS DATE)
                            ORDER BY A.AGENDAID ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);
                contextQuery.Parameters.Add("@DATAEVENTO", dataEvento);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    agenda = new RN.DTOs.Agenda.DadosAgendaEvento();

                    agenda.AgendaId = Convert.ToInt32(reader["AGENDAID"]);
                    agenda.EventoId = Convert.ToInt32(reader["EVENTOID"]);
                    agenda.TipoEventoId = Convert.ToInt32(reader["TIPOEVENTOID"]);
                    agenda.DataInicio = Convert.ToDateTime(reader["DATAINICIO"]);
                    agenda.DataFim = Convert.ToDateTime(reader["DATAFIM"]);
                    agenda.ParticipaUnidadeId = Convert.ToInt32(reader["PARTICIPAUNIDADEID"]);
                    agenda.ParticipaCursoId = Convert.ToInt32(reader["PARTICIPACURSOID"]);
                    agenda.CursoPorUnidade = Convert.ToBoolean(reader["CURSOPORUNIDADE"]);

                    listaAgendas.Add(agenda);
                }

                return listaAgendas;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public DadosParticipacao VerificaEventoInversoPor(int idEvento, string unidadeEnsino, string curso, string turno, int serie)
        {
            //UTILIZADO PARA BLOQUEIOS!

            DadosParticipacao evento = new DadosParticipacao();
            RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
            RN.Agenda.Agenda_UnidadeEnsino rnAgenda_UnidadeEnsino = new Techne.Lyceum.RN.Agenda.Agenda_UnidadeEnsino();
            RN.Agenda.Agenda_Curso rnAgenda_Curso = new Techne.Lyceum.RN.Agenda.Agenda_Curso();
            RN.Agenda.Agenda_Curso__Agenda_UnidadeEnsino rnAgenda_Curso__Agenda_UnidadeEnsino = new Techne.Lyceum.RN.Agenda.Agenda_Curso__Agenda_UnidadeEnsino();
            List<RN.DTOs.Agenda.DadosAgendaEvento> agendas = new List<RN.DTOs.Agenda.DadosAgendaEvento>();
            bool porCursoUnidade = false;
            bool porUnidade = false;
            bool porCurso = false;
            int agendaId = 0;
            
            try
            {
                //Verifica se existem eventos de bloqueio abertos
                agendas = rnAgenda.ListaAgendasEventosAbertaPor(idEvento, DateTime.Today);

                if (agendas.Count != 0)
                {
                    evento.ParticipaTotal = true;
                    evento.ParticipaCurso = true;
                    evento.ParticipaUnidade = true;

                    foreach (RN.DTOs.Agenda.DadosAgendaEvento agenda in agendas)
                    {
                        porCursoUnidade = agenda.CursoPorUnidade;
                        porUnidade = agenda.ParticipaUnidadeId > 0;
                        porCurso = agenda.ParticipaCursoId > 0;
                        agendaId = agenda.AgendaId;
                        evento.DataInicio = agenda.DataInicio;
                        evento.DataFim = agenda.DataFim;

                        evento.UnidadeEnsino = unidadeEnsino;
                        evento.Curso = curso;
                        evento.Turno = turno;
                        evento.Serie = serie;

                        //Verifica se é agenda é geral            
                        evento.ParticipaTotal = ((evento.ParticipaTotal) && (!porCursoUnidade && !porUnidade && !porCurso));
                        
                        //Verifica se é agenda por curso unidade
                        if (porCursoUnidade)
                        {
                            //Verifica se a escola / curso / turno / serie do aluno fazem parte do evento
                            //PRIORITARIO SOBRE OS DEMAIS
                            if (rnAgenda_Curso__Agenda_UnidadeEnsino.EhUnidadeCursoTurnoSerieParticipantePor(agendaId, unidadeEnsino, curso, turno, serie))
                            {
                                evento.ParticipaCurso = true;
                                evento.ParticipaUnidade = true;
                                return evento;
                            }
                        }
                        else
                        {
                            //Verifica se é agenda por unidade
                            if (porUnidade)
                            {
                                //Verifica se a escola do aluno faz parte do evento
                                if (!rnAgenda_UnidadeEnsino.EhUnidadeParticipantePor(agendaId, unidadeEnsino))
                                {
                                    evento.ParticipaUnidade = false;
                                }
                            }

                            //Verifica se é agenda por curso
                            if (porCurso)
                            {
                                //Verifica se o curso / turno / serie do aluno faz parte do evento
                                if (!rnAgenda_Curso.EhCursoTurnoSerieParticipantePor(agendaId, curso, turno, serie))
                                {
                                    evento.ParticipaCurso = false;
                                }
                            }
                        }
                    }
                }

                return evento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DadosParticipacao VerificaEventoPor(int idEvento, string unidadeEnsino, string curso, string turno, int serie)
        {
            DadosParticipacao evento = new DadosParticipacao();
            RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
            RN.Agenda.Agenda_UnidadeEnsino rnAgenda_UnidadeEnsino = new Techne.Lyceum.RN.Agenda.Agenda_UnidadeEnsino();
            RN.Agenda.Agenda_Curso rnAgenda_Curso = new Techne.Lyceum.RN.Agenda.Agenda_Curso();
            RN.Agenda.Agenda_Curso__Agenda_UnidadeEnsino rnAgenda_Curso__Agenda_UnidadeEnsino = new Techne.Lyceum.RN.Agenda.Agenda_Curso__Agenda_UnidadeEnsino();
            List<RN.DTOs.Agenda.DadosAgendaEvento> agendas = new List<RN.DTOs.Agenda.DadosAgendaEvento>();
            bool porCursoUnidade = false;
            bool porUnidade = false;
            bool porCurso = false;
            int agendaId = 0;

            try
            {
                //Verifica se existem eventos de bloqueio abertos
                agendas = rnAgenda.ListaAgendasEventosAbertaPor(idEvento, DateTime.Today);

                if (agendas.Count != 0)
                {
                    evento.ParticipaTotal = false;
                    evento.ParticipaCurso = false;
                    evento.ParticipaUnidade = false;

                    foreach (RN.DTOs.Agenda.DadosAgendaEvento agenda in agendas)
                    {
                        porCursoUnidade = agenda.CursoPorUnidade;
                        porUnidade = agenda.ParticipaUnidadeId > 0;
                        porCurso = agenda.ParticipaCursoId > 0;
                        agendaId = agenda.AgendaId;
                        evento.DataInicio = agenda.DataInicio;
                        evento.DataFim = agenda.DataFim;

                        evento.UnidadeEnsino = unidadeEnsino;
                        evento.Curso = curso;
                        evento.Turno = turno;
                        evento.Serie = serie;

                        //Verifica se é agenda é geral (Se não for por curso, unidade, ou curso / unidade)        
                        evento.ParticipaTotal = (!porCursoUnidade && !porUnidade && !porCurso);

                        if (evento.ParticipaTotal)
                        {
                            return evento;
                        }

                        //Verifica se é agenda por curso unidade
                        if (porCursoUnidade) 
                        {
                            //Verifica se a escola / curso / turno / serie do aluno fazem parte do evento
                            if (rnAgenda_Curso__Agenda_UnidadeEnsino.EhUnidadeCursoTurnoSerieParticipantePor(agendaId, unidadeEnsino, curso, turno, serie))
                            {
                                evento.ParticipaCurso = true;
                                evento.ParticipaUnidade = true;
                                return evento;
                            }
                        }
                        else
                        {
                            //Verifica se é agenda por curso e unidade
                            if (porCurso && porUnidade)
                            {
                                //Verifica se o curso / turno / serie e a unidade do aluno faz parte do evento
                                if (rnAgenda_Curso.EhCursoTurnoSerieParticipantePor(agendaId, curso, turno, serie) && rnAgenda_UnidadeEnsino.EhUnidadeParticipantePor(agendaId, unidadeEnsino))
                                {
                                    evento.ParticipaCurso = true;
                                    evento.ParticipaUnidade = true;
                                    return evento;
                                }
                            }

                            //Verifica se é agenda por unidade
                            if (porUnidade && !porCurso)
                            {
                                //Verifica se a escola do aluno faz parte do evento
                                if (rnAgenda_UnidadeEnsino.EhUnidadeParticipantePor(agendaId, unidadeEnsino))
                                {
                                    evento.ParticipaUnidade = true;
                                    return evento;
                                }
                            }

                            //Verifica se é agenda por curso
                            if (porCurso && !porUnidade)
                            {
                                //Verifica se o curso / turno / serie do aluno faz parte do evento
                                if (rnAgenda_Curso.EhCursoTurnoSerieParticipantePor(agendaId, curso, turno, serie))
                                {
                                    evento.ParticipaCurso = true;
                                    return evento;
                                }
                            }                            
                        }
                    }
                }

                return evento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RN.DTOs.Agenda.DadosAgendaEvento> ListaAgendasEventosAbertaPor(int tipoEventoId, DateTime dataEvento, int ano, int periodo)
        {
            List<RN.DTOs.Agenda.DadosAgendaEvento> listaAgendas = new List<RN.DTOs.Agenda.DadosAgendaEvento>();
            RN.DTOs.Agenda.DadosAgendaEvento agenda;
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                                A.AGENDAID ,
                                                e.EVENTOID ,
                                                e.TIPOEVENTOID ,
                                                e.DATAINICIO ,
                                                e.DATAFIM ,
                                                A.PARTICIPAUNIDADEID ,
                                                A.PARTICIPACURSOID ,
                                                A.CURSOPORUNIDADE
                                        FROM    agenda.AGENDA A
                                                INNER JOIN agenda.EVENTO E ON A.AGENDAID = E.AGENDAID
                                                INNER JOIN Agenda.PERIODOLETIVOAGENDA pl ON E.AGENDAID = pl.AGENDAID
                                        WHERE   E.TIPOEVENTOID = @TIPOEVENTOID
                                                AND CAST(@DATAEVENTO AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE)
                                                                              AND     CAST(E.DATAFIM AS DATE)
                                                AND ANO = @ANO
                                                AND PERIODO = @PERIODO
                                        ORDER BY A.AGENDAID  ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);
                contextQuery.Parameters.Add("@DATAEVENTO", dataEvento);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    agenda = new DadosAgendaEvento();
                    agenda.AgendaId = Convert.ToInt32(reader["AGENDAID"]);
                    agenda.EventoId = Convert.ToInt32(reader["EVENTOID"]);
                    agenda.TipoEventoId = Convert.ToInt32(reader["TIPOEVENTOID"]);
                    agenda.DataInicio = Convert.ToDateTime(reader["DATAINICIO"]);
                    agenda.DataFim = Convert.ToDateTime(reader["DATAFIM"]);
                    agenda.ParticipaUnidadeId = Convert.ToInt32(reader["PARTICIPAUNIDADEID"]);
                    agenda.ParticipaCursoId = Convert.ToInt32(reader["PARTICIPACURSOID"]);
                    agenda.CursoPorUnidade = Convert.ToBoolean(reader["CURSOPORUNIDADE"]);

                    listaAgendas.Add(agenda);
                }

                return listaAgendas;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public DadosParticipacao VerificaEventoPor(int idEvento, int ano, int periodo, string unidadeEnsino, string curso)
        {
            DadosParticipacao evento = new DadosParticipacao();
            RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
            RN.Agenda.Agenda_UnidadeEnsino rnAgenda_UnidadeEnsino = new Techne.Lyceum.RN.Agenda.Agenda_UnidadeEnsino();
            RN.Agenda.Agenda_Curso rnAgenda_Curso = new Techne.Lyceum.RN.Agenda.Agenda_Curso();
            RN.Agenda.Agenda_Curso__Agenda_UnidadeEnsino rnAgenda_Curso__Agenda_UnidadeEnsino = new Techne.Lyceum.RN.Agenda.Agenda_Curso__Agenda_UnidadeEnsino();
            List<RN.DTOs.Agenda.DadosAgendaEvento> agendas = new List<RN.DTOs.Agenda.DadosAgendaEvento>();
            bool porCursoUnidade = false;
            bool porUnidade = false;
            bool porCurso = false;
            int agendaId = 0;

            try
            {
                //Verifica se existem eventos de bloqueio abertos
                agendas = rnAgenda.ListaAgendasEventosAbertaPor(idEvento, DateTime.Today, ano, periodo);

                if (agendas.Count != 0)
                {
                    evento.ParticipaTotal = true;
                    evento.ParticipaCurso = true;
                    evento.ParticipaUnidade = true;

                    foreach (RN.DTOs.Agenda.DadosAgendaEvento agenda in agendas)
                    {
                        porCursoUnidade = agenda.CursoPorUnidade;
                        porUnidade = agenda.ParticipaUnidadeId > 0;
                        porCurso = agenda.ParticipaCursoId > 0;
                        agendaId = agenda.AgendaId;
                        evento.DataInicio = agenda.DataInicio;
                        evento.DataFim = agenda.DataFim;

                        evento.UnidadeEnsino = unidadeEnsino;
                        evento.Curso = curso;

                        //Verifica se é agenda é geral            
                        evento.ParticipaTotal = ((evento.ParticipaTotal) && (!porCursoUnidade && !porUnidade && !porCurso));

                        //Verifica se é agenda por curso unidade
                        if (porCursoUnidade)
                        {
                            //Verifica se a escola / curso / turno / serie do aluno fazem parte do evento
                            //PRIORITARIO SOBRE OS DEMAIS
                            if (rnAgenda_Curso__Agenda_UnidadeEnsino.EhUnidadeCursoParticipantePor(agendaId, unidadeEnsino, curso))
                            {
                                evento.ParticipaCurso = true;
                                evento.ParticipaUnidade = true;
                                return evento;
                            }
                        }
                        else
                        {
                            //Verifica se é agenda por unidade
                            if (porUnidade)
                            {
                                //Verifica se a escola do aluno faz parte do evento
                                if (!rnAgenda_UnidadeEnsino.EhUnidadeParticipantePor(agendaId, unidadeEnsino))
                                {
                                    evento.ParticipaUnidade = false;
                                }
                            }

                            //Verifica se é agenda por curso
                            if (porCurso)
                            {
                                //Verifica se o curso / turno / serie do aluno faz parte do evento
                                if (!rnAgenda_Curso.EhCursoParticipantePor(agendaId, curso))
                                {
                                    evento.ParticipaCurso = false;
                                }
                            }
                        }
                    }
                }

                return evento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void InsereAgenda(Entidades.Agenda agenda)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO agenda.AGENDA( 
                                        DESCRICAO         ,
                                        OBSERVACAO        ,
                                        PARTICIPAUNIDADEID,
                                        PARTICIPACURSOID  ,
                                        CURSOPORUNIDADE   ,
                                        DATACADASTRO      ,
                                        DATAALTERACAO     ,
                                        USUARIOID
                                    ) VALUES ( @DESCRICAO         ,
                                               @OBSERVACAO        ,
                                               @PARTICIPAUNIDADEID,
                                               @PARTICIPACURSOID  ,
                                               @CURSOPORUNIDADE   ,
                                               @DATACADASTRO      ,
                                               @DATAALTERACAO     ,
                                               @USUARIOID         
                                             )"
                };

                contextQuery.Parameters.Add("@DESCRICAO", agenda.Descricao.Trim());
                contextQuery.Parameters.Add("@OBSERVACAO", agenda.Observacao.Trim());
                contextQuery.Parameters.Add("@PARTICIPAUNIDADEID", agenda.ParticipaUnidadeId);
                contextQuery.Parameters.Add("@PARTICIPACURSOID", agenda.ParticipaCursoId);
                contextQuery.Parameters.Add("@CURSOPORUNIDADE", agenda.CursoPorUnidade);
                contextQuery.Parameters.Add("@DATACADASTRO", agenda.DataCadastro);
                contextQuery.Parameters.Add("@DATAALTERACAO", agenda.DataAlteracao);
                contextQuery.Parameters.Add("@USUARIOID", agenda.UsuarioId.Trim());

                contexto.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public static void AlteraAgenda(Entidades.Agenda agenda)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"UPDATE agenda.AGENDA
                                   SET DESCRICAO          = @DESCRICAO         ,
                                       OBSERVACAO         = @OBSERVACAO        ,
                                       PARTICIPAUNIDADEID = @PARTICIPAUNIDADEID,
                                       PARTICIPACURSOID   = @PARTICIPACURSOID  ,
                                       CURSOPORUNIDADE    = @CURSOPORUNIDADE   ,
                                       DATACADASTRO       = @DATACADASTRO      ,
                                       DATAALTERACAO      = @DATAALTERACAO     ,
                                       USUARIOID          = @USUARIOID
                                 WHERE AGENDAID = @AGENDAID "
                };

                contextQuery.Parameters.Add("@AGENDAID", agenda.AgendaId);
                contextQuery.Parameters.Add("@DESCRICAO", agenda.Descricao.Trim());
                contextQuery.Parameters.Add("@OBSERVACAO", agenda.Observacao.Trim());
                contextQuery.Parameters.Add("@PARTICIPAUNIDADEID", agenda.ParticipaUnidadeId);
                contextQuery.Parameters.Add("@PARTICIPACURSOID", agenda.ParticipaCursoId);
                contextQuery.Parameters.Add("@CURSOPORUNIDADE", agenda.CursoPorUnidade);
                contextQuery.Parameters.Add("@DATACADASTRO", agenda.DataCadastro);
                contextQuery.Parameters.Add("@DATAALTERACAO", agenda.DataAlteracao);
                contextQuery.Parameters.Add("@USUARIOID", agenda.UsuarioId.Trim());

                contexto.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public static void RemoveAgenda(int AgendaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" DELETE agenda.AGENDA
                                  WHERE AGENDAID = @AGENDAID "
                };

                contextQuery.Parameters.Add("@AGENDAID", AgendaId);

                contexto.ApplyModifications(contextQuery);
            }

            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Format(@"Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0} Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public DataTable ListaCursosMatriculaFacilPor(int agendaIdVinculada)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable cursos = null;

            try
            {

                if (agendaIdVinculada <= 0)
                {
                    return null;
                }

                contextQuery.Command = @" SELECT DISTINCT
                                                C.CURSO ,
                                                C.NOME ,
                                                S.SERIE
                                        FROM    LY_CURSO C
                                                INNER JOIN LY_SERIE S ON S.CURSO = C.CURSO
                                        WHERE   EXISTS ( SELECT 1
                                                         FROM   Agenda.AGENDA a
                                                         WHERE  a.AGENDAID = @AGENDAID
                                                                AND ( CASE WHEN a.PARTICIPACURSOID = 0 THEN 1
                                                                           WHEN a.PARTICIPACURSOID = 1
                                                                                AND EXISTS ( SELECT 1
                                                                                             FROM   Agenda.AGENDA_CURSO ac
                                                                                             WHERE  ac.AGENDAID = a.AGENDAID
                                                                                                    AND c.CURSO = ac.CURSOID
                                                                                                    AND S.SERIE = AC.SERIE )
                                                                           THEN 1
                                                                           WHEN a.PARTICIPACURSOID = 2
                                                                                AND NOT EXISTS ( SELECT
                                                                                                      1
                                                                                                 FROM Agenda.AGENDA_CURSO ac
                                                                                                 WHERE
                                                                                                      ac.AGENDAID = a.AGENDAID
                                                                                                      AND c.CURSO = ac.CURSOID
                                                                                                      AND S.SERIE = ac.SERIE )
                                                                           THEN 1
                                                                      END ) = 1 ) ";

                contextQuery.Parameters.Add("@AGENDAID", agendaIdVinculada);

                cursos = contexto.GetDataTable(contextQuery);

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
            

            return cursos;
        }

        public int ObtemAgendaVinculadaPor(int agendaId, int tipoEvendoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            int agendaVinculadaId = 0;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  A.AGENDAID
                            FROM    AGENDA.AGENDAVINCULADA V
                                    INNER JOIN AGENDA.AGENDA A ON A.AGENDAID = V.AGENDAVINCULADAID
                                    INNER JOIN AGENDA.EVENTO E ON E.AGENDAID = A.AGENDAID
                            WHERE   V.AGENDAID = @AGENDAID
                                    AND E.TIPOEVENTOID = @TIPOEVENDOID "
                };
                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@TIPOEVENDOID", tipoEvendoId);


                agendaVinculadaId = contexto.GetReturnValue(contextQuery) == null ? 0 : contexto.GetReturnValue<int>(contextQuery);

                return agendaVinculadaId;
            }

            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

        }

        public List<string> ObtemListaCursosNaoParticipantesPor(int agendaId)
        {
            List<string> lista = new List<string>();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                        C.CURSO
                FROM    LY_CURSO C
                WHERE   NOT EXISTS ( SELECT 1
                                     FROM   agenda.AGENDA a
                                     WHERE  a.AGENDAID = @AGENDAID
                                            AND ( CASE WHEN a.PARTICIPACURSOID = 0 THEN 1
                                                       WHEN a.PARTICIPACURSOID = 1
                                                            AND EXISTS ( SELECT
                                                                              1
                                                                         FROM agenda.AGENDA_CURSO ac
                                                                         WHERE
                                                                              ac.AGENDAID = a.AGENDAID
                                                                              AND c.CURSO = ac.CURSOID )
                                                       THEN 1
                                                       WHEN a.PARTICIPACURSOID = 2
                                                            AND NOT EXISTS ( SELECT
                                                                              1
                                                                             FROM
                                                                              agenda.AGENDA_CURSO ac
                                                                             WHERE
                                                                              ac.AGENDAID = a.AGENDAID
                                                                              AND c.CURSO = ac.CURSOID )
                                                       THEN 1
                                                  END ) = 1 ) ";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lista.Add(Convert.ToString(reader["CURSO"]));
                }

                return lista;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public DataTable ConsultaAgendaPorAnoEPeriodo(int ano, string periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;


            try
            {
                contextQuery.Command = @" SELECT DISTINCT A.[AGENDAID],
                                                 PLA.ANO,
                                                 TE.[NOME] AS TIPOEVENTO,
                                                 A.[DESCRICAO] AS NOMEAGENDA,
                                                 Convert(Date,E.[DATAINICIO]) AS DATAINICIO,
                                                 Convert(Date,E.[DATAFIM]) AS DATAFIM,
                                                 Stuff((SELECT Cast('-' AS VARCHAR(52)) + Cast(P.PERIODO AS VARCHAR(10))
                                                          FROM [LYCEUM].[Agenda].[PERIODOLETIVOAGENDA] P
                                                         WHERE P.AGENDAID = A.AGENDAID
                                                         ORDER BY P.PERIODO
                                                           FOR xml path('')), 1, 1, '') AS PERIODO
	                                        FROM [LYCEUM].[Agenda].[AGENDA] A
	                                       INNER JOIN [LYCEUM].[Agenda].[EVENTO] E
		                                      ON A.AGENDAID = E.AGENDAID
	                                       INNER JOIN [LYCEUM].[Agenda].[TIPOEVENTO] TE
		                                      ON E.TIPOEVENTOID = TE.TIPOEVENTOID
	                                       INNER JOIN [LYCEUM].[Agenda].[PERIODOLETIVOAGENDA] PLA
		                                      ON A.AGENDAID = PLA.AGENDAID
	                                       WHERE TE.TIPOEVENTOID NOT IN (SELECT TIPOEVENTOID
									                                       FROM [LYCEUM].[Agenda].[TIPOEVENTOCORRELATO])
	                                         AND PLA.ANO = @ANO
                                             AND PLA.PERIODO IN (" + periodo + ") AND TE.TIPOEVENTOID = @TIPOEVENTOID";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPOEVENTOID", RN.Agenda.TipoEvento.TipoEventoAgenda.ProcessoSeletivoAluno);

                agenda = contexto.GetDataTable(contextQuery);
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

            return agenda;
        }

        public DataTable ConsultaDadosGeraisAgenda(int agendaId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @"SELECT A.[AGENDAID],
                                                A.[DESCRICAO],
                                                A.[OBSERVACAO],
                                                A.[PARTICIPAUNIDADEID],
                                                A.[PARTICIPACURSOID],
                                                A.[CURSOPORUNIDADE],
                                                E.[DATAINICIO],
                                                E.[DATAFIM],
                                                E.[TIPOEVENTOID]
                                           FROM [LYCEUM].[Agenda].[AGENDA] A
                                          INNER JOIN [LYCEUM].[Agenda].[EVENTO] E
                                             ON E.[AGENDAID] = A.[AGENDAID]
                                          WHERE E.[TIPOEVENTOID] NOT IN (SELECT TIPOEVENTOID
                                                                           FROM [LYCEUM].[Agenda].[TIPOEVENTOCORRELATO])
                                            AND A.[AGENDAID] = @AGENDAID";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                agenda = contexto.GetDataTable(contextQuery);
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

            return agenda;
        }

        public DataTable ConsultaDadosCursoPorUnidade(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
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
                                            INNER JOIN 
                                            [LYCEUM].[Agenda].[AGENDA_CURSO__AGENDA_UNIDADEENSINO] ACUET
                                            ON ACUET.[AGENDA_CURSO_ID] = ACT.[AGENDA_CURSO_ID]
                                            INNER JOIN [LYCEUM].[Agenda].[AGENDA_UNIDADEENSINO] AUET
                                            ON ACUET.[AGENDA_UNIDADEENSINO_ID] =
                                            AUET.[AGENDA_UNIDADEENSINO_ID]
                                            WHERE ACT.[CURSOID] = AC.[CURSOID]
                                            AND ACT.[AGENDAID] = A.[AGENDAID]
                                            AND ACT.[SERIE] = AC.[SERIE]
                                            AND AUET.[UNIDADEENSINOID] = AUE.UNIDADEENSINOID
                                            AND AUET.[AGENDAID] = A.[AGENDAID]
                                            ORDER BY DESCRICAO
                                            FOR XML PATH('')) TURNOS (TURNOS)
                                            WHERE A.[AGENDAID] = @AGENDAID";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

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

        public DataTable ConsultDadosProcSeletivo(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT P.[NUMEROEDITAL],
                                            P.[DATANASCIMENTOINICIAL],
                                            P.[DATANASCIMENTOFINAL],
                                            P.[NOMEARQUIVOEDITAL]
                                            FROM [LYCEUM].[Agenda].[PROCESSOSELETIVO] P
                                            INNER JOIN [LYCEUM].[Agenda].[AGENDA] A
                                            ON P.[AGENDAID] = A.[AGENDAID]
                                            WHERE A.[AGENDAID] = @AGENDAID";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

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

        public DataTable ListaParamUnidEnsino(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
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
        /// Recupera o arquivo de edital do concurso de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable ListaEdital(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT P.[EDITAL]
                                            FROM [LYCEUM].[Agenda].[PROCESSOSELETIVO] P
                                            INNER JOIN [LYCEUM].[Agenda].[AGENDA] A
                                            ON P.[AGENDAID] = A.[AGENDAID]
                                            WHERE A.[AGENDAID] = @AGENDAID";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

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
        /// Lista todos os eventos de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable ListaEventosAgenda(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
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
        /// Verifica se há candidatos inscritos de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable VerificaInscritos(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT 1
                                        FROM [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO] I
                                        INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] PS
                                        ON I.[PROCESSOSELETIVOID] = PS.[PROCESSOSELETIVOID]
                                        WHERE PS.[AGENDAID] = @AGENDAID";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

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
        /// Exporta os candidatos isncritos de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable ExportaInscritos(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PROCESSOSELETIVOALUNO.PR_CONSULTA_CANDIDATOS_INSCRITOS";
                contextQuery.Parameters.Add("@IDAGENDA", agendaId);
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
        /// Exporta os recursos de provas aplicadas aos candidatos inscritos de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable ExportaRecursosProvas(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PROCESSOSELETIVOALUNO.PR_CONSULTA_RECURSO_APLICACAO_PROVA_CANDIDATOS_INSCRITOS";
                contextQuery.Parameters.Add("@IDAGENDA", agendaId);

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
        /// Exporta os dados dos candidatos inscritos de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable ExportaDadosInscritos(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"PROCESSOSELETIVOALUNO.PR_CONSULTA_UNIDADE_ENSINO_CURSO_TURNO_CANDIDATOS_INSCRITOS";
                contextQuery.Parameters.Add("@IDAGENDA", agendaId);
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
        /// Verifica se há mensagem de email produzida para envio ao candidato de acordo com a agenda
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable VerificaMensagemEmail(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
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
        /// Preapara a mensagem de email para envio ao candidato
        /// </summary>
        /// <param name="agendaId"></param>
        /// <param name="USUARIOID"></param>
        /// <returns></returns>
        public int PreparaEmail(int agendaId, string USUARIOID)
        {
            DataContext ctx = null;
            ContextQuery contextQuery = new ContextQuery();
            int ret;

            try
            {
                ctx = DataContextBuilder.FromLyceum.UsingNoLock();
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

               ret = ctx.ApplyModifications(contextQuery);
            }
            catch (Exception exception)
            {
                if (ctx != null)
                    ctx.Abandon();

                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (ctx != null)
                ctx.Dispose();
            }

            return ret;
        }
        /// <summary>
        /// Seleciona os dados do remtente do email
        /// </summary>
        /// <param name="agendaId"></param>
        /// <returns></returns>
        public DataTable SelecionaDadosEmail(int agendaId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
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
        /// Lista dados dos destinatários do email
        /// </summary>
        /// <param name="idMensagEnvio"></param>
        /// <returns></returns>
        public DataTable ListaDestinatarioEmail(int idMensagEnvio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
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
        /// Lista os campos a serem substituídos na mensagem 
        /// </summary>
        /// <param name="IDENTIFICADORCAMPO"></param>
        /// <returns></returns>
        public DataTable ListaCampoSubstituido(string IDENTIFICADORCAMPO)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT C.IDENTIFICADORCAMPO,
                                          C.NOMECAMPO
                                          FROM [LYCEUM].[dbo].[CAMPOENVIOMENSAGEM] C
                                          WHERE C.IDENTIFICADORCAMPO IN (" + IDENTIFICADORCAMPO + ")";

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
        /// Atualiza a situação de envio da mensagem de email para o candidato
        /// </summary>
        /// <param name="INSCRICAO_ENVIOMENSAGEM_ID"></param>
        /// <returns></returns>
        public int AtualizaSituacaoEnvio(int INSCRICAO_ENVIOMENSAGEM_ID)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret;

            try
            {
                contextQuery.Command = @"UPDATE [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO_ENVIOMENSAGEM]
                                           SET ENVIADO = 1
                                             , DATAENVIO = GETDATE()
                                             , MENSAGEMERRO = ''
                                         WHERE INSCRICAO_ENVIOMENSAGEM_ID = @INSCRICAO_ENVIOMENSAGEM_ID";

                contextQuery.Parameters.Add("@INSCRICAO_ENVIOMENSAGEM_ID", INSCRICAO_ENVIOMENSAGEM_ID);

                ret = ctx.ApplyModifications(contextQuery);
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
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret;

            try
            {
                contextQuery.Command = @"UPDATE [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO_ENVIOMENSAGEM]
                                       SET MENSAGEMERRO = @MENSAGEM
                                       WHERE INSCRICAO_ENVIOMENSAGEM_ID = @INSCRICAO_ENVIOMENSAGEM_ID";

                contextQuery.Parameters.Add("@INSCRICAO_ENVIOMENSAGEM_ID", INSCRICAO_ENVIOMENSAGEM_ID);
                contextQuery.Parameters.Add("@MENSAGEM", MENSAGEM);
                ret = ctx.ApplyModifications(contextQuery);
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

            return ret;
        }
        /// <summary>
        /// Insere no banco o histórico de importação de aqruivos
        /// </summary>
        /// <param name="nomearquivo"></param>
        /// <param name="statusProcessamento"></param>
        /// <param name="tipoimportacaoid"></param>
        /// <param name="tipoentradasistemaid"></param>
        /// <param name="arquivo"></param>
        /// <param name="dataimportacao"></param>
        /// <returns></returns>
        public int InsereHistImportacao(string nomearquivo, string statusProcessamento, int tipoimportacaoid, int tipoentradasistemaid, string arquivo, string dataimportacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret;

            try
            {
                contextQuery.Command = @"INSERT INTO [LYCEUM].[dbo].[HISTORICOIMPORTACAO]
                                        (NOMEARQUIVO, STATUSPROCESSAMENTO, TIPOIMPORTACAOID, TIPOENTRADASISTEMAID, ARQUIVO, DATAIMPORTACAO)
                                        VALUES
	                                    (@NOMEARQUIVO, @STATUSPROCESSAMENTO, @TIPOIMPORTACAOID, @TIPOENTRADASISTEMAID, @ARQUIVO, @DATAIMPORTACAO)";

                contextQuery.Parameters.Add("@NOMEARQUIVO", nomearquivo);
                contextQuery.Parameters.Add("@STATUSPROCESSAMENTO", statusProcessamento);
                contextQuery.Parameters.Add("@TIPOIMPORTACAOID", tipoimportacaoid);
                contextQuery.Parameters.Add("@TIPOENTRADASISTEMAID", tipoentradasistemaid);
                contextQuery.Parameters.Add("@ARQUIVO", arquivo);
                contextQuery.Parameters.Add("@DATAIMPORTACAO", dataimportacao);

                ret = ctx.ApplyModifications(contextQuery);
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

            return ret;
        }
        /// <summary>
        /// Valida o arquivo a ser importado
        /// </summary>
        /// <param name="agendaId"></param>
        /// <param name="numInscricao"></param>
        /// <returns></returns>
        public DataTable ValidaArquivoDeImportacao(int agendaId, int numInscricao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"SELECT 1
                                          FROM [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO] I
                                         INNER JOIN [LYCEUM].[Agenda].[PROCESSOSELETIVO] OS
                                            ON I.[PROCESSOSELETIVOID] = PS.PROCESSOSELETIVOID
                                         WHERE I.[NUMEROINSCRICAO] = @NUMEROINSCRICAO
                                           AND PS.AGENDAID = @AGENDAID";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@NUMEROINSCRICAO", numInscricao);

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
        /// Atualiza a situação do candidato classificado
        /// </summary>
        /// <param name="numInscricao"></param>
        /// <returns></returns>
        public int AtualizaCandidatoClassificado(int numInscricao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret;

            try
            {
                contextQuery.Command = @"UPDATE [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO]
                                        SET SITUACAO = 1
                                        WHERE NUMEROINSCRICAO = @NUMEROINSCRICAO";

                contextQuery.Parameters.Add("@NUMEROINSCRICAO", numInscricao);

                ret = ctx.ApplyModifications(contextQuery);
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

            return ret;
        }
        /// <summary>
        /// Atualiza a situação do candidato inscrito
        /// </summary>
        /// <param name="numInscricao"></param>
        /// <returns></returns>
        public int AtualizaCandidatoInscrito(int numInscricao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            int ret;

            try
            {
                contextQuery.Command = @"UPDATE [LYCEUM].[ProcessoSeletivoAluno].[INSCRICAO]
                                        SET SITUACAO = 0
                                        WHERE NUMEROINSCRICAO = @NUMEROINSCRICAO";

                contextQuery.Parameters.Add("@NUMEROINSCRICAO", numInscricao);

                ret = ctx.ApplyModifications(contextQuery);
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

            return ret;
        }

        public static DataTable ListaAnoPeriodoAgendaPorTipoEventoEDataEvento(int TipoEventoId, DateTime DataEvento)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable agenda = null;

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT
                                        CONVERT(VARCHAR(4), ano) + '-' + CONVERT(VARCHAR(1), periodo) AS ANOPERIODO
                                            FROM agenda.AGENDA      A
                                           INNER JOIN agenda.EVENTO E
                                                ON A.AGENDAID         = E.AGENDAID
                                           INNER JOIN agenda.periodoletivoagenda P ON E.AGENDAID = P.AGENDAID
                                           WHERE   E.TIPOEVENTOID = @TIPOEVENTOID
                                                AND CAST(@DATAEVENTO AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE)
                                      AND     CAST(E.DATAFIM AS DATE)
                                        ORDER BY CONVERT(VARCHAR(4), ano) + '-' + CONVERT(VARCHAR(1), periodo)  ";

                contextQuery.Parameters.Add("@TIPOEVENTOID", TipoEventoId);
                contextQuery.Parameters.Add("@DATAEVENTO", DataEvento);

                agenda = contexto.GetDataTable(contextQuery);
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

            return agenda;
        }

        public int ObtemAgendaAbertaPor(int tipoEvendoId,DateTime dataEvento, int ano, int periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            int agendaVinculadaId = 0;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT DISTINCT A.AGENDAID ,
                                    A.DESCRICAO ,
                                    A.OBSERVACAO ,
                                    A.PARTICIPAUNIDADEID ,
                                    A.PARTICIPACURSOID ,
                                    A.CURSOPORUNIDADE ,
                                    A.DATACADASTRO ,
                                    A.DATAALTERACAO ,
                                    A.USUARIOID
                            FROM    agenda.AGENDA A
                                    INNER JOIN agenda.EVENTO E ON A.AGENDAID = E.AGENDAID
                                    INNER JOIN Agenda.PERIODOLETIVOAGENDA pl ON E.AGENDAID = pl.AGENDAID
                            WHERE   E.TIPOEVENTOID = @TIPOEVENTOID
                                    AND CAST(@DATAEVENTO AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE)
                                                                   AND     CAST(E.DATAFIM AS DATE)
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                            ORDER BY A.AGENDAID  "
                };
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEvendoId);
                contextQuery.Parameters.Add("@DATAEVENTO", dataEvento);


                agendaVinculadaId = contexto.GetReturnValue(contextQuery) == null ? 0 : contexto.GetReturnValue<int>(contextQuery);

                return agendaVinculadaId;
            }

            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

        }
    }
}
