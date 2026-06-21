using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.Threading;
using OleDb = System.Data.OleDb;

namespace Techne.Data
{
    public class TDataAdapter
    {
        private readonly DbDataAdapter da;

        private TCommand deleteCommand;

        private TCommand insertCommand;

        private TCommand updateCommand;

        public TDataAdapter() : this(TFactory.Instance.CreateDataAdapter())
        {
        }

        public TDataAdapter(TCommand selectCommand) : this()
        {
            this.SelectCommand = selectCommand;
        }

        public TDataAdapter(string selectCommandText, TConnection selectConnection) : this()
        {
            this.SelectCommand = new TCommand(selectCommandText, selectConnection);
        }

        private TDataAdapter(DbDataAdapter dataAdapter)
        {
            this.da = dataAdapter;
            if (this.da is OleDb.OleDbDataAdapter)
            {
                ((OleDb.OleDbDataAdapter)this.da).RowUpdating += this.da_RowUpdating;
                ((OleDb.OleDbDataAdapter)this.da).RowUpdated += this.da_RowUpdated;
            }
        }

        public event TDataRowUpdateEventHandler RowUpdated;

        public event TDataRowUpdateEventHandler RowUpdating;

        public bool ContinueUpdateOnError
        {
            get
            {
                return this.da.ContinueUpdateOnError;
            }

            set
            {
                this.da.ContinueUpdateOnError = value;
            }
        }

        public TCommand DeleteCommand
        {
            get
            {
                return this.deleteCommand;
            }

            set
            {
                this.deleteCommand = value;
            }
        }

        public TCommand InsertCommand
        {
            get
            {
                return this.insertCommand;
            }

            set
            {
                this.insertCommand = value;
            }
        }

        public TCommand SelectCommand { get; set; }

        public DataTableMappingCollection TableMappings
        {
            get
            {
                return this.da.TableMappings;
            }
        }

        public TCommand UpdateCommand
        {
            get
            {
                return this.updateCommand;
            }

            set
            {
                this.updateCommand = value;
            }
        }

        public int Fill(DataSet dataSet, string srcTable)
        {
            this.da.SelectCommand = TCommand.ConvertToDb(this.SelectCommand);
            this.da.SelectCommand.Transaction = this.SelectCommand.Connection.Transaction;

            // Enquanto não encontramos a solução das mensagens provenientes do banco contidas dentro de
            // OleDbException.Errors quanto a cultura da thread é pt-BR, estamos salvando-a para restaurá-la depois.
            var saveCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            int result;
            try
            {
                result = this.da.Fill(dataSet, srcTable);
            }
            catch
            {
                if (this.SelectCommand.Connection.State == ConnectionState.Open)
                {
                    this.SelectCommand.Connection.Rollback();
                }

                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saveCulture;
            }

            return result;
        }

        public int Fill(DataSet dataSet)
        {
            this.da.SelectCommand = TCommand.ConvertToDb(this.SelectCommand);
            this.da.SelectCommand.Transaction = this.SelectCommand.Connection.Transaction;

            // Enquanto não encontramos a solução das mensagens provenientes do banco contidas dentro de
            // OleDbException.Errors quanto a cultura da thread é pt-BR, estamos salvando-a para restaurá-la depois.
            var saveCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            int result;
            try
            {
                result = this.da.Fill(dataSet);
            }
            catch
            {
                if (this.SelectCommand.Connection.State == ConnectionState.Open)
                {
                    this.SelectCommand.Connection.Rollback();
                }

                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saveCulture;
            }

            return result;
        }

        public int Fill(DataTable dataTable)
        {
            this.da.SelectCommand = TCommand.ConvertToDb(this.SelectCommand);
            this.da.SelectCommand.Transaction = this.SelectCommand.Connection.Transaction;

            // Enquanto não encontramos a solução das mensagens provenientes do banco contidas dentro de
            // OleDbException.Errors quanto a cultura da thread é pt-BR, estamos salvando-a para restaurá-la depois.
            var saveCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            int result;
            try
            {
                result = this.da.Fill(dataTable);
            }
            catch
            {
                if (this.SelectCommand.Connection.State == ConnectionState.Open)
                {
                    this.SelectCommand.Connection.Rollback();
                }

                // O "throw new Exception()" foi trocado pelo "throw" simples porque TSearchBase faz um catch
                // e ele verifica se a exception é um OleDbException. Fazendo throw new Exception aqui, esse
                // catch não funcionaria adequadamente.
                // throw new Exception(exc.Message, exc);
                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saveCulture;
            }

            return result;
        }

