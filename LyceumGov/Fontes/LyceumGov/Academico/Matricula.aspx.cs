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

    [NavUrl("~/Academico/Matricula.aspx"),
    ControlText("Enturmação"),
    Title("Enturmação"),]
    public partial class Matricula : TPage
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

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdDisciplinasAtivas, "Disciplinas");
            TituloGrid(this.grdProgressao, string.Empty);
            TituloGrid(this.grdEletivas, string.Empty);
            TituloGrid(this.grdOptativaReforco, string.Empty);
            TituloGrid(this.grdDisciplinasMaisEducacaoAtivas, "Disciplinas");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
            ControlaAcesso(btnExluirEletivas, AcaoControle.excluir);
            ControlaAcesso(btnSalvarProgressao, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);

            this.ControlaAcesso(this.grdOptativaReforco);
            this.ControlaAcesso(this.grdProgressao);
            this.ControlaAcesso(this.grdEletivas);
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

                if (!ddlTurmaProgressao.Enabled)
                {
                    tseDisciplinaProgressao.Enabled = false;
                    tseDisciplinaReferencia.Enabled = false;
                }
                tseUnidadeProfissionalizante.Visible = chkConcomitante.Checked;

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
        private void CarregaAnoLetivo(DropDownList drop)
        {
            ListItem item = new ListItem("<Nenhum>", string.Empty);

            try
            {
                drop.Items.Clear();
                drop.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.AnoLetivo, RN.PeriodoLetivo.QueryListaAnos);
                drop.DataBind();
                drop.Items.Insert(0, item);

                if (drop == ddlAnoProgressao)
                {
                    if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && ddlAnoProgressao.Items.Count > 0)
                    {
                        ddlAnoProgressao.SelectedValue = ddlAno.SelectedValue;
                        ddlAnoProgressao_SelectedIndexChanged(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
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
        protected void grdDisciplinasAtivas_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGridDisciplinas(grdDisciplinasAtivas);
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
                        btnExcluir.Visible = false;

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
                        btnSalvarProgressao.Visible = true;
                        btnSalvarUnidadeProfissionalizante.Visible = true;
                        btnExcluirTurmaProfissionalizante.Visible = false;
                        break;
                    }
            }
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnExcluir.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
            btnSalvarProgressao.Visible = false;
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
                LimparCamposTurmaConcomitante();
                LimparCamposTurmaMaisEducacao();
                LimparCamposTurmaOptativaReforco();
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
                        pnlProgressao.Visible = false;
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        lblMensagem2.Text = lblMensagem.Text;
                        btnNovo.Visible = false;
                        btnExcluir.Visible = false;
                        btnSalvarProgressao.Visible = false;
                        this.LimparTela();
                    }
                }
                else if (!tseAluno.DBValue.IsNull)
                {
                    pnAnoPeriodo.Visible = false;
                    pnSerieTurma.Visible = false;
                    pnlProgressao.Visible = false;
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    lblMensagem2.Text = lblMensagem.Text;
                    btnNovo.Visible = false;
                    btnSalvarProgressao.Visible = false;
                    btnExcluir.Visible = false;
                    pnlTurmaProfissionalizante.Visible = false;
                    pnlConcomitante.Visible = false;
                    pnlTurmaMaisEducacao.Visible = false;
                    pnlTurmaOptativaReforco.Visible = false;
                    pnlEletivas.Visible = false;
                }
                else
                {
                    pnAnoPeriodo.Visible = false;
                    pnSerieTurma.Visible = false;
                    pnlProgressao.Visible = false;
                    lblMensagem.Text = "Favor consultar um aluno.";
                    lblMensagem2.Text = lblMensagem.Text;
                    btnNovo.Visible = false;
                    btnSalvarProgressao.Visible = false;
                    btnExcluir.Visible = false;
                    pnlTurmaProfissionalizante.Visible = false;
                    pnlConcomitante.Visible = false;
                    pnlTurmaMaisEducacao.Visible = false;
                    pnlTurmaOptativaReforco.Visible = false;
                    pnlEletivas.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = lblMensagem.Text;
            }
        }

        private void CarregaTSearchs()
        {
            if (Page.IsCallback)
            {
                return;
            }

            CarregaDisciplinaReferencia();
            CarregaDisciplinaProgressao();
            CarregaCursoConcomitante();
            CarregaCurso();
        }

        private void CarregaDisciplinaReferencia()
        {
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;

            if (pnlProgressao.Visible)
            {
                //Verificar se tem parametros para query
                if (!string.IsNullOrEmpty(ddlSerieProgressao.SelectedValue) && tseAluno.IsValidDBValue && tseAluno.Value != null)
                {
                    table = " VW_DISCIPLINAREFERENCIA ";
                    tseDisciplinaReferencia.SqlWhere = " SERIE = '" + ddlSerieProgressao.SelectedValue + "' and ALUNO = '" + tseAluno.DBValue.ToString() + "'";
                }
            }

            coluna.Add("disciplina");
            coluna.Add("NOME_COMPL");

            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

            tseDisciplinaReferencia.SqlSelect = sqlSelect;
            tseDisciplinaReferencia.DataBind();
        }

        private void CarregaDisciplinaProgressao()
        {
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;

            if (pnlProgressao.Visible)
            {
                //Verificar se tem parametros para query
                if (tseAluno.IsValidDBValue && tseAluno.Value != null
                    && !string.IsNullOrEmpty(ddlAnoProgressao.SelectedValue)
                    && !string.IsNullOrEmpty(ddlPeriodoProgressao.SelectedValue)
                    && !string.IsNullOrEmpty(ddlTurnoProgressao.SelectedValue)
                    && !string.IsNullOrEmpty(ddlTurmaProgressao.SelectedValue))
                {
                    table = " VW_DISCIPLINAPROGRESSAO ";
                    tseDisciplinaProgressao.SqlWhere = " ANO = '" + ddlAnoProgressao.SelectedValue + "' and SEMESTRE = '" + ddlPeriodoProgressao.SelectedValue + "' AND TURNO = '" + ddlTurnoProgressao.SelectedValue + "' AND TURMA ='" + ddlTurmaProgressao.SelectedValue + "' ";
                }
            }

            coluna.Add("DISCIPLINA");
            coluna.Add("NOME_COMPL");
            coluna.Add("CURRICULO");

            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

            tseDisciplinaProgressao.SqlSelect = sqlSelect;
            tseDisciplinaProgressao.DataBind();
        }

        private void CarregaCursoConcomitante()
        {
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;

            if (pnlTurmaProfissionalizante.Visible)
            {
                //Verificar se tem parametros para query
                if (!string.IsNullOrEmpty(lblUnidadeEnsinoConcomitante.Text))
                {
                    table = " VW_CURSOCONCOMITANTE ";

                    sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

                    tseCursoConcomitante.SqlSelect = sqlSelect;
                    tseCursoConcomitante.SqlWhere = string.Format(" unidade_ens = '{0}' ", lblUnidadeEnsinoConcomitante.Text);
                    tseCursoConcomitante.DataBind();
                }
            }

            coluna.Add("curso");
            coluna.Add("nome");

            tseCursoConcomitante.SqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);
            tseDisciplinaProgressao.DataBind();
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

        private void CarregaDadosConfirmacao()
        {
            //Desabilita campos, pois ira valer os dados da confirmação 
            DesabilitarCamposMatriculaPrincipal();

            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
            RN.Turno rnTurno = new Turno();
            string aluno = tseAluno.DBValue.ToString();
            string censo = txtFaculdade.Text;
            int ano = Convert.ToInt32(ddlAno.SelectedValue);
            int periodo = Convert.ToInt32(ddlSemestre.SelectedValue);

            //Busca confirmação de matricula confirmada para o ano / periodo selecionado
            confirmacaoMatricula = rnConfirmacaoMatricula.ObtemConfirmacaoAtivaPor(aluno, ano, periodo);

            //verifica se aluno possui confirmação de matricula confirmada para a escola
            if (confirmacaoMatricula.Censo == censo)
            {
                tseCurso.DBValue = confirmacaoMatricula.Curso;

                ddlTurno.Items.Add(new ListItem(rnTurno.RetornaDescricaoTurno(confirmacaoMatricula.Turno), confirmacaoMatricula.Turno));
                ddlTurno.SelectedValue = confirmacaoMatricula.Turno;

                ddlSerie.Items.Add(confirmacaoMatricula.Serie.ToString());
                ddlSerie.SelectedValue = confirmacaoMatricula.Serie.ToString();
                ddlSerie_SelectedIndexChanged(null, null);
            }
            else
            {
                lblMensagem.Text = "Não existe confirmação de matrícula para este aluno nesta escola / ano / periodo.";
                lblMensagem2.Text = lblMensagem.Text;
            }
        }

        private void CarregaMatriculaPrincipal()
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
            DadosEnturmacaoAluno enturmacao = new DadosEnturmacaoAluno();
            RN.Turno rnTurno = new Turno();
            bool possuiMatriculaPrincial = false;

            try
            {
                LimparCamposMatriculaPrincipal();

                if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue || tseAluno.ValidateKey(tseAluno.DBValue.ToString())))
                {
                    //Carrega dados na matricula principal
                    enturmacao = rnMatricula.ObtemMatriculaPrincipalAtivaPor(tseAluno.DBValue.ToString());
                    possuiMatriculaPrincial = !string.IsNullOrEmpty(enturmacao.Aluno);

                    if (_tipoOperacao == TipoOperacao.Consultar)
                    {
                        if (possuiMatriculaPrincial)
                        {
                            //Para opção de Consulta, adiciona nos combos apenas a opção correta                        
                            ddlAno.Items.Add(enturmacao.Ano.ToString());
                            ddlAno.SelectedValue = enturmacao.Ano.ToString();

                            ddlSemestre.Items.Add(enturmacao.Periodo.ToString());
                            ddlSemestre.SelectedValue = enturmacao.Periodo.ToString();
                            CarregaCurso();

                            tseCurso.DBValue = enturmacao.Curso;

                            ddlTurno.Items.Add(rnTurno.RetornaDescricaoTurno(enturmacao.Turno));
                            ddlTurno.SelectedValue = rnTurno.RetornaDescricaoTurno(enturmacao.Turno);

                            ddlSerie.Items.Add(enturmacao.Serie.ToString());
                            ddlSerie.SelectedValue = enturmacao.Serie.ToString();

                            ddlTurma.Items.Add(enturmacao.Turma);
                            ddlTurma.SelectedValue = enturmacao.Turma.ToString();

                            CarregaAnoLetivo(ddlAnoProgressao);

                            btnExcluir.Visible = true;
                            btnSalvarProgressao.Visible = true;
                        }
                        else
                        {
                            lblMensagem.Text = "Não existe enturmação para este aluno(a). Para enturmar clique no botão NOVO.";
                            lblMensagem2.Text = lblMensagem.Text;
                            ddlTurma.Items.Clear();
                            ddlAno.Items.Clear();
                            ddlSemestre.Items.Clear();
                            btnExcluir.Visible = false;
                        }
                    }
                    else
                    {
                        if (possuiMatriculaPrincial)
                        {
                            lblMensagem.Text = "Já existe matrícula para este aluno.";
                            lblMensagem2.Text = lblMensagem.Text;
                            btnSalvar.Enabled = false;
                        }
                        else
                        {
                            lblMensagem.Text = string.Empty;
                            lblMensagem2.Text = string.Empty;
                            CarregaAnoLetivo(ddlAno);
                        }
                    }

                    //Se o Aluno possuir Matricula principal verificar matriculas especiais
                    if (possuiMatriculaPrincial)
                    {
                        var periodoConc = rnMatricula.ObtemPeriodoMatriculaConcomitantePor(int.Parse(ddlAno.SelectedValue), tseAluno.DBValue.ToString());

                        this.ControlaVisibilidadeUnidadeEnsinoProfissionalizante((periodoConc == -1 ? int.Parse(ddlSemestre.SelectedValue) : periodoConc));
                        this.ControlaVisibilidadeTurmaConcomitante((periodoConc == -1 ? int.Parse(ddlSemestre.SelectedValue) : periodoConc));
                        this.ControlaVisibilidadeTurmaMaisEducacao();
                        this.ControlaVisibilidadeTurmaOptativaReforco();
                        this.ControlaVisibilidadeEletiva();
                        pnlProgressao.Visible = true;
                        btnNovo.Visible = false;
                    }
                    else
                    {
                        pnlTurmaProfissionalizante.Visible = false;
                        pnlTurmaMaisEducacao.Visible = false;
                        pnlTurmaOptativaReforco.Visible = false;
                        pnlEletivas.Visible = false;
                        pnlProgressao.Visible = false;
                        pnlConcomitante.Visible = false;
                        btnNovo.Visible = true;
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

            this.LimparCamposMatriculaPrincipal();
            this.LimparCamposProgressao();
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

        private void DesabilitarCamposMatriculaPrincipal()
        {
            ddlTurno.Enabled = false;
            ddlSerie.Enabled = false;
            tseCurso.Mode = ControlMode.View;
            ddlTipoCurso.Enabled = false;
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
                    pnlConcomitante.Visible = false;
                    pnlTurmaProfissionalizante.Visible = false;
                    pnlTurmaMaisEducacao.Visible = false;
                    pnlTurmaOptativaReforco.Visible = false;
                    pnlEletivas.Visible = false;
                    pnlProgressao.Visible = false;
                    btnExcluir.Visible = false;
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

                    this.ControlaVisibilidadeUnidadeEnsinoProfissionalizante((periodoConc == -1 ? int.Parse(ddlSemestre.SelectedValue) : periodoConc));
                    this.ControlaVisibilidadeTurmaConcomitante(periodoConc);
                    this.ControlaVisibilidadeTurmaMaisEducacao();
                    this.ControlaVisibilidadeTurmaOptativaReforco();
                    this.ControlaVisibilidadeEletiva();
                    btnExcluir.Visible = true;
                    btnSalvar.Visible = false;
                    btnCancel.Visible = false;
                    btnNovo.Visible = false;
                    tseAluno.Enabled = true;
                    pnlProgressao.Visible = true;
                    btnSalvarProgressao.Visible = true;
                    ddlAno.Enabled = false;
                    ddlSemestre.Enabled = false;
                    ddlTurma.Enabled = false;
                    ddlSerie.Enabled = false;
                    ddlTurno.Enabled = false;
                    tseCurso.Mode = ControlMode.View;
                    tseCurso.Enabled = false;
                    lnkVisualizarDisciplina.Visible = false;

                    CarregaAnoLetivo(ddlAnoProgressao);
                    if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && ddlAnoProgressao.Items.Count > 0)
                    {
                        ddlAnoProgressao.SelectedValue = ddlAno.SelectedValue;
                        ddlAnoProgressao_SelectedIndexChanged(null, null);
                    }
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

                    if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && ddlAnoProgressao.Items.Count > 0)
                    {
                        ddlAnoProgressao.SelectedValue = ddlAno.SelectedValue;
                        ddlAnoProgressao_SelectedIndexChanged(null, null);
                    }
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
                CarregaDadosConfirmacao();
            }
            catch (Exception EX)
            {
                lblMensagem.Text = EX.Message;
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

        private void LimparCamposProgressao()
        {

            tseDisciplinaProgressao.ResetValue();
            tseDisciplinaProgressao.Enabled = false;
            tseDisciplinaProgressao.DataBind();
            tseDisciplinaReferencia.ResetValue();
            tseDisciplinaReferencia.Enabled = false;
            tseDisciplinaReferencia.DataBind();

            ddlPeriodoProgressao.ClearSelection();
            ddlTurno.ClearSelection();
            ddlTurmaProgressao.Items.Clear();
            ddlSerieProgressao.Items.Clear();
            ddlTurnoProgressao.Items.Clear();
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

        public void Delete(object ALUNO, object DISCIPLINA, object TURMA, object ANO, object SEMESTRE)
        {
        }

        protected void chkConcomitante_CheckedChanged(object sender, EventArgs e)
        {
            lblUnidadeProfissionalizante.Visible = chkConcomitante.Checked;
            tseUnidadeProfissionalizante.ResetValue();
            tseUnidadeProfissionalizante.Visible = chkConcomitante.Checked;
        }

        protected void btnSalvarUnidadeProfissionalizante_Click(object sender, EventArgs e)
        {
            var mensagem = string.Empty;
            ValidacaoDados validacao = new ValidacaoDados();
            try
            {
                var alunoProfissionalizante = new TceAlunoConcomitante
                {
                    Aluno = Convert.ToString(tseAluno.DBValue),
                    Ano = !string.IsNullOrEmpty(ddlAno.SelectedValue) ? int.Parse(ddlAno.SelectedValue) : 0,
                    Periodo = int.Parse(ddlSemestre.SelectedValue),
                    Status = chkConcomitante.Checked ? RN.AlunoConcomitante.Liberado : RN.AlunoConcomitante.Cancelado,
                    Matricula = this.User.Identity.Name
                };

                if (chkConcomitante.Checked)
                {
                    if (tseUnidadeProfissionalizante.IsValidDBValue)
                    {
                        alunoProfissionalizante.Censo = Convert.ToString(tseUnidadeProfissionalizante.DBValue);
                    }
                    else
                    {
                        lblMensagem.Text = "O Campo CENSO é inválido";
                        lblMensagem2.Text = lblMensagem.Text;
                        return;
                    }
                }

                validacao = RN.AlunoConcomitante.Validar(alunoProfissionalizante);

                if (validacao.Valido)
                {
                    RN.AlunoConcomitante.Salvar(alunoProfissionalizante);

                    lblMensagem.Text = "Aluno concomitante " + alunoProfissionalizante.Status + " com sucesso.";

                    pnlTurmaProfissionalizante.Visible = tseUnidadeProfissionalizante.DBValue.ToString() == txtFaculdade.Text && alunoProfissionalizante.Status == RN.AlunoConcomitante.Liberado;

                    pnlTurmaProfissionalizante.Visible = alunoProfissionalizante.Status == RN.AlunoConcomitante.Liberado;

                    lblUnidadeEnsinoConcomitante.Text = alunoProfissionalizante.Censo;
                    lblNomeUnidadeEnsinoConcomitante.Text = tseUnidadeProfissionalizante["NOME_COMP"].ToString();
                    lblAnoConcomitante.Text = alunoProfissionalizante.Ano.ToString();

                    if (pnlTurmaProfissionalizante.Visible)
                    {
                        tseCursoConcomitante.ResetValue();
                        CarregaCursoConcomitante();
                        CarregaPeriodoLetivo(ddlPeriodoConcomitante, alunoProfissionalizante.Ano);
                    }

                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    lblMensagem2.Text = lblMensagem.Text;
                }

                lblUnidadeProfissionalizante.Visible = chkConcomitante.Checked;
                tseUnidadeProfissionalizante.Visible = chkConcomitante.Checked;
                tseUnidadeProfissionalizante.Mode = chkConcomitante.Checked ? ControlMode.Edit : ControlMode.View;

                ControleAcessoSalvarUnidadeEnsinoProfissionalConcomitante(alunoProfissionalizante);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void btnSalvarTurmaProfissionalizante_Click(object sender, EventArgs e)
        {
            try
            {
                var lyMatricula = new LyMatricula
                {
                    Ano = decimal.Parse(lblAnoConcomitante.Text),
                    Semestre = decimal.Parse(ddlPeriodoConcomitante.SelectedValue),
                    Turma = ddlTurmaConcomitante.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = Convert.ToString(tseAluno.DBValue)
                };

                var turmaRegular = new LyMatricula
                {
                    Ano = decimal.Parse(ddlAno.SelectedValue),
                    Semestre = decimal.Parse(ddlSemestre.SelectedValue),
                    Turma = ddlTurma.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = Convert.ToString(tseAluno.DBValue)
                };

                var retorno = RN.Matricula.ValidarEnsProfConcomitante(lyMatricula, turmaRegular, ddlTurnoConcomitante.SelectedValue, ddlTurno.SelectedValue);

                if (retorno.Valido)
                {
                    RN.Matricula.InserirEnsProfConcomitante(lyMatricula);
                    tseCursoConcomitante.Enabled = false;
                    ddlPeriodoConcomitante.Enabled = false;
                    ddlTurnoConcomitante.Enabled = false;
                    ddlSerieConcomitante.Enabled = false;
                    ddlTurmaConcomitante.Enabled = false;

                    lblMensagem.Text = "Enturmação realizada com sucesso.";

                }
                else
                {
                    lblMensagem.Text = retorno.Mensagem;
                    lblMensagem2.Text = lblMensagem.Text;
                }

                btnExcluirTurmaProfissionalizante.Visible = retorno.Valido;
                btnSalvarTurmaProfissionalizante.Visible = !retorno.Valido;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = lblMensagem.Text;
            }
        }

        protected void btnExcluirTurmaProfissionalizante_Click(object sender, EventArgs e)
        {
            try
            {
                var lyMatricula = new LyMatricula()
                {
                    Ano = decimal.Parse(lblAnoConcomitante.Text),
                    Semestre = decimal.Parse(ddlPeriodoConcomitante.SelectedValue),
                    Turma = ddlTurmaConcomitante.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = Convert.ToString(tseAluno.DBValue)
                };
                var retorno = RN.Matricula.ValidarRemoverEnsProfConcomitante(lyMatricula);

                if (retorno.Valido)
                {
                    RN.Matricula.RemoverEnsProfConcomitante(lyMatricula);
                    tseCursoConcomitante.ResetValue();
                    ddlPeriodoConcomitante.ClearSelection();
                    ddlTurnoConcomitante.Items.Clear();
                    ddlSerieConcomitante.Items.Clear();
                    ddlTurmaConcomitante.Items.Clear();
                    btnExcluirTurmaProfissionalizante.Visible = false;

                    btnSalvarTurmaProfissionalizante.Visible = true;
                    tseCursoConcomitante.Enabled = true;
                    ddlPeriodoConcomitante.Enabled = true;
                    ddlTurnoConcomitante.Enabled = true;
                    ddlSerieConcomitante.Enabled = true;
                    ddlTurmaConcomitante.Enabled = true;

                    lblMensagem.Text = "Exclusão de turma realizada com sucesso.";

                    int ultimoPeriodo = RN.Matricula.RetornaUltimoPeriodoConcomitante(tseAluno.DBValue.ToString(), Convert.ToInt32(lblAnoConcomitante.Text));

                    if (ultimoPeriodo >= 0)
                    {
                        ddlPeriodoConcomitante.SelectedValue = ultimoPeriodo.ToString();
                        ddlPeriodoConcomitante.Enabled = false;
                    }
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

        protected void btnExluirEletivas_Click(object sender, EventArgs e)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();

            try
            {
                string aluno = Convert.ToString(tseAluno.DBValue);
                string usuario = User.Identity.Name;

                var retorno = rnMatricula.ValidaRemocaoEletivas(aluno, usuario);
                if (retorno.Valido)
                {
                    rnMatricula.RemoveEletivas(aluno, usuario);
                    lblMensagem.Text = "Exclusão de eletivas realizada com sucesso.";
                    odsEletivas.Select();
                    this.odsEletivas.DataBind();
                    this.grdEletivas.DataBind();
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

        #region "Controle de Visibilidade e Acesso"

        private void ControlaVisibilidadeUnidadeEnsinoProfissionalizante(int periodoConc)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();

            if (!string.IsNullOrEmpty(ddlSemestre.SelectedValue) && !string.IsNullOrEmpty(ddlAno.SelectedValue))
            {
                RN.DTOs.DadosEnsProfConcomitante concomitante = RN.Matricula.CarregarEnsProfConcomitante(tseAluno.DBValue.ToString(), int.Parse(ddlAno.SelectedValue),
                    periodoConc, User.Identity.Name);

                chkConcomitante.Checked = concomitante.Status == RN.AlunoConcomitante.Liberado;

                chkConcomitante.Visible = concomitante.ExibirLiberacao;
                lblUnidadeProfissionalizante.Visible = chkConcomitante.Checked;
                tseUnidadeProfissionalizante.Visible = chkConcomitante.Checked;
                pnlConcomitante.Visible = concomitante.ExibirLiberacao;

                if (concomitante.Censo != null)
                {
                    tseUnidadeProfissionalizante.DBValue = concomitante.Censo;
                }

                lblUnidadeEnsinoConcomitante.Text = concomitante.Censo;
                lblNomeUnidadeEnsinoConcomitante.Text = concomitante.NomeUnidadeEnsino;

                this.ControleAcessoUnidadeEnsinoProfissionalConcomitante(concomitante);
            }
        }

        protected void ControlaVisibilidadeTurmaConcomitante(int periodoConc)
        {
            LimparCamposTurmaConcomitante();
            DataTable turmas = new DataTable();

            if (!string.IsNullOrEmpty(ddlSemestre.SelectedValue) && !string.IsNullOrEmpty(ddlAno.SelectedValue))
            {

                RN.DTOs.DadosEnsProfConcomitante concomitante = RN.Matricula.CarregarEnsProfConcomitante(tseAluno.DBValue.ToString(), int.Parse(ddlAno.SelectedValue),
                                                         periodoConc, User.Identity.Name);

                pnlTurmaProfissionalizante.Visible = concomitante.Status == RN.AlunoConcomitante.Liberado;

                if (pnlTurmaProfissionalizante.Visible)
                {
                    //tseCursoConcomitante.SqlWhere = string.Format(" c.CONCOMITANTE = 'S' AND uec.unidade_ens = '{0}' ", lblUnidadeEnsinoConcomitante.Text);
                    CarregaCursoConcomitante();
                    lblAnoConcomitante.Text = concomitante.Ano.ToString();

                    CarregaPeriodoLetivo(ddlPeriodoConcomitante, concomitante.Ano);

                    if (concomitante.Enturmado)
                    {
                        tseCursoConcomitante.DBValue = concomitante.Curso;
                        ddlPeriodoConcomitante.SelectedValue = concomitante.Periodo.ToString();

                        if (!string.IsNullOrEmpty(lblUnidadeEnsinoConcomitante.Text) && !string.IsNullOrEmpty(tseCursoConcomitante.DBValue.ToString()))
                        {
                            CarregaTurnoConcomitante(lblUnidadeEnsinoConcomitante.Text, tseCursoConcomitante.DBValue.ToString());
                            ddlTurnoConcomitante.SelectedValue = concomitante.Turno;
                        }

                        if (!string.IsNullOrEmpty(ddlTurnoConcomitante.SelectedValue) && !string.IsNullOrEmpty(tseCursoConcomitante.DBValue.ToString()))
                        {
                            CarregaSerieConcomitante(tseCursoConcomitante.DBValue.ToString(), ddlTurnoConcomitante.SelectedValue);
                            ddlSerieConcomitante.SelectedValue = concomitante.Serie.ToString();
                        }
                        turmas = CarregarDadosDropDataTable(ddlTurmaConcomitante.ID, true);

                        bool resultado = false;

                        if (turmas != null)
                        {
                            foreach (DataRow turma in turmas.Rows)
                            {
                                if (Convert.ToString(turma["TURMA"]) == concomitante.Turma)
                                {
                                    resultado = true;
                                    break;
                                }
                            }

                            if (turmas.Rows.Count < 1 || resultado == false)
                            {
                                ddlTurmaConcomitante.Items.Add(concomitante.Turma);
                                ddlTurmaConcomitante.DataTextField = "turma";
                                ddlTurmaConcomitante.DataValueField = "turma";
                                ddlTurmaConcomitante.SelectedValue = concomitante.Turma;
                            }
                            else if (resultado)
                            {
                                ddlTurmaConcomitante.SelectedValue = concomitante.Turma;
                            }
                        }

                        lblUnidadeEnsinoConcomitante.Text = concomitante.Censo;
                        lblNomeUnidadeEnsinoConcomitante.Text = concomitante.NomeUnidadeEnsino;
                    }

                    this.ControleAcessoTurmaProfissionalConcomitante(concomitante);

                    if (!concomitante.Enturmado)
                    {
                        ddlPeriodoConcomitante.Enabled = true;

                        //Retirado a pedido do chamado 24302
                        //    int ultimoPeriodo = RN.Matricula.RetornaUltimoPeriodoConcomitante(tseAluno.DBValue.ToString(), concomitante.Ano);
                        //    if (ultimoPeriodo >= 0)
                        //    {
                        //        ddlPeriodoConcomitante.SelectedValue = ultimoPeriodo.ToString();
                        //        ddlPeriodoConcomitante.Enabled = false;
                        //    }
                        //    else
                        //    {
                        //        ddlPeriodoConcomitante.Enabled = true;
                        //    }
                    }
                }
            }
        }

        protected void ControlaVisibilidadeTurmaMaisEducacao()
        {
            LimparCamposTurmaMaisEducacao();

            lblAnoMaisEducacao.Text = ddlAno.SelectedValue;
            lblCursoMaisEducacao.Text = "9999.92";
            lblNomeCursoMaisEducacao.Text = "MAIS EDUCAÇÃO";

            if (!string.IsNullOrEmpty(ddlSemestre.SelectedValue) && !string.IsNullOrEmpty(lblAnoMaisEducacao.Text))
            {
                RN.DTOs.DadosMaisEducacao maisEducacao = RN.Matricula.CarregarDadosMaisEducacao(tseAluno.DBValue.ToString(), int.Parse(lblAnoMaisEducacao.Text),
                                                         int.Parse(ddlSemestre.SelectedValue), User.Identity.Name);

                pnlTurmaMaisEducacao.Visible = maisEducacao.ExibirMaisEducacao;

                if (pnlTurmaMaisEducacao.Visible)
                {
                    ddlPeriodoMaisEducacao.Items.Add("0");
                    ddlPeriodoMaisEducacao.SelectedValue = "0";

                    CarregaTurnoMaisEducacao(txtFaculdade.Text, lblCursoMaisEducacao.Text);

                    CarregarDadosDropDataTable(ddlTurmaMaisEducacao.ID, true);

                    if (maisEducacao.Enturmado)
                    {
                        //ddlPeriodoMaisEducacao.SelectedValue = maisEducacao.Periodo.ToString();

                        if (ddlTurnoMaisEducacao.Items.Count > 0)
                        {
                            ddlTurnoMaisEducacao.SelectedValue = maisEducacao.Turno;
                            CarregaSerieMaisEducacao(lblCursoMaisEducacao.Text, ddlTurnoMaisEducacao.SelectedValue);
                        }

                        if (ddlSerieMaisEducacao.Items.Count > 0)
                        {
                            ddlSerieMaisEducacao.SelectedValue = Convert.ToString(maisEducacao.Serie);
                        }

                        DataTable turmas = this.CarregarDadosDropDataTable(ddlTurmaMaisEducacao.ID, true);

                        bool resultado = false;

                        foreach (DataRow turma in turmas.Rows)
                        {
                            if (Convert.ToString(turma["TURMA"]) == maisEducacao.Turma)
                            {
                                resultado = true;
                                break;
                            }
                        }

                        if (turmas.Rows.Count < 1 || resultado == false)
                        {
                            ddlTurmaMaisEducacao.Items.Add(maisEducacao.Turma);
                            ddlTurmaMaisEducacao.DataTextField = "turma";
                            ddlTurmaMaisEducacao.DataValueField = "turma";
                            ddlTurmaMaisEducacao.SelectedValue = maisEducacao.Turma;
                        }
                        else if (resultado == true)
                        {
                            ddlTurmaMaisEducacao.SelectedValue = maisEducacao.Turma;
                        }
                    }

                    this.ControleAcessoTurmaMaisEducacao(maisEducacao);
                }
            }
        }

        protected void ControlaVisibilidadeEletiva()
        {
            RN.Curso rnCurso = new Curso();
            pnlEletivas.Visible = false;

            //Verifica se o curso da turma principal permite eletiva
            if (this.tseCurso.IsValidDBValue && !this.tseCurso.DBValue.IsNull)
            {
                if (rnCurso.PermiteEletivaPor(tseCurso.DBValue.ToString()))
                {
                    pnlEletivas.Visible = true;
                    odsEletivas.Select();
                    this.odsEletivas.DataBind();
                    this.grdEletivas.DataBind();
                }
            }
        }

        protected void ControlaVisibilidadeTurmaOptativaReforco()
        {
            Turma turma = new Turma();
            RN.Matricula matricula = new RN.Matricula();
            RN.Turno turno = new RN.Turno();
            string aluno = tseAluno.DBValue.ToString();
            int ano = 0;
            int periodo = 0;
            string censo = txtFaculdade.Text;

            this.LimparCamposTurmaOptativaReforco();
            pnlTurmaOptativaReforco.Visible = false;

            lblAnoOptativaReforco.Text = ddlAno.SelectedValue;
            lblPeriodoOptativaReforco.Text = ddlSemestre.SelectedValue;

            if (!string.IsNullOrEmpty(lblPeriodoOptativaReforco.Text) && !string.IsNullOrEmpty(lblAnoOptativaReforco.Text))
            {
                ano = int.Parse(lblAnoOptativaReforco.Text);
                periodo = int.Parse(lblPeriodoOptativaReforco.Text);

                pnlTurmaOptativaReforco.Visible = true;

                if (pnlTurmaOptativaReforco.Visible)
                {
                    ddlTurnoOptativaReforco.DataSource = turno.ListaTurnosOptativaReforcoPor(censo, ano, periodo);
                    ddlTurnoOptativaReforco.DataBind();
                    ddlTurnoOptativaReforco.Items.Insert(0, new ListItem("<Nenhum>", " "));

                    this.ControleAcessoTurmaOptativaReforco(censo);
                }
            }
        }

        private void ControleAcessoTurmaProfissionalConcomitante(RN.DTOs.DadosEnsProfConcomitante concomitante)
        {
            if (RN.Usuarios.VerificaAcesso(concomitante.Censo, User.Identity.Name))
            {
                btnSalvarTurmaProfissionalizante.Visible = !concomitante.Enturmado;
                btnExcluirTurmaProfissionalizante.Visible = concomitante.Enturmado;

                ddlPeriodoConcomitante.Enabled = !concomitante.Enturmado;
                tseCursoConcomitante.Enabled = !concomitante.Enturmado;
                tseCursoConcomitante.Mode = ControlMode.Edit;
                ddlTurnoConcomitante.Enabled = !concomitante.Enturmado;
                ddlSerieConcomitante.Enabled = !concomitante.Enturmado;
                ddlTurmaConcomitante.Enabled = !concomitante.Enturmado;
            }
            else
            {
                btnSalvarTurmaProfissionalizante.Visible = false;
                btnExcluirTurmaProfissionalizante.Visible = false;

                ddlPeriodoConcomitante.Enabled = false;
                tseCursoConcomitante.Enabled = false;
                tseCursoConcomitante.Mode = ControlMode.View;
                ddlTurnoConcomitante.Enabled = false;
                ddlSerieConcomitante.Enabled = false;
                ddlTurmaConcomitante.Enabled = false;
            }
        }

        private void ControleAcessoUnidadeEnsinoProfissionalConcomitante(RN.DTOs.DadosEnsProfConcomitante concomitante)
        {
            if (RN.Usuarios.VerificaAcesso(txtFaculdade.Text, User.Identity.Name))
            {
                chkConcomitante.Enabled = true;
                tseUnidadeProfissionalizante.Enabled = true;
                tseUnidadeProfissionalizante.Mode = ControlMode.Edit;
                pnlConcomitante.Enabled = true;
                lblUnidadeEnsinoConcomitante.Enabled = true;
                lblNomeUnidadeEnsinoConcomitante.Enabled = true;
            }
            else
            {
                chkConcomitante.Enabled = false;
                tseUnidadeProfissionalizante.Enabled = false;
                tseUnidadeProfissionalizante.Mode = ControlMode.View;
                pnlConcomitante.Enabled = false;
                lblUnidadeEnsinoConcomitante.Enabled = false;
                lblNomeUnidadeEnsinoConcomitante.Enabled = false;
            }
        }

        private void ControleAcessoSalvarUnidadeEnsinoProfissionalConcomitante(TceAlunoConcomitante concomitante)
        {
            if (RN.Usuarios.VerificaAcesso(txtFaculdade.Text, User.Identity.Name))
            {
                chkConcomitante.Enabled = true;
                tseUnidadeProfissionalizante.Enabled = true;
                tseUnidadeProfissionalizante.Mode = ControlMode.Edit;
                pnlConcomitante.Enabled = true;
                lblUnidadeEnsinoConcomitante.Enabled = true;
                lblNomeUnidadeEnsinoConcomitante.Enabled = true;
                ddlPeriodoConcomitante.Enabled = true;
                ddlTurnoConcomitante.Enabled = true;
                ddlSerieConcomitante.Enabled = true;
                ddlTurmaConcomitante.Enabled = true;
                tseCursoConcomitante.Mode = ControlMode.Edit;
            }
            else
            {
                chkConcomitante.Enabled = false;
                tseUnidadeProfissionalizante.Enabled = false;
                tseUnidadeProfissionalizante.Mode = ControlMode.View;
                pnlConcomitante.Enabled = false;
                lblUnidadeEnsinoConcomitante.Enabled = false;
                lblNomeUnidadeEnsinoConcomitante.Enabled = false;
                ddlPeriodoConcomitante.Enabled = false;
                ddlTurnoConcomitante.Enabled = false;
                ddlSerieConcomitante.Enabled = false;
                ddlTurmaConcomitante.Enabled = false;
                tseCursoConcomitante.Mode = ControlMode.View;
            }
        }

        private void ControleAcessoTurmaMaisEducacao(RN.DTOs.DadosMaisEducacao maisEducacao)
        {
            if (string.IsNullOrEmpty(maisEducacao.Censo))
            {
                maisEducacao.Censo = txtFaculdade.Text;
            }

            if (RN.Usuarios.VerificaAcesso(maisEducacao.Censo, User.Identity.Name))
            {
                btnSalvarTurmaMaisEducacao.Visible = !maisEducacao.Enturmado;
                btnExcluirTurmaMaisEducacao.Visible = maisEducacao.Enturmado;

                //ddlPeriodoMaisEducacao.Enabled = !maisEducacao.Enturmado;
                ddlTurnoMaisEducacao.Enabled = !maisEducacao.Enturmado;
                ddlSerieMaisEducacao.Enabled = !maisEducacao.Enturmado;
                ddlTurmaMaisEducacao.Enabled = !maisEducacao.Enturmado;
            }
            else
            {
                btnSalvarTurmaMaisEducacao.Visible = false;
                btnExcluirTurmaMaisEducacao.Visible = false;

                ddlPeriodoMaisEducacao.Enabled = false;
                ddlTurnoMaisEducacao.Enabled = false;
                ddlSerieMaisEducacao.Enabled = false;
                ddlTurmaMaisEducacao.Enabled = false;
            }
        }

        private void ControleAcessoTurmaOptativaReforco(string censo)
        {
            if (RN.Usuarios.VerificaAcesso(censo, User.Identity.Name))
            {
                btnSalvarTurmaOptativaReforco.Visible = true;
                grdOptativaReforco.Enabled = true;
            }
            else
            {
                btnSalvarTurmaOptativaReforco.Visible = false;
                grdOptativaReforco.Enabled = false;
            }
        }

        #endregion

        #region "Progressao"

        protected void odsProgressao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var matricula = new LyMatricula
            {
                Aluno = e.InputParameters["ALUNO"].ToString(),
                Disciplina = e.InputParameters["DISCIPLINA"].ToString(),
                Turma = e.InputParameters["TURMA"].ToString(),
                Ano = Convert.ToDecimal(e.InputParameters["ANO"]),
                Semestre = Convert.ToDecimal(e.InputParameters["SEMESTRE"]),
                Matricula = this.User.Identity.Name
            };

            RN.Matricula.RemoverProgressaoParcial(matricula);
        }

        private void CarregaTurnoProgressao(int ano, int periodo)
        {
            Turno rnTurno = new Turno();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
            try
            {
                ddlTurnoProgressao.Items.Clear();
                ddlTurnoProgressao.DataSource = rnTurno.ListaTurnoMatrizAtivaPor(ano, periodo);
                ddlTurnoProgressao.DataBind();
                ddlTurnoProgressao.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAnoProgressao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPeriodoProgressao.Items.Clear();
                ddlTurnoProgressao.Items.Clear();
                ddlTurmaProgressao.Items.Clear();
                tseDisciplinaProgressao.ResetValue();
                tseDisciplinaProgressao.Enabled = false;
                ddlSerieProgressao.Items.Clear();
                tseDisciplinaReferencia.ResetValue();
                tseDisciplinaReferencia.Enabled = false;

                if (!string.IsNullOrEmpty(ddlAnoProgressao.SelectedValue))
                {
                    CarregaPeriodoLetivo(ddlPeriodoProgressao, Convert.ToInt32(ddlAnoProgressao.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ddlPeriodoProgressao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlTurnoProgressao.Items.Clear();
                ddlTurmaProgressao.Items.Clear();
                tseDisciplinaProgressao.ResetValue();
                tseDisciplinaProgressao.Enabled = false;
                ddlSerieProgressao.Items.Clear();
                tseDisciplinaReferencia.ResetValue();
                tseDisciplinaReferencia.Enabled = false;

                if (!string.IsNullOrEmpty(ddlAnoProgressao.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodoProgressao.SelectedValue))
                {
                    CarregaTurnoProgressao(Convert.ToInt32(ddlAnoProgressao.SelectedValue), Convert.ToInt32(ddlPeriodoProgressao.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ddlTurnoProgressao_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Turma rnTurma = new Turma();
            try
            {
                ddlTurmaProgressao.Items.Clear();
                tseDisciplinaProgressao.ResetValue();
                tseDisciplinaProgressao.Enabled = false;
                ddlSerieProgressao.Items.Clear();
                tseDisciplinaReferencia.ResetValue();
                tseDisciplinaReferencia.Enabled = false;

                if (!string.IsNullOrEmpty(ddlAnoProgressao.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodoProgressao.SelectedValue) && !string.IsNullOrEmpty(ddlTurnoProgressao.SelectedValue) && !string.IsNullOrEmpty(txtFaculdade.Text))
                {
                    ListItem item = new ListItem("<Nenhum>", string.Empty);

                    ddlTurmaProgressao.DataSource = rnTurma.ListaTurmaAbertaPor(Convert.ToInt32(ddlAnoProgressao.SelectedValue),
                                                                     Convert.ToInt32(ddlPeriodoProgressao.SelectedValue),
                                                                     ddlTurnoProgressao.SelectedValue,
                                                                     txtFaculdade.Text.Trim());
                    ddlTurmaProgressao.DataBind();
                    ddlTurmaProgressao.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ddlTurmaProgressao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseDisciplinaProgressao.ResetValue();
                tseDisciplinaProgressao.Enabled = false;
                ddlSerieProgressao.Items.Clear();
                tseDisciplinaReferencia.ResetValue();
                tseDisciplinaReferencia.Enabled = false;

                if (!string.IsNullOrEmpty(ddlTurma.SelectedValue))
                {
                    tseDisciplinaProgressao.Enabled = true;
                    CarregaDisciplinaProgressao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void tseDisciplinaProgressao_Changed(object sender, EventArgs args)
        {
            RN.Serie rnSerie = new Serie();
            try
            {
                ddlSerieProgressao.Items.Clear();
                tseDisciplinaReferencia.ResetValue();
                tseDisciplinaReferencia.Enabled = false;

                if (this.tseDisciplinaProgressao.IsValidDBValue && !this.tseDisciplinaProgressao.DBValue.IsNull)
                {
                    ListItem item = new ListItem("<Nenhum>", string.Empty);

                    ddlSerieProgressao.DataSource = rnSerie.ListaSerieHistoricoPor(tseAluno.DBValue.ToString());
                    ddlSerieProgressao.DataBind();
                    ddlSerieProgressao.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ddlSerieProgressao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseDisciplinaReferencia.ResetValue();
                tseDisciplinaReferencia.Enabled = false;

                if (!string.IsNullOrEmpty(ddlSerieProgressao.SelectedValue))
                {
                    tseDisciplinaReferencia.Enabled = true;
                    CarregaDisciplinaReferencia();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void btnSalvarProgressao_Click(object sender, EventArgs e)
        {
            var mensagem = string.Empty;

            try
            {
                

                if (string.IsNullOrEmpty(ddlSerieProgressao.SelectedValue))
                {
                    lblMensagem.Text = "O campo SERIE REFERENCIA é obrigatório!";
                    lblMensagem2.Text = lblMensagem.Text;
                    return;
                }

                var progressao = new LyMatricula
                {
                    Aluno = tseAluno.DBValue.ToString(),
                    Ano = !string.IsNullOrEmpty(ddlAnoProgressao.SelectedValue) ? Convert.ToDecimal(ddlAnoProgressao.SelectedValue) : -1,
                    Semestre = !string.IsNullOrEmpty(ddlPeriodoProgressao.SelectedValue) ? Convert.ToDecimal(ddlPeriodoProgressao.SelectedValue) : -1,
                    Disciplina = !string.IsNullOrEmpty(tseDisciplinaProgressao.DBValue.ToString()) ? tseDisciplinaProgressao.DBValue.ToString() : null,
                    DisciplinaReferencia = !string.IsNullOrEmpty(tseDisciplinaReferencia.DBValue.ToString()) ? tseDisciplinaReferencia.DBValue.ToString() : null,
                    Turma = ddlTurmaProgressao.SelectedValue,
                    SerieReferencia = !string.IsNullOrEmpty(ddlSerieProgressao.SelectedValue) ? Convert.ToDecimal(ddlSerieProgressao.SelectedValue) : -1,
                    Matricula = this.User.Identity.Name
                };

                var validacao = RN.Matricula.ValidarProgressaoParcial(progressao);

                if (validacao.Valido)
                {
                    RN.Matricula.InserirProgressaoParcial(progressao);

                    lblMensagem.Text = "Progressão Parcial incluída com sucesso.";

                    this.LimparCamposProgressao();

                    this.odsProgressao.Select();
                    this.odsProgressao.DataBind();
                    this.grdProgressao.DataBind();
                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    lblMensagem2.Text = lblMensagem.Text;
                    tseDisciplinaProgressao.Enabled = true;
                    tseDisciplinaReferencia.Enabled = true;
                }

                _tipoOperacao = TipoOperacao.Consultar;
                this.ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        public object ListarProgressao(object aluno, object painel)
        {
            var alu = aluno.ToString();

            if (!string.IsNullOrEmpty(alu) && Convert.ToBoolean(painel))
            {
                return RN.Matricula.ListarProgressaoParcial(alu);
            }
            return null;
        }

        public object ListarEletivas(object aluno, object painel)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
            var alu = aluno.ToString();

            if (!string.IsNullOrEmpty(alu) && Convert.ToBoolean(painel))
            {
                return rnMatricula.ListaMatriculaEletivaAtivaPor(alu);
            }
            return null;
        }

        protected void grdProgressao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdProgressao);
        }
        protected void grdProgressao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdProgressao.Settings.ShowFilterRow = false;
        }

        protected void grdProgressao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdProgressao.Settings.ShowFilterRow = false;
        }

        protected void grdEletivaso_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdEletivas);
        }

        #endregion

        #region "Concomitante"

        private void LimparCamposTurmaConcomitante()
        {
            ddlPeriodoConcomitante.Items.Clear();
            tseCursoConcomitante.ResetValue();
            ddlTurnoConcomitante.Items.Clear();
            ddlSerieConcomitante.Items.Clear();
            ddlTurmaConcomitante.Items.Clear();
        }

        protected void ddlPeriodoConcomitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTurnoConcomitante.Items.Clear();
            ddlTurmaConcomitante.Items.Clear();
            ddlSerieConcomitante.Items.Clear();

            if (this.tseCursoConcomitante.IsValidDBValue && !this.tseCursoConcomitante.DBValue.IsNull)
            {
                if (!string.IsNullOrEmpty(lblUnidadeEnsinoConcomitante.Text) && !string.IsNullOrEmpty(tseCursoConcomitante.DBValue.ToString()))
                {
                    CarregaTurnoConcomitante(lblUnidadeEnsinoConcomitante.Text, tseCursoConcomitante.DBValue.ToString());
                }
            }
        }

        private void CarregaTurnoConcomitante(string unidade, string curso)
        {
            RN.Turno rnTurno = new Turno();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
            try
            {
                ddlTurnoConcomitante.Items.Clear();
                ddlTurnoConcomitante.DataSource = rnTurno.ListaTurnoPor(unidade, curso);
                ddlTurnoConcomitante.DataBind();
                ddlTurnoConcomitante.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaSerieConcomitante(string curso, string turno)
        {
            RN.Serie rnSerie = new Serie();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
            try
            {
                ddlSerieConcomitante.Items.Clear();
                ddlSerieConcomitante.DataSource = rnSerie.ListaSeriePor(curso, turno);
                ddlSerieConcomitante.DataBind();
                ddlSerieConcomitante.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCursoConcomitante_Changed(object sender, EventArgs args)
        {
            try
            {
                ddlTurnoConcomitante.Items.Clear();
                ddlTurmaConcomitante.Items.Clear();
                ddlSerieConcomitante.Items.Clear();

                if (this.tseCursoConcomitante.IsValidDBValue && !this.tseCursoConcomitante.DBValue.IsNull)
                {
                    if (!string.IsNullOrEmpty(lblUnidadeEnsinoConcomitante.Text) && !string.IsNullOrEmpty(tseCursoConcomitante.DBValue.ToString()))
                    {
                        CarregaTurnoConcomitante(lblUnidadeEnsinoConcomitante.Text, tseCursoConcomitante.DBValue.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ddlSerieConcomitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlTurmaConcomitante.Items.Clear();

                this.CarregarDadosDropDataTable(ddlTurmaConcomitante.ID, true);

                if (ddlTurmaConcomitante.Items.Count == 1)
                {
                    lblMensagem.Text = "Não foram encontradas turmas com vagas para este ano / periodo.";
                    lblMensagem2.Text = lblMensagem.Text;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ddlTurnoConcomitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Serie rnSerie = new Serie();
            try
            {
                ddlTurmaConcomitante.Items.Clear();
                ddlSerieConcomitante.Items.Clear();



                if (!string.IsNullOrEmpty(tseCursoConcomitante.DBValue.ToString()) && !string.IsNullOrEmpty(ddlTurnoConcomitante.SelectedValue))
                {
                    CarregaSerieConcomitante(tseCursoConcomitante.DBValue.ToString(), ddlTurnoConcomitante.SelectedValue);
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        #endregion

        #region "Mais Educacao"

        private void LimparCamposTurmaMaisEducacao()
        {
            ddlPeriodoMaisEducacao.Items.Clear();
            ddlTurnoMaisEducacao.Items.Clear();
            ddlSerieMaisEducacao.Items.Clear();
            ddlTurmaMaisEducacao.Items.Clear();
        }
        protected void lnkVisualizarDisciplinaMaisEducacao_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlTurmaMaisEducacao.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupDisciplinaMaisEducacao();", true);

                    CarregaGridDisciplinas(grdDisciplinasMaisEducacaoAtivas);
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        private void CarregaTurnoMaisEducacao(string unidade, string curso)
        {
            Turno rnTurno = new Turno();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
            try
            {
                ddlTurnoMaisEducacao.Items.Clear();
                ddlTurnoMaisEducacao.DataSource = rnTurno.ListaTurnoPor(unidade, curso);
                ddlTurnoMaisEducacao.DataBind();
                ddlTurnoMaisEducacao.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        private void CarregaSerieMaisEducacao(string curso, string turno)
        {
            Serie rnSerie = new Serie();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
            try
            {
                ddlSerieMaisEducacao.Items.Clear();
                ddlSerieMaisEducacao.DataSource = rnSerie.ListaSeriePor(curso, turno);
                ddlSerieMaisEducacao.DataBind();
                ddlSerieMaisEducacao.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        protected void ddlTurmaMaisEducacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkVisualizarDisciplinaMaisEducacao.Visible = false;
            if (!string.IsNullOrEmpty(ddlTurmaMaisEducacao.SelectedValue))
            {
                lnkVisualizarDisciplinaMaisEducacao.Visible = true;
            }
        }

        protected void ddlTurnoMaisEducacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlSerieMaisEducacao.Items.Clear();
                ddlTurmaMaisEducacao.Items.Clear();
                lnkVisualizarDisciplinaMaisEducacao.Visible = false;

                if (!string.IsNullOrEmpty(ddlTurnoMaisEducacao.SelectedValue) && !string.IsNullOrEmpty(lblCursoMaisEducacao.Text))
                {
                    CarregaSerieMaisEducacao(lblCursoMaisEducacao.Text, ddlTurnoMaisEducacao.SelectedValue);
                }
                CarregarDadosDropDataTable(ddlTurmaMaisEducacao.ID, true);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        protected void ddlSerieMaisEducacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Turma rnTurma = new Turma();

            try
            {
                ddlTurmaMaisEducacao.Items.Clear();

                if (!string.IsNullOrEmpty(ddlSerieMaisEducacao.SelectedValue))
                {
                    CarregarDadosDropDataTable(ddlTurmaMaisEducacao.ID, true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }
        protected void btnSalvarTurmaMaisEducacao_Click(object sender, EventArgs e)
        {
            try
            {
                LyMatricula lyMatricula = new LyMatricula()
                {
                    Ano = decimal.Parse(lblAnoMaisEducacao.Text),
                    Semestre = decimal.Parse(ddlPeriodoMaisEducacao.SelectedValue),
                    Turma = ddlTurmaMaisEducacao.SelectedValue,
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

                var retorno = RN.Matricula.ValidarMaisEducacao(lyMatricula, turmaRegular);

                if (retorno.Valido)
                {
                    RN.Matricula.InserirMaisEducacao(lyMatricula);
                    ddlPeriodoMaisEducacao.Enabled = false;
                    ddlTurnoMaisEducacao.Enabled = false;
                    ddlSerieMaisEducacao.Enabled = false;
                    ddlTurmaMaisEducacao.Enabled = false;

                    lblMensagem.Text = "Enturmação realizada com sucesso.";

                }
                else
                {
                    lblMensagem.Text = retorno.Mensagem;
                    lblMensagem2.Text = lblMensagem.Text;
                }

                btnExcluirTurmaMaisEducacao.Visible = retorno.Valido;
                btnSalvarTurmaMaisEducacao.Visible = !retorno.Valido;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = lblMensagem.Text;
            }
        }

        protected void btnExcluirTurmaMaisEducacao_Click(object sender, EventArgs e)
        {
            try
            {
                LyMatricula lyMatricula = new LyMatricula()
                {
                    Ano = decimal.Parse(lblAnoMaisEducacao.Text),
                    Semestre = decimal.Parse(ddlPeriodoMaisEducacao.SelectedValue),
                    Turma = ddlTurmaMaisEducacao.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                var retorno = RN.Matricula.ValidarRemoverMaisEducacao(lyMatricula);

                if (retorno.Valido)
                {
                    RN.Matricula.RemoverMaisEducacao(lyMatricula);

                    ddlTurnoMaisEducacao.ClearSelection();
                    ddlSerieMaisEducacao.Items.Clear();
                    ddlTurmaMaisEducacao.Items.Clear();
                    btnExcluirTurmaMaisEducacao.Visible = false;

                    btnSalvarTurmaMaisEducacao.Visible = true;

                    ddlTurnoMaisEducacao.Enabled = true;
                    ddlSerieMaisEducacao.Enabled = true;
                    ddlTurmaMaisEducacao.Enabled = true;

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

        protected void grdDisciplinasMaisEducacaoAtivas_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CarregaGridDisciplinas(grdDisciplinasMaisEducacaoAtivas);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }


        #endregion

        #region "Optativa Reforço"

        protected void ddlTurnoOptativaReforco_SelectedIndexChanged(object sender, EventArgs e)
        {
            Turma turma = new Turma();
            ddlTurmaOptativaReforco.ClearSelection();
            ddlTurmaOptativaReforco.Items.Clear();

            try
            {
                string aluno = tseAluno.DBValue.ToString();
                int ano = !string.IsNullOrEmpty(lblAnoOptativaReforco.Text) ? int.Parse(lblAnoOptativaReforco.Text) : 0;
                int periodo = !string.IsNullOrEmpty(lblPeriodoOptativaReforco.Text) ? int.Parse(lblPeriodoOptativaReforco.Text) : 0;
                string censo = txtFaculdade.Text;
                string turno = ddlTurnoOptativaReforco.SelectedValue;

                if (ddlTurnoOptativaReforco.SelectedIndex > 0)
                {
                    ddlTurmaOptativaReforco.DataSource = turma.ListaTurmaAbertaComVagaOptativaReforcoPor(ano, periodo, censo, turno, aluno);
                    ddlTurmaOptativaReforco.DataBind();
                    ddlTurmaOptativaReforco.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        private void LimparCamposTurmaOptativaReforco()
        {
            ddlTurmaOptativaReforco.Items.Clear();
            lblAnoOptativaReforco.Text = string.Empty;
            lblPeriodoOptativaReforco.Text = string.Empty;

            grdOptativaReforco.DataBind();
        }

        protected void grdOptativaReforco_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdOptativaReforco);
        }
        public object ListaMatriculaOptativaReforcoPor(object aluno, object ano, object semestre, object painel)
        {
            string alunoMatriculado = aluno != null ? aluno.ToString() : string.Empty;
            string anoMatriculado = ano != null ? ano.ToString() : string.Empty;
            string periodoMatriculado = semestre != null ? semestre.ToString() : string.Empty;
            RN.Matricula matricula = new RN.Matricula();
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(alunoMatriculado) && !string.IsNullOrEmpty(anoMatriculado) && !string.IsNullOrEmpty(periodoMatriculado) && Convert.ToBoolean(painel))
                dt = matricula.ListaMatriculaAtivaOptativaReforcoPor(alunoMatriculado);

            return dt;
        }

        protected void grdOptativaReforco_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdOptativaReforco.Settings.ShowFilterRow = false;
        }

        protected void grdOptativaReforco_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdOptativaReforco.Settings.ShowFilterRow = false;
        }

        protected void odsOptativaReforco_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            LyMatricula matricula = new LyMatricula();
            RN.Matricula matriculaOptativaReforco = new RN.Matricula();

            matricula.Aluno = e.InputParameters["ALUNO"].ToString();
            matricula.Ano = Convert.ToDecimal(e.InputParameters["ANO"].ToString());
            matricula.Semestre = Convert.ToDecimal(e.InputParameters["SEMESTRE"].ToString());
            matricula.Matricula = this.User.Identity.Name;
            matricula.Turma = e.InputParameters["TURMA"].ToString();

            matriculaOptativaReforco.RemoveMatriculaOptativaReforco(matricula);
        }

        public void RemoveMatriculaOptativaReforco(object ALUNO, object ANO, object SEMESTRE, object TURMA)
        {
        }

        #endregion

        private DataTable CarregarDadosDropDataTable(string idDrop, params bool[] diferenciado)
        {
            DataTable dados = new DataTable();
            Turma rnTurma = new Turma();

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLTURMACONCOMITANTE":
                        {
                            if (diferenciado.Length > 0 && diferenciado[0])
                            {
                                decimal serie = string.IsNullOrEmpty(ddlSerieConcomitante.SelectedValue) ? -1 : Convert.ToDecimal(ddlSerieConcomitante.SelectedValue);
                                dados = rnTurma.ListaTurmasGradeComVagasPor(lblAnoConcomitante.Text, ddlPeriodoConcomitante.SelectedValue, lblUnidadeEnsinoConcomitante.Text, tseCursoConcomitante.DBValue.ToString(), ddlTurnoConcomitante.SelectedValue, serie);
                            }
                            else
                            {
                                dados = rnTurma.ListaTurmasGradeComVagasPor(lblAnoConcomitante.Text, ddlPeriodoConcomitante.SelectedValue, lblUnidadeEnsinoConcomitante.Text);
                            }

                            ddlTurmaConcomitante.DataSource = dados;
                            ddlTurmaConcomitante.DataBind();
                            ddlTurmaConcomitante.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));

                            break;
                        }
                    case "DDLTURMAMAISEDUCACAO":
                        {
                            if (diferenciado.Length > 0 && diferenciado[0])
                            {
                                decimal serie = string.IsNullOrEmpty(ddlSerieMaisEducacao.SelectedValue) ? -1 : Convert.ToDecimal(ddlSerieMaisEducacao.SelectedValue);
                                dados = rnTurma.ListaTurmasGradeComVagasPor(lblAnoMaisEducacao.Text, ddlPeriodoMaisEducacao.SelectedValue, txtFaculdade.Text, lblCursoMaisEducacao.Text, ddlTurnoMaisEducacao.SelectedValue, serie);
                            }
                            else
                            {
                                dados = rnTurma.ListaTurmasGradeComVagasPor(lblAnoMaisEducacao.Text, ddlPeriodoMaisEducacao.SelectedValue, txtFaculdade.Text);
                            }

                            ddlTurmaMaisEducacao.DataSource = dados;
                            ddlTurmaMaisEducacao.DataBind();
                            ddlTurmaMaisEducacao.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));

                            break;
                        }
                }
            }
            catch
            {
                throw;
            }

            return dados;
        }

        protected void btnSalvarTurmaOptativaReforco_Click(object sender, EventArgs e)
        {
            RN.Matricula matricula = new RN.Matricula();
            LyMatricula matriculaOptativaReforco = new LyMatricula();

            try
            {
                matriculaOptativaReforco.Ano = decimal.Parse(lblAnoOptativaReforco.Text);
                matriculaOptativaReforco.Semestre = decimal.Parse(lblPeriodoOptativaReforco.Text);
                matriculaOptativaReforco.Turma = ddlTurmaOptativaReforco.SelectedValue;
                matriculaOptativaReforco.Matricula = User.Identity.Name;
                matriculaOptativaReforco.Aluno = tseAluno.DBValue.ToString();

                var retorno = matricula.ValidaMatriculaOptativaReforco(matriculaOptativaReforco);

                if (retorno.Valido)
                {
                    matricula.SalvaMatriculaOptativaReforco(matriculaOptativaReforco);

                    lblMensagem.Text = "Matrícula Optativa/Reforço realizada com sucesso.";

                    grdOptativaReforco.DataBind();
                    ddlTurmaOptativaReforco.Items.Clear();
                    ddlTurnoOptativaReforco.ClearSelection();
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
    }
}