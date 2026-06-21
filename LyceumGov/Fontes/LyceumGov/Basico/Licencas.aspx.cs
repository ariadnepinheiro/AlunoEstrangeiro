using System;
using Techne.Web;
using Techne.Lyceum.RN;


namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/Licencas.aspx"),
    ControlText("Licencas"),
    Title("Licenças"),]

    public partial class Licencas : TPage
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
            TituloGrid(grdLicencas, "Licenças");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void grdLicencas_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (e.NewValues["possui_dtfim"] == null)
            {
                e.NewValues["possui_dtfim"] = "N";
                e.NewValues["periodo_limite"] = null;
            }
            if (e.NewValues["participacontratotemporario"] == null)
            {
                e.NewValues["participacontratotemporario"] = "N";
            }
            if (e.NewValues["validaalocacao"] == null)
            {
                e.NewValues["validaalocacao"] = "S";
            }
            e.NewValues["motivo"] = e.NewValues["motivo"].ToString().Trim();

            String motivo = Convert.ToString(e.NewValues["motivo"]);
            String descricao = Convert.ToString(e.NewValues["descricao"]);
            String possui_dtfim = Convert.ToString(e.NewValues["possui_dtfim"]);
            String periodo_limite = Convert.ToString(e.NewValues["periodo_limite"]).Trim();
            String bloqueia_glp = Convert.ToString(e.NewValues["bloqueia_glp"]);
            String participaCT = Convert.ToString(e.NewValues["participacontratotemporario"]);
            String validaAlocacao = Convert.ToString(e.NewValues["validaalocacao"]);

            RetValue retInsercao = RN.Licencas.InserirLicenca(motivo, descricao, possui_dtfim, periodo_limite, bloqueia_glp, participaCT, validaAlocacao);
            if (retInsercao != null && !retInsercao.Ok)
                throw new ApplicationException(retInsercao.Errors.ToString());            
        }

        protected void grdLicencas_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            if (e.NewValues["possui_dtfim"] == null)
            {
                e.NewValues["possui_dtfim"] = "N";
                e.NewValues["periodo_limite"] = null;
            }
            else if (e.NewValues["possui_dtfim"].ToString() == "N")
            {
                e.NewValues["periodo_limite"] = null;
            }

            e.NewValues["motivo"] = e.NewValues["motivo"].ToString().Trim();
            if (e.NewValues["periodo_limite"] != null)
            {
                e.NewValues["periodo_limite"] = e.NewValues["periodo_limite"].ToString().Trim();
            }

            if (e.NewValues["participacontratotemporario"] == null)
            {
                e.NewValues["participacontratotemporario"] = "N";
            }
            if (e.NewValues["validaalocacao"] == null)
            {
                e.NewValues["validaalocacao"] = "S";
            }
            String oldMotivo = Convert.ToString(e.OldValues["motivo"]);
            String motivo = Convert.ToString(e.NewValues["motivo"]);
            String descricao = Convert.ToString(e.NewValues["descricao"]);
            String possui_dtfim = Convert.ToString(e.NewValues["possui_dtfim"]);
            String periodo_limite = Convert.ToString(e.NewValues["periodo_limite"]);
            String bloqueia_glp = Convert.ToString(e.NewValues["bloqueia_glp"]);
            String participaCT = Convert.ToString(e.NewValues["participacontratotemporario"]);
            String validaAlocacao = Convert.ToString(e.NewValues["validaalocacao"]);

            RetValue retAtualizacao = RN.Licencas.AlterarLicenca(oldMotivo, motivo, descricao, possui_dtfim, periodo_limite, bloqueia_glp, participaCT,validaAlocacao);
            if (retAtualizacao != null && !retAtualizacao.Ok)
                throw new ApplicationException(retAtualizacao.Errors.ToString());
        }

        protected void grdLicencas_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            RetValue retRemocao = RN.Licencas.RemoverLicenca(Convert.ToString(e.Keys["motivo"]));
            if (retRemocao != null && !retRemocao.Ok)
                throw new ApplicationException(retRemocao.Errors.ToString());
        }

        protected void grdLicencas_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdLicencas.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "possui_dtfim")
                    e.Editor.Value = "N";

                if ((e.Column.FieldName) == "validaalocacao")
                    e.Editor.Value = "S";
            }
        }

        protected void grdLicencas_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["motivo"] == null)
            {
                e.RowError = "Licença: campo obrigatório.";
            }
            if (e.NewValues["descricao"] == null)
            {
                e.RowError = "Descrição: campo obrigatório.";
            }

            if (e.NewValues["possui_dtfim"].ToString() == "S")
            {
                if (e.NewValues["periodo_limite"] == null)
                {
                    e.RowError = "Período Limite (em dias): campo obrigatório quando a Licença possui data fim.";
                }
            }

            if (e.NewValues["periodo_limite"] != null)
            {
                e.NewValues["periodo_limite"] = e.NewValues["periodo_limite"].ToString().Trim();
            }

        }

        protected void grdLicencas_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdLicencas.Settings.ShowFilterRow = false;
        }

        protected void grdLicencas_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdLicencas.Settings.ShowFilterRow = false;
        }

       
        
            protected void Button1_Click_ExportarButton1_Click(object sender, EventArgs e)
            {
            
            RN.Licencas rnLicencasColuna = new Techne.Lyceum.RN.Licencas();
            RN.Util.ExportaExcel rnExportaExcel = new Techne.Lyceum.RN.Util.ExportaExcel();
            System.Data.DataTable consulta = new System.Data.DataTable();

            try
            {
                // Valida campos obrigatórios para listagem
                if (true) // (Validar()) 
                {
                    // Busca lista para exportar
                    //consulta = rnTipoProtocolo.ListaTipoProtocolo();
                    consulta = rnLicencasColuna.ListaExcel();
   

                    // Verifica se existem itens para exportar
                    if (consulta.Rows.Count > 0)
                    {
                        // Chama a função para exportar para Excel
                       // ExportaExcelParaNavegador(consulta);
                        rnExportaExcel.ExportaDataTablePor(consulta, "LICENÇAS", "Licencas");

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
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Licencas.csv");
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
        