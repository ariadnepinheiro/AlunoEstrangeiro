using System;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/DisciplinasNaoExcluidasGLP.aspx"),
      ControlText("DisciplinasNaoExcluidasGLP"),
      Title("Disciplinas Não Canceladas de GLP"),]

    public partial class DisciplinasNaoExcluidasGLP : TPage
    {
		protected void Page_Init(object sender, EventArgs e)
		{
            TituloGrid(grdDisciplinas, "Disciplinas Não Canceladas de GLP");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		void Page_PreRenderComplete(object sender, EventArgs e)
		{
			ControlaAcesso(grdDisciplinas);
		}

		protected void grdDisciplinas_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			ControlaAcesso(grdDisciplinas);
		}

        protected void grdDisciplinas_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdDisciplinas.Settings.ShowFilterRow = false;
        }

        protected void grdDisciplinas_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["tipo_filtro"] = "DisciplinasNaoExcluidas";
        }
       
        protected void grdDisciplinas_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            DateTime hoje = DateTime.Today;
            DateTime milnov = new DateTime(1900, 1, 1);
            DateTime dataini = Convert.ToDateTime(e.NewValues["dt_inicio"]);
            DateTime datafim = Convert.ToDateTime(e.NewValues["dt_fim"]);
			if (dataini < hoje)
			{
				e.RowError = "Data Início não pode ser menor que data atual.";
				return;
			}
			if (dataini < milnov)
			{
				e.RowError = "Data de solicitação não pode ser menor que 1900.";
				return;
			}
			if (datafim < dataini)
			{
				e.RowError = "Data Final não pode ser menor que Data Início.";
				return;
			}
			if (e.NewValues["valor_filtro"] != null)
			{
				if (RN.Disciplina.ExisteDisciplinaNaoExcluidaGLP(e.NewValues["valor_filtro"].ToString(), dataini, datafim))
				{
					e.RowError = "Período de tempo sobreposto a um já cadastrado no mesmo grupo de disciplinas.";
					return;
				}
			}
        }

        protected void grdDisciplinas_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "dt_inicio")
            {
                (e.Editor as ASPxDateEdit).MinDate = DateTime.Today;
            }
        }
    }
}


