using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.UI;

namespace Techne.Controls
{
    public enum ControlMode
    {
        View, 
        Edit
    }

    public enum ValidationOption
    {
        NotSet, 
        False, 
        True
    }

    public delegate void ChangedEventHandler(object sender, ChangedEventArgs args);

    [
        DefaultEvent("Changed")
    ]
    public abstract class TControlEditable : TControl, ITControlEditable
    {
        public const ValidationOption DataTypeValidation_Def = ValidationOption.True;

        public const bool EnableClientScript_Def = true;

        public const bool IndexedColumn_Def = false;

        public const ValidationOption RequiredFieldValidation_Def = ValidationOption.NotSet;

        private const bool FollowContainerMode_Def = true;

        private string fieldName;

        private string maximumValue;

        private string minimumValue;

        public TControlEditable()
        {
            this.DataTypeValidation = DataTypeValidation_Def;
            this.EnableClientScript = EnableClientScript_Def;
            this.FieldName = string.Empty;
            this.FollowContainerMode = FollowContainerMode_Def;
            this.IndexedColumn = IndexedColumn_Def;
            this.KeepValueAfterSave = false;
            this.MaximumValue = string.Empty;
            this.MinimumValue = string.Empty;
            this.ReadOnly = false;
            this.RequiredFieldValidation = RequiredFieldValidation_Def;

            this.ViewState["Configured"] = false;
        }

        [
            Category("Techne"), 
            Description("Dispara quando o valor do controle é alterado.")
        ]
        public event ChangedEventHandler Changed;

        public override DbObject DBValue
        {
            get
            {
                return base.DBValue;
            }

            set
            {
                if (!this.SettingState && this.Manager != null && this.ColumnName.Length > 0 && this.Mode != ControlMode.Edit)
                {
                    throw new InvalidOperationException(
                        "A propriedade " + this.UniqueID + ".DBValue não pode ser alterada no modo " + this.Mode + " " +
                        "devido ao controle pertencer ao manager " + ((Control)this.Manager).UniqueID
                        );
                }

                base.DBValue = value;
            }
        }

        [Category("Techne - Validação"), DefaultValue(DataTypeValidation_Def), Description("Valida tipo de dado no cliente (browser)")]
        public ValidationOption DataTypeValidation { get; set; }

        [Category("Techne - Validação"), DefaultValue(EnableClientScript_Def), Description("Define se roda as validações no cliente")]
        public bool EnableClientScript { get; set; }

        [
            Category("Techne - Validação"), 
            DefaultValue(""), 
        ]
        public string FieldName
        {
            get
            {
                return this.fieldName;
            }

            set
            {
                this.fieldName = value == null ? string.Empty : value.Trim();
            }
        }

        [Category("Techne"), DefaultValue(FollowContainerMode_Def), Description("Se ColumnName não foi informado, o valor false fará com que o controle " + "permaneça em modo Edit, independentemente do modo em que o Manager se encontre."),]
        public bool FollowContainerMode { get; set; }

        [Category("Techne"), DefaultValue(IndexedColumn_Def)]
        public bool IndexedColumn { get; set; }

        [Category("Techne"), DefaultValue(false), Description("Determina se o controle mantém seu valor após salvar o registro. " + "Normalmente utilizado em record managers cuja propriedade DataEntry é true.")]
        public bool KeepValueAfterSave { get; set; }

