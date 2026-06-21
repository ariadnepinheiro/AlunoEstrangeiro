using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class CtvConfTurno : RNBase
    {
        public static void InserirTurnoNovo(DataContext context, TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO dbo.TCE_CTV_CONF_TURNO
                            ( ID_AGENDA_CONF_TURNO_VAGA ,
                              CENSO ,
                              TURNO ,
                              CONTINUIDADE ,
                              NOVO ,
                              CONFIRMADA ,
                              MATRICULA                                        
                            )
                            SELECT DISTINCT
                                    @ID_AGENDA_CONF_TURNO_VAGA ,
                                    FACULDADE ,
                                    TURNO ,
                                    0 ,
                                    1 ,
                                    @CONFIRMADA ,
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
                                        ) = 'ESTADUAL'  "
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CONFIRMADA", false);
            contextQuery.Parameters.Add("@MATRICULA", ctvAgendaConfTurnoVaga.Matricula);
            contextQuery.Parameters.Add("@CURSO", ctvAgendaConfTurnoVaga.Curso);
            contextQuery.Parameters.Add("@SERIE", ctvAgendaConfTurnoVaga.Serie);
            contextQuery.Parameters.Add("@ANO_REFERENCIA", ctvAgendaConfTurnoVaga.AnoReferencia);
            contextQuery.Parameters.Add("@PERIODO_REFERENCIA", ctvAgendaConfTurnoVaga.PeriodoReferencia);

            context.ApplyModifications(contextQuery);
        }

        public static void InserirTurnoNovoProgressao(DataContext context, TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO dbo.TCE_CTV_CONF_TURNO
                            ( ID_AGENDA_CONF_TURNO_VAGA ,
                              CENSO ,
                              TURNO ,
                              CONTINUIDADE ,
                              NOVO ,
                              CONFIRMADA ,
                              MATRICULA                                        
                                                
                            )
                            SELECT DISTINCT
                                    @ID_AGENDA_CONF_TURNO_VAGA ,
                                    FACULDADE ,
                                    TURNO ,
                                    0 ,
                                    1 ,
                                    @CONFIRMADA ,
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
                                                     FROM   TCE_CTV_CONF_TURNO ti
                                                     WHERE  ti.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                                            AND ti.TURNO = t.TURNO
                                                            AND ti.CENSO = t.FACULDADE
                                                            AND NOVO = 1 )
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
                                        ) = 'ESTADUAL'   "
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CONFIRMADA", false);
            contextQuery.Parameters.Add("@MATRICULA", ctvAgendaConfTurnoVaga.Matricula);
            contextQuery.Parameters.Add("@CURSO", ctvAgendaConfTurnoVaga.Curso);
            contextQuery.Parameters.Add("@SERIE", ctvAgendaConfTurnoVaga.Serie);
            contextQuery.Parameters.Add("@ANO_REFERENCIA", ctvAgendaConfTurnoVaga.AnoReferencia);
            contextQuery.Parameters.Add("@PERIODO_REFERENCIA", ctvAgendaConfTurnoVaga.PeriodoReferencia);

            context.ApplyModifications(contextQuery);
        }

        public static void InserirTurnoContinuidade(DataContext context, TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO dbo.TCE_CTV_CONF_TURNO
                            ( ID_AGENDA_CONF_TURNO_VAGA ,
                              CENSO ,
                              TURNO ,
                              CONTINUIDADE ,
                              NOVO ,
                              CONFIRMADA ,
                              MATRICULA                                        
                            )
                            SELECT DISTINCT
                                    @ID_AGENDA_CONF_TURNO_VAGA ,
                                    FACULDADE ,
                                    TURNO ,
                                    1 ,
                                    0 ,
                                    @CONFIRMADA ,
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
                                        ) = 'ESTADUAL'  "
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CONFIRMADA", false);
            contextQuery.Parameters.Add("@MATRICULA", ctvAgendaConfTurnoVaga.Matricula);
            contextQuery.Parameters.Add("@CURSO", ctvAgendaConfTurnoVaga.Curso);
            contextQuery.Parameters.Add("@SERIE", ctvAgendaConfTurnoVaga.Serie);
            contextQuery.Parameters.Add("@ANO_REFERENCIA", ctvAgendaConfTurnoVaga.AnoReferencia);
            contextQuery.Parameters.Add("@PERIODO_REFERENCIA", ctvAgendaConfTurnoVaga.PeriodoReferencia);

            context.ApplyModifications(contextQuery);
        }

        public static void InserirTurnoContinuidadeProgressao(DataContext context, TceCtvAgendaConfTurnoVaga ctvAgendaConfTurnoVaga)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO dbo.TCE_CTV_CONF_TURNO
                            ( ID_AGENDA_CONF_TURNO_VAGA ,
                              CENSO ,
                              TURNO ,
                              CONTINUIDADE ,
                              NOVO ,
                              CONFIRMADA ,
                              MATRICULA                                        
                                                
                            )
                            SELECT DISTINCT
                                    @ID_AGENDA_CONF_TURNO_VAGA ,
                                    FACULDADE ,
                                    TURNO ,
                                    1 ,
                                    0 ,
                                    @CONFIRMADA ,
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
                                                     FROM   TCE_CTV_CONF_TURNO ti
                                                     WHERE  ti.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                                            AND ti.TURNO = t.TURNO
                                                            AND ti.CENSO = t.FACULDADE
                                                            and CONTINUIDADE = 1 )
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
                                        ) = 'ESTADUAL'   "
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvAgendaConfTurnoVaga.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CONFIRMADA", false);
            contextQuery.Parameters.Add("@MATRICULA", ctvAgendaConfTurnoVaga.Matricula);
            contextQuery.Parameters.Add("@CURSO", ctvAgendaConfTurnoVaga.Curso);
            contextQuery.Parameters.Add("@SERIE", ctvAgendaConfTurnoVaga.Serie);
            contextQuery.Parameters.Add("@ANO_REFERENCIA", ctvAgendaConfTurnoVaga.AnoReferencia);
            contextQuery.Parameters.Add("@PERIODO_REFERENCIA", ctvAgendaConfTurnoVaga.PeriodoReferencia);

            context.ApplyModifications(contextQuery);
        }

        private static void Inserir(DataContext context, TceCtvConfTurno ctvConfTurno)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO dbo.TCE_CTV_CONF_TURNO
                        ( ID_AGENDA_CONF_TURNO_VAGA ,
                          CENSO ,
                          TURNO ,
                          CONTINUIDADE ,
                          NOVO ,
                          CONFIRMADA ,
                          MATRICULA                     
                        )
                VALUES  ( @ID_AGENDA_CONF_TURNO_VAGA ,
                          @CENSO ,
                          @TURNO ,
                          @CONTINUIDADE ,
                          @NOVO ,
                          @CONFIRMADA ,
                          @MATRICULA                     
                        ) "
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvConfTurno.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CENSO", ctvConfTurno.Censo);
            contextQuery.Parameters.Add("@TURNO", ctvConfTurno.Turno);
            contextQuery.Parameters.Add("@CONTINUIDADE", ctvConfTurno.Continuidade);
            contextQuery.Parameters.Add("@NOVO", ctvConfTurno.Novo);
            contextQuery.Parameters.Add("@CONFIRMADA", ctvConfTurno.Confirmada);
            contextQuery.Parameters.Add("@MATRICULA", ctvConfTurno.Matricula);

            context.ApplyModifications(contextQuery);
        }

        public static ICollection<DadosConfTurno> ListarAbertos(string censo, int ano, int idPerfil)
        {
            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                @" SELECT  DISTINCT
                        TI.ID_AGENDA_CONF_TURNO_VAGA ,
                        A.AGENDAID ,
                        A.PERIODO ,
                        A.DT_INICIO_CONF_TURNO ,
                        TI.CENSO ,
                        A.CURSO ,
                        c.TIPO AS 'CODIGO_TIPO' ,
                        tc.DESCRICAO AS 'DESCRICAO_TIPO' ,
                        MC.DESCRICAO AS 'MODALIDADE' ,
                        C.MODALIDADE AS 'CODIGO_MODALIDADE' ,
                        C.NOME AS 'NOME_CURSO' ,
                        A.SERIE ,
                        DBO.F_CTVTURNOSNOVO(TI.ID_AGENDA_CONF_TURNO_VAGA, TI.CENSO) AS 'TURNOS_NOVO' ,
                        DBO.F_CTVTURNOSCONTINUIDADE(TI.ID_AGENDA_CONF_TURNO_VAGA, TI.CENSO) AS 'TURNOS_CONTINUIDADE' ,
                        DBO.F_CTVTURNOSINICIAIS(TI.ID_AGENDA_CONF_TURNO_VAGA, TI.CENSO) AS 'TURNOS_INICIAIS' ,
                        J.JUSTIFICATIVA_CONTINUIDADE ,
                        J.JUSTIFICATIVA_NOVO ,
                        CASE WHEN A.ENCERRADO = 1 THEN 1
                             -- WHEN ( A.DT_FIM_CONF_TURNO < CONVERT(DATE, GETDATE())
                             --        AND ID_FINALIZADO IS NULL
                             --     ) THEN 1
                             WHEN ID_FINALIZADO IS NOT NULL THEN 1
                             ELSE 0
                        END FINALIZADO ,
                         CASE WHEN A.ENCERRADO = 1 THEN 1
                                 WHEN @PERFIL_LOGADO = 0 THEN A.ENCERRADO
                                 WHEN @PERFIL_LOGADO <> 0
                                      AND EXISTS ( SELECT TOP 1
                                                            1
                                                   FROM     AGENDA.AGENDA AA
                                                            INNER JOIN AGENDA.EVENTO AE ON AA.AGENDAID = AE.AGENDAID
                                                            INNER JOIN AGENDA.EVENTO_PERFIL AEP ON AEP.EVENTOID = AE.EVENTOID
                                                            INNER JOIN HADES.DBO.TCE_PERFIL AP ON AP.ID_PERFIL = AEP.PERFILID
                                                            INNER JOIN AGENDA.TIPOEVENTO ATE ON AE.TIPOEVENTOID = ATE.TIPOEVENTOID
                                                    WHERE   CONVERT(DATE, DATAINICIO) <= CONVERT(DATE, GETDATE())
                                                            AND CONVERT(DATE, DATAFIM) >= CONVERT(DATE, GETDATE())
                                                            AND AP.ID_PERFIL = @PERFIL_LOGADO
                                                            AND AA.AGENDAID = a.AGENDAID 
                                                            AND ATE.TIPOEVENTOID = @TIPOEVENTOID) THEN 0
                                 ELSE 1
                            END ENCERRADO ,
                        PE.DESCRICAO AS PERFIL_RESPONSAVEL ,
                        A.ANO_REFERENCIA ,
                        A.PERIODO_REFERENCIA
                INTO    #TURNOS
                FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                        INNER JOIN LY_CURSO C ON C.CURSO = A.CURSO
                        INNER JOIN dbo.LY_TIPO_CURSO tc ON c.TIPO = tc.TIPO
                        INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                        INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON TI.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                        LEFT JOIN DBO.TCE_CTV_CONF_TURNO T ON T.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                        LEFT JOIN DBO.TCE_CTV_JUSTIFICATIVA J ON J.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                                                 AND J.CENSO = TI.CENSO
                                                                 AND J.TURNO = 1
                        LEFT JOIN DBO.TCE_CTV_FINALIZADO F ON T.ID_AGENDA_CONF_TURNO_VAGA = F.ID_AGENDA_CONF_TURNO_VAGA
                                                              AND TI.CENSO = F.CENSO
                                                              AND F.TURNO = 1
                        LEFT JOIN DBO.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                        LEFT JOIN HADES.DBO.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                WHERE   TI.CENSO = @CENSO
                        AND A.ANO = @ANO
                        AND NOT EXISTS ( SELECT 1
                                         FROM   DBO.TCE_CTV_RESTRICAO RE
                                         WHERE  ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                                                AND TI.CENSO = RE.CENSO )
                        AND CONVERT(DATE, GETDATE()) >= A.DT_INICIO_CONF_TURNO
                                                           
                SELECT DISTINCT
                        ID_AGENDA_CONF_TURNO_VAGA ,
                        AGENDAID ,
                        PERIODO ,
                        DT_INICIO_CONF_TURNO ,
                        CENSO ,
                        CURSO ,
                        DESCRICAO_TIPO ,
                        CODIGO_TIPO ,
                        MODALIDADE ,
                        CODIGO_MODALIDADE ,
                        NOME_CURSO ,
                        SERIE ,
                        TURNOS_NOVO ,
                        TURNOS_CONTINUIDADE ,
                        TURNOS_INICIAIS ,
                        JUSTIFICATIVA_CONTINUIDADE ,
                        JUSTIFICATIVA_NOVO ,
                        FINALIZADO ,
                        ENCERRADO ,
                        PERFIL_RESPONSAVEL ,
                        ANO_REFERENCIA ,
                        PERIODO_REFERENCIA ,
                        ( SELECT TOP 1
                                    LS.DESCRICAO
                          FROM      LY_SERIE LS
                                    JOIN LY_CURSO C ON LS.CURSO = C.CURSO
                                    JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO
                                                            AND LC.CURSO = LS.CURSO
                          WHERE     T.SERIE = LS.SERIE
                                    AND T.CURSO = LS.CURSO
                                    AND LC.ANO_INI = T.ANO_REFERENCIA
                                    AND LC.SEM_INI = T.PERIODO_REFERENCIA
                        ) AS DESCRICAO_SERIE
                FROM    #TURNOS T
                WHERE ENCERRADO = 0
                        --AND FINALIZADO = 0
                ORDER BY PERIODO, T.MODALIDADE ,
                        NOME_CURSO ,
                        SERIE        
                DROP TABLE  #TURNOS ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@PERFIL_LOGADO", idPerfil);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPOEVENTOID", (int)RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoTurnos);

                var turnos = new List<DadosConfTurno>();

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        var turnosIniciais = Convert.ToString(reader["TURNOS_INICIAIS"]);

                        var coditoTurnosIniciais = string.Format(
                           "{0}{1}{2}{3}{4}",
                           turnosIniciais.Contains("M") ? "1" : "0",
                           turnosIniciais.Contains("T") ? "1" : "0",
                           turnosIniciais.Contains("N") ? "1" : "0",
                           turnosIniciais.Contains("I") ? "1" : "0",
                           turnosIniciais.Contains("A") ? "1" : "0");

                        var codigoTurnos = Convert.ToString(reader["TURNOS_CONTINUIDADE"])
                                                  .Split(new[]
                                                             {
                                                                 ";"
                                                             }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(x => x.Trim())
                                                  .ToList();
                        var codigoTurnosNovo = Convert.ToString(reader["TURNOS_NOVO"])
                                                  .Split(new[]
                                                             {
                                                                 ";"
                                                             }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(x => x.Trim())
                                                  .ToList();

                        var dadosConfTurno = new DadosConfTurno
                        {
                            AgendaId = Convert.ToInt32(reader["AGENDAID"]),
                            IdAgendaConfTurnoVaga = Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]),
                            Periodo = Convert.ToInt32(reader["PERIODO"]),
                            Curso = Convert.ToString(reader["CURSO"]),
                            Justificativa = Convert.ToString(reader["JUSTIFICATIVA_CONTINUIDADE"]),
                            JustificativaNovo = Convert.ToString(reader["JUSTIFICATIVA_NOVO"]),
                            DescricaoTipo = Convert.ToString(reader["DESCRICAO_TIPO"]),
                            CodigoTipo = Convert.ToString(reader["CODIGO_TIPO"]),
                            Modalidade = Convert.ToString(reader["MODALIDADE"]),
                            CodigoModalidade = Convert.ToString(reader["CODIGO_MODALIDADE"]),
                            NomeCurso = Convert.ToString(reader["NOME_CURSO"]),
                            TurnosIniciais = coditoTurnosIniciais,
                            Serie = Convert.ToInt32(reader["SERIE"]),
                            DescricaoSerie = Convert.ToString(reader["DESCRICAO_SERIE"]),
                            Finalizado = Convert.ToBoolean(reader["FINALIZADO"]),
                            Encerrado = Convert.ToBoolean(reader["ENCERRADO"]),
                            TurnosListaInicial = turnosIniciais,
                            PerfilResponsavel = Convert.ToString(reader["PERFIL_RESPONSAVEL"]),
                            Censo = Convert.ToString(reader["CENSO"]),
                            DtInicioConfTurno = Convert.ToDateTime(reader["DT_INICIO_CONF_TURNO"])
                        };

                        dadosConfTurno.CarregarTurnos(codigoTurnos, codigoTurnosNovo);

                        turnos.Add(dadosConfTurno);
                    }
                }

                return turnos;
            }
        }

        public static ICollection<DadosConfTurno> Listar(string censo, int ano, int idPerfil)
        {
            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"  SELECT  DISTINCT
                            TI.ID_AGENDA_CONF_TURNO_VAGA ,
                            a.AGENDAID ,
                            A.PERIODO ,
                            A.DT_INICIO_CONF_TURNO ,
                            TI.CENSO ,
                            A.CURSO ,
                            c.TIPO AS 'CODIGO_TIPO' ,
                            tc.DESCRICAO AS 'DESCRICAO_TIPO' ,
                            C.MODALIDADE AS 'CODIGO_MODALIDADE' ,
                            MC.DESCRICAO AS 'MODALIDADE' ,
                            C.NOME AS 'NOME_CURSO' ,
                            A.SERIE ,
                            DBO.F_CTVTURNOSNOVO(TI.ID_AGENDA_CONF_TURNO_VAGA, TI.CENSO) AS 'TURNOS_NOVO' ,
                            DBO.F_CTVTURNOSCONTINUIDADE(TI.ID_AGENDA_CONF_TURNO_VAGA, TI.CENSO) AS 'TURNOS_CONTINUIDADE' ,
                            DBO.F_CTVTURNOSINICIAIS(TI.ID_AGENDA_CONF_TURNO_VAGA, TI.CENSO) AS 'TURNOS_INICIAIS' ,
                            J.JUSTIFICATIVA_CONTINUIDADE ,
                            J.JUSTIFICATIVA_NOVO ,
                            CASE WHEN A.ENCERRADO = 1 THEN 1
                                 --WHEN ( A.DT_FIM_CONF_TURNO < CONVERT(DATE, GETDATE())
                                 --       AND ID_FINALIZADO IS NULL
                                 --     ) THEN 1
                                 WHEN ID_FINALIZADO IS NOT NULL THEN 1
                                 ELSE 0
                            END FINALIZADO ,
                            CASE WHEN A.ENCERRADO = 1 THEN 1
                                 WHEN @PERFIL_LOGADO = 0 THEN A.ENCERRADO
                                 WHEN @PERFIL_LOGADO <> 0
                                      AND EXISTS ( SELECT TOP 1
                                                            1
                                                   FROM     AGENDA.AGENDA AA
                                                            INNER JOIN AGENDA.EVENTO AE ON AA.AGENDAID = AE.AGENDAID
                                                            INNER JOIN AGENDA.EVENTO_PERFIL AEP ON AEP.EVENTOID = AE.EVENTOID
                                                            INNER JOIN HADES.DBO.TCE_PERFIL AP ON AP.ID_PERFIL = AEP.PERFILID
                                                            INNER JOIN AGENDA.TIPOEVENTO ATE ON AE.TIPOEVENTOID = ATE.TIPOEVENTOID
                                                   WHERE    CONVERT(DATE, DATAINICIO) <= CONVERT(DATE, GETDATE())
                                                            AND CONVERT(DATE, DATAFIM) >= CONVERT(DATE, GETDATE())
                                                            AND AP.ID_PERFIL = @PERFIL_LOGADO
                                                            AND AA.AGENDAID = a.AGENDAID 
                                                            AND ATE.TIPOEVENTOID = @TIPOEVENTOID) THEN 0
                                 ELSE 1
                            END ENCERRADO ,
                            PE.DESCRICAO AS PERFIL_RESPONSAVEL ,
                            A.ANO_REFERENCIA ,
                            A.PERIODO_REFERENCIA
                     INTO   #TURNOS
                     FROM   DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                            INNER JOIN LY_CURSO C ON C.CURSO = A.CURSO
                            INNER JOIN dbo.LY_TIPO_CURSO tc ON c.TIPO = tc.TIPO
                            INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                            INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON TI.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                            LEFT JOIN DBO.TCE_CTV_CONF_TURNO T ON T.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                            LEFT JOIN DBO.TCE_CTV_JUSTIFICATIVA J ON J.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                                                     AND J.CENSO = TI.CENSO
                                                                     AND J.TURNO = 1
                            LEFT JOIN DBO.TCE_CTV_FINALIZADO F ON T.ID_AGENDA_CONF_TURNO_VAGA = F.ID_AGENDA_CONF_TURNO_VAGA
                                                                  AND TI.CENSO = F.CENSO
                                                                  AND F.TURNO = 1
                            LEFT JOIN DBO.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                            LEFT JOIN HADES.DBO.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                     WHERE  TI.CENSO = @CENSO
                            AND A.ANO = @ANO
                            AND A.DT_INICIO_CONF_TURNO <= CONVERT(DATE, GETDATE())
                            AND NOT EXISTS ( SELECT 1
                                             FROM   DBO.TCE_CTV_RESTRICAO RE
                                             WHERE  ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                                                    AND TI.CENSO = RE.CENSO ) 
                                                                                
                                                                    
                     SELECT DISTINCT
                            ID_AGENDA_CONF_TURNO_VAGA ,
                            AGENDAID,
                            PERIODO ,
                            DT_INICIO_CONF_TURNO ,
                            CENSO ,
                            CURSO ,
                            DESCRICAO_TIPO ,
                            CODIGO_TIPO ,
                            MODALIDADE ,
                            CODIGO_MODALIDADE ,
                            NOME_CURSO ,
                            SERIE ,
                            TURNOS_NOVO ,
                            TURNOS_CONTINUIDADE ,
                            TURNOS_INICIAIS ,
                            JUSTIFICATIVA_CONTINUIDADE ,
                            JUSTIFICATIVA_NOVO ,
                            FINALIZADO ,
                            ENCERRADO ,
                            PERFIL_RESPONSAVEL ,
                            ANO_REFERENCIA ,
                            PERIODO_REFERENCIA ,
                            ( SELECT TOP 1
                                        LS.DESCRICAO
                              FROM      LY_SERIE LS
                                        JOIN LY_CURSO C ON LS.CURSO = C.CURSO
                                        JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO
                                                                AND LC.CURSO = LS.CURSO
                              WHERE     T.SERIE = LS.SERIE
                                        AND T.CURSO = LS.CURSO
                                        AND LC.ANO_INI = T.ANO_REFERENCIA
                                        AND LC.SEM_INI = T.PERIODO_REFERENCIA
                            ) AS DESCRICAO_SERIE
                     FROM   #TURNOS T
                     ORDER BY PERIODO, T.MODALIDADE ,
                            NOME_CURSO ,
                            SERIE        
                     DROP TABLE  #TURNOS ");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@PERFIL_LOGADO", idPerfil);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPOEVENTOID", (int)RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoTurnos);

                var turnos = new List<DadosConfTurno>();

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        var turnosIniciais = Convert.ToString(reader["TURNOS_INICIAIS"]);

                        var coditoTurnosIniciais = string.Format(
                           "{0}{1}{2}{3}{4}",
                           turnosIniciais.Contains("M") ? "1" : "0",
                           turnosIniciais.Contains("T") ? "1" : "0",
                           turnosIniciais.Contains("N") ? "1" : "0",
                           turnosIniciais.Contains("I") ? "1" : "0",
                           turnosIniciais.Contains("A") ? "1" : "0");

                        var codigoTurnos = Convert.ToString(reader["TURNOS_CONTINUIDADE"])
                                                  .Split(new[]
                                                             {
                                                                 ";"
                                                             }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(x => x.Trim())
                                                  .ToList();
                        var codigoTurnosNovo = Convert.ToString(reader["TURNOS_NOVO"])
                                                  .Split(new[]
                                                             {
                                                                 ";"
                                                             }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(x => x.Trim())
                                                  .ToList();

                        var dadosConfTurno = new DadosConfTurno
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
                            TurnosIniciais = coditoTurnosIniciais,
                            Serie = Convert.ToInt32(reader["SERIE"]),
                            DescricaoSerie = Convert.ToString(reader["DESCRICAO_SERIE"]),
                            Finalizado = Convert.ToBoolean(reader["FINALIZADO"]),
                            Encerrado = Convert.ToBoolean(reader["ENCERRADO"]),
                            PerfilResponsavel = Convert.ToString(reader["PERFIL_RESPONSAVEL"]),
                            TurnosListaInicial = turnosIniciais,
                            Censo = Convert.ToString(reader["CENSO"]),
                            DtInicioConfTurno = Convert.ToDateTime(reader["DT_INICIO_CONF_TURNO"])
                        };

                        dadosConfTurno.CarregarTurnos(codigoTurnos, codigoTurnosNovo);

                        turnos.Add(dadosConfTurno);
                    }
                }

                return turnos;
            }
        }

        public static ValidacaoDados ValidarParcial(DadosConfTurno dadosConfTurno, int codPerfil, int totalTurnos)
        {
            var mensagens = new List<string>();
            Agenda.ParametroTurnoVaga rnParametroTurnoVaga = new Agenda.ParametroTurnoVaga();
            Agenda.Entidades.ParametroTurnoVaga parametroTurnoVaga = new Agenda.Entidades.ParametroTurnoVaga();
            CtvConfTurnoInicial rnCtvConfTurnoInicial = new CtvConfTurnoInicial();
            int qtdeTurnosFinal = 0;
            int qtdeTurnosMinima = 0;
            int qtdeTurnosMaxima = 0;
            int qtdeTurnosProposta = 0;

            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosConfTurno == null)
            {
                return validacaoDados;
            }

            if (codPerfil < 0)
            {
                mensagens.Add("Não é permitido alterar turnos para seu perfil de usuário");
            }

            if (dadosConfTurno.IdAgendaConfTurnoVaga <= 0)
            {
                mensagens.Add("O campo ID AGENDA é obrigatório!");
            }

            if (dadosConfTurno.AgendaId <= 0)
            {
                mensagens.Add("O campo  AGENDAID é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosConfTurno.Censo)
               || (!string.IsNullOrEmpty(dadosConfTurno.Censo)
                   && dadosConfTurno.Censo.Length > 8))
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é obrigatório com o máximo de 8 caracteres!");
            }

            if (string.IsNullOrEmpty(dadosConfTurno.Matricula)
                || (!string.IsNullOrEmpty(dadosConfTurno.Matricula)
                    && dadosConfTurno.Matricula.Length > 20))
            {
                mensagens.Add("O campo MATRICULA DO RESPONSÁVEL é obrigatório com o máximo de 20 caracteres!");
            }

            //Verificar se esta sendo desmarcado um turno de continuidade que ja tem turma na ctvConfVaga
            var turnoContinuidadeUsado = VerificaVagasContinuidade(dadosConfTurno);
            if (!string.IsNullOrEmpty(turnoContinuidadeUsado))
            {
                mensagens.Add(
                    string.Format(
                        "O turno de Continuidade {0}, do curso: {1}, ano escolaridade: {2} não pode ser desmarcado pois já possui vagas cadastradas!",
                        turnoContinuidadeUsado, dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
            }

            //Verificar se esta sendo desmarcado um turno novo que ja tem turma na ctvConfVaga
            var turnoNovoUsado = VerificaVagasNova(dadosConfTurno);
            if (!string.IsNullOrEmpty(turnoNovoUsado))
            {
                mensagens.Add(
                    string.Format(
                        "O turno Novo {0}, do curso: {1}, ano escolaridade: {2} não pode ser desmarcado pois já possui vagas cadastradas",
                        turnoContinuidadeUsado, dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
            }

            var restricao = RN.CtvRestricao.VerificaRestricao(dadosConfTurno.IdAgendaConfTurnoVaga, dadosConfTurno.Censo);
            if (restricao)
            {
                mensagens.Add("Este censo possui uma restrição para o lançamento do CURSO: " + dadosConfTurno.NomeCurso + " SÉRIE: " + Convert.ToString(dadosConfTurno.Serie));
            }

            //Verificar se esta sendo desmarcado um turno de continuidade que ja tem aluno renovado
            var turnoContinuidadeUsadoRenovacao = VerificaRenovacaoAlunoContinuidade(dadosConfTurno);
            if (!string.IsNullOrEmpty(turnoContinuidadeUsadoRenovacao))
            {
                mensagens.Add(
                    string.Format(
                        "Não é permitido remover o turno de Continuidade {0}, do curso: {1}, ano escolaridade: {2} pois já existem alunos renovados para ele. É necessário cancelar as renovações antes.",
                        turnoContinuidadeUsadoRenovacao, dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
            }

            //Verificar se esta sendo desmarcado um turno novo que ja tem aluno renovado
            var turnoNovoUsadoRenovacao = VerificaRenovacaoAlunoNovo(dadosConfTurno);
            if (!string.IsNullOrEmpty(turnoNovoUsadoRenovacao))
            {
                mensagens.Add(
                    string.Format(
                        "Não é permitido remover o turno Novo {0}, do curso: {1}, ano escolaridade: {2} pois já existem alunos renovados para ele. É necessário cancelar as renovações antes.",
                        turnoNovoUsadoRenovacao, dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
            }

            if (codPerfil == 8)
            {
                //cria lista de strings com turnos iniciais
                var turnosInicial = MontarListaTurnosInicial(dadosConfTurno);
                //cria lista de strings com turnos finais
                var turnosFinal = MontarListaTurnosFinal(dadosConfTurno);

                foreach (var turno in turnosFinal)
                {
                    if (!turnosInicial.Exists(x => x == turno))
                    {
                        //Verifica se é curso da oferta de itinerario do ano atual, pois estes estão permitidos
                        if (!RN.Pedagogico.TrilhaAprendizagemEscola.EhCursoOfertaPor(dadosConfTurno.Ano, dadosConfTurno.Censo, dadosConfTurno.Curso, dadosConfTurno.Serie))
                        {
                            //Verifica se é curso da oferta de itinerario do ano anterior na proxima serie, pois estes estão permitidos
                            if (!RN.Pedagogico.TrilhaAprendizagemEscola.EhCursoOfertaPor(dadosConfTurno.Ano - 1, dadosConfTurno.Censo, dadosConfTurno.Curso, dadosConfTurno.Serie - 1))
                            {
                                mensagens.Add(string.Format("O turno {0}, do curso {1} e ano de escolaridade {2} não pode ser adicionado.", turno, dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
                            }
                        }
                    }
                }
            }
            if (dadosConfTurno.Finalizado)
            {
                if (!dadosConfTurno.TurnosIniciais.Equals(dadosConfTurno.Turnos)
                && string.IsNullOrEmpty(dadosConfTurno.Justificativa))
                {
                    mensagens.Add("O campo JUSTIFICATIVA CONTINUIDADE é de preenchimento obrigatório.");
                }

                if (!dadosConfTurno.TurnosIniciais.Equals(dadosConfTurno.TurnosNovo)
                    && string.IsNullOrEmpty(dadosConfTurno.JustificativaNovo))
                {
                    mensagens.Add("O campo JUSTIFICATIVA NOVO é de preenchimento obrigatório.");
                }

                if (!string.IsNullOrEmpty(dadosConfTurno.Justificativa))
                {
                    if (dadosConfTurno.Justificativa.Length < 10)
                    {
                        mensagens.Add("O campo JUSTIFICATIVA CONTINUIDADE deve ter mais 10 caracteres.");
                    }

                    if (dadosConfTurno.Justificativa.Length > 500)
                    {
                        mensagens.Add("O campo JUSTIFICATIVA CONTINUIDADE é obrigatório com o máximo de 500 caracteres!");
                    }

                    var regex = new Regex(@"(\w)\1\1+");

                    if (regex.IsMatch(dadosConfTurno.Justificativa))
                    {
                        mensagens.Add("O campo JUSTIFICATIVA CONTINUIDADE não deve ter mais de 2 letras consecutivas repetidas.");
                    }
                }

                if (!string.IsNullOrEmpty(dadosConfTurno.JustificativaNovo))
                {
                    if (dadosConfTurno.JustificativaNovo.Length < 10)
                    {
                        mensagens.Add("O campo JUSTIFICATIVA NOVO deve ter mais 10 caracteres.");
                    }

                    if (dadosConfTurno.JustificativaNovo.Length > 500)
                    {
                        mensagens.Add("O campo JUSTIFICATIVA NOVO é obrigatório com o máximo de 500 caracteres!");
                    }

                    var regex = new Regex(@"(\w)\1\1+");

                    if (regex.IsMatch(dadosConfTurno.JustificativaNovo))
                    {
                        mensagens.Add("O campo JUSTIFICATIVA NOVO não deve ter mais de 2 letras consecutivas repetidas.");
                    }
                }

                //Verifica se aquele perfil possui parametrização.
                parametroTurnoVaga = rnParametroTurnoVaga.ObtemPor(codPerfil, dadosConfTurno.AgendaId);
                if (parametroTurnoVaga.ParametroTurnoVagaId > 0)
                {
                    if (parametroTurnoVaga.AlterarTurnoModalidade != (int)RN.Agenda.ParametroTurnoVaga.ParametroTurnoVagaAlterarTurnoModalidade.NaoPermite)
                    {
                        if (parametroTurnoVaga.AlterarTurnoModalidade == (int)RN.Agenda.ParametroTurnoVaga.ParametroTurnoVagaAlterarTurnoModalidade.PermiteComRestricao)
                        {
                            //Lista quantidade de turnos cadastradas anteriormente
                            qtdeTurnosProposta = rnCtvConfTurnoInicial.ListaTurnosniciaisAgendaPor(dadosConfTurno.Censo, dadosConfTurno.Ano, dadosConfTurno.AgendaId, dadosConfTurno.IdAgendaConfTurnoVaga).Rows.Count;

                            qtdeTurnosMinima = Math.Max(RN.Agenda.ParametroTurnoVaga.NumeroMinimoTurnos, qtdeTurnosProposta - parametroTurnoVaga.VariacaoTurno);
                            qtdeTurnosMaxima = Math.Min(totalTurnos, qtdeTurnosProposta + parametroTurnoVaga.VariacaoTurno);

                            //Verifica quantos turnos estao marcados para aquela Agenda
                            qtdeTurnosFinal = MontarListaTurnosFinal(dadosConfTurno).Count();

                            //Verifica se a quantidade de turnos lançadas respeita o minimo 
                            if (qtdeTurnosFinal < qtdeTurnosMinima)
                            {
                                mensagens.Add(string.Format("Para o seu perfil, não é permitido selecionar menos de {0} turno(s) no curso: {1}, ano escolaridade: {2}.",
                                    qtdeTurnosMinima.ToString(),
                                    dadosConfTurno.NomeCurso,
                                    dadosConfTurno.Serie));
                            }

                            //Verifica se a quantidade de turnos lançadas respeita o maximo 
                            if (qtdeTurnosFinal > qtdeTurnosMaxima)
                            {
                                mensagens.Add(string.Format("Para o seu perfil, não é permitido selecionar mais de {0} turno(s) no curso: {1}, ano escolaridade: {2}.",
                                    qtdeTurnosMaxima.ToString(),
                                    dadosConfTurno.NomeCurso,
                                    dadosConfTurno.Serie));
                            }
                        }
                    }
                    else
                    {
                        mensagens.Add(string.Format("Para o seu perfil, não é permitido alterar turnos no curso: {0}, ano escolaridade: {1}.",
                            dadosConfTurno.NomeCurso,
                            dadosConfTurno.Serie));
                    }
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

        public static ValidacaoDados Validar(DadosConfTurno dadosConfTurno, int codPerfil, int totalTurnos)
        {
            var mensagens = new List<string>();
            Agenda.ParametroTurnoVaga rnParametroTurnoVaga = new Agenda.ParametroTurnoVaga();
            Agenda.Entidades.ParametroTurnoVaga parametroTurnoVaga = new Agenda.Entidades.ParametroTurnoVaga();
            CtvConfTurnoInicial rnCtvConfTurnoInicial = new CtvConfTurnoInicial();
            int qtdeTurnosFinal = 0;
            int qtdeTurnosMinima = 0;
            int qtdeTurnosMaxima = 0;
            int qtdeTurnosProposta = 0;

            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosConfTurno == null)
            {
                return validacaoDados;
            }

            if (codPerfil < 0)
            {
                mensagens.Add("Não é permitido alterar turnos para seu perfil de usuário");
            }

            if (dadosConfTurno.IdAgendaConfTurnoVaga <= 0)
            {
                mensagens.Add("O campo ID AGENDA é obrigatório!");
            }

            if (dadosConfTurno.AgendaId <= 0)
            {
                mensagens.Add("O campo  AGENDAID é obrigatório!");
            }

            if (string.IsNullOrEmpty(dadosConfTurno.Censo)
               || (!string.IsNullOrEmpty(dadosConfTurno.Censo)
                   && dadosConfTurno.Censo.Length > 8))
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é obrigatório com o máximo de 8 caracteres!");
            }

            if (!dadosConfTurno.TurnosIniciais.Equals(dadosConfTurno.Turnos)
                && string.IsNullOrEmpty(dadosConfTurno.Justificativa))
            {
                mensagens.Add("O campo JUSTIFICATIVA CONTINUIDADE é de preenchimento obrigatório.");
            }

            if (!dadosConfTurno.TurnosIniciais.Equals(dadosConfTurno.TurnosNovo)
                && string.IsNullOrEmpty(dadosConfTurno.JustificativaNovo))
            {
                mensagens.Add("O campo JUSTIFICATIVA NOVO é de preenchimento obrigatório.");
            }

            if (!string.IsNullOrEmpty(dadosConfTurno.Justificativa))
            {
                if (dadosConfTurno.Justificativa.Length < 10)
                {
                    mensagens.Add("O campo JUSTIFICATIVA CONTINUIDADE deve ter mais 10 caracteres.");
                }

                if (dadosConfTurno.Justificativa.Length > 500)
                {
                    mensagens.Add("O campo JUSTIFICATIVA CONTINUIDADE é obrigatório com o máximo de 500 caracteres!");
                }

                var regex = new Regex(@"(\w)\1\1+");

                if (regex.IsMatch(dadosConfTurno.Justificativa))
                {
                    mensagens.Add("O campo JUSTIFICATIVA CONTINUIDADE não deve ter mais de 2 letras consecutivas repetidas.");
                }
            }

            if (!string.IsNullOrEmpty(dadosConfTurno.JustificativaNovo))
            {
                if (dadosConfTurno.JustificativaNovo.Length < 10)
                {
                    mensagens.Add("O campo JUSTIFICATIVA NOVO deve ter mais 10 caracteres.");
                }

                if (dadosConfTurno.JustificativaNovo.Length > 500)
                {
                    mensagens.Add("O campo JUSTIFICATIVA NOVO é obrigatório com o máximo de 500 caracteres!");
                }

                var regex = new Regex(@"(\w)\1\1+");

                if (regex.IsMatch(dadosConfTurno.JustificativaNovo))
                {
                    mensagens.Add("O campo JUSTIFICATIVA NOVO não deve ter mais de 2 letras consecutivas repetidas.");
                }
            }

            if (string.IsNullOrEmpty(dadosConfTurno.Matricula)
                || (!string.IsNullOrEmpty(dadosConfTurno.Matricula)
                    && dadosConfTurno.Matricula.Length > 20))
            {
                mensagens.Add("O campo MATRICULA DO RESPONSÁVEL é obrigatório com o máximo de 20 caracteres!");
            }

            //cria lista de strings com turnos iniciais
            var turnosInicial = MontarListaTurnosInicial(dadosConfTurno);
            //cria lista de strings com turnos finais
            var turnosFinal = MontarListaTurnosFinal(dadosConfTurno);

            //Verifica se é diretor
            if (codPerfil == 8)
            {
                foreach (var turno in turnosFinal)
                {
                    if (!turnosInicial.Exists(x => x == turno))
                    {
                        //Verifica se é curso da oferta de itinerario do ano atual, pois estes estão permitidos
                        if (!RN.Pedagogico.TrilhaAprendizagemEscola.EhCursoOfertaPor(dadosConfTurno.Ano, dadosConfTurno.Censo, dadosConfTurno.Curso, dadosConfTurno.Serie))
                        {
                            //Verifica se é curso da oferta de itinerario do ano anterior na proxima serie, pois estes estão permitidos
                            if (!RN.Pedagogico.TrilhaAprendizagemEscola.EhCursoOfertaPor(dadosConfTurno.Ano -1 , dadosConfTurno.Censo, dadosConfTurno.Curso, dadosConfTurno.Serie - 1))
                            {
                                mensagens.Add(string.Format("O turno {0}, do curso {1} e ano de escolaridade {2} não pode ser adicionado.", turno, dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
                            }
                        }
                    }
                }

                //Verificar se esta sendo desmarcado um turno de continuidade que possui proposta
                var turnosContinuidade = SelecionarTurnosContinuidade(dadosConfTurno);
                if (string.IsNullOrEmpty(turnosContinuidade) && ExistePropostaContinuidadePor(dadosConfTurno))
                {
                    mensagens.Add(
                        string.Format("Deve estar marcado ao menos 1 turno de Continuidade do curso: {0}, ano escolaridade: {1} pois existe proposta seeduc para vagas de continuidade!",
                            dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
                }

                //Verificar se esta sendo desmarcado um turno novo que possui proposta
                var turnosNovo = SelecionarTurnosNovo(dadosConfTurno);
                if (string.IsNullOrEmpty(turnosNovo) && ExistePropostaNovaPor(dadosConfTurno))
                {
                    mensagens.Add(
                        string.Format("Deve estar marcado ao menos 1 turno Novo do curso: {0}, ano escolaridade: {1} pois existe proposta seeduc para vagas novas!",
                            dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
                }
            }

            //Verificar se esta sendo desmarcado um turno de continuidade que ja tem turma na ctvConfVaga
            var turnoContinuidadeUsado = VerificaVagasContinuidade(dadosConfTurno);
            if (!string.IsNullOrEmpty(turnoContinuidadeUsado))
            {
                mensagens.Add(
                    string.Format("O turno de Continuidade {0}, do curso: {1}, ano escolaridade: {2} não pode ser desmarcado pois já possui vagas cadastradas!",
                        turnoContinuidadeUsado, dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
            }

            //Verificar se esta sendo desmarcado um turno novo que ja tem turma na ctvConfVaga
            var turnoNovoUsado = VerificaVagasNova(dadosConfTurno);
            if (!string.IsNullOrEmpty(turnoNovoUsado))
            {
                mensagens.Add(
                    string.Format("O turno Novo {0}, do curso: {1}, ano escolaridade: {2} não pode ser desmarcado pois já possui vagas cadastradas!",
                        turnoContinuidadeUsado, dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
            }

            var restricao = RN.CtvRestricao.VerificaRestricao(dadosConfTurno.IdAgendaConfTurnoVaga, dadosConfTurno.Censo);
            if (restricao)
            {
                mensagens.Add("Este censo possui uma restrição para o lançamento do CURSO: " + dadosConfTurno.NomeCurso + " SÉRIE: " + Convert.ToString(dadosConfTurno.Serie));
            }

            //Verifica se aquele perfil possui parametrização.
            parametroTurnoVaga = rnParametroTurnoVaga.ObtemPor(codPerfil, dadosConfTurno.AgendaId);
            if (parametroTurnoVaga.ParametroTurnoVagaId > 0)
            {
                if (parametroTurnoVaga.AlterarTurnoModalidade != (int)RN.Agenda.ParametroTurnoVaga.ParametroTurnoVagaAlterarTurnoModalidade.NaoPermite)
                {
                    if (parametroTurnoVaga.AlterarTurnoModalidade == (int)RN.Agenda.ParametroTurnoVaga.ParametroTurnoVagaAlterarTurnoModalidade.PermiteComRestricao)
                    {
                        //Lista quantidade de turnos cadastradas anteriormente
                        qtdeTurnosProposta = rnCtvConfTurnoInicial.ListaTurnosniciaisAgendaPor(dadosConfTurno.Censo, dadosConfTurno.Ano, dadosConfTurno.AgendaId, dadosConfTurno.IdAgendaConfTurnoVaga).Rows.Count;

                        qtdeTurnosMinima = Math.Max(RN.Agenda.ParametroTurnoVaga.NumeroMinimoTurnos, qtdeTurnosProposta - parametroTurnoVaga.VariacaoTurno);
                        qtdeTurnosMaxima = Math.Min(totalTurnos, qtdeTurnosProposta + parametroTurnoVaga.VariacaoTurno);

                        //Verifica quantos turnos estao marcados para aquela Agenda
                        qtdeTurnosFinal = MontarListaTurnosFinal(dadosConfTurno).Count();

                        //Verifica se a quantidade de turnos lançadas respeita o minimo 
                        if (qtdeTurnosFinal < qtdeTurnosMinima)
                        {
                            mensagens.Add(string.Format("Para o seu perfil, não é permitido selecionar menos de {0} turno(s) no curso: {1}, ano escolaridade: {2}.",
                                qtdeTurnosMinima.ToString(),
                                dadosConfTurno.NomeCurso,
                                dadosConfTurno.Serie));
                        }

                        //Verifica se a quantidade de turnos lançadas respeita o maximo 
                        if (qtdeTurnosFinal > qtdeTurnosMaxima)
                        {
                            mensagens.Add(string.Format("Para o seu perfil, não é permitido selecionar mais de {0} turno(s) no curso: {1}, ano escolaridade: {2}.",
                                qtdeTurnosMaxima.ToString(),
                                dadosConfTurno.NomeCurso,
                                dadosConfTurno.Serie));
                        }
                    }
                }
                else
                {
                    mensagens.Add(string.Format("Não é permitido alterar turnos no curso: {0}, ano escolaridade: {1} para seu perfil de usuário",
                        dadosConfTurno.NomeCurso,
                        dadosConfTurno.Serie));
                }
            }
            //Verificar se esta sendo desmarcado um turno de continuidade que ja tem aluno renovado
            var turnoContinuidadeUsadoRenovacao = VerificaRenovacaoAlunoContinuidade(dadosConfTurno);
            if (!string.IsNullOrEmpty(turnoContinuidadeUsadoRenovacao))
            {
                mensagens.Add(
                    string.Format(
                        "Não é permitido remover o turno de Continuidade {0}, do curso: {1}, ano escolaridade: {2} pois já existem alunos renovados para ele. É necessário cancelar as renovações antes.",
                        turnoContinuidadeUsadoRenovacao, dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
            }

            //Verificar se esta sendo desmarcado um turno novo que ja tem aluno renovado
            var turnoNovoUsadoRenovacao = VerificaRenovacaoAlunoNovo(dadosConfTurno);
            if (!string.IsNullOrEmpty(turnoNovoUsadoRenovacao))
            {
                mensagens.Add(
                    string.Format(
                        "Não é permitido remover o turno Novo {0}, do curso: {1}, ano escolaridade: {2} pois já existem alunos renovados para ele. É necessário cancelar as renovações antes.",
                        turnoNovoUsadoRenovacao, dadosConfTurno.NomeCurso, dadosConfTurno.Serie));
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

        public static List<string> MontarListaTurnosInicial(DadosConfTurno dadosConfTurno)
        {
            var lista = new List<string>();
            if (dadosConfTurno.TurnosListaInicial.Contains("M"))
            {
                lista.Add("M");
            }
            if (dadosConfTurno.TurnosListaInicial.Contains("T"))
            {
                lista.Add("T");
            }
            if (dadosConfTurno.TurnosListaInicial.Contains("N"))
            {
                lista.Add("N");
            }
            if (dadosConfTurno.TurnosListaInicial.Contains("A"))
            {
                lista.Add("A");
            }
            if (dadosConfTurno.TurnosListaInicial.Contains("I"))
            {
                lista.Add("I");
            }
            return lista;
        }

        public static List<string> MontarListaTurnosFinal(DadosConfTurno dadosConfTurno)
        {
            var lista = new List<string>();
            if (dadosConfTurno.Manha || dadosConfTurno.ManhaNovo)
            {
                lista.Add("M");
            }
            if (dadosConfTurno.Tarde || dadosConfTurno.TardeNovo)
            {
                lista.Add("T");
            }
            if (dadosConfTurno.Noite || dadosConfTurno.NoiteNovo)
            {
                lista.Add("N");
            }
            if (dadosConfTurno.Ampliado || dadosConfTurno.AmpliadoNovo)
            {
                lista.Add("A");
            }
            if (dadosConfTurno.Integral || dadosConfTurno.IntegralNovo)
            {
                lista.Add("I");
            }
            return lista;
        }

        public static void Salvar(ICollection<DadosConfTurno> dadosConfTurno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    //var agenda = CtvAgendaConfTurnoVaga.Carregar(dadosConfTurno.Select(x => x.IdAgendaConfTurnoVaga).First());

                    //criar os DTO com os dados gerais para matricula Facil
                    RN.DTOs.ConsolidaTurnosDoMatriculaFacil objConsolida = new ConsolidaTurnosDoMatriculaFacil
                       {
                           UnidadeEnsino = dadosConfTurno.Select(x => x.Censo).First(),
                           NomeUnidadeEnsino = RN.UnidadeEnsino.RetornaNomeUnidadeEnsino(dadosConfTurno.Select(x => x.Censo).First()),
                           Ano = Convert.ToString(dadosConfTurno.Select(x => x.Ano).First()),
                           Periodo = Convert.ToString(dadosConfTurno.Select(x => x.Periodo).First())
                       };

                    //cria a lista onde serão acrescentados os turnos do Matricula Facil
                    IList<DetalhesConsolidaTurnosDoMatriculaFacil> listaDetalhes = new List<DetalhesConsolidaTurnosDoMatriculaFacil>();

                    foreach (var confTurno in dadosConfTurno)
                    {
                        SalvarTurnos(ctx, confTurno, listaDetalhes);

                        //Monta a justificativa
                        var ctvJustificativa = new TceCtvJustificativa
                           {
                               IdAgendaConfTurnoVaga = confTurno.IdAgendaConfTurnoVaga,
                               Censo = confTurno.Censo,
                               JustificativaContinuidade = confTurno.Justificativa,
                               JustificativaNovo = confTurno.JustificativaNovo,
                               Matricula = confTurno.Matricula,
                               Vaga = false,
                               Turno = true
                           };
                        //Verifica se justificativa ja existe para a agenda censo
                        var idJustificativa = CtvJustificativa.RetornaIdJustificativa(ctvJustificativa);

                        if (idJustificativa > 0)
                        {
                            ctvJustificativa.IdJustificativa = idJustificativa;
                            CtvJustificativa.Alterar(ctx, ctvJustificativa);
                        }
                        else
                        {
                            CtvJustificativa.Inserir(ctx, ctvJustificativa);
                        }
                    }

                    //joga a lista que foi carregada pelo Salvar no objeto

                    if (listaDetalhes.Count > 0)
                    {
                        objConsolida.DetalhesConsolidaTurnosDoMatriculaFacil = listaDetalhes;
                        //objConsolida.ExecutaConsolidaTurnosDoMatriculaFacil(objConsolida);
                    }
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public void SalvaComAnalise(ICollection<DadosConfTurno> dadosConfTurno, List<TceCtvAnalise> analises, string perfil)
        {
            RN.TurnosVagas.HistoricoTurno rnHistoricoTurno = new RN.TurnosVagas.HistoricoTurno();
            int ano = 0;
            string censo = string.Empty;
            int idAnalise = 0;
            IEnumerable<string> listaPeriodos = new List<string>();
            string periodos = string.Empty;


            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    ano = analises.Select(x => x.Ano).First();
                    censo = analises.Select(x => x.Censo).First();
                    listaPeriodos = analises.Select(x => Convert.ToString(x.Periodo)).Distinct();
                    periodos = listaPeriodos.Aggregate((x, y) => x + " , " + y);

                    //Salvar dados atuais em historico do diretor (turnos e justificativa)
                    rnHistoricoTurno.SalvaHistoricoTurnoDiretor(ctx, ano, periodos, censo);

                    if (dadosConfTurno.Count > 0)
                    {
                        //criar os DTO com os dados gerais para matricula Facil
                        RN.DTOs.ConsolidaTurnosDoMatriculaFacil objConsolida = new ConsolidaTurnosDoMatriculaFacil
                        {
                            UnidadeEnsino = censo,
                            NomeUnidadeEnsino = RN.UnidadeEnsino.RetornaNomeUnidadeEnsino(dadosConfTurno.Select(x => x.Censo).First()),
                            Ano = Convert.ToString(ano),
                            Periodo = Convert.ToString(dadosConfTurno.Select(x => x.Periodo).First())
                        };

                        //cria a lista onde serão acrescentados os turnos do Matricula Facil
                        IList<DetalhesConsolidaTurnosDoMatriculaFacil> listaDetalhes = new List<DetalhesConsolidaTurnosDoMatriculaFacil>();

                        foreach (var confTurno in dadosConfTurno)
                        {
                            SalvarTurnos(ctx, confTurno, listaDetalhes);

                            //Monta a justificativa
                            var ctvJustificativa = new TceCtvJustificativa
                            {
                                IdAgendaConfTurnoVaga = confTurno.IdAgendaConfTurnoVaga,
                                Censo = confTurno.Censo,
                                JustificativaContinuidade = confTurno.Justificativa,
                                JustificativaNovo = confTurno.JustificativaNovo,
                                Matricula = confTurno.Matricula,
                                Vaga = false,
                                Turno = true
                            };

                            //Verifica se justificativa ja existe para a agenda censo
                            var idJustificativa = CtvJustificativa.RetornaIdJustificativa(ctvJustificativa);

                            if (idJustificativa > 0)
                            {
                                ctvJustificativa.IdJustificativa = idJustificativa;
                                CtvJustificativa.Alterar(ctx, ctvJustificativa);
                            }
                            else
                            {
                                CtvJustificativa.Inserir(ctx, ctvJustificativa);
                            }
                        }

                        //joga a lista que foi carregada pelo Salvar no objeto
                        if (listaDetalhes.Count > 0)
                        {
                            objConsolida.DetalhesConsolidaTurnosDoMatriculaFacil = listaDetalhes;
                            //objConsolida.ExecutaConsolidaTurnosDoMatriculaFacil(objConsolida);
                        }
                    }
                    //Salva dados atualizados em historico de seeduc (turnos e justificativa)
                    rnHistoricoTurno.SalvaHistoricoTurnoSeeduc(ctx, ano, periodos, censo);

                    //Salva analises
                    foreach (TceCtvAnalise analise in analises)
                    {
                        //Verifica se analise já existe
                        idAnalise = CtvAnalise.RetornaIdAnalise(analise);

                        if (idAnalise > 0)
                        {
                            analise.IdAnalise = idAnalise;
                            CtvAnalise.Alterar(ctx, analise, perfil);
                        }
                        else
                        {
                            CtvAnalise.Inserir(ctx, analise);
                        }
                    }
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        private static void SalvarTurnos(DataContext dataContext, DadosConfTurno dadosConfTurno, IList<DetalhesConsolidaTurnosDoMatriculaFacil> listaDetalhes)
        {
            // Remover os turnos que não mais existentes
            var turnosContinuidade = SelecionarTurnosContinuidade(dadosConfTurno);
            RemoverTurnosContinuidade(dataContext, dadosConfTurno, turnosContinuidade);

            var turnosNovo = SelecionarTurnosNovo(dadosConfTurno);
            RemoverTurnosNovo(dataContext, dadosConfTurno, turnosNovo, listaDetalhes);

            // Inserir turnos
            var ctvConfTurno = new TceCtvConfTurno
            {
                IdAgendaConfTurnoVaga = dadosConfTurno.IdAgendaConfTurnoVaga,
                Matricula = dadosConfTurno.Matricula,
                Censo = dadosConfTurno.Censo,
                Confirmada = true,
                Continuidade = false,
                Novo = false
            };

            if (dadosConfTurno.Manha
                && string.IsNullOrEmpty(dadosConfTurno.ManhaCodigo))
            {
                ctvConfTurno.Turno = "M";
                ctvConfTurno.Continuidade = true;
                ctvConfTurno.Novo = false;
                Inserir(dataContext, ctvConfTurno);
            }

            if (dadosConfTurno.ManhaNovo
                && string.IsNullOrEmpty(dadosConfTurno.ManhaNovoCodigo))
            {
                ctvConfTurno.Turno = "M";
                ctvConfTurno.Novo = true;
                ctvConfTurno.Continuidade = false;
                Inserir(dataContext, ctvConfTurno);

                //Inserir na lista de detalhes para o Matricula facil
                DetalhesConsolidaTurnosDoMatriculaFacil detalhe = new DetalhesConsolidaTurnosDoMatriculaFacil
                  {
                      Curso = dadosConfTurno.Curso,
                      NomeCurso = dadosConfTurno.NomeCurso,
                      ModalidadeCurso = dadosConfTurno.CodigoModalidade,
                      Serie = Convert.ToString(dadosConfTurno.Serie),
                      TipoCurso = dadosConfTurno.CodigoTipo,
                      TipoOperacao = "I", //to tipo insert
                      Turno = ctvConfTurno.Turno
                  };
                listaDetalhes.Add(detalhe);
            }

            if (dadosConfTurno.Tarde
                && string.IsNullOrEmpty(dadosConfTurno.TardeCodigo))
            {
                ctvConfTurno.Turno = "T";
                ctvConfTurno.Continuidade = true;
                ctvConfTurno.Novo = false;
                Inserir(dataContext, ctvConfTurno);
            }

            if (dadosConfTurno.TardeNovo
                && string.IsNullOrEmpty(dadosConfTurno.TardeNovoCodigo))
            {
                ctvConfTurno.Turno = "T";
                ctvConfTurno.Novo = true;
                ctvConfTurno.Continuidade = false;
                Inserir(dataContext, ctvConfTurno);

                //Inserir na lista de detalhes para o Matricula facil
                DetalhesConsolidaTurnosDoMatriculaFacil detalhe = new DetalhesConsolidaTurnosDoMatriculaFacil
                {
                    Curso = dadosConfTurno.Curso,
                    NomeCurso = dadosConfTurno.NomeCurso,
                    ModalidadeCurso = dadosConfTurno.CodigoModalidade,
                    Serie = Convert.ToString(dadosConfTurno.Serie),
                    TipoCurso = dadosConfTurno.CodigoTipo,
                    TipoOperacao = "I", //to tipo insert
                    Turno = ctvConfTurno.Turno
                };
                listaDetalhes.Add(detalhe);
            }

            if (dadosConfTurno.Noite
                && string.IsNullOrEmpty(dadosConfTurno.NoiteCodigo))
            {
                ctvConfTurno.Turno = "N";
                ctvConfTurno.Continuidade = true;
                ctvConfTurno.Novo = false;
                Inserir(dataContext, ctvConfTurno);
            }

            if (dadosConfTurno.NoiteNovo
                && string.IsNullOrEmpty(dadosConfTurno.NoiteNovoCodigo))
            {
                ctvConfTurno.Turno = "N";
                ctvConfTurno.Novo = true;
                ctvConfTurno.Continuidade = false;
                Inserir(dataContext, ctvConfTurno);

                //Inserir na lista de detalhes para o Matricula facil
                DetalhesConsolidaTurnosDoMatriculaFacil detalhe = new DetalhesConsolidaTurnosDoMatriculaFacil
                {
                    Curso = dadosConfTurno.Curso,
                    NomeCurso = dadosConfTurno.NomeCurso,
                    ModalidadeCurso = dadosConfTurno.CodigoModalidade,
                    Serie = Convert.ToString(dadosConfTurno.Serie),
                    TipoCurso = dadosConfTurno.CodigoTipo,
                    TipoOperacao = "I", //to tipo insert
                    Turno = ctvConfTurno.Turno
                };
                listaDetalhes.Add(detalhe);
            }

            if (dadosConfTurno.Integral
                && string.IsNullOrEmpty(dadosConfTurno.IntegralCodigo))
            {
                ctvConfTurno.Turno = "I";
                ctvConfTurno.Continuidade = true;
                ctvConfTurno.Novo = false;
                Inserir(dataContext, ctvConfTurno);
            }

            if (dadosConfTurno.IntegralNovo
                && string.IsNullOrEmpty(dadosConfTurno.IntegralNovoCodigo))
            {
                ctvConfTurno.Turno = "I";
                ctvConfTurno.Novo = true;
                ctvConfTurno.Continuidade = false;
                Inserir(dataContext, ctvConfTurno);

                //Inserir na lista de detalhes para o Matricula facil
                DetalhesConsolidaTurnosDoMatriculaFacil detalhe = new DetalhesConsolidaTurnosDoMatriculaFacil
                {
                    Curso = dadosConfTurno.Curso,
                    NomeCurso = dadosConfTurno.NomeCurso,
                    ModalidadeCurso = dadosConfTurno.CodigoModalidade,
                    Serie = Convert.ToString(dadosConfTurno.Serie),
                    TipoCurso = dadosConfTurno.CodigoTipo,
                    TipoOperacao = "I", //to tipo insert
                    Turno = ctvConfTurno.Turno
                };
                listaDetalhes.Add(detalhe);
            }

            if (dadosConfTurno.Ampliado
                && string.IsNullOrEmpty(dadosConfTurno.AmpliadoCodigo))
            {
                ctvConfTurno.Turno = "A";
                ctvConfTurno.Continuidade = true;
                ctvConfTurno.Novo = false;
                Inserir(dataContext, ctvConfTurno);
            }

            if (dadosConfTurno.AmpliadoNovo
                && string.IsNullOrEmpty(dadosConfTurno.AmpliadoNovoCodigo))
            {
                ctvConfTurno.Turno = "A";
                ctvConfTurno.Novo = true;
                ctvConfTurno.Continuidade = false;
                Inserir(dataContext, ctvConfTurno);

                //Inserir na lista de detalhes para o Matricula facil
                DetalhesConsolidaTurnosDoMatriculaFacil detalhe = new DetalhesConsolidaTurnosDoMatriculaFacil
                {
                    Curso = dadosConfTurno.Curso,
                    NomeCurso = dadosConfTurno.NomeCurso,
                    ModalidadeCurso = dadosConfTurno.CodigoModalidade,
                    Serie = Convert.ToString(dadosConfTurno.Serie),
                    TipoCurso = dadosConfTurno.CodigoTipo,
                    TipoOperacao = "I", //to tipo insert
                    Turno = ctvConfTurno.Turno
                };
                listaDetalhes.Add(detalhe);
            }

            Confirmar(dataContext, ctvConfTurno);
        }

        public static void Confirmar(DataContext context, TceCtvConfTurno ctvConfTurno)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE TCE_CTV_CONF_TURNO
                     SET    CONFIRMADA = 1 ,
                            MATRICULA = @MATRICULA ,
                            DT_ALTERACAO = GETDATE()
                     WHERE  ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                            AND CENSO = @CENSO ");

            contextQuery.Parameters.Add("@MATRICULA", ctvConfTurno.Matricula);
            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvConfTurno.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CENSO", ctvConfTurno.Censo);

            context.ApplyModifications(contextQuery);
        }

        private static string SelecionarTurnosContinuidade(DadosConfTurno dadosConfTurno)
        {
            var turnos = new List<string>();

            if (dadosConfTurno.Manha)
            {
                turnos.Add("'M'");
            }

            if (dadosConfTurno.Tarde)
            {
                turnos.Add("'T'");
            }

            if (dadosConfTurno.Noite)
            {
                turnos.Add("'N'");
            }

            if (dadosConfTurno.Integral)
            {
                turnos.Add("'I'");
            }

            if (dadosConfTurno.Ampliado)
            {
                turnos.Add("'A'");
            }

            if (turnos.Count == 0)
            {
                return string.Empty;
            }

            return turnos.Aggregate((x, y) => string.Format("{0}, {1}", x, y));
        }

        private static string VerificaVagasContinuidade(DadosConfTurno dadosConfTurno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var turnosContinuidade = SelecionarTurnosContinuidade(dadosConfTurno);

                var contextQuery = new ContextQuery
                {
                    Command =
                        string.Format(@" SELECT  T.DESCRICAO AS TURNO
                                FROM    DBO.TCE_CTV_CONF_VAGA cv
                                        INNER JOIN ly_turno t ON cv.TURNO = t.TURNO
                        WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND CENSO = @CENSO
                                AND cv.VAGAS_CONTINUIDADE > 0
                            {0}",
                    string.IsNullOrEmpty(turnosContinuidade) ? string.Empty : string.Format("AND cv.TURNO NOT IN ({0})", turnosContinuidade))
                };

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", dadosConfTurno.IdAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", dadosConfTurno.Censo);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        return Convert.ToString(reader["TURNO"]);
                    }
                }
            }

            return string.Empty;
        }

        private static string VerificaVagasNova(DadosConfTurno dadosConfTurno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var turnosNovo = SelecionarTurnosNovo(dadosConfTurno);

                var contextQuery = new ContextQuery
                {
                    Command =
                        string.Format(@" SELECT  T.DESCRICAO AS TURNO
                                FROM    DBO.TCE_CTV_CONF_VAGA cv
                                        INNER JOIN ly_turno t ON cv.TURNO = t.TURNO
                        WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND CENSO = @CENSO
                                AND cv.VAGAS_NOVAS > 0
                            {0}",
                    string.IsNullOrEmpty(turnosNovo) ? string.Empty : string.Format("AND cv.TURNO NOT IN ({0})", turnosNovo))
                };

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", dadosConfTurno.IdAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", dadosConfTurno.Censo);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        return Convert.ToString(reader["TURNO"]);
                    }
                }
            }

            return string.Empty;
        }

        private static bool ExistePropostaContinuidadePor(DadosConfTurno dadosConfTurno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                            FROM    TCE_CTV_PROPOSTA_SEEDUC PS
                                    WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                            AND CENSO = @CENSO
                                            AND PS.VAGAS_CONTINUIDADE > 0 ";

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", dadosConfTurno.IdAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", dadosConfTurno.Censo);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
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
                contexto.Dispose();
            }
        }

        private static bool ExistePropostaNovaPor(DadosConfTurno dadosConfTurno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                            FROM    TCE_CTV_PROPOSTA_SEEDUC PS
                                    WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                            AND CENSO = @CENSO
                                            AND PS.VAGAS_NOVAS > 0 ";

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", dadosConfTurno.IdAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", dadosConfTurno.Censo);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
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
                contexto.Dispose();
            }
        }

        public bool EhTurnoFinalizadoPor(int ano, string censo, int tipoEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                FROM    DBO.TCE_CTV_CONF_TURNO T
                                        INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON T.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                        LEFT JOIN DBO.TCE_CTV_FINALIZADO F ON T.ID_AGENDA_CONF_TURNO_VAGA = F.ID_AGENDA_CONF_TURNO_VAGA
                                                                              AND T.CENSO = F.CENSO
                                                                              AND F.TURNO = 1
                                WHERE   A.ANO = @ANO
                                        AND A.ENCERRADO = 0
                                        AND a.PERIODO IN (
                                        SELECT  PERIODO
                                        FROM    AGENDA.PERIODOLETIVOAGENDA P
                                                INNER JOIN agenda.AGENDA AA ON aa.AGENDAID = P.AGENDAID
                                                INNER JOIN agenda.EVENTO AE ON AA.AGENDAID = AE.AGENDAID
                                        WHERE   GETDATE() BETWEEN DATAINICIO AND DATAFIM
                                                AND AE.TIPOEVENTOID = @TIPOEVENTOID --TipoEvento Copnfirmação de Vagas
                                                AND ANO = a.ANO )
                                        AND T.CENSO = @CENSO
                                        AND ( A.DT_FIM_CONF_TURNO < CAST(GETDATE() AS DATE)
                                              OR F.ID_FINALIZADO IS NOT NULL
                                            ) ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

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

        public static bool VerificaFinalizacao(int ano, int periodo, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                1
                        FROM    DBO.TCE_CTV_CONF_TURNO T
                                INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON T.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                LEFT JOIN DBO.TCE_CTV_FINALIZADO F ON T.ID_AGENDA_CONF_TURNO_VAGA = F.ID_AGENDA_CONF_TURNO_VAGA
                                                                      AND T.CENSO = F.CENSO
                                                                      AND F.TURNO = 1
                        WHERE   A.ANO = @ANO
                                AND A.PERIODO = @PERIODO
                                AND T.CENSO = @CENSO
                                AND ( A.DT_FIM_CONF_TURNO < CAST(GETDATE() AS DATE)
                                      OR F.ID_FINALIZADO IS NOT NULL
                                    ) "
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

        public static bool VerificaTurnoContinuidade(int idAgenda, string censo, string turno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                1
                        FROM    dbo.TCE_CTV_CONF_TURNO
                        WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND CENSO = @CENSO
                                AND TURNO = @TURNO
                                AND CONTINUIDADE = 1 "
                };

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURNO", turno);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool VerificaTurnoNovo(int idAgenda, string censo, string turno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                1
                        FROM    dbo.TCE_CTV_CONF_TURNO
                        WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND CENSO = @CENSO
                                AND TURNO = @TURNO
                                AND NOVO = 1 "
                };

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURNO", turno);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        private static string SelecionarTurnosNovo(DadosConfTurno dadosConfTurno)
        {
            var turnos = new List<string>();

            if (dadosConfTurno.ManhaNovo)
            {
                turnos.Add("'M'");
            }

            if (dadosConfTurno.TardeNovo)
            {
                turnos.Add("'T'");
            }

            if (dadosConfTurno.NoiteNovo)
            {
                turnos.Add("'N'");
            }

            if (dadosConfTurno.IntegralNovo)
            {
                turnos.Add("'I'");
            }

            if (dadosConfTurno.AmpliadoNovo)
            {
                turnos.Add("'A'");
            }

            if (turnos.Count == 0)
            {
                return string.Empty;
            }

            return turnos.Aggregate((x, y) => string.Format("{0}, {1}", x, y));
        }

        private static void BuscarTurnosRemovidos(DadosConfTurno dadosConfTurno, string turnos, IList<DetalhesConsolidaTurnosDoMatriculaFacil> listaDetalhes)
        {
            //Verifica todos os cursos que serão excluidos para adicionar na lista do matricula facil
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery(
                                string.Format(
                                    @"SELECT TURNO  FROM dbo.TCE_CTV_CONF_TURNO
                        WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND NOVO = 1
                                AND CENSO = @CENSO
                            {0}",
                                    string.IsNullOrEmpty(turnos) ? string.Empty : string.Format("AND TURNO NOT IN ({0})", turnos)));

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", dadosConfTurno.IdAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", dadosConfTurno.Censo);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        //Inserir na lista de detalhes para o Matricula facil
                        DetalhesConsolidaTurnosDoMatriculaFacil detalhe = new DetalhesConsolidaTurnosDoMatriculaFacil
                        {
                            Curso = dadosConfTurno.Curso,
                            NomeCurso = dadosConfTurno.NomeCurso,
                            ModalidadeCurso = dadosConfTurno.CodigoModalidade,
                            Serie = Convert.ToString(dadosConfTurno.Serie),
                            TipoCurso = dadosConfTurno.CodigoTipo,
                            TipoOperacao = "E", //to tipo Excluir
                            Turno = Convert.ToString(reader["TURNO"])
                        };
                        listaDetalhes.Add(detalhe);
                    }
                }
            }
        }

        private static void RemoverTurnosNovo(DataContext dataContext, DadosConfTurno dadosConfTurno, string turnos, IList<DetalhesConsolidaTurnosDoMatriculaFacil> listaDetalhes)
        {
            BuscarTurnosRemovidos(dadosConfTurno, turnos, listaDetalhes);

            var contextQuery = new ContextQuery(
                string.Format(
                    @"DELETE  FROM dbo.TCE_CTV_CONF_TURNO
                        WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND NOVO = 1
                                AND CENSO = @CENSO
                            {0}",
                    string.IsNullOrEmpty(turnos) ? string.Empty : string.Format("AND TURNO NOT IN ({0})", turnos)));

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", dadosConfTurno.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CENSO", dadosConfTurno.Censo);

            dataContext.ApplyModifications(contextQuery);
        }

        private static void RemoverTurnosContinuidade(DataContext dataContext, DadosConfTurno dadosConfTurno, string turnos)
        {
            var contextQuery = new ContextQuery(
                string.Format(
                    @"DELETE  FROM dbo.TCE_CTV_CONF_TURNO
                        WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND CONTINUIDADE = 1
                                AND CENSO = @CENSO
                            {0}",
                    string.IsNullOrEmpty(turnos) ? string.Empty : string.Format("AND TURNO NOT IN ({0})", turnos)));

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", dadosConfTurno.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CENSO", dadosConfTurno.Censo);

            dataContext.ApplyModifications(contextQuery);
        }

        public static string RetornarSeriesManha(ICollection<DadosConfTurno> dadosConfTurno, string modalidade)
        {
            var listaSeries = string.Empty;

            var series = dadosConfTurno.Where(x => ((x.Manha || x.ManhaNovo) && (x.Modalidade == modalidade)))
                .Select(x => x.DescricaoSerie);

            foreach (var serie in series)
            {
                if (!string.IsNullOrEmpty(listaSeries))
                {
                    listaSeries = listaSeries + " / ";
                }

                listaSeries = listaSeries + serie;
            }

            return listaSeries;
        }

        public static string RetornarSeriesTarde(ICollection<DadosConfTurno> dadosConfTurno, string modalidade)
        {
            var listaSeries = string.Empty;

            var series = dadosConfTurno.Where(x => ((x.Tarde || x.TardeNovo) && (x.Modalidade == modalidade)))
                .Select(x => x.DescricaoSerie);

            foreach (var serie in series)
            {
                if (!string.IsNullOrEmpty(listaSeries))
                {
                    listaSeries = listaSeries + " / ";
                }

                listaSeries = listaSeries + serie;
            }

            return listaSeries;
        }

        public static string RetornarSeriesNoite(ICollection<DadosConfTurno> dadosConfTurno, string modalidade)
        {
            var listaSeries = string.Empty;

            var series = dadosConfTurno.Where(x => ((x.Noite || x.NoiteNovo) && (x.Modalidade == modalidade)))
                .Select(x => x.DescricaoSerie);

            foreach (var serie in series)
            {
                if (!string.IsNullOrEmpty(listaSeries))
                {
                    listaSeries = listaSeries + " / ";
                }
                listaSeries = listaSeries + serie;
            }

            return listaSeries;
        }

        public static string RetornarSeriesAmpliado(ICollection<DadosConfTurno> dadosConfTurno, string modalidade)
        {
            var listaSeries = string.Empty;

            var series = dadosConfTurno.Where(x => ((x.Ampliado || x.AmpliadoNovo) && (x.Modalidade == modalidade)))
                .Select(x => x.DescricaoSerie);

            foreach (var serie in series)
            {
                if (!string.IsNullOrEmpty(listaSeries))
                {
                    listaSeries = listaSeries + " / ";
                }
                listaSeries = listaSeries + serie;
            }

            return listaSeries;
        }

        public static string RetornarSeriesIntegral(ICollection<DadosConfTurno> dadosConfTurno, string modalidade)
        {
            var listaSeries = string.Empty;

            var series = dadosConfTurno.Where(x => ((x.Integral || x.IntegralNovo) && (x.Modalidade == modalidade)))
                .Select(x => x.DescricaoSerie);

            foreach (var serie in series)
            {
                if (!string.IsNullOrEmpty(listaSeries))
                {
                    listaSeries = listaSeries + " / ";
                }
                listaSeries = listaSeries + serie;
            }

            return listaSeries;
        }

        public IList<String> ListaTurnosParaLancamentoVagasPor(string censo, int ano, int tipoEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            List<String> lista = new List<String>();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                T.TURNO
                        FROM    TCE_CTV_CONF_TURNO T
                                INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA a ON T.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA V ON T.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                                LEFT JOIN DBO.TCE_CTV_FINALIZADO F ON T.ID_AGENDA_CONF_TURNO_VAGA = F.ID_AGENDA_CONF_TURNO_VAGA
                                                                      AND T.CENSO = F.CENSO
                                                                      AND F.TURNO = 1
                        WHERE   T.CENSO = @CENSO
                                AND V.ANO = @ANO
                                AND A.ENCERRADO = 0
                                AND ( V.DT_FIM_CONF_TURNO < CONVERT(DATE, GETDATE())
                                      OR ID_FINALIZADO IS NOT NULL
                                    )
                                AND NOT EXISTS ( SELECT 1
                                                 FROM   DBO.TCE_CTV_RESTRICAO RE
                                                 WHERE  ID_AGENDA_CONF_TURNO_VAGA = T.ID_AGENDA_CONF_TURNO_VAGA
                                                        AND T.CENSO = RE.CENSO )
                                AND V.PERIODO IN (
                                SELECT  PERIODO
                                FROM    AGENDA.PERIODOLETIVOAGENDA P
                                        INNER JOIN agenda.AGENDA AA ON aa.AGENDAID = P.AGENDAID
                                        INNER JOIN agenda.EVENTO AE ON AA.AGENDAID = AE.AGENDAID
                                WHERE   GETDATE() BETWEEN DATAINICIO AND DATAFIM
                                        AND AE.TIPOEVENTOID = @TIPOEVENTOID
                                        AND ANO = V.ANO)--TipoEvento Copnfirmação de Vagas  ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lista.Add(Convert.ToString(reader["TURNO"]));
                }

                return lista;
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

        public static IList<String> RetornaTurnosPorCensoAnoPeriodo(string censo, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT DISTINCT
                                T.TURNO
                        FROM    TCE_CTV_CONF_TURNO T
                                INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA V ON T.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                                LEFT JOIN DBO.TCE_CTV_FINALIZADO F ON T.ID_AGENDA_CONF_TURNO_VAGA = F.ID_AGENDA_CONF_TURNO_VAGA
                                                                      AND T.CENSO = F.CENSO
                                                                      AND F.TURNO = 1      
                        WHERE   T.CENSO = @CENSO
                                AND V.ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND ( V.DT_FIM_CONF_TURNO < CONVERT(DATE, GETDATE())
                                      OR ID_FINALIZADO IS NOT NULL
                                    ) "
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                var lista = new List<String>();

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        lista.Add(Convert.ToString(reader["TURNO"]));
                    }
                }

                return lista;
            }
        }

        public static void Excluir(DataContext context, int idAgenda)
        {
            var contextQuery = new ContextQuery(
                @" DELETE  DBO.TCE_CTV_CONF_TURNO
                       WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA ");

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

            context.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaInclusaoModalidadeSerie(DTOs.DadosInclusaoModalidadeSerieTurnosVagas modalidadeSerie)
        {
            List<string> mensagens = new List<string>();
            string descricaoTurno = string.Empty;
            CtvConfTurnoInicial rnCtvConfTurnoInicial = new CtvConfTurnoInicial();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (modalidadeSerie == null)
            {
                return validacaoDados;
            }

            if (modalidadeSerie.Ano <= 0)
            {
                mensagens.Add("O campo Ano é de preenchimento obrigatório.");
            }

            if (modalidadeSerie.Periodo < 0)
            {
                mensagens.Add("O campo Período é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(modalidadeSerie.Censo)
                || (!string.IsNullOrEmpty(modalidadeSerie.Censo) && modalidadeSerie.Censo.Length > 8))
            {
                mensagens.Add("O campo Unidade de Ensino é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(modalidadeSerie.Curso))
            {
                mensagens.Add("O campo Curso é de preenchimento obrigatório.");
            }

            if (modalidadeSerie.Periodo < 0)
            {
                mensagens.Add("O campo Série é de preenchimento obrigatório.");
            }

            if (modalidadeSerie.ListaTurnos.Count <= 0)
            {
                mensagens.Add("É necessario selecionar ao menos um turno.");
            }
            else
            {
                foreach (DadosTurnoInclusaoModalidadeSerie turno in modalidadeSerie.ListaTurnos)
                {
                    if (string.IsNullOrEmpty(turno.Turno))
                    {
                        mensagens.Add("Campo TURNO não encontrado!");
                    }
                    else
                    {
                        switch (turno.Turno)
                        {
                            case "M":
                                descricaoTurno = "Manhã";
                                break;
                            case "T":
                                descricaoTurno = "Tarde";
                                break;
                            case "N":
                                descricaoTurno = "Noite";
                                break;
                            case "I":
                                descricaoTurno = "Integral";
                                break;
                            case "A":
                                descricaoTurno = "Ampliado";
                                break;
                        }

                        if (!turno.Continuidade & !turno.Novo)
                        {
                            mensagens.Add(string.Format("O turno {0} deve ter marcação como CONTINUIDADE e/ou NOVO!", descricaoTurno));
                        }
                    }
                }
            }

            if (modalidadeSerie.PropostaVagaNova == -1)
            {
                mensagens.Add("O campo PROPOSTA VAGA NOVA é de preenchimento obrigatório.");
            }
            else if (modalidadeSerie.PropostaVagaNova < 0 || modalidadeSerie.PropostaVagaNova > 999)
            {
                mensagens.Add("O campo PROPOSTA VAGA NOVA permite somente valores inteiros de 0 a 999.");
            }

            if (modalidadeSerie.PropostaVagaContinuidade == -1)
            {
                mensagens.Add("O campo PROPOSTA VAGA CONTINUIDADE é de preenchimento obrigatório.");
            }
            else if (modalidadeSerie.PropostaVagaContinuidade < 0 || modalidadeSerie.PropostaVagaContinuidade > 999)
            {
                mensagens.Add("O campo PROPOSTA VAGA CONTINUIDADE permite somente valores inteiros de 0 a 999.");
            }

            if (string.IsNullOrEmpty(modalidadeSerie.UsuarioResponsavel)
                || (!string.IsNullOrEmpty(modalidadeSerie.UsuarioResponsavel) && modalidadeSerie.UsuarioResponsavel.Length > 20))
            {
                mensagens.Add("O campo MATRICULA DO RESPONSÁVEL é obrigatório com o máximo de 20 caracteres!");
            }

            if (mensagens.Count == 0)
            {
                modalidadeSerie.IdAgendaConfTurnoVaga = CtvAgendaConfTurnoVaga.RetornaIdAgenda(
                                                               modalidadeSerie.Ano,
                                                               modalidadeSerie.Periodo,
                                                               modalidadeSerie.Curso,
                                                               modalidadeSerie.Serie);

                if (modalidadeSerie.IdAgendaConfTurnoVaga <= 0)
                {
                    mensagens.Add("Operação não realizada: A AGENDA não existe ou foi encerrada!");
                }
                else
                {
                    var restricao = RN.CtvRestricao.VerificaRestricao(modalidadeSerie.IdAgendaConfTurnoVaga, modalidadeSerie.Censo);

                    if (restricao)
                    {
                        mensagens.Add("Este censo possiu uma restrição para o lançamento para este ano / periodo / curso / serie!");
                    }

                    //Verifica se a escola já possui modalidade / curso / serie para lançamento
                    if (rnCtvConfTurnoInicial.PossuiTurnoParaConfirmacaoPor(modalidadeSerie.IdAgendaConfTurnoVaga, modalidadeSerie.Censo))
                    {
                        mensagens.Add("A escola já possui esta modalidade / curso / serie para lançamento, favor utilizar a tela de Confirmação de Turnos e Vagas para alterar os turnos conforme necessário.");
                    }
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

        public void SalvaInclusaoModalidadeSerie(DTOs.DadosInclusaoModalidadeSerieTurnosVagas modalidadeSerie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            TceCtvConfTurno ctvConfTurno = new TceCtvConfTurno();
            TceCtvPropostaSeeduc proposta = new TceCtvPropostaSeeduc();
            RN.CtvPropostaSeeduc rnProposta = new CtvPropostaSeeduc();

            try
            {
                foreach (DadosTurnoInclusaoModalidadeSerie turno in modalidadeSerie.ListaTurnos)
                {
                    ctvConfTurno = new TceCtvConfTurno();
                    ctvConfTurno.IdAgendaConfTurnoVaga = modalidadeSerie.IdAgendaConfTurnoVaga;
                    ctvConfTurno.Censo = modalidadeSerie.Censo;
                    ctvConfTurno.Matricula = modalidadeSerie.UsuarioResponsavel;
                    ctvConfTurno.DtCadastro = DateTime.Now;
                    ctvConfTurno.Turno = turno.Turno;

                    //Insere todos os Censos/Turnos daquele, curso/serie
                    CtvConfTurnoInicial.Inserir(ctx, ctvConfTurno);

                    if (turno.Novo)
                    {
                        //Insere turno novo
                        ctvConfTurno.Novo = true;
                        ctvConfTurno.Continuidade = false;
                        Inserir(ctx, ctvConfTurno);
                    }
                    if (turno.Continuidade)
                    {
                        //Insere turno continuidade
                        ctvConfTurno.Novo = false;
                        ctvConfTurno.Continuidade = true;
                        Inserir(ctx, ctvConfTurno);
                    }
                }

                //Insere proposta
                proposta.IdAgendaConfTurnoVaga = modalidadeSerie.IdAgendaConfTurnoVaga;
                proposta.Censo = modalidadeSerie.Censo;
                proposta.VagasContinuidade = modalidadeSerie.PropostaVagaContinuidade;
                proposta.VagasNovas = modalidadeSerie.PropostaVagaNova;
                proposta.Matricula = modalidadeSerie.UsuarioResponsavel;
                proposta.TaxaReprovacao = 0;

                rnProposta.Insere(ctx, proposta);
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

        public DataTable ListaTurnosPor(string censo, int ano, int agendaId)
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
                            INNER JOIN DBO.TCE_CTV_CONF_TURNO TI ON AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                    WHERE   AG.ANO = @ANO
                            AND CENSO = @CENSO
                            AND AGENDAID = @AGENDAID");

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@AGENDAID", agendaId);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public ValidacaoDados VerificaRemocaoTurno(int codPerfil, List<DadosConfTurno> dadosConfTurno)
        {
            var mensagens = new List<string>();
            CtvConfTurnoInicial rnCtvConfTurnoInicial = new CtvConfTurnoInicial();
            DataTable dtTurnos = new DataTable();
            RN.Agenda.ParametroTurnoVaga rnParametroTurnoVaga = new Techne.Lyceum.RN.Agenda.ParametroTurnoVaga();
            RN.Agenda.Entidades.ParametroTurnoVaga parametroTurnoVaga = new RN.Agenda.Entidades.ParametroTurnoVaga();
            int qtdeTurnoContinuidade = 0;
            int qtdeTurnoNovo = 0;
            int idAgenda = 0;

            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosConfTurno == null)
            {
                return validacaoDados;
            }

            idAgenda = Convert.ToInt32(dadosConfTurno.Select(x => x.AgendaId).First());

            parametroTurnoVaga = rnParametroTurnoVaga.ObtemPor(codPerfil, idAgenda);

            if (parametroTurnoVaga.ParametroTurnoVagaId > 0)
            {
                if (!parametroTurnoVaga.RemoveTurnoInteiro)
                {
                    dtTurnos = rnCtvConfTurnoInicial.ListaTurnosIniciaisPor(dadosConfTurno.Select(x => x.Censo).First(), dadosConfTurno.Select(x => x.Ano).First(), idAgenda);

                    if (dtTurnos.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtTurnos.Rows)
                        {
                            qtdeTurnoContinuidade = 0;
                            qtdeTurnoNovo = 0;

                            switch (item["TURNO"].ToString())
                            {
                                case "M":
                                    qtdeTurnoContinuidade = dadosConfTurno.Where(x => x.Manha == true).Count();
                                    qtdeTurnoNovo = dadosConfTurno.Where(x => x.ManhaNovo == true).Count();

                                    if ((qtdeTurnoContinuidade + qtdeTurnoNovo) == 0)
                                    {
                                        mensagens.Add("Não é permitido remover completamente o turno MANHÃ da escola para o seu perfil.");

                                    }

                                    break;
                                case "T":
                                    qtdeTurnoContinuidade = dadosConfTurno.Where(x => x.Tarde == true).Count();
                                    qtdeTurnoNovo = dadosConfTurno.Where(x => x.TardeNovo == true).Count();

                                    if ((qtdeTurnoContinuidade + qtdeTurnoNovo) == 0)
                                    {
                                        mensagens.Add("Não é permitido remover completamente o turno TARDE da escola para o seu perfil.");
                                    }
                                    break;
                                case "N":
                                    qtdeTurnoContinuidade = dadosConfTurno.Where(x => x.Noite == true).Count();
                                    qtdeTurnoNovo = dadosConfTurno.Where(x => x.NoiteNovo == true).Count();

                                    if ((qtdeTurnoContinuidade + qtdeTurnoNovo) == 0)
                                    {
                                        mensagens.Add("Não é permitido remover completamente o turno NOITE da escola para o seu perfil.");
                                    }
                                    break;
                                case "A":
                                    qtdeTurnoContinuidade = dadosConfTurno.Where(x => x.Ampliado == true).Count();
                                    qtdeTurnoNovo = dadosConfTurno.Where(x => x.AmpliadoNovo == true).Count();

                                    if ((qtdeTurnoContinuidade + qtdeTurnoNovo) == 0)
                                    {
                                        mensagens.Add("Não é permitido remover completamente o turno AMPLIADO da escola para o seu perfil.");
                                    }
                                    break;
                                case "I":
                                    qtdeTurnoContinuidade = dadosConfTurno.Where(x => x.Integral == true).Count();
                                    qtdeTurnoNovo = dadosConfTurno.Where(x => x.IntegralNovo == true).Count();

                                    if ((qtdeTurnoContinuidade + qtdeTurnoNovo) == 0)
                                    {
                                        mensagens.Add("Não é permitido remover completamente o turno INTEGRAL da escola para o seu perfil.");
                                    }
                                    break;
                            }
                        }
                    }
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

        private static string VerificaRenovacaoAlunoContinuidade(DadosConfTurno dadosConfTurno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var turnosContinuidade = SelecionarTurnosContinuidade(dadosConfTurno);

                var contextQuery = new ContextQuery
                {
                    Command =
                        string.Format(@" SELECT  T.DESCRICAO AS TURNO
                                 FROM    [LYCEUM].[dbo].[RENOVACAO] R
                               INNER JOIN [LYCEUM].[dbo].LY_TURNO t ON R.TURNOID = t.TURNO
                        WHERE   R.UNIDADEENSINOID = @UNIDADEENSINOID
                                AND R.TIPOVAGA = 'VC'
                                AND R.ANO = @ANO
                                AND R.PERIODO = @PERIODO
                                AND R.CURSOID = @CURSOID
                                AND R.SERIE = @SERIE
                           {0} {1}",
                    string.IsNullOrEmpty(turnosContinuidade) ? string.Empty : string.Format("AND R.TURNOID NOT IN ({0})",
                        turnosContinuidade),
                    string.Format(" AND R.SITUACAORENOVACAOID in ( {0},{1})",
                        (int)RN.ConfirmacaoMatricula.SituacaoRenovacao.Ativo,
                        (int)RN.ConfirmacaoMatricula.SituacaoRenovacao.PossuiConfirmacao))
                };

                contextQuery.Parameters.Add("@UNIDADEENSINOID", dadosConfTurno.Censo);
                contextQuery.Parameters.Add("@ANO", dadosConfTurno.Ano);
                contextQuery.Parameters.Add("@PERIODO", dadosConfTurno.Periodo);
                contextQuery.Parameters.Add("@CURSOID", dadosConfTurno.Curso);
                contextQuery.Parameters.Add("@SERIE", dadosConfTurno.Serie);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        return Convert.ToString(reader["TURNO"]);
                    }
                }
            }

            return string.Empty;
        }

        private static string VerificaRenovacaoAlunoNovo(DadosConfTurno dadosConfTurno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var turnosNovo = SelecionarTurnosNovo(dadosConfTurno);

                var contextQuery = new ContextQuery
                {
                    Command =
                        string.Format(@" SELECT  T.DESCRICAO AS TURNO
                               FROM    [LYCEUM].[dbo].[RENOVACAO] R
                               INNER JOIN [LYCEUM].[dbo].LY_TURNO t ON R.TURNOID = t.TURNO
                        WHERE   R.UNIDADEENSINOID = @UNIDADEENSINOID
                                AND R.TIPOVAGA = 'VN'
                                AND R.ANO = @ANO
                                AND R.PERIODO = @PERIODO
                                AND R.CURSOID = @CURSOID
                                AND R.SERIE = @SERIE
                            {0} {1}",
                    string.IsNullOrEmpty(turnosNovo) ? string.Empty : string.Format("AND R.TURNOID NOT IN ({0})",
                        turnosNovo),
                    string.Format(" AND R.SITUACAORENOVACAOID in ( {0},{1})",
                        (int)RN.ConfirmacaoMatricula.SituacaoRenovacao.Ativo,
                        (int)RN.ConfirmacaoMatricula.SituacaoRenovacao.PossuiConfirmacao))
                };

                contextQuery.Parameters.Add("@UNIDADEENSINOID", dadosConfTurno.Censo);
                contextQuery.Parameters.Add("@ANO", dadosConfTurno.Ano);
                contextQuery.Parameters.Add("@PERIODO", dadosConfTurno.Periodo);
                contextQuery.Parameters.Add("@CURSOID", dadosConfTurno.Curso);
                contextQuery.Parameters.Add("@SERIE", dadosConfTurno.Serie);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        return Convert.ToString(reader["TURNO"]);
                    }
                }
            }

            return string.Empty;
        }

        public static bool VerificaTurno(int idAgenda, string censo, string turno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                1
                        FROM    dbo.TCE_CTV_CONF_TURNO
                        WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                                AND CENSO = @CENSO
                                AND TURNO = @TURNO
                                "
                };

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURNO", turno);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ExisteTurnoPor(int idAgendaConfTurnoVaga, string censo, bool novo, bool continuidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                    FROM    DBO.TCE_CTV_CONF_TURNO
                    WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                            AND CENSO = @CENSO
                            AND NOVO = @NOVO
                            AND CONTINUIDADE = @CONTINUIDADE ";

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@NOVO", novo);
                contextQuery.Parameters.Add("@CONTINUIDADE", continuidade);

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
    }
}