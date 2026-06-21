namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;
    using System.Data.SqlClient;
    using Techne.Lyceum.CR;
    using Techne.Data;

    public class HistMatricula : RNBase
    {
        public static bool ExisteHistorico(string aluno, string disciplina, string turma, decimal ano, decimal semestre)
        {
            var retorno = ExecutarFuncao<int?>(
                new ContextQuery(
                    @"SELECT TOP 1
                            1
                    FROM    LY_HISTMATRICULA
                    WHERE   ALUNO = @ALUNO
                            AND DISCIPLINA = @DISCIPLINA
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE",
                    new ContextQueryParameter("@ALUNO", TechneDbType.T_CODIGO, aluno),
                    new ContextQueryParameter("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina),
                    new ContextQueryParameter("@TURMA", turma),
                    new ContextQueryParameter("@ANO", TechneDbType.T_ANO, ano),
                    new ContextQueryParameter("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre)));

            return retorno != null;
        }

        public bool EhMatriculaHistoricoAtiva(string aluno, int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existeMatriculaAtiva = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    DBO.LY_HISTMATRICULA (NOLOCK)
                        WHERE   ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND SEMESTRE = @PERIODO
                                AND SITUACAO_HIST <> 'Cancelado'
                                AND SITUACAO_HIST <> 'Dispensado'
                                AND SITUACAO_HIST <> 'Inconcluido'  ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existeMatriculaAtiva = true;
                }

                return existeMatriculaAtiva;
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

        public bool EhMatriculaHistoricoAtivaPor(DataContext ctx, string aluno, int ano, int periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    DBO.LY_HISTMATRICULA (NOLOCK)
                        WHERE   ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
								AND TURMA = @TURMA
                                AND SITUACAO_HIST <> 'Cancelado'
                                AND SITUACAO_HIST <> 'Dispensado'
                                AND SITUACAO_HIST <> 'Inconcluido'   ";

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhMatriculaHistoricoAtivaPor(string aluno, int ano, int periodo, string turma, string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existeMatriculaAtiva = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    DBO.LY_HISTMATRICULA (NOLOCK)
                        WHERE   ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND SEMESTRE = @PERIODO
                                AND TURMA = @TURMA
                                AND DISCIPLINA = @DISCIPLINA
                                AND SITUACAO_HIST <> 'Cancelado'
                                AND SITUACAO_HIST <> 'Dispensado'
                                AND SITUACAO_HIST <> 'Inconcluido'  ";

                contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);
                contextQuery.Parameters.Add("@DISCIPLINA", TechneDbType.T_CODIGO, disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    existeMatriculaAtiva = true;
                }

                return existeMatriculaAtiva;
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


        public bool ExisteHistoricoPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                            FROM    dbo.LY_HISTMATRICULA
                            WHERE   DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

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

        public static DataTable Listar(string aluno, int ano, int semestre, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            try
            {


                var contextQuery = new ContextQuery(
                    @"SELECT  ly_histmatricula.aluno,
                    ly_histmatricula.ordem,
                    ly_histmatricula.ano,
                    ly_histmatricula.semestre,
                    ly_histmatricula.disciplina,
                    ly_histmatricula.turma,
                    REPLACE(ly_histmatricula.nota_final,',','.') nota_final,
                    ly_histmatricula.situacao_hist situacao_hist,
                    ly_histmatricula.perc_presenca,
                    ly_histmatricula.horas_aula,
                    ly_histmatricula.creditos,
                    ly_histmatricula.nome_disc_orig,
                    CONVERT(DECIMAL(12,0),ly_histmatricula.aulas_dadas) AS aulas_dadas,
                    ly_histmatricula.observacao,
                    ly_histmatricula.num_func,
                    ly_histmatricula.lanc_deb,
                    ly_histmatricula.aulas_previstas,
                    ly_histmatricula.num_chamada,
                    ly_histmatricula.nivel_presenca,
                    ly_histmatricula.serie,
                    ly_histmatricula.grupo_eletiva,
                    ly_histmatricula.subturma1,
                    ly_histmatricula.subturma2,
                    ly_histmatricula.sit_detalhe,
                    ly_histmatricula.dt_inicio,
                    ly_histmatricula.dt_fim,
                    ly_histmatricula.cobranca_sep,
                    ly_histmatricula.dt_ultalt,
                    ly_histmatricula.dt_matricula,
                    ly_histmatricula.nota_final_num,
                    ly_histmatricula.area_conhecimento,
                    ly_histmatricula.tipo_aprovacao,
                    ly_disciplina.nome nomedisciplina,
                    ly_histmatricula.unidade_ensino,
                    ly_histmatricula.outras,
                    ly_histmatricula.falta_final,
                    ly_histmatricula.aulas_dadas,
                    ly_unidade_ensino.nome_comp nome_comp03,
                     CASE WHEN LY_HISTMATRICULA.DEPENDENCIA = 'S'
                              AND LY_HISTMATRICULA.OPTATIVAREFORCO = 'N'
                         THEN 'Dependência'
                         WHEN LY_HISTMATRICULA.DEPENDENCIA = 'S'
                              AND LY_HISTMATRICULA.OPTATIVAREFORCO <> 'N'
                         THEN 'Dependência/Optativa/Reforço'
                         WHEN (ISNULL(LY_HISTMATRICULA.DEPENDENCIA,'N') = 'N')
                              AND LY_HISTMATRICULA.OPTATIVAREFORCO <> 'N'
                         THEN 'Optativa/Reforço'
                         ELSE NULL
                    END DEPENDENCIA,
                    dr.nome as DISCIPLINA_REFERENCIA, 
                    ly_histmatricula.SERIE_REFERENCIA
                FROM    ly_histmatricula
                        INNER JOIN ly_disciplina ON ly_disciplina.disciplina = ly_histmatricula.disciplina
                        left JOIN ly_instituicao ON ly_instituicao.outra_faculdade = ly_histmatricula.UNIDADE_ENSINO
                        left JOIN ly_unidade_ensino ON ly_unidade_ensino.UNIDADE_ENS = LY_HISTMATRICULA.UNIDADE_ENSINO
                        left JOIN ly_disciplina dr ON LY_HISTMATRICULA.DEPENDENCIA = 'S' and dr.disciplina = ly_histmatricula.DISCIPLINA_REFERENCIA
                WHERE   ly_histmatricula.ALUNO = @ALUNO
                        AND ANO = @ANO
                        AND SEMESTRE = @SEMESTRE
                        AND TURMA = @TURMA ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);

                return Consultar(contextQuery);
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

        public static DataTable ListarTurmasHistoricoPor(string aluno, int ano, int semestre)
        {
            var contextQuery = new ContextQuery(
                @"SELECT  distinct LHM.TURMA
                FROM    [LYCEUM].[DBO].[LY_HISTMATRICULA] LHM
                WHERE   ALUNO = @ALUNO
                        AND ANO = @ANO
                        AND SEMESTRE = @SEMESTRE");

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);

            return Consultar(contextQuery);
        }

        public DataTable ListaDisciplinasPor(string aluno, int ano, int periodo, string turma)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT H.DISCIPLINA, 
                                        D.NOME AS DISCIPLINANOME, 
                                        H.SITUACAO_HIST, 
                                        H.PERC_PRESENCA 
                        FROM   LY_HISTMATRICULA H (NOLOCK) 
                               INNER JOIN LY_DISCIPLINA D (NOLOCK) 
                                       ON H.DISCIPLINA = D.DISCIPLINA 
                        WHERE  ALUNO = @ALUNO 
                               AND TURMA = @TURMA
                               AND ANO = @ANO 
                               AND SEMESTRE = @SEMESTRE 
                               AND SITUACAO_HIST <> 'Cancelado'
                        ORDER BY DISCIPLINANOME ";

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);

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

        public static DataTable ListarProgressaoParcial(LyMatricula matricula)
        {
            var contextQuery = new ContextQuery(
                @" SELECT  m.DISCIPLINA_REFERENCIA, m.DISCIPLINA, d.NOME_COMPL AS NOME_DISCIPLINA, m.SITUACAO_HIST AS 'SITUACAO'
                            FROM    DBO.LY_HISTMATRICULA m
                                    INNER JOIN dbo.LY_DISCIPLINA d ON m.DISCIPLINA_REFERENCIA = d.DISCIPLINA
                            WHERE   DEPENDENCIA = 'S'                            
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE
                            AND ALUNO = @ALUNO
                            AND TURMA = @TURMA");
            contextQuery.Parameters.Add("@ANO", matricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", matricula.Semestre);
            contextQuery.Parameters.Add("@ALUNO", matricula.Aluno);
            contextQuery.Parameters.Add("@TURMA", matricula.Turma);

            return Consultar(contextQuery);
        }

        public static string VerificaSituacaoFinal(string aluno, int ano, int semestre, string turma)
        {
            decimal frequencia = VerificaFrequenciaGlobal(aluno, ano, semestre, turma);
            int totalDisciplina = RetornaTotalDisciplinas(aluno, ano, semestre, turma);
            int totalReprovados = RetornaTotalReprovados(aluno, ano, semestre, turma);
            int totalReprovadosPorNota = RetornaTotalReprovadosPorNota(aluno, ano, semestre, turma);
            int totalAprovados = RetornaTotalAprovados(aluno, ano, semestre, turma);
            string situacaoFinal = null;
            int totalDep = 0;
            RN.HistMatricula rnHistMatricula = new HistMatricula();

            if (ano == 2025)
            {
                int valor = rnHistMatricula.ObtemDependenciasPermitidasPor(turma, ano, semestre);

                if (valor > 0)
                {
                    totalDep = valor;
                }
                else
                {
                    totalDep = 2;
                }
            }
            else
            {
                totalDep = 2;
            }



            if (totalDisciplina != 0)
            {
                if (totalAprovados == totalDisciplina)
                {
                    situacaoFinal = "Aprovado";
                }
                else
                {
                    if (frequencia >= 75)
                    {
                        if (totalReprovados > 0)
                        {
                            if (totalReprovados <= totalDep)
                            {
                                situacaoFinal = "Aprovado Com Dep";
                            }
                            else
                            {
                                if (totalReprovadosPorNota > totalDep)
                                    situacaoFinal = "Rep Nota";
                                else
                                    situacaoFinal = "Reprovado";
                            }
                        }
                        //if(String.IsNullOrEmpty(situacaoFinal) && totalReprovados > 2)
                        //    situacaoFinal = "Reprovado";
                    }
                    else
                    {
                        situacaoFinal = "Rep Freq";
                    }
                }
            }
            else
            {
                situacaoFinal = "Sem Disciplina";
            }

            return situacaoFinal;
        }

        public static int RetornaTotalDisciplinas(string aluno, int ano, int semestre, string turma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT COUNT(*)		
                        FROM LY_HISTMATRICULA LHM
							INNER JOIN LY_TURMA T
											ON T.TURMA = LHM.TURMA
											AND T.ANO = LHM.ANO
											AND T.SEMESTRE = LHM.SEMESTRE
											AND T.DISCIPLINA = LHM.DISCIPLINA
                             LEFT JOIN LY_GRADE_SERIE GS ON LHM.ANO = GS.ANO
                                                        AND LHM.SEMESTRE = GS.SEMESTRE
                                                        AND LHM.TURMA = GS.GRADE
                             LEFT JOIN DBO.LY_GRADE G ON GS.CURRICULO = G.CURRICULO
                                                        AND GS.CURSO = G.CURSO
                                                        AND LHM.DISCIPLINA = G.DISCIPLINA
                                                        AND GS.TURNO = G.TURNO
                                                        AND G.SERIE_IDEAL = GS.SERIE
                       WHERE LHM.ALUNO = @ALUNO
                         AND LHM.ANO = @ANO
                         AND LHM.SEMESTRE = @SEMESTRE
                         AND LHM.TURMA = @TURMA
                         AND UPPER(LHM.SITUACAO_HIST) <> 'CANCELADO'
                         AND ( G.OBRIGATORIA = 'S' OR G.DISCIPLINA IS NULL )
                         AND ( LHM.DEPENDENCIA = 'N' OR LHM.DEPENDENCIA IS NULL)
						 AND ( T.ELETIVA = 'N' OR T.ELETIVA IS NULL) ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);

                return ctx.GetReturnValue<int>(contextQuery);
            }
        }

        public static int RetornaTotalReprovados(string aluno, int ano, int semestre, string turma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  COUNT(*) AS REPROVADOS
                    FROM    LY_HISTMATRICULA H
							INNER JOIN LY_TURMA T
											ON T.TURMA = H.TURMA
											AND T.ANO = H.ANO
											AND T.SEMESTRE = H.SEMESTRE
											AND T.DISCIPLINA = H.DISCIPLINA
                            LEFT JOIN LY_GRADE_SERIE GS ON H.ANO = GS.ANO
                                                            AND H.SEMESTRE = GS.SEMESTRE
                                                            AND H.TURMA = GS.GRADE
                            LEFT JOIN DBO.LY_GRADE G ON GS.CURRICULO = G.CURRICULO
                                                         AND GS.CURSO = G.CURSO
                                                         AND H.DISCIPLINA = G.DISCIPLINA
                                                         AND GS.TURNO = G.TURNO
                                                         AND G.SERIE_IDEAL = GS.SERIE
                    WHERE   h.ALUNO = @ALUNO
                            AND h.ANO = @ANO
                            AND h.SEMESTRE = @SEMESTRE
                            AND h.TURMA = @TURMA
                            AND UPPER(h.SITUACAO_HIST) IN ( 'REP NOTA', 'REP FREQ' ) 
                            AND ( G.OBRIGATORIA = 'S' OR G.DISCIPLINA IS NULL )
                            AND ( H.DEPENDENCIA = 'N' OR H.DEPENDENCIA IS NULL)
							AND ( T.ELETIVA = 'N' OR T.ELETIVA IS NULL) ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);

                return ctx.GetReturnValue<int>(contextQuery);
            }
        }

        private static int RetornaTotalReprovadosPorNota(string aluno, int ano, int semestre, string turma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  COUNT(*) AS REPROVADOS
                    FROM    LY_HISTMATRICULA H
							INNER JOIN LY_TURMA T
											ON T.TURMA = H.TURMA
											AND T.ANO = H.ANO
											AND T.SEMESTRE = H.SEMESTRE
											AND T.DISCIPLINA = H.DISCIPLINA
                            LEFT JOIN LY_GRADE_SERIE GS ON H.ANO = GS.ANO
                                                            AND H.SEMESTRE = GS.SEMESTRE
                                                            AND H.TURMA = GS.GRADE
                            LEFT JOIN DBO.LY_GRADE G ON GS.CURRICULO = G.CURRICULO
                                                         AND GS.CURSO = G.CURSO
                                                         AND H.DISCIPLINA = G.DISCIPLINA
                                                         AND GS.TURNO = G.TURNO
                                                         AND G.SERIE_IDEAL = GS.SERIE
                    WHERE   h.ALUNO = @ALUNO
                            AND h.ANO = @ANO
                            AND h.SEMESTRE = @SEMESTRE
                            AND h.TURMA = @TURMA
                            AND ( G.OBRIGATORIA = 'S' OR G.DISCIPLINA IS NULL ) 
                            AND UPPER(h.SITUACAO_HIST) = 'REP NOTA'
                            AND ( H.DEPENDENCIA = 'N' OR H.DEPENDENCIA IS NULL)
							AND ( T.ELETIVA = 'N' OR T.ELETIVA IS NULL)  ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);

                return ctx.GetReturnValue<int>(contextQuery);
            }
        }

        public static int RetornaTotalAprovados(string aluno, int ano, int semestre, string turma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  COUNT(*) AS APROVADOS
                        FROM    LY_HISTMATRICULA H
								INNER JOIN LY_TURMA T
											ON T.TURMA = H.TURMA
											AND T.ANO = H.ANO
											AND T.SEMESTRE = H.SEMESTRE
											AND T.DISCIPLINA = H.DISCIPLINA
                                LEFT JOIN LY_GRADE_SERIE GS ON H.ANO = GS.ANO
                                                                AND H.SEMESTRE = GS.SEMESTRE
                                                                AND H.TURMA = GS.GRADE
                                LEFT JOIN DBO.LY_GRADE G ON GS.CURRICULO = G.CURRICULO
                                                             AND GS.CURSO = G.CURSO
                                                             AND H.DISCIPLINA = G.DISCIPLINA
                                                             AND GS.TURNO = G.TURNO
                                                             AND G.SERIE_IDEAL = GS.SERIE
                        WHERE   h.ALUNO = @ALUNO
                                AND h.ANO = @ANO
                                AND h.SEMESTRE = @SEMESTRE
                                AND h.TURMA = @TURMA
                                AND h.SITUACAO_HIST IN ( 'Aprovado', 'Aprovado Conselho','Promovido') 
                                AND ( G.OBRIGATORIA = 'S' OR G.DISCIPLINA IS NULL )
                                AND ( H.DEPENDENCIA = 'N' OR H.DEPENDENCIA IS NULL)
								AND ( T.ELETIVA = 'N' OR T.ELETIVA IS NULL) ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);

                return ctx.GetReturnValue<int>(contextQuery);
            }
        }

        public static decimal VerificaFrequenciaGlobal(string aluno, int ano, int semestre, string turma)
        {
            decimal totalFaltas = 0;
            decimal totalAulas = 0;
            decimal frequenciaGlobal = 0;

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {

                var contextQuery = new ContextQuery(
                    @"SELECT  ISNULL(SUM(h.FALTA_FINAL), 0) AS TOTAL_FALTA_FINAL,
                        ISNULL(SUM(h.AULAS_DADAS), 0) AS TOTAL_AULAS_DADAS
		                FROM    LY_HISTMATRICULA H
								INNER JOIN LY_TURMA T
											ON T.TURMA = H.TURMA
											AND T.ANO = H.ANO
											AND T.SEMESTRE = H.SEMESTRE
											AND T.DISCIPLINA = H.DISCIPLINA
				                LEFT JOIN LY_GRADE_SERIE GS ON H.ANO = GS.ANO
												                AND H.SEMESTRE = GS.SEMESTRE
												                AND H.TURMA = GS.GRADE
				                LEFT JOIN DBO.LY_GRADE G ON GS.CURRICULO = G.CURRICULO
											                 AND GS.CURSO = G.CURSO
											                 AND H.DISCIPLINA = G.DISCIPLINA
											                 AND GS.TURNO = G.TURNO
											                 AND G.SERIE_IDEAL = GS.SERIE
                        WHERE   h.ALUNO = @ALUNO
                                AND h.ANO = @ANO
                                AND h.SEMESTRE = @SEMESTRE
                                AND h.TURMA = @TURMA
                                AND h.SITUACAO_HIST IN ( 'APROVADO', 'REP NOTA', 'REP FREQ','Promovido','Retido' )
                                AND ( G.OBRIGATORIA = 'S' OR G.DISCIPLINA IS NULL )
                                AND ( H.DEPENDENCIA = 'N' OR H.DEPENDENCIA IS NULL)
								AND ( T.ELETIVA = 'N' OR T.ELETIVA IS NULL) ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURMA", turma);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        totalFaltas = Convert.ToDecimal(reader["TOTAL_FALTA_FINAL"]);
                        totalAulas = Convert.ToDecimal(reader["TOTAL_AULAS_DADAS"]);
                    }
                }
            }

            if (totalAulas != 0)
            {
                frequenciaGlobal = 100 - ((100 * totalFaltas) / totalAulas);
            }

            return frequenciaGlobal;
        }

        private static decimal GerarOrdem(decimal ano, decimal periodo, string turma, string aluno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT ISNULL(MAX(ORDEM), 0)
                    FROM    LY_HISTMATRICULA
                    WHERE   ALUNO = @ALUNO
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE
                            AND TURMA = @TURMA ");

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);
                contextQuery.Parameters.Add("@TURMA", turma);

                var ordem = ctx.GetReturnValue<decimal>(contextQuery);

                if (ordem == 0)
                {
                    contextQuery = new ContextQuery(
                   @" SELECT  ISNULL(MAX(ORDEM), 0) +1
                            FROM    LY_HISTMATRICULA
                            WHERE   ALUNO = @ALUNO
                                    AND ANO = @ANO
                                    AND SEMESTRE = @SEMESTRE ");

                    contextQuery.Parameters.Add("@ALUNO", aluno);
                    contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                    contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);

                    ordem = ctx.GetReturnValue<decimal>(contextQuery);
                }

                return ordem;
            }
        }

        public static void Inserir(LyHistMatricula histmatricula)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    histmatricula.Ordem = GerarOrdem(histmatricula.Ano, histmatricula.Semestre, histmatricula.TurmaHist, histmatricula.Aluno);

                    Inserir(histmatricula, context);

                    var situacaoFinalAluno = new TceSituacaoFinalAluno
                    {
                        Aluno = histmatricula.Aluno,
                        Ano = histmatricula.Ano,
                        Periodo = histmatricula.Semestre,
                        Turma = histmatricula.Turma,
                        SituacaoFinal = VerificaSituacaoFinal(histmatricula.Aluno, Convert.ToInt32(histmatricula.Ano), Convert.ToInt32(histmatricula.Semestre), histmatricula.Turma),
                        FrequenciaGlobal = VerificaFrequenciaGlobal(histmatricula.Aluno, Convert.ToInt32(histmatricula.Ano), Convert.ToInt32(histmatricula.Semestre), histmatricula.Turma),
                        Matricula = histmatricula.Matricula
                    };

                    //SituacaoFinalAluno.Remover(situacaoFinalAluno, context);

                    //if (!string.IsNullOrEmpty(situacaoFinalAluno.SituacaoFinal))
                    //{
                    //    SituacaoFinalAluno.Inserir(situacaoFinalAluno, context);
                    //}

                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public void InserePorMatriculaParaFechamento(DataContext ctx, LyHistMatricula histMatricula)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" INSERT INTO dbo.LY_HISTMATRICULA
                        ( ALUNO ,
                          ORDEM ,
                          ANO ,
                          SEMESTRE ,
                          DISCIPLINA ,
                          TURMA ,
                          NOTA_FINAL ,
                          SITUACAO_HIST ,
                          PERC_PRESENCA ,
                          HORAS_AULA ,
                          CREDITOS ,
                          AULAS_DADAS ,
                          OBSERVACAO ,
                          AULAS_PREVISTAS ,
                          NUM_CHAMADA ,
                          NIVEL_PRESENCA ,
                          SERIE ,
                          GRUPO_ELETIVA ,
                          SUBTURMA1 ,
                          SUBTURMA2 ,
                          SIT_DETALHE ,
                          DT_INICIO ,
                          DT_FIM ,
                          COBRANCA_SEP ,
                          DT_ULTALT ,
                          DT_MATRICULA ,
                          TIPO_APROVACAO ,
                          UNIDADE_ENSINO ,
                          MATRICULA ,
                          DEPENDENCIA ,
                          SERIE_REFERENCIA ,
                          DISCIPLINA_REFERENCIA ,
                          FALTA_FINAL ,
                          CONCOMITANTE ,
                          EDUC_ESPECIAL ,
                          MAIS_EDUCACAO ,
                          OPTATIVAREFORCO
                        )
                        SELECT  M.ALUNO ,
                                @ORDEM ,
                                M.ANO ,
                                M.SEMESTRE ,
                                M.DISCIPLINA ,
                                M.TURMA ,
                                @NOTA_FINAL ,
                                @SITUACAO_HIST ,
                                ISNULL(CONVERT(DECIMAL(10, 2), 100 - ( ( @FALTA_FINAL
                                                              * 100 )
                                                              / CASE
                                                              WHEN @AULAS_DADAS = 0
                                                              THEN 1
                                                              ELSE @AULAS_DADAS
                                                              END )), 0) ,
                                @HORAS_AULA ,
                                @CREDITOS ,
                                @AULAS_DADAS ,
                                M.OBS ,
                                @AULAS_PREVISTAS ,
                                M.NUM_CHAMADA ,
                                @NIVEL_PRESENCA ,
                                @SERIE ,
                                M.GRUPO_ELETIVA ,
                                M.SUBTURMA1 ,
                                M.SUBTURMA2 ,
                                M.SIT_DETALHE ,
                                @DT_INICIO ,
                                @DT_FIM ,
                                M.COBRANCA_SEP ,
                                M.DT_ULTALT ,
                                M.DT_MATRICULA ,
                                M.TIPO_APROVACAO ,
                                @UNIDADE_ENSINO ,
                                @MATRICULA ,
                                ISNULL(M.DEPENDENCIA, 'N') ,
                                M.SERIE_REFERENCIA ,
                                M.DISCIPLINA_REFERENCIA ,
                                @FALTA_FINAL ,
                                ISNULL(M.CONCOMITANTE, 'N') ,
                                ISNULL(M.EDUC_ESPECIAL, 'N') ,
                                ISNULL(M.MAIS_EDUCACAO, 'N') ,
                                @OPTATIVAREFORCO
                        FROM    DBO.LY_MATRICULA M
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE 
                                AND SIT_MATRICULA = 'Matriculado' ";

                contextQuery.Parameters.Add("@ALUNO", histMatricula.Aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", histMatricula.Disciplina);
                contextQuery.Parameters.Add("@TURMA", histMatricula.Turma);
                contextQuery.Parameters.Add("@ANO", histMatricula.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", histMatricula.Semestre);
                contextQuery.Parameters.Add("@ORDEM", histMatricula.Ordem);
                contextQuery.Parameters.Add("@NOTA_FINAL", histMatricula.NotaFinal);
                contextQuery.Parameters.Add("@SITUACAO_HIST", histMatricula.SituacaoHist);
                contextQuery.Parameters.Add("@HORAS_AULA", histMatricula.HorasAula);
                contextQuery.Parameters.Add("@AULAS_DADAS", histMatricula.AulasDadas);
                contextQuery.Parameters.Add("@AULAS_PREVISTAS", histMatricula.AulasPrevistas);
                contextQuery.Parameters.Add("@NIVEL_PRESENCA", histMatricula.NivelPresenca);
                contextQuery.Parameters.Add("@SERIE", histMatricula.Serie);
                contextQuery.Parameters.Add("@DT_INICIO", histMatricula.DtInicio);
                contextQuery.Parameters.Add("@DT_FIM", histMatricula.DtFim);
                contextQuery.Parameters.Add("@UNIDADE_ENSINO", histMatricula.UnidadeEnsino);
                contextQuery.Parameters.Add("@MATRICULA", histMatricula.Matricula);
                contextQuery.Parameters.Add("@FALTA_FINAL", histMatricula.FaltaFinal);
                contextQuery.Parameters.Add("@OPTATIVAREFORCO", histMatricula.OptativaReforco);
                contextQuery.Parameters.Add("@CREDITOS", histMatricula.Creditos);

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

        public void AlteraPorMatriculaParaFechamento(DataContext ctx, LyHistMatricula histMatricula)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" UPDATE  DBO.LY_HISTMATRICULA
                        SET     TURMA = @TURMA ,
                                NOTA_FINAL = @NOTA_FINAL ,
                                SITUACAO_HIST = @SITUACAO_HIST ,
                                HORAS_AULA = @HORAS_AULA ,
                                AULAS_DADAS = @AULAS_DADAS ,
                                AULAS_PREVISTAS = @AULAS_PREVISTAS ,
                                NIVEL_PRESENCA = @NIVEL_PRESENCA ,
                                PERC_PRESENCA = ISNULL(CONVERT(DECIMAL(10, 2), 100 - ( ( FALTA_FINAL
                                                                                              * 100 )
                                                                                              / CASE
                                                                                              WHEN AULAS_DADAS = 0
                                                                                              THEN 1
                                                                                              ELSE AULAS_DADAS
                                                                                              END )), 0),
                                SERIE = @SERIE ,
                                DT_INICIO = @DT_INICIO ,
                                DT_FIM = @DT_FIM ,
                                UNIDADE_ENSINO = @UNIDADE_ENSINO ,
                                MATRICULA = @MATRICULA ,
                                DEPENDENCIA = ISNULL(@DEPENDENCIA, 'N') ,
                                SERIE_REFERENCIA = @SERIE_REFERENCIA ,
                                DISCIPLINA_REFERENCIA = @DISCIPLINA_REFERENCIA ,
                                FALTA_FINAL = @FALTA_FINAL ,
                                CONCOMITANTE = ISNULL(@CONCOMITANTE, 'N') ,
                                EDUC_ESPECIAL = ISNULL(@EDUC_ESPECIAL, 'N') ,
                                MAIS_EDUCACAO = ISNULL(@MAIS_EDUCACAO, 'N') ,
                                OPTATIVAREFORCO = @OPTATIVAREFORCO
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND ORDEM = @ORDEM
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@ALUNO", histMatricula.Aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", histMatricula.Disciplina);
                contextQuery.Parameters.Add("@TURMA", histMatricula.Turma);
                contextQuery.Parameters.Add("@ANO", histMatricula.Ano);
                contextQuery.Parameters.Add("@SEMESTRE", histMatricula.Semestre);
                contextQuery.Parameters.Add("@ORDEM", histMatricula.Ordem);
                contextQuery.Parameters.Add("@NOTA_FINAL", histMatricula.NotaFinal);
                contextQuery.Parameters.Add("@SITUACAO_HIST", histMatricula.SituacaoHist);
                contextQuery.Parameters.Add("@HORAS_AULA", histMatricula.HorasAula);
                contextQuery.Parameters.Add("@AULAS_DADAS", histMatricula.AulasDadas);
                contextQuery.Parameters.Add("@AULAS_PREVISTAS", histMatricula.AulasPrevistas);
                contextQuery.Parameters.Add("@NIVEL_PRESENCA", histMatricula.NivelPresenca);
                contextQuery.Parameters.Add("@SERIE", histMatricula.Serie);
                contextQuery.Parameters.Add("@DT_INICIO", histMatricula.DtInicio);
                contextQuery.Parameters.Add("@DT_FIM", histMatricula.DtFim);
                contextQuery.Parameters.Add("@UNIDADE_ENSINO", histMatricula.UnidadeEnsino);
                contextQuery.Parameters.Add("@MATRICULA", histMatricula.Matricula);
                contextQuery.Parameters.Add("@FALTA_FINAL", histMatricula.FaltaFinal);
                contextQuery.Parameters.Add("@OPTATIVAREFORCO", histMatricula.OptativaReforco);
                contextQuery.Parameters.Add("@DEPENDENCIA", histMatricula.Dependencia);
                contextQuery.Parameters.Add("@SERIE_REFERENCIA", histMatricula.SerieReferencia);
                contextQuery.Parameters.Add("@DISCIPLINA_REFERENCIA", histMatricula.DisciplinaReferencia);
                contextQuery.Parameters.Add("@CONCOMITANTE", histMatricula.Concomitante);
                contextQuery.Parameters.Add("@EDUC_ESPECIAL", histMatricula.EducEspecial);
                contextQuery.Parameters.Add("@MAIS_EDUCACAO", histMatricula.MaisEducacao);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
        }

        private static void Inserir(LyHistMatricula histmatricula, DataContext context)
        {
            var contextQuery = new ContextQuery(
                    @" INSERT  LY_HISTMATRICULA ( ALUNO, 
                            ORDEM, 
                            ANO, 
                            SEMESTRE, 
                            DISCIPLINA, 
                            TURMA,
                            NOTA_FINAL, 
                            SITUACAO_HIST,                             
                            AULAS_DADAS, 
                            CREDITOS,
                            OBSERVACAO,
                            NIVEL_PRESENCA,
                            SERIE, 
                            DT_INICIO, 
                            DT_FIM, 
                            DT_MATRICULA,
                            UNIDADE_ENSINO, 
                            OUTRAS, 
                            MATRICULA,
                            DEPENDENCIA,
                            FALTA_FINAL,
                            OPTATIVAREFORCO)
                        VALUES  (
                            @ALUNO, 
                            @ORDEM, 
                            @ANO, 
                            @SEMESTRE, 
                            @DISCIPLINA, 
                            @TURMA,
                            @NOTA_FINAL, 
                            @SITUACAO_HIST, 
                            @AULAS_DADAS, 
                            @CREDITOS,
                            @OBSERVACAO,
                            @NIVEL_PRESENCA,
                            @SERIE, 
                            @DT_INICIO, 
                            @DT_FIM, 
                            GetDate(),
                            @UNIDADE_ENSINO, 
                            @OUTRAS, 
                            @MATRICULA,
                            @DEPENDENCIA,
                            @FALTA_FINAL,
                            @OPTATIVAREFORCO) ");

            contextQuery.Parameters.Add("@ALUNO", histmatricula.Aluno);
            contextQuery.Parameters.Add("@ORDEM", TechneDbType.T_NUMERO_PEQUENO, histmatricula.Ordem);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, histmatricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, histmatricula.Semestre);
            contextQuery.Parameters.Add("@DISCIPLINA", histmatricula.Disciplina);
            contextQuery.Parameters.Add("@TURMA", histmatricula.TurmaHist);
            contextQuery.Parameters.Add("@NOTA_FINAL", histmatricula.NotaFinal);
            contextQuery.Parameters.Add("@SITUACAO_HIST", histmatricula.SituacaoHist);
            //contextQuery.Parameters.Add("@PERC_PRESENCA", histmatricula.PercPresenca);
            contextQuery.Parameters.Add("@AULAS_DADAS", TechneDbType.T_DECIMAL_MEDIO, histmatricula.AulasDadas);
            contextQuery.Parameters.Add("@CREDITOS", TechneDbType.T_DECIMAL_MEDIO, histmatricula.Creditos);
            contextQuery.Parameters.Add("@OBSERVACAO", histmatricula.Observacao);
            contextQuery.Parameters.Add("@NIVEL_PRESENCA", histmatricula.NivelPresenca);
            contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, histmatricula.Serie);
            contextQuery.Parameters.Add("@DT_INICIO", TechneDbType.T_DATA, histmatricula.DtInicio);
            contextQuery.Parameters.Add("@DT_FIM", TechneDbType.T_DATA, histmatricula.DtFim);
            contextQuery.Parameters.Add("@DT_MATRICULA", TechneDbType.T_DATA, histmatricula.DtMatricula);
            contextQuery.Parameters.Add("@UNIDADE_ENSINO", histmatricula.UnidadeEnsino);
            contextQuery.Parameters.Add("@OUTRAS", histmatricula.Outras);
            contextQuery.Parameters.Add("@MATRICULA", histmatricula.Matricula);
            contextQuery.Parameters.Add("@DEPENDENCIA", histmatricula.Dependencia);
            contextQuery.Parameters.Add("@FALTA_FINAL", histmatricula.FaltaFinal);
            contextQuery.Parameters.Add("@OPTATIVAREFORCO", histmatricula.OptativaReforco);

            context.ApplyModifications(contextQuery);
        }

        public static void Inserir(Ly_histmatricula histMatricula)
        {
            var contextQuery = new ContextQuery(
                    @" INSERT  LY_HISTMATRICULA ( ALUNO, 
                            ORDEM, 
                            ANO, 
                            SEMESTRE, 
                            DISCIPLINA, 
                            TURMA,
                            NOTA_FINAL, 
                            SITUACAO_HIST,                             
                            AULAS_DADAS, 
                            CREDITOS,
                            OBSERVACAO,
                            NIVEL_PRESENCA,
                            SERIE, 
                            DT_INICIO, 
                            DT_FIM,                             
                            UNIDADE_ENSINO,                            
                            MATRICULA,
                            DEPENDENCIA,
                            HORAS_AULA,
                            LANC_DEB,
                            AULAS_PREVISTAS,
                            NUM_CHAMADA,
                            GRUPO_ELETIVA,
                            SUBTURMA1,
                            SUBTURMA2,
                            COBRANCA_SEP,
                            DT_ULTALT,
                            DT_MATRICULA,
                            TIPO_APROVACAO,
                            SERIE_REFERENCIA,                            
                            DISCIPLINA_REFERENCIA,
                            SIT_DETALHE,
                            FALTA_FINAL,
                            CONCOMITANTE,
                            MAIS_EDUCACAO,
                            EDUC_ESPECIAL,
                            OPTATIVAREFORCO )
                        VALUES  (
                            @ALUNO, 
                            @ORDEM, 
                            @ANO, 
                            @SEMESTRE, 
                            @DISCIPLINA, 
                            @TURMA,
                            @NOTA_FINAL, 
                            @SITUACAO_HIST, 
                            @AULAS_DADAS, 
                            @CREDITOS,
                            @OBSERVACAO,
                            @NIVEL_PRESENCA,
                            @SERIE, 
                            @DT_INICIO, 
                            @DT_FIM,                             
                            @UNIDADE_ENSINO,                             
                            @MATRICULA,
                            @DEPENDENCIA,
                            @HORAS_AULA,
                            @LANC_DEB,
                            @AULAS_PREVISTAS,
                            @NUM_CHAMADA,
                            @GRUPO_ELETIVA,
                            @SUBTURMA1,
                            @SUBTURMA2,
                            @COBRANCA_SEP,
                            @DT_ULTALT,
                            @DT_MATRICULA,
                            @TIPO_APROVACAO,
                            @SERIE_REFERENCIA,
                            @DISCIPLINA_REFERENCIA,
                            @SIT_DETALHE,
                            @FALTA_FINAL,
                            @CONCOMITANTE,
                            @MAIS_EDUCACAO,
                            @EDUC_ESPECIAL,
                            @OPTATIVAREFORCO) ");

            contextQuery.Parameters.Add("@ALUNO", histMatricula.Rows[0].Aluno);
            contextQuery.Parameters.Add("@ORDEM", TechneDbType.T_NUMERO_PEQUENO, histMatricula.Rows[0].Ordem);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, histMatricula.Rows[0].Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, histMatricula.Rows[0].Semestre);
            contextQuery.Parameters.Add("@DISCIPLINA", histMatricula.Rows[0].Disciplina);
            contextQuery.Parameters.Add("@TURMA", histMatricula.Rows[0].Turma);
            contextQuery.Parameters.Add("@NOTA_FINAL", histMatricula.Rows[0].Nota_final);
            contextQuery.Parameters.Add("@SITUACAO_HIST", histMatricula.Rows[0].Situacao_hist);
            contextQuery.Parameters.Add("@AULAS_DADAS", TechneDbType.T_DECIMAL_MEDIO, histMatricula.Rows[0].Aulas_dadas);
            contextQuery.Parameters.Add("@CREDITOS", TechneDbType.T_DECIMAL_MEDIO, histMatricula.Rows[0].Creditos);
            contextQuery.Parameters.Add("@OBSERVACAO", histMatricula.Rows[0].Observacao);
            contextQuery.Parameters.Add("@NIVEL_PRESENCA", histMatricula.Rows[0].Nivel_presenca);
            contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, histMatricula.Rows[0].Serie);
            contextQuery.Parameters.Add("@DT_INICIO", TechneDbType.T_DATA, histMatricula.Rows[0].Dt_inicio);
            contextQuery.Parameters.Add("@DT_FIM", TechneDbType.T_DATA, histMatricula.Rows[0].Dt_fim);
            contextQuery.Parameters.Add("@DT_MATRICULA", TechneDbType.T_DATA, histMatricula.Rows[0].Dt_matricula);
            contextQuery.Parameters.Add("@UNIDADE_ENSINO", histMatricula.Rows[0].Unidade_ensino);
            contextQuery.Parameters.Add("@MATRICULA", histMatricula.Rows[0].Matricula);
            contextQuery.Parameters.Add("@DEPENDENCIA", histMatricula.Rows[0].Dependencia);
            contextQuery.Parameters.Add("@FALTA_FINAL", histMatricula.Rows[0].Falta_final);
            contextQuery.Parameters.Add("@HORAS_AULA", TechneDbType.T_DECIMAL_MEDIO, histMatricula.Rows[0].Horas_aula);
            contextQuery.Parameters.Add("@LANC_DEB", TechneDbType.T_NUMERO, histMatricula.Rows[0].Lanc_deb);
            contextQuery.Parameters.Add("@AULAS_PREVISTAS", TechneDbType.T_DECIMAL_MEDIO_PRECISO, histMatricula.Rows[0].Aulas_previstas);
            contextQuery.Parameters.Add("@NUM_CHAMADA", TechneDbType.T_NUMERO, histMatricula.Rows[0].Num_chamada);
            contextQuery.Parameters.Add("@GRUPO_ELETIVA", histMatricula.Rows[0].Grupo_eletiva);
            contextQuery.Parameters.Add("@SUBTURMA1", histMatricula.Rows[0].Subturma1);
            contextQuery.Parameters.Add("@SUBTURMA2", histMatricula.Rows[0].Subturma2);
            contextQuery.Parameters.Add("@COBRANCA_SEP", histMatricula.Rows[0].Cobranca_sep);
            contextQuery.Parameters.Add("@DT_ULTALT", TechneDbType.T_DATA, histMatricula.Rows[0].Dt_ultalt);
            contextQuery.Parameters.Add("@TIPO_APROVACAO", histMatricula.Rows[0].Tipo_aprovacao);
            contextQuery.Parameters.Add("@SERIE_REFERENCIA", histMatricula.Rows[0].Serie_referencia);
            contextQuery.Parameters.Add("@DISCIPLINA_REFERENCIA", histMatricula.Rows[0].Disciplina_referencia);
            contextQuery.Parameters.Add("@SIT_DETALHE", histMatricula.Rows[0].Sit_detalhe);

            contextQuery.Parameters.Add("@CONCOMITANTE", histMatricula.Rows[0].Concomitante);
            contextQuery.Parameters.Add("@MAIS_EDUCACAO", histMatricula.Rows[0].Mais_educacao);
            contextQuery.Parameters.Add("@EDUC_ESPECIAL", histMatricula.Rows[0].Educ_especial);
            contextQuery.Parameters.Add("@OPTATIVAREFORCO", histMatricula.Rows[0].Optativareforco);

            ExecutarAlteracao(contextQuery);
        }

        public static void AtualizarSituacaoFinalEFrequenciaGlobal(LyHistMatricula histmatricula)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    Alterar(histmatricula, context);

                    var situacaoFinalAluno = new TceSituacaoFinalAluno
                        {
                            Aluno = histmatricula.Aluno,
                            Ano = histmatricula.Ano,
                            Periodo = histmatricula.Semestre,
                            Turma = histmatricula.Turma,
                            FrequenciaGlobal = VerificaFrequenciaGlobal(histmatricula.Aluno, Convert.ToInt32(histmatricula.Ano), Convert.ToInt32(histmatricula.Semestre), histmatricula.Turma),
                            Matricula = histmatricula.Matricula
                        };

                    if (histmatricula.Ano == 2021)
                    {
                        situacaoFinalAluno.SituacaoFinal = histmatricula.SituacaoHist;
                    }
                    else
                    {
                        situacaoFinalAluno.SituacaoFinal = VerificaSituacaoFinal(histmatricula.Aluno, Convert.ToInt32(histmatricula.Ano), Convert.ToInt32(histmatricula.Semestre), histmatricula.Turma);
                    }

                    if (situacaoFinalAluno.SituacaoFinal != "Sem Disciplina")
                    {
                        if (!SituacaoFinalAluno.ExisteSituacaoFinalPor(context, situacaoFinalAluno.Aluno, situacaoFinalAluno.Ano, situacaoFinalAluno.Periodo, situacaoFinalAluno.Turma))
                        {
                            SituacaoFinalAluno.Inserir(situacaoFinalAluno, context);
                        }
                        else
                        {
                            SituacaoFinalAluno.AtualizarSituacaoFinalEFrequenciaGlobal(situacaoFinalAluno, context);
                        }
                    }
                    else
                    {
                        SituacaoFinalAluno.Remover(situacaoFinalAluno, context);
                    }
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void Alterar(LyHistMatricula histMatricula, DataContext context)
        {
            var contextQuery = new ContextQuery(
                    @" UPDATE [LYCEUM].[DBO].LY_HISTMATRICULA
                        SET   FALTA_FINAL = @FALTA_FINAL,
                              SITUACAO_HIST = @SITUACAO_HIST,
                              NOTA_FINAL = @NOTA_FINAL,
                              OBSERVACAO = @OBSERVACAO,
                              AULAS_DADAS = @AULAS_DADAS
                        WHERE ALUNO = @ALUNO                             
                              AND ANO = @ANO
                              AND SEMESTRE = @SEMESTRE
                              AND TURMA = @TURMA
                              AND DISCIPLINA = @DISCIPLINA
                    ");

            contextQuery.Parameters.Add("@ALUNO", histMatricula.Aluno);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, histMatricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, histMatricula.Semestre);
            contextQuery.Parameters.Add("@TURMA", histMatricula.Turma);
            contextQuery.Parameters.Add("@DISCIPLINA", histMatricula.Disciplina);
            contextQuery.Parameters.Add("@FALTA_FINAL", histMatricula.FaltaFinal);
            contextQuery.Parameters.Add("@SITUACAO_HIST", histMatricula.SituacaoHist);
            contextQuery.Parameters.Add("@NOTA_FINAL", histMatricula.NotaFinal.Replace(',', '.'));
            contextQuery.Parameters.Add("@OBSERVACAO", histMatricula.Observacao);
            contextQuery.Parameters.Add("@AULAS_DADAS", histMatricula.AulasDadas);

            context.ApplyModifications(contextQuery);
        }

        public static ValidacaoDados ValidarAlteracao(LyHistMatricula histmatricula, bool situacaoNaoAlterada)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
                                     {
                                         Valido = false
                                     };
            int notaMaxima = Disciplina.ConsultarNotaMaximaPor(histmatricula.Disciplina);
            int qtdBimestres = histmatricula.Semestre == 0 ? 4 : 2;
            int maiorNotaPossivel = notaMaxima * qtdBimestres;
            decimal notaFinal = decimal.Parse(histmatricula.NotaFinal);
            DataTable dt = Lyceum.RN.Disciplina.ConsultarPorDisciplina(histmatricula.Disciplina);
            bool possuiCriterioPorFrequencia = dt.Rows[0]["tem_freq"].ToString().Equals("S");
            bool possuiCriterioPorNota = dt.Rows[0]["tem_nota"].ToString().Equals("S");

            if (string.IsNullOrEmpty(histmatricula.Aluno))
            {
                mensagens.Add("O campo Aluno é obrigatório!");
            }

            if (string.IsNullOrEmpty(histmatricula.SituacaoHist))
            {
                mensagens.Add("O campo Situação da Disciplina é obrigatório!");
            }

            if (possuiCriterioPorNota)
            {
                if (string.IsNullOrEmpty(histmatricula.NotaFinal))
                {
                    mensagens.Add("O campo Total de Pontos é obrigatório!");
                }
                else
                {
                    if (notaFinal > maiorNotaPossivel)
                        mensagens.Add("Total de pontos não pode ser superior ao total de pontos da disciplina!");
                }
            }

            if (possuiCriterioPorFrequencia)
            {
                if (!histmatricula.AulasDadas.HasValue)
                {
                    mensagens.Add("O campo Aulas Dadas é obrigatório!");
                }

                if (!histmatricula.FaltaFinal.HasValue)
                {
                    mensagens.Add("O campo Total Falta é obrigatório!");
                }

                if (histmatricula.FaltaFinal.HasValue)
                {
                    if (!histmatricula.AulasDadas.HasValue || histmatricula.FaltaFinal.Value > histmatricula.AulasDadas.Value)
                        mensagens.Add("Total de faltas não pode ser superior ao total de aulas dadas!");
                }
            }

            if (!String.IsNullOrEmpty(histmatricula.Observacao) && histmatricula.Observacao.Length > 2000)
            {
                mensagens.Add("O campo observação não pode ter mais que 2000 caracteres!");
            }

            if ((histmatricula.Ano != 2020 && histmatricula.Ano != 2021 && !(histmatricula.Ano == 2023 && histmatricula.Semestre == 1)) && (histmatricula.SituacaoHist == "Promovido" || histmatricula.SituacaoHist == "Retido"))
            {
                mensagens.Add("Para este ano, esta situação não pode ser utilizada.");
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                if (situacaoNaoAlterada)
                {
                    if (histmatricula.Dependencia == "S")
                    {
                        if (notaFinal >= 5)
                        {
                            histmatricula.SituacaoHist = "APROVADO";
                        }
                        else
                        {
                            histmatricula.SituacaoHist = "REP NOTA";
                        }
                    }
                    else
                    {
                        if (histmatricula.Ano != 2021)
                        {
                            if (!histmatricula.SituacaoHist.Equals("APROVADO CONSELHO") && !histmatricula.SituacaoHist.Equals("CANCELADO"))
                            {
                                bool possuiFrequenciaParaAprovacao = true;
                                bool possuiNotaParaAprovacao = true;

                                if (possuiCriterioPorFrequencia)
                                {
                                    decimal maximoDeFalta = 0.25m * histmatricula.AulasDadas.Value;
                                    possuiFrequenciaParaAprovacao = histmatricula.FaltaFinal.Value < maximoDeFalta;
                                }

                                if (possuiCriterioPorNota)
                                    possuiNotaParaAprovacao = notaFinal >= maiorNotaPossivel / 2;

                                if (possuiFrequenciaParaAprovacao && possuiNotaParaAprovacao)
                                    histmatricula.SituacaoHist = "APROVADO";
                                else
                                {
                                    if (possuiFrequenciaParaAprovacao)
                                        histmatricula.SituacaoHist = "REP NOTA";
                                    else
                                        histmatricula.SituacaoHist = "REP FREQ";
                                }
                            }
                        }
                    }
                }

                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static ValidacaoDados ValidarInserir(LyHistMatricula histmatricula)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
                                 {
                                     Valido = false
                                 };

            if (string.IsNullOrEmpty(histmatricula.Aluno))
            {
                mensagens.Add("O campo ALUNO é obrigatório!");
            }

            if (histmatricula.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (histmatricula.Semestre < 0)
            {
                mensagens.Add("O campo SEMESTRE é obrigatório!");
            }

            if (string.IsNullOrEmpty(histmatricula.UnidadeEnsino))
            {
                mensagens.Add("O campo UNIDADE DE ENSINO é obrigatório!");
            }
            else
            {
                if (histmatricula.UnidadeEnsino == "99999991" && string.IsNullOrEmpty(histmatricula.Outras))
                {
                    mensagens.Add("O campo NOME DA ESCOLA é obrigatório!");
                }
            }

            if (string.IsNullOrEmpty(histmatricula.TurmaHist))
            {
                mensagens.Add("O campo TURMA é obrigatório!");
            }

            if (string.IsNullOrEmpty(histmatricula.Disciplina))
            {
                mensagens.Add("O campo DISCIPLINA é obrigatório!");
            }

            if (string.IsNullOrEmpty(histmatricula.SituacaoHist))
            {
                mensagens.Add("O campo SITUAÇÃO DA DISCIPLINA é obrigatório!");
            }

            if (histmatricula.UnidadeEnsino == "99999991" && string.IsNullOrEmpty(histmatricula.Serie.ToString()))
            {
                mensagens.Add("O campo Ano de Escolaridade é obrigatório!");
            }

            if (histmatricula.UnidadeEnsino == "99999991" && histmatricula.Serie == 0)
            {
                mensagens.Add("O campo Ano de Escolaridade não pode ser 0(zero)!");
            }

            if (histmatricula.Serie != null && histmatricula.Serie > 999)
            {
                mensagens.Add("O campo Ano de Escolaridade não pode ter mais que 3 dígitos!");
            }

            if (string.IsNullOrEmpty(histmatricula.NotaFinal))
            {
                mensagens.Add("O campo TOTAL DE PONTOS é obrigatório!");
            }

            if (histmatricula.FaltaFinal == null)
            {
                mensagens.Add("O campo FALTA FINAL é obrigatório!");
            }

            if (histmatricula.AulasDadas == null || histmatricula.AulasDadas == 0)
            {
                mensagens.Add("O campo AULAS DADAS é obrigatório e não pode ser 0(zero)!");
            }

            if (histmatricula.FaltaFinal != null && histmatricula.AulasDadas != null)
            {
                if (histmatricula.FaltaFinal > histmatricula.AulasDadas)
                {
                    mensagens.Add("O campo FALTA FINAL não pode ser maior que as AULAS DADAS!");
                }
            }
            ContextQuery contextQueryVerificaMatriculaRegular = new ContextQuery();
            int resultadoVerificaMatriculaRegular = 0;
            ContextQuery contextQueryVerificaLinguaEstrangeiraOptativaOuEnsinoReligioso = new ContextQuery();
            int resultadoVLinguaEstrangeiraOptativaOuEnsinoReligioso = 0;
            string auxMensagem = string.Empty;

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*) FROM LY_HISTMATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND TURMA = @TURMA
                                AND DISCIPLINA = @DISCIPLINA ");

                    contextQuery.Parameters.Add("@ALUNO", histmatricula.Aluno);
                    contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, histmatricula.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, histmatricula.Semestre);
                    contextQuery.Parameters.Add("@DISCIPLINA", histmatricula.Disciplina);
                    contextQuery.Parameters.Add("@TURMA", histmatricula.TurmaHist);

                    var resultado = ctx.GetReturnValue<int>(contextQuery);

                    if (resultado > 0)
                    {
                        mensagens.Add("Já existe um histórico cadastrado com estes dados.");
                    }

                    if (histmatricula.OptativaReforco != "N")
                    {

                        contextQueryVerificaMatriculaRegular.Command =
                            @" SELECT COUNT(*)
                                FROM LY_HISTMATRICULA
                                WHERE ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND ALUNO = @ALUNO
                                AND SITUACAO_HIST <> 'Cancelado'
                                AND ISNULL(DEPENDENCIA, 'N') = 'N'
                                AND ISNULL(CONCOMITANTE, 'N') = 'N'
                                AND ISNULL(EDUC_ESPECIAL, 'N') = 'N'
                                AND ISNULL(MAIS_EDUCACAO, 'N') = 'N'
                                AND ISNULL(OPTATIVAREFORCO, 'N') = 'N'";

                        contextQueryVerificaMatriculaRegular.Parameters.Add("@ALUNO", histmatricula.Aluno);
                        contextQueryVerificaMatriculaRegular.Parameters.Add("@ANO", TechneDbType.T_ANO,
                                                                            histmatricula.Ano);
                        contextQueryVerificaMatriculaRegular.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2,
                                                                            histmatricula.Semestre);

                        resultadoVerificaMatriculaRegular = ctx.GetReturnValue<int>(contextQueryVerificaMatriculaRegular);

                        if (resultadoVerificaMatriculaRegular == 0)
                        {
                            mensagens.Add("Não existe matrícula regular para ano/período.");
                        }

                        if (histmatricula.OptativaReforco == "L" || histmatricula.OptativaReforco == "R")
                        {
                            contextQueryVerificaLinguaEstrangeiraOptativaOuEnsinoReligioso.Command =
                                @" SELECT COUNT(*)
                                FROM LY_HISTMATRICULA M (NOLOCK)
                                INNER JOIN TCE_CONFIRMACAO_MATRICULA C (NOLOCK)
                                ON (C.ALUNO = M.ALUNO
                                AND C.ANO = M.ANO
                                AND C.PERIODO = M.SEMESTRE)
                                WHERE M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE
                                AND M.ALUNO = @ALUNO
                                AND M.SITUACAO_HIST <> 'Cancelado'
                                AND C.STATUS = 'Confirmado'
                                AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
                                AND ISNULL(M.CONCOMITANTE, 'N') = 'N'
                                AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N'
                                AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N'";

                            if (histmatricula.OptativaReforco == "L")
                            {
                                contextQueryVerificaLinguaEstrangeiraOptativaOuEnsinoReligioso.Command +=
                                    " AND C.LINGUA_ESTRANGEIRA_FACULTATIVA=0";
                            }
                            if (histmatricula.OptativaReforco == "R")
                            {
                                contextQueryVerificaLinguaEstrangeiraOptativaOuEnsinoReligioso.Command +=
                                    " AND C.ENSINO_RELIGIOSO=0";
                            }

                            contextQueryVerificaLinguaEstrangeiraOptativaOuEnsinoReligioso.Parameters.Add("@ALUNO",
                                                                                                          histmatricula.
                                                                                                              Aluno);
                            contextQueryVerificaLinguaEstrangeiraOptativaOuEnsinoReligioso.Parameters.Add("@ANO",
                                                                                                          TechneDbType.
                                                                                                              T_ANO,
                                                                                                          histmatricula.
                                                                                                              Ano);
                            contextQueryVerificaLinguaEstrangeiraOptativaOuEnsinoReligioso.Parameters.Add("@SEMESTRE",
                                                                                                          TechneDbType.
                                                                                                              T_SEMESTRE2,
                                                                                                          histmatricula.
                                                                                                              Semestre);

                            resultadoVLinguaEstrangeiraOptativaOuEnsinoReligioso =
                                ctx.GetReturnValue<int>(contextQueryVerificaLinguaEstrangeiraOptativaOuEnsinoReligioso);

                            if (resultadoVLinguaEstrangeiraOptativaOuEnsinoReligioso > 0)
                            {
                                auxMensagem = "Confirmação de matrícula do aluno não prevê matrícula em turma de " +
                                              (histmatricula.OptativaReforco == "L"
                                                   ? " Língua Estrangeira Optativa."
                                                   : " Ensino Religioso.");

                                mensagens.Add(auxMensagem);
                            }
                        }
                    }



                    if (histmatricula.UnidadeEnsino != "99999991")
                    {
                        contextQuery = new ContextQuery(
                            @" SELECT COUNT(*) FROM dbo.LY_TURMA t
                             INNER JOIN LY_GRADE_SERIE GS ON t.ANO = GS.ANO
                                                                    AND t.SEMESTRE = GS.SEMESTRE
                                                                    AND t.TURMA = GS.GRADE
                                    INNER JOIN DBO.LY_GRADE G ON GS.CURRICULO = G.CURRICULO
                                                                 AND GS.CURSO = G.CURSO
                                                                 AND t.DISCIPLINA = G.DISCIPLINA
                                                                 AND GS.TURNO = G.TURNO
                                                                 AND G.SERIE_IDEAL = GS.SERIE
                            WHERE   t.ANO = @ANO
                                    AND t.SEMESTRE = @SEMESTRE
                                    AND t.TURMA = @TURMA ");

                        contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, histmatricula.Ano);
                        contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, histmatricula.Semestre);
                        contextQuery.Parameters.Add("@TURMA", histmatricula.TurmaHist);

                        resultado = ctx.GetReturnValue<int>(contextQuery);

                        if (resultado > 0)
                        {
                            contextQuery = new ContextQuery(
                        @" SELECT COUNT(*) FROM dbo.LY_TURMA t
                             INNER JOIN LY_GRADE_SERIE GS ON t.ANO = GS.ANO
                                                                    AND t.SEMESTRE = GS.SEMESTRE
                                                                    AND t.TURMA = GS.GRADE
                                    INNER JOIN DBO.LY_GRADE G ON GS.CURRICULO = G.CURRICULO
                                                                 AND GS.CURSO = G.CURSO
                                                                 AND t.DISCIPLINA = G.DISCIPLINA
                                                                 AND GS.TURNO = G.TURNO
                                                                 AND G.SERIE_IDEAL = GS.SERIE
                            WHERE   t.ANO = @ANO
                                    AND t.SEMESTRE = @SEMESTRE
                                    AND t.TURMA = @TURMA
                                    AND G.DISCIPLINA = @DISCIPLINA ");

                            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, histmatricula.Ano);
                            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, histmatricula.Semestre);
                            contextQuery.Parameters.Add("@DISCIPLINA", histmatricula.Disciplina);
                            contextQuery.Parameters.Add("@TURMA", histmatricula.TurmaHist);

                            resultado = ctx.GetReturnValue<int>(contextQuery);

                            if (resultado <= 0)
                            {
                                mensagens.Add("Esta disciplina não faz parte da grade para esta turma / ano / periodo.");
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

        public static void Remover(LyHistMatricula histmatricula)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery(
                        @" SELECT  DISTINCT
                                UNIDADE_ENSINO
                        FROM    LY_HISTMATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND TURMA = @TURMA
                                AND SEMESTRE = @SEMESTRE ");

                    contextQuery.Parameters.Add("@ALUNO", histmatricula.Aluno);
                    contextQuery.Parameters.Add("@ANO", histmatricula.Ano);
                    contextQuery.Parameters.Add("@TURMA", histmatricula.Turma);
                    contextQuery.Parameters.Add("@SEMESTRE", histmatricula.Semestre);

                    var unidadeEnsino = context.GetReturnValue<string>(contextQuery);
                    histmatricula.UnidadeEnsino = unidadeEnsino;

                    Remover(histmatricula, context);

                    var situacaoFinalAluno = new TceSituacaoFinalAluno
                    {
                        Aluno = histmatricula.Aluno,
                        Ano = histmatricula.Ano,
                        Periodo = histmatricula.Semestre,
                        Turma = histmatricula.Turma,
                        Matricula = histmatricula.Matricula
                    };

                    SituacaoFinalAluno.Remover(situacaoFinalAluno, context);

                    contextQuery = new ContextQuery(
                        @" SELECT  COUNT(*) FROM LY_HISTMATRICULA
                        WHERE   ALUNO = @ALUNO                                
                                AND ANO = @ANO
                                AND UNIDADE_ENSINO = @UNIDADE_ENSINO
                                AND SEMESTRE = @SEMESTRE ");

                    contextQuery.Parameters.Add("@ALUNO", histmatricula.Aluno);
                    contextQuery.Parameters.Add("@ANO", histmatricula.Ano);
                    contextQuery.Parameters.Add("@SEMESTRE", histmatricula.Semestre);
                    contextQuery.Parameters.Add("@UNIDADE_ENSINO", histmatricula.UnidadeEnsino);

                    var historicos = context.GetReturnValue<int>(contextQuery);

                    if (historicos > 0)
                    {
                        situacaoFinalAluno.SituacaoFinal = VerificaSituacaoFinal(histmatricula.Aluno,
                                                                                 Convert.ToInt32(histmatricula.Ano),
                                                                                 Convert.ToInt32(histmatricula.Semestre),
                                                                                 histmatricula.Turma);
                        situacaoFinalAluno.FrequenciaGlobal = VerificaFrequenciaGlobal(histmatricula.Aluno,
                                                                                       Convert.ToInt32(histmatricula.Ano),
                                                                                       Convert.ToInt32(
                                                                                           histmatricula.Semestre),
                                                                                       histmatricula.Turma);

                        if (!string.IsNullOrEmpty(situacaoFinalAluno.SituacaoFinal))
                        {
                            SituacaoFinalAluno.Inserir(situacaoFinalAluno, context);
                        }
                    }
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public static void Remover(LyHistMatricula histmatricula, DataContext context)
        {
            var contextQuery = new ContextQuery(
                     @" DELETE LY_HISTORICO_DOCENTE
                                WHERE  ALUNO = @ALUNO
                                AND ORDEM = @ORDEM
                                AND ANO = @ANO
                                AND PERIODO = @SEMESTRE
                                AND DISCIPLINA = @DISCIPLINA

                        DELETE LY_FALTA_HISTMATR                    
                        WHERE   ALUNO = @ALUNO
                                AND ORDEM = @ORDEM
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND DISCIPLINA = @DISCIPLINA

                        DELETE LY_NOTA_HISTMATR                    
                        WHERE   ALUNO = @ALUNO
                                AND ORDEM = @ORDEM
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND DISCIPLINA = @DISCIPLINA                       
                        
                        DELETE LY_HISTMATRICULA                    
                        WHERE   ALUNO = @ALUNO
                                AND ORDEM = @ORDEM
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND DISCIPLINA = @DISCIPLINA ");

            contextQuery.Parameters.Add("@ALUNO", histmatricula.Aluno);
            contextQuery.Parameters.Add("@ORDEM", TechneDbType.T_NUMERO_PEQUENO, histmatricula.Ordem);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, histmatricula.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, histmatricula.Semestre);
            contextQuery.Parameters.Add("@DISCIPLINA", histmatricula.Disciplina);

            context.ApplyModifications(contextQuery);
        }

        public static void Remover(string aluno, decimal ano, decimal semestre, decimal ordem)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"  DELETE LY_HISTMATRICULA                    
                        WHERE   ALUNO = @ALUNO
                                AND ORDEM = @ORDEM
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE "
                    };

                    contextQuery.Parameters.Add("@ALUNO", aluno);
                    contextQuery.Parameters.Add("@ORDEM", TechneDbType.T_NUMERO_PEQUENO, ordem);
                    contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                    contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public string ObtemTurmaOptativaReforcoPor(string censo, int ano, int periodo, string turma)
        {
            var contextQuery = new ContextQuery();
            string TurmaOptativaReforco = string.Empty;

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                contextQuery.Command =
                     @"SELECT   TOP 1 OPTATIVAREFORCO
                            FROM LY_CURSO C 
                            INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO 
                            INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                            INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO 
                            INNER JOIN dbo.LY_TURNO TU ON TU.TURNO=T.TURNO
                            INNER JOIN dbo.LY_SERIE S ON S.SERIE=T.SERIE AND S.CURSO=T.CURSO
                           WHERE t.FACULDADE = @CENSO                      
                            AND T.ANO = @ANO
                            AND T.SEMESTRE= @PERIODO 
                            AND T.TURMA = @TURMA  
                            AND T.OPTATIVAREFORCO <> 'N'                     
                        ORDER BY T.TURMA ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURMA", turma);


                TurmaOptativaReforco = ctx.GetReturnValue<string>(contextQuery); ;
            }

            return TurmaOptativaReforco;
        }

        public bool ConsultaMatriculaPorAno(string aluno, int ano, int periodo, string turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            object obj = new Object();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"SELECT TOP 1
                            1
                     FROM   DBO.LY_HISTMATRICULA
                     WHERE  SITUACAO_HIST <> 'CANCELADO'
                            AND SITUACAO_HIST <> 'DISPENSADO'
                            AND SITUACAO_HIST <> 'INCONCLUIDO'
                            AND TURMA = @TURMA
                            AND ALUNO = @ALUNO
                            AND ANO = @ANO
                            AND SEMESTRE= @PERIODO "
                };

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                obj = ctx.GetReturnValue(contextQuery);

                if (obj == null)
                {
                    return false;
                }

                return true;
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

        public int ObtemOrdemIdeal(string aluno, int ano, int periodo, string turma)
        {
            HistFaculdade nrHistFaculdade = new HistFaculdade();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            int ordemHistFaculdade = 0;
            int ordem = 0;
            int ordemHistMatricula = 0;

            try
            {
                //Verifica se a turma já possui uma ordem para o ano / periodo / aluno
                ordemHistMatricula = this.ObtemOrdemPor(turma, aluno, ano, periodo);
                if (ordemHistMatricula <= 0)
                {
                    //Caso não exista busca maior ordem da tabela histFaculdade e incrementa
                    ordemHistFaculdade = nrHistFaculdade.ObtemMaiorOrdemPor(aluno);
                    ordem = ordemHistFaculdade + 1;
                }
                else
                {
                    ordem = ordemHistMatricula;
                }

                // Verificar se a ordem já existe na tabela em outra turma
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    DBO.LY_HISTMATRICULA
                        WHERE   ALUNO = @ALUNO
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND TURMA <> @TURMA
                                AND ORDEM = @ORDEM "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ORDEM", ordem);

                //Caso exista a ordem em outra turma, busca maior ordem da tabela histMatricula e incrementa
                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    ordem = this.ObtemMaiorOrdemPor(aluno, ano, periodo) + 1;
                }

                return ordem;
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

        public int ObtemOrdemPor(string turma, string aluno, int ano, int periodo)
        {
            int ordem = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  ISNULL(MAX(ORDEM),0) AS ORDEM
                                FROM    LY_HISTMATRICULA
                                WHERE   ALUNO = @ALUNO
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND TURMA = @TURMA ")
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@TURMA", turma);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    ordem = Convert.ToInt32(reader["ORDEM"]);
                }

                return ordem;
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

        public int ObtemMaiorOrdemPor(string aluno, int ano, int periodo)
        {
            int ordem = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  ISNULL(MAX(ORDEM), 0) AS ORDEM
                                FROM    LY_HISTMATRICULA
                                WHERE   ALUNO = @ALUNO
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE ")
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    ordem = Convert.ToInt32(reader["ORDEM"]);
                }

                return ordem;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public bool PossuiHistoricoMatriculaPor(string aluno, int ordem, decimal ano, decimal semestre, string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    DBO.LY_HISTMATRICULA 
                        WHERE   ALUNO = @ALUNO
                                AND DISCIPLINA = @DISCIPLINA
                                AND ORDEM = @ORDEM
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE "
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@ORDEM", ordem);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public static DataTable ConsultaHistoricoMatriculaPor(string disciplina, string turma, decimal ano, decimal semestre, decimal? subperiodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            try
            {
                contextQuery.Command =
                    @"SELECT DISTINCT
                            pe.nome_compl ,
                            m.aluno ,
                            UPPER(m.SITUACAO_HIST) AS sit_matricula ,
                            m.ORDEM,
                            m.num_chamada ,
                            m.disciplina ,
                            m.turma ,
                            m.ano ,
                            m.semestre ,


                            CASE WHEN ( m.SITUACAO_HIST NOT IN ( 'Cancelado', 'Dispensado',
                                                                 'Trancado', 'Inconcluido' )
                                        AND 
                                            n.notaprova IS NULL 
                                            AND 
                                            n.conceito IS NOT NULL 
                                            AND 
                                            rtrim(n.conceito) <> '' ) 
                                 THEN CONVERT(decimal(5,2), replace(n.conceito,',','.'))

                                 WHEN ( m.SITUACAO_HIST NOT IN ( 'Cancelado', 'Dispensado',
                                                                 'Trancado', 'Inconcluido' )
                                        AND n.NotaProva IS NOT NULL
                                      ) 
                                THEN CONVERT(DECIMAL(5,2), REPLACE(n.NotaProva,',','.'))
                                 ELSE NULL
                            END AS MÉDIA ,
                            CASE WHEN m.SITUACAO_HIST NOT IN ( 'Cancelado', 'Dispensado',
                                                               'Trancado', 'Inconcluido' )
                                 THEN REPLACE(n.NotaRecuperacao,'.',',')
                                 ELSE NULL
                            END AS NotaRecuperacao ,
                            '' AS NotaFinal ,
                            CASE WHEN m.SITUACAO_HIST NOT IN ( 'Cancelado', 'Dispensado',
                                                               'Trancado', 'Inconcluido' )
                                 THEN CONVERT(INT, f.faltas)
                                 ELSE NULL
                            END AS faltas ,
                            prova.nome AS nome_prova ,
                            prova.nota_max ,
                            prova.formula ,
                            CASE WHEN m.SITUACAO_HIST NOT IN ( 'Cancelado', 'Dispensado',
                                                               'Trancado', 'Inconcluido' )
                                 THEN n.recuperacaoparalela
                                 ELSE 'N'
                            END AS recuperacao_paralela ,
                            CASE WHEN m.SITUACAO_HIST NOT IN ( 'Cancelado', 'Dispensado',
                                                               'Trancado', 'Inconcluido' )
                                 THEN n.semavaliacao
                                 ELSE 'N'
                            END AS sem_avaliacao ,
                            CASE
                                 WHEN m.SITUACAO_HIST NOT IN ( 'Cancelado', 'Dispensado',
                                                               'Trancado', 'Inconcluido' )
                                      AND MOTIVOSEMNOTAID IS NOT NULL
                                 THEN CONVERT(VARCHAR(1), n.MOTIVOSEMNOTAID)
                                 ELSE ''
                            END AS justificativa
                    FROM    dbo.LY_HISTMATRICULA m WITH ( NOLOCK )
                            JOIN ly_aluno a WITH ( NOLOCK ) ON m.aluno = a.aluno
                            JOIN LY_PESSOA PE ON PE.PESSOA = A.PESSOA
                            LEFT JOIN ly_freq freq WITH ( NOLOCK ) ON freq.disciplina = m.disciplina
                                                                      AND freq.turma = m.turma
                                                                      AND freq.ano = m.ano
                                                                      AND freq.periodo = m.semestre
                                                                      AND freq.subperiodo = @SUBPERIODO
                            LEFT JOIN ly_falta_histmatr f WITH ( NOLOCK ) ON f.ALUNO = m.ALUNO
                                                                             AND f.ANO = m.ANO
                                                                             AND f.DISCIPLINA = m.DISCIPLINA
                                                                             AND f.SEMESTRE = m.SEMESTRE
                                                                             AND f.TURMA = m.TURMA
                                                                             AND freq.freq = f.FREQ_ID
                            LEFT JOIN ly_prova prova WITH ( NOLOCK ) ON prova.disciplina = m.disciplina
                                                                        AND prova.turma = m.turma
                                                                        AND prova.ano = m.ano
                                                                        AND prova.semestre = m.semestre
                                                                        AND prova.subperiodo = @SUBPERIODO
                            LEFT JOIN LY_NOTA_histmatr n WITH ( NOLOCK ) ON n.DISCIPLINA = prova.disciplina
                                                                            AND n.turma = prova.TURMA
                                                                            AND n.ANO = prova.ANO
                                                                            AND n.SEMESTRE = prova.SEMESTRE
                                                                            AND n.NOTA_ID = prova.PROVA
                                                                            AND n.ALUNO = m.ALUNO
                    WHERE   m.disciplina = @DISCIPLINA
                            AND m.turma = @TURMA
                            AND m.ano = @ANO
                            AND m.semestre = @SEMESTRE
                            AND (freq.disciplina IS NOT NULL OR prova.disciplina IS NOT NULL)
                    ORDER BY m.num_chamada ,
                            PE.nome_compl
                        ";

                contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);

                return Consultar(contextQuery);
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

        public static QueryTable RetornaAnosDeHistoricoPor(string aluno)
        {
            return Consultar(@"SELECT DISTINCT LHM.ANO 
                              FROM [LYCEUM].[DBO].[LY_HISTMATRICULA] LHM
                              WHERE LHM.ALUNO = ? ORDER BY ANO DESC", aluno);
        }

        public static object RetornaPeriodosDeHistoricoPor(string ano, string aluno)
        {
            return Consultar(@"SELECT  DISTINCT LHM.SEMESTRE
                                FROM [LYCEUM].[DBO].[LY_HISTMATRICULA] LHM
                                WHERE LHM.ALUNO = ?
	                                AND LHM.ANO = ?
                                ", aluno, ano);
        }

        public DataTable ListaHistoricoMatriculasConsolidadoBimestralPor(int ano, int periodo, string turma, string disciplina)
        {
            DataContext contexto = null;
            ContextQuery contextQuery = new ContextQuery();
            DataTable lista = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingNoLock();

                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"SP_CONSOLIDADOBIMESTRAL_HISTORICO";
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                lista = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
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

        public void AtualizaSituacaoFinal(string aluno, decimal ano, decimal semestre, string turma, string situacao, string usuario)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            SituacaoFinalAluno rnSituacao = new SituacaoFinalAluno();

            try
            {
                AlterarSituacao(aluno, ano, semestre, turma, situacao, ctx);

                var situacaoFinalAluno = new TceSituacaoFinalAluno
                {
                    Aluno = aluno,
                    Ano = ano,
                    Periodo = semestre,
                    Turma = turma,
                    SituacaoFinal = situacao,
                    Matricula = usuario
                };

                rnSituacao.AtualizarSituacaoFinal(situacaoFinalAluno, ctx);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();

                //Trata mensagens de erros de validacao no processo de enturmacao
                if (Convert.ToString(ex.Message).Contains("ERRO_VALIDACAO:"))
                {
                    mensagem = Convert.ToString(ex.Message).Replace("ERRO_VALIDACAO:", string.Empty);
                }
                else
                {
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
                }

                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static void AlterarSituacao(string aluno, decimal ano, decimal semestre, string turma, string situacao, DataContext context)
        {
            var contextQuery = new ContextQuery(
                    @" UPDATE [LYCEUM].[DBO].LY_HISTMATRICULA                        SET   
                              SITUACAO_HIST = @SITUACAO_HIST,
                              DT_ULTALT = GETDATE()                             
                        WHERE ALUNO = @ALUNO                             
                              AND ANO = @ANO
                              AND SEMESTRE = @SEMESTRE
                              AND TURMA = @TURMA                              
                    ");

            contextQuery.Parameters.Add("@ALUNO", aluno);
            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@SITUACAO_HIST", situacao);

            context.ApplyModifications(contextQuery);
        }

        private int ObtemDependenciasPermitidasPor(string turma, int ano, int semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int qtdeDEP = 0;

            try
            {

                contextQuery.Command = @" SELECT DISTINCT QTDE_REPROV
                                FROM    EXTRACOES..CURSO_PARTICIPANTES_FECH_2025 GS (NOLOCK)
                                INNER JOIN LY_TURMA T ON T.ANO=GS.ANO AND T.CURSO=GS.CURSO AND T.SERIE=GS.SERIE
                                WHERE   TURMA = @TURMA
                                    AND t.ANO = @ANO
                                    AND t.SEMESTRE= @SEMESTRE";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO",TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre);
                              

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    qtdeDEP = Convert.ToInt32(reader["QTDE_REPROV"]);
                }



                return qtdeDEP;
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