using System;
using System.ComponentModel;

namespace Techne.Controls
{
    /// <summary>
    ///   Summary description for ReturnButton.
    /// </summary>
    internal class ReturnButton : ImageLink
    {
        private const string ImageUrl_Def = "~/Images/Return.gif";

        public ReturnButton()
        {
            this.Click += this.ButtonClick;
            this.PreRender += this.Desenha;
            this.ImageUrl = ImageUrl_Def;
            this.AccessKey = "V";
        }

        [DefaultValue("V")]
        public override string AccessKey
        {
            get
            {
                return base.AccessKey;
            }

            set
            {
                base.AccessKey = value;
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            if (this.Page.Request.Params.Get("ReturnUrl") != null)
            {
                this.Page.Response.Redirect(this.Page.Request["ReturnUrl"]);
            }
        }

        private void Desenha(object sender, EventArgs e)
        {
            if (this.Page.Request.Params.Get("ReturnUrl") == null)
            {
                this.Visible = false;
            }
        }
    }
}