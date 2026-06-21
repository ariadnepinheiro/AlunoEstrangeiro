using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls.Design
{
    internal class TCheckBoxDesigner : TControlDesigner
    {
        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            var source = (TCheckBox)this.Component;

            var chk = new CheckBox();
            source.CopyProperties(chk);

            // Esta atribuição é necessária para que não Page.GetPostBackClientEvent
            // não seja chamado em design time (situação em que Page=null).
            // Se não for feito, erro de runtime ocorrerá em CheckBox.RenderInputTag.
            // Provavelmente é um bug do CheckBox, que não verifica se Page == null
            // antes de chamar Page.GetPostBackClientEvent().
            // Ainda bem que a propriedade AutoPostBack é indiferente em design-time!! :)
            chk.AutoPostBack = false;

            chk.Text = "[" + source.ID + "]";
            chk.RenderControl(writer);
        }

        protected override void RenderControlViewMode(HtmlTextWriter writer)
        {
            var chk = (TCheckBox)this.Component;

            var img = new Image();
            img.ImageUrl = TUtil.TranslateRelativeUrl(chk.Checked ? chk.CheckOnImageUrl : chk.CheckOffImageUrl, chk);
            img.RenderControl(writer);
        }
    }
}