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

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/AvisoTransferenciaBens.aspx"), ControlText("Aviso de Transferência de Bens Móveis - ATBM"), Title("Aviso de Transferência de Bens Móveis - ATBM")]
    public partial class AvisoTransferenciaBens : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        CarregaQueryString();
                        MontarRelatorio();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaQueryString()
        {
            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

            string[] listaDados = decodedText.Split('&');

            lblLote.InnerHtml = string.Empty;
            lblAno.InnerHtml = string.Empty;
            lblUnidadeCedente.InnerHtml = string.Empty;
            lblUnidadeDestino.InnerHtml = string.Empty;
            lblUACedente.InnerHtml = string.Empty;
            lblUADestino.InnerHtml = string.Empty;

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("Lote") >= 0)
                {
                    lblLote.InnerHtml = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("Cedente") >= 0)
                {
                    lblUnidadeCedente.InnerHtml = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("Destinataria") >= 0)
                {
                    lblUnidadeDestino.InnerHtml = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("SetorOrigem") >= 0)
                {
                    lblUACedente.InnerHtml = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("SetorDestino") >= 0)
                {
                    lblUADestino.InnerHtml = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("Ano") >= 0)
                {
                    lblAno.InnerHtml = dados.Substring(dados.LastIndexOf('=') + 1);
                }
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

                RN.Patrimonio.Transferencia rnTransferencia = new Techne.Lyceum.RN.Patrimonio.Transferencia();

                if (!lblLote.InnerHtml.IsNullOrEmptyOrWhiteSpace())
                {
                    geral = rnTransferencia.ObtemAvisoTransferenciaBensMoveisPor(Convert.ToInt32(lblLote.InnerHtml));

                    if (geral.Rows.Count > 0)
                    {
                        //Monta div principal com css
                        html = "<div><div >";

                        lblDataSolicitacao.InnerHtml = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(geral.Rows[0]["DATASOLICITACAO"]));

                        html += "<table align=\"center\" style=\" width: 100%; border-spacing: 0; \" border=\"0\">";
                        html += "<thead><tr style=\"text-align: center;font-weight: bold;font-family: Tahoma, Geneva, sans-serif;font-size: 11px;\" border=1 bgcolor=\"#D8D8D8\"\">";
                        html += "   <td border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000; border-style:solid ;border-spacing: 0; border-width:1px \">";
                        html += "      CÓDIGO DE CLASSIFICAÇÃO";
                        html += " </td>";
                        html += " <td border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000; border-style:solid ;border-spacing: 0; border-width:1px \">";
                        html += "      NÚMEROS DE PATRIMÔNIO";
                        html += "  </td>";
                        html += " <td border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000; border-style:solid ;border-spacing: 0; border-width:1px \">";
                        html += "     CARACTERÍSTICA DE IDENTIFICAÇÃO";
                        html += "  </td>";
                        html += "  <td border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000; border-style:solid ;border-spacing: 0; border-width:1px \">";
                        html += "      UNIDADE DE MEDIDA";
                        html += "    </td>";
                        html += "   <td border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000; border-style:solid ;border-spacing: 0; border-width:1px \">";
                        html += "         QTDE";
                        html += "      </td>";
                        html += "     <td border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000; border-style:solid ;border-spacing: 0; border-width:1px \">";
                        html += "          VALOR UNITÁRIO";
                        html += "        </td>";
                        html += "          <td border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000; border-style:solid ;border-spacing: 0; border-width:1px \">";
                        html += "             VALOR GLOBAL";
                        html += "         </td>";
                        html += "        <td border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-color: #000000; border-style:solid ;border-spacing: 0; border-width:1px \">";
                        html += "               OBSERVAÇÃO";
                        html += "          </td>";
                        html += "        </tr></thead>";

                        //inicia a montagem da table para cada unidade
                        foreach (DataRow item in geral.Select())
                        {
                            valor = 0;
                            html += "<tbody><tr align=\"center\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" ><td align=\"center\" border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px;border-style:solid; border-width:1px\">" + item["CONTA"].ToString() + " </td>";
                            html += "<td align=\"center\" border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px;border-style:solid; border-width:1px\">" + item["NUMEROBEMORIGEM"].ToString() + " </td>";
                            html += "<td align=\"left\" border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px;border-style:solid; border-width:1px\">" + item["DESCRICAO"].ToString() + " </td>";
                            html += "<td align=\"center\" border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px;border-style:solid; border-width:1px\">" + item["UNIDADEMEDIDA"].ToString() + " </td>";
                            html += "<td align=\"center\" border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px;border-style:solid; border-width:1px\">" + item["QUANTIDADE"].ToString() + " </td>";

                            valor = Convert.ToDecimal(item["VALORCOMSIGLA"].ToString().Substring(2, item["VALORCOMSIGLA"].ToString().Length - 2).Replace('.', ','));
                            moeda = item["VALORCOMSIGLA"].ToString().Substring(0, 2);
                            html += "<td border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px;border-style:solid; border-width:1px\">" + moeda + " " + string.Format("{0:N2}", valor) + " </td>";
                            html += "<td border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px;border-style:solid; border-width:1px\">" + moeda + " " + string.Format("{0:N2}", valor) + " </td>";
                            html += "<td align=\"left\" border=\"1\" style=\"font-family: Tahoma, Geneva, sans-serif; font-size: 11px;border-style:solid; border-width:1px\">" + item["DOCUMENTOHABIL"].ToString() + " </td>";

                            total = total + valor;

                            html += "</tr>";
                            html += "</tbody>";
                        }

                        html += "<tr><td colspan=4 >&nbsp;";
                        html += "</td><td align=\"center\" border=\"1\" colspan=2 style=\"font-weight: bold; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;border-style:solid; border-width:1px\">Total";
                        html += "</td><td border=\"1\" style=\"font-weight: bold; font-family: Tahoma, Geneva, sans-serif; font-size: 11px;border-style:solid; border-width:1px\" align=\"center\">" + moeda + " " + string.Format("{0:N2}", total) + "</td>";
                        html += "<td> &nbsp;</td> </tr>";
                        //fecha a table
                        html += " </table> <br/>  <br/>";

                        //fecha a div
                        html += " </div></div>";

                        divControle.InnerHtml = html;
                        pnlImprimir.Visible = true;
                    }
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

                    //Cria arquivo com div
                    StringBuilder html = new StringBuilder();
                    StringWriter stringWriter = new StringWriter(html);
                    HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
                    divPrincipal.RenderControl(writer);

                    //Cria css
                    string cssText = File.ReadAllText(HttpContext.Current.Server.MapPath("../LyceumNet.css"));
                    cssText = cssText + File.ReadAllText(HttpContext.Current.Server.MapPath("../Scripts/themes/RelatorioPatrimonio.css"));

                    exportaPdf.ExportaHtmlCssOrientacaoPaisagemPor(html.ToString(), "AvisoTransferenciaBens" + String.Format("{0:dd/MM/yyyy}", DateTime.Now), cssText);
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

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string queryString = MontarQueryString();

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                Response.Redirect("ListaAvisoTransferenciaBens.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private string MontarQueryString()
        {
            string queryString = string.Empty;

            if (!lblUACedente.InnerHtml.IsNullOrEmptyOrWhiteSpace() && !lblUADestino.InnerHtml.IsNullOrEmptyOrWhiteSpace())
            {
                queryString = "Cedente=" + lblUACedente.InnerHtml + "&Destino=" + lblUADestino.InnerHtml;
            }
            return queryString;
        }

    }
}
