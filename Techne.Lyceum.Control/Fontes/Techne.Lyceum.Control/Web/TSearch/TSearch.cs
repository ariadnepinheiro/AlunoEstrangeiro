using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallback;
using Techne.Exceptions;

namespace Techne.Web
{
    [ToolboxData("<{0}:TSearch runat=\"server\" Text=\"\"></{0}:TSearch>"), SupportsEventValidation, DefaultEvent("Click"), DefaultProperty("Text"), ControlValueProperty("Value"), ValidationProperty("Text")]
    public class TSearch : WebControl, INamingContainer, IPostBackEventHandler
    {
// , IPostBackDataHandler
        private const int MaxRowsUpperLimit = 200;

        private static readonly object EventSelecting = new object();

        private readonly TSearchClientSideEvents _clientSideEvents = new TSearchClientSideEvents();

        private readonly TSearchColumnCollection _columns = new TSearchColumnCollection();

		private readonly Dictionary<string, WebControl> _filterControls = new Dictionary<string, WebControl>();

        private readonly Style _filterPanelStyle = new Style();

        private readonly Style _panelButtonStyle = new Style();

        private readonly TSearchParameterCollection _parameters = new TSearchParameterCollection();

        private readonly Style _popupStyle = new Style();

        private readonly Style _titleStyle = new Style();

        private Button _btFilter;

        private ImageButton _button;

        private string _buttonImageUrl = string.Empty;

        private Unit _buttonWidth = Unit.Pixel(20);

        private ASPxCallback _callback;

        private bool _checkRowData = true;

        private string _descriptionField = string.Empty;

        private TextBox _descriptionTextBox;

        private Panel _filterPanel;

        private string _gridClass = "scroll";

        private HtmlTable _gridTable;

        private Unit _gridWidth = Unit.Empty;

        private string _imagePath = "~/Scripts/themes/basic/images";

        private string _mask = string.Empty;

        private int _maxLength = -1;

        private int _maxRows = MaxRowsUpperLimit;

        private Label _messageLabel;

        private TSearchMessages _messages = new TSearchMessages();

        private HtmlControl _pager;

        private string _panelButtonText = "Buscar";

        private Panel _popup;

        private string _queryClass = string.Empty;

        private string _queryMethod = string.Empty;

        private object _queryObject;

        private ParameterCollection _queryParameters;

        private HiddenField _rowData;

        private HiddenField _settings;

        private string _settingsTypeName = string.Empty;

        private bool _showButton = true;

        private Table _tablePopup;

        private string _textField = string.Empty;

        private TSearchDataType _textFieldType = TSearchDataType.NotSet;

        private Unit _textFieldWidth = Unit.Pixel(110);

        private string _title = string.Empty;

        private string _valueField = string.Empty;

        private TSearchDataType _valueFieldType = TSearchDataType.NotSet;

        private TextBox _valueTextBox;

        public TSearch()
        {
            this.Width = Unit.Pixel(450);
        }

        [Description("Dispara quando o valor do controle \x00e9 alterado."), Category("Behavior")]
        public event EventHandler Changed;

        [Category("Data")]
        public event TSearchSelectingEventHandler Selecting
        {
            add
            {
                this.Events.AddHandler(EventSelecting, value);
            }

            remove
            {
                this.Events.RemoveHandler(EventSelecting, value);
            }
        }

        [Category("Behavior")]
        [Description("Dispara quando o texto do controle é alterado.")]
        public event EventHandler TextChanged;

        [Category("Behavior"), DefaultValue(true)]
        public override bool Enabled
        {
            get
            {
                this.EnsureChildControls();
                return this._valueTextBox.Enabled;
            }

            set
            {
                this.EnsureChildControls();
                this._valueTextBox.Enabled = value;
                this._descriptionTextBox.Enabled = value;
                this.RefreshSizes();
            }
        }

        [DefaultValue(typeof (Unit), "450px")]
        public override Unit Width
        {
            get
            {
                return base.Width;
            }

            set
            {
                base.Width = value;
            }
        }

        [DefaultValue(false)]
        [Category("Behavior")]
        public bool AutoPostBack { get; set; }

