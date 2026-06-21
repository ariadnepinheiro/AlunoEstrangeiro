using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal class TextBoxColumn : TGridColumn
    {
        public TextBoxColumn(string columnName) : base(columnName)
        {
            this.Rows = TTextBox.Rows_Def;
            this.TextMode = TTextBox.TextMode_Def;
            this.Wrap = TTextBox.Wrap_Def;
        }

        public TextBoxColumn() : this(string.Empty)
        {
        }

        [
            Category("Behavior"), 
            DefaultValue(TTextBox.Rows_Def)
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
            Category("Behavior"), 
            DefaultValue(TTextBox.TextMode_Def)
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
            DefaultValue(TTextBox.Wrap_Def), 
            Category("Layout")
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

        protected override bool ExpandInEditMode
        {
            get
            {
                return true;
            }
        }

        protected override void CopyProperties(ITControl target)
        {
            var txt = target as TTextBox;
            if (txt == null)
            {
                return;
            }

            base.CopyProperties(target);

            txt.Rows = this.Rows;
            txt.TextMode = this.TextMode;
            txt.Wrap = this.Wrap;
        }

        protected override ITControl GetTControl(string id)
        {
            var txt = new TTextBox();
            txt.ID = id;
            this.CopyProperties(txt);
            return txt;
        }
    }
}