using System.IO;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Techne.Controls.Design
{
    internal class GridButtonsDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            var sw = new StringWriter();
            var writer = new HtmlTextWriter(sw);
            var control = (GridButtons)this.Component;
            var empty = true;

            if (control.EnableNewButton)
            {
                new ImageLink { ImageUrl = TUtil.TranslateRelativeUrl(control.ImageUrl_NewButton, control), ToolTip = control.ToolTip_NewButton }.RenderControl(writer);
                empty = false;
            }

            if (control.EnableDeleteButton)
            {
                new ImageButton { ImageUrl = TUtil.TranslateRelativeUrl(control.ImageUrl_DelButton, control), ToolTip = control.ToolTip_DelButton, CommandName = "Remove" }.RenderControl(writer);
                empty = false;
            }

            if (control.EnablePaging)
            {
                var c = control.Page.FindControl(control.Grid);
                var grid = c != null && c is TDataGrid ? (TDataGrid)c : null;

                new ImageButton { ToolTip = ((grid != null) && grid.AllowPaging) ? control.ToolTip_MultPage : control.ToolTip_SinglePage, ImageUrl = ((grid != null) && grid.AllowPaging) ? TUtil.TranslateRelativeUrl(control.ImageUrl_SinglePage, control) : TUtil.TranslateRelativeUrl(control.ImageUrl_MultPage, control) }.RenderControl(writer);

                empty = false;
            }

            if (control.EnableColumnsButton)
            {
                new ImageButton { ImageUrl = TUtil.TranslateRelativeUrl(control.ImageUrl_ColumnsButton, control), ToolTip = control.ToolTip_ColumnsButton }.RenderControl(writer);
                empty = false;
            }

            if (!empty)
            {
                return writer.ToString();
            }

            return "[" + this.ID + "]";
        }
    }
}