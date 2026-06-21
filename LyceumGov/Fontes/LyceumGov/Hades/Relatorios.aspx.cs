using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using System.Data;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.Hades
{
    [
        NavUrl("~/Hades/Relatorios.aspx"),
        ControlText("Relatorios"),
        Title("Cadastro de Relatórios"),
    ]
    public partial class Relatorios : TPage
    {

        public string controle
        {
            get
            {
                return (string)ViewState["controle"];
            }
            set
            {
                ViewState["controle"] = value;
            }
        }
		protected void Page_Init(object sender, EventArgs e)
		{
			TituloGrid(grdGrupoRelatorio, "Grupos de Relatório");
			TituloGrid(grdRelatorio, "Relatórios");
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsCallback)
            {
                grdGrupoRelatorio.FocusedRowIndex = 0;

                string grupoRelatorio = ObtemGrupoRelatorio();

                if (!string.IsNullOrEmpty(grupoRelatorio))
                {
                    //aplica o filtro de acordo com o valor obtido da chave de grupo de conceito
                    tdsRelatorio.SqlWhere = "gruporelat = '" + RN.RNBase.MudarAspas(grupoRelatorio) + "' and sis = 'LyceumNet'";
                    lblRelatorio.Text = "Grupo selecionado: " + grupoRelatorio;
                }
                else
                {
                    tdsRelatorio.SqlWhere = "gruporelat = '' and sis = 'LyceumNet'";
                    lblRelatorio.Text = "";
                }

                tdsRelatorio.Select();
                tdsRelatorio.DataBind();
                grdGrupoRelatorio.DataBind();
                grdRelatorio.DataBind();
            }
            else
            {
                grdGrupoRelatorio.FocusedRowIndex = grdGrupoRelatorio.FocusedRowIndex;

                string grupoRelatorio = ObtemGrupoRelatorio();

                if (!string.IsNullOrEmpty(grupoRelatorio))
                {
                    tdsRelatorio.SqlWhere = "gruporelat = '" + RN.RNBase.MudarAspas(grupoRelatorio) + "' and sis = 'LyceumNet'";
                }
                else
                {
                    tdsRelatorio.SqlWhere = "gruporelat = '' and sis = 'LyceumNet'";
                }

                tdsRelatorio.Select();
                grdRelatorio.DataBind();
            }

            if (!grdGrupoRelatorio.IsNewRowEditing && !grdGrupoRelatorio.IsEditing)
            {
                grdRelatorio.Columns[""].Visible = true;
            }
            if(!grdRelatorio.IsNewRowEditing && !grdRelatorio.IsEditing)
            {
                grdGrupoRelatorio.Columns[""].Visible = true;
            }

            ControlaAcesso(grdGrupoRelatorio);
            ControlaAcesso(grdRelatorio);

            if (grdGrupoRelatorio.IsEditing)
            {
                DevExpress.Web.ASPxClasses.Internal.RenderUtils.LoadPostDataRecursive(grdGrupoRelatorio, Request.Params, true);
            }
		}

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdGrupoRelatorio);
            ControlaAcesso(grdRelatorio);
        }

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

        private string ObtemGrupoRelatorio()
        {
            //obtém o indice atual da seleção
            int curPageSelection = GetSelectedRowOnTheCurrentPage();
            string grupoRelatorio = (string)grdGrupoRelatorio.GetRowValues(grdGrupoRelatorio.FocusedRowIndex, "gruporelat");

            return grupoRelatorio;
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdGrupoRelatorio.PageIndex * grdGrupoRelatorio.SettingsPager.PageSize;
            for (int i = 0; i < grdGrupoRelatorio.VisibleRowCount; i++)
            {
                if (grdGrupoRelatorio.FocusedRowIndex == startIndexOnPage + i)
                    return startIndexOnPage + 1;
            }
            return -1;
        }

        #region grdGrupoRelatorio

        protected void grdGrupoRelatorio_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdGrupoRelatorio.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "gruporelat")
                    e.Editor.Enabled = true;
            }
            else if (grdGrupoRelatorio.IsEditing)
            {
                if ((e.Column.FieldName) == "gruporelat")
                    e.Editor.Enabled = false;
            }
        }

        protected void grdGrupoRelatorio_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            //controle = "I";

            //if (e.Parameters.ToUpper() == "FALSE")
            //{
            //    grdGrupoRelatorio.Columns[""].Visible = false;
            //    grdGrupoRelatorio.Enabled = false;
            //    grdGrupoRelatorio.DataBind();
            //}
            //else
            //{
            //    grdGrupoRelatorio.Columns[""].Visible = true;
            //    grdGrupoRelatorio.Enabled = true;
            //    grdGrupoRelatorio.DataBind();
            //}
        }

        protected void grdGrupoRelatorio_CustomJSProperties(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            //e.Properties["cpIsNewRowEditing"] = (sender as ASPxGridView).IsNewRowEditing.ToString();
            //e.Properties["cpIsEditing"] = (sender as ASPxGridView).IsEditing.ToString();
            //if (controle != "I")
            //{
            //    e.Properties["cpControle"] = "T";
            //    controle = "T";
            //}
            //else
            //{
            //    e.Properties["cpControle"] = "I";
            //}
        }

        protected void grdGrupoRelatorio_FocusedRowChanged(object sender, EventArgs e)
        {
            //grdGrupoRelatorio.FocusedRowIndex = ((DevExpress.Web.ASPxGridView.ASPxGridView)sender).FocusedRowIndex;
            if (!IsPostBack && !IsCallback)
            {
                //grdGrupoRelatorio.FocusedRowIndex = 0;

                string grupoRelatorio = ObtemGrupoRelatorio();

                if (!string.IsNullOrEmpty(grupoRelatorio))
                {
                    //aplica o filtro de acordo com o valor obtido da chave de grupo de conceito
                    tdsRelatorio.SqlWhere = "gruporelat = '" + RN.RNBase.MudarAspas(grupoRelatorio) + "' and sis = 'LyceumNet'";
                    lblRelatorio.Text = "Grupo selecionado: " + grupoRelatorio;
                }
                else
                {
                    tdsRelatorio.SqlWhere = "gruporelat = '' and sis = 'LyceumNet'";
                    lblRelatorio.Text = "Grupo selecionado: Nenhum";
                }

                tdsRelatorio.Select();
                tdsRelatorio.DataBind();
                grdGrupoRelatorio.DataBind();
                grdRelatorio.DataBind();

            }
            else
            {
                grdGrupoRelatorio.FocusedRowIndex = ((DevExpress.Web.ASPxGridView.ASPxGridView)sender).FocusedRowIndex;

                string grupoRelatorio = ObtemGrupoRelatorio();

                if (!string.IsNullOrEmpty(grupoRelatorio))
                {
                    //aplica o filtro de acordo com o valor obtido da chave de grupo de conceito
                    tdsRelatorio.SqlWhere = "gruporelat = '" + RN.RNBase.MudarAspas(grupoRelatorio) + "' and sis = 'LyceumNet'";
                    lblRelatorio.Text = "Grupo selecionado: " + grupoRelatorio;
                }
                else
                {
                    tdsRelatorio.SqlWhere = "gruporelat = '' and sis = 'LyceumNet'";
                    lblRelatorio.Text = "Grupo selecionado: Nenhum";
                }

                tdsRelatorio.Select();
                tdsRelatorio.DataBind();
                grdRelatorio.DataBind();
                grdRelatorio.ExpandAll();
            }
            grdRelatorio.CancelEdit();
            grdGrupoRelatorio.CancelEdit();
        }

        protected void grdGrupoRelatorio_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdGrupoRelatorio.Settings.ShowFilterRow = false;
            grdRelatorio.Columns[""].Visible = false;
            grdRelatorio.CancelEdit();
        }

        protected void grdGrupoRelatorio_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdGrupoRelatorio.Settings.ShowFilterRow = false;
            grdRelatorio.Columns[""].Visible = false;
            grdRelatorio.CancelEdit();
        }

        protected void grdGrupoRelatorio_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["sis"] = "LyceumNet";
        }

        #endregion
        #region grdRelatorio

        protected void grdRelatorio_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (grdGrupoRelatorio.FocusedRowIndex == -1)
            {
                grdRelatorio.Columns[""].Visible = false;
                grdRelatorio.DataBind();
            }
        }

        protected void grdRelatorio_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdRelatorio.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "gruporelat")
                    e.Editor.Enabled = true;

                if ((e.Column.FieldName) == "relatorio")
                    e.Editor.Enabled = true;
            }
            else if (grdRelatorio.IsEditing)
            {
                if ((e.Column.FieldName) == "gruporelat")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "relatorio")
                    e.Editor.Enabled = false;
            }
        }

        protected void grdRelatorio_CustomCallback1(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            //controle = "T";

            //if (e.Parameters.ToUpper() == "FALSE")
            //{
            //    grdRelatorio.Columns[""].Visible = false;
            //    grdRelatorio.DataBind();
            //}
            //else
            //{
            //    grdRelatorio.Columns[""].Visible = true;
            //    grdRelatorio.DataBind();
            //}
        }

        protected void grdRelatorio_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpIsNewRowEditing"] = (sender as ASPxGridView).IsNewRowEditing.ToString();
            e.Properties["cpIsEditing"] = (sender as ASPxGridView).IsEditing.ToString();
            //if (controle != "T")
            //{
            //    e.Properties["cpControle"] = "I";
            //    controle = "I";
            //}
            //else
            //{
            //    e.Properties["cpControle"] = "T";
            //}
        }

        protected void grdRelatorio_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string sis = "LyceumNet";
                string grupo = Convert.ToString(e.GetListSourceFieldValue("gruporelat"));
                string relatorio = Convert.ToString(e.GetListSourceFieldValue("relatorio"));
                e.Value = sis + "|" + grupo + "|" + relatorio;
            }
        }

        protected void grdRelatorio_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdRelatorio.Settings.ShowFilterRow = false;
            string grupoRel = ObtemGrupoRelatorio();
            if (grupoRel == string.Empty)
            {
                grdRelatorio.CancelEdit();
            }
            else
            {
                e.NewValues["gruporelat"] = grupoRel;
            }
            grdGrupoRelatorio.Columns[""].Visible = false;
        }

        protected void grdRelatorio_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string[] chaves = e.Keys[0].ToString().Split('|');

            e.Keys.Clear();

            e.Keys.Add("sis", chaves[0]);
            e.Keys.Add("gruporelat", chaves[1]);
            e.Keys.Add("relatorio", chaves[2]);
        }

        protected void grdRelatorio_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                string grupoRelatorio = ObtemGrupoRelatorio();
                
                e.NewValues["gruporelat"] = grupoRelatorio;
                e.NewValues["sis"] = "LyceumNet";
                e.NewValues["auditar"] = "N";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdRelatorio_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys[0].ToString().Split('|');

            e.Keys.Clear();

            e.Keys.Add("sis", chaves[0]);
            e.Keys.Add("gruporelat", chaves[1]);
            e.Keys.Add("relatorio", chaves[2]);
        }

        protected void grdRelatorio_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string grupoRelatorio = ObtemGrupoRelatorio();
            if (!RN.Relatorio.ExisteGrupoRelatorio(grupoRelatorio))
            {
                e.RowError = "O grupo selecionado já foi removido.";
                grdRelatorio.CancelEdit();
                return;
            }
        }

        protected void grdRelatorio_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdRelatorio.Settings.ShowFilterRow = false;
            grdGrupoRelatorio.Columns[""].Visible = false;
        }

        #endregion

        protected void tdsGrupoRelatorio_Selecting(object sender, Techne.Controls.TTableDataSourceSelectingEventArgs e)
        {
            if (this.grdGrupoRelatorio.FocusedRowIndex != -1)
            {
                string grupoRelatorio = ObtemGrupoRelatorio();
                if (!string.IsNullOrEmpty(grupoRelatorio))
                {
                    tdsRelatorio.SqlWhere = "gruporelat = '" + RN.RNBase.MudarAspas(grupoRelatorio) + "' and sis = 'LyceumNet'";
                }
                else
                {
                    tdsRelatorio.SqlWhere = "gruporelat = '' and sis = 'LyceumNet'";
                }

                tdsRelatorio.DataBind();
                grdRelatorio.DataBind();
            }
        }

        protected void grdGrupoRelatorio_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.Keys.Add("sis", "LyceumNet");
        }

        protected void grdGrupoRelatorio_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            if (e.Values["gruporelat"] != null)
            {
                if (!RN.Relatorio.PermiteRemoverGrupoRelatorio(e.Values["gruporelat"].ToString()))
                {
                    e.Cancel = true;
                    throw new Exception("Não é possível remover este grupo, pois este possui relatório vinculados a ele.");
                }
            }
            else
            {
                e.Cancel = true;
                throw new Exception("Não foi possível encontrar o grupo a ser removido.");
            }
            e.Keys.Add("sis", "LyceumNet");
        }

        protected void grdGrupoRelatorio_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdRelatorio.Columns[""].Visible = true;
        }

        protected void grdGrupoRelatorio_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            grdRelatorio.Columns[""].Visible = true;
        }

        protected void grdGrupoRelatorio_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            grdRelatorio.Columns[""].Visible = true;
        }

        protected void grdGrupoRelatorio_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            grdRelatorio.Columns[""].Visible = true;
        }

        protected void grdRelatorio_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            grdGrupoRelatorio.Columns[""].Visible = true;
        }

        protected void grdRelatorio_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            grdGrupoRelatorio.Columns[""].Visible = true;
        }

        protected void grdRelatorio_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdGrupoRelatorio.Columns[""].Visible = true;
        }

        protected void grdGrupoRelatorio_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdGrupoRelatorio);
        }

        protected void grdRelatorio_AfterPerformCallback1(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRelatorio);
        }
    }
}
