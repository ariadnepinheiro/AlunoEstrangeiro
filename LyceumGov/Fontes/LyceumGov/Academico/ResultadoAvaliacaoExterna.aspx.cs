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
        NavUrl("~/Academico/ResultadoAvaliacaoExterna.aspx"),
        ControlText("Resultado Avaliação Externa"),
        Title("Resultado Avaliação Externa")
    ]
    public partial class ResultadoAvaliacaoExterna : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    var dtPerfil = Perfil.ListarPerfil(User.Identity.Name);

                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("RESULTADO AVALIAÇÃO") + "'").Length > 0)
                    {
                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiZWQ1MzE1MGItMGRmNS00Y2Y3LWEwOWItZTgyYjAyZGM4NWZmIiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
                        return;
                    }

                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("RESULTADO AVALIAÇÃO - PARCIAL") + "'").Length > 0)
                    {

                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiYmQzYWIyNzktMzg2YS00N2NlLTkxZmQtNzUyOWYwYWE3NTExIiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9&pageName=ReportSectioneaa4f2c36058be6e5c45";
                    }


                    if (dtPerfil.Select("perfil ='" + RN.RNBase.MudarAspas("RESULTADO AVALIAÇÃO - REGIONAL") + "'").Length > 0)
                    {
                        frResultado.Attributes["src"] = "https://app.powerbi.com/view?r=eyJrIjoiNTdlYTM0ZDEtYjAwYy00NDYzLTk0MzMtNDFiOGQ2MzE5YmUyIiwidCI6IjBjMjgyOWM5LTQxZmEtNDg4NS1iMDU3LWEzMjdmYTVmMzdkNCJ9";
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
