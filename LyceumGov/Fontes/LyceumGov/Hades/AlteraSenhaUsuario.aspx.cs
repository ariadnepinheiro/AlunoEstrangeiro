using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN;

namespace Techne.Lyceum.Net.Hades
{
    [
        NavUrl("~/Hades/AlteraSenhaUsuario.aspx"),
        ControlText("Alteração de Senha do Usuário"),
        Title("Alteração de Senha do Usuário"),
    ]
    public partial class AlteraSenhaUsuario : TPage
    {
        #region Código gerado Techne
        public static string GetUrl()
        {

            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });

        }
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
        }

        protected void tseUsuario_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            pnSenha.Visible = false;

            if (tseUsuario.IsValidDBValue && !tseUsuario.DBValue.IsNull)
            {
                pnSenha.Visible = true;

                lblUsuario.Text = Convert.ToString(tseUsuario.DBValue);
                lblNome.Text = Convert.ToString(tseUsuario["nomeusuario"]);
                lblCPF.Text = Convert.ToString(tseUsuario["cpf"]);
                lblEmail.Text = Convert.ToString(tseUsuario["e_mail_interno"]);

                var ultimosDados = RN.AcessoUsuario.CarregarDadosUltimoResetAcesso(Convert.ToString(tseUsuario.DBValue));
                if (ultimosDados.Rows.Count != 0)
                {
                    lblUltimoAcesso.Text = ultimosDados.Rows[0]["ULTIMO_ACESSO"].ToString();
                    lblUltimoReset.Text = ultimosDados.Rows[0]["ULTIMO_RESET"].ToString();
                }
            }
            else
            {                
                lblUsuario.Text = string.Empty;
                lblNome.Text = string.Empty;
                lblCPF.Text = string.Empty;
                lblEmail.Text = string.Empty;
                lblUltimoAcesso.Text = string.Empty;
                lblUltimoReset.Text = string.Empty;
                txtSenha.Text = string.Empty;
                txtConfirmarSenha.Text = string.Empty;
            }
        }

        protected void btnResetarCpf_Click(object sender, EventArgs e)
        {
            try
            {
                //RetValue retorno = null;
                RN.Usuarios rnUsuarios = new Techne.Lyceum.RN.Usuarios();

                if (lblCPF.Text == string.Empty)
                {
                    lblMensagem.Text = "Este usuário não possui CPF, favor digitar uma senha e acionar o botao Resetar Senha.";
                    return;
                }

                rnUsuarios.AlteraSenhaUsuario(tseUsuario.DBValue.ToString(), lblCPF.Text);
                lblMensagem.Text = "Senha Alterada com sucesso.<br />A nova senha é: " + lblCPF.Text + ".";
                lblUltimoReset.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnResetar_Click(object sender, EventArgs e)
        {
            try
            {
                //RetValue retorno = null;
                RN.Usuarios rnUsuarios = new Techne.Lyceum.RN.Usuarios();

                if (txtSenha.Text != txtConfirmarSenha.Text)
                {
                    lblMensagem.Text = "A senha precisa ser igual nos campos senha e confirmar senha.";
                    return;
                }
                if (txtSenha.Text.Length < 6)
                {
                    lblMensagem.Text = "Favor inserir uma senha de, no mínimo, 6 dígitos.";
                    return;
                }

                rnUsuarios.AlteraSenhaUsuario(tseUsuario.DBValue.ToString(), txtSenha.Text);
                lblMensagem.Text = "Senha Alterada com sucesso.<br />A nova senha é: " + txtSenha.Text + ".";
                lblUltimoReset.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
