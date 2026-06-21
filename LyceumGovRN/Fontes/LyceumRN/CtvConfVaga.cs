using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Text;
using System.Data.SqlClient;
using System.Web;

namespace Techne.Lyceum.RN
{
    public class CtvConfVaga : RNBase
    {
        public List<DadosConfVaga> ListaQuadroDeSalasPor(string censo, int ano, int codPerfil, string descPerfil, int tipoEventoId, List<int> periodosAtivos, List<string> cursosNaoParticipantes)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            List<DadosConfVaga> turmas = new List<DadosConfVaga>();
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder strSql = new StringBuilder();
            string periodos = string.Empty;
            string cursos = null;
            bool turnoFinalizado = false;
            bool adicionar = true;

            try
            {
                if (periodosAtivos.Count() > 0)
                {
                    //Adiciona os periodos da periodosAtivos
                    foreach (int p in periodosAtivos)
                    {
                        //Verifica se aquele periodo já foi adicionado
                        if (!periodos.Contains(p.ToString()))
                        {
                            if (!string.IsNullOrEmpty(periodos))
                            {
                                periodos = periodos + " , ";
                            }

                            periodos = periodos + p.ToString();
                        }
                    }

                    if (cursosNaoParticipantes.Count > 0)
                    {
                        //Adiciona os cursos que não participam do lançamento de turnos e vagas
                        foreach (string c in cursosNaoParticipantes)
                        {
                            //Verifica se aquele periodo já foi adicionado
                            if (!periodos.Contains(c))
                            {
                                if (!string.IsNullOrEmpty(cursos))
                                {
                                    cursos = string.Format("{0}, ", cursos);
                                }

                                cursos = string.Format("{0}'{1}'", cursos, c);
                            }
                        }
                    }

                    strSql.Append(string.Format(@" SELECT DISTINCT
                                    T.ID_CONF_VAGA ,
                                    D.DEPENDENCIA + '[' + CONVERT(VARCHAR, ISNULL(D.NUM_ALUNOS, 0)) + ']' AS SALACAPACIDADE ,
                                    T.ID_AGENDA_CONF_TURNO_VAGA ,
                                    T.AGENDAID ,
                                    T.ENCERRADO ,
                                    T.DT_INICIO_CONF_VAGAS ,
                                    T.DT_FIM_CONF_TURNO ,
                                    T.TURMA ,
                                    T.VAGAS_CONTINUIDADE ,
                                    T.VAGAS_NOVAS ,
                                    T.NUM_ALUNOS ,
                                    T.TURNO ,
                                    T.FINALIZADO ,
                                    T.PERFIL_RESPONSAVEL ,
                                    T.PERIODO
                            FROM    LY_DEPENDENCIA D
                                    LEFT JOIN ( SELECT DISTINCT
                                                        A.ID_AGENDA_CONF_TURNO_VAGA ,
                                                        A.AGENDAID ,
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
                                                                                        AND ATE.TIPOEVENTOID = @TIPOEVENTOID )
                                                             THEN 0
                                                             ELSE 1
                                                        END ENCERRADO ,
                                                        A.DT_INICIO_CONF_VAGAS ,
                                                        A.DT_FIM_CONF_VAGAS ,
                                                        A.DT_FIM_CONF_TURNO ,
                                                        A.PERIODO ,
                                                        V.CENSO ,
                                                        V.VAGAS_CONTINUIDADE ,
                                                        V.VAGAS_NOVAS ,
                                                        NULL AS NUM_ALUNOS ,
                                                        V.TURMA ,
                                                        V.TURNO ,
                                                        V.SALA ,
                                                        V.ID_CONF_VAGA ,
                                                        CASE WHEN A.ENCERRADO = 1 THEN 1
                                                             WHEN ID_FINALIZADO IS NOT NULL THEN 1
                                                             ELSE 0
                                                        END FINALIZADO ,
                                                        PE.DESCRICAO AS PERFIL_RESPONSAVEL
                                                FROM    dbo.TCE_CTV_CONF_VAGA V
                                                        INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                                        INNER JOIN dbo.LY_DEPENDENCIA D2 ON V.SALA = D2.DEPENDENCIA
                                                        INNER JOIN LY_CURSO C ON C.CURSO = A.CURSO
                                                        LEFT JOIN dbo.TCE_CTV_FINALIZADO F ON A.ID_AGENDA_CONF_TURNO_VAGA = F.ID_AGENDA_CONF_TURNO_VAGA
                                                                                          AND V.CENSO = F.CENSO
                                                                                          AND F.VAGA = 1
                                                        LEFT JOIN dbo.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                                                        LEFT JOIN hades.dbo.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                                                WHERE   V.CENSO = @CENSO
                                                        AND A.ANO = @ANO
                                                        AND NOT EXISTS ( SELECT 1 --Restrição de unidade
                                                                         FROM   dbo.TCE_CTV_RESTRICAO RE
                                                                         WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                                                                AND RE.CENSO = V.CENSO )                                                 
                                                        AND ( A.PERIODO IN ({0})) --Lista de periodos ativos                                          
                                              ) T ON D.FACULDADE = T.CENSO
                                                     AND D.DEPENDENCIA = T.SALA
                            WHERE   D.FACULDADE = @CENSO
                                    AND D.ATIVA = 'S'
                                    AND ( D.DEPENDENCIA LIKE 'SL-%'
                                          OR D.DEPENDENCIA LIKE 'SR-%'
                                        ) ",
                        periodos));

                    //Verifica se existe periodo 2 aberto
                    if (periodosAtivos.Contains(2))
                    {
                        //Caso o Periodo aberto seja 2, trazer turmas anuais já existentes                    
                        strSql.Append(string.Format(@" UNION --Pega Turmas reais se do periodo zero se a confirmação for do periodo 2  
                                SELECT DISTINCT
                                        T.ID_CONF_VAGA ,
                                        D.DEPENDENCIA + '[' + CONVERT(VARCHAR, ISNULL(D.NUM_ALUNOS, 0)) + ']' AS SALACAPACIDADE ,
                                        T.ID_AGENDA_CONF_TURNO_VAGA ,
                                        T.AGENDAID ,
                                        T.ENCERRADO ,
                                        T.DT_INICIO_CONF_VAGAS ,
                                        T.DT_FIM_CONF_TURNO ,
                                        T.TURMA ,
                                        T.VAGAS_CONTINUIDADE ,
                                        T.VAGAS_NOVAS ,
                                        T.NUM_ALUNOS ,
                                        T.TURNO ,
                                        T.FINALIZADO ,
                                        T.PERFIL_RESPONSAVEL ,
                                        T.PERIODO
                                FROM    LY_DEPENDENCIA D
                                        LEFT JOIN ( SELECT DISTINCT
                                                            0 AS ID_AGENDA_CONF_TURNO_VAGA ,
                                                            0 AS AGENDAID ,
                                                            1 AS ENCERRADO ,
                                                            NULL AS DT_INICIO_CONF_VAGAS ,
                                                            NULL AS DT_FIM_CONF_VAGAS ,
                                                            NULL AS DT_FIM_CONF_TURNO ,
                                                            V.SEMESTRE AS PERIODO ,
                                                            V.FACULDADE AS CENSO ,
                                                            NULL AS VAGAS_CONTINUIDADE ,
                                                            NULL AS VAGAS_NOVAS ,
                                                            V.NUM_ALUNOS ,
                                                            V.TURMA ,
                                                            V.TURNO ,
                                                            V.DEPENDENCIA AS SALA ,
                                                            0 AS ID_CONF_VAGA ,
                                                            1 AS FINALIZADO ,
                                                            PE.DESCRICAO AS PERFIL_RESPONSAVEL
                                                    FROM    LY_TURMA V
                                                            INNER JOIN dbo.LY_DEPENDENCIA D2 ON V.DEPENDENCIA = D2.DEPENDENCIA
                                                            INNER JOIN LY_CURSO C ON C.CURSO = V.CURSO
                                                            LEFT JOIN dbo.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                                                            LEFT JOIN hades.dbo.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                                                    WHERE   V.FACULDADE = @CENSO
                                                            AND V.ANO = @ANO
                                                            AND V.SEMESTRE = 0
                                                            AND V.SIT_TURMA <> 'Desativada'
                                                            AND V.CURSO NOT IN ( {0} ) --cursos que nao participam do Turnos e Vagas
                                                            AND V.OPTATIVAREFORCO = 'N' 
                                                            AND ISNULL(V.ELETIVA,'N') = 'N'
                                                  ) T ON D.FACULDADE = T.CENSO
                                                         AND D.DEPENDENCIA = T.SALA
                                WHERE   D.FACULDADE = @CENSO
                                        AND D.ATIVA = 'S'
                                        AND ( D.DEPENDENCIA LIKE 'SL-%'
                                              OR D.DEPENDENCIA LIKE 'SR-%'
                                            ) ",
                                                                   cursos ?? "''"));
                    }

                    strSql.Append(@" ORDER BY SALACAPACIDADE ,
                                 T.TURMA DESC  ");

                    contextQuery.Command = strSql.ToString();

                    contextQuery.Parameters.Add("@PERFIL_LOGADO", codPerfil);
                    contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);
                    contextQuery.Parameters.Add("@CENSO", censo);
                    contextQuery.Parameters.Add("@ANO", ano);

                    reader = ctx.GetDataReader(contextQuery);

                    while (reader.Read())
                    {
                        adicionar = true;

                        DadosConfVaga dadosConfVaga = new DadosConfVaga
                        {
                            Periodo = !String.IsNullOrEmpty(reader["PERIODO"].ToString()) ? Convert.ToInt32(reader["PERIODO"]) : -1,
                            IdAgendaConfTurnoVaga = !String.IsNullOrEmpty(reader["ID_AGENDA_CONF_TURNO_VAGA"].ToString()) ? Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]) : default(int),
                            IdCtvConfVaga = !String.IsNullOrEmpty(reader["ID_CONF_VAGA"].ToString()) ? Convert.ToInt32(reader["ID_CONF_VAGA"]) : default(int),
                            SalaCapacidade = Convert.ToString(reader["salacapacidade"]),
                            Turma = Convert.ToString(reader["TURMA"]),
                            VagasContinuidade = !String.IsNullOrEmpty(reader["VAGAS_CONTINUIDADE"].ToString()) ? Convert.ToInt32(reader["VAGAS_CONTINUIDADE"]) : default(int),
                            VagasNova = !String.IsNullOrEmpty(reader["VAGAS_NOVAS"].ToString()) ? Convert.ToInt32(reader["VAGAS_NOVAS"]) : default(int),
                            NumAlunos = !String.IsNullOrEmpty(reader["NUM_ALUNOS"].ToString()) ? Convert.ToInt32(reader["NUM_ALUNOS"]) : default(int),
                            Turno = Convert.ToString(reader["TURNO"]),
                            Editavel = false,
                            PerfilResponsavel = Convert.ToString(reader["PERFIL_RESPONSAVEL"]),
                            DtInicioConfVaga = !String.IsNullOrEmpty(reader["DT_INICIO_CONF_VAGAS"].ToString()) ? Convert.ToDateTime(reader["DT_INICIO_CONF_VAGAS"]) : default(DateTime),
                            DtFimConfTurno = !String.IsNullOrEmpty(reader["DT_FIM_CONF_TURNO"].ToString()) ? Convert.ToDateTime(reader["DT_FIM_CONF_TURNO"]) : default(DateTime),
                            Finalizado = !String.IsNullOrEmpty(reader["FINALIZADO"].ToString()) ? Convert.ToBoolean(reader["FINALIZADO"]) : default(bool),
                            AgendaEncerrada = !String.IsNullOrEmpty(reader["ENCERRADO"].ToString()) ? Convert.ToBoolean(reader["ENCERRADO"]) : true
                        };

