using System;
using System.Collections;
using System.Data;
using System.Reflection;
using Techne.Library;

namespace Techne.Data
{
    public class TDataRow : DataRow
    {
        protected TDataRow(DataRowBuilder rb) : base(rb)
        {
        }

        /// <summary>
        ///   Se a tabela principal do DataTable for historificada, indica se o registro foi removido.
        /// </summary>
        public bool Deleted
        {
            get
            {
                // Se RowState=Detached, o DataRow acaba de ser criado com
                // DataTable.NewRow(). A coluna "Hist Status", portanto, não existe,
                // mas sabemos que false deverá ser devolvido nesta situação.
                if (this.TableHistoryEnabled && this.RowState != DataRowState.Detached && this.RowState != DataRowState.Added)
                {
                    return this["Hist Status"] == "R";
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///   Array contendo os valores das colunas contidas na primary key do TDataTable ao qual este TDataRow pertence.
        /// </summary>
        public DbObject[] PrimaryKeyValues
        {
            get
            {
                var pkCols = this.Table.PrimaryKey;
                var pk = new DbObject[pkCols.Length];

                for (var i = 0; i < pk.Length; i++)
                {
                    pk[i] = DbObject.ToDbObject(this[pkCols[i]]);
                }

                return pk;
            }
        }

        public new TDataTable Table
        {
            get
            {
                return (TDataTable)base.Table;
            }
        }

        /// <summary>
        ///   Se a tabela for historificada, informa se o registro já foi
        ///   alterado (ou removido) pelo menos uma vez.
        /// </summary>
        public bool Updated
        {
            get
            {
                if (!this.TableHistoryEnabled)
                {
                    throw new InvalidOperationException("Operação não permitida em registros de tabelas não historificadas.");
                }

                return this["Hist Status"] != "C";
            }
        }

        private bool TableHistoryEnabled
        {
            get
            {
                var table = this.Table;
                return table != null && table.HistoryEnabled;
            }
        }

        public new DbObject this[string columnName]
        {
            get
            {
                return DbObject.ToDbObject(base[columnName, this.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Default]);
            }

            set
            {
                if (this.Deleted)
                {
                    throw new InvalidOperationException("Registro removido. Alterações não são permitidas.");
                }

                var newValue = value.ToObject();
                if (!base[columnName].Equals(newValue))
                {
                    base[columnName] = newValue;
                }
            }
        }

        public new DbObject this[int index]
        {
            get
            {
                return DbObject.ToDbObject(base[index, this.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Default]);
            }

            set
            {
                if (this.Deleted)
                {
                    throw new InvalidOperationException("Registro removido. Alterações não são permitidas.");
                }

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

        /// <summary>
        ///   Reinsere um registro removido. Se Deleted = false, o método causa exception.
        ///   A operação é realizada no banco de dados, e a efetivação depende da conexão passada.
        /// </summary>
        /// <param name = "connection">
        ///   Conexão sob a qual a operação será realizada no banco de dados.
        ///   Não pode ser null. Para não ter que informar a conexão, utilize outro overload de Undelete().
        /// </param>
        public virtual bool Undelete(TConnectionWritable connection)
        {
            return false;
        }

        public override string ToString()
        {
            return this.ToString("{0} ({1})");
        }

        public void Put(TConnectionWritable connection)
        {
            if (this.RowState == DataRowState.Detached)
            {
                throw new InvalidOperationException("Operação inválida quando o registro está Detached.");
            }

            this.Table.Put(connection, new DataRow[] { this });
        }

        /// <param name = "formatText">{0}: nome da tabela; {1}: chave</param>
        public string ToString(string formatText)
        {
            var list = new ArrayList();

            foreach (TDataColumn column in this.Table.PrimaryKey)
            {
                var columnName = column.GetName();
                if (columnName.Trim().Length == 0)
                {
                    columnName = column.ColumnName;
                }

                list.Add(StrLib.ToProper(columnName) + " " + this[column]);
            }

            var tableName = this.Table.Name;
            if (tableName.Trim().Length == 0)
            {
                tableName = this.Table.TableName;
            }

            return string.Format(
                formatText, 
                StrLib.ToProper(tableName), 
                StrLib.EnumerableToStr(list, ", ")
                );
        }

        public bool Undelete()
        {
            return this.Undelete(null);
        }

        internal static bool GetHistoryValues(TConnection connection, 
                                              decimal histId, string campo, 
                                              out string valorAnt, out string descAnt, 
                                              out string valorPos, out string descPos)
        {
            valorAnt = string.Empty;
            descAnt = string.Empty;
            valorPos = string.Empty;
            descPos = string.Empty;

            var cm = new TCommand(
                "SELECT ValorAnt, DescAnt, ValorPos, DescPos " +
                "FROM " + HistoryLib.TabHistDet + " " +
                "WHERE HistId = ? AND " +
                "Campo = ?", 
                connection
                );
            cm.Parameters.Add("HistId", histId);
            cm.Parameters.Add("Campo", campo);

            connection.Open();
            try
            {
                var rd = cm.ExecuteReader();
                try
                {
                    if (rd.Read())
                    {
                        valorAnt = rd["ValorAnt"].ToString();
                        descAnt = rd["DescAnt"].ToString();
                        valorPos = rd["ValorPos"].ToString();
                        descPos = rd["DescPos"].ToString();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                finally
                {
                    rd.Close();
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        ///   Cria um DataTable contendo registros de operações sobre o TDataRow corrente.
        ///   Devolve null se a tabela não possui histórico habilitado.
        /// </summary>
        /// <param name = "connection">Conexão através da qual a consulta ao histórico será feita. Pode ser null.</param>
        internal DataTable GetHistory(TConnection connection)
        {
            if (!this.TableHistoryEnabled)
            {
                return null;
            }

            var hist = new QueryTable(
                "SELECT h.Id, t.Usuario, h.Stamp, h.Status " +
                "FROM " + HistoryLib.TabHist + " h " +
                "INNER JOIN " + HistoryLib.TabHistTrans + " t ON t.Id = h.TransId " +
                "WHERE IdTab = ?"
                );

            hist.Query(
                connection != null ? connection : this.CreateConnection(), 
                this["Hist IdTab"]
                );

            hist.TableName = "Hist";

            return hist;
        }

        internal string[] GetHistoryFields(TConnection connection)
        {
            var list = new ArrayList();
            var idTab = (Number)this["Hist IdTab"];

            if (!idTab.IsNull)
            {
                var cn = connection != null ? connection : this.CreateConnection();

                cn.Open();
                try
                {
                    var rd = cn.CreateDataReader(
                        "SELECT DISTINCT c.Campo " +
                        "FROM " + HistoryLib.TabHistDet + " c " +
                        "INNER JOIN " + HistoryLib.TabHist + " p ON p.Id = c.HistId " +
                        "WHERE p.IdTab = ?", 
                        new DbObject[] { idTab }
                        );

                    try
                    {
                        while (rd.Read())
                        {
                            list.Add((string)rd["Campo"]);
                        }
                    }
                    finally
                    {
                        if (!rd.IsClosed)
                        {
                            rd.Close();
                        }
                    }
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }

            return (string[])list.ToArray(typeof (string));
        }

        /// <param name = "cn">A conexão deve estar aberta para o chamador poder obter as mensagens de erro.</param>
        protected internal static bool Delete(TConnectionWritable cn, TDataTable tab, 
                                              string[] pkColumns, object[] pkValues)
        {
            

            if (cn == null)
            {
                throw new ArgumentNullException();
            }

            cn.Open(true);
            try
            {
                var errorCount = cn.ErrorCount;

                

                if (errorCount > 0)
                {
                    return false;
                }

                var row = Query(cn, tab, pkColumns, pkValues);
                if (cn.ErrorCount > 0)
                {
                    return false;
                }

                if (row == null)
                {
                    cn.SetError("O registro não existe.");
                    return false;
                }

                row.Delete();
                row.Put(cn);
                if (row.HasErrors)
                {
                    // Copia os erros do DataRow para TConnection.
                    ErrorList.CreateFromDataRow(row).CopyToConnection(cn);
                    return false;
                }

                return true;

                #region cn.Close();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            #endregion
        }

        /// <summary>
        ///   Devolve null se e somente se cn.GetErrors() contiver novo erro.
        /// </summary>
        protected internal static TDataRow Insert(TConnectionWritable cn, TDataTable tab, 
                                                  string[] columns1, object[] values1, 
                                                  string columnList2, object[] values2)
        {
            if (values2 == null)
            {
                throw new ArgumentNullException();
            }

            

            if (cn == null)
            {
                throw new ArgumentNullException();
            }

            cn.Open(true);
            try
            {
                var errorCount = cn.ErrorCount;

                

                if (errorCount > 0)
                {
                    return null;
                }

                var columns2 = columnList2 == null ? new string[0] : columnList2.Split(',');

                var columns = new string[(columns1 == null ? 0 : columns1.Length) + columns2.Length];
                var values = new DbObject[(values1 == null ? 0 : values1.Length) + values2.Length];
                if (columns.Length != values.Length)
                {
                    throw new ArgumentException("O número de valores deve ser igual ao número de colunas.");
                }

                DbObject[] aux = null;
                if (columns1 != null)
                {
                    columns1.CopyTo(columns, 0);
                }

                if (values1 != null)
                {
                    aux = DbObject.ToDbObjectArray(values1);
                    aux.CopyTo(values, 0);
                }

                columns2.CopyTo(columns, columns1.Length);
                if (values2 != null)
                {
                    aux = DbObject.ToDbObjectArray(values2);
                    aux.CopyTo(values, values1.Length);
                }

                var row = tab.NewRow();
                for (var i = 0; i < columns.Length; i++)
                {
                    row[columns[i].Trim()] = values[i];
                }

                var hasPk = tab.PrimaryKey != null;

                // Desabilita a primary key para que colunas com valores gerados pelo banco
                // (colunas autonumber ou cujos valores são gerados por entry-points) não tenham
                // problemas no momento que o DataRow for adicionado ao DataTable.
                tab.EnablePrimaryKey(false);

                tab.Rows.Add(row);
                row.Put(cn);
                if (row.HasErrors)
                {
                    // Copia os erros do DataRow para TConnection.
                    ErrorList.CreateFromDataRow(row).CopyToConnection(cn);

                    // Tem que remover o row com problema da tabela.
                    tab.Rows.Remove(row);

                    // Se deu problema, deve devolver null.
                    row = null;
                }

                // Tem que reabilitar a primary key se ela existia ao entrar no método.
                if (hasPk)
                {
                    tab.EnablePrimaryKey(true);
                }

                return row;

                #region cn.Close();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            #endregion
        }
        

        /// <summary>
        ///   Obtém um registro utilizando as colunas e os valores respectivos para montar a cláusula where
        ///   (normalmente utiliza-se as colunas da chave da tabela).
        ///   Dá exception caso o where não restrinja suficientemente e o select traga mais de um registro.
        ///   Devolve null se nenhum registro for encontrado. Neste caso a mensagem de erro correspondente
        ///   será adicionada à conexão.
        /// </summary>
        protected internal static TDataRow Query(TConnection cn, TDataTable tab, 
                                                 string[] pkColumns, object[] pkValues)
        {
            return Query(cn, tab, false, pkColumns, pkValues);
        }

        protected internal static TDataRow Undelete(TConnectionWritable cn, TDataTable tab, 
                                                    string[] pkColumns, object[] pkValues)
        {
            

            if (cn == null)
            {
                throw new ArgumentNullException();
            }

            cn.Open(true);
            try
            {
                var errorCount = cn.ErrorCount;

                

                if (errorCount > 0)
                {
                    return null;
                }

                // Se tabela não possui historificação, devolve null.
                if (!tab.HistoryEnabled)
                {
                    return null;
                }

                var createCommand = GetCreateCommandMethod(tab.GetType());
                var cm = (TCommand)createCommand.Invoke(null, new object[] { cn, CommandOperation.Undelete });

                for (var i = 0; i < pkColumns.Length; i++)
                {
                    cm.Parameters["pk" + StrLib.ToProper(pkColumns[i])].Value = DbObject.ToDbObject(pkValues[i]);
                }

                // Dispara evento Pre
                var rowPre = Query(cn, tab, true, pkColumns, pkValues);
                var argsPre = new TDataRowUpdateEventArgs(cm, rowPre, null, UpdateStatus.Continue, CommandOperation.Undelete);
                tab.RowUpdating(argsPre);
                if (rowPre.RowState != DataRowState.Unchanged)
                {
                    throw new InvalidOperationException("O registro não pode ser alterado no entrypoint PreUndelete.");
                }

                #region if(rowPre.HasErrors) [seta erro na conexão]; return null;

                var errorsPre = ErrorList.CreateFromDataRow(rowPre);
                if (errorsPre.Count > 0)
                {
                    errorsPre.CopyToConnection(cn);
                }
                else if (argsPre.Status != UpdateStatus.Continue)
                {
                    cn.SetError("Erro ao reinserir o registro.");
                }

                if (cn.ErrorCount > errorCount)
                {
                    return null;
                }

                #endregion

                cm.ExecuteNonQuery();

                var erro = (VarChar)cm.Parameters["erro"].Value;
                if (!erro.IsNull)
                {
                    cn.SetError(erro);
                    return null;
                }

                // Dispara evento Post
                var rowPost = Query(cn, tab, true, pkColumns, pkValues);
                var argsPost = new TDataRowUpdateEventArgs(cm, rowPost, null, UpdateStatus.Continue, CommandOperation.Undelete);
                tab.RowUpdated(argsPost);
                if (rowPost.RowState != DataRowState.Unchanged)
                {
                    throw new InvalidOperationException("O registro não pode ser alterado no entrypoint PostUndelete.");
                }

                #region if(rowPost.HasErrors) [seta erro na conexão]; return null;

                var errorsPost = ErrorList.CreateFromDataRow(rowPost);
                if (errorsPost.Count > 0)
                {
                    errorsPost.CopyToConnection(cn);
                }
                else if (argsPre.Status != UpdateStatus.Continue)
                {
                    cn.SetError("Erro ao reinserir o registro.");
                }

                if (cn.ErrorCount > errorCount)
                {
                    return null;
                }

                #endregion

                return rowPost;

                #region cn.Close();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            #endregion
        }

        /// <summary>
        ///   Devolve null se e somente se cn.GetErrors() contiver novo erro.
        /// </summary>
        protected internal static TDataRow Update(TConnectionWritable cn, TDataTable tab, 
                                                  string[] pkColumns, object[] pkValues, 
                                                  string columnList, params object[] newValues)
        {
            

            if (cn == null)
            {
                throw new ArgumentNullException();
            }

            cn.Open(true);
            try
            {
                var errorCount = cn.ErrorCount;

                

                if (errorCount > 0)
                {
                    return null;
                }

                var columns = columnList.Split(',');
                if (newValues.Length != columns.Length)
                {
                    throw new ArgumentException("O número de valores deve ser igual ao número de colunas.");
                }

                var row = Query(cn, tab, pkColumns, pkValues);
                if (cn.ErrorCount > 0)
                {
                    return null;
                }

                if (row == null)
                {
                    cn.SetError("O registro não existe.");
                    return null;
                }

                for (var i = 0; i < columns.Length; i++)
                {
                    row[columns[i].Trim()] = DbObject.ToDbObject(newValues[i]);
                }

                row.Put(cn);
                if (row.HasErrors)
                {
                    // Copia os erros do DataRow para TConnection.
                    ErrorList.CreateFromDataRow(row).CopyToConnection(cn);
                    return null;
                }

                return row;

                #region cn.Close();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            #endregion
        }

        /// <summary>
        ///   Se não conseguir obter conexão, dá exception. Nunca devolve null.
        /// </summary>
        protected TConnection CreateConnection()
        {
            var cn = this.Table.CreateConnection();
            if (cn == null)
            {
                throw new InvalidOperationException("Não foi possível obter conexão ao banco de dados");
            }

            return cn;
        }

        /// <summary>
        ///   Se não conseguir obter conexão, dá exception. Nunca devolve null.
        /// </summary>
        protected TConnectionWritable CreateWritableConnection(TPermission permission)
        {
            var cn = this.Table.CreateWritableConnection(permission);
            if (cn == null)
            {
                throw new InvalidOperationException("Não foi possível obter conexão ao banco de dados");
            }

            return cn;
        }

        private static MethodInfo GetCreateCommandMethod(Type dataTable)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException();
            }

            var type = dataTable;
            while (type != null && type.BaseType != typeof (TDataTable))
            {
                type = type.BaseType;
            }

            if (type == null)
            {
                throw new ArgumentException("O tipo informado não é derivado de TDataTable.");
            }

            // TODO Verificar se dataTable é derivado de TDataTable.
            var createCommand = type.GetMethod("CreateCommand", BindingFlags.Static | BindingFlags.NonPublic);

            if (createCommand == null)
            {
                throw new ArgumentException("A classe informada não implementa o método estático CreateCommand().");
            }

            return createCommand;
        }

        /// <summary>
        ///   Obtém um registro utilizando as colunas e os valores respectivos para montar a cláusula where
        ///   (normalmente utiliza-se as colunas da chave da tabela).
        ///   Dá exception caso o where não restrinja suficientemente e o select traga mais de um registro.
        ///   Devolve null se nenhum registro for encontrado. Neste caso a mensagem de erro correspondente
        ///   será adicionada à conexão.
        /// </summary>
        private static TDataRow Query(TConnection cn, TDataTable tab, bool getDeletedRecords, 
                                      string[] pkColumns, object[] pkValues)
        {
            

            if (cn == null)
            {
                throw new ArgumentNullException();
            }

            cn.Open();
            try
            {
                var errorCount = cn.ErrorCount;

                

                tab.Get(cn, new string[0], 
                        StrLib.EnumerableToStr(pkColumns, " AND ", tab.MainTableName + ".", " = ?"), 
                        pkValues, string.Empty, 0, 0, true, getDeletedRecords);
                if (cn.ErrorCount > errorCount)
                {
                    throw new Exception(cn.GetErrors().ToString());
                }

                if (tab.Rows.Count > 1)
                {
                    throw new TooManyRowsException();
                }
                else if (tab.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    return tab.Rows[0];
                }

                #region cn.Close();
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            #endregion
        }
    }

    public class TDataRowCollection : IEnumerable
    {
        private readonly DataRowCollection rows;

        protected internal TDataRowCollection(DataRowCollection rows)
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

        protected internal TDataRow this[int index]
        {
            get
            {
                return (TDataRow)this.rows[index];
            }
        }

        public void Clear()
        {
            this.rows.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return this.rows.GetEnumerator();
        }

        protected internal void Add(TDataRow row)
        {
            this.rows.Add(row);
        }

        protected internal TDataRow Find(DbObject[] pkValues)
        {
            return (TDataRow)this.rows.Find(DbObject.ToObjectArray(pkValues));
        }

        protected internal TDataRow Find(object[] pkValues)
        {
            return (TDataRow)this.rows.Find(pkValues);
        }

        protected internal void Remove(TDataRow row)
        {
            this.rows.Remove(row);
        }

        protected TDataRow Add(DbObject[] values)
        {
            return (TDataRow)this.rows.Add(DbObject.ToObjectArray(values));
        }

        protected TDataRow Add(object[] values)
        {
            return (TDataRow)this.rows.Add(values);
        }

        protected void InsertAt(TDataRow row, int pos)
        {
            this.rows.InsertAt(row, pos);
        }
    }

    public class TooManyRowsException : Exception
    {
    }
}