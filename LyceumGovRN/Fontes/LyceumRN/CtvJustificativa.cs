using System;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
    public class CtvJustificativa : RNBase
    {
        public static int RetornaIdJustificativa(TceCtvJustificativa ctvJustificativa)
        {
            var idJustificativa = 0;

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                                       {
                                           Command =
                                               @"SELECT  ID_JUSTIFICATIVA
                            FROM    DBO.TCE_CTV_JUSTIFICATIVA
                            WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                    AND CENSO = @CENSO
                                    AND TURNO = @TURNO
                                    AND VAGA = @VAGA                                    "
                                       };
                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvJustificativa.IdAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", ctvJustificativa.Censo);
                contextQuery.Parameters.Add("@TURNO", ctvJustificativa.Turno);
                contextQuery.Parameters.Add("@VAGA", ctvJustificativa.Vaga);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        idJustificativa = Convert.ToInt32(reader["ID_JUSTIFICATIVA"]);
                    }
                }
            }
            return idJustificativa;
        }

        public static void AtualizarVagas(DataContext context, TceCtvJustificativa ctvJustificativa)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  DBO.TCE_CTV_JUSTIFICATIVA
                    SET     VAGAS_CONTINUIDADE = TABELA.VAGAS_CONTINUIDADE ,
                            VAGAS_NOVAS = TABELA.VAGAS_NOVAS
                    FROM    ( SELECT    SUM(VG.VAGAS_CONTINUIDADE) AS VAGAS_CONTINUIDADE ,
                                        SUM(VG.VAGAS_NOVAS) AS VAGAS_NOVAS
                              FROM      TCE_CTV_JUSTIFICATIVA J
                                        INNER JOIN DBO.TCE_CTV_CONF_VAGA VG ON J.CENSO = VG.CENSO
                                                                               AND J.ID_AGENDA_CONF_TURNO_VAGA = VG.ID_AGENDA_CONF_TURNO_VAGA
                                                                               AND J.VAGA = 1
                              WHERE     J.ID_JUSTIFICATIVA = @ID_JUSTIFICATIVA
                            ) TABELA
                    WHERE   ID_JUSTIFICATIVA = @ID_JUSTIFICATIVA ");
          
            contextQuery.Parameters.Add("@ID_JUSTIFICATIVA", ctvJustificativa.IdJustificativa);

            context.ApplyModifications(contextQuery);
        }
        
        public static int Inserir(DataContext context, TceCtvJustificativa ctvJustificativa)
        {
            var contextQuery = new ContextQuery(
                @" INSERT INTO DBO.TCE_CTV_JUSTIFICATIVA
                        ( ID_AGENDA_CONF_TURNO_VAGA ,
                          CENSO ,
                          JUSTIFICATIVA_CONTINUIDADE ,
                          JUSTIFICATIVA_NOVO ,
                          TURNO ,
                          VAGA ,
                          MATRICULA ,
                          VAGAS_CONTINUIDADE ,
                          VAGAS_NOVAS                                  
                        )
                VALUES  ( @ID_AGENDA_CONF_TURNO_VAGA ,
                          @CENSO ,
                          @JUSTIFICATIVA_CONTINUIDADE ,
                          @JUSTIFICATIVA_NOVO ,
                          @TURNO ,
                          @VAGA ,
                          @MATRICULA  ,
                          @VAGAS_CONTINUIDADE ,
                          @VAGAS_NOVAS                                    
                        ) 

                     SELECT ID_JUSTIFICATIVA
                     FROM   TCE_CTV_JUSTIFICATIVA
                     WHERE  CENSO = @CENSO
                            AND ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                            AND VAGA = @VAGA
                            AND TURNO = @TURNO ");

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvJustificativa.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CENSO", ctvJustificativa.Censo);
            contextQuery.Parameters.Add("@JUSTIFICATIVA_CONTINUIDADE", ctvJustificativa.JustificativaContinuidade);
            contextQuery.Parameters.Add("@JUSTIFICATIVA_NOVO", ctvJustificativa.JustificativaNovo);
            contextQuery.Parameters.Add("@TURNO", ctvJustificativa.Turno);
            contextQuery.Parameters.Add("@VAGA", ctvJustificativa.Vaga);
            contextQuery.Parameters.Add("@MATRICULA", ctvJustificativa.Matricula);
            contextQuery.Parameters.Add("@VAGAS_CONTINUIDADE", ctvJustificativa.VagasContinuidade);
            contextQuery.Parameters.Add("@VAGAS_NOVAS", ctvJustificativa.VagasNovo);

            return Convert.ToInt32(context.GetReturnValue(contextQuery));
        }

        public static void Alterar(DataContext context, TceCtvJustificativa ctvJustificativa)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  DBO.TCE_CTV_JUSTIFICATIVA
                    SET     JUSTIFICATIVA_CONTINUIDADE = @JUSTIFICATIVA_CONTINUIDADE ,
                            JUSTIFICATIVA_NOVO = @JUSTIFICATIVA_NOVO ,
                            MATRICULA = @MATRICULA ,
                            VAGAS_CONTINUIDADE = @VAGAS_CONTINUIDADE ,
                            VAGAS_NOVAS = @VAGAS_NOVAS ,
                            DT_ALTERACAO = GETDATE()
                    WHERE   ID_JUSTIFICATIVA = @ID_JUSTIFICATIVA ");

            contextQuery.Parameters.Add("@JUSTIFICATIVA_CONTINUIDADE", ctvJustificativa.JustificativaContinuidade);
            contextQuery.Parameters.Add("@JUSTIFICATIVA_NOVO", ctvJustificativa.JustificativaNovo);
            contextQuery.Parameters.Add("@MATRICULA", ctvJustificativa.Matricula);
            contextQuery.Parameters.Add("@VAGAS_CONTINUIDADE", ctvJustificativa.VagasContinuidade);
            contextQuery.Parameters.Add("@VAGAS_NOVAS", ctvJustificativa.VagasNovo);
            contextQuery.Parameters.Add("@ID_JUSTIFICATIVA", ctvJustificativa.IdJustificativa);

            context.ApplyModifications(contextQuery);
        }

        public static void Remover(int id)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" delete from [TCE_CTV_JUSTIFICATIVA] WHERE [ID_JUSTIFICATIVA] = @ID_JUSTIFICATIVA"
                    };

                    contextQuery.Parameters.Add("@ID_JUSTIFICATIVA", id);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static int Salvar(TceCtvJustificativa ctvJustificativa)
        {
            int id = 0;
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    id = RetornaIdJustificativa(ctvJustificativa);

                    if (id > 0)
                    {
                        CtvJustificativa.Alterar(context, ctvJustificativa);
                    }
                    else
                    {
                        id = CtvJustificativa.Inserir(context, ctvJustificativa);
                        ctvJustificativa.IdJustificativa = id;
                    }

                    if (ctvJustificativa.Vaga)
                    {
                        CtvJustificativa.AtualizarVagas(context, ctvJustificativa);
                    }

                    return id;
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }
    }
}
