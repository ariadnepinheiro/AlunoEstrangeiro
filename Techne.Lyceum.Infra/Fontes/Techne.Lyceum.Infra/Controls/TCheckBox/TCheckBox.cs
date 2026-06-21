using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    [Designer("Techne.Controls.Design.TCheckBoxDesigner, Techne")]
    internal class TCheckBox : TControlEditable, IPostBackDataHandler
    {
        public const string CheckOffImageUrl_Def = "~/images/CheckOff.gif";

        public const string CheckOnImageUrl_Def = "~/images/CheckOn.gif";

        public TCheckBox()
        {
            this.Checked = false;
            this.CheckOnImageUrl = CheckOnImageUrl_Def;
            this.CheckOffImageUrl = CheckOffImageUrl_Def;
        }

        [
            Description("Valor do controle tipado conforme a propriedade DataType."), 
            Browsable(false)
        ]
        public override DbObject DBValue
        {
            get
            {
                if (this.DataType == DbType.VarChar)
                {
                    return this.Checked ? "S" : "N";
                }
                else if (this.DataType == DbType.Number)
                {
                    return this.Checked ? -1 : 0;
                }
                else
                {
                    throw new NotImplementedException("Tipo não suportado pelo controle.");
                }
            }

            set
            {
                base.DBValue = value;

                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                if (value.IsNull || value.Type == DbType.VarChar && (string)value == string.Empty)
                {
                    // TODO (22/02/02) Tratar caso de checkbox tristate
                    this.Checked = false;
                }
                else if (value.Type == DbType.VarChar)
                {
                    this.Checked = ((string)value).ToUpper() == "S";
                }
                else if (value.Type == DbType.Number)
                {
                    this.Checked = (decimal)value != 0;
                }
                else
                {
                    throw new NotImplementedException("Tipo não suportado pelo controle.");
                }
            }
        }

        [
            DefaultValue(CheckOffImageUrl_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
            Category("Appearance")
        ]
        public string CheckOffImageUrl
        {
            get
            {
                return (string)this.ViewState["CheckOffImageUrl"];
            }

            set
            {
                this.ViewState["CheckOffImageUrl"] = value == null ? CheckOffImageUrl_Def : value;
            }
        }

        [
            DefaultValue(CheckOnImageUrl_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
            Category("Appearance")
        ]
        public string CheckOnImageUrl
        {
            get
            {
                return (string)this.ViewState["CheckOnImageUrl"];
            }

            set
            {
                this.ViewState["CheckOnImageUrl"] = value == null ? CheckOnImageUrl_Def : value;
            }
        }

        [
            Browsable(false)
        ]
        public bool Checked
        {
            get
            {
                return (bool)this.ViewState["Checked"];
            }

            set
            {
                this.ViewState["Checked"] = value;
            }
        }

        protected override string EntryValue
        {
            get
            {
                return this.Checked ? "S" : "N";
            }
        }

        private bool AutoPostBack
        {
            get
            {
                return this.Dependers.Length > 0 && !TControl.InDesignMode(this);
            }
        }

        public override void CopyProperties(WebControl target)
        {
            // Copia propriedades gerais
            base.CopyProperties(target);

            if (target is CheckBox)
            {
                // Copia propriedades específicas do controle CheckBox
                ((CheckBox)target).AutoPostBack = this.AutoPostBack;
                ((CheckBox)target).Checked = this.Checked;
            }
        }

        public override string GetValueError()
        {
            // Este controle não é passível de erro.
            return string.Empty;
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
            {
                return;
            }

            var state = (Pair)savedState;

            base.LoadViewState(state.First);
            this.Checked = (bool)state.Second;
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);

            if (this.Page != null && !this.ReadOnly && this.Mode == ControlMode.Edit)
            {
                this.Page.RegisterRequiresPostBack(this);
            }
        }

        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            var chk = new CheckBox();
            this.CopyProperties(chk);
            chk.ID = this.UniqueID;

            if (this.AutoPostBack)
            {
                // Esta atribuição é necessária para que sejam gerados atributos para o
                // AutoPostBack. Se não for informado, erro de runtime ocorrerá em
                // CheckBox.RenderInputTag. Provavelmente é um bug do CheckBox, que não
                // verifica se Page == null antes de chamar Page.GetPostBackClientEvent().
                chk.Page = this.Page;
            }

            chk.RenderControl(writer);
        }

        protected override void RenderControlViewMode(HtmlTextWriter writer)
        {
            var lbl = new Label();
            this.CopyProperties(lbl);
            lbl.ID = this.UniqueID;
            var img = new Image();
            img.ImageUrl = this.Checked ? TUtil.TranslateRelativeUrl(this.CheckOnImageUrl, this) : TUtil.TranslateRelativeUrl(this.CheckOffImageUrl, this);
            lbl.Controls.Add(img);
            lbl.RenderControl(writer);
        }

        protected override object SaveViewState()
        {
            return new Pair(base.SaveViewState(), this.Checked);
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, 
                                               NameValueCollection postCollection)
        {
            var oldValue = this.Checked;
            var newValue = postCollection[postDataKey] == "on";

            if (newValue == oldValue)
            {
                return false;
            }
            else
            {
                this.Checked = newValue;
                return true;
            }
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            this.OnChanged(new ChangedEventArgs(this));
        }
    }
}