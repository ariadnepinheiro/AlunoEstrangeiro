using System;
using DevExpress.Web.ASPxGridView;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{

    [NavUrl("~/ProcessoSelectivo/Experiencias.aspx"),
    ControlText("Experiências"),
    Title("Experiências para Processos Seletivos"),]


    public partial class Experiencias : TPage
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
            ControlaAcesso(grdExperiencias);
        }

		private void TituloGrid()
		{
			string tituloGrade = grdExperiencias.SettingsText.Title;
			if (tituloGrade != string.Empty) grdExperiencias.SettingsText.Title = tituloGrade.Replace("|Tabela:|", "Experiências para Processos Seletivos");
		}

        protected void grdExperiencias_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdExperiencias.Settings.ShowFilterRow = false;
        }

        protected void grdExperiencias_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdExperiencias.Settings.ShowFilterRow = false;
        }

        protected void grdExperiencias_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdExperiencias.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "experiencia")
                {
                    e.Editor.Enabled = true;
                }
				if (e.Column.FieldName == "origem")
				{
					e.Editor.Value = "N";
				}
            }
            else if (grdExperiencias.IsEditing)
            {
                if ((e.Column.FieldName) == "experiencia")
                {
                    e.Editor.Enabled = false;
                }
            }
        }

        protected void grdExperiencias_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdExperiencias);
        }
    }
}
