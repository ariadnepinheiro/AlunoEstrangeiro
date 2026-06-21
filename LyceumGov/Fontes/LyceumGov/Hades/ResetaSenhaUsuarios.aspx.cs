using System;
using System.Configuration;
using Techne.Lyceum.RN;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Hades
{
    [NavUrl("~/Hades/ResetaSenhaUsuarios.aspx"),
     ControlText("ResetaSenhaUsuario"),
     Title("Reseta Senha Portais"),]
    public partial class ResetaSenhaUsuarios : TPage
    {
        #region Código gerado Techne

        public static string GetUrl()
        {
            return
                Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(
                    new object[] { });
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnResetarDocente, AcaoControle.novo);
        }

        private void VisibilidadeDadosDocente(bool visivel)
        {
            trNomeDocente.Visible = visivel;
            trCPFDocente.Visible = visivel;
            trEmailDocente.Visible = visivel;
            trUltimoAcesso.Visible = visivel;
            trUltimoReset.Visible = visivel;
        }

        private void LimparDadosDocente()
        {
            lNomeDocente.Text = string.Empty;
            lblCPFDocente.Text = string.Empty;
            lblEmailDocente.Text = string.Empty;
            lblUltimoAcesso.Text = string.Empty;
            lblUltimoReset.Text = string.Empty;
        }

        protected void tseDocente_Changed(object sender, EventArgs e)
        {
            try
            {

                this.LimparDadosDocente();
                this.VisibilidadeDadosDocente(false);

                if (tseDocente.IsValidDBValue && !tseDocente.DBValue.IsNull)
                {
                    if (!tseDocente["idfuncional"].ToString().IsNullOrEmptyOrWhiteSpace() && !tseDocente["idvinculo"].ToString().Contains("/"))
                    {
                        lblMensagem.Text = "Este docente não possui Id Funcional e/ou Vinculo, com isso não é possivel utilizar o Docente Online.";
                        //lblMensagem.Text = "Formato incorreto. Para pesquisar um docente é necessário informar Id/Vinculo.";
                        return;
                    }

                    lNomeDocente.Text = tseDocente["nome"].ToString();
                    lblCPFDocente.Text = Convert.ToString(tseDocente["cpf"]);
                    lblEmailDocente.Text = Convert.ToString(tseDocente["email"]);

                    var ultimosDados =
                        RN.AcessoUsuario.CarregarDadosUltimoResetAcesso(Convert.ToString(tseDocente["matricula"]));

                    if (ultimosDados.Rows.Count != 0)
                    {
                        lblUltimoAcesso.Text = ultimosDados.Rows[0]["ULTIMO_ACESSO"].ToString();
                        lblUltimoReset.Text = ultimosDados.Rows[0]["ULTIMO_RESET"].ToString();
                    }

                    this.VisibilidadeDadosDocente(true);

                    if (!btnResetarDocente.Visible)
                    {
                        lblMensagem.Text = "Usuário sem permissão para efetuar de reset senha dos portais.";
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        private bool ValidarResetDocente()
        {
            if (!tseDocente.IsValidDBValue && !tseDocente.DBValue.IsNull)
            {
                lblMensagem.Text = "Selecione um docente válido.";
                return false;
            }
            if (string.IsNullOrEmpty(lblCPFDocente.Text.Trim()))
            {
                lblMensagem.Text = "Não foi encontrado cpf para este docente.";
                return false;
            }
            return true;
        }

        protected void btnResetarDocente_Click(object sender, EventArgs e)
        {
            if (!this.ValidarResetDocente())
            {
                return;
            }
            try
            {
                var enviarMail = Convert.ToBoolean(ConfigurationManager.AppSettings["EnviarMail"] ?? "false");
                var senha = lblCPFDocente.Text.Trim();

                if (enviarMail)
                {
                    senha = Senha.GerarSenhaNumerica(999999);
                }

                var num_func = Convert.ToString(tseDocente["num_func"]);
                var emailDocente = lblEmailDocente.Text.Trim();

                RN.AcessoUsuario.ResetarSenhaDocente(senha, num_func, tseDocente["matricula"].ToString());

                if (!enviarMail)
                {
                    lblMensagem.Text = "Senha Alterada com sucesso.<br />A nova senha do docente é: " + senha + ".";
                    return;
                }

                if (!string.IsNullOrEmpty(emailDocente) && emailDocente.Split('@')[1].Trim() == "prof.educacao.rj.gov.br")
                {
                    var email = RN.Util.Email.MontarEmailReset(tseDocente["matricula"].ToString(), senha, emailDocente);
#if DEBUG
                    lblMensagem.Text = "Email não é enviado no debug.<br />Senha Alterada, nova senha: " + senha;
#else
                if (RN.Util.Email.EnviarMail(email))
                {
                    lblMensagem.Text = "Senha Alterada com sucesso.<br />A nova senha do docente é: " + senha + "<br />A nova senha foi enviada para o e-mail institucional do docente.";
                }
#endif
                }
                else
                {
                    lblMensagem.Text = "Senha Alterada com sucesso.<br />A nova senha do docente é: " + senha + "<br />A senha não foi envida por e-mail, pois apenas são permitidos e-mail @prof.educacao.rj.gov.br.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
