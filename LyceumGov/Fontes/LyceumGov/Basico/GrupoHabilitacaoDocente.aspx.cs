using System;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Helpers;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.HtmlControls;


namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/GrupoHabilitacaoDocente.aspx"),
      ControlText("GrupoHabilitaçãoDocente"),
      Title("Grupos de Habilitações por Docente"),
    ]

    public partial class GrupoHabilitacaoDocente : TPage
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
            TituloGrid(grdDiscHabilit, "Grupos de Disciplinas Habilitadas");
            TituloGrid(grdDiscHabilitProv, "Grupos de Disciplinas Habilitadas Provisoriamente");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pcDicsHabilitacaoDoc.ActiveTabIndex = 0; //abrir sempre na primeira aba
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            //ControlaAcesso(grdDiscHabilit);
            //ControlaAcesso(grdDiscHabilitProv);
        }

        protected void grdDiscHabilit_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            //ControlaAcesso(grdDiscHabilit);
            ControlaAcesso(grdDiscHabilit, AcaoControle.editar, "btnEditar");
            ControlaAcesso(grdDiscHabilit, AcaoControle.excluir, "btnExcluir");
            AcessoGrid();
        }

        protected void grdDiscHabilit_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdDiscHabilit.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "agrupamento")
                {
                    e.Editor.ClientEnabled = true;
                    e.Editor.ReadOnly = false;
                }
            }
            else if (grdDiscHabilit.IsEditing)
            {
                if ((e.Column.FieldName) == "agrupamento")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "agrupamento_ingresso")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "provisorio")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "num_func")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "campo_01")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "campo_02")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
            }
        }


        protected void grdDiscHabilit_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string num_func = Convert.ToString(e.GetListSourceFieldValue("num_func"));
                string agrupamento = Convert.ToString(e.GetListSourceFieldValue("agrupamento"));
                string provisorio = Convert.ToString(e.GetListSourceFieldValue("provisorio"));
                e.Value = num_func + "-" + agrupamento + "-" + provisorio;
            }
        }

        protected void grdDiscHabilit_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            RN.GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new Techne.Lyceum.RN.GrupoHabilitacaoDoc();
            RN.Entidades.LyGrupoHabilitacaoDoc grupoHabilitacaoDoc = new RN.Entidades.LyGrupoHabilitacaoDoc();
            ValidacaoDados validacao = new ValidacaoDados();

            string[] chaves = e.Keys["CompositeKey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("num_func", chaves[0]);
            e.Keys.Add("agrupamento", chaves[1]);
            e.Keys.Add("provisorio", chaves[2]);

            decimal pessoa = !this.tseDocentes.DBValue.IsNull && this.tseDocentes.IsValidDBValue ? Convert.ToDecimal(tseDocentes["pessoa"]) : -1;

            grupoHabilitacaoDoc.NumFunc = Convert.ToDecimal(chaves[0]);
            grupoHabilitacaoDoc.Agrupamento = chaves[1];
            grupoHabilitacaoDoc.Provisorio = chaves[2];
            grupoHabilitacaoDoc.Campo01 = e.NewValues["campo_01"] != null ? Convert.ToString(e.NewValues["campo_01"]) : "N";
            grupoHabilitacaoDoc.Campo02 = e.NewValues["campo_02"] != null ? Convert.ToString(e.NewValues["campo_02"]) : "N";
            grupoHabilitacaoDoc.Documentacao = e.NewValues["documentacao"] != null ? Convert.ToString(e.NewValues["documentacao"]).ToUpper() : null;
            grupoHabilitacaoDoc.UsuarioId = User.Identity.Name;
            grupoHabilitacaoDoc.AgrupamentoIngresso = e.NewValues["agrupamento_ingresso"] != null ? Convert.ToString(e.NewValues["agrupamento_ingresso"]) : "N";


            validacao = rnGrupoHabilitacaoDoc.Valida(grupoHabilitacaoDoc, pessoa, false);

            if (validacao.Valido)
            {
                rnGrupoHabilitacaoDoc.Atualiza(grupoHabilitacaoDoc);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdDiscHabilit.DataBind();
        }

        protected void grdDiscHabilit_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            RN.GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new Techne.Lyceum.RN.GrupoHabilitacaoDoc();
            e.Keys.Clear();
            e.Keys.Add("num_func", e.Values["num_func"]);
            e.Keys.Add("agrupamento", e.Values["agrupamento"]);
            e.Keys.Add("provisorio", e.Values["provisorio"]);

            rnGrupoHabilitacaoDoc.Remove(Convert.ToDecimal(e.Values["num_func"]), Convert.ToString(e.Values["agrupamento"]), Convert.ToString(e.Values["provisorio"]));
        }

        protected void grdDiscHabilit_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            RN.GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new Techne.Lyceum.RN.GrupoHabilitacaoDoc();
            RN.Entidades.LyGrupoHabilitacaoDoc grupoHabilitacaoDoc = new RN.Entidades.LyGrupoHabilitacaoDoc();
            ValidacaoDados validacao = new ValidacaoDados();

            //Monta Entidades
            grupoHabilitacaoDoc.NumFunc = !this.tseDocentes.DBValue.IsNull && this.tseDocentes.IsValidDBValue ? Convert.ToDecimal(tseDocentes["num_func"].ToString()) : -1;
            grupoHabilitacaoDoc.Agrupamento = e.NewValues["agrupamento"] != null ? Convert.ToString(e.NewValues["agrupamento"]) : null;
            grupoHabilitacaoDoc.AgrupamentoIngresso = e.NewValues["agrupamento_ingresso"] != null ? Convert.ToString(e.NewValues["agrupamento_ingresso"]) : "N";
            grupoHabilitacaoDoc.Provisorio = "N";
            grupoHabilitacaoDoc.DtLimite = null;
            grupoHabilitacaoDoc.Campo01 = e.NewValues["campo_01"] != null ? Convert.ToString(e.NewValues["campo_01"]) : "N";
            grupoHabilitacaoDoc.Campo02 = e.NewValues["campo_02"] != null ? Convert.ToString(e.NewValues["campo_02"]) : "N";
            grupoHabilitacaoDoc.Documentacao = e.NewValues["documentacao"] != null ? Convert.ToString(e.NewValues["documentacao"]).ToUpper() : null;
            grupoHabilitacaoDoc.UsuarioId = User.Identity.Name;

            decimal pessoa = !this.tseDocentes.DBValue.IsNull && this.tseDocentes.IsValidDBValue ? Convert.ToDecimal(tseDocentes["pessoa"]) : -1;
            validacao = rnGrupoHabilitacaoDoc.Valida(grupoHabilitacaoDoc, pessoa, true);

            if (validacao.Valido)
            {
                rnGrupoHabilitacaoDoc.Insere(grupoHabilitacaoDoc);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdDiscHabilit.DataBind();
        }

        protected void grdDiscHabilitProv_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdDiscHabilitProv.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "agrupamento")
                {
                    e.Editor.ClientEnabled = true;
                    e.Editor.ReadOnly = false;
                }
            }
            else if (grdDiscHabilitProv.IsEditing)
            {
                if ((e.Column.FieldName) == "agrupamento")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "provisorio")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "num_func")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "campo_01")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "campo_02")
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdDiscHabilitProv_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDiscHabilitProv);
        }

        protected void grdDiscHabilitProv_CustomUnboundColumnData1(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKeyProv")
            {
                string num_func = Convert.ToString(e.GetListSourceFieldValue("num_func"));
                string agrupamento = Convert.ToString(e.GetListSourceFieldValue("agrupamento"));
                string provisorio = Convert.ToString(e.GetListSourceFieldValue("provisorio"));
                e.Value = num_func + "-" + agrupamento + "-" + provisorio;
            }
        }
        
        protected void tseDocentes_Changed(object sender, EventArgs args)
        {
            try
            {
                hdnNumFunc.Value = string.Empty;
                lblMensagem.Text = string.Empty;

                if (tseDocentes.IsValidDBValue && !tseDocentes.DBValue.IsNull)
                {
                    hdnNumFunc.Value = tseDocentes["num_func"].ToString();
                    pcDicsHabilitacaoDoc.Visible = true;


                    grdDiscHabilitProv.DataBind();
                    grdDiscHabilit.DataBind();

                }
                else
                {
                    pcDicsHabilitacaoDoc.Visible = false;
                    lblMensagem.Text = "Docente inválido.";
                }
            }
            catch (Exception e)
            {
                if (e.Message.ToUpper().Contains("TEMPO EXCEDIDO"))
                    lblMensagem.Text = "Erro: Tempo excedido. Tente novamente.";
            }
        }

        protected void grdDiscHabilit_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdDiscHabilit.Settings.ShowFilterRow = false;
        }

        protected void grdDiscHabilit_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdDiscHabilit.Settings.ShowFilterRow = false;
        }

        protected void grdDiscHabilitProv_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdDiscHabilitProv.Settings.ShowFilterRow = false;
        }

        protected void grdDiscHabilitProv_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdDiscHabilitProv.Settings.ShowFilterRow = false;
        }

        protected void grdDiscHabilit_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data
                || !this.grdDiscHabilit.Visible || this.grdDiscHabilit.VisibleRowCount == 0)
            {
                return;
            }

            var colDocumentacao = this.grdDiscHabilit.Columns["documentacao"] as GridViewDataColumn;
            var txtJustificativa = (TextBox)this.grdDiscHabilit.FindRowCellTemplateControl(e.VisibleIndex, colDocumentacao, "txtJustificativa");

            var hfObservacao = (HiddenField)this.grdDiscHabilit.FindRowCellTemplateControl(e.VisibleIndex, colDocumentacao, "hfObservacao");

            var cmbObservacao = DevExpressHelper.GetControl<DropDownList>(this.grdDiscHabilit, e.VisibleIndex, "observacao", "cmbObservacao");

            if (hfObservacao.Value != null)
            {
                if (string.IsNullOrEmpty(hfObservacao.Value))
                {
                    cmbObservacao.SelectedValue = string.Empty;
                    txtJustificativa.Enabled = false;
                    txtJustificativa.BackColor = Color.Gainsboro;
                    txtJustificativa.TabIndex = -1;
                }
                else
                {
                    //cmbObservacao.SelectedValue = hfObservacao.Value;

                    if (cmbObservacao.SelectedValue == "TAD SEM PROCESSO" || cmbObservacao.SelectedValue == "DISCIPLINA SEM OBRIGATORIEDADE DE TAD")
                    {
                        txtJustificativa.Enabled = false;
                        txtJustificativa.BackColor = Color.Gainsboro;
                        txtJustificativa.TabIndex = -1;
                        txtJustificativa.Text = string.Empty;
                    }
                }

            //    cmbObservacao.Enabled = false;
          


            cmbObservacao.Attributes.Add("txtJustificativa", txtJustificativa.ClientID);

            //    cmbObservacao.ClearSelection();

            //    var observacao = string.IsNullOrEmpty(hfObservacao.Value) ? "Selecione" : hfObservacao.Value;

            //    cmbObservacao.SelectedValue = observacao;
            }
        }

        protected void HabilitaPnlNovo(object sender, EventArgs e)
        {
            try
            {
                pnlGrupoHab.Visible = true;
                LimpaCampos();
                hdnId.Value = string.Empty;
                CarregaGrupo();
                ddlGrupo.Enabled = true;
               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaGrupo()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            RN.GrupoHabilitacao rnGrupoHabilitacao = new Techne.Lyceum.RN.GrupoHabilitacao();


            ddlGrupo.Items.Clear();
            ddlGrupo.DataSource = rnGrupoHabilitacao.ListaGrupoHabilitacaoDocAtivo();
            ddlGrupo.DataBind();
            ddlGrupo.Items.Insert(0, item);
        }

        protected void LimpaCampos()
        {
            ddlGrupo.ClearSelection();
            chkIngresso.Checked = false;
            chkHabilitacao.Checked = false;
            chkGLP.Checked = false;
            ddlTipoTAD.ClearSelection();
            txtDocumentacao.Text = string.Empty;
        }

        protected void ddlTipoTAD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtDocumentacao.Text = string.Empty;
                txtDocumentacao.Enabled = false;
                
                if (!string.IsNullOrEmpty(ddlTipoTAD.SelectedValue))
                {
                    if (ddlTipoTAD.SelectedValue == "TAD SEM PROCESSO" || ddlTipoTAD.SelectedValue == "DISCIPLINA SEM OBRIGATORIEDADE DE TAD")
                    {
                        
                        txtDocumentacao.Enabled = false;
                    }
                    else if (ddlTipoTAD.SelectedValue == "TAD COM PROCESSO")
                    {
                       
                        txtDocumentacao.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDiscHabilit_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {

                if (e.ButtonID == "btnEditar")
                {

                    LimpaCampos();
                    CarregaGrupo();                    
                    txtDocumentacao.Enabled = false;

                    chkIngresso.Checked = Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "agrupamento_ingresso")) == "S" ? true : false;
                    txtDocumentacao.Text = grdDiscHabilit.GetRowValues(e.VisibleIndex, "documentacao").ToString();
                    hdnId.Value = Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "CompositeKey"));
                    ddlGrupo.SelectedValue = Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "agrupamento") );
                    ddlGrupo.Enabled = false;
                    chkHabilitacao.Checked = Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "campo_01")) == "S" ? true : false;;
                    chkGLP.Checked = Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "campo_02"))  == "S" ? true : false;;
                    pnlGrupoHab.Visible = true;

                    if (string.IsNullOrEmpty(Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "documentacao"))))
                    {
                        ddlTipoTAD.SelectedValue = string.Empty;
                        txtDocumentacao.Text = string.Empty;
                    }
                    else if (Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "documentacao")) == "TAD SEM PROCESSO" || Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "documentacao")) == "DISCIPLINA SEM OBRIGATORIEDADE DE TAD")
                    {
                        ddlTipoTAD.SelectedValue = Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "documentacao"));
                        txtDocumentacao.Text = string.Empty;
                        txtDocumentacao.Enabled = false;
                    }
                    else
                    {
                        ddlTipoTAD.SelectedValue = "TAD COM PROCESSO";
                        txtDocumentacao.Text = Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "documentacao"));
                        txtDocumentacao.Enabled = true;
                    }

                        btnSalvar.Visible = true;

                }

                if (e.ButtonID == "btnExcluir")
                {
                    hdnId.Value = Convert.ToString(grdDiscHabilit.GetRowValues(e.VisibleIndex, "CompositeKey"));

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new Techne.Lyceum.RN.GrupoHabilitacaoDoc();
                RN.Entidades.LyGrupoHabilitacaoDoc grupoHabilitacaoDoc = new RN.Entidades.LyGrupoHabilitacaoDoc();
                ValidacaoDados validacao = new ValidacaoDados();

                if (!hdnId.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    string[] chaves = hdnId.Value.ToString().Split('-');
                    grupoHabilitacaoDoc.Provisorio = chaves[2];
                }
                else
                {                    
                    grupoHabilitacaoDoc.Provisorio = "N";
                    grupoHabilitacaoDoc.DtLimite = null;
                }

                //Monta Entidades
                grupoHabilitacaoDoc.NumFunc = !this.tseDocentes.DBValue.IsNull && this.tseDocentes.IsValidDBValue ? Convert.ToDecimal(tseDocentes["num_func"].ToString()) : -1;
                grupoHabilitacaoDoc.Agrupamento = !ddlGrupo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlGrupo.SelectedValue : null;
                grupoHabilitacaoDoc.AgrupamentoIngresso = chkIngresso.Checked ? "S" : "N";
                grupoHabilitacaoDoc.Campo01 = chkHabilitacao.Checked ? "S" : "N";
                grupoHabilitacaoDoc.Campo02 = chkGLP.Checked ? "S" : "N";
               

                if (ddlTipoTAD.SelectedValue == "TAD SEM PROCESSO" || ddlTipoTAD.SelectedValue == "DISCIPLINA SEM OBRIGATORIEDADE DE TAD")
                {
                    grupoHabilitacaoDoc.Documentacao = ddlTipoTAD.SelectedValue;
                }
                else if (ddlTipoTAD.SelectedValue == "TAD COM PROCESSO")
                {
                    grupoHabilitacaoDoc.Documentacao = txtDocumentacao.Text;
                }
                else
                {
                    grupoHabilitacaoDoc.Documentacao = null;
                }
                
                grupoHabilitacaoDoc.UsuarioId = User.Identity.Name;

                decimal pessoa = !this.tseDocentes.DBValue.IsNull && this.tseDocentes.IsValidDBValue ? Convert.ToDecimal(tseDocentes["pessoa"]) : -1;
                validacao = rnGrupoHabilitacaoDoc.Valida(grupoHabilitacaoDoc, pessoa, (hdnId.Value.IsNullOrEmptyOrWhiteSpace() ? true : false));

                if (validacao.Valido)
                {
                    if (hdnId.Value.IsNullOrEmptyOrWhiteSpace())
                    {

                        rnGrupoHabilitacaoDoc.Insere(grupoHabilitacaoDoc);
                    }
                    else
                    {
                        rnGrupoHabilitacaoDoc.Atualiza(grupoHabilitacaoDoc);
                    }

                    grdDiscHabilit.DataBind();
                    LimpaCampos();
                    pnlGrupoHab.Visible = false;

                    lblMensagem.Text = "Grupo Habilitação " + (hdnId.Value.IsNullOrEmptyOrWhiteSpace() ? "cadastrado" : "atualizado") + " com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

                grdDiscHabilit.DataBind();                

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void AcessoGrid()
        {
            if (grdDiscHabilit != null)
            {
                HtmlInputImage img = (HtmlInputImage)grdDiscHabilit.FindHeaderTemplateControl(grdDiscHabilit.Columns[""], "btnNovoGrid");
               

                if (img != null)
                {
                    img.Visible = Permission.AllowInsert;
                   
                }
            }
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.GrupoHabilitacaoDoc rnGrupoHabilitacaoDoc = new Techne.Lyceum.RN.GrupoHabilitacaoDoc();
                RN.Entidades.LyGrupoHabilitacaoDoc grupoHabilitacaoDoc = new RN.Entidades.LyGrupoHabilitacaoDoc();

                string[] chaves = hdnId.Value.ToString().Split('-');
             
                rnGrupoHabilitacaoDoc.Remove(Convert.ToDecimal(chaves[0]), Convert.ToString(chaves[1]), Convert.ToString(chaves[2]));

                grdDiscHabilit.DataBind();
                
                this.pucConfirmar.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmar.ShowOnPageLoad = false;
            grdDiscHabilit.CancelEdit();
        }

        #region Métodos ODS
        public static void InserirGrupoDeHabilitacaoDocente(string agrupamento, string agrupamento_ingresso, object campo_01, object campo_02, decimal? num_func, string provisorio, object dt_limite, string documentacao) { }
        public static void InserirGrupoDeHabilitacaoDocente(string agrupamento, string agrupamento_ingresso, object campo_01, object campo_02, object datacadastro, object documentacao) { }
        public static void RemoverGrupoDeHabilitacaoDocente(decimal num_func, string agrupamento, string provisorio) { }
        public static void AtualizarGrupoDeHabilitacaoDocente(string agrupamento,object agrupamento_ingresso, object campo_01, object campo_02,object datacadastro,string documentacao, decimal? num_func, string provisorio) { }

        public static QueryTable ObterGruposDeHabilitacaoDocente(string num_func)
        {
            return RN.GrupoHabilitacao.ObterGruposDeHabilitacaoDocente(num_func);
        }

        public static QueryTable ObterGruposDeHabilitacaoDocenteProvisorios(string num_func)
        {
            return RN.GrupoHabilitacao.ObterGruposDeHabilitacaoDocenteProvisorios(num_func);
        }
        #endregion
    }
}
