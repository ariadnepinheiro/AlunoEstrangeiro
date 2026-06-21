namespace Techne.Lyceum.Net.Seguranca
{
    using System;
    using System.Reflection;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Techne.Lyceum.RN;
    using Techne.Web;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    [NavUrl("~/Seguranca/Identificacao.aspx"), Title("Login")]
    public partial class Identificacao : TPage
    {
        public static string GetUrl()
        {
            return Navigation
                .GetNavigation(MethodBase.GetCurrentMethod())
                .GetUrl(new object[]
                        {
                        });
        }

        public void Page_Load(object sender, EventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (!this.IsPostBack)
            {
                var anos = PeriodoLetivo.ConsultarAnoInf(DateTime.Now.Year.ToString());
 
                this.CarregarAnos(this.cmbAnoLetivo, anos, DateTime.Now.Year.ToString());

#if DEBUG
                //Aluno de exemplo pra debug
                //this.txtUsuario.Text = "201505170580402";
                //calendario.Text = "04/02/2003";
#endif
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
            this.DisabledNavigationKeys = NavigationKey.Backspace;
        }

        protected void btnEntrar_Click(object sender, ImageClickEventArgs e)
        {
            List<string> mensagens = VerificaPreenchimento();

            if (mensagens.Count == 0)
            {
                this.Autentica();
            }
            else
            {
                this.lblMensagem.Text = mensagens.Aggregate((x, y) => x + "<br />" + y);
                this.txtChave.Text = string.Empty;
            }
        }

        public List<string> VerificaPreenchimento()
        {
            List<string> mensagens = new List<string>();

            if (this.cmbAnoLetivo.SelectedValue == string.Empty)
            {
                mensagens.Add("O ANO é de preenchimento obrigatório.");
            }

            if (this.cmbPeriodoLetivo.SelectedValue == string.Empty)
            {
                mensagens.Add("O PERIODO é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(this.txtUsuario.Text))
            {
                mensagens.Add("A MATRICULA é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(calendario.Text))
            {
                mensagens.Add("A DATA DE NASCIMENTO é de preenchimento obrigatório.");
            }

            if (string.IsNullOrEmpty(txtChave.Text))
            {
                mensagens.Add("O CÓDIGO DA IMAGEM é de preenchimento obrigatório.");
            }
            else
            {
                string captchaGerado = string.Empty;
                if (HttpContext.Current.Response.Cookies["CaptchaValue"] != null)
                {
                    captchaGerado = HttpContext.Current.Request.Cookies["CaptchaValue"].Value;
                }

                // Valida Captcha
                if (this.txtChave.Text != captchaGerado)
                {
                    this.txtChave.Text = string.Empty;
                    mensagens.Add("Código digitado incorreto. Digite-o novamente.");
                }
            }

            return mensagens;
        }

        private void Autentica()
        {
            Matricula matricula = new Matricula();
            HistMatricula rnHistMatricula = new HistMatricula();
            bool historico = false;
            bool ativa = false;

            try
            {
                // Verifica se existe matrícula
                var alunoAutenticado = Aluno.Autenticar(this.txtUsuario.Text);

                if (alunoAutenticado == null)
                {
                    this.lblMensagem.Text = "Matrícula inválida.";
                    this.txtChave.Text = string.Empty;
                    return;
                }

                var dtNascimento = Convert.ToDateTime(alunoAutenticado.DataNascimento);
                var dtNascimentoLogin = Convert.ToDateTime(calendario.Text);

                //verifica se a consulta será para a matricula atual
                ativa = matricula.EhMatriculaAtiva(this.txtUsuario.Text, Convert.ToInt32(this.cmbAnoLetivo.Text), Convert.ToInt32(this.cmbPeriodoLetivo.SelectedValue));

                if (!ativa)
                {
                    //verifica se a consulta será para historico
                    historico = rnHistMatricula.EhMatriculaHistoricoAtiva(this.txtUsuario.Text, Convert.ToInt32(this.cmbAnoLetivo.Text), Convert.ToInt32(this.cmbPeriodoLetivo.SelectedValue));
                }
                // Valida data de nascimento com matrícula
                if (!alunoAutenticado.DataNascimento.HasValue
                    || dtNascimento.Date != dtNascimentoLogin.Date)
                {
                    this.lblMensagem.Text = "Matrícula e data de nascimento não conferem.";
                    this.txtChave.Text = string.Empty;

                    return;
                }

                // condição para verirficar se o que foi digitado é realmente o que esta guardado na variável de Sessão
                this.Session["AnoLetivo"] = this.cmbAnoLetivo.Text;
                this.Session["PeriodoLetivo"] = this.cmbPeriodoLetivo.SelectedValue;
                this.Session["AlunoAutenticado"] = alunoAutenticado;
                this.Session["MatriculaAtiva"] = ativa;
                this.Session["MatriculaHistorico"] = historico;

                AcessoUsuario.AtualizaUltimoAcessoAlunoOnline("AOL", alunoAutenticado.Matricula);

                FormsAuthentication.SetAuthCookie(this.txtUsuario.Text, false);

                this.Response.Redirect("~/Academico/Boletim.aspx");
                //Response.Redirect("~/Menu/Config.aspx"); //Abrir popup
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarAnos(DropDownList drop, object data, string defaultValue)
        {
            drop.Items.Clear();
            drop.DataSource = data;
            drop.DataBind();

            var itemVazio = new ListItem("<Nenhum>", string.Empty);

            drop.Items.Insert(0, itemVazio);
            drop.SelectedValue = string.Empty;

            if (!string.IsNullOrEmpty(defaultValue)
                && drop.Items.FindByValue(defaultValue) != null)
            {
                drop.SelectedValue = defaultValue;
            }
        }

        private void InitializeComponent()
        {
            txtUsuario.Focus();
        }
    }
}