using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI.Design;

namespace Techne.Controls
{
    internal class CheckBoxColumn : TGridColumn
    {
        public CheckBoxColumn()
        {
            this.Checked = false;
            this.CheckOnImageUrl = TCheckBox.CheckOnImageUrl_Def;
            this.CheckOffImageUrl = TCheckBox.CheckOffImageUrl_Def;
        }

        [
            Category("Appearance"), 
            DefaultValue(TCheckBox.CheckOffImageUrl_Def), 
            Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), 
        ]
        public string CheckOffImageUrl
        {
            get
            {
                return (string)this.ViewState["CheckOffImageUrl"];
            }

            set
            {
                this.ViewState["CheckOffImageUrl"] = value == null ? TCheckBox.CheckOffImageUrl_Def : value;
            }
        }

        [
            Category("Appearance"), 
            DefaultValue(TCheckBox.CheckOnImageUrl_Def), 
            Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), 
        ]
        public string CheckOnImageUrl
        {
            get
            {
                return (string)this.ViewState["CheckOnImageUrl"];
            }

            set
            {
                this.ViewState["CheckOnImageUrl"] = value == null ? TCheckBox.CheckOnImageUrl_Def : value;
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
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

        protected override void CopyProperties(ITControl target)
        {
            var chk = target as TCheckBox;
            if (chk == null)
            {
                return;
            }

            base.CopyProperties(target);

            chk.Checked = this.Checked;
            chk.CheckOnImageUrl = this.CheckOnImageUrl;
            chk.CheckOffImageUrl = this.CheckOffImageUrl;
        }

        protected override ITControl GetTControl(string id)
        {
            var chk = new TCheckBox();
            chk.ID = id;
            this.CopyProperties(chk);
            return chk;
        }
    }
}