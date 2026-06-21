using System;
using System.Configuration;

namespace Techne.Lyceum.Net.Modulos
{
    public partial class PublicMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hlHelp.Attributes.Add("OnClick", "__Help(); return(false);");
            hlHelp.Style.Add("cursor", "pointer");
            hlSair.Attributes.Add("OnClick", "self.close(); return(false);");
            hlSair.Style.Add("cursor", "pointer");

            var versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var sufixo = ConfigurationManager.AppSettings["VersaoSufixo"] ?? string.Empty;

            lblVersao.Text = versao + sufixo;

            lnkFacebook.Attributes["href"] = ConfigurationManager.AppSettings["linkfacebook"];
            lnkInstagram.Attributes["href"] = ConfigurationManager.AppSettings["linkinstagram"];
            lnkYoutube.Attributes["href"] = ConfigurationManager.AppSettings["linkyoutube"];
            lnkSite.Attributes["href"] = ConfigurationManager.AppSettings["linksite"];
        }
    }
}