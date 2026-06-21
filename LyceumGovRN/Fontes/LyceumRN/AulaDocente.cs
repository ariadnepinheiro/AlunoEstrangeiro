using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;
using System.Data;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class AulaDocente : RNBase
    {
        //verifica se tem aula alocadas com data fim da turma maior que ano atual e não são no setor de destino
        public static bool VerificaAulasAlocadasFuturas(decimal num_func, string setor)
        {
            int retorno;
            string sql = @"select top 1 1
                        from LY_AULA_DOCENTE ad
                        join LY_TURMA t on 
                        ad.ANO = t.ANO
                        AND ad.SEMESTRE = t.SEMESTRE
                        and ad.DISCIPLINA = t.DISCIPLINA
                        and ad.FACULDADE = t.FACULDADE
                        and ad.TURMA = t.TURMA
                        and ad.TURNO = t.TURNO 
                        and ad.DATA_FIM = t.DT_FIM
                        where 
                        t.sit_turma = 'Aberta'
                        and ad.NUM_FUNC = ? 
                        and DATEPART(YEAR,ad.DATA_FIM) >= DATEPART(YEAR,GETDATE())
                        and EXISTS
                           (select uass.unidade_fis from LY_UNIDADES_ASSOCIADAS uass
                            where EXISTS (select 1 from LY_UNIDADE_ENSINO ue where ue.setor <> ? and uass.UNIDADE_ENS = ue.unidade_ens)
                            and uass.unidade_fis = ad.faculdade)";
            retorno = ExecutarFuncao(sql, num_func, setor);

            if (retorno == 1)
                return true; //possui aulas alocadas no período fora deste setor
            else
                return false;
        }

        public List<decimal> ObtemDocentesEmAulaPor(string disciplina, string turma, decimal ano, decimal periodo, string aluno)
        {
            List<decimal> listaDocentes = new List<decimal>();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            decimal docente = 0;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT DISTINCT
                                NUM_FUNC
                        FROM    LY_AULA_DOCENTE A
                        WHERE   DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE 
                                AND EXISTS (SELECT * FROM LY_MATRICULA M 
                                            WHERE 
                                                    M.ANO = A.ANO 
                                                    AND M.SEMESTRE = A.SEMESTRE 
                                                    AND M.TURMA=A.TURMA 
                                                    AND M.DISCIPLINA=A.DISCIPLINA 
                                                    AND SIT_MATRICULA = 'Matriculado' 
                                                    AND ALUNO = @ALUNO)
                        ")
                };

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@ALUNO", aluno);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    docente = Convert.ToDecimal(reader["NUM_FUNC"]);
                    listaDocentes.Add(docente);
                }

                return listaDocentes;
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

        public DataTable ListaTurmaCarenciaPor(string unidadeEnsino, string agrupamentoDisciplina)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT T.TURMA, 
                                                T.DISCIPLINA, 
                                                D.NOME   AS NOMEDISCIPLINA, 
                                                GHDISC.AGRUPAMENTO, 
                                                GH.DESCRICAO, 
                                                T.ANO, 
                                                T.SEMESTRE, 
				                                AD.NUM_FUNC, 
                                                COUNT(*) AS CONTAGEMCARENCIAS,
				                                CASE 
					                                WHEN AD.NUM_FUNC = 115451 then 'Carência Temporaria'
					                                when AD.NUM_FUNC = 115460 then 'Carência Real'
				                                END TIPOCARENCIA
                                FROM   LY_AULA_DOCENTE AD (NOLOCK) 
                                       INNER JOIN LY_TURMA T (NOLOCK) 
                                               ON T.TURMA = AD.TURMA 
                                                  AND T.ANO = AD.ANO 
                                                  AND T.SEMESTRE = AD.SEMESTRE 
                                                  AND T.DISCIPLINA = AD.DISCIPLINA 
                                                  AND T.DT_FIM = AD.DATA_FIM 
                                       INNER JOIN LY_GRUPO_HABILITACAO_DISC GHDISC (NOLOCK) 
                                               ON GHDISC.DISCIPLINA = ISNULL(T.DISCIPLINA_MULTIPLA, 
                                                                      T.DISCIPLINA) 
                                       INNER JOIN LY_DISCIPLINA D (NOLOCK) 
                                               ON T.DISCIPLINA = D.DISCIPLINA 
                                       INNER JOIN LY_GRUPO_HABILITACAO GH (NOLOCK) 
                                               ON GH.AGRUPAMENTO = GHDISC.AGRUPAMENTO 
                                WHERE  GHDISC.AGRUPAMENTO = @AGRUPAMENTO  
                                       AND T.FACULDADE = @CENSO  
                                       AND AD.NUM_FUNC IN ( 115460, 115451 ) 
                                       AND CONVERT(DATE, GETDATE()) <= CONVERT(DATE, T.DT_FIM) 
                                       AND T.SIT_TURMA = 'ABERTA' 
                                GROUP  BY T.TURMA, 
                                          T.DISCIPLINA, 
                                          D.NOME, 
                                          GHDISC.AGRUPAMENTO, 
                                          GH.DESCRICAO, 
                                          T.ANO, 
                                          T.SEMESTRE, 
		                                  AD.NUM_FUNC ";

                contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, unidadeEnsino);
                contextQuery.Parameters.Add("@AGRUPAMENTO", TechneDbType.T_ALFAMEDIUM, agrupamentoDisciplina);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }
        public bool ExisteDocentesRealEmAulaAtivaPor(DataContext ctx, string turma, decimal ano, decimal periodo, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT  COUNT(do.NUM_FUNC)
                                    FROM    DBO.LY_AULA_DOCENTE a
                                    INNER JOIN LY_DISCIPLINA D ON D.DISCIPLINA = a.DISCIPLINA
                                    inner join LY_DOCENTE do on a.NUM_FUNC = do.NUM_FUNC
                                   WHERE   TURMA = @TURMA
                                            AND ANO = @ANO
                                            AND SEMESTRE = @SEMESTRE
                                            AND DATA_INICIO <> DATA_FIM
                                            AND ( DATA_FIM IS NULL
			                                      OR DATA_FIM > GETDATE()
			                                    )  
                                            AND do.MATRICULA not in('00000000','11111111','22222222','44444444','66666666','88888888','55555551','55555555','99999999')                                            
                                            AND a.DISCIPLINA = @DISCIPLINA ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool ExisteDocentesEmAulaPor(string turma, decimal ano, decimal periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(NUM_FUNC)
                                    FROM    DBO.LY_AULA_DOCENTE
                                    WHERE   TURMA = @TURMA
                                            AND ANO = @ANO
                                            AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);

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

        public bool PossuiDocentesEmAulaAtivaEletiva(DataContext ctx, string censo, int ano, int periodo, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @"  SELECT  COUNT(1)
                                    FROM    DBO.LY_AULA_DOCENTE AD (NOLOCK)
                                            INNER JOIN LY_TURMA T (NOLOCK) ON AD.ANO = T.ANO
                                                                AND AD.SEMESTRE = T.SEMESTRE
                                                                AND AD.DISCIPLINA = T.DISCIPLINA
                                                                AND AD.TURMA = T.TURMA
		                                    LEFT JOIN LY_TURMA TR (NOLOCK) ON T.TURMAREFERENCIA = TR.TURMA 
							                                    AND T.ANO = TR.ANO 
							                                    AND T.SEMESTRE = TR.SEMESTRE
                                    WHERE   ISNULL(T.ELETIVA,'N') = 'S' 
                                            AND T.ANO = @ANO
		                                    AND T.SEMESTRE = @SEMESTRE
		                                    AND ISNULL(TR.CURSO, T.CURSO)  = @CURSO
		                                    AND ISNULL(TR.SERIE, T.SERIE) = @SERIE
		                                    AND T.FACULDADE = @CENSO
                                            AND AD.DATA_INICIO <> AD.DATA_FIM
                                            AND ( DATA_FIM IS NULL
			                                        OR DATA_FIM > GETDATE() ) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int ObtemQuantidadeAulasAtivasDocentePor(DataContext ctx, string matricula)
        {
            int quantidadeAulas = 0;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*) CONTADOR
                            FROM    LY_AULA_DOCENTE A ( NOLOCK )
                                    INNER JOIN LY_DOCENTE D ( NOLOCK ) ON A.NUM_FUNC = D.NUM_FUNC
                                    INNER JOIN LY_TURMA T ON A.DISCIPLINA=T.DISCIPLINA AND A.TURMA=T.TURMA AND A.ANO=T.ANO AND A.SEMESTRE=T.SEMESTRE
                            WHERE   D.MATRICULA = @MATRICULA
                                    AND A.ANO >= YEAR(GETDATE())
                                    AND T.SIT_TURMA='Aberta'
                                    AND A.DATA_INICIO <> A.DATA_FIM
                                    AND ( A.DATA_FIM IS NULL
                                          OR A.DATA_FIM > GETDATE()
                                        ) ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    quantidadeAulas = Convert.ToInt32(reader["CONTADOR"]);
                }

                return quantidadeAulas;
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
            }
        }

        public int ObtemQuantidadeGlpsAtivasDocentePor(DataContext ctx, string matricula)
        {
            int quantidadeGlps = 0;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*) CONTADOR
                                    FROM    LY_AULA_DOCENTE_TIPO GLP
                                            INNER JOIN LY_DOCENTE D ON GLP.NUM_FUNC = D.NUM_FUNC
                                            INNER JOIN LY_TURMA T ON GLP.DISCIPLINA=T.DISCIPLINA AND GLP.TURMA=T.TURMA AND GLP.ANO=T.ANO AND GLP.SEMESTRE=T.SEMESTRE
                                    WHERE   D.MATRICULA = @MATRICULA
                                            AND GLP.TIPO_AULA = 'GLP'
                                            AND T.SIT_TURMA='Aberta'
                                            AND GLP.ANO >= YEAR(GETDATE()) ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    quantidadeGlps = Convert.ToInt32(reader["CONTADOR"]);
                }

                return quantidadeGlps;
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
            }
        }

        public int ObtemQuantidadeAulasAtivasDocentePor(string matricula)
        {
            int quantidadeAulas = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                quantidadeAulas = this.ObtemQuantidadeAulasNormaisAtivasDocentePor(ctx, matricula);

                return quantidadeAulas;
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

        public int ObtemQuantidadeAulasNormaisAtivasDocentePor(DataContext ctx, string matricula)
        {
            int quantidadeAulas = 0;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" select DBO.f_TotalAulaNormal (@MATRICULA) CONTADOR ";
                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    quantidadeAulas = Convert.ToInt32(reader["CONTADOR"]);
                }

                return quantidadeAulas;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public List<DadosHorarios> ObtemHorariosPor(DataContext ctx, string disciplina, string turma, decimal ano, decimal semestre)
        {
            List<DadosHorarios> horarios = new List<DadosHorarios>();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@"SELECT 
                                           T.DISCIPLINA, 
                                           T.TURMA, 
                                           T.ANO, 
                                           T.SEMESTRE, 
                                           T.DT_INICIO   DTINICIOTURMA, 
                                           T.DT_FIM      DTFIMTURMA, 
                                           H.DIA_SEMANA, 
                                           H.AULA, 
                                           H.HORAINI_AULA, 
                                           H.HORAFIM_AULA, 
                                           A.DATA_INICIO DTINICIOAULA, 
                                           A.DATA_FIM    DTFIMAULA 
                                    FROM   LY_TURMA T 
                                           JOIN LY_HOR_AULA H 
                                             ON T.DISCIPLINA = H.DISCIPLINA 
                                                AND T.TURMA = H.TURMA 
                                                AND T.ANO = H.ANO 
                                                AND T.SEMESTRE = H.SEMESTRE 
                                           JOIN LY_AULA_DOCENTE A 
                                             ON H.TURNO = A.TURNO 
                                                AND H.FACULDADE = A.FACULDADE 
                                                AND H.DIA_SEMANA = A.DIA_SEMANA 
                                                AND H.AULA = A.AULA 
                                                AND H.DISCIPLINA = A.DISCIPLINA 
                                                AND H.TURMA = A.TURMA 
                                                AND H.ANO = A.ANO 
                                                AND H.SEMESTRE = A.SEMESTRE 
                                    WHERE  T.DISCIPLINA = @DISCIPLINA 
                                           AND T.TURMA = @TURMA 
                                           AND T.ANO = @ANO 
                                           AND T.SEMESTRE = @SEMESTRE 
                                           AND A.DATA_FIM = T.DT_FIM 
                                           AND T.SIT_TURMA = 'Aberta'   ")
                };

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DadosHorarios d = new DadosHorarios
                    {
                        Disciplina = Convert.ToString(reader["DISCIPLINA"]),
                        Turma = Convert.ToString(reader["TURMA"]),
                        Ano = Convert.ToInt32(reader["ANO"]),
                        Semestre = Convert.ToInt32(reader["SEMESTRE"]),
                        DtInicioTurma = Convert.ToDateTime(reader["DTINICIOTURMA"]),
                        DtFimTurma = Convert.ToDateTime(reader["DTFIMTURMA"]),
                        DiaSemana = Convert.ToInt32(reader["DIA_SEMANA"]),
                        Aula = Convert.ToInt32(reader["AULA"]),
                        HoraInicioAula = Convert.ToDateTime(reader["HORAINI_AULA"]),
                        HoraFimAula = Convert.ToDateTime(reader["HORAFIM_AULA"]),
                        DtInicioAula = Convert.ToDateTime(reader["DTINICIOAULA"]),
                        DtFimAula = Convert.ToDateTime(reader["DTFIMAULA"])
                    };

                    horarios.Add(d);
                }

                return horarios;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool ExisteAulaDisciplinaHabilitadaProvisoriamentePor(decimal num_func, string agrupamento, string provisorio)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT Count(*)
                                            FROM   DBO.LY_AULA_DOCENTE
                                            WHERE  NUM_FUNC = @NUM_FUNC		                                            
                                                   AND DISCIPLINA IN (SELECT disciplina
							                                            FROM   LY_GRUPO_HABILITACAO_DISC GHD
								                                               JOIN LY_GRUPO_HABILITACAO_DOC GHDO
									                                             ON GHD.AGRUPAMENTO = GHDO.AGRUPAMENTO
							                                            WHERE NUM_FUNC = @NUM_FUNC  
								                                               AND GHDO.PROVISORIO = @PROVISORIO								                         
								                                               AND GHDO.AGRUPAMENTO = @AGRUPAMENTO) ";

                contextQuery.Parameters.Add("@NUM_FUNC", num_func);
                contextQuery.Parameters.Add("@AGRUPAMENTO", agrupamento);
                contextQuery.Parameters.Add("@PROVISORIO", provisorio);


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

        public void DesalocaAulas(DataContext ctx, decimal numFunc, DateTime dataFimAula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_AULA_DOCENTE 
                                            SET STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO,
                                                DATA_FIM = case when @DATAFIMAULA > DATA_INICIO then  @DATAFIMAULA else DATA_INICIO end
                                        WHERE  DATA_FIM >= CONVERT(DATE, @DATAFIMAULA) 
                                               AND DATA_FIM <> DATA_INICIO
                                               AND NUM_FUNC = @NUM_FUNC  ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
                contextQuery.Parameters.Add("@DATAFIMAULA", dataFimAula.Date);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

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

        public void DesalocaAulas(DataContext ctx, decimal numFunc, string turno, string faculdade, int diaSemana, int aula, string disciplina, string turma, decimal ano, decimal semestre, DateTime dataInicioAula, DateTime dataFimAula)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_AULA_DOCENTE 
                                            SET STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO,
                                                DATA_FIM = case when @DATAFIMAULA > DATA_INICIO then  @DATAFIMAULA else DATA_INICIO end
                                        WHERE  DATA_FIM >= CONVERT(DATE, @DATAFIMAULA) 
                                               AND DATA_FIM <> DATA_INICIO
                                               AND NUM_FUNC = @NUM_FUNC
											   AND TURNO = @TURNO
											   AND FACULDADE = @FACULDADE
                                               AND AULA = @AULA
											   AND DIA_SEMANA = @DIA_SEMANA
											   AND DISCIPLINA = @DISCIPLINA
											   AND TURMA = @TURMA
											   AND ANO = @ANO
											   AND SEMESTRE = @SEMESTRE
											   AND DATA_INICIO = @DATA_INICIO  ";

                contextQuery.Parameters.Add("@NUM_FUNC", numFunc);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@DIA_SEMANA", diaSemana);
                contextQuery.Parameters.Add("@AULA", aula);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@DATA_INICIO", dataInicioAula);
                contextQuery.Parameters.Add("@DATAFIMAULA", dataFimAula.Date);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

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

        public void SubstituiPorCarenciaPor(DataContext ctx, decimal numFuncAtual, DateTime dataFimAulaAtual)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT DISTINCT
	                                        CASE 
		                                    WHEN (AD.TIPO_DOCENTE = '00000000' AND TIPO_AULA = 'GLP') THEN 115451 --CARENCIA TEMPORARIA
			                                    ELSE 115460 END NUM_FUNC --CARENCIA REAL
                                            ,AD.TURNO
                                            ,AD.FACULDADE
                                            ,AD.DIA_SEMANA
                                            ,AD.AULA
                                            ,AD.DISCIPLINA
                                            ,AD.TURMA
                                            ,AD.ANO
                                            ,AD.SEMESTRE
                                            ,CASE 
		                                    WHEN AD.DATA_INICIO < @DATA_FIM_ATUAL THEN  @DATA_FIM_ATUAL
		                                    ELSE AD.DATA_INICIO END DATA_INICIO
                                            ,AD.DATA_FIM
                                            ,@STAMP_ATUALIZACAO AS STAMP_ATUALIZACAO
                                    INTO #CARENCIAS
                                    FROM LY_AULA_DOCENTE AD
	                                      LEFT JOIN LY_AULA_DOCENTE_TIPO T1
			                                    ON  T1.ANO = AD.ANO
					                                    AND T1.SEMESTRE = AD.SEMESTRE
					                                    AND T1.FACULDADE = AD.FACULDADE
					                                    AND T1.DIA_SEMANA = AD.DIA_SEMANA
					                                    AND T1.AULA = AD.AULA
					                                    AND T1.NUM_FUNC = AD.NUM_FUNC
					                                    AND T1.TURMA = AD.TURMA
					                                    AND T1.TURNO = AD.TURNO
					                                    AND T1.DISCIPLINA = AD.DISCIPLINA
					                                    AND T1.TIPO_AULA = 'GLP'
                                    WHERE  AD.DATA_FIM >= @DATA_FIM_ATUAL
	                                       AND AD.DATA_FIM <> AD.DATA_INICIO
	                                       AND AD.NUM_FUNC = @NUM_FUNC 

                                    INSERT INTO DBO.LY_AULA_DOCENTE
                                                (NUM_FUNC
                                                ,TURNO
                                                ,FACULDADE
		                                        ,DIA_SEMANA
                                                ,AULA
                                                ,DISCIPLINA
		                                        ,TURMA
                                                ,ANO
                                                ,SEMESTRE
		                                        ,DATA_INICIO
		                                        ,DATA_FIM
		                                        ,STAMP_ATUALIZACAO)
                                    SELECT NUM_FUNC
                                            ,TURNO
                                            ,FACULDADE
		                                    ,DIA_SEMANA
                                            ,AULA
                                            ,DISCIPLINA
		                                    ,TURMA
                                            ,ANO
                                            ,SEMESTRE
		                                    ,DATA_INICIO
		                                    ,DATA_FIM
		                                    ,STAMP_ATUALIZACAO 
                                    FROM #CARENCIAS C 
                                    WHERE NOT EXISTS ( SELECT TOP 1 1 
				                                    FROM LY_AULA_DOCENTE A2
				                                    WHERE NUM_FUNC = C.NUM_FUNC
							                                    AND A2.TURNO = C.TURNO
							                                    AND A2.FACULDADE = C.FACULDADE
							                                    AND A2.DIA_SEMANA = C.DIA_SEMANA
							                                    AND A2.AULA = C.AULA
							                                    AND A2.DISCIPLINA = C.DISCIPLINA
							                                    AND A2.TURMA = C.TURMA
							                                    AND A2.ANO = C.ANO
							                                    AND A2.SEMESTRE = C.SEMESTRE
							                                    AND A2.DATA_INICIO = C.DATA_INICIO)

                                    DROP TABLE #CARENCIAS ";

            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);
            contextQuery.Parameters.Add("@DATA_FIM_ATUAL", dataFimAulaAtual.Date);
            contextQuery.Parameters.Add("@NUM_FUNC", numFuncAtual);

            ctx.ApplyModifications(contextQuery);
        }

        public void Insere(DataContext contexto, Entidades.LyAulaDocente aulaDocente)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO dbo.LY_AULA_DOCENTE
                                           (NUM_FUNC
                                           ,TURNO
                                           ,FACULDADE
                                           ,DIA_SEMANA
                                           ,AULA
                                           ,DISCIPLINA
                                           ,TURMA
                                           ,ANO
                                           ,SEMESTRE
                                           ,TIPO
                                           ,DATA_INICIO
                                           ,DATA_FIM
                                           ,TIPO_DOCENTE
                                           ,STAMP_ATUALIZACAO)
                                     VALUES
                                           (@NUM_FUNC,
                                           @TURNO, 
                                           @FACULDADE, 
                                           @DIA_SEMANA,
                                           @AULA, 
                                           @DISCIPLINA,
                                           @TURMA, 
                                           @ANO, 
                                           @SEMESTRE, 
                                           @TIPO, 
                                           @DATA_INICIO, 
                                           @DATA_FIM, 
                                           @TIPO_DOCENTE,
                                           @STAMP_ATUALIZACAO) ";

            contextQuery.Parameters.Add("@NUM_FUNC", SqlDbType.Int, aulaDocente.NumFunc);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, aulaDocente.Turno);
            contextQuery.Parameters.Add("@FACULDADE", SqlDbType.VarChar, aulaDocente.Faculdade);
            contextQuery.Parameters.Add("@DIA_SEMANA", SqlDbType.Int, aulaDocente.DiaSemana);
            contextQuery.Parameters.Add("@AULA", SqlDbType.Int, aulaDocente.Aula);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, aulaDocente.Disciplina);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, aulaDocente.Turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, aulaDocente.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, aulaDocente.Semestre);
            contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, aulaDocente.Tipo);
            contextQuery.Parameters.Add("@DATA_INICIO", SqlDbType.DateTime, aulaDocente.DataInicio);
            contextQuery.Parameters.Add("@DATA_FIM", SqlDbType.DateTime, aulaDocente.DataFim);
            contextQuery.Parameters.Add("@TIPO_DOCENTE", SqlDbType.VarChar, aulaDocente.TipoDocente);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public bool ExisteAulaAlocadaPor(DataContext ctx, decimal num_func, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT Count(*)
                                            FROM   DBO.LY_AULA_DOCENTE
                                            WHERE  NUM_FUNC = @NUM_FUNC	
                                                   AND DATA_FIM <> DATA_INICIO	                                            
                                                   AND DATA_FIM >= CONVERT(DATE, @DATA) ";

            contextQuery.Parameters.Add("@NUM_FUNC", num_func);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;

        }

        public bool ExisteAulaAlocadaPeriodoLotacaoPor(decimal numFunc, DateTime dataNomeacao, DateTime dataDesativacao)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool existe = false;

            try
            {
                existe = this.ExisteAulaAlocadaPeriodoLotacaoPor(ctx, numFunc, dataNomeacao, dataDesativacao);

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

        /// <summary>
        /// Lista as Agrupamentos com carencia real ou glp real ou contrato real que o professor informado tenha habilitação 
        /// </summary>
        /// <param name="numFunc"></param>
        /// <returns></returns>
        public DataTable ObtemAgrupamentoCarenciaGlpContratoPor(int ano, string censo, int numFunc)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"RecursosHumanos.AGRUPAMENTOCARENCIAGLPCONTRATO";


                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@NUN_FUNC", SqlDbType.Int, numFunc);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                lista = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return lista;
        }

        /// <summary>
        /// Lista as turmas / Aulas com carencia real ou glp real ou contrato real que o professor informado tenha habilitação 
        /// </summary>
        /// <param name="numFunc"></param>
        /// <returns></returns>
        public DataTable ObtemTurmaCarenciaGlpContratoPor(int ano, string censo, int numFunc, string agrupamento)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"RecursosHumanos.TURMACARENCIAGLPCONTRATO";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@NUN_FUNC", SqlDbType.Int, numFunc);
                contextQuery.Parameters.Add("@AGRUPAMENTO", SqlDbType.VarChar, agrupamento);

                lista = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return lista;
        }

        public bool ExisteAulaAlocadaPeriodoLotacaoPor(DataContext ctx, decimal num_func, DateTime dataNomeacao, DateTime dataDesativacao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT Count(*)
                                            FROM   DBO.LY_AULA_DOCENTE
                                            WHERE  NUM_FUNC = @NUM_FUNC	
                                                   AND DATA_INICIO <> DATA_FIM	                                            
                                                   AND ( DATA_INICIO  BETWEEN @DATANOMEACAO AND @DATADESATIVACAO
                                                       OR DATA_FIM BETWEEN @DATANOMEACAO AND @DATADESATIVACAO )";

            contextQuery.Parameters.Add("@NUM_FUNC", num_func);
            contextQuery.Parameters.Add("@DATANOMEACAO", dataNomeacao);
            contextQuery.Parameters.Add("@DATADESATIVACAO", dataDesativacao);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool ExisteAulaAlocadaPeriodoLotacaoPor(DataContext ctx, decimal num_func, DateTime dataNomeacao)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT Count(*)
                                            FROM   DBO.LY_AULA_DOCENTE
                                            WHERE  NUM_FUNC = @NUM_FUNC	
                                                   AND DATA_INICIO <> DATA_FIM	                                            
                                                   AND (DATA_INICIO  >= @DATANOMEACAO
                                                       OR DATA_FIM >= @DATANOMEACAO )";

            contextQuery.Parameters.Add("@NUM_FUNC", num_func);
            contextQuery.Parameters.Add("@DATANOMEACAO", dataNomeacao);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public int ObtemTotalAulasAlocadasPor(decimal nunFunc, DateTime dataConsulta)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            int totalAulasAlocadas = 0;

            try
            {
                totalAulasAlocadas = this.ObtemTotalAulasAlocadasPor(ctx, nunFunc, dataConsulta);
                return totalAulasAlocadas;
            }

            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public int ObtemTotalAulasAlocadasPor(DataContext ctx, decimal nunFunc, DateTime dataConsulta)
        {
            ContextQuery contextQuery = new ContextQuery();
            int totalAulasAlocadas = 0;

            contextQuery.Command = @" SELECT  COUNT(*)
                    FROM    LY_AULA_DOCENTE AD (NOLOCK)
                            JOIN LY_TURMA T (NOLOCK) ON AD.ANO = T.ANO
                                               AND AD.SEMESTRE = T.SEMESTRE
                                               AND AD.DISCIPLINA = T.DISCIPLINA
                                               AND AD.FACULDADE = T.FACULDADE
                                               AND AD.TURMA = T.TURMA
                                               AND AD.TURNO = T.TURNO
                                               AND AD.DATA_FIM = T.DT_FIM
                    WHERE   AD.NUM_FUNC = @NUM_FUNC
                            AND T.SIT_TURMA = 'ABERTA'
                            AND @DATACONSULTA BETWEEN AD.DATA_INICIO
                                                   AND     AD.DATA_FIM  ";

            contextQuery.Parameters.Add("@NUM_FUNC", nunFunc);
            contextQuery.Parameters.Add("@DATACONSULTA", dataConsulta);

            totalAulasAlocadas = ctx.GetReturnValue<int>(contextQuery);

            return totalAulasAlocadas;
        }

        public bool ExisteAulaVigenteNoGrupoHabilitacaoDisciplinaPor(string agrupamento)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @"  SELECT Count(*) AS CONTAGEM
                                            FROM   LY_AULA_DOCENTE AD (NOLOCK)
                                                   INNER JOIN LY_TURMA T (NOLOCK)
                                                           ON T.TURMA = AD.TURMA
                                                              AND T.ANO = AD.ANO
                                                              AND T.SEMESTRE = AD.SEMESTRE
                                                              AND T.DISCIPLINA = AD.DISCIPLINA
                                                              AND T.DT_FIM = AD.DATA_FIM
                                                   INNER JOIN LY_GRUPO_HABILITACAO_DISC GHDISC (NOLOCK)
                                                           ON GHDISC.DISCIPLINA = ISNULL(T.DISCIPLINA_MULTIPLA,
                                                                                  T.DISCIPLINA)
                                            WHERE  GHDISC.AGRUPAMENTO = @AGRUPAMENTO
                                                   AND ( AD.DATA_FIM IS NULL
                                                          OR AD.DATA_FIM > Getdate() )
                                                   AND T.SIT_TURMA = 'Aberta'   ";


                contextQuery.Parameters.Add("@AGRUPAMENTO", agrupamento);


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

        public bool ExisteDocentesEmAulaAtivaPor(string turma, decimal ano, decimal periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {

                return this.ExisteDocentesEmAulaAtivaPor(ctx, turma, ano, periodo);
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

        public bool ExisteDocentesEmAulaAtivaPor(DataContext ctx, string turma, decimal ano, decimal periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT  COUNT(NUM_FUNC)
                                    FROM    DBO.LY_AULA_DOCENTE
                                    INNER JOIN LY_DISCIPLINA D ON D.DISCIPLINA = LY_AULA_DOCENTE.DISCIPLINA
                                   WHERE   TURMA = @TURMA
                                            AND ANO = @ANO
                                            AND SEMESTRE = @SEMESTRE
                                            AND DATA_INICIO <> DATA_FIM
                                            AND ( DATA_FIM IS NULL
			                                      OR DATA_FIM > GETDATE()
			                                    )  
                                            AND ISNULL(D.ELETIVA, 'N') = 'S'";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public string RetornaPrimeiroDocentesEmAulaPor(string turma, decimal ano, decimal periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.RetornaPrimeiroDocentesEmAulaPor(contexto, turma, ano, periodo);
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

        public string RetornaPrimeiroDocentesEmAulaPor(DataContext ctx, string turma, decimal ano, decimal periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            string docente;

            contextQuery.Command = @" SELECT TOP 1 P.NOME_COMPL
                                    FROM  DBO.LY_AULA_DOCENTE A
										INNER JOIN LY_DOCENTE D ON A.NUM_FUNC = D.NUM_FUNC
										INNER JOIN LY_PESSOA P ON D.PESSOA = P.PESSOA
                                    WHERE TURMA = @TURMA
                                            AND ANO = @ANO
                                            AND SEMESTRE = @SEMESTRE
											AND D.MATRICULA NOT IN('00000000','11111111','22222222','44444444','66666666','88888888','55555551','55555555','99999999')
                                            AND DATA_INICIO <> DATA_FIM
                                            AND ( DATA_FIM IS NULL
			                                      OR DATA_FIM > GETDATE()
			                                    )
										ORDER BY DATA_INICIO ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);

            docente = ctx.GetReturnValue<string>(contextQuery);

            return docente;
        }
        
        public string RetornaDocenteEmAulaPor(DataContext ctx, string turma, int ano, int periodo, string turno, string faculdade, string disciplina, int diaSemana, int aula)
        {
            ContextQuery contextQuery = new ContextQuery();
            string docente;

            contextQuery.Command = @" SELECT A.NUM_FUNC
                                    FROM  DBO.LY_AULA_DOCENTE A
                                    WHERE TURMA = @TURMA
                                            AND ANO = @ANO
                                            AND SEMESTRE = @SEMESTRE
                                            AND DATA_INICIO <> DATA_FIM
                                            AND ( DATA_FIM IS NULL
			                                      OR DATA_FIM >= convert(date,GETDATE())
			                                    )
                                           AND TURNO = @TURNO
										   AND FACULDADE = @FACULDADE
                                           AND AULA = @AULA
										   AND DIA_SEMANA = @DIA_SEMANA
										   AND DISCIPLINA = @DISCIPLINA
										ORDER BY DATA_INICIO DESC";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@FACULDADE", faculdade);
            contextQuery.Parameters.Add("@DIA_SEMANA", diaSemana);
            contextQuery.Parameters.Add("@AULA", aula);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

            docente = ctx.GetReturnValue<string>(contextQuery);

            return docente;
        }

        public bool ExisteDocentesEmAulaTurmaReferenciaAtivaPor(DataContext ctx, string turmaReferencia, decimal ano, decimal periodo, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT  COUNT(DISTINCT LAD.NUM_FUNC)
                                   FROM    DBO.LY_AULA_DOCENTE LAD
                                    JOIN	LY_TURMA LT (NOLOCK) ON LT.ANO = LAD.ANO
							                                    AND LT.SEMESTRE = LAD.SEMESTRE         
							                                    AND LT.TURMA = LAD.TURMA
							                                    AND LT.DISCIPLINA = LAD.DISCIPLINA
							                                    AND LT.DT_FIM = LAD.DATA_FIM
							                                    AND LT.SIT_TURMA = 'ABERTA'
							                                    AND LT.DT_FIM >= CONVERT(DATE, GETDATE())
                            --INNER JOIN LY_DISCIPLINA D ON D.DISCIPLINA = LT.DISCIPLINA
                                    WHERE  TURMAREFERENCIA = @TURMAREFERENCIA
                                            AND LAD.ANO = @ANO
                                            AND LAD.SEMESTRE = @SEMESTRE
                                            AND DATA_INICIO <> DATA_FIM
                                            AND LAD.DISCIPLINA = @DISCIPLINA 
                                            ";

            contextQuery.Parameters.Add("@TURMAREFERENCIA", turmaReferencia);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool ExisteDocentesEmAulaAtivaPor(DataContext ctx, string turma, decimal ano, decimal periodo, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT  COUNT(NUM_FUNC)
                                    FROM    DBO.LY_AULA_DOCENTE
                                    INNER JOIN LY_DISCIPLINA D ON D.DISCIPLINA = LY_AULA_DOCENTE.DISCIPLINA
                                   WHERE   TURMA = @TURMA
                                            AND ANO = @ANO
                                            AND SEMESTRE = @SEMESTRE
                                            AND DATA_INICIO <> DATA_FIM
                                            AND ( DATA_FIM IS NULL
			                                      OR DATA_FIM >= convert(date,GETDATE())
			                                    )  
                                            AND ISNULL(D.ELETIVA, 'N') = 'S'
                                            AND LY_AULA_DOCENTE.DISCIPLINA = @DISCIPLINA ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool ExisteAulaVigentePor(string faculdade, string turno, string curso, string curriculo, decimal serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                                FROM    LY_AULA_DOCENTE AD 
                                        INNER JOIN LY_TURMA T WITH ( NOLOCK ) ON T.ANO = AD.ANO
                                                                                 AND T.SEMESTRE = AD.SEMESTRE
                                                                                 AND T.DISCIPLINA = AD.DISCIPLINA
                                                                                 AND T.TURMA = AD.TURMA
                                                                                 AND T.DT_FIM = AD.DATA_FIM
                                WHERE   T.SIT_TURMA = 'ABERTA'
                                        AND T.FACULDADE = @FACULDADE
                                        AND T.TURNO = @TURNO
                                        AND T.CURSO = @CURSO
                                        AND T.CURRICULO = @CURRICULO
                                        AND T.SERIE = @SERIE ";

                contextQuery.Parameters.Add("@FACULDADE", faculdade);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);

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

        public DataTable ListaTurmaCarenciaContratoGLPPor(string unidadeEnsino, string agrupamentoDisciplina)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT T.TURMA, 
                                                T.DISCIPLINA, 
                                                D.NOME   AS NOMEDISCIPLINA, 
                                                GHDISC.AGRUPAMENTO, 
                                                GH.DESCRICAO, 
                                                T.ANO, 
                                                T.SEMESTRE, 
				                                AD.NUM_FUNC, 
                                                COUNT(*) AS CONTAGEMCARENCIAS,
				                                CASE 
					                                WHEN AD.NUM_FUNC = 115451 then 'Carência Temporaria'
					                                when AD.NUM_FUNC = 115460 then 'Carência Real'
				                                END TIPOCARENCIA
                                FROM   LY_AULA_DOCENTE AD (NOLOCK) 
                                       INNER JOIN LY_TURMA T (NOLOCK) 
                                               ON T.TURMA = AD.TURMA 
                                                  AND T.ANO = AD.ANO 
                                                  AND T.SEMESTRE = AD.SEMESTRE 
                                                  AND T.DISCIPLINA = AD.DISCIPLINA 
                                                  AND T.DT_FIM = AD.DATA_FIM 
                                       INNER JOIN LY_GRUPO_HABILITACAO_DISC GHDISC (NOLOCK) 
                                               ON GHDISC.DISCIPLINA = ISNULL(T.DISCIPLINA_MULTIPLA, 
                                                                      T.DISCIPLINA) 
                                       INNER JOIN LY_DISCIPLINA D (NOLOCK) 
                                               ON T.DISCIPLINA = D.DISCIPLINA 
                                       INNER JOIN LY_GRUPO_HABILITACAO GH (NOLOCK) 
                                               ON GH.AGRUPAMENTO = GHDISC.AGRUPAMENTO 
                                WHERE  GHDISC.AGRUPAMENTO = @AGRUPAMENTO  
                                       AND T.FACULDADE = @CENSO  
                                       AND AD.NUM_FUNC IN ( 115460, 115451 ) 
                                       AND CONVERT(DATE, GETDATE()) <= CONVERT(DATE, T.DT_FIM) 
                                       AND T.SIT_TURMA = 'ABERTA' 
                                GROUP  BY T.TURMA, 
                                          T.DISCIPLINA, 
                                          D.NOME, 
                                          GHDISC.AGRUPAMENTO, 
                                          GH.DESCRICAO, 
                                          T.ANO, 
                                          T.SEMESTRE, 
		                                  AD.NUM_FUNC ";

                contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, unidadeEnsino);
                contextQuery.Parameters.Add("@AGRUPAMENTO", TechneDbType.T_ALFAMEDIUM, agrupamentoDisciplina);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public bool ExisteAulaAlocadaMigracaoPor(DataContext ctx, decimal num_func, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT Count(*)
                                            FROM   DBO.LY_AULA_DOCENTE
                                            WHERE  NUM_FUNC = @NUM_FUNC	
                                                   AND DATA_FIM <> DATA_INICIO	                                            
                                                   AND DATA_FIM > CONVERT(DATE, @DATA) ";
           
            contextQuery.Parameters.Add("@NUM_FUNC", num_func);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;

        }

        public int ObtemQuantidadeAulasGLPsAtivasDocentePor(DataContext ctx, string matricula)
        {
            int quantidadeAulas = 0;
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" select DBO.f_TotalAulaGLP (@MATRICULA) CONTADOR ";
                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    quantidadeAulas = Convert.ToInt32(reader["CONTADOR"]);
                }

                return quantidadeAulas;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public List<string> ObtemDocentesEmAulaPor(int ano, int periodo, string turma,string disciplina )
        {
            List<string> listaDocentes = new List<string>();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            string docente = string.Empty;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT DISTINCT
                              (  CONVERT(VARCHAR,P.IDFUNCIONAL) + '/' +  CONVERT(VARCHAR,D.VINCULO) + ' - ' + P.NOME_COMPL ) AS NOMEPROFESSOR
                        FROM    LY_AULA_DOCENTE AD
                        INNER JOIN LY_TURMA T WITH ( NOLOCK ) ON T.ANO = AD.ANO
                                                                                 AND T.SEMESTRE = AD.SEMESTRE
                                                                                 AND T.DISCIPLINA = AD.DISCIPLINA
                                                                                 AND T.TURMA = AD.TURMA
                                                                                 AND T.DT_FIM = AD.DATA_FIM
                        INNER JOIN LY_DOCENTE D ON AD.NUM_FUNC = D.NUM_FUNC
                        INNER JOIN LY_PESSOA P ON P.PESSOA = D.PESSOA
                        WHERE   AD.DISCIPLINA = @DISCIPLINA
                                AND AD.TURMA = @TURMA
                                AND AD.ANO = @ANO
                                AND AD.SEMESTRE = @SEMESTRE 
                                AND DATA_INICIO <> DATA_FIM
                                            AND ( DATA_FIM IS NULL
			                                      OR DATA_FIM > GETDATE()
			                                    )
                        ")
                };

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);


                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["NOMEPROFESSOR"] != DBNull.Value)
                    {
                        docente = reader["NOMEPROFESSOR"].ToString();
                        listaDocentes.Add(docente);
                    }
                }

                return listaDocentes;
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
