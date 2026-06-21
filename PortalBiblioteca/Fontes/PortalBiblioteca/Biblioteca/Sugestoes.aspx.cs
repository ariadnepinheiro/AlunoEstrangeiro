using System;
using System.Web;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Web;

namespace Techne.Lyceum.Net.Biblioteca
{
    [NavUrl("~/Biblioteca/Sugestoes.aspx"),
    ControlText("Sugestoes"),
    Title("Incluir Sugestões"),]

    public partial class Sugestoes : TPage
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

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            string usuario = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(usuario))
            {
                lblMensagem.Text = "É necessário entrar com usuário e senha no sistema para enviar uma sugestão.";
                return;
            }

            Ly_bib_sugestao.Row linha = new Ly_bib_sugestao().NewRow();
            linha.Titulo = txtTitulo.Text;
            linha.Autor = txtAutor.Text;
            linha.Editora = txtEditora.Text;
            if (!string.IsNullOrEmpty(txtAno.Text))
                linha.Ano = Convert.ToDecimal(txtAno.Text);
            linha.Observacoes = txtObs.Text;
            linha.Id_bib_usuario = RN.Biblioteca.ConsultarIDUsuario(usuario); ;
            linha.Data = DateTime.Today;

            RetValue retorno = null;
            retorno = RN.Biblioteca.IncluirSugestao(linha, usuario);
            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    lblMensagem.Text = retorno.Errors.ToString();
                }
                else
                {
                    lblMensagem.Text = retorno.Message;
                    LimparCampos();
                }
            }
        }

        protected void LimparCampos()
        {
            txtAutor.Text = string.Empty;
            txtTitulo.Text = string.Empty;
            txtAno.Text = string.Empty;
            txtEditora.Text = string.Empty;
            txtObs.Text = string.Empty;
        }
    }
}
