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

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/RecursoAtendimentoEducacaoEspecial.aspx")]
    [ControlText("Recurso Atendimento Educação Especial")]
    [Title("Recurso Atendimento Educação Especial")]

    public partial class RecursoAtendimentoEducacaoEspecial : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Inicial,
            Sucesso,
            BuscaRecurso,
            Consultar
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
                dteDtNasc.MaxDate = DateTime.Now.Date.AddDays(-1);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaTipoRecurso()
        {
            RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial rnTipoRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.TipoRecursoNecessidadeEspecial();

            chkTipoRecurso.Items.Clear();
            chkTipoRecurso.DataSource = rnTipoRecursoNecessidadeEspecial.ListaTipoRecursoNecessidadeEspecialAtivo();
            chkTipoRecurso.DataTextField = "DESCRICAO";
            chkTipoRecurso.DataValueField = "TIPORECURSONECESSIDADEESPECIALID";
            chkTipoRecurso.DataBind();
        }

        private void CarregaEstadoCivil()
        {
            ListItem item = new ListItem("Nenhum", string.Empty);

            ddlEst_Civil.Items.Clear();
            ddlEst_Civil.DataSource = RN.Util.Cache.CarregaItemTabelaGeralPor(RN.Util.Cache.EstadoCivil);
            ddlEst_Civil.DataBind();
            ddlEst_Civil.Items.Insert(0, item);
        }

        private void CarregaPaisEndereco()
        {
            ListItem item = new ListItem("Nenhum", string.Empty);

            ddlPais.Items.Clear();
            ddlPais.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Pais, RN.Basico.QueryListaPaises);
            ddlPais.DataBind();
            ddlPais.Items.Insert(0, item);

            item = ddlPais.Items.FindByText("BRASIL");
            if (item != null)
            {
                ddlPais.ClearSelection();
                item.Selected = true;
            }
        }

        protected void LimparTela()
        {
            hdnBloqueado.Value = string.Empty;
            hdnRecursoId.Value = string.Empty;
            chkTipoRecurso.ClearSelection();
            tseMunicipio.ResetValue();
            hdnPessoa.Value = string.Empty;
            txtNomeComplPessoa.Text = string.Empty;
            dteDtNasc.Text = string.Empty;
            rblSexo.SelectedIndex = -1;
            ddlEst_Civil.ClearSelection();
            txtEnderecoPessoa.Text = string.Empty;
            txtEndNumPessoa.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            ddlPais.ClearSelection();
            txtMunicipio.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtBairro.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }

        protected void txtCPF_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RN.DTOs.DadosRecursoNecessidadeEspecial dadosRecursoNecessidadeEspecial = new Techne.Lyceum.RN.DTOs.DadosRecursoNecessidadeEspecial();
                RN.NecessidadeEspecial.RecursoNecessidadeEspecial rnRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.RecursoNecessidadeEspecial();
                lblmensagemBloqueio.Visible = false;
                _tipoOperacao = TipoOperacao.BuscaRecurso;
                ControlarTipoOperacao();
                divDados.Visible = false;

                if (!txtCPF.Text.IsNullOrEmptyOrWhiteSpace() && txtCPF.Text.RetirarMascaraCPF().Length == 11)
                {
                    txtCPF.Text = txtCPF.Text.AplicarMascaraCPF();
                    dadosRecursoNecessidadeEspecial = rnRecursoNecessidadeEspecial.ObtemDadosRecursoNecessidadeEspecialPor(txtCPF.Text.RetirarMascaraCPF());
                    hdnBloqueado.Value = dadosRecursoNecessidadeEspecial.Bloqueado.ToString();

                    if (dadosRecursoNecessidadeEspecial.PessoaId > 0)
                    {
                        PreencheDadosRecurso(dadosRecursoNecessidadeEspecial);
                        rblAtivo.SelectedValue = "1";
                        if (dadosRecursoNecessidadeEspecial.Bloqueado)
                        {
                            lblmensagemBloqueio.Visible = true;
                        }
                    }
                    divDados.Visible = true;
                    btnSalvar.Visible = true;
                    BloquearDesbloquearEdicao(!dadosRecursoNecessidadeEspecial.Bloqueado);
                }
                else
                {
                    lblMensagem.Text = "CPF é de preenchimento obrigatório.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void BloquearDesbloquearEdicao(bool ativado)
        {
            try
            {
                txtNomeComplPessoa.Enabled = ativado;
                txtCEP.Enabled = ativado;
                tsCEP.Enabled = ativado;
                tseMunicipio.Enabled = ativado;
                txtEnderecoPessoa.Enabled = ativado;
                txtEndNumPessoa.Enabled = ativado;
                txtEndCompl.Enabled = ativado;
                txtBairro.Enabled = ativado; ;
                txtTelefone.Enabled = ativado;
                txtCelular.Enabled = ativado;
                txtEmail.Enabled = ativado;
                dteDtNasc.Enabled = ativado;
                rblSexo.Enabled = ativado;
                ddlEst_Civil.Enabled = ativado;
                ddlPais.Enabled = ativado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencheDadosRecurso(RN.DTOs.DadosRecursoNecessidadeEspecial dadosRecursoNecessidadeEspecial)
        {
            List<int> listaTipoRecurso = new List<int>();

            txtCPF.Text = dadosRecursoNecessidadeEspecial.Cpf.AplicarMascaraCPF();
            rblAtivo.SelectedValue = dadosRecursoNecessidadeEspecial.Ativo ? "1" : "0";
            hdnPessoa.Value = dadosRecursoNecessidadeEspecial.PessoaId.ToString();
            hdnRecursoId.Value = dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId.ToString();
            hdnBloqueado.Value = dadosRecursoNecessidadeEspecial.Bloqueado.ToString();
            txtNomeComplPessoa.Text = dadosRecursoNecessidadeEspecial.Nome;
            if (dadosRecursoNecessidadeEspecial.DataNascimento.HasValue)
            {
                dteDtNasc.Date = dadosRecursoNecessidadeEspecial.DataNascimento.Value;
            }
            if (!string.IsNullOrEmpty(dadosRecursoNecessidadeEspecial.Sexo))
            {
                if (rblSexo.Items.FindByValue(dadosRecursoNecessidadeEspecial.Sexo) != null)
                {
                    rblSexo.Text = dadosRecursoNecessidadeEspecial.Sexo;
                }
            }
            ddlEst_Civil.SelectedValue = dadosRecursoNecessidadeEspecial.EstadoCivl;
            
            if (!dadosRecursoNecessidadeEspecial.PaisEndereco.IsNullOrEmptyOrWhiteSpace())
            {
                ddlPais.SelectedValue = dadosRecursoNecessidadeEspecial.PaisEndereco;
            }

            txtCEP.Text = dadosRecursoNecessidadeEspecial.Cep;
            tseMunicipio.DBValue = dadosRecursoNecessidadeEspecial.Municipio;
            tseMunicipio_Changed(null, null);
            txtEnderecoPessoa.Text = dadosRecursoNecessidadeEspecial.Endereco;
            txtEndNumPessoa.Text = dadosRecursoNecessidadeEspecial.Numero;
            txtEndCompl.Text = dadosRecursoNecessidadeEspecial.Complemento;
            txtBairro.Text = dadosRecursoNecessidadeEspecial.Bairro;
            txtTelefone.Text = dadosRecursoNecessidadeEspecial.Telefone.AplicarMascaraTelefoneComDDD();
            txtCelular.Text = dadosRecursoNecessidadeEspecial.Celular.AplicarMascaraTelefoneComDDD();
            txtEmail.Text = dadosRecursoNecessidadeEspecial.Email;

            if (dadosRecursoNecessidadeEspecial.TipoRecursoNecessidadeEspecial != null)
            {
                listaTipoRecurso = dadosRecursoNecessidadeEspecial.TipoRecursoNecessidadeEspecial;

                foreach (var item in listaTipoRecurso)
                {
                    chkTipoRecurso.Items.FindByValue(item.ToString()).Selected = true;
                }
            }
        }

        protected void tseRecurso_Changed(object sender, EventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            try
            {
                RN.DTOs.DadosRecursoNecessidadeEspecial dadosRecursoNecessidadeEspecial = new Techne.Lyceum.RN.DTOs.DadosRecursoNecessidadeEspecial();
                RN.NecessidadeEspecial.RecursoNecessidadeEspecial rnRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.RecursoNecessidadeEspecial();

                LimparTela();
                if (!this.tseRecurso.DBValue.IsNull)
                {
                    if (this.tseRecurso.IsValidDBValue)
                    {
                        dadosRecursoNecessidadeEspecial = rnRecursoNecessidadeEspecial.ObtemDadosRecursoNecessidadeEspecialPor(Convert.ToInt32(tseRecurso["codigo"].ToString()));
                        hdnBloqueado.Value = dadosRecursoNecessidadeEspecial.Bloqueado.ToString();
                        if (dadosRecursoNecessidadeEspecial.PessoaId > 0)
                        {
                            PreencheDadosRecurso(dadosRecursoNecessidadeEspecial);

                            _tipoOperacao = TipoOperacao.Consultar;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Recurso não cadastrado.";
                        _tipoOperacao = TipoOperacao.Inicial;
                    }
                }
                else
                {
                    lblMensagem.Text = "Recurso não cadastrado.";
                    _tipoOperacao = TipoOperacao.Inicial;
                }
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEnderecoPessoa.Text = string.Empty;
            txtEndNumPessoa.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtBairro.Text = string.Empty;
            txtCEP.Text = string.Empty;
            tseMunicipio.ResetValue();
        }

        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {
            if (tseMunicipio.IsValidDBValue
                && !tseMunicipio.DBValue.IsNull)
            {
                txtEstado.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
            }
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Novo;
                ControlarTipoOperacao();
                rblAtivo.SelectedValue = "1";
                rblAtivo.Enabled = true;
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
                ValidacaoDados validacao = new ValidacaoDados();
                RN.NecessidadeEspecial.RecursoNecessidadeEspecial rnRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.RecursoNecessidadeEspecial();
                RN.DTOs.DadosRecursoNecessidadeEspecial dadosRecursoNecessidadeEspecial = new Techne.Lyceum.RN.DTOs.DadosRecursoNecessidadeEspecial();
                List<int> tipoRecurso = new List<int>();
                bool cadastro = false;

                foreach (ListItem item in chkTipoRecurso.Items)
                {
                    if (item.Selected)
                    {
                        tipoRecurso.Add(Convert.ToInt32(item.Value));
                    }
                }

                dadosRecursoNecessidadeEspecial.Bloqueado = Convert.ToBoolean(hdnBloqueado.Value);
                dadosRecursoNecessidadeEspecial.Ativo = rblAtivo.SelectedValue == "1" ? true : false;
                dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId = !hdnRecursoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnRecursoId.Value) : 0;
                dadosRecursoNecessidadeEspecial.PessoaId = !hdnPessoa.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(hdnPessoa.Value) : 0;
                dadosRecursoNecessidadeEspecial.Cpf = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? this.txtCPF.Text.RetirarMascaraCPF() : null;
                dadosRecursoNecessidadeEspecial.Nome = !txtNomeComplPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeComplPessoa.Text.Trim() : null;
                dadosRecursoNecessidadeEspecial.DataNascimento = dteDtNasc.Date != DateTime.MinValue ? dteDtNasc.Date : DateTime.MinValue;
                dadosRecursoNecessidadeEspecial.Sexo = !rblSexo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblSexo.SelectedValue : null;
                dadosRecursoNecessidadeEspecial.EstadoCivl = !ddlEst_Civil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEst_Civil.SelectedValue : null;
                dadosRecursoNecessidadeEspecial.PaisEndereco = !ddlPais.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPais.SelectedValue : null;
                dadosRecursoNecessidadeEspecial.Cep = !txtCEP.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToString(txtCEP.Text.Replace("-", String.Empty).Replace(".", String.Empty)) : null;
                dadosRecursoNecessidadeEspecial.Municipio = (!tseMunicipio.DBValue.IsNull && this.tseMunicipio.IsValidDBValue) ? Convert.ToString(tseMunicipio.Value.ToString()) : null;
                dadosRecursoNecessidadeEspecial.Endereco = !txtEnderecoPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtEnderecoPessoa.Text.Trim() : null;
                dadosRecursoNecessidadeEspecial.Numero = !txtEndNumPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? this.txtEndNumPessoa.Text : null;
                dadosRecursoNecessidadeEspecial.Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text.Trim() : null;
                dadosRecursoNecessidadeEspecial.Complemento = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text.Trim() : null;
                dadosRecursoNecessidadeEspecial.Email = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null;
                dadosRecursoNecessidadeEspecial.Telefone = !txtTelefone.Text.IsNullOrEmptyOrWhiteSpace() ? txtTelefone.Text.Trim().RetirarMascaraTelefone() : null;
                dadosRecursoNecessidadeEspecial.Celular = !txtCelular.Text.IsNullOrEmptyOrWhiteSpace() ? txtCelular.Text.Trim().RetirarMascaraTelefone() : null;
                dadosRecursoNecessidadeEspecial.UsuarioId = User.Identity.Name;
                dadosRecursoNecessidadeEspecial.TipoRecursoNecessidadeEspecial = tipoRecurso;

                cadastro = dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId == 0;
                validacao = rnRecursoNecessidadeEspecial.Valida(dadosRecursoNecessidadeEspecial, cadastro);

                if (validacao.Valido)
                {
                    if (dadosRecursoNecessidadeEspecial.RecursoNecessidadeEspecialId == 0)
                    {
                        rnRecursoNecessidadeEspecial.Insere(dadosRecursoNecessidadeEspecial);
                        tseRecurso.ResetValue();
                        tseRecurso.DBValue = dadosRecursoNecessidadeEspecial.Cpf;
                    }
                    else
                    {
                        rnRecursoNecessidadeEspecial.Atualiza(dadosRecursoNecessidadeEspecial);
                    }

                    lblMensagem.Text = "Recurso " + (cadastro ? "incluído" : "alterado") + " com sucesso.";
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

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                tseRecurso.ResetValue();
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnDesabilitar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.NecessidadeEspecial.RecursoNecessidadeEspecial rnRecursoNecessidadeEspecial = new Techne.Lyceum.RN.NecessidadeEspecial.RecursoNecessidadeEspecial();

                if (!hdnRecursoId.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    validacao = rnRecursoNecessidadeEspecial.ValidaDesativacao(Convert.ToInt32(hdnRecursoId.Value));

                    if (validacao.Valido)
                    {
                        rnRecursoNecessidadeEspecial.Desativa(Convert.ToInt32(hdnRecursoId.Value), User.Identity.Name);
                        _tipoOperacao = TipoOperacao.Excluir;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }
                else
                {
                    lblMensagem.Text = "Recurso não encontrado.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles);
                        pnlBuscaCPF.Visible = false;
                        CarregaCampos();
                        divDados.Visible = false;
                        tseRecurso.Enabled = true;
                        LimparTela();
                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnDesabilitar };                        
                        ControlarVisibilidadeControle(controles);

                        if (rblAtivo.SelectedValue == "0")
                        {
                            btnDesabilitar.Visible = false;
                        }

                        DesabilitaCampos();
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        LimparTela();
                        txtCPF.Text = string.Empty;
                        txtCPF.Enabled = true;
                        pnlBuscaCPF.Visible = true;
                        tseRecurso.ResetValue();
                        tseRecurso.Enabled = false;
                        divDados.Visible = false;
                        var controles = new[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        break;
                    }

                case TipoOperacao.Excluir:
                    {
                        _tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                        btnDesabilitar.Visible = false;
                        lblMensagem.Text = "Recurso desabilitado com sucesso.";

                        break;
                    }
                case TipoOperacao.BuscaRecurso:
                    {
                        LimparTela();
                        CarregaCampos();
                        HabilitaCampos();

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar, btnDesabilitar };
                        ControlarVisibilidadeControle(controles);
                        DesabilitaCampos();

                        if (rblAtivo.SelectedValue == "0")
                        {
                            btnDesabilitar.Visible = false;
                        }

                        pnlBuscaCPF.Visible = true;
                        divDados.Visible = true;
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        var controles = new[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        if (!hdnBloqueado.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            bool ativado = Convert.ToBoolean(hdnBloqueado.Value);
                            if (ativado)
                            {
                                BloquearDesbloquearEdicao(!ativado);
                                lblmensagemBloqueio.Visible = true;
                                chkTipoRecurso.Enabled = true;
                                rblAtivo.Enabled = true;
                            }
                            else
                            {
                                HabilitaCampos();
                                lblmensagemBloqueio.Visible = false;
                            }
                        }
                        txtCPF.Enabled = false;
                        break;
                    }
            }
        }

        protected void CarregaCampos()
        {
            CarregaEstadoCivil();
            CarregaPaisEndereco();
            CarregaTipoRecurso();
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnDesabilitar, AcaoControle.excluir);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnDesabilitar.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        protected void DesabilitaCampos()
        {
            txtMunicipio.ReadOnly = true;
            tseMunicipio.Enabled = false;
            txtEstado.Attributes.Add("readonly", "readonly");
            chkTipoRecurso.Enabled = false;
            rblAtivo.Enabled = false;
            txtNomeComplPessoa.Enabled = false;
            dteDtNasc.Enabled = false;
            rblSexo.Enabled = false;
            ddlEst_Civil.Enabled = false;
            txtTelefone.Enabled = false;
            txtCelular.Enabled = false;
            txtEmail.Enabled = false; ;
            txtCPF.Enabled = false;

            ddlPais.Enabled = false;
            txtCEP.Enabled = false;
            tsCEP.ShowButton = false;

            txtEndCompl.Enabled = false;
            txtEndNumPessoa.Enabled = false;
            txtEnderecoPessoa.Enabled = false;
            txtBairro.Enabled = false;
        }

        protected void HabilitaCampos()
        {
            txtMunicipio.ReadOnly = false;
            tseMunicipio.Enabled = true;
            txtEstado.Attributes.Add("readonly", "readonly");
            chkTipoRecurso.Enabled = true;
            rblAtivo.Enabled = true;

            txtNomeComplPessoa.Enabled = true;
            dteDtNasc.Enabled = true;
            rblSexo.Enabled = true;
            ddlEst_Civil.Enabled = true;
            ddlPais.Enabled = true;
            txtTelefone.Enabled = true;
            txtCelular.Enabled = true;
            txtEmail.Enabled = true;
            txtCPF.Enabled = true;

            txtCEP.Enabled = true;
            tsCEP.ShowButton = true;

            txtEndCompl.Enabled = true;
            txtEndNumPessoa.Enabled = true;
            txtEnderecoPessoa.Enabled = true;
            txtBairro.Enabled = true;
        }
    }
}
