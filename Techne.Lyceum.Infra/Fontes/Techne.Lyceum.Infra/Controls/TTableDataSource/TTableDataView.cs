using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;

namespace Techne.Controls
{
    public class TTableDataView : DataSourceView, IStateManager
    {
        // Fields
        private static readonly object EventSelecting = new object();

        private readonly HttpContext _context;

        private readonly TTableDataSource _owner;

        private TConnectionWritable _connection;

        private string _dataTableClassName = string.Empty;

        private TDataTable _dataTableObject;

        private Type _dataTableType;

        private TTableDataSourceMethod _selectMethodInfo;

        private string _selectMethodName = string.Empty;

        private ParameterCollection _selectMethodParameters;

        private string _sqlColumns = string.Empty;

        private string _sqlWhereExpression = string.Empty;

        private string _sqlWhereOrder = string.Empty;

        private ParameterCollection _sqlWhereParameters;

        private bool _tracking;

        // Methods
        public TTableDataView(TTableDataSource owner, string name, HttpContext context)
            : base(owner, name)
        {
            this._owner = owner;
            this._context = context;
        }

        public event TTableDataSourceSelectingEventHandler Selecting
        {
            add
            {
                base.Events.AddHandler(EventSelecting, value);
            }

            remove
            {
                base.Events.RemoveHandler(EventSelecting, value);
            }
        }

        public override bool CanDelete
        {
            get
            {
                return this.DataTableType != null;
            }
        }

        public override bool CanInsert
        {
            get
            {
                return this.DataTableType != null;
            }
        }

        public override bool CanPage
        {
            get
            {
                return this.DataTableType != null;
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
                return this.DataTableType != null;
            }
        }

        public override bool CanUpdate
        {
            get
            {
                return this.DataTableType != null;
            }
        }

        public bool IsTrackingViewState
        {
            get
            {
                return this._tracking;
            }
        }

        public ParameterCollection SelectMethodParameters
        {
            get
            {
                if (this._selectMethodParameters == null)
                {
                    this._selectMethodParameters = new ParameterCollection();
                    this._selectMethodParameters.ParametersChanged += this.SelectMethodParametersChangedEventHandler;
                    if (this._tracking)
                    {
                        ((IStateManager)this._selectMethodParameters).TrackViewState();
                    }
                }

                return this._selectMethodParameters;
            }
        }

        public string SqlColumns
        {
            get
            {
                return this._sqlColumns;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                if (this._sqlColumns != value)
                {
                    this._sqlColumns = value;
                    this.OnDataSourceViewChanged(EventArgs.Empty);
                }
            }
        }

        public string SqlOrder
        {
            get
            {
                return this._sqlWhereOrder;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                if (this._sqlWhereOrder != value)
                {
                    this._sqlWhereOrder = value;
                    this.OnDataSourceViewChanged(EventArgs.Empty);
                }
            }
        }

        // public ConflictOptions ConflictDetection
        // {
        // get
        // {
        // return this._conflictDetection;
        // }
        // set
        // {
        // if ((value < ConflictOptions.OverwriteChanges) || (value > ConflictOptions.CompareAllValues))
        // {
        // throw new ArgumentOutOfRangeException("value");
        // }
        // this._conflictDetection = value;
        // this.OnDataSourceViewChanged(EventArgs.Empty);
        // }
        // }

        // public bool ConvertNullToDBNull
        // {
        // get
        // {
        // return this._convertNullToDBNull;
        // }
        // set
        // {
        // this._convertNullToDBNull = value;
        // }
        // }

        // public string DataObjectTypeName
        // {
        // get
        // {
        // if (this._dataObjectTypeName == null)
        // {
        // return string.Empty;
        // }
        // return this._dataObjectTypeName;
        // }
        // set
        // {
        // if (this.DataObjectTypeName != value)
        // {
        // this._dataObjectTypeName = value;
        // this.OnDataSourceViewChanged(EventArgs.Empty);
        // }
        // }
        // }

        // public bool EnablePaging
        // {
        // get
        // {
        // return this._enablePaging;
        // }
        // set
        // {
        // if (this.EnablePaging != value)
        // {
        // this._enablePaging = value;
        // this.OnDataSourceViewChanged(EventArgs.Empty);
        // }
        // }
        // }
        public string SqlWhere
        {
            get
            {
                return this._sqlWhereExpression;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                if (this._sqlWhereExpression != value)
                {
                    this._sqlWhereExpression = value;
                    this.OnDataSourceViewChanged(EventArgs.Empty);
                }
            }
        }

