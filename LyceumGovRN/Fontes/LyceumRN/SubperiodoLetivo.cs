namespace Techne.Lyceum.RN
{
    using System;
    using System.Data;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using System.Data.SqlClient;

    public class SubperiodoLetivo : RNBase
    {
        public static QueryTable ConsultarSubPeriodosLetivos()
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT
                                   null AS subperiodo,
                                   '<nenhum>' AS descricao
                               UNION ALL
                               SELECT DISTINCT
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

        public bool PossuiAnoPeriodoPor(DataContext ctx, int ano, int periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool ativo = false;

            contextQuery.Command = @" SELECT  COUNT(1)
                                        FROM    DBO.LY_SUBPERIODO_LETIVO
                                        WHERE   ANO = @ANO
                                                AND PERIODO = @PERIODO ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                ativo = true;
            }

            return ativo;
        }

        public static QueryTable ConsultarSubPeriodosLetivos(decimal? ano, decimal? semestre)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT 
                                   null AS subperiodo, 
                                   '<nenhum>' AS descricao
                               UNION ALL
                               SELECT DISTINCT 
                                   subperiodo, 
                                   descricao
                               FROM ly_subperiodo_letivo
                               WHERE ano = ? AND periodo = ?";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, ano.Value, semestre.Value);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static QueryTable ConsultarSubPeriodosLetivosTodos(decimal? ano, decimal? semestre)
        {
            if (!ano.HasValue || !semestre.HasValue)
                return null;

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT 
                                   null AS subperiodo, 
                                   '<todos>' AS descricao
                               UNION ALL
                               SELECT DISTINCT 
                                   subperiodo, 
                                   descricao
                               FROM ly_subperiodo_letivo
                               WHERE ano = ? AND periodo = ?";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, ano.Value, semestre.Value);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static DataTable ConsultarSubPeriodosLetivosResumoAnual(decimal? ano, decimal? semestre)
        {
            if (!ano.HasValue || !semestre.HasValue)
                return null;

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT 
                                   null AS subperiodo, 
                                   '[RESUMO ANUAL]' AS descricao
                               UNION ALL
                               SELECT DISTINCT 
                                   subperiodo, 
                                   descricao
                               FROM ly_subperiodo_letivo
                               WHERE ano = @ANO AND periodo = @PERIODO "

                };
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", semestre);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static QueryTable ConsultarSubPeriodosLetivos(decimal? ano)
        {
            if (!ano.HasValue)
                return null;

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT 
                                   null AS subperiodo, 
                                   '[RESUMO ANUAL]' AS descricao
                               UNION ALL
                               SELECT DISTINCT 
                                   subperiodo, 
                                   descricao
                               FROM ly_subperiodo_letivo
                               WHERE ano = ?";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, ano.Value);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static DataTable ConsultarSubPeriodosLetivos(decimal? ano, string semestre)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"SELECT DISTINCT
                            subperiodo,
                            descricao
                    FROM    ly_subperiodo_letivo
                    WHERE   ano = @ANO
                            AND periodo = @PERIODO");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", semestre);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static bool ConsultarSubPeriodosLetivosAtivo(decimal? ano, string semestre, string subperiodo)
        {
            var sql = @"SELECT 1
                            FROM    LY_SUBPERIODO_LETIVO (NOLOCK)
                            WHERE   ANO = ?
                                    AND PERIODO = ?
                                    AND SUBPERIODO = ?
                                    AND CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO AND DT_LANCAMENTO";

            var retorno = ExecutarFuncao(sql, ano.Value, semestre, subperiodo);

            return retorno == 1;
        }


        public static bool ConsultarSubPeriodosAtivoCurriculoMinimo(string ano, string semestre, string subperiodo)
        {
            var sql = @"SELECT 1
                            FROM    LY_SUBPERIODO_LETIVO (NOLOCK)
                            WHERE   ANO = ?
                                    AND PERIODO = ?
                                    AND SUBPERIODO = ?
                                    AND CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO AND DT_CURRICULO_MINIMO";

            var retorno = ExecutarFuncao(sql, ano, semestre, subperiodo);

            return retorno == 1;
        }

        public int ObtemSubperiodoPor(int ano, int periodo, DateTime data)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            int retorno = 0;

            try
            {
                retorno = this.ObtemSubperiodoPor(contexto, ano, periodo, data);
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

        public int ObtemSubperiodoPor(DataContext contexto, int ano, int periodo, DateTime data)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT SUBPERIODO
                            FROM    LY_SUBPERIODO_LETIVO (NOLOCK)
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND CONVERT(DATE, @DATA) BETWEEN DT_INICIO AND DT_FIM ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);
                contextQuery.Parameters.Add("@DATA", SqlDbType.DateTime, data.Date);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["SUBPERIODO"]);
                }

                return retorno;
            }

            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public static QueryTable ConsultarSubPeriodosLetivosVigentes(decimal? ano, string semestre)
        {

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                String sql = @"SELECT  
                                   subperiodo
                               FROM ly_subperiodo_letivo (NOLOCK)
                               WHERE ano = ? AND periodo = ?
                                AND convert(date,GETDATE()) >= dt_inicio AND convert(date,GETDATE()) <= DT_LANCAMENTO
                                ";
                QueryTable qt = new QueryTable(sql);
                qt.Query(connection, ano.Value, semestre);
                return qt;
            }
            finally
            {
                connection.Close();
            }
        }

        public static DataTable ListarAnos()
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "SELECT  distinct ano FROM LY_SUBPERIODO_LETIVO order by ano desc";
                // contextQuery.Parameters.Add("@ano", 2011);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }
        public static DataTable ListarPeriodo(string ano)
        {
            var contextQuery = new ContextQuery(
                @"SELECT DISTINCT PERIODO
                FROM    LY_SUBPERIODO_LETIVO (NOLOCK)
                WHERE   ANO = @ano ");

            contextQuery.Parameters.Add("@ano", ano);

            return Consultar(contextQuery);
        }

        public static DataTable ListarAnosPeriodo()
        {
            var contextQuery = new ContextQuery(
                @"SELECT  DISTINCT
                        CONVERT(VARCHAR, ano) + ' - ' + CONVERT(VARCHAR, periodo) ano
                FROM    LY_SUBPERIODO_LETIVO
                ORDER BY ano DESC");

            return Consultar(contextQuery);
        }

        public static DataTable ListarBimestres(string ano, string periodo)
        {
            var contextQuery = new ContextQuery(
                @"SELECT DISTINCT SUBPERIODO,descricao
                FROM    LY_SUBPERIODO_LETIVO (NOLOCK)
                WHERE   ANO = @ano
                        AND PERIODO = @periodo");
            //AND CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO AND DT_FIM

            contextQuery.Parameters.Add("@ano", ano);
            contextQuery.Parameters.Add("@periodo", periodo);

            return Consultar(contextQuery);
        }

        public static DataTable ListarBimestresAvaliacao(string ano, string periodo)
        {
            var contextQuery = new ContextQuery(
                @"SELECT DISTINCT
                        SUBPERIODO,
                        descricao
                FROM    LY_SUBPERIODO_LETIVO (NOLOCK)
                WHERE   ANO = @ano
                        AND PERIODO = @periodo
                UNION ALL
                SELECT  '5',
                        'FINAL'");

            contextQuery.Parameters.Add("@ano", ano);
            contextQuery.Parameters.Add("@periodo", periodo);

            return Consultar(contextQuery);
        }

        public static bool ConsultarSubPeriodosLetivoAnterior(decimal ano, string periodo, string subperiodo)
        {
            var sql = @"SELECT 1
                        FROM    LY_SUBPERIODO_LETIVO (NOLOCK)
                        WHERE   ANO = ?
                                AND PERIODO = ?
                                AND SUBPERIODO = ?
                                AND CONVERT(DATE, GETDATE()) > DT_LANCAMENTO";

            var retorno = ExecutarFuncao(sql, ano, periodo, subperiodo);

            return retorno == 1;
        }

        public bool PossuiOutroSubPeriodoIntercaladoPor(int ano, int periodo, int subPeriodo, DateTime dataInicio, DateTime dataFim)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                       
                         FROM   LY_SUBPERIODO_LETIVO (NOLOCK)
                        WHERE   ANO = @ANO
                               AND PERIODO = @PERIODO
                               AND SUBPERIODO <> @SUBPERIODO
                               AND @DATAINICIO <= CONVERT(DATE, dt_inicio) 
                               AND @DATAFIM >= CONVERT(DATE, ISNULL(dt_fim, GETDATE())) ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@SUBPERIODO", subPeriodo);
                contextQuery.Parameters.Add("@DATAINICIO", dataInicio);
                contextQuery.Parameters.Add("@DATAFIM", dataFim);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool PossuiCompetenciaHabilidadePor(int ano, int periodo, int subPeriodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                       
                         FROM   TCE_COMPETENCIA_HABILIDADE_DOCENTE (NOLOCK)
                        WHERE   ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND SUBPERIODO = @SUBPERIODO
                               ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@SUBPERIODO", subPeriodo);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool PossuiGrupoCompetenciaHabilidadePor(int ano, int periodo, int subPeriodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                       
                         FROM   TCE_COMPETENCIA_HABILIDADE_GRUPO (NOLOCK)
                        WHERE   ANO = @ANO
                                AND PERIODO = @PERIODO
                                AND SUBPERIODO = @SUBPERIODO
                               ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@SUBPERIODO", subPeriodo);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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

        public bool PossuiMaterialEstudoPor(int ano, int periodo, int subPeriodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                       
                         FROM   LancamentoNotas.TURMA_MATERIALESTUDO (NOLOCK)
                        WHERE   ANO = @ANO
                                AND SEMESTRE = @PERIODO
                                AND SUBPERIODO = @SUBPERIODO
                               ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@SUBPERIODO", subPeriodo);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
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




    }
}