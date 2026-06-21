using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxTabControl;
using System.Web.UI.HtmlControls;
using System.Data;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/PeriodoConfMatricula.aspx"), ControlText("PeriodoConfMatricula"), Title("Período Confirmação Matrícula")]

    public partial class PeriodoConfMatricula : TPage
    {
        public object Lista()
        {
            RN.Pedagogico.PeriodoConfirmacao rnPeriodoConfirmacao = new Techne.Lyceum.RN.Pedagogico.PeriodoConfirmacao();

            return rnPeriodoConfirmacao.Lista();
        }

        public object ListaCursoSerie(object anoPeriodo)
        {
            RN.Pedagogico.PeriodoConfirmacaoCurso rnPeriodoConfirmacaoCurso = new Techne.Lyceum.RN.Pedagogico.PeriodoConfirmacaoCurso();

            if (!Convert.ToString(anoPeriodo).IsNullOrEmptyOrWhiteSpace())
            {
                return rnPeriodoConfirmacaoCurso.ListaPor(Convert.ToInt32(anoPeriodo));
            }

            return null;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!this.IsPostBack)
                {
                    LimpaCamposInicial();

                    CarregaAno();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdInicial, string.Empty);
            TituloGrid(grdCursoSerie, string.Empty);

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdInicial);
            ControlaAcesso(grdCursoSerie);
            ControlaAcesso(grdInicial, AcaoControle.editar, "btnEditar");
            ControlaAcesso(grdInicial, AcaoControle.excluir, "btnExcluir");
            ControlaAcesso(grdCursoSerie, AcaoControle.editar, "btnEditarCursoSerie");
            ControlaAcesso(grdCursoSerie, AcaoControle.excluir, "btnExcluirCursoSerie");
            if (Permission.AllowInsert)
            {
                ControlaAcesso(btnSalvar, AcaoControle.novo);
                ControlaAcesso(btnSalvarCursoSerie, AcaoControle.novo);
            }

            if (Permission.AllowUpdate)
            {
                ControlaAcesso(btnSalvar, AcaoControle.editar);
                ControlaAcesso(btnSalvarCursoSerie, AcaoControle.editar);
            }

            AcessoGrid();
        }

        private void CarregaAno()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

            ddlAno.Items.Clear();
            ddlAno.DataSource = rnPeriodoLetivo.ListaProximosAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        private void CarregaPeriodo(int ano)
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

            ddlPeriodo.Items.Clear();
            ddlPeriodo.DataSource = rnPeriodoLetivo.ListaPeriodosletivosPor(ano);
            ddlPeriodo.DataBind();
            ddlPeriodo.Items.Insert(0, item);
        }


        protected void HabilitaPnlNovo(object sender, EventArgs e)
        {
            try
            {
                pnAbaInicial.Visible = true;
                LimpaCamposInicial();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaCamposInicial()
        {

            hdnIdLiberacao.Value = string.Empty;
            ddlAno.ClearSelection();
            ddlPeriodo.Items.Clear();
            dtDataInicio.Text = string.Empty;
            dtDataFim.Text = string.Empty;

        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.PeriodoConfirmacao rnPeriodoConfirmacao = new Techne.Lyceum.RN.Pedagogico.PeriodoConfirmacao();
                RN.Pedagogico.Entidades.PeriodoConfirmacao periodoConfirmacao = new Techne.Lyceum.RN.Pedagogico.Entidades.PeriodoConfirmacao();

                periodoConfirmacao.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                periodoConfirmacao.Periodo = !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPeriodo.SelectedValue) : -1;
                periodoConfirmacao.PeriodoConfirmacaoId = !hdnIdLiberacao.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnIdLiberacao.Value) : -1;
                periodoConfirmacao.DataInicio = !dtDataInicio.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataInicio.Date : DateTime.MinValue;
                periodoConfirmacao.DataFim = !dtDataFim.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataFim.Date : DateTime.MinValue;
                periodoConfirmacao.UsuarioId = User.Identity.Name;

                validacao = rnPeriodoConfirmacao.Valida(periodoConfirmacao, periodoConfirmacao.PeriodoConfirmacaoId == -1 ? true : false);

                if (validacao.Valido)
                {
                    if (periodoConfirmacao.PeriodoConfirmacaoId == -1)
                    {
                        rnPeriodoConfirmacao.Insere(periodoConfirmacao);
                    }
                    else
                    {
                        rnPeriodoConfirmacao.Atualiza(periodoConfirmacao);
                    }
                    grdInicial.DataBind();
                    LimpaCamposInicial();
                    pnAbaInicial.Visible = false;

                    lblMensagem.Text = "Período " + (periodoConfirmacao.PeriodoConfirmacaoId == -1 ? "cadastrado" : "atualizado") + " com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }



        protected void pcLiberacao_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;
        }

        protected void grdInicial_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdInicial.Settings.ShowFilterRow = false;
        }

        protected void grdInicial_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdInicial.Settings.ShowFilterRow = false;
        }

        protected void grdInicial_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdInicial);
            ControlaAcesso(grdInicial, AcaoControle.editar, "btnEditar");
            ControlaAcesso(grdInicial, AcaoControle.excluir, "btnExcluir");
            AcessoGrid();
        }

        protected void grdInicial_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {

                if (e.ButtonID == "btnEditar")
                {

                    LimpaCamposInicial();

                    hdnIdLiberacao.Value = Convert.ToString(grdInicial.GetRowValues(e.VisibleIndex, "PERIODOCONFIRMACAOID"));
                    ddlAno.SelectedValue = Convert.ToString(grdInicial.GetRowValues(e.VisibleIndex, "ANO"));
                    ddlAno_SelectedIndexChanged(null, null);
                    ddlPeriodo.SelectedValue = Convert.ToString(grdInicial.GetRowValues(e.VisibleIndex, "PERIODO"));
                    dtDataInicio.Date = Convert.ToDateTime(grdInicial.GetRowValues(e.VisibleIndex, "DATAINICIO"));
                    dtDataFim.Date = Convert.ToDateTime(grdInicial.GetRowValues(e.VisibleIndex, "DATAFIM"));

                    pnAbaInicial.Visible = true;

                }

                if (e.ButtonID == "btnExcluir")
                {
                    hdnIdLiberacao.Value = Convert.ToString(grdInicial.GetRowValues(e.VisibleIndex, "PERIODOCONFIRMACAOID"));

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.PeriodoConfirmacao rnPeriodoConfirmacao = new Techne.Lyceum.RN.Pedagogico.PeriodoConfirmacao();

                int periodoConfirmacaoId = 0;

                periodoConfirmacaoId = Convert.ToInt32(hdnIdLiberacao.Value);

                validacao = rnPeriodoConfirmacao.ValidaRemocao(periodoConfirmacaoId);

                if (validacao.Valido)
                {
                    rnPeriodoConfirmacao.Remove(periodoConfirmacaoId);
                    grdInicial.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    grdInicial.CancelEdit();
                }

                this.pucConfirmarInicial.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmarInicial.ShowOnPageLoad = false;
            grdInicial.CancelEdit();
        }


        public void Delete(object PERIODOCONFIRMACAOID) { }


        protected void HabilitaPnlNovaCursoSerie(object sender, EventArgs e)
        {
            try
            {
                pnlAbaCurso.Visible = true;
                tseCurso.ResetValue();
                ddlSerie.Items.Clear();


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaCamposCursoSerie()
        {

            tseAnoPeriodo.ResetValue();
            tseCurso.ResetValue();
            ddlSerie.Items.Clear();
            grdCursoSerie.DataBind();

        }

        protected void btnSalvarCursoSerie_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.PeriodoConfirmacaoCurso rnPeriodoConfirmacaoCurso = new Techne.Lyceum.RN.Pedagogico.PeriodoConfirmacaoCurso();
                RN.Pedagogico.Entidades.PeriodoConfirmacaoCurso periodo = new Techne.Lyceum.RN.Pedagogico.Entidades.PeriodoConfirmacaoCurso();

                periodo.PeriodoConfirmacaoCursoId = !hdnIdCursoSerie.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnIdCursoSerie.Value) : -1;
                periodo.PeriodoConfirmacaoId = (!this.tseAnoPeriodo.DBValue.IsNull && this.tseAnoPeriodo.IsValidDBValue) ? Convert.ToInt32(tseAnoPeriodo.DBValue) : -1;
                periodo.Curso = (!this.tseCurso.DBValue.IsNull && this.tseCurso.IsValidDBValue) ? tseCurso.DBValue.ToString() : null;
                periodo.Serie = !ddlSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSerie.SelectedValue) : -1;


                periodo.UsuarioId = User.Identity.Name;

                validacao = rnPeriodoConfirmacaoCurso.Valida(periodo, Convert.ToInt32(tseAnoPeriodo["ano"]), Convert.ToInt32(tseAnoPeriodo["periodo"]), periodo.PeriodoConfirmacaoCursoId == -1 ? true : false);

                if (validacao.Valido)
                {
                    if (periodo.PeriodoConfirmacaoCursoId == -1)
                    {
                        rnPeriodoConfirmacaoCurso.Insere(periodo);
                    }

                    grdCursoSerie.DataBind();
                    tseCurso.ResetValue();
                    ddlSerie.Items.Clear();
                    pnlAbaCurso.Visible = false;

                    lblMensagem.Text = "Curso/Série cadastrado com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdCursoSerie_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCursoSerie.Settings.ShowFilterRow = false;
        }

        protected void grdCursoSerie_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdCursoSerie.Settings.ShowFilterRow = false;
        }

        protected void grdCursoSerie_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCursoSerie);
            ControlaAcesso(grdCursoSerie, AcaoControle.editar, "btnEditarCursoSerie");
            ControlaAcesso(grdCursoSerie, AcaoControle.excluir, "btnExcluirCursoSerie");
            AcessoGrid();
        }

        protected void grdCursoSerie_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {           

            if (e.ButtonID == "btnExcluirCursoSerie")
            {
                tseCurso.ResetValue();
                ddlSerie.Items.Clear();
                pnlAbaCurso.Visible = false;
                hdnIdCursoSerie.Value = Convert.ToString(grdCursoSerie.GetRowValues(e.VisibleIndex, "PERIODOCONFIRMACAOCURSOID"));

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopupCursoSerie();", true);
            }
        }

        public void DeleteCursoSerie(object PERIODOCONFIRMACAOCURSOID) { }


        protected void btnSimCursoSerie_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Pedagogico.PeriodoConfirmacaoCurso rnPeriodoConfirmacaoCurso = new Techne.Lyceum.RN.Pedagogico.PeriodoConfirmacaoCurso();
                int IdCursoSerie = 0;

                IdCursoSerie = Convert.ToInt32(hdnIdCursoSerie.Value);

                validacao = rnPeriodoConfirmacaoCurso.ValidaRemocao(IdCursoSerie);

                if (validacao.Valido)
                {
                    rnPeriodoConfirmacaoCurso.Remove(IdCursoSerie);
                    grdCursoSerie.DataBind();
                    pnlAbaCurso.Visible = true;
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    grdCursoSerie.CancelEdit();
                }
                this.pucConfirmarCursoSerie.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNaoCursoSerie_Click(object sender, EventArgs e)
        {
            this.pucConfirmarCursoSerie.ShowOnPageLoad = false;
            grdCursoSerie.CancelEdit();
        }

        protected void AcessoGrid()
        {
            if (grdInicial != null)
            {
                HtmlInputImage img = (HtmlInputImage)grdInicial.FindHeaderTemplateControl(grdInicial.Columns[""], "btnNovoGridInicial");
                HtmlInputImage imgT = (HtmlInputImage)grdCursoSerie.FindHeaderTemplateControl(grdCursoSerie.Columns[""], "btnNovoGridCursoSerie");


                if (img != null)
                {
                    img.Visible = Permission.AllowInsert;
                    imgT.Visible = Permission.AllowInsert;


                }
            }
        }

        protected void tseCurso_Changed(object sender, EventArgs e)
        {
            try
            {
                RN.Serie rnSerie = new Serie();

                this.ddlSerie.Items.Clear();
                this.ddlSerie.Enabled = false;

                if ((this.tseCurso.IsValidDBValue && !this.tseCurso.DBValue.IsNull) && (this.tseAnoPeriodo.IsValidDBValue && !this.tseAnoPeriodo.DBValue.IsNull))
                {
                    this.ddlSerie.Enabled = true;
                    this.ddlSerie.DataSource = rnSerie.ListaSeriePor(Convert.ToInt32(tseAnoPeriodo["ano"]), Convert.ToInt32(tseAnoPeriodo["periodo"]), this.tseCurso.DBValue.ToString());
                    this.ddlSerie.DataBind();
                    this.ddlSerie.Items.Insert(0, new ListItem("Selecione", string.Empty));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPeriodo.Items.Clear();

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    CarregaPeriodo(Convert.ToInt32(ddlAno.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
    }
}
