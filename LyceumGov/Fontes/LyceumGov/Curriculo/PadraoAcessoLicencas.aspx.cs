using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
    [NavUrl("~/Curriculo/PadraoAcessoLicencas.aspx"),
      ControlText("Padrão de Acesso das Licenças"),
      Title("Padrão de Acesso das Licenças")]
    public partial class PadraoAcessoLicencas : TPage
    {
        #region Código gerado Techne
        public static string GetUrl()
        {
            
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }
        #endregion

        private void InitializeComponent()
        {
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAcesso);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAcesso, "Licenças");
        }

		protected void grdAcesso_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
		{
			grdAcesso.Settings.ShowFilterRow = false;
		}

		protected void grdAcesso_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
		{
			grdAcesso.Settings.ShowFilterRow = false;
		}

        protected void grdAcesso_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["padaces"] = ddlPadaces.SelectedValue;
        }

        protected void grdAcesso_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAcesso);
        }
    }
}
