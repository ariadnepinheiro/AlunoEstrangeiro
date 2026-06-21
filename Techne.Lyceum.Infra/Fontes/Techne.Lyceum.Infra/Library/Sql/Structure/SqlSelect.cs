using System;
using System.Collections;
using System.ComponentModel;

namespace Techne.Library.Sql.Structure
{
    [TypeConverter(typeof (SqlSelectConverter)), Description("Armazena um comando Select (SQL) de forma estruturada. Suporta somente o formato simples SELECT [colunas] FROM [tabela], sem JOIN's, WHERE ou ORDER BY")]
    public class SqlSelect
    {
        private readonly SqlSelectColumns columns;

        private readonly bool distinct;

        private readonly string table;

        public SqlSelect() : this(string.Empty, new string[0], false)
        {
        }

        public SqlSelect(string table, string[] columns, bool distinct)
        {
            this.table = table;

            this.columns = new SqlSelectColumns();
            this.columns.AddRange(columns);

            this.distinct = distinct;
        }

        public SqlSelect(string table, SqlSelectColumns columns, bool distinct)
        {
            this.table = table;
            this.columns = columns;
            this.distinct = distinct;
        }

        [
            TypeConverter(typeof (ArrayConverter))
        ]
        public SqlSelectColumns Columns
        {
            get
            {
                return this.columns;
            }
        }

        public bool Distinct
        {
            get
            {
                return this.distinct;
            }
        }

        public string Table
        {
            get
            {
                return this.table;
            }
        }

        public static SqlSelect Parse(string sql)
        {
            const string idSelect = "SELECT ";
            const string idFrom = " FROM ";

            if (sql == null)
            {
                throw new ArgumentNullException();
            }

            if (sql == string.Empty)
            {
                return new SqlSelect();
            }

            var upper = sql.ToUpper();

            if (upper.IndexOf(" WHERE ") >= 0)
            {
                throw new ArgumentException("A cláusula WHERE não é permitida");
            }

            if (upper.IndexOf(" ORDER BY ") >= 0)
            {
                throw new ArgumentException("A cláusula ORDER BY não é permitida");
            }

            var i = upper.IndexOf(idSelect);
            var j = upper.IndexOf(idFrom);
            if (i < 0 || j < 0)
            {
                throw new ArgumentException("O comando SQL contém um erro de sintaxe");
            }

            var table = string.Empty;
            if (j + idFrom.Length < sql.Length)
            {
                table = sql.Substring(j + idFrom.Length).Trim();
            }

            var distinct = false;
            var columns = new ArrayList();
            if (j > i + idSelect.Length)
            {
                var columnsList = sql.Substring(i + idSelect.Length, j - (i + idSelect.Length));

                if (columnsList.TrimStart().ToUpper().StartsWith("DISTINCT "))
                {
                    var distinctIndex = columnsList.ToUpper().IndexOf("DISTINCT ");
                    distinct = true;
                    columnsList = columnsList.Substring(distinctIndex + 9);
                }

                foreach (var column in columnsList.Split(','))
                {
                    columns.Add(column);
                }
            }

            return new SqlSelect(table, (string[])columns.ToArray(typeof (string)), distinct);
        }

        public override string ToString()
        {
            if (this.table == string.Empty && this.columns.Count == 0)
            {
                return string.Empty;
            }

            return "SELECT " + (this.distinct ? "DISTINCT " : string.Empty) + StrLib.EnumerableToStr(this.columns, ", ") + " " +
                   "FROM " + this.table.ToLower();
        }

        public SqlSelect Clone()
        {
            return new SqlSelect(this.table, this.columns.Clone(), this.distinct);
        }

        /// <summary>
        ///   Devolve lista com o nome das colunas. Remove tabela, se especificada,
        ///   e, caso seja especificado alias, devolve o alias.
        ///   Exemplo: [coluna] -> [coluna], [tabela].[coluna] -> [coluna], [coluna] AS [alias] -> [alias], [tabela].[coluna] AS [alias] -> [alias]
        /// </summary>
        public string[] GetColumnIds()
        {
            var names = new string[this.columns.Count];

            for (var i = 0; i < this.columns.Count; i++)
            {
                names[i] = this.columns[i].Id;
            }

            return names;
        }
    }

    public class SqlSelectColumn
    {
        private string alias;

        private string column;

        private string id;

        private string table;

