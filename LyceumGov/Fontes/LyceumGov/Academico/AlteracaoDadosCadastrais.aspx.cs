using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/AlteracaoDadosCadastrais.aspx"), ControlText("AlteracaoDadosCadastrais"), Title("Alteração Dados Cadastrais")]
    public partial class AlteracaoDadosCadastrais : TPage
    {
        public enum TipoOperacao
        {
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

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                RN.Perfil rnPerfil = new Perfil();
                RN.RecursosHumanos.PeriodoAlteracaoAluno rnPeriodoAlteracaoAluno = new Techne.Lyceum.RN.RecursosHumanos.PeriodoAlteracaoAluno();
                RN.Usuarios rnUsuarios = new Usuarios();

                if (!IsPostBack)
                {
                    //Verifica se pessoa nao tem perfil para alterar em qualquer data ou se esta em periodo que permite a alteração
                    if (!rnPerfil.PossuiPerfilAlteracaoDadosCadastraisAlunoForaPeriodoPor(User.Identity.Name)
                         && !rnPeriodoAlteracaoAluno.PossuiPeriodoAlteracaoAlunoAbertoPor(DateTime.Now.Year, DateTime.Now)
                         && !rnUsuarios.EhPrivilegiado(User.Identity.Name))
                    {
                        pnBusca.Visible = false;
                        pntabDadosPessoais.Visible = false;
                        LimparTela();
                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(controles);

                        lblMensagem.Text = "O período para alteração dos dados cadastrais do aluno não está aberto.";

                        return;
                    }
                    else
                    {
                        CarregaNacionalidade();
                        CarregaEtnia();
                        CarregaEstadoCivil();
                        CarregaRGTipoPessoa();
                        CarregaRGEmissor();
                        CarregaUfRg();
                        CarregaUfCertNasc();
                        CarregaUFCartorio();

                        this._tipoOperacao = TipoOperacao.Inicial;
                        this.ControlarTipoOperacao();

                        dtDataExped.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        dboDOC_CertNasc_DtEmissao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);

            bool nascidoFora = !string.IsNullOrEmpty(txtPaisNasc.Text)
                               && txtPaisNasc.Text.Trim().ToUpper() != "BRASIL";

            bool modoEdicao = (_tipoOperacao == TipoOperacao.Alterar);

            if (modoEdicao)
            {
                lblPaisNasc.Visible = nascidoFora;
                txtPaisNasc.Visible = nascidoFora;
            }
            else
            {
                lblPaisNasc.Visible = !string.IsNullOrEmpty(txtPaisNasc.Text);
                txtPaisNasc.Visible = !string.IsNullOrEmpty(txtPaisNasc.Text);
            }

            lblNaturalidadeUF.Visible = true;
            txtUFNascimento.Visible = true;

            if (modoEdicao)
            {
                tseNaturalidade.Visible = false;
                tseNaturalidadeEstrangeira.Visible = false;

                if (!string.IsNullOrEmpty(ddlNacionalidade.SelectedValue))
                {
                    tseNaturalidade.Visible = !nascidoFora;
                    tseNaturalidadeEstrangeira.Visible = nascidoFora;
                }
            }
            else
            {
                tseNaturalidade.Visible = !nascidoFora;
                tseNaturalidadeEstrangeira.Visible = nascidoFora;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
        }

        private void CarregaUFCartorio()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlUFCartorio.Items.Clear();
            ddlUFCartorio.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.UfCartorio, RN.Basico.QueryListaUFCartorio);
            ddlUFCartorio.DataBind();
            ddlUFCartorio.Items.Insert(0, item);
        }

        private void CarregaEtnia()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlEtnia.Items.Clear();
            ddlEtnia.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Etnia, RN.Basico.QueryListaEtniaAtiva);
            ddlEtnia.DataBind();
            ddlEtnia.Items.Insert(0, item);
        }

        private void CarregaEstadoCivil()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlEst_Civil.Items.Clear();
            ddlEst_Civil.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.EstadoCivil);
            ddlEst_Civil.DataBind();
            ddlEst_Civil.Items.Insert(0, item);
        }

        private void CarregaNacionalidade()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlNacionalidade.Items.Clear();
            ddlNacionalidade.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Nacionalidade, RN.Basico.QueryListaNacionalidades);
            ddlNacionalidade.DataBind();
            ddlNacionalidade.Items.Insert(0, item);

            item = ddlNacionalidade.Items.FindByText("BRASILEIRA");
            if (item != null)
            {
                ddlNacionalidade.ClearSelection();
                item.Selected = true;
            }
        }

        private void CarregaRGTipoPessoa()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlRGTipoPessoa.Items.Clear();
            ddlRGTipoPessoa.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.RgTipoPessoa);
            ddlRGTipoPessoa.DataBind();
            ddlRGTipoPessoa.Items.Insert(0, item);
        }

        private void CarregaRGEmissor()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            cmbRGEmissor.Items.Clear();
            cmbRGEmissor.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.RgEmissor);
            cmbRGEmissor.DataBind();
            cmbRGEmissor.Items.Insert(0, item);
        }

        private void CarregaUfRg()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object dtUf;

            dtUf = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Uf, RN.Basico.QueryListaUF);

            cmbRGUF.Items.Clear();
            cmbRGUF.DataSource = dtUf;
            cmbRGUF.DataBind();
            cmbRGUF.Items.Insert(0, item);
        }

        private void CarregaUfCertNasc()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object dtUf;

            dtUf = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Uf, RN.Basico.QueryListaUF);

            ddDOC_CertNasc_Uf.Items.Clear();
            ddDOC_CertNasc_Uf.DataSource = dtUf;
            ddDOC_CertNasc_Uf.DataBind();
            ddDOC_CertNasc_Uf.Items.Insert(0, item);
        }

        private void LimparTela()
        {
            hddDataAlteracaoEmail.Value = string.Empty;
            tseNaturalidade.ResetValue();
            txtPessoa.Text = string.Empty;
            txtNomeCompl.Text = string.Empty;
            txtNomeSocial.Text = string.Empty;
            txtFilhos.Text = string.Empty;
            lblMensagem.Text = string.Empty;
            dtDataNasc.Text = string.Empty;
            rblSexo.SelectedIndex = -1;
            ddlEtnia.SelectedValue = string.Empty;
            txtEndereco.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtFone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            hddTxtEmail.Value = string.Empty;
            txtEmail.Text = string.Empty;
            ddlRGTipoPessoa.SelectedValue = string.Empty;
            txtRGNum.Text = string.Empty;
            cmbRGEmissor.SelectedValue = string.Empty;
            dtDataExped.Text = string.Empty;
            txtDOC_CertNasc_Numero.Text = string.Empty;
            txtDOC_CertNasc_Folha.Text = string.Empty;
            txtDOC_CertNasc_Livro.Text = string.Empty;
            ddlCartorio.Items.Clear();
            dboDOC_CertNasc_DtEmissao.Text = string.Empty;
            ddDOC_CertNasc_Uf.SelectedValue = string.Empty;
            ddlNacionalidade.SelectedValue = string.Empty;
            ddlEst_Civil.SelectedValue = string.Empty;
            txtMunicipio.Text = string.Empty;
            hdnCodMunicipio.Value = string.Empty;
            txtPessoa.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtCPF.Text = string.Empty;
            ddlUFCartorio.ClearSelection();
            ddlMunicipioCartorio.Items.Clear();
            ddlCartorio.Items.Clear();
            txtNumMatriculaCertidao.Text = string.Empty;
            txtComplIdentidade.Text = string.Empty;
            ddlCertidaoCivil.ClearSelection();
            pnlNovo.Visible = false;
            pnlAntigo.Visible = false;
            pnlTipoCertidaoCivil.Visible = false;
            txtNomePai.Text = string.Empty;
            txtNomeMae.Text = string.Empty;
            chkNaoDeclarMae.Checked = false;
            chkNaoDeclarPai.Checked = false;
            chkFalecidaMae.Checked = false;
            chkFalecidoPai.Checked = false;
            chkDeclaroAusenciaMae.Checked = false;
            chkDeclaroAusenciaPai.Checked = false;
            chkDeclaroCertidaoCivil.Checked = false;
            chkDeclaroAusenciaMae.Visible = false;
            chkDeclaroAusenciaPai.Visible = false;
            chkDeclaroCertidaoCivil.Visible = false;
            txtCPFPai.Text = string.Empty;
            txtCPFMae.Text = string.Empty;
            txtTelefoneMae.Text = string.Empty;
            txtTelefonePai.Text = string.Empty;
            rblResponsavel.ClearSelection();
            txtNomeResponsavel.Text = string.Empty;
            txtCPFResponsavel.Text = string.Empty;
            txtTelefoneResp.Text = string.Empty;
            txtNomeResponsavel.Visible = false;
            lblNomeResponsavel.Visible = false;
            txtCPFResponsavel.Visible = false;
            lblCPFResponsavel.Visible = false;
            txtTelefoneResp.Visible = false;
            lblTelefoneResponsavel.Visible = false;
            txtMotivoCertidaoCivil.Text = string.Empty;
            txtMotivoCertidaoCivil.Visible = false;
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewState["confirmaalteracao"] = null;
                _tipoOperacao = TipoOperacao.Inicial;

                tseAluno.Enabled = true;
                lblMensagem.Text = string.Empty;
                LimparTela();

                ControlarTipoOperacao();
                ControlarTSearchs();
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
                RN.Aluno rnAluno = new Aluno();

                if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
                {

                    _tipoOperacao = TipoOperacao.Alterar;
                    ControlarTipoOperacao();
                    ControlarTSearchs();
                }
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                this.Salvar();

                this.pucConfirmar.ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmar.ShowOnPageLoad = false;
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcesso(btnEditar, AcaoControle.editar);

        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnSalvar.Visible = false;
        }

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(controles);

                        tseAluno.ResetValue();

                        pntabDadosPessoais.Visible = false;
                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = true;

                        txtPessoa.Visible = false;
                        lblPessoa.Visible = false;

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnEditar };
                        ControlarVisibilidadeControle(controles);

                        DesabilitaCampos();

                        tseAluno.Enabled = true;
                        tseNaturalidade.Mode = ControlMode.View;
                        tseNaturalidade.Enabled = false;
                        tseNaturalidadeEstrangeira.Mode = ControlMode.View;
                        tseNaturalidadeEstrangeira.Enabled = false;

                        txtPessoa.Visible = false;
                        lblPessoa.Visible = false;
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
                        {
                            RN.Aluno rnAluno = new Aluno();
                            string censoAluno = Aluno.ConsultarCenso(tseAluno.Text);
                            RN.EventoGeral rnEventoGeral = new EventoGeral();
                            HabilitaCampos();

                            btnEditar.Visible = false;
                            txtPessoa.Visible = false;


                            ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                            ControlarVisibilidadeControle(controles);
                            lblMensagem.Text = String.Empty;

                            lbltxtPessoa.Visible = false;
                            txtPessoa.Visible = true;
                            txtPessoa.Visible = false;
                            lblPessoa.Visible = false;


                            break;
                        }
                        else
                        {
                            this._tipoOperacao = TipoOperacao.Inicial;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";

                            break;
                        }
                    }
                case TipoOperacao.Consultar:
                    {
                        tseAluno.Enabled = true;
                        tseNaturalidade.Mode = ControlMode.View;
                        tseNaturalidade.Enabled = false;
                        tseNaturalidadeEstrangeira.Mode = ControlMode.View;
                        tseNaturalidadeEstrangeira.Enabled = false;

                        lbltxtPessoa.Visible = false;
                        txtPessoa.Visible = true;

                        lblMensagem.Text = string.Empty;
                        LimparTela();
                        LimparEndereco();

                        var dadosAluno = Aluno.Carregar(tseAluno.DBValue.ToString());

                        if (!IsPostBack)
                        {
                            if (Request.QueryString.Keys.Count > 0)
                            {
                                string aluno = string.Empty;
                                if (!string.IsNullOrEmpty(Request.QueryString["Chave"]))
                                {
                                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                                    aluno = decodedText.Substring(decodedText.LastIndexOf('=') + 1);
                                }
                                else if (!string.IsNullOrEmpty(Request.QueryString["Aluno"]))
                                {
                                    aluno = Request.QueryString["Aluno"];
                                }
                                else if (!String.IsNullOrEmpty(Request.QueryString["ChaveConfirmacao"]))
                                {
                                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveConfirmacao"]);
                                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                                    aluno = decodedText.Substring(decodedText.LastIndexOf('=') + 1);
                                }

                                dadosAluno.Aluno = aluno;
                            }
                        }

                        if (dadosAluno.Aluno == null)
                        {
                            lblMensagem.Text = "Aluno não encontrado.";

                            ImageButton[] controles = new ImageButton[] { };
                            ControlarVisibilidadeControle(controles);
                        }
                        else
                        {
                            txtPessoa.Text = Convert.ToString(dadosAluno.Pessoa);
                            ImageButton[] controles = new ImageButton[] { btnEditar };

                            pntabDadosPessoais.Visible = true;
                            CarregaDadosAluno(dadosAluno);

                            var dadosPessoa = Pessoa.Carregar(Convert.ToInt32(dadosAluno.Pessoa));

                            if (dadosPessoa != null)
                            {
                                CarregaDadosPessoa(dadosPessoa);
                            }
                            else
                            {
                                lblMensagem.Text = "Pessoa não cadastrada para este aluno.";
                                return;
                            }

                            var dadosFieldPessoa = FlPessoa.Carregar(Convert.ToDecimal(dadosAluno.Pessoa));

                            if (dadosFieldPessoa != null)
                            {
                                CarregaDadosFieldPessoa(dadosFieldPessoa);
                            }
                            else
                            {
                                ddlLocalZona.SelectedValue = "";
                                ddlTipoCertidao.SelectedValue = "";

                            }

                            if (dadosAluno.SitAluno != "Ativo")
                            {
                                controles = new ImageButton[] { };

                                lblMensagem.Text = "Não é possível alterar dados de um aluno que não está ativo.";

                            }

                            ControlarVisibilidadeControle(controles);
                            DesabilitaCampos();
                            txtPessoa.Visible = false;
                        }

                        break;
                    }
            }
        }

        private void CarregaDadosPessoa(LyPessoa dadosPessoa)
        {
            string dataLimite = "31/01/" + (DateTime.Now.Year + 1);

            txtPessoa.Text = dadosPessoa.Pessoa > 0 ? Convert.ToString(dadosPessoa.Pessoa) : string.Empty;
            txtNomeCompl.Text = !dadosPessoa.Nome_compl.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Nome_compl : string.Empty;

            if (dadosPessoa.Dt_nasc.HasValue)
            {
                dtDataNasc.Date = dadosPessoa.Dt_nasc.Value;
            }

            txtNomeSocial.Text = !dadosPessoa.PreNomeSocial.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.PreNomeSocial : string.Empty;

            if (!string.IsNullOrEmpty(dadosPessoa.Sexo))
            {
                if (rblSexo.Items.FindByValue(dadosPessoa.Sexo) != null)
                {
                    rblSexo.Text = dadosPessoa.Sexo;
                }
            }
            txtEndereco.Text = !dadosPessoa.Endereco.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Endereco : string.Empty;
            txtEndNum.Text = !dadosPessoa.End_num.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.End_num : string.Empty;
            txtEndCompl.Text = !dadosPessoa.End_compl.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.End_compl : string.Empty;
            txtCep.Text = !dadosPessoa.Cep.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Cep : string.Empty;
            txtBairro.Text = !dadosPessoa.Bairro.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Bairro : string.Empty;
            txtEmail.Text = !dadosPessoa.E_mail.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.E_mail : string.Empty;
            hddTxtEmail.Value = !dadosPessoa.E_mail.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.E_mail : string.Empty;
            txtRGNum.Text = !dadosPessoa.Rg_num.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.Rg_num : string.Empty;
            txtFilhos.Text = dadosPessoa.QtFilhos.HasValue ? Convert.ToString(dadosPessoa.QtFilhos) : string.Empty;
            txtDOC_CertNasc_Numero.Text = !dadosPessoa.CertNascNum.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.CertNascNum : string.Empty;
            txtDOC_CertNasc_Folha.Text = !dadosPessoa.CertNascFolha.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.CertNascFolha : string.Empty;
            txtDOC_CertNasc_Livro.Text = !dadosPessoa.CertNascLivro.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.CertNascLivro : string.Empty;
            txtNumMatriculaCertidao.Text = !dadosPessoa.CertNumeroMatricula.IsNullOrEmptyOrWhiteSpace() ? dadosPessoa.CertNumeroMatricula : string.Empty;

            if (dadosPessoa.CertNascEmissao.HasValue)
            {
                dboDOC_CertNasc_DtEmissao.Date = dadosPessoa.CertNascEmissao.Value;
            }

            //PreencherDadoCombo(ddlUFCartorio, Convert.ToString(dadosPessoa.CodigoUf));
            //if (!string.IsNullOrEmpty(dadosPessoa.CodigoUf))
            //{
            //    this.CarregaMunicipioCartorio();
            //    PreencherDadoCombo(ddlMunicipioCartorio, Convert.ToString(dadosPessoa.CodigoMunicipio));
            //}

            if (dadosPessoa.IdCartorio != null && dadosPessoa.IdCartorio != 0)
            {
                CarregaCartorio();
                PreencherDadoCombo(ddlCartorio, Convert.ToString(dadosPessoa.IdCartorio));
            }

            if (!string.IsNullOrEmpty(dadosPessoa.Pais_nasc))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosPessoa.Pais_nasc);
                bool ehEstrangeiro = descricaoPais.ToUpper() != "BRASIL";

                if (ehEstrangeiro)
                {
                    tseNaturalidade.ResetValue();
                    txtPaisNasc.Text = descricaoPais;
                    tseNaturalidadeEstrangeira.Enabled = true;

                    if (!string.IsNullOrEmpty(dadosPessoa.Municipio_nasc))
                    {
                        tseNaturalidadeEstrangeira.DBValue = dadosPessoa.Municipio_nasc;

                        if (tseNaturalidadeEstrangeira.IsValidDBValue && !tseNaturalidadeEstrangeira.DBValue.IsNull)
                        {
                            // UF e País preenchidos automaticamente pelo TSE
                            txtUFNascimento.Text = tseNaturalidadeEstrangeira["ESTADO"].ToString();
                            txtPaisNasc.Text = tseNaturalidadeEstrangeira["PAIS"].ToString();
                        }
                    }
                }
                else
                {
                    tseNaturalidadeEstrangeira.ResetValue();
                    txtPaisNasc.Text = string.Empty;

                    if (!string.IsNullOrEmpty(dadosPessoa.Municipio_nasc))
                    {
                        tseNaturalidadeEstrangeira.ResetValue();
                        txtPaisNasc.Text = string.Empty;

                        if (!string.IsNullOrEmpty(dadosPessoa.Municipio_nasc))
                        {
                            tseNaturalidade.DBValue = dadosPessoa.Municipio_nasc;
                            if (tseNaturalidade.IsValidDBValue && !tseNaturalidade.DBValue.IsNull)
                                txtUFNascimento.Text = tseNaturalidade["uf_sigla"].ToString();
                            else
                                tseNaturalidade.ResetValue();
                        }
                    }
                }
            }

            // verifica se existe valor para municipio
            if (!string.IsNullOrEmpty(dadosPessoa.End_municipio))
            {
                // preenche os dados nos controles da tela
                hdnCodMunicipio.Value = dadosPessoa.End_municipio;
                txtMunicipio.Text = dadosPessoa.NomeMunicipio;

                // obtém a UF de acordo com o codigo do municipío
                txtEstado.Value = Endereco.ObterUFMunicipio(dadosPessoa.End_municipio);
            }
            else
            {
                hdnCodMunicipio.Value = string.Empty;
                txtMunicipio.Text = string.Empty;
                txtEstado.Value = string.Empty;
            }

            //PreencherDadoCombo(ddlPaisNasc, Convert.ToString(dadosPessoa.Pais_nasc));

            //if (!string.IsNullOrEmpty(dadosPessoa.Est_civil))
            //{
            //    if (ddlEst_Civil.Items.FindByValue(dadosPessoa.Est_civil) != null)
            //    {
            //        ddlEst_Civil.SelectedValue = dadosPessoa.Est_civil;
            //    }
            //}

            if (!string.IsNullOrEmpty(dadosPessoa.Nacionalidade))
            {
                if (ddlNacionalidade.Items.FindByValue(dadosPessoa.Nacionalidade) != null)
                {
                    ddlNacionalidade.SelectedValue = dadosPessoa.Nacionalidade;
                }
            }

            PreencherDadoCombo(ddlEtnia, Convert.ToString(dadosPessoa.Etnia));
            PreencherDadoCombo(cmbRGUF, Convert.ToString(dadosPessoa.Rg_uf));
            PreencherDadoCombo(cmbRGEmissor, Convert.ToString(dadosPessoa.Rg_emissor));
            PreencherDadoCombo(ddlRGTipoPessoa, Convert.ToString(dadosPessoa.Rg_tipo));
            PreencherDadoCombo(ddDOC_CertNasc_Uf, Convert.ToString(dadosPessoa.CertNascCartorioUf));

            this.ControlarObrigatoriedadeDocumentos(ddlRGTipoPessoa.Text);

            long result;

            if (long.TryParse(dadosPessoa.Cpf, out result))
            {
                if (result != 0)
                {
                    txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", result);
                }
                else
                {
                    txtCPF.Text = string.Empty;
                }
            }
            else
            {
                txtCPF.Text = dadosPessoa.Cpf;
            }

            long resultadoFixoCelular;
            var fixoCelular = dadosPessoa.Fone.RetirarMascaraTelefone();

            if (long.TryParse(fixoCelular, out resultadoFixoCelular))
            {
                if (fixoCelular.Length == 10)
                {
                    txtFone.Text = string.Format("{0:(00)0000-0000}", resultadoFixoCelular);
                }
                else if (fixoCelular.Length == 11)
                {
                    txtFone.Text = string.Format("{0:(00)00000-0000}", resultadoFixoCelular);
                }
                else
                {
                    txtFone.Text = resultadoFixoCelular.ToString();

                }
            }
            long resultado;
            var celular = dadosPessoa.Celular.RetirarMascaraTelefone();
            if (long.TryParse(celular, out resultado))
            {
                if (celular.Length == 10)
                {
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                }
                else if (celular.Length == 11)
                {
                    txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                }
                else
                {
                    txtCelular.Text = resultado.ToString();
                }
            }


            if (dadosPessoa.Rg_dtexp.HasValue)
                dtDataExped.Date = dadosPessoa.Rg_dtexp.Value;

            txtNomeMae.Text = dadosPessoa.NomeMae;
            txtNomePai.Text = dadosPessoa.NomePai;


            if (!string.IsNullOrEmpty(dadosPessoa.Responsavel))
            {
                string[] tipo_resp = dadosPessoa.Responsavel.Split(';');
                foreach (String str in tipo_resp)
                {
                    if (!string.IsNullOrEmpty(str) && (rblResponsavel.Items.FindByValue(str) != null))
                    {
                        rblResponsavel.Items.FindByValue(str).Selected = true;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosPessoa.RespNomeCompl))
            {
                txtTelefoneResp.Visible = true;
                txtCPFResponsavel.Visible = true;
                txtNomeResponsavel.Visible = true;
                lblTelefoneResponsavel.Visible = true;
                lblCPFResponsavel.Visible = true;
                lblNomeResponsavel.Visible = true;

                txtNomeResponsavel.Text = dadosPessoa.RespNomeCompl;
                if (Int64.TryParse(dadosPessoa.RespCpf, out result))
                {
                    if (result != 0)
                        txtCPFResponsavel.Text = string.Format(@"{0:000\.000\.000-00}", result);
                    else
                        txtCPFResponsavel.Text = "";
                }
                else
                    txtCPFResponsavel.Text = dadosPessoa.RespCpf;

                if (Int64.TryParse(dadosPessoa.RespFone, out result))
                    txtTelefoneResp.Text = string.Format("{0:(00)0000-0000}", result);
                else
                    txtTelefoneResp.Text = dadosPessoa.RespFone;

            }

            if (Int64.TryParse(dadosPessoa.MaeCpf, out result))
            {
                if (result != 0)
                    txtCPFMae.Text = string.Format(@"{0:000\.000\.000-00}", result);
                else
                    txtCPFMae.Text = "";
            }
            else
                txtCPFMae.Text = dadosPessoa.MaeCpf;

            if (Int64.TryParse(dadosPessoa.PaiCpf, out result))
            {
                if (result != 0)
                    txtCPFPai.Text = string.Format(@"{0:000\.000\.000-00}", result);
                else
                    txtCPFPai.Text = "";
            }
            else
                txtCPFPai.Text = dadosPessoa.PaiCpf;

            if (Int64.TryParse(dadosPessoa.MaeTelefone, out result))
                txtTelefoneMae.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtTelefoneMae.Text = dadosPessoa.MaeTelefone;

            if (Int64.TryParse(dadosPessoa.PaiTelefone, out result))
                txtTelefonePai.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtTelefonePai.Text = dadosPessoa.PaiTelefone;

            chkFalecidaMae.Checked = dadosPessoa.MaeFalecida == "S";

            chkNaoDeclarMae.Checked = dadosPessoa.NomeMae == chkNaoDeclarMae.Text.ToUpper();


            if (chkFalecidaMae.Checked || chkNaoDeclarMae.Checked)
                DesabilitaResponsavelLegal("H", "Mãe");

            if (chkNaoDeclarMae.Checked)
            {
                txtCPFMae.Text = string.Empty;
                txtTelefoneMae.Text = string.Empty;
                chkFalecidaMae.Checked = false;
                txtNomeMae.ReadOnly = true;
                txtCPFMae.Enabled = false;
                txtTelefoneMae.Enabled = false;
                chkFalecidaMae.Enabled = false;
            }
            chkNaoDeclarPai.Checked = dadosPessoa.NomePai == chkNaoDeclarPai.Text.ToUpper();


            if (chkFalecidoPai.Checked || chkNaoDeclarPai.Checked)
                DesabilitaResponsavelLegal("H", "Pai");

            if (chkNaoDeclarPai.Checked)
            {
                txtCPFPai.Text = string.Empty;
                txtTelefonePai.Text = string.Empty;
                txtNomePai.ReadOnly = true;
                txtCPFPai.Enabled = false;
                txtTelefonePai.Enabled = false;
                chkFalecidoPai.Checked = false;
            }

            chkAreaAssentamento.Checked = !dadosPessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaAssentamento == "S" ? true : false) : false;
            chkTerraIndigena.Checked = !dadosPessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.TerraIndigena == "S" ? true : false) : false;
            chkQuilombos.Checked = !dadosPessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaQuilombos == "S" ? true : false) : false;
            chkAreaTradicional.Checked = !dadosPessoa.AreaTradicional.IsNullOrEmptyOrWhiteSpace() ? (dadosPessoa.AreaTradicional == "S" ? true : false) : false;
            chkNaoSeAplica.Checked = (!chkAreaAssentamento.Checked && !chkTerraIndigena.Checked && !chkQuilombos.Checked && !chkAreaTradicional.Checked) ? true : false;

        }

        private void ControlarObrigatoriedadeDocumentos(string tipoDocumento)
        {
            if (tipoDocumento == "RG")
            {
                lblRG_Num.Text = "Número*: ";
                lblRG_Num.Font.Bold = true;

                lblRG_UF.Text = "Estado*: ";
                lblRG_UF.Font.Bold = true;

                lblRG_Emissor.Text = "Órgão Emissor*: ";
                lblRG_Emissor.Font.Bold = true;

                lblRG_Data.Text = "Data de Expedição*: ";
                lblRG_Data.Font.Bold = true;
            }
            else if (tipoDocumento == string.Empty)
            {
                lblRG_Num.Text = "Número: ";
                lblRG_Num.Font.Bold = false;

                lblRG_UF.Text = "Estado: ";
                lblRG_UF.Font.Bold = false;

                lblRG_Emissor.Text = "Órgão Emissor: ";
                lblRG_Emissor.Font.Bold = false;

                lblRG_Data.Text = "Data de Expedição: ";
                lblRG_Data.Font.Bold = false;
            }
            else
            {
                lblRG_Num.Text = "Número*: ";
                lblRG_Num.Font.Bold = true;

                lblRG_UF.Text = "Estado: ";
                lblRG_UF.Font.Bold = false;

                lblRG_Emissor.Text = "Órgão Emissor*: ";
                lblRG_Emissor.Font.Bold = true;

                lblRG_Data.Text = "Data de Expedição: ";
                lblRG_Data.Font.Bold = false;
            }
        }

        private void CarregaCartorio()
        {
            if (!string.IsNullOrEmpty(ddlMunicipioCartorio.SelectedValue))
            {
                RN.Basico rnBasico = new Techne.Lyceum.RN.Basico();
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlCartorio.Items.Clear();
                ddlCartorio.DataSource = rnBasico.ObtemListaCartorioPor(ddlUFCartorio.SelectedValue, ddlMunicipioCartorio.SelectedValue.ToString());
                ddlCartorio.DataBind();
                ddlCartorio.Items.Insert(0, item);
            }
        }

        private void CarregaMunicipioCartorio()
        {
            if (!string.IsNullOrEmpty(ddlUFCartorio.SelectedValue))
            {
                RN.Basico rnBasico = new Techne.Lyceum.RN.Basico();
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlMunicipioCartorio.Items.Clear();
                ddlMunicipioCartorio.DataSource = rnBasico.ObtemListaMunicipioCartorioPor(ddlUFCartorio.SelectedValue);
                ddlMunicipioCartorio.DataBind();
                ddlMunicipioCartorio.Items.Insert(0, item);
            }
        }

        private void CarregaDadosAluno(LyAluno dadosAluno)
        {
            hddDataAlteracaoEmail.Value = dadosAluno.DataAtualizacaoEmailInterno.HasValue ? dadosAluno.DataAtualizacaoEmailInterno.Value.ToString() : string.Empty;
            txtDataAtualizacaoEmail.Text = hddDataAlteracaoEmail.Value;
        }

        protected void DesabilitaResponsavelLegal(string operacao, string filiacao)
        {
            foreach (ListItem item in rblResponsavel.Items)
            {
                if (item.Text == filiacao)
                {
                    if (operacao == "H")
                    {
                        item.Selected = false;
                        item.Enabled = false;
                        return;
                    }
                    if (operacao == "D")
                    {
                        item.Selected = false;
                        item.Enabled = true;
                        return;
                    }
                }
            }
        }

        private void CarregaDadosFieldPessoa(LyFlPessoa dadosFieldPessoa)
        {
            ddlLocalZona.ClearSelection();
            ddlTipoCertidao.ClearSelection();
            ddlCertidaoCivil.ClearSelection();
            txtComplIdentidade.ReadOnly = true;
            txtComplIdentidade.Text = string.Empty;

            pnlNovo.Visible = false;
            pnlAntigo.Visible = false;

            if (!dadosFieldPessoa.FlField01.IsNullOrEmptyOrWhiteSpace())
            {
                PreencherDadoCombo(ddlLocalZona, dadosFieldPessoa.FlField01);
            }
            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField02))
            {
                PreencherDadoCombo(ddlTipoCertidao, dadosFieldPessoa.FlField02);

                if (dadosFieldPessoa.FlField02 == "Nenhum")
                {
                    pnlTipoCertidaoCivil.Visible = true;
                }
            }

            if (ddlNacionalidade.SelectedItem.Text == "BRASILEIRA")
            {
                if (!string.IsNullOrEmpty(ddlRGTipoPessoa.SelectedValue))
                {
                    if (ddlRGTipoPessoa.SelectedValue == "RG")
                    {
                        if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField07))
                        {
                            txtComplIdentidade.Text = dadosFieldPessoa.FlField07;
                            txtComplIdentidade.ReadOnly = false;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosFieldPessoa.FlField09))
            {
                PreencherDadoCombo(ddlCertidaoCivil, dadosFieldPessoa.FlField09);

                if (ddlCertidaoCivil.SelectedValue == "Modelo Novo")
                {
                    pnlNovo.Visible = true;
                    pnlAntigo.Visible = false;
                }
                else if (ddlCertidaoCivil.SelectedValue == "Modelo Antigo")
                {
                    pnlNovo.Visible = false;
                    pnlAntigo.Visible = true;
                }
            }
        }

        protected void DesabilitaCampos()
        {
            tseNaturalidade.Mode = ControlMode.View;
            tseNaturalidadeEstrangeira.Mode = ControlMode.View;

            chkAreaAssentamento.Enabled = false;
            chkTerraIndigena.Enabled = false;
            chkQuilombos.Enabled = false;
            chkAreaTradicional.Enabled = false;
            chkNaoSeAplica.Enabled = false;
            chkFalecidaMae.Enabled = false;
            chkFalecidoPai.Enabled = false;
            chkNaoDeclarMae.Enabled = false;
            chkNaoDeclarPai.Enabled = false;
            chkDeclaroAusenciaMae.Enabled = false;
            chkDeclaroAusenciaPai.Enabled = false;
            chkDeclaroCertidaoCivil.Enabled = false;

            cmbRGUF.Enabled = false;
            cmbRGEmissor.Enabled = false;

            dboDOC_CertNasc_DtEmissao.Enabled = false;
            ddDOC_CertNasc_Uf.Enabled = false;

            dtDataExped.Enabled = false;
            dtDataNasc.Enabled = false;
            ddlCartorio.Enabled = false;
            ddlNacionalidade.Enabled = false;
            ddlEst_Civil.Enabled = false;
            ddlEtnia.Enabled = false;
            ddlRGTipoPessoa.Enabled = false;
            ddlLocalZona.Enabled = false;
            ddlTipoCertidao.Enabled = false;
            ddlCertidaoCivil.Enabled = false;
            ddlUFCartorio.Enabled = false;
            ddlMunicipioCartorio.Enabled = false;

            txtNomeCompl.ReadOnly = true;
            txtFone.ReadOnly = true;
            txtCelular.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtRGNum.ReadOnly = true;
            txtCep.ReadOnly = true;
            txtEndCompl.ReadOnly = true;
            txtEndNum.ReadOnly = true;
            txtEndereco.ReadOnly = true;
            txtBairro.ReadOnly = true;
            txtMunicipio.ReadOnly = true;
            txtEstado.Attributes.Add("readonly", "readonly");
            txtCPF.ReadOnly = true;
            txtDOC_CertNasc_Numero.ReadOnly = true;
            txtDOC_CertNasc_Folha.ReadOnly = true;
            txtDOC_CertNasc_Livro.ReadOnly = true;
            txtNomeSocial.ReadOnly = true;
            txtFilhos.ReadOnly = true;
            txtNomeMae.ReadOnly = true;
            txtNomePai.ReadOnly = true;
            txtNomeResponsavel.ReadOnly = true;
            txtCPFMae.ReadOnly = true;
            txtCPFPai.ReadOnly = true;
            txtCPFResponsavel.ReadOnly = true;
            txtTelefoneMae.ReadOnly = true;
            txtTelefonePai.ReadOnly = true;
            txtTelefoneResp.ReadOnly = true;
            txtMotivoCertidaoCivil.ReadOnly = true;
            txtNumMatriculaCertidao.ReadOnly = true;
            txtComplIdentidade.ReadOnly = true;

            rblSexo.Enabled = false;
            rblResponsavel.Enabled = false;

            tsCEP.ShowButton = false;
        }

        protected void HabilitaCampos()
        {
            tseNaturalidade.Mode = ControlMode.Edit;
            tseNaturalidadeEstrangeira.Mode = ControlMode.View;
            tseNaturalidade.Enabled = true;
            tseNaturalidadeEstrangeira.Enabled = true;

            chkAreaAssentamento.Enabled = true;
            chkTerraIndigena.Enabled = true;
            chkQuilombos.Enabled = true;
            chkAreaTradicional.Enabled = true;
            chkNaoSeAplica.Enabled = true;
            chkNaoDeclarMae.Enabled = true;
            chkNaoDeclarPai.Enabled = true;
            chkDeclaroAusenciaMae.Enabled = true;
            chkDeclaroCertidaoCivil.Enabled = true;
            chkDeclaroAusenciaPai.Enabled = true;

            cmbRGUF.Enabled = true;
            cmbRGEmissor.Enabled = true;

            ddlTipoCertidao.Enabled = true;
            ddlNacionalidade.Enabled = true;
            ddlCartorio.Enabled = true;
            ddlEst_Civil.Enabled = true;
            ddlEtnia.Enabled = true;
            ddlRGTipoPessoa.Enabled = true;
            ddlLocalZona.Enabled = true;
            ddlCertidaoCivil.Enabled = true;
            ddlUFCartorio.Enabled = true;
            ddlMunicipioCartorio.Enabled = true;

            dtDataNasc.Enabled = true;
            dtDataExped.Enabled = true;

            dboDOC_CertNasc_DtEmissao.Enabled = true;
            ddDOC_CertNasc_Uf.Enabled = true;

            txtNomeCompl.ReadOnly = false;
            txtFone.ReadOnly = false;
            txtCelular.ReadOnly = false;
            txtRGNum.ReadOnly = false;
            txtCPF.ReadOnly = false;
            txtDOC_CertNasc_Numero.ReadOnly = false;
            txtDOC_CertNasc_Folha.ReadOnly = false;
            txtDOC_CertNasc_Livro.ReadOnly = false;
            txtCep.ReadOnly = false;
            txtEndCompl.ReadOnly = false;
            txtEndNum.ReadOnly = false;
            txtEndereco.ReadOnly = false;
            txtBairro.ReadOnly = false;
            txtEstado.Attributes.Add("readonly", "readonly");
            txtNomeSocial.ReadOnly = false;
            txtFilhos.ReadOnly = false;
            txtMunicipio.ReadOnly = false;
            txtComplIdentidade.ReadOnly = false;
            txtNumMatriculaCertidao.ReadOnly = false;
            txtNomeResponsavel.ReadOnly = false;
            txtCPFResponsavel.ReadOnly = false;
            txtTelefoneResp.ReadOnly = false;
            txtMotivoCertidaoCivil.ReadOnly = false;
            txtEmail.ReadOnly = false;

            rblSexo.Enabled = true;          
            rblResponsavel.Enabled = true;

            tsCEP.ShowButton = true;

            if (!chkNaoDeclarMae.Checked)
            {
                txtNomeMae.ReadOnly = false;
                txtTelefoneMae.ReadOnly = false;
                txtCPFMae.ReadOnly = false;
                chkFalecidaMae.Enabled = true;
                txtTelefoneMae.Enabled = true;
                txtCPFMae.Enabled = true;
                chkDeclaroAusenciaMae.Visible = false;
            }
            else
            {
                txtNomeMae.ReadOnly = true;
                txtTelefoneMae.Enabled = false;
                txtCPFMae.Enabled = false;
                chkFalecidaMae.Enabled = false;
                chkDeclaroAusenciaMae.Visible = true;
            }

            if (!chkNaoDeclarPai.Checked)
            {
                txtNomePai.ReadOnly = false;
                txtCPFPai.ReadOnly = false;
                txtTelefonePai.ReadOnly = false;
                chkFalecidoPai.Enabled = true;
                txtTelefonePai.Enabled = true;
                txtCPFPai.Enabled = true;
                chkDeclaroAusenciaPai.Visible = false;
            }
            else
            {
                txtNomePai.ReadOnly = true;
                txtTelefonePai.Enabled = false;
                txtCPFPai.Enabled = false;
                chkFalecidoPai.Enabled = false;
                chkDeclaroAusenciaPai.Visible = true;
            }

            if (chkNaoDeclarMae.Checked)
                txtNomeMae.ReadOnly = true;
            else
                txtNomeMae.ReadOnly = false;

            if (chkNaoDeclarPai.Checked)
                txtNomePai.ReadOnly = true;
            else
                txtNomePai.ReadOnly = false;
        }

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tseAluno.Enabled = true;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        tseAluno.Enabled = true;
                        tseNaturalidade.Mode = ControlMode.View;
                        tseNaturalidadeEstrangeira.Mode = ControlMode.View;
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tseAluno.Enabled = false;
                        tseNaturalidade.Enabled = true;
                        tseNaturalidadeEstrangeira.Enabled = true;
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tseNaturalidade.Mode = ControlMode.View;
                        tseNaturalidadeEstrangeira.Mode = ControlMode.View;
                        break;
                    }
            }
        }

        private void Salvar()
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pessoa rnPessoa = new Pessoa();
                RN.RecursosHumanos.DTO.DadosCadastraisAluno dadosAluno = new Techne.Lyceum.RN.RecursosHumanos.DTO.DadosCadastraisAluno();
                RN.Entidades.DeclaracaoAusencia dtDeclaracao = new RN.Entidades.DeclaracaoAusencia();
                var declaracoesAusencia = new List<RN.Entidades.DeclaracaoAusencia>();

                string naturalidade = string.Empty;
                string naturalidadeNome = string.Empty;
                string municipioEstrangeiro = string.Empty;
                string tipo_resp = string.Empty;

                foreach (ListItem item in rblResponsavel.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                    {
                        tipo_resp += item.Value;
                        tipo_resp += ";";
                    }
                }

                bool nascidoForaDoBrasil = tseNaturalidadeEstrangeira.IsValidDBValue
                           && !tseNaturalidadeEstrangeira.DBValue.IsNull;

                if (nascidoForaDoBrasil)
                {
                    naturalidade = tseNaturalidadeEstrangeira.DBValue.ToString();
                    naturalidadeNome = tseNaturalidadeEstrangeira["MUNICIPIO"].ToString();
                    dadosAluno.Pais_nasc = tseNaturalidadeEstrangeira["ID_PAIS"].ToString();
                    dadosAluno.Pais_nasc_nome = tseNaturalidadeEstrangeira["PAIS"].ToString();
                    dadosAluno.UF_nasc = tseNaturalidadeEstrangeira["ESTADO"].ToString();
                }
                else if (!tseNaturalidade.DBValue.IsNull && tseNaturalidade.IsValidDBValue)
                {
                    naturalidade = tseNaturalidade.DBValue.ToString();
                    naturalidadeNome = tseNaturalidade["nome"].ToString();
                    dadosAluno.Pais_nasc = null; // Brasil não tem código de país
                    dadosAluno.Pais_nasc_nome = "BRASIL";
                    dadosAluno.UF_nasc = tseNaturalidade["uf_sigla"].ToString();
                }

                dadosAluno.Pessoa = !txtPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtPessoa.Text) : -1;
                dadosAluno.Nome_compl = !txtNomeCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeCompl.Text.Trim().ToUpper() : null;
                dadosAluno.Dt_nasc = !dtDataNasc.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataNasc.Date : (DateTime?)null;
                dadosAluno.Sexo = !rblSexo.Text.IsNullOrEmptyOrWhiteSpace() ? rblSexo.Text : null;
                dadosAluno.Est_civil = !ddlEst_Civil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEst_Civil.SelectedValue : null;
                dadosAluno.Etnia = !ddlEtnia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEtnia.SelectedValue : null;
                dadosAluno.QtFilhos = !txtFilhos.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtFilhos.Text.Trim()) : (decimal?)null;
                dadosAluno.PreNomeSocial = !txtNomeSocial.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeSocial.Text.Trim().ToUpper() : null;
                dadosAluno.Nacionalidade = !ddlNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlNacionalidade.SelectedValue : null;
                dadosAluno.Endereco = !txtEndereco.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndereco.Text.Trim() : null;
                dadosAluno.End_num = !txtEndNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndNum.Text.Trim() : null;
                dadosAluno.End_compl = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text.Trim() : null;
                dadosAluno.Cep = !txtCep.Text.RetirarCaracteres().IsNullOrEmptyOrWhiteSpace() ? txtCep.Text.RetirarCaracteres() : null;
                dadosAluno.Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text.Trim() : null;
                dadosAluno.End_municipio = !hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCodMunicipio.Value : null;
                dadosAluno.End_NomeMunicipio = !txtMunicipio.Text.IsNullOrEmptyOrWhiteSpace() ? txtMunicipio.Text : null;
                dadosAluno.ZonaResidencial = !ddlLocalZona.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlLocalZona.SelectedValue : null;
                dadosAluno.Fone = txtFone.Text.Trim();
                dadosAluno.Celular = txtCelular.Text.Trim();
                dadosAluno.E_mail = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null;
                dadosAluno.Municipio_nasc = naturalidade;
                dadosAluno.Municipio_nasc_nome = naturalidadeNome;
                dadosAluno.Rg_tipo = !ddlRGTipoPessoa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGTipoPessoa.SelectedValue : null;
                dadosAluno.Rg_num = !txtRGNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGNum.Text.Trim() : null;
                dadosAluno.Rg_uf = !cmbRGUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbRGUF.SelectedValue : null;
                dadosAluno.Rg_emissor = !cmbRGEmissor.Text.IsNullOrEmptyOrWhiteSpace() ? cmbRGEmissor.Text.Trim() : null;
                dadosAluno.Rg_dtexp = !dtDataExped.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataExped.Date : (DateTime?)null;
                dadosAluno.Cpf = !txtCPF.Text.RetirarMascaraCPF().IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.RetirarMascaraCPF().Trim() : null;
                dadosAluno.TipoCertidao = !ddlTipoCertidao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTipoCertidao.SelectedValue : null;
                dadosAluno.CertidaoCivil = !ddlCertidaoCivil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlCertidaoCivil.SelectedValue : null;
                dadosAluno.CertNascNum = !txtDOC_CertNasc_Numero.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_CertNasc_Numero.Text.Trim() : null;
                dadosAluno.CertNascFolha = !txtDOC_CertNasc_Folha.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_CertNasc_Folha.Text.Trim() : null;
                dadosAluno.CertNascLivro = !txtDOC_CertNasc_Livro.Text.IsNullOrEmptyOrWhiteSpace() ? txtDOC_CertNasc_Livro.Text.Trim() : null;
                dadosAluno.IdCartorio = !ddlCartorio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? int.Parse(ddlCartorio.SelectedValue) : (int?)null;
                dadosAluno.NomeCartorio = !ddlCartorio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlCartorio.SelectedItem.Text : null;
                dadosAluno.CertNumeroMatricula = !txtNumMatriculaCertidao.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumMatriculaCertidao.Text.Trim() : null;
                dadosAluno.CertNascEmissao = !dboDOC_CertNasc_DtEmissao.Text.IsNullOrEmptyOrWhiteSpace() ? dboDOC_CertNasc_DtEmissao.Date : (DateTime?)null;
                dadosAluno.CertNascCartorioUf = !ddDOC_CertNasc_Uf.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddDOC_CertNasc_Uf.SelectedValue : null;
                dadosAluno.MunicipioCartorio = !ddlMunicipioCartorio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlMunicipioCartorio.SelectedValue : null;
                dadosAluno.MunicipioCartorioNome = !ddlMunicipioCartorio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlMunicipioCartorio.SelectedItem.Text : null;
                dadosAluno.UfCartorio = !ddlUFCartorio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlUFCartorio.SelectedValue : null;
                dadosAluno.UfCartorioNome = !ddlUFCartorio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlUFCartorio.SelectedItem.Text : null;
                dadosAluno.MaeFalecida = chkFalecidaMae.Checked ? "S" : "N";
                dadosAluno.PaiFalecido = chkFalecidoPai.Checked ? "S" : "N";
                dadosAluno.MaeCpf = !txtCPFMae.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPFMae.Text.Trim() : null;
                dadosAluno.PaiCpf = !txtCPFPai.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPFPai.Text.Trim() : null;
                dadosAluno.MaeTelefone = !txtTelefoneMae.Text.IsNullOrEmptyOrWhiteSpace() ? txtTelefoneMae.Text.Trim() : null;
                dadosAluno.PaiTelefone = !txtTelefonePai.Text.IsNullOrEmptyOrWhiteSpace() ? txtTelefonePai.Text.Trim() : null;
                dadosAluno.Responsavel = tipo_resp;
                dadosAluno.RespNomeCompl = !txtNomeResponsavel.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeResponsavel.Text.Trim().ToUpper() : null;
                dadosAluno.RespCpf = !txtCPFResponsavel.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPFResponsavel.Text.Trim() : null;
                dadosAluno.RespFone = !txtTelefoneResp.Text.IsNullOrEmptyOrWhiteSpace() ? txtTelefoneResp.Text.Trim() : null;
                dadosAluno.NomeMae = !txtNomeMae.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeMae.Text.Trim().ToUpper() : null;
                dadosAluno.NomePai = !txtNomePai.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomePai.Text.Trim().ToUpper() : null;
                dadosAluno.AreaAssentamento = chkAreaAssentamento.Checked ? "S" : "N";
                dadosAluno.TerraIndigena = chkTerraIndigena.Checked ? "S" : "N";
                dadosAluno.AreaQuilombos = chkQuilombos.Checked ? "S" : "N";
                dadosAluno.AreaTradicional = chkAreaTradicional.Checked ? "S" : "N";
                dadosAluno.UsuarioId = User.Identity.Name;
                dadosAluno.DeclaroAusenciaMae = chkDeclaroAusenciaMae.Checked;
                dadosAluno.DeclaroAusenciaPai = chkDeclaroAusenciaPai.Checked;
                dadosAluno.DeclaroCertidaoCivil = chkDeclaroCertidaoCivil.Checked;
                dadosAluno.ComplementoIdentidade = !txtComplIdentidade.Text.IsNullOrEmptyOrWhiteSpace() ? txtComplIdentidade.Text : null;
                dadosAluno.End_UF = !txtEstado.Value.IsNullOrEmptyOrWhiteSpace() ? txtEstado.Value : null;

                string aluno = string.IsNullOrEmpty(Convert.ToString(tseAluno.DBValue)) ? null : tseAluno.DBValue.ToString();

                if (chkNaoDeclarMae.Checked)
                {
                    dtDeclaracao = new RN.Entidades.DeclaracaoAusencia();

                    dtDeclaracao.AlunoId = aluno;
                    dtDeclaracao.Matricula = User.Identity.Name;
                    dtDeclaracao.TipoDeclaracaoAusenciaId = Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoAusenciaMae);
                    declaracoesAusencia.Add(dtDeclaracao);
                }
                if (chkNaoDeclarPai.Checked)
                {
                    dtDeclaracao = new RN.Entidades.DeclaracaoAusencia();

                    dtDeclaracao.AlunoId = aluno;
                    dtDeclaracao.Matricula = User.Identity.Name;
                    dtDeclaracao.TipoDeclaracaoAusenciaId = Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoAusenciaPai);
                    declaracoesAusencia.Add(dtDeclaracao);
                }
                if (ddlTipoCertidao.SelectedValue == "Nenhum")
                {
                    dtDeclaracao = new RN.Entidades.DeclaracaoAusencia();

                    dtDeclaracao.AlunoId = aluno;
                    dtDeclaracao.Matricula = User.Identity.Name;
                    dtDeclaracao.TipoDeclaracaoAusenciaId = Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoCertidaoCivil);
                    dtDeclaracao.Motivo = txtMotivoCertidaoCivil.Text.TrimEnd().ToUpper();
                    declaracoesAusencia.Add(dtDeclaracao);
                }

                validacao = rnPessoa.ValidaAlteracaoDadosCadastrais(dadosAluno, aluno, (chkNaoSeAplica.Checked ? "S" : "N"), declaracoesAusencia);

                if (validacao.Valido)
                {
                    rnPessoa.AlteraDadosCadastrais(dadosAluno, declaracoesAusencia, tseAluno.DBValue.ToString());
                    lblMensagem.Text = "Dados alterados com sucesso.";

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                    ControlarTSearchs();
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

        protected void chkNaoDeclarMae_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNomeMae.ReadOnly = false;
                txtCPFMae.Enabled = true;
                txtTelefoneMae.Enabled = true;
                txtNomeMae.Text = string.Empty;
                chkFalecidaMae.Enabled = true;
                chkDeclaroAusenciaMae.Visible = false;

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), string.Format("CheckedChangedMae('{0}','{1}')", this.chkNaoDeclarMae.ClientID, this.chkDeclaroAusenciaMae.ClientID), true);

                if (chkNaoDeclarMae.Checked)
                {
                    txtNomeMae.Text = chkNaoDeclarMae.Text.ToUpper();
                    txtNomeMae.ReadOnly = true;
                    txtCPFMae.Enabled = false;
                    txtTelefoneMae.Enabled = false;
                    txtCPFMae.Text = string.Empty;
                    txtTelefoneMae.Text = string.Empty;
                    chkFalecidaMae.Checked = false;
                    chkFalecidaMae.Enabled = false;
                    chkDeclaroAusenciaMae.Visible = true;
                    DesabilitaResponsavelLegal("H", "Mãe");
                }
                else
                {
                    DesabilitaResponsavelLegal("D", "Mãe");
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
                txtNomePai.ReadOnly = false;
                txtCPFPai.Enabled = true;
                txtTelefonePai.Enabled = true;
                txtNomePai.Text = string.Empty;
                chkFalecidoPai.Enabled = true;
                chkDeclaroAusenciaPai.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), string.Format("CheckedChangedPai('{0}','{1}')", this.chkNaoDeclarPai.ClientID, this.chkDeclaroAusenciaPai.ClientID), true);

                if (chkNaoDeclarPai.Checked)
                {
                    txtNomePai.Text = chkNaoDeclarPai.Text.ToUpper();
                    txtCPFPai.Text = string.Empty;
                    txtTelefonePai.Text = string.Empty;
                    txtNomePai.ReadOnly = true;
                    txtCPFPai.Enabled = false;
                    txtTelefonePai.Enabled = false;
                    chkFalecidoPai.Checked = false;
                    chkFalecidoPai.Enabled = false;
                    chkDeclaroAusenciaPai.Visible = true;
                    DesabilitaResponsavelLegal("H", "Pai");
                }
                else
                {
                    DesabilitaResponsavelLegal("D", "Pai");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkFalecidaMae_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkFalecidaMae.Checked)
                {
                    DesabilitaResponsavelLegal("H", "Mãe");
                }
                else
                {
                    DesabilitaResponsavelLegal("D", "Mãe");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkFalecidoPai_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkFalecidoPai.Checked)
                    DesabilitaResponsavelLegal("H", "Pai");
                else
                    DesabilitaResponsavelLegal("D", "Pai");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblResponsavel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblNomeResponsavel.Visible = false;
                txtNomeResponsavel.Visible = false;
                lblCPFResponsavel.Visible = false;
                txtCPFResponsavel.Visible = false;
                lblTelefoneResponsavel.Visible = false;
                txtTelefoneResp.Visible = false;
                var verifica = false;

                foreach (ListItem item in rblResponsavel.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Text == "Outros")
                        {
                            verifica = true;
                            lblNomeResponsavel.Visible = true;
                            txtNomeResponsavel.Visible = true;
                            lblCPFResponsavel.Visible = true;
                            txtCPFResponsavel.Visible = true;
                            lblTelefoneResponsavel.Visible = true;
                            txtTelefoneResp.Visible = true;
                        }
                        if (item.Text == "Próprio Aluno")
                        {
                            if (dtDataNasc.Text.Trim() != string.Empty)
                            {
                                var idade = Utils.CalcularIdade(dtDataNasc.Date);

                                if (idade < 18)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup",
                                                                            "alert('Para o aluno ser responsável é necessário ter mais que 18 anos.');",
                                                                            true);
                                    item.Selected = false;
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup",
                                                                        "alert('Favor preencher o campo Data de Nascimento.');",
                                                                        true);
                                item.Selected = false;
                            }
                        }
                    }
                }

                if (!verifica)
                {
                    txtNomeResponsavel.Text = string.Empty;
                    txtCPFResponsavel.Text = string.Empty;
                    txtTelefoneResp.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void cmbPaises_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LimparEndereco();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlNacionalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtComplIdentidade.ReadOnly = true;
                txtComplIdentidade.Text = string.Empty;

                if (ddlNacionalidade.SelectedItem.Text == "BRASILEIRA")
                {

                    if (!string.IsNullOrEmpty(ddlRGTipoPessoa.SelectedValue))
                    {
                        if (ddlRGTipoPessoa.SelectedValue == "RG")
                        {
                            txtComplIdentidade.ReadOnly = false;
                        }
                    }
                }

                tseNaturalidade.ResetValue();
                tseNaturalidadeEstrangeira.ResetValue();
                txtUFNascimento.Text = string.Empty;
                txtPaisNasc.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlRGTipoPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtComplIdentidade.ReadOnly = true;
                txtComplIdentidade.Text = string.Empty;

                if (ddlRGTipoPessoa.SelectedValue == "RG")
                {
                    lblRG_Num.Text = "Número*: ";
                    lblRG_Num.Font.Bold = true;

                    lblRG_UF.Text = "Estado*: ";
                    lblRG_UF.Font.Bold = true;

                    lblRG_Emissor.Text = "Órgão Emissor*: ";
                    lblRG_Emissor.Font.Bold = true;

                    lblRG_Data.Text = "Data de Expedição*: ";
                    lblRG_Data.Font.Bold = true;

                    if (ddlNacionalidade.SelectedItem.Text == "BRASILEIRA")
                    {
                        txtComplIdentidade.ReadOnly = false;
                    }

                }
                else if (ddlRGTipoPessoa.SelectedValue == string.Empty)
                {
                    lblRG_Num.Text = "Número: ";
                    lblRG_Num.Font.Bold = false;

                    lblRG_UF.Text = "Estado: ";
                    lblRG_UF.Font.Bold = false;

                    lblRG_Emissor.Text = "Órgão Emissor: ";
                    lblRG_Emissor.Font.Bold = false;

                    lblRG_Data.Text = "Data de Expedição: ";
                    lblRG_Data.Font.Bold = false;
                }
                else if (ddlRGTipoPessoa.SelectedValue == "PASSAPORTE")
                {
                    lblRG_Num.Text = "Número: ";
                    lblRG_Num.Font.Bold = false;

                    lblRG_UF.Text = "Estado: ";
                    lblRG_UF.Font.Bold = false;

                    lblRG_Emissor.Text = "Órgão Emissor: ";
                    lblRG_Emissor.Font.Bold = false;
                    cmbRGEmissor.Enabled = true;

                    lblRG_Data.Text = "Data de Expedição: ";
                    lblRG_Data.Font.Bold = false;

                    CarregaRGEmissor();
                    cmbRGEmissor.SelectedValue = string.Empty;
                }
                else
                {
                    lblRG_Num.Text = "Número*: ";
                    lblRG_Num.Font.Bold = true;

                    lblRG_UF.Text = "Estado: ";
                    lblRG_UF.Font.Bold = false;

                    lblRG_Emissor.Text = "Órgão Emissor*: ";
                    lblRG_Emissor.Font.Bold = true;

                    lblRG_Data.Text = "Data de Expedição: ";
                    lblRG_Data.Font.Bold = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlCertidaoCivil_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtNumMatriculaCertidao.Text = string.Empty;
                txtDOC_CertNasc_Folha.Text = string.Empty;
                txtDOC_CertNasc_Livro.Text = string.Empty;
                txtDOC_CertNasc_Numero.Text = string.Empty;
                ddlUFCartorio.ClearSelection();
                ddlMunicipioCartorio.Items.Clear();
                ddlCartorio.Items.Clear();
                dboDOC_CertNasc_DtEmissao.Text = string.Empty;
                ddDOC_CertNasc_Uf.ClearSelection();

                if (ddlCertidaoCivil.SelectedValue == "Modelo Novo")
                {
                    pnlNovo.Visible = true;
                    pnlAntigo.Visible = false;
                }
                else if (ddlCertidaoCivil.SelectedValue == "Modelo Antigo")
                {
                    pnlNovo.Visible = false;
                    pnlAntigo.Visible = true;
                }
                else
                {
                    pnlNovo.Visible = false;
                    pnlAntigo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlMunicipioCartorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlCartorio.Items.Clear();

                if (!string.IsNullOrEmpty(ddlMunicipioCartorio.SelectedValue))
                {
                    CarregaCartorio();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlUFCartorio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlMunicipioCartorio.Items.Clear();
                ddlCartorio.Items.Clear();

                if (!string.IsNullOrEmpty(ddlUFCartorio.SelectedValue))
                {
                    this.CarregaMunicipioCartorio();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTipoCertidao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtNumMatriculaCertidao.Text = string.Empty;
                txtDOC_CertNasc_Folha.Text = string.Empty;
                txtDOC_CertNasc_Livro.Text = string.Empty;
                txtDOC_CertNasc_Numero.Text = string.Empty;
                ddlCertidaoCivil.ClearSelection();
                ddlUFCartorio.ClearSelection();
                ddlMunicipioCartorio.Items.Clear();
                ddlCartorio.Items.Clear();
                dboDOC_CertNasc_DtEmissao.Text = string.Empty;
                ddDOC_CertNasc_Uf.ClearSelection();
                pnlTipoCertidaoCivil.Visible = false;
                chkDeclaroCertidaoCivil.Checked = false;
                txtMotivoCertidaoCivil.Text = string.Empty;
                lblCertCivil.Visible = true;
                ddlCertidaoCivil.Visible = true;

                if (!string.IsNullOrEmpty(ddlTipoCertidao.SelectedValue))
                {
                    if (ddlTipoCertidao.SelectedValue == "Nenhum")
                    {
                        pnlTipoCertidaoCivil.Visible = true;
                        chkDeclaroCertidaoCivil.Checked = true;
                        chkDeclaroCertidaoCivil.Visible = true;
                        txtMotivoCertidaoCivil.Visible = true;
                        pnlAntigo.Visible = false;
                        pnlNovo.Visible = false;
                        lblCertCivil.Visible = false;
                        ddlCertidaoCivil.Visible = false;
                    }
                    else
                    {
                        ddlCertidaoCivil.Enabled = true;
                        pnlAntigo.Visible = false;
                        pnlNovo.Visible = false;
                    }
                }
                else
                {
                    ddlCertidaoCivil.Enabled = false;
                    pnlAntigo.Visible = false;
                    pnlNovo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparEndereco()
        {
            txtMunicipio.Text = string.Empty;
            hdnCodMunicipio.Value = string.Empty;
            txtEstado.Value = string.Empty;
            txtEndereco.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            chkAreaAssentamento.Checked = false;
            chkTerraIndigena.Checked = false;
            chkQuilombos.Checked = false;
            chkAreaTradicional.Checked = false;
            chkNaoSeAplica.Checked = false;
        }

        private void ValidaLocalizacaoDiferenciada()
        {
            if (chkNaoSeAplica.Checked)
            {
                chkQuilombos.Checked = !chkNaoSeAplica.Checked;
                chkAreaTradicional.Checked = !chkNaoSeAplica.Checked;
                chkAreaAssentamento.Checked = !chkNaoSeAplica.Checked;
                chkTerraIndigena.Checked = !chkNaoSeAplica.Checked;

                chkQuilombos.Enabled = !chkNaoSeAplica.Checked;
                chkAreaTradicional.Enabled = !chkNaoSeAplica.Checked;
                chkAreaAssentamento.Enabled = !chkNaoSeAplica.Checked;
                chkTerraIndigena.Enabled = !chkNaoSeAplica.Checked;
            }
            else
            {
                HabilitaLocalizacaoDiferenciada();
            }
        }

        private void HabilitaLocalizacaoDiferenciada()
        {
            if (!chkNaoSeAplica.Checked)
            {
                Util.Utils.HabilitaDesabilitaControlesWeb(
                    new WebControl[] {
                        chkQuilombos, chkTerraIndigena, chkAreaAssentamento, chkAreaTradicional
                    }, true
                );
            }
            chkNaoSeAplica.Enabled = true;
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;

                        lblMensagem.Text = string.Empty;

                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;

                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseAluno_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseNaturalidade_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseNaturalidade_Changed(object sender, EventArgs args)
        {
            try
            {
                if (tseNaturalidade.IsValidDBValue && !tseNaturalidade.DBValue.IsNull)
                    txtUFNascimento.Text = tseNaturalidade["uf_sigla"].ToString();
            }
            catch (Exception ex) { lblMensagem.Text = ex.Message; }
        }

        protected void tseNaturalidadeEstrangeira_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseNaturalidadeEstrangeira_Changed(object sender, EventArgs args)
        {
            try
            {
                if (tseNaturalidadeEstrangeira.IsValidDBValue && !tseNaturalidadeEstrangeira.DBValue.IsNull)
                {
                    txtUFNascimento.Text = tseNaturalidadeEstrangeira["ESTADO"].ToString();
                    txtPaisNasc.Text = tseNaturalidadeEstrangeira["PAIS"].ToString();
                }
            }
            catch (Exception ex) { lblMensagem.Text = ex.Message; }
        }

        protected void chkNaoSeAplica_CheckedChanged(object sender, EventArgs e)
        {
            ValidaLocalizacaoDiferenciada();
        }

        protected void hplLinkNomeSocial_Click(object sender, EventArgs e)
        {
            try
            {
                string FilePath = HttpContext.Current.Server.MapPath(@"~\Arquivos\SaibaMaisNomeSocial.pdf");
                WebClient client = new WebClient();
                Byte[] buffer = client.DownloadData(FilePath);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    }
}
