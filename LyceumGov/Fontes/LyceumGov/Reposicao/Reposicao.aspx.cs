using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.Net.Basico;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using Techne.Controls;
using Techne.Lyceum.RN.DTOs;
using System.Data;
using System.Globalization;
using DevExpress.Web.ASPxEditors;
using Seeduc.Infra.Helpers;
using System.Web.UI.HtmlControls;

namespace Techne.Lyceum.Net.Reposicao
{
    [
     NavUrl("~/Reposicao/Reposicao.aspx"),
     ControlText("Reposicao"),
     Title("Reposição de CH - Greve"),
 ]
    public partial class Reposicao : TPage
    {
        protected readonly string UpdateError = string.Empty;

        public static string GetUrl()
        {
            return Navigation.GetNavigation(System.Reflection.MethodBase.GetCurrentMethod()).GetUrl(new object[] { });
        }


        public object Lista(object ano, object periodo, object periodoReferencia, object turma, object dataGreve)
        {
            RN.Reposicao.Reposicao rnReposicao = new Techne.Lyceum.RN.Reposicao.Reposicao();

            if (!Convert.ToString(periodoReferencia).IsNullOrEmptyOrWhiteSpace() && !Convert.ToString(ano).IsNullOrEmptyOrWhiteSpace() && !Convert.ToString(periodo).IsNullOrEmptyOrWhiteSpace() && !Convert.ToString(turma).IsNullOrEmptyOrWhiteSpace() && Convert.ToDateTime(dataGreve).Year > 0)
            {
                var NomeTurma = turma.ToString().Split(';');
                return rnReposicao.ListaAlocacaoPor(Convert.ToInt32(periodoReferencia), Convert.ToDateTime(dataGreve), Convert.ToInt32(ano), Convert.ToInt32(periodo), NomeTurma[0]);
            }
            return null;
        }

        public object ListaReposicao(object ano, object periodo, object periodoReferencia, object turma, object dataGreve)
        {
            RN.Reposicao.Reposicao rnReposicao = new Techne.Lyceum.RN.Reposicao.Reposicao();


            if (!Convert.ToString(periodoReferencia).IsNullOrEmptyOrWhiteSpace() && !Convert.ToString(ano).IsNullOrEmptyOrWhiteSpace() && !Convert.ToString(periodo).IsNullOrEmptyOrWhiteSpace() && !Convert.ToString(turma).IsNullOrEmptyOrWhiteSpace() && Convert.ToDateTime(dataGreve).Year > 0)
            {
                var NomeTurma = turma.ToString().Split(';');

                return rnReposicao.ListaPor(Convert.ToInt32(periodoReferencia), Convert.ToDateTime(dataGreve), Convert.ToInt32(ano), Convert.ToInt32(periodo), NomeTurma[0]);
            }

