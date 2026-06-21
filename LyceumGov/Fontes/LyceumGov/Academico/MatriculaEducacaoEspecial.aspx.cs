using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using System.Collections.Generic;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;
using System.Web.UI.HtmlControls;
namespace Techne.Lyceum.Net.Academico
{

    [NavUrl("~/Academico/MatriculaEducacaoEspecial.aspx"),
    ControlText("Enturmação Educação Especial"),
    Title("Enturmação Educação Especial"),]
    public partial class MatriculaEducacaoEspecial : TPage
    {
        #region Código Padrão Techne
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
        #endregion

        #region Propriedades e Enumeradores
        public enum TipoOperacao
        {
            Novo,
            Consultar
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion

        public object ListaSalaRecurso(object aluno, object ano, object semestre, object painel)
        {
            string alunoMatriculado = aluno != null ? aluno.ToString() : string.Empty;
            string anoMatriculado = ano != null ? ano.ToString() : string.Empty;
            string periodoMatriculado = semestre != null ? semestre.ToString() : string.Empty;
            RN.Matricula matricula = new RN.Matricula();
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(alunoMatriculado) && !string.IsNullOrEmpty(anoMatriculado) && !string.IsNullOrEmpty(periodoMatriculado) && Convert.ToBoolean(painel))
                dt = matricula.ListaMatriculaEducacaoEspecialPor(alunoMatriculado, Convert.ToInt32(anoMatriculado), Convert.ToInt32(periodoMatriculado), "9999.91");

            return dt;
        }

        public object ListaAtendEspecializado(object aluno, object ano, object semestre, object painel)
        {
            string alunoMatriculado = aluno != null ? aluno.ToString() : string.Empty;
            string anoMatriculado = ano != null ? ano.ToString() : string.Empty;
            string periodoMatriculado = semestre != null ? semestre.ToString() : string.Empty;
            RN.Matricula matricula = new RN.Matricula();
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(alunoMatriculado) && !string.IsNullOrEmpty(anoMatriculado) && !string.IsNullOrEmpty(periodoMatriculado) && Convert.ToBoolean(painel))
                dt = matricula.ListaMatriculaEducacaoEspecialPor(alunoMatriculado, Convert.ToInt32(anoMatriculado), Convert.ToInt32(periodoMatriculado), "9999.04");

            return dt;
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdDisciplinasAtivas, "Disciplinas");
            TituloGrid(this.grdSalaRecurso, string.Empty);
            TituloGrid(this.grdAtendEspecializado, string.Empty);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            ControlaAcesso(btnSalvarAtendimentoEspecializado, AcaoControle.novo);
            ControlaAcesso(btnSalvarTurmaEducacaoEspecial, AcaoControle.novo);
           

            this.ControlaAcesso(this.grdSalaRecurso);
            this.ControlaAcesso(this.grdAtendEspecializado);

            if (txtTurno.Text != "I")
            {
                ddlTurnoAtendimentoEspecializado.Enabled = false;
            }
            else
            {
                ddlTurnoAtendimentoEspecializado.Enabled = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;
                this.lblMensagem2.Text = string.Empty;

                if (!ddlTurno.Enabled)
                {
                    tseCurso.Mode = ControlMode.View;
                }

                //Desabilita campos, pois ira valer os dados da confirmação 
                DesabilitarCamposMatriculaPrincipal();

                if (!Page.IsPostBack)
                {
                    tseAluno.Enabled = true;

                    RetiraVisibilidadeBotao();
                    _tipoOperacao = TipoOperacao.Consultar;
                    this.ControlarTipoOperacao();

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        String aluno = Request.QueryString["Aluno"];
                        if (!String.IsNullOrEmpty(aluno))
                        {
                            tseAluno.DBValue = aluno;
                        }
                        if (!String.IsNullOrEmpty(Request.QueryString["Chave"]))
                        {
                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                            var alunoChave = decodedText.Substring(decodedText.LastIndexOf('=') + 1);

                            _tipoOperacao = TipoOperacao.Consultar;
                            this.ControlarTipoOperacao();

                            tseAluno.DBValue = alunoChave;
                        }

                        tseAluno_Changed(null, null);
                    }
                }

