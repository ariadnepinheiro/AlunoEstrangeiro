using System;
using System.Data;
using System.Globalization;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using System.Text;
using System.Collections.Generic;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;
using System.Linq;

namespace Techne.Lyceum.RN
{
    public class Curriculo : RNBase
    {
        public bool PossuiCursoPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM LY_CURRICULO
                                WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static QueryTable Consultar(string turno, string curso)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT CURRICULO FROM LY_CURRICULO WHERE TURNO = ? AND CURSO = ? ORDER BY CURRICULO";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, turno, curso);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable Consultar(DbObject pcurso, string pturno)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT CURRICULO FROM LY_CURRICULO WHERE TURNO = ? AND CURSO = ? ORDER BY CURRICULO";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, pturno, pcurso.ToString());
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarHO(DbObject pcurso, string pturno)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT CURRICULO FROM LY_CURRICULO WHERE TURNO = ? AND CURSO = ? AND ANO_INI > 2009 ORDER BY CURRICULO";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, pturno, pcurso.ToString());
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        //consultar dados da matriz curricular
        public static Ly_curriculo.Row ConsultarPorCursoCurriculo(string curso, string turno, string curriculo)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                object[] parametros = new object[] { curso, turno, curriculo };
                Ly_curriculo.Row consulta = Ly_curriculo.QueryFirstRow(connection, "CURSO = ? and turno = ? AND CURRICULO = ?", parametros);

