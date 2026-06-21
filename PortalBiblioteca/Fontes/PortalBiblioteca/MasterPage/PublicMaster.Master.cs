using System;
using System.Web;
using System.Web.UI;
using Techne.Lyceum.RN;

namespace Techne.Lyceum.Net.Modulos
{

    public partial class PublicMaster : System.Web.UI.MasterPage
    {
        public bool habilitaLoading = false;

        public enum paineis
        {
            inicial,
            login,
            logado
        }

        private string _biblioteca
        {
            get
            {
                if (Session["biblioteca"] != null)
                {
                    if (Session["biblioteca"] is string)
                        return Session["biblioteca"].ToString();
                }

                return string.Empty;
            }
            set { Session["biblioteca"] = value; }
        }

        private string _nomeUsu
        {
            get
            {
                if (Session["nomeUsu"] != null)
                {
                    if (Session["nomeUsu"] is string)
                        return Session["nomeUsu"].ToString();
                }

                return string.Empty;
            }
            set { Session["nomeUsu"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                pucLogin.ShowOnPageLoad = false;

            if (string.IsNullOrEmpty(txtPesquisaSimples.Text) && Session["busca"] != null)
                txtPesquisaSimples.Text = Session["busca"].ToString();

            if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                lnkSugestoes.Visible = false;
            else
                lnkSugestoes.Visible = true;

            if (this.Page is TPage)
                lblTituloPagina.Text = ((TPage)this.Page).GetPageTitle();
            else
                lblTituloPagina.Text = "";

            hlHelp.Attributes.Add("OnClick", "__Help(); return(false);");
            hlHelp.Style.Add("cursor", "pointer");

            lblVersao.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Techne.Web.TSearchBox.RegisterTSearchBoxScript(this.Page);

            VerificaLogin();

            if (habilitaLoading)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("managerMaster"))
                {
                    Page.ClientScript.RegisterStartupScript(typeof(System.Web.UI.MasterPage), "managerMaster",
                      "<script  type=\"text/javascript\" >" +
                            @"//add event handlers to the search UpdatePanel
                        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(startRequest);
                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

                        function startRequest(sender, e) {
                            pucLoading.Show();
                        }

                        function endRequest(sender, e) {
                            pucLoading.Hide();
                        }" +
                      "</script>\r\n"
                    );
                }
            }
        }

        protected void scrMng_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            scrMng.AsyncPostBackErrorMessage = e.Exception.Message;
        }

        private void VerificaLogin()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                txtUsuario.Text = HttpContext.Current.User.Identity.Name;
                lblSaudacoes.Text = "Benvindo " + _nomeUsu + ".";
                ControlaPaineis(paineis.logado);
            }
            else
            {
                lblSaudacoes.Text = "Benvindo visitante.";
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsuario.Text) || string.IsNullOrEmpty(txtSenha.Text))
            {
                lblMensagem.Text = "Preencha o usuário e a senha.";
                ControlaPaineis(paineis.login);
                return;
            }
            Autentica(txtUsuario.Text, txtSenha.Text);
        }

        private void Autentica(string usuario, string senha)
        {
            string msg = string.Empty;

            AcessoPortalBiblioteca acesso = new AcessoPortalBiblioteca(usuario, senha);
            acesso.LoginPortalBiblioteca();

            if (acesso.Valido)
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie(usuario, false);
                txtSenha.Text = "";
                ControlaPaineis(paineis.logado);
                lblSaudacoes.Text = "Benvindo " + acesso.Nome + ".";
                _biblioteca = acesso.Biblioteca.ToString();
                _nomeUsu = acesso.Nome;
                lnkSugestoes.Visible = true;
                pucLogin.ShowOnPageLoad = false;
            }
            else
            {
                pucLogin.ShowOnPageLoad = true;
                txtSenha.Focus();
                lblMensagem.Text = "Usuário e/ou senha inválidos.";
                ControlaPaineis(paineis.login);
                lnkSugestoes.Visible = false;
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon();
            ControlaPaineis(paineis.inicial);
            lnkSugestoes.Visible = false;
            lblSaudacoes.Text = "Benvindo visitante.";
            Response.Redirect("~/Biblioteca/Inicial.aspx");
        }

        private void ControlaPaineis(paineis painel)
        {
            switch (painel)
            {
                case paineis.inicial:
                    dvLogado.Style[HtmlTextWriterStyle.Visibility] = "collapse";
                    dvInicial.Style[HtmlTextWriterStyle.Visibility] = "visible";
                    break;
                case paineis.login:
                    dvLogado.Style[HtmlTextWriterStyle.Visibility] = "collapse";
                    dvInicial.Style[HtmlTextWriterStyle.Visibility] = "collapse";
                    break;
                case paineis.logado:
                    dvLogado.Style[HtmlTextWriterStyle.Visibility] = "visible";
                    dvInicial.Style[HtmlTextWriterStyle.Visibility] = "collapse";
                    break;
            }
        }


        protected void btnBuscarSimples_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Biblioteca/Pesquisa.aspx?Chave=" + txtPesquisaSimples.Text);
        }

        protected void lnkBuscaAv_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Biblioteca/PesquisaAvancada.aspx");
        }

        protected void lnkSugestoes_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Biblioteca/Sugestoes.aspx");
        }



    }
}
