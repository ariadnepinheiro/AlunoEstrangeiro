using System;
using System.Web;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;
using Techne.Lyceum.RN;
using System.Web.UI;
using Techne.Lyceum.RN.Util;
using System.Data;

namespace Techne.Lyceum.Net.Academico
{
    [
     NavUrl("~/Academico/ListarNotificacaoControle.aspx"),
      ControlText("ListarNotificacaoControle"),
      Title("Notificação Controle de Aluno"),
    ]

    public partial class ListarNotificacaoControle : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
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

        public object Listar(object alunoFiltro)
        {
            RN.Turmas.Notificacao rnNotificacao = new Techne.Lyceum.RN.Turmas.Notificacao();

            var aluno = alunoFiltro != null ? alunoFiltro.ToString() : null;

            if (!aluno.IsNullOrEmptyOrWhiteSpace())
            {
                return rnNotificacao.ListaPor(aluno);

            }
            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAlunos, string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    hdnAluno.Value = string.Empty;
                    btnNovo.Visible = false;

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        if (Request.QueryString["Chave"] != null)
                        {

                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                            ObterDadosQueryString(decodedText);
                            tseAluno.DBValue = hdnAluno.Value;
                            tseAluno_Changed(sender, e);

                        }

                        if (Request.QueryString["Aluno"] != null)
                        {
                            hdnAluno.Value = Request.QueryString["Aluno"].ToString();

                            tseAluno.DBValue = hdnAluno.Value;
                            tseAluno_Changed(sender, e);

                            btnNovo.Visible = true;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAlunos);

        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                btnNovo.Visible = false;

                if (!tseAluno.DBValue.IsNull)
                {
                    if (!tseAluno.IsValidDBValue)
                    {
                        if (hdnAluno.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        }
                    }
                    else
                    {
                        btnNovo.Visible = true;
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAlunos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAlunos);
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdAlunos.PageIndex * grdAlunos.SettingsPager.PageSize;
            for (int i = 0; i < grdAlunos.VisibleRowCount; i++)
            {
                if (grdAlunos.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    return startIndexOnPage + i;
                }
            }
            return -1;
        }

        private void RedirecionarPagina()
        {
            HttpContext.Current.Items.Add("chave", "1000");

            Server.Transfer("NotificacaoControle.aspx");
        }

        private string MontarQueryString(string Id)
        {
            string queryString = string.Empty;

            queryString += "Id=" + Id;
            queryString += "&aluno=" + tseAluno.DBValue.ToString();

            return queryString;
        }

        private string ObterDadosQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');
            string aluno = null;
            string mensagem = null;

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("aluno=") >= 0)
                    hdnAluno.Value = dados.Substring(dados.LastIndexOf('=') + 1);
            }

            lblMensagem.Text = mensagem;

            return aluno;
        }


        protected void grdAlunos_SelectionChanged(object sender, EventArgs e)
        {
            string tipoOperacao = string.Empty;

            int curPageSelection = GetSelectedRowOnTheCurrentPage();

            string Id = Convert.ToString(grdAlunos.GetRowValues(curPageSelection, "NOTIFICACAOID"));

            string queryString = MontarQueryString(Id);

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);


            ASPxWebControl.RedirectOnCallback("NotificacaoControle.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.Turmas.Notificacao rnNotificacao = new Techne.Lyceum.RN.Turmas.Notificacao();
                DateTime? data = new DateTime();
                int faltas = 0;

                faltas = rnNotificacao.ObtemQuantidadeFaltasPor(tseAluno.DBValue.ToString(), out data);
                string situacao = Convert.ToString(tseAluno["SIT_ALUNO"]);

                if (situacao.ToUpper() == "ATIVO")
                {
                    if (faltas > 0)
                    {
                        string Id = string.Empty;
                        string aluno = tseAluno.DBValue.ToString();

                        string queryString = MontarQueryString(Id);

                        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                        Response.Redirect("NotificacaoControle.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                    }
                    else
                    {
                        lblMensagem.Text = "Este aluno não possui ao menos 3 faltas consecutivas lançadas no Diário On Line.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Este aluno não está Ativo.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    }
}
