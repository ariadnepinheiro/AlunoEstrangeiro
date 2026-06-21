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
using System.Configuration;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using Techne.Data;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/Titulacao.aspx"),
      ControlText("Titulacao"),
      Title("Titulação"),
    ]

    public partial class Titulacao : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdTitulacao, "Titulações");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		void Page_PreRenderComplete(object sender, EventArgs e)
		{
			ControlaAcesso(grdTitulacao);
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

		protected void grdTitulacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			ControlaAcesso(grdTitulacao);
		}

        protected void grdTitulacao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdTitulacao.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "titulacao")
                    e.Editor.Enabled = true;

            }
            else if (grdTitulacao.IsEditing)
            {
                if ((e.Column.FieldName) == "titulacao")
                    e.Editor.Enabled = false;
            }
        }

        protected void grdTitulacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTitulacao.Settings.ShowFilterRow = false;
        }

        protected void grdTitulacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTitulacao.Settings.ShowFilterRow = false;
        }

        protected void grdTitulacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["titulacao"] = e.NewValues["titulacao"].ToString().Trim(); 
        }

        protected void grdTitulacao_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (grdTitulacao.IsNewRowEditing == true)
            {
                if (e.NewValues["titulacao"] != null)
                {
                    QueryTable qt = RN.Titulacao.VerificarTitulacao(Convert.ToString(e.NewValues["titulacao"]));
                    if (qt.Rows.Count > 0)
                    {
                        e.RowError = "Titulação já cadastrada.";
                    }
                }
            }
        }        
    }
}

