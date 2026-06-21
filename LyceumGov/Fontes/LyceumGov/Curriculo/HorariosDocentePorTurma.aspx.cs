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


namespace Techne.Lyceum.Net.Curriculo
{
    [NavUrl("~/Curriculo/HorariosDocentePorTurma.aspx"),
 ControlText("HorariosDocentePorTurma"),
 Title("Horários do Docente por Turma"),]

    public partial class HorariosDocentePorTurma : TPage
    {
		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdHorarrioDocente, "Horários do Docente");
		}

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    lblGradeID.Text = decodedText;
                    ObterDadosQueryString(decodedText);

                }
                else
                    Response.Redirect("TesteHorario.aspx");
                //Response.Redirect("ListarTurma.aspx");

                ConsultarDadosTurma();
                CarregarDadosDrop("ddlDocentes");
            }
		}

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdHorarrioDocente);
        }

        [Serializable]
        private class DadosTurma
        {
            public DadosTurma()
            {

            }
            private string _turma;
            public string Turma
            {
                get { return _turma; }
                set { _turma = value; }
            }

        }

        private DadosTurma ObjetoTurma
        {
            get { return (DadosTurma)ViewState["ObjetoTurma"]; }
            set { ViewState["ObjetoTurma"] = value; }
        }

        private void ObterDadosQueryString(string grade_id)
        {
            ObjetoTurma = new DadosTurma();

            if (!string.IsNullOrEmpty(grade_id))
            {
                ObjetoTurma.Turma = grade_id;
                lblGradeID.Text = grade_id;
            }

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



        public object Listar(string docente, string gradeid)
        {
            QueryTable dadosGrid = null;
            if (!string.IsNullOrEmpty(docente))
            {
                dadosGrid = RN.Docentes.ConsultarHorarios(gradeid, docente);
            }

            return dadosGrid;
        }


        protected void ConsultarDadosTurma()
        {
            QueryTable qt = null;
            qt = RN.GradeSerie.ConsultarDadosTurma(lblGradeID.Text);

            if (qt.Rows.Count > 0)
            {
                txtAno.Text = qt.Rows[0]["ano"].ToString();
                txtPeriodo.Text = qt.Rows[0]["semestre"].ToString();
                txtTurno.Text = qt.Rows[0]["turno"].ToString();
                txtCurso.Text = qt.Rows[0]["curso"].ToString();
                txtUnidadeEnsino.Text = qt.Rows[0]["unidade"].ToString();
                txtSérie.Text = qt.Rows[0]["serie"].ToString();
                txtTurma.Text = qt.Rows[0]["grade"].ToString(); //isso é a turma?
            }
        }

        protected void LimparCampos()
        {
            txtAno.Text = string.Empty;
            txtPeriodo.Text = string.Empty;
            txtTurno.Text = string.Empty;
            txtCurso.Text = string.Empty;
            txtUnidadeEnsino.Text = string.Empty;
            txtSérie.Text = string.Empty;
            txtTurma.Text = string.Empty;
        }

        public void Delete() { }

        public void Insert() { }

        public void Update() { }



        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();
            ListItem itemVazio = new ListItem("<Nenhum>", "");
            drop.Items.Add(itemVazio);
            drop.SelectedValue = "";
        }


        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLDOCENTES":
                        {
                            dadosDrop = RN.GradeSerie.ConsultarDocentesTurma(lblGradeID.Text);
                            CarregarDropDownList(ddlDocentes, dadosDrop, string.Empty);
                            break;
                        }
                }
            }
            catch
            {
                throw;
            }
            return dadosDrop;
        }

        protected void ddlDocentes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDocentes.SelectedValue != "")
                grdHorarrioDocente.Visible = true;
            else
                grdHorarrioDocente.Visible = false;
        }


    }// fim da classe
}// fim do namespace