                        //Caso a AGENDA_CONF_TURNO_VAGA já esteja encerrado ou fora do periodo para o perfil (tratamento na query)
                        if (!dadosConfVaga.AgendaEncerrada)
                        {
                            //Verifica se o turno ja foi finalizado
                            turnoFinalizado = CtvFinalizado.VerificaTurnoFinalizada(ano, dadosConfVaga.Periodo, censo, dadosConfVaga.IdAgendaConfTurnoVaga);

                            //Verifica se o turno nao foi finalizado e o periodo de lançamento de turno não terminou
                            if (turnoFinalizado || dadosConfVaga.DtFimConfTurno < DateTime.Now.Date)
                            {
                                dadosConfVaga.Editavel = true;
                            }

                            if (descPerfil == "SUPED" || descPerfil == "SUPLAN")
                            {
                                if (descPerfil != dadosConfVaga.PerfilResponsavel)
                                {
                                    dadosConfVaga.Editavel = false;
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(dadosConfVaga.Turno))
                        {
                            if (turmas.Where(x => x.SalaCapacidade == dadosConfVaga.SalaCapacidade && x.Turno != string.Empty).Count() > 0)
                            {
                                adicionar = false;
                            }
                        }

                        if (adicionar)
                        {
                            turmas.Add(dadosConfVaga);
                        }
                    }
                }

                return turmas;
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

        public List<DadosTurmaConfVaga> ListaTurmasParaLancamentoPor(string censo, int ano, List<int> periodosAtivos, List<string> cursosNaoParticipantes)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            List<DadosTurmaConfVaga> turmas = new List<DadosTurmaConfVaga>();
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder strSql = new StringBuilder();
            string periodos = string.Empty;
            string cursos = null;

            try
            {
                if (periodosAtivos.Count() > 0)
                {
                    //Adiciona os periodos da periodosAtivos
                    foreach (int p in periodosAtivos)
                    {
                        //Verifica se aquele periodo já foi adicionado
                        if (!periodos.Contains(p.ToString()))
                        {
                            if (!string.IsNullOrEmpty(periodos))
                            {
                                periodos = periodos + " , ";
                            }

                            periodos = periodos + p.ToString();
                        }
                    }

                    if (cursosNaoParticipantes.Count > 0)
                    {
                        //Adiciona os cursos que não participam do lançamento de turnos e vagas
                        cursos = "'";
                        cursos += cursosNaoParticipantes.Aggregate((x, y) => x + "', '" + y);//.Aggregate((x, y) => "'" + x + " , " + y + "'");
                        cursos += "'";
                    }

                    strSql.Append(string.Format(@"  SELECT DISTINCT
                                CASE WHEN ( PERIODO = 0 ) THEN TURMA + ' [Anual]'
                                     ELSE TURMA + ' [' + CAST(PERIODO AS VARCHAR) + 'º Semestre]'
                                END DESCRICAOTURMA ,
                                TURMA ,
                                PERIODO
                         FROM   ( SELECT DISTINCT
                                            ( T.TURMA ) AS TURMA ,
                                            AG.PERIODO
                                  FROM      LY_TURMA T
                                            INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA AG ON T.ANO = AG.ANO_REFERENCIA
                                                                                      AND T.SEMESTRE = AG.PERIODO_REFERENCIA
                                                                                      AND T.SERIE = AG.SERIE
                                                                                      AND T.CURSO = AG.CURSO
                                  WHERE     T.FACULDADE = @CENSO
                                            AND T.SIT_TURMA <> 'Desativada'
                                            AND AG.ANO = @ANO
                                            AND ( AG.PERIODO IN ( {0} ) ) --LISTA DE PERIODOS 
                                            AND OPTATIVAREFORCO = 'N'
                                            AND ISNULL(T.ELETIVA,'N') = 'N'
                                            AND T.DEPENDENCIA IS NOT NULL
                                            AND NOT EXISTS ( SELECT 1
                                                             FROM   dbo.TCE_CTV_RESTRICAO RE
                                                             WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                                                    AND RE.CENSO = T.FACULDADE )
                                  UNION
                                  SELECT DISTINCT
                                            TURMA ,
                                            t.PERIODO
                                  FROM      dbo.TCE_CTV_TURMA_PROVISORIA T
                                            INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA AG ON T.ANO = AG.ANO
                                                                                      AND T.PERIODO = AG.PERIODO
                                                                                      AND T.SERIE = AG.SERIE
                                                                                      AND T.CURSO = AG.CURSO
                                  WHERE     T.CENSO = @CENSO
                                            AND T.ANO = @ANO
                                            AND ( AG.PERIODO IN ( {0} ) )   --LISTA DE PERIODOS 
                                            AND NOT EXISTS ( SELECT 1
                                                             FROM   dbo.TCE_CTV_RESTRICAO RE
                                                             WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                                                    AND RE.CENSO = T.CENSO ) ",
                        periodos));

                    //Verifica se existe periodo 2 aberto
                    if (periodosAtivos.Contains(2))
                    {
                        //Caso o Periodo aberto seja 2, trazer turmas anuais já existentes                    
                        strSql.Append(string.Format(@" UNION
                              SELECT DISTINCT
                                        ( T.TURMA ) AS TURMA ,
                                        T.SEMESTRE AS PERIODO
                              FROM      LY_TURMA T
                              WHERE     T.FACULDADE = @CENSO
                                        AND T.SIT_TURMA = 'Aberta'
                                        AND T.ANO = @ANO
                                        AND T.SEMESTRE = 0
                                        AND T.DEPENDENCIA IS NOT NULL
                                        AND T.CURSO NOT IN ( {0} ) --cursos que nao participam do Turnos e Vagas
                                        AND OPTATIVAREFORCO = 'N'
                                        AND ISNULL(T.ELETIVA,'N') = 'N' ",
                                                                   cursos ?? "''"));
                    }

                    strSql.Append(@" ) tabela ");

                    contextQuery.Command = strSql.ToString();

                    contextQuery.Parameters.Add("@CENSO", censo);
                    contextQuery.Parameters.Add("@ANO", ano);

                    reader = ctx.GetDataReader(contextQuery);

                    while (reader.Read())
                    {
                        DadosTurmaConfVaga turma = new DadosTurmaConfVaga
                        {
                            Turma = Convert.ToString(reader["TURMA"]),
                            DescricaoTurma = Convert.ToString(reader["DESCRICAOTURMA"]),
                            Periodo = Convert.ToInt32(reader["PERIODO"])
                        };

                        turmas.Add(turma);
                    }
                }
                return turmas;
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

        public static DataTable ListarQuadroPorAnoPeriodoCenso(int ano, int periodo, string censo, string perfil)
        {
            var dt = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  AG.ANO ,
                            AG.PERIODO ,
                            AG.SERIE ,
                            PR.CENSO ,
                            AG.ID_AGENDA_CONF_TURNO_VAGA ,
                            AG.ANO_REFERENCIA ,
                            AG.PERIODO_REFERENCIA ,
                            MC.MODALIDADE ,
                            MC.DESCRICAO AS DESCRICAO_MODALIDADE ,
                            AG.CURSO ,
                            C.NOME AS NOME_CURSO ,
                            AG.DT_FIM_CONF_TURNO ,
                            AG.DT_INICIO_CONF_VAGAS ,
                            AG.DT_FIM_CONF_VAGAS ,
                            AG.ENCERRADO ,
                            J.JUSTIFICATIVA_CONTINUIDADE ,
                            J.JUSTIFICATIVA_NOVO ,
                            ( SELECT    SUM(VG.VAGAS_CONTINUIDADE)
                                  FROM      DBO.TCE_CTV_CONF_VAGA VG
                                  WHERE     vg.CENSO = PR.CENSO
                                            AND vg.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                  GROUP BY  vg.ID_AGENDA_CONF_TURNO_VAGA ,
                                            vg.CENSO
                                ) AS VAGAS_CONTINUIDADE ,
                            ( SELECT    SUM(VG.VAGAS_NOVAS)
                                  FROM      DBO.TCE_CTV_CONF_VAGA VG
                                  WHERE     vg.CENSO = PR.CENSO
                                            AND vg.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                  GROUP BY  vg.ID_AGENDA_CONF_TURNO_VAGA ,
                                            vg.CENSO
                                ) AS VAGAS_NOVAS ,
                            J.ID_JUSTIFICATIVA ,
                            DBO.f_CtvVagasUtilizadasContinuidade(pr.CENSO, ag.CURSO, ag.SERIE,
                                                                 ag.ano, ag.PERIODO) AS VAGAS_UTILIZADAS_CONT ,
                            DBO.f_CtvVagasUtilizadasNovas(pr.CENSO, ag.CURSO, ag.SERIE, ag.ano,
                                                          ag.PERIODO) AS VAGAS_UTILIZADAS_NOVA ,
                            CASE WHEN ( AG.DT_FIM_CONF_VAGAS < CONVERT(DATE, GETDATE())
                                        AND ID_FINALIZADO IS NULL
                                      ) THEN 1
                                 WHEN ID_FINALIZADO IS NOT NULL THEN 1
                                 ELSE 0
                            END FINALIZADO ,
                            PE.DESCRICAO AS PERFIL_RESPONSAVEL ,
                            PR.VAGAS_CONTINUIDADE AS PROPOSTA_SEEDUC_CONT ,
                            PR.VAGAS_NOVAS AS PROPOSTA_SEEDUC_NOVA
                    INTO    #VAGAS
                    FROM    TCE_CTV_PROPOSTA_SEEDUC PR
                            INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG ON PR.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                            INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL TI ON PR.CENSO = TI.CENSO
                                                                            AND AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                            INNER JOIN LY_CURSO C ON AG.CURSO = C.CURSO
                            INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                            INNER JOIN LY_TIPO_CURSO TC ON TC.TIPO = C.TIPO
                            LEFT JOIN TCE_CTV_JUSTIFICATIVA J ON J.ID_AGENDA_CONF_TURNO_VAGA = PR.ID_AGENDA_CONF_TURNO_VAGA
                                                                 AND J.CENSO = PR.CENSO
                                                                 AND J.VAGA = 1
                            LEFT JOIN DBO.TCE_CTV_FINALIZADO F ON F.ID_AGENDA_CONF_TURNO_VAGA = PR.ID_AGENDA_CONF_TURNO_VAGA
                                                                  AND F.CENSO = PR.CENSO
                                                                  AND F.VAGA = 1
                            LEFT JOIN DBO.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                            LEFT JOIN HADES.DBO.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL                           
                    WHERE   PR.CENSO = @CENSO
                            AND AG.ANO = @ANO
                            AND AG.PERIODO = @PERIODO
                            AND NOT EXISTS ( SELECT 1
                                             FROM   DBO.TCE_CTV_RESTRICAO RE
                                             WHERE  re.ID_AGENDA_CONF_TURNO_VAGA = ag.ID_AGENDA_CONF_TURNO_VAGA
                                                    AND re.CENSO = PR.CENSO )
                    GROUP BY AG.ANO ,
                            AG.PERIODO ,
                            AG.SERIE ,
                            PR.CENSO ,
                            AG.ID_AGENDA_CONF_TURNO_VAGA ,
                            AG.ANO_REFERENCIA ,
                            AG.PERIODO_REFERENCIA ,
                            MC.MODALIDADE ,
                            MC.DESCRICAO ,
                            AG.CURSO ,
                            C.NOME ,
                            AG.DT_FIM_CONF_TURNO ,
                            AG.DT_INICIO_CONF_VAGAS ,
                            AG.DT_FIM_CONF_VAGAS ,
                            AG.ENCERRADO ,
                            J.JUSTIFICATIVA_CONTINUIDADE ,
                            J.JUSTIFICATIVA_NOVO ,
                            J.VAGAS_CONTINUIDADE ,
                            J.VAGAS_NOVAS ,
                            J.ID_JUSTIFICATIVA ,
                            ID_FINALIZADO ,
                            PE.DESCRICAO,
                            PR.VAGAS_CONTINUIDADE  ,
                            PR.VAGAS_NOVAS     
                                                
                    SELECT DISTINCT
                            t.* ,
                            ( SELECT TOP 1
                                        ls.DESCRICAO
                              FROM      ly_serie ls
                                        JOIN LY_CURSO c ON ls.CURSO = c.CURSO
                                        JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO
                                                                AND LC.CURSO = LS.CURSO
                              WHERE     t.SERIE = ls.SERIE
                                        AND t.CURSO = ls.CURSO
                                        AND lc.ANO_INI = t.ANO_REFERENCIA
                                        AND lc.SEM_INI = t.PERIODO_REFERENCIA
                            ) AS DESCRICAO_SERIE
                    FROM    #VAGAS t
                    ORDER BY t.MODALIDADE ,
                            NOME_CURSO ,
                            SERIE        
                    DROP TABLE  #VAGAS "
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);

                dt = ctx.GetDataTable(contextQuery);
            }

            dt.Columns.Add("EDITAVEL", typeof(bool));
            dt.Columns.Add("ANALISE_LIBERADA", typeof(bool));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var editavel = true;
                var analiseLiberada = true;
                var idAgenda = !String.IsNullOrEmpty(dt.Rows[i]["ID_AGENDA_CONF_TURNO_VAGA"].ToString())
                                     ? Convert.ToInt32(dt.Rows[i]["ID_AGENDA_CONF_TURNO_VAGA"])
                                     : default(int);
                var perfilResponsavel = Convert.ToString(dt.Rows[i]["PERFIL_RESPONSAVEL"]);
                var finalizado = !String.IsNullOrEmpty(dt.Rows[i]["FINALIZADO"].ToString())
                                     ? Convert.ToBoolean(dt.Rows[i]["FINALIZADO"])
                                     : default(bool);
                var encerrado = !String.IsNullOrEmpty(dt.Rows[i]["ENCERRADO"].ToString())
                                    ? Convert.ToBoolean(dt.Rows[i]["ENCERRADO"])
                                    : default(bool);
                var dataInicioVaga = !String.IsNullOrEmpty(dt.Rows[i]["DT_INICIO_CONF_VAGAS"].ToString())
                                     ? Convert.ToDateTime(dt.Rows[i]["DT_INICIO_CONF_VAGAS"])
                                     : default(DateTime);
                var dataFimTurno = !String.IsNullOrEmpty(dt.Rows[i]["DT_FIM_CONF_TURNO"].ToString())
                                     ? Convert.ToDateTime(dt.Rows[i]["DT_FIM_CONF_TURNO"])
                                     : default(DateTime);

                if (!string.IsNullOrEmpty(perfilResponsavel))
                {
                    var turnoFinalizado = CtvFinalizado.VerificaTurnoFinalizada(ano, periodo, censo, idAgenda);

                    if (!turnoFinalizado && dataFimTurno > DateTime.Now.Date)
                    {
                        editavel = false;
                    }

                    if (perfil == "SUPLAN" || perfil == "SUPED" || perfil == "DIESP")
                    {
                        analiseLiberada = !encerrado && perfilResponsavel == perfil &&
                                                finalizado;
                        editavel = !encerrado &&
                            perfilResponsavel == perfil &&
                            dataInicioVaga <= DateTime.Now.Date;
                    }
                    else if (perfil == "DIRETOR_UE")
                    {
                        analiseLiberada = false;
                        editavel = !finalizado && dataInicioVaga <= DateTime.Now.Date;
                    }
                }
                if (encerrado)
                {
                    editavel = false;
                }

                dt.Rows[i]["EDITAVEL"] = editavel;
                dt.Rows[i]["ANALISE_LIBERADA"] = analiseLiberada;
            }

