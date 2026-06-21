using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using Techne.Controls;
using Techne.Web;
using Techne.Data;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;

namespace Techne.Lyceum.Net.Hades
{

    [NavUrl("~/Hades/Nacionalidade.aspx"),
    ControlText("Cadastro de Nacionalidades"),
    Title("Nacionalidades"),]


    public partial class Nacionalidade : TPage
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

		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdNacionalidades, "Nacionalidades");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
		}

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdNacionalidades);
        }

        protected void grdNacionalidades_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdNacionalidades.Settings.ShowFilterRow = false;
        }

        protected void grdNacionalidades_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdNacionalidades.Settings.ShowFilterRow = false;
        }

        protected void grdNacionalidades_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdNacionalidades.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "nacionalidade")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdNacionalidades.IsEditing)
            {
                if ((e.Column.FieldName) == "nacionalidade")
                {
                    e.Editor.Enabled = false;
                }
            }
        }

        protected void grdNacionalidades_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdNacionalidades);
        }
    }
}
