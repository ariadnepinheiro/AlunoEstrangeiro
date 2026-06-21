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
     NavUrl("~/Basico/ModalidadeCurso.aspx"),
      ControlText("ModalidadeCurso"),
      Title("Modalidade de Ensino"),
    ]

    public partial class ModalidadeCurso : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdSubnivel, "Modalidades");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

		void Page_PreRenderComplete(object sender, EventArgs e)
		{
			ControlaAcesso(grdSubnivel);
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

		protected void grdSubnivel_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
		{
			ControlaAcesso(grdSubnivel);
		}

        protected void grdSubnivel_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdSubnivel.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "modalidade")
                    e.Editor.Enabled = true;

                if ((e.Column.FieldName) == "descricao")
                    e.Editor.Enabled = true;

                if ((e.Column.FieldName) == "sistema_avaliacao")
                    e.Editor.Enabled = true;
            }
            else
            {
                if (grdSubnivel.IsEditing)
                {
                    if ((e.Column.FieldName) == "modalidade")
                        e.Editor.Enabled = false;

                    if ((e.Column.FieldName) == "descricao")
                        e.Editor.Enabled = true;

                    if ((e.Column.FieldName) == "sistema_avaliacao")
                        e.Editor.Enabled = true;
                }
            }

        }

        protected void grdSubnivel_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdSubnivel.Settings.ShowFilterRow = false;
        }

        protected void grdSubnivel_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdSubnivel.Settings.ShowFilterRow = false;
        }
    }
}