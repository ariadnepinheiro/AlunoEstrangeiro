using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Academico
{
    [
        NavUrl("~/Academico/ListarFrequenciaTurma.aspx"),
        ControlText("ListarFrequenciaTurma"),
        Title("Frequência Diária"),
    ]

    public partial class ListarFrequenciaTurma : TPage
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
            TituloGrid(grdTurma, "Turmas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!this.IsPostBack)
                {
                    LimpaCampos();
                    CarregaAno();

                    if (Request.QueryString.Keys.Count > 0)
                        CarregarDadosTurma();

                    CarregarGrid();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void LimpaCampos()
        {
            ddlAno.Items.Clear();
            ddlPeriodo.Items.Clear();
            tseRegional.ResetValue();
            tseMunicipio.ResetValue();
            tseUnidadeResponsavel.ResetValue();
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTurma);
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            CarregarGrid();
        }

        private void CarregaAno()
        {
            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = RN.PeriodoLetivo.ConsultarAnoFrequencia();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        private void CarregaPeriodo(int ano)
        {
            ddlPeriodo.Items.Clear();

            if (ano > 0)
            {
                ListItem item = new ListItem("Selecione", string.Empty);
                ddlPeriodo.DataSource = RN.PeriodoLetivo.ListarPeriodo(ano.ToString());
                ddlPeriodo.DataBind();
                ddlPeriodo.Items.Insert(0, item);
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
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
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
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
                ObjetoTurmaPesquisa = null;
                CarregarGrid();

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);
                            sessao.Municipio = tseUnidadeResponsavel["municipio"].ToString();
                            sessao.Regional = tseUnidadeResponsavel["id_regional"].ToString();
                            tseMunicipio.Value = tseUnidadeResponsavel["municipio"].ToString();
                            tseRegional.Value = tseUnidadeResponsavel["id_regional"].ToString();
                        }
                    }
                    else
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Regional = string.Empty;
                        }

                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }
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
                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaPeriodo(Convert.ToInt32(ddlAno.SelectedValue));
                }
                ObjetoTurmaPesquisa = null;
                CarregarGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
          
                ObjetoTurmaPesquisa = null;
                CarregarGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        /// <summary>
        /// Evento de SelectionChanged da grid de grade serie
        /// </summary>
        /// <param name="sender">objeto do sistema</param>
        /// <param name="e">argumento do sistema</param>
        protected void grdTurma_SelectionChanged(object sender, EventArgs e)
        {
            int curPageSelection = GetSelectedRowOnTheCurrentPage();

            Techne.Lyceum.RN.Turma.DadosTurma turma = new Techne.Lyceum.RN.Turma.DadosTurma();

            turma.Turma = Convert.ToString(grdTurma.GetRowValues(curPageSelection, "turma"));
            turma.Faculdade = Convert.ToString(grdTurma.GetRowValues(curPageSelection, "faculdade"));
            turma.Turno = Convert.ToString(grdTurma.GetRowValues(curPageSelection, "turno"));            
            turma.Curso = Convert.ToString(grdTurma.GetRowValues(curPageSelection, "curso"));
            turma.Ano = Convert.ToString(grdTurma.GetRowValues(curPageSelection, "ano"));
            turma.Periodo = Convert.ToString(grdTurma.GetRowValues(curPageSelection, "semestre"));
            turma.Serie = Convert.ToString(grdTurma.GetRowValues(curPageSelection, "serie"));
            turma.Curriculo = Convert.ToString(grdTurma.GetRowValues(curPageSelection, "curriculo"));
            turma.Municipio = tseMunicipio["nome"].ToString();
            turma.Regional = tseRegional["regional"].ToString();
            turma.UnidadeResponsavel = tseUnidadeResponsavel["nome_comp"].ToString();
            turma.NomeCurso = Convert.ToString(grdTurma.GetRowValues(curPageSelection, "nomeCurso"));

            string queryString = MontarQueryString(turma);

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

            ASPxWebControl.RedirectOnCallback("FrequenciaTurma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        protected void grdTurma_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
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
            coluna.Add("municipio");
            coluna.Add("id_regional");

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
                    DefinirFiltroUnidadeEnsino();

                    tseUnidadeResponsavel.DBValue = sessao.Escola;

                    QueryTable qt = RN.UnidadeEnsino.ConsultarUnidadeQT(Convert.ToString(tseUnidadeResponsavel.DBValue));

                    if (!tseUnidadeResponsavel.IsValidDBValue)
                    {
                        tseUnidadeResponsavel.Msg = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(sessao.Regional))
                            tseRegional.Value = tseUnidadeResponsavel["id_regional"].ToString();

                        if (string.IsNullOrEmpty(sessao.Municipio))
                            tseMunicipio.Value = tseUnidadeResponsavel["municipio"].ToString();
                    }
                }
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdTurma.PageIndex * grdTurma.SettingsPager.PageSize;
            for (int i = 0; i < grdTurma.VisibleRowCount; i++)
            {
                if (grdTurma.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
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
                    ObjetoTurma.Regional = dados.Substring(dados.LastIndexOf('=') + 1);
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

                if (!ObjetoTurma.Ano.IsNullOrEmptyOrWhiteSpace())
                {
                    ddlAno.SelectedValue = ObjetoTurma.Ano;
                    ddlAno_SelectedIndexChanged(null, null);
                }

                if (!ObjetoTurma.Periodo.IsNullOrEmptyOrWhiteSpace())
                {
                    ddlPeriodo.SelectedValue = ObjetoTurma.Periodo;
                    ddlPeriodo_SelectedIndexChanged(null, null);
                }

                if (!ObjetoTurma.Faculdade.IsNullOrEmptyOrWhiteSpace())
                {
                    tseUnidadeResponsavel.DBValue = ObjetoTurma.Faculdade;
                    tseUnidadeResponsavel_Changed(null, null);
                }
             

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        private DataTable DataTableEstrutura()
        {
            DataTable dtEstrutura = new DataTable();


            dtEstrutura.Columns.Add("ano", typeof(int));
            dtEstrutura.Columns.Add("semestre", typeof(int));
            dtEstrutura.Columns.Add("curso", typeof(string));
            dtEstrutura.Columns.Add("nomeCurso", typeof(string));
            dtEstrutura.Columns.Add("turno", typeof(string));
            dtEstrutura.Columns.Add("descricaoTurno", typeof(string));
            dtEstrutura.Columns.Add("serie", typeof(string));
            dtEstrutura.Columns.Add("descricaoSerie", typeof(string));
            dtEstrutura.Columns.Add("turma", typeof(string));
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
            DataTable qt = new DataTable();
            RN.Turma rnTurma = new Techne.Lyceum.RN.Turma();
            try
            {

                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace()
                    && (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue))

                {

                    qt = rnTurma.ListaTurmaFrequenciaPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), tseUnidadeResponsavel.DBValue.ToString());

                }

                if (qt != null)
                {
                    if (qt.Rows.Count > 0)
                        grdTurma.DataSource = qt;
                    else
                        grdTurma.DataSource = DataTableEstrutura();
                }
                else
                    grdTurma.DataSource = DataTableEstrutura();

                grdTurma.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }


        private string MontarQueryString(Techne.Lyceum.RN.Turma.DadosTurma turma)
        {
            string queryString = string.Empty;

            if (turma != null)
            {
                queryString += "ano=" + turma.Ano;
                queryString += "&semestre=" + turma.Periodo;
                queryString += "&regional=" + turma.Regional;
                queryString += "&municipio=" + turma.Municipio;
                queryString += "&unidade=" + turma.UnidadeResponsavel;
                queryString += "&turma=" + turma.Turma;
                queryString += "&censo=" + turma.Faculdade;
                queryString += "&turno=" + turma.Turno;
                queryString += "&curso=" + turma.Curso;
                queryString += "&serie=" + turma.Serie;
                queryString += "&curriculo=" + turma.Curriculo;
                queryString += "&escolaridade=" + turma.NomeCurso;
            }
            return queryString;
        }
        #endregion

    }
}
