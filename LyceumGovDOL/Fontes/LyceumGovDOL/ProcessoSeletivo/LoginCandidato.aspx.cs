using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.UI;
using Techne.Lyceum.RN;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [
    NavUrl("~/Seguranca/Login.aspx"),
    Title("Login")
    ]
    public partial class LoginCandidato : TPage
    {
        protected System.Web.UI.WebControls.Label PageUser;
        protected System.Web.UI.WebControls.Label PageDate;

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Navigation.GetNavigation(MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void bt_Entrar_Click(object sender, ImageClickEventArgs e)
        {
            Autentica(cUsuario.Text, cSenha.Text);
        }

        private void SetPanelLoginVisibility(bool visible)
        {
            PanelLogin.Visible = visible;
            PanelPassword.Visible = !visible;
        }

        private void Autentica(string usuario, string senha)
        {
            string msg = string.Empty;

            AcessoUsuario acesso = new AcessoUsuario(usuario, senha);
            acesso.LoginProcessoSeletivo();
                        
            if (acesso.Valido)
            {
                if (acesso.AlterarSenha || string.IsNullOrEmpty(senha))
                {
                    cMsg.Text = string.Empty;
                    ViewState.Add("USUARIO.ID", usuario);
                    ViewState.Add("SENHA", senha);
                    lblUsuarioSenha.Text = usuario;
                    SetPanelLoginVisibility(false);
                    lblMsgAlteracao.Text = msg + "<br>Digite uma nova senha.";
                }
                else
                {
                    var dict = new NameValueCollection(1);

                    dict.Add("usuario", usuario);
                    Response.Redirect(String.Format("~/ProcessoSeletivo/CandidatoDocenteFicha.aspx?{0}", CodificaQueryString(dict)));
                }
            }
            else
            {
                cSenha.Focus();
                cMsg.Text = "A matrícula e a senha não se correspondem.";
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
            this.DisabledNavigationKeys = NavigationKey.Backspace | NavigationKey.Enter;
        }

        private void InitializeComponent()
        {
            cUsuario.Focus();
        }
        #endregion

        protected void ib_ok_Click(object sender, ImageClickEventArgs e)
        {
            if (ViewState["USUARIO.ID"] == null)
                SetPanelLoginVisibility(true);

            else if (txtSenhaNova1.Text != txtSenhaNova2.Text)
                lblMsgAlteracao.Text = "As senhas digitadas não conferem";
            else if (string.IsNullOrEmpty(txtSenhaNova1.Text) || txtSenhaNova1.Text.Length < 6)
                lblMsgAlteracao.Text = "Insira uma senha de, no mínimo, 6 caracteres";
            else if (ViewState["SENHA"].ToString() != txtSenhaAtual.Text)
                lblMsgAlteracao.Text = "A senha atual não está correta.";
            else
            {
                var acesso = new AcessoUsuario(ViewState["USUARIO.ID"].ToString(), cSenha.Text, txtSenhaNova1.Text);

                acesso.AlteraSenhaDocente();
                Autentica(cUsuario.Text, txtSenhaNova1.Text);
            }
        }

        protected void ib_cancelar_Click(object sender, ImageClickEventArgs e)
        {
            SetPanelLoginVisibility(true);
        }
    }
}