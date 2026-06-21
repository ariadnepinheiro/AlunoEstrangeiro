using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.CR;
using System.Web.UI.MobileControls;
using Techne.Lyceum.RN;
using System.IO;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/ListarOcorrencias.aspx"),
     ControlText("ListadeOcorrencias"),
     Title("Histórico de Ocorrências"),]
    public partial class ListarOcorrencias : TPage
    {
		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdOcorrencias, "Histórico de Ocorrências");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            odsTurma.SelectParameters[0].DefaultValue = "-1";
            pnDadosAcademicos.Visible = false;
            grdOcorrencias.Enabled = false;

            string aluno;
            if (!IsPostBack)
            {
                grdOcorrencias.Visible = false;

                if (Request.QueryString.Keys.Count > 0)
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    aluno = ObterDadosQueryString(decodedText);
                    tseAluno.Text = aluno;
                    tseAluno.DataBind();
                    grdOcorrencias.Enabled = true;
                    odsTurma.SelectParameters[0].DefaultValue = aluno;
                    CarregarDadosGrid("grdOcorrencias", aluno); 
                    CarregaDadosAluno(aluno);
                    grdOcorrencias.Visible = true;
                }
            }
            if (tseAluno.IsValidDBValue && !tseAluno.DBValue.IsNull)
            {
                grdOcorrencias.Enabled = true;
                grdOcorrencias.Visible = true;
                pnDadosAcademicos.Visible = true;
                odsTurma.SelectParameters[0].DefaultValue = tseAluno.DBValue.ToString();
                CarregarDadosGrid("grdOcorrencias", tseAluno.DBValue.ToString());
                CarregaDadosAluno(tseAluno.DBValue.ToString());

            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdOcorrencias);
        }

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

        #region Eventos

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            LimparCampos();
           
            if (tseAluno.IsValidDBValue && !tseAluno.DBValue.IsNull)
            {
                string aluno = tseAluno.DBValue.ToString();
                CarregarDadosGrid("grdOcorrencias", aluno);
                grdOcorrencias.Enabled = true;
                grdOcorrencias.Visible = true;
                pnDadosAcademicos.Visible = true;
                lblMensagem.Text = string.Empty;
                CarregaDadosAluno(aluno);

            }
            else if (!tseAluno.DBValue.IsNull)
            {
                grdOcorrencias.Visible = false;
                pnDadosAcademicos.Visible = false;
                lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";

            }
            else
            {
                grdOcorrencias.Visible = false;
                pnDadosAcademicos.Visible = false;
                lblMensagem.Text = "Favor consultar um aluno.";
            }
        }

        private QueryTable CarregarDadosGrid(string idGrid, string aluno)
        {
            pnDadosAcademicos.Visible = true;
            QueryTable dadosGrid = null;

            dadosGrid = RN.Ocorrencia.Consultar(aluno);

            //adiciona 3 pontos as descrições maiores que 100
            int count = dadosGrid.Rows.Count;
            string descricao = "";
            for (int i = 0; i < count; i++)
            {
                descricao = string.Empty;
                descricao = dadosGrid.Rows[i]["descricao"].ToString();
                if (descricao.Length > 100)
                {
                    string corta = descricao.Substring(0, 100);
                    dadosGrid.Rows[i]["descricao"] = corta + "...";
                }

            }

            grdOcorrencias.DataSource = dadosGrid;
            grdOcorrencias.DataBind();
            //tseAluno.DataBind();
            return dadosGrid;
        }

        protected void grdOcorrencias_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("aluno", e.Values["aluno"]);
            e.Keys.Add("data", e.Values["data"]);
            e.Keys.Add("tipo", e.Values["tipo"]);
            e.Keys.Add("ordem", e.Values["ordem"]);

            string tipoOperacao = string.Empty;
            Ly_ocorrencia dtOcorrencia = new Ly_ocorrencia();
            Ly_ocorrencia.Row dadosOcorrencia = dtOcorrencia.NewRow();

            dadosOcorrencia.Aluno = Convert.ToString(e.Values["aluno"]);

            tipoOperacao = "EXCLUIR";
            dadosOcorrencia.Aluno = Convert.ToString(e.Values["aluno"]);
            dadosOcorrencia.Data = Convert.ToDateTime(e.Values["data"]);
            dadosOcorrencia.Tipo = (string)e.Values["tipo"];
            dadosOcorrencia.Ordem = Convert.ToDecimal(e.Values["ordem"]);

            string queryString = MontarQueryString(tipoOperacao, dadosOcorrencia);

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

            ASPxWebControl.RedirectOnCallback("Ocorrencias.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        protected void grdOcorrencias_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "OcorrenciaKey")
            {
                string aluno = Convert.ToString(e.GetListSourceFieldValue("aluno"));
                string data = Convert.ToString(e.GetListSourceFieldValue("data"));
                string tipo = Convert.ToString(e.GetListSourceFieldValue("tipo"));
                string ordem = Convert.ToString(e.GetListSourceFieldValue("ordem"));
                e.Value = aluno + "-" + data + "-" + tipo + "-" + ordem;
            }

        }

        protected void grdOcorrencias_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
			ControlaAcesso(grdOcorrencias);

            if ((e.CallbackName == "ADDNEWROW") || (e.CallbackName == "STARTEDIT") || (e.CallbackName == "DELETEROW"))
            {
                string tipoOperacao = string.Empty;
                Ly_ocorrencia dtOcorrencia = new Ly_ocorrencia();
                Ly_ocorrencia.Row dadosOcorrencia = dtOcorrencia.NewRow();

                if (e.CallbackName == "ADDNEWROW")
                {
                    tipoOperacao = "NOVO";
                    dadosOcorrencia.Aluno = tseAluno.Value.ToString();
                }
                else if (e.CallbackName == "STARTEDIT")
                {
                    tipoOperacao = "ALTERAR";
                    dadosOcorrencia.Aluno = (string)grdOcorrencias.GetRowValuesByKeyValue(e.Args[0], "aluno");
                    dadosOcorrencia.Data = Convert.ToDateTime(grdOcorrencias.GetRowValuesByKeyValue(e.Args[0], "data"));
                    dadosOcorrencia.Tipo = (string)grdOcorrencias.GetRowValuesByKeyValue(e.Args[0], "tipo");
                    dadosOcorrencia.Ordem = Convert.ToDecimal(grdOcorrencias.GetRowValuesByKeyValue(e.Args[0], "ordem"));
                }
                else if (e.CallbackName == "DELETEROW")
                {
                    tipoOperacao = "EXCLUIR";
                    dadosOcorrencia.Aluno = (string)grdOcorrencias.GetRowValuesByKeyValue(e.Args[0], "aluno");
                    dadosOcorrencia.Data = Convert.ToDateTime(grdOcorrencias.GetRowValuesByKeyValue(e.Args[0], "data"));
                    dadosOcorrencia.Tipo = (string)grdOcorrencias.GetRowValuesByKeyValue(e.Args[0], "tipo");
                    dadosOcorrencia.Ordem = Convert.ToDecimal(grdOcorrencias.GetRowValuesByKeyValue(e.Args[0], "ordem"));
                }

                string queryString = MontarQueryString(tipoOperacao, dadosOcorrencia);

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                ASPxWebControl.RedirectOnCallback("Ocorrencias.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
        }

        protected void grdOcorrencias_SelectionChanged(object sender, EventArgs e)
        {
            string tipoOperacao = string.Empty;
            Ly_ocorrencia dtOcorrencia = new Ly_ocorrencia();
            Ly_ocorrencia.Row dadosOcorrencia = dtOcorrencia.NewRow();

            int curPageSelection = GetSelectedRowOnTheCurrentPage();
            tipoOperacao = "CONSULTAR";
            dadosOcorrencia.Aluno = (string)grdOcorrencias.GetRowValues(curPageSelection, "aluno");
            dadosOcorrencia.Data = Convert.ToDateTime(grdOcorrencias.GetRowValues(curPageSelection, "data"));
            dadosOcorrencia.Tipo = (string)grdOcorrencias.GetRowValues(curPageSelection, "tipo");
            dadosOcorrencia.Ordem = Convert.ToDecimal(grdOcorrencias.GetRowValues(curPageSelection, "ordem"));

            string queryString = MontarQueryString(tipoOperacao, dadosOcorrencia);

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

            ASPxWebControl.RedirectOnCallback("Ocorrencias.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        protected void grdOcorrencias_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdOcorrencias.Settings.ShowFilterRow = false;
        }

        protected void grdOcorrencias_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            if (tseAluno.IsValidDBValue && !tseAluno.DBValue.IsNull)

                grdOcorrencias.Settings.ShowFilterRow = false;
        }

        #endregion


        #region Metodos

        private void RedirecionarPagina()
        {
            HttpContext.Current.Items.Add("chave", "1000");

            Server.Transfer("Ocorrencias.aspx");
        }

        private string MontarQueryString(string tipoOperacao, Ly_ocorrencia.Row dadosOcorrencia)
        {
            string queryString = string.Empty;

            queryString += "Operacao=" + tipoOperacao;
            queryString += "&aluno=" + dadosOcorrencia.Aluno;
            queryString += "&data=" + dadosOcorrencia.Data;
            queryString += "&tipo=" + dadosOcorrencia.Tipo;
            queryString += "&ordem=" + dadosOcorrencia.Ordem;

            return queryString;
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdOcorrencias.PageIndex * grdOcorrencias.SettingsPager.PageSize;
            for (int i = 0; i < grdOcorrencias.VisibleRowCount; i++)
            {
                if (grdOcorrencias.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    return startIndexOnPage + i;
                }
            }
            return -1;
        }

        private string ObterDadosQueryString(string queryString)
        {
            //ObjetoOcorrencia = new DadosOcorrencia();
            string[] listaDados = queryString.Split('&');
            string aluno = null;

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("aluno") >= 0)
                    aluno = dados.Substring(dados.LastIndexOf('=') + 1);
            }

            return aluno;
        }

        private void CarregaDadosAluno(string aluno)
        {
            QueryTable qt = RN.Ocorrencia.ConsultarDadosAluno(aluno);
            if (qt != null)
            {
                if (qt.Rows.Count > 0)
                {
                    txtCurso.Text = qt.Rows[0]["curso"].ToString();
                    txtTurno.Text = qt.Rows[0]["turno"].ToString();
                    txtSerie.Text = qt.Rows[0]["serie"].ToString();
                    txtUnidadeEnsino.Text = qt.Rows[0]["unidade_ensino"].ToString();
                    txtUnidadeFisica.Text = qt.Rows[0]["unidade_fisica"].ToString();
                }
            }
        }

        private void LimparCampos()
        {
            txtCurso.Text = string.Empty;
            txtTurno.Text = string.Empty;
            txtSerie.Text = string.Empty;
            txtUnidadeEnsino.Text = string.Empty;
            txtUnidadeFisica.Text = string.Empty;
        }
        #endregion


    }
}
