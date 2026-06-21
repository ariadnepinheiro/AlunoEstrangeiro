using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls.Design;
using Techne.Data;
using Techne.Library;
using Techne.Library.Sql.Structure;

namespace Techne.Controls
{
    [ToolboxData("<{0}:TSearchBase runat=server></{0}:TSearchBase>"), Designer(typeof (TSearchBaseDesigner))]
    internal abstract class TSearchBase : TTextBox, IDepender, INamingContainer, IPostBackEventHandler
    {
        // (em ordem alfabética)
        private const int ArgumentColumns_Def = 70;

        private const string ArgumentControlName = "__Argument__";

        private const string ButtonCssClass_Def = "TSearchButton";

        private const string ClickToSearchMsg_Def = "Clique no botão <B>Buscar</B> ao lado";

        private const string GridCssClass_Def = "TSearchGrid";

        private const int GridPageSize_Def = 10;

        private const string NotFoundMsg_Def = "Nenhum registro encontrado";

        private const string SearchButtonImageUrl_Def = "~/images/bt_search.gif";

        private const string SearchButtonToolTip_Def = "Search";

        private const string Separator_Def = " - ";

        private readonly TSearchColumnCollection gridColumns;

        /// <summary>
        ///   Indica se houve alteração no valor do controle que representa a descrição
        ///   após um postback. É alterado em LoadPostData().
        /// </summary>
        private bool ArgumentChanged;

        /// <summary>
        ///   Indica se houve alteração no valor do controle que representa a chave (controle base)
        ///   após um postback. É alterado em LoadPostData().
        /// </summary>
        private bool KeyChanged;

        private ChangedEventHandler changedHandler;

        private string[] dependees;

        private TDataGrid grd;

        private ImageLink hypSearch;

        private Unit pvGridWidth = Unit.Pixel(500);

        private NameDbObjectCollection pvPersistedColumnValues;

        private string pvSeparator;

        public TSearchBase()
        {
            

            this.ArgumentColumns = ArgumentColumns_Def;
            this.ArgumentText = string.Empty;
            this.ButtonCssClass = ButtonCssClass_Def;
            this.ClickToSearchMsg = ClickToSearchMsg_Def;
            this.Connection = string.Empty;
            this.GridCssClass = GridCssClass_Def;
            this.GridPageSize = GridPageSize_Def;
            this.GridVisible = false;
            this.NotFoundMsg = NotFoundMsg_Def;
            this.SearchButtonImageUrl = SearchButtonImageUrl_Def;
            this.SearchButtonToolTip = SearchButtonToolTip_Def;
            this.Separator = Separator_Def;

            

            this.BaseAlternateKeys = new string[0];
            this.BaseArgument = string.Empty;
            this.BaseKey = string.Empty;
            this.BaseSqlOrder = string.Empty;
            this.BaseSqlSelect = new SqlSelect();
            this.BaseSqlWhere = string.Empty;
            this.BaseWhereValues = new DbObject[0];

            this.PersistedColumnNames = new string[0];
            this.PersistedColumnValues = new DbObject[0];

            this.gridColumns = new TSearchColumnCollection();
        }

        /// <summary>
        ///   Atribui um valor ao controle, buscando a descrição correspondente.
        /// </summary>
        public override DbObject DBValue
        {
            get
            {
                return base.DBValue;
            }

            set
            {
                base.DBValue = value;
                if (!this.SettingState && !value.IsNull)
                {
                    this.GetAndBind(false);
                }
            }
        }

        /// <summary>
        ///   Número de caracteres da coluna de argumentos, para dimensionar o tamanho do controle de argumentos
        /// </summary>
        [
            Category("Appearance"), 
            DefaultValue(ArgumentColumns_Def), 
            Description("Número de caracteres da coluna de argumentos, para dimensionar o tamanho do controle de argumentos")
        ]
        public int ArgumentColumns
        {
            get
            {
                return (int)this.ViewState["ArgumentColumns"];
            }

            set
            {
                this.ViewState["ArgumentColumns"] = value;
            }
        }

