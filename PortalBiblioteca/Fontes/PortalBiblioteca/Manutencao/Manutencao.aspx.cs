using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Techne.Lyceum.Net.Manutencao
{
    public partial class Manutencao : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Keys.Count > 0)
            {
                string page = Request.QueryString["aspxerrorpath"];
                if (page == "/Inicial.aspx")
                {
                    FormsAuthentication.SignOut();
                }
                
            }
        }
    }
}
