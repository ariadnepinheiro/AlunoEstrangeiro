using System;
using DevExpress.Web.ASPxGridView;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/Funcoes.aspx"),
      ControlText("Funcoes"),
      Title("Funções"),]
    public partial class Funcoes : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdFuncoes, "Funções");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdFuncoes);
        }

        protected void grdFuncoes_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdFuncoes);
        }

        protected void grdFuncoes_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdFuncoes.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "funcao")
                {
                    e.Editor.Enabled = true;
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "campo_01")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_02")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_03")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_04")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_05")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_07")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_08")
                {
                    e.Editor.Enabled = true;
                }

                if ((e.Column.FieldName) == "campo_09")
                {
                    e.Editor.Enabled = true;
                }

                if ((e.Column.FieldName) == "campo_10")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "SEMCARGAHORARIAEFETIVA")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "ATIVO")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdFuncoes.IsEditing)
            {
                if ((e.Column.FieldName) == "funcao")
                {
                    e.Editor.Enabled = false;
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "campo_01")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_02")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_03")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_04")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_05")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_07")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "campo_08")
                {
                    e.Editor.Enabled = true;
                }

                if ((e.Column.FieldName) == "campo_09")
                {
                    e.Editor.Enabled = true;
                }

                if ((e.Column.FieldName) == "campo_10")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "SEMCARGAHORARIAEFETIVA")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "ATIVO")
                {
                    e.Editor.Enabled = true;
                }
            }
        }

        protected void grdFuncoes_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdFuncoes.Settings.ShowFilterRow = false;
        }
       
        protected void grdFuncoes_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdFuncoes.Settings.ShowFilterRow = false;
        }

        protected void grdFuncoes_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["funcao"] = RN.Basico.GeraFuncao();

            if (e.NewValues["campo_01"] == null)
            {
                e.NewValues["campo_01"] = "N";
            }
            if (e.NewValues["campo_02"] == null)
            {
                e.NewValues["campo_02"] = "N";
            }
            if (e.NewValues["campo_03"] == null)
            {
                e.NewValues["campo_03"] = "N";
            }
            if (e.NewValues["campo_04"] == null)
            {
                e.NewValues["campo_04"] = "N";
            }
            if (e.NewValues["campo_05"] == null)
            {
                e.NewValues["campo_05"] = "N";
            }
            if (e.NewValues["campo_07"] == null)
            {
                e.NewValues["campo_07"] = "N";
            }
            if (e.NewValues["campo_08"] == null)
            {
                e.NewValues["campo_08"] = "N";
            }
            if (e.NewValues["campo_09"] == null)
            {
                e.NewValues["campo_09"] = "N";
            }
            if (e.NewValues["campo_10"] == null)
            {
                e.NewValues["campo_10"] = "N";
            }
            if (e.NewValues["SEMCARGAHORARIAEFETIVA"] == null)
            {
                e.NewValues["SEMCARGAHORARIAEFETIVA"] = "N";
            }
            if (e.NewValues["ATIVO"] == null)
            {
                e.NewValues["ATIVO"] = "N";
            }
        }

        protected void grdFuncoes_AutoFilterCellEditorCreate(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "campo_01")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }

            if (e.Column.FieldName == "campo_02")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }

            if (e.Column.FieldName == "campo_03")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }

            if (e.Column.FieldName == "campo_04")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }

            if (e.Column.FieldName == "campo_05")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }

            if (e.Column.FieldName == "campo_07")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }

            if (e.Column.FieldName == "campo_08")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }

            if (e.Column.FieldName == "campo_09")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }

            if (e.Column.FieldName == "campo_10")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }
            if (e.Column.FieldName == "SEMCARGAHORARIAEFETIVA")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }
            if (e.Column.FieldName == "ATIVO")
            {
                DevExpress.Web.ASPxEditors.ASPxComboBox combo = e.Editor as DevExpress.Web.ASPxEditors.ASPxComboBox;
                combo.Items.Clear();
                DevExpress.Web.ASPxGridView.GridViewDataCheckColumn check = e.Column as DevExpress.Web.ASPxGridView.GridViewDataCheckColumn;
                combo.ValueType = check.PropertiesCheckEdit.ValueType;
                combo.Items.Add(string.Empty, null);
                combo.Items.Add("Marcado", check.PropertiesCheckEdit.ValueChecked);
                combo.Items.Add("Desmarcado", check.PropertiesCheckEdit.ValueUnchecked);
            }
        }

        protected void grdFuncoes_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (e.NewValues["campo_01"] == null)
            {
                e.NewValues["campo_01"] = "N";
            }
            if (e.NewValues["campo_02"] == null)
            {
                e.NewValues["campo_02"] = "N";
            }
            if (e.NewValues["campo_03"] == null)
            {
                e.NewValues["campo_03"] = "N";
            }
            if (e.NewValues["campo_04"] == null)
            {
                e.NewValues["campo_04"] = "N";
            }
            if (e.NewValues["campo_05"] == null)
            {
                e.NewValues["campo_05"] = "N";
            }
            if (e.NewValues["campo_07"] == null)
            {
                e.NewValues["campo_07"] = "N";
            }
            if (e.NewValues["campo_08"] == null)
            {
                e.NewValues["campo_08"] = "N";
            }
            if (e.NewValues["campo_09"] == null)
            {
                e.NewValues["campo_09"] = "N";
            }
            if (e.NewValues["campo_10"] == null)
            {
                e.NewValues["campo_10"] = "N";
            }
            if (e.NewValues["SEMCARGAHORARIAEFETIVA"] == null)
            {
                e.NewValues["SEMCARGAHORARIAEFETIVA"] = "N";
            }
            if (e.NewValues["ATIVO"] == null)
            {
                e.NewValues["ATIVO"] = "N";
            }
        }

        protected void grdFuncoes_CustomColumnDisplayText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDisplayTextEventArgs e)
        {
        }

        protected void grdFuncoes_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            string descricao = Convert.ToString(e.NewValues["descricao"]).Trim();
            string funcao = Convert.ToString(grdFuncoes.GetRowValues(grdFuncoes.EditingRowVisibleIndex, "funcao")).Trim();
            if (grdFuncoes.IsNewRowEditing)
            {
                if (RN.Funcao.ExisteDescricao(descricao))
                    e.RowError = "Já existe uma função cadastrada com essa descrição.";
            }
            else
            {
                if (RN.Funcao.ExisteDescricaoAlteracao(descricao, funcao))
                    e.RowError = "Já existe uma função cadastrada com essa descrição.";
            }


            // Não pode alterar o CAMPO_07 (PERMITE GLP) se existe solicitação ativa 
            string oldPermiteGLP = Convert.ToString(e.OldValues["campo_07"]);
            if (String.IsNullOrEmpty(oldPermiteGLP)) oldPermiteGLP = "N";

            string newPermiteGLP = Convert.ToString(e.NewValues["campo_07"]);
            if (String.IsNullOrEmpty(newPermiteGLP)) newPermiteGLP = "N";

            if (oldPermiteGLP != newPermiteGLP)
            {
                if (RN.Funcao.PossuiLotacaoComSolicitacaoGLP(funcao))
                    e.RowError = "Não é possível alterar \"Permite GLP?\". Existem solicitações de GLP ativas de docentes cuja lotação é desta função.";
            }

            string oldSemCHEfetiva = Convert.ToString(e.OldValues["SEMCARGAHORARIAEFETIVA"]);
            if (String.IsNullOrEmpty(oldSemCHEfetiva)) oldSemCHEfetiva = "N";

            string newSemCHEfetiva = Convert.ToString(e.NewValues["SEMCARGAHORARIAEFETIVA"]);
            if (String.IsNullOrEmpty(newPermiteGLP)) newSemCHEfetiva = "N";

            if (oldSemCHEfetiva != newSemCHEfetiva)
            {
                if (RN.Funcao.PossuiLotacaoComSolicitacaoGLP(funcao))
                    e.RowError = "Não é possível alterar \"Sem CH Efetiva?\". Existem solicitações de GLP ativas de docentes cuja lotação é desta função.";
            }
        }

        //// Função para exportar DataTable para Excel

        protected void Button1_Click_ExportarButton1_Click(object sender, EventArgs e)
        {
            
           // RN.Protocolo.TipoProtocolo rnTipoProtocolo = new Techne.Lyceum.RN.Protocolo.TipoProtocolo();
            RN.Funcao rnFuncoesColuna = new Techne.Lyceum.RN.Funcao();
            RN.Util.ExportaExcel rnExportaExcel = new Techne.Lyceum.RN.Util.ExportaExcel();
            System.Data.DataTable consulta = new System.Data.DataTable();

            try
            {
                // Valida campos obrigatórios para listagem
                if (true) // (Validar()) 
                {
                    // Busca lista para exportar
                    //consulta = rnTipoProtocolo.ListaTipoProtocolo();
                    consulta = rnFuncoesColuna.ListaExcel();
   

                    // Verifica se existem itens para exportar
                    if (consulta.Rows.Count > 0)
                    {
                        // Chama a função para exportar para Excel
                       // ExportaExcelParaNavegador(consulta);
                        rnExportaExcel.ExportaDataTablePor(consulta, "FUNÇÕES", "Funcoes");

                    }
                    else
                    {
                        // lblMensagem.Text = "Não existem dados à serem exportados para o excel.";
                    }
                }
            }
            catch (Exception ex)
            {
                // lblMensagem.Text = ex.Message;
            }
        }

        // Função para exportar DataTable para Excel (Formato CSV)
        private void ExportaExcelParaNavegador(System.Data.DataTable consulta)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Buffer = true;
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Funcoes.csv");
            System.Web.HttpContext.Current.Response.Charset = "";

            // Criar o conteúdo CSV
            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                // Criar o escritor de texto HTML
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

                // Adicionar cabeçalho (nomes das colunas)
                for (int i = 0; i < consulta.Columns.Count; i++)
                {
                    sw.Write(consulta.Columns[i].ColumnName);
                    if (i < consulta.Columns.Count - 1)
                        sw.Write(",");
                }
                sw.Write("\r\n");         


                // aqui deve tratar para criar linhas e colunas


                
                // Adicionar os dados (linhas)
                foreach (System.Data.DataRow row in consulta.Rows)
                {
                    for (int i = 1; i < consulta.Columns.Count; i++)
                    {
                        sw.Write(row[i].ToString());
                        if (i < consulta.Columns.Count - 1)
                            sw.Write(",");
                    }
                    sw.Write("\r\n");
                }

                // Escrever o conteúdo do CSV na resposta HTTP
                System.Web.HttpContext.Current.Response.Write(sw.ToString());
                System.Web.HttpContext.Current.Response.End();
            }
        }
    }
}