        [
            Browsable(false), 
            DefaultValue("")
        ]
        public string ArgumentText
        {
            get
            {
                return (string)this.ViewState["ArgumentText"];
            }

            set
            {
                this.ViewState["ArgumentText"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Appearance"), 
            DefaultValue(ButtonCssClass_Def)
        ]
        public string ButtonCssClass
        {
            get
            {
                return (string)this.ViewState["ButtonCssClass"];
            }

            set
            {
                this.ViewState["ButtonCssClass"] = value == null ? string.Empty : value;
            }
        }

        /// <summary>
        ///   Mensagem a ser mostrada quando o usuário informou a descrição mas não clicou em Buscar
        /// </summary>
        [
            DefaultValue(ClickToSearchMsg_Def), 
            Category("Techne - Messages"), 
            Description("Mensagem a ser mostrada quando o usuário informou a descrição mas não clicou em Buscar")
        ]
        public string ClickToSearchMsg
        {
            get
            {
                return (string)this.ViewState["ClickToSearchMsg"];
            }

            set
            {
                this.ViewState["ClickToSearchMsg"] = value == null ? string.Empty : value;
            }
        }

        /// <summary>
        ///   Nome da conexão com banco de dados
        /// </summary>
        [
            DefaultValue(""), 
            Category("Techne"), 
            Description("Nome da conexão com banco de dados")
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
            Category("Appearance"), 
            DefaultValue("")
        ]
        public string GridColumnReadOnlyCssClass
        {
            get
            {
                return (string)this.ViewState["GridColumnReadOnlyCssClass"];
            }

            set
            {
                this.ViewState["GridColumnReadOnlyCssClass"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Appearance - Grid"), 
            DefaultValue(GridCssClass_Def)
        ]
        public string GridCssClass
        {
            get
            {
                return (string)this.ViewState["GridCssClass"];
            }

            set
            {
                this.ViewState["GridCssClass"] = value == null ? string.Empty : value;
            }
        }

        /// <summary>
        ///   Número de registros que serão mostrados numa página da grid de busca. Caso seja 0, não pagina.
        /// </summary>
        [
            Category("Appearance - Grid"), 
            DefaultValue(GridPageSize_Def), 
            Description("Número de registros que serão mostrados numa página da grid de busca. Caso seja 0, não pagina.")
        ]
        public int GridPageSize
        {
            get
            {
                return (int)this.ViewState["GridPageSize"];
            }

            set
            {
                this.ViewState["GridPageSize"] = value > 0 ? value : 0;
            }
        }

        [
            Category("Appearance - Grid"), 
            DefaultValue("500px"), 
        ]
        public Unit GridWidth
        {
            get
            {
                return this.pvGridWidth;
            }

            set
            {
                this.pvGridWidth = value;
            }
        }

        /// <summary>
        ///   Indica se DBValue pertence à lista de valores válidos do controle definida
        ///   pelas propriedades SqlSelect e SqlWhere.
        /// </summary>
        [
            Browsable(false), 
        ]
        public bool IsValidDBValue
        {
            get
            {
                return this.PersistedColumnValues.Length > 0;
            }
        }

        /// <summary>
        ///   Mensagem a ser mostrada quando nenhum registro é encontrado
        /// </summary>
        [
            DefaultValue(NotFoundMsg_Def), 
            Category("Techne - Messages"), 
            Description("Mensagem a ser mostrada quando nenhum registro é encontrado")
        ]
        public string NotFoundMsg
        {
            get
            {
                return (string)this.ViewState["NotFoundMsg"];
            }

            set
            {
                this.ViewState["NotFoundMsg"] = value == null ? string.Empty : value;
            }
        }

        [
            DefaultValue(SearchButtonImageUrl_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
            Category("Appearance")
        ]
        public string SearchButtonImageUrl
        {
            get
            {
                return (string)this.ViewState["SearchButtonImageUrl"];
            }

            set
            {
                this.ViewState["SearchButtonImageUrl"] = value == null ? string.Empty : value;
            }
        }

        [
            DefaultValue(SearchButtonToolTip_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
            Category("Appearance")
        ]
        public string SearchButtonToolTip
        {
            get
            {
                return (string)this.ViewState["SearchButtonToolTip"];
            }

            set
            {
                this.ViewState["SearchButtonToolTip"] = value == null ? string.Empty : value;
            }
        }

        /// <summary>
        ///   Texto que separa o código e a descrição no modo View
        /// </summary>
        [
            DefaultValue(Separator_Def), 
            Category("Appearance"), 
            Description("Texto que separa o código e a descrição no modo View")
        ]
        public string Separator
        {
            get
            {
                return this.pvSeparator;
            }

            set
            {
                this.pvSeparator = value;
            }
        }

        [Bindable(true)]
        [Browsable(false)]
        public object Value
        {
            get
            {
                return this.DBValue == null ? null : this.DBValue.ToObject();
            }

            set
            {
                this.DBValue = DbObject.ToDbObject(value);
            }
        }

        /// <summary>
        ///   Devolve BaseArgument se ele for informado, caso contrário devolve a segunda coluna de BaseSqlSelect.
        ///   Se BaseSqlSelect também não foi informado ou possui apenas uma coluna, dá exception.
        /// </summary>
        protected string ArgumentCol
        {
            get
            {
                if (this.BaseArgument.Length == 0)
                {
                    if (this.BaseSqlSelect.Columns.Count < 2)
                    {
                        throw new InvalidOperationException("A propriedade Argument não foi informada ou a propriedade SqlSelect possui menos de duas colunas.");
                    }
                    else
                    {
                        return this.BaseSqlSelect.Columns[1].Id;
                    }
                }
                else
                {
                    return this.BaseArgument;
                }
            }
        }

        protected DbObject ArgumentValue
        {
            get
            {
                return DbObject.ToDbObject(this.ViewState["ArgumentValue"]);
            }

            set
            {
                this.ViewState["ArgumentValue"] = value.ToObject();
            }
        }

        protected string[] BaseAlternateKeys
        {
            get
            {
                return (string[])this.ViewState["BaseAlternateKeys"];
            }

            set
            {
                if (value == null)
                {
                    value = new string[0];
                }

                // Faz Trim() de cada coluna informada na lista, removendo as ocorrências de Key
                var key = this.BaseKey;
                var newAltKeys = new ArrayList();
                foreach (var alternateKey in value)
                {
                    if (key == null || key.Length == 0 || string.Compare(key, alternateKey.Trim(), true) != 0)
                    {
                        newAltKeys.Add(alternateKey.Trim());
                    }
                }

                value = (string[])newAltKeys.ToArray(typeof (string));

                // Se Argument foi informado e ele aparecer na lista informada, dá exception
                var arg = this.BaseArgument;
                if (arg != null && arg.Length > 0)
                {
                    foreach (var alternateKey in value)
                    {
                        if (string.Compare(arg, alternateKey.Trim(), true) == 0)
                        {
                            throw new ArgumentException("A coluna '" + alternateKey + "' foi informada na propriedade Argument. Ela não pode ser alternate key.");
                        }
                    }
                }

                this.ViewState["BaseAlternateKeys"] = value == null ? new string[0] : value;
            }
        }

        protected string BaseArgument
        {
            get
            {
                return (string)this.ViewState["BaseArgument"];
            }

            set
            {
                if (this.BaseKey != null && this.BaseKey.Length > 0 && string.Compare(value, this.BaseKey, true) == 0)
                {
                    throw new ArgumentException("A propriedade Argument não pode ter o mesmo valor da propriedade Key.");
                }

                this.ViewState["BaseArgument"] = value == null ? string.Empty : value.Trim();
            }
        }

        protected TSearchColumnCollection BaseGridColumns
        {
            get
            {
                return this.gridColumns;
            }
        }

        protected string BaseKey
        {
            get
            {
                return (string)this.ViewState["BaseKey"];
            }

            set
            {
                if (this.BaseArgument != null && this.BaseArgument.Length > 0 && string.Compare(value, this.BaseArgument, true) == 0)
                {
                    throw new ArgumentException("A propriedade Key não pode ter o mesmo valor da propriedade Argument.");
                }

                this.ViewState["BaseKey"] = value == null ? string.Empty : value.Trim();
            }
        }

        protected string BaseSqlOrder
        {
            get
            {
                return (string)this.ViewState["BaseSqlOrder"];
            }

            set
            {
                this.ViewState["BaseSqlOrder"] = value == null ? string.Empty : value;
            }
        }

        protected SqlSelect BaseSqlSelect
        {
            get
            {
                return (SqlSelect)this.ViewState["BaseSqlSelect"];
            }

            set
            {
                this.ViewState["BaseSqlSelect"] = value == null ? new SqlSelect() : value;
            }
        }

        protected string BaseSqlWhere
        {
            get
            {
                return (string)this.ViewState["BaseSqlWhere"];
            }

            set
            {
                this.ViewState["BaseSqlWhere"] = value == null ? string.Empty : value;

                // Força o recálculo de Dependees
                this.dependees = null;
            }
        }

        protected DbObject[] BaseWhereValues
        {
            get
            {
                return DbObject.ToDbObjectArray((object[])this.ViewState["BaseWhereValues"]);
            }

            set
            {
                this.ViewState["BaseWhereValues"] = DbObject.ToObjectArray(value);
            }
        }

        /// <summary>
        ///   Devolve BaseKey se ele for informado, caso contrário devolve a primeira coluna de BaseSqlSelect.
        ///   Se BaseSqlSelect também não foi informado, dá exception.
        /// </summary>
        protected string KeyCol
        {
            get
            {
                if (this.BaseKey.Length == 0)
                {
                    if (this.BaseSqlSelect.Columns.Count < 1)
                    {
                        throw new InvalidOperationException("A propriedade Key não foi informada ou a propriedade SqlSelect não informa nenhuma coluna.");
                    }
                    else
                    {
                        return this.BaseSqlSelect.Columns[0].Id;
                    }
                }
                else
                {
                    return this.BaseKey;
                }
            }
        }

        protected string[] PersistedColumnNames
        {
            get
            {
                return (string[])this.ViewState["PersistedColumnNames"];
            }

            set
            {
                this.ViewState["PersistedColumnNames"] = value == null ? new string[0] : value;
            }
        }

        protected DbObject[] PersistedColumnValues
        {
            get
            {
                return DbObject.ToDbObjectArray((object[])this.ViewState["PersistedColumnValues"]);
            }

            set
            {
                this.ViewState["PersistedColumnValues"] = DbObject.ToObjectArray(value);
            }
        }

        /// <summary>
        ///   Devolve BaseSqlSelect contendo as colunas originais, mais BaseKey, BaseArgument e AlternateKeys.
        /// </summary>
        protected SqlSelect SqlSelectExtended
        {
            get
            {
                var sqlSelect = this.BaseSqlSelect.Clone();

                if (!sqlSelect.Columns.Contains(this.KeyCol))
                {
                    sqlSelect.Columns.Insert(0, SqlSelectColumn.Parse(this.KeyCol));
                }

                if (!sqlSelect.Columns.Contains(this.ArgumentCol))
                {
                    sqlSelect.Columns.Insert(1, SqlSelectColumn.Parse(this.ArgumentCol));
                }

                foreach (var alternateKey in this.BaseAlternateKeys)
                {
                    if (!sqlSelect.Columns.Contains(alternateKey))
                    {
                        sqlSelect.Columns.Add(alternateKey);
                    }
                }

                return sqlSelect;
            }
        }

        ChangedEventHandler IDepender.ChangedHandler
        {
            get
            {
                if (this.changedHandler == null)
                {
                    this.changedHandler = new ChangedEventHandler(this.DependeeChanged);
                }

                return this.changedHandler;
            }
        }

        string[] IDepender.Dependees
        {
            get
            {
                if (this.dependees == null)
                {
                    try
                    {
                        this.dependees = DependerLib.GetDependees(this);
                    }
                    catch (Exception exc)
                    {
                        throw new InvalidOperationException("Existe algum erro na propriedade " + this.UniqueID + ".SQLWhere (" + this.BaseSqlWhere + ").", exc);
                    }
                }

                return this.dependees;
            }
        }

        private bool GridVisible
        {
            get
            {
                return (bool)this.ViewState["GridVisible"];
            }

            set
            {
                this.ViewState["GridVisible"] = value;
            }
        }

        string IDepender.SqlWhere
        {
            get
            {
                return this.BaseSqlWhere;
            }
        }

        /// <summary>
        ///   Neste momento se supõe que tenha passado pelo GetAndBind
        /// </summary>
        public DbObject this[string columnName]
        {
            get
            {
                if (!this.IsValidDBValue)
                {
                    throw new InvalidOperationException("Não existe registro corrente no controle " + this.ID);
                }

                if (this.pvPersistedColumnValues == null)
                {
                    this.pvPersistedColumnValues = new NameDbObjectCollection(false);
                    this.pvPersistedColumnValues.Fill(this.PersistedColumnNames, this.PersistedColumnValues);
                }

                if (this.pvPersistedColumnValues.IndexOfKey(columnName) < 0)
                {
                    throw new ArgumentException("A coluna " + columnName + " não foi encontrada nas propriedades SqlSelect, Key ou Argument do controle " + this.UniqueID + ".");
                }

                return this.pvPersistedColumnValues[columnName];
            }
        }

        public virtual void CopyArgumentProperties(WebControl target)
        {
            // Foi preferido atribuir propriedade a propriedade do que chamar CopyProperties(),
            // pois muitas das propriedades (como AccessKey, TabIndex) não devem ser copiadas.
            target.ControlStyle.CopyFrom(this.ControlStyle);
            target.Enabled = this.Enabled;
            target.Visible = this.Visible;
            target.CssClass = this.CssClass; // CssClass não é copiado adequadamente via ControlStyle.CopyFrom().

            if (target is TextBox)
            {
                var txt = (TextBox)target;
                txt.Columns = this.ArgumentColumns;

// ...
            }
            else if (target is Label)
            {
                var lbl = (Label)target;
                lbl.Text = this.ToString(this.ArgumentValue);

// ...
            }
        }

        public override void ResetValue()
        {
            this.ArgumentText = string.Empty;

            // Tem que resetar ArgumentValue antes do DBValue pois o DBValue(set)
            // chama o método GetAndBind() que utiliza o valor de ArgumentValue.
            this.ArgumentValue = DBNull.Value;

            base.ResetValue();
        }

        protected virtual void OnSetViewModeDescription(SetViewModeDescriptionEventArgs args)
        {
            // Existe somente para ser overriden.
        }

        protected virtual void RenderArgumentControl(HtmlTextWriter writer)
        {
            var txt = new TextBox();

            txt.ID = this.UniqueID + ':' + ArgumentControlName;
            this.CopyArgumentProperties(txt);

// Para quando passar do modo View para o modo Edit dentro de um RecordManager
            if (this.ArgumentText.Length == 0 && !this.ArgumentValue.IsNull)
            {
                this.ArgumentText = StrLib.ToStr(this.ArgumentValue);
            }

            txt.Text = this.ArgumentText;

            txt.RenderControl(writer); // BaseArgument
        }

        protected override bool LoadPostData(string postDataKey, 
                                             NameValueCollection postCollection)
        {
            // Verifica se houve alteração no controle que representa a descrição (argument)
            var newArgument = postCollection[this.UniqueID + ':' + ArgumentControlName];
            if (newArgument != null && newArgument != this.ArgumentText)
            {
                this.ArgumentText = newArgument;
                this.ArgumentChanged = true;
            }

            // base.LoadPostData() setará DBValue que, por sua vez, poderá chamar GetAndBind() e
            // mostrar a grid. Como GetAndBind() usa ArgumentText e ArgumentValue, estas propriedades
            // não devem ser alteradas mais!
            this.KeyChanged = base.LoadPostData(postDataKey, postCollection);

            return this.ArgumentChanged || this.KeyChanged;
        }

        protected override void LoadViewState(object savedState)
        {
            var state = (ArrayList)savedState;
            var offset = 0;

            this.ArgumentText = (string)state[offset++];
            this.ArgumentValue = DbObject.ToDbObject(state[offset++]);
            this.GridVisible = (bool)state[offset++];
            this.PersistedColumnNames = (string[])state[offset++];
            this.PersistedColumnValues = DbObject.ToDbObjectArray((object[])state[offset++]);

            if (this.GridVisible)
            {
                var grdState = state[offset++];

                // O base.LoadViewState() precisa ser chamado antes de GetAndBind() porque o primeiro
                // recuperará propriedades utilizadas pelo segundo, como DBValue.
                base.LoadViewState(state[offset++]);

                this.GetAndBind(false);
                this.grd.LoadViewStateInternal(grdState);

                // Este DataBind() é necessário porque, apesar de estar sendo feito dentro de GetAndBind() acima,
                // o estado (CurrentPageIndex, SortExpression, ...) é restaurado somente em LoadViewStateInternal().
                this.grd.DataBind();
            }
            else
            {
                base.LoadViewState(state[offset++]);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            // hypSearch
            this.hypSearch.ID = "hypSearch";
            this.hypSearch.ImageUrl = TUtil.TranslateRelativeUrl(this.SearchButtonImageUrl, this);
            this.hypSearch.Click += this.hypSearch_Click;
            this.hypSearch.CssClass = this.ButtonCssClass;

            // hypSearch deve ser filho do TSearch para que seja possível tratar o evento hypSearch.Click
            this.Controls.Add(this.hypSearch);

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Detecta alterações nos controles dos quais este controle depende
            DependerLib.RegisterDepender(this);
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Registra biblioteca comum TControl.js
            RegisterTControlScript(this);

            this.Page.RegisterRequiresPostBack(this);

            base.OnPreRender(e);
        }

        protected override void PreTControlCtor()
        {
            base.PreTControlCtor();

            // Deve ser feito antes de inicializar DBValue
            this.ArgumentValue = DBNull.Value;
            this.hypSearch = new ImageLink();
        }

        protected override void RaisePostDataChangedEvent()
        {
            this.Msg = string.Empty;

            // Grade
            if (this.KeyChanged)
            {
                this.ArgumentValue = DBNull.Value;
                if (this.DBValue.IsNull && this.EntryValue.Trim().Length == 0)
                {
                    this.ClearPersistedColumns();
                    base.RaisePostDataChangedEvent();
                }
                else
                {
                    this.GetAndBind(true);
                }
            }
            else if (this.ArgumentChanged)
            {
                this.ClearPersistedColumns();
                if (!this.DBValue.IsNull)
                {
                    base.DBValue = DBNull.Value;
                    this.ArgumentValue = DBNull.Value;
                }

                if (this.ArgumentText.Length > 0)
                {
                    this.Msg = this.ClickToSearchMsg;
                }
            }
        }

        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            if (this.Page != null && this.Page.IsPostBack)
            {
                this.Attributes.Add("OnChange", "ClearTSearch('" + this.ClientID + "', " +
                                                "'" + StrLib.ToStr(this.DBValue, this.Format, Thread.CurrentThread.CurrentCulture.Name).Replace("'", @"\'") + "', " +
                                                "'" + this.ArgumentText.Replace("'", @"\'") + "'" +
                                                ");");
            }

            base.RenderControlEditMode(writer); // BaseKey

            this.RenderArgumentControl(writer);

            if (!TControl.InDesignMode(this))
            {
                this.hypSearch.RenderControl(writer); // Botão buscar
            }
        }

        protected override void RenderControlViewMode(HtmlTextWriter writer)
        {
            base.RenderControlViewMode(writer);

            // Separador
            if (this.pvSeparator != null && this.pvSeparator != string.Empty)
            {
                var lblSep = new Label();
                lblSep.Text = this.pvSeparator;
                lblSep.ControlStyle.CopyFrom(this.ControlStyle);
                lblSep.CssClass = this.CssClass;
                lblSep.RenderControl(writer);
            }

            // BaseArgument
            var lblUserArg = new Label();
            this.CopyArgumentProperties(lblUserArg);
            lblUserArg.CssClass = this.CssClass;
            {
                var args = new SetViewModeDescriptionEventArgs(lblUserArg.Text);
                this.OnSetViewModeDescription(args);
                lblUserArg.Text = args.Description;
            }

            lblUserArg.RenderControl(writer);
        }

        protected override void RenderDebugInfo(HtmlTextWriter writer)
        {
            base.RenderDebugInfo(writer);

            writer.Write("<B>PersistedColumnNames: </B>" + (this.PersistedColumnNames == null ? "null" : StrLib.EnumerableToStr(this.PersistedColumnNames)) + "<BR/>");
            writer.Write("<B>PersistedColumnValues: </B>" + (this.PersistedColumnValues == null ? "null" : StrLib.EnumerableToStr(this.PersistedColumnValues)) + "<BR/>");
            writer.Write("<B>ArgumentText: </B>" + this.ArgumentText + "<BR/>");
            writer.Write("<B>ArgumentValue: </B>" + (this.ArgumentValue == null ? "null" : this.ToString(this.ArgumentValue)) + "<BR/>");
            writer.Write("<B>GridVisible: </B>" + this.GridVisible + "<BR/>");

            DependerLib.WriteDebugInfo(writer, (IDepender)this);
        }

        protected override void RenderExtra(HtmlTextWriter writer)
        {
            if (this.grd != null && this.GridVisible)
            {
                new LiteralControl("<span id=\"" + this.ClientID + "_list\" " +
                                   "style=\"background-color:white;" +
                                   "z-index:" + (this.GridVisible ? "500" : "-1") + ";" +
                                   "border-style:solid;" +
                                   "border-width:1px;" +
                                   "border-color:black;" +
                                   "position:absolute;" +
                                   "visibility:hidden;" +
                                   "top:0;" +
                                   "left:0;" +
                                   "width:" + this.pvGridWidth + "\"" +
                                   ">").RenderControl(writer);
                new LiteralControl("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td>").RenderControl(writer);
                this.grd.Width = Unit.Percentage(100);
                this.grd.RenderControl(writer);
                new LiteralControl("</td></tr><tr><td style=\"text-align:center;\" align=\"center\" height=\"25\">").RenderControl(writer);
                new LiteralControl("<a href=\"javascript:ShowTSearch('" + this.ClientID + "',false); " + this.Page.ClientScript.GetPostBackEventReference(this, "closeGrid") + ";\">Fechar</a>").RenderControl(writer);
                new LiteralControl("</td></tr></table>").RenderControl(writer);
                new LiteralControl("</span>").RenderControl(writer);
            }
        }

        protected override object SaveViewState()
        {
            var list = new ArrayList();

            list.Add(this.ArgumentText);
            list.Add(this.ArgumentValue.ToObject());
            list.Add(this.GridVisible);
            list.Add(this.PersistedColumnNames);
            list.Add(DbObject.ToObjectArray(this.PersistedColumnValues));

            if (this.GridVisible)
            {
                list.Add(this.grd.SaveViewStateInternal());
            }

            list.Add(base.SaveViewState());

            return list;
        }

        protected void ClearPersistedColumns()
        {
            this.PersistedColumnValues = new DbObject[0];
            this.pvPersistedColumnValues = null;
        }

        private void CreateGrid()
        {
            this.grd = new TDataGrid();
            this.CopyProperties(this.grd);
            this.grd.DebugMode = this.DebugMode;
            this.grd.AutoDataBind = false;
            this.grd.AutoGenerateColumns = false;
            this.grd.ItemCommand += this.grd_ItemCommand;
            this.grd.PageIndexChanged += this.grd_PageIndexChanged;
            this.grd.AllowPaging = this.GridPageSize > 0;
            this.grd.PageSize = this.GridPageSize > 0 ? this.GridPageSize : GridPageSize_Def;
            this.grd.ShowDeletedRecords = false;
            this.grd.ShowHistoryIcon = false;
            this.grd.CssClass = this.GridCssClass;
            this.grd.AllowSorting = false;

            
            {
                var gridCols = new ArrayList();
                gridCols.AddRange(this.SqlSelectExtended.GetColumnIds());
                this.grd.PersistColumns = (string[])gridCols.ToArray(typeof (string));
            }

            

            TextBoxColumn columnKey = null;
            var argumentColAdded = false;
            foreach (TextBoxColumn column in this.gridColumns)
            {
                if (string.Compare(column.ColumnName, this.KeyCol, true) == 0)
                {
                    columnKey = column;
                }
                else
                {
                    this.grd.Columns.Add(column);
                    if (string.Compare(column.ColumnName, this.ArgumentCol, true) == 0)
                    {
                        argumentColAdded = true;
                    }
                }
            }
            {
// Coluna Key
                var buttonCol = new ButtonColumn();
                buttonCol.ButtonType = ButtonColumnType.LinkButton;
                buttonCol.CommandName = "select";

                if (columnKey == null)
                {
                    buttonCol.DataTextField = this.KeyCol;
                    buttonCol.HeaderText = this.KeyCol;
                }
                else
                {
                    buttonCol.DataTextField = columnKey.ColumnName;
                    buttonCol.DataTextFormatString = columnKey.Format.Length == 0 ? string.Empty : "{0:" + columnKey.Format + "}";
                    buttonCol.HeaderText = columnKey.HeaderText;
                    buttonCol.SortExpression = columnKey.SortExpression;
                }

                this.grd.Columns.AddAt(0, buttonCol);
            }

            // Coluna Argument
            if (!argumentColAdded)
            {
                var textBoxCol = new TextBoxColumn();
                textBoxCol.ColumnName = this.ArgumentCol;
                textBoxCol.HeaderText = this.ArgumentCol;
                this.grd.Columns.AddAt(1, textBoxCol);
            }

            foreach (DataGridColumn col in this.grd.Columns)
            {
                if (col is TextBoxColumn)
                {
                    ((TextBoxColumn)col).ReadOnlyCssClass = this.GridColumnReadOnlyCssClass;
                }
            }
        }

        private void DependeeChanged(object sender, ChangedEventArgs args)
        {
            if (this.Manager == null)
            {
                this.ResetValue();
            }
        }

        private DataTable Get(TConnection cn, string where, DbObject[] whereValues)
        {
            var sql = this.SqlSelectExtended +
                      (where == string.Empty ? string.Empty : " WHERE " + where) +
                      (this.BaseSqlOrder == string.Empty ? string.Empty : " ORDER BY " + this.BaseSqlOrder);

            try
            {
                try
                {
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
                                throw new Exception("Verifique se a propriedade " + this.UniqueID + ".DataType está de acordo com o tipo da coluna informada na propriedade Key.");

                                // -2147217833: Arithmetic overflow error converting numeric to data type numeric.
                                // Este erro ocorre qdo passa-se um número maior do que o permitido pela coluna.
                            case -2147217833:
                                return null;
                        }
                    }

                    throw new Exception("Possível erro no select: " + sql, exc);
                }
            }
            finally
            {
            }
        }

        /// <summary>
        ///   Realiza a busca por código (DBValue) ou por descrição (ArgumentValue) e devolve o número
        ///   de registros encontrados. Se o DBValue for DBNull, busca por ArgumentValue. Se este também
        ///   for DBNull, nada faz e devolve 0.
        ///   Preenche DBValue, ArgumentValue e ArgumentText se encontrou um único registro. Se encontrou
        ///   mais de um registro, mostra a grid.
        /// </summary>
        private int GetAndBind(bool FireChangedEvent)
        {
            TTextBox txt = null;
            DataTable dt = null;

            if (this.BaseSqlSelect.ToString().Length == 0)
            {
                throw new InvalidOperationException("A propriedade " + this.ID + ".BaseSqlSelect não foi informada");
            }

            var cn = this.GetConnection();
            cn.Open();
            try
            {
                var where = this.BaseSqlWhere;
                var whereValues = new DbObject[0];

                if (this.EntryValue.Trim().Length > 0)
                {
                    

                    where += (where.Length == 0 ? string.Empty : " AND ") + "({0} = ?)";

                    #region DbObject[] whereValues1 = <whereValues com uma posição a mais>;

                    var whereValues1 = new DbObject[whereValues.Length + 1];
                    whereValues.CopyTo(whereValues1, 0);

                    #endregion

                    var index = whereValues1.Length - 1;
                    whereValues1[index] = this.DBValue;

                    string keyCol;
                    {
                        var indexKey = this.BaseSqlSelect.Columns.IndexOf(this.KeyCol);
                        if (indexKey < 0)
                        {
                            keyCol = this.KeyCol;
                        }
                        else
                        {
                            keyCol = this.BaseSqlSelect.Columns[indexKey].ToString(false);
                        }
                    }

                    var where1 = string.Format(where, keyCol);
                    try
                    {
                        GetSqlWhere(null, this.RecordContainer, this.NamingContainer, cn.Rdbms, false, ref where1, ref whereValues1);
                    }
                    catch (Exception exc)
                    {
                        throw new Exception("Possível erro no where: " + where1, exc);
                    }

                    #region DbType[] alternateKeyTypes = ...;

                    var alternateKeyTypes = new DbType[this.BaseAlternateKeys.Length];
                    dt = this.Get(cn, "1 = 0", new DbObject[0]);
                    for (var i = 0; i < alternateKeyTypes.Length; i++)
                    {
                        alternateKeyTypes[i] = DbObject.ToDbType(dt.Columns[this.BaseAlternateKeys[i]].DataType);
                    }

                    #endregion

                    // Tenta coluna informada pela propriedade Key
                    dt = this.Get(cn, where1, whereValues1);
                    if (dt != null && dt.Rows.Count > 1)
                    {
                        throw new InvalidOperationException("A restrição pela coluna Key (" + this.KeyCol + ") não foi suficiente para trazer somente um registro para o controle " + this.UniqueID + ".");
                    }

                    // Se não encontrou registro, tenta os AlternateKey's
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        for (var i = 0; i < this.BaseAlternateKeys.Length; i++)
                        {
                            try
                            {
                                whereValues1[index] = StrLib.TypeStr(this.EntryValue, alternateKeyTypes[i], Thread.CurrentThread.CurrentCulture.Name);

                                var where2 = string.Format(where, this.BaseAlternateKeys[i]);
                                try
                                {
                                    GetSqlWhere(null, this.RecordContainer, this.NamingContainer, cn.Rdbms, false, ref where2, ref whereValues1);
                                }
                                catch (Exception exc)
                                {
                                    throw new Exception("Possível erro no where: " + where2, exc);
                                }

                                dt = this.Get(cn, where2, whereValues1);

                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 1)
                                    {
                                        throw new InvalidOperationException("A restrição pela coluna AlternateKey '" + this.BaseAlternateKeys[i] + "' não foi suficiente para trazer somente um registro.");
                                    }
                                    else if (dt.Rows.Count > 0)
                                    {
                                        break;
                                    }
                                }
                            }
                            catch (FormatException)
                            {
                                // StrLib.TypeStr() deu erro na conversão: o alternateKey corrente é descartado.
                            }
                        }
                    }

                    
                }
                else if (!this.ArgumentValue.IsNull)
                {
                    

                    txt = new TTextBox();
                    txt.ID = "__ArgValue__";
                    txt.IndexedColumn = true;
                    txt.DBValue = this.ArgumentValue;
                    txt.EnableViewState = false; // ArgumentValue se encarrega da persistência
                    this.Controls.Add(txt);

                    int index;
                    string argumentCol;
                    {
                        index = this.BaseSqlSelect.Columns.IndexOf(this.ArgumentCol);
                        if (index < 0)
                        {
                            argumentCol = this.ArgumentCol;
                        }
                        else
                        {
                            argumentCol = this.BaseSqlSelect.Columns[index].ToString(false);
                        }
                    }

                    where += (where.Length > 0 ? " AND " : string.Empty) +
                             "(" + argumentCol + " = #" + this.UniqueID + ':' + txt.ID + "#)";

                    try
                    {
                        GetSqlWhere(null, this.RecordContainer, this.NamingContainer, cn.Rdbms, false, ref where, ref whereValues);
                    }
                    catch (Exception exc)
                    {
                        if (index < 0)
                        {
                            throw new InvalidOperationException("Propriedade Argument inválida: \"" + argumentCol + "\".", exc);
                        }
                        else
                        {
                            throw new InvalidOperationException("Possivelmente um erro na coluna " + (index + 1) + " do select: " + this.BaseSqlSelect + ".", exc);
                        }
                    }

                    dt = this.Get(cn, where, whereValues);

                    
                }
                else
                {
                    this.ClearPersistedColumns();
                    return 0;
                }
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }

            this.PersistedColumnNames = (string[])TechLib.EnumerableItemProperty(dt.Columns, "ColumnName");

            if (txt != null)
            {
                this.Controls.Remove(txt);
            }

            switch (dt.Rows.Count)
            {
                case 0:
                    this.Msg = this.NotFoundMsg;
                    if (this.GridVisible)
                    {
                        this.GridVisible = false;
                    }

                    if (FireChangedEvent)
                    {
                        this.OnChanged(new ChangedEventArgs(this));
                    }

                    this.ClearPersistedColumns();
                    break;

                case 1:

// Encontrou um único registro: escolhe-o automaticamente
                    var objValue = DbObject.ToDbObject(dt.Rows[0][this.KeyCol]);

                    if (!objValue.IsNull && objValue.Type != this.DataType)
                    {
                        throw new InvalidOperationException("A coluna '" + this.KeyCol + "' possui tipo (" + objValue.Type + ") diferente do informado na propriedade " + this.UniqueID + ".DataType.");
                    }

                    base.DBValue = objValue;
                    this.ArgumentValue = DbObject.ToDbObject(dt.Rows[0][this.ArgumentCol]);
                    this.ArgumentText = StrLib.ToStr(this.ArgumentValue);

// Salvar outras colunas
                    this.PersistedColumnValues = DbObject.ToDbObjectArray(dt.Rows[0].ItemArray);

                    this.Msg = string.Empty;
                    if (this.GridVisible)
                    {
                        this.GridVisible = false;
                    }

                    if (FireChangedEvent)
                    {
                        this.OnChanged(new ChangedEventArgs(this));
                    }

                    break;

                default:
                    if (this.grd == null)
                    {
                        this.CreateGrid();
                        this.Controls.Add(this.grd);
                    }
                    else if (this.grd.PageSize > 0)
                    {
                        var pageCount = TechLib.Ceiling(decimal.Divide(dt.Rows.Count, this.grd.PageSize));
                        if (this.grd.CurrentPageIndex > pageCount - 1)
                        {
                            this.grd.CurrentPageIndex = pageCount == 0 ? 0 : pageCount - 1;
                        }
                    }

                    this.Msg = null;
                    this.grd.DataSource = dt;
                    this.grd.DataBind();
                    this.GridVisible = true;
                    if (this.Mode == ControlMode.Edit)
                    {
                        this.Page.ClientScript.RegisterStartupScript(typeof (TSearchBase), this.ID + "show", 
                                                                     "<SCRIPT LANGUAGE='javascript'>if(ShowTSearch) ShowTSearch('" + this.ClientID + "', true);</SCRIPT>"
                            );
                    }

                    this.ClearPersistedColumns();
                    break;
            }

            return dt.Rows.Count;
        }

        /// <summary>
        ///   Se Connection foi informada, devolve uma conexão com base nela.
        ///   Caso contrário, se o controle estiver sob um manager, obtém a conexão do TechneDS associado a ele.
        ///   Caso contrário, devolve connection da TPage.
        ///   Caso contrário, dá exception.
        /// </summary>
        private TConnection GetConnection()
        {
            return CreateConnection(this, this.Connection);
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == "closeGrid")
            {
                this.GridVisible = false;
            }
        }

