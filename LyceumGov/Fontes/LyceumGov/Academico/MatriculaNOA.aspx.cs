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

    [NavUrl("~/Academico/MatriculaNOA.aspx"),
    ControlText("Matricula NOA"),
    Title("NOA"),]
    public partial class MatriculaNOA : TPage
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
            TituloGrid(this.grdMatriculas, string.Empty);
            TituloGrid(this.grdNOA, string.Empty);

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.ControlaAcesso(this.grdNOA);
            this.ControlaAcesso(this.grdMatriculas);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;
                this.lblMensagem2.Text = string.Empty;

                if (!Page.IsPostBack)
                {
                    hdnSegmentoPrincipal.Value = string.Empty;
                    tseAluno.Enabled = true;

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
                grd.DataSource = dtDisciplinas;
                grd.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Novo:
                    {
                        tseAluno.Enabled = false;
                        CarregaAnoLetivo(ddlAno);

                        break;

                    }
                case TipoOperacao.Consultar:
                    {
                        tseAluno.Enabled = true;

                        ddlAno.Enabled = false;
                        ddlSemestre.Enabled = false;

                        break;
                    }
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
                LimparCamposTurmaNOA();


                ddlTurma.Items.Clear();

                if ((!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue))
                {

                    lblMensagem.Text = string.Empty;
                    lblMensagem2.Text = string.Empty;


                    string aluno = tseAluno.IsValidDBValue ? Convert.ToString(tseAluno.DBValue) : null;

                    bool alunoAtivo = rnAluno.EhAlunoAtivoPor(aluno);

                    if (alunoAtivo)
                    {
                        CarregaDadosAluno(aluno);
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        lblMensagem2.Text = lblMensagem.Text;
                        this.LimparTela();
                    }
                }
                else if (!tseAluno.DBValue.IsNull)
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    lblMensagem2.Text = lblMensagem.Text;
                    pnlTurmaNOA.Visible = false;
                    pnlMatriculas.Visible = false;
                }
                else
                {

                    lblMensagem.Text = "Favor consultar um aluno.";
                    lblMensagem2.Text = lblMensagem.Text;
                    pnlTurmaNOA.Visible = false;
                    pnlMatriculas.Visible = false;
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

        }     

        public void CarregaDadosAluno(string aluno)
        {
            RN.Aluno rnAluno = new Aluno();
            DataTable qTable = rnAluno.ObtemDadosAlunoParaMatriculaPor(aluno);
            if (qTable.Rows.Count > 0)
            {

                lblUnidadeEnsinoReforco.Text = Convert.ToString(qTable.Rows[0]["NOMEUNIDADEFISICA"]);
                txtFaculdade.Text = Convert.ToString(qTable.Rows[0]["UNIDADE_ENSINO"]);

                _tipoOperacao = TipoOperacao.Consultar;
                ControlarTipoOperacao();
                this.CarregaMatriculaPrincipal();
            }
        }

        private void CarregaTurno(string unidade, string curso)
        {
            RN.Turno rnTurno = new Turno();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
        }

        private void CarregaSerie(string curso, string turno)
        {
            RN.Serie rnSerie = new Serie();
            ListItem item = new ListItem("<Nenhum>", string.Empty);
        }

        private void CarregaDadosConfirmacao()
        {
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
            RN.Turno rnTurno = new Turno();
            string aluno = tseAluno.DBValue.ToString();
        }

        private void CarregaMatriculaPrincipal()
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
            DadosEnturmacaoAluno enturmacao = new DadosEnturmacaoAluno();
            RN.Turno rnTurno = new Turno();
            bool possuiMatriculaPrincial = false;
            hdnSegmentoPrincipal.Value = string.Empty;

            try
            {
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
                            hdnSegmentoPrincipal.Value = enturmacao.Tipo;

                        }
                        else
                        {
                            lblMensagem.Text = "Não existe enturmação para este aluno(a).";
                            lblMensagem2.Text = lblMensagem.Text;
                        }
                    }
                    else
                    {
                        if (possuiMatriculaPrincial)
                        {
                            lblMensagem.Text = "Já existe matrícula para este aluno.";
                            lblMensagem2.Text = lblMensagem.Text;
                            //    btnSalvar.Enabled = false;
                        }
                        else
                        {
                            lblMensagem.Text = string.Empty;
                            lblMensagem2.Text = string.Empty;
                            // CarregaAnoLetivo(ddlAno);
                        }
                    }

                    //Se o Aluno possuir Matricula principal verificar matriculas especiais
                    if (possuiMatriculaPrincial)
                    {
                        this.ControlaVisibilidadeTurmaNOA(tseAluno.DBValue.ToString());

                        PesquisaTurma();

                    }
                    else
                    {
                        pnlTurmaNOA.Visible = false;
                        pnlMatriculas.Visible = false;
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
            lblUnidadeEnsinoReforco.Text = string.Empty;
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

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Consultar;
            ControlarTipoOperacao();
            CarregaMatriculaPrincipal();
        }

        protected void btnSalvarUnidadeProfissionalizante_Click(object sender, EventArgs e)
        {
            var mensagem = string.Empty;
            ValidacaoDados validacao = new ValidacaoDados();
        }




        protected void ControlaVisibilidadeEletiva()
        {
            RN.Curso rnCurso = new Curso();
            pnlMatriculas.Visible = false;
            pnlMatriculas.Visible = true;
            this.odsMatriculas.Select();
            this.odsMatriculas.DataBind();
            this.grdMatriculas.DataBind();
        }

        protected void ControlaVisibilidadeTurmaNOA(string aluno)
        {
            Turma turma = new Turma();
            RN.Matricula matricula = new RN.Matricula();
            RN.Turno turno = new RN.Turno();

            int ano = 0;
            int periodo = 0;
            string censo = txtFaculdade.Text;
            string TurnoPrincipalAluno = "PRINCIPAL";
            this.LimparCamposTurmaNOA();
            pnlTurmaNOA.Visible = false;
        
            pnlMatriculas.Visible = true;
            pnlTurmaNOA.Visible = true;
            lblAnoNOA.Text = ddlAno.SelectedValue;
            lblPeriodoNOA.Text = ddlSemestre.SelectedValue;

            pnlMatriculas.Visible = true;
            pnlTurmaNOA.Visible = true;

            if (!string.IsNullOrEmpty(lblPeriodoNOA.Text) && !string.IsNullOrEmpty(lblAnoNOA.Text))
            {
                ano = int.Parse(lblAnoNOA.Text);
                periodo = int.Parse(lblPeriodoNOA.Text);

                if (pnlTurmaNOA.Visible)
                {                   

                    this.ControleAcessoTurmaNOA(censo);
                    
                }
            }
        }

        private void ControleAcessoTurmaMaisEducacao(RN.DTOs.DadosMaisEducacao maisEducacao)
        {
            if (string.IsNullOrEmpty(maisEducacao.Censo))
            {
                maisEducacao.Censo = txtFaculdade.Text;
            }
        }

        private void ControleAcessoTurmaNOA(string censo)
        {
            if (RN.Usuarios.VerificaAcesso(censo, User.Identity.Name))
            {
                btnSalvarTurmaNOA.Visible = true;
                grdNOA.Enabled = true;
            }
            else
            {
                btnSalvarTurmaNOA.Visible = false;
                grdNOA.Enabled = false;
            }
        }


        private void LimparCamposTurmaNOA()
        {

            lblAnoNOA.Text = string.Empty;
            lblPeriodoNOA.Text = string.Empty;

            grdNOA.DataBind();
        }

        protected void grdNOA_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdNOA);
        }
        public object ListaMatriculaNOAPor(object aluno, object ano, object semestre, object painel)
        {
            string alunoMatriculado = aluno != null ? aluno.ToString() : string.Empty;
            string anoMatriculado = ano != null ? ano.ToString() : string.Empty;
            string periodoMatriculado = semestre != null ? semestre.ToString() : string.Empty;
            RN.Matricula matricula = new RN.Matricula();
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(alunoMatriculado) && !string.IsNullOrEmpty(anoMatriculado) && !string.IsNullOrEmpty(periodoMatriculado) && Convert.ToBoolean(painel))
                dt = matricula.ListaMatriculaAtivaNOA(alunoMatriculado);

            return dt;
        }

        protected void grdNOA_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdNOA.Settings.ShowFilterRow = false;
        }

        protected void grdNOA_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdNOA.Settings.ShowFilterRow = false;
        }

        protected void odsNOA_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            LyMatricula matricula = new LyMatricula();
            RN.Matricula matriculaNOA = new RN.Matricula();

            matricula.Aluno = e.InputParameters["ALUNO"].ToString();
            matricula.Ano = Convert.ToDecimal(e.InputParameters["ANO"].ToString());
            matricula.Semestre = Convert.ToDecimal(e.InputParameters["SEMESTRE"].ToString());
            matricula.Matricula = this.User.Identity.Name;
            matricula.Turma = e.InputParameters["TURMA"].ToString();

            matriculaNOA.RemoveMatriculaLetramentoNOA(matricula);
        }

        public void RemoveMatriculaNOA(object ALUNO, object ANO, object SEMESTRE, object TURMA)
        {
        }


        protected void btnSalvarTurmaNOA_Click(object sender, EventArgs e)
        {
            RN.Matricula matricula = new RN.Matricula();
            LyMatricula matriculaNOA = new LyMatricula();

            RN.Turma turma = new RN.Turma();

            try
            {
                matriculaNOA.Ano = decimal.Parse(lblAnoNOA.Text);
                matriculaNOA.Semestre = decimal.Parse(lblPeriodoNOA.Text);
                matriculaNOA.Turma = !ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurma.SelectedValue : null;
                matriculaNOA.Matricula = User.Identity.Name;
                matriculaNOA.Aluno = tseAluno.DBValue.ToString();
                matriculaNOA.MaisEducacao = "O";

                var retorno = matricula.ValidaMatriculaNOA(matriculaNOA);

                if (retorno.Valido)
                {
                    matricula.SalvaMatriculaLetramentoNOA(matriculaNOA);

                    lblMensagem.Text = "Matrícula NOA realizada com sucesso.";

                    grdNOA.DataBind();                   

                }
                else
                {
                    lblMensagem.Text = retorno.Mensagem.Replace(Environment.NewLine, "<br />");
                    lblMensagem2.Text = lblMensagem.Text;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = lblMensagem.Text;
            }
        }

     

        private void PesquisaTurma()
        {
            try
            {
                if (!tseAluno.DBValue.IsNull && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    Turma turma = new Turma();
                    string aluno = tseAluno.DBValue.ToString();
                    int ano = !string.IsNullOrEmpty(lblAnoNOA.Text) ? int.Parse(lblAnoNOA.Text) : 0;
                    int periodo = !string.IsNullOrEmpty(lblPeriodoNOA.Text) ? int.Parse(lblPeriodoNOA.Text) : 0;
                    string censo = txtFaculdade.Text;
                    string turno = ddlTurno.SelectedValue;

                    ListItem item = new ListItem("Selecione", string.Empty);

                    ddlTurma.Items.Clear();
                    ddlTurma.DataSource = turma.ListaTurmaNOAPor(ano, periodo, turno, censo, hdnSegmentoPrincipal.Value);
                    ddlTurma.DataBind();

                    ddlTurma.Items.Insert(0, item);
                    this.ddlTurma.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

        public object ListarMatriculas(object aluno, object painel)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
            var alu = aluno.ToString();

            if (!string.IsNullOrEmpty(alu) && Convert.ToBoolean(painel))
            {
                return rnMatricula.ListaMatriculaSemNOAPor(alu);
            }
            return null;
        }

        protected void grdMatriculas_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMatriculas);
        }

    }
}