        public int Fill(DataSet dataSet, int startRecord, int maxRecords, string srcTable)
        {
            this.da.SelectCommand = TCommand.ConvertToDb(this.SelectCommand);
            this.da.SelectCommand.Transaction = this.SelectCommand.Connection.Transaction;

            // Enquanto não encontramos a solução das mensagens provenientes do banco contidas dentro de
            // OleDbException.Errors quanto a cultura da thread é pt-BR, estamos salvando-a para restaurá-la depois.
            var saveCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            int result;
            try
            {
                result = this.da.Fill(dataSet, startRecord, maxRecords, srcTable);
            }
            catch
            {
                if (this.SelectCommand.Connection.State == ConnectionState.Open)
                {
                    this.SelectCommand.Connection.Rollback();
                }

                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saveCulture;
            }

            return result;
        }

        public int Update(DataSet dataSet, string srcTable)
        {
            this.PrepareUpdate();

            // Enquanto não encontramos a solução das mensagens provenientes do banco contidas dentro de
            // OleDbException.Errors quanto a cultura da thread é pt-BR, estamos salvando-a para restaurá-la depois.
            var saveCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            int result;
            try
            {
                result = this.da.Update(dataSet, srcTable);
            }
            catch
            {
                // Normalmente a conexão é a mesma para os 3 TCommand's, mas não se pode assumir isso.
                if (this.InsertCommand.Connection.State == ConnectionState.Open)
                {
                    this.InsertCommand.Connection.Rollback();
                }

                if (this.UpdateCommand.Connection.State == ConnectionState.Open)
                {
                    this.UpdateCommand.Connection.Rollback();
                }

                if (this.DeleteCommand.Connection.State == ConnectionState.Open)
                {
                    this.DeleteCommand.Connection.Rollback();
                }

                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saveCulture;
            }

            return result;
        }

        public int Update(DataRow[] dataRows)
        {
            this.PrepareUpdate();

            // Enquanto não encontramos a solução das mensagens provenientes do banco contidas dentro de
            // OleDbException.Errors quanto a cultura da thread é pt-BR, estamos salvando-a para restaurá-la depois.
            var saveCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            int result;
            try
            {
                result = this.da.Update(dataRows);
            }
            catch
            {
                // Normalmente a conexão é a mesma para os 3 TCommand's, mas não se pode assumir isso.
                if (this.InsertCommand.Connection.State == ConnectionState.Open)
                {
                    this.InsertCommand.Connection.Rollback();
                }

                if (this.UpdateCommand.Connection.State == ConnectionState.Open)
                {
                    this.UpdateCommand.Connection.Rollback();
                }

                if (this.DeleteCommand.Connection.State == ConnectionState.Open)
                {
                    this.DeleteCommand.Connection.Rollback();
                }

                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saveCulture;
            }

            return result;
        }

        public int Update(DataTable dataTable)
        {
            this.PrepareUpdate();

            // Enquanto não encontramos a solução das mensagens provenientes do banco contidas dentro de
            // OleDbException.Errors quanto a cultura da thread é pt-BR, estamos salvando-a para restaurá-la depois.
            var saveCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            int result;
            try
            {
                result = this.da.Update(dataTable);
            }
            catch
            {
                // Normalmente a conexão é a mesma para os 3 TCommand's, mas não se pode assumir isso.
                if (this.InsertCommand.Connection.State == ConnectionState.Open)
                {
                    this.InsertCommand.Connection.Rollback();
                }

                if (this.UpdateCommand.Connection.State == ConnectionState.Open)
                {
                    this.UpdateCommand.Connection.Rollback();
                }

                if (this.DeleteCommand.Connection.State == ConnectionState.Open)
                {
                    this.DeleteCommand.Connection.Rollback();
                }

                throw;
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = saveCulture;
            }

            return result;
        }

        private void FireRowUpdated(RowUpdatedEventArgs e)
        {
            TCommand command;
            if (e.Command == TCommand.ConvertToDb(this.insertCommand))
            {
                command = this.insertCommand;
            }
            else if (e.Command == TCommand.ConvertToDb(this.updateCommand))
            {
                command = this.updateCommand;
            }
            else if (e.Command == TCommand.ConvertToDb(this.deleteCommand))
            {
                command = this.deleteCommand;
            }
            else
            {
                throw new InvalidOperationException();
            }

            if (command.Transaction != null && command.Transaction.Connection == null)
            {
                throw new InvalidOperationException(
                    "Ocorreu um erro inesperado.\r\n\r\n" +
                    "CommandText:\r\n" + command.CommandText +
                    (e.Status == UpdateStatus.ErrorsOccurred
                         ? "\r\n\r\n" + e.Errors.Message
                         : string.Empty)
                    );
            }

            var targs = new TDataRowUpdateEventArgs(command, e.Row, e.Errors, e.Status, e.StatementType);
            if (this.RowUpdated != null)
            {
                this.RowUpdated(this, targs);

                e.Errors = targs.Errors;
                e.Status = targs.Status;
            }
        }

