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
using System.Data.SqlTypes;
using DevExpress.Web.ASPxClasses;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/IncluirCreditoeDebito.aspx"), ControlText("IncluirCreditoeDebito"), Title("Inclusão de Créditos e Débitos")]
    public partial class IncluirCreditoeDebito : TPage
    {
        RN.PrestacaoContas.PlanoTrabalho rnPlanoTrabalho = new RN.PrestacaoContas.PlanoTrabalho();
        RN.PrestacaoContas.Operacao rnOperacao = new RN.PrestacaoContas.Operacao();
        RN.PrestacaoContas.OperacaoDocumentos rnOperacaoDocumentos = new RN.PrestacaoContas.OperacaoDocumentos();
        RN.PrestacaoContas.OperacaoExigencia rnOperacaoExigencia = new RN.PrestacaoContas.OperacaoExigencia();
        RN.PrestacaoContas.OperacaoExigenciaArquivo rnOperacaoExigenciaArquivo = new RN.PrestacaoContas.OperacaoExigenciaArquivo();        
        private readonly Techne.Lyceum.RN.PrestacaoContas.Evento rnEvento = new Techne.Lyceum.RN.PrestacaoContas.Evento();
        private readonly Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia();
        public enum TipoOperacao
        {
            Novo,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            Cancelar,
            SucessoAnalise,
            Excluir,
            Desativar
        }
        public void ListOperacaoDocumento() { }
        public void Insert(object TIPOEXIGENCIAOPERACAOID, object JUSTIFICATIVA, object NOTAEXPLICATIVA, object APROVADO) { }
        public void Update(object TIPOEXIGENCIAOPERACAOID, object NOTAEXPLICATIVA, object JUSTIFICATIVA, object APROVADO, object OPERACAOEXIGENCIAID) { }
        public void Delete(object OPERACAOEXIGENCIAID) { }
        public void DeleteOperacaoDocumento(object OPERACAODOCUMENTOSID) { }
        

        protected void Page_Init()
        {
            if (IsPostBack)
                return;
            TituloGrid(grdDocumento, "Documentos");
            TituloGrid(grdExigencias, "");
        }

        protected void grdDocumento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdDocumento.Settings.ShowFilterRow = false;

        }
        private TipoOperacao _tipoOperacao
        {
            get
            {
                if (ViewState["_tipoOperacao"] != null)
                {
                    if (ViewState["_tipoOperacao"] is TipoOperacao)
                    {
                        return (TipoOperacao)ViewState["_tipoOperacao"];
                    }
                }

                return TipoOperacao.Inicial;
            }

            set
            {
                ViewState["_tipoOperacao"] = value;
            }
        }
        protected void Page_Load()
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                      }
                    else
                    {
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();
                    }
                }

                int fornecedorId;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var operacao = new RN.PrestacaoContas.Entidades.Operacao();

                var mensagens = new List<string>();

                if (tsePeriodoPrestacaoContas.Value == null || !tsePeriodoPrestacaoContas.IsValidDBValue)
                    mensagens.Add("PERÍODO DE PRESTAÇÃO DE CONTAS não selecionado (favor verificar).");

                if (tseUnidadeEnsino.Value == null || !tseUnidadeEnsino.IsValidDBValue)
                    mensagens.Add("UNIDADE DE ENSINO não selecionado (favor verificar).");


                if (tsePlanoTrabalho.Value == null || !tsePlanoTrabalho.IsValidDBValue)
                    mensagens.Add("PROJETO/PROGRAMA não selecionado (favor verificar).");

                if (mensagens.Any())
                {
                    lblMensagem.Text = mensagens.Distinct().Aggregate((x, y) => x + "<br />" + y);
                    return;
                }


                operacao = rnOperacao.ListaOperacaoPor(tsePeriodoPrestacaoContas.Value.ToString(), tsePlanoTrabalho.Value.ToString(), tseUnidadeEnsino.Value.ToString());

                txtValor.Text = operacao.Valor.ToString();
                txtJustificativa.Text = operacao.Justificativa.ToString();

                pcCreditoDebito.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    /*    protected void grdDocumento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();
                RN.PrestacaoContas.Entidades.OperacaoDocumentos arquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoDocumentos();
                
                int tipoexigenciaoperacaoid = 0;

                tipoexigenciaoperacaoid = Convert.ToInt32(e.Keys["OPERACAODOCUMENTOSID"]);

                validacao = rnArquivoOcorrencia.ValidaRemocao(tipoexigenciaoperacaoid);

                if (validacao.Valido)
                {
                    rnArquivoOcorrencia.Remove(tipoexigenciaoperacaoid);
                    grdExigencias.DataBind();
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch
            {
                return;
            }
        }*/
        protected void grdDocumento_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {

        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            var operacao = new RN.PrestacaoContas.Entidades.Operacao();
            var valido = true;
            string mensagem = string.Empty;
            ValidacaoDados validacao = new ValidacaoDados();


            try
            {
                //   operacao.PeriodoReferenciaId = !tsePeriodoPrestacaoContas.Text.IsNullOrEmptyOrWhiteSpace() ? txtRazaoSocial.Text.Trim().ToUpper() : null;

                operacao.PeriodoReferenciaId = Convert.ToInt32(tsePeriodoPrestacaoContas.DBValue.ToString());
                operacao.PlanoTrabalhoId = Convert.ToInt32(tsePlanoTrabalho.DBValue.ToString());
                operacao.Censo = tseUnidadeEnsino.DBValue.ToString();
                operacao.Justificativa = txtJustificativa.Text;
                operacao.TipoOperacao = ddlOperacao.SelectedValue;
                if (txtValor.Text.Length > 12)
                {
                    lblMensagem.Text =  "O campo Valor esta acima do limite esperado";
                    return;
                }
                operacao.Valor = !string.IsNullOrEmpty(txtValor.Text) ? Convert.ToDecimal(txtValor.Text) : 0;

                operacao.Status = 0;
                operacao.UsuarioId = User.Identity.Name;
                operacao.DataCadastro = DateTime.Now.Date;

                validacao = rnOperacao.Valida(operacao);

                if (validacao.Valido)
                {
                    
                    //var jaexiste = rnOperacao.ExisteOperacaoPor(operacao.PeriodoReferenciaId, operacao.PlanoTrabalhoId, operacao.Censo, operacao.TipoOperacao);
                    if (hdnOcorrenciaId.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        hdnOcorrenciaId.Value = rnOperacao.Insere(operacao).ToString();
 
                        pnlDocumento.Visible = true;

                        mensagem = "Operação inserida com sucesso.";
                        lblStatus.Text = RetornaStatus(operacao.Status);
                        hidStatus.Value = operacao.Status.ToString(); 
                        lblTxtStatus.Text = "Status da Operação:";
                        _tipoOperacao = TipoOperacao.Sucesso;
                    }
                    else
                    {
                        operacao.OperacaoId = Convert.ToInt32(hdnOcorrenciaId.Value);
                        rnOperacao.Atualiza(operacao);
                        mensagem = "Operação atualizada com sucesso.";
                    }

                    lblMensagem.Text = mensagem;

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
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
                _tipoOperacao = TipoOperacao.Cancelar ;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            var operacao = new RN.PrestacaoContas.Entidades.Operacao();
            string mensagem = string.Empty;
            try
            {

                rnOperacao.Remove(Convert.ToInt32(hdnOcorrenciaId.Value));

                lblMensagem.Text = "Operação excluida com sucesso.";
             //   lblStatus.Text = RetornaStatus(operacao.Status);
             //   hidStatus.Value = operacao.Status.ToString();
                lblTxtStatus.Text = "Status da Operação:";

                _tipoOperacao = TipoOperacao.Excluir ;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEnviarAnalise_Click(object sender, ImageClickEventArgs e)
        {
            var operacao = new RN.PrestacaoContas.Entidades.OperacaoDocumentos();
            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;

            try
            {
                validacao = rnOperacaoDocumentos.ValidaEnvioAnalise(Convert.ToInt32(hdnOcorrenciaId.Value));

                if (validacao.Valido)
                {
                    rnOperacaoDocumentos.EnviaAnalise(Convert.ToInt32(hdnOcorrenciaId.Value));
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                    lblMensagem.Text = "Operação enviado para analise com sucesso.";
                    lblStatus.Text = RetornaStatus(1);
                    hidStatus.Value = "1"; 
                    lblTxtStatus.Text = "Status da Operação";
                    btnCancel.Visible = true;
                    btnEditar.Visible = false;
                    btnNovo.Visible = false;
                    btnExcluir.Visible = false;
                    btnSalvar.Visible = false;
                    btnEnviarAnalise.Visible = false;
                    btnAnalisar.Visible = false;
                    ddlOperacao.Enabled = false;
                    txtValor.Enabled = false;
                    txtJustificativa.Enabled = false;
                  //  grdExigencias.Enabled = false;
                    TSeDocNecessario.Visible = false;
                    FileUpload1.Visible = false;
                    btnAnexar.Visible = false;
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void btnAnalisar_PreRender(object sender, EventArgs e)
        {

        }
        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int id = 0;
                try
                {
                    id = Convert.ToInt32(tseOperacao.Value);
                }
                catch {
                    id = Convert.ToInt32(hdnOcorrenciaId.Value);
                }
                var operacao = rnOperacao.ObtemPor(id);

                if (operacao.Status == 0)
                { 
                    pnlDocumento.Visible = true;
                    btnAnexar.Visible = true;
                    btnEnviarAnalise.Visible = false;
                    btnCancel.Visible = true;
                    btnSalvar.Visible = true;
                    btnEditar.Visible = false;
                    txtJustificativa.Visible = true;
                    txtValor.Visible = true;
                    txtJustificativa.Enabled = true;
                    txtValor.Enabled = true;

                    lblStatus.Text = RetornaStatus(operacao.Status);
                    hidStatus.Value = operacao.Status.ToString(); 

                    pnlDocumento.Enabled = true;
                    pnlDocumento.Visible = true;
                    pcCreditoDebito.Visible = true;

                    lblDocumentoInserir.Visible = true;
                    TSeDocNecessario.Visible = true;
                    FileUpload1.Visible = true;
                }
                else {
                    pnlDocumento.Visible = true;
                    btnAnexar.Visible = false;
                    btnEnviarAnalise.Visible = true;
                    btnCancel.Visible = true;
                    btnEditar.Visible = false;
                    btnSalvar.Visible = false;
                    
                    txtJustificativa.Visible = true;
                    txtValor.Visible = true;
                    txtJustificativa.Enabled = false;
                    txtValor.Enabled = false;
                    lblStatus.Text = RetornaStatus(operacao.Status);
                    hidStatus.Value = operacao.Status.ToString(); 
                    pnlDocumento.Enabled = true;
                    pnlDocumento.Visible = true;
                    pcCreditoDebito.Visible = true;

                    lblDocumentoInserir.Visible = false;
                    TSeDocNecessario.Visible = false;
                    FileUpload1.Visible = false;
                }


                
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void btnConfirmarAnalise_Click(object sender, EventArgs e)
        {
            RN.PrestacaoContas.FornecedorAnalise rnFornecedorAnalise = new Techne.Lyceum.RN.PrestacaoContas.FornecedorAnalise();
            RN.PrestacaoContas.Entidades.FornecedorAnalise analise = new Techne.Lyceum.RN.PrestacaoContas.Entidades.FornecedorAnalise();

            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;

            try
            {
                analise.Aprovada = rblConfirmacao.SelectedValue == "Aprovado";
                analise.MotivoReprovacaoFornecedorId = rblConfirmacao.SelectedValue == "Reprovado" && !ddlMotivoReprovacaoFornecedor.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivoReprovacaoFornecedor.SelectedValue) : (int?)null;
                analise.UsuarioId = User.Identity.Name;

                var motivosReprovacaoFornecedorId = new int[] { };
                try
                {
                    motivosReprovacaoFornecedorId = hidMotivosSelecionados.Value
                        .Split(';')
                        .Select(s => Convert.ToInt32(s))
                        .ToArray();
                }
                catch
                {
                }

                validacao = rnFornecedorAnalise.ValidaAnalise(analise, motivosReprovacaoFornecedorId);

                if (validacao.Valido)
                {
                    rnFornecedorAnalise.Analisa(analise, motivosReprovacaoFornecedorId);
                    //grdHistoricoAnalise.DataBind();

                    _tipoOperacao = TipoOperacao.SucessoAnalise;
                    ControlarTipoOperacao();
                    lblMensagem.Text = "Operação analisada com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
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
                var tabela = "";
                IOperacaoExigenciaArquivo rnArquivo = null;
                IOperacaoDocumentos rnArquivoDocumento = null;

                switch (((WebControl)sender).ID)
                {
                    case "lnkVisualizar":
                        tabela = "OperacaoDocumentos";
                        rnArquivoDocumento = rnOperacaoDocumentos;
                        break;

                    case "btnVisualizar":
                        tabela = "OperacaoExigenciaArquivo";
                        rnArquivo = rnOperacaoExigenciaArquivo;
                        break;

                    default:
                        throw new Exception("Deu erro na rotina de visualização de fotos.");
                }

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });
                var PossuiChave = rnOperacaoExigenciaArquivo.PossuiChave(Convert.ToInt32(chave[0]));
                if (!PossuiChave)
                   chave[0] = Convert.ToString(grdDocumento.GetRowValues(1, "OPERACAODOCUMENTOSID"));

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

                lblMensagem.Text = String.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void btnVisualizarExigencia_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "";
                IOperacaoExigenciaArquivo rnArquivo = null;
                IOperacaoDocumentos rnArquivoDocumento = null;

                switch (((WebControl)sender).ID)
                {
                    case "lnkVisualizar":
                        tabela = "OperacaoDocumentos";
                        rnArquivoDocumento = rnOperacaoDocumentos;
                        break;

                    case "btnVisualizar":
                        tabela = "OperacaoExigenciaArquivo";
                        rnArquivo = rnOperacaoExigenciaArquivo;
                        break;

                    default:
                        throw new Exception("Deu erro na rotina de visualização de fotos.");
                }

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });
              
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

                lblMensagem.Text = String.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.PrestacaoContas.OperacaoDocumentos rnOperacaoDocumentos = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();
                var mensagem = 0;
                lblMensagem.Text = "";
                hidStatus.Value = "0"; 
                if (tseUnidadeEnsino.Value == null || !tseUnidadeEnsino.IsValidDBValue)
                {
                    lblMensagem.Text = "UNIDADE DE ENSINO não selecionado (favor verificar).";
                    mensagem = mensagem + 1;}
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                }

                if (tsePeriodoPrestacaoContas.Value == null || !tsePeriodoPrestacaoContas.IsValidDBValue)
                    { lblMensagem.Text = "PERÍODO DE PRESTAÇÃO DE CONTAS não selecionado (favor verificar).";
                     mensagem = mensagem + 1;}
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                }

                if (tsePlanoTrabalho.Value == null || !tsePlanoTrabalho.IsValidDBValue)
                {
                    lblMensagem.Text = "PROJETO/PROGRAMA não selecionado (favor verificar).";
                    mensagem = mensagem + 1;}
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                }

                if (mensagem == 0)
                {
                  //  if (rnOperacaoDocumentos.VerificaEnvioSEIPorEscolaPeriodo(tseUnidadeEnsino.Value.ToString(), Convert.ToInt32(tsePeriodoPrestacaoContas.Value)))
                 //   {
                 //       throw new Exception("Operação não pode ser cadastrado, pois o Formulário SEI já foi gerado.");
                 //   }
                 //   else
                 //   {
                        _tipoOperacao = TipoOperacao.Novo;
                 //   }

                    ControlarTipoOperacao();
                }
                else return;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
             //   Response.Redirect("IncluirCreditoeDebito.aspx");
            }
        }
        protected void ddlOperacao_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

   

        private void HabilitaDesabilitaCamposAbaInformacoesGerais(bool habilita)
        {
  
        }

        protected void tsePlanoTrabalho_Changed(object sender, EventArgs args)
        {
            try
            {
                if (!tsePlanoTrabalho.DBValue.IsNull)
                {
                    if (tsePlanoTrabalho.IsValidDBValue)
                    {
                        DataContext contexto = null;
                        contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                        string[] dadosidentificador = rnPlanoTrabalho.ObtemIdentificadorPor(contexto, Convert.ToInt32(tsePlanoTrabalho.DBValue));
                        lblIdentificador.Text = dadosidentificador[0];
                        lblDescricao.Text = dadosidentificador[1];
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;
                        lblMensagem.Text = "Plano de Trabalho não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Plano de Trabalho não ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }


                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseUnidadeEnsino_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseUnidadeEnsino.DBValue.IsNull)
                {
                    if (tseUnidadeEnsino.IsValidDBValue)
                    {
                        //     this._tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;
                        lblMensagem.Text = "Unidade de Ensino não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Unidade de Ensino não ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }


                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void ControlarVisibilidadeControle(ImageButton[] imgBotoes, Button[] botoes)
        {
            RetiraVisibilidadeBotao();

            if (imgBotoes != null)
            {
                foreach (ImageButton botao in imgBotoes)
                {
                    botao.Visible = true;
                }
            }

            if (botoes != null)
            {
                foreach (Button botao in botoes)
                {
                    botao.Visible = true;
                }
            }

            ControlaAcesso(btnSalvar, AcaoControle.editar);
            ControlaAcesso(btnEnviarAnalise, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnAnalisar, AcaoControle.editar);
  
            
            if (!Permission.AllowUpdate || !Permission.AllowInsert)
            {
                //grdDocumento.Columns[9].Visible = false;
            }

        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnNovo.Visible = false;
            btnExcluir.Visible = false;
            btnSalvar.Visible = false;
            btnEnviarAnalise.Visible = false;
            btnAnalisar.Visible = false;
        }
        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tsePlanoTrabalho.Enabled = true;
                        tsePeriodoPrestacaoContas.Enabled = true;
                        tseUnidadeEnsino.Enabled = true;
                        txtJustificativa.Enabled = true;
                        tseOperacao.Enabled = true;
                        txtValor.Enabled = true;
                        btnEnviarAnalise.Visible = false;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        tseUnidadeEnsino.Mode = ControlMode.View;
                        tsePeriodoPrestacaoContas.Mode = ControlMode.View;
                        tsePlanoTrabalho.Mode = ControlMode.View;
                        tseOperacao.Mode = ControlMode.View;
                        tsePlanoTrabalho.Enabled = false;
                        tsePeriodoPrestacaoContas.Enabled = false;
                        tseUnidadeEnsino.Enabled = false;
                        txtJustificativa.Enabled = false;
                        tseOperacao.Enabled = false;                       
                        txtValor.Enabled = false;
                        if (hidStatus.Value == "1" || hidStatus.Value == "3" || hidStatus.Value == "4")
                            btnEnviarAnalise.Visible = false;
                        else
                            btnEnviarAnalise.Visible = true;
                        btnCancel.Visible = true;
                        btnNovo.Visible = false;
                        btnExcluir.Visible = false;
                        btnEditar.Visible = true;
                        btnSalvar.Visible = false;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        tseUnidadeEnsino.Mode = ControlMode.View;
                        tsePeriodoPrestacaoContas.Mode = ControlMode.View;
                        tsePlanoTrabalho.Mode = ControlMode.View;
                        tseOperacao.Mode = ControlMode.View;

                        pnlDocumento.Visible = false;
                        tsePlanoTrabalho.Enabled = true;
                        tsePeriodoPrestacaoContas.Enabled = true;
                        tseUnidadeEnsino.Enabled = true;
                        btnEnviarAnalise.Visible = false;
                        txtJustificativa.Enabled = true;
                        tseOperacao.Enabled = true;
                        txtValor.Enabled = true;
                        txtJustificativa.Text = string.Empty;
                        tseOperacao.Value = string.Empty;
                        txtValor.Text = string.Empty;
                        hdnPerfil.Value = "1";
                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                       
                         btnEnviarAnalise.Visible = false;
                  
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tseUnidadeEnsino.Mode = ControlMode.View;
                        tsePeriodoPrestacaoContas.Mode = ControlMode.View;
                        tsePlanoTrabalho.Mode = ControlMode.View;
                        tseOperacao.Mode = ControlMode.View;
                        tsePlanoTrabalho.Enabled = false;
                        tsePeriodoPrestacaoContas.Enabled = false;
                        tseUnidadeEnsino.Enabled = false;
                        tseOperacao.Enabled = false;
                        if (hidStatus.Value == "1" || hidStatus.Value == "3" || hidStatus.Value == "4")
                        { btnEnviarAnalise.Visible = false;
                          btnEditar.Visible = false;
                        }
                        else
                        {btnEnviarAnalise.Visible = true;
                        btnEditar.Visible = true;
                        }
         
                        if (hidStatus.Value == "0")
                            btnExcluir.Visible = true;
                        else
                            btnExcluir.Visible = false;

                        btnNovo.Visible = false;
                        btnCancel.Visible = true;
                        btnAnexar.Visible = false;

                        txtJustificativa.Visible = true;
                        txtValor.Visible = true;
                        txtJustificativa.Enabled = false;
                        txtValor.Enabled = false;

                        pnlDocumento.Enabled = true;
                        pnlDocumento.Visible = true;
                        pcCreditoDebito.Visible = true;

                        grdDocumento.Enabled = true;
                        lblDocumentoInserir.Visible = false;
                        TSeDocNecessario.Visible = false;
                        FileUpload1.Visible = false;

                        break;
                    }
            }
        }
        private void LimpaDadosInfoGerais()
        {
            lblStatus.Text = string.Empty;
            hidStatus.Value = string.Empty; 
            lblTxtStatus.Text = string.Empty;
    

        }

        protected void grdDocumento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
        }

        protected void grdDocumento_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {

                if (e.ButtonID == "btnExcluirDocumento")
                {
                    ValidacaoDados validacao = new ValidacaoDados();
                    RN.Ocorrencias.ArquivoOcorrencia rnArquivoOcorrencia = new Techne.Lyceum.RN.Ocorrencias.ArquivoOcorrencia();
                    lblMensagem.Text = "Documento excluído com sucesso.";
 
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;

            }

        }

        protected void repCarrossel_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (new ListItemType[] { ListItemType.Item, ListItemType.AlternatingItem }.Contains(e.Item.ItemType))
            {
                var tipoArquivo = ((System.Data.DataRowView)e.Item.DataItem)["TipoArquivo"].ToString();
                ((PlaceHolder)e.Item.FindControl("plaTipoPDF")).Visible = (tipoArquivo == "application/pdf");
                ((PlaceHolder)e.Item.FindControl("plaTipoImagem")).Visible = (tipoArquivo.StartsWith("image/"));
                ((PlaceHolder)e.Item.FindControl("plaSemArquivo")).Visible = (tipoArquivo != "application/pdf" && !tipoArquivo.StartsWith("image/"));
            }
        }

        protected void btnExcluirDoc_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Ocorrencias.ArquivoOcorrencia rnArquivoOcorrencia = new Techne.Lyceum.RN.Ocorrencias.ArquivoOcorrencia();
                int id = Convert.ToInt32(hdnArquivoId.Value);

                validacao = rnArquivoOcorrencia.ValidaRemocao(id, User.Identity.Name);

                if (validacao.Valido)
                {
                    rnArquivoOcorrencia.Remove(id, User.Identity.Name);
                     lblMensagem.Text = "Documento excluído com sucesso.";
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

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.PrestacaoContas.Entidades.OperacaoExigencia operacaoExigencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoExigencia();
                RN.PrestacaoContas.Entidades.OperacaoExigenciaArquivo operacaoExigenciaArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoExigenciaArquivo();
                ValidacaoDados validacao = new ValidacaoDados();
                ValidacaoDados validacaoArquivo = new ValidacaoDados();

                byte[] imageBytes = new byte[FileUpload2.PostedFile.InputStream.Length + 1];
                FileUpload2.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);

                operacaoExigencia.OperacaoExigenciaId = !hdnOperacaoExigenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOperacaoExigenciaId.Value) : -1;
                operacaoExigencia.Justificativa = TxtJustificativaExigencia.Text;
                operacaoExigencia.UsuarioId = User.Identity.Name;
                operacaoExigencia.DataAlteracao = DateTime.Now;

                operacaoExigenciaArquivo.OperacaoExigenciaId = !hdnOperacaoExigenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOperacaoExigenciaId.Value) : -1;
                operacaoExigenciaArquivo.ChaveArquivo = Guid.NewGuid().ToString();
                operacaoExigenciaArquivo.Arquivo = imageBytes;
                operacaoExigenciaArquivo.NomeArquivo = FileUpload2.PostedFile.FileName;
                operacaoExigenciaArquivo.TipoArquivo = FileUpload2.PostedFile.ContentType;
                operacaoExigenciaArquivo.UsuarioId = User.Identity.Name;
                operacaoExigenciaArquivo.DataCadastro = DateTime.Now;
                operacaoExigenciaArquivo.DataAlteracao = DateTime.Now;

                validacao = rnOperacaoExigencia.ValidaJustificativa(operacaoExigencia);
                if (!validacao.Valido)
                    lblMensagem.Text += (lblMensagem.Text.Length > 0 ? "<br />" : "") + validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                validacaoArquivo = rnOperacaoExigencia.ValidaArquvoExigencia(operacaoExigenciaArquivo);
                if (!validacaoArquivo.Valido)
                    lblMensagem.Text += (lblMensagem.Text.Length > 0 ? "<br />" : "") + validacaoArquivo.Mensagem.Replace(Environment.NewLine, "<br />");

                if (!validacao.Valido || !validacaoArquivo.Valido)
                    return;

                rnOperacaoExigencia.AtualizaJustificativa(operacaoExigencia);
                rnOperacaoExigencia.AtualizaArquivoExigencia(operacaoExigenciaArquivo);
                lblMensagem.Text = "Arquivo atualizado com sucesso.";
                TxtJustificativaExigencia.Text = "";

                grdExigencias.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var status = (int?)null;
                try { status = Convert.ToInt32(hidStatus.Value);
                lblStatus.Text = RetornaStatus(Convert.ToInt32(hidStatus.Value));
                lblTxtStatus.Text = "Status da Operação";
                }
                catch { status = null; }
               
                if (status != 2)
                    throw new Exception("A exigência ainda não está disponível para ser justificada"); 


                hdnOperacaoExigenciaId.Value = string.Empty;
                txtJustificativa.Text = string.Empty;

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                hdnOperacaoExigenciaId.Value = chave[0].ToString();

                pucConfirmarArquivo.ShowOnPageLoad = true;

                lblMensagem.Text = String.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAnexar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();
                RN.PrestacaoContas.Entidades.OperacaoDocumentos arquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoDocumentos();

                ValidacaoDados validacao = new ValidacaoDados();
                string mensagem = string.Empty;


                byte[] imageBytes = new byte[FileUpload1.PostedFile.InputStream.Length + 1];
                FileUpload1.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);

                arquivo.Arquivo = imageBytes;
                arquivo.OperacaoId = !hdnOcorrenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOcorrenciaId.Value) : -1;
                arquivo.NomeArquivo = FileUpload1.PostedFile.FileName;
                arquivo.TipoArquivo = FileUpload1.PostedFile.ContentType;
                arquivo.ChaveArquivo = Guid.NewGuid().ToString();
                arquivo.UsuarioId = User.Identity.Name;
                arquivo.DocumentosNecessariosOperacoesId = TSeDocNecessario.DBValue.IsNull || !TSeDocNecessario.IsValidDBValue ? 0 : Convert.ToInt32(TSeDocNecessario.DBValue);
           
                lblStatus.Text = RetornaStatus(Convert.ToInt32(hidStatus.Value));
                lblTxtStatus.Text = "Status da Operação";
                
                validacao = rnArquivoOcorrencia.Valida(arquivo);

                if (validacao.Valido)
                {
                    //Verifica duplicado
                    validacao = rnArquivoOcorrencia.ValidaArquivo(arquivo.OperacaoId, arquivo.DocumentosNecessariosOperacoesId);
                    if (validacao.Valido)
                    {
                        rnArquivoOcorrencia.Insere(arquivo);
                        lblMensagem.Text = "Documento incluído com sucesso.";
                        TSeDocNecessario.ResetValue();
                        odsDocumento.Select();
                        odsDocumento.DataBind();
                        grdDocumento.DataBind();
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                    //  repCarrossel.DataBind();
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

        protected void grdExigenciaEvento_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

            var btnDetalhes = (ImageButton)e.Row.Cells[3].Controls[0].Controls[1];
            ControlaAcesso(btnDetalhes, AcaoControle.editar);

        }


        protected void grdDocumento_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (this.grdDocumento.Visible || this.grdDocumento.VisibleRowCount != 0)
            {

                foreach (GridViewColumn col in grdDocumento.Columns)
                {
                    if (col is GridViewCommandColumn)
                    {
                        if (hidStatus.Value != "")
                        {
                            if (hidStatus.Value == "0")
                                ((GridViewCommandColumn)col).DeleteButton.Visible = true;
                            else
                                ((GridViewCommandColumn)col).DeleteButton.Visible = false;
                        }
                    }
                }

            }

        }


        public object UpdateDocumento(object id)
        {

            return null;
        }

        public object ListaDocumento(object id)
        {
            RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();

            if (id != null)
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    return rnArquivoOcorrencia.ListaPor(Convert.ToInt32(id));
                }
            }
            return null;
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnAnalisar, AcaoControle.editar);
            ControlaAcesso(btnEnviarAnalise, AcaoControle.editar);
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnImportar, AcaoControle.editar);
            ControlaAcesso(grdExigencias, AcaoControle.editar, "btnDetalhes");
        }
        protected void grdExigencias_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                var btnDetalhes = (ImageButton)e.Row.Cells[4].Controls[0].Controls[1];
                ControlaAcesso(btnDetalhes, AcaoControle.editar);
            }
            catch
            {
            }
        }
        protected void grdExigencias_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                
                RN.PrestacaoContas.OperacaoExigencia rnOperacaoExigencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoExigencia();
                RN.PrestacaoContas.Entidades.OperacaoExigencia operacaoExigencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoExigencia();

                operacaoExigencia.OperacaoExigenciaId = Convert.ToInt32(e.Keys["OPERACAOEXIGENCIAID"]);
                operacaoExigencia.TipoExigenciaOperacaoId = Convert.ToInt32(e.NewValues["TIPOEXIGENCIAOPERACAOID"]);
                operacaoExigencia.Justificativa = e.NewValues["JUSTIFICATIVA"] != null ? e.NewValues["JUSTIFICATIVA"].ToString().Trim().ToUpper() : null;
                operacaoExigencia.NotaExplicativa = e.NewValues["NOTAEXPLICATIVA"] != null ? e.NewValues["NOTAEXPLICATIVA"].ToString().Trim().ToUpper() : null;
                operacaoExigencia.OperacaoId = Convert.ToInt32(hdnOcorrenciaId.Value);
                operacaoExigencia.Aprovado = e.NewValues["APROVADO"] == null || e.NewValues["APROVADO"] == "0" ? 0 : 1;
                operacaoExigencia.UsuarioId = User.Identity.Name;

                validacao = rnOperacaoExigencia.Valida(operacaoExigencia, 0);

                if (validacao.Valido)
                {
                    rnOperacaoExigencia.Atualiza(operacaoExigencia);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdExigencias.DataBind();
            }
            catch
            {
                return;
            }
        }
        protected void grdExigencias_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.PrestacaoContas.Entidades.OperacaoExigencia operacaoExigencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoExigencia();

                RN.PrestacaoContas.Entidades.TipoExigenciaOperacao tipoExigenciaOperacao = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoExigenciaOperacao();
                RN.PrestacaoContas.OperacaoExigencia rnOperacaoExigencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoExigencia();
                operacaoExigencia.TipoExigenciaOperacaoId = Convert.ToInt32(e.NewValues["TIPOEXIGENCIAOPERACAOID"]);
                operacaoExigencia.Justificativa = e.NewValues["JUSTIFICATIVA"] != null ? e.NewValues["JUSTIFICATIVA"].ToString().Trim().ToUpper() : null;
                operacaoExigencia.NotaExplicativa = e.NewValues["NOTAEXPLICATIVA"] != null ? e.NewValues["NOTAEXPLICATIVA"].ToString().Trim().ToUpper() : null;
                operacaoExigencia.OperacaoId = Convert.ToInt32(hdnOcorrenciaId.Value);
                operacaoExigencia.Aprovado = e.NewValues["APROVADO"] == null || e.NewValues["APROVADO"] == "0" ? 0 : 1;
                operacaoExigencia.UsuarioId = User.Identity.Name;

                validacao = rnOperacaoExigencia.Valida(operacaoExigencia,0);

                if (validacao.Valido)
                {
                    rnOperacaoExigencia.Insere(operacaoExigencia);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdExigencias.DataBind();
            }
            catch
            {
                return;
            }
        }
        protected void grdExigenciaExtrato_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var ee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoExigencia();

                ee.OperacaoExigenciaId = Convert.ToInt32(grdExigencias.GetRowValues(grdExigencias.EditingRowVisibleIndex, "OPERACAOEXTRATOID"));
                ee.TipoExigenciaOperacaoId = Convert.ToInt32(e.NewValues["TIPOEXIGENCIAOPERACAOID"]);
                ee.NotaExplicativa = Convert.ToString(e.NewValues["NOTAEXPLICATIVA"]);
                ee.UsuarioId = User.Identity.Name;
                ee.DataAlteracao = DateTime.Now;

                validacao = rnOperacaoExigencia.Valida(ee, 0);

                if (validacao.Valido)
                {
                    rnOperacaoExigencia.Atualiza(ee);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdExigencias.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    
        protected void grdExigencias_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.PrestacaoContas.MotivoExigenciaCredDeb rnMotivoExigenciaCredDeb = new Techne.Lyceum.RN.PrestacaoContas.MotivoExigenciaCredDeb();
                int tipoexigenciaoperacaoid = 0;

                tipoexigenciaoperacaoid = Convert.ToInt32(e.Keys["OPERACAOEXIGENCIAID"]);

                validacao = rnMotivoExigenciaCredDeb.ValidaRemocao(tipoexigenciaoperacaoid);

                if (validacao.Valido)
                {
                    rnMotivoExigenciaCredDeb.RemoveExigencia(tipoexigenciaoperacaoid);
                    grdExigencias.DataBind();
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch
            {
                return;
            }
        }
        public object ListaTipoExigencias()
        {
            try
            {
                RN.PrestacaoContas.TipoExigenciaOperacao rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.TipoExigenciaOperacao();

                return rnArquivoOcorrencia.ListaAtivo();
            }
            catch
            {
                return null;
            }
        }
        protected void grdExigencias_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            try
            {
                if (this.grdExigencias.IsNewRowEditing)
                {
                    if (e.Column.FieldName == "OPERACAOID")
                    {
                        e.Editor.ReadOnly = true;
                    }
                    if (e.Column.FieldName == "btnDetalhes")
                    {
                        e.Editor.ReadOnly = true;
                    }
                    if (e.Column.FieldName == "TIPOEXIGENCIAOPERACAOID")
                    {
                        e.Editor.ReadOnly = false;
                    }
                    if (e.Column.FieldName == "JUSTIFICATIVA")
                    {
                        e.Editor.ReadOnly = false;
                    }
                    if (e.Column.FieldName == "NOTAEXPLICATIVA")
                    {
                        e.Editor.ReadOnly = true;
                    }
                    if (e.Column.FieldName == "APROVADO")
                    {
                        e.Editor.ReadOnly = false;
                    }
                }
                else if (this.grdExigencias.IsEditing)
                {
                    if (e.Column.FieldName == "OPERACAOID")
                    {
                        e.Editor.Enabled = true;
                    }
                    if (e.Column.FieldName == "ANEXO")
                    {
                        e.Editor.Enabled = true;
                    }
                    if (e.Column.FieldName == "btnDetalhes")
                    {
                      //  e.Editor.Enabled = true;
                        e.Editor.ReadOnly = false;
                    }
                    if (e.Column.FieldName == "TIPOEXIGENCIAOPERACAOID")
                    {
                        e.Editor.ReadOnly = true;
                    }
                    if (e.Column.FieldName == "JUSTIFICATIVA")
                    {
                        e.Editor.ReadOnly = false;
                    }
                    if (e.Column.FieldName == "NOTAEXPLICATIVA")
                    {
                        e.Editor.Enabled = true;
                    }
                    if (e.Column.FieldName == "APROVADO")
                    {
                        e.Editor.ReadOnly = false;
                    }
                }

            }
            catch
            {
                return;
            }
        }
        protected void grdExigencias_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["APROVADO"] = true;
            grdExigencias.Settings.ShowFilterRow = true;
        }
        protected void grdExigencias_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdExigencias.Settings.ShowFilterRow = false;
        }

        public object ListaExigencias(object id)
        {
            RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();

            if (id != null)
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    return rnArquivoOcorrencia.ListaExigenciasPorId(Convert.ToInt32(id));
                }
            }
            return null;
        }
        protected void tseOperacao_Changed(object sender, EventArgs e)
        {
            try
            {
                if (!tseOperacao.IsValidDBValue)
                {

                    return;
                }

                var mensagens = new List<string>();

                if (tsePeriodoPrestacaoContas.Value == null || !tsePeriodoPrestacaoContas.IsValidDBValue)
                    mensagens.Add("PERÍODO DE PRESTAÇÃO DE CONTAS: Seleção obrigatória");

                if (tseUnidadeEnsino.Value == null || !tseUnidadeEnsino.IsValidDBValue)
                    mensagens.Add("UNIDADE DE ENSINO: Seleção obrigatória");

                if (tsePlanoTrabalho.Value == null || !tsePlanoTrabalho.IsValidDBValue)
                    mensagens.Add("PROJETO/PROGRAMA: Seleção obrigatória");

                if (mensagens.Any())
                {
                    lblMensagem.Text = mensagens.Distinct().Aggregate((x, y) => x + "<br />" + y);
                    return;
                }
               
                try
                {
                    var operacao = rnOperacao.ObtemPor(Convert.ToInt32(tseOperacao.Value));
                    hdnOcorrenciaId.Value = operacao.OperacaoId.ToString();
                    txtValor.Text = operacao.Valor.ToString();
                    txtJustificativa.Text = operacao.Justificativa.ToString();
                    ddlOperacao.Text = operacao.TipoOperacao.ToString();
                    lblStatus.Text = RetornaStatus(operacao.Status);
                    lblTxtStatus.Text = "Status da Operação";
                    hidStatus.Value = operacao.Status.ToString();
                }
                catch
                {
                    var operacao = rnOperacao.ObtemPor(tseOperacao.Value.ToString());
                    hdnOcorrenciaId.Value = operacao.OperacaoId.ToString();
                    txtValor.Text = operacao.Valor.ToString();
                    txtJustificativa.Text = operacao.Justificativa.ToString();
                    ddlOperacao.Text = operacao.TipoOperacao.ToString();
                    lblStatus.Text = RetornaStatus(operacao.Status);
                    lblTxtStatus.Text = "Status da Operação";
                    hidStatus.Value = operacao.Status.ToString();
                }


 
                lblTxtStatus.Text = "Status da Operação:";
                odsDocumento.Select();
                odsDocumento.DataBind();
                grdDocumento.DataBind();

                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private string RetornaStatus(int status)
        {
            string ret = "";
            if (status == 0) ret = "Lançado pela AAE";
            if (status == 1) ret = "Enviado para Análise";
            if (status == 2) ret = "Devolvido para Revisão";
            if (status == 3) ret = "Aprovado";
            if (status == 4) ret = "Reprovado - " + rnOperacao.RetornaStatusReprovado(Convert.ToInt32(hdnOcorrenciaId.Value));;

            return ret;
        }
        protected void grdDocumento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

            RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();
            var id = Convert.ToInt32(e.Keys["OPERACAODOCUMENTOSID"]);
            var validacao = rnArquivoOcorrencia.ValidaRemocao(Convert.ToInt32(id));

            if (validacao.Valido)
            {
                rnArquivoOcorrencia.Remove(id);
                grdDocumento.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
        protected void odsDocumento_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
         /*   try
            {
                RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();
                var id = e.InputParameters["OPERACAODOCUMENTOSID"].ToString();
                var validacao = rnArquivoOcorrencia.ValidaRemocao(Convert.ToInt32(id));

                if (!validacao.Valido)
                {
                    lblMensagem.Text = validacao.Mensagem;
                    return;
                }
              //  else
              //      rnArquivoOcorrencia.Remove(int.Parse(id));
    
                odsDocumento.Select();
                odsDocumento.DataBind();
                grdDocumento.DataBind();
                grdDocumento.FocusedRowIndex = -1;
       

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }*/
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();
                        LimpaDadosInfoGerais();
                        pcCreditoDebito.Visible = false;
                        pnlDocumento.Visible = false;
                        pcCreditoDebito.ActiveTabIndex = 0;
                        hdnPerfil.Value = "0";
                        lblMensagem.Text = String.Empty;
                        lblTxtStatus.Text = String.Empty;

                        pnlFiltro.Enabled = true;

                        btnCancel.Visible = false;
                        btnEditar.Visible = false;
                        btnNovo.Visible = true;
                        btnSalvar.Visible = false;
                        btnEnviarAnalise.Visible = false;
                        btnAnalisar.Visible = false;

                        ddlOperacao.Text = string.Empty;
                        txtValor.Text = string.Empty;
                        txtJustificativa.Text = string.Empty;
  
                        pcCreditoDebito.Visible = false;
                        pnlDocumento.Visible = false;

                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles, null);
                         tsePeriodoPrestacaoContas.Enabled = false;
                         tsePlanoTrabalho.Enabled = false;
                         tseUnidadeEnsino.Enabled = false;
                         tseOperacao.Enabled = false;
                         pcCreditoDebito.Visible = true;
                         grdDocumento.Enabled = true;
                         pnlDocumento.Enabled = true;

                        btnCancel.Visible = true;
                        btnEditar.Visible = false;
                        btnNovo.Visible = false;          
                        btnSalvar.Visible = true;
                        btnEnviarAnalise.Visible = false;
                        btnAnalisar.Visible = false;
                        //pnlDocumento.Visible = false;

                        pnlFiltro.Enabled = false;

                        ControlarTSearchs();
                        pcCreditoDebito.Visible = true;
                        pcCreditoDebito.ActiveTabIndex = 0;
                        grdDocumento.Enabled = true;
                        pnlDocumento.Enabled = true;

                        LimpaDadosInfoGerais();
                
      

                        //hdnPerfil.Value = "1";
                        ddlOperacao.Text = string.Empty;
                        txtValor.Text = string.Empty;
                        txtJustificativa.Text = string.Empty;
                        hdnOcorrenciaId.Value = null;
                        pcCreditoDebito.ActiveTabIndex = 0;
                        HabilitaDesabilitaCamposAbaInformacoesGerais(true);
                        for (int i = 1; i < pcCreditoDebito.TabPages.Count; i++)
                        {
                            pcCreditoDebito.TabPages[i].Enabled = true;
                        }
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { };

                         ControlarTSearchs();
                         btnExcluir.Visible = true;
                    
                        hdnPerfil.Value = "0";
                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        tseUnidadeEnsino.Enabled = true;
                        tsePlanoTrabalho.Enabled = true;
                        tsePeriodoPrestacaoContas.Enabled = true;
                        tseOperacao.Enabled = true;

                        tseUnidadeEnsino.Value = null;
                        tsePlanoTrabalho.Value = null;
                        tsePeriodoPrestacaoContas.Value = null;
                        tseOperacao.Value = null;
                        pnlFiltro.Enabled = true;

                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();
                        LimpaDadosInfoGerais();                        
                        pcCreditoDebito.Visible = false;
                        pnlDocumento.Visible = false;                        
                        pcCreditoDebito.ActiveTabIndex = 0;
                        hdnPerfil.Value = "0";
                        hdnOcorrenciaId.Value = null;
                        hdnOcorrenciaId.Value = null;
                        lblStatus.Text = String.Empty; 
                        lblMensagem.Text = String.Empty;
                        lblTxtStatus.Text = String.Empty;
                        btnCancel.Visible = false;
                        btnEditar.Visible = false;
                        btnNovo.Visible = true;
                        btnSalvar.Visible = false;
                        btnEnviarAnalise.Visible = false;
                        btnAnalisar.Visible = false;
                        pcCreditoDebito.Visible = false;
                        pnlDocumento.Visible = false;

                        _tipoOperacao = TipoOperacao.Inicial;
                        break;
                    }   
                case TipoOperacao.SucessoAnalise:
                    {
                        ImageButton[] controles = new ImageButton[] { };

                        controles = new[] { btnNovo, btnEditar };
                        hdnPerfil.Value = "0";
                        ControlarVisibilidadeControle(controles, null);
                        pcCreditoDebito.ActiveTabIndex = 0;

                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        tseUnidadeEnsino.Enabled = true;
                        tsePlanoTrabalho.Enabled = true;
                        tsePeriodoPrestacaoContas.Enabled = true;
                        tseOperacao.Enabled = true;

                        tseUnidadeEnsino.Value = null;
                        tsePlanoTrabalho.Value = null;
                        tsePeriodoPrestacaoContas.Value = null;
                        tseOperacao.Value = null;
                        pnlFiltro.Enabled = true;

                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();
                        LimpaDadosInfoGerais();
                        pcCreditoDebito.Visible = false;
                        pnlDocumento.Visible = false;
                        pcCreditoDebito.ActiveTabIndex = 0;
                        hdnPerfil.Value = "0";
                        hdnOcorrenciaId.Value = null;
                        hdnOcorrenciaId.Value = null;
                        lblStatus.Text = String.Empty;
                        //lblMensagem.Text = String.Empty;
                        lblTxtStatus.Text = String.Empty;
                        btnCancel.Visible = false;
                        btnEditar.Visible = false;
                        btnNovo.Visible = true;
                        btnSalvar.Visible = false;
                        btnEnviarAnalise.Visible = false;
                        btnAnalisar.Visible = false;
                        pcCreditoDebito.Visible = false;
                        pnlDocumento.Visible = false;

                        _tipoOperacao = TipoOperacao.Inicial;
                        break;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ControlarTSearchs();
                        pcCreditoDebito.Visible = true;
                        pcCreditoDebito.ActiveTabIndex = 0;
                        grdDocumento.Enabled = true;
                        pnlDocumento.Enabled = true;
                        pnlFiltro.Enabled = false;

                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        if (hidStatus.Value == "1" || hidStatus.Value == "3" || hidStatus.Value == "4")
                            btnEnviarAnalise.Visible = false;
                        else
                            btnEnviarAnalise.Visible = true;
                        hdnPerfil.Value = "1";
                        break;
                    }
            }
        }
    }
}
