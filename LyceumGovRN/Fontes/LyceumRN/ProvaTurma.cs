namespace Techne.Lyceum.RN
{
    using System;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.Library;
    using Techne.Lyceum.CR;
    using Techne.Lyceum.RN.Entidades;

    public class ProvaTurma : RNBase
    {
        public static QueryTable ConsultarDadosGrade(string grade_id)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select gs.GRADE_ID, gs.UNIDADE_RESPONSAVEL, u.NOME_COMP, gs.GRADE,
                               gs.ANO, gs.SEMESTRE, gs.CURSO, c.NOME, gs.TURNO, gs.CURRICULO,
                               gs.SERIE, gs.DT_INICIO, gs.DT_FIM
                        from LY_GRADE_SERIE gs join LY_UNIDADE_ENSINO u
                                                    on gs.UNIDADE_RESPONSAVEL = u.UNIDADE_ENS
                                               join LY_CURSO c
                                                    on gs.CURSO = c.CURSO
                        where GRADE_ID = ? ";

                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection, grade_id);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static string ConsultarGradeID(string turma, string disciplina, string ano, string semestre)
        {
            String sql = @"select grade_id from ly_grade_turma where TURMA = ? and DISCIPLINA = ? and ANO = ? and SEMESTRE = ?";
            return ConsultarCampo(sql, turma, disciplina, ano, semestre);
        }

        public static string ConsultaNomeSerie(string curso, string turno, string curriculo, string serie)
        {
            string sql = @"select DESCRICAO from LY_SERIE WHERE CURSO = ? and TURNO = ? AND CURRICULO = ? AND SERIE = ?";
            return ConsultarCampo(sql, curso, turno, curriculo, serie);
        }

        public static Ly_prova.Row[] ConsultarProvas(String disciplina, String turma, decimal ano, decimal periodo, decimal? subperiodo)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                if (subperiodo.HasValue)
                {
                    Ly_prova dt = CR.Ly_prova.Query(connection,
                        @"DISCIPLINA = ?
                          AND TURMA = ?
                          AND ANO = ?
                          AND SEMESTRE = ?
                          AND SUBPERIODO = ?", disciplina, turma, ano, periodo, subperiodo.Value);
                    return dt.Select("", "ordem");
                }
                else
                {
                    Ly_prova dt = CR.Ly_prova.Query(connection,
                        @"DISCIPLINA = ?
                          AND TURMA = ?
                          AND ANO = ?
                          AND SEMESTRE = ?
                          AND
                          (PROVA = 'TPF_B1' OR PROVA = 'TPF_B2' OR PROVA = 'TPF_B3' OR PROVA = 'TPF_B4' OR PROVA = 'RF'
                          OR PROVA = 'BIM1' OR PROVA = 'BIM2' OR PROVA = 'BIM3' OR PROVA = 'BIM4')", disciplina, turma, ano, periodo);
                    return dt.Select("", "ordem");
                }
            }
            finally
            {
                connection.Close();
            }
        }

        public static Ly_prova.Row ConsultarProva(String prova, String disciplina, String turma, decimal ano, decimal periodo)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                return Ly_prova.Row.Query(connection, disciplina, turma, ano, periodo, prova);
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static LyProva ConsultarProva(string disciplina, string turma, decimal ano, decimal periodo, decimal subperiodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT TOP 1
                            disciplina,
                            turma,
                            ano,
                            semestre,
                            prova,
                            ordem,
                            subperiodo,
                            nome,
                            nota_max
                    FROM    ly_prova
                    WHERE   DISCIPLINA = @DISCIPLINA
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @PERIODO
                            AND SUBPERIODO = @SUBPERIODO");

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);

                var prova = ctx.TryToBindEntity<LyProva>(contextQuery);

                if (string.IsNullOrEmpty(prova.Prova))
                {
                    return null;
                }

                return prova;
            }
        }

        public static LyProva ConsultarProvaNotas(string disciplina, string turma, decimal ano, decimal periodo, decimal subperiodo)
        {
            try
            {
                using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
                {
                    var contextQuery = new ContextQuery(
                        @"select top 1 
                            DISCIPLINA
                            ,TURMA
                            ,ANO
                            ,SEMESTRE
                            ,PROVA 
                            ,ORDEM
                            ,SUBPERIODO
                            ,NOME
                            ,NOTA_MAX 
                        from ly_prova 
                        where DISCIPLINA = @DISCIPLINA 
                        and TURMA = @TURMA 
                        and ANO = @ANO 
                        and SEMESTRE = @PERIODO 
                        and SUBPERIODO = @SUBPERIODO");

                    contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                    contextQuery.Parameters.Add("@TURMA", turma);
                    contextQuery.Parameters.Add("@ANO", ano);
                    contextQuery.Parameters.Add("@PERIODO", periodo);
                    contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);

                    return ctx.TryToBindEntity<LyProva>(contextQuery);
                }
            }
            catch
            {
                return null;
            }
        }

        #region Aba Grid
        public static QueryTable Consultar(string turma, int ano, int semestre, string disciplina)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT turma, ano, semestre, disciplina,
                                      prova,
                                      nome,
                                      nota_max,
                                      ordem as ordem,
                                      subperiodo,
                                      on_line,
                                      formula,
                                    dt_prova,
                                    dt_divulgacao,
                                    data_divulgacao_aol
                               FROM Ly_prova
                                where DISCIPLINA = ?
                                  and TURMA = ?
                                  and ANO = ?
                                  and SEMESTRE = ?
                                  AND (PROVA <> 'TP_B1' and PROVA <> 'TP_B2' and PROVA <> 'TP_B3' and PROVA <> 'TP_B4') 
                                  AND (PROVA <> 'RP_B1' and PROVA <> 'RP_B2' and PROVA <> 'RP_B3' and PROVA <> 'RP_B4') 
                                  AND (PROVA <> 'TPF_B1' and PROVA <> 'TPF_B2' and PROVA <> 'TPF_B3' and PROVA <> 'TPF_B4')
                                  AND (PROVA <> 'CC_B1' and PROVA <> 'CC_B2' and PROVA <> 'CC_B3' and PROVA <> 'CC_B4')
                                  AND (PROVA <> 'RF')
                                order by ORDEM";


                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection, disciplina, turma, ano, semestre);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarInstrumentos(string turma, string disciplina, decimal? subperiodo, decimal ano, decimal periodo)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            if (!string.IsNullOrEmpty(turma) && !string.IsNullOrEmpty(disciplina))
            {
                try
                {
                    String sql;

                    sql = @"DECLARE @sub int
                            SET @sub = ?
                            SELECT 
                                   null AS prova, 
                                   '<nenhum>' AS descricao
                               UNION ALL
                               SELECT DISTINCT 
                                   prova, 
                                   descricao
                               FROM Ly_prova_discip
                               where DISCIPLINA = ?
                               and (SUBPERIODO = @sub or @sub is null)";
                    QueryTable qt_dis = new QueryTable(sql);
                    qt_dis.Query(connection, subperiodo, disciplina);

                    if (qt_dis.Rows.Count > 1)
                        return qt_dis;
                    else
                    {
                        String sql_serie = @"SELECT CURSO, CURRICULO, TURNO, SERIE
                               FROM Ly_turma
                               where DISCIPLINA = ? ANd TURMA = ? and ano = ? and semestre = ?";
                        QueryTable qt_serie = new QueryTable(sql_serie);
                        qt_serie.Query(connection, disciplina, turma, ano, periodo);
                        if (qt_serie.Rows.Count > 0)
                        {
                            string serie = qt_serie.Rows[0]["SERIE"].ToString();
                            string curso = qt_serie.Rows[0]["CURSO"].ToString();
                            string curriculo = qt_serie.Rows[0]["CURRICULO"].ToString();
                            string turno = qt_serie.Rows[0]["TURNO"].ToString();
                            //Columns["SERIE"].ToString();
                            String sql_instr;
                            sql_instr = @" DECLARE @sub int
                                    SET @sub = ?
                                    SELECT 
                                   null AS prova, 
                                   '<nenhum>' AS descricao
                               UNION ALL
                               SELECT DISTINCT 
                                   prova, 
                                   descricao
                               FROM Ly_prova_unidade_curso
                               where SERIE = ?
                               and CURSO = ?
                               and TURNO = ?
                               and CURRICULO = ?
                               and (SUBPERIODO = @sub or @sub is null)";
                            QueryTable qt_instr = new QueryTable(sql_instr);
                            qt_instr.Query(connection, subperiodo, serie, curso, turno, curriculo);
                            return qt_instr;
                        }
                        else
                        {
                            String sql_null = @"SELECT 
                                   null AS prova, 
                                   '<nenhum>' AS descricao";
                            QueryTable qt_null = new QueryTable(sql_null);
                            qt_null.Query(connection);
                            return qt_null;
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                String sql_null = @"SELECT 
                                   null AS prova, 
                                   '<nenhum>' AS descricao";
                QueryTable qt_null = new QueryTable(sql_null);
                qt_null.Query(connection);
                return qt_null;
            }
        }

        public static string ConsultaNomeProva(string disciplina, string turma, int ano, int semestre, string prova, string subperiodo)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            string nome = "nome";
            if (!string.IsNullOrEmpty(turma) && !string.IsNullOrEmpty(disciplina))
            {
                try
                {
                    String sql;

                    sql = @"DECLARE @sub int
                            SET @sub = ?
                               SELECT DISTINCT 
                                   descricao
                               FROM Ly_prova_discip
                               where DISCIPLINA = ?
                               and (SUBPERIODO = @sub or @sub is null)
                              and PROVA = ?";
                    nome = ConsultarCampo(sql, subperiodo, disciplina, prova);
                    if (!string.IsNullOrEmpty(nome))
                        return nome;
                    else
                    {
                        String sql_serie = @"SELECT CURSO, CURRICULO, TURNO, SERIE
                               FROM Ly_turma
                               where DISCIPLINA = ? and TURMA = ? and ano = ? and semestre = ?";
                        QueryTable qt_serie = new QueryTable(sql_serie);
                        qt_serie.Query(connection, disciplina, turma, ano, semestre);
                        if (qt_serie.Rows.Count > 0)
                        {
                            string serie = qt_serie.Rows[0]["SERIE"].ToString();
                            string curso = qt_serie.Rows[0]["CURSO"].ToString();
                            string curriculo = qt_serie.Rows[0]["CURRICULO"].ToString();
                            string turno = qt_serie.Rows[0]["TURNO"].ToString();
                            //Columns["SERIE"].ToString();
                            String sql_instr;
                            sql_instr = @" DECLARE @sub int
                                    SET @sub = ?
                               SELECT DISTINCT 
                                   descricao
                               FROM Ly_prova_unidade_curso
                               where SERIE = ?
                               and CURSO = ?
                               and TURNO = ?
                               and CURRICULO = ?
                               and (SUBPERIODO = @sub or @sub is null)
                               and PROVA = ?";

                            nome = ConsultarCampo(sql_instr, subperiodo, serie, curso, turno, curriculo, prova);
                            if (!string.IsNullOrEmpty(nome))
                                return nome;
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return nome;
        }

        public static Ly_prova.Row VerificarExistencia(Ly_prova.Row row)
        {
            if (String.IsNullOrEmpty(row.Turma)) return null;
            if (!row.Ano.HasValue) return null;
            if (!row.Semestre.HasValue) return null;
            if (String.IsNullOrEmpty(row.Disciplina)) return null;
            if (String.IsNullOrEmpty(row.Prova)) return null;

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                Ly_prova dt = Ly_prova.Query(connection,
                    "turma =? and ano = ? and semestre = ? and disciplina = ? and prova = ?", row.Turma, row.Ano, row.Semestre, row.Disciplina, row.Prova);

                if (dt.Rows.Count > 0)
                    return dt.Rows[0];
                else
                    return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static Ly_prova.Row VerificarOrdem(Ly_prova.Row row)
        {
            if (String.IsNullOrEmpty(row.Turma)) return null;
            if (!row.Ano.HasValue) return null;
            if (!row.Semestre.HasValue) return null;
            if (String.IsNullOrEmpty(row.Disciplina)) return null;
            if (String.IsNullOrEmpty(row.Prova)) return null;
            if (!row.Ordem.HasValue) return null;

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                Ly_prova dt = Ly_prova.Query(connection,
                    "turma =? and ano = ? and semestre = ? and disciplina = ? and ordem = ? ", row.Turma, row.Ano, row.Semestre, row.Disciplina, row.Ordem);

                if (dt.Rows.Count > 0)
                    return dt.Rows[0];
                else
                    return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static decimal ConsultarOrdem(string disciplina, string turma, string ano, string semestre, string subperiodo)
        {
            string sql = @"select MAX(ORDEM) from LY_PROVA where DISCIPLINA = ? AND TURMA = ? AND ANO = ? AND SEMESTRE = ? AND SUBPERIODO = ?
                           AND (PROVA <> 'TP_B1' and PROVA <> 'TP_B2' and PROVA <> 'TP_B3' and PROVA <> 'TP_B4') 
                          AND (PROVA <> 'RP_B1' and PROVA <> 'RP_B2' and PROVA <> 'RP_B3' and PROVA <> 'RP_B4') 
                          AND (PROVA <> 'TPF_B1' and PROVA <> 'TPF_B2' and PROVA <> 'TPF_B3' and PROVA <> 'TPF_B4')
                          AND (PROVA <> 'CC_B1' and PROVA <> 'CC_B2' and PROVA <> 'CC_B3' and PROVA <> 'CC_B4')
                          AND (PROVA <> 'RF')";

            decimal ordem = ExecutarFuncao(sql, disciplina, turma, ano, semestre, subperiodo);

            return ordem + 1;
        }

        public static QueryTable CarregarOrdem(string disciplina, string turma, string ano, string semestre, string subperiodo, string prova)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select ordem
                                from LY_PROVA
                                where DISCIPLINA = ?
                                  and TURMA = ?
                                  and ANO = ?
                                  and SEMESTRE = ?
                                  and SUBPERIODO = ?
                                  and PROVA = ?";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, disciplina, turma, Convert.ToDecimal(ano), Convert.ToDecimal(semestre),
                    Convert.ToDecimal(subperiodo), prova);

                return qt;
            }
            finally
            {
                connection.Close();
            }

        }

        public static RetValue Inserir(Ly_prova.Row row)
        {
            //Verifica se já existe prova cadastrada com a mesma chave para a disciplina
            Ly_prova.Row existeProvaDisciplina = VerificarExistencia(row);
            if (existeProvaDisciplina != null)
                return new RetValue(false, "", new ErrorList(String.Format("Instrumento '{0}' da disciplina '{1}' já cadastrado para a turma '{2}'.",
                    row.Prova, row.Disciplina, row.Turma)));

            //Verifica se já existe alguma prova com a mesma ordem para a disciplina
            Ly_prova.Row existeDisciplinaOrdem = VerificarOrdem(row);
            if (existeDisciplinaOrdem != null && existeDisciplinaOrdem.Prova != row.Prova)
                return new RetValue(false, "", new ErrorList(String.Format("Instrumento de ordem '{0}' já cadastrado para a disciplina '{1}' da turma '{2}'.",
                    row.Ordem, row.Disciplina, row.Turma)));

            //DEFINIÇÃO (26/04/2010): A fórmula dos Instrumentos de Avaliação é fixa e, portanto, não deve ser mais validada.
            //Validação da fórmula
            //if (!string.IsNullOrEmpty(row.Formula))
            //{
            //    RetValue validacaoFormula = Formula.ValidaFormulaTurma(row.Formula, row.Disciplina, row.Turma, Convert.ToInt32(row.Ano), Convert.ToInt32(row.Semestre), row.Prova, row.Ordem);
            //    if (validacaoFormula != null && !validacaoFormula.Ok)
            //        return validacaoFormula;
            //}

            //Inserção
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_prova.Row.Insert(connection, row.Disciplina, row.Turma, row.Ano, row.Semestre, row.Prova,
                    row.Nome, row.Ordem, row.On_line, row.Pode_alterar_formula, row.E_recuperacao,
                    row.E_prova_base_rec, "e_oficial, formula, subperiodo, data_divulgacao_aol, dt_divulgacao, dt_prova, nota_max", row.E_oficial, row.Formula,
                    row.Subperiodo, row.Data_divulgacao_aol, row.Dt_divulgacao, row.Dt_prova, row.Nota_max);
                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue Remover(String turma, int ano, int semestre, String disciplina, String prova)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_prova.Row.Delete(connection, disciplina, turma, ano, semestre, prova);

                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());
            }
            finally
            {
                connection.Close();
            }
        }

        public static RetValue Atualizar(Ly_prova.Row row)//(String pk_disciplina, String pk_turma, int pk_ano, int pk_semestre, String pk_prova, Ly_prova.Row newValues)
        {
            //Verifica se já existe alguma prova com a mesma ordem para a disciplina
            Ly_prova.Row existeDisciplinaOrdem = VerificarOrdem(row);
            if (existeDisciplinaOrdem != null && existeDisciplinaOrdem.Prova != row.Prova)
                return new RetValue(false, "", new ErrorList(String.Format("Instrumento de ordem '{0}' já cadastrado para a disciplina '{1}' da turma '{2}'.",
                    row.Ordem, row.Disciplina, row.Turma)));

            //DEFINIÇÃO (26/04/2010): A fórmula dos Instrumentos de Avaliação é fixa e, portanto, não deve ser mais validada.
            //Validação da fórmula
            //if (!string.IsNullOrEmpty(row.Formula))
            //{
            //    RetValue validacaoFormula = Formula.ValidaFormulaTurma(row.Formula, row.Disciplina, row.Turma, Convert.ToInt32(row.Ano), Convert.ToInt32(row.Semestre), row.Prova, row.Ordem);
            //    if (validacaoFormula != null && !validacaoFormula.Ok)
            //        return validacaoFormula;
            //}

            //Atualização do registro
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                Ly_prova.Row.Update(connection, row.Disciplina, row.Turma, row.Ano, row.Semestre, row.Prova,
                    "nome, ordem, on_line, formula, subperiodo, data_divulgacao_aol, dt_divulgacao, dt_prova, nota_max",
                    row.Nome, row.Ordem, row.On_line, row.Formula, row.Subperiodo, row.Data_divulgacao_aol, row.Dt_divulgacao, row.Dt_prova, row.Nota_max); //conferir colunas

                RetValue ret = VerificarErro(connection.GetErrors());
                if (ret != null && !ret.Ok)
                    connection.Rollback();
                return ret;
            }
            catch
            {
                connection.Rollback();
                return VerificarErro(connection.GetErrors());

            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarSubPeriodosLetivos()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT DISTINCT 
                                   subperiodo, 
                                   descricao
                               FROM ly_subperiodo_letivo";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection);

                return qt;
            }
            finally
            {
                connection.Close();
            }

        }

        public static int ContaProvas(string turma, string disciplina, int? ano, int? semestre, decimal? subperiodo)
        {
            string sql = @"select COUNT(1) from LY_PROVA WITH(NOLOCK) where TURMA = ? 
                          AND DISCIPLINA = ? AND ANO = ? AND SEMESTRE = ? AND SUBPERIODO = ?
                          AND (PROVA <> 'TP_B1' and PROVA <> 'TP_B2' and PROVA <> 'TP_B3' and PROVA <> 'TP_B4') 
                          AND (PROVA <> 'RP_B1' and PROVA <> 'RP_B2' and PROVA <> 'RP_B3' and PROVA <> 'RP_B4') 
                          AND (PROVA <> 'TPF_B1' and PROVA <> 'TPF_B2' and PROVA <> 'TPF_B3' and PROVA <> 'TPF_B4')
                          AND (PROVA <> 'CC_B1' and PROVA <> 'CC_B2' and PROVA <> 'CC_B3' and PROVA <> 'CC_B4')
                          AND (PROVA <> 'RF')";
            int num = ExecutarFuncao(sql, turma, disciplina, ano, semestre, subperiodo);
            return num;
        }

        public static int ContaProvasDependencia(string turma, string disciplina, int? ano, int? semestre)
        {
            string sql = @"select COUNT(1) from LY_PROVA where TURMA = ? 
                          AND DISCIPLINA = ? AND ANO = ? AND SEMESTRE = ?";
            int num = ExecutarFuncao(sql, turma, disciplina, ano, semestre);
            return num;
        }

        public static decimal SomaNotaMax(string turma, string disciplina, int? ano, int? semestre, decimal? subperiodo)
        {
            string sql = @"select SUM(convert(numeric(4,1), ISNULL(REPLACE(NOTA_MAX, ',','.'), 0))) from LY_PROVA WITH(NOLOCK)
                          where TURMA = ? AND DISCIPLINA = ? AND ANO = ? AND SEMESTRE = ? AND SUBPERIODO = ?
                          AND (PROVA <> 'TP_B1' and PROVA <> 'TP_B2' and PROVA <> 'TP_B3' and PROVA <> 'TP_B4') 
                          AND (PROVA <> 'RP_B1' and PROVA <> 'RP_B2' and PROVA <> 'RP_B3' and PROVA <> 'RP_B4') 
                          AND (PROVA <> 'TPF_B1' and PROVA <> 'TPF_B2' and PROVA <> 'TPF_B3' and PROVA <> 'TPF_B4')
                          AND (PROVA <> 'CC_B1' and PROVA <> 'CC_B2' and PROVA <> 'CC_B3' and PROVA <> 'CC_B4')
                          AND (PROVA <> 'RF')";
            decimal num = ExecutarFuncaoDec(sql, turma, disciplina, ano, semestre, subperiodo);
            return num;
        }

        public static bool GeraMedias(string turma, string disciplina, int ano, int semestre, decimal? subperiodo, string prova)
        {
            //GERA TOTAL DE PONTOS
            bool tp = GeraTotaldePontos(turma, disciplina, ano, semestre, subperiodo, prova);
            bool rp = GeraRecuperacaoParalela(turma, disciplina, ano, semestre, subperiodo);
            bool tpf = GeraTotaldePontosFinal(turma, disciplina, ano, semestre, subperiodo);
            bool existe_rf = ConsultaResultadoFinal(turma, disciplina, ano, semestre);
            bool rf = false;
            if (!existe_rf)
            {
                rf = GeraResultadoFinal(turma, disciplina, ano, semestre);
                if (tp && rp && tpf && rf)
                    return true;
                else
                    return false;
            }
            else
            {
                if (tp && rp && tpf)
                    return true;
                else
                    return false;
            }
        }

        private static bool GeraResultadoFinal(string turma, string disciplina, int? ano, int? semestre)
        {
            //GERA TOTAL DE PONTOS FINAL
            //Valores default
            String e_oficial = "S";
            String pode_alterar_formula = "S";
            String e_recuperacao = "N";
            String e_prova_base_rec = "N";

            String prova = "RF";
            String nome = "Resultado Final";
            String nota_max = "40";
            decimal? ordem = 40;
            String on_line = "S";
            string provas_finais = "";

            QueryTable qt = ConsultarProvasFinais(turma, disciplina, ano, semestre);
            for (int i = 0; i < qt.Rows.Count; i++)
            {
                provas_finais = qt.Rows[i]["prova"].ToString() + " +" + provas_finais;
            }

            if (qt.Rows.Count != 0)
                provas_finais = provas_finais.Remove(provas_finais.Length - 2);

            String formula = provas_finais;


            Ly_prova.Row row = new Ly_prova().NewRow();
            row.Turma = turma;
            row.Ano = ano;
            row.Semestre = semestre;
            row.Disciplina = disciplina;
            row.Prova = prova;
            row.Nome = nome;
            row.Nota_max = nota_max;
            row.Ordem = ordem;
            row.On_line = on_line;
            row.Pode_alterar_formula = pode_alterar_formula;
            row.E_recuperacao = e_recuperacao;
            row.E_prova_base_rec = e_prova_base_rec;
            row.E_oficial = e_oficial;
            row.Formula = formula;
            row.Subperiodo = null;

            RetValue ret = RN.ProvaTurma.Inserir(row);
            if (ret != null && !ret.Ok)
            {
                if (ret.Errors.Count > 0)
                    return false;
            }
            return true;
        }

        private static QueryTable ConsultarProvasFinais(string turma, string disciplina, int? ano, int? semestre)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"select PROVA from LY_PROVA where DISCIPLINA = ? AND TURMA = ?
                                AND ANO = ? AND SEMESTRE = ?
                                AND (PROVA = 'TPF_B1' OR PROVA = 'TPF_B2' OR PROVA = 'TPF_B3' OR PROVA = 'TPF_B4') ";


                QueryTable qt;
                qt = new QueryTable(sql);
                qt.Query(connection, disciplina, turma, ano, semestre);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        private static bool GeraRecuperacaoParalela(string turma, string disciplina, int? ano, int? semestre, decimal? subperiodo)
        {
            //GERA RECUPERAÇÃO PARALELA
            //Valores default
            String e_oficial = "S";
            String pode_alterar_formula = "S";
            String e_recuperacao = "N";
            String e_prova_base_rec = "N";

            String prova = "RP_B" + Convert.ToString(subperiodo);
            String nome = "Recuperação Paralela";
            String nota_max = "10";
            decimal? ordem = 8 + ((subperiodo - 1) * 10);
            String on_line = "S";
            String formula = "";

            Ly_prova.Row row = new Ly_prova().NewRow();
            row.Turma = turma;
            row.Ano = ano;
            row.Semestre = semestre;
            row.Disciplina = disciplina;
            row.Prova = prova;
            row.Nome = nome;
            row.Nota_max = nota_max;
            row.Ordem = ordem;
            row.On_line = on_line;
            row.Pode_alterar_formula = pode_alterar_formula;
            row.E_recuperacao = e_recuperacao;
            row.E_prova_base_rec = e_prova_base_rec;
            row.E_oficial = e_oficial;
            row.Formula = formula;
            row.Subperiodo = subperiodo;

            RetValue ret = RN.ProvaTurma.Inserir(row);
            if (ret != null && !ret.Ok)
            {
                if (ret.Errors.Count > 0)
                    return false;
            }
            return true;
        }

        private static bool GeraTotaldePontosFinal(string turma, string disciplina, int? ano, int? semestre, decimal? subperiodo)
        {
            //GERA TOTAL DE PONTOS FINAL
            //Valores default
            String e_oficial = "S";
            String pode_alterar_formula = "S";
            String e_recuperacao = "N";
            String e_prova_base_rec = "N";

            String prova = "TPF_B" + Convert.ToString(subperiodo);
            String nome = "Total de Pontos Final";
            String nota_max = "10";
            decimal? ordem = 9 + ((subperiodo - 1) * 10);
            String on_line = "S";
            String formula = "MAX(TP_B" + Convert.ToString(subperiodo) + " , RP_B" + Convert.ToString(subperiodo) + ")";

            Ly_prova.Row row = new Ly_prova().NewRow();
            row.Turma = turma;
            row.Ano = ano;
            row.Semestre = semestre;
            row.Disciplina = disciplina;
            row.Prova = prova;
            row.Nome = nome;
            row.Nota_max = nota_max;
            row.Ordem = ordem;
            row.On_line = on_line;
            row.Pode_alterar_formula = pode_alterar_formula;
            row.E_recuperacao = e_recuperacao;
            row.E_prova_base_rec = e_prova_base_rec;
            row.E_oficial = e_oficial;
            row.Formula = formula;
            row.Subperiodo = subperiodo;

            RetValue ret = RN.ProvaTurma.Inserir(row);
            if (ret != null && !ret.Ok)
            {
                if (ret.Errors.Count > 0)
                    return false;
            }
            return true;
        }

        public static bool GeraTotaldePontos(string turma, string disciplina, int? ano, int? semestre, decimal? subperiodo, string prova_formula)
        {
            //GERA TOTAL DE PONTOS
            //Valores default
            String e_oficial = "S";
            String pode_alterar_formula = "S";
            String e_recuperacao = "N";
            String e_prova_base_rec = "N";

            String prova = "TP_B" + Convert.ToString(subperiodo);
            String nome = "Total de Pontos";
            String nota_max = "10";
            decimal? ordem = 7 + ((subperiodo - 1) * 10);
            String on_line = "S";
            String formula = prova_formula;

            Ly_prova.Row row = new Ly_prova().NewRow();
            row.Turma = turma;
            row.Ano = ano;
            row.Semestre = semestre;
            row.Disciplina = disciplina;
            row.Prova = prova;
            row.Nome = nome;
            row.Nota_max = nota_max;
            row.Ordem = ordem;
            row.On_line = on_line;
            row.Pode_alterar_formula = pode_alterar_formula;
            row.E_recuperacao = e_recuperacao;
            row.E_prova_base_rec = e_prova_base_rec;
            row.E_oficial = e_oficial;
            row.Formula = formula;
            row.Subperiodo = subperiodo;

            RetValue ret = RN.ProvaTurma.Inserir(row);
            if (ret != null && !ret.Ok)
            {
                if (ret.Errors.Count > 0)
                    return false;
            }
            return true;
        }

        public static bool ConsultaMediaTP(string turma, string disciplina, int ano, int semestre, decimal? subperiodo)
        {
            string sql_tp = @"select 1 from LY_PROVA
                              where TURMA = ? AND DISCIPLINA = ? AND 
                              ANO = ? AND SEMESTRE = ? AND SUBPERIODO = ? AND PROVA = ?";
            int tp = ExecutarFuncao(sql_tp, turma, disciplina, ano, semestre, subperiodo, "TP_B" + Convert.ToString(subperiodo));


            if (tp != 0)
                return true;
            else
                return false;
        }

        public static bool ConsultaMediaRP(string turma, string disciplina, int ano, int semestre, decimal? subperiodo)
        {

            string sql_rp = @"select 1 from LY_PROVA
                              where TURMA = ? AND DISCIPLINA = ? AND 
                              ANO = ? AND SEMESTRE = ? AND SUBPERIODO = ? AND PROVA = ?";
            int rp = ExecutarFuncao(sql_rp, turma, disciplina, ano, semestre, subperiodo, "RP_B" + Convert.ToString(subperiodo));

            if (rp != 0)
                return true;
            else
                return false;
        }

        public static bool ConsultaMediaTPF(string turma, string disciplina, int ano, int semestre, decimal? subperiodo)
        {
            string sql_tpf = @"select 1 from LY_PROVA
                              where TURMA = ? AND DISCIPLINA = ? AND 
                              ANO = ? AND SEMESTRE = ? AND SUBPERIODO = ? AND PROVA = ?";
            int tpf = ExecutarFuncao(sql_tpf, turma, disciplina, ano, semestre, subperiodo, "TPF_B" + Convert.ToString(subperiodo));

            if (tpf != 0)
                return true;
            else
                return false;
        }

        public static bool ConsultaMediaCC(string turma, string disciplina, int ano, int semestre, decimal? subperiodo)
        {
            string sql_cc = @"select 1 from LY_PROVA
                              where TURMA = ? AND DISCIPLINA = ? AND 
                              ANO = ? AND SEMESTRE = ? AND SUBPERIODO = ? AND PROVA = ?";
            int cc = ExecutarFuncao(sql_cc, turma, disciplina, ano, semestre, subperiodo, "CC_B" + Convert.ToString(subperiodo));

            if (cc != 0)
                return true;
            else
                return false;
        }

        public static bool ConsultaResultadoFinal(string turma, string disciplina, int ano, int semestre)
        {
            string sql_cc = @"select 1 from LY_PROVA
                              where TURMA = ? AND DISCIPLINA = ? AND 
                              ANO = ? AND SEMESTRE = ? AND PROVA = ?";
            int cc = ExecutarFuncao(sql_cc, turma, disciplina, ano, semestre, "RF");

            if (cc != 0)
                return true;
            else
                return false;
        }

        public static bool AtualizaMedias(string turma, string disciplina, int ano, int semestre, decimal? subperiodo, string nova_prova, bool edicao)
        {
            bool tp = RN.ProvaTurma.ConsultaMediaTP(turma, disciplina, ano, semestre, subperiodo);
            bool rp = RN.ProvaTurma.ConsultaMediaRP(turma, disciplina, ano, semestre, subperiodo);
            bool tpf = RN.ProvaTurma.ConsultaMediaTPF(turma, disciplina, ano, semestre, subperiodo);
            bool rf = RN.ProvaTurma.ConsultaResultadoFinal(turma, disciplina, ano, semestre);
            //bool cc = RN.ProvaTurma.ConsultaMediaCC(turma, disciplina, ano, semestre, subperiodo);
            bool exitem_medias = tp && rp && tpf && rf;
            if (exitem_medias) //TESTAR
            {
                #region Atualiza TP_B
                //consulta formula de TP para atualizar
                string sql = @"select FORMULA from LY_PROVA
                              where TURMA = ? AND DISCIPLINA = ? AND 
                              ANO = ? AND SEMESTRE = ? AND SUBPERIODO = ? AND PROVA = ?";
                string velha_formula = ConsultarCampo(sql, turma, disciplina, ano, semestre, subperiodo, "TP_B" + Convert.ToString(subperiodo));
                string nova_formula;
                if (edicao)
                    nova_formula = velha_formula + " + " + nova_prova;
                else
                {
                    String formulaAux = velha_formula;

                    formulaAux = " " + formulaAux + " ";
                    formulaAux = formulaAux.Replace("+ " + nova_prova + " +", "");
                    formulaAux = formulaAux.Replace(" " + nova_prova + " +", "");
                    formulaAux = formulaAux.Replace("+ " + nova_prova + " ", "");
                    formulaAux = formulaAux.Replace(" " + nova_prova + " ", "");
                    formulaAux = formulaAux.Trim();

                    nova_formula = formulaAux;
                }

                //Valores default
                String e_oficial = "S";
                String pode_alterar_formula = "S";
                String e_recuperacao = "N";
                String e_prova_base_rec = "N";

                String prova = "TP_B" + Convert.ToString(subperiodo);
                String nome = "Total de Pontos";
                String nota_max = "10";
                decimal? ordem = 7 + ((subperiodo - 1) * 10);
                String on_line = "S";
                String formula = nova_formula;

                Ly_prova.Row row = new Ly_prova().NewRow();
                row.Turma = turma;
                row.Ano = ano;
                row.Semestre = semestre;
                row.Disciplina = disciplina;
                row.Prova = prova;
                row.Nome = nome;
                row.Nota_max = nota_max;
                row.Ordem = ordem;
                row.On_line = on_line;
                row.Pode_alterar_formula = pode_alterar_formula;
                row.E_recuperacao = e_recuperacao;
                row.E_prova_base_rec = e_prova_base_rec;
                row.E_oficial = e_oficial;
                row.Formula = formula;
                row.Subperiodo = subperiodo;

                RetValue retorno = RN.ProvaTurma.Atualizar(row);
                if (retorno != null)
                {
                    if (!retorno.Ok)
                    {
                        if (retorno.Errors.Count > 0)
                            throw new ApplicationException(retorno.Errors.ToString());
                        //O QUE FAZER COM A PROVA QUE JÁ FOI INSERIDA??- É deletada, está na tela
                    }
                }
                #endregion

                #region Atualiza RF

                bool atualiza_rf = AtualizaRF(turma, disciplina, ano, semestre);
                if (!atualiza_rf)
                    return false;

                #endregion
            }
            else // se alguma das médias não existir, descobrir qual não existe e gerar 
            {
                if (!tp) // se não existir TP precisa pesquisar os instrumentos existentes e montar a fórmula para gerar tp
                {
                    Ly_prova.Row[] provas = ConsultarProvas(disciplina, turma, ano, semestre, subperiodo);
                    int num_provas = provas.Length;
                    string formula = "";
                    for (int i = 0; i < num_provas; i++)
                    {
                        if ((provas[i]["prova"].ToString() != "RP_B" + Convert.ToString(subperiodo)) && (provas[i]["prova"].ToString() != "TP_B" + Convert.ToString(subperiodo)) && (provas[i]["prova"].ToString() != "TPF_B" + Convert.ToString(subperiodo)) && (provas[i]["prova"].ToString() != "RF"))
                        {
                            formula = provas[i]["prova"].ToString() + " + " + formula;
                        }
                    }
                    formula = formula.Remove(formula.Length - 3);
                    bool gera_tp = GeraTotaldePontos(turma, disciplina, ano, semestre, subperiodo, formula);
                    //O QUE DEVE SER FEITO SE GERA_TP = FALSE?
                    if (!gera_tp)
                        return false;
                }
                if (!rp)
                {
                    bool gera_rp = GeraRecuperacaoParalela(turma, disciplina, ano, semestre, subperiodo);
                    //O QUE DEVE SER FEITO SE GERA_RP = FALSE?
                    if (!gera_rp)
                        return false;
                }
                if (!tpf)
                {
                    bool gera_tpf = GeraTotaldePontosFinal(turma, disciplina, ano, semestre, subperiodo);
                    //O QUE DEVE SER FEITO SE GERA_TPF = FALSE?
                    if (!gera_tpf)
                        return false;
                }
                //if (!cc)
                //{
                //    bool gera_cc = GeraConselhoDeClasse(turma, disciplina, ano, semestre, subperiodo);
                //    //O QUE DEVE SER FEITO SE GERA_CC = FALSE?
                //    if (!gera_cc)
                //        return false;
                //}

                #region Atualiza RF

                bool atualiza_rf = AtualizaRF(turma, disciplina, ano, semestre);
                if (!atualiza_rf)
                    return false;

                #endregion

            }
            return true;
        }

        public static bool AtualizaRF(string turma, string disciplina, int ano, int semestre)
        {
            //GERA TOTAL DE PONTOS FINAL
            //Valores default
            String e_oficial = "S";
            String pode_alterar_formula = "S";
            String e_recuperacao = "N";
            String e_prova_base_rec = "N";

            String prova = "RF";
            String nome = "Resultado Final";
            String nota_max = "40";
            decimal? ordem = 40;
            String on_line = "S";
            string provas_finais = "";

            QueryTable qt = ConsultarProvasFinais(turma, disciplina, ano, semestre);
            //Se não existirem totais finais, deleta o resultado final
            if (qt.Rows.Count < 1)
            {
                RetValue ret = RN.ProvaTurma.Remover(turma, ano, semestre, disciplina, "RF");
                if (ret != null && !ret.Ok)
                {
                    if (ret.Errors.Count > 0)
                        return false;
                }
            }
            //Se existirem totais finais atualiza a fórmula
            else
            {
                for (int i = 0; i < qt.Rows.Count; i++)
                {
                    provas_finais = qt.Rows[i]["prova"].ToString() + " +" + provas_finais;
                }

                if (qt.Rows.Count != 0)
                    provas_finais = provas_finais.Remove(provas_finais.Length - 2);

                String formula = provas_finais;

                Ly_prova.Row row = new Ly_prova().NewRow();
                row.Turma = turma;
                row.Ano = ano;
                row.Semestre = semestre;
                row.Disciplina = disciplina;
                row.Prova = prova;
                row.Nome = nome;
                row.Nota_max = nota_max;
                row.Ordem = ordem;
                row.On_line = on_line;
                row.Pode_alterar_formula = pode_alterar_formula;
                row.E_recuperacao = e_recuperacao;
                row.E_prova_base_rec = e_prova_base_rec;
                row.E_oficial = e_oficial;
                row.Formula = formula;
                row.Subperiodo = null;

                RetValue ret = RN.ProvaTurma.Atualizar(row);
                if (ret != null && !ret.Ok)
                {
                    if (ret.Errors.Count > 0)
                        return false;
                }
            }
            return true;
        }

        public static bool GeraTotaldePontosDependencia(string turma, string disciplina, int? ano, int? semestre)
        {
            for (int i = 1; i < 5; i++)
            {
                //GERA TOTAL DE PONTOS FINAL
                //Valores default
                String e_oficial = "S";
                String pode_alterar_formula = "S";
                String e_recuperacao = "N";
                String e_prova_base_rec = "N";

                String prova = "TPF_B" + Convert.ToString(i);
                String nome = "Total de Pontos Final";
                String nota_max = "10";
                decimal? ordem = (decimal?)i;
                String on_line = "S";
                //String formula = "MAX(TP_B" + Convert.ToString(i) + " , RP_B" + Convert.ToString(i) + ")";

                Ly_prova.Row row = new Ly_prova().NewRow();
                row.Turma = turma;
                row.Ano = ano;
                row.Semestre = semestre;
                row.Disciplina = disciplina;
                row.Prova = prova;
                row.Nome = nome;
                row.Nota_max = nota_max;
                row.Ordem = ordem;
                row.On_line = on_line;
                row.Pode_alterar_formula = pode_alterar_formula;
                row.E_recuperacao = e_recuperacao;
                row.E_prova_base_rec = e_prova_base_rec;
                row.E_oficial = e_oficial;
                //row.Formula = formula;
                row.Subperiodo = i;

                RetValue ret = RN.ProvaTurma.Inserir(row);
                if (ret != null && !ret.Ok)
                {
                    if (ret.Errors.Count > 0)
                        return false;
                }
            }

            return true;

        }


        #region Métodos vazios para utilização pelo ObjectDataSource
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void InsertMethodODS(object prova, object nota_max, object subperiodo, object dt_prova, object data_divulgacao_aol)
        {
            return;
        }
        // subperiodo, prova, nome, nota_max, ordem, on_line, dt_prova, dt_divulgacao, data_divulgacao_aol, formula
        public static void InsertMethodODS(object subperiodo, object ordem, object prova, object nome, object nota_max, object on_line, object dt_prova, object dt_divulgacao, object data_divulgacao_aol)
        {
            return;
        }
        public static void InsertMethodODS(object subperiodo, object prova, object nome, object nota_max, object ordem, object on_line, object dt_prova, object dt_divulgacao, object data_divulgacao_aol, object formula)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void DeleteMethodODS(object disciplina, object turma, object ano, object semestre, object prova)
        {
            return;
        }
        [Obsolete("Não utilizar: método vazio para utilização pelo ObjectDataSource")]
        public static void UpdateMethodODS(object prova, object nota_max, object subperiodo, object dt_prova, object data_divulgacao_aol, object disciplina, object turma, object ano, object semestre)
        {
            return;
        }
        public static void UpdateMethodODS(object subperiodo, object prova, object nome, object nota_max, object ordem, object on_line, object dt_prova, object dt_divulgacao, object data_divulgacao_aol, object formula, object disciplina, object turma, object ano, object semestre)
        {
            return;
        }
        #endregion
        #endregion

        public static RetValue AlterarTurma(Ly_turma dtTurma)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            try
            {
                connection.Open(true);

                if (dtTurma != null)
                {
                    if (dtTurma.Rows != null)
                    {
                        ColunasTable colunas = MontarParametros(dtTurma.Columns, dtTurma.Rows[0]);

                        //Verifica se é não um turma exclusiva de eletivas
                        if (dtTurma.Rows[0].Eletiva != "S")
                        {
                            dtTurma.Rows[0].TurmaReferencia = null; 

                            //Verifica se a disciplina é eletiva
                            if (RN.Disciplina.EhDisciplinaEletiva(connection, dtTurma.Rows[0].Disciplina))
                            {
                                dtTurma.Rows[0].Eletiva = "S"; //Marca a disciplina                                        
                            }
                            else
                            {
                                dtTurma.Rows[0].Eletiva = "N";
                            }
                        }

                        Ly_turma.Row.Update(connection, dtTurma.Rows[0].Disciplina, dtTurma.Rows[0].Turma, dtTurma.Rows[0].Ano, dtTurma.Rows[0].Semestre, colunas.Colunas, colunas.ValorColuna);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }
                        return new RetValue(true, "Registro alterado com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static QueryTable ExisteNotaProva(String disciplina, String turma, decimal ano, decimal semestre,
            String prova, decimal? subperiodo)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            QueryTable qt = null;

            try
            {
                connection.Open(true);

                qt = new QueryTable(@"  
                    SELECT DISTINCT *
                    FROM LY_NOTA n
                    INNER JOIN LY_PROVA pv ON n.DISCIPLINA = pv.DISCIPLINA
	                    AND n.TURMA = pv.TURMA
	                    AND n.ANO = pv.ANO
	                    AND n.SEMESTRE = pv.SEMESTRE
	                    AND n.PROVA = pv.PROVA
                    WHERE n.DISCIPLINA = ?
                        AND n.TURMA = ?
                        AND n.ANO = ?
                        AND n.SEMESTRE = ?
                        AND n.PROVA = ?
                        AND pv.SUBPERIODO = ? ");
                qt.Query(connection, disciplina, turma, ano, semestre, prova, subperiodo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Open();
            }
            return qt;
        }

        public static QueryTable ExisteProva(String disciplina, String turma, decimal ano, decimal semestre, String prova)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            QueryTable qt = null;

            try
            {
                connection.Open(true);

                qt = new QueryTable(@"  
                    SELECT DISTINCT *
                    FROM LY_PROVA pv 
                    WHERE pv.DISCIPLINA = ?
                        AND pv.TURMA = ?
                        AND pv.ANO = ?
                        AND pv.SEMESTRE = ?
                        AND pv.PROVA = ? ");
                qt.Query(connection, disciplina, turma, ano, semestre, prova);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Open();
            }
            return qt;
        }

        /// <summary>
        /// Obtém as provas cadastradas para determinada turma.
        /// </summary>    
        /// <param name="turma">Código da turma.</param>
        /// <param name="ano">Ano da turma.</param>
        /// <param name="semestre">Semestre da turma.</param>
        /// <returns>QueryTable contendo prova, subperiodo de ly_prova.</returns>
        public static QueryTable ConsultarProvas(string turma, string ano, string semestre)
        {
            return Consultar(
                @"  SELECT	disciplina, prova, subperiodo
                    FROM	ly_prova (NOLOCK)
                    WHERE	turma = ? AND ano = ? AND semestre = ?",
                turma, ano, semestre);
        }

        /// <summary>
        /// Consulta as provas de determinada turma, retornando um InfoProva[].
        /// </summary>
        /// <param name="turma">Turma.</param>
        /// <param name="ano">Ano.</param>
        /// <param name="semestre">Semestre.</param>
        /// <returns>InfoProva[]</returns>
        public static InfoProva[] ConsultarProvasHistorico(string turma, decimal? ano, decimal? semestre)
        {
            QueryTable qt = Consultar(
                @"SELECT disciplina, prova,nome, CONVERT(decimal, nota_max) nota_max, subperiodo FROM 
                ly_prova WHERE turma = ? AND ano = ? AND semestre = ? ORDER BY disciplina, subperiodo",
                turma, ano, semestre);

            if (qt != null && qt.Rows.Count > 0)
            {
                return qt.Rows.Cast<SimpleRow>()
                    .Select(p => new InfoProva
                    {
                        Disciplina = Convert.ToString(p["disciplina"]),
                        Prova = Convert.ToString(p["prova"]),
                        Subperiodo = Convert.ToDecimal(p["subperiodo"]),
                        NotaMax = Util.Utils.ToNullableDecimal(p["nota_max"]),
                        Titulo = Convert.ToString(p["nome"])
                    }).ToArray();
            }
            else
                return new InfoProva[] { };
        }

        /// <summary>
        /// Consulta as provas de determinada turma, retornando um InfoProva[].
        /// </summary>
        /// <param name="turma">Turma.</param>
        /// <param name="ano">Ano.</param>
        /// <param name="semestre">Semestre.</param>
        /// <param name="disciplina">Disciplina.</param>
        /// <returns>InfoProva[]</returns>
        public static InfoProva[] ConsultarProvasHistorico(string turma, decimal? ano, decimal? semestre, string disciplina)
        {
            QueryTable qt = Consultar(
                @"SELECT disciplina, nome,prova, CONVERT(decimal, nota_max) nota_max, subperiodo FROM 
                ly_prova WHERE turma = ? AND ano = ? AND semestre = ? AND disciplina = ? ORDER BY disciplina, subperiodo",
                turma, ano, semestre, disciplina);

            if (qt != null && qt.Rows.Count > 0)
            {
                return qt.Rows.Cast<SimpleRow>()
                    .Select(p => new InfoProva
                    {
                        Disciplina = Convert.ToString(p["disciplina"]),
                        Prova = Convert.ToString(p["prova"]),
                        Subperiodo = Convert.ToDecimal(p["subperiodo"]),
                        NotaMax = Util.Utils.ToNullableDecimal(p["nota_max"]),
                        Titulo = Convert.ToString(p["nome"])
                    }).ToArray();
            }
            else
                return new InfoProva[] { };
        }

        [Serializable]
        public class InfoProva
        {
            public string Prova { get; set; }
            public string Disciplina { get; set; }
            public string Titulo { get; set; }
            public decimal? Subperiodo { get; set; }
            public decimal? NotaMax { get; set; }
        }

        public LyProva ConsultaProvaPor(string disciplina, string turma, decimal ano, decimal periodo, decimal subperiodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            try
            {
                
                ContextQuery contextQuery = new ContextQuery(
                    @"SELECT TOP 1
                            disciplina,
                            turma,
                            ano,
                            semestre,
                            prova,
                            ordem,
                            subperiodo,
                            nome,
                            nota_max
                    FROM    ly_prova
                    WHERE   DISCIPLINA = @DISCIPLINA
                            AND TURMA = @TURMA
                            AND ANO = @ANO
                            AND SEMESTRE = @PERIODO
                            AND SUBPERIODO = @SUBPERIODO");

                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
                contextQuery.Parameters.Add("@TURMA", turma);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@SUBPERIODO", subperiodo);

                var prova = ctx.TryToBindEntity<LyProva>(contextQuery);

                if (string.IsNullOrEmpty(prova.Prova))
                {
                    return null;
                }

                return prova;
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

