using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using Techne.Library;

namespace Techne.Data
{
    [
        DesignTimeVisible(true), 
        ToolboxItem(true), 
    ]
    public abstract class TDataTable : DataTable
    {
        private bool historyEnabled;

        private TDataRowCollection rows;

        private bool valid;

        [
            Browsable(false), 
        ]
        public abstract string ConnectionString { get; }

        [
            Browsable(false), 
        ]
        public abstract bool ReadOnly { get; }

        [
            Browsable(false), 
        ]
        public virtual string MainTableName
        {
            get
            {
                return string.Empty;
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public new DataColumnCollection Columns
        {
            get
            {
                return base.Columns;
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public new ConstraintCollection Constraints
        {
            get
            {
                return base.Constraints;
            }
        }

        [
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public new TDataSet DataSet
        {
            // Cast não é utilizado aqui porque quando o método TDataTable.Get() é chamado sem que
            // ele esteja dentro de um TDataSet, o TDataTable é inserido dentro de um DataSet simples.
            get
            {
                return base.DataSet as TDataSet;
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public bool EnforceConstraints
        {
            get
            {
                return base.DataSet == null ? true : base.DataSet.EnforceConstraints;
            }

            set
            {
                if (value)
                {
                    if (base.DataSet != null)
                    {
// 1) Enforce && DataSet != null
                        // Remove do DataSet
                        base.DataSet.Tables.Remove(this);
                    }

                    // Obs: 2) Enforce && DataSet == null: nada faz, pois o default é Enforce=true qdo uma tabela não pertence a dataset.
                }
                else if (base.DataSet == null)
                {
                    // 3) !Enforce && DataSet == null
                    // Adiciona a um DataSet
                    var ds = new DataSet();
                    ds.EnforceConstraints = false;
                    ds.Tables.Add(this);
                }
                else
                {
// 4) !Enforce && DataSet != null
                    base.DataSet.EnforceConstraints = false;
                }
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public new DataColumn[] PrimaryKey
        {
            get
            {
                return base.PrimaryKey;
            }
        }

        [
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public new TDataRowCollection Rows
        {
            get
            {
                return this.rows;
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public new string TableName
        {
            get
            {
                return base.TableName;
            }
        }

        [
            Browsable(false), 
        ]
        internal bool HistoryEnabled
        {
            get
            {
                return this.historyEnabled;
            }
        }

        [Browsable(false)]
        protected internal virtual string Name
        {
            get
            {
                return this.TableName;
            }
        }

        protected internal DataSet BaseDataSet
        {
            get
            {
                return base.DataSet;
            }
        }

        protected internal bool ValidInstance
        {
            get
            {
                return this.valid;
            }
        }

        protected DataRowCollection BaseRows
        {
            get
            {
                return base.Rows;
            }
        }

        public abstract int Get(TConnection connection, 
                                string[] columns, 
                                string where, DbObject[] whereValues, 
                                string order, 
                                int startRecord, 
                                int maxRecords, 
                                bool createPrimaryKey, 
                                bool getDeletedRecords, 
                                bool rowCountOnly);

        public virtual void Put(TConnectionWritable connection, DataRow[] dataRows)
        {
            throw new InvalidOperationException("Esta operação não é permitida neste DataTable.");
        }

        /// <summary>
        ///   Verifica se uma coluna pertence à lista de colunas.
        ///   Ignora case.
        /// </summary>
        public bool ContainsColumn(string columnName)
        {
            if (columnName == null)
            {
                throw new ArgumentNullException();
            }

            foreach (DataColumn column in this.Columns)
            {
                if (string.Compare(columnName, column.ColumnName, true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public TConnection CreateConnection()
        {
            return new TConnection(this.ConnectionString);
        }

        public TConnection CreateConnection(string transacao)
        {
            return new TConnection(this.ConnectionString, transacao);
        }

        public TConnectionWritable CreateWritableConnection(TPermission permission)
        {
            return new TConnectionWritable(permission, this.ConnectionString);
        }

        public TConnectionWritable CreateWritableConnection(TPermission permission, string transacao)
        {
            return new TConnectionWritable(permission, this.ConnectionString, transacao);
        }

        public void EnablePrimaryKey(bool enable)
        {
            if (enable)
            {
                this.SetPrimaryKey();
            }
            else
            {
                this.SetPrimaryKey(null);

                // Seta a propriedade AllowDBNull de todas as colunas de uma tabela para true,
                // pois qdo se seta a propriedade PrimaryKey, ele seta AllowDBNull para false.
                foreach (DataColumn col in this.Columns)
                {
                    col.AllowDBNull = true;
                }
            }
        }

        /// <summary>
        ///   Encontra um TDataColumn na collection Columns a partir do nome original da coluna
        ///   no formato 'tabela.coluna'. A busca NÃO É case-sensitive.
        ///   Devolve null se a coluna informada não for encontrada.
        /// </summary>
        public TDataColumn FindColumn(string columnFullName)
        {
            foreach (DataColumn col in this.Columns)
            {
                var column = col as TDataColumn;
                if (column != null && string.Compare(column.FullCol, columnFullName, true) == 0)
                {
                    return column;
                }
            }

            return null;
        }

        /// <summary>
        ///   Utiliza false para createPrimaryKey e getDeletedRecords.
        /// </summary>
        public int Get(TConnection connection, string where, params object[] whereValues)
        {
            return this.Get(connection, new string[0], where, whereValues, string.Empty, 0, 0, false, false);
        }

        public int Get(TConnection connection, 
                       string[] columns, 
                       string where, object[] whereValues, 
                       string order, 
                       int startRecord, 
                       int maxRecords, 
                       bool createPrimaryKey, 
                       bool getDeletedRecords)
        {
            return Get(connection, columns, where, whereValues, order, startRecord, maxRecords, createPrimaryKey, getDeletedRecords, false);
        }

        public int Get(TConnection connection, 
                       string[] columns, 
                       string where, object[] whereValues, 
                       string order, 
                       int startRecord, 
                       int maxRecords, 
                       bool createPrimaryKey, 
                       bool getDeletedRecords, 
                       bool rowCountOnly)
        {
            DbObject[] dbo = null;
            if (whereValues != null)
            {
                dbo = new DbObject[whereValues.Length];
                for (var i = 0; i < whereValues.Length; i++)
                {
                    if (whereValues[i] == null)
                    {
                        dbo[i] = DBNull.Value;
                    }
                    else
                    {
                        dbo[i] = DbObject.ToDbObject(whereValues[i]);
                    }
                }
            }

            return this.Get(connection, columns, where, dbo, order, startRecord, maxRecords, createPrimaryKey, getDeletedRecords, rowCountOnly);
        }

        public bool HasChanges()
        {
            return DataLib.HasChanges(this);
        }

        public new TDataRow NewRow()
        {
            return (TDataRow)base.NewRow();
        }

        public void Put(TConnectionWritable connection)
        {
            this.Put(connection, null);
        }

        internal void RowUpdated(TDataRowUpdateEventArgs args)
        {
            var cm = args.Command as DataTableCommand;
            if (cm != null && // Para manter compatibilidade enquanto os datasets não são gerados.
                (cm.Operation == CommandOperation.Insert ||
                 cm.Operation == CommandOperation.Update ||
                 cm.Operation == CommandOperation.Delete ||
                 cm.Operation == CommandOperation.Undelete))
            {
                cm.Connection.AddChangedTable(this.MainTableName);
            }

            var erro = string.Empty;
            switch (cm.Operation)
            {
                case CommandOperation.Insert:
                    erro = this.PosInsert(args.Row, (TConnectionWritable)args.Command.Connection);
                    break;
                case CommandOperation.Update:
                    erro = this.PosUpdate(args.Row, (TConnectionWritable)args.Command.Connection);
                    break;
                case CommandOperation.Delete:
                    erro = this.PosDelete(args.Row, (TConnectionWritable)args.Command.Connection);
                    break;
            }

            if (erro == null || erro.Trim().Length == 0)
            {
                // Dispara entry-points POST.
                erro = this.PostOperationInternal(args);
            }

            if (erro != null && erro.Trim().Length > 0)
            {
                (new ErrorList(erro)).CopyToDataRow(args.Row);
            }
            else
            {
                // GetUpdateErrors() vem depois de PostOperationInternal() para
                // pegar erros setados via TConnection.SetError() nos entry-points.
                GetUpdateErrors(args);
            }
        }

        internal void RowUpdating(TDataRowUpdateEventArgs args)
        {
            // Dispara entry-points PRE.
            var erro = this.PreOperationInternal(args);

            if (erro == null || erro.Trim().Length == 0)
            {
                var cm = args.Command as DataTableCommand;
                switch (cm.Operation)
                {
                    case CommandOperation.Insert:
                        erro = this.PreInsert(args.Row, (TConnectionWritable)args.Command.Connection);
                        break;
                    case CommandOperation.Update:
                        erro = this.PreUpdate(args.Row, (TConnectionWritable)args.Command.Connection);
                        break;
                    case CommandOperation.Delete:
                        erro = this.PreDelete(args.Row, (TConnectionWritable)args.Command.Connection);
                        break;
                }
            }

            if (erro != null && erro.Trim().Length > 0)
            {
                (new ErrorList(erro)).CopyToDataRow(args.Row);

                // Não faz o Insert e nem dispara o Post.
                args.Status = UpdateStatus.SkipCurrentRow;
            }
        }

        protected internal void SetHistoryEnabled(bool enabled)
        {
            this.historyEnabled = enabled;
        }

        protected internal void SetPrimaryKey(DataColumn[] columns)
        {
            base.PrimaryKey = columns;
        }

        protected internal void SetTableName(string tableName)
        {
            base.TableName = tableName;
        }

        /// <summary>
        ///   Chama o método AcceptChanges() somente para os DataRow's cuja propriedade HasErrors seja false.
        ///   Devolve true se não tiver sido encontrado nenhum erro.
        /// </summary>
        protected static ErrorList AcceptChangesWithoutError(DataTable table)
        {
            return AcceptChangesWithoutError(table.Select());
        }

        /// <summary>
        ///   Chama o método AcceptChanges() somente para os DataRow's cuja propriedade HasErrors seja false.
        ///   Devolve true se não tiver sido encontrado nenhum erro.
        /// </summary>
        protected static ErrorList AcceptChangesWithoutError(DataRow[] rows)
        {
            var firstError = ErrorList.Empty;

            foreach (var row in rows)
            {
                if (row.HasErrors)
                {
                    if (firstError.Count == 0)
                    {
                        firstError = ErrorList.CreateFromDataRow(row);
                    }
                }
                else if (row.RowState != DataRowState.Detached)
                {
                    // RowState é Detached após efetivar operação no banco qdo RowState original é Deleted.
                    // Se for chamado quando RowState é Detached, dá RowNotInTableException.
                    row.AcceptChanges();
                }
            }

            return firstError;
        }

        protected static DataTableCommand CreateDataTableCommand(TConnection connection, CommandOperation operation)
        {
            return DataTableCommand.Create(connection, operation);
        }

        /// <summary>
        ///   Dado o tipo de um TDataTable gerado pelo Cronos, obtém o construtor desse tipo ou do tipo customizado, se existir.
        ///   Caso exista mais de uma customização, ocorrerá exception.
        /// </summary>
        protected static ConstructorInfo FindConstructor(Type originalType)
        {
            var type = FindCustomType(originalType);

            var ctor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new Type[0], null);
            if (ctor == null)
            {
                throw new InvalidOperationException("Construtor da classe " + type.FullName + " não existente.");
            }

            return ctor;
        }

        /// <summary>
        ///   Parseia a mensagem de erro retornada pela stored procedure SQL Server e associa
        ///   cada erro à coluna correspondente.
        ///   É utilizado como tratamento do evento RowUpdated do data adapter.
        /// </summary>
        protected static void GetUpdateErrors(TDataRowUpdateEventArgs e)
        {
            VarChar erro;
            if (e.Command.Parameters.IndexOf("erro") >= 0)
            {
                erro = (VarChar)e.Command.Parameters["erro"].Value;
            }
            else
            {
                erro = DBNull.Value;
            }

            string error;
            if (erro == null || erro.IsNull || erro.Length == 0)
            {
                if (e.Errors == null)
                {
                    error = string.Empty;
                }
                else if (e.Errors is OleDbException)
                {
                    var excOleDb = (OleDbException)e.Errors;
                    var listErrors = new ArrayList();
                    foreach (OleDbError errOleDb in excOleDb.Errors)
                    {
                        listErrors.Add(errOleDb.NativeError +
                                       (errOleDb.Message.Length == 0 ? string.Empty : ": " + errOleDb.Message));
                    }

                    error = e.Errors.Message + " " +
                            "(NativeError" + (listErrors.Count > 1 ? "s" : string.Empty) + ": " + StrLib.EnumerableToStr(listErrors, ", ") + ")" +
                            "|";
                }
                else
                {
                    var exc = e.Errors;
                    while (true)
                    {
                        if (exc.Message.Trim().Length > 0)
                        {
                            error = exc.Message + ", StackTrace: " + exc.StackTrace + "|";
                            break;
                        }
                        else if (exc.InnerException == null)
                        {
                            error = e.Errors.GetType().FullName;
                        }
                        else
                        {
                            exc = exc.InnerException;
                        }
                    }

                    ;
                }
            }
            else
            {
                error = erro;
            }

            // Remove o erro "global".
            e.Errors = null;

            // Muda o Status de ErrorsOccurred para Continue, para que o erro "global"
            // (mesmo tendo a mensagem sido removida acima) não se manifeste.
            e.Status = UpdateStatus.Continue;

            var errorList = TConnection.GetErrors(e.Command.Transaction, e.Command.Connection.Rdbms);
            if (error.Length > 0)
            {
                errorList.Add(error);
            }

            if (errorList.Count > 0)
            {
                // Em caso de Delete é necessário desfazer as alterações para que o registro possa
                // ser apresentado pelo record manager juntamente com os erros.
                // (registros Deleted não podem ser acessados, o que impediria o record manager de mostrá-lo)
                if (e.CommandOperation == CommandOperation.Delete)
                {
                    e.Row.RejectChanges();
                }

                if (errorList.Count > 0)
                {
                    errorList.CopyToDataRow(e.Row);
                }
            }
        }

        protected virtual string PosDelete(DataRow row, TConnectionWritable connection)
        {
            return string.Empty;
        }

        protected virtual string PosInsert(DataRow row, TConnectionWritable connection)
        {
            return string.Empty;
        }

        protected virtual string PosUpdate(DataRow row, TConnectionWritable connection)
        {
            return string.Empty;
        }

        protected virtual string PostOperationInternal(TDataRowUpdateEventArgs args)
        {
            return string.Empty;
        }

        protected virtual string PreDelete(DataRow row, TConnectionWritable connection)
        {
            return string.Empty;
        }

        protected virtual string PreInsert(DataRow row, TConnectionWritable connection)
        {
            return string.Empty;
        }

        protected virtual string PreOperationInternal(TDataRowUpdateEventArgs args)
        {
            return string.Empty;
        }

        protected virtual string PreUpdate(DataRow row, TConnectionWritable connection)
        {
            return string.Empty;
        }

        protected virtual void SetPrimaryKey()
        {
            // As tabelas tipadas deverão fazer override deste
            // método setando a primary key respectiva.
            base.PrimaryKey = null;
        }

        protected int Get(TConnection connection, 
                          string[] columns, 
                          string where, object[] whereValues, 
                          SelectArguments args)
        {
            var order = string.Empty;
            var startRecord = 0;
            var maxRecords = 0;
            var createPrimaryKey = true;
            var getDeletedRecords = false;
            var rowCountOnly = false;
            if (args != null)
            {
                startRecord = args.StartRowIndex;
                maxRecords = args.MaximumRows;
                order = args.SortExpression;
                rowCountOnly = args.RowCountOnly;
            }

            return this.Get(connection, columns, where, whereValues, order, startRecord, maxRecords, createPrimaryKey, getDeletedRecords, rowCountOnly);
        }

        protected void InitRows(TDataRowCollection rows)
        {
            this.rows = rows;
        }

        protected void Put(TDataAdapter da, TConnectionWritable cn, DataRow[] dataRows)
        {
            cn.Open(true);
            try
            {
                if (dataRows == null)
                {
                    da.Update(this);
                    if (this.HasErrors)
                    {
                        cn.Rollback();
                    }
                }
                else
                {
                    da.Update(dataRows);
                    foreach (var dataRow in dataRows)
                    {
                        if (dataRow.HasErrors)
                        {
                            cn.Rollback();
                            break;
                        }
                    }
                }
            }
            catch
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Rollback();
                }

                throw;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        protected void da_RowUpdated(object sender, TDataRowUpdateEventArgs args)
        {
            this.RowUpdated(args);
        }

        protected void da_RowUpdating(object sender, TDataRowUpdateEventArgs args)
        {
            this.RowUpdating(args);
        }

        private static Type FindCustomType(Type originalType)
        {
            // Cria uma lista de derivados do tipo informado.
            var list = new ArrayList();
            foreach (var assembly in BusinessAssemblyAttribute.BusinessAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (originalType.IsAssignableFrom(type) && type != originalType)
                    {
                        list.Add(type);
                    }
                }
            }

            if (list.Count > 1)
            {
                throw new InvalidOperationException("Foram encontrados " + list.Count + " tipos derivados de " + originalType.FullName + ".");
            }
            else if (list.Count == 0)
            {
                return originalType;
            }
            else
            {
                return (Type)list[0];
            }
        }
    }
}