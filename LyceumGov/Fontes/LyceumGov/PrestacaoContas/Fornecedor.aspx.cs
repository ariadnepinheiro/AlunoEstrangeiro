using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Lyceum.RN.Util;
using Techne.Controls;
using System.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
     NavUrl("~/PrestacaoContas/Fornecedor.aspx"),
     ControlText("Fornecedor"),
     Title("Fornecedor"),
    ]
    public partial class Fornecedor : TPage
    {
        RN.PrestacaoContas.Fornecedor rnFornecedor = new RN.PrestacaoContas.Fornecedor();
        RN.PrestacaoContas.FornecedorAnalise rnFornecedorAnalise = new RN.PrestacaoContas.FornecedorAnalise();
        RN.PrestacaoContas.FornecedorProdutoServicoGrupo rnFornecedorProdutoServicoGrupo = new RN.PrestacaoContas.FornecedorProdutoServicoGrupo();
        RN.PrestacaoContas.FornecedorRazaoSocial rnFornecedorRazaoSocial = new RN.PrestacaoContas.FornecedorRazaoSocial();
        RN.PrestacaoContas.FornecedorRepresentanteLegal rnFornecedorRepresentanteLegal = new Techne.Lyceum.RN.PrestacaoContas.FornecedorRepresentanteLegal();
        RN.PrestacaoContas.DocumentosFornecedor rnDocumentosFornecedor = new Techne.Lyceum.RN.PrestacaoContas.DocumentosFornecedor();
        RN.PrestacaoContas.FornecedorDocumentoArquivo rnFornecedorDocumentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.FornecedorDocumentoArquivo();

        RN.PrestacaoContas.ProdutoServicoGrupo rnProdutoServicoGrupo = new Techne.Lyceum.RN.PrestacaoContas.ProdutoServicoGrupo();

        public enum TipoOperacao
        {
            Novo,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            SucessoAnalise,
            Excluir,
            Desativar
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

        public object ListaHistoricoAnalise(object IDFORNECEDOR)
        {
            var id = IDFORNECEDOR != null ? Convert.ToInt32(IDFORNECEDOR) : -1;

            if (id != -1)
            {
                return rnFornecedorAnalise.ListaPor(id);
            }

            return null;
        }



        protected void Page_Load(object sender, EventArgs e)
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
                        tseFornecedorPrestacao.DBValue = decodedText;
                        tseFornecedorPrestacao_Changed(null, null);
                    }
                    else
                    {
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();
                    }
                }

                int fornecedorId;
                if (int.TryParse((tseFornecedorPrestacao.Value ?? "").ToString(), out fornecedorId))
                    FornecedorPopup1.FornecedorId = fornecedorId;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdFornecedorProdutoServicoGrupo, "");
            TituloGrid(grdDocumento, "");
            TituloGrid(grdHistoricoRazaoSocial, "");
            TituloGrid(grdRepresentante, "");
            TituloGrid(grdHistoricoAnalise, "");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdFornecedorProdutoServicoGrupo);
            ControlaAcesso(grdDocumento);
            ControlaAcesso(grdHistoricoRazaoSocial);
            ControlaAcesso(grdRepresentante);
            ControlaAcesso(grdHistoricoAnalise);
            ControlaAcesso(grdDocumento, AcaoControle.editar, "btnDetalhes");
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
                        tseFornecedorPrestacao.ResetValue();
                        pcFornecedor.Visible = false;
                        pcFornecedor.ActiveTabIndex = 0;
                        
                        lnkAlteracaoSenha.Visible = lblAlteracaoSenha.Visible = false;
                        lnkConfirmacaoCadastro.Visible = lblConfirmacaoCadastro.Visible = false;

                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();

                        LimpaDadosInfoGerais();
                        grdDocumento.DataBind();
                        repCarrossel.DataBind();
                        lblSituacao.Text = "";
                        lnkAlteracaoSenha.Visible = lblAlteracaoSenha.Visible = false;
                        lnkConfirmacaoCadastro.Visible = lblConfirmacaoCadastro.Visible = false;
                        tseFornecedorPrestacao.ResetValue();
                        tseFornecedorPrestacao.Enabled = false;
                        pcFornecedor.Visible = true;
                        pcFornecedor.ActiveTabIndex = 0;
                        HabilitaDesabilitaCamposAbaInformacoesGerais(true);

                        rblTipo.SelectedValue = "Pessoa Jurídica";
                        txtCnpj.Visible = true;
                        txtCPF.Visible = false;
                        txtInscricao.Visible = lblInscricaoEstadual.Visible = true;
                        txtInscricaoMunicipal.Visible = lblInscricaoMunicipal.Visible = true;

                        for (int i = 1; i < pcFornecedor.TabPages.Count; i++)
                        {
                            pcFornecedor.TabPages[i].Enabled = false;
                        }

                        lnkAlteracaoSenha.Visible = lblAlteracaoSenha.Visible = false;
                        lnkConfirmacaoCadastro.Visible = lblConfirmacaoCadastro.Visible = false;

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { };

                        controles = new[] { btnNovo, btnEditar };

                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();

                        for (int i = 1; i < pcFornecedor.TabPages.Count; i++)
                        {
                            pcFornecedor.TabPages[i].Enabled =
                                (rblTipo.SelectedValue == "Pessoa Física" && new int[] { 0, 1, 4, 5 }.Contains(i)) ||
                                (rblTipo.SelectedValue == "Pessoa Jurídica" && new int[] { 0, 1, 2, 3, 4, 5 }.Contains(i));
                        }
                        
                        HabilitaDesabilitaCamposAbaInformacoesGerais(false);

                        var currentTab = pcFornecedor.ActiveTabIndex;
                        _tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                        pcFornecedor.ActiveTabIndex = currentTab;
                        
                        lnkAlteracaoSenha.Visible = lblAlteracaoSenha.Visible = false;
                        lnkConfirmacaoCadastro.Visible = lblConfirmacaoCadastro.Visible = false;

                        break;
                    }
                case TipoOperacao.SucessoAnalise:
                    {
                        ImageButton[] controles = new ImageButton[] { };

                        controles = new[] { btnNovo, btnEditar };

                        ControlarVisibilidadeControle(controles, null);

                        lblSituacao.Text = rnFornecedor.ObtemSituacaoPor(Convert.ToInt32(tseFornecedorPrestacao.DBValue)).Replace("\r\n", "<br />");
                        pcFornecedor.ActiveTabIndex = 0;

                        lnkAlteracaoSenha.Visible = lblAlteracaoSenha.Visible = false;
                        lnkConfirmacaoCadastro.Visible = lblConfirmacaoCadastro.Visible = false;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        RN.PrestacaoContas.Entidades.Fornecedor fornecedor = new Techne.Lyceum.RN.PrestacaoContas.Entidades.Fornecedor();
                        lnkAlteracaoSenha.Visible = lblAlteracaoSenha.Visible = false;
                        lnkConfirmacaoCadastro.Visible = lblConfirmacaoCadastro.Visible = false;
                        ControlarTSearchs();
                        pcFornecedor.Visible = true;
                        pcFornecedor.ActiveTabIndex = 0;

                        for (int i = 1; i < pcFornecedor.TabPages.Count; i++)
                        {
                            pcFornecedor.TabPages[i].Enabled =
                                (rblTipo.SelectedValue == "Pessoa Física" && new int[] { 0, 1, 4, 5 }.Contains(i)) ||
                                (rblTipo.SelectedValue == "Pessoa Jurídica" && new int[] { 0, 1, 2, 3, 4, 5 }.Contains(i));
                        }

                        LimpaDadosInfoGerais();
                        HabilitaDesabilitaCamposAbaInformacoesGerais(false);

                        var dadosFornecedor = rnFornecedor.ObtemPor(Convert.ToInt32(tseFornecedorPrestacao.DBValue));

                        if (dadosFornecedor.FornecedorId.HasValue)
                        {
                            hdnFinalizado.Value = Convert.ToString(dadosFornecedor.Finalizado);
                            hdnEnviado.Value = dadosFornecedor.Enviado && dadosFornecedor.Enviado ? "true" : string.Empty;
                            rblTipo.SelectedValue = !dadosFornecedor.Tipo.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.Tipo : string.Empty;
                            txtCnpj.Text = !dadosFornecedor.Cnpj.IsNullOrEmptyOrWhiteSpace() && rblTipo.SelectedValue == "Pessoa Física" ? dadosFornecedor.Cnpj.AplicarMascaraCPF() : dadosFornecedor.Cnpj.AplicarMascaraCNPJ();
                            txtCPF.Text = !dadosFornecedor.Cnpj.IsNullOrEmptyOrWhiteSpace() && rblTipo.SelectedValue == "Pessoa Física" ? dadosFornecedor.Cnpj.AplicarMascaraCPF() : dadosFornecedor.Cnpj.AplicarMascaraCNPJ();
                            txtRazaoSocial.Text = !dadosFornecedor.RazaoSocial.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.RazaoSocial.ToUpper() : string.Empty;
                            chkGrandePorte.Checked = dadosFornecedor.GrandePorte;
                            chkEventual.Checked = dadosFornecedor.Eventual;
                            txtInscricao.Text = !dadosFornecedor.InscricaoEstadual.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.InscricaoEstadual : string.Empty;
                            txtInscricaoMunicipal.Text = !dadosFornecedor.InscricaoMunicipal.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.InscricaoMunicipal : string.Empty;
                            txtCEP.Text = !dadosFornecedor.Cep.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.Cep : string.Empty;
                            txtEndereco.Text = !dadosFornecedor.Endereco.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.Endereco.ToUpper() : string.Empty;
                            txtEndCompl.Text = !dadosFornecedor.Complemento.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.Complemento : string.Empty;
                            txtEndNum.Text = !dadosFornecedor.Numero.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.Numero : string.Empty;
                            txtBairro.Text = !dadosFornecedor.Bairro.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.Bairro.ToUpper() : string.Empty;
                            txtEmail.Text = !dadosFornecedor.Email.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.Email : string.Empty;
                            tseMunicipio.DBValue = !dadosFornecedor.MunicipioId.IsNullOrEmptyOrWhiteSpace() ? dadosFornecedor.MunicipioId : string.Empty;
                            tseMunicipio_Changed(null, null);
                            
                            txtCPF.Visible = rblTipo.SelectedValue == "Pessoa Física" ? true : false;
                            txtCnpj.Visible = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;

                            lblInscricaoEstadual.Visible = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;
                            lblInscricaoMunicipal.Visible = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;
                            txtInscricaoMunicipal.Visible = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;
                            txtInscricao.Visible = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;

                            pcFornecedor.TabPages[2].Enabled = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;
                            pcFornecedor.TabPages[3].Enabled = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;

                            txtTelefone.Text = dadosFornecedor.Telefone.RetirarMascaraTelefone().AplicarMascaraCelularComDDD();

                            lblSituacao.Text = dadosFornecedor.Situacao.Replace("\r\n", "<br />");
                        }

                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, hdnEnviado.Value.IsNullOrEmptyOrWhiteSpace() ? btnEnviarAnalise : (hdnEnviado.Value == "true" && hdnFinalizado.Value.IsNullOrEmptyOrWhiteSpace()) ? btnAnalisar : btnNovo };
                        ControlarVisibilidadeControle(controles, null);

                        if (lblSituacao.Text == "Aguardando envio para análise")
                        {
                            lnkAlteracaoSenha.Visible = lblAlteracaoSenha.Visible = true;
                            lnkConfirmacaoCadastro.Visible = lblConfirmacaoCadastro.Visible = true;
                        }
                        else
                        {
                            lnkAlteracaoSenha.Visible = lblAlteracaoSenha.Visible = false;
                            lnkConfirmacaoCadastro.Visible = lblConfirmacaoCadastro.Visible = false;
                        }

                        break;

                    }
                case TipoOperacao.Alterar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                        Button[] controlesButton = new Button[] { };

                        ControlarVisibilidadeControle(controles, controlesButton);
                        ControlarTSearchs();
                        HabilitaDesabilitaCamposAbaInformacoesGeraisParaAlteracao(true);

                        lnkAlteracaoSenha.Visible = lblAlteracaoSenha.Visible = false;
                        lnkConfirmacaoCadastro.Visible = lblConfirmacaoCadastro.Visible = false;

                        for (int i = 1; i < pcFornecedor.TabPages.Count; i++)
                        {
                            pcFornecedor.TabPages[i].Enabled = false;
                        }

                        break;

                    }

            }
        }

        protected void lnkAlteracaoSenha_Click(object sender, EventArgs e)
        {
            try
            {
                RN.PrestacaoContas.Fornecedor rnFornecedor = new Techne.Lyceum.RN.PrestacaoContas.Fornecedor();

                var userName =
                        rblTipo.SelectedValue == "Pessoa Jurídica" ? txtCnpj.Text :
                        (rblTipo.SelectedValue == "Pessoa Física" ? txtCPF.Text :
                        null);

                if (!rnFornecedor.ExistePor(userName))
                {
                    lblMensagem.Text = "Para disparar o e-mail de redefinição de senha, o usuário precisa estar cadastrado no banco de dados.";
                    return;
                }

                string result = rnFornecedor.EnviaEmailRedefinicaoSenha(userName);
                if (result.Contains(userName))
                    lblMensagem.Text = "E-mail de redefinição de senha disparado com sucesso para o e-mail do usuário.";
                else
                    lblMensagem.Text = "Erro ao disparar o e-mail de redefinição de senha:<br />" + result;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao disparar o e-mail de redefinição de senha:<br />" + ex.Message;
            }
        }

        protected void lnkConfirmacaoCadastro_Click(object sender, EventArgs e)
        {
            try
            {
                RN.PrestacaoContas.Fornecedor rnFornecedor = new Techne.Lyceum.RN.PrestacaoContas.Fornecedor();

                var userName =
                        rblTipo.SelectedValue == "Pessoa Jurídica" ? txtCnpj.Text :
                        (rblTipo.SelectedValue == "Pessoa Física" ? txtCPF.Text :
                        null);

                if (!rnFornecedor.ExistePor(userName))
                {
                    lblMensagem.Text = "Para disparar o e-mail de confirmação, o usuário precisa estar cadastrado no banco de dados.";
                    return;
                }

                string result = rnFornecedor.EnviaEmailConfirmacao(userName);
                if (result.Contains(userName))
                    lblMensagem.Text = "E-mail de confirmação disparado com sucesso para o e-mail do usuário.";
                else
                    lblMensagem.Text = "Erro ao disparar o e-mail de confirmação:<br />" + result;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao disparar o e-mail de confirmação:<br />" + ex.Message;
            }
        }

        protected bool Finalizado
        {
            get
            {
                return hdnFinalizado.Value.ToLower() == "true";
            }
        }

        private void HabilitaDesabilitaCamposAbaInformacoesGeraisParaAlteracao(bool habilita)
        {
            rblTipo.Enabled = false;

            if (!Finalizado)
            {
                txtCnpj.Enabled = habilita;
                txtCPF.Enabled = habilita;
            }
            else
            {
                txtCnpj.Enabled = false;
                txtCPF.Enabled = false;
            }

            txtRazaoSocial.Enabled = habilita;
            txtInscricao.Enabled = habilita;
            txtInscricaoMunicipal.Enabled = habilita;
            txtCEP.Enabled = habilita;
            txtEstado.Attributes["readonly"] = (habilita ? "" : "readonly");
            tseMunicipio.Mode = habilita ? ControlMode.Edit : ControlMode.View;
            tsCEP.Visible = habilita;
            txtEndereco.Enabled = habilita;
            txtEndNum.Enabled = habilita;
            txtEndCompl.Enabled = habilita;
            txtEmail.Enabled = habilita;
            txtTelefone.Enabled = habilita;
            txtBairro.Enabled = habilita;
            chkGrandePorte.Enabled = habilita;
            chkEventual.Enabled = habilita;
        }

        private void HabilitaDesabilitaCamposAbaInformacoesGerais(bool habilita)
        {
            rblTipo.Enabled = habilita;
            txtCnpj.Enabled = habilita;
            txtCPF.Enabled = habilita;
            txtRazaoSocial.Enabled = habilita;
            txtInscricao.Enabled = habilita;
            txtInscricaoMunicipal.Enabled = habilita;
            txtCEP.Enabled = habilita;
            txtEstado.Attributes["readonly"] = (habilita ? "" : "readonly");
            tseMunicipio.Mode = habilita ? ControlMode.Edit : ControlMode.View;
            tsCEP.Visible = habilita;
            txtEndereco.Enabled = habilita;
            txtEndNum.Enabled = habilita;
            txtEndCompl.Enabled = habilita;
            txtEmail.Enabled = habilita;
            txtTelefone.Enabled = habilita;
            txtBairro.Enabled = habilita;
            chkGrandePorte.Enabled = habilita;
            chkEventual.Enabled = habilita;
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
                grdDocumento.Columns[9].Visible = false;
            }

        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
            btnEnviarAnalise.Visible = false;
            btnAnalisar.Visible = false;
        }

        private void LimpaDadosInfoGerais()
        {
            txtMunicipio.Text = string.Empty;
            tseMunicipio.ResetValue();
            txtCnpj.Text = string.Empty;
            txtRazaoSocial.Text = string.Empty;
            txtInscricao.Text = string.Empty;
            txtInscricaoMunicipal.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtEndereco.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            hdnFornecedorDocumentoId.Value = string.Empty;
            hdnFornecedorId.Value = string.Empty;
            hdnFinalizado.Value = string.Empty;
            hdnFornecedorTipoDocumentoId.Value = string.Empty;
            chkGrandePorte.Checked = false;
            chkEventual.Checked = false;
            rblTipo.ClearSelection();

        }

        protected void tseMunicipio_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (tseMunicipio.IsValidDBValue
                    && !tseMunicipio.DBValue.IsNull)
                {
                    txtEstado.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
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
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
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
                _tipoOperacao = TipoOperacao.Novo;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            var dadosFornecedor = new RN.PrestacaoContas.DTOs.DadosFornecedor();
            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;
            
            try
            {
                dadosFornecedor.FornecedorId = !this.tseFornecedorPrestacao.DBValue.IsNull && tseFornecedorPrestacao.IsValidDBValue ? (int?)Convert.ToInt32(tseFornecedorPrestacao["IDFORNECEDOR"]) : null;
                dadosFornecedor.Tipo = !rblTipo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblTipo.SelectedValue : null;
                if (!dadosFornecedor.Tipo.IsNullOrEmptyOrWhiteSpace())
                {
                    if (dadosFornecedor.Tipo == "Pessoa Jurídica")
                    {
                        dadosFornecedor.Cnpj = !txtCnpj.Text.IsNullOrEmptyOrWhiteSpace() ? txtCnpj.Text.RetirarMascaraCNPJ() : null;
                    }
                    else
                    {
                        dadosFornecedor.Cnpj = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.RetirarMascaraCPF() : null;
                    }
                }

                dadosFornecedor.RazaoSocial = !txtRazaoSocial.Text.IsNullOrEmptyOrWhiteSpace() ? txtRazaoSocial.Text.Trim().ToUpper() : null;
                dadosFornecedor.InscricaoEstadual = !txtInscricao.Text.IsNullOrEmptyOrWhiteSpace() ? txtInscricao.Text.Trim() : null;
                dadosFornecedor.InscricaoMunicipal = !txtInscricaoMunicipal.Text.IsNullOrEmptyOrWhiteSpace() ? txtInscricaoMunicipal.Text.Trim() : null;
                dadosFornecedor.Cep = !txtCEP.Text.IsNullOrEmptyOrWhiteSpace() ? txtCEP.Text : null;
                dadosFornecedor.Endereco = !txtEndereco.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndereco.Text.Trim().ToUpper() : null;
                dadosFornecedor.Numero = !txtEndNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndNum.Text.Trim() : null;
                dadosFornecedor.Complemento = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text.Trim() : null;
                dadosFornecedor.Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text.Trim().ToUpper() : null;
                dadosFornecedor.MunicipioId = !this.tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue ? tseMunicipio.DBValue.ToString() : null;
                dadosFornecedor.Email = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null;
                dadosFornecedor.Telefone = !txtTelefone.Text.IsNullOrEmptyOrWhiteSpace() ? txtTelefone.Text.RetirarMascaraTelefone().Trim() : null;
                dadosFornecedor.Enviado = false; //Inicia com false
                dadosFornecedor.GrandePorte = chkGrandePorte.Checked;
                dadosFornecedor.Eventual = chkEventual.Checked;
                dadosFornecedor.UsuarioId = User.Identity.Name;
                dadosFornecedor.DataAlteracao = DateTime.Now.Date;

                validacao = rnFornecedor.Valida(dadosFornecedor, (this.tseFornecedorPrestacao.DBValue.IsNull ? true : false));

                if (validacao.Valido)
                {
                    if (_tipoOperacao == TipoOperacao.Novo)
                    {
                        rnFornecedor.Insere(dadosFornecedor);
                        tseFornecedorPrestacao.ResetValue();
                        tseFornecedorPrestacao.DBValue = dadosFornecedor.FornecedorId;
                        tseFornecedorPrestacao_Changed(null, null);

                        mensagem = "Fornecedor inserido com sucesso.";

                        lnkConfirmacaoCadastro_Click(sender, e);
                        lnkAlteracaoSenha_Click(sender, e);
                    }
                    else
                    {
                        rnFornecedor.Atualiza(dadosFornecedor);
                        mensagem = "Fornecedor atualizado com sucesso.";
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

        protected void btnEnviarAnalise_Click(object sender, ImageClickEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;

            var fornecedorId = !this.tseFornecedorPrestacao.DBValue.IsNull && tseFornecedorPrestacao.IsValidDBValue ? (int?)Convert.ToInt32(tseFornecedorPrestacao["IDFORNECEDOR"]) : null;

            if (!fornecedorId.HasValue)
                return;

            try
            {
                validacao = rnFornecedor.ValidaEnvioAnalise(fornecedorId.Value, User.Identity.Name, rblTipo.SelectedValue, chkGrandePorte.Checked, chkEventual.Checked);
                  
                if (validacao.Valido)
                {
                    rnFornecedor.EnviaAnalise(fornecedorId.Value, User.Identity.Name);
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                    lblMensagem.Text = "Fornecedor enviado para analise com sucesso.";
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

        protected void btnEnviarAnalise_PreRender(object sender, EventArgs e)
        {
            btnEnviarAnalise.Visible = !Finalizado;
        }

        protected void btnAnalisar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                rblConfirmacao.ClearSelection();
                CarregaMotivo();
                //pnlMotivo.Visible = false;
                this.pucConfirmar.ShowOnPageLoad = true;
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
                analise.FornecedorId = !this.tseFornecedorPrestacao.DBValue.IsNull && tseFornecedorPrestacao.IsValidDBValue ? Convert.ToInt32(tseFornecedorPrestacao["IDFORNECEDOR"]) : -1;
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
                    grdHistoricoAnalise.DataBind();

                    _tipoOperacao = TipoOperacao.SucessoAnalise;
                    ControlarTipoOperacao();
                    lblMensagem.Text = "Fornecedor analisado com sucesso.";
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
            btnAnalisar.Visible = !Finalizado;
            CarregaMotivo();
        }

        private void CarregaMotivo()
        {
            RN.PrestacaoContas.MotivoReprovacaoFornecedor rnMotivoReprovacaoFornecedor = new Techne.Lyceum.RN.PrestacaoContas.MotivoReprovacaoFornecedor();
            try
            {
                ddlMotivoReprovacaoFornecedor.Items.Clear();
                ddlMotivoReprovacaoFornecedor.DataSource = rnMotivoReprovacaoFornecedor.ListaAtivoPor();
                ddlMotivoReprovacaoFornecedor.Items.Insert(0, new ListItem("Selecione", string.Empty));
                ddlMotivoReprovacaoFornecedor.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblConfirmacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //pnlMotivo.Visible = false;
                hidMotivosSelecionados.Value = string.Empty;

                if (!rblConfirmacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    btnSalvar.Visible = true;
                    //pnlMotivo.Visible = (rblConfirmacao.SelectedValue == "Reprovado");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void pcFornecedor_TabClick(object source, TabControlCancelEventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        protected void tseFornecedorPrestacao_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseFornecedorPrestacao.DBValue.IsNull)
                {
                    if (tseFornecedorPrestacao.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;
                        lblMensagem.Text = "Fornecedor não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Fornecedor não ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }


                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tseFornecedorPrestacao.Enabled = true;
                        tseFornecedorPrestacao.Mode = ControlMode.Edit;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        tseFornecedorPrestacao.Enabled = true;
                        tseFornecedorPrestacao.Mode = ControlMode.Edit;
                        tseMunicipio.Mode = ControlMode.View;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        tseFornecedorPrestacao.Enabled = false;
                        tseFornecedorPrestacao.Mode = ControlMode.View;
                        tseMunicipio.Enabled = true;
                        tseMunicipio.Mode = ControlMode.Edit;
                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        tseFornecedorPrestacao.Enabled = false;
                        tseFornecedorPrestacao.Mode = ControlMode.View;
                        tseMunicipio.Mode = ControlMode.Edit;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tseMunicipio.Mode = ControlMode.View;

                        break;
                    }
            }
        }

        protected void tseFornecedorPrestacao_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.PrestacaoContas.Entidades.FornecedorDocumentoArquivo fornecedorDocumentoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.FornecedorDocumentoArquivo();
                ValidacaoDados validacao = new ValidacaoDados();
                string mensagem = string.Empty;
                Statuslbl.Text = string.Empty;

                byte[] imageBytes = new byte[FileUpload1.PostedFile.InputStream.Length + 1];
                FileUpload1.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);

                fornecedorDocumentoArquivo.DocumentosFornecedorId = !hdnFornecedorDocumentoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnFornecedorDocumentoId.Value) : -1;
                fornecedorDocumentoArquivo.ChaveArquivo = Guid.NewGuid().ToString();
                fornecedorDocumentoArquivo.Arquivo = imageBytes;
                fornecedorDocumentoArquivo.NomeArquivo = FileUpload1.PostedFile.FileName;
                fornecedorDocumentoArquivo.TipoArquivo = FileUpload1.PostedFile.ContentType;
                fornecedorDocumentoArquivo.UsuarioId = User.Identity.Name;
                fornecedorDocumentoArquivo.DataCadastro = DateTime.Now;
                fornecedorDocumentoArquivo.DataAlteracao = DateTime.Now;

                validacao = rnFornecedorDocumentoArquivo.Valida(fornecedorDocumentoArquivo);
                if (validacao.Valido)
                {
                    rnFornecedorDocumentoArquivo.Atualiza(fornecedorDocumentoArquivo);
                    mensagem = "Arquivo atualizado com sucesso.";

                    Statuslbl.Text = mensagem;
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    Statuslbl.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

                grdDocumento.DataBind();
                repCarrossel.DataBind();

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
                hdnFornecedorTipoDocumentoId.Value = string.Empty;
                hdnFornecedorDocumentoId.Value = string.Empty;
                Statuslbl.Text = string.Empty;

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                hdnFornecedorTipoDocumentoId.Value = chave[1].ToString();
                hdnFornecedorDocumentoId.Value = chave[0].ToString();

                pucConfirmarArquivo.ShowOnPageLoad = true;
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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=FornecedorDocumentoArquivo&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnFornecedorDocumentoArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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

        protected void rblTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtCPF.Visible = false;
                txtCnpj.Visible = false;
                txtInscricao.Enabled = false;
                txtInscricaoMunicipal.Enabled = false;
                txtCPF.Text = txtCnpj.Text = txtInscricaoMunicipal.Text = txtInscricao.Text = string.Empty;

                if (!rblTipo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    txtCPF.Visible = rblTipo.SelectedValue == "Pessoa Física" ? true : false;
                    txtCnpj.Visible = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;

                    lblInscricaoEstadual.Visible = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;
                    lblInscricaoMunicipal.Visible = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;
                    txtInscricaoMunicipal.Visible = txtInscricaoMunicipal.Enabled = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;
                    txtInscricao.Visible = txtInscricao.Enabled = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;

                    pcFornecedor.TabPages[2].Enabled = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;
                    pcFornecedor.TabPages[3].Enabled = rblTipo.SelectedValue == "Pessoa Jurídica" ? true : false;
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

        #region Habilitação

        public object ListaProdutoServicoGrupo()
        {
            var dv = rnProdutoServicoGrupo.ListaAtivoPor().DefaultView;
            dv.Sort = "DESCRICAO asc";
            return dv;
        }



        public object ListaFornecedorProdutoServicoGrupo(string IDFORNECEDOR)
        {
            return rnFornecedorProdutoServicoGrupo.ListaPor(Convert.ToInt32(IDFORNECEDOR));
        }

        public void InsertFornecedorProdutoServicoGrupo(object PRODUTOSERVICOGRUPOID) { }

        public void UpdateFornecedorProdutoServicoGrupo(object FORNECEDOR__PRODUTOSERVICOGRUPOID, object PRODUTOSERVICOGRUPOID) { }

        public void DeleteFornecedorProdutoServicoGrupo(object FORNECEDOR__PRODUTOSERVICOGRUPOID) { }



        protected void grdFornecedorProdutoServicoGrupo_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdFornecedorProdutoServicoGrupo);
        }

        protected void grdFornecedorProdutoServicoGrupo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdFornecedorProdutoServicoGrupo.Settings.ShowFilterRow = false;
        }

        protected void grdFornecedorProdutoServicoGrupo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdFornecedorProdutoServicoGrupo.Settings.ShowFilterRow = false;
        }

        protected void grdFornecedorProdutoServicoGrupo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var fpsg = new RN.PrestacaoContas.Entidades.FornecedorProdutoServicoGrupo();

                fpsg.FornecedorProdutoServicoGrupoId = null;
                fpsg.FornecedorId = Convert.ToInt32(tseFornecedorPrestacao.DBValue.ToString());
                fpsg.ProdutoServicoGrupoId = Convert.ToInt32(e.NewValues["PRODUTOSERVICOGRUPOID"]);
                fpsg.UsuarioId = User.Identity.Name;
                fpsg.DataCadastro = DateTime.Now;
                fpsg.DataAlteracao = DateTime.Now;

                validacao = rnFornecedorProdutoServicoGrupo.Valida(fpsg);

                if (validacao.Valido)
                {
                    rnFornecedorProdutoServicoGrupo.Insere(fpsg);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdFornecedorProdutoServicoGrupo.DataBind();

                _tipoOperacao = TipoOperacao.Sucesso;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdFornecedorProdutoServicoGrupo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var fpsg = new Techne.Lyceum.RN.PrestacaoContas.Entidades.FornecedorProdutoServicoGrupo();

                fpsg.FornecedorProdutoServicoGrupoId = Convert.ToInt32(e.Keys["FORNECEDOR__PRODUTOSERVICOGRUPOID"]);
                fpsg.ProdutoServicoGrupoId = Convert.ToInt32(e.NewValues["PRODUTOSERVICOGRUPOID"]);
                fpsg.UsuarioId = User.Identity.Name;
                fpsg.DataAlteracao = DateTime.Now;

                validacao = rnFornecedorProdutoServicoGrupo.Valida(fpsg);

                if (validacao.Valido)
                {
                    rnFornecedorProdutoServicoGrupo.Atualiza(fpsg);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdFornecedorProdutoServicoGrupo.DataBind();

                _tipoOperacao = TipoOperacao.Sucesso;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                throw;
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdFornecedorProdutoServicoGrupo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                int fornecedorProdutoServicoGrupoId = 0;

                fornecedorProdutoServicoGrupoId = Convert.ToInt32(e.Keys["FORNECEDOR__PRODUTOSERVICOGRUPOID"]);

                validacao = rnFornecedorProdutoServicoGrupo.ValidaRemocao(fornecedorProdutoServicoGrupoId);

                if (validacao.Valido)
                {
                    rnFornecedorProdutoServicoGrupo.Remove(fornecedorProdutoServicoGrupoId);
                    grdFornecedorProdutoServicoGrupo.DataBind();

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Razão Social

        public object ListaRazaoSocial(string IDFORNECEDOR)
        {
            return rnFornecedorRazaoSocial.ListaPor(Convert.ToInt32(IDFORNECEDOR));
        }

        public void InsertRazaoSocial(object DESCRICAO, object DATAINICIO, object DATAFIM) { }

        public void UpdateRazaoSocial(object FORNECEDORRAZAOSOCIALID, object DESCRICAO, object DATAINICIO, object DATAFIM) { }

        public void DeleteRazaoSocial(object FORNECEDORRAZAOSOCIALID) { }



        protected void grdHistoricoRazaoSocial_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdHistoricoRazaoSocial);
        }

        protected void grdHistoricoRazaoSocial_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (e.Column.FieldName == "DATAFIM")
                e.Editor.Visible = e.Editor.Value != null;
        }

        protected void grdHistoricoRazaoSocial_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdHistoricoRazaoSocial.Settings.ShowFilterRow = false;
        }

        protected void grdHistoricoRazaoSocial_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["DATAINICIO"] = DateTime.Now;
            grdHistoricoRazaoSocial.Settings.ShowFilterRow = false;
        }

        protected void grdHistoricoRazaoSocial_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var razaoSocial = new Techne.Lyceum.RN.PrestacaoContas.Entidades.FornecedorRazaoSocial();

                razaoSocial.FornecedorRazaoSocialId = Convert.ToInt32(e.Keys["FORNECEDORRAZAOSOCIALID"]);
                razaoSocial.FornecedorId = Convert.ToInt32(tseFornecedorPrestacao.DBValue.ToString());
                razaoSocial.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
                razaoSocial.DataInicio = Convert.ToDateTime(e.NewValues["DATAINICIO"]);
                razaoSocial.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : (DateTime?)null;
                razaoSocial.UsuarioId = User.Identity.Name;
                razaoSocial.DataAlteracao = DateTime.Now;

                validacao = rnFornecedorRazaoSocial.Valida(razaoSocial, false);

                if (validacao.Valido)
                {
                    rnFornecedorRazaoSocial.Atualiza(razaoSocial);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdHistoricoRazaoSocial.DataBind();

                _tipoOperacao = TipoOperacao.Sucesso;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                throw;
                //lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Representante Legal

        public object ListaRepresentanteLegal(string IDFORNECEDOR)
        {
            return rnFornecedorRepresentanteLegal.ListaPor(Convert.ToInt32(IDFORNECEDOR));
        }

        public void InsertRepresentanteLegal(object NOME, object CPF, object DATAINICIO, object DATAFIM) { }

        public void UpdateRepresentanteLegal(object FORNECEDORREPRESENTANTELEGALID, object NOME, object CPF, object DATAINICIO, object DATAFIM) { }

        public void DeleteRepresentanteLegal(object FORNECEDORREPRESENTANTELEGALID) { }



        protected void grdRepresentante_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRepresentante);
        }

        protected void grdRepresentante_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (grdRepresentante.GetRowValues(e.VisibleIndex, "DATAFIM") != DBNull.Value)
                if (e.ButtonType == ColumnCommandButtonType.Edit || e.ButtonType == ColumnCommandButtonType.Delete)
                    e.Visible = false;
        }

       
        protected void grdDocumento_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            var periodicidade = (grdDocumento.GetRowValues(e.VisibleIndex, "PERIODICIDADE_MESES") as int?) ?? 0;
            if (periodicidade > 0 && e.Column.FieldName == "DATAFIM")
                e.Editor.Visible = false;
        }

        protected void grdRepresentante_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdRepresentante.Settings.ShowFilterRow = false;
        }

        protected void grdRepresentante_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdRepresentante.Settings.ShowFilterRow = false;
        }

        protected void grdRepresentante_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var frl = new RN.PrestacaoContas.Entidades.FornecedorRepresentanteLegal();

                frl.FornecedorRepresentanteLegalId = null;
                frl.FornecedorId = Convert.ToInt32(tseFornecedorPrestacao.DBValue.ToString());
                frl.Nome = e.NewValues["NOME"] != null ? e.NewValues["NOME"].ToString().Trim().ToUpper() : null;
                frl.Cpf = e.NewValues["CPF"] != null ? e.NewValues["CPF"].ToString().Trim().ToUpper() : null;
                frl.DataInicio = Convert.ToDateTime(e.NewValues["DATAINICIO"]);
                frl.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : (DateTime?)null;
                frl.UsuarioId = User.Identity.Name;
                frl.DataCadastro = DateTime.Now;
                frl.DataAlteracao = DateTime.Now;

                validacao = rnFornecedorRepresentanteLegal.Valida(frl, true);

                if (validacao.Valido)
                {
                    rnFornecedorRepresentanteLegal.Insere(frl);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdRepresentante.DataBind();

                _tipoOperacao = TipoOperacao.Sucesso;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdRepresentante_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var frl = new Techne.Lyceum.RN.PrestacaoContas.Entidades.FornecedorRepresentanteLegal();

                frl.FornecedorRepresentanteLegalId = Convert.ToInt32(e.Keys["FORNECEDORREPRESENTANTELEGALID"]);
                frl.FornecedorId = Convert.ToInt32(tseFornecedorPrestacao.DBValue.ToString());
                frl.Nome = e.NewValues["NOME"] != null ? e.NewValues["NOME"].ToString().Trim().ToUpper() : null;
                frl.Cpf = e.NewValues["CPF"] != null ? e.NewValues["CPF"].ToString().Trim().RetirarMascaraCPF() : null;
                frl.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
                frl.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : (DateTime?)null;
                frl.UsuarioId = User.Identity.Name;
                frl.DataAlteracao = DateTime.Now;

                validacao = rnFornecedorRepresentanteLegal.Valida(frl, false);

                if (validacao.Valido)
                {
                    rnFornecedorRepresentanteLegal.Atualiza(frl);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdRepresentante.DataBind();

                _tipoOperacao = TipoOperacao.Sucesso;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                throw;
                //lblMensagem.Text = ex.Message;
            }
        }

        protected void grdRepresentante_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                int fornecedorRepresentanteLegalId = 0;

                fornecedorRepresentanteLegalId = Convert.ToInt32(e.Keys["FORNECEDORREPRESENTANTELEGALID"]);

                validacao = rnFornecedorRepresentanteLegal.ValidaRemocao(fornecedorRepresentanteLegalId);

                if (validacao.Valido)
                {
                    rnFornecedorRepresentanteLegal.Remove(fornecedorRepresentanteLegalId);
                    grdRepresentante.DataBind();

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Documento

        public object ListaDocumento(string IDFORNECEDOR, string tipo)
        {
            return rnDocumentosFornecedor.ListaPor(Convert.ToInt32(IDFORNECEDOR), tipo);
        }

        public void UpdateDocumento(object DOCUMENTOSNECESSARIOSFORNECEDORID, object DATAINICIO, object DATAFIM) { }

        public void UpdateDocumento(object DOCUMENTOSNECESSARIOSFORNECEDORID, object DATAINICIO) { }

        protected void grdDocumento_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var df = new Techne.Lyceum.RN.PrestacaoContas.Entidades.DocumentosFornecedor();

                var periodicidade = (grdDocumento.GetRowValues(grdDocumento.EditingRowVisibleIndex, "PERIODICIDADE_MESES") as int?) ?? 0;

                df.DocumentosFornecedorId = grdDocumento.GetRowValues(grdDocumento.EditingRowVisibleIndex, "DOCUMENTOSFORNECEDORID") as int?;
                df.FornecedorId = Convert.ToInt32(tseFornecedorPrestacao.DBValue.ToString());
                df.DocumentosNecessariosFornecedorId = Convert.ToInt32(e.Keys["DOCUMENTOSNECESSARIOSFORNECEDORID"]);
                df.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : (DateTime?)null;

                if (periodicidade > 0)
                    df.DataFim = df.DataInicio.Value.AddMonths(periodicidade).AddDays(-1);
                else
                    df.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : (DateTime?)null;

                df.UsuarioId = User.Identity.Name;
                df.DataCadastro = DateTime.Now;
                df.DataAlteracao = DateTime.Now;

                validacao = rnDocumentosFornecedor.Valida(df);

                if (validacao.Valido)
                {
                    if (!df.DocumentosFornecedorId.HasValue)
                        rnDocumentosFornecedor.Insere(df);
                    else
                        rnDocumentosFornecedor.Atualiza(df);

                    if (hdnEnviado.Value == "")
                        lblMensagem.Text = "Documento alterado com sucesso.";
                    else
                        lblMensagem.Text = "Documento alterado com sucesso.<br />Fornecedor regressou para o status inicial e deverá ser enviado para análise novamente.";
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdDocumento.DataBind();
                repCarrossel.DataBind();

                _tipoOperacao = TipoOperacao.Sucesso;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                throw;
                //lblMensagem.Text = ex.Message;
            }
        }

        #endregion
    }
}