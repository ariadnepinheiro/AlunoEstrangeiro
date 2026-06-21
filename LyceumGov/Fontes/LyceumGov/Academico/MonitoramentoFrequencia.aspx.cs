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
         NavUrl("~/Academico/MonitoramentoFrequencia.aspx"),
         ControlText("MonitoramentoFrequencia"),
         Title("Monitoramento da Frequência dos Alunos")
     ]
    public partial class MonitoramentoFrequencia : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    hplMonitoramentoDiretor.Visible = false;
                    hplMonitoramentoRegional.Visible = false;
                    liDiretor.Visible = false;
                    liRegional.Visible = false;


                    var dtPerfil = Perfil.ListarPerfil(User.Identity.Name);

                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("FREQUENCIA ESCOLAR") + "'").Length > 0)
                    {
                        liDiretor.Visible = true;
                        hplMonitoramentoDiretor.Visible = true;
                      
                    }
                    else
                    {
                        liRegional.Visible = true;
                        hplMonitoramentoRegional.Visible = true;  
                        
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
