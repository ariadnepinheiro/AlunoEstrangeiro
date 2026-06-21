using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Techne.Controls;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI.HtmlControls;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/RenovacaoMatricula.aspx"),
    ControlText("RenovacaoMatricula"),
    Title("Renovação de Matrícula"),]
    public partial class RenovacaoMatricula : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAluno, "Renovação de Matrícula");
            TituloGrid(grdRenovacoes, "Alunos");

            Modulos.LyceumMaster master = this.Master as Modulos.LyceumMaster;
            if (master != null)
            {
                master.habilitaLoading = true;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcessoGrid();            
            ControlaAcesso(btnIncluir, AcaoControle.novo);
            ControlaAcesso(btnCancelarRenovacao, AcaoControle.excluir);
            ControlaAcesso(btnConfirmar, AcaoControle.novo);
            ControlaAcesso(grdAluno);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                //Desabilitando o botão de comando após o primeiro click do usuário
                btnConfirmar.Attributes.Add("onclick", "document.body.style.cursor = 'wait'; this.value='Aguarde, Enviando...'; this.disabled = true; pucConfirmar.Hide(); " + Page.ClientScript.GetPostBackEventReference(btnConfirmar, string.Empty) + ";");

                if (!IsPostBack)
                {
                    if (!CarregaAgendaRenovacao())
                    {
                        lblMensagem.Text = @"Não existe nenhuma renovação de matrícula disponível no momento.";
                        pnlPesquisa.Visible = false;
                        pnlAluno.Visible = false;
                        pnlRenovacoes.Visible = false;
                    }
                    else
                    {
                        //Inicia com opção Aluno marcada
                        ControlaVisibilidadePesquisaPorAluno();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ControlaAcessoGrid()
        {
            if (grdRenovacoes != null)
            {
                if (!Permission.AllowDelete)
                {
                    grdRenovacoes.Columns[""].Visible = false;
                }
            }
            
            ControlaAcesso(grdRenovacoes);
        }

        private bool CarregaAgendaRenovacao()
        {
            HttpContext.Current.Session["agenda"] = new RN.Agenda.Entidades.Agenda();
            RN.Agenda.Entidades.Agenda agenda = new RN.Agenda.Entidades.Agenda();
            int tipoEventoAgenda = 0;
            DateTime dataRenovacao = DateTime.Now;
            DataTable dtAgenda = new DataTable();

            try
            {
                tipoEventoAgenda = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.RenovacaoMatricula);
                dtAgenda = RN.Agenda.Agenda.ListaAgendaPorTipoEventoEDataEvento(tipoEventoAgenda, dataRenovacao);

                if (dtAgenda.Rows.Count > 0)
                {
                    agenda.AgendaId = Convert.ToInt32(dtAgenda.Rows[0]["AGENDAID"]);
                    agenda.ParticipaUnidadeId = Convert.ToInt32(dtAgenda.Rows[0]["PARTICIPAUNIDADEID"]);
                    agenda.ParticipaCursoId = Convert.ToInt32(dtAgenda.Rows[0]["PARTICIPACURSOID"]);
                    agenda.CursoPorUnidade = Convert.ToBoolean(dtAgenda.Rows[0]["CURSOPORUNIDADE"]);

                    HttpContext.Current.Session["agenda"] = agenda;

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return false;
            }
        }

        protected void rbPesquisaAluno_Changed(object sender, EventArgs args)
        {
            pnlAluno.Visible = false;
            pnlRenovacoes.Visible = false;
            ControlaVisibilidadePesquisaPorAluno();
        }

        protected void rbPesquisaRenovacoes_Changed(object sender, EventArgs args)
        {
            pnlAluno.Visible = false;
            pnlRenovacoes.Visible = false;
            tseAluno.ResetValue();

            ControlaVisibilidadePesquisaPorRenovacoes();
        }

        private void CarregaAno()
        {
            ListItem listItem;
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();

            try
            {
                ddlAno.Items.Clear();
                ddlAno.DataSource = rnRenovacao.ListaAnosComRenovacoes();
                ddlAno.DataBind();
                listItem = new ListItem("Selecione", string.Empty);
                ddlAno.Items.Insert(0, listItem);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaPeriodo(int ano)
        {
            ListItem listItem;
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();

            try
            {
                ddlPeriodo.Items.Clear();
                ddlPeriodo.DataSource = rnRenovacao.ListaPeriodosComRenovacoesPor(ano);
                ddlPeriodo.DataBind();
                listItem = new ListItem("Selecione", string.Empty);
                ddlPeriodo.Items.Insert(0, listItem);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlaVisibilidadePesquisaPorAluno()
        {
            tseAluno.Enabled = true;
            tseRegional.Mode = ControlMode.View;
            tseUnidadeResponsavel.Mode = ControlMode.View;
            tseEscolaridade.Mode = ControlMode.View;
            ddlAno.Enabled = false;
            ddlPeriodo.Enabled = false;
            ddlTurno.Enabled = false;
            ddlSerie.Enabled = false;

            tseRegional.ResetValue();
            tseUnidadeResponsavel.ResetValue();
            tseEscolaridade.ResetValue();
            ddlAno.Items.Clear();
            ddlPeriodo.Items.Clear();
            ddlTurno.Items.Clear();
            ddlSerie.Items.Clear();
        }

        private void ControlaVisibilidadePesquisaPorRenovacoes()
        {
            tseAluno.Enabled = false;
            tseRegional.Mode = ControlMode.Edit;
            tseUnidadeResponsavel.Mode = ControlMode.Edit;
            tseEscolaridade.Mode = ControlMode.Edit;
            ddlAno.Enabled = true;
            ddlPeriodo.Enabled = true;
            ddlTurno.Enabled = true;
            ddlSerie.Enabled = true;

            tseAluno.ResetValue();
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                ControlaVisibilidadePesquisaPorAluno();
                LimpaDadosRenovacaoAluno();
                pnlAluno.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAnoPeriodoRenovacaoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList<RN.DTOs.DadosPossiveisRenovacoes> listaRenovacoes = new List<RN.DTOs.DadosPossiveisRenovacoes>();
            ListItem listItem;
            string tipoBusca = string.Empty;
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            string[] anoperiodo;

            try
            {
                ddlUnidadeEnsinoRenovacaoMatricula.Items.Clear();
                ddlModalidadeRenovacaoMatricula.Items.Clear();
                ddlSerieAluno.Items.Clear();
                ddlTurnoRenovacaoMatricula.Items.Clear();
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Checked = false;
                chkEnsReligioso.Enabled = false;
                chkLinguaEstrangeira.Enabled = false;

                if (!string.IsNullOrEmpty(ddlAnoPeriodoRenovacaoMatricula.SelectedValue))
                {
                    anoperiodo = ddlAnoPeriodoRenovacaoMatricula.SelectedValue.Split('-');
                    tipoBusca = rnRenovacao.ObtemTipoBuscaTurnosPor(Convert.ToInt32(anoperiodo[0]), Convert.ToInt32(anoperiodo[1]));

                    if (tipoBusca != RN.RenovacaoMatricula.Renovacao.TipoBuscaPeriodoInvalido)
                    {
                        //Carrega objeto com todos os dados para ser utilizado em todos os demais combos
                        CarregaRenovacoesPossiveis(tipoBusca);
                        listaRenovacoes = (IList<RN.DTOs.DadosPossiveisRenovacoes>)HttpContext.Current.Session["possiveisRenovacoes" + tseAluno.DBValue.ToString() + ddlAnoPeriodoRenovacaoMatricula.SelectedValue];

                        //Carrega combo unidade ensino renovação
                        ddlUnidadeEnsinoRenovacaoMatricula.DataSource = listaRenovacoes.Select(x => new { x.UnidadeEnsino, x.UnidadeEnsinoNome }).Distinct();
                        ddlUnidadeEnsinoRenovacaoMatricula.DataBind();
                        listItem = new ListItem("Selecione", string.Empty);
                        ddlUnidadeEnsinoRenovacaoMatricula.Items.Insert(0, listItem);
                        ddlUnidadeEnsinoRenovacaoMatricula.Enabled = true;
                    }
                    else
                    {
                        lblMensagem.Text = "A renovação não pode ser efetuada pois não existe um evento de turnos e vagas para o periodo letivo";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlUnidadeEnsinoRenovacaoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList<RN.DTOs.DadosPossiveisRenovacoes> listaRenovacoes = new List<RN.DTOs.DadosPossiveisRenovacoes>();
            ListItem listItem;

            try
            {
                ddlModalidadeRenovacaoMatricula.Items.Clear();
                ddlSerieAluno.Items.Clear();
                ddlTurnoRenovacaoMatricula.Items.Clear();
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Checked = false;
                chkEnsReligioso.Enabled = false;
                chkLinguaEstrangeira.Enabled = false;

                if (!string.IsNullOrEmpty(ddlUnidadeEnsinoRenovacaoMatricula.SelectedValue))
                {
                    listaRenovacoes = (IList<RN.DTOs.DadosPossiveisRenovacoes>)HttpContext.Current.Session["possiveisRenovacoes" + tseAluno.DBValue.ToString() + ddlAnoPeriodoRenovacaoMatricula.SelectedValue];

                    ddlModalidadeRenovacaoMatricula.DataSource = listaRenovacoes.Where(x => x.UnidadeEnsino == ddlUnidadeEnsinoRenovacaoMatricula.SelectedValue).Select(x => new { x.Curso, x.ModalidadeSegmentoCurso }).Distinct();
                    ddlModalidadeRenovacaoMatricula.DataBind();
                    listItem = new ListItem("Selecione", string.Empty);
                    ddlModalidadeRenovacaoMatricula.Items.Insert(0, listItem);
                    ddlModalidadeRenovacaoMatricula.Enabled = true;

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlModalidadeRenovacaoMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList<RN.DTOs.DadosPossiveisRenovacoes> listaRenovacoes = new List<RN.DTOs.DadosPossiveisRenovacoes>();
            ListItem listItem;

            try
            {
                ddlSerieAluno.Items.Clear();
                ddlTurnoRenovacaoMatricula.Items.Clear();
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Checked = false;
                chkEnsReligioso.Enabled = false;
                chkLinguaEstrangeira.Enabled = false;

                if (!string.IsNullOrEmpty(ddlModalidadeRenovacaoMatricula.SelectedValue))
                {
                    listaRenovacoes = (IList<RN.DTOs.DadosPossiveisRenovacoes>)HttpContext.Current.Session["possiveisRenovacoes" + tseAluno.DBValue.ToString() + ddlAnoPeriodoRenovacaoMatricula.SelectedValue];

                    ddlSerieAluno.DataSource = listaRenovacoes.Where(x => x.UnidadeEnsino == ddlUnidadeEnsinoRenovacaoMatricula.SelectedValue && x.Curso == ddlModalidadeRenovacaoMatricula.SelectedValue).Select(x => new { x.SerieSeguinte }).Distinct();
                    ddlSerieAluno.DataBind();
                    listItem = new ListItem("Selecione", string.Empty);
                    ddlSerieAluno.Items.Insert(0, listItem);
                    ddlSerieAluno.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSerieAluno_SelectedIndexChanged(object sender, EventArgs args)
        {
            IList<RN.DTOs.DadosPossiveisRenovacoes> listaRenovacoes = new List<RN.DTOs.DadosPossiveisRenovacoes>();
            ListItem listItem;

            try
            {
                ddlTurnoRenovacaoMatricula.Items.Clear();
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Checked = false;
                chkEnsReligioso.Enabled = false;
                chkLinguaEstrangeira.Enabled = false;

                if (!string.IsNullOrEmpty(ddlSerieAluno.SelectedValue))
                {
                    listaRenovacoes = (IList<RN.DTOs.DadosPossiveisRenovacoes>)HttpContext.Current.Session["possiveisRenovacoes" + tseAluno.DBValue.ToString() + ddlAnoPeriodoRenovacaoMatricula.SelectedValue];

                    ddlTurnoRenovacaoMatricula.DataSource = listaRenovacoes.Where(x => x.UnidadeEnsino == ddlUnidadeEnsinoRenovacaoMatricula.SelectedValue && x.Curso == ddlModalidadeRenovacaoMatricula.SelectedValue && x.SerieSeguinte == int.Parse(ddlSerieAluno.SelectedValue)).Select(x => new { x.Turno, x.TurnoNome }).Distinct();
                    ddlTurnoRenovacaoMatricula.DataBind();
                    listItem = new ListItem("Selecione", string.Empty);
                    ddlTurnoRenovacaoMatricula.Items.Insert(0, listItem);
                    ddlTurnoRenovacaoMatricula.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurnoRenovacaoMatricula_SelectedIndexChanged(object sender, EventArgs args)
        {
            string[] anoperiodo;
            DataTable dtCurriculo = new DataTable();

            try
            {
                //Habilita Lingua Estrangeira e/ou Ensino Religioso
                anoperiodo = ddlAnoPeriodoRenovacaoMatricula.SelectedValue.Split('-');
                chkEnsReligioso.Enabled = false;
                chkLinguaEstrangeira.Enabled = false;

                if (!string.IsNullOrEmpty(ddlTurnoRenovacaoMatricula.SelectedValue.ToString()))
                {
                    dtCurriculo = RN.Curriculo.ListaCurriculoRenovacaoMatriculaPor(
                        Convert.ToInt32(anoperiodo[0])
                        , Convert.ToInt32(anoperiodo[1])
                        , ddlUnidadeEnsinoRenovacaoMatricula.SelectedValue.ToString()
                        , ddlModalidadeRenovacaoMatricula.SelectedValue.ToString()
                        , ddlTurnoRenovacaoMatricula.SelectedValue.ToString());

                    if (dtCurriculo.Rows.Count > 0)
                    {
                        if (dtCurriculo.Rows[0]["PODE_LINGUA_ESTRANGEIRA"].ToString() == "S")
                        {
                            chkLinguaEstrangeira.Enabled = true;
                        }
                        else
                        {
                            chkLinguaEstrangeira.Enabled = false;
                        }

                        if (dtCurriculo.Rows[0]["PODE_ENSINO_RELIGIOSO"].ToString() == "S")
                        {
                            chkEnsReligioso.Enabled = true;
                        }
                        else
                        {
                            chkEnsReligioso.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, EventArgs args)
        {
            try
            {
                tseUnidadeResponsavel.SqlWhere = " situacao = 'ESTADUAL'";
                tseUnidadeResponsavel.ResetValue();
                tseEscolaridade.ResetValue();
                ddlAno.ClearSelection();
                ddlPeriodo.Items.Clear();
                ddlTurno.Items.Clear();
                ddlSerie.Items.Clear();
                grdRenovacoes.DataSource = null;
                grdRenovacoes.DataBind();
                pnlRenovacoes.Visible = false;

                if (Page.IsCallback)
                {
                    return;
                }
                if (!this.tseRegional.DBValue.IsNull && this.tseRegional.IsValidDBValue)
                {
                    tseUnidadeResponsavel.SqlWhere = tseUnidadeResponsavel.SqlWhere + " and id_regional='" + tseRegional.DBValue.ToString() + "'";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, EventArgs args)
        {
            tseEscolaridade.ResetValue();
            ddlAno.ClearSelection();
            ddlPeriodo.Items.Clear();
            ddlTurno.Items.Clear();
            ddlSerie.Items.Clear();
            grdRenovacoes.DataSource = null;
            grdRenovacoes.DataBind();
            pnlRenovacoes.Visible = false;

            if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel["unidade_ens"].IsNull)
            {
                tseRegional.Value = tseUnidadeResponsavel["id_regional"];
            }
        }

        protected void tseEscolaridade_Changed(object sender, EventArgs args)
        {
            ddlAno.ClearSelection();
            ddlPeriodo.Items.Clear();
            ddlTurno.Items.Clear();
            ddlSerie.Items.Clear();
            grdRenovacoes.DataSource = null;
            grdRenovacoes.DataBind();
            pnlRenovacoes.Visible = false;

            if (!this.tseEscolaridade.DBValue.IsNull && this.tseEscolaridade.IsValidDBValue)
            {
                CarregaAno();
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs args)
        {
            ddlPeriodo.Items.Clear();
            ddlTurno.Items.Clear();
            ddlSerie.Items.Clear();
            grdRenovacoes.DataSource = null;
            grdRenovacoes.DataBind();
            pnlRenovacoes.Visible = false;

            if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
            {
                CarregaPeriodo(Convert.ToInt32(ddlAno.SelectedValue));
            }
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs args)
        {
            ddlTurno.Items.Clear();
            ddlSerie.Items.Clear();
            grdRenovacoes.DataSource = null;
            grdRenovacoes.DataBind();
            pnlRenovacoes.Visible = false;

            if ((!tseEscolaridade.DBValue.IsNull && tseEscolaridade.IsValidDBValue)
                && (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue)
                && !string.IsNullOrEmpty(ddlAno.SelectedValue)
                && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
            {
                CarregaTurno(tseUnidadeResponsavel.DBValue.ToString(), tseEscolaridade.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue));
            }
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs args)
        {
            ddlSerie.Items.Clear();
            grdRenovacoes.DataSource = null;
            grdRenovacoes.DataBind();
            pnlRenovacoes.Visible = false;

            if ((!tseEscolaridade.DBValue.IsNull && tseEscolaridade.IsValidDBValue)
                    && (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue)
                    && !string.IsNullOrEmpty(ddlTurno.SelectedValue)
                    && !string.IsNullOrEmpty(ddlAno.SelectedValue)
                    && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
            {
                CarregaSerie(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), tseUnidadeResponsavel.DBValue.ToString(), tseEscolaridade.Value.ToString(), ddlTurno.SelectedValue.ToString());
            }
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs args)
        {
            grdRenovacoes.DataSource = null;
            grdRenovacoes.DataBind();
            pnlRenovacoes.Visible = false;
        }

        private void CarregaTurno(string unidadeEnsino, string curso, int ano, int periodo)
        {
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            ListItem listItem;

            try
            {
                ddlTurno.Items.Clear();
                ddlTurno.DataSource = rnRenovacao.ListaTurnosComRenovacoesPor(ano, periodo, unidadeEnsino, curso);
                ddlTurno.DataBind();
                listItem = new ListItem("Selecione", string.Empty);
                ddlTurno.Items.Insert(0, listItem);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaSerie(int ano, int periodo, string unidadeEnsino, string curso, string turno)
        {
            ListItem listItem;
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();

            try
            {
                ddlSerie.Items.Clear();

                ddlSerie.DataSource = rnRenovacao.ListaSeriesComRenovacoesPor(ano, periodo, unidadeEnsino, curso, turno);
                ddlSerie.DataBind();
                listItem = new ListItem("Selecione", string.Empty);
                ddlSerie.Items.Insert(0, listItem);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaRenovacoesPossiveis(string tipoBusca)
        {
            HttpContext.Current.Session["possiveisRenovacoes" + tseAluno.DBValue.ToString() + ddlAnoPeriodoRenovacaoMatricula.SelectedValue] = new List<RN.DTOs.DadosPossiveisRenovacoes>();
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new RN.RenovacaoMatricula.Renovacao();
            IList<RN.DTOs.DadosPossiveisRenovacoes> listaRenovacoes = new List<RN.DTOs.DadosPossiveisRenovacoes>();
            RN.Agenda.Entidades.Agenda agenda = new Techne.Lyceum.RN.Agenda.Entidades.Agenda();
            DateTime dataNascimentoAluno = DateTime.Now;
            string[] anoperiodo;
            int proximoAno = -1;
            int proximoPeriodo = -1;
            string aluno = string.Empty;

            try
            {
                anoperiodo = ddlAnoPeriodoRenovacaoMatricula.SelectedValue.ToString().Split('-');
                agenda = (RN.Agenda.Entidades.Agenda)HttpContext.Current.Session["agenda"];
                proximoAno = Convert.ToInt32(anoperiodo[0]);
                proximoPeriodo = Convert.ToInt32(anoperiodo[1]);
                aluno = tseAluno.Value.ToString();

                if (tseAluno["dt_nascimento"] != System.DBNull.Value)
                {
                    dataNascimentoAluno = Convert.ToDateTime(tseAluno["dt_nascimento"]);
                }

                listaRenovacoes = rnRenovacao.ListaPossiveisRenovacoesPor(agenda, proximoAno, proximoPeriodo, aluno, dataNascimentoAluno, tipoBusca);

                HttpContext.Current.Session["possiveisRenovacoes" + tseAluno.DBValue.ToString() + ddlAnoPeriodoRenovacaoMatricula.SelectedValue] = listaRenovacoes;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaAnoPeriodoRenovacaoPor(string aluno)
        {
            ListItem listItem;
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new RN.RenovacaoMatricula.Renovacao();
            RN.Agenda.Entidades.Agenda agenda = new RN.Agenda.Entidades.Agenda();
            DataTable dtAnoPeriodo = new DataTable();
            ddlAnoPeriodoRenovacaoMatricula.Items.Clear();
            ddlAnoPeriodoRenovacaoMatricula.DataBind();

            try
            {
                agenda = (RN.Agenda.Entidades.Agenda)HttpContext.Current.Session["agenda"];
                dtAnoPeriodo = rnRenovacao.ListaProximoAnoPeriodoParaRenovacaoPor(aluno, agenda.AgendaId);

                if (dtAnoPeriodo != null && dtAnoPeriodo.Rows.Count != 0)
                {
                    ddlAnoPeriodoRenovacaoMatricula.DataSource = dtAnoPeriodo;
                    ddlAnoPeriodoRenovacaoMatricula.DataBind();
                    listItem = new ListItem("Selecione", string.Empty);
                    ddlAnoPeriodoRenovacaoMatricula.Items.Insert(0, listItem);
                    ddlAnoPeriodoRenovacaoMatricula.Enabled = true;
                }
                else
                {
                    lblMensagem.Text = "A renovação não está disponível para o período que o aluno está matriculado.";
                    LimpaDadosRenovacaoAluno();
                    pnlAluno.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAluno_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregaGridAluno();
        }

        protected void grdRenovacoes_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregaGridRenovacao();
        }

        private void CarregaGridAluno()
        {
            Techne.Lyceum.RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();

            try
            {
                grdAluno.DataSource = rnRenovacao.ListaRenovacaoMatriculaPor(tseAluno.DBValue.ToString());
                grdAluno.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaGridRenovacao()
        {
            Techne.Lyceum.RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();

            try
            {
                if (!this.tseUnidadeResponsavel.DBValue.IsNull && !this.tseEscolaridade.DBValue.IsNull && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue) && !string.IsNullOrEmpty(ddlSerie.SelectedValue) && !string.IsNullOrEmpty(ddlTurno.SelectedValue))
                {
                    grdRenovacoes.DataSource = rnRenovacao.ListaRenovacoesMatriculasPor(tseUnidadeResponsavel.DBValue.ToString(), tseEscolaridade.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlSerie.SelectedValue, ddlTurno.SelectedValue);
                }
                else
                {
                    grdRenovacoes.DataSource = null;
                    lblMensagem.Text = "Os campos Ano/Periodo/Unidade de Ensino/Curso/Série/Turno são de preenchimento obrigatórios.";
                }

                grdRenovacoes.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs args)
        {
            List<string> mensagens = new List<string>();

            try
            {
                if (rbPesquisaAluno.Checked)
                {
                    hdnAluno.Value = string.Empty;
                    if (tseAluno.DBValue.IsNull || !tseAluno.IsValidDBValue)
                    {
                        lblMensagem.Text = "Campo Aluno é obrigatório.";
                        return;
                    }
                    else
                    {
                        hdnAluno.Value = tseAluno.DBValue.ToString();
                        CarregaGridAluno();
                        pnlAluno.Visible = true;
                        CarregaAnoPeriodoRenovacaoPor(tseAluno.DBValue.ToString());
                    }
                }

                if (rbPesquisaRenovacoes.Checked)
                {
                    if ((tseUnidadeResponsavel.Value == null) || (string.IsNullOrEmpty(tseUnidadeResponsavel.Value.ToString())))
                    {
                        mensagens.Add("O campo Unidade de Ensino é obrigatório.");
                    }

                    if ((tseEscolaridade.Value == null) || (string.IsNullOrEmpty(tseEscolaridade.Value.ToString())))
                    {
                        mensagens.Add("O campo Escolaridade é obrigatório.");
                    }

                    if (ddlAno.SelectedIndex <= 0)
                    {
                        mensagens.Add("O campo Ano é obrigatório.");
                    }

                    if (ddlPeriodo.SelectedIndex <= 0)
                    {
                        mensagens.Add("O campo Período é obrigatório.");
                    }

                    if ((ddlTurno.SelectedValue == null) || (string.IsNullOrEmpty(ddlTurno.SelectedValue.ToString())))
                    {
                        mensagens.Add("O campo Turno é obrigatório.");
                    }

                    if ((ddlSerie.SelectedValue == null) || (string.IsNullOrEmpty(ddlSerie.SelectedValue.ToString())))
                    {
                        mensagens.Add("O campo Série é obrigatório.");
                    }
                    if (mensagens.Count > 0)
                    {
                        lblMensagem.Text = mensagens.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />"); ;
                    }
                    else
                    {
                        CarregaGridRenovacao();
                        pnlRenovacoes.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaDadosRenovacaoAluno()
        {
            ddlAnoPeriodoRenovacaoMatricula.Items.Clear();
            ddlAnoPeriodoRenovacaoMatricula.DataBind();
            ddlUnidadeEnsinoRenovacaoMatricula.Items.Clear();
            ddlUnidadeEnsinoRenovacaoMatricula.DataBind();
            ddlModalidadeRenovacaoMatricula.Items.Clear();
            ddlModalidadeRenovacaoMatricula.DataBind();
            ddlSerieAluno.Items.Clear();
            ddlSerieAluno.DataBind();
            ddlTurnoRenovacaoMatricula.Items.Clear();
            ddlTurnoRenovacaoMatricula.DataBind();

            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Checked = false;
            chkEnsReligioso.Enabled = false;
            chkLinguaEstrangeira.Enabled = false;
            grdAluno.DataSource = null;
            grdAluno.DataBind();
        }

        private RN.RenovacaoMatricula.Entidades.Renovacao MontaRenovacaoMatricula()
        {
            RN.RenovacaoMatricula.Entidades.Renovacao renovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Entidades.Renovacao();
            string tipoVaga = string.Empty;
            string[] anoperiodo = null;

            try
            {
                if (!string.IsNullOrEmpty(ddlAnoPeriodoRenovacaoMatricula.SelectedValue) && !string.IsNullOrEmpty(ddlAnoPeriodoRenovacaoMatricula.SelectedValue))
                {
                    anoperiodo = ddlAnoPeriodoRenovacaoMatricula.SelectedValue.ToString().Split('-');
                }

                tipoVaga = RetornaTipoVagaParaRenovacaoPor(Convert.ToString(tseAluno.Value.ToString()), ddlUnidadeEnsinoRenovacaoMatricula.SelectedValue);

                renovacao = new RN.RenovacaoMatricula.Entidades.Renovacao()
                {
                    AlunoId = Convert.ToString(tseAluno.Value),
                    UnidadeEnsinoId = Convert.ToString(ddlUnidadeEnsinoRenovacaoMatricula.SelectedValue),
                    CursoId = Convert.ToString(ddlModalidadeRenovacaoMatricula.SelectedValue),
                    TurnoId = Convert.ToString(ddlTurnoRenovacaoMatricula.SelectedValue),
                    Ano = anoperiodo == null ? -1 : Convert.ToInt32(anoperiodo[0]),
                    Periodo = anoperiodo == null ? -1 : Convert.ToInt32(anoperiodo[1]),
                    Serie = string.IsNullOrEmpty(Convert.ToString(ddlSerieAluno.SelectedValue)) ? -1 : Convert.ToInt32(ddlSerieAluno.SelectedValue),
                    SituacaoRenovacaoId = Convert.ToInt32(RN.RenovacaoMatricula.Entidades.SituacaoRenovacao.Ativo),
                    Usuario = User.Identity.Name,
                    DataAlteracao = DateTime.Now,
                    TipoVaga = tipoVaga,
                    EnsinoReligioso = chkEnsReligioso.Checked,
                    LinguaEstrangeira = chkLinguaEstrangeira.Checked,
                    DataCadastro = DateTime.Now,
                };
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            return renovacao;
        }

        protected void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupFinalizar();", true);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            RN.RenovacaoMatricula.Entidades.Renovacao renovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Entidades.Renovacao();
            ValidacaoDados validaRenovacao = new ValidacaoDados();

            try
            {
                //Simula a carga no servidor por 2 segundos
                //System.Threading.Thread.Sleep(2000);

                this.pucConfirmar.ShowOnPageLoad = false;

                renovacao = MontaRenovacaoMatricula();
                validaRenovacao = rnRenovacao.Valida(renovacao);

                if (validaRenovacao.Valido)
                {
                    rnRenovacao.InsereRenovacaoMatricula(renovacao);
                    LimpaDadosRenovacaoAluno();

                    //Recarrega renovaçoes
                    grdAluno.DataSource = rnRenovacao.ListaRenovacaoMatriculaPor(renovacao.AlunoId);
                    grdAluno.DataBind();

                    ddlRenovacaoMatricula.DataBind();
                    lblMensagem.Text = "Renovação incluida com sucesso.";
                    HttpContext.Current.Session.Remove("possiveisRenovacoes" + tseAluno.DBValue.ToString() + ddlAnoPeriodoRenovacaoMatricula.SelectedValue);

                }
                else
                {
                    lblMensagem.Text = validaRenovacao.Mensagem;
                }

                updatePanel3.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                updatePanel3.Update();
            }
        }

        protected void btnImprimirRenovacao_Click(object sender, EventArgs e)
        {
            RN.RenovacaoMatricula.Entidades.LogImpressaoFichaRenovacao dtImprimirLog = new Techne.Lyceum.RN.RenovacaoMatricula.Entidades.LogImpressaoFichaRenovacao();
            RN.RenovacaoMatricula.LogImpressaoFichaRenovacao rnLogImpressaoFichaRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.LogImpressaoFichaRenovacao();

            try
            {
                if (ddlRenovacaoMatricula.Value != null && !hdnAluno.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    //Gravar log de impressao
                    dtImprimirLog = new RN.RenovacaoMatricula.Entidades.LogImpressaoFichaRenovacao
                    {
                        AlunoId = hdnAluno.Value.ToString(),
                        RenovacaoId = Convert.ToInt32(ddlRenovacaoMatricula.Value.ToString()),
                        Matricula = User.Identity.Name
                    };

                    rnLogImpressaoFichaRenovacao.Insere(dtImprimirLog);

                    //Abrir Ficha
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(ddlRenovacaoMatricula.Value.ToString());
                    StringBuilder strLink = new StringBuilder();

                    strLink.Append("pagina = " + @"'FichaRenovacao.aspx?Chave=" + Convert.ToBase64String(bytesToEncode) + "'" + ";");
                    strLink.Append("abriu = false;");
                    strLink.Append("function abrir() {newWindow = window.open(pagina, 'nova', 'status=no, scrollbars=yes, resizable=yes, width=850, height=800'); if (newWindow) {abriu = true;   return false;}}");
                    strLink.Append("abrir();");
                    strLink.Append("abrirPopupBloqueado();");

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), strLink.ToString(), true);

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private string RetornaTipoVagaParaRenovacaoPor(string aluno, string unidadeEnsinoDestino)
        {
            RN.Matricula rnMatricula = new RN.Matricula();
            string tipoVaga = string.Empty;
            DadosEnturmacaoAluno dadosAtualAluno = new DadosEnturmacaoAluno();

            try
            {
                //Obtem Escola Atual do aluno
                dadosAtualAluno = rnMatricula.ObtemMatriculaPrincipalAtivaPor(aluno);

                //se for a mesma unidade de ensino: VC | se for outra unidade: VN
                if (unidadeEnsinoDestino == dadosAtualAluno.Censo)
                {
                    tipoVaga = "VC";
                }
                else
                {
                    tipoVaga = "VN";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            return tipoVaga;
        }

        protected void btnCancelarRenovacao_Click(object sender, EventArgs args)
        {
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            List<int> itensSelecionados = new List<int>();

            try
            {
                if (!this.tseUnidadeResponsavel.DBValue.IsNull && !this.tseEscolaridade.DBValue.IsNull && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue) && !string.IsNullOrEmpty(ddlSerie.SelectedValue) && !string.IsNullOrEmpty(ddlTurno.SelectedValue))
                {

                    itensSelecionados = this.grdRenovacoes
                        .GetSelectedFieldValues("RENOVACAOID")
                         .Select(x => int.Parse(x.ToString()))
                       .ToList();

                    if (itensSelecionados.Count() > 0)
                    {
                        rnRenovacao.CancelaRenovacoes(itensSelecionados, User.Identity.Name);

                        lblMensagem.Text = "Renovação(ões) cancelada(s) com sucesso.";

                        grdRenovacoes.DataSource = rnRenovacao.ListaRenovacoesMatriculasPor(tseUnidadeResponsavel.DBValue.ToString(), tseEscolaridade.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlSerie.SelectedValue, ddlTurno.SelectedValue);
                        grdRenovacoes.DataBind();
                        this.grdRenovacoes.Selection.UnselectAll();
                    }
                    else
                    {
                        lblMensagem.Text = "Para cancelar é necessário selecionar pelo menos uma renovação.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Os campos Ano/Período/Unidade de Ensino/Curso/Série/Turno são de preenchimento obrigatórios.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected bool VerificaCheck(object valor)
        {
            if (valor is DBNull)
            {
                return false;
            }
            if (valor is string)
            {
                return (string)valor == "S";
            }
            if (valor is bool)
            {
                return (bool)valor;
            }
            return false;
        }
    }
}
