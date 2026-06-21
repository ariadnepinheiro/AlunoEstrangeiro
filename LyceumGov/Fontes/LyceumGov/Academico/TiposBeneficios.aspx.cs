using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/TiposBeneficios.aspx"),
    ControlText("TiposBenefícios"),
    Title("Tipos de Benefícios"),]
    public partial class TiposBeneficios : TPage
    {
        #region Código Padrão Techne
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
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTiposBeneficios, "Tipos de Benefícios");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_RenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTiposBeneficios);
        }

        #region Métodos da Grid
        protected void grdTiposBeneficios_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTiposBeneficios);
        }

        protected void grdTiposBeneficios_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTiposBeneficios.Settings.ShowFilterRow = false;
        }

        protected void grdTiposBeneficios_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            grdTiposBeneficios.Settings.ShowFilterRow = false;
            string codigo = e.NewValues["tipo_beneficio"].ToString().Trim();
            string codigoOld = e.OldValues["tipo_beneficio"] == null ? null : e.OldValues["tipo_beneficio"].ToString().Trim();
            if (grdTiposBeneficios.IsNewRowEditing)
            {
                if (RN.TipoBeneficio.ExisteCodigo(codigo))
                    e.RowError = "Já existe um tipo de benefício cadastrado com este código.";
            }
            else if (grdTiposBeneficios.IsEditing)
            {
                if (codigo != codigoOld)
                {
                    if (RN.TipoBeneficio.ExisteCodigo(codigo))
                        e.RowError = "Já existe um tipo de benefício cadastrado com este código.";
                }

                if (RN.TipoBeneficio.ExisteProgramaBeneficio(codigo))
                    throw new ApplicationException("Não é possível alterar o código do tipo de benefício.\nExiste programa social da unidade cadastrado para ele.");
            }
        }

        protected void grdTiposBeneficios_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTiposBeneficios.Settings.ShowFilterRow = false;
        }


        protected void grdTiposBeneficios_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string codigo = e.Values["tipo_beneficio"].ToString().Trim();

            //verifica se benefício é usado em programas sociais da unidade
            if (RN.TipoBeneficio.ExisteProgramaBeneficio(codigo))
                throw new ApplicationException("Não é possível excluir o tipo de benefício.\nExiste programa social da unidade cadastrado para ele.");
        }
        #endregion
    }
}
