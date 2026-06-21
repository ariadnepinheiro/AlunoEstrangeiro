namespace Techne.Lyceum.Net.Manutencao
{
    using System;
    using System.Web.Security;

    public partial class Manutencao : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Keys.Count > 0)
            {
                var page = Request.QueryString["aspxerrorpath"];

                if (page == "/Index.aspx")
                {
                    FormsAuthentication.SignOut();
                }
            }
        }
    }
}