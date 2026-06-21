using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Threading;
using System.Web;
using Techne.Auditing;
using Techne.Library;

namespace Techne.Data
{
    public enum Rdbms
    {
        Unknown,
        SQLServer,
        Oracle
    }

    public class TConnection : IDisposable
    {
        // Indica se vai criar automaticamente uma transação via BeginTransaction internamente
        // Deve ser alterado para false se a TConnection for ser utilizada sob TransactionScope do .NET 2.0.
        public readonly bool USE_TRANSACTIONS = true;

        internal const string CronosPrefix = "zzCRO_";

        internal const string SpVarsGet = CronosPrefix + "VarsGet";

        internal const string SpVarsSet = CronosPrefix + "VarsSet";

        internal const string TabErros = CronosPrefix + "Erros";

        internal const string TabVars = CronosPrefix + "Vars";

        private readonly DbConnection cn;

        private bool audita = true;

        private ArrayList changedTables;

        private int level;

        private bool rollback;

        private DbTransaction tr;

        private string transacao = string.Empty;

        public TConnection(string connectionString)
            : this(connectionString, string.Empty)
        {
        }

        public TConnection(string connectionString, string transacao)
        {
            this.cn = TFactory.Instance.CreateConnection();
            this.cn.ConnectionString = connectionString;
            this.transacao = transacao;
        }

        public TConnection(DbConnection dbConnection)
        {
            this.cn = dbConnection;
            this.transacao = string.Empty;
            if (dbConnection.State == ConnectionState.Open)
            {
                this.level = 1;
            }

            this.rollback = false;
            this.changedTables = new ArrayList();
        }

