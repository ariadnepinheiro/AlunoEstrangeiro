using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;
using Techne.Lyceum.RN;
using System.Configuration;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/FechamentoMatriculaLista.aspx"), ControlText("Fechamento Matrícula"), Title("Fechamento do Ano Letivo e Matrícula"),]
    public partial class FechamentoMatriculaLista : TPage
    {

        #region Propriedades

        private string Curriculo
        {
            get { return (string)ViewState["Curriculo"]; }
            set { ViewState["Curriculo"] = value; }
        }

        private Techne.Lyceum.RN.Turma.DadosTurma ObjetoTurma
        {
            get { return (Techne.Lyceum.RN.Turma.DadosTurma)ViewState["ObjetoTurma"]; }
            set { ViewState["ObjetoTurma"] = value; }
        }

        private Techne.Lyceum.RN.Turma.DadosTurma ObjetoTurmaPesquisa
        {
            get { return (Techne.Lyceum.RN.Turma.DadosTurma)ViewState["ObjetoTurmaPesquisa"]; }
            set { ViewState["ObjetoTurmaPesquisa"] = value; }
        }

        #endregion

        #region Código Padrão Techne
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
        #endregion

        #region Eventos

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdGradeSerie, "Turmas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdGradeSerie);
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CarregarDadosDrop(ddlAno.ID);
                    CarregarDadosDrop(ddlPeriodo.ID);

                    PreencherDadosSession();

                    //verifica se existe alguma querystring
                    if (Request.QueryString.Keys.Count > 0)
                        CarregarDadosTurma();
                    else
                        grdGradeSerie.Enabled = false;
                }
                //Carrega grid de acordo com os filtros selecionados pelo usuário
                CarregarGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                lblUAValor.Text = string.Empty;

                ObjetoTurmaPesquisa = null;
                CarregarGrid();

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!tseUnidadeResponsavel["ua_atual"].IsNull)
                            lblUAValor.Text = Convert.ToString(tseUnidadeResponsavel["ua_atual"]);

                        if (sessao != null)
                            sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);
                    }
                    else
                    {
                        if (sessao != null)
                            sessao.Escola = string.Empty;
                    }
                }
                else
                {
                    if (sessao != null)
                        sessao.Escola = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CarregarDadosDrop(ddlPeriodo.ID);
                ObjetoTurmaPesquisa = null;
                CarregarGrid();

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                    {
                        sessao.Ano = ddlAno.SelectedValue;
                        sessao.Periodo = string.Empty;
                    }
                    else
                    {
                        sessao.Ano = string.Empty;
                        sessao.Periodo = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                ObjetoTurmaPesquisa = null;

                ObterDadosTelaPesquisa();
                CarregarGrid();

                grdGradeSerie.Enabled = true;


                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                        sessao.Periodo = ddlPeriodo.SelectedValue;
                    else
                        sessao.Periodo = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;

                throw;
            }
        }

        protected void grdGradeSerie_SelectionChanged(object sender, EventArgs e)
        {
            string tipoOperacao = "CONSULTAR";
            try
            {
                int curPageSelection = GetSelectedRowOnTheCurrentPage();

                Techne.Lyceum.RN.Turma.DadosTurma turma = new Techne.Lyceum.RN.Turma.DadosTurma();

                turma.Grade = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "grade"));
                turma.Faculdade = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "faculdade"));
                turma.Turno = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "turno"));
                turma.Curso = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "curso"));
                turma.Ano = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "ano"));
                turma.Periodo = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "semestre"));
                turma.Serie = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "serie"));
                turma.Grade_ID = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "grade_id"));
                turma.Curriculo = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "curriculo"));
                turma.UnidadeResponsavel = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "unidade_responsavel"));
                turma.OptativaReforco = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "optativareforco"));
                turma.Eletiva = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "eletiva"));

                string queryString = MontarQueryString(tipoOperacao, turma);

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                ASPxWebControl.RedirectOnCallback("FechamentoMatricula.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdGradeSerie_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdGradeSerie);
        }

        #endregion

        #region Métodos

        private void PreencherDadosSession()
        {
            try
            {
                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!string.IsNullOrEmpty(sessao.Ano))
                    {
                        if (ddlAno.Items.FindByValue(sessao.Ano) != null)
                        {
                            ddlAno.SelectedValue = sessao.Ano;
                            if (ObjetoTurma == null)
                                ObjetoTurma = new Techne.Lyceum.RN.Turma.DadosTurma();

                            ObjetoTurma.Ano = sessao.Ano;
                            CarregarDadosDrop(ddlPeriodo.ID);
                        }
                    }
                    if (!string.IsNullOrEmpty(sessao.Periodo))
                    {
                        if (ddlPeriodo.Items.FindByValue(sessao.Periodo) != null)
                            ddlPeriodo.SelectedValue = sessao.Periodo;
                    }

                    if (!string.IsNullOrEmpty(sessao.Escola))
                    {
                        lblUAValor.Text = string.Empty;

                        tseUnidadeResponsavel.DBValue = sessao.Escola;

                        if (!tseUnidadeResponsavel.IsValidDBValue)
                        {
                            tseUnidadeResponsavel.Msg = string.Empty;
                            tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            if (!tseUnidadeResponsavel["ua_atual"].IsNull)
                                lblUAValor.Text = Convert.ToString(tseUnidadeResponsavel["ua_atual"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            try
            {
                int startIndexOnPage = grdGradeSerie.PageIndex * grdGradeSerie.SettingsPager.PageSize;
                for (int i = 0; i < grdGradeSerie.VisibleRowCount; i++)
                {
                    if (grdGradeSerie.Selection.IsRowSelected(startIndexOnPage + i))
                        return startIndexOnPage + i;
                }
                return -1;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return -1;
            }
        }

        private void ObterDadosQueryString(string queryString)
        {
            try
            {
                ObjetoTurma = new Techne.Lyceum.RN.Turma.DadosTurma();
                lblMensagem.Text = string.Empty;
                string[] listaDados = queryString.Split('&');

                foreach (string dados in listaDados)
                {
                    if (dados.IndexOf("ano") >= 0)
                        ObjetoTurma.Ano = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("semestre") >= 0)
                        ObjetoTurma.Periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("nucleo") >= 0)
                        ObjetoTurma.Nucleo = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("municipio") >= 0)
                        ObjetoTurma.Municipio = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("grade") >= 0)
                        ObjetoTurma.Grade = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("grade") >= 0)
                        ObjetoTurma.Grade = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("mensagem") >= 0)
                        lblMensagem.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        private void CarregarDadosTurma()
        {
            try
            {
                byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                ObterDadosQueryString(decodedText);

                CarregarDadosDrop(ddlAno.ID);
                CarregarDadosDrop(ddlPeriodo.ID);

                grdGradeSerie.Enabled = true;

                ObterDadosTelaPesquisa();
                ObjetoTurma = null;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        private DataTable DataTableEstrutura()
        {
            DataTable dtEstrutura = new DataTable();

            dtEstrutura.Columns.Add("grade_id", typeof(int));
            dtEstrutura.Columns.Add("ano", typeof(int));
            dtEstrutura.Columns.Add("semestre", typeof(int));
            dtEstrutura.Columns.Add("curso", typeof(string));
            dtEstrutura.Columns.Add("nomeCurso", typeof(string));
            dtEstrutura.Columns.Add("turno", typeof(string));
            dtEstrutura.Columns.Add("descricaoTurno", typeof(string));
            dtEstrutura.Columns.Add("serie", typeof(string));
            dtEstrutura.Columns.Add("descricaoSerie", typeof(string));
            dtEstrutura.Columns.Add("grade", typeof(string));
            dtEstrutura.Columns.Add("unidade_responsavel", typeof(string));
            dtEstrutura.Columns.Add("nomeUnidadeResponsavel", typeof(string));
            dtEstrutura.Columns.Add("capacidade", typeof(string));
            dtEstrutura.Columns.Add("dependencia", typeof(string));
            dtEstrutura.Columns.Add("em_elaboracao", typeof(string));
            dtEstrutura.Columns.Add("eletiva", typeof(string));

            return dtEstrutura;
        }

        private void ObterDadosTelaPesquisa()
        {
            try
            {
                if (ObjetoTurmaPesquisa == null)
                    ObjetoTurmaPesquisa = new Techne.Lyceum.RN.Turma.DadosTurma();

                ObjetoTurmaPesquisa.Ano = ddlAno.SelectedValue;
                ObjetoTurmaPesquisa.Periodo = ddlPeriodo.SelectedValue;

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                    ObjetoTurmaPesquisa.UnidadeResponsavel = Convert.ToString(tseUnidadeResponsavel.DBValue);

                if (!tseCurso.DBValue.IsNull)
                    ObjetoTurmaPesquisa.Curso = Convert.ToString(tseCurso.DBValue);

                if (!string.IsNullOrEmpty(ddlTurno.SelectedValue))
                    ObjetoTurmaPesquisa.Turno = ddlTurno.SelectedValue;

                RN.Entidades.LyUnidadeEnsino rowUE = RN.UnidadeEnsino.Carregar(Convert.ToString(tseUnidadeResponsavel.DBValue));
                if (rowUE != null)
                {
                    ObjetoTurmaPesquisa.Nucleo = rowUE.Nucleo;
                    ObjetoTurmaPesquisa.Municipio = rowUE.Municipio;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarGrid()
        {
            QueryTable dtGradeSerie = null;
            RN.GradeSerie rnGradeSerie = new GradeSerie();
            try
            {
                if (ObjetoTurmaPesquisa != null)
                {
                    if (!string.IsNullOrEmpty(ObjetoTurmaPesquisa.Ano) && !string.IsNullOrEmpty(ObjetoTurmaPesquisa.Periodo)
                        && !string.IsNullOrEmpty(ObjetoTurmaPesquisa.UnidadeResponsavel))
                    {
                        dtGradeSerie = GradeSerie.ConsultarTurmaMatricula(ObjetoTurmaPesquisa.Ano, ObjetoTurmaPesquisa.Periodo, ObjetoTurmaPesquisa.Curso,
                                                               ObjetoTurmaPesquisa.Turno, ObjetoTurmaPesquisa.UnidadeResponsavel, null);
                    }
                }
                if (dtGradeSerie != null)
                {
                    if (dtGradeSerie.Rows.Count > 0)
                        grdGradeSerie.DataSource = dtGradeSerie;
                    else
                        grdGradeSerie.DataSource = DataTableEstrutura();
                }
                else
                    grdGradeSerie.DataSource = DataTableEstrutura();

                grdGradeSerie.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarDropDownList(DropDownList drop, QueryTable data, List<DropDownList> listaDrop, string defaultValue)
        {
            try
            {
                drop.DataSource = data;
                drop.DataBind();

                if (data != null)
                {
                    if (data.Rows != null)
                    {
                        if (data.Rows.Count > 0)
                        {
                            if (drop.Items.FindByValue(defaultValue) != null)
                                drop.SelectedValue = defaultValue;
                            else
                                CriarItemVazio(drop, true);
                        }
                    }
                }
                if (listaDrop != null)
                {
                    foreach (DropDownList dropDependente in listaDrop)
                    {
                        dropDependente.Items.Clear();
                        dropDependente.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CriarItemVazio(DropDownList drop, bool selecionado)
        {
            ListItem itemVazio = new ListItem("<Selecione>", "-1");
            if (!drop.Items.Contains(itemVazio))
                drop.Items.Add(itemVazio);

            if (selecionado)
            {
                drop.ClearSelection();
                //seleciona o item vazio
                drop.Items.FindByValue("-1").Selected = true;
            }
        }

        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;
            RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
            var periodoSimultaneo = Convert.ToBoolean(ConfigurationManager.AppSettings["PeriodoSimultaneo"] ?? "false");
            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLANO":
                        {
                            if (periodoSimultaneo)
                            {
                                dadosDrop = RN.PeriodoLetivo.ConsultarAno();
                            }
                            else
                            {
                                dadosDrop = RN.PeriodoLetivo.ConsultarAnosFinalizados();
                            }

                            List<DropDownList> listaDrop = new List<DropDownList>();
                            listaDrop.Add(ddlPeriodo);

                            if ((ObjetoTurma == null) || (ObjetoTurma != null && string.IsNullOrEmpty(ObjetoTurma.Ano)))
                            {
                                CarregarDropDownList(ddlAno, dadosDrop, listaDrop, null);
                                CriarItemVazio(ddlAno, true);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(ObjetoTurma.Ano))
                                    CarregarDropDownList(ddlAno, dadosDrop, listaDrop, ObjetoTurma.Ano);
                                else
                                    CarregarDropDownList(ddlAno, dadosDrop, listaDrop, null);
                            }

                            break;
                        }
                    case "DDLPERIODO":
                        {
                            string ano = string.Empty;

                            if (ddlAno.Items.Count > 0)
                                ano = ddlAno.SelectedValue;

                            if (!string.IsNullOrEmpty(ano))
                            {

                                //Código temporário solicitado pelo Alessandro. Deve ser removido e dar uma melhor opção.
                                if (periodoSimultaneo)
                                {
                                    dadosDrop = RN.PeriodoLetivo.ConsultarPeriodo(ano);
                                }
                                else if (ano == "2011")
                                {
                                    dadosDrop = RN.PeriodoLetivo.RetornarPeriodoEscolhido(ano, 1);
                                }
                                else
                                {
                                    dadosDrop = RN.PeriodoLetivo.ConsultarPeriodosFinalizados(ano);
                                }

                                List<DropDownList> listaDrop = new List<DropDownList>();

                                if (ObjetoTurma == null)
                                    CarregarDropDownList(ddlPeriodo, dadosDrop, listaDrop, string.Empty);
                                else
                                {
                                    if (!string.IsNullOrEmpty(ObjetoTurma.Periodo))
                                        CarregarDropDownList(ddlPeriodo, dadosDrop, listaDrop, ObjetoTurma.Periodo);
                                    else
                                        CarregarDropDownList(ddlPeriodo, dadosDrop, listaDrop, null);
                                }
                            }

                            if ((ObjetoTurma == null) || (ObjetoTurma != null && string.IsNullOrEmpty(ObjetoTurma.Periodo)))
                                CriarItemVazio(ddlPeriodo, true);

                            break;
                        }

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            return dadosDrop;
        }

        private string MontarQueryString(string tipoOperacao, Techne.Lyceum.RN.Turma.DadosTurma turma)
        {
            string queryString = string.Empty;

            if (turma != null)
            {
                queryString += "tipoOperacao=" + tipoOperacao;
                queryString += "&ano=" + turma.Ano;
                queryString += "&semestre=" + turma.Periodo;
                queryString += "&nucleo=" + turma.Nucleo;
                queryString += "&municipio=" + turma.Municipio;
                queryString += "&unidadeResponsavel=" + turma.UnidadeResponsavel;
                queryString += "&prefixoUnidadeResponsavel=" + turma.MnemonicoUnidadeResponsavel;
                queryString += "&grade=" + turma.Grade.Replace("&", "##");
                queryString += "&faculdade=" + turma.Faculdade;
                queryString += "&turno=" + turma.Turno;
                queryString += "&curso=" + turma.Curso;
                queryString += "&serie=" + turma.Serie;
                queryString += "&gradeId=" + turma.Grade_ID;
                queryString += "&curriculo=" + turma.Curriculo;
                queryString += "&eletiva=" + turma.Eletiva;

            }
            return queryString;
        }
        #endregion
    }
}
