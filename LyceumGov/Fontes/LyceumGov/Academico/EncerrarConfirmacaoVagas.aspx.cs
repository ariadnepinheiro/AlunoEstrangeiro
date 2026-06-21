using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;


namespace Techne.Lyceum.Net.Academico
{
    [
       NavUrl("~/Academico/EncerrarConfirmacaoVagas.aspx"),
        ControlText("EncerrarConfirmacaoVagas"),
        Title("Encerramento das Agendas - Confirmação de Turnos e Vagas"),
   ]
    public partial class EncerrarConfirmacaoVagas : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdEncerrar, "Encerramento das Agendas - Confirmação de Turnos e Vagas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (!this.Page.IsPostBack)
            {

                ddlAno.Items.Clear();
                ddlAno.DataSource = RN.CtvAgendaConfTurnoVaga.ListarAnos();
                ddlAno.Items.Insert(0, "Selecione");
                ddlAno.DataBind();
            }
        }
        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdEncerrar.Visible = false;
            btnEncerrar.Visible = false;
            btnImprimir.Visible = false;
            ddlPeriodo.Items.Clear();

            if (ddlAno.SelectedValue != "Selecione")
            {
                ddlPeriodo.DataSource = RN.CtvAgendaConfTurnoVaga.ListarPeriodo(int.Parse(this.ddlAno.SelectedValue));
                ddlPeriodo.Items.Insert(0, "Selecione");
                ddlPeriodo.DataBind();
            }

        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPeriodo.SelectedValue != "Selecione")
            {
                grdEncerrar.Visible = true;
                btnEncerrar.Visible = true;
                btnImprimir.Visible = true;
            }

        }

        protected void btnEncerrar_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItems = this.grdEncerrar
                    .GetSelectedFieldValues("ID_AGENDA_CONF_TURNO_VAGA")
                     .Select(x => int.Parse(x.ToString()))
                   .ToList();

                if (selectedItems.Count() > 0)
                {
                    CtvAgendaConfTurnoVaga.EncerrarLancamento(selectedItems, User.Identity.Name);

                    lblMensagem.Text = "Pedido(s) aprovado(s) com sucesso.";

                    odsAgenda.Select();
                    odsAgenda.DataBind();
                    grdEncerrar.DataBind();
                    this.grdEncerrar.Selection.UnselectAll();
                }
                else
                {
                    lblMensagem.Text = "Para encerrar é necessário selecionar pelo menos uma agenda.";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdEncerrar_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdEncerrar);
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            IDictionary<string, string> pares = new Dictionary<string, string>();
            pares.Add("ano", ddlAno.SelectedValue);
            pares.Add("periodo", ddlPeriodo.SelectedValue);
            var script = string.Empty;

            script = @"window.open('../Relatorio/Relatorios.aspx?report=RelLogEncerramentoVagas&grp=gestao&" + CodificaQueryString(pares) + @"','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); ";

          
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
        }
       
    }
}
