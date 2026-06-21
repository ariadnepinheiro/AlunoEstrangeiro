using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using Techne.Data;
using Techne.Controls;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Helpers;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/MatriculaEletiva.aspx"),
  ControlText("Enturmação Eletiva"),
  Title("Enturmação Eletiva")]
    public partial class MatriculaEletiva : TPage
    {
        public object Lista(object aluno, object turma, object ano, object periodo)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();

            var matricula = Convert.ToString(aluno);
            var anofiltro = Convert.ToString(ano);
            var periodofiltro = Convert.ToString(periodo);
            var turmafiltro = Convert.ToString(turma);

            if (!matricula.IsNullOrEmptyOrWhiteSpace() && !anofiltro.IsNullOrEmptyOrWhiteSpace() && !periodofiltro.IsNullOrEmptyOrWhiteSpace())
            {
                return rnMatricula.ListaMatriculasEletivasPorAluno(matricula, Convert.ToInt32(anofiltro), Convert.ToInt32(periodofiltro));
            }

            if (matricula.IsNullOrEmptyOrWhiteSpace() && !turmafiltro.IsNullOrEmptyOrWhiteSpace() && !anofiltro.IsNullOrEmptyOrWhiteSpace() && !periodofiltro.IsNullOrEmptyOrWhiteSpace())
            {
                return rnMatricula.ListaMatriculasEletivasPor(turmafiltro, Convert.ToInt32(anofiltro), Convert.ToInt32(periodofiltro));
            }

            return null;
        }

        public object ListarGrupo1(object unidade_ens, object ano, object periodo, object curso, object serie, object turno, object itinerario, object tipo, object modalidade)
        {
            RN.Turma rnTurma = new Turma();

            var anofiltro = Convert.ToString(ano);
            var periodofiltro = Convert.ToString(periodo);
            var unidade = Convert.ToString(unidade_ens);
            var cursofiltro = Convert.ToString(curso);
            var seriefiltro = Convert.ToString(serie);
            var turnofiltro = Convert.ToString(turno);
            var itinerariofiltro = Convert.ToString(itinerario);

            if (!unidade.IsNullOrEmptyOrWhiteSpace() && !anofiltro.IsNullOrEmptyOrWhiteSpace() && !periodofiltro.IsNullOrEmptyOrWhiteSpace() && !cursofiltro.IsNullOrEmptyOrWhiteSpace() && !seriefiltro.IsNullOrEmptyOrWhiteSpace() && !turnofiltro.IsNullOrEmptyOrWhiteSpace())
            {
                return rnTurma.ListaTurmaEletivaAbertaComVagaPor(unidade_ens.ToString(), 1, Convert.ToInt32(ano), Convert.ToInt32(periodo), curso.ToString(), Convert.ToInt32(serie), turno.ToString(),itinerariofiltro, Convert.ToString(tipo), Convert.ToString(modalidade));
            }

            return null;
        }

        public object ListarGrupo2(object unidade_ens, object ano, object periodo, object curso, object serie, object turno, object itinerario, object tipo, object modalidade)
        {
            RN.Turma rnTurma = new Turma();

            var anofiltro = Convert.ToString(ano);
            var periodofiltro = Convert.ToString(periodo);
            var unidade = Convert.ToString(unidade_ens);
            var cursofiltro = Convert.ToString(curso);
            var seriefiltro = Convert.ToString(serie);
            var turnofiltro = Convert.ToString(turno);
            var itinerariofiltro = Convert.ToString(itinerario);

            if (!unidade.IsNullOrEmptyOrWhiteSpace() && !anofiltro.IsNullOrEmptyOrWhiteSpace() && !periodofiltro.IsNullOrEmptyOrWhiteSpace() && !cursofiltro.IsNullOrEmptyOrWhiteSpace() && !seriefiltro.IsNullOrEmptyOrWhiteSpace() && !turnofiltro.IsNullOrEmptyOrWhiteSpace())
            {
                return rnTurma.ListaTurmaEletivaAbertaComVagaPor(unidade_ens.ToString(), 2, Convert.ToInt32(ano), Convert.ToInt32(periodo), curso.ToString(), Convert.ToInt32(serie), turno.ToString(), itinerariofiltro, Convert.ToString(tipo), Convert.ToString(modalidade));
            }

            return null;
        }

        public object ListarGrupo3(object unidade_ens, object ano, object periodo, object curso, object serie, object turno, object itinerario, object tipo, object modalidade)
        {
            RN.Turma rnTurma = new Turma();

            var anofiltro = Convert.ToString(ano);
            var periodofiltro = Convert.ToString(periodo);
            var unidade = Convert.ToString(unidade_ens);
            var cursofiltro = Convert.ToString(curso);
            var seriefiltro = Convert.ToString(serie);
            var turnofiltro = Convert.ToString(turno);
            var itinerariofiltro = Convert.ToString(itinerario);

            if (!unidade.IsNullOrEmptyOrWhiteSpace() && !anofiltro.IsNullOrEmptyOrWhiteSpace() && !periodofiltro.IsNullOrEmptyOrWhiteSpace() && !cursofiltro.IsNullOrEmptyOrWhiteSpace() && !seriefiltro.IsNullOrEmptyOrWhiteSpace() && !turnofiltro.IsNullOrEmptyOrWhiteSpace())
            {
                return rnTurma.ListaTurmaEletivaAbertaComVagaPor(unidade_ens.ToString(), 3, Convert.ToInt32(ano), Convert.ToInt32(periodo), curso.ToString(), Convert.ToInt32(serie), turno.ToString(), itinerariofiltro, Convert.ToString(tipo), Convert.ToString(modalidade));
            }

            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {
                    tseAluno.Enabled = false;
                    LimpaCampos();
                    CarregaAno();

                    RetiraVisibilidadeBotao();
                    ControlaRadios();
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAluno, string.Empty);
        }

        private void LimpaCampos()
        {
            hdnCurso.Value = string.Empty;
            hdnSerie.Value = string.Empty;
            hdnUnidade.Value = string.Empty;
            hdnTurno.Value = string.Empty;
            tseAluno.ResetValue();
            tseRegional.ResetValue();
            tseUnidadeEnsino.ResetValue();
            tseCurso.ResetValue();
            ddlAno.Items.Clear();
            ddlPeriodo.Items.Clear();
            ddlSerie.Items.Clear();
            ddlTurma.Items.Clear();
            ddlTurno.Items.Clear();
            hdnItinerario.Value = string.Empty;
            hdnModalidade.Value = string.Empty;
            hdnTipo.Value = string.Empty;
        }

        private void CarregaAno()
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();

            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = rnPeriodoLetivo.ListaAnoEletiva();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        private void CarregaTurno(string censo, string curso, int ano, int periodo)
        {
            RN.Turno rnTurno = new Turno();

            ddlTurno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlTurno.DataSource = rnTurno.ListaTurnosAtivosPor(censo, ano, periodo, curso);
            ddlTurno.DataBind();
            ddlTurno.Items.Insert(0, item);
        }

        private void CarregaSerie(string censo, string curso, int ano, int periodo, string turno)
        {
            RN.Serie rnSerie = new Serie();

            ddlSerie.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlSerie.DataSource = rnSerie.ListaSerieAtivaPor(censo, ano, periodo, curso, turno);
            ddlSerie.DataBind();
            ddlSerie.Items.Insert(0, item);
        }


        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcesso(btnSalvar, AcaoControle.editar);

        }

        private void RetiraVisibilidadeBotao()
        {
            btnSalvar.Visible = false;

        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPeriodo.Items.Clear();
                dvTurmas.Visible = false;
                hdnUnidade.Value = string.Empty;
                hdnCurso.Value = string.Empty;
                hdnSerie.Value = string.Empty;
                hdnTurno.Value = string.Empty;
                hdnItinerario.Value = string.Empty;
                hdnModalidade.Value = string.Empty;
                hdnTipo.Value = string.Empty;
                tseAluno.ResetValue();
                tseRegional.ResetValue();
                tseUnidadeEnsino.ResetValue();
                tseCurso.ResetValue();
                ddlSerie.Items.Clear();
                ddlTurma.Items.Clear();
                ddlTurno.Items.Clear();

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {

                    ListItem item = new ListItem("Selecione", string.Empty);
                    ddlPeriodo.Items.Clear();
                    ddlPeriodo.DataSource = RN.PeriodoLetivo.ListarPeriodo(this.ddlAno.SelectedValue);
                    ddlPeriodo.DataBind();
                    ddlPeriodo.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            updatePanel3.Update();
        }

        protected void rbPrimeiraOpcao_CheckedChanged(object sender, EventArgs e)
        {
            hdnUnidade.Value = string.Empty;
            hdnCurso.Value = string.Empty;
            hdnSerie.Value = string.Empty;
            hdnTurno.Value = string.Empty;
            hdnItinerario.Value = string.Empty;
            hdnModalidade.Value = string.Empty;
            hdnTipo.Value = string.Empty;
            ControlaRadios();

            updatePanel3.Update();

        }

        protected void rbSegundaOpcao_CheckedChanged(object sender, EventArgs e)
        {
            hdnUnidade.Value = string.Empty;
            hdnCurso.Value = string.Empty;
            hdnSerie.Value = string.Empty;
            hdnTurno.Value = string.Empty;
            ControlaRadios();

            updatePanel3.Update();

        }

        protected void tseUnidadeResponsavel_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                dvTurmas.Visible = false;
                this.lblMensagem.Text = string.Empty;

                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);

                tseCurso.ResetValue();
                ddlTurno.Items.Clear();
                ddlSerie.Items.Clear();

                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeEnsino.DBValue.IsNull)
                {
                    if (this.tseUnidadeEnsino.IsValidDBValue)
                    {
                        if (!this.tseUnidadeEnsino["unidade_ens"].IsNull)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeEnsino.DBValue);
                            this.tseRegional.Value = this.tseUnidadeEnsino["id_regional"];
                        }

                    }
                    else
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Coordenadoria = string.Empty;
                        }


                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";

                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Coordenadoria = string.Empty;
                    }


                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            updatePanel3.Update();
        }

        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {

            try
            {
                hdnUnidade.Value = string.Empty;
                hdnCurso.Value = string.Empty;
                hdnSerie.Value = string.Empty;
                hdnTurno.Value = string.Empty;
                hdnItinerario.Value = string.Empty;
                hdnModalidade.Value = string.Empty;
                hdnTipo.Value = string.Empty;

                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;
                dvTurmas.Visible = false;
                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);

                if (!this.tseCurso.DBValue.IsNull)
                {
                    if (!this.tseCurso.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Curso não cadastrado.";
                    }
                    else
                    {
                        if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        {
                            CarregaTurno(tseUnidadeEnsino.DBValue.ToString(), tseCurso.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue));
                        }
                        else
                        {
                            lblMensagem.Text = "Preencha Ano e Período.";
                        }
                    }
                }
                else
                {

                    this.lblMensagem.Text = "Favor consultar um Curso.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            updatePanel3.Update();
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dvTurmas.Visible = false;
                hdnUnidade.Value = string.Empty;
                hdnCurso.Value = string.Empty;
                hdnSerie.Value = string.Empty;
                hdnTurno.Value = string.Empty;
                hdnItinerario.Value = string.Empty;
                hdnModalidade.Value = string.Empty;
                hdnTipo.Value = string.Empty;

                ddlSerie.Items.Clear();

                if (!ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue && !tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        CarregaSerie(tseUnidadeEnsino.DBValue.ToString(), tseCurso.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlTurno.SelectedValue);
                    }
                    else
                    {
                        lblMensagem.Text = "Preencha os campos obrigatórios.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            updatePanel3.Update();
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dvTurmas.Visible = false;

                ddlTurma.Items.Clear();
                hdnUnidade.Value = string.Empty;
                hdnCurso.Value = string.Empty;
                hdnSerie.Value = string.Empty;
                hdnTurno.Value = string.Empty;
                hdnItinerario.Value = string.Empty;
                hdnModalidade.Value = string.Empty;
                hdnTipo.Value = string.Empty;

                if (!string.IsNullOrEmpty(ddlSerie.SelectedValue))
                {
                    if (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue && !tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        ListItem item = new ListItem("Selecione", string.Empty);
                        ddlTurma.Items.Clear();
                        ddlTurma.DataSource = RN.Turma.ListarPorTurmaUE(tseUnidadeEnsino.DBValue.ToString(), tseCurso.DBValue.ToString(), Convert.ToInt32(ddlSerie.SelectedValue), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlTurno.SelectedValue);
                        ddlTurma.DataBind();
                        ddlTurma.Items.Insert(0, item);
                    }
                    else
                    {
                        lblMensagem.Text = "Preencha os campos obrigatórios.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            updatePanel3.Update();
        }

        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dvTurmas.Visible = false;
                hdnUnidade.Value = string.Empty;
                hdnCurso.Value = string.Empty;
                hdnSerie.Value = string.Empty;
                hdnTurno.Value = string.Empty;
                hdnItinerario.Value = string.Empty;
                hdnModalidade.Value = string.Empty;
                hdnTipo.Value = string.Empty;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            updatePanel3.Update();
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
                ValidacaoDados validacao = new ValidacaoDados();
                RN.DTOs.DadosEnturmacaoEletiva dados = new Techne.Lyceum.RN.DTOs.DadosEnturmacaoEletiva();
                List<RN.DTOs.DadosEnturmacaoEletiva> lista = new List<Techne.Lyceum.RN.DTOs.DadosEnturmacaoEletiva>();
                bool enturmar = false;

                for (var rowIndex = 0; rowIndex < this.grdAluno.VisibleRowCount; rowIndex++)
                {
                    dados = new Techne.Lyceum.RN.DTOs.DadosEnturmacaoEletiva();

                    var aluno = this.grdAluno.GetRowValues(rowIndex, "ALUNO").ToString();
                    var nome = this.grdAluno.GetRowValues(rowIndex, "NOME_COMPL").ToString();
                    var ddlGrupo1 = DevExpressHelper.GetControl<DropDownList>(this.grdAluno, rowIndex, "TURMAELETIVA1", "ddlGrupo1");
                    var ddlGrupo2 = DevExpressHelper.GetControl<DropDownList>(this.grdAluno, rowIndex, "TURMAELETIVA2", "ddlGrupo2");
                    var ddlGrupo3 = DevExpressHelper.GetControl<DropDownList>(this.grdAluno, rowIndex, "TURMAELETIVA3", "ddlGrupo3");
                    var hdnTurma1 = DevExpressHelper.GetControl<HiddenField>(this.grdAluno, rowIndex, "TURMAELETIVA1", "hdnTurma1");
                    var hdnTurma2 = DevExpressHelper.GetControl<HiddenField>(this.grdAluno, rowIndex, "TURMAELETIVA2", "hdnTurma2");
                    var hdnTurma3 = DevExpressHelper.GetControl<HiddenField>(this.grdAluno, rowIndex, "TURMAELETIVA3", "hdnTurma3");

                    dados.Aluno = aluno;
                    dados.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                    dados.Periodo = !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlPeriodo.SelectedValue) : -1;
                    dados.Nome = nome;
                    dados.TurmaReferencia = !ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurma.SelectedValue : null;
                    dados.UsuarioResponsavel = User.Identity.Name;

                    if (ddlGrupo1.Visible)
                    {
                        dados.TurmaEletiva1 = !ddlGrupo1.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlGrupo1.SelectedValue : null;
                    }
                    else
                    {
                        dados.TurmaEletiva1 = !hdnTurma1.Value.IsNullOrEmptyOrWhiteSpace() ? hdnTurma1.Value : null;
                    }

                    if (ddlGrupo2.Visible)
                    {
                        dados.TurmaEletiva2 = !ddlGrupo2.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlGrupo2.SelectedValue : null;
                    }
                    else
                    {
                        dados.TurmaEletiva2 = !hdnTurma2.Value.IsNullOrEmptyOrWhiteSpace() ? hdnTurma2.Value : null;
                    }

                    if (ddlGrupo3.Visible)
                    {
                        dados.TurmaEletiva3 = !ddlGrupo3.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlGrupo3.SelectedValue : null;                        
                    }
                    else
                    {
                        dados.TurmaEletiva3 = !hdnTurma3.Value.IsNullOrEmptyOrWhiteSpace() ? hdnTurma3.Value : null;
                    }

                    if (!ddlGrupo1.SelectedValue.IsNullOrEmptyOrWhiteSpace() || !ddlGrupo2.SelectedValue.IsNullOrEmptyOrWhiteSpace() || !ddlGrupo3.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        enturmar = true;
                    }

                    lista.Add(dados);
                }

                validacao = rnMatricula.ValidaMatriculaEletiva(lista, ddlTurma.SelectedValue, Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), rbPrimeiraOpcao.Checked, enturmar);

                if (validacao.Valido)
                {
                    rnMatricula.SalvaMatriculaEletiva(lista);        
                    lblMensagem.Text = "Enturmação realizada com sucesso.";
                    odsAluno.Select();
                    odsAluno.DataBind();
                    grdAluno.DataBind();

                    //RetiraVisibilidadeBotao();

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
            updatePanel3.Update();
        }

        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton[] controles = new ImageButton[] { };
                RN.Curso rnCurso = new Curso();

                dvTurmas.Visible = false;
                hdnCurso.Value = string.Empty;
                hdnSerie.Value = string.Empty;
                hdnUnidade.Value = string.Empty;
                hdnTurno.Value = string.Empty;
                hdnItinerario.Value = string.Empty;
                hdnModalidade.Value = string.Empty;
                hdnTipo.Value = string.Empty;

                if ((rbPrimeiraOpcao.Checked && !tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue) || (rbSegundaOpcao.Checked && !tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue && !tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue && !ddlSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace()))
                {
                    dvTurmas.Visible = true;

                    if (rbPrimeiraOpcao.Checked)
                    {
                        hdnUnidade.Value = tseAluno["unidade_ensino"].ToString();
                        hdnSerie.Value = tseAluno["serie"].ToString();
                        hdnCurso.Value = tseAluno["curso"].ToString();
                        hdnTurno.Value = tseAluno["turno"].ToString();
                        hdnItinerario.Value = tseAluno["itinerario"].ToString();
                        hdnTipo.Value = tseAluno["tipo"].ToString();
                        hdnModalidade.Value = tseAluno["modalidade"].ToString();

                        odsGrupo1.Select();
                        odsGrupo1.DataBind();

                        odsGrupo2.Select();
                        odsGrupo2.DataBind();

                        odsGrupo3.Select();
                        odsGrupo3.DataBind();
                        grdAluno.DataBind();


                    }
                    else
                    {
                        hdnUnidade.Value = !this.tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue ? tseUnidadeEnsino.DBValue.ToString() : string.Empty;
                        hdnCurso.Value = !this.tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue ? tseCurso.DBValue.ToString() : string.Empty;
                        hdnSerie.Value = !ddlSerie.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlSerie.SelectedValue : string.Empty;
                        hdnTurno.Value = !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurno.SelectedValue : string.Empty;
                        hdnItinerario.Value = !this.tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue ? tseCurso["itinerarioformativo"].ToString() : string.Empty;
                        hdnTipo.Value = !this.tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue ? tseCurso["tipo"].ToString() : string.Empty;
                        hdnModalidade.Value = !this.tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue ? tseCurso["codmodalidade"].ToString() : string.Empty;

                    }

                    controles = new ImageButton[] { btnSalvar };

                    odsAluno.Select();
                    odsAluno.DataBind();
                    grdAluno.DataBind();
                }
                else
                {
                    lblMensagem.Text = "Preencha os campos obrigatórios.";
                }

                ControlarVisibilidadeControle(controles);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            updatePanel3.Update();
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                lblMensagem.Text = string.Empty;
                dvTurmas.Visible = false;
                RetiraVisibilidadeBotao();
                hdnCurso.Value = string.Empty;
                hdnSerie.Value = string.Empty;
                hdnUnidade.Value = string.Empty;
                hdnTurno.Value = string.Empty;
                hdnItinerario.Value = string.Empty;
                hdnModalidade.Value = string.Empty;
                hdnTipo.Value = string.Empty;

                if (!this.tseAluno.DBValue.IsNull)
                {
                    if (!this.tseAluno.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Aluno não cadastrado.";
                    }
                    else
                    {

                        if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        {
                            //hdnUnidade.Value = tseAluno["unidade_ensino"].ToString();
                            //hdnSerie.Value = tseAluno["serie"].ToString();
                            //hdnCurso.Value = tseAluno["curso"].ToString();
                            //hdnTurno.Value = tseAluno["turno"].ToString();

                            ////CarregaAlunoBusca();
                            //odsGrupo1.DataBind();
                            //odsGrupo2.DataBind();
                            //odsGrupo3.DataBind();
                            //odsAluno.DataBind();
                            //dvTurmas.Visible = true;

                            //ImageButton[] controles = new ImageButton[] { btnSalvar };
                            //ControlarVisibilidadeControle(controles);
                        }
                        else
                        {
                            lblMensagem.Text = "O campo ANO e PERÍODO são obrigatórios.";
                            tseAluno.ResetValue();
                        }

                    }
                }
                else
                {

                    this.lblMensagem.Text = "Favor consultar um Aluno.";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            updatePanel3.Update();

        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;
                lblMensagem.Text = string.Empty;
                dvTurmas.Visible = false;

                if (!tseRegional.IsValidDBValue)
                {
                    lblMensagem.Text = "Regional não cadastrada.";
                    return;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        private void ControlaRadios()
        {
            ImageButton[] controles = new ImageButton[] { };
            ControlarVisibilidadeControle(controles);

            if (rbPrimeiraOpcao.Checked)
            {
                tseRegional.ResetValue();
                tseRegional.Mode = ControlMode.View;
                tseCurso.ResetValue();
                tseCurso.Mode = ControlMode.View;
                ddlTurno.Enabled = false;
                ddlSerie.Enabled = false;
                ddlTurma.Enabled = false;
                ddlTurma.Items.Clear();
                ddlTurno.Items.Clear();
                ddlSerie.Items.Clear();
                tseUnidadeEnsino.ResetValue();
                tseUnidadeEnsino.Mode = ControlMode.View;
                // btnPesquisar.Visible = false;

                tseAluno.Enabled = true;
            }
            else
            {
                tseAluno.ResetValue();
                tseAluno.Enabled = false;

                tseUnidadeEnsino.Mode = ControlMode.Edit;
                ddlTurno.Enabled = true;
                ddlSerie.Enabled = true;
                ddlTurma.Enabled = true;
                tseCurso.Mode = ControlMode.Edit;
                tseRegional.Mode = ControlMode.Edit;
                btnPesquisar.Visible = true;
            }
            dvTurmas.Visible = false;
            updatePanel3.Update();
        }

        private void CarregaAlunoBusca(params string[] origem)
        {
            if (rbPrimeiraOpcao.Checked)
            {
                if (tseAluno.IsValidDBValue && !tseAluno.DBValue.IsNull)
                {
                    string aluno = tseAluno.DBValue.ToString();
                    dvTurmas.Visible = true;
                }
            }
        }

        protected void grdAluno_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (!this.grdAluno.Visible || this.grdAluno.VisibleRowCount == 0 || e.RowType != GridViewRowType.Data)
            {
                return;
            }

            var hdnTurma1 = DevExpressHelper.GetControl<HiddenField>(this.grdAluno, e.VisibleIndex, "TURMAELETIVA1", "hdnTurma1");
            var hdnTurma2 = DevExpressHelper.GetControl<HiddenField>(this.grdAluno, e.VisibleIndex, "TURMAELETIVA2", "hdnTurma2");
            var hdnTurma3 = DevExpressHelper.GetControl<HiddenField>(this.grdAluno, e.VisibleIndex, "TURMAELETIVA3", "hdnTurma3");
            var txtTurmaDisciplina1 = DevExpressHelper.GetControl<TextBox>(this.grdAluno, e.VisibleIndex, "TURMAELETIVA1", "txtTurmaDisciplina1");
            var txtTurmaDisciplina2 = DevExpressHelper.GetControl<TextBox>(this.grdAluno, e.VisibleIndex, "TURMAELETIVA2", "txtTurmaDisciplina2");
            var txtTurmaDisciplina3 = DevExpressHelper.GetControl<TextBox>(this.grdAluno, e.VisibleIndex, "TURMAELETIVA3", "txtTurmaDisciplina3");
            var ddlGrupo1 = DevExpressHelper.GetControl<DropDownList>(this.grdAluno, e.VisibleIndex, "TURMAELETIVA1", "ddlGrupo1");
            var ddlGrupo2 = DevExpressHelper.GetControl<DropDownList>(this.grdAluno, e.VisibleIndex, "TURMAELETIVA2", "ddlGrupo2");
            var ddlGrupo3 = DevExpressHelper.GetControl<DropDownList>(this.grdAluno, e.VisibleIndex, "TURMAELETIVA3", "ddlGrupo3");

            if (ddlGrupo1 == null
                || ddlGrupo2 == null
                || ddlGrupo3 == null
                )
            {
                return;
            }

            ddlGrupo1.ClearSelection();
            var turma1 = string.IsNullOrEmpty(hdnTurma1.Value) ? string.Empty : hdnTurma1.Value;
            if (turma1.IsNullOrEmptyOrWhiteSpace())
            {
                ddlGrupo1.Visible = true;
                txtTurmaDisciplina1.Visible = false;
            }
            else
            {
                ddlGrupo1.Visible = false;
                txtTurmaDisciplina1.Visible = true;
            }

            ddlGrupo2.ClearSelection();
            var turma2 = string.IsNullOrEmpty(hdnTurma2.Value) ? string.Empty : hdnTurma2.Value;
            if (turma2.IsNullOrEmptyOrWhiteSpace())
            {
                ddlGrupo2.Visible = true;
                txtTurmaDisciplina2.Visible = false;
            }
            else
            {
                ddlGrupo2.Visible = false;
                txtTurmaDisciplina2.Visible = true;
            }

            ddlGrupo3.ClearSelection();
            var turma3 = string.IsNullOrEmpty(hdnTurma3.Value) ? string.Empty : hdnTurma3.Value;
            if (turma3.IsNullOrEmptyOrWhiteSpace())
            {
                ddlGrupo3.Visible = true;
                txtTurmaDisciplina3.Visible = false;
            }
            else
            {
                ddlGrupo3.Visible = false;
                txtTurmaDisciplina3.Visible = true;
            }

            //Verifica se tem permissão
            if (!Permission.AllowUpdate)
            {
                ddlGrupo1.Enabled = false;
                ddlGrupo2.Enabled = false;
                ddlGrupo3.Enabled = false;
            }
        }
    }
}
