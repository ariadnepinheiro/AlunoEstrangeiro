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
    [
          NavUrl("~/Basico/Externo.aspx"),
          ControlText("Recurso Externo"),
          Title("Recurso Externo")
      ]
    public partial class Externo : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Inicial,
            Sucesso,
            BuscaExterno,
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

        private void CarregaTipoExterno()
        {
            RN.RecursosHumanos.TipoUsuarioExterno rnTipoUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.TipoUsuarioExterno();
            ListItem item = new ListItem("Nenhum", string.Empty);

            ddlTipoExterno.Items.Clear();
            ddlTipoExterno.DataSource = rnTipoUsuarioExterno.ListaTipoUsuarioExternoAtivo(); 
            ddlTipoExterno.DataBind();
            ddlTipoExterno.Items.Insert(0, item);
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
            tseMunicipio.ResetValue();
            hdnPessoa.Value = string.Empty;
            hdnUsuarioExterno.Value = string.Empty;
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
            txtEmailAlternativo.Text = string.Empty;
        }

        protected void txtCPF_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RN.DTOs.DadosExterno dadosExterno = new Techne.Lyceum.RN.DTOs.DadosExterno();
                RN.RecursosHumanos.UsuarioExterno rnUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.UsuarioExterno();
                lblmensagemBloqueio.Visible = false;
                _tipoOperacao = TipoOperacao.BuscaExterno;
                ControlarTipoOperacao();
                divDados.Visible = false;

                if (!txtCPF.Text.IsNullOrEmptyOrWhiteSpace() && txtCPF.Text.RetirarMascaraCPF().Length == 11)
                {
                    txtCPF.Text = txtCPF.Text.AplicarMascaraCPF();
                    dadosExterno = rnUsuarioExterno.ObtemDadosExternoPor(txtCPF.Text.RetirarMascaraCPF());
                    hdnBloqueado.Value = dadosExterno.Bloqueado.ToString();

                    if (dadosExterno.PessoaId > 0)
                    {
                        PreencheDadosExterno(dadosExterno);
                        rblAtivo.SelectedValue = "1";
                        if (dadosExterno.Bloqueado)
                        {
                            lblmensagemBloqueio.Visible = true;
                        }
                    }
                    divDados.Visible = true;
                    btnSalvar.Visible = true;
                    ddlTipoExterno.Enabled = true;
                    BloquearDesbloquearEdicao(!dadosExterno.Bloqueado);
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
                txtEmailAlternativo.Enabled = ativado;
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

        private void PreencheDadosExterno(RN.DTOs.DadosExterno dadosExterno)
        {
            //Verifica se não existe o tipo de externo
            if (dadosExterno.TipoExternoId > 0)
            {
                ListItem listItem = ddlTipoExterno.Items.FindByText(dadosExterno.TipoExterno);
                if (listItem == null)
                {
                    ListItem item = new ListItem(dadosExterno.TipoExterno, dadosExterno.TipoExternoId.ToString());
                    ddlTipoExterno.Items.Insert(0, item);
                }

                ddlTipoExterno.SelectedValue = dadosExterno.TipoExternoId.ToString();
            }
           
            txtCPF.Text = dadosExterno.Cpf.AplicarMascaraCPF();
            rblAtivo.SelectedValue = dadosExterno.Ativo ? "1" : "0";
            hdnPessoa.Value = dadosExterno.PessoaId.ToString();
            hdnBloqueado.Value = dadosExterno.Bloqueado.ToString();
            hdnUsuarioExterno.Value = dadosExterno.UsuarioExternoId.ToString();
            txtNomeComplPessoa.Text = dadosExterno.Nome;
            if (dadosExterno.DataNascimento.HasValue)
            {
                dteDtNasc.Date = dadosExterno.DataNascimento.Value;
            }
            if (!string.IsNullOrEmpty(dadosExterno.Sexo))
            {
                if (rblSexo.Items.FindByValue(dadosExterno.Sexo) != null)
                {
                    rblSexo.Text = dadosExterno.Sexo;
                }
            }
            ddlEst_Civil.SelectedValue = dadosExterno.EstadoCivl;

            if (!dadosExterno.PaisEndereco.IsNullOrEmptyOrWhiteSpace())
            {
                ddlPais.SelectedValue = dadosExterno.PaisEndereco;
            }

            txtCEP.Text = dadosExterno.Cep;
            tseMunicipio.DBValue = dadosExterno.Municipio;
            tseMunicipio_Changed(null, null);
            txtEnderecoPessoa.Text = dadosExterno.Endereco;
            txtEndNumPessoa.Text = dadosExterno.Numero;
            txtEndCompl.Text = dadosExterno.Complemento;
            txtBairro.Text = dadosExterno.Bairro;
            txtTelefone.Text = dadosExterno.Telefone.AplicarMascaraTelefoneComDDD();
            txtCelular.Text = dadosExterno.Celular.AplicarMascaraTelefoneComDDD();
            txtEmail.Text = dadosExterno.Email;
            txtEmailAlternativo.Text = dadosExterno.EmailAlternativo;
           
        }

        protected void tseExterno_Changed(object sender, EventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            try
            {
                RN.DTOs.DadosExterno dadosExterno = new Techne.Lyceum.RN.DTOs.DadosExterno();
                RN.RecursosHumanos.UsuarioExterno rnUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.UsuarioExterno();

                ddlTipoExterno.ClearSelection();
                txtCPF.Text = string.Empty;
                LimparTela();
                if (!this.tseExterno.DBValue.IsNull)
                {
                    if (this.tseExterno.IsValidDBValue)
                    {
                        dadosExterno = rnUsuarioExterno.ObtemDadosExternoPor(Convert.ToInt32(tseExterno["codigo"].ToString())); 
                        hdnBloqueado.Value = dadosExterno.Bloqueado.ToString();
                        if (dadosExterno.PessoaId > 0)
                        {
                            PreencheDadosExterno(dadosExterno);

                            _tipoOperacao = TipoOperacao.Consultar;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Recurso externo não cadastrado.";
                        _tipoOperacao = TipoOperacao.Inicial;
                    }
                }
                else
                {
                    lblMensagem.Text = "Recurso externo não cadastrado.";
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
                RN.RecursosHumanos.UsuarioExterno rnUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.UsuarioExterno();
                RN.DTOs.DadosExterno dadosExterno = new Techne.Lyceum.RN.DTOs.DadosExterno();
                List<int> tipoExterno = new List<int>();
                bool cadastro = false;                

                dadosExterno.Bloqueado = !hdnBloqueado.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToBoolean(hdnBloqueado.Value) : true;
                dadosExterno.Ativo = rblAtivo.SelectedValue == "1" ? true : false;
                dadosExterno.PessoaId = !hdnPessoa.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(hdnPessoa.Value) : 0;
                dadosExterno.Cpf = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? this.txtCPF.Text.RetirarMascaraCPF() : null;
                dadosExterno.Nome = !txtNomeComplPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeComplPessoa.Text.Trim().ToUpper() : null;
                dadosExterno.DataNascimento = dteDtNasc.Date != DateTime.MinValue ? dteDtNasc.Date : DateTime.MinValue;
                dadosExterno.Sexo = !rblSexo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblSexo.SelectedValue : null;
                dadosExterno.EstadoCivl = !ddlEst_Civil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEst_Civil.SelectedValue : null;
                dadosExterno.PaisEndereco = !ddlPais.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPais.SelectedValue : null;
                dadosExterno.Cep = !txtCEP.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToString(txtCEP.Text.Replace("-", String.Empty).Replace(".", String.Empty)) : null;
                dadosExterno.Municipio = (!tseMunicipio.DBValue.IsNull && this.tseMunicipio.IsValidDBValue) ? Convert.ToString(tseMunicipio.Value.ToString()) : null;
                dadosExterno.Endereco = !txtEnderecoPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtEnderecoPessoa.Text.Trim().ToUpper() : null;
                dadosExterno.Numero = !txtEndNumPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? this.txtEndNumPessoa.Text.ToUpper() : null;
                dadosExterno.Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text.Trim().ToUpper() : null;
                dadosExterno.Complemento = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text.Trim().ToUpper() : null;
                dadosExterno.Email = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null;
                dadosExterno.EmailAlternativo = !txtEmailAlternativo.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailAlternativo.Text.Trim() : null;
                dadosExterno.Telefone = !txtTelefone.Text.IsNullOrEmptyOrWhiteSpace() ? txtTelefone.Text.Trim().RetirarMascaraTelefone() : null;
                dadosExterno.Celular = !txtCelular.Text.IsNullOrEmptyOrWhiteSpace() ? txtCelular.Text.Trim().RetirarMascaraTelefone() : null;
                dadosExterno.UsuarioResponsavel = User.Identity.Name;
                dadosExterno.UsuarioExternoId = !hdnUsuarioExterno.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnUsuarioExterno.Value) : 0;
                dadosExterno.TipoExternoId = !ddlTipoExterno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlTipoExterno.SelectedValue) : 0;

                cadastro = dadosExterno.UsuarioExternoId == 0;
                validacao = rnUsuarioExterno.Valida(dadosExterno, cadastro); 

                if (validacao.Valido)
                {
                    if (dadosExterno.UsuarioExternoId == 0)
                    {
                        rnUsuarioExterno.Insere(dadosExterno);
                        tseExterno.ResetValue();
                        tseExterno.DBValue = dadosExterno.Cpf;
                        hdnUsuarioExterno.Value = dadosExterno.UsuarioExternoId.ToString();
                        hdnPessoa.Value = dadosExterno.PessoaId.ToString();
                    }
                    else
                    {
                        rnUsuarioExterno.Atualiza(dadosExterno); 
                    }

                    lblMensagem.Text = "Recurso externo " + (cadastro ? "incluído" : "alterado") + " com sucesso.";
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
                tseExterno.ResetValue();
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
                RN.RecursosHumanos.UsuarioExterno rnUsuarioExterno = new Techne.Lyceum.RN.RecursosHumanos.UsuarioExterno();

                if (!hdnUsuarioExterno.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    int tipoExternoid = !ddlTipoExterno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlTipoExterno.SelectedValue) : 0;
                    validacao = rnUsuarioExterno.ValidaDesativacao(Convert.ToInt32(hdnUsuarioExterno.Value), tipoExternoid, Convert.ToDecimal(hdnPessoa.Value), User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnUsuarioExterno.Desativa(Convert.ToInt32(hdnUsuarioExterno.Value), tipoExternoid, Convert.ToDecimal(hdnPessoa.Value), User.Identity.Name);
                        rblAtivo.SelectedValue = "0";
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
                    lblMensagem.Text = "Recurso externo não encontrado.";
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
                        ddlTipoExterno.ClearSelection();
                        txtCPF.Text = string.Empty;
                        tseExterno.Enabled = true;
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
                        ddlTipoExterno.ClearSelection();
                        ddlTipoExterno.Enabled = true;
                        txtCPF.Text = string.Empty;
                        txtCPF.Enabled = true;
                        pnlBuscaCPF.Visible = true;
                        tseExterno.ResetValue();
                        tseExterno.Enabled = false;
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
                        lblMensagem.Text = "Recurso externo desabilitado com sucesso.";

                        break;
                    }
                case TipoOperacao.BuscaExterno:
                    {
                        LimparTela();
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
                                ddlTipoExterno.Enabled = true;
                                BloquearDesbloquearEdicao(!ativado);
                                lblmensagemBloqueio.Visible = true;
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
            CarregaTipoExterno();
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
            rblAtivo.Enabled = false;
            txtNomeComplPessoa.Enabled = false;
            dteDtNasc.Enabled = false;
            rblSexo.Enabled = false;
            ddlEst_Civil.Enabled = false;
            txtTelefone.Enabled = false;
            txtCelular.Enabled = false;
            txtEmail.Enabled = false;
            txtEmailAlternativo.Enabled = false;
            txtCPF.Enabled = false;
            ddlTipoExterno.Enabled = false;

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
            ddlTipoExterno.Enabled = true;
            txtMunicipio.ReadOnly = false;
            tseMunicipio.Enabled = true;
            txtEstado.Attributes.Add("readonly", "readonly");
            rblAtivo.Enabled = true;
            txtNomeComplPessoa.Enabled = true;
            dteDtNasc.Enabled = true;
            rblSexo.Enabled = true;
            ddlEst_Civil.Enabled = true;
            ddlPais.Enabled = true;
            txtTelefone.Enabled = true;
            txtCelular.Enabled = true;
            txtEmail.Enabled = true;
            txtEmailAlternativo.Enabled = true;
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
