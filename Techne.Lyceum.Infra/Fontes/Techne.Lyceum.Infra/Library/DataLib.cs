using System;
using System.Collections;
using System.Data;
using System.Text;

namespace Techne
{
    public class DataLib
    {
        /// <summary>
        ///   Obtém uma mensagem que informa a quantidade de registros novos,
        ///   removidos, alterados e detacheds em uma tabela. Devolve string.Empty
        ///   caso não haja nenhuma alteração na tabela. Nunca devolve null.
        /// </summary>
        public static string ChangeCountMessage(DataTable tab)
        {
            var added = tab.Select(string.Empty, string.Empty, DataViewRowState.Added).Length;
            var deleted = tab.Select(string.Empty, string.Empty, DataViewRowState.Deleted).Length;
            var modified = tab.Select(string.Empty, string.Empty, DataViewRowState.ModifiedCurrent).Length;

            if (added != 0 || deleted != 0 || modified != 0)
            {
                var msg = new ArrayList();
                if (added != 0)
                {
                    msg.Add(added + " registros novos");
                }

                if (deleted != 0)
                {
                    msg.Add(deleted + " registros removidos");
                }

                if (modified != 0)
                {
                    msg.Add(modified + " registros alterados");
                }

                return StrLib.EnumerableToStr(msg);
            }

            return string.Empty;
        }

        /// <summary>
        ///   Excetuando-se os registros detached, obtém uma lista contendo a descrição
        ///   da alteração de cada um dos registros alterados. Nunca devolve null.
        /// </summary>
        public static string[] ChangeList(DataTable tab)
        {
            var list = new ArrayList();
            var primaryKey = tab.PrimaryKey;

            foreach (var state in new[] { DataViewRowState.Added, DataViewRowState.ModifiedCurrent, DataViewRowState.Deleted })
            {
                foreach (var row in tab.Select(string.Empty, string.Empty, state))
                {
                    var version = state == DataViewRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current;

                    

                    var originalValues = string.Empty;
                    if (state == DataViewRowState.ModifiedCurrent)
                    {
                        var changes = new ArrayList();
                        foreach (DataColumn dataCol in tab.Columns)
                        {
                            var original = row[dataCol, DataRowVersion.Original];
                            if (!row[dataCol].Equals(original))
                            {
                                var s = original.ToString();
                                if (s.Length > 30)
                                {
                                    s = s.Substring(0, 27) + "...";
                                }

                                changes.Add(dataCol.ColumnName + ": " + s);
                            }
                        }

                        originalValues = " Original: (" + StrLib.EnumerableToStr(changes, ", ") + ")";
                    }

                    

                    list.Add(
                        state + ": " + tab.TableName + "(" +
                        DataLib.DataRowToString(row, primaryKey, version) +
                        ")" +
                        (originalValues.Length == 0 ? string.Empty : originalValues)
                        );
                }
            }

            return (string[])list.ToArray(typeof (string));
        }

        /// <summary>
        ///   Devolve o primeiro registro da tabela que contém erro (TDataRow.HasErrors=true).
        ///   Se a tabela não contém nenhum registro com erro (TDataTable.HasErrors=false), devolve null;
        /// </summary>
        public static DataRow FirstRowWithError(DataTable table)
        {
            if (table.HasErrors)
            {
                foreach (var row in table.Select())
                {
                    if (row.HasErrors)
                    {
                        return row;
                    }
                }
            }

            return null;
        }

        public static string RowErrorsToString(DataRow row)
        {
            if (row == null || !row.HasErrors)
            {
                return string.Empty;
            }

            var b = new StringBuilder();

            if (row.RowError.Length > 0)
            {
                b.Append(row.RowError);
            }
            else
            {
                b.Append("Erro(s):");
            }

            foreach (var column in row.GetColumnsInError())
            {
                if (b.Length > 0)
                {
                    b.Append("\n");
                }

                b.Append("  " + column.ColumnName + ": " + row.GetColumnError(column));
            }

            return b.ToString();
        }

