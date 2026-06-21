using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System;

namespace Techne.Lyceum.RN
{
    public class CtvConfTurnoInicial : RNBase
    {
        public static void Inserir(DataContext context, TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO dbo.TCE_CTV_CONF_TURNO_INICIAL
                    ( 
                      ID_AGENDA_CONF_TURNO_VAGA ,
                      CENSO ,
                      TURNO ,                     
                      MATRICULA 
                    )
                    SELECT DISTINCT
                            @ID_AGENDA_CONF_TURNO_VAGA ,
                            FACULDADE ,
                            TURNO ,                            
                            @MATRICULA
                    FROM    LY_TURMA T
                    WHERE   T.CURSO = @CURSO
                            AND T.SERIE = @SERIE
                            AND T.ANO = @ANO_REFERENCIA
                            AND T.SEMESTRE = @PERIODO_REFERENCIA
                            AND T.SIT_TURMA <> 'Desativada'
                            AND ( SELECT TOP 1
                                            SITUACAO
                                  FROM      LY_UNIDADE_ENSINO_SITUACAO ues
                                  WHERE     ues.UNIDADE_ENS = T.FACULDADE
                                  ORDER BY  DT_SITUACAO DESC
                                ) = 'ESTADUAL' "
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@MATRICULA", ctvAgendaConfTurnoVaga.Matricula);
            contextQuery.Parameters.Add("@CURSO", ctvAgendaConfTurnoVaga.Curso);
            contextQuery.Parameters.Add("@SERIE", ctvAgendaConfTurnoVaga.Serie);
            contextQuery.Parameters.Add("@ANO_REFERENCIA", ctvAgendaConfTurnoVaga.AnoReferencia);
            contextQuery.Parameters.Add("@PERIODO_REFERENCIA", ctvAgendaConfTurnoVaga.PeriodoReferencia);

            context.ApplyModifications(contextQuery);
        }

        public static void InserirProgressao(DataContext context, TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO dbo.TCE_CTV_CONF_TURNO_INICIAL
                                ( ID_AGENDA_CONF_TURNO_VAGA ,
                                  CENSO ,
                                  TURNO ,
                                  MATRICULA           
                                )
                                SELECT DISTINCT
                                        @ID_AGENDA_CONF_TURNO_VAGA ,
                                        FACULDADE ,
                                        TURNO ,
                                        @MATRICULA
                                FROM    TurnosVagas.PROGRESSAOSERIE_UNIDADEENSINO p
                                        INNER JOIN LY_TURMA T ON p.CURSOID = t.CURSO
                                                                 AND p.SERIE = t.SERIE
                                                                 AND p.UNIDADEENSINOID = t.FACULDADE
                                WHERE   p.PROXIMOCURSOID = @CURSO
                                        AND p.PROXIMASERIE = @SERIE
                                        AND T.ANO = @ANO_REFERENCIA
                                        AND T.SEMESTRE = @PERIODO_REFERENCIA
                                        AND T.SIT_TURMA <> 'Desativada'
                                        AND t.CURSO = @CURSO --Apenas pegar progressoes do mesmo ano
                                        AND p.PREFERENCIAL = 1
                                        AND NOT EXISTS ( SELECT TOP 1
                                                                1
                                                         FROM   TCE_CTV_CONF_TURNO_INICIAL ti
                                                         WHERE  ti.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                                                AND ti.TURNO = t.TURNO
                                                                AND ti.CENSO = t.FACULDADE )
                                        AND NOT EXISTS ( SELECT TOP 1
                                                        1
                                                 FROM   dbo.LY_TURMA t2
                                                 WHERE  t2.ANO = @ANO_REFERENCIA
                                                        AND t2.SEMESTRE = @PERIODO_REFERENCIA
                                                        AND t2.CURSO = @CURSO
                                                        AND t2.SERIE = @SERIE
                                                        AND t2.FACULDADE = t.FACULDADE
                                                        AND T.SIT_TURMA <> 'Desativada' ) --Verifica se não existe turma para ano/periodo/censo/curso/serie 
                                        AND ( SELECT TOP 1
                                                        SITUACAO
                                              FROM      LY_UNIDADE_ENSINO_SITUACAO ues
                                              WHERE     ues.UNIDADE_ENS = T.FACULDADE
                                              ORDER BY  DT_SITUACAO DESC
                                            ) = 'ESTADUAL' "
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@MATRICULA", ctvAgendaConfTurnoVaga.Matricula);
            contextQuery.Parameters.Add("@CURSO", ctvAgendaConfTurnoVaga.Curso);
            contextQuery.Parameters.Add("@SERIE", ctvAgendaConfTurnoVaga.Serie);
            contextQuery.Parameters.Add("@ANO_REFERENCIA", ctvAgendaConfTurnoVaga.AnoReferencia);
            contextQuery.Parameters.Add("@PERIODO_REFERENCIA", ctvAgendaConfTurnoVaga.PeriodoReferencia);

            context.ApplyModifications(contextQuery);
        }

        public static void Inserir(DataContext context, TceCtvConfTurno ctvConfTurno)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO dbo.TCE_CTV_CONF_TURNO_INICIAL
                    ( 
                      ID_AGENDA_CONF_TURNO_VAGA ,
                      CENSO ,
                      TURNO ,                     
                      MATRICULA 
                    )
                    VALUES
                    ( 
                      @ID_AGENDA_CONF_TURNO_VAGA ,
                      @CENSO ,
                      @TURNO ,                     
                      @MATRICULA 
                    ) "
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvConfTurno.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CENSO", ctvConfTurno.Censo);
            contextQuery.Parameters.Add("@TURNO", ctvConfTurno.Turno);
            contextQuery.Parameters.Add("@MATRICULA", ctvConfTurno.Matricula);

