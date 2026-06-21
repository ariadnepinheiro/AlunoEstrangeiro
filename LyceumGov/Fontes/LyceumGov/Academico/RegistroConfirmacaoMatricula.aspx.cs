using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using Techne.Controls;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/RegistroConfirmacaoMatricula.aspx"), ControlText("Registro Confirmação de Matricula"), Title("Registro Confirmação de Matricula")]

    public partial class RegistroConfirmacaoMatricula : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.PadroesDeAcessos rnPadroesDeAcessos = new PadroesDeAcessos();
                RN.Pedagogico.PeriodoConfirmacao rnPeriodoConfirmacao = new Techne.Lyceum.RN.Pedagogico.PeriodoConfirmacao();

                lblMensagem.Text = string.Empty;
                lblMensagemAluno.Text = string.Empty;

                if (!IsPostBack)
                {
                    CarregaPeriodo();
                    CarregaAno();

                    pnBusca.Visible = true;

                    if (rnPadroesDeAcessos.PossuiPadraoDiretorPor(User.Identity.Name) && !rnPeriodoConfirmacao.ObtemPeriodoAberto())
                    {
                        lblMensagem.Text = "Não existe Período de Liberação do Registro de Confirmação vigente.";
                        pnBusca.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;

            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdConfirmacao, "Histórico de Confirmação de Matrícula");

        }
        private void CarregaAno()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlAno.Items.Clear();
            ddlAno.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.ProximosAnosLetivos, RN.PeriodoLetivo.QueryListaProximosAnos);
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        private void CarregaPeriodo()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlPeriodo.Items.Clear();
            ddlPeriodo.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.PeriodoLetivo, RN.PeriodoLetivo.QueryListaPeriodos);
            ddlPeriodo.DataBind();
            ddlPeriodo.Items.Insert(0, item);
        }

        private void CarregaModalidade()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object objModalidade = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Modalidade, RN.Curso.QueryListaModalidadeCurso);

            ddlModalidade.Items.Clear();
            ddlModalidade.DataSource = objModalidade;
            ddlModalidade.DataBind();
            ddlModalidade.Items.Insert(0, item);

        }

        private void CarregaNivel()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object objNivel = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Nivel, RN.Curso.QueryListaTipoCurso);

            ddlNivel.Items.Clear();
            ddlNivel.DataSource = objNivel;
            ddlNivel.DataBind();
            ddlNivel.Items.Insert(0, item);
        }
        private void CarregaTurno()
        {
            RN.Turno rnTurno = new Turno();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlTurno.Items.Clear();
            ddlTurno.DataSource = rnCurriculo.ObtemListaTurnoPor(tseCurso.DBValue.ToString());
            ddlTurno.DataBind();
            ddlTurno.Items.Insert(0, item);
        }

        private void CarregaSerie()
        {
            RN.Serie rnSerie = new Serie();


            ListItem item = new ListItem("Selecione", string.Empty);
            ddlSerie.Items.Clear();

            if ((tseCurso.IsValidDBValue && !tseCurso.DBValue.IsNull) && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                ddlSerie.DataSource = rnSerie.ListaSeriePor(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue.ToString());
            }
            ddlSerie.DataBind();
            ddlSerie.Items.Insert(0, item);
        }

        private void LimparTela()
        {
            hdnCurriculo.Value = string.Empty;
            ddlTipoCurso.ClearSelection();
            ddlNivel.ClearSelection();            
            ddlSerie.Items.Clear();
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Checked = false;
            grdConfirmacao.DataSource = null;
            grdConfirmacao.DataBind();
            tseCurso.ResetValue();
            CarregarFiltroCurso();
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();

                if (Page.IsCallback)
                {
                    return;
                }

                LimparTela();
                pnlGeral.Visible = false;

                ddlAno.ClearSelection();
                ddlPeriodo.ClearSelection();
                ddlAno.Enabled = false;

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                        ddlAno.Enabled = true;
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";

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
                LimparTela();
                ddlPeriodo.ClearSelection();
                ddlPeriodo.Enabled = false;
                pnlGeral.Visible = false;
                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    ddlPeriodo.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();

                LimparTela();
                pnlGeral.Visible = false;
                if (!ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    pnlGeral.Visible = true;
                    CarregaModalidade();
                    CarregaNivel();
                    CarregaGrid();

                    if (rnMatricula.PossuiMatriculaAtivaPossiveisPeriodosPor(tseAluno.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue)))
                    {
                        lblMensagemAluno.Text = "Aluno já se encontra enturmado. Para realizar a operação será necessário o cancelamento da enturmação.";
                        pnlGeral.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void ddlModalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseCurso.ResetValue();
                CarregarFiltroCurso();
                ddlSerie.Items.Clear();
                ddlTurno.Items.Clear();
                chkLinguaEstrangeira.Checked = false;
                chkEnsReligioso.Checked = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseCurso.ResetValue();
                CarregarFiltroCurso();
                ddlModalidade.ClearSelection();
                ddlSerie.Items.Clear();
                ddlTurno.Items.Clear();
                chkLinguaEstrangeira.Checked = false;
                chkEnsReligioso.Checked = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarFiltroCurso()
        {
            if (!tseAluno["UNIDADE_ENSINO"].ToString().IsNullOrEmptyOrWhiteSpace() && !ddlNivel.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlModalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                tseCurso.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseAluno["UNIDADE_ENSINO"].ToString()) + "' and modalidade = '" + RN.RNBase.MudarAspas(ddlModalidade.SelectedValue.ToString()) + "' and tipo = '" + RN.RNBase.MudarAspas(ddlNivel.SelectedValue.ToString()) + "'";
            else if (!tseAluno["UNIDADE_ENSINO"].ToString().IsNullOrEmptyOrWhiteSpace() && ddlNivel.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlModalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                tseCurso.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseAluno["UNIDADE_ENSINO"].ToString()) + "' and modalidade = '" + RN.RNBase.MudarAspas(ddlModalidade.SelectedValue.ToString()) + "'";
            else if (!tseAluno["UNIDADE_ENSINO"].ToString().IsNullOrEmptyOrWhiteSpace() && !ddlNivel.SelectedValue.IsNullOrEmptyOrWhiteSpace() && ddlModalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                tseCurso.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseAluno["UNIDADE_ENSINO"].ToString()) + "' and tipo = '" + RN.RNBase.MudarAspas(ddlNivel.SelectedValue.ToString()) + "'";
            else if (!tseAluno["UNIDADE_ENSINO"].ToString().IsNullOrEmptyOrWhiteSpace() && ddlNivel.SelectedValue.IsNullOrEmptyOrWhiteSpace() && ddlModalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                tseCurso.SqlWhere = "pc.curso is null and UNIDADE_ENS = '" + RN.RNBase.MudarAspas(tseAluno["UNIDADE_ENSINO"].ToString()) + "'";

            tseCurso.DataBind();
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSerie.Items.Clear();
                if (!ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (!string.IsNullOrEmpty(ddlTurno.SelectedValue) && (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue))
                    {
                        CarregaSerie();
                    }
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
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Checked = false;

                if (!tseCurso.DBValue.IsNull)
                {
                    if (tseCurso.IsValidDBValue)
                    {
                        CarregaTurno();
                        ddlSerie.Items.Clear();
                        if (!string.IsNullOrEmpty(ddlTurno.SelectedValue) && !tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue)
                        {
                            CarregaSerie();
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Curso não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Curso não ativo ou não cadastrado (favor verificar).";
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
                hdnCurriculo.Value = string.Empty;
                chkEnsReligioso.Enabled = false;
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Enabled = false;
                chkLinguaEstrangeira.Checked = false;

                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() &&
                   (!tseCurso.DBValue.IsNull || tseCurso.IsValidDBValue) && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() &&
                    !ddlSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    //Busca Curriculo
                    curriculo = rnCurriculo.ObtemPrimeiroAtivoPor(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue, Convert.ToInt32(ddlSerie.SelectedValue), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue));
                    hdnCurriculo.Value = curriculo.Curriculo;

                    //Verifica se o curriculo permite ensino religioso
                    if (curriculo.EnsinoReligioso == "S")
                    {
                        chkEnsReligioso.Enabled = true;
                    }

                    //Verifica se o curriculo permite lingua estrangeira
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

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
                RN.Entidades.TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
                ValidacaoDados validacao = new ValidacaoDados();

                confirmacaoMatricula.Aluno = (!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue) ? tseAluno.DBValue.ToString() : null;
                confirmacaoMatricula.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(ddlAno.SelectedValue) : -1;
                confirmacaoMatricula.Periodo = !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(ddlPeriodo.SelectedValue) : -1;
                confirmacaoMatricula.Curriculo = !hdnCurriculo.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCurriculo.Value : null;
                confirmacaoMatricula.Censo = (!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue) ? tseAluno["UNIDADE_ENSINO"].ToString() : null;
                confirmacaoMatricula.Curso = (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) ? tseCurso.DBValue.ToString() : null;
                confirmacaoMatricula.EnsinoReligioso = chkEnsReligioso.Checked;
                confirmacaoMatricula.LinguaEstrangeiraFacultativa = chkLinguaEstrangeira.Checked;
                confirmacaoMatricula.Matricula = User.Identity.Name;
                confirmacaoMatricula.Serie = !ddlSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(ddlSerie.SelectedValue) : -1;
                confirmacaoMatricula.Status = null;
                confirmacaoMatricula.TipoVagaOcupada = "VC";
                confirmacaoMatricula.Turno = !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurno.SelectedValue : null;

                validacao = rnConfirmacaoMatricula.Valida(confirmacaoMatricula);

                if (validacao.Valido)
                {
                    rnConfirmacaoMatricula.Insere(confirmacaoMatricula);

                    lblMensagem.Text = "Confirmação de matricula incluída com sucesso.";
                    CarregaGrid();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />"); ;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            ddlNivel.ClearSelection();
            ddlModalidade.ClearSelection();           
            ddlTurno.Items.Clear();
            ddlSerie.Items.Clear();
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Checked = false;
            tseCurso.ResetValue();
            CarregarFiltroCurso();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlGeral.Visible = false;
            LimparTela();
            tseAluno.ResetValue();
            ddlAno.ClearSelection();
            ddlPeriodo.ClearSelection();
            ddlAno.Enabled = false;
            ddlPeriodo.Enabled = false;
        }

        protected bool VerificarCheck(object valor)
        {
            if (valor is DBNull)
            {
                return false;
            }

            if (valor is string)
            {
                return (string)valor == "1";
            }

            if (valor is bool)
            {
                return (bool)valor;
            }
            return false;
        }

        private void CarregaGrid()
        {
            try
            {
                RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
                if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
                {
                    grdConfirmacao.DataSource = rnConfirmacaoMatricula.ListaPossiveisConfirmacaoMatriculaPor(tseAluno.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue));
                    grdConfirmacao.DataBind();

                    if (grdConfirmacao.VisibleRowCount > 0)
                    {
                        grdConfirmacao.Visible = true;
                    }
                    else
                    {
                        grdConfirmacao.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
