using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/NivelCurso.aspx"),
        ControlText("NivelCurso"),
        Title("Nível de Ensino"),]
    public partial class NivelCurso : TPage
    {
		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdTiposCurso, "Níveis de Ensino");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		void Page_PreRenderComplete(object sender, EventArgs e)
		{
			ControlaAcesso(grdTiposCurso);
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

		protected void grdTiposCurso_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			ControlaAcesso(grdTiposCurso);
		}

        protected void grdTiposCurso_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdTiposCurso.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "tipo")
                {
                    e.Editor.Enabled = true;
                    e.Editor.ReadOnly = false;
                }
            }

            else if (grdTiposCurso.IsEditing)
            {
                if ((e.Column.FieldName) == "tipo")
                {
                    e.Editor.Enabled = false;
                    e.Editor.ReadOnly = true;
                }
            }
        }


        protected void grdTiposCurso_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTiposCurso.Settings.ShowFilterRow = false;
        }

        protected void grdTiposCurso_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTiposCurso.Settings.ShowFilterRow = false;
        }
    }
}