        public ParameterCollection SqlWhereParameters
        {
            get
            {
                if (this._sqlWhereParameters == null)
                {
                    this._sqlWhereParameters = new ParameterCollection();
                    this._sqlWhereParameters.ParametersChanged += this.SelectParametersChangedEventHandler;
                    if (this._tracking)
                    {
                        ((IStateManager)this._sqlWhereParameters).TrackViewState();
                    }
                }

                return this._sqlWhereParameters;
            }
        }

        internal string DataTableClassName
        {
            get
            {
                return this._dataTableClassName;
            }

            set
            {
                this._dataTableClassName = value == null ? string.Empty : value;
            }
        }

        internal Type DataTableType
        {
            get
            {
                if (this._dataTableType == null)
                {
                    this._dataTableType = BuildManager.GetType(this._dataTableClassName, false, true);
                }

                return this._dataTableType;
            }
        }

        internal string SelectMethod
        {
            get
            {
                return this._selectMethodName;
            }

            set
            {
                this._selectMethodName = value == null ? string.Empty : value;
            }
        }

        private TConnectionWritable Connection
        {
            get
            {
                if (this._connection == null)
                {
                    var dt = this.DataTableObject;
                    if (dt != null)
                    {
                        TPermission permission = null;
                        TechneHttpApplication application = null;
                        if (System.Web.HttpContext.Current != null)
                        {
                            application = System.Web.HttpContext.Current.ApplicationInstance as TechneHttpApplication;
                        }

                        if (application != null)
                        {
                            permission = application.GetPagePermission(System.Web.HttpContext.Current.Request);
                        }
                        else if (TPermission.ThreadPermission != null)
                        {
                            permission = TPermission.ThreadPermission;
                        }
                        else
                        {
                            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null)
                            {
                                permission = new TPermission(System.Web.HttpContext.Current.Request.Url.PathAndQuery, "PAGINA", true, true, true, true);
                            }
                            else
                            {
                                permission = new TPermission(string.Empty, "PAGINA", true, true, true, true);
                            }
                        }

                        this._connection = dt.CreateWritableConnection(permission);
                    }
                }

                return this._connection;
            }
        }

        private TDataTable DataTableObject
        {
            get
            {
                if (this._dataTableObject == null)
                {
                    if (this.DataTableType != null)
                    {
                        this._dataTableObject = this.DataTableType.Assembly.CreateInstance(this.DataTableType.FullName) as TDataTable;
                    }
                }

                return this._dataTableObject;
            }
        }

        // public string SortExpression
        // {
        // get
        // {
        // return this._sortExpression;
        // }
        // set
        // {
        // if (this.SortExpression != value)
        // {
        // this._sortExpression = value==null?"":value;
        // this.OnDataSourceViewChanged(EventArgs.Empty);
        // }
        // }
        // }

        private TTableDataSourceMethod SelectMethodInfo
        {
            get
            {
                if (this._selectMethodInfo == null)
                {
                    if (this._selectMethodName != null && this._selectMethodName.Length > 0 && this.DataTableType != null)
                    {
                        var parameters = new Dictionary<string, System.Data.DbType>();
                        foreach (Parameter par in this.SelectMethodParameters)
                        {
                            parameters.Add(par.Name, par.DbType);
                        }

                        this._selectMethodInfo = this.GetMethodInfo(this.DataTableType, this._selectMethodName, parameters);
                    }
                }

                return this._selectMethodInfo;
            }
        }

        public int Insert(IDictionary values)
        {
            return this.ExecuteInsert(values);
        }

        public IEnumerable Select(DataSourceSelectArguments arguments)
        {
            return this.ExecuteSelect(arguments);
        }

        protected virtual void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                var pair = (Pair)savedState;
                if (pair.First != null)
                {
                    ((IStateManager)this.SqlWhereParameters).LoadViewState(pair.First);
                }

