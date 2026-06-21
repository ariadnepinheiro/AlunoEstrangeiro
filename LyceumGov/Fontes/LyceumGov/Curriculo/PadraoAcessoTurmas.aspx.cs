using System;
using DevExpress.Web.ASPxGridView;
using Techne.Controls;
using Techne.Web;


namespace Techne.Lyceum.Net.Curriculo
{
    [
     NavUrl("~/Curriculo/PadraoAcessoTurmas.aspx"),
      ControlText("PadraoAcessoTurmas"),
      Title("Padrão de Acesso das Turmas"),
    ]

    public partial class PadraoAcessoTurmas : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdPadaces, "Padrões de Acesso e Períodos de Criação de Turmas");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            
		}

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPadaces);
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

        #region Eventos da Grid



        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
            grdPadaces.CancelEdit();
            if (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull)
            {
                lblMensagem.Text = string.Empty;
                grdPadaces.Visible = true;

            }
            else if (!tseCurso.DBValue.IsNull)
            {
                lblMensagem.Text = "Escolaridade não cadastrada.";
                grdPadaces.Visible = false;
            }
            else
            {
                lblMensagem.Text = "Favor consultar uma escolaridade.";
                grdPadaces.Visible = false;
            }
        }


        protected void grdPadaces_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdPadaces.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "padaces")
                    e.Editor.ReadOnly = false;

                if ((e.Column.FieldName) == "operacao")
                    e.Editor.ReadOnly = false;

            }
            else if (grdPadaces.IsEditing)
            {
                if ((e.Column.FieldName) == "padaces")
                    e.Editor.ReadOnly = true;

                if ((e.Column.FieldName) == "operacao")
                    e.Editor.ReadOnly = true;
            }
        }

        protected void grdPadaces_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
                e.NewValues["curso"] = tseCurso.DBValue.ToString();
        }

        protected void grdPadaces_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            DateTime dataini = Convert.ToDateTime(e.NewValues["dt_inicio"]);
            DateTime datafim = Convert.ToDateTime(e.NewValues["dt_fim"]);
            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if (dataini < hoje)
                e.RowError = "Data Início deve ser maior ou igual a data atual.";

            if (datafim < hoje)
                e.RowError = "Data Fim deve ser maior ou igual a data atual.";

            if(datafim < dataini)
                e.RowError = "Data Fim não pode ser menor que Data Início.";

        }

        protected void grdPadaces_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPadaces.Settings.ShowFilterRow = false;
        }

        protected void grdPadaces_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("curso", e.Values["curso"]);
            e.Keys.Add("padaces", e.Values["padaces"]);
            e.Keys.Add("operacao", e.Values["operacao"]);
        }

        protected void grdPadaces_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            if (tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull)
                e.NewValues["curso"] = tseCurso.DBValue.ToString();

            grdPadaces.Settings.ShowFilterRow = false;
        }

        protected void grdPadaces_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string curso = tseCurso.DBValue.ToString();
            e.Keys.Clear();
            e.Keys.Add("curso", curso);
            e.Keys.Add("padaces", e.OldValues["padaces"]);
            e.Keys.Add("operacao", e.OldValues["operacao"]);
        }

        protected void grdPadaces_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string curso = Convert.ToString(e.GetListSourceFieldValue("curso"));
                string padaces = Convert.ToString(e.GetListSourceFieldValue("padaces"));
                string oper = Convert.ToString(e.GetListSourceFieldValue("operacao"));
                e.Value = curso + "-" + padaces + "-" + oper;

            }
        }
        #endregion  

        protected void grdPadaces_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPadaces);
        }
    }
}