        private void PrepareUpdate()
        {
            if (((!(this.InsertCommand.Connection is TConnectionWritable) || ((TConnectionWritable)this.InsertCommand.Connection).ReadOnly) || (!(this.UpdateCommand.Connection is TConnectionWritable) || ((TConnectionWritable)this.UpdateCommand.Connection).ReadOnly)) || (!(this.DeleteCommand.Connection is TConnectionWritable) || ((TConnectionWritable)this.DeleteCommand.Connection).ReadOnly))
            {
                throw new InvalidOperationException("TDataAdapter.Update() não é permitido sobre conexões readonly.");
            }

            this.da.InsertCommand = TCommand.ConvertToDb(this.InsertCommand);
            if (this.da.InsertCommand != null)
            {
                this.da.InsertCommand.Transaction = this.InsertCommand.Connection.Transaction;
            }

            this.da.UpdateCommand = TCommand.ConvertToDb(this.UpdateCommand);
            if (this.da.UpdateCommand != null)
            {
                this.da.UpdateCommand.Transaction = this.UpdateCommand.Connection.Transaction;
            }

            this.da.DeleteCommand = TCommand.ConvertToDb(this.DeleteCommand);
            if (this.da.DeleteCommand != null)
            {
                this.da.DeleteCommand.Transaction = this.DeleteCommand.Connection.Transaction;
            }
        }

        private void da_OleDbRowUpdated(object sender, OleDbRowUpdatedEventArgs e)
        {
            this.da_RowUpdated(sender, e);
        }

        private void da_OleDbRowUpdating(object sender, OleDbRowUpdatingEventArgs args)
        {
            this.da_RowUpdating(sender, args);
        }

        private void da_RowUpdated(object sender, RowUpdatedEventArgs e)
        {
            if (!e.Row.HasErrors)
            {
                this.FireRowUpdated(e);
            }
        }

        private void da_RowUpdating(object sender, RowUpdatingEventArgs args)
        {
            // Verifica se algum parâmetro é DateTime e se a data é superior a 1/1/1753 (limitação do SQL Server)
            foreach (DbParameter parameter in args.Command.Parameters)
            {
                var erro = TParameter.GetDateTimeError(parameter.Value, false);
                if (erro != string.Empty)
                {
                    args.Row.SetColumnError(parameter.SourceColumn, erro);
                }
            }

            if (args.Row.HasErrors)
            {
                args.Status = UpdateStatus.ErrorsOccurred;

// A mensagem da exception abaixo será atribuída à propriedade RowError do DataRow.
                args.Errors = new ArgumentException("Foi informado pelo menos um valor inválido.");
                return;
            }

            TCommand command;
            if (args.Command == TCommand.ConvertToDb(this.insertCommand))
            {
                command = this.insertCommand;
            }
            else if (args.Command == TCommand.ConvertToDb(this.updateCommand))
            {
                command = this.updateCommand;
            }
            else if (args.Command == TCommand.ConvertToDb(this.deleteCommand))
            {
                command = this.deleteCommand;
            }
            else
            {
                throw new InvalidOperationException();
            }

            if (command.Transaction != null && command.Transaction.Connection == null)
            {
                throw new InvalidOperationException(
                    "Ocorreu um erro inesperado" +
                    (args.Status == UpdateStatus.ErrorsOccurred
                         ? "\r\n\r\n" + args.Errors.Message
                         : string.Empty)
                    );
            }

            var targs = new TDataRowUpdateEventArgs(command, args.Row, args.Errors, args.Status, args.StatementType);
            if (this.RowUpdating != null)
            {
                this.RowUpdating(this, targs);

                // Os parâmetros do tipo InputOutput são aqueles que representam novos valores (em insert e update).
                // Os demais parâmetros (primary key e outros) são apenas de Input ou de Output.
                foreach (TParameter parameter in command.Parameters)
                {
                    if (parameter.Direction == ParameterDirection.InputOutput)
                    {
                        parameter.Value = ((TDataRow)args.Row)[parameter.ParameterName];
                    }
                }

                args.Errors = targs.Errors;
                args.Status = targs.Status;
            }
        }
    }
}