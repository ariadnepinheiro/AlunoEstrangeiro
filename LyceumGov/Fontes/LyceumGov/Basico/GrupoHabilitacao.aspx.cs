using System;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/GrupoHabilitacao.aspx"),
      ControlText("GrupoHabilitação"),
      Title("Grupos de Habilitações"),
    ]
    public partial class GrupoHabilitacao : TPage
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
			TituloGrid(grdGrupoHabilitacao, "Grupos de Habilitações");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdGrupoHabilitacao);
        }

        protected void grdGrupoHabilitacao_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (CadastroNovoGrupoHabilitacao(e))
                e.Editor.Enabled = true;
            else if (EdicaoGrupoHabilitacao(e))
                e.Editor.Enabled = false;
            
            if (grdGrupoHabilitacao.IsNewRowEditing)
            {
                if (e.Column.FieldName == "tipo")
                {
                    e.Editor.Value = "Disciplina";
                }
            }
        }

        private bool EdicaoGrupoHabilitacao(ASPxGridViewEditorEventArgs e)
        {
            return grdGrupoHabilitacao.IsEditing && (e.Column.FieldName) == "agrupamento";
        }

        private bool CadastroNovoGrupoHabilitacao(ASPxGridViewEditorEventArgs e)
        {
            return grdGrupoHabilitacao.IsNewRowEditing && (e.Column.FieldName) == "agrupamento";
        }

        protected void grdGrupoHabilitacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdGrupoHabilitacao.Settings.ShowFilterRow = false;
        }

        protected void grdGrupoHabilitacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdGrupoHabilitacao.Settings.ShowFilterRow = false;
        }

        protected void grdGrupoHabilitacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            string aux = e.NewValues["agrupamento"].ToString();
            e.NewValues["agrupamento"] = aux.Trim();
            string ingresso = e.NewValues["ingresso"] == null ? "N" : e.NewValues["ingresso"].ToString();
            e.NewValues["ingresso"] = ingresso;
            e.NewValues["ativo"] = e.NewValues["ativo"] == null ? "N" : e.NewValues["ativo"].ToString();
            e.NewValues["usuario"] = User.Identity.Name;
            e.NewValues["datacadastro"] = DateTime.Now;
            e.NewValues["dataalteracao"] = DateTime.Now;
        }

        protected void grdGrupoHabilitacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.NewValues["usuario"] = User.Identity.Name;
            e.NewValues["dataalteracao"] = DateTime.Now;
        }

        protected void grdGrupoHabilitacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdGrupoHabilitacao);
        }

        protected void grdGrupoHabilitacao_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            RN.AulaDocente rnAulaDocente = new Techne.Lyceum.RN.AulaDocente();

            if ( Convert.ToString(e.OldValues["ativo"]) == "S" &&  Convert.ToString(e.NewValues["ativo"]) == "N")
            {
                if (rnAulaDocente.ExisteAulaVigenteNoGrupoHabilitacaoDisciplinaPor( Convert.ToString(e.Keys["agrupamento"])))
                {
                    e.RowError = "Este grupo não pode ser desativado, pois existe aula vigente para este grupo de disciplina.";
                    return;
                }
            }
        }
    }
}
