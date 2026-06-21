namespace Techne.Lyceum.Net
{
    using System;
    using System.ComponentModel;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using DevExpress.Web.ASPxClasses;
    using DevExpress.Web.ASPxGridView;

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

        public static void TituloGrid(ASPxGridView grid, string titulo)
        {
            var tituloGrade = grid.SettingsText.Title;

            if (tituloGrade != string.Empty)
            {
                grid.SettingsText.Title = tituloGrade.Replace("|Tabela:|", titulo);
            }
        }

        /// <summary>
        /// Método que recebe o controle e restringe o acesso a ele caso seja necessário
        /// </summary>
        /// <param name="control">Controle a ser manipulado</param>
        public void ControlaAcesso(Control control)
        {
            if (control is ASPxGridView)
            {
                var gv = (ASPxGridView)control;

                foreach (GridViewColumn col in gv.Columns)
                {
                    if (col is GridViewCommandColumn)
                    {
                        if (((GridViewCommandColumn)col).EditButton.Visible)
                        {
                            ((GridViewCommandColumn)col).EditButton.Visible = this.Permission.AllowUpdate;
                        }

                        if (((GridViewCommandColumn)col).DeleteButton.Visible)
                        {
                            ((GridViewCommandColumn)col).DeleteButton.Visible = this.Permission.AllowDelete;
                        }

                        if (((GridViewCommandColumn)col).NewButton.Visible)
                        {
                            ((GridViewCommandColumn)col).NewButton.Visible = this.Permission.AllowInsert;
                        }

                        break;
                    }

                    // Isto corrige a posição dos botões na grid para o Firefox.
                    col.CellStyle.Wrap = DefaultBoolean.False;
                }

                if (gv != null)
                {
                    var img = (HtmlImage)gv.FindHeaderTemplateControl(gv.Columns[string.Empty], "btnNovoGrid");

                    if (img != null)
                    {
                        img.Visible = this.Permission.AllowInsert;
                    }
                }
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
                    "<script language=\"JavaScript\" src=\"" + Techne.TUtil.TranslateRelativeUrl("~/Scripts/TPageLyceum.js", this) + "\"></script>\r\n");
            }

            RegisterDisabledNavigationKeys(this, this.DisabledNavigationKeys);
        }
    }
}