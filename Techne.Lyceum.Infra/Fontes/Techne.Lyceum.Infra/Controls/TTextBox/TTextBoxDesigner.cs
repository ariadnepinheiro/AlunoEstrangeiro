using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls.Design
{
    public abstract class TTextBoxDesigner : TControlDesigner
    {
        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            var source = (TTextBox)this.Component;

            var txt = new TextBox();
            source.CopyProperties(txt);
            txt.Text = "[" + source.ID + "]";
            txt.RenderControl(writer);
        }
    }
}