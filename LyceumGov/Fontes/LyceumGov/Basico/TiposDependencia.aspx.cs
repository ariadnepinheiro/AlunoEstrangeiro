using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Techne.Controls;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/TiposDependencia.aspx"),
      ControlText("Tipos de Dependencia"),
      Title("Tipos de Dependência"),
    ]

    public partial class TiposDependencia : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdTiposDependencia, "Tipos de Dependência");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		void Page_PreRenderComplete(object sender, EventArgs e)
		{
			ControlaAcesso(grdTiposDependencia);
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

		protected void grdTiposDependencia_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			ControlaAcesso(grdTiposDependencia);
		}

        protected void grdTiposDependencia_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdTiposDependencia.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "tipo_depend")
                    e.Editor.Enabled = true;
            }
            else if (grdTiposDependencia.IsEditing)
            {
                if ((e.Column.FieldName) == "tipo_depend")
                    e.Editor.Enabled = false;
            }
        }

        protected void grdTiposDependencia_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTiposDependencia.Settings.ShowFilterRow = false;
        }

        protected void grdTiposDependencia_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTiposDependencia.Settings.ShowFilterRow = false;
        }
    }
}
