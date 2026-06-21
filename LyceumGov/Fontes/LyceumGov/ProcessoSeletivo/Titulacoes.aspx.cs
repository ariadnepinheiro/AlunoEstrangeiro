using System;
using DevExpress.Web.ASPxGridView;
using Techne.Web;


namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [NavUrl("~/ProcessoSelectivo/Titulacoes.aspx"),
    ControlText("Titulações"),
    Title("Titulações para Processos Seletivos"),]

    public partial class Titulacoes : TPage
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

        protected void Page_Load(object sender, EventArgs e)
        {
			TituloGrid();
		}

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTitulacoes);
        }

		private void TituloGrid()
		{
			string tituloGrade = grdTitulacoes.SettingsText.Title;
			if (tituloGrade != string.Empty) grdTitulacoes.SettingsText.Title = tituloGrade.Replace("|Tabela:|", "Titulações para Processos Seletivos");
		}

        protected void grdTitulacoes_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTitulacoes.Settings.ShowFilterRow = false;
        }

        protected void grdTitulacoes_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTitulacoes.Settings.ShowFilterRow = false;
        }

        protected void grdTitulacoes_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdTitulacoes.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "titulacao")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdTitulacoes.IsEditing)
            {
                if ((e.Column.FieldName) == "titulacao")
                {
                    e.Editor.Enabled = false;
                }
            }
        }

        protected void grdTitulacoes_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTitulacoes);
        }
    }
}
