using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing.Design;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallback;
using Techne.Controls;
using Techne.Data;
using Techne.Library;
using Techne.Library.Sql.Structure;
using Techne.Web.Design;

namespace Techne.Web
{
    [ParseChildren(true, "GridColumns"), ControlValueProperty("DBValue"), ToolboxData("<{0}:TSearchBox runat=\"server\"></{0}:TSearchBox>"), Designer(typeof (TSearchBoxDesigner))]
    public class TSearchBox : TTextBox, IDepender, INamingContainer, IPostBackEventHandler
    {
        // (em ordem alfabética)
        public string _imagePath = "~/Scripts/themes/basic/images";

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

        private readonly Style _popupStyle = new Style();

        private readonly TSearchBoxColumnCollection gridColumns;

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

        private string _argument = string.Empty;

        private ASPxCallback _callback;

        private TConnectionWritable _connection;

        private bool _enableInternalCaching = true;

        private string _gridClass = "scroll";

        private string _gridSkinID = "TSearchBox";

        private HtmlTable _gridTable;

        private string _instanceID;

        private string _key = string.Empty;

        private HtmlGenericControl _pager;

        private WebControl _popup;

        private HiddenField _settings;

        private string _sqlOrder = string.Empty;

        private SqlSelect _sqlSelect;

        private string _sqlWhere = string.Empty;

        private bool _validateText = true;

        private ChangedEventHandler changedHandler;

        private string[] dependees;

        private ImageButton hypSearch;

        // private bool _visible = true;
        // private bool _enabled = true;
        private Unit pvGridHeight = Unit.Pixel(200);

        private Unit pvGridWidth = Unit.Pixel(500);

        private NameDbObjectCollection pvPersistedColumnValues;

        public TSearchBox()
        {
            this.ArgumentColumns = ArgumentColumns_Def;
            this.ArgumentText = string.Empty;
            this.ButtonCssClass = ButtonCssClass_Def;
            this.ClickToSearchMsg = ClickToSearchMsg_Def;
            this.Connection = string.Empty;
            this.GridCssClass = GridCssClass_Def;
            this.GridPageSize = GridPageSize_Def;

// GridVisible = false;
            this.NotFoundMsg = NotFoundMsg_Def;
            this.SearchButtonImageUrl = SearchButtonImageUrl_Def;
            this.SearchButtonToolTip = SearchButtonToolTip_Def;
            this.Separator = Separator_Def;

            this.BaseAlternateKeys = new string[0];
            this.Argument = string.Empty;
            this.Key = string.Empty;
            this.WhereValues = new DbObject[0];

            this.PersistedColumnNames = new string[0];
            this.PersistedColumnValues = new DbObject[0];

            this.gridColumns = new TSearchBoxColumnCollection();
        }

