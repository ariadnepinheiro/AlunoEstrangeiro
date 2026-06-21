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
    [NavUrl("~/Patrimonio/DemonstrativoMovimentacaoConsolidado.aspx"), ControlText("Demonstrativo Movimentação - Anexo I - Geral"), Title("Demonstrativo Movimentação - Anexo I - Geral")]

    public partial class DemonstrativoMovimentacaoConsolidado : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {
                    divPrincipal.Visible = false;
                    pnlImprimir.Visible = false;
                }

                dtDataInicio.MaxDate = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));
                dtDataFim.MaxDate = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                divPrincipal.Visible = false;
                pnlImprimir.Visible = false;
                if (!dtDataInicio.Text.IsNullOrEmptyOrWhiteSpace() && !dtDataFim.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    MontaDados();
                }
                else
                {
                    lblMensagem.Text = "Para efetuar a busca é necessário o preenchimento da Unidade Administrativa, Data Início e Fim do Período.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void MontaDados()
        {
            try
            {
                string html = string.Empty;
                var data = string.Empty;
                DateTime dataSaldoAnterior = new DateTime();
                DateTime dataSaldoFinal = new DateTime();
                DataTable geral = new DataTable();
                DataTable dtDadosSetor = new DataTable();
                RN.Patrimonio.Movimentacao rnMovimentacao = new Techne.Lyceum.RN.Patrimonio.Movimentacao();
                RN.Setores rnSetores = new Setores();
                decimal saldoAnterior = 0;
                decimal entrada = 0;
                decimal saida = 0;
                decimal ajustesMais = 0;
                decimal ajustesMenos = 0;
                decimal saldo = 0;

                geral = rnMovimentacao.ObtemDemonstrativoMovimentacaoPor(dtDataInicio.Date, dtDataFim.Date, out dataSaldoAnterior, out dataSaldoFinal);

                if (geral.Rows.Count > 0)
                {
                    //Monta div principal com css
                    html = "<div><div >";

                    html += "<table align=\"center\" style=\"border-style: solid; border-width: 1px; width: 80%;border-collapse: collapse\">";
                    html += " <tr align=\"center\">";
                    html += "<td align=\"center\" colspan=\"8\" style=\"font-weight: bold; border-bottom-style: solid; border-bottom-width: 1px\">";
                    html += "    BENS PATRIMONIAIS - DEMONSTRATIVO DA MOVIMENTAÇÃO<br />";
                    html += "</td></tr>";
                    html += "<tr align=\"center\">";
                    html += "<td align=\"center\" colspan=\"8\" style=\"font-weight: bold; border-bottom-style: solid; border-bottom-width: 1px\">";
                    html += " PERÍODO " + String.Format("{0:dd/MM/yyyy}", dtDataInicio.Date);
                    html += " &nbsp;A " + String.Format("{0:dd/MM/yyyy}", dtDataFim.Date);
                    html += "</td>  </tr>";

                    html += "<tr align=\"center\">";
                    html += "    <td align=\"center\" colspan=\"8\" style=\"font-weight: bold; border-bottom-style: solid; border-bottom-width: 1px\">";
                    html += "           (ANEXO I - IN 29/2014)";
                    html += "    </td></tr>";

                    html += "<tr> <td align=\"left\" colspan=\"8\" style=\"height: 23px; border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #000000\">";
                    html += " SECRETARIA DE ESTADO DE EDUCAÇÃO </td></tr>";


                    //MONTA CABECALHO
                    html += "<tr align=\"center\">";
                    html += "<td colspan=\"2\" class=\"cabecalhoMovimentacao\">Classificação</td>";
                    html += "<td rowspan=\"2\" class=\"cabecalhoMovimentacao\">Saldo Anterior R$ em <br/>" + String.Format("{0:dd/MM/yyyy}", dataSaldoAnterior) + "</td>";
                    html += "<td colspan=\"2\" class=\"cabecalhoMovimentacao\">Movimento do Período R$ </td>";
                    html += "<td colspan=\"2\" class=\"cabecalhoMovimentacao\">Ajustes R$></td>  ";
                    html += "<td rowspan=\"2\" class=\"cabecalhoMovimentacao\">Saldo R$ em  <br/>" + String.Format("{0:dd/MM/yyyy}", dataSaldoFinal) + "</td></tr>";
                    html += " <tr align=\"center\">";
                    html += " <td class=\"cabecalhoMovimentacao\"> Código do Plano de Contas </td>";
                    html += " <td class=\"cabecalhoMovimentacao\"> Interpretação </td>";
                    html += " <td class=\"cabecalhoMovimentacao\">  Entradas </td>";
                    html += " <td class=\"cabecalhoMovimentacao\"> Saídas </td>";
                    html += " <td class=\"cabecalhoMovimentacao\">  (+) </td>";
                    html += " <td class=\"cabecalhoMovimentacao\">  (-) </td></tr>";

                    //inicia a montagem da table para cada MOVIMENTAÇÃO
                    foreach (DataRow item in geral.Select())
                    {
                        html += "<tbody><tr><td class=\"movimentacao\">" + item["CONTA"].ToString() + " </td>";
                        html += "<td class=\"movimentacao\">" + string.Format("{0:N2}", item["CLASSIFICACAO"].ToString()) + " </td>";

                        html += "<td align=\"center\" class=\"movimentacao\">" + string.Format("{0:N2}", Convert.ToDecimal(item["SALDOANTERIOR"].ToString())) + " </td>";
                        saldoAnterior = saldoAnterior + Convert.ToDecimal(item["SALDOANTERIOR"].ToString());

                        html += "<td align=\"center\" class=\"movimentacao\">" + string.Format("{0:N2}", Convert.ToDecimal(item["ENTRADAS"].ToString())) + " </td>";
                        entrada = entrada + Convert.ToDecimal(item["ENTRADAS"].ToString());

                        html += "<td align=\"center\" class=\"movimentacao\">" + string.Format("{0:N2}", Convert.ToDecimal(item["SAIDAS"].ToString())) + " </td>";
                        saida = saida + Convert.ToDecimal(item["SAIDAS"].ToString());

                        html += "<td align=\"center\" class=\"movimentacao\">" + string.Format("{0:N2}", Convert.ToDecimal(item["AJUSTEMAIS"].ToString())) + " </td>";
                        ajustesMais = ajustesMais + Convert.ToDecimal(item["AJUSTEMAIS"].ToString());

                        html += "<td align=\"center\" class=\"movimentacao\">" + string.Format("{0:N2}", Convert.ToDecimal(item["AJUSTEMENOS"].ToString())) + " </td>";
                        ajustesMenos = ajustesMenos + Convert.ToDecimal(item["AJUSTEMENOS"].ToString());

                        html += "<td align=\"center\" class=\"movimentacao\">" + string.Format("{0:N2}", Convert.ToDecimal(item["SALDOFINAL"].ToString())) + " </td> </tr></tbody>";
                        saldo = saldo + Convert.ToDecimal(item["SALDOFINAL"].ToString());

                    }
                    //TOTALIZADOR
                    html += " <tr><td align=\"right\" class=\"totalMovimentacao\" colspan=\"2\">TOTAL</td>";
                    html += " <td class=\"cabecalhoMovimentacao\" align=\"center\">" + string.Format("{0:N2}", saldoAnterior) + "  </td>";
                    html += " <td class=\"cabecalhoMovimentacao\" align=\"center\">" + string.Format("{0:N2}", entrada) + "  </td>";
                    html += " <td class=\"cabecalhoMovimentacao\" align=\"center\">" + string.Format("{0:N2}", saida) + "  </td>";
                    html += " <td class=\"cabecalhoMovimentacao\" align=\"center\">" + string.Format("{0:N2}", ajustesMais) + "  </td>";
                    html += " <td class=\"cabecalhoMovimentacao\" align=\"center\">" + string.Format("{0:N2}", ajustesMenos) + "  </td>";
                    html += " <td class=\"cabecalhoMovimentacao\" align=\"center\">" + string.Format("{0:N2}", saldo) + "  </td></tr>";
                    html += "  <tr><td colspan=\"8\" class=\"cabecalhoMovimentacao\">&nbsp; </td></tr>";

                    //CABECALHO ASSINATURAS
                    html += " <tr align=\"center\"><td colspan=\"2\" class=\"cabecalhoMovimentacao\">Elaborado por </td>";
                    html += " <td colspan=\"2\" class=\"cabecalhoMovimentacao\">Conferido por</td>";
                    html += " <td colspan=\"2\" class=\"cabecalhoMovimentacao\">Visto</td>";
                    html += " <td colspan=\"2\" class=\"cabecalhoMovimentacao\">Data</td></tr>";

                    html += " <tr><td colspan=\"2\" class=\"assinaturaMovimentacao\"> Nome do Servidor<br /> ID<br />  Assinatura</td>";
                    html += "<td colspan=\"2\" class=\"assinaturaMovimentacao\"></td>";
                    html += "<td colspan=\"2\" class=\"assinaturaMovimentacao\"></td>";
                    html += "<td colspan=\"2\" class=\"assinaturaMovimentacao\"></td> </tr>";
                    html += "</table>  <br/>  <br/>";

                    //fecha a div
                    html += " </div></div>";

                    divControle.InnerHtml = html;
                    pnlImprimir.Visible = true;
                    divPrincipal.Visible = true;

                }
                else
                {
                    lblMensagem.Text = "Não existe demonstrativo de movimentação para o filtro selecionado.";
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

                    //Cria css
                    string cssText = File.ReadAllText(HttpContext.Current.Server.MapPath("../LyceumNet.css"));
                    cssText = cssText + File.ReadAllText(HttpContext.Current.Server.MapPath("../Scripts/themes/RelatorioPatrimonio.css"));

                    exportaPdf.ExportaHtmlCssOrientacaoPaisagemPor(html.ToString(), "DemonstrativoMovimentacao_" + String.Format("{0:dd/MM/yyyy}", dtDataInicio.Text) + "_" + String.Format("{0:dd/MM/yyyy}", dtDataFim.Text), cssText);
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
