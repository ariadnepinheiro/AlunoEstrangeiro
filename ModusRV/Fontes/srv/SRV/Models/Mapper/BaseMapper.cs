using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using SRV.Models.DTO;

namespace SRV.Models.Mapper
{
    public abstract class BaseMapper<T>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(BaseMapper<T>));
        
        public SqlConnection connection { get; set; }
        public SqlTransaction transaction { get; set; }

        abstract protected string QueryFindObject();

        abstract protected string QueryListObjects();

        abstract public T LoadObject(SqlDataReader reader);

        protected int? GetNextValSequence(string tableName)
        {
            string query = @"DECLARE @val INT
                                EXEC @val = sp_sequence_nextval @tableName
                              SELECT @val";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;

            if (transaction != null)
                cmd.Transaction = transaction;

            cmd.Parameters.AddWithValue("tableName", tableName.ToUpper());

            log.Debug("SQL: " + cmd.CommandText);

            SqlDataReader reader = cmd.ExecuteReader();

            int? result = null;

            if (reader.Read())
            {
                result = Convert.ToInt32(reader[0]);
            }
            if (reader != null)
                reader.Dispose();
            if (cmd != null)
                cmd.Dispose();

            return result;
        }

        /// <summary>
        /// Executa uma query escalar (que retorna um único valor) e retorna o resultado. 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected object ExecuteScalarQuery(string query, IDictionary param)
        {
            object result;

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;

            if (transaction != null)
                cmd.Transaction = transaction;

            if (param != null)
                foreach (DictionaryEntry de in param)
                {
                    cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value ?? DBNull.Value);

                    if (de.Value is string)
                        cmd.Parameters[cmd.Parameters.Count - 1].DbType = DbType.AnsiString;
                }

            log.Debug("SQL: " + cmd.CommandText);

            result = cmd.ExecuteScalar();

            if (cmd != null)
                cmd.Dispose();

            if (result == DBNull.Value)
                result = null;

            return result;
        }


        /// <summary>
        /// Busca um objeto baseado na primary key informada como parâmetro. A query usada é definida através do método
        /// <see cref="QueryFindObject"/>. O objeto será carrega através do método <see cref="LoadObject"/>
        /// </summary>
        /// <param name="pkName">Nome da primary key</param>
        /// <param name="pkValue">Valor da primary key</param>
        /// <returns></returns>
        protected T FindObject(string pkName, long pkValue)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add(pkName, pkValue);

            return FindObject(QueryFindObject(), param, LoadObject);
        }

        /// <summary>
        /// Busca um objeto baseado na query definida no método <see cref="QueryFindObject"/> com parâmetros dinâmicos
        /// definidos pelo param. O objeto será carrega através do método <see cref="LoadObject"/>
        /// </summary>
        /// <param name="param">Parâmetros da query</param>
        /// <returns></returns>
        protected T FindObject(IDictionary param)
        {
            return FindObject(QueryFindObject(), param, LoadObject);
        }

        /// <summary>
        /// Busca um objeto baseado na query e parâmetros dinâmicos informados. 
        /// O objeto será carrega através do método <see cref="LoadObject"/>
        /// </summary>
        /// <param name="query">query a ser utilizada</param>
        /// <param name="param">Parâmetros da query</param>
        /// <returns></returns>
        protected T FindObject(string query, IDictionary param)
        {
            return FindObject(query, param, LoadObject);
        }

        /// <summary>
        /// Busca um objeto baseado na query e parâmetros dinâmicos informados. 
        /// O objeto será carrega através do método definido pelo parâmetro loadObject
        /// </summary>
        /// <param name="query">query a ser utilizada</param>
        /// <param name="param">Parâmetros da query</param>
        /// <param name="loadObject">Método responsável pela carga do objeto</param>
        /// <returns></returns>
        protected T FindObject(string query, IDictionary param, Func<SqlDataReader, T> loadObject)
        {
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;

            if (transaction != null)
                cmd.Transaction = transaction;

            if (param != null)
                foreach (DictionaryEntry de in param)
                {
                    cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value ?? DBNull.Value);

                    if (de.Value is string)
                        cmd.Parameters[cmd.Parameters.Count - 1].DbType = DbType.AnsiString;
                }

            log.Debug("SQL: " + cmd.CommandText);

            SqlDataReader reader = cmd.ExecuteReader();

            T result = default(T);

            if (reader.Read())
            {
                result = loadObject(reader);
            }
            if (reader != null)
                reader.Dispose();
            if (cmd != null)
                cmd.Dispose();

            return result;
        }


        /// <summary>
        /// Retorna uma lista de objetos baseado na query definida no método <see cref="QueryListObjects"/>.
        /// Os objetos serão carregados através do método <see cref="LoadObject"/>
        /// </summary>
        /// <returns></returns>
        protected IList<T> ListObjects()
        {
            return ListObjects(QueryListObjects(), null, LoadObject);
        }

        /// <summary>
        /// Retorna uma lista de objetos baseado na query definida no método <see cref="QueryListObjects"/>
        /// com parâmetros dinâmicos definidos pelo param.
        /// Os objetos serão carregados através do método <see cref="LoadObject"/>
        /// </summary>
        /// <param name="param">Parâmetros da query</param>
        /// <returns></returns>
        protected IList<T> ListObjects(IDictionary param)
        {
            return ListObjects(QueryListObjects(), param, LoadObject);
        }

        /// <summary>
        /// Retorna uma lista de objetos baseado na query e parâmetros dinâmicos informados.
        /// Os objetos serão carregados através do método <see cref="LoadObject"/>
        /// </summary>
        /// <param name="query">Query a ser utilizada</param>
        /// <param name="param">Parâmetros da query</param>
        /// <returns></returns>
        protected IList<T> ListObjects(string query, IDictionary param)
        {
            return ListObjects(query, param, LoadObject);
        }

        /// <summary>
        /// Retorna uma lista de objetos baseado na query e parâmetros dinâmicos informados.
        /// Os objetos serão carregados através do método definido pelo parâmetro loadObject
        /// </summary>
        /// <param name="query">Query a ser utilizada</param>
        /// <param name="param">Parâmetros da query</param>
        /// <param name="loadObject">Método responsável pela carga do objeto</param>
        /// <returns></returns>
        protected IList<T> ListObjects(string query, IDictionary param, Func<SqlDataReader, T> loadObject)
        {
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;

            if (transaction != null)
                cmd.Transaction = transaction;

            if(param != null)
                foreach (DictionaryEntry de in param)
                {
                    cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value ?? DBNull.Value);

                    if (de.Value is string)
                        cmd.Parameters[cmd.Parameters.Count - 1].DbType = DbType.AnsiString;
                }

            log.Debug("SQL: " + cmd.CommandText);

            SqlDataReader reader = cmd.ExecuteReader();

            IList<T> result = new List<T>();

            while (reader.Read())
            {
                result.Add(loadObject(reader));
            }
            if (reader != null)
                reader.Dispose();
            if (cmd != null)
                cmd.Dispose();

            return result;
        }

        /// <summary>
        /// Retorna um objeto <see cref="Paging"/> que contém uma lista de objetos baseado na query default,
        /// realizando paginação.
        /// Os objetos serão carregados através do método <see cref="LoadObject"/>
        /// </summary>
        /// <param name="currentPage">Pagina a ser carregada</param>
        /// <param name="pageSize">Número de itens por página</param>
        /// <returns></returns>
        protected Paging<T> ListPagingObjects(int currentPage, int pageSize)
        {
            return ListPagingObjects(QueryListObjects(), null, LoadObject, currentPage, pageSize);
        }

        /// <summary>
        /// Retorna um objeto <see cref="Paging"/> que contém uma lista de objetos baseado na query
        /// e parâmetros dinâmicos informados, realizando paginação.
        /// Os objetos serão carregados através do método <see cref="LoadObject"/>
        /// </summary>
        /// <param name="query">Query a ser utilizada</param>
        /// <param name="param">Parâmetros da query</param>
        /// <param name="currentPage">Pagina a ser carregada</param>
        /// <param name="pageSize">Número de itens por página</param>
        /// <returns></returns>
        protected Paging<T> ListPagingObjects(string query, IDictionary param, int currentPage, int pageSize)
        {
            return ListPagingObjects(query, param, LoadObject, currentPage, pageSize);
        }

        /// <summary>
        /// Retorna um objeto <see cref="Paging"/> que contém uma lista de objetos baseado na query
        /// e parâmetros dinâmicos informados, realizando paginação.
        /// Os objetos serão carregados através do método definido pelo parâmetro loadObject
        /// </summary>
        /// <param name="query">Query a ser utilizada</param>
        /// <param name="param">Parâmetros da query</param>
        /// <param name="loadObject">Método responsável pela carga do objeto</param>
        /// <param name="currentPage">Pagina a ser carregada</param>
        /// <param name="pageSize">Número de itens por página</param>
        /// <returns></returns>
        protected Paging<T> ListPagingObjects(string query, IDictionary param, Func<SqlDataReader, T> loadObject, int currentPage, int pageSize)
        {
            query = PagingQuery(query, currentPage, pageSize);

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;

            if (transaction != null)
                cmd.Transaction = transaction;

            if (param != null)
                foreach (DictionaryEntry de in param)
                {
                    cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value ?? DBNull.Value);

                    if (de.Value is string)
                        cmd.Parameters[cmd.Parameters.Count - 1].DbType = DbType.AnsiString;
                }

            cmd.Parameters.AddWithValue("LowerBound", (currentPage - 1) * pageSize + 1);
            cmd.Parameters.AddWithValue("HigherBound", currentPage * pageSize);


            log.Debug("SQL: " + cmd.CommandText);

            SqlDataReader reader = cmd.ExecuteReader();

            IList<T> result = new List<T>();

            int totalItems = 0;

            while (reader.Read())
            {
                if(totalItems == 0)
                    totalItems = Convert.ToInt32(reader["__totalItems"]);

                result.Add(loadObject(reader));
            }
            if (reader != null)
                reader.Dispose();
            if (cmd != null)
                cmd.Dispose();

            Paging<T> resultPaging = new Paging<T>();
            resultPaging.CurrentPage = currentPage;
            resultPaging.PageSize = pageSize;
            resultPaging.Pages = (int)(-1L + totalItems + pageSize) / pageSize;
            resultPaging.TotalItems = totalItems;
            resultPaging.Items = result;

            return resultPaging;
        }


        /// <summary>
        /// Realiza a chamda de uma function do banco de dados
        /// </summary>
        /// <param name="functionName">Nome da function</param>
        /// <param name="returnType">Tipo do dado de retorno</param>
        /// <param name="param">Parâmetros da function</param>
        /// <returns></returns>
        protected Object CallFunction(string functionName, DbType returnType, IDictionary param)
        {
            SqlCommand cmd = new SqlCommand(functionName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            if (transaction != null)
                cmd.Transaction = transaction;

            SqlParameter returnValue = new SqlParameter("returnValue", SqlDbType.VarChar, 4000);
            returnValue.DbType = returnType;
            returnValue.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnValue);

            foreach (DictionaryEntry de in param)
            {
                cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value ?? DBNull.Value);

                if (de.Value is string)
                    cmd.Parameters[cmd.Parameters.Count - 1].DbType = DbType.AnsiString;
            }

            log.Debug("Chamando Function: " + functionName);

            cmd.ExecuteNonQuery();

            return returnValue.Value;
        }

        protected void CallProcedure(string procedureName, IDictionary param)
        {
            SqlCommand cmd = new SqlCommand(procedureName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandTimeout = 1500; //25 min

            if (transaction != null)
                cmd.Transaction = transaction;

            foreach (DictionaryEntry de in param)
            {
                cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value ?? DBNull.Value);

                if (de.Value is string)
                    cmd.Parameters[cmd.Parameters.Count - 1].DbType = DbType.AnsiString;
            }

            log.Debug("Chamando Procedure: " + procedureName);

            cmd.ExecuteNonQuery();
        }


        protected string PagingQuery(string query, int currentPage, int pageSize)
        {
            query = ReplaceFirst(query, "SELECt", String.Format("SELECT TOP {0} COUNT(*) OVER() __TotalItems,", currentPage * pageSize));

            return String.Format(@"WITH __actualSet AS 
                                    ( 
                                        SELECT *, ROW_NUMBER() OVER (ORDER BY CURRENT_TIMESTAMP) AS __rowcnt 
                                        FROM ( {0} ) AS _tmpSet
                                    ) 
                                    SELECT * FROM __actualSet 
                                     WHERE [__rowcnt] >= @LowerBound
                                       AND [__rowcnt] <= @HigherBound
                                     ORDER BY [__rowcnt] ASC ", query);

        }

        protected void InsertObject(string query, IDictionary param)
        {
            ExecuteNonQuery(query, param);
        }

        /// <summary>
        /// Executa um Insert no banco de dados em uma tabela que a chave primária seja IDENTITY e retorna 
        /// o valor da chave gerada.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected int? InsertObjectWithIdentity(string query, IDictionary param)
        {
            return ExecuteNonQueryWithIdentity(query, param);
        }

        protected void UpdateObject(string query, IDictionary param)
        {
            ExecuteNonQuery(query, param);
        }

        protected void DeleteObject(string query, IDictionary param)
        {
            ExecuteNonQuery(query, param);
        }

        private void ExecuteNonQuery(string query, IDictionary param)
        {
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;

            if (transaction != null)
                cmd.Transaction = transaction;

            if (param != null)
                foreach (DictionaryEntry de in param)
                {
                    cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value ?? DBNull.Value);

                    if (de.Value is string)
                        cmd.Parameters[cmd.Parameters.Count - 1].DbType = DbType.AnsiString;
                }

            log.Debug("SQL: " + cmd.CommandText);

            cmd.ExecuteNonQuery();

            if (cmd != null)
                cmd.Dispose();
        }

        private int? ExecuteNonQueryWithIdentity(string query, IDictionary param)
        {
            query = query + Environment.NewLine + "SELECT SCOPE_IDENTITY()";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;

            if (transaction != null)
                cmd.Transaction = transaction;

            if (param != null)
                foreach (DictionaryEntry de in param)
                {
                    cmd.Parameters.AddWithValue(de.Key.ToString(), de.Value ?? DBNull.Value);

                    if (de.Value is string)
                        cmd.Parameters[cmd.Parameters.Count - 1].DbType = DbType.AnsiString;
                }

            log.Debug("SQL: " + cmd.CommandText);

            SqlDataReader reader = cmd.ExecuteReader();

            int? result = null;

            if (reader.Read())
            {
                result = Convert.ToInt32(reader[0]);
            }

            if (reader != null)
                reader.Dispose();
            if (cmd != null)
                cmd.Dispose();

            return result;
        }

        protected string StringToUpper(string param)
        {
            return param != null ? param.ToUpper() : null;
        }

        protected string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search, StringComparison.CurrentCultureIgnoreCase);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

    }
}