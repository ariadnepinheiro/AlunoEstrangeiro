namespace Techne.Lyceum.Net
{
    using System;
    using System.ComponentModel;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web;
    using Techne.Auditing;

    [Flags]
    public enum NavigationKey
    {
        Enter = 1,

        Backspace = 2
    }

    public class TPage : Techne.TPage
    {
        private const string ConnectionDef = "Lyceum";

        private NavigationKey disabledNavigationKeys = NavigationKey.Enter | NavigationKey.Backspace;

        public TPage()
        {
            this.Connection = ConnectionDef;
            this.SwitchPostback = false;
        }

        public enum AcaoControle
        {
            excluir,

            novo,

            editar
        }

        public override string ApplicationName
        {
            get
            {
                return "LyceumNet";
            }
        }

        [DefaultValue(ConnectionDef)]
        public override sealed string Connection
        {
            get
            {
                return base.Connection;
            }

            set
            {
                base.Connection = value;
            }
        }

        [DefaultValue(NavigationKey.Enter | NavigationKey.Backspace)]
        public NavigationKey DisabledNavigationKeys
        {
            get
            {
                return this.disabledNavigationKeys;
            }

            set
            {
                this.disabledNavigationKeys = value;
            }
        }

        public static void RegisterDisabledNavigationKeys(Page page, NavigationKey disabledKeys)
        {
            var strScript = new StringBuilder();

            if (!page.ClientScript.IsStartupScriptRegistered(typeof(Page), "RegisterDisabledNavigationKeys"))
            {
                strScript.AppendLine("<script language=\"JavaScript\">");
                strScript.AppendLine("if(typeof disableNavigationKeys == 'function') {");
                strScript.AppendLine(" disableNavigationKeys(" + ((int)disabledKeys).ToString() + ");");
                strScript.AppendLine("}");
                strScript.AppendLine("</script>");
                page.ClientScript.RegisterStartupScript(typeof(Page), "RegisterDisabledNavigationKeys", strScript.ToString());
            }
        }

        public void ControlaAcesso(Control control, AcaoControle ac)
        {
            if (control is ImageButton)
            {
                if (ac == AcaoControle.excluir)
                {
                    if (control.Visible)
                    {
                        control.Visible = this.Permission.AllowDelete;
                    }
                }
                else if (ac == AcaoControle.novo)
                {
                    if (control.Visible)
                    {
                        control.Visible = this.Permission.AllowInsert;
                    }
                }
                else if (ac == AcaoControle.editar)
                {
                    if (control.Visible)
                    {
                        control.Visible = this.Permission.AllowUpdate;
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.ClientScript.IsClientScriptBlockRegistered(typeof(TPage), "TPageLyceum"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(
                    typeof(TPage), 
                    "TPageLyceum",
                    "<script language=\"JavaScript\" src=\"" + TUtil.TranslateRelativeUrl("~/Scripts/TPageLyceum.js", this) + "\"></script>\r\n");
            }

            RegisterDisabledNavigationKeys(this, this.DisabledNavigationKeys);
        }

        public void AuditaPagina(string pagina)
        {
            var current = HttpContext.Current;
            current.Items["__AuditingInfo"] = new AuditingInfo(this.User.Identity.Name, pagina, true);
        }
    }
}