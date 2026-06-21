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

namespace Techne.Lyceum.Net.Academico
{
    [
        NavUrl("~/Academico/MonitoramentoIF.aspx"),
        ControlText("Monitoramento do IF"),
        Title("Monitoramento do IF")
    ]
    public partial class MonitoramentoIF : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    var dtPerfil = Perfil.ListarPerfil(User.Identity.Name);

                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("MONITORAMENTO DO IF - ESCOLA") + "'").Length > 0)
                    {
                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiYTNjMDdhZWItMTk4Zi00ODI4LWJlNjAtNjZjZTBhNTBjZjVlIiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
                        return;
                    }

                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("MONITORAMENTO DO IF - REGIONAL") + "'").Length > 0)
                    {

                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiM2VlY2M3ZGUtZDg2Zi00MDM2LWI2MTMtZmU1MmZmNzg2NGM2IiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
                    }


                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("MONITORAMENTO DO IF - SEDE") + "'").Length > 0)
                    {
                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiZWExMTJiNDEtNjNlZC00NDY4LWIwNzItMjdkOGJhOGYxZDI3IiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
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
