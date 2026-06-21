using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Certificacao;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Certificacao
{
    [NavUrl("~/Certificacao/AlunoCertificacao.aspx"), ControlText("Aluno Certificação"), Title("Aluno Certificação")]
    public partial class AlunoCertificacao : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Alterar,
            Consultar,
            Inicial,
            Sucesso
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


        public object Lista(object alunoCertificacaoId)
        {
            RN.Certificacao.AlunoDocumento rnAlunoDocumento = new AlunoDocumento();

            if (!Convert.ToString(alunoCertificacaoId).IsNullOrEmptyOrWhiteSpace())
            {
                return rnAlunoDocumento.ListaPor(Convert.ToInt32(alunoCertificacaoId));
            }

            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                ValidarCampos();

                if (!IsPostBack)
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    this.ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }


        }

        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdDadosEscolares, string.Empty);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnNovo, AcaoControle.novo);

            ControlaAcesso(grdDadosEscolares, AcaoControle.editar, "btnEditarReposicao");

            if (Permission.AllowInsert)
            {
                ControlaAcesso(btnSalvar, AcaoControle.novo);
            }

            if (Permission.AllowUpdate)
            {
                ControlaAcesso(btnSalvar, AcaoControle.editar);
            }

            AcessoGrid();
        }

        protected void DesabilitaCampos()
        {
            tseNaturalidade.Mode = ControlMode.View;

            txtNomeAluno.ReadOnly = true;
            txtNomeMae.ReadOnly = true;
            chkNaoDeclarMae.Enabled = false;
            txtNomePai.ReadOnly = true;
            chkNaoDeclarPai.Enabled = false;
            txtCPF.ReadOnly = true;
            txtMunicipioNaturalidade.ReadOnly = true;
            dtDataNasc.ReadOnly = true;
            cmbNacionalidade.Enabled = false;
            txtNRg.ReadOnly = true;
            cmbRGEmissor.Enabled = false;
            cmbRGUF.Enabled = false;
        }

        protected void DesabilitaCamposDadosEscolares()
        {
            tseUnidadeEns.Enabled = false;
            txtCurso.ReadOnly = true;
            txtAtoAutoriza.ReadOnly = true;
            dtDataAto.Enabled = false;
            txtTotalHAula.ReadOnly = true;
            txtTotalHRelogio.ReadOnly = true;
            dtDataConclusao.Enabled = false;

            txtNumLivro.ReadOnly = true;
            txtFolhaLivro.ReadOnly = true;
            txtLivro.ReadOnly = true;
            txtObservacao.ReadOnly = true;

            ddlNivelModalidade.Enabled = false;
            ddlTipoConclusao.Enabled = false;
            ddlTipoDocumento.Enabled = false;
        }

        protected void HabilitaCampos()
        {
            tseNaturalidade.Mode = ControlMode.Edit;

            txtNomeAluno.ReadOnly = false;
            txtNomeMae.ReadOnly = false;
            chkNaoDeclarMae.Enabled = true;
            txtNomePai.ReadOnly = false;
            chkNaoDeclarPai.Enabled = true;
            txtCPF.ReadOnly = false;
            txtMunicipioNaturalidade.ReadOnly = false;
            dtDataNasc.ReadOnly = false;
            cmbNacionalidade.Enabled = true;
            txtNRg.ReadOnly = false;
            cmbRGEmissor.Enabled = true;
            cmbRGUF.Enabled = true;
        }

        protected void HabilitaCamposDadosEscolares()
        {
            ddlNivelModalidade.Enabled = true;
            ddlTipoConclusao.Enabled = true;
            ddlTipoDocumento.Enabled = true;

            tseUnidadeEns.Enabled = true;
            txtCurso.ReadOnly = false;
            txtAtoAutoriza.ReadOnly = false;
            dtDataAto.Enabled = true;
            txtTotalHAula.ReadOnly = false;
            txtTotalHRelogio.ReadOnly = false;
            dtDataConclusao.Enabled = true;

            txtNumLivro.ReadOnly = false;
            txtFolhaLivro.ReadOnly = false;
            txtLivro.ReadOnly = false;
            txtObservacao.ReadOnly = false;
        }


        private void LimparTela()
        {
            hdnAlunoCertificacaoId.Value = string.Empty;
            hdnAlunoDocumentoId.Value = string.Empty;
            tseNaturalidade.ResetValue();
            tseUnidadeEns.ResetValue();

            txtNomeAluno.Text = string.Empty;
            txtNomeMae.Text = string.Empty;
            chkNaoDeclarMae.Checked = false;
            txtNomePai.Text = string.Empty;
            chkNaoDeclarPai.Checked = false;
            txtCPF.Text = string.Empty;
            txtMunicipioNaturalidade.Text = string.Empty;
            dtDataNasc.Text = string.Empty;
            cmbNacionalidade.ClearSelection();
            txtNRg.Text = string.Empty;
            cmbRGEmissor.ClearSelection();
            cmbRGUF.ClearSelection();

            txtCurso.Text = string.Empty;
            txtAtoAutoriza.Text = string.Empty;
            dtDataAto.Text = string.Empty;
            txtTotalHAula.Text = string.Empty;
            txtTotalHRelogio.Text = string.Empty;
            dtDataConclusao.Text = string.Empty;

            txtNumLivro.Text = string.Empty;
            txtFolhaLivro.Text = string.Empty;
            txtLivro.Text = string.Empty;
            txtObservacao.Text = string.Empty;

            ddlTipoConclusao.ClearSelection();
            ddlTipoDocumento.ClearSelection();
        }

        private void LimparTelaDadosEscolares()
        {
            hdnAlunoDocumentoId.Value = string.Empty;
            tseUnidadeEns.ResetValue();
            txtCurso.Text = string.Empty;
            txtAtoAutoriza.Text = string.Empty;
            dtDataAto.Text = string.Empty;
            txtTotalHAula.Text = string.Empty;
            txtTotalHRelogio.Text = string.Empty;
            dtDataConclusao.Text = string.Empty;

            txtNumLivro.Text = string.Empty;
            txtFolhaLivro.Text = string.Empty;
            txtLivro.Text = string.Empty;
            txtObservacao.Text = string.Empty;

            ddlTipoConclusao.ClearSelection();
            ddlTipoDocumento.ClearSelection();
        }

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tseAlunoCPF.Enabled = true;
                        tseNaturalidade.Mode = ControlMode.Edit;
                        tseUnidadeEns.Mode = ControlMode.Edit;

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        tseAlunoCPF.Enabled = true;
                        tseNaturalidade.Mode = ControlMode.Edit;
                        tseUnidadeEns.Mode = ControlMode.Edit;


                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        tseAlunoCPF.Enabled = false;
                        tseNaturalidade.Mode = ControlMode.Edit;
                        tseUnidadeEns.Mode = ControlMode.Edit;

                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        tseAlunoCPF.Enabled = false;
                        tseNaturalidade.Mode = ControlMode.Edit;
                        tseUnidadeEns.Mode = ControlMode.Edit;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tseAlunoCPF.Enabled = false;
                        tseNaturalidade.Mode = ControlMode.Edit;
                        tseUnidadeEns.Mode = ControlMode.Edit;

                        break;
                    }
            }
        }

        protected void tseAlunoCPF_Changed(object sender, EventArgs args)
        {
            btnImprimirCert.Visible = false;
            btnImprimirDipl.Visible = false;

            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseAlunoCPF.DBValue.IsNull)
                {
                    if (tseAlunoCPF.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;
                        lblMensagem.Text = "Aluno não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseNaturalidade_Changed(object sender, EventArgs args)
        {
            try
            {
                if (tseNaturalidade.IsValidDBValue && !tseNaturalidade.DBValue.IsNull)
                {
                    txtMunicipioNaturalidade.Text = tseNaturalidade["uf_sigla"].ToString();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEns_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void chkNaoDeclarMae_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNomeMae.Text = string.Empty;

                if (chkNaoDeclarMae.Checked)
                {
                    txtNomeMae.Text = chkNaoDeclarMae.Text.ToUpper();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkNaoDeclarPai_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNomePai.Text = string.Empty;

                if (chkNaoDeclarPai.Checked)
                {
                    txtNomePai.Text = chkNaoDeclarPai.Text.ToUpper();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaRGEmissor()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            cmbRGEmissor.Items.Clear();
            cmbRGEmissor.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.RgEmissor);
            cmbRGEmissor.DataBind();
            cmbRGEmissor.Items.Insert(0, item);
        }

        private void CarregaRGUF()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object dtUf;

            dtUf = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Uf, RN.Basico.QueryListaUF);

            cmbRGUF.Items.Clear();
            cmbRGUF.DataSource = dtUf;
            cmbRGUF.DataBind();
            cmbRGUF.Items.Insert(0, item);
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

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Certificacao.AlunoDocumento rnAlunoDocumento = new AlunoDocumento();
                RN.Certificacao.Entidades.AlunoDocumento alunoDocumento = new Techne.Lyceum.RN.Certificacao.Entidades.AlunoDocumento();
                ValidacaoDados validacao = new ValidacaoDados();

                alunoDocumento.UnidadeEnsino = !tseUnidadeEns.DBValue.IsNull ? tseUnidadeEns.DBValue.ToString() : null;
                alunoDocumento.Modalidade = !ddlNivelModalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlNivelModalidade.SelectedValue : null;
                alunoDocumento.TipoConclusaoId = !ddlTipoConclusao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlTipoConclusao.SelectedValue) : -1;
                alunoDocumento.DocumentoId = !ddlTipoDocumento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlTipoDocumento.SelectedValue) : -1;
                alunoDocumento.NomeCurso = !txtCurso.Text.IsNullOrEmptyOrWhiteSpace() ? txtCurso.Text.Trim() : null;
                alunoDocumento.AtoAutoriza = !txtAtoAutoriza.Text.IsNullOrEmptyOrWhiteSpace() ? txtAtoAutoriza.Text.Trim() : null;
                alunoDocumento.DataAutoriza = !dtDataAto.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataAto.Date : DateTime.MinValue;
                alunoDocumento.TotalHorasAula = !txtTotalHAula.Text.IsNullOrEmptyOrWhiteSpace() ? txtTotalHAula.Text.Trim() : null;
                alunoDocumento.TotalHorasRelogio = !txtTotalHRelogio.Text.IsNullOrEmptyOrWhiteSpace() ? txtTotalHRelogio.Text.Trim() : null;
                alunoDocumento.DataConclusao = !dtDataConclusao.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataConclusao.Date : DateTime.MinValue;

                alunoDocumento.NumeroLivro = !txtNumLivro.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumLivro.Text.Trim() : null;
                alunoDocumento.FolhaLivro = !txtFolhaLivro.Text.IsNullOrEmptyOrWhiteSpace() ? txtFolhaLivro.Text.Trim() : null;
                alunoDocumento.Livro = !txtLivro.Text.IsNullOrEmptyOrWhiteSpace() ? txtLivro.Text.Trim() : null;
                alunoDocumento.Observacao = !txtObservacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtObservacao.Text.Trim() : null;
                alunoDocumento.UsuarioId = User.Identity.Name;
                alunoDocumento.AlunoCertificacaoId = !hdnAlunoCertificacaoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnAlunoCertificacaoId.Value) : -1;
                alunoDocumento.AlunoDocumentoId = !hdnAlunoDocumentoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnAlunoDocumentoId.Value) : -1;


                validacao = rnAlunoDocumento.Valida(alunoDocumento, dtDataNasc.Date, (hdnAlunoDocumentoId.Value.IsNullOrEmptyOrWhiteSpace() ? true : false));

                if (validacao.Valido)
                {
                    if (hdnAlunoDocumentoId.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnAlunoDocumento.Insere(alunoDocumento);
                        hdnAlunoDocumentoId.Value = alunoDocumento.AlunoDocumentoId > 0 ? alunoDocumento.AlunoDocumentoId.ToString() : string.Empty;
                    }
                    else
                    {
                        rnAlunoDocumento.Atualiza(alunoDocumento);
                    }
                    btnGerarDocumento_Click(sender, e);

                    this.odsDadosEscolares.Select();
                    this.odsDadosEscolares.DataBind();
                    this.grdDadosEscolares.DataBind();

                    LimparTelaDadosEscolares();

                    pnlNovoDadosEscolares.Visible = false;

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

        /*private void CarregarDadosAlunoCertificacaoPor(string CPF)
        {
            RN.Aluno rnAluno = new RN.Aluno();
            RN.DTOs.AlunoCertificacao alunoCertificacao = new Techne.Lyceum.RN.DTOs.AlunoCertificacao();
            try
            {
                //Busca dados do aluno passando o parâmetro CPF
                alunoCertificacao = rnAluno.ObtemFichaAlunoCertificacaoPor(CPF);

                //Buscar dados a partir do cpf deste aluno
                if (!string.IsNullOrEmpty(alunoCertificacao.CPF))
                {
                    pnlDados.Visible = true;

                    //Dados pessoais
                    lblNomeAluno.Text = alunoCertificacao.Nome.Trim();
                    lblDataNascimento.Text = alunoCertificacao.DataNascimento.ToString("dd/MM/yyyy");
                    txtMunicipioNaturalidade.Text = alunoCertificacao.MunicipioNascimento;
                    lblNacionalidade.Text = (!string.IsNullOrEmpty(alunoCertificacao.Nacionalidade.ToString()) ? alunoCertificacao.Nacionalidade : "Não declarado");
                    lblCPF.Text = alunoCertificacao.CPF.AplicarMascaraCPF();
                    lblNRg.Text = alunoCertificacao.RgNumero;
                    lblRGEmissor.Text = alunoCertificacao.RgEmissor;
                    lblRGUF.Text = alunoCertificacao.RgUf;

                    //Dados Filiação
                    lblNomeMae.Text = alunoCertificacao.NomeMae;
                    lblNomePai.Text = alunoCertificacao.NomePai;

                    //Dados da Escola
                    lblUnidadeEnsino.Text = alunoCertificacao.UnidadeEnsino;

                    //Dados do Curso
                    lblCurso.Text = alunoCertificacao.NomeCurso;
                    lblAtoAutoriza.Text = alunoCertificacao.AtoAutoriza;
                    lblDtAto.Text = alunoCertificacao.AtoData.ToString();
                    lblTotalHAula.Text = alunoCertificacao.TotalAula;
                    lblTotalHRelogio.Text = alunoCertificacao.TotalRelogio;
                    lblDtConclusao.Text = alunoCertificacao.ConclusaoData.ToString();

                    //Observação
                    lblNumeroLivro.Text = alunoCertificacao.NumLivro;
                    lblFolhaLivro.Text = alunoCertificacao.FolhaLivro;
                    lblLivro.Text = alunoCertificacao.Livro;
                    lblObservacao.Text = alunoCertificacao.Observacao;


                    //btnGerarDocumento.Visible = true;
                    btnCancel.Visible = true;
                }
                else
                {
                    //limpar a tela
                    pnlDados.Visible = false;
                    //btnGerarDocumento.Visible = false;
                    tseAlunoCPF.ResetValue();
                    //ddlTipoConclusao.ClearSelection();
                    lblMensagem.Text = "Aluno não encontrado.";
                    return;
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }*/

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }


            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnNovo.Visible = false;
            btnImprimirCert.Visible = false;
            btnImprimirDipl.Visible = false;
        }

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {

                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        btnImprimirCert.Visible = false;
                        btnImprimirDipl.Visible = false;

                        lblMensagem.Text = String.Empty;

                        tseAlunoCPF.ResetValue();
                        LimparTela();

                        pnlDados.Visible = false;
                        pnlEnsino.Visible = false;
                        pnlCurso.Visible = false;
                        pnlObservacao.Visible = false;

                        CarregaNacionalidade();
                        CarregaRGEmissor();
                        CarregaRGUF();

                        tseAlunoCPF.ResetValue();
                        listarTipos();
                        pnAbas.Visible = false;

                        pcCertificacao.ActiveTabIndex = 0;
                        pcCertificacao.TabPages[1].Enabled = false;
                        ControlarTSearchs();

                        break;
                    }

                case TipoOperacao.Novo:
                    {

                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);
                        btnImprimirCert.Visible = false;
                        btnImprimirDipl.Visible = false;

                        LimparTela();
                        HabilitaCampos();
                        ControlarTSearchs();

                        pnlDados.Visible = true;
                        pnlEnsino.Visible = true;
                        pnlCurso.Visible = true;
                        pnlObservacao.Visible = true;
                        pnAbas.Visible = true;
                        pcCertificacao.ActiveTabIndex = 0;
                        pcCertificacao.TabPages[1].Enabled = false;

                        break;

                    }

                case TipoOperacao.Alterar:
                    {

                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);
                        btnImprimirCert.Visible = false;
                        btnImprimirDipl.Visible = false;

                        HabilitaCampos();
                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        RN.Certificacao.AlunoCertificacao rnAlunoCertificacao = new Techne.Lyceum.RN.Certificacao.AlunoCertificacao();
                        RN.Certificacao.Entidades.TipoDocumentoCertifica tipoDocumentoCertifica = new Techne.Lyceum.RN.Certificacao.Entidades.TipoDocumentoCertifica();
                        RN.Certificacao.Entidades.AlunoCertificacao alunoCertificacao = new Techne.Lyceum.RN.Certificacao.Entidades.AlunoCertificacao();
                        RN.Endereco rnEndereco = new Endereco();

                        lblMensagem.Text = string.Empty;
                        LimparTela();

                        alunoCertificacao = rnAlunoCertificacao.ObtemDadosPor(Convert.ToString(tseAlunoCPF.DBValue).RetirarMascaraCPF());

                        if (alunoCertificacao.CPF.IsNullOrEmptyOrWhiteSpace())
                        {
                            lblMensagem.Text = "Aluno não encontrado.";

                            ImageButton[] controles = new ImageButton[] { };
                            ControlarVisibilidadeControle(controles);

                            pcCertificacao.TabPages[1].Enabled = false;
                        }
                        else
                        {
                            ImageButton[] controles = new ImageButton[] { btnCancel };
                            ControlarVisibilidadeControle(controles);

                            pcCertificacao.TabPages[1].Enabled = true;

                            hdnAlunoCertificacaoId.Value = alunoCertificacao.AlunoCertificacaoId > 0 ? Convert.ToString(alunoCertificacao.AlunoCertificacaoId) : string.Empty;
                            tseAlunoCPF.DBValue = !alunoCertificacao.CPF.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.CPF.Trim() : string.Empty;
                            txtNomeAluno.Text = !alunoCertificacao.Nome.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.Nome.Trim() : string.Empty;
                            txtNomeMae.Text = !alunoCertificacao.NomeMae.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.NomeMae.Trim() : string.Empty;
                            txtNomePai.Text = !alunoCertificacao.NomePai.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.NomePai.Trim() : string.Empty;
                            txtCPF.Text = !alunoCertificacao.CPF.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.CPF.Trim() : string.Empty;
                            txtMunicipioNaturalidade.Text = !alunoCertificacao.MunicipioNascimento.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.MunicipioNascimento.Trim() : string.Empty;
                            dtDataNasc.Text = alunoCertificacao.DataNascimento.Day > 0 ? alunoCertificacao.DataNascimento.ToString() : string.Empty;

                            if (alunoCertificacao.DataNascimento != null)
                            {
                                dtDataNasc.Date = alunoCertificacao.DataNascimento.Date;
                            }

                            chkNaoDeclarMae.Checked = alunoCertificacao.NomeMae == chkNaoDeclarMae.Text.ToUpper();

                            chkNaoDeclarPai.Checked = alunoCertificacao.NomePai == chkNaoDeclarPai.Text.ToUpper();

                            cmbNacionalidade.SelectedValue = !alunoCertificacao.Nacionalidade.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.Nacionalidade : string.Empty;

                            if (!cmbNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                            {
                                txtMunicipioNaturalidade.Visible = false;

                                if (cmbNacionalidade.SelectedItem.Text.ToUpper() == "BRASILEIRA")
                                {
                                    tseNaturalidade.DBValue = !alunoCertificacao.MunicipioNascimento.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.MunicipioNascimento : string.Empty;
                                    tseNaturalidade.DBValue = !alunoCertificacao.MunicipioNascimento.IsNullOrEmptyOrWhiteSpace() ? tseNaturalidade["uf_sigla"].ToString() : string.Empty;
                                }
                                else
                                {
                                    // obtém o municipio estrangeiro
                                    string municipio = rnEndereco.ObtemMunicipioPor(alunoCertificacao.MunicipioNascimento);
                                    if (!municipio.IsNullOrEmptyOrWhiteSpace())
                                    {
                                        txtMunicipioNaturalidade.Text = municipio;
                                        txtMunicipioNaturalidade.Visible = true;
                                        tseNaturalidade.Visible = false;
                                    }
                                    else
                                    {
                                        // obtém o municipio estrangeiro
                                        string municipio2 = rnEndereco.ObtemCodigoMunicipioPor(alunoCertificacao.MunicipioNascimento);


                                        if (!municipio2.IsNullOrEmptyOrWhiteSpace())
                                        {
                                            txtMunicipioNaturalidade.Text = municipio2;
                                            txtMunicipioNaturalidade.Visible = true;
                                            tseNaturalidade.Visible = false;
                                        }
                                    }
                                }
                            }

                            HabilitaBotoesWord();

                            txtNRg.Text = !alunoCertificacao.RgNumero.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.RgNumero.Trim() : string.Empty;
                            cmbRGEmissor.SelectedValue = !alunoCertificacao.RgEmissor.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.RgEmissor : string.Empty;
                            cmbRGUF.SelectedValue = !alunoCertificacao.RgUf.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.RgUf : string.Empty;

                            tseNaturalidade.DBValue = !alunoCertificacao.MunicipioNascimento.IsNullOrEmptyOrWhiteSpace() ? alunoCertificacao.MunicipioNascimento : string.Empty;

                            pnlDados.Visible = true;
                            pnlEnsino.Visible = true;
                            pnlCurso.Visible = true;
                            pnlObservacao.Visible = true;
                            pnAbas.Visible = true;

                            //btnImprimirCert.Visible = true;
                            //btnImprimirDipl.Visible = true;

                        }

                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);
                        btnImprimirCert.Visible = true;
                        btnImprimirDipl.Visible = true;

                        pcCertificacao.ActiveTabIndex = 1;
                        pcCertificacao.TabPages[1].Enabled = true;
                        pnlNovoDadosEscolares.Visible = false;

                        break;
                    }

            }
        }

        private void CarregaNacionalidade()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            cmbNacionalidade.Items.Clear();
            cmbNacionalidade.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Nacionalidade, RN.Basico.QueryListaNacionalidades);
            cmbNacionalidade.DataBind();
            cmbNacionalidade.Items.Insert(0, item);

            item = cmbNacionalidade.Items.FindByText("BRASILEIRA");
            if (item != null)
            {
                cmbNacionalidade.ClearSelection();
                item.Selected = true;
            }
        }

        protected void cmbNacionalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!cmbNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (cmbNacionalidade.SelectedItem.Text == "ESTRANGEIRA")
                    {
                        txtMunicipioNaturalidade.Enabled = false;
                    }

                    if (cmbNacionalidade.SelectedItem.Text == "BRASILEIRA")
                    {
                        tseNaturalidade.Enabled = true;
                        txtMunicipioNaturalidade.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnGerarDocumento_Click(object sender, EventArgs e)
        {
            if (!ddlTipoDocumento.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlTipoConclusao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                switch (Convert.ToInt32(ddlTipoDocumento.SelectedValue))
                {
                    case 3:
                        GerarDocumento(RN.Certificacao.TipoDocumento.CERTIFICADO_ESCOLAR);
                        break;

                    case 4:
                        GerarDocumento(RN.Certificacao.TipoDocumento.DIPLOMA);
                        break;

                    default:
                        lblMensagem.Text = "OPÇÃO INVÁLIDA";
                        break;
                }
            }
            else
            {
                lblMensagem.Text = "Para Gerar o documento é necessário escolher o Tipo de Conclusão e o Tipo de Documento.";
            }

        }

        public void GerarDocumento(int tipoDocumentoID)
        {

            string usuario = User.Identity.Name;
            ExportaPdf pdf = new ExportaPdf();
            string html = string.Empty;
            string tipoArquivo = string.Empty;


            if (tipoDocumentoID == 3)
                tipoArquivo = "CERTIFICADO_ESCOLAR";

            else
                tipoArquivo = "DIPLOMA_ESCOLAR";

            lblMensagem.Text = string.Empty;

            try
            {

                string cssText = File.ReadAllText(HttpContext.Current.Server.MapPath("Css/CertificadoEscolar.css"));
                string certificado = GerarCertificadoDiplomaEscolarCertidao(tseAlunoCPF.DBValue.ToString(), Convert.ToInt32(ddlTipoConclusao.SelectedValue), usuario, tipoDocumentoID);

                if (certificado.Substring(2, 6) != "<html>")
                {
                    lblMensagem.Text = certificado;
                    lblMensagem.Focus();
                }
                else
                {
                    pdf.ExportaHtmlCssPor(certificado, tipoArquivo + "_" + tseAlunoCPF.DBValue.ToString() + "_" + DateTime.Now.ToShortDateString().Replace("/", "_"), cssText);
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = (ex.Message);
            }
        }

        private void listarTipos()
        {
            ddlTipoConclusao.DataSource = RN.Certificacao.HistoricoEscolar.listarTipoConclusao();
            ddlTipoConclusao.DataBind();
            ddlTipoConclusao.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Selecione", "-1"));
        }

        public string GerarCertificadoDiplomaEscolarCertidao(String Aluno, int TipoConclusao, string usuariologado, int tipoDocumentoSelecionado)
        {
            string numero, folhas, livro;
            int alunoCertificacaoId = -1;
            string corpoCertificado = null;
            RN.Aluno rnAluno = new RN.Aluno();
            RN.DTOs.DadosFichaAluno dadosAluno = new Techne.Lyceum.RN.DTOs.DadosFichaAluno();
            RN.Certificacao.Entidades.AlunoDocumentoGerado dadosAlunoDocumentoGerado = new Techne.Lyceum.RN.Certificacao.Entidades.AlunoDocumentoGerado();
            string curso, tipo_curso, unidade_Ens, municipioEscola, modalidadeNivel, modalidade, UA, numerocertificado, obs, nivel = string.Empty;
            string corpopronto = null;
            RN.Certificacao.AlunoDocumentoGerado rnAlunoDocumentoGerado = new AlunoDocumentoGerado();
            string msgErro = string.Empty;
            ExportaPdf pdf = new ExportaPdf();
            string cssText = File.ReadAllText(HttpContext.Current.Server.MapPath("Css/CertificadoEscolar.css"));
            byte[] Pdf_nao_Assinado = null;

            RN.Certificacao.CertificadoEscolar rnCertificadoEscolar = new Techne.Lyceum.RN.Certificacao.CertificadoEscolar();

            //Campos do certificado
            string identificacaoCompletadaUE, atodeCriacao, regional, nomeCompletodoConcluinte, nacionalidade, tipoDocumentodeIdentificacao, orgaoDocumentodeIdentificacao, ufDocumentodeIdentificacao, filiacaoMae, filiacaoPai, ufNaturalidade, dtNascimetnoCompletaporExtenso, identificacaodoCurso, atodeAutorizacaodoCurso, cargaHorariaTotal, datadeConclusaoporExtenso, filiacao;

            try
            {

                curso = txtCurso.Text.Trim();

                unidade_Ens = tseUnidadeEns.DBValue.ToString();

                municipioEscola = tseUnidadeEns["nomemunicipio"].ToString();

                if (!municipioEscola.IsNullOrEmptyOrWhiteSpace())
                {
                    municipioEscola = RN.Util.Utils.Capitaliza(municipioEscola);
                }

                UA = tseUnidadeEns["ua_atual"].ToString();

                modalidadeNivel = txtCurso.Text.Trim();

                modalidade = ddlNivelModalidade.SelectedValue;

                if (ddlNivelModalidade.SelectedValue == "EPC" || ddlNivelModalidade.SelectedValue == "EPS")
                {
                    tipo_curso = ", na forma Concomitante/Subsequente ";
                }
                else
                {
                    tipo_curso = string.Empty;
                }

                string cargaHoraria = txtTotalHAula.Text.Trim();

                if (string.IsNullOrEmpty(cargaHoraria))
                    return msgErro = "Não foi possível calcular a carga horária.";

                cargaHorariaTotal = cargaHoraria;
                string cargaHorariaRelogio = txtTotalHRelogio.Text;

                numero = txtNumLivro.Text.Trim();
                folhas = txtFolhaLivro.Text.Trim();
                livro = txtLivro.Text.Trim();
                obs = txtObservacao.Text.Trim();


                RN.Certificacao.TipoDocumentoCertifica rntipodocumentocertifica = new TipoDocumentoCertifica();

                corpoCertificado = rntipodocumentocertifica.ListarCorpopor(Convert.ToInt32(ddlTipoDocumento.SelectedValue));

                if (corpoCertificado == null)
                    return msgErro = "Não foi possível obter o texto para compor o documento solicitado.";


                identificacaoCompletadaUE = string.Empty;
                atodeCriacao = string.Empty;
                regional = string.Empty;


                identificacaoCompletadaUE = tseUnidadeEns["nome_comp"].ToString();
                atodeCriacao = txtAtoAutoriza.Text.Trim();
                regional = tseUnidadeEns["regional"].ToString();
                nomeCompletodoConcluinte = txtNomeAluno.Text.Trim();
                nacionalidade = cmbNacionalidade.SelectedItem.Text.ToLower();
                tipoDocumentodeIdentificacao = " RG nº " + txtNRg.Text.Trim();
                orgaoDocumentodeIdentificacao = cmbRGEmissor.SelectedItem.Text;
                ufDocumentodeIdentificacao = cmbRGUF.SelectedItem.Text;

                filiacaoMae = RN.Util.Utils.Capitaliza(txtNomeMae.Text.Trim());
                filiacaoPai = RN.Util.Utils.Capitaliza(txtNomePai.Text.Trim());
                filiacao = string.Empty;

                if (!string.IsNullOrEmpty(filiacaoMae) && !string.IsNullOrEmpty(filiacaoPai))
                    filiacao = filiacaoMae + " e " + filiacaoPai;
                else
                {
                    if (string.IsNullOrEmpty(filiacaoPai))
                        filiacao = filiacaoMae;
                    else if (string.IsNullOrEmpty(filiacaoMae))
                        filiacao = filiacaoPai;
                }

                RN.Certificacao.Util rnUtil = new Techne.Lyceum.RN.Certificacao.Util();

                ufNaturalidade = RN.Util.Utils.Capitaliza(tseNaturalidade["nome"].ToString()) + " - " + tseNaturalidade["uf_sigla"].ToString();
                dtNascimetnoCompletaporExtenso = rnUtil.DataporExtenso(dtDataNasc.Date);

                string ato_curso = RN.Util.Utils.Capitaliza(txtAtoAutoriza.Text);
                string dt_ato = dtDataAto.Date.ToShortDateString();

                if ((tipoDocumentoSelecionado == 4) & (TipoConclusao == 3))

                    identificacaodoCurso = modalidadeNivel + " " + tipo_curso;
                else
                    identificacaodoCurso = modalidadeNivel;

                atodeAutorizacaodoCurso = ato_curso + " de " + rnUtil.DataporExtenso(DateTime.Parse(dt_ato));

                datadeConclusaoporExtenso = rnUtil.DataporExtenso(dtDataConclusao.Date);



                corpopronto = corpoCertificado.Replace("#UE", identificacaoCompletadaUE).Replace("#atoCriação", atodeCriacao).Replace("#nomecompleto", nomeCompletodoConcluinte).Replace("#nacionalidade", nacionalidade).Replace("#RG", tipoDocumentodeIdentificacao).Replace("#orgaoExpedidor", orgaoDocumentodeIdentificacao).Replace("#UFexpedicao", ufDocumentodeIdentificacao).Replace("#filiação", filiacao).Replace("#UFnatural", ufNaturalidade).Replace("#dataNacimentoExtenso", dtNascimetnoCompletaporExtenso).Replace("#modalidade", identificacaodoCurso).Replace("#atoAutorizacaoCurso", atodeAutorizacaodoCurso).Replace("#cargaHoraria", cargaHorariaTotal).Replace("#dataConclusaoExntenso", datadeConclusaoporExtenso).Replace("#nomeAluno", nomeCompletodoConcluinte).Replace("#horasRelogio", cargaHorariaRelogio).Replace("#numCertidao", numero).Replace("#folhaCertidao", folhas).Replace("#livroCertidao", livro);


                switch (tipoDocumentoSelecionado)
                {
                    case 2: //2		Certidão

                        corpopronto = corpopronto.Replace("#anoLetivodeconclusao", dtDataConclusao.Date.Year.ToString());

                        break;
                    case 4://4		Diploma

                        //verifica se o curso é da modalidade normal, caso seja acresenta as inf do diploma
                        if (ddlNivelModalidade.SelectedValue == "CN")
                            corpopronto = corpopronto.Replace("#normal", "Em especial o exercício do magistério na educação infantil e nos cinco primeiros anos do ensino fundamental.");
                        else
                            corpopronto = corpopronto.Replace("#normal", ".");

                        break;

                    default:
                        break;
                }


                dadosAlunoDocumentoGerado.AlunoDocumentoId = Convert.ToInt32(hdnAlunoDocumentoId.Value);
                dadosAlunoDocumentoGerado.UsuarioId = User.Identity.Name;
                alunoCertificacaoId = Convert.ToInt32(hdnAlunoCertificacaoId.Value);


                int sequencial = 0;
                numerocertificado = GerarCodigoValidador(unidade_Ens, curso, modalidade, nivel, tipo_curso, tipoDocumentoSelecionado, dtDataConclusao.Date.Year, dadosAlunoDocumentoGerado.AlunoDocumentoId, out sequencial);

                dadosAlunoDocumentoGerado.NumeroGerado = numerocertificado;

                dadosAlunoDocumentoGerado.NomeArquivo = "CERTIFICADO_" + tseAlunoCPF.DBValue.ToString() + "_" + DateTime.Now.ToShortDateString().Replace("/", "_") + "-" + ddlTipoConclusao.SelectedItem.Text + ".pdf";
                dadosAlunoDocumentoGerado.TipoArquivo = "application/pdf";

                corpopronto = rnCertificadoEscolar.MontarHistoricoFundamental(corpopronto, identificacaoCompletadaUE, livro, folhas, numero, obs, numerocertificado, tipoDocumentoSelecionado, regional, municipioEscola);

                Pdf_nao_Assinado = pdf.gerapdfstream(corpopronto, false, cssText);

                dadosAlunoDocumentoGerado.Arquivo = Pdf_nao_Assinado;

                rnAlunoDocumentoGerado.Manter(dadosAlunoDocumentoGerado, unidade_Ens, sequencial, numerocertificado);

            }
            catch (Exception ex)
            {
                corpopronto = "Erro ao GerarCertificadoDiplomaEscolar" + ex.Message;
            }

            return corpopronto;
        }

        public string GerarNumeroCertificado(string UA, string unidade_Ens, string anoConclusao, int alunoCertificacaoId, int tipoConclusao)
        {
            RN.Certificacao.AlunoDocumentoGerado rnAlunoDocumentoGerado = new AlunoDocumentoGerado();
            string nCertificado = null;
            DataTable dtAlunoDocumentoGerado = null;

            string[] via_;
            int via;

            try
            {
                dtAlunoDocumentoGerado = rnAlunoDocumentoGerado.Listar(alunoCertificacaoId);
                //tipo Documento
                //1		Histórico Escolar
                //2		Certidão
                //3		Certificado Escolar
                //4		Diploma

                if (tipoConclusao == 2)// Certificado Escolar, não precisa de nº de Vias
                {
                    nCertificado = UA + unidade_Ens + anoConclusao + "-" + alunoCertificacaoId;

                }
                else
                {

                    if (dtAlunoDocumentoGerado.Rows.Count > 0)
                    {
                        via_ = dtAlunoDocumentoGerado.Rows[0]["NUMEROGERADO"].ToString().Split('V');
                        via = Convert.ToInt32(via_[1]) + 1;
                    }
                    else
                    {
                        via = 1;
                    }
                    nCertificado = UA + unidade_Ens + anoConclusao + "-" + alunoCertificacaoId + "V" + via;
                }


            }
            catch (Exception ex)
            {
                nCertificado = null;
                throw new Exception("Erro ao gerar número do documento.");
            }
            return nCertificado;

        }

        public string GerarCodigoValidador(string unidade_Ens, string curso, string modalidade, string nivel, string tipo_curso, int tipoDocumento, int anoConclusao, int alunoDocumentoId, out int sequencial)
        {

            RN.Certificacao.AlunoDocumentoGerado rnAlunoDocumentoGerado = new AlunoDocumentoGerado();
            RN.Certificacao.AlunoDocumento rnAlunoDocumento = new Techne.Lyceum.RN.Certificacao.AlunoDocumento();
            sequencial = 0;
            string codigo = null;
            DataTable dtAlunoDocumentoGerado = null;
            string via = string.Empty;
            int total = 0;

            try
            {
                dtAlunoDocumentoGerado = rnAlunoDocumentoGerado.Listar(alunoDocumentoId);

                if (dtAlunoDocumentoGerado.Rows.Count > 0)
                {
                    total = (dtAlunoDocumentoGerado.Rows.Count + 1);

                    if (total <= 9)
                    {
                        via = total.ToString().PadLeft(2, '0');
                    }
                }
                else
                {
                    via = "01";
                }

                var tipoDoc = tipoDocumento == 3 ? "03" : "04";

                string nivelModalidade = ddlNivelModalidade.SelectedValue;


                //Busca sequencial
                sequencial = rnAlunoDocumento.ObtemSequencialPor(unidade_Ens, tipoDocumento, alunoDocumentoId);

                string seguencialFormatado = sequencial.ToString().PadLeft(5, '0');

                codigo = tipoDoc + "." + nivelModalidade + ".01." + unidade_Ens + "." + seguencialFormatado + "." + anoConclusao.ToString().Substring(2, 2) + "." + via + "." + DateTime.Now.Year.ToString().Substring(2, 2);



            }
            catch (Exception ex)
            {
                codigo = null;
                throw new Exception("Erro ao gerar número do documento.");
            }
            return codigo;
        }

        protected void btnSalvarDadosPessoais_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Certificacao.AlunoCertificacao rnAlunoCertificacao = new Techne.Lyceum.RN.Certificacao.AlunoCertificacao();
                RN.Certificacao.Entidades.AlunoCertificacao dadosAlunoCertificacao = new Techne.Lyceum.RN.Certificacao.Entidades.AlunoCertificacao();
                RN.Endereco rnEndereco = new Endereco();
                ValidacaoDados validacao = new ValidacaoDados();
                string mensagem;

                dadosAlunoCertificacao.Nome = !txtNomeAluno.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeAluno.Text.Trim() : null;
                dadosAlunoCertificacao.NomeMae = !txtNomeMae.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeMae.Text.Trim() : null;
                dadosAlunoCertificacao.chkNaoDeclarMae = chkNaoDeclarMae.Checked;
                dadosAlunoCertificacao.NomePai = !txtNomePai.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomePai.Text.Trim() : null;
                dadosAlunoCertificacao.chkNaoDeclarPai = chkNaoDeclarPai.Checked;
                dadosAlunoCertificacao.CPF = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.Trim().RetirarMascaraCPF() : null;
                dadosAlunoCertificacao.MunicipioNascimento = !txtMunicipioNaturalidade.Text.IsNullOrEmptyOrWhiteSpace() ? txtMunicipioNaturalidade.Text.Trim() : null;
                dadosAlunoCertificacao.DataNascimento = !dtDataNasc.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataNasc.Date : DateTime.MinValue;
                dadosAlunoCertificacao.Nacionalidade = !cmbNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbNacionalidade.SelectedValue : null;

                if (!cmbNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {

                    if (cmbNacionalidade.SelectedItem.Text.ToUpper() == "BRASILEIRA")
                    {
                        dadosAlunoCertificacao.MunicipioNascimento = (!tseNaturalidade.DBValue.IsNull && tseNaturalidade.IsValidDBValue) ? tseNaturalidade.DBValue.ToString() : null;
                    }
                    else
                    {
                        dadosAlunoCertificacao.MunicipioNascimento = !txtMunicipioNaturalidade.Text.IsNullOrEmptyOrWhiteSpace() ? rnEndereco.ObtemCodigoMunicipioPor(txtMunicipioNaturalidade.Text) : null;
                    }
                }

                dadosAlunoCertificacao.RgNumero = !txtNRg.Text.IsNullOrEmptyOrWhiteSpace() ? txtNRg.Text.Trim() : null;
                dadosAlunoCertificacao.RgEmissor = !cmbRGEmissor.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbRGEmissor.SelectedValue : null;
                dadosAlunoCertificacao.RgUf = !cmbRGUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbRGUF.SelectedValue : null;
                dadosAlunoCertificacao.UsuarioResponsavel = User.Identity.Name;
                dadosAlunoCertificacao.AlunoCertificacaoId = !hdnAlunoCertificacaoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnAlunoCertificacaoId.Value) : -1;

                validacao = rnAlunoCertificacao.Validar(dadosAlunoCertificacao);

                if (validacao.Valido)
                {
                    rnAlunoCertificacao.Salva(dadosAlunoCertificacao);

                    if (hdnAlunoCertificacaoId.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        hdnAlunoCertificacaoId.Value = dadosAlunoCertificacao.AlunoCertificacaoId > 0 ? dadosAlunoCertificacao.AlunoCertificacaoId.ToString() : string.Empty;
                        tseAlunoCPF.ResetValue();
                        tseAlunoCPF.DBValue = txtCPF.Text;
                    }

                    mensagem = "Dados do Aluno salvo com sucesso.";

                    lblMensagem.Text = mensagem;

                    this._tipoOperacao = TipoOperacao.Sucesso;
                    this.ControlarTipoOperacao();


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

        protected void grdDadosEscolares_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.Reposicao.Reposicao rnReposicao = new Techne.Lyceum.RN.Reposicao.Reposicao();
            RN.Reposicao.Entidades.Reposicao reposicao = new Techne.Lyceum.RN.Reposicao.Entidades.Reposicao();

            try
            {

                if (e.ButtonID == "btnExcluir")
                {
                    hdnAlunoDocumentoId.Value = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "ALUNODOCUMENTOID"));

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }

                if (e.ButtonID == "btnEditar")
                {
                    LimparTelaDadosEscolares();

                    hdnAlunoDocumentoId.Value = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "ALUNODOCUMENTOID"));

                    ddlNivelModalidade.SelectedValue = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "MODALIDADE"));
                    ddlTipoConclusao.SelectedValue = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "TIPOCONCLUSAOID"));
                    ddlTipoDocumento.SelectedValue = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "DOCUMENTOID"));

                    tseUnidadeEns.ResetValue();

                    tseUnidadeEns.DBValue = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "UNIDADEENSINO"));
                    txtCurso.Text = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "NOMECURSO"));
                    txtAtoAutoriza.Text = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "ATOAUTORIZA"));
                    dtDataAto.Date = Convert.ToDateTime(grdDadosEscolares.GetRowValues(e.VisibleIndex, "DATAAUTORIZA"));
                    txtTotalHAula.Text = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "TOTALHORASAULA"));
                    txtTotalHRelogio.Text = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "TOTALHORASRELOGIO"));
                    dtDataConclusao.Date = Convert.ToDateTime(grdDadosEscolares.GetRowValues(e.VisibleIndex, "DATACONCLUSAO"));

                    txtNumLivro.Text = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "NUMEROLIVRO"));
                    txtFolhaLivro.Text = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "FOLHALIVRO"));
                    txtLivro.Text = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "LIVRO"));
                    txtObservacao.Text = Convert.ToString(grdDadosEscolares.GetRowValues(e.VisibleIndex, "OBSERVACAO"));

                    pnlNovoDadosEscolares.Visible = true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void AcessoGrid()
        {
            if (grdDadosEscolares != null)
            {
                HtmlInputImage img = (HtmlInputImage)grdDadosEscolares.FindHeaderTemplateControl(grdDadosEscolares.Columns[""], "btnNovoGrid");

                if (img != null)
                {
                    img.Visible = Permission.AllowInsert;

                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LimparTelaDadosEscolares();
            pnlNovoDadosEscolares.Visible = false;

        }

        protected void HabilitaPnlNovo(object sender, EventArgs e)
        {
            try
            {
                pnlNovoDadosEscolares.Visible = false;

                if (!tseAlunoCPF.DBValue.IsNull)
                {
                    LimparTelaDadosEscolares();

                    pnlNovoDadosEscolares.Visible = true;

                }
                else
                {
                    lblMensagem.Text = "Para criar uma novo dados escolares é necessário escolher um aluno.";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDadosEscolares_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDadosEscolares, AcaoControle.editar, "btnEditar");
            AcessoGrid();
        }

        protected void pcCertificacao_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (!string.IsNullOrEmpty(e.Tab.Text))
            {
                if (e.Tab.Text == "Dados Escolares")
                {
                    pnlNovoDadosEscolares.Visible = false;
                }
            }
        }

        private void ValidarCampos()
        {
            txtTotalHAula.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
            txtTotalHAula.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            txtTotalHAula.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");


            txtTotalHRelogio.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
            txtTotalHRelogio.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            txtTotalHRelogio.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

        }

        private void HabilitaBotoesWord()
        {
            btnImprimirCert.Visible = false;
            btnImprimirDipl.Visible = false;

            foreach (GridViewColumn column_tmp in grdDadosEscolares.Columns)
            {
                var tipoDocumento = (string)this.grdDadosEscolares.GetRowValues(column_tmp.VisibleIndex, "DESCRICAODOCUMENTO");

                if (tipoDocumento == "Certificado Escolar")
                    btnImprimirCert.Visible = true;
                if (tipoDocumento == "Diploma")
                    btnImprimirDipl.Visible = true;
            }
        }

        protected void btnImprimirCert_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> pares = new Dictionary<string, string>();
                pares.Add("cpf", tseAlunoCPF.DBValue.ToString());

                this.btnImprimirCert.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=RelCertificadoConcluinte&grp=Certificacao&" + CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;

            }
        }

        protected void btnImprimirDipl_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> pares = new Dictionary<string, string>();
                pares.Add("cpf", tseAlunoCPF.DBValue.ToString());

                this.btnImprimirDipl.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=RelDiplomaConcluinte&grp=Certificacao&" + CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;

            }
        }
    }
}