        [DefaultValue(""), Description("Imagem do bot\x00e3o"), Bindable(true), Category("Appearance"), Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor)), UrlProperty]
        public string ButtonImageUrl
        {
            get
            {
                return this._buttonImageUrl;
            }

            set
            {
                this._buttonImageUrl = value == null ? string.Empty : value;
            }
        }

        [DefaultValue(typeof (Unit), "20px")]
        [Category("Appearance")]
        public Unit ButtonWidth
        {
            get
            {
                return this._buttonWidth;
            }

            set
            {
                this._buttonWidth = value;
                this.RefreshSizes();
            }
        }

        [Category("Client-Side"), PersistenceMode(PersistenceMode.InnerProperty), AutoFormatDisable, Themeable(false), MergableProperty(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TSearchClientSideEvents ClientSideEvents
        {
            get
            {
                return this._clientSideEvents;
            }
        }

        [Browsable(false)]
        public DbObject DBValue
        {
            get
            {
                DbObject ret = DBNull.Value;
                object oVal = null;
                this.EnsureChildControls();
                var dataType = this.TextFieldType;
                if (this.RowDataControl != null && !string.IsNullOrEmpty(this.RowDataControl.Value))
                {
                    var json = Techne.Web.JSON.JsonDecode(this.RowDataControl.Value) as Hashtable;
                    if (json != null)
                    {
                        if (this.ValueField.Trim() != string.Empty && json.ContainsKey(this.ValueField.ToLower().Trim()))
                        {
                            oVal = json[this.ValueField.ToLower().Trim()];
                            dataType = this.ValueFieldType;
                        }
                        else
                        {
                            oVal = this.Text;
                        }
                    }
                    else
                    {
                        oVal = this.Text;
                    }
                }
                else
                {
                    oVal = this.Text;
                }

                if (oVal is string && string.IsNullOrEmpty((string)oVal))
                {
                    oVal = null;
                }

                switch (dataType)
                {
                    case TSearchDataType.NotSet: // default é string
                    case TSearchDataType.String:
                        if (oVal == null)
                        {
                            ret = DBNull.Value;
                        }
                        else
                        {
                            ret = oVal.ToString();
                        }

                        break;
                    case TSearchDataType.Decimal:
                        if (oVal == null || oVal.ToString().Length == 0)
                        {
                            ret = DBNull.Value;
                        }
                        else
                        {
                            decimal dec;
                            try
                            {
                                dec = Convert.ToDecimal(oVal);
                                ret = DbObject.ToDbObject(dec);
                            }
                            catch
                            {
                                ret = DBNull.Value;
                            }
                        }

                        break;
                    case TSearchDataType.Integer:
                        if (oVal == null || oVal.ToString().Length == 0)
                        {
                            ret = DBNull.Value;
                        }
                        else
                        {
                            int i;
                            try
                            {
                                i = Convert.ToInt32(oVal);
                                ret = DbObject.ToDbObject(i);
                            }
                            catch
                            {
                                ret = DBNull.Value;
                            }
                        }

                        break;
                }

                return ret;
            }

            set
            {
                var val = value == null ? string.Empty : value.ToString();
                this.Text = val;
            }
        }

        public string DescriptionField
        {
            get
            {
                return this._descriptionField;
            }

            set
            {
                this._descriptionField = value == null ? string.Empty : value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public Style FilterPanelStyle
        {
            get
            {
                return this._filterPanelStyle;
            }
        }

        [
            PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public TSearchColumnCollection GridColumns
        {
            get
            {
                return this._columns;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TSearchParameterCollection GridFilterParameters
        {
            get
            {
                return this._parameters;
            }
        }

        [DefaultValue(typeof (Unit), ""), Category("Appearance")]
        public Unit GridWidth
        {
            get
            {
                return this._gridWidth;
            }

            set
            {
                this._gridWidth = value;
            }
        }

        [Browsable(false)]
        public bool IsValidDBValue
        {
            get
            {
                return this.DBValue.IsNull || (this.RowDataControl != null && !string.IsNullOrEmpty(this.RowDataControl.Value));
            }
        }

        public string Mask
        {
            get
            {
                return this._mask;
            }

            set
            {
                this._mask = value == null ? string.Empty : value;
            }
        }

        public int MaxLength
        {
            get
            {
                if (this._maxLength > -1)
                {
                    return this._maxLength;
                }
                else if (this.GridFilterParameters[this.TextField] != null)
                {
                    return this.GridFilterParameters[this.TextField].MaxLength;
                }
                else
                {
                    return 0;
                }
            }

            set
            {
                this._maxLength = value < 0 ? 0 : value;
            }
        }

        public int MaxRows
        {
            get
            {
                return this._maxRows;
            }

            set
            {
                this._maxRows = (value <= 0 || value > MaxRowsUpperLimit) ? MaxRowsUpperLimit : value;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TSearchMessages Messages
        {
            get
            {
                return this._messages;
            }

            internal set
            {
                this._messages = value;
            }
        }

        public bool Modal { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style PopupStyle
        {
            get
            {
                return this._popupStyle;
            }
        }

        [DefaultValue(""), Category("Data")]
        public string QueryMethod
        {
            get
            {
                return this._queryMethod;
            }

            set
            {
                this._queryMethod = value == null ? string.Empty : value;
            }
        }

        [Editor(typeof (ParameterCollectionEditor), typeof (UITypeEditor)), MergableProperty(false), PersistenceMode(PersistenceMode.InnerProperty), DefaultValue((string)null), Category("Data")]
        public ParameterCollection QueryParameters
        {
            get
            {
                if (this._queryParameters == null)
                {
                    this._queryParameters = new ParameterCollection();
                    ((IStateManager)this._queryParameters).TrackViewState();
                }

                return this._queryParameters;
            }
        }

        [DefaultValue("")]
        [Category("Data")]
        public string QueryTypeName
        {
            get
            {
                return this._queryClass;
            }

            set
            {
                this._queryClass = value == null ? string.Empty : value;
            }
        }

        [DefaultValue(true), Category("Behavior")]
        public bool ReadOnly
        {
            get
            {
                this.EnsureChildControls();
                return this._valueTextBox.ReadOnly;
            }

            set
            {
                this.EnsureChildControls();
                this._valueTextBox.ReadOnly = value;
                this._descriptionTextBox.ReadOnly = value;
                this.RefreshSizes();
            }
        }

        [Category("Data"), DefaultValue("")]
        public string SettingsTypeName
        {
            get
            {
                return this._settingsTypeName;
            }

            set
            {
                this._settingsTypeName = value == null ? string.Empty : value;
            }
        }

        // private void RegisterStartupScript()
        // {
        // if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "tsearchscript" + this.UniqueID))
        // {
        // EnsureChildControls();
        // StringBuilder strScript = new StringBuilder();
        // strScript.AppendLine("<script type='text/javascript' language='javascript'>");
        // strScript.AppendLine("$().ready(function() {");
        // strScript.AppendLine("    $('#" + this.ClientID + "').tsearch(");
        // strScript.AppendLine("     {");
        // if (this.TextFieldVisible)
        // strScript.AppendLine("        keyFieldID: '" + _valueTextBox.ClientID + "',");
        // if(this.DescriptionFieldVisible)
        // strScript.AppendLine("        nameFieldID: '" + _descriptionTextBox.ClientID + "',");
        // if(this.ButtonVisible)
        // strScript.AppendLine("        buttonID: '" + _button.ClientID + "',");
        // strScript.AppendLine("        keyColumn: '" + this.TextField + "',");
        // strScript.AppendLine("        nameColumn: '" + this.DescriptionField + "',");
        // strScript.AppendLine("        rowDataID: '" + _rowData.ClientID + "',");
        // if(this.ValidateText)
        // strScript.AppendLine("        textValidation: true,");
        // if (this.AutoPostBack)
        // {
        // PostBackOptions options = new PostBackOptions(this);
        // options.AutoPostBack = true;
        // options.Argument = "";
        // strScript.AppendLine("        postback: function(){" + this.Page.ClientScript.GetPostBackEventReference(options) + "}, ");
        // }
        // strScript.AppendLine("        popupID: '" + _popup.ClientID + "'");
        // strScript.AppendLine("     });");
        // strScript.AppendLine("});");

        // strScript.AppendLine("</script>");

        // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "tsearchscript" + this.UniqueID, strScript.ToString());
        // }
        // }
        [DefaultValue(true)]
        public bool ShowButton
        {
            get
            {
                return this._showButton;
            }

            set
            {
                this._showButton = value;
                this.RefreshSizes();
            }
        }

        [DefaultValue(false)]
        public bool ShowTitle { get; set; }

        [DefaultValue(""), Bindable(true, BindingDirection.TwoWay)]
        public string Text
        {
            get
            {
                this.EnsureChildControls();
                return this._valueTextBox.Text;
            }

            set
            {
                this.EnsureChildControls();
                var val = value == null ? string.Empty : value;
                if (this._valueTextBox.Text != val)
                {
                    this._checkRowData = true;
                    this._valueTextBox.Text = val;
                }
            }
        }

        public string TextField
        {
            get
            {
                return this._textField;
            }

            set
            {
                this._textField = value == null ? string.Empty : value;
            }
        }

        [DefaultValue(TSearchDataType.NotSet)]
        public TSearchDataType TextFieldType
        {
            get
            {
                if (this._textFieldType != TSearchDataType.NotSet)
                {
                    return this._textFieldType;
                }
                else if (this.GridFilterParameters[this.TextField] != null)
                {
                    return this.GridFilterParameters[this.TextField].ParameterType;
                }
                else
                {
                    return TSearchDataType.NotSet;
                }
            }

            set
            {
                this._textFieldType = value;
            }
        }

        [DefaultValue(typeof (Unit), "110px")]
        [Category("Appearance")]
        public Unit TextFieldWidth
        {
            get
            {
                return this._textFieldWidth;
            }

            set
            {
                this._textFieldWidth = value;
                this.RefreshSizes();
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }

            set
            {
                this._title = value == null ? string.Empty : value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), PersistenceMode(PersistenceMode.InnerProperty), Browsable(true)]
        public Style TitleStyle
        {
            get
            {
                return this._titleStyle;
            }
        }

        [DefaultValue(false)]
        public bool ValidateText { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Bindable(true, BindingDirection.TwoWay), DefaultValue((string)null)]
        public object Value
        {
            get
            {
                return this.DBValue == null || this.DBValue.IsNull ? null : this.DBValue.ToObject();
            }

            set
            {
                if (value == null)
                {
                    this.DBValue = DBNull.Value;
                }
                else
                {
                    this.DBValue = DbObject.ToDbObject(value);
                }
            }
        }

        public string ValueField
        {
            get
            {
                return this._valueField;
            }

            set
            {
                this._valueField = value == null ? string.Empty : value;
            }
        }

        [DefaultValue(TSearchDataType.NotSet)]
        public TSearchDataType ValueFieldType
        {
            get
            {
                if (this._valueFieldType != TSearchDataType.NotSet)
                {
                    return this._valueFieldType;
                }
                else if (this.GridFilterParameters[this.ValueField] != null)
                {
                    return this.GridFilterParameters[this.ValueField].ParameterType;
                }
                else
                {
                    return TSearchDataType.NotSet;
                }
            }

            set
            {
                this._valueFieldType = value;
            }
        }

        internal object QueryObject
        {
            get
            {
                if (this._queryObject == null)
                {
                    this._queryObject = TQueryHelper.CreateObject(this.QueryTypeName);
                }

                return this._queryObject;
            }
        }

        protected HiddenField RowDataControl
        {
            get
            {
                return this._rowData;
            }
        }

        private bool ButtonVisible
        {
            get
            {
                return this.ShowButton && this.Enabled && !this.ReadOnly;
            }
        }

        private bool DescriptionFieldVisible
        {
            get
            {
                return this.DescriptionField.Trim().Length > 0;
            }
        }

        [DefaultValue("scroll")]
        private string GridClass
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

        [DefaultValue("~/Scripts/themes/basic/images")]
        private string GridImagePath
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

        private Unit InternalGridWidth
        {
            get
            {
                var gridWidth = this.GridWidth;
                if (gridWidth.IsEmpty)
                {
                    gridWidth = this.Width;
                }

                if (gridWidth.IsEmpty)
                {
                    gridWidth = Unit.Pixel(400);
                }

                return gridWidth;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Browsable(true)]
        private Style PanelButtonStyle
        {
            get
            {
                return this._panelButtonStyle;
            }
        }

        [DefaultValue("Buscar")]
        private string PanelButtonText
        {
            get
            {
                return this._panelButtonText;
            }

            set
            {
                this._panelButtonText = value;
            }
        }

        private Control PopupAnchor { get; set; }

        private bool TextFieldVisible
        {
            get
            {
                return this.TextField.Trim().Length > 0;
            }
        }

        [Browsable(false)]
        public object this[string columnName]
        {
            get
            {
                this.EnsureChildControls();
                if (this.RowDataControl == null || string.IsNullOrEmpty(this.RowDataControl.Value))
                {
                    return DBNull.Value;
                }
                else
                {
                    var json = Techne.Web.JSON.JsonDecode(this.RowDataControl.Value) as Hashtable;
                    if (json == null)
                    {
                        return DBNull.Value;
                    }
                    else if (json.ContainsKey(columnName.ToLower().Trim()))
                    {
                        var val = json[columnName.ToLower().Trim()];
                        if (val == null)
                        {
                            return DBNull.Value;
                        }
                        else
                        {
                            return val;
                        }
                    }
                    else
                    {
                        return DBNull.Value;
                    }
                }
            }
        }

        public static void RegisterTSearchScript(Page page)
        {
            string resourceName;
            resourceName = "Techne.Web.Resources.Scripts.jquery.tsearch.js";

// resourceName = "Techne.Web.Resources.Scripts.jquery.tsearch.min.js";
            if (!page.ClientScript.IsClientScriptIncludeRegistered(typeof (TSearch), "jquery.tsearch.js"))
            {
                page.ClientScript.RegisterClientScriptInclude(typeof (TSearch), "jquery.tsearch.js", 
                                                              page.ClientScript.GetWebResourceUrl(typeof (TSearch), resourceName));
            }
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (this.Page != null)
            {
                this.EnsureChildControls();
                this.Page.ClientScript.RegisterForEventValidation(this._valueTextBox.UniqueID);
            }

            this._settings.Value = Techne.Web.JSON.JsonEncode(this.GetPluginOptions());

            if (this._button != null)
            {
                if (this._buttonImageUrl.Trim() == string.Empty)
                {
                    if (this.Page != null && this.Page.ClientScript != null)
                    {
                        this._button.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Techne.Web.Resources.Images.TSearchButton.png");
                    }
                    else
                    {
                        this._button.ImageUrl = string.Empty;
                    }
                }
                else
                {
                    this._button.ImageUrl = this._buttonImageUrl;
                }
            }

            this.RefreshSizes();
            this.RefreshRowData();
            this._valueTextBox.Attributes.Add("originalValue", this._valueTextBox.Text);
            base.RenderControl(writer);
        }

        public void RaisePostBackEvent(string eventArgument)
        {
        }

        [Browsable(false)]
        public void ResetValue()
        {
            this.Text = string.Empty;
        }

        protected virtual void OnSelecting(TSearchSelectingEventArgs e)
        {
            var handler = base.Events[EventSelecting] as TSearchSelectingEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnTextChanged(object sender, EventArgs e)
        {
        }

        protected virtual string ProcessCallback(string eventArgument)
        {
            var jsonData = (Hashtable)Techne.Web.JSON.JsonDecode(eventArgument);
            if (jsonData == null)
            {
                jsonData = new Hashtable();
            }

            var operation = string.Empty;
            if (jsonData.ContainsKey("operation"))
            {
                if (jsonData["operation"] != null)
                {
                    operation = jsonData["operation"].ToString();
                }
            }

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

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.ApplySettings();

            this.Style.Add(HtmlTextWriterStyle.Display, "inline");
            this.Style.Add(HtmlTextWriterStyle.WhiteSpace, "nowrap");
            this.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");

            this._rowData = new HiddenField();
            this._rowData.ID = "rd";
            this.Controls.Add(this._rowData);
            this._settings = new HiddenField();
            this._settings.ID = "st";
            this.Controls.Add(this._settings);

            this._valueTextBox = new TextBox();
            if (this.MaxLength > 0)
            {
                this._valueTextBox.MaxLength = this.MaxLength;
            }

            this._valueTextBox.ID = "ValueBox";
            this._valueTextBox.Attributes.Add("autocomplete", "off");
            if (this.MaxLength > 0)
            {
                this._valueTextBox.MaxLength = this.MaxLength;
            }

            this.Controls.Add(this._valueTextBox);
            this._descriptionTextBox = new TextBox();
            this._descriptionTextBox.ID = "DescriptionBox";
            this._descriptionTextBox.Enabled = true;
            this._descriptionTextBox.MaxLength = 255;
            this._descriptionTextBox.Attributes.Add("autocomplete", "off");
            this.Controls.Add(this._descriptionTextBox);
            this._button = new ImageButton();
            this._button.Attributes.Add("align", "absmiddle");
            this.Controls.Add(this._button);

            this._valueTextBox.Attributes.Add("OnKeyPress", "tsearchTextFieldKeyPress('" + this._settings.ClientID + "',event);");
            this._valueTextBox.Attributes.Remove("OnChange");
            this._valueTextBox.Attributes.Add("OnChange", "tsearchTextFieldChange('" + this._settings.ClientID + "');");
            this._descriptionTextBox.Attributes.Add("OnKeyPress", "tsearchDescriptionFieldKeyPress('" + this._settings.ClientID + "',event);");
            this._descriptionTextBox.Attributes.Add("OnKeyUp", "tsearchDescriptionFieldKeyUp('" + this._settings.ClientID + "',event);");
            this._descriptionTextBox.Attributes.Add("OnBlur", "tsearchDescriptionFieldBlur('" + this._settings.ClientID + "',event);");
            this._button.Attributes.Add("OnClick", "tsearchButtonClick('" + this._settings.ClientID + "',event);");

            if (!this.DesignMode)
            {
                // adiciona Popup
                this._popup = this.CreatePopup();
                this.Controls.AddAt(0, this._popup);

                // adiciona callback
                this._callback = new ASPxCallback();
                this.Controls.Add(this._callback);
                this._callback.ID = "cllbk";
                this._callback.Callback += this._callback_Callback;
                this._callback.ClientSideEvents.CallbackComplete = "function(s, e) {tsearchCallbackSuccess(s,e,'" + this.ClientID + "');}";
                this._callback.ClientSideEvents.CallbackError = "function(s, e) {tsearchCallbackFailure(s,e,'" + this.ClientID + "');}";

                // adiciona eventos
                this._valueTextBox.TextChanged += this.ValueTextBox_ValueChanged;
            }

// ajusta larguras dos controles
            this.RefreshSizes();
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                var pair = (Pair)savedState;
                if (pair.First != null)
                {
                    base.LoadViewState(pair.First);
                }

                if (pair.Second != null)
                {
                    ((IStateManager)this._queryParameters).LoadViewState(pair.Second);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.EnsureChildControls();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.EnsureChildControls();
            this.RefreshRowData();
            RegisterTSearchScript(this.Page);
            if (!this.Page.ClientScript.IsStartupScriptRegistered(this.GetType(), this.UniqueID))
            {
                var script = new StringBuilder();
                script.AppendLine("<script language='javascript'>");
                script.AppendLine("$().ready(function() {tsearchFind('" + this._settings.ClientID + "')});");
                script.AppendLine("</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, script.ToString());
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.EnsureChildControls();
            if (this.DesignMode)
            {
                this.RenderChildren(writer);
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
                writer.AddAttribute("cfg", this._settings.ClientID);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                this.RenderChildren(writer);
                writer.RenderEndTag();
            }
        }

        protected override object SaveViewState()
        {
            var pair = new Pair();
            pair.First = base.SaveViewState();
            pair.Second = (this._queryParameters != null) ? ((IStateManager)this._queryParameters).SaveViewState() : null;
            return pair;
        }

        protected Panel CreatePopup()
        {
            this._popup = new Panel();
            this._popup.ID = "pp";
            this._popup.Attributes.Add("skipDisableNavigationKey", "true");
            this._popup.Style.Add("position", "absolute");
            this._popup.Style.Add("top", "0px");
            this._popup.Style.Add("left", "0px");
            this._popup.Style.Add("visibility", "hidden");
            this._popup.Style.Add("background-color", "#FFFFFF");
            if (this.PopupStyle.BorderStyle == BorderStyle.NotSet && string.IsNullOrEmpty(this.PopupStyle.CssClass))
            {
                this._popup.BorderStyle = BorderStyle.Solid;
                this._popup.BorderWidth = Unit.Pixel(1);
                if (this.Modal)
                {
                    this._popup.BorderColor = System.Drawing.Color.Black;
                }
                else
                {
                    this._popup.BorderColor = System.Drawing.Color.Gray;
                }
            }
            else
            {
                this._popup.MergeStyle(this.PopupStyle);
            }

            this._tablePopup = new Table();
            this._tablePopup.ID = "tb";
            this._tablePopup.CellPadding = 0;
            this._tablePopup.CellSpacing = 0;
            this._popup.Controls.Add(this._tablePopup);
            this._tablePopup.Width = this.InternalGridWidth;
            this._tablePopup.Height = Unit.Percentage(100);

            if (this.ShowTitle)
            {
                var rowTitle = new TableRow();
                this._tablePopup.Rows.Add(rowTitle);
                var cellTitle = new TableCell();
                rowTitle.Cells.Add(cellTitle);
                cellTitle.Text = this.Title;
                cellTitle.ControlStyle.MergeWith(this.TitleStyle);
            }

            var row1 = new TableRow();
            this._tablePopup.Rows.Add(row1);
            var cell1 = new TableCell();
            row1.Cells.Add(cell1);
            cell1.Height = Unit.Pixel(20);
            cell1.Attributes.Add("height", "20");
            var row2 = new TableRow();
            this._tablePopup.Rows.Add(row2);
            var cell2 = new TableCell();
            row2.Cells.Add(cell2);
            cell2.VerticalAlign = VerticalAlign.Top;
            cell2.Height = Unit.Percentage(100);
            cell2.Attributes.Add("height", "100%");

            this._filterPanel = this.CreateFilterPanel();
            if (this._filterPanel != null)
            {
                cell1.Controls.Add(this._filterPanel);
            }

            this._gridTable = new HtmlTable();
            this._gridTable.ID = "grid";
            this._gridTable.CellPadding = 0;
            this._gridTable.CellSpacing = 0;
            this._gridTable.Attributes.Add("class", this.GridClass);
            cell2.Controls.Add(this._gridTable);

            this._pager = new HtmlGenericControl("div");
            this._pager.ID = "pager";
            this._pager.Attributes.Add("class", this.GridClass);
            this._pager.Style.Add("text-align", "center");
            cell2.Controls.Add(this._pager);

            return this._popup;
        }

        protected IDictionary<string, object> GetParameterValues()
        {
            IOrderedDictionary parameters = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);

            foreach (DictionaryEntry entry in this.QueryParameters.GetValues(HttpContext.Current, this))
            {
                var key = entry.Key == null ? string.Empty : entry.Key.ToString();
                while (key.StartsWith("@"))
                {
                    key = key.Remove(0, 1);
                }

                parameters[key] = entry.Value;
            }

// dispara evento "Selecting", permitindo mudança manual de parâmetros
            var e = new TSearchSelectingEventArgs(parameters);
            this.OnSelecting(e);

            var ret = new Dictionary<string, object>();
            foreach (DictionaryEntry entry in parameters)
            {
                ret.Add(entry.Key.ToString(), entry.Value);
            }

            return ret;
        }

        protected string ProcessFieldCallback(Hashtable jsonData)
        {
            var key = jsonData.ContainsKey("key") ? jsonData["key"] as string : string.Empty;
            var pars = this.GetParameterValues();
            var rowData = TQueryHelper.GetRowJSON(this.QueryTypeName, this.QueryMethod, pars, key);

            var retJson = new Hashtable();
            if (rowData == null)
            {
// não encontrado
                retJson.Add("message", this.Messages.KeyNotFound);
            }
            else
            {
                var name = string.Empty;

                if (rowData.ContainsKey(this.DescriptionField.ToLower()) &&
                    rowData[this.DescriptionField.ToLower()] != null)
                {
                    name = rowData[this.DescriptionField.ToLower()].ToString();
                }

                retJson.Add("keyvalue", key);
                retJson.Add("namevalue", name);
                retJson.Add("rowdata", rowData);
            }

            return Techne.Web.JSON.JsonEncode(retJson);
        }

        protected string ProcessGridCallback(Hashtable jsonData)
        {
            bool erroPreenchimento = false;
            var oper = jsonData.ContainsKey("operation") ? jsonData["operation"].ToString() : string.Empty;

// se for busca avançada
            if (oper == "gridSearch")
            {
                // pega parâmetros hard-coded
                var pars = this.GetParameterValues();

                // Pega parâmetros enviados no json
                var jsonPars = new Dictionary<string, object>();
                var filter = jsonData["filter"] as ArrayList;
                if (filter != null)
                {
                    foreach (var o in filter)
                    {
                        var f = o as Hashtable;
                        if (f.ContainsKey("name") && f.ContainsKey("value"))
                        {
                            if (jsonPars.ContainsKey(f["name"].ToString()))
                            {
                                jsonPars[f["name"].ToString()] = f["value"];
                            }
                            else
                            {
                                jsonPars.Add(f["name"].ToString(), f["value"]);
                            }
                        }
                    }
                }

                // sobrescreve parâmetros hard coded com os valores do jason
                var i = 0;
                foreach (var par in this.GridFilterParameters)
                {
                    if (jsonPars.ContainsKey(par.ParameterName))
                    {
                        if (!pars.ContainsKey(par.ParameterName))
                        {
                            pars.Add(par.ParameterName, jsonPars[par.ParameterName]);
                        }
                        else
                        {
                            pars[par.ParameterName] = jsonPars[par.ParameterName];
                        }
                    }

                    i++;
                }

// pega parâmetros da grid
                var page = Convert.ToInt32(jsonData["page"]);
                var limit = Convert.ToInt32(jsonData["rows"]);
                var sidx = jsonData["sidx"].ToString();
                var sord = jsonData["sord"].ToString();
                var count = 1000;
                var result = count.ToString();
                if (String.IsNullOrEmpty(sidx))
                {
                    sidx = "1";
                }

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

                if (Convert.ToInt32(page) > totalpages)
                {
                    page = totalpages;
                }

                var start = limit * page - limit;

                if (start < 0)
                {
                    start = 0;
                }

                string error = null;
                var hasMoreRows = false;
                var maxRows = this.MaxRows;
                if (maxRows < 1 || maxRows > 200)
                {
                    maxRows = 200;
                }

                // json de retorno
                var retJSON = new Hashtable();
                try
                {
                    // executa query
                    var ret = TQueryHelper.ExecuteQuery(this.QueryTypeName, this.QueryMethod, pars, null, maxRows + 1);
                    DataView dv = null;
                    if (ret is DataTable)
                    {
                        dv = ((DataTable)ret).DefaultView;
                    }
                    else if (ret is DataView)
                    {
                        dv = (DataView)ret;
                    }

                    if (dv == null || dv.Count == 0)
                    {
                        retJSON.Add("message", this.EmptyMessage());
                    }
                    else
                    {
                        // valida as colunas, se estiver em modo de debug
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            foreach (var col in this.GridColumns)
                            {
                                if (!dv.Table.Columns.Contains(col.FieldName))
                                {
                                    throw new Exception("A coluna " + col.FieldName + " não existe na grade de resultados do controle " + this.ID);
                                }
                            }
                        }

                        if (dv.Count > maxRows)
                        {
                            dv.Table.Rows.Remove(dv.Table.Rows[dv.Table.Rows.Count - 1]);
                            hasMoreRows = true;
                        }

                        retJSON.Add("message", string.Empty);
                        retJSON.Add("page", 1);
                        retJSON.Add("total", 1);
                        retJSON.Add("records", dv.Count);
                        var rows = new ArrayList();
                        retJSON.Add("rows", rows);
                        try
                        {
                            dv.Sort = sidx + " " + sord;
                        }
                        catch
                        {
                        }

                        ;
                        foreach (DataRowView reader in dv)
                        {
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
                catch (Exception exDataBind)
                {
                    if (exDataBind.InnerException is GetDataException && !string.IsNullOrEmpty(exDataBind.InnerException.Message))
                    {
                        erroPreenchimento = true;
                        error = exDataBind.InnerException.Message;
                    }
                    else
                    {
                        error = exDataBind.Message;
                    }
                }

                if (error != null)
                {
                    string msg;
                    retJSON.Clear();
                    if (System.Diagnostics.Debugger.IsAttached || erroPreenchimento)
                    {
                        msg = error;
                    }
                    else
                    {
                        msg = this.Messages.QueryFailure;
                    }

                    retJSON.Add("message", string.Format(msg, maxRows));
                }
                else if (hasMoreRows && maxRows > 0)
                {
                    var msg = this.Messages.TooManyRows;
                    retJSON["message"] = string.Format(msg, maxRows);
                }

                return Techne.Web.JSON.JsonEncode(retJSON);
            }
            else
            {
                return string.Empty;
            }
        }

        private void ApplySettings()
        {
            if (string.IsNullOrEmpty(this.SettingsTypeName))
            {
                return;
            }

            var settings = TQueryHelper.CreateObject(this.SettingsTypeName) as TSearchSettings;
            if (settings != null)
            {
                if (this.TextField == string.Empty)
                {
                    this.TextField = settings.TextField;
                }

                if (this.DescriptionField == string.Empty)
                {
                    this.DescriptionField = settings.DescriptionField;
                }

                if (this.ValueField == string.Empty)
                {
                    this.ValueField = settings.ValueField;
                }

                if (this.QueryTypeName == string.Empty)
                {
                    this.QueryTypeName = settings.QueryTypeName;
                }

                if (this.QueryMethod == string.Empty)
                {
                    this.QueryMethod = settings.QueryMethod;
                }

                if (this.MaxRows == MaxRowsUpperLimit)
                {
                    this.MaxRows = settings.MaxRows;
                }

                if (this.ValueFieldType == TSearchDataType.NotSet)
                {
                    this.ValueFieldType = settings.ValueFieldType;
                }

                if (this.TextFieldType == TSearchDataType.NotSet)
                {
                    this.TextFieldType = settings.TextFieldType;
                }

                if (this.MaxLength == 0)
                {
                    this.MaxLength = settings.MaxLength;
                }

                if (this.Mask == string.Empty)
                {
                    this.Mask = settings.Mask;
                }

                if (this.GridColumns.Count == 0)
                {
                    foreach (var col in settings.GridColumns)
                    {
                        this.GridColumns.Add(col);
                    }
                }

                if (this.GridFilterParameters.Count == 0)
                {
                    foreach (var par in settings.GridFilterParameters)
                    {
                        this.GridFilterParameters.Add(par);
                    }
                }

                if (this.GridWidth.IsEmpty)
                {
                    this.GridWidth = settings.GridWidth;
                }

                this.Messages = settings.Messages;
            }
        }

        private Panel CreateFilterPanel()
        {
            var pnl = new Panel();
            pnl.ID = "filterPanel";
            pnl.ControlStyle.MergeWith(this.FilterPanelStyle);
            if (this.GridFilterParameters.Count == 0)
            {
                return null;
            }

            TableRow row = null;
            var tab = new Table();
            tab.BorderStyle = BorderStyle.None;
            tab.Width = Unit.Percentage(100);
            tab.CellSpacing = 0;
            tab.CellPadding = 2;
            pnl.Controls.Add(tab);

            var i = 0;
            this._filterControls.Clear();
            foreach (var par in this.GridFilterParameters)
            {
                if (!par.ShowInFilterPanel)
                {
                    continue;
                }

                if (i % 2 == 0)
                {
                    row = new TableRow();
                    tab.Rows.Add(row);
                }
                else
                {
                    var cellMiddle = new TableCell();
                    cellMiddle.Width = Unit.Pixel(20);
                    row.Cells.Add(cellMiddle);
                    cellMiddle.Text = "   ";
                }

// caption
                var cellCaption = new TableCell();
                cellCaption.Width = Unit.Percentage(5);
                row.Cells.Add(cellCaption);
                var lblCaption = new Label();
                lblCaption.Text = par.Caption;
                cellCaption.Controls.Add(lblCaption);
                var cellField = new TableCell();
                cellField.Width = Unit.Percentage(45);
                
                
                if (par.ParameterType == TSearchDataType.Boolean)
                {
					var checkBox = new CheckBox();
					checkBox.ID = "filter" + i;
					if (par.DefaultValue == "checked")
					{
						checkBox.Checked = true;
					}
					else
					{
						checkBox.Checked = false;
					}

					this._filterControls.Add(par.ParameterName.ToLower().Trim(), checkBox);
					cellField.Controls.Add(checkBox);
                }
                else {
					var textBox = new TextBox();
					textBox.Attributes.Add("autocomplete", "off");
					if (par.MaxLength > 0)
					{
						textBox.MaxLength = par.MaxLength;
					}

					textBox.Attributes.Add("filterMaxLength", textBox.MaxLength.ToString());
					textBox.ID = "filter" + i;
					textBox.Width = Unit.Percentage(95);
					if (par.Mask.Length > 0)
					{
						textBox.Attributes.Add("mask", par.Mask);
					}
					else if (par.ParameterType == TSearchDataType.Integer)
					{
						textBox.Attributes.Add("charFilter", "integer");
					}
					else if (par.ParameterType == TSearchDataType.Decimal)
					{
						textBox.Attributes.Add("charFilter", "decimal");
					}

					this._filterControls.Add(par.ParameterName.ToLower().Trim(), textBox);
					cellField.Controls.Add(textBox);
                }
                row.Cells.Add(cellField);
                i++;
            }

            if (i % 2 == 1)
            {
                var cellEmpty = new TableCell();
                cellEmpty.ColumnSpan = 3;
                row.Cells.Add(cellEmpty);
            }

            // adiciona linha para o botão Buscar e para o Label de mensagens
            row = new TableRow();
            tab.Rows.Add(row);
            var cell = new TableCell();
            cell.ColumnSpan = 5;
            row.Cells.Add(cell);

// cria uma tabela para conter mensagem e botão
            var tableBottom = new Table();
            tableBottom.CellPadding = 0;
            tableBottom.CellSpacing = 0;
            tableBottom.BorderStyle = BorderStyle.None;
            tableBottom.Width = Unit.Percentage(100);
            cell.Controls.Add(tableBottom);
            var rowBottom = new TableRow();
            tableBottom.Rows.Add(rowBottom);
            var cellMessage = new TableCell();
            var cellButtom = new TableCell();
            rowBottom.Cells.Add(cellMessage);
            rowBottom.Cells.Add(cellButtom);
            cellMessage.Width = Unit.Percentage(80);
            cellButtom.Width = Unit.Percentage(20);

// adiciona label de mensagem
            this._messageLabel = new Label();
            this._messageLabel.ID = "msgLbl";
            this._messageLabel.ForeColor = System.Drawing.Color.DarkRed;
            cellMessage.Controls.Add(this._messageLabel);

// adiciona botão de busca
            cellButtom.HorizontalAlign = HorizontalAlign.Right;
            this._btFilter = new Button();
            this._btFilter.ID = "filterButton";
            this._btFilter.Text = this.PanelButtonText;
            this._btFilter.ControlStyle.MergeWith(this.PanelButtonStyle);
            this._btFilter.CausesValidation = true;
            this._btFilter.ValidationGroup = this.UniqueID + "Panel";
            cellButtom.Controls.Add(this._btFilter);
            pnl.DefaultButton = this._btFilter.ID;
            return pnl;
        }

        private string EmptyMessage()
        {
            return "Nenhum registro encontrado";
        }

        private Hashtable GetPluginOptions()
        {
            string width;
            var options = new Hashtable();
            if (this.Modal)
            {
                options.Add("modal", true);
            }

            if (this.PopupAnchor != null)
            {
                options.Add("anchorId", this.PopupAnchor.ClientID);
            }

            if (this._btFilter != null)
            {
                options.Add("searchButtonId", this._btFilter.ClientID);
            }

            if (this._gridTable != null)
            {
                options.Add("gridId", this._gridTable.ClientID);
            }

            if (this._pager != null)
            {
                options.Add("pagerId", this._pager.ClientID);
            }

            if (this._messageLabel != null)
            {
                options.Add("messageId", this._messageLabel.ClientID);
            }

// campos de filtro
            if (this._filterControls.Count > 0)
            {
                var filterFields = new ArrayList();
                options.Add("filterFields", filterFields);
                foreach (var name in this._filterControls.Keys)
                {
                    var f = new Hashtable();
                    f.Add("name", name);
                    f.Add("id", this._filterControls[name].ClientID);
                    if (!string.IsNullOrEmpty(this._filterControls[name].Attributes["mask"]))
                    {
                        f.Add("mask", this._filterControls[name].Attributes["mask"]);
                        this._filterControls[name].Attributes.Remove("mask");
                    }
                    else if (!string.IsNullOrEmpty(this._filterControls[name].Attributes["charFilter"]))
                    {
                        f.Add("charFilter", this._filterControls[name].Attributes["charFilter"]);
                        this._filterControls[name].Attributes.Remove("charFilter");
                    }

                    filterFields.Add(f);
                }
            }

// Nomes das colunas
            var colNames = new ArrayList();
            options.Add("colNames", colNames);
            foreach (var col in this.GridColumns)
            {
                colNames.Add(col.Caption);
            }

            width = this.InternalGridWidth.Type == UnitType.Pixel ? this.InternalGridWidth.Value.ToString() : this.InternalGridWidth.ToString();
            options.Add("width", width);

// detalhes das colunas
            var colModel = new ArrayList();
            options.Add("colModel", colModel);
            foreach (var col in this.GridColumns)
            {
                width = col.Width.Type == UnitType.Pixel ? col.Width.Value.ToString() : col.Width.ToString();
                var f = new Hashtable();
                f.Add("name", col.FieldName);
                f.Add("index", col.FieldName);
                f.Add("width", width);
                f.Add("hidden", !col.Visible);
                colModel.Add(f);
            }

            options.Add("rowNum", this.MaxRows);
            options.Add("imgpath", this.Page.ResolveClientUrl(this.GridImagePath));
            if (this._callback != null)
            {
                options.Add("callbackMethod", "function (postdata,success,context,failure) {" + this._callback.ClientID + ".PerformCallback(postdata);}");
            }

            options.Add("tsID", this.ClientID);
            if (this.TextFieldVisible)
            {
                options.Add("keyFieldID", this._valueTextBox.ClientID);
            }

            if (this.DescriptionFieldVisible)
            {
                options.Add("nameFieldID", this._descriptionTextBox.ClientID);
            }

            if (this.ButtonVisible)
            {
                options.Add("buttonID", this._button.ClientID);
            }

            options.Add("keyColumn", this.TextField);
            options.Add("nameColumn", this.DescriptionField);
            options.Add("rowDataID", this._rowData.ClientID);
            if (this.ValidateText)
            {
                options.Add("textValidation", true);
            }

            options.Add("disabled", !this.Enabled);
            if (this.AutoPostBack)
            {
                var pboptions = new PostBackOptions(this);
                pboptions.AutoPostBack = true;
                pboptions.Argument = string.Empty;
                options.Add("postback", "function(){" + this.Page.ClientScript.GetPostBackEventReference(pboptions) + "}");
            }

            if (this._popup != null)
            {
                options.Add("popupID", this._popup.ClientID);
            }

// se a máscara for definida, o filtro de caracteres por tipo é desabilitado
            if (this.Mask.Length > 0)
            {
                options.Add("mask", this.Mask);
            }
            else if (this.TextFieldType == TSearchDataType.Integer)
            {
                options.Add("charFilter", "integer");
            }
            else if (this.TextFieldType == TSearchDataType.Decimal)
            {
                options.Add("charFilter", "decimal");
            }

            // eventos do lado do cliente
            if (this.ClientSideEvents.ValueChanged.Trim().Length > 0)
            {
                options.Add("onValueChanged", this.ClientSideEvents.ValueChanged);
            }

            return options;
        }

        private void RefreshRowData()
        {
            if (!this._checkRowData)
            {
                return;
            }

            if (string.IsNullOrEmpty(this._valueTextBox.Text.Trim()))
            {
                this._descriptionTextBox.Text = string.Empty;
                this._rowData.Value = string.Empty;
            }
            else
            {
                // se alguma chave foi escolhida, pega os dados da linha e a descrição
                var refreshData = false;
                if (string.IsNullOrEmpty(this._rowData.Value.Trim()))
                {
                    refreshData = true;
                }
                else
                {
                    Hashtable currRow = null;
                    try
                    {
                        currRow = Techne.Web.JSON.JsonDecode(this._rowData.Value) as Hashtable;
                        if (currRow != null && currRow.ContainsKey(this.TextField.ToLower()) && currRow[this.TextField.ToLower()] != null)
                        {
                            if (this._valueTextBox.Text.Trim().ToLower() != currRow[this.TextField.ToLower()].ToString().Trim().ToLower())
                            {
                                refreshData = true;
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                if (refreshData)
                {
                    try
                    {
                        var pars = this.GetParameterValues();
                        object key = this._valueTextBox.Text.Trim();
                        var row = TQueryHelper.GetRowJSON(this.QueryTypeName, this.QueryMethod, pars, key);
                        if (row != null)
                        {
                            this._rowData.Value = Techne.Web.JSON.JsonEncode(row);
                            if (row.ContainsKey(this.DescriptionField.ToLower()) && row[this.DescriptionField.ToLower()] != null)
                            {
                                this._descriptionTextBox.Text = row[this.DescriptionField.ToLower()].ToString();
                            }
                            else
                            {
                                this._descriptionTextBox.Text = string.Empty;
                            }
                        }
                        else
                        {
                            this._rowData.Value = string.Empty;
                            this._descriptionTextBox.Text = string.Empty;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            this._checkRowData = false;
        }

        private void RefreshSizes()
        {
            // sai se os controles filhos ainda não foram criados
            if (this._valueTextBox == null)
            {
                return;
            }

            this._button.Visible = this.ButtonVisible;
            this._valueTextBox.Visible = this.TextFieldVisible;
            this._descriptionTextBox.Visible = this.DescriptionFieldVisible;

            if (this.DesignMode)
            {
                this._valueTextBox.Visible = true;
                this._descriptionTextBox.Visible = true;
                this._button.Visible = true;
            }

            // ancora
            if (this.TextFieldVisible)
            {
                this.PopupAnchor = this._valueTextBox;
            }
            else if (this.DescriptionFieldVisible)
            {
                this.PopupAnchor = this._descriptionTextBox;
            }
            else if (this.ButtonVisible)
            {
                this.PopupAnchor = this._button;
            }

            // só o botão visível
            if (this.ButtonVisible && !this.TextFieldVisible && !this.DescriptionFieldVisible)
            {
                if (this._valueTextBox != null)
                {
                    this._valueTextBox.Width = Unit.Pixel(0);
                }

                if (this._descriptionTextBox != null)
                {
                    this._descriptionTextBox.Width = Unit.Pixel(0);
                }
            }
            else
            {
                if (this.TextFieldVisible)
                {
                    if (this.TextFieldWidth.Type == UnitType.Pixel)
                    {
                        this._valueTextBox.Width = this.TextFieldWidth;
                    }
                }
                else
                {
                    if (this.ButtonWidth.Type == UnitType.Pixel && this.Width.Type == UnitType.Pixel)
                    {
                        var textWidth = this.Width.Value - this.ButtonWidth.Value;
                        if (textWidth > 0)
                        {
                            this._valueTextBox.Width = Unit.Pixel((int)textWidth);
                        }
                    }
                }

                if (this.DescriptionFieldVisible)
                {
                    if (this.TextFieldWidth.Type == UnitType.Pixel && this.Width.Type == UnitType.Pixel &&
                        this._descriptionTextBox != null)
                    {
                        this._descriptionTextBox.Width = Unit.Pixel((int)(this.Width.Value - this.TextFieldWidth.Value));
                    }
                }

                // if (this.ButtonVisible)
                // {
                // if (ButtonWidth.Type == UnitType.Pixel)
                // _button.Width = ButtonWidth;
                // }
            }
        }

        void ValueTextBox_ValueChanged(object sender, EventArgs e)
        {
            this.OnTextChanged(sender, e);
            this.RaiseBubbleEvent(this, e);

            if (this.TextChanged != null && (this.Page == null || !this.Page.IsCallback))
            {
                this.TextChanged(this, e);
            }

            if (this.Changed != null && (this.Page == null || !this.Page.IsCallback))
            {
                this.Changed(this, e);
            }
        }

        void _callback_Callback(object source, CallbackEventArgs e)
        {
            e.Result = this.ProcessCallback(e.Parameter);
        }

        public bool ValidateKey(string key)
        {
            bool isValid = false;

            if (!String.IsNullOrEmpty(key))
            {
                object ret = TQueryHelper.ExecuteQuery(this.QueryTypeName, this.QueryMethod, new Dictionary<string, object>(), key ?? "0", 1);

                DataView dv = null;
                if (ret is DataTable)
                {
                    dv = ((DataTable)ret).DefaultView;
                }
                else if (ret is DataView)
                {
                    dv = (DataView)ret;
                }

                isValid = dv != null && dv.Count > 0;
            }

            return isValid;
        }
    }

    public delegate void TSearchSelectingEventHandler(object sender, TSearchSelectingEventArgs e);

    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class TSearchSelectingEventArgs : EventArgs
    {
        private readonly IOrderedDictionary _inputParameters;

        public TSearchSelectingEventArgs(IOrderedDictionary inputParameters)
        {
            this._inputParameters = inputParameters;
        }

        public IOrderedDictionary InputParameters
        {
            get
            {
                return this._inputParameters;
            }
        }
    }
}