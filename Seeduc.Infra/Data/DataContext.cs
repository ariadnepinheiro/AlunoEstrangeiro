namespace Seeduc.Infra.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Threading;
    using System.Web;
    using Seeduc.Infra.Aspects;
    using Seeduc.Infra.Configuration;
    using Seeduc.Infra.Entities;
    using Seeduc.Infra.Mapper;
    using Techne.Auditing;

    public class DataContext : IDisposable
    {
        private readonly ApplicationName applicationName;

        private readonly string connectionString;

        private readonly ContextQueryBulkManager contextQueryBulkManager;

        private readonly SqlConnection sqlConnection;

        private readonly TransactionContext transactionContext;

        private ProcessMode processMode;

        private bool rollback;

        private SqlTransaction sqlTransaction;

        //[WatchAllExceptions]
        internal DataContext(ApplicationName applicationName, TransactionContext transactionContext)
        {
            var configurationManager = ConfigurationManager.Instance;

            this.applicationName = applicationName;
            this.transactionContext = transactionContext;
            this.connectionString = configurationManager.GetConnectionString(this.applicationName);
            this.sqlConnection = new SqlConnection(this.connectionString);
            this.contextQueryBulkManager = new ContextQueryBulkManager();
            this.processMode = ProcessMode.Single;
            this.CommandTimeout = configurationManager.Section.ContextQuery.ExecutionTimeout;

            try
            {
                this.sqlConnection.Open();
            }
            catch (Exception ex)
            {
                throw new DataContextException("Failed to open the Data Context!", ex);
            }

            if (transactionContext == TransactionContext.WithoutLockAndReadOnly)
            {
                // Since it's dirty read and read only, just rollback after execution
                // to make sure that nothing was changed
                this.rollback = true;

                return;
            }

            try
            {
                // Needed for Techne integration
                this.ExecuteVarsSet();
            }
            catch (Exception ex)
            {
                throw new DataContextException("Failed to prepare the Data Context for writing!", ex);
            }
        }

        //
        internal DataContext(string connString, ApplicationName applicationName, TransactionContext transactionContext)
        {
            var configurationManager = ConfigurationManager.Instance;
            this.applicationName = applicationName;
            this.transactionContext = transactionContext;
            this.connectionString = connString;
            this.sqlConnection = new SqlConnection(this.connectionString);
            this.contextQueryBulkManager = new ContextQueryBulkManager();
            this.processMode = ProcessMode.Single;
            this.CommandTimeout = configurationManager.Section.ContextQuery.ExecutionTimeout;

            try
            {
                this.sqlConnection.Open();
            }
            catch (Exception ex)
            {
                throw new DataContextException("Failed to open the Data Context!", ex);
            }

            if (transactionContext == TransactionContext.WithoutLockAndReadOnly)
            {
                // Since it's dirty read and read only, just rollback after execution
                // to make sure that nothing was changed
                this.rollback = true;

                return;
            }

            try
            {
                // Needed for Techne integration
                this.ExecuteVarsSet();
            }
            catch (Exception ex)
            {
                throw new DataContextException("Failed to prepare the Data Context for writing!", ex);
            }
        }

        public int CommandTimeout { get; set; }

        public void Abandon()
        {
            this.rollback = true;
        }

        public T ApplyModifications<T>(ContextQuery contextQuery, ContextQuery returnQuery) where T : class, IEntity, new()
        {
            T obj = null;

            if (this.transactionContext == TransactionContext.WithoutLockAndReadOnly)
            {
                throw new DataContextException("It's not possible to apply modifications on the Data Context without lock and read only!");
            }

            if (ContextQuery.IsEmpty(contextQuery))
            {
                throw new ContextQueryException("It's not possible to apply modifications with an empty Context Query!");
            }

            try
            {
                SqlCommand sqlCommand;

                sqlCommand = this.GenerateCommand(contextQuery);
                int rowsAffected = sqlCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    obj = TryToBindEntity<T>(returnQuery);
                }
            }
            catch (Exception ex)
            {
                Seeduc.Infra.Aspects.ExceptionsDataContext gravaErro = new Seeduc.Infra.Aspects.ExceptionsDataContext();

                gravaErro.GravaError(ex, contextQuery.ToPlainSql());

                if (ex is SqlException
                    || ex is SqlTypeException)
                {
                    throw new ContextQueryException("Error executing ContextQuery!", ex);
                }

                throw new DataContextException("Failed to apply the modifications on the Data Context!", ex);
            }

            return obj;
        }

        //[WatchAllExceptionsAndTraceContextQuery]
        public int ApplyModifications(ContextQuery contextQuery)
        {
            if (this.transactionContext == TransactionContext.WithoutLockAndReadOnly)
            {
                throw new DataContextException("It's not possible to apply modifications on the Data Context without lock and read only!");
            }

            if (ContextQuery.IsEmpty(contextQuery))
            {
                throw new ContextQueryException("It's not possible to apply modifications with an empty Context Query!");
            }

            try
            {
                SqlCommand sqlCommand;

                if (this.processMode == ProcessMode.Bulk)
                {
                    this.contextQueryBulkManager.Enqueue(contextQuery);

                    if (!this.contextQueryBulkManager.IsFull)
                    {
                        return 0;
                    }

                    sqlCommand = this.GenerateCommand(this.contextQueryBulkManager.Process());
                }
                else
                {
                    sqlCommand = this.GenerateCommand(contextQuery);
                }

                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Seeduc.Infra.Aspects.ExceptionsDataContext gravaErro = new Seeduc.Infra.Aspects.ExceptionsDataContext();

                gravaErro.GravaError(ex, contextQuery.ToPlainSql());

                if (ex is SqlException
                    || ex is SqlTypeException)
                {
                    throw new ContextQueryException("Error executing ContextQuery!", ex);
                }

                throw new DataContextException("Failed to apply the modifications on the Data Context!", ex);
            }
        }

        public void BeginBulkModifications()
        {
            if (!this.contextQueryBulkManager.Enable)
            {
                throw new ContextQueryException("You can not use the bulk feature because it is not properly configured. Please set within the sectionHandler to enable it.");
            }

            this.processMode = ProcessMode.Bulk;
        }

        public void Dispose()
        {
            if (this.sqlConnection == null
                || this.sqlConnection.State != ConnectionState.Open)
            {
                return;
            }

            if (this.sqlTransaction != null)
            {
                if (this.processMode == ProcessMode.Bulk)
                {
                    this.EndBulkModifications();
                }

                if (this.rollback)
                {
                    this.sqlTransaction.Rollback();
                }
                else
                {
                    this.sqlTransaction.Commit();
                }

                this.sqlTransaction.Dispose();
            }

            this.sqlConnection.Close();
            this.sqlConnection.Dispose();
        }

        public void EndBulkModifications()
        {
            if (!this.contextQueryBulkManager.Enable)
            {
                throw new ContextQueryException("You can not use the bulk feature because it is not properly configured. Please set within the sectionHandler to enable it.");
            }

            if (this.processMode != ProcessMode.Bulk)
            {
                throw new ContextQueryException("You can not disable the bulk feature because it is not running.");
            }

            this.processMode = ProcessMode.Single;

            if (this.contextQueryBulkManager.IsEmpty)
            {
                return;
            }

            var contextQuery = this.contextQueryBulkManager.Process();

            this.ApplyModifications(contextQuery);
        }

        //[WatchAllExceptionsAndTraceContextQuery]
        public SqlDataReader GetDataReader(ContextQuery contextQuery)
        {
            if (ContextQuery.IsEmpty(contextQuery))
            {
                throw new ContextQueryException("It's not possible to apply modifications with an empty Context Query!");
            }

            try
            {
                var sqlCommand = this.GenerateCommand(contextQuery);

                return sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                Seeduc.Infra.Aspects.ExceptionsDataContext gravaErro = new Seeduc.Infra.Aspects.ExceptionsDataContext();

                gravaErro.GravaError(ex, contextQuery.ToPlainSql());

                if (ex is SqlException
                    || ex is SqlTypeException)
                {
                    throw new ContextQueryException("Error executing ContextQuery!", ex);
                }

                throw new DataContextException("Failed to get the data from the Data Context!", ex);
            }
        }

        //[WatchAllExceptionsAndTraceContextQuery]
        public DataTable GetDataTable(ContextQuery contextQuery)
        {
            if (ContextQuery.IsEmpty(contextQuery))
            {
                throw new ContextQueryException("It's not possible to apply modifications with an empty Context Query!");
            }

            try
            {
                var sqlCommand = this.GenerateCommand(contextQuery);
                var dataTable = new DataTable();
                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlDataAdapter.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                Seeduc.Infra.Aspects.ExceptionsDataContext gravaErro = new Seeduc.Infra.Aspects.ExceptionsDataContext();

                gravaErro.GravaError(ex,contextQuery.ToPlainSql());

                if (ex is SqlException
                    || ex is SqlTypeException)
                {
                    throw new ContextQueryException("Error executing ContextQuery!", ex);
                }

                throw new DataContextException("Failed to get the data from the Data Context!", ex);
            }
        }

        //[WatchAllExceptionsAndTraceContextQuery]
        public DataSet GetDataSet(ContextQuery contextQuery)
        {
            if (ContextQuery.IsEmpty(contextQuery))
            {
                throw new ContextQueryException("It's not possible to apply modifications with an empty Context Query!");
            }

            try
            {
                var sqlCommand = this.GenerateCommand(contextQuery);
                var dataSet = new DataSet();
                var sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlDataAdapter.Fill(dataSet);

                return dataSet;
            }
            catch (Exception ex)
            {
                Seeduc.Infra.Aspects.ExceptionsDataContext gravaErro = new Seeduc.Infra.Aspects.ExceptionsDataContext();

                gravaErro.GravaError(ex, contextQuery.ToPlainSql());

                if (ex is SqlException
                    || ex is SqlTypeException)
                {
                    throw new ContextQueryException("Error executing ContextQuery!", ex);
                }

                throw new DataContextException("Failed to get the data from the Data Context!", ex);
            }
        }

        //[WatchAllExceptionsAndTraceContextQuery]
        public object GetReturnValue(ContextQuery contextQuery)
        {
            if (ContextQuery.IsEmpty(contextQuery))
            {
                throw new ContextQueryException("It's not possible to apply modifications with an empty Context Query!");
            }

            try
            {
                var sqlCommand = this.GenerateCommand(contextQuery);

                return sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Seeduc.Infra.Aspects.ExceptionsDataContext gravaErro = new Seeduc.Infra.Aspects.ExceptionsDataContext();

                gravaErro.GravaError(ex, contextQuery.ToPlainSql());

                if (ex is SqlException
                    || ex is SqlTypeException)
                {
                    throw new ContextQueryException("Error executing ContextQuery!", ex);
                }

                throw new DataContextException("Failed to get the data from the Data Context!", ex);
            }
        }        

        //[WatchAllExceptionsAndTraceContextQuery]
        public T GetReturnValue<T>(ContextQuery contextQuery)
        {
            try
            {
                var value = this.GetReturnValue(contextQuery);

                if (value == DBNull.Value)
                {
                    value = null;
                }

                return (T)ConvertObject.To(typeof(T), value);
            }
            catch (Exception ex)
            {
                if (ex is OverflowException
                    || ex is InvalidCastException
                    || ex is ArgumentNullException
                    || ex is NullReferenceException)
                {
                    throw new DataContextException("Failed to cast the return value to the specified type!", ex);
                }

                Seeduc.Infra.Aspects.ExceptionsDataContext gravaErro = new Seeduc.Infra.Aspects.ExceptionsDataContext();

                gravaErro.GravaError(ex, contextQuery.ToPlainSql());

                throw;
            }
        }

        public ICollection<T> TryToBindEntities<T>(ContextQuery contextQuery)
            where T : class, IEntity, new()
        {
            var entities = new List<T>();

            using (var result = this.GetDataReader(contextQuery))
            {
                while (result.Read())
                {
                    entities.Add(SqlDataReaderMapper.CreateAndMapTo<T>(result));
                }
            }

            return entities;
        }

        /// <summary>
        /// Transforma o retorno da primeira coluna do SQL em uma lista de objetos.
        /// </summary>
        /// <typeparam name="T">Tipo primitivo para retorno na lista</typeparam>
        /// <param name="contextQuery">Objeto ContextQuery com o SQL a ser executado</param>
        /// <returns>Retorna uma lista de objetos de tipo primitivo</returns>
        /// <example>SELECT TOP 10 NOME FROM ALUNO</example>
        public ICollection<T> TryToBind<T>(ContextQuery contextQuery)
        {
            var entities = new List<T>();
            
            using (var result = this.GetDataReader(contextQuery))
            {
                if (result.HasRows)
                {
                    List<object> objs = new List<object>();

                    while (result.Read())
                    {
                        objs.Add(result.GetValue(0));
                    }

                    entities = objs.ConvertAll<T>(
                        obj => ( (T)Convert.ChangeType(obj, typeof(T)) )
                    );
                }
            }

            return entities;
        }

        public T TryToBindEntity<T>(ContextQuery contextQuery)
            where T : class, IEntity, new()
        {
            using (var result = this.GetDataReader(contextQuery))
            {
                if (result == null
                    || result.IsClosed
                    || !result.HasRows)
                {
                    return new T();
                }

                result.Read();

                return SqlDataReaderMapper.CreateAndMapTo<T>(result);
            }
        }

        private void BeginTransaction()
        {
            if (this.transactionContext == TransactionContext.WithLock)
            {
                this.sqlTransaction = this.sqlConnection.BeginTransaction(IsolationLevel.ReadCommitted);

                return;
            }

            this.sqlTransaction = this.sqlConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        private void ExecuteVarsSet()
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            var ctx = HttpContext.Current;
            var contextQuery = new ContextQuery
                               {
                                   Command = "zzCRO_VarsSet",
                                   ContextQueryType = ContextQueryType.StoredProcedure
                               };

            contextQuery.Parameters.Add("transacao", DBNull.Value);

            if (ctx.Request.UserHostName != null)
            {
                contextQuery.Parameters.Add("estacao", ctx.Request.UserHostName);
            }
            else
            {
                contextQuery.Parameters.Add("estacao", DBNull.Value);
            }

            if (ctx.Items["__AuditingInfo"] is AuditingInfo)
            {
                var info = (AuditingInfo)HttpContext.Current.Items["__AuditingInfo"];

                contextQuery.Parameters.Add("usuario", info.Usuario);
                contextQuery.Parameters.Add("pagina", info.Pagina);
                contextQuery.Parameters.Add("audita", info.Audita ? "S" : "N");
            }
            else if (Thread.CurrentPrincipal != null)
            {
                contextQuery.Parameters.Add("usuario", Thread.CurrentPrincipal.Identity.Name);
                contextQuery.Parameters.Add("pagina", DBNull.Value);
                contextQuery.Parameters.Add("audita", DBNull.Value);
            }
            else
            {
                contextQuery.Parameters.Add("usuario", DBNull.Value);
                contextQuery.Parameters.Add("pagina", DBNull.Value);
                contextQuery.Parameters.Add("audita", DBNull.Value);
            }

            this.ApplyModifications(contextQuery);
        }

        private SqlCommand GenerateCommand(ContextQuery contextQuery)
        {
            if (this.sqlTransaction == null)
            {
                this.BeginTransaction();
            }

            var sqlCommand = new SqlCommand(contextQuery.Command, this.sqlConnection, this.sqlTransaction)
                             {
                                 CommandTimeout = this.CommandTimeout,
                                 CommandType = contextQuery.ContextQueryType == ContextQueryType.StoredProcedure ? CommandType.StoredProcedure : CommandType.Text
                             };

            foreach (var parameter in contextQuery.Parameters)
            {
                sqlCommand.Parameters.Add(parameter.ToSqlParameter());
            }

            return sqlCommand;
        }
    }
}