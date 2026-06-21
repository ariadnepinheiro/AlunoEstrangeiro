namespace Techne.Lyceum.Net.Modulos
{
    using System;
    using System.Configuration;
    using System.Web.Security;
    using System.Web.UI;

    public partial class LyceumMaster : MasterPage
    {
        private const string Boletim = "~/Academico/Boletim.aspx";
        private const string Cartao = "~/Academico/ConsultaCartao.aspx";
        private const string GoogleEducation = "~/Academico/GoogleEducation.aspx";
        private const string AcompanhamentoClassroom = "~/Academico/AcompanhamentoClassroom.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Inserido por Felipe Ribeiro Gomes em 21/06/2022.
            //Esta linha faz com que todos os blocos de Bind <%# ... %> sejam processados no Header da página.
            //O tipo de bloco <%= ... %> foi preterido porque estava disparando exception quando usado em conjunto com CSS.
            Page.Header.DataBind();

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

            var versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var sufixo = ConfigurationManager.AppSettings["VersaoSufixo"] ?? string.Empty;
            var saudacao = string.Format(
                "Usuário: {0} (IP: {1})",
                 System.Threading.Thread.CurrentPrincipal.Identity.Name,
                 Request.UserHostAddress);

            //lblSaudacoes.Text = saudacao;
            lblUsuario.Text = string.Format(
                "{0} (IP: {1})",
                 System.Threading.Thread.CurrentPrincipal.Identity.Name,
                 Request.UserHostAddress);

            lblVersao.Text = versao + sufixo;

            Techne.Web.TSearchBox.RegisterTSearchBoxScript(this.Page);
        }

        protected void lnk1_Click(object sender, EventArgs e)
        {
            Response.Redirect(Boletim);
        }

        protected void lnk10_Click(object sender, EventArgs e)
        {
            Response.Redirect(Cartao);
        }

        protected void lnk2_Click(object sender, EventArgs e)
        {
            Response.Redirect(GoogleEducation);
        }

        protected void lnk3_Click(object sender, EventArgs e)
        {
            Response.Redirect(AcompanhamentoClassroom);
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