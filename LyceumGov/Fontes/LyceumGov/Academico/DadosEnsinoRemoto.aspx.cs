using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Academico
{
    [
         NavUrl("~/Academico/DadosEnsinoRemoto.aspx"),
         ControlText("DadosEnsinoRemoto"),
         Title("Dados Ensino Remoto")
     ]
    public partial class DadosEnsinoRemoto : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}
