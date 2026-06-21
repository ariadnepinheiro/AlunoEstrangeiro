namespace Techne.Data
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.OleDb;

    public class QueryTable : SimpleTable
    {
        readonly TDataAdapter ad;

        public QueryTable(string tableName, string sql)
            : this(sql)
        {
            this.TableName = tableName;
        }

        public QueryTable(string sql)
        {
            var cm = new TCommand
                     {
                         CommandText = sql
                     };

            this.ad = new TDataAdapter(cm);
        }

        public int CommandTimeout
        {
            get
            {
                if (this.ad != null && this.ad.SelectCommand != null)
                {
                    return this.ad.SelectCommand.CommandTimeout;
                }

                return 0;
            }

            set
            {
                if (this.ad != null && this.ad.SelectCommand != null)
                {
                    this.ad.SelectCommand.CommandTimeout = value;
                }
            }
        }

        public string Sql
        {
            get
            {
                return this.ad.SelectCommand.CommandText;
            }
        }

        public SimpleRow[] Query(TConnection cn, params object[] parametersValues)
        {
            var dbo = DbObject.ToDbObjectArray(parametersValues);
            return this.Query(cn, dbo);
        }

        public SimpleRow[] Query(TConnection cn, DbObject[] parametersValues)
        {
            return this.Query(cn, false, parametersValues);
        }

        private SimpleRow[] Query(TConnection cn, bool keepCurrentRows, params DbObject[] parametersValues)
        {
            if (cn == null)
            {
                throw new ArgumentNullException();
            }

            if (!keepCurrentRows)
            {
                this.Clear();
            }

            var oldrows = new ArrayList();
            foreach (DataRow row in this.Rows)
            {
                oldrows.Add(row);
            }

            var cm = this.ad.SelectCommand;
            cm.Connection = cn;

            var parameters = cm.Parameters;
            parameters.Clear();
            parameters.AddValues(parametersValues);

            try
            {
                cn.Open();
                this.ad.Fill(this);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            if (keepCurrentRows)
            {
                var newrows = new ArrayList();
                foreach (SimpleRow row in this.Rows)
                {
                    if (!oldrows.Contains(row))
                    {
                        newrows.Add(row);
                    }
                }

                return (SimpleRow[])newrows.ToArray(typeof(SimpleRow));
            }

            return this.Select();
        }

        private SimpleRow[] Query(TConnection cn, int startRow, int maxRecords, params DbObject[] parametersValues)
        {
            if (cn == null)
            {
                throw new ArgumentNullException();
            }

            this.Clear();

            var cm = this.ad.SelectCommand;
            cm.Connection = cn;

            var parameters = cm.Parameters;
            parameters.Clear();
            parameters.AddValues(parametersValues);

            var ds = new DataSet();
            ds.Tables.Add(this);
            try
            {
                cn.Open();
                try
                {
                    this.ad.Fill(ds, startRow, maxRecords, this.TableName);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }
            finally
            {
                ds.Tables.Remove(this);
            }

            return this.Select();
        }

        private SimpleRow[] QueryPage(TConnection cn, PagingArguments arguments, params object[] parametersValues)
        {
            int startRow = 0, maxRecords = 0;
            if (arguments != null)
            {
                startRow = arguments.StartRowIndex;
                maxRecords = arguments.MaximumRows;
                if (maxRecords < 0)
                {
                    maxRecords = 0;
                }
            }

            return this.Query(cn, startRow, maxRecords, DbObject.ToDbObjectArray(parametersValues));
        }
    }

    public class SimpleTable : DataTable
    {
        private readonly SimpleRowCollection rows;

        protected SimpleTable()
        {
            this.rows = new SimpleRowCollection(base.Rows);
        }

        public SimpleTable(DataTable dataTable)
        {
            this.rows = new SimpleRowCollection(dataTable.Rows);
        }

        public new SimpleRowCollection Rows
        {
            get
            {
                return this.rows;
            }
        }

        public new SimpleRow NewRow()
        {
            return (SimpleRow)base.NewRow();
        }

        public new SimpleRow[] Select()
        {
            return this.Select(string.Empty);
        }

        public new SimpleRow[] Select(string filterExpression)
        {
            return this.Select(filterExpression, string.Empty);
        }

        public new SimpleRow[] Select(string filterExpression, string sort)
        {
            return this.Select(filterExpression, sort, DataViewRowState.CurrentRows);
        }

        public new SimpleRow[] Select(string filterExpression, string sort, DataViewRowState recordStates)
        {
            if (this.Rows.Count == 0)
            {
                return new SimpleRow[0];
            }

            var rows = base.Select(filterExpression, sort, recordStates);
            var result = new SimpleRow[rows.Length];
            rows.CopyTo(result, 0);
            return result;
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new SimpleRow(builder);
        }

        private bool HasChanges()
        {
            return DataLib.HasChanges(this);
        }
    }

    public class SimpleRow : DataRow
    {
        internal SimpleRow(DataRowBuilder rb)
            : base(rb)
        {
        }

        public new DbObject[] ItemArray
        {
            get
            {
                return DbObject.ToDbObjectArray(base.ItemArray);
            }
        }

        public new SimpleTable Table
        {
            get
            {
                return (SimpleTable)base.Table;
            }
        }

        public new DbObject this[int index]
        {
            get
            {
                return DbObject.ToDbObject(base[index]);
            }

            set
            {
                var newValue = value.ToObject();
                if (!base[index].Equals(newValue))
                {
                    base[index] = newValue;
                }
            }
        }

        public new DbObject this[string columnName, DataRowVersion version]
        {
            get
            {
                return DbObject.ToDbObject(base[columnName, version]);
            }
        }

        public new DbObject this[string columnName]
        {
            get
            {
                return DbObject.ToDbObject(base[columnName]);
            }

            set
            {
                var newValue = value.ToObject();
                if (!base[columnName].Equals(newValue))
                {
                    base[columnName] = newValue;
                }
            }
        }

        /// <summary>
        ///   Realiza uma consulta no banco e devolve somente o primeiro registro.
        ///   Se o resultado da consulta for vazio, devolve null.
        ///   A conexão não precisa estar aberta.
        /// </summary>
        public static SimpleRow QueryFirstRow(TConnection cn, string sql, params DbObject[] parametersValues)
        {
            var rows = (new QueryTable(sql)).Query(cn, parametersValues);
            if (rows.Length != 0)
            {
                return rows[0];
            }

            return null;
        }

        public override string ToString()
        {
            return DataLib.DataRowToString(this);
        }
    }

    public class SimpleRowCollection : IEnumerable
    {
        private readonly DataRowCollection rows;

        internal SimpleRowCollection(DataRowCollection rows)
        {
            this.rows = rows;
        }

        public int Count
        {
            get
            {
                return this.rows.Count;
            }
        }

        public SimpleRow this[int index]
        {
            get
            {
                return (SimpleRow)this.rows[index];
            }
        }

        public void Add(SimpleRow row)
        {
            this.rows.Add(row);
        }

        public IEnumerator GetEnumerator()
        {
            return this.rows.GetEnumerator();
        }

        public void InsertAt(SimpleRow row, int pos)
        {
            this.rows.InsertAt(row, pos);
        }

        public void Remove(SimpleRow row)
        {
            this.rows.Remove(row);
        }

        public void RemoveAt(int index)
        {
            this.rows.RemoveAt(index);
        }
    }

    internal class QueryTableLib
    {
        private QueryTableLib()
        {
        }

        public static int Update(TConnectionWritable connection, QueryTable table, string tableName)
        {
            return Update(connection, table, tableName, new string[0]);
        }

        public static int Update(TConnectionWritable connection, QueryTable table, string tableName, string[] excludeColumns)
        {
            return Update(connection, table, tableName, DataRowState.Added | DataRowState.Modified | DataRowState.Deleted, excludeColumns);
        }

        public static int Update(TConnectionWritable connection, QueryTable table, string tableName, DataRowState rowstate)
        {
            return Update(connection, table, tableName, rowstate, new string[0]);
        }

        public static int Update(TConnectionWritable connection, QueryTable table, string tableName, DataRowState rowstate, string[] excludeColumns)
        {
            if (table.PrimaryKey == null || table.PrimaryKey.Length == 0)
            {
                throw new ArgumentException("A propriedade PrimaryKey não está definida na tabela fornecida (" + tableName + ").");
            }

            if ((rowstate & DataRowState.Detached) != 0 || (rowstate & DataRowState.Unchanged) != 0)
            {
                throw new ArgumentException("Detached e Unchanged não são valores permitidos para rowstate.");
            }

            DataColumn[] columns;
            {
                var list = new ArrayList();
                foreach (DataColumn col in table.Columns)
                {
                    if (StrLib.IndexOfInsensitive(col.ColumnName, excludeColumns) < 0)
                    {
                        list.Add(col);
                    }
                }

                columns = (DataColumn[])list.ToArray(typeof(DataColumn));
            }

            string[] columnnames;
            {
                columnnames = (string[])TechLib.EnumerableItemProperty(columns, "ColumnName", true);
                for (var i = 0; i < columnnames.Length; i++)
                {
                    columnnames[i] = columnnames[i].ToLower();
                }
            }

            string wherepk;
            {
                var pkassign = new string[table.PrimaryKey.Length];
                for (var i = 0; i < pkassign.Length; i++)
                {
                    pkassign[i] = table.PrimaryKey[i].ColumnName.ToLower() + " = ?";
                }

                wherepk = StrLib.EnumerableToStr(pkassign, " AND ");
            }

            var ad = new TDataAdapter();

            ad.InsertCommand = new TCommand(
                "INSERT INTO " + tableName.ToLower() + "(" + StrLib.EnumerableToStr(columnnames, ", ") + ") " +
                "VALUES(" + StrLib.EnumerableToStr(new string('?', columns.Length).ToCharArray(), ", ") + ")",
                connection);
            foreach (var col in columns)
            {
                ad.InsertCommand.Parameters.Add(new TParameter(
                                                    col.ColumnName.ToLower(), ToDbType(col.DataType), 0, ParameterDirection.Input,
                                                    true, 0, 0, col.ColumnName, DataRowVersion.Current, DBNull.Value
                                                    ));
            }

            ad.UpdateCommand = new TCommand(
                "UPDATE " + tableName.ToLower() + " " +
                "SET " + StrLib.EnumerableToStr(columnnames, ", ", string.Empty, " = ?") + " " +
                "WHERE " + wherepk,
                connection);
            foreach (var col in columns)
            {
                ad.UpdateCommand.Parameters.Add(new TParameter(
                                                    col.ColumnName.ToLower(), ToDbType(col.DataType), 0, ParameterDirection.Input,
                                                    true, 0, 0, col.ColumnName, DataRowVersion.Current, DBNull.Value
                                                    ));
            }

            foreach (var col in table.PrimaryKey)
            {
                ad.UpdateCommand.Parameters.Add(new TParameter(
                                                    "pk" + StrLib.ToProper(col.ColumnName), ToDbType(col.DataType), 0, ParameterDirection.Input,
                                                    true, 0, 0, col.ColumnName, DataRowVersion.Original, DBNull.Value
                                                    ));
            }

            ad.DeleteCommand = new TCommand(
                "DELETE FROM " + tableName.ToLower() + " " +
                "WHERE " + wherepk,
                connection);
            foreach (var col in table.PrimaryKey)
            {
                ad.DeleteCommand.Parameters.Add(new TParameter(
                                                    "pk" + StrLib.ToProper(col.ColumnName), ToDbType(col.DataType), 0, ParameterDirection.Input,
                                                    true, 0, 0, col.ColumnName, DataRowVersion.Original, DBNull.Value
                                                    ));
            }

            if ((rowstate & DataRowState.Added) != 0 &&
                (rowstate & DataRowState.Modified) != 0 &&
                (rowstate & DataRowState.Deleted) != 0)
            {
                return ad.Update(table);
            }
            else
            {
                var list = new ArrayList();

                if ((rowstate & DataRowState.Added) != 0)
                {
                    list.AddRange(table.Select(string.Empty, string.Empty, DataViewRowState.Added));
                }

                if ((rowstate & DataRowState.Modified) != 0)
                {
                    list.AddRange(table.Select(string.Empty, string.Empty, DataViewRowState.ModifiedCurrent));
                }

                if ((rowstate & DataRowState.Deleted) != 0)
                {
                    list.AddRange(table.Select(string.Empty, string.Empty, DataViewRowState.Deleted));
                }

                return ad.Update((DataRow[])list.ToArray(typeof(DataRow)));
            }
        }

        private static OleDbType ToDbType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }

            if (type == typeof(string))
            {
                return OleDbType.VarChar;
            }
            else if (type == typeof(decimal))
            {
                return OleDbType.Numeric;
            }
            else if (type == typeof(DateTime))
            {
                return OleDbType.Date;
            }
            else
            {
                throw new NotImplementedException("ToDbType(): '" + type.FullName + "' não implementado.");
            }
        }
    }
}