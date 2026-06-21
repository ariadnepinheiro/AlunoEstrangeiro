using Techne.Data;

namespace Techne.Lyceum.Net.Academico
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DevExpress.Web.ASPxEditors;
    using DevExpress.Web.ASPxGridView;
    using Seeduc.Infra.Helpers;
    using Techne.Lyceum.Net.Modulos;
    using Techne.Lyceum.RN;
    using Techne.Web;
    using Image = System.Drawing.Image;
    using System.Data;
    using Techne.Controls;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.CR;
    using System.Collections;
    using System.Web.UI.HtmlControls;
    using System.Configuration;
    using Techne.Lyceum.RN.Util;
    using Techne.Lyceum.RN.DTOs;

    [NavUrl("~/Academico/FechamentoMatricula.aspx"), ControlText("Fechamento Matrícula"), Title("Fechamento do Ano Letivo e Matrícula")]
    public partial class FechamentoMatricula : TPage
    {
        #region Propriedades

        private const string NaoCadastrado = "não cadastrado";
        private string strEscolaridadeOrigem = string.Empty;
        private string strNomeEscolaridadeOrigem = string.Empty;
        private string strUnidadeEnsinoOrigem = string.Empty;
        private string strNomeUnidadeEnsinoOrigem = string.Empty;
        private string strTurmaOrigem = string.Empty;
        private string strAnoOrigem = string.Empty;
        private string strPeriodoOrigem = string.Empty;
        private string strTurnoOrigem = string.Empty;
        private string strDescricaoTurnoOrigem = string.Empty;
        private string strCurriculoOrigem = string.Empty;
        private string strNomeSerieOrigem = string.Empty;
        private string strSerieOrigem = string.Empty;
        private string strDescricaoSerieOrigem = string.Empty;

        #endregion

        private Turma.DadosTurma ObjetoTurma
        {
            get
            {
                return (Turma.DadosTurma)this.ViewState["ObjetoTurma"];
            }

            set
            {
                this.ViewState["ObjetoTurma"] = value;
            }
        }

        public static string GetUrl()
        {
            return Navigation.GetNavigation(MethodBase.GetCurrentMethod()).GetUrl(new object[]
                                                                                  {
                                                                                  });
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();

            base.OnInit(e);
        }

        public static QueryTable ListarAlunosEletivas(string ano, string semestre, string turma)
        {
            return RN.FechamentoMatricula.ListarAlunosEletivas(ano, semestre, turma);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdMatriculas, "Matrículas");
            TituloGrid(this.grdEletivas, "Disciplinas Eletivas");
            TituloGrid(this.grdHistorico, "Histórico");
            TituloGrid(this.grdDependencia, "Disciplinas");
            TituloGrid(this.grdProgressaoParcial, "Matrículas");

            var mp = (LyceumMaster)this.Master;

            if (mp == null)
            {
                return;
            }

            mp.habilitaLoading = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                btnExecutar.Attributes.Add("onclick", "document.body.style.cursor = 'wait'; this.value='Aguarde, Enviando...'; this.disabled = true; pucConfirmar.Hide(); " + Page.ClientScript.GetPostBackEventReference(btnExecutar, string.Empty) + ";");
                lblMensagem.Text = string.Empty;

                // Verifica se não é post back da página
                if (!this.IsPostBack)
                {
                    // Para a primeira vez que a página é carregada o tipo de operação será inicial
                    if (this.Request.QueryString.Keys.Count > 0)
                    {
                        LimparCampos();
                        this.CarregarDadosTurma();
                    }
                    else
                    {
                        this.Response.Redirect("FechamentoMatriculaLista.aspx");
                    }
                }

                if (ddlUnidadeEnsino.Items.Count <= 1)
                {
                    ddlUnidadeEnsino.Enabled = false;
                }

                this.tseCurso.SqlWhere = "uec.unidade_ens = '" + this.ddlUnidadeEnsino.SelectedValue.ToString() + "' and c.curso not in ('9999.91','9999.92','9999.01','9999.02','9999.03','9999.04') ";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                if (txtAno.Text != "2020" && txtAno.Text != "2021")
                {
                    rbRetido.Checked = false;
                    rbPromovido.Checked = false;
                    rbRetido.Visible = false;
                    rbPromovido.Visible = false;
                }

                if (txtAno.Text == "2023" && txtPeriodo.Text == "1")
                {
                    rbPromovido.Visible = true;
                }

                if (!this.IsPostBack)
                {
                    int ano, periodo;
                    int proximoAno;
                    RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();

                    if (int.TryParse(this.txtAnoTop.Text, out ano)
                        && int.TryParse(this.txtPeriodoTop.Text, out periodo))
                    {
                        proximoAno = rnPeriodoLetivo.ObtemProximoAnoPor(ano, periodo);

                        if (proximoAno > 0)
                        {
                            ddlPeriodo.Items.Clear();
                            this.ddlAno.SelectedValue = proximoAno.ToString();

                            this.ddlPeriodo.DataSource = rnPeriodoLetivo.ListaPossiveisPeriodosPor(periodo, Convert.ToInt32(txtAnoTop.Text));

                            this.ddlPeriodo.DataBind();
                            this.ddlPeriodo.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                            this.ddlAno.Enabled = false;

                        }
                    }
                    if (this.ddlTurno.Items.FindByValue(this.txtTurnoVal.Text) != null && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                    {
                        this.ddlTurno.SelectedValue = this.txtTurnoVal.Text;
                        this.ddlSerie.Enabled = true;
                        this.ddlSerie.DataSource = Serie.ListarSerie(this.ddlUnidadeEnsino.SelectedValue.ToString(), Convert.ToDecimal(this.ddlAno.SelectedValue), Convert.ToDecimal(this.ddlPeriodo.SelectedValue), this.ddlTurno.SelectedValue, this.txtEscolaridadeVal.Text);
                        this.ddlSerie.DataBind();
                        this.ddlSerie.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                    }


                    ControlaVisibilidadeProgressao();
                    ControlaVisibilidadeHistorico();
                    ControlaVisibilidadeEletiva();

                    rbAprovadoEletiva.Checked = true;

                    if (grdProgressaoParcial.VisibleRowCount == 0 && grdMatriculas.VisibleRowCount == 0 && txtSituacaoTurma.Text == "Aberta")
                    {
                        VerificaTurmaAberta();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void VerificaTurmaAberta()
        {
            try
            {
                RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
                RN.Turma rnTurma = new Turma();

                if (rnTurma.PossuiTurmaAbertaSemAlunoMatriculado(txtTurmaTop.Text, Convert.ToDecimal(txtAnoTop.Text), Convert.ToDecimal(txtPeriodo.Text)))
                {
                    if (!rnMatricula.PossuiMatriculaAtivaNaTurma(txtTurmaTop.Text, Convert.ToDecimal(txtAnoTop.Text), Convert.ToDecimal(txtPeriodo.Text)))
                    {
                        //Caso não exista mais alunos, muda a situação da turma para finalizada.
                        rnTurma.FinalizaTurma(txtTurmaTop.Text, Convert.ToDecimal(txtAnoTop.Text), Convert.ToDecimal(txtPeriodo.Text));
                        txtSituacaoTurma.Text = "Finalizada";
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
            hdnQtdeDep.Value = string.Empty;
            hdnSerieConcluinte.Value = string.Empty;
            txtUnidadeEns.Text = string.Empty;
            txtTurma.Text = string.Empty;
            txtAno.Text = string.Empty;
            txtPeriodo.Text = string.Empty;
            txtEscolaridade.Text = string.Empty;
            txtTurma.Text = string.Empty;
            txtTurno.Text = string.Empty;
            txtSerie.Text = string.Empty;
            txtOptativaReforco.Text = string.Empty;

            txtUnidadeEnsVal.Text = string.Empty;
            txtTurmaTop.Text = string.Empty;
            txtAnoTop.Text = string.Empty;
            txtPeriodoTop.Text = string.Empty;
            txtEscolaridadeVal.Text = string.Empty;
            txtTurnoVal.Text = string.Empty;
            txtCurriculoTop.Text = string.Empty;
            txtSerieVal.Text = string.Empty;

        }

        private void ControlaVisibilidadeProgressao()
        {
            if (grdProgressaoParcial.VisibleRowCount > 0)
            {
                pnlProgressaoParcial.Visible = true;
            }
            else
            {
                pnlProgressaoParcial.Visible = false;
            }
        }

        private void ControlaVisibilidadeHistorico()
        {
            if (grdHistorico.VisibleRowCount > 0)
            {
                pnlGridHistorico.Visible = true;
            }
            else
            {
                pnlGridHistorico.Visible = false;
            }
        }

        private void ControlaVisibilidadeEletiva()
        {
            if (grdEletivas.VisibleRowCount > 0)
            {
                pnlEletivas.Visible = true;
            }
            else
            {
                pnlEletivas.Visible = false;
            }
        }

        protected void btCancelar_Click(object sender, EventArgs e)
        {
            this.pucConfirmar.ShowOnPageLoad = false;
        }

        protected void btConfirma_Click(object sender, EventArgs e)
        {
            try
            {
                this.pucConfirmar.ShowOnPageLoad = true;

                var alunos = 0;

                for (var i = 0; i < this.grdMatriculas.VisibleRowCount; i++)
                {
                    var optSel = this.grdMatriculas.FindRowCellTemplateControl(i, (GridViewDataColumn)this.grdMatriculas.Columns[0], "chkBox") as ASPxCheckBox;

                    if (optSel != null
                        && optSel.Checked)
                    {
                        alunos++;
                    }
                }

                var situacao = string.Empty;

                if (this.rbAprovar.Checked)
                {
                    situacao = "aprovando";
                }
                else if (this.rbReprovadoFalta.Checked)
                {
                    situacao = "reprovando por falta";
                }
                else if (this.rbReprovadoNota.Checked)
                {
                    situacao = "reprovando por nota";
                }

                this.lblConfirmar.Text = string.Format(
                    "Este processo finalizará o fechamento de <b>{0}</b> alunos da turma <b>{1}</b>, {2} para a turma <b>{3}<\b> da unidade {4}.",
                    alunos,
                    this.txtTurma.Text,
                    situacao,
                    this.ddlTurma.SelectedValue,
                    this.txtUnidadeEns.Text);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelarDependencia_Click(object sender, EventArgs e)
        {
            this.pcDependencia.ShowOnPageLoad = false;

            this.lblMensagem.Text = string.Empty;
        }

        protected void ddlUnidadeEnsino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.tseCurso.ResetValue();
                ddlTurma.Items.Clear();
                ddlSerie.Items.Clear();

                RN.GradeSerie rnGradeSerie = new GradeSerie();

                if (!string.IsNullOrEmpty(this.ObjetoTurma.Ano) && !string.IsNullOrEmpty(this.ObjetoTurma.Periodo))
                {

                    var qt = rnGradeSerie.ObtemConsultaTurmasParaFechamentoMatriculaPor(
                        this.ObjetoTurma.Ano,
                        this.ObjetoTurma.Periodo,
                        this.ObjetoTurma.Curso,
                        this.ObjetoTurma.Turno,
                        this.ObjetoTurma.UnidadeResponsavel,
                        this.ObjetoTurma.Grade);

                    var sr = qt.Rows[0];

                    if (!Convert.ToString(sr["unidade_responsavel"]).Trim().Equals(ddlUnidadeEnsino.SelectedItem.Value))
                    {
                        ddlTurma.Items.Clear();
                        ddlTurno.Enabled = false;
                    }
                    else
                    {
                        if (!tseCurso.DBValue.IsNull)
                        {
                            this.ddlTurma.Enabled = true;

                            this.ddlTurma.DataSource = Turma.ConsultarPrimeiraTurmaDisponivel(
                                Convert.ToDecimal(this.ddlAno.SelectedValue),
                                Convert.ToDecimal(this.ddlPeriodo.SelectedValue),
                                ddlUnidadeEnsino.SelectedValue.ToString(),
                                this.ddlTurno.SelectedValue,
                                this.tseCurso.DBValue.ToString(),
                                Convert.ToDecimal(this.ddlSerie.SelectedValue));

                            this.ddlTurma.DataBind();
                            this.ddlTurma.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnDependencia_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Dependencia rnDependencia = new Dependencia();
                var disciplinas = this.ObterDisciplinas();
                var matriculaNova = this.rdSim.Checked;
                var alunos = this.ObterAlunos();
                PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
                int[] anoPeriodo;
                decimal periodoProximo = -1;
                decimal anoProximo = -1;

                RN.FechamentoMatricula rnFechamentoMatricula = new Techne.Lyceum.RN.FechamentoMatricula();

                this.pcDependencia.ShowOnPageLoad = false;

                if (!this.DadosValidos(alunos))
                {
                    return;
                }

                if (disciplinas.Count == 0
                    || disciplinas.Count > Convert.ToInt32(hdnQtdeDep.Value))
                {
                    this.lblMensagem.Text = "Selecione uma ou duas disciplinas para dependência.";

                    return;
                }

                validacao = rnFechamentoMatricula.ValidaDependencia(alunos, disciplinas.Count, Convert.ToInt32(hdnQtdeDep.Value));

                if (validacao.Valido)
                {
                    if (this.rbAprovarComDep.Checked)
                    {
                        if (matriculaNova)
                        {
                            var curriculo = Turma.RetornaCurriculo(

                                Convert.ToDecimal(this.ddlAno.SelectedValue),
                                Convert.ToDecimal(this.ddlPeriodo.SelectedValue),
                                this.ddlTurno.SelectedValue,
                                this.tseCurso.DBValue.ToString(),
                                this.txtUnidadeEnsVal.Text,
                                this.ddlTurma.SelectedValue);

                            if (string.IsNullOrEmpty(curriculo))
                            {
                                lblMensagem.Text = "Não existe Matriz Curricular cadastrada para este unidade de ensino/curso/turno/serie";
                                return;
                            }

                            rnFechamentoMatricula.AprovaAlunosComDependenciaComEnturmacao(
                                this.ObjetoTurma,
                                alunos,
                                this.ddlAno.SelectedValue,
                                this.ddlPeriodo.SelectedValue,
                                this.tseCurso.DBValue.ToString(),
                                this.ddlTurno.SelectedValue,
                                curriculo,
                                this.ddlSerie.SelectedValue,
                                this.ddlTurma.SelectedValue,
                                disciplinas,
                                User.Identity.Name,
                                ddlUnidadeEnsino.SelectedValue.ToString());

                        }
                        else
                        {
                            anoPeriodo = rnPeriodoLetivo.ObtemProximoAnoPeriodoPor(Convert.ToInt32(txtAnoTop.Text), Convert.ToInt32(txtPeriodoTop.Text));

                            if (anoPeriodo.Length > 0 && anoPeriodo[0] > 0)
                            {
                                anoProximo = Convert.ToDecimal(anoPeriodo[0]);
                                periodoProximo = Convert.ToDecimal(anoPeriodo[1]);
                            }

                            rnFechamentoMatricula.AprovaAlunosComDependencia(
                                this.ObjetoTurma,
                                alunos,
                                disciplinas,
                                User.Identity.Name, anoProximo, periodoProximo);
                        }
                    }

                    this.lblMensagem.Text = "Atualização concluída com sucesso.";

                    this.grdMatriculas.DataBind();
                    this.grdHistorico.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

                //Verificar se existe dados de historico para exibir o grid
                ControlaVisibilidadeHistorico();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnDesmarcar_Click(object sender, EventArgs e)
        {
            this.AlterarSelecaoEmMatriculas(false, grdMatriculas);
        }

        protected void btnExecutar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.FechamentoMatricula rnFechamentoMatricula = new Techne.Lyceum.RN.FechamentoMatricula();
                RN.AulaDocente rnAulaDocente = new AulaDocente();
                var matriculaNova = this.rdSim.Checked;
                var alunos = this.ObterAlunos();
                string alunosSelec = string.Empty;
                string listaAlunos = string.Empty;
                string listaAlunosConcomitante = string.Empty;
                string turmasAtivas = string.Empty;
                var mensagensTurmasAtivas = new List<string>();
                string msgAlunoTurma = string.Empty;
                string curriculo = string.Empty;
                DataTable dtRenovacoesDuplicadas = new DataTable();
                RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
                var mensagensRenovacaoDuplicada = new List<string>();
                PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
                int[] anoPeriodo;
                decimal periodoProximo = -1;
                decimal anoProximo = -1;
                bool periodoSimultaneo = Convert.ToBoolean(ConfigurationManager.AppSettings["PeriodoSimultaneo"] ?? "false");
                bool verificaAlocacaoPeriodoSimultaneo = Convert.ToBoolean(ConfigurationManager.AppSettings["VerificaAlocacaoPeriodoSimultaneo"] ?? "false");
                DataTable dtRenovacaoAtiva = null;
                DataTable dtAlunoRenovacaoAtiva = null;
                var mensagensRenovacaoAtiva = new List<string>();
                var mensagensAlunoRenovacaoAtiva = new List<string>();
                var mensagensAlunoRenovacaoNaoAtiva = new List<string>();
                int possuiRenovacaoAutomatica = 0;
                DataTable unidadesRenovacaoAutomatica = new DataTable();
                ICollection<DadosAlunoFechamento> listaConcomitante;
                List<string> alunosConc = new List<string>();
                RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
                RN.Curso rnCurso = new Curso();

                if (!this.DadosValidos(alunos))
                {
                    return;
                }

                if (alunos.Count == 0)
                {
                    this.lblMensagem.Text = "Para efetuar o fechamento é necessário selecionar pelo menos um(1) aluno.";
                    return;
                }

                //Carrega em cache a lista com escolas com renovação automativa
                unidadesRenovacaoAutomatica = (DataTable)RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.UnidadesEnsinoRenovacaoAutomatica, RN.RenovacaoMatricula.UnidadeEnsinoRenovacaoAutomatica.QueryListaUnidadesEnsinoRenovacaoAutomatica);

                //Verificar se aluno está sendo enturmado em uma escola com renovação automatica
                possuiRenovacaoAutomatica = unidadesRenovacaoAutomatica.Select("UNIDADEENSINOID = " + ddlUnidadeEnsino.SelectedValue).Count();

                foreach (var aluno in alunos)
                {
                    alunosSelec = alunosSelec + "'" + aluno.ToString() + "',";
                }

                listaAlunos = alunosSelec.Substring(0, alunosSelec.Length - 1);

                listaConcomitante = RN.FechamentoMatricula.ListarAlunosMatriculadosConcomitante(txtAno.Text, listaAlunos, this.ObjetoTurma.Grade, txtPeriodo.Text);


                //Não será realizado o fechamento das matriculas principais ate as outras sejam finalizadas:
                //Optativa reforço, concomitantes, mais educacao e foco, educacao especial e dependencia
                if (txtOptativaReforco.Text == "N"
                    && this.ObjetoTurma.Curso != "9999.92"
                    && this.ObjetoTurma.Curso != "9999.91"
                    && this.ObjetoTurma.Curso != "9999.01"
                    && this.ObjetoTurma.Curso != "9999.02"
                    && this.ObjetoTurma.Curso != "9999.03"
                    && this.ObjetoTurma.Curso != "9999.04"
                    && this.ObjetoTurma.Curso != "2025.02"
                    && this.ObjetoTurma.Curso != "2025.03"
                    && this.ObjetoTurma.Curso != "2025.04"
                    && this.ObjetoTurma.Curso != "2025.05"
                    && hdnEletiva.Value == "N")
                {
                    if (listaConcomitante.Count > 0)
                    {
                        alunosSelec = string.Empty;
                        listaAlunos = string.Empty;

                        foreach (var item in alunos)
                        {
                            var lista = listaConcomitante.Where(x => x.Aluno == item.ToString());

                            if (lista.Count() == 0)
                            {
                                alunosSelec = alunosSelec + "'" + item.ToString() + "',";
                            }
                        }
                        if (!string.IsNullOrEmpty(alunosSelec))
                        {
                            listaAlunos = alunosSelec.Substring(0, alunosSelec.Length - 1);

                        }
                    }

                    if (!string.IsNullOrEmpty(listaAlunos))
                    {

                        var listagemAlunos = RN.FechamentoMatricula.ListarAlunosMatriculadosCursosEspeciais(txtAno.Text, listaAlunos, this.ObjetoTurma.Grade);

                        foreach (var linha in listagemAlunos.Select(ao => ao.Aluno).Distinct())
                        {
                            turmasAtivas = string.Empty;
                            var listaTurmaAtivas = listagemAlunos.Where(x => x.Aluno == linha);

                            foreach (var item in listaTurmaAtivas)
                            {
                                turmasAtivas = turmasAtivas + item.Turma + ",";
                            }

                            mensagensTurmasAtivas.Add(listaTurmaAtivas.Select(ao => ao.NomeAluno).FirstOrDefault().ToString() + ": " + turmasAtivas.Substring(0, turmasAtivas.Length - 1));
                        }



                        if (mensagensTurmasAtivas.Count > 0)
                        {
                            mensagensTurmasAtivas.Add("Favor realizar primeiro o fechamento do(s) aluno(s) na(s) turma(s) listada(s) acima e, somente após esse procedimento, realizar fechamento da turma principal.");

                            msgAlunoTurma = mensagensTurmasAtivas.Aggregate((x, y) => x + Environment.NewLine + y);

                            lblMensagem.Text = msgAlunoTurma.Replace(Environment.NewLine, "<br />");

                            return;
                        }
                    }
                }


                if (listaConcomitante.Count > 0)
                {
                    foreach (var item in listaConcomitante)
                    {
                        alunosConc.Add(String.Format("{0} - {1} ", item.Aluno.ToString(), item.NomeAluno));
                    }

                    if (rdSim.Checked)
                    {
                        lblMensagem.Text = "Para aluno do Concomitante, não é permitido Efetuar Matrícula. Realize o fechamento em separado. Verifique o(s) aluno(s) abaixo: <br />" + alunosConc.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }

                if (listaConcomitante.Count > 0 && listaConcomitante.Count != alunos.Count)
                {
                    if (rdNao.Checked)
                    {
                        lblMensagem.Text = "Para aluno do Concomitante, realize o fechamento em separado. Verifique o(s) aluno(s) abaixo: <br />" + alunosConc.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }

                anoPeriodo = rnPeriodoLetivo.ObtemProximoAnoPeriodoPor(Convert.ToInt32(txtAnoTop.Text), Convert.ToInt32(txtPeriodoTop.Text));


                if (txtAno.Text == "2020" || txtAno.Text == "2021")
                {
                    if (!rbAprovar.Checked && !rbAprovarComDep.Checked && !rbReprovadoFalta.Checked && !rbReprovadoNota.Checked && !rbPromovido.Checked && !rbRetido.Checked)
                    {
                        this.lblMensagem.Text = "Selecione a situação final do(s) aluno(s) selecionado(s).";
                        return;
                    }

                    if (rbAprovarComDep.Checked || rbReprovadoNota.Checked || rbReprovadoFalta.Checked)
                    {
                        lblMensagem.Text = "EM " + txtAno.Text + ", EXCEPCIONALMENTE, NÃO PODERÁ SER INFORMADA A SITUAÇÃO";
                        return;
                    }

                    //CASO A SERIE NÃO SEJA CONCLUINTE, A OPÇAO APROVADO NÃO PODERÁ SER ESCOLHIDA
                    if (hdnSerieConcluinte.Value == "N" && rbAprovar.Checked && txtOptativaReforco.Text != "S")
                    {
                        lblMensagem.Text = "EM " + txtAno.Text + ", EXCEPCIONALMENTE, NÃO PODERÁ SER INFORMADA A SITUAÇÃO";
                        return;
                    }

                    //CASO A SERIE SEJA CONCLUINTE, A OPÇAO PROMOVIDO NÃO PODERÁ SER ESCOLHIDA
                    if (hdnSerieConcluinte.Value == "S" && rbPromovido.Checked)
                    {
                        lblMensagem.Text = "Esta situação é apenas para alunos das séries/fases/anos/módulos que não sejam terminalidades.";
                        return;
                    }

                }
                else
                {
                    if (!rbAprovar.Checked && !rbAprovarComDep.Checked && !rbReprovadoFalta.Checked && !rbReprovadoNota.Checked && !rbPromovido.Checked)
                    {
                        this.lblMensagem.Text = "Selecione a situação final do(s) aluno(s) selecionado(s).";
                        return;
                    }
                }

                if (periodoSimultaneo && verificaAlocacaoPeriodoSimultaneo)
                {
                    //Caso seja setado para finalizar periodos nao encerrados, valida se existe professor alocado na turma
                    if (rnAulaDocente.ExisteDocentesEmAulaAtivaPor(this.ObjetoTurma.Grade, Convert.ToDecimal(txtAno.Text), Convert.ToDecimal(txtPeriodo.Text)))
                    {
                        this.lblMensagem.Text = "Não é possível realizar o fechamento com docentes ainda alocados no quadro de horários da turma. Por favor, realize a desalocação dos docentes e tente novamente.";
                        return;
                    }
                }

                foreach (var aluno in alunos)
                {
                    alunosSelec = alunosSelec + "'" + aluno.ToString() + "',";
                }

                listaAlunos = alunosSelec.Substring(0, alunosSelec.Length - 1);

                if (rbRetido.Checked || rbReprovadoFalta.Checked || rbReprovadoNota.Checked)
                {
                    if (rdNao.Checked && listaConcomitante.Count != alunos.Count)
                    {
                        if (hdnSerieConcluinte.Value == "S")
                        {
                            if (!((((Convert.ToDecimal(txtAno.Text) == 2022 || Convert.ToDecimal(txtAno.Text) == 2023) && Convert.ToDecimal(txtPeriodo.Text) == 0 && ObjetoTurma.Curso == "0002.31" && (ObjetoTurma.Serie == "2" || ObjetoTurma.Serie == "3")) ||
                               ((Convert.ToDecimal(txtAno.Text) == 2022 || Convert.ToDecimal(txtAno.Text) == 2023) && Convert.ToDecimal(txtPeriodo.Text) == 2 && ObjetoTurma.Curso == "0002.83" && (ObjetoTurma.Serie == "3")) ||
                                (rnCurso.EhItinerarioFormativoTrihaComMatrizPor(ObjetoTurma.Curso, Convert.ToDecimal(txtAno.Text), Convert.ToDecimal(txtPeriodo.Text))
                                && ((Convert.ToDecimal(txtPeriodo.Text) == 0 && ObjetoTurma.Serie == "2") ||
                                    (Convert.ToDecimal(txtPeriodo.Text) == 2 && (ObjetoTurma.Serie == "3" || ObjetoTurma.Serie == "4")))
                              )
                                )
                               && (this.rbReprovadoFalta.Checked || this.rbReprovadoNota.Checked)))
                            {
                                lblMensagem.Text = "Para aluno " + (rbRetido.Checked ? "retido" : "reprovado") + " nas séries/anos/fases/módulos das terminalidades, selecione a opção Efetuar Matrícula e enturme na série de retenção.";
                                return;
                            }
                        }

                        //passa o periodo atual do aluno pra identificar os possiveis
                        dtRenovacaoAtiva = rnConfirmacaoMatricula.ObtemListaConfirmacaConfirmadaPor(Convert.ToInt32(anoPeriodo[0]), Convert.ToInt32(txtPeriodoTop.Text), txtUnidadeEnsVal.Text, listaAlunos);

                        if (dtRenovacaoAtiva.Rows.Count > 0)
                        {
                            //Para o ano 2022, 2ª serie do Médio ou EJA III
                            //Para o ano 2023, 3ª serie do Médio ou EJA IV ou 


                            if ((((Convert.ToDecimal(txtAno.Text) == 2022 || Convert.ToDecimal(txtAno.Text) == 2023) && Convert.ToDecimal(txtPeriodo.Text) == 0 && ObjetoTurma.Curso == "0002.31" && (ObjetoTurma.Serie == "2" || ObjetoTurma.Serie == "3")) ||
                                 ((Convert.ToDecimal(txtAno.Text) == 2022 || Convert.ToDecimal(txtAno.Text) == 2023) && Convert.ToDecimal(txtPeriodo.Text) == 2 && ObjetoTurma.Curso == "0002.83" && (ObjetoTurma.Serie == "3")) ||
                                  (rnCurso.EhItinerarioFormativoTrihaComMatrizPor(ObjetoTurma.Curso, Convert.ToDecimal(txtAno.Text), Convert.ToDecimal(txtPeriodo.Text))
                                  && ((Convert.ToDecimal(txtPeriodo.Text) == 0 && ObjetoTurma.Serie == "2") ||
                                      (Convert.ToDecimal(txtPeriodo.Text) == 2 && (ObjetoTurma.Serie == "3" || ObjetoTurma.Serie == "4")))
                                )
                                  )
                                 && (this.rbReprovadoFalta.Checked || this.rbReprovadoNota.Checked))
                            {
                                lblMensagem.Text = "Aluno(s) retido(s) na 2ª/3ª Série do Regular ou EJA III/IV ou  ficarão em pendência de enturmação para futura escolha do curso/série.";
                            }
                            else
                            {

                                foreach (DataRow row in dtRenovacaoAtiva.Rows)
                                {
                                    mensagensRenovacaoAtiva.Add(String.Format("{0} - {1} ", row["ALUNO"].ToString(), row["NOME_COMPL"].ToString()));
                                }

                                lblMensagem.Text = @"Para aluno " + (rbRetido.Checked ? "retido" : "reprovado") + " com confirmação ativa, selecione  a opção Efetuar Matrícula e enturme na série de retenção. Verifique o(s) aluno(s) abaixo: <br />" + mensagensRenovacaoAtiva.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");

                                return;
                            }

                        }
                    }
                }

                //passa o periodo atual do aluno pra identificar os possiveis
                dtAlunoRenovacaoAtiva = rnConfirmacaoMatricula.ObtemAlunoConfirmacaoPor(Convert.ToInt32(anoPeriodo[0]), Convert.ToInt32(txtPeriodoTop.Text), listaAlunos);

                if (dtAlunoRenovacaoAtiva.Rows.Count > 0)
                {
                    foreach (DataRow row in dtAlunoRenovacaoAtiva.Rows)
                    {

                        //SO SERÁ CONSIDERADA RENOVAÇÃO ATIVA CASO ESTEJA NA MESMA UNIDADE ATUAL DO ALUNO
                        if (row["RENOVACAO"].ToString() == "S" && row["UNIDADE_ENSINO"].ToString() == row["UNIDADEENSINOID"].ToString())
                        {
                            mensagensAlunoRenovacaoAtiva.Add(String.Format("{0} - {1} ", row["ALUNO"].ToString(), row["NOME_COMPL"].ToString())); ;
                        }
                        else
                        {
                            mensagensAlunoRenovacaoNaoAtiva.Add(String.Format("{0} - {1} ", row["ALUNO"].ToString(), row["NOME_COMPL"].ToString())); ;
                        }
                    }
                }

                if (rdSim.Checked && mensagensAlunoRenovacaoNaoAtiva.Count > 0 && hdnSerieConcluinte.Value != "S" && possuiRenovacaoAutomatica == 0 && hdnEletiva.Value == "N" && txtOptativaReforco.Text == "N")
                {
                    lblMensagem.Text = @"Para aluno sem confirmação ativa nesta escola, selecione a opção Não Efetuar Matrícula. Verifique o(s) aluno(s) abaixo: <br />" + mensagensAlunoRenovacaoNaoAtiva.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                    return;

                }

                if (rdNao.Checked
                    && mensagensAlunoRenovacaoAtiva.Count > 0
                    && listaConcomitante.Count != alunos.Count
                    && hdnEletiva.Value == "N"
                    && txtOptativaReforco.Text == "N"
                    && this.ObjetoTurma.Curso != "9999.01"
                    && this.ObjetoTurma.Curso != "9999.03"
                    && this.ObjetoTurma.Curso != "9999.02"
                    && this.ObjetoTurma.Curso != "9999.04"
                    && this.ObjetoTurma.Curso != "9999.91"
                    && this.ObjetoTurma.Curso != "2025.02"
                    && this.ObjetoTurma.Curso != "2025.03"
                    && this.ObjetoTurma.Curso != "2025.04"
                    && this.ObjetoTurma.Curso != "2025.05"
                    )
                {

                    if ((((Convert.ToDecimal(txtAno.Text) == 2022 || Convert.ToDecimal(txtAno.Text) == 2023) && Convert.ToDecimal(txtPeriodo.Text) == 0 && ObjetoTurma.Curso == "0002.31" && (ObjetoTurma.Serie == "2" || ObjetoTurma.Serie == "3")) ||
                               ((Convert.ToDecimal(txtAno.Text) == 2022 || Convert.ToDecimal(txtAno.Text) == 2023) && Convert.ToDecimal(txtPeriodo.Text) == 2 && ObjetoTurma.Curso == "0002.83" && (ObjetoTurma.Serie == "3")) ||
                                (rnCurso.EhItinerarioFormativoTrihaComMatrizPor(ObjetoTurma.Curso, Convert.ToDecimal(txtAno.Text), Convert.ToDecimal(txtPeriodo.Text))
                                && ((Convert.ToDecimal(txtPeriodo.Text) == 0 && ObjetoTurma.Serie == "2") ||
                                    (Convert.ToDecimal(txtPeriodo.Text) == 2 && (ObjetoTurma.Serie == "3" || ObjetoTurma.Serie == "4")))
                              )
                                )
                               && (this.rbReprovadoFalta.Checked || this.rbReprovadoNota.Checked))
                    {
                        lblMensagem.Text = "Aluno(s) retido(s) na 2ª/3ª Série do Regular ou EJA III/IV ficarão em pendência de enturmação para futura escolha do curso/série.";
                    }
                    else
                    {
                        if (Convert.ToDecimal(txtAno.Text) >= DateTime.Now.Year - 1)
                        {
                            lblMensagem.Text = @"Para aluno com renovação ou confirmação ativa, selecione  a opção Efetuar Matrícula e enturme na etapa correspondente a sua situação final. Verifique o(s) aluno(s) abaixo: <br />" + mensagensAlunoRenovacaoAtiva.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                            return;
                        }
                    }
                }


                if (rbAprovar.Checked &&
                    ((ObjetoTurma.Serie == "9" && (ObjetoTurma.Curso == "0001.42" || ObjetoTurma.Curso == "0001.21"))
                     || (ObjetoTurma.Serie == "4" && ObjetoTurma.Curso == "0092.30"))
                        )
                {
                    if (mensagensAlunoRenovacaoAtiva.Count > 0 || mensagensAlunoRenovacaoNaoAtiva.Count > 0)
                    {
                        if (rdNao.Checked && mensagensAlunoRenovacaoAtiva.Count > 0)
                        {
                            lblMensagem.Text = @"Para aluno aprovado com confirmação ativa, selecione  a opção Efetuar Matrícula. Verifique o(s) aluno(s) abaixo: <br />" + mensagensAlunoRenovacaoAtiva.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                            return;
                        }

                        if (rdSim.Checked && mensagensAlunoRenovacaoNaoAtiva.Count > 0)
                        {
                            lblMensagem.Text = @"Para aluno aprovado sem confirmação ativa nesta escola, selecione  a opção Não Efetuar Matrícula. Verifique o(s) aluno(s) abaixo: <br />" + mensagensAlunoRenovacaoNaoAtiva.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                            return;
                        }
                    }
                }



                if (this.rbAprovarComDep.Checked)
                {
                    for (var i = 0; i < this.grdDependencia.VisibleRowCount; i++)
                    {
                        var chkDependencia = DevExpressHelper.GetControl<ASPxCheckBox>(this.grdDependencia, i, "DEPENDENCIA", "chkDependencia");

                        if (chkDependencia == null)
                        {
                            continue;
                        }

                        chkDependencia.Checked = false;
                    }

                    this.pcDependencia.ShowOnPageLoad = true;

                    return;
                }



                dtRenovacoesDuplicadas = rnRenovacao.ObtemListaAlunosComDuplicicadeRenovacaoPor(alunos);

                if (dtRenovacoesDuplicadas.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRenovacoesDuplicadas.Rows)
                    {
                        mensagensRenovacaoDuplicada.Add(String.Format("{0} - {1} ", row["ALUNOID"].ToString(), row["NOME_COMPL"].ToString())); ;
                    }

                    lblMensagem.Text = @"Para prosseguir com o fechamento é necessário verificar os dados da renovação do(s) aluno(s) abaixo: <br />" + mensagensRenovacaoDuplicada.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                    return;
                }

                if (matriculaNova)
                {
                    curriculo = Turma.RetornaCurriculo(

                        Convert.ToDecimal(this.ddlAno.SelectedValue),
                        Convert.ToDecimal(this.ddlPeriodo.SelectedValue),
                        this.ddlTurno.SelectedValue,
                        this.tseCurso.DBValue.ToString(),
                        ddlUnidadeEnsino.SelectedValue.ToString(),
                        this.ddlTurma.SelectedValue);

                    if (string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                    {
                        this.lblMensagem.Text = "Selecione um período para realizar a enturmação.";
                        return;
                    }
                }

                if (!matriculaNova)
                {
                    if (string.IsNullOrEmpty(ddlAno.SelectedValue) || string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                    {

                        if (anoPeriodo.Length > 0 && anoPeriodo[0] > 0)
                        {
                            anoProximo = Convert.ToDecimal(anoPeriodo[0]);
                            periodoProximo = Convert.ToDecimal(anoPeriodo[1]);
                        }
                    }

                }



                if (this.rbAprovar.Checked)
                {
                    if (matriculaNova)
                    {
                        rnFechamentoMatricula.AprovaAlunosComEnturmacao(this.ObjetoTurma,
                            alunos,
                            this.ddlAno.SelectedValue,
                            this.ddlPeriodo.SelectedValue,
                            this.tseCurso.DBValue.ToString(),
                            this.ddlTurno.SelectedValue,
                            curriculo,
                            this.ddlSerie.SelectedValue,
                            this.ddlTurma.SelectedValue,
                            User.Identity.Name,
                            ddlUnidadeEnsino.SelectedValue.ToString());

                    }
                    else
                    {
                        rnFechamentoMatricula.AprovaAlunos(this.ObjetoTurma, alunos, User.Identity.Name, anoProximo, periodoProximo);
                    }
                }
                else if (this.rbReprovadoFalta.Checked)
                {
                    if (matriculaNova)
                    {
                        if (this.ddlSerie.SelectedValue != this.txtSerieVal.Text)
                        {
                            this.lblMensagem.Text = "Não é possível mudar o ano de escolaridade para reprovar. Favor selecionar o mesmo ano de escolaridade.";

                            return;
                        }
                        rnFechamentoMatricula.ReprovaAlunosPorFrequenciaComEnturmacao(this.ObjetoTurma,
                            alunos,
                            this.ddlAno.SelectedValue,
                            this.ddlPeriodo.SelectedValue,
                            this.tseCurso.DBValue.ToString(),
                            this.ddlTurno.SelectedValue,
                            curriculo,
                            this.ddlSerie.SelectedValue,
                            this.ddlTurma.SelectedValue,
                            User.Identity.Name,
                            ddlUnidadeEnsino.SelectedValue.ToString());
                    }
                    else
                    {
                        rnFechamentoMatricula.ReprovaAlunosPorFrequencia(this.ObjetoTurma, alunos, User.Identity.Name, anoProximo, periodoProximo);
                    }
                }
                else if (rbPromovido.Checked)
                {
                    if (matriculaNova)
                    {
                        rnFechamentoMatricula.PromoveAlunosComEnturmacao(this.ObjetoTurma,
                            alunos,
                            this.ddlAno.SelectedValue,
                            this.ddlPeriodo.SelectedValue,
                            this.tseCurso.DBValue.ToString(),
                            this.ddlTurno.SelectedValue,
                            curriculo,
                            this.ddlSerie.SelectedValue,
                            this.ddlTurma.SelectedValue,
                            User.Identity.Name,
                            ddlUnidadeEnsino.SelectedValue.ToString());

                    }
                    else
                    {
                        rnFechamentoMatricula.PromoveAlunos(this.ObjetoTurma, alunos, User.Identity.Name, anoProximo, periodoProximo);
                    }
                }
                else if (rbRetido.Checked)
                {
                    if (matriculaNova)
                    {
                        if (ObjetoTurma.Curso == "0092.30" && ObjetoTurma.Serie == "4")
                        {
                            if (this.ddlSerie.SelectedValue != "9")
                            {
                                this.lblMensagem.Text = "Para aluno(s) do Correção de Fluxo do módulo IV retido(s), deverão ser alocados no 9º ano do EF anos finais, até que possam fazer suas classificações.";

                                return;
                            }
                        }
                        else
                        {
                            if (this.ddlSerie.SelectedValue != this.txtSerieVal.Text)
                            {
                                this.lblMensagem.Text = "Não é possível mudar o ano de escolaridade para reprovar. Favor selecionar o mesmo ano de escolaridade.";

                                return;
                            }
                        }

                        rnFechamentoMatricula.RetemAlunosPorNotaComEnturmacao(
                             this.ObjetoTurma,
                            alunos,
                            this.ddlAno.SelectedValue,
                            this.ddlPeriodo.SelectedValue,
                            this.tseCurso.DBValue.ToString(),
                            this.ddlTurno.SelectedValue,
                            curriculo,
                            this.ddlSerie.SelectedValue,
                            this.ddlTurma.SelectedValue,
                            User.Identity.Name,
                            ddlUnidadeEnsino.SelectedValue.ToString());

                    }
                    else
                    {
                        rnFechamentoMatricula.RetemAlunos(this.ObjetoTurma, alunos, User.Identity.Name, anoProximo, periodoProximo);
                    }
                }

                else
                {
                    if (matriculaNova)
                    {
                        if (this.ddlSerie.SelectedValue != this.txtSerieVal.Text)
                        {
                            this.lblMensagem.Text = "Não é possível mudar o ano de escolaridade para reprovar. Favor selecionar o mesmo ano de escolaridade.";

                            return;
                        }
                        rnFechamentoMatricula.ReprovaAlunosPorNotaComEnturmacao(
                             this.ObjetoTurma,
                            alunos,
                            this.ddlAno.SelectedValue,
                            this.ddlPeriodo.SelectedValue,
                            this.tseCurso.DBValue.ToString(),
                            this.ddlTurno.SelectedValue,
                            curriculo,
                            this.ddlSerie.SelectedValue,
                            this.ddlTurma.SelectedValue,
                            User.Identity.Name,
                            ddlUnidadeEnsino.SelectedValue.ToString());

                    }
                    else
                    {
                        rnFechamentoMatricula.ReprovaAlunosPorNota(this.ObjetoTurma, alunos, User.Identity.Name, anoProximo, periodoProximo);
                    }
                }

                this.lblMensagem.Text = "Atualização concluída com sucesso.";

                if (rdSim.Checked)
                {
                    this.ddlTurma.DataSource = Turma.ConsultarPrimeiraTurmaDisponivel(
                            Convert.ToDecimal(this.ddlAno.SelectedValue),
                            Convert.ToDecimal(this.ddlPeriodo.SelectedValue),
                            ddlUnidadeEnsino.SelectedValue.ToString(),
                            this.ddlTurno.SelectedValue,
                            this.tseCurso.DBValue.ToString(),
                            Convert.ToDecimal(this.ddlSerie.SelectedValue));

                    this.ddlTurma.DataBind();
                    this.ddlTurma.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                }

                this.grdMatriculas.DataBind();
                this.grdHistorico.DataBind();

                //Verificar se existe dados de historico e progressao para exibir os grids
                ControlaVisibilidadeHistorico();
                ControlaVisibilidadeProgressao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnMarcar_Click(object sender, EventArgs e)
        {
            this.AlterarSelecaoEmMatriculas(true, grdMatriculas);
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("FechamentoMatriculaLista.aspx");
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlPeriodo.DataBind();
            this.ddlTurno.DataBind();
            this.ddlSerie.Enabled = false;
            this.ddlSerie.Items.Clear();
            this.ddlTurma.Items.Clear();
            this.ddlTurma.Enabled = false;
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlTurno.DataBind();
            this.ddlSerie.Enabled = false;
            this.ddlSerie.Items.Clear();
            this.ddlTurma.Items.Clear();
            this.ddlTurma.Enabled = false;
            this.tseCurso.ResetValue();
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.GradeSerie rnGradeSerie = new GradeSerie();

                if (!string.IsNullOrEmpty(this.ObjetoTurma.Ano) && !string.IsNullOrEmpty(this.ObjetoTurma.Periodo))
                {
                    var qt = rnGradeSerie.ObtemConsultaTurmasParaFechamentoMatriculaPor(
                        this.ObjetoTurma.Ano,
                        this.ObjetoTurma.Periodo,
                        this.ObjetoTurma.Curso,
                        this.ObjetoTurma.Turno,
                        this.ObjetoTurma.UnidadeResponsavel,
                        this.ObjetoTurma.Grade);

                    var sr = qt.Rows[0];

                    this.ddlTurma.Items.Clear();
                    this.ddlTurma.Enabled = false;

                    if (string.IsNullOrEmpty(this.ddlSerie.SelectedValue))
                    {
                        return;
                    }

                    this.ddlTurma.Enabled = true;

                    this.ddlTurma.DataSource = Turma.ConsultarPrimeiraTurmaDisponivel(
                        Convert.ToDecimal(this.ddlAno.SelectedValue),
                        Convert.ToDecimal(this.ddlPeriodo.SelectedValue),
                        ddlUnidadeEnsino.SelectedValue.ToString(),
                        this.ddlTurno.SelectedValue,
                        this.tseCurso.DBValue.ToString(),
                        Convert.ToDecimal(this.ddlSerie.SelectedValue));

                    this.ddlTurma.DataBind();
                    this.ddlTurma.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ddlSerie.Items.Clear();
                this.ddlSerie.Enabled = false;
                this.ddlTurma.Items.Clear();
                this.ddlTurma.Enabled = false;

                if (string.IsNullOrEmpty(this.ddlTurno.SelectedValue) || string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                {
                    lblMensagem.Text = "Favor preencher o Período/Escolaridade/Turno.";
                    return;
                }

                this.ddlSerie.Enabled = true;
                this.ddlSerie.DataSource = Serie.ListarSerie(this.ddlUnidadeEnsino.SelectedValue.ToString(), Convert.ToDecimal(this.ddlAno.SelectedValue), Convert.ToDecimal(this.ddlPeriodo.SelectedValue), this.ddlTurno.SelectedValue, this.tseCurso.DBValue.ToString());
                this.ddlSerie.DataBind();
                this.ddlSerie.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdMatriculas_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    return;
                }

                this.pucInfoAluno.ShowOnPageLoad = true;
                var matricula = this.grdMatriculas.GetRowValues(this.GetSelectedRowOnTheCurrentPage(grdMatriculas), "aluno");

                if (matricula == null)
                {
                    return;
                }

                RN.Entidades.LyAluno rowAluno = Aluno.Carregar(matricula.ToString());

                if (rowAluno != null)
                {
                    if (rowAluno.Pessoa.HasValue)
                    {
                        RN.Entidades.LyFotoPessoa rowFoto = FotoPessoa.Carregar(Convert.ToInt32(rowAluno.Pessoa));

                        if (rowFoto == null)
                        {
                            this.bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                            this.bimgFotoPessoa.EmptyImage.AlternateText = "sem foto";
                            this.bimgFotoPessoa.ContentBytes = null;
                        }
                        else
                        {
                            try
                            {
                                //Tenta carregar array de bytes em objeto Image. 
                                //Em caso de exceção, a foto está em formato inválido
                                Image.FromStream(new MemoryStream(rowFoto.Foto));
                                this.bimgFotoPessoa.ContentBytes = rowFoto.Foto;
                            }
                            catch
                            {
                                this.bimgFotoPessoa.EmptyImage.Url = "~/Images/fotoinvalida.jpg";
                                this.bimgFotoPessoa.EmptyImage.AlternateText = "foto inválida";
                                this.bimgFotoPessoa.ContentBytes = null;
                            }
                        }
                    }

                    if (!rowAluno.Pessoa.HasValue)
                    {
                        return;
                    }

                    RN.Entidades.LyPessoa rowPessoa = Pessoa.Carregar(Convert.ToInt32(rowAluno.Pessoa));

                    this.lblNome.Text = string.IsNullOrEmpty(rowPessoa.Nome_compl) ? NaoCadastrado : rowPessoa.Nome_compl;
                    this.lblNomePai.Text = string.IsNullOrEmpty(rowPessoa.NomePai) ? NaoCadastrado : rowPessoa.NomePai;
                    this.lblNomeMae.Text = string.IsNullOrEmpty(rowPessoa.NomeMae) ? NaoCadastrado : rowPessoa.NomeMae;

                    var emails = new[]
                             {
                                 rowAluno.EMailInterno, 
                                 rowPessoa.E_mail_interno, 
                                 rowPessoa.E_mail 
                                 
                             };

                    var emailsValidos = emails
                        .Where(em => !string.IsNullOrEmpty(em))
                        .ToList();

                    if (emailsValidos.Count() > 0)
                    {
                        this.hlEmail.Text = emailsValidos.First();
                        this.hlEmail.NavigateUrl = "mailto:" + emailsValidos.First();
                        this.hlEmail.Visible = true;
                        this.lblEmail.Visible = false;
                    }
                    else
                    {
                        this.hlEmail.Visible = false;
                        this.lblEmail.Visible = true;
                    }
                }
                else
                {
                    this.bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                    this.bimgFotoPessoa.EmptyImage.AlternateText = "sem foto";
                    this.bimgFotoPessoa.ContentBytes = null;
                }

                this.grdMatriculas.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdEletivas_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    return;
                }

                this.pucInfoAluno.ShowOnPageLoad = true;
                var matricula = this.grdEletivas.GetRowValues(this.GetSelectedRowOnTheCurrentPage(grdEletivas), "aluno");

                if (matricula == null)
                {
                    return;
                }

                RN.Entidades.LyAluno rowAluno = Aluno.Carregar(matricula.ToString());

                if (rowAluno != null)
                {
                    if (rowAluno.Pessoa.HasValue)
                    {
                        RN.Entidades.LyFotoPessoa rowFoto = FotoPessoa.Carregar(Convert.ToInt32(rowAluno.Pessoa));

                        if (rowFoto == null)
                        {
                            this.bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                            this.bimgFotoPessoa.EmptyImage.AlternateText = "sem foto";
                            this.bimgFotoPessoa.ContentBytes = null;
                        }
                        else
                        {
                            try
                            {
                                //Tenta carregar array de bytes em objeto Image. 
                                //Em caso de exceção, a foto está em formato inválido
                                Image.FromStream(new MemoryStream(rowFoto.Foto));
                                this.bimgFotoPessoa.ContentBytes = rowFoto.Foto;
                            }
                            catch
                            {
                                this.bimgFotoPessoa.EmptyImage.Url = "~/Images/fotoinvalida.jpg";
                                this.bimgFotoPessoa.EmptyImage.AlternateText = "foto inválida";
                                this.bimgFotoPessoa.ContentBytes = null;
                            }
                        }
                    }

                    if (!rowAluno.Pessoa.HasValue)
                    {
                        return;
                    }

                    RN.Entidades.LyPessoa rowPessoa = Pessoa.Carregar(Convert.ToInt32(rowAluno.Pessoa));

                    this.lblNome.Text = string.IsNullOrEmpty(rowPessoa.Nome_compl) ? NaoCadastrado : rowPessoa.Nome_compl;
                    this.lblNomePai.Text = string.IsNullOrEmpty(rowPessoa.NomePai) ? NaoCadastrado : rowPessoa.NomePai;
                    this.lblNomeMae.Text = string.IsNullOrEmpty(rowPessoa.NomeMae) ? NaoCadastrado : rowPessoa.NomeMae;

                    var emails = new[]
                             {
                                 rowAluno.EMailInterno, 
                                 rowPessoa.E_mail_interno, 
                                 rowPessoa.E_mail
                             };

                    var emailsValidos = emails
                        .Where(em => !string.IsNullOrEmpty(em))
                        .ToList();

                    if (emailsValidos.Count() > 0)
                    {
                        this.hlEmail.Text = emailsValidos.First();
                        this.hlEmail.NavigateUrl = "mailto:" + emailsValidos.First();
                        this.hlEmail.Visible = true;
                        this.lblEmail.Visible = false;
                    }
                    else
                    {
                        this.hlEmail.Visible = false;
                        this.lblEmail.Visible = true;
                    }
                }
                else
                {
                    this.bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                    this.bimgFotoPessoa.EmptyImage.AlternateText = "sem foto";
                    this.bimgFotoPessoa.ContentBytes = null;
                }

                this.grdEletivas.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdProgressaoParcial_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    return;
                }

                this.pucInfoAluno.ShowOnPageLoad = true;
                var matricula = this.grdProgressaoParcial.GetRowValues(this.GetSelectedRowOnTheCurrentPage(grdProgressaoParcial), "aluno");

                if (matricula == null)
                {
                    return;
                }

                RN.Entidades.LyAluno rowAluno = Aluno.Carregar(matricula.ToString());

                if (rowAluno != null)
                {
                    if (rowAluno.Pessoa.HasValue)
                    {
                        RN.Entidades.LyFotoPessoa rowFoto = FotoPessoa.Carregar(Convert.ToInt32(rowAluno.Pessoa));

                        if (rowFoto == null)
                        {
                            this.bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                            this.bimgFotoPessoa.EmptyImage.AlternateText = "sem foto";
                            this.bimgFotoPessoa.ContentBytes = null;
                        }
                        else
                        {
                            try
                            {
                                //Tenta carregar array de bytes em objeto Image. 
                                //Em caso de exceção, a foto está em formato inválido
                                Image.FromStream(new MemoryStream(rowFoto.Foto));
                                this.bimgFotoPessoa.ContentBytes = rowFoto.Foto;
                            }
                            catch
                            {
                                this.bimgFotoPessoa.EmptyImage.Url = "~/Images/fotoinvalida.jpg";
                                this.bimgFotoPessoa.EmptyImage.AlternateText = "foto inválida";
                                this.bimgFotoPessoa.ContentBytes = null;
                            }
                        }
                    }

                    if (!rowAluno.Pessoa.HasValue)
                    {
                        return;
                    }

                    RN.Entidades.LyPessoa rowPessoa = Pessoa.Carregar(Convert.ToInt32(rowAluno.Pessoa));

                    this.lblNome.Text = string.IsNullOrEmpty(rowPessoa.Nome_compl) ? NaoCadastrado : rowPessoa.Nome_compl;
                    this.lblNomePai.Text = string.IsNullOrEmpty(rowPessoa.NomePai) ? NaoCadastrado : rowPessoa.NomePai;
                    this.lblNomeMae.Text = string.IsNullOrEmpty(rowPessoa.NomeMae) ? NaoCadastrado : rowPessoa.NomeMae;

                    var emails = new[]
                             {
                                 rowAluno.EMailInterno, 
                                 rowPessoa.E_mail_interno, 
                                 rowPessoa.E_mail
                             };

                    var emailsValidos = emails
                        .Where(em => !string.IsNullOrEmpty(em))
                        .ToList();

                    if (emailsValidos.Count() > 0)
                    {
                        this.hlEmail.Text = emailsValidos.First();
                        this.hlEmail.NavigateUrl = "mailto:" + emailsValidos.First();
                        this.hlEmail.Visible = true;
                        this.lblEmail.Visible = false;
                    }
                    else
                    {
                        this.hlEmail.Visible = false;
                        this.lblEmail.Visible = true;
                    }
                }
                else
                {
                    this.bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                    this.bimgFotoPessoa.EmptyImage.AlternateText = "sem foto";
                    this.bimgFotoPessoa.ContentBytes = null;
                }

                this.grdProgressaoParcial.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdHistorico_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    return;
                }

                this.pucInfoAluno.ShowOnPageLoad = true;
                var matricula = this.grdHistorico.GetRowValues(this.GetSelectedRowOnTheCurrentPage(grdHistorico), "aluno");

                if (matricula == null)
                {
                    return;
                }

                RN.Entidades.LyAluno rowAluno = Aluno.Carregar(matricula.ToString());

                if (rowAluno != null)
                {
                    if (rowAluno.Pessoa.HasValue)
                    {
                        RN.Entidades.LyFotoPessoa rowFoto = FotoPessoa.Carregar(Convert.ToInt32(rowAluno.Pessoa));

                        if (rowFoto == null)
                        {
                            this.bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                            this.bimgFotoPessoa.EmptyImage.AlternateText = "sem foto";
                            this.bimgFotoPessoa.ContentBytes = null;
                        }
                        else
                        {
                            try
                            {
                                //Tenta carregar array de bytes em objeto Image. 
                                //Em caso de exceção, a foto está em formato inválido
                                Image.FromStream(new MemoryStream(rowFoto.Foto));
                                this.bimgFotoPessoa.ContentBytes = rowFoto.Foto;
                            }
                            catch
                            {
                                this.bimgFotoPessoa.EmptyImage.Url = "~/Images/fotoinvalida.jpg";
                                this.bimgFotoPessoa.EmptyImage.AlternateText = "foto inválida";
                                this.bimgFotoPessoa.ContentBytes = null;
                            }
                        }
                    }

                    if (!rowAluno.Pessoa.HasValue)
                    {
                        return;
                    }

                    RN.Entidades.LyPessoa rowPessoa = Pessoa.Carregar(Convert.ToInt32(rowAluno.Pessoa));

                    this.lblNome.Text = string.IsNullOrEmpty(rowPessoa.Nome_compl) ? NaoCadastrado : rowPessoa.Nome_compl;
                    this.lblNomePai.Text = string.IsNullOrEmpty(rowPessoa.NomePai) ? NaoCadastrado : rowPessoa.NomePai;
                    this.lblNomeMae.Text = string.IsNullOrEmpty(rowPessoa.NomeMae) ? NaoCadastrado : rowPessoa.NomeMae;

                    var emails = new[]
                             {
                                 rowAluno.EMailInterno, 
                                 rowPessoa.E_mail_interno, 
                                 rowPessoa.E_mail
                             };

                    var emailsValidos = emails
                        .Where(em => !string.IsNullOrEmpty(em))
                        .ToList();

                    if (emailsValidos.Count() > 0)
                    {
                        this.hlEmail.Text = emailsValidos.First();
                        this.hlEmail.NavigateUrl = "mailto:" + emailsValidos.First();
                        this.hlEmail.Visible = true;
                        this.lblEmail.Visible = false;
                    }
                    else
                    {
                        this.hlEmail.Visible = false;
                        this.lblEmail.Visible = true;
                    }
                }
                else
                {
                    this.bimgFotoPessoa.EmptyImage.Url = "~/Images/semfoto.jpg";
                    this.bimgFotoPessoa.EmptyImage.AlternateText = "sem foto";
                    this.bimgFotoPessoa.ContentBytes = null;
                }

                this.grdHistorico.Visible = true;

                //Verificar se existe dados de historico para exibir o grid
                ControlaVisibilidadeHistorico();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rdNao_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlaRadios();
        }

        protected void rdSim_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlaRadios();
        }

        protected void tseCurso_Changed(object sender, EventArgs e)
        {
            try
            {
                this.ddlSerie.Items.Clear();
                this.ddlSerie.Enabled = false;
                this.ddlTurma.Items.Clear();
                this.ddlTurma.Enabled = false;

                if (this.tseCurso.IsValidDBValue && !this.tseCurso.DBValue.IsNull)
                {
                    if (!string.IsNullOrEmpty(this.ddlTurno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                    {
                        this.ddlSerie.Enabled = true;
                        this.ddlSerie.DataSource = Serie.ListarSerie(this.ddlUnidadeEnsino.SelectedValue.ToString(), Convert.ToDecimal(this.ddlAno.SelectedValue), Convert.ToDecimal(this.ddlPeriodo.SelectedValue), this.ddlTurno.SelectedValue, this.tseCurso.DBValue.ToString());
                        this.ddlSerie.DataBind();
                        this.ddlSerie.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void AlterarSelecaoEmMatriculas(bool selecionado, ASPxGridView grid)
        {
            for (var i = 0; i < grid.VisibleRowCount; i++)
            {
                var chkBox = DevExpressHelper.GetControl<ASPxCheckBox>(grid, i, "selecionar", "chkBox");

                if (chkBox == null)
                {
                    continue;
                }

                chkBox.Checked = selecionado;
            }
        }

        private void CarregarDadosTurma()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["Turma"]))
                {
                    var parametrosValidos = this.ObterDadosQueryStringRelatorio();

                    if (!parametrosValidos)
                    {
                        this.pnDadosTurma.Visible = false;
                        this.upnlMatriculas.Visible = false;
                        this.lblMensagemRelatorioTurmaInvalida.Visible = true;

                        this.lblMensagemRelatorioTurmaInvalida.Text = string.Format(
                            "Turma inválida. Parâmetros: Turma={0}, Ano={1}, Semestre={2}",
                            this.Request.QueryString["Turma"],
                            this.Request.QueryString["Ano"],
                            this.Request.QueryString["Semestre"]);
                    }
                }
                else
                {
                    var decodedBytes = Convert.FromBase64String(this.Request.QueryString["Chave"]);
                    var decodedText = Encoding.UTF8.GetString(decodedBytes);

                    this.ObterDadosQueryString(decodedText);
                }

                this.PreencherDadosTurma();
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }

        private void ControlaRadios()
        {
            this.divEfetuarMatricula.Visible = !this.rdNao.Checked;
        }

        private bool DadosValidos(ICollection<string> alunos)
        {
            if (alunos.Count <= 0)
            {
                this.lblMensagem.Text = "É necessário selecionar de 1 a " + hdnQtdeDep.Value + " disciplinas para dependência.";

                return false;
            }

            var matriculaNova = this.rdSim.Checked;

            if (matriculaNova)
            {
                if (string.IsNullOrEmpty(this.ddlAno.SelectedValue)
                    || string.IsNullOrEmpty(this.ddlPeriodo.SelectedValue)
                    || !this.tseCurso.IsValidDBValue
                    || this.tseCurso.DBValue.IsNull
                    || string.IsNullOrEmpty(this.ddlTurno.SelectedValue)
                    || string.IsNullOrEmpty(this.ddlTurma.SelectedValue)
                    || string.IsNullOrEmpty(this.ddlSerie.SelectedValue))
                {
                    this.lblMensagem.Text = "Selecione todos os dados da nova matrícula.";

                    return false;
                }
            }

            return true;
        }

        private int GetSelectedRowOnTheCurrentPage(ASPxGridView grid)
        {
            var startIndexOnPage = grid.PageIndex * grid.SettingsPager.PageSize;
            var selectedRow = -1;

            for (var i = 0; i < grid.VisibleRowCount; i++)
            {
                if (grid.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    selectedRow = startIndexOnPage + i;

                    break;
                }
            }

            grid.Selection.UnselectAll();
            return selectedRow;
        }

        private void InitializeComponent()
        {
        }

        private List<string> ObterAlunos()
        {
            var alunos = new List<string>();

            for (var i = 0; i < this.grdMatriculas.VisibleRowCount; i++)
            {
                var optSel = this.grdMatriculas.FindRowCellTemplateControl(i, (GridViewDataColumn)this.grdMatriculas.Columns[0], "chkBox") as ASPxCheckBox;

                if (optSel != null
                    && optSel.Checked)
                {
                    alunos.Add(this.grdMatriculas.GetRowValues(i, "aluno").ToString());
                }
            }

            return alunos;
        }

        private void ObterDadosQueryString(string queryString)
        {
            try
            {
                this.ObjetoTurma = new Turma.DadosTurma();

                var listaDados = queryString.Split('&');

                foreach (var dados in listaDados)
                {
                    if (dados.IndexOf("ano") >= 0)
                    {
                        this.ObjetoTurma.Ano = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("semestre") >= 0)
                    {
                        this.ObjetoTurma.Periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("turno") >= 0)
                    {
                        this.ObjetoTurma.Turno = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("curso") >= 0)
                    {
                        this.ObjetoTurma.Curso = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("unidadeResponsavel") >= 0)
                    {
                        this.ObjetoTurma.UnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("prefixoUnidadeResponsavel") >= 0)
                    {
                        this.ObjetoTurma.MnemonicoUnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("gradeId") >= 0)
                    {
                        this.ObjetoTurma.Grade_ID = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("grade") >= 0)
                    {
                        string turma = dados.Substring(dados.LastIndexOf('=') + 1);
                        this.ObjetoTurma.Grade = turma.Replace("##", "&");
                    }
                    else if (dados.IndexOf("serie") >= 0)
                    {
                        this.ObjetoTurma.Serie = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("curriculo") >= 0)
                    {
                        this.ObjetoTurma.Curriculo = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("faculdade") >= 0)
                    {
                        this.ObjetoTurma.Faculdade = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("nucleo") >= 0)
                    {
                        this.ObjetoTurma.Nucleo = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("municipio") >= 0)
                    {
                        this.ObjetoTurma.Municipio = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("optativareforco") >= 0)
                    {
                        this.ObjetoTurma.OptativaReforco = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("eletiva") >= 0)
                    {
                        this.ObjetoTurma.Eletiva = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private bool ObterDadosQueryStringRelatorio()
        {
            var turma = this.Request.QueryString["Turma"];
            var ano = this.Request.QueryString["Ano"];
            var semestre = this.Request.QueryString["Semestre"];
            var dtTurma = Turma.Consultar(turma, ano, semestre);

            if (dtTurma.Rows.Count > 0)
            {
                var rowTurma = dtTurma.Rows[0];

                this.ObjetoTurma = new Turma.DadosTurma
                {
                    Ano = Convert.ToString(rowTurma.Ano),
                    Curriculo = rowTurma.Curriculo,
                    Curso = rowTurma.Curso,
                    Faculdade = rowTurma.Faculdade,
                    Grade = rowTurma.Turma,
                    Grade_ID = Convert.ToString(Turma.ObterGrade(rowTurma)),
                    Turno = rowTurma.Turno,
                    Periodo = Convert.ToString(rowTurma.Semestre),
                    Serie = Convert.ToString(rowTurma.Serie),
                    UnidadeResponsavel = rowTurma.Unidade_responsavel,
                    OptativaReforco = rowTurma.OptativaReforco,
                    Eletiva = rowTurma.Eletiva
                };

                return true;
            }

            return false;
        }

        private List<string> ObterDisciplinas()
        {
            var disciplinas = new List<string>();

            for (var i = 0; i < this.grdDependencia.VisibleRowCount; i++)
            {
                var chkDependencia = DevExpressHelper.GetControl<ASPxCheckBox>(this.grdDependencia, i, "DEPENDENCIA", "chkDependencia");

                if (chkDependencia == null
                    || !chkDependencia.Checked)
                {
                    continue;
                }

                disciplinas.Add(Convert.ToString(this.grdDependencia.GetRowValues(i, "DISCIPLINA")));
            }

            return disciplinas;
        }

        private void PreencherDadosTurma()
        {
            try
            {
                RN.GradeSerie rnGradeSerie = new GradeSerie();
                RN.FechamentoMatricula rnFechamentoMatricula = new Techne.Lyceum.RN.FechamentoMatricula();
                hdnSerieConcluinte.Value = string.Empty;
                hdnRegional.Value = string.Empty;

                if (!string.IsNullOrEmpty(this.ObjetoTurma.Ano) && !string.IsNullOrEmpty(this.ObjetoTurma.Periodo))
                {

                    var qt = rnGradeSerie.ObtemConsultaTurmasParaFechamentoMatriculaPor(
                        this.ObjetoTurma.Ano,
                        this.ObjetoTurma.Periodo,
                        this.ObjetoTurma.Curso,
                        this.ObjetoTurma.Turno,
                        this.ObjetoTurma.UnidadeResponsavel,
                        this.ObjetoTurma.Grade);

                    if (qt.Rows.Count > 0)
                    {
                        var sr = qt.Rows[0];

                        this.txtUnidadeEns.Text = sr["nomeUnidadeResponsavel"].ToString();
                        this.txtUnidadeEnsVal.Text = sr["unidade_responsavel"].ToString();
                        this.txtTurmaTop.Text = this.txtTurma.Text = sr["grade"].ToString();
                        this.txtAnoTop.Text = this.txtAno.Text = this.ObjetoTurma.Ano;
                        this.txtPeriodoTop.Text = this.txtPeriodo.Text = this.ObjetoTurma.Periodo;
                        this.txtEscolaridade.Text = sr["nomeCurso"].ToString();
                        this.txtEscolaridadeVal.Text = sr["curso"].ToString();
                        this.txtTurno.Text = sr["descricaoTurno"].ToString();
                        this.txtTurnoVal.Text = sr["turno"].ToString();
                        this.txtCurriculoTop.Text = this.txtCurriculo.Text = sr["curriculo"].ToString();
                        this.txtSerie.Text = sr["descricaoSerie"].ToString();
                        this.txtSerieVal.Text = sr["serie"].ToString();
                        this.txtSituacaoTurma.Text = sr["sit_turma"].ToString();
                        hdnSerieConcluinte.Value = sr["ANO_SERIE_CONCLUINTE"].ToString();
                        hdnRegional.Value = sr["ID_REGIONAL"].ToString();
                        hdnEletiva.Value = sr["ELETIVA"].ToString();

                        if (txtAno.Text == "2025")
                        {
                            int valor = rnFechamentoMatricula.ObtemDependenciasPermitidasPor(txtEscolaridadeVal.Text,Convert.ToInt32(txtSerieVal.Text));

                            if (valor > 0)
                            {
                                hdnQtdeDep.Value = valor.ToString();
                            }
                            else
                            {
                                hdnQtdeDep.Value = "2";
                            }
                        }
                        if (!string.IsNullOrEmpty(sr["DT_INICIO"].ToString()))
                        {
                            this.txtDataInicio.Text = Convert.ToDateTime(sr["DT_INICIO"]).ToString("dd/MM/yyyy");
                            this.txtDataFim.Text = Convert.ToDateTime(sr["DT_FIM"]).ToString("dd/MM/yyyy");
                            ObjetoTurma.dtInicio = Convert.ToDateTime(sr["DT_INICIO"]);
                            ObjetoTurma.dtFim = Convert.ToDateTime(sr["DT_FIM"]);
                        }
                        if (!string.IsNullOrEmpty(txtUnidadeEns.Text.Trim()))
                            ObjetoTurma.NomeUnidade = txtUnidadeEns.Text.Trim();

                        //Não deixar enturmar turmas do tipo optativa reforço
                        if (sr["optativareforco"].ToString() != "N")
                        {
                            txtOptativaReforco.Text = "S";
                            rdSim.Visible = false;
                            this.divEfetuarMatricula.Visible = false;
                            rdNao.Checked = true;
                            this.ObjetoTurma.OptativaReforco = RN.Turma.VerificaOptativaReforco(txtTurma.Text, txtUnidadeEnsVal.Text, Convert.ToInt32(txtAno.Text), Convert.ToInt32(txtPeriodo.Text));
                        }
                        else
                        {
                            txtOptativaReforco.Text = "N";
                            this.ObjetoTurma.OptativaReforco = "N";
                        }

                        //Não deixar enturmar cursos do tipo educacao especial e mais educacao
                        if (sr["curso"].ToString() == "9999.91" || sr["curso"].ToString() == "9999.92" 
                            || sr["curso"].ToString() == "9999.01" || sr["curso"].ToString() == "9999.03" 
                            || sr["curso"].ToString() == "9999.02" || sr["curso"].ToString() == "9999.04"
                            || sr["curso"].ToString() == "2025.02" || sr["curso"].ToString() == "2025.03"
                            || sr["curso"].ToString() == "2025.04" || sr["curso"].ToString() == "2025.05"
                            )
                        {
                            rdSim.Visible = false;
                            this.divEfetuarMatricula.Visible = false;
                            rdNao.Checked = true;
                        }
                        if (sr["curso"].ToString() == "9999.01" || sr["curso"].ToString() == "9999.02" || sr["curso"].ToString() == "9999.03")
                        {
                            rbAprovarComDep.Visible = false;
                            rbReprovadoNota.Visible = false;
                            rbReprovadoFalta.Visible = false;
                        }
                        if (sr["curso"].ToString() == "0092.30" || sr["curso"].ToString() == "2024.20" )
                        {
                            rbAprovarComDep.Visible = false;                           
                        }

                        if (hdnEletiva.Value == "S")
                        {
                            rdSim.Visible = false;
                            this.divEfetuarMatricula.Visible = false;
                            rdNao.Checked = true;
                        }

                        //Para o curso de Mais Educação ou turma optativa, somente a opçao de Aprovado ficara habilitada
                        if (sr["curso"].ToString() == "9999.92" || sr["curso"].ToString() == "9999.01" 
                            || sr["curso"].ToString() == "9999.03" || sr["curso"].ToString() == "9999.02"
                            || sr["curso"].ToString() == "9999.04" || sr["curso"].ToString() == "9999.91" 
                            || sr["curso"].ToString() =="2025.02" || sr["curso"].ToString() =="2025.03"
                            || sr["curso"].ToString() == "2025.04" || sr["curso"].ToString() =="2025.05"
                            || txtOptativaReforco.Text == "S" || hdnEletiva.Value == "S")
                        {
                            rbAprovar.Checked = true;
                            rbAprovarComDep.Checked = false;
                            rbReprovadoNota.Checked = false;
                            rbReprovadoFalta.Checked = false;
                            rbAprovarComDep.Enabled = false;
                            rbReprovadoNota.Enabled = false;
                            rbReprovadoFalta.Enabled = false;
                            rbPromovido.Enabled = false;
                            rbRetido.Enabled = false;
                            rbRetido.Checked = false;
                            rbPromovido.Checked = false;

                        }

                        //Para o aluno do CEJA, somente poderá ocorrer fechamento com a situação “APROVADO” e a opção ‘NÃO EFETUAR MATRÍCULA
                        if (sr["curso"].ToString() == "0002.37" || sr["curso"].ToString() == "0001.27")
                        {
                            rbAprovar.Checked = true;
                            rbAprovarComDep.Checked = false;
                            rbReprovadoNota.Checked = false;
                            rbReprovadoFalta.Checked = false;
                            rbAprovarComDep.Enabled = false;
                            rbReprovadoNota.Enabled = false;
                            rbReprovadoFalta.Enabled = false;
                            rbPromovido.Enabled = false;
                            rbRetido.Enabled = false;
                            rbRetido.Checked = false;
                            rbPromovido.Checked = false;
                            rdNao.Checked = true;
                            rdSim.Checked = false;
                            rdSim.Visible = false;
                            this.divEfetuarMatricula.Visible = false;
                        }

                        DataTable dtUnidades = new DataTable();

                        dtUnidades = RN.UnidadeEnsino.RetornaUnidadeAbsorvida(Convert.ToString(sr["unidade_responsavel"]));

                        if (dtUnidades.Rows.Count > 0)
                        {
                            ddlUnidadeEnsino.DataSource = dtUnidades;
                            ddlUnidadeEnsino.DataBind();
                        }

                        this.grdDependencia.DataSource = Disciplina.ListarDisciplinasObrigatorias(
                            this.ObjetoTurma.Grade,
                            int.Parse(this.ObjetoTurma.Ano),
                            int.Parse(this.ObjetoTurma.Periodo));
                        this.grdDependencia.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnMarcarTodasDependencia_Click(object sender, EventArgs e)
        {
            this.AlterarSelecaoEmMatriculas(true, grdProgressaoParcial);
        }

        protected void btnDesMarcarTodasDependencia_Click(object sender, EventArgs e)
        {
            this.AlterarSelecaoEmMatriculas(false, grdProgressaoParcial);
        }

        protected void btnMarcarTodasEletiva_Click(object sender, EventArgs e)
        {
            this.AlterarSelecaoEmMatriculas(true, grdEletivas);
        }

        protected void btnDesMarcarTodasEletiva_Click(object sender, EventArgs e)
        {
            this.AlterarSelecaoEmMatriculas(false, grdEletivas);
        }

        protected void btnExecutarDependencia_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var listaAlunoDisciplina = this.ObterAlunosDependencia();
                string alunosSelec = string.Empty;
                string listaAlunos = string.Empty;
                RN.FechamentoMatricula rnFechamentoMatricula = new RN.FechamentoMatricula();
                RN.AulaDocente rnAulaDocente = new AulaDocente();
                bool periodoSimultaneo = Convert.ToBoolean(ConfigurationManager.AppSettings["PeriodoSimultaneo"] ?? "false");
                bool verificaAlocacaoPeriodoSimultaneo = Convert.ToBoolean(ConfigurationManager.AppSettings["VerificaAlocacaoPeriodoSimultaneo"] ?? "false");
                List<string> alunos = listaAlunoDisciplina.Select(x => x[0]).ToList();

                if (alunos.Count <= 0)
                {
                    this.lblMensagem.Text = "Selecione pelo menos um aluno em progressão parcial.";
                    return;
                }

                if (!rbAprovadoDependencia.Checked && !rbReprovadoDependencia.Checked)
                {
                    this.lblMensagem.Text = "Selecione a situação final do(s) aluno(s) selecionado(s) em Progressão Parcial.";
                    return;
                }

                if (periodoSimultaneo && verificaAlocacaoPeriodoSimultaneo)
                {
                    //Caso seja setado para finalizar periodos nao encerrados, valida se existe professor alocado na turma
                    if (rnAulaDocente.ExisteDocentesEmAulaAtivaPor(this.ObjetoTurma.Grade, Convert.ToDecimal(txtAno.Text), Convert.ToDecimal(txtPeriodo.Text)))
                    {
                        this.lblMensagem.Text = "Não é possível realizar o fechamento com docentes ainda alocados no quadro de horários da turma. Por favor, realize a desalocação dos docentes e tente novamente.";
                        return;
                    }
                }
                if (this.rbAprovadoDependencia.Checked)
                {
                    rnFechamentoMatricula.AprovaMatriculaDependenciaAlunos(this.ObjetoTurma, listaAlunoDisciplina, User.Identity.Name);
                }
                else
                {
                    rnFechamentoMatricula.ReprovaMatriculaDependenciaAlunos(this.ObjetoTurma, listaAlunoDisciplina, User.Identity.Name);
                }


                lblMensagem.Text = "Atualização concluída com sucesso.";

                this.grdProgressaoParcial.DataBind();

                rbAprovadoDependencia.Checked = false;
                rbReprovadoDependencia.Checked = false;

                ControlaVisibilidadeProgressao();
                ControlaVisibilidadeHistorico();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnExecutarEletiva_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                var listaAlunoDisciplina = this.ObterAlunosEletivas();
                //string alunosSelec = string.Empty;
                //string listaAlunos = string.Empty;
                RN.FechamentoMatricula rnFechamentoMatricula = new RN.FechamentoMatricula();
                RN.AulaDocente rnAulaDocente = new AulaDocente();
                bool periodoSimultaneo = Convert.ToBoolean(ConfigurationManager.AppSettings["PeriodoSimultaneo"] ?? "false");
                bool verificaAlocacaoPeriodoSimultaneo = Convert.ToBoolean(ConfigurationManager.AppSettings["VerificaAlocacaoPeriodoSimultaneo"] ?? "false");
                List<string> alunos = listaAlunoDisciplina.Select(x => x[0]).ToList();

                if (alunos.Count <= 0)
                {
                    this.lblMensagem.Text = "Selecione pelo menos um aluno em Eletivas.";
                    return;
                }

                if (!rbAprovadoEletiva.Checked)
                {
                    this.lblMensagem.Text = "Selecione a situação final do(s) aluno(s) selecionado(s) em Eletivas.";
                    return;
                }

                if (periodoSimultaneo && verificaAlocacaoPeriodoSimultaneo)
                {
                    //Caso seja setado para finalizar periodos nao encerrados, valida se existe professor alocado na turma
                    if (rnAulaDocente.ExisteDocentesEmAulaAtivaPor(this.ObjetoTurma.Grade, Convert.ToDecimal(txtAno.Text), Convert.ToDecimal(txtPeriodo.Text)))
                    {
                        this.lblMensagem.Text = "Não é possível realizar o fechamento com docentes ainda alocados no quadro de horários da turma. Por favor, realize a desalocação dos docentes e tente novamente.";
                        return;
                    }
                }
                if (this.rbAprovadoEletiva.Checked)
                {
                    rnFechamentoMatricula.AprovaMatriculaEletivaAlunos(this.ObjetoTurma, listaAlunoDisciplina, User.Identity.Name);
                }


                lblMensagem.Text = "Atualização concluída com sucesso.";

                this.grdEletivas.DataBind();
                this.grdHistorico.DataBind();


                ControlaVisibilidadeEletiva();
                ControlaVisibilidadeHistorico();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private List<string[]> ObterAlunosDependencia()
        {
            var listaAlunos = new List<string[]>();
            var alunos = new List<string>();

            for (var i = 0; i < this.grdProgressaoParcial.VisibleRowCount; i++)
            {
                var optSel = this.grdProgressaoParcial.FindRowCellTemplateControl(i, (GridViewDataColumn)this.grdProgressaoParcial.Columns[0], "chkBox") as ASPxCheckBox;

                if (optSel != null
                    && optSel.Checked)
                {
                    var alunoDisciplina = new string[] { this.grdProgressaoParcial.GetRowValues(i, "aluno").ToString(), this.grdProgressaoParcial.GetRowValues(i, "DISCIPLINA").ToString() };
                    listaAlunos.Add(alunoDisciplina);
                }
            }

            return listaAlunos;
        }

        private List<string[]> ObterAlunosEletivas()
        {
            var listaAlunos = new List<string[]>();
            var alunos = new List<string>();

            for (var i = 0; i < this.grdEletivas.VisibleRowCount; i++)
            {
                var optSel = this.grdEletivas.FindRowCellTemplateControl(i, (GridViewDataColumn)this.grdEletivas.Columns[0], "chkBox") as ASPxCheckBox;

                if (optSel != null
                    && optSel.Checked)
                {
                    var alunoDisciplina = new string[] { this.grdEletivas.GetRowValues(i, "aluno").ToString(), this.grdEletivas.GetRowValues(i, "DISCIPLINAORIGINAL").ToString() };
                    listaAlunos.Add(alunoDisciplina);
                }
            }

            return listaAlunos;
        }

    }
}