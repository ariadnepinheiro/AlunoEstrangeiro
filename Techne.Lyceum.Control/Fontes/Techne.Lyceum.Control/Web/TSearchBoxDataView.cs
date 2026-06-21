using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web;
using System.Web.UI;
using Techne.Controls;
using Techne.Data;
using Techne.Library.Sql.Structure;

namespace Techne.Web
{
    public class TSearchBoxDataView : DataSourceView
    {
        // Fields
        private readonly TSearchBoxDataSource _owner;

        private TConnectionWritable _connection;

        private HttpContext _context;

        // Methods
        public TSearchBoxDataView(TSearchBoxDataSource owner, string name, HttpContext context)
            : base(owner, name)
        {
            this._owner = owner;
            this._context = context;
        }

        public override bool CanDelete
        {
            get
            {
                return false;
            }
        }

        public override bool CanInsert
        {
            get
            {
                return false;
            }
        }

        public override bool CanPage
        {
            get
            {
                return false;
            }
        }

        public override bool CanRetrieveTotalRowCount
        {
            get
            {
                return true;
            }
        }

        public override bool CanSort
        {
            get
            {
                return false;
            }
        }

        public override bool CanUpdate
        {
            get
            {
                return false;
            }
        }

        private string BaseSqlOrder
        {
            get
            {
                return this._owner.SqlOrder;
            }
        }

        private SqlSelect BaseSqlSelect
        {
            get
            {
                return this._owner.SqlSelect;
            }
        }

        private string BaseSqlWhere
        {
            get
            {
                return this._owner.SqlWhere;
            }
        }

        private TConnectionWritable Connection
        {
            get
            {
                if (this._connection == null)
                {
                    if (!string.IsNullOrEmpty(this._owner.Connection))
                    {
                        this._connection = ConnectionList.CreateWritableConnection(this._owner.Connection);
                    }
                    else
                    {
                        this._connection = new TConnectionWritable(Techne.Controls.TControl.GetConnectionString(this._owner, string.Empty));
                    }
                }

                return this._connection;
            }
        }

        public IEnumerable Select(DataSourceSelectArguments arguments)
        {
            return this.ExecuteSelect(arguments);
        }

        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {
            if (this.CanSort)
            {
                arguments.AddSupportedCapabilities(DataSourceCapabilities.Sort);
            }

            if (this.CanPage)
            {
                arguments.AddSupportedCapabilities(DataSourceCapabilities.Page);
            }

            if (this.CanRetrieveTotalRowCount)
            {
                arguments.AddSupportedCapabilities(DataSourceCapabilities.RetrieveTotalRowCount);
            }

            arguments.RaiseUnsupportedCapabilitiesError(this);

            var dt = this.Get();
            return dt.DefaultView;
        }

        private DataTable Get()
        {
            if (this.BaseSqlSelect.ToString().Length == 0)
            {
                throw new InvalidOperationException("A propriedade " + this._owner.ID + ".SqlSelect não foi informada");
            }

            TConnection cn = this.Connection;
            var where = this.BaseSqlWhere;
            var whereValues = new DbObject[0];
            var sql = string.Empty;
            cn.Open();
            try
            {
                TControl.GetSqlWhere(null, this._owner.RecordContainer, this._owner.Parent.NamingContainer, cn.Rdbms, false, ref where, ref whereValues);

                sql = this._owner.SqlSelectExtended +
                      (where == string.Empty ? string.Empty : " WHERE " + where) +
                      (this.BaseSqlOrder == string.Empty ? string.Empty : " ORDER BY " + this.BaseSqlOrder);

                var tab = new QueryTable(sql);
                tab.Query(cn, whereValues);
                return tab;
            }
            catch (Exception exc)
            {
                var oleDbExc = exc as OleDbException;
                if (oleDbExc != null)
                {
                    switch (oleDbExc.ErrorCode)
                    {
                        case -2147217913:
                            throw new Exception("Verifique se a propriedade " + this._owner.UniqueID + ".DataType está de acordo com o tipo da coluna informada na propriedade Key.");

                            // -2147217833: Arithmetic overflow error converting numeric to data type numeric.
                            // Este erro ocorre qdo passa-se um número maior do que o permitido pela coluna.
                        case -2147217833:
                            return null;
                    }
                }

                throw new Exception("Possível erro no select: " + sql, exc);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        private void ThrowErrors(TConnection connection)
        {
            if (connection == null || connection.ErrorCount < 1)
            {
                return;
            }

            var errors = connection.GetErrors();
            var fields = new List<string>();
            fields.Add(string.Empty);
            fields.AddRange(errors.FieldList);

            foreach (var field in fields)
            {
                var fieldErrors = errors[field];
                if (fieldErrors != null && fieldErrors.Length > 0)
                {
                    var msg = new StringBuilder();
                    foreach (var errMsg in fieldErrors)
                    {
                        if (msg.Length > 0)
                        {
                            msg.Append(", ");
                        }

                        msg.Append(errMsg);
                    }

                    connection.ClearErrors();
                    throw new Exception(msg.ToString());
                }
            }
        }
    }
}