        [Description("Permite consultar/alterar a descri\x00e7\x00e3o do valor do controle utilizada em modo View."), Category("Techne")]
        public event SetViewModeDescriptionEventHandler SetViewModeDescription;

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
                if (!this.SettingState)
                {
                    if (!value.IsNull)
                    {
                        this.GetAndBind(false);
                    }
                    else
                    {
                        this.ArgumentValue = DBNull.Value;
                        this.ArgumentText = StrLib.ToStr(this.ArgumentValue);
                    }
                }
            }
        }

        public string Argument
        {
            get
            {
                return this._argument;
            }

            set
            {
                if (!this.DesignMode && this.Key != null && this.Key.Length > 0 && string.Compare(value == null ? string.Empty : value, this.Key, true) == 0)
                {
                    throw new ArgumentException("A propriedade Argument não pode ter o mesmo valor da propriedade Key.");
                }

                this._argument = value == null ? string.Empty : value.Trim();
            }
        }

        /// <summary>
        ///   Número de caracteres da coluna de argumentos, para dimensionar o tamanho do controle de argumentos
        /// </summary>
        [Description("N\x00famero de caracteres da coluna de argumentos, para dimensionar o tamanho do controle de argumentos"), Category("Appearance"), DefaultValue(70)]
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

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), Obsolete("Esta propriedade est\x00e1 obsoleta e n\x00e3o \x00e9 mais utilizada pelo controle.")]
        public bool AutoGenerateColumns
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        [Category("Behavior"), DefaultValue(true)]
        public bool AutoPostBack
        {
            get
            {
                return this.ViewState["AutoPostBack"] == null ? true : (bool)this.ViewState["AutoPostBack"];
            }

            set
            {
                this.ViewState["AutoPostBack"] = value;
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
        [Description("Nome da conex\x00e3o com banco de dados"), Category("Techne"), DefaultValue("")]
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

        [DefaultValue("true")]
        [Browsable(false)]
        public bool EnableInternalCaching
        {
            get
            {
                return this._enableInternalCaching;
            }

            set
            {
                this._enableInternalCaching = value;
            }
        }

        [DefaultValue("scroll")]
        public string GridClass
        {
            get
            {
                return this._gridClass;
            }

            set
            {
                this._gridClass = value == null ? string.Empty : value;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TSearchBoxColumnCollection GridColumns
        {
            get
            {
                return this.gridColumns;
            }
        }

        [Category("Appearance - Grid"), DefaultValue("TSearchGrid")]
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

        [
            Category("Appearance - Grid"), 
            DefaultValue("200px"), 
        ]
        public Unit GridHeight
        {
            get
            {
                return this.pvGridHeight;
            }

            set
            {
                this.pvGridHeight = value;
            }
        }

        [DefaultValue("~/Scripts/themes/basic/images")]
        public string GridImagePath
        {
            get
            {
                return this._imagePath;
            }

            set
            {
                this._imagePath = value == null ? string.Empty : value;
            }
        }

        /// <summary>
        ///   Número de registros que serão mostrados numa página da grid de busca. Caso seja 0, não pagina.
        /// </summary>
        [DefaultValue(10), Description("N\x00famero de registros que ser\x00e3o mostrados numa p\x00e1gina da grid de busca. Caso seja 0, n\x00e3o pagina."), Category("Appearance - Grid")]
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

        [DefaultValue("TSearchBox")]
        public string GridSkinID
        {
            get
            {
                return this._gridSkinID;
            }

            set
            {
                this._gridSkinID = value == null ? string.Empty : value;
            }
        }

        [DefaultValue("500px"), Category("Appearance - Grid")]
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

        [Description("Nome da coluna que \x00e9 a chave da tabela sobre a qual ser\x00e1 feita a busca. N\x00e3o suporta chaves compostas"), DefaultValue("")]
        public string Key
        {
            get
            {
                return this._key;
            }

            set
            {
                if (!this.DesignMode && this.Argument != null && this.Argument.Length > 0 && string.Compare(value == null ? string.Empty : value, this.Argument, true) == 0)
                {
                    throw new ArgumentException("A propriedade Key não pode ter o mesmo valor da propriedade Argument.");
                }

                this._key = value == null ? string.Empty : value.Trim();
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

        [Browsable(true), PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style PopupStyle
        {
            get
            {
                return this._popupStyle;
            }
        }

        [Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), DefaultValue("~/images/bt_search.gif"), Category("Appearance")]
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

        [Category("Appearance"), Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), DefaultValue("Search")]
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
        [DefaultValue(" - "), Category("Appearance"), Description("Texto que separa o c\x00f3digo e a descri\x00e7\x00e3o no modo View")]
        public string Separator { get; set; }

        [DefaultValue("")]
        public string SqlOrder
        {
            get
            {
                return this._sqlOrder;
            }

            set
            {
                this._sqlOrder = value == null ? string.Empty : value;
            }
        }

        [DefaultValue(null)]
        public SqlSelect SqlSelect
        {
            get
            {
                return this._sqlSelect;
            }

            set
            {
                this._sqlSelect = value == null ? new SqlSelect() : value;
            }
        }

        public string SqlWhere
        {
            get
            {
                return this._sqlWhere;
            }

            set
            {
                this._sqlWhere = value == null ? string.Empty : value;

                // Força o recálculo de Dependees
                this.dependees = null;
            }
        }

        [DefaultValue(true)]
        public bool ValidateText
        {
            get
            {
                return this._validateText;
            }

            set
            {
                this._validateText = value;
            }
        }

        [Bindable(true, BindingDirection.TwoWay), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue((string)null)]
        public object Value
        {
            get
            {
                return this.DBValue == null || this.DBValue.IsNull ? null : this.DBValue.ToObject();
            }

            set
            {
                this.DBValue = value == null ? DBNull.Value : DbObject.ToDbObject(value);
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
                if (this.Argument.Length == 0)
                {
                    if (this.SqlSelect.Columns.Count < 2)
                    {
                        throw new InvalidOperationException("A propriedade Argument não foi informada ou a propriedade SqlSelect possui menos de duas colunas.");
                    }
                    else
                    {
                        return this.SqlSelect.Columns[1].Id;
                    }
                }
                else
                {
                    return this.Argument;
                }
            }
        }

        protected string InstanceID
        {
            get
            {
                if (this._instanceID == null)
                {
                    this._instanceID = Guid.NewGuid() + this.UniqueID;
                }

                return this._instanceID;
            }

            set
            {
                this._instanceID = value == null ? string.Empty : value;
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
                if (this.Key.Length == 0)
                {
                    if (this.SqlSelect.Columns.Count < 1)
                    {
                        throw new InvalidOperationException("A propriedade Key não foi informada ou a propriedade SqlSelect não informa nenhuma coluna.");
                    }
                    else
                    {
                        return this.SqlSelect.Columns[0].Id;
                    }
                }
                else
                {
                    return this.Key;
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
                var sqlSelect = this.SqlSelect.Clone();

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

        [Description("Caso o valor digitado no controle (associado \x00e0 coluna Key) n\x00e3o seja encontrado, tenta buscar o valor em colunas alternativas relacionadas nesta propriedade."), DefaultValue((string)null), Category("Techne"), TypeConverter(typeof (Techne.Controls.StringArrayConverter))]
        private string[] AlternateKeys
        {
            get
            {
                return this.BaseAlternateKeys;
            }

            set
            {
                this.BaseAlternateKeys = value == null ? new string[0] : value;
            }
        }

        [
            Browsable(false), 
            DefaultValue("")
        ]
        private string ArgumentText
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

        private DbObject ArgumentValue
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

        private string[] BaseAlternateKeys
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
                var key = this.Key;
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
                var arg = this.Argument;
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

        private TConnectionWritable DataConnection
        {
            get
            {
                if (this._connection == null)
                {
                    if (!string.IsNullOrEmpty(this.Connection))
                    {
                        this._connection = ConnectionList.CreateWritableConnection(this.Connection);
                    }
                    else
                    {
                        this._connection = new TConnectionWritable(Techne.Controls.TControl.GetConnectionString(this, string.Empty));
                    }
                }

                return this._connection;
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
                        throw new InvalidOperationException("Existe algum erro na propriedade SQLWhere do controle" + this.UniqueID + ".\nSQLWhere='" + this.SqlWhere + "').", exc);
                    }
                }

                return this.dependees;
            }
        }

        private Unit InternalGridHeight
        {
            get
            {
                if (!this.GridHeight.IsEmpty)
                {
                    return this.GridHeight;
                }
                else
                {
                    return Unit.Pixel(300);
                }
            }
        }

        private Unit InternalGridWidth
        {
            get
            {
                if (!this.GridWidth.IsEmpty)
                {
                    return this.GridWidth;
                }
                else if (!this.Width.IsEmpty)
                {
                    return this.Width;
                }
                else
                {
                    return Unit.Pixel(500);
                }
            }
        }

        string IDepender.SqlWhere
        {
            get
            {
                return this.SqlWhere;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue((string)null), Browsable(false)]
        private DbObject[] WhereValues
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

        public static void RegisterTSearchBoxScript(Page page)
        {
            TSearch.RegisterTSearchScript(page);
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
            this.ArgumentValue = DBNull.Value;

            base.ResetValue();
        }

        protected virtual string ProcessCallback(string operation, Hashtable jsonData)
        {
            if (operation == "gridSearch")
            {
                return this.ProcessGridCallback(jsonData);
            }
            else if (operation == "description")
            {
                return this.ProcessFieldCallback(jsonData);
            }
            else
            {
                return string.Empty;
            }
        }

        protected virtual void RenderArgumentControl(HtmlTextWriter writer)
        {
            var txt = new TextBox();

            txt.ID = this.UniqueID + '_' + ArgumentControlName;
            this.CopyArgumentProperties(txt);

// Para quando passar do modo View para o modo Edit dentro de um RecordManager
            if (this.ArgumentText.Length == 0 && !this.ArgumentValue.IsNull)
            {
                this.ArgumentText = StrLib.ToStr(this.ArgumentValue);
            }

            txt.Text = this.ArgumentText;
            txt.ReadOnly = true;
            txt.RenderControl(writer); // BaseArgument
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // hypSearch deve ser filho do TSearch para que seja possível tratar o evento hypSearch.Click
            this.hypSearch = new ImageButton();
            this.hypSearch.ID = "hypSearch";
            this.hypSearch.ImageUrl = TUtil.TranslateRelativeUrl(this.SearchButtonImageUrl, this);
            this.hypSearch.CssClass = this.ButtonCssClass;
            this.Controls.Add(this.hypSearch);

            if (!this.DesignMode)
            {
                // popup
                this._popup = this.CreatePopup();
                this.Controls.Add(this._popup);

                // settings
                this._settings = new HiddenField();
                this._settings.ID = "cfg";
                this.Controls.Add(this._settings);

                this.Attributes.Add("OnFocus", "tsearchboxTextFocus('" + this._settings.ClientID + "')");
                this.Attributes.Add("OnBlur", "tsearchboxTextBlur('" + this._settings.ClientID + "')");
                this.hypSearch.Attributes.Add("OnClick", "tsearchboxButtonClick('" + this._settings.ClientID + "'); return false;");
                this.hypSearch.Attributes.Add("align", "absmiddle");

                this._callback = new ASPxCallback();
                this.Controls.Add(this._callback);
                this._callback.ID = "cllbk";
                this._callback.Callback += this._callback_Callback;
                this._callback.ClientSideEvents.CallbackComplete = "function(s, e) {tsearchboxCallbackSuccess(s,e,'" + this.ClientID + "');}";
                this._callback.ClientSideEvents.CallbackError = "function(s, e) {tsearchboxCallbackFailure(s,e,'" + this.ClientID + "');}";
            }
        }

        protected override void LoadControlState(object savedState)
        {
            var p = (Pair)savedState;
            base.LoadControlState(p.First);
            var ar = (ArrayList)p.Second;

            this.InstanceID = (string)ar[0];
            this.SqlSelect = (SqlSelect)ar[1];
            this.SqlWhere = (string)ar[2];
            this.SqlOrder = (string)ar[3];
            this.Key = (string)ar[4];
            this.Argument = (string)ar[5];
        }

        protected override bool LoadPostData(string postDataKey, 
                                             NameValueCollection postCollection)
        {
            // Verifica se houve alteração no controle que representa a descrição (argument)
            var newArgument = postCollection[this.UniqueID + '_' + ArgumentControlName];
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
            this.PersistedColumnNames = (string[])state[offset++];
            this.PersistedColumnValues = DbObject.ToDbObjectArray((object[])state[offset++]);

// _enabled = (bool)state[offset++];
            // _visible = (bool)state[offset++];
            // this.Enabled = _enabled;
            // this.Visible = _visible;

            // if (GridVisible)
            // {
            // object grdState = state[offset++];

            // // O base.LoadViewState() precisa ser chamado antes de GetAndBind() porque o primeiro
            // // recuperará propriedades utilizadas pelo segundo, como DBValue.
            // base.LoadViewState(state[offset++]);

            // GetAndBind(false);
            // grd.LoadViewStateInternal(grdState);

            // // Este DataBind() é necessário porque, apesar de estar sendo feito dentro de GetAndBind() acima,
            // // o estado (CurrentPageIndex, SortExpression, ...) é restaurado somente em LoadViewStateInternal().
            // grd.DataBind();
            // }
            // else
            base.LoadViewState(state[offset++]);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.RegisterRequiresControlState(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Detecta alterações nos controles dos quais este controle depende
            DependerLib.RegisterDepender(this);

// se não for um callback, a página está sendo recarregada
            // neste caso, limpe o conteúdo do cache para forçar uma nova busca
            if (!this.Page.IsCallback)
            {
                this.ClearDataFromCache();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.EnsureChildControls();

// Registra biblioteca comum TControl.js
            RegisterTControlScript(this);

            RegisterTSearchBoxScript(this.Page);

            this.Page.RegisterRequiresPostBack(this);

            base.OnPreRender(e);
        }

        protected override void PreTControlCtor()
        {
            base.PreTControlCtor();

            // Deve ser feito antes de inicializar DBValue
            this.ArgumentValue = DBNull.Value;
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

// else if (ArgumentChanged)
            // {
            // ClearPersistedColumns();
            // if (!DBValue.IsNull)
            // {
            // base.DBValue = DBNull.Value;
            // ArgumentValue = DBNull.Value;
            // }
            // if (ArgumentText.Length > 0)
            // Msg = ClickToSearchMsg;
            // }
        }

        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            this.EnsureChildControls();

// guarda opções do plugin no campo hidden
            this._settings.Value = Techne.Web.JSON.JsonEncode(this.GetPluginOptions());
            if (!this.AutoPostBack)
            {
                this.Attributes.Add("AutoPostBack", "false");
            }

            writer.AddStyleAttribute("display", "inline");
            writer.AddStyleAttribute("white-space", "nowrap");
            writer.AddStyleAttribute("vertical-align", "middle");

            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            base.RenderControlEditMode(writer); // BaseKey

            this.RenderArgumentControl(writer);

            if (!TControl.InDesignMode(this) && this.hypSearch != null)
            {
                this.hypSearch.Style.Add(HtmlTextWriterStyle.VerticalAlign, "bottom");
                this.hypSearch.RenderControl(writer); // Botão buscar
                this._settings.RenderControl(writer);
            }

            writer.RenderEndTag();
        }

        protected override void RenderControlViewMode(HtmlTextWriter writer)
        {
            this.EnsureChildControls();
            writer.AddStyleAttribute("display", "inline");
            writer.AddStyleAttribute("white-space", "nowrap");
            writer.AddStyleAttribute("vertical-align", "middle");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            base.RenderControlEditMode(writer, true);

            this.RenderArgumentControl(writer);

            writer.RenderEndTag();
        }

        protected override void RenderDebugInfo(HtmlTextWriter writer)
        {
            base.RenderDebugInfo(writer);

            writer.Write("<B>PersistedColumnNames: </B>" + (this.PersistedColumnNames == null ? "null" : StrLib.EnumerableToStr(this.PersistedColumnNames)) + "<BR/>");
            writer.Write("<B>PersistedColumnValues: </B>" + (this.PersistedColumnValues == null ? "null" : StrLib.EnumerableToStr(this.PersistedColumnValues)) + "<BR/>");
            writer.Write("<B>ArgumentText: </B>" + this.ArgumentText + "<BR/>");
            writer.Write("<B>ArgumentValue: </B>" + (this.ArgumentValue == null ? "null" : this.ToString(this.ArgumentValue)) + "<BR/>");

// writer.Write("<B>GridVisible: </B>" + GridVisible + "<BR/>");
            DependerLib.WriteDebugInfo(writer, (IDepender)this);
        }

        protected override void RenderExtra(HtmlTextWriter writer)
        {
            if (this._popup != null)
            {
                this._popup.RenderControl(writer);
            }

            if (this._callback != null)
            {
                this._callback.RenderControl(writer);
            }
        }

        protected override object SaveControlState()
        {
            var ar = new ArrayList();
            ar.Add(this.InstanceID);
            ar.Add(this.SqlSelect);
            ar.Add(this.SqlWhere);
            ar.Add(this.SqlOrder);
            ar.Add(this.Key);
            ar.Add(this.Argument);

            var baseState = base.SaveControlState();
            var p = new Pair();
            p.First = baseState;
            p.Second = ar;
            return p;
        }

        protected override object SaveViewState()
        {
            var list = new ArrayList();

            list.Add(this.ArgumentText);
            list.Add(this.ArgumentValue.ToObject());
            list.Add(this.PersistedColumnNames);
            list.Add(DbObject.ToObjectArray(this.PersistedColumnValues));

// list.Add(_enabled);
            // list.Add(_visible);

            // if (GridVisible)
            // list.Add(grd.SaveViewStateInternal());
            list.Add(base.SaveViewState());

            return list;
        }

        protected void ClearPersistedColumns()
        {
            this.PersistedColumnValues = new DbObject[0];
            this.pvPersistedColumnValues = null;
        }

        protected WebControl CreatePopup()
        {
            this._popup = new Panel();
            this._popup.ID = "pp";
            this._popup.Attributes.Add("skipDisableNavigationKey", "true");
            this._popup.Style.Add("position", "absolute");
            this._popup.Style.Add("top", "0px");
            this._popup.Style.Add("left", "0px");
            this._popup.Style.Add("visibility", "hidden");
            this._popup.Style.Add("background-color", "#FFFFFF");
            this._popup.ControlStyle.MergeWith(this.PopupStyle);
            if (this.PopupStyle.BorderStyle == BorderStyle.NotSet && string.IsNullOrEmpty(this.PopupStyle.CssClass))
            {
                this._popup.BorderStyle = BorderStyle.Solid;
                this._popup.BorderWidth = Unit.Pixel(1);
                this._popup.BorderColor = System.Drawing.Color.Gray;
            }

            this._gridTable = new HtmlTable();
            this._gridTable.ID = "grid";
            this._gridTable.CellPadding = 0;
            this._gridTable.CellSpacing = 0;
            this._gridTable.Attributes.Add("class", this.GridClass);
            this._popup.Controls.Add(this._gridTable);

            this._pager = new HtmlGenericControl("div");
            this._pager.ID = "pager";
            this._pager.Attributes.Add("class", this.GridClass);
            this._pager.Style.Add("text-align", "center");
            this._popup.Controls.Add(this._pager);

            return this._popup;
        }

        protected void OnSetViewModeDescription(SetViewModeDescriptionEventArgs args)
        {
            if (this.SetViewModeDescription != null)
            {
                this.SetViewModeDescription(this, args);
            }
        }

        private void ClearDataFromCache()
        {
            var key = this.InstanceID;
            HttpContext.Current.Cache.Remove(key);
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
                      (this.SqlOrder == string.Empty ? string.Empty : " ORDER BY " + this.SqlOrder);

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

        private int GetAndBind(bool FireChangedEvent)
        {
            TTextBox txt = null;
            DataTable dt = null;

            if (this.SqlSelect.ToString().Length == 0)
            {
                throw new InvalidOperationException("A propriedade " + this.ID + ".SqlSelect não foi informada");
            }

            var cn = this.GetConnection();
            cn.Open();
            try
            {
                var where = this.SqlWhere;
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
                        var indexKey = this.SqlSelect.Columns.IndexOf(this.KeyCol);
                        if (indexKey < 0)
                        {
                            keyCol = this.KeyCol;
                        }
                        else
                        {
                            keyCol = this.SqlSelect.Columns[indexKey].ToString(false);
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
                        index = this.SqlSelect.Columns.IndexOf(this.ArgumentCol);
                        if (index < 0)
                        {
                            argumentCol = this.ArgumentCol;
                        }
                        else
                        {
                            argumentCol = this.SqlSelect.Columns[index].ToString(false);
                        }
                    }

                    where += (where.Length > 0 ? " AND " : string.Empty) +
                             "(" + argumentCol + " = #" + this.UniqueID + '_' + txt.ID + "#)";

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
                            throw new InvalidOperationException("Possivelmente um erro na coluna " + (index + 1) + " do select: " + this.SqlSelect + ".", exc);
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
                    if (FireChangedEvent)
                    {
                        this.OnChanged(new ChangedEventArgs(this));
                    }

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

        private DataTable GetData()
        {
            if (this.SqlSelect.ToString().Length == 0)
            {
                throw new InvalidOperationException("A propriedade " + this.ID + ".SqlSelect não foi informada");
            }

            TConnection cn = this.DataConnection;
            var where = this.SqlWhere;
            var whereValues = new DbObject[0];
            var sql = string.Empty;

// monta comando
            TControl.GetSqlWhere(null, this.RecordContainer, this.Parent.NamingContainer, cn.Rdbms, false, ref where, ref whereValues);

            sql = this.SqlSelectExtended +
                  (where == string.Empty ? string.Empty : " WHERE " + where) +
                  (this.SqlOrder == string.Empty ? string.Empty : " ORDER BY " + this.SqlOrder);

            // verifica se os dados já estão no cache
            var ret = this.GetDataFromCache(sql, whereValues);

            if (ret == null)
            {
                cn.Open();
                try
                {
                    var tab = new QueryTable(sql);
                    tab.Query(cn, whereValues);
                    ret = tab;
                    this.SaveDataToCache(ret, sql, whereValues);
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
                finally
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }

            return ret;
        }

        private DataTable GetDataFromCache(string sql, DbObject[] whereValues)
        {
            if (HttpContext.Current == null)
            {
                return null;
            }

            DataTable tab = null;
            var key = this.InstanceID;
            if (HttpContext.Current.Cache[key] != null)
            {
                tab = HttpContext.Current.Cache[key] as DataTable;
                if (tab != null)
                {
                    var changed = false;
                    var tabsql = tab.ExtendedProperties.ContainsKey("sql") ? tab.ExtendedProperties["sql"] as string : string.Empty;
                    var tabwhereValues = tab.ExtendedProperties.ContainsKey("whereValues") ? tab.ExtendedProperties["whereValues"] as DbObject[] : null;
                    if (!tabsql.Equals(sql, StringComparison.InvariantCultureIgnoreCase))
                    {
                        changed = true;
                    }
                    else if ((tabwhereValues == null && whereValues != null) || (tabwhereValues != null && whereValues == null))
                    {
                        changed = true;
                    }
                    else if (tabwhereValues == null && whereValues == null)
                    {
                        changed = false;
                    }
                    else if (tabwhereValues.Length != whereValues.Length)
                    {
                        changed = true;
                    }
                    else
                    {
                        for (var i = 0; i < whereValues.Length; i++)
                        {
                            if (whereValues[i] != tabwhereValues[i])
                            {
                                changed = true;
                                break;
                            }
                        }
                    }

                    if (changed)
                    {
                        HttpContext.Current.Cache.Remove(key);
                        tab = null;
                    }
                }
            }

            return tab;
        }

        private Hashtable GetPluginOptions()
        {
            // salva configurações do plugin
            // tamanho da grid
            var gridWidth = -1;
            if (this.InternalGridWidth.Type == UnitType.Pixel)
            {
                gridWidth = (int)this.InternalGridWidth.Value;
            }
            else
            {
                gridWidth = 500;
            }

            var gridHeight = -1;
            if (this.InternalGridHeight.Type == UnitType.Pixel)
            {
                gridHeight = (int)this.InternalGridHeight.Value;
            }
            else
            {
                gridHeight = 300;
            }

            var options = new Hashtable();
            options.Add("text", this.ClientID);
            options.Add("description", this.ClientID + "_" + ArgumentControlName);
            options.Add("button", this.hypSearch.ClientID);
            options.Add("popup", this._popup.ClientID);
            options.Add("grid", this._gridTable.ClientID);
            options.Add("pager", this._pager.ClientID);

// options.Add("callbackMethod", "function (postdata,success,context,failure) {" + Page.ClientScript.GetCallbackEventReference(this, "postdata", "success", "context", "failure", true) + ";}");
            options.Add("callbackMethod", "function (postdata,success,context,failure) {" + this._callback.ClientID + ".PerformCallback(postdata);}");
            if (gridWidth > -1)
            {
                options.Add("width", gridWidth);
            }

            if (gridHeight > -1)
            {
                options.Add("height", gridHeight);
            }

// Nomes das colunas
            var colnames = new ArrayList();
            options.Add("colNames", colnames);
            foreach (var col in this.GridColumns)
            {
                colnames.Add(col.Caption);
            }

// detalhes das colunas
            var colModel = new ArrayList();
            options.Add("colModel", colModel);
            int colWidth;
            foreach (var col in this.GridColumns)
            {
                var colMod = new Hashtable();
                colMod.Add("name", col.FieldName.ToLower());
                colMod.Add("index", col.FieldName.ToLower());
                if (col.Width.Type == UnitType.Pixel)
                {
                    colWidth = (int)col.Width.Value;
                }
                else if (col.Width.Type == UnitType.Percentage)
                {
                    colWidth = (int)(col.Width.Value * gridWidth);
                    if (colWidth < 0)
                    {
                        colWidth = 0;
                    }
                }
                else
                {
                    colWidth = -1;
                }

                if (colWidth >= 0)
                {
                    colMod.Add("width", colWidth);
                }

                if (!col.Visible)
                {
                    colMod.Add("hidden", true);
                }

                colModel.Add(colMod);
            }

            options.Add("rowNum", "20");
            options.Add("imgpath", this.Page.ResolveClientUrl(this.GridImagePath));
            options.Add("textColumn", this.KeyCol);
            options.Add("descriptionColumn", this.ArgumentCol);
            if (this.AutoPostBack)
            {
                var pb = new PostBackOptions(this);
                pb.AutoPostBack = true;
                pb.Argument = string.Empty;
                options.Add("postback", "function(){" + this.Page.ClientScript.GetPostBackEventReference(pb) + "}; ");
            }

            if (string.IsNullOrEmpty(this.SqlOrder))
            {
                options.Add("sidx", this.Argument);
                options.Add("sord", "asc");
            }

            if (this.ValidateText)
            {
                options.Add("textValidation", true);
            }

            return options;
        }

        private string ProcessFieldCallback(Hashtable jsonData)
        {
            var retJSON = new Hashtable();

// executa query
            object ret = this.GetData();
            DataView dv = null;
            if (ret is DataTable)
            {
                dv = ((DataTable)ret).DefaultView;
            }
            else if (ret is DataView)
            {
                dv = (DataView)ret;
            }

            var key = jsonData.Contains("key") ? jsonData["key"] : string.Empty;
            dv.RowFilter = "[" + this.KeyCol + "]='" + key + "'";
            if (dv.Count > 0)
            {
                retJSON.Add("keyvalue", dv[0][this.KeyCol]);
                retJSON.Add("descriptionvalue", dv[0][this.ArgumentCol]);
                var rowData = new Hashtable();
                foreach (var col in this.GridColumns)
                {
                    rowData.Add(col.FieldName.ToLower(), dv[0][col.FieldName] == DBNull.Value ? null : dv[0][col.FieldName]);
                }

                retJSON.Add("rowdata", rowData);
            }
            else
            {
                retJSON.Add("message", "Código inválido.");
            }

            return Techne.Web.JSON.JsonEncode(retJSON);
        }

        private string ProcessGridCallback(Hashtable jsonData)
        {
            // pega parâmetros da grid
            var page = Convert.ToInt32(jsonData["page"]);
            var limit = Convert.ToInt32(jsonData["rows"]);
            if (limit < 1)
            {
                limit = 1;
            }
            else if (limit > 100)
            {
                limit = 100;
            }

            var sidx = jsonData["sidx"].ToString();
            var sord = jsonData["sord"].ToString();
            if (String.IsNullOrEmpty(sord) || (sord != "asc" && sord != "desc"))
            {
                sord = "asc";
            }

            // pega parâmetros de filtro
            var filter = new Dictionary<string, object>();
            string key;
            foreach (var col in this.GridColumns)
            {
                key = col.FieldName.ToLower();
                if (jsonData.ContainsKey(key))
                {
                    filter.Add(key, jsonData[key]);
                }
            }

            string error = null;

// json de retorno
            var retJSON = new Hashtable();
            try
            {
                // executa query
                object ret = this.GetData();
                DataView dv = null;
                if (ret is DataTable)
                {
                    dv = ((DataTable)ret).DefaultView;
                }
                else if (ret is DataView)
                {
                    dv = (DataView)ret;
                }

                if (filter.Count > 0)
                {
                    var rowFilter = new StringBuilder();
                    foreach (var pair in filter)
                    {
                        if (rowFilter.Length > 0)
                        {
                            rowFilter.Append(" and ");
                        }

                        rowFilter.Append(" [" + pair.Key + "] like '*" + pair.Value.ToString().Replace("'", string.Empty) + "*'");
                    }

                    dv.RowFilter = rowFilter.ToString();
                }
                else
                {
                    dv.RowFilter = string.Empty;
                }

                if (dv == null)
                {
                    return string.Empty;
                }
                else
                {
                    var count = dv.Count;
                    var result = count.ToString();
                    int totalpages;
                    var hasilbagi = Convert.ToDouble(result) / Convert.ToDouble(limit);
                    if (int.Parse(result) > 0)
                    {
                        totalpages = Convert.ToInt32(Math.Ceiling(hasilbagi));
                    }
                    else
                    {
                        totalpages = 0;
                    }

                    if (page > totalpages)
                    {
                        page = totalpages;
                    }
                    else if (page < 1)
                    {
                        page = 1;
                    }

                    retJSON.Add("message", string.Empty);
                    retJSON.Add("page", page);
                    retJSON.Add("total", totalpages);
                    retJSON.Add("records", dv.Count);
                    var rows = new ArrayList();
                    retJSON.Add("rows", rows);
                    try
                    {
                        if (!string.IsNullOrEmpty(sidx))
                        {
                            dv.Sort = sidx + " " + sord;
                        }
                    }
                    catch
                    {
                    }

                    ;
                    var max = page * limit;
                    if (max > dv.Count)
                    {
                        max = dv.Count;
                    }

                    if (dv.Count > 0)
                    {
                        for (var i = (page - 1) * limit; i < max; i++)
                        {
                            var reader = dv[i];
                            var row = new Hashtable();
                            rows.Add(row);

// row.Add("id", reader["id"]);
                            var cell = new ArrayList();
                            row.Add("cell", cell);
                            foreach (var col in this.GridColumns)
                            {
                                cell.Add(reader[col.FieldName].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception exDataBind)
            {
                error = exDataBind.Message;
            }

            if (error != null)
            {
                retJSON.Clear();

// string msg = Messages.QueryFailure;
                retJSON.Add("message", error);
            }

            return Techne.Web.JSON.JsonEncode(retJSON);
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            // if (eventArgument == "closeGrid")
            // GridVisible = false;
        }

        private void SaveDataToCache(DataTable tab, string sql, DbObject[] whereValues)
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            var key = this.InstanceID;
            HttpContext.Current.Cache.Remove(key);
            tab.ExtendedProperties.Add("sql", sql.ToLower());
            tab.ExtendedProperties.Add("whereValues", whereValues);
            HttpContext.Current.Cache.Add(key, tab, null, DateTime.Now.AddMinutes(2), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        void _callback_Callback(object source, CallbackEventArgs e)
        {
            var jsonData = (Hashtable)Techne.Web.JSON.JsonDecode(e.Parameter);
            var operation = string.Empty;
            if (jsonData.ContainsKey("operation"))
            {
                if (jsonData["operation"] != null)
                {
                    operation = jsonData["operation"].ToString();
                }
            }

            e.Result = this.ProcessCallback(operation, jsonData);
        }
    }
}