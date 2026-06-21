using System;
using System.Collections;
using System.Data;
using Techne.Data;

namespace Techne
{
    internal class DataLib2
    {
        /// <summary>
        ///   Dado um registro de uma tabela, cria uma cláusula WHERE (sintaxe ADO.NET) que obtém
        ///   este registro restringindo as colunas da chave primária da tabela.
        /// </summary>
        public static void CreateWhere(TDataRow row, 
                                       out string where, out DbObject[] whereValues)
        {
            CreateWhere(row, GetPKColumnNames(row.Table), out where, out whereValues);
        }

        /// <summary>
        ///   Dado um registro de uma tabela, cria uma cláusula WHERE (sintaxe ADO.NET)
        ///   baseado nas colunas informadas. Note que a cláusula obtida não obtém necessariamente
        ///   um registro único. Isso só acontecerá se o conjunto de colunas informadas for
        ///   uma unique key (ou variações dela).
        /// </summary>
        public static void CreateWhere(TDataRow row, string[] columnNames, 
                                       out string where, out DbObject[] whereValues)
        {
            var columns = row.Table.Columns;
            var a = new string[columnNames.Length];
            var values = new ArrayList();
            var i = 0;

            foreach (var columnName in columnNames)
            {
                var columnValue = row[columnName];

                string strValue;
                if (columnValue.IsNull)
                {
                    strValue = " IS NULL";
                }
                else if (columnValue.Type == DbType.VarChar ||
                         columnValue.Type == DbType.Number ||
                         columnValue.Type == DbType.Date)
                {
                    strValue = " = ?";
                    values.Add(columnValue);
                }
                else
                {
                    throw new NotImplementedException("O tipo DbType." + columnValue.Type + " não foi implementado em Techne.DataLib.CreateWhere().");
                }

                a[i++] = ((TDataColumn)columns[columnName]).FullCol + strValue;
            }

            where = StrLib.EnumerableToStr(a, " AND ");
            whereValues = (DbObject[])values.ToArray(typeof (DbObject));
        }

        public static string[] GetPKColumnNames(DataTable table)
        {
            return GetPKColumnNames(table, false);
        }

        public static string[] GetPKColumnNames(DataTable table, bool fullName)
        {
            if (fullName && !(table is TDataTable))
            {
                throw new ArgumentException("O parâmetro fullName só pode ser true quando a tabela informada for do tipo TDataTable.");
            }

            var tablePK = new ArrayList();

            foreach (var c in table.PrimaryKey)
            {
                tablePK.Add(fullName ? ((TDataColumn)c).FullCol : c.ColumnName);
            }

            return (string[])tablePK.ToArray(typeof (string));
        }

        /// <summary>
        ///   Busca no banco de dados um registro da tabela informada, dado um array com os valores da primary key.
        ///   O registro será trazido para a tabela informada.
        ///   Traz todas as colunas da tabela.
        /// </summary>
        /// <param name = "connection">Se null, abre uma nova conexão.</param>
        public static TDataRow GetRecord(TConnection connection, 
                                         TDataTable dataTable, DbObject[] primaryKeyValues, 
                                         bool getIfDeleted)
        {
            return GetRecord(connection, dataTable, primaryKeyValues, new string[0], getIfDeleted);
        }

        /// <summary>
        ///   Busca no banco de dados um registro da tabela informada, dado um array com os valores da primary key.
        ///   O registro será trazido para a tabela informada.
        ///   Este overload permite informar as colunas da tabela que serão trazidas.
        /// </summary>
        /// <param name = "connection">Se null, abre uma nova conexão.</param>
        /// <param name = "columns">Para trazer todas as colunas informe new string[0]. Null não é permitido.</param>
        public static TDataRow GetRecord(TConnection connection, 
                                         TDataTable dataTable, DbObject[] primaryKeyValues, string[] columns, 
                                         bool getIfDeleted)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException("dataTable");
            }

            if (primaryKeyValues == null)
            {
                throw new ArgumentNullException("primaryKeyValues");
            }

            if (columns == null)
            {
                throw new ArgumentNullException("columns");
            }

            var cn = connection != null ? connection : dataTable.CreateConnection();

            cn.Open();
            try
            {
                // TODO Corrigir o parâmetro columns qdo null não for mais permitido como parâmetro de Get()
                var count = dataTable.Get(
                    cn, 
                    columns.Length == 0 ? null : columns, // string[] columns
                    StrLib.EnumerableToStr(GetPKColumnNames(dataTable, true), " AND ", string.Empty, " = ?"), // string where
                    primaryKeyValues, // DbObject[] whereValues
                    string.Empty, // string order
                    0, 0, // startRecord, maxRecord
                    false, getIfDeleted // createPrimaryKeys, getDeletedRecords
                    );

                if (count == 0)
                {
                    return null;
                }
                else
                {
                    return dataTable.Rows[dataTable.Rows.Count - 1];
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
    }
}