            return null;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                ASPxWebControl.RegisterBaseScript(this.Page);
                if (!IsPostBack)
                {
                    DataTable dtPeriodo = new DataTable();
                    LimparDadosBusca();
                    CarregaAno();

                    RN.Reposicao.PeriodoLancamento rnPeriodoLancamento = new Techne.Lyceum.RN.Reposicao.PeriodoLancamento();

                    dtPeriodo = rnPeriodoLancamento.ListaPeriodoAberto();

                    ddlPeriodoReferencia.Items.Clear();
                    ListItem item = new ListItem("Selecione", string.Empty);
                    ddlPeriodoReferencia.DataSource = dtPeriodo;
                    ddlPeriodoReferencia.DataBind();
                    ddlPeriodoReferencia.Items.Insert(0, item);
                }
                else
                {
                    this.grdProfessor.FocusedRowIndex = this.grdProfessor.FocusedRowIndex;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ConfiguraPeriodoData()
        {
            RN.Reposicao.PeriodoLancamento rnPeriodoLancamento = new Techne.Lyceum.RN.Reposicao.PeriodoLancamento();
            RN.Reposicao.Entidades.PeriodoLancamento periodoLancamento = new Techne.Lyceum.RN.Reposicao.Entidades.PeriodoLancamento();
            dtDataGreve.Text = string.Empty;
            
            if (!ddlPeriodoReferencia.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                //Busca dados do periodo
                periodoLancamento = rnPeriodoLancamento.ObtemPor(Convert.ToInt32(ddlPeriodoReferencia.SelectedValue));

                DateTime dtInicioPeriodo = periodoLancamento.DataInicioGreve;
                DateTime? dtFimPeriodo = periodoLancamento.DataFimGreve;

                dtDataGreve.MinDate = new DateTime(dtInicioPeriodo.Year, dtInicioPeriodo.Month, dtInicioPeriodo.Day);
                if (dtFimPeriodo != null && dtFimPeriodo != DateTime.MinValue)
                {
                    DateTime dtLimite = Convert.ToDateTime(dtFimPeriodo);
                    dtDataGreve.MaxDate = new DateTime(dtLimite.Year, dtLimite.Month, dtLimite.Day);
                }
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                ControlaAcesso(grdProfessor);

                ControlaAcesso(grdReposicao, AcaoControle.excluir, "btnExcluirReposicao");
                ControlaAcesso(grdReposicao, AcaoControle.editar, "btnEditarReposicao");

                if (Permission.AllowInsert)
                {
                    ControlaAcesso(btnSalvar, AcaoControle.novo);
                }

                if (Permission.AllowUpdate)
                {
                    ControlaAcesso(btnSalvar, AcaoControle.editar);
                }

                AcessoGrid();

                if (!ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (grdProfessor.VisibleRowCount == 0)
                    {
                        grdReposicao.Columns[0].Visible = false;
                        lblMensagem.Text = "Não existem professores licenciados nesta data de greve para esta unidade escolar/turma.";
                    }
                    else
                    {
                        grdReposicao.Columns[0].Visible = true;

                        if (Permission.AllowInsert)
                        {
                            HtmlInputImage img = (HtmlInputImage)grdReposicao.FindHeaderTemplateControl(grdReposicao.Columns[""], "btnNovoGrid");

                            if (img != null)
                            {

                                if (!hdnSituacao.Value.IsNullOrEmptyOrWhiteSpace())
                                {
                                    if (hdnSituacao.Value == "Recusado")
                                    {
                                        img.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void AcessoGrid()
        {
            if (grdReposicao != null)
            {
                HtmlInputImage img = (HtmlInputImage)grdReposicao.FindHeaderTemplateControl(grdReposicao.Columns[""], "btnNovoGrid");

                if (img != null)
                {
                    img.Visible = Permission.AllowInsert;

                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                TituloGrid(grdProfessor, string.Empty);
                TituloGrid(grdReposicao, string.Empty);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparDadosReposicao()
        {
            lblCHPendente.Text = string.Empty;
            chkRecusado.Checked = false;
            dtDataReposicao.Enabled = true;
            txtCHReposicao.Enabled = true;
            txtCHReposicao.Text = string.Empty;
            dtDataReposicao.Text = string.Empty;
            lblDiaSemanaReposicao.Text = string.Empty;
            hdnIdReposicao.Value = string.Empty;

            if (grdProfessor.FocusedRowIndex >= 0)
            {
                hdnIdReposicao.Value = string.Empty;
            }
            else
            {
                hdnNumFunc.Value = string.Empty;
                hdnDisciplina.Value = string.Empty;
                hdnTipoAula.Value = string.Empty;
            }
        }

        private void LimparDadosBusca()
        {
            lblDiaSemana.Text = string.Empty;
            lblCurso.Text = string.Empty;
            lblTurno.Text = string.Empty;
            ddlPeriodoReferencia.Items.Clear();
            ddlAno.Items.Clear();
            ddlPeriodo.ClearSelection();
            dtDataGreve.Text = string.Empty;
            tseRegional.ResetValue();
            tseMunicipio.ResetValue();
            tseUnidadeResponsavel.ResetValue();
            ddlTurma.Items.Clear();
            lblProfessor.Text = string.Empty;
            lblMsgInformativa.Text = string.Empty;
            hdnTempoPendente.Value = string.Empty;
            hdnSituacao.Value = string.Empty;
            hdnTipoAula.Value = string.Empty;
            hdnNumFunc.Value = string.Empty;
            hdnDisciplina.Value = string.Empty;
            pnlNovaReposicao.Visible = false;
            grdProfessor.FocusedRowIndex = -1;
            grdProfessor.DataBind();
        }

        private void CarregaAno()
        {
            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        private void CarregaTurma()
        {
            RN.Turma rnTurma = new Techne.Lyceum.RN.Turma();
            DataTable dtTurma = new DataTable();
            ddlTurma.Items.Clear();
            lblMsgInformativa.Text = string.Empty;

            if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) && !dtDataGreve.Text.IsNullOrEmptyOrWhiteSpace())
            {
                dtTurma = rnTurma.ListaTurmaReposiçãoPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), tseUnidadeResponsavel.DBValue.ToString(), dtDataGreve.Date);

                if (dtTurma.Rows.Count > 0)
                {
                    ddlTurma.DataSource = dtTurma;
                    ListItem item = new ListItem("Selecione", string.Empty);
                    ddlTurma.DataBind();
                    ddlTurma.Items.Insert(0, item);
                }
                else
                {
                    lblMensagem.Text = "Não há turmas com professores licenciados nesta data de greve para esta unidade escolar/turma.";
                }
            }
            else
            {
                lblMensagem.Text = "Para listar as turmas é necessário preencher a Data da Greve, o Ano, Período e a Unidade Escolar.";
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                ddlPeriodo.ClearSelection();
                lblProfessor.Text = string.Empty;
                lblMsgInformativa.Text = string.Empty;
                ddlTurma.Items.Clear();
                pnlNovaReposicao.Visible = false;
                grdProfessor.FocusedRowIndex = -1;
                grdProfessor.DataBind();


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodoReferencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblProfessor.Text = string.Empty;
                lblMsgInformativa.Text = string.Empty;
                grdReposicao.CancelEdit();
                grdProfessor.CancelEdit();
                pnlNovaReposicao.Visible = false;
                grdProfessor.FocusedRowIndex = -1;
                grdProfessor.DataBind();
                dtDataGreve.Text = string.Empty;

                if (!ddlPeriodoReferencia.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    this.ConfiguraPeriodoData();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblProfessor.Text = string.Empty;
                lblMsgInformativa.Text = string.Empty;
                grdReposicao.CancelEdit();
                grdProfessor.CancelEdit();
                pnlNovaReposicao.Visible = false;
                grdProfessor.FocusedRowIndex = -1;
                grdProfessor.DataBind();

                if (!ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    var turma = ddlTurma.SelectedValue.Split(';');
                    lblTurno.Text = turma[2];
                    lblCurso.Text = turma[3];
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                lblMensagem.Text = string.Empty;
                ddlTurma.Items.Clear();
                ddlTurma.Enabled = false;
                lblProfessor.Text = string.Empty;
                lblMsgInformativa.Text = string.Empty;
                pnlNovaReposicao.Visible = false;
                grdProfessor.FocusedRowIndex = -1;
                grdProfessor.DataBind();

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
                    {
                        sessao.Municipio = tseMunicipio.DBValue.ToString();
                        tseUnidadeResponsavel.ResetValue();
                    }

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                lblMensagem.Text = string.Empty;
                ddlTurma.Items.Clear();
                ddlTurma.Enabled = false;
                lblProfessor.Text = string.Empty;
                lblMsgInformativa.Text = string.Empty;
                pnlNovaReposicao.Visible = false;
                grdProfessor.FocusedRowIndex = -1;
                grdProfessor.DataBind();

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue)
                    {
                        sessao.Regional = Convert.ToString(tseRegional.DBValue);
                        tseUnidadeResponsavel.ResetValue();
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                lblMensagem.Text = string.Empty;
                ddlTurma.Items.Clear();
                ddlTurma.Enabled = false;
                lblCurso.Text = string.Empty;
                lblTurno.Text = string.Empty;
                lblProfessor.Text = string.Empty;
                lblMsgInformativa.Text = string.Empty;
                pnlNovaReposicao.Visible = false;
                grdProfessor.FocusedRowIndex = -1;
                grdProfessor.DataBind();

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel["unidade_ens"].IsNull)
                {

                    tseRegional.Value = tseUnidadeResponsavel["id_regional"];
                    tseMunicipio.Value = tseUnidadeResponsavel["municipio"];

                    if (sessao != null)
                    {
                        sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);
                        sessao.Municipio = tseMunicipio.DBValue.ToString();
                        sessao.Regional = tseUnidadeResponsavel["id_regional"].ToString();
                    }

                    if (dtDataGreve.Date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        if (!ddlPeriodoReferencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) && !dtDataGreve.Text.IsNullOrEmptyOrWhiteSpace())
                        {
                            CarregaTurma();
                            ddlTurma.Enabled = true;
                        }
                        else
                        {
                            lblMensagem.Text = "Para carregar as turmas todos os campos são obrigatórios";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void dtDataGreve_DateChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblProfessor.Text = string.Empty;
                lblMsgInformativa.Text = string.Empty;
                pnlNovaReposicao.Visible = false;
                grdProfessor.FocusedRowIndex = -1;
                grdProfessor.DataBind();

                if (!dtDataGreve.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    if (dtDataGreve.Date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        //Dia da semana (int) por extenso em português (segunda-feira)
                        string diaExtenso = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dtDataGreve.Date.DayOfWeek);

                        lblDiaSemana.Text = diaExtenso;

                        if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) && !dtDataGreve.Text.IsNullOrEmptyOrWhiteSpace())
                        {
                            CarregaTurma();
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Sábado e Domingo não são considerados como dia de greve";
                        ddlTurma.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void HabilitaPnlNovo(object sender, EventArgs e)
        {
            try
            {
                RN.Reposicao.Reposicao rnReposicao = new Techne.Lyceum.RN.Reposicao.Reposicao();
                int ch = 0;
                pnlNovaReposicao.Visible = false;

                if (!ddlPeriodoReferencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !hdnTipoAula.Value.IsNullOrEmptyOrWhiteSpace() && !hdnNumFunc.Value.IsNullOrEmptyOrWhiteSpace() && !hdnDisciplina.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    pnlNovaReposicao.Visible = true;
                    txtCHReposicao.Text = string.Empty;
                    chkRecusado.Checked = false;
                    dtDataReposicao.Enabled = true;
                    txtCHReposicao.Enabled = true;
                    dtDataReposicao.Text = string.Empty;
                    lblCHPendente.Text = string.Empty;
                    hdnIdReposicao.Value = string.Empty;
                    lblDiaSemanaReposicao.Text = string.Empty;
                    var turma = ddlTurma.SelectedValue.Split(';');

                    ch = rnReposicao.ObtemQuantidadeTemposPendentesPor(Convert.ToInt32(ddlPeriodoReferencia.SelectedValue), dtDataGreve.Date, Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), turma[0], hdnDisciplina.Value, Convert.ToInt32(hdnNumFunc.Value), hdnTipoAula.Value);
                    lblCHPendente.Text = ch.ToString();
                }
                else
                {
                    lblMensagem.Text = "Para criar uma nova reposição é necessário escolher um professor no grid Professores licenciados - Greve.";
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
                RN.Reposicao.Reposicao rnReposicao = new Techne.Lyceum.RN.Reposicao.Reposicao();

                int Id = 0;

                Id = Convert.ToInt32(hdnIdReposicao.Value);

                rnReposicao.Remove(Id);
                grdReposicao.DataBind();
                grdProfessor.DataBind();

                this.pucConfirmarReposicao.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmarReposicao.ShowOnPageLoad = false;
            grdReposicao.CancelEdit();
        }

        protected void grdReposicao_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.Reposicao.Reposicao rnReposicao = new Techne.Lyceum.RN.Reposicao.Reposicao();
            RN.Reposicao.Entidades.Reposicao reposicao = new Techne.Lyceum.RN.Reposicao.Entidades.Reposicao();
            int ch = 0;

            try
            {

                if (e.ButtonID == "btnExcluirReposicao")
                {
                    hdnIdReposicao.Value = Convert.ToString(grdReposicao.GetRowValues(e.VisibleIndex, "REPOSICAOID"));

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                }

                if (e.ButtonID == "btnEditarReposicao")
                {
                    pnlNovaReposicao.Visible = true;

                    lblProfessor.Text = string.Empty;
                    lblMsgInformativa.Text = string.Empty;
                    grdProfessor.FocusedRowIndex = -1;
                    //grdProfessor.DataBind();
                    //grdProfessor.Selection.UnselectAll();         
                    lblCHPendente.Text = string.Empty;
                    chkRecusado.Checked = false;
                    dtDataReposicao.Enabled = true;
                    txtCHReposicao.Enabled = true;
                    txtCHReposicao.Text = string.Empty;
                    dtDataReposicao.Text = string.Empty;
                    lblDiaSemanaReposicao.Text = string.Empty;
                    hdnIdReposicao.Value = string.Empty;
                    hdnNumFunc.Value = string.Empty;
                    hdnDisciplina.Value = string.Empty;
                    hdnTipoAula.Value = string.Empty;


                    var turma = ddlTurma.SelectedValue.Split(';');

                    hdnIdReposicao.Value = Convert.ToString(grdReposicao.GetRowValues(e.VisibleIndex, "REPOSICAOID"));
                    chkRecusado.Checked = Convert.ToBoolean(grdReposicao.GetRowValues(e.VisibleIndex, "RECUSADO"));
                    txtCHReposicao.Text = Convert.ToString(grdReposicao.GetRowValues(e.VisibleIndex, "CHREPOSICAO"));
                    dtDataReposicao.Date = Convert.ToDateTime(grdReposicao.GetRowValues(e.VisibleIndex, "DATAREPOSICAO"));

                    hdnTipoAula.Value = Convert.ToString(grdReposicao.GetRowValues(e.VisibleIndex, "TIPO_AULA"));
                    hdnDisciplina.Value = Convert.ToString(grdReposicao.GetRowValues(e.VisibleIndex, "DISCIPLINA"));
                    hdnNumFunc.Value = Convert.ToString(grdReposicao.GetRowValues(e.VisibleIndex, "NUM_FUNC"));



                    if (!ddlPeriodoReferencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !hdnTipoAula.Value.IsNullOrEmptyOrWhiteSpace() && !hdnNumFunc.Value.IsNullOrEmptyOrWhiteSpace() && !hdnDisciplina.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        ch = rnReposicao.ObtemQuantidadeTemposPendentesPor(Convert.ToInt32(ddlPeriodoReferencia.SelectedValue), dtDataGreve.Date, Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), turma[0], hdnDisciplina.Value, Convert.ToInt32(hdnNumFunc.Value), hdnTipoAula.Value);
                        lblCHPendente.Text = ch.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LimparDadosReposicao();
            pnlNovaReposicao.Visible = false;

        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {

                ValidacaoDados validacao = new ValidacaoDados();
                RN.Reposicao.Reposicao rnReposicao = new Techne.Lyceum.RN.Reposicao.Reposicao();
                RN.Reposicao.Entidades.Reposicao reposicao = new Techne.Lyceum.RN.Reposicao.Entidades.Reposicao();

                reposicao.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                reposicao.Censo = !tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue ? tseUnidadeResponsavel.DBValue.ToString() : null;
                reposicao.DataGreve = !dtDataGreve.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataGreve.Date : DateTime.MinValue;

                if (!chkRecusado.Checked)
                {
                    reposicao.CHReposicao = !txtCHReposicao.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtCHReposicao.Text) : -1;
                    reposicao.DataReposicao = !dtDataReposicao.Text.IsNullOrEmptyOrWhiteSpace() ? dtDataReposicao.Date : DateTime.MinValue;

                }
                else
                {
                    reposicao.CHReposicao = 0;
                    reposicao.DataReposicao = DateTime.Now;
                }

                reposicao.Disciplina = !hdnDisciplina.Value.IsNullOrEmptyOrWhiteSpace() ? hdnDisciplina.Value : null;
                reposicao.NumFunc = !hdnNumFunc.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnNumFunc.Value) : -1;
                reposicao.TipoAula = !hdnTipoAula.Value.IsNullOrEmptyOrWhiteSpace() ? hdnTipoAula.Value : null;
                reposicao.Semestre = !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPeriodo.SelectedValue) : -1;
                reposicao.Turma = !ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurma.SelectedItem.Text : null;
                reposicao.ReposicaoId = !hdnIdReposicao.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnIdReposicao.Value) : -1;
                reposicao.UsuarioId = User.Identity.Name;


                validacao = rnReposicao.Valida(reposicao, chkRecusado.Checked, Convert.ToInt32(ddlPeriodoReferencia.SelectedValue), reposicao.ReposicaoId == -1 ? true : false);

                if (validacao.Valido)
                {

                    if (reposicao.ReposicaoId == -1)
                    {
                        rnReposicao.Insere(reposicao);
                    }
                    else
                    {
                        rnReposicao.Atualiza(reposicao);
                    }

                    grdReposicao.DataBind();
                    if (grdProfessor.FocusedRowIndex >= 0)
                    {
                        hdnIdReposicao.Value = string.Empty;
                    }
                    else
                    {
                        LimparDadosReposicao();
                    }
                    pnlNovaReposicao.Visible = false;

                    lblMensagem.Text = "Reposição " + (reposicao.ReposicaoId == -1 ? "cadastrada" : "atualizada") + " com sucesso.";

                }

                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

                grdReposicao.DataBind();
                grdProfessor.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdProfessor.PageIndex * grdProfessor.SettingsPager.PageSize;
            for (int i = 0; i < grdProfessor.VisibleRowCount; i++)
            {
                if (grdProfessor.FocusedRowIndex == startIndexOnPage + i)
                    return startIndexOnPage + 1;
            }
            return -1;
        }

        private string ObtemProfessorSelecionado()
        {
            try
            {
                //obtém o indice atual da seleção
                int curPageSelection = GetSelectedRowOnTheCurrentPage();

                decimal numFunc = Convert.ToDecimal(grdProfessor.GetRowValues(grdProfessor.FocusedRowIndex, "NUM_FUNC"));
                string disciplina = (string)grdProfessor.GetRowValues(grdProfessor.FocusedRowIndex, "DISCIPLINA");
                string IDVINCULO = (string)grdProfessor.GetRowValues(grdProfessor.FocusedRowIndex, "IDVINCULO");
                string NOME_COMPL = (string)grdProfessor.GetRowValues(grdProfessor.FocusedRowIndex, "NOME_COMPL");
                string TIPO_AULA = (string)grdProfessor.GetRowValues(grdProfessor.FocusedRowIndex, "TIPO_AULA");
                string TEMPOS = Convert.ToString(grdProfessor.GetRowValues(grdProfessor.FocusedRowIndex, "TEMPOS"));
                string PENDENTES = Convert.ToString(grdProfessor.GetRowValues(grdProfessor.FocusedRowIndex, "PENDENTES"));
                string SITUACAO = Convert.ToString(grdProfessor.GetRowValues(grdProfessor.FocusedRowIndex, "SITUACAO"));

                if (numFunc > 0)
                {
                    return (numFunc + ";" + disciplina + ";" + IDVINCULO + ";" + NOME_COMPL + ";" + TIPO_AULA + ";" + TEMPOS + ";" + PENDENTES + ";" + SITUACAO);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return string.Empty;
            }
        }



        protected void grdProfessor_FocusedRowChanged(object sender, EventArgs e)
        {
            try
            {
                hdnTipoAula.Value = string.Empty;
                hdnNumFunc.Value = string.Empty;
                hdnDisciplina.Value = string.Empty;
                hdnSituacao.Value = string.Empty;
                hdnIdReposicao.Value = string.Empty;

                if (!IsPostBack && !IsCallback)
                {
                    lblProfessor.Text = string.Empty;
                    lblMsgInformativa.Text = string.Empty;

                    var dados = ObtemProfessorSelecionado().Split(';');


                    if (!dados[0].IsNullOrEmptyOrWhiteSpace())
                    {
                        lblProfessor.Text = "Professor selecionado: " + dados[2] + " - " + dados[3];
                        hdnTempoPendente.Value = dados[6];
                        hdnNumFunc.Value = dados[0];
                        hdnDisciplina.Value = dados[1];
                        hdnTipoAula.Value = dados[4];
                        hdnSituacao.Value = dados[7];

                        lblMsgInformativa.Text = "Por favor, cadastre a CH e Data de Reposição do professor selecionado no quadro abaixo.";
                        lblProfessor.Font.Bold = true;
                        lblProfessor.ForeColor = System.Drawing.Color.Red;
                        lblProfessor.Font.Size = 13;
                        lblMsgInformativa.Font.Bold = true;
                        lblMsgInformativa.ForeColor = System.Drawing.Color.Red;
                        lblMsgInformativa.Font.Size = 11;
                    }
                    else
                    {

                        lblProfessor.Text = !ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !pnlNovaReposicao.Visible ? "Professor selecionado: Nenhum" : string.Empty;
                        lblMsgInformativa.Text = string.Empty;
                        lblProfessor.Font.Bold = true;
                        lblProfessor.ForeColor = System.Drawing.Color.Red;
                        lblProfessor.Font.Size = 13;
                    }


                    grdProfessor.DataBind();
                    grdReposicao.DataBind();

                }
                else
                {
                    lblProfessor.Text = string.Empty;

                    grdProfessor.FocusedRowIndex = ((DevExpress.Web.ASPxGridView.ASPxGridView)sender).FocusedRowIndex;

                    var dados = ObtemProfessorSelecionado().Split(';');

                    if (!dados[0].IsNullOrEmptyOrWhiteSpace())
                    {

                        lblProfessor.Text = "Professor selecionado: " + dados[2] + " - " + dados[3];
                        hdnTempoPendente.Value = dados[6];
                        hdnNumFunc.Value = dados[0];
                        hdnDisciplina.Value = dados[1];
                        hdnTipoAula.Value = dados[4];
                        hdnSituacao.Value = dados[7];

                        lblMsgInformativa.Text = "Por favor, cadastre a CH e Data de Reposição do professor selecionado no quadro abaixo.";
                        lblProfessor.Font.Bold = true;
                        lblProfessor.ForeColor = System.Drawing.Color.Red;
                        lblProfessor.Font.Size = 13;
                        lblMsgInformativa.Font.Bold = true;
                        lblMsgInformativa.ForeColor = System.Drawing.Color.Red;
                        lblMsgInformativa.Font.Size = 11;
                    }
                    else
                    {

                        lblProfessor.Text = !ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !pnlNovaReposicao.Visible ? "Professor selecionado: Nenhum" : string.Empty;
                        lblMsgInformativa.Text = string.Empty;
                        lblProfessor.Font.Bold = true;
                        lblProfessor.ForeColor = System.Drawing.Color.Red;
                        lblProfessor.Font.Size = 13;
                    }

                    grdReposicao.DataBind();
                    grdReposicao.ExpandAll();
                }

                grdReposicao.CancelEdit();
                grdProfessor.CancelEdit();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        protected void grdProfessor_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "SITUACAO")
            {
                String situacao = Convert.ToString(e.CellValue).Trim().ToUpper();
                if (situacao == "PENDENTE")
                    e.Cell.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected void grdProfessor_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpUpdateError"] = this.UpdateError;
        }

        protected void grdProfessor_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }

        protected void grdReposicao_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            var recusado = Convert.ToBoolean(grid.GetRowValues(grid.EditingRowVisibleIndex, new string[] { "RECUSADO" }));
            var numFunc = Convert.ToInt32(grid.GetRowValues(grid.EditingRowVisibleIndex, new string[] { "NUM_FUNC" }));
            var disciplina = Convert.ToString(grid.GetRowValues(grid.EditingRowVisibleIndex, new string[] { "DISCIPLINA" }));
            var tipoAula = Convert.ToString(grid.GetRowValues(grid.EditingRowVisibleIndex, new string[] { "TIPO_AULA" }));
        }

        protected void DATAREPOSICAO_DateChanged(object sender, EventArgs e)
        {
            try
            {

                if (!dtDataReposicao.Text.IsNullOrEmptyOrWhiteSpace())
                {

                    //Dia da semana (int) por extenso em português (segunda-feira)
                    string diaExtenso = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dtDataReposicao.Date.DayOfWeek);

                    lblDiaSemanaReposicao.Text = diaExtenso;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkRecusado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                if (chkRecusado.Checked)
                {
                    dtDataReposicao.Enabled = false;
                    txtCHReposicao.Enabled = false;
                    dtDataReposicao.Text = string.Empty;
                    txtCHReposicao.Text = string.Empty;
                }
                else
                {
                    dtDataReposicao.Enabled = true;
                    txtCHReposicao.Enabled = true;

                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    }
}
