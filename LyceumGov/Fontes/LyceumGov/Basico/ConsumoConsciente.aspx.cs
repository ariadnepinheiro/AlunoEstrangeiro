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
         NavUrl("~/Basico/ConsumoConsciente.aspx"),
         ControlText("ConsumoConsciente"),
         Title("Consumo Consciente")
     ]
    public partial class ConsumoConsciente : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {               

                    var dtPerfil = Perfil.ListarPerfil(User.Identity.Name);


                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("CONSUMO CONSCIENTE") + "'").Length > 0)
                    {
                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiOGRkMDU5YWEtMWRiZS00ODVlLWE1YjAtZTY1MWUwYWVlZmQ5IiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
                        return;
                    }


                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("CONSUMO CONSCIENTE - ESCOLA") + "'").Length > 0)
                    {
                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiNGIzZDM5N2MtOGQ3Zi00ZDVhLTgwZGQtNDkxZTkxNzliMWQyIiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
                       
                    }

                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("CONSUMO CONSCIENTE - REGIONAL") + "'").Length > 0)//REGIONAL
                    {
                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiNTg3MTg5ZGMtOTI5Yi00YjIxLTgwYTMtOTYwNzg2MmI3MDhiIiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
                        
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
