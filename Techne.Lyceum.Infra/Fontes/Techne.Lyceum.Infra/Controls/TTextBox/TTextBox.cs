using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    [Designer("Techne.Controls.Design.TTextBoxDesigner")]
    [ValidationPropertyAttribute("DBValue")]
    [ControlValueProperty("DBValue")]
    public class TTextBox : TControlEditable, IPostBackDataHandler
    {
        // Em ordem alfabética!!
        public const int Columns_Def = 20;

        public const int Rows_Def = 0;

        public const TextBoxMode TextMode_Def = TextBoxMode.SingleLine;

        public const bool Wrap_Def = true;

        private string entryValue;

        private string formatDecimalEdit = StrLib.defaultFormatDecimal;

        public TTextBox()
        {
            this.Columns = Columns_Def;

            

            this.SetEntryValue(string.Empty);

            

            this.FormatDateEdit = string.Empty;
            this.MaxLength = 0;
            this.MinLength = 0;
            this.Precision = -1;
            this.Rows = Rows_Def;
            this.Scale = -1;
            this.TextMode = TextMode_Def;
            this.Wrap = Wrap_Def;
        }

        [
            Category("Appearance"), 
            DefaultValue(Columns_Def), 
            Description("Indica a largura do controle em número de caracteres. Só tem efeito se a propriedade Width não estiver setada.")
        ]
        public virtual int Columns
        {
            get
            {
                return (int)this.ViewState["Columns"];
            }

            set
            {
                this.ViewState["Columns"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Quando DateType=DateTime, o formato do modo de edição."), 
            TypeConverter(typeof (DateTimeFormatCharConverter))
        ]
        public virtual string FormatDateEdit
        {
            get
            {
                return (string)this.ViewState["FormatDateEdit"];
            }

            set
            {
                this.ViewState["FormatDateEdit"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Description("Valor do controle tipado conforme a propriedade DataType."), 
            Browsable(false)
        ]
        public override DbObject DBValue
        {
            get
            {
                return base.DBValue;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                if (value.IsNull)
                {
                    this.SetEntryValue(string.Empty);
                }
                else if (value.Type == DbType.VarChar)
                {
                    this.SetEntryValue((string)value);
                }
                else if (value.Type == DbType.Number)
                {
                    this.SetEntryValue(StrLib.ToStr(value, this.formatDecimalEdit, Thread.CurrentThread.CurrentCulture.Name));
                }
                else if (value.Type == DbType.Date)
                {
                    this.SetEntryValue(StrLib.ToStr(value, this.FormatDateEdit.Length == 0 ? "d" : this.FormatDateEdit, Thread.CurrentThread.CurrentCulture.Name));
                }
                else
                {
                    throw new NotImplementedException();
                }

                // Se o valor contido no text box apresentado é válido, guarda o valor tipado em DBValue,
                // caso contrário guarda DBNull (DBValue devolve DBNull quando o valor não é válido).
                var validValue = true;
                try
                {
                    this.TypeString(this.EntryValue);
                }
                catch (FormatException)
                {
                    validValue = false;
                }
                catch (OverflowException)
                {
                    validValue = false;
                }

                base.DBValue = validValue ? value : DBNull.Value;
            }
        }

        [
            Category("Techne - Validação"), 
            Description("Número máximo de caracteres aceitos pelo campo. O valor 0 indica que não há tal limitação."), 
            DefaultValue(0)
        ]
        public int MaxLength
        {
            get
            {
                return (int)this.ViewState["MaxLength"];
            }

            set
            {
                this.ViewState["MaxLength"] = value;
            }
        }

        [
            Category("Techne - Validação"), 
            Description("Número mínimo de caracteres aceitos pelo campo."), 
            DefaultValue(0)
        ]
        public int MinLength
        {
            get
            {
                return (int)this.ViewState["MinLength"];
            }

            set
            {
                this.ViewState["MinLength"] = value < 0 ? 0 : value;
            }
        }

        [
            Category("Techne - Validação"), 
            DefaultValue(-1), 
        ]
        public int Precision
        {
            get
            {
                return (int)this.ViewState["Precision"];
            }

            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("A propriedade Precision não pode ser 0.");
                }

                this.ViewState["Precision"] = value < 0 ? -1 : value;
            }
        }

        [
            Category("Behavior"), 
            DefaultValue(Rows_Def)
        ]
        public int Rows
        {
            get
            {
                return (int)this.ViewState["Rows"];
            }

            set
            {
                this.ViewState["Rows"] = value;
            }
        }

        [
            Category("Techne - Validação"), 
            DefaultValue(-1), 
        ]
        public int Scale
        {
            get
            {
                return (int)this.ViewState["Scale"];
            }

            set
            {
                this.ViewState["Scale"] = value < 0 ? -1 : value;
            }
        }

        [
            Category("Behavior"), 
            DefaultValue(TextMode_Def)
        ]
        public TextBoxMode TextMode
        {
            get
            {
                return (TextBoxMode)this.ViewState["TextMode"];
            }

            set
            {
                this.ViewState["TextMode"] = value;
            }
        }

        [
            Category("Layout"), 
            DefaultValue(Wrap_Def)
        ]
        public bool Wrap
        {
            get
            {
                return (bool)this.ViewState["Wrap"];
            }

            set
            {
                this.ViewState["Wrap"] = value;
            }
        }

        /// <summary>
        ///   Formato de apresentação de valores decimal em modo Edit.
        ///   Criado para ser alterado pelo controle derivado TCurrency.
        /// </summary>
        protected virtual string FormatDecimalEdit
        {
            get
            {
                return this.formatDecimalEdit;
            }

            set
            {
                this.formatDecimalEdit = value == null ? StrLib.defaultFormatDecimal : value;
            }
        }

        protected override string EntryValue
        {
            get
            {
                return this.entryValue;
            }
        }

        public override void CopyProperties(WebControl target)
        {
            // Copia propriedades gerais
            base.CopyProperties(target);

            if (target is TextBox)
            {
                var txt = (TextBox)target;

                // Copia propriedades específicas do controle TextBox implementadas por este controle
                txt.Columns = this.Columns;
                txt.MaxLength = this.MaxLength;
                txt.Rows = this.Rows;
                txt.TextMode = this.TextMode;
                txt.Wrap = this.Wrap;

                txt.Text = this.EntryValue;
            }
        }

        protected virtual bool LoadPostData(string postDataKey, 
                                            NameValueCollection postCollection)
        {
            try
            {
                var strNewValue = postCollection[postDataKey];
                if (strNewValue == null)
                {
                    return false;
                }

                DbObject oldValue = null;
                var validOld = true;
                try
                {
                    oldValue = this.TypeString(this.EntryValue);
                }
                catch (FormatException)
                {
                    validOld = false;
                }

                DbObject newValue = null;
                var validNew = true;
                try
                {
                    newValue = this.TypeString(strNewValue);
                }
                catch (FormatException)
                {
                    validNew = false;
                }
                catch (ArgumentOutOfRangeException)
                {
                    validNew = false;
                }
                catch (OverflowException)
                {
                    validNew = false;
                }

                var changed = true;
                if (validOld && validNew)
                {
                    if (oldValue.Equals(newValue))
                    {
                        changed = false;
                    }
                    else
                    {
                        this.DBValue = newValue;
                    }
                }
                else if (validNew)
                {
                    this.DBValue = newValue;
                }
                else if (validOld)
                {
                    // EntryValue tem que ser setado depois de DBValue pq DBValue seta EntryValue
                    this.DBValue = DBNull.Value;
                    this.SetEntryValue(strNewValue);
                }
                else
                    
// Comparação de strings somente
                    if (this.EntryValue.Equals(strNewValue))
                    {
                        changed = false;
                    }
                    else
                    {
                        this.SetEntryValue(strNewValue);
                    }

                return changed;
            }
            finally
            {
            }
        }

        protected virtual void RaisePostDataChangedEvent()
        {
            this.OnChanged(new ChangedEventArgs(this));
        }

        protected override void LoadViewState(object savedState)
        {
            try
            {
                var state = (object[])savedState;

                if (state == null)
                {
                    return;
                }

                // Tem que chamar base.LoadViewState() antes porque ele seta DBValue.
                // Se EntryValue for setado antes de DBValue, ele é sobreescrito qdo DBValue for setado.
                base.LoadViewState(state[3]);
                this.SetEntryValue((string)state[0]);
                this.Precision = (int)state[1];
                this.Scale = (int)state[2];
            }
            finally
            {
            }
        }

        protected override void OnChanged(ChangedEventArgs args)
        {
            base.OnChanged(args);
            if (this.Manager == null)
            {
                this.Msg = this.GetValueError();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.WillRenderEdit && this.DataType == DbType.Number && this.Page is TPage)
            {
                this.TPage.NumericFields.Add(this);
            }
        }

        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            this.RenderControlEditMode(writer, false);
        }

        protected override object SaveViewState()
        {
            var list = new ArrayList();
            list.Add(this.EntryValue);
            list.Add(this.Precision);
            list.Add(this.Scale);
            list.Add(base.SaveViewState());
            return list.ToArray();
        }

        protected void RenderControlEditMode(HtmlTextWriter writer, bool readOnly)
        {
            var txt = new TextBox();
            txt.ID = this.UniqueID;

            if (this.DataType == DbType.Date)
            {
                // Passa o formato da data quando em modo de edição
                // Este formato é usado pelos scripts do browser para validar 
                // o texto digitado pelo usuário
                if (this.FormatDateEdit.Length > 0 && this.FormatDateEdit != "d")
                {
                    this.Attributes.Add("dateformat", this.FormatDateEdit);
                }
            }
            else
            {
                if (this.DataType == DbType.Number)
                {
                    if (this.Precision >= 0)
                    {
                        this.Attributes.Add("Prec", this.Precision.ToString(CultureInfo.InvariantCulture));
                    }

                    if (this.Scale >= 0)
                    {
                        this.Attributes.Add("Scal", this.Scale.ToString(CultureInfo.InvariantCulture));
                    }
                }

                if (this.MinLength > 0)
                {
                    this.Attributes.Add("MinLen", this.MinLength.ToString(CultureInfo.InvariantCulture));
                }

                // Não é necessário adicionar o atributo MaxLength,
                // já que o controle renderizado (TextBox txt) já o contém.
            }

            this.CopyProperties(txt);
            if (readOnly)
            {
                txt.ReadOnly = true;
            }

            txt.RenderControl(writer);
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, 
                                               NameValueCollection postCollection)
        {
            return this.LoadPostData(postDataKey, postCollection);
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            this.RaisePostDataChangedEvent();
        }

        private void SetEntryValue(string text)
        {
            this.entryValue = text == null ? string.Empty : text;
        }
    }
}