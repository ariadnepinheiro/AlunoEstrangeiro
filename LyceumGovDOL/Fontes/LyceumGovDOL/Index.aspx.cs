using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System.Data;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Seeduc.Infra.Helpers;
using DevExpress.Web.Data;
using Techne.Lyceum.RN.ContratoTemporario;
using Techne.Lyceum.RN.Entidades;
using System.Linq;
using System.Configuration;

namespace Techne.Lyceum.Net
{
    [NavUrl("~/Menu/Home.aspx"),
    ControlText("Home"),
    Title("Home"),]

    public partial class Index : TPage
    {
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.hlHelp.Attributes.Add("OnClick", "__Help(); return(false);");
            //this.hlHelp.Style.Add("cursor", "pointer");

            var versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var sufixo = ConfigurationManager.AppSettings["VersaoSufixo"] ?? string.Empty;
            var saudacao = string.Format(
                "{0} (IP: {1})",
                System.Threading.Thread.CurrentPrincipal.Identity.Name,
                Sistema.ObterIP());

            lblSaudacoes.Text = saudacao;
            lblVersao.Text = versao + sufixo;

        }
    }
}
