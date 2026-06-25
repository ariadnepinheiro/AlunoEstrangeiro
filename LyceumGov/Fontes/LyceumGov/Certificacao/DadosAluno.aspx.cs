using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Certificacao
{
    [NavUrl("~/Certificacao/DadosAluno.aspx"), ControlText("Atualização Dados Alunos"), Title("Atualização Dados Alunos")]
    public partial class DadosAluno : TPage
    {
        public enum TipoOperacao { Inicial, Alterar, Sucesso, Consultar }

        private TipoOperacao _tipoOperacao
        {
            get
            {
                if (ViewState["_tipoOperacao"] != null && ViewState["_tipoOperacao"] is TipoOperacao)
                    return (TipoOperacao)ViewState["_tipoOperacao"];
                return TipoOperacao.Inicial;
            }
            set { ViewState["_tipoOperacao"] = value; }
        }

        // ─────────────────────────────────────────────────────────
        // CICLO DE VIDA
        // ─────────────────────────────────────────────────────────

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    this.ControlarTipoOperacao();
                }
            }
            catch (Exception ex) { lblMensagem.Text = ex.Message; }
        }

        /// <summary>
        /// Controla a visibilidade das duas tse de acordo com a operação
        /// e com a resposta de "Nascido fora do Brasil?".
        ///
        /// Alterar / Inicial  → ambas ocultas; o rbl decide qual mostrar
        /// Consultar / Sucesso → exibe apenas a que foi preenchida na carga
        /// </summary>

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.novo);

            lblPaisNascimento.Visible = true;
            txtPaisNascimento.Visible = true;
            lblUFNascimento.Visible = true;
            txtUFNascimento.Visible = true;

            bool nascidoFora = !string.IsNullOrEmpty(txtPaisNascimento.Text)
                               && txtPaisNascimento.Text.Trim().ToUpper() != "BRASIL";

            bool modoEdicao = (_tipoOperacao == TipoOperacao.Alterar);

            if (modoEdicao)
            {
                // Em edição, mostra TSE correto
                tseNaturalidade.Visible = false;
                tseNaturalidadeEstrangeira.Visible = false;

                if (!string.IsNullOrEmpty(cmbNacionalidade.SelectedValue))
                {
                    tseNaturalidade.Visible = !nascidoFora;
                    tseNaturalidadeEstrangeira.Visible = nascidoFora;
                }
            }
            else
            {
                // Em consulta, mostra baseado no país
                tseNaturalidade.Visible = !nascidoFora;
                tseNaturalidadeEstrangeira.Visible = nascidoFora;
            }
        }

        // ─────────────────────────────────────────────────────────
        // HABILITAR / DESABILITAR
        // ─────────────────────────────────────────────────────────

        protected void DesabilitaCampos()
        {
            tseNaturalidade.Mode = ControlMode.View;
            tseNaturalidadeEstrangeira.Mode = ControlMode.View;
            txtNomeAluno.ReadOnly = true;
            txtNRg.ReadOnly = true;
            txtNomemae.ReadOnly = true;
            txtNomepai.ReadOnly = true;
            txtUFNascimento.Enabled = true;
            txtPaisNascimento.Enabled = true;
            dtDataNasc.Enabled = false;
            dtExpedicaoRg.Enabled = false;
            cmbRGUF.Enabled = false;
            cmbRGEmissor.Enabled = false;
            cmbNacionalidade.Enabled = false;
        }

        protected void HabilitaCampos()
        {
            tseNaturalidade.Mode = ControlMode.Edit;
            tseNaturalidadeEstrangeira.Mode = ControlMode.Edit;
            txtNomeAluno.ReadOnly = false;
            txtNRg.ReadOnly = false;
            txtNomemae.ReadOnly = false;
            txtNomepai.ReadOnly = false;
            txtUFNascimento.Enabled = true;
            txtPaisNascimento.Enabled = true;
            dtDataNasc.Enabled = true;
            dtExpedicaoRg.Enabled = true;
            cmbRGUF.Enabled = true;
            cmbRGEmissor.Enabled = true;
            cmbNacionalidade.Enabled = true;
        }

        private void LimparTela()
        {
            //tseNaturalidade.ResetValue();
            //tseNaturalidadeEstrangeira.ResetValue();
            tseNaturalidadeEstrangeira.Enabled = false;
            txtPaisNascimento.Text = string.Empty;
            txtNomemae.Text = string.Empty;
            txtNomepai.Text = string.Empty;
            txtNomeAluno.Text = string.Empty;
            txtNRg.Text = string.Empty;
            txtUFNascimento.Text = string.Empty;
            txtPaisNascimento.Text = string.Empty;
            cmbRGUF.ClearSelection();
            cmbRGEmissor.ClearSelection();
            cmbNacionalidade.ClearSelection();
            dtExpedicaoRg.Text = string.Empty;
            dtDataNasc.Text = string.Empty;
        }

        // ─────────────────────────────────────────────────────────
        // HANDLERS DOS CONTROLES TSE / RADIO / COMBO
        // ─────────────────────────────────────────────────────────

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback) return;

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
                        lblMensagem.Text = "Aluno ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex) { lblMensagem.Text = ex.Message; }
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

        protected void tseNaturalidadeEstrangeira_Changed(object sender, EventArgs args)
        {
            try
            {
                if (tseNaturalidadeEstrangeira.IsValidDBValue && !tseNaturalidadeEstrangeira.DBValue.IsNull)
                {
                    txtUFNascimento.Text = tseNaturalidadeEstrangeira["ESTADO"].ToString();
                    txtPaisNascimento.Text = tseNaturalidadeEstrangeira["PAIS"].ToString();
                }
            }
            catch (Exception ex) { lblMensagem.Text = ex.Message; }
        }

        protected void rblNascidoEstrangeiro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseNaturalidade.ResetValue();
                tseNaturalidadeEstrangeira.ResetValue();
                txtUFNascimento.Text = string.Empty;
                txtPaisNascimento.Text = string.Empty;
                tseNaturalidadeEstrangeira.Enabled = false;
            }
            catch (Exception ex) { lblMensagem.Text = ex.Message; }
        }

        protected void cmbNacionalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtUFNascimento.Text = string.Empty;
                txtPaisNascimento.Text = string.Empty;

                if (cmbNacionalidade.SelectedItem.Text == "ESTRANGEIRA")
                    tseNaturalidadeEstrangeira.Enabled = true;

                if (cmbNacionalidade.SelectedItem.Text == "BRASILEIRA")
                {
                    tseNaturalidade.Enabled = true;
                    tseNaturalidadeEstrangeira.Enabled = true;
                }
            }
            catch (Exception ex) { lblMensagem.Text = ex.Message; }
        }

        // ─────────────────────────────────────────────────────────
        // BOTÕES
        // ─────────────────────────────────────────────────────────

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
            }
            catch (Exception ex) { lblMensagem.Text = ex.Message; }
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue)
                {
                    _tipoOperacao = TipoOperacao.Alterar;
                    ControlarTipoOperacao();
                }
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    lblMensagem.Text = "Aluno ativo ou não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex) { lblMensagem.Text = ex.Message; }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.DTOs.DadosAlunoCertificacao dadosAluno = new Techne.Lyceum.RN.DTOs.DadosAlunoCertificacao();
                RN.Pessoa rnPessoa = new Pessoa();
                ValidacaoDados validacao = new ValidacaoDados();
                string mensagem;

                dadosAluno.Pessoa = (!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue) ? Convert.ToDecimal(tseAluno["pessoa"]) : 0;
                dadosAluno.Nome = !txtNomeAluno.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeAluno.Text.Trim() : null;
                dadosAluno.DataNascimento = !dtDataNasc.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataNasc.Date : DateTime.MinValue;
                dadosAluno.NomeMae = !txtNomemae.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomemae.Text.Trim() : null;
                dadosAluno.NomePai = !txtNomepai.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomepai.Text.Trim() : null;
                dadosAluno.RgNumero = !txtNRg.Text.IsNullOrEmptyOrWhiteSpace() ? txtNRg.Text.Trim() : null;
                dadosAluno.RgEmissor = !cmbRGEmissor.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbRGEmissor.SelectedValue : null;
                dadosAluno.RgUf = !cmbRGUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbRGUF.SelectedValue : null;
                dadosAluno.Nacionalidade = !cmbNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbNacionalidade.SelectedValue : null;
                dadosAluno.RgDataExpedicao = !dtExpedicaoRg.Text.IsNullOrEmptyOrWhiteSpace() ? dtExpedicaoRg.Date : DateTime.MinValue;
                dadosAluno.UsuarioResponsavel = User.Identity.Name;

                // ── Captura do município e país de nascimento ──────────────────
                bool nascidoForaDoBrasil = tseNaturalidadeEstrangeira.IsValidDBValue
                                           && !tseNaturalidadeEstrangeira.DBValue.IsNull;

                if (nascidoForaDoBrasil)
                {
                    dadosAluno.MunicipioNascimento = tseNaturalidadeEstrangeira.DBValue.ToString();
                    dadosAluno.PaisNascimento = tseNaturalidadeEstrangeira["ID_PAIS"].ToString();
                }
                else
                {
                    dadosAluno.MunicipioNascimento = (!tseNaturalidade.DBValue.IsNull && tseNaturalidade.IsValidDBValue)
                                                     ? tseNaturalidade.DBValue.ToString()
                                                     : null;
                    dadosAluno.PaisNascimento = null;
                }
                // ────────────────────────────────────────────────────────────────

                validacao = rnPessoa.ValidaDadosCertificacao(dadosAluno);

                if (validacao.Valido)
                {
                    rnPessoa.AtualizaDadosCertificacao(dadosAluno);
                    mensagem = "Dados de Aluno atualizado com sucesso.";
                    lblMensagem.Text = mensagem;

                    this._tipoOperacao = TipoOperacao.Sucesso;
                    this.ControlarTipoOperacao();

                    this.Page.ClientScript.RegisterStartupScript(
                        this.Page.GetType(), "popup",
                        @"alert('" + mensagem + @"');", true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "");
                }
            }
            catch (Exception ex) { lblMensagem.Text = ex.Message; }
        }

        // ─────────────────────────────────────────────────────────
        // CONTROLE DE OPERAÇÃO
        // ─────────────────────────────────────────────────────────

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();
            foreach (var botao in botoes) botao.Visible = true;
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
                        ControlarVisibilidadeControle(new ImageButton[] { });
                        CarregaRGEmissor();
                        CarregaUfRg();
                        CarregaNacionalidade();
                        tseAluno.ResetValue();
                        LimparTela();
                        pnlDados.Visible = false;
                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        if (!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue)
                        {
                            ControlarVisibilidadeControle(new ImageButton[] { btnCancel, btnSalvar });
                            lblMensagem.Text = string.Empty;
                            HabilitaCampos();

                            // Só recarrega do banco se o TSE não tiver valor (primeira entrada no modo Editar)
                            bool temEstrangeiro = tseNaturalidadeEstrangeira.IsValidDBValue && !tseNaturalidadeEstrangeira.DBValue.IsNull;
                            bool temBrasileiro = tseNaturalidade.IsValidDBValue && !tseNaturalidade.DBValue.IsNull;

                            if (!temEstrangeiro && !temBrasileiro)
                            {
                                RN.Pessoa rnPessoa = new Pessoa();
                                RN.DTOs.DadosAlunoCertificacao dadosAluno = rnPessoa.ObtemDadosCertificacaoPor(Convert.ToDecimal(tseAluno["pessoa"]));

                                bool nascidoForaDoBrasil = !string.IsNullOrEmpty(dadosAluno.PaisNascimento)
                                                           && dadosAluno.PaisNascimento.Trim() != "0";

                                if (nascidoForaDoBrasil)
                                {
                                    tseNaturalidade.ResetValue();
                                    tseNaturalidadeEstrangeira.Enabled = true;
                                    txtPaisNascimento.Text = RN.Endereco.ObterPaisEstrangeiro(dadosAluno.PaisNascimento);

                                    if (!string.IsNullOrEmpty(dadosAluno.MunicipioNascimento) && dadosAluno.MunicipioNascimento.Trim() != "0")
                                    {
                                        tseNaturalidadeEstrangeira.DBValue = dadosAluno.MunicipioNascimento;
                                        if (tseNaturalidadeEstrangeira.IsValidDBValue && !tseNaturalidadeEstrangeira.DBValue.IsNull)
                                        {
                                            txtUFNascimento.Text = tseNaturalidadeEstrangeira["ESTADO"].ToString();
                                            txtPaisNascimento.Text = tseNaturalidadeEstrangeira["PAIS"].ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    tseNaturalidadeEstrangeira.ResetValue();
                                    txtPaisNascimento.Text = string.Empty;
                                    tseNaturalidade.Enabled = true;

                                    if (!string.IsNullOrEmpty(dadosAluno.MunicipioNascimento) && dadosAluno.MunicipioNascimento.Trim() != "0")
                                    {
                                        tseNaturalidade.DBValue = dadosAluno.MunicipioNascimento;
                                        if (tseNaturalidade.IsValidDBValue && !tseNaturalidade.DBValue.IsNull)
                                            txtUFNascimento.Text = tseNaturalidade["uf_sigla"].ToString();
                                    }
                                }
                            }
                        }
                        else
                        {
                            this._tipoOperacao = TipoOperacao.Inicial;
                            lblMensagem.Text = "Aluno ativo ou não cadastrado (favor verificar).";
                        }
                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        RN.Pessoa rnPessoa = new Pessoa();
                        RN.DTOs.DadosAlunoCertificacao dadosAluno = new Techne.Lyceum.RN.DTOs.DadosAlunoCertificacao();

                        lblMensagem.Text = string.Empty;
                        LimparTela();

                        dadosAluno = rnPessoa.ObtemDadosCertificacaoPor(Convert.ToDecimal(tseAluno["pessoa"]));

                        if (dadosAluno.Nome.IsNullOrEmptyOrWhiteSpace())
                        {
                            lblMensagem.Text = "Aluno não encontrado.";
                            ControlarVisibilidadeControle(new ImageButton[] { });
                        }
                        else
                        {
                            ControlarVisibilidadeControle(new ImageButton[] { btnEditar, btnCancel });

                            tseAluno.Enabled = true;
                            txtNomeAluno.Text = !dadosAluno.Nome.IsNullOrEmptyOrWhiteSpace() ? dadosAluno.Nome.Trim() : string.Empty;
                            txtNomemae.Text = !dadosAluno.NomeMae.IsNullOrEmptyOrWhiteSpace() ? dadosAluno.NomeMae.Trim() : string.Empty;
                            txtNomepai.Text = !dadosAluno.NomePai.IsNullOrEmptyOrWhiteSpace() ? dadosAluno.NomePai.Trim() : string.Empty;
                            txtNRg.Text = !dadosAluno.RgNumero.IsNullOrEmptyOrWhiteSpace() ? dadosAluno.RgNumero.Trim() : string.Empty;

                            cmbRGEmissor.SelectedValue = !dadosAluno.RgEmissor.IsNullOrEmptyOrWhiteSpace() ? dadosAluno.RgEmissor.Trim() : string.Empty;
                            cmbRGUF.SelectedValue = !dadosAluno.RgUf.IsNullOrEmptyOrWhiteSpace() ? dadosAluno.RgUf : string.Empty;
                            cmbNacionalidade.SelectedValue = !dadosAluno.Nacionalidade.IsNullOrEmptyOrWhiteSpace() ? dadosAluno.Nacionalidade : string.Empty;

                            if (dadosAluno.DataNascimento != null) dtDataNasc.Date = dadosAluno.DataNascimento.Date;
                            if (dadosAluno.RgDataExpedicao != null) dtExpedicaoRg.Date = dadosAluno.RgDataExpedicao.Date;

                            // ── Carga da naturalidade ───────────────────────────

                            tseNaturalidadeEstrangeira.Enabled = true;

                            bool nascidoForaDoBrasil = !string.IsNullOrEmpty(dadosAluno.PaisNascimento)
                                                       && dadosAluno.PaisNascimento.Trim() != "0";

                            if (nascidoForaDoBrasil)
                            {
                                // País estrangeiro — txtPaisNasc já preenchido por CarregaDadosPessoa
                                tseNaturalidade.ResetValue();
                                tseNaturalidade.Visible = false;
                                tseNaturalidadeEstrangeira.Visible = true;

                                txtPaisNascimento.Text = RN.Endereco.ObterPaisEstrangeiro(dadosAluno.PaisNascimento);

                                if (!string.IsNullOrEmpty(dadosAluno.MunicipioNascimento)
                                    && dadosAluno.MunicipioNascimento.Trim() != "0")
                                {
                                    tseNaturalidadeEstrangeira.DBValue = dadosAluno.MunicipioNascimento;

                                    if (tseNaturalidadeEstrangeira.IsValidDBValue && !tseNaturalidadeEstrangeira.DBValue.IsNull)
                                    {
                                        txtUFNascimento.Text = tseNaturalidadeEstrangeira["ESTADO"].ToString();
                                        txtPaisNascimento.Text = tseNaturalidadeEstrangeira["PAIS"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                // Nascido no Brasil
                                tseNaturalidadeEstrangeira.ResetValue();
                                tseNaturalidadeEstrangeira.Visible = false;
                                tseNaturalidade.Visible = true;
                                //txtPaisNascimento.Text = string.Empty;

                                if (!string.IsNullOrEmpty(dadosAluno.MunicipioNascimento)
                                    && dadosAluno.MunicipioNascimento.Trim() != "0")
                                {
                                    tseNaturalidade.DBValue = dadosAluno.MunicipioNascimento;

                                    if (tseNaturalidade.IsValidDBValue && !tseNaturalidade.DBValue.IsNull)
                                        txtUFNascimento.Text = tseNaturalidade["uf_sigla"].ToString();
                                    else
                                        tseNaturalidade.ResetValue();
                                }
                            }

                            pnlDados.Visible = true;
                            DesabilitaCampos();
                        }
                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        ControlarVisibilidadeControle(new ImageButton[] { btnEditar });
                        DesabilitaCampos();
                        break;
                    }
            }
        }

        // ─────────────────────────────────────────────────────────
        // CARREGAMENTO DE LISTAS
        // ─────────────────────────────────────────────────────────

        private void CarregaRGEmissor()
        {
            cmbRGEmissor.Items.Clear();
            cmbRGEmissor.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.RgEmissor);
            cmbRGEmissor.DataBind();
            cmbRGEmissor.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregaUfRg()
        {
            cmbRGUF.Items.Clear();
            cmbRGUF.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Uf, RN.Basico.QueryListaUF);
            cmbRGUF.DataBind();
            cmbRGUF.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregaNacionalidade()
        {
            cmbNacionalidade.Items.Clear();
            cmbNacionalidade.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Nacionalidade, RN.Basico.QueryListaNacionalidades);
            cmbNacionalidade.DataBind();
            cmbNacionalidade.Items.Insert(0, new ListItem("Selecione", string.Empty));

            ListItem item = cmbNacionalidade.Items.FindByText("BRASILEIRA");
            if (item != null) { cmbNacionalidade.ClearSelection(); item.Selected = true; }
        }
    }
}

