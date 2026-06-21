using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Controls;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
    [
        NavUrl("~/Academico/ReaberturaAluno.aspx"),
        ControlText("ReaberturaAluno"),
        Title("Reabertura de Aluno"),
    ]

    public partial class ReaberturaAluno : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    LimparTela();
                    pnlGeral.Visible = false;
                    btnReabrir.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            dtDataReabertura.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnReabrir, AcaoControle.novo);
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            RN.DTOs.DadosEncerramentoAluno dadosEncerramentoAluno = new Techne.Lyceum.RN.DTOs.DadosEncerramentoAluno();
            RN.HCursosConcl rnHCursosConcl = new Techne.Lyceum.RN.HCursosConcl();


            if (this.Page.IsCallback)
            {
                return;
            }

            try
            {
                LimparTela();
                pnlGeral.Visible = false;
                btnReabrir.Visible = false;
                if (!this.tseAluno.DBValue.IsNull)
                {
                    if (this.tseAluno.IsValidDBValue)
                    {
                        dadosEncerramentoAluno = rnHCursosConcl.ObtemDadosAlunoEncerramentoPor(tseAluno.DBValue.ToString());

                        if (dadosEncerramentoAluno.Ano > 0)
                        {
                            txtUnidadeEnsino.Text = dadosEncerramentoAluno.NomeUnidadeEnsinoAtual;
                            hdnCenso.Value = dadosEncerramentoAluno.UnidadeEnsinoAtual;
                            txtCurso.Text = dadosEncerramentoAluno.CursoAtual;
                            txtNomeCurso.Text = dadosEncerramentoAluno.NomeCursoAtual;
                            txtTurno.Text = dadosEncerramentoAluno.TurnoAtual;
                            txtNomeTurno.Text = dadosEncerramentoAluno.NomeTurnoAtual;
                            txtSerie.Text = dadosEncerramentoAluno.SerieAtual.ToString();
                            txtNomeSerie.Text = dadosEncerramentoAluno.NomeSerieAtual;
                            txtCurriculoEncerramento.Text = dadosEncerramentoAluno.CurriculoAtual;
                            txtAnoEncerramento.Text = dadosEncerramentoAluno.Ano.ToString();
                            txtPeriodoEncerramento.Text = dadosEncerramentoAluno.Periodo.ToString();
                            txtMotivoEncerramento.Text = dadosEncerramentoAluno.Motivo;
                            txtDescMotivoEncerramento.Text = dadosEncerramentoAluno.MotivoDescricao.ToUpper();
                            txtSituacao.Text = dadosEncerramentoAluno.Situacao;
                            txtDataEncerramento.Text = dadosEncerramentoAluno.DtEncerramento.ToShortDateString();
                            if (txtMotivoEncerramento.Text == "PROV_DES")
                            {
                                lblMotivoReabertura.Visible = true;
                                ddlMotivoReabertura.Visible = true;
                                CarregaMotivoReabertura();
                            }
                            dtDataReabertura.Date = DateTime.Now;
                            CarregaAnoReabertura();
                            pnlGeral.Visible = true;
                            btnReabrir.Visible = true;
                        }
                        else
                        {
                            lblMensagem.Text = "Reabertura não disponível para este aluno.";
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não encontrado.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não cadastrado.";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void LimparTela()
        {
            txtCurriculoEncerramento.Text = string.Empty;
            txtCurso.Text = string.Empty;
            txtNomeCurso.Text = string.Empty;
            txtNomeTurno.Text = string.Empty;
            txtSerie.Text = string.Empty;
            txtNomeSerie.Text = string.Empty;
            txtSituacao.Text = string.Empty;
            txtTurno.Text = string.Empty;
            txtUnidadeEnsino.Text = string.Empty;
            txtAnoEncerramento.Text = string.Empty;
            txtPeriodoEncerramento.Text = string.Empty;
            txtMotivoEncerramento.Text = string.Empty;
            txtDescMotivoEncerramento.Text = string.Empty;
            hdnCenso.Value = string.Empty;
            hdnCurriculoReabertura.Value = string.Empty;

            dtDataReabertura.Text = string.Empty;
            ddlAnoReabertura.ClearSelection();
            ddlPeriodoReabertura.Items.Clear();
            tseCurso.ResetValue();
            ddlTurno.Items.Clear();
            ddlSerie.Items.Clear();
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Checked = false;
            lblMotivoReabertura.Visible = false;
            ddlMotivoReabertura.Visible = false;
        }

        protected void CarregaAnoReabertura()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlAnoReabertura.Items.Clear();
            ddlAnoReabertura.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.ProximosAnosLetivos, RN.PeriodoLetivo.QueryListaProximosAnos);
            ddlAnoReabertura.DataBind();
            ddlAnoReabertura.Items.Insert(0, item);
        }

        protected void CarregaMotivoReabertura()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlMotivoReabertura.Items.Clear();
            ddlMotivoReabertura.DataSource = RN.Basico.ConsultaItemTabelaValDescr("MotivoReabertura");
            ddlMotivoReabertura.DataBind();
            ddlMotivoReabertura.Items.Insert(0, item);
        }

        protected void ddlAnoReabertura_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

                ddlPeriodoReabertura.Items.Clear();
                ddlSerie.Items.Clear();
                ddlTurno.Items.Clear();
                tseCurso.ResetValue();
                hdnCurriculoReabertura.Value = string.Empty;
                chkEnsReligioso.Enabled = false;
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Enabled = false;
                chkLinguaEstrangeira.Checked = false;

                if (!ddlAnoReabertura.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    ListItem item = new ListItem("Selecione", string.Empty);

                    ddlPeriodoReabertura.DataSource = rnPeriodoLetivo.ListaPeriodosletivosPor(Convert.ToInt32(ddlAnoReabertura.SelectedValue));
                    ddlPeriodoReabertura.DataBind();
                    ddlPeriodoReabertura.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodoReabertura_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlTurno.Items.Clear();
                ddlSerie.Items.Clear();
                tseCurso.ResetValue();
                hdnCurriculoReabertura.Value = string.Empty;
                chkEnsReligioso.Enabled = false;
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Enabled = false;
                chkLinguaEstrangeira.Checked = false;

                if (!ddlPeriodoReabertura.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    SimpleRow sr = RN.Aluno.ConsultarDadosAluno(tseAluno.Value.ToString());
                    hdnCenso.Value = sr["unidade_ensino"].ToString();

                    tseCurso.SqlWhere = " Unidade_ens = '" + hdnCenso.Value + "'";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                ddlSerie.Items.Clear();
                ddlTurno.Items.Clear();
                hdnCurriculoReabertura.Value = string.Empty;
                chkEnsReligioso.Enabled = false;
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Enabled = false;
                chkLinguaEstrangeira.Checked = false;

                if (!tseCurso.DBValue.IsNull || tseCurso.IsValidDBValue)
                {
                    ListItem item = new ListItem("Selecione", string.Empty);

                    ddlTurno.DataSource = RN.Curriculo.ConsultarTurno(tseCurso.DBValue.ToString());
                    ddlTurno.DataBind();
                    ddlTurno.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Serie rnSerie = new Techne.Lyceum.RN.Serie();
            try
            {
                ddlSerie.Items.Clear();
                hdnCurriculoReabertura.Value = string.Empty;
                chkEnsReligioso.Enabled = false;
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Enabled = false;
                chkLinguaEstrangeira.Checked = false;

                if (!ddlAnoReabertura.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodoReabertura.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!tseCurso.DBValue.IsNull || tseCurso.IsValidDBValue))
                {
                    ListItem item = new ListItem("Selecione", string.Empty);

                    ddlSerie.DataSource = rnSerie.ObtemSeries(hdnCenso.Value, Convert.ToDecimal(this.ddlAnoReabertura.SelectedValue), Convert.ToDecimal(this.ddlPeriodoReabertura.SelectedValue), this.ddlTurno.SelectedValue, this.tseCurso.DBValue.ToString());
                    ddlSerie.DataBind();
                    ddlSerie.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LyCurriculo curriculo = new LyCurriculo();
                RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
                hdnCurriculoReabertura.Value = string.Empty;
                chkEnsReligioso.Enabled = false;
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Enabled = false;
                chkLinguaEstrangeira.Checked = false;

                if (!ddlAnoReabertura.SelectedValue.IsNullOrEmptyOrWhiteSpace()
                    && !ddlPeriodoReabertura.SelectedValue.IsNullOrEmptyOrWhiteSpace()
                    && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace()
                    && !ddlSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace()
                    && (!tseCurso.DBValue.IsNull || tseCurso.IsValidDBValue))
                {
                    curriculo = rnCurriculo.ObtemPrimeiroAtivoPor(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, Convert.ToInt32(ddlSerie.SelectedValue), Convert.ToInt32(ddlAnoReabertura.SelectedValue), Convert.ToInt32(ddlPeriodoReabertura.SelectedValue));
                    hdnCurriculoReabertura.Value = curriculo.Curriculo;

                    if (curriculo.EnsinoReligioso == "S")
                    {
                        chkEnsReligioso.Enabled = true;
                    }

                    if (curriculo.LinguaEstrangeira == "S")
                    {
                        chkLinguaEstrangeira.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnReabrir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.HCursosConcl rnHCursosConcl = new Techne.Lyceum.RN.HCursosConcl();
                RN.DTOs.DadosReabertura dadosReabertura = new Techne.Lyceum.RN.DTOs.DadosReabertura();
                ValidacaoDados validacao = new ValidacaoDados();
                decimal pessoa = 0;

                pessoa = (!tseAluno.DBValue.IsNull && this.tseAluno.IsValidDBValue) ? Convert.ToDecimal(tseAluno["pessoa"]) : -1;
                dadosReabertura.Aluno = (!tseAluno.DBValue.IsNull && this.tseAluno.IsValidDBValue) ? Convert.ToString(tseAluno.Value.ToString()) : null;
                dadosReabertura.AnoReabertura = !ddlAnoReabertura.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAnoReabertura.SelectedValue) : -1;
                dadosReabertura.PeriodoReabertura = !ddlPeriodoReabertura.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPeriodoReabertura.SelectedValue) : -1;
                dadosReabertura.CurriculoReabertura = !hdnCurriculoReabertura.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCurriculoReabertura.Value : null;
                dadosReabertura.CursoReabertura = (!tseCurso.DBValue.IsNull && this.tseAluno.IsValidDBValue) ? Convert.ToString(tseCurso.Value.ToString()) : null;

                dadosReabertura.DataReabertura = !dtDataReabertura.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(dtDataReabertura.Text) : DateTime.MinValue;
                dadosReabertura.EnsinoReligioso = chkEnsReligioso.Checked ? true : false;
                dadosReabertura.MotivoReabertura = !ddlMotivoReabertura.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlMotivoReabertura.SelectedValue : null;
                dadosReabertura.LinguaEstrangeira = chkLinguaEstrangeira.Checked ? true : false;
                dadosReabertura.SerieReabertura = !ddlSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSerie.SelectedValue) : -1;
                dadosReabertura.TurnoReabertura = !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurno.SelectedValue : null;
                dadosReabertura.UnidadeEnsino = !hdnCenso.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCenso.Value : null;
                dadosReabertura.UsuarioResponsavel = User.Identity.Name;

                dadosReabertura.AnoEncerramento = !txtAnoEncerramento.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtAnoEncerramento.Text) : -1;
                dadosReabertura.PeriodoEncerramento = !txtPeriodoEncerramento.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtPeriodoEncerramento.Text) : -1;
                dadosReabertura.DataEncerramento = !txtDataEncerramento.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtDataEncerramento.Text) : DateTime.MinValue;
                dadosReabertura.CurriculoAtual = !txtCurriculoEncerramento.Text.IsNullOrEmptyOrWhiteSpace() ? txtCurriculoEncerramento.Text.Trim() : null;
                dadosReabertura.CursoAtual = !txtCurso.Text.IsNullOrEmptyOrWhiteSpace() ? txtCurso.Text.Trim() : null;
                dadosReabertura.DataNascimento = (!tseAluno.DBValue.IsNull && this.tseAluno.IsValidDBValue) ? Convert.ToDateTime(tseAluno["dt_nascimento"].ToString()) : DateTime.MinValue;
                dadosReabertura.SerieAtual = !txtSerie.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtSerie.Text) : -1;
                dadosReabertura.MotivoEncerramento = !txtMotivoEncerramento.Text.IsNullOrEmptyOrWhiteSpace() ? txtMotivoEncerramento.Text.Trim() : null;
                dadosReabertura.TipoVaga = (txtAnoEncerramento.Text == ddlAnoReabertura.SelectedValue && txtPeriodoEncerramento.Text == ddlPeriodoReabertura.SelectedValue) ? "VC" : "VN";
                dadosReabertura.TurnoAtual = !txtTurno.Text.IsNullOrEmptyOrWhiteSpace() ? txtTurno.Text.Trim() : null;
                dadosReabertura.UnidadeEnsino = !hdnCenso.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCenso.Value : null;

                validacao = rnHCursosConcl.ValidaReabertura(dadosReabertura, pessoa);

                if (validacao.Valido)
                {
                    rnHCursosConcl.Reabre(dadosReabertura);
                    pnlGeral.Visible = false;
                    btnReabrir.Visible = false;
                    LimparTela();
                    lblMensagem.Text = "Reabertura realizada com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
