using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls.Design
{
    internal class TDropDownListDesigner : TControlDesigner
    {
        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            var source = (TDropDownListBase)this.Component;

            var drp = new DropDownList();
            source.CopyProperties(drp);
            drp.Items.Add("[" + source.ID + "]");
            drp.RenderControl(writer);
        }
    }
}