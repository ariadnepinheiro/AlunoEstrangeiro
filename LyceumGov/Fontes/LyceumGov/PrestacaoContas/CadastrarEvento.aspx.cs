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
    [NavUrl("~/PrestacaoContas/CadastrarEvento.aspx"), ControlText("Cadastrar Despesa"), Title("Cadastrar Despesa")]
    public partial class CadastrarEvento : TPage
    {
        private readonly Techne.Lyceum.RN.PrestacaoContas.Evento rnEvento = new Techne.Lyceum.RN.PrestacaoContas.Evento();
        private readonly Techne.Lyceum.RN.PrestacaoContas.PequenaDespesa rnPequenaDespesa = new Techne.Lyceum.RN.PrestacaoContas.PequenaDespesa();
        private readonly Techne.Lyceum.RN.PrestacaoContas.Fornecedor rnFornecedor = new Techne.Lyceum.RN.PrestacaoContas.Fornecedor();
        private readonly Techne.Lyceum.RN.PrestacaoContas.ExigenciaEvento rnExigenciaEvento = new Techne.Lyceum.RN.PrestacaoContas.ExigenciaEvento();
        private readonly Techne.Lyceum.RN.PrestacaoContas.ExigenciaEventoArquivo rnExigenciaEventoArquivo = new Techne.Lyceum.RN.PrestacaoContas.ExigenciaEventoArquivo();
        private readonly Techne.Lyceum.RN.PrestacaoContas.TipoTransporte rnTipoTransporte = new Techne.Lyceum.RN.PrestacaoContas.TipoTransporte();
        private readonly Techne.Lyceum.RN.PrestacaoContas.ImportacaoXmlEvento rnImportacaoXmlEvento = new Techne.Lyceum.RN.PrestacaoContas.ImportacaoXmlEvento();
        private readonly Techne.Lyceum.RN.PrestacaoContas.PlanoTrabalho rnPlanoTrabalho = new Techne.Lyceum.RN.PrestacaoContas.PlanoTrabalho();

        protected void Page_Init()
        {
            TituloGrid(grdExigenciaEvento, "Exigências da Despesa");
            TituloGrid(grdXML, "Dados do XML");
            TituloGrid(grdServidores_DCTS, "Servidores Incluídos");
            TituloGrid(grdItensNf, string.Empty);


        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnEditarDatas, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnDeletar, AcaoControle.excluir);
            ControlaAcesso(grdExigenciaEvento);
            ControlaAcesso(grdItensNf);
            ControlaAcesso(grdServidores_DCTS);
            ControlaAcesso(grdXML);            
            ControlaAcesso(btnImportar, AcaoControle.editar);
            ControlaAcesso(btnEnviarEvento, AcaoControle.editar);


        }

        protected void grdExigenciaEvento_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

            if (!this.grdExigenciaEvento.Visible || this.grdExigenciaEvento.VisibleRowCount == 0)
            {
                return;
            }
            var btnDetalhes = DevExpressHelper.GetControl<ImageButton>(this.grdExigenciaEvento, e.VisibleIndex, "btnDetalhes", "btnDetalhes");
            var id = grdExigenciaEvento.GetRowValues(e.VisibleIndex, "EXIGENCIAEVENTOID");

            btnDetalhes.Visible = false;

            //Verifica se tem permissão
            if (Permission.AllowUpdate)
            {
                if (Convert.ToInt32(id) > 0)
                {
                    btnDetalhes.Visible = true;
                }
            }

        }

        protected void Page_Load()
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (Request["__EVENTTARGET"] == "FiltraEvento")
                    if (FiltraEvento())
                        ModoTela = ModoTelaEnum.ConsultaVazio;
                    else
                        ModoTela = ModoTelaEnum.FiltroVazio;

                if (!IsPostBack)
                {
                    CarregaModal();

                    //Verifica se é redirecinamento da tela de exigencias
                    if (!string.IsNullOrEmpty(Request.QueryString["Chave"]))
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                        this.ObterDadosQueryString(decodedText);
                    }
                }

                int fornecedorId_DC;
                if (pcCadastrarEvento.ActiveTabPage == pcCadastrarEvento.TabPages.FindByName("tabDespesaComum") && int.TryParse((tseFornecedor.Value ?? "").ToString(), out fornecedorId_DC))
                    FornecedorPopup_DC.FornecedorId = fornecedorId_DC;

                int fornecedorId_DCC;
                if (pcCadastrarEvento.ActiveTabPage == pcCadastrarEvento.TabPages.FindByName("tabPequenaDespesaComComprovacao") && int.TryParse((tseFornecedor_DCC.Value ?? "").ToString(), out fornecedorId_DCC))
                    FornecedorPopup_DC.FornecedorId = fornecedorId_DCC;

                int fornecedorId_DSC;
                if (pcCadastrarEvento.ActiveTabPage == pcCadastrarEvento.TabPages.FindByName("tabPequenaDespesaSemComprovacao") && int.TryParse((tseFornecedor_DSC.Value ?? "").ToString(), out fornecedorId_DSC))
                    FornecedorPopup_DC.FornecedorId = fornecedorId_DSC;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ObterDadosQueryString(string queryString)
        {
            try
            {
                var listaDados = queryString.Split('&');

                string eventoId = string.Empty;
                string censo = string.Empty;
                string planoTrabalhoId = string.Empty;
                string tipoEvento = string.Empty;
                string referencia = string.Empty;

                foreach (var dados in listaDados)
                {
                    if (dados.IndexOf("tipoEvento") >= 0)
                    {
                        tipoEvento = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("eventoId") >= 0)
                    {
                        eventoId = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("censo") >= 0)
                    {
                        censo = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("planoTrabalhoId") >= 0)
                    {
                        planoTrabalhoId = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("referencia") >= 0)
                    {
                        referencia = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                }

                //Verifica preenchimento dos campos
                if (!tipoEvento.IsNullOrEmptyOrWhiteSpace() && !eventoId.IsNullOrEmptyOrWhiteSpace()
                    && !censo.IsNullOrEmptyOrWhiteSpace() && !planoTrabalhoId.IsNullOrEmptyOrWhiteSpace()
                    && !referencia.IsNullOrEmptyOrWhiteSpace())
                {
                    tsePlanoTrabalho.DBValue = planoTrabalhoId;
                    tsePlanoTrabalho.DataBind();
                    tseUnidadeEnsino.DBValue = censo;
                    tseUnidadeEnsino.DataBind();
                    rblFiltroTipoEvento.SelectedValue = tipoEvento;
                    tsePeriodoPrestacaoContas.DBValue = referencia;
                    tsePeriodoPrestacaoContas.DataBind();
                    FiltraEvento();
                    tseEvento.DataBind();
                    tseEvento.DBValue = eventoId;
                    tseEvento_Changed(null, null);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsePlanoTrabalho_Changed(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                rblFiltroTipoEvento.ClearSelection();
                hidPermitePequenaDespesa.Value = string.Empty;
                rblDocumentoFiscal.ClearSelection();

                if (!tsePlanoTrabalho.DBValue.IsNull && tsePlanoTrabalho.IsValidDBValue)
                {
                    hidPermitePequenaDespesa.Value = rnPlanoTrabalho.PermitePequenaDespesa(Convert.ToInt32(tsePlanoTrabalho.Value)).ToString();

                    if (tsePlanoTrabalho["FINALIDADEID"].ToString() == "2") //MERENDA
                    {
                        rblDocumentoFiscal.Items.FindByValue("1").Enabled = false;
                    }
                    else
                    {
                        rblDocumentoFiscal.Items.FindByValue("1").Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseEvento_Changed(object sender, EventArgs e)
        {
            try
            {
                if (!tseEvento.IsValidDBValue)
                {
                    ModoTela = ModoTelaEnum.ConsultaVazio;
                    return;
                }

                var tipoDespesa = ObtemTipoDespesa();

                switch (tipoDespesa)
                {
                    case 0:

                        ModoTela = ModoTelaEnum.ConsultaDespesaComumExistente;
                        ConsultaDespesaComumExistente();
                        break;

                    case 1:

                        ModoTela = ModoTelaEnum.ConsultaDespesaComumExistente;
                        ConsultaDespesaComumExistente();
                        break;

                    case 2:

                        ModoTela = ModoTelaEnum.ConsultaPequenaDespesaComComprovacaoExistente;
                        ConsultaPequenaDespesaComComprovacaoExistente();
                        break;

                    case 3:

                        ModoTela = ModoTelaEnum.ConsultaPequenaDespesaComTransladoExistente;
                        ConsultaPequenaDespesaComTransladoExistente();
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseFornecedor_PreRender(object sender, EventArgs e)
        {
            if (new ModoTelaEnum[] { ModoTelaEnum.InserirNovaDespesaComum, ModoTelaEnum.EditarDespesaComumExistente }.Contains(ModoTela))
                tseFornecedor.Visible = true;
            else
                tseFornecedor.Visible = false;
        }

        protected void tseFornecedor_Changed(object sender, EventArgs e)
        {
            var self = sender as TSearchBox;

            var fornecedorId = self.Value;
        }

        protected void tseFornecedor_DCC_PreRender(object sender, EventArgs e)
        {
            if (new ModoTelaEnum[] { ModoTelaEnum.InserirNovaPequenaDespesa, ModoTelaEnum.EditarPequenaDespesaComComprovacaoExistente }.Contains(ModoTela))
                tseFornecedor_DCC.Visible = true;
            else
                tseFornecedor_DCC.Visible = false;
        }

        protected void tseFornecedor_DSC_PreRender(object sender, EventArgs e)
        {
            if (new ModoTelaEnum[] { ModoTelaEnum.InserirNovaPequenaDespesa, ModoTelaEnum.EditarPequenaDespesaSemComprovacaoExistente }.Contains(ModoTela))
                tseFornecedor_DSC.Visible = true;
            else
                tseFornecedor_DSC.Visible = false;
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
                pucVisualizarXML.ShowOnPageLoad = true;
                pucVisualizarXML.Width = Unit.Pixel(880);
                pucVisualizarXML.Height = Unit.Pixel(580);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lnkVisualizarNotaFiscal_DCC_Command(object sender, CommandEventArgs e)
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

        protected void lnkVisualizarComprovantePgto_DCC_Command(object sender, CommandEventArgs e)
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

        #region Botões de Comando

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                var tipoDespesa = ObtemTipoDespesa();

                switch (tipoDespesa)
                {
                    case 0:
                        ModoTela = ModoTelaEnum.EditarDespesaComumExistente;
                        ConsultaDespesaComumExistente();
                        break;

                    case 1:
                        ModoTela = ModoTelaEnum.EditarDespesaComumExistente;
                        ConsultaDespesaComumExistente();
                        break;

                    case 2:
                        ModoTela = ModoTelaEnum.EditarPequenaDespesaComComprovacaoExistente;
                        ConsultaPequenaDespesaComComprovacaoExistente();
                        break;

                    case 3:
                        ModoTela = ModoTelaEnum.EditarPequenaDespesaComTransladoExistente;
                        ConsultaPequenaDespesaComTransladoExistente();
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEditarDatas_Click(object sender, EventArgs e)
        {
            try
            {
                hdnEditaData.Value = "S";
                btnSalvar.Visible = true;
                btnEditarDatas.Visible = false;

                var tipoDespesa = ObtemTipoDespesa();

                switch (tipoDespesa)
                {
                    case 0:
                        txtDataPagamentoNF.Enabled = true;
                        txtDataPagamentoNF.ReadOnly = false;
                        txtDataNF.Enabled = true;
                        txtDataNF.ReadOnly = false;
                        break;

                    case 1:
                        txtDataPagamentoNF.Enabled = true;
                        txtDataPagamentoNF.ReadOnly = false;
                        txtDataNF.Enabled = true;
                        txtDataNF.ReadOnly = false;
                        break;

                    case 2:

                        txtDataNF_DCC.Enabled = true;
                        txtDataNF_DCC.ReadOnly = false;
                        txtDataPagamento_DCC.Enabled = true;
                        txtDataPagamento_DCC.ReadOnly = false;
                        break;

                    case 3:
                        txtDataPagamento_DCTS.Enabled = true;
                        txtDataPagamento_DCTS.ReadOnly = false;
                        break;                   
                }

                if (ModoTela == ModoTelaEnum.ConsultaPequenaDespesaSemComprovacaoExistente)
                {
                    txtDataPagamento_DSC.Enabled = true;
                }
                
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!tsePlanoTrabalho.DBValue.IsNull && tsePlanoTrabalho.IsValidDBValue &&
                    !tsePeriodoPrestacaoContas.DBValue.IsNull && tsePeriodoPrestacaoContas.IsValidDBValue &&
                    !tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue)
                {
                    RN.PrestacaoContas.Evento rnEvento = new Techne.Lyceum.RN.PrestacaoContas.Evento();
                  //  var verificaenviosei = rnEvento.VerificaEnvioSEIPorUnidade(tseUnidadeEnsino.DBValue.ToString(), Convert.ToInt32(tsePeriodoPrestacaoContas.DBValue));
                  //  if (verificaenviosei)
                   //     throw new Exception("Não é possível cadastrar esta despesa porque o Formulário SEI já foi gerado");

                    rblFiltroTipoEvento.ClearSelection();
                    rbFiltroTipoPequenaDespesa.ClearSelection();
                    rblDocumentoFiscal.ClearSelection();

                    ModoTela = ModoTelaEnum.InserirNovo;
                    tseEvento.DBValue = DBNull.Value;
                   
                }
                else
                {
                    lblMensagem.Text = "Selecione a UNIDADE DE ENSINO, PROJETO / PROGRAMA e PERÍODO DA PRESTAÇÃO DE CONTAS para cadastrar uma nova despesa.";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaModal()
        {
            ddlModalTransporte_DCTS.Items.Clear();
            ddlModalTransporte_DCTS.Items.Add(new ListItem { Text = "", Value = "" });
            ddlModalTransporte_DCTS.DataSource = ListaTipoTransporte();
            ddlModalTransporte_DCTS.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (new ModoTelaEnum[] 
            { 
                ModoTelaEnum.InserirNovo, 
                ModoTelaEnum.InserirNovaDespesaComum, 
                ModoTelaEnum.InserirNovaPequenaDespesa,
                ModoTelaEnum.InserirNovaPequenaDespesaComComprovacao,
                ModoTelaEnum.InserirNovaPequenaDespesaSemComprovacao,
                ModoTelaEnum.InserirNovaPequenaDespesaComTranslado
            }.Contains(ModoTela))
                {
                    ModoTela = ModoTelaEnum.ConsultaVazio;
                }
                else if (ModoTela == ModoTelaEnum.EditarDespesaComumExistente)
                {
                    ModoTela = ModoTelaEnum.ConsultaDespesaComumExistente;
                    ConsultaDespesaComumExistente();
                }
                else if (ModoTela == ModoTelaEnum.EditarPequenaDespesaComComprovacaoExistente)
                {
                    ModoTela = ModoTelaEnum.ConsultaPequenaDespesaComComprovacaoExistente;
                    ConsultaPequenaDespesaComComprovacaoExistente();
                }
                else if (ModoTela == ModoTelaEnum.EditarPequenaDespesaSemComprovacaoExistente)
                {
                    ModoTela = ModoTelaEnum.ConsultaPequenaDespesaSemComprovacaoExistente;
                    ConsultaPequenaDespesaSemComprovacaoExistente();
                }
                else if (ModoTela == ModoTelaEnum.EditarPequenaDespesaComTransladoExistente)
                {
                    ModoTela = ModoTelaEnum.ConsultaPequenaDespesaComTransladoExistente;
                    ConsultaPequenaDespesaComTransladoExistente();
                }
                else
                {
                    ModoTela = ModoTelaEnum.ConsultaVazio;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                if (rblFiltroTipoEvento.SelectedValue == "0" && rblDocumentoFiscal.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "O TIPO DE DOCUMENTO FISCAL é de preenchimento obrigatório.";
                    return;
                }

                // VERIFICA SE A DATA ATUAL PASSOU A DATA LIMITE
                if (tsePeriodoPrestacaoContas["DATALIMITEDESPESAS"] != DBNull.Value)
                {
                    DateTime dataLimite = Convert.ToDateTime(tsePeriodoPrestacaoContas["DATALIMITEDESPESAS"]);

                    if (dataLimite != null && DateTime.Now > dataLimite)
                    {
                        lblMensagem.Text = "A DATA ATUAL passou a DATA LIMITE DE DESPESAS.";
                        return;
                    }
                }

                RN.PrestacaoContas.Evento rnEvento = new Techne.Lyceum.RN.PrestacaoContas.Evento();
              //  var verificaenviosei = rnEvento.VerificaEnvioSEIPorUnidade(tseUnidadeEnsino.DBValue.ToString(), Convert.ToInt32(tsePeriodoPrestacaoContas.DBValue));
              //  if (verificaenviosei)
              //      throw new Exception("Não é possível cadastrar esta despesa porque o Formulário SEI já foi gerado");
   
                var tipoDespesa = ObtemTipoDespesa();

                if (hdnEditaData.Value.IsNullOrEmptyOrWhiteSpace())
                {

                    switch (tipoDespesa)
                    {
                        case -1:
                            validacao.Mensagem = "Tipo de despesa é obrigatório.";
                            validacao.Valido = false;
                            break;
                        case 0:
                            validacao = SalvaDespesaComum();
                            break;
                        case 1:
                            {
                                if (!rnEvento.VerificaPeriodoPagamento(tsePeriodoPrestacaoContas.Value.ToString(), txtDataPagamentoNF.Text))
                                {
                                    lblMensagem.Text = "Período escolhido fora da data de pagamento.";
                                    return;
                                }
                                validacao = SalvaDespesaDocumentoFiscal();
                                break;
                            }

                        case 2:
                            validacao = SalvaPequenaDespesaComComprovacao();
                            break;

                        case 3:
                            validacao = SalvaPequenaDespesaComTranslado();
                            break;
                    }

                    if (!validacao.Valido)
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                    else
                    {
                        lblMensagem.Text = "Despesa salva com sucesso.";
                    }

                    if (tseEvento.DBValue.IsNull && !hdnEventoId.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        tseEvento.DBValue = hdnEventoId.Value;
                    }

                    tseEvento_Changed(sender, e);
                }
                else
                {                               
                    DateTime? dataNF = new DateTime();
                    DateTime dataPgto = new DateTime();

                    switch (tipoDespesa)
                    {
                        
                        case 0:
                            dataNF = !txtDataNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataNF.Text) : (DateTime?)null;
                            dataPgto = !txtDataPagamentoNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataPagamentoNF.Text) : DateTime.MinValue;
                            break;
                        case 1:
                            {
                                dataNF = !txtDataNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataNF.Text) : (DateTime?)null;
                                dataPgto = !txtDataPagamentoNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataPagamentoNF.Text) : DateTime.MinValue;
                                break;
                            }

                        case 2:

                            dataNF = !txtDataNF_DCC.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataNF_DCC.Text) : (DateTime?)null;
                            dataPgto = !txtDataPagamento_DCC.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataPagamento_DCC.Text) : DateTime.MinValue; 

                            
                            break;

                        case 3:                           

                            dataPgto = !txtDataPagamento_DCTS.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataPagamento_DCTS.Text) : DateTime.MinValue;

                            break;
                    }

                    if (ModoTela == ModoTelaEnum.ConsultaPequenaDespesaSemComprovacaoExistente)
                    {
                        dataPgto = !txtDataPagamento_DSC.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataPagamento_DSC.Text) : DateTime.MinValue;
                        
                    }

                  
                    validacao = rnEvento.ValidaDatas(Convert.ToInt32(tseEvento.DBValue), tipoDespesa, dataNF, dataPgto,Convert.ToInt32(tsePeriodoPrestacaoContas.DBValue), User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnEvento.AtualizaDatas(Convert.ToInt32(tseEvento.DBValue), dataNF, dataPgto, User.Identity.Name);
                        lblMensagem.Text = "Data(s) atualizada(s) com sucesso.";
                        btnEditarDatas.Visible = true;
                        btnSalvar.Visible = false;
                        tseEvento.Enabled = true;

                        txtDataNF.ReadOnly = true;
                        txtDataPagamentoNF.ReadOnly = true;
                        txtDataNF_DCC.ReadOnly = true;
                        txtDataPagamento_DCC.ReadOnly = true;
                        txtDataPagamento_DSC.ReadOnly = true;
                        txtDataPagamento_DCTS.ReadOnly = true;

                        txtDataNF.Enabled = false;
                        txtDataPagamentoNF.Enabled = false;
                        txtDataNF_DCC.Enabled = false;
                        txtDataPagamento_DCC.Enabled = false;
                        txtDataPagamento_DSC.Enabled = false;
                        txtDataPagamento_DCTS.Enabled = false;
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnDeletar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                int eventoId = 0;

                eventoId = Convert.ToInt32(tseEvento.Value);

                RN.PrestacaoContas.Evento rnEvento = new Techne.Lyceum.RN.PrestacaoContas.Evento();
                var verificaenviosei = rnEvento.VerificaEnvioSEIPorUnidade(tseUnidadeEnsino.DBValue.ToString(), Convert.ToInt32(tsePeriodoPrestacaoContas.DBValue));
                if (verificaenviosei)
                    throw new Exception("Não é possível excluir esta despesa porque o Formulário SEI já foi gerado");

                validacao = rnEvento.ValidaRemocao(eventoId);

                if (validacao.Valido)
                {
                    rnEvento.Remove(eventoId);
                    if (FiltraEvento())
                        ModoTela = ModoTelaEnum.ConsultaVazio;
                    else
                        ModoTela = ModoTelaEnum.FiltroVazio;


                    lblMensagem.Text = "Despesa excluída com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEnviarEvento_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var evento = new DadosEvento();

                evento.EventoId = Convert.ToInt32(tseEvento.Value);
                evento.PlanoTrabalhoId = Convert.ToInt32(tsePlanoTrabalho.Value);
                evento.TipoDespesa = ObtemTipoDespesa(); //rblFiltroTipoEvento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? -1 : Convert.ToInt32(rblFiltroTipoEvento.SelectedValue);
                evento.UsuarioId = User.Identity.Name;

                validacao = rnEvento.ValidaEnvioAnalise(evento);

                if (validacao.Valido)
                {
                    rnEvento.EnviaAnalise(evento);
                    tseEvento_Changed(sender, e);
                }
                else
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Grid de Exigências

        public DataTable ListaExigenciaEvento(int eventoId)
        {
            return rnExigenciaEvento.ListaExigenciasPor(eventoId);
        }

        public void UpdateExigenciaEvento(object JUSTIFICATIVA, object EXIGENCIAEVENTOID)
        {
        }


        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            try
            {
                hdnExigenciaEventoId.Value = string.Empty;
                txtJustificativaExigencia.Text = string.Empty;
                txtValorRessarcimento.Text = string.Empty;
                dtDataRessarcimento.Text = string.Empty;

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });
                hdnExigenciaEventoId.Value = chave[0].ToString();

                //Verifica se a exigencia necessario ressarcmento
                if (rnExigenciaEvento.NecessitaRessarcimentoPor(Convert.ToInt32(hdnExigenciaEventoId.Value)))
                {
                    lblValorRessarcimento.Visible = true;
                    txtValorRessarcimento.Visible = true;
                    lblDataRessarcimento.Visible = true;
                    dtDataRessarcimento.Visible = true;
                }
                else
                {
                    lblValorRessarcimento.Visible = false;
                    txtValorRessarcimento.Visible = false;
                    lblDataRessarcimento.Visible = false;
                    dtDataRessarcimento.Visible = false;
                }

                pucConfirmarArquivo.ShowOnPageLoad = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.PrestacaoContas.Entidades.ExigenciaEvento exigenciaEvento = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ExigenciaEvento();
                RN.PrestacaoContas.Entidades.ExigenciaEventoArquivo exigenciaEventoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ExigenciaEventoArquivo();
                ValidacaoDados validacao = new ValidacaoDados();
                ValidacaoDados validacaoArquivo = new ValidacaoDados();

                byte[] imageBytes = new byte[FileUpload1.PostedFile.InputStream.Length + 1];
                FileUpload1.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);

                exigenciaEventoArquivo.ExigenciaEventoId = !hdnExigenciaEventoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnExigenciaEventoId.Value) : -1;
                exigenciaEventoArquivo.ChaveArquivo = Guid.NewGuid().ToString();
                exigenciaEventoArquivo.Arquivo = imageBytes;
                exigenciaEventoArquivo.NomeArquivo = FileUpload1.PostedFile.FileName;
                exigenciaEventoArquivo.TipoArquivo = FileUpload1.PostedFile.ContentType;
                exigenciaEventoArquivo.UsuarioId = User.Identity.Name;
                exigenciaEventoArquivo.DataCadastro = DateTime.Now;
                exigenciaEventoArquivo.DataAlteracao = DateTime.Now;

                string censo = tseUnidadeEnsino.DBValue.IsNull || !tseUnidadeEnsino.IsValidDBValue ? string.Empty : Convert.ToString(tseUnidadeEnsino.Value);
                int planoTrabalhoId = tsePlanoTrabalho.DBValue.IsNull || !tsePlanoTrabalho.IsValidDBValue ? 0 : Convert.ToInt32(tsePlanoTrabalho.Value);
                string justificativa = txtJustificativaExigencia.Text;
                decimal? valorRessarcimento = !txtValorRessarcimento.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorRessarcimento.Text) : (decimal?)null;
                DateTime? dataRessarcimento = !dtDataRessarcimento.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataRessarcimento.Date : (DateTime?)null;

                validacao = rnExigenciaEvento.ValidaCorrigeExigencia(censo, planoTrabalhoId, justificativa, valorRessarcimento, dataRessarcimento, exigenciaEventoArquivo);

                if (validacao.Valido)
                {
                    rnExigenciaEvento.CorrigeExigencia(censo, planoTrabalhoId, justificativa, valorRessarcimento, dataRessarcimento, exigenciaEventoArquivo);
                    lblMensagem.Text = "Arquivo atualizado com sucesso.";
                    grdExigenciaEvento.DataBind();
                    pucConfirmarArquivo.ShowOnPageLoad = false;
                }
                else
                {
                    lblMensagemImportarArquivo.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    pucConfirmarArquivo.ShowOnPageLoad = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
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

        #endregion

        #region Grid XML

        public DataTable ListaXML(int eventoId, string censo)
        {
            return rnImportacaoXmlEvento.ListaItensXmlPor(eventoId, censo);
        }

        #endregion

        #region Métodos auxiliares

        private bool FiltraEvento()
        {
            tseEvento.SqlWhere = " ";
            tseEvento.Value = null;

            if (tseUnidadeEnsino.DBValue.IsNull || !tseUnidadeEnsino.IsValidDBValue)
                return false;

            if (tsePlanoTrabalho.DBValue.IsNull || !tsePlanoTrabalho.IsValidDBValue)
                return false;

            if (tsePeriodoPrestacaoContas.DBValue.IsNull || !tsePeriodoPrestacaoContas.IsValidDBValue)
                return false;

            var where = string.Format(@"
                    PLANOTRABALHOID = {0}
                    AND CENSO = '{1}'                    
                    AND PERIODOREFERENCIAID = {2}
                    
                "
            , tsePlanoTrabalho.Value ?? "0"
            , tseUnidadeEnsino.Value ?? "0"
            , tsePeriodoPrestacaoContas.Value ?? "0"
            );

            tseEvento.SqlWhere = where;
            tseEvento.Value = null;

            return true;
        }

        private void ZerarCamposDespesaComum()
        {
            try
            {
                txtDescricao.Text = string.Empty;

                lnkVisualizarOrcamento1.Visible = false;
                hidOrcamento1.Value = string.Empty;
                lnkVisualizarOrcamento2.Visible = false;
                hidOrcamento2.Value = string.Empty;
                lnkVisualizarOrcamento3.Visible = false;
                hidOrcamento3.Value = string.Empty;
                txtJustificativa.Text = string.Empty;
                txtValorRessarcimento.Text = string.Empty;
                dtDataRessarcimento.Text = string.Empty;

                tseFornecedor.ResetValue();
                lblFornecedor.Text = string.Empty;

                rblDocumentoFiscal.ClearSelection();
                pnlTipoDocumentoFiscal.Enabled = true;
                txtChaveAcesso.Text = string.Empty;
                txtNumeroNF.Text = string.Empty;
                txtValorTotalNF.Text = string.Empty;
                txtDataNF.Text = string.Empty;
                txtValorPagoNF.Text = string.Empty;
                txtDataPagamentoNF.Text = string.Empty;
                lnkVisualizarNotaFiscal.Visible = false;
                hidNotaFiscal.Value = string.Empty;
                lnkVisualizarComprovantePgto.Visible = false;
                hidComprovantePgto.Value = string.Empty;
                lnkVisualizarXML.Visible = false;
                lnkVisualizarXML.Text = "XML inserido";
                hidXML.Value = string.Empty;

                txtObservacao.Text = string.Empty;
                lnkVisualizarEvidencia.Visible = false;
                hidEvidencia.Value = string.Empty;
                tbChaveAcesso.Visible = true;
                tbXML.Visible = true;
            }
            catch
            {
                throw;
            }
        }

        private void ZerarCamposPequenaDespesaComComprovacao()
        {
            try
            {
                txtDescricao.Text = string.Empty;

                tseFornecedor_DCC.ResetValue();
                lblFornecedor_DCC.Text = string.Empty;

                txtNumeroNF_DCC.Text = string.Empty;
                txtValorNF_DCC.Text = string.Empty;
                txtValorPagoNF_DCC.Text = string.Empty;
                txtDataNF_DCC.Text = string.Empty;
                txtDataPagamento_DCC.Text = string.Empty;
                ddlFormaPagamento_DCC.SelectedIndex = -1;
                lnkVisualizarNotaFiscal_DCC.Visible = false;
                hidNotaFiscal_DCC.Value = string.Empty;
                lnkVisualizarComprovantePgto_DCC.Visible = false;
                hidComprovantePgto_DCC.Value = string.Empty;
            }
            catch
            {
                throw;
            }
        }

        private void ZerarCamposPequenaDespesaSemComprovacao()
        {
            try
            {
                txtDescricao.Text = string.Empty;

                tseFornecedor_DCC.ResetValue();
                lblFornecedor_DCC.Text = string.Empty;

                txtValorPago_DSC.Text = string.Empty;
                txtDataPagamento_DSC.Text = string.Empty;
                ddlFormaPagamento_DSC.SelectedIndex = -1;
                txtJustificativa_DSC.Text = string.Empty;
            }
            catch
            {
                throw;
            }
        }

        private void ZerarCamposPequenaDespesaComTranslado()
        {
            try
            {
                txtDescricao.Text = string.Empty;

                ddlModalTransporte_DCTS.SelectedIndex = -1;
                txtOrigem_DCTS.Text = string.Empty;
                txtDestino_DCTS.Text = string.Empty;
                txtValorPago_DCTS.Text = string.Empty;
                txtDataPagamento_DCTS.Text = string.Empty;
                txtJustificativa_DCTS.Text = string.Empty;
            }
            catch
            {
                throw;
            }
        }

        private void ConsultaDespesaComumExistente()
        {
             ExigenciaEvento rnExigenciaEvento = new ExigenciaEvento();

            try
            {
                if (tseEvento.DBValue.IsNull || !tseEvento.IsValidDBValue)
                    return;

                var evento = rnEvento.ObtemDadosEventoPor(Convert.ToInt32(tseEvento.Value));
                var pequenaDespesa = rnPequenaDespesa.ObtemPor(Convert.ToInt32(tseEvento.Value));

                hidPequenaDespesaId.Value = "";
                txtDescricao.Text = evento.Descricao;

                rblFiltroTipoEvento.SelectedValue = "0";
                pnlTipoDocumentoFiscal.Visible = true;
                pnlTipoPequenaDespesa.Visible = false;

                if (evento.TipoDespesa == 1)
                {
                    rblDocumentoFiscal.SelectedValue = "1";
                }
                else
                {
                    rblDocumentoFiscal.SelectedValue = "0";
                }

                rblDocumentoFiscal_SelectedIndexChanged(null, null);

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

                var fornecedor = rnFornecedor.ObtemPor(evento.FornecedorId ?? 0);
                if (fornecedor != null)
                {
                    lblFornecedor.Text = fornecedor.FornecedorId + " - " + fornecedor.RazaoSocial;
                    tseFornecedor.DBValue = Convert.ToString(evento.FornecedorId);
                }
                else
                    lblFornecedor.Text = "";

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

                btnEnviarEvento.Visible = (evento.StatusAnalise == "Aberto");
                lblStatusEvento.Visible = (evento.StatusAnalise != "Aberto");
                lblStatusEvento.Text = evento.StatusAnalise;
                btnEditar.Visible = btnEnviarEvento.Visible;

                if (!btnEditar.Visible && rnExigenciaEvento.PossuiEventoExigenciaAbertaPor(evento.EventoId))
                {
                    //Verificar se é despesa em analise com exigencia aberta
                    btnEditarDatas.Visible = true;
                }
                else
                {
                    btnEditarDatas.Visible = false;
                }

                btnDeletar.Visible = btnEnviarEvento.Visible;

                grdXML.DataBind();
                grdItensNf.DataBind();
                grdExigenciaEvento.DataBind();
                grdServidores_DCTS.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ConsultaPequenaDespesaComComprovacaoExistente()
        {
            ExigenciaEvento rnExigenciaEvento = new ExigenciaEvento();

            try
            {
                if (!tseEvento.IsValidDBValue)
                    return;

                var evento = rnEvento.ObtemDadosEventoPor(Convert.ToInt32(tseEvento.Value));
                var pequenaDespesa = rnPequenaDespesa.ObtemPor(Convert.ToInt32(tseEvento.Value));

                hidPequenaDespesaId.Value = pequenaDespesa != null ? pequenaDespesa.PequenaDespesaId.ToString() : "";
                txtDescricao.Text = evento.Descricao;

                rblFiltroTipoEvento.SelectedValue = "1";
                pnlTipoDocumentoFiscal.Visible = false;
                pnlTipoPequenaDespesa.Visible = true;

                if (pequenaDespesa.TipoDespesa == "PEQUENADESPESA")
                {
                    rbFiltroTipoPequenaDespesa.SelectedValue = "0";
                }

                var fornecedor = rnFornecedor.ObtemPor(evento.FornecedorId ?? 0);
                if (fornecedor != null)
                {
                    lblFornecedor_DCC.Text = fornecedor.FornecedorId + " - " + fornecedor.RazaoSocial;
                    tseFornecedor_DCC.DBValue = Convert.ToString(evento.FornecedorId);
                }
                else
                    lblFornecedor_DCC.Text = "";

                txtNumeroNF_DCC.Text = evento.NumeroNotaFiscal;
                txtValorNF_DCC.Text = evento.ValorNotaFiscal.HasValue ? evento.ValorNotaFiscal.Value.ToString("N2") : "";
                txtValorPagoNF_DCC.Text = evento.ValorPagamento.ToString("N2");
                txtDataNF_DCC.Text = evento.DataNotaFiscal.HasValue ? evento.DataNotaFiscal.Value.ToString("dd/MM/yyyy") : "";
                txtDataPagamento_DCC.Text = evento.DataPagamento.HasValue ? evento.DataPagamento.Value.ToString("dd/MM/yyyy") : "";
                ddlFormaPagamento_DCC.SelectedValue = evento.FormaPagamento;
                lnkVisualizarNotaFiscal_DCC.Visible = evento.NotaFiscalArquivoId.HasValue;
                lnkVisualizarNotaFiscal_DCC.CommandArgument = evento.NotaFiscalArquivoId.ToString() + "," + (!evento.NotaFiscalTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.NotaFiscalTipoArquivo : "");
                lnkVisualizarComprovantePgto_DCC.Visible = evento.ComprovantePagamentoArquivoId.HasValue;
                lnkVisualizarComprovantePgto_DCC.CommandArgument = evento.ComprovantePagamentoArquivoId.ToString() + "," + (!evento.ComprovantePagamentoTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.ComprovantePagamentoTipoArquivo : "");

                btnEnviarEvento.Visible = (evento.StatusAnalise == "Aberto");
                lblStatusEvento.Visible = (evento.StatusAnalise != "Aberto");
                lblStatusEvento.Text = evento.StatusAnalise;
                btnEditar.Visible = btnEnviarEvento.Visible;

                if (!btnEditar.Visible && rnExigenciaEvento.PossuiEventoExigenciaAbertaPor(evento.EventoId))
                {
                    //Verificar se é despesa em analise com exigencia aberta
                    btnEditarDatas.Visible = true;
                }
                else
                {
                    btnEditarDatas.Visible = false;
                }

                btnDeletar.Visible = btnEnviarEvento.Visible;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ConsultaPequenaDespesaSemComprovacaoExistente()
        {
            ExigenciaEvento rnExigenciaEvento = new ExigenciaEvento();

            try
            {
                if (!tseEvento.IsValidDBValue)
                    return;

                var evento = rnEvento.ObtemDadosEventoPor(Convert.ToInt32(tseEvento.Value));
                var pequenaDespesa = rnPequenaDespesa.ObtemPor(Convert.ToInt32(tseEvento.Value));

                rblFiltroTipoEvento.SelectedValue = "1";
                pnlTipoDocumentoFiscal.Visible = false;
                pnlTipoPequenaDespesa.Visible = true;

                rbFiltroTipoPequenaDespesa.SelectedValue = "0";

                hidPequenaDespesaId.Value = pequenaDespesa != null ? pequenaDespesa.PequenaDespesaId.ToString() : "";
                txtDescricao.Text = evento.Descricao;

                var fornecedor = rnFornecedor.ObtemPor(evento.FornecedorId ?? 0);
                if (fornecedor != null)
                {
                    lblFornecedor_DSC.Text = fornecedor.FornecedorId + " - " + fornecedor.RazaoSocial;
                    tseFornecedor_DSC.DBValue = Convert.ToString(evento.FornecedorId);
                }
                else
                    lblFornecedor_DSC.Text = "";

                txtValorPago_DSC.Text = evento.ValorPagamento.ToString("N2");
                txtDataPagamento_DSC.Text = evento.DataPagamento.HasValue ? evento.DataPagamento.Value.ToString("dd/MM/yyyy") : "";
                ddlFormaPagamento_DSC.SelectedValue = evento.FormaPagamento;
                txtJustificativa_DSC.Text = evento.Justificativa;

                btnEnviarEvento.Visible = (evento.StatusAnalise == "Aberto");
                lblStatusEvento.Visible = (evento.StatusAnalise != "Aberto");
                lblStatusEvento.Text = evento.StatusAnalise;
                btnEditar.Visible = btnEnviarEvento.Visible;

                if (!btnEditar.Visible && rnExigenciaEvento.PossuiEventoExigenciaAbertaPor(evento.EventoId))
                {
                    //Verificar se é despesa em analise com exigencia aberta
                    btnEditarDatas.Visible = true;
                }
                else
                {
                    btnEditarDatas.Visible = false;
                }

                btnDeletar.Visible = btnEnviarEvento.Visible;
            }
            catch
            {
                throw;
            }
        }

        private void ConsultaPequenaDespesaComTransladoExistente()
        {
            ExigenciaEvento rnExigenciaEvento = new ExigenciaEvento();

            try
            {
                if (!tseEvento.IsValidDBValue)
                    return;

                var evento = rnEvento.ObtemDadosEventoPor(Convert.ToInt32(tseEvento.Value));
                var pequenaDespesa = rnPequenaDespesa.ObtemPor(Convert.ToInt32(tseEvento.Value));

                hidPequenaDespesaId.Value = pequenaDespesa != null ? pequenaDespesa.PequenaDespesaId.ToString() : "";
                txtDescricao.Text = evento.Descricao;

                rblFiltroTipoEvento.SelectedValue = "1";

                pnlTipoDocumentoFiscal.Visible = false;
                pnlTipoPequenaDespesa.Visible = true;
                if (pequenaDespesa.TipoDespesa == "TRANSPORTE")
                {
                    rbFiltroTipoPequenaDespesa.SelectedValue = "2";
                }

                ddlModalTransporte_DCTS.SelectedValue = evento.TipoTransporteId.HasValue && evento.TipoTransporteId.Value > 0 ? evento.TipoTransporteId.Value.ToString() : "";
                txtOrigem_DCTS.Text = evento.Origem;
                txtDestino_DCTS.Text = evento.Destino;
                txtValorPago_DCTS.Text = evento.ValorPagamento.ToString("N2");
                txtDataPagamento_DCTS.Text = evento.DataPagamento.HasValue ? evento.DataPagamento.Value.ToString("dd/MM/yyyy") : "";
                txtJustificativa_DCTS.Text = evento.Justificativa;
                Servidores_DCTS = evento.Servidores;

                grdServidores_DCTS.DataSource = evento.Servidores;
                grdServidores_DCTS.DataBind();

                btnEnviarEvento.Visible = (evento.StatusAnalise == "Aberto");
                lblStatusEvento.Visible = (evento.StatusAnalise != "Aberto");
                lblStatusEvento.Text = evento.StatusAnalise;
                btnEditar.Visible = btnEnviarEvento.Visible;

                if (!btnEditar.Visible && rnExigenciaEvento.PossuiEventoExigenciaAbertaPor(evento.EventoId))
                {
                    //Verificar se é despesa em analise com exigencia aberta
                    btnEditarDatas.Visible = true;
                }
                else
                {
                    btnEditarDatas.Visible = false;
                }

                btnDeletar.Visible = btnEnviarEvento.Visible;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ValidacaoDados Salva(DadosEvento dados, DadosArquivoXml arquivoXml)
        {
            try
            {
                var cadastro = dados.EventoId == 0;
                hdnEventoId.Value = string.Empty;
                var validacao = rnEvento.Valida(dados, arquivoXml, cadastro);

                if (validacao.Valido)
                    if (cadastro)
                    {
                        rnEvento.Insere(dados, arquivoXml);

                        hdnEventoId.Value = dados.EventoId.ToString();
                    }
                    else
                        rnEvento.Atualiza(dados, arquivoXml);

                return validacao;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ValidacaoDados SalvaDespesaComum()
        {
            try
            {
                var evento = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosEvento();
                var arquivoXml = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosArquivoXml();
                ValidacaoDados validacao = new ValidacaoDados();
                string msgData = string.Empty;
                int tipoDespesa = -1;

                if (!tseEvento.DBValue.IsNull && tseEvento.IsValidDBValue)
                    evento = rnEvento.ObtemDadosEventoPor(Convert.ToInt32(tseEvento.Value));
                
                evento.PlanoTrabalhoId = Convert.ToInt32(tsePlanoTrabalho.Value);
                evento.Censo = Convert.ToString(tseUnidadeEnsino.Value);

                tipoDespesa = (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum;
                evento.TipoDespesa = tipoDespesa;
                //!rblDocumentoFiscal.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(rblDocumentoFiscal.SelectedValue) : -1;

                evento.Descricao = !txtDescricao.Text.IsNullOrEmptyOrWhiteSpace() ? txtDescricao.Text.ToUpper() : null;

                if (hidOrcamento1.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidOrcamento1.Value);
                    evento.Orcamento1Arquivo = dataUrlParts.ByteArray;
                    evento.Orcamento1NomeArquivo = dataUrlParts.Name;
                    evento.Orcamento1TipoArquivo = dataUrlParts.MimeType;
                }

                if (hidOrcamento2.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidOrcamento2.Value);
                    evento.Orcamento2Arquivo = dataUrlParts.ByteArray;
                    evento.Orcamento2NomeArquivo = dataUrlParts.Name;
                    evento.Orcamento2TipoArquivo = dataUrlParts.MimeType;
                }

                if (hidOrcamento3.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidOrcamento3.Value);
                    evento.Orcamento3Arquivo = dataUrlParts.ByteArray;
                    evento.Orcamento3NomeArquivo = dataUrlParts.Name;
                    evento.Orcamento3TipoArquivo = dataUrlParts.MimeType;
                }

                evento.JustificativaOrcamento = !txtJustificativa.Text.IsNullOrEmptyOrWhiteSpace() ? txtJustificativa.Text : null;

                if (tseFornecedor.IsValidDBValue)
                    evento.FornecedorId = Convert.ToInt32(tseFornecedor.Value);

                evento.ChaveAcesso = !txtChaveAcesso.Text.IsNullOrEmptyOrWhiteSpace() ? txtChaveAcesso.Text : null;
                evento.NumeroNotaFiscal = !txtNumeroNF.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumeroNF.Text : null;
                evento.ValorNotaFiscal = !txtValorTotalNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorTotalNF.Text) : (decimal?)null;
                evento.ValorPagamento = !txtValorPagoNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorPagoNF.Text) : 0;
                evento.DataPagamento = !txtDataPagamentoNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataPagamentoNF.Text) : (DateTime?)null;
                evento.DataNotaFiscal = !txtDataNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataNF.Text) : (DateTime?)null;


                if (hidNotaFiscal.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidNotaFiscal.Value);
                    evento.NotaFiscalArquivo = dataUrlParts.ByteArray;
                    evento.NotaFiscalNomeArquivo = dataUrlParts.Name;
                    evento.NotaFiscalTipoArquivo = dataUrlParts.MimeType;
                }

                if (hidComprovantePgto.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidComprovantePgto.Value);
                    evento.ComprovantePagamentoArquivo = dataUrlParts.ByteArray;
                    evento.ComprovantePagamentoNomeArquivo = dataUrlParts.Name;
                    evento.ComprovantePagamentoTipoArquivo = dataUrlParts.MimeType;
                }

                if (rblDocumentoFiscal.SelectedValue == "0") //NF-e
                {
                    if (hidXML.Value.Length > 0)
                    {
                        var dataUrlParts = SplitDataURL(hidXML.Value);
                        arquivoXml.ArquivoXml = dataUrlParts.Stream;
                        arquivoXml.NomeArquivo = dataUrlParts.Name;
                        arquivoXml.TipoArquivo = dataUrlParts.MimeType;
                    }
                }

                evento.Observacoes = !txtObservacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtObservacao.Text : null;

                if (hidEvidencia.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidEvidencia.Value);
                    evento.EvidenciaArquivo = dataUrlParts.ByteArray;
                    evento.EvidenciaNomeArquivo = dataUrlParts.Name;
                    evento.EvidenciaTipoArquivo = dataUrlParts.MimeType;
                }

                evento.UsuarioId = User.Identity.Name;

                validacao = Salva(evento, arquivoXml);

                return validacao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ValidacaoDados SalvaDespesaDocumentoFiscal()
        {
            try
            {
                var evento = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosEvento();
                var arquivoXml = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosArquivoXml();
                ValidacaoDados validacao = new ValidacaoDados();
                string msgData = string.Empty;
                int tipoDespesa = -1;

                if (!tseEvento.DBValue.IsNull && tseEvento.IsValidDBValue)
                    evento = rnEvento.ObtemDadosEventoPor(Convert.ToInt32(tseEvento.Value));

                evento.PlanoTrabalhoId = Convert.ToInt32(tsePlanoTrabalho.Value);
                evento.Censo = Convert.ToString(tseUnidadeEnsino.Value);
                tipoDespesa = (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais;

                evento.TipoDespesa = tipoDespesa;
                //!rblDocumentoFiscal.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(rblDocumentoFiscal.SelectedValue) : -1;

                evento.Descricao = !txtDescricao.Text.IsNullOrEmptyOrWhiteSpace() ? txtDescricao.Text.ToUpper() : null;

                if (hidOrcamento1.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidOrcamento1.Value);
                    evento.Orcamento1Arquivo = dataUrlParts.ByteArray;
                    evento.Orcamento1NomeArquivo = dataUrlParts.Name;
                    evento.Orcamento1TipoArquivo = dataUrlParts.MimeType;
                }

                if (hidOrcamento2.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidOrcamento2.Value);
                    evento.Orcamento2Arquivo = dataUrlParts.ByteArray;
                    evento.Orcamento2NomeArquivo = dataUrlParts.Name;
                    evento.Orcamento2TipoArquivo = dataUrlParts.MimeType;
                }

                if (hidOrcamento3.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidOrcamento3.Value);
                    evento.Orcamento3Arquivo = dataUrlParts.ByteArray;
                    evento.Orcamento3NomeArquivo = dataUrlParts.Name;
                    evento.Orcamento3TipoArquivo = dataUrlParts.MimeType;
                }

                evento.JustificativaOrcamento = !txtJustificativa.Text.IsNullOrEmptyOrWhiteSpace() ? txtJustificativa.Text : null;

                if (tseFornecedor.IsValidDBValue)
                    evento.FornecedorId = Convert.ToInt32(tseFornecedor.Value);

                evento.ChaveAcesso = !txtChaveAcesso.Text.IsNullOrEmptyOrWhiteSpace() ? txtChaveAcesso.Text : null;
                evento.NumeroNotaFiscal = !txtNumeroNF.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumeroNF.Text : null;
                evento.ValorNotaFiscal = !txtValorTotalNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorTotalNF.Text) : (decimal?)null;
                evento.ValorPagamento = !txtValorPagoNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorPagoNF.Text) : 0;
                evento.DataPagamento = !txtDataPagamentoNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataPagamentoNF.Text) : (DateTime?)null;
                evento.DataNotaFiscal = !txtDataNF.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataNF.Text) : (DateTime?)null;


                if (hidNotaFiscal.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidNotaFiscal.Value);
                    evento.NotaFiscalArquivo = dataUrlParts.ByteArray;
                    evento.NotaFiscalNomeArquivo = dataUrlParts.Name;
                    evento.NotaFiscalTipoArquivo = dataUrlParts.MimeType;
                }

                if (hidComprovantePgto.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidComprovantePgto.Value);
                    evento.ComprovantePagamentoArquivo = dataUrlParts.ByteArray;
                    evento.ComprovantePagamentoNomeArquivo = dataUrlParts.Name;
                    evento.ComprovantePagamentoTipoArquivo = dataUrlParts.MimeType;
                }

                if (rblDocumentoFiscal.SelectedValue == "0") //NF-e
                {
                    if (hidXML.Value.Length > 0)
                    {
                        var dataUrlParts = SplitDataURL(hidXML.Value);
                        arquivoXml.ArquivoXml = dataUrlParts.Stream;
                        arquivoXml.NomeArquivo = dataUrlParts.Name;
                        arquivoXml.TipoArquivo = dataUrlParts.MimeType;
                    }
                }

                evento.Observacoes = !txtObservacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtObservacao.Text : null;

                if (hidEvidencia.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidEvidencia.Value);
                    evento.EvidenciaArquivo = dataUrlParts.ByteArray;
                    evento.EvidenciaNomeArquivo = dataUrlParts.Name;
                    evento.EvidenciaTipoArquivo = dataUrlParts.MimeType;
                }

                evento.UsuarioId = User.Identity.Name;

                validacao = Salva(evento, arquivoXml);

                return validacao;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ValidacaoDados SalvaPequenaDespesaComComprovacao()
        {
            try
            {
                var evento = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosEvento();
                string msgData = string.Empty;

                if (!tseEvento.DBValue.IsNull && tseEvento.IsValidDBValue)
                    evento = rnEvento.ObtemDadosEventoPor(Convert.ToInt32(tseEvento.Value));

                evento.PlanoTrabalhoId = Convert.ToInt32(tsePlanoTrabalho.Value);
                evento.Censo = Convert.ToString(tseUnidadeEnsino.Value);
                evento.TipoDespesa = (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao;

                evento.Descricao = txtDescricao.Text;

                if (tseFornecedor_DCC.IsValidDBValue)
                    evento.FornecedorId = Convert.ToInt32(tseFornecedor_DCC.Value);

                evento.NumeroNotaFiscal = txtNumeroNF_DCC.Text;
                evento.ValorNotaFiscal = !txtValorNF_DCC.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorNF_DCC.Text) : (decimal?)null;
                evento.ValorPagamento = !txtValorPagoNF_DCC.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorPagoNF_DCC.Text) : 0;
                evento.FormaPagamento = ddlFormaPagamento_DCC.SelectedValue;
                evento.DataNotaFiscal = !txtDataNF_DCC.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataNF_DCC.Text) : (DateTime?)null;
                evento.DataPagamento = !txtDataPagamento_DCC.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataPagamento_DCC.Text) : (DateTime?)null; ;

                if (hidNotaFiscal_DCC.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidNotaFiscal_DCC.Value);
                    evento.NotaFiscalArquivo = dataUrlParts.ByteArray;
                    evento.NotaFiscalNomeArquivo = dataUrlParts.Name;
                    evento.NotaFiscalTipoArquivo = dataUrlParts.MimeType;
                }

                if (hidComprovantePgto_DCC.Value.Length > 0)
                {
                    var dataUrlParts = SplitDataURL(hidComprovantePgto_DCC.Value);
                    evento.ComprovantePagamentoArquivo = dataUrlParts.ByteArray;
                    evento.ComprovantePagamentoNomeArquivo = dataUrlParts.Name;
                    evento.ComprovantePagamentoTipoArquivo = dataUrlParts.MimeType;
                }

                lnkVisualizarNotaFiscal_DCC.Visible = evento.NotaFiscalArquivoId.HasValue;
                lnkVisualizarNotaFiscal_DCC.CommandArgument = evento.NotaFiscalArquivoId.ToString() + "," + (!evento.NotaFiscalTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.NotaFiscalTipoArquivo : "");

                lnkVisualizarComprovantePgto_DCC.Visible = evento.ComprovantePagamentoArquivoId.HasValue;
                lnkVisualizarComprovantePgto_DCC.CommandArgument = evento.ComprovantePagamentoArquivoId.ToString() + "," + (!evento.ComprovantePagamentoTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.ComprovantePagamentoTipoArquivo : "");

                evento.UsuarioId = User.Identity.Name;

                return Salva(evento, new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosArquivoXml());
            }
            catch
            {
                throw;
            }
        }

        private ValidacaoDados SalvaPequenaDespesaSemComprovacao()
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                string msgData = string.Empty;

                var evento = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosEvento();

                if (!tseEvento.DBValue.IsNull && tseEvento.IsValidDBValue)
                    evento = rnEvento.ObtemDadosEventoPor(Convert.ToInt32(tseEvento.Value));
      
                evento.PlanoTrabalhoId = Convert.ToInt32(tsePlanoTrabalho.Value);
                evento.Censo = Convert.ToString(tseUnidadeEnsino.Value);
                evento.TipoDespesa = (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaSemComprovacao;

                evento.Descricao = txtDescricao.Text;

                if (tseFornecedor_DSC.IsValidDBValue)
                    evento.FornecedorId = Convert.ToInt32(tseFornecedor_DSC.Value);

                evento.ValorPagamento = !txtValorPago_DSC.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorPago_DSC.Text) : 0;
                evento.DataPagamento = !txtDataPagamento_DSC.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataPagamento_DSC.Text) : (DateTime?)null;
                evento.FormaPagamento = ddlFormaPagamento_DSC.SelectedValue;
                evento.Justificativa = txtJustificativa_DSC.Text;

                evento.UsuarioId = User.Identity.Name;

                validacao = Salva(evento, new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosArquivoXml());

                if (!msgData.IsNullOrEmptyOrWhiteSpace())
                {
                    validacao.Mensagem += msgData;
                }

                return validacao;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ValidacaoDados SalvaPequenaDespesaComTranslado()
        {
            try
            {
                var evento = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosEvento();
                string msgData = string.Empty;

                if (!tseEvento.DBValue.IsNull && tseEvento.IsValidDBValue)
                    evento = rnEvento.ObtemDadosEventoPor(Convert.ToInt32(tseEvento.Value));

                evento.PlanoTrabalhoId = Convert.ToInt32(tsePlanoTrabalho.Value);
                evento.Censo = Convert.ToString(tseUnidadeEnsino.Value);
                evento.TipoDespesa = (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores;

                evento.Descricao = txtDescricao.Text;

                evento.TipoTransporteId = !ddlModalTransporte_DCTS.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlModalTransporte_DCTS.SelectedValue) : (int?)null;
                evento.Origem = txtOrigem_DCTS.Text;
                evento.Destino = txtDestino_DCTS.Text;
                evento.ValorPagamento = !txtValorPago_DCTS.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtValorPago_DCTS.Text) : 0;
                evento.Justificativa = txtJustificativa_DCTS.Text;
                evento.Servidores = Servidores_DCTS.Distinct().ToList();
                evento.DataPagamento = !txtDataPagamento_DCTS.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataPagamento_DCTS.Text) : (DateTime?)null; ;


                evento.UsuarioId = User.Identity.Name;
                evento.PequenaDespesaId = !hidPequenaDespesaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hidPequenaDespesaId.Value) : -1;

                return Salva(evento, new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosArquivoXml());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataTable ListaTipoTransporte()
        {
            return rnTipoTransporte.ListaAtivo();
        }

        private int ObtemTipoDespesa()
        {
            if (tseEvento.DBValue.IsNull || !tseEvento.IsValidDBValue)
            {
                if (!rblFiltroTipoEvento.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rblFiltroTipoEvento.SelectedValue.Contains("0"))
                    {
                        switch (rblDocumentoFiscal.SelectedValue)
                        {
                            case "0":
                                return (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum; //Despesa com NF-e = 0

                            case "1":
                                return (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais; //DDespesa com Demais Documentos Fiscais = 1
                        }
                    }
                    else
                    {
                        if (!rbFiltroTipoPequenaDespesa.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        {
                            switch (rbFiltroTipoPequenaDespesa.SelectedValue)
                            {
                                case "0":
                                    return (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao; //PequenaDespesaComComprovacao = 2

                                //case "1":
                                //    return (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaSemComprovacao;

                                case "2":
                                    return (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores; ;//PequenaDespesaComTranslado = 3

                            }
                        }
                        else
                            return -1;
                    }
                }
                else
                {
                    return -1;
                }

            }
            var eventoId = Convert.ToInt32(tseEvento.Value);

            return rnEvento.ObtemTipoEventoPor(eventoId);
        }

        private bool TemItensNF()
        {
            if (tseEvento.DBValue.IsNull || !tseEvento.IsValidDBValue)
                return false;

            var eventoId = Convert.ToInt32(tseEvento.Value);

            return rnEvento.TemItensNFPor(eventoId);
        }

        #endregion

        #region Propriedade auxiliar de modo de tela

        private enum ModoTelaEnum
        {
            FiltroVazio = 0,

            ConsultaVazio = 1,
            ConsultaDespesaComumExistente = 2,
            ConsultaPequenaDespesaComComprovacaoExistente = 3,
            ConsultaPequenaDespesaSemComprovacaoExistente = 4, //revisar
            ConsultaPequenaDespesaComTransladoExistente = 5,

            InserirNovo = 6,
            InserirNovaDespesaComum = 7,
            InserirNovaPequenaDespesa = 8,
            InserirNovaPequenaDespesaComComprovacao = 9,
            InserirNovaPequenaDespesaSemComprovacao = 10,
            InserirNovaPequenaDespesaComTranslado = 11,

            EditarDespesaComumExistente = 12,
            EditarPequenaDespesaComComprovacaoExistente = 13,
            EditarPequenaDespesaSemComprovacaoExistente = 14,
            EditarPequenaDespesaComTransladoExistente = 15,
        }

        private ModoTelaEnum ModoTela
        {
            get
            {
                return (ModoTelaEnum)(ViewState["ModoTela"] ?? ModoTelaEnum.FiltroVazio);
            }
            set
            {
                var tabDespesaComum = pcCadastrarEvento.TabPages.FindByName("tabDespesaComum");
                var tabPequenaDespesaComComprovacao = pcCadastrarEvento.TabPages.FindByName("tabPequenaDespesaComComprovacao");
                var tabPequenaDespesaSemComprovacao = pcCadastrarEvento.TabPages.FindByName("tabPequenaDespesaSemComprovacao");
                var tabPequenaDespesaComTranslado = pcCadastrarEvento.TabPages.FindByName("tabPequenaDespesaComTranslado");
                var tabExigencias = pcCadastrarEvento.TabPages.FindByName("tabExigencias");
                var tabItensNF = pcCadastrarEvento.TabPages.FindByName("tabItensNf");

                switch (value)
                {
                    case ModoTelaEnum.FiltroVazio:

                        tseUnidadeEnsino.Enabled = true;
                        tsePlanoTrabalho.Enabled = true;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = false;
                        btnCancel.Visible = false;

                        plaCadastrarEvento.Visible = false;

                        break;

                    case ModoTelaEnum.ConsultaVazio:

                        tseUnidadeEnsino.Enabled = true;
                        tsePlanoTrabalho.Enabled = true;
                        tseEvento.Enabled = true;

                        btnNovo.Visible = true;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = false;
                        btnCancel.Visible = false;

                        plaCadastrarEvento.Visible = false;

                        break;

                    case ModoTelaEnum.ConsultaDespesaComumExistente:

                        tseUnidadeEnsino.Enabled = true;
                        tsePlanoTrabalho.Enabled = true;
                        tseEvento.Enabled = true;

                        btnNovo.Visible = true;
                        btnEditar.Visible = true;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = true;
                        btnSalvar.Visible = false;
                        btnCancel.Visible = false;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = false;

                        pnlDespesa.Enabled = false;
                        pnlTipoPequenaDespesa.Visible = false;
                        pnlTipoPequenaDespesa.Enabled = false;
                        pnlTipoDocumentoFiscal.Visible = true;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = true;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = true;
                        tabItensNF.ClientVisible = TemItensNF();

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabDespesaComum;

                        btnAnexarOrcamento1.Visible = false;
                        btnAnexarOrcamento2.Visible = false;
                        btnAnexarOrcamento3.Visible = false;
                        lnkVisualizarOrcamento1.Visible = false;
                        lnkVisualizarOrcamento2.Visible = false;
                        lnkVisualizarOrcamento3.Visible = false;
                        txtJustificativa.ReadOnly = true;

                        tseFornecedor.Visible = false;
                        lblFornecedor.Visible = true;

                        tbChaveAcesso.Visible = false;
                        txtChaveAcesso.ReadOnly = true;
                        txtNumeroNF.ReadOnly = true;
                        txtValorTotalNF.ReadOnly = true;
                        txtValorPagoNF.ReadOnly = true;
                        txtDataNF.ReadOnly = true;
                        txtDataPagamentoNF.ReadOnly = true;
                        btnAnexarNotaFiscal.Visible = false;
                        btnAnexarComprovantePgto.Visible = false;
                        btnAnexarXML.Visible = false;
                        lnkVisualizarNotaFiscal.Visible = false;
                        lnkVisualizarComprovantePgto.Visible = false;
                        lnkVisualizarXML.Visible = false;
                        lnkVisualizarXML.Text = "XML inserido";

                        txtObservacao.ReadOnly = true;
                        btnAnexarEvidencia.Visible = false;
                        lnkVisualizarEvidencia.Visible = false;

                        pnlStatusAnalise.Visible = true;

                        break;

                    case ModoTelaEnum.ConsultaPequenaDespesaComComprovacaoExistente:

                        tseUnidadeEnsino.Enabled = true;
                        tsePlanoTrabalho.Enabled = true;
                        tseEvento.Enabled = true;

                        btnNovo.Visible = true;
                        btnEditar.Visible = true;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = true;
                        btnSalvar.Visible = false;
                        btnCancel.Visible = false;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = false;

                        pnlDespesa.Enabled = false;
                        pnlTipoPequenaDespesa.Visible = true;
                        pnlTipoPequenaDespesa.Enabled = false;
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = true;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = true;
                        tabItensNF.ClientVisible = TemItensNF();

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabPequenaDespesaComComprovacao;

                        tseFornecedor_DCC.Visible = false;
                        lblFornecedor_DCC.Visible = true;

                        txtNumeroNF_DCC.ReadOnly = true;
                        txtValorNF_DCC.ReadOnly = true;
                        txtValorPagoNF_DCC.ReadOnly = true;
                        txtDataNF_DCC.ReadOnly = true;
                        txtDataPagamento_DCC.ReadOnly = true;
                        ddlFormaPagamento_DCC.Enabled = false;

                        btnAnexarNotaFiscal_DCC.Visible = false;
                        lnkVisualizarNotaFiscal_DCC.Visible = false;

                        btnAnexarComprovantePgto_DCC.Visible = false;
                        lnkVisualizarComprovantePgto_DCC.Visible = false;

                        pnlStatusAnalise.Visible = true;

                        break;

                    case ModoTelaEnum.ConsultaPequenaDespesaSemComprovacaoExistente:

                        tseUnidadeEnsino.Enabled = true;
                        tsePlanoTrabalho.Enabled = true;
                        tseEvento.Enabled = true;

                        btnNovo.Visible = true;
                        btnEditar.Visible = true;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = true;
                        btnSalvar.Visible = false;
                        btnCancel.Visible = false;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = false;
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        pnlDespesa.Enabled = false;
                        pnlTipoDocumentoFiscal.Visible = true;
                        pnlTipoPequenaDespesa.Enabled = false;

                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = true;
                        tabItensNF.ClientVisible = TemItensNF();

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabPequenaDespesaSemComprovacao;

                        tseFornecedor_DSC.Visible = false;
                        lblFornecedor_DSC.Visible = true;

                        txtValorPago_DSC.ReadOnly = true;
                        txtDataPagamento_DSC.ReadOnly = true;
                        txtJustificativa_DSC.ReadOnly = true;
                        ddlFormaPagamento_DSC.Enabled = false;

                        pnlStatusAnalise.Visible = true;

                        break;

                    case ModoTelaEnum.ConsultaPequenaDespesaComTransladoExistente:

                        tseUnidadeEnsino.Enabled = true;
                        tsePlanoTrabalho.Enabled = true;
                        tseEvento.Enabled = true;

                        btnNovo.Visible = true;
                        btnEditar.Visible = true;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = true;
                        btnSalvar.Visible = false;
                        btnCancel.Visible = false;

                        plaCadastrarEvento.Visible = true;
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = false;

                        pnlDespesa.Enabled = false;
                        pnlTipoPequenaDespesa.Enabled = false;
                        pnlTipoPequenaDespesa.Enabled = true;
                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = true;
                        tabExigencias.ClientVisible = true;
                        tabItensNF.ClientVisible = TemItensNF();

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabPequenaDespesaComTranslado;

                        ddlModalTransporte_DCTS.Enabled = false;
                        txtOrigem_DCTS.ReadOnly = true;
                        txtDestino_DCTS.ReadOnly = true;
                        txtValorPago_DCTS.ReadOnly = true;
                        txtDataPagamento_DCTS.ReadOnly = true;
                        txtJustificativa_DCTS.ReadOnly = true;
                        plaAdicionarServidor_DCTS.Visible = false;

                        plaAdicionarServidor_DCTS.Visible = false;

                        pnlStatusAnalise.Visible = true;

                        break;

                    case ModoTelaEnum.InserirNovo:

                        tseUnidadeEnsino.Enabled = false;
                        tsePlanoTrabalho.Enabled = false;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = false;
                        btnCancel.Visible = true;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = false;
                        txtDescricao.Enabled = false;
                        rblFiltroTipoEvento.Enabled = true;
                        pnlDespesa.Visible = true;
                        pnlDespesa.Enabled = true;
                        pnlTipoPequenaDespesa.Visible = false;
                        pnlTipoPequenaDespesa.Enabled = false;
                        rblFiltroTipoEvento.Items.FindByValue("1").Enabled = Convert.ToBoolean(hidPermitePequenaDespesa.Value);
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = false;
                        tabItensNF.ClientVisible = false;

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = null;

                        pnlStatusAnalise.Visible = false;

                        break;

                    case ModoTelaEnum.InserirNovaDespesaComum:

                        tseUnidadeEnsino.Enabled = false;
                        tsePlanoTrabalho.Enabled = false;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = true;

                        pnlDespesa.Visible = true;
                        pnlDespesa.Enabled = true;
                        pnlTipoPequenaDespesa.Visible = false;
                        pnlTipoPequenaDespesa.Enabled = false;
                        pnlTipoDocumentoFiscal.Visible = true;
                        pnlTipoDocumentoFiscal.Enabled = true;
                        tabDespesaComum.ClientVisible = true;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = false;
                        tabItensNF.ClientVisible = false;

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabDespesaComum;

                        btnAnexarOrcamento1.Visible = true;
                        btnAnexarOrcamento2.Visible = true;
                        btnAnexarOrcamento3.Visible = true;
                        lnkVisualizarOrcamento1.Visible = false;
                        lnkVisualizarOrcamento2.Visible = false;
                        lnkVisualizarOrcamento3.Visible = false;
                        txtJustificativa.ReadOnly = false;

                        tseFornecedor.Visible = true;
                        lblFornecedor.Visible = false;

                        tbChaveAcesso.Visible = true;
                        txtChaveAcesso.ReadOnly = false;
                        txtNumeroNF.ReadOnly = false;
                        txtValorTotalNF.ReadOnly = false;
                        txtValorPagoNF.ReadOnly = false;
                        txtDataNF.ReadOnly = false;
                        txtDataPagamentoNF.ReadOnly = false;
                        btnAnexarNotaFiscal.Visible = true;
                        btnAnexarComprovantePgto.Visible = true;
                        btnAnexarXML.Visible = true;
                        lnkVisualizarNotaFiscal.Visible = false;
                        lnkVisualizarComprovantePgto.Visible = false;
                        lnkVisualizarXML.Visible = false;
                        lnkVisualizarXML.Text = "XML inserido";

                        txtObservacao.ReadOnly = false;
                        btnAnexarEvidencia.Visible = true;
                        lnkVisualizarEvidencia.Visible = false;

                        pnlStatusAnalise.Visible = false;

                        break;

                    case ModoTelaEnum.InserirNovaPequenaDespesa:

                        tseUnidadeEnsino.Enabled = false;
                        tsePlanoTrabalho.Enabled = false;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = false;
                        btnCancel.Visible = true;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = false;
                        txtDescricao.Enabled = false;

                        pnlDespesa.Visible = true;
                        pnlTipoPequenaDespesa.Visible = true;
                        pnlTipoPequenaDespesa.Enabled = true;
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = false;
                        tabItensNF.ClientVisible = false;

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = null;

                        pnlStatusAnalise.Visible = false;

                        break;

                    case ModoTelaEnum.InserirNovaPequenaDespesaComComprovacao:

                        tseUnidadeEnsino.Enabled = false;
                        tsePlanoTrabalho.Enabled = false;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = true;

                        pnlDespesa.Visible = true;
                        pnlTipoPequenaDespesa.Visible = true;
                        pnlTipoPequenaDespesa.Enabled = true;
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = true;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = false;
                        tabItensNF.ClientVisible = false;

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabPequenaDespesaComComprovacao;

                        tseFornecedor_DCC.Visible = true;
                        lblFornecedor_DCC.Visible = false;

                        txtNumeroNF_DCC.ReadOnly = false;
                        txtValorNF_DCC.ReadOnly = false;
                        txtValorPagoNF_DCC.ReadOnly = false;
                        txtDataNF_DCC.ReadOnly = false;
                        txtDataPagamento_DCC.ReadOnly = false;
                        ddlFormaPagamento_DCC.Enabled = true;

                        btnAnexarNotaFiscal_DCC.Visible = true;
                        lnkVisualizarNotaFiscal_DCC.Visible = true;
                        btnAnexarComprovantePgto_DCC.Visible = true;
                        lnkVisualizarComprovantePgto_DCC.Visible = true;

                        pnlStatusAnalise.Visible = false;

                        break;

                    case ModoTelaEnum.InserirNovaPequenaDespesaSemComprovacao:

                        tseUnidadeEnsino.Enabled = false;
                        tsePlanoTrabalho.Enabled = false;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = true;

                        pnlDespesa.Visible = true;
                        pnlTipoPequenaDespesa.Visible = true;
                        pnlTipoPequenaDespesa.Enabled = true;
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = false;
                        tabItensNF.ClientVisible = false;

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabPequenaDespesaSemComprovacao;

                        tseFornecedor_DSC.Visible = true;
                        lblFornecedor_DSC.Visible = false;

                        txtValorPago_DSC.ReadOnly = false;
                        txtDataPagamento_DSC.ReadOnly = false;
                        txtJustificativa_DSC.ReadOnly = false;
                        ddlFormaPagamento_DSC.Enabled = true;

                        pnlStatusAnalise.Visible = false;

                        break;

                    case ModoTelaEnum.InserirNovaPequenaDespesaComTranslado:

                        tseUnidadeEnsino.Enabled = false;
                        tsePlanoTrabalho.Enabled = false;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = true;

                        pnlDespesa.Visible = true;
                        pnlTipoPequenaDespesa.Visible = true;
                        pnlTipoPequenaDespesa.Enabled = true;
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = true;
                        tabExigencias.ClientVisible = false;
                        tabItensNF.ClientVisible = false;

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabPequenaDespesaComTranslado;

                        ddlModalTransporte_DCTS.Enabled = true;
                        txtOrigem_DCTS.ReadOnly = false;
                        txtDestino_DCTS.ReadOnly = false;
                        txtValorPago_DCTS.ReadOnly = false;
                        txtDataPagamento_DCTS.ReadOnly = false;
                        txtJustificativa_DCTS.ReadOnly = false;

                        pnlStatusAnalise.Visible = false;
                        plaAdicionarServidor_DCTS.Visible = true;
                        break;

                    case ModoTelaEnum.EditarDespesaComumExistente:

                        tseUnidadeEnsino.Enabled = false;
                        tsePlanoTrabalho.Enabled = false;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = true;

                        pnlDespesa.Enabled = false;
                        pnlTipoPequenaDespesa.Visible = false;
                        pnlTipoPequenaDespesa.Enabled = true;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = true;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = false;
                        tabItensNF.ClientVisible = false;

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabDespesaComum;

                        btnAnexarOrcamento1.Visible = true;
                        btnAnexarOrcamento2.Visible = true;
                        btnAnexarOrcamento3.Visible = true;
                        lnkVisualizarOrcamento1.Visible = false;
                        lnkVisualizarOrcamento2.Visible = false;
                        lnkVisualizarOrcamento3.Visible = false;
                        txtJustificativa.ReadOnly = false;

                        tseFornecedor.Visible = true;
                        lblFornecedor.Visible = false;

                        tbChaveAcesso.Visible = true;
                        txtChaveAcesso.ReadOnly = false;
                        txtNumeroNF.ReadOnly = false;
                        txtValorTotalNF.ReadOnly = false;
                        txtValorPagoNF.ReadOnly = false;
                        txtDataNF.ReadOnly = false;
                        txtDataPagamentoNF.ReadOnly = false;
                        btnAnexarNotaFiscal.Visible = true;
                        btnAnexarComprovantePgto.Visible = true;
                        btnAnexarXML.Visible = true;
                        lnkVisualizarNotaFiscal.Visible = false;
                        lnkVisualizarComprovantePgto.Visible = false;
                        lnkVisualizarXML.Visible = false;
                        lnkVisualizarXML.Text = "XML inserido";

                        txtObservacao.ReadOnly = false;
                        btnAnexarEvidencia.Visible = true;
                        lnkVisualizarEvidencia.Visible = false;

                        pnlStatusAnalise.Visible = false;

                        break;

                    case ModoTelaEnum.EditarPequenaDespesaComComprovacaoExistente:

                        tseUnidadeEnsino.Enabled = false;
                        tsePlanoTrabalho.Enabled = false;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = true;

                        pnlDespesa.Enabled = false;
                        pnlTipoPequenaDespesa.Enabled = false;
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = true;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = false;
                        tabItensNF.ClientVisible = false;

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabPequenaDespesaComComprovacao;

                        tseFornecedor_DCC.Visible = true;
                        lblFornecedor_DCC.Visible = false;

                        txtNumeroNF_DCC.ReadOnly = false;
                        txtValorNF_DCC.ReadOnly = false;
                        txtValorPagoNF_DCC.ReadOnly = false;
                        txtDataNF_DCC.ReadOnly = false;
                        txtDataPagamento_DCC.ReadOnly = false;
                        ddlFormaPagamento_DCC.Enabled = true;

                        btnAnexarNotaFiscal_DCC.Visible = true;
                        lnkVisualizarNotaFiscal_DCC.Visible = true;
                        btnAnexarComprovantePgto_DCC.Visible = true;
                        lnkVisualizarComprovantePgto_DCC.Visible = true;

                        pnlStatusAnalise.Visible = false;

                        break;

                    case ModoTelaEnum.EditarPequenaDespesaSemComprovacaoExistente:

                        tseUnidadeEnsino.Enabled = false;
                        tsePlanoTrabalho.Enabled = false;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = true;

                        pnlDespesa.Enabled = false;
                        pnlTipoPequenaDespesa.Enabled = false;
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = false;
                        tabItensNF.ClientVisible = false;

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabPequenaDespesaSemComprovacao;

                        tseFornecedor_DSC.Visible = true;
                        lblFornecedor_DSC.Visible = false;

                        txtValorPago_DSC.ReadOnly = false;
                        txtDataPagamento_DSC.ReadOnly = false;
                        txtJustificativa_DSC.ReadOnly = false;
                        ddlFormaPagamento_DSC.Enabled = true;

                        pnlStatusAnalise.Visible = false;

                        break;

                    case ModoTelaEnum.EditarPequenaDespesaComTransladoExistente:

                        tseUnidadeEnsino.Enabled = false;
                        tsePlanoTrabalho.Enabled = false;
                        tseEvento.Enabled = false;

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnEditarDatas.Visible = false;
                        btnDeletar.Visible = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;

                        plaCadastrarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = true;

                        pnlDespesa.Enabled = false;
                        pnlTipoPequenaDespesa.Enabled = false;
                        pnlTipoDocumentoFiscal.Visible = false;
                        pnlTipoDocumentoFiscal.Enabled = false;
                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = true;
                        tabExigencias.ClientVisible = false;
                        tabItensNF.ClientVisible = false;

                        pcCadastrarEvento.Visible =
                            tabDespesaComum.ClientVisible ||
                            tabPequenaDespesaComComprovacao.ClientVisible ||
                            tabPequenaDespesaSemComprovacao.ClientVisible ||
                            tabPequenaDespesaComTranslado.ClientVisible;
                        pcCadastrarEvento.ActiveTabPage = tabPequenaDespesaComTranslado;

                        ddlModalTransporte_DCTS.Enabled = true;
                        txtOrigem_DCTS.ReadOnly = false;
                        txtDestino_DCTS.ReadOnly = false;
                        txtValorPago_DCTS.ReadOnly = false;
                        txtDataPagamento_DCTS.ReadOnly = false;
                        txtJustificativa_DCTS.ReadOnly = false;
                        plaAdicionarServidor_DCTS.Visible = true;

                        pnlStatusAnalise.Visible = false;

                        break;
                }

                ViewState["ModoTela"] = value;
                hidModoTela.Value = value.ToString();
            }
        }

        #endregion

        protected void rblFiltroTipoEvento_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (rblFiltroTipoEvento.SelectedValue)
            {
                case "0":
                    ModoTela = ModoTelaEnum.InserirNovaDespesaComum;
                    ZerarCamposDespesaComum();
                    break;

                case "1":
                    ModoTela = ModoTelaEnum.InserirNovaPequenaDespesa;
                    break;
            }
        }

        protected void rbFiltroTipoPequenaDespesa_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (rbFiltroTipoPequenaDespesa.SelectedValue)
            {
                case "0":
                    ModoTela = ModoTelaEnum.InserirNovaPequenaDespesaComComprovacao;
                    ZerarCamposPequenaDespesaComComprovacao();
                    break;

                //case "1":
                //    ModoTela = ModoTelaEnum.InserirNovaPequenaDespesaSemComprovacao;
                //    ZerarCamposPequenaDespesaSemComprovacao();
                //    break;

                case "2":
                    ModoTela = ModoTelaEnum.InserirNovaPequenaDespesaComTranslado;
                    ZerarCamposPequenaDespesaComTranslado();
                    break;
            }
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

        protected void btnAdicionarServidor_DCTS_Click(object sender, EventArgs e)
        {
            try
            {
                if (tseAdicionarServidor_DCTS.DBValue.IsNull || !tseAdicionarServidor_DCTS.IsValidDBValue)
                {
                    lblMensagem.Text = "Para adicionar é necessário escolher um servidor.";
                    return;
                }

                if (grdServidores_DCTS.GetCurrentPageRowValues("Matricula").Any(q => q.ToString() == tseAdicionarServidor_DCTS.Value.ToString()))
                {
                    lblMensagem.Text = "Este servidor já tinha sido adicionado.";
                    return;
                }

                if (tseAdicionarServidor_DCTS["pessoa"].ToString().IsNullOrEmptyOrWhiteSpace() ||
                    tseAdicionarServidor_DCTS["nome"].ToString().IsNullOrEmptyOrWhiteSpace() ||
                    tseAdicionarServidor_DCTS["idfuncional"].ToString().IsNullOrEmptyOrWhiteSpace() ||
                    tseAdicionarServidor_DCTS["matricula"].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "Para escolher um servidor todas as suas informações(Id Funcional/Matrícula) deverão estar preenchidas.";
                    return;
                }

                Servidores_DCTS.Add(new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosPequenaDespesaServidor
                {
                    PequenaDespesaServidorId = 0,
                    PequenaDespesaId = !hidPequenaDespesaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hidPequenaDespesaId.Value) : -1,
                    Pessoa = Convert.ToInt32(tseAdicionarServidor_DCTS["pessoa"]),
                    NomeCompl = Convert.ToString(tseAdicionarServidor_DCTS["nome"]),
                    IdFuncional = Convert.ToInt32(tseAdicionarServidor_DCTS["idfuncional"]),
                    Matricula = Convert.ToString(tseAdicionarServidor_DCTS["matricula"]),
                    UsuarioId = User.Identity.Name,
                    DataCadastro = DateTime.Now,
                    DataAlteracao = DateTime.Now,
                });

                grdServidores_DCTS.DataSource = Servidores_DCTS;
                grdServidores_DCTS.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected IList<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosPequenaDespesaServidor> Servidores_DCTS
        {
            get
            {
                if (ViewState["Servidores_DCTS"] == null)
                    ViewState["Servidores_DCTS"] = new List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosPequenaDespesaServidor>();

                return (ViewState["Servidores_DCTS"] as List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosPequenaDespesaServidor>);
            }
            set
            {
                ViewState["Servidores_DCTS"] = value;
            }
        }

        protected void grdServidores_DCTS_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            e.Visible = ((ModoTela == ModoTelaEnum.EditarPequenaDespesaComTransladoExistente || ModoTela == ModoTelaEnum.InserirNovaPequenaDespesaComTranslado) && e.ButtonType == ColumnCommandButtonType.Delete);
        }

        protected void grdServidores_DCTS_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.PrestacaoContas.PequenaDespesaServidor rnPequenaDespesaServidor = new RN.PrestacaoContas.PequenaDespesaServidor();
                int pequenaDespesaServidorId = 0;

                pequenaDespesaServidorId = Convert.ToInt32(e.Keys["PEQUENADESPESASERVIDORID"]);

                var enumerator = e.Values.GetEnumerator();
                int pessoa = 0;
                int idFuncional = 0;
                while (enumerator.MoveNext())
                    if (enumerator.Key == "IdFuncional")
                        idFuncional = Convert.ToInt32(enumerator.Value);
                    else if (enumerator.Key == "Pessoa")
                        pessoa = Convert.ToInt32(enumerator.Value);


                var objeto = Servidores_DCTS.FirstOrDefault(q => q.Pessoa == pessoa && q.IdFuncional == idFuncional);

                if (objeto != null)
                {
                    Servidores_DCTS.Remove(objeto);
                }

                grdServidores_DCTS.DataSource = Servidores_DCTS;
                grdServidores_DCTS.DataBind();

                e.Cancel = true;


            }
            catch (NotSupportedException ex)
            {
                throw ex;
                lblMensagem.Text = ex.Message;
            }

        }

        private DataURLParts SplitDataURL(string dataURL)
        {
            if (dataURL.IsNullOrEmptyOrWhiteSpace())
                return null;

            var dataUrlParts = dataURL.Split(';');
            if (dataUrlParts.Length < 3)
                return null;

            var getPartValue = new Func<string[], string, char, string>((parts, partName, delimiter) =>
            {
                var partValue = null as string;

                if (!parts.Any())
                    return null;

                foreach (var part in parts)
                {
                    var partArray = part.Split(delimiter);

                    if (partArray.Length != 2)
                        continue;

                    if (partArray[0] != partName)
                        continue;

                    partValue = partArray[1];
                    break;
                }

                return partValue;
            });

            var loadBase64 = new Func<string, Stream>(base64 =>
            {
                byte[] bytes = Convert.FromBase64String(base64);
                MemoryStream ms = new MemoryStream(bytes);
                return ms as Stream;
            });

            return new DataURLParts
            {
                MimeType = getPartValue(dataUrlParts, "data", ':'),
                Name = getPartValue(dataUrlParts, "name", ':'),
                Stream = loadBase64(getPartValue(dataUrlParts, "base64", ',')),
                ByteArray = Convert.FromBase64String(getPartValue(dataUrlParts, "base64", ',')),
            };
        }

        private class DataURLParts
        {
            public string MimeType { get; set; }
            public string Name { get; set; }
            public Stream Stream { get; set; }
            public byte[] ByteArray { get; set; }
        }


        protected void rblDocumentoFiscal_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                tbXML.Visible = true;
                tbChaveAcesso.Visible = true;
                if (rblDocumentoFiscal.SelectedValue == "1")
                {
                    tbChaveAcesso.Visible = false;
                    tbXML.Visible = false;
                    txtChaveAcesso.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsePeriodoPrestacaoContas_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                FiltraEvento();
                tseEvento.Enabled = true ;
                tseEvento.DataBind();

                if (tsePeriodoPrestacaoContas.IsValidDBValue)
                {
                    ModoTela = ModoTelaEnum.ConsultaVazio;
                    return;
                }

                rblFiltroTipoEvento.ClearSelection();
                hidPermitePequenaDespesa.Value = string.Empty;
                rblDocumentoFiscal.ClearSelection();

                if (!tsePlanoTrabalho.DBValue.IsNull && tsePlanoTrabalho.IsValidDBValue)
                {
                    hidPermitePequenaDespesa.Value = rnPlanoTrabalho.PermitePequenaDespesa(Convert.ToInt32(tsePlanoTrabalho.Value)).ToString();

                    if (tsePlanoTrabalho["FINALIDADEID"].ToString() == "2") //MERENDA
                    {
                        rblDocumentoFiscal.Items.FindByValue("1").Enabled = false;
                    }
                    else
                    {
                        rblDocumentoFiscal.Items.FindByValue("1").Enabled = true;
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