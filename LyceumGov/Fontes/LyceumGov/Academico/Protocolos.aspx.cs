//using System;
//using System.Collections.Generic;
namespace Techne.Lyceum.Net.Academico
{
    using System;
    using Techne.Web;

    [NavUrl("~/Academico/Protocolos.aspx"), ControlText("Protocolos"), Title("Protocolos")]
    public partial class Protocolos : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdProtocolos, "Protocolos");

            odsProtocolos.SelectParameters["matricula"].DefaultValue = User.Identity.Name;
        }
    }
}