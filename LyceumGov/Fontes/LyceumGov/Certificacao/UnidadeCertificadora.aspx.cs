using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Web;
using Techne.Controls;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Certificacao
{
    [
         NavUrl("~/Certificacao/UnidadeCertificadora.aspx"),
         ControlText("UnidadeCertificadora"),
         Title("Unidade Certificadora")
     ]

    public partial class UnidadeCertificadora : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return
                Techne.Web.Navigation.GetNavigation(
                    System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #region Web Form Designer gnerated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion

        #region :: Enumerador e ViewState do Tipo de Operação ::
        public enum TipoOperacao
        {
            Iniciar,
            Consultar,
            Novo,
            Alterar,
            Excluir,
            Cancelar,
            Sucesso
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = String.Empty;

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Iniciar;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseUnidade_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                LimparTela();
                rblTipo.ClearSelection();

                if (!tseUnidade.DBValue.IsNull)
                {
                    if (tseUnidade.IsValidDBValue)
                    {
                        ImageButton[] botoes = new ImageButton[] { btnNovo, btnAlterar, btnExcluir };
                        VisibilidadeBotoes(botoes);

                        _tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                    }
                }
                else
                {
                    lblMensagem.Text = "Unidade não cadastrada.";
                    _tipoOperacao = TipoOperacao.Iniciar;
                    ControlarTipoOperacao();
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
                {
                    txtEstado.Text = Convert.ToString(tseMunicipio["uf_sigla"]);
                }
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

        protected void btnAlterar_Click(object sender, ImageClickEventArgs e)
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

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Excluir;
                ControlarTipoOperacao();
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
                RN.Certificacao.Entidades.UnidadeCertificadora unidade = new Techne.Lyceum.RN.Certificacao.Entidades.UnidadeCertificadora();
                RN.Certificacao.UnidadeCertificadora rnUnidadeCertificadora = new RN.Certificacao.UnidadeCertificadora();

                unidade.Ativo = chkAtivo.Checked ? true : false;
                unidade.Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text.ToUpper().Trim() : null;
                unidade.Cep = !txtCEP.Text.IsNullOrEmptyOrWhiteSpace() ? txtCEP.Text.Trim() : null;
                unidade.Complemento = !txtComplemento.Text.IsNullOrEmptyOrWhiteSpace() ? txtComplemento.Text.ToUpper().Trim() : null;
                unidade.Descricao = !txtUnidade.Text.IsNullOrEmptyOrWhiteSpace() ? txtUnidade.Text.ToUpper().Trim() : null;
                unidade.DescricaoSite = !txtDescricaoSite.Text.IsNullOrEmptyOrWhiteSpace() ? txtDescricaoSite.Text.ToUpper().Trim() : null;
                unidade.Endereco = !txtEndereco.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndereco.Text.ToUpper().Trim() : null;
                unidade.GrupoUnidadeCertificadoraId = !ddlGrupo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlGrupo.SelectedValue) : -1;
                unidade.Municipio = (tseMunicipio.IsValidDBValue && !tseMunicipio.DBValue.IsNull) ? tseMunicipio.DBValue.ToString() : null;
                unidade.Numero = !txtNumero.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumero.Text.Trim() : null;
                unidade.Telefone = !txtFone.Text.IsNullOrEmptyOrWhiteSpace() ? txtFone.Text.RetirarMascaraTelefone() : null;
                unidade.Tipo = !rblTipo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblTipo.SelectedValue : null;
                unidade.UnidadeCertificadoraId = (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull) ? Convert.ToInt32(tseUnidade.DBValue) : -1;
                unidade.UsuarioId = User.Identity.Name;

                validacao = rnUnidadeCertificadora.Valida(unidade, unidade.UnidadeCertificadoraId == -1 ? true : false);

                if (validacao.Valido)
                {
                    if (unidade.UnidadeCertificadoraId == -1)
                    {
                        rnUnidadeCertificadora.Insere(unidade);
                        tseUnidade.ResetValue();
                        tseUnidade.DBValue = unidade.UnidadeCertificadoraId.ToString();
                        tseUnidade.DataBind();
                    }
                    else
                    {
                        rnUnidadeCertificadora.Atualiza(unidade);
                    }

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                    lblMensagem.Text = "Unidade Certificadora salva com sucesso.";
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

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Cancelar;
                ControlarTipoOperacao();
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
                case TipoOperacao.Iniciar:
                    {
                        ImageButton[] botoes = new ImageButton[] { btnNovo };
                        VisibilidadeBotoes(botoes);

                        tseUnidade.ResetValue();
                        DesabilitarCampos();
                        CarregaGrupo();
                        pnlUnidade.Visible = false;

                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        lblMensagem.Text = String.Empty;

                        ImageButton[] botoes = new ImageButton[] { btnNovo, btnAlterar, btnExcluir };
                        VisibilidadeBotoes(botoes);
                        PreencherDados();
                        DesabilitarCampos();
                        pnlUnidade.Visible = true;

                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        LimparTela();
                        lblMensagem.Text = String.Empty;

                        ImageButton[] botoes = new ImageButton[] { btnSalvar, btnCancelar };
                        VisibilidadeBotoes(botoes);

                        HabilitarCampos();
                        pnlUnidade.Visible = true;
                        tseUnidade.Enabled = false;
                        tseUnidade.ResetValue();
                        tseUnidade.Mode = ControlMode.View;
                        rblTipo.ClearSelection();
                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        ImageButton[] botoes = new ImageButton[] { btnSalvar, btnCancelar };
                        VisibilidadeBotoes(botoes);
                        HabilitarCampos();
                        pnlUnidade.Visible = true;

                        break;
                    }

                case TipoOperacao.Excluir:
                    {
                        RN.Certificacao.UnidadeCertificadora rnUnidadeCertificadora = new Techne.Lyceum.RN.Certificacao.UnidadeCertificadora();
                        ValidacaoDados validacao = new ValidacaoDados();

                        validacao = rnUnidadeCertificadora.ValidaRemocao(Convert.ToInt32(tseUnidade.DBValue));

                        if (validacao.Valido)
                        {
                            rnUnidadeCertificadora.Remove(Convert.ToInt32(tseUnidade.DBValue));
                            LimparTela();
                            rblTipo.ClearSelection();

                            lblMensagem.Text = "Unidade Certificadora excluída com sucesso";
                        }
                        else
                        {
                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                            return;
                        }

                        _tipoOperacao = TipoOperacao.Iniciar;
                        ControlarTipoOperacao();


                        break;
                    }

                case TipoOperacao.Cancelar:
                    {
                        LimparTela();
                        rblTipo.ClearSelection();

                        if (!tseUnidade.DBValue.IsNull)
                        {
                            _tipoOperacao = TipoOperacao.Consultar;
                            ControlarTipoOperacao();
                        }
                        else
                        {
                            _tipoOperacao = TipoOperacao.Iniciar;
                            ControlarTipoOperacao();
                        }

                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] botoes = new ImageButton[] { btnNovo, btnAlterar, btnExcluir };
                        VisibilidadeBotoes(botoes);

                        tseUnidade.Mode = ControlMode.Edit;
                        DesabilitarCampos();
                        break;
                    }
            }
        }

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Iniciar:
                    {
                        tseUnidade.Enabled = true;

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        tseUnidade.Enabled = true;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        tseUnidade.Enabled = false;
                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        tseUnidade.Enabled = false;
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tseUnidade.Enabled = false;
                        break;
                    }               
            }
        }

        private void VisibilidadeBotoes(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotoes();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcessoBotoes();
        }

        private void RetiraVisibilidadeBotoes()
        {
            btnNovo.Visible = false;
            btnAlterar.Visible = false;
            btnExcluir.Visible = false;
            btnSalvar.Visible = false;
            btnCancelar.Visible = false;
        }

        private void ControlaAcessoBotoes()
        {
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnAlterar, AcaoControle.editar);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
        }

        private void LimparTela()
        {
            txtUnidade.Text = string.Empty;
            txtDescricaoSite.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtEstado.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtNumero.Text = string.Empty;
            txtComplemento.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtFone.Text = string.Empty;
            tsCEP.ResetValue();
            tseMunicipio.ResetValue();
            ddlGrupo.ClearSelection();
            chkAtivo.Checked = false;
        }

        private void HabilitarCampos()
        {
            txtUnidade.ReadOnly = false;
            txtDescricaoSite.ReadOnly = false;
            txtCEP.ReadOnly = false;
            txtEndereco.ReadOnly = false;
            txtNumero.ReadOnly = false;
            txtComplemento.ReadOnly = false;
            txtBairro.ReadOnly = false;
            txtFone.ReadOnly = false;
            tsCEP.ReadOnly = false;
            tseMunicipio.ReadOnly = false;
            ddlGrupo.Enabled = true;
            rblTipo.Enabled = true;
        }

        private void DesabilitarCampos()
        {
            txtUnidade.ReadOnly = true;
            txtDescricaoSite.ReadOnly = true;
            txtCEP.ReadOnly = true;
            txtEstado.ReadOnly = true;
            txtEndereco.ReadOnly = true;
            txtNumero.ReadOnly = true;
            txtComplemento.ReadOnly = true;
            txtBairro.ReadOnly = true;
            txtFone.ReadOnly = true;
            tsCEP.ReadOnly = true;
            tseMunicipio.ReadOnly = true;
            ddlGrupo.Enabled = false;
            rblTipo.Enabled = false;
        }

        private void PreencherDados()
        {
            try
            {
                RN.Certificacao.UnidadeCertificadora rnUnidadeCertificadora = new Techne.Lyceum.RN.Certificacao.UnidadeCertificadora();
                RN.Certificacao.Entidades.UnidadeCertificadora unidade = new Techne.Lyceum.RN.Certificacao.Entidades.UnidadeCertificadora();

                LimparTela();
                rblTipo.ClearSelection();

                if (tseUnidade != null && tseUnidade.Value != null)
                {
                    unidade = rnUnidadeCertificadora.ObtemPor(Convert.ToInt32(tseUnidade.DBValue));

                    if (unidade.UnidadeCertificadoraId > 0)
                    {
                        rblTipo.SelectedValue = !unidade.Tipo.IsNullOrEmptyOrWhiteSpace() ? unidade.Tipo : string.Empty;

                        string grupoUnidadeCertificadoraId = unidade.GrupoUnidadeCertificadoraId > 0 ? unidade.GrupoUnidadeCertificadoraId.ToString() : string.Empty;
                        foreach (ListItem item in ddlGrupo.Items)
                        {
                            if (item.Value == grupoUnidadeCertificadoraId)
                            {
                                ddlGrupo.SelectedValue = grupoUnidadeCertificadoraId;
                                break;
                            }
                        }

                        txtUnidade.Text = !unidade.Descricao.IsNullOrEmptyOrWhiteSpace() ? unidade.Descricao : string.Empty;
                        txtDescricaoSite.Text = !unidade.DescricaoSite.IsNullOrEmptyOrWhiteSpace() ? unidade.DescricaoSite : string.Empty;
                        txtCEP.Text = !unidade.Cep.IsNullOrEmptyOrWhiteSpace() ? unidade.Cep : string.Empty;
                        tsCEP.DBValue = !unidade.Cep.IsNullOrEmptyOrWhiteSpace() ? unidade.Cep : string.Empty;
                        tseMunicipio.DBValue = !unidade.Municipio.IsNullOrEmptyOrWhiteSpace() ? unidade.Municipio : string.Empty;
                        txtEndereco.Text = !unidade.Endereco.IsNullOrEmptyOrWhiteSpace() ? unidade.Endereco : string.Empty;
                        txtNumero.Text = !unidade.Numero.IsNullOrEmptyOrWhiteSpace() ? unidade.Numero : string.Empty;
                        txtComplemento.Text = !unidade.Complemento.IsNullOrEmptyOrWhiteSpace() ? unidade.Complemento : string.Empty;
                        txtBairro.Text = !unidade.Bairro.IsNullOrEmptyOrWhiteSpace() ? unidade.Bairro : string.Empty;
                        if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
                        {
                            txtEstado.Text = Convert.ToString(tseMunicipio["uf_sigla"]);
                        }
                        txtFone.Text = !unidade.Telefone.IsNullOrEmptyOrWhiteSpace() ? unidade.Telefone.AplicarMascaraTelefoneComDDD() : string.Empty;

                        chkAtivo.Checked = unidade.Ativo;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaGrupo()
        {
            RN.Certificacao.GrupoUnidadeCertificadora rnGrupoUnidadeCertificadora = new Techne.Lyceum.RN.Certificacao.GrupoUnidadeCertificadora();
            try
            {
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlGrupo.Items.Clear();
                ddlGrupo.DataSource = rnGrupoUnidadeCertificadora.ListaAtivoPor();
                ddlGrupo.DataBind();
                ddlGrupo.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
