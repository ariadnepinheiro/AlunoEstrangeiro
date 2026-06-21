using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using Techne.Data;
using Techne.Controls;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/MeusAlunos.aspx"),
      ControlText("MeusAlunos"),
      Title("Meus Alunos")]
    public partial class MeusAlunos : TPage
    {
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
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMeusAlunos, "Alunos");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if ((!tseRegional.IsValidDBValue || tseRegional.DBValue.IsNull) && (!tseUnidadeEnsino.IsValidDBValue || tseUnidadeEnsino.DBValue.IsNull))
                tseTurma.SqlWhere = " gs.GRADE = null ";
            else
                tseTurma.SqlWhere = " (UE.ID_REGIONAL = #tseRegional# or #tseRegional# is null) AND (GS.UNIDADE_RESPONSAVEL = #tseUnidadeEnsino# or #tseUnidadeEnsino# is null) AND (GS.CURSO = #tseCurso# or #tseCurso# is null)";

            if (!IsPostBack)
            {
                tseAluno.Enabled = false;

                CarregarDadosDrop(ddlTurno.ID);
                ListItem item = new ListItem("<Nenhum>", "");
                ddlSerie.Items.Add(item);
                ddlSerie.SelectedValue = "";
            }
            ControlaRadios();
            CarregaAlunoBusca();
        }

        protected void rbPrimeiraOpcao_CheckedChanged(object sender, EventArgs e)
        {
            ControlaRadios();
        }

        protected void rbSegundaOpcao_CheckedChanged(object sender, EventArgs e)
        {
            ControlaRadios();
        }

		protected void tseUnidadeEnsino_Changed(object sender, ChangedEventArgs args)
        {
			tseTurma.ResetValue();
        }

        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
			tseTurma.ResetValue();
            CarregarDadosDrop(ddlSerie.ID);
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
			StringBuilder sqlWhere = new StringBuilder(string.Empty);
			tseTurma.ResetValue();
            CarregarDadosDrop(ddlSerie.ID);
			if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
				sqlWhere.Append(" UE.ID_REGIONAL = '" + tseRegional.DBValue.ToString() + "' ");
			if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull && 
				tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull)
				sqlWhere.Append(" AND GS.UNIDADE_RESPONSAVEL = '" + tseUnidadeEnsino.DBValue.ToString() + "' ");
			else if (tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull)
				sqlWhere.Append(" GS.UNIDADE_RESPONSAVEL = '" + tseUnidadeEnsino.DBValue.ToString() + "' ");
			if ((tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull ||
					tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull) &&
				tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull)
				sqlWhere.Append(" AND GS.CURSO = '" + tseCurso.DBValue.ToString() + "' ");
			else if (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull)
				sqlWhere.Append(" GS.CURSO = '" + tseCurso.DBValue.ToString() + "' ");
			if ((tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull ||
					tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull ||
					tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull) &&
				!String.IsNullOrEmpty(ddlTurno.SelectedValue))
				sqlWhere.Append(" AND GS.TURNO = '" + ddlTurno.SelectedValue + "' ");
			else if (!String.IsNullOrEmpty(ddlTurno.SelectedValue))
				sqlWhere.Append(" GS.TURNO = '" + ddlTurno.SelectedValue + "' ");
			if ((tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull ||
					tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull ||
					tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull ||
					!String.IsNullOrEmpty(ddlTurno.SelectedValue)) &&
				!String.IsNullOrEmpty(ddlSerie.SelectedValue))
				sqlWhere.Append(" AND GS.SERIE = '" + ddlSerie.SelectedValue + "' ");
			else if (!String.IsNullOrEmpty(ddlSerie.SelectedValue))
				sqlWhere.Append(" GS.SERIE = '" + ddlSerie.SelectedValue + "' ");
			tseTurma.SqlWhere = sqlWhere.ToString();
			tseTurma.DataBind();
        }

		protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
		{
			StringBuilder sqlWhere = new StringBuilder(string.Empty);
			tseTurma.ResetValue();
			if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
				sqlWhere.Append(" UE.ID_REGIONAL = '" + tseRegional.DBValue.ToString() + "' ");
			if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull &&
				tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull)
				sqlWhere.Append(" AND GS.UNIDADE_RESPONSAVEL = '" + tseUnidadeEnsino.DBValue.ToString() + "' ");
			else if (tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull)
				sqlWhere.Append(" GS.UNIDADE_RESPONSAVEL = '" + tseUnidadeEnsino.DBValue.ToString() + "' ");
			if ((tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull ||
					tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull) &&
				tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull)
				sqlWhere.Append(" AND GS.CURSO = '" + tseCurso.DBValue.ToString() + "' ");
			else if (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull)
				sqlWhere.Append(" GS.CURSO = '" + tseCurso.DBValue.ToString() + "' ");
			if ((tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull ||
					tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull ||
					tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull) &&
				!String.IsNullOrEmpty(ddlTurno.SelectedValue))
				sqlWhere.Append(" AND GS.TURNO = '" + ddlTurno.SelectedValue + "' ");
			else if (!String.IsNullOrEmpty(ddlTurno.SelectedValue))
				sqlWhere.Append(" GS.TURNO = '" + ddlTurno.SelectedValue + "' ");
			if ((tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull ||
					tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull ||
					tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull ||
					!String.IsNullOrEmpty(ddlTurno.SelectedValue)) &&
				!String.IsNullOrEmpty(ddlSerie.SelectedValue))
				sqlWhere.Append(" AND GS.SERIE = '" + ddlSerie.SelectedValue + "' ");
			else if (!String.IsNullOrEmpty(ddlSerie.SelectedValue))
				sqlWhere.Append(" GS.SERIE = '" + ddlSerie.SelectedValue + "' ");
			tseTurma.SqlWhere = sqlWhere.ToString();
			tseTurma.DataBind();
		}


        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            CarregaAlunoBusca("botao");
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            CarregaAlunoBusca();
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            tseUnidadeEnsino.ResetValue();
			tseTurma.ResetValue();
            if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
            {
                tseUnidadeEnsino.SqlWhere = "id_regional = '" + RN.RNBase.MudarAspas(tseRegional.DBValue.ToString()) + "'";

            }
            else
                tseUnidadeEnsino.SqlWhere = "";
        }

		protected void grdMeusAlunos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			CarregaAlunoBusca("gridCallback");
		}

        #endregion

        #region Metodos

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        private void ControlaRadios()
        {
            if (rbPrimeiraOpcao.Checked)
            {
                tseRegional.ResetValue();
                tseRegional.Mode = ControlMode.View;
                tseCurso.ResetValue();
                tseCurso.Mode = ControlMode.View;
                ddlTurno.Enabled = false;
                ddlSerie.Enabled = false;

				tseTurma.Mode = ControlMode.View;
				tseTurma.ResetValue();

                tseUnidadeEnsino.ResetValue();
                tseUnidadeEnsino.Mode = ControlMode.View;
                btnPesquisar.Visible = false;

                tseAluno.Enabled = true;

                ddlSerie.Items.Clear();
                ListItem item = new ListItem("<Nenhum>", "");
                ddlSerie.Items.Add(item);
                ddlSerie.SelectedValue = "";
            }
            else
            {
                tseAluno.ResetValue();
                tseAluno.Enabled = false;

                tseUnidadeEnsino.Mode = ControlMode.Edit;
                ddlTurno.Enabled = true;
                ddlSerie.Enabled = true;
                tseCurso.Mode = ControlMode.Edit;
                tseRegional.Mode = ControlMode.Edit;
				tseTurma.Mode = ControlMode.Edit;
                btnPesquisar.Visible = true;
            }
        }

        private void CarregaAlunoBusca(params string[] origem)
        {
            grdMeusAlunos.Selection.UnselectAll();

            if (rbPrimeiraOpcao.Checked)
            {
                if (tseAluno.IsValidDBValue && !tseAluno.DBValue.IsNull)
                {
                    string aluno = tseAluno.DBValue.ToString();
                    grdMeusAlunos.DataSource = RN.Aluno.ConsultarAlunos(aluno, null, null, null, null, null, null);
                    grdMeusAlunos.DataBind();
                    grdMeusAlunos.Visible = true;
                }
                else
                {
                    grdMeusAlunos.Visible = false;
                }
            }
            else
            {
				if (origem != null && origem.Length > 0 && (origem[0] == "botao" || origem[0] == "gridCallback"))
                    grdMeusAlunos.Visible = true;
                else
                {
                    grdMeusAlunos.Visible = false;
                    return;
                }
                
                string coordenadoria, unidadeEns, curso, turno, serie, turma;
				coordenadoria = unidadeEns = curso = turno = serie = turma = string.Empty;
				if (tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull)
					coordenadoria = tseRegional.DBValue.ToString();
				if (tseUnidadeEnsino.IsValidDBValue && !tseUnidadeEnsino.DBValue.IsNull)
					unidadeEns = tseUnidadeEnsino.DBValue.ToString();
				if (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull)
					curso = tseCurso.DBValue.ToString();
				if (!String.IsNullOrEmpty(ddlTurno.SelectedValue))
					turno = ddlTurno.SelectedValue;
				if (!String.IsNullOrEmpty(ddlSerie.SelectedValue))
					serie = ddlSerie.SelectedValue;
				if (tseTurma.IsValidDBValue && !tseTurma.DBValue.IsNull)
					turma = tseTurma.DBValue.ToString();
                grdMeusAlunos.DataSource = RN.Aluno.ConsultarMeusAlunos(coordenadoria, unidadeEns, curso, turno, serie, turma);
                grdMeusAlunos.DataBind();
            }
        }


        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLTURNO":
                        {
                            dadosDrop = RN.Turno.Consultar();

                            List<DropDownList> listaDrop = new List<DropDownList>();
                            listaDrop.Add(ddlSerie);

                            CarregarDropDownList(ddlTurno, dadosDrop, listaDrop, string.Empty);
                            break;
                        }
                    case "DDLSERIE":
                        {
                            string turno, curso, curriculo;

                            if (!string.IsNullOrEmpty(ddlTurno.SelectedValue) && !tseCurso.DBValue.IsNull)
                            {
                                if (tseCurso.IsValidDBValue)
                                {
                                    turno = ddlTurno.SelectedValue;
                                    curso = tseCurso.DBValue.ToString();

                                    curriculo = RN.Curriculo.Consultar(turno, curso, Convert.ToDecimal(DateTime.Now.Year));

                                    dadosDrop = RN.Serie.Consultar(curso, turno, curriculo);
                                }

                            }

                            List<DropDownList> listaDrop = new List<DropDownList>();
                            CarregarDropDownList(ddlSerie, dadosDrop, listaDrop, string.Empty);

                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return dadosDrop;
        }

        private void CarregarDropDownList(DropDownList drop, QueryTable data, List<DropDownList> listaDrop, string defaultValue)
        {
            drop.Items.Clear();
            drop.DataSource = data;
            drop.DataBind();

            ListItem item = new ListItem("<Nenhum>", "");
            drop.Items.Add(item);
            drop.SelectedValue = "";


            if (data != null)
            {
                if (data.Rows != null)
                {
                    if (data.Rows.Count > 0)
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
        #endregion
    }
}