        [Obsolete("Verificar se os elementos do Array retornado devem ser transformados para DbObject.")]
        internal static Array DataColumnValueToArray(DataView view, string columnName)
        {
            return DataColumnValueToArray(view, columnName, typeof (object));
        }

        /// <summary>
        ///   Obtém uma lista de valores correspondente a uma coluna de um DataView.
        /// </summary>
        /// <param name = "type">Tipo da coluna. Se informado, será possível fazer o cast para o array do tipo.</param>
        [Obsolete("Verificar se os elementos do Array retornado devem ser transformados para DbObject.")]
        internal static Array DataColumnValueToArray(DataView view, string columnName, Type type)
        {
            var result = new ArrayList();
            for (var n = 0; n < view.Count; n++)
            {
                result.Add(view[n].Row[columnName]);
            }

            return result.ToArray(type);
        }

        /// <summary>
        ///   Mostra nome e valor de cada coluna do DataRow informado.
        /// </summary>
        internal static string DataRowToString(DataRow row)
        {
            return DataRowToString(row, new DataColumn[0]);
        }

        /// <summary>
        ///   Mostra nome e valor de cada coluna do DataRow informado.
        /// </summary>
        /// <param name = "columns">Colunas a serem mostradas. Informar (new DataColumn[0]) para mostrar todas.</param>
        internal static string DataRowToString(DataRow row, DataColumn[] columns)
        {
            return DataRowToString(
                row, columns, 
                row.RowState == DataRowState.Deleted ? DataRowVersion.Original : DataRowVersion.Current
                );
        }

        internal static string DataRowToString(DataRow row, DataColumn[] columns, DataRowVersion version)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            if (columns.Length == 0)
            {
                var aux = new ArrayList();
                foreach (DataColumn column in row.Table.Columns)
                {
                    aux.Add(column);
                }

                columns = (DataColumn[])aux.ToArray(typeof (DataColumn));
            }

            var strColVal = StrLib.EnumerablesToStrArray(
                ": ", 
                TechLib.EnumerableItemProperty(columns, "ColumnName"), 
                GetRowValues(row, columns, version)
                );

            return StrLib.EnumerableToStr(strColVal);
        }

        internal static string DataTableToString(DataTable table)
        {
            return DataTableToString(table, new DataColumn[0]);
        }

        internal static string DataTableToString(DataTable table, DataColumn[] columns)
        {
            return DataTableToString(table, columns, "\r\n");
        }

        internal static string DataTableToString(DataTable table, DataColumn[] columns, string separator)
        {
            var rowList = new ArrayList();
            foreach (DataRow row in table.Rows)
            {
                rowList.Add(row.RowState + ": " +
                            DataRowToString(row, columns, row.RowState == DataRowState.Deleted ? DataRowVersion.Original
                                                              : DataRowVersion.Current));
            }

            return StrLib.EnumerableToStr(rowList, separator);
        }

        /// <summary>
        ///   Obtém o valor de uma coluna da chave primária de uma tabela.
        /// </summary>
        /// <param name = "pkColumnName">Nome da coluna da chave primária cujo valor deseja-se obter.</param>
        /// <param name = "pkValues">Lista de valores da chave primária.</param>
        /// <param name = "table">Tabela à qual a chave primária se refere.</param>
        internal static DbObject FindPkColumnValue(string pkColumnName, DbObject[] pkValues, DataTable table)
        {
            var lower = pkColumnName.ToLower();
            var primaryKey = table.PrimaryKey;
            for (var i = 0; i < primaryKey.Length; i++)
            {
                if (primaryKey[i].ColumnName.ToLower() == lower)
                {
                    return pkValues[i];
                }
            }

            throw new ArgumentException("A coluna informada não faz parte do DataTable.", "pkColumn");
        }