                if (pair.Second != null)
                {
                    ((IStateManager)this.SelectMethodParameters).LoadViewState(pair.Second);
                }
            }
        }

        protected virtual void OnSelecting(TTableDataSourceSelectingEventArgs e)
        {
            var handler = base.Events[EventSelecting] as TTableDataSourceSelectingEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual object SaveViewState()
        {
            var pair = new Pair();
            pair.First = (this._sqlWhereParameters != null) ? ((IStateManager)this._sqlWhereParameters).SaveViewState() : null;
            pair.Second = (this._selectMethodParameters != null) ? ((IStateManager)this._selectMethodParameters).SaveViewState() : null;
            if ((pair.First == null) && (pair.Second == null))
            {
                return null;
            }

            return pair;
        }

        protected virtual void TrackViewState()
        {
            this._tracking = true;
            if (this._sqlWhereParameters != null)
            {
                ((IStateManager)this._sqlWhereParameters).TrackViewState();
            }

            if (this._selectMethodParameters != null)
            {
                ((IStateManager)this._selectMethodParameters).TrackViewState();
            }
        }

        protected override int ExecuteDelete(IDictionary keys, IDictionary oldValues)
        {
            if (!this.CanDelete)
            {
                throw new NotSupportedException(this._owner.ID + ": Operação de remoção não suportada");
            }

            var listPkNames = new List<string>();
            var listPkValues = new List<DbObject>();
            foreach (var k in keys.Keys)
            {
                listPkNames.Add(string.Empty + k);
                if (keys[k] == null || keys[k] == DBNull.Value)
                {
                    listPkValues.Add(DbObject.ToDbObject(DBNull.Value));
                }
                else
                {
                    listPkValues.Add(DbObject.ToDbObject(keys[k]));
                }
            }

            var pkNames = listPkNames.ToArray();
            var pkValues = listPkValues.ToArray();

            var conn = this.Connection;
            conn.Open(true);
            try
            {
                var tab = this.DataTableObject;
                var deleted = TDataRow.Delete(conn, tab, pkNames, pkValues);
                if (deleted == false)
                {
                    this.ThrowErrors(this.Connection);
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            finally
            {
                conn.Close();
            }
        }

        protected override int ExecuteInsert(IDictionary values)
        {
            if (!this.CanInsert)
            {
                throw new NotSupportedException(this._owner.ID + ": Operação de inserção não suportada.");
            }

            var listColNames = new List<string>();
            var listColValues = new List<DbObject>();
            foreach (var k in values.Keys)
            {
                listColNames.Add(string.Empty + k);
                if (values[k] == null || values[k] == DBNull.Value)
                {
                    listColValues.Add(DbObject.ToDbObject(DBNull.Value));
                }
                else
                {
                    listColValues.Add(DbObject.ToDbObject(values[k]));
                }
            }

            var colNames = listColNames.ToArray();
            var colValues = listColValues.ToArray();

            var conn = this.Connection;
            conn.Open(true);
            try
            {
                var tab = this.DataTableObject;
                var row = TDataRow.Insert(conn, tab, colNames, colValues, null, new DbObject[] { });

                if (row == null)
                {
                    this.ThrowErrors(conn);
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            finally
            {
                conn.Close();
            }
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

            var usingSelectMethod = this._selectMethodName != null && this._selectMethodName.Trim().Length > 0;

// pega valores parâmetros
            IOrderedDictionary parameters = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);

            if (usingSelectMethod)
            {
                foreach (DictionaryEntry entry in this.SelectMethodParameters.GetValues(this._context, this._owner))
                {
                    parameters[entry.Key] = entry.Value;
                }
            }
            else
            {
                foreach (DictionaryEntry entry in this.SqlWhereParameters.GetValues(this._context, this._owner))
                {
                    parameters[entry.Key] = entry.Value;
                }
            }

// dispara evento "Selecting", permitindo mudança manual de parãmetros
            var e = new TTableDataSourceSelectingEventArgs(parameters, arguments, false);
            this.OnSelecting(e);
            if (e.Cancel)
            {
                return null;
            }

            if (usingSelectMethod)
            {
                return this.ExecuteSelectMethod(parameters, arguments);
            }
            else
            {
                return this.ExecuteSqlSelect(parameters, arguments);
            }
        }

        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
        {
            var listPkNames = new List<string>();
            var listPkValues = new List<DbObject>();
            foreach (var k in keys.Keys)
            {
                listPkNames.Add(string.Empty + k);
                if (keys[k] == null || keys[k] == DBNull.Value)
                {
                    listPkValues.Add(DbObject.ToDbObject(DBNull.Value));
                }
                else
                {
                    listPkValues.Add(DbObject.ToDbObject(keys[k]));
                }
            }

            var listColNames = new List<string>();
            var listColValues = new List<DbObject>();
            foreach (var k in values.Keys)
            {
                listColNames.Add(string.Empty + k);
                if (values[k] == null || values[k] == DBNull.Value)
                {
                    listColValues.Add(DbObject.ToDbObject(DBNull.Value));
                }
                else
                {
                    listColValues.Add(DbObject.ToDbObject(values[k]));
                }
            }

            var pkNames = listPkNames.ToArray();
            var pkValues = listPkValues.ToArray();
            var colNames = listColNames.ToArray();
            var colValues = listColValues.ToArray();
            var colList = string.Join(",", colNames);

            var conn = this.Connection;
            conn.Open(true);
            try
            {
                var tab = this.DataTableObject;
                var row = TDataRow.Update(this.Connection, tab, pkNames, pkValues, colList, colValues);
                if (row == null)
                {
                    this.ThrowErrors(this.Connection);
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            finally
            {
                conn.Close();
            }
        }

        protected void ConvertWhere(string sqlWhereExpression, IOrderedDictionary parameters, out string sqlWhere, out DbObject[] parValues)
        {
            var bInQuotes = false;
            var bInDoubleQuotes = false;

            var resultWhere = new StringBuilder();
            var listPars = new List<DbObject>();

            if (sqlWhereExpression == null)
            {
                sqlWhereExpression = string.Empty;
            }

            for (var i = 0; i < sqlWhereExpression.Length; i++)
            {
                var c = sqlWhereExpression[i];
                if (bInDoubleQuotes)
                {
                    if (c == '\"')
                    {
                        bInDoubleQuotes = false;
                    }

                    resultWhere.Append(c);
                    continue;
                }
                else if (bInQuotes)
                {
                    if (c == '\'')
                    {
                        bInQuotes = false;
                    }

                    resultWhere.Append(c);
                    continue;
                }
                else
                {
                    if (c == '\"')
                    {
                        bInDoubleQuotes = true;
                        resultWhere.Append(c);
                    }
                    else if (c == '\\')
                    {
                        bInQuotes = true;
                        resultWhere.Append(c);
                    }
                    else if (c != '@')
                    {
                        resultWhere.Append(c);
                    }
                    else
                    {
                        var parname = new StringBuilder();
                        var j = i;
                        while (j <= sqlWhereExpression.Length)
                        {
                            if (j == sqlWhereExpression.Length)
                            {
                                c = ' ';
                            }
                            else
                            {
                                c = sqlWhereExpression[j];
                            }

                            if (c == '@' || c == '_' || char.IsLetterOrDigit(c))
                            {
                                parname.Append(c);
                            }
                            else
                            {
                                var par = parname.ToString();
                                var bParameterFound = false;
                                foreach (var okey in parameters.Keys)
                                {
                                    var skey = string.Empty + okey;
                                    if (skey.Length > 0 && skey[0] != '@')
                                    {
                                        skey = "@" + skey;
                                    }

                                    if (skey.Equals(par, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        bParameterFound = true;
                                        resultWhere.Append("?");
                                        if (parameters[okey] == null || parameters[okey] == DBNull.Value)
                                        {
                                            listPars.Add(DBNull.Value);
                                        }
                                        else
                                        {
                                            listPars.Add(DbObject.ToDbObject(parameters[okey]));
                                        }

                                        i = j - 1;
                                        break;
                                    }
                                }

// se nÃ£o encontrou o parÃ¢metro, dispara exceÃ§Ã£o
                                if (!bParameterFound)
                                {
                                    throw new NotSupportedException("parÃ¢metro " + par + " nÃ£o foi definido");
                                }

                                break;
                            }

                            j++;
                        }
                    }
                }
            }

            sqlWhere = resultWhere.ToString();
            parValues = listPars.ToArray();
        }

        private TDataTable CreateNewTable()
        {
            var tabType = this.DataTableType;
            if (tabType == null)
            {
                return null;
            }
            else
            {
                var tab = tabType.Assembly.CreateInstance(tabType.FullName) as TDataTable;
                return tab;
            }
        }

        private IEnumerable ExecuteSelectMethod(IOrderedDictionary parameters, DataSourceSelectArguments arguments)
        {
            if (this.SelectMethodInfo != null)
            {
                // executa query
                var conn = this.Connection;
                conn.Open(true);
                try
                {
                    var selArgs = new SelectArguments();
                    selArgs.MaximumRows = arguments.MaximumRows;
                    selArgs.RowCountOnly = arguments.RetrieveTotalRowCount;
                    selArgs.SortExpression = arguments.SortExpression;
                    selArgs.StartRowIndex = arguments.StartRowIndex;

                    TDataTable tab = null;
                    var ok = false;
                    tab = this.CreateNewTable();

                    object ret = null;
                    if (this._selectMethodInfo.MethodInfo.IsStatic)
                    {
                        ret = this._selectMethodInfo.Invoke(null, parameters, conn, selArgs);
                    }
                    else if (tab != null)
                    {
                        tab.Clear();
                        ret = this._selectMethodInfo.Invoke(tab, parameters, conn, selArgs);
                    }

                    ok = conn.ErrorCount == 0;
                    if (ok == false)
                    {
                        this.ThrowErrors(this.Connection);
                        return tab.DefaultView;
                    }
                    else
                    {
                        return tab.DefaultView;
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                return null;
            }
        }

        private IEnumerable ExecuteSqlSelect(IOrderedDictionary parameters, DataSourceSelectArguments arguments)
        {
            // executa query
            var conn = this.Connection;
            conn.Open(true);
            try
            {
                // monta Where
                var where = string.Empty;
                var pars = new DbObject[] { };
                this.ConvertWhere(this._sqlWhereExpression, parameters, out where, out pars);

                TDataTable tab = null;
                var ok = false;
                tab = this.CreateNewTable();
                string[] columns = null;
                if (this._sqlColumns.Trim() == string.Empty)
                {
                    columns = new string[] { };
                }
                else
                {
                    columns = this._sqlColumns.Split(new[] { ',' });
                }

                if (arguments.RetrieveTotalRowCount)
                {
                    arguments.TotalRowCount = tab.Get(conn, columns, where, pars, string.Empty, 0, 16000, true, true);
                }

                tab.Clear();
                tab.Get(conn, columns, where, pars, this._sqlWhereOrder, arguments.StartRowIndex, arguments.MaximumRows, true, false);
                ok = conn.ErrorCount == 0;
                if (ok == false)
                {
                    this.ThrowErrors(this.Connection);
                    return tab.DefaultView;
                }
                else
                {
                    return tab.DefaultView;
                }
            }
            finally
            {
                conn.Close();
            }
        }

        private TTableDataSourceMethod GetMethodInfo(Type type, string methodName, IDictionary allParameters)
        {
            var methods = type.GetMethods(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
            MethodInfo methodInfo = null;
            ParameterInfo[] methodParamInfo = null;
            var count = allParameters.Count;
            foreach (var currMethod in methods)
            {
                if (string.Equals(methodName, currMethod.Name, StringComparison.OrdinalIgnoreCase) && !currMethod.IsGenericMethodDefinition)
                {
                    var CurrMethodParams = currMethod.GetParameters();
                    if (CurrMethodParams.Length <= count + 2 && CurrMethodParams.Length >= count)
                    {
                        var parameterNamesMismatch = false;
                        var nConnections = 0;
                        var nSelectArgs = 0;
                        foreach (var currMethodParam in CurrMethodParams)
                        {
                            if (currMethodParam.ParameterType == typeof (TConnection))
                            {
                                nConnections++;
                                if (nConnections > 1)
                                {
                                    parameterNamesMismatch = true;
                                    break;
                                }
                            }
                            else if (currMethodParam.ParameterType == typeof (SelectArguments))
                            {
                                nSelectArgs++;
                                if (nSelectArgs > 1)
                                {
                                    parameterNamesMismatch = true;
                                    break;
                                }
                            }
                            else if (!allParameters.Contains(currMethodParam.Name))
                            {
                                parameterNamesMismatch = true;
                                break;
                            }
                        }

                        if (!parameterNamesMismatch)
                        {
                            methodInfo = currMethod;
                            methodParamInfo = CurrMethodParams;
                        }
                    }
                }
            }

            if (methodInfo == null)
            {
                return null;
            }
            else
            {
                return new TTableDataSourceMethod(type, methodInfo);
            }
        }

        void IStateManager.LoadViewState(object savedState)
        {
            this.LoadViewState(savedState);
        }

        object IStateManager.SaveViewState()
        {
            return this.SaveViewState();
        }

        private void SelectMethodParametersChangedEventHandler(object o, EventArgs e)
        {
            this.OnDataSourceViewChanged(EventArgs.Empty);
        }

        private void SelectParametersChangedEventHandler(object o, EventArgs e)
        {
            this.OnDataSourceViewChanged(EventArgs.Empty);
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

        void IStateManager.TrackViewState()
        {
            this.TrackViewState();
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        internal class TTableDataSourceMethod
        {
            internal MethodInfo MethodInfo;

            internal string[] ParameterNames;

            internal Type[] ParameterTypes;

            internal Type Type;

            internal string _connectionParam = string.Empty;

            internal string _selargParam = string.Empty;

            internal TTableDataSourceMethod(Type type, MethodInfo methodInfo)
            {
                this.Type = type;
                this.MethodInfo = methodInfo;
                var parInfos = methodInfo.GetParameters();
                this.ParameterNames = new string[parInfos.Length];
                this.ParameterTypes = new Type[parInfos.Length];
                for (var i = 0; i < parInfos.Length; i++)
                {
                    this.ParameterNames[i] = parInfos[i].Name;
                    this.ParameterTypes[i] = parInfos[i].ParameterType;
                    if (parInfos[i].ParameterType == typeof (TConnection))
                    {
                        this._connectionParam = parInfos[i].Name;
                    }

                    if (parInfos[i].ParameterType == typeof (SelectArguments))
                    {
                        this._selargParam = parInfos[i].Name;
                    }
                }
            }

            internal object Invoke(object o, IDictionary pars, TConnection conn, SelectArguments selArgs)
            {
                var values = new object[this.ParameterNames.Length];
                for (var i = 0; i < this.ParameterNames.Length; i++)
                {
                    if (this.ParameterNames[i] == this._connectionParam)
                    {
                        values[i] = conn;
                    }
                    else if (this.ParameterNames[i] == this._selargParam)
                    {
                        values[i] = selArgs;
                    }
                    else if (pars.Contains(this.ParameterNames[i]))
                    {
                        var parValue = pars[this.ParameterNames[i]];
                        if (parValue == DBNull.Value || (parValue is DbObject && ((DbObject)parValue).IsNull))
                        {
                            parValue = null;
                        }

                        if (this.ParameterTypes[i] == typeof (Techne.Number))
                        {
                            values[i] = parValue == null ? DBNull.Value : parValue;
                        }
                        else if (this.ParameterTypes[i] == typeof (Techne.VarChar))
                        {
                            values[i] = parValue == null ? DBNull.Value : (Techne.VarChar)parValue.ToString();
                        }
                        else if (this.ParameterTypes[i] == typeof (Techne.Date))
                        {
                            values[i] = parValue == null ? DBNull.Value : parValue;
                        }
                        else if (this.ParameterTypes[i] == typeof (decimal?))
                        {
                            if (parValue == null)
                            {
                                values[i] = null;
                            }
                            else
                            {
                                values[i] = Convert.ToDecimal(parValue);
                            }
                        }
                        else if (this.ParameterTypes[i] == typeof (decimal))
                        {
                            values[i] = Convert.ToDecimal(parValue);
                        }
                        else if (this.ParameterTypes[i] == typeof (string))
                        {
                            values[i] = parValue == null ? null : parValue.ToString();
                        }
                        else if (this.ParameterTypes[i] == typeof (DateTime?))
                        {
                            if (parValue == null)
                            {
                                values[i] = null;
                            }
                            else
                            {
                                values[i] = Convert.ToDateTime(parValue);
                            }
                        }
                        else if (this.ParameterTypes[i] == typeof (DateTime))
                        {
                            values[i] = Convert.ToDateTime(parValue);
                        }
                        else if (parValue is DbObject)
                        {
                            values[i] = ((DbObject)parValue).ToObject();
                        }
                        else
                        {
                            values[i] = parValue;
                        }
                    }
                    else
                    {
                        values[i] = DBNull.Value;
                    }
                }

                return this.MethodInfo.Invoke(o, values);
            }
        }
    }
}