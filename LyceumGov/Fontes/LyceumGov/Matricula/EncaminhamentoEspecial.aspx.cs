using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.Net.Basico;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using Techne.Controls;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/EncaminhamentoEspecial.aspx"), ControlText("Encaminhamento Especial"), Title("Encaminhamento Especial")]
    public partial class EncaminhamentoEspecial : TPage
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

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnFinalizar, AcaoControle.novo);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void HabilitaPainel(bool visivel)
        {
            pnDados.Visible = visivel;
            pnlCEP.Visible = visivel;
            pnlFiliacao.Visible = visivel;
            pnlOpcao.Visible = visivel;
        }

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        dvGeral.Visible = false;

                        LimparDadosBusca();
                        LimparDadosCadastro();
                        LimpaDadosCEP();
                        LimparDadosOpcao();

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        RN.DTOs.DadosEncaminhamentoEspecial dados = new Techne.Lyceum.RN.DTOs.DadosEncaminhamentoEspecial();
                        RN.Aluno rnAluno = new Aluno();

                        LimparDadosCadastro();
                        LimparDadosOpcao();
                        dvGeral.Visible = true;
                        CarregaAnoMatricula();
                        CarregaPeriodoMatricula();
                        CarregaNivel();
                        CarregaModalidade();
                        CarregaNecessidadeEspecial();
                        CarregaMotivoEncaminhamento();

                        dados = rnAluno.ObtemDadosEncaminhamentoEspecialPor(txtNomeBusca.Text, txtNomeMae.Text, Convert.ToDateTime(dtDataNasc.Text));

                        hdnPessoa.Value = string.Empty;
                        hdnPreCadastroAlunoId.Value = string.Empty;
                        txtNomeCadastro.Text = txtNomeBusca.Text;
                        txtNomeCadastro.Enabled = false;
                        txtNomeMaeCadastro.Text = txtNomeMae.Text;
                        txtNomeMaeCadastro.Enabled = false;
                        DtNascimentoCadastro.Text = dtDataNasc.Text;
                        DtNascimentoCadastro.Enabled = false;
                        chkNaoDeclarMaeCadastro.Checked = chkNaoDeclarMae.Checked;

                        if (!dados.Nome.IsNullOrEmptyOrWhiteSpace())
                        {
                            hdnPessoa.Value = dados.Pessoa == null ? string.Empty : Convert.ToString(dados.Pessoa);
                            hdnPreCadastroAlunoId.Value = dados.PreCadastroAlunoId == null ? string.Empty : Convert.ToString(dados.PreCadastroAlunoId);

                            if (!dados.Sexo.IsNullOrEmptyOrWhiteSpace())
                            {
                                if (ddlSexo.Items.FindByValue(dados.Sexo) != null)
                                {
                                    ddlSexo.SelectedValue = ddlSexo.Items.FindByValue(dados.Sexo).Value;
                                }
                            }

                            txtNomePai.Text = !dados.NomePai.IsNullOrEmptyOrWhiteSpace() ? dados.NomePai : string.Empty;
                            chkNaoDeclarPai.Checked = dados.PaiNãoDeclarado;
                            if (chkNaoDeclarPai.Checked)
                            {
                                chkNaoDeclarPai_CheckedChanged(null, null);
                            }

                            txtLogradouro.Text = !dados.Endereco.IsNullOrEmptyOrWhiteSpace() ? dados.Endereco : string.Empty;
                            txtNumero.Text = !dados.NumeroEndereco.IsNullOrEmptyOrWhiteSpace() ? dados.NumeroEndereco : string.Empty;
                            txtComplemento.Text = !dados.ComplementoEndereco.IsNullOrEmptyOrWhiteSpace() ? dados.ComplementoEndereco : string.Empty;
                            txtCep.Text = !dados.Cep.IsNullOrEmptyOrWhiteSpace() ? dados.Cep : string.Empty;
                            txtEstado.Value = !dados.UfEndereco.IsNullOrEmptyOrWhiteSpace() ? dados.UfEndereco : string.Empty;
                            txtMunicipio.Value = !dados.DescricaoMunicipioEndereco.IsNullOrEmptyOrWhiteSpace() ? dados.DescricaoMunicipioEndereco : string.Empty;
                            hdnCodMunicipio.Value = !dados.MunicipioEndereco.IsNullOrEmptyOrWhiteSpace() ? dados.MunicipioEndereco : string.Empty;

                            if (!hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace())
                            {
                                CarregaBairro(hdnCodMunicipio.Value);
                                if (!dados.Bairro.IsNullOrEmptyOrWhiteSpace())
                                {
                                    if (ddlBairro.Items.FindByText(dados.Bairro.ToUpper()) != null)
                                    {
                                        ddlBairro.SelectedValue = ddlBairro.Items.FindByText(dados.Bairro.ToUpper()).Value;
                                    }
                                }
                            }

                            Int64 resultadoCPF;
                            if (Int64.TryParse(dados.Cpf, out resultadoCPF))
                            {
                                if (resultadoCPF != 0)
                                {
                                    txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", resultadoCPF);
                                }
                                else
                                {
                                    txtCPF.Text = resultadoCPF.ToString();
                                }
                            }
                            if (dados.NecessidadeEspecialId > 0)
                            {
                                if (ddlDeficiencia.Items.FindByValue(Convert.ToString(dados.NecessidadeEspecialId)) != null)
                                {
                                    ddlDeficiencia.SelectedValue = ddlDeficiencia.Items.FindByValue(Convert.ToString(dados.NecessidadeEspecialId)).Value;
                                }
                            }
                        }

                        break;
                    }
            }
        }

        private void LimparDadosBusca()
        {
            txtNomeBusca.Text = string.Empty;
            txtNomeMae.Text = string.Empty;
            dtDataNasc.Text = string.Empty;
            chkNaoDeclarMae.Checked = false;
        }

        private void LimparDadosCadastro()
        {
            hdnPessoa.Value = string.Empty;
            hdnPreCadastroAlunoId.Value = string.Empty;
            txtNomeCadastro.Text = string.Empty;
            txtCPF.Text = string.Empty;
            DtNascimentoCadastro.Text = string.Empty;
            ddlSexo.ClearSelection();
            txtNomeMaeCadastro.Text = string.Empty;
            chkNaoDeclarMaeCadastro.Checked = false;
            txtNomePai.Text = string.Empty;
            chkNaoDeclarPai.Checked = false;
            ddlDeficiencia.ClearSelection();
            ddlMotivo.ClearSelection();
            txtObservacao.Text = string.Empty;
            LimpaDadosCEP();
        }

        private void LimpaDadosCEP()
        {
            txtCep.Text = string.Empty;
            txtLogradouro.Text = string.Empty;
            txtMunicipio.Value = string.Empty;
            hdnCodMunicipio.Value = string.Empty;
            txtEstado.Value = string.Empty;
            ddlBairro.Items.Clear();
            ddlBairro.Enabled = false;
            txtNumero.Text = string.Empty;
            txtComplemento.Text = string.Empty;
        }

        private void LimparDadosOpcao()
        {
            ddlAnoMatricula.ClearSelection();
            ddlPeriodoMatricula.ClearSelection();
            tseUnidadeEnsinoMatricula.ResetValue();
            ddlCursoMatricula.Items.Clear();
            ddlModalidadeMatricula.ClearSelection();
            ddlNivelMatricula.ClearSelection();
            ddlSerieMatricula.Items.Clear();
            ddlTurnoMatricula.Items.Clear();
        }

        protected void CarregaBairro(string municipioId)
        {
            RN.Bairro rnBairro = new Techne.Lyceum.RN.Bairro();
            ddlBairro.Items.Clear();
            ddlBairro.DataSource = rnBairro.ListaPor(municipioId);
            ddlBairro.DataBind();
            ddlBairro.Items.Insert(0, new ListItem("Selecione", string.Empty));
            ddlBairro.Enabled = true;
        }

        private void CarregaPeriodoMatricula()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlPeriodoMatricula.Items.Clear();
            ddlPeriodoMatricula.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.PeriodoLetivo, RN.PeriodoLetivo.QueryListaPeriodos);
            ddlPeriodoMatricula.DataBind();
            ddlPeriodoMatricula.Items.Insert(0, item);
        }

        private void CarregaAnoMatricula()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlAnoMatricula.Items.Clear();
            ddlAnoMatricula.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.ProximosAnosLetivos, RN.PeriodoLetivo.QueryListaProximosAnos);
            ddlAnoMatricula.DataBind();
            ddlAnoMatricula.Items.Insert(0, item);
        }

        private void CarregaTurnoMatricula()
        {
            RN.Turno rnTurno = new Techne.Lyceum.RN.Turno();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlTurnoMatricula.Items.Clear();

            if (!ddlCursoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                ddlTurnoMatricula.DataSource = rnCurriculo.ObtemListaTurnoPor(ddlCursoMatricula.SelectedValue.ToString());

            }
            else
            {
                ddlTurnoMatricula.DataSource = rnTurno.ListaTurnosPor();
            }
            ddlTurnoMatricula.DataBind();
            ddlTurnoMatricula.Items.Insert(0, item);
        }

        private void CarregaSerieMatricula()
        {
            RN.Serie rnSerie = new Serie();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlSerieMatricula.Items.Clear();

            if ((!tseUnidadeEnsinoMatricula.DBValue.IsNull && tseUnidadeEnsinoMatricula.IsValidDBValue) && !ddlAnoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlCursoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlTurnoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                ddlSerieMatricula.DataSource = rnSerie.ObtemSerieParticipa3FasePor(Convert.ToInt32(ddlAnoMatricula.SelectedValue), Convert.ToInt32(ddlPeriodoMatricula.SelectedValue), tseUnidadeEnsinoMatricula.DBValue.ToString(), ddlCursoMatricula.SelectedValue, ddlTurnoMatricula.SelectedValue.ToString());
            }
            ddlSerieMatricula.DataBind();
            ddlSerieMatricula.Items.Insert(0, item);
        }

        private void CarregaUnidadeEnsinoMatricula()
        {
            tseUnidadeEnsinoMatricula.ResetValue();
            //Cria listagem da tseUnidadeEnsinoMatricula
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;

            //Esta view lista todos as escolas considerando as permissoes usuario
            table = " VW_UNIDADE_ENSINO_SITUACAO ";

            coluna.Add("unidade_ens");
            coluna.Add("nome_comp");
            coluna.Add("setor");
            coluna.Add("cgc");
            coluna.Add("situacao");
            coluna.Add("municipio");
			coluna.Add("ua_atual");

            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

            tseUnidadeEnsinoMatricula.SqlSelect = sqlSelect;
            tseUnidadeEnsinoMatricula.SqlWhere = string.Empty;
            tseUnidadeEnsinoMatricula.DataBind();
        }

        private void CarregarCursoMatricula(int ano, int periodo, string censo, string modalidade, string nivel)
        {
            RN.Curso rnCurso = new Curso();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlCursoMatricula.Items.Clear();

            ddlCursoMatricula.DataSource = rnCurso.ObtemCursoParticipa3FasePor(ano, periodo, censo, modalidade, nivel);
            ddlCursoMatricula.DataBind();
            ddlCursoMatricula.Items.Insert(0, item);
        }

        private void CarregaModalidade()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object objModalidade = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Modalidade, RN.Curso.QueryListaModalidadeCurso);

            ddlModalidadeMatricula.Items.Clear();
            ddlModalidadeMatricula.DataSource = objModalidade;
            ddlModalidadeMatricula.DataBind();
            ddlModalidadeMatricula.Items.Insert(0, item);
        }

        private void CarregaNivel()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object objNivel = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Nivel, RN.Curso.QueryListaTipoCurso);

            ddlNivelMatricula.Items.Clear();
            ddlNivelMatricula.DataSource = objNivel;
            ddlNivelMatricula.DataBind();
            ddlNivelMatricula.Items.Insert(0, item);
        }

        private void CarregaNecessidadeEspecial()
        {
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new RN.NecessidadeEspecial.NecessidadeEspecial();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);
            ListItem itemNaoPossui = new ListItem("Não Possui", "30");

            ddlDeficiencia.Items.Clear();
            ddlDeficiencia.DataSource = rnNecessidadeEspecial.ListaNecessidadeEspecialAtiva();
            ddlDeficiencia.DataBind();
            ddlDeficiencia.Items.Insert(0, itemVazio);
            ddlDeficiencia.Items.Insert(1, itemNaoPossui);
        }

        private void CarregaMotivoEncaminhamento()
        {
            RN.Matriculas.MotivoEncaminhamentoEspecial rnMotivoEncaminhamentoEspecial = new Techne.Lyceum.RN.Matriculas.MotivoEncaminhamentoEspecial();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlMotivo.Items.Clear();

            ddlMotivo.DataSource = rnMotivoEncaminhamentoEspecial.ListaAtivo();
            ddlMotivo.DataBind();
            ddlMotivo.Items.Insert(0, item);
        }


        protected void txtNumero_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    this.CarregaBairro(hdnCodMunicipio.Value);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAnoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPeriodoMatricula.ClearSelection();
                CarregaUnidadeEnsinoMatricula();
                ddlNivelMatricula.ClearSelection();
                ddlModalidadeMatricula.ClearSelection();
                ddlCursoMatricula.Items.Clear();
                ddlTurnoMatricula.Items.Clear();
                ddlSerieMatricula.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CarregaUnidadeEnsinoMatricula();
                ddlNivelMatricula.ClearSelection();
                ddlModalidadeMatricula.ClearSelection();
                ddlCursoMatricula.Items.Clear();
                ddlTurnoMatricula.Items.Clear();
                ddlSerieMatricula.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlModalidadeMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlCursoMatricula.Items.Clear();
                ddlTurnoMatricula.Items.Clear();
                ddlSerieMatricula.Items.Clear();
                if (!ddlAnoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!tseUnidadeEnsinoMatricula.DBValue.IsNull && tseUnidadeEnsinoMatricula.IsValidDBValue) && !ddlModalidadeMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlNivelMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregarCursoMatricula(Convert.ToInt32(ddlAnoMatricula.SelectedValue), Convert.ToInt32(ddlPeriodoMatricula.SelectedValue), tseUnidadeEnsinoMatricula.DBValue.ToString(), ddlModalidadeMatricula.SelectedValue, ddlNivelMatricula.SelectedValue);
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlNivelMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlCursoMatricula.Items.Clear();
                ddlTurnoMatricula.Items.Clear();
                ddlSerieMatricula.Items.Clear();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlCursoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ddlSerieMatricula.Items.Clear();
                this.ddlTurnoMatricula.Items.Clear();

                if (!ddlCursoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaTurnoMatricula();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void ddlTurnoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSerieMatricula.Items.Clear();
                if (!ddlTurnoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaSerieMatricula();
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
                txtNomeMae.Text = string.Empty;

                if (chkNaoDeclarMae.Checked)
                {
                    txtNomeMae.Text = chkNaoDeclarMae.Text.ToUpper();
                    txtNomeMae.ReadOnly = true;
                }

                chkNaoDeclarMaeCadastro.Checked = chkNaoDeclarMae.Checked;
                this.chkNaoDeclarMaeCadastro_CheckedChanged(null, null);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkNaoDeclarMaeCadastro_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNomeMaeCadastro.ReadOnly = true;
                txtNomeMaeCadastro.Text = string.Empty;

                if (chkNaoDeclarMaeCadastro.Checked)
                {
                    txtNomeMaeCadastro.Text = chkNaoDeclarMaeCadastro.Text.ToUpper();
                    txtNomeMaeCadastro.ReadOnly = true;
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
                txtNomePai.Text = string.Empty;
                if (chkNaoDeclarPai.Checked)
                {
                    txtNomePai.Text = chkNaoDeclarPai.Text.ToUpper();
                    txtNomePai.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsinoMatricula_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                var sessao = SessaoUsuario.GetSessaoUsuario();
                ddlNivelMatricula.ClearSelection();
                ddlModalidadeMatricula.ClearSelection();
                ddlCursoMatricula.Items.Clear();
                ddlTurnoMatricula.Items.Clear();
                ddlSerieMatricula.Items.Clear();


                if (!tseUnidadeEnsinoMatricula.DBValue.IsNull)
                {
                    if (tseUnidadeEnsinoMatricula.IsValidDBValue)
                    {
                        sessao.Escola = Convert.ToString(this.tseUnidadeEnsinoMatricula.DBValue);
                    }
                    else
                    {
                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }

                }
                else
                {
                    this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void btnBuscarCandidato_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtNomeBusca.Text.IsNullOrEmptyOrWhiteSpace() && !txtNomeMae.Text.IsNullOrEmptyOrWhiteSpace() && !dtDataNasc.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    this._tipoOperacao = TipoOperacao.Consultar;
                }
                else
                {
                    lblMensagem.Text = "É necessário o Nome, Nome da Mãe e Data de Nascimento.";
                    return;
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.DTOs.DadosEncaminhamentoEspecial dados = new Techne.Lyceum.RN.DTOs.DadosEncaminhamentoEspecial();
                RN.Aluno rnAluno = new Aluno();

                dados.Nome = !txtNomeCadastro.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeCadastro.Text.Trim().ToUpper() : null;
                dados.Pessoa = !hdnPessoa.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(hdnPessoa.Value) : (decimal?)null;
                dados.PreCadastroAlunoId = !hdnPreCadastroAlunoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnPreCadastroAlunoId.Value) : (int?)null;
                dados.DataNascimento = !DtNascimentoCadastro.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(dtDataNasc.Text) : DateTime.MinValue;
                dados.NomeMae = !txtNomeMaeCadastro.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeMaeCadastro.Text.Trim().ToUpper() : null;
                dados.MaeNãoDeclarada = chkNaoDeclarMaeCadastro.Checked;
                dados.Sexo = !ddlSexo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlSexo.SelectedValue : null;
                dados.NomePai = !txtNomePai.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomePai.Text.Trim().ToUpper() : null;
                dados.PaiNãoDeclarado = chkNaoDeclarPai.Checked;
                dados.Cep = !txtCep.Text.IsNullOrEmptyOrWhiteSpace() ? txtCep.Text.Trim() : null;
                dados.Endereco = !txtLogradouro.Text.IsNullOrEmptyOrWhiteSpace() ? txtLogradouro.Text.Trim() : null;
                dados.NumeroEndereco = !txtNumero.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumero.Text.Trim() : null;
                dados.ComplementoEndereco = !txtComplemento.Text.IsNullOrEmptyOrWhiteSpace() ? txtComplemento.Text.Trim() : null;
                dados.UfEndereco = !txtEstado.Value.IsNullOrEmptyOrWhiteSpace() ? txtEstado.Value.Trim() : null;
                dados.MunicipioEndereco = !hdnCodMunicipio.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCodMunicipio.Value : null;
                dados.Bairro = !ddlBairro.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlBairro.SelectedItem.Text : null;
                dados.Cpf = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.RetirarMascaraCPF() : null;
                dados.NecessidadeEspecialId = !ddlDeficiencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlDeficiencia.SelectedValue) : -1;
                dados.Ano = !ddlAnoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAnoMatricula.SelectedValue) : -1;
                dados.Periodo = !ddlPeriodoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPeriodoMatricula.SelectedValue) : -1;
                dados.Censo = (!tseUnidadeEnsinoMatricula.DBValue.IsNull && tseUnidadeEnsinoMatricula.IsValidDBValue) ? tseUnidadeEnsinoMatricula.DBValue.ToString() : null;
                dados.Curso = !ddlCursoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlCursoMatricula.SelectedValue : null;
                dados.Serie = !ddlSerieMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSerieMatricula.SelectedValue) : -1;
                dados.Turno = !ddlTurnoMatricula.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurnoMatricula.SelectedValue : null;
                dados.MotivoEncaminhamentoEspecial = !ddlMotivo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivo.SelectedValue) : -1;
                dados.Observacao = !txtObservacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtObservacao.Text.Trim() : null;
                dados.UsuarioResponsavel = User.Identity.Name;

                validacao = rnAluno.ValidaEncaminhamentoEspecial(dados);

                if (validacao.Valido)
                {
                    rnAluno.InsereEncaminhamentoEspecial(dados);

                    lblMensagem.Text = "Encaminhamento especial incluído com sucesso.";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Encaminhamento especial incluído com sucesso.');", true);

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


    }
}
