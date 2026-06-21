using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal abstract class TLinkButtonBase : TLinkBase, IPostBackEventHandler
    {
        protected virtual void OnClick(EventArgs args)
        {
        }

        protected virtual void RaisePostBackEvent(string eventArgument)
        {
            this.OnClick(EventArgs.Empty);
        }

        protected override WebControl CreateInternalControl()
        {
            if (!this.Enabled)
            {
                return base.CreateInternalControl();
            }

            return new LinkButton();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (InDesignMode(this))
            {
                this.InternalControl.Attributes["href"] = "#";
            }
            else if (this.Enabled && this.Page != null)
            {
                this.InternalControl.Attributes["href"] = this.Page.ClientScript.GetPostBackClientHyperlink(this, string.Empty);
            }
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.RaisePostBackEvent(eventArgument);
        }
    }
}