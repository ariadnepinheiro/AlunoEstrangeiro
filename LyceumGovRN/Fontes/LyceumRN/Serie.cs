namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.Lyceum.CR;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;
    using System.Configuration;
    using System.Data.SqlClient;

    public class Serie : RNBase
    {
        public static void Alterar(LySerie serie)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery(
                        @"UPDATE  LY_SERIE
                        SET     DESCRICAO = @DESCRICAO, COMPLEMENTO1 = @COMPLEMENTO1,
                                COMPLEMENTO2 = @COMPLEMENTO2, IDADE_MINIMA = @IDADE_MINIMA,
                                DIA_ANIV = @DIA_ANIV, MES_ANIV = @MES_ANIV,
                                STAMP_ATUALIZACAO = GetDate(), DT_EXTINCAO = @DT_EXTINCAO,
                                CURSO_SEGUINTE = @CURSO_SEGUINTE, SERIE_SEGUINTE = @SERIE_SEGUINTE,
                                ANO_SERIE_CONCLUINTE = @ANO_SERIE_CONCLUINTE, MATRICULA = @MATRICULA,
                                EMITE_CERTIFICACAO = @EMITE_CERTIFICACAO, OFERTAELETIVA = @OFERTAELETIVA  
                        WHERE   CURSO = @CURSO
                                AND TURNO = @TURNO
                                AND CURRICULO = @CURRICULO
                                AND SERIE = @SERIE ");

                    contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, serie.Curso);
                    contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, serie.Turno);
                    contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, serie.Curriculo);
                    contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, serie.Serie);
                    contextQuery.Parameters.Add("@DESCRICAO", serie.Descricao);
                    contextQuery.Parameters.Add("@COMPLEMENTO1", serie.Complemento1);
                    contextQuery.Parameters.Add("@COMPLEMENTO2", serie.Complemento2);
                    contextQuery.Parameters.Add("@IDADE_MINIMA", TechneDbType.T_NUMERO, serie.IdadeMinima);
                    contextQuery.Parameters.Add("@DIA_ANIV", TechneDbType.T_NUMERO, serie.DiaAniv);
                    contextQuery.Parameters.Add("@MES_ANIV", TechneDbType.T_NUMERO, serie.MesAniv);
                    contextQuery.Parameters.Add("@DT_EXTINCAO", serie.DtExtincao);
                    contextQuery.Parameters.Add("@CURSO_SEGUINTE", serie.CursoSeguinte);
                    contextQuery.Parameters.Add("@SERIE_SEGUINTE", serie.SerieSeguinte);
                    contextQuery.Parameters.Add("@ANO_SERIE_CONCLUINTE", serie.AnoSerieConcluinte);
                    contextQuery.Parameters.Add("@EMITE_CERTIFICACAO", serie.EmiteCertificacao);
                    contextQuery.Parameters.Add("@OFERTAELETIVA", serie.OfertaEletiva);
                    contextQuery.Parameters.Add("@MATRICULA", serie.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();

                    throw;
                }
            }
        }

        public static RetValue AlterarSerie(Ly_serie dtSerie)
        {
            var connection = Config.CreateWritableConnection();

            connection.Open(true);

            try
            {
                if (dtSerie != null)
                {
                    if (dtSerie.Rows != null)
                    {
                        var colunas = MontarParametros(dtSerie.Columns, dtSerie.Rows[0]);

                        Ly_serie.Row.Update(connection, dtSerie.Rows[0].Curso, dtSerie.Rows[0].Turno, dtSerie.Rows[0].Curriculo, dtSerie.Rows[0].Serie, colunas.Colunas, colunas.ValorColuna);

                        var retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null)
                        {
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

            return null;
        }

        public static string BuscarSerieDescricao(object serie, string curso, string turno, int ano, int semestre)
        {
            if (serie == null || serie.ToString() == string.Empty)
            {
                return string.Empty;
            }

            return Consultar(
                @"select top 1 descricao from ly_serie s
                inner join LY_CURRICULO c on c.CURSO = s.CURSO and c.TURNO = s.TURNO and c.CURRICULO = s.CURRICULO 
                and c.ANO_INI = ? and c.SEM_INI = ? 
                where SERIE = ? and s.TURNO = ? and s.CURSO = ?",
                ano,
                semestre,
                serie.ToString(),
                turno,
                curso).Rows[0]["descricao"].ToString();
        }

        public static string BuscarSerieDescricaoSemAnoPeriodo(object serie, string curso, string turno)
        {
            if (serie == null || serie.ToString() == string.Empty)
            {
                return string.Empty;
            }

            return RNBase.Consultar(
                @"select top 1 descricao from ly_serie s
                inner join LY_CURRICULO c on c.CURSO = s.CURSO and c.TURNO = s.TURNO and c.CURRICULO = s.CURRICULO 
                where SERIE = ? and s.TURNO = ? and s.CURSO = ?",
                serie.ToString(),
                turno,
                curso).Rows[0]["descricao"].ToString();
        }

        public static QueryTable ComboConsultar(DbObject curso, string turno, string curriculo)
        {
            var scurso = string.IsNullOrEmpty(curso.ToString()) ? string.Empty : curso.ToString();

            turno = string.IsNullOrEmpty(turno) ? string.Empty : turno;
            curriculo = string.IsNullOrEmpty(curriculo) ? string.Empty : curriculo;

            var connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt;

            var sql = " SELECT distinct serie, descricao from ly_serie where curso = ? and turno = ? and curriculo = ?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, scurso, turno, curriculo);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ComboConsultar(DbObject curso, string turno)
        {
            var scurso = string.IsNullOrEmpty(curso.ToString()) ? string.Empty : curso.ToString();

            turno = string.IsNullOrEmpty(turno) ? string.Empty : turno;

            var connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt;

            var sql = " SELECT distinct serie, descricao from ly_serie where curso = ? and turno = ?";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, scurso, turno);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultaFiltrosHorOper(string grade)
        {
            var sql = "Select curso, turno, curriculo, serie, faculdade, unidade_responsavel,ano from ly_grade_serie where GRADE_ID = ?";
            var qt = new QueryTable(sql);

            qt.Query(Config.CreateConnection(), Convert.ToDecimal(grade));

            return qt;
        }

        public static QueryTable Consultar(string ano, string periodo, string turno, string curso, string curriculo)
        {
            var connection = Config.CreateConnection();
            connection.Open();

            try
            {
                return Consultar(connection, ano, periodo, turno, curso, curriculo);
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable Consultar(TConnection connection, string ano, string periodo, string turno, string curso, string curriculo)
        {
            var sql = @" 
                    SELECT SERIE, DESCRICAO
                    FROM LY_SERIE LS
                    INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO
                    AND LC.ANO_INI = ? AND LC.SEM_INI = ? AND LC.TURNO = LS.TURNO AND LC.CURSO = LS.CURSO
                    WHERE
                    LS.TURNO = ?
                    AND LS.CURSO = ? AND LS.CURRICULO = ?
                    AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE()))
                    GROUP BY SERIE, DESCRICAO ";

            var qt = new QueryTable(sql);
            qt.Query(connection, ano, periodo, turno, curso, curriculo);
            return qt;
        }

        public static QueryTable Consultar(string ano, string periodo, string turno, string curso)
        {
            var connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt;

            var sql = @" SELECT SERIE, (CONVERT(VARCHAR, SERIE) + '-' + COMPLEMENTO1) AS SERIEPREFIXO, DESCRICAO 
                         FROM LY_SERIE LS 
                         INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO 
                         AND LC.ANO_INI = ? AND LC.SEM_INI = ? AND LC.TURNO = LS.TURNO AND LC.CURSO = LS.CURSO 
                         WHERE 
                         LS.TURNO = ? 
                         AND LS.CURSO = ? 
                         AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE())) 
                         GROUP BY SERIE, COMPLEMENTO1, DESCRICAO ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, ano, periodo, turno, curso);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable Consultar(string curso, string turno, string curriculo)
        {
            var connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt;

            var sql = " SELECT distinct SERIE, descricao from ly_serie where curso = ? and turno = ? and curriculo = ? and (dt_extincao is null or convert(date,dt_extincao) > convert(date, getdate())) ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, curso, turno, curriculo);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarIncluirAluno(string curso, string turno, string curriculo)
        {
            var connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt;

            var sql = " SELECT distinct SERIE, descricao from ly_serie where curso = ? and turno = ? and curriculo = ? and (dt_extincao is null or convert(date,dt_extincao) > convert(date, getdate())) ";

            //Adicionada para atender demanda 3390, bloquear temporariamente series 1 e 6
            var bloquearSeriesIniciaisAluno = Convert.ToBoolean(ConfigurationManager.AppSettings["BloquearSeriesIniciaisTransfAluno"] ?? "false");
            if (bloquearSeriesIniciaisAluno)
            {
                sql += "  AND SERIE NOT IN ( 1, 6 ) ";
            }

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, curso, turno, curriculo);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable Consultar(DbObject pcurso, string pturno, string pcurriculo)
        {
            var connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt;

            var sql = " SELECT distinct SERIE, descricao from ly_serie where curso = ? and turno = ? and curriculo = ? ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, pcurso.ToString(), pturno, pcurriculo);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable Consultar()
        {
            return Consultar("SELECT distinct SERIE, DESCRICAO FROM LY_SERIE");
        }

        public static QueryTable ConsultarCurriculosSeries(DbObject curso, DbObject turno, DbObject curriculo)
        {
            var sql = "select distinct curso, turno, curriculo, serie, descricao, complemento1,complemento2, idade_minima, dia_aniv, mes_aniv, dt_extincao,curso_seguinte, serie_seguinte, ano_serie_concluinte, emite_certificacao,ofertaeletiva from LY_SERIE WHERE curso = ? and turno = ? and curriculo = ? ";

            return Consultar(sql, curso, turno, curriculo);
        }

        public static QueryTable ConsultarSeriesPorCurso(DbObject curso)
        {
            var sql = @"
                        SELECT DISTINCT serie
                        FROM   ly_serie 
                        WHERE  curso = ? ";

            return Consultar(sql, curso);
        }

        public static DataTable ConsultarSeriesPorCursoDt(DbObject curso)
        {
            var sql = @"SELECT DISTINCT serie
                        FROM   ly_serie 
                        WHERE  curso = ? ";

            return Consultar(sql, curso);
        }

        public static string ConsultarPrefixoSerie(string curso, string turno, string curriculo, decimal serie)
        {
            var connection = Config.CreateConnection();

            connection.Open();

            try
            {
                return ConsultarPrefixoSerie(connection, curso, turno, curriculo, serie);
            }
            finally
            {
                connection.Close();
            }
        }

        public static string ConsultarPrefixoSerie(TConnection connection, string curso, string turno, string curriculo, decimal serie)
        {
            var valorConsulta = TCommand.ExecuteScalar(connection, "select COMPLEMENTO1 from ly_serie where curso = ? and turno = ? and curriculo = ? and serie = ?", curso, turno, curriculo, serie);
            if (!valorConsulta.IsNull)
            {
                return (string)valorConsulta;
            }

            return string.Empty;
        }

        public string ObtemPrefixoSeriePor(DataContext contexto, string curso, string turno, string curriculo, decimal serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @"   select COMPLEMENTO1 
                                        from ly_serie 
                                        where curso = @curso 
                                            and turno = @turno 
                                            and curriculo = @curriculo 
                                            and serie = @serie ";

            contextQuery.Parameters.Add("@curso", curso);
            contextQuery.Parameters.Add("@turno", turno);
            contextQuery.Parameters.Add("@curriculo", curriculo);
            contextQuery.Parameters.Add("@serie", serie); 

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public static QueryTable ConsultarSemDtExtincao(string ano, string periodo, string turno, string curso)
        {
            var connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt;

            var sql = " SELECT SERIE, DESCRICAO " +
                      " FROM LY_SERIE LS " +
                      " INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO " +
                      " AND LC.ANO_INI = ? AND LC.SEM_INI = ? AND LC.TURNO = LS.TURNO AND LC.CURSO = LS.CURSO " +
                      " WHERE " +
                      " LS.TURNO = ? " +
                      " AND LS.CURSO = ? " +
                      " GROUP BY SERIE, DESCRICAO ";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, ano, periodo, turno, curso);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static QueryTable ConsultarSerie(string curso)
        {
            return Consultar("select DISTINCT SERIE, DESCRICAO, TURNO, CURRICULO from ly_serie WHERE CURSO = ? ORDER BY DESCRICAO", curso);
        }

        public static QueryTable ConsultarSerie()
        {
            return Consultar("select DISTINCT SERIE, DESCRICAO from ly_serie ORDER BY DESCRICAO");
        }

        public static QueryTable ConsultarSerie(string curso, string turno, string curriculo)
        {
            DbObject valorCurriculo = DBNull.Value, valorCurso = DBNull.Value, valorTurno = DBNull.Value;

            if (!string.IsNullOrEmpty(curso))
            {
                valorCurso = curso;
            }

            if (!string.IsNullOrEmpty(turno))
            {
                valorTurno = turno;
            }

            if (!string.IsNullOrEmpty(curriculo))
            {
                valorCurriculo = curriculo;
            }

            // return Consultar("select DISTINCT serie, descricao, turno, curriculo from ly_serie WHERE CURSO = ? AND TURNO = ? AND CURRICULO = ? ORDER BY DESCRICAO", valorCurso, valorTurno, valorCurriculo);
            return Consultar("select DISTINCT CAST(serie as varchar)+ '|' + turno + '|' + curriculo as serie, descricao, turno, curriculo from ly_serie WHERE CURSO = ? AND TURNO = ? AND CURRICULO = ? ORDER BY DESCRICAO", valorCurso, valorTurno, valorCurriculo);
        }

        public static QueryTable ConsultarSeries(string curso, string turno, string curriculo)
        {
            var sql = "select SERIE, DESCRICAO from ly_serie where CURSO = ? AND TURNO = ? AND CURRICULO = ? ";
            return RNBase.Consultar(sql, curso, turno, curriculo);
        }
        public static QueryTable ConsultarSeries(string curso, string turno)
        {
            var sql = "select DISTINCT SERIE from ly_serie where CURSO = ? AND TURNO = ? AND (DT_EXTINCAO IS NULL OR CONVERT(DATE,DT_EXTINCAO) > CONVERT(DATE, GETDATE())) ORDER BY SERIE ";
            return RNBase.Consultar(sql, curso, turno);
        }

        public static QueryTable ConsultarSufixo(string curso, string turno, string curriculo, string serie)
        {
            using (var connection = Config.CreateConnection())
            {
                connection.Open();
                try
                {
                    return ConsultarSufixo(connection, curso, turno, curriculo, serie);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static QueryTable ConsultarSufixo(TConnection connection, string curso, string turno, string curriculo, string serie)
        {
            if (new[]
                {
                    curso, turno, curriculo, serie
                }.Count(str => string.IsNullOrEmpty(str)) > 0)
            {
                return null;
            }

            var sql = new StringBuilder();

            sql.Append(" select SUFIXO, DESCRICAO from LY_SERIE_SUFIXO ");
            sql.Append(" WHERE ");
            sql.Append(" CURSO = ? AND TURNO = ? AND CURRICULO = ? AND SERIE = ? ");

            return RNBase.Consultar(sql.ToString(), curso, turno, curriculo, serie);
        }

        public static RetValue ExcluirSerie(Ly_serie dtSerie)
        {
            var connection = Config.CreateWritableConnection();

            connection.Open(true);

            try
            {
                if (dtSerie != null)
                {
                    if (dtSerie.Rows != null)
                    {
                        Ly_serie.Row.Delete(connection, dtSerie.Rows[0].Curso, dtSerie.Rows[0].Turno, dtSerie.Rows[0].Curriculo, dtSerie.Rows[0].Serie);

                        var retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null)
                        {
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

            return null;
        }

        public static bool ExisteSerie(string curso, string turno, string curriculo)
        {
            var sqlQuery = "select top 1 1 from ly_serie where curso = ? and turno = ? and curriculo = ?";
            var qt = new QueryTable(sqlQuery);

            qt.Query(Config.CreateConnection(), curso, turno, curriculo);

            return qt.Rows.Count > 0;
        }

        public static bool ExisteSerie(string curso, string curriculo)
        {
            var sqlQuery = "select top 1 1 from ly_serie where curso = ? and curriculo = ?";
            var qt = new QueryTable(sqlQuery);

            qt.Query(Config.CreateConnection(), curso, curriculo);

            return qt.Rows.Count > 0;
        }

        public static RetValue IncluirSerie(Ly_serie dtSerie)
        {
            var connection = Config.CreateWritableConnection();

            connection.Open(true);

            try
            {
                if (dtSerie != null)
                {
                    if (dtSerie.Rows != null)
                    {
                        var colunas = MontarParametros(dtSerie.Columns, dtSerie.Rows[0]);

                        Ly_serie.Row.Insert(connection, dtSerie.Rows[0].Curso, dtSerie.Rows[0].Turno, dtSerie.Rows[0].Curriculo, dtSerie.Rows[0].Serie, dtSerie.Rows[0].Descricao, /*dtSerie.Rows[0].DtExtincao,*/ colunas.Colunas, colunas.ValorColuna);

                        var retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null)
                        {
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

            return null;
        }

        public static void Inserir(LySerie serie)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery(
                        @"INSERT  INTO LY_SERIE ( CURSO, CURRICULO, TURNO, SERIE, DESCRICAO,
                                                        COMPLEMENTO1, COMPLEMENTO2, IDADE_MINIMA, DIA_ANIV,
                                                        MES_ANIV, DT_EXTINCAO, CURSO_SEGUINTE,
                                                        SERIE_SEGUINTE, ANO_SERIE_CONCLUINTE, MATRICULA, EMITE_CERTIFICACAO,OFERTAELETIVA  )
                            VALUES  ( @CURSO, @CURRICULO, @TURNO, @SERIE, @DESCRICAO, @COMPLEMENTO1,
                                      @COMPLEMENTO2, @IDADE_MINIMA, @DIA_ANIV, @MES_ANIV, @DT_EXTINCAO,
                                      @CURSO_SEGUINTE, @SERIE_SEGUINTE, @ANO_SERIE_CONCLUINTE, @MATRICULA, @EMITE_CERTIFICACAO, @OFERTAELETIVA )");

                    contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, serie.Curso);
                    contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, serie.Turno);
                    contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, serie.Curriculo);
                    contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, serie.Serie);
                    contextQuery.Parameters.Add("@DESCRICAO", serie.Descricao);
                    contextQuery.Parameters.Add("@COMPLEMENTO1", serie.Complemento1);
                    contextQuery.Parameters.Add("@COMPLEMENTO2", serie.Complemento2);
                    contextQuery.Parameters.Add("@IDADE_MINIMA", TechneDbType.T_NUMERO, serie.IdadeMinima);
                    contextQuery.Parameters.Add("@DIA_ANIV", TechneDbType.T_NUMERO, serie.DiaAniv);
                    contextQuery.Parameters.Add("@MES_ANIV", TechneDbType.T_NUMERO, serie.MesAniv);
                    contextQuery.Parameters.Add("@DT_EXTINCAO", serie.DtExtincao);
                    contextQuery.Parameters.Add("@CURSO_SEGUINTE", serie.CursoSeguinte);
                    contextQuery.Parameters.Add("@SERIE_SEGUINTE", serie.SerieSeguinte);
                    contextQuery.Parameters.Add("@ANO_SERIE_CONCLUINTE", serie.AnoSerieConcluinte);
                    contextQuery.Parameters.Add("@EMITE_CERTIFICACAO", serie.EmiteCertificacao);
                    contextQuery.Parameters.Add("@OFERTAELETIVA", serie.OfertaEletiva);
                    contextQuery.Parameters.Add("@MATRICULA", serie.Matricula);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();

                    throw;
                }
            }
        }

        public static QueryTable ListarSerie(DbObject curso, string turno, string ano, string semestre)
        {
            if (turno == null)
            {
                turno = string.Empty;
            }

            return Consultar(
                @"select distinct serie, descricao 
                from ly_serie s 
                inner join LY_CURRICULO c on c.CURSO = s.CURSO and c.TURNO = s.TURNO and c.CURRICULO = s.CURRICULO
                where ANO_INI = ?
                and SEM_INI = ?
                and s.CURSO = ?
                and s.TURNO = ?",
                ano,
                semestre,
                curso,
                turno);
        }

        public static DataTable ListarSerie(string censo, decimal ano, decimal periodo, string turno, string curso)
        {
            if (string.IsNullOrEmpty(censo))
            {
                return null;
            }

            var contextQuery = new ContextQuery(@" SELECT DISTINCT
                            S.SERIE, S.SERIE
                    FROM    LY_TURMA T
                            INNER JOIN LY_SERIE S ON T.SERIE = S.SERIE
                    WHERE   T.DT_FIM > GETDATE()
		                    AND SIT_TURMA = 'ABERTA'
                            AND T.CURSO = @CURSO
                            AND S.TURNO = @TURNO
                            AND ANO = @ANO
                            AND SEMESTRE = @SEMESTRE
                            AND FACULDADE = @FACULDADE ");

            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@SEMESTRE", periodo);
            contextQuery.Parameters.Add("@FACULDADE", censo);

            return Consultar(contextQuery);
        }

        public static DataTable ListarSeriePorTurmaUE(string censo, string curso, int ano, int periodo, string turno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT S.SERIE
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
                        AND T.TURNO= @TURNO
                    ");

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TURNO", turno);

                //Adicionada para atender demanda 3341, bloquear temporariamente series 1 e 6
                var bloquearSeriesIniciaisTransfAluno = Convert.ToBoolean(ConfigurationManager.AppSettings["BloquearSeriesIniciaisTransfAluno"] ?? "false");
                if (bloquearSeriesIniciaisTransfAluno)
                {
                    contextQuery.Command += "  AND T.SERIE NOT IN (1, 6) ";
                }

                contextQuery.Command += " ORDER BY S.SERIE ";

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarSeries(string curso)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT SERIE
                    FROM LY_SERIE LS
                    INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO AND LC.CURSO = LS.CURSO
                    WHERE
                    LS.CURSO = @CURSO 
                    AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE()))
                    GROUP BY SERIE");

                contextQuery.Parameters.Add("@CURSO", curso);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarSeries(string nivel, string modalidade)
        {
            var contextQuery = new ContextQuery(
                @"SELECT DISTINCT C.CURSO,C.NOME 
                FROM LY_CURSO C 
                INNER JOIN LY_TIPO_CURSO TC ON C.TIPO=TC.TIPO 
                INNER JOIN LY_MODALIDADE_CURSO MC ON C.MODALIDADE = MC.MODALIDADE 
                WHERE TC.TIPO = @TIPO
                AND MC.MODALIDADE = @MODALIDADE
                ORDER BY C.NOME");

            contextQuery.Parameters.Add("@TIPO", nivel);
            contextQuery.Parameters.Add("@MODALIDADE", modalidade);

            return Consultar(contextQuery);
        }

        public static DataTable ListarSeriesPorUE(string censo, string curso)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT SERIE
                    FROM  LY_UNIDADE_ENSINO_CURSOS uc
                            JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                    JOIN LY_SERIE LS ON ls.CURSO=c.CURSO
                    INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO AND LC.CURSO = LS.CURSO
                    WHERE uc.UNIDADE_ENS = @CENSO
                    AND LS.CURSO = @CURSO
                    AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE())) 
                    ORDER BY SERIE");

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CENSO", censo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarSeriesRestricaoMatricula(string ano, string curso)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT SERIE FROM [LYCEUM].[dbo].[LY_SERIE] S
                    INNER JOIN LY_CURRICULO C ON S.CURRICULO = C.CURRICULO AND S.CURSO = C.CURSO AND S.TURNO =C.TURNO 
                    WHERE ANO_INI = @ano
                    AND S.CURSO = @curso");

                contextQuery.Parameters.Add("@ano", ano);
                contextQuery.Parameters.Add("@curso", curso);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void Remover(LySerie serie)
        {
            if (string.IsNullOrEmpty(serie.Curso)
                || string.IsNullOrEmpty(serie.Turno)
                || string.IsNullOrEmpty(serie.Curriculo)
                || serie.Serie <= 0)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery(
                          @"DELETE  LY_SERIE
                        WHERE   CURSO = @CURSO
                                AND TURNO = @TURNO
                                AND CURRICULO = @CURRICULO
                                AND SERIE = @SERIE ");

                    contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, serie.Curso);
                    contextQuery.Parameters.Add("@TURNO", TechneDbType.T_CODIGO, serie.Turno);
                    contextQuery.Parameters.Add("@CURRICULO", TechneDbType.T_CODIGO, serie.Curriculo);
                    contextQuery.Parameters.Add("@SERIE", TechneDbType.T_NUMERO_PEQUENO, serie.Serie);

                    ctx.ApplyModifications(contextQuery);

                }
                catch (Exception)
                {
                    ctx.Abandon();

                    throw;
                }
            }
        }

        public static ValidacaoDados ValidarAlterar(LySerie serie)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados();

            if (serie == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(serie.Curso))
            {
                mensagens.Add("O campo CURSO é obrigatório!");
            }

            if (string.IsNullOrEmpty(serie.Turno))
            {
                mensagens.Add("O campo TURNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(serie.Curriculo))
            {
                mensagens.Add("O campo CURRICULO é obrigatório!");
            }

            if (serie.Serie <= 0)
            {
                mensagens.Add("O campo ANO/SERIE é obrigatório e deve ser maior que zero(0)!");
            }
            if (Convert.ToInt32(serie.Complemento2) <= 0)
            {
                mensagens.Add("O campo TEMPOS DE AULA é obrigatório e deve ser maior que zero(0)!");
            }
            
            if (string.IsNullOrEmpty(serie.AnoSerieConcluinte))
            {
                mensagens.Add("O campo ANO SÉRIE CONCLUINTE é obrigatório!");
            }

            if (string.IsNullOrEmpty(serie.EmiteCertificacao))
            {
                mensagens.Add("O campo EMITE CERTIFIÇÃO é obrigatório!");
            }

            if (serie.AnoSerieConcluinte == "S"
                && (serie.SerieSeguinte.HasValue
                    || !string.IsNullOrEmpty(serie.CursoSeguinte)))
            {
                mensagens.Add("O campo ANO / SÉRIE CONCLUINTE? não pode ser selecionado junto com o CURSO SEGUINTE e SÉRIE SEGUINTE!");
            }

            if (serie.AnoSerieConcluinte == "N"
                && (!serie.SerieSeguinte.HasValue
                    || string.IsNullOrEmpty(serie.CursoSeguinte)))
            {
                mensagens.Add("O campo SÉRIE SEGUINTE e CURSO SEGUINTE são obrigatórios quando o ANO / SÉRIE CONCLUINTE? não está selecionado!");
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

        public static ValidacaoDados ValidarInserir(LySerie serie)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados();

            if (serie == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(serie.Curso))
            {
                mensagens.Add("O campo CURSO é obrigatório!");
            }

            if (string.IsNullOrEmpty(serie.Turno))
            {
                mensagens.Add("O campo TURNO é obrigatório!");
            }

            if (string.IsNullOrEmpty(serie.Curriculo))
            {
                mensagens.Add("O campo CURRICULO é obrigatório!");
            }

            if (serie.Serie <= 0)
            {
                mensagens.Add("O campo ANO/SERIE é obrigatório e deve ser maior que zero(0)!");
            }

            if (Convert.ToInt32(serie.Complemento2) <= 0)
            {
                mensagens.Add("O campo TEMPOS DE AULA é obrigatório e deve ser maior que zero(0)!");
            }

            if (string.IsNullOrEmpty(serie.AnoSerieConcluinte))
            {
                mensagens.Add("O campo ANO / SÉRIE CONCLUINTE? é obrigatório!");
            }

            if (string.IsNullOrEmpty(serie.EmiteCertificacao))
            {
                mensagens.Add("O campo EMITE CERTIFIÇÃO é obrigatório!");
            }

            if (serie.AnoSerieConcluinte == "S"
                && serie.SerieSeguinte.HasValue
                && !string.IsNullOrEmpty(serie.CursoSeguinte))
            {
                mensagens.Add("O campo ANO / SÉRIE CONCLUINTE? não pode ser selecionado junto com o CURSO SEGUINTE e SÉRIE SEGUINTE!");
            }

            if (serie.AnoSerieConcluinte == "N"
                && !serie.SerieSeguinte.HasValue
                && string.IsNullOrEmpty(serie.CursoSeguinte))
            {
                mensagens.Add("O campo SÉRIE SEGUINTE e CURSO SEGUINTE são obrigatórios quando o ANO / SÉRIE CONCLUINTE? não está selecionado!");
            }

            if (mensagens.Count == 0)
            {
                var contextQuery = new ContextQuery(
                    @" SELECT  COUNT(*)
                    FROM    ly_serie
                    WHERE   CURSO = @CURSO
                            AND TURNO = @TURNO
                            AND CURRICULO = @CURRICULO
                            AND SERIE = @SERIE ");

                contextQuery.Parameters.Add("@CURSO", serie.Curso);
                contextQuery.Parameters.Add("@TURNO", serie.Turno);
                contextQuery.Parameters.Add("@CURRICULO", serie.Curriculo);
                contextQuery.Parameters.Add("@SERIE", serie.Serie);

                var quantidade = ExecutarFuncao<int>(contextQuery);

                if (quantidade > 0)
                {
                    mensagens.Add("Já existe uma SÉRIE cadastrada com estes mesmos dados.");
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

        public static ValidacaoDados ValidarRemover(LySerie serie)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados();

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var quantidade = ctx.GetReturnValue<int>(
                    new ContextQuery(
                        @"SELECT  COUNT(*)
                            FROM    LY_GRADE
                            WHERE   CURSO = @CURSO
                                    AND TURNO = @TURNO
                                    AND CURRICULO = @CURRICULO
                                    AND SERIE_IDEAL = @SERIE ",
                        new ContextQueryParameter("@CURSO", serie.Curso),
                        new ContextQueryParameter("@CURRICULO", serie.Curriculo),
                        new ContextQueryParameter("@SERIE", serie.Serie),
                        new ContextQueryParameter("@TURNO", serie.Turno)));

                if (quantidade > 0)
                {
                    mensagens.Add("Está série não pode ser removida pois já existe uma grade vinculada.");
                }
            }


            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var quantidade = ctx.GetReturnValue<int>(
                    new ContextQuery(
                        @"SELECT  COUNT(*)
                            FROM    dbo.LY_HOR_OPER
                            WHERE   CURSO = @CURSO
                                    AND TURNO = @TURNO
                                    AND CURRICULO = @CURRICULO
                                    AND SERIE = @SERIE ",
                        new ContextQueryParameter("@CURSO", serie.Curso),
                        new ContextQueryParameter("@CURRICULO", serie.Curriculo),
                        new ContextQueryParameter("@SERIE", serie.Serie),
                        new ContextQueryParameter("@TURNO", serie.Turno)));

                if (quantidade > 0)
                {
                    mensagens.Add("Está série não pode ser removida pois existe um horário operacional vinculado.");
                }
            }

            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var quantidade = ctx.GetReturnValue<int>(
                    new ContextQuery(
                        @"SELECT  COUNT(*)
                            FROM    dbo.LY_ALUNO
                            WHERE   CURSO = @CURSO
                                    AND TURNO = @TURNO
                                    AND CURRICULO = @CURRICULO
                                    AND SERIE = @SERIE ",
                        new ContextQueryParameter("@CURSO", serie.Curso),
                        new ContextQueryParameter("@CURRICULO", serie.Curriculo),
                        new ContextQueryParameter("@SERIE", serie.Serie),
                        new ContextQueryParameter("@TURNO", serie.Turno)));

                if (quantidade > 0)
                {
                    mensagens.Add("Está série não pode ser removida pois existe " + quantidade + " aluno(s) vinculado.");
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

        public static bool VerificarPodeExcluirSufixo(string curso, string turno, string curriculo, decimal? serie, string sufixo)
        {
            var connection = Config.CreateConnection();
            connection.Open();
            try
            {
                return Ly_turma.QueryFirstRow(connection, "curso = ? AND turno = ? AND curriculo = ? AND serie = ? AND turma_integracao = ?", curso, turno, curriculo, serie, sufixo) == null;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool VerificarSerieAlunoAtivo(TConnectionWritable tconnw, string curso, string turno, string curriculo, decimal serie, DateTime dt_extincao)
        {
            var retorno = false;
            var qt = new QueryTable(
                " SELECT 1 " +
                " FROM LY_H_CURSOS_CONCL hcc " +
                " INNER JOIN LY_ALUNO a ON hcc.ALUNO = a.ALUNO " +
                " WHERE a.SIT_ALUNO = 'Ativo' " +
                " AND CONVERT(DATE,hcc.DT_ENCERRAMENTO) >= CONVERT(DATE, ?) " +
                " AND a.CURSO = ? " +
                " AND a.TURNO = ? " +
                " AND a.CURRICULO = ? " +
                " AND a.SERIE = ? ");

            qt.Query(tconnw, dt_extincao, curso, turno, curriculo, serie);

            if (qt.Rows.Count > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        /// <summary>
        /// Verifica se a serie de acordo com os parametros usados contém valor. Retornando valor ela está desativada
        /// </summary>
        /// <param name="connection">conexão com a base</param>
        /// <param name="curso">Curso da serie</param>
        /// <param name="turno">Turno da serie</param>
        /// <param name="curriculo">Curriculo da serie</param>
        /// <param name="serie">Codigo da serie</param>
        /// <returns>true caso nao esteja extinta, false caso esteja </returns>
        public static bool VerificarSerieExtinta(TConnection connection, string curso, string turno, string curriculo, decimal serie)
        {
            var sql = " select 1 from ly_serie where curso = ? and turno = ? and curriculo = ? and serie = ? and Convert(Date, dt_extincao) <= Convert(Date,getdate()) ";
            var valor = ExecutarFuncaoScalar(sql, curso, turno, curriculo, serie);

            if (!valor.IsNull)
            {
                return false;
            }

            return true;
        }

        public static bool VerificarTurmaDataTermino(TConnectionWritable tconnw, string curso, string turno, string curriculo, decimal serie, DateTime dt_extincao)
        {
            var retorno = false;
            var qt = new QueryTable(
                " SELECT 1 " +
                " FROM LY_TURMA t " +
                " WHERE t.SIT_TURMA <> 'Desativada' " +
                " AND CONVERT(DATE,t.DT_FIM) >= CONVERT(DATE,?) " +
                " AND t.CURSO = ? " +
                " AND t.TURNO = ? " +
                " AND t.CURRICULO = ? " +
                " AND t.SERIE = ? ");

            qt.Query(tconnw, dt_extincao, curso, turno, curriculo, serie);

            if (qt.Rows.Count > 0)
            {
                retorno = true;
            }

            return retorno;
        }

        public void DeleteCurriculosSerie(string curso, string turno, string curriculo, decimal serie)
        {
        }

        public void InsertCurriculosSerie(decimal serie, string descricao, string complemento1, string complemento2, decimal idade_minima, decimal dia_aniv, decimal mes_aniv, DateTime dt_extincao, string curso_seguinte, decimal serie_seguinte, string ano_serie_concluinte, string emite_certificacao,string ofertaeletiva, string curso, string turno, string curriculo)
        {
        }

        public void UpdateCurriculosSerie(decimal serie, string descricao, string complemento1, string complemento2, decimal idade_minima, decimal dia_aniv, decimal mes_aniv, DateTime dt_extincao, string curso_seguinte, decimal serie_seguinte, string ano_serie_concluinte, string emite_certificacao, string ofertaeletiva, string curso, string turno, string curriculo)
        {
        }

        [Serializable]
        public class DadosBuscaSerie
        {
            public bool Buscou { get; set; }

            public string Curriculo { get; set; }

            public string Curso { get; set; }

            public string Serie { get; set; }

            public string Turno { get; set; }
        }

        public static QueryTable ListaSerieHistorico(string aluno)
        {
            return RN.RNBase.Consultar(@" SELECT  DISTINCT
                                                    SERIE
                                            FROM    dbo.LY_HISTMATRICULA
                                            WHERE   ALUNO = ?
                                                    AND SITUACAO_HIST = 'Rep Nota'", aluno);

        }

        public static QueryTable ConsultarSerieVagasComTurno(string ano, string periodo, string turno, string curso, string censo)
        {
            var connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt;

            var sql = @" SELECT DISTINCT LS.SERIE, (CONVERT(VARCHAR, LS.SERIE) + '-' + LS.COMPLEMENTO1) AS SERIEPREFIXO, LS.DESCRICAO 
                         FROM    LY_UNIDADE_ENSINO_CURSOS uc
                        JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                        JOIN LY_SERIE LS ON ls.CURSO = c.CURSO
                        INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO
                                                      AND LC.CURSO = LS.CURSO
                                                      AND LC.TURNO = LS.TURNO
                                                      AND LC.ANO_INI = ?
                                                      AND LC.SEM_INI = ?  
                        INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON A.CURSO=LS.CURSO 
												                        AND A.SERIE=LS.SERIE 
												                        AND A.ANO=LC.ANO_INI
												                        AND A.PERIODO=LC.SEM_INI                      
                        INNER JOIN dbo.TCE_CTV_CONF_TURNO ct ON ct.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                                AND uc.UNIDADE_ENS = ct.CENSO
                        WHERE 
                         LS.TURNO = ? 
                         AND LS.CURSO = ? 
                         AND  uc.UNIDADE_ENS = ?
                         AND (LC.DT_EXTINCAO IS NULL OR CONVERT(DATE,LC.DT_EXTINCAO) > CONVERT(DATE, GETDATE()))
                         AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE())) 
                        		AND NOT EXISTS (SELECT 1
                                             FROM   DBO.TCE_CTV_RESTRICAO RE
                                             WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA=A.ID_AGENDA_CONF_TURNO_VAGA
                                                 AND RE.CENSO = UC.UNIDADE_ENS)
                         ORDER BY SERIE";

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection, ano, periodo, turno, curso, censo);
            }
            finally
            {
                connection.Close();
            }

            return qt;
        }

        public static DataTable ListarSeriesPorUE(string censo, string curso, string turno, string ano, string periodo, string curriculo)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT
                                SERIE
                        FROM    LY_UNIDADE_ENSINO_CURSOS uc
                                JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                                JOIN LY_SERIE LS ON ls.CURSO = c.CURSO
                                INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO
                                                              AND LC.CURSO = LS.CURSO
                        WHERE   uc.UNIDADE_ENS = @CENSO
                                AND LS.CURSO = @CURSO
                                AND LS.TURNO = @TURNO
                                AND LC.ANO_INI = @ANO_INI
                                AND LC.SEM_INI = @SEM_INI
                                AND LC.CURRICULO = @CURRICULO
                                AND ( LS.DT_EXTINCAO IS NULL
                                      OR CONVERT(DATE, LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE())
                                    )
                        ORDER BY SERIE");

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@ANO_INI", ano);
                contextQuery.Parameters.Add("@SEM_INI", periodo);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static LySerie Carregar(string curso, string curriculo, int serie, string turno)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @" SELECT * FROM LY_SERIE
                                WHERE CURSO = @CURSO
                                AND CURRICULO = @CURRICULO
                                AND SERIE = @SERIE
                                AND TURNO = @TURNO
                            ");

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@SERIE", serie);
                contextQuery.Parameters.Add("@TURNO", turno);

                return ctx.TryToBindEntity<LySerie>(contextQuery);
            }
        }
        public static QueryTable ListarSeriesIngresso(string curso)
        {
            return Consultar("SELECT DISTINCT SERIE FROM LY_SERIE LS INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO AND LC.CURSO = LS.CURSO WHERE LS.CURSO= ? AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE()))  GROUP BY SERIE", curso);

        }

        /// <summary>
        /// Retorna séries para renovação de matrícula
        /// </summary>
        /// <param name="curso"></param>
        /// <param name="turno"></param>
        /// <returns></returns>
        public static QueryTable RetornaSeriePor(string curso, string turno)
        {
            var connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt;

            var sql = string.Format(@"
            SELECT DISTINCT ly_serie.serie AS SERIE
            --, ly_serie.serie AS SERIE 
            FROM   ly_serie 
            WHERE  curso = '{0}' 
                   AND turno = '{1}' 
                   AND ( dt_extincao IS NULL 
                          OR CONVERT(DATE, dt_extincao) > CONVERT(DATE, Getdate()) ) "
                , curso
                , turno);

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

        public DataTable ObtemSeriesNovaTurmaTurnosVagasPor(string ano, string periodo, string turno, string curso, string censo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;

            try
            {
                contextQuery.Command =
                     @"SELECT DISTINCT LS.SERIE, (CONVERT(VARCHAR, LS.SERIE) + '-' + LS.COMPLEMENTO1) AS SERIEPREFIXO--, LS.DESCRICAO 
                         FROM    LY_UNIDADE_ENSINO_CURSOS uc
                        JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                        JOIN LY_SERIE LS ON ls.CURSO = c.CURSO
                        INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO
                                                      AND LC.CURSO = LS.CURSO
                                                      AND LC.TURNO = LS.TURNO
                                                      AND LC.ANO_INI = @ANO_INI
                                                      AND LC.SEM_INI = @SEM_INI  
                        INNER JOIN dbo.TCE_CTV_AGENDA_CONF_TURNO_VAGA A ON A.CURSO=LS.CURSO 
												                        AND A.SERIE=LS.SERIE 
												                        AND A.ANO=LC.ANO_INI
												                        AND A.PERIODO=LC.SEM_INI                      
                        INNER JOIN dbo.TCE_CTV_CONF_TURNO ct ON ct.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA
                                AND uc.UNIDADE_ENS = ct.CENSO
                        WHERE 
                         LS.TURNO = @TURNO 
                         AND LS.CURSO = @CURSO 
                         AND  uc.UNIDADE_ENS = @CENSO
                         AND (LC.DT_EXTINCAO IS NULL OR CONVERT(DATE,LC.DT_EXTINCAO) > CONVERT(DATE, GETDATE()))
                         AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE())) 
                        		AND NOT EXISTS (SELECT 1
                                             FROM   DBO.TCE_CTV_RESTRICAO RE
                                             WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA=A.ID_AGENDA_CONF_TURNO_VAGA
                                                 AND RE.CENSO = UC.UNIDADE_ENS)
                         ORDER BY SERIE";

                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO_INI", ano);
                contextQuery.Parameters.Add("@SEM_INI", periodo);

                series = ctx.GetDataTable(contextQuery);
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

            return series;
        }

        public DataTable ListaSeriePor(string curso, string turno, string curriculo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable serie = null;

            try
            {
                contextQuery.Command = string.Format(@"
                    SELECT DISTINCT SERIE, DESCRICAO 
                    FROM LY_SERIE 
                    WHERE 
                    CURSO = '{0}'
                    AND TURNO = '{1}'
                    AND CURRICULO = '{2}' 
                    ORDER BY SERIE "
                    , curso
                    , turno
                    , curriculo);

                serie = ctx.GetDataTable(contextQuery);
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

            return serie;
        }

        public DataTable ListaSeriePor(string curso, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                SERIE AS SERIE
                        FROM    LY_SERIE
                        WHERE   CURSO = @CURSO
                                AND TURNO = @TURNO
                                AND ( DT_EXTINCAO IS NULL
                                      OR CONVERT(DATE, DT_EXTINCAO) > CONVERT(DATE, GETDATE())
                                    )
                        ORDER BY SERIE  ";

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);

                series = ctx.GetDataTable(contextQuery);
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

            return series;
        }

        public DataTable listaSerieAgendaTurnoVagaPor(int ano, int perfilId, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;

            try
            {
                contextQuery.Command = @"SELECT  DISTINCT
                                                TV.SERIE
                                        FROM    DBO.TCE_CTV_CONF_TURNO_INICIAL TI
                                                INNER JOIN DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA TV ON TI.ID_AGENDA_CONF_TURNO_VAGA = TV.ID_AGENDA_CONF_TURNO_VAGA
                                                INNER JOIN DBO.LY_CURSO C ON TV.CURSO = C.CURSO
                                                INNER JOIN DBO.LY_TIPO_CURSO TC ON C.TIPO = TC.TIPO
                                                INNER JOIN LY_MODALIDADE_CURSO MC ON MC.MODALIDADE = C.MODALIDADE
                                                LEFT JOIN DBO.PERFILMODALIDADE PM ON C.MODALIDADE = PM.MODALIDADEID
                                                LEFT JOIN HADES.DBO.TCE_PERFIL PE ON PM.PERFILID = PE.ID_PERFIL
                                        WHERE   ANO = @ANO
                                                AND (PERFILID = @PERFILID OR @PERFILID = 7) -- 7 = DIESP QUE NÃO TEM TRATAMENTO DE PERFIL POR MODALIDADE
                                                AND TV.CURSO = @CURSO
                                        ORDER BY  TV.SERIE";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERFILID", perfilId);
                contextQuery.Parameters.Add("@CURSO", curso);

                series = ctx.GetDataTable(contextQuery);
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

            return series;
        }

        public DataTable ObtemSeriesPor(string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;

            try
            {
                contextQuery.Command =
                     @"SELECT DISTINCT SERIE
                    FROM  LY_UNIDADE_ENSINO_CURSOS uc
                            JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                    JOIN LY_SERIE LS ON ls.CURSO=c.CURSO
                    INNER JOIN LY_CURRICULO LC ON LC.CURRICULO = LS.CURRICULO AND LC.CURSO = LS.CURSO
                    INNER JOIN dbo.LY_UNIDADE_ENSINO ue ON uc.UNIDADE_ENS = ue.UNIDADE_ENS
                    WHERE LS.CURSO = @CURSO
                    AND (LS.DT_EXTINCAO IS NULL OR CONVERT(DATE,LS.DT_EXTINCAO) > CONVERT(DATE, GETDATE())) 
                    ORDER BY SERIE";

                contextQuery.Parameters.Add("@CURSO", curso);


                series = ctx.GetDataTable(contextQuery);
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

            return series;
        }

        public List<string[]> ObtemCursosSeriesAnterioresPor(string curso, int serie)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            List<string[]> listaCursosSerie = new List<string[]>();
            string[] cursoSerie;
            string cursoAnterior = string.Empty;
            string serieAnterior = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT
                                CURSO ,
                                SERIE
                        FROM    DBO.LY_SERIE
                        WHERE   CURSO_SEGUINTE = @CURSO
                                AND SERIE_SEGUINTE = @SERIE ";

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@SERIE", serie);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    cursoAnterior = Convert.ToString(reader["CURSO"]);
                    serieAnterior = Convert.ToString(reader["SERIE"]);

                    cursoSerie = new string[] { cursoAnterior, serieAnterior };
                    listaCursosSerie.Add(cursoSerie);
                }

                return listaCursosSerie;
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


        public DataTable ListaSerieHistoricoPor(string aluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT
                                                SERIE
                                        FROM    dbo.VW_DISCIPLINAREFERENCIA
                                        WHERE   ALUNO = @ALUNO
                                        ORDER BY SERIE ";

                contextQuery.Parameters.Add("@ALUNO", aluno);

                series = ctx.GetDataTable(contextQuery);
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

            return series;
        }

        public DataTable ObtemSeries(string censo, decimal ano, decimal periodo, string turno, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;

            try
            {
                contextQuery.Command =
                     @" SELECT DISTINCT S.SERIE
                        FROM   LY_TURMA T
                               INNER JOIN LY_SERIE S
                                       ON T.SERIE = S.SERIE
                        WHERE  T.DT_FIM > Getdate()
                               AND SIT_TURMA = 'ABERTA'
                               AND T.CURSO = @CURSO
                               AND S.TURNO = @TURNO
                               AND ANO = @ANO
                               AND SEMESTRE = @SEMESTRE
                               AND FACULDADE = @FACULDADE  ";

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@FACULDADE", censo);

                series = ctx.GetDataTable(contextQuery);
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

            return series;
        }

        public DataTable ObtemSeriesAlunoNovoPor(string curso, string turno, string curriculo, bool bloquearSeriesIniciaisAluno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;

            try
            {
                contextQuery.Command =
                     @"SELECT DISTINCT SERIE,
                                        DESCRICAO
                        FROM   LY_SERIE
                        WHERE  CURSO = @CURSO
                               AND TURNO = @TURNO
                               AND CURRICULO = @CURRICULO
                               AND ( DT_EXTINCAO IS NULL
                                      OR CONVERT(DATE, DT_EXTINCAO) > CONVERT(DATE, Getdate()) )  ";

                if (bloquearSeriesIniciaisAluno)
                {
                    contextQuery.Command += "  AND SERIE NOT IN ( 1, 6 ) ";
                }

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@TURNO", turno);

                series = ctx.GetDataTable(contextQuery);
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

            return series;
        }

        public bool PermiteEletivaPor(Seeduc.Infra.Data.DataContext contexto, string curriculo, string curso, string turno, int serie)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT count(*)
                                        FROM   LY_SERIE
                                        WHERE CURRICULO = @CURRICULO
														AND TURNO = @TURNO
														AND CURSO = @CURSO 
														AND SERIE = @SERIE      
														AND OFERTAELETIVA = 'S' ";

            contextQuery.Parameters.Add("@TURNO", turno);
            contextQuery.Parameters.Add("@CURRICULO", curriculo);
            contextQuery.Parameters.Add("@CURSO", curso);
            contextQuery.Parameters.Add("@SERIE", serie);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ObtemSerieParticipa3FasePor(int ano, int periodo, string censo, string curso, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            DataTable dtSerie = new DataTable();

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT DISTINCT  S.SERIE
                        FROM  TCE_CONTROLE_VAGA CV
                        INNER JOIN LY_SERIE S ON S.SERIE = CV.SERIE AND S.CURSO = CV.CURSO AND S.TURNO = CV.TURNO
                    WHERE CV.ANO = @ANO
                        AND CV.PERIODO = @PERIODO
                        AND CV.CENSO = @CENSO
                        AND CV.CURSO = @CURSO
                        AND CV.TURNO = @TURNO
                    ORDER BY S.SERIE"
                };

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);


                dtSerie = ctx.GetDataTable(contextQuery);

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

            return dtSerie;
        }


        public DataTable ListaSerieAtivaPor(string censo, int ano, int periodo, string curso, string turno)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                            T.SERIE 
                                    FROM    LY_TURMA T
                                    WHERE   T.FACULDADE = @CENSO
                                            AND T.ANO = @ANO  
                                            AND T.SEMESTRE = @SEMESTRE    
                                            AND T.CURSO = @CURSO    
                                            AND T.TURNO = @TURNO                                            
                                            AND SIT_TURMA = 'Aberta'                                     
                                    ORDER BY T.SERIE  ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@SEMESTRE", periodo);
                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@TURNO", turno);

                series = ctx.GetDataTable(contextQuery);
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

            return series;           
        }


        public DataTable ListaSeriePor(int ano, int periodo, string curso)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;
            string possiveisPeriodosAnteriores = string.Empty;
            int anoAnterior;

            if (periodo == 2)
            {
                anoAnterior = ano;
                possiveisPeriodosAnteriores = "1";
            }
            else
            {
                anoAnterior = ano - 1;
                possiveisPeriodosAnteriores = "0, 2";
            }

            try
            {
                contextQuery.Command = string.Format(@"  SELECT DISTINCT
                            S.SERIE, S.SERIE
                    FROM    LY_TURMA T
                            INNER JOIN LY_SERIE S ON T.SERIE = S.SERIE
                    WHERE   SIT_TURMA <> 'Desativada'
                            AND T.CURSO = @CURSO
                            AND T.ANO = @ANOANTERIOR
	                        AND T.SEMESTRE IN ({0})
                            AND ( DT_EXTINCAO IS NULL OR CONVERT(DATE, DT_EXTINCAO) > CONVERT(DATE, GETDATE())) ", possiveisPeriodosAnteriores);

                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@ANOANTERIOR", anoAnterior);

                series = ctx.GetDataTable(contextQuery);
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

            return series;
        }

        public DataTable ObtemSeriesNotificacaoPor(string curso, string turno, string curriculo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable series = null;

            try
            {
                contextQuery.Command =
                     @"SELECT DISTINCT SERIE,
                                        DESCRICAO
                        FROM   LY_SERIE
                        WHERE  CURSO = @CURSO
                               AND TURNO = @TURNO
                               AND CURRICULO = @CURRICULO
                               AND ( DT_EXTINCAO IS NULL
                                      OR CONVERT(DATE, DT_EXTINCAO) > CONVERT(DATE, Getdate()) )  ";


                contextQuery.Parameters.Add("@CURSO", curso);
                contextQuery.Parameters.Add("@CURRICULO", curriculo);
                contextQuery.Parameters.Add("@TURNO", turno);

                series = ctx.GetDataTable(contextQuery);
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

            return series;
        }
    }
}