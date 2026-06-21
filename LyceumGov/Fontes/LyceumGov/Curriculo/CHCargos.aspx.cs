using System;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.RN.RecursosHumanos;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace Techne.Lyceum.Net.Curriculo
{
    [
     NavUrl("~/Curriculo/CHCargos.aspx"),
      ControlText("CHCargos"),
      Title("Carga Horária da GLP"),
    ]

    public partial class CHCargos : TPage
    {
        private readonly ChGlp rnChGlp;
        private readonly AgrupamentoCargos rnAgrupamentoCargos;
        private readonly Funcao rnFuncao;
        private readonly RN.RecursosHumanos.ChAgrupamentoCargo rnChAgrupamentoCargo; 

        public CHCargos()
        {
            rnChGlp = new ChGlp();
            rnAgrupamentoCargos = new AgrupamentoCargos();
            rnFuncao = new Funcao();
            rnChAgrupamentoCargo = new ChAgrupamentoCargo();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdChGlp, "Cargas Horárias");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //TESTE DA CLASSE no LyceumRN

            //ChGlp ch = new ChGlp();

            //var obj = new Techne.Lyceum.RN.RecursosHumanos.Entidades.CH_GLP
            //{
            //    NR_MATRICULAS = 1,
            //    AGRUPAMENTOCARGOSID = 1,
            //    CH_GRUPO = 1,
            //    FUNCAO = "1",
            //    AGRUPAMENTOCARGOSID_2 = 1,
            //    CH_GRUPO_2 = 1,
            //    FUNCAO_2 = "1",
            //    CH_SEMANAL_TOTAL = 1,
            //    CH_GLP = 1,
            //    USUARIOID = User.Identity.Name,
            //};

            //ch.Insere(obj);

            //obj.NR_MATRICULAS = 2;

            //ch.Atualiza(obj);

            //ch.Remove(obj.CH_GLPID);

            //AgrupamentoCargos ac = new AgrupamentoCargos();

            //var lista = ac.ListaAtivo();


        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdChGlp);

            if (!IsPostBack)
                txtChSemanaTotal.Text = ConfigurationManager.AppSettings["CH_SEMANAL_TOTAL"];

            if (EmEdicao)
                AtualizaCamposCalculados();
        }

        public object ListaGrupo()
        {
            return rnAgrupamentoCargos.Lista();
        }

        public object ListaGrupoAtivo()
        {
            return rnAgrupamentoCargos.ListaAtivo();
        }

        public object ListaFuncao()
        {
            return rnFuncao.Lista();
        }

        public object Lista(int? AGRUPAMENTOCARGOSID)
        {
            return rnChGlp.Lista(AGRUPAMENTOCARGOSID);
        }
        public void Insere(object NR_MATRICULAS, object AGRUPAMENTOCARGOSID, object CH_GRUPO, object FUNCAO, object AGRUPAMENTOCARGOSID_2, object CH_GRUPO_2, object FUNCAO_2, object CH_SEMANAL_TOTAL, object CH_GLP) { }
        public void Remove(object CH_GLPID) { }

        public bool EmEdicao
        {
            get
            {
                return !plaConsulta.Visible && plaEdicao.Visible;
            }
            set
            {
                lblMensagem.Text = "";

                ddlGrupo.SelectedIndex = -1;
                grdChGlp.DataBind();

                rblNrMatriculas.SelectedValue = "1";
                rblNrMatriculas_SelectedIndexChanged(null, null);

                ddlGrupo1.SelectedIndex = -1;
                txtChGrupo1.Text = "";
                tseFuncao1.Value = null;

                ddlGrupo2.SelectedIndex = -1;
                txtChGrupo2.Text = "";
                tseFuncao2.Value = null;

                plaConsulta.Visible = !value;
                plaEdicao.Visible = value;
            }
        }

        private void AtualizaCamposCalculados()
        {            
            bool camposPreenchidos = true;
            int chGrupo1 = 0;
            int chGrupo2 = 0;
            int chTotalGrupo = 0;
            int chGlp = 0;
            int chSemanalTotal = 0;

            if (string.IsNullOrEmpty((tseFuncao1.Value ?? "").ToString()) || string.IsNullOrEmpty(ddlGrupo1.SelectedValue))
            {
                camposPreenchidos = false;
            }
            else if (rblNrMatriculas.SelectedValue == "2")
            {
                if (string.IsNullOrEmpty((tseFuncao2.Value ?? "").ToString()) || string.IsNullOrEmpty(ddlGrupo2.SelectedValue))
                {
                    camposPreenchidos = false;
                }
            }
            if (camposPreenchidos)
            {
                int.TryParse(txtChGrupo1.Text, out chGrupo1);
                int.TryParse(txtChSemanaTotal.Text, out chSemanalTotal);
                int.TryParse(rblNrMatriculas.SelectedValue == "2" ? txtChGrupo2.Text : "0", out chGrupo2);

                //Busca CH do conjunto grupo + Funcao
                chGrupo1 = rnChAgrupamentoCargo.ObtemCargaTotalPor(Convert.ToInt32(ddlGrupo1.SelectedValue), tseFuncao1.Value.ToString());
                
                if (rblNrMatriculas.SelectedValue == "2")
                {
                    chGrupo2 = rnChAgrupamentoCargo.ObtemCargaTotalPor(Convert.ToInt32(ddlGrupo2.SelectedValue), tseFuncao2.Value.ToString());
                }

                chTotalGrupo = chGrupo1 + chGrupo2;
                chGlp = chSemanalTotal - chTotalGrupo;
                chGlp = chGlp <= 0 ? 0 : chGlp;
            }
            txtChGrupo1.Text = chGrupo1.ToString();
            txtChGrupo2.Text = chGrupo2.ToString();
            txtTotalGrupo.Text = chTotalGrupo.ToString();
            txtChGlp.Text = chGlp.ToString();
        }

        private bool SegundaMatriculaEstaIgualAPrimeira
        {
            get
            {
                bool Tem2Matriculas = rblNrMatriculas.SelectedValue == "2";
                bool grupo1Preenchido = !string.IsNullOrEmpty(ddlGrupo1.SelectedValue);
                bool funcao1Preenchida = !string.IsNullOrEmpty((tseFuncao1.Value ?? "").ToString());
                bool grupo2IgualGrupo1 = ddlGrupo1.SelectedValue == ddlGrupo2.SelectedValue;
                bool funcao2IgualFuncao1 = (tseFuncao1.Value ?? "").ToString() == (tseFuncao2.Value ?? "").ToString();

                return (Tem2Matriculas && grupo1Preenchido && funcao1Preenchida && grupo2IgualGrupo1 && funcao2IgualFuncao1);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            EmEdicao = true;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            EmEdicao = false;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> mensagens = new List<string>();

                int nr_matriculas;
                int.TryParse(rblNrMatriculas.SelectedValue ?? "", out nr_matriculas);
                if (!new int[] { 1, 2 }.Contains(nr_matriculas))
                    mensagens.Add("É preciso selecionar a qtd. de matrículas (1 ou 2).");

                if (ddlGrupo1.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("É preciso selecionar o grupo da 1º matrícula.");

                if (txtChGrupo1.Text.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("É preciso informar a carga horária da 1º matrícula.");

                int chGrupo1;
                if (!int.TryParse(txtChGrupo1.Text, out chGrupo1))
                    mensagens.Add("A carga horária da 1º matrícula precisa ser numérica.");

                if ((tseFuncao1.Value ?? "").ToString().IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("É preciso selecionar a função da 1º matrícula.");

                if (nr_matriculas == 2)
                {
                    if (ddlGrupo2.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        mensagens.Add("É preciso selecionar o grupo da 2º matrícula.");

                    if (txtChGrupo2.Text.IsNullOrEmptyOrWhiteSpace())
                        mensagens.Add("É preciso informar a carga horária da 2º matrícula.");

                    int chGrupo2;
                    if (!int.TryParse(txtChGrupo2.Text, out chGrupo2))
                        mensagens.Add("A carga horária da 2º matrícula precisa ser numérica.");

                    if ((tseFuncao2.Value ?? "").ToString().IsNullOrEmptyOrWhiteSpace())
                        mensagens.Add("É preciso selecionar a função da 2º matrícula.");

                }

                RN.RecursosHumanos.Entidades.ChGlp obj = new RN.RecursosHumanos.Entidades.ChGlp
                {
                    NrMatriculas = nr_matriculas,
                    AgrupamentoCargosId = int.Parse(ddlGrupo1.SelectedValue),
                    ChGrupo = int.Parse(txtChGrupo1.Text),
                    Funcao = tseFuncao1.Value.ToString(),
                    ChSemanalTotal = int.Parse(txtChSemanaTotal.Text),
                    Ch_Glp = int.Parse(txtChGlp.Text),
                    UsuarioId = User.Identity.Name,
                };
                if (obj.NrMatriculas == 2)
                {
                    obj.AgrupamentoCargosId2 = int.Parse(ddlGrupo2.SelectedValue);
                    obj.ChGrupo2 = int.Parse(txtChGrupo2.Text);
                    obj.Funcao2 = tseFuncao2.Value.ToString();
                }

                if (rnChGlp.ExistePor(obj))
                    mensagens.Add("Já existe uma carga horária cadastrada com esta combinação de GRUPO e FUNÇÃO em matrícula 1 e 2.");

                if (mensagens.Any())
                {
                    lblMensagem.Text = string.Join("<br />", mensagens.ToArray());
                    return;
                }

                rnChGlp.Insere(obj);

                btnCancelar_Click(sender, e);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdChGlp_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            //e.NewValues["categoria_docente"] = tseCategoria.DBValue.ToString();
        }

        protected void grdChGlp_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            //e.NewValues["categoria_docente"] = tseCategoria.DBValue.ToString();
        }

        protected void grdChGlp_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.ChGlp rnChGlp = new ChGlp();

            int chGlpId = Convert.ToInt32(e.Keys["CH_GLPID"]);

            validacao = rnChGlp.ValidaRemocao(chGlpId);

            if (validacao.Valido)
            {
                rnChGlp.Remove(chGlpId);
                grdChGlp.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void grdChGlp_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            //    string funcao = Convert.ToString(e.NewValues["funcao"]);
            //    string num_matr = Convert.ToString(e.NewValues["nr_matriculas"]);
            //    string readaptado = Convert.ToString(e.NewValues["readaptado"]);
            //    string glp = Convert.ToString(e.NewValues["glp"]);
            //    string categoria = string.Empty;

            //    if (!tseCategoria.DBValue.IsNull)
            //    {
            //        if (tseCategoria.IsValidDBValue)
            //            categoria = Convert.ToString(tseCategoria.DBValue);
            //    }

            //    QueryTable qt = RN.CHCategorias.ConsultarInclusao(categoria, funcao, num_matr, readaptado, glp);

            //    if (grdCategoria.IsNewRowEditing)
            //    {
            //        if (qt.Rows.Count >= 1)
            //        {
            //            e.RowError = "Os campos 'Função', 'Nr. Matrículas', 'Readaptado' e 'GLP' não podem ser duplicados para uma mesma categoria.";
            //            return;
            //        }
            //    }
            //    else if (grdCategoria.IsEditing)
            //    {
            //        if (qt.Rows.Count > 1)
            //        {
            //            e.RowError = "Os campos 'Função', 'Nr. Matrículas', 'Readaptado' e 'GLP' não podem ser duplicados para uma mesma categoria.";
            //            return;
            //        }
            //    }

            //    decimal total = Convert.ToDecimal(e.NewValues["ch_semanal_total"]);
            //    decimal efetiva = Convert.ToDecimal(e.NewValues["ch_semanal_efetiva"]);
            //    if (total <= 0)
            //        e.RowError = "Carga horária total não pode menor ou igual a zero.";
            //    if (efetiva <= 0)
            //        e.RowError = "Carga horária efetiva não pode ser menor ou igual a zero.";

            //    int total_int = Convert.ToInt32(total);
            //    if (total_int != 0)
            //    {
            //        if (total % total_int != 0)
            //            e.RowError = "Carga horária total deve ser um número inteiro.";


            //        int total_str = Convert.ToInt32(total);
            //        if (total % total_int != 0)
            //            e.RowError = "Carga horária total deve ser um número inteiro.";
            //    }

            //    int efetiva_int = Convert.ToInt32(efetiva);
            //    if (efetiva_int != 0)
            //    {
            //        if (efetiva % efetiva_int != 0)
            //            e.RowError = "Carga horária efetiva deve ser um número inteiro.";
            //    }


            //    if (efetiva > total)
            //        e.RowError = "Carga horária efetiva não pode ser maior que a total.";
        }

        protected void grdChGlp_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdChGlp.Settings.ShowFilterRow = false;
        }

        protected void grdChGlp_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdChGlp.Settings.ShowFilterRow = false;
        }

        protected void grdChGlp_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdChGlp.IsNewRowEditing)
            {
                //if ((e.Column.FieldName) == "nr_matriculas")
                //    e.Editor.ReadOnly = false;

                //if ((e.Column.FieldName) == "readaptado")
                //    e.Editor.ReadOnly = false;

                //if ((e.Column.FieldName) == "glp")
                //    e.Editor.ReadOnly = false;

            }
            else if (grdChGlp.IsEditing)
            {
                //if ((e.Column.FieldName) == "nr_matriculas")
                //    e.Editor.ReadOnly = true;

                //if ((e.Column.FieldName) == "readaptado")
                //    e.Editor.ReadOnly = true;

                //if ((e.Column.FieldName) == "glp")
                //    e.Editor.ReadOnly = true;
            }

        }

        protected void grdChGlp_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (grdChGlp.IsNewRowEditing)
            {
                //TSearchBox tseFuncao = (TSearchBox)grdChGlp.FindEditFormTemplateControl("tseFuncao");
                //if (tseFuncao != null)
                //    tseFuncao.ReadOnly = false;
            }
            else if (grdChGlp.IsEditing)
            {
                //TSearchBox tseFuncao = (TSearchBox)grdChGlp.FindEditFormTemplateControl("tseFuncao");
                //if (tseFuncao != null)
                //    tseFuncao.ReadOnly = true;
            }

            ControlaAcesso(grdChGlp);
        }

        protected void rblNrMatriculas_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlGrupo1.ClearSelection();
            ddlGrupo2.ClearSelection();
            txtChGrupo1.Text = string.Empty;
            txtChGrupo2.Text = string.Empty;
            tseFuncao1.ResetValue();
            tseFuncao2.ResetValue();

            plaMatricula2.Visible = (rblNrMatriculas.SelectedValue == "2");
        }

        protected void ddlGrupo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int agrupamentoGrupoId = 0;

                if (!int.TryParse(ddlGrupo1.SelectedValue ?? "0", out agrupamentoGrupoId))
                {
                    ddlGrupo1.SelectedValue = "";
                    txtChGrupo1.Text = "";
                    return;
                }               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlGrupo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int agrupamentoGrupoId = 0;

                if (!int.TryParse(ddlGrupo2.SelectedValue ?? "0", out agrupamentoGrupoId))
                {
                    ddlGrupo2.SelectedValue = "";
                    txtChGrupo2.Text = "";
                    return;
                }               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        // Função para exportar DataTable para Excel
        protected void Button1_Click_ExportarButton1_Click(object sender, EventArgs e)
        {

            RN.RecursosHumanos.ChGlp rnChGLPColuna = new Techne.Lyceum.RN.RecursosHumanos.ChGlp();
            RN.Util.ExportaExcel rnExportaExcel = new Techne.Lyceum.RN.Util.ExportaExcel();
            System.Data.DataTable consulta = new System.Data.DataTable();

            try
            {
                // Valida campos obrigatórios para listagem
                if (true) // (Validar()) 
                {
                    // Busca lista para exportar
                    consulta = rnChGLPColuna.ListaExcel();

                    // Verifica se existem itens para exportar
                    if (consulta.Rows.Count > 0)
                    {
                        // Chama a função para exportar para Excel
                       // ExportaExcelParaNavegador(consulta);
                        rnExportaExcel.ExportaDataTablePor(consulta, "CH Cargo", "ChAgrupamentoCargo");

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