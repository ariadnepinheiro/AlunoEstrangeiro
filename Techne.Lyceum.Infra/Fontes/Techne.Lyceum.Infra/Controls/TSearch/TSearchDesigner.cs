using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls.Design
{
    internal class TSearchBaseDesigner : TTextBoxDesigner
    {
        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            base.RenderControlEditMode(writer);

            var source = (TSearchBase)this.Component;

            var txtArg = new TextBox();
            source.CopyArgumentProperties(txtArg);
            txtArg.RenderControl(writer);

            if (source.Enabled && !source.ReadOnly)
            {
                var img = new ImageButton();
                img.ImageUrl = TUtil.TranslateRelativeUrl(source.SearchButtonImageUrl, source);
                img.RenderControl(writer);
            }
        }
    }
}