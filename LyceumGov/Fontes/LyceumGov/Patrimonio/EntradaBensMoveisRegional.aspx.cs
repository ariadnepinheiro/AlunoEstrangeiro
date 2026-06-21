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
using DevExpress.Web.ASPxEditors;
using Techne.Controls;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/EntradaBensMoveisRegional.aspx"), ControlText("Entrada Bens Móveis - Regional"), Title("Entrada Bens Móveis - Regional")]

    public partial class EntradaBensMoveisRegional : TPage
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
                    ListItem item = ddlAno.Items.FindByValue(DateTime.Today.Year.ToString());
                    if (item != null)
                    {
                        ddlAno.SelectedValue = DateTime.Today.Year.ToString();
                    }
                    CarregarMes(ddlMesInicial);
                    CarregarMes(ddlMesFinal);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {
                    CarregaAno();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void CarregarMes(DropDownList controle)
        {
            controle.Items.Clear();
            controle.DataSource = RN.Util.Utils.ListaMes();
            controle.DataBind();
            controle.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregaAno()
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            ddlAno.Items.Clear();
            ddlAno.DataSource = rnPeriodoLetivo.ListaAnos(false);
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void rblTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseUnidadeAdministrativa.ResetValue();
                tseRegional.ResetValue();
                tseClassificacao.ResetValue();
                ListItem item = ddlAno.Items.FindByValue(DateTime.Today.Year.ToString());
                if (item != null)
                {
                    ddlAno.SelectedValue = DateTime.Today.Year.ToString();
                }
                ddlMesInicial.ClearSelection();
                ddlMesFinal.ClearSelection();
                pnlFiltros.Visible = true;
                divPrincipal.Visible = false;
                pnlImprimir.Visible = false;

                if (rblTipoFiltro.SelectedValue == "R")
                {
                    pnlUnidade.Visible = false;
                }
                else
                {
                    pnlUnidade.Visible = true;
                }
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

                if (rblTipoFiltro.SelectedValue == "U")
                {
                    if ((!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue) && (!tseUnidadeAdministrativa.DBValue.IsNull && tseUnidadeAdministrativa.IsValidDBValue) && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlMesInicial.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlMesFinal.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMeses.InnerHtml = "BENS PATRIMONIAIS - ENTRADAS DE " + ddlMesInicial.SelectedItem.Text.ToUpper() + " A " + ddlMesFinal.SelectedItem.Text.ToUpper() + " DE " + ddlAno.SelectedValue;
                        lblNomeSetor.InnerHtml = tseUnidadeAdministrativa["nome"].ToString();
                        lblSetor.InnerHtml = tseUnidadeAdministrativa.DBValue.ToString();
                    }
                    else
                    {
                        lblMensagem.Text = "Para efetuar a busca é necessário o preenchimento da Regional, Unidade Administrativa, Mês Inicial e Final.";
                        return;
                    }
                }
                else
                {
                    if ((!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue) && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlMesInicial.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlMesFinal.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMeses.InnerHtml = "BENS PATRIMONIAIS - ENTRADAS DE " + ddlMesInicial.SelectedItem.Text.ToUpper() + " A " + ddlMesFinal.SelectedItem.Text.ToUpper() + " DE " + ddlAno.SelectedValue;
                        lblNomeSetor.InnerHtml = tseRegional["descricao"].ToString();
                        lblSetor.InnerHtml = string.Empty;


                    }
                    else
                    {
                        lblMensagem.Text = "Para efetuar a busca é necessário o preenchimento da Regional, Mês Inicial e Final.";
                        return;
                    }
                }
                MontarRelatorio();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                pnlImprimir.Visible = false;
                divPrincipal.Visible = false;

                if (!tseRegional.DBValue.IsNull)
                {
                    if (!tseRegional.IsValidDBValue)
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

        protected void tseUnidadeAdministrativa_Changed(object sender, ChangedEventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }
            try
            {
                pnlImprimir.Visible = false;
                divPrincipal.Visible = false;

                if (!this.tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    if (!this.tseUnidadeAdministrativa.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade administrativa.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseClassificacao_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (!this.tseClassificacao.DBValue.IsNull)
                {
                    if (!this.tseClassificacao.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Classificação não cadastrada.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma classificacao.";
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

                RN.Patrimonio.Movimentacao rnMovimentacao = new Techne.Lyceum.RN.Patrimonio.Movimentacao();

                geral = rnMovimentacao.ObtemEntradaBensMoveisPor(((!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue) ? Convert.ToInt32(tseRegional.DBValue) : 0), ((!tseUnidadeAdministrativa.DBValue.IsNull && tseUnidadeAdministrativa.IsValidDBValue) ? tseUnidadeAdministrativa["setor"].ToString() : null), ((!tseClassificacao.DBValue.IsNull && tseClassificacao.IsValidDBValue) ? Convert.ToInt32(tseClassificacao["CLASSIFICACAOID"].ToString()) : 0), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMesInicial.SelectedValue), Convert.ToInt32(ddlMesFinal.SelectedValue));

                if (geral.Rows.Count > 0)
                {
                    //Monta div principal com css
                    html = "<div><div >";

                    //Adiciona Cabeçalho
                    html += "<table align=\"center\" style=\" width: 80%; border-spacing: 0;border-collapse: collapse \" border=0 >";
                    html += " <thead><tr border=1 style=\" height: 10px;border: 1px solid #000000;text-align: center;font-weight: bold;background: #D8D8D8;\" >";
                    html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" >Unidade/ Unidade Apoiada/Subunidade</td>";
                    html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" >CÓDIGO DE CLASSIFICAÇÃO</td>";
                    html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" >NÚMERO DE PATRIMÔNIO</td>";
                    html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" >CARACTERÍSTICA DE IDENTIFICAÇÃO</td>";
                    html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" >UNIDADE DE MEDIDA</td>";
                    html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" >QTDE</td>";
                    html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" >VALOR UNITÁRIO</td>";
                    html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" >VALOR GLOBAL</td>";
                    html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" >OBSERVAÇÃO</td> </tr></thead>";

                    //inicia a montagem da table para cada unidade
                    foreach (DataRow item in geral.Select())
                    {
                        //Busca a primeira movimentacao do bem 
                        int id = Convert.ToInt32(item["MOVIMENTACAOID"].ToString());
                        int primeiraMovimentacao = rnMovimentacao.ObtemPrimeiraMovimentacaoPor(id);
                        string valorInicial;

                        //Verifica se é a primeira movimentacao do bem
                        if (id == primeiraMovimentacao)
                        {
                            //Caso seja o primeiro dono do item, mostar o valor de aquisição da entrada
                            valorInicial = string.Format("{0:N2}", Convert.ToDecimal(item["VALORINICIAL"].ToString()));
                            valor = valor + Convert.ToDecimal(item["VALORINICIAL"].ToString());
                        }
                        else
                        {
                            //CAso ja tenha tido transferencias, mostar o VALORCALCULADO 
                            valorInicial = string.Format("{0:N2}", Convert.ToDecimal(item["VALORCALCULADO"].ToString()));
                            valor = valor + Convert.ToDecimal(item["VALORCALCULADO"].ToString());
                        }

                        html += "<tbody><tr ><td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\"  >" + item["UNIDADEADMINISTRATIVA"].ToString() + " </td>";
                        html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\"  align=\"center\" >" + item["CONTA"].ToString() + " </td>";
                        html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\"  align=\"center\" >" + item["NUMERO"].ToString() + " </td>";
                        html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" align=\"left\" >" + item["DESCRICAO"].ToString() + " </td>";
                        html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\"  align=\"center\">" + item["UNIDADEMEDIDA"].ToString() + " </td>";
                        html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\"  align=\"center\">" + item["QUANTIDADE"].ToString() + " </td>";
                        html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\"  align=\"right\">" + valorInicial + " </td>";
                        html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\"  align=\"right\">" + valorInicial + " </td>";
                        html += "<td border=1 style=\"border: 1px solid #000000;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" align=\"left\" >" + item["OBSERVACAO"].ToString() + " </td>";
                        html += "</tr>";
                        html += "</tbody>";
                    }

                    html += "<tr style=\"text-align: right; font-weight: bold;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\" ><td colspan=4 border=\"0\"  style=\"border-width: 1px;font-family: Tahoma, Geneva, sans-serif; font-size: 11px; border-style: solid solid none none;\"></td><td colspan=3 border=\"1\" style=\"border-width: 1px; border-style: solid;\">SOMA TOTAL</td>";
                    html += " <td border=\"1\" style=\"border-width: 1px; border-style: solid;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\"> " + string.Format("{0:N2}", valor) + "</td>  <td  style=\"border-width: 1px; border-style: solid none none solid;font-family: Tahoma, Geneva, sans-serif; font-size: 11px;\"></td></tr>";

                    //fecha a table
                    html += " </table> <br/>  <br/>";

                    //fecha a div
                    html += " </div></div>";

                    divControle.InnerHtml = html;
                    pnlImprimir.Visible = true;
                    divPrincipal.Visible = true;

                }
                else
                {
                    lblMensagem.Text = "Não existem entradas para o período selecionado.";
                }
            }
            catch (Exception ex)
            {
                if (Convert.ToString(ex.Message).Contains("ERRO:") || Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados"))
                {
                    lblMensagem.Text = ex.Message.Replace("ERRO: ", string.Empty);
                }
                else
                {
                    lblMensagem.Text = "Falha na geração do relatório. Tempo limite de processamento atingido ou quantidade de registros.";
                }
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
                    exportaPdf.ExportaHtmlSimplesOrientacaoPaisagemPor(html.ToString(), "EntradaBensMoveis_" + tseUnidadeAdministrativa.DBValue.ToString() + "_" + ddlMesInicial.SelectedItem.Text.ToUpper() + "_" + ddlMesFinal.SelectedItem.Text.ToUpper());
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
