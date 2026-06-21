using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DevExpress.Web.ASPxEditors;
using Techne.Controls;
using DevExpress.Web.ASPxClasses;
using System.Text;
using System.Configuration;
using System.Reflection;


namespace Techne.Lyceum.Net.Basico
{

    [NavUrl("~/Basico/LoginRedeEmail.aspx")]
    [ControlText("Login Rede Email")]
    [Title("Login Rede Email")]

    public partial class LoginRedeEmail : TPage
    {
  
        public enum TipoOperacao
        {

            Alterar,
            ConsultarPessoa,
            Inicial,
            Sucesso
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }

        public static string GetUrl()
        {
            return Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
        }

        #region Web Form Designer generated code
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion

        public object ListaLotacao(object pessoa)
        {
            RN.Lotacao rnLotacao = new Lotacao();

            string codpessoa = pessoa != null ? pessoa.ToString() : null;

            if (!string.IsNullOrEmpty(codpessoa))
            {
                return rnLotacao.ListaLotacaoAtivaPor(Convert.ToInt32(codpessoa));
            }
            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdLotacao, "Lotação");
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }

                lblMensagem.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void LimparTela()
        {
            hdnPessoa.Value = string.Empty;
            txtNomeComplPessoa.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtIDFncional.Text = string.Empty;
            txtEmailGoogle.Text = string.Empty;
            txtEmailOffice.Text = string.Empty;
            txtEmailAlternativo.Text = string.Empty;
            txtLoginRede.Text = string.Empty;

        }

        protected void HabilitaCampos()
        {
            txtEmailGoogle.Enabled = true;
            txtEmailAlternativo.Enabled = true;
            txtEmailOffice.Enabled = true;
            txtLoginRede.Enabled = true;        
        }

        protected void DesabilitaCampos()
        {
            txtEmailGoogle.Enabled = false;
            txtEmailOffice.Enabled = false;
            txtLoginRede.Enabled = false;
            txtEmailAlternativo.Enabled = false;
        }


        protected void tseUsuario_Changed(object sender, EventArgs args)
        {
            try
            {
                LimparTela();
                if (!string.IsNullOrEmpty(tseUsuario.DBValue.ToString()))
                {
                    if (tseUsuario.IsValidDBValue)
                    {
                        _tipoOperacao = TipoOperacao.ConsultarPessoa;
                        ControlarTipoOperacao();

                    }
                    else
                    {
                        lblMensagem.Text = "Usuário não cadastrado.";
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();

                    }
                }
                else
                {
                    lblMensagem.Text = "Usuário não cadastrado.";
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUsuario_Load(object sender, EventArgs e)
        {

        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
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
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(controles);
                        LimparTela();
                        pnlGeral.Visible = false;
                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnEditar,btnCancel };
                        ControlarVisibilidadeControle(controles);

                        DesabilitaCampos();

                        break;
                    }


                case TipoOperacao.Alterar:
                    {
                        var controles = new[] { btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);


                        HabilitaCampos();

                        break;
                    }

                case TipoOperacao.ConsultarPessoa:
                    {
                        ImageButton[] controles = new ImageButton[] { btnEditar,btnCancel };
                        ControlarVisibilidadeControle(controles);
                        LimparTela();
                        pnlGeral.Visible = false;
                        RN.DTOs.DadosLoginRedeEmail dados = new Techne.Lyceum.RN.DTOs.DadosLoginRedeEmail();
                        RN.RecursosHumanos.LoginRede rnLogin = new Techne.Lyceum.RN.RecursosHumanos.LoginRede();


                        dados = rnLogin.ObtemDadosLoginRedeEmailPor(Convert.ToInt32(tseUsuario["pessoa"]));

                        if (!dados.Nome.IsNullOrEmptyOrWhiteSpace())
                        {
                            hdnPessoa.Value = dados.Pessoa.ToString();
                            txtNomeComplPessoa.Text = !dados.Nome.IsNullOrEmptyOrWhiteSpace() ? dados.Nome.Trim().ToUpper() : string.Empty;
                            txtCPF.Text = !dados.Cpf.IsNullOrEmptyOrWhiteSpace() ? dados.Cpf.AplicarMascaraCPF() : string.Empty;
                            txtIDFncional.Text = !dados.IdFuncional.IsNullOrEmptyOrWhiteSpace() ? dados.IdFuncional.ToString() : string.Empty;
                            txtEmailOffice.Text = !dados.EmailOffice365.IsNullOrEmptyOrWhiteSpace() ? dados.EmailOffice365.Trim() : string.Empty;
                            txtEmailGoogle.Text = !dados.EmailGoogleEducation.IsNullOrEmptyOrWhiteSpace() ? dados.EmailGoogleEducation.Trim() : string.Empty;
                            txtEmailAlternativo.Text = !dados.EmailAlternativo.IsNullOrEmptyOrWhiteSpace() ? dados.EmailAlternativo.Trim() : string.Empty;
                            txtLoginRede.Text = !dados.LoginRede.IsNullOrEmptyOrWhiteSpace() ? dados.LoginRede.Trim() : string.Empty;

                            DesabilitaCampos();
                            pnlGeral.Visible = true;
                           
                        }
                        else
                        {
                            lblMensagem.Text = "Usuário não identificado.";
                        }
                        break;
                    }
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
            tseUsuario.ResetValue();
           
            _tipoOperacao = TipoOperacao.Inicial;
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
                RN.DTOs.DadosLoginRedeEmail dados = new Techne.Lyceum.RN.DTOs.DadosLoginRedeEmail();
                RN.RecursosHumanos.LoginRede rnLoginRede = new Techne.Lyceum.RN.RecursosHumanos.LoginRede();

                dados.Pessoa = !hdnPessoa.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(hdnPessoa.Value) : -1;
                dados.Cpf = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.RetirarMascaraCPF() : null;
                dados.Nome = !txtNomeComplPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeComplPessoa.Text.Trim().ToUpper() : null;
                dados.IdFuncional = !txtIDFncional.Text.IsNullOrEmptyOrWhiteSpace() ? txtIDFncional.Text.Trim() : null;
                dados.EmailGoogleEducation = !txtEmailGoogle.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailGoogle.Text.Trim() : null;
                dados.EmailOffice365 = !txtEmailOffice.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailOffice.Text.Trim() : null;
                dados.EmailAlternativo = !txtEmailAlternativo.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmailAlternativo.Text.Trim() : null;
                dados.LoginRede = !txtLoginRede.Text.IsNullOrEmptyOrWhiteSpace() ? txtLoginRede.Text.Trim() : null;
                dados.UsuarioId = User.Identity.Name;

                validacao = rnLoginRede.ValidaDadosLoginRedeEmail(dados);

                if (validacao.Valido)
                {
                    rnLoginRede.SalvaDadosLoginRedeEmail(dados);
                    lblMensagem.Text = "Dados de acesso alterados com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

                _tipoOperacao = TipoOperacao.Sucesso;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    }
}
