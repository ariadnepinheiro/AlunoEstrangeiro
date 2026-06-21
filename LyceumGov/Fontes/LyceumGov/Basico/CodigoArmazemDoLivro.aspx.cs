using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN;
using System.Data;

namespace Techne.Lyceum.Net.Basico
{
    [
        NavUrl("~/Basico/CodigoArmazemDoLivro.aspx"),
        ControlText("Voucher - Armazém do Livro"),
        Title("Voucher - Armazém do Livro"),
    ]
    public partial class CodigoArmazemDoLivro : TPage
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

        protected string CodigoAcesso 
        {
            get
            {
                return ViewState["CodigoAcesso"] as string;
            }
            set
            {
                ViewState["CodigoAcesso"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnConfirmar, AcaoControle.editar);
        }

        protected void tseUsuario_Changed(object sender, EventArgs args)
        {
            CodigoAcesso = string.Empty;

            if (tseUsuario.IsValidDBValue && !tseUsuario.DBValue.IsNull)
            {
                RN.Usuarios rnUsuarios = new Techne.Lyceum.RN.Usuarios();
                RN.ArmazemLivro rnArmazemLivro = new Techne.Lyceum.RN.ArmazemLivro();
                txtEmail.Text = string.Empty;

                if (rnArmazemLivro.EhDiretor(tseUsuario.DBValue.ToString()))
                {
                    var cpfUsuarioLogado = rnUsuarios.ObtemCpfPor(User.Identity.Name);

                    if (cpfUsuarioLogado == tseUsuario.DBValue.ToString())
                    {
                        var rows = rnArmazemLivro.ObtemCodigosPor(cpfUsuarioLogado);

                        string codigos = string.Empty;
                        foreach (DataRow row in rows.Rows)
                        {
                            codigos += (!string.IsNullOrEmpty(codigos) ? "<br />" : "");
                            codigos += row["CATEGORIA"].ToString().Replace("SERVIDOR", "PROFESSOR") + " - " + row["CODIGO_ACESSO"].ToString();
                        }

                        CodigoAcesso = codigos;
                        pnCodigo.Visible = true;
                    }
                }

                pnEmail.Visible = true;
            }
            else
            {
                pnCodigo.Visible = false;
                pnEmail.Visible = false;
                txtEmail.Text = "";
            }
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.ArmazemLivro rnArmazemLivro = new Techne.Lyceum.RN.ArmazemLivro();

                if (!RN.Validacao.ValidaEmail(txtEmail.Text))
                {
                    lblMensagem.Text = "E-Mail inválido.";
                    return;
                }

                rnArmazemLivro.EnviaCodigo(tseUsuario.DBValue.ToString(), txtEmail.Text, User.Identity.Name);

                lblMensagem.Text = "Voucher enviado para o E-Mail com sucesso.";
                txtEmail.Text = "";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
