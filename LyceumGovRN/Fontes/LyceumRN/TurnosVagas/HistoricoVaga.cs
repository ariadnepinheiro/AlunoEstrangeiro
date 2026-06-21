using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Web;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class HistoricoVaga : RNBase
    {
        public void SalvaHistoricoVagaDiretor(int ano, List<int> listaPeriodos, string censo)
        {
            RN.TurnosVagas.HistoricoJustificativa rnHistoricoJustificativa = new RN.TurnosVagas.HistoricoJustificativa();
            RN.TurnosVagas.HistoricoTurnoVaga rnHistoricoTurnoVaga = new RN.TurnosVagas.HistoricoTurnoVaga();
            int tipoHistoricoDiretor = (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Diretor;
            int tipoHistoricoSeeduc = (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Seeduc;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.TurnosVagas.HistoricoTurno rnHistoricoTurno = new RN.TurnosVagas.HistoricoTurno();
            RN.TurnosVagas.HistoricoSalaAula rnHistoricoSalaAula = new HistoricoSalaAula();
            string periodos = string.Empty;

            try
            {
                periodos = listaPeriodos.ConvertAll(x => Convert.ToString(x)).Aggregate((x, y) => x + " , " + y);

                //Verifica se existe historico de turno ou justificativa de turno
                if (!rnHistoricoTurnoVaga.PossuiHistoricoTurnoDiretorPor(tipoHistoricoDiretor, ano, periodos, censo))
                {
                    //Caso nao existe historio de turno salvar turnos atuais como diretor e como seeduc
                    rnHistoricoTurnoVaga.InsereHistoricoTurnoDiretor(ctx, ano, periodos, censo, tipoHistoricoDiretor);
                    rnHistoricoTurno.InsereHistoricoTurno(ctx, ano, periodos, censo, tipoHistoricoDiretor);
                    rnHistoricoJustificativa.InsereHistoricoJustificativa(ctx, ano, periodos, censo, true, false, tipoHistoricoDiretor);

                    rnHistoricoTurnoVaga.InsereHistoricoTurnoDiretor(ctx, ano, periodos, censo, tipoHistoricoSeeduc);
                    rnHistoricoTurno.InsereHistoricoTurno(ctx, ano, periodos, censo, tipoHistoricoSeeduc);
                    rnHistoricoJustificativa.InsereHistoricoJustificativa(ctx, ano, periodos, censo, true, false, tipoHistoricoSeeduc);
                }

                //Apenas salva caso nao exista cadastrado outro historico de vaga do diretor
                if (!rnHistoricoTurnoVaga.PossuiHistoricoVagaDiretorPor(tipoHistoricoDiretor, ano, periodos, censo))
                {
                    rnHistoricoTurnoVaga.AtualizaHistoricoVaga(ctx, ano, periodos, censo, tipoHistoricoDiretor);
                    this.InsereHistoricoVaga(ctx, ano, periodos, censo, tipoHistoricoDiretor);
                    rnHistoricoJustificativa.InsereHistoricoJustificativa(ctx, ano, periodos, censo, false, true, tipoHistoricoDiretor);
                    rnHistoricoSalaAula.InsereHistoricoSala(ctx, ano, periodos, censo);
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

        public void SalvaHistoricoVagaSeeduc(int ano, List<int> listaPeriodos, string censo)
        {
            RN.TurnosVagas.HistoricoTurnoVaga rnHistoricoTurnoVaga = new RN.TurnosVagas.HistoricoTurnoVaga();
            RN.TurnosVagas.HistoricoJustificativa rnHistoricoJustificativa = new RN.TurnosVagas.HistoricoJustificativa();
            RN.TurnosVagas.HistoricoSalaAula rnHistoricoSalaAula = new HistoricoSalaAula();
            int tipoHistoricoSeeduc = (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Seeduc;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            string periodos = string.Empty;

            try
            {
                periodos = listaPeriodos.ConvertAll(x => Convert.ToString(x)).Aggregate((x, y) => x + " , " + y);

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

                //Caso já exista historico de vaga do tipo Seeduc, apaga o anterior
                this.DeletaHistoricoVaga(ctx, ano, periodos, censo, tipoHistoricoSeeduc);
                rnHistoricoJustificativa.DeletaHistoricoJustificativa(ctx, ano, periodos, censo, false, true, tipoHistoricoSeeduc);

                //Insere novos
                rnHistoricoTurnoVaga.AtualizaHistoricoVaga(ctx, ano, periodos, censo, tipoHistoricoSeeduc);
                this.InsereHistoricoVaga(ctx, ano, periodos, censo, tipoHistoricoSeeduc);
                rnHistoricoJustificativa.InsereHistoricoJustificativa(ctx, ano, periodos, censo, false, true, tipoHistoricoSeeduc);
                rnHistoricoSalaAula.InsereHistoricoSala(ctx, ano, periodos, censo);
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

        public void InsereHistoricoVaga(DataContext ctx, int ano, string periodos, string censo, int tipoHistorico)
        {
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();
            string usuarioResponsavel = HttpContext.Current.User.Identity.Name;

            try
            {
                sql.Append(@" INSERT INTO TurnosVagas.HISTORICOVAGA
                              ( ID_AGENDA_CONF_TURNO_VAGA ,
                                  HISTORICOTURNOVAGAID ,
                                  CURRICULO ,
                                  CURSO ,
                                  TURNO ,
                                  SALA ,
                                  CENSO ,
                                  TURMA ,
                                  VAGASNOVAS ,
                                  VAGASCONTINUIDADE ,
                                  DATACADASTRO ,
                                  DATAALTERACAO ,
                                  MATRICULA
                                ) ");

                if (tipoHistorico == (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Diretor)
                {
                    sql.Append(string.Format(@" SELECT DISTINCT
                                        V.ID_AGENDA_CONF_TURNO_VAGA ,
                                        H.HISTORICOTURNOVAGAID ,
                                        V.CURRICULO ,
                                        V.CURSO ,
                                        V.TURNO ,
                                        V.SALA ,
                                        V.CENSO ,
                                        V.TURMA ,
                                        V.VAGAS_NOVAS ,
                                        V.VAGAS_CONTINUIDADE ,
                                        V.DT_CADASTRO ,
                                        V.DT_ALTERACAO ,
                                        V.MATRICULA
                                FROM    DBO.TCE_CTV_CONF_VAGA V
                                        INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                        INNER JOIN TurnosVagas.HISTORICOTURNOVAGA H ON A.ANO = H.ANO
                                                                                       AND A.PERIODO = H.PERIODO
                                                                                       AND V.CENSO = H.CENSO
                                WHERE   V.CENSO = @CENSO
                                        AND A.ANO = @ANO
                                        AND A.PERIODO IN ( {0} )
		                                AND H.TIPOHISTORICOID = @TIPOHISTORICOID ", periodos));
                }

                if (tipoHistorico == (int)RN.TurnosVagas.TipoHistorico.TiposHistorico.Seeduc)
                {
                    sql.Append(string.Format(@" SELECT DISTINCT
                                V.ID_AGENDA_CONF_TURNO_VAGA ,
                                H.HISTORICOTURNOVAGAID ,
                                V.CURRICULO ,
                                V.CURSO ,
                                V.TURNO ,
                                V.SALA ,
                                V.CENSO ,
                                V.TURMA ,
                                V.VAGAS_NOVAS ,
                                V.VAGAS_CONTINUIDADE ,
                                V.DT_CADASTRO ,
                                GETDATE() ,
                                @MATRICULA
                        FROM    DBO.TCE_CTV_CONF_VAGA V
                                INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN TurnosVagas.HISTORICOTURNOVAGA H ON A.ANO = H.ANO
                                                                               AND A.PERIODO = H.PERIODO
                                                                               AND V.CENSO = H.CENSO
                        WHERE   V.CENSO = @CENSO
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

        public void DeletaHistoricoVaga(DataContext ctx, int ano, string periodos, string censo, int tipoHistorico)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = string.Format(@" DELETE  V
                        FROM    TurnosVagas.HISTORICOVAGA V
                                INNER JOIN TurnosVagas.HISTORICOTURNOVAGA H ON V.HISTORICOTURNOVAGAID = H.HISTORICOTURNOVAGAID
                        WHERE   H.TIPOHISTORICOID = @TIPOHISTORICOID
                                AND V.CENSO = @CENSO
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

        public DataTable ListaQuadroHistoricoPropostaPor(int ano, string censo, int tipoHistorico)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {
                contextQuery.Command = @"  SELECT DISTINCT
                                AG.ANO ,
                                AG.PERIODO ,
                                ag.AGENDAID ,
                                AG.SERIE ,
                                PR.CENSO ,
                                AG.ID_AGENDA_CONF_TURNO_VAGA ,
                                AG.ANO_REFERENCIA ,
                                AG.PERIODO_REFERENCIA ,
                                MC.MODALIDADE ,
                                MC.DESCRICAO AS DESCRICAO_MODALIDADE ,
                                AG.CURSO ,
                                C.NOME AS NOME_CURSO ,
                                J.JUSTIFICATIVANOVO AS JUSTIFICATIVA_NOVO ,
                                ( SELECT    SUM(VG.VAGASCONTINUIDADE)
                                  FROM      TurnosVagas.HISTORICOVAGA VG
                                            INNER JOIN TurnosVagas.HISTORICOTURNOVAGA h ON VG.HISTORICOTURNOVAGAID = h.HISTORICOTURNOVAGAID
                                  WHERE     vg.CENSO = PR.CENSO
                                            AND vg.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                            AND h.TIPOHISTORICOID = @TIPOHISTORICOID
                                  GROUP BY  vg.ID_AGENDA_CONF_TURNO_VAGA ,
                                            vg.CENSO
                                ) AS VAGAS_CONTINUIDADE ,
                                ( SELECT    SUM(VG.VAGASNOVAS)
                                  FROM      TurnosVagas.HISTORICOVAGA VG
                                            INNER JOIN TurnosVagas.HISTORICOTURNOVAGA h ON VG.HISTORICOTURNOVAGAID = h.HISTORICOTURNOVAGAID
                                  WHERE     vg.CENSO = PR.CENSO
                                            AND vg.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                            AND h.TIPOHISTORICOID = @TIPOHISTORICOID
                                  GROUP BY  vg.ID_AGENDA_CONF_TURNO_VAGA ,
                                            vg.CENSO
                                ) AS VAGAS_NOVAS ,
                                dbo.F_ctvvagasutilizadascontinuidade(pr.CENSO, ag.CURSO, ag.SERIE,
                                                                     ag.ANO, ag.PERIODO) AS VAGAS_UTILIZADAS_CONT ,
                                dbo.F_ctvvagasutilizadasnovas(pr.CENSO, ag.CURSO, ag.SERIE, ag.ANO,
                                                              ag.PERIODO) AS VAGAS_UTILIZADAS_NOVA ,
                                PR.VAGAS_CONTINUIDADE AS PROPOSTA_SEEDUC_CONT ,
                                PR.VAGAS_NOVAS AS PROPOSTA_SEEDUC_NOVA ,
                                CONVERT(VARCHAR, PR.TAXAREPROVACAO) + '%' AS TAXAREPROVACAO ,
                                CONVERT(VARCHAR, ( 100 - PR.TAXAREPROVACAO )) + '%' AS TAXAAPROVACAO ,
                                CONVERT(BIT, 0) AS EDITAVEL --Não existe edição na tela de historico
                         INTO   #vagas
                         FROM   TCE_CTV_PROPOSTA_SEEDUC PR
                                INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG ON PR.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN dbo.TCE_CTV_CONF_TURNO_INICIAL TI ON PR.CENSO = TI.CENSO
                                                                                AND AG.ID_AGENDA_CONF_TURNO_VAGA = TI.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN LY_CURSO C ON AG.CURSO = C.CURSO
                                INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                                INNER JOIN LY_TIPO_CURSO TC ON TC.TIPO = C.TIPO
                                INNER JOIN TurnosVagas.HISTORICOTURNOVAGA tv ON tv.ANO = ag.ANO
                                                                                AND tv.PERIODO = ag.PERIODO
                                                                                AND tv.CENSO = ti.CENSO
                                                                                AND tv.TIPOHISTORICOID = @TIPOHISTORICOID
                                LEFT JOIN TurnosVagas.HISTORICOJUSTIFICATIVA J ON J.ID_AGENDA_CONF_TURNO_VAGA = PR.ID_AGENDA_CONF_TURNO_VAGA
                                                                                  AND J.CENSO = PR.CENSO
                                                                                  AND J.VAGA = 1
                                                                                  AND j.HISTORICOTURNOVAGAID = tv.HISTORICOTURNOVAGAID
                         WHERE  PR.CENSO = @CENSO
                                AND AG.ANO = @ANO
                                AND NOT EXISTS ( SELECT 1
                                                 FROM   dbo.TCE_CTV_RESTRICAO RE
                                                 WHERE  re.ID_AGENDA_CONF_TURNO_VAGA = ag.ID_AGENDA_CONF_TURNO_VAGA
                                                        AND re.CENSO = PR.CENSO )
                         GROUP BY AG.ANO ,
                                AG.PERIODO ,
                                ag.AGENDAID ,
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
                                J.JUSTIFICATIVACONTINUIDADE ,
                                J.JUSTIFICATIVANOVO ,
                                J.VAGASCONTINUIDADE ,
                                J.VAGASNOVAS ,
                                J.HISTORICOJUSTIFICATIVAID ,
                                PR.VAGAS_CONTINUIDADE ,
                                PR.VAGAS_NOVAS ,
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
                         FROM   #vagas t
                         ORDER BY T.PERIODO ,
                                t.MODALIDADE ,
                                NOME_CURSO ,
                                SERIE 

                         DROP TABLE #vagas  ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistorico);

                dt = ctx.GetDataTable(contextQuery);

                //dt.Columns.Add("EDITAVEL", typeof(bool));

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    dt.Rows[i]["EDITAVEL"] = false; //Não existe edição na tela de historico
                //}
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

        public List<DadosConfVaga> ListaQuadroHistoricoSalasPor(string censo, int ano, string periodosQuadroSala, int tipoHistoricoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            List<DadosConfVaga> turmas = new List<DadosConfVaga>();
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder strSql = new StringBuilder();
            bool adicionar = true;

            try
            {
                strSql.Append(string.Format(@"  SELECT DISTINCT
                                T.HISTORICOVAGAID ,
                                D.SALA + '[' + CONVERT(VARCHAR, ISNULL(D.NUMEROALUNO, 0)) + ']' AS SALACAPACIDADE ,
                                T.ID_AGENDA_CONF_TURNO_VAGA ,
                                T.AGENDAID ,
                                T.ENCERRADO ,
                                T.DT_INICIO_CONF_VAGAS ,
                                T.DT_FIM_CONF_TURNO ,
                                CASE WHEN ( T.PERIODO = 0 ) THEN T.TURMA + ' [Anual]'
                                     ELSE T.TURMA + ' [' + CAST(T.PERIODO AS VARCHAR) + 'º Semestre]'
                                END TURMA ,
                                T.VAGASCONTINUIDADE ,
                                T.VAGASNOVAS ,
                                T.NUM_ALUNOS ,
                                T.TURNO ,
                                T.FINALIZADO ,
                                T.PERIODO
                         FROM   TurnosVagas.HISTORICOSALAAULA D
                                LEFT JOIN ( SELECT DISTINCT
                                                    A.ID_AGENDA_CONF_TURNO_VAGA ,
                                                    A.AGENDAID ,
                                                    A.ANO,
                                                    1 AS ENCERRADO ,
                                                    A.DT_INICIO_CONF_VAGAS ,
                                                    A.DT_FIM_CONF_VAGAS ,
                                                    A.DT_FIM_CONF_TURNO ,
                                                    A.PERIODO ,
                                                    V.CENSO ,
                                                    V.VAGASCONTINUIDADE ,
                                                    V.VAGASNOVAS ,
                                                    NULL AS NUM_ALUNOS ,
                                                    V.TURMA ,
                                                    V.TURNO ,
                                                    V.SALA ,
                                                    V.HISTORICOVAGAID ,
                                                    1 AS FINALIZADO
                                            FROM    TurnosVagas.HISTORICOVAGA V
                                                    INNER JOIN TurnosVagas.HISTORICOTURNOVAGA h ON v.HISTORICOTURNOVAGAID = h.HISTORICOTURNOVAGAID
                                                    INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON V.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                            WHERE   V.CENSO = @CENSO
                                                    AND A.ANO = @ANO
                                                    AND h.TIPOHISTORICOID = @TIPOHISTORICOID
                                                    AND NOT EXISTS ( SELECT 1 --Restrição de unidade
                                                                     FROM   dbo.TCE_CTV_RESTRICAO RE
                                                                     WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                                                            AND RE.CENSO = V.CENSO )
                                                    AND ( A.PERIODO IN ( {0} ) ) --Lista de periodos ativos                                          
                                            
                                          ) T ON D.CENSO = T.CENSO
                                                 AND D.SALA = T.SALA
                         WHERE  D.CENSO = @CENSO   
                                AND D.ANO = @ANO 
                                AND ( D.PERIODO IN ( {0} ) ) --Lista de periodos ativos                             
                                 ",
                    periodosQuadroSala));

                strSql.Append(@" ORDER BY SALACAPACIDADE ,
                                 TURMA DESC  ");

                contextQuery.Command = strSql.ToString();

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPOHISTORICOID", tipoHistoricoId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    adicionar = true;

                    DadosConfVaga dadosConfVaga = new DadosConfVaga
                    {
                        Periodo = !String.IsNullOrEmpty(reader["PERIODO"].ToString()) ? Convert.ToInt32(reader["PERIODO"]) : -1,
                        IdAgendaConfTurnoVaga = !String.IsNullOrEmpty(reader["ID_AGENDA_CONF_TURNO_VAGA"].ToString()) ? Convert.ToInt32(reader["ID_AGENDA_CONF_TURNO_VAGA"]) : default(int),
                        IdCtvConfVaga = !String.IsNullOrEmpty(reader["HISTORICOVAGAID"].ToString()) ? Convert.ToInt32(reader["HISTORICOVAGAID"]) : default(int),
                        SalaCapacidade = Convert.ToString(reader["salacapacidade"]),
                        Turma = Convert.ToString(reader["TURMA"]),
                        VagasContinuidade = !String.IsNullOrEmpty(reader["VAGASCONTINUIDADE"].ToString()) ? Convert.ToInt32(reader["VAGASCONTINUIDADE"]) : default(int),
                        VagasNova = !String.IsNullOrEmpty(reader["VAGASNOVAS"].ToString()) ? Convert.ToInt32(reader["VAGASNOVAS"]) : default(int),
                        NumAlunos = !String.IsNullOrEmpty(reader["NUM_ALUNOS"].ToString()) ? Convert.ToInt32(reader["NUM_ALUNOS"]) : default(int),
                        Turno = Convert.ToString(reader["TURNO"]),
                        Editavel = false,
                        DtInicioConfVaga = !String.IsNullOrEmpty(reader["DT_INICIO_CONF_VAGAS"].ToString()) ? Convert.ToDateTime(reader["DT_INICIO_CONF_VAGAS"]) : default(DateTime),
                        DtFimConfTurno = !String.IsNullOrEmpty(reader["DT_FIM_CONF_TURNO"].ToString()) ? Convert.ToDateTime(reader["DT_FIM_CONF_TURNO"]) : default(DateTime),
                        Finalizado = true,
                        AgendaEncerrada = true
                    };

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
    }
}
