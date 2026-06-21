using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Web;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class HistoricoTurnoVaga : RNBase
    {
        public bool PossuiHistoricoTurnoPor(int tipoHistorico, int ano, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool cadastrado = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    TurnosVagas.HISTORICOTURNOVAGA
                            WHERE   CENSO = @CENSO
                                    AND ANO = @ANO
                                       AND TIPOHISTORICOID = @TIPOHISTORICOID  
                                    AND POSSUIHISTORICOTURNO <> 0 ";

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    cadastrado = true;
                }

                return cadastrado;
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

        public bool PossuiHistoricoTurnoDiretorPor(int tipoHistorico, int ano, string periodos, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool cadastrado = false;

            try
            {
                contextQuery.Command = string.Format(@" SELECT  COUNT(*)
                            FROM    TurnosVagas.HISTORICOTURNOVAGA
                            WHERE   CENSO = @CENSO
                                    AND ANO = @ANO
                                    AND PERIODO IN ( {0} )
                                    AND TIPOHISTORICOID = @TIPOHISTORICOID  
                                    AND POSSUIHISTORICOTURNO <> 0 ", periodos);

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    cadastrado = true;
                }

                return cadastrado;
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

        public bool PossuiHistoricoTurnoPor(int tipoHistorico, int ano, int periodo, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool cadastrado = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    TurnosVagas.HISTORICOTURNOVAGA
                            WHERE   CENSO = @CENSO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND TIPOHISTORICOID = @TIPOHISTORICOID  
                                    AND POSSUIHISTORICOTURNO <> 0 ";

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    cadastrado = true;
                }

                return cadastrado;
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

        public bool PossuiHistoricoVagaDiretorPor(int tipoHistorico, int ano, string periodos, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool cadastrado = false;

            try
            {
                contextQuery.Command = string.Format(@" SELECT  COUNT(*)
                            FROM    TurnosVagas.HISTORICOTURNOVAGA
                            WHERE   CENSO = @CENSO
                                    AND ANO = @ANO
                                    AND PERIODO IN ( {0} )
                                    AND TIPOHISTORICOID = @TIPOHISTORICOID  
                                    AND POSSUIHISTORICOVAGA <> 0 ", periodos);

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    cadastrado = true;
                }

                return cadastrado;
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

        public bool PossuiHistoricoVagaPor(int tipoHistorico, int ano, int periodo, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool cadastrado = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    TurnosVagas.HISTORICOTURNOVAGA
                            WHERE   CENSO = @CENSO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND TIPOHISTORICOID = @TIPOHISTORICOID  
                                    AND POSSUIHISTORICOVAGA <> 0 ";

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    cadastrado = true;
                }

                return cadastrado;
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

        public void InsereHistoricoTurno(DataContext ctx, int ano, int periodo, string censo, int tipoHistorico)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            string usuarioResponsavel = HttpContext.Current.User.Identity.Name;

            try
            {
                contextQuery.Command = @" INSERT  INTO TURNOSVAGAS.HISTORICOTURNOVAGA
                        ( ANO ,
                          PERIODO ,
                          CENSO ,
                          TIPOHISTORICOID ,
                          POSSUIHISTORICOTURNO ,
                          POSSUIHISTORICOVAGA ,
                          DATACADASTRO ,
                          MATRICULA
                        )
                        SELECT  DISTINCT
                                A.ANO ,
                                A.PERIODO ,
                                TI.CENSO ,
                                @TIPOHISTORICOID ,
                                CASE WHEN ( SELECT  COUNT(*)
                                            FROM    DBO.TCE_CTV_CONF_TURNO T
                                                    INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A2 ON T.ID_AGENDA_CONF_TURNO_VAGA = A2.ID_AGENDA_CONF_TURNO_VAGA
                                            WHERE   T.CENSO = TI.CENSO
                                                    AND A2.ANO = A.ANO
                                                    AND A2.ENCERRADO = 0
                                                    AND A2.PERIODO = @PERIODO
                                          ) > 0 THEN 1
                                     ELSE 2
                                END POSSUIHISTORICOTURNO ,
                                0 AS POSSUIHISTORICOVAGA ,
                                GETDATE() ,
                                @MATRICULA
                        FROM    DBO.TCE_CTV_CONF_TURNO_INICIAL TI
                                INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON TI.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                        WHERE   TI.CENSO = @CENSO
                                AND ANO = @ANO
                                AND PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@MATRICULA", usuarioResponsavel);

                ctx.ApplyModifications(contextQuery);
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
        }

        public void InsereHistoricoTurnoDiretor(DataContext ctx, int ano, string periodos, string censo, int tipoHistorico)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            string usuarioResponsavel = HttpContext.Current.User.Identity.Name;

            try
            {
                contextQuery.Command = string.Format(@" INSERT  INTO TURNOSVAGAS.HISTORICOTURNOVAGA
                        ( ANO ,
                          PERIODO ,
                          CENSO ,
                          TIPOHISTORICOID ,
                          POSSUIHISTORICOTURNO ,
                          POSSUIHISTORICOVAGA ,
                          DATACADASTRO ,
                          MATRICULA
                        )
                        SELECT  DISTINCT
                                A.ANO ,
                                A.PERIODO ,
                                TI.CENSO ,
                                @TIPOHISTORICOID ,
                                CASE WHEN ( SELECT  COUNT(*)
                                            FROM    DBO.TCE_CTV_CONF_TURNO T
                                                    INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A2 ON T.ID_AGENDA_CONF_TURNO_VAGA = A2.ID_AGENDA_CONF_TURNO_VAGA
                                            WHERE   T.CENSO = TI.CENSO
                                                    AND A2.ANO = A.ANO
                                                    AND A2.ENCERRADO = 0
                                                    AND A2.PERIODO IN ( {0} )
                                          ) > 0 THEN 1
                                     ELSE 2
                                END POSSUIHISTORICOTURNO ,
                                0 AS POSSUIHISTORICOVAGA ,
                                GETDATE() ,
                                @MATRICULA
                        FROM    DBO.TCE_CTV_CONF_TURNO_INICIAL TI
                                INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON TI.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                        WHERE   TI.CENSO = @CENSO
                                AND ANO = @ANO
                                AND PERIODO IN ( {0} ) ", periodos);

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@MATRICULA", usuarioResponsavel);

                ctx.ApplyModifications(contextQuery);
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
        }

        public void AtualizaHistoricoVaga(DataContext ctx, int ano, string periodos, string censo, int tipoHistorico)
        {
            List<DTOs.DadosHistoricoTurnoVaga> listaHistoricos = new List<Techne.Lyceum.RN.DTOs.DadosHistoricoTurnoVaga>();
            string usuarioResponsavel = HttpContext.Current.User.Identity.Name;

            try
            {
                //Busca historicos que ja estao cadastrados para turnos
                listaHistoricos = ObtemDadosHistoricoTurnoVaga(ano, periodos, censo, tipoHistorico);

                //Atualiza os historicos com informaçoes de vagas
                foreach (DTOs.DadosHistoricoTurnoVaga historico in listaHistoricos)
                {
                    this.AtualizaHistoricoVaga(ctx, historico, usuarioResponsavel);
                }
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
        }

        public List<DTOs.DadosHistoricoTurnoVaga> ObtemDadosHistoricoTurnoVaga(int ano, string periodos, string censo, int tipoHistorico)
        {
            List<DTOs.DadosHistoricoTurnoVaga> listaHistoricoTurnoVaga = new List<DTOs.DadosHistoricoTurnoVaga>();
            DTOs.DadosHistoricoTurnoVaga historico = new Techne.Lyceum.RN.DTOs.DadosHistoricoTurnoVaga();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = string.Format(@" SELECT  CASE WHEN ( SELECT  COUNT(*)
                                            FROM    DBO.TCE_CTV_CONF_VAGA V
                                                    INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A2 ON V.ID_AGENDA_CONF_TURNO_VAGA = A2.ID_AGENDA_CONF_TURNO_VAGA
                                            WHERE   V.CENSO = H.CENSO
                                                    AND A2.ANO = H.ANO
                                                    AND A2.ENCERRADO = 0
                                                    AND A2.PERIODO IN ( {0} )
                                            
                                          ) > 0 THEN 1
                                     ELSE 2
                                END POSSUIHISTORICOVAGA ,
                                ANO ,
                                PERIODO ,
                                HISTORICOTURNOVAGAID
                        FROM    TURNOSVAGAS.HISTORICOTURNOVAGA H
                        WHERE   H.CENSO = @CENSO
                                AND ANO = @ANO
                                AND TIPOHISTORICOID = @TIPOHISTORICOID
                                AND PERIODO IN ( {0} ) ", periodos);

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    historico = new DTOs.DadosHistoricoTurnoVaga();

                    historico.PossuiHistoricoVaga = Convert.ToInt32(reader["POSSUIHISTORICOVAGA"]);
                    historico.Ano = Convert.ToInt32(reader["ANO"]);
                    historico.Periodo = Convert.ToInt32(reader["PERIODO"]);
                    historico.HistoricoTurnoVagaId = Convert.ToInt32(reader["HISTORICOTURNOVAGAID"]);

                    listaHistoricoTurnoVaga.Add(historico);
                }

                return listaHistoricoTurnoVaga;
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

        private void AtualizaHistoricoVaga(DataContext ctx, DTOs.DadosHistoricoTurnoVaga historico, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();

            try
            {
                contextQuery.Command = @" UPDATE  TURNOSVAGAS.HISTORICOTURNOVAGA
                        SET     POSSUIHISTORICOVAGA = @POSSUIHISTORICOVAGA ,
                                DATAALTERACAO = GETDATE() ,
                                MATRICULA = @MATRICULA
                        WHERE   HISTORICOTURNOVAGAID = @HISTORICOTURNOVAGAID ";

                contextQuery.Parameters.Add("@POSSUIHISTORICOVAGA", historico.PossuiHistoricoVaga);
                contextQuery.Parameters.Add("@MATRICULA", usuarioResponsavel);
                contextQuery.Parameters.Add("@HISTORICOTURNOVAGAID", historico.HistoricoTurnoVagaId);

                ctx.ApplyModifications(contextQuery);
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
        }
    }
}