        public bool Audita
        {
            get
            {
                return this.audita;
            }

            set
            {
                this.audita = value;

                if (this.level > 0)
                {
                    using (var cm = CreateDbCommand("CRO_SetAudita", this.cn, this.tr))
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        AddDbParameter(cm, "audita", this.audita ? "S" : "N");
                        cm.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        ///   Lista de tabelas que foram alteradas através desta conexão.
        /// </summary>
        public IList ChangedTables
        {
            get
            {
                if (this.WillRollback)
                {
                    return new ArrayList();
                }

                return ArrayList.ReadOnly(this.changedTables);
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.cn.ConnectionString;
            }

            set
            {
                if (this.level > 0)
                {
                    throw new ConnectionOpenException(this, "ConnectionString", true);
                }

                this.cn.ConnectionString = value;
            }
        }

        public int ErrorCount
        {
            get
            {
                using (var cm = CreateDbCommand("GetErrorsCount", this.cn, this.tr))
                {
                    cm.CommandType = CommandType.StoredProcedure;
                    AddDbParameter(cm,
                                   "count", System.Data.DbType.Int32, 0, ParameterDirection.Output,
                                   false, 0, 0, null, DataRowVersion.Current, 0
                        );

                    try
                    {
                        cm.ExecuteNonQuery();
                    }
                    catch (Exception exc)
                    {
                        if (exc is OleDbException && this.Rdbms == Rdbms.SQLServer)
                        {
                            foreach (OleDbError error in ((OleDbException)exc).Errors)
                            {
                                if (error.NativeError == 2812)
                                {
                                    throw new Exception("A stored procedure " + cm.CommandText + " não existe no banco " + GetConnectInfo(this.ConnectionString));
                                }
                            }
                        }

                        throw new Exception("Erro ao executar a stored procedure " + cm.CommandText + " no banco " + GetConnectInfo(this.ConnectionString) + ": " + exc.Message);
                    }

                    return (int)cm.Parameters["count"].Value;
                }
            }
        }

        public Rdbms Rdbms
        {
            get
            {
                if (this.cn is OleDbConnection)
                {
                    return DbLib.GetRdbms(this.ConnectionString);
                }

                return Rdbms.Unknown;
            }
        }

        public ConnectionState State
        {
            get
            {
                return this.cn.State;
            }
        }

        public string Transacao
        {
            get
            {
                return this.transacao;
            }

            set
            {
                this.transacao = value == null ? string.Empty : value.Trim();

                if (this.level > 0)
                {
                    using (var cm = CreateDbCommand("CRO_SetTransacao", this.cn, this.tr))
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        AddDbParameter(cm, "transacao", this.transacao);
                        cm.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        ///   Indica se o método Rollback() foi chamado para a conexão.
        /// </summary>
        public bool WillRollback
        {
            get
            {
                return this.rollback;
            }
        }

        internal int Level
        {
            get
            {
                return this.level;
            }
        }

        internal DbTransaction Transaction
        {
            get
            {
                return this.tr;
            }

            set
            {
                this.tr = value;
            }
        }

        /// <summary>
        ///   Troca o conteúdo de "password" para "&lt;não mostrado&gt;".
        ///   Utilizado para mostrar connection strings para o usuário.
        /// </summary>
        public static string ConnectionStringHidePassword(string connectionString)
        {
            var connectionInfo = StrLib.StrToDictionary(connectionString, ';', "=", true);
            if (connectionInfo.Contains("password"))
            {
                connectionInfo["password"] = "<não mostrado>";
            }

            return StrLib.DictionaryToStr(connectionInfo, ';', "=");
        }

        public static ErrorList GetErrors(DbConnection connection, Rdbms rdbms)
        {
            return GetErrors(connection, null, rdbms);
        }

        public static ErrorList GetErrors(DbTransaction transaction, Rdbms rdbms)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("A transação informada é inválida", "transaction");
            }

            if (transaction.Connection == null)
            {
                throw new InvalidOperationException("A transação informada contém uma conexão inválida");
            }

            return GetErrors(transaction.Connection, transaction, rdbms);
        }

        /// <summary>
        ///   Abre uma conexão como readonly.
        /// </summary>
        public virtual void Open()
        {
            if (this.level == 0)
            {
                try
                {
                    this.cn.Open();

                    try
                    {
                        this.SetVars();

                        try
                        {
                            ClearErrors(this.cn, null, this.Rdbms);
                            this.BeginTrans();
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    catch
                    {
                        this.cn.Close();
                        throw;
                    }
                }
                catch (Exception exc)
                {
                    throw new ApplicationException("Erro ao tentar abrir a conexão (connection string: " + ConnectionStringHidePassword(this.cn.ConnectionString) + "): " + exc.Message, exc);
                }

                this.rollback = false;
                this.changedTables = new ArrayList();
            }

            this.level++;
        }

        public void ClearErrors()
        {
            if (this.level == 0)
            {
                throw new ConnectionCloseException(this, "ClearErrors()", false);
            }

            ClearErrors(this.cn, this.tr, this.Rdbms);
        }

        public void Close()
        {
            if (this.level == 0)
            {
                throw new InvalidOperationException("A conexão não está aberta");
            }

            this.level--;
            if (this.level == 0)
            {
                if (this.rollback)
                {
                    this.RollbackTrans();
                    this.rollback = false; // Teoricamente não precisa, pois é inicializado no Open()
                }
                else
                {
                    this.CommitTrans();
                }

                this.tr = null;

                ClearErrors(this.cn, null, this.Rdbms);

                this.cn.Close();
                this.changedTables = null;
            }
        }

        public TCommand CreateCommand()
        {
            return TCommand.ConvertFromDb(this.cn.CreateCommand(), this);
        }

        public TCommand CreateCommand(string commandText, params DbObject[] parametersValues)
        {
            var cm = this.CreateCommand();
            cm.CommandText = commandText;
            cm.Parameters.AddValues(parametersValues);
            return cm;
        }

        public TDataReader CreateDataReader(string sqlSelect, params DbObject[] parametersValues)
        {
            if (this.State != ConnectionState.Open)
            {
                throw new ArgumentException("A conexão já deve estar aberta.");
            }

            return this.CreateCommand(sqlSelect, parametersValues).ExecuteReader();
        }

        public ErrorList GetErrors()
        {
            if (this.level == 0)
            {
                throw new ConnectionCloseException(this, "GetErrors()", false);
            }

            return GetErrors(this.cn, this.tr, this.Rdbms);
        }

        public void Rollback()
        {
            if (this.level == 0)
            {
                throw new ConnectionCloseException(this, "Rollback()", false);
            }

            this.rollback = true;
        }

        internal static DbParameter AddDbParameter(DbCommand cmd, string parameterName, object value)
        {
            var par = cmd.CreateParameter();
            par.ParameterName = parameterName;
            par.Value = value;
            cmd.Parameters.Add(par);
            return par;
        }

        internal static DbParameter AddDbParameter(DbCommand cmd, string parameterName, System.Data.DbType dbType,
                                                   int size, ParameterDirection direction, bool isNullable,
                                                   byte precision, byte scale, string sourceColumn,
                                                   DataRowVersion sourceVersion, object value)
        {
            var par = cmd.CreateParameter();
            par.ParameterName = parameterName;
            par.DbType = dbType;
            par.Size = size;
            par.Direction = direction;
            if (par is IDbDataParameter)
            {
                ((IDbDataParameter)par).Precision = precision;
                ((IDbDataParameter)par).Scale = scale;
            }

            par.SourceColumn = sourceColumn;
            par.SourceVersion = sourceVersion;
            par.Value = value;
            par.Value = value;
            cmd.Parameters.Add(par);
            return par;
        }

        internal static DbConnection ConvertToDb(TConnection connection)
        {
            if (connection == null)
            {
                return null;
            }

            return connection.cn;
        }

        internal static DbCommand CreateDbCommand(string commandText, DbConnection conn, DbTransaction trans)
        {
            var cmd = TFactory.Instance.CreateCommand();
            cmd.CommandText = commandText;
            cmd.Connection = conn;
            cmd.Transaction = trans;
            return cmd;
        }

        internal void AddChangedTable(string table)
        {
            // Aqui não precisa se preocupar com casing porque os nomes
            // das tabelas sempre são gerados pelo Cronos.
            if (!this.changedTables.Contains(table))
            {
                this.changedTables.Add(table);
            }
        }

        internal void SetError(string message)
        {
            var p = message.LastIndexOf('|');
            if (p < 0)
            {
                this.SetError(message, null);
            }
            else
            {
                this.SetError(message.Substring(0, p), message.Substring(p + 1));
            }
        }

        internal void SetError(string message, string field)
        {
            if (this.level == 0)
            {
                throw new ConnectionCloseException(this, "SetError()", false);
            }

            if (message == null)
            {
                throw new ArgumentNullException();
            }

            using (var cm = CreateDbCommand("SetErro", this.cn, this.tr))
            {
                cm.CommandType = CommandType.StoredProcedure;
                AddDbParameter(cm,
                               "@p_Erro", System.Data.DbType.String, 1024, ParameterDirection.Input,
                               false, 0, 0, null, DataRowVersion.Current, message
                    );
                AddDbParameter(cm,
                               "@p_Campo", System.Data.DbType.String, 50, ParameterDirection.Input,
                               true, 0, 0, null, DataRowVersion.Current, field == null ? Convert.DBNull : field
                    );

                try
                {
                    cm.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    if (exc is OleDbException && this.Rdbms == Rdbms.SQLServer)
                    {
                        foreach (OleDbError error in ((OleDbException)exc).Errors)
                        {
                            if (error.NativeError == 2812)
                            {
                                throw new Exception("A stored procedure " + cm.CommandText + " não existe no banco " + GetConnectInfo(this.ConnectionString));
                            }
                        }
                    }

                    throw new Exception("Erro ao executar a stored procedure " + cm.CommandText + " no banco " + GetConnectInfo(this.ConnectionString) + ": " + exc.Message);
                }
            }
        }

        private static void ClearErrors(DbConnection connection, DbTransaction transaction, Rdbms rdbms)
        {
            using (var cm = CreateDbCommand("RemErros", connection, transaction))
            {
                cm.CommandType = CommandType.StoredProcedure;

                try
                {
                    cm.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    if (exc is OleDbException && rdbms == Rdbms.SQLServer)
                    {
                        foreach (OleDbError error in ((OleDbException)exc).Errors)
                        {
                            if (error.NativeError == 2812)
                            {
                                throw new Exception("A stored procedure " + cm.CommandText + " não existe no banco " + GetConnectInfo(connection.ConnectionString));
                            }
                        }
                    }

                    throw new Exception("Erro ao executar a stored procedure " + cm.CommandText + " no banco " + GetConnectInfo(connection.ConnectionString) + ": " + exc.Message);
                }
            }
        }

        private static string FormatConnectInfo(string userName, string dbAlias)
        {
            return "Oracle (" +
                   (userName != null && userName.Length > 0 ? userName + "@" : string.Empty) +
                   dbAlias +
                   ")";
        }

        private static string FormatConnectInfo(string userName, string server, string database)
        {
            return "SQLServer (" +
                   (userName != null && userName.Length > 0 ? userName + "@" : string.Empty) +
                   server + ":" + database +
                   ")";
        }

        private static string GetConnectInfo(string connectionString)
        {
            var connectionInfo = StrLib.StrToDictionary(connectionString, ';', "=", true);
            var provider = (string)connectionInfo["provider"];

            if (provider != null)
            {
                if (provider.ToLower().StartsWith("msdaora.1"))
                {
                    return FormatConnectInfo((string)connectionInfo["user id"], (string)connectionInfo["data source"]);
                }

                if (provider.ToLower().StartsWith("sqloledb.1"))
                {
                    var server = connectionInfo.Contains("data source") ? (string)connectionInfo["data source"] : (string)connectionInfo["server"];
                    var database = connectionInfo.Contains("initial catalog") ? (string)connectionInfo["initial catalog"] : (string)connectionInfo["database"];
                    return FormatConnectInfo((string)connectionInfo["user id"], server, database);
                }

                throw new NotImplementedException("Provider inválido: " + provider);
            }

            return FormatConnectInfo((string)connectionInfo["uid"], (string)connectionInfo["data source"]);
        }

        private static ErrorList GetErrors(DbConnection connection, DbTransaction transaction, Rdbms rdbms)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("A conexão informada é inválida", "connection");
            }

            using (var cm = CreateDbCommand("GetErros", connection, transaction))
            {
                cm.CommandType = CommandType.StoredProcedure;
                AddDbParameter(cm,
                               "@p_Erro", System.Data.DbType.String, 1024, ParameterDirection.Output,
                               false, 0, 0, null, DataRowVersion.Current, Convert.DBNull
                    );

                try
                {
                    cm.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    if (exc is OleDbException && rdbms == Rdbms.SQLServer)
                    {
                        foreach (OleDbError error in ((OleDbException)exc).Errors)
                        {
                            if (error.NativeError == 2812)
                            {
                                throw new Exception("A stored procedure " + cm.CommandText + " não existe no banco " + GetConnectInfo(connection.ConnectionString));
                            }
                        }
                    }

                    throw new Exception("Erro ao executar a stored procedure " + cm.CommandText + " no banco " + GetConnectInfo(connection.ConnectionString) + ": " + exc.Message);
                }

                var erro = cm.Parameters["@p_Erro"].Value;
                if (erro is DBNull)
                {
                    return new ErrorList();
                }
                
                return new ErrorList((string)cm.Parameters["@p_Erro"].Value);
            }
        }

        private void BeginTrans()
        {
            if (!this.USE_TRANSACTIONS)
            {
                return;
            }

            this.tr = this.cn.BeginTransaction();

            if (this.Rdbms == Rdbms.SQLServer)
            {
                using (var cm = CreateDbCommand("BEGIN TRANSACTION", this.cn, this.tr))
                {
                    cm.ExecuteNonQuery();
                }
            }
        }

        private void CommitTrans()
        {
            if (!this.USE_TRANSACTIONS)
            {
                return;
            }

            this.tr.Commit();

            if (this.Rdbms == Rdbms.SQLServer)
            {
                using (var cm = CreateDbCommand("WHILE @@TRANCOUNT > 0 COMMIT TRANSACTION", this.cn, this.tr))
                {
                    cm.ExecuteNonQuery();
                }
            }
        }

        void IDisposable.Dispose()
        {
            ((IDisposable)this.cn).Dispose();
        }

        private void RollbackTrans()
        {
            if (!this.USE_TRANSACTIONS)
            {
                return;
            }

            this.tr.Rollback();

            if (this.Rdbms == Rdbms.SQLServer)
            {
                using (var cm = CreateDbCommand("WHILE @@TRANCOUNT > 0 ROLLBACK TRANSACTION", this.cn, this.tr))
                {
                    cm.ExecuteNonQuery();
                }
            }
        }

        private void SetVars()
        {
            // Utiliza OleDbCommand ao invés de TCommand porque o TCommand, ao interceptar exceptions,
            // tentava dar rollback na conexão. O SetVars() é chamado justamente na abertura da conexão,
            // o que causava um erro dizendo que Rollback() não pode ser chamado com a conexão fechada,
            // erro este que acabava se sobrepondo ao ocorrido no SetVars() (a mensagem era perdida).
            using (var cmd = this.cn.CreateCommand())
            {
                cmd.CommandText = SpVarsSet;
                cmd.CommandType = CommandType.StoredProcedure;

                // Não precisa setar cmd.Transaction porque este método só é
                // chamado de Open() antes da transação ter iniciado.
                var parEstacao = AddDbParameter(cmd, "estacao", DBNull.Value);
                var parUsuario = AddDbParameter(cmd, "usuario", DBNull.Value);
                var parTransacao = AddDbParameter(cmd, "transacao", DBNull.Value);
                var parAudita = AddDbParameter(cmd, "audita", DBNull.Value);
                var parPagina = AddDbParameter(cmd, "pagina", DBNull.Value);

                parAudita.Value = this.audita ? "S" : "N";
                if (HttpContext.Current != null)
                {
                    try
                    {
                        if (HttpContext.Current.Request != null 
                            && HttpContext.Current.Request.UserHostName != null)
                        {
                            parEstacao.Value = HttpContext.Current.Request.UserHostName;
                        }
                        else
                        {
                            parEstacao.Value = DBNull.Value;
                        }
                    }
                    catch
                    {
                        parEstacao.Value = DBNull.Value;
                    }

                    if (HttpContext.Current.Items["__AuditingInfo"] is AuditingInfo)
                    {
                        var info = (AuditingInfo)HttpContext.Current.Items["__AuditingInfo"];

                        parUsuario.Value = info.Usuario;
                        parPagina.Value = info.Pagina;
                        parAudita.Value = info.Audita ? "S" : "N";
                    }
                    else if (Thread.CurrentPrincipal != null &&
                             Thread.CurrentPrincipal.Identity != null)
                    {
                        parUsuario.Value = Thread.CurrentPrincipal.Identity.Name;
                    }
                }
                else
                {
                    var usuario = string.Empty;

                    if (Thread.CurrentPrincipal != null 
                        && Thread.CurrentPrincipal.Identity != null)
                    {
                        usuario = Thread.CurrentPrincipal.Identity.Name;
                    }

                    parUsuario.Value = usuario == null || usuario.Trim().Length == 0 ? DBNull.Value : (object)usuario;
                }

                parTransacao.Value = this.transacao.Length == 0 ? DBNull.Value : (object)this.transacao;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    if (exc is OleDbException && this.Rdbms == Rdbms.SQLServer)
                    {
                        foreach (OleDbError error in ((OleDbException)exc).Errors)
                        {
                            if (error.NativeError == 2812)
                            {
                                throw new Exception("A stored procedure " + cmd.CommandText + " não existe no banco " + GetConnectInfo(this.ConnectionString));
                            }
                        }
                    }

                    throw new Exception("Erro ao executar a stored procedure " + cmd.CommandText + " no banco " + GetConnectInfo(this.ConnectionString) + ": " + exc.Message);
                }
            }
        }
    }

    public class TConnectionWritable : TConnection
    {
        private readonly TPermission permission;

        private bool openWritable;

        public TConnectionWritable(string connectionString)
            : this(connectionString, string.Empty)
        {
        }

        public TConnectionWritable(string connectionString, string transacao)
            : base(connectionString, transacao)
        {
            if (TPermission.ThreadPermission != null)
            {
                this.permission = TPermission.ThreadPermission;
            }
            else
            {
                this.permission = new TPermission(string.Empty, string.Empty, true, true, true, true);
            }
        }

        public TConnectionWritable(TPermission permission, string connectionString)
            : this(permission, connectionString, string.Empty)
        {
        }

        public TConnectionWritable(TPermission permission, string connectionString, string transacao)
            : base(connectionString, transacao)
        {
            if (permission == null)
            {
                throw new ArgumentNullException();
            }

            this.permission = permission;
        }

        public TConnectionWritable(DbConnection dbConnection)
            : base(dbConnection)
        {
            if (TPermission.ThreadPermission != null)
            {
                this.permission = TPermission.ThreadPermission;
            }
            else
            {
                this.permission = new TPermission(string.Empty, string.Empty, true, true, true, true);
            }

            this.openWritable = true;
        }

        public TPermission Permission
        {
            get
            {
                return this.permission;
            }
        }

        public bool ReadOnly
        {
            get
            {
                if (this.permission == null || this.permission.ReadOnly)
                {
                    return false;
                }
                
                return !this.openWritable;
            }
        }

        public override void Open()
        {
            this.Open(false);
        }

        public void Open(bool writable)
        {
            if (writable && this.permission != null && this.permission.ReadOnly)
            {
                throw new InvalidOperationException("Não é permitida a abertura de conexão writable de acordo com seu perfil de acesso.");
            }

            if (writable && this.Level > 0 && !this.openWritable)
            {
                throw new InvalidOperationException("A conexão já foi aberta como readonly. Não é permitido chamar Open() como writable.");
            }

            base.Open();

            if (this.Level == 1)
            {
                this.openWritable = writable;
            }
        }

        internal static TConnectionWritable CreateWithoutPermission(string connectionString)
        {
            return new TConnectionWritable(
                new TPermission(string.Empty, string.Empty, true, true, true, true),
                connectionString
                );
        }
    }

    public class ConnectionOpenException : InvalidOperationException
    {
        internal ConnectionOpenException(TConnection connection)
            : this(connection, string.Empty, false)
        {
        }

        internal ConnectionOpenException(TConnection connection,
                                         string memberName, bool isProperty)
            : base(
                (memberName.Length == 0 ? "Operação inválida"
                     : (isProperty ? "A propriedade" : "O método") +
                       " TConnection." + memberName + " não pode ser " +
                       (isProperty ? "alterada" : "executado")) +
                " com a conexão aberta."
                )
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException();
            }

            connection.Rollback();
        }
    }

    public class ConnectionCloseException : InvalidOperationException
    {
        internal ConnectionCloseException(TConnection connection)
            : this(connection, string.Empty, false)
        {
        }

        internal ConnectionCloseException(TConnection connection,
                                          string memberName, bool isProperty)
            : base(
                (memberName.Length == 0 ? "Operação inválida"
                     : (isProperty ? "A propriedade" : "O método") +
                       " TConnection." + memberName + " não pode ser " +
                       (isProperty ? "alterada" : "executado")) +
                " com a conexão fechada."
                )
        {
        }
    }
}