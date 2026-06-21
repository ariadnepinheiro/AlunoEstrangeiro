using System;
using DevExpress.Web.ASPxGridView;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/TiposTransportes.aspx"),
      ControlText("TiposTransportes"),
      Title("Tipos de Transportes"),
    ]

    public partial class TiposTransportes : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdTiposTransporte, "Tipos de Transportes");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		void Page_PreRenderComplete(object sender, EventArgs e)
		{
			ControlaAcesso(grdTiposTransporte);
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

		protected void grdTiposTransporte_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			ControlaAcesso(grdTiposTransporte);
		}

        protected void grdTiposTransporte_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdTiposTransporte.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "tipo")
                    e.Editor.Enabled = true;
            }
            else if (grdTiposTransporte.IsEditing)
            {
                if ((e.Column.FieldName) == "tipo")
                    e.Editor.Enabled = false;
            }
        }

        protected void grdTiposTransporte_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTiposTransporte.Settings.ShowFilterRow = false;
        }

        protected void grdTiposTransporte_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTiposTransporte.Settings.ShowFilterRow = false;
        }
    }
}

