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

namespace Techne.Lyceum.Net.Basico
{
    [
         NavUrl("~/Basico/RelatorioRT.aspx"),
         ControlText("RelatorioRT"),
         Title("Relatório de Trabalho de Infraestrutura")
     ]
    public partial class RelatorioRT : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {               

                    var dtPerfil = Perfil.ListarPerfil(User.Identity.Name);


                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("RT") + "'").Length > 0)
                    {
                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiNGMwZWQ1ODEtYjU1OS00YTQ5LTg3ZmItNWEyNzZhODA1YmQ1IiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
                        return;
                    }


                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("RT - ESCOLA") + "'").Length > 0)
                    {
                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiMjg3MTY2MGUtODNjNC00MTQxLWFkZWYtZjJhMjRkYjM3M2MzIiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
                       
                    }

                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("RT - REGIONAL") + "'").Length > 0)//REGIONAL
                    {
                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiNDA1ZmNjMjEtY2U5OS00ODBkLTk3NmQtMWE5MmE2Y2RmNjA0IiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
                        
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
