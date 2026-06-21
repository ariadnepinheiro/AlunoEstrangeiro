using System;
using System.Web.UI;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Data;

namespace Techne.Lyceum.Net.Servico
{
    [
        NavUrl("~/Servico/Servicos.aspx"),
        ControlText("Servicos"),
        Title("Definição do Fluxo de Solicitações"),
    ]
    public partial class Servicos : TPage
    {

        private object servico
        {
            get { return (object)ViewState["servico"]; }
            set { ViewState["servico"] = value; }
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

        string updateError = "";
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
			TituloGrid(grdServicos, "Serviços");
			TituloGrid(grdFluxos, "Passos");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string servico = ObtemServico();

            ASPxGridView.RegisterBaseScript(Page);
            if (!IsPostBack && !IsCallback)
            {
                grdServicos.FocusedRowIndex = 0;
            }
            else
            {
                grdServicos.FocusedRowIndex = grdServicos.FocusedRowIndex;


                if (!string.IsNullOrEmpty(servico))
                {
                    tdsFluxos.SqlWhere = "ly_fluxo_de_andamento.servico = '" + RN.RNBase.MudarAspas(servico) + "'";
                }
                else
                {
                    tdsFluxos.SqlWhere = "ly_fluxo_de_andamento.servico = ''";
                }

                tdsFluxos.Select();
                grdFluxos.DataBind();
            }

            if (!grdServicos.IsNewRowEditing && !grdServicos.IsEditing)
            {
                grdFluxos.Columns[""].Visible = true;
            }
            if (!grdFluxos.IsNewRowEditing && !grdFluxos.IsEditing)
            {
                grdServicos.Columns[""].Visible = true;
            }


		}

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdServicos);
            ControlaAcesso(grdFluxos);
        }


        protected void grdServicos_FocusedRowChanged(object sender, EventArgs e)
        {
            string servico = ObtemServico();

            if (!IsPostBack && !IsCallback)
            {
                grdServicos.FocusedRowIndex = 0;

                if (!string.IsNullOrEmpty(servico))
                {
                    tdsFluxos.SqlWhere = "ly_fluxo_de_andamento.servico = '" + RN.RNBase.MudarAspas(servico) + "'";
                }
                else
                {
                    tdsFluxos.SqlWhere = "ly_fluxo_de_andamento.servico = ''";
                }

                tdsFluxos.Select();
                tdsFluxos.DataBind();
                grdServicos.DataBind();
                grdFluxos.DataBind();

            }
            else
            {
                grdServicos.FocusedRowIndex = ((DevExpress.Web.ASPxGridView.ASPxGridView)sender).FocusedRowIndex;

                if (!string.IsNullOrEmpty(servico))
                {
                    tdsFluxos.SqlWhere = "ly_fluxo_de_andamento.servico = '" + RN.RNBase.MudarAspas(servico) + "'";
                }
                else
                {
                    tdsFluxos.SqlWhere = "ly_fluxo_de_andamento.servico = ''";
                }

                tdsFluxos.Select();
                tdsFluxos.DataBind();
                grdFluxos.DataBind();
                grdFluxos.ExpandAll();
            }
            lblServico.Text = "Serviço selecionado: " + servico;
            grdServicos.CancelEdit();
            grdFluxos.CancelEdit();
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdServicos.PageIndex * grdServicos.SettingsPager.PageSize;
            for (int i = 0; i < grdServicos.VisibleRowCount; i++)
            {
                if (grdServicos.FocusedRowIndex == startIndexOnPage + i)
                    return startIndexOnPage + i;
            }
            return -1;
        }

        private string ObtemServico()
        {
            //obtém o indice atual da seleção
            int curPageSelection = GetSelectedRowOnTheCurrentPage();
            string servico;

            servico = (string)grdServicos.GetRowValues(curPageSelection, new string[] { grdServicos.KeyFieldName });

            return servico;
        }

        #region EsconderFiltro
        protected void grdServicos_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdServicos.Settings.ShowFilterRow = false;
            grdFluxos.Columns[""].Visible = false;
            grdFluxos.CancelEdit();
        }

        protected void grdServicos_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdServicos.Settings.ShowFilterRow = false;
            grdFluxos.Columns[""].Visible = false;
            grdFluxos.CancelEdit();
        }

        protected void grdFluxos_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdFluxos.Settings.ShowFilterRow = false;
            grdServicos.Columns[""].Visible = false;
        }
        #endregion

        protected void grdServicos_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdServicos.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "servico")
                    e.Editor.ReadOnly = false;
            }
            else if (grdServicos.IsEditing)
            {
                if ((e.Column.FieldName) == "servico")
                    e.Editor.ReadOnly = true;
            }
        }

        protected void grdFluxos_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            if (grdFluxos.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "servico")
                    e.Editor.ReadOnly = false;

                if ((e.Column.FieldName) == "passo")
                    e.Editor.ReadOnly = false;
            }
            else if (grdFluxos.IsEditing)
            {
                if ((e.Column.FieldName) == "servico")
                    e.Editor.ReadOnly = true;

                if ((e.Column.FieldName) == "passo")
                    e.Editor.ReadOnly = true;
            }
        }


        #region Grid
        protected void grdServicos_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            grdFluxos.Columns[""].Visible = true;
            grdServicos.FocusedRowIndex = 0;
        }



        protected void grdServicos_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdFluxos.Columns[""].Visible = true;
        }

        protected void grdFluxos_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            grdServicos.Columns[""].Visible = true;
        }

        protected void grdFluxos_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            grdServicos.Columns[""].Visible = true;
        }

        protected void grdFluxos_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            grdServicos.Columns[""].Visible = true;
        }

        protected void grdFluxos_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdServicos.Columns[""].Visible = true;
        }
        #endregion



        protected void grdServicos_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpUpdateError"] = this.updateError;
        }

        protected void grdServicos_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["solicitavel"] = "S";
            e.NewValues["revisao_prova"] = "N";
            e.NewValues["prazo_entrega"] = 0;
            e.NewValues["solicitavel_web"] = "S";
        }

        protected void tdsServicos_Selecting(object sender, Techne.Controls.TTableDataSourceSelectingEventArgs e)
        {
            if (this.grdServicos.FocusedRowIndex != -1)
            {
                string servico = ObtemServico();
                if (!string.IsNullOrEmpty(servico))
                {
                    tdsFluxos.SqlWhere = "ly_fluxo_de_andamento.servico = '" + RN.RNBase.MudarAspas(servico) + "'";
                }
                else
                {
                    tdsFluxos.SqlWhere = "ly_fluxo_de_andamento.servico = ''";
                }

                tdsFluxos.DataBind();
                grdFluxos.DataBind();
            }
        }

        protected void grdFluxos_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdFluxos.Settings.ShowFilterRow = false;
            grdServicos.Columns[""].Visible = false;
        }

        protected void grdFluxos_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                string servico = ObtemServico();
                e.NewValues["servico"] = servico;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdServicos_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            grdFluxos.Columns[""].Visible = true;
            servico = e.NewValues["servico"];
        }

        protected void grdServicos_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            grdFluxos.Columns[""].Visible = true;
            servico = e.NewValues["servico"];
        }

        protected void grdServicos_DataBound(object sender, EventArgs e)
        {
            if (servico != null)
            {
                grdServicos.FocusedRowIndex = grdServicos.FindVisibleIndexByKeyValue(servico);
                servico = null;
            }
        }

        protected void grdServicos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdServicos);
        }

        protected void grdFluxos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdFluxos);
        }




    }
}



