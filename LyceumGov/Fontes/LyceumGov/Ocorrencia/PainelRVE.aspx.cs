using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Controls;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Text;
using DevExpress.Web.ASPxTabControl;
using System.Data;
using Techne.Lyceum.RN;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;
using Techne.Lyceum.RN.DTOs;
using DevExpress.Web.Data;

namespace Techne.Lyceum.Net.Ocorrencia
{
    [
        NavUrl("~/Ocorrencia/PainelRVE.aspx"),
        ControlText("Painel RVE"),
        Title("Painel RVE")
    ]
    public partial class PainelRVE : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                //if (!IsPostBack)
                //{
                //    var dtPerfil = Perfil.ListarPerfil(User.Identity.Name);

                //    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("RVE - SEDE") + "'").Length > 0)
                //    {
                //        frResultado.Attributes["src"] = "https://app.powerbi.com/reportEmbed?reportId=1d9418c6-8de8-4c63-bd82-126cc0406008&autoAuth=true&ctid=0c2829c9-41fa-4885-b057-a327fa5f37d4";
                //        return;
                //    }
                //    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("RVE - REGIONAL") + "'").Length > 0)
                //    {
                //        frResultado.Attributes["src"] = "https://app.powerbi.com/reportEmbed?reportId=275a550c-2f79-4019-b77e-4cbfe711ebd4&autoAuth=true&ctid=0c2829c9-41fa-4885-b057-a327fa5f37d4";
                //    }
                //}
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
