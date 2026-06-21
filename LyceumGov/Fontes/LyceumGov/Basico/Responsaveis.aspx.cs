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
     NavUrl("~/Basico/Responsaveis.aspx"),
      ControlText("Responsaveis"),
      Title("Papel dos Responsáveis"),
    ]

    public partial class Responsaveis : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdPapelPessoa, "Responsáveis");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		void Page_PreRenderComplete(object sender, EventArgs e)
		{
			ControlaAcesso(grdPapelPessoa);
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

		protected void grdPapelPessoa_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			ControlaAcesso(grdPapelPessoa);
		}

        protected void grdPapelPessoa_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdPapelPessoa.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "papel")
                    e.Editor.Enabled = true;

            }
            else if (grdPapelPessoa.IsEditing)
            {
                if ((e.Column.FieldName) == "papel")
                    e.Editor.Enabled = false;

            }

        }

        protected void grdPapelPessoa_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdPapelPessoa.Settings.ShowFilterRow = false;
        }

        protected void grdPapelPessoa_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdPapelPessoa.Settings.ShowFilterRow = false;
        }

        protected void grdPapelPessoa_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["papel"] = e.NewValues["papel"].ToString().Trim();  
        }

        protected void grdPapelPessoa_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string papel = e.Values["papel"].ToString().ToUpper();
            if (papel == "IRMÃO" || papel == "MÃE" || papel == "PAI")
            {
                throw new Exception("Não é possível remover este tipo de responsável.");
            }
        }
    }
}


