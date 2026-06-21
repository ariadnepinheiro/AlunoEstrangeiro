namespace Techne.Lyceum.RN
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using Seeduc.Infra.Data;
    using Techne.Data;
    using Techne.Library;
    using Techne.Lyceum.CR;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using System.Linq;
    using Techne.Lyceum.RN.Util;

    [DataObjectAttribute]
    public class PeriodoLetivo : RNBase
    {
        public const string QueryListaProximosAnos = "select distinct ANO from ly_periodo_letivo where ano >= year(getdate()) ORDER BY ANO DESC ";
        public const string QueryListaPeriodos = "select DISTINCT PERIODO from ly_periodo_letivo ORDER BY PERIODO ";
        public const string QueryListaAnos = "select distinct ANO from ly_periodo_letivo ORDER BY ANO DESC ";
        public const string QueryListaAnosFrequencia = "select distinct ANO from ly_periodo_letivo WHERE ANO >=2024 ORDER BY ANO DESC ";
        public const string QueryListaAnosSuspensao = "select distinct ANO from ly_periodo_letivo WHERE ANO = year(getdate()) ORDER BY ANO DESC ";

        public static QueryTable ConsultarAno()
        {
            return Consultar(QueryListaAnos);
        }

        public static QueryTable ConsultarAnoFrequencia()
        {
            return Consultar(QueryListaAnosFrequencia);
        }

        public static QueryTable ConsultarAnoSuspensao()
        {
            return Consultar(QueryListaAnosSuspensao);
        }


        public static QueryTable ConsultarAnoInf(string ano)
        {
            return Consultar("select distinct ANO from ly_periodo_letivo where ano <= ? ORDER BY ANO DESC", ano);
        }

        public static QueryTable ConsultarProximosAnos()
        {
            return Consultar(QueryListaProximosAnos);
        }

        public DataTable ListaPeriodosletivosPor(int ano)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            DataTable periodos = null;

            try
            {
                contextQuery.Command = @" SELECT  ISNULL(CAST(PERIODO AS VARCHAR) + ' - ' + ID_REDUZIDA, PERIODO) ID_REDUZIDA,
                                        PERIODO
                                    FROM    LY_PERIODO_LETIVO
                                    WHERE   ANO = @ANO
                                    ORDER BY PERIODO ";

                contextQuery.Parameters.Add("@ANO", ano);

                periodos = ctx.GetDataTable(contextQuery);
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

            return periodos;
        }

        public static QueryTable ConsultarAnosFinalizados()
        {
            return Consultar("SELECT DISTINCT ANO FROM   LY_PERIODO_LETIVO WHERE  DT_FIM <= GETDATE() ORDER BY ANO DESC ");
        }

        public static QueryTable ConsultarAno(decimal limiteInferior)
        {
            return Consultar("select distinct ANO from ly_periodo_letivo WHERE ANO >= ? ORDER BY ANO DESC", limiteInferior);
        }

        public static QueryTable ConsultarComboAno()
        {
            return Consultar("select null ano, '<Selecione>' descricao union all select distinct ano, convert(varchar,ano) descricao from ly_periodo_letivo ORDER BY ANO");
        }

        public static QueryTable ConsultarAnoPeriodo()
        {
            return Consultar("select convert(varchar,ano) + ' - ' + convert(varchar,periodo) anoperiodo FROM ly_periodo_letivo order by ano desc");
        }

        public static decimal ConsultarAnoLetivoAtual()
        {
            return ExecutarFuncaoDec(@"select top 1 ano from ly_periodo_letivo
                where ano = year(getdate()) and convert(date, getdate()) <= dt_fim and convert(date, getdate()) >= dt_inicio
                order by DT_INICIO desc");
        }

        public static QueryTable ConsultarPeriodo(string ano)
        {
            DbObject valor = DBNull.Value;

            if (!String.IsNullOrEmpty(ano))
                valor = ano;

            return Consultar("select ISNULL(CAST(PERIODO AS VARCHAR) + ' - ' + ID_REDUZIDA, PERIODO) ID_REDUZIDA, PERIODO from ly_periodo_letivo WHERE ANO = ? ORDER BY PERIODO", valor);
        }


        public static QueryTable ConsultarPeriodosFinalizados(string ano)
        {
            DbObject valor = DBNull.Value;

            if (!String.IsNullOrEmpty(ano))
                valor = ano;

            return Consultar("SELECT  ISNULL(CAST(PERIODO AS VARCHAR) + ' - ' + ID_REDUZIDA, PERIODO) ID_REDUZIDA , PERIODO FROM ly_periodo_letivo WHERE DT_FIM <= GETDATE() AND ANO = ? ORDER BY PERIODO ", valor);
        }

        public static QueryTable RetornarPeriodoEscolhido(string ano, int periodo)
        {
            DbObject valor = DBNull.Value;

            if (!String.IsNullOrEmpty(ano))
                valor = ano;

            return Consultar("select ISNULL(CAST(PERIODO AS VARCHAR) + ' - ' + ID_REDUZIDA, PERIODO) ID_REDUZIDA, PERIODO from ly_periodo_letivo WHERE ANO = ? AND PERIODO = ? ORDER BY PERIODO", valor, periodo);
        }


        public static DataTable ListarPeriodo(string ano)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "select ISNULL(CAST(PERIODO AS VARCHAR) + ' - ' + ID_REDUZIDA, PERIODO) ID_REDUZIDA, PERIODO from ly_periodo_letivo WHERE ANO = @ANO ORDER BY PERIODO";
                contextQuery.Parameters.Add("@ANO", ano);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }

        public static QueryTable ConsultarSubPeriodoLetivo()
        {
            return Consultar(@"select convert(varchar,ano) + ' - ' + convert(varchar, periodo) as anoperiodo,
                             subperiodo, descricao, dt_inicio, dt_fim, dias_letivos,dt_lancamento,dt_curriculo_minimo,dt_limite_frequencia from LY_SUBPERIODO_LETIVO order by ano desc");
        }

        public static QueryTable ConsultarSubPeriodoLetivo(string ano, string periodo)
        {
            DbObject valorAno = DBNull.Value;
            if (!String.IsNullOrEmpty(ano))
                valorAno = ano;
            DbObject valorPer = DBNull.Value;
            if (!String.IsNullOrEmpty(periodo))
                valorPer = periodo;
            return Consultar(@"select convert(varchar,ano) + ' - ' + convert(varchar, periodo) as anoperiodo,
                subperiodo, descricao, dt_inicio, dt_fim, dias_letivos,dt_lancamento,dt_curriculo_minimo,dt_limite_frequencia from LY_SUBPERIODO_LETIVO 
                where ANO = ? and periodo = ?
                order by  SUBPERIODO", valorAno, valorPer);
        }

        //verificar se esta sendo usado
        public static QueryTable ConsultarPeriodo()
        {
            return Consultar(QueryListaPeriodos);
        }

        public static QueryTable ConsultarDatas(string ano, string periodo)
        {
            return Consultar("SELECT dt_inicio, dt_fim FROM ly_periodo_letivo WHERE ano = ? and periodo = ?", ano, periodo);
        }

        /// <summary>
        /// Consulta o próximo ano e periodo
        /// </summary>
        /// <param name="anoPeriodo">ano e período</param>
        /// <returns>próximo ano e período</returns>
        public Int32[] ObtemProximoAnoPeriodoPor(int ano, int periodo)
        {
            Int32[] proximoAnoPeriodo = new Int32[2];
            int proximoAno = 0;
            int proximoPeriodo = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT  ISNULL(per_ano, 0) per_ano ,
                                ISNULL(per_periodo, 0) per_periodo
                        FROM    LY_PERIODO_LETIVO
                        WHERE   ANO = @ANO
                                AND PERIODO = @PERIODO ")
                };

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (Convert.ToInt32(reader["per_ano"]) > 0)
                    {
                        proximoAno = Convert.ToInt32(reader["per_ano"]);
                        proximoPeriodo = Convert.ToInt32(reader["per_periodo"]);
                    }
                }

                proximoAnoPeriodo[0] = proximoAno;
                proximoAnoPeriodo[1] = proximoPeriodo;

                return proximoAnoPeriodo;
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

        public static RetValue ValidarDatas(DateTime datainicial, DateTime datafinal, string ano, string periodo)
        {
            try
            {
                //DateTime dataini = Convert.ToDateTime(datainicial);
                //DateTime datafim = Convert.ToDateTime(datafinal);

                //valida se data fim é maior que data início
                if (datainicial > datafinal)
                    return new RetValue(false, string.Empty, new ErrorList("Data de término inferior a data de início."));

                //valida se a data está dentro da data do período letivo
                QueryTable qt = RN.PeriodoLetivo.ConsultarDatas(ano, periodo);
                if ((datafinal > Convert.ToDateTime(qt.Rows[0]["dt_fim"])) || (datainicial < Convert.ToDateTime(qt.Rows[0]["dt_inicio"])))
                    //throw new Exception("Data definida está fora da data definida para o ano letivo.");
                    return new RetValue(false, string.Empty, new ErrorList("Data definida está fora da data definida para o ano letivo."));
            }
            catch
            {
                throw;
            }

            return new RetValue(true, "Registro válido.", null);
        }

        public bool ExisteAnoLetivoPor(DataContext contexto, decimal ano, decimal periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM   LY_PERIODO_LETIVO (NOLOCK) 
                                        WHERE  ANO = @ANO 
                                               AND PERIODO = @PERIODO ";

            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private bool EhProximoAnoLetivoPor(DataContext contexto, decimal ano, decimal periodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM   LY_PERIODO_LETIVO (NOLOCK) 
                                        WHERE  PER_ANO = @ANO 
                                               AND PER_PERIODO = @PERIODO ";

            contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
            contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
        public static SimpleRow ObterDataAula(decimal ano, decimal semestre)
        {
            TConnection connection = Config.CreateConnection();

            connection.Open();

            QueryTable qt = null;

            string sql = "select dt_inicio_aula, dt_fim_aula from ly_periodo_letivo where ano = " + ano + " AND periodo = " + semestre;

            try
            {
                qt = new QueryTable(sql);

                qt.Query(connection);

                if (qt.Rows.Count > 0)
                    return qt.Rows[0];

            }
            finally
            {
                connection.Close();
            }

            return null;
        }


        public void DeleteSubPeriodo(decimal ano, decimal periodo, decimal subperiodo) { }

        public void InsertSubPeriodo(string anoperiodo, decimal subperiodo, string descricao, DateTime dt_inicio, DateTime dt_fim, decimal dias_letivos, DateTime dt_lancamento, DateTime dt_curriculo_minimo, DateTime dt_limite_frequencia) { }

        public void UpdateSubPeriodo(decimal subperiodo, string descricao, DateTime dt_inicio, DateTime dt_fim, decimal dias_letivos, decimal ano, decimal periodo, DateTime dt_lancamento, DateTime dt_curriculo_minimo, DateTime dt_limite_frequencia) { }


        public static RetValue AlterarSubPeriodo(Ly_subperiodo_letivo dtSubPeriodo)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtSubPeriodo != null)
                {
                    if (dtSubPeriodo.Rows != null)
                    {
                        ColunasTable colunas = MontarParametros(dtSubPeriodo.Columns, dtSubPeriodo.Rows[0]);

                        Ly_subperiodo_letivo.Row.Update(connection, dtSubPeriodo.Rows[0].Ano, dtSubPeriodo.Rows[0].Periodo, dtSubPeriodo.Rows[0].Subperiodo, colunas.Colunas, colunas.ValorColuna);

                        retorno = VerificarErro(connection.GetErrors());

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        return new RetValue(true, "Registro incluido com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }

        public static RetValue IncluirSubPeriodo(Ly_subperiodo_letivo dtSubPeriodo)
        {
            RetValue retorno = null;

            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);

            try
            {
                if (dtSubPeriodo != null)
                {
                    if (dtSubPeriodo.Rows != null)
                    {
                        dtSubPeriodo.Put(connection);

                        retorno = VerificarErro(dtSubPeriodo);

                        if (retorno != null && !retorno.Ok)
                        {
                            connection.Rollback();
                            return retorno;
                        }

                        return new RetValue(true, "Registro incluido com sucesso.", null);
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        public static RetValue ExcluirSubPeriodo(Ly_subperiodo_letivo dtSubPeriodo)
        {
            RetValue retorno = null;
            TConnectionWritable connection = Config.CreateWritableConnection();
            connection.Open(true);
            try
            {
                if (dtSubPeriodo != null)
                {
                    if (dtSubPeriodo.Rows != null)
                    {
                        Ly_subperiodo_letivo.Row.Delete(connection, dtSubPeriodo.Rows[0].Ano, dtSubPeriodo.Rows[0].Periodo, dtSubPeriodo.Rows[0].Subperiodo);

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

        public static decimal ObterSubPeriodoAtual(string ano, string semestre)
        {
            return ExecutarFuncao(
                @"SELECT ISNULL((
                SELECT TOP 1
                        SUBPERIODO
                FROM    ly_subperiodo_letivo (NOLOCK)
                WHERE   ano = ?
                        AND periodo = ?
                        AND GETDATE() BETWEEN DT_INICIO AND DT_LANCAMENTO
                ORDER BY SUBPERIODO
                ), 0) subperiodo",
                ano,
                semestre);
        }

        public static decimal ObterSubPeriodoAtualCM(string ano, string semestre)
        {
            return ExecutarFuncao(
                @"SELECT ISNULL((
                SELECT TOP 1
                        SUBPERIODO
                FROM    ly_subperiodo_letivo (NOLOCK)
                WHERE   ano = ?
                        AND periodo = ?
                         AND CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO AND DT_CURRICULO_MINIMO
                ORDER BY SUBPERIODO
                ), 0) subperiodo",
                ano,
                semestre);
        }

        //        /// <summary>
        //        /// Obtém o próximo período letivo, através da verificação da
        //        /// data de início e término dos períodos letivos
        //        /// </summary>
        //        /// <param name="anoAtual"></param>
        //        /// <param name="semestreAtual"></param>
        //        /// <returns></returns>
        //        public static Ly_periodo_letivo.Row ConsultarProximoPeriodoLetivo(string anoAtual, string semestreAtual)
        //        {
        //            if (string.IsNullOrEmpty(anoAtual))
        //                return null;
        //            if (string.IsNullOrEmpty(semestreAtual))
        //                return null;

        //            TConnection conn = Config.CreateConnection();
        //            conn.Open();

        //            Ly_periodo_letivo.Row proxPeriodoLetivo = null;

        //            try
        //            {
        //                QueryTable qtProxPeriodo = new QueryTable(
        //                    @"  DECLARE @anoAtual       int,
        //		                        @semestreAtual  int, 
        //		                        @dtFimAtual		DATETIME
        //
        //                        SET @anoAtual = ?
        //                        SET @semestreAtual = ?
        //
        //                        SELECT	@dtFimAtual = dt_fim
        //                        FROM	ly_periodo_letivo (NOLOCK)
        //                        WHERE	ano = @anoAtual AND periodo = @semestreAtual
        //
        //                        SELECT		TOP 1 pl.ano, pl.periodo
        //                        FROM		ly_periodo_letivo pl  (NOLOCK)
        //                        WHERE		pl.dt_inicio > @dtFimAtual AND
        //									((@semestreAtual = 0 AND pl.periodo = 0) OR
        //									 (@semestreAtual IN (1,2) AND pl.periodo IN (1,2)))
        //                        ORDER BY	pl.dt_inicio ASC, pl.ano ASC, pl.periodo ASC");
        //                qtProxPeriodo.Query(conn, anoAtual, semestreAtual);

        //                if (qtProxPeriodo.Rows.Count > 0)
        //                {
        //                    decimal proxAno = Convert.ToDecimal(qtProxPeriodo.Rows[0]["ano"]);
        //                    decimal proxSemestre = Convert.ToDecimal(qtProxPeriodo.Rows[0]["periodo"]);
        //                    proxPeriodoLetivo = Ly_periodo_letivo.Row.Query(conn, proxAno, proxSemestre);
        //                }
        //            }
        //            catch { }
        //            finally
        //            {
        //                conn.Close();
        //            }

        //            return proxPeriodoLetivo;
        //        }

        public static DataTable ListarAnos()
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "select distinct ANO from ly_periodo_letivo ORDER BY ANO DESC";

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }

        public static bool PossuiProximoAno(decimal ano)
        {
            string sql = "select 1 from ly_periodo_letivo where ANO = ? AND PER_ANO IS NOT NULL ";

            int retorno = ExecutarFuncao(sql, ano);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public bool EhAnoPeriodoAtivoPor(int ano, int periodo, DateTime data)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool ativo = false;

            try
            {
                ativo = this.EhAnoPeriodoAtivoPor(ctx, ano, periodo, data);
                return ativo;
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

        public bool EhAnoPeriodoAtivoPor(DataContext ctx, int ano, int periodo, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool ativo = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    DBO.LY_PERIODO_LETIVO
                                        WHERE   ANO = @ANO
                                                AND PERIODO = @PERIODO
                                                AND CONVERT(DATE, DT_INICIO) <= CONVERT(DATE, @DATA)
                                                AND CONVERT(DATE, DT_FIM) >= CONVERT(DATE, @DATA) ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                ativo = true;
            }

            return ativo;
        }

        public bool EhAnoPeriodoFuturoPor(DataContext ctx, int ano, int periodo, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool ativo = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    DBO.LY_PERIODO_LETIVO
                                        WHERE   ANO = @ANO
                                                AND PERIODO = @PERIODO
                                                AND CONVERT(DATE, DT_INICIO) > CONVERT(DATE, @DATA) ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                ativo = true;
            }

            return ativo;
        }

        public int ObtemProximoAnoLetivoPor(int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            int proximoAno = 0;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @"  SELECT  ISNULL(per_ano, 0) per_ano 
                        FROM    LY_PERIODO_LETIVO
                        WHERE   ANO = @ANO
                                "
                };
                contextQuery.Parameters.Add("@ANO", ano);

                proximoAno = contexto.GetReturnValue(contextQuery) == null ? 0 : contexto.GetReturnValue<int>(contextQuery);

                return proximoAno;
            }

            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

        }

        public DataTable ListaPossiveisPeriodosPor(int periodo, int ano)
        {
            DataTable dtPeriodo = new DataTable();

            dtPeriodo.Columns.Add("periodo");

            var listaPeriodos = new List<int>();

            //Verificar possiveis periodos com relação:                      
            if (periodo == 1)
            {
                if (ano == 2020)
                {
                    dtPeriodo.Rows.Add(0);
                    dtPeriodo.Rows.Add(1);
                }
                else
                {
                    //1 pode ir para 2  
                    dtPeriodo.Rows.Add(2);
                }
            }
            else
            {
                //0 pode ir para 0 ou 1 
                //2 pode ir para 0 ou 1
                dtPeriodo.Rows.Add(0);
                dtPeriodo.Rows.Add(1);
            }

            return dtPeriodo;
        }

        public int ObtemProximoAnoPor(int ano, int periodo)
        {
            int proximoAno = 0;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  PER_ANO
                    FROM    LY_PERIODO_LETIVO
                    WHERE   ANO = @ANO
                            AND PERIODO = @PERIODO
                            AND PER_ANO IS NOT NULL  ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    proximoAno = Convert.ToInt32(reader["PER_ANO"]);
                }

                return proximoAno;
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

        public string[] ObtemDataInicioFimAulaPor(int ano, int periodo)
        {
            string[] datas = new string[2];
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT  DT_INICIO_AULA ,
                                    DT_FIM_AULA
                            FROM    LY_PERIODO_LETIVO
                            WHERE   ANO = @ANO
                                    AND PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    datas[0] = Convert.ToString(reader["DT_INICIO_AULA"]);
                    datas[1] = Convert.ToString(reader["DT_FIM_AULA"]);
                }


                return datas;
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
        public DataTable ListaAnos(bool finalizado)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT ANO
                                          FROM   LY_PERIODO_LETIVO WHERE";
                if (finalizado)
                {
                    contextQuery.Command += " ANO < Year(Getdate())";
                }
                else
                {
                    contextQuery.Command += " ANO <= Year(Getdate())";
                }

                contextQuery.Command += " ORDER  BY ANO DESC";


                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public DataTable ListaAnosAcima2015(bool finalizado)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT ANO
                                          FROM   LY_PERIODO_LETIVO WHERE ANO >= 2015 ";


                contextQuery.Command += " ORDER  BY ANO DESC";


                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }

        public DataTable ListaAnoPeriodoAberto()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable ano = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                            ANO, 
											PERIODO
                            FROM   LY_PERIODO_LETIVO (NOLOCK) 
                            WHERE CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO AND DT_FIM
		                    ORDER BY ANO DESC, PERIODO";


                ano = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return ano;
        }

        public int ObtemAnoAberto()
        {
            int ano = 0;
            DataTable anoPeriodo = this.ListaAnoPeriodoAberto();

            if (anoPeriodo.Rows.Count > 0)
            {
                ano = Convert.ToInt32(anoPeriodo.Rows[0]["ANO"]);
            }

            return ano;
        }

        public string ObtemPeriodoAbertoPor(int ano)
        {
            List<string> periodos = new List<string>();
            DataTable anoPeriodo = this.ListaAnoPeriodoAberto();

            foreach (DataRow item in anoPeriodo.Rows)
            {
                int anoitem = Convert.ToInt32(item["ANO"]);
                string periodo = Convert.ToString(item["PERIODO"]);

                if (anoitem == ano)
                {
                    periodos.Add(periodo);
                }
            }

            return periodos.Distinct().Aggregate((x, y) => x + "," + y);
        }

        public bool EhAnoLetivoPor(DataContext ctx, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    DBO.LY_PERIODO_LETIVO
                                        WHERE   ANO = @ANO ";

            contextQuery.Parameters.Add("@ANO", ano);

            return ctx.GetReturnValue<int>(contextQuery) > 0;
        }
        public DataTable ListaAnoFuturo()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable ano = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                            ANO
                            FROM   LY_PERIODO_LETIVO (NOLOCK) 
                             WHERE CONVERT(DATE,DT_INICIO) >= CONVERT(DATE,GETDATE())
		                    ORDER BY ANO DESC";


                ano = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return ano;
        }

        public DataTable ListaPeriodoFuturo(int ano)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable periodo = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT                                             
											PERIODO
                            FROM   LY_PERIODO_LETIVO (NOLOCK) 
                             WHERE CONVERT(DATE,DT_INICIO) >= CONVERT(DATE,GETDATE())
                              AND ANO = @ANO
		                    ORDER BY PERIODO DESC";

                contextQuery.Parameters.Add("@ANO", ano);

                periodo = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return periodo;
        }


        public bool EhAnoPeriodoAulaAtivoPor(DataContext ctx, int ano, int periodo, DateTime data)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool ativo = false;

            contextQuery.Command = @" SELECT  COUNT(*)
                                        FROM    DBO.LY_PERIODO_LETIVO
                                        WHERE   ANO = @ANO
                                                AND PERIODO = @PERIODO
                                                AND CONVERT(DATE, DT_INICIO) <= CONVERT(DATE, @DATA)
                                                AND CONVERT(DATE, DT_FIM_AULA) >= CONVERT(DATE, @DATA) ";

            contextQuery.Parameters.Add("@ANO", ano);
            contextQuery.Parameters.Add("@PERIODO", periodo);
            contextQuery.Parameters.Add("@DATA", data);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                ativo = true;
            }

            return ativo;
        }

        public DataTable Lista()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable ano = null;

            try
            {
                contextQuery.Command = @" SELECT  ano,
		                                        periodo,
		                                        dt_inicio,
		                                        dt_fim,
		                                        dt_inicio_aula,
		                                        dt_fim_aula,
		                                        data_inicio_docente,
		                                        data_fim_docente,
		                                        descricao,
		                                        per_ano,
		                                        per_periodo,
		                                        data_inicio_indicacao_eletiva,
		                                        data_fim_indicacao_eletiva,
		                                        data_inicio_distribuicao_eletiva,
		                                        data_fim_distribuicao_eletiva,
                                                qtde_subperiodo
                                        FROM DBO.LY_PERIODO_LETIVO (NOLOCK)
                                        ORDER BY ANO DESC, PERIODO DESC ";

                ano = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return ano;
        }

        public ValidacaoDados Valida(Entidades.LyPeriodoLetivo periodoLetivo, bool cadastro)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (periodoLetivo == null)
            {
                return validacaoDados;
            }

            if (periodoLetivo.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodoLetivo.Periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (periodoLetivo.QtdeSubperiodo <= 0)
            {
                mensagens.Add("Campo QUANTIDADE CICLOS AVALIATIVOS é obrigatório.");
            }

            if (periodoLetivo.DtInicio == null || periodoLetivo.DtInicio == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO é obrigatório.");
            }

            if (periodoLetivo.DtFim == null || periodoLetivo.DtFim == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM é obrigatório.");
            }

            if (periodoLetivo.DtInicioAula == null || periodoLetivo.DtInicioAula == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA INÍCIO AULA é obrigatório.");
            }

            if (periodoLetivo.DtFimAula == null || periodoLetivo.DtFimAula == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA FIM AULA é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                if (periodoLetivo.DtInicio > periodoLetivo.DtFim)
                {
                    mensagens.Add("A DATA DE INÍCIO não pode ser maior que a data de fim.");
                }

                if (periodoLetivo.DtInicioAula > periodoLetivo.DtFimAula)
                {
                    mensagens.Add("A data de INÍCIO DA AULA não pode ser maior que a data de fim da aula.");
                }

                if (periodoLetivo.DtInicioAula > periodoLetivo.DtFim)
                {
                    mensagens.Add("Data Início Aula não pode ser maior que Data Fim.");
                }
                if (periodoLetivo.DtInicioAula < periodoLetivo.DtInicio)
                {
                    mensagens.Add("Data Início Aula não pode ser menor que Data Início.");
                }

                if (periodoLetivo.DtFimAula > periodoLetivo.DtFim)
                {
                    mensagens.Add("Data Fim Aula não pode ser maior que Data Fim.");
                }
                if (periodoLetivo.DtFimAula < periodoLetivo.DtInicio)
                {
                    mensagens.Add("Data Fim Aula não pode ser menor que Data Início.");
                }

                if ((periodoLetivo.DataInicioDocente != null && periodoLetivo.DataInicioDocente != DateTime.MinValue
                    && (periodoLetivo.DataFimDocente == null || periodoLetivo.DataFimDocente == DateTime.MinValue)

                    || ((periodoLetivo.DataInicioDocente == null || periodoLetivo.DataInicioDocente == DateTime.MinValue)
                    && periodoLetivo.DataFimDocente != null && periodoLetivo.DataFimDocente != DateTime.MinValue)))
                {
                    mensagens.Add("Para informa DATA DOCENTE é necessário preencher início e fim.");
                }
                else
                {
                    if (periodoLetivo.DataInicioDocente != null && periodoLetivo.DataInicioDocente != DateTime.MinValue)
                    {
                        if (periodoLetivo.DataInicioDocente > periodoLetivo.DataFimDocente)
                        {
                            mensagens.Add("A data de INÍCIO DO DOCENTE não pode ser maior que a data de fim do docente.");
                        }

                        if (periodoLetivo.DataInicioDocente > periodoLetivo.DtFim)
                        {
                            mensagens.Add("Data Início Docente não pode ser maior que Data Fim.");
                        }
                        if (periodoLetivo.DataInicioDocente < periodoLetivo.DtInicio)
                        {
                            mensagens.Add("Data Início Docente não pode ser menor que Data Início.");
                        }

                        if (periodoLetivo.DataFimDocente > periodoLetivo.DtFim)
                        {
                            mensagens.Add("Data Fim Docente não pode ser maior que Data Fim.");
                        }
                        if (periodoLetivo.DataFimDocente < periodoLetivo.DtInicio)
                        {
                            mensagens.Add("Data Fim Docente não pode ser menor que Data Início.");
                        }
                    }
                }

                if ((periodoLetivo.DataInicioIndicacaoEletiva != null && periodoLetivo.DataInicioIndicacaoEletiva != DateTime.MinValue
                    && (periodoLetivo.DataFimIndicacaoEletiva == null || periodoLetivo.DataFimIndicacaoEletiva == DateTime.MinValue)
                    || ((periodoLetivo.DataInicioIndicacaoEletiva == null || periodoLetivo.DataInicioIndicacaoEletiva == DateTime.MinValue)
                    && periodoLetivo.DataFimIndicacaoEletiva != null && periodoLetivo.DataFimIndicacaoEletiva != DateTime.MinValue)))
                {
                    mensagens.Add("Para informa DATA INDICAÇÃO ELETIVA é necessário preencher início e fim.");
                }
                else
                {
                    if (periodoLetivo.DataInicioIndicacaoEletiva != null && periodoLetivo.DataInicioIndicacaoEletiva != DateTime.MinValue)
                    {
                        if (periodoLetivo.DataInicioIndicacaoEletiva > periodoLetivo.DataFimIndicacaoEletiva)
                        {
                            mensagens.Add("A data de INÍCIO DA INDICAÇÃO ELETIVA não pode ser maior que a data de fim da Indicação Eletiva.");
                        }
                    }
                }

                if ((periodoLetivo.DataInicioDistribuicaoEletiva != null && periodoLetivo.DataInicioDistribuicaoEletiva != DateTime.MinValue
                    && (periodoLetivo.DataFimDistribuicaoEletiva == null || periodoLetivo.DataFimDistribuicaoEletiva == DateTime.MinValue)
                    || ((periodoLetivo.DataInicioDistribuicaoEletiva == null || periodoLetivo.DataInicioDistribuicaoEletiva == DateTime.MinValue)
                    && periodoLetivo.DataFimDistribuicaoEletiva != null && periodoLetivo.DataFimDistribuicaoEletiva != DateTime.MinValue)))
                {
                    mensagens.Add("Para informa DATA DISTRIBUIÇÃO ELETIVA é necessário preencher início e fim.");
                }
                else
                {
                    if (periodoLetivo.DataInicioDistribuicaoEletiva != null && periodoLetivo.DataInicioDistribuicaoEletiva != DateTime.MinValue)
                    {
                        if (periodoLetivo.DataInicioDistribuicaoEletiva > periodoLetivo.DataFimDistribuicaoEletiva)
                        {
                            mensagens.Add("A data de INÍCIO DA DISTRIBUIÇÃO ELETIVA não pode ser maior que a data de fim da Distribuição Eletiva.");
                        }
                    }
                }

                if (mensagens.Count == 0)
                {
                    try
                    {
                        contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();


                        if (cadastro)
                        {
                            if (this.ExisteAnoLetivoPor(contexto, Convert.ToDecimal(periodoLetivo.Ano), Convert.ToDecimal(periodoLetivo.Periodo)))
                            {
                                mensagens.Add("ANO / PERÍODO Letivo já cadastrado.");
                            }
                        }
                        else
                        {
                            int total = ObtemTotalSubPeriodo(Convert.ToInt32(periodoLetivo.Ano), Convert.ToInt32(periodoLetivo.Periodo));

                            if (periodoLetivo.QtdeSubperiodo < total)
                            {
                                mensagens.Add("A quantidade de ciclos avaliativos não pode ser menor que a quantidade de períodos letivos já cadastrado para este ano/período.");
                            }
                        }



                        if (periodoLetivo.PerAno != null && periodoLetivo.PerPeriodo != null)
                        {
                            if (!this.ExisteAnoLetivoPor(contexto, Convert.ToDecimal(periodoLetivo.PerAno), Convert.ToDecimal(periodoLetivo.PerPeriodo)))
                            {
                                mensagens.Add("PRÓXIMO ANO / PERIODO Letivo não cadastrado.");
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

        public void Insere(Entidades.LyPeriodoLetivo periodoLetivo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO dbo.LY_PERIODO_LETIVO
                                                   (ANO
                                                   ,PERIODO
                                                   ,DESCRICAO
                                                   ,PER_ANO
                                                   ,PER_PERIODO
                                                   ,DT_INICIO
                                                   ,DT_FIM
                                                   ,DT_INICIO_AULA
                                                   ,DT_FIM_AULA
                                                   ,DATA_INICIO_DOCENTE
                                                   ,DATA_FIM_DOCENTE
                                                   ,DATA_INICIO_INDICACAO_ELETIVA
                                                   ,DATA_FIM_INDICACAO_ELETIVA
                                                   ,DATA_INICIO_DISTRIBUICAO_ELETIVA
                                                   ,DATA_FIM_DISTRIBUICAO_ELETIVA
                                                   ,QTDE_SUBPERIODO
                                                   ,USUARIOID
                                                   ,DATACADASTRO
                                                   ,DATAALTERACAO)
                                             VALUES
                                                   (@ANO
                                                   ,@PERIODO
                                                   ,@DESCRICAO
                                                   ,@PER_ANO
                                                   ,@PER_PERIODO
                                                   ,@DT_INICIO
                                                   ,@DT_FIM
                                                   ,@DT_INICIO_AULA
                                                   ,@DT_FIM_AULA
                                                   ,@DATA_INICIO_DOCENTE
                                                   ,@DATA_FIM_DOCENTE
                                                   ,@DATA_INICIO_INDICACAO_ELETIVA
                                                   ,@DATA_FIM_INDICACAO_ELETIVA
                                                   ,@DATA_INICIO_DISTRIBUICAO_ELETIVA
                                                   ,@DATA_FIM_DISTRIBUICAO_ELETIVA
                                                   ,@QTDE_SUBPERIODO
                                                   ,@USUARIOID
                                                   ,@DATACADASTRO
                                                   ,@DATAALTERACAO) ";

                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, periodoLetivo.Ano);
                contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodoLetivo.Periodo);
                contextQuery.Parameters.Add("@PER_ANO", TechneDbType.T_ANO, periodoLetivo.PerAno);
                contextQuery.Parameters.Add("@PER_PERIODO", TechneDbType.T_SEMESTRE2, periodoLetivo.PerPeriodo);
                contextQuery.Parameters.Add("@DT_INICIO", TechneDbType.T_DATA, periodoLetivo.DtInicio);
                contextQuery.Parameters.Add("@DT_FIM", TechneDbType.T_DATA, periodoLetivo.DtFim);
                contextQuery.Parameters.Add("@DT_INICIO_AULA", TechneDbType.T_DATA, periodoLetivo.DtInicioAula);
                contextQuery.Parameters.Add("@DT_FIM_AULA", TechneDbType.T_DATA, periodoLetivo.DtFimAula);
                contextQuery.Parameters.Add("@DATA_INICIO_DOCENTE", SqlDbType.DateTime, periodoLetivo.DataInicioDocente);
                contextQuery.Parameters.Add("@DATA_FIM_DOCENTE", SqlDbType.DateTime, periodoLetivo.DataFimDocente);
                contextQuery.Parameters.Add("@DATA_INICIO_INDICACAO_ELETIVA", SqlDbType.DateTime, periodoLetivo.DataInicioIndicacaoEletiva);
                contextQuery.Parameters.Add("@DATA_FIM_INDICACAO_ELETIVA", SqlDbType.DateTime, periodoLetivo.DataFimIndicacaoEletiva);
                contextQuery.Parameters.Add("@DATA_INICIO_DISTRIBUICAO_ELETIVA", SqlDbType.DateTime, periodoLetivo.DataInicioDistribuicaoEletiva);
                contextQuery.Parameters.Add("@DATA_FIM_DISTRIBUICAO_ELETIVA", SqlDbType.DateTime, periodoLetivo.DataFimDistribuicaoEletiva);
                contextQuery.Parameters.Add("@QTDE_SUBPERIODO", SqlDbType.Int, periodoLetivo.QtdeSubperiodo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoLetivo.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, periodoLetivo.Descricao);

                contexto.ApplyModifications(contextQuery);
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
        public void Atualiza(Entidades.LyPeriodoLetivo periodoLetivo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE dbo.LY_PERIODO_LETIVO
	                                        SET DESCRICAO = @DESCRICAO,
                                                PER_ANO = @PER_ANO,
                                                PER_PERIODO = @PER_PERIODO,
                                                DT_INICIO = @DT_INICIO,
                                                DT_FIM = @DT_FIM,
                                                DT_INICIO_AULA = @DT_INICIO_AULA,
                                                DT_FIM_AULA = @DT_FIM_AULA,
                                                DATA_INICIO_DOCENTE = @DATA_INICIO_DOCENTE,
                                                DATA_FIM_DOCENTE = @DATA_FIM_DOCENTE,
                                                DATA_INICIO_INDICACAO_ELETIVA = @DATA_INICIO_INDICACAO_ELETIVA,
                                                DATA_FIM_INDICACAO_ELETIVA = @DATA_FIM_INDICACAO_ELETIVA,
                                                DATA_INICIO_DISTRIBUICAO_ELETIVA = @DATA_INICIO_DISTRIBUICAO_ELETIVA,
                                                DATA_FIM_DISTRIBUICAO_ELETIVA = @DATA_FIM_DISTRIBUICAO_ELETIVA,
                                                QTDE_SUBPERIODO = @QTDE_SUBPERIODO,
                                                USUARIOID = @USUARIOID,
                                                DATAALTERACAO  = @DATAALTERACAO
                                        WHERE ANO = @ANO
	                                        AND PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, periodoLetivo.Ano);
                contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodoLetivo.Periodo);
                contextQuery.Parameters.Add("@PER_ANO", TechneDbType.T_ANO, periodoLetivo.PerAno);
                contextQuery.Parameters.Add("@PER_PERIODO", TechneDbType.T_SEMESTRE2, periodoLetivo.PerPeriodo);
                contextQuery.Parameters.Add("@DT_INICIO", TechneDbType.T_DATA, periodoLetivo.DtInicio);
                contextQuery.Parameters.Add("@DT_FIM", TechneDbType.T_DATA, periodoLetivo.DtFim);
                contextQuery.Parameters.Add("@DT_INICIO_AULA", TechneDbType.T_DATA, periodoLetivo.DtInicioAula);
                contextQuery.Parameters.Add("@DT_FIM_AULA", TechneDbType.T_DATA, periodoLetivo.DtFimAula);
                contextQuery.Parameters.Add("@DATA_INICIO_DOCENTE", SqlDbType.DateTime, periodoLetivo.DataInicioDocente);
                contextQuery.Parameters.Add("@DATA_FIM_DOCENTE", SqlDbType.DateTime, periodoLetivo.DataFimDocente);
                contextQuery.Parameters.Add("@DATA_INICIO_INDICACAO_ELETIVA", SqlDbType.DateTime, periodoLetivo.DataInicioIndicacaoEletiva);
                contextQuery.Parameters.Add("@DATA_FIM_INDICACAO_ELETIVA", SqlDbType.DateTime, periodoLetivo.DataFimIndicacaoEletiva);
                contextQuery.Parameters.Add("@DATA_INICIO_DISTRIBUICAO_ELETIVA", SqlDbType.DateTime, periodoLetivo.DataInicioDistribuicaoEletiva);
                contextQuery.Parameters.Add("@DATA_FIM_DISTRIBUICAO_ELETIVA", SqlDbType.DateTime, periodoLetivo.DataFimDistribuicaoEletiva);
                contextQuery.Parameters.Add("@QTDE_SUBPERIODO", SqlDbType.Int, periodoLetivo.QtdeSubperiodo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, periodoLetivo.UsuarioId);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DESCRICAO", SqlDbType.VarChar, periodoLetivo.Descricao);

                contexto.ApplyModifications(contextQuery);
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
        public ValidacaoDados ValidaRemocao(decimal ano, decimal periodo)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.SubperiodoLetivo rnSubPeriodoLetivo = new SubperiodoLetivo();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (periodo < 0)
            {
                mensagens.Add("Campo PERIODO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se possui subperiodo cadastrado
                    if (rnSubPeriodoLetivo.PossuiAnoPeriodoPor(contexto, Convert.ToInt32(ano), Convert.ToInt32(periodo)))
                    {
                        mensagens.Add("Este Ano letivo não pode ser excluído pois possui periodos letivos cadastrados.");
                    }

                    //Verificar é proximo ano / periodo de outro ano letivo
                    if (this.EhProximoAnoLetivoPor(contexto, ano, periodo))
                    {
                        mensagens.Add("Este Ano letivo não pode ser excluído pois foi utilizado como próximo ano letivo.");
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

        public bool EhPeriodoIndicacaoEletiva()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" select COUNT(1)
                        from LY_PERIODO_LETIVO
                        where GETDATE() BETWEEN DATA_INICIO_INDICACAO_ELETIVA 
		                        AND DATA_FIM_INDICACAO_ELETIVA ";

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

        public bool EhPeriodoDistribuicaoEletiva()
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" select COUNT(1)
                        from LY_PERIODO_LETIVO
                        where GETDATE() BETWEEN DATA_INICIO_DISTRIBUICAO_ELETIVA 
		                            AND DATA_FIM_DISTRIBUICAO_ELETIVA ";

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

        public void Remove(decimal ano, decimal periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE dbo.LY_PERIODO_LETIVO
                                          WHERE ANO = @ANO
	                                            AND PERIODO = @PERIODO ";

                contextQuery.Parameters.Add("@ANO", TechneDbType.T_ANO, ano);
                contextQuery.Parameters.Add("@PERIODO", TechneDbType.T_SEMESTRE2, periodo);

                contexto.ApplyModifications(contextQuery);
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
        public DataTable ListaAnoAberto()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable ano = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                            ANO
                            FROM   LY_PERIODO_LETIVO (NOLOCK) 
                            WHERE CONVERT(DATE, GETDATE()) BETWEEN DT_INICIO AND DT_FIM
		                    ORDER BY ANO ";


                ano = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return ano;
        }

        public DataTable ListaAnoPatrimonio()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable ano = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                            ANO
                            FROM   LY_PERIODO_LETIVO (NOLOCK) 
                            WHERE ANO >= 2018
		                    ORDER BY ANO DESC";


                ano = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return ano;
        }

        public DataTable ListaAnoPeriodoAlteracaoAluno()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable ano = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                            ANO
                                          FROM  LY_PERIODO_LETIVO (NOLOCK) 
                                          WHERE ANO >= 2024
		                                  ORDER BY ANO DESC";


                ano = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return ano;
        }

        public DataTable ListaAnoEletiva()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable ano = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                            ANO
                            FROM   LY_PERIODO_LETIVO (NOLOCK) 
                            WHERE ANO >= 2022
		                    ORDER BY ANO DESC";


                ano = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return ano;
        }

        public bool EhPeriodoDistribuicaoEletivaPor(int ano, int periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" select COUNT(1)
                        from LY_PERIODO_LETIVO
                        where GETDATE() BETWEEN DATA_INICIO_DISTRIBUICAO_ELETIVA 
		                            AND DATA_FIM_DISTRIBUICAO_ELETIVA 
                               AND ANO = @ANO
                               AND PERIODO = @PERIODO";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

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

        public DataTable ListaProximosAnos()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable ano = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT
                                            ANO
                            FROM   LY_PERIODO_LETIVO (NOLOCK) 
                             WHERE ANO >= YEAR(GETDATE())
		                    ORDER BY ANO DESC";


                ano = ctx.GetDataTable(contextQuery);
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                                  Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }

            return ano;
        }

        public bool EhMesDentroPeriodoLetivo(int ano, int periodo, int mes)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            DateTime data_do_combo;

            try
            {
                data_do_combo = new DateTime(ano, mes, 1);

                contextQuery.Command = @" select COUNT(1)
                        from LY_PERIODO_LETIVO
                        where ANO = @ANO
                        AND PERIODO = @PERIODO  
                         AND @PRIMEIRODIAMES between DT_INICIO and DT_FIM
                            ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@PRIMEIRODIAMES", data_do_combo);

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

        public static DataTable ListarPeriodoFrequencia(string ano)
        {
            var dataTable = new DataTable();

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery();

                contextQuery.Command = "select ISNULL(CAST(PERIODO AS VARCHAR) + ' - ' + ID_REDUZIDA, PERIODO) ID_REDUZIDA, PERIODO from ly_periodo_letivo WHERE ANO = @ANO and PERIODO = 0 ORDER BY PERIODO";
                contextQuery.Parameters.Add("@ANO", ano);

                dataTable = ctx.GetDataTable(contextQuery);
            }

            return dataTable;
        }


        public int ObtemQtdeSubPeriodo(int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            int qtde = 0;
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT QTDE_SUBPERIODO
                                             from LY_PERIODO_LETIVO
                                                where ANO = @ANO
                                                AND PERIODO = @PERIODO  
                                              ";


                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    qtde = Convert.ToInt32(reader["QTDE_SUBPERIODO"]);
                }
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
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return qtde;
        }

        public int ObtemTotalSubPeriodo(int ano, int periodo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            int total = 0;
            SqlDataReader reader = null;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) as TOTAL
                                             from LY_SUBPERIODO_LETIVO
                                                where ANO = @ANO
                                                AND PERIODO = @PERIODO  
                                              ";


                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["TOTAL"]);
                }
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
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return total;
        }       
    }
}
