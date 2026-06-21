using System;
using System.Web.UI;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using Techne.Lyceum.RN;

namespace Techne.Lyceum.Net.Hades
{
    [
        NavUrl("~/Hades/TabelaGeral.aspx"),
        ControlText("TabelaGeral"),
        Title("Tabela Geral"),
    ]
    public partial class TabelaGeral : TPage
    {
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
            TituloGrid(grdTabela, "Tabelas");
            TituloGrid(grdItem, "Itens");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTabela);
            ControlaAcesso(grdItem);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ASPxGridView.RegisterBaseScript(Page);
            string tabela = ObtemTabela();
            if (!IsPostBack && !IsCallback)
            {
                grdTabela.FocusedRowIndex = 0;
            }
            else
            {
                grdTabela.FocusedRowIndex = grdTabela.FocusedRowIndex;



                if (!string.IsNullOrEmpty(tabela))
                {
                    tdsItem.SqlWhere = "hd_tabelaitem.tabela = '" + RN.RNBase.MudarAspas(tabela) + "'";
                }
                else
                {
                    tdsItem.SqlWhere = "hd_tabelaitem.tabela = ''";
                }

                tdsItem.Select();
                grdItem.DataBind();
            }

            if (!grdTabela.IsNewRowEditing && !grdTabela.IsEditing)
            {
                grdItem.Columns[""].Visible = true;
            }
            if (!grdItem.IsNewRowEditing && !grdItem.IsEditing)
            {
                grdTabela.Columns[""].Visible = true;
            }
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


