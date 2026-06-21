using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.DTOs;
using System.Web;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class HistoricoTurno : RNBase
    {
        public void SalvaHistoricoTurnoDiretor(DataContext ctx, int ano, string periodos, string censo)
        {
            RN.TurnosVagas.HistoricoTurnoVaga rnHistoricoTurnoVaga = new RN.TurnosVagas.HistoricoTurnoVaga();
            RN.TurnosVagas.HistoricoJustificativa rnHistoricoJustificativa = new RN.TurnosVagas.HistoricoJustificativa();
            int tipoHistoricoDiretor = (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Diretor;

            try
            {
                //Apenas salva caso nao exista cadastrado outro historico de turno do diretor
                if (!rnHistoricoTurnoVaga.PossuiHistoricoTurnoDiretorPor(tipoHistoricoDiretor, ano, periodos, censo))
                {
                    rnHistoricoTurnoVaga.InsereHistoricoTurnoDiretor(ctx, ano, periodos, censo, tipoHistoricoDiretor);
                    this.InsereHistoricoTurno(ctx, ano, periodos, censo, tipoHistoricoDiretor);
                    rnHistoricoJustificativa.InsereHistoricoJustificativa(ctx, ano, periodos, censo, true, false, tipoHistoricoDiretor);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SalvaHistoricoTurnoSeeduc(DataContext ctx, int ano, string periodos, string censo)
        {
            RN.TurnosVagas.HistoricoTurnoVaga rnHistoricoTurnoVaga = new RN.TurnosVagas.HistoricoTurnoVaga();
            RN.TurnosVagas.HistoricoJustificativa rnHistoricoJustificativa = new RN.TurnosVagas.HistoricoJustificativa();
            int tipoHistoricoSeeduc = (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Seeduc;

            try
            {
                //Verifica se os periodos que estão sendo salvos ja existem
                if (periodos.Contains('0'))
                {
                    int periodo = 0;
                    
                    if (!rnHistoricoTurnoVaga.PossuiHistoricoTurnoPor(tipoHistoricoSeeduc, ano, periodo, censo))                    
                    {
                        rnHistoricoTurnoVaga.InsereHistoricoTurno(ctx, ano, periodo, censo, tipoHistoricoSeeduc);
                    }
                }

                if (periodos.Contains('1'))
                {
                    int periodo = 1;
                    
                    if (!rnHistoricoTurnoVaga.PossuiHistoricoTurnoPor(tipoHistoricoSeeduc, ano, periodo, censo))                    
                    {
                        rnHistoricoTurnoVaga.InsereHistoricoTurno(ctx, ano, periodo, censo, tipoHistoricoSeeduc);
                    }
                }

                if (periodos.Contains('2'))
                {
                    int periodo = 2;
                    
                    if (!rnHistoricoTurnoVaga.PossuiHistoricoTurnoPor(tipoHistoricoSeeduc, ano, periodo, censo))
                    {
                        rnHistoricoTurnoVaga.InsereHistoricoTurno(ctx, ano, periodo, censo, tipoHistoricoSeeduc);
                    }
                }

                //Apaga o historico de turno anterior do tipo Seeduc
                this.DeletaHistoricoTurno(ctx, ano, periodos, censo, tipoHistoricoSeeduc);
                rnHistoricoJustificativa.DeletaHistoricoJustificativa(ctx, ano, periodos, censo, true, false, tipoHistoricoSeeduc);

                //Insere
                this.InsereHistoricoTurno(ctx, ano, periodos, censo, tipoHistoricoSeeduc);
                rnHistoricoJustificativa.InsereHistoricoJustificativa(ctx, ano, periodos, censo, true, false, tipoHistoricoSeeduc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereHistoricoTurno(DataContext ctx, int ano, string periodos, string censo, int tipoHistorico)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            string usuarioResponsavel = HttpContext.Current.User.Identity.Name;

            try
            {
                sql.Append(@" INSERT  INTO TurnosVagas.HISTORICOTURNO
                            ( ID_AGENDA_CONF_TURNO_VAGA ,
                              HISTORICOTURNOVAGAID ,
                              CENSO ,
                              TURNO ,
                              CONTINUIDADE ,
                              NOVO ,
                              CONFIRMADA ,
                              MATRICULA ,
                              DATACADASTRO ,
                              DATAALTERACAO
                            ) ");

                if (tipoHistorico == (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Diretor)
                {
                    sql.Append(string.Format(@" SELECT  DISTINCT
                                        T.ID_AGENDA_CONF_TURNO_VAGA ,
                                        H.HISTORICOTURNOVAGAID ,
                                        T.CENSO ,
                                        T.TURNO ,
                                        T.CONTINUIDADE ,
                                        T.NOVO ,
                                        T.CONFIRMADA ,
                                        T.MATRICULA ,
                                        T.DT_CADASTRO ,
                                        T.DT_ALTERACAO
                                FROM    DBO.TCE_CTV_CONF_TURNO T
                                        INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON T.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                        INNER JOIN TurnosVagas.HISTORICOTURNOVAGA H ON A.ANO = H.ANO
                                                                                       AND A.PERIODO = H.PERIODO
                                                                                       AND T.CENSO = H.CENSO
                                WHERE   T.CENSO = @CENSO
                                        AND A.ANO = @ANO
                                        AND A.PERIODO IN ( {0} )
		                                AND H.TIPOHISTORICOID = @TIPOHISTORICOID ", periodos));
                }

                if (tipoHistorico == (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Seeduc)
                {
                    sql.Append(string.Format(@" SELECT  DISTINCT
                                T.ID_AGENDA_CONF_TURNO_VAGA ,
                                H.HISTORICOTURNOVAGAID ,
                                T.CENSO ,
                                T.TURNO ,
                                T.CONTINUIDADE ,
                                T.NOVO ,
                                T.CONFIRMADA ,
                                @MATRICULA ,
                                T.DT_CADASTRO ,
                                GETDATE()
                        FROM    DBO.TCE_CTV_CONF_TURNO T
                                INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON T.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN TurnosVagas.HISTORICOTURNOVAGA H ON A.ANO = H.ANO
                                                                               AND A.PERIODO = H.PERIODO
                                                                               AND T.CENSO = H.CENSO
                        WHERE   T.CENSO = @CENSO
                                AND A.ANO = @ANO
                                AND A.PERIODO IN ( {0} )
		                        AND H.TIPOHISTORICOID = @TIPOHISTORICOID ", periodos));
                }
                contextQuery.Command = sql.ToString();

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

        public void DeletaHistoricoTurno(DataContext ctx, int ano, string periodos, string censo, int tipoHistorico)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = string.Format(@" DELETE  T
                                        FROM    TurnosVagas.HISTORICOTURNO T
                                                INNER JOIN TurnosVagas.HISTORICOTURNOVAGA H ON T.HISTORICOTURNOVAGAID = H.HISTORICOTURNOVAGAID
                                        WHERE   H.TIPOHISTORICOID = @TIPOHISTORICOID
                                                AND T.CENSO = @CENSO
                                                AND H.ANO = @ANO
                                                AND H.PERIODO IN ( {0} ) ", periodos);

                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);

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

        public ICollection<DadosConfTurno> ListaQuadroHistoricoTurnos(string censo, int ano, int tipoHistorico)
        {
            List<DadosConfTurno> turnos = new List<DadosConfTurno>();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            DadosConfTurno dadosConfTurno = new DadosConfTurno();
            List<string> codigoTurnos = new List<string>();
            List<string> codigoTurnosNovo = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(censo) || ano == 0)
                {
                    return null;
                }

                contextQuery.Command = @" SELECT DISTINCT
                                Ti.ID_AGENDA_CONF_TURNO_VAGA ,
                                a.AGENDAID ,
                                A.PERIODO ,
                                TI.CENSO ,
                                A.CURSO ,
                                c.TIPO AS 'CODIGO_TIPO' ,
                                tc.DESCRICAO AS 'DESCRICAO_TIPO' ,
                                C.MODALIDADE AS 'CODIGO_MODALIDADE' ,
                                MC.DESCRICAO AS 'MODALIDADE' ,
                                C.NOME AS 'NOME_CURSO' ,
                                A.SERIE ,
                                TurnosVagas.FN_HISTORICOTURNOSNOVO(TI.ID_AGENDA_CONF_TURNO_VAGA,
                                                                   TI.CENSO, @TIPOHISTORICOID) AS 'TURNOS_NOVO' ,
                                TurnosVagas.FN_HISTORICOTURNOSCONTINUIDADE(TI.ID_AGENDA_CONF_TURNO_VAGA,
                                                                           TI.CENSO, @TIPOHISTORICOID) AS 'TURNOS_CONTINUIDADE' ,
                                J.JUSTIFICATIVACONTINUIDADE AS JUSTIFICATIVA_CONTINUIDADE ,
                                J.JUSTIFICATIVANOVO AS JUSTIFICATIVA_NOVO
                        FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA a
                                INNER JOIN LY_CURSO C ON C.CURSO = A.CURSO
                                INNER JOIN dbo.LY_TIPO_CURSO tc ON c.TIPO = tc.TIPO
                                INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                                INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON TI.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN TurnosVagas.HISTORICOTURNOVAGA tv ON tv.ANO = a.ANO
                                                                                AND tv.PERIODO = a.PERIODO
                                                                                AND tv.CENSO = ti.CENSO
                                                                                AND tv.TIPOHISTORICOID = @TIPOHISTORICOID
                                LEFT JOIN TurnosVagas.HISTORICOJUSTIFICATIVA j ON ti.ID_AGENDA_CONF_TURNO_VAGA = j.ID_AGENDA_CONF_TURNO_VAGA
                                                                                  AND ti.CENSO = j.CENSO
                                                                                  AND j.TURNO = 1
                                                                                  AND j.HISTORICOTURNOVAGAID = tv.HISTORICOTURNOVAGAID
                        WHERE   TI.CENSO = @CENSO
                                AND A.ANO = @ANO
                                AND NOT EXISTS ( SELECT 1
                                                 FROM   DBO.TCE_CTV_RESTRICAO RE
                                                 WHERE  ID_AGENDA_CONF_TURNO_VAGA = Ti.ID_AGENDA_CONF_TURNO_VAGA
                                                        AND Ti.CENSO = RE.CENSO ) 
                        ORDER BY A.PERIODO ,
                                C.MODALIDADE ,
                                C.NOME ,
                                A.SERIE";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    codigoTurnos = Convert.ToString(reader["TURNOS_CONTINUIDADE"])
                                                  .Split(new[]
                                                             {
                                                                 ";"
                                                             }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(x => x.Trim())
                                                  .ToList();
                    codigoTurnosNovo = Convert.ToString(reader["TURNOS_NOVO"])
                                              .Split(new[]
                                                             {
                                                                 ";"
                                                             }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(x => x.Trim())
                                              .ToList();

                    dadosConfTurno = new DadosConfTurno
                    {
                        AgendaId = Convert.ToInt32(reader["AGENDAID"]),
                        IdAgendaConfTurnoVaga = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]),
                        Periodo = Convert.ToInt32(reader["PERIODO"]),
                        Curso = Convert.ToString(reader["CURSO"]),
                        DescricaoTipo = Convert.ToString(reader["DESCRICAO_TIPO"]),
                        CodigoTipo = Convert.ToString(reader["CODIGO_TIPO"]),
                        Justificativa = Convert.ToString(reader["JUSTIFICATIVA_CONTINUIDADE"]),
                        JustificativaNovo = Convert.ToString(reader["JUSTIFICATIVA_NOVO"]),
                        Modalidade = Convert.ToString(reader["MODALIDADE"]),
                        CodigoModalidade = Convert.ToString(reader["CODIGO_MODALIDADE"]),
                        NomeCurso = Convert.ToString(reader["NOME_CURSO"]),
                        Serie = Convert.ToInt32(reader["SERIE"]),
                        //DescricaoSerie = Convert.ToString(reader["DESCRICAO_SERIE"]),
                        Censo = Convert.ToString(reader["CENSO"])
                    };

                    dadosConfTurno.CarregarTurnos(codigoTurnos, codigoTurnosNovo);

                    turnos.Add(dadosConfTurno);
                }

                return turnos;
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