                return consulta;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Obtém os currículos por parâmetros
        /// </summary>
        /// <param name="curso"></param>
        /// <param name="turno"></param>
        /// <param name="curriculo"></param>
        /// <returns>Currículo</returns>
        /// <autor>Anderson Wernek</autor>
        public static QueryTable ObtemGradePor(string pCurso, string pTurno, string pCurriculo, decimal pSerie)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = @"
                            SELECT g.*
                            FROM   ly_grade g
                            WHERE  g.turno = ?
                                   AND g.curso = ?
                                   AND g.curriculo = ? 
                                   AND g.serie_ideal = ?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, pTurno, pCurso, pCurriculo, pSerie);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        //excluir matriz curricular
        public static RetValue Excluir(string curso, string curriculo, string turno)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);
            try
            {
                Ly_curriculo dtCurriculo = Ly_curriculo.Query(connection, "CURSO = ? AND CURRICULO = ? and turno = ?", curso, curriculo, turno);

                if (dtCurriculo != null)
                {
                    if (dtCurriculo.Rows != null)
                    {
                        foreach (Ly_curriculo.Row linha in dtCurriculo.Rows)
                        {
                            linha.Delete();
                        }

                        dtCurriculo.Put(connection);
                        retorno = VerificarErro(dtCurriculo);

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Matriz curricular removida com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        //inclui matriz curricular
        public static RetValue Incluir(Ly_curriculo dtCurriculo)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;
            connection.Open(true);

            try
            {
                if (dtCurriculo != null)
                {
                    if (dtCurriculo.Rows != null)
                    {
                        ColunasTable colunas = MontarParametros(dtCurriculo.Columns, dtCurriculo.Rows[0]);
                        Ly_curriculo.Row.Insert(connection, dtCurriculo.Rows[0].Curso, dtCurriculo.Rows[0].Turno, dtCurriculo.Rows[0].Curriculo, dtCurriculo.Rows[0].Ano_ini, dtCurriculo.Rows[0].Sem_ini, dtCurriculo.Rows[0].Dt_homolog, dtCurriculo.Rows[0].Regime, dtCurriculo.Rows[0].Aulas_previstas, dtCurriculo.Rows[0].Creditos, dtCurriculo.Rows[0].Prazo_ideal, dtCurriculo.Rows[0].Prazo_max, dtCurriculo.Rows[0].Credmin_matr, dtCurriculo.Rows[0].Tranc_max, dtCurriculo.Rows[0].Tranc_cons_max, dtCurriculo.Rows[0].Tranc_max_discip, dtCurriculo.Rows[0].Canc_max_discip, dtCurriculo.Rows[0].Atlz_max_discip, dtCurriculo.Rows[0].Ratear_mens, dtCurriculo.Rows[0].Credmax_matr, dtCurriculo.Rows[0].Tranc_interv_data, dtCurriculo.Rows[0].Tese_dissertacao, dtCurriculo.Rows[0].Pesquisa, colunas.Colunas, colunas.ValorColuna);
                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Matriz curricular incluída com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        //altera matriz curricular
        public static RetValue Alterar(Ly_curriculo dtCurriculo)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtCurriculo != null)
                {
                    if (dtCurriculo.Rows != null)
                    {
                        ColunasTable colunas = MontarParametros(dtCurriculo.Columns, dtCurriculo.Rows[0]);
                        Ly_curriculo.Row.Update(connection, dtCurriculo.Rows[0].Curso, dtCurriculo.Rows[0].Turno, dtCurriculo.Rows[0].Curriculo, colunas.Colunas, colunas.ValorColuna);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        retorno = new RetValue(true, "Matriz curricular alterada com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }


        public static QueryTable Consultar(string turno, string curso, decimal ano, decimal periodo)
        {
            TConnection connection = Config.CreateConnection();
            try
            {
                connection.Open();
                return Consultar(connection, turno, curso, ano, periodo);
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarTodos(string turno, string curso, decimal ano, decimal periodo)
        {
            TConnection connection = Config.CreateConnection();
            try
            {
                connection.Open();
                return ConsultarTodos(connection, turno, curso, ano, periodo);
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable Consultar(TConnection connection, string turno, string curso, decimal ano, decimal periodo)
        {
            QueryTable qt = null;
            var sql = @"SELECT  *
                FROM    LY_CURRICULO
                WHERE   TURNO = ?
                        AND CURSO = ?
                        AND ANO_INI = ?
                        AND SEM_INI = ?
                        AND ( DT_EXTINCAO IS NULL
                              OR DT_EXTINCAO > GETDATE()
                            )
                ORDER BY CURRICULO";
            qt = new QueryTable(sql);
            qt.Query(connection, turno, curso, ano, periodo);
            return qt;
        }

        public static QueryTable ConsultarTodos(TConnection connection, string turno, string curso, decimal ano, decimal periodo)
        {
            QueryTable qt = null;
            var sql = @"SELECT  *
                FROM    LY_CURRICULO
                WHERE   TURNO = ?
                        AND CURSO = ?
                        AND ANO_INI = ?
                        AND SEM_INI = ?                        
                ORDER BY CURRICULO";
            qt = new QueryTable(sql);
            qt.Query(connection, turno, curso, ano, periodo);
            return qt;
        }

        public static QueryTable Consultar(DbObject pcurso, string pturno, string ano, string semestre)
        {
            if (ano == string.Empty)
                ano = "0";
            if (semestre == string.Empty)
                semestre = "0";
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT CURRICULO FROM LY_CURRICULO WHERE TURNO = ? AND CURSO = ? AND ANO_INI = ?  ";

            if (semestre == "1")
                sql += " AND SEM_INI <> '2'";
            if (semestre == "2")
                sql += " AND SEM_INI <> '1'";

            sql += " ORDER BY CURRICULO";
            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, pturno, pcurso, ano);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable Consultar(DbObject pcurso, string pturno, string ano)
        {
            if (ano == string.Empty)
                ano = "0";

            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT DISTINCT CURRICULO FROM LY_CURRICULO WHERE TURNO = ? AND CURSO = ? AND ANO_INI = ? ORDER BY CURRICULO";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, pturno, pcurso, ano);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static string Consultar(string turno, string curso, decimal ano)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT top 1 CURRICULO FROM LY_CURRICULO WHERE TURNO = ? AND CURSO = ? AND ANO_INI = ? ORDER BY CURRICULO";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, turno, curso, ano);
            }
            finally
            {
                connection.Close();
            }

            if (qt.Rows.Count > 0)
                return qt.Rows[0][0].ToString();
            else
                return string.Empty;
        }

        public static QueryTable Consultar()
        {
            TConnectionWritable connection = Config.CreateWritableConnection();

            connection.Open();
            QueryTable qt = null;

            string sql = "SELECT DISTINCT CURRICULO FROM LY_CURRICULO";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarTurno(string curso)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT DISTINCT T.TURNO, T.DESCRICAO FROM LY_CURRICULO C INNER JOIN LY_TURNO T on C.TURNO = T.TURNO WHERE CURSO = ?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, curso);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ComboConsultar(DbObject curso, string turno)
        {
            string scurso = string.IsNullOrEmpty(curso.ToString()) ? string.Empty : curso.ToString();
            turno = string.IsNullOrEmpty(turno) ? string.Empty : turno;

            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "SELECT CURRICULO FROM LY_CURRICULO WHERE TURNO = ? AND CURSO = ? ORDER BY CURRICULO";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, turno, scurso);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ComboConsultarSerie(DbObject curso, string turno, string serie)
        {
            string scurso = string.IsNullOrEmpty(curso.ToString()) ? string.Empty : curso.ToString();
            turno = string.IsNullOrEmpty(turno) ? string.Empty : turno;
            decimal dserie = string.IsNullOrEmpty(serie) ? 0 : Convert.ToDecimal(serie);

            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "select distinct a.curriculo, a.curriculo descricao FROM ly_serie a WHERE a.TURNO = ? AND a.CURSO = ? and serie = ?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, turno, scurso, serie);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarPorSerie(DbObject curso, string turno, string serie)
        {
            string scurso = string.IsNullOrEmpty(curso.ToString()) ? string.Empty : curso.ToString();
            turno = string.IsNullOrEmpty(turno) ? string.Empty : turno;
            decimal dserie = string.IsNullOrEmpty(serie) ? 0 : Convert.ToDecimal(serie);

            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "select distinct a.curriculo FROM ly_serie a WHERE a.TURNO = ? AND a.CURSO = ? and serie = ?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, turno, scurso, serie);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarGrades(string curso, string turno, string curriculo)
        {
            TConnection cn = Config.CreateConnection();

            QueryTable qt = new QueryTable("SELECT DISTINCT curso, turno, curriculo, g.disciplina as disciplina, d.NOME as nomedisciplina, serie_ideal, obrigatoria, permite_glp, area_conhecimento, " +
                                            "SUM(isnull(HORAS_AULA,0) + isnull(HORAS_LAB,0) + isnull(HORAS_ATIV,0) +isnull(HORAS_ESTAGIO,0))as cargahoraria,I.DESCR componente, macro, m.NOME as macro_nome " +
                                            "FROM ly_grade g " +
                                            "INNER JOIN LY_DISCIPLINA d on d.DISCIPLINA = g.DISCIPLINA " +
                                            "LEFT JOIN TCE_MACRO_CAMPOS m on m.ID_MACRO_CAMPOS = g.macro " +
                                            "LEFT JOIN ITEMTABELA I ON I.TAB = 'ComponenteDisciplina' AND ITEM=D.COMPONENTE " +
                                            "WHERE CURSO = ? and turno = ? AND CURRICULO = ? " +
                                            "GROUP BY curso, curriculo, g.DISCIPLINA, d.NOME, SERIE_IDEAL, OBRIGATORIA, PERMITE_GLP, AREA_CONHECIMENTO, I.DESCR , TURNO, MACRO, m.NOME " +
                                            "ORDER BY serie_ideal, d.NOME ");

            qt.Query(cn, curso, turno, curriculo);

            return qt;
        }

        public static RetValue AlterarGrade(Ly_grade dtGrade)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtGrade != null)
                {
                    if (dtGrade.Rows != null)
                    {
                        //QueryTable qt = new QueryTable("SELECT DISTINCT turno FROM ly_curriculo WHERE curso = ? and curriculo = ?");
                        //qt.Query(connection, dtGrade.Rows[0].Curso, dtGrade.Rows[0].Curriculo);

                        //for (int i = 0; i < qt.Rows.Count; i++)
                        //{
                        //    dtGrade.Rows[0].Turno = qt.Rows[i]["turno"].ToString();
                        Ly_grade.Row.Update(connection, dtGrade.Rows[0].Curso, dtGrade.Rows[0].Turno, dtGrade.Rows[0].Curriculo, dtGrade.Rows[0].Disciplina, MontarParametros(dtGrade.Columns, dtGrade.Rows[0]).Colunas, MontarParametros(dtGrade.Columns, dtGrade.Rows[0]).ValorColuna);
                        //}

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

        public static RetValue IncluirGrade(Ly_grade dtGrade)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtGrade != null)
                {
                    if (dtGrade.Rows != null)
                    {
                        //Ly_grade.Row.Insert(connection, dtGrade.Rows[0].Curso, dtGrade.Rows[0].Turno, dtGrade.Rows[0].Curriculo, dtGrade.Rows[0].Disciplina, dtGrade.Rows[0].Serie_ideal, dtGrade.Rows[0].Obrigatoria,dtGrade.Rows[0].Macro, dtGrade.Rows[0].Max_matr_aprov, MontarParametros(dtGrade.Columns, dtGrade.Rows[0]).Colunas, MontarParametros(dtGrade.Columns, dtGrade.Rows[0]).ValorColuna);
                        Ly_grade.Row.Insert(connection, dtGrade.Rows[0].Curso, dtGrade.Rows[0].Turno, dtGrade.Rows[0].Curriculo, dtGrade.Rows[0].Disciplina, dtGrade.Rows[0].Serie_ideal, dtGrade.Rows[0].Obrigatoria, dtGrade.Rows[0].Max_matr_aprov, MontarParametros(dtGrade.Columns, dtGrade.Rows[0]).Colunas, MontarParametros(dtGrade.Columns, dtGrade.Rows[0]).ValorColuna);
                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        return new RetValue(true, "Registro incluído com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static RetValue ExcluirGrade(Ly_grade dtGrade)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                if (dtGrade != null)
                {
                    if (dtGrade.Rows != null)
                    {
                        Ly_grade.Row.Delete(connection, dtGrade.Rows[0].Curso, dtGrade.Rows[0].Turno, dtGrade.Rows[0].Curriculo, dtGrade.Rows[0].Disciplina);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        return new RetValue(true, "Registro excluído com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static QueryTable ListarEscolaridades(string curso, string turno, string curriculo)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = new QueryTable("SELECT DISTINCT serie FROM ly_serie WHERE curso = ? and turno = ? AND curriculo = ?");

            try
            {
                qt.Query(connection, curso, turno, curriculo);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarUnidadesFisicas(DbObject curso, DbObject curriculo)
        {
            string sql = "select CURSO, TURNO, CURRICULO, FACULDADE, CHAVE from LY_CURRICULO_UNIDADE_FISICA WHERE CURSO = ? and CURRICULO = ? ";
            return Consultar(sql, curso, curriculo);
        }

        public static RetValue IncluirUnidadeFisica(Ly_curriculo_unidade_fisica dtUnidadeFisica)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtUnidadeFisica != null)
                {
                    if (dtUnidadeFisica.Rows != null)
                    {
                        QueryTable qt = new QueryTable("SELECT DISTINCT turno FROM ly_curriculo WHERE curso = ? and curriculo = ?");
                        qt.Query(connection, dtUnidadeFisica.Rows[0].Curso, dtUnidadeFisica.Rows[0].Curriculo);

                        for (int i = 0; i < qt.Rows.Count; i++)
                        {
                            dtUnidadeFisica.Rows[0].Turno = qt.Rows[i]["turno"].ToString();
                            dtUnidadeFisica.Put(connection);
                        }
                        retorno = VerificarErro(dtUnidadeFisica);

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        return new RetValue(true, "Registro incluído com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static RetValue ExcluirUnidadeFisica(Ly_curriculo_unidade_fisica dtUnidadeFisica)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                if (dtUnidadeFisica != null)
                {
                    if (dtUnidadeFisica.Rows != null)
                    {
                        QueryTable qt = new QueryTable("SELECT DISTINCT turno FROM ly_curriculo WHERE curso = ? and curriculo = ?");
                        qt.Query(connection, dtUnidadeFisica.Rows[0].Curso, dtUnidadeFisica.Rows[0].Curriculo);

                        for (int i = 0; i < qt.Rows.Count; i++)
                        {
                            dtUnidadeFisica.Rows[0].Turno = qt.Rows[i]["turno"].ToString();
                            Ly_curriculo_unidade_fisica.Row.Delete(connection, dtUnidadeFisica.Rows[0].Curso, dtUnidadeFisica.Rows[0].Turno, dtUnidadeFisica.Rows[0].Curriculo, dtUnidadeFisica.Rows[0].Faculdade, dtUnidadeFisica.Rows[0].Chave);
                        }

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        return new RetValue(true, "Registro excluído com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static QueryTable ConsultarSerie(string curso, string turno, string ano, string periodo)
        {
            string sql = "select Distinct SERIE, descricao from ly_serie s inner join LY_CURRICULO c on s.CURSO = c.CURSO and s.TURNO = c.TURNO and s.CURRICULO = c.CURRICULO where c.CURSO = ? and c.TURNO = ? and c.ANO_INI = ? and c.SEM_INI = ?";
            return Consultar(sql, curso, turno, ano, periodo);
        }

        public static QueryTable ConsultarCurriculo(string curso, string turno)
        {
            string sql = "select distinct curriculo from ly_curriculo where curso = ? and turno = ?";
            return Consultar(sql, curso, turno);
        }

        public static string ObterAnoPeriodoCurriculo(TConnection cn, string curso, string turno, string curriculo)
        {
            QueryTable qt = new QueryTable("Select convert(varchar,ano_ini) + '|' + convert(varchar,sem_ini) anoPeriodo from ly_curriculo where curso = ? and turno = ? and curriculo = ?");
            qt.Query(cn, curso, turno, curriculo);
            return qt.Rows[0]["anoPeriodo"].ToString();
        }

        public static QueryTable ConsultarCurriculPorAnoSemestreCursoTurno(String curso, String turno, decimal ano, decimal semestre)
        {
            StringBuilder sql = new StringBuilder();
            List<object> parametros = new List<object>();

            sql.AppendLine(@"SELECT DISTINCT curriculo FROM ly_curriculo WHERE ano_ini = ? AND sem_ini = ?");
            parametros.Add(ano);
            parametros.Add(semestre);

            if (!string.IsNullOrEmpty(curso))
            {
                sql.AppendLine(" AND curso = ?");
                parametros.Add(curso);
            }

            if (!string.IsNullOrEmpty(turno))
            {
                sql.AppendLine(" AND turno = ?");
                parametros.Add(turno);
            }

            sql.AppendLine("ORDER BY curriculo");

            return Consultar(sql.ToString(), parametros.ToArray());
        }

        //        public static String RetornaCurriculo(String curso, String turno, int ano, int semestre, int serie)
        //        {
        //            string curriculo = string.Empty;

        //            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
        //            {
        //                var contextQuery = new ContextQuery
        //                {
        //                    Command = @" SELECT DISTINCT CURRICULO 
        //                                    FROM LY_CURRICULO 
        //                                    WHERE ANO = @ANO 
        //                                    AND SEMESTRE = @SEMESTRE 
        //                                    AND CURSO = @CURSO 
        //                                    AND TURNO = @TURNO 
        //                                    AND SERIE = @SERIE "
        //                };

        //                contextQuery.Parameters.Add("@ANO", ano);
        //                contextQuery.Parameters.Add("@SEMESTRE", semestre);
        //                contextQuery.Parameters.Add("@CURSO", curso);
        //                contextQuery.Parameters.Add("@TURNO", turno);
        //                contextQuery.Parameters.Add("@SERIE", serie);

        //                using (var reader = ctx.GetDataReader(contextQuery))
        //                {
        //                    if (reader.Read())
        //                    {
        //                        curriculo = Convert.ToString(reader["curriculo"]);
        //                    }
        //                }

        //                return curriculo;
        //            }
        //        }

        public static DataTable ConsultarMatrizAluno(string turno, DbObject curso)
        {
            if (curso != null && !curso.IsNull)
            {
                var contextQuery = new ContextQuery
                    (@"SELECT  CURRICULO
                FROM    LY_CURRICULO
                WHERE  TURNO = @TURNO
                        AND CURSO = @CURSO
                        AND ( DT_EXTINCAO IS NULL
                                      OR DT_EXTINCAO > GETDATE()
                                    )
                ORDER BY  ANO_INI DESC,CURRICULO ");

                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso.ToString());

                return Consultar(contextQuery);
            }
            return null;
        }

        public static DataTable ListarMacrosRelacionadas(string curriculo, string curso, string turno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    //Lista os macros cadastrados e as disciplinas relacionadas
                    Command =
                        @" SELECT DISTINCT
                                    M.ID_MACRO_CAMPOS, M.NOME AS NOME_MACRO, OBRIGATORIO
                            FROM    TCE_MACRO_CAMPOS M
                                    INNER JOIN LY_GRADE G ON M.ID_MACRO_CAMPOS = G.MACRO
                                    INNER JOIN DBO.LY_DISCIPLINA D ON G.DISCIPLINA = D.DISCIPLINA
                            WHERE   g.CURRICULO = @CURRICULO
                                    AND g.CURSO = @CURSO
                                    AND g.TURNO = @TURNO "
                };
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public bool ExisteDisciplinaObrigatoriaPor(string curriculo, string curso, string turno, int serie, int idMacro)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool disciplinaObrigatoria = false;

            try
            {
                contextQuery.Command = @" SELECT  COUNT(*)
                        FROM    LY_GRADE G ( NOLOCK )
                                INNER JOIN LY_DISCIPLINA D ( NOLOCK ) ON D.DISCIPLINA = G.DISCIPLINA
                        WHERE   G.CURSO = @CURSO
                                AND G.TURNO = @TURNO
                                AND G.CURRICULO = @CURRICULO
                                AND G.OBRIGATORIA = 'S'
                                AND G.SERIE_IDEAL = @SERIE
                                AND MACRO = @MACRO ";

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@MACRO", idMacro);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    disciplinaObrigatoria = true;
                }

                return disciplinaObrigatoria;
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

        public bool ExisteCurriculoAtivoPor(DataContext ctx, int ano, int periodo, string curso, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT  COUNT(*)
	                                    FROM LY_CURRICULO C
		                                    INNER JOIN LY_GRADE G on C.CURSO = G.CURSO and C.TURNO = G.TURNO and C.CURRICULO = G.CURRICULO
	                                    WHERE C.ANO_INI = @ANO
		                                    AND C.SEM_INI = @PERIODO	                                   
		                                    AND C.CURSO = @CURSO 
											AND G.SERIE_IDEAL = @SERIE
											AND (C.DT_EXTINCAO IS NULL OR C.DT_EXTINCAO > GETDATE()) ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public static DataTable ListaCurriculoPor(string curriculo, string curso, string turno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT DISTINCT MD.MODALIDADE,
                                   ISNULL(ENSINO_RELIGIOSO, 'N') AS PODE_ENSINO_RELIGIOSO, ISNULL(LINGUA_ESTRANGEIRA, 'N') AS PODE_LINGUA_ESTRANGEIRA,*
                            FROM    LY_CURRICULO cm
                        INNER JOIN dbo.LY_CURSO C ON CM.CURSO = C.CURSO
                        INNER JOIN dbo.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE
                            WHERE   cm.CURRICULO = @CURRICULO
                                    AND cm.CURSO = @CURSO
                                    AND cm.TURNO = @TURNO "
                };
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);

                return ctx.GetDataTable(contextQuery);
            }
        }


        public static DataTable ListaCurriculoPor(string curso, string turno)
        {
            DataTable dt = new DataTable();
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" SELECT DISTINCT
                                   ISNULL(ENSINO_RELIGIOSO, 'N') AS PODE_ENSINO_RELIGIOSO, ISNULL(LINGUA_ESTRANGEIRA, 'N') AS PODE_LINGUA_ESTRANGEIRA,*
                            FROM    LY_CURRICULO cm
                        INNER JOIN dbo.LY_CURSO C ON CM.CURSO = C.CURSO
                        INNER JOIN dbo.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE
                            WHERE    cm.CURSO = @CURSO
                                    AND cm.TURNO = @TURNO "
                };

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);

                dt = ctx.GetDataTable(contextQuery);
                return dt;
            }
        }
        public static bool PodeAlterarCurriculo(Techne.Lyceum.CR.Ly_grade.Row dadosGrade)
        {
            DataTable dt = new DataTable();

            if (dadosGrade != null)
            {
                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                                           {
                                               Command = @"SELECT DISTINCT  *
                    FROM    DBO.LY_GRADE
                    WHERE   TURNO = @TURNO
                            AND CURSO = @CURSO
                            AND CURRICULO = @CURRICULO
                            AND DISCIPLINA = @DISCIPLINA
                             "
                                           };
                    contextQuery.Parameters.Add("@TURNO", dadosGrade.Turno);
                    contextQuery.Parameters.Add("@CURSO", dadosGrade.Curso);
                    contextQuery.Parameters.Add("@CURRICULO", dadosGrade.Curriculo);
                    contextQuery.Parameters.Add("@DISCIPLINA", dadosGrade.Disciplina);


                    dt = ctx.GetDataTable(contextQuery);

                    if (dt.Rows.Count > 0)
                    {
                        Decimal? serieIdeal = null;
                        Decimal? macro = null;

                        if (dt.Rows[0]["SERIE_IDEAL"] != DBNull.Value)
                            serieIdeal = Convert.ToDecimal(dt.Rows[0]["SERIE_IDEAL"]);

                        if (dt.Rows[0]["MACRO"] != DBNull.Value)
                            macro = Convert.ToDecimal(dt.Rows[0]["MACRO"]);

                        if (serieIdeal != dadosGrade.Serie_ideal || macro != dadosGrade.Macro || Convert.ToString(dt.Rows[0]["OBRIGATORIA"]) != dadosGrade.Obrigatoria)
                        {
                            return false;
                        }
                    }

                    return true;
                }

            }
            return false;
        }

        public static string RetornaCurriculo(string curso, string turno, int ano, int periodo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  TOP 1 CURRICULO
                        FROM    LY_CURRICULO
                        WHERE   TURNO = @TURNO
                                AND CURSO = @CURSO
                                AND ANO_INI = @ANO
                                AND SEM_INI = @PERIODO
                                AND ( DT_EXTINCAO IS NULL
                                      OR DT_EXTINCAO > GETDATE()
                                    )
                         ");

                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                return ctx.GetReturnValue<string>(contextQuery);
            }
        }

        public LyCurriculo ObtemPrimeiroAtivoPor(string curso, string turno, int serie, int ano, int periodo)
        {
            LyCurriculo curriculo = new LyCurriculo();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1
                                                CUR.*
                                        FROM    LY_CURRICULO CUR
		                                        INNER JOIN LY_GRADE G ON G.CURSO = CUR.CURSO
                                                                                AND G.SERIE_IDEAL = @SERIE
                                                                                AND G.CURRICULO = CUR.CURRICULO
                                                                                AND G.TURNO = CUR.TURNO
                                        WHERE     CUR.SEM_INI = @PERIODO
                                                AND CUR.ANO_INI = @ANO
                                                AND CUR.CURSO = @CURSO
                                                AND CUR.TURNO = @TURNO
                                                AND ( CUR.DT_EXTINCAO IS NULL
                                                        OR CUR.DT_EXTINCAO > GETDATE()
                                                    ) ";

                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@SERIE", serie);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    curriculo.AnoIni = ano;
                    curriculo.SemIni = periodo;
                    curriculo.Curso = curso;
                    curriculo.Turno = turno;
                    curriculo.Curriculo = Convert.ToString(reader["CURRICULO"]);
                    curriculo.LinguaEstrangeira = string.IsNullOrEmpty(Convert.ToString(reader["LINGUA_ESTRANGEIRA"])) ? "N" : Convert.ToString(reader["LINGUA_ESTRANGEIRA"]);
                    curriculo.EnsinoReligioso = string.IsNullOrEmpty(Convert.ToString(reader["ENSINO_RELIGIOSO"])) ? "N" : Convert.ToString(reader["ENSINO_RELIGIOSO"]);
                }

                return curriculo;
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

        public LyCurriculo ObtemCurriculoPor(int ano, int periodo, string turno, string curso, string unidade, string turma)
        {
            LyCurriculo curriculo = new LyCurriculo();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT c.*
                    FROM    LY_CURRICULO c
                            INNER JOIN dbo.LY_TURMA t ON t.CURRICULO = c.CURRICULO
                                                         AND t.CURSO = c.CURSO
                                                         AND t.turno = c.TURNO
                                                         AND t.ANo = c.ANO_INI
                                                         AND t.SEMESTRE = c.SEM_INI
                    WHERE   t.ANO = @ANO
                            AND t.SEMESTRE = @SEMESTRE
                            AND t.TURNO = @TURNO
                            AND t.CURSO = @CURSO
                            AND t.FACULDADE = @UNIDADE
                            AND t.TURMA = @TURMA ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@UNIDADE", unidade);
                contextQuery.Parameters.Add("@TURMA", turma);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    curriculo.AnoIni = ano;
                    curriculo.SemIni = periodo;
                    curriculo.Curso = curso;
                    curriculo.Turno = turno;
                    curriculo.Curriculo = Convert.ToString(reader["CURRICULO"]);
                    curriculo.EnsinoReligioso = string.IsNullOrEmpty(Convert.ToString(reader["ENSINO_RELIGIOSO"])) ? "N" : Convert.ToString(reader["ENSINO_RELIGIOSO"]);
                    curriculo.LinguaEstrangeira = string.IsNullOrEmpty(Convert.ToString(reader["LINGUA_ESTRANGEIRA"])) ? "N" : Convert.ToString(reader["LINGUA_ESTRANGEIRA"]);
                }

                return curriculo;
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

        public string ObtemPrimeiroCurriculoAtivoPor(DataContext contexto, int controleVagaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 CUR.CURRICULO 
                                        FROM   LY_CURRICULO CUR (NOLOCK) 
                                               INNER JOIN TCE_CONTROLE_VAGA CV (NOLOCK) 
                                                       ON CUR.SEM_INI = CV.PERIODO 
                                                          AND CUR.ANO_INI = CV.ANO 
                                                          AND CUR.CURSO = CV.CURSO 
                                                          AND CUR.TURNO = CV.TURNO 
                                                          AND ( CUR.DT_EXTINCAO IS NULL 
                                                                 OR CUR.DT_EXTINCAO > GETDATE() ) 
                                               INNER JOIN LY_GRADE G (NOLOCK) 
                                                       ON G.CURSO = CUR.CURSO 
                                                          AND G.SERIE_IDEAL = CV.SERIE 
                                                          AND G.CURRICULO = CUR.CURRICULO 
                                                          AND G.TURNO = CUR.TURNO 
                                        WHERE  CV.ID_CONTROLE_VAGA = @ID_CONTROLE_VAGA 
                                        ORDER  BY CUR.STAMP_ATUALIZACAO DESC  ";

            contextQuery.Parameters.Add("@ID_CONTROLE_VAGA", SqlDbType.Int, controleVagaId);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemPrimeiroCurriculoAtivoPor(DataContext contexto, string curso, string turno, int serie, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 CUR.CURRICULO 
                                        FROM   LY_CURRICULO CUR (NOLOCK) 
                                               INNER JOIN LY_GRADE G (NOLOCK) 
                                                       ON G.CURSO = CUR.CURSO 
                                                          AND G.SERIE_IDEAL = @SERIE 
                                                          AND G.CURRICULO = CUR.CURRICULO 
                                                          AND G.TURNO = CUR.TURNO 
                                        WHERE  CUR.SEM_INI = @PERIODO 
                                                          AND CUR.ANO_INI = @ANO 
                                                          AND CUR.CURSO = @CURSO 
                                                          AND CUR.TURNO = @TURNO 
                                                          AND ( CUR.DT_EXTINCAO IS NULL 
                                                                 OR CUR.DT_EXTINCAO > GETDATE() )  
                                        ORDER  BY CUR.STAMP_ATUALIZACAO DESC  ";

            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@SERIE", serie);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemPrimeiroCurriculoEletivaAtivoPor(DataContext contexto, int grupo, string turno, int serie, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @"  SELECT TOP 1 CUR.CURRICULO 
                                        FROM   LY_CURRICULO CUR (NOLOCK) 
                                               INNER JOIN LY_GRADE G (NOLOCK) 
                                                       ON G.CURSO = CUR.CURSO 
                                                          AND G.SERIE_IDEAL = @SERIE 
                                                          AND G.CURRICULO = CUR.CURRICULO 
                                                          AND G.TURNO = CUR.TURNO 
												INNER JOIN LY_DISCIPLINA D (NOLOCK)
														ON D.DISCIPLINA = G.DISCIPLINA
                                        WHERE  CUR.SEM_INI = @PERIODO 
                                                          AND CUR.ANO_INI = @ANO 
                                                          AND CUR.CURSO = @CURSOELETIVA
														  AND D.ELETIVA = 'S'
														  AND D.GRUPO = @GRUPO
                                                          AND CUR.TURNO = @TURNO 
                                                          AND ( CUR.DT_EXTINCAO IS NULL 
                                                                 OR CUR.DT_EXTINCAO > GETDATE() )  
                                        ORDER  BY CUR.STAMP_ATUALIZACAO DESC ";

            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURSOELETIVA", "9999.80");
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@SERIE", serie);
            contextQuery.Parameters.Add("@GRUPO", grupo);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public int ObtemGrupoEletivaPor(int ano, int periodo, int serie, string curriculo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT TOP 1 GRUPO
                                        FROM   LY_CURRICULO CUR (NOLOCK) 
                                               INNER JOIN LY_GRADE G (NOLOCK) 
                                                       ON G.CURSO = CUR.CURSO 
                                                          AND G.SERIE_IDEAL = @SERIE 
                                                          AND G.CURRICULO = CUR.CURRICULO 
                                                          AND G.TURNO = CUR.TURNO 
												INNER JOIN LY_DISCIPLINA D (NOLOCK)
														ON D.DISCIPLINA = G.DISCIPLINA
                                        WHERE  CUR.SEM_INI = @PERIODO 
                                                          AND CUR.ANO_INI = @ANO 
														  AND CUR.CURRICULO = @CURRICULO ";

                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["GRUPO"]);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ano"></param>
        /// <param name="periodo"></param>
        /// <param name="unidade"></param>
        /// <param name="curso"></param>
        /// <param name="turno"></param>
        /// <returns></returns>
        public static DataTable ListaCurriculoRenovacaoMatriculaPor(int ano, int periodo, string unidade, string curso, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable curriculo = null;

            try
            {
                contextQuery.Command = string.Format(@"
                SELECT DISTINCT C.curriculo AS CURRICULO, 
                                C.ensino_religioso AS PODE_ENSINO_RELIGIOSO, 
                                C.lingua_estrangeira AS PODE_LINGUA_ESTRANGEIRA 
                FROM   ly_grade G 
                       INNER JOIN ly_curriculo C 
                               ON G.curriculo = C.curriculo 
                                  AND G.curso = C.curso 
                                  AND G.turno = C.turno 
                       INNER JOIN ly_unidade_ensino_cursos U 
                               ON ( U.curso = C.curso ) 
                WHERE  C.ano_ini = {0} --Ano 
                       AND C.sem_ini = {1} --Período 
                       AND U.unidade_ens = '{2}' --Escola selecionada 
                       AND ( dt_extincao IS NULL 
                              OR CONVERT(DATE, dt_extincao) > CONVERT(DATE, Getdate()) ) 
                       AND C.curso = '{3}' --Curso Selecionado  
                       AND C.turno = '{4}' --Turno Selecionado  
                ORDER  BY C.curriculo "
                    , ano
                    , periodo
                    , unidade
                    , curso
                    , turno);

                curriculo = ctx.GetDataTable(contextQuery);
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

            return curriculo;
        }

        public DataTable ObtemCurriculoPor(DataContext contexto, int idConfirmacaoMatricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @"  SELECT DISTINCT C.CURRICULO AS CURRICULO, 
                                                            C.ENSINO_RELIGIOSO AS PODE_ENSINO_RELIGIOSO, 
                                                            C.LINGUA_ESTRANGEIRA AS PODE_LINGUA_ESTRANGEIRA 
                                            FROM  TCE_CONFIRMACAO_MATRICULA CM (NOLOCK)
				                                  INNER JOIN LY_CURRICULO C (NOLOCK)
                                                           ON CM.CURRICULO = C.CURRICULO 
                                                              AND CM.CURSO = C.CURSO 
                                                              AND CM.TURNO = C.TURNO 
								                              AND CM.ANO = C.ANO_INI
								                              AND CM.PERIODO = C.SEM_INI                       
                                            WHERE ID_CONFIRMACAO_MATRICULA = @ID_CONFIRMACAO_MATRICULA
                                            ORDER  BY C.CURRICULO  ";

            contextQuery.Parameters.Add("@ID_CONFIRMACAO_MATRICULA", SqlDbType.Int, idConfirmacaoMatricula);

            dt = contexto.GetDataTable(contextQuery);

            return dt;
        }

        public DataTable ObtemPodeEnsinoReligiosoLinguaEstrangPor(string curriculo, string curso, string turno, int ano, int semestre)
        {
            var contextQuery = new ContextQuery(
                @" SELECT DISTINCT
                        CASE WHEN ENSINO_RELIGIOSO = 'S' THEN 1
                                 ELSE 0
                            END AS PODE_ENSINO_RELIGIOSO ,
                        CASE WHEN LINGUA_ESTRANGEIRA = 'S' THEN 1
                                 ELSE 0
                            END AS PODE_LINGUA_ESTRANGEIRA ,		
                        CM.CURRICULO ,
                        CM.CURSO ,
                        CM.TURNO
                FROM    LY_CURRICULO CM
                        INNER JOIN DBO.LY_CURSO C ON CM.CURSO = C.CURSO
                        INNER JOIN DBO.LY_MODALIDADE_CURSO MD ON C.MODALIDADE = MD.MODALIDADE                                     
            WHERE   CM.CURRICULO = @CURRICULO
                    AND CM.CURSO = @CURSO
                    AND CM.TURNO = @TURNO
                    AND CM.ANO_INI = @ANO
                    AND CM.SEM_INI = @SEMESTRE");

            contextQuery.Parameters.Add("@CURRICULO", curriculo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);

            return Consultar(contextQuery);
        }

        public DataTable ListaCurriculoHorarioOperacionalPor(string curso, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable curriculo = null;

            try
            {
                contextQuery.Command = string.Format(@"
                SELECT DISTINCT C.CURRICULO ,
                                ( C.CURRICULO + '|' + CAST(C.ANO_INI AS VARCHAR) ) AS CURRICULO_ANO
                FROM   LY_CURRICULO C                               
                WHERE  C.ANO_INI > 2009 --ANO                     
                       AND C.CURSO = '{0}' --CURSO SELECIONADO  
                       AND C.TURNO = '{1}' --TURNO SELECIONADO  
                ORDER  BY C.CURRICULO "
                    , curso
                    , turno);

                curriculo = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
            return curriculo;
        }

        public ValidacaoDados ValidaGrade(Techne.Lyceum.CR.Ly_grade.Row dadosGrade, int ano, int periodo, bool cadastro)
        {
            var mensagens = new List<string>();
            DataContext contexto = null;
            RN.Disciplina rnDisciplina = new Disciplina();
            RN.Curso rnCurso = new Curso();
            RN.Turma rnTurma = new Turma();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosGrade == null)
            {
                return validacaoDados;
            }

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (dadosGrade.Disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo COMPONENTE CURRICULAR é obrigatório.");
            }

            if (dadosGrade.Serie_ideal == null || dadosGrade.Serie_ideal <= 0)
            {
                mensagens.Add("Campo ANO DE ESCOLARIDADE é obrigatório.");
            }

            if (dadosGrade.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ESCOLARIDADE é obrigatório.");
            }
            else
            {
                if (dadosGrade.Curso == "9999.92")
                {
                    if ((dadosGrade.Macro == null || dadosGrade.Macro <= 0))
                    {
                        throw new Exception("Esse curso exige preenchimento do campo MACRO");
                    }
                }
                else if (dadosGrade.Macro != null && dadosGrade.Macro > 0)
                {
                    throw new Exception("O campo MACRO apenas pode ser informado para o curso mais educação.");
                }
            }

            if (dadosGrade.Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (dadosGrade.Curriculo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURRICULO é obrigatório.");
            }

            if (dadosGrade.Obrigatoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo OBRIGATORIA é obrigatório.");
            }

            if (dadosGrade.Permite_glp.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo PERMITE GLP é obrigatório.");
            }

            if (!cadastro)
            {
                RetValue ret = RN.Turma.VerificaPodeAlterarGrade(dadosGrade.Curso, dadosGrade.Turno, dadosGrade.Curriculo, Convert.ToDecimal(dadosGrade.Serie_ideal));
                if (ret != null && !ret.Ok)
                {
                    if (!RN.Curriculo.PodeAlterarCurriculo(dadosGrade))
                        mensagens.Add("Não é possível alterar os componentes curriculares que compõem esta matriz curricular. Existem aulas alocadas para turmas desta matriz.");

                }

                //Caso a opção obrigatoria seja retirada, verifica a disciplina já foi utilizada por alguma turmas
                if (dadosGrade.Obrigatoria == "N" && rnTurma.ExisteTurmaDisciplinaComObrigatoriaCadastrada(dadosGrade.Curriculo, dadosGrade.Curso, dadosGrade.Turno, dadosGrade.Disciplina, Convert.ToInt32(dadosGrade.Serie_ideal)))
                {
                   mensagens.Add("Não é possível desmarcar a obrigatoriedade pois já existe turma utilizando o componente curricular.");
                }
            }
       
            if (rnTurma.ExisteTurmaCadastrada(dadosGrade.Curriculo, dadosGrade.Curso, dadosGrade.Turno, Convert.ToInt32(dadosGrade.Serie_ideal)))
            {
                mensagens.Add("Não é possível alterar a matriz pois já existe turma cadastrada.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca compontente da disciplina
                    string componente = rnDisciplina.ObtemComponentePor(contexto, dadosGrade.Disciplina);

                    //Verifica se a disciplina eh marcada como componente de área
                    if (componente.ToUpper() == "COMPONENTE CURRICULAR")
                    {
                        //Verifica se a displina foi atribuida para outra matriz ativa
                        if (this.ExisteOutroCurriculoAtivoPor(contexto, ano, periodo, dadosGrade.Disciplina, dadosGrade.Curso))
                        {
                            //3. Ocorrerá validação no intuito de permitir que cada disciplina do componente “Componente de área” só possa ser 
                            //atribuído a uma trilha (Curso) ativa por ano, a cada momento, ao montar a matriz curricular ocorrerá esta validação.
                            mensagens.Add("Esta disciplina do tipo COMPONENTE DE ÁREA já foi atribuida para outro curso com matriz curricular ativa neste ano / periodo.");
                        }
                        else
                        {
                            //Busca quantidade Máxima de componente de área do curso
                            int? maximoComponentes = rnCurso.ObtemMaximoComponentesPor(contexto, dadosGrade.Curso);

                            //Busca quantidade de componentes curriculares para o curriculo, serie (outras disciplinas)
                            int totalComponentes = this.ObtemTotalComponenteCurricularPor(contexto, dadosGrade.Curso, dadosGrade.Turno, dadosGrade.Curriculo, Convert.ToInt32(dadosGrade.Serie_ideal), dadosGrade.Disciplina);

                            //2. Cada trilha tem um número máximo de Componentes Curriculares que a compõe, que é indicado na tela de escolaridades.
                            if (maximoComponentes != null && totalComponentes >= maximoComponentes)
                            {
                                mensagens.Add("O número máximo de componente curricular do tipo COMPONENTE DE ÁREA já foi atingido para esta série / curso / turno.");
                            }
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

        private int ObtemTotalComponenteCurricularPor(DataContext contexto, string curso, string turno, string curriculo, int serie, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {
                contextQuery.Command = @" 	SELECT COUNT(DISTINCT g.DISCIPLINA) AS QUANTIDADE
	                                        FROM LY_GRADE g
		                                        INNER JOIN LY_DISCIPLINA D ON G.DISCIPLINA = D.DISCIPLINA
	                                        WHERE D.COMPONENTE = 'Componente Curricular'
		                                        AND CURSO = @CURSO
		                                        AND TURNO = @TURNO
		                                        AND CURRICULO = @CURRICULO
		                                        AND SERIE_IDEAL = @SERIE
		                                        AND G.DISCIPLINA <> @DISCIPLINA ";

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@DISCIPLINA", disciplina);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["QUANTIDADE"]);
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

        public bool ExisteOutroCurriculoAtivoPor(DataContext contexto, int ano, int periodo, string disciplina, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" 	SELECT COUNT(1)
	                                    FROM LY_CURRICULO C
		                                    INNER JOIN LY_GRADE G on C.CURSO = G.CURSO and C.TURNO = G.TURNO and C.CURRICULO = G.CURRICULO
	                                    WHERE C.ANO_INI = @ANO
		                                    AND C.SEM_INI = @PERIODO
		                                    AND G.DISCIPLINA = @DISCIPLINA
		                                    AND (C.DT_EXTINCAO IS NULL OR C.DT_EXTINCAO > GETDATE())
		                                    AND C.CURSO <> @CURSO ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@DISCIPLINA", disciplina);
            contextQuery.Parameters.Add("@CURSO", curso);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public ValidacaoDados Valida(Ly_curriculo curriculo, string EnsReligiosoAnt, string LinguaEstrangeiraAnt, bool cadastro)
        {
            var mensagens = new List<string>();
            DataContext contexto = null;
            RN.Turma rnTurma = new Turma();
            RN.Curso rnCurso = new Curso();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (curriculo == null)
            {
                return validacaoDados;
            }

            if (curriculo.Rows[0].Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ESCOLARIDADE é obrigatório.");
            }

            if (curriculo.Rows[0].Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (curriculo.Rows[0].Curriculo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ESPECIFICAÇÃO é obrigatório.");
            }

            if (curriculo.Rows[0].Ano_ini == null || curriculo.Rows[0].Ano_ini <= 0)
            {
                mensagens.Add("Campo ANO DE INÍCIO é obrigatório.");
            }

            if (curriculo.Rows[0].Sem_ini == null || curriculo.Rows[0].Sem_ini < 0)
            {
                mensagens.Add("Campo PERÍODO DE INÍCIO é obrigatório.");
            }

            if (curriculo.Rows[0].Dt_homolog == null || curriculo.Rows[0].Dt_homolog == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE PUBLICAÇÃO D.O. é obrigatório.");
            }
            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca trilha do curso
                    int trilhaAprendizagemId = rnCurso.ObtemTrilhaAprendizagemIdPor(contexto, curriculo.Rows[0].Curso);

                    //Verifica se o curso é de uma trilha 
                    if (trilhaAprendizagemId > 0)
                    {
                        //Verifica se existe outra  matriz curricular ativa no ano / periodo para o turno
                        if (this.ExisteOutroPor(contexto, Convert.ToInt32(curriculo.Rows[0].Ano_ini), Convert.ToInt32(curriculo.Rows[0].Sem_ini), curriculo.Rows[0].Curso, curriculo.Rows[0].Turno, curriculo.Rows[0].Curriculo))
                        {
                            mensagens.Add("Cada curso só pode fazer parte de uma matriz curricular ativa para cada turno no ano / periodo.");
                        }
                    }

                    //Verifica se é alteração
                    if (!cadastro)
                    {
                        //Verifica se foram alteradas as opçoes de optativas
                        if ((EnsReligiosoAnt == "S" && curriculo.Rows[0].Ensino_religioso == "N")
                            || (LinguaEstrangeiraAnt == "S" && curriculo.Rows[0].Lingua_estrangeira == "N"))
                        {
                            if (rnTurma.PossuiTurmaAbertaPor(contexto, Convert.ToDecimal(curriculo.Rows[0].Ano_ini), Convert.ToDecimal(curriculo.Rows[0].Sem_ini), curriculo.Rows[0].Curso, curriculo.Rows[0].Turno, curriculo.Rows[0].Curriculo))
                            {
                                mensagens.Add("Não é permitido desmarcar as Optativas, pois existe TURMA ATIVA para esta matriz curricular. Verifique.");
                            }
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

        private bool ExisteOutroPor(DataContext contexto, int ano, int semestre, string curso, string turno, string curriculo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1)
                                FROM LY_CURRICULO
                                WHERE ANO_INI = @ANO
	                                AND SEM_INI = @SEMESTRE
	                                AND CURSO = @CURSO
	                                AND TURNO = @TURNO
	                                AND (DT_EXTINCAO IS NULL OR DT_EXTINCAO > GETDATE())
	                                AND CURRICULO <> @CURRICULO ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", semestre);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURRICULO", curriculo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ObtemListaTurnoPor(string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable turnos = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT T.TURNO,
                                                            T.DESCRICAO
                                            FROM   LY_CURRICULO C
                                                   INNER JOIN LY_TURNO T
                                                           ON C.TURNO = T.TURNO
                                            WHERE  CURSO = @CURSO  ";

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


        public ValidacaoDados ValidaRemocao(Techne.Lyceum.CR.Ly_grade.Row dadosGrade)
        {
            var mensagens = new List<string>();
            RN.Turma rnTurma = new Turma();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosGrade.Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (dadosGrade.Curriculo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURRICULO é obrigatório.");
            }

            if (dadosGrade.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (dadosGrade.Disciplina.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo DISCIPLINA é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                if (rnTurma.ExisteTurmaCadastradaPor(dadosGrade.Curriculo, dadosGrade.Curso, dadosGrade.Turno, dadosGrade.Disciplina))
                {
                    mensagens.Add("Não é possível alterar a matriz pois já existe turma cadastrada.");
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
    }
}
