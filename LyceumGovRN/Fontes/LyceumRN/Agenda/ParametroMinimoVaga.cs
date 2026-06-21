using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using System.Data;
using Seeduc.Infra.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Agenda
{
    public class ParametroMinimoVaga : RNBase
    {
        public enum TipoSerie 
        {
            [StringValue("Série de Entrada")]
            SerieEntrada = 0,
            [StringValue("Demais Séries")]
            DemaisSeries = 1
        }

        public enum TipoVaga
        {
            [StringValue("Vagas de Continuidade")]
            VC = 0,
            [StringValue("Vagas Novas")]
            VN = 1
        }


        public List<Entidades.ParametroMinimoVaga> ObtemPor(int agendaId, int codPerfil)
        {
            List<Entidades.ParametroMinimoVaga> listaMinimoVaga = new List<Techne.Lyceum.RN.Agenda.Entidades.ParametroMinimoVaga>();
            Entidades.ParametroMinimoVaga parametroMinimoVaga = new Entidades.ParametroMinimoVaga();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  M.PARAMETROMINIMOVAGAID ,
                                        M.PARAMETROTURNOVAGAID ,
                                        M.TIPOVAGAID , --VC = 0, VN = 1
                                        M.TIPOSERIEID , -- 0-Série de Entrada, 1-Demais Séries
                                        M.QUANTIDADEVAGAS -- Minimo de Vagas
                                FROM    LYCEUM.AGENDA.PARAMETROTURNOVAGA P
                                        INNER JOIN AGENDA.PARAMETROMINIMOVAGA M ON P.PARAMETROTURNOVAGAID = M.PARAMETROTURNOVAGAID
                                WHERE   AGENDAID = @AGENDAID --<AgendaID da Agenda de Confirmação>
                                        AND PERFILID = @PERFILID --<Perfil do usuário logado> ";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@PERFILID", codPerfil);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    parametroMinimoVaga = new RN.Agenda.Entidades.ParametroMinimoVaga();

                    parametroMinimoVaga.ParametroMinimoVagaId = Convert.ToInt32(reader["PARAMETROMINIMOVAGAID"]);
                    parametroMinimoVaga.ParametroTurnoVagaId = Convert.ToInt32(reader["PARAMETROTURNOVAGAID"]);
                    parametroMinimoVaga.TipoSerieId = Convert.ToInt32(reader["TIPOSERIEID"]);
                    parametroMinimoVaga.TipoVagaId = Convert.ToInt32(reader["TIPOVAGAID"]);
                    parametroMinimoVaga.QuantidadeVagas = Convert.ToInt32(reader["QUANTIDADEVAGAS"]);

                    listaMinimoVaga.Add(parametroMinimoVaga);
                }

                return listaMinimoVaga;
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

        public List<Entidades.ParametroMinimoVaga> ObtemPadraoPor(int agendaId)
        {
            List<Entidades.ParametroMinimoVaga> listaMinimoVaga = new List<Techne.Lyceum.RN.Agenda.Entidades.ParametroMinimoVaga>();
            Entidades.ParametroMinimoVaga parametroMinimoVaga = new Entidades.ParametroMinimoVaga();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  M.PARAMETROMINIMOVAGAID ,
                                        M.PARAMETROTURNOVAGAID ,
                                        M.TIPOVAGAID , --VC = 0, VN = 1
                                        M.TIPOSERIEID , -- 0-Série de Entrada, 1-Demais Séries
                                        M.QUANTIDADEVAGAS -- Minimo de Vagas
                                FROM    LYCEUM.AGENDA.PARAMETROTURNOVAGA P
                                        INNER JOIN AGENDA.PARAMETROMINIMOVAGA M ON P.PARAMETROTURNOVAGAID = M.PARAMETROTURNOVAGAID
                                WHERE   AGENDAID = @AGENDAID --<AgendaID da Agenda de Confirmação>        
                                        AND CONFIGURACAOPADRAO = 1  ";

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    parametroMinimoVaga = new RN.Agenda.Entidades.ParametroMinimoVaga();

                    parametroMinimoVaga.ParametroMinimoVagaId = Convert.ToInt32(reader["PARAMETROMINIMOVAGAID"]);
                    parametroMinimoVaga.ParametroTurnoVagaId = Convert.ToInt32(reader["PARAMETROTURNOVAGAID"]);
                    parametroMinimoVaga.TipoSerieId = Convert.ToInt32(reader["TIPOSERIEID"]);
                    parametroMinimoVaga.TipoVagaId = Convert.ToInt32(reader["TIPOVAGAID"]);
                    parametroMinimoVaga.QuantidadeVagas = Convert.ToInt32(reader["QUANTIDADEVAGAS"]);

                    listaMinimoVaga.Add(parametroMinimoVaga);
                }

                return listaMinimoVaga;
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
