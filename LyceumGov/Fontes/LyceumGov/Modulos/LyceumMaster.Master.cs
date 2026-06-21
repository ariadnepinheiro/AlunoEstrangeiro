namespace Techne.Lyceum.Net.Modulos
{
    using System;
    using System.Configuration;
    using System.Web.UI;
    using Techne.Lyceum.RN;
    using System.Web.Security;

    public partial class LyceumMaster : MasterPage
    {
        public bool habilitaLoading = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Usuarios rnUsuarios = new Usuarios();
            hlHelp.Attributes.Add("OnClick", "__Help(); return(false);");
            hlHelp.Style.Add("cursor", "pointer");            

            if (this.Page is TPage)
            {
                lblTituloPagina.Text = ((TPage)this.Page).GetPageTitle();
            }
            else
            {
                lblTituloPagina.Text = string.Empty;
            }

            var email = string.Empty;

            if (Session["email"] != null
                && !string.IsNullOrEmpty(Session["email"].ToString()))
            {
                email = Session["email"].ToString();
            }

            var versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var sufixo = ConfigurationManager.AppSettings["VersaoSufixo"] ?? string.Empty;
            var usuario = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            var saudacao = string.Format(
                "{0} (IP: {1})",
                usuario,
                Sistema.ObterIP());

            lblSaudacoes.Text = saudacao;
            lblEmail.Text = email;
            lblVersao.Text = versao + sufixo;

            //Verifica se o usuario tem acesso ao rve
            if (rnUsuarios.EhPrivilegiado(usuario) || rnUsuarios.PossuiPermissaoPaginaPor(usuario, "Techne.Lyceum.Net.Menu.RVE"))
            {
                hlRVE.Visible = true;
            }
            else
            {
                hlRVE.Visible = false;
            }

            Techne.Web.TSearchBox.RegisterTSearchBoxScript(this.Page);

            #region  Validação


            if (this.habilitaLoading)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("managerMaster"))
                {
                    Page.ClientScript.RegisterStartupScript(typeof(MasterPage), "managerMaster",
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

            #endregion

            lnkFacebook.Attributes["href"] = ConfigurationManager.AppSettings["linkfacebook"];
            lnkInstagram.Attributes["href"] = ConfigurationManager.AppSettings["linkinstagram"];
            lnkYoutube.Attributes["href"] = ConfigurationManager.AppSettings["linkyoutube"];

        }

        //[Obsolete()]
        protected void scrMng_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            scrMng.AsyncPostBackErrorMessage = e.Exception.Message;
        }

        protected void lbSair_Click(object sender, EventArgs e)
        {
            Session.Abandon();

            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();

            this.Response.End();
        }
    }
}