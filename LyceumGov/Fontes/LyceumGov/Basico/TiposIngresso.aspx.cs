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
        NavUrl("~/Basico/TiposIngresso.aspx"),
        ControlText("TiposIngresso"),
        Title("Tipos de Ingresso"),
    ]
    
    public partial class TiposIngresso : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
                return
                Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
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

		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdTiposIngresso, "Tipos de Ingresso");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		void Page_PreRenderComplete(object sender, EventArgs e)
		{
			ControlaAcesso(grdTiposIngresso);
		}

		protected void grdTiposIngresso_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			ControlaAcesso(grdTiposIngresso);
		}

        protected void grdTiposIngresso_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdTiposIngresso.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "tipo_ingresso")
                    e.Editor.Enabled = true;

                if ((e.Column.FieldName) == "descricao")
                    e.Editor.Enabled = true;
            }
            else
            {
                if ((e.Column.FieldName) == "tipo_ingresso")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "descricao")
                    e.Editor.Enabled = true;
            }
        }

        protected void grdTiposIngresso_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("tipo_ingresso", e.OldValues["tipo_ingresso"]);
        }

        protected void grdTiposIngresso_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("tipo_ingresso", e.Values["tipo_ingresso"]);
        }

        protected void grdTiposIngresso_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTiposIngresso.Settings.ShowFilterRow = false;
        }

        protected void grdTiposIngresso_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTiposIngresso.Settings.ShowFilterRow = false;
        }

        protected void grdTiposIngresso_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string aux = e.NewValues["tipo_ingresso"].ToString();
            e.NewValues["tipo_ingresso"] = aux.Trim();
        }

    }
}
