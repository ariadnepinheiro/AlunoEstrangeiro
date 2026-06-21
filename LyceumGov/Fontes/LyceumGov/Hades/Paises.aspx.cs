using System;
using DevExpress.Web.ASPxGridView;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{

    [NavUrl("~/Hades/Paises.aspx"),
    ControlText("Cadastro de Países"),
    Title("Países"),]


    public partial class Paises : TPage
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
			TituloGrid(grdPaises, "Países");
		}

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPaises);
        }

        protected void grdPaises_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPaises.Settings.ShowFilterRow = false;
        }

        protected void grdPaises_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPaises.Settings.ShowFilterRow = false;
        }

        protected void grdPaises_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdPaises.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "pais")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdPaises.IsEditing)
            {
                if ((e.Column.FieldName) == "pais")
                {
                    e.Editor.Enabled = false;
                }
            }
        }

        protected void grdPaises_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPaises);
        }
    }
}
