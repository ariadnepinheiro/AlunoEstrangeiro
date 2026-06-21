using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Techne.Data;

namespace Techne.Controls
{
    [
        Designer(typeof (StringListDesigner))
    ]
    internal class StringList : WebControl, IDepender
    {
        private const string CaptionCssClass_Def = "";

        private const string Caption_Def = "";

        private const string ItemFormat_Def = "{0}";

        private const string Separator_Def = ", ";

        private ChangedEventHandler changedHandler;

        private object dataSource;

        public StringList()
        {
            this.Caption = Caption_Def;
            this.CaptionCssClass = CaptionCssClass_Def;
            this.ColumnNames = null;
            this.Connection = string.Empty;
            this.DataMember = string.Empty;
            this.DataSource = null;
            this.ViewState["Dependees"] = null;
            this.ViewState["Init"] = false;
            this.ItemFormat = ItemFormat_Def;
            this.Separator = Separator_Def;
            this.SqlOrder = string.Empty;
            this.SqlSelect = string.Empty;
            this.SqlWhere = string.Empty;
            this.SqlWhereValues = new DbObject[0];
            this.ViewState["Text"] = string.Empty;
        }

        [
            Category("Techne"), 
            DefaultValue(Caption_Def), 
        ]
        public string Caption
        {
            get
            {
                return (string)this.ViewState["Caption"];
            }

            set
            {
                this.ViewState["Caption"] = value == null ? Caption_Def : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(CaptionCssClass_Def), 
        ]
        public string CaptionCssClass
        {
            get
            {
                return (string)this.ViewState["CaptionCssClass"];
            }

            set
            {
                this.ViewState["CaptionCssClass"] = value == null ? CaptionCssClass_Def : value;
            }
        }

        [
            Category("Techne - Modo 1"), 
            DefaultValue(null), 
            Description("Nomes das colunas a serem mostradas para cada um dos registros. " +
                        "O formato de cada item (registro) é dado pela propriedade ItemFormat."), 
            TypeConverter(typeof (StringArrayConverter)), 
        ]
        public string[] ColumnNames
        {
            get
            {
                return (string[])this.ViewState["ColumnNames"];
            }

            set
            {
                this.ViewState["ColumnNames"] = value == null ? new string[0] : value;
            }
        }

        [
            Category("Techne - Modo 2"), 
            DefaultValue(""), 
            Description("String de conexão ou chave contida na seção <techne/database> do arquivo web.config " +
                        "a ser usada para executar o comando SqlQuery. ATENÇÃO: esta propriedade é case-sensitive.")
        ]
        public string Connection
        {
            get
            {
                return (string)this.ViewState["Connection"];
            }

            set
            {
                this.ViewState["Connection"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Techne - Modo 1"), 
            DefaultValue(""), 
            TypeConverter(typeof (DataMemberConverter)), 
        ]
        public string DataMember
        {
            get
            {
                return (string)this.ViewState["DataMember"];
            }

            set
            {
                this.ViewState["DataMember"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Modo 1"), 
            DefaultValue(null), 
            Description("Juntamente com a propriedade DataMember, informa a tabela que deverá conter os registros " +
                        "a serem mostrados pelo controle. Opcionalmente pode-se manter esta propriedade vazia e " +
                        "informar a propriedade SqlSelect."), 
        ]
        public object DataSource
        {
            get
            {
                return this.dataSource;
            }

            set
            {
                if (this.dataSource != null &&
                    this.dataSource as IListSource == null && this.dataSource as IEnumerable == null)
                {
                    throw new ArgumentException("Tipo inválido para a propriedade DataSource");
                }

                this.dataSource = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(ItemFormat_Def), 
            Description("Formato de cada um dos itens (registros). " +
                        "Os elementos a serem apresentados para cada um dos itens são relacionados na propriedade ColumnNames. " +
                        "A sintaxe utilizada é a mesma do método string.Format()."), 
        ]
        public string ItemFormat
        {
            get
            {
                return (string)this.ViewState["ItemFormat"];
            }

            set
            {
                this.ViewState["ItemFormat"] = value == null ? ItemFormat_Def : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(Separator_Def), 
            Description("Separador de itens (registros). Ex: \"<br>\"."), 
        ]
        public string Separator
        {
            get
            {
                return (string)this.ViewState["Separator"];
            }

            set
            {
                this.ViewState["Separator"] = value == null ? Separator_Def : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Cláusula ORDER BY do select realizado pelo controle"), 
        ]
        public string SqlOrder
        {
            get
            {
                return (string)this.ViewState["SqlOrder"];
            }

            set
            {
                this.ViewState["SqlOrder"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Modo 2"), 
            DefaultValue(""), 
            Description("Select que trará os registros a serem mostrados pelo controle. " +
                        "Será utilizado somente se a propriedade DataSource não for informada."), 
        ]
        public string SqlSelect
        {
            get
            {
                return (string)this.ViewState["SqlSelect"];
            }

            set
            {
                this.ViewState["SqlSelect"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Permite especificar os registros a serem listados.")
        ]
        public string SqlWhere
        {
            get
            {
                return (string)this.ViewState["SqlWhere"];
            }

            set
            {
                var newValue = value == null ? string.Empty : value.Trim();
                this.ViewState["SqlWhere"] = newValue;

                // Força o recálculo de Dependees
                this.ViewState["Dependees"] = null;
            }
        }

        [
            Browsable(false)
        ]
        public DbObject[] SqlWhereValues
        {
            get
            {
                return DbObject.ToDbObjectArray((object[])this.ViewState["SqlWhereValues"]);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                this.ViewState["SqlWhereValues"] = DbObject.ToObjectArray(value);
            }
        }

        [
            Browsable(false), 
        ]
        public string Text
        {
            get
            {
                return (string)this.ViewState["Text"];
            }
        }

        ChangedEventHandler IDepender.ChangedHandler
        {
            get
            {
                if (this.changedHandler == null)
                {
                    this.changedHandler = new ChangedEventHandler(this.Requery);
                }

                return this.changedHandler;
            }
        }

        private string[] Dependees
        {
            get
            {
                if (this.ViewState["Dependees"] == null)
                {
                    try
                    {
                        this.ViewState["Dependees"] = DependerLib.GetDependees(this);
                    }
                    catch (Exception exc)
                    {
                        throw new InvalidOperationException("Existe algum erro na propriedade " + this.UniqueID + ".SqlWhere.", exc);
                    }
                }

                return (string[])this.ViewState["Dependees"];
            }
        }

        string[] IDepender.Dependees
        {
            get
            {
                return this.Dependees;
            }
        }

        /// <summary>
        ///   System.Web.UI.DataSourceHelper.GetResolvedDataSource()
        /// </summary>
        public static IEnumerable GetResolvedDataSource(object dataSource, string dataMember)
        {
            if (dataSource == null)
            {
                return null;
            }

            var listSource = dataSource as IListSource;
            if (listSource != null)
            {
                var list = listSource.GetList();
                if (!listSource.ContainsListCollection)
                {
                    return list;
                }

                if (list != null && list as ITypedList != null)
                {
                    var typedList = (ITypedList)list;
                    var propertyCollection = typedList.GetItemProperties(new PropertyDescriptor[0]);
                    if (propertyCollection != null && propertyCollection.Count != 0)
                    {
                        PropertyDescriptor propertyDescriptor = null;
                        if (dataMember == null || dataMember.Length == 0)
                        {
                            propertyDescriptor = propertyCollection[0];
                        }
                        else
                        {
                            propertyDescriptor = propertyCollection.Find(dataMember, true);
                        }

                        if (propertyDescriptor != null)
                        {
                            var propertyValue = propertyDescriptor.GetValue(list[0]);
                            if (propertyValue is IEnumerable)
                            {
                                return (IEnumerable)propertyValue;
                            }
                        }

                        throw new HttpException("ListSource_Missing_DataMember");
                    }

                    throw new HttpException("ListSource_Without_DataMembers");
                }
            }

            if (dataSource as IEnumerable != null)
            {
                return (IEnumerable)dataSource;
            }

            return null;
        }

        public static void RenderStringList(HtmlTextWriter writer, 
                                            Unit width, string caption, string captionCssClass, string text)
        {
            var table = new HtmlTable();
            table.Width = width.ToString();

            var row = new HtmlTableRow();
            table.Rows.Add(row);

            HtmlTableCell cell;

            if (caption.Length > 0)
            {
                cell = new HtmlTableCell();
                cell.VAlign = "Top";
                cell.Width = "0pt";
                var lblCaption = new Label();
                lblCaption.Text = caption;
                lblCaption.CssClass = captionCssClass;
                cell.Controls.Add(lblCaption);
                row.Cells.Add(cell);
            }
            {
                cell = new HtmlTableCell();
                cell.VAlign = "Bottom";
                var lblText = new Label();
                lblText.Text = text;
                cell.Controls.Add(lblText);
                row.Cells.Add(cell);
            }

            table.RenderControl(writer);
        }

        public override void DataBind()
        {
            this.OnDataBinding(EventArgs.Empty);
        }

        /// <summary>
        ///   Refaz a query.
        /// </summary>
        public void Requery()
        {
            this.DataBind();

            if (this.DataSource != null)
            {
                this.RequeryFromDataSource();
            }
            else
            {
                this.RequeryFromSqlSelect();
            }
        }

        protected override void OnLoad(EventArgs args)
        {
            if (!(bool)this.ViewState["Init"])
            {
                this.Requery();
                this.ViewState["Init"] = true;
            }

            // Detecta alterações nos controles dos quais este controle depende
            DependerLib.RegisterDepender(this);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderStringList(writer, this.Width, this.Caption, this.CaptionCssClass, this.Text);
        }

        /// <summary>
        ///   Preenche uma TDataTable com registros. Devolve array de ponteiros para os registros adicionados.
        /// </summary>
        /// <param name = "control">Controle que utilizará os registros</param>
        /// <param name = "table">TDataTable a ser preenchido</param>
        /// <param name = "columnNames">Nomes das colunas que deverão ser trazidas (somente elas farão parte da query realizada no banco)</param>
        /// <param name = "where">Expressão para restrição da query. Pode referenciar controles (delimitados por '#') e campos (delimitados por '$').</param>
        /// <param name = "whereValues">Valores dos parâmetros indicados na cláusula where por '?'</param>
        /// <param name = "order">Colunas para a cláusula ORDER BY</param>
        private static DataRow[] DataTableGet(Control control, 
                                              TDataTable table, 
                                              string[] columnNames, 
                                              string where, DbObject[] whereValues, 
                                              string order)
        {
            var cn = table.CreateConnection();

            // Trata as propriedades SqlWhere e SqlWhereValues
            var sqlWhere = where;
            var sqlWhereValues = whereValues;
            try
            {
                TControl.GetSqlWhere(table, null, control.NamingContainer, cn.Rdbms, true, ref sqlWhere, ref sqlWhereValues);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException("Possível erro na cláusula " + control.ID + ".SqlWhere: " + where, exc);
            }

            // Faz uma lista dos DataRow's que estão na tabela antes do Get()
            var oldRows = new ArrayList();
            foreach (DataRow row in table.Rows)
            {
                oldRows.Add(row);
            }

            // Realiza o Get()
            int count;
            try
            {
                count = table.Get(
                    cn, 
                    columnNames, 
                    sqlWhere, sqlWhereValues, order, 
                    0, 0, 
                    false, false
                    );
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException("Possível erro na cláusula " + control.ID + ".SqlWhere: " + where, exc);
            }

            // Faz uma lista dos DataRow's adicionados pelo Get()
            var newRows = new ArrayList();
            foreach (DataRow row in table.Rows)
            {
                if (!oldRows.Contains(row))
                {
                    newRows.Add(row);
                }
            }

            return (DataRow[])newRows.ToArray(typeof (DataRow));
        }

        private void Requery(object sender, ChangedEventArgs args)
        {
            this.Requery();
        }

        /// <summary>
        ///   Inicializa o field 'text' a partir do TDataTable determinado através das propriedades
        ///   DataSource e DataMember. O TDataTable é preenchido com os registros, restringidos pelas
        ///   propriedades SqlWhere e SqlWhereValues.
        /// </summary>
        private void RequeryFromDataSource()
        {
            

            TDataTable table = null;
            var ds = this.DataSource as TDataSet;
            if (ds != null)
            {
                if (this.DataMember.Length > 0)
                {
                    table = ds.Tables[this.DataMember] as TDataTable;
                }
            }
            else if (this.DataSource is TDataTable)
            {
                table = (TDataTable)this.DataSource;
            }

            if (table != null)
            {
                DataTableGet(this, table, this.ColumnNames, this.SqlWhere, this.SqlWhereValues, this.SqlOrder);
            }

            

            #region Inicializa o field 'text' a partir do IEnumerable definido por DataSource/DataMember

            var enumerable = GetResolvedDataSource(this.dataSource, this.DataMember);
            var items = new ArrayList();

            if (enumerable != null)
            {
                var typedList = enumerable as ITypedList;
                if (typedList == null)
                {
                    foreach (var enumerableItem in enumerable)
                    {
                        items.Add(string.Format(this.ItemFormat, new object[] { enumerableItem.ToString() }));
                    }
                }
                else
                {
                    var collection = typedList.GetItemProperties(null);

                    foreach (var enumerableItem in enumerable)
                    {
                        var values = new ArrayList();
                        foreach (var columnName in this.ColumnNames)
                        {
                            values.Add(collection[columnName].GetValue(enumerableItem));
                        }

                        items.Add(string.Format(this.ItemFormat, (DbObject[])values.ToArray(typeof (DbObject))));
                    }
                }
            }

            this.ViewState["Text"] = StrLib.EnumerableToStr(items, this.Separator);

            #endregion
        }

        /// <summary>
        ///   Inicializa o field 'text' com base na propriedade SqlSelect.
        /// </summary>
        private void RequeryFromSqlSelect()
        {
            TConnection cn;

            

            string connStr;
            try
            {
                connStr = ConnectionList.GetConnectionString(this.Connection);
            }
            catch (ArgumentException)
            {
                connStr = this.Connection;
            }

            cn = new TConnection(connStr);

            

            cn.Open();

            ArrayList items;
            try
            {
                string sql;

                #region sql = ...;

                var sqlWhere = this.SqlWhere;
                var sqlWhereValues = this.SqlWhereValues;

                TControl.GetSqlWhere(this.NamingContainer, cn.Rdbms, true, ref sqlWhere, ref sqlWhereValues);

                sql = this.SqlSelect +
                      (sqlWhere.Length == 0 ? string.Empty : " WHERE " + sqlWhere) +
                      (this.SqlOrder.Length == 0 ? string.Empty : " ORDER BY " + this.SqlOrder);

                #endregion

                items = new ArrayList();

                #region Preenche items com as strings formatadas para cada um dos itens do select

                TDataReader rd;
                try
                {
                    rd = cn.CreateDataReader(sql, sqlWhereValues);
                }
                catch (OleDbException exc)
                {
                    throw new Exception("ExecuteReader(): " + sql + ": " + exc.Message);
                }

                try
                {
                    while (rd.Read())
                    {
                        var values = new ArrayList();
                        for (var i = 0; i < rd.FieldCount; i++)
                        {
                            values.Add(rd.GetValue(i));
                        }

                        items.Add(string.Format(this.ItemFormat, (DbObject[])values.ToArray(typeof (DbObject))));
                    }
                }
                finally
                {
                    if (!rd.IsClosed)
                    {
                        rd.Close();
                    }
                }

                #endregion
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            this.ViewState["Text"] = StrLib.EnumerableToStr(items, this.Separator);
        }
    }

    internal class StringListDesigner : ControlDesigner, IDataSourceProvider
    {
        public string DataSource
        {
            get
            {
                var binding = this.DataBindings["DataSource"];
                if (binding != null)
                {
                    return binding.Expression;
                }

                return string.Empty;
            }

            set
            {
                if (value == null || value.Length == 0)
                {
                    this.DataBindings.Remove("DataSource", false);
                }
                else
                {
                    var binding = this.DataBindings["DataSource"];

                    if (binding == null)
                    {
                        binding = new DataBinding("DataSource", typeof (IEnumerable), value);
                        this.DataBindings.Add(binding);
                    }
                    else
                    {
                        binding.Expression = value;
                        this.DataBindings.Remove(binding); // para forçar evento Changed
                        this.DataBindings.Add(binding);
                    }
                }
            }
        }

        public static int GetLastFormatSpecification(string formatString)
        {
            var max = -1;
            var pos = 0;

            while (true)
            {
                pos = formatString.IndexOf('{', pos);
                if (pos < 0)
                {
                    return max;
                }

                pos++;

                var strNum = string.Empty;
                while (pos < formatString.Length && char.IsDigit(formatString, pos))
                {
                    strNum += formatString[pos++];
                }

                var num = int.Parse(strNum);
                if (num > max)
                {
                    max = num;
                }
            }
        }

        public override string GetDesignTimeHtml()
        {
            var stringList = this.Component as StringList;
            if (stringList == null)
            {
                return "[StringList]";
            }

            var caption = stringList.Caption;
            var captionCssClass = stringList.CaptionCssClass;
            if (caption.Length == 0)
            {
                caption = "[" + stringList.ID + "]";
                captionCssClass = string.Empty;
            }

            string text;
            {
                var max = GetLastFormatSpecification(stringList.ItemFormat) + 1;
                var items = new ArrayList();
                var args = new string[max];
                for (var row = 0; row < 3; row++)
                {
                    for (var col = 0; col < max; col++)
                    {
                        args[col] = string.Format("Row{0}[{1}]", row, col < stringList.ColumnNames.Length ? stringList.ColumnNames[col] : "Col" + col);
                    }

                    items.Add(string.Format(stringList.ItemFormat, args));
                }

                text = StrLib.EnumerableToStr(items, stringList.Separator);
            }

            var writer = new HtmlTextWriter(new StringWriter());
            StringList.RenderStringList(writer, stringList.Width, caption, captionCssClass, text);
            return writer.InnerWriter.ToString();
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            PropertyDescriptor prop;

            base.PreFilterProperties(properties);

            

            prop = TypeDescriptor.CreateProperty(
                this.GetType(), 
                "DataSource", typeof (string), 
                new CategoryAttribute("Techne - Modo 1"), 
                new TypeConverterAttribute(typeof (DataSourceConverter)), 
                new BindableAttribute(true), 
                new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)
                );

            if (properties.Contains("DataSource"))
            {
                properties.Remove("DataSource");
            }

            properties.Add("DataSource", prop);

            
        }

        IEnumerable IDataSourceProvider.GetResolvedSelectedDataSource()
        {
            throw new NotImplementedException();
        }

        object IDataSourceProvider.GetSelectedDataSource()
        {
            var binding = this.DataBindings["DataSource"];
            if (binding == null)
            {
                return null;
            }

            var dataSource = binding.Expression;
            if (dataSource == null)
            {
                return null;
            }

            var componentSite = this.Component.Site;
            if (componentSite == null)
            {
                return null;
            }

            var container = (IContainer)componentSite.GetService(typeof (IContainer));
            if (container == null)
            {
                return null;
            }

            var comp = container.Components[dataSource];

            return comp;
        }
    }
}