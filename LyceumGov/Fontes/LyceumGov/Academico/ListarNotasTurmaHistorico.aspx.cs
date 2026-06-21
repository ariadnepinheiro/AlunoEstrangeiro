using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.Net.Academico
{
    [
        NavUrl("~/Academico/ListarNotasTurmaHistorico.aspx"),
        ControlText("ListarNotasTurmaHistorico"),
        Title("Notas e Frequências por Turma - Histórico"),
    ]

    public partial class ListarNotasTurmaHistorico : TPage
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

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdGradeSerie, "Turmas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            //verifica acesso do usuário para os controles da página
            ControlaAcesso(grdGradeSerie);

            if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue)
            {
                lblMensagem.Text = string.Empty;
                ObjetoTurmaPesquisa = null;
                ObterDadosTelaPesquisa();
                CarregarGrid();
                grdGradeSerie.Enabled = true;
                grdGradeSerie.Visible = true;
            }
            else
            {
                grdGradeSerie.Visible = false;
            }
        }

        protected void Page_PreLoad(object sender, EventArgs e)
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

            DefinirFiltroUnidadeEnsino();
            //Carrega grid de acordo com os filtros selecionados pelo usuário
            CarregarGrid();
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            ObjetoTurmaPesquisa = null;
            CarregarGrid();

            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            if (sessao != null)
            {
                if (!tseMunicipio.DBValue.IsNull)
                {
                    if (tseMunicipio.IsValidDBValue)
                    {
                        sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);

                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
                else
                {
                    sessao.Municipio = string.Empty;
                    sessao.Escola = string.Empty;
                }
            }
            tseUnidadeResponsavel.ResetValue();
            //DefinirFiltroUnidadeEnsino();
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            ObjetoTurmaPesquisa = null;
            CarregarGrid();
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            if (sessao != null)
            {
                if (!tseRegional.DBValue.IsNull)
                {
                    if (tseRegional.IsValidDBValue)
                    {
                        sessao.Regional = Convert.ToString(tseRegional.DBValue);
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
                else
                {
                    sessao.Regional = string.Empty;
                    sessao.Municipio = string.Empty;
                    sessao.Escola = string.Empty;
                }
            }
            tseMunicipio.ResetValue();
            tseUnidadeResponsavel.ResetValue();
            //DefinirFiltroUnidadeEnsino();
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            ObjetoTurmaPesquisa = null;
            CarregarGrid();

            txtPrefixoUnidade.Text = string.Empty;
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

            if (!tseUnidadeResponsavel.DBValue.IsNull)
            {
                if (tseUnidadeResponsavel.IsValidDBValue)
                {
                    if (sessao != null)
                        sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);

                    //CR.Ly_unidade_ensino.Row ue = RN.UnidadeEnsino.ConsultarUnidade(Convert.ToString(tseUnidadeResponsavel["unidade_ens"]));
                    LyUnidadeEnsino ue = RN.UnidadeEnsino.Carregar(Convert.ToString(tseUnidadeResponsavel["unidade_ens"]));

                    if (ue != null)
                    {
                        txtPrefixoUnidade.Text = ue.Mnemonico;

                        tseMunicipio.Value = ue.Municipio;
                        tseRegional.Value = ue.IdRegional;
                    }
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

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarDadosDrop(ddlPeriodo.ID);
            ObjetoTurmaPesquisa = null;
            CarregarGrid();
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Evento de SelectionChanged da grid de grade serie
        /// </summary>
        /// <param name="sender">objeto do sistema</param>
        /// <param name="e">argumento do sistema</param>
        protected void grdGradeSerie_SelectionChanged(object sender, EventArgs e)
        {
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
                turma.Municipio = ObjetoTurmaPesquisa.Municipio;
                turma.Nucleo = ObjetoTurmaPesquisa.Nucleo;

                turma.UnidadeResponsavel = Convert.ToString(grdGradeSerie.GetRowValues(curPageSelection, "unidade_responsavel"));

                ObterCurriculo(turma.Curso, turma.Turno, turma.Ano, turma.Periodo);

                string queryString = MontarQueryString(turma);

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                ASPxWebControl.RedirectOnCallback("HistoricoNotasTurma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdGradeSerie_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {

        }

        #region Métodos

        private void DefinirFiltroUnidadeEnsino()
        {
            string sqlWhere = string.Empty;
            string table = string.Empty;

            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();

            table = " VW_UNIDADE_ENSINO_SITUACAO ";

            coluna.Add("unidade_ens");
            coluna.Add("nome_comp");
            coluna.Add("setor");
            coluna.Add("cgc");
            coluna.Add("situacao");
			coluna.Add("ua_atual");
            coluna.Add("ua_antiga");

            if (!tseRegional.DBValue.IsNull)
            {
                if (tseRegional.IsValidDBValue)
                    sqlWhere = " id_regional = " + Convert.ToString(tseRegional.DBValue);
            }

            if (!tseMunicipio.DBValue.IsNull)
            {
                if (tseMunicipio.IsValidDBValue)
                {
                    if (string.IsNullOrEmpty(sqlWhere))
                        sqlWhere = "municipio = " + Convert.ToString(tseMunicipio.DBValue);
                    else
                        sqlWhere += " AND municipio = " + Convert.ToString(tseMunicipio.DBValue);
                }
            }

            if (coluna.Count > 0)
            {
                Techne.Library.Sql.Structure.SqlSelect sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

                tseUnidadeResponsavel.SqlSelect = sqlSelect;
                tseUnidadeResponsavel.SqlWhere = sqlWhere;

                tseUnidadeResponsavel.DataBind();
            }
        }

        private void PreencherDadosSession()
        {
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

            if (sessao != null)
            {
                if (!string.IsNullOrEmpty(sessao.Regional))
                {
                    tseRegional.DBValue = sessao.Regional;
                    if (!tseRegional.IsValidDBValue)
                    {
                        tseRegional.Msg = string.Empty;
                        tseRegional.ResetValue();
                    }
                }
                if (!string.IsNullOrEmpty(sessao.Municipio))
                {
                    tseMunicipio.DBValue = sessao.Municipio;
                    if (!tseMunicipio.IsValidDBValue)
                    {
                        tseMunicipio.Msg = string.Empty;
                        tseMunicipio.ResetValue();
                    }
                }
                if (!string.IsNullOrEmpty(sessao.Escola))
                {
                    tseUnidadeResponsavel.DBValue = sessao.Escola;

                    QueryTable qt = RN.UnidadeEnsino.ConsultarUnidadeQT(Convert.ToString(tseUnidadeResponsavel.DBValue));

                    if (qt != null)
                        txtPrefixoUnidade.Text = Convert.ToString(qt.Rows[0]["mnemonico"]);

                    if (!tseUnidadeResponsavel.IsValidDBValue)
                    {
                        tseUnidadeResponsavel.Msg = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdGradeSerie.PageIndex * grdGradeSerie.SettingsPager.PageSize;
            for (int i = 0; i < grdGradeSerie.VisibleRowCount; i++)
            {
                if (grdGradeSerie.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }

        private void ObterCurriculo(string curso, string turno, string ano, string periodo)
        {
            QueryTable qt = null;
            Curriculo = string.Empty;
            if (!string.IsNullOrEmpty(curso) && !string.IsNullOrEmpty(turno) && !string.IsNullOrEmpty(ano) && !string.IsNullOrEmpty(periodo))
            {
                qt = RN.Curriculo.Consultar(turno, curso, Convert.ToDecimal(ano), Convert.ToDecimal(periodo));

                if (qt != null)
                {
                    if (qt.Rows.Count > 0)
                        Curriculo = Convert.ToString(qt.Rows[0]["CURRICULO"]);
                }
            }
        }

        private void ObterDadosQueryString(string queryString)
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
                else if (dados.IndexOf("faculdade") >= 0)
                    ObjetoTurma.Faculdade = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("unidadeResponsavel") >= 0)
                    ObjetoTurma.UnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
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

                if (!String.IsNullOrEmpty(ObjetoTurma.Regional))
                    tseRegional.Value = ObjetoTurma.Regional;
                if (!String.IsNullOrEmpty(ObjetoTurma.Faculdade))
                    tseUnidadeResponsavel.Value = ObjetoTurma.Faculdade;
                if (!String.IsNullOrEmpty(ObjetoTurma.Municipio))
                    tseMunicipio.Value = ObjetoTurma.Municipio;

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

            return dtEstrutura;
        }

        private void ObterDadosTelaPesquisa()
        {
            if (ObjetoTurmaPesquisa == null)
                ObjetoTurmaPesquisa = new Techne.Lyceum.RN.Turma.DadosTurma();

            ObjetoTurmaPesquisa.Ano = ddlAno.SelectedValue;
            ObjetoTurmaPesquisa.Periodo = ddlPeriodo.SelectedValue;

            if (!tseUnidadeResponsavel.DBValue.IsNull)
            {
                ObjetoTurmaPesquisa.UnidadeResponsavel = Convert.ToString(tseUnidadeResponsavel.DBValue);
                ObjetoTurmaPesquisa.MnemonicoUnidadeResponsavel = txtPrefixoUnidade.Text;
            }

            if (!tseRegional.DBValue.IsNull)
            {
                if (tseRegional.IsValidDBValue)
                    ObjetoTurmaPesquisa.Regional = Convert.ToString(tseRegional.DBValue);
            }

            if (!tseMunicipio.DBValue.IsNull)
            {
                if (tseMunicipio.IsValidDBValue)
                    ObjetoTurmaPesquisa.Municipio = Convert.ToString(tseMunicipio.DBValue);
            }
        }

        private void CarregarGrid()
        {
            QueryTable qtGradeSerie = null;

            try
            {
                if (ObjetoTurmaPesquisa != null)
                {
                    if (!string.IsNullOrEmpty(ObjetoTurmaPesquisa.Ano) && !string.IsNullOrEmpty(ObjetoTurmaPesquisa.Periodo)
                        && !string.IsNullOrEmpty(ObjetoTurmaPesquisa.UnidadeResponsavel))
                    {
                        qtGradeSerie = RN.GradeSerie.ConsultarTurmaFinalizadaHistorico(ObjetoTurmaPesquisa.Ano, ObjetoTurmaPesquisa.Periodo, string.Empty,
                                                               string.Empty, ObjetoTurmaPesquisa.UnidadeResponsavel, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            if (qtGradeSerie != null)
            {
                if (qtGradeSerie.Rows.Count > 0)
                    grdGradeSerie.DataSource = qtGradeSerie;
                else
                    grdGradeSerie.DataSource = DataTableEstrutura();
            }
            else
                grdGradeSerie.DataSource = DataTableEstrutura();

            grdGradeSerie.DataBind();
        }

        private void CarregarDropDownList(DropDownList drop, QueryTable data, List<DropDownList> listaDrop, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();

            if (drop.Items.Count == 0)
            {
                ListItem itemVazio = new ListItem("<Lista Vazia>", "");
                drop.Items.Add(itemVazio);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(defaultValue))
                        drop.SelectedValue = defaultValue;
                }
                catch (ArgumentOutOfRangeException)
                {
                    drop.ClearSelection();
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

        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLANO":
                        {
                            dadosDrop = RN.PeriodoLetivo.ConsultarAno();

                            List<DropDownList> listaDrop = new List<DropDownList>();
                            listaDrop.Add(ddlPeriodo);

                            if (ObjetoTurma == null)
                                CarregarDropDownList(ddlAno, dadosDrop, listaDrop, DateTime.Now.Year.ToString());
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
                                dadosDrop = RN.PeriodoLetivo.ConsultarPeriodo(ano);

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

        private string MontarQueryString(Techne.Lyceum.RN.Turma.DadosTurma turma)
        {
            string queryString = string.Empty;

            if (turma != null)
            {
                queryString += "ano=" + turma.Ano;
                queryString += "&semestre=" + turma.Periodo;
                queryString += "&nucleo=" + turma.Nucleo;
                queryString += "&municipio=" + turma.Municipio;
                queryString += "&unidadeResponsavel=" + turma.UnidadeResponsavel;
                queryString += "&prefixoUnidadeResponsavel=" + turma.MnemonicoUnidadeResponsavel;
                queryString += "&grade=" + turma.Grade;
                queryString += "&faculdade=" + turma.Faculdade;
                queryString += "&turno=" + turma.Turno;
                queryString += "&curso=" + turma.Curso;
                queryString += "&serie=" + turma.Serie;
                queryString += "&gradeId=" + turma.Grade_ID;
                queryString += "&grade_id=" + turma.Grade_ID;

                if (!string.IsNullOrEmpty(Curriculo))
                    queryString += "&curriculo=" + Curriculo;

            }
            return queryString;
        }
        #endregion

    }
}