                CarregaTSearchs();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }


        private void LimparCamposMatriculaPrincipal()
        {
            ddlSemestre.Items.Clear();
            ddlTurma.Items.Clear();
            ddlTurno.Items.Clear();
            ddlSerie.Items.Clear();
            tseCurso.ResetValue();
            ddlTipoCurso.ClearSelection();
            ddlTipoCurso.Visible = false;
            ddlTipoCurso.Enabled = false;
            lblTipoCurso.Visible = false;
            lnkVisualizarDisciplina.Visible = false;
        }

        private void LimparTela()
        {
            txtCurso.Text = string.Empty;
            txtTurno.Text = string.Empty;
            txtCurriculo.Text = string.Empty;
            txtSerie.Text = string.Empty;
            txtUnidadeFisica.Text = string.Empty;
            txtUnidadeFisicaDescr.Text = string.Empty;
            txtFaculdade.Text = string.Empty;
            txtFaculdadeDescr.Text = string.Empty;

            LimparCamposMatriculaPrincipal();
            LimparCamposTurmaAtendimentoEspecializado();
            LimparCamposTurmaEducacaoEspecial();
        }


        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Novo:
                    {
                        tseAluno.Enabled = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;
                        btnNovo.Visible = false;
                        ddlAno.Items.Clear();
                        ddlSemestre.Items.Clear();
                        ddlTurma.Items.Clear();
                        ddlTurno.Items.Clear();
                        ddlSerie.Items.Clear();
                        tseCurso.ResetValue();
                        ddlAno.Enabled = true;
                        ddlSemestre.Enabled = true;
                        ddlTurma.Enabled = true;
                        ddlSerie.Enabled = true;
                        ddlTurno.Enabled = true;
                        tseCurso.Enabled = true;
                        lnkVisualizarDisciplina.Visible = false;
                        CarregaAnoLetivo(ddlAno);

                        break;

                    }
                case TipoOperacao.Consultar:
                    {
                        tseAluno.Enabled = true;

                        ddlAno.Enabled = false;
                        ddlSemestre.Enabled = false;
                        ddlTurma.Enabled = false;
                        ddlSerie.Enabled = false;
                        ddlTurno.Enabled = false;
                        tseCurso.Mode = ControlMode.View;
                        break;
                    }
            }
        }


        private void CarregaTSearchs()
        {
            if (Page.IsCallback)
            {
                return;
            }
        }

        public void CarregaDadosAluno(string aluno)
        {
            RN.Aluno rnAluno = new Aluno();
            DataTable qTable = rnAluno.ObtemDadosAlunoParaMatriculaPor(aluno);
            if (qTable.Rows.Count > 0)
            {
                txtCurso.Text = Convert.ToString(qTable.Rows[0]["CURSO"]);
                txtNomeCurso.Text = Convert.ToString(qTable.Rows[0]["NOMECURSO"]);
                txtTurno.Text = Convert.ToString(qTable.Rows[0]["TURNO"]);
                txtNomeTurno.Text = Convert.ToString(qTable.Rows[0]["NOMETURNO"]);
                txtCurriculo.Text = Convert.ToString(qTable.Rows[0]["CURRICULO"]);
                txtSerie.Text = Convert.ToString(qTable.Rows[0]["SERIE"]);
                txtUnidadeFisica.Text = Convert.ToString(qTable.Rows[0]["UNIDADE_FISICA"]);
                txtUnidadeFisicaDescr.Text = Convert.ToString(qTable.Rows[0]["NOMEUNIDADEFISICA"]);
                txtNucleo.Text = Convert.ToString(qTable.Rows[0]["ID_REGIONAL"]);
                txtDecricaoNucleo.Text = Convert.ToString(qTable.Rows[0]["REGIONAL"]);
                txtFaculdade.Text = Convert.ToString(qTable.Rows[0]["UNIDADE_ENSINO"]);
                txtFaculdadeDescr.Text = Convert.ToString(qTable.Rows[0]["NOMEFACULDADE"]);

                pnAnoPeriodo.Visible = true;

                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();
                this.CarregaMatriculaPrincipal();
            }
        }

        private void CarregaAnoLetivo(DropDownList drop)
        {
            ListItem item = new ListItem("<Nenhum>", string.Empty);

            try
            {
                drop.Items.Clear();
                drop.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.AnoLetivo, RN.PeriodoLetivo.QueryListaAnos);
                drop.DataBind();
                drop.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaCurso()
        {
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;
            coluna.Add("uec.curso");
            coluna.Add("nome");
            coluna.Add("tipo_curso");
            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);
            tseCurso.SqlSelect = sqlSelect;

            if (!string.IsNullOrEmpty(ddlSemestre.SelectedValue))
            {
                table = @" LY_CURSO c 
                           INNER JOIN LY_UNIDADE_ENSINO_CURSOS uec on (c.CURSO = uec.CURSO)";

                sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

                tseCurso.SqlSelect = sqlSelect;
                tseCurso.DataBind();
            }
        }

        private void CarregaGridDisciplinas(ASPxGridView grd)
        {
            DataTable dtDisciplinas = new DataTable();
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
            RN.Disciplina rnDisciplina = new Disciplina();

            try
            {
                if (!tseAluno.DBValue.IsNull && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlSemestre.SelectedValue) && !string.IsNullOrEmpty(ddlTurma.SelectedValue))
                {
                    dtDisciplinas = rnMatricula.ObtemDisciplinasAlunoMatriculadoPor(tseAluno.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlSemestre.SelectedValue), ddlTurma.SelectedValue);

                    if (dtDisciplinas.Rows.Count == 0)
                    {
                        dtDisciplinas = rnDisciplina.ObtemDisciplinasTurmaPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlSemestre.SelectedValue), ddlTurma.SelectedValue);
                    }
                }

                grd.DataSource = dtDisciplinas;
                grd.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaMatriculaPrincipal()
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
            DadosEnturmacaoAluno enturmacaoEducEspecial = new DadosEnturmacaoAluno();
            RN.Turno rnTurno = new Turno();
            bool possuiMatriculaPrincial = false;

            try
            {
                LimparCamposMatriculaPrincipal();
                pnlTurmaSalaRecurso.Visible = false;
                PnlAtendimentoEspecializado.Visible = false;

                if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue || tseAluno.ValidateKey(tseAluno.DBValue.ToString())))
                {
                    //Carrega dados na matricula principal
                    enturmacaoEducEspecial = rnMatricula.ObtemMatriculaPrincipalAtivaPor(tseAluno.DBValue.ToString());
                    possuiMatriculaPrincial = !string.IsNullOrEmpty(enturmacaoEducEspecial.Aluno);


                    if (possuiMatriculaPrincial)
                    {
                        //Para opção de Consulta, adiciona nos combos apenas a opção correta                        
                        ddlAno.Items.Add(enturmacaoEducEspecial.Ano.ToString());
                        ddlAno.SelectedValue = enturmacaoEducEspecial.Ano.ToString();

                        ddlSemestre.Items.Add(enturmacaoEducEspecial.Periodo.ToString());
                        ddlSemestre.SelectedValue = enturmacaoEducEspecial.Periodo.ToString();
                        CarregaCurso();

                        tseCurso.DBValue = enturmacaoEducEspecial.Curso;

                        ddlTurno.Items.Add(rnTurno.RetornaDescricaoTurno(enturmacaoEducEspecial.Turno));
                        ddlTurno.SelectedValue = rnTurno.RetornaDescricaoTurno(enturmacaoEducEspecial.Turno);

                        ddlSerie.Items.Add(enturmacaoEducEspecial.Serie.ToString());
                        ddlSerie.SelectedValue = enturmacaoEducEspecial.Serie.ToString();

                        ddlTurma.Items.Add(enturmacaoEducEspecial.Turma);
                        ddlTurma.SelectedValue = enturmacaoEducEspecial.Turma.ToString();

                       ControlaVisibilidadeAtendimentoEspecializado();
                        ControlaVisibilidadeSalaRecurso();
                    }
                    else
                    {
                        lblMensagem.Text = "Não existe enturmação para este aluno(a). Para enturmar clique no botão NOVO.";
                        lblMensagem2.Text = lblMensagem.Text;
                        ddlTurma.Items.Clear();
                        ddlAno.Items.Clear();
                        ddlSemestre.Items.Clear();
                    }

                }
                else
                {
                    lblMensagem.Text = "É necessário selecionar um aluno para efetuar a matrícula.";
                    lblMensagem2.Text = lblMensagem.Text;
                    LimparTela();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        private void CarregaPeriodoLetivo(DropDownList drop, int ano)
        {
            PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
            try
            {
                drop.Items.Clear();

                if (ano != 0)
                {
                    drop.DataSource = rnPeriodoLetivo.ListaPeriodosletivosPor(ano);
                    drop.DataBind();
                    drop.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaTurno(string unidade, string curso)
        {
            RN.Turno rnTurno = new Turno();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
            try
            {
                ddlTurno.Items.Clear();
                ddlTurno.DataSource = rnTurno.ListaTurnoPor(unidade, curso);
                ddlTurno.DataBind();
                ddlTurno.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaSerie(string curso, string turno)
        {
            RN.Serie rnSerie = new Serie();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
            try
            {
                ddlSerie.Items.Clear();
                ddlSerie.DataSource = rnSerie.ListaSeriePor(curso, turno);
                ddlSerie.DataBind();
                ddlSerie.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void grdDisciplinasAtivas_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGridDisciplinas(grdDisciplinasAtivas);
        }


        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
                {
                    _tipoOperacao = TipoOperacao.Novo;
                    this.ControlarTipoOperacao();
                    lblMensagem.Text = string.Empty;
                    lblMensagem2.Text = lblMensagem.Text;

                }
                else
                {
                    lblMensagem.Text = "É necessário selecionar um aluno para efetuar a matrícula.";
                    lblMensagem2.Text = lblMensagem.Text;
                    LimparTela();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            RN.Matricula rnMatricula = new RN.Matricula();
            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;
            try
            {
                validacao = rnMatricula.ValidaRemocaoMatriculaPrincipal(tseAluno.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlSemestre.SelectedValue), ddlTurma.SelectedValue);
                if (validacao.Valido)
                {
                    rnMatricula.RemoveMatricula(tseAluno.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlSemestre.SelectedValue), ddlTurma.SelectedValue);

                    lblMensagem.Text = "Matrícula excluída com sucesso.";

                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();

                    //Quando Excluir Matricula principal tudo deve ser retirado da tela
                    pnlTurmaSalaRecurso.Visible = false;
                    PnlAtendimentoEspecializado.Visible = false;
                    
                    btnNovo.Visible = true;
                    ddlSemestre.Items.Clear();
                    ddlTurma.Items.Clear();
                    lnkVisualizarDisciplina.Visible = false;
                    ddlTurno.Items.Clear();
                    ddlSerie.Items.Clear();
                    tseCurso.ResetValue();
                    ddlTipoCurso.ClearSelection();
                    ddlTipoCurso.Visible = false;
                    ddlTipoCurso.Enabled = false;
                    lblTipoCurso.Visible = false;
                }
                else
                {
                    lblMensagem.Text = "Não foi possível excluir a matrícula.\n" + validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Consultar;
            ControlarTipoOperacao();
            btnCancel.Visible = false;
            btnSalvar.Visible = false;
            CarregaMatriculaPrincipal();
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var mensagem = string.Empty;
                RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
                ValidacaoDados validacao = new ValidacaoDados();
                string tipoCurso = string.Empty;
                RN.GradeTurma rnGradeTurma = new GradeTurma();

                LyMatricula matricula = new LyMatricula()
                {
                    Ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? Convert.ToDecimal(ddlAno.SelectedValue) : -1,
                    Semestre = !string.IsNullOrEmpty(ddlSemestre.SelectedValue) ? Convert.ToDecimal(ddlSemestre.SelectedValue) : -1,
                    SitMatricula = RN.Matricula.Matriculado,
                    Turma = ddlTurma.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString(),
                    Disciplina = string.Empty,
                    CobrancaSep = "N"
                };

                LyTurma turma = new LyTurma()
                {
                    Ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? Convert.ToDecimal(ddlAno.SelectedValue) : -1,
                    Semestre = !string.IsNullOrEmpty(ddlSemestre.SelectedValue) ? Convert.ToDecimal(ddlSemestre.SelectedValue) : -1,
                    Curso = !tseCurso.DBValue.IsNull ? tseCurso.DBValue.ToString() : string.Empty,
                    Serie = !string.IsNullOrEmpty(ddlSerie.SelectedValue) ? Convert.ToDecimal(ddlSerie.SelectedValue) : -1,
                    Turno = !string.IsNullOrEmpty(ddlTurno.SelectedValue) ? ddlTurno.SelectedValue : string.Empty,
                    Turma = !string.IsNullOrEmpty(ddlTurma.SelectedValue) ? ddlTurma.SelectedValue : string.Empty,
                    Faculdade = txtFaculdade.Text
                };

                if (!tseCurso.DBValue.IsNull)
                {
                    tipoCurso = tseCurso["TIPO_CURSO"].ToString();
                }

                validacao = rnMatricula.ValidaMatriculaPrincipal(matricula, turma, tipoCurso, ddlTipoCurso.SelectedValue, tseAluno["necessidade_especial"].ToString());

                if (validacao.Valido)
                {
                    //Busca gradeId para enturmação
                    decimal gradeId = Convert.ToDecimal(rnGradeTurma.ObterGradeId(matricula.Ano, matricula.Semestre, matricula.Turma));

                    if (gradeId == 0)
                    {
                        lblMensagem.Text = "GradeId não encontrado. Verifique!";
                        return;
                    }

                    rnMatricula.InsereMatriculaPrincipal(matricula, ddlTipoCurso.SelectedValue, gradeId);

                    lblMensagem.Text = "Matrícula incluída com sucesso.";

                    var periodoConc = rnMatricula.ObtemPeriodoMatriculaConcomitantePor(int.Parse(ddlAno.SelectedValue), tseAluno.DBValue.ToString());

                    this.ControlaVisibilidadeSalaRecurso();
                    this.ControlaVisibilidadeAtendimentoEspecializado();
                    btnSalvar.Visible = false;
                    btnCancel.Visible = false;
                    btnNovo.Visible = false;
                    tseAluno.Enabled = true;
                    ddlAno.Enabled = false;
                    ddlSemestre.Enabled = false;
                    ddlTurma.Enabled = false;
                    ddlSerie.Enabled = false;
                    ddlTurno.Enabled = false;
                    tseCurso.Mode = ControlMode.View;
                    tseCurso.Enabled = false;
                    lnkVisualizarDisciplina.Visible = false;
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }


        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSemestre.Items.Clear();
                tseCurso.ResetValue();
                ddlTurno.Items.Clear();
                ddlSerie.Items.Clear();
                ddlTurma.Items.Clear();
                lnkVisualizarDisciplina.Visible = false;
                if (_tipoOperacao == TipoOperacao.Novo && !string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    tseAluno.Enabled = false;
                    CarregaPeriodoLetivo(ddlSemestre, Convert.ToInt32(ddlAno.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ddlSemestre_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseCurso.ResetValue();
                ddlTurno.Items.Clear();
                ddlSerie.Items.Clear();
                ddlTurma.Items.Clear();
                lnkVisualizarDisciplina.Visible = false;
                CarregaCurso();
            }
            catch (Exception EX)
            {
                lblMensagem.Text = EX.Message;
            }
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Turma rnTurma = new RN.Turma();
            DataTable dtTurmas = new DataTable();
            try
            {
                ddlTurma.Items.Clear();
                lnkVisualizarDisciplina.Visible = false;

                if (_tipoOperacao == TipoOperacao.Novo && !string.IsNullOrEmpty(ddlSerie.SelectedValue))
                {
                    tseAluno.Enabled = false;
                    dtTurmas = Turma.ConsultarPrimeiraTurmaDisponivel(
                        Convert.ToDecimal(this.ddlAno.SelectedValue),
                        Convert.ToDecimal(this.ddlSemestre.SelectedValue),
                        txtFaculdade.Text,
                        ddlTurno.SelectedValue,
                        tseCurso.DBValue.ToString(),
                        Convert.ToDecimal(ddlSerie.SelectedValue));

                    if (dtTurmas.Rows.Count > 0)
                    {
                        ListItem item = new ListItem("<Nenhum>", string.Empty);

                        ddlTurma.DataSource = dtTurmas;
                        ddlTurma.DataBind();
                        ddlTurma.Items.Insert(0, item);
                    }
                    else
                    {
                        lblMensagem.Text = "Não foram encontradas turmas com vagas para este ano / periodo.";
                        lblMensagem2.Text = lblMensagem.Text;
                    }
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
                lblTipoCurso.Visible = false;
                ddlTipoCurso.Visible = false;
                ddlTipoCurso.Enabled = false;
                ddlTipoCurso.ClearSelection();

                if (_tipoOperacao == TipoOperacao.Novo && !string.IsNullOrEmpty(((DropDownList)sender).SelectedValue))
                {
                    tseAluno.Enabled = false;

                    if (tseCurso["tipo_curso"].ToString() == "Concomitante/Subsequente")
                    {
                        lblTipoCurso.Visible = true;
                        ddlTipoCurso.Visible = true;
                        ddlTipoCurso.Enabled = true;
                    }
                }
                lnkVisualizarDisciplina.Visible = !string.IsNullOrEmpty(ddlTurma.SelectedValue);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Serie rnSerie = new Serie();
            try
            {
                ddlSerie.Items.Clear();
                ddlTurma.Items.Clear();
                lnkVisualizarDisciplina.Visible = false;

                if (!string.IsNullOrEmpty(tseCurso.DBValue.ToString()) && !string.IsNullOrEmpty(ddlTurno.SelectedValue))
                {
                    CarregaSerie(tseCurso.DBValue.ToString(), ddlTurno.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurnoAtendimentoEspecializado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {             
                ddlTurmaAtendimentoEspecializado.Items.Clear();

                if (!string.IsNullOrEmpty(ddlTurnoAtendimentoEspecializado.SelectedValue))
                {
                    CarregaTurmaAtendEspecializado();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }



        private void DesabilitarCamposMatriculaPrincipal()
        {
            ddlTurno.Enabled = false;
            ddlSerie.Enabled = false;
            tseCurso.Mode = ControlMode.View;
            ddlTipoCurso.Enabled = false;
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;            
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }


        protected void lnkVisualizarDisciplina_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlTurma.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupDisciplina();", true);

                    CarregaGridDisciplinas(grdDisciplinasAtivas);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ControlaVisibilidadeAtendimentoEspecializado()
        {
            this.LimparCamposTurmaAtendimentoEspecializado();
            ddlTurnoAtendimentoEspecializado.Enabled = false;

            lblAnoAtendimentoEspecializado.Text = ddlAno.SelectedValue;
            lblPeriodoAtendimentoEspecializado.Text = ddlSemestre.SelectedValue;

            if (!string.IsNullOrEmpty(ddlSemestre.SelectedValue) && !string.IsNullOrEmpty(lblAnoAtendimentoEspecializado.Text))
            {
                RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();

                PnlAtendimentoEspecializado.Visible = rnAvaliacaoNapes.PossuiRecursoPor(Convert.ToString(tseAluno.DBValue), 5);

                if (PnlAtendimentoEspecializado.Visible)
                {
                    lblUnidadeEnsinoAtendimentoEspecializado.Text = txtFaculdade.Text;
                    lblNomeUnidadeEnsinoAtendimentoEspecializado.Text = txtFaculdadeDescr.Text;
                    CarregaTurnoAtendimentoEspecializado(lblUnidadeEnsinoAtendimentoEspecializado.Text, "9999.04");

                    if (txtTurno.Text != "I")
                    {
                        ddlTurnoAtendimentoEspecializado.SelectedValue = txtTurno.Text;
                        ddlTurnoAtendimentoEspecializado.Enabled = false;
                    }
                    else
                    {
                        ddlTurnoAtendimentoEspecializado.Enabled = true;
                    }

                    if (!string.IsNullOrEmpty(ddlTurnoAtendimentoEspecializado.SelectedValue))
                    {
                        CarregaTurmaAtendEspecializado();
                    }
                }
            }
        }

        protected void ControlaVisibilidadeTurmaEducacaoEspecial()
        {
            this.LimparCamposTurmaEducacaoEspecial();

            lblAnoEducacaoEspecial.Text = ddlAno.SelectedValue;
            lblPeriodoEducacaoEspecial.Text = ddlSemestre.SelectedValue;

            if (!string.IsNullOrEmpty(ddlSemestre.SelectedValue) && !string.IsNullOrEmpty(lblAnoEducacaoEspecial.Text))
            {
                RN.DTOs.DadosEducacaoEspecial educacaoEspecial = RN.Matricula.CarregarDadosSalaRecurso(Convert.ToString(tseAluno.DBValue), int.Parse(lblAnoEducacaoEspecial.Text),
                                                         int.Parse(ddlSemestre.SelectedValue), User.Identity.Name);

                RN.NecessidadeEspecial.AvaliacaoNapes rnAvaliacaoNapes = new Techne.Lyceum.RN.NecessidadeEspecial.AvaliacaoNapes();

                pnlTurmaSalaRecurso.Visible = rnAvaliacaoNapes.PossuiRecursoPor(Convert.ToString(tseAluno.DBValue), 4);

                if (pnlTurmaSalaRecurso.Visible)
                {
                    if (educacaoEspecial.Enturmado)
                    {

                        CarregaSerieEducEspecial("9999.91", ddlTurnoEducEspecial.SelectedValue);
                    }
                    this.ControleAcessoTurmaEducacaoEspecial(educacaoEspecial);
                }
            }
        }

        protected void ControlaVisibilidadeSalaRecurso()
        {
            this.LimparCamposTurmaEducacaoEspecial();

            lblAnoEducacaoEspecial.Text = ddlAno.SelectedValue;
            lblPeriodoEducacaoEspecial.Text = ddlSemestre.SelectedValue;

            if (!string.IsNullOrEmpty(ddlSemestre.SelectedValue) && !string.IsNullOrEmpty(ddlAno.SelectedValue))
            {
                RN.DTOs.DadosEducacaoEspecial educacaoEspecial = RN.Matricula.CarregarDadosSalaRecurso(Convert.ToString(tseAluno.DBValue), int.Parse(lblAnoEducacaoEspecial.Text),
                                                         int.Parse(ddlSemestre.SelectedValue), User.Identity.Name);

                pnlTurmaSalaRecurso.Visible = educacaoEspecial.Aceite;

                if (pnlTurmaSalaRecurso.Visible)               {
                

                    this.ControleAcessoSalaRecurso(educacaoEspecial);
                }
            }
        }





        private void ControleAcessoAtendimentoEspecializado(RN.DTOs.DadosEducacaoEspecial educacaoEspecial)
        {
            if (RN.Usuarios.VerificaAcesso(educacaoEspecial.Censo, User.Identity.Name))
            {
                btnSalvarAtendimentoEspecializado.Visible = !educacaoEspecial.Enturmado;
                ddlTurnoAtendimentoEspecializado.Enabled = !educacaoEspecial.Enturmado;
                ddlTurmaAtendimentoEspecializado.Enabled = !educacaoEspecial.Enturmado;
            }
            else
            {
                btnSalvarAtendimentoEspecializado.Visible = false;
                ddlTurnoAtendimentoEspecializado.Enabled = false;
                ddlTurmaAtendimentoEspecializado.Enabled = false;
            }
        }

        private void ControleAcessoSalaRecurso(RN.DTOs.DadosEducacaoEspecial educacaoEspecial)
        {
            if (RN.Usuarios.VerificaAcesso(educacaoEspecial.Censo, User.Identity.Name))
            {
                btnSalvarTurmaEducacaoEspecial.Visible = !educacaoEspecial.Enturmado;
                ddlTurnoEducEspecial.Enabled = !educacaoEspecial.Enturmado;
                ddlTurmaEducEspecial.Enabled = !educacaoEspecial.Enturmado;
            }
            else
            {
                btnSalvarTurmaEducacaoEspecial.Visible = false;
                ddlTurnoEducEspecial.Enabled = false;
                ddlTurmaEducEspecial.Enabled = false;
            }
        }

        private void ControleAcessoTurmaEducacaoEspecial(RN.DTOs.DadosEducacaoEspecial educacaoEspecial)
        {
            if (RN.Usuarios.VerificaAcesso(educacaoEspecial.Censo, User.Identity.Name))
            {
                btnSalvarTurmaEducacaoEspecial.Visible = !educacaoEspecial.Enturmado;
                ddlTurnoEducEspecial.Enabled = !educacaoEspecial.Enturmado;
                ddlTurmaEducEspecial.Enabled = !educacaoEspecial.Enturmado;
            }
            else
            {
                btnSalvarTurmaEducacaoEspecial.Visible = false;
                ddlTurnoEducEspecial.Enabled = false;
                ddlTurmaEducEspecial.Enabled = false;
            }
        }


        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }

            try
            {
                RN.Aluno rnAluno = new RN.Aluno();
                tseAluno.Enabled = true;
                lblMensagem.Text = string.Empty;
                lblMensagem2.Text = string.Empty;

                LimparTela();
                RetiraVisibilidadeBotao();

                var alunoChave = string.Empty;
                bool alunoParamValido = false;

                if (!String.IsNullOrEmpty(Request.QueryString["Chave"]))
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    alunoChave = decodedText.Substring(decodedText.LastIndexOf('=') + 1);
                }
                else
                {
                    alunoChave = Request.QueryString["Aluno"];
                }

                alunoParamValido = tseAluno.ValidateKey(alunoChave);

                if ((!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue) || alunoParamValido)
                {
                    pnSerieTurma.Visible = true;
                    lblMensagem.Text = string.Empty;
                    lblMensagem2.Text = string.Empty;

                    String alunoQueryString = Request.QueryString["Aluno"];
                    string aluno = tseAluno.IsValidDBValue ? Convert.ToString(tseAluno.DBValue) : alunoChave;

                    bool alunoAtivo = rnAluno.EhAlunoAtivoPor(aluno);

                    if (alunoAtivo)
                    {
                        CarregaDadosAluno(aluno);
                    }
                    else
                    {
                        pnAnoPeriodo.Visible = false;
                        pnSerieTurma.Visible = false;
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        lblMensagem2.Text = lblMensagem.Text;
                        btnNovo.Visible = false;                        
                        this.LimparTela();
                    }
                }
                else if (!tseAluno.DBValue.IsNull)
                {
                    pnAnoPeriodo.Visible = false;
                    pnSerieTurma.Visible = false;
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    lblMensagem2.Text = lblMensagem.Text;
                    btnNovo.Visible = false;
                    pnlTurmaSalaRecurso.Visible = false;
                    PnlAtendimentoEspecializado.Visible = false;
                }
                else
                {
                    pnAnoPeriodo.Visible = false;
                    pnSerieTurma.Visible = false;
                    lblMensagem.Text = "Favor consultar um aluno.";
                    lblMensagem2.Text = lblMensagem.Text;
                    btnNovo.Visible = false;
                    pnlTurmaSalaRecurso.Visible = false;
                    PnlAtendimentoEspecializado.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = lblMensagem.Text;
            }
        }

        protected void tseCurso_Changed(object sender, EventArgs args)
        {
            try
            {
                ddlTurno.Items.Clear();
                ddlSerie.Items.Clear();
                ddlTurma.Items.Clear();
                lnkVisualizarDisciplina.Visible = false;

                if (this.tseCurso.IsValidDBValue && !this.tseCurso.DBValue.IsNull)
                {
                    if (!string.IsNullOrEmpty(txtFaculdade.Text) && !string.IsNullOrEmpty(tseCurso.DBValue.ToString()))
                    {
                        CarregaTurno(txtFaculdade.Text, tseCurso.DBValue.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsinoSalaRecurso_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                ddlTurnoEducEspecial.Items.Clear();
                ddlSerieEducEspecial.Items.Clear();
                ddlTurmaEducEspecial.Items.Clear();
                RN.DTOs.DadosEducacaoEspecial educacaoEspecial = new DadosEducacaoEspecial();

                if (this.tseUnidadeEnsinoSalaRecurso.IsValidDBValue && !this.tseUnidadeEnsinoSalaRecurso.DBValue.IsNull)
                {
                    tseRegionalSalaRecurso.Value = tseUnidadeEnsinoSalaRecurso["id_regional"];
                    educacaoEspecial.Censo = tseUnidadeEnsinoSalaRecurso.DBValue.ToString();
                    this.ControleAcessoSalaRecurso(educacaoEspecial);

                    CarregaTurnoEducEspecial(tseUnidadeEnsinoSalaRecurso.DBValue.ToString(), "9999.91");

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegionalSalaRecurso_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;

                tseUnidadeEnsinoSalaRecurso.ResetValue();
                ddlTurnoEducEspecial.Items.Clear();
                ddlSerieEducEspecial.Items.Clear();
                ddlTurmaEducEspecial.Items.Clear();


                if (this.tseRegionalSalaRecurso.IsValidDBValue && !this.tseRegionalSalaRecurso.DBValue.IsNull)
                {


                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void Delete(object ALUNO, object DISCIPLINA, object TURMA, object ANO, object SEMESTRE)
        {
        }

        protected void ddlTurmaAtendimentoEspecializado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.AulaDocente rnAulaDocente = new AulaDocente();

                lblProfessorAtendimento.Text = string.Empty;

                if (!ddlTurmaAtendimentoEspecializado.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    string nome = rnAulaDocente.RetornaPrimeiroDocentesEmAulaPor(ddlTurmaAtendimentoEspecializado.SelectedValue, Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(lblPeriodoAtendimentoEspecializado.Text));

                    if (nome.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblProfessorAtendimento.Text = "Não há professor alocado para esta turma";
                    }

                    else
                    {
                        lblProfessorAtendimento.Text = nome;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }



        #region "Educacao Especial"

        private void LimparCamposTurmaAtendimentoEspecializado()
        {
            ddlTurmaAtendimentoEspecializado.ClearSelection();
            lblProfessorAtendimento.Text = string.Empty;
        }

        private void LimparCamposTurmaEducacaoEspecial()
        {
            tseUnidadeEnsinoSalaRecurso.ResetValue();
            tseRegionalSalaRecurso.ResetValue();
            ddlTurnoEducEspecial.Items.Clear();
            ddlSerieEducEspecial.Items.Clear();
            ddlTurmaEducEspecial.Items.Clear();
        }

        private void CarregaSerieEducEspecial(string curso, string turno)
        {
            Serie rnSerie = new Serie();
            ListItem item = new ListItem("Selecione", string.Empty);

            try
            {
                ddlSerieEducEspecial.Items.Clear();
                ddlSerieEducEspecial.DataSource = rnSerie.ListaSeriePor(curso, turno);
                ddlSerieEducEspecial.DataBind();
                ddlSerieEducEspecial.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        private void CarregaTurnoAtendimentoEspecializado(string unidade, string curso)
        {
            RN.Turno rnTurno = new Turno();
            ListItem item = new ListItem("Selecione", string.Empty);
            try
            {
                ddlTurnoAtendimentoEspecializado.Items.Clear();
                ddlTurnoAtendimentoEspecializado.DataSource = rnTurno.ListaTurnosContraTurnoPor(unidade, Convert.ToInt32(ddlAno.SelectedValue), txtNomeTurno.Text);
                ddlTurnoAtendimentoEspecializado.DataBind();
                ddlTurnoAtendimentoEspecializado.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaTurnoEducEspecial(string unidade, string curso)
        {
            Turno rnTurnoEducEspecial = new Turno();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
            try
            {
                ddlTurnoEducEspecial.Items.Clear();
                ddlTurnoEducEspecial.DataSource = rnTurnoEducEspecial.ListaTurnoEducEspecialPor(unidade, curso);
                ddlTurnoEducEspecial.DataBind();
                ddlTurnoEducEspecial.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }       

        protected void CarregaTurmaAtendEspecializado()
        {
            RN.Turma rnTurma = new Turma();
            ddlTurmaAtendimentoEspecializado.Items.Clear();

            if (!string.IsNullOrEmpty(ddlTurnoAtendimentoEspecializado.SelectedValue))
            {
                ListItem item = new ListItem("Selecione", string.Empty);
                ddlTurmaAtendimentoEspecializado.DataSource = rnTurma.ListaTurmasGradeComVagasPor(ddlAno.SelectedValue,
                    ddlSemestre.SelectedValue, lblUnidadeEnsinoAtendimentoEspecializado.Text.Trim(), "9999.04",
                    ddlTurnoAtendimentoEspecializado.SelectedValue, 1);
                ddlTurmaAtendimentoEspecializado.DataBind();
                ddlTurmaAtendimentoEspecializado.Items.Insert(0, item);
            }
        }


        protected void ddlPeriodoAtendimentoEspecializado_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTurnoAtendimentoEspecializado.Items.Clear();
            ddlTurmaAtendimentoEspecializado.Items.Clear();

            try
            {
                CarregaTurnoAtendimentoEspecializado(lblUnidadeEnsinoAtendimentoEspecializado.Text, "9999.04");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ddlSerieEducEspecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Turma rnTurma = new Turma();
            DataTable dtTurmas = new DataTable();

            try
            {
                ddlTurmaEducEspecial.Items.Clear();

                if (!string.IsNullOrEmpty(ddlSerieEducEspecial.SelectedValue))
                {
                    ListItem item = new ListItem("Selecione", string.Empty);
                    decimal serie = string.IsNullOrEmpty(ddlSerieEducEspecial.SelectedValue) ? -1 : Convert.ToDecimal(ddlSerieEducEspecial.SelectedValue);

                    ddlTurmaEducEspecial.DataSource = rnTurma.ListaTurmasGradeComVagasPor(ddlAno.SelectedValue, ddlSemestre.SelectedValue, tseUnidadeEnsinoSalaRecurso.DBValue.ToString(), "9999.91", ddlTurnoEducEspecial.SelectedValue, serie);
                    ddlTurmaEducEspecial.DataBind();
                    ddlTurmaEducEspecial.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ddlTurnoEducEspecial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSerieEducEspecial.Items.Clear();
                ddlTurmaEducEspecial.Items.Clear();

                if (!string.IsNullOrEmpty(ddlTurnoEducEspecial.SelectedValue))
                {
                    CarregaSerieEducEspecial("9999.91", ddlTurnoEducEspecial.SelectedValue);
                }
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }


        protected void btnExcluirAtendimentoEspecializado_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            lblMensagem2.Text = lblMensagem.Text;

            try
            {
                LyMatricula lyMatricula = new LyMatricula()
                {
                    Ano = decimal.Parse(lblAnoAtendimentoEspecializado.Text),                  
                    Turma = ddlTurmaAtendimentoEspecializado.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                var retorno = RN.Matricula.ValidarRemoverEducacaoEspecial(lyMatricula);

                if (retorno.Valido)
                {
                    RN.Matricula.RemoverEducacaoEspecial(lyMatricula);

                    ddlTurnoAtendimentoEspecializado.Items.Clear();
                    ddlTurmaAtendimentoEspecializado.Items.Clear();

                    btnSalvarAtendimentoEspecializado.Visible = true;
                    ddlTurnoAtendimentoEspecializado.Enabled = true;
                    ddlTurmaAtendimentoEspecializado.Enabled = true;

                    lblMensagem.Text = "Exclusão de turma realizada com sucesso.";
                }
                else
                {
                    lblMensagem.Text = retorno.Mensagem;
                    lblMensagem2.Text = lblMensagem.Text;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = lblMensagem.Text;
            }
        }

        protected void btnExcluirTurmaEducacaoEspecial_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            lblMensagem2.Text = lblMensagem.Text;

            try
            {
                LyMatricula lyMatricula = new LyMatricula()
                {
                    Ano = decimal.Parse(lblAnoEducacaoEspecial.Text),
                    Semestre = decimal.Parse(lblPeriodoEducacaoEspecial.Text),
                    Turma = ddlTurmaEducEspecial.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                var retorno = RN.Matricula.ValidarRemoverEducacaoEspecial(lyMatricula);

                if (retorno.Valido)
                {
                    RN.Matricula.RemoverEducacaoEspecial(lyMatricula);
                    ddlTurnoEducEspecial.Items.Clear();
                    ddlTurmaEducEspecial.Items.Clear();
                    btnSalvarTurmaEducacaoEspecial.Visible = true;
                    ddlTurnoEducEspecial.Enabled = true;
                    ddlTurmaEducEspecial.Enabled = true;

                    lblMensagem.Text = "Exclusão de turma realizada com sucesso.";
                }
                else
                {
                    lblMensagem.Text = retorno.Mensagem;
                    lblMensagem2.Text = lblMensagem.Text;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = lblMensagem.Text;
            }
        }

        protected void btnSalvarAtendimentoEspecializado_Click(object sender, EventArgs e)
        {
            try
            {
                LyMatricula lyMatricula = new LyMatricula()
                {
                    Ano = decimal.Parse(lblAnoAtendimentoEspecializado.Text),
                    Semestre = decimal.Parse(ddlSemestre.SelectedValue),
                    Turma = ddlTurmaAtendimentoEspecializado.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                LyMatricula turmaRegular = new LyMatricula()
                {
                    Ano = decimal.Parse(ddlAno.SelectedValue),
                    Semestre = decimal.Parse(ddlSemestre.SelectedValue),
                    Turma = ddlTurma.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                var retorno = RN.Matricula.ValidarEducacaoEspecial(lyMatricula, turmaRegular, null, "9999.04");

                if (retorno.Valido)
                {
                    RN.Matricula.InserirSalaRecurso(lyMatricula);

                    odsAtendEspecializado.Select();
                    odsAtendEspecializado.DataBind();
                    grdAtendEspecializado.DataBind();

                    LimparCamposTurmaAtendimentoEspecializado();

                    lblMensagem.Text = "Enturmação Atendimento Especializado realizada com sucesso.";

                }
                else
                {
                    lblMensagem.Text = retorno.Mensagem;
                    lblMensagem2.Text = lblMensagem.Text;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = lblMensagem.Text;
            }
        }

        protected void btnSalvarTurmaEducacaoEspecial_Click(object sender, EventArgs e)
        {
            try
            {
                LyMatricula lyMatricula = new LyMatricula()
                {
                    Ano = decimal.Parse(lblAnoEducacaoEspecial.Text),
                    Semestre = decimal.Parse(lblPeriodoEducacaoEspecial.Text),
                    Turma = ddlTurmaEducEspecial.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                LyMatricula turmaRegular = new LyMatricula()
                {
                    Ano = decimal.Parse(ddlAno.SelectedValue),
                    Semestre = decimal.Parse(ddlSemestre.SelectedValue),
                    Turma = ddlTurma.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                var retorno = RN.Matricula.ValidarEducacaoEspecial(lyMatricula, turmaRegular, null, "9999.91");

                if (retorno.Valido)
                {
                    RN.Matricula.InserirSalaRecurso(lyMatricula);

                    odsSalaRecurso.Select();
                    odsSalaRecurso.DataBind();
                    grdSalaRecurso.DataBind();

                    LimparCamposTurmaEducacaoEspecial();

                    lblMensagem.Text = "Enturmação Sala de Recurso realizada com sucesso.";
                }
                else
                {
                    lblMensagem.Text = retorno.Mensagem;
                    lblMensagem2.Text = lblMensagem.Text;
                }

                btnSalvarTurmaEducacaoEspecial.Visible = !retorno.Valido;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = lblMensagem.Text;
            }
        }


        protected void grdAtendEspecializado_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAtendEspecializado);
        }

        protected void grdAtendEspecializado_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var situacao = (string)grdAtendEspecializado.GetRowValues(e.VisibleIndex, "SIT_MATRICULA");

            if (!string.IsNullOrEmpty(situacao)
                && situacao == "Cancelado")
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }
        }

        protected void grdSalaRecurso_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdSalaRecurso);
        }

        protected void grdSalaRecurso_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var situacao = (string)grdSalaRecurso.GetRowValues(e.VisibleIndex, "SIT_MATRICULA");

            if (!string.IsNullOrEmpty(situacao)
                && situacao == "Cancelado")
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }
            }
        }


        protected void odsAtendEspecializado_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var matricula = new LyMatricula
            {
                Aluno = e.InputParameters["ALUNO"].ToString(),
                Turma = e.InputParameters["TURMA"].ToString(),
                Ano = Convert.ToDecimal(e.InputParameters["ANO"]),
                Semestre = Convert.ToDecimal(e.InputParameters["SEMESTRE"]),
                Matricula = this.User.Identity.Name
            };

            RN.Matricula.RemoverSalaRecurso(matricula);
        }

        protected void odsSalaRecurso_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var matricula = new LyMatricula
            {
                Aluno = e.InputParameters["ALUNO"].ToString(),
                Turma = e.InputParameters["TURMA"].ToString(),
                Ano = Convert.ToDecimal(e.InputParameters["ANO"]),
                Semestre = Convert.ToDecimal(e.InputParameters["SEMESTRE"]),
                Matricula = this.User.Identity.Name
            };

            RN.Matricula.RemoverSalaRecurso(matricula);
        }


        public void DeleteAtendEspecializado(object ALUNO, object TURMA, object ANO, object SEMESTRE, object SIT_MATRICULA)
        {
        }

        public void DeleteSalaRecurso(object ALUNO, object TURMA, object ANO, object SEMESTRE, object SIT_MATRICULA)
        {
        }

        #endregion
    }
}