        private SqlSelectColumn(string table, string column, string alias)
        {
            if (column == null || column.Length == 0)
            {
                throw new ArgumentException("Coluna não informada.");
            }

            this.table = table == null ? string.Empty : table;
            this.column = column;
            this.alias = alias == null ? string.Empty : alias;

            this.RedoId();
        }

        public string Alias
        {
            get
            {
                return this.alias;
            }

            set
            {
                this.alias = value == null ? string.Empty : value.Trim();
                this.RedoId();
            }
        }

        public string Column
        {
            get
            {
                return this.column;
            }

            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    throw new ArgumentNullException("Coluna não pode ser null ou string vazia.");
                }

                this.column = value == null ? string.Empty : value.Trim();
                this.RedoId();
            }
        }

        /// <summary>
        ///   Identificador da coluna. Sempre minúscula.
        ///   É o Alias se ele existir. Caso contrário é a Coluna.
        /// </summary>
        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public string Table
        {
            get
            {
                return this.table;
            }

            set
            {
                this.table = value == null ? string.Empty : value.Trim();
            }
        }

        /// <summary>
        ///   Parseia uma coluna que pode aparecer na lista de colunas de um comando SELECT.
        /// </summary>
        /// <param name = "sqlColumn">String no formato [tabela].[coluna] AS [alias], [tabela].[coluna], [coluna] AS [alias] ou [coluna].</param>
        public static SqlSelectColumn Parse(string sqlColumn)
        {
            var table = string.Empty;
            var column = string.Empty;
            var alias = string.Empty;

            var indexAs = sqlColumn.ToUpper().IndexOf(" AS ");
            if (indexAs >= 0)
            {
                alias = sqlColumn.Substring(indexAs + 4).Trim();
                sqlColumn = sqlColumn.Substring(0, indexAs);
            }

            var indexDot = sqlColumn.IndexOf(".");
            if (indexDot < 0)
            {
                column = sqlColumn.Trim();
            }
            else
            {
                table = sqlColumn.Substring(0, indexDot).Trim();
                column = sqlColumn.Substring(indexDot + 1).Trim();
            }

            return new SqlSelectColumn(table, column, alias);
        }

        public override string ToString()
        {
            return this.ToString(true);
        }

        public SqlSelectColumn Clone()
        {
            return new SqlSelectColumn(this.table, this.column, this.alias);
        }

        public string ToString(bool withAlias)
        {
            return (this.table.Length > 0 ? this.table + "." : string.Empty) +
                   this.column +
                   (withAlias && this.alias.Length > 0 ? " AS " + this.alias : string.Empty);
        }

        private void RedoId()
        {
            this.id = (this.alias.Length > 0 ? this.alias : this.column).ToLower();
        }
    }

    public class SqlSelectColumns : CollectionBase
    {
        public SqlSelectColumn this[int index]
        {
            get
            {
                return (SqlSelectColumn)this.List[index];
            }
        }

        public SqlSelectColumn this[string columnId]
        {
            get
            {
                var index = this.IndexOf(columnId);
                if (index < 0)
                {
                    throw new ArgumentException("A coluna solicitada (" + columnId + ") não existe na collection.");
                }

                return this[index];
            }
        }

        /// <param name = "sqlColumn">String no formato [tabela].[coluna] AS [alias], [tabela].[coluna], [coluna] AS [alias] ou [coluna].</param>
        public int Add(string sqlColumn)
        {
            return this.List.Add(SqlSelectColumn.Parse(sqlColumn));
        }

        public void AddRange(string[] sqlColumns)
        {
            foreach (var sqlColumn in sqlColumns)
            {
                Add(sqlColumn);
            }
        }

        public SqlSelectColumns Clone()
        {
            var clone = new SqlSelectColumns();
            foreach (SqlSelectColumn column in this.List)
            {
                clone.Add(column.Clone());
            }

            return clone;
        }

        public bool Contains(string columnId)
        {
            return this.IndexOf(columnId) >= 0;
        }

        public int IndexOf(string columnId)
        {
            for (var i = 0; i < this.List.Count; i++)
            {
                if (((SqlSelectColumn)this.List[i]).Id == columnId)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, SqlSelectColumn column)
        {
            if (this.Contains(column.Id))
            {
                throw new InvalidOperationException("A coluna " + column.Id + " já existe na lista de colunas.");
            }

            this.List.Insert(index, column);
        }

        private int Add(SqlSelectColumn column)
        {
            if (this.Contains(column.Id))
            {
                throw new InvalidOperationException("A coluna " + column.Id + " já existe na lista de colunas.");
            }

            return this.List.Add(column);
        }
    }
}