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

    [NavUrl("~/Academico/MatriculaReforco.aspx"),
    ControlText("MatriculaReforco"),
    Title("Reforço Escolar"),]
    public partial class MatriculaReforco : TPage
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
            TituloGrid(this.grdEletivas, string.Empty);
            TituloGrid(this.grdOptativaReforco, string.Empty);

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnExluirEletivas, AcaoControle.excluir);
            this.ControlaAcesso(this.grdOptativaReforco);
            this.ControlaAcesso(this.grdEletivas);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;
                this.lblMensagem2.Text = string.Empty;

                if (!Page.IsPostBack)
                {
                    hdnSeriePrincipal.Value = string.Empty;
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

                        tsbDisciplina.Mode = ControlMode.View;
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
                LimparCamposTurmaOptativaReforco();


                ddlTurma.Items.Clear();
                tsbDisciplina.ResetValue();

                if ((!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue) )
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
                    pnlTurmaOptativaReforco.Visible = false;
                    pnlEletivas.Visible = false;
                }
                else
                {

                    lblMensagem.Text = "Favor consultar um aluno.";
                    lblMensagem2.Text = lblMensagem.Text;
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

        }
        private void CarregaCursoConcomitante()
        {
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;
                    table = " VW_CURSOCONCOMITANTE ";

                    sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

            coluna.Add("curso");
            coluna.Add("nome");

          //  tseCursoConcomitante.SqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);
          //  tseDisciplinaProgressao.DataBind();
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
            hdnSeriePrincipal.Value = string.Empty;

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
                            hdnSeriePrincipal.Value = enturmacao.Serie.ToString();
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
                      this.ControlaVisibilidadeTurmaOptativaReforco(tseAluno.DBValue.ToString());
 
                    }
                    else
                    {
                        pnlTurmaOptativaReforco.Visible = false;
                        pnlEletivas.Visible = false;
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

        protected void btnExluirEletivas_Click(object sender, EventArgs e)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();           
        }

        #region "Controle de Visibilidade e Acesso"

        private void ControlaVisibilidadeUnidadeEnsinoProfissionalizante(int periodoConc)
        {
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
        }

        protected void ControlaVisibilidadeTurmaConcomitante(int periodoConc)
        {
            DataTable turmas = new DataTable();
        }

      protected void ControlaVisibilidadeEletiva()
        {
            RN.Curso rnCurso = new Curso();
            pnlEletivas.Visible = false;
                    pnlEletivas.Visible = true;
                    odsEletivas.Select();
                    this.odsEletivas.DataBind();
                    this.grdEletivas.DataBind();
        }

      protected void ControlaVisibilidadeTurmaOptativaReforco(string aluno)
      {
          Turma turma = new Turma();
          RN.Matricula matricula = new RN.Matricula();
          RN.Turno turno = new RN.Turno();

          int ano = 0;
          int periodo = 0;
          string censo = txtFaculdade.Text;
          string TurnoPrincipalAluno = "PRINCIPAL";
          this.LimparCamposTurmaOptativaReforco();
          pnlTurmaOptativaReforco.Visible = false;
          if (TurnoPrincipalAluno != "INTEGRAL")
          {
              pnlEletivas.Visible = true;
              pnlTurmaOptativaReforco.Visible = true;
              lblAnoOptativaReforco.Text = ddlAno.SelectedValue;
              lblPeriodoOptativaReforco.Text = ddlSemestre.SelectedValue;

              pnlEletivas.Visible = true;
              pnlTurmaOptativaReforco.Visible = true;

              if (!string.IsNullOrEmpty(lblPeriodoOptativaReforco.Text) && !string.IsNullOrEmpty(lblAnoOptativaReforco.Text))
              {
                  ano = int.Parse(lblAnoOptativaReforco.Text);
                  periodo = int.Parse(lblPeriodoOptativaReforco.Text);



                  if (pnlTurmaOptativaReforco.Visible)
                  {

                      //Pesquisa o turno principal
                      RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
                      DataTable alunos = rnMatricula.ListaDadosEscolaresPor(aluno);

                      foreach (DataRow item in alunos.Rows)
                      {
                          if (Convert.ToString(item["TIPO"]) == TurnoPrincipalAluno)
                          {
                              TurnoPrincipalAluno = Convert.ToString(item["TURNO"]);
                          }
                      }

                      if (TurnoPrincipalAluno != "INTEGRAL")
                      {
                          ListItem item = new ListItem("Selecione", string.Empty);

                          ddlTurno.DataSource = turno.ListaTurnosContraTurnoPor(censo, ano, periodo, TurnoPrincipalAluno);
                          ddlTurno.DataBind();
                          ddlTurno.Items.Insert(0,item);

                          this.ControleAcessoTurmaOptativaReforco(censo);
                      }
                      else
                      {
                          lblMensagem.Text = "Este aluno não é elegível ao reforço escolar.";
                        //  lblMensagem2.Text = lblMensagem.Text;
                          pnlEletivas.Visible = false;
                          pnlTurmaOptativaReforco.Visible = false;
                      }
                  }
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


    

    
        protected void btnSalvarProgressao_Click(object sender, EventArgs e)
        {
            var mensagem = string.Empty;
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
                return rnMatricula.ListaDadosEscolaresPor(alu);
            }
            return null;
        }

        protected void grdEletivaso_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdEletivas);
        }

        #endregion

        #region "Concomitante"

        #endregion

        #region "Mais Educacao"
               

        protected void btnSalvarTurmaMaisEducacao_Click(object sender, EventArgs e)
        {
            try
            {
                LyMatricula lyMatricula = new LyMatricula()
                {
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                LyMatricula turmaRegular = new LyMatricula()
                {
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                var retorno = RN.Matricula.ValidarMaisEducacao(lyMatricula, turmaRegular);

                if (retorno.Valido)
                {
                    RN.Matricula.InserirMaisEducacao(lyMatricula);
                    lblMensagem.Text = "Enturmação realizada com sucesso.";
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

        protected void btnExcluirTurmaMaisEducacao_Click(object sender, EventArgs e)
        {
            try
            {
                LyMatricula lyMatricula = new LyMatricula()
                {
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                var retorno = RN.Matricula.ValidarRemoverMaisEducacao(lyMatricula);

                if (retorno.Valido)
                {
                    RN.Matricula.RemoverMaisEducacao(lyMatricula);

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



        #endregion

        #region "Educacao Especial"
     
        #endregion

        #region "Optativa Reforço"       

        private void LimparCamposTurmaOptativaReforco()
        {

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
                dt = matricula.ListaMatriculaAtivaOptativaReforcoPorProjetoFoco(alunoMatriculado);

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

        protected void btnSalvarTurmaOptativaReforco_Click(object sender, EventArgs e)
        {
            RN.Matricula matricula = new RN.Matricula();
            LyMatricula matriculaOptativaReforco = new LyMatricula();

            RN.Turma turma = new RN.Turma();

            try
            {
                matriculaOptativaReforco.Ano = decimal.Parse(lblAnoOptativaReforco.Text);
                matriculaOptativaReforco.Semestre = decimal.Parse(lblPeriodoOptativaReforco.Text);
                matriculaOptativaReforco.Turma = !ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurma.SelectedValue : null ;
                matriculaOptativaReforco.Matricula = User.Identity.Name;
                matriculaOptativaReforco.Aluno = tseAluno.DBValue.ToString();
                matriculaOptativaReforco.MaisEducacao = turma.ObtemTipoTurma( matriculaOptativaReforco.Ano ,matriculaOptativaReforco.Semestre,  matriculaOptativaReforco.Turma );//P 
  
                var retorno = matricula.ValidaMatriculaOptativaReforcoProjetoFoco(matriculaOptativaReforco);

                if (retorno.Valido)
                {
                    matricula.SalvaMatriculaOptativaReforcoProjetoFoco(matriculaOptativaReforco);

                    lblMensagem.Text = "Matrícula Reforço realizada com sucesso.";

                    grdOptativaReforco.DataBind();
                    ddlTurno.ClearSelection();
                    tsbDisciplina.ResetValue();
                    ddlTurma.Items.Clear();
                    
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

        protected void tsbDisciplina_Changed(object sender, ChangedEventArgs args)
        {
            PesquisaTurma();
        }

        private void PesquisaTurma() {
            try
            {
                if (!tseAluno.DBValue.IsNull && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && tsbDisciplina.DBValue.ToString() != "")
                {
                    Turma turma = new Turma();
                    string aluno = tseAluno.DBValue.ToString();
                    int ano = !string.IsNullOrEmpty(lblAnoOptativaReforco.Text) ? int.Parse(lblAnoOptativaReforco.Text) : 0;
                    int periodo = !string.IsNullOrEmpty(lblPeriodoOptativaReforco.Text) ? int.Parse(lblPeriodoOptativaReforco.Text) : 0;
                    string censo = txtFaculdade.Text;
                    string turno = ddlTurno.SelectedValue;
                    string disciplina = tsbDisciplina.DBValue.ToString();

                    ListItem item = new ListItem("Selecione", string.Empty);

                    ddlTurma.Items.Clear();
                    ddlTurma.DataSource = turma.ListaTurmaReforcoPor(ano, periodo, turno, censo, disciplina, Convert.ToInt32(hdnSeriePrincipal.Value));
                    ddlTurma.DataBind();        

                    ddlTurma.Items.Insert(0, item);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem2.Text = ex.Message;
            }
        }

    }
}