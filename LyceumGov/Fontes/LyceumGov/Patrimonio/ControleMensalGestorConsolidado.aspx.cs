using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using System.Data;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using Techne.Controls;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/ControleMensalGestorConsolidado.aspx"), ControlText("Controle Mensal do Gestor  - Anexo II - Regional"), Title("Controle Mensal do Gestor - Anexo II - Regional")]
    public partial class ControleMensalGestorConsolidado : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    divPrincipal.Visible = false;
                    pnlImprimir.Visible = false;
                    CarregaAno();
                    CarregarMes();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
      
        private void CarregaAno()
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            ddlAno.Items.Clear();
            ddlAno.DataSource = rnPeriodoLetivo.ListaAnos(false);
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregarMes()
        {
            ddlMes.Items.Clear();
            ddlMes.DataSource = RN.Util.Utils.ListaMes();
            ddlMes.DataBind();
            ddlMes.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void tseRegional_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                divPrincipal.Visible = false;
                pnlImprimir.Visible = false;
                ddlAno.ClearSelection();
                ddlMes.ClearSelection();
                if (!tseRegional.DBValue.IsNull)
                {
                    if (tseRegional.IsValidDBValue)
                    {

                        lblMensagem.Text = string.Empty;
                        ListItem item = ddlAno.Items.FindByValue(DateTime.Today.Year.ToString());
                        if (item != null)
                        {
                            ddlAno.SelectedValue = DateTime.Today.Year.ToString();
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Regional não cadastrada (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Regional não cadastrada (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                divPrincipal.Visible = false;
                pnlImprimir.Visible = false;
                ddlMes.ClearSelection();
              
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                divPrincipal.Visible = false;
                pnlImprimir.Visible = false;
                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue))
                {
                    lblReferencia.InnerText = ddlMes.SelectedItem.Text + "/" + ddlAno.SelectedValue;

                    MontarRelatorio();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void MontarRelatorio()
        {
            try
            {
                DataTable geral = new DataTable();
                string html = string.Empty;
                string moeda = string.Empty;
                decimal valor = 0;
                decimal total = 0;

                RN.Patrimonio.Bem rnBem = new Techne.Lyceum.RN.Patrimonio.Bem();

                geral = rnBem.ObtemControleMensalGestorPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue),Convert.ToInt32(tseRegional.DBValue.ToString()));

                if (geral.Rows.Count > 0)
                {
                    //Monta div principal com css
                    html = "<div><div >";

                    //Adiciona Cabeçalho
                    html += "<table align=\"center\" style=\" border-spacing: 0; border-style: solid; width: 80%; border-collapse: collapse\" border=1 >";
                    html += " <thead><tr style=\"text-align: center;font-weight: bold;font-family: Tahoma, Geneva, sans-serif;font-size: 11px;\" border=1>";
                    html += "<td style=\"font-family: Tahoma, Geneva, sans-serif;font-size: 11px;border-color: #000000; border-style:none solid solid none; \" border=1>Unidade Administrativa</td>";
                    html += "<td style=\"font-family: Tahoma, Geneva, sans-serif;font-size: 11px;border-color: #000000; border-style:none solid solid none; \" border=1>Encarregados/Gestores</td>";
                    html += "<td style=\"font-family: Tahoma, Geneva, sans-serif;font-size: 11px;border-color: #000000; border-style:none solid solid none;\" border=1>Valor da Prestação de Contas(R$)</td> </tr></thead>";

                    //inicia a montagem da table para cada unidade
                    foreach (DataRow item in geral.Select())
                    {
                        valor = 0;
                        html += "<tbody><tr ><td style=\"font-family: Tahoma, Geneva, sans-serif;font-size: 11px;border-color: #000000; border-style:none solid none none; \" border=1>" + item["UNIDADEADMINISTRATIVA"].ToString() + " </td>";
                        html += "<td style=\"font-family: Tahoma, Geneva, sans-serif;font-size: 11px;border-color: #000000; border-style:none solid none none; \" border=1>" + item["AGENTERESPONSAVEL"].ToString() + " </td>";

                        valor = Convert.ToDecimal(item["TOTALCALCULADOCOMSIGLA"].ToString().Substring(2, item["TOTALCALCULADOCOMSIGLA"].ToString().Length - 2).Replace('.', ','));
                        moeda = item["TOTALCALCULADOCOMSIGLA"].ToString().Substring(0, 2);
                        html += "<td style=\"font-family: Tahoma, Geneva, sans-serif;font-size: 11px;text-align: center;border-color: #000000; border-style:none solid none none; \"border=1>" + moeda + " " + string.Format("{0:N2}", valor) + " </td>";

                        total = total + valor;

                        html += "</tr>";
                        html += "</tbody>";

                    }
                    html += "<tr  bgcolor=\"#D8D8D8\" style=\"font-family: Tahoma, Geneva, sans-serif;font-size: 11px;background: #D8D8D8;text-align: center; font-weight: bold;border-color: #000000;\"border=1 ><td colspan=2 style=\"border-style: solid;border-color: #000000;\" border=1 > TOTAL</td><td style=\"font-family: Tahoma, Geneva, sans-serif;font-size: 11px;border-color: #000000; border-style:solid none none solid; \" border=1 ";

                    html += " >" + moeda + " " + string.Format("{0:N2}", total) + "</td></tr>";

                    //fecha a table
                    html += " </table> <br/>  <br/>";

                    //fecha a div
                    html += " </div></div>";

                    divControle.InnerHtml = html;
                    divPrincipal.Visible = true;
                    pnlImprimir.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Não existe controle mensal de gestor para o filtro selecionado.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
        {
            RN.Util.ExportaPdf exportaPdf = new ExportaPdf();

            try
            {
                //Verifica se dados para exportar já estão montados na tela
                if (divPrincipal.Visible)
                {
                    Image1.Src = HttpContext.Current.Server.MapPath("~/Images/logo_govrj.jpg");
                    Image1.Align = "center";

                    //Cria arquivo com div
                    StringBuilder html = new StringBuilder();
                    StringWriter stringWriter = new StringWriter(html);
                    HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
                    divPrincipal.RenderControl(writer);
                    exportaPdf.ExportaHtmlSimplesPor(html.ToString(), "ControleMensal_" + ddlMes.SelectedItem.Text + "_" + ddlAno.SelectedValue);
                }
                else
                {
                    lblMensagem.Text = "Não existem dados à serem exportados.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}

