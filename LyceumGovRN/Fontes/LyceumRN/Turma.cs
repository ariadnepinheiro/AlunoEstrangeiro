namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.Library;
    using Techne.Lyceum.CR;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;
    using Seeduc.Infra.MapeamentoAtributos;
    using System.Data.SqlClient;
    using System.Configuration;

    public class Turma : RNBase
    {
        #region Classes auxiliares

        public enum TelaExclusaoAlocacao
        {
            QHI,
            EXC
        }

        public enum FuncaoDOC
        {
            DOCI,
            DOCII,
            DOCI_DOCII,
            NENHUM
        }

        [Serializable]
        public class DadosTurma
        {
            public string Ano { get; set; }

            [ParamentrosQueryString(Nome = "Semestre")]
            public string Periodo { get; set; }

            public string Curso { get; set; }

            [ParamentrosQueryString(Disponivel = false)]
            public string NomeCurso { get; set; }

            public string Turno { get; set; }

            public string Serie { get; set; }

            [ParamentrosQueryString(Disponivel = false)]
            public string NomeSerie { get; set; }

            public string Nucleo { get; set; }

            public string Regional { get; set; }

            public string Municipio { get; set; }

            public string Dependencia { get; set; }

            public string UnidadeResponsavel { get; set; }

            [ParamentrosQueryString(Disponivel = false)]
            public string NomeUnidade { get; set; }

            [ParamentrosQueryString(Nome = "PrefixoUnidadeResponsavel")]
            public string MnemonicoUnidadeResponsavel { get; set; }

            public string Grade { get; set; }

            public string Turma { get; set; }

            public string Curriculo { get; set; }

            public string Faculdade { get; set; }

            [ParamentrosQueryString(Nome = "GradeId")]
            public string Grade_ID { get; set; }

            public DateTime dtFim { get; set; }

            public DateTime dtInicio { get; set; }

            public string Sufixo { get; set; }

            public string Tipogestao { get; set; }

            [ParamentrosQueryString(Disponivel = false)]
            public string Ambienteexterno { get; set; }

            [ParamentrosQueryString(Disponivel = false)]
            public string OptativaReforco { get; set; }

            [ParamentrosQueryString(Disponivel = false)]
            public string SalaExterna { get; set; }

            [ParamentrosQueryString(Disponivel = false)]
            public string Eletiva { get; set; }

            [ParamentrosQueryString(Disponivel = false)]
            public string TurmaReferencia { get; set; }

            public string matriculadosprincipal { get; set; }

            public string matriculadoseletivas { get; set; }
        }

        public class DadosTurmaDisciplina
        {
            public int Ano { get; set; }

            public int Periodo { get; set; }

            public string Turma { get; set; }

            public string Disciplina { get; set; }

            public string DisciplinaMultipla { get; set; }

            public string Eletiva { get; set; }

            public string TurmaReferencia { get; set; }

            public int? Grupo { get; set; }
        }

        [Serializable]
        public class DadosQuadroHorario
        {
            public string Turno { get; set; }

            public string Faculdade { get; set; }

            public decimal DiaSemana { get; set; }

            public decimal Aula { get; set; }

            public string Disciplina { get; set; }

            public string Turma { get; set; }

            public decimal Ano { get; set; }

            public decimal Semestre { get; set; }

            public decimal? NumFunc { get; set; }

            public DateTime HoraIni { get; set; }

            public DateTime HoraFim { get; set; }

            public string Dependencia { get; set; }

            public DateTime DtIni { get; set; }

            public DateTime DtFim { get; set; }
        }

        public class TurmaError
        {
            public string Mensagem { get; set; }

            public int DiaDaSemana { get; set; }

            public DateTime HoraInicio { get; set; }

            public DateTime HoraFim { get; set; }

            public string TipoErro { get; set; }

            public string Matricula { get; set; }

            public string Disciplina { get; set; }

            public decimal Aula { get; set; }

            public bool Equals(TurmaError turma)
            {
                return Mensagem == turma.Mensagem &&
                    DiaDaSemana == turma.DiaDaSemana &&
                    HoraInicio.CompareTo(turma.HoraInicio) == 0 &&
                    HoraFim.CompareTo(turma.HoraFim) == 0 &&
                    TipoErro == turma.TipoErro &&
                    Matricula == turma.Matricula &&
                    Disciplina == turma.Disciplina &&
                    Aula == turma.Aula;
            }
        }

        public class TurmaErrorComparer : IEqualityComparer<TurmaError>
        {
            private static TurmaErrorComparer comparer;
            public static TurmaErrorComparer Default
            {
                get
                {
                    if (comparer == null)
                        comparer = new TurmaErrorComparer();
                    return comparer;
                }
            }

            #region IEqualityComparer<TurmaError> Members

            public bool Equals(TurmaError x, TurmaError y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(TurmaError obj)
            {
                return comparer.GetHashCode();
            }

            #endregion
        }

        private enum StatusQH
        {
            NAO_ALTERADO,
            SEM_ALOCACAO,
            HORARIO_INCOMPLETO,
            HORARIO_COMPLETO
        }

        public class ParametrosFechamento
        {
            public String UnidadeEnsino { get; set; }
            public String UnidadeResponsavel { get; set; }
            public String UnidadeFisica { get; set; }
            public String Turno { get; set; }
            public decimal? Ano { get; set; }
            public decimal? Periodo { get; set; }
            public String Disciplina { get; set; }
            public String Turma { get; set; }
            public String Curso { get; set; }
            public String Serie { get; set; }
            public String Opcao { get; set; }
            public char? IgnoraInconcluido { get; set; }

            private System.Web.SessionState.HttpSessionState session;
            public System.Web.SessionState.HttpSessionState Session
            {
                set { session = value; }
            }

            public float? Progresso
            {
                get
                {
                    return session != null ? (float?)session["prog_fechamento"] : (float?)null;
                }
                set
                {
                    if (session != null)
                        session["prog_fechamento"] = value;
                }
            }

            public ParametrosFechamento(System.Web.SessionState.HttpSessionState session)
            {
                this.Session = session;
            }
        }

        #endregion

        public List<string> ListaTurnosPor(DataContext contexto, string censo, int ano, int periodo, int serie, string curso, int serieRef, string cursoRef)
        {
            List<string> turnos = new List<string>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT TURNO
                            FROM LY_TURMA T
	                            INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
                            WHERE ANO = @ANO
	                            AND SEMESTRE = @PERIODO
	                            AND T.FACULDADE = @CENSO
	                            AND SERIE = @SERIE
	                            AND T.CURSO IN (SELECT CURSO
			                                        FROM LY_CURSO
			                                        WHERE ITINERARIOFORMATIVO IS NOT NULL 
				                                        AND ITINERARIOFORMATIVO = 'S' 
				                                        AND TRILHAAPRENDIZAGEMID IS NOT NULL
				                                        and TRILHAAPRENDIZAGEMID <> 31)
                                AND SIT_TURMA = 'Aberta' 
                                AND EXISTS (SELECT DISTINCT T2.FACULDADE
                                        FROM LY_TURMA T2
	                                        INNER JOIN LY_CURSO C ON T2.CURSO = C.CURSO
                                        WHERE ANO = @ANO
	                                        AND SEMESTRE = @PERIODO	                          
	                                        AND SERIE = @SERIEREF
	                                        AND T2.CURSO =  @CURSOREF
                                            AND SIT_TURMA = 'ABERTA'
								            AND T2.FACULDADE = T.FACULDADE)";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSOREF", SqlDbType.VarChar, cursoRef);
                contextQuery.Parameters.Add("@SERIEREF", SqlDbType.Int, serieRef);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    string turno = Convert.ToString(reader["TURNO"]);
                    turnos.Add(turno);
                }

                return turnos;
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

        private static DataTable ConsultarVagasDisponiveisNasTurmas(decimal ano, decimal periodo, string censo, string turno, string curso, string curriculo, decimal serie)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT  t.TURMA,
                            MAX(NUM_ALUNOS) AS 'MAXIMO_ALUNOS',
                            COUNT(DISTINCT mat.ALUNO) AS 'ALUNOS_MATRICULADOS',
                            COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'ALUNOS_RESERVADOS',
                            MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'VAGAS'
                    FROM    dbo.LY_TURMA t
                            LEFT JOIN ly_matricula mat ON mat.DISCIPLINA = t.DISCIPLINA
                                                          AND mat.TURMA = t.TURMA
                                                          AND mat.ANO = t.ANO
                                                          AND mat.SEMESTRE = t.SEMESTRE
                                                          AND mat.SIT_MATRICULA <> 'Cancelado'
                                                          AND ( mat.DEPENDENCIA <> 'S'
                                                               OR mat.DEPENDENCIA IS NULL )  
                            LEFT JOIN dbo.TCE_TRANSFERENCIA_DESTINO td ON t.FACULDADE = td.CENSO
                                                                          AND t.ANO = td.ANO
                                                                          AND t.SEMESTRE = td.PERIODO
                                                                          AND t.TURMA = td.TURMA
                                                                           AND ID_TRANSFERENCIA IN(SELECT ID_TRANSFERENCIA 
                                                                                                            FROM TCE_TRANSFERENCIA TRANSF 
														                                                    WHERE TRANSF.STATUS = 'PENDENTE')
                            LEFT JOIN dbo.TCE_TRANSFERENCIA tr ON td.ID_TRANSFERENCIA = tr.ID_TRANSFERENCIA
                                                                   AND tr.[STATUS] = @STATUS
                    WHERE   t.ANO = @ANO
                            AND t.SEMESTRE = @PERIODO
                            AND t.FACULDADE = @CENSO
                            AND t.TURNO = @TURNO
                            AND t.CURSO = @CURSO
                            AND t.CURRICULO = @CURRICULO
                            AND t.SERIE = @SERIE
                            AND t.SIT_TURMA = 'Aberta'
							AND ISNULL(T.ELETIVA, 'N') = 'N'
                    GROUP BY t.TURMA
                    HAVING  MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) > 0
                    ORDER BY t.TURMA ASC",
                    new ContextQueryParameter("@STATUS", SqlDbType.VarChar, Transferencia.Pendente),
                    new ContextQueryParameter("@ANO", TechneDbType.T_ANO, ano),
                    new ContextQueryParameter("@PERIODO", TechneDbType.T_SEMESTRE2, periodo),
                    new ContextQueryParameter("@CENSO", TechneDbType.T_CODIGO, censo),
                    new ContextQueryParameter("@TURNO", TechneDbType.T_CODIGO, turno),
                    new ContextQueryParameter("@CURSO", TechneDbType.T_CODIGO, curso),
                    new ContextQueryParameter("@CURRICULO", TechneDbType.T_CODIGO, curriculo),
                    new ContextQueryParameter("@SERIE", TechneDbType.T_NUMERO_PEQUENO, serie)));
        }

        public static DataTable ConsultarPrimeiraTurmaDisponivel(decimal ano, decimal periodo, string censo, string turno, string curso, decimal serie)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT top 1 t.TURMA,
                            MAX(NUM_ALUNOS) AS 'MAXIMO_ALUNOS',
                            COUNT(DISTINCT mat.ALUNO) AS 'ALUNOS_MATRICULADOS',
                            COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'ALUNOS_RESERVADOS',
                            MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'VAGAS'
                    FROM    dbo.LY_TURMA t
                            LEFT JOIN ly_matricula mat ON mat.DISCIPLINA = t.DISCIPLINA
                                                          AND mat.TURMA = t.TURMA
                                                          AND mat.ANO = t.ANO
                                                          AND mat.SEMESTRE = t.SEMESTRE
                                                          AND mat.SIT_MATRICULA <> 'Cancelado'                                                          
                                                          AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)
                            LEFT JOIN dbo.TCE_TRANSFERENCIA_DESTINO td ON t.FACULDADE = td.CENSO
                                                                          AND t.ANO = td.ANO
                                                                          AND t.SEMESTRE = td.PERIODO
                                                                          AND t.TURMA = td.TURMA
                                                                            AND ID_TRANSFERENCIA IN(SELECT ID_TRANSFERENCIA 
                                                                                                            FROM TCE_TRANSFERENCIA TRANSF 
														                                                    WHERE TRANSF.STATUS = 'PENDENTE')
                            LEFT JOIN dbo.TCE_TRANSFERENCIA tr ON td.ID_TRANSFERENCIA = tr.ID_TRANSFERENCIA
                                                                   AND tr.[STATUS] = @STATUS
                    WHERE   t.ANO = @ANO
                            AND t.SEMESTRE = @PERIODO
                            AND t.FACULDADE = @CENSO
                            AND t.TURNO = @TURNO
                            AND t.CURSO = @CURSO                           
                            AND t.SERIE = @SERIE
                            AND t.SIT_TURMA = 'Aberta'
                            AND t.OPTATIVAREFORCO = 'N'
                            AND ISNULL(T.ELETIVA,'N') = 'N'
                    GROUP BY t.TURMA
                    HAVING  MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) > 0
                    ORDER BY t.TURMA ASC",
                    new ContextQueryParameter("@STATUS", Transferencia.Pendente),
                    new ContextQueryParameter("@ANO", ano),
                    new ContextQueryParameter("@PERIODO", periodo),
                    new ContextQueryParameter("@CENSO", censo),
                    new ContextQueryParameter("@TURNO", turno),
                    new ContextQueryParameter("@CURSO", curso),
                    new ContextQueryParameter("@SERIE", serie)));
        }

        public static DataTable ConsultarPrimeiraTurmaDisponivelDiferente(decimal ano, decimal periodo, string censo, string turno, string curso, decimal serie, string turma)
        {
            return Consultar(
                new ContextQuery(
                    @"SELECT top 1 t.TURMA,
                            MAX(NUM_ALUNOS) AS 'MAXIMO_ALUNOS',
                            COUNT(DISTINCT mat.ALUNO) AS 'ALUNOS_MATRICULADOS',
                            COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'ALUNOS_RESERVADOS',
                            MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'VAGAS'
                    FROM    dbo.LY_TURMA t
                            LEFT JOIN ly_matricula mat ON mat.DISCIPLINA = t.DISCIPLINA
                                                          AND mat.TURMA = t.TURMA
                                                          AND mat.ANO = t.ANO
                                                          AND mat.SEMESTRE = t.SEMESTRE
                                                          AND mat.SIT_MATRICULA <> 'Cancelado'                                                          
                                                          AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)
                            LEFT JOIN dbo.TCE_TRANSFERENCIA_DESTINO td ON t.FACULDADE = td.CENSO
                                                                          AND t.ANO = td.ANO
                                                                          AND t.SEMESTRE = td.PERIODO
                                                                          AND t.TURMA = td.TURMA
                                                                            AND ID_TRANSFERENCIA IN(SELECT ID_TRANSFERENCIA 
                                                                                                            FROM TCE_TRANSFERENCIA TRANSF 
														                                                    WHERE TRANSF.STATUS = 'PENDENTE')
                            LEFT JOIN dbo.TCE_TRANSFERENCIA tr ON td.ID_TRANSFERENCIA = tr.ID_TRANSFERENCIA
                                                                   AND tr.[STATUS] = @STATUS
                    WHERE   t.ANO = @ANO
                            AND t.SEMESTRE = @PERIODO
                            AND t.FACULDADE = @CENSO
                            AND t.TURNO = @TURNO
                            AND t.CURSO = @CURSO                           
                            AND t.SERIE = @SERIE                          
                            AND t.TURMA <> @TURMA
                            AND t.SIT_TURMA = 'Aberta'
                            AND t.OPTATIVAREFORCO = 'N'
                            AND ISNULL(T.ELETIVA,'N') = 'N'
                    GROUP BY t.TURMA
                    HAVING  MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) > 0
                    ORDER BY t.TURMA ASC",
                    new ContextQueryParameter("@STATUS", Transferencia.Pendente),
                    new ContextQueryParameter("@ANO", ano),
                    new ContextQueryParameter("@PERIODO", periodo),
                    new ContextQueryParameter("@CENSO", censo),
                    new ContextQueryParameter("@TURNO", turno),
                    new ContextQueryParameter("@CURSO", curso),
                    new ContextQueryParameter("@TURMA", turma),
                    new ContextQueryParameter("@SERIE", serie)));
        }

        public string ObtemPrimeiraTurmaComVagaPor(DataContext contexto, int idControleVaga)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT top 1 t.TURMA,
                            MAX(NUM_ALUNOS) AS 'MAXIMO_ALUNOS',
                            COUNT(DISTINCT mat.ALUNO) AS 'ALUNOS_MATRICULADOS',
                            COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'ALUNOS_RESERVADOS',
                            MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'VAGAS'
                    FROM    dbo.LY_TURMA t
							INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) ON t.ANO = CV.ANO
																	AND t.SEMESTRE = CV.PERIODO
																	AND t.FACULDADE = CV.CENSO
																	AND t.TURNO = CV.TURNO
																	AND t.CURSO = CV.CURSO
																	AND t.SERIE = CV.SERIE
                            LEFT JOIN ly_matricula mat ON mat.DISCIPLINA = t.DISCIPLINA
                                                          AND mat.TURMA = t.TURMA
                                                          AND mat.ANO = t.ANO
                                                          AND mat.SEMESTRE = t.SEMESTRE
                                                          AND mat.SIT_MATRICULA <> 'Cancelado'                                                          
                                                          AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)
                            LEFT JOIN dbo.TCE_TRANSFERENCIA_DESTINO td ON t.FACULDADE = td.CENSO
                                                                          AND t.ANO = td.ANO
                                                                          AND t.SEMESTRE = td.PERIODO
                                                                          AND t.TURMA = td.TURMA
                                                                            AND ID_TRANSFERENCIA IN(SELECT ID_TRANSFERENCIA 
                                                                                                            FROM TCE_TRANSFERENCIA TRANSF 
														                                                    WHERE TRANSF.STATUS = 'PENDENTE')
                            LEFT JOIN dbo.TCE_TRANSFERENCIA tr ON td.ID_TRANSFERENCIA = tr.ID_TRANSFERENCIA
                                                                   AND tr.[STATUS] = @STATUS
                    WHERE   CV.ID_CONTROLE_VAGA = @ID_CONTROLE_VAGA
                            AND t.SIT_TURMA = 'Aberta'
                            AND t.OPTATIVAREFORCO = 'N'
                            AND ISNULL(T.ELETIVA,'N') = 'N'
                    GROUP BY t.TURMA
                    HAVING  MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) > 0
                    ORDER BY t.TURMA ASC ";

            contextQuery.Parameters.Add("@ID_CONTROLE_VAGA", SqlDbType.Int, idControleVaga);
            contextQuery.Parameters.Add("@STATUS", SqlDbType.VarChar, Transferencia.Pendente);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemPrimeiraTurmaComVagaPor(DataContext contexto, int ano, int periodo, string censo, string turno, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @"  SELECT top 1 t.TURMA,
                            MAX(NUM_ALUNOS) AS 'MAXIMO_ALUNOS',
                            COUNT(DISTINCT mat.ALUNO) AS 'ALUNOS_MATRICULADOS',
                            COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'ALUNOS_RESERVADOS',
                            MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'VAGAS'
                    FROM    dbo.LY_TURMA t
                            LEFT JOIN ly_matricula mat ON mat.DISCIPLINA = t.DISCIPLINA
                                                          AND mat.TURMA = t.TURMA
                                                          AND mat.ANO = t.ANO
                                                          AND mat.SEMESTRE = t.SEMESTRE
                                                          AND mat.SIT_MATRICULA <> 'Cancelado'                                                          
                                                          AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)
                            LEFT JOIN dbo.TCE_TRANSFERENCIA_DESTINO td ON t.FACULDADE = td.CENSO
                                                                          AND t.ANO = td.ANO
                                                                          AND t.SEMESTRE = td.PERIODO
                                                                          AND t.TURMA = td.TURMA
                                                                            AND ID_TRANSFERENCIA IN(SELECT ID_TRANSFERENCIA 
                                                                                                            FROM TCE_TRANSFERENCIA TRANSF 
														                                                    WHERE TRANSF.STATUS = 'PENDENTE')
                            LEFT JOIN dbo.TCE_TRANSFERENCIA tr ON td.ID_TRANSFERENCIA = tr.ID_TRANSFERENCIA
                                                                   AND tr.[STATUS] = @STATUS
                    WHERE   t.ANO = @ANO
							AND t.SEMESTRE = @PERIODO
							AND t.FACULDADE = @CENSO
							AND t.TURNO = @TURNO
							AND t.CURSO = @CURSO
							AND t.SERIE = @SERIE
                            AND t.SIT_TURMA = 'Aberta'
                            AND t.OPTATIVAREFORCO = 'N'
                            AND ISNULL(T.ELETIVA,'N') = 'N'
                    GROUP BY t.TURMA
                    HAVING  MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) > 0
                    ORDER BY t.TURMA ASC ";

            contextQuery.Parameters.Add("@STATUS", SqlDbType.VarChar, Transferencia.Pendente);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemPrimeiraTurmaPor(DataContext contexto, int ano, int periodo, string censo, string turno, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @"  SELECT TOP 1 T.TURMA
                    FROM    DBO.LY_TURMA T (NOLOCK)
                    WHERE   T.ANO = @ANO
							AND T.SEMESTRE = @PERIODO
							AND T.FACULDADE = @CENSO
							AND T.TURNO = @TURNO
							AND T.CURSO = @CURSO
							AND T.SERIE = @SERIE
                            AND T.SIT_TURMA = 'Aberta'
                            AND T.OPTATIVAREFORCO = 'N'
                            AND ISNULL(T.ELETIVA,'N') = 'N'
                    ORDER BY T.TURMA ASC ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public static bool EnturmarAluno(TceConfirmacaoMatricula confirmacaoMatricula, out string turma, out string mensagem)
        {
            RN.Matricula rnMatricula = new Matricula();
            RN.Turma rnTurma = new Turma();
            RN.Disciplina rnDisciplina = new Disciplina();

            if (confirmacaoMatricula == null)
            {
                turma = string.Empty;
                mensagem = "Confirmação de matrícula vazia!";

                return false;
            }

            var turmasComVagas = ConsultarVagasDisponiveisNasTurmas(
                confirmacaoMatricula.Ano,
                confirmacaoMatricula.Periodo,
                confirmacaoMatricula.Censo,
                confirmacaoMatricula.Turno,
                confirmacaoMatricula.Curso,
                confirmacaoMatricula.Curriculo,
                confirmacaoMatricula.Serie);

            if (turmasComVagas.Rows.Count == 0)
            {
                turma = string.Empty;
                mensagem = "Não existem turmas com vagas disponíveis.";
                return false;
            }

            var turmaSelecionada = Convert.ToString(turmasComVagas.Rows[0]["TURMA"]);
            var disciplinas = Matricula.ConsultaDisciplinaGrade(Convert.ToString(confirmacaoMatricula.Ano), Convert.ToString(confirmacaoMatricula.Periodo), turmaSelecionada);

            if (disciplinas.Rows.Count == 0)
            {
                var disciplinasTurma = rnTurma.ObtemDisciplinasTurmaPor(Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo), turmaSelecionada);

                turma = string.Empty;
                mensagem = string.Format("Turma com situação {0} e {1} disciplina(s). Entre em contato com o Administrador do Sistema repassando esta mensagem(Confirmação de Matricula) ou tente novamente mais tarde.", disciplinasTurma.Count() == 0 ? "NÃO ENCONTRADA" : disciplinasTurma.Select(x => x.SitTurma).First(), disciplinasTurma.Count().ToString());

                return false;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    foreach (DataRow disciplinaRow in disciplinas.Rows)
                    {
                        var disciplina = disciplinaRow["disciplina"].ToString();

                        if (HistMatricula.ExisteHistorico(confirmacaoMatricula.Aluno, disciplina, turmaSelecionada, confirmacaoMatricula.Ano, confirmacaoMatricula.Periodo))
                        {
                            turma = string.Empty;
                            mensagem = string.Format(
                                "Já existe histórico do aluno {0} para a disciplina {1} da turma {2}",
                                confirmacaoMatricula.Aluno,
                                disciplina,
                                turmaSelecionada);

                            ctx.Abandon();
                            return false;
                        }

                        //Verifica se a disciplina não é uma eletiva com enturmação separada
                        if (!rnDisciplina.EhDisciplinaGradeEletivaPor(ctx, confirmacaoMatricula.Ano, confirmacaoMatricula.Periodo, turmaSelecionada, disciplina))
                        {
                            rnMatricula.InsereouAtualizaMatriculaPrincipal(ctx, confirmacaoMatricula, turmaSelecionada, disciplina);
                        }
                    }

                    if (!rnMatricula.PossuiMatriculaAtivaNaTurmaPorAluno(ctx, confirmacaoMatricula.Aluno, turmaSelecionada, confirmacaoMatricula.Ano, confirmacaoMatricula.Periodo))
                    {
                        var disciplinasTurma = rnTurma.ObtemDisciplinasTurmaPor(Convert.ToInt32(confirmacaoMatricula.Ano), Convert.ToInt32(confirmacaoMatricula.Periodo), turmaSelecionada);

                        turma = string.Empty;
                        mensagem = string.Format("Turma com situação {0} e {1} disciplina(s). Entre em contato com o Administrador do Sistema repassando esta mensagem(Confirmação de Matricula) ou tente novamente mais tarde.", disciplinasTurma.Count() == 0 ? "NÃO ENCONTRADA" : disciplinasTurma.Select(x => x.SitTurma).First(), disciplinasTurma.Count().ToString());

                        ctx.Abandon();
                        return false;
                    }

                    var gradeId = GradeSerie.ObterGradeId(
                        ctx,
                        confirmacaoMatricula.Ano,
                        confirmacaoMatricula.Periodo,
                        confirmacaoMatricula.Curso,
                        confirmacaoMatricula.Curriculo,
                        confirmacaoMatricula.Serie,
                        turmaSelecionada);

                    if (gradeId.IsNullOrEmptyOrWhiteSpace())
                    {
                        turma = string.Empty;
                        mensagem = string.Format("Grade série não encontrada para Turma {0} selecionada.", turmaSelecionada);

                        ctx.Abandon();
                        return false;
                    }
                    else
                    {
                        ctx.ApplyModifications(
                            new ContextQuery(
                                string.Format(
                                    @"DECLARE @aluno T_CODIGO,
                                    @grade_id T_NUMERO_GRANDE,
                                    @sit_matgrade T_SIT_MATGRADE,
									@curriculo 	 T_CODIGO	
                                                                		
                                SET @aluno = '{0}'
                                SET @grade_id = {1}
                                SET @curriculo = '{2}'
                                SET @sit_matgrade = 'Matriculado'

                                IF NOT EXISTS ( SELECT  *
                                                FROM    LY_MATGRADE
                                                WHERE   ALUNO = @aluno
                                                        AND GRADE_ID = @grade_id
                                                        AND SIT_MATGRADE = @sit_matgrade ) 
                                    INSERT  INTO LY_MATGRADE
                                            (
                                              ALUNO,
                                              GRADE_ID,
                                              SIT_MATGRADE,
                                              DT_ULTALT
                                            )
                                    VALUES  (
                                              @aluno,
                                              @grade_id,
                                              @sit_matgrade,
                                              GETDATE()
                                            )

                                UPDATE  LY_MATGRADE
                                SET     SIT_MATGRADE = 'Cancelado'
                                WHERE   ALUNO = @aluno
                                        AND GRADE_ID <> @grade_id
                                        AND SIT_MATGRADE = 'Matriculado'

                                UPDATE LY_ALUNO SET
                                CURRICULO = @CURRICULO
                                WHERE   ALUNO = @aluno
                                AND CURRICULO <> @CURRICULO
                                    ",
                                    confirmacaoMatricula.Aluno,
                                    gradeId,
                                    confirmacaoMatricula.Curriculo)));
                    }
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }

            turma = turmaSelecionada;
            mensagem = string.Empty;

            return true;
        }

        // Insere a turma com o quadro de horários definidos pelo usuário
        public static RetValue IncluirTurmaComQuadroHorario(Ly_turma dtTurma, string macro)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            RetValue valorRetorno = null;

            ErrorList errorList = new ErrorList();

            DataSet validacaoDados = ValidaInclusaoDeTurmaQuadroDeHorario(dtTurma);

            try
            {
                if (validacaoDados.Tables[0].Rows.Count > 0 && Convert.ToInt32(validacaoDados.Tables[0].Rows[0][0]) != 1)
                {
                    return new RetValue(false, "Curso não autorizado para unidade de ensino.", null);
                }

                //Verifica se é optativa reforço
                if (dtTurma.Rows[0].OptativaReforco == "N" && dtTurma.Rows[0].Eletiva == "N")
                {
                    if (validacaoDados.Tables[2].Rows.Count > 0 && Convert.ToInt32(validacaoDados.Tables[2].Rows[0][0]) == 1)
                    {
                        return new RetValue(false, "Conflito de dependencia.", null);
                    }
                }

                if (dtTurma.Rows[0].OptativaReforco == "N" && dtTurma.Rows[0].Eletiva == "N")
                {

                    if (RN.Turma.VerificarConflitoDependencia(connection, dtTurma.Rows[0].Faculdade, dtTurma.Rows[0].Dependencia, dtTurma.Rows[0].Dt_inicio.Value, dtTurma.Rows[0].Dt_fim.Value, dtTurma.Rows[0].Turma, dtTurma.Rows[0].Turno))
                    {
                        connection.Rollback();
                        return new RetValue(false, null, new ErrorList("Conflito de dependencia."));
                    }
                }

                if (validacaoDados.Tables[1].Rows.Count > 0 && Convert.ToInt32(validacaoDados.Tables[1].Rows[0][0]) == 1)
                {
                    return new RetValue(false, "Série extinta.", null);
                }

                if (dtTurma.Rows[0].Dt_inicio.Value.Year != dtTurma.Rows[0].Ano)
                {
                    errorList.Add("Início das Aulas deve ser do ano " + dtTurma.Rows[0].Ano + ".", "ERRO");
                    return new RetValue(false, "", errorList);
                }

                if (dtTurma.Rows[0].Dt_fim.Value.Year != dtTurma.Rows[0].Ano)
                {
                    errorList.Add("Término das Aulas deve ser do ano " + dtTurma.Rows[0].Ano + ".", "ERRO");
                    return new RetValue(false, "", errorList);
                }

                //	Inclusão de turma
                valorRetorno = Incluir(connection, dtTurma, macro);

                if (valorRetorno != null)
                {
                    connection.Rollback();
                    return valorRetorno;
                }
            }
            catch (Exception ex)
            {
                return new RetValue(false, ex.Message, errorList);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }

            return new RetValue(true, "Registro incluído com sucesso.", errorList);

        }

        private static StatusQH VerificarAlocacao(TConnection connection, Ly_turma.Row dadosTurma)
        {
            //Obtém a quantidade de aulas semanais necessárias a serem cumpridas naquela grade
            QueryTable qtDisciplinasGrade = ObterDisciplinaGrade(connection, dadosTurma.Curso, dadosTurma.Turno, dadosTurma.Curriculo, dadosTurma.Serie.Value);
            Dictionary<string, decimal> aulasSemanais = new Dictionary<string, decimal>();

            if (qtDisciplinasGrade != null)
            {
                aulasSemanais = qtDisciplinasGrade
                    .Rows.Cast<SimpleRow>()
                    .Select(sr => new
                    {
                        Disciplina = Convert.ToString(sr["disciplina"]),
                        NomeDisciplina = Convert.ToString(sr["nome"]),
                        AulasSemanais = sr["aulas_semanais"].IsNull ? 0M : Convert.ToDecimal(sr["aulas_semanais"])
                    })
                    .OrderBy(a => a.Disciplina)
                    .ToDictionary(s => s.Disciplina, s => s.AulasSemanais);
            }

            QueryTable qtAulaDocente = new QueryTable(
                @"  SELECT ad.disciplina
                    FROM ly_aula_docente ad
                    INNER JOIN ly_turma t (NOLOCK) ON
                        ad.disciplina = t.disciplina AND
                        ad.turma = t.turma AND
                        ad.ano = t.ano AND
                        ad.semestre = t.semestre AND
                        ad.data_fim = t.dt_fim
                    WHERE                        
                        ad.turno = ? AND 
                        ad.faculdade = ? AND 
                        ad.turma = ? AND 
                        ad.ano = ? AND 
                        ad.semestre = ? AND 
                        ad.data_inicio <= ? AND 
                        ad.data_fim >= ? AND
                        t.sit_turma = 'Aberta'");
            qtAulaDocente.Query(connection, dadosTurma.Turno, dadosTurma.Faculdade, dadosTurma.Turma, dadosTurma.Ano.Value, dadosTurma.Semestre.Value, dadosTurma.Dt_fim, dadosTurma.Dt_inicio);

            //Obtém a quantidade de aulas semanais alocadas
            var aulasSemanaisDadas = qtAulaDocente
                .Rows.Cast<SimpleRow>()
                .Select(ha => new { Disciplina = Convert.ToString(ha["DISCIPLINA"]) })
                .GroupBy(h => h.Disciplina)
                .Select(gh => new { Disciplina = gh.Key, AulasSemanaisDadas = Convert.ToDecimal(gh.Count()) })
                .OrderBy(gh => gh.Disciplina)
                .ToDictionary(g => g.Disciplina, g => g.AulasSemanaisDadas);

            if (aulasSemanaisDadas.Count() == 0)
            {
                return StatusQH.SEM_ALOCACAO;
            }

            //Compara a quantidade de aulas semanais dadas com a quantidade de aulas necesárias da grade
            foreach (var aulaSemanal in aulasSemanais)
            {
                decimal qtdeAulasSemanaisGrade = aulaSemanal.Value;
                decimal qtdeAulasSemanaisTurma = aulasSemanaisDadas.Keys.Contains(aulaSemanal.Key) ? aulasSemanaisDadas[aulaSemanal.Key] : 0M;
                if (qtdeAulasSemanaisGrade > qtdeAulasSemanaisTurma)
                {
                    return StatusQH.HORARIO_INCOMPLETO;
                }
            }
            return StatusQH.HORARIO_COMPLETO;
        }

        /// <summary>
        /// Atualiza o status do quadro de horário da turma (LY_TURMA.EM_ELABORACAO).
        /// </summary>        
        /// <param name="dtTurma">DataTable contendo os registros de LY_TURMA a terem o status atualizado.</param>
        /// <param name="statusQH">Status do quadro de horário da turma. 
        ///     -   Horário incompleto: em_elaboracao = "S"; 
        ///     -   Horário completo:   em_elaboracao = "N"
        ///     -   Sem alocação:       em_elaboracao = null</param>
        /// <returns>Mensagem de Sucesso/Erro.</returns>
        private static RetValue AtualizarStatusQH(TConnectionWritable connection, Ly_turma dtTurma, StatusQH statusQH)
        {
            DbObject status;
            switch (statusQH)
            {
                case StatusQH.HORARIO_INCOMPLETO: status = "S"; break;
                case StatusQH.HORARIO_COMPLETO: status = "N"; break;
                case StatusQH.SEM_ALOCACAO:
                default:
                    status = DBNull.Value; break;
            }

            if (dtTurma != null && dtTurma.Rows.Count > 0 && statusQH != StatusQH.NAO_ALTERADO)
            {
                foreach (Ly_turma.Row linhaTurma in dtTurma.Rows)
                {
                    if (!string.IsNullOrEmpty(linhaTurma.Disciplina))
                    {
                        Ly_turma.Row.Update(connection, linhaTurma.Disciplina, linhaTurma.Turma, linhaTurma.Ano, linhaTurma.Semestre, "em_elaboracao", status);
                        RetValue valor = VerificarErro(connection.GetErrors());
                        if (valor != null)
                        {
                            return valor;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Verifica se disciplinas do quadro de horário estão contidas na grade
        /// </summary>
        /// <param name="connection">conexão com banco</param>
        /// <param name="dtHorAula">Datatable com os dados do quadro de horário para horario operacional</param>
        /// <param name="qtGrade">Dados da grade de horário com as disciplinas que deverão estar no quadro de horário</param>
        /// <returns>Error list com as disciplinas que não atenderam sua parametrizacao e/ou não foram usadas no quadro de horário.</returns>
        private static ErrorList VerificarDisciplina(TConnection connection, Ly_hor_aula dtHorAula, QueryTable qtGrade)
        {
            ErrorList errorDisciplina = null;
            //verifica se a função retornou dados
            if (qtGrade != null && qtGrade.Rows.Count > 0)
            {
                //verifica se existem dados no datatable de horario operacional
                if (dtHorAula != null && dtHorAula.Rows.Count > 0)
                {
                    //loop nos registro de grade
                    foreach (SimpleRow srGrade in qtGrade.Rows)
                    {
                        //obtém o código da disciplina
                        string disciplina = Convert.ToString(srGrade["disciplina"]);

                        //verifica se retornou algum valor
                        if (!string.IsNullOrEmpty(disciplina))
                        {
                            //consulta no datatable se existe a disciplina da grade
                            Ly_hor_aula.Row[] consultaDisciplina = dtHorAula.Select("disciplina = '" + RN.RNBase.MudarAspas(disciplina) + "'");

                            if (consultaDisciplina != null)
                            {
                                int numeroAulaQuadroHorario = consultaDisciplina.Length;

                                if (numeroAulaQuadroHorario == 0)
                                {
                                    int numeroAulaSemanalDisciplina = 0;

                                    if (!srGrade["aulas_semanais"].IsNull)
                                    {
                                        numeroAulaSemanalDisciplina = Convert.ToInt32(srGrade["aulas_semanais"]);
                                    }

                                    if (errorDisciplina == null)
                                    {
                                        errorDisciplina = new ErrorList();
                                    }

                                    //caso entre na condição de não existir, será armazenado qual disciplina não está sendo
                                    //usada no quadro de horário
                                    errorDisciplina.Add("Falta(m) " + numeroAulaSemanalDisciplina + " aula(s) de " + Convert.ToString(srGrade["nome"]) + ".", "ERRO");
                                }
                                else
                                {
                                    //será verificado se o número de disciplinas incluídas no quadro de horários está igual ao parametrizado em disciplinas

                                    int numeroAulaSemanalDisciplina = 0;

                                    if (!srGrade["aulas_semanais"].IsNull)
                                        numeroAulaSemanalDisciplina = Convert.ToInt32(srGrade["aulas_semanais"]);

                                    if (numeroAulaQuadroHorario < numeroAulaSemanalDisciplina)
                                    {
                                        if (errorDisciplina == null)
                                            errorDisciplina = new ErrorList();

                                        errorDisciplina.Add("Falta(m) " + (numeroAulaSemanalDisciplina - numeroAulaQuadroHorario) + " aula(s) de " + Convert.ToString(srGrade["nome"]) + ".", "ERRO");
                                    }
                                    else if (numeroAulaQuadroHorario > numeroAulaSemanalDisciplina)
                                    {
                                        if (errorDisciplina == null)
                                        {
                                            errorDisciplina = new ErrorList();
                                        }

                                        errorDisciplina.Add("Existe(m) " + (numeroAulaQuadroHorario - numeroAulaSemanalDisciplina) + " aula(s) de " + Convert.ToString(srGrade["nome"]) + " excedendo o quantitativo da matriz curricular.", "ERRO");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return errorDisciplina;
        }

        /// <summary>
        /// Atualiza os registros de aula docente que foram excluídos
        /// </summary>
        /// <param name="connection">conexão com a base</param>
        /// <param name="dtAulaDocenteExcluido">datatable das aulas que foram filtradas sem disciplinas</param>
        /// <param name="dtInicioTurma">data inicio da turma</param>
        /// <returns>RetValue nulo se não ocorreu nenhum erro</returns>
        private static RetValue AtualizarAulaDocente(TConnectionWritable connection, Ly_aula_docente dtAulaDocenteExcluido, DateTime dtInicioTurma)
        {
            RetValue valorRetorno = null;
            if (dtAulaDocenteExcluido != null)
            {
                foreach (Ly_aula_docente.Row linha in dtAulaDocenteExcluido.Rows)
                {
                    valorRetorno = ExcluirAulaDocente(connection, linha, dtInicioTurma);
                    if (valorRetorno != null)
                    {
                        return valorRetorno;
                    }
                }
            }
            return valorRetorno;
        }

        /// <summary>
        /// Atualiza os registros de aula docente que foram excluídos
        /// </summary>
        /// <param name="connection">conexão com a base</param>
        /// <param name="dtAulaDocenteExcluido">datatable das aulas que foram filtradas sem disciplinas</param>
        /// <returns>RetValue nulo se não ocorreu nenhum erro</returns>
        private static RetValue AtualizarAulaDocenteQHI(TConnectionWritable connection, Ly_aula_docente dtAulaDocenteExcluido, DateTime dtInicioTurma)
        {
            RetValue valorRetorno = null;
            if (dtAulaDocenteExcluido != null)
            {
                foreach (Ly_aula_docente.Row linha in dtAulaDocenteExcluido.Rows)
                {
                    valorRetorno = ExcluirAulaDocenteQHI(connection, linha, dtInicioTurma);
                    if (valorRetorno != null)
                        return valorRetorno;
                }
            }
            return valorRetorno;
        }

        private static RetValue ExcluirAulaDocenteQHI(TConnectionWritable connection, Ly_aula_docente.Row linha, DateTime dtInicioTurma)
        {
            RetValue valorRetorno = null;

            //instancia a data com o valor da data atual
            DateTime dtFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

            //caso a data de fim da turma seja maior que a data de hoje, será usado a data final da turma
            if (dtFim < dtInicioTurma)
            {
                dtFim = dtInicioTurma;
            }

            Ly_aula_docente.Row.Update(connection, linha.Num_func, linha.Turno, linha.Faculdade, linha.Dia_semana, linha.Aula, linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, linha.Data_inicio, "DATA_FIM, TIPO", dtFim, TelaExclusaoAlocacao.QHI.ToString());
            valorRetorno = VerificarErro(connection.GetErrors());
            if (valorRetorno != null)
            {
                return valorRetorno;
            }

            return valorRetorno;
        }

        /// <summary>
        /// Atualiza os registros de aula docente tipo que foram excluídos
        /// </summary>
        /// <param name="connection">conexão com a base</param>
        /// <param name="dtAulaDocenteTipoExcluido">datatable das aulas que foram filtradas sem disciplinas</param>
        /// <returns>RetValue nulo se não ocorreu nenhum erro</returns>
        private static RetValue AtualizarAulaDocenteTipo(TConnectionWritable connection, Ly_aula_docente_tipo.Row[] dtAulaDocenteTipoExcluido, DateTime dtInicioTurma)
        {
            RetValue valorRetorno = null;
            if (dtAulaDocenteTipoExcluido != null)
            {
                foreach (Ly_aula_docente_tipo.Row linha in dtAulaDocenteTipoExcluido)
                {
                    valorRetorno = ExcluirAulaDocenteTipo(connection, linha, dtInicioTurma);
                    valorRetorno = VerificarErro(connection.GetErrors());
                    if (valorRetorno != null)
                    {
                        return valorRetorno;
                    }
                }
            }

            return valorRetorno;
        }

        private static RetValue ExcluirAulaDocenteTipo(TConnectionWritable connection, Ly_aula_docente_tipo.Row linha, DateTime dtInicioTurma)
        {
            //instancia a data com o valor da data atual
            DateTime dtFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

            //caso a data de fim da turma seja maior que a data de hoje, será usado a data final da turma
            if (dtFim < dtInicioTurma)
            {
                dtFim = dtInicioTurma;
            }

            Ly_aula_docente_tipo.Row.Update(connection, linha.Num_func, linha.Turno, linha.Faculdade, linha.Dia_semana, linha.Aula, linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, linha.Data_inicio, "DATA_FIM, TIPO_AULA", dtFim, "NGLP");
            RetValue valorRetorno = VerificarErro(connection.GetErrors());
            if (valorRetorno != null)
            {
                return valorRetorno;
            }

            Ly_docente_funcao_glp.Row rowSolicitacao = Ly_docente_funcao_glp.Row.Query(connection, linha.Id_docente_funcao_glp);
            if (rowSolicitacao != null)
            {
                if (rowSolicitacao.Glp_usada > 0)
                {
                    Ly_docente_funcao_glp.Row.Update(connection, linha.Id_docente_funcao_glp, "glp_usada", rowSolicitacao.Glp_usada - 1);
                    valorRetorno = VerificarErro(connection.GetErrors());
                    if (valorRetorno != null)
                    {
                        return valorRetorno;
                    }
                }
            }

            return valorRetorno;
        }

        private static RetValue ExcluirAulaDocente(TConnectionWritable connection, Ly_aula_docente.Row linha, DateTime dtInicioTurma)
        {
            //instancia a data com o valor da data atual
            DateTime dtFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

            //caso a data de fim da turma seja maior que a data de hoje, será usado a data final da turma
            if (dtFim < dtInicioTurma)
                dtFim = dtInicioTurma;

            int rowsAffected = TCommand.ExecuteNonQuery(connection, @"
                UPDATE  ly_aula_docente
                SET     data_fim = ?
                WHERE   num_func = ? AND
                        turno = ? AND
                        faculdade = ? AND
                        dia_semana = ? AND
                        aula = ? AND
                        disciplina = ? AND
                        turma = ? AND
                        ano = ? AND
                        semestre = ? AND
                        data_inicio = ?",
                dtFim,
                linha.Num_func, linha.Turno, linha.Faculdade, linha.Dia_semana,
                linha.Aula, linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, linha.Data_inicio);

            return VerificarErro(connection.GetErrors());
        }

        /// <summary>
        /// Obtém datatable com hora aula atual
        /// </summary>
        /// <param name="dtHorAulaAtual">Datatable com os dados atuais de hora aula</param>
        /// <returns>Datatable com os dados obtidos</returns>
        private static Ly_hor_aula ObterHorAula(Ly_hor_aula dtHorAulaAtual)
        {
            Ly_hor_aula dtHorAula = new Ly_hor_aula();
            foreach (Ly_hor_aula.Row linha in dtHorAulaAtual.Rows)
            {
                if (!string.IsNullOrEmpty(linha.Disciplina))
                {
                    Ly_hor_aula.Row linhaHorAula = dtHorAula.NewRow();
                    PopularLinhaHorAula(linha, linhaHorAula);
                    dtHorAula.Rows.Add(linhaHorAula);
                }
            }
            return dtHorAula;
        }

        /// <summary>
        /// Atualiza o valor da data final para os datatables de aula docente e aula docente tipo que foram excluídos (disciplina nula)
        /// </summary>
        /// <param name="connection">conexão com a base</param>
        /// <param name="dtAulaDocenteAtual">Datatable aula docente atual com todo o quadro de horario relacionado a turma atual</param>
        /// <param name="dtAulaDocenteOriginal">Datatable aula docente original, antes das alterações da turma atual</param>
        /// <param name="dtAulaDocenteTipo">Datatable aula docente tipo original, antes das alterações da turma atual</param>
        /// <returns>RetValue nulo quando não ocorrer erro</returns>
        public static RetValue AtualizarAulaDocenteExcluido(TConnectionWritable connection, Ly_aula_docente dtAulaDocenteAtual, Ly_aula_docente dtAulaDocenteOriginal, Ly_aula_docente_tipo dtAulaDocenteTipo, DateTime dtInicioTurma)
        {
            RetValue valorRetorno = null;
            Ly_aula_docente dtAulaDocenteExcluido = ObterDadoAulaDocenteExcluido(dtAulaDocenteAtual, dtAulaDocenteOriginal);

            List<Ly_aula_docente_tipo.Row> dtAulaDocenteTipoExcluido = new List<Ly_aula_docente_tipo.Row>();
            foreach (Ly_aula_docente.Row ad in dtAulaDocenteExcluido.Rows)
            {
                dtAulaDocenteTipoExcluido.AddRange(dtAulaDocenteTipo.Select(
                    String.Format("num_func = '{0}' and dia_semana = '{1}' and aula = '{2}' and disciplina = '{3}' and turma = '{4}' and ano = '{5}' and semestre = '{6}'",
                    ad.Num_func, ad.Dia_semana, ad.Aula, ad.Disciplina, ad.Turma.Replace("'", "''"), ad.Ano, ad.Semestre)));
            }

            valorRetorno = AtualizarAulaDocenteTipo(connection, dtAulaDocenteTipoExcluido.ToArray(), dtInicioTurma);

            if (valorRetorno != null)
            {
                connection.Rollback();
                return valorRetorno;
            }

            //atualiza aula docente 
            valorRetorno = AtualizarAulaDocenteQHI(connection, dtAulaDocenteExcluido, dtInicioTurma);

            if (valorRetorno != null)
            {
                connection.Rollback();
                return valorRetorno;
            }

            return valorRetorno;
        }

        private static Ly_aula_docente ObterDadoAulaDocenteExcluido(Ly_aula_docente dtAulaDocenteAtual, Ly_aula_docente dtAulaDocenteOriginal)
        {
            Ly_aula_docente dtAulaDocente = new Ly_aula_docente();
            Ly_aula_docente dtAulaDocenteAtualAux = Clonar(dtAulaDocenteAtual);

            foreach (Ly_aula_docente.Row linhaOriginal in dtAulaDocenteOriginal.Rows)
            {
                Ly_aula_docente.Row[] dados = dtAulaDocenteAtualAux.Select(" AULA = " + linhaOriginal.Aula +
                                                                           " AND DIA_SEMANA = " + linhaOriginal.Dia_semana);
                if (dados != null)
                {
                    if (dados.Length > 0)
                    {
                        foreach (Ly_aula_docente.Row linhaConsulta in dados)
                        {
                            Ly_aula_docente.Row[] dadosExcluidos = dtAulaDocenteOriginal.Select(" AULA = " + linhaConsulta.Aula +
                                                                                                " AND DIA_SEMANA = " + linhaConsulta.Dia_semana +
                                                                                                " AND NUM_FUNC = " + linhaConsulta.Num_func +
                                                                                                " AND DISCIPLINA = '" + RN.RNBase.MudarAspas(linhaConsulta.Disciplina) + "'");
                            if (dadosExcluidos.Length == 0)
                            {
                                Ly_aula_docente.Row linhaAulaDocente = dtAulaDocente.NewRow();
                                //armazena os dados da linha original que existia no quadro de horário
                                PopularLinhaAulaDocente(linhaOriginal, linhaAulaDocente);

                                if (VerificaAulaDocente(dtAulaDocente, linhaAulaDocente) == null)
                                {
                                    dtAulaDocente.Rows.Add(linhaAulaDocente);
                                }

                                dtAulaDocenteAtualAux.Rows.Remove(linhaConsulta);
                            }
                        }
                    }
                    else
                    {
                        Ly_aula_docente.Row linhaAulaDocente = dtAulaDocente.NewRow();
                        //armazena os dados da linha original que existia no quadro de horário
                        PopularLinhaAulaDocente(linhaOriginal, linhaAulaDocente);
                        dtAulaDocente.Rows.Add(linhaAulaDocente);
                    }
                }
                else
                {
                    Ly_aula_docente.Row linhaAulaDocente = dtAulaDocente.NewRow();
                    //armazena os dados da linha original que existia no quadro de horário
                    PopularLinhaAulaDocente(linhaOriginal, linhaAulaDocente);
                    dtAulaDocente.Rows.Add(linhaAulaDocente);
                }
            }
            return dtAulaDocente;
        }

        /// <summary>
        /// Overload do método de mesmo nome, encapsulando a conexão.
        /// </summary>
        public static RetValue AlterarTurmaComQuadroHorario(DadosTurma turmaOld, String nomeTurma, Ly_turma dtTurmaUI, Ly_hor_aula dtHoraAulaUI, Ly_aula_docente dtAulaDocenteUI)
        {
            ErrorList errorList = new ErrorList();

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                RetValue ret = AlterarTurmaComQuadroHorario(connection, turmaOld, nomeTurma, dtTurmaUI, dtHoraAulaUI, dtAulaDocenteUI);

                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch (Exception ex)
            {
                connection.Rollback();
                if (ex.Message.ToUpper().Contains("DEADLOCK"))
                    return new RetValue(false, "Tempo limite de consulta excedido (DEADLOCK). Tente novamente.", null);
                else if (ex.Message.ToUpper().Contains("SELECT"))
                    return new RetValue(false, "Erro de execução de consulta. Tente novamente.", null);
                else
                    return new RetValue(false, ex.Message, null);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }

        /// <summary>
        /// Altera turma e seu respectivo quadro de horário
        /// </summary>
        /// <param name="connection">Conexão</param>
        /// <param name="turmaOld">Dados antigos da turma</param>
        /// <param name="nomeTurma">Novo nome da turma</param>
        /// <param name="dtTurmaUI">Datatable com os dados da turma</param>
        /// <param name="dtHoraAulaUI">Datatable com os dados de hora aula</param>
        /// <param name="dtAulaDocenteUI">Datatable com dados de aula docente</param>
        /// <returns>RetValue com mensagem de sucesso ou erro</returns>     
        public static RetValue AlterarTurmaComQuadroHorario(TConnectionWritable connection, DadosTurma turmaOld, String nomeTurma, Ly_turma dtTurmaUI, Ly_hor_aula dtHoraAulaUI, Ly_aula_docente dtAulaDocenteUI)
        {
            RN.Docentes rnDocentes = new Docentes();
            RetValue valorRetorno = null;
            ErrorList errorList = new ErrorList();
            RN.GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new GrupoHabilitacaoDoc();
            RN.DTOs.DadosTrocaMatriculaDocente dados = new Techne.Lyceum.RN.DTOs.DadosTrocaMatriculaDocente();

            #region Validações da Turma

            if (dtTurmaUI.Rows[0].Dt_fim.Value.Date < DateTime.Now.Date)
            {
                connection.Rollback();
                errorList.Add("Término das Aulas não deve ser menor que data atual.", "ERRO");
                return new RetValue(false, "", errorList);
            }

            if (dtTurmaUI.Rows[0].Dt_inicio.Value.Year != dtTurmaUI.Rows[0].Ano)
            {
                connection.Rollback();
                errorList.Add("Início das Aulas deve ser do ano " + dtTurmaUI.Rows[0].Ano + ".", "ERRO");
                return new RetValue(false, "", errorList);
            }

            if (dtTurmaUI.Rows[0].Dt_fim.Value.Year != dtTurmaUI.Rows[0].Ano)
            {
                connection.Rollback();
                errorList.Add("Término das Aulas deve ser do ano " + dtTurmaUI.Rows[0].Ano + ".", "ERRO");
                return new RetValue(false, "", errorList);
            }

            DataSet validacaoDados = ValidaInclusaoDeTurmaQuadroDeHorario(dtTurmaUI);

            if (validacaoDados.Tables[0].Rows.Count == 0)
            {
                return new RetValue(false, "Curso não autorizado para unidade de ensino.", null);
            }

            //Verifica se é optativa reforço ou eletiva
            if (dtTurmaUI.Rows[0].OptativaReforco == "N" && dtTurmaUI.Rows[0].Eletiva == "N")
            {
                if (validacaoDados.Tables[2].Rows.Count > 0 && Convert.ToInt32(validacaoDados.Tables[2].Rows[0][0]) == 1)
                {
                    return new RetValue(false, "Conflito de dependencia.", null);
                }

                if (RN.Turma.VerificarConflitoDependencia(connection, dtTurmaUI.Rows[0].Faculdade, dtTurmaUI.Rows[0].Dependencia, dtTurmaUI.Rows[0].Dt_inicio.Value, dtTurmaUI.Rows[0].Dt_fim.Value, dtTurmaUI.Rows[0].Turma, dtTurmaUI.Rows[0].Turno))
                {
                    connection.Rollback();
                    return new RetValue(false, null, new ErrorList("Conflito de dependencia."));
                }
            }

            if (validacaoDados.Tables[1].Rows.Count > 0 && Convert.ToInt32(validacaoDados.Tables[1].Rows[0][0]) == 1)
            {
                return new RetValue(false, "Série extinta.", null);
            }

            DataSet validacaoDadosAlteracao = ValidaAlteracaoTurmaComQuadroHorario(dtTurmaUI.Rows[0].OptativaReforco.ToString(), turmaOld, nomeTurma, dtTurmaUI.Rows[0].Turno);

            if (turmaOld.Turno != dtTurmaUI.Rows[0].Turno)
            {
                var listaErros = PermiteAlterarTurnoDeTurmaComDataSet(validacaoDadosAlteracao);

                if (listaErros.Count > 0)
                {
                    connection.Rollback();
                    errorList.Add(listaErros.First());
                    return new RetValue(false, "", errorList);
                }
            }

            Ly_turma.Row rowTurmaUI = dtTurmaUI.Rows[0];

            DateTime rowTurmaOldDt_Inicio = validacaoDadosAlteracao.Tables[8].Rows.Count > 0 ? Convert.ToDateTime(validacaoDadosAlteracao.Tables[8].Rows[0][0]) : DateTime.MinValue;
            DateTime rowTurmaOldDt_Fim = validacaoDadosAlteracao.Tables[8].Rows.Count > 0 ? Convert.ToDateTime(validacaoDadosAlteracao.Tables[8].Rows[0][1]) : DateTime.MinValue;

            Boolean temAulaDocenteAtiva = validacaoDadosAlteracao.Tables[9].Rows.Count > 0;

            if (temAulaDocenteAtiva && validacaoDadosAlteracao.Tables[8].Rows.Count > 0)
            {
                if (rowTurmaUI.Dt_inicio != rowTurmaOldDt_Inicio)
                {
                    connection.Rollback();
                    errorList.Add("Não é possível alterar a Data de Início das Aulas da turma. Existem aulas alocadas no Quadro de Horários.", "ERRO");
                    return new RetValue(false, "", errorList);
                }
                if (rowTurmaUI.Dt_fim != rowTurmaOldDt_Fim)
                {
                    connection.Rollback();
                    errorList.Add("Não é possível alterar a Data de Término das Aulas da turma. Existem aulas alocadas no Quadro de Horários.", "ERRO");
                    return new RetValue(false, "", errorList);
                }
            }

            decimal numAlunosMatriculados = validacaoDadosAlteracao.Tables[10].Rows.Count > 0 ?
                                        Convert.ToInt32(validacaoDadosAlteracao.Tables[10].Rows[0][0]) : 0;
            if (numAlunosMatriculados > dtTurmaUI.Rows[0].Num_alunos)
            {
                connection.Rollback();
                errorList.Add("O Número Máximo de Alunos deve ser maior ou igual ao número de Alunos Matriculados.", "ERRO");
                return new RetValue(false, "", errorList);
            }

            //Crítico//////////////////
            valorRetorno = Alterar(connection, dtTurmaUI, turmaOld.Turno);
            if (valorRetorno != null)
            {
                connection.Rollback();
                return valorRetorno;
            }
            //////////////////////////////

            #endregion

            #region Verificação disciplina múltipla (verificação redundante em caso de inconsitência proveniente dos DataTables preenchidos na tela)

            var aulasDocMultiplas = dtAulaDocenteUI.Rows.Cast<Ly_aula_docente.Row>()
                .Where(a => a.Disciplina.Contains("|"))
                .Select(h =>
                    new
                    {
                        DisciplinaPrincipal = h.Disciplina.Split('|')[0],
                        DisciplinaMultipla = h.Disciplina.Split('|')[1],
                        h.Aula,
                        h.Dia_semana,
                        h.Num_func,
                        Registro = h
                    })
                .GroupBy(h => h.DisciplinaPrincipal);

            foreach (var aulaDocMultipla in aulasDocMultiplas)
            {
                string disciplinaPrincipal = aulaDocMultipla.Key;
                string disciplinaMultipla = null;
                decimal? aulaMultipla = null, diaSemanaMultipla = null;
                Ly_aula_docente.Row aulaDocMultipla_tmp = null;
                foreach (var tmp in aulaDocMultipla)
                {
                    if (disciplinaMultipla == null)
                    {
                        disciplinaMultipla = tmp.DisciplinaMultipla;
                        aulaMultipla = tmp.Aula;
                        diaSemanaMultipla = tmp.Dia_semana;
                        aulaDocMultipla_tmp = tmp.Registro;
                    }
                    else if (disciplinaMultipla != tmp.DisciplinaMultipla)
                    {
                        //MONTAR UMA CONSULTA UNICA 
                        string nomeDiscipPrincipal = Ly_disciplina.QueryFirstRow(connection, "disciplina = ?", disciplinaPrincipal).Nome_compl;

                        Ly_hor_oper.Row rowHorOper = Ly_hor_oper.QueryFirstRow(connection, "aula = ? and dia_semana = ?", tmp.Aula, tmp.Dia_semana);
                        errorList.Add(MontarMensagem(connection, rowHorOper, "Não é possível alocar duas disciplinas múltiplas distintas para a disciplina " + nomeDiscipPrincipal + " (" + disciplinaPrincipal + ").", tmp.DisciplinaMultipla, tmp.Num_func).ToString(), "ERRO_VALIDACAO");

                        Ly_hor_oper.Row rowHorOperMultipla = Ly_hor_oper.QueryFirstRow(connection, "aula = ? and dia_semana = ?", aulaMultipla, diaSemanaMultipla);
                        errorList.Add(MontarMensagem(connection, rowHorOperMultipla, "Não é possível alocar duas disciplinas múltiplas distintas para a disciplina " + nomeDiscipPrincipal + " (" + disciplinaPrincipal + ").", disciplinaMultipla, tmp.Num_func).ToString(), "ERRO_VALIDACAO");

                        dtAulaDocenteUI.Rows.Remove(tmp.Registro);
                        dtAulaDocenteUI.Rows.Remove(aulaDocMultipla_tmp);

                        connection.Rollback();
                        return new RetValue(false, "", errorList);
                        //////////////////////////////////////////////////////
                    }
                }
            }

            foreach (var aulaDocMult in dtAulaDocenteUI.Rows.Cast<Ly_aula_docente.Row>().Where(a => a.Disciplina.Contains('|')))
            {
                string disciplinaPrincipal = aulaDocMult.Disciplina.Split('|')[0];
                string disciplinaMultipla = aulaDocMult.Disciplina.Split('|')[1];
                Ly_turma.Row rowTurma = dtTurmaUI.Rows[0];
                rowTurma = Ly_turma.Row.Query(connection, disciplinaPrincipal, rowTurma.Turma, rowTurma.Ano, rowTurma.Semestre);
                if (dtTurmaUI.Rows[0].Eletiva == "N")
                {
                    Ly_turma.Row.Update(connection, disciplinaPrincipal, rowTurma.Turma, rowTurma.Ano, rowTurma.Semestre, "disciplina_multipla", disciplinaMultipla);
                }
                RetValue retMul = VerificarErro(connection.GetErrors());
                if (retMul != null && !retMul.Ok)
                {
                    ErrorList e = new ErrorList();
                    errorList.Add(retMul.Errors.ToString(), "ERRO");

                    connection.Rollback();
                    return new RetValue(false, "", e);
                }
            }

            foreach (var tmp in dtHoraAulaUI.Rows.Cast<Ly_hor_aula.Row>().Where(h => h.Disciplina.Contains("|")))
                tmp.Disciplina = tmp.Disciplina.Split('|')[0];
            foreach (var tmp in dtAulaDocenteUI.Rows.Cast<Ly_aula_docente.Row>().Where(a => a.Disciplina.Contains("|")))
                tmp.Disciplina = tmp.Disciplina.Split('|')[0];

            #endregion

            Ly_turma.Row dadosTurma = dtTurmaUI.Rows[0];

            #region Remove, da coleção dtAulaDocenteUI, entradas manipuladas na tela porém não alteradas
            QueryTable qtRemoverUI = new QueryTable(@"
                    SELECT ad.num_func, ad.dia_semana, ad.aula, ad.disciplina from ly_aula_docente ad (NOLOCK) INNER join
                    ly_turma t (NOLOCK) on ad.disciplina = t.disciplina AND
                    ad.turma = t.turma AND ad.ano = t.ano AND ad.semestre = t.semestre AND ad.data_fim = t.dt_fim
                    WHERE ad.turma = ? AND ad.ano = ? AND ad.semestre = ? AND t.sit_turma = 'Aberta'");
            qtRemoverUI.Query(connection, dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);

            foreach (SimpleRow sr in qtRemoverUI.Rows)
            {
                var rowRemoverADUI = dtAulaDocenteUI.Rows.Cast<Ly_aula_docente.Row>().Where(adUI =>
                    adUI.Num_func == sr["num_func"] && adUI.Dia_semana == sr["dia_semana"] &&
                    adUI.Aula == sr["aula"] && adUI.Disciplina == sr["disciplina"]);
                if (rowRemoverADUI.Count() > 0)
                    dtAulaDocenteUI.Rows.Remove(rowRemoverADUI.First());
            }
            #endregion

            //obtém os registros originais antes de realizar modificação            
            QueryTable qtEstadoOriginal = ConsultarAulaDocente(connection, dtTurmaUI.Rows[0].Turno, dtTurmaUI.Rows[0].Faculdade, dtTurmaUI.Rows[0].Turma, dtTurmaUI.Rows[0].Ano.Value, dtTurmaUI.Rows[0].Semestre.Value);
            QueryTable qtEstadoOriginalAux = ConsultarAulaDocente(connection, dtTurmaUI.Rows[0].Turno, dtTurmaUI.Rows[0].Faculdade, dtTurmaUI.Rows[0].Turma, dtTurmaUI.Rows[0].Ano.Value, dtTurmaUI.Rows[0].Semestre.Value);
            Ly_aula_docente_tipo dtAulaDocenteTipo = ConsultarLyAulaDocenteTipo(connection, dtTurmaUI.Rows[0].Turno, dtTurmaUI.Rows[0].Faculdade, dtTurmaUI.Rows[0].Turma, dtTurmaUI.Rows[0].Ano.Value, dtTurmaUI.Rows[0].Semestre.Value);
            Ly_aula_docente dtAulaDocenteOriginal = PopularLyAulaDocente(qtEstadoOriginal, null);

            //copia os dados de aula docente (obtidos na IU) para o datatable auxiliar
            Ly_aula_docente dtAulaDocenteAux = Clonar(dtAulaDocenteUI);
            //obtém a nova aula de docente (somente os registros alterados)
            Ly_aula_docente dtAulaDocenteAtual = ObterAulaDocenteAtual(dtAulaDocenteOriginal, dtAulaDocenteAux);

            #region desativa os registros que foram excluidos
            valorRetorno = AtualizarAulaDocenteExcluido(connection, dtAulaDocenteAtual, dtAulaDocenteOriginal, dtAulaDocenteTipo, dtTurmaUI.Rows[0].Dt_inicio.Value);
            if (valorRetorno != null)
            {
                connection.Rollback();
                return valorRetorno;
            }
            #endregion

            #region inclusão/atualização dos registros atuais (hora aula  e aula docente)
            //atualiza horario da aula
            valorRetorno = AtualizarHoraAulaAtual(connection, dtHoraAulaUI);
            if (valorRetorno != null)
            {
                connection.Rollback();
                return valorRetorno;
            }

            //atualiza aula docente
            valorRetorno = AtualizarAulaDocenteAtual(connection, dtAulaDocenteUI, dtTurmaUI.Rows[0].Dt_fim.Value, dtTurmaUI.Rows[0].Dt_inicio.Value);
            if (valorRetorno != null)
            {
                connection.Rollback();
                return valorRetorno;
            }
            #endregion

            Ly_hor_aula dtHoraAulaAtual = Ly_hor_aula.Query(connection, "turno = ? and faculdade = ? and turma = ? and ano = ? and semestre = ? ", dtTurmaUI.Rows[0].Turno, dtTurmaUI.Rows[0].Faculdade, dtTurmaUI.Rows[0].Turma, dtTurmaUI.Rows[0].Ano.Value, dtTurmaUI.Rows[0].Semestre.Value);

            //obtém aula docente atualizado para turma atual                                
            QueryTable qtAulaDocenteAtual = ConsultarAulaDocente(connection, dtTurmaUI.Rows[0].Turno, dtTurmaUI.Rows[0].Faculdade, dtTurmaUI.Rows[0].Turma, dtTurmaUI.Rows[0].Ano.Value, dtTurmaUI.Rows[0].Semestre.Value);
            Ly_aula_docente dtAulaDocenteAtualAux = PopularLyAulaDocente(qtAulaDocenteAtual, null);

            AtualizaTipoAulaDocente(dtAulaDocenteAtualAux, dtAulaDocenteUI);
            AtualizaTipoHoraAula(dtHoraAulaAtual, dtHoraAulaUI);

            #region Atualiza a flag TIPO para "0.5" caso seja GLP

            foreach (Ly_aula_docente.Row adTemp in dtAulaDocenteAtualAux.Rows)
            {
                String adSelect = String.Format(
                    "num_func = '{0}' AND turno = '{1}' AND faculdade = '{2}' AND dia_semana = '{3}' AND aula = '{4}' AND disciplina = '{5}' AND turma = '{6}' AND ano = '{7}' AND semestre = '{8}' AND data_inicio = '{9}' AND tipo_aula = 'GLP'",
                    adTemp.Num_func, adTemp.Turno, adTemp.Faculdade, adTemp.Dia_semana, adTemp.Aula, adTemp.Disciplina, adTemp.Turma.Replace("'", "''"), adTemp.Ano, adTemp.Semestre, adTemp.Data_inicio.Value.ToString("yyyy-MM-dd"));
                if (qtEstadoOriginalAux.Select(adSelect).Count() > 0) //é GLP                    
                    adTemp.Tipo = "0.5";
            }

            foreach (Ly_hor_aula.Row haTemp in dtHoraAulaAtual.Rows)
            {
                String adSelect = String.Format(
                    "turno = '{0}' AND faculdade = '{1}' AND dia_semana = '{2}' AND aula = '{3}' AND disciplina = '{4}' AND turma = '{5}' AND ano = '{6}' AND semestre = '{7}' AND tipo_aula = 'GLP'",
                    haTemp.Turno, haTemp.Faculdade, haTemp.Dia_semana, haTemp.Aula, haTemp.Disciplina, haTemp.Turma.Replace("'", "''"), haTemp.Ano, haTemp.Semestre);
                if (qtEstadoOriginalAux.Select(adSelect).Count() > 0) //é GLP                    
                    haTemp.Tipo = "0.5";
            }

            #endregion

            #region exclui os registros logicamente da aula atual e original, deixando a turma sem os registros de aula docente que foram alterados
            valorRetorno = AtualizarAulaDocente(connection, dtAulaDocenteUI, dtTurmaUI.Rows[0].Dt_inicio.Value);
            if (valorRetorno != null)
            {
                connection.Rollback();
                return valorRetorno;
            }
            #endregion

            bool validaConsecutividade = true;
            bool validaQHItotal = true;

            //CHAMADO 39579
            if (dtTurmaUI.Rows[0].Curso == "9999.04")
            {
                if (dtAulaDocenteAtual.Rows.Count > 0)
                {
                    //CHAMADO 74895 - RETIRADA DA REGRA
                    ////obtém os dados da grade
                    //QueryTable qtGradeEdEspecial = ObterTotalDisciplinaGrade(connection, dtTurmaUI.Rows[0].Curso,
                    //                                                    dtTurmaUI.Rows[0].Turno, dtTurmaUI.Rows[0].Curriculo,
                    //                                                    dtTurmaUI.Rows[0].Serie.Value);

                    //if (qtGradeEdEspecial != null && dtAulaDocenteAtual != null)
                    //{
                    //    if (Convert.ToInt32(qtGradeEdEspecial.Rows[0]["total_disc_grade"]) != dtAulaDocenteAtual.Rows.Count)
                    //    {
                    //        errorList.Add("O total de alocação deve ser igual ao total da matriz.", "ERRO");
                    //        validaQHItotal = false;
                    //    }
                    //}


                    //CHAMADO 74895 - RETIRADA DA REGRA
                    //VERIFICA A QNT DE PROFESSORES ALOCADOS NAS ATIVIDADES PARA O CURSO DE ED. ESPECIAL
                    //var totalMatEdEspec = dtAulaDocenteAtual.Rows.Cast<DataRow>()
                    //    .Select(r => (decimal)r["Num_func"])
                    //    .Distinct()
                    //    .Where(r => r.ToString() != "115451" && r.ToString() != "115460")
                    //    .Count();

                    //var MatEdEspec = dtAulaDocenteAtual.Rows.Cast<DataRow>()
                    //   .Select(r => (decimal)r["Num_func"])
                    //   .Distinct()
                    //   .Where(r => r.ToString() != "115451" && r.ToString() != "115460");


                    //if (totalMatEdEspec > 1)
                    //{
                    //    List<decimal> pessoas = new List<decimal>();

                    //    foreach (var item in MatEdEspec)
                    //    {

                    //        Ly_docente.Row rowDocente = Ly_docente.Row.Query(connection, item);
                    //        pessoas.Add(rowDocente.Pessoa.Value);
                    //    }

                    //    if (pessoas.Distinct().Count() > 1)
                    //    {
                    //        errorList.Add(
                    //            "Para o curso de Educação Especial somente 1 professor pode ser alocado nas Atividades.", "ERRO");

                    //        validaQHItotal = false;
                    //    }
                    //}


                    //VERIFICA CONSECUTIVIDADE

                    //Ly_aula_docente consecutivosAux = new Ly_aula_docente();

                    //foreach (Ly_aula_docente.Row atual in dtAulaDocenteAtual.Rows)
                    //{
                    //    String adConsecutivo = String.Format("Aula = '{0}' AND Dia_semana = '{1}' AND Disciplina = '{2}'", atual.Aula, atual.Dia_semana.ToString(), atual.Disciplina);

                    //    if (consecutivosAux.Rows.Count > 0)
                    //    {
                    //        if (consecutivosAux.Select(adConsecutivo).Count() > 0)
                    //        {
                    //            continue;
                    //        }
                    //    }

                    //    QueryTable qtProxOrdemAula = ConsultaOrdemAula(atual.Turma, atual.Ano.ToString(), atual.Semestre.ToString(), atual.Dia_semana.ToString(),
                    //                                           atual.Aula.ToString());
                    //    String adSelect = string.Empty;
                    //    if (qtProxOrdemAula.Rows.Count > 0)
                    //    {
                    //        adSelect = String.Format("Aula = '{0}' AND Dia_semana = '{1}' AND Disciplina = '{2}'", qtProxOrdemAula.Rows[0]["aula"].ToString(), atual.Dia_semana.ToString(), atual.Disciplina);
                    //    }

                    //    if (dtAulaDocenteAtual.Select(adSelect).Count() == 0)
                    //    {
                    //        validaConsecutividade = true;
                    //    }
                    //    else
                    //    {
                    //        Ly_aula_docente.Row linhaAulaDocente = consecutivosAux.NewRow();

                    //        Ly_aula_docente.Row[] linha = dtAulaDocenteAtual.Select(adSelect);

                    //        PopularLinhaAulaDocenteAux(linha[0], linhaAulaDocente);

                    //        consecutivosAux.Rows.Add(linhaAulaDocente);
                    //    }
                    //}

                }
            }

            //VERIFICA SE O QUADRO ESTA SENDO SALVO PARCIALMENTE PARA O CURSO ED. ESPECIAL

            //RETIRADA A REGRA CONFORME CHAMADO 27922 SOLICITADO PELO QHI EM 08/02/2023
            /*
            if (dtTurmaUI.Rows[0].Curso == "9999.91")
            {
                if (dtAulaDocenteAtual.Rows.Count > 0)
                {
                    //RETIRADA A REGRA CONFORME CHAMADO 27922 SOLICITADO PELO QHI EM 08/02/2023
                    ////obtém os dados da grade
                    //QueryTable qtGradeEdEspecial = ObterTotalDisciplinaGrade(connection, dtTurmaUI.Rows[0].Curso,
                    //                                                    dtTurmaUI.Rows[0].Turno, dtTurmaUI.Rows[0].Curriculo,
                    //                                                    dtTurmaUI.Rows[0].Serie.Value);

                    //if (qtGradeEdEspecial != null && dtAulaDocenteAtual != null)
                    //{
                    //    if (Convert.ToInt32(qtGradeEdEspecial.Rows[0]["total_disc_grade"]) != dtAulaDocenteAtual.Rows.Count)
                    //    {
                    //        errorList.Add("O total de alocação deve ser igual ao total da matriz.", "ERRO");
                    //        validaQHItotal = false;
                    //    }
                    //}


                    //RETIRADA A REGRA CONFORME CHAMADO 22039 SOLICITADO PELO QHI EM 10/08/2022
                    //VERIFICA A QNT DE PROFESSORES ALOCADOS NAS ATIVIDADES PARA O CURSO DE ED. ESPECIAL
                    //var totalMatEdEspec = dtAulaDocenteAtual.Rows.Cast<DataRow>()
                    //    .Select(r => (decimal)r["Num_func"])
                    //    .Distinct()
                    //    .Where(r => r.ToString() != "115451" && r.ToString() != "115460")
                    //    .Count();

                    //if (totalMatEdEspec > 1)
                    //{
                    //    errorList.Add(
                    //        "Para o curso de Educação Especial somente 1 professor pode ser alocado nas Atividades.", "ERRO");

                    //    validaQHItotal = false;
                    //}

                    //VERIFICA CONSECUTIVIDADE
                  
                    Ly_aula_docente consecutivosAux = new Ly_aula_docente();

                    foreach (Ly_aula_docente.Row atual in dtAulaDocenteAtual.Rows)
                    {
                        String adConsecutivo = String.Format("Aula = '{0}' AND Dia_semana = '{1}' AND Disciplina = '{2}'", atual.Aula, atual.Dia_semana.ToString(), atual.Disciplina);

                        if (consecutivosAux.Rows.Count > 0)
                        {
                            if (consecutivosAux.Select(adConsecutivo).Count() > 0)
                            {
                                continue;
                            }
                        }

                        QueryTable qtProxOrdemAula = ConsultaOrdemAula(atual.Turma, atual.Ano.ToString(), atual.Semestre.ToString(), atual.Dia_semana.ToString(),
                                                               atual.Aula.ToString());
                        String adSelect = string.Empty;
                        if (qtProxOrdemAula.Rows.Count > 0)
                        {
                            adSelect = String.Format("Aula = '{0}' AND Dia_semana = '{1}' AND Disciplina = '{2}'", qtProxOrdemAula.Rows[0]["aula"].ToString(), atual.Dia_semana.ToString(), atual.Disciplina);
                        }

                        if (dtAulaDocenteAtual.Select(adSelect).Count() == 0)
                        {
                            validaConsecutividade = true;
                        }
                        else
                        {
                            Ly_aula_docente.Row linhaAulaDocente = consecutivosAux.NewRow();

                            Ly_aula_docente.Row[] linha = dtAulaDocenteAtual.Select(adSelect);

                            PopularLinhaAulaDocenteAux(linha[0], linhaAulaDocente);

                            consecutivosAux.Rows.Add(linhaAulaDocente);
                        }
                    }
                    
                }
            }
 */
            //neste momento a turma nao tem nenhum registro de aula docente e nem de aula docente tipo, sera varrido todos os dados 
            //da turma atual para ser atualizado celula por celula

            //obtém os dados da grade
            QueryTable qtGrade = ObterDisciplinaGrade(connection, dtTurmaUI.Rows[0].Curso, dtTurmaUI.Rows[0].Turno, dtTurmaUI.Rows[0].Curriculo, dtTurmaUI.Rows[0].Serie.Value);

            Ly_aula_docente consecutivos = new Ly_aula_docente();
            //filtro para verificar se as linhas alteradas estão dentro das atualizadas
            foreach (Ly_aula_docente.Row linhaAulaDocente in dtAulaDocenteAtualAux.Select("tipo = '1'", "tipo desc"))
            {
                Ly_hor_aula.Row linhaHoraAula = dtHoraAulaAtual.Rows.Cast<Ly_hor_aula.Row>()
                   .Where(ha => ha.Turno == linhaAulaDocente.Turno &&
                    ha.Faculdade == linhaAulaDocente.Faculdade &&
                    ha.Dia_semana == linhaAulaDocente.Dia_semana &&
                    ha.Aula == linhaAulaDocente.Aula &&
                    ha.Turma == linhaAulaDocente.Turma &&
                    ha.Ano == linhaAulaDocente.Ano &&
                    ha.Semestre == linhaAulaDocente.Semestre &&
                    ha.Disciplina == linhaAulaDocente.Disciplina
                  ).First();

                try
                {
                    //instancia o aula docente tipo para verificação dos registros em GLP
                    Ly_aula_docente_tipo dtAulaDocenteTipoNovo = new Ly_aula_docente_tipo();

                    if (linhaAulaDocente.Num_func.Value > 0)
                    {
                        bool atualizar = true;
                        //string matricula = RN.Docentes.ObterMatricula(connection, linhaAulaDocente.Num_func.Value);
                        LyDocente docente = RN.Docentes.Carregar(Convert.ToInt32(linhaAulaDocente.Num_func));
                        string matricula = docente.Matricula;

                        if (!string.IsNullOrEmpty(matricula))
                        {
                            if (matricula != "00000000" && matricula != "99999999")
                            {
                                //Verifica se a matricula atual é 66666666, e se antes de alocar este valor foi gravado o valor 99999999
                                if (VerificarMatriculaNecessidadeContratoTemporario(qtEstadoOriginal, linhaAulaDocente, matricula))
                                {
                                    if (matricula != "66666666")
                                    {
                                        //Verifica se a matricula atual é de contrato, e se antes de alocar este valor foi gravado o valor 99999999 ou 00000000
                                        if (docente.RegimeContratacaoId == (int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario)
                                        {
                                            if (!PossuiMatriculaParaNecessidadeContratoTemporario(qtEstadoOriginal, linhaAulaDocente, matricula))
                                            {
                                                StringBuilder mensagem = MontarMensagem(connection, linhaHoraAula, "Não é permitido alocar professor de 'Contrato Temporario' sem antes alocar horário para a disciplina como 99999999 ou 00000000.", matricula, linhaAulaDocente.Num_func.Value);
                                                errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
                                                atualizar = false;
                                            }
                                        }

                                        QueryTable qtDocente = RN.Docentes.ConsultarMatriculaDocente(connection, linhaAulaDocente.Num_func.Value);
                                        if (qtDocente != null && qtDocente.Rows.Count > 0)
                                        {
                                            foreach (SimpleRow linhaDocente in qtDocente.Rows)
                                            {
                                                decimal num_func_2_matricula = Convert.ToDecimal(linhaDocente["num_func"]);

                                                //realiza a validação do horário
                                                ErrorList erroListValidacao = ValidarHorario(connection, linhaHoraAula, dtTurmaUI.Rows[0], num_func_2_matricula, linhaAulaDocente.Num_func.Value, matricula, linhaAulaDocente.Disciplina);
                                                if (erroListValidacao != null && erroListValidacao["ERRO_VALIDACAO"].Length > 0)
                                                {
                                                    errorList.Add(erroListValidacao);
                                                    atualizar = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    StringBuilder mensagem = MontarMensagem(connection, linhaHoraAula, "Não é permitido alocar 66666666 sem antes alocar horário como 99999999.", matricula, linhaAulaDocente.Num_func.Value);
                                    errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
                                    atualizar = false;
                                }

                            }

                            if (atualizar)
                            {
                                ErrorList erroListGLP = new ErrorList();
                                //Aguardando registros/definição da tabela para realizar verificação da GLP
                                erroListGLP = ValidarHoraGLP(connection, linhaHoraAula, dtTurmaUI.Rows[0], linhaAulaDocente.Num_func.Value, qtEstadoOriginal, dtAulaDocenteTipoNovo, linhaAulaDocente.Data_inicio.Value);

                                //validação de GLP para docente
                                if (erroListGLP != null)
                                {
                                    if (erroListGLP["ERRO_GLP"].Length > 0)
                                    {
                                        atualizar = false;
                                    }

                                    //erros que podem ser inseridos "INFORMA_GLP" ou "ERRO_GLP"
                                    errorList.Add(erroListGLP);
                                }
                            }

                            if (qtGrade != null && qtGrade.Rows.Count > 0)
                            {
                                SimpleRow[] sr = qtGrade.Select("DISCIPLINA = '" + linhaAulaDocente.Disciplina + "' ");
                                int numeroAulaSemanalDisciplina = 0;

                                if (sr != null && sr.Length > 0 && !sr[0]["aulas_semanais"].IsNull)
                                    numeroAulaSemanalDisciplina = Convert.ToInt32(sr[0]["aulas_semanais"]);

                                int? contagemAlocacoesDisciplina = ContagemAlocacoesAtivasPorDisciplina(connection, linhaAulaDocente.Turno, linhaAulaDocente.Faculdade, linhaAulaDocente.Turma, linhaAulaDocente.Ano.Value, linhaAulaDocente.Semestre.Value, linhaAulaDocente.Disciplina);
                                if (contagemAlocacoesDisciplina != null && contagemAlocacoesDisciplina.HasValue)
                                {
                                    int numeroAulaQuadroHorario = contagemAlocacoesDisciplina.Value + 1; //incrementa um considerando a aula docente atual

                                    if (numeroAulaQuadroHorario > numeroAulaSemanalDisciplina)
                                    {
                                        StringBuilder mensagem = new StringBuilder();
                                        if (sr != null && sr.Length > 0 && !sr[0]["nome"].IsNull)
                                        {
                                            mensagem = MontarMensagem(connection, linhaHoraAula, "Disciplina " + Convert.ToString(sr[0]["nome"]) + " já extrapolou seu limite de carga horária para esta turma.", matricula, linhaAulaDocente.Num_func.Value);
                                        }
                                        else
                                        {
                                            string nomedisciplina = RN.Disciplina.ObterNomeDisciplina(connection, linhaAulaDocente.Disciplina);
                                            mensagem = MontarMensagem(connection, linhaHoraAula, "Disciplina " + nomedisciplina + " não está presente na Matriz Curricular desta turma.", matricula, linhaAulaDocente.Num_func.Value);
                                        }
                                        errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
                                        atualizar = false;
                                    }
                                }
                            }

                            //26/08 - CHAMADO 23361 - SOLICITO EXCLUIR A REGRA DE DURAÇÃO DOS TEMPOS PARA ALOCAÇÃO NA SALA DE RECURSOS. 
                            /**/
                            if (dtTurmaUI.Rows[0].Curso == "9999.91")
                            {
                                bool validadoEP = true;
                                String adConsecutivo = String.Format("Aula = '{0}' AND Dia_semana = '{1}' AND Disciplina = '{2}'", linhaAulaDocente.Aula, linhaAulaDocente.Dia_semana.ToString(), linhaAulaDocente.Disciplina);

                                if (consecutivos.Rows.Count > 0)
                                {
                                    if (consecutivos.Select(adConsecutivo).Count() > 0)
                                        validadoEP = false;
                                }
                                if (validadoEP)
                                {
                                    QueryTable qtProxOrdemAula = ConsultaOrdemAula(linhaAulaDocente.Turma, linhaAulaDocente.Ano.ToString(), linhaAulaDocente.Semestre.ToString(), linhaAulaDocente.Dia_semana.ToString(),
                                                                           linhaAulaDocente.Aula.ToString());

                                    String adSelect = String.Format("Aula = '{0}' AND Dia_semana = '{1}' AND Disciplina = '{2}'", qtProxOrdemAula.Rows.Count > 0 ? qtProxOrdemAula.Rows[0]["aula"].ToString() : "0", linhaAulaDocente.Dia_semana.ToString(), linhaAulaDocente.Disciplina);

                                    if (dtAulaDocenteAtualAux.Select(adSelect).Count() == 0)
                                    {
                                        string nomedisciplina = RN.Disciplina.ObterNomeDisciplina(connection, linhaAulaDocente.Disciplina);
                                        // errorList.Add(
                                        //"Atividade " + nomedisciplina + " deve ter duração de 2 horas(tempos).", "ERRO");

                                        //StringBuilder mensagem = MontarMensagem(connection, linhaHoraAula, "Atividade " + nomedisciplina + " deve ter duração de 2 horas(tempos) consecutivas.", matricula, linhaAulaDocente.Num_func.Value);
                                        //errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
                                    }
                                    else
                                    {
                                        Ly_aula_docente.Row linhaDocente = consecutivos.NewRow();

                                        Ly_aula_docente.Row[] linha = dtAulaDocenteAtualAux.Select(adSelect);

                                        PopularLinhaAulaDocenteAux(linha[0], linhaDocente);

                                        consecutivos.Rows.Add(linhaDocente);
                                    }
                                }

                                if (!validaQHItotal || !validaConsecutividade)
                                {
                                    StringBuilder mensagem = MontarMensagem(connection, linhaHoraAula, "Problemas ao salvar a turma.", matricula, linhaAulaDocente.Num_func.Value);
                                    errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
                                }
                            }

                            //se nao ocorreu nenhum problema sera atualizado o registro de aula docente
                            if (atualizar && validaQHItotal && validaConsecutividade)
                            {
                                valorRetorno = AtualizarAulaDocente(connection, linhaAulaDocente, dtTurmaUI.Rows[0].Dt_fim.Value, Convert.ToInt32(docente.RegimeContratacaoId));
                                if (valorRetorno != null)
                                {
                                    connection.Rollback();
                                    return valorRetorno;
                                }

                                //atualiza aula docente tipo
                                if (dtAulaDocenteTipoNovo.Rows.Count > 0)
                                {
                                    valorRetorno = AtualizarAulaDocenteTipoAtual(connection, dtAulaDocenteTipoNovo, dtAulaDocenteAtualAux, dtTurmaUI.Rows[0].Dt_fim.Value, dtTurmaUI.Rows[0].Dt_inicio.Value);
                                    if (valorRetorno != null)
                                    {
                                        connection.Rollback();
                                        return valorRetorno;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw exc;
                }
            }
            if (dtTurmaUI.Rows[0].Curso != "9999.91")
            {
                if (errorList != null && TransformarErrorList(new RetValue(false, "", errorList)).Count > 0)
                {
                    connection.Rollback();
                    return new RetValue(false, "erro", errorList);
                }
            }

            //instancia um novo datatable pois o método para obter o quadro de horario alterado remove o registro
            Ly_hor_aula dtHorAulaAux = Clonar(dtHoraAulaUI);

            //obtém a horaaula atual
            QueryTable qtEstadoAtual = ConsultarAulaDocente(connection, dtTurmaUI.Rows[0].Turno, dtTurmaUI.Rows[0].Faculdade, dtTurmaUI.Rows[0].Turma, dtTurmaUI.Rows[0].Ano.Value, dtTurmaUI.Rows[0].Semestre.Value);

            //obtém os dados da hora aula original e instancia um novo hor_aula para fazer a comparacao de todo quadro de horário
            Ly_hor_aula dtHorAulaCompleto = ObterQuadroHorarioAlterado(qtEstadoAtual, dtHorAulaAux);

            #region Remove os resíduos gerados e reativa registros anteriores em células criticadas

            DateTime dtInicio = dadosTurma.Dt_inicio.Value.Date > DateTime.Today.Date ?
                dadosTurma.Dt_inicio.Value : DateTime.Today.Date;
            dtInicio = new DateTime(dtInicio.Year, dtInicio.Month, dtInicio.Day, 0, 0, 0);

            QueryTable qtDisciplinasTurma = new QueryTable(@"
                    SELECT d.disciplina, d.nome_compl FROM ly_turma t (NOLOCK)
                    INNER JOIN ly_disciplina d (NOLOCK) ON t.disciplina = d.disciplina
                    WHERE t.turma = ? AND t.ano = ? AND t.semestre = ?");
            qtDisciplinasTurma.Query(connection, dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
            var disciplinas = qtDisciplinasTurma.Rows.Cast<SimpleRow>()
                .Select(d => new { Disciplina = Convert.ToString(d["disciplina"]), NomeCompleto = Convert.ToString(d["nome_compl"]) });
            //.ToDictionary(d => d.NomeCompleto, d => d.Disciplina);

            var errosResiduos = TransformarErrorList(new RetValue(false, "", errorList))
                .Where(e => e.TipoErro == "ERRO_VALIDACAO" || e.TipoErro == "ERRO_GLP");

            foreach (TurmaError erroResiduo in errosResiduos)
            {
                if (disciplinas.Where(d => d.NomeCompleto == erroResiduo.Disciplina).Count() > 1)
                    continue;
                if (disciplinas.Where(d => d.NomeCompleto == erroResiduo.Disciplina).Count() == 0)
                    continue;

                string disciplina = disciplinas.Where(d => d.NomeCompleto == erroResiduo.Disciplina).Select(d => d.Disciplina).First();
                string matricula = erroResiduo.Matricula.Split('-')[0].Trim();
                decimal num_func = rnDocentes.ObtemNumFuncPor(matricula);

                //Reativa os aula_docentes originais que foram desativados, nas células que foram criticadas
                var rowsReativar = qtEstadoOriginalAux.Rows.Cast<SimpleRow>()
                    .Where(original =>
                    original["turno"] == dadosTurma.Turno &&
                    original["faculdade"] == dadosTurma.Faculdade &&
                    original["dia_semana"] == erroResiduo.DiaDaSemana &&
                    original["aula"] == erroResiduo.Aula &&
                        //original["disciplina"] == disciplina &&
                    original["ano"] == dadosTurma.Ano &&
                    original["semestre"] == dadosTurma.Semestre);
                if (rowsReativar != null && rowsReativar.Count() > 0)
                {
                    var original = rowsReativar.First();

                    DateTime hoje = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);

                    if (original["tipo_aula"] == "GLP")
                    {
                        SimpleRow aulaAnterior = ObterAulaDocenteAnterior(connection,
                                Convert.ToString(original["num_func"]), Convert.ToString(original["turno"]), Convert.ToString(original["faculdade"]),
                                Convert.ToInt32(original["dia_semana"]), Convert.ToString(original["aula"]), Convert.ToString(original["disciplina"]),
                                Convert.ToString(original["turma"]), Convert.ToString(original["ano"]), Convert.ToString(original["semestre"]));

                        if (aulaAnterior != null)
                        {
                            string matriculaAnterior = Convert.ToString(aulaAnterior["matricula"]);
                            decimal numfuncAnterior = Convert.ToDecimal(aulaAnterior["num_func"]);
                            DateTime dataInicioAnterior = Convert.ToDateTime(aulaAnterior["data_inicio"]);

                            if (matriculaAnterior == "00000000" || matriculaAnterior == "99999999")
                            {
                                if (dataInicioAnterior != hoje)
                                {
                                    Ly_aula_docente.Row.Insert(connection, numfuncAnterior,
                                        Convert.ToString(original["turno"]), Convert.ToString(original["faculdade"]),
                                        Convert.ToDecimal(original["dia_semana"]), Convert.ToDecimal(original["aula"]),
                                        Convert.ToString(original["disciplina"]), Convert.ToString(original["turma"]),
                                        Convert.ToDecimal(original["ano"]), Convert.ToDecimal(original["semestre"]), hoje,
                                        "data_fim, stamp_atualizacao", dadosTurma.Dt_fim, DateTime.Now);
                                }
                                else
                                {
                                    Ly_aula_docente.Row.Update(connection,
                                        numfuncAnterior, Convert.ToString(aulaAnterior["Turno"]), Convert.ToString(aulaAnterior["Faculdade"]),
                                        Convert.ToDecimal(aulaAnterior["Dia_semana"]), Convert.ToDecimal(aulaAnterior["Aula"]), Convert.ToString(aulaAnterior["Disciplina"]),
                                        Convert.ToString(aulaAnterior["Turma"]), Convert.ToDecimal(aulaAnterior["Ano"]), Convert.ToDecimal(aulaAnterior["Semestre"]),
                                        dataInicioAnterior, "data_fim, stamp_atualizacao", dadosTurma.Dt_fim, DateTime.Now);
                                }
                            }
                        }
                    }
                    else
                    {
                        Ly_aula_docente.Row rowReativar = Ly_aula_docente.QueryFirstRow(connection,
                            "num_func = ? AND turma = ? AND turno = ? AND faculdade = ? AND dia_semana = ? AND aula = ? AND disciplina = ? AND ano = ? AND semestre = ? AND data_inicio = ?",
                            original["num_func"], dadosTurma.Turma, original["turno"], original["faculdade"], original["dia_semana"], original["aula"], original["disciplina"], original["ano"], original["semestre"], original["data_inicio"]);

                        if (rowReativar != null)
                        {
                            Ly_aula_docente.Row.Update(connection,
                                rowReativar.Num_func, rowReativar.Turno, rowReativar.Faculdade, rowReativar.Dia_semana, rowReativar.Aula,
                                rowReativar.Disciplina, rowReativar.Turma, rowReativar.Ano, rowReativar.Semestre, rowReativar.Data_inicio,
                                "data_fim, stamp_atualizacao", dadosTurma.Dt_fim, DateTime.Now);
                            RetValue retReativacao = VerificarErro(connection.GetErrors());
                            rowReativar.Data_fim = dadosTurma.Dt_fim;
                        }
                    }
                }

                //Remove resíduo da aula docente 
                Ly_aula_docente.Row.Delete(connection, num_func, dadosTurma.Turno, dadosTurma.Faculdade, erroResiduo.DiaDaSemana, erroResiduo.Aula, disciplina,
                    dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre, dtInicio);
                RetValue erroRemocaoADResiduo = VerificarErro(connection.GetErrors());


                //Verifica se pode remover hora aula
                Ly_aula_docente.Row rowADResiduo = Ly_aula_docente.QueryFirstRow(connection, "turno = ? AND faculdade = ? AND dia_semana = ? AND aula = ? AND disciplina = ? AND turma = ? AND ano = ? AND semestre = ?",
                    dadosTurma.Turno, dadosTurma.Faculdade, erroResiduo.DiaDaSemana, erroResiduo.Aula, disciplina, dadosTurma.Turma,
                    dadosTurma.Ano, dadosTurma.Semestre);
                if (rowADResiduo == null)
                {
                    Ly_hor_aula.Row rowHAResiduo = Ly_hor_aula.Row.Query(connection, dadosTurma.Turno, dadosTurma.Faculdade, erroResiduo.DiaDaSemana, erroResiduo.Aula, disciplina, dadosTurma.Turma,
                    dadosTurma.Ano, dadosTurma.Semestre);
                    if (rowHAResiduo != null)
                    {
                        TCommand.ExecuteNonQuery(connection,
                          "DELETE FROM ly_hor_aula WHERE turno = ? AND faculdade = ? AND dia_semana = ? AND aula = ? AND disciplina = ? AND turma = ? AND ano = ? AND semestre = ?",
                        dadosTurma.Turno, dadosTurma.Faculdade, erroResiduo.DiaDaSemana, erroResiduo.Aula, disciplina, dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
                    }
                }
                RetValue erroRemovaoHAResiduo = VerificarErro(connection.GetErrors());
            }

            #endregion

            #region Limpa DISCIPLINA_MULTIPLA dos registros LY_TURMA da turma

            var turmaDiscips = Ly_turma.Query(connection, "turma = ? and ano = ? and semestre = ? and disciplina_multipla is not null",
                dtTurmaUI.Rows[0].Turma, dtTurmaUI.Rows[0].Ano, dtTurmaUI.Rows[0].Semestre).Rows.Cast<Ly_turma.Row>();
            foreach (var turmaDiscip in turmaDiscips)
            {
                String discipMultipla = null;

                SimpleRow aulaDoc = SimpleRow.QueryFirstRow(connection,
                    @"  SELECT TOP 1 1 
                        FROM  ly_aula_docente ad INNER JOIN
                              ly_turma tu (NOLOCK) ON    tu.disciplina = ad.disciplina AND
                                                tu.turma = ad.turma AND
                                                tu.ano = ad.ano AND
                                                tu.semestre = ad.semestre AND 
                                                tu.dt_fim = ad.data_fim
                        WHERE   tu.sit_turma = 'Aberta' AND
                                ad.disciplina = ? AND
                                ad.turno = ? AND
                                ad.turma = ? AND
                                ad.ano = ? AND
                                ad.semestre = ?",
                    turmaDiscip.Disciplina, turmaDiscip.Turno, turmaDiscip.Turma, turmaDiscip.Ano, turmaDiscip.Semestre);
                if (aulaDoc == null)
                {
                    if (dtTurmaUI.Rows[0].Eletiva == "N")
                    {
                        if (!RN.Disciplina.EhDisciplinaEletiva(connection, turmaDiscip.Disciplina))
                        {
                            Ly_turma.Row.Update(connection, turmaDiscip.Disciplina, turmaDiscip.Turma, turmaDiscip.Ano, turmaDiscip.Semestre,
                                "disciplina_multipla", discipMultipla);
                        }
                    }
                }
            }

            #endregion

            #region TRATAMENTO DE GLPS DE OUTRAS TURMAS - QUEDA AUTOMÁTICA SE NECESSÁRIO

            //Obtenção dos docentes que tiveram aulas de matrícula desativadas, para a turma atual                
            List<int> numFuncs = new List<int>();

            var aulasDocenteAtuais = ConsultarAulaDocente(connection, dtTurmaUI.Rows[0].Turno, dtTurmaUI.Rows[0].Faculdade, dtTurmaUI.Rows[0].Turma, dtTurmaUI.Rows[0].Ano.Value, dtTurmaUI.Rows[0].Semestre.Value).Rows.Cast<SimpleRow>();

            foreach (SimpleRow sr in qtEstadoOriginal.Rows)
            {
                if (aulasDocenteAtuais
                    .Count(r => r["NUM_FUNC"] == sr["NUM_FUNC"] && r["TURNO"] == sr["TURNO"] &&
                    r["FACULDADE"] == sr["FACULDADE"] && r["DIA_SEMANA"] == sr["DIA_SEMANA"] &&
                    r["AULA"] == sr["AULA"] && r["DISCIPLINA"] == sr["DISCIPLINA"] &&
                    r["TURMA"] == sr["TURMA"] && r["ANO"] == sr["ANO"] &&
                    r["SEMESTRE"] == sr["SEMESTRE"] && r["DATA_INICIO"] == sr["DATA_INICIO"]) == 0 &&
                    sr["TIPO_AULA"] != "GLP")
                    numFuncs.Add(Convert.ToInt32(sr["NUM_FUNC"]));
            }

            foreach (int numfunc_analise in numFuncs.Distinct())
            {
                var glpsDocente = Ly_aula_docente_tipo.Query(connection,
                    "num_func = ? AND tipo_aula = 'GLP' AND turma <> ? AND data_inicio <= ? AND data_fim >= ? AND data_fim >= CONVERT(DATE, GETDATE())",
                    numfunc_analise, dtTurmaUI.Rows[0].Turma, dadosTurma.Dt_fim, dadosTurma.Dt_inicio).Rows.Cast<Ly_aula_docente_tipo.Row>();

                foreach (Ly_aula_docente_tipo.Row glpDocente in glpsDocente)
                {
                    #region Obtém o registro Ly_docente do docente e outras informações do docente
                    Ly_docente.Row rowDocente = Ly_docente.Row.Query(connection, numfunc_analise);
                    string matricula = rowDocente.Matricula;

                    string categoriaMatricula = rowDocente.Categoria;
                    string docenteReadaptado = ObterReadaptado(connection, matricula);
                    #endregion

                    dados = rnDocentes.ObtemDadosTrocaMatriculaDocentePor(matricula);

                    #region Obtém registro ly_lotacao referente à lotação válida do docente
                    Ly_lotacao.Row rowLotacao = ObterLotacaoAtiva(connection, rowDocente.Matricula, glpDocente.Data_inicio.Value, glpDocente.Data_fim.Value);
                    #endregion

                    #region Obtém registro ly_funcao refente à lotação
                    Ly_funcao.Row rowFuncao = null;
                    if (rowLotacao != null)
                        rowFuncao = Ly_funcao.QueryFirstRow(connection, "funcao = ?", rowLotacao.Funcao);
                    #endregion

                    #region Obtém registro ly_hora_aula
                    Ly_hor_aula.Row dadosHoraAula = Ly_hor_aula.Row.Query(connection, glpDocente.Turno, glpDocente.Faculdade, glpDocente.Dia_semana, glpDocente.Aula, glpDocente.Disciplina, glpDocente.Turma, glpDocente.Ano, glpDocente.Semestre);
                    #endregion

                    #region Obtém o registro ly_disciplina referente à disciplina que está sendo verificada para alocação
                    Ly_disciplina.Row rowDisciplina = Ly_disciplina.Row.Query(connection, dadosHoraAula.Disciplina);
                    #endregion

                    #region Obtém quantas matriculas estão relacionadas à pessoa passada como parâmetro
                    int numeroMatriculasDocente = ObterNumeroMatriculaDocente(connection, rowDocente.Pessoa.Value);
                    #endregion

                    #region Obtém a carga horária permitida para a função do docente

                    decimal? chPermitidaFuncao = null;
                    string funcaoMatricula = rowFuncao != null ? rowFuncao.Funcao : string.Empty;
                    decimal? chContrato = null;

                    if (rowDocente.RegimeContratacaoId == 3)
                    {


                        dados = rnDocentes.ObtemDadosTrocaMatriculaDocentePor(matricula);

                        chContrato = dados.RegimeTrabalho;

                        if (chContrato == null)
                        {
                            chPermitidaFuncao = ObterCargaHorariaNormalSemanal(connection, categoriaMatricula, funcaoMatricula);
                        }
                        else
                        {
                            chPermitidaFuncao = chContrato;
                        }
                    }
                    else
                    {
                        chPermitidaFuncao = ObterCargaHorariaNormalSemanal(connection, categoriaMatricula, funcaoMatricula);
                    }
                    #endregion

                    #region Obtém licenças ativas
                    IEnumerable<Ly_licencas.Row> licencasAtivas = ObterLicencasAtivas(connection, rowDocente.Num_func, dadosTurma.Dt_inicio, dadosTurma.Dt_fim);
                    #endregion

                    #region Verifica se docente possui licença de CH Reduzida. Se sim, 50% da CH é considerada
                    if (licencasAtivas.Count(lic => lic.Motivo == "43") > 0)
                        chPermitidaFuncao /= 2;
                    #endregion

                    #region Obtém a carga horária normal utilizada pelo docente
                    int chUsadaMatricula = ObterNumeroAulaDocentePeriodo(connection, dadosTurma.Ano.Value, dadosTurma.Semestre.Value, rowDocente.Num_func.Value, glpDocente.Data_inicio.Value, dadosTurma.Dt_fim.Value, false);
                    #endregion

                    #region Obtém o agrupamento da disciplina

                    Ly_turma.Row rowTurmaDisciplina = Ly_turma.Row.Query(connection, dadosHoraAula.Disciplina, dadosHoraAula.Turma, dadosHoraAula.Ano, dadosHoraAula.Semestre);
                    String disciplinaAgrupamento = String.IsNullOrEmpty(rowTurmaDisciplina.Disciplina_multipla) ? dadosHoraAula.Disciplina : rowTurmaDisciplina.Disciplina_multipla;
                    Ly_grupo_habilitacao.Row[] agrupamentos = ObterAgrupamentosDisciplina(connection, rowDocente.Num_func.Value, disciplinaAgrupamento);

                    #endregion

                    #region Verifica se o Grupo de Habilitação do docente permite alocação Normal
                    bool permiteAlocacaoNormal = false;
                    if (agrupamentos != null)
                    {
                        foreach (Ly_grupo_habilitacao.Row agrupamento in agrupamentos)
                        {

                            if (rnGrupoHabilitacaoDoc.PossuiHabilitacaoAlocacaoNormalPor(rowDocente.Num_func.Value, agrupamento.Agrupamento))
                            {
                                permiteAlocacaoNormal = true;
                            }
                        }
                    }

                    if (!permiteAlocacaoNormal)
                        chPermitidaFuncao = 0;
                    #endregion

                    if (chPermitidaFuncao > chUsadaMatricula)
                    {
                        DateTime dataInicioTurma = dtTurmaUI.Rows[0].Dt_inicio.Value;
                        valorRetorno = ExcluirAulaDocenteTipo(connection, glpDocente, dataInicioTurma);
                        if (valorRetorno != null && !valorRetorno.Ok)
                        {
                            connection.Rollback();
                            return valorRetorno;
                        }

                        Ly_hor_aula.Row rowGLPHA = Ly_hor_aula.Row.Query(connection, glpDocente.Turno, glpDocente.Faculdade, glpDocente.Dia_semana, glpDocente.Aula,
                            glpDocente.Disciplina, glpDocente.Turma, glpDocente.Ano, glpDocente.Semestre);
                        Ly_turno.Row rowGLPTurno = Ly_turno.Row.Query(connection, glpDocente.Turno);
                        Ly_disciplina.Row rowGLPDisciplina = Ly_disciplina.Row.Query(connection, glpDocente.Disciplina);

                        errorList.Add(String.Format("GLP do docente '{0}' desativada na Turma {1}, Ano/Sem. {2}/{3} - {4} - {5} - {6} {7}-{8}.",
                            dados.IdVinculoMatricula,
                            glpDocente.Turma,
                            glpDocente.Ano,
                            glpDocente.Semestre,
                            rowGLPTurno.Descricao,
                            rowGLPDisciplina.Nome,
                            ObterDiaSemana(glpDocente.Dia_semana.Value),
                            rowGLPHA.Horaini_aula.Value.ToString("HH:mm"),
                            rowGLPHA.Horafim_aula.Value.ToString("HH:mm")), "erro");
                    }
                }
            }

            #endregion

            #region Verifica disciplinas faltantes para completar o quadro

            //verificação de disciplinas da grade com as disciplinas do quadro de horario
            ErrorList errorDisciplina = VerificarDisciplina(connection, dtHorAulaCompleto, qtGrade);

            if (errorDisciplina != null && errorDisciplina["ERRO"].Length > 0)
                errorList.Add(errorDisciplina);

            #endregion

            #region Atualiza o Status do Quadro de Horários (COMPLETO, INCOMPLETO, SEM ALOCAÇÃO)
            var statusQHTemp = VerificarAlocacao(connection, dtTurmaUI.Rows[0]);

            if (statusQHTemp != StatusQH.NAO_ALTERADO)
            {
                Ly_turma dtTurmaAuxiliar = Consultar(connection, dtTurmaUI.Rows[0]);
                RetValue valor = AtualizarStatusQH(connection, dtTurmaAuxiliar, statusQHTemp);
                if (valor != null && !valor.Ok)
                    return valor;
            }
            #endregion

            #region Atualiza o campo CQHI na tabela LY_CANDIDATO_DOCENTE

            //AULA ALOCADA - Procura num_funcs de contrato temporário alocados na turma atual e atualiza ly_candidato_docente caso CQHI seja nulo
            QueryTable qtCQHI = new QueryTable(@"
                    SELECT DISTINCT 
                        ad.num_func,
                        doc.concurso,
                        doc.candidato
                    FROM 
                        ly_aula_docente ad
                    INNER JOIN ly_turma t (NOLOCK) on 
                        ad.TURMA = t.TURMA and 
                        ad.ANO = t.ANO and 
                        ad.SEMESTRE = t.SEMESTRE and 
                        ad.DISCIPLINA =t.DISCIPLINA and 
                        ad.DATA_FIM = t.DT_FIM
                    INNER JOIN ly_docente doc (NOLOCK) on 
                        ad.NUM_FUNC = doc.NUM_FUNC                        
                    LEFT JOIN ly_candidato_docente cd (NOLOCK) on 
                        cd.CANDIDATO = doc.CANDIDATO and 
                        cd.CONCURSO = doc.CONCURSO                        
                    WHERE 
                        doc.CANDIDATO is not null and 
                        doc.CONCURSO is not null and
                        --cd.CQHI is null and
                        ad.TURMA = ? and 
                        ad.ANO = ? and 
                        ad.SEMESTRE = ? AND
                        t.sit_turma = 'Aberta'");
            qtCQHI.Query(connection, dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);

            foreach (var rowCQHI in qtCQHI.Rows.Cast<SimpleRow>().Select(r => new { num_func = Convert.ToDecimal(r["num_func"]), concurso = Convert.ToString(r["concurso"]), candidato = Convert.ToString(r["candidato"]) }))
            {
                Ly_candidato_docente.Row rowCandDoc = Ly_candidato_docente.Row.Query(connection, rowCQHI.concurso, rowCQHI.candidato);

                if (rowCandDoc != null)
                {
                    if (rowCandDoc.Cqhi != RN.ProcessoSeletivo.CQHI_NECESSARIA)
                    {
                        if ((rowCandDoc.Status == RN.ProcessoSeletivo.Status.ReprovadoRH.ToDecimal() &&
                                   rowCandDoc.Cdrh == RN.ProcessoSeletivo.CDRH_NAO_AUTORIZADO) ||
                              (rowCandDoc.Status == RN.ProcessoSeletivo.Status.PropostaContratoTemporario.ToDecimal() &&
                                   (rowCandDoc.Cdrh == RN.ProcessoSeletivo.CDRH_EM_EXIGENCIA || String.IsNullOrEmpty(rowCandDoc.Cdrh))))
                        {
                            rowCandDoc.Cqhi = RN.ProcessoSeletivo.CQHI_NECESSARIA;
                            rowCandDoc.Dt_cqhi = DateTime.Now;
                            rowCandDoc.Status_obs = RN.ProcessoSeletivo.CQHI_NECESSARIA_OBS;
                        }
                        if (rowCandDoc.Status == RN.ProcessoSeletivo.Status.PropostaContratoTemporario.ToDecimal() &&
                              rowCandDoc.Cdrh == RN.ProcessoSeletivo.CDRH_AUTORIZADO)
                        {
                            rowCandDoc.Status = RN.ProcessoSeletivo.Status.AguardandoRemessaAprovacao.ToDecimal();
                            rowCandDoc.Cqhi = RN.ProcessoSeletivo.CQHI_NECESSARIA;
                            rowCandDoc.Dt_cqhi = DateTime.Now;
                            rowCandDoc.Status_obs = RN.ProcessoSeletivo.APROVADA_OBS;
                        }
                    }
                    else
                        rowCandDoc.Dt_cqhi = DateTime.Now;

                    Ly_candidato_docente.Row.Update(connection, rowCandDoc.Concurso, rowCandDoc.Candidato,
                        "status, status_obs, cqhi, dt_cqhi", rowCandDoc.Status, rowCandDoc.Status_obs, rowCandDoc.Cqhi, rowCandDoc.Dt_cqhi);
                }
            }

            //AULA DESALOCADA - Procura num_funcs de contrato temporário que estavam alocados na turma atual e que não possuem mais CH alocada, atualizando CQHI para nulo                
            foreach (var cqhiNumFunc in qtEstadoOriginalAux.Rows.Cast<SimpleRow>().Select(r => r["num_func"]).Distinct())
            {
                Ly_docente.Row docQCHI = Ly_docente.QueryFirstRow(connection, "num_func = ? AND candidato is not null and concurso is not null", cqhiNumFunc);
                if (docQCHI != null)
                {
                    Ly_candidato_docente.Row rowCandDoc = Ly_candidato_docente.Row.Query(connection, docQCHI.Concurso, docQCHI.Candidato);
                    if (rowCandDoc != null)
                    {
                        DbObject aulas = TCommand.ExecuteScalar(connection, @"
                                SELECT count(1)                                                                        
                                from 
                                    LY_AULA_DOCENTE ad
                                inner join LY_TURMA t (NOLOCK) on 
                                    ad.DISCIPLINA = t.DISCIPLINA and 
                                    ad.TURMA = t.TURMA and  
                                    ad.ANO = t.ANO and 
                                    ad.SEMESTRE = t.SEMESTRE and 
                                    ad.DATA_FIM = t.DT_FIM                                
                                WHERE                                     
                                    ad.NUM_FUNC = ? AND t.sit_turma = 'Aberta'", cqhiNumFunc);

                        if (Convert.ToDecimal(aulas) == 0)
                        {
                            if (rowCandDoc.Cqhi == RN.ProcessoSeletivo.CQHI_NECESSARIA)
                            {
                                if ((rowCandDoc.Status == RN.ProcessoSeletivo.Status.ReprovadoRH.ToDecimal() &&
                                           rowCandDoc.Cdrh == RN.ProcessoSeletivo.CDRH_NAO_AUTORIZADO) ||
                                      (rowCandDoc.Status == RN.ProcessoSeletivo.Status.PropostaContratoTemporario.ToDecimal() &&
                                           (rowCandDoc.Cdrh == RN.ProcessoSeletivo.CDRH_EM_EXIGENCIA || String.IsNullOrEmpty(rowCandDoc.Cdrh))))
                                {
                                    rowCandDoc.Status_obs = RN.ProcessoSeletivo.CQHI_NAO_NECESSARIA_OBS;
                                    rowCandDoc.Cqhi = RN.ProcessoSeletivo.CQHI_NAO_NECESSARIA;
                                    rowCandDoc.Dt_cqhi = DateTime.Now;
                                }
                            }
                            else
                                rowCandDoc.Dt_cqhi = DateTime.Now;

                            Ly_candidato_docente.Row.Update(connection, rowCandDoc.Concurso, rowCandDoc.Candidato,
                                "status_obs, cqhi, dt_cqhi", rowCandDoc.Status_obs, rowCandDoc.Cqhi, rowCandDoc.Dt_cqhi);
                        }
                    }
                }
            }

            #endregion

            foreach (var error in connection.GetErrors())
            {
                errorList.Add(error.ToString());
                return new RetValue(false, "", errorList);
            }
            //            if (dtTurmaUI.Rows[0].Curso == "9999.91")
            //            {
            //                if (errorList != null && TransformarErrorList(new RetValue(false, "", errorList)).Count > 0)
            //                {
            //                    if (validaQHItotal && validaConsecutividade)
            //                    {
            //                        foreach (Ly_aula_docente.Row linhaAulaDocente in dtAulaDocenteAtualAux.Select("tipo = '1'", "tipo desc"))
            //                        {
            //                            Ly_hor_aula.Row linhaHoraAula = dtHoraAulaAtual.Rows.Cast<Ly_hor_aula.Row>()
            //                               .Where(ha => ha.Turno == linhaAulaDocente.Turno &&
            //                                ha.Faculdade == linhaAulaDocente.Faculdade &&
            //                                ha.Dia_semana == linhaAulaDocente.Dia_semana &&
            //                                ha.Aula == linhaAulaDocente.Aula &&
            //                                ha.Turma == linhaAulaDocente.Turma &&
            //                                ha.Ano == linhaAulaDocente.Ano &&
            //                                ha.Semestre == linhaAulaDocente.Semestre &&
            //                                ha.Disciplina == linhaAulaDocente.Disciplina
            //                              ).First();

            //                            string matricula = rnDocentes.ObtemMatriculaPor(linhaAulaDocente.Num_func.Value);

            //                            StringBuilder mensagem = MontarMensagem(connection, linhaHoraAula, "Problemas ao salvar a turma.", matricula, linhaAulaDocente.Num_func.Value);
            //                            errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
            //                        }

            //                        #region Remove os resíduos gerados e reativa registros anteriores em células criticadas

            //                        DateTime dtInicioED = dadosTurma.Dt_inicio.Value.Date > DateTime.Today.Date ?
            //                            dadosTurma.Dt_inicio.Value : DateTime.Today.Date;
            //                        dtInicioED = new DateTime(dtInicio.Year, dtInicio.Month, dtInicio.Day, 0, 0, 0);

            //                        QueryTable qtDisciplinasTurmaED = new QueryTable(@"
            //                    SELECT d.disciplina, d.nome_compl FROM ly_turma t (NOLOCK)
            //                    INNER JOIN ly_disciplina d (NOLOCK) ON t.disciplina = d.disciplina
            //                    WHERE t.turma = ? AND t.ano = ? AND t.semestre = ?");
            //                        qtDisciplinasTurmaED.Query(connection, dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
            //                        var disciplinasED = qtDisciplinasTurmaED.Rows.Cast<SimpleRow>()
            //                            .Select(d => new { Disciplina = Convert.ToString(d["disciplina"]), NomeCompleto = Convert.ToString(d["nome_compl"]) });
            //                        //.ToDictionary(d => d.NomeCompleto, d => d.Disciplina);

            //                        var errosResiduosED = TransformarErrorList(new RetValue(false, "", errorList))
            //                            .Where(e => e.TipoErro == "ERRO_VALIDACAO" || e.TipoErro == "ERRO_GLP");

            //                        foreach (TurmaError erroResiduo in errosResiduosED)
            //                        {
            //                            if (disciplinasED.Where(d => d.NomeCompleto == erroResiduo.Disciplina).Count() > 1)
            //                                continue;
            //                            if (disciplinasED.Where(d => d.NomeCompleto == erroResiduo.Disciplina).Count() == 0)
            //                                continue;

            //                            string disciplina = disciplinas.Where(d => d.NomeCompleto == erroResiduo.Disciplina).Select(d => d.Disciplina).First();
            //                            string matricula = erroResiduo.Matricula.Split('-')[0].Trim();
            //                            decimal num_func = rnDocentes.ObtemNumFuncPor(matricula);

            //                            //Reativa os aula_docentes originais que foram desativados, nas células que foram criticadas
            //                            var rowsReativar = qtEstadoOriginalAux.Rows.Cast<SimpleRow>()
            //                                .Where(original =>
            //                                original["turno"] == dadosTurma.Turno &&
            //                                original["faculdade"] == dadosTurma.Faculdade &&
            //                                original["dia_semana"] == erroResiduo.DiaDaSemana &&
            //                                original["aula"] == erroResiduo.Aula &&
            //                                    //original["disciplina"] == disciplina &&
            //                                original["ano"] == dadosTurma.Ano &&
            //                                original["semestre"] == dadosTurma.Semestre);
            //                            if (rowsReativar != null && rowsReativar.Count() > 0)
            //                            {
            //                                var original = rowsReativar.First();

            //                                DateTime hoje = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);

            //                                if (original["tipo_aula"] == "GLP")
            //                                {
            //                                    SimpleRow aulaAnterior = ObterAulaDocenteAnterior(connection,
            //                                            Convert.ToString(original["num_func"]), Convert.ToString(original["turno"]), Convert.ToString(original["faculdade"]),
            //                                            Convert.ToInt32(original["dia_semana"]), Convert.ToString(original["aula"]), Convert.ToString(original["disciplina"]),
            //                                            Convert.ToString(original["turma"]), Convert.ToString(original["ano"]), Convert.ToString(original["semestre"]));

            //                                    if (aulaAnterior != null)
            //                                    {
            //                                        string matriculaAnterior = Convert.ToString(aulaAnterior["matricula"]);
            //                                        decimal numfuncAnterior = Convert.ToDecimal(aulaAnterior["num_func"]);
            //                                        DateTime dataInicioAnterior = Convert.ToDateTime(aulaAnterior["data_inicio"]);

            //                                        if (matriculaAnterior == "00000000" || matriculaAnterior == "99999999")
            //                                        {
            //                                            if (dataInicioAnterior != hoje)
            //                                            {
            //                                                Ly_aula_docente.Row.Insert(connection, numfuncAnterior,
            //                                                    Convert.ToString(original["turno"]), Convert.ToString(original["faculdade"]),
            //                                                    Convert.ToDecimal(original["dia_semana"]), Convert.ToDecimal(original["aula"]),
            //                                                    Convert.ToString(original["disciplina"]), Convert.ToString(original["turma"]),
            //                                                    Convert.ToDecimal(original["ano"]), Convert.ToDecimal(original["semestre"]), hoje,
            //                                                    "data_fim, stamp_atualizacao", dadosTurma.Dt_fim, DateTime.Now);
            //                                            }
            //                                            else
            //                                            {
            //                                                Ly_aula_docente.Row.Update(connection,
            //                                                    numfuncAnterior, Convert.ToString(aulaAnterior["Turno"]), Convert.ToString(aulaAnterior["Faculdade"]),
            //                                                    Convert.ToDecimal(aulaAnterior["Dia_semana"]), Convert.ToDecimal(aulaAnterior["Aula"]), Convert.ToString(aulaAnterior["Disciplina"]),
            //                                                    Convert.ToString(aulaAnterior["Turma"]), Convert.ToDecimal(aulaAnterior["Ano"]), Convert.ToDecimal(aulaAnterior["Semestre"]),
            //                                                    dataInicioAnterior, "data_fim, stamp_atualizacao", dadosTurma.Dt_fim, DateTime.Now);
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    Ly_aula_docente.Row rowReativar = Ly_aula_docente.QueryFirstRow(connection,
            //                                        "num_func = ? AND turma = ? AND turno = ? AND faculdade = ? AND dia_semana = ? AND aula = ? AND disciplina = ? AND ano = ? AND semestre = ? AND data_inicio = ?",
            //                                        original["num_func"], dadosTurma.Turma, original["turno"], original["faculdade"], original["dia_semana"], original["aula"], original["disciplina"], original["ano"], original["semestre"], original["data_inicio"]);

            //                                    if (rowReativar != null)
            //                                    {
            //                                        Ly_aula_docente.Row.Update(connection,
            //                                            rowReativar.Num_func, rowReativar.Turno, rowReativar.Faculdade, rowReativar.Dia_semana, rowReativar.Aula,
            //                                            rowReativar.Disciplina, rowReativar.Turma, rowReativar.Ano, rowReativar.Semestre, rowReativar.Data_inicio,
            //                                            "data_fim, stamp_atualizacao", dadosTurma.Dt_fim, DateTime.Now);
            //                                        RetValue retReativacao = VerificarErro(connection.GetErrors());
            //                                        rowReativar.Data_fim = dadosTurma.Dt_fim;
            //                                    }
            //                                }
            //                            }

            //                            //Remove resíduo da aula docente 
            //                            Ly_aula_docente.Row.Delete(connection, num_func, dadosTurma.Turno, dadosTurma.Faculdade, erroResiduo.DiaDaSemana, erroResiduo.Aula, disciplina,
            //                                dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre, dtInicio);
            //                            RetValue erroRemocaoADResiduo = VerificarErro(connection.GetErrors());


            //                            //Verifica se pode remover hora aula
            //                            Ly_aula_docente.Row rowADResiduo = Ly_aula_docente.QueryFirstRow(connection, "turno = ? AND faculdade = ? AND dia_semana = ? AND aula = ? AND disciplina = ? AND turma = ? AND ano = ? AND semestre = ?",
            //                                dadosTurma.Turno, dadosTurma.Faculdade, erroResiduo.DiaDaSemana, erroResiduo.Aula, disciplina, dadosTurma.Turma,
            //                                dadosTurma.Ano, dadosTurma.Semestre);
            //                            if (rowADResiduo == null)
            //                            {
            //                                Ly_hor_aula.Row rowHAResiduo = Ly_hor_aula.Row.Query(connection, dadosTurma.Turno, dadosTurma.Faculdade, erroResiduo.DiaDaSemana, erroResiduo.Aula, disciplina, dadosTurma.Turma,
            //                                dadosTurma.Ano, dadosTurma.Semestre);
            //                                if (rowHAResiduo != null)
            //                                {
            //                                    TCommand.ExecuteNonQuery(connection,
            //                                      "DELETE FROM ly_hor_aula WHERE turno = ? AND faculdade = ? AND dia_semana = ? AND aula = ? AND disciplina = ? AND turma = ? AND ano = ? AND semestre = ?",
            //                                    dadosTurma.Turno, dadosTurma.Faculdade, erroResiduo.DiaDaSemana, erroResiduo.Aula, disciplina, dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
            //                                }
            //                            }
            //                            RetValue erroRemovaoHAResiduo = VerificarErro(connection.GetErrors());
            //                        }

            //                        #endregion

            //                    }
            //                    connection.Rollback();
            //                    return new RetValue(false, "erro", errorList);
            //                }
            //            }



            return new RetValue(true, "Registro alterado com sucesso.", errorList);
        }

        private static Ly_grupo_habilitacao.Row[] ObterAgrupamentosDisciplina(TConnection connection, decimal num_func, String codigoDisciplina)
        {
            QueryTable qtAgrupamento = new QueryTable(
                    @"select distinct gh.AGRUPAMENTO agrupamento
                            from LY_GRUPO_HABILITACAO gh (NOLOCK)
                            inner join ly_grupo_habilitacao_doc gd (NOLOCK) on gd.AGRUPAMENTO = gh.AGRUPAMENTO
                            inner join LY_GRUPO_HABILITACAO_DISC gdi (NOLOCK) on gdi.AGRUPAMENTO = gh.AGRUPAMENTO
                            where gd.NUM_FUNC = ? and gdi.DISCIPLINA = ? and
                            (gd.provisorio = 'N' or (gd.provisorio = 'S' and gd.dt_limite >= convert(date, getdate())))");
            qtAgrupamento.Query(connection, num_func, codigoDisciplina);
            List<String> agrupamentos = qtAgrupamento.Rows
                .Cast<SimpleRow>()
                .Select(sr => Convert.ToString(sr["agrupamento"])).ToList();

            if (agrupamentos.Count() > 0)
            {
                string sequenciaAgrupamento = "'" + agrupamentos.Aggregate((a, b) => a + "," + b).Replace(",", "','") + "'";
                return Ly_grupo_habilitacao.Query(connection, "agrupamento in (" + sequenciaAgrupamento + ")")
                    .Rows.Cast<Ly_grupo_habilitacao.Row>().ToArray();
            }
            else
                return null;
        }

        /// <summary>
        /// Verifica se a aula docente atual está como 99999999 para poder alocar como 66666666
        /// </summary>
        /// <param name="qtOriginal">quadro de horario original</param>
        /// <param name="linhaAulaDocente">linha da aula docente atual</param>
        /// <returns>true nao existir alocacao anterior como 99999999, false caso contrario</returns>
        private static bool VerificarMatriculaNecessidadeContratoTemporario(QueryTable qtOriginal, Ly_aula_docente.Row linhaAulaDocente, string matricula)
        {
            if (matricula == "66666666")
            {
                SimpleRow[] dadosLinha = qtOriginal.Select("turno = '" + linhaAulaDocente.Turno + "' " +
                                                           " AND faculdade = '" + linhaAulaDocente.Faculdade + "' " +
                                                           " AND dia_semana =  " + linhaAulaDocente.Dia_semana +
                                                           " AND aula = '" + linhaAulaDocente.Aula + "' " +
                                                           " AND turma = '" + linhaAulaDocente.Turma + "' " +
                                                           " AND ano = " + linhaAulaDocente.Ano +
                                                           " AND semestre =  " + linhaAulaDocente.Semestre +
                                                           " AND disciplina = '" + linhaAulaDocente.Disciplina + "'" +
                                                           " AND matricula in ('99999999', '66666666')");
                if (dadosLinha != null && dadosLinha.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private static RetValue AtualizarAulaDocente(TConnectionWritable connection, Ly_aula_docente.Row linhaAulaDocente, DateTime dtFimTurma, int regimeContratacao)
        {
            string carenciaAnterior = string.Empty;
            if (regimeContratacao == (int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario)
            {
                SimpleRow rowAnterior = ObterAulaDocenteAnterior(connection, Convert.ToString(linhaAulaDocente.Num_func.Value), linhaAulaDocente.Turno, linhaAulaDocente.Faculdade, Convert.ToInt32(linhaAulaDocente.Dia_semana.Value), Convert.ToString(linhaAulaDocente.Aula.Value), linhaAulaDocente.Disciplina, linhaAulaDocente.Turma, Convert.ToString(linhaAulaDocente.Ano.Value), Convert.ToString(linhaAulaDocente.Semestre.Value));
                if (rowAnterior != null)
                {
                    carenciaAnterior = Convert.ToString(rowAnterior["matricula"]);

                    int rowsAffectedCT = TCommand.ExecuteNonQuery(connection, @"
                        UPDATE  ly_aula_docente
                        SET     data_fim = ?, stamp_atualizacao = ?, tipo = ?, tipo_docente = ?
                        WHERE   num_func = ? AND
                                turno = ? AND
                                faculdade = ? AND
                                dia_semana = ? AND
                                aula = ? AND
                                disciplina = ? AND
                                turma = ? AND
                                ano = ? AND
                                semestre = ? AND
                                data_inicio = ?",
                          dtFimTurma, DateTime.Now, DBNull.Value, carenciaAnterior,
                          linhaAulaDocente.Num_func, linhaAulaDocente.Turno, linhaAulaDocente.Faculdade, linhaAulaDocente.Dia_semana,
                          linhaAulaDocente.Aula, linhaAulaDocente.Disciplina, linhaAulaDocente.Turma, linhaAulaDocente.Ano, linhaAulaDocente.Semestre,
                          linhaAulaDocente.Data_inicio);
                    return VerificarErro(connection.GetErrors());
                }
            }

            int rowsAffected = TCommand.ExecuteNonQuery(connection, @"
                UPDATE  ly_aula_docente
                SET     data_fim = ?, stamp_atualizacao = ?, tipo = ?
                WHERE   num_func = ? AND
                        turno = ? AND
                        faculdade = ? AND
                        dia_semana = ? AND
                        aula = ? AND
                        disciplina = ? AND
                        turma = ? AND
                        ano = ? AND
                        semestre = ? AND
                        data_inicio = ?",
                dtFimTurma, DateTime.Now, DBNull.Value,
                linhaAulaDocente.Num_func, linhaAulaDocente.Turno, linhaAulaDocente.Faculdade, linhaAulaDocente.Dia_semana,
                linhaAulaDocente.Aula, linhaAulaDocente.Disciplina, linhaAulaDocente.Turma, linhaAulaDocente.Ano, linhaAulaDocente.Semestre,
                linhaAulaDocente.Data_inicio);


            return VerificarErro(connection.GetErrors());
        }

        private static void AtualizaTipoAulaDocente(Ly_aula_docente dtAulaDocenteOriginal, Ly_aula_docente dtAulaDocenteAtual)
        {
            foreach (Ly_aula_docente.Row linhaOriginal in dtAulaDocenteOriginal.Rows)
            {
                // verifica se existe o dado da linha atual no datatable original, caso não exista será adiciona a flag para o tipo com valor "0"              
                Ly_aula_docente.Row linhaAtual = VerificaAulaDocente(dtAulaDocenteAtual, linhaOriginal);
                linhaOriginal.Tipo = (linhaAtual != null) ? "1" : "0";
            }
        }

        private static void AtualizaTipoHoraAula(Ly_hor_aula dtHoraAulaOriginal, Ly_hor_aula dtHoraAulaAtual)
        {
            foreach (Ly_hor_aula.Row linhaOriginal in dtHoraAulaOriginal.Rows)
            {
                // verifica se existe o dado da linha atual no datatable original, caso não exista será adiciona a flag para o tipo com valor "0"             
                Ly_hor_aula.Row linhaAtual = VerificaHoraAula(dtHoraAulaAtual, linhaOriginal);
                linhaOriginal.Tipo = (linhaAtual != null) ? "1" : "0";
            }
        }

        private static Ly_hor_aula.Row VerificaHoraAula(Ly_hor_aula dtHoraAula, Ly_hor_aula.Row linha)
        {
            return dtHoraAula.Rows.Find(linha.Turno, linha.Faculdade, linha.Dia_semana, linha.Aula, linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre);
        }

        public static Ly_aula_docente PopularLyAulaDocente(QueryTable qtDados, decimal? num_func)
        {
            if (qtDados == null) return new Ly_aula_docente();

            Ly_aula_docente dtAulaDocenteDestino = new Ly_aula_docente();
            foreach (SimpleRow linhaDados in qtDados.Rows)
            {
                Ly_aula_docente.Row linhaDestino = dtAulaDocenteDestino.NewRow();
                PopularLinhaAulaDocente(linhaDados, linhaDestino);

                if (num_func.HasValue)
                {
                    linhaDestino.Num_func = num_func;
                }

                if (VerificaAulaDocente(dtAulaDocenteDestino, linhaDestino) == null)
                {
                    dtAulaDocenteDestino.Rows.Add(linhaDestino);
                }
            }
            return dtAulaDocenteDestino;
        }

        /// <summary>
        /// Insere ou atualiza os registros de aula docente para a turma
        /// </summary>
        /// <param name="connection">conexao</param>
        /// <param name="dtAulaDocente">datatable de aula docente</param>
        /// <param name="dtFimTurma">data final da turma</param>
        /// <param name="dtInicioTurma">data inicial da turma</param>
        /// <returns>nulo se nao ocorrer erro</returns>
        private static RetValue AtualizarAulaDocenteAtual(TConnectionWritable connection, Ly_aula_docente dtAulaDocente, DateTime dtFimTurma, DateTime dtInicioTurma)
        {
            RetValue valorRetorno = null;
            foreach (Ly_aula_docente.Row linha in dtAulaDocente.Rows)
            {
                valorRetorno = AtualizarLinhaAulaDocente(connection, linha, dtFimTurma, dtInicioTurma);
                if (valorRetorno != null)
                {
                    return valorRetorno;
                }
            }
            return valorRetorno;
        }

        /// <summary>
        /// Verifica se para os dados de aula docente existe registro, no caso de nao existir será inserido um novo registro, caso contrario será atualizado.
        /// </summary>
        /// <param name="connection">conexao com base</param>
        /// <param name="linha">datarow com dados da aula docente</param>
        /// <param name="dtFimTurma">data final da turma</param>
        /// <param name="dtInicioTurma">data inicial da turma</param>
        /// <returns></returns>
        private static RetValue AtualizarLinhaAulaDocente(TConnectionWritable connection, Ly_aula_docente.Row linha, DateTime dtFimTurma, DateTime dtInicioTurma)
        {
            RetValue valorRetorno = null;
            DateTime dtInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

            //caso a data de inicio da turma seja maior que a data de hoje, será usado a data inicial da turma
            linha.Data_inicio = (dtInicio < dtInicioTurma) ? dtInicioTurma : dtInicio;

            string sql = @"SELECT data_inicio FROM ly_aula_docente ad inner join 
                        LY_TURMA t (NOLOCK) on ad.disciplina = t.disciplina AND 
                            ad.turma = t.turma AND 
                            ad.ano = t.ano AND 
                            ad.semestre = t.semestre AND 
                            ad.data_fim = t.dt_fim
                        WHERE ad.num_func = ? AND ad.turno = ? AND ad.faculdade = ? AND ad.dia_semana = ? 
                        AND ad.aula = ? AND ad.disciplina = ? AND ad.turma = ? AND ad.ano = ? AND ad.semestre = ?
                        AND t.sit_turma = 'Aberta'";
            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, linha.Num_func.Value, linha.Turno,
                linha.Faculdade, linha.Dia_semana, linha.Aula, linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre);
            if (!valorConsulta.IsNull)
                linha.Data_inicio = Convert.ToDateTime(valorConsulta);

            if (!VerificarAulaDocente(connection, linha.Num_func.Value, linha.Turno, linha.Faculdade, linha.Dia_semana.Value, linha.Aula.Value, linha.Disciplina, linha.Turma, linha.Ano.Value, linha.Semestre.Value, linha.Data_inicio.Value))
            {
                if (!string.IsNullOrEmpty(linha.Disciplina))
                {
                    linha.Data_fim = new DateTime(dtFimTurma.Year, dtFimTurma.Month, dtFimTurma.Day, 0, 0, 0, 0);
                    Ly_aula_docente.Row.Insert(connection, linha.Num_func, linha.Turno, linha.Faculdade, linha.Dia_semana,
                                               linha.Aula, linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, linha.Data_inicio,
                                               "data_fim, stamp_atualizacao", linha.Data_fim, DateTime.Now);

                    valorRetorno = VerificarErro(connection.GetErrors());
                    if (valorRetorno != null)
                        return valorRetorno;
                }
            }
            else
            {
                DateTime dtFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

                //caso a data de fim da turma seja maior que a data de hoje, será usado a data final da turma
                linha.Data_fim = (dtFim < dtFimTurma) ? dtFimTurma : dtFim;

                Ly_aula_docente.Row.Update(connection, linha.Num_func, linha.Turno, linha.Faculdade, linha.Dia_semana,
                                           linha.Aula, linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, linha.Data_inicio,
                                           "data_fim, stamp_atualizacao", linha.Data_fim, DateTime.Now);
                valorRetorno = VerificarErro(connection.GetErrors());
                if (valorRetorno != null)
                {
                    return valorRetorno;
                }
            }

            return valorRetorno;
        }

        private static bool VerificarAulaDocente(TConnection connection, decimal num_func, string turno, string faculdade, decimal dia_semana, decimal aula, string disciplina, string turma, decimal ano, decimal semestre, DateTime dtInicio)
        {
            string sql = @"SELECT TOP 1 1 FROM ly_aula_docente ad 
                        WHERE ad.num_func = ? AND ad.turno = ? AND ad.faculdade = ? AND ad.dia_semana = ? 
                        AND ad.aula = ? AND ad.disciplina = ? AND ad.turma = ? AND ad.ano = ? AND ad.semestre = ? AND ad.data_inicio = ?";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, num_func, turno, faculdade, dia_semana, aula, disciplina, turma, ano, semestre, dtInicio);
            if (!valorConsulta.IsNull)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static RetValue AtualizarAulaDocenteTipoAtual(TConnectionWritable connection, Ly_aula_docente_tipo dtAulaDocenteTipo, Ly_aula_docente dtAulaDocente, DateTime dtFimTurma, DateTime dtInicioTurma)
        {
            RetValue valorRetorno = null;

            foreach (Ly_aula_docente_tipo.Row linha in dtAulaDocenteTipo.Rows)
            {
                DateTime dtInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
                String carenciaAnterior = "";

                SimpleRow rowAnterior = ObterAulaDocenteAnterior(connection, Convert.ToString(linha.Num_func.Value), linha.Turno, linha.Faculdade, Convert.ToInt32(linha.Dia_semana.Value), Convert.ToString(linha.Aula.Value), linha.Disciplina, linha.Turma, Convert.ToString(linha.Ano.Value), Convert.ToString(linha.Semestre.Value));
                if (rowAnterior != null)
                {
                    if (Convert.ToString(rowAnterior["matricula"]) == "99999999" || Convert.ToString(rowAnterior["matricula"]) == "00000000")
                    {
                        carenciaAnterior = Convert.ToString(rowAnterior["matricula"]);
                    }
                    else
                    {
                        return new RetValue(false, "", new ErrorList("Não foi possível atualizar a GLP, matricula anterior não é de carência."));
                    }
                }
                if (TCommand.ExecuteScalar(connection, "SELECT TOP 1 1 FROM ly_tipo_docente (NOLOCK) WHERE tipo_docente = ?", carenciaAnterior).IsNull)
                {
                    TCommand.ExecuteNonQuery(connection, "INSERT INTO ly_tipo_docente(tipo_docente) VALUES(?)", carenciaAnterior);
                }

                int rowsAffected = TCommand.ExecuteNonQuery(connection,
                    @"  UPDATE  TOP (1) ad 
                        SET     ad.tipo_docente = ? 
                        FROM    ly_aula_docente ad
                        WHERE                            
                            ad.num_func = ? AND
                            ad.turno = ? AND
                            ad.faculdade = ? AND
                            ad.dia_semana = ? AND
                            ad.aula = ? AND
                            ad.disciplina = ? AND
                            ad.turma = ? AND
                            ad.ano = ? AND
                            ad.semestre = ? AND
                            ad.data_fim = ?",
                    carenciaAnterior,
                    linha.Num_func, linha.Turno, linha.Faculdade, linha.Dia_semana, linha.Aula,
                    linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, dtFimTurma);
                valorRetorno = VerificarErro(connection.GetErrors());
                if (valorRetorno != null)
                    return valorRetorno;

                //caso a data de fim da turma seja maior que a data de hoje, será usado a data final da turma
                linha.Data_inicio = (dtInicio < dtInicioTurma) ? dtInicioTurma : dtInicio;

                if (!VerificarAulaDocenteTipo(connection, linha.Num_func.Value, linha.Turno, linha.Faculdade, linha.Dia_semana.Value, linha.Aula.Value, linha.Disciplina, linha.Turma, linha.Ano.Value, linha.Semestre.Value, linha.Data_inicio.Value))
                {
                    rowsAffected = TCommand.ExecuteNonQuery(connection, @"
                        INSERT INTO ly_aula_docente_tipo(num_func,turno,faculdade,dia_semana,
                            aula,disciplina,turma,ano,semestre,data_inicio,
                            data_fim,tipo_aula,id_docente_funcao_glp)
                        VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?)",
                        linha.Num_func, linha.Turno, linha.Faculdade, linha.Dia_semana,
                        linha.Aula, linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, linha.Data_inicio,
                        linha.Data_fim, linha.Tipo_aula, linha.Id_docente_funcao_glp
                    );

                    valorRetorno = VerificarErro(connection.GetErrors());
                    if (valorRetorno != null)
                    {
                        return valorRetorno;
                    }
                }
                else
                {
                    DateTime dtFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);

                    //caso a data de fim da turma seja maior que a data de hoje, será usado a data final da turma
                    linha.Data_fim = (dtFim < dtFimTurma) ? dtFimTurma : dtFim;

                    rowsAffected = TCommand.ExecuteNonQuery(connection, @"
                        UPDATE  ly_aula_docente_tipo
                        SET     data_fim = ?, tipo_aula = ?, id_docente_funcao_glp = ?
                        WHERE   num_func = ? AND  
                                turno = ? AND  
                                faculdade = ? AND  
                                dia_semana = ? AND 
                                aula = ? AND  
                                disciplina = ? AND  
                                turma = ? AND  
                                ano = ? AND  
                                semestre = ? AND  
                                data_inicio = ?",
                            linha.Data_fim, linha.Tipo_aula, linha.Id_docente_funcao_glp,
                            linha.Num_func, linha.Turno, linha.Faculdade, linha.Dia_semana,
                            linha.Aula, linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, linha.Data_inicio);
                    valorRetorno = VerificarErro(connection.GetErrors());
                    if (valorRetorno != null)
                    {
                        return valorRetorno;
                    }
                }

                if (dtAulaDocente.Rows.Cast<Ly_aula_docente.Row>().Count(r =>
                    r.Turno == linha.Turno && r.Faculdade == linha.Faculdade && r.Dia_semana == linha.Dia_semana &&
                    r.Aula == linha.Aula && r.Turma == linha.Turma && r.Ano == linha.Ano &&
                    r.Semestre == linha.Semestre && r.Disciplina == linha.Disciplina) > 0)
                {
                    rowsAffected = TCommand.ExecuteNonQuery(connection, @"
                        UPDATE  ly_docente_funcao_glp
                        SET     glp_usada = ISNULL(glp_usada, 0) + 1
                        WHERE   id_docente_funcao_glp = ?", linha.Id_docente_funcao_glp);

                    valorRetorno = VerificarErro(connection.GetErrors());
                    if (valorRetorno != null)
                    {
                        return valorRetorno;
                    }

                    if (rowsAffected != 1)
                    {
                        return new RetValue(false, "", new ErrorList("Não foi possível atualizar a GLP usada."));
                    }
                }
            }
            return valorRetorno;
        }

        private static bool VerificarAulaDocenteTipo(TConnection connection, decimal num_func, string turno, string faculdade, decimal dia_semana, decimal aula, string disciplina, string turma, decimal ano, decimal semestre, DateTime data_inicio)
        {
            Ly_aula_docente_tipo.Row row = Ly_aula_docente_tipo.Row.Query(connection, num_func, turno, faculdade, dia_semana, aula, disciplina, turma, ano, semestre, data_inicio);
            return row != null;
        }

        private static RetValue AtualizarHoraAulaAtual(TConnectionWritable connection, Ly_hor_aula dtHorAula)
        {
            RetValue valorRetorno = null;

            //obtém registros de hora aula que não foram excluídos
            Ly_hor_aula dtHoraAulaAtual = ObterHorAula(dtHorAula);

            //será verificado para cada registro encontrado se ele existe na base para inserir
            foreach (Ly_hor_aula.Row linha in dtHoraAulaAtual.Rows)
            {
                //formatando a hora
                linha.Horaini_aula = new DateTime(1899, 12, 30, linha.Horaini_aula.Value.Hour, linha.Horaini_aula.Value.Minute, linha.Horaini_aula.Value.Second, linha.Horaini_aula.Value.Millisecond);
                linha.Horafim_aula = new DateTime(1899, 12, 30, linha.Horafim_aula.Value.Hour, linha.Horafim_aula.Value.Minute, linha.Horafim_aula.Value.Second, linha.Horafim_aula.Value.Millisecond);
                if (!VerificarHoraAula(connection, linha.Turno, linha.Faculdade, linha.Dia_semana.Value, linha.Aula.Value, linha.Disciplina, linha.Turma, linha.Ano.Value, linha.Semestre.Value))
                {
                    Ly_hor_aula.Row.Insert(connection, linha.Turno, linha.Faculdade, linha.Dia_semana, linha.Aula,
                                           linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, "dependencia, horaini_aula, horafim_aula, Qtde_aula", linha.Dependencia, linha.Horaini_aula, linha.Horafim_aula, linha.Qtde_aula);

                    valorRetorno = VerificarErro(connection.GetErrors());
                    if (valorRetorno != null)
                        return valorRetorno;
                }
                else
                {
                    Ly_hor_aula.Row.Update(connection, linha.Turno, linha.Faculdade, linha.Dia_semana, linha.Aula,
                        linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, "dependencia, horaini_aula, horafim_aula, Qtde_aula",
                        linha.Dependencia, linha.Horaini_aula, linha.Horafim_aula, linha.Qtde_aula);
                    valorRetorno = VerificarErro(connection.GetErrors());
                    if (valorRetorno != null)
                        return valorRetorno;
                }
            }
            return valorRetorno;
        }

        private static bool VerificarHoraAula(TConnection connection, string turno, string faculdade, decimal dia_semana, decimal aula, string disciplina, string turma, decimal ano, decimal semestre)
        {
            string sql = "SELECT TOP 1 1 FROM ly_hor_aula WHERE turno = ? AND faculdade = ? AND dia_semana = ? AND aula = ? " +
                         " AND disciplina = ? AND turma = ? AND ano = ? AND semestre = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, turno, faculdade, dia_semana, aula, disciplina, turma, ano, semestre);
            if (!valorConsulta.IsNull)
                return true;
            else
                return false;
        }

        private static Ly_aula_docente_tipo ConsultarLyAulaDocenteTipo(TConnection connection, string turno, string faculdade, string turma, decimal ano, decimal semestre)
        {
            return Ly_aula_docente_tipo.Query(connection, "turno = ? AND faculdade = ? AND turma = ? AND ano = ? AND semestre = ? AND tipo_aula = 'GLP'", turno, faculdade, turma, ano, semestre);
        }

        private static Ly_aula_docente ObterAulaDocente(TConnection connection, decimal num_func, DateTime dtIni, DateTime dtFim, bool glp)
        {
            StringBuilder sql = new StringBuilder();
            if (!glp)
            {
                sql.Append(@"
                    SELECT
                        AD.TURNO, 
                        AD.FACULDADE, 
                        AD.DIA_SEMANA, 
                        AD.AULA,
                        AD.DISCIPLINA, 
                        AD.TURMA, 
                        AD.ANO,
                        AD.SEMESTRE, 
                        AD.NUM_FUNC, 
                        AD.DATA_INICIO, 
                        AD.DATA_FIM
                    FROM 
                        ly_aula_docente AD
                    INNER JOIN LY_TURMA T (NOLOCK) ON 
                        T.TURMA = AD.TURMA AND  
                        T.FACULDADE = AD.FACULDADE AND 
                        T.ANO = AD.ANO AND 
                        T.SEMESTRE = AD.SEMESTRE AND 
                        T.TURNO = AD.TURNO AND 
                        T.DISCIPLINA = AD.DISCIPLINA AND 
                        T.DT_FIM = AD.DATA_FIM
                    WHERE 
                        AD.NUM_FUNC = ? AND
                        AD.DATA_INICIO <= ? AND
                        AD.DATA_FIM >= ? AND
                        T.SIT_TURMA = 'Aberta'
                    EXCEPT ");
            }

            sql.Append(@"
                SELECT
                    AD.TURNO, 
                    AD.FACULDADE, 
                    AD.DIA_SEMANA, 
                    AD.AULA, 
                    AD.DISCIPLINA, 
                    AD.TURMA, 
                    AD.ANO, 
                    AD.SEMESTRE, 
                    AD.NUM_FUNC, 
                    AD.DATA_INICIO, 
                    AD.DATA_FIM 
                FROM ly_aula_docente AD 
                INNER JOIN LY_TURMA T (NOLOCK) ON 
                    T.TURMA = AD.TURMA AND 
                    T.FACULDADE = AD.FACULDADE AND 
                    T.ANO = AD.ANO AND 
                    T.SEMESTRE = AD.SEMESTRE AND 
                    T.TURNO = AD.TURNO AND 
                    T.DISCIPLINA = AD.DISCIPLINA AND 
                    T.DT_FIM = AD.DATA_FIM 
                INNER JOIN LY_AULA_DOCENTE_TIPO ADT ON 
                    ADT.turno = AD.turno AND 
                    ADT.FACULDADE = AD.FACULDADE AND 
                    ADT.DIA_SEMANA = AD.DIA_SEMANA AND 
                    ADT.AULA = AD.AULA AND 
                    ADT.DISCIPLINA = AD.DISCIPLINA AND 
                    ADT.TURMA = AD.TURMA AND 
                    ADT.ANO = AD.ANO AND 
                    ADT.SEMESTRE = AD.SEMESTRE AND 
                    ADT.NUM_FUNC = AD.NUM_FUNC
                WHERE 
                    AD.NUM_FUNC = ? AND
                    ADT.TIPO_AULA = 'GLP' AND
                    AD.DATA_INICIO <= ? AND
                    AD.DATA_FIM >= ? AND
                    T.SIT_TURMA = 'Aberta'");

            QueryTable qt = new QueryTable(sql.ToString());

            if (!glp)
            {
                qt.Query(connection, num_func, dtFim, dtIni, num_func, dtFim, dtIni);
            }
            else
            {
                qt.Query(connection, num_func, dtFim, dtIni);
            }

            Ly_aula_docente dtAulaDocente = PopularLyAulaDocente(qt, null);

            return dtAulaDocente;
        }

        private static int CalcularHorarioPessoa(TConnection connection, decimal ano, decimal semestre, decimal num_func, DateTime dtInicio, DateTime dtFim)
        {
            QueryTable qt = new QueryTable(@"
                DECLARE @num_func T_NUMFUNC = ?
                DECLARE @pessoa T_NUMERO                            

                SELECT @pessoa = pessoa FROM ly_docente (NOLOCK) WHERE num_func = @num_func
                
                SELECT 
                    COUNT(1)
                FROM ly_aula_docente ad 
                INNER JOIN ly_turma t (NOLOCK) ON 
                    t.turma = ad.turma AND 
                    t.faculdade = ad.faculdade AND
                    t.ano = ad.ano AND 
                    t.semestre = ad.semestre AND 
                    t.turno = ad.turno AND
                    t.disciplina = ad.disciplina AND 
                    t.dt_fim = ad.data_fim 
                INNER JOIN ly_docente doc (NOLOCK) ON 
                    ad.num_func = doc.num_func
                WHERE                
                    doc.pessoa = @pessoa AND 
                    ad.ano = ? AND 
                    ad.semestre = ? AND
                    ad.data_inicio <= ? AND ad.data_fim >= ?");

            qt.Query(connection, num_func, ano, semestre, dtFim, dtInicio);
            return qt.Rows.Count;
        }

        private static int ObterNumeroAulaDocentePeriodo(TConnection connection, decimal ano, decimal periodo, decimal num_func, DateTime dtIni, DateTime dtFim, bool glp)
        {
            StringBuilder sql = new StringBuilder();

            if (!glp)
            {
                sql.AppendLine(@" 
                    SELECT
                        ad.turno, 
                        ad.faculdade, 
                        ad.dia_semana, 
                        ad.aula, 
                        ad.disciplina, 
                        ad.turma, 
                        ad.ano, 
                        ad.semestre, 
                        ad.num_func, 
                        ad.data_inicio, 
                        ad.data_fim
                    FROM ly_aula_docente ad
                    INNER JOIN ly_turma t (NOLOCK) ON    t.turma = ad.turma AND
                                                t.faculdade = ad.faculdade AND
                                                t.ano = ad.ano AND 
                                                t.semestre = ad.semestre AND 
                                                t.turno = ad.turno AND
                                                t.disciplina = ad.disciplina AND 
                                                t.dt_fim = ad.data_fim 
                    WHERE
                        ad.num_func = ? AND                        
                        CONVERT(DATE, ad.data_fim) >= CONVERT(DATE, ?) AND
                        ad.data_inicio <= ?
                    EXCEPT");
            }

            sql.AppendLine(@"
                SELECT
                    ad.turno, 
                    ad.faculdade, 
                    ad.dia_semana, 
                    ad.aula, 
                    ad.disciplina, 
                    ad.turma, 
                    ad.ano, 
                    ad.semestre, 
                    ad.num_func, 
                    ad.data_inicio, 
                    ad.data_fim 
                FROM ly_aula_docente ad
                INNER JOIN ly_turma t (NOLOCK) ON    t.turma = ad.turma AND 
                                            t.faculdade = ad.faculdade AND 
                                            t.ano = ad.ano AND 
                                            t.semestre = ad.semestre AND 
                                            t.turno = ad.turno AND 
                                            t.disciplina = ad.disciplina AND 
                                            t.dt_fim = ad.data_fim 
                INNER JOIN ly_aula_docente_tipo adt ON  adt.turno = ad.turno AND
                                                        adt.faculdade = ad.faculdade AND
                                                        adt.dia_semana = ad.dia_semana AND
                                                        adt.aula = ad.aula AND
                                                        adt.disciplina = ad.disciplina AND
                                                        adt.turma = ad.turma AND
                                                        adt.ano = ad.ano AND
                                                        adt.semestre = ad.semestre AND
                                                        adt.num_func = ad.num_func AND
                                                        adt.tipo_aula = 'GLP' 
                WHERE 
                    ad.num_func = ? AND                    
                    CONVERT(DATE, ad.data_fim) >= CONVERT(DATE, ?) AND
                    ad.data_inicio <= ?");

            QueryTable qt = new QueryTable(sql.ToString());

            if (!glp)
            {
                qt.Query(connection, num_func, dtIni, dtFim, num_func, dtIni, dtFim);
            }
            else
            {
                qt.Query(connection, num_func, dtIni, dtFim);
            }

            return qt.Rows.Count;
        }

        private static void PopularLinhaAulaDocenteTipo(Ly_aula_docente_tipo.Row linhaDados, Ly_aula_docente_tipo.Row linhaDestino)
        {
            linhaDestino.Ano = linhaDados.Ano;
            linhaDestino.Aula = linhaDados.Aula;
            linhaDestino.Data_fim = linhaDados.Data_fim;
            linhaDestino.Data_inicio = linhaDados.Data_inicio;
            linhaDestino.Dia_semana = linhaDados.Dia_semana;
            linhaDestino.Disciplina = linhaDados.Disciplina;
            linhaDestino.Faculdade = linhaDados.Faculdade;
            linhaDestino.Num_func = linhaDados.Num_func;
            linhaDestino.Semestre = linhaDados.Semestre;
            linhaDestino.Turma = linhaDados.Turma;
            linhaDestino.Turno = linhaDados.Turno;
            linhaDestino.Tipo_aula = linhaDados.Tipo_aula;
        }

        private static void PopularLinhaAulaDocente(SimpleRow linhaDados, Ly_aula_docente.Row linhaDestino)
        {
            linhaDestino.Ano = Convert.ToDecimal(linhaDados["ANO"]);
            linhaDestino.Aula = Convert.ToDecimal(linhaDados["AULA"]);
            linhaDestino.Data_fim = Convert.ToDateTime(linhaDados["DATA_FIM"]);
            linhaDestino.Data_inicio = Convert.ToDateTime(linhaDados["DATA_INICIO"]);
            linhaDestino.Dia_semana = Convert.ToDecimal(linhaDados["DIA_SEMANA"]);
            linhaDestino.Disciplina = Convert.ToString(linhaDados["DISCIPLINA"]);
            linhaDestino.Faculdade = Convert.ToString(linhaDados["FACULDADE"]);
            linhaDestino.Num_func = Convert.ToDecimal(linhaDados["NUM_FUNC"]);
            linhaDestino.Semestre = Convert.ToDecimal(linhaDados["SEMESTRE"]);
            linhaDestino.Turma = Convert.ToString(linhaDados["TURMA"]);
            linhaDestino.Turno = Convert.ToString(linhaDados["TURNO"]);
        }

        public static Boolean NaoExisteAulaDocenteAtiva(TConnection connection, Ly_turma.Row rowTurma)
        {
            SimpleRow rowAD = SimpleRow.QueryFirstRow(connection,
                @"  SELECT TOP 1 1
                    FROM ly_aula_docente ad 
                    INNER JOIN ly_turma t (NOLOCK) ON
                        ad.disciplina = t.disciplina AND
                        ad.turma = t.turma AND
                        ad.ano = t.ano AND
                        ad.semestre = t.semestre AND
                        ad.data_fim = t.dt_fim
                    WHERE
                        ad.turno = ? AND 
                        ad.faculdade = ? AND 
                        ad.turma = ? AND 
                        ad.ano = ? AND 
                        ad.semestre = ? AND 
                        t.sit_turma = 'Aberta'",
                rowTurma.Turno, rowTurma.Faculdade, rowTurma.Turma, rowTurma.Ano, rowTurma.Semestre);
            return rowAD == null;
        }

        public static bool EhReferenciaEletiva(int ano, int periodo, string turma)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(1)
                                    FROM LY_TURMA
                                    WHERE TURMAREFERENCIA = @TURMA
                                    AND ANO = @ANO
                                    AND SEMESTRE = @PERIODO ";

                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

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

        public bool VerificaSeTurmaExiste(DataContext contexto, string turma, int ano, int semestre)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT COUNT(0) 
                                FROM LY_TURMA LYT 
                                WHERE LYT.TURMA = @TURMA
                                      AND ANO = @ANO
                                      AND SEMESTRE = @SEMESTRE ";

            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);

            return (contexto.GetReturnValue<int>(contextQuery) > 0);
        }

        #region Exclusão de Turma

        public static Boolean PodeExcluirTurma(String turno, String faculdade, String turma, String ano, String semestre)
        {
            using (TConnection connection = Config.CreateConnection())
            {
                connection.Open();
                try
                {
                    SimpleRow rowAD = SimpleRow.QueryFirstRow(connection,
                        @"  SELECT TOP 1 1 
                        FROM ly_aula_docente ad
                        INNER JOIN ly_turma t (NOLOCK) ON
                            ad.disciplina = t.disciplina AND
                            ad.turma = t.turma AND
                            ad.ano = t.ano AND
                            ad.semestre = t.semestre AND
                            ad.data_fim = t.dt_fim
                        WHERE
                            ad.turno = ? AND 
                            ad.faculdade = ? AND 
                            ad.turma = ? AND 
                            ad.ano = ? AND 
                            ad.semestre = ? AND
                            t.sit_turma = 'Aberta'",
                        turno, faculdade, turma, ano, semestre);
                    return rowAD == null;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static RetValue ExcluirTurmaComQuadroHorario(Ly_turma dtTurma, Ly_hor_aula dtHoraAula, Ly_aula_docente dtAulaDocente)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            RetValue valorRetorno = null;

            try
            {
                Ly_turma.Row rowTurma = dtTurma.Rows[0];

                valorRetorno = ExcluirTurmaDefinitivamente(connection, rowTurma);
                if (valorRetorno != null && !valorRetorno.Ok)
                {
                    connection.Rollback();
                    return valorRetorno;
                }
            }
            catch (Exception ex)
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Rollback();
                return new RetValue(false, "", new ErrorList(ex.Message));
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }

            return new RetValue(true, "Registro excluído com sucesso.", null);
        }

        public static RetValue ExcluirTurmaDefinitivamente(TConnectionWritable connection, Ly_turma.Row dadosTurma)
        {
            RetValue retorno = null;
            RN.RecursosHumanos.DocenteFuncaoGlpTurma rnDocenteFuncaoGlpTurma = new Techne.Lyceum.RN.RecursosHumanos.DocenteFuncaoGlpTurma();

            try
            {
                #region Validações

                if (!NaoExisteAulaDocenteAtiva(connection, dadosTurma))
                {
                    connection.Rollback();
                    return new RetValue(false,
                                        "Não é possível excluir a turma. Existem aulas alocadas no Quadro de Horários.",
                                        new ErrorList(
                                            "Não é possível excluir a turma. Existem aulas alocadas no Quadro de Horários."));
                }

                //***** TURMA, ANO e SEMESTRE
                Dictionary<String, String> tabTurmaAnoSemestreSituacao = new Dictionary<String, String>();
                tabTurmaAnoSemestreSituacao.Add("LY_MATRICULA", "Existe(m) aluno(s) matriculado(s).");

                //***** TURMA, ANO, SEMESTRE E SITUACAO DA MATRICULA
                Dictionary<String, String> tabMatriculadosFalta = new Dictionary<String, String>();
                tabMatriculadosFalta.Add("LY_FALTA", "Existe(m) falta(s) cadastrada(s) para o(s) aluno(s) da turma.");

                Dictionary<String, String> tabMatriculadosNota = new Dictionary<String, String>();
                tabMatriculadosNota.Add("LY_NOTA", "Existe(m) nota(s) cadastrada(s) para o(s) aluno(s) da turma.");


                //***** TURMA, ANO e SEMESTRE
                Dictionary<String, String> tabTurmaAnoSemestre = new Dictionary<String, String>();

                tabTurmaAnoSemestre.Add("LY_IMAGEM", "Existe(m) frequência(s) cadastrada(s) para a turma.");
                tabTurmaAnoSemestre.Add("LY_COMP_IMAGEM", "Existe(m) frequência(s) cadastrada(s) para a turma.");
                tabTurmaAnoSemestre.Add("LY_COMP_LISTA", "Existe(m) frequência(s) cadastrada(s) para a turma.");
                tabTurmaAnoSemestre.Add("LY_LISTA_FREQ",
                                        "Existe(m) lista(s) de frequência(s) cadastrada(s) para a turma.");

                //***** TURMA, ANO e PERIODO
                Dictionary<String, String> tabTurmaAnoPeriodo = new Dictionary<String, String>();
                tabTurmaAnoPeriodo.Add("LY_FREQ", "Existe(m) frequência(s) cadastrada(s) para o(s) aluno(s) da turma.");


                //***** GRADE_ID     
                Dictionary<String, String> tabGradeID = new Dictionary<String, String>();

                tabGradeID.Add("LY_MATGRADE", "Existe(m) aluno(s) matriculado(s) na turma.");

                foreach (String tabela in tabTurmaAnoSemestreSituacao.Keys)
                {
                    DbObject dbObj = TCommand.ExecuteScalar(connection,
                                                            String.Format(
                                                                @"
                            IF EXISTS (SELECT TOP 1 1 FROM {0} (NOLOCK) WHERE turma = ? AND ano = ? AND semestre = ? AND SIT_MATRICULA <> 'Cancelado' ) SELECT 1
                            ELSE SELECT 0",
                                                                tabela),
                                                            dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
                    if (Convert.ToString(dbObj) == "1")
                    {
                        ErrorList errorList = new ErrorList();
                        errorList.Add("Não é possível excluir a turma. " + tabTurmaAnoSemestreSituacao[tabela], "ERRO");
                        return new RetValue(false, "", errorList);
                    }
                }

                foreach (String tabela in tabTurmaAnoSemestre.Keys)
                {
                    DbObject dbObj = TCommand.ExecuteScalar(connection,
                                                            String.Format(
                                                                @"
                            IF EXISTS (SELECT TOP 1 1 FROM {0} (NOLOCK) WHERE turma = ? AND ano = ? AND semestre = ?) SELECT 1
                            ELSE SELECT 0",
                                                                tabela),
                                                            dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
                    if (Convert.ToString(dbObj) == "1")
                    {
                        ErrorList errorList = new ErrorList();
                        errorList.Add("Não é possível excluir a turma. " + tabTurmaAnoSemestre[tabela], "ERRO");
                        return new RetValue(false, "", errorList);
                    }
                }

                foreach (String tabela in tabTurmaAnoPeriodo.Keys)
                {
                    DbObject dbObj = TCommand.ExecuteScalar(connection,
                                                            String.Format(
                                                                @"
                            IF EXISTS (SELECT TOP 1 1 FROM {0} (NOLOCK) WHERE turma = ? AND ano = ? AND periodo = ?) SELECT 1
                            ELSE SELECT 0",
                                                                tabela),
                                                            dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
                    if (Convert.ToString(dbObj) == "1")
                    {
                        ErrorList errorList = new ErrorList();
                        errorList.Add("Não é possível excluir a turma. " + tabTurmaAnoPeriodo[tabela], "ERRO");
                        return new RetValue(false, "", errorList);
                    }
                }

                foreach (String tabela in tabMatriculadosFalta.Keys)
                {
                    DbObject dbObj = TCommand.ExecuteScalar(connection,
                                                            String.Format(
                                                                @"
                            IF EXISTS (SELECT TOP 1 1 FROM {0} f (NOLOCK) 
                            INNER JOIN dbo.LY_MATRICULA m ON f.ALUNO = m.ALUNO
                                                                     AND f.ANO = m.ANO
                                                                     AND f.PERIODO = m.SEMESTRE
                                                                     AND f.TURMA = m.TURMA
                                                                     AND f.DISCIPLINA = m.DISCIPLINA
                            WHERE m.turma = ? AND m.ano = ? AND m.semestre = ? AND SIT_MATRICULA <> 'Cancelado') SELECT 1
                            ELSE SELECT 0",
                                                                tabela),
                                                            dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
                    if (Convert.ToString(dbObj) == "1")
                    {
                        ErrorList errorList = new ErrorList();
                        errorList.Add("Não é possível excluir a turma. " + tabMatriculadosFalta[tabela], "ERRO");
                        return new RetValue(false, "", errorList);
                    }
                }
                foreach (String tabela in tabMatriculadosNota.Keys)
                {
                    DbObject dbObj = TCommand.ExecuteScalar(connection,
                                                            String.Format(
                                                                @"
                            IF EXISTS (SELECT TOP 1 1 FROM {0} n (NOLOCK) 
                            INNER JOIN dbo.LY_MATRICULA m ON n.ALUNO = m.ALUNO
                                                                     AND n.ANO = m.ANO
                                                                     AND n.SEMESTRE = m.SEMESTRE
                                                                     AND n.TURMA = m.TURMA
                                                                     AND n.DISCIPLINA = m.DISCIPLINA
                            WHERE m.turma = ? AND m.ano = ? AND m.semestre = ? AND SIT_MATRICULA <> 'Cancelado') SELECT 1
                            ELSE SELECT 0",
                                                                tabela),
                                                            dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
                    if (Convert.ToString(dbObj) == "1")
                    {
                        ErrorList errorList = new ErrorList();
                        errorList.Add("Não é possível excluir a turma. " + tabMatriculadosNota[tabela], "ERRO");
                        return new RetValue(false, "", errorList);
                    }
                }

                foreach (String tabela in tabGradeID.Keys)
                {
                    DbObject dbObj = TCommand.ExecuteScalar(connection,
                                                            String.Format(
                                                                @"
                            IF EXISTS(SELECT TOP 1 1 FROM {0} t (NOLOCK)
	                        INNER JOIN ly_grade_serie gs (NOLOCK) ON t.grade_id = gs.grade_id
	                        WHERE gs.grade = ? AND gs.ano = ? AND gs.semestre = ? AND SIT_MATGRADE = 'Matriculado')
	                            SELECT 1
                            ELSE SELECT 0",
                                                                tabela),
                                                            dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
                    if (Convert.ToString(dbObj) == "1")
                    {
                        ErrorList errorList = new ErrorList();
                        errorList.Add("Não é possível excluir a turma. " + tabGradeID[tabela], "ERRO");
                        return new RetValue(false, "", errorList);
                    }
                }

                #endregion

                //Verifica se a turma é referencia de alguma eletiva
                if (EhReferenciaEletiva(Convert.ToInt32(dadosTurma.Ano), Convert.ToInt32(dadosTurma.Semestre), dadosTurma.Turma))
                {
                    ErrorList errorList = new ErrorList();
                    errorList.Add("Não é possível excluir a turma. Pois ela foi utilizada como referência de turma(s) eletiva(s).");
                    return new RetValue(false, "", errorList);
                }

                RN.Turmas.FrequenciaDiaria Freq = new Techne.Lyceum.RN.Turmas.FrequenciaDiaria();
                if (Freq.PossuiFrequenciaDiaria(Convert.ToInt32(dadosTurma.Ano), Convert.ToInt32(dadosTurma.Semestre), dadosTurma.Turma))
                {
                    ErrorList errorList = new ErrorList();
                    errorList.Add("Não é possível excluir a turma, pois existe lançamento de frequência para esta turma.");
                    return new RetValue(false, "", errorList);
                }


                #region Query Recupera a existência de notas por alunos
                string Query = @"SELECT aluno
                    FROM	ly_nota (NOLOCK)
                    WHERE turma = @TURMA 
				    AND ano = @ANO 
				    AND semestre = @SEMESTRE ";

                #endregion
                DataTable dt = ConsultaNotasExistentes(Query, dadosTurma.Turma, dadosTurma.Ano.ToString(), dadosTurma.Semestre.ToString());

                if (dt.Rows.Count > 0)
                {
                    ErrorList errorList = new ErrorList();
                    errorList.Add("Não é possível excluir a(s) prova(s) da turma. Existe(m) nota(s) cadastrada(s).");
                    return new RetValue(false, "", errorList);
                }
                else
                {
                    #region EXCLUI TODAS AS TABELAS RELATIVAS Á TURMAS
                    Query = @" 
				DELETE FROM LY_NOTA 
				WHERE turma = @TURMA 
				AND ano = @ANO 
				AND semestre = @SEMESTRE;
				
				----------------------------------------------------
				
				DELETE  FROM LY_FALTA 
				WHERE turma = @TURMA 
				AND ano = @ANO 
				AND periodo = @SEMESTRE;
				
				----------------------------------------------------
				
				DELETE  FROM LY_FREQ 
				WHERE turma = @TURMA 
				AND ano = @ANO 
				AND periodo = @SEMESTRE; 
				
				----------------------------------------------------
				
				DELETE  FROM LY_PROVA 
				WHERE turma = @TURMA 
				AND ano = @ANO 
				AND SEMESTRE  = @SEMESTRE; 
				
				----------------------------------------------------
				
				DELETE  FROM LY_AULA_DOCENTE_TIPO 
				WHERE turma = @TURMA 
				AND ano = @ANO 
				AND SEMESTRE = @SEMESTRE;
				
				----------------------------------------------------
				
				DELETE  FROM LY_AULA_DOCENTE 
				WHERE turma = @TURMA 
				AND ano = @ANO 
				AND SEMESTRE = @SEMESTRE;
				----------------------------------------------------
				
				DELETE FROM LY_HOR_AULA 
				WHERE turma = @TURMA 
				AND ano = @ANO 
				AND SEMESTRE = @SEMESTRE;
				
				----------------------------------------------------
				
				DELETE FROM LY_MATRICULA 
				WHERE turma = @TURMA 
				AND ano = @ANO 
				AND semestre = @SEMESTRE;
				
				----------------------------------------------------
				
				DELETE T FROM LY_MATGRADE T 
				INNER JOIN ly_grade_serie gs ON T.grade_id = gs.grade_id 
				WHERE gs.grade = @TURMA 
                AND gs.ano = @ANO 
                AND gs.semestre = @SEMESTRE;
				
                ----------------------------------------------------
               				
				DELETE FROM ly_grade_turma 
				WHERE turma = @TURMA 
				AND ano = @ANO 
				AND semestre = @SEMESTRE;
				
				----------------------------------------------------
				
				DELETE FROM ly_grade_serie 
				WHERE grade = @TURMA 
				AND ano = @ANO 
				AND semestre = @SEMESTRE;
				
				----------------------------------------------------
                UPDATE RECURSOSHUMANOS.DOCENTEFUNCAOGLP_TURMA SET  
                ATIVO = 0,
                DATAEXCLUSAOTURMA = GETDATE()
				WHERE TURMA = @TURMA 
				AND ano = @ANO 
				AND PERIODO = @SEMESTRE;
				
				----------------------------------------------------            
				
				DELETE FROM ly_turma 
				WHERE turma = @TURMA 
				AND ano = @ANO 
				AND semestre = @SEMESTRE; ";
                    #endregion
                    ExcluiTurmas(Query, dadosTurma.Turma, dadosTurma.Ano.ToString(), dadosTurma.Semestre.ToString());
                }





                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null && !retorno.Ok)
                { return retorno; }
            }
            catch (Exception exc)
            {
                ErrorList errorList = new ErrorList();
                errorList.Add("Não é possível excluir a turma. Motivo: " + exc.Message, "ERRO");
                return new RetValue(false, "", errorList);
            }

            return retorno;
        }


        public static void ExcluiTurmas(string Query, string Turma, string Ano, string Semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = Query;

                contextQuery.Parameters.Add("@ANO", Ano);
                contextQuery.Parameters.Add("@SEMESTRE", Semestre);
                contextQuery.Parameters.Add("@TURMA", Turma);

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

        public static DataTable ConsultaNotasExistentes(string Query, string Turma, string Ano, string Semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = new DataTable();

            try
            {
                contextQuery.Command = Query;

                contextQuery.Parameters.Add("@ANO", Ano);
                contextQuery.Parameters.Add("@SEMESTRE", Semestre);
                contextQuery.Parameters.Add("@TURMA", Turma);

                dt = ctx.GetDataTable(contextQuery);

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

        #endregion

        public static Ly_hor_aula ConsultarHoraAula(string turma)
        {
            TConnection connection = Config.CreateConnection();
            return Ly_hor_aula.Query(connection, "turma = ?", turma);
        }

        public static Ly_aula_docente ConsultarAulaDocente(string turma)
        {
            TConnection connection = Config.CreateConnection();
            return Ly_aula_docente.Query(connection, "turma = ?", turma);
        }

        public static QueryTable ConsultarAulaDocente(string turno, string faculdade, string turma, decimal ano, decimal semestre)
        {
            var connection = Config.CreateConnection();

            connection.Open();

            var sql = @"SELECT DISTINCT
                                AD.TURNO,
                                AD.FACULDADE,
                                AD.DIA_SEMANA,
                                AD.AULA,
                                AD.DISCIPLINA,
                                D.NOME NOME_DISCIPLINA,
                                AD.TURMA,
                                AD.ANO,
                                AD.SEMESTRE,
                                AD.NUM_FUNC,
                                P.MATRICULA,
                                ( ISNULL(ISNULL((convert(varchar,pe.IDFUNCIONAL) + '/' + convert(varchar,p.VINCULO)),p.matricula) + ' - ', '') + Pe.NOME_COMPL ) NOME_DOCENTE,
                                HA.HORAINI_AULA,
                                HA.HORAFIM_AULA,
                                ADT.TIPO_AULA,
                                AD.TIPO_DOCENTE,
                                P.REGIMECONTRATACAOID
                        FROM    ly_aula_docente AD WITH ( NOLOCK )
                                LEFT JOIN LY_AULA_DOCENTE_TIPO ADT WITH ( NOLOCK ) ON ADT.turno = AD.turno
                                                                                      AND ADT.FACULDADE = AD.FACULDADE
                                                                                      AND ADT.DIA_SEMANA = AD.DIA_SEMANA
                                                                                      AND ADT.AULA = AD.AULA
                                                                                      AND ADT.DISCIPLINA = AD.DISCIPLINA
                                                                                      AND ADT.TURMA = AD.TURMA
                                                                                      AND ADT.ANO = AD.ANO
                                                                                      AND ADT.SEMESTRE = AD.SEMESTRE
                                                                                      AND ADT.NUM_FUNC = AD.NUM_FUNC
                                                                                      AND ADT.TIPO_AULA = 'GLP'
                                INNER JOIN ly_hor_aula HA WITH ( NOLOCK ) ON HA.TURNO = AD.TURNO
                                                                             AND HA.FACULDADE = AD.FACULDADE
                                                                             AND HA.DIA_SEMANA = AD.DIA_SEMANA
                                                                             AND HA.AULA = AD.AULA
                                                                             AND HA.DISCIPLINA = AD.DISCIPLINA
                                                                             AND HA.TURMA = AD.TURMA
                                                                             AND HA.ANO = AD.ANO
                                                                             AND HA.SEMESTRE = AD.SEMESTRE
                                INNER JOIN ly_docente P ( NOLOCK ) ON P.NUM_FUNC = AD.NUM_FUNC
                                INNER JOIN ly_pessoa PE ( NOLOCK ) ON Pe.pessoa = p.pessoa
                                INNER JOIN ly_turma T ( NOLOCK ) ON T.TURMA = AD.TURMA
                                                                    AND T.DT_FIM = AD.DATA_FIM
                                                                    AND T.DISCIPLINA = AD.DISCIPLINA
                                                                    AND T.ANO = AD.ANO
                                                                    AND T.SEMESTRE = AD.SEMESTRE
                                INNER JOIN ly_disciplina D ( NOLOCK ) ON D.DISCIPLINA = ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA)
                        WHERE   ad.FACULDADE = ?
                                AND ad.TURMA = ?
                                AND ad.ANO = ?
                                AND ad.SEMESTRE = ?
                                AND t.sit_turma = 'Aberta'";

            try
            {
                var qt = new QueryTable(sql);

                qt.Query(connection, faculdade, turma, ano, semestre);

                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        private static QueryTable ConsultarAulaDocente(TConnection connection, string turno, string faculdade, string turma, decimal ano, decimal semestre)
        {
            string sql = @"
                        SELECT
                            AD.TURNO, 
                            AD.FACULDADE, 
                            AD.DIA_SEMANA, 
                            AD.AULA,
                            AD.DISCIPLINA, 
                            (AD.DISCIPLINA + ' - ' + D.NOME) NOME_DISCIPLINA, 
                            AD.TURMA, 
                            AD.ANO,
                            AD.SEMESTRE, 
                            AD.NUM_FUNC, 
                            P.MATRICULA, 
                            (ISNULL(P.MATRICULA + ' - ','') + Pe.NOME_COMPL) NOME_DOCENTE,
                            HA.HORAINI_AULA, 
                            HA.HORAFIM_AULA, 
                            AD.DATA_INICIO, 
                            AD.DATA_FIM, 
                            ADT.TIPO_AULA, 
                            T.CURSO, 
                            T.CURRICULO, 
                            T.SERIE, 
                            AD.TIPO,
                            ADT.ID_DOCENTE_FUNCAO_GLP
                        FROM 
                            ly_aula_docente AD (NOLOCK)
                            INNER JOIN ly_hor_aula HA (NOLOCK) ON 
                                HA.TURNO = AD.TURNO
                                AND HA.FACULDADE = AD.FACULDADE
                                AND HA.DIA_SEMANA = AD.DIA_SEMANA
                                AND HA.AULA = AD.AULA
                                AND HA.DISCIPLINA = AD.DISCIPLINA
                                AND HA.TURMA = AD.TURMA
                                AND HA.ANO = AD.ANO
                                AND HA.SEMESTRE = AD.SEMESTRE
                            INNER JOIN ly_docente P (NOLOCK) ON 
                                P.NUM_FUNC = AD.NUM_FUNC
                            INNER JOIN ly_pessoa PE ( NOLOCK ) 
                                ON Pe.pessoa = p.pessoa
                            INNER JOIN ly_disciplina D (NOLOCK) ON 
                                D.DISCIPLINA = AD.DISCIPLINA
                            INNER JOIN ly_turma T (NOLOCK) ON 
                                t.ano = ha.ano
                                AND t.semestre = ha.semestre
                                AND t.turma = ha.turma
                                AND t.disciplina = ha.disciplina
                                AND t.DT_FIM = ad.DATA_FIM
                            LEFT JOIN ly_aula_docente_tipo ADT (NOLOCK) ON 
                                ADT.NUM_FUNC = AD.NUM_FUNC
                                AND ADT.TURNO = AD.TURNO 
                                AND ADT.FACULDADE = AD.FACULDADE 
                                AND ADT.DIA_SEMANA = AD.DIA_SEMANA
                                AND ADT.AULA = AD.AULA 
                                AND ADT.DISCIPLINA = AD.DISCIPLINA 
                                AND ADT.TURMA = AD.TURMA
                                AND ADT.ANO = AD.ANO 
                                AND ADT.SEMESTRE = AD.SEMESTRE
                                AND ADT.TIPO_AULA = 'GLP'
                        WHERE
                            ad.turno = ?
                            AND ad.FACULDADE = ?
                            AND ad.TURMA = ?
                            AND ad.ANO = ? 
                            AND ad.SEMESTRE = ?    
                            AND t.sit_turma = 'Aberta'
ORDER BY AD.TURNO ,
        AD.FACULDADE ,
        AD.DIA_SEMANA ,
        AD.AULA   ";

            QueryTable qt = new QueryTable(sql);
            qt.Query(connection, turno, faculdade, turma, ano, semestre);
            return qt;
        }

        /// <summary>
        /// Obtém a quantidade de alocações ativas em uma determinada turma, para determinada disciplina.
        /// </summary>        
        /// <param name="turno">Turno da turma</param>
        /// <param name="faculdade">Faculdade da turma</param>
        /// <param name="turma">Código da turma</param>
        /// <param name="ano">Ano da turma</param>
        /// <param name="semestre">Semestre da turma</param>
        /// <param name="disciplina">Disciplina da turma</param>        
        private static int? ContagemAlocacoesAtivasPorDisciplina(TConnection connection, string turno, string faculdade, string turma, decimal ano, decimal semestre, string disciplina)
        {
            try
            {
                string sql = @"
                        SELECT COUNT(1)
                        FROM    ly_aula_docente ad INNER JOIN
                                ly_turma t (NOLOCK) ON
                                    ad.disciplina = t.disciplina AND
                                    ad.turma = t.turma AND
                                    ad.ano = t.ano AND
                                    ad.semestre = t.semestre AND
                                    ad.data_fim = t.dt_fim
                        WHERE   t.sit_turma = 'Aberta' AND                                           
                                ad.turno = ? AND
                                ad.FACULDADE = ? AND
                                ad.TURMA = ? AND
                                ad.ANO = ? AND
                                ad.SEMESTRE = ? AND
                                ad.disciplina = ?";

                DbObject contagem = ExecutarFuncao(sql, connection, turno, faculdade, turma, ano, semestre, disciplina);
                if (contagem != null && !contagem.IsNull)
                    return Convert.ToInt32(contagem);
            }
            catch { }
            return (int?)null;
        }

        /// <summary>
        /// Método usado para inclusão de turma
        /// </summary>
        /// <param name="connection">Conexão usada para incluir a turma</param>
        /// <param name="dtTurma">DataTable de Turma com os dados da turma que será incluida</param>
        public static RetValue Incluir(TConnectionWritable connection, Ly_turma dtTurma, string macro)
        {
            RetValue valorRetorno = null;

            //verifica se o datatable de turma não é nulo
            if (dtTurma != null)
            {
                //verifica se existem linhas no datatable de turma
                if (dtTurma.Rows != null)
                {
                    if (dtTurma.Rows.Count > 0)
                    {
                        //cria registros de turma por disciplina relacionadas a grade curricular
                        PopularTurmaPorDisciplina(connection, dtTurma, macro);

                        #region Verifica se já existe turma com o mesmo nome, para o mesmo ano e semestre
                        Ly_turma.Row turmaExistente = Ly_turma.QueryFirstRow(connection, "turma = ? AND ano = ? AND semestre = ?", dtTurma.Rows[0].Turma, dtTurma.Rows[0].Ano, dtTurma.Rows[0].Semestre);
                        Ly_grade_turma.Row gradeTurmaExistente = Ly_grade_turma.QueryFirstRow(connection, "turma = ? AND ano = ? AND semestre = ?", dtTurma.Rows[0].Turma, dtTurma.Rows[0].Ano, dtTurma.Rows[0].Semestre);
                        Ly_grade_serie.Row gradeSerieExistente = Ly_grade_serie.QueryFirstRow(connection, "grade = ? AND ano = ? AND semestre = ?", dtTurma.Rows[0].Turma, dtTurma.Rows[0].Ano, dtTurma.Rows[0].Semestre);

                        if (turmaExistente != null || gradeTurmaExistente != null || gradeSerieExistente != null)
                            return new RetValue(false, "", new ErrorList(String.Format("Não é possível cadastrar a turma. O código de turma gerado {0} já existe para o Ano {1} e Período {2}.", dtTurma.Rows[0].Turma, dtTurma.Rows[0].Ano, dtTurma.Rows[0].Semestre)));

                        #endregion

                        foreach (Ly_turma.Row linhaTurma in dtTurma.Rows)
                        {
                            if (!String.IsNullOrEmpty(linhaTurma.Disciplina))
                            {
                                if (linhaTurma.OptativaReforco == null)
                                {
                                    linhaTurma.OptativaReforco = "N";
                                }

                                if (linhaTurma.Turma.Length > 50)
                                    return new RetValue(false, "", new ErrorList("O código de turma gerado com prefixo + turma + sufixo + -UA não pode exceder o limite de 50 caracteres. \nAtualmente contém: " + linhaTurma.Turma.Length + " caracteres em " + linhaTurma.Turma + "."));

                                //Verifica se é não um turma exclusiva de eletivas
                                if (linhaTurma.Eletiva != "S")
                                {
                                    linhaTurma.TurmaReferencia = null;

                                    //Verifica se a disciplina é eletiva
                                    if (RN.Disciplina.EhDisciplinaEletiva(connection, linhaTurma.Disciplina))
                                    {
                                        linhaTurma.Eletiva = "S"; //Marca a disciplina                                        
                                    }
                                    else
                                    {
                                        linhaTurma.Eletiva = "N";
                                    }
                                }

                                Ly_turma.Row.Insert(connection, linhaTurma.Disciplina, linhaTurma.Turma, linhaTurma.Ano, linhaTurma.Semestre, linhaTurma.Turno,
                                                    linhaTurma.Aulas_previstas, linhaTurma.Min_aulas, linhaTurma.Dt_ultalt, linhaTurma.Dt_inicio, linhaTurma.Dt_fim,
                                                    linhaTurma.Sit_turma, linhaTurma.Unidade_responsavel, linhaTurma.Especial, linhaTurma.Utiliza_indice,
                                                    linhaTurma.Nivel_presenca, linhaTurma.OptativaReforco, linhaTurma.Eletiva, linhaTurma.TurmaReferencia,
                                                    "CURSO, SERIE, FACULDADE, CURRICULO, DEPENDENCIA, NUM_ALUNOS, AULAS_DADAS, CLASSIFICACAO, NIVEL, EM_ELABORACAO, TURMA_INTEGRACAO, DT_CRIACAO,TIPO_GESTAO, AMBIENTE_EXTERNO",
                                                    linhaTurma.Curso, linhaTurma.Serie, linhaTurma.Faculdade, linhaTurma.Curriculo, linhaTurma.Dependencia,
                                                    linhaTurma.Num_alunos, linhaTurma.Aulas_dadas, linhaTurma.Classificacao, linhaTurma.Nivel, DBNull.Value, linhaTurma.Turma_integracao, linhaTurma.Dt_criacao, linhaTurma.Tipo_gestao, linhaTurma.Ambiente_externo);
                                valorRetorno = VerificarErro(connection.GetErrors());
                                if (valorRetorno != null)
                                {
                                    return valorRetorno;
                                }
                            }
                            else
                            {
                                return new RetValue(false, "", new ErrorList("Não há disciplinas cadastradas na matriz curricular."));
                            }
                        }

                        //para a turma incluida será incluido um registro em ly_grade_serie
                        Ly_grade_serie.Row dadosGradeSerie = IncluirGradeSerie(connection, dtTurma.Rows[0]);

                        valorRetorno = VerificarErro(dadosGradeSerie);


                        if (valorRetorno != null)
                        {
                            return valorRetorno;
                        }

                        Ly_grade_serie.Row gradeSerieAtual = Ly_grade_serie.QueryFirstRow(connection, "grade = ? AND ano = ? AND semestre = ?", dtTurma.Rows[0].Turma, dtTurma.Rows[0].Ano, dtTurma.Rows[0].Semestre);

                        if (dadosGradeSerie.Grade_id != gradeSerieAtual.Grade_id)
                        {
                            return new RetValue(false, "grade Id não confere", null);
                        }

                        //para a turma/disciplina incluida será incluido um registro em ly_grade_turma
                        valorRetorno = IncluirGradeTurma(connection, dtTurma, gradeSerieAtual.Grade_id);

                        if (valorRetorno != null)
                        {
                            return valorRetorno;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Método usado para inclusão de turma
        /// </summary>
        /// <param name="dtTurma">DataTable de Turma com os dados da turma que será incluida</param>    

        private static Ly_grade_serie.Row IncluirGradeSerie(TConnectionWritable connection, Ly_turma.Row dadosTurma)
        {
            Ly_grade_serie dtGradeSerie = new Ly_grade_serie();
            Ly_grade_serie.Row dadosGradeSerie = dtGradeSerie.NewRow();

            dadosGradeSerie.Curso = dadosTurma.Curso;
            dadosGradeSerie.Turno = dadosTurma.Turno;
            dadosGradeSerie.Curriculo = dadosTurma.Curriculo;
            dadosGradeSerie.Serie = dadosTurma.Serie;
            dadosGradeSerie.Ano = dadosTurma.Ano;
            dadosGradeSerie.Semestre = dadosTurma.Semestre;
            dadosGradeSerie.Grade = dadosTurma.Turma;
            dadosGradeSerie.Unidade_responsavel = dadosTurma.Unidade_responsavel;
            dadosGradeSerie.Capacidade = dadosTurma.Num_alunos;
            dadosGradeSerie.Dependencia = dadosTurma.Dependencia;
            dadosGradeSerie.Dt_fim = dadosTurma.Dt_fim;
            dadosGradeSerie.Dt_inicio = dadosTurma.Dt_inicio;
            dadosGradeSerie.Faculdade = dadosTurma.Faculdade;
            dadosGradeSerie.Num_func = dadosTurma.Num_func;

            dtGradeSerie.Rows.Add(dadosGradeSerie);
            dtGradeSerie.Put(connection);
            return dtGradeSerie.Rows[0];
        }

        private static RetValue IncluirGradeTurma(TConnectionWritable connection, Ly_turma dtTurma, decimal? gradeId)
        {
            Ly_grade_turma dtGradeTurma = new Ly_grade_turma();
            RetValue valorRetorno = null;



            foreach (Ly_turma.Row dadosTurma in dtTurma.Rows)
            {
                Ly_grade_turma.Row dadosGradeTurma = dtGradeTurma.NewRow();

                dadosGradeTurma.Disciplina = dadosTurma.Disciplina;
                dadosGradeTurma.Turma = dadosTurma.Turma;
                dadosGradeTurma.Ano = dadosTurma.Ano;
                dadosGradeTurma.Semestre = dadosTurma.Semestre;
                dadosGradeTurma.Grade_id = gradeId;

                dtGradeTurma.Rows.Add(dadosGradeTurma);
            }

            if (dtGradeTurma.Rows.Count > 0)
            {
                dtGradeTurma.Put(connection);
                valorRetorno = VerificarErro(dtGradeTurma);
            }

            return valorRetorno;
        }

        /// <summary>
        /// Cria registros de turma por disciplinas encontradas na grade relacionada ao currículo
        /// </summary>
        /// <param name="dtTurma">DataTable de turma que será incluido os registros de disciplina por grade/currículo</param>
        private static void PopularTurmaPorDisciplina(TConnectionWritable connection, Ly_turma dtTurma, string macro)
        {
            QueryTable qtGrade = null;
            //SOMENTE TERA MACRO DISCIPLINAS DO CURSO MAIS EDUCACAO
            if (dtTurma.Rows[0].Curso != "9999.92")
            {
                //pesquisa na grade quais disciplinas estão contidas no curriculo relacionado a turma
                qtGrade = ObterDisciplinaGrade(connection, dtTurma.Rows[0].Curso, dtTurma.Rows[0].Turno,
                                                          dtTurma.Rows[0].Curriculo, dtTurma.Rows[0].Serie.Value);
            }
            else
            {
                qtGrade = ObterDisciplinaGradeMacro(connection, dtTurma.Rows[0].Curso, dtTurma.Rows[0].Turno,
                                           dtTurma.Rows[0].Curriculo, dtTurma.Rows[0].Serie.Value, macro);
            }

            //verifica se o datatable de grade não é nulo e se existem linhas no datatable de grade
            if (qtGrade != null && qtGrade.Rows != null)
            {
                //faz um loop para inserir as disciplinas encontradas dentro da tabela de grade no datatable de turma
                foreach (SimpleRow linhaGrade in qtGrade.Rows)
                {
                    string disciplina = Convert.ToString(linhaGrade["disciplina"]);

                    if (!string.IsNullOrEmpty(disciplina))
                    {
                        //se for o primeiro registro
                        if (dtTurma.Rows.Count == 1)
                        {
                            //será verificado se já existe disciplina relacionada
                            if (string.IsNullOrEmpty(dtTurma.Rows[0].Disciplina))
                            {
                                //caso não exista será associada a disciplina encontrada ao primeiro registro
                                dtTurma.Rows[0].Disciplina = disciplina;

                                //nova instancia de data para deixar hora com valor 00:00:00
                                dtTurma.Rows[0].Dt_inicio = new DateTime(dtTurma.Rows[0].Dt_inicio.Value.Year, dtTurma.Rows[0].Dt_inicio.Value.Month, dtTurma.Rows[0].Dt_inicio.Value.Day);
                                dtTurma.Rows[0].Dt_fim = new DateTime(dtTurma.Rows[0].Dt_fim.Value.Year, dtTurma.Rows[0].Dt_fim.Value.Month, dtTurma.Rows[0].Dt_fim.Value.Day);

                            }
                            else //nos próximos registros será incluido uma turma por disciplina
                                InserirDadosTurma(dtTurma, dtTurma.Rows[0], disciplina);
                        }
                        else //caso não seja o unico registro será incluido uma turma por disciplina
                            InserirDadosTurma(dtTurma, dtTurma.Rows[0], disciplina);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém disciplinas relacionadas a grade de acordo com os parametros informados
        /// </summary>
        /// <param name="connection">conexão com banco lyceum</param>
        /// <param name="curso">curso da turma</param>
        /// <param name="turno">turno da turma</param>
        /// <param name="curriculo">curriculo da turma</param>
        /// <param name="serie">serie da turma</param>
        /// <returns>Disciplinas relacionadas na grade</returns>
        private static QueryTable ObterDisciplinaGrade(TConnection connection, string curso, string turno, string curriculo, decimal serie)
        {
            string sql = @" 
                SELECT  g.disciplina, d.nome, d.aulas_semanais 
                FROM    ly_grade g (NOLOCK) INNER JOIN
                        ly_disciplina d (NOLOCK) ON d.disciplina = g.disciplina
                WHERE   g.curso = ? AND g.turno = ? AND g.curriculo = ? AND g.serie_ideal = ? AND g.OBRIGATORIA = 'S'";

            QueryTable qt = new QueryTable(sql);
            qt.Query(connection, curso, turno, curriculo, serie);

            if (qt != null && qt.Rows.Count > 0)
            {
                return qt;
            }
            else
            {
                return null;
            }
        }

        private static QueryTable ObterTotalDisciplinaGrade(TConnection connection, string curso, string turno, string curriculo, decimal serie)
        {
            string sql = @" 
                SELECT  sum(d.aulas_semanais) AS total_disc_grade
                FROM    ly_grade g (NOLOCK) INNER JOIN
                        ly_disciplina d (NOLOCK) ON d.disciplina = g.disciplina
                WHERE   g.curso = ? AND g.turno = ? AND g.curriculo = ? AND g.serie_ideal = ?";

            QueryTable qt = new QueryTable(sql);
            qt.Query(connection, curso, turno, curriculo, serie);

            if (qt != null && qt.Rows.Count > 0)
            {
                return qt;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtém disciplinas relacionadas a grade de acordo com os parametros informados
        /// </summary>
        /// <param name="connection">conexão com banco lyceum</param>
        /// <param name="curso">curso da turma</param>
        /// <param name="turno">turno da turma</param>
        /// <param name="curriculo">curriculo da turma</param>
        /// <param name="serie">serie da turma</param>
        /// <returns>Disciplinas relacionadas na grade</returns>
        private static QueryTable ObterDisciplinaGradeMacro(TConnection connection, string curso, string turno, string curriculo, decimal serie, string macro)
        {
            string sql = @" 
                SELECT  g.disciplina, d.nome, d.aulas_semanais 
                FROM    ly_grade g (NOLOCK) INNER JOIN
                        ly_disciplina d (NOLOCK) ON d.disciplina = g.disciplina
                WHERE   g.curso = ? AND g.turno = ? AND g.curriculo = ?  AND g.OBRIGATORIA = 'S' AND g.serie_ideal = ? and macro in (" + macro + ")";

            QueryTable qt = new QueryTable(sql);
            qt.Query(connection, curso, turno, curriculo, serie);

            if (qt != null && qt.Rows.Count > 0)
                return qt;
            else
                return null;

        }

        /// <summary>
        /// Replica os dados de turma para a nova linha, mudando somente a disciplina relacionada
        /// </summary>
        /// <param name="dtTurma">DataTable de turma que será incluida a nova linha</param>
        /// <param name="dadosTurma">Linha do datatable que será usada para replicar os dados para a nova linha</param>
        /// <param name="disciplina">Código que será associado a nova linha</param>
        private static void InserirDadosTurma(Ly_turma dtTurma, Ly_turma.Row dadosTurma, string disciplina)
        {
            Ly_turma.Row dadosDisciplinaTurma = dtTurma.NewRow();

            dadosDisciplinaTurma.Tipo_gestao = dadosTurma.Tipo_gestao;
            dadosDisciplinaTurma.Ano = dadosTurma.Ano;
            dadosDisciplinaTurma.Semestre = dadosTurma.Semestre;
            dadosDisciplinaTurma.Turno = dadosTurma.Turno;
            dadosDisciplinaTurma.Curso = dadosTurma.Curso;
            dadosDisciplinaTurma.Curriculo = dadosTurma.Curriculo;
            dadosDisciplinaTurma.Serie = dadosTurma.Serie;
            dadosDisciplinaTurma.Turma = dadosTurma.Turma;
            dadosDisciplinaTurma.Aulas_previstas = 0;
            dadosDisciplinaTurma.Min_aulas = 0;
            dadosDisciplinaTurma.Especial = dadosTurma.Especial;
            dadosDisciplinaTurma.Utiliza_indice = dadosTurma.Utiliza_indice;
            dadosDisciplinaTurma.Nivel_presenca = dadosTurma.Nivel_presenca;
            dadosDisciplinaTurma.Ambiente_externo = dadosTurma.Ambiente_externo;

            dadosDisciplinaTurma.Faculdade = dadosTurma.Faculdade;
            dadosDisciplinaTurma.Num_func = dadosTurma.Num_func;
            dadosDisciplinaTurma.Unidade_responsavel = dadosTurma.Unidade_responsavel;

            dadosDisciplinaTurma.Dependencia = dadosTurma.Dependencia;
            dadosDisciplinaTurma.Num_alunos = dadosTurma.Num_alunos;
            dadosDisciplinaTurma.Aulas_dadas = dadosTurma.Aulas_dadas;

            //nova instancia de data para deixar hora com valor 00:00:00
            dadosDisciplinaTurma.Dt_inicio = new DateTime(dadosTurma.Dt_inicio.Value.Year, dadosTurma.Dt_inicio.Value.Month, dadosTurma.Dt_inicio.Value.Day);
            dadosDisciplinaTurma.Dt_fim = new DateTime(dtTurma.Rows[0].Dt_fim.Value.Year, dtTurma.Rows[0].Dt_fim.Value.Month, dtTurma.Rows[0].Dt_fim.Value.Day);
            dadosDisciplinaTurma.Dt_criacao = DateTime.Now;

            dadosDisciplinaTurma.Dt_ultalt = dadosTurma.Dt_ultalt;
            dadosDisciplinaTurma.Sit_turma = dadosTurma.Sit_turma;

            //associa a disciplina a nova linha
            dadosDisciplinaTurma.Disciplina = disciplina;

            dadosDisciplinaTurma.Turma_integracao = dadosTurma.Turma_integracao;

            dadosDisciplinaTurma.Classificacao = dadosTurma.Classificacao;

            dadosDisciplinaTurma.Eletiva = dadosTurma.Eletiva;
            dadosDisciplinaTurma.TurmaReferencia = dadosTurma.TurmaReferencia;

            dtTurma.Rows.Add(dadosDisciplinaTurma);
        }

        /// <summary>
        /// Método usado para atualização de turma
        /// </summary>
        /// <param name="dtTurma">DataTable de Turma com os dados da turma que será atualizada</param>
        public static RetValue Alterar(TConnectionWritable connection, Ly_turma dtTurma, String velhoTurno)
        {
            RetValue valorRetorno = null;
            //verifica se o datatable de turma não é nulo e se existem linhas no datatable de turma
            if (dtTurma != null && dtTurma.Rows != null)
            {
                Ly_turma.Row dadosTurma = dtTurma.Rows[0];
                String novoTurno = dadosTurma.Turno;
                dadosTurma.Turno = velhoTurno;

                Ly_turma dtTurmaAuxiliar = Consultar(connection, dadosTurma);

                if (dtTurmaAuxiliar != null && dtTurmaAuxiliar.Rows != null)
                {
                    foreach (Ly_turma.Row linhaTurma in dtTurmaAuxiliar.Rows)
                    {
                        linhaTurma.Dependencia = dadosTurma.Dependencia;
                        linhaTurma.Dt_fim = dadosTurma.Dt_fim;
                        linhaTurma.Dt_inicio = dadosTurma.Dt_inicio;
                        linhaTurma.Dt_ultalt = dadosTurma.Dt_ultalt;
                        linhaTurma.Faculdade = dadosTurma.Faculdade;
                        linhaTurma.Num_alunos = dadosTurma.Num_alunos;
                        linhaTurma.Num_func = dadosTurma.Num_func;
                        linhaTurma.Unidade_responsavel = dadosTurma.Unidade_responsavel;
                        linhaTurma.Aulas_dadas = dadosTurma.Aulas_dadas;
                        linhaTurma.Num_alunos = dadosTurma.Num_alunos;
                        linhaTurma.Especial = dadosTurma.Especial;
                        linhaTurma.Tipo_gestao = dadosTurma.Tipo_gestao;
                        linhaTurma.Ambiente_externo = dadosTurma.Ambiente_externo;
                        linhaTurma.OptativaReforco = dadosTurma.OptativaReforco;
                        linhaTurma.Eletiva = dadosTurma.Eletiva;
                        linhaTurma.TurmaReferencia = dadosTurma.TurmaReferencia;

                        //Verifica se é não um turma exclusiva de eletivas
                        if (linhaTurma.Eletiva != "S")
                        {
                            linhaTurma.TurmaReferencia = null;

                            //Verifica se a disciplina é eletiva
                            if (RN.Disciplina.EhDisciplinaEletiva(connection, linhaTurma.Disciplina))
                            {
                                linhaTurma.Eletiva = "S"; //Marca a disciplina  

                                linhaTurma.Disciplina_multipla = linhaTurma.Disciplina_multipla;
                            }
                            else
                            {
                                linhaTurma.Eletiva = "N";
                            }
                        }
                    }
                }

                dtTurmaAuxiliar.Put(connection);

                valorRetorno = VerificarErro(dtTurmaAuxiliar);
                if (valorRetorno != null)
                {
                    return valorRetorno;
                }

                Ly_grade_serie dtGradeSerie = Ly_grade_serie.Query(connection, "curso = ? AND turno = ? AND serie = ? " +
                                                                   " AND ano = ? AND semestre = ? AND grade = ?",
                                                                   dadosTurma.Curso, dadosTurma.Turno, dadosTurma.Serie,
                                                                   dadosTurma.Ano, dadosTurma.Semestre, dadosTurma.Turma);
                if (dtGradeSerie != null)
                {
                    foreach (Ly_grade_serie.Row linha in dtGradeSerie.Rows)
                    {
                        linha.Curso = dadosTurma.Curso;
                        linha.Curriculo = dadosTurma.Curriculo;
                        linha.Serie = dadosTurma.Serie;
                        linha.Ano = dadosTurma.Ano;
                        linha.Semestre = dadosTurma.Semestre;
                        linha.Grade = dadosTurma.Turma;
                        linha.Unidade_responsavel = dadosTurma.Unidade_responsavel;
                        linha.Capacidade = dadosTurma.Num_alunos;
                        linha.Dependencia = dadosTurma.Dependencia;
                        linha.Dt_fim = dadosTurma.Dt_fim;
                        linha.Dt_inicio = dadosTurma.Dt_inicio;
                        linha.Faculdade = dadosTurma.Faculdade;
                        linha.Num_func = dadosTurma.Num_func;
                    }

                    dtGradeSerie.Put(connection);
                    valorRetorno = VerificarErro(dtGradeSerie);

                    if (valorRetorno != null)
                    {
                        return valorRetorno;
                    }
                }

                //Altera o turno da turma
                if (velhoTurno != novoTurno)
                {
                    Ly_turma dtTurmaToUpdate = Ly_turma.Query(connection, "turma = ? AND ano = ? AND semestre = ?",
                        dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
                    foreach (Ly_turma.Row rowTurma in dtTurmaToUpdate.Rows)
                    {
                        Ly_turma.Row.Update(connection, rowTurma.Disciplina, rowTurma.Turma, rowTurma.Ano, rowTurma.Semestre, "turno", novoTurno);
                        valorRetorno = VerificarErro(connection.GetErrors());
                        if (valorRetorno != null && !valorRetorno.Ok)
                        {
                            return valorRetorno;
                        }
                    }

                    Ly_grade_serie dtGradeSerieToUpdate = Ly_grade_serie.Query(connection,
                        "curso = ? AND turno = ? AND curriculo = ? AND serie = ? and ANO = ? AND semestre = ? AND grade = ? AND faculdade = ?",
                        dadosTurma.Curso, velhoTurno, dadosTurma.Curriculo, dadosTurma.Serie, dadosTurma.Ano, dadosTurma.Semestre, dadosTurma.Turma, dadosTurma.Faculdade);
                    foreach (Ly_grade_serie.Row rowGS in dtGradeSerieToUpdate.Rows)
                    {
                        Ly_grade_serie.Row.Update(connection, rowGS.Grade_id, "turno", novoTurno);
                        valorRetorno = VerificarErro(connection.GetErrors());
                        if (valorRetorno != null && !valorRetorno.Ok)
                            return valorRetorno;
                    }
                    dadosTurma.Turno = novoTurno;

                    //Altera o turno dos alunos matriculados (ly_aluno.turno)
                    QueryTable qtAlunos = new QueryTable(@"
                        SELECT 
                            a.aluno 
                        FROM LY_ALUNO a (NOLOCK)
                        INNER JOIN 
                            ly_matgrade mg (NOLOCK) ON 
                                a.aluno = mg.aluno AND 
                                mg.sit_matgrade = 'Matriculado'
                        INNER JOIN 
                            ly_grade_serie gs (NOLOCK) ON 
                                gs.grade_id = mg.grade_id
                        WHERE 
                            gs.grade = ? AND 
                            gs.ano = ? AND 
                            gs.semestre = ? AND 
                            gs.serie = ? AND 
                            gs.turno = ?");
                    qtAlunos.Query(connection, dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre, dadosTurma.Serie, dadosTurma.Turno);
                    foreach (String matriculaAluno in qtAlunos.Rows.Cast<SimpleRow>().Select(r => Convert.ToString(r[0])))
                    {
                        Ly_aluno.Row aluno = Ly_aluno.Row.Query(connection, matriculaAluno);

                        if (aluno != null)
                        {
                            QueryTable qt = new QueryTable(@"UPDATE ly_aluno SET turno = ? WHERE aluno = ?");
                            qt.Query(connection, novoTurno, matriculaAluno);
                            RetValue retAluno = VerificarErro(connection.GetErrors());
                            if (retAluno != null && !retAluno.Ok)
                            {
                                return retAluno;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private static Ly_turma Consultar(TConnection connection, Ly_turma.Row dadosTurma)
        {
            return Ly_turma.Query(connection, "ano = ? AND semestre = ? AND turno = ? AND curso = ? AND curriculo = ? AND serie = ? AND turma = ? ", dadosTurma.Ano, dadosTurma.Semestre, dadosTurma.Turno, dadosTurma.Curso, dadosTurma.Curriculo, dadosTurma.Serie, dadosTurma.Turma);
        }

        // Desativa a turma, alterando seu status SIT_TURMA para 'Desativada'.
        // A turma não pode ter alocações ativas.
        public static RetValue DesativarTurma(String turma, String ano, String semestre, String turno, String faculdade)
        {
            RN.Turma rnTurma = new Turma();
            TConnectionWritable connection = Config.CreateWritableConnection();
            try
            {
                connection.Open(true);

                Ly_turma.Row dadosTurma = new Ly_turma().NewRow();
                dadosTurma.Turma = turma;
                dadosTurma.Ano = Convert.ToDecimal(ano);
                dadosTurma.Semestre = Convert.ToDecimal(semestre);
                dadosTurma.Turno = turno;
                dadosTurma.Faculdade = faculdade;

                if (NaoExisteAulaDocenteAtiva(connection, dadosTurma))
                {
                    if (rnTurma.ObtemAlunosMatriculadosNaTurmaPor(dadosTurma.Turma, dadosTurma.Ano.ToString(), dadosTurma.Semestre.ToString()) <= 0)
                    {
                        TCommand.ExecuteNonQuery(connection,
                            "UPDATE ly_turma SET sit_turma = ? WHERE turma = ? AND ano = ? AND semestre = ? AND sit_turma = ?",
                            "Desativada", dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre, "Aberta");

                        RetValue retUpdate = VerificarErro(connection.GetErrors());
                        if (retUpdate != null && !retUpdate.Ok)
                        {
                            throw new Exception("Não foi possível atualizar o status da turma para 'Desativada'.");
                        }
                        else
                        {
                            return new RetValue(true, "Turma desativada com sucesso.", null);
                        }
                    }
                    else
                    {
                        return new RetValue(false, string.Empty, new ErrorList("Não é possível desativar a turma.\nExistem alunos matriculados."));
                    }
                }
                else
                {
                    return new RetValue(false, string.Empty, new ErrorList("Não é possível desativar a turma.\nExistem aulas alocadas no Quadro de Horários."));
                }
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, string.Empty, new ErrorList("Erro ao desativar turma: " + e.Message));
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        // Reativa uma turma desativada.
        public void ReativaTurmaPor(string turma, string ano, string semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                if (turma.IsNullOrEmptyOrWhiteSpace() || ano.IsNullOrEmptyOrWhiteSpace() || semestre.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new Exception("Parametros Turma / Ano / Periodo não encontrados.");
                }

                contextQuery.Command = @" UPDATE LY_TURMA 
                                        SET    SIT_TURMA = 'Aberta' 
                                        WHERE  TURMA = @TURMA 
                                               AND ANO = @ANO 
                                               AND SEMESTRE = @SEMESTRE
                                               AND SIT_TURMA = 'Desativada'  ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

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

        /// <summary>
        /// Verifica se turma é vigente, com data válida. Retorna booleano resultante de DT_FIM >= HOJE
        /// </summary>        
        public static bool VerificaTurmaVigente(String turma, String ano, String semestre)
        {
            TConnection connection = Config.CreateConnection();

            try
            {
                connection.Open();

                DbObject value = TCommand.ExecuteScalar(connection,
                    @"SELECT TOP 1
	                    (SELECT CASE
		                    WHEN CONVERT(DATE, dt_fim) >= CONVERT(DATE, GETDATE()) THEN 'S'
		                    ELSE 'N' END) vigente
                     FROM ly_turma (NOLOCK) WHERE turma = ? AND ano = ? AND semestre = ?", turma, ano, semestre);
                if (value.IsNull)
                {
                    return false;
                }
                else
                {
                    return Convert.ToChar(value) == 'S';
                }
            }
            catch
            {
                connection.Rollback();
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return false;
        }

        /// <summary>
        /// Método usado para a consulta da turma
        /// </summary>
        /// <param name="connection">Conexão usada no método</param>
        /// <param name="dadosTurma">dados que serão pesquisados</param>
        /// <returns>DataRow de turma com os dados obtidos na consulta</returns>
        public static Ly_turma.Row Consultar(string ano, string semestre, string turno, string curso, string serie, string turma)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                return Ly_turma.QueryFirstRow(connection,
                    "ano = ? AND semestre = ? AND turno = ? AND curso = ? AND serie = ? AND turma = ? ",
                    ano, semestre, turno, curso, serie, turma);
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable Consultar(string ano, string semestre, string turno, string curso, string serie)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            string sql = "SELECT TURMA FROM LY_TURMA (NOLOCK) WHERE ANO= ? AND SEMESTRE = ? AND TURNO = ? AND CURSO = ? AND SERIE = ? GROUP BY TURMA ORDER BY TURMA";

            try
            {
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, ano, semestre, turno, curso, serie);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static Ly_turma Consultar(String turma, object ano, object semestre)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                return Ly_turma.Query(connection, "turma = ? AND ano = ? AND semestre = ?", turma, ano, semestre);
            }
            finally
            {
                connection.Close();
            }
        }

        public static ErrorList ValidarHorario(TConnection connection, Ly_hor_aula.Row dadosHoraAula, Ly_turma.Row dadosTurma, decimal num_func_2_matricula, decimal num_func, string matriculaDocente, string nomeDisciplina)
        {
            ErrorList retorno = null;

            string matricula = RN.Docentes.ObterMatricula(connection, num_func_2_matricula);

            //verifica se algum campo incluido está vazio ou inválido
            retorno = ValidarCampoVazioInvalido(dadosHoraAula, dadosTurma.Curso);
            if (retorno != null)
            {
                return retorno;
            }

            //Verifica se existe conflito de horário para dependência e turma
            retorno = VerificarConflitoHorario(connection, dadosHoraAula, dadosTurma.Dt_inicio.Value, dadosTurma.Dt_fim.Value, dadosTurma.Curso, dadosTurma.Curriculo, dadosTurma.Serie.Value, num_func_2_matricula);
            if (retorno != null)
            {
                return retorno;
            }

            //verifica se o docente está disponível
            retorno = VerificarDisponibilidadeDocente(connection, num_func_2_matricula, dadosHoraAula.Dia_semana.Value, dadosHoraAula.Ano.Value, dadosHoraAula.Semestre.Value, dadosHoraAula.Turma, dadosHoraAula.Horaini_aula.Value, dadosHoraAula.Horafim_aula.Value, dadosTurma.Dt_inicio.Value, dadosTurma.Dt_fim.Value, dadosHoraAula.Aula.Value, num_func, dadosHoraAula.Disciplina);
            if (retorno != null)
            {
                return retorno;
            }

            retorno = RN.Atividade.VerificarDisponibilidadeAtividade(connection, dadosHoraAula.Ano.Value, dadosHoraAula.Semestre.Value, dadosHoraAula.Dia_semana.Value, num_func_2_matricula, dadosHoraAula.Horaini_aula.Value, dadosHoraAula.Horafim_aula.Value, dadosHoraAula.Aula.Value, dadosHoraAula.Disciplina);
            if (retorno != null)
            {
                return retorno;
            }

            retorno = RN.Dependencia.VerificarDisponibilidadeDependencia(connection,
                            dadosHoraAula.Dia_semana.Value,
                            dadosHoraAula.Turno, dadosHoraAula.Ano.Value,
                            dadosHoraAula.Semestre.Value,
                            dadosHoraAula.Horaini_aula.Value,
                            dadosHoraAula.Horafim_aula.Value,
                            dadosTurma.Dt_inicio.Value, dadosTurma.Dt_fim.Value,
                            dadosHoraAula.Faculdade,
                            dadosHoraAula.Dependencia,
                            dadosTurma.Turma,
                            dadosHoraAula.Aula.Value,
                            num_func_2_matricula, dadosHoraAula.Disciplina);
            if (retorno != null)
            {
                return retorno;
            }
            else
            {
                return null;
            }
        }

        private static string ObterDiaSemana(decimal diaSemana)
        {
            switch ((int)diaSemana)
            {
                case 2: return "Segunda";
                case 3: return "Terça";
                case 4: return "Quarta";
                case 5: return "Quinta";
                case 6: return "Sexta";
                case 7: return "Sábado";
                case 1: return "Domingo";
                default: return string.Empty;
            }
        }

        private static ErrorList VerificarDisponibilidadeDocente(TConnection connection, decimal num_func, decimal diaSemana, decimal ano, decimal semestre, string turma, DateTime horaIni, DateTime horaFim, DateTime dtIni, DateTime dtFim, decimal aula,
            decimal num_func_primeira, string codDisciplina_primeira)
        {
            RN.DTOs.DadosDocente dadosDocente = new Techne.Lyceum.RN.DTOs.DadosDocente();
            RN.DTOs.DadosDocente dadosDocentePrimeira = new Techne.Lyceum.RN.DTOs.DadosDocente();
            RN.Docentes rnDocentes = new Docentes();

            string sql = @" SELECT ad.DISCIPLINA, ad.TURMA, ad.ANO, ad.SEMESTRE, ad.NUM_FUNC
                          From LY_HOR_OPER ho
                          INNER JOIN LY_AULA_DOCENTE ad ON 
                            ho.TURNO = ad.TURNO 
                            AND ho.FACULDADE = ad.FACULDADE 
                            AND ho.DIA_SEMANA = ad.DIA_SEMANA 
                            AND ho.AULA = ad.AULA 
                          INNER JOIN LY_TURMA T (NOLOCK) ON 
                            T.TURMA = ad.TURMA 
                            AND T.ANO = AD.ANO 
                            AND T.SEMESTRE = AD.SEMESTRE 
                            AND T.DISCIPLINA = AD.DISCIPLINA 
                            AND T.DT_FIM = AD.DATA_FIM                          
                          WHERE 
                            ad.DIA_SEMANA = ?
                            AND ad.ANO = ?
                            
                            AND ad.TURMA <> ?
                            AND ho.HORAFIM_AULA > ?
                            AND ho.HORAINI_AULA < ?
                            AND ad.NUM_FUNC = ?
                            AND ((ad.DATA_INICIO >= ? AND ad.DATA_INICIO <= ?)
                            OR (ad.DATA_FIM >= ? AND ad.DATA_FIM <= ? )
                            OR (ad.DATA_INICIO <= ? AND ad.DATA_FIM >= ? ))
                            AND t.sit_turma = 'Aberta'
                            AND (ad.DATA_FIM >= GETDATE())";
            //RETIRADO EM 07/04 AFIM DE RESOLVER O PROBLEMA DE PERMISSAO DE CONFLITO DE HORARIO
            //INCLUSAO EM 13/08 PARA VALIDAR AULAS DO EJA(DEMANDA 2941)
            //AND ad.SEMESTRE = ?
            var strHoraIni = new DateTime(1899, 12, 30, horaIni.Hour, horaIni.Minute, horaIni.Second);
            var strHoraFim = new DateTime(1899, 12, 30, horaFim.Hour, horaFim.Minute, horaFim.Second);
            var strDtIni = new DateTime(dtIni.Year, dtIni.Month, dtIni.Day, 0, 0, 0);
            var strDtFim = new DateTime(dtFim.Year, dtFim.Month, dtFim.Day, 0, 0, 0);

            QueryTable qt = new QueryTable(sql); //, semestre
            qt.Query(connection, diaSemana, ano, turma, strHoraIni, strHoraFim, num_func,
                strDtIni, strDtFim, strDtIni, strDtFim, strDtIni, strDtFim);

            if (qt != null && qt.Rows.Count > 0)
            {
                dadosDocente = rnDocentes.ObtemDadosDocentePor(num_func);
                dadosDocentePrimeira = rnDocentes.ObtemDadosDocentePor(num_func_primeira);

                string nomeDisciplina = RN.Disciplina.ObterNomeDisciplina(connection, codDisciplina_primeira);
                string nomeDisciplinaConflito = RN.Disciplina.ObterNomeDisciplina(connection, Convert.ToString(qt.Rows[0]["DISCIPLINA"]));

                string mensagem = aula + "|" + diaSemana + "|O Docente " + dadosDocentePrimeira.Matricula + " está alocado neste horário para: " +
                                  " Disciplina: " + nomeDisciplinaConflito +
                                  " Turma: " + Convert.ToString(qt.Rows[0]["TURMA"]) +
                                  " Ano: " + Convert.ToString(qt.Rows[0]["ANO"]) +
                                  " Semestre: " + Convert.ToString(qt.Rows[0]["SEMESTRE"]) +
                                  " Matrícula: " + dadosDocente.Matricula +
                                  "|" + String.Format("{0:HH:mm}", horaIni) + "|" + String.Format("{0:HH:mm}", horaFim) + "|" + dadosDocentePrimeira.Matricula + " - " + dadosDocentePrimeira.NomeCompleto + "|" + nomeDisciplina;

                ErrorList erro = new ErrorList();
                erro.Add(mensagem, "ERRO_VALIDACAO");
                return erro;
            }
            return null;
        }

        /// <summary>
        /// Verifica se os dados da linha de hora aula estão corretos (preenchidos e válidos)
        /// </summary>
        /// <param name="dadosHoraAula">Dados de hora aula que serão validados</param>
        /// <returns>Objeto RetValue, se retornar null os campos são válidos</returns>
        private static ErrorList ValidarCampoVazioInvalido(Ly_hor_aula.Row dadosHoraAula, string curso)
        {
            string mensagem = string.Empty;

            if (string.IsNullOrEmpty(dadosHoraAula.Faculdade))
            {
                mensagem = "Unidade Responsável";
            }
            else if (string.IsNullOrEmpty(dadosHoraAula.Turno))
            {
                mensagem = "Turno";
            }
            else if (string.IsNullOrEmpty(dadosHoraAula.Dependencia) && (curso != "9999.92" && curso != "9999.98"))
            {
                mensagem = "Dependencia";
            }
            else if (!dadosHoraAula.Aula.HasValue)
            {
                mensagem = "Aula";
            }
            else if (dadosHoraAula.Aula.Value < 0)
            {
                mensagem = "Aula";
            }
            else if (!dadosHoraAula.Dia_semana.HasValue)
            {
                mensagem = "Dia da semana";
            }
            else if (string.IsNullOrEmpty(dadosHoraAula.Turma))
            {
                mensagem = "Turma";
            }
            else if ((dadosHoraAula.Dia_semana.Value < 0) || (dadosHoraAula.Dia_semana.Value > 7))
            {
                mensagem = "Dia da semana";
            }
            else if (!dadosHoraAula.Ano.HasValue)
            {
                mensagem = "Ano";
            }
            else if (dadosHoraAula.Ano.Value < 1900)
            {
                mensagem = "Ano";
            }
            else if (!dadosHoraAula.Semestre.HasValue)
            {
                mensagem = "Semestre";
            }
            else if ((dadosHoraAula.Semestre.Value < 0) || (dadosHoraAula.Semestre.Value > 99))
            {
                mensagem = "Semestre";
            }

            if (!string.IsNullOrEmpty(mensagem))
            {
                ErrorList errorList = new ErrorList();
                errorList.Add("Campo " + mensagem + " inválido ou não preenchido!", "ERRO_VALIDACAO");
                return errorList;
            }
            //return new RetValue(false, "Campo " + mensagem + " inválido ou não preenchido!", null);

            return null;
        }

        /// <summary>
        /// Verifica se existe conflito de horário para para dependencia e turma
        /// </summary>
        /// <param name="dadosHoraAula">dados da hora aula que serão validados</param>
        /// <param name="dtInicio">Data início da turma</param>
        /// <param name="dtFim">data fim da turma</param>
        /// <param name="diaSemana">dia da semana que será incluído a aula</param>
        /// <param name="aula">aula que será verificada</param>
        /// <returns>Objeto RetValue, se retornar null não existe conflito de horário para dependencia e turma</returns>
        private static ErrorList VerificarConflitoHorario(TConnection connection, Ly_hor_aula.Row dadosHoraAula, DateTime dtInicio, DateTime dtFim, string curso, string curriculo, decimal serie, decimal num_func)
        {
            ErrorList errorList = null;
            string matricula = string.Empty;
            StringBuilder mensagem = null;

            //verifica se o horário operacional existe
            QueryTable qt = RN.HorarioOperacional.VerificarHorarioOperacional(connection, dadosHoraAula.Turno, dadosHoraAula.Faculdade, dadosHoraAula.Dia_semana.Value, dadosHoraAula.Aula.Value, curso, curriculo, serie);
            DateTime horaIni, horaFim;

            if (qt == null)
            {
                matricula = RN.Docentes.ObterMatricula(connection, num_func);
                mensagem = MontarMensagem(connection, dadosHoraAula, "Horário Operacional da Aula não cadastrado." +
                      "Dia da semana: " + ObterDiaSemana(dadosHoraAula.Dia_semana.Value) +
                      "Inicio: " + String.Format("{0:HH:mm}", dadosHoraAula.Horaini_aula.Value) +
                      "Término: " + String.Format("{0:HH:mm}", dadosHoraAula.Horafim_aula.Value), matricula, num_func);

                errorList = new ErrorList();
                errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
                return errorList;
            }
            else if (qt.Rows.Count == 0)
            {
                matricula = RN.Docentes.ObterMatricula(connection, num_func);
                mensagem = MontarMensagem(connection, dadosHoraAula, "Horário Operacional da Aula não cadastrado." +
                                          "Dia da semana: " + ObterDiaSemana(dadosHoraAula.Dia_semana.Value) +
                                          "Inicio: " + String.Format("{0:HH:mm}", dadosHoraAula.Horaini_aula.Value) +
                                          "Término: " + String.Format("{0:HH:mm}", dadosHoraAula.Horafim_aula.Value), matricula, num_func);

                errorList = new ErrorList();
                errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
                return errorList;
            }

            if (string.IsNullOrEmpty(Convert.ToString(qt.Rows[0]["HORAINI_AULA"])))
            {
                matricula = RN.Docentes.ObterMatricula(connection, num_func);

                mensagem = MontarMensagem(connection, dadosHoraAula, "Horário Operacional da Aula não cadastrado." +
                                          "Dia da semana: " + ObterDiaSemana(dadosHoraAula.Dia_semana.Value) +
                                          "Inicio: " + String.Format("{0:HH:mm}", dadosHoraAula.Horaini_aula.Value) +
                                          "Término: " + String.Format("{0:HH:mm}", dadosHoraAula.Horafim_aula.Value), matricula, num_func);

                errorList = new ErrorList();
                errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
                return errorList;
            }
            else
            {
                horaIni = Convert.ToDateTime(qt.Rows[0]["HORAINI_AULA"]);
                horaFim = Convert.ToDateTime(qt.Rows[0]["HORAFIM_AULA"]);
            }

            ////verifica se existe conflito de horário para a dependência
            //QueryTable qtDados = RN.Dependencia.VerificarConflitoHorarioDependencia(connection, dadosHoraAula.Faculdade, dadosHoraAula.Dia_semana.Value, dadosHoraAula.Dependencia, dadosHoraAula.Turma, dtInicio, dtFim, horaIni, horaFim);

            //if (qtDados != null)
            //{
            //    if (qtDados.Rows.Count > 0)
            //    {
            //        if (Convert.ToString(qtDados.Rows[0]["DISCIPLINA"]) == dadosHoraAula.Disciplina &&
            //            Convert.ToString(qtDados.Rows[0]["TURMA"]) == dadosHoraAula.Turma &&
            //            Convert.ToDecimal(qtDados.Rows[0]["ANO"]) == dadosHoraAula.Ano.Value &&
            //            Convert.ToDecimal(qtDados.Rows[0]["SEMESTRE"]) == dadosHoraAula.Semestre.Value)
            //        {
            //            if (qtDados.Rows.Count >= 2)
            //            {
            //                matricula = RN.Docentes.ObterMatricula(connection, num_func);

            //                mensagem = MontarMensagem(connection, dadosHoraAula, "Conflito de horário com a turma " + Convert.ToString(qtDados.Rows[0]["TURMA"]) + " da disciplina " + Convert.ToString(qtDados.Rows[0]["DISCIPLINA"]) +
            //                " do ano " + Convert.ToString(qtDados.Rows[0]["ANO"] + " e período " + Convert.ToString(qtDados.Rows[0]["SEMESTRE"]) +
            //                ", na aula " + Convert.ToString(qtDados.Rows[0]["AULA"]) + ", com horário entre " + String.Format("{0:HH:mm}", qtDados.Rows[0]["HORAINI_AULA"])) + " e " + String.Format("{0:HH:mm}", qtDados.Rows[0]["HORAFIM_AULA"]), matricula, num_func);

            //                errorList = new ErrorList();
            //                errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
            //                return errorList;
            //            }
            //        }
            //    }
            //    qtDados.Dispose();
            //}

            //verifica se existe conflito de horario com a turma
            QueryTable qtDados = VerificarConflitoHorarioTurma(connection, dadosHoraAula.Disciplina, dadosHoraAula.Dia_semana.Value, dadosHoraAula.Turma, dadosHoraAula.Ano.Value, dadosHoraAula.Semestre.Value, dadosHoraAula.Turno, horaIni, horaFim, dtFim);

            if (qtDados != null && qtDados.Rows.Count > 0)
            {
                if (Convert.ToString(qtDados.Rows[0]["DISCIPLINA"]) == dadosHoraAula.Disciplina &&
                    Convert.ToString(qtDados.Rows[0]["TURMA"]) == dadosHoraAula.Turma &&
                    Convert.ToDecimal(qtDados.Rows[0]["ANO"]) == dadosHoraAula.Ano.Value &&
                    Convert.ToDecimal(qtDados.Rows[0]["SEMESTRE"]) == dadosHoraAula.Semestre.Value &&
                    Convert.ToDecimal(qtDados.Rows[0]["AULA"]) == dadosHoraAula.Aula.Value &&
                    Convert.ToString(qtDados.Rows[0]["TURNO"]) == dadosHoraAula.Turno &&
                    Convert.ToDecimal(qtDados.Rows[0]["DIA_SEMANA"]) == dadosHoraAula.Dia_semana
                    )
                {
                    if (qtDados.Rows.Count >= 2)
                    {
                        matricula = RN.Docentes.ObterMatricula(connection, num_func);
                        mensagem = MontarMensagem(connection, dadosHoraAula, "Conflito de horário da aula " + Convert.ToString(dadosHoraAula.Aula.Value) + " com a aula " + Convert.ToString(qtDados.Rows[0]["AULA"]) + " na disciplina " + dadosHoraAula.Disciplina, matricula, num_func);
                        errorList = new ErrorList();
                        errorList.Add(mensagem.ToString(), "ERRO_VALIDACAO");
                        return errorList;
                    }
                }
            }
            return null;
        }

        public static StringBuilder MontarMensagem(TConnection connection, Ly_hor_aula.Row dadosHoraAula, string mensagem, string matricula, decimal num_func)
        {
            StringBuilder formato = new StringBuilder();
            RN.Docentes rnDocentes = new Docentes();
            formato.Append(dadosHoraAula.Aula);
            formato.Append("|");
            formato.Append(dadosHoraAula.Dia_semana);
            formato.Append("|");
            formato.Append(mensagem);
            formato.Append("|");
            formato.Append(String.Format("{0:HH:mm}", dadosHoraAula.Horaini_aula));
            formato.Append("|");
            formato.Append(String.Format("{0:HH:mm}", dadosHoraAula.Horafim_aula));
            formato.Append("|");
            formato.Append(matricula);
            formato.Append(" - ");
            formato.Append(rnDocentes.ObtemNomeDocentePorNumFunc(num_func));
            formato.Append("|");
            formato.Append(RN.Disciplina.ObterNomeDisciplina(connection, dadosHoraAula.Disciplina));
            return formato;
        }

        public static StringBuilder MontarMensagem(TConnection connection, Ly_hor_oper.Row horOper, String mensagem, String disciplina, decimal? num_func)
        {
            RN.Docentes rnDocentes = new Docentes();
            RN.DTOs.DadosDocente dadosDocentes = new Techne.Lyceum.RN.DTOs.DadosDocente();

            dadosDocentes = rnDocentes.ObtemDadosDocentePor(Convert.ToDecimal(num_func));

            StringBuilder formato = new StringBuilder();
            formato.Append(horOper.Aula);
            formato.Append("|");
            formato.Append(horOper.Dia_semana);
            formato.Append("|");
            formato.Append(mensagem);
            formato.Append("|");
            formato.Append(String.Format("{0:HH:mm}", horOper.Horaini_aula.Value));
            formato.Append("|");
            formato.Append(String.Format("{0:HH:mm}", horOper.Horafim_aula.Value));
            formato.Append("|");
            formato.Append(dadosDocentes.Matricula);
            formato.Append(" - ");
            formato.Append(dadosDocentes.NomeCompleto);
            formato.Append("|");
            formato.Append(RN.Disciplina.ObterNomeDisciplina(connection, disciplina));
            return formato;
        }

        private static ErrorList MontarErrorListGLP(TConnection connection, Ly_hor_aula.Row rowHoraAula, String mensagem, RN.DTOs.DadosDocente dadosDocente)
        {
            ErrorList errorList = new ErrorList();
            String msg = MontarMensagem(connection, rowHoraAula, mensagem, dadosDocente.Matricula, dadosDocente.DocenteId).ToString();
            errorList.Add(msg, "ERRO_GLP");
            return errorList;
        }

        private static ErrorList MontarInformaGLP(TConnection connection, Ly_hor_aula.Row rowHoraAula, String mensagem, RN.DTOs.DadosDocente dadosDocente)
        {
            ErrorList informaGLP = new ErrorList();
            String msg = MontarMensagem(connection, rowHoraAula, mensagem, dadosDocente.Matricula, dadosDocente.DocenteId).ToString();
            informaGLP.Add(msg, "INFORMA_GLP");
            return informaGLP;
        }
        /// <summary>
        /// [NEW] Analisa código da matrícula e verifica se ela é especial.
        /// </summary>
        /// <param name="matricula">Código da matrícula do docente.</param>
        /// <returns>Retorna true se a matrícula é especial; false caso contrário.</returns>
        private static bool IsMatriculaEspecial(string matricula)
        {
            switch (matricula)
            {
                case "00000000": //CARÊNCIA TEMP.
                case "11111111": //PROF. MUNICÍPIO
                case "22222222": //PROF. DEGASE
                case "44444444": //NÃO MINISTRADA
                case "55555555": //SEM ALOCACAO
                case "55555551": //ALOCAÇÃO CONCLUÍDA 
                case "66666666": //NECESSIDADE DE CONTRATO TEMP.
                case "77777777": //PROFESSOR EM ATUAÇÃO
                case "88888888": //PROF. CONVENIO
                case "99999999": //SEM PROF.
                case "88888880": //PROF. DUPLA ESCOLA
                case "88888881": //PROF. PROEMI  
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// [NEW] Obtém a lotação ativa da matrícula do docente em um determinado período.
        /// </summary>        
        /// <param name="matricula">Código da matrícula do docente.</param>    
        /// <param name="dataInicio">Data inicial do período considerado.</param>
        /// <param name="dataFim">Data final do período considerado.</param>
        /// <returns>Retorna o registro Ly_lotacao.Row da lotação ativa da matrícula do docente. NULL caso não seja encontrado</returns>
        public static Ly_lotacao.Row ObterLotacaoAtiva(TConnection connection, string matricula, DateTime dataInicio, DateTime dataFim)
        {
            return Ly_lotacao.QueryFirstRow(connection,
                @"Ly_lotacao.matricula = ? and convert(date,Ly_lotacao.data_nomeacao) <= convert(date,?) and 
                (Ly_lotacao.data_desativacao is null OR convert(date,Ly_lotacao.data_desativacao) >= convert(date,?)) 
                order by ordem desc",
                matricula, dataInicio, dataFim);
        }

        /// <summary>
        /// [NEW] Obtém a lotação ativa atual da matrícula do docente
        /// </summary>     
        /// <param name="matricula">Código da matrícula do docente.</param>        
        /// <returns>Retorna o registro Ly_lotacao.Row da lotação ativa da matrícula do docente. NULL caso não seja encontrado</returns>
        public static Ly_lotacao.Row ObterLotacaoAtiva(TConnection connection, string matricula)
        {
            return Ly_lotacao.QueryFirstRow(connection,
                @"Ly_lotacao.matricula = ? and Ly_lotacao.data_nomeacao <= convert(date,GETDATE()) and 
                (Ly_lotacao.data_desativacao is null OR convert(date,Ly_lotacao.data_desativacao) >= convert(date,GETDATE())) 
                order by ordem desc",
                matricula);
        }

        /// <summary>
        /// [NEW] Obtém as solicitações GLP que estão aceitas e que possuem saldo disponível.
        /// </summary>        
        /// <param name="matricula">Matrícula do docente.</param>
        /// <param name="unidade_ens">Unidade de Ensino/Unidade Responsável.</param>
        /// <param name="agrupamento">Código do agrupamento da disciplina.</param>
        /// <returns>Retorna um vetor de solicitações GLP disponíveis a serem utilizadas.</returns>
        private static Ly_docente_funcao_glp.Row[] ObterSolicitacoesGLPDisponiveis(TConnection connection, string matricula, string unidade_ens, string agrupamento, decimal? anoTurma, decimal? periodoTurma, string turma, string disciplina, decimal? numFuncAlocacao)
        {
            List<Ly_docente_funcao_glp.Row> solicitacoes = new List<Ly_docente_funcao_glp.Row>();
            QueryTable qtSolicitacoes = new QueryTable(@"
                SELECT DISTINCT
                    df.id_docente_funcao_glp
                FROM    ly_docente_funcao_glp df (NOLOCK) INNER JOIN 
                        ly_docente_funcao_glp_detalhe detalhe (NOLOCK) ON
                            df.id_docente_funcao_glp = detalhe.id_docente_funcao_glp 
                        INNER JOIN [RecursosHumanos].[DOCENTEFUNCAOGLP_TURMA] GT (NOLOCK) ON 
                            GT.id_docente_funcao_glp = DF.id_docente_funcao_glp
                WHERE
                        df.matricula = ? AND 
                        df.status = 'Aceita' AND    
                        df.unidade_ens = ? AND 
                        df.agrupamento = ? AND 
						detalhe.status = 'Aceita' AND
                        gt.ano = ? AND
                        gt. periodo = ? AND
                        gt.turma = ? AND
                        gt.disciplina = ? AND
                        gt.numfunccarencia = ? AND
                        df.ano = ? AND
                        (df.glp_solicitada - isnull(df.glp_usada, 0) > isnull(df.glp_cancelada, 0)) AND
                        (ISNULL(df.prazo, 0) = 0 OR DATEADD(DAY, ISNULL(df.prazo, 0), detalhe.data) >= CONVERT(DATETIME, CONVERT(VARCHAR, GETDATE(), 110)))");
            qtSolicitacoes.Query(connection, matricula, unidade_ens, agrupamento, anoTurma, periodoTurma, turma, disciplina, numFuncAlocacao, anoTurma);
            foreach (SimpleRow rowTmp in qtSolicitacoes.Rows)
            {
                solicitacoes.Add(Ly_docente_funcao_glp.Row.Query(connection, Convert.ToDecimal(rowTmp["id_docente_funcao_glp"])));
            }
            return solicitacoes.ToArray();
        }

        /// <summary>
        /// [NEW] Verifica se o docente possui alguma GLP ativa em alguma
        /// disciplina do tipo Atividades Integradas.
        /// Consulta a tabela ITEMTABELA, TAB = 'AtividadesIntegradas' para
        /// descobrir quais agrupamentos são deste tipo.
        /// </summary>
        /// <param name="connection">Conexão.</param>
        /// <param name="num_func">Código num_func do docente.</param>
        /// <returns>TRUE: possui GLP. FALSE: não possui GLP</returns>
        private static bool VerificaDocentePossuiGLPEmAtividadesIntegradas(TConnection connection, decimal num_func)
        {
            try
            {
                DbObject retorno = TCommand.ExecuteScalar(connection, @"
                    DECLARE @num_func T_NUMFUNC = ?

                    SELECT CASE WHEN EXISTS (
                        SELECT	TOP 1 1
                        FROM	ly_aula_docente_tipo adt (NOLOCK)
                        WHERE 
                            adt.tipo_aula = 'GLP'
                            AND adt.num_func = @num_func
                            AND EXISTS (
                                SELECT	TOP 1 1 
                                FROM	ly_grupo_habilitacao_disc ghd (NOLOCK)
                                WHERE	ghd.disciplina = adt.disciplina
					                    AND EXISTS (
						                    SELECT	TOP 1 1
						                    FROM	itemtabela it (NOLOCK)
						                    WHERE	it.tab = 'AtividadesIntegradas'
								                    AND it.item = ghd.agrupamento
					                    )
                            )
                            AND EXISTS (
                                SELECT	TOP 1 1
                                FROM	ly_turma t (NOLOCK)
                                WHERE	t.disciplina = adt.disciplina
                                        AND t.turma = adt.turma
                                        AND t.ano = adt.ano
                                        AND t.semestre = adt.semestre
                                        AND t.dt_fim = adt.data_fim                                        
                            )
                        ) THEN 'S' ELSE 'N' END POSSUI_GLP", num_func);
                return Convert.ToString(retorno) == "S";
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// Verifica se a categoria/cargo é de 40 Horas.
        /// Consulta a ITEMTABELA, TAB = 'Cargo40H'.
        /// </summary>
        /// <param name="connection">Conexão.</param>
        /// <param name="funcao">Código da categoria/cargo.</param>
        /// <returns>TRUE: é de 40 horas. FALSE: não é de 40 horas.</returns>
        private static bool VerificaCargo40Horas(TConnection connection, string categoria)
        {
            try
            {
                DbObject retorno = TCommand.ExecuteScalar(connection, @"
                    DECLARE @funcao T_CODIGO = ?
                    SELECT CASE WHEN EXISTS (
	                    SELECT	TOP 1 1 
	                    FROM	ITEMTABELA
	                    WHERE	tab = 'Cargo40H'
			                    AND item = @funcao
	                    ) THEN 'S' ELSE 'N' END FUNCAO_40H", categoria);
                return Convert.ToString(retorno) == "S";
            }
            catch { }
            return false;
        }

        private static ErrorList ValidarHoraGLP(TConnectionWritable connection, Ly_hor_aula.Row dadosHoraAula, Ly_turma.Row dadosTurma, decimal num_func, QueryTable qtEstadoOriginal, Ly_aula_docente_tipo dtAulaDocenteTipo, DateTime dataInicioAlocacao)
        {
            RN.GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new GrupoHabilitacaoDoc();
            RN.DTOs.DadosDocente dadosDocente = new Techne.Lyceum.RN.DTOs.DadosDocente();
            RN.Docentes rnDocentes = new Docentes();
            dadosDocente = rnDocentes.ObtemDadosDocentePor(num_func);
            #region Obtém o registro Ly_docente do docente e outras informações do docente
            //Ly_docente.Row rowDocente = Ly_docente.Row.Query(connection, num_func);
            string matricula = dadosDocente.Matricula;
            string categoriaMatricula = dadosDocente.Categoria;
            string docenteReadaptado = ObterReadaptado(connection, dadosDocente.Matricula);
            #endregion

            #region Caso é matrícula especial, não é necessário realizar validações GLP
            if (IsMatriculaEspecial(matricula))
                return null;
            #endregion

            #region Obtém a função da turma, de acordo com o curso e série da turma
            string modalidadeCurso = RN.Curso.ObterModalidadeCurso(connection, dadosTurma.Curso);
            #endregion

            #region Obtém registro ly_lotacao referente à lotação válida do docente
            Ly_lotacao.Row rowLotacao = ObterLotacaoAtiva(connection, dadosDocente.Matricula, dataInicioAlocacao, dadosTurma.Dt_fim.Value);

            if (dadosDocente.Voluntario == "S" && rowLotacao == null)
            {
                return MontarErrorListGLP(connection, dadosHoraAula, String.Format("Não é possível alocar o voluntário {0} - {1}, pois o mesmo não possui lotação ativa para a vigência da turma.", dadosDocente.Matricula, dadosDocente.NomeCompleto), dadosDocente);
            }

            #endregion

            #region Obtém registro ly_funcao refente à lotação
            Ly_funcao.Row rowFuncao = null;
            if (rowLotacao != null)
                rowFuncao = Ly_funcao.QueryFirstRow(connection, "funcao = ?", rowLotacao.Funcao);
            #endregion

            #region Obtém o registro ly_disciplina referente à disciplina que está sendo verificada para alocação
            Ly_disciplina.Row rowDisciplina = Ly_disciplina.Row.Query(connection, dadosHoraAula.Disciplina);
            if (rowDisciplina == null)
                return MontarErrorListGLP(connection, dadosHoraAula, "Disciplina inválida.", dadosDocente);
            #endregion

            #region Obtém quantas matriculas estão relacionadas à pessoa passada como parâmetro
            int numeroMatriculasDocente = ObterNumeroMatriculaDocente(connection, dadosDocente.Pessoa);
            #endregion

            #region Obtém a carga horária permitida para a função do docente
            decimal? chPermitidaFuncao = null;
            decimal? chContrato = null;
            RN.Funcao rnFuncao = new Funcao();

            string funcaoMatricula = rowFuncao != null ? rowFuncao.Funcao : string.Empty;

            if (funcaoMatricula == "220")
                return MontarErrorListGLP(connection, dadosHoraAula, "O docente não pode ser alocado pois sua função ativa não é de Regente.", dadosDocente);

            if (string.IsNullOrEmpty(categoriaMatricula))
                return MontarErrorListGLP(connection, dadosHoraAula, "Docente sem categoria preenchida.", dadosDocente);

            if (dadosDocente.RegimeContratacaoId == 3)
            {
                RN.Docentes rnDocente = new Docentes();
                chContrato = rnDocente.ObtemRegimeTrabalhoPor(matricula);
                if (chContrato == null)
                {
                    chPermitidaFuncao = ObterCargaHorariaNormalSemanal(connection, categoriaMatricula, funcaoMatricula);
                }
                else
                {
                    chPermitidaFuncao = chContrato;
                }
            }
            else
            {
                chPermitidaFuncao = ObterCargaHorariaNormalSemanal(connection, categoriaMatricula, funcaoMatricula);
            }

            if (chPermitidaFuncao == null && !rnFuncao.EhFuncaoSemCHEfetiva(funcaoMatricula))
                return MontarErrorListGLP(connection, dadosHoraAula, "Docente sem carga horária para a função.", dadosDocente);

            #endregion

            #region Obtém licenças ativas do docente
            IEnumerable<Ly_licencas.Row> licencasAtivas = ObterLicencasAtivas(connection, dadosDocente.DocenteId, dadosTurma.Dt_inicio, dadosTurma.Dt_fim);
            #endregion

            #region Verifica se docente possui licença de CH Reduzida. Se sim, 50% da CH é considerada
            if (licencasAtivas.Count(lic => lic.Motivo == "43") > 0)
                if (chPermitidaFuncao.HasValue)
                    chPermitidaFuncao /= 2;
            #endregion

            #region Validação de licença. Se existir ativa (e motivo != 43), bloqueia alocação
            if (licencasAtivas.Count(lic => lic.Motivo != "43") > 0)
            {
                Ly_licencas.Row lic = licencasAtivas.Where(l => l.Motivo != "43").First();
                return MontarErrorListGLP(connection, dadosHoraAula,
                    String.Format("O docente está em uma situação ({0}) na qual não pode ser alocado.", lic.Descricao), dadosDocente);
            }
            #endregion

            #region Obtém os agrupamentos da disciplina para o docente

            Ly_turma.Row rowTurmaDisciplina = Ly_turma.Row.Query(connection, dadosHoraAula.Disciplina, dadosTurma.Turma, dadosTurma.Ano, dadosTurma.Semestre);
            String disciplinaAgrupamento = String.IsNullOrEmpty(rowTurmaDisciplina.Disciplina_multipla) ? dadosHoraAula.Disciplina : rowTurmaDisciplina.Disciplina_multipla;

            Ly_grupo_habilitacao.Row[] agrupamentos = ObterAgrupamentosDisciplina(connection, num_func, disciplinaAgrupamento);
            if (agrupamentos == null)
            {
                Ly_disciplina.Row rowDisciplinaAgrupamento = Ly_disciplina.Row.Query(connection, disciplinaAgrupamento);
                if (rowDisciplinaAgrupamento != null)
                    return MontarErrorListGLP(connection, dadosHoraAula, String.Format("O docente não possui grupo de habilitação cadastrado para a disciplina {0}.", rowDisciplinaAgrupamento.Nome_compl), dadosDocente);
                else
                    return MontarErrorListGLP(connection, dadosHoraAula, String.Format("O docente não possui grupo de habilitação cadastrado para a disciplina {0}.", rowDisciplina.Nome_compl), dadosDocente);
            }
            #endregion

            #region Se for registro novo, verifica se PADACE do usuário considera limite de tempo do Grupo de Habilitação / Verifica se existe limite de tempos de acordo com o Grupo de Habilitação

            if (dadosHoraAula.Tipo == "1")
            {
                string tabelaLimiteAlocacaoQHI = "LimiteAlocacaoQHI";
                string tabelaLiberacaoLimiteAlocacao = "LiberacaoLimAloc";

                String[] padaces = PadroesDeAcessos.ConsultarPadaces(connection, HttpContext.Current.User.Identity.Name);
                String[] padacesLiberados = Itemtabela.Query(connection, "tab = ?", tabelaLiberacaoLimiteAlocacao).Rows.Cast<Itemtabela.Row>().Select(r => r.Item).ToArray();
                if (padaces.Count(p => padacesLiberados.Contains(p)) == 0)
                {
                    foreach (Ly_grupo_habilitacao.Row agrupamento in agrupamentos)
                    {
                        Itemtabela.Row limiteAlocacaoQHI = Itemtabela.QueryFirstRow(connection, "tab = ? AND item = ?", tabelaLimiteAlocacaoQHI, agrupamento.Descricao);
                        if (limiteAlocacaoQHI != null)
                        {
                            String sqlTempos =
                            @"SELECT COUNT(*) FROM ly_aula_docente ad
                              INNER JOIN ly_turma tu (NOLOCK) ON 
                                  ad.disciplina = tu.disciplina AND
                                  ad.turma = tu.turma AND 
                                  ad.ano = tu.ano AND 
                                  ad.semestre = tu.semestre AND
                                  ad.data_fim = tu.dt_fim
                              INNER JOIN LY_GRUPO_HABILITACAO_DISC ghd (NOLOCK) ON 
                                  ad.disciplina = ghd.disciplina
                              WHERE ad.num_func = ? AND 
                                  ad.data_inicio <= ? AND 
                                  ad.data_fim >= ? AND
                                  ghd.agrupamento = ? AND
                                  tu.sit_turma = 'Aberta'
                              GROUP BY ghd.agrupamento";
                            DbObject dboTempos = TCommand.ExecuteScalar(connection, sqlTempos, num_func, dadosTurma.Dt_fim, dadosTurma.Dt_inicio, agrupamento.Agrupamento);
                            decimal nTempos = dboTempos.IsNull ? 0M : (decimal)dboTempos;
                            int? nLimite = !String.IsNullOrEmpty(limiteAlocacaoQHI.Descr) ? Convert.ToInt32(limiteAlocacaoQHI.Descr) : (int?)null;

                            if (nLimite.HasValue && nTempos >= nLimite.Value)
                            {
                                String msg = String.Format("Limite de {0} tempo(s) atingido para a matrícula '{1}' no grupo de habilitação '{2}'.", nLimite, matricula, agrupamento.Descricao);
                                return MontarErrorListGLP(connection, dadosHoraAula, msg, dadosDocente);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Verifica se o Grupo de Habilitação do docente permite alocação Normal
            bool permiteAlocacaoNormal = false, permiteGLP = false;
            DataTable dtHabilitacaoDoc = new DataTable();
            foreach (Ly_grupo_habilitacao.Row agrupamento in agrupamentos)
            {
                dtHabilitacaoDoc = rnGrupoHabilitacaoDoc.ObtemHabilitacaoPor(num_func, agrupamento.Agrupamento);

                if (dtHabilitacaoDoc.Rows.Count > 0)
                {
                    if (dtHabilitacaoDoc.Rows[0]["CAMPO_01"].ToString() == "S")
                    {
                        permiteAlocacaoNormal = true;
                    }
                    if (dtHabilitacaoDoc.Rows[0]["CAMPO_02"].ToString() == "S")
                    {
                        permiteGLP = true;
                    }
                }
            }

            if (!permiteAlocacaoNormal)
            {
                if (!permiteGLP)
                    return MontarErrorListGLP(connection, dadosHoraAula, "O professor não está habilitado nesta disciplina.", dadosDocente);
            }

            #endregion

            #region Obtém a carga horária normal utilizada pelo docente
            int chUsadaMatricula = 1 + ObterNumeroAulaDocentePeriodo(connection, dadosTurma.Ano.Value, dadosTurma.Semestre.Value, num_func, dataInicioAlocacao, dadosTurma.Dt_fim.Value, false);
            #endregion

            //Verifica se é GLP
            if (!chPermitidaFuncao.HasValue || chUsadaMatricula > chPermitidaFuncao)
            {
                #region Validações exclusivas de GLP

                #region Verifica se Grupo de Habilitação do Docente permite GLP
                if (!permiteGLP)
                    return MontarErrorListGLP(connection, dadosHoraAula, "O professor não está habilitado nesta disciplina para atuar em GLP.", dadosDocente);
                #endregion

                if (rowLotacao == null)
                    return MontarErrorListGLP(connection, dadosHoraAula, "O docente não possui lotação ativa.", dadosDocente);

                if (rowFuncao == null)
                    return MontarErrorListGLP(connection, dadosHoraAula, "Função do docente inválida.", dadosDocente);

                if (rowFuncao.Campo_07 == "N" || String.IsNullOrEmpty(rowFuncao.Campo_07))
                    return MontarErrorListGLP(connection, dadosHoraAula, String.Format("A função do docente ({0}) não permite GLP.", rowFuncao.Descricao), dadosDocente);

                #region Verifica se professor pode dar aula: Não possui licença durante o período de existência da turma; se possui licenca não pode bloquear GLP
                foreach (var licenca in licencasAtivas)
                {
                    if (licenca.Bloqueia_glp == "S")
                        return MontarErrorListGLP(connection, dadosHoraAula,
                                String.Format("Este PROFESSOR está alocado em uma situação ({0}) na qual não pode ser alocado em GLP.", licenca.Descricao), dadosDocente);
                }
                #endregion

                #region Verifica se professor é de contrato temporário
                if (!String.IsNullOrEmpty(dadosDocente.Candidato) || !String.IsNullOrEmpty(dadosDocente.Concurso))
                    return MontarErrorListGLP(connection, dadosHoraAula, "Este PROFESSOR é de CONTRATO TEMPORÁRIO e não pode ser alocado em GLP.", dadosDocente);
                #endregion

                #region  Verifica se professor possui 2ª matrícula - Carga horária em aula é menor que carga horária permitida para a função

                QueryTable qt2Matricula = new QueryTable(@"
                    SELECT doc.matricula, doc.num_func FROM ly_docente doc (NOLOCK)
                        INNER JOIN ly_lotacao lot (NOLOCK) ON lot.matricula = doc.matricula AND lot.pessoa = doc.pessoa
                        AND doc.matricula <> ? AND doc.pessoa = ?                        
                        AND lot.data_nomeacao <= convert(date,?)
                        AND (lot.data_desativacao IS NULL OR lot.data_desativacao >= CONVERT(date,?))");
                qt2Matricula.Query(connection, matricula, dadosDocente.Pessoa, dadosTurma.Dt_inicio.Value, dadosTurma.Dt_fim.Value);

                bool matricula2_GLP_AI = false; // indica se a segunda matrícula possui GLP em Atividades Integradas

                if (qt2Matricula.Rows.Count > 0)
                {
                    decimal num_func_2matricula = Convert.ToDecimal(qt2Matricula.Rows[0]["num_func"]);

                    Ly_docente.Row rowDocente2 = Ly_docente.Row.Query(connection, num_func_2matricula);

                    //Obtém registro ly_lotacao referente à lotação válida do docente
                    Ly_lotacao.Row rowLotacao2 = ObterLotacaoAtiva(connection, rowDocente2.Matricula, dataInicioAlocacao, dadosTurma.Dt_fim.Value);
                    if (rowLotacao2 == null)
                        return MontarErrorListGLP(connection, dadosHoraAula, "Lotação da segunda matrícula do docente inválida.", dadosDocente);

                    //Obtém registro ly_funcao refente à lotação encontrada anteriormente
                    Ly_funcao.Row rowFuncao2 = Ly_funcao.QueryFirstRow(connection, "funcao = ?", rowLotacao2.Funcao);
                    string funcao2Matricula = rowFuncao2 != null ? rowFuncao2.Funcao : string.Empty;

                    #region O docente não pode ter cargo de 40 horas em nenhuma das suas 2 matrículas
                    if (VerificaCargo40Horas(connection, dadosDocente.Categoria))
                        return MontarErrorListGLP(connection, dadosHoraAula, "GLP não é permitida pois o docente possui matrícula 40h.", dadosDocente);
                    if (VerificaCargo40Horas(connection, rowDocente2.Categoria))
                        return MontarErrorListGLP(connection, dadosHoraAula, "GLP não é permitida pois o docente possui matrícula 40h.", dadosDocente);
                    #endregion

                    string docenteReadaptado2 = ObterReadaptado(connection, rowDocente2.Matricula);
                    decimal? chTemp2 = ObterCargaHorariaNormalSemanal(connection, rowDocente2.Categoria, funcao2Matricula);
                    decimal chPermitida2 = chTemp2.HasValue ? chTemp2.Value : 0;
                    decimal chAulaDocente2 = ObterNumeroAulaDocentePeriodo(connection, dadosTurma.Ano.Value, dadosTurma.Semestre.Value, rowDocente2.Num_func.Value,
                        dataInicioAlocacao, dadosTurma.Dt_fim.Value, false);

                    #region Se o docente não for regente na segunda matrícula, e sua função não permitir GLP, bloqueia alocação de GLP
                    if (rowFuncao2 != null && rowFuncao2.Campo_01 != "S" && rowFuncao2.Campo_03 != "S")
                        return MontarErrorListGLP(connection, dadosHoraAula,
                            "GLP não permitida! Docente não possui função na segunda matrícula que permita alocação de GLP.", dadosDocente);
                    #endregion

                    #region Obtém licenças ativas da segunda matrícula do docente
                    IEnumerable<Ly_licencas.Row> licencasAtivasSegundaMatricula = ObterLicencasAtivas(connection, rowDocente2.Num_func, dadosTurma.Dt_inicio, dadosTurma.Dt_fim);
                    #endregion

                    #region Verifica se 2ª matrícula do docente possui licença de CH Reduzida. Se sim, 50% da CH é considerada
                    if (licencasAtivasSegundaMatricula.Count(lic => lic.Motivo == "43") > 0)
                        chPermitida2 /= 2;
                    #endregion

                    #region Se não existe licença ativa (com licença diferente de 43) na segunda matrícula, valida CH Livre na segunda matrícula
                    if (licencasAtivasSegundaMatricula.Count(lic => lic.Motivo != "43") == 0)
                        if (chAulaDocente2 < chPermitida2)
                            return MontarErrorListGLP(connection, dadosHoraAula, "Esta aula não pode ser inclusa como GLP, pois este PROFESSOR possui Carga Horária livre na 2ª matrícula.", dadosDocente);
                    #endregion

                    #region Verifica se segunda matrícula do docente faz GLP em Atividades Integradas
                    if (RN.Funcao.VerificaFuncaoDOCII(connection, rowFuncao != null ? rowFuncao.Funcao : string.Empty))
                        matricula2_GLP_AI = VerificaDocentePossuiGLPEmAtividadesIntegradas(connection, num_func_2matricula);
                    #endregion
                }

                #endregion

                #region Verifica se solicitação GLP está liberada e, se sim, se existe GLP disponível

                Ly_docente_funcao_glp.Row rowSolicitacaoGLP = null;

                decimal? numFuncAlocacao = 0;

                if (qtEstadoOriginal.Rows.Count > 0)
                {

                    SimpleRow[] dadosLinhaOriginal = qtEstadoOriginal.Select(" DIA_SEMANA= " + dadosHoraAula.Dia_semana +
                                                                            " AND TURNO = '" + dadosHoraAula.Turno + "' " +
                                                                            " AND AULA = " + dadosHoraAula.Aula +
                                                                            " AND HORAINI_AULA = '" + String.Format("{0:1899-12-30 HH:mm:ss}", dadosHoraAula.Horaini_aula) + "' " +
                                                                            " AND HORAFIM_AULA = '" + String.Format("{0:1899-12-30 HH:mm:ss}", dadosHoraAula.Horafim_aula) + "' " +
                                                                            " AND ANO = " + dadosHoraAula.Ano +
                                                                            " AND SEMESTRE = " + dadosHoraAula.Semestre +
                                                                            " AND FACULDADE = '" + dadosHoraAula.Faculdade + "'");

                    if (dadosLinhaOriginal.Count() > 0)
                    {
                        numFuncAlocacao = Convert.ToDecimal(dadosLinhaOriginal[0]["num_func"]);
                    }
                }
                if (numFuncAlocacao > 0)
                {
                    foreach (Ly_grupo_habilitacao.Row agrupamento in agrupamentos)
                    {
                        Ly_docente_funcao_glp.Row[] solicitacoesGLPAgrupamento = ObterSolicitacoesGLPDisponiveis(connection, matricula, dadosTurma.Unidade_responsavel, agrupamento.Agrupamento, dadosTurma.Ano, dadosTurma.Semestre, dadosTurma.Turma, dadosHoraAula.Disciplina, numFuncAlocacao);
                        if (solicitacoesGLPAgrupamento != null && solicitacoesGLPAgrupamento.Length > 0)
                        {
                            rowSolicitacaoGLP = solicitacoesGLPAgrupamento[0];
                            break;
                        }
                    }
                }

                if (rowSolicitacaoGLP == null)
                    return MontarErrorListGLP(connection, dadosHoraAula, "Não existe pedido de GLP disponível para esta UA, Disciplina e Matrícula.", dadosDocente);

                #endregion

                #region Verifica se a função permite GLP e obtém carga horária da função

                decimal chPermitidaFuncaoGLP = 0M;
                string funcaoEmGLP = rowSolicitacaoGLP.Funcao_glp;
                int ch_glpId = rowSolicitacaoGLP.Ch_glpid.Value;

                Ly_funcao.Row rowFuncaoEmGLP = Ly_funcao.Row.Query(connection, funcaoEmGLP);

                if (rowDisciplina.Horas_ativ.HasValue && rowDisciplina.Horas_ativ.Value > 0M && RN.Funcao.VerificaFuncaoDOCII(connection, funcaoEmGLP))
                    chPermitidaFuncaoGLP = 10M; // Definição: se for DOC II e disciplina for atividade complementar, limite GLP é 10
                else
                {
                    int chTemp = RecursosHumanos.ChGlp.ObtemCH_GLP(connection, ch_glpId);

                    chPermitidaFuncaoGLP = chTemp;
                }

                if (chPermitidaFuncaoGLP == 0)
                    return MontarErrorListGLP(connection, dadosHoraAula, "GLP não permitida para o docente de acordo com a definição em Carga Horária dos Cargos.", dadosDocente);
                #endregion

                #region Verifica se a função em GLP é compatível com a turma
                if (!ValidaFuncaoDocenteTurma(connection, modalidadeCurso, dadosTurma.Serie.Value, rowFuncaoEmGLP.Funcao))
                    return MontarErrorListGLP(connection, dadosHoraAula, "Função em GLP do docente incompatível com a função da turma.", dadosDocente);
                #endregion

                #region Se função da lotação e função em GLP forem DOC II, valida se segunda matrícula faz GLP em Atividades Integradas
                if (matricula2_GLP_AI && RN.Funcao.VerificaFuncaoDOCII(connection, funcaoEmGLP))
                    return MontarErrorListGLP(connection, dadosHoraAula, "Esta aula não pode ser inclusa como GLP, pois este PROFESSOR possui GLP em Atividades Integradas na 2ª matrícula.", dadosDocente);
                #endregion

                #region Verifica se professor possui carga horária em GLP dentro do limite para a função
                int chUsadaGLP = 1 + ObterNumeroAulaDocentePeriodo(connection, dadosTurma.Ano.Value, dadosTurma.Semestre.Value, num_func, dataInicioAlocacao, dadosTurma.Dt_fim.Value, true);
                if (chUsadaGLP > chPermitidaFuncaoGLP)
                    return MontarErrorListGLP(connection, dadosHoraAula, "Carga horária semanal GLP da matrícula do docente ultrapassa carga horária GLP permitida.", dadosDocente);
                #endregion

                #region Verifica se disciplina é Atividade Complementar se função for "DOC II": o docente será obrigado a fazer todas as suas GLPs em disciplinas normais ou todas as suas GLPs em disciplinas de atividades complementares
                if (RN.Funcao.VerificaFuncaoDOCII(connection, rowFuncao.Funcao))
                {       //COMENTADO EM 06/04 POR THAIS
                    //(SELECT CASE d.horas_ativ WHEN 0 THEN 'N' WHEN NULL THEN 'N' ELSE 'S' END) atividade_complementar
                    QueryTable qtDisciplinasGLP = new QueryTable(@"
                        SELECT DISTINCT
	                        adt.disciplina,
	                        d.nome_compl,
	                       (SELECT CASE  WHEN d.horas_ativ = 0 THEN 'N' WHEN D.HORAS_ATIV IS NULL THEN 'N' ELSE 'S' END) atividade_complementar
                        FROM ly_aula_docente_tipo adt
                        INNER JOIN ly_disciplina d (NOLOCK) ON d.disciplina = adt.disciplina
                        INNER JOIN ly_turma t (NOLOCK) ON
                            adt.disciplina = t.disciplina AND
                            adt.turma = t.turma AND
                            adt.ano = t.ano AND
                            adt.semestre = t.semestre AND
                            adt.data_fim = t.dt_fim
                        WHERE
                            adt.NUM_FUNC = ? AND
                            adt.TURNO = ? AND
                            adt.FACULDADE = ? AND
                            adt.TURMA = ? AND
                            adt.ANO = ? AND
                            adt.SEMESTRE = ? AND
                            adt.TIPO_AULA = 'GLP' AND
                            adt.DATA_INICIO <= ? AND 
                            adt.DATA_FIM >= ? AND
                            t.sit_turma = 'Aberta'");
                    qtDisciplinasGLP.Query(connection, num_func, dadosTurma.Turno, dadosTurma.Faculdade, dadosTurma.Turma,
                        dadosTurma.Ano, dadosTurma.Semestre, dadosTurma.Dt_fim, dadosTurma.Dt_inicio);
                    var disciplinasAtivCompl = qtDisciplinasGLP.Rows
                        .Cast<SimpleRow>()
                        .Select(q => new
                        {
                            Disciplina = q["disciplina"],
                            Nome = q["nome_compl"],
                            AtividadeComplementar = q["atividade_complementar"] == "S"
                        })
                        .GroupBy(q => q.AtividadeComplementar)
                        .Select(q => new { AtividadeComplementar = q.Key, Contagem = q.Count(a => a.AtividadeComplementar == q.Key) })
                        .ToDictionary(q => q.AtividadeComplementar, q => q.Contagem);

                    bool ativComplDisciplinaAtual = rowDisciplina.Horas_ativ.HasValue ? (rowDisciplina.Horas_ativ.Value > 0) : false;
                    if (ativComplDisciplinaAtual && disciplinasAtivCompl[!ativComplDisciplinaAtual] > 0)
                        return MontarErrorListGLP(connection, dadosHoraAula, "O docente deve fazer todas as suas GLPs em disciplina normais ou todas as suas GLPs em disciplinas de atividades complementares.", dadosDocente);
                    if (disciplinasAtivCompl.Keys.Count > 1 && disciplinasAtivCompl[true] > 0 && disciplinasAtivCompl[false] > 0)
                        return MontarErrorListGLP(connection, dadosHoraAula, "O docente deve fazer todas as suas GLPs em disciplina normais ou todas as suas GLPs em disciplinas de atividades complementares.", dadosDocente);
                }
                #endregion

                #region Verifica se a carga horária total do docente (normal + GLP) ultrapassa 65 tempos
                RN.AulaDocenteTipo rnAulaDocenteTipo = new AulaDocenteTipo();

                int chTotalUsadaGLP = 1 + rnAulaDocenteTipo.ObtemQuantidadeGlpsAtivasPor(dadosDocente.Pessoa, dadosTurma.Ano.Value);

                if (chTotalUsadaGLP > chPermitidaFuncaoGLP)
                    return MontarErrorListGLP(connection, dadosHoraAula, string.Format("O somatório da carga horária de GLP do docente ultrapassa {0} horas.", chPermitidaFuncaoGLP), dadosDocente);
                #endregion

                //somente será verificado caso seja uma alteração
                //onde será passado no parametro a consulta com os dados originais do quadro de horário                
                SimpleRow dadosDocenteGLP = VerificaExisteDocenteGLP(qtEstadoOriginal, dadosHoraAula, num_func);
                if (dadosDocenteGLP != null) //GLP do docente já está presente, então nada faz e apenas informa no quadro de horário
                {
                    //preenche o datatable de tipo GLP com os dados atuais do docente
                    PreencherAulaDocenteTipo(dadosHoraAula, dtAulaDocenteTipo, num_func, dadosHoraAula.Aula.Value, dadosHoraAula.Dia_semana.Value,
                        dadosTurma.Dt_inicio.Value, dadosTurma.Dt_fim.Value, rowSolicitacaoGLP.Id_docente_funcao_glp.Value);
                    //remove do estado original a linha atualizada
                    qtEstadoOriginal.Rows.Remove(dadosDocenteGLP);
                    return MontarInformaGLP(connection, dadosHoraAula, string.Empty, dadosDocente);
                }
                else
                {
                    //filtra o horário analisado obtendo os dados originais 
                    SimpleRow[] dadosLinhaOriginal = qtEstadoOriginal.Select(" DIA_SEMANA= " + dadosHoraAula.Dia_semana +
                                                                             " AND TURNO = '" + dadosHoraAula.Turno + "' " +
                                                                             " AND AULA = " + dadosHoraAula.Aula +
                                                                             " AND HORAINI_AULA = '" + String.Format("{0:1899-12-30 HH:mm:ss}", dadosHoraAula.Horaini_aula) + "' " +
                                                                             " AND HORAFIM_AULA = '" + String.Format("{0:1899-12-30 HH:mm:ss}", dadosHoraAula.Horafim_aula) + "' " +
                                                                             " AND ANO = " + dadosHoraAula.Ano +
                                                                             " AND SEMESTRE = " + dadosHoraAula.Semestre +
                                                                             " AND FACULDADE = '" + dadosHoraAula.Faculdade + "'");

                    //não retornou registro então deverá retornar erro pois não existe nenhum cadastro anterior neste horário
                    if (dadosLinhaOriginal.Length == 0)
                        return MontarErrorListGLP(connection, dadosHoraAula, "GLP somente pode ser alocada em horário onde ocorrer o código 00000000 ou 99999999.", dadosDocente);

                    //caso exista algum valor no horário/dia da semana/aula será verificado o valor do registro que foi retornado
                    //caso seja igual a 00000000 ou 99999999 o docente poderá fazer GLP
                    String matriculaAnterior = dadosLinhaOriginal[0]["MATRICULA"].ToString();
                    if (matriculaAnterior != "00000000" && matriculaAnterior != "99999999")
                        return MontarErrorListGLP(connection, dadosHoraAula, "GLP somente pode ser alocada em horário onde ocorrer o código 00000000 ou 99999999.", dadosDocente);

                    string curso = Convert.ToString(dadosLinhaOriginal[0]["CURSO"]);
                    string turno = Convert.ToString(dadosLinhaOriginal[0]["TURNO"]);
                    string curriculo = Convert.ToString(dadosLinhaOriginal[0]["CURRICULO"]);
                    string disciplina = Convert.ToString(dadosLinhaOriginal[0]["DISCIPLINA"]);

                    //preenche o datatable de tipo GLP com os dados atuais do docente
                    PreencherAulaDocenteTipo(dadosHoraAula, dtAulaDocenteTipo, num_func, dadosHoraAula.Aula.Value, dadosHoraAula.Dia_semana.Value, dadosTurma.Dt_inicio.Value, dadosTurma.Dt_fim.Value, rowSolicitacaoGLP.Id_docente_funcao_glp.Value);
                    //remove do estado original a linha atualizada
                    qtEstadoOriginal.Rows.Remove(dadosLinhaOriginal[0]);
                    return MontarInformaGLP(connection, dadosHoraAula, string.Empty, dadosDocente);
                }
                #endregion
            }
            //Pode ser alocada como aula normal
            else
            {
                if (!permiteAlocacaoNormal)
                    return MontarErrorListGLP(connection, dadosHoraAula, "O professor não está habilitado nesta disciplina para atuar na matrícula.", dadosDocente);

                if (!ValidaFuncaoDocenteTurma(connection, modalidadeCurso, dadosTurma.Serie.Value, rowFuncao != null ? rowFuncao.Funcao : string.Empty))
                    return MontarErrorListGLP(connection, dadosHoraAula, "Função do docente incompatível com a função da turma.", dadosDocente);
            }

            return null;
        }

        private static SimpleRow VerificaExisteDocenteGLP(QueryTable qtEstadoOriginal, Ly_hor_aula.Row dadosHoraAula, decimal num_func)
        {
            SimpleRow[] dadosLinhaOriginal = qtEstadoOriginal.Select(" NUM_FUNC = " + num_func +
                                                                     " AND DIA_SEMANA= " + dadosHoraAula.Dia_semana +
                                                                     " AND TURNO = '" + RN.RNBase.MudarAspas(dadosHoraAula.Turno) + "' " +
                                                                     " AND AULA = " + dadosHoraAula.Aula +
                                                                     " AND HORAINI_AULA = '" + RN.RNBase.MudarAspas(String.Format("{0:1899-12-30 HH:mm:ss}", dadosHoraAula.Horaini_aula)) + "' " +
                                                                     " AND HORAFIM_AULA = '" + RN.RNBase.MudarAspas(String.Format("{0:1899-12-30 HH:mm:ss}", dadosHoraAula.Horafim_aula)) + "' " +
                                                                     " AND ANO = " + dadosHoraAula.Ano +
                                                                     " AND SEMESTRE = " + dadosHoraAula.Semestre +
                                                                     " AND FACULDADE = '" + RN.RNBase.MudarAspas(dadosHoraAula.Faculdade) + "'" +
                                                                     " AND TIPO_AULA = 'GLP' ");
            if (dadosLinhaOriginal.Length > 0)
                return dadosLinhaOriginal[0];
            else
                return null;
        }

        private static void PreencherAulaDocenteTipo(Ly_hor_aula.Row drHorAula, Ly_aula_docente_tipo dtAulaDocenteTipo, decimal num_func, decimal aula, decimal dia_semana, DateTime dtIni, DateTime dtFim, decimal id_docente_funcao_glp)
        {
            Ly_aula_docente_tipo.Row linhaTipo = dtAulaDocenteTipo.NewRow();
            linhaTipo.Turno = drHorAula.Turno;
            linhaTipo.Faculdade = drHorAula.Faculdade;
            linhaTipo.Dia_semana = drHorAula.Dia_semana;
            linhaTipo.Aula = drHorAula.Aula;
            linhaTipo.Disciplina = drHorAula.Disciplina;
            linhaTipo.Ano = drHorAula.Ano;
            linhaTipo.Semestre = drHorAula.Semestre;
            linhaTipo.Turma = drHorAula.Turma;
            linhaTipo.Num_func = num_func;
            linhaTipo.Data_inicio = dtIni;
            linhaTipo.Data_fim = dtFim;
            linhaTipo.Tipo_aula = "GLP";
            linhaTipo.Id_docente_funcao_glp = id_docente_funcao_glp;
            dtAulaDocenteTipo.Rows.Add(linhaTipo);
        }

        public static string ObterReadaptado(TConnection connection, string matricula)
        {
            if (matricula.IsNullOrEmptyOrWhiteSpace())
            {
                return "N";
            }
            else
            {
                return RN.DocenteGLP.IsReadaptado(connection, matricula) ? "S" : "N";
            }
        }

        public static QueryTable ConsultarTurmaPorGradeSerie(decimal gradeId)
        {
            string sql = @" 
                SELECT DISTINCT
                    t.turma, t.ano, t.semestre, t.curso, t.turno, t.unidade_responsavel, t.serie, t.curriculo, t.faculdade, t.turma_integracao sufixo,t.tipo_gestao,t.ambiente_externo,ISNULL(t.eletiva,'N') AS eletiva,t.turmareferencia, t.dependencia
                FROM ly_turma t (NOLOCK) INNER JOIN ly_grade_serie gs (NOLOCK) ON
                    t.turma = gs.grade
                    AND t.curso = gs.curso
                    AND t.turno = gs.turno
                    AND t.curriculo = gs.curriculo
                    AND t.serie = gs.serie
                    AND t.ano = gs.ano
                    AND t.semestre = gs.semestre
                WHERE
                    gs.grade_id = ? ";

            TConnection connection = Config.CreateConnection();

            connection.Open();

            try
            {
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, gradeId);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static DataTable ConsultarTurmaPorGradeSerieNota(decimal gradeId)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT DISTINCT
                                t.turma, t.ano, t.semestre, t.curso, t.turno, t.unidade_responsavel, t.serie, t.curriculo, t.faculdade, t.turma_integracao sufixo,t.tipo_gestao 
                            FROM ly_turma t (NOLOCK) INNER JOIN ly_grade_serie gs (NOLOCK) ON
                                t.turma = gs.grade
                                AND t.curso = gs.curso
                                AND t.turno = gs.turno
                                AND t.curriculo = gs.curriculo
                                AND t.serie = gs.serie
                                AND t.ano = gs.ano
                                AND t.semestre = gs.semestre
                            WHERE
                                gs.grade_id = @GRADE_ID "
                };

                contextQuery.Parameters.Add("@GRADE_ID", gradeId);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static decimal? ObterCargaHorariaNormalSemanal(TConnection connection, string categoriaMatricula, string funcaoMatricula)
        {
            if (new String[] { categoriaMatricula, funcaoMatricula }.Count(str => String.IsNullOrEmpty(str)) > 0)
                return (decimal?)null;

            string sql = @"    SELECT CH.CARGAHORARIAREGENCIA 
                                        FROM   RECURSOSHUMANOS.CH_AGRUPAMENTOCARGO CH (NOLOCK) 
											   INNER JOIN LY_CATEGORIA_DOCENTE CD (NOLOCK) ON CH.AGRUPAMENTOCARGOSID = CD.AGRUPAMENTOCARGOSID
                                        WHERE  CD.CATEGORIA = ?
                                               AND CH.FUNCAO = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, categoriaMatricula, funcaoMatricula);
            if (!valorConsulta.IsNull)
                return (decimal)valorConsulta;
            else
                return (decimal?)null;
        }

        public static decimal ObterCodigoPessoa(TConnection connection, decimal num_func)
        {
            DbObject valorConsulta = TCommand.ExecuteScalar(connection, "select TOP 1 PESSOA from ly_docente (NOLOCK) WHERE num_func = ?", num_func); ;
            if (!valorConsulta.IsNull)
                return (decimal)valorConsulta;
            else
                return 0M;
        }

        private static bool ValidaFuncaoDocenteTurma(TConnection connection, String modalidadeCurso, decimal serie, String funcaoDocente)
        {
            FuncaoDOC funcTurma = FuncaoDOC.NENHUM;
            FuncaoDOC funcDoc = FuncaoDOC.NENHUM;

            if (modalidadeCurso.ToUpper().Trim().Equals("ENSINO FUNDAMENTAL") || modalidadeCurso.ToUpper().Trim().Equals("EF"))
            {
                if (serie >= 1 && serie <= 5)
                    funcTurma = FuncaoDOC.DOCII;
                else if (serie >= 6 && serie <= 9)
                    funcTurma = FuncaoDOC.DOCI;
            }
            else if (modalidadeCurso.ToUpper().Trim().Equals("ENSINO MÉDIO") || modalidadeCurso.ToUpper().Trim().Equals("EM"))
                funcTurma = FuncaoDOC.DOCI;
            else if (modalidadeCurso.ToUpper().Trim().Equals("ENSINO MÉDIO E ENSINO FUNDAMENTAL"))
                funcTurma = FuncaoDOC.DOCI_DOCII;

            bool docI, docII;
            docI = RN.Funcao.VerificaFuncaoDOCI(connection, funcaoDocente);
            docII = RN.Funcao.VerificaFuncaoDOCII(connection, funcaoDocente);

            if (docI && docII) funcDoc = FuncaoDOC.DOCI_DOCII;
            else if (docI) funcDoc = FuncaoDOC.DOCI;
            else if (docII) funcDoc = FuncaoDOC.DOCII;

            switch (funcTurma)
            {
                case FuncaoDOC.DOCI_DOCII:
                    return funcDoc == FuncaoDOC.DOCI || funcDoc == FuncaoDOC.DOCII || funcDoc == FuncaoDOC.DOCI_DOCII;
                case FuncaoDOC.DOCI:
                    return funcDoc == FuncaoDOC.DOCI || funcDoc == FuncaoDOC.DOCI_DOCII;
                case FuncaoDOC.DOCII:
                    return funcDoc == FuncaoDOC.DOCII || funcDoc == FuncaoDOC.DOCI_DOCII;
                default:
                    return false;
            }
        }

        public static int ObterNumeroMatriculaDocente(TConnection connection, decimal pessoa)
        {
            DbObject valor = TCommand.ExecuteScalar(connection,
                @"SELECT 
                    COUNT(d.matricula)
                 FROM 
                    ly_docente d (NOLOCK) INNER JOIN 
                    ly_lotacao l (NOLOCK) on d.PESSOA = l.pessoa and d.matricula = l.matricula
                 WHERE 
                    d.pessoa = ? and 
                    convert(date,l.DATA_NOMEACAO) <= convert(date,GetDate()) AND (l.DATA_DESATIVACAO is null or CONVERT(date, l.DATA_DESATIVACAO) > CONVERT(date,getdate()))", pessoa);
            return valor.IsNull ? 0 : (int)valor;
        }

        /// <summary>
        /// Verifica se existe conflito de horário com a turma
        /// </summary>
        /// <param name="disciplina">Código da disciplina</param>
        /// <param name="diaSemana">Dia da semana</param>
        /// <param name="turma">Código da turma</param>
        /// <param name="ano">Ano</param>
        /// <param name="semestre">Semestre</param>
        /// <param name="horaIni">Horário inicial</param>
        /// <param name="horaFim">Horário final</param>
        /// <returns>Querytable com os dados obtidos, caso retorne vazio não existe conflito</returns>
        private static QueryTable VerificarConflitoHorarioTurma(TConnection connection, string disciplina, decimal diaSemana, string turma, decimal ano, decimal semestre, string turno, DateTime horaIni, DateTime horaFim, DateTime dtFimTurma)
        {
            //SELECT  ha.aula, 
            //            ha.dia_semana,                         
            //            ha.disciplina, 
            //            ha.ano, 
            //            ha.semestre, 
            //            ha.turma,
            //            ha.turno
            //    FROM    ly_hor_aula ha,
            //            ly_aula_docente ad
            //    WHERE   ha.disciplina = ?
            //            AND ha.dia_semana = ?
            //            AND ha.turma = ?
            //            AND ha.ano = ?
            //            AND ha.semestre = ?
            //            AND ha.turno = ?                    
            //            AND ad.turno = ha.turno
            //            AND ad.faculdade = ha.faculdade
            //            AND ad.dia_semana = ha.dia_semana
            //            AND ad.aula = ha.aula
            //            and ad.disciplina = ha.disciplina
            //            AND ad.turma = ha.turma
            //            AND ad.ano = ha.ano
            //            AND ad.semestre = ha.semestre                        
            //            AND ad.data_fim = ?
            //            AND ( (ha.horaini_aula  <= ? and ha.horafim_aula >= ?)
            //              or (ha.horaini_aula >= ? and ha.horafim_aula <= ?)
            //              or (ha.horafim_aula > ? and ha.horafim_aula <= ?)
            //              or (ha.horaini_aula >= ? and ha.horaini_aula < ?) )
            //RETIRADO EM 07/04 AFIM DE RESOLVER O PROBLEMA DE PERMISSAO DE CONFLITO DE HORARIO
            string sql = @" SELECT  ha.aula, 
                        ha.dia_semana,                         
                        ha.disciplina, 
                        ha.ano, 
                        ha.semestre, 
                        ha.turma,
                        ha.turno
                FROM    ly_hor_aula ha
                inner join ly_aula_docente ad on ad.turno = ha.turno
								AND ad.faculdade = ha.faculdade
								AND ad.dia_semana = ha.dia_semana
								AND ad.aula = ha.aula
								and ad.disciplina = ha.disciplina
								AND ad.turma = ha.turma
								AND ad.ano = ha.ano
								AND ad.semestre = ha.semestre    
                WHERE   ha.disciplina = ?
                        AND ha.dia_semana = ?
                        AND ha.turma = ?
                        AND ha.ano = ?
                        AND ha.turno = ?                    
                        AND ad.data_fim = ?
                        AND ( (ha.horaini_aula  <= ? and ha.horafim_aula >= ?)
                          or (ha.horaini_aula >= ? and ha.horafim_aula <= ?)
                          or (ha.horafim_aula > ? and ha.horafim_aula <= ?)
                          or (ha.horaini_aula >= ? and ha.horaini_aula < ?) )
                ";

            var strHoraIni = new DateTime(1899, 12, 30, horaIni.Hour, horaIni.Minute, horaIni.Second);
            var strHoraFim = new DateTime(1899, 12, 30, horaFim.Hour, horaFim.Minute, horaFim.Second);
            //, semestr
            QueryTable qt = new QueryTable(sql);
            qt.Query(connection, disciplina, diaSemana, turma, ano, turno, dtFimTurma,
                strHoraIni, strHoraFim, strHoraIni, strHoraFim, strHoraIni, strHoraFim, strHoraIni, strHoraFim);
            return qt;
        }

        /// <summary>
        /// Obtém o "merge" do quadro de horário com os novos dados inseridos/excluídos/alterados pelo usuário
        /// </summary>
        /// <param name="dtHorAulaOriginal">Quadro de horário original</param>
        /// <param name="dtHorAulaAtual">Quadro de horário atual</param>
        /// <returns>Quadro de horário com os dados atualizados</returns>
        private static Ly_hor_aula ObterQuadroHorarioAlterado(QueryTable qtHorAulaOriginal, Ly_hor_aula dtHorAulaAtual)
        {
            Ly_hor_aula dtHorAulaNovo = new Ly_hor_aula();
            //Ly_hor_aula dtHorAulaAux = new Ly_hor_aula();

            //verifica se existe dados para o quadro de horario original e atual
            if (qtHorAulaOriginal != null && dtHorAulaAtual != null)
            {
                if (qtHorAulaOriginal.Rows != null && dtHorAulaAtual.Rows != null)
                {
                    //loop nas linhas do quadro de horário original
                    foreach (SimpleRow linhaOriginal in qtHorAulaOriginal.Rows)
                    {
                        //se linhaOriginal existe em dtHorAulaAtual
                        Ly_hor_aula.Row[] dadosLinha = dtHorAulaAtual.Select(" AULA =" + Convert.ToDecimal(linhaOriginal["AULA"]) +
                                                                             " AND DIA_SEMANA = " + Convert.ToDecimal(linhaOriginal["DIA_SEMANA"]) +
                                                                             " AND DISCIPLINA = '" + RN.RNBase.MudarAspas(Convert.ToString(linhaOriginal["DISCIPLINA"])) + "'");
                        if (dadosLinha != null)
                        {
                            if (dadosLinha.Length == 0)
                            {
                                string turno = Convert.ToString(linhaOriginal["TURNO"]);
                                string faculdade = Convert.ToString(linhaOriginal["FACULDADE"]);
                                decimal dia_semana = Convert.ToDecimal(linhaOriginal["DIA_SEMANA"]);
                                decimal aula = Convert.ToDecimal(linhaOriginal["AULA"]);
                                string disciplina = Convert.ToString(linhaOriginal["DISCIPLINA"]);
                                decimal ano = Convert.ToDecimal(linhaOriginal["ANO"]);
                                decimal semestre = Convert.ToDecimal(linhaOriginal["SEMESTRE"]);
                                DateTime horafim_aula = Convert.ToDateTime(linhaOriginal["HORAFIM_AULA"]);
                                DateTime horaini_aula = Convert.ToDateTime(linhaOriginal["HORAINI_AULA"]);
                                string turma = Convert.ToString(linhaOriginal["TURMA"]);

                                if (dtHorAulaNovo.Rows.Find(turno, faculdade, dia_semana, aula, disciplina, turma, ano, semestre) == null)
                                {
                                    Ly_hor_aula.Row linha = dtHorAulaNovo.NewRow();

                                    linha.Turno = turno;
                                    linha.Faculdade = faculdade;
                                    linha.Dia_semana = dia_semana;
                                    linha.Aula = aula;
                                    linha.Disciplina = disciplina;
                                    linha.Ano = ano;
                                    linha.Semestre = semestre;
                                    linha.Horafim_aula = horafim_aula;
                                    linha.Horaini_aula = horaini_aula;
                                    linha.Turma = turma;

                                    dtHorAulaNovo.Rows.Add(linha);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(dadosLinha[0].Disciplina))
                                {
                                    Ly_hor_aula.Row linha = dtHorAulaNovo.NewRow();

                                    PopularLinhaHorAula(dadosLinha[0], linha);

                                    dtHorAulaNovo.Rows.Add(linha);

                                    dtHorAulaAtual.Rows.Remove(dadosLinha[0]);
                                }
                            }
                        }
                        else
                        {
                            Ly_hor_aula.Row linha = dtHorAulaNovo.NewRow();

                            linha.Turno = Convert.ToString(linhaOriginal["TURNO"]);
                            linha.Faculdade = Convert.ToString(linhaOriginal["FACULDADE"]);
                            linha.Dia_semana = Convert.ToDecimal(linhaOriginal["DIA_SEMANA"]);
                            linha.Aula = Convert.ToDecimal(linhaOriginal["AULA"]);
                            linha.Disciplina = Convert.ToString(linhaOriginal["DISCIPLINA"]);
                            linha.Ano = Convert.ToDecimal(linhaOriginal["ANO"]);
                            linha.Semestre = Convert.ToDecimal(linhaOriginal["SEMESTRE"]);
                            linha.Horafim_aula = Convert.ToDateTime(linhaOriginal["HORAFIM_AULA"]);
                            linha.Horaini_aula = Convert.ToDateTime(linhaOriginal["HORAINI_AULA"]);

                            dtHorAulaNovo.Rows.Add(linha);
                        }
                    }

                    foreach (Ly_hor_aula.Row linhaAtual in dtHorAulaAtual.Rows)
                    {
                        if (!string.IsNullOrEmpty(linhaAtual.Disciplina))
                        {
                            Ly_hor_aula.Row linha = dtHorAulaNovo.NewRow();

                            PopularLinhaHorAula(linhaAtual, linha);

                            dtHorAulaNovo.Rows.Add(linha);
                        }
                    }
                }
            }
            return dtHorAulaNovo;
        }

        private static void PopularLinhaHorAula(Ly_hor_aula.Row linhaDados, Ly_hor_aula.Row linhaDestino)
        {
            linhaDestino.Ano = linhaDados.Ano;
            linhaDestino.Aula = linhaDados.Aula;
            linhaDestino.Dependencia = linhaDados.Dependencia;
            linhaDestino.Dia_semana = linhaDados.Dia_semana;
            linhaDestino.Disciplina = linhaDados.Disciplina;
            linhaDestino.Faculdade = linhaDados.Faculdade;
            linhaDestino.Frequencia = linhaDados.Frequencia;
            linhaDestino.Horafim_aula = linhaDados.Horafim_aula;
            linhaDestino.Horaini_aula = linhaDados.Horaini_aula;
            linhaDestino.Qtde_aula = linhaDados.Qtde_aula;
            linhaDestino.Semestre = linhaDados.Semestre;
            linhaDestino.Stamp_atualizacao = linhaDados.Stamp_atualizacao;
            linhaDestino.Tipo = linhaDados.Tipo;
            linhaDestino.Turma = linhaDados.Turma;
            linhaDestino.Turno = linhaDados.Turno;
        }

        #region Métodos Clonagem

        private static Ly_hor_aula Clonar(Ly_hor_aula dtHorAulaDados)
        {
            Ly_hor_aula dtHorAulaDestino = new Ly_hor_aula();
            foreach (Ly_hor_aula.Row linhaOriginal in dtHorAulaDados.Rows)
            {
                Ly_hor_aula.Row linha = dtHorAulaDestino.NewRow();

                linha.Turno = linhaOriginal.Turno;
                linha.Faculdade = linhaOriginal.Faculdade;
                linha.Dia_semana = linhaOriginal.Dia_semana;
                linha.Aula = linhaOriginal.Aula;
                linha.Disciplina = linhaOriginal.Disciplina;
                linha.Ano = linhaOriginal.Ano;
                linha.Semestre = linhaOriginal.Semestre;
                linha.Horafim_aula = linhaOriginal.Horafim_aula;
                linha.Horaini_aula = linhaOriginal.Horaini_aula;
                linha.Turma = linhaOriginal.Turma;

                dtHorAulaDestino.Rows.Add(linha);
            }
            return dtHorAulaDestino;
        }

        private static Ly_aula_docente Clonar(Ly_aula_docente dtAulaDados)
        {
            Ly_aula_docente dtAulaDestino = new Ly_aula_docente();

            foreach (Ly_aula_docente.Row linhaOriginal in dtAulaDados.Rows)
            {
                Ly_aula_docente.Row linha = dtAulaDestino.NewRow();

                linha.Turno = linhaOriginal.Turno;
                linha.Faculdade = linhaOriginal.Faculdade;
                linha.Dia_semana = linhaOriginal.Dia_semana;
                linha.Aula = linhaOriginal.Aula;
                linha.Disciplina = linhaOriginal.Disciplina;
                linha.Ano = linhaOriginal.Ano;
                linha.Semestre = linhaOriginal.Semestre;
                linha.Turma = linhaOriginal.Turma;
                linha.Num_func = linhaOriginal.Num_func;
                linha.Data_inicio = linhaOriginal.Data_inicio;
                linha.Data_fim = linhaOriginal.Data_fim;

                dtAulaDestino.Rows.Add(linha);
            }
            return dtAulaDestino;
        }

        #endregion

        private static Ly_aula_docente ObterAulaDocenteAtual(Ly_aula_docente dtAulaDocenteOriginal, Ly_aula_docente dtAulaDocenteAtual)
        {
            Ly_aula_docente dtAulaDocenteNovo = new Ly_aula_docente();
            Ly_aula_docente dtAulaDocenteAtualAux = Clonar(dtAulaDocenteAtual);

            if (dtAulaDocenteOriginal != null && dtAulaDocenteAtual != null)
            {
                if (dtAulaDocenteOriginal.Rows != null && dtAulaDocenteAtual.Rows != null)
                {
                    foreach (Ly_aula_docente.Row linhaOriginal in dtAulaDocenteOriginal.Rows)
                    {
                        //se linhaOriginal existe em dtHorAulaAtual
                        Ly_aula_docente.Row[] dadosLinha = dtAulaDocenteAtual.Select(" AULA =" + linhaOriginal.Aula +
                                                                                     " AND DIA_SEMANA = " + linhaOriginal.Dia_semana);
                        if (dadosLinha != null)
                        {
                            if (dadosLinha.Length == 0)
                            {
                                Ly_aula_docente.Row linha = dtAulaDocenteNovo.NewRow();
                                PopularLinhaAulaDocente(linhaOriginal, linha);
                                if (VerificaAulaDocente(dtAulaDocenteNovo, linha) == null)
                                {
                                    dtAulaDocenteNovo.Rows.Add(linha);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(dadosLinha[0].Disciplina))
                                {
                                    Ly_aula_docente.Row linha = dtAulaDocenteNovo.NewRow();

                                    PopularLinhaAulaDocente(dadosLinha[0], linha);
                                    dtAulaDocenteNovo.Rows.Add(linha);

                                    dtAulaDocenteAtual.Rows.Remove(dadosLinha[0]);
                                }
                            }
                        }
                        else
                        {
                            Ly_aula_docente.Row linha = dtAulaDocenteNovo.NewRow();

                            PopularLinhaAulaDocente(linhaOriginal, linha);
                            dtAulaDocenteNovo.Rows.Add(linha);
                        }
                    }

                    foreach (Ly_aula_docente.Row linhaAtual in dtAulaDocenteAtual.Rows)
                    {
                        if (!string.IsNullOrEmpty(linhaAtual.Disciplina))
                        {
                            Ly_aula_docente.Row linha = dtAulaDocenteNovo.NewRow();

                            PopularLinhaAulaDocente(linhaAtual, linha);
                            dtAulaDocenteNovo.Rows.Add(linha);
                        }
                    }
                }
            }

            return dtAulaDocenteNovo;
        }

        private static Ly_aula_docente.Row VerificaAulaDocente(Ly_aula_docente dtAulaDocenteAux, Ly_aula_docente.Row linha)
        {
            return dtAulaDocenteAux.Rows.Find(linha.Num_func, linha.Turno, linha.Faculdade, linha.Dia_semana, linha.Aula, linha.Disciplina, linha.Turma, linha.Ano, linha.Semestre, linha.Data_inicio);
        }

        private static void PopularLinhaAulaDocente(Ly_aula_docente.Row linhaDados, Ly_aula_docente.Row linhaDestino)
        {
            linhaDestino.Turno = linhaDados.Turno;
            linhaDestino.Faculdade = linhaDados.Faculdade;
            linhaDestino.Dia_semana = linhaDados.Dia_semana;
            linhaDestino.Aula = linhaDados.Aula;
            linhaDestino.Disciplina = linhaDados.Disciplina;
            linhaDestino.Ano = linhaDados.Ano;
            linhaDestino.Semestre = linhaDados.Semestre;
            linhaDestino.Turma = linhaDados.Turma;
            linhaDestino.Num_func = linhaDados.Num_func;
            linhaDestino.Data_inicio = linhaDados.Data_inicio;
            linhaDestino.Data_fim = linhaDados.Data_fim;
        }

        private static DataSet ValidaInclusaoDeTurmaQuadroDeHorario(Ly_turma turma)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            DataSet ds = new DataSet();

            try
            {
                contextQuery.Command = (@"
										--VerificaAutorizacaoUnidadeEnsino
										SELECT   TOP 1 1
										FROM     
											ly_unidade_ensino_cursos (NOLOCK)
										WHERE    
											unidade_ens = @UNIDADE_RESPONSAVEL 
											AND curso = @CURSO 
											AND turno = @TURNO

	        
										-- VERIFICA SERIE DISTINTA
										select 1 
										from ly_serie 
										where 
											curso = @CURSO 
											and turno = @TURNO 
											and curriculo = @CURRICULO 
											and serie = @SERIE
											and Convert(Date, dt_extincao) <= Convert(Date,getdate()) 
	

										-- SE @OPTATIVAREFORCO = 'N'
										-- VerificarConflitoDependencia
										if(@OPTATIVAREFORCO = 'N')
										begin
										 SELECT TOP 1
												1
										FROM    LY_GRADE_SERIE gs ( NOLOCK )
												INNER JOIN LY_TURMA t ( NOLOCK ) ON t.TURMA = gs.GRADE
																					AND t.ANO = gs.ANO
																					AND t.SEMESTRE = gs.SEMESTRE
										WHERE   gs.FACULDADE = @FACULDADE
																AND gs.DEPENDENCIA = @DEPENDENCIA
																AND ((gs.DT_INICIO <= @DATAINICIO and gs.DT_FIM >= @DATAFIM) 
																		or (gs.DT_INICIO <= @DATAINICIO and gs.DT_FIM > @DATAFIM)
																		or (gs.DT_INICIO >= @DATAINICIO and gs.DT_INICIO <= @DATAINICIO))
																AND gs.TURNO = @TURNO
																AND gs.GRADE <> @TURMA
												AND T.OPTATIVAREFORCO = 'N'
                                                AND ISNULL(T.ELETIVA,'N') = 'N'
												AND t.SIT_TURMA NOT IN ( 'Finalizada', 'Desativada' ) 	
										end
										   
										        ");

                contextQuery.Parameters.Add("@OPTATIVAREFORCO", turma.Rows[0].OptativaReforco);
                contextQuery.Parameters.Add("@UNIDADE_RESPONSAVEL", turma.Rows[0].Unidade_responsavel);
                contextQuery.Parameters.Add("@CURRICULO", turma.Rows[0].Curriculo);
                contextQuery.Parameters.Add("@CURSO", turma.Rows[0].Curso);
                contextQuery.Parameters.Add("@TURNO", turma.Rows[0].Turno);
                contextQuery.Parameters.Add("@FACULDADE", turma.Rows[0].Faculdade);
                contextQuery.Parameters.Add("@DEPENDENCIA", turma.Rows[0].Dependencia);
                contextQuery.Parameters.Add("@DATAINICIO", turma.Rows[0].Dt_inicio);
                contextQuery.Parameters.Add("@DATAFIM", turma.Rows[0].Dt_fim);
                contextQuery.Parameters.Add("@TURMA", turma.Rows[0].Turma);
                contextQuery.Parameters.Add("@SERIE", turma.Rows[0].Serie);


                ds = ctx.GetDataSet(contextQuery);
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

            return ds;
        }

        private static bool VerificaAutorizacaoUnidadeEnsino(TConnection connection, string unidadeEnsino, string curso, string turno)
        {
            string sql = @"SELECT   TOP 1 1
                           FROM     ly_unidade_ensino_cursos (NOLOCK)
                           WHERE    unidade_ens = ? AND curso = ? AND turno = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, unidadeEnsino, curso, turno);
            if (!valorConsulta.IsNull)
            {
                return true;
            }

            return false;
        }

        public static decimal ObterNumAlunoMatriculadoPrincipal(decimal ano, decimal periodo, string turma)
        {
            string sql = @" SELECT  COUNT(DISTINCT aluno)
                        FROM    dbo.LY_MATRICULA m
								inner join LY_TURMA t on m.TURMA = t.TURMA
													and m.SEMESTRE = t.SEMESTRE
													and m.ANO = t.ANO 
													and m.DISCIPLINA = t.DISCIPLINA
                        WHERE   m.turma = ?
                                AND m.ano = ?
                                AND m.SEMESTRE = ?
                                AND m.SIT_MATRICULA = 'Matriculado'
                                AND ( m.DEPENDENCIA <> 'S'
                                    OR m.DEPENDENCIA IS NULL )
                                AND ( t.ELETIVA <> 'S'
                                    OR t.ELETIVA IS NULL ) ";

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, turma, ano, periodo);
                if (!valorConsulta.IsNull)
                {
                    return (decimal)valorConsulta;
                }
                else
                {
                    return 0M;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public static decimal ObterNumAlunoMatriculadoProgessao(decimal ano, decimal periodo, string turma)
        {
            string sql = @" SELECT  COUNT(DISTINCT aluno)
                        FROM    dbo.LY_MATRICULA
                        WHERE   turma = ?
                                AND ano = ?
                                AND SEMESTRE = ?
                                AND SIT_MATRICULA = 'Matriculado'
                                AND DEPENDENCIA = 'S' ";

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, turma, ano, periodo);
                if (!valorConsulta.IsNull)
                {
                    return (decimal)valorConsulta;
                }
                else
                {
                    return 0M;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public static decimal ObterNumAlunoMatriculadoEletiva(decimal ano, decimal periodo, string turma)
        {
            string sql = @" SELECT  COUNT(DISTINCT aluno)
                        FROM    dbo.LY_MATRICULA m
								inner join LY_TURMA t on m.TURMA = t.TURMA
													and m.SEMESTRE = t.SEMESTRE
													and m.ANO = t.ANO 
													and m.DISCIPLINA = t.DISCIPLINA
                        WHERE   m.turma = ?
                                AND m.ano = ?
                                AND m.SEMESTRE = ?
                                AND m.SIT_MATRICULA = 'Matriculado'
                                AND t.ELETIVA = 'S' ";

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, turma, ano, periodo);
                if (!valorConsulta.IsNull)
                {
                    return (decimal)valorConsulta;
                }
                else
                {
                    return 0M;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public static int ObterNumAlunoMatriculadoEletiva(decimal ano, decimal periodo, string turma, int grupo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(DISTINCT aluno) AS QTDE
                        FROM    dbo.LY_MATRICULA m
	                            INNER JOIN LY_DISCIPLINA D ON M.DISCIPLINA = D.DISCIPLINA
								INNER JOIN LY_TURMA T ON M.TURMA = T.TURMA
													AND M.SEMESTRE = T.SEMESTRE
													AND M.ANO = T.ANO 
													AND M.DISCIPLINA = T.DISCIPLINA
                        WHERE   M.TURMA = @TURMA
                                AND M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE
                                AND M.SIT_MATRICULA = 'Matriculado'
                                AND T.ELETIVA = 'S'
                                AND D.GRUPO = @GRUPO ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@GRUPO", grupo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QTDE"]);
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

        public static RetValue VerificaPodeAlterarGrade(string curso, string turno, string curriculo, decimal serie)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                string sql = @"
                    SELECT 
                        COUNT(*) 
                    FROM 
                        ly_matgrade mat (NOLOCK)
                    INNER JOIN 
                        ly_grade_serie gs (NOLOCK) ON gs.grade_id = mat.grade_id
                    WHERE
                        gs.curso = ? AND 
                        gs.turno = ? AND 
                        gs.curriculo = ? AND 
                        gs.serie = ? AND 
                        mat.sit_matgrade NOT IN 
                            ('Cancelado', 
                            'Trancado', 
                            'Intercambio', 
                            'Transf.Internamente',
                            'Remanejado', 
                            'Transf.Externamente',
                            'Desistente', 
                            'Concluido', 
                            'Jubilado')";

                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, curso, turno, curriculo, serie);
                if (!valorConsulta.IsNull && ((decimal)valorConsulta) > 0)
                {
                    return new RetValue(false, "", new ErrorList("Não é possível alterar as disciplinas que compõem esta matriz curricular. Existem alunos matriculados para turmas desta matriz."));
                }

                sql = @"
                    SELECT
                        COUNT(*)
                    FROM
                        ly_aula_docente ad
                    INNER JOIN
                        ly_turma t (NOLOCK) ON
                            ad.disciplina = t.disciplina AND
                            ad.turma = t.turma AND
                            ad.ano = t.ano AND
                            ad.semestre = t.semestre AND
                            ad.data_fim = t.dt_fim
                    INNER JOIN
                        ly_grade_serie gs (NOLOCK) ON
                            gs.grade = ad.turma AND
                            gs.turno = ad.turno AND
                            ad.ano = gs.ano AND
                            ad.semestre = gs.semestre AND
                            gs.faculdade = ad.faculdade
                    WHERE
                        t.sit_turma = 'Aberta' AND
                        gs.curso = ? AND
                        gs.turno = ? AND
                        gs.curriculo = ? AND
                        gs.serie = ?";
                valorConsulta = TCommand.ExecuteScalar(connection, sql, curso, turno, curriculo, serie);
                if (!valorConsulta.IsNull && ((decimal)valorConsulta) > 0)
                {
                    return new RetValue(false, "", new ErrorList("Não é possível alterar as disciplinas que compõem esta matriz curricular. Existem aulas alocadas para turmas desta matriz."));
                }

                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static decimal ObterGrade(Ly_turma.Row turma)
        {
            if (turma == null)
            {
                return 0M;
            }

            string sql = @" select grade_id from ly_grade_serie (NOLOCK) 
                            where grade = ? and ano = ? and semestre = ?
                            and curso = ? and turno = ? and curriculo = ? and serie = ?";

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, turma.Turma, turma.Ano, turma.Semestre,
                    turma.Curso, turma.Turno, turma.Curriculo, turma.Serie);
                if (!valorConsulta.IsNull)
                {
                    return (decimal)valorConsulta;
                }
                else
                {
                    return 0M;
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private static int ObterNumeroMatricula(TConnection connection, string disciplina, string turma, string ano, string semestre)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT COUNT(*) ");
            sql.Append(" FROM LY_MATRICULA (NOLOCK) ");
            sql.Append(" WHERE DISCIPLINA = ? ");
            sql.Append(" AND turma = ?");
            sql.Append(" AND ANO = ? ");
            sql.Append(" AND SEMESTRE = ? ");
            sql.Append(" AND SIT_MATRICULA = 'Matriculado' ");
            sql.Append(" AND ISNULL(DEPENDENCIA, 'N') = 'N' ");

            DbObject valor = TCommand.ExecuteScalar(connection, sql.ToString(), disciplina, turma, ano, semestre);
            if (!valor.IsNull)
                return Convert.ToInt32(valor);
            else
                return 0;
        }

        public static QueryTable ConsultarTurma(string curso, string turno, string curriculo, string serie, string ano, string semestre)
        {
            return Consultar("select distinct GRADE as TURMA, GRADE_ID from ly_grade_serie (NOLOCK) where CURSO = ? and TURNO = ? and CURRICULO = ? and SERIE = ? and ANO = ? and SEMESTRE = ?", curso, turno, curriculo, serie, ano, semestre);
        }

        /// <summary>
        /// Verifica se existe registro em aula docente
        /// </summary>
        /// <param name="turno">turno para a pesquisa</param>
        /// <param name="faculdade">faculdade para a pesquisa</param>
        /// <param name="turma">turma para a pesquisa</param>
        /// <param name="ano">ano para a pesquisa</param>
        /// <param name="semestre">semestre para a pesquisa</param>
        /// <returns>true caso não exista registro em aula docente</returns>
        public static bool VerificarAulaDocente(string turno, string faculdade, string turma, string ano, string semestre)
        {
            var valor = ExecutarFuncaoScalar(
                @"  SELECT TOP 1 1 FROM ly_aula_docente ad 
                    INNER JOIN ly_turma t (NOLOCK) ON
                        ad.disciplina = t.disciplina AND
                        ad.turma = t.turma AND
                        ad.ano = t.ano AND
                        ad.semestre = t.semestre AND
                        ad.data_fim = t.dt_fim
                    WHERE 
                        ad.turno = ? AND
                        ad.faculdade = ? AND
                        ad.turma = ? AND
                        ad.ano = ? AND
                        ad.semestre = ? AND
                        t.sit_turma = 'Aberta'",
                turno, faculdade, turma, ano, semestre);

            return valor.IsNull;
        }

        /// <summary>
        /// Verifica se houve conflito de dependencia para outras turmas no periodo informado.
        /// </summary>        
        public static bool VerificarConflitoDependencia(TConnection connection, string faculdade, string dependencia, DateTime dtInicio, DateTime dtFim, string turma, string turno)
        {
            var sql = new StringBuilder();

            sql.Append(@" 
                SELECT TOP 1
                        1
                FROM    LY_GRADE_SERIE gs ( NOLOCK )
                        INNER JOIN LY_TURMA t ( NOLOCK ) ON t.TURMA = gs.GRADE
                                                            AND t.ANO = gs.ANO
                                                            AND t.SEMESTRE = gs.SEMESTRE
                            WHERE   gs.FACULDADE = ?
                            AND gs.DEPENDENCIA = ?
                            AND gs.GRADE <> ?
                            AND (? BETWEEN gs.DT_INICIO  and gs.DT_FIM 
                                    OR ?  BETWEEN gs.DT_INICIO  and gs.DT_FIM 
                                    )
                           
                        AND T.OPTATIVAREFORCO = 'N'
                        AND ISNULL(T.ELETIVA,'N') = 'N'
                        AND t.SIT_TURMA NOT IN ( 'Finalizada', 'Desativada' ) ");

            if (turno == "I")
            {
                sql.Append(" AND (gs.TURNO = ? OR gs.TURNO = 'M' or gs.TURNO = 'T') ");
            }
            else if (turno == "A")
            {
                sql.Append(" AND (gs.TURNO = ? OR gs.TURNO = 'N' or gs.TURNO = 'T') ");
            }
            else if (turno == "M")
            {
                sql.Append(" AND (gs.TURNO = ? OR gs.TURNO = 'I') ");
            }
            else if (turno == "T")
            {
                sql.Append(" AND (gs.TURNO = ? OR gs.TURNO = 'I' or gs.TURNO = 'A') ");
            }
            else
            {
                sql.Append(" AND (gs.TURNO = ? OR gs.TURNO = 'A') ");
            }

            var valor = TCommand.ExecuteScalar(connection, sql.ToString(), faculdade, dependencia, turma, dtInicio, dtFim, turno);

            return !valor.IsNull;
        }

        /// <summary>
        /// Método auxiliar para transformar a instância de RetValue em uma coleção
        /// de TurmaError para facilitar a manipulação das mensagens.
        /// </summary>
        /// <param name="retorno">RetValue a ser transformado.</param>
        /// <returns>Lista de TurmaError contendo as mensagens do RetValue.</returns>
        public static List<RN.Turma.TurmaError> TransformarErrorList(RetValue retorno)
        {
            List<RN.Turma.TurmaError> lista = new List<RN.Turma.TurmaError>();

            foreach (String fieldList in retorno.Errors.FieldList)
            {
                string[] errors = retorno.Errors[fieldList];

                switch (fieldList.ToUpper().Trim())
                {
                    case "ERRO_VALIDACAO":
                    case "ERRO_GLP":
                        foreach (String error in errors)
                        {
                            decimal aula = Convert.ToDecimal(error.Split('|')[0]);
                            int diaSemana = Convert.ToInt32(error.Split('|')[1]);
                            String mensagem = error.Split('|')[2];
                            DateTime horaInicio = Convert.ToDateTime(error.Split('|')[3]);
                            DateTime horaFim = Convert.ToDateTime(error.Split('|')[4]);
                            String matricula = Convert.ToString(error.Split('|')[5]);
                            String disciplina = Convert.ToString(error.Split('|')[6]);
                            lista.Add(new RN.Turma.TurmaError
                            {
                                DiaDaSemana = diaSemana,
                                HoraFim = horaFim,
                                HoraInicio = horaInicio,
                                Mensagem = mensagem,
                                TipoErro = fieldList.ToUpper().Trim(),
                                Disciplina = disciplina,
                                Matricula = matricula,
                                Aula = aula
                            });
                        }
                        break;

                    case "ERRO":
                        foreach (String error in errors)
                            lista.Add(new RN.Turma.TurmaError { DiaDaSemana = 0, Mensagem = error });
                        break;
                    case "INFORMA_GLP":
                        break;
                    default:
                        //foreach (String error in errors)
                        //    lista.Add(new RN.Turma.TurmaError { DiaDaSemana = 0, Mensagem = error });
                        lista.Add(new RN.Turma.TurmaError { DiaDaSemana = 0, Mensagem = "Erro." });
                        break;
                }
            }
            return lista.Distinct(TurmaErrorComparer.Default).ToList();
        }

        /// <summary>
        /// Método que verifica se é possível alterar o turno de uma turma já criada.
        /// Observar, dentro do método, os comentários sobre as verificações realizadas.
        /// </summary>
        /// <param name="turma">Dados da turma.</param>
        /// <param name="nomeTurma">Código da turma.</param>
        /// <param name="novoTurno">Novo turno selecionado (carregado da interface).</param>
        /// <returns>Lista de erros.</returns>
        public static List<String> PermiteAlterarTurnoDeTurma(DadosTurma turma, String nomeTurma, String novoTurno)
        {
            using (TConnection connection = Config.CreateConnection())
            {
                connection.Open();
                try
                {
                    return PermiteAlterarTurnoDeTurma(connection, turma, nomeTurma, novoTurno);
                }
                catch (TimeoutException)
                {
                    return new List<String>(new string[] { "Tempo excedido durante validação de alteração de turno." });
                }
                catch (Exception)
                {
                    return new List<string>(new string[] { "Erro durante validação de alteração de turno." });
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static DataSet ValidaAlteracaoTurmaComQuadroHorario(string optativaReforco, DadosTurma turma, string nomeTurma, string novoTurno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();
            DataSet ds = new DataSet();

            try
            {
                contextQuery.Command = (@"--Validações da Turma

										--	
										-- 0) Verifica se existe aula_docente válidas alocadas	

										SELECT COUNT(1) 
										FROM ly_aula_docente ad
										INNER JOIN ly_turma t (NOLOCK) ON
											ad.disciplina = t.disciplina AND
											ad.turma = t.turma AND
											ad.ano = t.ano AND
											ad.semestre = t.semestre AND
											ad.data_fim = t.dt_fim
										WHERE
											ad.turno = @DadosTurma_turno AND
											ad.faculdade = @DadosTurma_faculdade AND
											ad.turma = @nomeTurma AND
											ad.ano = @DadosTurma_ano AND
											ad.semestre = @DadosTurma_semestre AND
											t.sit_turma = 'Aberta'
										    
										-- 1) Verifica se a sala de aula está ocupada por outra turma    
										select top 1 turma
										from ly_turma t
										where 
												ano = @DadosTurma_ano 
											AND semestre = @DadosTurma_semestre 
											AND faculdade = @DadosTurma_faculdade 
											AND turno = @novoTurno 
											AND sit_turma NOT IN ('Finalizada', 'Desativada') 
											AND dependencia = (select top 1 DEPENDENCIA
																from ly_turma t
																where 
																	t.turma			= @DadosTurma_turma
																	and t.ano		= @DadosTurma_ano
																	and t.semestre	= @DadosTurma_semestre) 
											AND turma <> @nomeTurma
											
										-- 2) Verifica se mesmo com a alteração do turno, existe matriz curricular semelhante

										SELECT  count(*)
										FROM    LY_CURRICULO
										WHERE		TURNO = @novoTurno
												AND CURSO = @DadosTurma_curso
												AND ANO_INI = @DadosTurma_ano
												AND SEM_INI = @DadosTurma_semestre
												AND ( DT_EXTINCAO IS NULL
													  OR DT_EXTINCAO > GETDATE()
													)  
												and curriculo = @DadosTurma_curriculo
										             
										--ORDER BY CURRICULO	

										-- 3) Verifica se as disciplinas que compõem a matriz são as mesmas
										-- gradeAntiga
										SELECT  g.disciplina, d.nome, d.aulas_semanais 
										FROM    ly_grade g (NOLOCK) INNER JOIN
												ly_disciplina d (NOLOCK) ON d.disciplina = g.disciplina
										WHERE   
											g.curso = @DadosTurma_curso 
											AND g.turno = @DadosTurma_turno
											AND g.curriculo = @DadosTurma_curriculo
											AND g.serie_ideal = @DadosTurma_serie
										--order by g.disciplina

										-- 4) gradeNova
										SELECT  g.disciplina, d.nome, d.aulas_semanais 
										FROM    ly_grade g (NOLOCK) INNER JOIN
												ly_disciplina d (NOLOCK) ON d.disciplina = g.disciplina
										WHERE   
											g.curso = @DadosTurma_curso 
											AND g.turno = @novoTurno
											AND g.curriculo = @DadosTurma_curriculo
											AND g.serie_ideal = @DadosTurma_serie
										--order by g.disciplina
											
										-- 5) Verifica se existe a mesma série

										SELECT count(*)
										FROM LY_SERIE LS
										INNER JOIN LY_CURRICULO LC 
											ON LC.CURRICULO = LS.CURRICULO
											AND LC.ANO_INI = @DadosTurma_ano
											AND LC.SEM_INI = @DadosTurma_semestre
											AND LC.TURNO = LS.TURNO 
											AND LC.CURSO = LS.CURSO
										WHERE
											LS.TURNO = @novoTurno
											AND LS.CURSO = @DadosTurma_curso
											AND LS.CURRICULO = @DadosTurma_curriculo
											AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE()))
											and serie = @DadosTurma_serie
										GROUP BY SERIE, DESCRICAO

										-- 6) Verifica se prefixo é mantido o mesmo
										--  Prefixo antigo
										select COMPLEMENTO1 
										from ly_serie 
										where 
												curso = @DadosTurma_curso 
											and turno = @DadosTurma_turno
											and curriculo = @DadosTurma_curriculo
											and serie = @DadosTurma_serie
											
										-- 7) Prefixo novo
										select COMPLEMENTO1 
										from ly_serie 
										where 
												curso = @DadosTurma_curso 
											and turno = @novoTurno
											and curriculo = @DadosTurma_curriculo
											and serie = @DadosTurma_serie
											
										-- _______________________________________________________________________________________
										-- Se existem aula_docentes ativas bloqueia a alteração das datas de vigência da turma
										---------------------------------------------------------------------------------------

										--8) 
										select top 1 dt_inicio, dt_fim
										from ly_turma t
										where 
											t.turma			= @DadosTurma_turma
											and t.ano		= @DadosTurma_ano
											and t.semestre	= @DadosTurma_semestre
											
										-- 9) NaoExisteAulaDocenteAtiva

										SELECT TOP 1 1
										FROM ly_aula_docente ad 
										INNER JOIN ly_turma t (NOLOCK) ON
											ad.disciplina = t.disciplina AND
											ad.turma = t.turma AND
											ad.ano = t.ano AND
											ad.semestre = t.semestre AND
											ad.data_fim = t.dt_fim
										WHERE
											ad.turno = @DadosTurma_turno AND 
											ad.faculdade = @DadosTurma_faculdade AND 
											ad.turma = @DadosTurma_turma AND 
											ad.ano = @DadosTurma_ano AND 
											ad.semestre = @DadosTurma_semestre AND 
											t.sit_turma = 'Aberta'
										    
										-- _______________________________________________________________________________________
										-- Validação do número máximo de alunos
										---------------------------------------------------------------------------------------
										-- 10) Obter NumAlunoMatriculado
										
										SELECT  COUNT(DISTINCT aluno)
										FROM    dbo.LY_MATRICULA m
								                inner join LY_TURMA t on m.TURMA = t.TURMA
													and m.SEMESTRE = t.SEMESTRE
													and m.ANO = t.ANO 
													and m.DISCIPLINA = t.DISCIPLINA
										WHERE   m.turma = @DadosTurma_turma
												AND m.ano = @DadosTurma_ano
												AND m.SEMESTRE = @DadosTurma_semestre
												AND m.SIT_MATRICULA = 'Matriculado'
												AND ( m.DEPENDENCIA <> 'S'
													OR m.DEPENDENCIA IS NULL )
                                                AND ( t.ELETIVA <> 'S'
                                                    OR t.ELETIVA IS NULL ) 
										
										--select count(DISTINCT ALUNO)
										--from LY_MATGRADE (NOLOCK)
										--where 
										--	SIT_MATGRADE not in ('Cancelado','Trancado','Intercambio', 
										--						'Transf.Internamente','Remanejado', 
										--						'Transf.Externamente','Desistente', 
										--						'Concluido', 'Jubilado') 
										--	and GRADE_ID = @DadosTurma_grade_id
											
										        ");


                contextQuery.Parameters.Add("@DadosTurma_turno", turma.Turno);
                contextQuery.Parameters.Add("@DadosTurma_faculdade", turma.Faculdade);
                contextQuery.Parameters.Add("@DadosTurma_semestre", turma.Periodo);
                contextQuery.Parameters.Add("@nomeTurma", nomeTurma);
                contextQuery.Parameters.Add("@DadosTurma_ano", turma.Ano);
                contextQuery.Parameters.Add("@DadosTurma_turma", turma.Grade);
                contextQuery.Parameters.Add("@DadosTurma_curriculo", turma.Curriculo);
                contextQuery.Parameters.Add("@DadosTurma_curso", turma.Curso);
                contextQuery.Parameters.Add("@DadosTurma_serie", turma.Serie);
                contextQuery.Parameters.Add("@novoTurno", novoTurno);
                contextQuery.Parameters.Add("@DadosTurma_grade_id", turma.Grade_ID);


                ds = ctx.GetDataSet(contextQuery);
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

            return ds;
        }

        public static List<String> PermiteAlterarTurnoDeTurmaComDataSet(DataSet validacaoDadosAlteracao)
        {
            try
            {
                int countAulas = validacaoDadosAlteracao.Tables[0].Rows.Count > 0 ? Convert.ToInt32(validacaoDadosAlteracao.Tables[0].Rows[0][0]) : 0;

                if (countAulas == 1)
                {
                    return new List<String>(new String[] { "Existe 1 aula alocada no Quadro de Horários da turma." });
                }
                else if (countAulas > 1)
                {
                    return new List<String>(new String[] { "Existem " + countAulas + " aulas alocadas no Quadro de Horários da turma." });
                }

                if (validacaoDadosAlteracao.Tables[1].Rows.Count > 0)
                {
                    return new List<String>(new String[] { "Conflito de dependência com a turma " + validacaoDadosAlteracao.Tables[1].Rows[0][0].ToString() + "." });
                }

                if (validacaoDadosAlteracao.Tables[2].Rows.Count == 0 || Convert.ToInt32(validacaoDadosAlteracao.Tables[2].Rows[0][0]) == 0)
                {
                    return new List<String>(new String[] { "Não existe a mesma Matriz Curricular para o Turno, Escolaridade, Ano e Período selecionados." });
                }

                DataTable qtGradeOld = validacaoDadosAlteracao.Tables[3];
                DataTable qtGradeNew = validacaoDadosAlteracao.Tables[4];

                if (qtGradeNew.Rows.Count == 0)
                {
                    return new List<String>(new String[] { "A Matriz Curricular cadastrada para a Escolaridade, Turno e Ano Escolar selecionados não contém disciplinas associadas." });
                }
                if (qtGradeOld.Rows.Count == 0)
                {
                    return new List<String>(new String[] { "Grade inválida." });
                }

                var disciplinasGradeOld = qtGradeOld.Rows;
                var disciplinasGradeNew = qtGradeNew.Rows;

                List<string> disciplinasOld = new List<string>();
                List<string> disciplinasNew = new List<string>();

                foreach (DataRow disciplinaGradeOld in disciplinasGradeOld)
                {
                    disciplinasOld.Add(disciplinaGradeOld[0].ToString());
                }
                foreach (DataRow disciplinaGradeNew in disciplinasGradeNew)
                {
                    disciplinasNew.Add(disciplinaGradeNew[0].ToString());
                }

                //if (!disciplinasNew.SequenceEqual(disciplinasOld))
                //{
                //    return new List<String>() { "A Matriz para a Escolaridade, Turno e Ano Escolar selecionados é composta por disciplinas diferentes da Matriz Curricular original." };
                //}

                if (validacaoDadosAlteracao.Tables[5].Rows.Count == 0 || Convert.ToInt32(validacaoDadosAlteracao.Tables[5].Rows[0][0]) == 0)
                    return new List<String>(new String[] { "Não existe o mesmo Ano Escolar para o Turno, Escolaridade, Matriz Curricular, Ano e Período selecionados." });

                String oldPrefixoSerie = validacaoDadosAlteracao.Tables[6].Rows.Count > 0 ? validacaoDadosAlteracao.Tables[6].Rows[0][0].ToString() : "";
                String newPrefixoSerie = validacaoDadosAlteracao.Tables[7].Rows.Count > 0 ? validacaoDadosAlteracao.Tables[7].Rows[0][0].ToString() : "";

                if (oldPrefixoSerie != newPrefixoSerie)
                    return new List<String>(new String[] { "O prefixo do Ano de Escolaridade é diferente." });

                return new List<String>();
            }
            catch (TimeoutException)
            {
                return new List<String>(new string[] { "Tempo excedido durante validação de alteração de turno." });
            }
            catch (Exception)
            {
                return new List<string>(new string[] { "Erro durante validação de alteração de turno." });
            }
        }

        public static List<String> PermiteAlterarTurnoDeTurma(TConnection connection, DadosTurma turma, String nomeTurma, String novoTurno)
        {
            try
            {
                Ly_turma.Row rowTurma = Ly_turma.QueryFirstRow(connection, "turma = ? AND ano = ? AND semestre = ?",
                    turma.Grade, turma.Ano, turma.Periodo);

                //Verifica se existe aula_docente válidas alocadas
                //<ALTERADO>
                DbObject dB_CountAulas = TCommand.ExecuteScalar(connection,
                    @"  SELECT COUNT(1) FROM ly_aula_docente ad
                    INNER JOIN ly_turma t (NOLOCK) ON
                        ad.disciplina = t.disciplina AND
                        ad.turma = t.turma AND
                        ad.ano = t.ano AND
                        ad.semestre = t.semestre AND
                        ad.data_fim = t.dt_fim
                    WHERE
                        ad.turno = ? AND
                        ad.faculdade = ? AND
                        ad.turma = ? AND
                        ad.ano = ? AND
                        ad.semestre = ? AND
                        t.sit_turma = 'Aberta'",
                        turma.Turno, turma.Faculdade, nomeTurma, turma.Ano, turma.Periodo);
                int countAulas = (dB_CountAulas != null && !dB_CountAulas.IsNull) ? Convert.ToInt32(dB_CountAulas) : 0;

                if (countAulas == 1)
                {
                    return new List<String>(new String[] { "Existe 1 aula alocada no Quadro de Horários da turma." });
                }
                else if (countAulas > 1)
                {
                    return new List<String>(new String[] { "Existem " + countAulas + " aulas alocadas no Quadro de Horários da turma." });
                }

                //Verifica se a sala de aula está ocupada por outra turma
                Ly_turma.Row turmaDependencia = Ly_turma.QueryFirstRow(connection,
                   "ano = ? AND semestre = ? AND faculdade = ? AND turno = ? AND sit_turma NOT IN ('Finalizada', 'Desativada') AND dependencia = ? AND turma <> ?",
                   turma.Ano, turma.Periodo, turma.Faculdade, novoTurno, rowTurma.Dependencia, nomeTurma);
                if (turmaDependencia != null)
                {
                    return new List<String>(new String[] { "Conflito de dependência com a turma " + turmaDependencia.Turma + "." });
                }

                //Verifica se mesmo com a alteração do turno, existe matriz curricular semelhante
                QueryTable qtCurriculos = RN.Curriculo.Consultar(connection, novoTurno, turma.Curso, Convert.ToDecimal(turma.Ano), Convert.ToDecimal(turma.Periodo));
                if (qtCurriculos == null)
                {
                    return new List<String>(new String[] { "Não existe Matriz Curricular para o Turno, Escolaridade, Ano e Período selecionados." });
                }

                var curriculos = qtCurriculos.Rows.Cast<SimpleRow>().Select(c => Convert.ToString(c["curriculo"]));
                if (curriculos.Count(c => c == turma.Curriculo) == 0)
                {
                    return new List<String>(new String[] { "Não existe a mesma Matriz Curricular para o Turno, Escolaridade, Ano e Período selecionados." });
                }

                //Verifica se as disciplinas que compõem a matriz são as mesmas
                QueryTable qtGradeOld = ObterDisciplinaGrade(connection, turma.Curso, turma.Turno, turma.Curriculo, Convert.ToDecimal(turma.Serie));
                QueryTable qtGradeNew = ObterDisciplinaGrade(connection, turma.Curso, novoTurno, turma.Curriculo, Convert.ToDecimal(turma.Serie));

                if (qtGradeNew == null)
                {
                    return new List<String>(new String[] { "A Matriz Curricular cadastrada para a Escolaridade, Turno e Ano Escolar selecionados não contém disciplinas associadas." });
                }
                if (qtGradeOld == null)
                {
                    return new List<String>(new String[] { "Grade inválida." });
                }

                var disciplinasGradeOld = qtGradeOld.Rows.Cast<SimpleRow>().Select(d => d["disciplina"]);
                var disciplinasGradeNew = qtGradeNew.Rows.Cast<SimpleRow>().Select(d => d["disciplina"]);
                foreach (var disciplinaGradeOld in disciplinasGradeOld)
                {
                    if (disciplinasGradeNew.Count(d => d == disciplinaGradeOld) == 0)
                    {
                        return new List<String>() { "A Matriz para a Escolaridade, Turno e Ano Escolar selecionados é composta por disciplinas diferentes da Matriz Curricular original." };
                    }
                }
                foreach (var disciplinaGradeNew in disciplinasGradeNew)
                {
                    if (disciplinasGradeOld.Count(d => d == disciplinaGradeNew) == 0)
                    {
                        return new List<String>() { "A Matriz para a Escolaridade, Turno e Ano Escolar selecionados é composta por disciplinas diferentes da Matriz Curricular original." };
                    }
                }

                //Verifica se existe a mesma série
                QueryTable qtSeries = RN.Serie.Consultar(connection, turma.Ano, turma.Periodo, novoTurno, turma.Curso, turma.Curriculo);
                if (qtSeries == null)
                    return new List<String>(new String[] { "Não existe Ano Escolar para o Turno, Escolaridade, Matriz Curricular, Ano e Período selecionados." });

                var series = qtSeries.Rows.Cast<SimpleRow>().Select(s => Convert.ToDecimal(s["serie"]));
                if (series.Count(s => s == Convert.ToDecimal(turma.Serie)) == 0)
                    return new List<String>(new String[] { "Não existe o mesmo Ano Escolar para o Turno, Escolaridade, Matriz Curricular, Ano e Período selecionados." });

                //Verifica se prefixo é mantido o mesmo
                String oldPrefixoSerie = Serie.ConsultarPrefixoSerie(connection, turma.Curso, turma.Turno, turma.Curriculo, Convert.ToDecimal(turma.Serie));
                String newPrefixoSerie = Serie.ConsultarPrefixoSerie(connection, turma.Curso, novoTurno, turma.Curriculo, Convert.ToDecimal(turma.Serie));
                if (oldPrefixoSerie != newPrefixoSerie)
                    return new List<String>(new String[] { "O prefixo do Ano de Escolaridade é diferente." });

                ////Verificar se sufixo da turma é mantido            
                //QueryTable qtSufixoNew = Serie.ConsultarSufixo(connection, turma.Curso, novoTurno, turma.Curriculo, turma.Serie);
                //var newSufixos = qtSufixoNew.Rows.Cast<SimpleRow>().Select(s => s["sufixo"]);

                return new List<String>();
            }
            catch (TimeoutException)
            {
                return new List<String>(new string[] { "Tempo excedido durante validação de alteração de turno." });
            }
            catch (Exception)
            {
                return new List<string>(new string[] { "Erro durante validação de alteração de turno." });
            }
        }


        /// <summary>
        /// Método utilizado para montar uma mensagem dinâmica de KeyNotFound da TSearch de docentes da tela de Turma, de acordo
        /// com o tipo de situação: matrícula inválida, matrícula sem grupo de habilitação ou matrícula sem lotação ativa.
        /// </summary>
        /// <param name="matriculaDocente">Código da matrícula do docente.</param>
        /// <param name="disciplina">Código da disciplina.</param>
        /// <param name="nomeDisciplina">Nome da disciplina.</param>
        /// <returns>Mensagem dinâmica para ser utilizada na propriedade .Messages.KeyNotFound da TSearch.</returns>
        public static String QueryDocenteQHI_Message_KeyNotFound(String matriculaDocente, String disciplina, String nomeDisciplina, DateTime dataIniTurma, DateTime dataFimTurma)
        {
            if (String.IsNullOrEmpty(matriculaDocente))
                return "Matrícula sem habilitação cadastrada.";

            if (String.IsNullOrEmpty(disciplina))
                return "Selecione uma disciplina.";

            //Matrículas fixas
            switch (matriculaDocente)
            {
                case "00000000": // Carência temp.
                case "11111111": // Prof. município
                case "22222222": // Prof. degase
                case "44444444": // Não ministrada
                case "55555551": // Alocação Concluída
                case "66666666": // Necessidade de contrato temp.
                case "77777777": // Prof. em atuação
                case "88888888": // Prof. convênio
                case "99999999": // Sem prof.
                case "88888880": // PROF. DUPLA ESCOLA
                case "88888881": // PROF. PROEMI  
                    return String.Empty;
                case "55555555": // Sem aula
                    return @"Matrícula bloqueada para alocação. Qualquer dúvida entrar em contato com a Coordenadoria Regional.";
            }

            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                Ly_docente.Row rowDocente = Ly_docente.QueryFirstRow(connection, "matricula = ?", matriculaDocente);
                if (rowDocente == null)
                {
                    return "Matrícula inválida.";
                }

                DateTime hoje = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
                Ly_lotacao.Row rowLotacao = ObterLotacaoAtiva(connection, matriculaDocente, hoje, dataFimTurma);
                if (rowLotacao == null)
                {
                    return "Matrícula não possui lotação ativa.";
                }

                string[] listaDados = disciplina.Split('|');

                if (listaDados.Count() > 1)
                {
                    disciplina = listaDados[1];
                }

                DbObject ghdo = TCommand.ExecuteScalar(connection, @"
                    SELECT TOP 1 0
                    FROM 
	                    ly_grupo_habilitacao_disc ghdi (NOLOCK)
                    JOIN 
	                    ly_grupo_habilitacao_doc ghdo (NOLOCK) ON ghdi.agrupamento = ghdo.agrupamento
                    JOIN 
	                    ly_docente do (NOLOCK) ON ghdo.num_func = do.num_func    
                    WHERE 
	                    (ghdo.provisorio = 'N' OR (ghdo.provisorio = 'S' AND ghdo.DT_LIMITE >= convert(date,GETDATE())))
	                    AND do.num_func = ? AND ghdi.DISCIPLINA = ?", rowDocente.Num_func, disciplina);
                if (ghdo.IsNull)
                {
                    return String.Format("Matrícula sem grupo de habilitação para a disciplina {0}.", nomeDisciplina);
                }

                DbObject licenca = TCommand.ExecuteScalar(connection, @"
                    SELECT TOP 1 
                        l.descricao
                    FROM 
                        ly_licenca_docente ld (NOLOCK)
                        INNER JOIN ly_licencas l (NOLOCK)
                        on ld.motivo = l.motivo
                    WHERE
                        num_func = ? AND dtini <= ? AND dtfim >= ? AND l.motivo <> 43",
                    rowDocente.Num_func, dataFimTurma, dataIniTurma);
                if (!licenca.IsNull)
                    return String.Format("Situação do docente ({0}) não permite alocação.", Convert.ToString(licenca));

                if (!String.IsNullOrEmpty(rowDocente.Candidato) || !String.IsNullOrEmpty(rowDocente.Concurso))
                {
                    Ly_candidato_doc_contrato.Row rowContrato = Ly_candidato_doc_contrato.Row.Query(connection, rowDocente.Concurso, rowDocente.Candidato);
                    if (rowContrato != null)
                    {
                        if (rowContrato.Dt_fim_contrato.HasValue && rowContrato.Dt_inicio_contrato.HasValue)
                        {
                            if (rowContrato.Dt_inicio_contrato.Value > dataFimTurma || rowContrato.Dt_fim_contrato.Value < dataIniTurma)
                                return String.Format("Contrato Temporário não está mais vigente.\n- Data início: {0}.\n- Data fim: {1}.",
                                    rowContrato.Dt_inicio_contrato.Value.ToString("dd/MM/yyyy"), rowContrato.Dt_fim_contrato.Value.ToString("dd/MM/yyyy"));
                        }
                    }
                }
            }
            catch
            {
                return "Erro durante requisição.";
            }
            finally
            {
                connection.Close();
            }
            return "Matrícula inválida.";
        }

        #region Métodos utilizados na Tela "Curriculo/ExclusaoAlocaoDocente"

        /// <summary>
        /// Overload. Desativa os registros em ly_aula_docente_tipo e ly_aula_docente e substitui por carência (real ou temporária).
        /// </summary>
        /// <param name="dtAD">DataTable contendo as chaves dos registro de ly_aula_docente que serão desativados.</param>
        /// <param name="dtADT">DataTable contendo as chaves dos registros de ly_aula_docente_tipo que serão desativados.</param>
        /// <param name="matriculaCarencia">Código de matrícula (00000000 ou 99999999) que irá substituir os registros desativados.</param>
        /// <returns>RetValue contendo o resultado do processamento.</returns>
        public static RetValue DesalocarAulasDocenteAlocarCarencia(Ly_aula_docente.Row[] dtAD, Ly_aula_docente_tipo.Row[] dtADT, String matriculaCarencia)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                return DesalocarAulasDocenteAlocarCarencia(connection, dtAD, dtADT, matriculaCarencia);
            }
            catch (Exception e)
            {
                connection.Rollback();
                return new RetValue(false, "", new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Desativa os registros em ly_aula_docente_tipo e ly_aula_docente e substitui por carência (real ou temporária).
        /// </summary>
        /// <param name="dtAD">DataTable contendo as chaves dos registro de ly_aula_docente que serão desativados.</param>
        /// <param name="dtADT">DataTable contendo as chaves dos registros de ly_aula_docente_tipo que serão desativados.</param>
        /// <param name="matriculaCarencia">Código de matrícula (00000000 ou 99999999) que irá substituir os registros desativados.</param>
        /// <param name="connection">Conexão que será utilizada. Já deve estar aberta.</param>
        /// <returns>RetValue contendo o resultado do processamento.</returns>
        public static RetValue DesalocarAulasDocenteAlocarCarencia(TConnectionWritable connection, Ly_aula_docente.Row[] dtAD, Ly_aula_docente_tipo.Row[] dtADT, String matriculaCarencia)
        {
            if (String.IsNullOrEmpty(matriculaCarencia))
                return new RetValue(false, "", new ErrorList("É necessário selecionar CARÊNCIA TEMPORÁRIA ou CARÊNCIA REAL."));
            if (matriculaCarencia.Trim() != "00000000" && matriculaCarencia.Trim() != "99999999")
                return new RetValue(false, "", new ErrorList("Código de matrícula não pode ser utilizado para substituição. É necessário informar CARÊNCIA TEMPORÁRIA (00000000) ou CARÊNCIA REAL (99999999)."));

            if (dtAD == null)
                return new RetValue(false, "", new ErrorList("Não há aulas para desalocação."));
            if (dtAD.Length == 0)
                return new RetValue(false, "", new ErrorList("Não há aulas para desalocação."));

            //obtém o registro ly_docente da matrícula de carência
            Ly_docente.Row docCarencia = Ly_docente.QueryFirstRow(connection, "matricula = ?", matriculaCarencia);
            if (docCarencia == null)
                return new RetValue(false, "", new ErrorList("Matrícula inválida."));

            int totalDesativadas = 0, glpsDesativadas = 0;
            DateTime hoje = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

            //desativa as GLPs, e atualiza a contagem de GLPs utilizadas na solicitação da GLP
            foreach (Ly_aula_docente_tipo.Row adt in dtADT)
            {
                var adtRow = Ly_aula_docente_tipo.Row.Query(connection, adt.Num_func, adt.Turno, adt.Faculdade, adt.Dia_semana,
                    adt.Aula, adt.Disciplina, adt.Turma, adt.Ano, adt.Semestre, adt.Data_inicio);

                if (adtRow != null)
                {
                    Ly_turma.Row turma = Ly_turma.QueryFirstRow(connection, "turma = ? AND ano = ? AND semestre = ?", adtRow.Turma, adtRow.Ano, adtRow.Semestre);
                    DateTime dtFimAula = hoje < turma.Dt_inicio ? turma.Dt_inicio.Value : hoje;

                    var solicitacaoGLP = Ly_docente_funcao_glp.Row.Query(connection, adtRow.Id_docente_funcao_glp);
                    if (solicitacaoGLP != null)
                    {
                        RetValue ret = null;
                        if (solicitacaoGLP.Glp_usada > 0)
                        {
                            Ly_docente_funcao_glp.Row.Update(connection, solicitacaoGLP.Id_docente_funcao_glp, "glp_usada", solicitacaoGLP.Glp_usada - 1);
                            ret = VerificarErro(connection.GetErrors());
                            if (ret != null && !ret.Ok)
                            {
                                connection.Rollback();
                                return ret;
                            }
                        }

                        Ly_aula_docente_tipo.Row.Update(connection, adtRow.Num_func, adtRow.Turno, adt.Faculdade,
                        adtRow.Dia_semana, adtRow.Aula, adtRow.Disciplina, adtRow.Turma, adtRow.Ano, adtRow.Semestre, adtRow.Data_inicio,
                        "tipo_aula, data_fim", "NGLP", dtFimAula);
                        ret = VerificarErro(connection.GetErrors());
                        if (ret != null && !ret.Ok)
                        {
                            connection.Rollback();
                            return ret;
                        }
                        glpsDesativadas++;
                    }
                }
            }

            //desativa as alocações normais
            foreach (Ly_aula_docente.Row ad in dtAD)
            {
                var adRow = Ly_aula_docente.Row.Query(connection, ad.Num_func, ad.Turno, ad.Faculdade, ad.Dia_semana, ad.Aula,
                    ad.Disciplina, ad.Turma, ad.Ano, ad.Semestre, ad.Data_inicio);

                if (adRow != null)
                {
                    Ly_turma.Row turma = Ly_turma.QueryFirstRow(connection, "turma = ? AND ano = ? AND semestre = ?", adRow.Turma, adRow.Ano, adRow.Semestre);
                    DateTime dtFimAula = hoje < turma.Dt_inicio ? turma.Dt_inicio.Value : hoje;

                    Ly_aula_docente.Row.Update(connection, adRow.Num_func, adRow.Turno, adRow.Faculdade,
                           adRow.Dia_semana, adRow.Aula, adRow.Disciplina, adRow.Turma, adRow.Ano, adRow.Semestre, adRow.Data_inicio,
                           "data_fim, tipo", dtFimAula, TelaExclusaoAlocacao.EXC.ToString());
                    RetValue ret = VerificarErro(connection.GetErrors());
                    if (ret != null && !ret.Ok)
                    {
                        connection.Rollback();
                        return ret;
                    }


                    if (turma.Dt_inicio > hoje)
                    {
                        hoje = turma.Dt_inicio.Value;
                    }

                    //Insere/Reativa a carência no lugar do registro desativado
                    var adCarencia = Ly_aula_docente.Row.Query(connection, docCarencia.Num_func, ad.Turno, ad.Faculdade,
                        ad.Dia_semana, ad.Aula, ad.Disciplina, ad.Turma, ad.Ano, ad.Semestre, hoje);
                    if (adCarencia == null)
                        Ly_aula_docente.Row.Insert(connection, docCarencia.Num_func, ad.Turno, ad.Faculdade, ad.Dia_semana, ad.Aula,
                            ad.Disciplina, ad.Turma, ad.Ano, ad.Semestre, hoje, "data_fim", turma.Dt_fim);
                    else
                        Ly_aula_docente.Row.Update(connection, docCarencia.Num_func, ad.Turno, ad.Faculdade, ad.Dia_semana, ad.Aula,
                            ad.Disciplina, ad.Turma, ad.Ano, ad.Semestre, hoje, "data_fim", turma.Dt_fim);
                    ret = VerificarErro(connection.GetErrors());
                    if (ret != null && !ret.Ok)
                    {
                        connection.Rollback();
                        return ret;
                    }
                    totalDesativadas++;
                }

                #region Atualiza o campo CQHI da tabela LY_CANDIDATO_DOCENTE

                Ly_docente.Row rowDocente = Ly_docente.QueryFirstRow(connection, "num_func = ? AND concurso IS NOT NULL AND candidato IS NOT NULL", ad.Num_func);
                if (rowDocente != null)
                {
                    QueryTable qtAD = new QueryTable(@"
                         SELECT COUNT(1)                                                                        
                         FROM 
                             ly_aula_docente ad
                         INNER JOIN ly_turma t (NOLOCK) ON 
                             ad.disciplina = t.disciplina AND 
                             ad.turma = t.turma AND  
                             ad.ano = t.ano AND 
                             ad.semestre = t.semestre AND 
                             ad.data_fim = t.dt_fim
                         WHERE
                             ad.num_func = ? AND
                             t.sit_turma = 'Aberta'");
                    qtAD.Query(connection, rowDocente.Num_func);

                    if (qtAD.Rows.Count > 0 && Convert.ToDecimal(qtAD.Rows[0][0]) == 0)
                    {
                        Ly_candidato_docente.Row rowCandDoc = Ly_candidato_docente.Row.Query(connection, rowDocente.Concurso, rowDocente.Candidato);
                        if (rowCandDoc != null)
                        {
                            string cqhi = null;
                            if (rowCandDoc.Status == 3)
                            {
                                Ly_candidato_docente.Row.Update(connection, rowCandDoc.Concurso, rowCandDoc.Candidato,
                                    "STATUS, CQHI, DT_CQHI",
                                    2, cqhi, DateTime.Now);
                            }
                            else if (rowCandDoc.Status == 4 && rowCandDoc.Cdrh.ToUpper() == "AUTORIZADO")
                            {
                                Ly_candidato_docente.Row.Update(connection, rowCandDoc.Concurso, rowCandDoc.Candidato,
                                    "STATUS, CQHI, DT_CQHI",
                                    3, cqhi, DateTime.Now);
                            }
                            else if (rowCandDoc.Status == 12)
                            {
                                Ly_candidato_docente.Row.Update(connection, rowCandDoc.Concurso, rowCandDoc.Candidato,
                                    "CQHI, DT_CQHI",
                                    cqhi, DateTime.Now);
                            }

                            RetValue ret = VerificarErro(connection.GetErrors());
                            if (ret != null && !ret.Ok)
                            {
                                connection.Rollback();
                                return ret;
                            }
                        }
                    }
                }

                #endregion
            }

            StringBuilder info = new StringBuilder();
            info.Append("Exclusão realizada com sucesso:<br/>");
            if (glpsDesativadas == 1)
                info.Append(String.Format("- 1 GLP foi desativada.<br/>"));
            else if (glpsDesativadas > 0)
                info.Append(String.Format("- {0} GLPs foram desativadas.<br/>", glpsDesativadas));
            if (totalDesativadas - glpsDesativadas == 1)
                info.Append(String.Format("- 1 alocação foi desativada.<br/>"));
            else if (totalDesativadas - glpsDesativadas > 0)
                info.Append(String.Format("- {0} alocações foram desativadas.<br/>", totalDesativadas - glpsDesativadas));
            return new RetValue(true, info.ToString(), null);
        }

        public static QueryTable ObterAulasDoDocente(String numfunc, String ano)
        {
            QueryTable qt = new QueryTable(@"
                DECLARE @numfunc AS VARCHAR(MAX), @ano T_ANO
                SET @numfunc = ?               
                SET @ano = ?

                SELECT
	                doc.matricula matricula,                
	                mun.codigo municipio,
	                mun.nome municipio_descr,
	                ad.faculdade ue,
	                ue.nome_comp ue_descr,
                    setor.NOVOSETOR,
                    ad.ano ano,
                    ad.semestre semestre,
                    ad.num_func num_func,
                    ad.data_inicio,
                    ad.aula,
	                ue.ID_REGIONAL id_regional,
	                nuc.REGIONAL regional,
	                ue.setor ua,
	                setor.NOME ua_descr,
	                ad.turma turma,
	                ad.turno turno,
	                tur.descricao turno_descr,
	                ad.dia_semana dia_semana,
	                GH.DESCRICAO AS DISCIPLINA_INGRESSO,
	                CASE ad.dia_semana
		                WHEN '2' THEN 'Segunda'
		                WHEN '3' THEN 'Terça'
		                WHEN '4' THEN 'Quarta'
		                WHEN '5' THEN 'Quinta'
		                WHEN '6' THEN 'Sexta'
		                WHEN '7' THEN 'Sábado'
		                END dia_semana_descr,
	                ad.DISCIPLINA disciplina,
	                disc.nome_compl disciplina_descr,
	                CONVERT(VARCHAR, DATEPART(hh, ha.horaini_aula)) + ':' + 
                        RIGHT('0' + CONVERT(VARCHAR, DATEPART(mi, ha.horaini_aula)), 2) hora_inicio,
	                CONVERT(VARCHAR, DATEPART(hh, ha.HORAFIM_AULA)) + ':' + 
                        RIGHT('0' + CONVERT(VARCHAR, DATEPART(mi, ha.horafim_aula)), 2) hora_fim,
	                ISNULL(adt.tipo_aula, 'Normal') tipo,
                    CASE 
                        WHEN ISNULL(ADT.NUM_FUNC, 0) <> 0 AND ISNULL(AD.TIPO_DOCENTE, '99999999') <> '00000000' THEN 'REAL' 
                        WHEN ISNULL(ADT.NUM_FUNC, 0) <> 0 AND AD.TIPO_DOCENTE = '00000000' THEN 'TEMPORARIA' 
                    END AS tipoglp, 
	                'censo' censo,
                    CASE doc.num_func WHEN @numfunc THEN '1' ELSE '0' END pode_excluir,
                    (CONVERT (VARCHAR, PE.IDFUNCIONAL) + '/' + CONVERT (VARCHAR, doc.VINCULO)) AS idvinculo
                FROM
	                ly_docente doc (NOLOCK)
                    INNER JOIN LY_PESSOA PE ON PE.PESSOA = DOC.PESSOA
	                INNER JOIN LY_AULA_DOCENTE ad ON
		                doc.num_func = ad.num_func
                    INNER JOIN LY_TURMA t (NOLOCK) ON
                        ad.disciplina = t.disciplina AND
                        ad.turma = t.turma AND
                        ad.ano = t.ano AND
                        ad.semestre = t.semestre AND
                        ad.data_fim = t.dt_fim
                    LEFT JOIN LY_AULA_DOCENTE_TIPO adt ON
		                ad.num_func = adt.num_func AND
		                ad.turno = adt.turno AND
		                ad.faculdade = adt.faculdade AND
		                ad.dia_semana = adt.dia_semana AND
		                ad.aula = adt.aula AND
		                ad.disciplina = adt.disciplina AND
		                ad.turma = adt.turma AND
		                ad.semestre = adt.semestre AND		                
		                adt.tipo_aula  = 'GLP'
	                JOIN LY_HOR_AULA ha ON
		                ha.turno = ad.turno AND
		                ha.faculdade = ad.faculdade AND
		                ha.dia_semana = ad.dia_semana AND
		                ha.aula = ad.aula AND
		                ha.disciplina = ad.disciplina AND
		                ha.turma = ad.turma AND
		                ha.ano = ad.ano AND
		                ha.semestre = ad.semestre			            	
	                INNER JOIN ly_unidade_ensino ue (NOLOCK) ON
		                ue.unidade_ens = ad.faculdade
	                INNER JOIN TCE_REGIONAL nuc (NOLOCK) ON
		                nuc.ID_REGIONAL = ue.ID_REGIONAL
	                INNER JOIN hades..hd_setor setor (NOLOCK) ON
		                setor.setor = ue.setor
	                INNER JOIN municipio mun (NOLOCK) ON
		                mun.codigo = ue.municipio
	                INNER JOIN ly_turno tur (NOLOCK) ON
		                tur.turno = ad.turno
	                INNER JOIN ly_disciplina disc (NOLOCK) ON
		                disc.disciplina =  ISNULL(T.DISCIPLINA_MULTIPLA, T.DISCIPLINA)
		            LEFT JOIN ly_grupo_habilitacao_doc ghdoc ON ad.NUM_FUNC = ghdoc.NUM_FUNC and AGRUPAMENTO_INGRESSO = 'S'
                                                   LEFT JOIN ly_grupo_habilitacao gh ON ghdoc.agrupamento = gh.agrupamento
                    
                WHERE
	                doc.pessoa = (SELECT pessoa FROM ly_docente (NOLOCK) WHERE num_func = @numfunc) AND
                    ad.ano = @ano AND
                    t.sit_turma = 'Aberta'
                ORDER BY
                    ad.ano,
                    ad.semestre,
	                doc.matricula,
                    tipo desc,
                    tur.turno,
                    dia_semana,
	                ha.horaini_aula,
                    ha.horafim_aula
                                   ");

            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                qt.Query(connection, numfunc, ano);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        public static QueryTable ConsultarDadosDocente(String matricula)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = @"
                SELECT 
                    P.nome_compl nome,
                    d.num_func num_func, 
                    d.matricula matricula, 
                    p.cpf AS cpf,
                    f.descricao AS funcao,     
                    (convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,d.VINCULO)) idvinculo,         
                    ISNULL(cat.nome, d.categoria) AS cargo,
                     (SELECT TOP 1 (convert(varchar,p2.IDFUNCIONAL) + '/' + convert(varchar,doc2.VINCULO))  FROM
	                    ly_docente doc2 (NOLOCK) INNER JOIN
	                    ly_lotacao l2 (NOLOCK) ON doc2.matricula = l2.matricula
						INNER JOIN LY_PESSOA P2 ON P2.PESSOA = DOC2.PESSOA
	                    WHERE doc2.pessoa = d.pessoa AND doc2.matricula <> d.matricula
	                    AND (l2.data_desativacao IS NULL OR CONVERT(DATE, l2.data_desativacao) > CONVERT(DATE,GETDATE()))) matricula2
                FROM 
                    ly_docente d (NOLOCK)
                    INNER JOIN ly_pessoa p (NOLOCK) ON d.pessoa = p.pessoa
                    LEFT JOIN ly_lotacao l (NOLOCK) ON d.matricula = l.matricula
                    LEFT JOIN ly_funcao f (NOLOCK) ON l.funcao = f.funcao                     
                    LEFT JOIN ly_categoria_docente cat (NOLOCK) ON cat.categoria = d.categoria
                    WHERE 
                        CONVERT(DATE,l.data_nomeacao) <= CONVERT(DATE, GETDATE()) AND 
                        (l.data_desativacao IS NULL OR CONVERT(DATE, l.data_desativacao) > CONVERT(DATE,GETDATE())) AND
                        d.num_func = ?";
            try
            {
                qt = new QueryTable(sql);
                qt.Query(connection, matricula);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        #endregion

        public static Ly_grade_serie.Row ConsultarGradeSerie(String curso, String turno, String curriculo, String ano, String periodo, String turma)
        {
            using (TConnection connection = Config.CreateConnection())
            {
                connection.Open();
                try
                {
                    return Ly_grade_serie.QueryFirstRow(connection,
                    "curso = ? AND turno = ? AND curriculo = ? AND ano = ? AND semestre = ? AND grade = ?",
                    curso, turno, curriculo, ano, periodo, turma);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static SimpleRow ObterAulaDocenteAnterior(TConnection connection, String numFunc, String turno, String faculdade, int diaSemana, String aula, String disciplina, String turma, String ano, String semestre)
        {
            QueryTable qtAnterior = new QueryTable(
                @"  SELECT TOP 1                         
                        d.matricula,
                        ad.*
                    FROM ly_aula_docente ad
                    INNER JOIN ly_docente d (NOLOCK) ON 
                        d.num_func = ad.num_func
                    WHERE 
                        ad.num_func <> ? AND 
                        ad.turno = ? AND 
                        ad.faculdade = ? AND 
                        ad.dia_semana = ? AND 
                        ad.aula = ? AND 
                        ad.disciplina = ? AND 
                        ad.turma = ? AND 
                        ad.ano = ? AND 
                        ad.semestre = ?                     
                    ORDER BY 
                        ad.data_inicio DESC, 
                        ad.data_fim DESC, 
                        ad.stamp_atualizacao DESC");
            qtAnterior.Query(connection, numFunc, turno, faculdade, diaSemana, aula,
                disciplina, turma, ano, semestre);
            return qtAnterior.Rows.Count > 0 ? qtAnterior.Rows[0] : null;
        }

        public static IEnumerable<Ly_licencas.Row> ObterLicencasAtivas(TConnection connection, decimal? num_func, DateTime? dtIniTurma, DateTime? dtFimTurma)
        {
            QueryTable qt = new QueryTable(
            @"  DECLARE @num_func T_NUMFUNC = ?
                DECLARE @dataInicioTurma T_DATA = ?
                DECLARE @dataFimTurma T_DATA = ?                
                DECLARE @hoje DATE = CONVERT(DATE, GETDATE())

                IF @hoje >= @dataInicioTurma AND @hoje <= @dataFimTurma                
	                SELECT motivo FROM ly_licenca_docente ld (NOLOCK)
	                WHERE ld.num_func = @num_func AND
	                (ld.DTFIM >= @hoje OR ld.DTFIM IS NULL) AND
	                ld.DTINI <= @dataFimTurma                
                ELSE                
	                SELECT motivo FROM ly_licenca_docente ld (NOLOCK)
	                WHERE ld.num_func = @num_func AND
	                (ld.DTFIM >= @dataInicioTurma OR ld.DTFIM IS NULL) AND
	                ld.DTINI <= @dataFimTurma");
            qt.Query(connection, num_func, dtIniTurma, dtFimTurma);

            List<Ly_licencas.Row> licencas = new List<Ly_licencas.Row>();

            foreach (SimpleRow row in qt.Rows)
            {
                Ly_licencas.Row licenca = Ly_licencas.Row.Query(connection, Convert.ToString(row["motivo"]));
                if (licenca != null)
                {
                    licencas.Add(licenca);
                }
            }
            return licencas;
        }

        public static QueryTable ObterTurmas(decimal ano, decimal semestre, decimal? serie, string curso, string nucleo, string unidade_ens, bool adicionar_item_nenhum)
        {
            List<DbObject> parameters = new List<DbObject>();
            StringBuilder sql = new StringBuilder();

            if (adicionar_item_nenhum)
            {
                sql.AppendLine("SELECT null AS grade_id, '<NENHUMA>' AS turma");
                sql.AppendLine("UNION");
            }

            sql.AppendLine(@"
                SELECT DISTINCT 
                    gs.grade_id AS grade_id, 
                    gs.grade    AS turma                    
                FROM 
                    ly_grade_serie gs (NOLOCK) INNER JOIN 
                    ly_unidade_ensino ue (NOLOCK) ON ue.unidade_ens = gs.faculdade
                WHERE
                    gs.ano = ? AND 
                    gs.semestre = ?");

            parameters.Add(ano);
            parameters.Add(semestre);

            if (serie.HasValue)
            {
                parameters.Add(serie);
                sql.AppendLine(" AND gs.serie = ?");
            }

            if (!string.IsNullOrEmpty(curso))
            {
                parameters.Add(curso);
                sql.AppendLine(" AND gs.curso = ?");
            }

            if (!string.IsNullOrEmpty(nucleo))
            {
                parameters.Add(nucleo);
                sql.AppendLine(" AND ue.nucleo = ?");
            }

            if (!string.IsNullOrEmpty(unidade_ens))
            {
                parameters.Add(unidade_ens);
                sql.AppendLine(" AND gs.unidade_responsavel  = ?");
            }

            sql.AppendLine("ORDER BY turma ASC");

            return Consultar(sql.ToString(), parameters.ToArray());
        }

        public static QueryTable ObterTurmasMesmaUnidadeESituacao(string turma, string ano, string semestre)
        {
            TConnection conn = Config.CreateConnection();

            conn.Open();

            try
            {
                QueryTable qt = new QueryTable(@"
                    DECLARE @ano T_ANO,
                            @semestre T_SEMESTRE2,
                            @turma VARCHAR(50),
                            @faculdade T_CODIGO,
                            @sit_turma T_ALFASMALL              

                    SET @turma = ?
                    SET @ano = ?
                    SET @semestre = ?                                       

                    SELECT  @faculdade = faculdade, @sit_turma = sit_turma 
                    FROM    ly_turma (NOLOCK)
                    WHERE   turma = @turma AND ano = @ano AND semestre = @semestre
                    
                    SELECT  DISTINCT turma, ano, semestre,
                            substring(turma, 0, 1 + LEN(turma) - CHARINDEX('-', REVERSE(turma))) turma_nome
                    FROM    ly_turma (NOLOCK)
                    WHERE   faculdade = @faculdade 
                            AND ano = @ano 
                            AND semestre = @semestre
                            AND sit_turma = @sit_turma
                    ORDER BY turma
                ");

                qt.Query(conn, turma, ano, semestre);

                return qt;
            }
            catch
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DadosTurma ObterDadosTurma(string turma, string ano, string semestre)
        {
            TConnection conn = Config.CreateConnection();

            conn.Open();

            try
            {
                QueryTable qt = new QueryTable(@"                     
                    SELECT TOP 1 
                        GS.GRADE_ID, 
                        GS.CURSO,
                        GS.TURNO, 
                        GS.SERIE, 
                        GS.ANO,
                        GS.SEMESTRE,
                        GS.GRADE TURMA,
                        GS.CURRICULO,
                        GS.FACULDADE,
                        TU.TURMA_INTEGRACAO AS SUFIXO,
                        UE.NUCLEO,
                        UE.MUNICIPIO,
                        UE.MNEMONICO,
                        TU.DEPENDENCIA
                    FROM
                        LY_GRADE_SERIE  GS (NOLOCK)
                        inner join ly_unidade_ensino UE (NOLOCK) ON
                            UE.unidade_ens = GS.faculdade
                        inner join LY_TURMA TU (NOLOCK) ON 
                            TU.TURMA = GS.GRADE AND
                            TU.ANO = GS.ANO AND 
                            TU.SEMESTRE = GS.SEMESTRE AND 
	                        TU.CURRICULO = GS.CURRICULO AND
	                        TU.CURSO = GS.CURSO AND
                            TU.TURNO = GS.TURNO AND
	                        TU.SERIE = GS.SERIE
                    WHERE
                        TU.TURMA = ? AND
                        TU.ANO = ? AND
                        TU.SEMESTRE = ?
                ");

                qt.Query(conn, turma, ano, semestre);

                if (qt.Rows.Count > 0)
                {
                    DadosTurma dados = new DadosTurma
                    {
                        Ano = Convert.ToString(qt.Rows[0]["ANO"]),
                        Curriculo = Convert.ToString(qt.Rows[0]["CURRICULO"]),
                        Curso = Convert.ToString(qt.Rows[0]["CURSO"]),
                        Faculdade = Convert.ToString(qt.Rows[0]["FACULDADE"]),
                        Grade = Convert.ToString(qt.Rows[0]["TURMA"]),
                        Grade_ID = Convert.ToString(qt.Rows[0]["GRADE_ID"]),
                        Municipio = Convert.ToString(qt.Rows[0]["MUNICIPIO"]),
                        MnemonicoUnidadeResponsavel = Convert.ToString(qt.Rows[0]["MNEMONICO"]),
                        Nucleo = Convert.ToString(qt.Rows[0]["NUCLEO"]),
                        Periodo = Convert.ToString(qt.Rows[0]["SEMESTRE"]),
                        Serie = Convert.ToString(qt.Rows[0]["SERIE"]),
                        Sufixo = Convert.ToString(qt.Rows[0]["SUFIXO"]),
                        Turno = Convert.ToString(qt.Rows[0]["TURNO"]),
                        UnidadeResponsavel = Convert.ToString(qt.Rows[0]["FACULDADE"])
                    };
                    return dados;
                }
            }
            catch { }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// Atualiza a situação da turma (Ly_turma.sit_turma)
        /// </summary>
        /// <param name="connW">Conexão.</param>
        /// <param name="turma">Turma.</param>
        /// <param name="ano">Ano da turma.</param>
        /// <param name="semestre">Semestre da turma.</param>
        /// <param name="novaSituacao">Nova situação da turma;</param>
        /// <returns>Erro, se ocorrer.</returns>
        public static RetValue AlterarSituacaoTurma(TConnectionWritable connW, string turma, string ano, string semestre, string novaSituacao)
        {
            return IAE(connW,
                @"  UPDATE  ly_turma
                    SET     sit_turma = ?
                    WHERE   turma = ? AND
                            ano = ? AND
                            semestre = ?", novaSituacao, turma, ano, semestre);
        }

        #region Métodos para Transferência de alocação

        /// <summary>
        /// Obtém as aulas que estão disponíveis (sem alocação) na turma, ano, semestre.
        /// Caso seja especificada uma disciplina, retorna também, além das aulas disponíveis, as alocações
        /// em carência (Real ou Temporária) na turma para a disciplina especificada.
        /// </summary>
        /// <param name="turmaDestino">Código da turma.</param>
        /// <param name="ano">Ano da turma.</param>
        /// <param name="semestre">Semestre da turma.</param>
        /// <param name="disciplinaDestino">(Opcional) Disciplina para retornar a(s) carência(s) se existentes.</param>
        /// <returns></returns>
        public static List<AulaSelecionada> ObterAlocacoesDisponiveis(String turmaDestino, decimal? ano, decimal? semestre, string disciplinaDestino)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            List<AulaSelecionada> aulas = new List<AulaSelecionada>();

            try
            {
                //<ALTERADO>
                QueryTable qt = new QueryTable(@"
                declare @turma VARCHAR(50) = ?
                declare @ano T_ANO = ?
                declare @semestre T_SEMESTRE2 = ?
                declare @diaSemana numeric = ?

                SELECT DISTINCT 
                    ho.aula, 
                    @diaSemana AS diaSemana, 
                    ho.horaini_aula, 
                    ho.horafim_aula
                FROM ly_hor_oper ho 
                INNER JOIN ly_turma tu (NOLOCK) ON
                    ho.faculdade = tu.faculdade AND 
                    ho.turno = tu.turno AND    
                    ho.curso = tu.curso AND 
                    ho.curriculo = tu.curriculo AND 
                    ho.serie = tu.serie                
                WHERE 
                    tu.turma = @turma AND 
                    tu.ano = @ano AND 
                    tu.semestre = @semestre AND
                    ho.dia_semana = @diaSemana

                EXCEPT

                SELECT 
                    ad.aula, 
                    ad.dia_semana, 
                    ha.horaini_aula, 
                    ha.horafim_aula 
                FROM
                    ly_aula_docente ad 
                    INNER JOIN ly_turma tu (NOLOCK) ON 
                        ad.turma = tu.turma AND 
                        ad.disciplina = tu.disciplina AND
                        ad.ano = tu.ano AND
                        ad.semestre = tu.semestre AND
                        ad.data_fim = tu.dt_fim
                    INNER JOIN ly_hor_aula ha ON
                        ha.turno = ad.turno AND 
                        ha.faculdade = ad.faculdade AND 
                        ha.dia_semana = ad.dia_semana AND 
                        ha.aula = ad.aula AND 
                        ha.disciplina = ad.disciplina AND 
                        ha.turma = ad.turma AND 
                        ha.ano = ad.ano AND 
                        ha.semestre = ad.semestre
                    WHERE 
                        ad.turma = @turma AND 
                        ad.ano = @ano AND 
                        ad.semestre = @semestre AND 
                        ad.dia_semana = @diaSemana AND
                        tu.sit_turma = 'Aberta'");

                for (int diaSemana = 2; diaSemana <= 7; diaSemana++)
                {
                    qt.Query(connection, turmaDestino, ano, semestre, diaSemana);
                    foreach (SimpleRow row in qt.Rows)
                    {
                        AulaSelecionada aula = new AulaSelecionada
                        {
                            Aula = Convert.ToDecimal(row["aula"]),
                            DiaSemana = diaSemana,
                            HoraIni = Convert.ToDateTime(row["horaini_aula"]).ToString("HH:mm"),
                            HoraFim = Convert.ToDateTime(row["horafim_aula"]).ToString("HH:mm"),
                            Observacao = "Sem alocação",
                            IsCarencia = false
                        };
                        aulas.Add(aula);
                    }
                }

                if (!String.IsNullOrEmpty(disciplinaDestino))
                {
                    qt = new QueryTable(@"
                    declare @turma VARCHAR(50) = ?
                    declare @ano T_ANO = ?
                    declare @semestre T_SEMESTRE2 = ?
                    declare @disciplina T_CODIGO = ?
           
                    SELECT 
                        ad.aula, 
                        ad.dia_semana, 
                        ha.horaini_aula, 
                        ha.horafim_aula, 
                        pe.nome_compl, 
                        doc.matricula, 
                        doc.num_func 
                    FROM 
                        ly_aula_docente ad 
                        INNER JOIN ly_turma tu (NOLOCK) ON 
                            ad.turma = tu.turma AND 
                            ad.disciplina = tu.disciplina AND
                            ad.ano = tu.ano AND
                            ad.semestre = tu.semestre AND
                            ad.data_fim = tu.dt_fim
                        INNER JOIN ly_hor_aula ha ON
                            ha.turno = ad.turno AND 
                            ha.faculdade = ad.faculdade AND 
                            ha.dia_semana = ad.dia_semana AND 
                            ha.aula = ad.aula AND 
                            ha.disciplina = ad.disciplina AND 
                            ha.turma = ad.turma AND 
                            ha.ano = ad.ano AND 
                            ha.semestre = ad.semestre
                        INNER JOIN ly_docente doc (NOLOCK) ON 
                            doc.num_func = ad.num_func
                        INNER JOIN ly_pessoa pe (NOLOCK) ON 
                            doc.pessoa = pe.pessoa
                        WHERE 
                            ad.turma = @turma AND 
                            ad.ano = @ano AND 
                            ad.semestre = @semestre AND 
                            doc.matricula IN ('00000000', '99999999') AND
                            ad.disciplina = @disciplina AND
                            ad.sit_turma = 'Aberta'");
                    qt.Query(connection, turmaDestino, ano, semestre, disciplinaDestino);
                    int carenciaOrderID = 0;
                    foreach (SimpleRow row in qt.Rows)
                    {
                        AulaSelecionada aula = new AulaSelecionada();
                        aula.Aula = Convert.ToDecimal(row["aula"]);
                        aula.DiaSemana = Convert.ToDecimal(row["dia_semana"]);
                        aula.HoraIni = Convert.ToDateTime(row["horaini_aula"]).ToString("HH:mm");
                        aula.HoraFim = Convert.ToDateTime(row["horafim_aula"]).ToString("HH:mm");
                        aula.Observacao = Convert.ToString(row["nome_compl"]);
                        aula.Matricula = Convert.ToString(row["matricula"]);
                        aula.NumFunc = Convert.ToDecimal(row["num_func"]);
                        aula.IsCarencia = true;
                        aula.CarenciaOrderID = carenciaOrderID++;
                        aulas.Add(aula);
                    }
                }
            }
            catch { }
            finally
            {
                connection.Close();
            }
            return aulas.OrderBy(a => a.HoraIni).OrderBy(a => a.DiaSemana).ToList();
        }

        /// <summary>
        /// Valida e realiza a transferência de alocações, da origem para o destino.        
        /// </summary>
        /// <param name="aulasOrigem">Coleção de aulas que serão removidas da turma original.</param>
        /// <param name="aulasDestino">Coleção de aulas que serão alocadas na turma de destino.</param>
        /// <returns></returns>
        public static RetValue TransferirAlocacoes(List<AulaSelecionada> aulasOrigem, List<AulaSelecionada> aulasDestino, String codigoCarencia, String disciplinaDestino, String turmaDestino, decimal anoTurmaDestino, decimal semestreTurmaDestino, String turnoTurmaDestino)
        {
            ErrorList logs = new ErrorList();
            TConnectionWritable conn = Config.CreateWritableConnection();
            conn.Open(true);
            RN.Docentes rnDocentes = new Docentes();
            RN.DTOs.DadosDocente dadosDocente = new Techne.Lyceum.RN.DTOs.DadosDocente();

            try
            {
                #region Verifica se o código da carência é válido
                if (String.IsNullOrEmpty(codigoCarencia))
                    return new RetValue(false, "", new ErrorList("O código da carência deve ser fornecido."));
                if (codigoCarencia.Trim() != "00000000" && codigoCarencia.Trim() != "99999999")
                    return new RetValue(false, "", new ErrorList(String.Format("Código de carência {0} inválido. Deve ser 00000000 ou 99999999.", codigoCarencia)));
                #endregion

                if (aulasOrigem == null || (aulasOrigem != null && aulasOrigem.Count == 0))
                    return new RetValue(false, "", new ErrorList("Nenhuma aula foi selecionada para a realização da transferência de alocações."));

                if (aulasDestino == null || (aulasDestino != null && aulasDestino.Count == 0))
                    return new RetValue(false, "", new ErrorList("Nenhum aula livre foi selecionada "));

                if (aulasOrigem.Count != aulasDestino.Count)
                    return new RetValue(false, "", new ErrorList("O número de aulas selecionadas na turma de origem deve ser o mesmo de aulas selecionadas na turma de destino."));

                #region Verificação: Aulas da origem não podem ser GLP
                foreach (AulaSelecionada aulaOrigem in aulasOrigem)
                {
                    //<ALTERADO>
                    SimpleRow aulaOrigemGLP = SimpleRow.QueryFirstRow(conn,
                        @"  SELECT TOP 1 1 
                            FROM ly_aula_docente_tipo adt
                            INNER JOIN ly_turma tu (NOLOCK) ON
                                adt.disciplina = tu.disciplina AND
                                adt.turma = tu.turma AND
                                adt.ano = tu.ano AND
                                adt.semestre = tu.semestre
                            WHERE
                                tu.sit_turma = ? AND
                                adt.tipo_aula = ? AND                                
                                adt.num_func = ? AND 
                                adt.turno = ? AND 
                                adt.dia_semana = ? AND 
                                adt.aula = ? AND 
                                adt.disciplina = ? AND 
                                adt.turma = ? AND 
                                adt.ano = ? AND
                                adt.semestre = ?",
                            "Aberta", "GLP", aulaOrigem.NumFunc, aulaOrigem.Turno, aulaOrigem.DiaSemana, aulaOrigem.Aula,
                            aulaOrigem.Disciplina, aulaOrigem.Turma, aulaOrigem.Ano, aulaOrigem.Semestre);
                    if (aulaOrigemGLP != null)
                        return new RetValue(false, "", new ErrorList(String.Format("Não é permitido transferir GLP. Disciplina={0}, Docente={1}, DiaSemana={2}, Horário={3}-{4}",
                            aulaOrigem.DisciplinaDescr, aulaOrigem.NomeDocente, aulaOrigem.DiaSemanaDescr, aulaOrigem.HoraIni, aulaOrigem.HoraFim)));
                }
                #endregion

                #region Aulas de origem devem ser do mesmo docente e mesma disciplina, e não podem ser carência
                if (aulasOrigem.Select(ao => ao.NumFunc).Distinct().Count() > 1)
                    return new RetValue(false, "", new ErrorList("As aulas a serem transferidas devem ser do mesmo docente."));
                if (aulasOrigem.Select(ao => ao.Disciplina).Distinct().Count() > 1)
                    return new RetValue(false, "", new ErrorList("As aulas a serem transferidas devem ser da mesma disciplina."));
                if (aulasOrigem.Count(ao => ao.Matricula == "00000000" || ao.Matricula == "99999999") > 0)
                    return new RetValue(false, "", new ErrorList("Aulas em carência não podem ser transferidas."));
                #endregion

                #region Verificação: Aulas do destino estão livres ou são CARÊNCIA (00000000, 99999999)
                foreach (AulaSelecionada aulaDestino in aulasDestino)
                {
                    //<ALTERADO>
                    SimpleRow rowAulaLivre = SimpleRow.QueryFirstRow(conn,
                    @"  SELECT TOP 1 doc.matricula
                        FROM ly_docente doc (NOLOCK)
                        WHERE                            
                            doc.matricula NOT IN (?, ?) AND
                            EXISTS (
                                SELECT TOP 1 1
                                FROM ly_aula_docente ad
                                INNER JOIN ly_turma tu (NOLOCK) ON
                                    ad.disciplina = tu.disciplina AND
                                    ad.turma = tu.turma AND
                                    ad.ano = tu.ano AND
                                    ad.semestre = tu.semestre AND
                                    ad.data_fim = tu.dt_fim
                                WHERE 
                                    ad.num_func = doc.num_func AND
                                    tu.sit_turma = ? AND                                    
                                    ad.turno = ? AND 
                                    ad.dia_semana = ? AND 
                                    ad.aula = ? AND 
                                    ad.disciplina = ? AND 
                                    ad.turma = ? AND 
                                    ad.ano = ? AND 
                                    ad.semestre = ?",
                        "00000000", "99999999", "Aberta", aulaDestino.Turno, aulaDestino.DiaSemana, aulaDestino.Aula,
                        aulaDestino.Disciplina, aulaDestino.Turma, aulaDestino.Ano, aulaDestino.Semestre);
                    if (rowAulaLivre != null)
                    {
                        return new RetValue(false, "",
                            new ErrorList(String.Format("Já existe aula alocada. Disciplina={)}, Docente={1}, DiaSemana={2}, Horário={3}-{4}",
                            aulaDestino.DisciplinaDescr, aulaDestino.NomeDocente, aulaDestino.DiaSemanaDescr, aulaDestino.HoraIni, aulaDestino.HoraFim)));
                    }
                }

                #endregion

                #region Remove as aulas da origem
                List<Ly_aula_docente.Row> adsDesativar = new List<Ly_aula_docente.Row>();
                logs.Add(String.Format("Alocações desativadas na Turma:{0}, Ano:{1}, Semestre:{2}", aulasOrigem[0].Turma, aulasOrigem[0].Ano, aulasOrigem[0].Semestre) + "<br/>");

                foreach (AulaSelecionada aulaOrigem in aulasOrigem)
                {
                    QueryTable qtADDesativar = new QueryTable(@"
                        SELECT ad.* 
                        FROM ly_aula_docente ad 
                        INNER JOIN ly_turma tu (NOLOCK) ON
                            tu.disciplina = ad.disciplina AND
                            tu.turma = ad.turma AND
                            tu.ano = ad.ano AND
                            tu.semestre = ad.semestre AND
                            tu.dt_fim = ad.data_fim
                        WHERE
                            ad.num_func = ? AND 
                            ad.turno = ? AND 
                            ad.dia_semana = ? AND 
                            ad.aula = ? AND 
                            ad.disciplina = ? AND 
                            ad.turma = ? AND 
                            ad.ano = ? AND 
                            ad.semestre = ? AND
                            tu.sit_turma = ?");
                    qtADDesativar.Query(conn,
                        aulaOrigem.NumFunc, aulaOrigem.Turno, aulaOrigem.DiaSemana, aulaOrigem.Aula,
                        aulaOrigem.Disciplina, aulaOrigem.Turma, aulaOrigem.Ano, aulaOrigem.Semestre, "Aberta");
                    foreach (SimpleRow queryRowDesativar in qtADDesativar.Rows)
                    {
                        decimal ad_num_func = Convert.ToDecimal(queryRowDesativar["num_func"]);
                        string ad_turno = Convert.ToString(queryRowDesativar["turno"]);
                        string ad_faculdade = Convert.ToString(queryRowDesativar["faculdade"]);
                        decimal ad_dia_semana = Convert.ToDecimal(queryRowDesativar["dia_semana"]);
                        decimal ad_aula = Convert.ToDecimal(queryRowDesativar["aula"]);
                        string ad_disciplina = Convert.ToString(queryRowDesativar["disciplina"]);
                        string ad_turma = Convert.ToString(queryRowDesativar["turma"]);
                        decimal ad_ano = Convert.ToDecimal(queryRowDesativar["ano"]);
                        decimal ad_semestre = Convert.ToDecimal(queryRowDesativar["semestre"]);
                        DateTime ad_data_inicio = Convert.ToDateTime(queryRowDesativar["data_inicio"]);

                        Ly_aula_docente.Row rowDesativar = Ly_aula_docente.Row.Query(conn,
                            ad_num_func, ad_turno, ad_faculdade, ad_dia_semana, ad_aula, ad_disciplina,
                            ad_turma, ad_ano, ad_semestre, ad_data_inicio);

                        if (rowDesativar != null)
                        {
                            adsDesativar.Add(rowDesativar);
                            logs.Add(String.Format("  - Matrícula:{0}   Docente:{1}   Dia Semana:{2}    Horário:{3}-{4}",
                                aulaOrigem.Matricula, aulaOrigem.NomeDocente, aulaOrigem.DiaSemanaDescr, aulaOrigem.HoraIni, aulaOrigem.HoraFim) + "<br/>");
                        }
                    }
                }

                RetValue retDesalocacao = DesalocarAulasDocenteAlocarCarencia(conn, adsDesativar.ToArray(), new Ly_aula_docente_tipo.Row[0], codigoCarencia);
                if (retDesalocacao != null && !retDesalocacao.Ok)
                {
                    conn.Rollback();
                    return retDesalocacao;
                }
                #endregion

                #region Aloca as aulas no destino
                Ly_turma dtTurmaDestino = Ly_turma.Query(conn, "turma = ?  AND ano = ? AND semestre = ?", turmaDestino, anoTurmaDestino, semestreTurmaDestino);
                Ly_grade_turma dtGradeTurmaDestino = Ly_grade_turma.Query(conn, "turma = ? AND ano = ? AND semestre = ?", turmaDestino, anoTurmaDestino, semestreTurmaDestino);
                Ly_disciplina.Row rowDisciplinaDestino = Ly_disciplina.Row.Query(conn, disciplinaDestino);

                if (dtTurmaDestino == null || dtGradeTurmaDestino == null)
                    return new RetValue(false, "", new ErrorList("A turma de destino não existe ou não está corretamente cadastrada no sistema."));
                if (dtTurmaDestino.Rows.Cast<Ly_turma.Row>().Count(t => t.Disciplina == disciplinaDestino) == 0 ||
                    dtGradeTurmaDestino.Rows.Cast<Ly_grade_turma.Row>().Count(gt => gt.Disciplina == disciplinaDestino) == 0)
                    return new RetValue(false, "", new ErrorList("A disciplina de destino não existe para a turma de destino ou não está corretamente cadastrada no sistema."));

                String nomeTurmaDestino = dtTurmaDestino.Rows[0].Turma;
                decimal numFuncOrigem = aulasOrigem[0].NumFunc;

                DadosTurma dadosTurmaOld = new DadosTurma
                {
                    Ano = Convert.ToString(dtTurmaDestino.Rows[0].Ano),
                    Curriculo = dtTurmaDestino.Rows[0].Curriculo,
                    Curso = dtTurmaDestino.Rows[0].Curso,
                    Faculdade = dtTurmaDestino.Rows[0].Faculdade,
                    Grade = dtTurmaDestino.Rows[0].Turma,
                    Grade_ID = Convert.ToString(dtGradeTurmaDestino.Rows[0].Grade_id),
                    Periodo = Convert.ToString(dtTurmaDestino.Rows[0].Semestre),
                    Serie = Convert.ToString(dtTurmaDestino.Rows[0].Serie),
                    Turno = dtTurmaDestino.Rows[0].Turno,
                    UnidadeResponsavel = dtTurmaDestino.Rows[0].Unidade_responsavel
                };

                Ly_hor_aula dtHoraAula = new Ly_hor_aula();
                Ly_aula_docente dtAulaDocente = new Ly_aula_docente();

                foreach (AulaSelecionada aulaDestino in aulasDestino)
                {
                    Ly_hor_aula.Row ha = dtHoraAula.NewRow();
                    ha.Ano = dtTurmaDestino.Rows[0].Ano;
                    ha.Aula = aulaDestino.Aula;
                    ha.Dependencia = dtTurmaDestino.Rows[0].Dependencia;
                    ha.Dia_semana = aulaDestino.DiaSemana;
                    ha.Disciplina = rowDisciplinaDestino.Disciplina;
                    ha.Faculdade = dtTurmaDestino.Rows[0].Faculdade;
                    ha.Horaini_aula = new DateTime(1899, 12, 30, Convert.ToInt32(aulaDestino.HoraIni.Split(':')[0]), Convert.ToInt32(aulaDestino.HoraIni.Split(':')[1]), 0);
                    ha.Horafim_aula = new DateTime(1899, 12, 30, Convert.ToInt32(aulaDestino.HoraFim.Split(':')[0]), Convert.ToInt32(aulaDestino.HoraFim.Split(':')[1]), 0);
                    ha.Semestre = dtTurmaDestino.Rows[0].Semestre;
                    ha.Turma = dtTurmaDestino.Rows[0].Turma;
                    ha.Turno = dtTurmaDestino.Rows[0].Turno;
                    ha.Qtde_aula = 1;
                    dtHoraAula.Rows.Add(ha);

                    Ly_aula_docente.Row ad = dtAulaDocente.NewRow();
                    ad.Ano = dtTurmaDestino.Rows[0].Ano;
                    ad.Aula = aulaDestino.Aula;
                    ad.Data_fim = dtTurmaDestino.Rows[0].Dt_fim;
                    ad.Data_inicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
                    ad.Dia_semana = aulaDestino.DiaSemana;
                    ad.Disciplina = rowDisciplinaDestino.Disciplina;
                    ad.Faculdade = dtTurmaDestino.Rows[0].Faculdade;
                    ad.Num_func = numFuncOrigem;
                    ad.Semestre = dtTurmaDestino.Rows[0].Semestre;
                    ad.Turma = dtTurmaDestino.Rows[0].Turma;
                    ad.Turno = dtTurmaDestino.Rows[0].Turno;
                    dtAulaDocente.Rows.Add(ad);
                }

                #region Insere as aulas transferidas às cegas (o quadro todo será revalidado posteriormente)
                logs.Add(String.Format("<br/>Aulas transferidas para a Turma:{0}, Ano:{1}, Semestre:{2}", dtAulaDocente.Rows[0].Turma, dtAulaDocente.Rows[0].Ano, dtAulaDocente.Rows[0].Semestre) + "<br/>");
                foreach (Ly_aula_docente.Row rowAulaDocente in dtAulaDocente.Rows)
                {
                    #region Recupera o Ly_hora_aula corresponde à Ly_aula_docente da iteração

                    var hasDestino = dtHoraAula.Rows.Cast<Ly_hor_aula.Row>().Where(ha =>
                        ha.Turno == rowAulaDocente.Turno && ha.Faculdade == rowAulaDocente.Faculdade &&
                        ha.Dia_semana == rowAulaDocente.Dia_semana && ha.Aula == rowAulaDocente.Aula &&
                        ha.Disciplina == rowAulaDocente.Disciplina && ha.Turma == rowAulaDocente.Turma &&
                        ha.Ano == rowAulaDocente.Ano && ha.Semestre == rowAulaDocente.Semestre);
                    Ly_hor_aula.Row haDestino = hasDestino.First();

                    #endregion

                    #region Desativa a alocação anterior no destino, caso exista

                    SimpleRow rowAnterior = SimpleRow.QueryFirstRow(conn,
                        @"  SELECT * FROM ly_aula_docente ad 
                            INNER JOIN ly_turma t (NOLOCK) ON
                                ad.disciplina = t.disciplina AND
                                ad.turma = t.turma AND
                                ad.ano = t.ano AND
                                ad.semestre = t.semestre AND
                                ad.data_fim = t.dt_fim
                            WHERE
                                ad.turno = ? AND 
                                ad.faculdade = ? AND 
                                ad.dia_semana = ? AND 
                                ad.aula = ? AND 
                                ad.turma = ? AND 
                                ad.ano = ? AND 
                                ad.semestre = ? AND
                                tu.sit_turma = ?",
                        rowAulaDocente.Turno, rowAulaDocente.Faculdade, rowAulaDocente.Dia_semana, rowAulaDocente.Aula,
                        rowAulaDocente.Turma, rowAulaDocente.Ano, rowAulaDocente.Semestre, "Aberta");
                    if (rowAnterior != null)
                    {
                        Ly_aula_docente.Row.Update(conn,
                            Convert.ToDecimal(rowAnterior["num_func"]),
                            Convert.ToString(rowAnterior["turno"]),
                            Convert.ToString(rowAnterior["faculdade"]), Convert.ToDecimal(rowAnterior["dia_semana"]),
                            Convert.ToDecimal(rowAnterior["aula"]), Convert.ToString(rowAnterior["disciplina"]),
                            Convert.ToString(rowAnterior["turma"]), Convert.ToDecimal(rowAnterior["ano"]),
                            Convert.ToDecimal(rowAnterior["semestre"]), Convert.ToDateTime(rowAnterior["data_inicio"]), "data_fim",
                            new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day));
                        RetValue retDesativacaoAnterior = VerificarErro(conn.GetErrors());
                        if (retDesativacaoAnterior != null && !retDesativacaoAnterior.Ok)
                        {
                            conn.Rollback();
                            return new RetValue(false, "", new ErrorList(String.Format("Não foi possível transferir a alocação. ({0}).", retDesativacaoAnterior.Errors.ToString())));
                        }
                    }

                    #endregion

                    #region Insere Ly_hor_aula se necessário

                    Ly_hor_aula.Row haNovo = Ly_hor_aula.Row.Query(conn, rowAulaDocente.Turno,
                        rowAulaDocente.Faculdade, rowAulaDocente.Dia_semana, rowAulaDocente.Aula,
                        rowAulaDocente.Disciplina, rowAulaDocente.Turma, rowAulaDocente.Ano, rowAulaDocente.Semestre);
                    if (haNovo == null)
                    {
                        Ly_hor_aula.Row.Insert(conn, rowAulaDocente.Turno, rowAulaDocente.Faculdade, rowAulaDocente.Dia_semana,
                            rowAulaDocente.Aula, rowAulaDocente.Disciplina, rowAulaDocente.Turma, rowAulaDocente.Ano, rowAulaDocente.Semestre,
                            "dependencia, horaini_aula, horafim_aula, qtde_aula, stamp_atualizacao",
                            haDestino.Dependencia, haDestino.Horaini_aula, haDestino.Horafim_aula, haDestino.Qtde_aula, DateTime.Now);
                        RetValue retInsercaoHA = VerificarErro(conn.GetErrors());
                        if (retInsercaoHA != null && !retInsercaoHA.Ok)
                        {
                            conn.Rollback();
                            return new RetValue(false, "", new ErrorList(String.Format("Não foi possível transferir a alocação. ({0}).", retInsercaoHA.Errors.ToString())));
                        }
                    }

                    #endregion

                    #region Insere Ly_aula_docente no destino, ou reativa caso já exista a chave
                    if (Ly_aula_docente.Row.Query(conn, rowAulaDocente.Num_func, rowAulaDocente.Turno, rowAulaDocente.Faculdade,
                        rowAulaDocente.Dia_semana, rowAulaDocente.Aula, rowAulaDocente.Disciplina, rowAulaDocente.Turma,
                        rowAulaDocente.Ano, rowAulaDocente.Semestre, new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0)) != null)
                    {
                        Ly_aula_docente.Row.Update(conn, rowAulaDocente.Num_func, rowAulaDocente.Turno, rowAulaDocente.Faculdade,
                            rowAulaDocente.Dia_semana, rowAulaDocente.Aula, rowAulaDocente.Disciplina, rowAulaDocente.Turma,
                            rowAulaDocente.Ano, rowAulaDocente.Semestre, new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0),
                            "data_fim", rowAulaDocente.Data_fim);
                    }
                    else
                    {
                        Ly_aula_docente.Row.Insert(conn, rowAulaDocente.Num_func, rowAulaDocente.Turno, rowAulaDocente.Faculdade,
                            rowAulaDocente.Dia_semana, rowAulaDocente.Aula, rowAulaDocente.Disciplina, rowAulaDocente.Turma,
                            rowAulaDocente.Ano, rowAulaDocente.Semestre, new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0),
                            "data_fim", rowAulaDocente.Data_fim);
                    }
                    RetValue retInsercao = VerificarErro(conn.GetErrors());
                    if (retInsercao != null && !retInsercao.Ok)
                    {
                        conn.Rollback();
                        return new RetValue(false, "", new ErrorList(String.Format("Não foi possível transferir a alocação. ({0}).", retInsercao.Errors.ToString())));
                    }

                    dadosDocente = rnDocentes.ObtemDadosDocentePor(Convert.ToDecimal(rowAulaDocente.Num_func));

                    logs.Add(String.Format("  - Matrícula:{0}   Docente:{1}   Dia Semana:{2}    Horário:{3}-{4}",
                               dadosDocente.Matricula, dadosDocente.NomeCompleto, ObterDiaSemana(rowAulaDocente.Dia_semana.Value),
                               haDestino.Horaini_aula.Value.ToString("HH:mm"), haDestino.Horafim_aula.Value.ToString("HH:mm")) + "<br/>");

                    #endregion
                }
                #endregion

                RetValue retDestino = AlterarTurmaComQuadroHorario(conn, dadosTurmaOld, nomeTurmaDestino, dtTurmaDestino, dtHoraAula, dtAulaDocente);
                List<TurmaError> errosDestino = RN.Turma.TransformarErrorList(retDestino);

                if (errosDestino.Count(err => err.TipoErro == "ERRO_GLP" || err.TipoErro == "ERRO_VALIDACAO") > 0)
                {
                    conn.Rollback();
                    return new RetValue(false, "", new ErrorList(errosDestino
                        .Where(err => err.TipoErro == "ERRO_GLP" || err.TipoErro == "ERRO_VALIDACAO")
                        .Select(err => err.Mensagem + " (" + ObterDiaSemana(err.DiaDaSemana) + " " + err.HoraInicio.ToString("HH:mm") + "-" + err.HoraFim.ToString("HH:mm") + ")")
                        .Aggregate((a, b) => a + "<br/>" + b)));
                }

                #endregion
            }
            catch (Exception e)
            {
                conn.Rollback();
                return new RetValue(false, "", new ErrorList("Transferência não realizada: " + e.Message));
            }
            finally
            {
                conn.Close();
            }

            return new RetValue(true, "Processo de transferência de alocações bem sucedido. <br/><br/>" + logs.ToString(), null);
        }

        public class AulaSelecionada
        {
            public String Matricula { get; set; }
            public String NomeDocente { get; set; }
            public decimal NumFunc { get; set; }
            public String Disciplina { get; set; }
            public String DisciplinaDescr { get; set; }
            public decimal Ano { get; set; }
            public decimal Semestre { get; set; }
            public decimal Aula { get; set; }
            public decimal DiaSemana { get; set; }
            public String DiaSemanaDescr { get { return ObterDiaSemana(DiaSemana); } }
            public String Turma { get; set; }
            public String Turno { get; set; }
            public String TurnoDescr { get; set; }
            public String HoraIni { get; set; }
            public String HoraFim { get; set; }
            public String Observacao { get; set; }
            public Boolean IsCarencia { get; set; }
            public int CarenciaOrderID { get; set; }

            public static List<AulaSelecionada> PreencherAulasSelecionadas(List<AulaSelecionada> aulas)
            {
                TConnection connection = Config.CreateConnection();
                connection.Open();
                try
                {
                    List<AulaSelecionada> aulasInexistentes = new List<AulaSelecionada>();
                    List<AulaSelecionada> aulasGLP = new List<AulaSelecionada>();

                    foreach (AulaSelecionada aula in aulas)
                    {
                        if (!aula.ExisteAula(connection))
                        {
                            aulasInexistentes.Add(aula);
                            continue;
                        }
                        if (aula.IsGLP(connection))
                        {
                            aulasGLP.Add(aula);
                            continue;
                        }

                        QueryTable qt = new QueryTable("SELECT d.matricula, p.nome_compl FROM ly_docente d (NOLOCK) INNER JOIN ly_pessoa P ( NOLOCK ) ON Pe.pessoa = d.pessoa where d.num_func = ?");
                        qt.Query(connection, aula.NumFunc);
                        if (qt.Rows.Count > 0)
                        {
                            aula.Matricula = Convert.ToString(qt.Rows[0]["matricula"]);
                            aula.NomeDocente = Convert.ToString(qt.Rows[0]["nome_compl"]);
                        }

                        aula.DisciplinaDescr = Convert.ToString(TCommand.ExecuteScalar(connection, "SELECT nome_compl from ly_disciplina (NOLOCK) where disciplina = ?", aula.Disciplina));
                        aula.TurnoDescr = Convert.ToString(TCommand.ExecuteScalar(connection, "SELECT descricao from ly_turno (NOLOCK) where turno = ?", aula.Turno));
                    }

                    foreach (AulaSelecionada aulaInexistente in aulasInexistentes)
                    {
                        aulas.Remove(aulaInexistente);
                    }
                    foreach (AulaSelecionada aulaGLP in aulasGLP)
                    {
                        aulas.Remove(aulaGLP);
                    }
                }
                catch { }
                finally
                {
                    connection.Close();
                }
                return aulas;
            }

            private Boolean ExisteAula(TConnection connection)
            {
                //<ALTERADO>
                DbObject retorno = TCommand.ExecuteScalar(connection, @"
                    select top 1 1 
                    from LY_AULA_DOCENTE ad inner join LY_TURMA tu (NOLOCK) on 
                        ad.DISCIPLINA = tu.DISCIPLINA and
                        ad.TURMA = tu.TURMA and 
                        ad.ANO = tu.ANO and 
                        ad.SEMESTRE = tu.SEMESTRE and 
                        ad.DATA_FIM = tu.DT_FIM
                    where
                        ad.NUM_FUNC = ? AND 
                        ad.TURNO = ? AND 
                        ad.DIA_SEMANA = ? AND 
                        ad.AULA = ? AND 
                        ad.DISCIPLINA = ? AND 
                        ad.TURMA = ? AND 
                        ad.ANO = ? AND 
                        ad.SEMESTRE = ? AND
                        tu.SIT_TURMA = ?",
                    NumFunc, Turno, DiaSemana, Aula, Disciplina, Turma, Ano, Semestre, "Aberta");
                return !retorno.IsNull;
            }

            private Boolean IsGLP(TConnection connection)
            {
                DbObject retorno = TCommand.ExecuteScalar(connection, @"
                SELECT TOP 1 1
                FROM ly_aula_docente_tipo ad
                WHERE 
                    TIPO_AULA = 'GLP' AND
                    NUM_FUNC = ? AND 
                    TURNO = ? AND
                    DIA_SEMANA = ? AND 
                    AULA = ? AND
                    DISCIPLINA = ? AND
                    TURMA = ? AND
                    ANO = ? AND
                    SEMESTRE = ?",
                    NumFunc, Turno, DiaSemana, Aula, Disciplina, Turma, Ano, Semestre);
                return !retorno.IsNull;
            }

            private Boolean MatriculaIsCarencia()
            {
                return !String.IsNullOrEmpty(Matricula) && (Matricula == "00000000" || Matricula == "99999999");
            }
        }

        #endregion

        public static DataTable ListarPorTurmaUE(string censo, string curso, int serie, int ano, int periodo, string turno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT T.TURMA
                        FROM LY_CURSO C 
                        INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO 
                        INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                        INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO 
                        INNER JOIN dbo.LY_TURNO TU ON TU.TURNO=T.TURNO
                        INNER JOIN dbo.LY_SERIE S ON S.SERIE=T.SERIE AND S.CURSO=T.CURSO
                       WHERE t.FACULDADE = @CENSO                      
                        AND T.ANO = @ANO
                        AND T.SEMESTRE= @PERIODO
                        AND T.CURSO = @CURSO
                        AND T.TURNO = @TURNO
                        AND T.SERIE = @SERIE
                        AND T.SIT_TURMA='Aberta'
                        AND t.OPTATIVAREFORCO = 'N'
                        AND ISNULL(T.ELETIVA,'N') = 'N'
                    ORDER BY T.TURMA";

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@SERIE", serie);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPorTurmaUE(string censo, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT T.TURMA
                        FROM LY_CURSO C 
                        INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO 
                        INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                        INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO 
                        INNER JOIN dbo.LY_TURNO TU ON TU.TURNO=T.TURNO
                        INNER JOIN dbo.LY_SERIE S ON S.SERIE=T.SERIE AND S.CURSO=T.CURSO
                       WHERE t.FACULDADE = @CENSO                      
                        AND T.ANO = @ANO
                        AND T.SEMESTRE= @PERIODO
                        AND T.SIT_TURMA='Aberta'
                    ORDER BY T.TURMA";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarPorTurmaUESemSituacao(string censo, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT T.TURMA
                        FROM LY_CURSO C 
                        INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO 
                        INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                        INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO 
                        INNER JOIN dbo.LY_TURNO TU ON TU.TURNO=T.TURNO
                        INNER JOIN dbo.LY_SERIE S ON S.SERIE=T.SERIE AND S.CURSO=T.CURSO
                       WHERE t.FACULDADE = @CENSO                      
                        AND T.ANO = @ANO
                        AND T.SEMESTRE= @PERIODO                        
                    ORDER BY T.TURMA";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static int RetornaVagasPrincipal(string censo, int ano, int periodo, int serie, string curso, string turno, string turma)
        {
            var contextQuery = new ContextQuery
            (@"  SELECT MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO)
                            - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS vagas
                     FROM   LYCEUM.DBO.LY_TURMA t ( NOLOCK )
                            LEFT JOIN LYCEUM.DBO.ly_matricula mat ( NOLOCK ) ON mat.DISCIPLINA = t.DISCIPLINA
                                                                                AND mat.TURMA = t.TURMA
                                                                                AND mat.ANO = t.ANO
                                                                                AND mat.SEMESTRE = t.SEMESTRE
                                                                                AND mat.SIT_MATRICULA <> 'Cancelado'
                                                                                AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)
                            LEFT JOIN LYCEUM.DBO.TCE_TRANSFERENCIA_DESTINO td ( NOLOCK ) ON t.FACULDADE = td.CENSO
                                                                                  AND t.ANO = td.ANO
                                                                                  AND t.SEMESTRE = td.PERIODO
                                                                                  AND t.TURMA = td.TURMA
                                                                                  AND ID_TRANSFERENCIA IN(SELECT ID_TRANSFERENCIA 
                                                                                                            FROM TCE_TRANSFERENCIA TRANSF 
														                                                    WHERE TRANSF.STATUS = 'Pendente')
                            LEFT JOIN LYCEUM.DBO.TCE_TRANSFERENCIA tr ( NOLOCK ) ON td.ID_TRANSFERENCIA = tr.ID_TRANSFERENCIA
                                                                                  AND tr.[STATUS] = 'Pendente'
                     WHERE  T.turma = @TURMA
                            AND t.FACULDADE = @CENSO
                            AND t.CURSO = @CURSO
                            AND t.SERIE = @SERIE
                            AND t.TURNO = @TURNO
                            AND t.ANO = @ANO
                            AND t.SEMESTRE = @PERIODO
							AND ISNULL(T.ELETIVA, 'N') = 'N'
                     GROUP BY t.TURMA, t.CURRICULO ");

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            return ExecutarFuncao(contextQuery);
        }

        public static int RetornaVagas(int ano, int periodo, string turma)
        {
            RN.Turma rnTurma = new Turma();
            DataContext contexto = null;
            int retorno = 0;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                retorno = rnTurma.ObtemVagasPrincipalLiberadasTurmaPor(contexto, ano, periodo, turma);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
            return retorno;
        }

        public int ObtemVagasPrincipalLiberadasTurmaPor(DataContext ctx, int ano, int periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            contextQuery.Command = @" SELECT MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO)
                            - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS vagas
                     FROM   LYCEUM.DBO.LY_TURMA t ( NOLOCK )
                            LEFT JOIN LYCEUM.DBO.ly_matricula mat ( NOLOCK ) ON mat.DISCIPLINA = t.DISCIPLINA
                                                                                AND mat.TURMA = t.TURMA
                                                                                AND mat.ANO = t.ANO
                                                                                AND mat.SEMESTRE = t.SEMESTRE
                                                                                AND mat.SIT_MATRICULA <> 'Cancelado'
                                                                                AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)    
                            LEFT JOIN LYCEUM.DBO.TCE_TRANSFERENCIA_DESTINO td ( NOLOCK ) ON t.FACULDADE = td.CENSO
                                                                                  AND t.ANO = td.ANO
                                                                                  AND t.SEMESTRE = td.PERIODO
                                                                                  AND t.TURMA = td.TURMA
                                                                                  AND ID_TRANSFERENCIA IN(SELECT ID_TRANSFERENCIA 
                                                                                                            FROM TCE_TRANSFERENCIA TRANSF 
														                                                    WHERE TRANSF.STATUS = 'PENDENTE')
                            LEFT JOIN LYCEUM.DBO.TCE_TRANSFERENCIA tr ( NOLOCK ) ON td.ID_TRANSFERENCIA = tr.ID_TRANSFERENCIA
                                                                                  AND tr.[STATUS] = 'Pendente'
                     WHERE  T.ANO = @ANO
                            AND T.SEMESTRE = @PERIODO
                            AND T.TURMA = @TURMA
							AND ISNULL(T.ELETIVA, 'N') = 'N'
                     GROUP BY t.TURMA, t.CURRICULO ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                retorno = Convert.ToInt32(reader["vagas"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return retorno;
        }

        public int ObtemVagasEletivaLiberadasTurmaPor(DataContext ctx, int ano, int periodo, string turma, int grupo)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            contextQuery.Command = @" SELECT MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO)
                            - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS vagas
                     FROM   LYCEUM.DBO.LY_TURMA t ( NOLOCK )
							INNER JOIN LY_DISCIPLINA d ON ISNULL(t.DISCIPLINA_MULTIPLA, t.DISCIPLINA) = d.DISCIPLINA
                            LEFT JOIN LYCEUM.DBO.ly_matricula mat ( NOLOCK ) ON mat.DISCIPLINA = t.DISCIPLINA
                                                                                AND mat.TURMA = t.TURMA
                                                                                AND mat.ANO = t.ANO
                                                                                AND mat.SEMESTRE = t.SEMESTRE
                                                                                AND mat.SIT_MATRICULA <> 'Cancelado'
                                                                                AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)    
                            LEFT JOIN LYCEUM.DBO.TCE_TRANSFERENCIA_DESTINO td ( NOLOCK ) ON t.FACULDADE = td.CENSO
                                                                                  AND t.ANO = td.ANO
                                                                                  AND t.SEMESTRE = td.PERIODO
                                                                                  AND t.TURMA = td.TURMA
                                                                                  AND ID_TRANSFERENCIA IN(SELECT ID_TRANSFERENCIA 
                                                                                                            FROM TCE_TRANSFERENCIA TRANSF 
														                                                    WHERE TRANSF.STATUS = 'PENDENTE')
                            LEFT JOIN LYCEUM.DBO.TCE_TRANSFERENCIA tr ( NOLOCK ) ON td.ID_TRANSFERENCIA = tr.ID_TRANSFERENCIA
                                                                                  AND tr.[STATUS] = 'Pendente'
                     WHERE  T.ANO = @ANO
                            AND T.SEMESTRE = @PERIODO
                            AND T.TURMA = @TURMA
							AND ISNULL(T.ELETIVA, 'N') = 'S'
							AND D.GRUPO = @GRUPO
                     GROUP BY t.TURMA, t.CURRICULO ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@GRUPO", grupo);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                retorno = Convert.ToInt32(reader["vagas"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return retorno;
        }

        public static ValidacaoDados ValidarEnturmacao(TceConfirmacaoMatricula confirmacaoMatricula, LyAluno aluno)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (confirmacaoMatricula == null)
            {
                return validacaoDados;
            }

            if (aluno.UnidadeEnsino != confirmacaoMatricula.Censo
                || aluno.Curso != confirmacaoMatricula.Curso
                || aluno.Serie != confirmacaoMatricula.Serie
                || aluno.Turno != confirmacaoMatricula.Turno)
            {
                mensagens.Add("A enturmação automática não foi possível, pois os dados da ESCOLARIDADE não são os mesmo dados da confirmação!");
            }

            if (mensagens.Count == 0)
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
                {
                    var contextQuery = new ContextQuery(
                        @"SELECT  1
                            FROM    dbo.LY_MATRICULA
                            WHERE   ALUNO = @ALUNO
                                    AND SIT_MATRICULA = 'Matriculado'");

                    contextQuery.Parameters.Add("@ALUNO", confirmacaoMatricula.Aluno);

                    var obj = ctx.GetReturnValue(contextQuery);

                    if (obj != null)
                    {
                        mensagens.Add("A enturmação automática não foi possível, pois o aluno possui enturmação ativa. Em casos de dúvida, acesse a tela MATRICULA disponível no módulo GESTAO ESCOLAR.");
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

        public static string RetornaCurriculo(decimal ano, decimal semestre, string turno, string curso, string unidade, string turma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT DISTINCT TOP 1
                                CURRICULO
                        FROM    DBO.LY_TURMA
                        WHERE   ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND TURNO = @TURNO 
                                AND CURSO = @CURSO 
                                AND FACULDADE = @UNIDADE 
                                AND TURMA = @TURMA ");
                // AND SERIE = @SERIE 

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@UNIDADE", unidade);
                contextQuery.Parameters.Add("@TURMA", turma);
                //contextQuery.Parameters.Add("@SERIE", serie);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }

        public static decimal VerificaSerie(string turma, string unidadeEns, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT DISTINCT TOP 1
                                SERIE
                        FROM    DBO.LY_TURMA
                        WHERE   ANO = @ANO
                        AND SEMESTRE = @SEMESTRE
                        AND FACULDADE = @FACULDADE
                        AND TURMA = @TURMA");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@FACULDADE", unidadeEns);
                contextQuery.Parameters.Add("@TURMA", turma);

                return ctx.GetReturnValue<decimal>(contextQuery);
            }
        }

        public static string VerificaOptativaReforco(string turma, string unidadeEns, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT DISTINCT TOP 1
                                OPTATIVAREFORCO
                        FROM    DBO.LY_TURMA
                        WHERE   ANO = @ANO
                        AND SEMESTRE = @SEMESTRE
                        AND FACULDADE = @FACULDADE
                        AND TURMA = @TURMA");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@FACULDADE", unidadeEns);
                contextQuery.Parameters.Add("@TURMA", turma);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }

        public static QueryTable ListarTurma(string ano, string periodo, string turno, string faculdade)
        {
            return RN.RNBase.Consultar(@" SELECT DISTINCT TURMA 
                        FROM dbo.LY_TURMA
                        WHERE ANO=?
                        AND SEMESTRE=?
                        AND TURNO=?
                        AND FACULDADE=?  
                        ORDER BY TURMA", ano, periodo, turno, faculdade);
        }

        public static QueryTable ListarTurmaAberta(string ano, string periodo, string turno, string faculdade)
        {
            return RN.RNBase.Consultar(@" SELECT DISTINCT TURMA 
                        FROM dbo.LY_TURMA
                        WHERE ANO=?
                        AND SEMESTRE=?
                        AND TURNO=?
                        AND FACULDADE=?  
                        AND SIT_TURMA = 'Aberta' 
                        AND OPTATIVAREFORCO = 'N' 
                        AND ISNULL(T.ELETIVA,'N') = 'N'
                        ORDER BY TURMA", ano, periodo, turno, faculdade);
        }

        public static bool VerificaSituacaoAtiva(string turma, decimal ano, decimal semestre)
        {
            var retorno = ExecutarFuncao<int?>(
                new ContextQuery(
                    @"SELECT TOP 1
                            1                   
                        FROM    dbo.LY_TURMA t
                        WHERE   t.TURMA = @TURMA
                                AND t.ANO = @ANO
                                AND t.SEMESTRE = @SEMESTRE
                                AND SIT_TURMA = 'Aberta'
                                AND ( DT_FIM IS NULL
                                      OR DT_FIM >= GETDATE()
                                    )",
                     new ContextQueryParameter("@TURMA", turma),
                    new ContextQueryParameter("@ANO", ano),
                    new ContextQueryParameter("@SEMESTRE", semestre)));

            return retorno != null;
        }

        public static LyTurma Carregar(int ano, int periodo, string turma)
        {
            var lyTurma = new LyTurma();

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT DISTINCT
                            ANO, SEMESTRE, FACULDADE, CURSO, CURRICULO, TURNO, SERIE, TURMA
                    FROM    DBO.LY_TURMA
                    WHERE   ANO = @ANO
                            AND SEMESTRE = @SEMESTRE
                            AND TURMA = @TURMA
                            AND SIT_TURMA = 'Aberta' "
                };
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@TURMA", turma);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        lyTurma.Ano = ano;
                        lyTurma.Semestre = periodo;
                        lyTurma.Faculdade = Convert.ToString(reader["FACULDADE"]);
                        lyTurma.Curso = Convert.ToString(reader["CURSO"]);
                        lyTurma.Curriculo = Convert.ToString(reader["CURRICULO"]);
                        lyTurma.Turno = Convert.ToString(reader["TURNO"]);
                        lyTurma.Serie = Convert.ToDecimal(reader["SERIE"]);
                        lyTurma.Turma = turma;
                    }
                }
                return lyTurma;
            }
        }

        public ICollection<Entidades.LyTurma> ObtemDisciplinasTurmaPor(int ano, int periodo, string turma)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ICollection<LyTurma> turmas = new List<LyTurma>();

            try
            {
                turmas = this.ObtemDisciplinasTurmaPor(contexto, ano, periodo, turma);
                return turmas;
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

        public ICollection<Entidades.LyTurma> ObtemDisciplinasTurmaPor(DataContext contexto, int ano, int periodo, string turma)
        {
            ICollection<LyTurma> turmas = new List<LyTurma>();
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT *
                    FROM    DBO.LY_TURMA
                    WHERE   ANO = @ANO
                            AND SEMESTRE = @SEMESTRE
                            AND TURMA = @TURMA ";

            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            turmas = contexto.TryToBindEntities<LyTurma>(contextQuery);

            return turmas;
        }

        public LyTurma CarregaTurmaDependenciaPor(string aluno, int serieReferencia, string disciplinaReferencia)
        {
            var lyTurma = new LyTurma();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@"SELECT  T.ANO ,
                                T.SEMESTRE ,
                                T.FACULDADE ,
                                T.CURSO ,
                                T.CURRICULO ,
                                T.TURNO ,
                                T.SERIE ,
                                T.TURMA
                        FROM    LY_HISTMATRICULA H
                                INNER JOIN dbo.LY_TURMA t ON h.TURMA = t.TURMA
                                                             AND h.DISCIPLINA = t.DISCIPLINA
                                                             AND h.ano = t.ANO
                                                             AND h.SEMESTRE = t.SEMESTRE
                                INNER JOIN TCE_SITUACAO_FINAL_ALUNO s ON s.ALUNO = h.ALUNO
                                                                         AND s.ANO = h.ANO
                                                                         AND s.PERIODO = h.SEMESTRE
                                                                         AND SITUACAO_FINAL = 'Aprovado Com Dep'
                        WHERE   SITUACAO_HIST = 'Rep Nota'
                                AND H.ALUNO = @ALUNO
                                AND H.SERIE = @SERIE_REFERENCIA
                                AND H.DISCIPLINA = @DISCIPLINA_REFERENCIA ")
                };

                contextQuery.Parameters.Add("@ALUNO", aluno);
                contextQuery.Parameters.Add("@SERIE_REFERENCIA", serieReferencia);
                contextQuery.Parameters.Add("@DISCIPLINA_REFERENCIA", disciplinaReferencia);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    lyTurma.Ano = Convert.ToDecimal(reader["ANO"]);
                    lyTurma.Semestre = Convert.ToDecimal(reader["SEMESTRE"]);
                    lyTurma.Faculdade = Convert.ToString(reader["FACULDADE"]);
                    lyTurma.Curso = Convert.ToString(reader["CURSO"]);
                    lyTurma.Curriculo = Convert.ToString(reader["CURRICULO"]);
                    lyTurma.Turno = Convert.ToString(reader["TURNO"]);
                    lyTurma.Serie = Convert.ToDecimal(reader["SERIE"]);
                    lyTurma.Turma = Convert.ToString(reader["TURMA"]);
                }


                return lyTurma;
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

        public static DataTable ListarMacros(string curriculo, string curso, string turno, string turma, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    //Lista os macros cadastrados e as disciplinas relacionadas
                    Command =
                        @" SELECT DISTINCT
                                    M.ID_MACRO_CAMPOS, M.NOME AS NOME_MACRO
                            FROM    LY_TURMA T
                                    INNER JOIN LY_GRADE G ON T.CURRICULO = G.CURRICULO
                                                             AND T.CURSO = G.CURSO
                                                             AND T.TURNO = G.TURNO
                                                             AND T.DISCIPLINA = G.DISCIPLINA   
                                    INNER JOIN TCE_MACRO_CAMPOS M ON G.MACRO = M.ID_MACRO_CAMPOS
                            WHERE   G.CURRICULO = @CURRICULO
                                    AND G.CURSO = @CURSO
                                    AND G.TURNO = @TURNO
                                    AND T.TURMA = @TURMA
                                    AND T.ANO = @ANO
                                    AND T.SEMESTRE = @PERIODO "
                };
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static QueryTable ConsultaOrdemAula(string turma, string ano, string semestre, string diaSemana, string aula)
        {
            return RN.RNBase.Consultar(@"DECLARE @ordem INT

                             SET @ordem = (
                           SELECT DISTINCT ORDEM 
                         FROM ly_hor_oper ho 
                        INNER JOIN ly_turma tu (NOLOCK) ON
                            ho.faculdade = tu.faculdade AND 
                            ho.turno = tu.turno AND    
                            ho.curso = tu.curso AND 
                            ho.curriculo = tu.curriculo AND 
                            ho.serie = tu.serie                
                        WHERE 
                            tu.turma = ? AND 
                            tu.ano =? AND 
                            tu.semestre = ? AND
                            ho.dia_semana = ? AND
                            AULA = ?
			                            ) 

                        SELECT DISTINCT ordem,ho.TURNO,ho.FACULDADE,DIA_SEMANA,AULA,ho.CURSO,ho.CURRICULO,ho.SERIE
                         FROM ly_hor_oper ho 
                        INNER JOIN ly_turma tu (NOLOCK) ON
                            ho.faculdade = tu.faculdade AND 
                            ho.turno = tu.turno AND    
                            ho.curso = tu.curso AND 
                            ho.curriculo = tu.curriculo AND 
                            ho.serie = tu.serie                
                        WHERE 
                            tu.turma = ? AND 
                            tu.ano = ? AND 
                            tu.semestre = ? AND
                            ho.dia_semana = ? 
                            AND ordem = (@ordem + 1)", turma, ano, semestre, diaSemana, aula, turma, ano, semestre, diaSemana);
        }

        private static void PopularLinhaAulaDocenteAux(Ly_aula_docente.Row linhaDados, Ly_aula_docente.Row linhaDestino)
        {
            linhaDestino.Ano = linhaDados.Ano;
            linhaDestino.Aula = linhaDados.Aula;
            linhaDestino.Data_fim = linhaDados.Data_fim;
            linhaDestino.Data_inicio = linhaDados.Data_inicio;
            linhaDestino.Dia_semana = linhaDados.Dia_semana;
            linhaDestino.Disciplina = linhaDados.Disciplina;
            linhaDestino.Faculdade = linhaDados.Faculdade;
            linhaDestino.Num_func = linhaDados.Num_func;
            linhaDestino.Semestre = linhaDados.Semestre;
            linhaDestino.Turma = linhaDados.Turma;
            linhaDestino.Turno = linhaDados.Turno;
        }

        public static DataTable ListarAtendimentosEducacaoEspecial(string turma, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                //Atendimentos são apenas para o curso de educação Especial
                //Codigo de Educacao Especial: 9999.91
                var curso = "9999.91";

                var contextQuery = new ContextQuery
                {
                    //Lista atendimentos relacionados a turma (sala de recurso)
                    Command =
                        @"  SELECT DISTINCT
                                    AD.DISCIPLINA ,
                                    AD.DIA_SEMANA ,
                                    MIN(HA.HORAINI_AULA) AS INICIO ,
                                    MAX(HA.HORAFIM_AULA) AS FIM ,
                                    CASE ( AD.DIA_SEMANA )
                                      WHEN 2
                                      THEN 'SEGUNDA: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                           + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108)  + ' - ' +  AD.DISCIPLINA
                                      WHEN 3
                                      THEN 'TERÇA: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                           + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108) + ' - ' +  AD.DISCIPLINA
                                      WHEN 4
                                      THEN 'QUARTA: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                           + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108)  + ' - ' +  AD.DISCIPLINA
                                      WHEN 5
                                      THEN 'QUINTA: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                           + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108) + ' - ' +  AD.DISCIPLINA
                                      WHEN 6
                                      THEN 'SEXTA: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                           + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108) + ' - ' +  AD.DISCIPLINA
                                      WHEN 7
                                      THEN 'SÁBADO: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                           + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108) + ' - ' +  AD.DISCIPLINA
                                      ELSE NULL
                                    END HORARIO
                            FROM    LY_AULA_DOCENTE AD
                                    INNER JOIN LY_TURMA TU ( NOLOCK ) ON AD.TURMA = TU.TURMA
                                                                         AND AD.DISCIPLINA = TU.DISCIPLINA
                                                                         AND AD.ANO = TU.ANO
                                                                         AND AD.SEMESTRE = TU.SEMESTRE
                                                                         AND AD.DATA_FIM = TU.DT_FIM
                                    INNER JOIN LY_HOR_AULA HA ON HA.TURNO = AD.TURNO
                                                                 AND HA.FACULDADE = AD.FACULDADE
                                                                 AND HA.DIA_SEMANA = AD.DIA_SEMANA
                                                                 AND HA.AULA = AD.AULA
                                                                 AND HA.DISCIPLINA = AD.DISCIPLINA
                                                                 AND HA.TURMA = AD.TURMA
                                                                 AND HA.ANO = AD.ANO
                                                                 AND HA.SEMESTRE = AD.SEMESTRE
                            WHERE   TU.TURMA = @TURMA
                                    AND TU.ANO = @ANO
                                    AND TU.SEMESTRE = @PERIODO
                                    AND TU.CURSO = @CURSO
                            GROUP BY AD.DIA_SEMANA ,
                                    AD.DISCIPLINA
                            ORDER BY AD.DIA_SEMANA ,
                                    INICIO "
                };
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static string RetornaHorarioAtendimento(decimal ano, decimal semestre, string curso, string disciplina, string turma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT DISTINCT TOP 1               
                            CASE ( AD.DIA_SEMANA )
                              WHEN 2
                              THEN 'SEGUNDA: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                   + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108)  + ' - ' +  AD.DISCIPLINA
                              WHEN 3
                              THEN 'TERÇA: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                   + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108)  + ' - ' +  AD.DISCIPLINA
                              WHEN 4
                              THEN 'QUARTA: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                   + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108)  + ' - ' +  AD.DISCIPLINA
                              WHEN 5
                              THEN 'QUINTA: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                   + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108)  + ' - ' +  AD.DISCIPLINA
                              WHEN 6
                              THEN 'SEXTA: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                   + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108)  + ' - ' +  AD.DISCIPLINA
                              WHEN 7
                              THEN 'SÁBADO: ' + CONVERT (VARCHAR(10), MIN(HA.HORAINI_AULA), 108)
                                   + ' / ' + CONVERT (VARCHAR(10), MAX(HA.HORAFIM_AULA), 108)  + ' - ' +  AD.DISCIPLINA
                              ELSE NULL
                            END HORARIO
                    FROM      LY_AULA_DOCENTE AD
                            INNER JOIN LY_TURMA TU ( NOLOCK ) ON AD.TURMA = TU.TURMA
                                                                 AND AD.DISCIPLINA = TU.DISCIPLINA
                                                                 AND AD.ANO = TU.ANO
                                                                 AND AD.SEMESTRE = TU.SEMESTRE
                                                                 AND AD.DATA_FIM = TU.DT_FIM
                            INNER JOIN LY_HOR_AULA HA ON HA.TURNO = AD.TURNO
                                                         AND HA.FACULDADE = AD.FACULDADE
                                                         AND HA.DIA_SEMANA = AD.DIA_SEMANA
                                                         AND HA.AULA = AD.AULA
                                                         AND HA.DISCIPLINA = AD.DISCIPLINA
                                                         AND HA.TURMA = AD.TURMA
                                                         AND HA.ANO = AD.ANO
                                                         AND HA.SEMESTRE = AD.SEMESTRE
                    WHERE   TU.TURMA = @TURMA
                            AND TU.ANO = @ANO
                            AND TU.SEMESTRE = @SEMESTRE
                            AND TU.CURSO = @CURSO
                            AND AD.DISCIPLINA = @DISCIPLINA
                    GROUP BY  AD.DIA_SEMANA, AD.DISCIPLINA ");

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }

        public static int RetornaQtdAlunosAtendidos(decimal ano, decimal semestre, string disciplina, string turma)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  COUNT(DISTINCT M.ALUNO)
                        FROM    LY_MATRICULA M
                        WHERE   M.TURMA = @TURMA
                                AND M.ANO = @ANO
                                AND M.SEMESTRE = @SEMESTRE
                                AND M.DISCIPLINA = @DISCIPLINA
                                AND M.EDUC_ESPECIAL = 'S'
                                AND M.SIT_MATRICULA = @SIT_MATRICULA ");

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);
                contextQuery.Parameters.Add("@SIT_MATRICULA", Matricula.Matriculado);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                return ctx.GetReturnValue<int>(contextQuery);
            }
        }

        public static void InserirTurmaLancamentoVaga(DataContext context, LyTurma turma, int idAgenda, int idConfVaga)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO LY_TURMA
                            ( 
                              DISCIPLINA ,
                              TURMA ,
                              ANO ,
                              SEMESTRE ,
                              FACULDADE ,
                              DEPENDENCIA ,
                              TURNO ,
                              CURSO ,
                              CURRICULO ,
                              SERIE ,
                              AULAS_PREVISTAS ,
                              AULAS_DADAS ,
                              MIN_AULAS ,
                              NUM_ALUNOS ,
                              DT_ULTALT ,
                              DT_INICIO ,
                              DT_FIM ,
                              SIT_TURMA ,
                              UNIDADE_RESPONSAVEL ,
                              ESPECIAL ,
                              UTILIZA_INDICE ,
                              NIVEL_PRESENCA ,
                              EXIBE_SOMENTE_LISTA_SEL ,
                              LANCAMENTO_HISTORICO ,
                              DT_CRIACAO ,
                              PERMITE_CHOQUE_HORARIO ,
                              TIPO_GESTAO ,
                              CLASSIFICACAO,
                              EM_ELABORACAO,
                              ELETIVA
                            )
                            SELECT DISTINCT
                                G.DISCIPLINA ,
                                V.TURMA , 
                                A.ANO ,
                                A.PERIODO ,
                                V.CENSO ,
                                V.SALA ,
                                V.TURNO ,
                                V.CURSO ,
                                V.CURRICULO ,
                                A.SERIE ,
                                C.AULAS_PREVISTAS ,
                                @AULAS_DADAS AS AULAS_DADAS ,
                                @MIN_AULAS AS MIN_AULAS ,
                                V.VAGAS_CONTINUIDADE + V.VAGAS_NOVAS AS NUM_ALUNOS ,
                                @DT_ULTALT AS DT_ULTALT ,
                                @DT_INICIO AS DT_INICIO ,
                                @DT_FIM AS DT_FIM ,
                                @SIT_TURMA AS SIT_TURMA ,
                                V.CENSO AS UNIDADE_RESPONSAVEL ,
                                @ESPECIAL AS ESPECIAL ,
                                @UTILIZA_INDICE AS UTILIZA_INDICE ,
                                @NIVEL_PRESENCA AS NIVEL_PRESENCA ,
                                @EXIBE_SOMENTE_LISTA_SEL AS EXIBE_SOMENTE_LISTA_SEL ,
                                @LANCAMENTO_HISTORICO AS LANCAMENTO_HISTORICO ,
                                @DT_CRIACAO AS DT_CRIACAO ,
                                @PERMITE_CHOQUE_HORARIO AS PERMITE_CHOQUE_HORARIO ,
                                @TIPO_GESTAO AS TIPO_GESTAO ,
                                @CLASSIFICACAO AS CLASSIFICACAO,
                                @EM_ELABORACAO,
                                CASE WHEN  S.OFERTAELETIVA = 'S' THEN D.ELETIVA ELSE 'N' END 
                        FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                INNER JOIN dbo.TCE_CTV_CONF_VAGA V ON A.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN dbo.LY_CURRICULO c ON c.CURSO = V.CURSO
                                                                 AND c.TURNO = V.TURNO
                                                                 AND c.ANO_INI = a.ANO
                                                                 AND c.SEM_INI = a.PERIODO
                                                                 AND ( c.DT_EXTINCAO IS NULL
                                                                       OR c.DT_EXTINCAO > GETDATE()
                                                                     )
                                INNER JOIN LY_GRADE G ON g.CURSO = V.CURSO
                                                         AND g.SERIE_IDEAL = A.SERIE
                                                         AND G.CURRICULO = V.CURRICULO
                                                         AND g.TURNO = V.TURNO
                                INNER JOIN LY_DISCIPLINA D ON D.DISCIPLINA = G.DISCIPLINA
                                INNER JOIN LY_SERIE S on V.CURRICULO = S.CURRICULO
														AND V.TURNO = S.TURNO
														AND V.CURSO = S.CURSO
														AND A.SERIE = S.SERIE
                                WHERE   V.ID_CONF_VAGA = @ID_CONF_VAGA
                                AND A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA  
                                AND C.CURRICULO = (SELECT  TOP 1 CURRICULO
														FROM    LY_CURRICULO C2
														WHERE c2.CURSO = V.CURSO
                                                                 AND c2.TURNO = V.TURNO
                                                                 AND c2.ANO_INI = a.ANO
                                                                 AND c2.SEM_INI = a.PERIODO
                                                                 AND ( c2.DT_EXTINCAO IS NULL
                                                                       OR c2.DT_EXTINCAO > GETDATE()
                                                                     ))
                                AND OBRIGATORIA='S'"
            };

            contextQuery.Parameters.Add("@AULAS_DADAS", turma.AulasDadas);
            contextQuery.Parameters.Add("@MIN_AULAS", turma.MinAulas);
            contextQuery.Parameters.Add("@DT_ULTALT", turma.DtUltalt);
            contextQuery.Parameters.Add("@DT_INICIO", turma.DtInicio);
            contextQuery.Parameters.Add("@DT_FIM", turma.DtFim);
            contextQuery.Parameters.Add("@SIT_TURMA", turma.SitTurma);
            contextQuery.Parameters.Add("@ESPECIAL", turma.Especial);
            contextQuery.Parameters.Add("@UTILIZA_INDICE", turma.UtilizaIndice);
            contextQuery.Parameters.Add("@NIVEL_PRESENCA", turma.NivelPresenca);
            contextQuery.Parameters.Add("@EXIBE_SOMENTE_LISTA_SEL", turma.ExibeSomenteListaSel);
            contextQuery.Parameters.Add("@LANCAMENTO_HISTORICO", turma.LancamentoHistorico);
            contextQuery.Parameters.Add("@DT_CRIACAO", turma.DtCriacao);
            contextQuery.Parameters.Add("@PERMITE_CHOQUE_HORARIO", turma.PermiteChoqueHorario);
            contextQuery.Parameters.Add("@TIPO_GESTAO", turma.TipoGestao);
            contextQuery.Parameters.Add("@CLASSIFICACAO", turma.Classificacao);
            contextQuery.Parameters.Add("@ID_CONF_VAGA", idConfVaga);
            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);
            contextQuery.Parameters.Add("@EM_ELABORACAO", turma.EmElaboracao);

            context.GetReturnValue(contextQuery);
        }

        public static void InserirTurmaLancamentoVagaEletiva(DataContext context, LyTurma turma, int idAgenda, int idConfVaga, string turmaEletiva, string curriculo, string salaAlternativa)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO LY_TURMA
                            ( 
                              DISCIPLINA ,
                              TURMA ,
                              ANO ,
                              SEMESTRE ,
                              FACULDADE ,
                              DEPENDENCIA ,
                              TURNO ,
                              CURSO ,
                              CURRICULO ,
                              SERIE ,
                              AULAS_PREVISTAS ,
                              AULAS_DADAS ,
                              MIN_AULAS ,
                              NUM_ALUNOS ,
                              DT_ULTALT ,
                              DT_INICIO ,
                              DT_FIM ,
                              SIT_TURMA ,
                              UNIDADE_RESPONSAVEL ,
                              ESPECIAL ,
                              UTILIZA_INDICE ,
                              NIVEL_PRESENCA ,
                              EXIBE_SOMENTE_LISTA_SEL ,
                              LANCAMENTO_HISTORICO ,
                              DT_CRIACAO ,
                              PERMITE_CHOQUE_HORARIO ,
                              TIPO_GESTAO ,
                              CLASSIFICACAO,
                              EM_ELABORACAO,
                              ELETIVA,
                              TURMAREFERENCIA
                            )
                            SELECT DISTINCT
                                G.DISCIPLINA ,
                                @TURMAELETIVA , 
                                A.ANO ,
                                A.PERIODO ,
                                V.CENSO ,
                                @SALA ,
                                V.TURNO ,                                
                                @CURSOELETIVA ,
                                @CURRICULO ,
                                A.SERIE ,
                                C.AULAS_PREVISTAS ,
                                @AULAS_DADAS AS AULAS_DADAS ,
                                @MIN_AULAS AS MIN_AULAS ,
                                V.VAGAS_CONTINUIDADE + V.VAGAS_NOVAS AS NUM_ALUNOS ,
                                @DT_ULTALT AS DT_ULTALT ,
                                @DT_INICIO AS DT_INICIO ,
                                @DT_FIM AS DT_FIM ,
                                @SIT_TURMA AS SIT_TURMA ,
                                V.CENSO AS UNIDADE_RESPONSAVEL ,
                                @ESPECIAL AS ESPECIAL ,
                                @UTILIZA_INDICE AS UTILIZA_INDICE ,
                                @NIVEL_PRESENCA AS NIVEL_PRESENCA ,
                                @EXIBE_SOMENTE_LISTA_SEL AS EXIBE_SOMENTE_LISTA_SEL ,
                                @LANCAMENTO_HISTORICO AS LANCAMENTO_HISTORICO ,
                                @DT_CRIACAO AS DT_CRIACAO ,
                                @PERMITE_CHOQUE_HORARIO AS PERMITE_CHOQUE_HORARIO ,
                                @TIPO_GESTAO AS TIPO_GESTAO ,
                                @CLASSIFICACAO AS CLASSIFICACAO,
                                @EM_ELABORACAO,
                                'S',
                                V.TURMA
                        FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                INNER JOIN dbo.TCE_CTV_CONF_VAGA V ON A.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                                INNER JOIN dbo.LY_CURRICULO c ON c.CURSO = @CURSOELETIVA
                                                                 AND c.TURNO = V.TURNO
                                                                 AND c.ANO_INI = a.ANO
                                                                 AND c.SEM_INI = a.PERIODO
                                                                 AND ( c.DT_EXTINCAO IS NULL
                                                                       OR c.DT_EXTINCAO > GETDATE()
                                                                     )
                                INNER JOIN LY_GRADE G ON g.CURSO = C.CURSO
                                                         AND g.SERIE_IDEAL = A.SERIE
                                                         AND G.CURRICULO = C.CURRICULO
                                                         AND g.TURNO = V.TURNO
                                INNER JOIN LY_DISCIPLINA D ON D.DISCIPLINA = G.DISCIPLINA
                                WHERE   V.ID_CONF_VAGA = @ID_CONF_VAGA
                                    AND A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA  
                                    AND C.CURRICULO = @CURRICULO
                                    AND OBRIGATORIA= 'S' "
            };

            contextQuery.Parameters.Add("@AULAS_DADAS", turma.AulasDadas);
            contextQuery.Parameters.Add("@MIN_AULAS", turma.MinAulas);
            contextQuery.Parameters.Add("@DT_ULTALT", turma.DtUltalt);
            contextQuery.Parameters.Add("@DT_INICIO", turma.DtInicio);
            contextQuery.Parameters.Add("@DT_FIM", turma.DtFim);
            contextQuery.Parameters.Add("@SIT_TURMA", turma.SitTurma);
            contextQuery.Parameters.Add("@ESPECIAL", turma.Especial);
            contextQuery.Parameters.Add("@UTILIZA_INDICE", turma.UtilizaIndice);
            contextQuery.Parameters.Add("@NIVEL_PRESENCA", turma.NivelPresenca);
            contextQuery.Parameters.Add("@EXIBE_SOMENTE_LISTA_SEL", turma.ExibeSomenteListaSel);
            contextQuery.Parameters.Add("@LANCAMENTO_HISTORICO", turma.LancamentoHistorico);
            contextQuery.Parameters.Add("@DT_CRIACAO", turma.DtCriacao);
            contextQuery.Parameters.Add("@PERMITE_CHOQUE_HORARIO", turma.PermiteChoqueHorario);
            contextQuery.Parameters.Add("@TIPO_GESTAO", turma.TipoGestao);
            contextQuery.Parameters.Add("@CLASSIFICACAO", turma.Classificacao);
            contextQuery.Parameters.Add("@ID_CONF_VAGA", idConfVaga);
            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);
            contextQuery.Parameters.Add("@EM_ELABORACAO", turma.EmElaboracao);
            contextQuery.Parameters.Add("@CURRICULO", curriculo);
            contextQuery.Parameters.Add("@TURMAELETIVA", turmaEletiva);
            contextQuery.Parameters.Add("@SALA", salaAlternativa);
            contextQuery.Parameters.Add("@CURSOELETIVA", "9999.80");

            context.GetReturnValue(contextQuery);
        }

        public static void InserirGradeSerieLancamentoVaga(DataContext context, LyTurma turma, int idAgenda, int idConfVaga, string matricula)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO dbo.LY_GRADE_SERIE
                                    ( CURSO ,
                                      TURNO ,
                                      CURRICULO ,
                                      SERIE ,
                                      ANO ,
                                      SEMESTRE ,
                                      GRADE ,
                                      UNIDADE_RESPONSAVEL ,
                                      CAPACIDADE ,
                                      DEPENDENCIA ,
                                      DT_INICIO ,
                                      DT_FIM ,
                                      FACULDADE ,
                                      MATRICULA ,
                                      FECHAMENTO_MANUAL
                                    )
                                    SELECT DISTINCT
                                            V.CURSO ,
                                            V.TURNO ,
                                            V.CURRICULO ,
                                            A.SERIE ,
                                            A.ANO ,
                                            A.PERIODO ,
                                            V.TURMA ,
                                            V.CENSO ,
                                            V.VAGAS_CONTINUIDADE + V.VAGAS_NOVAS AS NUM_ALUNOS ,
                                            V.SALA ,
                                            @DT_INICIO AS DT_INICIO ,
                                            @DT_FIM AS DT_FIM ,
                                            V.CENSO AS UNIDADE_RESPONSAVEL ,
                                            @MATRICULA ,
                                            'N'
                                    FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                            INNER JOIN dbo.TCE_CTV_CONF_VAGA V ON A.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                                    WHERE   V.ID_CONF_VAGA = @ID_CONF_VAGA
                                            AND A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA  "
            };

            contextQuery.Parameters.Add("@DT_INICIO", turma.DtInicio);
            contextQuery.Parameters.Add("@DT_FIM", turma.DtFim);
            contextQuery.Parameters.Add("@ID_CONF_VAGA", idConfVaga);
            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);
            contextQuery.Parameters.Add("@MATRICULA", matricula);

            context.GetReturnValue(contextQuery);
        }

        public static void InserirGradeSerieLancamentoVagaEletiva(DataContext context, LyTurma turma, int idAgenda, int idConfVaga, string matricula, string turmaEletiva, string curriculo, string salaAlternativa)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO dbo.LY_GRADE_SERIE
                                    ( CURSO ,
                                      TURNO ,
                                      CURRICULO ,
                                      SERIE ,
                                      ANO ,
                                      SEMESTRE ,
                                      GRADE ,
                                      UNIDADE_RESPONSAVEL ,
                                      CAPACIDADE ,
                                      DEPENDENCIA ,
                                      DT_INICIO ,
                                      DT_FIM ,
                                      FACULDADE ,
                                      MATRICULA ,
                                      FECHAMENTO_MANUAL
                                    )
                                    SELECT DISTINCT
                                            @CURSOELETIVA ,
                                            V.TURNO ,
                                            @CURRICULO ,
                                            A.SERIE ,
                                            A.ANO ,
                                            A.PERIODO ,
                                            @TURMAELETIVA ,
                                            V.CENSO ,
                                            V.VAGAS_CONTINUIDADE + V.VAGAS_NOVAS AS NUM_ALUNOS ,
                                            @SALA ,
                                            @DT_INICIO AS DT_INICIO ,
                                            @DT_FIM AS DT_FIM ,
                                            V.CENSO AS UNIDADE_RESPONSAVEL ,
                                            @MATRICULA ,
                                            'N'
                                    FROM    dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                            INNER JOIN dbo.TCE_CTV_CONF_VAGA V ON A.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                                    WHERE   V.ID_CONF_VAGA = @ID_CONF_VAGA
                                            AND A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA  "
            };

            contextQuery.Parameters.Add("@DT_INICIO", turma.DtInicio);
            contextQuery.Parameters.Add("@DT_FIM", turma.DtFim);
            contextQuery.Parameters.Add("@ID_CONF_VAGA", idConfVaga);
            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);
            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@CURRICULO", curriculo);
            contextQuery.Parameters.Add("@TURMAELETIVA", turmaEletiva);
            contextQuery.Parameters.Add("@SALA", salaAlternativa);
            contextQuery.Parameters.Add("@CURSOELETIVA", "9999.80");

            context.GetReturnValue(contextQuery);
        }

        public static void InserirGradeTurmaLancamentoVaga(DataContext context, int idAgenda, int idConfVaga)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO DBO.LY_GRADE_TURMA
                                    ( DISCIPLINA ,
                                      TURMA ,
                                      ANO ,
                                      SEMESTRE ,
                                      GRADE_ID
                                    )
                                    SELECT DISTINCT
                                            G.DISCIPLINA ,
                                            V.TURMA ,
                                            A.ANO ,
                                            A.PERIODO ,
                                            GS.GRADE_ID
                                    FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                            INNER JOIN DBO.TCE_CTV_CONF_VAGA V ON A.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                                            INNER JOIN LY_GRADE G ON G.CURSO = V.CURSO
                                                                     AND G.SERIE_IDEAL = A.SERIE
                                                                     AND G.CURRICULO = V.CURRICULO
                                                                     AND G.TURNO = V.TURNO
                                            INNER JOIN LY_DISCIPLINA D ON D.DISCIPLINA = G.DISCIPLINA
                                            INNER JOIN DBO.LY_GRADE_SERIE GS ON GS.CURSO = V.CURSO
                                                                                AND GS.SERIE = A.SERIE
                                                                                AND GS.CURRICULO = V.CURRICULO
                                                                                AND GS.TURNO = V.TURNO
                                                                                AND GS.GRADE = V.TURMA
                                    WHERE   V.ID_CONF_VAGA = @ID_CONF_VAGA
                                            AND A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA  
                                            AND OBRIGATORIA='S' "
            };

            contextQuery.Parameters.Add("@ID_CONF_VAGA", idConfVaga);
            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);

            context.GetReturnValue(contextQuery);
        }

        public static void InserirGradeTurmaLancamentoVagaEletiva(DataContext context, int idAgenda, int idConfVaga, string turmaEletiva, string curriculo)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT  INTO DBO.LY_GRADE_TURMA
                                    ( DISCIPLINA ,
                                      TURMA ,
                                      ANO ,
                                      SEMESTRE ,
                                      GRADE_ID
                                    )
                                    SELECT DISTINCT
                                            G.DISCIPLINA ,
                                            @TURMAELETIVA ,
                                            A.ANO ,
                                            A.PERIODO ,
                                            GS.GRADE_ID
                                    FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                            INNER JOIN DBO.TCE_CTV_CONF_VAGA V ON A.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                                            INNER JOIN LY_GRADE G ON G.CURSO = @CURSOELETIVA
                                                                     AND G.SERIE_IDEAL = A.SERIE
                                                                     AND G.CURRICULO = @CURRICULO
                                                                     AND G.TURNO = V.TURNO
                                            INNER JOIN LY_DISCIPLINA D ON D.DISCIPLINA = G.DISCIPLINA
                                            INNER JOIN DBO.LY_GRADE_SERIE GS ON GS.CURSO = G.CURSO
                                                                                AND GS.SERIE = A.SERIE
                                                                                AND GS.CURRICULO = G.CURRICULO
                                                                                AND GS.TURNO = V.TURNO
                                                                                AND GS.GRADE = @TURMAELETIVA
                                    WHERE   V.ID_CONF_VAGA = @ID_CONF_VAGA
                                            AND A.ID_AGENDA_CONF_TURNO_VAGA = @ID_AGENDA_CONF_TURNO_VAGA  
                                            AND OBRIGATORIA='S' "
            };

            contextQuery.Parameters.Add("@ID_CONF_VAGA", idConfVaga);
            contextQuery.Parameters.Add("@ID_AGENDA_CONF_TURNO_VAGA", idAgenda);
            contextQuery.Parameters.Add("@CURRICULO", curriculo);
            contextQuery.Parameters.Add("@TURMAELETIVA", turmaEletiva);
            contextQuery.Parameters.Add("@CURSOELETIVA", "9999.80");

            context.GetReturnValue(contextQuery);
        }

        public void LimpaMultiplaEletiva(DataContext contexto, string censo, int ano, int periodo, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE T
                                    SET DISCIPLINA_MULTIPLA = NULL
                                    FROM DBO.LY_TURMA T
	                                    INNER JOIN LY_TURMA TR ON T.TURMAREFERENCIA = TR.TURMA 
							                                    AND T.ANO = TR.ANO 
							                                    AND T.SEMESTRE = TR.SEMESTRE
                                    WHERE ISNULL(T.ELETIVA,'N') = 'S'
	                                    AND T.ANO = @ANO
	                                    AND T.SEMESTRE = @SEMESTRE
	                                    AND TR.CURSO = @CURSO
	                                    AND TR.SERIE = @SERIE
	                                    AND T.FACULDADE = @CENSO ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

            contexto.ApplyModifications(contextQuery);
        }

        public DataTable ListaTurmaEletivaAbertaComVagaPor(string censo, int grupo, int ano, int periodo, string curso, int serie, string turno, string itinerario, string tipo, string modalidade)
        {
            Curso rnCurso = new Curso();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @"                    	
					SELECT '' AS 'TURMA','Selecione' AS DISPLINATURMA,@GRUPO AS GRUPO,0 AS 'MAXIMO_ALUNOS' ,0 AS 'ALUNOS_MATRICULADOS',0 AS 'ALUNOS_RESERVADOS',0 AS 'VAGAS'
                    UNION ALL
                    SELECT t.TURMA,  	
							D.NOME + ' - ' + T.TURMA AS DISPLINATURMA,
							D.GRUPO,
                            MAX(t.NUM_ALUNOS) AS 'MAXIMO_ALUNOS',
                            COUNT(DISTINCT mat.ALUNO) AS 'ALUNOS_MATRICULADOS',
                            COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'ALUNOS_RESERVADOS',
                            MAX(t.NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'VAGAS'
                    FROM    dbo.LY_TURMA t
							INNER JOIN LY_DISCIPLINA d ON ISNULL(t.DISCIPLINA_MULTIPLA, t.DISCIPLINA) = d.DISCIPLINA
                            INNER JOIN LY_CURSO C ON C.CURSO = T.CURSO
                            LEFT JOIN ly_matricula mat ON mat.DISCIPLINA = t.DISCIPLINA
                                                          AND mat.TURMA = t.TURMA
                                                          AND mat.ANO = t.ANO
                                                          AND mat.SEMESTRE = t.SEMESTRE
                                                          AND mat.SIT_MATRICULA <> 'Cancelado'                                                          
                                                          AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)
                            LEFT JOIN dbo.TCE_TRANSFERENCIA_DESTINO td ON t.FACULDADE = td.CENSO
                                                                          AND t.ANO = td.ANO
                                                                          AND t.SEMESTRE = td.PERIODO
                                                                          AND t.TURMA = td.TURMA
                                                                            AND ID_TRANSFERENCIA IN(SELECT ID_TRANSFERENCIA 
                                                                                                            FROM TCE_TRANSFERENCIA TRANSF 
														                                                    WHERE TRANSF.STATUS = 'PENDENTE')
                            LEFT JOIN dbo.TCE_TRANSFERENCIA tra ON td.ID_TRANSFERENCIA = tra.ID_TRANSFERENCIA
                                                                   AND tra.[STATUS] = 'Pendente'
                   WHERE T.SIT_TURMA = 'Aberta'
						AND ISNULL(T.ELETIVA,'N') = 'S'
						AND D.GRUPO = @GRUPO
						AND T.ANO = @ANO
						AND T.SEMESTRE = @SEMESTRE						
						AND T.SERIE = @SERIE
						AND T.TURNO = @TURNO
						AND T.TURNO = @TURNO 
						AND T.FACULDADE =  @CENSO";

                //Verifica se o curso atual é itineario
                if (rnCurso.EhItinerarioFormativoTrihaPor(curso))
                {
                    //Se o curso for de itinerario pode trocar para outros itinerarios da mesma modalidade tipo
                    contextQuery.Command += string.Format(" AND c.ITINERARIOFORMATIVO IS NOT NULL AND c.ITINERARIOFORMATIVO = 'S' AND c.TRILHAAPRENDIZAGEMID IS NOT NULL AND c.MODALIDADE = '{0}' AND c.TIPO = {1} and c.TRILHAAPRENDIZAGEMID <> 31", modalidade, tipo);
                }
                else
                {
                    contextQuery.Command += " AND C.CURSO = @CURSO ";
                }

                contextQuery.Command += @"
                    GROUP BY t.TURMA, d.NOME, t.TURMAREFERENCIA, d.GRUPO
                    HAVING  MAX(t.NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) > 0
					UNION ALL
                    SELECT t.TURMA,  	
							D.NOME + ' - ' + T.TURMA AS DISPLINATURMA,
							D.GRUPO,
                            MAX(t.NUM_ALUNOS) AS 'MAXIMO_ALUNOS',
                            COUNT(DISTINCT mat.ALUNO) AS 'ALUNOS_MATRICULADOS',
                            COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'ALUNOS_RESERVADOS',
                            MAX(t.NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS 'VAGAS'
                    FROM    dbo.LY_TURMA t
							INNER JOIN LY_DISCIPLINA d ON ISNULL(t.DISCIPLINA_MULTIPLA, t.DISCIPLINA) = d.DISCIPLINA
							INNER JOIN LY_TURMA TR ON T.TURMAREFERENCIA = TR.TURMA AND T.ANO = TR.ANO AND T.SEMESTRE = TR.SEMESTRE
							INNER JOIN LY_CURSO C ON C.CURSO = TR.CURSO
                            LEFT JOIN ly_matricula mat ON mat.DISCIPLINA = t.DISCIPLINA
                                                          AND mat.TURMA = t.TURMA
                                                          AND mat.ANO = t.ANO
                                                          AND mat.SEMESTRE = t.SEMESTRE
                                                          AND mat.SIT_MATRICULA <> 'Cancelado'                                                          
                                                          AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)
                            LEFT JOIN dbo.TCE_TRANSFERENCIA_DESTINO td ON t.FACULDADE = td.CENSO
                                                                          AND t.ANO = td.ANO
                                                                          AND t.SEMESTRE = td.PERIODO
                                                                          AND t.TURMA = td.TURMA
                                                                            AND ID_TRANSFERENCIA IN(SELECT ID_TRANSFERENCIA 
                                                                                                            FROM TCE_TRANSFERENCIA TRANSF 
														                                                    WHERE TRANSF.STATUS = 'PENDENTE')
                            LEFT JOIN dbo.TCE_TRANSFERENCIA tra ON td.ID_TRANSFERENCIA = tra.ID_TRANSFERENCIA
                                                                   AND tra.[STATUS] = 'Pendente'
                   WHERE T.SIT_TURMA = 'Aberta'
						AND ISNULL(T.ELETIVA,'N') = 'S'
						AND D.GRUPO = @GRUPO
						AND T.ANO = @ANO
						AND T.SEMESTRE = @SEMESTRE						
						AND TR.SERIE = @SERIE
						AND TR.TURNO = @TURNO
						AND T.TURNO = @TURNO 
						AND T.FACULDADE = @CENSO

                ";

                //Verifica se o curso atual é itineario
                if (rnCurso.EhItinerarioFormativoTrihaPor(curso))
                {
                    //Se o curso for de itinerario pode trocar para outros itinerarios da mesma modalidade tipo
                    contextQuery.Command += string.Format(" AND c.ITINERARIOFORMATIVO IS NOT NULL AND c.ITINERARIOFORMATIVO = 'S' AND c.TRILHAAPRENDIZAGEMID IS NOT NULL AND c.MODALIDADE = '{0}' AND c.TIPO = {1} and c.TRILHAAPRENDIZAGEMID <> 31", modalidade, tipo);
                }
                else
                {
                    contextQuery.Command += " AND C.CURSO = @CURSO ";
                }

                contextQuery.Command += @"
                    GROUP BY t.TURMA, d.NOME, t.TURMAREFERENCIA, d.GRUPO
                    HAVING  MAX(t.NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) - COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) > 0
                    ORDER BY TURMA ASC  
                    ";

                contextQuery.Parameters.Add("@GRUPO", SqlDbType.Int, grupo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

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

        public DataTable ListaTurmaAbertaComVagaOptativaReforcoPor(int ano, int periodo, string censo, string turno, string aluno)
        {
            DataTable turmasOptativaReforco = null;

            if (ano > 0 && periodo >= 0 && !string.IsNullOrEmpty(censo) && !string.IsNullOrEmpty(turno) && !string.IsNullOrEmpty(aluno))
            {
                try
                {
                    var contextQuery = new ContextQuery
                     {
                         Command =
                             @" SELECT  DISTINCT
                                T.TURMA
                        FROM    DBO.LY_TURMA T
                                INNER JOIN DBO.TCE_CONFIRMACAO_MATRICULA CM ON CM.ALUNO = @ALUNO
                                                                               AND CM.ANO = T.ANO
                                                                               AND CM.PERIODO = T.SEMESTRE
                                LEFT JOIN LY_MATRICULA MAT ON MAT.DISCIPLINA = T.DISCIPLINA
                                                              AND MAT.TURMA = T.TURMA
                                                              AND MAT.ANO = T.ANO
                                                              AND MAT.SEMESTRE = T.SEMESTRE
                                                              AND MAT.SIT_MATRICULA <> 'CANCELADO'
                                                              AND ( MAT.DEPENDENCIA <> 'S'
                                                                    OR MAT.DEPENDENCIA IS NULL
                                                                  )
                                LEFT JOIN DBO.TCE_TRANSFERENCIA_DESTINO TD ON T.FACULDADE = TD.CENSO
                                                                              AND T.ANO = TD.ANO
                                                                              AND T.SEMESTRE = TD.PERIODO
                                                                              AND T.TURMA = TD.TURMA
                                                                              AND ID_TRANSFERENCIA IN (
                                                                              SELECT  ID_TRANSFERENCIA
                                                                              FROM    TCE_TRANSFERENCIA TRANSF
                                                                              WHERE   TRANSF.STATUS = 'PENDENTE' )
                                LEFT JOIN DBO.TCE_TRANSFERENCIA TR ON TD.ID_TRANSFERENCIA = TR.ID_TRANSFERENCIA
                                                                      AND TR.[STATUS] = 'PENDENTE'
                        WHERE   T.ANO = @ANO
                                AND T.SEMESTRE = @PERIODO
                                AND T.FACULDADE = @CENSO
                                AND T.TURNO = @TURNO
                                AND T.SIT_TURMA = 'ABERTA'
                                AND CM.STATUS = 'CONFIRMADO'
                                AND ( T.OPTATIVAREFORCO = 'S'
                                      OR ( T.OPTATIVAREFORCO = 'R'
                                           AND CM.ENSINO_RELIGIOSO = '1'
                                         )
                                      OR ( T.OPTATIVAREFORCO = 'L'
                                           AND CM.LINGUA_ESTRANGEIRA_FACULTATIVA = '1'
                                         )
                                    )
                                AND NOT EXISTS ( SELECT TOP 1
                                                        1
                                                 FROM   DBO.LY_MATRICULA M
                                                        INNER JOIN LY_TURMA T2 ON M.TURMA = T2.TURMA
                                                                                 AND M.DISCIPLINA = T2.DISCIPLINA
                                                                                 AND M.ANO = T2.ANO
                                                                                 AND M.SEMESTRE = T2.SEMESTRE
                                                 WHERE  M.SIT_MATRICULA = 'MATRICULADO'
                                                        AND M.ALUNO = CM.ALUNO
                                                        AND t2.ANO = T.ANO
                                                        AND t2.SEMESTRE = T.SEMESTRE
                                                        AND t2.TURMA = T.TURMA
                                                        AND NOT ( T.OPTATIVAREFORCO = 'N' ) )
                        GROUP BY T.TURMA
                        HAVING  MAX(NUM_ALUNOS) - COUNT(DISTINCT MAT.ALUNO)
                                - COUNT(DISTINCT TD.ID_TRANSFERENCIA_DESTINO) > 0
                        ORDER BY T.TURMA ASC "
                     };

                    contextQuery.Parameters.Add("@ALUNO", aluno);
                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@TURNO", turno);
                    contextQuery.Parameters.Add("@CENSO", censo);

                    turmasOptativaReforco = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return turmasOptativaReforco;
        }

        public DataTable ListaTurmaParaTransferenciaAbertaComVagaOptativaReforcoPor(int ano, int periodo, string censo, string turmaOrigem, string aluno)
        {
            DataTable turmasOptativaReforco = null;

            if (ano > 0 && periodo >= 0 && !string.IsNullOrEmpty(censo) && !string.IsNullOrEmpty(turmaOrigem) && !string.IsNullOrEmpty(aluno))
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command =
                            @" SELECT  DISTINCT
                                        T.TURMA
                                FROM    DBO.LY_TURMA T
                                        LEFT JOIN LY_MATRICULA MAT ON MAT.DISCIPLINA = T.DISCIPLINA
                                                                      AND MAT.TURMA = T.TURMA
                                                                      AND MAT.ANO = T.ANO
                                                                      AND MAT.SEMESTRE = T.SEMESTRE
                                                                      AND MAT.SIT_MATRICULA <> 'CANCELADO'
                                                                      AND ( MAT.DEPENDENCIA <> 'S'
                                                                            OR MAT.DEPENDENCIA IS NULL
                                                                          )
                                        LEFT JOIN DBO.TCE_TRANSFERENCIA_DESTINO TD ON T.FACULDADE = TD.CENSO
                                                                                      AND T.ANO = TD.ANO
                                                                                      AND T.SEMESTRE = TD.PERIODO
                                                                                      AND T.TURMA = TD.TURMA
                                                                                      AND ID_TRANSFERENCIA IN (
                                                                                      SELECT  ID_TRANSFERENCIA
                                                                                      FROM    TCE_TRANSFERENCIA TRANSF
                                                                                      WHERE   TRANSF.STATUS = 'PENDENTE' )
                                        LEFT JOIN DBO.TCE_TRANSFERENCIA TR ON TD.ID_TRANSFERENCIA = TR.ID_TRANSFERENCIA
                                                                              AND TR.[STATUS] = 'PENDENTE'
                                WHERE   T.ANO = @ANO
                                        AND T.SEMESTRE = @PERIODO
                                        AND T.FACULDADE = @CENSO
                                        AND T.SIT_TURMA = 'ABERTA'
                                        AND NOT EXISTS ( SELECT TOP 1
                                                                1
                                                         FROM   DBO.LY_MATRICULA M
                                                                INNER JOIN LY_TURMA T2 ON M.TURMA = T.TURMA
                                                                                          AND M.DISCIPLINA = T2.DISCIPLINA
                                                                                          AND M.ANO = T2.ANO
                                                                                          AND M.SEMESTRE = T2.SEMESTRE
                                                         WHERE  M.SIT_MATRICULA = 'MATRICULADO'
                                                                AND M.ALUNO = @ALUNO
                                                                AND t2.ANO = T.ANO
                                                                AND t2.SEMESTRE = T.SEMESTRE
                                                                AND t2.TURMA = T.TURMA
                                                                AND NOT ( T.OPTATIVAREFORCO = 'N' ) )
                                        AND T.OPTATIVAREFORCO = ( SELECT  DISTINCT
                                                                            OPTATIVAREFORCO
                                                                  FROM      DBO.LY_TURMA TOR
                                                                  WHERE     T.ANO = TOR.ANO
                                                                            AND T.SEMESTRE = TOR.SEMESTRE
                                                                            AND TOR.TURMA = @TURMAORIGEM
                                                                )
                                GROUP BY T.TURMA
                                HAVING  MAX(t.NUM_ALUNOS) - COUNT(DISTINCT MAT.ALUNO)
                                        - COUNT(DISTINCT TD.ID_TRANSFERENCIA_DESTINO) > 0
                                ORDER BY T.TURMA ASC "
                    };

                    contextQuery.Parameters.Add("@ALUNO", aluno);
                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@CENSO", censo);
                    contextQuery.Parameters.Add("@TURMAORIGEM", turmaOrigem);

                    turmasOptativaReforco = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return turmasOptativaReforco;
        }


        public DataTable ListaTurmaReforcoPor(int ano, int periodo, string turno, string censo, string disciplina, decimal serie)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                if (ano > 0 && periodo >= 0 && !string.IsNullOrEmpty(censo) && !string.IsNullOrEmpty(disciplina) && !string.IsNullOrEmpty(turno) && serie > 0)
                {
                    var contextQuery = new ContextQuery
                         {
                             Command = @"SELECT t.TURMA,
                                            MAX(NUM_ALUNOS) AS 'MAXIMO_ALUNOS',
                                            COUNT(DISTINCT mat.ALUNO) AS 'ALUNOS_MATRICULADOS',
                                            MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) AS 'VAGAS'
                                    FROM    dbo.LY_TURMA t
                                            LEFT JOIN ly_matricula mat ON mat.DISCIPLINA = t.DISCIPLINA
                                                                          AND mat.TURMA = t.TURMA
                                                                          AND mat.ANO = t.ANO
                                                                          AND mat.SEMESTRE = t.SEMESTRE
                                                                          AND mat.SIT_MATRICULA <> 'Cancelado'                                                          
                                                                          AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)
                                    WHERE   t.ANO = @ANO
							                AND t.SEMESTRE = @PERIODO
							                AND t.FACULDADE = @CENSO
							                AND t.TURNO = @TURNO
							                and t.disciplina = @DISCIPLINA
                                            AND T.SERIE = @SERIE
							                AND t.CURSO in ('9999.01','9999.03','9999.02')
                                            AND t.SIT_TURMA = 'Aberta'
                                    GROUP BY t.TURMA
                                    HAVING  MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) > 0
                                    ORDER BY t.TURMA ASC"

                             /*           Command =
                                            @" select TURMA 
                                               from LY_TURMA 
                                               where ano = @ANO
                                                 and faculdade = @CENSO   
                                                 and semestre = @PERIODO                       
                                                 and turno = @TURNO
                                                 and disciplina = @DISCIPLINA"*/
                         };
                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@TURNO", turno);
                    contextQuery.Parameters.Add("@CENSO", censo);
                    contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                    contextQuery.Parameters.Add("@SERIE", serie);


                    return ctx.GetDataTable(contextQuery);
                }
            }
            return null;
        }

        public string ObtemDisciplinaOptativa(decimal ano, decimal periodo, string turma)
        {
            DataContext contexto = null;
            string disciplina = string.Empty;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                disciplina = ObtemDisciplinaOptativaPor(contexto, ano, periodo, turma);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
            return disciplina;
        }

        public string ObtemTipoTurma(decimal ano, decimal periodo, string turma)
        {
            DataContext contexto = null;
            string curso = string.Empty;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                curso = ObtemTipoTurmaPor(contexto, ano, periodo, turma);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
            return curso;
        }

        public string ObtemDisciplinaOptativaProjetoFoco(decimal ano, decimal periodo, string turma)
        {
            DataContext contexto = null;
            string disciplina = string.Empty;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                disciplina = ObtemDisciplinaOptativaProjetoFocoPor(contexto, ano, periodo, turma);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return disciplina;
        }

        public string ObtemTipoTurmaPor(DataContext ctx, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            string CURSO = string.Empty;

            contextQuery.Command = @" SELECT  TOP 1
                                CURSO
                        FROM    DBO.LY_TURMA
                        WHERE   TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @PERIODO";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            CURSO = ctx.GetReturnValue<string>(contextQuery);

            return CURSO;
        }

        public DateTime ObtemDataFimPor(DataContext contexto, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            DateTime retorno = DateTime.MinValue;

            try
            {
                contextQuery.Command = @" SELECT TOP 1 DT_FIM
                        FROM    DBO.LY_TURMA
                        WHERE   TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @PERIODO ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDateTime(reader["DT_FIM"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public DateTime ObtemDataInicioPor(DataContext contexto, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            DateTime retorno = DateTime.MinValue;

            try
            {
                contextQuery.Command = @" SELECT TOP 1 DT_INICIO
                        FROM    DBO.LY_TURMA
                        WHERE   TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @PERIODO ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDateTime(reader["DT_INICIO"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public string ObtemDisciplinaOptativaPor(DataContext ctx, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            string disciplina = string.Empty;

            contextQuery.Command = @" SELECT  TOP 1
                                DISCIPLINA
                        FROM    DBO.LY_TURMA
                        WHERE   TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @PERIODO
                                AND NOT ( OPTATIVAREFORCO = 'N' ) ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            disciplina = ctx.GetReturnValue<string>(contextQuery);

            return disciplina;
        }
        public string ObtemDisciplinaOptativaProjetoFocoPor(DataContext ctx, decimal ano, decimal periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            string disciplina = string.Empty;

            contextQuery.Command = @" SELECT  TOP 1
                                DISCIPLINA
                        FROM    DBO.LY_TURMA
                        WHERE   TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @PERIODO";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            disciplina = ctx.GetReturnValue<string>(contextQuery);

            return disciplina;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTurma"></param>
        /// <param name="pAno"></param>
        /// <param name="pPeriodo"></param>
        /// <returns></returns>
        /// <autor>Anderson Wernek</autor>
        public static QueryTable ObtemAlunosAtivosMatriculadosNaTurmaPor(string pTurma, string pAno, string pPeriodo)
        {
            return RN.RNBase.Consultar(@"
                                        SELECT DISTINCT *
                                        FROM   ly_turma t (nolock)
                                               INNER JOIN ly_matricula m (nolock)
                                                       ON ( m.disciplina = t.disciplina
                                                            AND m.turma = t.turma
                                                            AND m.ano = t.ano
                                                            AND m.semestre = t.semestre )
                                        WHERE  m.sit_matricula = 'Matriculado'
                                               AND t.turma = ?
                                               AND t.ano = ?
                                               AND t.semestre = ?  
                                        ", pTurma, pAno, pPeriodo);
        }

        public int ObtemAlunosMatriculadosNaTurmaPor(string Turma, string Ano, string Semestre)
        {
            int total = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  COUNT(DISTINCT M.ALUNO) AS TOTAL
                                        FROM   ly_turma t (nolock)
                                               INNER JOIN ly_matricula m (nolock)
                                                       ON ( m.disciplina = t.disciplina
                                                            AND m.turma = t.turma
                                                            AND m.ano = t.ano
                                                            AND m.semestre = t.semestre )
                                        WHERE  m.sit_matricula = 'Matriculado'
                                               AND t.turma = @TURMA
                                               AND t.ano = @ANO
                                               AND t.semestre = @SEMESTRE ")
                };

                contextQuery.Parameters.Add("@TURMA", Turma);
                contextQuery.Parameters.Add("@ANO", Ano);
                contextQuery.Parameters.Add("@SEMESTRE", Semestre);


                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["TOTAL"]);
                }

                return total;
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

        public string ObtemDisciplinasAtivasPor(int ano, int periodo, string turma, List<string> alunos)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            string disciplinasAtivas = string.Empty;
            string listaAlunos = string.Empty;

            for (var j = 0; j < alunos.Count; j++)
            {
                if (!string.IsNullOrEmpty(listaAlunos))
                {
                    listaAlunos = string.Format("{0}', '", listaAlunos);
                }

                listaAlunos = listaAlunos + alunos[j];
            }

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = string.Format(@" SELECT  DISCIPLINA
                                FROM    DBO.LY_MATRICULA  (NOLOCK)
                                WHERE   SIT_MATRICULA = 'Matriculado'
                                        AND TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND ALUNO IN ( '{0}' )
                                GROUP BY DISCIPLINA ",
                    listaAlunos)
                };

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(disciplinasAtivas))
                    {
                        disciplinasAtivas = string.Format("{0} - ", disciplinasAtivas);
                    }

                    disciplinasAtivas += Convert.ToString(reader["DISCIPLINA"]);
                }


                return disciplinasAtivas;
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


        public void FinalizaTurma(DataContext ctx, string turma, decimal ano, decimal periodo)
        {
            try
            {
                ContextQuery contextQuery = new ContextQuery();

                contextQuery.Command = @" UPDATE  LY_TURMA
                        SET     SIT_TURMA = 'Finalizada'
                        WHERE   TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);

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

        public bool PossuiTurmaAbertaSemAlunoMatriculado(string turma, decimal ano, decimal semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    dbo.LY_TURMA
                        WHERE   SIT_TURMA = 'Aberta'
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE
                                AND TURMA = @TURMA
                                AND TURMA NOT IN ( SELECT DISTINCT
                                                            TURMA
                                                   FROM     dbo.LY_MATRICULA
                                                   WHERE    SIT_MATRICULA = 'Matriculado'
                                                            AND ANO = @ANO
                                                            AND SEMESTRE = @SEMESTRE )
                                AND TURMA IN ( SELECT  TURMA
                                                            FROM    dbo.LY_HISTMATRICULA
                                                            WHERE   ANO = @ANO
                                                                    AND SEMESTRE = @SEMESTRE ) "
                };

                contextQuery.Parameters.Add("@TURMA", turma);
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

        public bool EhTurmaAbertaPor(DataContext ctx, int ano, int periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                    FROM    DBO.LY_TURMA
                                    WHERE   ANO = @ANO
                                            AND SEMESTRE = @SEMESTRE
                                            AND TURMA = @TURMA
                                            AND SIT_TURMA = 'Aberta' ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool EhEletivaAbertaPor(DataContext ctx, int ano, int periodo, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                    FROM    DBO.LY_TURMA
                                    WHERE   ANO = @ANO
                                            AND SEMESTRE = @SEMESTRE
                                            AND TURMA = @TURMA
                                            AND ELETIVA = 'S'
                                            AND SIT_TURMA = 'Aberta' ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@TURMA", turma);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void FinalizaTurma(string turma, decimal ano, decimal periodo)
        {
            try
            {
                var contextQuery = new ContextQuery(@" UPDATE  LY_TURMA
                        SET     SIT_TURMA = 'Finalizada'
                        WHERE   TURMA = @TURMA
                                AND ANO = @ANO
                                AND SEMESTRE = @SEMESTRE ");

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);

                ExecutarAlteracao(contextQuery);
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
        }

        public bool ConsultaTurmaAtivaPor(string curso)
        {
            var retorno = ExecutarFuncao<int?>(
                new ContextQuery(
                    @"SELECT TOP 1
                            1                   
                        FROM    dbo.LY_TURMA t
                        WHERE   t.CURSO = @CURSO
                                AND SIT_TURMA = 'Aberta'
                                AND ( DT_FIM IS NULL
                                      OR DT_FIM >= GETDATE()
                                    )
                                AND AMBIENTE_EXTERNO='S' ",
                     new ContextQueryParameter("@CURSO", curso)
                   ));

            return retorno != null;
        }

        public int RetornaQtdeTurmasConfTurnoVagaPor(int ano, string censo, int tipoEventoId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            int totalTurmas = 0;

            try
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT  CASE WHEN ( SELECT TOP 1
                                                1
                                        FROM    agenda.PERIODOLETIVOAGENDA P
                                                INNER JOIN agenda.AGENDA AA ON AA.AGENDAID = P.AGENDAID
                                                INNER JOIN agenda.EVENTO AE ON AA.AGENDAID = AE.AGENDAID
                                        WHERE   GETDATE() BETWEEN DATAINICIO AND DATAFIM
                                                AND AE.TIPOEVENTOID = @TIPOEVENTOID --TipoEvento Confirmação de Vagas   
                                                AND P.PERIODO = 2
                                                AND P.ANO = @ANO --ANO DA CONFIRMAÇÃO 
                                        
                                      ) = 1
                                 THEN 
                           --Se agenda ativa possui periodo 2 
                                      ( SELECT  SUM(TOTAL)
                                        FROM    ( SELECT    COUNT(DISTINCT TURMA) TOTAL
                                                  FROM      dbo.LY_TURMA T
                                                            INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON T.ANO = A.ANO_REFERENCIA
                                                                                  AND T.SEMESTRE = A.PERIODO_REFERENCIA
                                                                                  AND T.CURSO = A.CURSO
                                                                                  AND T.SERIE = A.SERIE
                                                  WHERE     T.SIT_TURMA <> 'Desativada'
                                                            AND T.FACULDADE = @CENSO
                                                            AND A.ANO = @ANO --ANO DA CONFIRMAÇÃO 
                                                            AND A.PERIODO = 2
                                                            AND OPTATIVAREFORCO = 'N'
                                                            AND ISNULL(T.ELETIVA,'N') = 'N'
                                                            AND NOT EXISTS ( SELECT
                                                                          1
                                                                     FROM dbo.TCE_CTV_RESTRICAO RE
                                                                     WHERE
                                                                          RE.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                                                          AND RE.CENSO = T.FACULDADE )
                                                            AND T.CURSO NOT IN (
                                                            SELECT  CURSO
                                                            FROM    LY_CURSO
                                                            WHERE   PARTICIPACALCULONOVASTURMASTURNOSVAGAS = 'N' )
                                                  UNION
                                                  SELECT    COUNT(DISTINCT TURMA) TOTAL
                                                  FROM      dbo.LY_TURMA T
                                                  WHERE     T.SIT_TURMA = 'Aberta'
                                                            AND T.FACULDADE = @CENSO
                                                            AND T.ANO = @ANO --ANO DA CONFIRMAÇÃO 
                                                            AND T.SEMESTRE = 0
                                                            AND T.OPTATIVAREFORCO = 'N'
                                                            AND ISNULL(T.ELETIVA,'N') = 'N'
                                                            AND T.CURSO NOT IN (
                                                            SELECT  CURSO
                                                            FROM    LY_CURSO
                                                            WHERE   PARTICIPACALCULONOVASTURMASTURNOSVAGAS = 'N' )
                                                ) TABELASOMA
                                      )
                                 ELSE 
                           --Se agenda ativa não possui periodo 2 
                                      ( SELECT  COUNT(DISTINCT TURMA)
                                        FROM    dbo.LY_TURMA T
                                                INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON T.ANO = A.ANO_REFERENCIA
                                                                                  AND T.SEMESTRE = A.PERIODO_REFERENCIA
                                                                                  AND T.CURSO = A.CURSO
                                                                                  AND T.SERIE = A.SERIE
                                        WHERE   T.SIT_TURMA <> 'Desativada'
                                                AND T.FACULDADE = @CENSO
                                                AND A.ANO = @ANO --ANO DA CONFIRMAÇÃO 
                                                AND A.PERIODO IN ( 0, 1 )
                                                AND OPTATIVAREFORCO = 'N'
                                                AND ISNULL(T.ELETIVA,'N') = 'N'
                                                AND NOT EXISTS ( SELECT
                                                                          1
                                                                     FROM dbo.TCE_CTV_RESTRICAO RE
                                                                     WHERE
                                                                          RE.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                                                          AND RE.CENSO = T.FACULDADE )
                                                AND T.CURSO NOT IN (
                                                SELECT  CURSO
                                                FROM    LY_CURSO
                                                WHERE   PARTICIPACALCULONOVASTURMASTURNOSVAGAS = 'N' )
                                      )
                            END TOTALTURMAS "
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TIPOEVENTOID", tipoEventoId);

                totalTurmas = ctx.GetReturnValue<int>(contextQuery);

                return totalTurmas;
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

        public DataTable PrimeiraTurmasGradeComVagasDiferentePor(string ano, string periodo, string unidadeEns, string curso, string turno, decimal serie, string turma)
        {
            DataTable turmas = null;

            if (!ano.IsNullOrEmptyOrWhiteSpace() && !periodo.IsNullOrEmptyOrWhiteSpace() && !unidadeEns.IsNullOrEmptyOrWhiteSpace() && !curso.IsNullOrEmptyOrWhiteSpace() && !turno.IsNullOrEmptyOrWhiteSpace() && serie != -1)
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command =
                            @" DECLARE @TURMA_TRANSFERENCIA_PENDENTE TABLE
                                    (
                                      CENSO VARCHAR(20) ,
                                      ANO NUMERIC(4) ,
                                      PERIODO NUMERIC(2) ,
                                      TURMA VARCHAR(50) ,
                                      NUM_TRANSFERENCIAS INTEGER
                                    )

                                DECLARE @TURMA_GRADE TABLE
                                    (
                                      FACULDADE VARCHAR(20) ,
                                      ANO NUMERIC(4) ,
                                      SEMESTRE NUMERIC(2) ,
                                      TURMA VARCHAR(100) ,
                                      GRADE VARCHAR(120) ,
                                      GRADE_ID VARCHAR(100) ,
                                      NUM_ALUNOS INTEGER ,
                                      ALUNOS_MATRICULADOS INTEGER
                                    )

                                INSERT  INTO @TURMA_TRANSFERENCIA_PENDENTE
                                        SELECT  TD.CENSO ,
                                                TD.ANO ,
                                                TD.PERIODO ,
                                                TD.TURMA ,
                                                COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS NUM_TRANSFERENCIAS
                                        FROM    LYCEUM.DBO.TCE_TRANSFERENCIA T ( NOLOCK )
                                                INNER JOIN LYCEUM.DBO.TCE_TRANSFERENCIA_DESTINO TD ( NOLOCK ) ON T.ID_TRANSFERENCIA = TD.ID_TRANSFERENCIA
                                        WHERE   T.[STATUS] = 'Pendente'
                                                AND TD.ANO = @ANO
                                                AND TD.PERIODO = @PERIODO
                                                AND TD.CENSO = @CENSO
                                                AND TD.CURSO = @CURSO
                                                AND TD.TURNO = @TURNO
                                                AND TD.SERIE = @SERIE
                                        GROUP BY TD.CENSO ,
                                                TD.ANO ,
                                                TD.PERIODO ,
                                                TD.TURMA
                                        
                                INSERT  INTO @TURMA_GRADE
                                        SELECT DISTINCT
                                                T.FACULDADE ,
                                                T.ANO ,
                                                T.SEMESTRE ,
                                                T.TURMA ,
                                                T.TURMA + ' - ' + TU.DESCRICAO AS GRADE ,
                                                ( CONVERT(VARCHAR, GS.GRADE_ID) + '|' + GS.GRADE ) AS GRADE_ID ,
                                                MAX(NUM_ALUNOS) AS NUM_ALUNOS ,
                                                COUNT(DISTINCT MAT.ALUNO) AS ALUNOS_MATRICULADOS
                                        FROM    LYCEUM.DBO.LY_TURMA T ( NOLOCK )
                                                INNER JOIN LY_TURNO TU ( NOLOCK ) ON T.TURNO = TU.TURNO
                                                INNER JOIN LY_GRADE_SERIE GS ( NOLOCK ) ON T.CURSO = GS.CURSO
                                                                                           AND T.TURNO = GS.TURNO
                                                                                           AND T.CURRICULO = GS.CURRICULO
                                                                                           AND T.SERIE = GS.SERIE
                                                                                           AND T.ANO = GS.ANO
                                                                                           AND T.SEMESTRE = GS.SEMESTRE
                                                                                           AND T.FACULDADE = GS.UNIDADE_RESPONSAVEL
                                                                                           AND T.TURMA = GS.GRADE
                                                LEFT JOIN LYCEUM.DBO.LY_MATRICULA MAT ( NOLOCK ) ON MAT.DISCIPLINA = T.DISCIPLINA
                                                                                              AND MAT.TURMA = T.TURMA
                                                                                              AND MAT.ANO = T.ANO
                                                                                              AND MAT.SEMESTRE = T.SEMESTRE
                                                                                              AND MAT.SIT_MATRICULA <> 'Cancelado'
                                                                                              AND ( mat.DEPENDENCIA <> 'S'
                                                                                              OR mat.DEPENDENCIA IS NULL
                                                                                              )
                                        WHERE   t.ANO = @ANO
                                                AND t.SEMESTRE = @PERIODO
                                                AND t.FACULDADE = @CENSO
                                                AND t.CURSO = @CURSO
                                                AND t.TURNO = @TURNO
                                                AND t.SERIE = @SERIE
                                                AND T.SIT_TURMA = 'Aberta'
                                                AND T.TURMA <> @TURMA
                                                AND T.OPTATIVAREFORCO = 'N'
                                                AND ISNULL(T.ELETIVA,'N') = 'N'
                                        GROUP BY T.FACULDADE ,
                                                T.ANO ,
                                                T.SEMESTRE ,
                                                T.TURMA ,
                                                TU.DESCRICAO ,
                                                GS.GRADE_ID ,
                                                GS.GRADE
                                             
                                SELECT  TOP 1 TG.TURMA ,
                                        TG.GRADE ,
                                        TG.GRADE_ID
                                FROM    @TURMA_GRADE TG
                                        LEFT JOIN @TURMA_TRANSFERENCIA_PENDENTE TT ON TG.TURMA = TT.TURMA
                                WHERE   NUM_ALUNOS > ( ALUNOS_MATRICULADOS + ISNULL(NUM_TRANSFERENCIAS, 0) )
                                ORDER BY TG.TURMA ASC "
                    };


                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@CENSO", unidadeEns);
                    contextQuery.Parameters.Add("@CURSO", curso);
                    contextQuery.Parameters.Add("@TURNO", turno);
                    contextQuery.Parameters.Add("@SERIE", serie);
                    contextQuery.Parameters.Add("@TURMA", turma);

                    turmas = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return turmas;
        }

        public DataTable ListaTurmasGradeComVagasPor(string ano, string periodo, string unidadeEns, string curso, string turno, decimal serie)
        {
            DataTable turmas = null;

            if (!ano.IsNullOrEmptyOrWhiteSpace() && !periodo.IsNullOrEmptyOrWhiteSpace() && !unidadeEns.IsNullOrEmptyOrWhiteSpace() && !curso.IsNullOrEmptyOrWhiteSpace() && !turno.IsNullOrEmptyOrWhiteSpace() && serie != -1)
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command =
                            @" DECLARE @TURMA_TRANSFERENCIA_PENDENTE TABLE
                                    (
                                      CENSO VARCHAR(20) ,
                                      ANO NUMERIC(4) ,
                                      PERIODO NUMERIC(2) ,
                                      TURMA VARCHAR(50) ,
                                      NUM_TRANSFERENCIAS INTEGER
                                    )

                                DECLARE @TURMA_GRADE TABLE
                                    (
                                      FACULDADE VARCHAR(20) ,
                                      ANO NUMERIC(4) ,
                                      SEMESTRE NUMERIC(2) ,
                                      TURMA VARCHAR(100) ,
                                      GRADE VARCHAR(120) ,
                                      GRADE_ID VARCHAR(100) ,
                                      NUM_ALUNOS INTEGER ,
                                      ALUNOS_MATRICULADOS INTEGER
                                    )

                                INSERT  INTO @TURMA_TRANSFERENCIA_PENDENTE
                                        SELECT  TD.CENSO ,
                                                TD.ANO ,
                                                TD.PERIODO ,
                                                TD.TURMA ,
                                                COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS NUM_TRANSFERENCIAS
                                        FROM    LYCEUM.DBO.TCE_TRANSFERENCIA T ( NOLOCK )
                                                INNER JOIN LYCEUM.DBO.TCE_TRANSFERENCIA_DESTINO TD ( NOLOCK ) ON T.ID_TRANSFERENCIA = TD.ID_TRANSFERENCIA
                                        WHERE   T.[STATUS] = 'Pendente'
                                                AND TD.ANO = @ANO
                                                AND TD.PERIODO = @PERIODO
                                                AND TD.CENSO = @CENSO
                                                AND TD.CURSO = @CURSO
                                                AND TD.TURNO = @TURNO
                                                AND TD.SERIE = @SERIE
                                        GROUP BY TD.CENSO ,
                                                TD.ANO ,
                                                TD.PERIODO ,
                                                TD.TURMA
                                        
                                INSERT  INTO @TURMA_GRADE
                                        SELECT DISTINCT
                                                T.FACULDADE ,
                                                T.ANO ,
                                                T.SEMESTRE ,
                                                T.TURMA ,
                                                T.TURMA + ' - ' + TU.DESCRICAO AS GRADE ,
                                                ( CONVERT(VARCHAR, GS.GRADE_ID) + '|' + GS.GRADE ) AS GRADE_ID ,
                                                MAX(NUM_ALUNOS) AS NUM_ALUNOS ,
                                                COUNT(DISTINCT MAT.ALUNO) AS ALUNOS_MATRICULADOS
                                        FROM    LYCEUM.DBO.LY_TURMA T ( NOLOCK )
                                                INNER JOIN LY_TURNO TU ( NOLOCK ) ON T.TURNO = TU.TURNO
                                                INNER JOIN LY_GRADE_SERIE GS ( NOLOCK ) ON T.CURSO = GS.CURSO
                                                                                           AND T.TURNO = GS.TURNO
                                                                                           AND T.CURRICULO = GS.CURRICULO
                                                                                           AND T.SERIE = GS.SERIE
                                                                                           AND T.ANO = GS.ANO
                                                                                           AND T.SEMESTRE = GS.SEMESTRE
                                                                                           AND T.FACULDADE = GS.UNIDADE_RESPONSAVEL
                                                                                           AND T.TURMA = GS.GRADE
                                                LEFT JOIN LYCEUM.DBO.LY_MATRICULA MAT ( NOLOCK ) ON MAT.DISCIPLINA = T.DISCIPLINA
                                                                                              AND MAT.TURMA = T.TURMA
                                                                                              AND MAT.ANO = T.ANO
                                                                                              AND MAT.SEMESTRE = T.SEMESTRE
                                                                                              AND MAT.SIT_MATRICULA <> 'Cancelado'
                                                                                              AND ( mat.DEPENDENCIA <> 'S'
                                                                                              OR mat.DEPENDENCIA IS NULL
                                                                                              )
                                        WHERE   t.ANO = @ANO
                                                AND t.SEMESTRE = @PERIODO
                                                AND t.FACULDADE = @CENSO
                                                AND t.CURSO = @CURSO
                                                AND t.TURNO = @TURNO
                                                AND t.SERIE = @SERIE
                                                AND T.SIT_TURMA = 'Aberta'
                                                AND T.OPTATIVAREFORCO = 'N'
                                                AND ISNULL(T.ELETIVA,'N') = 'N'
                                        GROUP BY T.FACULDADE ,
                                                T.ANO ,
                                                T.SEMESTRE ,
                                                T.TURMA ,
                                                TU.DESCRICAO ,
                                                GS.GRADE_ID ,
                                                GS.GRADE
                                             
                                SELECT  TG.TURMA ,
                                        TG.GRADE ,
                                        TG.GRADE_ID
                                FROM    @TURMA_GRADE TG
                                        LEFT JOIN @TURMA_TRANSFERENCIA_PENDENTE TT ON TG.TURMA = TT.TURMA
                                WHERE   NUM_ALUNOS > ( ALUNOS_MATRICULADOS + ISNULL(NUM_TRANSFERENCIAS, 0) )
                                ORDER BY TG.TURMA ASC "
                    };


                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@CENSO", unidadeEns);
                    contextQuery.Parameters.Add("@CURSO", curso);
                    contextQuery.Parameters.Add("@TURNO", turno);
                    contextQuery.Parameters.Add("@SERIE", serie);

                    turmas = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return turmas;
        }

        public DataTable ListaTurmasGradeComVagasPor(string ano, string periodo, string unidadeEns)
        {
            DataTable turmas = null;

            if (!ano.IsNullOrEmptyOrWhiteSpace() && !periodo.IsNullOrEmptyOrWhiteSpace() && !unidadeEns.IsNullOrEmptyOrWhiteSpace())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command =
                            @" DECLARE @TURMA_TRANSFERENCIA_PENDENTE TABLE
                                (
                                  CENSO VARCHAR(20) ,
                                  ANO NUMERIC(4) ,
                                  PERIODO NUMERIC(2) ,
                                  TURMA VARCHAR(50) ,
                                  NUM_TRANSFERENCIAS INTEGER
                                )

                            DECLARE @TURMA_GRADE TABLE
                                (
                                  FACULDADE VARCHAR(20) ,
                                  ANO NUMERIC(4) ,
                                  SEMESTRE NUMERIC(2) ,
                                  TURMA VARCHAR(100) ,
                                  GRADE VARCHAR(120) ,
                                  GRADE_ID VARCHAR(100) ,
                                  NUM_ALUNOS INTEGER ,
                                  ALUNOS_MATRICULADOS INTEGER
                                )

                            INSERT  INTO @TURMA_TRANSFERENCIA_PENDENTE
                                    SELECT  TD.CENSO ,
                                            TD.ANO ,
                                            TD.PERIODO ,
                                            TD.TURMA ,
                                            COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS NUM_TRANSFERENCIAS
                                    FROM    LYCEUM.DBO.TCE_TRANSFERENCIA T ( NOLOCK )
                                            INNER JOIN LYCEUM.DBO.TCE_TRANSFERENCIA_DESTINO TD ( NOLOCK ) ON T.ID_TRANSFERENCIA = TD.ID_TRANSFERENCIA
                                    WHERE   T.[STATUS] = 'Pendente'
                                            AND TD.ANO = @ANO
                                            AND TD.PERIODO = @PERIODO
                                            AND TD.CENSO = @CENSO
                                    GROUP BY TD.CENSO ,
                                            TD.ANO ,
                                            TD.PERIODO ,
                                            TD.TURMA
                                    
                            INSERT  INTO @TURMA_GRADE
                                    SELECT DISTINCT
                                            T.FACULDADE ,
                                            T.ANO ,
                                            T.SEMESTRE ,
                                            T.TURMA ,
                                            T.TURMA + ' - ' + TU.DESCRICAO AS GRADE ,
                                            ( CONVERT(VARCHAR, GS.GRADE_ID) + '|' + GS.GRADE ) AS GRADE_ID ,
                                            MAX(NUM_ALUNOS) AS NUM_ALUNOS ,
                                            COUNT(DISTINCT MAT.ALUNO) AS ALUNOS_MATRICULADOS
                                    FROM    LYCEUM.DBO.LY_TURMA T ( NOLOCK )
                                            INNER JOIN LY_TURNO TU ( NOLOCK ) ON T.TURNO = TU.TURNO
                                            INNER JOIN LY_GRADE_SERIE GS ( NOLOCK ) ON T.CURSO = GS.CURSO
                                                                                       AND T.TURNO = GS.TURNO
                                                                                       AND T.CURRICULO = GS.CURRICULO
                                                                                       AND T.SERIE = GS.SERIE
                                                                                       AND T.ANO = GS.ANO
                                                                                       AND T.SEMESTRE = GS.SEMESTRE
                                                                                       AND T.FACULDADE = GS.UNIDADE_RESPONSAVEL
                                                                                       AND T.TURMA = GS.GRADE
                                            LEFT JOIN LYCEUM.DBO.LY_MATRICULA MAT ( NOLOCK ) ON MAT.DISCIPLINA = T.DISCIPLINA
                                                                                          AND MAT.TURMA = T.TURMA
                                                                                          AND MAT.ANO = T.ANO
                                                                                          AND MAT.SEMESTRE = T.SEMESTRE
                                                                                          AND MAT.SIT_MATRICULA <> 'Cancelado'
                                                                                          AND ( mat.DEPENDENCIA <> 'S'
                                                                                          OR mat.DEPENDENCIA IS NULL
                                                                                          )
                                    WHERE   t.ANO = @ANO
                                            AND t.SEMESTRE = @PERIODO
                                            AND t.FACULDADE = @CENSO
                                            AND T.SIT_TURMA = 'Aberta'
                                            AND T.OPTATIVAREFORCO = 'N'
                                            AND ISNULL(T.ELETIVA,'N') = 'N'
                                    GROUP BY T.FACULDADE ,
                                            T.ANO ,
                                            T.SEMESTRE ,
                                            T.TURMA ,
                                            TU.DESCRICAO ,
                                            GS.GRADE_ID ,
                                            GS.GRADE
                                         
                            SELECT  TG.TURMA ,
                                    TG.GRADE ,
                                    TG.GRADE_ID
                            FROM    @TURMA_GRADE TG
                                    LEFT JOIN @TURMA_TRANSFERENCIA_PENDENTE TT ON TG.TURMA = TT.TURMA
                            WHERE   NUM_ALUNOS > ( ALUNOS_MATRICULADOS + ISNULL(NUM_TRANSFERENCIAS, 0) )
                            ORDER BY TG.TURMA ASC "
                    };

                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@CENSO", unidadeEns);

                    turmas = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return turmas;
        }

        public DataTable PrimeiraTurmasGradeComVagasDiferentePor(string ano, string periodo, string unidadeEns, string turma)
        {
            DataTable turmas = null;

            if (!ano.IsNullOrEmptyOrWhiteSpace() && !periodo.IsNullOrEmptyOrWhiteSpace() && !unidadeEns.IsNullOrEmptyOrWhiteSpace())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command =
                            @" DECLARE @TURMA_TRANSFERENCIA_PENDENTE TABLE
                                (
                                  CENSO VARCHAR(20) ,
                                  ANO NUMERIC(4) ,
                                  PERIODO NUMERIC(2) ,
                                  TURMA VARCHAR(50) ,
                                  NUM_TRANSFERENCIAS INTEGER
                                )

                            DECLARE @TURMA_GRADE TABLE
                                (
                                  FACULDADE VARCHAR(20) ,
                                  ANO NUMERIC(4) ,
                                  SEMESTRE NUMERIC(2) ,
                                  TURMA VARCHAR(100) ,
                                  GRADE VARCHAR(120) ,
                                  GRADE_ID VARCHAR(100) ,
                                  NUM_ALUNOS INTEGER ,
                                  ALUNOS_MATRICULADOS INTEGER
                                )

                            INSERT  INTO @TURMA_TRANSFERENCIA_PENDENTE
                                    SELECT  TD.CENSO ,
                                            TD.ANO ,
                                            TD.PERIODO ,
                                            TD.TURMA ,
                                            COUNT(DISTINCT td.ID_TRANSFERENCIA_DESTINO) AS NUM_TRANSFERENCIAS
                                    FROM    LYCEUM.DBO.TCE_TRANSFERENCIA T ( NOLOCK )
                                            INNER JOIN LYCEUM.DBO.TCE_TRANSFERENCIA_DESTINO TD ( NOLOCK ) ON T.ID_TRANSFERENCIA = TD.ID_TRANSFERENCIA
                                    WHERE   T.[STATUS] = 'Pendente'
                                            AND TD.ANO = @ANO
                                            AND TD.PERIODO = @PERIODO
                                            AND TD.CENSO = @CENSO
                                    GROUP BY TD.CENSO ,
                                            TD.ANO ,
                                            TD.PERIODO ,
                                            TD.TURMA
                                    
                            INSERT  INTO @TURMA_GRADE
                                    SELECT DISTINCT
                                            T.FACULDADE ,
                                            T.ANO ,
                                            T.SEMESTRE ,
                                            T.TURMA ,
                                            T.TURMA + ' - ' + TU.DESCRICAO AS GRADE ,
                                            ( CONVERT(VARCHAR, GS.GRADE_ID) + '|' + GS.GRADE ) AS GRADE_ID ,
                                            MAX(NUM_ALUNOS) AS NUM_ALUNOS ,
                                            COUNT(DISTINCT MAT.ALUNO) AS ALUNOS_MATRICULADOS
                                    FROM    LYCEUM.DBO.LY_TURMA T ( NOLOCK )
                                            INNER JOIN LY_TURNO TU ( NOLOCK ) ON T.TURNO = TU.TURNO
                                            INNER JOIN LY_GRADE_SERIE GS ( NOLOCK ) ON T.CURSO = GS.CURSO
                                                                                       AND T.TURNO = GS.TURNO
                                                                                       AND T.CURRICULO = GS.CURRICULO
                                                                                       AND T.SERIE = GS.SERIE
                                                                                       AND T.ANO = GS.ANO
                                                                                       AND T.SEMESTRE = GS.SEMESTRE
                                                                                       AND T.FACULDADE = GS.UNIDADE_RESPONSAVEL
                                                                                       AND T.TURMA = GS.GRADE
                                            LEFT JOIN LYCEUM.DBO.LY_MATRICULA MAT ( NOLOCK ) ON MAT.DISCIPLINA = T.DISCIPLINA
                                                                                          AND MAT.TURMA = T.TURMA
                                                                                          AND MAT.ANO = T.ANO
                                                                                          AND MAT.SEMESTRE = T.SEMESTRE
                                                                                          AND MAT.SIT_MATRICULA <> 'Cancelado'
                                                                                          AND ( mat.DEPENDENCIA <> 'S'
                                                                                          OR mat.DEPENDENCIA IS NULL
                                                                                          )
                                    WHERE   t.ANO = @ANO
                                            AND t.SEMESTRE = @PERIODO
                                            AND t.FACULDADE = @CENSO
                                            AND T.TURMA <> @TURMA
                                            AND T.SIT_TURMA = 'Aberta'
                                            AND T.OPTATIVAREFORCO = 'N'
                                            AND ISNULL(T.ELETIVA,'N') = 'N'
                                    GROUP BY T.FACULDADE ,
                                            T.ANO ,
                                            T.SEMESTRE ,
                                            T.TURMA ,
                                            TU.DESCRICAO ,
                                            GS.GRADE_ID ,
                                            GS.GRADE
                                         
                            SELECT  TOP 1 TG.TURMA ,
                                    TG.GRADE ,
                                    TG.GRADE_ID
                            FROM    @TURMA_GRADE TG
                                    LEFT JOIN @TURMA_TRANSFERENCIA_PENDENTE TT ON TG.TURMA = TT.TURMA
                            WHERE   NUM_ALUNOS > ( ALUNOS_MATRICULADOS + ISNULL(NUM_TRANSFERENCIAS, 0) )
                            ORDER BY TG.TURMA ASC "
                    };

                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@CENSO", unidadeEns);
                    contextQuery.Parameters.Add("@TURMA", turma);

                    turmas = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return turmas;
        }

        public int ObtemTotalAlunosMatriculadosNaTurmaPor(string turma, string ano, string periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            int totalAlunos = 0;
            try
            {
                var contextQuery = new ContextQuery(


                        @" SELECT  COUNT(DISTINCT M.ALUNO)
                                                    FROM   ly_matricula m (nolock)                                                                
                                                    WHERE  m.sit_matricula = 'Matriculado'
                                                           AND m.turma = @TURMA
                                                           AND m.ano = @ANO
                                                           AND m.semestre = @SEMESTRE ");

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);

                totalAlunos = ctx.GetReturnValue<int>(contextQuery);

                return totalAlunos;
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

        public int ObtemQuantidadeTurmaPor(int ano, int periodo, string censo, string curso, int serie, string turno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT COUNT(DISTINCT TURMA) AS QTDETURMAS
		                                    FROM LY_TURMA
		                                    WHERE ANO = @ANO
			                                    AND SEMESTRE = @PERIODO
			                                    AND FACULDADE = @CENSO 
			                                    AND CURSO = @CURSO
			                                    AND SERIE = @SERIE
			                                    AND TURNO = @TURNO
			                                    AND OPTATIVAREFORCO = 'N' 
		                                        AND ISNULL(ELETIVA, 'N') = 'N' ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QTDETURMAS"]);
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

        public string ObtemTipoOptativaReforcoPor(string turma, decimal ano, decimal periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            string tipo = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT TOP 1
                                        OPTATIVAREFORCO
                                FROM    DBO.LY_TURMA
                                WHERE   TURMA = @TURMA
                                        AND ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);

                tipo = ctx.GetReturnValue<string>(contextQuery);

                return tipo;
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

        public bool ExisteTurmaDisciplinaComObrigatoriaCadastrada(string curriculo, string curso, string turno, string disciplina, int serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    DBO.LY_TURMA T
                                INNER JOIN LY_GRADE G ON T.CURRICULO = G.CURRICULO
                                                         AND T.TURNO = G.TURNO
                                                         AND T.CURSO = G.CURSO
                                                         AND T.DISCIPLINA = G.DISCIPLINA
                                                         AND T.SERIE = G.SERIE_IDEAL
                        WHERE   G.OBRIGATORIA = 'S'
                                AND T.CURRICULO = @CURRICULO
                                AND T.CURSO = @CURSO
                                AND T.TURNO = @TURNO
                                AND T.DISCIPLINA = @DISCIPLINA
                                AND T.SERIE = @SERIE ";

                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
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

        public bool ExisteTurmaPor(string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    dbo.LY_TURMA
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

        public bool ExisteTurmaPor(string disciplina, string disciplinaMultipla)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    dbo.LY_TURMA
                        WHERE   DISCIPLINA = @DISCIPLINA
                                AND DISCIPLINA_MULTIPLA = @DISCIPLINA_MULTIPLA ";

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@DISCIPLINA_MULTIPLA", disciplinaMultipla);

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

        public DTOs.DadosConsolidadoBimestralTurma ObtemConsolidadoBimestralPor(int ano, int periodo, string turma, string disciplina)
        {
            DTOs.DadosConsolidadoBimestralTurma consolidadoBimestral = new DTOs.DadosConsolidadoBimestralTurma();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            RN.Frequencia rnFrequencia = new Frequencia();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int subperiodo = 0;
            int quantidadeSubPeriodos = 0;

            try
            {
                //Busca informações sobre aulas da turma
                rnFrequencia.ObtemConsolidadoBimestralPor(ctx, ano, periodo, turma, disciplina, out consolidadoBimestral);

                //Carrega dados da turma
                consolidadoBimestral.Ano = ano;
                consolidadoBimestral.Periodo = periodo;
                consolidadoBimestral.Turma = turma;
                consolidadoBimestral.Disciplina = disciplina;

                //Busca Medias da turma
                contextQuery = new ContextQuery();
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"SP_MEDIA_POR_BIMESTRE_TURMA";
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    quantidadeSubPeriodos++;
                    subperiodo = Convert.ToInt32(reader["SUBPERIODO"]);

                    if (subperiodo == 1 && (reader["MEDIA"] != DBNull.Value))
                    {
                        consolidadoBimestral.MediaTurmaBimestre1 = Convert.ToDecimal(reader["MEDIA"]);
                    }
                    if (subperiodo == 2 && (reader["MEDIA"] != DBNull.Value))
                    {
                        consolidadoBimestral.MediaTurmaBimestre2 = Convert.ToDecimal(reader["MEDIA"]);
                    }
                    if (subperiodo == 3 && (reader["MEDIA"] != DBNull.Value))
                    {
                        consolidadoBimestral.MediaTurmaBimestre3 = Convert.ToDecimal(reader["MEDIA"]);
                    }
                    if (subperiodo == 4 && (reader["MEDIA"] != DBNull.Value))
                    {
                        consolidadoBimestral.MediaTurmaBimestre4 = Convert.ToDecimal(reader["MEDIA"]);
                    }
                }
                consolidadoBimestral.QuantidadeSubPeriodos = quantidadeSubPeriodos;

                return consolidadoBimestral;
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

        public DTOs.DadosConsolidadoBimestralTurma ObtemHistoricoConsolidadoBimestralPor(int ano, int periodo, string turma, string disciplina)
        {
            DTOs.DadosConsolidadoBimestralTurma consolidadoBimestral = new DTOs.DadosConsolidadoBimestralTurma();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            RN.Frequencia rnFrequencia = new Frequencia();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int subperiodo = 0;
            int quantidadeSubPeriodos = 0;

            try
            {
                //Busca informações sobre aulas da turma
                rnFrequencia.ObtemConsolidadoBimestralPor(ctx, ano, periodo, turma, disciplina, out consolidadoBimestral);

                //Carrega dados da turma
                consolidadoBimestral.Ano = ano;
                consolidadoBimestral.Periodo = periodo;
                consolidadoBimestral.Turma = turma;
                consolidadoBimestral.Disciplina = disciplina;

                //Busca Historico de Medias da turma
                contextQuery = new ContextQuery();
                contextQuery.ContextQueryType = ContextQueryType.StoredProcedure;
                contextQuery.Command = @"SP_MEDIA_POR_BIMESTRE_TURMA_HISTORICO";
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    quantidadeSubPeriodos++;
                    subperiodo = Convert.ToInt32(reader["SUBPERIODO"]);

                    if (subperiodo == 1 && (reader["MEDIA"] != DBNull.Value))
                    {
                        consolidadoBimestral.MediaTurmaBimestre1 = Convert.ToDecimal(reader["MEDIA"]);
                    }
                    if (subperiodo == 2 && (reader["MEDIA"] != DBNull.Value))
                    {
                        consolidadoBimestral.MediaTurmaBimestre2 = Convert.ToDecimal(reader["MEDIA"]);
                    }
                    if (subperiodo == 3 && (reader["MEDIA"] != DBNull.Value))
                    {
                        consolidadoBimestral.MediaTurmaBimestre3 = Convert.ToDecimal(reader["MEDIA"]);
                    }
                    if (subperiodo == 4 && (reader["MEDIA"] != DBNull.Value))
                    {
                        consolidadoBimestral.MediaTurmaBimestre4 = Convert.ToDecimal(reader["MEDIA"]);
                    }
                }
                consolidadoBimestral.QuantidadeSubPeriodos = quantidadeSubPeriodos;

                return consolidadoBimestral;
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

        public DataTable ListaTurmaAbertaPor(int ano, int periodo, string turno, string unidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT TURMA 
                                        FROM   DBO.LY_TURMA 
                                        WHERE  ANO = @ANO 
                                               AND SEMESTRE = @PERIODO 
                                               AND TURNO = @TURNO 
                                               AND FACULDADE = @UNIDADE 
                                               AND SIT_TURMA = 'Aberta' 
                                               AND OPTATIVAREFORCO = 'N' 
                                               AND ISNULL(ELETIVA,'N') = 'N'
                                        ORDER  BY TURMA  ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@UNIDADE", unidade);

                turmas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return turmas;
        }

        public DataTable ListaTurmaFrequenciaPor(int ano, int periodo, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT 
                                            TU.curso, 
                                            C.NOME nomeCurso, 
                                            TU.turno, 
                                            T.DESCRICAO descricaoTurno,
                                            TU.serie, 
                                            S.DESCRICAO descricaoSerie,
                                            TU.ano,
                                            TU.semestre,
                                            TU.turma,
					                        tu.NUM_ALUNOS as capacidade,
                                            UE.NOME_COMP nomeUnidadeResponsavel,
                                            TU.dependencia,
                                            TU.curriculo,
                                            TU.faculdade,
                                            (select
		                                        case TU.em_elaboracao
			                                    when 'S' then 'Horário incompleto'
			                                    when 'N' then 'Horário completo'
			                                    else 'Sem alocação' end) em_elaboracao,
                                            TU.turma_integracao as sufixo,
                                            tu.sit_turma
                                        FROM LY_TURMA TU                    
                                            inner join ly_unidade_ensino UE ON
                                                UE.unidade_ens = tu.FACULDADE
                                            inner join ly_turno T ON
                                                T.turno = tu.turno
                                            inner join ly_serie S ON 
                                                S.SERIE = tu.SERIE AND 
                                                S.TURNO = tu.TURNO AND 
                                                S.CURRICULO = tu.CURRICULO AND
						                        S.CURSO = tu.CURSO
                                            inner join LY_CURSO C ON 
                                                C.CURSO = tu.CURSO                        
                                        WHERE TU.ESPECIAL <> 'S' 
                                            AND tu.ANO = @ANO
                                            AND TU.SEMESTRE = @SEMESTRE
                                            AND TU.FACULDADE = @CENSO
					                        AND TU.SIT_TURMA <> 'Desativada'
                                        ORDER BY TU.TURMA ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);

                turmas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return turmas;
        }

        private static bool PossuiMatriculaParaNecessidadeContratoTemporario(QueryTable qtOriginal, Ly_aula_docente.Row linhaAulaDocente, string matricula)
        {
            //Verificar s existia 99999999 ou 00000000 antes de alocar contrato
            if (!string.IsNullOrEmpty(matricula))
            {
                SimpleRow[] dadosLinha = qtOriginal.Select("turno = '" + linhaAulaDocente.Turno + "' " +
                                                           " AND faculdade = '" + linhaAulaDocente.Faculdade + "' " +
                                                           " AND dia_semana =  " + linhaAulaDocente.Dia_semana +
                                                           " AND aula = '" + linhaAulaDocente.Aula + "' " +
                                                           " AND turma = '" + linhaAulaDocente.Turma + "' " +
                                                           " AND ano = " + linhaAulaDocente.Ano +
                                                           " AND semestre =  " + linhaAulaDocente.Semestre +
                                                           " AND disciplina = '" + linhaAulaDocente.Disciplina + "'" +
                                                           " AND matricula in ('99999999', '00000000', '" + matricula + "')");
                if (dadosLinha != null && dadosLinha.Length > 0)
                {
                    return true;







                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public bool ExisteDisciplinaTurmaDestinoPor(DataContext ctx, decimal ano, decimal semestre, string turmaDestino, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) FROM dbo.LY_TURMA t
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
                                   AND G.DISCIPLINA = @DISCIPLINA ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@TURMA", turmaDestino);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public string ObtemTurnoTurmaPor(DataContext ctx, decimal ano, decimal semestre, string turma)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            string turno = string.Empty;

            contextQuery.Command = @" SELECT DISTINCT ANO, 
                                            SEMESTRE, 
                                            FACULDADE, 
                                            CURSO, 
                                            CURRICULO, 
                                            TURNO, 
                                            SERIE, 
                                            TURMA 
                            FROM   DBO.LY_TURMA 
                            WHERE  ANO = @ANO 
                                   AND SEMESTRE = @SEMESTRE 
                                   AND TURMA = @TURMA 
                                   AND SIT_TURMA = 'Aberta'  ";

            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                turno = Convert.ToString(reader["turno"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return turno;
        }

        public DTOs.DadosTurma ObtemDadosTurmaAbertaPor(DataContext contexto, string turma, int ano, int semestre)
        {
            DTOs.DadosTurma dadosTurma = new DTOs.DadosTurma();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT       
                                                ANO ,
                                                SEMESTRE ,
                                                TURMA ,
                                                CURSO ,
                                                TURNO ,
                                                SERIE ,
                                                CURRICULO ,
                                                FACULDADE ,
		                                        DT_FIM
                                        FROM    DBO.LY_TURMA (NOLOCK)
                                        WHERE   TURMA = @TURMA
		                                        AND ANO = @ANO
		                                        AND SEMESTRE = @SEMESTRE
                                                AND SIT_TURMA = 'Aberta' ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosTurma.Ano = Convert.ToInt32(reader["ANO"]);
                    dadosTurma.Periodo = Convert.ToInt32(reader["SEMESTRE"]);
                    dadosTurma.Turma = Convert.ToString(reader["TURMA"]);
                    dadosTurma.Curso = Convert.ToString(reader["CURSO"]);
                    dadosTurma.Serie = Convert.ToInt32(reader["SERIE"]);
                    dadosTurma.Turno = Convert.ToString(reader["TURNO"]);
                    dadosTurma.Censo = Convert.ToString(reader["FACULDADE"]);
                    dadosTurma.Curriculo = Convert.ToString(reader["CURRICULO"]);
                    dadosTurma.DataFimTurma = Convert.ToDateTime(reader["DT_FIM"]);
                }

                return dadosTurma;
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

        public DTOs.DadosTurma ObtemDadosTurmaPor(string turma, int ano, int semestre)
        {
            DTOs.DadosTurma dadosTurma = new DTOs.DadosTurma();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT       
                                                ANO ,
                                                SEMESTRE ,
                                                TURMA ,
                                                CURSO ,
                                                TURNO ,
                                                SERIE ,
                                                CURRICULO ,
                                                FACULDADE ,
		                                        DT_FIM
                                        FROM    DBO.LY_TURMA (NOLOCK)
                                        WHERE   TURMA = @TURMA
		                                        AND ANO = @ANO
		                                        AND SEMESTRE = @SEMESTRE
                                                AND SIT_TURMA = 'Aberta' ";

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@SEMESTRE", TechneDbType.T_SEMESTRE2, semestre);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosTurma.Ano = Convert.ToInt32(reader["ANO"]);
                    dadosTurma.Periodo = Convert.ToInt32(reader["SEMESTRE"]);
                    dadosTurma.Turma = Convert.ToString(reader["TURMA"]);
                    dadosTurma.Curso = Convert.ToString(reader["CURSO"]);
                    dadosTurma.Serie = Convert.ToInt32(reader["SERIE"]);
                    dadosTurma.Turno = Convert.ToString(reader["TURNO"]);
                    dadosTurma.Censo = Convert.ToString(reader["FACULDADE"]);
                    dadosTurma.Curriculo = Convert.ToString(reader["CURRICULO"]);
                    dadosTurma.DataFimTurma = Convert.ToDateTime(reader["DT_FIM"]);
                }

                return dadosTurma;
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

        public bool PossuiTurmaAbertaPor(decimal ano, decimal semestre)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT  COUNT(*)
                        FROM    dbo.LY_TURMA
                        WHERE   ANO = @ANO
                                AND SEMESTRE = @SEMESTRE"
                };

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

        public bool PossuiTurmaAbertaPor(DataContext ctx, decimal ano, decimal semestre, string curso, string turno, string curriculo)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
               {
                   Command = @" SELECT  COUNT(1)
                                FROM    LY_TURMA
                                WHERE   ANO = @ANO
                                        AND SEMESTRE = @SEMESTRE
                                        AND CURSO = @CURSO
                                        AND TURNO = @TURNO 
                                        AND CURRICULO = @CURRICULO
                                        AND SIT_TURMA = 'Aberta'"
               };

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURRICULO", curriculo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool PossuiTurmaAbertaMesmaSalaETurnoPor(decimal ano, int semestre, string dependencia, string turno, string faculdade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(semestre);
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(string.Format(@" SELECT  COUNT(*)
                        FROM    dbo.LY_TURMA
                        WHERE   ANO = @ANO
                                AND SEMESTRE IN ( {0} )  
                                AND SIT_TURMA = 'Aberta'
                                AND FACULDADE = @FACULDADE
                                AND OPTATIVAREFORCO = 'N'
                                AND ISNULL(ELETIVA,'N') = 'N'
                                AND DEPENDENCIA = @DEPENDENCIA
                                 ", possiveisPeriodos));

                if (turno == "I")
                {
                    sql.Append(" AND (TURNO = @TURNO OR TURNO = 'M' or TURNO = 'T') ");
                }
                else if (turno == "A")
                {
                    sql.Append(" AND (TURNO = @TURNO OR TURNO = 'N' or TURNO = 'T') ");
                }
                else if (turno == "M")
                {
                    sql.Append(" AND (TURNO = @TURNO OR TURNO = 'I') ");
                }
                else if (turno == "T")
                {
                    sql.Append(" AND (TURNO = @TURNO OR TURNO = 'I' or TURNO = 'A') ");
                }
                else
                {
                    sql.Append(" AND (TURNO = @TURNO OR TURNO = 'A') ");
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@DEPENDENCIA", dependencia);
                contextQuery.Parameters.Add("@FACULDADE", faculdade);

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

        public string RetornaTurmaDesativadaMesmaSalaETurnoPor(decimal ano, int semestre, string dependencia, string turno, string faculdade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<string> turmas = new List<string>();
            string resultado = string.Empty;
            string possiveisPeriodos = Utils.RecuperaPossiveisPeriodosCompleto(semestre);
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(string.Format(@" SELECT DISTINCT TURMA
                        FROM    dbo.LY_TURMA
                        WHERE   ANO = @ANO
                                AND SEMESTRE IN ( {0} )  
                                AND SIT_TURMA = 'Desativada'
                                AND FACULDADE = @FACULDADE
                                AND OPTATIVAREFORCO = 'N'
                                AND ISNULL(ELETIVA,'N') = 'N'
                                AND DEPENDENCIA = @DEPENDENCIA
                                 ", possiveisPeriodos));

                if (turno == "I")
                {
                    sql.Append(" AND (TURNO = @TURNO OR TURNO = 'M' or TURNO = 'T') ");
                }
                else if (turno == "A")
                {
                    sql.Append(" AND (TURNO = @TURNO OR TURNO = 'N' or TURNO = 'T') ");
                }
                else if (turno == "M")
                {
                    sql.Append(" AND (TURNO = @TURNO OR TURNO = 'I') ");
                }
                else if (turno == "T")
                {
                    sql.Append(" AND (TURNO = @TURNO OR TURNO = 'I' or TURNO = 'A') ");
                }
                else
                {
                    sql.Append(" AND (TURNO = @TURNO OR TURNO = 'A') ");
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@DEPENDENCIA", dependencia);
                contextQuery.Parameters.Add("@FACULDADE", faculdade);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    turmas.Add(Convert.ToString(reader["TURMA"]));
                }

                if (turmas.Count > 0)
                {
                    resultado = turmas.Aggregate((x, y) => x + " e " + y);
                }

                return resultado;
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

        public RetValue ReativaTurma(decimal ano, decimal semestre, string turma, string dependencia, int numAlunos)
        {
            RetValue retorno;
            var connection = Config.CreateWritableConnection();

            var sql = @"UPDATE    LY_TURMA
                        SET     DEPENDENCIA = ? ,
                                NUM_ALUNOS = ?,
                                SIT_TURMA = 'Aberta',
                                STAMP_ATUALIZACAO = GETDATE()
                        WHERE  ANO = ?
                                AND SEMESTRE = ?
                                AND TURMA = ?
                                AND SIT_TURMA = 'Desativada'
                               ";

            try
            {
                connection.Open(true);

                TCommand.ExecuteNonQuery(
                            connection,
                            sql,
                            dependencia,
                            numAlunos,
                            ano,
                            semestre,
                            turma);

                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null
                    && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }

                retorno = new RetValue(true, "Reativação eftetuada com sucesso.", null);
            }
            catch (Exception e)
            {
                connection.Rollback();
                ErrorList errorList = new ErrorList();
                errorList.Add("Não é possível reativar a turma. Motivo: " + e.Message, "ERRO");
                retorno = new RetValue(false, string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}", Environment.NewLine, Convert.ToString(e.Message)), errorList);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }

            return retorno;
        }

        public DataTable ListaTurmasParaReferenciaPor(string ano, string periodo, string unidadeEns)
        {
            DataTable turmas = null;

            if (!ano.IsNullOrEmptyOrWhiteSpace() && !periodo.IsNullOrEmptyOrWhiteSpace() && !unidadeEns.IsNullOrEmptyOrWhiteSpace())
            {
                try
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" SELECT DISTINCT
                                            T.FACULDADE ,
                                            T.ANO ,
                                            T.SEMESTRE ,
                                            T.TURMA ,                                           
                                            ( CONVERT(VARCHAR, GS.GRADE_ID) + '|' + GS.GRADE ) AS GRADE_ID 
                                    FROM    LYCEUM.DBO.LY_TURMA T ( NOLOCK )
                                            INNER JOIN LY_CURSO C ( NOLOCK ) ON C.CURSO = T.CURSO
                                            INNER JOIN LY_GRADE_SERIE GS ( NOLOCK ) ON T.CURSO = GS.CURSO
                                                                                       AND T.TURNO = GS.TURNO
                                                                                       AND T.CURRICULO = GS.CURRICULO
                                                                                       AND T.SERIE = GS.SERIE
                                                                                       AND T.ANO = GS.ANO
                                                                                       AND T.SEMESTRE = GS.SEMESTRE
                                                                                       AND T.FACULDADE = GS.UNIDADE_RESPONSAVEL
                                                                                       AND T.TURMA = GS.GRADE
											INNER JOIN LY_SERIE S on t.CURRICULO = S.CURRICULO
														AND T.TURNO = S.TURNO
														AND T.CURSO = S.CURSO  
														AND T.SERIE = S.SERIE                                        
                                    WHERE   t.ANO = @ANO
                                            AND t.SEMESTRE = @PERIODO
                                            AND t.FACULDADE = @CENSO
                                            AND T.OPTATIVAREFORCO = 'N'
                                            AND ISNULL(T.ELETIVA,'N') = 'N'
                                            AND C.OFERTAELETIVA = 'S'
											and S.OFERTAELETIVA = 'S' "
                    };

                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@CENSO", unidadeEns);

                    turmas = Consultar(contextQuery);
                }
                catch (Exception exception)
                {
                    var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                            Environment.NewLine, Convert.ToString(exception.Message));
                    throw new Exception(mensagem);
                }
            }

            return turmas;
        }

        public List<string> ListaTurnoPor(string censo, int ano, int periodo, string curso, int serie)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ListaTurnoPor(contexto, censo, ano, periodo, curso, serie);
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

        public List<string> ListaTurnoPor(DataContext contexto, string censo, int ano, int periodo, string curso, int serie)
        {
            List<string> turnos = new List<string>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT TURNO
										FROM LY_TURMA 
                                        WHERE FACULDADE = @CENSO
		                                        AND ANO = @ANO 
		                                        AND SEMESTRE = @PERIODO
		                                        AND CURSO = @CURSO
		                                        AND SERIE = @SERIE ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    turnos.Add(Convert.ToString(reader["TURNO"]));
                }

                return turnos;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DataTable ListaTurmaRegularOfereceEletivaPor(decimal ano, decimal semestre, string curso, string turno, string censo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ListaTurmaRegularOfereceEletivaPor(contexto, ano, semestre, curso, turno, censo);
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
        }

        private DataTable ListaTurmaRegularOfereceEletivaPor(DataContext contexto, decimal ano, decimal semestre, string curso, string turno, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT  DISTINCT t.TURMA + ' - Referência' as DESCRICAOTURMA, 
												T.TURMA,
												T.ANO, 
												T.SEMESTRE, 
												T.SERIE, 
												T.CURSO, 
												T.TURNO, 
												T.FACULDADE,
												1 AS REFERENCIA
										FROM LY_TURMA T 
											INNER JOIN LY_TURMA E 
														ON T.TURMA = E.TURMAREFERENCIA 
														AND T.ANO = E.ANO 
														AND T.SEMESTRE = E.SEMESTRE		
										WHERE T.ANO = @ANO	
											AND T.SEMESTRE = @SEMESTRE
											AND T.CURSO = @CURSO
											AND T.TURNO = @TURNO
											AND T.FACULDADE = @CENSO	
											AND T.OPTATIVAREFORCO = 'N'
											AND ISNULL(T.ELETIVA,'N') = 'N' 
										UNION												
										SELECT  DISTINCT TURMA AS DESCRICAOTURMA, 
												TURMA,
												ANO, 
												SEMESTRE, 
												t.SERIE, 
												t.CURSO, 
												t.TURNO, 
												FACULDADE,
												0 AS REFERENCIA
                                          FROM  dbo.LY_TURMA t
												INNER JOIN LY_SERIE S on t.CURRICULO = S.CURRICULO
														AND T.TURNO = S.TURNO
														AND T.CURSO = S.CURSO  
														AND T.SERIE = S.SERIE 
                                            WHERE   t.ANO = @ANO
                                                AND t.SEMESTRE = @SEMESTRE
		                                        AND t.CURSO = @CURSO
		                                        AND t.TURNO = @TURNO
		                                        AND t.FACULDADE = @CENSO
		                                        AND t.OPTATIVAREFORCO = 'N'
												AND ISNULL(t.ELETIVA,'N') = 'N' 
												and S.OFERTAELETIVA = 'S'
                                            ORDER BY SERIE, TURMA ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CENSO", censo);

            dt = contexto.GetDataTable(contextQuery);

            return dt;
        }

        public List<DTOs.DadosDistruicaoEletivas> ObtemListaDistribuicaoEletivaPor(decimal ano, decimal semestre, string curso, string turno, string censo)
        {
            List<DTOs.DadosDistruicaoEletivas> lista = new List<DTOs.DadosDistruicaoEletivas>();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                //Lista turmas regulares
                DataTable turmas = this.ListaTurmaRegularOfereceEletivaPor(contexto, ano, semestre, curso, turno, censo);

                foreach (DataRow item in turmas.Rows)
                {
                    DTOs.DadosDistruicaoEletivas dados = new Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas();
                    dados.TurmaReferencia = item["TURMA"].ToString();
                    dados.DescricaoTurmaReferencia = item["DESCRICAOTURMA"].ToString();
                    dados.Ano = Convert.ToInt32(ano);
                    dados.Semestre = Convert.ToInt32(semestre);
                    dados.Referencia = Convert.ToBoolean(item["REFERENCIA"]);

                    //Verifica se a turma é de referencia
                    if (Convert.ToBoolean(dados.Referencia))
                    {
                        //Busca Turma eletiva, e sua disciplina multipla do grupo 1 para esta turma de referencia
                        DadosTurmaDisciplina grupo1 = this.ObtemTurmaEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 1).FirstOrDefault();
                        if (grupo1 != null)
                        {
                            dados.TurmaEletivaGrupo1 = grupo1.Turma;
                            dados.DisciplinaGrupo1 = grupo1.DisciplinaMultipla;
                        }

                        //Busca Turma eletiva, e sua disciplina multipla do grupo 2 para esta turma de referencia
                        DadosTurmaDisciplina grupo2 = this.ObtemTurmaEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 2).FirstOrDefault();
                        if (grupo2 != null)
                        {
                            dados.TurmaEletivaGrupo2 = grupo2.Turma;
                            dados.DisciplinaGrupo2 = grupo2.DisciplinaMultipla;
                        }

                        //Busca Turma eletiva, e sua disciplina multipla do grupo 3 para esta turma de referencia
                        DadosTurmaDisciplina grupo3 = this.ObtemTurmaEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 3).FirstOrDefault();
                        if (grupo3 != null)
                        {
                            dados.TurmaEletivaGrupo3 = grupo3.Turma;
                            dados.DisciplinaGrupo3 = grupo3.DisciplinaMultipla;
                        }
                    }
                    else
                    {
                        //Busca disciplina multipla do grupo 1 
                        DadosTurmaDisciplina grupo1 = this.ObtemDadosEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 1).FirstOrDefault();
                        if (grupo1 != null)
                        {
                            dados.TurmaEletivaGrupo1 = grupo1.Turma;
                            dados.DisciplinaGrupo1 = grupo1.DisciplinaMultipla;
                        }

                        //Busca disciplina multipla do grupo 2
                        DadosTurmaDisciplina grupo2 = this.ObtemDadosEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 2).FirstOrDefault();
                        if (grupo2 != null)
                        {
                            dados.TurmaEletivaGrupo2 = grupo2.Turma;
                            dados.DisciplinaGrupo2 = grupo2.DisciplinaMultipla;
                        }

                        //Busca disciplina multipla do grupo 3
                        DadosTurmaDisciplina grupo3 = this.ObtemDadosEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 3).FirstOrDefault();
                        if (grupo3 != null)
                        {
                            dados.TurmaEletivaGrupo3 = grupo3.Turma;
                            dados.DisciplinaGrupo3 = grupo3.DisciplinaMultipla;
                        }
                    }

                    lista.Add(dados);
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
                contexto.Dispose();
            }
        }

        public DTOs.DadosDistruicaoEletivas ObtemDistribuicaoEletivaPor(int ano, int semestre, string turma, bool referencia)
        {
            DTOs.DadosDistruicaoEletivas dados = new Techne.Lyceum.RN.DTOs.DadosDistruicaoEletivas();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                dados.TurmaReferencia = turma;
                dados.Ano = ano;
                dados.Semestre = semestre;
                dados.Referencia = referencia;

                if (referencia)
                {
                    //Busca Turma eletiva, e sua disciplina multipla do grupo 1 para esta turma de referencia
                    DadosTurmaDisciplina grupo1 = this.ObtemTurmaEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 1).FirstOrDefault();
                    if (grupo1 != null)
                    {
                        dados.TurmaEletivaGrupo1 = grupo1.Turma;
                        dados.DisciplinaGrupo1 = grupo1.DisciplinaMultipla;
                    }

                    //Busca Turma eletiva, e sua disciplina multipla do grupo 2 para esta turma de referencia
                    DadosTurmaDisciplina grupo2 = this.ObtemTurmaEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 2).FirstOrDefault();
                    if (grupo2 != null)
                    {
                        dados.TurmaEletivaGrupo2 = grupo2.Turma;
                        dados.DisciplinaGrupo2 = grupo2.DisciplinaMultipla;
                    }

                    //Busca Turma eletiva, e sua disciplina multipla do grupo 3 para esta turma de referencia
                    DadosTurmaDisciplina grupo3 = this.ObtemTurmaEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 3).FirstOrDefault();
                    if (grupo3 != null)
                    {
                        dados.TurmaEletivaGrupo3 = grupo3.Turma;
                        dados.DisciplinaGrupo3 = grupo3.DisciplinaMultipla;
                    }
                }
                else
                {
                    //Busca disciplinas pela coluna turma 

                    //Busca disciplina multipla do grupo 1 
                    DadosTurmaDisciplina grupo1 = this.ObtemDadosEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 1).FirstOrDefault();
                    if (grupo1 != null)
                    {
                        dados.TurmaEletivaGrupo1 = grupo1.Turma;
                        dados.DisciplinaGrupo1 = grupo1.DisciplinaMultipla;
                    }

                    //Busca disciplina multipla do grupo 2
                    DadosTurmaDisciplina grupo2 = this.ObtemDadosEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 2).FirstOrDefault();
                    if (grupo2 != null)
                    {
                        dados.TurmaEletivaGrupo2 = grupo2.Turma;
                        dados.DisciplinaGrupo2 = grupo2.DisciplinaMultipla;
                    }

                    //Busca disciplina multipla do grupo 3
                    DadosTurmaDisciplina grupo3 = this.ObtemDadosEletivaPor(contexto, ano, semestre, dados.TurmaReferencia, 3).FirstOrDefault();
                    if (grupo3 != null)
                    {
                        dados.TurmaEletivaGrupo3 = grupo3.Turma;
                        dados.DisciplinaGrupo3 = grupo3.DisciplinaMultipla;
                    }
                }

                return dados;
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

        public bool PossuiTurmaEletivaPor(decimal ano, decimal semestre, string turmaReferencia, int grupo)
        {
            DataContext contexto = null;
            bool retorno = false;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                var turmasExistentes = this.ObtemTurmaEletivaPor(contexto, ano, semestre, turmaReferencia, grupo);
                if (turmasExistentes != null && turmasExistentes.Count > 0)
                {
                    retorno = true;
                }
            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            return retorno;
        }

        public List<DadosTurmaDisciplina> ObtemTurmaEletivaPor(DataContext contexto, decimal ano, decimal semestre, string turmaReferencia, int grupo)
        {
            List<DadosTurmaDisciplina> lista = new List<DadosTurmaDisciplina>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT  TURMA, d.DISCIPLINA, DISCIPLINA_MULTIPLA, TURMAREFERENCIA, ANO, SEMESTRE, GRUPO
                                            FROM LY_TURMA T
                                            INNER JOIN LY_DISCIPLINA D ON T.DISCIPLINA = D.DISCIPLINA
                                            WHERE T.ELETIVA = 'S'
	                                            AND TURMAREFERENCIA = @TURMAREFERENCIA
	                                            AND D.GRUPO = @GRUPO
	                                            and ANO = @ANO
	                                            and SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@TURMAREFERENCIA", turmaReferencia);
                contextQuery.Parameters.Add("@GRUPO", grupo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DadosTurmaDisciplina dados = new DadosTurmaDisciplina();

                    dados.Turma = Convert.ToString(reader["TURMA"]);
                    dados.Disciplina = Convert.ToString(reader["DISCIPLINA"]);
                    dados.DisciplinaMultipla = Convert.ToString(reader["DISCIPLINA_MULTIPLA"]);
                    dados.TurmaReferencia = Convert.ToString(reader["TURMAREFERENCIA"]);
                    dados.Ano = Convert.ToInt32(reader["ANO"]);
                    dados.Periodo = Convert.ToInt32(reader["SEMESTRE"]);

                    if (reader["GRUPO"] != DBNull.Value)
                    {
                        dados.Grupo = Convert.ToInt32(reader["GRUPO"]);
                    }
                    else
                    {
                        dados.Grupo = null;
                    }

                    lista.Add(dados);
                }

                return lista;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public List<DadosTurmaDisciplina> ObtemDadosEletivaPor(DataContext contexto, decimal ano, decimal semestre, string turma, int grupo)
        {
            List<DadosTurmaDisciplina> lista = new List<DadosTurmaDisciplina>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"   SELECT  TURMA, d.DISCIPLINA, DISCIPLINA_MULTIPLA, TURMAREFERENCIA, ANO, SEMESTRE, GRUPO
                                            FROM LY_TURMA T
                                            INNER JOIN LY_DISCIPLINA D ON T.DISCIPLINA = D.DISCIPLINA
                                            WHERE t.ELETIVA = 'S'
	                                            AND TURMA = @TURMAREFERENCIA
	                                            AND D.GRUPO = @GRUPO
	                                            and ANO = @ANO
	                                            and SEMESTRE = @SEMESTRE ";

                contextQuery.Parameters.Add("@TURMAREFERENCIA", turma);
                contextQuery.Parameters.Add("@GRUPO", grupo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DadosTurmaDisciplina dados = new DadosTurmaDisciplina();

                    dados.Turma = Convert.ToString(reader["TURMA"]);
                    dados.Disciplina = Convert.ToString(reader["DISCIPLINA"]);
                    dados.DisciplinaMultipla = Convert.ToString(reader["DISCIPLINA_MULTIPLA"]);
                    dados.TurmaReferencia = Convert.ToString(reader["TURMAREFERENCIA"]);
                    dados.Ano = Convert.ToInt32(reader["ANO"]);
                    dados.Periodo = Convert.ToInt32(reader["SEMESTRE"]);

                    if (reader["GRUPO"] != DBNull.Value)
                    {
                        dados.Grupo = Convert.ToInt32(reader["GRUPO"]);
                    }
                    else
                    {
                        dados.Grupo = null;
                    }

                    lista.Add(dados);
                }

                return lista;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public ValidacaoDados ValidaDistribuicaoEletiva(List<DTOs.DadosDistruicaoEletivas> turmasEletivas, List<DTOs.DadosDistruicaoEletivas> turmasEletivasCompleta, string usuarioId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.AulaDocente rnAulaDocente = new AulaDocente();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (turmasEletivas == null || turmasEletivas.Count == 0)
            {
                mensagens.Add("Nenhuma turma foi informada.");
            }
            else
            {
                foreach (DTOs.DadosDistruicaoEletivas item in turmasEletivas)
                {
                    if (item.TurmaReferencia.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo TURMA é obrigatório.");
                    }
                    else
                    {
                        if (item.DescricaoTurmaReferencia.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Campo DESCRIÇÃO TURMA é obrigatório.");
                        }

                        if (item.Referencia == null)
                        {
                            mensagens.Add("Campo TIPO TURMA é obrigatório.");
                        }

                        if (item.Serie <= 0)
                        {
                            mensagens.Add(string.Format("Campo SÉRIE é obrigatório para a Turma: {0}", item.DescricaoTurmaReferencia));
                        }
                        //RETIRADA A REGRA POR FALTA DE DEFINIÇÃO DA AREA RESPONSAVEL
                        //if (item.DisciplinaGrupo1.IsNullOrEmptyOrWhiteSpace())
                        //{
                        //    mensagens.Add(string.Format("Campo DISCIPLINA GRUPO 1 é obrigatório para a Turma: {0}", item.DescricaoTurmaReferencia));
                        //}

                        //if (item.DisciplinaGrupo2.IsNullOrEmptyOrWhiteSpace())
                        //{
                        //    mensagens.Add(string.Format("Campo DISCIPLINA GRUPO 2 é obrigatório para a Turma: {0}", item.DescricaoTurmaReferencia));
                        //}

                        //if (item.DisciplinaGrupo3.IsNullOrEmptyOrWhiteSpace())
                        //{
                        //    mensagens.Add(string.Format("Campo DISCIPLINA GRUPO 3 é obrigatório para a Turma: {0}", item.DescricaoTurmaReferencia));
                        //}
                    }
                }
            }

            var total = (turmasEletivas.Where(x => x.DisciplinaGrupo1 != null).Select(x => x.DisciplinaGrupo1).Distinct().Count()
                    + turmasEletivas.Where(x => x.DisciplinaGrupo2 != null).Select(x => x.DisciplinaGrupo2).Distinct().Count()
                    + turmasEletivas.Where(x => x.DisciplinaGrupo3 != null).Select(x => x.DisciplinaGrupo3).Distinct().Count());

            if (total == 0)
            {
                mensagens.Add("Para salvar é necessário escolher pelo menos 1 disciplina.");
            }

            if (usuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    foreach (DTOs.DadosDistruicaoEletivas item in turmasEletivas)
                    {
                        if (Convert.ToBoolean(item.Referencia))
                        {

                            //Busca turmas eletivas do grupo 1 que existem para a turma referencia
                            List<DadosTurmaDisciplina> turmasGrupo1 = this.ObtemTurmaEletivaPor(contexto, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), item.TurmaReferencia, 1);

                            //Verifica se tem turma eletiva do grupo 1
                            if (turmasGrupo1 == null || turmasGrupo1.Count == 0)
                            {
                                //AJUSTADA A REGRA POR FALTA DE DEFINIÇÃO DA AREA RESPONSAVEL
                                if (item.DisciplinaGrupo1 != null)
                                {
                                    mensagens.Add(string.Format("Não foi encontrada turma eletiva do grupo 1 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                                }
                            }
                            else if (turmasGrupo1.Count > 1)
                            {
                                mensagens.Add(string.Format("Foram encontradas mais de uma turma eletiva do grupo 1 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                            }
                            else
                            {
                                item.TurmaEletivaGrupo1 = turmasGrupo1.Select(x => x.Turma).FirstOrDefault();
                            }

                            if (turmasGrupo1.Select(x => x.DisciplinaMultipla).FirstOrDefault() != item.DisciplinaGrupo1)
                            {
                                //Verifica se existem aulas alocadas 
                                if (rnAulaDocente.ExisteDocentesEmAulaTurmaReferenciaAtivaPor(contexto, item.DescricaoTurmaReferencia, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), turmasGrupo1.Select(x => x.Disciplina).FirstOrDefault()))
                                {
                                    mensagens.Add(string.Format("Não é possível realizar alteração na Turma: {0}, pois existe docente(s) alocado(s) no quadro de horário. Por favor, realize a desalocação do(s) docente(s) e tente novamente.", item.TurmaReferencia));
                                }
                            }

                            //Busca turmas eletivas do grupo 2 que existem para a turma referencia
                            List<DadosTurmaDisciplina> turmasGrupo2 = this.ObtemTurmaEletivaPor(contexto, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), item.TurmaReferencia, 2);

                            //Verifica se tem turma eletiva do grupo 2
                            if (turmasGrupo2 == null || turmasGrupo2.Count == 0)
                            {
                                //AJUSTADA A REGRA POR FALTA DE DEFINIÇÃO DA AREA RESPONSAVEL
                                if (item.DisciplinaGrupo2 != null)
                                {
                                    mensagens.Add(string.Format("Não foi encontrada turma eletiva do grupo 2 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                                }
                            }
                            else if (turmasGrupo2.Count > 1)
                            {
                                mensagens.Add(string.Format("Foram encontradas mais de uma turma eletiva do grupo 2 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                            }
                            else
                            {
                                item.TurmaEletivaGrupo2 = turmasGrupo2.Select(x => x.Turma).FirstOrDefault();
                            }

                            if (turmasGrupo2.Select(x => x.DisciplinaMultipla).FirstOrDefault() != item.DisciplinaGrupo2)
                            {
                                //Verifica se existem aulas alocadas 
                                if (rnAulaDocente.ExisteDocentesEmAulaTurmaReferenciaAtivaPor(contexto, item.DescricaoTurmaReferencia, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), turmasGrupo2.Select(x => x.Disciplina).FirstOrDefault()))
                                {
                                    mensagens.Add(string.Format("Não é possível realizar alteração na Turma: {0}, pois existe docente(s) alocado(s) no quadro de horário. Por favor, realize a desalocação do(s) docente(s) e tente novamente.", item.TurmaReferencia));
                                }
                            }

                            //Busca turmas eletivas do grupo 3 que existem para a turma referencia
                            List<DadosTurmaDisciplina> turmasGrupo3 = this.ObtemTurmaEletivaPor(contexto, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), item.TurmaReferencia, 3);

                            //Verifica se tem turma eletiva do grupo 3
                            if (turmasGrupo3 == null || turmasGrupo3.Count == 0)
                            {
                                //AJUSTADA A REGRA POR FALTA DE DEFINIÇÃO DA AREA RESPONSAVEL
                                if (item.DisciplinaGrupo3 != null)
                                {
                                    mensagens.Add(string.Format("Não foi encontrada turma eletiva do grupo 3 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                                }
                            }
                            else if (turmasGrupo3.Count > 1)
                            {
                                mensagens.Add(string.Format("Foram encontradas mais de uma turma eletiva do grupo 3 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                            }
                            else
                            {
                                item.TurmaEletivaGrupo3 = turmasGrupo3.Select(x => x.Turma).FirstOrDefault();
                            }

                            if (turmasGrupo3.Select(x => x.DisciplinaMultipla).FirstOrDefault() != item.DisciplinaGrupo3)
                            {
                                //Verifica se existem aulas alocadas 
                                if (rnAulaDocente.ExisteDocentesEmAulaTurmaReferenciaAtivaPor(contexto, item.DescricaoTurmaReferencia, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), turmasGrupo3.Select(x => x.Disciplina).FirstOrDefault()))
                                {
                                    mensagens.Add(string.Format("Não é possível realizar alteração na Turma: {0}, pois existe docente(s) alocado(s) no quadro de horário. Por favor, realize a desalocação do(s) docente(s) e tente novamente.", item.TurmaReferencia));
                                }
                            }
                        }
                        else
                        {


                            //Busca dados eletivas do grupo 1 
                            List<DadosTurmaDisciplina> turmasGrupo1 = this.ObtemDadosEletivaPor(contexto, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), item.TurmaReferencia, 1);

                            //Verifica se tem turma eletiva do grupo 1
                            if (turmasGrupo1 == null || turmasGrupo1.Count == 0)
                            {
                                //AJUSTADA A REGRA POR FALTA DE DEFINIÇÃO DA AREA RESPONSAVEL
                                if (item.DisciplinaGrupo1 != null)
                                {
                                    mensagens.Add(string.Format("Não foi encontrada turma eletiva do grupo 1 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                                }
                            }
                            else if (turmasGrupo1.Count > 1)
                            {
                                mensagens.Add(string.Format("Foram encontradas mais de uma turma eletiva do grupo 1 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                            }
                            else
                            {
                                item.TurmaEletivaGrupo1 = turmasGrupo1.Select(x => x.Turma).FirstOrDefault();
                            }

                            if (turmasGrupo1.Select(x => x.DisciplinaMultipla).FirstOrDefault() != item.DisciplinaGrupo1)
                            {
                                //Verifica se existem aulas alocadas 
                                if (rnAulaDocente.ExisteDocentesEmAulaAtivaPor(contexto, item.DescricaoTurmaReferencia, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), turmasGrupo1.Select(x => x.Disciplina).FirstOrDefault()))
                                {
                                    mensagens.Add(string.Format("Não é possível realizar alteração na Turma: {0}, pois existe docente(s) alocado(s) no quadro de horário. Por favor, realize a desalocação do(s) docente(s) e tente novamente.", item.TurmaReferencia));
                                }
                            }

                            //Busca turmas eletivas do grupo 2 que existem para a turma referencia
                            List<DadosTurmaDisciplina> turmasGrupo2 = this.ObtemDadosEletivaPor(contexto, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), item.TurmaReferencia, 2);

                            //Verifica se tem turma eletiva do grupo 2
                            if (turmasGrupo2 == null || turmasGrupo2.Count == 0)
                            {
                                //AJUSTADA A REGRA POR FALTA DE DEFINIÇÃO DA AREA RESPONSAVEL
                                if (item.DisciplinaGrupo2 != null)
                                {
                                    mensagens.Add(string.Format("Não foi encontrada turma eletiva do grupo 2 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                                }
                            }
                            else if (turmasGrupo2.Count > 1)
                            {
                                mensagens.Add(string.Format("Foram encontradas mais de uma turma eletiva do grupo 2 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                            }
                            else
                            {
                                item.TurmaEletivaGrupo2 = turmasGrupo2.Select(x => x.Turma).FirstOrDefault();
                            }

                            if (turmasGrupo2.Select(x => x.DisciplinaMultipla).FirstOrDefault() != item.DisciplinaGrupo2)
                            {
                                //Verifica se existem aulas alocadas 
                                if (rnAulaDocente.ExisteDocentesEmAulaAtivaPor(contexto, item.DescricaoTurmaReferencia, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), turmasGrupo2.Select(x => x.Disciplina).FirstOrDefault()))
                                {
                                    mensagens.Add(string.Format("Não é possível realizar alteração na Turma: {0}, pois existe docente(s) alocado(s) no quadro de horário. Por favor, realize a desalocação do(s) docente(s) e tente novamente.", item.TurmaReferencia));
                                }
                            }

                            //Busca turmas eletivas do grupo 3 que existem para a turma referencia
                            List<DadosTurmaDisciplina> turmasGrupo3 = this.ObtemDadosEletivaPor(contexto, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), item.TurmaReferencia, 3);

                            //Verifica se tem turma eletiva do grupo 3
                            if (turmasGrupo3 == null || turmasGrupo3.Count == 0)
                            {
                                //AJUSTADA A REGRA POR FALTA DE DEFINIÇÃO DA AREA RESPONSAVEL
                                if (item.DisciplinaGrupo3 != null)
                                {
                                    mensagens.Add(string.Format("Não foi encontrada turma eletiva do grupo 3 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                                }
                            }
                            else if (turmasGrupo3.Count > 1)
                            {
                                mensagens.Add(string.Format("Foram encontradas mais de uma turma eletiva do grupo 3 cadastrada para a Turma Referência: {0}", item.TurmaReferencia));
                            }
                            else
                            {
                                item.TurmaEletivaGrupo3 = turmasGrupo3.Select(x => x.Turma).FirstOrDefault();
                            }

                            if (turmasGrupo3.Select(x => x.DisciplinaMultipla).FirstOrDefault() != item.DisciplinaGrupo3)
                            {
                                //Verifica se existem aulas alocadas 
                                if (rnAulaDocente.ExisteDocentesEmAulaAtivaPor(contexto, item.DescricaoTurmaReferencia, Convert.ToDecimal(item.Ano), Convert.ToDecimal(item.Semestre), turmasGrupo3.Select(x => x.Disciplina).FirstOrDefault()))
                                {
                                    mensagens.Add(string.Format("Não é possível realizar alteração na Turma: {0}, pois existe docente(s) alocado(s) no quadro de horário. Por favor, realize a desalocação do(s) docente(s) e tente novamente.", item.TurmaReferencia));
                                }
                            }
                        }
                    }

                    //Busca serie enviadas
                    List<int> series = turmasEletivas.Select(x => x.Serie).Distinct().ToList();
                    foreach (var serie in series)
                    {
                        //Verifica se existe mais e uma turma e caso exista se tem mais de uma disciplina
                        if (turmasEletivas.Where(x => x.Serie == serie).Select(x => x.TurmaReferencia).Distinct().Count() > 1
                           && turmasEletivasCompleta.Where(x => x.Serie == serie && x.DisciplinaGrupo3 != null).Select(x => x.DisciplinaGrupo3).Distinct().Count() == 1)
                        {
                            mensagens.Add("Não é permitido ofertar apenas uma eletiva do grupo 3 para uma mesma série. Verifique antes de salvar.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
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

        public void SalvaDistribuicaoEletiva(List<DTOs.DadosDistruicaoEletivas> turmasEletivas, string usuarioId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                foreach (DTOs.DadosDistruicaoEletivas item in turmasEletivas)
                {
                    //Atualizar discplina multipla da turma de eletiva do grupo 1
                    this.AtualizaDisciplinaMultiplaEletiva(contexto, item.Ano, item.Semestre, item.TurmaEletivaGrupo1, item.DisciplinaGrupo1, item.TurmaReferencia, 1);

                    //Atualizar discplina multipla da turma de eletiva do grupo 2
                    this.AtualizaDisciplinaMultiplaEletiva(contexto, item.Ano, item.Semestre, item.TurmaEletivaGrupo2, item.DisciplinaGrupo2, item.TurmaReferencia, 2);

                    //Atualizar discplina multipla da turma de eletiva do grupo 3
                    this.AtualizaDisciplinaMultiplaEletiva(contexto, item.Ano, item.Semestre, item.TurmaEletivaGrupo3, item.DisciplinaGrupo3, item.TurmaReferencia, 3);
                }
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

        private void AtualizaDisciplinaMultiplaEletiva(DataContext contexto, decimal ano, decimal semestre, string turma, string disciplinaMultipla, string turmaReferencia, int grupo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_TURMA
                                    SET DISCIPLINA_MULTIPLA = @DISCIPLINA_MULTIPLA
                                    FROM LY_TURMA T
                                    INNER JOIN LY_DISCIPLINA d ON ISNULL(t.DISCIPLINA_MULTIPLA, t.DISCIPLINA) = d.DISCIPLINA
                                    WHERE T.TURMA = @TURMA
	                                    AND T.ANO = @ANO
	                                    AND T.SEMESTRE = @SEMESTRE
	                                    AND T.ELETIVA = 'S'
	                                    AND D.GRUPO = @GRUPO ";

            contextQuery.Parameters.Add("@DISCIPLINA_MULTIPLA", disciplinaMultipla);
            contextQuery.Parameters.Add("@TURMA", turma);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@GRUPO", grupo);

            contexto.ApplyModifications(contextQuery);
        }

        public int ObtemNumeroMaximoAlunosPor(decimal ano, decimal semestre, string turma)
        {
            int total = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  TOP 1 NUM_ALUNOS 
                                        FROM   ly_turma t (nolock)                                             
                                        WHERE  t.turma = @TURMA
                                               AND t.ano = @ANO
                                               AND t.semestre = @SEMESTRE ")
                };

                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", semestre);


                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["NUM_ALUNOS"]);
                }

                return total;
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

        public bool PossuiCursoPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM LY_TURMA
                                WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaTurmaAbertaRetornoPresencialPor(int ano, int periodo, string turno, string unidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT TURMA 
                                        FROM   DBO.LY_TURMA 
                                        WHERE  ANO = @ANO 
                                               AND SEMESTRE = @PERIODO 
                                               AND TURNO = @TURNO 
                                               AND FACULDADE = @UNIDADE 
                                               AND SIT_TURMA = 'Aberta' 
                                               AND OPTATIVAREFORCO = 'N' 
                                               AND ISNULL(ELETIVA,'N') = 'N'
											   AND CURSO NOT IN ('0001.27','0002.37','9999.99','9999.89','9999.81','9999.82','9999.83','9999.84','9999.85','9999.86','9999.87','9999.88')
                                               AND FACULDADE NOT IN ('33019665','33021643', '33139245','33040060', '33042390',
                                                                         '33017999','33149380', '33145199','33096848', '33138834',
                                                                         '33100250','33139830', '33057982','33098972', '33125295',
                                                                         '33055300','33088691', '33138753','33062889', '33067678',
                                                                         '33097925','33138575', '33062862','33075042', '33085404',
                                                                         '33084025','33048274', '33142297','33099774', '33015163',
                                                                         '33025088','33119740', '33138699', '33027080','33139970',
                                                                         '33159602')
                                        ORDER  BY TURMA  ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@UNIDADE", unidade);

                turmas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return turmas;
        }

        public DataTable ListaTurnosOfertaPor(int ano, string censo, string modalidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            DataTable turnos = new DataTable();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int periodoReferencia = -1;
            int anoReferencia;
            int serieReferencia = -1;
            string cursoReferencia;

            try
            {
                if (modalidade == "RE1") //regular
                {
                    anoReferencia = ano - 1;
                    periodoReferencia = 0;
                    serieReferencia = 1;
                    cursoReferencia = "0002.61";
                }
                else
                {
                    anoReferencia = ano - 1;
                    periodoReferencia = 2;
                    serieReferencia = 2;
                    cursoReferencia = "0002.83";
                }

                contextQuery.Command = @" SELECT DISTINCT T.TURNO,TU.DESCRICAO
                            FROM LY_TURMA T
	                            INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
                                inner JOIN LY_TURNO TU ON TU.TURNO = T.TURNO
                            WHERE ANO = @ANO
	                            AND SEMESTRE = @PERIODO
	                            AND MODALIDADE = @MODALIDADE
	                            AND T.CURSO = @CURSO
                                AND SERIE = @SERIE
								AND T.FACULDADE = @CENSO	                            
                                AND SIT_TURMA = 'Aberta' ";



                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@MODALIDADE", SqlDbType.VarChar, modalidade);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, cursoReferencia);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serieReferencia);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, anoReferencia);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodoReferencia);

                turnos = ctx.GetDataTable(contextQuery);

                return turnos;
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

        public int RetornaQuantidadeTurmaOfertaPor(DataContext contexto, int ano, string censo, string modalidade, string turno)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;
            int periodoReferencia = -1;
            int anoReferencia;
            int serieReferencia = -1;
            string cursoReferencia;

            try
            {
                if (modalidade == "RE1") //regular
                {
                    anoReferencia = ano - 1;
                    periodoReferencia = 0;
                    serieReferencia = 1;
                    cursoReferencia = "0002.61";
                }
                else
                {
                    anoReferencia = ano - 1;
                    periodoReferencia = 2;
                    serieReferencia = 2;
                    cursoReferencia = "0002.83";
                }

                contextQuery.Command = @" SELECT COUNT(DISTINCT T.TURMA) AS TURMAS
                            FROM LY_TURMA T
	                            INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
                                inner JOIN LY_TURNO TU ON TU.TURNO = T.TURNO
                            WHERE ANO = @ANO
	                            AND SEMESTRE = @PERIODO
	                            AND MODALIDADE = @MODALIDADE
	                            AND T.CURSO = @CURSO
                                AND SERIE = @SERIE	
								AND T.FACULDADE = @CENSO 
								AND T.TURNO = @TURNO                           
                                AND SIT_TURMA = 'Aberta' ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@MODALIDADE", SqlDbType.VarChar, modalidade);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, cursoReferencia);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serieReferencia);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, anoReferencia);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodoReferencia);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["TURMAS"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public string ObtemTurmaPor(decimal gradeId)
        {
            string turma = string.Empty;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  GRADE 
                                        FROM   ly_GRADE_SERIE (nolock)                                             
                                        WHERE  GRADE_ID =  @GRADEID")
                };

                contextQuery.Parameters.Add("@GRADEID", gradeId);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    turma = Convert.ToString(reader["GRADE"]);
                }

                return turma;
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
        public List<string> ListaTurnosOfertaPor(DataContext contexto, int ano, string censo, string modalidade)
        {
            List<string> turnos = new List<string>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int periodoReferencia = -1;
            int anoReferencia;
            int serieReferencia = -1;
            string cursoReferencia;

            try
            {
                if (modalidade == "RE1") //regular
                {
                    anoReferencia = ano - 1;
                    periodoReferencia = 0;
                    serieReferencia = 1;
                    cursoReferencia = "0002.61";
                }
                else
                {
                    anoReferencia = ano - 1;
                    periodoReferencia = 2;
                    serieReferencia = 2;
                    cursoReferencia = "0002.83";
                }

                contextQuery.Command = @" SELECT DISTINCT T.TURNO,TU.DESCRICAO
                            FROM LY_TURMA T
	                            INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
                                inner JOIN LY_TURNO TU ON TU.TURNO = T.TURNO
                            WHERE ANO = @ANO
	                            AND SEMESTRE = @PERIODO
	                            AND MODALIDADE = @MODALIDADE
	                            AND T.CURSO = @CURSO
                                AND SERIE = @SERIE
								AND T.FACULDADE = @CENSO	                            
                                AND SIT_TURMA = 'Aberta' ";



                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@MODALIDADE", SqlDbType.VarChar, modalidade);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, cursoReferencia);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serieReferencia);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, anoReferencia);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodoReferencia);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    turnos.Add(Convert.ToString(reader["TURNO"]));
                }

                return turnos;
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
        public DataTable ListaTurmaReposiçãoPor(int ano, int periodo, string unidade, DateTime dataGreve)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;

            try
            {
                //Verifica dia da semana
                int diaSemana = (int)dataGreve.Date.DayOfWeek + 1; //Dia da semana no banco começa em 1 (domingo) o DayOfWeek começa em 0 (domingo);

                contextQuery.Command = @"SELECT DISTINCT T.TURMA , T.TURMA + ';'+ convert(varchar,T.SERIE) + ';' + TU.DESCRICAO + ';' + CU.NOME AS DADOSTURMA 
                                        FROM   DBO.LY_TURMA T 
                                        INNER JOIN Reposicao.LY_AULA_DOCENTE_CONGELADA A ON A.ANO = T.ANO AND A.SEMESTRE = T.SEMESTRE AND A.TURMA=T.TURMA AND T.DISCIPLINA=A.DISCIPLINA
                                        INNER JOIN LY_CURSO CU ON CU.CURSO = T.CURSO
                                        INNER JOIN LY_TURNO TU ON TU.TURNO = T.TURNO   
                                        inner join  LY_LICENCA_DOCENTE LD ON       A.NUM_FUNC = LD.NUM_FUNC AND ld.MOTIVO = '61' AND @DATAGREVE BETWEEN LD.DTINI AND LD.DTFIM                                     
                                        WHERE  T.ANO = @ANO 
                                               AND T.SEMESTRE = @PERIODO 
                                               AND T.FACULDADE = @UNIDADE 
                                               AND A.DIA_SEMANA = @DIA_SEMANA                                          
                                        ORDER  BY T.TURMA  ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@UNIDADE", unidade);
                contextQuery.Parameters.Add("@DATAGREVE", SqlDbType.DateTime, dataGreve.Date);
                contextQuery.Parameters.Add("@DIA_SEMANA", SqlDbType.Int, diaSemana);

                turmas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return turmas;
        }


        public DataTable ListaTurmaAbertaEnsinoMedioPor(int ano, int periodo, string unidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT TURMA 
                                        FROM   DBO.LY_TURMA TU
                                        INNER JOIN LY_CURSO C ON C.CURSO = TU.CURSO
                                        WHERE  ANO = @ANO 
                                        AND SEMESTRE = @PERIODO 
                                        AND TIPO in (2,3) 
                                        AND TU.FACULDADE = @UNIDADE 
                                        AND SIT_TURMA = 'Aberta' 
                                        AND OPTATIVAREFORCO = 'N' 
                                        AND ISNULL(ELETIVA,'N') = 'N'
                                        AND EXISTS(SELECT *
		                                        FROM   LY_MATRICULA M (NOLOCK)
			                                        INNER JOIN LY_ALUNO A (NOLOCK)
					                                        ON M.ALUNO = A.ALUNO
			                                        INNER JOIN LY_PESSOA P (NOLOCK)
					                                        ON A.PESSOA = P.PESSOA
			                                        INNER JOIN DBO.LY_TURMA T (NOLOCK)
					                                        ON M.DISCIPLINA = T.DISCIPLINA
						                                        AND M.TURMA = T.TURMA
						                                        AND M.ANO = T.ANO
						                                        AND M.SEMESTRE = T.SEMESTRE
			                                        INNER JOIN TURMA.ALUNOPEDEMEIA PE WITH(INDEX(IDX_DBA_ON_Turma_ALUNOPEDEMEIA_25042025))
					                                        ON PE.ALUNO = M.ALUNO                                                                
						                                        AND ELEGIVEL = @ELEGIVEL
	                                        WHERE  M.ANO = @ANO
			                                        AND M.SEMESTRE = @PERIODO
			                                        AND T.FACULDADE = @UNIDADE 
			                                        AND M.TURMA = TU.TURMA			                                      
			                                        AND Isnull(T.eletiva, 'N') = 'N'
			                                        AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
			                                        AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N'
			                                        AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N'
			                                        AND ISNULL(M.CONCOMITANTE, 'N') = 'N' 
			                                        )
                                        ORDER  BY TURMA  ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@UNIDADE", unidade);
                contextQuery.Parameters.Add("@ELEGIVEL", int.Parse(ConfigurationManager.AppSettings["AlunoElegivel"]));

                turmas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return turmas;
        }

        public DataTable ListaTurmaAbertaFinalizadaEnsinoMedioPor(int ano, int periodo, string unidade)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turmas = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT TURMA 
                                        FROM   DBO.LY_TURMA TU
                                        INNER JOIN LY_CURSO C ON C.CURSO = TU.CURSO
                                        WHERE  ANO = @ANO 
                                        AND SEMESTRE = @PERIODO 
                                        AND TIPO IN (2,3)
                                        AND TU.FACULDADE = @UNIDADE 
                                        AND SIT_TURMA <> 'Desativada'
                                        AND OPTATIVAREFORCO = 'N' 
                                        AND ISNULL(ELETIVA,'N') = 'N'
                                        AND EXISTS(SELECT *
		                                        FROM   LY_HISTMATRICULA M (NOLOCK)
			                                        INNER JOIN LY_ALUNO A (NOLOCK)
					                                        ON M.ALUNO = A.ALUNO
			                                        INNER JOIN LY_PESSOA P (NOLOCK)
					                                        ON A.PESSOA = P.PESSOA
			                                        INNER JOIN DBO.LY_TURMA T (NOLOCK)
					                                        ON M.DISCIPLINA = T.DISCIPLINA
						                                        AND M.TURMA = T.TURMA
						                                        AND M.ANO = T.ANO
						                                        AND M.SEMESTRE = T.SEMESTRE
			                                        INNER JOIN TURMA.ALUNOPEDEMEIA PE
					                                        ON PE.ALUNO = M.ALUNO
						                                        --AND ELEGIVEL = 0
	                                        WHERE  M.ANO = @ANO
			                                        AND M.SEMESTRE = @PERIODO
			                                        AND T.FACULDADE = @UNIDADE 
			                                        AND M.TURMA = TU.TURMA			                                      
			                                        AND Isnull(T.eletiva, 'N') = 'N'
			                                        AND ISNULL(M.DEPENDENCIA, 'N') = 'N'
			                                        AND ISNULL(M.MAIS_EDUCACAO, 'N') = 'N'
			                                        AND ISNULL(M.EDUC_ESPECIAL, 'N') = 'N'
			                                        AND ISNULL(M.CONCOMITANTE, 'N') = 'N' 
			                                        )

                                        ORDER  BY TURMA  ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@UNIDADE", unidade);

                turmas = ctx.GetDataTable(contextQuery);
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
                if (ctx != null)
                    ctx.Dispose();
            }

            return turmas;
        }

        public static ErrorList ValidarHorarioMigracao(TConnection connection, Ly_hor_aula.Row dadosHoraAula, Ly_turma.Row dadosTurma, decimal num_func_2_matricula, decimal num_func, string matriculaDocente, string nomeDisciplina)
        {
            ErrorList retorno = null;

            string matricula = RN.Docentes.ObterMatricula(connection, num_func_2_matricula);

            //verifica se algum campo incluido está vazio ou inválido
            retorno = ValidarCampoVazioInvalido(dadosHoraAula, dadosTurma.Curso);
            if (retorno != null)
            {
                return retorno;
            }

            //verifica se o docente está disponível
            retorno = VerificarDisponibilidadeDocenteMigracao(connection, num_func_2_matricula, dadosHoraAula.Dia_semana.Value, dadosHoraAula.Ano.Value, dadosHoraAula.Semestre.Value, dadosHoraAula.Turma, dadosHoraAula.Horaini_aula.Value, dadosHoraAula.Horafim_aula.Value, dadosTurma.Dt_inicio.Value, dadosTurma.Dt_fim.Value, dadosHoraAula.Aula.Value, num_func, dadosHoraAula.Disciplina, nomeDisciplina);
            if (retorno != null)
            {
                return retorno;
            }

            return null;
        }

        private static ErrorList VerificarDisponibilidadeDocenteMigracao(TConnection connection, decimal num_func, decimal diaSemana, decimal ano, decimal semestre, string turma, DateTime horaIni, DateTime horaFim, DateTime dtIni, DateTime dtFim, decimal aula,
           decimal num_func_primeira, string codDisciplina_primeira, string nomeDisciplina)
        {

            string sql = @" SELECT ad.DISCIPLINA, ad.TURMA, ad.ANO, ad.SEMESTRE, ad.NUM_FUNC
                          From LY_HOR_OPER ho
                          INNER JOIN LY_AULA_DOCENTE ad ON 
                            ho.TURNO = ad.TURNO 
                            AND ho.FACULDADE = ad.FACULDADE 
                            AND ho.DIA_SEMANA = ad.DIA_SEMANA 
                            AND ho.AULA = ad.AULA 
                          INNER JOIN LY_TURMA T (NOLOCK) ON 
                            T.TURMA = ad.TURMA 
                            AND T.ANO = AD.ANO 
                            AND T.SEMESTRE = AD.SEMESTRE 
                            AND T.DISCIPLINA = AD.DISCIPLINA 
                            AND T.DT_FIM = AD.DATA_FIM                          
                          WHERE 
                            ad.DIA_SEMANA = ?
                            AND ad.ANO = ?
                            
                            AND ad.TURMA <> ?
                            AND ho.HORAFIM_AULA > ?
                            AND ho.HORAINI_AULA < ?
                            AND ad.NUM_FUNC = ?
                            AND ((ad.DATA_INICIO >= ? AND ad.DATA_INICIO <= ?)
                            OR (ad.DATA_FIM >= ? AND ad.DATA_FIM <= ? )
                            OR (ad.DATA_INICIO <= ? AND ad.DATA_FIM >= ? ))
                            AND t.sit_turma = 'Aberta'
                            AND (ad.DATA_FIM >= GETDATE())";

            var strHoraIni = new DateTime(1899, 12, 30, horaIni.Hour, horaIni.Minute, horaIni.Second);
            var strHoraFim = new DateTime(1899, 12, 30, horaFim.Hour, horaFim.Minute, horaFim.Second);
            var strDtIni = new DateTime(dtIni.Year, dtIni.Month, dtIni.Day, 0, 0, 0);
            var strDtFim = new DateTime(dtFim.Year, dtFim.Month, dtFim.Day, 0, 0, 0);

            QueryTable qt = new QueryTable(sql); //, semestre
            qt.Query(connection, diaSemana, ano, turma, strHoraIni, strHoraFim, num_func,
                strDtIni, strDtFim, strDtIni, strDtFim, strDtIni, strDtFim);

            if (qt != null && qt.Rows.Count > 0)
            {
                ErrorList erro = new ErrorList();
                erro.Add(string.Format("Conflito de horário na {0} na disciplina {1} e no horário de {2} a {3} ", ObterDiaSemana(diaSemana), nomeDisciplina, String.Format("{0:HH:mm}", horaIni), String.Format("{0:HH:mm}", horaFim))
, "ERRO_VALIDACAO");
                return erro;
            }
            return null;
        }


        public int ObtemAlunosMatriculadosEdEspecialPor(string Turma, string Ano, string Semestre)
        {
            int total = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  COUNT(DISTINCT M.ALUNO) AS TOTAL
                                        FROM   ly_turma t (nolock)
                                               INNER JOIN ly_matricula m (nolock)
                                                       ON ( m.disciplina = t.disciplina
                                                            AND m.turma = t.turma
                                                            AND m.ano = t.ano
                                                            AND m.semestre = t.semestre )
                                        WHERE  m.sit_matricula = 'Matriculado'
                                               AND t.turma = @TURMA
                                               AND t.ano = @ANO
                                               AND t.semestre = @SEMESTRE ")
                };

                contextQuery.Parameters.Add("@TURMA", Turma);
                contextQuery.Parameters.Add("@ANO", Ano);
                contextQuery.Parameters.Add("@SEMESTRE", Semestre);


                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["TOTAL"]);
                }

                return total;
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

        public DataTable ListaTurmaLetramentoPor(int ano, int periodo, string turno, string censo, string segmento)
        {
            DataTable turmas = null;
            ContextQuery contextQuery = new ContextQuery();


            try
            {
                if (ano > 0 && periodo >= 0 && !string.IsNullOrEmpty(censo) && !string.IsNullOrEmpty(segmento) && !string.IsNullOrEmpty(turno))
                {

                    contextQuery.Command = @"SELECT t.TURMA,
                                            MAX(NUM_ALUNOS) AS 'MAXIMO_ALUNOS',
                                            COUNT(DISTINCT mat.ALUNO) AS 'ALUNOS_MATRICULADOS',
                                            MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) AS 'VAGAS'
                                    FROM    dbo.LY_TURMA t
                                            LEFT JOIN ly_matricula mat ON mat.DISCIPLINA = t.DISCIPLINA
                                                                          AND mat.TURMA = t.TURMA
                                                                          AND mat.ANO = t.ANO
                                                                          AND mat.SEMESTRE = t.SEMESTRE
                                                                          AND mat.SIT_MATRICULA <> 'Cancelado'                                                          
                                                                          AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)
                                    WHERE   t.ANO = @ANO
							                AND t.SEMESTRE = @PERIODO
							                AND t.FACULDADE = @CENSO
							                AND t.TURNO = @TURNO							              
                                            AND t.SIT_TURMA = 'Aberta' ";

                    if (segmento == "3")//MEDIO
                    {
                        contextQuery.Command += @" AND T.CURSO = '2025.03'";
                    }
                    else
                    {
                        contextQuery.Command += @" AND T.CURSO = '2025.02'";
                    }

                    contextQuery.Command += @"							              
                                            
                                    GROUP BY t.TURMA
                                    HAVING  MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) > 0
                                    ORDER BY t.TURMA ASC";



                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@TURNO", turno);
                    contextQuery.Parameters.Add("@CENSO", censo);


                    turmas = Consultar(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            return turmas;
        }


        public DataTable ListaTurmaNOAPor(int ano, int periodo, string turno, string censo, string segmento)
        {
            DataTable turmas = null;
            ContextQuery contextQuery = new ContextQuery();


            try
            {
                if (ano > 0 && periodo >= 0 && !string.IsNullOrEmpty(censo) && !string.IsNullOrEmpty(segmento) && !string.IsNullOrEmpty(turno))
                {

                    contextQuery.Command = @"SELECT t.TURMA,
                                            MAX(NUM_ALUNOS) AS 'MAXIMO_ALUNOS',
                                            COUNT(DISTINCT mat.ALUNO) AS 'ALUNOS_MATRICULADOS',
                                            MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) AS 'VAGAS'
                                    FROM    dbo.LY_TURMA t
                                            LEFT JOIN ly_matricula mat ON mat.DISCIPLINA = t.DISCIPLINA
                                                                          AND mat.TURMA = t.TURMA
                                                                          AND mat.ANO = t.ANO
                                                                          AND mat.SEMESTRE = t.SEMESTRE
                                                                          AND mat.SIT_MATRICULA <> 'Cancelado'                                                          
                                                                          AND (mat.DEPENDENCIA <> 'S' OR mat.DEPENDENCIA IS NULL)
                                    WHERE   t.ANO = @ANO
							                AND t.SEMESTRE = @PERIODO
							                AND t.FACULDADE = @CENSO
							                AND t.TURNO = @TURNO							              
                                            AND t.SIT_TURMA = 'Aberta' ";

                    if (segmento == "3")//MEDIO
                    {
                        contextQuery.Command += @" AND T.CURSO = '2025.05'";
                    }
                    else
                    {
                        contextQuery.Command += @" AND T.CURSO = '2025.04'";
                    }

                    contextQuery.Command += @"							              
                                            
                                    GROUP BY t.TURMA
                                    HAVING  MAX(NUM_ALUNOS) - COUNT(DISTINCT mat.ALUNO) > 0
                                    ORDER BY t.TURMA ASC";



                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@TURNO", turno);
                    contextQuery.Parameters.Add("@CENSO", censo);


                    turmas = Consultar(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            return turmas;
        }


        public bool ExisteTurmaCadastrada(string curriculo, string curso, string turno, int serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    DBO.LY_TURMA T                              
                        WHERE T.CURRICULO = @CURRICULO
                                AND T.CURSO = @CURSO
                                AND T.TURNO = @TURNO
                                AND T.SERIE = @SERIE ";

                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
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

        public bool ExisteTurmaCadastradaPor(string curriculo, string curso, string turno, string disciplina)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    DBO.LY_TURMA T                              
                        WHERE T.CURRICULO = @CURRICULO
                                AND T.CURSO = @CURSO
                                AND T.TURNO = @TURNO
                                AND T.DISCIPLINA = @DISCIPLINA ";

                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
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
    }
}