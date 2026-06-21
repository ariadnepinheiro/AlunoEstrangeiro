using System.Web.UI;

namespace Techne.Controls.Design
{
    internal class TDateBoxDesigner : TTextBoxDesigner
    {
        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            // Renderiza o TextBox.
            base.RenderControlEditMode(writer);

            // Renderiza o botão.
            TDateBox.RenderDateBoxButton((TDateBox)this.Component, writer);
        }
    }
}