        /// <summary>
        ///   Um número do tipo 'decimal' pode ter mais de uma representação interna (lo, mid, hi, flags)
        ///   para o mesmo número. Aparentemente, quando um número é buscado do banco, ele vem numa
        ///   representação interna que, quando convertido para string via ToString(), apresenta zeros
        ///   após a última casa decimal, o que é indesejável para seu display.
        ///   Operações booleanas de comparação não detectam tal diferença pois, apesar da representação
        ///   interna ser diferente, representam o mesmo número.
        ///   Por exemplo, o número 1 torna-se 1.000000000 e 1.23 tornava-se 1.230000000.
        ///   Para contornar tal problema, este método converte um valor 'decimal' para o tipo 'double'
        ///   e o converte de volta para 'decimal'. Essas conversões fazem com que o valor
        ///   no tipo 'decimal' seja "reconstruído" na sua representação interna mais simples,
        ///   e o método ToString() funcione adequadamente.
        /// </summary>
        internal static object FixOleDbValue(object value)
        {
            if (value is decimal)
            {
                var dbl = Convert.ToDouble((decimal)value);
                return Convert.ToDecimal(dbl);
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        ///   Retorna o RowState de um DataRow para Unchanged se não houverem alterações no registro.
        /// </summary>
        internal static bool FixRowState(DataRow row)
        {
            if (row.RowState != DataRowState.Modified)
            {
                return false;
            }

            foreach (DataColumn col in row.Table.Columns)
            {
                if (!row[col].Equals(row[col, DataRowVersion.Original]))
                {
                    return false;
                }
            }

            row.RejectChanges();
            return true;
        }

        internal static DataColumn GetChildColumn(DataColumn column, DataRelation relation)
        {
            for (var n = 0; n < relation.ParentColumns.Length; n++)
            {
                if (relation.ParentColumns[n] == column)
                {
                    return relation.ChildColumns[n];
                }
            }

            return null;
        }

        /// <summary>
        ///   Devolve o conjunto de colunas que não fazem parte da primary key da tabela.
        /// </summary>
        internal static DataColumn[] GetColumnsOffPrimaryKey(DataTable table)
        {
            var result = new DataColumn[table.Columns.Count - table.PrimaryKey.Length];
            var i = 0;
            foreach (DataColumn column in table.Columns)
            {
                if (!((IList)table.PrimaryKey).Contains(column))
                {
                    result[i++] = column;
                }
            }

            return result;
        }

        internal static DataColumn[] GetDataColumnList(DataTable table, string[] columnNames)
        {
            var columns = new ArrayList();
            foreach (var columnName in columnNames)
            {
                if (!table.Columns.Contains(columnName))
                {
                    throw new ArgumentException("A coluna " + columnName + " não existe na tabela " + table.TableName);
                }

                columns.Add(table.Columns[columnName]);
            }

            return (DataColumn[])columns.ToArray(typeof (DataColumn));
        }

        internal static DataColumn GetParentColumn(DataColumn column, DataRelation relation)
        {
            for (var n = 0; n < relation.ChildColumns.Length; n++)
            {
                if (relation.ChildColumns[n] == column)
                {
                    return relation.ParentColumns[n];
                }
            }

            return null;
        }

        /// <summary>
        ///   Devolve lista de valores do DataRow correspondente a uma lista de colunas.
        ///   DICA: Para obter todas as colunas, utilize diretamente a propriedade ItemArray do DataRow.
        /// </summary>
        internal static DbObject[] GetRowValues(DataRow row, DataColumn[] columns)
        {
            return GetRowValues(row, columns, DataRowVersion.Default);
        }

        internal static DbObject[] GetRowValues(DataRow row, DataColumn[] columns, DataRowVersion version)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            var result = new DbObject[columns.Length];

            for (var i = 0; i < columns.Length; i++)
            {
                result[i] = DbObject.ToDbObject(row[columns[i], version]);
            }

            return result;
        }

        internal static bool HasChanges(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row.RowState != DataRowState.Unchanged)
                {
                    return true;
                }
            }

            return false;
        }
    }
}