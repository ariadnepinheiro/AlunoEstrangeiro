using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.Data;
using Techne.Lyceum.RN.PrestacaoContas;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Techne.Controls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using Techne.Lyceum.RN.PrestacaoContas.DTOs;
using Seeduc.Infra.Data;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using Seeduc.Infra.Helpers;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/ConsultaNotaFiscal.aspx"), ControlText("Consulta - Nota Fiscal"), Title("Consulta - Nota Fiscal")]
    public partial class ConsultaNotaFiscal : TPage
    {
        private readonly Techne.Lyceum.RN.PrestacaoContas.Evento rnEvento = new Techne.Lyceum.RN.PrestacaoContas.Evento();
        private readonly Techne.Lyceum.RN.PrestacaoContas.Fornecedor rnFornecedor = new Techne.Lyceum.RN.PrestacaoContas.Fornecedor();
        private readonly Techne.Lyceum.RN.PrestacaoContas.ImportacaoXmlEvento rnImportacaoXmlEvento = new Techne.Lyceum.RN.PrestacaoContas.ImportacaoXmlEvento();
        private readonly Techne.Lyceum.RN.PrestacaoContas.ExigenciaEvento rnExigenciaEvento = new Techne.Lyceum.RN.PrestacaoContas.ExigenciaEvento();

        private int EventoIdSelecionado
        {
            get { return ViewState["EventoIdSelecionado"] != null ? (int)ViewState["EventoIdSelecionado"] : 0; }
            set { ViewState["EventoIdSelecionado"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdExigenciaEvento.Settings.ShowTitlePanel = true;
            grdExigenciaEvento.SettingsText.Title = "Exigências da Despesa";

            grdItensNf.Settings.ShowTitlePanel = true;
            grdItensNf.SettingsText.Title = "";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ConsultaDespesaComumExistente();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            plaConsultaNotaFiscal.Visible = false;
            btnCancel.Visible = false;
            txtChave.Value = "";
        }

        private void ConsultaDespesaComumExistente()
        {
            try
            {
                lblMensagem.Text = "";
                var evento = rnEvento.ObtemDadosEventoPorChave(txtChave.Value);

                if (evento == null || string.IsNullOrEmpty(evento.Descricao))
                {
                    lblMensagem.Text = "Nenhum registro foi encontrado para a chave de acesso informada. Verifique e tente novamente.";
                    plaConsultaNotaFiscal.Visible = false;
                    btnCancel.Visible = false;
                    return;
                }

                if (txtChave.Value == "")
                {
                    lblMensagem.Text = "Por favor, informe a chave de acesso para realizar a consulta.";
                    plaConsultaNotaFiscal.Visible = false;
                    btnCancel.Visible = false;
                    return;
                }

                plaConsultaNotaFiscal.Visible = true;
                btnCancel.Visible = true;

                EventoIdSelecionado = evento.EventoId;

                grdXML.DataBind();

                txtDescricao.Text = evento.Descricao;

                rblFiltroTipoEvento.SelectedValue = "0";

                if (evento.TipoDespesa == 1)
                {
                    rblDocumentoFiscal.SelectedValue = "1";
                }
                else
                {
                    rblDocumentoFiscal.SelectedValue = "0";
                }

                var fornecedor = rnFornecedor.ObtemPor(evento.FornecedorId ?? 0);
                if (fornecedor != null)
                {
                    lblFornecedor.Text = fornecedor.FornecedorId + " - " + fornecedor.RazaoSocial;
                }

                lnkVisualizarOrcamento1.Visible = evento.Orcamento1Id.HasValue;
                lnkVisualizarOrcamento1.CommandArgument = (evento.Orcamento1Id ?? 0).ToString() + "," + (!evento.Orcamento1TipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.Orcamento1TipoArquivo : "");
                hidOrcamento1.Value = string.Empty;
                lnkVisualizarOrcamento2.Visible = evento.Orcamento2Id.HasValue;
                lnkVisualizarOrcamento2.CommandArgument = (evento.Orcamento2Id ?? 0).ToString() + "," + (!evento.Orcamento2TipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.Orcamento2TipoArquivo : "");
                hidOrcamento2.Value = string.Empty;
                lnkVisualizarOrcamento3.Visible = evento.Orcamento3Id.HasValue;
                lnkVisualizarOrcamento3.CommandArgument = (evento.Orcamento3Id ?? 0).ToString() + "," + (!evento.Orcamento3TipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.Orcamento3TipoArquivo : "");
                hidOrcamento3.Value = string.Empty;
                txtJustificativa.Text = evento.JustificativaOrcamento;

                txtChaveAcesso.Text = evento.ChaveAcesso;
                txtNumeroNF.Text = evento.NumeroNotaFiscal;
                txtValorTotalNF.Text = evento.ValorNotaFiscal.HasValue ? evento.ValorNotaFiscal.Value.ToString("N2") : "";
                txtDataNF.Text = evento.DataNotaFiscal.HasValue ? evento.DataNotaFiscal.Value.ToString("dd/MM/yyyy") : "";
                txtValorPagoNF.Text = evento.ValorPagamento.ToString("N2");
                txtDataPagamentoNF.Text = evento.DataPagamento.HasValue ? evento.DataPagamento.Value.ToString("dd/MM/yyyy") : "";

                lnkVisualizarNotaFiscal.Visible = evento.NotaFiscalArquivoId.HasValue;
                lnkVisualizarNotaFiscal.CommandArgument = evento.NotaFiscalArquivoId.ToString() + "," + (!evento.NotaFiscalTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.NotaFiscalTipoArquivo : "");
                hidNotaFiscal.Value = string.Empty;
                lnkVisualizarComprovantePgto.Visible = evento.ComprovantePagamentoArquivoId.HasValue;
                lnkVisualizarComprovantePgto.CommandArgument = evento.ComprovantePagamentoArquivoId.ToString() + "," + (!evento.ComprovantePagamentoTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.ComprovantePagamentoTipoArquivo : "");
                hidComprovantePgto.Value = string.Empty;
                lnkVisualizarXML.Visible = evento.PossuiXmlImportado;
                lnkVisualizarXML.Text = "XML inserido";
                if (evento.PossuiXmlImportado && rnEvento.PossuiXmlGeradoInternamente(evento.EventoId))
                {
                    lnkVisualizarXML.Text = "Nota sem XML";
                }

                hidXML.Value = string.Empty;

                txtObservacao.Text = evento.Observacoes;
                lnkVisualizarEvidencia.Visible = evento.EvidenciaArquivoId.HasValue;
                lnkVisualizarEvidencia.CommandArgument = evento.EvidenciaArquivoId.ToString() + "," + (!evento.EvidenciaTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.EvidenciaTipoArquivo : "");
                hidEvidencia.Value = string.Empty;

                lblStatusEvento.Visible = (evento.StatusAnalise != "Aberto");
                lblStatusEvento.Text = evento.StatusAnalise;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lnkVisualizarOrcamento_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "OrcamentoArquivo";
                RN.PrestacaoContas.OrcamentoArquivo rnArquivo = new OrcamentoArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
                        bimgArquivo.Visible = true;
                    }
                }
                else
                {
                    var evento = rnEvento.ObtemDadosEventoPorChave(txtChave.Value);
                    lblMensagem.Text = evento.Orcamento1TipoArquivo;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para visualização');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lnkVisualizarNotaFiscal_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "EventoNotaFiscalArquivo";
                RN.PrestacaoContas.EventoNotaFiscalArquivo rnArquivo = new EventoNotaFiscalArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
                        bimgArquivo.Visible = true;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para visualização');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lnkVisualizarComprovantePgto_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "ComprovantePagamentoArquivo";
                RN.PrestacaoContas.ComprovantePagamentoArquivo rnArquivo = new ComprovantePagamentoArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
                        bimgArquivo.Visible = true;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para visualização');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lnkVisualizarXML_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var evento = rnEvento.ObtemDadosEventoPorChave(txtChave.Value);
                var dados = rnImportacaoXmlEvento.ListaItensXmlPor(evento.EventoId, evento.Censo);

                grdXML.DataSource = dados;
                grdXML.DataBind();

                pucVisualizarXML.ShowOnPageLoad = true;
                pucVisualizarXML.Width = Unit.Pixel(880);
                pucVisualizarXML.Height = Unit.Pixel(580);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarGridXML()
        {
            if (EventoIdSelecionado == 0) return;

            var evento = rnEvento.ObtemDadosEventoPorChave(txtChave.Value);
            var dados = rnImportacaoXmlEvento.ListaItensXmlPor(evento.EventoId, evento.Censo);

            grdXML.DataSource = dados;
        }

        protected void grdXML_DataBinding(object sender, EventArgs e)
        {
            CarregarGridXML();
        }

        protected void lnkVisualizarEvidencia_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "EvidenciaArquivo";
                RN.PrestacaoContas.EvidenciaArquivo rnEvidenciaArquivo = new EvidenciaArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnEvidenciaArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
                        bimgArquivo.Visible = true;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para visualização');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public DataTable ListaExigenciaEvento(string chave)
        {
            if (chave == null || chave.Trim() == "")
            {
                return new DataTable();
            }

            var evento = rnEvento.ObtemDadosEventoPorChave(chave);

            if (evento == null)
            {
                return new DataTable();
            }

            return rnExigenciaEvento.ListaExigenciasPor(evento.EventoId);
        }

        public DataTable ListaXML(string chave)
        {
            if (chave == null || chave.Trim() == "")
            {
                return new DataTable();
            }

            var evento = rnEvento.ObtemDadosEventoPorChave(chave);

            if (evento == null)
            {
                return new DataTable();
            }

            return rnImportacaoXmlEvento.ListaItensXmlPor(evento.EventoId, evento.Censo);
        }

        protected void btnVisualizar_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "ExigenciaEventoArquivo";
                RN.PrestacaoContas.ExigenciaEventoArquivo rnArquivo = new ExigenciaEventoArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((ImageButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
                        bimgArquivo.Visible = true;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para visualização');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}