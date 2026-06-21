using System;
using System.Data;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using System.Web.UI;
using Techne.Lyceum.RN.Util;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/CategoriaDocente.aspx"),
      ControlText("CategoriaDocente"),
      Title("Cargos do Docente/Funcionário"),]
    public partial class CategoriaDocente : TPage
    {
        public object Lista()
        {
            RN.CategoriaDocente rnCategoriaDocente = new Techne.Lyceum.RN.CategoriaDocente();

            return rnCategoriaDocente.Lista();

        }
        public object ListaGrupo()
        {
            RN.RecursosHumanos.AgrupamentoCargos rnAgrupamentoCargos = new Techne.Lyceum.RN.RecursosHumanos.AgrupamentoCargos();

            return rnAgrupamentoCargos.ListaAtivo();

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdCategoria, "Cargos do Docente/Funcionário");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdCategoria);
        }

        protected void grdCategoria_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdCategoria.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "CATEGORIA")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdCategoria.IsEditing)
            {
                if ((e.Column.FieldName) == "CATEGORIA")
                {
                    e.Editor.Enabled = false;
                }

                if ((e.Column.FieldName) == "CARGAHORARIAGRUPO")
                {
                    e.Editor.Enabled = false;
                }

                if ((e.Column.FieldName) == "CHAVE")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdCategoria_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCategoria);
        }

        protected void grdCategoria_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCategoria.Settings.ShowFilterRow = false;
        }

        protected void grdCategoria_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCategoria.Settings.ShowFilterRow = false;
        }

        protected void grdCategoria_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Entidades.LyCategoriaDocente categoria = new Techne.Lyceum.RN.Entidades.LyCategoriaDocente();
            RN.CategoriaDocente rnCategoriaDocente = new Techne.Lyceum.RN.CategoriaDocente();
            int chGrupo = 0;

            categoria.Categoria = e.NewValues["CATEGORIA"] != null ? e.NewValues["CATEGORIA"].ToString().Trim().ToUpper() : null;
            categoria.AgrupamentoCargosId = e.NewValues["CHAVE"] != null ? Convert.ToInt32(e.NewValues["CHAVE"].ToString().Split('_')[0]) : -1;
            categoria.CargaHorariaPlanejamento = e.NewValues["CARGAHORARIAPLANEJAMENTO"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIAPLANEJAMENTO"]) : -1;
            categoria.CargaHorariaRegencia = e.NewValues["CARGAHORARIAREGENCIA"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIAREGENCIA"]) : -1;
            categoria.Nome = e.NewValues["NOME"] != null ? e.NewValues["NOME"].ToString().Trim().ToUpper() : null;
            categoria.Funcao = e.NewValues["FUNCAO"] != null ? e.NewValues["FUNCAO"].ToString().Trim() : null;
            categoria.Ingresso = e.NewValues["INGRESSO"] != null ? e.NewValues["INGRESSO"].ToString().Trim() : "N";
            categoria.NecessitaSuperior = e.NewValues["NECESSITA_SUPERIOR"] != null ? e.NewValues["NECESSITA_SUPERIOR"].ToString().Trim() : "N";
            categoria.Funcionario = e.NewValues["FUNCIONARIO"] != null ? e.NewValues["FUNCIONARIO"].ToString().Trim() : "N";
            categoria.Tipo = e.NewValues["TIPO"] != null ? e.NewValues["TIPO"].ToString().Trim() : null;
            categoria.UsuarioId = User.Identity.Name;
            chGrupo =e.NewValues["CHAVE"] != null ? Convert.ToInt32(e.NewValues["CHAVE"].ToString().Split('_')[1]) :-1;


            validacao = rnCategoriaDocente.Valida(categoria, chGrupo, true);

            if (validacao.Valido)
            {
                rnCategoriaDocente.Insere(categoria);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdCategoria.DataBind();

        }

        protected void grdCategoria_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Entidades.LyCategoriaDocente categoria = new Techne.Lyceum.RN.Entidades.LyCategoriaDocente();
            RN.CategoriaDocente rnCategoriaDocente = new Techne.Lyceum.RN.CategoriaDocente();
            int chGrupo = 0;

            categoria.AgrupamentoCargosId = e.NewValues["CHAVE"] != null ? Convert.ToInt32(e.NewValues["CHAVE"].ToString().Split('_')[0]) : -1;
            categoria.CargaHorariaPlanejamento = e.NewValues["CARGAHORARIAPLANEJAMENTO"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIAPLANEJAMENTO"]) : -1;
            categoria.CargaHorariaRegencia = e.NewValues["CARGAHORARIAREGENCIA"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIAREGENCIA"]) : -1;
            categoria.Nome = e.NewValues["NOME"] != null ? e.NewValues["NOME"].ToString().Trim().ToUpper() : null;
            categoria.Funcao = e.NewValues["FUNCAO"] != null ? e.NewValues["FUNCAO"].ToString().Trim() : null;
            categoria.Ingresso = e.NewValues["INGRESSO"] != null ? e.NewValues["INGRESSO"].ToString().Trim() : "N";
            categoria.NecessitaSuperior = e.NewValues["NECESSITA_SUPERIOR"] != null ? e.NewValues["NECESSITA_SUPERIOR"].ToString().Trim() : "N";
            categoria.Tipo = e.NewValues["TIPO"] != null ? e.NewValues["TIPO"].ToString().Trim() : null;
            categoria.Funcionario = e.NewValues["FUNCIONARIO"] != null ? e.NewValues["FUNCIONARIO"].ToString().Trim() : "N";
            categoria.UsuarioId = User.Identity.Name;
            categoria.Categoria = Convert.ToString(e.Keys["CATEGORIA"]);
            chGrupo = Convert.ToInt32(e.NewValues["CHAVE"].ToString().Split('_')[1]);

            validacao = rnCategoriaDocente.Valida(categoria, chGrupo, false);

            if (validacao.Valido)
            {
                rnCategoriaDocente.Atualiza(categoria);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdCategoria.DataBind();
        }

        protected void grdCategoria_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.CategoriaDocente rnCategoriaDocente = new Techne.Lyceum.RN.CategoriaDocente();
            string categoria = string.Empty;

            categoria = Convert.ToString(e.Keys["CATEGORIA"]);

            validacao = rnCategoriaDocente.ValidaRemocao(categoria);

            if (validacao.Valido)
            {
                rnCategoriaDocente.Remove(categoria);
                grdCategoria.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        public void Insert(object CATEGORIA, object NOME, object INGRESSO, object NECESSITA_SUPERIOR, object FUNCIONARIO, object CHAVE, object CARGAHORARIAREGENCIA, object CARGAHORARIAPLANEJAMENTO, object CARGAHORARIAGRUPO, object FUNCAO, object TIPO)
        { }

        public void Update(object NOME, object INGRESSO, object NECESSITA_SUPERIOR, object FUNCIONARIO, object CHAVE, object CARGAHORARIAREGENCIA, object CARGAHORARIAPLANEJAMENTO, object CARGAHORARIAGRUPO, object FUNCAO, object TIPO, object CATEGORIA)
        { }

        public void Update(object NOME, object INGRESSO, object NECESSITA_SUPERIOR, object FUNCIONARIO, object CHAVE, object CARGAHORARIAREGENCIA, object CARGAHORARIAPLANEJAMENTO, object FUNCAO, object TIPO, object CATEGORIA)
        { }

        public void Delete(object CATEGORIA)
        { }

        protected void odsGrupo_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
        { }
        //// Função para exportar DataTable para Excel

        protected void Button1_Click_ExportarButton1_Click(object sender, EventArgs e)
        {

            // RN.Protocolo.TipoProtocolo rnTipoProtocolo = new Techne.Lyceum.RN.Protocolo.TipoProtocolo();
            RN.CategoriaDocente rnCategoriaDocenteColuna = new Techne.Lyceum.RN.CategoriaDocente();
            RN.Util.ExportaExcel rnExportaExcel = new Techne.Lyceum.RN.Util.ExportaExcel();
            System.Data.DataTable consulta = new System.Data.DataTable();

            try
            {
                // Valida campos obrigatórios para listagem
                if (true) // (Validar()) 
                {
                    // Busca lista para exportar
                    //consulta = rnTipoProtocolo.ListaTipoProtocolo();
                    consulta = rnCategoriaDocenteColuna.ListaExcel();


                    // Verifica se existem itens para exportar
                    if (consulta.Rows.Count > 0)
                    {
                        // Chama a função para exportar para Excel
                        // ExportaExcelParaNavegador(consulta);
                        rnExportaExcel.ExportaDataTablePor(consulta, "CATEGORIA DOCENTE", "CategoriaDocente");

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