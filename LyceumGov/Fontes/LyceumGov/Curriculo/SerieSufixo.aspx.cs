using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.Data;
using Techne.Controls;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Curriculo
{
    [
       NavUrl("~/Curriculo/SerieSufixo.aspx"),
       ControlText("Sufixo do Ano de Escolaridade"),
       Title("Sufixo do Ano de Escolaridade"),
    ]
    public partial class SerieSufixo : TPage
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
        

        private void InitializeComponent()
        {
        }
        #endregion
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSerieSufixo, "Sufixos do Ano de Escolaridade");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                grdSerieSufixo.Visible = false;
        }       

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdSerieSufixo);
            ControlaVisibilidadeGrid();            
        }

        private void ControlaVisibilidadeGrid()
        {
            if (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue && !string.IsNullOrEmpty(ddlTurno.SelectedValue) && !string.IsNullOrEmpty(ddlCurriculo.SelectedValue) && !string.IsNullOrEmpty(ddlSerie.SelectedValue))
            {
                grdSerieSufixo.Visible = true;
            }
            else
            {
                grdSerieSufixo.Visible = false;
                grdSerieSufixo.CancelEdit();
            }
        }                 

        protected void grdSerieSufixo_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSerieSufixo);
        }

        protected void grdSerieSufixo_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.NewValues["curso"] = tseCurso.DBValue.ToString();
            e.NewValues["turno"] = ddlTurno.SelectedValue;
            e.NewValues["curriculo"] = ddlCurriculo.SelectedValue;
            e.NewValues["serie"] = ddlSerie.SelectedValue;            
        }
               
        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddlCurriculo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

    }
}