        #region NãoPermitirAlteração
        protected void grdItem_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdItem.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "tabela")
                    e.Editor.Enabled = true;

                if ((e.Column.FieldName) == "item")
                    e.Editor.Enabled = true;
            }
            else if (grdItem.IsEditing)
            {
                if ((e.Column.FieldName) == "tabela")
                    e.Editor.Enabled = false;

                if ((e.Column.FieldName) == "item")
                    e.Editor.Enabled = false;
            }
        }
        protected void grdTabela_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdTabela.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "tabela")
                    e.Editor.Enabled = true;
            }
            else if (grdTabela.IsEditing)
            {
                if ((e.Column.FieldName) == "tabela")
                    e.Editor.Enabled = false;
            }
        }
        #endregion

        #region CompositeKey
        protected void grdItem_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string tabela = Convert.ToString(e.GetListSourceFieldValue("tabela"));
                string item = Convert.ToString(e.GetListSourceFieldValue("item"));
                e.Value = tabela + "-" + item;
            }
        }

        protected void grdItem_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            e.Keys.Clear();
            e.Keys.Add("tabela", e.Values["tabela"]);
            e.Keys.Add("item", e.Values["item"]);
        }

        protected void grdItem_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string[] chaves = e.Keys[0].ToString().Split('-');

            e.Keys.Clear();

            e.Keys.Add("tabela", chaves[0]);
            e.Keys.Add("item", chaves[1]);
        }

        protected void grdItem_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["item"] = e.NewValues["item"].ToString().Trim();  
        }
        #endregion

        #region Seleção da grid
        protected void tdsTabela_Selecting(object sender, Techne.Controls.TTableDataSourceSelectingEventArgs e)
        {
            if (this.grdTabela.FocusedRowIndex != -1)
            {
                string tabela = ObtemTabela();
                if (!string.IsNullOrEmpty(tabela))
                {
                    tdsItem.SqlWhere = "hd_tabelaitem.tabela = '" + RN.RNBase.MudarAspas(tabela) + "'";
                }
                else
                {
                    tdsItem.SqlWhere = "hd_tabelaitem.tabela = ''";
                }

                tdsItem.DataBind();
                grdItem.DataBind();
            }

        }

        protected void grdTabela_FocusedRowChanged(object sender, EventArgs e)
        {
            string tabela = ObtemTabela();

            if (!IsPostBack && !IsCallback)
            {
                grdTabela.FocusedRowIndex = 0;

                if (!string.IsNullOrEmpty(tabela))
                {
                    tdsItem.SqlWhere = "hd_tabelaitem.tabela = '" + RN.RNBase.MudarAspas(tabela) + "'";
                }
                else
                {
                    tdsItem.SqlWhere = "hd_tabelaitem.tabela = ''";
                }

                tdsItem.Select();
                tdsItem.DataBind();
                grdTabela.DataBind();
                grdItem.DataBind();

            }
            else
            {
                grdTabela.FocusedRowIndex = ((DevExpress.Web.ASPxGridView.ASPxGridView)sender).FocusedRowIndex;

                if (!string.IsNullOrEmpty(tabela))
                {
                    tdsItem.SqlWhere = "hd_tabelaitem.tabela = '" + RN.RNBase.MudarAspas(tabela) + "'";
                }
                else
                {
                    tdsItem.SqlWhere = "hd_tabelaitem.tabela = ''";
                }

                tdsItem.Select();
                tdsItem.DataBind();
                grdItem.DataBind();
                grdItem.ExpandAll();

            }

            lblTabela.Text = "Tabela selecionada: " + tabela;
            grdItem.CancelEdit();
            grdTabela.CancelEdit();

        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdTabela.PageIndex * grdTabela.SettingsPager.PageSize;
            for (int i = 0; i < grdTabela.VisibleRowCount; i++)
            {
                if (grdTabela.FocusedRowIndex == startIndexOnPage + i)
                    return startIndexOnPage + i;
            }
            return -1;
        }

        private string ObtemTabela()
        {
            //obtém o indice atual da seleção
            int curPageSelection = GetSelectedRowOnTheCurrentPage();
            string tabela;
            
            tabela = (string)grdTabela.GetRowValues(grdTabela.FocusedRowIndex, "tabela");

            return tabela;
        }
        #endregion

        #region EsconderFiltro
        protected void grdTabela_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTabela.Settings.ShowFilterRow = false;
            grdItem.Columns[""].Visible = false;
            grdItem.CancelEdit();
        }

        protected void grdTabela_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTabela.Settings.ShowFilterRow = false;
            grdItem.Columns[""].Visible = false;
            grdItem.CancelEdit();
        }

        protected void grdItem_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdItem.Settings.ShowFilterRow = false;

            try
            {
                string tabela = ObtemTabela();
                e.NewValues["tabela"] = tabela;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            grdTabela.Columns[""].Visible = false;
        }

        protected void grdItem_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdItem.Settings.ShowFilterRow = false;
            grdTabela.Columns[""].Visible = false;
        }
        #endregion


        protected void grdTabela_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["sis"] = "Lyceum";
            e.NewValues["tabela"] = e.NewValues["tabela"].ToString().Trim();  
        }

        protected void grdItem_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string tabela = ObtemTabela();
            if (!RN.TabelaGeral.ExisteTabela(tabela))
            {
                e.RowError = "A tabela selecionada já foi removida.";
                return;
            }

            if (tabela == "Verba Maxima")
            {
                if (grdItem.IsNewRowEditing)
                {
                    if (RN.TabelaGeral.ExisteItemTabela(tabela))
                    {
                        e.RowError = "Não pode haver mais de um item cadastrado nesta tabela. Edite o existente ou remova o existente e insira um novo.";
                        return;
                    }
                }

                if (!Validacao.Validou(Convert.ToString(e.NewValues["descr"]), Validacao.Tipo.dinheiroGrande))
                {
                    e.RowError = "O valor de verba máxima deve ser numérico com no máximo 10 casas inteiras e exatamente 2 casas decimais separadas por vírgula.";
                    return;
                }

                
            }
        }

        protected void grdTabela_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            grdItem.Columns[""].Visible = true;
        }

        protected void grdTabela_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            grdItem.Columns[""].Visible = true;
        }

        protected void grdTabela_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            grdItem.Columns[""].Visible = true;
        }

        protected void grdTabela_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdItem.Columns[""].Visible = true;
        }

        protected void grdItem_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e)
        {
            grdTabela.Columns[""].Visible = true;
        }

        protected void grdItem_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            grdTabela.Columns[""].Visible = true;
        }

        protected void grdItem_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            grdTabela.Columns[""].Visible = true;
        }

        protected void grdItem_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTabela.Columns[""].Visible = true;
        }

        protected void grdTabela_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                if (e.Values["tabela"] != null)
                {
                    if (RN.TabelaGeral.ExisteItemTabela(e.Values["tabela"].ToString()))
                    {
                        e.Cancel = true;
                        throw new Exception("Não é possível remover esta tabela, pois esta possui item tabela vinculados a ela.");
                    }
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception("Não foi possível encontrar a tabela a ser removida.");
                }
                e.Keys.Add("sis", "Lyceum");
            }
            catch (Exception ex)
            {
                this.updateError = ex.Message;
                e.Cancel = true;
            }
        }

        protected void grdTabela_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpUpdateError"] = this.updateError;
        }

        protected void grdTabela_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTabela);
        }

        protected void grdItem_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdItem);
        }



    }
}

