namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using Seeduc.Infra.Data;
    using Seeduc.Infra.Helpers;
    using Techne.Data;
    using Techne.Library;
    using System.Text.RegularExpressions;


    public abstract class RNBase
    {
        private static readonly string stringConn = Regex.Replace(ConnectionList.GetConnectionString("Lyceum"), "Provider=SQLOLEDB.1;", String.Empty, RegexOptions.IgnoreCase);

        public static string StringConn
        {
            get { return RNBase.stringConn; }
        }

        public static string HdPal(string text)
        {
            var b = _HdPal(text);

            if (b.Length == 0)
            {
                return string.Empty;
            }

            return Encoding.GetEncoding(1252).GetString(b);
        }

        public static string MudarAspas(object parametro)
        {
            if (parametro != null)
            {
                var retorno = Convert.ToString(parametro);

                retorno = retorno.Replace("'", "''");

                return retorno;
            }

            return string.Empty;
        }

        public static void RetirarEspaco(DataRow row)
        {
            if (row != null
                && row.Table != null
                && row.Table.Columns.Count > 0)
            {
                foreach (DataColumn col in row.Table.Columns)
                {
                    if (col.DataType == typeof(string)
                        && !col.ReadOnly
                        && row[col] != null)
                    {
                        var valor = Convert.ToString(row[col]);

                        if (!string.IsNullOrEmpty(valor))
                        {
                            row[col] = valor.Trim();
                        }
                    }
                }
            }
        }

        protected static QueryTable Consultar(string query, params object[] parametros)
        {
            var qt = new QueryTable(query);

            using (var connection = Config.CreateConnection())
            {
                connection.Open();

                try
                {
                    qt.Query(connection, parametros);
                }
                catch
                {
                    connection.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }

            return qt;
        }

        /// <summary>
        ///   Realiza uma consulta no banco de dados do Lyceum.
        /// </summary>
        /// <param name = "timeout">Tempo de timeout, em Segundos. (Deve ser maior que zero)</param>
        /// <param name = "query">Consulta SQL.</param>
        /// <param name = "parametros">Lista de parâmetros da consulta.</param>
        /// <returns>QueryTable contendo os registros encontrados.</returns>
        protected static QueryTable Consultar(int timeout, string query, params object[] parametros)
        {
            var qt = new QueryTable(query)
            {
                CommandTimeout = timeout
            };

            using (var connection = Config.CreateConnection())
            {
                connection.Open();

                try
                {
                    qt.Query(connection, parametros);
                }
                catch
                {
                    connection.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }

            return qt;
        }

        protected static QueryTable Consultar(TConnection connection, string query, params DbObject[] parametros)
        {
            var qt = new QueryTable(query);

            qt.Query(connection, parametros);

            return qt;
        }

        protected static DataTable Consultar(ContextQuery contextQuery)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                return ctx.GetDataTable(contextQuery);
            }
        }

        protected static string ConsultarCampo(string consulta, params DbObject[] parametros)
        {
            var contextQuery = ContextQueryConverter.FromTechne(consulta, parametros);

            return ConsultarCampo(contextQuery);
        }

        protected static string ConsultarCampo(TConnection conn, string consulta, params DbObject[] parametros)
        {
            var result = TCommand.ExecuteScalar(conn, consulta, parametros);

            if (!result.IsNull)
            {
                return Convert.ToString(result);
            }

            return string.Empty;
        }

        protected static bool ConsultarSolicitacao(TConnection conn, string consulta, params DbObject[] parametros)
        {
            var result = TCommand.ExecuteScalar(conn, consulta, parametros);

            if (!result.IsNull)
            {
                if ((int)result > 0)
                {
                    return true;
                }
            }

            return false;
        }

        protected static string ConsultarCampo(ContextQuery contextQuery)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var result = ctx.GetReturnValue(contextQuery);

                if (result != null
                    && result != DBNull.Value)
                {
                    return result.ToString();
                }
            }

            return string.Empty;
        }

        protected static string ConsultarCampoHades(ContextQuery contextQuery)
        {
            using (var ctx = DataContextBuilder.FromHades.ToFastReadingOnly())
            {
                var result = ctx.GetReturnValue(contextQuery);

                if (result != null
                    && result != DBNull.Value)
                {
                    return result.ToString();
                }
            }

            return string.Empty;
        }

        protected static string ConsultarCampoHades(string consulta, params DbObject[] parametros)
        {
            var contextQuery = ContextQueryConverter.FromTechne(consulta, parametros);

            return ConsultarCampoHades(contextQuery);
        }

        protected static QueryTable ConsultarHades(string query, params object[] parametros)
        {
            var qt = new QueryTable(query);

            using (var connection = Config.CreateConnection())
            {
                connection.Open();

                try
                {
                    qt.Query(connection, parametros);
                }
                catch
                {
                    connection.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }

            return qt;
        }

        protected static int ExecutarAlteracao(ContextQuery contextQuery)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    return ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();

                    throw;
                }
            }
        }

        protected static int ExecutarFuncao(ContextQuery contextQuery)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var result = ctx.GetReturnValue(contextQuery);

                if (result != null
                    && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }

            return 0;
        }

        protected static T ExecutarFuncao<T>(ContextQuery contextQuery)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                return ctx.GetReturnValue<T>(contextQuery);
            }
        }

        protected static int ExecutarFuncao(string consulta, params DbObject[] parametros)
        {
            var contextQuery = ContextQueryConverter.FromTechne(consulta, parametros);

            return ExecutarFuncao(contextQuery);
        }

        protected static int ExecutarFuncao(string consulta, TConnection connection, params DbObject[] parametros)
        {
            var result = TCommand.ExecuteScalar(connection, consulta, parametros);

            if (!result.IsNull)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        protected static decimal ExecutarFuncaoDec(string consulta, params DbObject[] parametros)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var contextQuery = ContextQueryConverter.FromTechne(consulta, parametros);
                var result = ctx.GetReturnValue(contextQuery);

                if (result != null
                    && result != DBNull.Value)
                {
                    return Convert.ToDecimal(result);
                }
            }

            return 0m;
        }

        protected static int ExecutarFuncaoHades(ContextQuery contextQuery)
        {
            using (var ctx = DataContextBuilder.FromHades.ToFastReadingOnly())
            {
                var result = ctx.GetReturnValue(contextQuery);

                if (result != null
                    && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }

            return 0;
        }

        protected static int ExecutarFuncaoHades(string consulta, params DbObject[] parametros)
        {
            var contextQuery = ContextQueryConverter.FromTechne(consulta, parametros);

            return ExecutarFuncaoHades(contextQuery);
        }

        protected static int ExecutarFuncaoHades(TConnectionWritable connection, string consulta, params DbObject[] parametros)
        {
            var result = TCommand.ExecuteScalar(connection, consulta, parametros);

            if (!result.IsNull)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        protected static int ExecutarFuncaoRetMenosUm(string consulta, TConnection connection, params DbObject[] parametros)
        {
            var result = TCommand.ExecuteScalar(connection, consulta, parametros);

            if (!result.IsNull)
            {
                return Convert.ToInt32(result);
            }

            return -1;
        }

        protected static DbObject ExecutarFuncaoScalar(ContextQuery contextQuery)
        {
            using (var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly())
            {
                var result = ctx.GetReturnValue(contextQuery);

                return DbObject.ToDbObject(result);
            }
        }

        protected static DbObject ExecutarFuncaoScalar(string consulta, params DbObject[] parametros)
        {
            var contextQuery = ContextQueryConverter.FromTechne(consulta, parametros);

            return ExecutarFuncaoScalar(contextQuery);
        }

        protected static DbObject ExecutarFuncaoScalarHades(ContextQuery contextQuery)
        {
            using (var ctx = DataContextBuilder.FromHades.ToFastReadingOnly())
            {
                var result = ctx.GetReturnValue(contextQuery);

                return DbObject.ToDbObject(result);
            }
        }

        protected static DbObject ExecutarFuncaoScalarHades(string consulta, params DbObject[] parametros)
        {
            var contextQuery = ContextQueryConverter.FromTechne(consulta, parametros);

            return ExecutarFuncaoScalarHades(contextQuery);
        }

        protected static RetValue IAE(TConnectionWritable connection, string sql, params DbObject[] parametros)
        {
            TCommand.ExecuteNonQuery(connection, sql, parametros);

            return VerificarErro(connection.GetErrors());
        }

        protected static RetValue IAE(string sql, params DbObject[] parametros)
        {
            RetValue retorno;

            var connection = Config.CreateWritableConnection();

            try
            {
                connection.Open(true);
                TCommand.ExecuteNonQuery(connection, sql, parametros);
                retorno = VerificarErro(connection.GetErrors());

                if (retorno != null
                    && !retorno.Ok)
                {
                    return retorno;
                }
            }
            catch (Exception e)
            {
                connection.Rollback();
                retorno = new RetValue(false, null, new ErrorList(e.Message));
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }

        protected static ColunasTable MontarParametros(DataTable dt)
        {
            var colunas = new ColunasTable();

            foreach (DataColumn col in dt.Columns)
            {
                colunas.Colunas += col.ColumnName + ",";
            }

            colunas.Colunas = colunas.Colunas.Substring(0, colunas.Colunas.LastIndexOf(","));

            if (dt.Rows.Count > 0)
            {
                colunas.ValorColuna = dt.Rows[0].ItemArray;
            }

            return colunas;
        }

        protected static ColunasTable MontarParametros(DataColumnCollection colunas, DataRow dadosParametros)
        {
            var colunasTable = new ColunasTable();

            if (colunas != null)
            {
                for (var i = 0; i < colunas.Count; i++)
                {
                    if (!colunas[i].ReadOnly
                        && !(colunas[i] is TDataColumn
                            && ((TDataColumn)colunas[i]).IsAux))
                    {
                        colunasTable.Colunas += colunas[i].ColumnName + ",";
                    }
                }

                if (colunasTable.Colunas.EndsWith(","))
                {
                    colunasTable.Colunas = colunasTable.Colunas.Remove(colunasTable.Colunas.Length - 1);
                }

                if (dadosParametros != null)
                {
                    var objList = new List<object>();
                    var cols = colunasTable.Colunas.Split(',');

                    foreach (var nomeCol in cols)
                    {
                        objList.Add(dadosParametros[nomeCol]);
                    }

                    colunasTable.ValorColuna = objList.ToArray();
                }
            }

            return colunasTable;
        }

        protected static RetValue VerificarErro(DataTable dt)
        {
            var erro = ObterErro(dt);

            if (erro != null)
            {
                if (erro.Count > 0)
                {
                    return new RetValue(false, string.Empty, erro);
                }
            }

            return null;
        }

        protected static RetValue VerificarErro(DataRow dr)
        {
            var erro = ObterErro(dr);

            if (erro != null)
            {
                if (erro.Count > 0)
                {
                    return new RetValue(false, string.Empty, erro);
                }
            }

            return null;
        }

        protected static RetValue VerificarErro(ErrorList erro)
        {
            if (erro != null)
            {
                if (erro.Count > 0)
                {
                    return new RetValue(false, string.Empty, erro);
                }
            }

            return null;
        }

        private static ErrorList ObterErro(DataTable dt)
        {
            if (dt != null)
            {
                if (dt.Rows != null)
                {
                    foreach (DataRow linha in dt.Rows)
                    {
                        var erroLinha = ErrorList.CreateFromDataRow(linha);

                        if (erroLinha != null)
                        {
                            if (erroLinha.Count > 0)
                            {
                                return erroLinha;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static ErrorList ObterErro(DataRow dr)
        {
            if (dr != null)
            {
                var erroLinha = ErrorList.CreateFromDataRow(dr);

                if (erroLinha != null)
                {
                    if (erroLinha.Count > 0)
                    {
                        return erroLinha;
                    }
                }
            }

            return null;
        }

        private static byte[] _HdPal(string text)
        {
            const string H = "TECHNELYCEUM";

            if (string.IsNullOrEmpty(text))
            {
                return new byte[0];
            }

            if (text.Length > 30)
            {
                text = text.Substring(0, 30);
            }
            else
            {
                text = text.PadRight(30, ' ');
            }

            byte offset = 0;
            var senha = new byte[30];
            var j = 0;
            var enc = Encoding.GetEncoding(1252);

            for (var i = 0; i < 30; i++)
            {
                var a = enc.GetBytes(text.Substring(i, 1))[0];
                var b = enc.GetBytes(H.Substring(j, 1))[0];
                var cod = Convert.ToByte((a + offset + b) & 255);

                senha[i] = cod;
                offset = Convert.ToByte((offset + b) & 255);

                if (++j >= H.Length)
                {
                    j = 0;
                }
            }

            return senha;
        }

        public class ColunasTable
        {
            public string Colunas { get; set; }

            public object[] ValorColuna { get; set; }
        }

        public class StringValueAttribute : Attribute
        {
            public StringValueAttribute(string value)
            {
                this.StringValue = value;
            }

            public string StringValue { get; protected set; }
        }


    }
}