        [
            Category("Techne - Validação"), 
            DefaultValue(""), 
            Description("Valor máximo aceito pelo campo. Na checagem feita no cliente (browser).")
        ]
        public string MaximumValue
        {
            get
            {
                return this.maximumValue;
            }

            set
            {
                this.maximumValue = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Techne - Validação"), 
            DefaultValue(""), 
            Description("Valor mínimo aceito pelo campo. Na checagem feita no cliente (browser).")
        ]
        public string MinimumValue
        {
            get
            {
                return this.minimumValue;
            }

            set
            {
                this.minimumValue = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public ControlMode Mode
        {
            get
            {
                return (ControlMode)this.ViewState["ControlMode"];
            }

            set
            {
                this.ViewState["ControlMode"] = value;
            }
        }

        [Category("Techne"), DefaultValue(false), Description("Determina se o controle aceita ou não entrada de dados")]
        public bool ReadOnly { get; set; }

        [Category("Techne - Validação"), DefaultValue(RequiredFieldValidation_Def), Description("Valida obrigatoriedade do campo no cliente")]
        public ValidationOption RequiredFieldValidation { get; set; }

        [
            Browsable(false), 
            EditorBrowsable(EditorBrowsableState.Never), 
        ]
        protected abstract string EntryValue { get; }

        /// <summary>
        ///   Indica se há algum manager fora do modo View que referencia este controle.
        /// </summary>
        protected bool Blocked
        {
            get
            {
                var blocked = false;
                foreach (var control in this.Dependers)
                {
                    if (control is IContainerManager)
                    {
                        var manager = (IContainerManager)control;
                        if (manager != null && !ContainerManagerLib.AllContainersInMode(manager, RecordManagerMode.View))
                        {
                            blocked = true;
                            break;
                        }
                    }
                }

                return blocked;
            }
        }

        protected TPage TPage
        {
            get
            {
                if (!(this.Page is TPage))
                {
                    throw new Exception("A página não é derivada de Techne.TPage.");
                }

                return (TPage)this.Page;
            }
        }

        /// <summary>
        ///   Indica se RenderControlEditMode() será chamado no lugar de RenderControlViewMode().
        /// </summary>
        protected bool WillRenderEdit
        {
            get
            {
                return !this.ReadOnly && (this.Mode == ControlMode.Edit || InDesignMode(this)) && !this.Blocked;
            }
        }

        /// <summary>
        ///   Executa a validação do controle do lado do servidor e retorna o erro
        /// </summary>
        /// <returns>Mensagem de Erro. String.Empty se não houver erros</returns>
        public virtual string GetValueError()
        {
            // Valida obrigatoriedade do campo
            if (this.RequiredFieldValidation == ValidationOption.True)
            {
                if (this.EntryValue.Length == 0)
                {
                    return "Preenchimento obrigatório";
                }
            }

            // Valida tipo do campo
            if (this.EntryValue.Length > 0 && this.DataTypeValidation == ValidationOption.True)
            {
                if (this.DBValue.Type == DbType.Null)
                {
                    if (this.DataType == DbType.Date)
                    {
                        return "Data inválida";
                    }

                    if (this.DataType == DbType.Number)
                    {
                        return "Número inválido";
                    }
                }
            }

            // Valida limites
            string smin = this.MinimumValue, smax = this.MaximumValue;
            if (this.DBValue.Type != DbType.Null && (smin.Length > 0 || smax.Length > 0))
            {
                var message = string.Empty;
                if (smin.Length > 0 && smax.Length > 0)
                {
                    message = "{0} deve estar entre {1} e {2}";
                }
                else if (smin.Length > 0)
                {
                    message = "{0} deve ser igual ou superior {1}";
                }
                else
                {
                    message = "{0} deve ser igual ou inferior a {2}";
                }

                if (this.DataType == DbType.Date)
                {
                    var dtmin = smin.Length > 0 ? DateTime.Parse(smin) : DateTime.MinValue;
                    var dtmax = smax.Length > 0 ? DateTime.Parse(smax) : DateTime.MaxValue;
                    if ((DateTime)this.DBValue < dtmin || (DateTime)this.DBValue > dtmax)
                    {
                        return string.Format(message, this.FieldName.Length > 0 ? "Valor" : this.FieldName, dtmin.ToShortDateString(), dtmax.ToShortDateString());
                    }
                }

                if (this.DataType == DbType.Number)
                {
                    var dcmin = smin.Length > 0 ? decimal.Parse(smin) : decimal.MinValue;
                    var dcmax = smax.Length > 0 ? decimal.Parse(smax) : decimal.MaxValue;
                    if ((decimal)this.DBValue < dcmin || (decimal)this.DBValue > dcmax)
                    {
                        return string.Format(message, this.FieldName.Length > 0 ? "Valor" : this.FieldName, dcmin, dcmax);
                    }
                }
            }

            return string.Empty;
        }

        public override void ResetValue()
        {
            if (!this.SettingState && this.Manager != null && this.ColumnName.Length > 0 && this.Mode != ControlMode.Edit)
            {
                throw new InvalidOperationException(
                    "O método " + this.UniqueID + ".ResetValue() não é permitido no modo " + this.Mode + " " +
                    "devido ao controle pertencer ao manager " + ((Control)this.Manager).UniqueID
                    );
            }

            if (this.ColumnName.Length > 0 && this.Manager != null && this.Manager.Table != null && this.Manager.Table.ContainsColumn(this.ColumnName))
            {
                // Inicializa o controle com o valor default da coluna à qual está associado.
                // O valor default é definido no banco.
                this.DBValue = DbObject.ToDbObject(this.Manager.Table.Columns[this.ColumnName].DefaultValue);
            }
            else
            {
                base.ResetValue();
            }
        }

        protected internal virtual void ConfigureObject()
        {
        }

        protected abstract void RenderControlEditMode(HtmlTextWriter writer);

        protected virtual void OnChanged(ChangedEventArgs args)
        {
            this.RaiseBubbleEvent(this, args);

            if (this.Changed != null)
            {
                this.Changed(this, args);
            }
        }

        protected override void LoadViewState(object savedState)
        {
            var state = (Triplet)savedState;
            var limValues = state.First as string[];
            if (limValues != null && limValues.Length > 1)
            {
                this.MinimumValue = limValues[0];
                this.MaximumValue = limValues[1];
            }

            if (state.Second is bool)
            {
                this.ReadOnly = (bool)state.Second;
            }

            base.LoadViewState(state.Third);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!InDesignMode(this))
            {
                if (this.EnableViewState)
                {
                    // O ViewState["Configured"] é utilizado no lugar de !Page.IsPostBack porque o controle
                    // pode ter sido criado em postback (no caso de controles dentro de TDataGrid's, por exemplo).
                    if (!(bool)this.ViewState["Configured"])
                    {
                        this.TrackViewState();
                        this.ConfigureObject();
                        this.ViewState["Configured"] = true;
                    }
                }
                else
                {
                    this.ConfigureObject();
                }

                // Script de cultura para validação de números e datas
                this.RegisterCultureScript();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            

            // Registra a biblioteca de scripts dos TControls
            RegisterTControlScript(this);

            // Cria atributos usados para validação no cliente
            if (this.RecordContainer is Control && ((Control)this.RecordContainer).ID != null)
            {
                this.Attributes["rc"] = ((Control)this.RecordContainer).ID;
            }

            if (this.FieldName.Length > 0)
            {
                this.Attributes["fieldname"] = this.FieldName;
            }

            if (this.MinimumValue.Length > 0)
            {
                this.Attributes["minimumvalue"] = this.MinimumValue;
            }

            if (this.MaximumValue.Length > 0)
            {
                this.Attributes["maximumvalue"] = this.MaximumValue;
            }

            if (this.RequiredFieldValidation == ValidationOption.True)
            {
                this.Attributes["required"] = "true";
            }

            switch (this.Mode)
            {
                case ControlMode.Edit:
                    if (this.EnableClientScript && this is TTextBox /* && DataType != DbType.Number*/)
                    {
                        this.Attributes["OnChange"] = "if(typeof(TControl_OnChangeValidate) != 'undefined') return TControl_OnChangeValidate('" + this.ClientID + "');";
                    }

                    // O atributo DBValue foi adicionado em TControl.OnPreRender().
                    // Quando o controle está em modo Edit, este atributo não é necessário, o valor é pego do controle diretamente.
                    if (this.ReadOnly)
                    {
                        this.Attributes["id"] = this.ClientID;
                    }
                    else
                    {
                        this.Attributes.Remove("DBValue");
                    }

                    break;

                case ControlMode.View:
                    this.Attributes["id"] = this.ClientID;
                    break;
            }

            // Preenche mensagem de erro de validação
            if (this.Manager == null && this.Page.IsPostBack)
            {
                this.Msg = this.GetValueError();
            }

            
        }

        protected override void PreTControlCtor()
        {
            base.PreTControlCtor();
            this.Mode = ControlMode.Edit; // Deve ser antes de DBValue
        }

        protected override void RenderSpecific(HtmlTextWriter writer)
        {
            if (this.WillRenderEdit)
            {
                this.RenderControlEditMode(writer);
            }
            else
            {
                base.RenderSpecific(writer);
            }
        }

        protected override object SaveViewState()
        {
            var limValues = new[] { this.MinimumValue, this.MaximumValue };
            return new Triplet(
                limValues, 
                this.ReadOnly, 
                base.SaveViewState()
                );
        }

        protected void RegisterCultureScript()
        {
            CultureInfo ci;
            string todayText, dateorder;
            int i;

            if (this.Page != null && !this.Page.ClientScript.IsStartupScriptRegistered(typeof (TControlEditable), "TControl_CultureInfo"))
            {
                ci = System.Threading.Thread.CurrentThread.CurrentCulture;

                if (ci.Name == "pt-BR")
                {
                    return;
                }

                switch (ci.TwoLetterISOLanguageName)
                {
                    case "pt":
                        todayText = "Hoje";
                        break;
                    case "en":
                        todayText = "Today";
                        break;
                    case "es":
                        todayText = "Hoy";
                        break;
                    case "de":
                        todayText = "Heute";
                        break;
                    case "it":
                        todayText = "Oggi";
                        break;
                    case "fr":
                        todayText = "Aujourd'hui";
                        break;
                    default:
                        todayText = "Today";
                        break;
                }

                dateorder = ci.DateTimeFormat.ShortDatePattern.ToLower().Replace(ci.DateTimeFormat.DateSeparator, string.Empty).Replace("yyyy", "y").Replace("yyy", "y").Replace("yy", "y").Replace("dd", "d").Replace("mm", "m");

                var sb = new StringBuilder();
                sb.Append("\n<script language=\"javascript\">\n");
                sb.Append("\tif(typeof(TControl_CultureInfo)=='object')\n");
                sb.Append("\t{\n");
                sb.Append("\t\tTControl_CultureInfo.code=\"" + ci.Name + "\";\n");
                sb.Append("\t\tTControl_CultureInfo.decimalchar=\"" + ci.NumberFormat.NumberDecimalSeparator + "\";\n");
                sb.Append("\t\tTControl_CultureInfo.groupchar=\"" + ci.NumberFormat.NumberGroupSeparator + "\";\n");
                sb.Append("\t\tTControl_CultureInfo.dateorder=\"" + dateorder + "\";\n");
                sb.Append("\t\tTControl_CultureInfo.century=2000;\n");
                sb.Append("\t\tTControl_CultureInfo.cutoffyear=50;\n");
                sb.Append("\t\tTControl_CultureInfo.currencydigits=" + ci.NumberFormat.CurrencyDecimalDigits + ";\n");
                sb.Append("\t\tTControl_CultureInfo.daynames=new Array(\"" + ci.DateTimeFormat.DayNames[0] + "\"");
                for (i = 1; i < ci.DateTimeFormat.DayNames.Length; i++)
                {
                    sb.Append(",\"" + ci.DateTimeFormat.DayNames[i] + "\"");
                }

                sb.Append(");\n");
                sb.Append("\t\tTControl_CultureInfo.abbreviateddaynames=new Array(\"" + ci.DateTimeFormat.AbbreviatedDayNames[0] + "\"");
                for (i = 1; i < ci.DateTimeFormat.AbbreviatedDayNames.Length; i++)
                {
                    sb.Append(",\"" + ci.DateTimeFormat.AbbreviatedDayNames[i] + "\"");
                }

                sb.Append(");\n");
                sb.Append("\t\tTControl_CultureInfo.monthnames=new Array(\"" + ci.DateTimeFormat.MonthNames[0] + "\"");
                for (i = 1; i < ci.DateTimeFormat.MonthNames.Length; i++)
                {
                    sb.Append(",\"" + ci.DateTimeFormat.MonthNames[i] + "\"");
                }

                sb.Append(");\n");
                sb.Append("\t\tTControl_CultureInfo.abbreviatedmonthnames=new Array(\"" + ci.DateTimeFormat.AbbreviatedMonthNames[0] + "\"");
                for (i = 1; i < ci.DateTimeFormat.AbbreviatedMonthNames.Length; i++)
                {
                    sb.Append(",\"" + ci.DateTimeFormat.AbbreviatedMonthNames[i] + "\"");
                }

                sb.Append(");\n");
                sb.Append("\t\tTControl_CultureInfo.todayname=\"" + todayText + "\";\n");
                sb.Append("\t}\n");
                sb.Append("</script>\n");
                this.Page.ClientScript.RegisterStartupScript(typeof (TControlEditable), "TControl_CultureInfo", sb.ToString());
            }
        }
    }

    public class ChangedEventArgs : EventArgs
    {
        private readonly ITControlEditable changedControl;

        public ChangedEventArgs(ITControlEditable changedControl)
        {
            if (changedControl == null)
            {
                throw new ArgumentNullException();
            }

            this.changedControl = changedControl;
        }

        public ChangedEventArgs()
        {
            this.changedControl = null;
        }

        public ITControlEditable ChangedControl
        {
            get
            {
                return this.changedControl;
            }
        }
    }
}