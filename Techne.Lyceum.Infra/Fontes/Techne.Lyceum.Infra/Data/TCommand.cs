using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Threading;

namespace Techne.Data
{
    public class TCommand : IDisposable
    {
        private readonly DbCommand cm;

        private TConnection cn;

        public TCommand()
        {
            this.cm = TFactory.Instance.CreateCommand();
        }

        public TCommand(string cmdText) : this()
        {
            this.CommandText = cmdText;
        }

        public TCommand(string cmdText, TConnection connection) : this(cmdText)
        {
            this.Connection = connection;
        }

        private TCommand(DbCommand command)
        {
            this.cm = command;
        }

        public string CommandText
        {
            get
            {
                return this.cm.CommandText;
            }

            set
            {
                this.cm.CommandText = value;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return this.cm.CommandTimeout;
            }

            set
            {
                this.cm.CommandTimeout = value;
            }
        }

        public CommandType CommandType
        {
            get
            {
                return this.cm.CommandType;
            }

            set
            {
                this.cm.CommandType = value;
            }
        }

        public TConnection Connection
        {
            get
            {
                return this.cn;
            }

            set
            {
                this.cn = value;
                this.cm.Connection = TConnection.ConvertToDb(this.cn);
                if (this.cn.Transaction != null)
                {
                    this.cm.Transaction = this.cn.Transaction;
                }
            }
        }

        public TParameterCollection Parameters
        {
            get
            {
                return TParameterCollection.ConvertFromDb(this.cm.Parameters);
            }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get
            {
                return this.cm.UpdatedRowSource;
            }

            set
            {
                this.cm.UpdatedRowSource = value;
            }
        }

        internal DbTransaction Transaction
        {
            get
            {
                return this.cn.Transaction;
            }

            set
            {
                this.cn.Transaction = value;
                this.cm.Transaction = value;
            }
        }

        public static int ExecuteNonQuery(TConnectionWritable connection, 
                                          string sql, params DbObject[] paramsValues)
        {
            if (connection.ReadOnly)
            {
                throw new InvalidOperationException("ExecuteNonQuery() não é permitido através de conexões readonly.");
            }

            using (var cm = connection.CreateCommand())
            {
                cm.CommandText = sql;
                cm.Parameters.AddValues(paramsValues);

                connection.Open(true);
                try
                {
                    return cm.ExecuteNonQuery();
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        ///   Devolve o valor correspondente à primeira coluna.
        ///   Se nenhum registro for encontrado, devolve null.
        ///   Se mais de um registro for encontrado, devolve o valor contido no primeiro registro.
        ///   Neste caso, portanto, é importante que a cláusula ORDER BY seja especificada.
        /// </summary>
        public static DbObject ExecuteScalar(TConnection connection, 
                                             string sql, params DbObject[] paramsValues)
        {
            using (var cm = connection.CreateCommand())
            {
                cm.CommandText = sql;
                cm.Parameters.AddValues(paramsValues);

                connection.Open();
                try
                {
                    return cm.ExecuteScalar();
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        public TParameter CreateParameter()
        {
            return TParameter.ConvertFromDb(this.cm.CreateParameter());
        }

        public void Dispose()
        {
            this.cm.Dispose();
        }

        public int ExecuteNonQuery()
        {
            if (!(this.cn is TConnectionWritable) || ((TConnectionWritable)this.cn).ReadOnly)
            {
                throw new InvalidOperationException("ExecuteNonQuery() não é permitido através de conexões readonly.");
            }

            this.SetCommand();

            // Enquanto não encontramos a solução das mensagens provenientes do banco contidas dentro de
            // OleDbException.Errors quanto a cultura da thread é pt-BR, estamos salvando-a para restaurá-la depois.
            var saveCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            int result;
            try
            {
                result = this.cm.ExecuteNonQuery();
            }
            catch
            {
                this.cn.Rollback();
                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saveCulture;
            }

            return result;
        }

        public TDataReader ExecuteReader()
        {
            this.SetCommand();

            // Enquanto não encontramos a solução das mensagens provenientes do banco contidas dentro de
            // OleDbException.Errors quanto a cultura da thread é pt-BR, estamos salvando-a para restaurá-la depois.
            var saveCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            TDataReader result;
            try
            {
                result = TDataReader.ConvertFromDb(this.cm.ExecuteReader());
            }
            catch
            {
                this.cn.Rollback();
                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saveCulture;
            }

            return result;
        }

        /// <summary>
        ///   Devolve o valor correspondente à primeira coluna.
        ///   Se nenhum registro for encontrado, devolve null.
        ///   Se mais de um registro for encontrado, devolve o valor contido no primeiro registro.
        ///   Neste caso, portanto, é importante que a cláusula ORDER BY seja especificada.
        /// </summary>
        public DbObject ExecuteScalar()
        {
            this.SetCommand();

            // Enquanto não encontramos a solução das mensagens provenientes do banco contidas dentro de
            // OleDbException.Errors quanto a cultura da thread é pt-BR, estamos salvando-a para restaurá-la depois.
            var saveCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            object result;
            try
            {
                result = this.cm.ExecuteScalar();
            }
            catch
            {
                this.cn.Rollback();
                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saveCulture;
            }

            return DbObject.ToDbObject(result);
        }

        // O parâmetro TConnection é a informação complementar ao parâmetro OleDbCommand para
        // a criação de um TCommand completo.
        internal static TCommand ConvertFromDb(DbCommand command, TConnection connection)
        {
            var newCommand = new TCommand(command);
            newCommand.Connection = connection;
            return newCommand;
        }

        internal static DbCommand ConvertToDb(TCommand command)
        {
            if (command == null)
            {
                return null;
            }

            return command.cm;
        }

        private void SetCommand()
        {
            this.cm.Connection = TConnection.ConvertToDb(this.cn);
            this.cm.Transaction = this.Transaction;
        }
    }
}