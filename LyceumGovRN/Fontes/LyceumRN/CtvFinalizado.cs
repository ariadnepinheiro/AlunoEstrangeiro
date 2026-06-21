using System;
using System.Collections.Generic;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class CtvFinalizado : RNBase
    {
        public static bool VerificaTurnoParcial(int ano, int periodo, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                                       {
                                           Command =
                                               @" SELECT TOP 1
                                        1
                                FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA a
                                WHERE  ANO=@ANO 
                                       AND PERIODO = @PERIODO
                                       AND CONVERT(DATE, GETDATE()) BETWEEN a.DT_INICIO_CONF_TURNO
                                                                 AND     a.DT_FIM_CONF_TURNO
                                        AND EXISTS ( SELECT 1
                                                         FROM   TCE_CTV_FINALIZADO
                                                         WHERE  TCE_CTV_FINALIZADO.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA 
                                                         AND CENSO = @CENSO
                                                         AND TURNO = 1 ) "
                                       };
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool VerificaTurnoFinalizadoDiretor(int ano, int periodo, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                        1
                                FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA a
                                WHERE  ANO=@ANO 
                                       AND PERIODO = @PERIODO
                                       AND  a.DT_FIM_CONF_TURNO < CONVERT(DATE, GETDATE())
                                        or EXISTS ( SELECT 1
                                                         FROM   TCE_CTV_FINALIZADO
                                                         WHERE  TCE_CTV_FINALIZADO.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA 
                                                         AND CENSO = @CENSO
                                                         AND TURNO = 1 ) "
                };
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool VerificaVagaFinalizada(int ano, int periodo, string censo, int idAgenda)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                    1
                            FROM    TCE_CTV_FINALIZADO F
                                    INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON A.ID_AGENDA_CONF_TURNO_VAGA = F.ID_AGENDA_CONF_TURNO_VAGA
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND F.CENSO = @CENSO
                                    AND A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                    AND VAGA = 1 "
                };
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool VerificaTurnoFinalizada(int ano, int periodo, string censo, int idAgenda)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                    1
                            FROM    TCE_CTV_FINALIZADO F
                                    INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON A.ID_AGENDA_CONF_TURNO_VAGA = F.ID_AGENDA_CONF_TURNO_VAGA
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND F.CENSO = @CENSO
                                    AND A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                    AND TURNO = 1 "
                };
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static void InserirTurno(ICollection<DadosConfTurno> dadosConfTurno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var ids = dadosConfTurno.Select(x => x.IdAgendaConfTurnoVaga).Distinct();

                    foreach (var id in ids)
                    {
                        var ctvFinalizado = new TceCtvFinalizado
                        {
                            IdAgendaConfTurnoVaga = id,
                            Censo = dadosConfTurno.First().Censo,
                            Matricula = dadosConfTurno.First().Matricula,
                            Vaga = false,
                            Turno = true
                        };

                        Inserir(ctx, ctvFinalizado);
                    }
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static void InserirVaga(ICollection<DadosAgendaVagas> dadosAgendaVagas, string censo, string usuario)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    foreach (var item in dadosAgendaVagas)
                    {
                        var finalizado = new TceCtvFinalizado
                        {
                            Censo = censo,
                            IdAgendaConfTurnoVaga = item.IDAgenda,
                            Matricula = usuario,
                            Vaga = true,
                            Turno = false
                        };
                        var confere = VerificaVagaFinalizadaExistente(finalizado);

                        if (!confere)
                        {
                            Inserir(ctx, finalizado);
                        }
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
                finally
                {
                    ctx.Dispose();
                }
            }
        }

        private static bool VerificaVagaFinalizadaExistente(TceCtvFinalizado ctvFinalizado)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1 1
                            FROM    TCE_CTV_FINALIZADO F                                    
                            WHERE       F.TURNO = 0
                                    AND F.CENSO = @CENSO
                                    AND F.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                    AND F.VAGA = 1 "
                };

                contextQuery.Parameters.Add("@CENSO", ctvFinalizado.Censo);
                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvFinalizado.IdAgendaConfTurnoVaga);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        private static void Inserir(DataContext context, TceCtvFinalizado ctvFinalizado)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO dbo.TCE_CTV_FINALIZADO
                                ( ID_AGENDA_CONF_TURNO_VAGA ,
                                  CENSO ,
                                  TURNO ,
                                  VAGA ,
                                  MATRICULA 
                                )
                        VALUES   ( @ID_AGENDA_CONF_TURNO_VAGA ,
                                  @CENSO ,
                                  @TURNO ,
                                  @VAGA ,
                                  @MATRICULA
                                ) "
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvFinalizado.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CENSO", ctvFinalizado.Censo);
            contextQuery.Parameters.Add("@TURNO", ctvFinalizado.Turno);
            contextQuery.Parameters.Add("@VAGA", ctvFinalizado.Vaga);
            contextQuery.Parameters.Add("@MATRICULA", ctvFinalizado.Matricula);

            context.ApplyModifications(contextQuery);
        }

        public static DateTime RetornaDataFinalizacao(int ano, int periodo)
        {
            var data = new DateTime();
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT  MIN(DT_FIM_CONF_TURNO) as DATA
                    FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA
                    WHERE   ANO = @ANO
                            AND PERIODO = @PERIODO"
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        data = Convert.ToDateTime(reader["DATA"]);
                    }
                }
            }

            return data;
        }

        public static ValidacaoDados Validar(DadosConfTurno dadosConfTurno)
        {
            var mensagens = new List<string>();

            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosConfTurno == null)
            {
                return validacaoDados;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                //verifica se esta agenda / censo já foi finalizada
                var contextQuery = new ContextQuery(
                    @" SELECT TOP 1
                                1
                        FROM    dbo.TCE_CTV_FINALIZADO
                        WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND CENSO = @CENSO
                                AND TURNO = 1 ");

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", dadosConfTurno.IdAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", dadosConfTurno.Censo);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    mensagens.Add("A agenda: " + Convert.ToString(dadosConfTurno.IdAgendaConfTurnoVaga) + " já foi finalizada anteriormente para este censo.");
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }
        public static DateTime? RetornaDataFinalizacaoVaga(int ano, int periodo, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT  MAX(f.DT_FINALIZACAO) AS DATA
                            FROM    TCE_CTV_FINALIZADO F
                                    INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON F.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                            WHERE   CENSO = @CENSO
                                    AND VAGA = 1
                                    AND A.ANO = @ANO
                                    AND A.PERIODO = @PERIODO"
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);

                var result = ctx.GetReturnValue(contextQuery);

                if (result != null
                    && result != DBNull.Value)
                {
                    return (DateTime)result;
                }
            }

            return null;
        }

        public string RetornaDadosFinalizacao(int ano, string censo, bool turno, bool vaga)
        {
            string finalizados = string.Empty;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            DateTime data = DateTime.MinValue;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  A.PERIODO ,
                                        MAX(F.DT_FINALIZACAO) AS DATA ,
                                        ( SELECT TOP 1
                                                    F2.MATRICULA + ' - ' + U.NOME
                                          FROM      TCE_CTV_FINALIZADO F2
                                                    INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A2 ON F2.ID_AGENDA_CONF_TURNO_VAGA = A2.ID_AGENDA_CONF_TURNO_VAGA
                                                    INNER JOIN HADES.DBO.HD_USUARIO U ON F2.MATRICULA = U.USUARIO
                                          WHERE     F2.CENSO = @CENSO
                                                    AND A2.ANO = @ANO
                                                    AND A2.PERIODO = A.PERIODO
                                                    AND F2.TURNO = @TURNO
                                                    AND F2.VAGA = @VAGA
                                          ORDER BY  f2.DT_FINALIZACAO DESC
                                        ) AS USUARIO
                                FROM    TCE_CTV_FINALIZADO F
                                        INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON F.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                WHERE   CENSO = @CENSO
                                        AND A.ANO = @ANO
                                        AND TURNO = @TURNO
                                        AND VAGA = @VAGA
                                GROUP BY A.PERIODO
                                ORDER BY A.PERIODO "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@VAGA", vaga);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(finalizados))
                    {
                        finalizados = string.Format("{0}{1}", finalizados, Environment.NewLine);
                    }

                    data = Convert.ToDateTime(reader["DATA"]);

                    finalizados = string.Format("{0}Ano: {1} Periodo: {2} - Finalizado em {3} às {4} por {5}",
                        finalizados,
                        ano.ToString(),
                        Convert.ToString(reader["PERIODO"]),
                        data.ToString("dd/MM/yyyy"),
                        data.ToString("HH:mm"),
                        Convert.ToString(reader["USUARIO"]));
                }

                return finalizados;
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