            return dt;
        }

        public DataTable ListaQuadroPropostaPor(int ano, string censo, int codPerfil, string descPerfil, int tipoEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();
            DateTime dataFimTurno = default(DateTime);
            bool editavel = false;
            bool turnoFinalizado = false;
            bool encerrado = false;
            int periodo = -1;
            int idAgenda = -1;
            string perfilResponsavel = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT  AG.ANO ,
                        AG.PERIODO ,
                        ag.AGENDAID ,
                        ISNULL(SE.ENTRADA, 0) AS ENTRADA ,
                        AG.SERIE ,
                        PR.CENSO ,
                        AG.ID_AGENDA_CONF_TURNO_VAGA ,
                        AG.ANO_REFERENCIA ,
                        AG.PERIODO_REFERENCIA ,
                        MC.MODALIDADE ,
                        MC.DESCRICAO AS DESCRICAO_MODALIDADE ,
                        AG.CURSO ,
                        C.NOME AS NOME_CURSO ,
                        AG.DT_FIM_CONF_TURNO ,
                        AG.DT_INICIO_CONF_VAGAS ,
                        AG.DT_FIM_CONF_VAGAS ,
                        J.JUSTIFICATIVA_NOVO ,
                        ( SELECT    SUM(VG.VAGAS_CONTINUIDADE)
                          FROM      dbo.TCE_CTV_CONF_VAGA VG
                          WHERE     vg.CENSO = PR.CENSO
                                    AND vg.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                          GROUP BY  vg.ID_AGENDA_CONF_TURNO_VAGA ,
                                    vg.CENSO
                        ) AS VAGAS_CONTINUIDADE ,
                        ( SELECT    SUM(VG.VAGAS_NOVAS)
                          FROM      dbo.TCE_CTV_CONF_VAGA VG
                          WHERE     vg.CENSO = PR.CENSO
                                    AND vg.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                          GROUP BY  vg.ID_AGENDA_CONF_TURNO_VAGA ,
                                    vg.CENSO
                        ) AS VAGAS_NOVAS ,
                        J.ID_JUSTIFICATIVA ,
                        dbo.F_ctvvagasutilizadascontinuidade(pr.CENSO, ag.CURSO, ag.SERIE,
                                                             ag.ANO, ag.PERIODO) AS VAGAS_UTILIZADAS_CONT ,
                        dbo.F_ctvvagasutilizadasnovas(pr.CENSO, ag.CURSO, ag.SERIE, ag.ANO,
                                                      ag.PERIODO) AS VAGAS_UTILIZADAS_NOVA ,
                        CASE WHEN Ag.ENCERRADO = 1 THEN 1
                             WHEN ID_FINALIZADO IS NOT NULL THEN 1
                             ELSE 0
                        END FINALIZADO ,
                        CASE WHEN Ag.ENCERRADO = 1 THEN 1
                             WHEN @PERFIL_LOGADO = 0 THEN Ag.ENCERRADO
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
                                                        AND AA.AGENDAID = ag.AGENDAID
                                                        AND ATE.TIPOEVENTOID = @TIPOEVENTOID )
                             THEN 0
                             ELSE 1
                        END ENCERRADO ,
                        PE.DESCRICAO AS PERFIL_RESPONSAVEL ,
                        PR.VAGAS_CONTINUIDADE AS PROPOSTA_SEEDUC_CONT ,
                        PR.VAGAS_NOVAS AS PROPOSTA_SEEDUC_NOVA ,
                        CONVERT(VARCHAR, PR.TAXAREPROVACAO) + '%' AS TAXAREPROVACAO ,
                        CONVERT(VARCHAR, ( 100 - PR.TAXAREPROVACAO )) + '%' AS TAXAAPROVACAO
                INTO    #vagas
                FROM    TCE_CTV_PROPOSTA_SEEDUC PR
                        INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG ON PR.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                        INNER JOIN dbo.TCE_CTV_CONF_TURNO_INICIAL TI ON PR.CENSO = TI.CENSO
                                                                        AND AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                        INNER JOIN LY_CURSO C ON AG.CURSO = C.CURSO
                        INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                        INNER JOIN LY_TIPO_CURSO TC ON TC.TIPO = C.TIPO
                        LEFT JOIN TCE_CTV_JUSTIFICATIVA J ON J.ID_AGENDA_CONF_TURNO_VAGA = PR.ID_AGENDA_CONF_TURNO_VAGA
                                                             AND J.CENSO = PR.CENSO
                                                             AND J.VAGA = 1
                        LEFT JOIN dbo.TCE_CTV_FINALIZADO F ON F.ID_AGENDA_CONF_TURNO_VAGA = PR.ID_AGENDA_CONF_TURNO_VAGA
                                                              AND F.CENSO = PR.CENSO
                                                              AND F.VAGA = 1
                        LEFT JOIN dbo.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                        LEFT JOIN hades.dbo.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                        LEFT JOIN TurnosVagas.SERIEENTRADA SE ON AG.CURSO = SE.CURSOID
                                                              AND AG.SERIE = SE.SERIE
                                                              AND SE.ENTRADA = 1
                WHERE   PR.CENSO = @CENSO
                        AND AG.ANO = @ANO
                        AND NOT EXISTS ( SELECT 1
                                         FROM   dbo.TCE_CTV_RESTRICAO RE
                                         WHERE  re.ID_AGENDA_CONF_TURNO_VAGA = ag.ID_AGENDA_CONF_TURNO_VAGA
                                                AND re.CENSO = PR.CENSO )
                GROUP BY AG.ANO ,
                        AG.PERIODO ,
                        ag.AGENDAID ,
                        SE.ENTRADA ,
                        AG.SERIE ,
                        PR.CENSO ,
                        AG.ID_AGENDA_CONF_TURNO_VAGA ,
                        AG.ANO_REFERENCIA ,
                        AG.PERIODO_REFERENCIA ,
                        MC.MODALIDADE ,
                        MC.DESCRICAO ,
                        AG.CURSO ,
                        C.NOME ,
                        AG.DT_FIM_CONF_TURNO ,
                        AG.DT_INICIO_CONF_VAGAS ,
                        AG.DT_FIM_CONF_VAGAS ,
                        AG.ENCERRADO ,
                        J.JUSTIFICATIVA_CONTINUIDADE ,
                        J.JUSTIFICATIVA_NOVO ,
                        J.VAGAS_CONTINUIDADE ,
                        J.VAGAS_NOVAS ,
                        J.ID_JUSTIFICATIVA ,
                        ID_FINALIZADO ,
                        PE.DESCRICAO ,
                        PR.VAGAS_CONTINUIDADE ,
                        PR.VAGAS_NOVAS, 
                        PR.TAXAREPROVACAO

                SELECT DISTINCT
                        t.* ,
                        ( SELECT TOP 1
                                    ls.DESCRICAO
                          FROM      LY_SERIE ls
                                    JOIN LY_CURSO c ON ls.CURSO = c.CURSO
                                    JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO
                                                            AND LC.CURSO = LS.CURSO
                          WHERE     t.SERIE = ls.SERIE
                                    AND t.CURSO = ls.CURSO
                                    AND lc.ANO_INI = t.ANO_REFERENCIA
                                    AND lc.SEM_INI = t.PERIODO_REFERENCIA
                        ) AS DESCRICAO_SERIE
                FROM    #vagas t
                ORDER BY T.PERIODO, 
                        t.MODALIDADE ,
                        NOME_CURSO ,
                        SERIE 

