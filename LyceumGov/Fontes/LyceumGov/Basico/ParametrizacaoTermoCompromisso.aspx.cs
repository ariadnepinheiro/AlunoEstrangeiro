using System;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxTabControl;
using Techne.Lyceum.RN;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    using Techne.Lyceum.RN.Entidades;
    [
        NavUrl("~/Basico/ParametrizacaoTermoCompromisso.aspx"),
         ControlText("ParametrizacaoTermoCompromisso"),
         Title("Parametrização - Termo de Compromisso"),
    ]
    public partial class ParametrizacaoTermoCompromisso : TPage
    {
        public object Listar()
        {
            return RN.TermoCompromissoGestao.Listar();
        }

        public object ListarDOL()
        {
            return RN.TermoCompromissoDocente.Listar();
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            TituloGrid(grdCompromissoGestao, "Termo de Compromisso - Gestão");
            TituloGrid(grdCompromissoDOL, "Termo de Compromisso - Docente");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {
                ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
                ddlAno.Items.Insert(0, "Selecione");
                ddlAno.DataBind();

                ddlAnoDOL.DataSource = RN.PeriodoLetivo.ListarAnos();
                ddlAnoDOL.Items.Insert(0, "Selecione");
                ddlAnoDOL.DataBind();
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsePadrao.ResetValue();
            txtDataInicio.Text = string.Empty;
            txtDataFim.Text = string.Empty;
            txtArquivo.Text = string.Empty;

            if (ddlAno.SelectedValue != "Selecione")
            {
                var dt = RN.PeriodoLetivo.ConsultarDatas(ddlAno.SelectedValue, "0");

                if (dt.Rows.Count > 0)
                {
                    txtDataInicio.Text = dt.Rows[0]["dt_inicio"].ToString("dd/MM/yyyy");
                    txtDataFim.Text = dt.Rows[0]["dt_fim"].ToString("dd/MM/yyyy");
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var TTCG = new TceTermoCompromissoGestao
                {
                    Ano = string.IsNullOrEmpty(this.ddlAno.SelectedValue) || this.ddlAno.SelectedValue == "Selecione" ? -1 : Convert.ToInt32(this.ddlAno.SelectedValue),
                    PadraoAcesso = !this.tsePadrao.IsValidDBValue || this.tsePadrao.DBValue.IsNull ? string.Empty : this.tsePadrao.DBValue.ToString(),
                    DtInicio = string.IsNullOrEmpty(txtDataInicio.Text) ? DateTime.MinValue : Convert.ToDateTime(txtDataInicio.Text),
                    DtFim = string.IsNullOrEmpty(txtDataFim.Text) ? DateTime.MinValue : Convert.ToDateTime(txtDataFim.Text),
                    Arquivo = txtArquivo.Text.Trim(),
                    Matricula = this.User.Identity.Name
                };

                var validacao = RN.TermoCompromissoGestao.Validar(TTCG);

                if (validacao.Valido)
                {
                    TermoCompromissoGestao.Inserir(TTCG);

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Termo de Compromisso do Gestão incluído com sucesso.');", true);

                    this.LimparCampos();

                    this.odsCompromissoGestao.Select();
                    this.odsCompromissoGestao.DataBind();
                    this.grdCompromissoGestao.DataBind();
                }
                else
                {
                    if (!string.IsNullOrEmpty(validacao.Mensagem))
                    {
                        this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparCampos()
        {
            this.lblMensagem.Text = string.Empty;

            this.tsePadrao.ResetValue();
            this.ddlAno.SelectedIndex = 0;
            txtDataInicio.Text = string.Empty;
            txtDataFim.Text = string.Empty;
            this.txtArquivo.Text = string.Empty;

        }

        protected void grdCompromissoGestao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCompromissoGestao);
        }

        protected void grdCompromissoGestao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdCompromissoGestao.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ID_TERMO_GESTAO")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "ANO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "PADRAO_ACESSO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DATA_INICIO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DATA_FIM")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "ARQUIVO")
                    e.Editor.ReadOnly = true;

            }
            else if (grdCompromissoGestao.IsEditing)
            {
                if ((e.Column.FieldName) == "ID_TERMO_GESTAO")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "ANO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "PADRAO_ACESSO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DATA_INICIO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DATA_FIM")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "ARQUIVO")
                    e.Editor.ReadOnly = true;
            }
           

        }

        protected void grdCompromissoGestao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCompromissoGestao.Settings.ShowFilterRow = false;

        }

        protected void grdCompromissoGestao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCompromissoGestao.Settings.ShowFilterRow = false;
        }

        protected void grdCompromissoGestao_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["DT_INICIO"])))
            {
                e.RowError = "Favor informar a Data Início.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["DT_FIM"])))
            {
                e.RowError = "Favor informar a Data Fim.";
            }


        }

        public void Delete(object ID_TERMO_GESTAO)
        {
        }

        public void Update(object ANO, object PADRAO_ACESSO, object DT_INICIO, object DT_FIM, object ARQUIVO, object ID_TERMO_GESTAO)
        {
        }

        protected void odsCompromissoGestao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["ID_TERMO_GESTAO"].ToString();

            var validacao = RN.TermoCompromissoGestao.ValidarRemover(int.Parse(id));

            if (validacao.Valido)
            {
                RN.TermoCompromissoGestao.Remover(int.Parse(id));
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsCompromissoGestao_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var TTCG = new TceTermoCompromissoGestao
            {
                IdTermoGestao = Convert.ToInt32(e.InputParameters["ID_TERMO_GESTAO"]),
                DtInicio = e.InputParameters["DT_INICIO"] == null ? DateTime.MinValue : Convert.ToDateTime(e.InputParameters["DT_INICIO"]),
                DtFim = e.InputParameters["DT_FIM"] == null ? DateTime.MinValue : Convert.ToDateTime(e.InputParameters["DT_FIM"]),
                Ano = Convert.ToInt32(e.InputParameters["ANO"]),
                Arquivo = e.InputParameters["ARQUIVO"].ToString(),
                PadraoAcesso = e.InputParameters["PADRAO_ACESSO"].ToString(),
                Matricula = User.Identity.Name
            };

            var validacao = RN.TermoCompromissoGestao.Validar(TTCG);

            if (validacao.Valido)
            {
                RN.TermoCompromissoGestao.Alterar(TTCG);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void pcTermo_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;
        }

        #region "DOL"

        protected void ddlAnoDOL_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtDataInicioDOL.Text = string.Empty;
            txtDataFimDOL.Text = string.Empty;
            txtArquivoDOL.Text = string.Empty;

            if (ddlAnoDOL.SelectedValue != "Selecione")
            {
                var dt = RN.PeriodoLetivo.ConsultarDatas(ddlAnoDOL.SelectedValue, "0");

                if (dt.Rows.Count > 0)
                {
                    txtDataInicioDOL.Text = dt.Rows[0]["dt_inicio"].ToString("dd/MM/yyyy");
                    txtDataFimDOL.Text = dt.Rows[0]["dt_fim"].ToString("dd/MM/yyyy");
                }
            }
        }

        protected void btnSalvarDOL_Click(object sender, EventArgs e)
        {
            try
            {
                var TTCD = new TceTermoCompromissoDocente
                {
                    Ano = string.IsNullOrEmpty(this.ddlAnoDOL.SelectedValue) || this.ddlAnoDOL.SelectedValue == "Selecione" ? -1 : Convert.ToInt32(this.ddlAnoDOL.SelectedValue),
                    DtInicio = string.IsNullOrEmpty(txtDataInicioDOL.Text) ? DateTime.MinValue : Convert.ToDateTime(txtDataInicioDOL.Text),
                    DtFim = string.IsNullOrEmpty(txtDataFimDOL.Text) ? DateTime.MinValue : Convert.ToDateTime(txtDataFimDOL.Text),
                    Arquivo = txtArquivoDOL.Text.Trim(),
                    Matricula = this.User.Identity.Name
                };

                var validacao = RN.TermoCompromissoDocente.Validar(TTCD);

                if (validacao.Valido)
                {
                    TermoCompromissoDocente.Inserir(TTCD);

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Termo de Compromisso do Docente Online incluído com sucesso.');", true);

                    this.LimparCamposDOL();

                    this.odsCompromissoDOL.Select();
                    this.odsCompromissoDOL.DataBind();
                    this.grdCompromissoDOL.DataBind();
                }
                else
                {
                    if (!string.IsNullOrEmpty(validacao.Mensagem))
                    {
                        this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparCamposDOL()
        {
            this.lblMensagem.Text = string.Empty;

            this.ddlAnoDOL.SelectedIndex = 0;
            txtDataInicioDOL.Text = string.Empty;
            txtDataFimDOL.Text = string.Empty;
            this.txtArquivoDOL.Text = string.Empty;

        }

        protected void grdCompromissoDOL_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCompromissoDOL);
        }

        protected void grdCompromissoDOL_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdCompromissoGestao.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ID_TERMO_DOCENTE")
                    e.Editor.Enabled = true;
                if ((e.Column.FieldName) == "ANO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DATA_INICIO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DATA_FIM")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "ARQUIVO")
                    e.Editor.ReadOnly = true;

            }
            else if (grdCompromissoGestao.IsEditing)
            {
                if ((e.Column.FieldName) == "ID_TERMO_DOCENTE")
                    e.Editor.Enabled = false;
                if ((e.Column.FieldName) == "ANO")
                    e.Editor.ReadOnly = true;

                if ((e.Column.FieldName) == "DATA_INICIO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DATA_FIM")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "ARQUIVO")
                    e.Editor.ReadOnly = true;
            }

        }

        protected void grdCompromissoDOL_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCompromissoDOL.Settings.ShowFilterRow = false;
        }

        protected void grdCompromissoDOL_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCompromissoDOL.Settings.ShowFilterRow = false;
        }

        protected void grdCompromissoDOL_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["DT_INICIO"])))
            {
                e.RowError = "Favor informar a Data Início.";
            }
            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["DT_FIM"])))
            {
                e.RowError = "Favor informar a Data Fim.";
            }


        }

        public void DeleteDOL(object ID_TERMO_DOCENTE)
        {
        }

        public void UpdateDOL(object ANO, object DT_INICIO, object DT_FIM, object ARQUIVO, object ID_TERMO_DOCENTE)
        {
        }

        protected void odsCompromissoDOL_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            string id = e.InputParameters["ID_TERMO_DOCENTE"].ToString();

            var validacao = RN.TermoCompromissoDocente.ValidarRemover(int.Parse(id));

            if (validacao.Valido)
            {
                RN.TermoCompromissoDocente.Remover(int.Parse(id));
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsCompromissoDOL_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var TTCD = new TceTermoCompromissoDocente
            {
                IdTermoDocente = Convert.ToInt32(e.InputParameters["ID_TERMO_DOCENTE"]),
                DtInicio = e.InputParameters["DT_INICIO"] == null ? DateTime.MinValue : Convert.ToDateTime(e.InputParameters["DT_INICIO"]),
                DtFim = e.InputParameters["DT_FIM"] == null ? DateTime.MinValue : Convert.ToDateTime(e.InputParameters["DT_FIM"]),
                Ano = Convert.ToInt32(e.InputParameters["ANO"]),
                Arquivo = e.InputParameters["ARQUIVO"].ToString(),
                Matricula = User.Identity.Name
            };

            var validacao = RN.TermoCompromissoDocente.Validar(TTCD);

            if (validacao.Valido)
            {
                RN.TermoCompromissoDocente.Alterar(TTCD);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        #endregion

    }
}