            context.ApplyModifications(contextQuery);
        }

        public static void Excluir(DataContext context, int idAgenda)
        {
            var contextQuery = new ContextQuery(
                @" DELETE  DBO.TCE_CTV_CONF_TURNO_INICIAL
                    WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA ");

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

            context.ApplyModifications(contextQuery);
        }

        public static DataTable Listar(string censo, int ano, int periodo)
        {
            if (string.IsNullOrEmpty(censo) || ano <= 0 || periodo < 0)
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT DISTINCT
                        TI.CENSO,
                        TI.CENSO + ' - ' + UE.NOME_COMP AS ESCOLA ,
                        AG.CURSO ,
                        C.NOME AS NOME_CURSO ,
                        AG.SERIE ,
                        MC.DESCRICAO AS 'MODALIDADE' ,
                        TI.TURNO ,
                        TU.DESCRICAO AS 'NOME_TURNO',
                        ISNULL(( SELECT TOP 1
                                        'X'
                                 FROM   DBO.TCE_CTV_CONF_TURNO TC
                                 WHERE  TC.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                        AND TC.CENSO = TI.CENSO
                                        AND TC.TURNO = TI.TURNO
                                        AND TC.CONTINUIDADE = 1
                               ), '') AS CONTINUIDADE ,
                        ISNULL(( SELECT TOP 1
                                        'X'
                                 FROM   DBO.TCE_CTV_CONF_TURNO TN
                                 WHERE  TN.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                        AND TN.CENSO = TI.CENSO
                                        AND TN.TURNO = TI.TURNO
                                        AND TN.NOVO = 1
                               ), '') AS NOVO
                FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                        INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                        INNER JOIN LY_CURSO C ON AG.CURSO = C.CURSO
                        INNER JOIN LY_SERIE S ON S.SERIE = AG.SERIE AND s.CURSO = c.curso
                        INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                        INNER JOIN LY_UNIDADE_ENSINO UE ON UE.UNIDADE_ENS = TI.CENSO
                        INNER JOIN LY_TURNO TU ON TU.TURNO=TI.TURNO
                WHERE   AG.ANO = @ANO
                        AND AG.PERIODO = @PERIODO
                        AND CENSO = @CENSO  ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarTurnosPor(string censo, int ano, int periodo, string curso, int serie)
        {
            if (string.IsNullOrEmpty(censo) || ano <= 0 || periodo < 0 || string.IsNullOrEmpty(curso) || serie < 0)
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT DISTINCT       
                            TI.TURNO ,
                            ISNULL(( SELECT TOP 1
                                            1
                                     FROM   DBO.TCE_CTV_CONF_TURNO TC
                                     WHERE  TC.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                            AND TC.CENSO = TI.CENSO
                                            AND TC.TURNO = TI.TURNO
                                            AND TC.CONTINUIDADE = 1
                                   ), 0) AS CONTINUIDADE ,
                            ISNULL(( SELECT TOP 1
                                            1
                                     FROM   DBO.TCE_CTV_CONF_TURNO TN
                                     WHERE  TN.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                            AND TN.CENSO = TI.CENSO
                                            AND TN.TURNO = TI.TURNO
                                            AND TN.NOVO = 1
                                   ), 0) AS NOVO
                    FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                            INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                            INNER JOIN LY_CURSO C ON AG.CURSO = C.CURSO
                    WHERE   AG.ANO = @ANO
                            AND AG.PERIODO = @PERIODO
                            AND CENSO = @CENSO
                            AND AG.CURSO = @CURSO
                            AND AG.SERIE = @SERIE  ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SERIE", serie);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public DataTable ListaTurnosIniciaisPor(string censo, int ano, int agendaId)
        {
            if (string.IsNullOrEmpty(censo) || ano <= 0)
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT DISTINCT       
                            TI.TURNO                            
                    FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                            INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                    WHERE   AG.ANO = @ANO
                            AND CENSO = @CENSO
                            AND AGENDAID = @AGENDAID
						    AND NOT EXISTS ( SELECT 1
                                                 FROM   DBO.TCE_CTV_RESTRICAO RE
                                                 WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                                        AND TI.CENSO = RE.CENSO ) ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public DataTable ListaTurnosniciaisAgendaPor(string censo, int ano, int agendaId, int idAgendaConfTurnoVaga)
        {
            if (string.IsNullOrEmpty(censo) || ano <= 0)
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT DISTINCT       
                            TI.TURNO                            
                    FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                            INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                    WHERE   AG.ANO = @ANO
                            AND CENSO = @CENSO
                            AND AGENDAID = @AGENDAID
                            AND AG.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@AGENDAID", agendaId);
                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgendaConfTurnoVaga);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public bool PossuiTurnoParaConfirmacaoPor(int idAgendaConfTurnoVaga, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.PossuiTurnoParaConfirmacaoPor(ctx, idAgendaConfTurnoVaga, censo);
                return possui;
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

        public bool PossuiTurnoParaConfirmacaoPor(DataContext ctx, int idAgendaConfTurnoVaga, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                    FROM    TCE_CTV_CONF_TURNO_INICIAL
                    WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                            AND CENSO = @CENSO ";

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CENSO", censo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }
    }
}