                DROP TABLE #vagas ";

                contextQuery.Parameters.Add("@PERFIL_LOGADO", codPerfil);
                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);

                dt = ctx.GetDataTable(contextQuery);

                dt.Columns.Add("EDITAVEL", typeof(bool));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    editavel = true;
                    idAgenda = !String.IsNullOrEmpty(dt.Rows[i]["ID_AGENDA_CONF_TURNO_VAGA"].ToString())
                                         ? Convert.ToInt32(dt.Rows[i]["ID_AGENDA_CONF_TURNO_VAGA"])
                                         : -1;
                    perfilResponsavel = Convert.ToString(dt.Rows[i]["PERFIL_RESPONSAVEL"].ToString());
                    periodo = !String.IsNullOrEmpty(dt.Rows[i]["PERIODO"].ToString())
                                         ? Convert.ToInt32(dt.Rows[i]["PERIODO"])
                                         : -1;
                    encerrado = !String.IsNullOrEmpty(dt.Rows[i]["ENCERRADO"].ToString())
                                        ? Convert.ToBoolean(dt.Rows[i]["ENCERRADO"])
                                        : false;

                    //Caso a AGENDA_CONF_TURNO_VAGA já esteja encerrado ou fora do periodo para o perfil (tratamento na query)
                    if (encerrado)
                    {
                        editavel = false;
                    }
                    else
                    {
                        if (descPerfil == "SUPED" || descPerfil == "SUPLAN")
                        {
                            if (descPerfil != perfilResponsavel)
                            {
                                editavel = false;
                            }
                        }

                        //Verifica se o turno ja foi finalizado
                        turnoFinalizado = CtvFinalizado.VerificaTurnoFinalizada(ano, periodo, censo, idAgenda);

                        //Verifica se o turno nao foi finalizado e o periodo de lançamento de turno não terminou
                        if (!turnoFinalizado && dataFimTurno > DateTime.Now.Date)
                        {
                            editavel = false;
                        }
                    }

                    dt.Rows[i]["EDITAVEL"] = editavel;
                }
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

        private ValidacaoDados ValidaParcialmente(DadosConfVagaEncadeado ctvConfVaga, string Descricaoperfil, int codPerfil, int tipoEventoId, List<string> turmas)
        {
            List<string> mensagens = new List<string>();
            RN.CapacidadeAlunoTurmaMunicipio capacidadeAlunoTurmaMunicipio = new RN.CapacidadeAlunoTurmaMunicipio();
            int somaVagas = 0;
            string nomeTurno = string.Empty;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ctvConfVaga == null)
            {
                return validacaoDados;
            }

            //campos obrigatorios          

            if (ctvConfVaga.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (ctvConfVaga.Periodo < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (string.IsNullOrEmpty(Descricaoperfil))
            {
                mensagens.Add("O campo PERFIL é obrigatório!");
            }

            if (codPerfil < 0)
            {
                mensagens.Add("O campo CODIGO PERFIL é obrigatório!");
            }

            if (ctvConfVaga.IdAgendaConfTurnoVaga < 0)
            {
                mensagens.Add("O campo IDAGENDACONFTURNOVAGA é obrigatório!");
            }

            if (tipoEventoId <= 0)
            {
                mensagens.Add("O campo TIPO EVENTO é obrigatório!");
            }

            if (string.IsNullOrEmpty(ctvConfVaga.Turno))
            {
                mensagens.Add("O campo TURNO é obrigatório!");
            }
            else
            {
                switch (ctvConfVaga.Turno)
                {
                    case "M":
                        nomeTurno = "Manhã";
                        break;
                    case "N":
                        nomeTurno = "Noite";
                        break;
                    case "T":
                        nomeTurno = "Tarde";
                        break;
                    case "I":
                        nomeTurno = "Integral";
                        break;
                    case "A":
                        nomeTurno = "Ampliado";
                        break;
                }
            }

            if (string.IsNullOrEmpty(ctvConfVaga.Sala))
            {
                mensagens.Add("O campo SALA é obrigatório!");
            }

            if (string.IsNullOrEmpty(ctvConfVaga.Censo))
            {
                mensagens.Add("O campo CENSO é obrigatório!");
            }

            if (string.IsNullOrEmpty(ctvConfVaga.Turma))
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }
            else
            {
                //Verificar se a turma já esta na lista de turmas lançadadas
                if (turmas.Contains(ctvConfVaga.Turma))
                {
                    mensagens.Add(string.Format("ERRO: A turma {0} se encontra alocada em mais de uma sala.", ctvConfVaga.Turma));
                }
                else
                {
                    //Coloca a turma na lista de turmas lançadadas
                    turmas.Add(ctvConfVaga.Turma);
                }
            }

            if (string.IsNullOrEmpty(ctvConfVaga.Matricula)
                || (!string.IsNullOrEmpty(ctvConfVaga.Matricula)
                    && ctvConfVaga.Matricula.Length > 12))
            {
                mensagens.Add("O campo MATRICULA é obrigatório com o máximo de 12 caracteres!");
            }

            if (ctvConfVaga.SalaCapacidade <= 0)
            {
                mensagens.Add(string.Format("A sala {0} está com a capacidade zerada. Verifique o cadastro de dependências.",
                    ctvConfVaga.Sala));
            }

            //Carregar dados da turma (idAgenda, curso, curriculo, serie)
            var dadosTurma = CtvAgendaConfTurnoVaga.VerificaDadosTurma(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Censo,
                                                                       ctvConfVaga.Turma, ctvConfVaga.Turno);
            if (dadosTurma == null || dadosTurma.IdAgenda <= 0)
            {
                //Se não encontrar turma no ano/periodo referencia, ver turma provisoria
                dadosTurma = CtvAgendaConfTurnoVaga.VerificaDadosTurmaProvisoria(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Censo,
                                                                                 ctvConfVaga.Turma, ctvConfVaga.Turno);
            }

            var encerrado = false;

            if (string.IsNullOrEmpty(dadosTurma.Curriculo))
            {
                mensagens.Add(string.Format("Não foi encontrada matriz curricular cadastrada para o ano: {0}, periodo: {1}, curso: {2}, serie: {3}, turno: {4}!",
                    ctvConfVaga.Ano, ctvConfVaga.Periodo, dadosTurma.NomeCurso, dadosTurma.Serie, ctvConfVaga.Turno));
            }
            else
            {
                //Carregar campos necessarios
                ctvConfVaga.IdAgendaConfTurnoVaga = dadosTurma.IdAgenda;
                ctvConfVaga.Curso = dadosTurma.Curso;
                ctvConfVaga.Curriculo = dadosTurma.Curriculo;
                ctvConfVaga.Serie = dadosTurma.Serie;
            }

            if (mensagens.Count == 0 && ctvConfVaga.IdAgendaConfTurnoVaga == 0)
            {
                mensagens.Add(string.Format("Não existe agenda para este ano / periodo: {0} / curso: {1} / série: {2} ou ela já foi encerrada.",
                        ctvConfVaga.Periodo.ToString(),
                        dadosTurma.NomeCurso,
                        ctvConfVaga.Serie));
            }
            else
            {
                //Carregar dados da agenda: ano e periodo referencia
                var agenda = RN.CtvAgendaConfTurnoVaga.Carregar(ctvConfVaga.IdAgendaConfTurnoVaga);

                if (agenda.Encerrado)
                {
                    mensagens.Add(string.Format("O lançamento de vagas para este ano / periodo / curso: {0} / série: {1} já foi encerrado.",
                         dadosTurma.NomeCurso,
                         ctvConfVaga.Serie));
                    encerrado = true;
                }
            }

            if (ctvConfVaga.IdAgendaConfTurnoVaga > 0 && !encerrado)
            {
                //Verifica se existe proposta SEEDUC para o censo / ano / periodo / curso / serie
                if (!CtvPropostaSeeduc.VerificaPropostaSeeducPorAgenda(ctvConfVaga.IdAgendaConfTurnoVaga, ctvConfVaga.Censo))
                {
                    mensagens.Add(string.Format("Não existe proposta Seeduc para o curso: {0} série: {1}.",
                        dadosTurma.NomeCurso,
                        ctvConfVaga.Serie));
                }

                if (!CtvConfTurno.VerificaTurno(ctvConfVaga.IdAgendaConfTurnoVaga, ctvConfVaga.Censo, ctvConfVaga.Turno))
                {
                    mensagens.Add(string.Format("O Turno: {0} não está habilitado para o curso/serie da Turma: {1}.",
                        nomeTurno,
                        ctvConfVaga.Turma));
                }


                //Verifica turno de continuidade
                if (ctvConfVaga.VagasContinuidade > 0)
                {
                    if (!CtvConfTurno.VerificaTurnoContinuidade(ctvConfVaga.IdAgendaConfTurnoVaga, ctvConfVaga.Censo, ctvConfVaga.Turno))
                    {
                        mensagens.Add(string.Format("O Turno: {0} não está habilitado como continuidade para o curso/serie da Turma: {1}.",
                            nomeTurno,
                            ctvConfVaga.Turma));
                    }
                }

                //Verifica turno novo
                if (ctvConfVaga.VagasNovas > 0)
                {
                    if (!CtvConfTurno.VerificaTurnoNovo(ctvConfVaga.IdAgendaConfTurnoVaga, ctvConfVaga.Censo, ctvConfVaga.Turno))
                    {
                        mensagens.Add(string.Format("O Turno: {0} não está habilitado como novo para o curso/serie da Turma: {1}.",
                           nomeTurno,
                           ctvConfVaga.Turma));
                    }
                }
            }

            if (!encerrado && ctvConfVaga.IdAgendaConfTurnoVaga > 0 && dadosTurma.DtInicioConfVagas > DateTime.Now.Date)
            {
                mensagens.Add(string.Format("O prazo para lançamento de Vagas da Turma: {0} terá inicio apenas em: {1}",
                              ctvConfVaga.Turma,
                              dadosTurma.DtInicioConfVagas.ToString("dd/MM/yyyy")));
            }

            var turnoFinalizado = CtvFinalizado.VerificaTurnoFinalizada(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Censo,
                                                                        ctvConfVaga.IdAgendaConfTurnoVaga);

            if (!encerrado && ctvConfVaga.IdAgendaConfTurnoVaga > 0 && !turnoFinalizado && dadosTurma.DtFimConfTurno > DateTime.Now.Date)
            {
                mensagens.Add(string.Format("O lançamento de Turnos da Turma: {0} ainda nao foi finalizado.",
                    ctvConfVaga.Turma));
            }

            //Verifica se a agenda/censo já foi finalizada
            var finalizado = CtvFinalizado.VerificaVagaFinalizada(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Censo,
                                                                  ctvConfVaga.IdAgendaConfTurnoVaga);

            var perfilResp = RN.PerfilModalidade.RetornaPerfilResponsavel(dadosTurma.Modalidade);

            if (Descricaoperfil == "SUPED" || Descricaoperfil == "SUPLAN")
            {
                if (Descricaoperfil != perfilResp)
                {
                    mensagens.Add(string.Format("Para alocar a Turma: {0} é necessário ter o perfil: {1}.",
                        ctvConfVaga.Turma,
                        perfilResp));
                }
            }

            if (ctvConfVaga.VagasContinuidade < 0)
            {
                mensagens.Add("O valor de VAGAS DE CONTINUIDADE não pode ser menor que 0.");
            }

            if (ctvConfVaga.VagasNovas < 0)
            {
                mensagens.Add("O valor de VAGAS NOVAS não pode ser menor que 0.");
            }

            if (ctvConfVaga.VagasContinuidade > 999)
            {
                mensagens.Add("O valor de VAGAS DE CONTINUIDADE não pode ser maior que 999.");
            }

            if (ctvConfVaga.VagasNovas > 999)
            {
                mensagens.Add("O valor de VAGAS NOVAS não pode ser maior que 999.");
            }

            somaVagas = ctvConfVaga.VagasContinuidade + ctvConfVaga.VagasNovas;

            //Anderson Wernek
            int resultadoCapacidadeMaxima = 0;
            var salaCapacidadeMunicipio = capacidadeAlunoTurmaMunicipio.RetornaCapacidaMaximaDeAlunoTurmaMunicipioPor(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Censo);

            if (salaCapacidadeMunicipio == null)
            {
                resultadoCapacidadeMaxima = ctvConfVaga.SalaCapacidade;

                var capacidade = RN.CapacidaDeAlunoTurma.Carregar(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Curso);

                if (capacidade.CapacidaDeAlunoTurmaId > 0)
                {
                    resultadoCapacidadeMaxima = capacidade.CapacidadeMaxima;
                }
            }
            else
            {
                resultadoCapacidadeMaxima = salaCapacidadeMunicipio.Capacidade;
            }

            if (somaVagas > resultadoCapacidadeMaxima)
            {
                mensagens.Add(string.Format("Somatório dos campos VC e VN não pode ser maior que {0}, que representa a capacidade do municipio da unidade, ou do curso {1} (resolução SEEDUC 4778 de 20/mar/2012) ou da sala {2}.",
                    resultadoCapacidadeMaxima,
                    dadosTurma.NomeCurso,
                    ctvConfVaga.Sala));
            }

            //Verifica se possui somente confirmacao de turnos para continuidade para o curso / serie / turno em questao, desconsiderar a regras.
            if (CtvConfTurno.VerificaTurnoNovo(ctvConfVaga.IdAgendaConfTurnoVaga, ctvConfVaga.Censo, ctvConfVaga.Turno))
            {
                if (Descricaoperfil == "DIRETOR_UE" &&
                ctvConfVaga.Curso != "0001.27" &&
                ctvConfVaga.Curso != "0001.17" &&
                ctvConfVaga.Curso != "0002.37" &&
                ctvConfVaga.Curso != "0091.29" &&
                ctvConfVaga.Curso != "0092.39" &&
                ctvConfVaga.Curso != "0001.51" &&
                ctvConfVaga.Curso != "0001.16" &&
                dadosTurma.Modalidade != "ED4" &&
                dadosTurma.Modalidade != "ED5" &&
                dadosTurma.Modalidade != "ED6")
                {
                    int resultadoCapacidadeMinima = 20;

                    if (resultadoCapacidadeMaxima < resultadoCapacidadeMinima)
                    {
                        resultadoCapacidadeMinima = resultadoCapacidadeMaxima;
                    }

                    if (somaVagas < resultadoCapacidadeMinima)
                    {
                        mensagens.Add(string.Format("O somatório dos campos VC e VN não pode ser menor que: {0} que representa a capacidade da sala ou resolução SEEDUC no. 4778 de 20/mar/2012 para o curso {1}",
                            resultadoCapacidadeMinima,
                            dadosTurma.NomeCurso));
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        private ValidacaoDados Valida(DadosConfVagaEncadeado ctvConfVaga, string Descricaoperfil, int codPerfil, int tipoEventoId, List<string> turmas)
        {
            RN.CapacidadeAlunoTurmaMunicipio capacidadeAlunoTurmaMunicipio = new RN.CapacidadeAlunoTurmaMunicipio();
            List<string> mensagens = new List<string>();
            int somaVagas = 0;
            string nomeTurno = string.Empty;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ctvConfVaga == null)
            {
                return validacaoDados;
            }

            //campos obrigatorios          

            if (ctvConfVaga.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (ctvConfVaga.Periodo < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }

            if (string.IsNullOrEmpty(Descricaoperfil))
            {
                mensagens.Add("O campo PERFIL é obrigatório!");
            }

            if (codPerfil < 0)
            {
                mensagens.Add("O campo CODIGO PERFIL é obrigatório!");
            }

            if (ctvConfVaga.IdAgendaConfTurnoVaga < 0)
            {
                mensagens.Add("O campo IDAGENDACONFTURNOVAGA é obrigatório!");
            }

            if (tipoEventoId <= 0)
            {
                mensagens.Add("O campo TIPO EVENTO é obrigatório!");
            }

            if (string.IsNullOrEmpty(ctvConfVaga.Turno))
            {
                mensagens.Add("O campo TURNO é obrigatório!");
            }
            else
            {
                switch (ctvConfVaga.Turno)
                {
                    case "M":
                        nomeTurno = "Manha";
                        break;
                    case "N":
                        nomeTurno = "Noite";
                        break;
                    case "T":
                        nomeTurno = "Tarde";
                        break;
                    case "I":
                        nomeTurno = "Integral";
                        break;
                    case "A":
                        nomeTurno = "Ampliado";
                        break;
                }
            }

            if (string.IsNullOrEmpty(ctvConfVaga.Sala))
            {
                mensagens.Add("O campo SALA é obrigatório!");
            }

            if (string.IsNullOrEmpty(ctvConfVaga.Censo))
            {
                mensagens.Add("O campo CENSO é obrigatório!");
            }

            if (string.IsNullOrEmpty(ctvConfVaga.Turma))
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }
            else
            {
                //Verificar se a turma já esta na lista de turmas lançadadas
                if (turmas.Contains(ctvConfVaga.Turma))
                {
                    mensagens.Add(string.Format("ERRO: A turma {0} se encontra alocada em mais de uma sala.", ctvConfVaga.Turma));
                }
                else
                {
                    //Coloca a turma na lista de turmas lançadadas
                    turmas.Add(ctvConfVaga.Turma);
                }
            }

            if (string.IsNullOrEmpty(ctvConfVaga.Matricula)
                || (!string.IsNullOrEmpty(ctvConfVaga.Matricula)
                    && ctvConfVaga.Matricula.Length > 12))
            {
                mensagens.Add("O campo MATRICULA é obrigatório com o máximo de 12 caracteres!");
            }

            if (ctvConfVaga.SalaCapacidade <= 0)
            {
                mensagens.Add(string.Format("A sala {0} está com a capacidade zerada. Verifique o cadastro de dependências.",
                    ctvConfVaga.Sala));
            }

            //Carregar dados da turma (idAgenda, curso, curriculo, serie)
            var dadosTurma = CtvAgendaConfTurnoVaga.VerificaDadosTurma(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Censo,
                                                                       ctvConfVaga.Turma, ctvConfVaga.Turno);
            if (dadosTurma == null || dadosTurma.IdAgenda <= 0)
            {
                //Se não encontrar turma no ano/periodo referencia, ver turma provisoria
                dadosTurma = CtvAgendaConfTurnoVaga.VerificaDadosTurmaProvisoria(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Censo,
                                                                                 ctvConfVaga.Turma, ctvConfVaga.Turno);
            }

            var encerrado = false;

            if (string.IsNullOrEmpty(dadosTurma.Curriculo))
            {
                mensagens.Add(string.Format("Não foi encontrada matriz curricular cadastrada para o ano: {0}, periodo: {1}, curso: {2}, serie: {3}, turno: {4}!",
                    ctvConfVaga.Ano, ctvConfVaga.Periodo, dadosTurma.NomeCurso, dadosTurma.Serie, ctvConfVaga.Turno));
            }
            else
            {
                //Carregar campos necessarios
                ctvConfVaga.IdAgendaConfTurnoVaga = dadosTurma.IdAgenda;
                ctvConfVaga.Curso = dadosTurma.Curso;
                ctvConfVaga.Curriculo = dadosTurma.Curriculo;
                ctvConfVaga.Serie = dadosTurma.Serie;
            }

            if (mensagens.Count == 0 && ctvConfVaga.IdAgendaConfTurnoVaga == 0)
            {
                mensagens.Add("Não existe agenda para este ano / periodo / curso / série ou ela já foi encerrada.");
            }
            else
            {
                //Carregar dados da agenda: ano e periodo referencia
                var agenda = RN.CtvAgendaConfTurnoVaga.Carregar(ctvConfVaga.IdAgendaConfTurnoVaga);

                if (agenda.Encerrado)
                {
                    mensagens.Add(string.Format("O lançamento de vagas para este ano / periodo / curso: {0} série: {1} já foi encerrado.",
                         dadosTurma.NomeCurso,
                         ctvConfVaga.Serie));
                    encerrado = true;
                }
            }

            if (ctvConfVaga.IdAgendaConfTurnoVaga > 0 && !encerrado)
            {
                //Verifica se existe proposta SEEDUC para o censo / ano / periodo / curso / serie
                if (!CtvPropostaSeeduc.VerificaPropostaSeeducPorAgenda(ctvConfVaga.IdAgendaConfTurnoVaga, ctvConfVaga.Censo))
                {
                    mensagens.Add(string.Format("Não existe proposta Seeduc para o curso: {0} série: {1}.",
                        dadosTurma.NomeCurso,
                        ctvConfVaga.Serie));
                }

                if (!CtvConfTurno.VerificaTurno(ctvConfVaga.IdAgendaConfTurnoVaga, ctvConfVaga.Censo, ctvConfVaga.Turno))
                {
                    mensagens.Add(string.Format("O Turno: {0} não está habilitado para o curso/serie da Turma: {1}.",
                        nomeTurno,
                        ctvConfVaga.Turma));
                }

                //Verifica turno de continuidade
                if (ctvConfVaga.VagasContinuidade > 0)
                {
                    if (!CtvConfTurno.VerificaTurnoContinuidade(ctvConfVaga.IdAgendaConfTurnoVaga, ctvConfVaga.Censo, ctvConfVaga.Turno))
                    {
                        mensagens.Add(string.Format("O Turno: {0} não está habilitado como continuidade para o curso/serie da Turma: {1}.",
                            nomeTurno,
                            ctvConfVaga.Turma));
                    }
                }

                //Verifica turno novo
                if (ctvConfVaga.VagasNovas > 0)
                {
                    if (!CtvConfTurno.VerificaTurnoNovo(ctvConfVaga.IdAgendaConfTurnoVaga, ctvConfVaga.Censo, ctvConfVaga.Turno))
                    {
                        mensagens.Add(string.Format("O Turno: {0} não está habilitado como novo para o curso/serie da Turma: {1}.",
                           nomeTurno,
                           ctvConfVaga.Turma));
                    }
                }
            }

            if (!encerrado && ctvConfVaga.IdAgendaConfTurnoVaga > 0 && dadosTurma.DtInicioConfVagas > DateTime.Now.Date)
            {
                mensagens.Add(string.Format("O prazo para lançamento de Vagas da Turma: {0} terá inicio apenas em: {1}",
                              ctvConfVaga.Turma,
                              dadosTurma.DtInicioConfVagas.ToString("dd/MM/yyyy")));
            }

            var turnoFinalizado = CtvFinalizado.VerificaTurnoFinalizada(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Censo,
                                                                        ctvConfVaga.IdAgendaConfTurnoVaga);

            if (!encerrado && ctvConfVaga.IdAgendaConfTurnoVaga > 0 && !turnoFinalizado && dadosTurma.DtFimConfTurno > DateTime.Now.Date)
            {
                mensagens.Add(string.Format("O lançamento de Turnos da Turma: {0} ainda nao foi finalizado.",
                    ctvConfVaga.Turma));
            }

            //Verifica se a agenda/censo já foi finalizada
            var finalizado = CtvFinalizado.VerificaVagaFinalizada(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Censo,
                                                                  ctvConfVaga.IdAgendaConfTurnoVaga);

            var perfilResp = RN.PerfilModalidade.RetornaPerfilResponsavel(dadosTurma.Modalidade);

            if (Descricaoperfil == "SUPED" || Descricaoperfil == "SUPLAN")
            {
                if (Descricaoperfil != perfilResp)
                {
                    mensagens.Add(string.Format("Para alocar a Turma: {0} é necessário ter o perfil: {1}.",
                        ctvConfVaga.Turma,
                        perfilResp));
                }
            }

            if (ctvConfVaga.VagasContinuidade < 0)
            {
                mensagens.Add("O valor de VAGAS DE CONTINUIDADE não pode ser menor que 0.");
            }

            if (ctvConfVaga.VagasNovas < 0)
            {
                mensagens.Add("O valor de VAGAS NOVAS não pode ser menor que 0.");
            }

            if (ctvConfVaga.VagasContinuidade > 999)
            {
                mensagens.Add("O valor de VAGAS DE CONTINUIDADE não pode ser maior que 999.");
            }

            if (ctvConfVaga.VagasNovas > 999)
            {
                mensagens.Add("O valor de VAGAS NOVAS não pode ser maior que 999.");
            }

            somaVagas = ctvConfVaga.VagasContinuidade + ctvConfVaga.VagasNovas;

            //Anderson Wernek
            int resultadoCapacidadeMaxima = 0;
            var salaCapacidadeMunicipio = capacidadeAlunoTurmaMunicipio.RetornaCapacidaMaximaDeAlunoTurmaMunicipioPor(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Censo);

            if (salaCapacidadeMunicipio == null)
            {
                resultadoCapacidadeMaxima = ctvConfVaga.SalaCapacidade;

                var capacidade = RN.CapacidaDeAlunoTurma.Carregar(ctvConfVaga.Ano, ctvConfVaga.Periodo, ctvConfVaga.Curso);

                if (capacidade.CapacidaDeAlunoTurmaId > 0)
                {
                    resultadoCapacidadeMaxima = capacidade.CapacidadeMaxima;
                }
            }
            else
            {
                resultadoCapacidadeMaxima = salaCapacidadeMunicipio.Capacidade;
            }

            if (somaVagas > resultadoCapacidadeMaxima)
            {
                mensagens.Add(string.Format("Somatório dos campos VC e VN não pode ser maior que {0}, que representa a capacidade do municipio da unidade, ou do curso {1} (resolução SEEDUC 4778 de 20/mar/2012) ou da sala {2}.",
                    resultadoCapacidadeMaxima,
                    dadosTurma.NomeCurso,
                    ctvConfVaga.Sala));
            }

            //Verifica se possui somente confirmacao de turnos para continuidade para o curso / serie / turno em questao, desconsiderar a regras.
            if (CtvConfTurno.VerificaTurnoNovo(ctvConfVaga.IdAgendaConfTurnoVaga, ctvConfVaga.Censo, ctvConfVaga.Turno))
            {
                if (Descricaoperfil == "DIRETOR_UE" &&
                ctvConfVaga.Curso != "0001.27" &&
                ctvConfVaga.Curso != "0001.17" &&
                ctvConfVaga.Curso != "0002.37" &&
                ctvConfVaga.Curso != "0091.29" &&
                ctvConfVaga.Curso != "0092.39" &&
                ctvConfVaga.Curso != "0001.51" &&
                ctvConfVaga.Curso != "0001.16" &&
                dadosTurma.Modalidade != "ED4" &&
                dadosTurma.Modalidade != "ED5" &&
                dadosTurma.Modalidade != "ED6")
                {
                    int resultadoCapacidadeMinima = 20;

                    if (resultadoCapacidadeMaxima < resultadoCapacidadeMinima)
                    {
                        resultadoCapacidadeMinima = resultadoCapacidadeMaxima;
                    }

                    if (somaVagas < resultadoCapacidadeMinima)
                    {
                        mensagens.Add(string.Format("O somatório dos campos VC e VN não pode ser menor que: {0} que representa a capacidade da sala ou resolução SEEDUC no. 4778 de 20/mar/2012 para o curso {1}",
                            resultadoCapacidadeMinima,
                            dadosTurma.NomeCurso));
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


        private static int VerificaUsoSalaOutroPeriodoPor(int ano, int periodo, string censo, string sala, string turno)
        {
            int periodoUtilizado = -1;

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  DISTINCT
                                    PERIODO
                            FROM    DBO.TCE_CTV_CONF_VAGA V
                                    INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                            WHERE   CENSO = @CENSO
                                    AND A.ANO = @ANO
                                    AND SALA = @SALA
                                    AND PERIODO <> @PERIODO
                                    AND TURNO = @TURNO "
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SALA", sala);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TURNO", turno);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        periodoUtilizado = Convert.ToInt32(reader["PERIODO"]);
                    }
                }

                return periodoUtilizado;
            }
        }


        public void ProcessaListaEncadeada(List<DadosConfVagaEncadeado> listaPaisVagas, List<DadosConfVagaEncadeado> listaTotalVagas, List<string> mensagensParcial, bool finalizar, List<string> turmas, string PerfilUsuario, int codPerfil, List<DadosVagaSalva> listaVagasSalvas)
        {
            foreach (DadosConfVagaEncadeado pai in listaPaisVagas)
            {
                if (!pai.Validado)
                {
                    List<string>  errosValidacao = new List<string>();
                    List<DadosVagaSalva> encadeamentoSalvo = new List<DadosVagaSalva>();

                    ValidaTurmasEncadeadas(listaTotalVagas, pai, errosValidacao, finalizar, turmas, PerfilUsuario, codPerfil);

                    if (errosValidacao.Count() == 0)
                    {
                        //Cria um contexto para cada pai
                        DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
                        try
                        {
                            SalvaTurmaEncadeada(ctx, listaTotalVagas, pai, encadeamentoSalvo);

                            //Atualiza lista com ids que foram salvos no banco
                            listaVagasSalvas.AddRange(encadeamentoSalvo);                           
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
                    else
                    {
                        mensagensParcial.AddRange(errosValidacao);
                    }
                }
            }
        }

        private void ValidaTurmasEncadeadas(List<DadosConfVagaEncadeado> listaTotalVagas, DadosConfVagaEncadeado vaga, List<string> mensagensParcial, bool finalizar, List<string> turmas, string PerfilUsuario, int codPerfil)
        {
            RN.CtvConfVaga rnCtvConfVaga = new RN.CtvConfVaga();
            ValidacaoDados validacao = new ValidacaoDados();
            string erro = string.Empty;
            int tipoEventoConfirmacaoVagas = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoVagas);

            vaga.Validado = true;

            //Caso exista turma fazer validações
            if (!vaga.Turma.IsNullOrEmptyOrWhiteSpace())
            {
                //Verifica se é finalização ou salvamento parcial, para escolher qual validação deve ser feita
                if (finalizar)
                {
                    validacao = rnCtvConfVaga.Valida(vaga, PerfilUsuario, codPerfil, tipoEventoConfirmacaoVagas, turmas);
                }
                else
                {
                    validacao = rnCtvConfVaga.ValidaParcialmente(vaga, PerfilUsuario, codPerfil, tipoEventoConfirmacaoVagas, turmas);
                }

                if (!validacao.Valido)
                {
                    //adicionado erro na lista
                    mensagensParcial.Add(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }

            //Chama validação para turma filha
            if (!string.IsNullOrEmpty(vaga.TurmaReferenciada))
            {
                var proximoItem = listaTotalVagas.Where(x => x.Turma == vaga.TurmaReferenciada && !x.Validado).ToList();
                if (proximoItem.Count() > 0)
                {
                    ValidaTurmasEncadeadas(listaTotalVagas, proximoItem.First(), mensagensParcial, finalizar, turmas, PerfilUsuario, codPerfil);
                }
            }
        }

        private void SalvaTurmaEncadeada(DataContext ctx, List<DadosConfVagaEncadeado> listaTotalVagas, DadosConfVagaEncadeado vaga, List<DadosVagaSalva> encadeamentoSalvo)
        {
            string idSalvo = string.Empty;
            vaga.Processado = true;
            DadosVagaSalva dadosSalvos = new DadosVagaSalva();

            try
            {
                if (!vaga.Turma.IsNullOrEmptyOrWhiteSpace())
                {
                    idSalvo = RN.CtvConfVaga.SalvaItem(ctx, vaga).ToString();
                }
                else
                {
                    RN.CtvConfVaga.Remove(ctx, vaga.IdConfVaga);
                }

                //Coloca turno que foi salva na lista para atualização na tela
                dadosSalvos.IdSalvo = idSalvo;
                dadosSalvos.Sala = vaga.Sala;
                dadosSalvos.Turma = vaga.Turma;
                dadosSalvos.Turno = vaga.Turno;
                encadeamentoSalvo.Add(dadosSalvos);

                //Chama salvar para turma filha
                if (!string.IsNullOrEmpty(vaga.TurmaReferenciada))
                {
                    var proximoItem = listaTotalVagas.Where(x => x.Turma == vaga.TurmaReferenciada && !x.Processado).ToList();
                    if (proximoItem.Count() > 0)
                    {
                        SalvaTurmaEncadeada(ctx, listaTotalVagas, proximoItem.First(), encadeamentoSalvo);
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
        }

        private static int SalvaItem(DataContext ctx, DadosConfVagaEncadeado dadosConfVaga)
        {
            ContextQuery contextQuery = new ContextQuery();
            TceCtvConfVaga ctvConfVaga = new TceCtvConfVaga();

            try
            { 
                //Carregar dados da turma (idAgenda, curso, curriculo, serie)
                var dadosTurma = CtvAgendaConfTurnoVaga.VerificaDadosTurma(dadosConfVaga.Ano, dadosConfVaga.Periodo, dadosConfVaga.Censo,
                                                                           dadosConfVaga.Turma, dadosConfVaga.Turno);
                if (dadosTurma == null || dadosTurma.IdAgenda <= 0)
                {
                    //Se não encontrar turma no ano/periodo referencia, ver turma provisoria
                    dadosTurma = CtvAgendaConfTurnoVaga.VerificaDadosTurmaProvisoria(dadosConfVaga.Ano, dadosConfVaga.Periodo, dadosConfVaga.Censo,
                                                                                     dadosConfVaga.Turma, dadosConfVaga.Turno);
                }

                //Carregar campos necessarios
                dadosConfVaga.IdAgendaConfTurnoVaga = dadosTurma.IdAgenda;
                dadosConfVaga.Curso = dadosTurma.Curso;
                dadosConfVaga.Curriculo = dadosTurma.Curriculo;
                dadosConfVaga.Serie = dadosTurma.Serie;

                //Monta entidade TceCtvConfVaga
                ctvConfVaga.IdConfVaga = dadosConfVaga.IdConfVaga;
                ctvConfVaga.Turno = dadosConfVaga.Turno;
                ctvConfVaga.Sala = dadosConfVaga.Sala;
                ctvConfVaga.Censo = dadosConfVaga.Censo;
                ctvConfVaga.Turma = dadosConfVaga.Turma;
                ctvConfVaga.VagasNovas = dadosConfVaga.VagasNovas;
                ctvConfVaga.VagasContinuidade = dadosConfVaga.VagasContinuidade;
                ctvConfVaga.Matricula = dadosConfVaga.Matricula;
                ctvConfVaga.DtCadastro = dadosConfVaga.DtCadastro;
                ctvConfVaga.DtAlteracao = dadosConfVaga.DtAlteracao;
                ctvConfVaga.SalaCapacidade = dadosConfVaga.SalaCapacidade;
                ctvConfVaga.IdAgendaConfTurnoVaga = dadosConfVaga.IdAgendaConfTurnoVaga;
                ctvConfVaga.Curso = dadosConfVaga.Curso;
                ctvConfVaga.Curriculo = dadosConfVaga.Curriculo;

                //Verifica se já existe cadastro para aquele censo / sala / agenda
                var id = RetornaIdConfirmacao(ctvConfVaga, dadosConfVaga.Ano, dadosConfVaga.Periodo);

                if (id > 0)
                {
                    ctvConfVaga.IdConfVaga = id;
                    Alterar(ctx, ctvConfVaga);
                }
                else
                {
                    id = Inserir(ctx, ctvConfVaga);
                }

                dadosConfVaga.IdConfVaga = ctvConfVaga.IdConfVaga = id;

                return id;
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

        public void RemoveDuplicidadeLancamentoTurmaPor(int ano, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            string ip = RN.Sistema.ObterIP();
            string usuario = HttpContext.Current.User.Identity.Name;
            string motivo = "Excluído automaticamente pelo sistema devido a utilização concorrente.";

            try
            {
                contextQuery.Command = @" INSERT  INTO dbo.TCE_CTV_CONF_VAGA_LOG
                                    ( ID_CONF_VAGA ,
                                      ID_AGENDA_CONF_TURNO_VAGA ,
                                      CURRICULO ,
                                      CURSO ,
                                      TURNO ,
                                      SALA ,
                                      CENSO ,
                                      TURMA ,
                                      VAGAS_NOVAS ,
                                      VAGAS_CONTINUIDADE ,
                                      MATRICULA ,
                                      DT_CADASTRO ,
                                      DT_ALTERACAO ,
                                      DATAHORALOG ,
                                      MOTIVO ,
                                      USUARIO ,
                                      IP
                                    )
                                    ( SELECT    * ,
                                                GETDATE() ,
                                                @MOTIVO ,
                                                @USUARIO ,
                                                @IP
                                      FROM      TCE_CTV_CONF_VAGA
                                      WHERE     ID_CONF_VAGA IN (
                                                SELECT  MIN(ID_CONF_VAGA)
                                                FROM    TCE_CTV_CONF_VAGA V ( NOLOCK )
                                                        INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ( NOLOCK ) ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                                WHERE   ANO = @ANO
                                                        AND CENSO = @CENSO
                                                GROUP BY ANO ,
                                                        PERIODO ,
                                                        CENSO ,
                                                        TURMA
                                                HAVING  COUNT(*) > 1 )
                                    )
        
                            DELETE  TCE_CTV_CONF_VAGA
                            WHERE   ID_CONF_VAGA IN (
                                    SELECT  MIN(ID_CONF_VAGA)
                                    FROM    TCE_CTV_CONF_VAGA V ( NOLOCK )
                                            INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ( NOLOCK ) ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                    WHERE   ANO = @ANO
                                            AND CENSO = @CENSO
                                    GROUP BY ANO ,
                                            PERIODO ,
                                            CENSO ,
                                            TURMA
                                    HAVING  COUNT(*) > 1 ) ";

                contextQuery.Parameters.Add("@MOTIVO", motivo);
                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@IP", ip);
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
            finally
            {
                ctx.Dispose();
            }
        }

        private static int RetornaIdConfirmacao(TceCtvConfVaga ctvConfVaga, int ano, int periodo)
        {
            var id = 0;

            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodos(periodo);

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        string.Format(@" SELECT  ID_CONF_VAGA
                            FROM    TCE_CTV_CONF_VAGA CV
                                    INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG ON CV.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                            WHERE   ANO = @ANO
                                    AND PERIODO in ({0})
                                    AND CENSO = @CENSO
                                    AND SALA = @SALA
                                    AND TURNO = @TURNO", possiveisPeriodos)
                };

                contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvConfVaga.IdAgendaConfTurnoVaga);
                contextQuery.Parameters.Add("@CENSO", ctvConfVaga.Censo);
                contextQuery.Parameters.Add("@SALA", ctvConfVaga.Sala);
                contextQuery.Parameters.Add("@TURNO", ctvConfVaga.Turno);
                contextQuery.Parameters.Add("@ANO", ano);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        id = Convert.ToInt32(reader["ID_CONF_VAGA"]);
                    }
                }
            }

            return id;
        }

        private static int Inserir(DataContext context, TceCtvConfVaga ctvConfVaga)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO dbo.TCE_CTV_CONF_VAGA
                        ( ID_AGENDA_CONF_TURNO_VAGA ,
                          CURRICULO ,
                          CURSO ,
                          TURNO ,
                          SALA ,
                          CENSO ,
                          TURMA ,
                          VAGAS_NOVAS ,
                          VAGAS_CONTINUIDADE ,
                          MATRICULA 
                        )
                VALUES   ( @ID_AGENDA_CONF_TURNO_VAGA ,
                          @CURRICULO ,
                          @CURSO ,
                          @TURNO ,
                          @SALA ,
                          @CENSO ,
                          @TURMA ,
                          @VAGAS_NOVAS ,
                          @VAGAS_CONTINUIDADE ,
                          @MATRICULA 
                        )

                SELECT  ID_CONF_VAGA
                    FROM    TCE_CTV_CONF_VAGA
                    WHERE   ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA
                            AND CENSO = @CENSO
                            AND SALA = @SALA
                            AND TURNO = @TURNO"
            };

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvConfVaga.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@CURRICULO", ctvConfVaga.Curriculo);
            contextQuery.Parameters.Add("@CURSO", ctvConfVaga.Curso);
            contextQuery.Parameters.Add("@TURNO", ctvConfVaga.Turno);
            contextQuery.Parameters.Add("@SALA", ctvConfVaga.Sala);
            contextQuery.Parameters.Add("@CENSO", ctvConfVaga.Censo);
            contextQuery.Parameters.Add("@TURMA", ctvConfVaga.Turma);
            contextQuery.Parameters.Add("@VAGAS_NOVAS", ctvConfVaga.VagasNovas);
            contextQuery.Parameters.Add("@VAGAS_CONTINUIDADE", ctvConfVaga.VagasContinuidade);
            contextQuery.Parameters.Add("@MATRICULA", ctvConfVaga.Matricula);

            return Convert.ToInt32(context.GetReturnValue(contextQuery));
        }

        private static void Alterar(DataContext context, TceCtvConfVaga ctvConfVaga)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  TCE_CTV_CONF_VAGA
                    SET     ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA ,
                            CURRICULO = @CURRICULO ,
                            CURSO = @CURSO ,
                            TURNO = @TURNO ,
                            TURMA = @TURMA ,
                            VAGAS_NOVAS = @VAGAS_NOVAS ,
                            VAGAS_CONTINUIDADE = @VAGAS_CONTINUIDADE ,
                            MATRICULA = @MATRICULA ,
                            DT_ALTERACAO = GETDATE()
                    WHERE   ID_CONF_VAGA = @ID_CONF_VAGA ");

            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", ctvConfVaga.IdAgendaConfTurnoVaga);
            contextQuery.Parameters.Add("@ID_CONF_VAGA", ctvConfVaga.IdConfVaga);
            contextQuery.Parameters.Add("@CURRICULO", ctvConfVaga.Curriculo);
            contextQuery.Parameters.Add("@CURSO", ctvConfVaga.Curso);
            contextQuery.Parameters.Add("@TURNO", ctvConfVaga.Turno);
            contextQuery.Parameters.Add("@TURMA", ctvConfVaga.Turma);
            contextQuery.Parameters.Add("@VAGAS_NOVAS", ctvConfVaga.VagasNovas);
            contextQuery.Parameters.Add("@VAGAS_CONTINUIDADE", ctvConfVaga.VagasContinuidade);
            contextQuery.Parameters.Add("@MATRICULA", ctvConfVaga.Matricula);

            context.ApplyModifications(contextQuery);
        }

        private static void Remove(DataContext ctx, int id)
        {
            try
            {
                var contextQuery = new ContextQuery(
                      @"DELETE  FROM TCE_CTV_CONF_VAGA
                            WHERE   ID_CONF_VAGA = @ID_CONF_VAGA");

                contextQuery.Parameters.Add("@ID_CONF_VAGA", id);

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


        public static bool VerificaEdicaoSuplanSupedDiesp(int ano, int periodo, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                    1
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                    INNER JOIN DBO.TCE_CTV_CONF_TURNO T ON A.ID_AGENDA_CONF_TURNO_VAGA = T.ID_AGENDA_CONF_TURNO_VAGA
                                                                           AND T.CENSO = @CENSO
                                    LEFT JOIN DBO.TCE_CTV_FINALIZADO FTURNO ON A.ID_AGENDA_CONF_TURNO_VAGA = FTURNO.ID_AGENDA_CONF_TURNO_VAGA
                                                                               AND FTURNO.CENSO = T.CENSO
                                                                               AND FTURNO.TURNO = 1
                                    LEFT JOIN DBO.TCE_CTV_FINALIZADO FVAGA ON A.ID_AGENDA_CONF_TURNO_VAGA = FVAGA.ID_AGENDA_CONF_TURNO_VAGA
                                                                              AND FVAGA.CENSO = T.CENSO
                                                                              AND FVAGA.VAGA = 1
                            WHERE   A.ANO = @ANO
                                    AND A.PERIODO = @PERIODO
                                    AND A.ENCERRADO = 0
                                    AND ( A.DT_FIM_CONF_TURNO < CONVERT(DATE, GETDATE())
                                          OR FTURNO.ID_FINALIZADO IS NOT NULL
                                        )
                                    AND DT_INICIO_CONF_VAGAS <= CONVERT(DATE, GETDATE())
                                    AND NOT EXISTS ( SELECT 1
                                             FROM   DBO.TCE_CTV_RESTRICAO RE
                                             WHERE  re.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                                                    AND re.CENSO = t.CENSO ) "
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool VerificaAnaliseSuplanSupedDiesp(int ano, int periodo, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                    1
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                    INNER JOIN DBO.TCE_CTV_CONF_TURNO T ON A.ID_AGENDA_CONF_TURNO_VAGA = T.ID_AGENDA_CONF_TURNO_VAGA
                                                                           AND T.CENSO = @CENSO
                                    LEFT JOIN DBO.TCE_CTV_FINALIZADO FTURNO ON A.ID_AGENDA_CONF_TURNO_VAGA = FTURNO.ID_AGENDA_CONF_TURNO_VAGA
                                                                               AND FTURNO.CENSO = T.CENSO
                                                                               AND FTURNO.TURNO = 1
                                    LEFT JOIN DBO.TCE_CTV_FINALIZADO FVAGA ON A.ID_AGENDA_CONF_TURNO_VAGA = FVAGA.ID_AGENDA_CONF_TURNO_VAGA
                                                                              AND FVAGA.CENSO = T.CENSO
                                                                              AND FVAGA.VAGA = 1
                            WHERE   A.ANO = @ANO
                                    AND A.PERIODO = @PERIODO
                                    AND A.ENCERRADO = 0
                                    AND ( A.DT_FIM_CONF_TURNO < CONVERT(DATE, GETDATE())
                                          OR FTURNO.ID_FINALIZADO IS NOT NULL
                                        )
                                    AND ( A.DT_FIM_CONF_VAGAS < CONVERT(DATE, GETDATE())
                                          OR FVAGA.ID_FINALIZADO IS NOT NULL
                                        )
                                    AND NOT EXISTS ( SELECT 1
                         FROM   DBO.TCE_CTV_RESTRICAO RE
                         WHERE  re.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                                AND re.CENSO = t.CENSO ) "
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool VerificaEdicaoDiretor(int ano, int periodo, string censo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT TOP 1
                                    1
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                    INNER JOIN DBO.TCE_CTV_CONF_TURNO T ON A.ID_AGENDA_CONF_TURNO_VAGA = T.ID_AGENDA_CONF_TURNO_VAGA
                                                                           AND T.CENSO = @CENSO
                                    LEFT JOIN DBO.TCE_CTV_FINALIZADO FTURNO ON A.ID_AGENDA_CONF_TURNO_VAGA = FTURNO.ID_AGENDA_CONF_TURNO_VAGA
                                                                               AND FTURNO.CENSO = T.CENSO
                                                                               AND FTURNO.TURNO = 1
                                    LEFT JOIN DBO.TCE_CTV_FINALIZADO FVAGA ON A.ID_AGENDA_CONF_TURNO_VAGA = FVAGA.ID_AGENDA_CONF_TURNO_VAGA
                                                                              AND FVAGA.CENSO = T.CENSO
                                                                              AND FVAGA.VAGA = 1
                            WHERE   A.ANO = @ANO
                                    AND A.PERIODO = @PERIODO
                                    AND A.ENCERRADO = 0
                                    AND ( A.DT_FIM_CONF_TURNO < CONVERT(DATE, GETDATE())
                                          OR FTURNO.ID_FINALIZADO IS NOT NULL
                                        )
                                    AND CONVERT(DATE, GETDATE()) BETWEEN A.DT_INICIO_CONF_VAGAS
                                                                 AND     A.DT_FIM_CONF_VAGAS
                                    AND FVAGA.ID_FINALIZADO IS NULL 
                                    AND NOT EXISTS ( SELECT 1
                                         FROM   DBO.TCE_CTV_RESTRICAO RE
                                         WHERE  re.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                                                AND re.CENSO = t.CENSO ) "
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public string ObtemPeriodosAbertosPor(string censo, int ano, int tipoEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string periodos = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                PERIODO
                        FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                INNER JOIN DBO.TCE_CTV_CONF_TURNO T ON A.ID_AGENDA_CONF_TURNO_VAGA = T.ID_AGENDA_CONF_TURNO_VAGA
                                                                       AND T.CENSO = @CENSO
                                LEFT JOIN DBO.TCE_CTV_FINALIZADO FTURNO ON A.ID_AGENDA_CONF_TURNO_VAGA = FTURNO.ID_AGENDA_CONF_TURNO_VAGA
                                                                           AND FTURNO.CENSO = T.CENSO
                                                                           AND FTURNO.TURNO = 1
                                LEFT JOIN DBO.TCE_CTV_FINALIZADO FVAGA ON A.ID_AGENDA_CONF_TURNO_VAGA = FVAGA.ID_AGENDA_CONF_TURNO_VAGA
                                                                          AND FVAGA.CENSO = T.CENSO
                                                                          AND FVAGA.VAGA = 1
                        WHERE   A.ANO = @ANO
                                AND A.PERIODO NOT IN (
                                SELECT  PERIODO
                                FROM    AGENDA.PERIODOLETIVOAGENDA P
                                        INNER JOIN agenda.AGENDA AA ON aa.AGENDAID = P.AGENDAID
                                        INNER JOIN agenda.EVENTO AE ON AA.AGENDAID = AE.AGENDAID
                                WHERE   GETDATE() BETWEEN DATAINICIO AND DATAFIM
                                        AND AE.TIPOEVENTOID = @TIPOEVENTOID --TipoEvento Copnfirmação de Vagas
                                        AND ANO = A.ANO )
                                AND A.ENCERRADO = 0
                                AND ( A.DT_FIM_CONF_TURNO < CONVERT(DATE, GETDATE())
                                      OR FTURNO.ID_FINALIZADO IS NOT NULL
                                    )
                                AND CONVERT(DATE, GETDATE()) BETWEEN A.DT_INICIO_CONF_VAGAS
                                                             AND     A.DT_FIM_CONF_VAGAS
                                AND FVAGA.ID_FINALIZADO IS NULL
                                AND NOT EXISTS ( SELECT 1
                                                 FROM   DBO.TCE_CTV_RESTRICAO RE
                                                 WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                                        AND RE.CENSO = T.CENSO ) ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    periodos += Convert.ToString(reader["PERIODO"]);
                    periodos += "/";
                }

                if (!string.IsNullOrEmpty(periodos))
                {
                    periodos = periodos.Substring(0, (periodos.Length - 1));
                }

                return periodos;
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

        public static string ObtemPeriodosEmAbertoPor(string censo, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT DISTINCT PERIODO
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                    INNER JOIN DBO.TCE_CTV_CONF_TURNO T ON A.ID_AGENDA_CONF_TURNO_VAGA = T.ID_AGENDA_CONF_TURNO_VAGA
                                                                           AND T.CENSO = @CENSO
                                    LEFT JOIN DBO.TCE_CTV_FINALIZADO FTURNO ON A.ID_AGENDA_CONF_TURNO_VAGA = FTURNO.ID_AGENDA_CONF_TURNO_VAGA
                                                                               AND FTURNO.CENSO = T.CENSO
                                                                               AND FTURNO.TURNO = 1
                                    LEFT JOIN DBO.TCE_CTV_FINALIZADO FVAGA ON A.ID_AGENDA_CONF_TURNO_VAGA = FVAGA.ID_AGENDA_CONF_TURNO_VAGA
                                                                              AND FVAGA.CENSO = T.CENSO
                                                                              AND FVAGA.VAGA = 1
                            WHERE   A.ANO = @ANO
                                    AND A.PERIODO <> @PERIODO
                                    AND A.ENCERRADO = 0
                                    AND ( A.DT_FIM_CONF_TURNO < CONVERT(DATE, GETDATE())
                                          OR FTURNO.ID_FINALIZADO IS NOT NULL
                                        )
                                    AND CONVERT(DATE, GETDATE()) BETWEEN A.DT_INICIO_CONF_VAGAS
                                                                 AND     A.DT_FIM_CONF_VAGAS
                                    AND FVAGA.ID_FINALIZADO IS NULL 
                                    AND NOT EXISTS ( SELECT 1
                                         FROM   DBO.TCE_CTV_RESTRICAO RE
                                         WHERE  re.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                                                AND re.CENSO = t.CENSO )  "
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                string periodos = string.Empty;

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        periodos += Convert.ToString(reader["PERIODO"]);
                        periodos += "/";
                    }
                }

                if (!string.IsNullOrEmpty(periodos))
                {
                    periodos = periodos.Substring(0, (periodos.Length - 1));
                }

                return periodos;
            }
        }

        public static object RetornaDetalhes(string ano, string periodo, string censo)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"
                                SELECT CV.censo                                       CENSO, 
                                       MC.descricao                                   MODALIDADE, 
                                       TC.descricao                                   SEGMENTO, 
                                       AG.curso                                       CURSO, 
                                       CR.nome                                        NOMECURSO, 
                                       AG.serie                                       SERIE, 
                                       CONVERT(VARCHAR(10), PS.vagas_continuidade) 
                                       + ' / ' + CONVERT(VARCHAR(10), PS.vagas_novas) AS PROPOSTA, 
                                       CONVERT(VARCHAR(10), Sum(CV.vagas_continuidade)) 
                                       + ' / ' 
                                       + CONVERT(VARCHAR(10), Sum(CV.vagas_novas))    AS CONFIRMACAO, 
                                       JT.justificativa_continuidade AS JUSTIFICATIVA_VC,
                                       JT.justificativa_novo AS JUSTIFICATIVA_VN 
                                FROM   tce_ctv_conf_vaga CV 
                                       INNER JOIN tce_ctv_agenda_conf_turno_vaga AG 
                                               ON ( CV.id_agenda_conf_turno_vaga = AG.id_agenda_conf_turno_vaga 
                                                  ) 
                                       INNER JOIN ly_curso CR 
                                               ON ( CR.curso = AG.curso ) 
                                       INNER JOIN ly_modalidade_curso MC 
                                               ON ( MC.modalidade = CR.modalidade ) 
                                       INNER JOIN ly_tipo_curso TC 
                                               ON ( TC.tipo = CR.tipo ) 
                                       INNER JOIN tce_ctv_proposta_seeduc PS 
                                               ON ( PS.id_agenda_conf_turno_vaga = CV.id_agenda_conf_turno_vaga 
                                                    AND PS.censo = CV.censo ) 
                                       LEFT OUTER JOIN tce_ctv_justificativa JT 
                                                    ON ( JT.id_agenda_conf_turno_vaga = 
                                                         PS.id_agenda_conf_turno_vaga 
                                                         AND JT.censo = PS.censo 
                                                         AND JT.vaga = 1 ) 
                                WHERE  PS.censo = " + censo + @" 
                                       AND AG.ano =  " + ano + @" 
                                       AND AG.periodo =  " + periodo + @" 
                                GROUP  BY CV.censo, 
                                          MC.descricao, 
                                          TC.descricao, 
                                          AG.curso, 
                                          CR.nome, 
                                          AG.serie, 
                                          PS.vagas_continuidade, 
                                          PS.vagas_novas, 
                                          JT.justificativa_continuidade, 
                                          JT.justificativa_novo 
                                ORDER  BY CV.censo, 
                                          MC.descricao, 
                                          TC.descricao, 
                                          CR.nome, 
                                          AG.serie ");
                return Consultar(Convert.ToString(strSql));
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public DataTable ListaTurmasDuplicidadeVagasPor(int ano, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;

            try
            {
                contextQuery.Command = @" SELECT  *
                                    FROM    TCE_CTV_CONF_VAGA V ( NOLOCK )
                                            INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ( NOLOCK ) ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                    WHERE   ANO = @ANO
                                            AND CENSO = @CENSO
                                            AND TURMA IN (
                                            SELECT  TURMA
                                            FROM    TCE_CTV_CONF_VAGA V ( NOLOCK )
                                                    INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ( NOLOCK ) ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                            WHERE   ANO = @ANO
                                                    AND CENSO = @CENSO
                                            GROUP BY ANO ,
                                                    PERIODO ,
                                                    CENSO ,
                                                    TURMA
                                            HAVING  COUNT(*) > 1 )
                                            ORDER BY ID_CONF_VAGA";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);

                turmas = ctx.GetDataTable(contextQuery);
                return turmas;
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

        public List<string> ListaTurmaEmSalaInativaPor(int ano, string censo)
        {
            List<string> lista = new List<string>();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            System.Data.SqlClient.SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TURMA
                                        FROM   TCE_CTV_CONF_VAGA V
                                               INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                                       ON A.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                                               INNER JOIN LY_DEPENDENCIA D
                                                       ON D.FACULDADE = V.CENSO
                                                          AND D.DEPENDENCIA = V.SALA
                                        WHERE  ANO = @ANO
                                               AND V.CENSO = @CENSO
                                               AND ATIVA = 'N'  
                                        ORDER BY TURMA
                                                               ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);


                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lista.Add(Convert.ToString(reader["TURMA"]));
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
    }
}
