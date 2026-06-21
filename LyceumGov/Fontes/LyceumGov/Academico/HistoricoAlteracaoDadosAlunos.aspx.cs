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
     NavUrl("~/Academico/HistoricoAlteracaoDadosAlunos.aspx"),
      ControlText("HistoricoAlteracaoDadosAlunos"),
      Title("Histórico de Alteração dos Dados do Alunos"),
    ]

    public partial class HistoricoAlteracaoDadosAlunos : TPage
    {
        #region Propriedades e Enumeradores

        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            Retorno
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion

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

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdEncerramentos, "Histórico de Alteração dos Dados Cadastrais do Aluno ");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        _tipoOperacao = TipoOperacao.Retorno;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        lblMensagem.Text = string.Empty;
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();
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
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        _tipoOperacao = TipoOperacao.Consultar;
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        _tipoOperacao = TipoOperacao.Inicial;
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }



        protected void grdEncerramentos_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string curso = Convert.ToString(e.GetListSourceFieldValue("curso"));
                string turno = Convert.ToString(e.GetListSourceFieldValue("turno"));
                string curriculo = Convert.ToString(e.GetListSourceFieldValue("curriculo"));
                string aluno = Convert.ToString(e.GetListSourceFieldValue("aluno"));
                string dt_encerramento = Convert.ToString(e.GetListSourceFieldValue("dt_encerramento"));

                e.Value = curso + "|" + turno + "|" + curriculo + "|" + aluno + "|" + dt_encerramento;
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdEncerramentos.PageIndex * grdEncerramentos.SettingsPager.PageSize;
            for (int i = 0; i < grdEncerramentos.VisibleRowCount; i++)
            {
                if (grdEncerramentos.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    return startIndexOnPage + i;
                }
            }
            return -1;
        }

        private void RedirecionarPagina()
        {
            HttpContext.Current.Items.Add("chave", "1000");

            Server.Transfer("EncerramentoAluno.aspx");
        }



        private string MontarQueryStringAluno(string tipoOperacao, Ly_aluno.Row dadosAluno)
        {
            string queryString = string.Empty;

            queryString += "Operacao=" + tipoOperacao;
            queryString += "&aluno=" + dadosAluno.Aluno;
            queryString += "&ano_ingresso=" + dadosAluno.Ano_ingresso;
            queryString += "&periodo_ingresso=" + dadosAluno.Sem_ingresso;
            queryString += "&pessoa=" + dadosAluno.Pessoa;

            return queryString;
        }

        protected void grdEncerramentos_PageIndexChanged(object sender, EventArgs e)
        {
            CarregarDadosGrid("grdEncerramentos", tseAluno.DBValue.ToString());
        }


        private string ObterDadosQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');
            string aluno = null;
            string mensagem = null;
            string compartilhada = string.Empty;
            string ano = null;
            string periodo = null;
            string curso = null;
            string serie = null;
            hdnCompartilhada.Value = string.Empty;

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("aluno=") >= 0)
                    aluno = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("comp=") >= 0)
                    compartilhada = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("ano=") >= 0)
                    ano = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("periodo=") >= 0)
                    periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("curso=") >= 0)
                    curso = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("serie=") >= 0)
                    serie = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("mensagem=") >= 0)
                    mensagem = dados.Substring(dados.LastIndexOf('=') + 1);
            }

            lblMensagem.Text = mensagem;

            if (!compartilhada.IsNullOrEmptyOrWhiteSpace())
            {
                hdnCompartilhada.Value = compartilhada;
                hdnAno.Value = ano;
                hdnPeriodo.Value = periodo;
                hdnCurso.Value = curso;
                hdnSerie.Value = serie;
            }

            return aluno;
        }

        private DataTable CarregarDadosGrid(string idGrid, string aluno)
        {
            RN.RecursosHumanos.HistoricoAlteracaoAluno rnHistoricoAlteracaoAluno = new Techne.Lyceum.RN.RecursosHumanos.HistoricoAlteracaoAluno();
            DataTable dadosGrid = null;

            dadosGrid = rnHistoricoAlteracaoAluno.ListaPor(aluno);


            grdEncerramentos.DataSource = dadosGrid;
            grdEncerramentos.DataBind();

            return dadosGrid;
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        pnDados.Visible = false;
                        grdEncerramentos.Visible = false;
                        tseAluno.ResetValue();

                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        lblMensagem.Text = string.Empty;

                        tseAluno.Enabled = true;

                        Ly_aluno dtAluno = new Ly_aluno();
                        Ly_aluno.Row dadosAluno = dtAluno.NewRow();
                        dadosAluno.Aluno = tseAluno.DBValue.ToString();

                        dadosAluno = RN.Aluno.ConsultarAluno(dadosAluno.Aluno);

                        if (dadosAluno != null)
                        {
                            PreencherDadosTela(dadosAluno);
                            CarregarDadosGrid("grdEncerramentos", dadosAluno.Aluno);
                            pnDados.Visible = true;
                            grdEncerramentos.Visible = true;
                        }
                        else
                        {
                            LimparTela();
                            pnDados.Visible = false;
                            grdEncerramentos.Visible = false;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        }
                        break;
                    }

                case TipoOperacao.Retorno:
                    {
                        string aluno = string.Empty;

                        if (!String.IsNullOrEmpty(Request.QueryString["Aluno"]))
                            aluno = Request.QueryString["Aluno"];
                        else
                        {
                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                            aluno = ObterDadosQueryString(decodedText);
                        }
                        tseAluno.DBValue = aluno;
                        tseAluno.DataBind();
                        tseAluno.Enabled = true;

                        Ly_aluno dtAluno = new Ly_aluno();
                        Ly_aluno.Row dadosAluno = dtAluno.NewRow();
                        dadosAluno.Aluno = aluno;

                        dadosAluno = RN.Aluno.ConsultarAluno(dadosAluno.Aluno);

                        if (dadosAluno != null)
                        {
                            PreencherDadosTela(dadosAluno);
                            CarregarDadosGrid("grdEncerramentos", dadosAluno.Aluno);
                            pnDados.Visible = true;
                            grdEncerramentos.Visible = true;
                        }
                        else
                        {
                            LimparTela();
                            pnDados.Visible = false;
                            grdEncerramentos.Visible = false;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        }
                        break;
                    }
            }
        }


        protected void LimparTela()
        {
            txtCurriculo.Text = string.Empty;
            txtCurso.Text = string.Empty;
            txtSerie.Text = string.Empty;
            txtSituacao.Text = string.Empty;
            txtTurno.Text = string.Empty;
            txtUniEnsino.Text = string.Empty;

        }


        private void PreencherDadosTela(Ly_aluno.Row dadosAluno)
        {
            txtCurriculo.Text = Convert.ToString(dadosAluno.Curriculo);
            txtCurso.Text = Convert.ToString(dadosAluno.Curso);
            txtNomeCurso.Text = Convert.ToString(dadosAluno.Nome_curso);
            txtSerie.Text = Convert.ToString(dadosAluno.Serie);
            txtNomeSerie.Text = Convert.ToString(dadosAluno.Descricao_serie);
            txtSituacao.Text = Convert.ToString(dadosAluno.Sit_aluno);
            txtTurno.Text = Convert.ToString(dadosAluno.Turno);
            txtNomeTurno.Text = Convert.ToString(dadosAluno.Descricao_turno);
            txtUniEnsino.Text = Convert.ToString(dadosAluno.Nome_comp04);

        }

           
    }
}