        private void grd_ItemCommand(object sender, DataGridCommandEventArgs args)
        {
            switch (args.CommandName)
            {
                case "select":
                    var gridItem = args.Item as TGridItem;
                    var objValue = gridItem[this.KeyCol];

                    if (!objValue.IsNull && objValue.Type != this.DataType)
                    {
                        throw new InvalidOperationException("A coluna '" + this.KeyCol + "' possui tipo (" + objValue.Type + ") diferente do informado na propriedade " + this.UniqueID + ".DataType.");
                    }

                    base.DBValue = objValue;
                    this.ArgumentValue = gridItem[this.ArgumentCol];
                    this.ArgumentText = StrLib.ToStr(this.ArgumentValue);

// Salvar outras colunas
                    this.PersistedColumnValues = gridItem.RowValues;

                    this.Msg = string.Empty;
                    this.GridVisible = false;
                    this.OnChanged(new ChangedEventArgs(this));
                    break;

                case "Page":

// Tratado pelo método grd_PageIndexChanged()
                    break;

                case "Sort":
                    this.GetAndBind(false);
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private void grd_PageIndexChanged(object sender, DataGridPageChangedEventArgs args)
        {
            this.grd.CurrentPageIndex = args.NewPageIndex;
            this.GetAndBind(false);
        }

        private void hypSearch_Click(object sender, EventArgs args)
        {
            if (this.ArgumentText.Length == 0)
            {
                return;
            }

            if (this.KeyChanged && !this.DBValue.IsNull)
            {
                return;
            }

            // Apaga a mensagem ClickToSearchMsg setada em RaisePostDataChangedEvent
            this.Msg = string.Empty;

            this.ArgumentValue = this.ArgumentText;
            this.GetAndBind(true);
        }
    }

    internal class TSearchColumnCollection : CollectionBase
    {
        public TextBoxColumn this[int index]
        {
            get
            {
                return (TextBoxColumn)this.List[index];
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                this.List[index] = value;
            }
        }

        public int Add(TextBoxColumn column)
        {
            return this.List.Add(column);
        }

        public int Add(string columnName)
        {
            return this.List.Add(new TextBoxColumn(columnName));
        }

        public int Add(string columnName, string headerText)
        {
            var textBoxColumn = new TextBoxColumn(columnName);
            textBoxColumn.HeaderText = headerText;
            return this.List.Add(textBoxColumn);
        }
    }

    public class SetViewModeDescriptionEventArgs : EventArgs
    {
        private string description;

        public SetViewModeDescriptionEventArgs(string description)
        {
            this.description = description;
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value == null ? string.Empty : value;
            }
        }
    }
}