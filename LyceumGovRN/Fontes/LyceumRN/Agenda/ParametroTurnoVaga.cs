using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN.Agenda
{
    public class ParametroTurnoVaga : RNBase
    {
        #region Propriedades e Enum

        public const int NumeroMinimoTurnos = 1;

        public enum ParametroTurnoVagaAlterarTurnoModalidade
        {
            [StringValue("Não permite")]
            NaoPermite = 0,
            [StringValue("Permite")]
            Permite = 1,
            [StringValue("Permite com restrição")]
            PermiteComRestricao = 2
        }
        #endregion

        public Entidades.ParametroTurnoVaga ObtemPor(int perfilId, int agendaId)
        {
            Entidades.ParametroTurnoVaga parametroTurnoVaga = new Entidades.ParametroTurnoVaga();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = default(SqlDataReader);

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  PARAMETROTURNOVAGAID ,
                                        ALTERARTURNOMODALIDADE , --0 Não permite ,1 Permite ,2 Permite com restrição [FE3:1,2,3]
                                        VARIACAOTURNO , -- De 1 a 5 turnos [FE3:3]
                                        PERCENTUALAUMENTOVAGA , -- De 0% a n%
                                        PERCENTUALDIMINUICAOVAGA , -- De 0% a n%
                                        PERCENTUALCRIACAOTURMA , -- De 0% a n%
                                        REMOVETURNOINTEIRO , -- 0 Não permite, 1 Permite [FE3:4]       
                                        EDITARTURNOFINALIZADO , -- 0 Não permite, 1 Permite [RN10]
                                        EDITARVAGAFINALIZADA ,-- 0 Não permite, 1 Permite [RN20]
                                        CONFIGURACAOPADRAO,
                                        PODEANALISAR,
                                        PODETURMAPROVISORIA,
                                        POSSUILIMITETURMAPROVISORIA
                                FROM    LYCEUM.Agenda.PARAMETROTURNOVAGA
                                WHERE   AGENDAID = @AGENDAID --<AgendaID da Agenda de Confirmação>
                                        AND PERFILID = @PERFILID ---<Perfil do usuário logado> "
                };

                contextQuery.Parameters.Add("@PERFILID", perfilId);
                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    parametroTurnoVaga.PerfilId = perfilId;
                    parametroTurnoVaga.AgendaId = agendaId;
                    parametroTurnoVaga.ParametroTurnoVagaId = Convert.ToInt32(reader["PARAMETROTURNOVAGAID"]);
                    parametroTurnoVaga.AlterarTurnoModalidade = Convert.ToInt32(reader["ALTERARTURNOMODALIDADE"]);
                    parametroTurnoVaga.VariacaoTurno = Convert.ToInt32(reader["VARIACAOTURNO"]);
                    parametroTurnoVaga.PercentualAumentoVaga = Convert.ToDecimal(reader["PERCENTUALAUMENTOVAGA"]);
                    parametroTurnoVaga.PercentualDiminuicaoVaga = Convert.ToDecimal(reader["PERCENTUALDIMINUICAOVAGA"]);
                    parametroTurnoVaga.PercentualCriacaoTurma = Convert.ToDecimal(reader["PERCENTUALCRIACAOTURMA"]);                   
                    parametroTurnoVaga.RemoveTurnoInteiro = Convert.ToBoolean(reader["REMOVETURNOINTEIRO"]);
                    parametroTurnoVaga.EditarTurnoFinalizado = Convert.ToBoolean(reader["EDITARTURNOFINALIZADO"]);
                    parametroTurnoVaga.ConfiguracaoPadrao = Convert.ToBoolean(reader["CONFIGURACAOPADRAO"]);
                    parametroTurnoVaga.EditarVagaFinalizada = Convert.ToBoolean(reader["EDITARVAGAFINALIZADA"]);
                    parametroTurnoVaga.PodeAnalisar = Convert.ToBoolean(reader["PODEANALISAR"]);
                    parametroTurnoVaga.PodeTurmaProvisoria = Convert.ToBoolean(reader["PODETURMAPROVISORIA"]);
                    parametroTurnoVaga.PossuiLimiteTurmaProvisoria = Convert.ToBoolean(reader["POSSUILIMITETURMAPROVISORIA"]);
                }

                return parametroTurnoVaga;
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

        public Entidades.ParametroTurnoVaga ObtemPadraoPor(int agendaId)
        {
            Entidades.ParametroTurnoVaga parametroTurnoVaga = new Entidades.ParametroTurnoVaga();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  PARAMETROTURNOVAGAID ,
                                ALTERARTURNOMODALIDADE , --0 Não permite ,1 Permite ,2 Permite com restrição [FE3:1,2,3]
                                VARIACAOTURNO , -- De 1 a 5 turnos [FE3:3]	
                                PERCENTUALAUMENTOVAGA , -- De 0% a n%
                                PERCENTUALDIMINUICAOVAGA , -- De 0% a n%
                                PERCENTUALCRIACAOTURMA , -- De 0% a n%
                                PERFILID ,
                                REMOVETURNOINTEIRO , -- 0 Não permite, 1 Permite [FE3:4]
                                EDITARTURNOFINALIZADO , -- 0 Não permite, 1 Permite [RN10]
                                EDITARVAGAFINALIZADA , -- 0 Não permite, 1 Permite [RN20]
                                PODEANALISAR,
                                PODETURMAPROVISORIA,
                                POSSUILIMITETURMAPROVISORIA
                        FROM    LYCEUM.Agenda.PARAMETROTURNOVAGA
                        WHERE   AGENDAID = @AGENDAID --<AgendaID da Agenda de Confirmação>
                                AND CONFIGURACAOPADRAO = 1 "
                };

                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    parametroTurnoVaga.AgendaId = agendaId;
                    parametroTurnoVaga.ConfiguracaoPadrao = true;
                    parametroTurnoVaga.ParametroTurnoVagaId = Convert.ToInt32(reader["PARAMETROTURNOVAGAID"]);
                    parametroTurnoVaga.AlterarTurnoModalidade = Convert.ToInt32(reader["ALTERARTURNOMODALIDADE"]);
                    parametroTurnoVaga.VariacaoTurno = Convert.ToInt32(reader["VARIACAOTURNO"]);
                    parametroTurnoVaga.PercentualAumentoVaga = Convert.ToDecimal(reader["PERCENTUALAUMENTOVAGA"]);
                    parametroTurnoVaga.PercentualDiminuicaoVaga = Convert.ToDecimal(reader["PERCENTUALDIMINUICAOVAGA"]);
                    parametroTurnoVaga.PercentualCriacaoTurma = Convert.ToDecimal(reader["PERCENTUALCRIACAOTURMA"]);
                    parametroTurnoVaga.PerfilId = Convert.ToInt32(reader["PERFILID"]);                    
                    parametroTurnoVaga.RemoveTurnoInteiro = Convert.ToBoolean(reader["REMOVETURNOINTEIRO"]);
                    parametroTurnoVaga.EditarTurnoFinalizado = Convert.ToBoolean(reader["EDITARTURNOFINALIZADO"]);                   
                    parametroTurnoVaga.EditarVagaFinalizada = Convert.ToBoolean(reader["EDITARVAGAFINALIZADA"]);
                    parametroTurnoVaga.PodeAnalisar = Convert.ToBoolean(reader["PODEANALISAR"]);
                    parametroTurnoVaga.PodeTurmaProvisoria = Convert.ToBoolean(reader["PODETURMAPROVISORIA"]);
                    parametroTurnoVaga.PossuiLimiteTurmaProvisoria = Convert.ToBoolean(reader["POSSUILIMITETURMAPROVISORIA"]);
                }

                return parametroTurnoVaga;
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

        public DataTable ListaPerfilPodeAnalisarTurnosVagasPor(string usuario, bool privilegiado)
        {
            //Lista perfis que podem analisar turnos vagas de agendas que estejam abertas
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable perfis = null;

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT
                                PE.ID_PERFIL AS PERFILID ,
                                PE.DESCRICAO
                        FROM    HADES.DBO.HD_PADUSUARIO PU ( NOLOCK )
                                INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP ( NOLOCK ) ON PU.PADACES = PP.PADACES
                                INNER JOIN HADES.DBO.TCE_PERFIL PE ( NOLOCK ) ON PP.ID_PERFIL = PE.ID_PERFIL
                                INNER JOIN AGENDA.PARAMETROTURNOVAGA PT ( NOLOCK ) ON PT.PERFILID = PE.ID_PERFIL
                        WHERE   pt.PODEANALISAR = 1
                                AND ( pu.USUARIO = @USUARIO
                                      OR @PRIVILEGIADO = 1 --Usuario privilegiado              
                                    )
                                AND EXISTS ( SELECT TOP 1
                                                    1
                                             FROM   agenda.AGENDA A ( NOLOCK )
                                                    INNER JOIN agenda.EVENTO E ( NOLOCK ) ON A.AGENDAID = E.AGENDAID
                                             WHERE  E.TIPOEVENTOID IN ( @TIPOEVENTOIDTURNO,
                                                                        @TIPOEVENTOIDVAGA )
                                                    AND CAST(GETDATE() AS DATE) BETWEEN CAST(E.DATAINICIO AS DATE)
                                                                                AND   CAST(E.DATAFIM AS DATE)
                                                    AND A.AGENDAID = pt.AGENDAID )
                        ORDER BY PE.DESCRICAO DESC ";

                contextQuery.Parameters.Add("@TIPOEVENTOIDTURNO", (int)RN.Agenda.TipoEvento.TipoEventoAgenda.AnaliseTurnos);
                contextQuery.Parameters.Add("@TIPOEVENTOIDVAGA", (int)RN.Agenda.TipoEvento.TipoEventoAgenda.AnaliseVagas);
                contextQuery.Parameters.Add("@PRIVILEGIADO", privilegiado);
                contextQuery.Parameters.Add("@USUARIO", usuario);

                perfis = ctx.GetDataTable(contextQuery);
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

            return perfis;
        }

        public bool RetornaPodeTurmaProvisoriaPor(int agendaId, int perfilId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT PODETURMAPROVISORIA
                                FROM [Agenda].[PARAMETROTURNOVAGA]
                                WHERE AGENDAID = @AGENDAID
                                    AND PERFILID = @PERFILID ";

                contextQuery.Parameters.Add("@AGENDAID", SqlDbType.Int, agendaId);
                contextQuery.Parameters.Add("@PERFILID", SqlDbType.Int, perfilId);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToBoolean(reader["PODETURMAPROVISORIA"]);
                }

                return retorno;
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
    }
}