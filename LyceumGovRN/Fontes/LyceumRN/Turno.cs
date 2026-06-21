using System;
using System.Text;
using Techne.Data;
using Techne.Lyceum.CR;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class Turno : RNBase
    {
        public string RetornaDescricaoTurno(string primeiraLetra)
        {
            switch (primeiraLetra)
            {
                case "M": return "Manhã";
                case "T": return "Tarde";
                case "N": return "Noite";
                case "A": return "Ampliado";
                case "I": return "Integral";
                default:
                    return String.Empty;
            }
        }

        public static QueryTable Consultar()
        {
            string sql = "SELECT TURNO, DESCRICAO FROM LY_TURNO ORDER BY DESCRICAO";
            return RNBase.Consultar(sql);
        }

        public static QueryTable ConsultarPorUnidadeEnsino(string unidade_ens)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            sql.Append(" SELECT distinct t.TURNO, t.DESCRICAO from ly_turno T ");
            sql.Append(" inner join ly_unidade_ensino_cursos uec on uec.turno = t.turno and unidade_ens = ? ");
            sql.Append(" ORDER BY t.DESCRICAO ");

            return RNBase.Consultar(sql.ToString(), unidade_ens);
        }

        public static QueryTable ConsultarPorUnidadeEnsinoECurso(string unidade_ensino, string curso)
        {
            String sql = @"
                SELECT distinct 
                    t.turno,    
                    t.descricao 
                FROM ly_turno t
                INNER JOIN ly_unidade_ensino_cursos uec 
                    ON uec.turno = t.turno 
                    AND uec.unidade_ens = ? AND uec.curso = ?";
            return RNBase.Consultar(sql, unidade_ensino, curso);
        }

        public static QueryTable ComboConsultar()
        {
            string sql = "SELECT turno, descricao FROM LY_TURNO UNION SELECT '' turno, ' <NENHUM>' descricao ORDER BY DESCRICAO";
            return RNBase.Consultar(sql);
        }

        public static QueryTable ComboConsultar(DbObject unidade_ens, DbObject curso)
        {
            string sql = "SELECT t.turno, t.descricao FROM LY_TURNO t inner join ly_unidade_ensino_cursos uec on t.turno = uec.turno where uec.unidade_ens = ? and uec.curso = ? UNION SELECT '' turno, ' <NENHUM>' descricao ORDER BY DESCRICAO";
            return RNBase.Consultar(sql, unidade_ens, curso);
        }

        public static void ReplicarTurnoEmCurriculo(TConnectionWritable tconnw, string turno)
        {
            string sql = "DECLARE @TURNO varchar(20) " +
                        "set @TURNO = ? " +
                        "insert into LY_CURRICULO  " +
                        "Select CURSO, @TURNO, CURRICULO, ANO_INI, SEM_INI, DT_HOMOLOG, DT_EXTINCAO,  " +
                        "REGIME, AULAS_PREVISTAS, CREDITOS, PRAZO_IDEAL, PRAZO_MAX, CREDMIN_MATR, TRANC_MAX,  " +
                        "TRANC_CONS_MAX, TRANC_MAX_DISCIP, CANC_MAX_DISCIP, ATLZ_MAX_DISCIP, RETEM_SERIE,  " +
                        "OBS, RATEAR_MENS, PERMITE_CANCELAMENTO, EXCECAO_TRANCAMENTO, PRAZO_MAX_ADAP, UNID_PRAZO_MAX_ADAP,  " +
                        "MATR_OBRIG_TODAS_DISCIP_SERIE, CREDMIN_FORAGRADE, CREDMAX_FORAGRADE, MODALIDADE,  " +
                        "ATIV_COMPL_CH, ATIV_COMPL_CREDITOS, CREDMIN_MATR, HABILINEP, TRANCA_PRIMEIRO_PERIODO, " +
                        "PERC_CH_PRES, PERC_CH_SEMI_PRES, PERC_CH_NAO_PRES, RATEAR_DESC, SERVICO, TRANC_INTERV_DATA, " +
                        "CLASSIFICACAO, CODIGO_SECUNDARIO, NOME_SECUNDARIO, MIN_ALUNOS, TESE_DISSERTACAO, PESQUISA, " +
                        "TIPO_PRAZO_CONCL, PRAZO_CONC_PREV, EMP_CONTATO, EMP_ENDERECO, EMP_END_NUM, EMP_END_COMPL, " +
                        "EMP_BAIRRO, EMP_MUNICIPIO, EMP_CEP, EMP_CNPJ, EMP_FONE, TIPO_PRAZO_ORIEN, PRAZO_DESIG_ORIEN, " +
                        "SERIE_MAX_ORIENT, N_MAX_DIAS_TRANC, INDICE, VALOR, STAMP_ATUALIZACAO " +
                        "from LY_CURRICULO where TURNO = (Select top 1 TURNO from LY_CURRICULO)";

            IAE(tconnw, sql, turno);
        }

        public static void ReplicarTurnoEmGrade(TConnectionWritable tconnw, string turno)
        {
            string sql = "DECLARE @TURNO varchar(20) " +
                        "set @TURNO = ?  " +
                        "insert into ly_grade   " +
                        "Select CURSO, @TURNO, CURRICULO, DISCIPLINA, SERIE_PREREQ, SERIE_IDEAL, OBRIGATORIA, FORMULA_PREREQ,  " +
                        "FORMULA_EQUIV, RETEM_SERIE, MAX_MATR_APROV, MAX_REPROV, DISCIPLINA_EXTENSIVA, NOME_EXIBICAO, COMPLEMENTO, " +
                        "AULAS_SEMANAIS, AULAS_SEM_AULA, AULAS_SEM_LAB, AULAS_SEM_ATIV, TESE_DISSERTACAO, PERMITE_GLP, STAMP_ATUALIZACAO " +
                        "from ly_grade where TURNO = (Select top 1 TURNO from ly_grade)";

            IAE(tconnw, sql, turno);
        }

        public static void ReplicarTurnoEmGradeSerie(TConnectionWritable tconnw, string turno)
        {
            string sql = "DECLARE @TURNO varchar(20) " +
                        "set @TURNO = ?  " +
                        "insert into LY_GRADE_SERIE " +
                        "Select DISTINCT CURSO, @TURNO, CURRICULO, SERIE, ANO, SEMESTRE, GRADE,  " +
                        "CAPACIDADE, NUM_FUNC, FACULDADE, DEPENDENCIA, DT_INICIO, DT_FIM, FORMULA_MF1,  " +
                        "FORMULA_MF3, FORMULA_MF2, FORMULA_CA1, FORMULA_CA2, FORMULA_CA3, CONCEITO_MIN_1,  " +
                        "CONCEITO_MIN_2, CONCEITO_MIN_3, CONCEITO_MIN_EX, CONCEITO_MIN_EX_2, ULT_NUM_CHAMADA, UNIDADE_RESPONSAVEL, DATA_NUMERACAO " +
                        "from LY_GRADE_SERIE where TURNO = (Select top 1 TURNO from LY_GRADE_SERIE)";

            IAE(tconnw, sql, turno);
        }

        public static void ReplicarTurnoEmSerie(TConnectionWritable tconnw, string turno)
        {
            string sql = "DECLARE @TURNO varchar(20) " +
                        "set @TURNO = ?  " +
                        "insert into LY_SERIE   " +
                        "Select CURSO, @TURNO, CURRICULO, SERIE, DESCRICAO, SERVICO, SERVICO_CRED, ATLZ_MAX_DISCIP,  " +
                        "COMPLEMENTO1, COMPLEMENTO2, IDADE_MINIMA, DIA_ANIV, MES_ANIV, CONSOLIDADA, STAMP_ATUALIZACAO, DT_EXTINCAO " +
                        "from LY_SERIE where TURNO = (Select top 1 TURNO from LY_SERIE)";

            IAE(tconnw, sql, turno);
        }

        public static void Replicar(string turno)
        {
            TConnectionWritable tconnw = Config.CreateWritableConnection();

            try
            {
                tconnw.Open(true);
                ReplicarEmTudo(tconnw, turno);

                RetValue retorno = VerificarErro(tconnw.GetErrors());
                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        tconnw.Rollback();
                    }
                }
            }
            catch (Exception exc)
            {
                tconnw.Rollback();
                throw exc;
            }
            finally
            {
                tconnw.Close();
            }
        }

        private static void ReplicarEmTudo(TConnectionWritable tconnw, string turno)
        {
            StringBuilder sql = new StringBuilder(@"DECLARE @TURNO varchar(20) set @TURNO = ? ");

            //Replicar em Currículo
            sql.Append(@"insert into LY_CURRICULO  
                        Select CURSO, @TURNO, CURRICULO, ANO_INI, SEM_INI, DT_HOMOLOG, DT_EXTINCAO,  
                        REGIME, AULAS_PREVISTAS, CREDITOS, PRAZO_IDEAL, PRAZO_MAX, CREDMIN_MATR, TRANC_MAX,  
                        TRANC_CONS_MAX, TRANC_MAX_DISCIP, CANC_MAX_DISCIP, ATLZ_MAX_DISCIP, RETEM_SERIE,  
                        OBS, RATEAR_MENS, PERMITE_CANCELAMENTO, EXCECAO_TRANCAMENTO, PRAZO_MAX_ADAP, UNID_PRAZO_MAX_ADAP,  
                        MATR_OBRIG_TODAS_DISCIP_SERIE, CREDMIN_FORAGRADE, CREDMAX_FORAGRADE, MODALIDADE,  
                        ATIV_COMPL_CH, ATIV_COMPL_CREDITOS, CREDMIN_MATR, HABILINEP, TRANCA_PRIMEIRO_PERIODO, 
                        PERC_CH_PRES, PERC_CH_SEMI_PRES, PERC_CH_NAO_PRES, RATEAR_DESC, SERVICO, TRANC_INTERV_DATA, 
                        CLASSIFICACAO, CODIGO_SECUNDARIO, NOME_SECUNDARIO, MIN_ALUNOS, TESE_DISSERTACAO, PESQUISA, 
                        TIPO_PRAZO_CONCL, PRAZO_CONC_PREV, EMP_CONTATO, EMP_ENDERECO, EMP_END_NUM, EMP_END_COMPL, 
                        EMP_BAIRRO, EMP_MUNICIPIO, EMP_CEP, EMP_CNPJ, EMP_FONE, TIPO_PRAZO_ORIEN, PRAZO_DESIG_ORIEN, 
                        SERIE_MAX_ORIENT, N_MAX_DIAS_TRANC, INDICE, VALOR, STAMP_ATUALIZACAO 
                        from LY_CURRICULO where TURNO = (Select top 1 TURNO from LY_CURRICULO) ");

            //Replicar em Serie
            sql.Append(@"insert into LY_SERIE 
                        Select CURSO, @TURNO, CURRICULO, SERIE, DESCRICAO, SERVICO, SERVICO_CRED, ATLZ_MAX_DISCIP,  
                        COMPLEMENTO1, COMPLEMENTO2, IDADE_MINIMA, DIA_ANIV, MES_ANIV, CONSOLIDADA, STAMP_ATUALIZACAO, DT_EXTINCAO 
                        from LY_SERIE where TURNO = (Select top 1 TURNO from LY_SERIE) ");

            //Replicar em Grade Série
            sql.Append(@"insert into LY_GRADE_SERIE
                        Select DISTINCT CURSO, @TURNO, CURRICULO, SERIE, ANO, SEMESTRE, GRADE,  
                        CAPACIDADE, NUM_FUNC, FACULDADE, DEPENDENCIA, DT_INICIO, DT_FIM, FORMULA_MF1,  
                        FORMULA_MF3, FORMULA_MF2, FORMULA_CA1, FORMULA_CA2, FORMULA_CA3, CONCEITO_MIN_1,  
                        CONCEITO_MIN_2, CONCEITO_MIN_3, CONCEITO_MIN_EX, CONCEITO_MIN_EX_2, ULT_NUM_CHAMADA, UNIDADE_RESPONSAVEL, DATA_NUMERACAO 
                        from LY_GRADE_SERIE where TURNO = (Select top 1 TURNO from LY_GRADE_SERIE) ");

            //Replicar em Grade
            sql.Append(@"insert into ly_grade 
                        Select CURSO, @TURNO, CURRICULO, DISCIPLINA, SERIE_PREREQ, SERIE_IDEAL, OBRIGATORIA, FORMULA_PREREQ,  
                        FORMULA_EQUIV, RETEM_SERIE, MAX_MATR_APROV, MAX_REPROV, DISCIPLINA_EXTENSIVA, NOME_EXIBICAO, COMPLEMENTO, 
                        AULAS_SEMANAIS, AULAS_SEM_AULA, AULAS_SEM_LAB, AULAS_SEM_ATIV, TESE_DISSERTACAO, PERMITE_GLP, STAMP_ATUALIZACAO 
                        from ly_grade where TURNO = (Select top 1 TURNO from ly_grade) ");

            IAE(tconnw, sql.ToString(), turno);
        }

        public static QueryTable ConsultarTurno(string unidadeEns, string curso)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            string sql = "SELECT distinct t.turno, t.descricao FROM LY_TURNO t inner join ly_unidade_ensino_cursos uec on t.TURNO = uec.TURNO where uec.UNIDADE_ENS = ? and uec.CURSO = ?";

            return Consultar(sql, unidadeEns, curso);
        }

        public static QueryTable ConsultarTurnoSimples(string unidadeEns, string curso)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            string sql = "SELECT distinct t.turno, t.descricao FROM LY_TURNO t inner join ly_unidade_ensino_cursos uec on t.TURNO = uec.TURNO where uec.UNIDADE_ENS = ? and uec.CURSO = ? AND t.TURNO <> 'A' AND t.TURNO <> 'I' ";

            return Consultar(sql, unidadeEns, curso);
        }

        public static QueryTable ConsultarTurnoSerie(string curso)
        {
            string sql = "select distinct tr.TURNO as turno, tr.DESCRICAO from LY_CURRICULO cr INNER JOIN LY_TURNO tr ON cr.TURNO = cr.TURNO where CURSO = ?";

            return Consultar(sql, curso);
        }

        public static int ConsultaTurno(string turno)
        {
            string sql = "select 1 from ly_turno where turno = ?";
            return ExecutarFuncao(sql, turno);
        }

        public static Ly_turno.Row Consultar(String turno)
        {
            if (String.IsNullOrEmpty(turno)) return null;

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                return Ly_turno.Row.Query(connection, turno);
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarTurnoPorUEDeCurriculo(String curso)
        {
            if (!string.IsNullOrEmpty(curso))
                return Consultar(@"
                    SELECT distinct c.turno as turno, t.descricao as descricao FROM ly_curriculo c
                    INNER JOIN ly_turno t ON t.turno = c.turno
                    WHERE c.curso = ?
                    ORDER BY t.descricao", curso);
            else
                return Consultar();
        }

        public DataTable ListaTurnosPor(string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                    C.TURNO ,
                                    T.DESCRICAO
                            FROM    LY_CURRICULO C
                                    INNER JOIN LY_TURNO T ON T.TURNO = C.TURNO
                            WHERE   C.CURSO = @CURSO
                            ORDER BY T.DESCRICAO ";

                contextQuery.Parameters.Add("@CURSO", curso);

                turnos = ctx.GetDataTable(contextQuery);
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

            return turnos;
        }

        public static System.Data.DataTable ListarTurnosPorUE(string censo, string curso, int serie)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT LC.TURNO,T.DESCRICAO
                                        FROM  LY_UNIDADE_ENSINO_CURSOS uc
                                                JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                                        JOIN LY_SERIE LS ON ls.CURSO=c.CURSO
                                        INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO AND LC.CURSO = LS.CURSO
                                        JOIN LY_TURNO t ON LC.TURNO = t.TURNO
                                        WHERE uc.UNIDADE_ENS = @CENSO
                                        AND LS.CURSO = @CURSO 
                                        AND LS.SERIE= @SERIE
                                        AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE()))
                                        ORDER BY T.DESCRICAO";

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@SERIE", serie);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static System.Data.DataTable ListarTurnosPor(string censo, string curso, int serie, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT
                                LC.TURNO ,
                                T.DESCRICAO
                        FROM    LY_UNIDADE_ENSINO_CURSOS uc
                                JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                                JOIN LY_SERIE LS ON ls.CURSO = c.CURSO
                                INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO
                                                              AND LC.CURSO = LS.CURSO
                                JOIN LY_TURNO t ON LC.TURNO = t.TURNO
                        WHERE   uc.UNIDADE_ENS = @CENSO
                                AND LS.CURSO = @CURSO
                                AND LS.SERIE = @SERIE
                                AND ( LS.DT_EXTINCAO IS NULL
                                      OR CONVERT(DATE, LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE())
                                    )
                                AND LC.ANO_INI = @ANO
                                AND LC.SEM_INI = @PERIODO
                        ORDER BY T.DESCRICAO ";

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static System.Data.DataTable ListarTurnosPorTurmaUE(string censo, string curso, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT TU.TURNO,TU.DESCRICAO
                        FROM LY_CURSO C 
                        INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO 
                        INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                        INNER JOIN LY_TURMA T ON C.CURSO = t.CURSO 
                        INNER JOIN dbo.LY_TURNO TU ON TU.TURNO=T.TURNO
                       WHERE t.FACULDADE = @CENSO                      
                        AND T.ANO = @ANO
                        AND T.SEMESTRE= @PERIODO
                        AND T.CURSO = @CURSO
                    ORDER BY TU.DESCRICAO     ";

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public DataTable ListaTurnosOptativaReforcoPor(string censo, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT
                                            TU.TURNO ,
                                            TU.DESCRICAO
                                    FROM    LY_TURMA T
                                            INNER JOIN DBO.LY_TURNO TU ON TU.TURNO = T.TURNO
                                    WHERE   T.FACULDADE = @CENSO
                                            AND T.ANO = @ANO
                                            AND T.SEMESTRE = @PERIODO
                                            AND NOT ( T.OPTATIVAREFORCO = 'N' )
                                    ORDER BY TU.DESCRICAO     ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public DataTable ListaTurnosContraTurnoPor(string censo, int ano, int periodo, string Turno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"	SELECT DISTINCT
                                            TU.TURNO ,
                                            TU.DESCRICAO
                                    FROM    LY_TURMA T
                                            INNER JOIN DBO.LY_TURNO TU ON TU.TURNO = T.TURNO
                                    WHERE   T.FACULDADE = @CENSO
                                            AND T.ANO = @ANO
                                            AND T.SEMESTRE = @PERIODO
                                            AND NOT ( TU.DESCRICAO = @TURNO OR TU.DESCRICAO = 'INTEGRAL' OR TU.DESCRICAO = 'NOITE')
                                    ORDER BY TU.DESCRICAO     ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TURNO", Turno);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public DataTable ListaTurnosContraTurnoLetramentoPor(string turno)
        {
            DataTable table = new DataTable();
            table.Columns.Add("TURNO").DataType = typeof(string);
            table.Columns.Add("DESCRICAO").DataType = typeof(string);

            switch (turno.ToUpper())
            {
                case "MANHÃ":
                    table.Rows.Add("T", "TARDE");
                    break;
                case "TARDE":
                    table.Rows.Add("M", "MANHÃ");
                    break;
                case "INTEGRAL":
                    table.Rows.Add("M", "MANHÃ");
                    table.Rows.Add("T", "TARDE");
                    break;
                case "NOITE":
                    table.Rows.Add("T", "TARDE");
                    break;
            }            

            return table;
        }

        public static bool VerificarContraTurno(string turnoAnalisado, string turnoReferencia)
        {
            if (turnoAnalisado == turnoReferencia)
            {
                return false;
            }

            if ((turnoAnalisado.Equals("M")) && (turnoReferencia.Equals("I") || turnoReferencia.Equals("A")))
            {
                return false;
            }

            if ((turnoAnalisado.Equals("T")) && (turnoReferencia.Equals("I") || turnoReferencia.Equals("A")))
            {
                return false;
            }

            if ((turnoAnalisado.Equals("I")) && (turnoReferencia.Equals("M") || turnoReferencia.Equals("T") || turnoReferencia.Equals("A")))
            {
                return false;
            }

            if ((turnoAnalisado.Equals("A")) && (turnoReferencia.Equals("M") || turnoReferencia.Equals("T") || turnoReferencia.Equals("I")))
            {
                return false;
            }

            return true;
        }

        public int ObtemTotalTurnos()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            int totalTurnos = 0;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT COUNT(*)                                   
                            FROM LY_TURNO "
                };

                totalTurnos = ctx.GetReturnValue<int>(contextQuery);

                return totalTurnos;
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

        public string[] ObtemHoraInicioFimPor(string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            string[] horarios = new string[2];

            try
            {
                contextQuery.Command = @" SELECT  TURNO ,
                                                HORAINICIO ,
                                                HORAFIM
                                        FROM    LY_TURNO
                                        WHERE   TURNO = @TURNO ";

                contextQuery.Parameters.Add("@TURNO", turno);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    horarios[0] = Convert.ToString(reader["HORAINICIO"]).Substring(0, 5);
                    horarios[1] = Convert.ToString(reader["HORAFIM"]).Substring(0, 5);
                }

                return horarios;
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

        public DataTable ListaTurnosPor()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @" SELECT  TURNO ,
                                DESCRICAO
                        FROM    LY_TURNO
                        ORDER BY DESCRICAO ";

                turnos = ctx.GetDataTable(contextQuery);
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

            return turnos;
        }

        public DataTable listaTurnoAgendaTurnoVagaPor(int ano, int perfilId, string curso, int serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @"SELECT  DISTINCT
                                                TI.TURNO, T.DESCRICAO 
                                        FROM    DBO.TCE_CTV_CONF_TURNO_INICIAL TI
                                                INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA TV ON TI.ID_AGENDA_CONF_TURNO_VAGA = TV.ID_AGENDA_CONF_TURNO_VAGA
                                                INNER JOIN DBO.LY_CURSO C ON TV.CURSO = C.CURSO
                                                INNER JOIN DBO.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                                                INNER JOIN LY_TURNO T ON T.TURNO = TI.TURNO
                                                INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                                                LEFT JOIN DBO.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                                                LEFT JOIN HADES.DBO.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                                        WHERE   ANO = @ANO
                                                AND (PERFILID = @PERFILID OR @PERFILID = 7) -- 7 = DIESP QUE NÃO TEM TRATAMENTO DE PERFIL POR MODALIDADE
                                                AND TV.CURSO = @CURSO
                                                AND TV.SERIE = @SERIE
                                        ORDER BY  T.DESCRICAO";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERFILID", perfilId);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SERIE", serie);

                turnos = ctx.GetDataTable(contextQuery);
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

            return turnos;
        }

        public DataTable ListaTurnoPor(string unidade, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT T.TURNO, 
                                                        T.DESCRICAO 
                                        FROM   LY_TURNO T 
                                               INNER JOIN LY_UNIDADE_ENSINO_CURSOS UEC 
                                                       ON T.TURNO = UEC.TURNO 
                                        WHERE  UEC.UNIDADE_ENS = @UNIDADE 
                                               AND UEC.CURSO = @CURSO  ";

                contextQuery.Parameters.Add("@UNIDADE", unidade);
                contextQuery.Parameters.Add("@CURSO", curso);

                turnos = ctx.GetDataTable(contextQuery);
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

            return turnos;
        }

        public DataTable ListaTurnoMatrizAtivaPor(int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                        C.TURNO ,T.DESCRICAO
                                FROM    LY_GRADE G
                                        INNER JOIN LY_CURRICULO C ON G.CURRICULO = C.CURRICULO
                                                                     AND G.CURSO = C.CURSO
                                                                     AND G.TURNO = C.TURNO
                                        INNER JOIN LY_TURNO T ON T.TURNO = C.TURNO
                                WHERE   ANO_INI = @ANO
                                        AND SEM_INI = @PERIODO
                                        AND ( DT_EXTINCAO IS NULL
                                              OR CONVERT(DATE, DT_EXTINCAO) > CONVERT(DATE, GETDATE())
                                            )
                                        
                                ORDER BY C.TURNO ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                turnos = ctx.GetDataTable(contextQuery);
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

            return turnos;
        }

        public DataTable ListaTurnoEducEspecialPor(string unidade, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @"SELECT DISTINCT T.TURNO , 
                                                        T.DESCRICAO 
                                        FROM   LY_TURNO T 
                                               INNER JOIN LY_UNIDADE_ENSINO_CURSOS UEC 
                                                       ON T.TURNO = UEC.TURNO 
                                        WHERE  UEC.UNIDADE_ENS = @UNIDADE 
                                               AND UEC.CURSO = @CURSO 
                                               AND T.TURNO <> 'A' 
                                               AND T.TURNO <> 'I'   ";

                contextQuery.Parameters.Add("@UNIDADE", unidade);
                contextQuery.Parameters.Add("@CURSO", curso);

                turnos = ctx.GetDataTable(contextQuery);
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

            return turnos;
        }

        public DataTable ListaTurnosRotaTransportePor()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @" SELECT  TURNO ,
                                DESCRICAO
                        FROM    LY_TURNO
                        WHERE TURNO <> 'A'
                        ORDER BY DESCRICAO ";

                turnos = ctx.GetDataTable(contextQuery);
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

            return turnos;
        }

        public DataTable ListaTurnosAtivosPor(string censo, int ano)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"SELECT DISTINCT
                                            TU.TURNO ,
                                            TU.DESCRICAO
                                    FROM    LY_TURMA T
                                            INNER JOIN DBO.LY_TURNO TU ON TU.TURNO = T.TURNO
                                    WHERE   T.FACULDADE = @CENSO
                                            AND T.ANO = @ANO                                            
                                            AND SIT_TURMA = 'Aberta' 
                                               AND OPTATIVAREFORCO = 'N' 
                                               AND ISNULL(ELETIVA,'N') = 'N'
											   AND CURSO NOT IN ('0001.27','0002.37','9999.99','9999.89','9999.81','9999.82','9999.83','9999.84','9999.85','9999.86','9999.87','9999.88')
                                               AND T.FACULDADE NOT IN ('33019665','33021643', '33139245','33040060', '33042390',
                                                                         '33017999','33149380', '33145199','33096848', '33138834',
                                                                         '33100250','33139830', '33057982','33098972', '33125295',
                                                                         '33055300','33088691', '33138753','33062889', '33067678',
                                                                         '33097925','33138575', '33062862','33075042', '33085404',
                                                                         '33084025','33048274', '33142297','33099774', '33015163',
                                                                         '33025088','33119740', '33138699', '33027080','33139970',
                                                                         '33159602')
                                    ORDER BY TU.DESCRICAO     ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);

                return ctx.GetDataTable(contextQuery);
            }
        }


        public DataTable ListaTurnosAtivosPor(string censo, int ano, int periodo, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                            TU.TURNO ,
                                            TU.DESCRICAO
                                    FROM    LY_TURMA T
                                            INNER JOIN DBO.LY_TURNO TU ON TU.TURNO = T.TURNO
                                    WHERE   T.FACULDADE = @CENSO
                                            AND T.ANO = @ANO  
                                            AND T.SEMESTRE = @SEMESTRE    
                                            AND T.CURSO = @CURSO                                            
                                            AND SIT_TURMA = 'Aberta'                                     
                                    ORDER BY TU.DESCRICAO ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);

                turnos = ctx.GetDataTable(contextQuery);
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

            return turnos;

        }

        public DataTable ListaTurnosContraTurnoPor(string censo, int ano, string turno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = @"	SELECT DISTINCT
                                            TU.TURNO ,
                                            TU.DESCRICAO
                                    FROM    LY_TURMA T
                                            INNER JOIN DBO.LY_TURNO TU ON TU.TURNO = T.TURNO
                                    WHERE   T.FACULDADE = @CENSO
                                            AND T.ANO = @ANO";
                if (turno.ToUpper() == "INTEGRAL")
                {
                    contextQuery.Command += " AND NOT ( TU.DESCRICAO = @TURNO OR TU.DESCRICAO = 'INTEGRAL' OR TU.DESCRICAO = 'NOITE')";
                }
                                          
                 contextQuery.Command += " ORDER BY TU.DESCRICAO     ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@TURNO", turno);

                return ctx.GetDataTable(contextQuery);
            }
        }
      
    }
}
