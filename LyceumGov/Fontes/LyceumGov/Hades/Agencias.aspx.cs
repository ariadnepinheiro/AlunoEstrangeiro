using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using System.Web.UI.HtmlControls;

namespace Techne.Lyceum.Net.Hades
{
    [NavUrl("~/Hades/Agencias.aspx"),
      ControlText("Agencias"),
      Title("Agências"),]
    public partial class Agencias : TPage
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAgencia, "Agências");
        }

        protected void grdAgencia_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAgencia);
        }

        protected void grdAgencia_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["banco"] = tseBanco.DBValue;
            TSearchBox tseMunicipio = (TSearchBox)grdAgencia.FindEditRowCellTemplateControl((GridViewDataColumn)grdAgencia.Columns["municipio"], "tseMunicipio");
            if (tseMunicipio != null)
                e.NewValues["uf"] = tseMunicipio["uf_sigla"];
        }

        protected void grdAgencia_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            TSearchBox tseMunicipio = (TSearchBox)grdAgencia.FindEditRowCellTemplateControl((GridViewDataColumn)grdAgencia.Columns["municipio"], "tseMunicipio");
            if (tseMunicipio != null)
                e.NewValues["uf"] = tseMunicipio["uf_sigla"];
        }

        protected void tseBanco_Changed(object sender, EventArgs args)
        {
            grdAgencia.Visible = (tseBanco.IsValidDBValue && !tseBanco.DBValue.IsNull);
        }

        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {            
            TSearchBox tseMunicipio = (TSearchBox)grdAgencia.FindEditFormTemplateControl("tseMunicipio");
            HtmlInputText txtUF = (HtmlInputText)grdAgencia.FindEditFormTemplateControl("txtUF");
            
            if (!tseMunicipio.DBValue.IsNull)
            {
                if (tseMunicipio.IsValidDBValue) 
                {
                    txtUF.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
                }
            }
        }

        protected void grdAgencia_CustomColumnDisplayText(object sender, 
            DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "telefone" && e.Value != null)
            {
                decimal telefone = 0;
                if (decimal.TryParse(e.Value.ToString().Replace(" ", ""), out telefone))
                    e.DisplayText = string.Format(@"{0:(##)####-####}", telefone);
                else
                    e.DisplayText = "";
            }

            if (e.Column.FieldName == "cep" && e.Value != null)
            {
                decimal cep = 0;
                if (decimal.TryParse(e.Value.ToString().Replace(" ", ""), out cep))
                    e.DisplayText = string.Format(@"{0:#####-###}", cep);
                else
                    e.DisplayText = "";
            }
        }
    }
}
