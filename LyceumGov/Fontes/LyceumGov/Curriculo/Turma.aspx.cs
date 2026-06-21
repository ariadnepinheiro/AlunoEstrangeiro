using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Collections.Generic;
using Techne.Lyceum.RN;
using System.Linq;
using DevExpress.Web.ASPxTabControl;
using System.Text;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System.Data;
using Seeduc.Infra.Data;
using System.Configuration;
using Techne.Lyceum.RN.Util;
using System.Text.RegularExpressions;

namespace Techne.Lyceum.Net.Curriculo
{
    [
        NavUrl("~/Curriculo/Turma.aspx"),
        ControlText("Turma"),
        Title("Cadastro de Turma"),
    ]
    public partial class Turma : TPage
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

        #region Propriedades e Enum
        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            ConsultarRetornaDados,
            Inicial,
            Reativar
        }

        private String Usuario;

        public String usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        private DadosPadraoAcessoTurma Padaces = new DadosPadraoAcessoTurma();

        public DadosPadraoAcessoTurma padaces
        {
            get { return Padaces; }
            set { Padaces = value; }
        }

        private static String COR_GLP = "#C8FFBF";
        private static String COR_GLP_00000000 = "#C8FFBF";
        private static String COR_GLP_99999999 = "MediumSeaGreen";
        private static String COR_CT = "#7FC9FF";
        private static String COR_CT_00000000 = "#7FC9FF"; //AZUL CLARO
        private static String COR_CT_99999999 = "#0F6BFF"; //AZUL ESCURO
        private int NumMaxAluno = -1;

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }

        private RN.Turma.DadosTurma ObjetoTurma
        {
            get { return (RN.Turma.DadosTurma)ViewState["ObjetoTurma"]; }
            set { ViewState["ObjetoTurma"] = value; }
        }

        private RN.Turma.DadosTurma ObjetoTurmaInicial
        {
            get { return (RN.Turma.DadosTurma)ViewState["ObjetoTurmaInicial"]; }
            set { ViewState["ObjetoTurmaInicial"] = value; }
        }

        #endregion

        #region Eventos de Página

        private int GetSelectedRowOnTheCurrentPage()
        {
            var startIndexOnPage = this.grdMatriculaAluno.PageIndex * this.grdMatriculaAluno.SettingsPager.PageSize;
            var selectedRow = -1;

            for (var i = 0; i < this.grdMatriculaAluno.VisibleRowCount; i++)
            {
                if (this.grdMatriculaAluno.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    selectedRow = startIndexOnPage + i;
                    break;
                }
            }

            this.grdMatriculaAluno.Selection.UnselectAll();
            return selectedRow;
        }

        protected void grdMatriculaAluno_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var dependencia = (string)grdMatriculaAluno.GetRowValues(e.VisibleIndex, "DEPENDENCIA");

            if (e.ButtonType == ColumnCommandButtonType.Select)
            {
                e.Visible = false;

                if (!string.IsNullOrEmpty(dependencia)
                    && dependencia == "S")
                {
                    e.Visible = true;
                }
            }
        }

        protected void grdMatriculaAluno_SelectionChanged(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                return;
            }

            this.pucDependencia.ShowOnPageLoad = true;
            var aluno = Convert.ToString(grdMatriculaAluno.GetRowValues(GetSelectedRowOnTheCurrentPage(), "ALUNO1"));

            if (string.IsNullOrEmpty(aluno))
            {
                return;
            }

            var matricula = new LyMatricula
                                  {
                                      Aluno = aluno,
                                      Ano = Convert.ToDecimal(ObjetoTurma.Ano),
                                      Semestre = Convert.ToDecimal(ObjetoTurma.Periodo),
                                      Turma = ObjetoTurma.Grade
                                  };
            DataTable dependencias = null;

            var ativo = RN.Turma.VerificaSituacaoAtiva(ObjetoTurma.Grade, Convert.ToDecimal(ObjetoTurma.Ano), Convert.ToDecimal(ObjetoTurma.Periodo));

            if (ativo)
            {
                dependencias = RN.Matricula.ListarProgressaoParcial(matricula);
            }
            else
            {
                dependencias = HistMatricula.ListarProgressaoParcial(matricula);
            }

            if (dependencias != null && dependencias.Rows.Count > 0)
            {
                this.grdDependencia.DataSource = dependencias;
                this.grdDependencia.DataBind();
            }
            else
            {
                lblMensagemPopup.Text = "Este aluno não possui dependencia nesta turma / ano / semestre.";
            }

            this.grdMatriculaAluno.Visible = true;

        }

        /// <summary>
        /// Customiza texto do título das Grids
        /// </summary>        
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMatriculaAluno, "Matrículas");
            TituloGrid(grdMatriculaAlunoEletiva, "Matrículas Eletivas");
            TituloGrid(grdTransferenciaSelecionada, "Alocações selecionadas");
            TituloGrid(this.grdDependencia, "Dependências");
        }

        /// <summary>
        /// Evento de load da página
        /// </summary>        
        protected void Page_Load(object sender, EventArgs e)
        {

            //verifica se não é post back da página
            if (!IsPostBack)
            {
                //para a primeira vez que a página é carregada o tipo de operação será inicial
                _tipoOperacao = TipoOperacao.Inicial;

                if (Request.QueryString.Keys.Count > 0) // Tenta carregar dados da turma se existe QueryString
                {
                    CarregarDadosTurma();
                    if (ddlCurso.Items.FindByValue(ObjetoTurma.Curso) != null)
                    {
                        ddlCurso.SelectedItem = ddlCurso.Items.FindByValue(ObjetoTurma.Curso);
                    }
                    else
                    {
                        lblMensagem.Text = "O Curso não está autorizado na unidade escolar.";
                        return;
                    }

                    if (padaces.PermissaoParcial)
                    {
                        hFPermissao.Value = "Parcial";
                    }
                    else
                    {
                        hFPermissao.Value = "Completo";
                    }
                }
                else // Redireciona para ListarTurma caso não exista QueryString
                {
                    Response.Redirect("ListarTurma.aspx");
                }

                ControlarTipoOperacao();
            }

            PopularTable();
            ControlarCorPostCelula();

            // Seta WHERE na TSearchBox de alocações disponíveis do popup de Transferência
            if (tseTurmaTransferencia.IsValidDBValue && !tseTurmaTransferencia.DBValue.IsNull &&
                tseDisciplinaDestino.IsValidDBValue && !tseDisciplinaDestino.DBValue.IsNull &&
                ddlAno.SelectedIndex >= 0 && ddlPeriodo.SelectedIndex >= 0)
            {
                odsTransferencia.SelectParameters.Clear();
                odsTransferencia.SelectParameters.Add("turmaDestino", Convert.ToString(tseTurmaTransferencia["grade"]));
                odsTransferencia.SelectParameters.Add("ano", ddlAno.SelectedValue);
                odsTransferencia.SelectParameters.Add("semestre", ddlPeriodo.SelectedValue);
                odsTransferencia.SelectParameters.Add("disciplinaDestino", Convert.ToString(tseDisciplinaDestino.Value));
            }
        }

        /// <summary>
        /// Evento de LoadComplete da página
        /// </summary>        
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            usuario = HttpContext.Current.User.Identity.Name.ToString();
            padaces = RN.PadraoAcessoTurmas.ConsultarPadAcesTurma(ObjetoTurma.Curso, usuario);
            RN.Turma rnTurma = new RN.Turma();

            if (!IsPostBack)
            {
                // se operação é de Alteração, ativa a Tab de Quadro de Horários
                if (_tipoOperacao == TipoOperacao.Alterar)
                {
                    TabPage tab = pcTurma.TabPages.FindByName("QuadroHorario");
                    if (tab != null)
                        pcTurma.ActiveTabIndex = tab.Index;
                }
            }

            if (_tipoOperacao == TipoOperacao.Novo)
            {
                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!string.IsNullOrEmpty(sessao.Escola))
                    {
                        tseUnidadeResponsavel.Enabled = false;
                    }
                    else
                    {
                        tseUnidadeResponsavel.Enabled = true;
                    }
                }

                QueryTable qt = null;

                //verifica se existe série no objeto de turma
                if (!string.IsNullOrEmpty(ddlSerie.SelectedValue))
                {
                    decimal serie = 0M;
                    //tenta converter o valor da série no objeto da turma para decimal
                    if (Decimal.TryParse(ddlSerie.SelectedValue, out serie))
                    {
                        string faculdade = string.Empty;
                        if (tseUnidadeFisica.IsValidDBValue)
                        {
                            faculdade = Convert.ToString(tseUnidadeFisica.DBValue);
                        }
                        qt = RN.HorarioOperacional.Consultar(faculdade, ddlTurno.SelectedValue, ddlCurso.Value == null ? "" : ddlCurso.Value.ToString(), ObjetoTurma.Curriculo, serie);
                    }
                }

                VerificarHorarioOperacional(qt);
            }
            else if (_tipoOperacao == TipoOperacao.Alterar)
            {
                tseUnidadeResponsavel.Enabled = false;
                tseUnidadeFisica.Enabled = false;

                if (ObjetoTurma != null && !string.IsNullOrEmpty(ObjetoTurma.Serie))
                {
                    decimal serie = 0M;
                    //tenta converter o valor da série no objeto da turma para decimal
                    if (Decimal.TryParse(ObjetoTurma.Serie, out serie))
                    {
                        QueryTable qt = RN.HorarioOperacional.Consultar(ObjetoTurma.Faculdade, ObjetoTurma.Turno, ObjetoTurma.Curso, ObjetoTurma.Curriculo, serie);
                        VerificarHorarioOperacional(qt);
                    }
                }
            }

            if (!string.IsNullOrEmpty(ddlDisciplinaQuadroHorario.SelectedValue))
                tseDocente.QueryParameters["disciplina"].DefaultValue = ddlDisciplinaQuadroHorario.SelectedValue;
            tseDocente.QueryParameters["dtInicio"].DefaultValue = dtIniAula.Date.ToString();
            tseDocente.QueryParameters["dtFim"].DefaultValue = dtFimAula.Date.ToString();

            if (!string.IsNullOrEmpty(ddlDisciplinaQuadroHorario2.SelectedValue))
                tseDocente2.QueryParameters["disciplina"].DefaultValue = ddlDisciplinaQuadroHorario2.SelectedValue;
            tseDocente2.QueryParameters["dtInicio"].DefaultValue = dtIniAula.Date.ToString();
            tseDocente2.QueryParameters["dtFim"].DefaultValue = dtFimAula.Date.ToString();

            //Caso o curso seja Mais Educação somente será listado os docentes marcados como VOLUNTARIOS
            if (ddlCurso.Value != null)
            {
                if (ddlCurso.Value.ToString() == "9999.92")
                {
                    tseDocente.QueryParameters["voluntario"].DefaultValue = "S";
                }
                else
                {
                    tseDocente.QueryParameters["voluntario"].DefaultValue = "N";
                }
            }

            ddlOptativaReforco.Enabled = false;

            //Anderson Wernek
            if (ddlCurso.Value != null)
            {
                int alunosAtivos = 0;
                if (!string.IsNullOrEmpty(ddlTurma.SelectedValue) && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                {

                    alunosAtivos = rnTurma.ObtemTotalAlunosMatriculadosNaTurmaPor(
                               ddlTurma.SelectedValue
                               , ddlAno.SelectedValue
                               , ddlPeriodo.SelectedValue);
                }

                //Se curso 9999.91 ou 9999.92 ou 9999.99, a optativa será desabilitada
                //Mudança de regra: ddlCurso.Value.ToString() != "9999.99"
                if (ddlCurso.Value.ToString() != "9999.91" && ddlCurso.Value.ToString() != "9999.92" && alunosAtivos == 0)
                {
                    ddlOptativaReforco.Enabled = true;
                }
                else
                {
                    ddlOptativaReforco.Enabled = false;
                }
            }

            if (pcTurma.ActiveTabPage.Name == "Matricula")
            {
                //Apenas carrregar quando estiver na aba Alunos matriculado
                CarregarGridMatriculaAluno();
            }
            if (_tipoOperacao == TipoOperacao.Reativar)
            {
                tseUnidadeFisica.Enabled = false;
                ddlOptativaReforco.Enabled = false;
                tseUnidadeResponsavel.Enabled = false;
                ddlTurma.Enabled = false;
            }
        }

        protected void btnBuscarMatriculas_Click(object sender, EventArgs e)
        {
            CarregarGridMatriculaAluno();
        }

        /// <summary>
        /// Evento de PreRenderComplete da página
        /// </summary>        
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnConfirmar, AcaoControle.excluir);

            if (_tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.Novo)
                btnSalvar.Visible = Permission.AllowUpdate;

            //Habilita botão Transferência se ValidarGLP do Web.config estiver com TRUE            
            bool habilitarTransferencia = false;
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["ValidarGLP"]))
                habilitarTransferencia = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["ValidarGLP"]);
            btnTransferencia.Visible = habilitarTransferencia;
            btnTransferencia2.Visible = habilitarTransferencia;
        }

        #endregion

        #region Eventos dos Controles da Aba De Dados da Turma

        protected void dtFimAula_DateChanged(object sender, EventArgs e)
        {
            if (dtFimAula.Date < DateTime.Now)
            {
                tdControleQH.Visible = false;
                tdControleQH2.Visible = false;
                LimparTable();
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarDadosDrop(ddlPeriodo.ID);
            CarregarDadosDrop(ddlTurma.ID);
            ObjetoTurma.Ano = ddlAno.SelectedValue;

            //controle para cores das celulas alteradas
            ControlarCorPostCelula();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <autor> Anderson Wernek</autor>
        protected void ddlOptativaReforco_SelectedIndexChanged(object sender, EventArgs e)
        {
            Verifica();
            //CarregarDadosDrop(ddlDependencia.ID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdDependencia"></param>
        public void Verifica()
        {
            //QueryTable dadosDrop = null;
            string unidadeFisica = Convert.ToString(tseUnidadeFisica.DBValue);
            string turno = Convert.ToString(ddlTurno.SelectedValue);
            Regex rgx = new Regex("\t|\\s+");
            string turma = rgx.Replace(txtTurma.Text, " ");
            txtTurma.Text = turma;
            string dependencia = string.Empty;
            var tipo_depend = string.Empty;
            RN.Dependencia rnDependencia = new Dependencia();
            DataTable dtDependencia = new DataTable();

            //PARA CURSO DE EDUCACAO ESPECIAL 
            if (ddlCurso.Value != null && ddlCurso.Value.ToString() == "9999.91")
            {
                tipo_depend = "SALAAEE";
            }
            else
            {
                tipo_depend = "SALA";
            }

            if (!String.IsNullOrEmpty(ObjetoTurmaInicial.Curso) && !String.IsNullOrEmpty(ObjetoTurmaInicial.Turno) && !String.IsNullOrEmpty(ObjetoTurmaInicial.Curriculo)
                && !String.IsNullOrEmpty(ObjetoTurmaInicial.Ano) && !String.IsNullOrEmpty(ObjetoTurmaInicial.Periodo) && !String.IsNullOrEmpty(tbTurma2.Value))
            {
                var gs = RN.Turma.ConsultarGradeSerie(ObjetoTurmaInicial.Curso, ObjetoTurmaInicial.Turno, ObjetoTurmaInicial.Curriculo,
                ObjetoTurmaInicial.Ano, ObjetoTurmaInicial.Periodo, tbTurma2.Value);
                if (gs != null)
                {
                    dependencia = gs.Dependencia;
                }
            }

            if (ddlOptativaReforco.SelectedValue == "N")
            {
                if (ddlDependencia.Items.Count > 1)
                {
                    dtDependencia = rnDependencia.ConsultarAtiva(unidadeFisica, turno, turma, tipo_depend, ddlAno.SelectedValue, ddlPeriodo.SelectedValue, ddlCurso.Value.ToString(), ddlTurmaReferencia.SelectedValue, ddlDependencia.SelectedValue);
                }
            }
            else
            {
                if (ddlDependencia.Items.Count > 1)
                {
                    dtDependencia = rnDependencia.ConsultaDependenciaAtivaPor(unidadeFisica, turno, turma,
                        ddlAno.SelectedValue,
                        ddlPeriodo.SelectedValue,
                        tipo_depend, ddlCurso.Value.ToString(), ddlDependencia.SelectedValue);
                }

                if (ddlOptativaReforco.SelectedValue == "S")
                {
                    txtNumMaxAluno.Enabled = true;
                    txtNumMaxAluno.ReadOnly = false;
                }
            }

            CarregaDependencia(dtDependencia);
            
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string periodo = ddlPeriodo.SelectedValue;
            CarregarDadosTurma();

            ddlPeriodo.SelectedValue = periodo;
            CarregarDadosDrop(ddlTurma.ID);
            ObjetoTurma.Periodo = ddlPeriodo.SelectedValue;
            ObjetoTurmaInicial.Periodo = ddlPeriodo.SelectedValue;
            pcTurma.Visible = !String.IsNullOrEmpty(ddlPeriodo.SelectedValue);

            if (!string.IsNullOrEmpty(ObjetoTurma.Ano) && !string.IsNullOrEmpty(ObjetoTurma.Periodo))
            {
                decimal ano = Convert.ToDecimal(ObjetoTurma.Ano);
                decimal semestre = Convert.ToDecimal(ObjetoTurma.Periodo);

                SimpleRow dataAula = RN.PeriodoLetivo.ObterDataAula(ano, semestre);

                if (dataAula != null)
                {
                    if (!dataAula["dt_inicio_aula"].IsNull)
                    {
                        dtIniAula.Date = Convert.ToDateTime(dataAula["dt_inicio_aula"]);
                    }
                    if (!dataAula["dt_fim_aula"].IsNull)
                    {
                        dtFimAula.Date = Convert.ToDateTime(dataAula["dt_fim_aula"]);
                    }
                }
            }

            CarregarDadosDrop(ddlCurso.ID);

            //controle para cores das celulas alteradas
            ControlarCorPostCelula();
        }

        protected void ddlCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.Usuarios rnUsuarios = new Usuarios();
                RN.Perfil rnPerfil = new Perfil();
                string unidade_ensino = Convert.ToString(tseUnidadeResponsavel.DBValue);
                var qtTurma = RN.Curso.ConsultarPorUnidadeEnsino(unidade_ensino);
                DataTable dt = (DataTable)qtTurma;

                ddlDependencia.Items.Clear();

                ObjetoTurma.Curriculo = ddlCurriculo.SelectedValue;
                if (ddlCurso.SelectedItem.Text != "Selecione" && dt != null)
                {
                    ObjetoTurma.SalaExterna = dt.Rows[ddlCurso.SelectedIndex].Field<string>("salaexterna").ToString();
                }

                CarregarDadosDrop(ddlTurno.ID);
                CarregarDadosDrop(ddlSerie.ID);
                CarregarDadosDrop(ddlCurriculo.ID);
                CarregarDadosDrop(ddlDependencia.ID);

                CarregarDadosDrop(ddlTurma.ID);
                ObjetoTurma.Curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();

                CarregarDadosDrop(ddlDisciplinaQuadroHorario.ID);
                CarregarDadosDrop(ddlDisciplinaQuadroHorario2.ID);

                if (tseUnidadeFisica.IsValidDBValue)
                {
                    ObjetoTurma.Faculdade = Convert.ToString(tseUnidadeFisica.DBValue);
                }

                chkAmbienteExterno.Checked = false;

                if ((!string.IsNullOrEmpty(ObjetoTurma.Curso)) && (ObjetoTurma.Curso != "9999.92")) //CURSO MAIS EDUCACAO
                {
                    chkAmbienteExterno.Enabled = false;
                    chkMacros.Items.Clear();
                    chkMacros.Visible = false;
                    lblMacros.Visible = false;
                    txtNumMaxAluno.Enabled = true;
                    txtNumMaxAluno.ReadOnly = false;
                    ddlDependencia.Enabled = true;
                }
                else
                {
                    //aqui tenho que desabilitar o campo saladeaula
                    txtNumMaxAluno.Enabled = false;
                    txtNumMaxAluno.ReadOnly = true;
                    chkAmbienteExterno.Visible = true;
                    lblAmbienteExterno.Visible = true;
                    ddlDependencia.Enabled = false;

                    ddlDependencia.ClearSelection();
                    if (ddlDependencia.Items.Count > 0)
                    {
                        ddlDependencia.Items.FindByValue("").Selected = true;
                    }
                }

                if ((ObjetoTurma.SalaExterna == null || ObjetoTurma.SalaExterna.Equals("N")))
                {
                    chkAmbienteExterno.Enabled = false;
                    chkAmbienteExterno.Checked = false;
                    lblAmbienteExterno.Visible = false;
                    chkAmbienteExterno.Visible = false;
                }
                else
                {
                    chkAmbienteExterno.Enabled = true;
                    lblAmbienteExterno.Visible = true;
                    chkAmbienteExterno.Visible = true;
                }

                LimparTable();
                PopularTable();
                //controle para cores das celulas alteradas
                ControlarCorPostCelula();

                if (_tipoOperacao != TipoOperacao.Novo)
                {
                    ControlarVisibilidadeDependencia();
                }
                else
                {
                    var BloquearNumeroMaximoAlunoTurma = Convert.ToBoolean(ConfigurationManager.AppSettings["BloquearNumeroMaximoAlunoTurma"] ?? "false");

                    //Desabilita campo numero maximo de alunos
                    if (BloquearNumeroMaximoAlunoTurma && !rnUsuarios.EhPrivilegiado(User.Identity.Name) && !rnPerfil.PossuiPerfilAlteraMaximoAlunoPor(User.Identity.Name))
                    {
                        txtNumMaxAluno.Enabled = false;
                        txtNumMaxAluno.ReadOnly = true;
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
            if (_tipoOperacao != TipoOperacao.Novo)
            {
                string turma = tbTurma2.Value;
                List<String> errosAlteracaoTurno = RN.Turma.PermiteAlterarTurnoDeTurma(ObjetoTurmaInicial, turma, ddlTurno.SelectedValue);
                lblMensagem.Text = String.Empty;
                if (errosAlteracaoTurno.Count > 0)
                {
                    lblMensagem.Text = " - Não é permitido alterar o Turno para '" + ddlTurno.SelectedItem.Text + "'.";
                    foreach (var erro in errosAlteracaoTurno)
                        lblMensagem.Text += "<br/>" + erro;
                    ddlTurno.SelectedValue = ObjetoTurmaInicial.Turno;
                    return;
                }
            }

            CarregarDadosDrop(ddlCurriculo.ID);
            CarregarDadosDrop(ddlSerie.ID);

            ObjetoTurma.Serie = ddlSerie.SelectedValue;

            CarregarDadosDrop(ddlTurma.ID);

            //atualiza o objeto turma. Esse objeto atualizado irá compor o novo quadro de horário (filtrado por faculdade e turno)
            if (ObjetoTurma.Turno != ddlTurno.SelectedValue)
            {
                ObjetoTurma.Turno = ddlTurno.SelectedValue;
            }

            CarregarDadosDrop(ddlDisciplinaQuadroHorario.ID);
            CarregarDadosDrop(ddlDisciplinaQuadroHorario2.ID);
            //CarregarDadosDrop(ddlDependencia.ID);

            if (tseUnidadeFisica.IsValidDBValue)
            {
                ObjetoTurma.Faculdade = Convert.ToString(tseUnidadeFisica.DBValue);
            }

            if (!string.IsNullOrEmpty(ddlCurso.Value == null ? "" : ddlCurso.Value.ToString()))
            {
                ObjetoTurma.Curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
            }

            LimparTable();
            PopularTable();
            //controle para cores das celulas alteradas
            ControlarCorPostCelula();
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtPrefixoSerie.Text = string.Empty;

            CarregarDadosDrop(ddlTurma.ID);
            ObjetoTurma.Serie = ddlSerie.SelectedValue;

            CarregarDadosDrop(ddlDisciplinaQuadroHorario.ID);
            CarregarDadosDrop(ddlDisciplinaQuadroHorario2.ID);
            if (!String.IsNullOrEmpty(ObjetoTurma.Serie))
            {
                txtPrefixoSerie.Text = RN.Serie.ConsultarPrefixoSerie(ddlCurso.Value == null ? "" : ddlCurso.Value.ToString(), ddlTurno.SelectedValue, ObjetoTurma.Curriculo, Convert.ToDecimal(ObjetoTurma.Serie));
            }

            CarregarDadosDrop(ddlSufixoSerie.ID);

            ObjetoTurma.Curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();

            if (tseUnidadeFisica.IsValidDBValue)
            {
                ObjetoTurma.Faculdade = Convert.ToString(tseUnidadeFisica.DBValue);
            }

            LimparTable();
            PopularTable();
            //controle para cores das celulas alteradas
            ControlarCorPostCelula();
        }

        protected void ddlCurriculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjetoTurma.Curriculo = ddlCurriculo.SelectedValue;
            ObjetoTurma.Serie = string.Empty;
            CarregarDadosDrop(ddlSerie.ID);

            if (!string.IsNullOrEmpty(ddlCurriculo.SelectedValue) && ddlCurso.Value.ToString() == "9999.92")
            {
                CarregarList(ddlCurso.Value.ToString(), ddlTurno.SelectedValue, ddlCurriculo.SelectedValue);
                lblMacros.Visible = true;
                chkMacros.Visible = true;
            }
            else
            {
                lblMacros.Visible = false;
                chkMacros.Visible = false;
            }
        }

        protected void ddlDependencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.Usuarios rnUsuarios = new Usuarios();
                RN.Perfil rnPerfil = new Perfil();

                PopularNumeroAlunoDependencia();
                //controle para cores das celulas alteradas
                ControlarCorPostCelula();

                if (!string.IsNullOrEmpty(txtNumMaxAluno.Text.Trim()) && Convert.ToInt32(txtNumMaxAluno.Text) > NumMaxAluno && ddlCurso.SelectedItem.Value.ToString() != "9999.02")
                {
                    txtNumMaxAluno.Enabled = false;
                    txtNumMaxAluno.ReadOnly = true;
                    var script = @"alert('" + "Não é permitido a alteração do campo \"Número Máximo de Alunos\". Valor informado é maior que o limite permitido." + @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                }
                else
                {
                    txtNumMaxAluno.Enabled = true;
                    txtNumMaxAluno.ReadOnly = false;
                }

                string sala = ddlDependencia.SelectedItem.Value.ToString();

                if (!string.IsNullOrEmpty(sala))
                {
                    if (sala.Substring(0, 2).Equals("SA") && ddlOptativaReforco.SelectedValue != "S" && !rnUsuarios.EhPrivilegiado(User.Identity.Name) && !rnPerfil.PossuiPerfilAlteraMaximoAlunoPor(User.Identity.Name))
                    {
                        txtNumMaxAluno.Enabled = false;
                        txtNumMaxAluno.ReadOnly = true;
                    }
                    else
                    {
                        txtNumMaxAluno.Enabled = true;
                        txtNumMaxAluno.ReadOnly = false;
                    }
                }
                //ATENDIMENTO DEMANDA 6775 FECHAMENNTO ANO LETIVO 2017(BLOQUEIO DO CAMPO)
                var BloquearNumeroMaximoAlunoTurma = Convert.ToBoolean(ConfigurationManager.AppSettings["BloquearNumeroMaximoAlunoTurma"] ?? "false");

                if (BloquearNumeroMaximoAlunoTurma && !rnUsuarios.EhPrivilegiado(User.Identity.Name) && !rnPerfil.PossuiPerfilAlteraMaximoAlunoPor(User.Identity.Name))
                {
                    txtNumMaxAluno.Enabled = false;
                    txtNumMaxAluno.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, EventArgs e)
        {
            try
            {
                lblUAValor.Text = string.Empty;
                hdnUA.Value = string.Empty;

                if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue)
                {
                    ObjetoTurma.UnidadeResponsavel = Convert.ToString(tseUnidadeResponsavel.DBValue);

                    if (!tseUnidadeResponsavel["unidade_ens"].IsNull)
                    {
                        lblUAValor.Text = Convert.ToString(tseUnidadeResponsavel["UA_ATUAL"]);
                        hdnUA.Value = Convert.ToString(tseUnidadeResponsavel["SETOR"]);
                    }

                    tseUnidadeFisica.ResetValue();
                    ObjetoTurma.Faculdade = string.Empty;

                    string unidade_ensino = Convert.ToString(tseUnidadeResponsavel.DBValue);
                    QueryTable qtUnidadeEnsino = RN.UnidadeFisica.ConsultarPorUnidadeEnsino(unidade_ensino);

                    if (qtUnidadeEnsino != null && qtUnidadeEnsino.Rows.Count == 1)
                    {
                        tseUnidadeFisica.DBValue = qtUnidadeEnsino.Rows[0]["unidade_fis"];
                        if (tseUnidadeFisica.IsValidDBValue)
                        {
                            ObjetoTurma.Faculdade = Convert.ToString(tseUnidadeFisica.DBValue);
                        }
                    }

                    CarregarDadosDrop(ddlDependencia.ID);
                }

                if (!string.IsNullOrEmpty(ddlTurno.SelectedValue))
                {
                    ObjetoTurma.Turno = ddlTurno.SelectedValue;
                }

                if (!string.IsNullOrEmpty(ddlCurso.Value == null ? "" : ddlCurso.Value.ToString()))
                {
                    ObjetoTurma.Curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
                }

                if (!string.IsNullOrEmpty(ddlSerie.SelectedValue))
                {
                    ObjetoTurma.Serie = ddlSerie.SelectedValue;
                }

                //controle para cores das celulas alteradas
                ControlarCorPostCelula();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeFisica_Changed(object sender, EventArgs e)
        {
            CarregarDadosDrop(ddlDependencia.ID);

            if (!tseUnidadeFisica.DBValue.IsNull)
            {
                if (ObjetoTurma.Faculdade != Convert.ToString(tseUnidadeFisica.DBValue))
                {
                    if (string.IsNullOrEmpty(ObjetoTurma.Turno) && !string.IsNullOrEmpty(ddlTurno.SelectedValue))
                    {
                        ObjetoTurma.Turno = ddlTurno.SelectedValue;
                    }

                    if (tseUnidadeFisica.IsValidDBValue)
                    {
                        //atualiza o objeto turma. Esse objeto atualizado irá compor o novo quadro de horário (filtrado por faculdade e turno)
                        ObjetoTurma.Faculdade = Convert.ToString(tseUnidadeFisica.DBValue);
                    }
                }
            }

            PopularNumeroAlunoDependencia();

            LimparTable();
            PopularTable();

            //controle para cores das celulas alteradas
            ControlarCorPostCelula();
        }

        private void CarregarList(string curso, string turno, string curriculo)
        {
            DataTable dados = RN.Curriculo.ListarMacrosRelacionadas(curriculo, curso, turno);
            if (dados != null)
            {
                chkMacros.Items.Clear();
                chkMacros.DataSource = dados;
                chkMacros.DataBind();
                if (_tipoOperacao == TipoOperacao.Novo)
                {
                    foreach (ListItem item in chkMacros.Items)
                    {
                        bool obrigatorio = false;

                        foreach (DataRow dado in dados.Rows)
                        {
                            if (dado["NOME_MACRO"].ToString() == item.Text)
                            {
                                if (Convert.ToBoolean(dado["OBRIGATORIO"]))
                                {
                                    obrigatorio = true;
                                }
                                break;
                            }
                        }

                        if (obrigatorio)
                        {
                            item.Selected = true;
                            item.Enabled = false;
                            return;
                        }
                    }
                }
            }
        }


        #endregion

        #region Eventos dos Controles da Aba Quadro de Horários

        /// <summary>
        /// Assinatura do método deve ser mantida para que a mensagem dinâmica de KeyNotFound da TSearch funcione corretamente
        /// </summary>                
        protected void tseDocente_Selecting(object sender, EventArgs args)
        {
            tseDocente.Messages.KeyNotFound = RN.Turma.QueryDocenteQHI_Message_KeyNotFound(tseDocente["matricula"].ToString(), ddlDisciplinaQuadroHorario.SelectedItem.Value, ddlDisciplinaQuadroHorario.SelectedItem.Text, dtIniAula.Date, dtFimAula.Date);
        }

        /// <summary>
        /// Assinatura do método deve ser mantida para que a mensagem dinâmica de KeyNotFound da TSearch funcione corretamente
        /// </summary>        
        protected void tseDocente2_Selecting(object sender, EventArgs args)
        {
            tseDocente2.Messages.KeyNotFound = RN.Turma.QueryDocenteQHI_Message_KeyNotFound(tseDocente2["matricula"].ToString(), ddlDisciplinaQuadroHorario2.SelectedItem.Value, ddlDisciplinaQuadroHorario2.SelectedItem.Text, dtIniAula.Date, dtFimAula.Date);
        }

        protected void tseDocente_Changed(object sender, EventArgs args)
        {
            //armazena num_func em campo hidden para utilização via Javascript
            if (tseDocente.IsValidDBValue)
                hCodigoDocente.Value = Convert.ToString(tseDocente["num_func"]);
            ControlarCorPostCelula();
            //atualiza flag para informar no Javascript após o callback da TSearch qual TSearch mudou de valor
            ddlDisciplinaQuadroHorario.Attributes.Add("flag", "true");
        }

        protected void tseDocente2_Changed(object sender, EventArgs args)
        {
            //armazena num_func em campo hidden para utilização via Javascript
            if (tseDocente2.IsValidDBValue)
                hCodigoDocente.Value = Convert.ToString(tseDocente2["num_func"]);
            ControlarCorPostCelula();
            //atualiza flag para informar no Javascript após o callback da TSearch qual TSearch mudou de valor
            ddlDisciplinaQuadroHorario2.Attributes.Add("flag", "true");
        }

        protected void ddlDisciplinaQuadroHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdnLimpaGrid.Value == "true")
            {
                tseDocente.ResetValue();
                tseDocente.GridFilterParameters.Clear();
            }
            else
                hdnLimpaGrid.Value = "true";

            hCodigoDocente.Value = string.Empty;
            //controle para cores das celulas alteradas
            ControlarCorPostCelula();
        }

        protected void ddlDisciplinaQuadroHorario2_SelectedIndexChanged(object sender, EventArgs e)
        {
            hCodigoDocente.Value = string.Empty;
            if (hdnLimpaGrid2.Value == "true")
            {
                tseDocente2.ResetValue();
                tseDocente2.GridFilterParameters.Clear();
            }
            else
                hdnLimpaGrid2.Value = "true";

            //controle para cores das celulas alteradas
            ControlarCorPostCelula();
        }

        #endregion

        #region Botões
        /// <summary>
        /// Retorna para tela ListaTurmas
        /// </summary>        
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            string queryString = MontarQueryString(string.Empty);
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
            Response.Redirect("ListarTurma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        /// <summary>
        /// Confirma exclusão da turma
        /// </summary>        
        protected void btnConfirmar_Click(object sender, ImageClickEventArgs e)
        {
            lblMensagem.Text = string.Empty;
            Ly_turma dtTurma = new Ly_turma();
            //preenche o datatable de turma com os dados da tela
            ObterDados(dtTurma);

            Ly_hor_aula dtHoraAula = new Ly_hor_aula();
            Ly_aula_docente dtAulaDocente = new Ly_aula_docente();

            RetValue retorno = null;

            if (_tipoOperacao.Equals(TipoOperacao.Excluir))
            {
                if (string.IsNullOrEmpty(ddlTurma.SelectedValue) || string.IsNullOrEmpty(dtTurma.Rows[0]["turma"].ToString()))
                {
                    lblMensagem.Text = "Turma não preenchida. Não é possível excluir a turma.";
                    return;
                }

                //preenche os datatables com os dados do quadro de horário
                dtHoraAula = RN.Turma.ConsultarHoraAula(ddlTurma.SelectedValue);
                dtAulaDocente = RN.Turma.ConsultarAulaDocente(ddlTurma.SelectedValue);

                retorno = RN.Turma.ExcluirTurmaComQuadroHorario(dtTurma, dtHoraAula, dtAulaDocente);
            }
            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    if (retorno.Errors.Count > 0)
                    {
                        lblMensagem.Text = retorno.Errors.ToString().Replace("\n", " ");
                        lblMensagemErro.Text = retorno.Errors.ToString().Replace("\n", " ");
                        lblMensagemErro.Visible = true;
                    }
                    else
                    {
                        lblMensagem.Text = retorno.Errors.ToString().Replace("\n", " ");

                        lblMensagemErro.Text = retorno.Errors.ToString().Replace("\n", " ");
                        lblMensagemErro.Visible = true;
                    }
                }
                else
                {
                    string queryString = MontarQueryString(retorno.Message);
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
                    Response.Redirect("ListarTurma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
            }
        }

        /// <summary>
        /// Redireciona para página de Horário Operacional
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPagHorOper_Click(object sender, EventArgs e)
        {
            string _curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
            string _turno = ddlTurno.SelectedValue;

            string _unidadeEns = tseUnidadeResponsavel.DBValue.ToString();
            string _unidadeFis = tseUnidadeFisica.DBValue.ToString();
            decimal serie = 0M;

            Decimal.TryParse(ddlSerie.SelectedValue, out serie);

            if (ObjetoTurma != null && !string.IsNullOrEmpty(ObjetoTurma.Curriculo))
            {
                string _gradeID = RN.HorarioOperacional.ObterGradeID(_curso, _turno, ObjetoTurma.Curriculo, _unidadeEns, _unidadeFis, serie);

                if (_gradeID != string.Empty)
                {
                    string tipoOperacao = string.Empty;

                    if (_tipoOperacao == TipoOperacao.Novo)
                        tipoOperacao = "NOVO";
                    else if (_tipoOperacao == TipoOperacao.Alterar)
                        tipoOperacao = "ALTERAR";
                    else if (_tipoOperacao == TipoOperacao.Consultar)
                        tipoOperacao = "CONSULTAR";
                    else if (_tipoOperacao == TipoOperacao.Excluir)
                        tipoOperacao = "EXCLUIR";

                    string queryString = "grade_id=" + _gradeID + "&tipoOperacao=" + tipoOperacao;
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                    string chaveAtual = Request.QueryString["Chave"];

                    Response.Redirect("HorarioOperacional.aspx?Chave=" + Convert.ToBase64String(bytesToEncode) + "&ChaveOrigem=" + chaveAtual);
                }
                else
                {
                    lblMensagem.Text = "Não existe Grade Série para esses filtros. <br>Portanto não pode ser redirecionado para a página de Horário Operacional.";
                }
            }
            else
            {
                Response.Redirect("HorarioOperacional.aspx");
            }
        }

        /// <summary>
        /// Salva as alterações do Quadro de Horários e dos Dados da Turma
        /// </summary>        
        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            lblMensagem.Text = string.Empty;
            Ly_turma dtTurma = new Ly_turma();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
            RN.Agenda.Evento rnEvento = new RN.Agenda.Evento();
            List<RN.Agenda.Entidades.Evento> eventos = new List<RN.Agenda.Entidades.Evento>();
            int idEventoConfirmacaoTurnosVagas = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.ConfirmacaoTurnosVagas);
            DateTime dataFim;
            int minimoAlunosReforco = 0;
            int maximoAlunosReforco = 0;
            RN.Turma rnTurma = new Techne.Lyceum.RN.Turma();
            RN.Curso rnCurso = new Techne.Lyceum.RN.Curso();
            RN.Grade rnGrade = new Grade();
            lblMensagemTurmaDependencia.Text = string.Empty;

            if (_tipoOperacao == TipoOperacao.Novo)
            {
                if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                {
                    //caso esteja sendo realizada uma inclusao, verifica se está em período de Confirmação de Turnos e Vagas
                    eventos = rnEvento.ListaEventoAbertoPor(idEventoConfirmacaoTurnosVagas, DateTime.Today, int.Parse(ddlAno.SelectedValue), int.Parse(ddlPeriodo.SelectedValue));
                    if (eventos.Count != 0)
                    {
                        //Caso exista emitir mensagem de aviso
                        dataFim = eventos.Max(x => x.DataFim);

                        var script = string.Format(@"alert('Não é possível criar turmas durante período de Confirmação de Turnos e Vagas que termina em {0}.');",
                           dataFim.ToString("dd/MM/yyyy"));
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                        return;
                    }


                }
                else
                {
                    var script = string.Format(@"alert('Não é possível criar turmas sem selecionar ano/período');");
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    return;

                }
            }

            if (_tipoOperacao == TipoOperacao.Reativar)
            {
                if (ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() || ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() || ddlDependencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() || ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    var script = string.Format(@"alert('Não é possível reativar turmas sem selecionar ano/período/sala de aula/turno');");
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    return;
                }
                else
                {
                    if (ddlOptativaReforco.SelectedValue == "N" & !chkEletiva.Checked)
                    {
                        if (rnTurma.PossuiTurmaAbertaMesmaSalaETurnoPor(Convert.ToDecimal(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlDependencia.SelectedValue, ddlTurno.SelectedValue, tseUnidadeResponsavel.DBValue.ToString()))
                        {
                            var script = @"alert('" + "Não é possível reativar a turma, pois existe turma aberta na mesma sala/turno." +
                                          @"');";

                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                            return;
                        }
                    }
                }
            }
            //preenche o datatable de turma com os dados da tela
            ObterDados(dtTurma);

            if (ddlCurso.SelectedItem.Value.ToString() != "0001.18" && ddlCurso.SelectedItem.Value.ToString() != "9999.02" && ddlCurso.SelectedItem.Value.ToString() != "0001.19" && ddlCurso.SelectedItem.Value.ToString() != "0001.17" && ddlCurso.SelectedItem.Value.ToString() != "0001.27" & ddlCurso.SelectedItem.Value.ToString() != "0002.37")
            {
                NumMaxAluno = RetornaNumeroMaximoAluno();

                if (txtNumMaxAluno.Enabled && Convert.ToInt32(txtNumMaxAluno.Text) > NumMaxAluno && ddlOptativaReforco.SelectedValue != "S")
                {
                    var script = @"alert('" + "Não é permitido incluir capacidade da sala diferente de " + NumMaxAluno + " alunos para esta turma." +
                                 @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(ddlDependencia.SelectedValue) && chkAmbienteExterno.Checked)
            {
                var script = @"alert('" + "Não é permitido que a turma esteja realizando atividade em ambiente externo e ter sala de aula selecionada." +
                                    @"');";

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                return;
            }

            if (ddlOptativaReforco.SelectedItem.Value != "N")
            {
                string curso = string.Empty, turno = string.Empty, curriculo = string.Empty;
                decimal? ano = null, semestre = null;
                decimal serie = decimal.MinValue;

                QueryTable resultado = null;

                if (ddlCurso.Items.Count > 0)
                    curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
                if (ddlTurno.Items.Count > 0)
                    turno = ddlTurno.SelectedValue;
                if (ddlAno.Items.Count > 0)
                    ano = Convert.ToDecimal(ddlAno.SelectedValue);
                if (ddlPeriodo.Items.Count > 0 && !String.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                    semestre = Convert.ToDecimal(ddlPeriodo.SelectedValue);
                if (ddlCurriculo.Items.Count > 0 && !String.IsNullOrEmpty(ddlCurriculo.SelectedValue))
                    curriculo = ddlCurriculo.SelectedItem.Text;
                if (ddlSerie.Items.Count > 0 && !String.IsNullOrEmpty(ddlSerie.SelectedValue))
                    serie = Convert.ToDecimal(ddlSerie.SelectedValue);
                if (!String.IsNullOrEmpty(curso) && !String.IsNullOrEmpty(turno) && ano.HasValue && semestre.HasValue)
                    resultado = RN.Curriculo.ObtemGradePor(curso, turno, curriculo, serie);

                if (resultado.Rows.Count > 1)
                {
                    var script = @"alert('" + "A turma (Optativa/Reforço) deverá ter somente 1 (um) componente curricular." +
                                @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    return;
                }

                ////Verifica se é uma turma de reforço
                if (ddlOptativaReforco.SelectedValue == "S")
                {
                    //Busca minimo de alunos para turma reforço
                    minimoAlunosReforco = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MinAlunoReforcoPorTurma"]);

                    //Busca maximo de alunos para turma reforço
                    maximoAlunosReforco = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaxAlunoReforcoPorTurma"]);

                    //Se for turma de reforço valida se ela possui capacidade entre o minimo e maximo de alunos para turmas de reforço
                    if (Convert.ToInt32(txtNumMaxAluno.Text) < minimoAlunosReforco || Convert.ToInt32(txtNumMaxAluno.Text) > maximoAlunosReforco)
                    {
                        var script = @"alert('" + string.Format("A turma de reforço deverá ter capacidade de no mínimo {0} e no máximo {1} alunos.", minimoAlunosReforco, maximoAlunosReforco) +
                                    @"');";
                        lblMensagem.Text = string.Format("A turma de reforço deverá ter capacidade de no mínimo {0} e no máximo {1} alunos.", minimoAlunosReforco, maximoAlunosReforco);
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                        return;
                    }
                }

            }
            if (ddlCurso.SelectedItem.Value.ToString() != "9999.92" && chkAmbienteExterno.Checked == false && string.IsNullOrEmpty(ddlDependencia.SelectedValue))
            {
                lblMensagem.Text = "O campo Sala de Aula é de preenchimento obrigatório.";
                return;
            }

            //Verifica se o curso escolhido é o mais educação
            if (ddlCurso.SelectedItem.Value.ToString() == "9999.92")
            {
                if (Convert.ToInt32(txtNumMaxAluno.Text) < 20 || Convert.ToInt32(txtNumMaxAluno.Text) > 30)
                {
                    var script = @"alert('" + "A turma deverá ter capacidade de no mínimo 20 e no máximo 30 alunos." +
                                @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    return;
                }
            }

            if (ddlCurso.SelectedItem.Value.ToString() == "9999.01" || ddlCurso.SelectedItem.Value.ToString() == "9999.03")
            {
                if (Convert.ToInt32(txtNumMaxAluno.Text) > 35)
                {
                    var script = @"alert('" + "A turma deverá ter capacidade no máximo 35 alunos." +
                                @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    return;
                }
            }

            if (ddlCurso.SelectedItem.Value.ToString() == "9999.02")
            {
                if (Convert.ToInt32(txtNumMaxAluno.Text) > 100)
                {
                    var script = @"alert('" + "A turma deverá ter capacidade no máximo 100 alunos." +
                                @"');";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    return;
                }
            }

            RetValue retorno = null;

            var macro = string.Empty;
            if (ddlCurso.Value != null)
            {
                //Verifica se o curso escolhido é o mais educação
                if (ddlCurso.Value.ToString() == "9999.92")
                {
                    string curso = string.Empty;
                    string turno = string.Empty;
                    string curriculo = string.Empty;
                    int serie = 0;

                    if (ddlCurso.Items.Count > 0)
                    {
                        curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
                    }
                    if (ddlTurno.Items.Count > 0)
                    {
                        turno = ddlTurno.SelectedValue;
                    }

                    if (ddlCurriculo.Items.Count > 0 && !String.IsNullOrEmpty(ddlCurriculo.SelectedValue))
                    {
                        curriculo = ddlCurriculo.SelectedItem.Text;
                    }
                    if (ddlSerie.Items.Count > 0 && !String.IsNullOrEmpty(ddlSerie.SelectedValue))
                    {
                        serie = Convert.ToInt32(ddlSerie.SelectedValue);
                    }

                    foreach (ListItem item in chkMacros.Items)
                    {
                        if (item.Selected && !string.IsNullOrEmpty(item.Value))
                        {
                            //Verifica se o macro possui disciplinas obrigatorias
                            if (!rnCurriculo.ExisteDisciplinaObrigatoriaPor(curriculo, curso, turno, serie, Convert.ToInt32(item.Value)))
                            {
                                //Caso um dos macros escolhidos não possua disciplinas obrigatorias, impedir inclusao
                                lblMensagem.Text = string.Format("O Macro {0} não possui componente curricular obrigatório, por isso não pode ser utilizado.", item.Text);
                                return;
                            }

                            macro += item.Value;
                            macro += ",";
                        }
                    }

                    if (string.IsNullOrEmpty(macro))
                    {
                        lblMensagem.Text = "O campo Macro é de preenchimento obrigatório para o Curso Mais Educação.";
                        return;
                    }
                }
            }
            if (string.IsNullOrEmpty(txtNumMaxAluno.Text))
            {
                lblMensagem.Text = "O campo Número Máximo de Alunos à Enturmar é de preenchimento obrigatório.";
                return;
            }

            if (!string.IsNullOrEmpty(macro))
                macro = macro.ToString().Substring(0, macro.ToString().Length - 1);

            //Verifica se é turma eletiva
            if (chkEletiva.Checked)
            {
                if (ddlCurso.Value.ToString() != "9999.80")
                {
                    lblMensagem.Text = "O curso selecionado não permite eletiva.";
                    return;
                }

                //Busca grupo da matriz da eletiva
                int grupo = rnCurriculo.ObtemGrupoEletivaPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), Convert.ToInt32(ddlSerie.SelectedValue), ddlCurriculo.SelectedValue);


                if (ddlTurmaReferencia.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "Para turma eletiva é necessário informar a Turma Referência.";
                    return;
                }
                else if (_tipoOperacao == TipoOperacao.Novo)
                {
                    //Bsuca dados da turma referencia
                    var dadosReferencia = rnTurma.ObtemDadosTurmaPor(ddlTurmaReferencia.SelectedValue, Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue));

                    //Verifica se a eletiva´não é do grupo 1
                    if (grupo != 1)
                    {
                        //Busca quantidade de turmas por turno / curso / serie
                        int qtdeTurmas = rnTurma.ObtemQuantidadeTurmaPor(dadosReferencia.Ano, dadosReferencia.Periodo, dadosReferencia.Censo, dadosReferencia.Curso, dadosReferencia.Serie, dadosReferencia.Turno);

                        //Caso não seja eletiva do grupo 1, verifica se tem mais de 1 turma
                        if (qtdeTurmas != 1)
                        {
                            lblMensagem.Text = "Para criar turma eletiva 2 ou 3 é necessário que exista apenas uma turma para o ano / período / curso / serie no turno.";
                            return;
                        }
                    }
                }

                int numeroMaxAluno = rnTurma.ObtemNumeroMaximoAlunosPor(Convert.ToDecimal(ddlAno.SelectedValue), Convert.ToDecimal(ddlPeriodo.SelectedValue), ddlTurmaReferencia.SelectedValue);

                if (numeroMaxAluno != Convert.ToInt32(txtNumMaxAluno.Text))
                {
                    lblMensagem.Text = "O número máximo de alunos a enturmar não pode ser diferente do número de alunos da Turma Referência.";
                    return;
                }

                if (_tipoOperacao.Equals(TipoOperacao.Novo))
                {
                    if (grupo <= 0)
                    {
                        lblMensagem.Text = "A matriz curricular escolhida não possui disciplinas eletivas com o grupo informado.";
                        return;
                    }

                    //Validar se já existe outra Eletiva do mesmo grupo para a mesma turma referencia
                    if (rnTurma.PossuiTurmaEletivaPor(Convert.ToDecimal(ddlAno.SelectedValue), Convert.ToDecimal(ddlPeriodo.SelectedValue), ddlTurmaReferencia.SelectedValue, grupo))
                    {
                        lblMensagem.Text = "Já existe uma turma eletiva deste grupo para esta Turma Referência.";
                        return;
                    }
                }
            }

            //Cria uma nova turma de acordo com dados preenchidos na tela
            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                if (!rnGrade.PossuiGradePor(ddlCurso.Value.ToString(), ddlTurno.SelectedValue, ddlCurriculo.SelectedValue, Convert.ToDecimal(ddlSerie.SelectedValue)))
                {
                    lblMensagem.Text = "Não há disciplinas cadastradas na matriz curricular.";
                    return;
                }

                //VERIFICA SE A SALA ESCOLHIDA POSSUI TURMA DESATIVADA
                if (hdnValidaDependTurma.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    RetValue valorRetorno = null;
                    var turmaDepend = rnTurma.RetornaTurmaDesativadaMesmaSalaETurnoPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlDependencia.SelectedValue, ddlTurno.SelectedValue, tseUnidadeFisica.DBValue.ToString());
                    if (!turmaDepend.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMensagemTurmaDependencia.Text = "Esta sala de aula possui a(s) turma(s) " + turmaDepend + "  desativada(s). Deseja continuar?";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopupTurmaDependencia();", true);
                        retorno = new RetValue(false, "Verifique", null);
                    }
                }

                if (lblMensagemTurmaDependencia.Text == string.Empty)
                {
                    retorno = RN.Turma.IncluirTurmaComQuadroHorario(dtTurma, macro);
                }
            }
            //Salva alterações na turma e no quadro de horários
            else if (_tipoOperacao.Equals(TipoOperacao.Alterar))
            {
                Ly_hor_aula dtHoraAula = new Ly_hor_aula();
                Ly_aula_docente dtAulaDocente = new Ly_aula_docente();
                //preenche os datatables com os dados do quadro de horário
                ObterDadosTabela(dtHoraAula, dtAulaDocente);
                retorno = RN.Turma.AlterarTurmaComQuadroHorario(ObjetoTurmaInicial, tbTurma2.Value, dtTurma, dtHoraAula, dtAulaDocente);
            }
            else if (_tipoOperacao.Equals(TipoOperacao.Reativar))
            {
                retorno = rnTurma.ReativaTurma(Convert.ToDecimal(ddlAno.SelectedValue), Convert.ToDecimal(ddlPeriodo.SelectedValue), ddlTurma.SelectedValue, ddlDependencia.SelectedValue, Convert.ToInt32(txtNumMaxAluno.Text));

            }

            //Tratamento das mensagens/erros retornados
            if (retorno != null && retorno.Ok)
            {
                //caso esteja sendo realizada uma inclusao, sera alterado o tipo de operacao para alteracao
                if (_tipoOperacao == TipoOperacao.Novo)
                {
                    _tipoOperacao = TipoOperacao.Alterar;
                    ObjetoTurma.Grade = dtTurma.Rows[0].Turma;
                    CarregarDadosDrop(ddlTurma.ID);
                    tbTurma2.Value = ObjetoTurma.Grade;
                    DateTime? dt_criacao = dtTurma.Rows[0].Dt_criacao;
                    lblCriacaoValor.Text = dt_criacao.HasValue ? (dt_criacao.Value.ToString("dd/MM/yyyy HH") + "h" + dt_criacao.Value.ToString("mm") + "min") : "--/--/----";
                    lblAlunoMatriculaValor.Text = "0";
                    lblAlunoMatriculaProgressaoValor.Text = "0";
                    lblAlunoMatriculaEletiva1Valor.Text = "0";
                    lblAlunoMatriculaEletiva2Valor.Text = "0";
                    lblAlunoMatriculaEletiva3Valor.Text = "0";
                }

                ObjetoTurmaInicial.Turno = ObjetoTurma.Turno;
                ObjetoTurmaInicial.Ano = ObjetoTurma.Ano;
                ObjetoTurmaInicial.Curriculo = ObjetoTurma.Curriculo;
                ObjetoTurmaInicial.Curso = ObjetoTurma.Curso;
                ObjetoTurmaInicial.Faculdade = ObjetoTurma.Faculdade;
                ObjetoTurmaInicial.Grade = ObjetoTurma.Grade;
                ObjetoTurmaInicial.Grade_ID = Convert.ToString(RN.Turma.ObterGrade(dtTurma.Rows[0]));
                ObjetoTurmaInicial.Municipio = ObjetoTurma.Municipio;
                ObjetoTurmaInicial.Nucleo = ObjetoTurma.Nucleo;
                ObjetoTurmaInicial.Periodo = ObjetoTurma.Periodo;
                ObjetoTurmaInicial.Serie = ObjetoTurma.Serie;
                ObjetoTurmaInicial.Sufixo = ObjetoTurma.Sufixo;
                ObjetoTurmaInicial.Turno = ObjetoTurma.Turno;
                ObjetoTurmaInicial.UnidadeResponsavel = ObjetoTurma.UnidadeResponsavel;
                ObjetoTurmaInicial.Dependencia = ObjetoTurma.Dependencia;

                ControlarTipoOperacao();

                lblMensagemErro.Visible = true;

                hControleCelula.Value = string.Empty;
                LimparTable();
                PopularTable();
            }

            //controle para cores das celulas alteradas
            ControlarCorPostCelula();
            MontarMensagemTela(retorno);
            CarregarDadosDrop(ddlDisciplinaQuadroHorario.ID);
            CarregarDadosDrop(ddlDisciplinaQuadroHorario2.ID);

            if (!retorno.Ok && retorno != null)
            {
                lblMensagem.Text = "Aviso: as alterações realizadas apresentaram inconsistências e por isso não foram salvas. Favor, realize a correção dos horários sinalizados e salve novamente. " + retorno.Message.ToString() + " " + retorno.Errors.ToString();
            }
            else
            {
                lblCursoDesc.Text = ddlCurso.SelectedItem.Text;
                tbTurma2.Value = dtTurma.Rows[0].Turma;
                CarregarDadosDrop(ddlTurma.ID);
                ddlTurmasUnidadeDev.Value = dtTurma.Rows[0].Turma;
                ddlTurmasUnidadeDev.Visible = true;
                ddlTurmasUnidadeDev.DataBind();
            }
        }

        /// <summary>
        /// Força o salvamento das alterações (utilizado para chamada por Javascript)
        /// </summary>        
        protected void btnSalvarClient_Click(object sender, EventArgs e)
        {
            btnSalvar_Click(sender, null);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Adiciona no dropdownlist passado como parametro um item vazio
        /// </summary>
        /// <param name="drop">Dropdownlist</param>
        /// <param name="selecionado">se o item vazio deve ser selecionado</param>
        private void CriarItemVazio(DropDownList drop, bool selecionado)
        {
            ListItem itemVazio = new ListItem("Selecione", "");
            if (!drop.Items.Contains(itemVazio))
                drop.Items.Add(itemVazio);

            if (selecionado)
            {
                drop.ClearSelection();
                //seleciona o item vazio
                drop.Items.FindByValue("").Selected = true;
            }
        }

        private void CriarItemVazio(ASPxComboBox drop, bool selecionado)
        {
            ListEditItem itemVazio = new ListEditItem("Selecione", "");
            if (drop.Items.FindByValue("") == null)
                drop.Items.Add(itemVazio);

            if (selecionado)
            {
                drop.SelectedItem = null;
                //seleciona o item vazio
                drop.Items.FindByValue("").Selected = true;
            }
        }

        private void CarregarGridMatriculaAluno()
        {
            DataTable qtMatricula = null;
            DataTable qtMatriculaEletiva = null;

            if (ObjetoTurma != null)
            {
                if (!string.IsNullOrEmpty(ObjetoTurma.Grade)
                    && !string.IsNullOrEmpty(ObjetoTurma.UnidadeResponsavel)
                    && !string.IsNullOrEmpty(ObjetoTurma.Ano)
                    && !string.IsNullOrEmpty(ObjetoTurma.Periodo)
                    && !string.IsNullOrEmpty(ObjetoTurma.Curso)
                    && !string.IsNullOrEmpty(ObjetoTurma.Turno)
                    && !string.IsNullOrEmpty(ObjetoTurma.Curriculo)
                    && !string.IsNullOrEmpty(ObjetoTurma.Serie)
                    && !string.IsNullOrEmpty(ObjetoTurma.Grade_ID))
                {

                    var ativo = RN.Turma.VerificaSituacaoAtiva(ObjetoTurma.Grade, Convert.ToDecimal(ObjetoTurma.Ano), Convert.ToDecimal(ObjetoTurma.Periodo));

                    //Busca dados das turmas principais e eletivas
                    if (ativo)
                    {
                        qtMatricula = RN.Aluno.ConsultarMatriculaPrincipalPorTurma(ObjetoTurma.Grade, ObjetoTurma.UnidadeResponsavel,
                                                                     Convert.ToDecimal(ObjetoTurma.Ano),
                                                                     Convert.ToDecimal(ObjetoTurma.Periodo),
                                                                     ObjetoTurma.Curso, ObjetoTurma.Turno,
                                                                     ObjetoTurma.Curriculo,
                                                                     Convert.ToDecimal(ObjetoTurma.Serie));

                        qtMatriculaEletiva = RN.Aluno.ConsultarMatriculaEletivaPorTurma(ObjetoTurma.Grade, ObjetoTurma.UnidadeResponsavel,
                                                                     Convert.ToDecimal(ObjetoTurma.Ano),
                                                                     Convert.ToDecimal(ObjetoTurma.Periodo),
                                                                     ObjetoTurma.Curso, ObjetoTurma.Turno,
                                                                     ObjetoTurma.Curriculo,
                                                                     Convert.ToDecimal(ObjetoTurma.Serie));
                    }
                    else
                    {
                        qtMatricula = RN.Aluno.ConsultarMatriculaPrincipalPorTurmaHistorico(ObjetoTurma.Grade,
                                                                                   ObjetoTurma.UnidadeResponsavel,
                                                                                   Convert.ToDecimal(ObjetoTurma.Ano),
                                                                                   Convert.ToDecimal(ObjetoTurma.Periodo),
                                                                                   ObjetoTurma.Curso, ObjetoTurma.Turno,
                                                                                   ObjetoTurma.Curriculo,
                                                                                   Convert.ToDecimal(ObjetoTurma.Serie));

                        qtMatriculaEletiva = RN.Aluno.ConsultarMatriculaEletivaPorTurmaHistorico(ObjetoTurma.Grade,
                                                                                  ObjetoTurma.UnidadeResponsavel,
                                                                                  Convert.ToDecimal(ObjetoTurma.Ano),
                                                                                  Convert.ToDecimal(ObjetoTurma.Periodo),
                                                                                  ObjetoTurma.Curso, ObjetoTurma.Turno,
                                                                                  ObjetoTurma.Curriculo,
                                                                                  Convert.ToDecimal(ObjetoTurma.Serie));
                    }
                }
            }

            //Apenas exibe grid de Matricula Principal caso a turma nao seja de eletiva
            if (qtMatricula != null && qtMatricula.Rows.Count > 0)
            {
                //Alimenta grid Principal
                grdMatriculaAluno.DataSource = qtMatricula;
                grdMatriculaAluno.DataBind();
                grdMatriculaAluno.Visible = true;
            }
            else
            {
                grdMatriculaAluno.DataSource = null;
                grdMatriculaAluno.Visible = false;
                grdMatriculaAluno.DataBind();
            }

            //Apenas exibe grid de Eletivas caso a turma tenha eletivas
            if (qtMatriculaEletiva != null && qtMatriculaEletiva.Rows.Count > 0)
            {
                //Alimenta grid Eletivas
                grdMatriculaAlunoEletiva.DataSource = qtMatriculaEletiva;
                grdMatriculaAlunoEletiva.DataBind();
                grdMatriculaAlunoEletiva.Visible = true;
            }
            else
            {
                grdMatriculaAlunoEletiva.DataSource = null;
                grdMatriculaAlunoEletiva.Visible = false;
                grdMatriculaAlunoEletiva.DataBind();
            }
        }

        private void PopularNumeroAlunoDependencia()
        {
            DataTable valorDependencia = new DataTable();
            txtNumMaxAluno.Text = string.Empty;
            txtCapacidadeSala.Text = string.Empty;
            txtCapacidadeTurma.Text = string.Empty;

            if ((ddlCurso.Items.Count > 0) && (ddlCurso.SelectedItem.Text != "Selecione"))
            {
                valorDependencia = (DataTable)ddlDependencia.DataSource;
            }

            txtNumMaxAluno.Text = RetornaNumeroMaximoAluno().ToString();

            if ((ddlCurso.Items.Count > 0) && (ddlCurso.SelectedItem.Text != "Selecione"))
            {
                if (valorDependencia != null)
                {
                    if (!string.IsNullOrEmpty(ddlDependencia.SelectedValue))
                    {
                        if ((!valorDependencia.Rows[ddlDependencia.SelectedIndex].Field<string>("tipo_depend").Equals("SALA"))
                        && (!valorDependencia.Rows[ddlDependencia.SelectedIndex].Field<string>("tipo_depend").Equals("SALAAEE")))
                        {
                            txtNumMaxAluno.Enabled = false;
                            txtNumMaxAluno.ReadOnly = true;
                        }
                    }
                    else
                    {
                        txtNumMaxAluno.Enabled = false;
                        txtNumMaxAluno.ReadOnly = true;
                    }
                }
            }

            if (ddlCurso.SelectedItem.Value.Equals("9999.92"))
            {
                txtNumMaxAluno.Text = "30";
                txtNumMaxAluno.Enabled = false;
                txtNumMaxAluno.ReadOnly = true;
            }


            if (ddlCurso.SelectedItem.Value.Equals("9999.01") || ddlCurso.SelectedItem.Value.Equals("9999.03"))
            {
                txtNumMaxAluno.Text = "35";
                txtCapacidadeTurma.Text = "35";
                txtNumMaxAluno.Enabled = false;
                txtNumMaxAluno.ReadOnly = true;
            }


            if (ddlCurso.SelectedItem.Value.Equals("9999.02"))
            {
                txtNumMaxAluno.Text = "100";
                txtCapacidadeTurma.Text = "100";
                txtNumMaxAluno.Enabled = false;
                txtNumMaxAluno.ReadOnly = true;
            }



            string sala = ddlDependencia.SelectedItem.Value.ToString();
            RN.Perfil rnPerfil = new Perfil();
            RN.Usuarios rnUsuarios = new Usuarios();

            if (!string.IsNullOrEmpty(sala))
            {
                if (sala.Substring(0, 2).Equals("SA") && ddlOptativaReforco.SelectedValue != "S" && !rnUsuarios.EhPrivilegiado(User.Identity.Name) && !rnPerfil.PossuiPerfilAlteraMaximoAlunoPor(User.Identity.Name))
                {
                    txtNumMaxAluno.Enabled = false;
                    txtNumMaxAluno.ReadOnly = true;
                }
                else
                {
                    txtNumMaxAluno.Enabled = true;
                    txtNumMaxAluno.ReadOnly = false;
                }
            }
        }

        private void ControlarCorPostCelula()
        {
            foreach (TableRow tr in tQuadroHorario.Rows)
            {
                if (tr.TableSection == TableRowSection.TableBody)
                {
                    foreach (TableCell td in tr.Cells)
                    {
                        if (td.Controls.Count > 3)
                        {
                            HtmlInputText txtDocente = null;
                            HtmlInputText txtDisciplina = null;

                            //verifica se o controle de indice 0 é um textbox
                            if (td.Controls[0] is HtmlInputText)
                                //cast do textbox de docente encontrado na célula (representado pelo indice 0)
                                txtDocente = (HtmlInputText)td.Controls[0];

                            if (td.Controls[2] is HtmlInputText)
                                txtDisciplina = (HtmlInputText)td.Controls[2];

                            ControlarCorControleCelula(txtDocente, txtDisciplina);
                        }
                    }
                }
            }
        }

        protected bool VerificarCheck(object valor)
        {
            if (valor is DBNull)
            {
                return false;
            }

            if (valor is string)
            {
                return (string)valor == "S";
            }

            return false;
        }

        private List<String> OrdenarErrorList(RetValue retorno)
        {
            List<RN.Turma.TurmaError> lista = RN.Turma.TransformarErrorList(retorno);

            var errosGLPValidacaoSorted = from msg in lista
                                          where msg.DiaDaSemana != 0
                                          orderby msg.DiaDaSemana, msg.HoraInicio, msg.HoraFim, msg.Mensagem
                                          select ObterDiaSemanaPorNumero(msg.DiaDaSemana.ToString()) + " " +
                                          msg.HoraInicio.ToString("HH:mm") + " / " +
                                          msg.HoraFim.ToString("HH:mm") + " - " + msg.Mensagem;
            var errosGenericosSorted = from msg in lista
                                       where msg.DiaDaSemana == 0
                                       orderby msg.Mensagem
                                       select msg.Mensagem;

            List<String> msgs = new List<String>();
            msgs.AddRange(errosGenericosSorted);
            msgs.AddRange(errosGLPValidacaoSorted);
            return msgs;
        }

        private void MontarMensagemTela(Techne.Lyceum.RN.RetValue retorno)
        {
            List<string> lista = new List<string>();

            if (retorno != null)
            {
                string mensagemErro = string.Empty;

                if (retorno.Errors["ERRO_VALIDACAO"].Length > 0)
                {
                    foreach (string erro in retorno.Errors["ERRO_VALIDACAO"])
                        mensagemErro += ObterDiaSemanaPorNumero(erro.Split('|')[1]) + " " + Convert.ToDateTime(erro.Split('|')[3]).ToString("HH:mm") + " / " + Convert.ToDateTime(erro.Split('|')[4]).ToString("HH:mm") + " - " + erro.Split('|')[2] + "<br>";

                    if (string.IsNullOrEmpty(retorno.Message))
                        lblMensagem.Text = " - A página contém erros. Favor clicar na imagem acima do quadro de horários para visualizar.";
                    else
                    {
                        if (retorno.Errors != null && !string.IsNullOrEmpty(retorno.Errors.ToString()))
                            lblMensagem.Text = " - " + retorno.Message + " - A página contém erros. Favor clicar na imagem acima do quadro de horários para visualizar. ";
                        else
                            lblMensagem.Text = " - " + retorno.Message;
                    }

                    lblMensagemErro.Text = mensagemErro;
                    lblMensagemErro.Visible = true;

                    lista.AddRange(retorno.Errors["ERRO_VALIDACAO"]);
                }

                if (retorno.Errors["ERRO"].Length > 0)
                {
                    foreach (string erro in retorno.Errors["ERRO"])
                        mensagemErro += erro + "<br>";

                    if (string.IsNullOrEmpty(retorno.Message))
                        lblMensagem.Text = " - A página contém erros. Favor clicar na imagem acima do quadro de horários para visualizar.";
                    else
                    {
                        if (retorno.Errors != null && !string.IsNullOrEmpty(retorno.Errors.ToString()))
                            lblMensagem.Text = " - " + retorno.Message + " - A página contém erros. Favor clicar na imagem acima do quadro de horários para visualizar. ";
                        else
                            lblMensagem.Text = " - " + retorno.Message;
                    }

                    lblMensagemErro.Text = mensagemErro;
                    lblMensagemErro.Visible = true;
                }


                //se existir algum erro será mostrado em cada célula o motivo
                //somente será mostrado se é permitido glp caso ocorra algum erro
                if (retorno.Errors["ERRO_GLP"].Length > 0)
                {
                    lista.AddRange(retorno.Errors["ERRO_GLP"]);

                    foreach (string erro in retorno.Errors["ERRO_GLP"])
                        mensagemErro += ObterDiaSemanaPorNumero(erro.Split('|')[1]) + " " + Convert.ToDateTime(erro.Split('|')[3]).ToString("HH:mm") + " / " + Convert.ToDateTime(erro.Split('|')[4]).ToString("HH:mm") + " - " + erro.Split('|')[2] + "<br>";

                    //if (retorno.Errors["INFORMA_GLP"].Length > 0)
                    //    lista.AddRange(retorno.Errors["INFORMA_GLP"]);

                    if (string.IsNullOrEmpty(retorno.Message))
                        lblMensagem.Text = " - A página contém erros. Favor clicar na imagem acima do quadro de horários para visualizar.";
                    else
                    {
                        if (retorno.Errors != null && !string.IsNullOrEmpty(retorno.Errors.ToString()))
                            lblMensagem.Text = " - " + retorno.Message + " - A página contém erros. Favor clicar na imagem acima do quadro de horários para visualizar. ";
                        else
                            lblMensagem.Text = " - " + retorno.Message;
                    }

                    lblMensagemErro.Text = mensagemErro;
                    lblMensagemErro.Visible = true;
                }

                MontarMensagemTabela(lista, false);

                if (string.IsNullOrEmpty(retorno.Message))
                    lblMensagem.Text = " - " + retorno.Errors.ToString();
                else
                    lblMensagem.Text = " - " + retorno.Message;

                lblMensagemErro.Text = "";
                foreach (String msgErro in OrdenarErrorList(retorno))
                    lblMensagemErro.Text += msgErro + "<br/>";
            }
            else
            {
                if (string.IsNullOrEmpty(retorno.Message))
                    lblMensagem.Text = " - " + retorno.Errors.ToString();
                else
                    lblMensagem.Text = " - " + retorno.Message;
            }

            //Apenas mantém a mensagem do lblMensagem caso a TabPage ativa seja "Geral"
            if (pcTurma.ActiveTabPage.Name != "Geral")
            {
                lblMensagem.Text = "";

                if (retorno != null && String.IsNullOrEmpty(lblMensagemErro.Text))
                    lblMensagemErro.Text += "<br/>" + retorno.Message + "<br/>";
            }
        }

        private void MontarMensagemTabela(List<string> listaInformacao, bool informativo)
        {
            RN.Docentes rnDocente = new Docentes();
            hControleCelula.Value = "";

            foreach (TableRow tr in tQuadroHorario.Rows.Cast<TableRow>().Where(tr => !(tr is TableHeaderRow)))
            {
                string aula = string.Empty;
                string id = string.Empty;
                string horaIni = string.Empty;
                string horaFim = string.Empty;

                foreach (TableCell td in tr.Cells)
                {
                    //é a primeira célula e será obtido a aula
                    if (td.Controls.Count == 2)
                    {
                        if (td.Controls[0] is HtmlInputHidden)
                        {
                            HtmlInputHidden hAula = (HtmlInputHidden)td.Controls[0];
                            if (hAula != null)
                                aula = hAula.Value;

                            if (td.Controls[1] is LiteralControl)
                            {
                                LiteralControl horario = (LiteralControl)td.Controls[1];
                                if (horario != null)
                                {
                                    horaIni = horario.Text.Split('/')[0].Trim();
                                    horaFim = horario.Text.Split('/')[1].Trim();
                                    id = aula + "_" + horaIni.Replace(":", "_") + "_" + horaFim.Replace(":", "_");
                                }
                            }
                        }
                    }
                    else if (td.Controls.Count > 2)
                    {
                        if (!string.IsNullOrEmpty(aula))
                        {
                            HtmlInputText txtDocente = null;
                            HtmlInputText txtDisciplina = null;
                            HtmlInputHidden hNumFunc = null;
                            HtmlInputHidden hCodDisciplina = null;

                            //verifica se o controle de indice 0 é um textbox
                            if (td.Controls[0] is HtmlInputText)
                                //cast do textbox de docente encontrado na célula (representado pelo indice 0)
                                txtDocente = (HtmlInputText)td.Controls[0];

                            //verifica se o controle de indice 2 é um textbox
                            if (td.Controls[2] is HtmlInputText)
                                //cast do textbox de docente encontrado na célula (representado pelo indice 2)
                                txtDisciplina = (HtmlInputText)td.Controls[2];

                            if (td.Controls[4] is HtmlInputHidden)
                                hNumFunc = (HtmlInputHidden)td.Controls[4];

                            if (td.Controls[5] is HtmlInputHidden)
                                hCodDisciplina = (HtmlInputHidden)td.Controls[5];

                            //verifica se a lista de erro/informação contém mais de um registro
                            if (listaInformacao.Count > 0)
                            {
                                //verifica se os controles de docente e disciplina existem
                                if (txtDocente != null && txtDisciplina != null)
                                {
                                    //loop na lista de erro/informação
                                    foreach (string erro in listaInformacao)
                                    {
                                        //transforma a mensagem em um array usando seu separador
                                        string[] erroQH = erro.Split('|');

                                        //verifica se retornou algum valor para o array
                                        if (erroQH != null)
                                        {
                                            //verifica se a mensagem retornou um array com o tamanho definido e válido
                                            if (erroQH.Length >= 6)
                                            {
                                                if (erroQH[0] == aula && erroQH[3] == horaIni && erroQH[4] == horaFim)
                                                {
                                                    decimal diaSemanaDocente = 0M;
                                                    if (Decimal.TryParse(erroQH[1], out diaSemanaDocente))
                                                    {
                                                        string diaSemana = ObterControleDocenteDiaSemana(diaSemanaDocente);

                                                        //verifica o dia da semana e identificador (aula, horario inicio e fim) conferem com o id do docente
                                                        if (txtDocente.ID.Contains(diaSemana + "_" + id))
                                                        {
                                                            string dia_semanaErro = erro.Split('|')[1];
                                                            string mensagemErro = erro.Split('|')[2];

                                                            if (!informativo)
                                                            {
                                                                txtDocente.Style.Add("background-color", "#FF6666");
                                                                txtDocente.Style.Add("color", "White");

                                                                txtDisciplina.Style.Add("background-color", "#FF6666");
                                                                txtDisciplina.Style.Add("color", "White");

                                                                HtmlImage img = new HtmlImage();
                                                                img.Src = "../Images/AlertaMens.gif";
                                                                //img.Alt = mensagemErro;
                                                                img.Attributes.Add("title", mensagemErro);

                                                                td.Controls.Add(img);
                                                            }
                                                            else
                                                            {
                                                                txtDocente.Style.Add("background-color", "#99FFCC");
                                                                txtDocente.Style.Add("color", "Black");

                                                                txtDisciplina.Style.Add("background-color", "#99FFCC");
                                                                txtDisciplina.Style.Add("color", "Black");
                                                            }

                                                            hControleCelula.Value += "|" + diaSemana + "_" + id;

                                                            txtDocente.Value = erroQH[5];
                                                            txtDisciplina.Value = erroQH[6];
                                                            hNumFunc.Value = rnDocente.ObtemNumFuncPor(erroQH[5].Split('-')[0]).ToString();

                                                            foreach (ListItem item in ddlDisciplinaQuadroHorario.Items)
                                                                //if (item.Text.Contains(erroQH[6])) 
                                                                //Trocado pois quando o nome de uma disciplina continha o nome da outra estava pegando codigo errado
                                                                if (item.Text == erroQH[6])
                                                                    hCodDisciplina.Value = item.Value;
                                                            //hCodDisciplina.Value = ddlDisciplinaQuadroHorario.Items.FindByText(erroQH[6]).Value;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private string MontarQueryString(string mensagem)
        {
            StringBuilder queryString = new StringBuilder();

            if (!string.IsNullOrEmpty(mensagem))
            {
                queryString.Append("mensagem=" + mensagem);
                queryString.Append("&ano=" + ddlAno.SelectedValue);
            }
            else
                queryString.Append("ano=" + ddlAno.SelectedValue);

            queryString.Append("&semestre=" + ddlPeriodo.SelectedValue);
            queryString.Append("&turno=" + ddlTurno.SelectedValue);
            if (ddlCurso.Value == null)
                queryString.Append("&curso=");
            else
                queryString.Append("&curso=" + ddlCurso.Value.ToString());
            queryString.Append("&nucleo=" + ObjetoTurma.Nucleo);
            queryString.Append("&municipio=" + ObjetoTurma.Municipio);
            queryString.Append("&serie=" + ddlSerie.SelectedValue);

            if (!string.IsNullOrEmpty(ddlTurma.SelectedValue))
                queryString.Append("&gradeId=" + ObjetoTurma.Grade_ID);

            queryString.Append("&faculdade=" + ObjetoTurma.Faculdade);
            queryString.Append("&unidadeResponsavel=" + ObjetoTurma.UnidadeResponsavel);
            queryString.Append("&turno=" + ObjetoTurma.Turno);

            return queryString.ToString();
        }

        /// <summary>
        /// Obtém os dados da classe auxiliar para o DataTable de Hora Aula
        /// </summary>
        /// <param name="dtHoraAula">DataTable de hora aula onde será armazenado o datarow com os dados da classe auxiliar</param>
        /// <param name="dadosQuadroHorario">Classe auxiliar com os dados necessários para preencher a linha de Hora Aula</param>
        private void ObterHoraAula(Ly_hor_aula dtHoraAula, Techne.Lyceum.RN.Turma.DadosQuadroHorario dadosQuadroHorario)
        {
            if (ExisteHoraAula(dtHoraAula, dadosQuadroHorario))
            {
                Ly_hor_aula.Row linhaHoraAula = dtHoraAula.NewRow();

                linhaHoraAula.Ano = dadosQuadroHorario.Ano;
                linhaHoraAula.Aula = dadosQuadroHorario.Aula;
                linhaHoraAula.Dependencia = dadosQuadroHorario.Dependencia;
                linhaHoraAula.Dia_semana = dadosQuadroHorario.DiaSemana;
                linhaHoraAula.Disciplina = dadosQuadroHorario.Disciplina;
                linhaHoraAula.Faculdade = dadosQuadroHorario.Faculdade;
                linhaHoraAula.Horaini_aula = dadosQuadroHorario.HoraIni;
                linhaHoraAula.Horafim_aula = dadosQuadroHorario.HoraFim;
                linhaHoraAula.Semestre = dadosQuadroHorario.Semestre;
                linhaHoraAula.Turma = dadosQuadroHorario.Turma;
                linhaHoraAula.Turno = dadosQuadroHorario.Turno;
                linhaHoraAula.Qtde_aula = 1;
                //linhaHoraAula.Tipo = "0";

                dtHoraAula.Rows.Add(linhaHoraAula);
            }
        }

        private static bool ExisteHoraAula(Ly_hor_aula dtHoraAula, Techne.Lyceum.RN.Turma.DadosQuadroHorario dadosQuadroHorario)
        {
            return dtHoraAula.Select("aula = " + dadosQuadroHorario.Aula + " AND dia_semana = " + dadosQuadroHorario.DiaSemana + " AND faculdade =  '" + RN.RNBase.MudarAspas(dadosQuadroHorario.Faculdade) + "' AND turno =  '" + RN.RNBase.MudarAspas(dadosQuadroHorario.Turno) + "' AND disciplina = '" + RN.RNBase.MudarAspas(dadosQuadroHorario.Disciplina) + "' AND turma = '" + RN.RNBase.MudarAspas(dadosQuadroHorario.Turma) + "' AND ano = " + dadosQuadroHorario.Ano).Length == 0;
        }

        /// <summary>
        /// Obtém os dados da classe auxiliar para o DataTable de Aula Docente
        /// </summary>
        /// <param name="dtAulaDocente">DataTable de aula docente onde será armazenado o datarow com os dados da classe auxiliar</param>
        /// <param name="dadosQuadroHorario">Classe auxiliar com os dados necessários para preencher a linha de Aula Docente</param>
        private void ObterAulaDocente(Ly_aula_docente dtAulaDocente, Techne.Lyceum.RN.Turma.DadosQuadroHorario dadosQuadroHorario)
        {
            if (dtAulaDocente.Select("num_func = " + dadosQuadroHorario.NumFunc + " AND aula = " + dadosQuadroHorario.Aula + " AND dia_semana = " + dadosQuadroHorario.DiaSemana + " AND faculdade =  '" + RN.RNBase.MudarAspas(dadosQuadroHorario.Faculdade) + "' AND turno =  '" + RN.RNBase.MudarAspas(dadosQuadroHorario.Turno) + "' AND disciplina = '" + RN.RNBase.MudarAspas(dadosQuadroHorario.Disciplina) + "' AND turma = '" + RN.RNBase.MudarAspas(dadosQuadroHorario.Turma) + "' AND ano = " + dadosQuadroHorario.Ano + " AND semestre = " + dadosQuadroHorario.Semestre).Length == 0)
            {
                Ly_aula_docente.Row linhaAulaDocente = dtAulaDocente.NewRow();

                linhaAulaDocente.Ano = dadosQuadroHorario.Ano;
                linhaAulaDocente.Aula = dadosQuadroHorario.Aula;
                linhaAulaDocente.Data_fim = dadosQuadroHorario.DtFim;
                linhaAulaDocente.Data_inicio = dadosQuadroHorario.DtIni;
                linhaAulaDocente.Dia_semana = dadosQuadroHorario.DiaSemana;
                linhaAulaDocente.Disciplina = dadosQuadroHorario.Disciplina;
                linhaAulaDocente.Faculdade = dadosQuadroHorario.Faculdade;
                linhaAulaDocente.Num_func = dadosQuadroHorario.NumFunc;
                linhaAulaDocente.Semestre = dadosQuadroHorario.Semestre;
                linhaAulaDocente.Turma = dadosQuadroHorario.Turma;
                linhaAulaDocente.Turno = dadosQuadroHorario.Turno;
                //linhaAulaDocente.Tipo = "0";

                dtAulaDocente.Rows.Add(linhaAulaDocente);
            }
        }

        /// <summary>
        /// Obtém o valor em decimal do dia da semana
        /// </summary>
        /// <param name="id">valor descritivo do dia da semana (id do controle disciplina)</param>
        /// <returns>identificador decimal para o dia da semana</returns>
        private decimal ObterDiaSemana(string id)
        {
            if (id.ToUpper().Contains("SEGUNDA"))
                return 2;
            else if (id.ToUpper().Contains("TERCA"))
                return 3;
            else if (id.ToUpper().Contains("QUARTA"))
                return 4;
            else if (id.ToUpper().Contains("QUINTA"))
                return 5;
            else if (id.ToUpper().Contains("SEXTA"))
                return 6;
            else if (id.ToUpper().Contains("SABADO"))
                return 7;
            else if (id.ToUpper().Contains("DOMINGO"))
                return 1;
            else
                return 0M;
        }

        private string ObterDiaSemanaPorNumero(string id)
        {
            if (id == "2")
                return "Segunda";
            else if (id == "3")
                return "Terça";
            else if (id == "4")
                return "Quarta";
            else if (id == "5")
                return "Quinta";
            else if (id == "6")
                return "Sexta";
            else if (id == "7")
                return "Sábado";
            else
                return string.Empty;
        }

        private string ObterControleDocenteDiaSemana(decimal id)
        {
            if (id == 2)
                return "txtDocenteSegunda";
            else if (id == 3)
                return "txtDocenteTerca";
            else if (id == 4)
                return "txtDocenteQuarta";
            else if (id == 5)
                return "txtDocenteQuinta";
            else if (id == 6)
                return "txtDocenteSexta";
            else if (id == 7)
                return "txtDocenteSabado";
            else
                return string.Empty;
        }

        private string ObterControleDisciplinaDiaSemana(decimal id)
        {
            if (id == 2)
                return "txtDisciplinaSegunda";
            else if (id == 3)
                return "txtDisciplinaTerca";
            else if (id == 4)
                return "txtDisciplinaQuarta";
            else if (id == 5)
                return "txtDisciplinaQuinta";
            else if (id == 6)
                return "txtDisciplinaSexta";
            else if (id == 7)
                return "txtDisciplinaSabado";
            else
                return string.Empty;
        }

        /// <summary>
        /// Obtém os dados de quadro de horário de acordo com o parâmetro de dados
        /// </summary>
        /// <param name="dados">
        /// dados do quadro de horário divididos pelo separador "|"
        /// segue a estrutura da string:
        /// 0: Código da aula
        /// 1: horário inicial
        /// 2: horário final
        /// </param>
        private void ObterDadosQuadroHorario(string dados, Techne.Lyceum.RN.Turma.DadosQuadroHorario dadosQuadroHorario, Nullable<decimal> num_func, string disciplina, string idSemana)
        {
            if (!string.IsNullOrEmpty(dados))
            {
                string[] dadosQH = dados.Split('|');
                dadosQuadroHorario.Aula = Convert.ToDecimal(dadosQH[0]);

                DateTime horaIni = Convert.ToDateTime(dadosQH[1]);
                DateTime horaFim = Convert.ToDateTime(dadosQH[2]);

                //formata o campo de data para o padrão (somente usar a hora)
                dadosQuadroHorario.HoraIni = new DateTime(1899, 12, 30, horaIni.Hour, horaIni.Minute, horaIni.Second, horaIni.Millisecond);
                dadosQuadroHorario.HoraFim = new DateTime(1899, 12, 30, horaFim.Hour, horaFim.Minute, horaFim.Second, horaFim.Millisecond);

                dadosQuadroHorario.NumFunc = num_func;
                dadosQuadroHorario.Disciplina = disciplina;

                dadosQuadroHorario.Ano = Convert.ToDecimal(ddlAno.SelectedValue);
                dadosQuadroHorario.DiaSemana = ObterDiaSemana(idSemana);

                if (!tseUnidadeFisica.DBValue.IsNull)
                    dadosQuadroHorario.Faculdade = Convert.ToString(tseUnidadeFisica.DBValue);

                dadosQuadroHorario.NumFunc = num_func;
                dadosQuadroHorario.Semestre = Convert.ToDecimal(ddlPeriodo.SelectedValue);

                if (_tipoOperacao.Equals(TipoOperacao.Novo))
                    dadosQuadroHorario.Turma = txtTurma.Text;
                else
                    dadosQuadroHorario.Turma = ddlTurma.SelectedValue;

                dadosQuadroHorario.Turno = ddlTurno.SelectedValue;
                dadosQuadroHorario.Dependencia = ddlDependencia.SelectedValue;
                dadosQuadroHorario.DtIni = dtIniAula.Date;
                dadosQuadroHorario.DtFim = dtFimAula.Date;
            }
        }

        /// <summary>
        /// Obtém dados param Ly_hor_aula e Ly_aula_docente
        /// </summary>
        /// <param name="dtHoraAula">DataTable de hora aula onde será armazenado os dados</param>
        /// <param name="dtAulaDocente">DataTable de aula docente onde será armazenado os dados</param>
        private void ObterDadosTabela(Ly_hor_aula dtHoraAula, Ly_aula_docente dtAulaDocente)
        {
            //Obtém do controle Hidden as células que foram alteradas
            List<string> listaControles = ObterListaHorario();

            foreach (TableRow tr in tQuadroHorario.Rows)
            {
                foreach (TableCell td in tr.Cells)
                {
                    //verifica se existe mais de um controle na célula e se houve alteração/inclusão.
                    if (td.Controls.Count > 1 && listaControles != null)
                    {
                        //verifica se o controle de indice 0 é um textbox
                        if (td.Controls[0] is HtmlInputText)
                        {
                            //cast do textbox de docente encontrado na célula (representado pelo indice 0)
                            HtmlInputText txtDocente = (HtmlInputText)td.Controls[0];

                            //verifica se o ID do docente encontrado (txtDocente) está na lista de controles alterados
                            //somente será armazenado no datatable de hora aula e aula docente os controles que estão nesta lista
                            if (listaControles.Contains(txtDocente.ID))
                            {
                                HtmlInputText txtDisciplina = null;
                                if (td.Controls[2] is HtmlInputText)
                                    txtDisciplina = (HtmlInputText)td.Controls[2];

                                System.Web.UI.WebControls.TextBox txtDados = null;
                                if (td.Controls[3] is System.Web.UI.WebControls.TextBox)
                                    txtDados = (System.Web.UI.WebControls.TextBox)td.Controls[3];

                                HtmlInputHidden hCodigoDocente = null;
                                if (td.Controls[4] is HtmlInputHidden)
                                    hCodigoDocente = (HtmlInputHidden)td.Controls[4];

                                HtmlInputHidden hCodigoDisciplina = null;
                                if (td.Controls[5] is HtmlInputHidden)
                                    hCodigoDisciplina = (HtmlInputHidden)td.Controls[5];

                                if (txtDocente != null && txtDisciplina != null)
                                {
                                    //instancia a classe auxiliar para trabalhar os dados do quadro de horário
                                    Techne.Lyceum.RN.Turma.DadosQuadroHorario dadosQuadroHorario = new Techne.Lyceum.RN.Turma.DadosQuadroHorario();

                                    Nullable<decimal> num_func = -1;
                                    string disciplina = string.Empty;

                                    if (!string.IsNullOrEmpty(txtDocente.Value) && !string.IsNullOrEmpty(hCodigoDocente.Value))
                                    {
                                        num_func = Convert.ToDecimal(hCodigoDocente.Value);
                                        disciplina = hCodigoDisciplina.Value;
                                    }

                                    //Obtém os dados para a classe auxiliar
                                    ObterDadosQuadroHorario(txtDados.Text, dadosQuadroHorario,
                                                            num_func,
                                                            disciplina,
                                                            txtDisciplina.ID);

                                    //Obtém os datarows para hora aula
                                    ObterHoraAula(dtHoraAula, dadosQuadroHorario);
                                    //Obtém os datarows para aula docente
                                    ObterAulaDocente(dtAulaDocente, dadosQuadroHorario);

                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém a lista de horarios que foram alterados na tela, todos horários que estão nesta lista serão 
        /// incluidos/alterados de acordo com o tipo de operação.
        /// Este controle é feito de acordo com o id do docente inserido na lista de quadro de horários
        /// </summary>
        /// <returns>Lista de ID de docente alterado/incluido no quadro de horário</returns>
        private List<string> ObterListaHorario()
        {
            List<string> listaHorario = null;

            //verifica se o controle Hidden contém algum valor
            if (!string.IsNullOrEmpty(hControleCelula.Value))
            {
                //instancia a lista que armazenará os ID dos controles alterados (por ID de docente)
                listaHorario = new List<string>();

                //Divide o controle pelo separador "|" obtendo uma array com os ID dos docentes alterados.
                string[] controles = hControleCelula.Value.Split('|');

                if (controles.Length > 0)
                    foreach (string controle in controles.Where(s => !String.IsNullOrEmpty(s)))
                        listaHorario.Add(controle);//armazena na lista o ID do docente alterado                                    
            }
            //retorna uma lista de ID de todos docentes alterados no quadro de horário
            return listaHorario;
        }

        private void ObterDadosQueryString(string queryString)
        {
            ObjetoTurma = ObterDadosQueryStringObjeto(queryString);
        }

        private RN.Turma.DadosTurma ObterDadosQueryStringObjeto(string queryString)
        {
            RN.Turma.DadosTurma dadosTurma = new RN.Turma.DadosTurma();
            string grade_id = string.Empty;
            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("ano") >= 0)
                    dadosTurma.Ano = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("semestre") >= 0)
                    dadosTurma.Periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("turno") >= 0)
                    dadosTurma.Turno = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("curso") >= 0)
                    dadosTurma.Curso = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("unidadeResponsavel") >= 0)
                    dadosTurma.UnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("prefixoUnidadeResponsavel") >= 0)
                    dadosTurma.MnemonicoUnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("grade_id") >= 0)
                    grade_id = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("gradeId") >= 0)
                {
                    dadosTurma.Grade_ID = dados.Substring(dados.LastIndexOf('=') + 1);
                    grade_id = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                else if (dados.IndexOf("grade") >= 0)
                    dadosTurma.Grade = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("serie") >= 0)
                    dadosTurma.Serie = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("curriculo") >= 0)
                    dadosTurma.Curriculo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("faculdade") >= 0)
                    dadosTurma.Faculdade = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("nucleo") >= 0)
                    dadosTurma.Nucleo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("municipio") >= 0)
                    dadosTurma.Municipio = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("sufixo") >= 0)
                    dadosTurma.Sufixo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("dependencia") >= 0)
                    dadosTurma.Dependencia = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("tipogestao") >= 0)
                    dadosTurma.Tipogestao = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("tipoOperacao") >= 0)
                {
                    string tipoOperacao = dados.Substring(dados.LastIndexOf('=') + 1);

                    if (tipoOperacao == "NOVO")
                    {
                        _tipoOperacao = TipoOperacao.Novo;
                        lblTipoOperacao.Text = "Modo de inclusão";
                    }
                    else if (tipoOperacao == "ALTERAR")
                    {
                        _tipoOperacao = TipoOperacao.Alterar;
                        lblTipoOperacao.Text = "Modo de alteração";
                    }
                    else if (tipoOperacao == "EXCLUIR")
                    {
                        _tipoOperacao = TipoOperacao.Excluir;
                        lblTipoOperacao.Text = "Modo de exclusão";
                    }
                    else if (tipoOperacao == "CONSULTAR")
                    {
                        _tipoOperacao = TipoOperacao.ConsultarRetornaDados;
                        lblTipoOperacao.Text = "Modo de consulta";
                    }
                    else if (tipoOperacao == "REATIVAR")
                    {
                        _tipoOperacao = TipoOperacao.Reativar;
                        lblTipoOperacao.Text = "Modo de reativação";
                    }
                }
            }

            if (!string.IsNullOrEmpty(grade_id))
            {
                decimal gradeId = 0M;
                Decimal.TryParse(grade_id, out gradeId);

                QueryTable qtTurma = RN.Turma.ConsultarTurmaPorGradeSerie(gradeId);
                if (qtTurma != null && qtTurma.Rows.Count > 0)
                {
                    dadosTurma.Ano = Convert.ToString(qtTurma.Rows[0]["ANO"]);
                    dadosTurma.Periodo = Convert.ToString(qtTurma.Rows[0]["SEMESTRE"]);
                    dadosTurma.Turno = Convert.ToString(qtTurma.Rows[0]["TURNO"]);
                    dadosTurma.Curso = Convert.ToString(qtTurma.Rows[0]["CURSO"]);
                    dadosTurma.UnidadeResponsavel = Convert.ToString(qtTurma.Rows[0]["UNIDADE_RESPONSAVEL"]);
                    dadosTurma.Grade = Convert.ToString(qtTurma.Rows[0]["TURMA"]);
                    dadosTurma.Serie = Convert.ToString(qtTurma.Rows[0]["SERIE"]);
                    dadosTurma.Curriculo = Convert.ToString(qtTurma.Rows[0]["CURRICULO"]);
                    dadosTurma.Faculdade = Convert.ToString(qtTurma.Rows[0]["FACULDADE"]);
                    dadosTurma.Sufixo = Convert.ToString(qtTurma.Rows[0]["SUFIXO"]);
                    dadosTurma.Tipogestao = Convert.ToString(qtTurma.Rows[0]["TIPO_GESTAO"]);
                    dadosTurma.Ambienteexterno = Convert.ToString(qtTurma.Rows[0]["AMBIENTE_EXTERNO"]);
                    dadosTurma.Eletiva = Convert.ToString(qtTurma.Rows[0]["ELETIVA"]);
                    dadosTurma.TurmaReferencia = Convert.ToString(qtTurma.Rows[0]["TURMAREFERENCIA"]);
                    dadosTurma.Dependencia = Convert.ToString(qtTurma.Rows[0]["DEPENDENCIA"]);
                }
            }

            return dadosTurma;
        }

        /// <summary>
        /// Habilita a visibilidade dos botões de acordo com o tipo de operação
        /// </summary>
        private void ControlarTipoOperacao()
        {
            lblCurriculo.Visible = _tipoOperacao == TipoOperacao.Novo;
            ddlCurriculo.Visible = _tipoOperacao == TipoOperacao.Novo;
            trCurriculo.Visible = _tipoOperacao == TipoOperacao.Novo;
            trCriacao.Visible = _tipoOperacao != TipoOperacao.Novo;
            trAlunosMatriculados.Visible = _tipoOperacao != TipoOperacao.Novo;
            trAlunosMatriculadosProgressao.Visible = _tipoOperacao != TipoOperacao.Novo;
            trAlunosMatriculadosEletiva1.Visible = _tipoOperacao != TipoOperacao.Novo;
            trAlunosMatriculadosEletiva2.Visible = _tipoOperacao != TipoOperacao.Novo;
            trAlunosMatriculadosEletiva3.Visible = _tipoOperacao != TipoOperacao.Novo;

            switch (_tipoOperacao)
            {
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnVoltar, btnCancel, btnConfirmar };
                        ControlarVisibilidadeControle(controles);

                        ddlTurma.Visible = false;
                        ddlTurma.Enabled = true;

                        txtTurma.Visible = false;
                        tdTurma.Visible = txtTurma.Visible;
                        lblTurma.Visible = txtTurma.Visible;

                        pnlCadastro.Enabled = false;
                        pnlDefinicao.Enabled = true;

                        CarregarDadosDrop(ddlAno.ID);
                        CarregarDadosDrop(ddlPeriodo.ID);
                        CarregarDadosDrop(ddlTurno.ID);
                        CarregarDadosDrop(ddlCurso.ID);

                        trTurma.Visible = false;

                        pcTurma.TabPages.FindByName("QuadroHorario").ClientEnabled = true;
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnVoltar, btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        ddlTurma.Visible = false;

                        txtTurma.Visible = true;
                        tdTurma.Visible = txtTurma.Visible;
                        lblTurma.Visible = txtTurma.Visible;

                        pnlCadastro.Enabled = true;
                        pnlDefinicao.Enabled = true;

                        trTurma.Visible = true;

                        ddlPeriodo.Enabled = true;
                        pcTurma.Visible = !String.IsNullOrEmpty(ddlPeriodo.SelectedValue);

                        pcTurma.TabPages.FindByName("QuadroHorario").ClientEnabled = false;

                        break;
                    }
                case TipoOperacao.Excluir:
                    {
                        ImageButton[] controles = new ImageButton[] { btnVoltar, btnCancel, btnConfirmar };
                        ControlarVisibilidadeControle(controles);

                        ddlTurma.Visible = false;
                        ddlTurma.Enabled = false;

                        tdControleQH.Visible = false;
                        tdControleQH2.Visible = false;

                        txtTurma.Visible = false;
                        tdTurma.Visible = txtTurma.Visible;
                        lblTurma.Visible = txtTurma.Visible;

                        pnlCadastro.Enabled = false;
                        pnlDefinicao.Enabled = false;
                        pnlQuadroHorario.Enabled = false;

                        trTurma.Visible = false;

                        pcTurma.TabPages.FindByName("QuadroHorario").ClientEnabled = true;

                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        bool habilitarCampos;

                        ImageButton[] controles = new ImageButton[] { btnVoltar, btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        //não deixa editar campos chave
                        ddlTurma.Visible = false;
                        ddlTurma.Enabled = false;

                        ddlAno.Enabled = false;
                        ddlPeriodo.Enabled = false;

                        //curso e série não serão alterados
                        ddlCurso.Enabled = false;
                        ddlSerie.Enabled = false;
                        ddlSufixoSerie.Enabled = false;
                        ddlTurmaReferencia.Enabled = false;
                        chkEletiva.Enabled = false;

                        //Se alteração, a Optativa Reforço fica desabilitada caso alunos já matriculados e ativos.
                        //Se os números de matriculas ativas for maior que zero, desabilita ddlOptativaReforco                      

                        if (!string.IsNullOrEmpty(ddlTurma.SelectedValue) && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                        {

                            RN.Turma rnTurma = new RN.Turma();
                            int alunosAtivos = rnTurma.ObtemTotalAlunosMatriculadosNaTurmaPor(
                                ddlTurma.SelectedValue
                                , ddlAno.SelectedValue
                                , ddlPeriodo.SelectedValue);

                            if (alunosAtivos > 0)
                            {
                                ddlOptativaReforco.Enabled = false;
                            }
                        }
                        tseUnidadeResponsavel.Enabled = false;

                        //não deixa editar campos chave de ly_hor_aula, ly_aula_docente
                        tseUnidadeFisica.Enabled = false;
                        ddlTurno.Enabled = true;

                        txtTurma.Visible = false;
                        tdTurma.Visible = txtTurma.Visible;
                        lblTurma.Visible = txtTurma.Visible;

                        pnlCadastro.Enabled = true;
                        pnlDefinicao.Enabled = true;

                        if (ObjetoTurma != null &&
                            !string.IsNullOrEmpty(ObjetoTurma.Turno) &&
                            !string.IsNullOrEmpty(ObjetoTurma.Faculdade) &&
                            !string.IsNullOrEmpty(ObjetoTurma.Grade) &&
                            !string.IsNullOrEmpty(ObjetoTurma.Ano) &&
                            !string.IsNullOrEmpty(ObjetoTurma.Periodo)
                            )
                        {

                            //se houver algum registro em aula docente o controle será desabilitado
                            //dtIniAula.Enabled = RN.Turma.VerificarAulaDocente(ObjetoTurma.Turno, ObjetoTurma.Faculdade, ObjetoTurma.Grade, ObjetoTurma.Ano, ObjetoTurma.Periodo); 

                            habilitarCampos = RN.Turma.VerificarAulaDocente(ObjetoTurma.Turno, ObjetoTurma.Faculdade, ObjetoTurma.Grade, ObjetoTurma.Ano, ObjetoTurma.Periodo);
                            //                  habilitarCampos &= padaces.PermissaoGeral;

                            dtIniAula.ReadOnly = !habilitarCampos;
                            dtFimAula.ReadOnly = !habilitarCampos;


                            if (ddlCurso.Value != null)
                            {
                                if (ddlCurso.Value.ToString() == "9999.92")
                                    ddlDependencia.Enabled = false;
                            }
                        }

                        HabilitarImpressaoQHI();

                        //** Controle de Padrão de Acesso da Turma: habilita/desabilita/visibilidade controles
                        usuario = HttpContext.Current.User.Identity.Name.ToString();
                        padaces = RN.PadraoAcessoTurmas.ConsultarPadAcesTurma(ObjetoTurma.Curso, usuario);

                        if (!padaces.PermissaoGeral && !padaces.PermissaoQHI && !Permission.AllowUpdate)
                        {
                            _tipoOperacao = TipoOperacao.ConsultarRetornaDados;
                            ControlarTipoOperacao();
                        }
                        if (!padaces.PermissaoGeral)
                        {
                            ddlCurso.Enabled = false;
                            ddlTurno.Enabled = false;
                            ddlSerie.Enabled = false;
                            tseUnidadeFisica.Enabled = false;

                            dtIniAula.ReadOnly = true;
                            dtFimAula.ReadOnly = true;
                        }

                        if (btnSalvar.Style["visibility"] != "hidden")
                            btnSalvar.Style.Add("visibility", padaces.PermissaoGeral || padaces.PermissaoQHI ? "visible" : "hidden");
                        if (btnCancel.Style["visibility"] != "hidden")
                            btnCancel.Style.Add("visibility", padaces.PermissaoGeral || padaces.PermissaoQHI ? "visible" : "hidden");

                        tdControleQH.Visible = padaces.PermissaoQHI;
                        tdControleQH2.Visible = padaces.PermissaoQHI;
                        tseDocente.Enabled = padaces.PermissaoQHI;
                        tseDocente2.Enabled = padaces.PermissaoQHI;
                        ddlDisciplinaQuadroHorario.Enabled = padaces.PermissaoQHI;
                        ddlDisciplinaQuadroHorario2.Enabled = padaces.PermissaoQHI;
                        //** Fim de Controle de Padrão de Acesso da Turma

                        trTurma.Visible = false;

                        pcTurma.TabPages.FindByName("QuadroHorario").ClientEnabled = true;

                        ddlDependencia.Enabled = Permission.AllowUpdate;

                        if (ddlDependencia.Enabled)
                            ddlDependencia.Enabled = !chkAmbienteExterno.Checked;

                        if (ddlCurso.Value != null)
                        {
                            if (ddlCurso.Value.ToString() == "9999.92")
                            {
                                ddlDependencia.Enabled = false;
                                chkAmbienteExterno.Enabled = false;
                                chkMacros.Enabled = false;
                                txtNumMaxAluno.Text = "30";
                                txtNumMaxAluno.Enabled = false;
                                txtNumMaxAluno.ReadOnly = true;
                            }
                        }
                        txtNumMaxAluno.Enabled = Permission.AllowUpdate;

                        if (!string.IsNullOrEmpty(txtNumMaxAluno.Text))
                        {
                            txtNumMaxAluno.Enabled = Convert.ToInt32(txtNumMaxAluno.Text) <= NumMaxAluno;
                        }

                        if ((ddlCurso.SelectedItem.Value.ToString() == "0001.18") || (ddlCurso.SelectedItem.Value.ToString() == "0001.19") || (ddlCurso.SelectedItem.Value.ToString() == "0001.17") || (ddlCurso.SelectedItem.Value.ToString() == "0001.27") || (ddlCurso.SelectedItem.Value.ToString() == "0002.37"))
                        {
                            txtNumMaxAluno.Enabled = true;
                            txtNumMaxAluno.ReadOnly = false;
                        }

                        ControlarVisibilidadeDependencia();

                        break;
                    }
                case TipoOperacao.ConsultarRetornaDados:
                    {
                        ImageButton[] controles = new ImageButton[] { btnVoltar };
                        ControlarVisibilidadeControle(controles);

                        ddlTurma.Visible = false;
                        ddlTurma.Enabled = false;
                        lblTurma.Visible = ddlTurma.Visible;

                        tdControleQH.Visible = false;
                        tdControleQH2.Visible = false;

                        txtTurma.Visible = false;
                        tdTurma.Visible = txtTurma.Visible;
                        lblTurma.Visible = txtTurma.Visible;

                        pnlCadastro.Enabled = false;
                        pnlDefinicao.Enabled = false;
                        //pnlQuadroHorario.Enabled = false;

                        trTurma.Visible = false;

                        ddlDisciplinaQuadroHorario.Enabled = false;
                        //                        tseDocente.Enabled = false;
                        ddlDisciplinaQuadroHorario2.Enabled = false;
                        tseDocente2.Enabled = false;

                        HabilitarImpressaoQHI();

                        pcTurma.TabPages.FindByName("QuadroHorario").ClientEnabled = true;

                        break;
                    }
                case TipoOperacao.Reativar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnVoltar, btnCancel, btnSalvar };
                        ControlarVisibilidadeControle(controles);

                        ddlTurma.Visible = false;
                        ddlTurma.Enabled = false;
                        lblTurma.Visible = ddlTurma.Visible;
                        ddlCurso.Enabled = false;
                        ddlTurno.Enabled = false;
                        ddlSerie.Enabled = false;
                        tseUnidadeFisica.Enabled = false;
                        dtIniAula.ReadOnly = true;
                        dtFimAula.ReadOnly = true;
                        chkAmbienteExterno.Enabled = false;
                        ddlOptativaReforco.Enabled = false;
                        chkMacros.Enabled = false;
                        ddlTurmaReferencia.Enabled = false;
                        chkEletiva.Enabled = false;

                        tdControleQH.Visible = false;
                        tdControleQH2.Visible = false;
                        txtTurma.Visible = false;
                        tdTurma.Visible = txtTurma.Visible;
                        lblTurma.Visible = txtTurma.Visible;

                        pnlDefinicaoGeral.Enabled = false;
                        pnlPeriodoLetivo.Enabled = false;
                        pnlDefinicao.Enabled = false;

                        trTurma.Visible = false;

                        ddlDisciplinaQuadroHorario.Enabled = false;
                        ddlDisciplinaQuadroHorario2.Enabled = false;
                        tseDocente2.Enabled = false;

                        HabilitarImpressaoQHI();

                        pcTurma.TabPages.FindByName("QuadroHorario").ClientEnabled = true;

                        if (!chkEletiva.Checked)
                        {
                            lblMensagem.Text = "Informativo: Para reativar a turma será necessário escolher uma sala de aula que não exista turma aberta na mesma sala/turno.";
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTurma"></param>
        /// <param name="pAno"></param>
        /// <param name="pPeriodo"></param>
        /// <returns></returns>
        /// <autor>Anderson Wernek</autor>
        private int ObtemAlunosAtivosMatriculadosNaTurmaPor(string pTurma, string pAno, string pPeriodo)
        {
            QueryTable resultado = null;

            try
            {
                resultado = RN.Turma.ObtemAlunosAtivosMatriculadosNaTurmaPor(pTurma, pAno, pPeriodo);
                return resultado.Rows.Count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Habilita a visibilidade nos botões passados como parâmetro
        /// </summary>
        /// <param name="botoes">Array com os botões que serão visiveis</param>
        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();
            foreach (ImageButton botao in botoes)
                botao.Visible = true;
        }

        private void ControlarVisibilidadeDependencia()
        {
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.Perfil rnPerfil = new Perfil();

            var BloquearNumeroMaximoAlunoTurma = Convert.ToBoolean(ConfigurationManager.AppSettings["BloquearNumeroMaximoAlunoTurma"] ?? "false");

            //Desabilita combo sala de aula
            if (BloquearNumeroMaximoAlunoTurma && !rnUsuarios.EhPrivilegiado(User.Identity.Name) && !rnPerfil.PossuiPerfilAlteraMaximoAlunoPor(User.Identity.Name))
            {
                ddlDependencia.Enabled = false;
                txtNumMaxAluno.Enabled = false;
                txtNumMaxAluno.ReadOnly = true;
            }
        }

        private void HabilitarImpressaoQHI()
        {
            btnImprimir.Visible = true;
            string queryString = MontarQueryString(string.Empty);
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
            btnImprimir.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/RelatorioTurma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
        }

        /// <summary>
        /// Retira a visibilidade de todos botões
        /// </summary>
        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnConfirmar.Visible = false;
            btnVoltar.Visible = false;
            btnSalvar.Visible = false;
        }

        /// <summary>
        /// Armazena uma nova linha com os dados da tela no datatable passado como parâmetro
        /// </summary>
        /// <param name="dtTurma">DataTable de turma que será adicionado uma nova linha</param>
        private void ObterDados(Ly_turma dtTurma)
        {
            CR.Ly_turma.Row dadosTurma = dtTurma.NewRow();

            dadosTurma.Tipo_gestao = ddlTipoGestao.SelectedValue;
            dadosTurma.Ano = Convert.ToDecimal(ddlAno.SelectedValue);
            dadosTurma.Semestre = Convert.ToDecimal(ddlPeriodo.SelectedValue);
            dadosTurma.Turno = ddlTurno.SelectedValue;
            dadosTurma.Curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
            dadosTurma.Ambiente_externo = chkAmbienteExterno.Checked ? "S" : null;

            //Adicionar Optativa ou Reforço
            //Autor: Anderson Wernek
            //Data: 24/08/2012
            dadosTurma.OptativaReforco = ddlOptativaReforco.SelectedValue == null ? "N" : ddlOptativaReforco.SelectedValue.ToString();

            decimal serie = 0M;

            Decimal.TryParse(ddlSerie.SelectedValue, out serie);

            if (serie != 0)
                dadosTurma.Serie = serie;

            if (!tseUnidadeFisica.DBValue.IsNull)
                dadosTurma.Faculdade = Convert.ToString(tseUnidadeFisica.DBValue);

            if (!_tipoOperacao.Equals(TipoOperacao.Consultar))
            {
                if (ObjetoTurma != null && !string.IsNullOrEmpty(ObjetoTurma.Curriculo))
                    dadosTurma.Curriculo = ObjetoTurma.Curriculo;

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                    dadosTurma.Unidade_responsavel = Convert.ToString(tseUnidadeResponsavel.DBValue);
                dadosTurma.Dependencia = ddlDependencia.SelectedValue;

                dadosTurma.Dt_inicio = dtIniAula.Date;
                dadosTurma.Dt_fim = dtFimAula.Date;


                if (!string.IsNullOrEmpty(txtNumMaxAluno.Text))
                {
                    decimal numAlunos = 0M;
                    dadosTurma.Num_alunos = decimal.TryParse(txtNumMaxAluno.Text, out numAlunos) ? numAlunos : (decimal?)null;
                }
            }

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                string prefixoSerie = string.Empty;
                string sufixoSerie = string.Empty;
                if (ObjetoTurma != null)
                {
                    if (!string.IsNullOrEmpty(ObjetoTurma.Curriculo) && !string.IsNullOrEmpty(ObjetoTurma.Serie))
                    {
                        prefixoSerie = RN.Serie.ConsultarPrefixoSerie(ddlCurso.Value == null ? "" : ddlCurso.Value.ToString(), ddlTurno.SelectedValue, ObjetoTurma.Curriculo, Convert.ToDecimal(ObjetoTurma.Serie));
                        if (!string.IsNullOrEmpty(ddlSufixoSerie.SelectedValue))
                            sufixoSerie = ddlSufixoSerie.SelectedValue;
                    }
                }

                string codigoUA = string.Empty;
                if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue)
                {
                    if (!tseUnidadeResponsavel["SETOR"].IsNull)
                        codigoUA = Convert.ToString(tseUnidadeResponsavel["SETOR"]);
                }

                dadosTurma.Turma = prefixoSerie + txtTurma.Text.Trim().ToUpper() + sufixoSerie + "-" + codigoUA;
                dadosTurma.Turma_integracao = sufixoSerie;
            }
            else
            {
                dadosTurma.Turma = ddlTurma.SelectedValue;

            }

            //campo obrigatório no datarow
            dadosTurma.Disciplina = string.Empty;
            dadosTurma.Aulas_previstas = 0;
            dadosTurma.Aulas_dadas = 0;
            dadosTurma.Classificacao = string.Empty;
            dadosTurma.Min_aulas = 0;
            dadosTurma.Nivel = string.Empty;
            dadosTurma.Nivel_presenca = "Presencial";
            dadosTurma.Sit_turma = string.Empty;
            dadosTurma.Dt_ultalt = DateTime.Now;
            dadosTurma.Sit_turma = "Aberta";
            dadosTurma.Dt_criacao = DateTime.Now;

            dadosTurma.Especial = "N";
            dadosTurma.Utiliza_indice = "N";

            dadosTurma.Eletiva = chkEletiva.Checked ? "S" : "N";
            dadosTurma.TurmaReferencia = !ddlTurmaReferencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurmaReferencia.SelectedValue : null;

            dtTurma.Rows.Add(dadosTurma);
        }

        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosTurma">Linha com os dados de turma</param>
        private void PreencherDadosTela(Ly_turma.Row dadosTurma)
        {
            RN.Perfil rnPerfil = new Perfil();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.Curso rnCurso = new Curso();
            RN.CapacidadeAlunoTurmaMunicipio rnCapacidade = new RN.CapacidadeAlunoTurmaMunicipio();
            int salaCapacidadeMunicipio = 0;

            ddlTipoGestao.SelectedValue = Convert.ToString(dadosTurma.Tipo_gestao);
            ddlAno.SelectedValue = Convert.ToString(dadosTurma.Ano);
            ddlPeriodo.SelectedValue = Convert.ToString(dadosTurma.Semestre);
            ddlTurno.SelectedValue = dadosTurma.Turno;
            ddlCurso.Value = dadosTurma.Curso;
            ddlSerie.SelectedValue = Convert.ToString(dadosTurma.Serie);

            //Adicionar Optativa ou Reforço
            //Autor: Anderson Wernek
            //Data: 24/08/2012
            ddlOptativaReforco.SelectedValue = Convert.ToString(dadosTurma.OptativaReforco);

            if (dadosTurma.Curso == "9999.92")
            {
                lblAmbienteExterno.Visible = true;
                chkAmbienteExterno.Visible = true;

                chkAmbienteExterno.Checked = dadosTurma.Ambiente_externo == "S";

                lblMacros.Visible = true;
                chkMacros.Visible = true;

                CarregarList(ddlCurso.Value.ToString(), ddlTurno.SelectedValue, Convert.ToString(dadosTurma.Curriculo));
                string macrodisc = string.Empty;

                var dt = RN.Turma.ListarMacros(Convert.ToString(dadosTurma.Curriculo),
                                                                            ddlCurso.Value.ToString(),
                                                                            ddlTurno.SelectedValue, ddlTurma.SelectedValue, Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        macrodisc += item["ID_MACRO_CAMPOS"].ToString();
                        macrodisc += ";";
                    }

                }

                if (!string.IsNullOrEmpty(macrodisc))
                {
                    string[] macros = macrodisc.Split(';');
                    foreach (String str in macros)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            chkMacros.Items.FindByValue(str).Selected = true;
                        }
                    }
                }
            }
            else
            {
                //lblAmbienteExterno.Visible = false;
                //chkAmbienteExterno.Visible = false;
                lblMacros.Visible = false;
                chkMacros.Visible = false;
            }

            //var qtTurma = RN.Curso.ConsultarPorUnidadeEnsino(dadosTurma.Unidade_responsavel.ToString());
            //DataTable data = (DataTable)qtTurma;
            //string salaExterna = data.Rows[ddlCurso.SelectedIndex].Field<string>("salaexterna").ToString();

            //if (salaExterna.Equals("N"))
            //{
            //    chkAmbienteExterno.Enabled = false;
            //    chkAmbienteExterno.Checked = false;
            //}
            //else
            //{
            //    chkAmbienteExterno.Enabled = true;
            //    //chkAmbienteExterno.Checked = false;
            //}

            chkAmbienteExterno.Checked = (dadosTurma.Ambiente_externo == "S");

            lblAmbienteExterno.Visible = chkAmbienteExterno.Visible = rnCurso.EhCursoMinistradoSalaExterna(ddlCurso.Value.ToString());

            ddlTurma.SelectedValue = dadosTurma.Turma;
            txtTurma.Text = dadosTurma.Turma;
            Regex rgx = new Regex("\t|\\s+");
            string turma = rgx.Replace(txtTurma.Text, " ");
            txtTurma.Text = turma;
            tbTurma2.Value = dadosTurma.Turma;
            ddlTurmasUnidadeDev.Value = dadosTurma.Turma;

            lblCursoDesc.Text = rnCurso.ObtemDescricaoPor(dadosTurma.Curso);

            try
            {
                tseUnidadeResponsavel.DBValue = dadosTurma.Unidade_responsavel;
            }
            catch
            {
                lblMensagem.Text = dadosTurma.Unidade_responsavel;
            }

            tseUnidadeFisica.DBValue = dadosTurma.Faculdade;

            if (dadosTurma.Dt_inicio.HasValue)
            {
                dtIniAula.Date = dadosTurma.Dt_inicio.Value;
            }
            if (dadosTurma.Dt_fim.HasValue)
            {
                dtFimAula.Date = dadosTurma.Dt_fim.Value;
            }

            chkEletiva.Checked = dadosTurma.Eletiva == "S" && !dadosTurma.TurmaReferencia.IsNullOrEmptyOrWhiteSpace() ? true : false;

            chkEletiva_CheckedChanged(null, null);

            ddlTurmaReferencia.SelectedValue = !dadosTurma.TurmaReferencia.IsNullOrEmptyOrWhiteSpace() ? dadosTurma.TurmaReferencia : string.Empty;

            //CarregaDependencia(
            CarregarDadosDrop(ddlDependencia.ID);
            if (dadosTurma.Dependencia != null)
            {
                ddlDependencia.SelectedValue = dadosTurma.Dependencia;
            }
            else
            {
                ddlDependencia.Items.FindByValue("").Selected = true;

                if (dadosTurma.Curso == "9999.92")
                {
                    ddlDependencia.Enabled = false;
                }
            }

            if (dadosTurma.Curso == "9999.92")
            {
                txtNumMaxAluno.Text = "30";
                txtNumMaxAluno.Enabled = false;
                txtNumMaxAluno.ReadOnly = true;
            }
            else
            {
                txtNumMaxAluno.Text = Convert.ToString(dadosTurma.Num_alunos);
            }

            //Alimenta Label de Capacidade da sala:
            txtCapacidadeSala.Text = RN.Dependencia.ObterNumeroAluno(Convert.ToString(tseUnidadeFisica.DBValue), ddlDependencia.SelectedValue).ToString();

            //Verifica Capacidade da turma pelo municipio
            salaCapacidadeMunicipio = rnCapacidade.RetornaCapacidadePor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), tseUnidadeResponsavel["municipio"].ToString());
            if (salaCapacidadeMunicipio == -1)
            {
                //Caso não exista, verifica Capacidade da turma pelo Curso
                var capacidade = RN.CapacidaDeAlunoTurma.Carregar(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlCurso.SelectedItem.Value.ToString());

                if (capacidade.CapacidaDeAlunoTurmaId > 0)
                {
                    txtCapacidadeTurma.Text = capacidade.CapacidadeMaxima.ToString();
                }
            }
            else
            {
                //Alimenta Label de Capacidade da turma pelo municipio
                txtCapacidadeTurma.Text = salaCapacidadeMunicipio.ToString();
            }

            txtNumMaxAluno.Enabled = Convert.ToInt32(txtNumMaxAluno.Text) <= NumMaxAluno;

            lblAlunoMatriculaValor.Text = Convert.ToString(RN.Turma.ObterNumAlunoMatriculadoPrincipal(Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Semestre), dadosTurma.Turma));

            lblAlunoMatriculaProgressaoValor.Text = Convert.ToString(RN.Turma.ObterNumAlunoMatriculadoProgessao(Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Semestre), dadosTurma.Turma));

            lblAlunoMatriculaEletiva1Valor.Text = Convert.ToString(RN.Turma.ObterNumAlunoMatriculadoEletiva(Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Semestre), dadosTurma.Turma, 1));
            lblAlunoMatriculaEletiva2Valor.Text = Convert.ToString(RN.Turma.ObterNumAlunoMatriculadoEletiva(Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Semestre), dadosTurma.Turma, 2));
            lblAlunoMatriculaEletiva3Valor.Text = Convert.ToString(RN.Turma.ObterNumAlunoMatriculadoEletiva(Convert.ToDecimal(dadosTurma.Ano), Convert.ToDecimal(dadosTurma.Semestre), dadosTurma.Turma, 3));

            DateTime? dt_criacao = dadosTurma.Dt_criacao;
            lblCriacaoValor.Text = dt_criacao.HasValue ? (dt_criacao.Value.ToString("dd/MM/yyyy HH") + "h" + dt_criacao.Value.ToString("mm") + "min") : "--/--/----";

            string sala = ddlDependencia.SelectedItem.Value.ToString();

            if (!string.IsNullOrEmpty(sala))
            {
                if (sala.Substring(0, 2).Equals("SA") && ddlOptativaReforco.SelectedValue != "S" && !rnUsuarios.EhPrivilegiado(User.Identity.Name) && !rnPerfil.PossuiPerfilAlteraMaximoAlunoPor(User.Identity.Name))
                {
                    txtNumMaxAluno.Enabled = false;
                    txtNumMaxAluno.ReadOnly = true;
                }
                else
                {
                    txtNumMaxAluno.Enabled = true;
                    txtNumMaxAluno.ReadOnly = false;
                }
            }

        }

        private void CarregarDropDownList(DropDownList drop, QueryTable data, List<DropDownList> listaDrop, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();

            if (data != null)
            {
                if (data.Rows != null)
                {
                    if (data.Rows.Count > 0)
                    {
                        if (drop.Items.FindByValue(defaultValue) != null)
                            drop.SelectedValue = defaultValue;
                        else
                            CriarItemVazio(drop, true);
                    }
                }
            }

            if (listaDrop != null)
            {
                foreach (DropDownList dropDependente in listaDrop)
                {
                    dropDependente.Items.Clear();
                    dropDependente.DataBind();
                }
            }
        }

        private void CarregarDropDownList(ASPxComboBox drop, QueryTable data, List<DropDownList> listaDrop, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();

            if (data != null)
            {
                if (data.Rows != null)
                {
                    if (data.Rows.Count > 0)
                    {
                        if (drop.Items.FindByValue(defaultValue) != null)
                            drop.SelectedItem = drop.Items.FindByValue(defaultValue);
                        else
                            CriarItemVazio(drop, true);
                    }
                }
            }
            if (listaDrop != null)
            {
                foreach (DropDownList dropDependente in listaDrop)
                {
                    dropDependente.Items.Clear();
                    dropDependente.DataBind();
                }
            }
        }

        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLCURRICULO":
                        {

                            string curso = string.Empty, turno = string.Empty;
                            decimal? ano = null, semestre = null;

                            if (ddlCurso.Items.Count > 0)
                                curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
                            if (ddlTurno.Items.Count > 0)
                                turno = ddlTurno.SelectedValue;
                            if (ddlAno.Items.Count > 0)
                                ano = Convert.ToDecimal(ddlAno.SelectedValue);
                            if (ddlPeriodo.Items.Count > 0 && !String.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                                semestre = Convert.ToDecimal(ddlPeriodo.SelectedValue);

                            if (!String.IsNullOrEmpty(curso) && !String.IsNullOrEmpty(turno) && ano.HasValue && semestre.HasValue)
                            {
                                //adicionado para casos onde a data do curriculo tenha expirado, em casos de operação que nao seja inclusao, serão listados todos os curriculos
                                if (_tipoOperacao == TipoOperacao.Novo)
                                {
                                    dadosDrop = RN.Curriculo.Consultar(turno, curso, ano.Value, semestre.Value);
                                }
                                else
                                {
                                    dadosDrop = RN.Curriculo.ConsultarTodos(turno, curso, ano.Value, semestre.Value);
                                }
                            }

                            List<DropDownList> listaDrop = new List<DropDownList>();
                            listaDrop.Add(ddlSerie);
                            listaDrop.Add(ddlTurma);

                            if (dadosDrop != null && dadosDrop.Rows.Count == 1)
                                CarregarDropDownList(ddlCurriculo, dadosDrop, listaDrop, dadosDrop.Rows[0]["curriculo"].ToString());
                            else
                                CarregarDropDownList(ddlCurriculo, dadosDrop, listaDrop, ObjetoTurma.Curriculo);

                            if (_tipoOperacao == TipoOperacao.Novo)
                                CriarItemVazio(ddlCurriculo, true);

                            break;
                        }
                    case "DDLANO":
                        {
                            dadosDrop = RN.PeriodoLetivo.ConsultarAno();

                            List<DropDownList> listaDrop = new List<DropDownList>();
                            listaDrop.Add(ddlPeriodo);
                            listaDrop.Add(ddlTurma);

                            CarregarDropDownList(ddlAno, dadosDrop, listaDrop, ObjetoTurma.Ano);

                            if ((ObjetoTurma == null) || (ObjetoTurma != null && string.IsNullOrEmpty(ObjetoTurma.Ano)))
                                CriarItemVazio(ddlAno, true);
                            else
                            {
                                int ano = 0;
                                if (int.TryParse(ObjetoTurma.Ano, out ano))
                                {
                                    dtIniAula.MinDate = new DateTime(ano, 1, 1);
                                    dtIniAula.MaxDate = new DateTime(ano, 12, DateTime.DaysInMonth(ano, 12));
                                    dtFimAula.MinDate = new DateTime(ano, 1, 1);
                                    dtFimAula.MaxDate = new DateTime(ano, 12, DateTime.DaysInMonth(ano, 12));
                                }
                            }

                            break;
                        }
                    case "DDLPERIODO":
                        {
                            string ano = string.Empty;

                            if (ddlAno.Items.Count > 0)
                                ano = ddlAno.SelectedValue;

                            if (!string.IsNullOrEmpty(ano))
                            {
                                dadosDrop = RN.PeriodoLetivo.ConsultarPeriodo(ano);

                                List<DropDownList> listaDrop = new List<DropDownList>();
                                listaDrop.Add(ddlTurma);

                                CarregarDropDownList(ddlPeriodo, dadosDrop, listaDrop, ObjetoTurma.Periodo);

                                if ((ObjetoTurma == null) || (ObjetoTurma != null && string.IsNullOrEmpty(ObjetoTurma.Periodo)))
                                    CriarItemVazio(ddlPeriodo, true);
                            }

                            break;
                        }
                    case "DDLTURNO":
                        {
                            string unidade_ens = string.Empty, curso = string.Empty;

                            if (tseUnidadeResponsavel.IsValidDBValue)
                                unidade_ens = Convert.ToString(tseUnidadeResponsavel.DBValue);
                            if (!String.IsNullOrEmpty(ddlCurso.Value == null ? "" : ddlCurso.Value.ToString()))
                                curso = ddlCurso.Value.ToString();

                            dadosDrop = RN.Turno.ConsultarPorUnidadeEnsinoECurso(unidade_ens, curso);

                            List<DropDownList> listaDrop = new List<DropDownList>();
                            listaDrop.Add(ddlSerie);
                            listaDrop.Add(ddlTurma);

                            CarregarDropDownList(ddlTurno, dadosDrop, listaDrop, ObjetoTurma.Turno);

                            if ((ObjetoTurma == null) || (ObjetoTurma != null && string.IsNullOrEmpty(ObjetoTurma.Turno)))
                                CriarItemVazio(ddlTurno, true);

                            break;
                        }
                    case "DDLCURSO":
                        {
                            string unidade_ens = string.Empty;

                            if (tseUnidadeResponsavel.IsValidDBValue)
                                unidade_ens = Convert.ToString(tseUnidadeResponsavel.DBValue);

                            //dadosDrop = RN.Curso.ConsultarPorUnidadeEnsino(unidade_ens);

                            List<DropDownList> listaDrop = new List<DropDownList>();
                            listaDrop.Add(ddlTurma);
                            listaDrop.Add(ddlSerie);
                            listaDrop.Add(ddlCurriculo);

                            ddlCurso.DataBind();

                            if (ObjetoTurma != null && !String.IsNullOrEmpty(ObjetoTurma.Curso))
                            {
                                ddlCurso.SelectedItem = ddlCurso.Items.FindByValue(ObjetoTurma.Curso);
                            }

                            CarregarDadosDrop(ddlTurno.ID);

                            break;
                        }
                    case "DDLSERIE":
                        {
                            string ano, semestre, turno, curso, curriculo;

                            if (!string.IsNullOrEmpty(ddlTurno.SelectedValue) && !string.IsNullOrEmpty(ddlCurso.Value == null ? "" : ddlCurso.Value.ToString()) && !string.IsNullOrEmpty(ddlCurriculo.SelectedValue))
                            {
                                ano = ddlAno.SelectedValue;
                                semestre = ddlPeriodo.SelectedValue;
                                turno = ddlTurno.SelectedValue;
                                curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
                                curriculo = ddlCurriculo.SelectedValue;

                                dadosDrop = RN.Serie.Consultar(ano, semestre, turno, curso, curriculo);

                                List<DropDownList> listaDrop = new List<DropDownList>();
                                listaDrop.Add(ddlTurma);
                                listaDrop.Add(ddlSufixoSerie);

                                CarregarDropDownList(ddlSerie, dadosDrop, listaDrop, ObjetoTurma.Serie);

                                if (!string.IsNullOrEmpty(ddlSerie.SelectedValue))
                                {
                                    txtPrefixoSerie.Text = RN.Serie.ConsultarPrefixoSerie(ddlCurso.Value == null ? "" : ddlCurso.Value.ToString(), ddlTurno.SelectedValue, ObjetoTurma.Curriculo, Convert.ToDecimal(ddlSerie.SelectedValue));
                                    CarregarDadosDrop(ddlSufixoSerie.ID);
                                }
                            }
                            else
                            {
                                ObjetoTurma.Serie = string.Empty;
                                ddlSerie.Items.Clear();
                            }

                            if ((ObjetoTurma == null) || (ObjetoTurma != null && string.IsNullOrEmpty(ObjetoTurma.Serie)))
                            {
                                txtPrefixoSerie.Text = string.Empty;
                                CriarItemVazio(ddlSerie, true);
                            }

                            break;
                        }
                    case "DDLTURMA":
                        {
                            string ano, periodo, turno, curso, serie;

                            if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue) &&
                                !string.IsNullOrEmpty(ddlTurno.SelectedValue) && !string.IsNullOrEmpty(ddlCurso.Value == null ? "" : ddlCurso.Value.ToString()) &&
                                !string.IsNullOrEmpty(ddlSerie.SelectedValue))
                            {
                                ano = ddlAno.SelectedValue;
                                periodo = ddlPeriodo.SelectedValue;
                                turno = ddlTurno.SelectedValue;
                                curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
                                serie = ddlSerie.SelectedValue;

                                dadosDrop = RN.Turma.Consultar(ano, periodo, turno, curso, serie);

                                ddlTurma.Items.Clear();
                                if (!String.IsNullOrEmpty(ObjetoTurma.Grade) && ObjetoTurma.Grade.LastIndexOf("-") >= 0)
                                {
                                    string turma_tmp = ObjetoTurma.Grade.Substring(0, ObjetoTurma.Grade.LastIndexOf("-"));
                                    if (!string.IsNullOrEmpty(txtPrefixoSerie.Text) && turma_tmp.StartsWith(txtPrefixoSerie.Text))
                                        turma_tmp = turma_tmp.Substring(txtPrefixoSerie.Text.Length);
                                    ddlTurma.Items.Add(new ListItem(turma_tmp, ObjetoTurma.Grade));

                                }
                            }

                            break;
                        }
                    case "DDLDEPENDENCIA":
                        {
                            string unidadeFisica = Convert.ToString(tseUnidadeFisica.DBValue);
                            string turno = Convert.ToString(ddlTurno.SelectedValue);
                            Regex rgx = new Regex("\t|\\s+");
                            string turma = rgx.Replace(txtTurma.Text, " ");
                            txtTurma.Text = turma;
                            RN.Dependencia rnDependencia = new Dependencia();
                            DataTable dtDependencia = new DataTable();

                            var tipo_depend = string.Empty;

                            //PARA CURSO DE EDUCACAO ESPECIAL 
                            if (ddlCurso.Value != null && ddlCurso.Value.ToString() == "9999.91")
                            {
                                tipo_depend = "SALAAEE";
                            }
                            else
                            {
                                tipo_depend = "SALA";
                            }

                            string dependencia = string.Empty;
                            if (!String.IsNullOrEmpty(ObjetoTurmaInicial.Curso) && !String.IsNullOrEmpty(ObjetoTurmaInicial.Turno) && !String.IsNullOrEmpty(ObjetoTurmaInicial.Curriculo)
                                && !String.IsNullOrEmpty(ObjetoTurmaInicial.Ano) && !String.IsNullOrEmpty(ObjetoTurmaInicial.Periodo) && !String.IsNullOrEmpty(tbTurma2.Value))
                            {
                                var gs = RN.Turma.ConsultarGradeSerie(ObjetoTurmaInicial.Curso, ObjetoTurmaInicial.Turno, ObjetoTurmaInicial.Curriculo,
                                ObjetoTurmaInicial.Ano, ObjetoTurmaInicial.Periodo, tbTurma2.Value);
                                if (gs != null)
                                    dependencia = gs.Dependencia;
                            }

                            var ddlDep = !ddlDependencia.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlDependencia.SelectedValue : (!ObjetoTurmaInicial.Dependencia.IsNullOrEmptyOrWhiteSpace() ? ObjetoTurmaInicial.Dependencia : null);

                            if (ddlOptativaReforco.SelectedValue == "N")
                            {
                                if ((ddlCurso.Items.Count > 0) && (ddlCurso.SelectedItem.Text != "Selecione"))
                                {
                                    dtDependencia = rnDependencia.ConsultarAtiva(unidadeFisica, turno, turma, tipo_depend, ddlAno.SelectedValue,
                                                                         ddlPeriodo.SelectedValue, ddlCurso.Value.ToString(), ddlTurmaReferencia.SelectedValue, ddlDep);
                                }
                            }
                            else
                            {
                                if ((ddlCurso.Items.Count > 0) && (ddlCurso.SelectedItem.Text != "Selecione"))
                                {
                                    dtDependencia = rnDependencia.ConsultaDependenciaAtivaPor(unidadeFisica, turno, turma,
                                        ddlAno.SelectedValue,
                                        ddlPeriodo.SelectedValue,
                                        tipo_depend, ddlCurso.Value.ToString(), ddlDep);
                                }
                            }

                            //if (tipo_depend.Equals("SALA") || tipo_depend.Equals("SALAAEE"))
                            //{
                            //    chkAmbienteExterno.Enabled = false;
                            //    chkAmbienteExterno.Checked = false;
                            //}

                            CarregaDependencia(dtDependencia);


                            if (txtNumMaxAluno.Enabled)
                            {
                                PopularNumeroAlunoDependencia();
                            }

                            break;
                        }
                    case "DDLDISCIPLINAQUADROHORARIO":
                        {
                            PopularDropDownDisciplinaQuadroDeHorarios(ddlDisciplinaQuadroHorario);

                            break;
                        }
                    case "DDLDISCIPLINAQUADROHORARIO2":
                        {
                            PopularDropDownDisciplinaQuadroDeHorarios(ddlDisciplinaQuadroHorario2);

                            break;
                        }
                    case "DDLSUFIXOSERIE":
                        {
                            string turno, curso, serie;

                            if (!string.IsNullOrEmpty(ddlTurno.SelectedValue) && !string.IsNullOrEmpty(ddlCurso.Value == null ? "" : ddlCurso.Value.ToString()))
                            {
                                turno = ddlTurno.SelectedValue;
                                curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
                                serie = ddlSerie.SelectedValue;

                                dadosDrop = RN.Serie.ConsultarSufixo(curso, turno, ObjetoTurma.Curriculo, serie);

                                CarregarDropDownList(ddlSufixoSerie, dadosDrop, null, ObjetoTurma.Sufixo);
                            }

                            if ((ObjetoTurma == null) || (ObjetoTurma != null && string.IsNullOrEmpty(ObjetoTurma.Serie)) || (dadosDrop != null && dadosDrop.Rows.Count == 0))
                                CriarItemVazio(ddlSufixoSerie, true);

                            break;
                        }

                    case "DDLTURMASUNIDADEDEV":
                        ddlTurmasUnidadeDev.DataSource = RN.Turma.ObterTurmasMesmaUnidadeESituacao(tbTurma2.Value, ddlAno.SelectedValue, ddlPeriodo.SelectedValue);
                        ddlTurmasUnidadeDev.DataBind();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            return dadosDrop;
        }

        private void CarregaDependencia(DataTable dtDependencia)
        {
            ddlDependencia.Items.Clear();
            ddlDependencia.DataSource = dtDependencia;
            ddlDependencia.DataBind();
            ddlDependencia.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void PopularDropDownDisciplinaQuadroDeHorarios(DropDownList ddlQuadroHorario)
        {

            string curso = ddlCurso.Value == null ? "" : ddlCurso.Value.ToString();
            string turno = ddlTurno.SelectedValue;
            string turma = ddlTurma.Text;
            string ano = ddlAno.Text;
            string semestre = ddlPeriodo.Text;
            decimal serie = 0M;

            if (!string.IsNullOrEmpty(ddlSerie.SelectedValue))
                serie = Convert.ToDecimal(ddlSerie.SelectedValue);


            QueryTable dadosDrop = !chkEletiva.Checked ? RN.Disciplina.ConsultarComMultipla(turma, ano, semestre) : RN.Disciplina.ListaDisciplinaMultipla(turma, ano, semestre);

            CarregarDropDownList(ddlQuadroHorario, dadosDrop, null, string.Empty);
        }

        private void LimparTela()
        {
            ddlAno.Items.Clear();
            ddlPeriodo.Items.Clear();
            ddlTurno.Items.Clear();
            ddlCurso.Items.Clear();
            ddlSerie.Items.Clear();
            ddlTurma.Items.Clear();
            txtTurma.Text = string.Empty;
            tbTurma2.Value = string.Empty;
            lblCursoDesc.Text = string.Empty;

            tseUnidadeResponsavel.DBValue = DBNull.Value;
            hdnUA.Value = string.Empty;

            txtNumMaxAluno.Text = string.Empty;
            txtCapacidadeSala.Text = string.Empty;
            txtCapacidadeTurma.Text = string.Empty;

            tseUnidadeFisica.DBValue = DBNull.Value;

            ddlDependencia.Items.Clear();

            dtIniAula.Text = string.Empty;
            dtFimAula.Text = string.Empty;
            chkEletiva.Checked = false;
            ddlTurmaReferencia.Items.Clear();

        }

        private void CarregarDadosTurma()
        {
            try
            {
                byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                ObterDadosQueryString(decodedText);
                ObjetoTurmaInicial = ObterDadosQueryStringObjeto(decodedText);

                if (ObjetoTurma != null)
                {
                    if (!string.IsNullOrEmpty(ObjetoTurma.UnidadeResponsavel))
                    {
                        try
                        {
                            tseUnidadeResponsavel.DBValue = ObjetoTurma.UnidadeResponsavel;
                        }
                        catch
                        {
                            lblMensagem.Text = ObjetoTurma.UnidadeResponsavel;
                        }

                        if (!tseUnidadeResponsavel["SETOR"].IsNull)
                        {
                            lblUAValor.Text = Convert.ToString(tseUnidadeResponsavel["UA_ATUAL"]);
                            hdnUA.Value = Convert.ToString(tseUnidadeResponsavel["SETOR"]);
                        }
                    }

                    if (tseUnidadeResponsavel.IsValidDBValue)
                    {
                        string unidade_ensino = Convert.ToString(tseUnidadeResponsavel.DBValue);
                        QueryTable qtUnidadeEnsino = RN.UnidadeFisica.ConsultarPorUnidadeEnsino(unidade_ensino);

                        if (qtUnidadeEnsino != null)
                        {
                            if (qtUnidadeEnsino.Rows.Count == 1)
                                tseUnidadeFisica.DBValue = qtUnidadeEnsino.Rows[0]["unidade_fis"];
                        }
                    }
                }

                CarregarDadosDrop(ddlAno.ID);
                CarregarDadosDrop(ddlPeriodo.ID);
                CarregarDadosDrop(ddlCurso.ID);
                if (ddlCurso.Items.FindByValue(ObjetoTurma.Curso) != null)
                {
                    ddlCurso.SelectedItem = ddlCurso.Items.FindByValue(ObjetoTurma.Curso);
                }
                else
                {
                    lblMensagem.Text = "Curso não está autorizado na unidade escolar.";
                    return;
                }
                CarregarDadosDrop(ddlTurno.ID);
                CarregarDadosDrop(ddlCurriculo.ID);
                CarregarDadosDrop(ddlSerie.ID);
                CarregarDadosDrop(ddlTurma.ID);


                if (_tipoOperacao != TipoOperacao.Novo)
                {
                    Ly_turma dtTurma = new Ly_turma();
                    ObterDados(dtTurma);

                    Ly_turma.Row dadosTurma = RN.Turma.Consultar(ObjetoTurma.Ano, ObjetoTurma.Periodo, ObjetoTurma.Turno, ObjetoTurma.Curso, ObjetoTurma.Serie, ObjetoTurma.Grade);
                    if (dadosTurma != null)
                        PreencherDadosTela(dadosTurma);

                    CarregarDadosDrop(ddlTurmasUnidadeDev.ID);
                    ddlTurmasUnidadeDev.Visible = true;
                }
                else
                {
                    ddlTurmasUnidadeDev.Visible = false;

                    //caso seja um novo registro será carregado as datas das aulas configuradas no periodo letivo
                    if (ObjetoTurma != null)
                    {
                        if (!string.IsNullOrEmpty(ObjetoTurma.Ano) && !string.IsNullOrEmpty(ObjetoTurma.Periodo))
                        {
                            decimal ano = Convert.ToDecimal(ObjetoTurma.Ano);
                            decimal semestre = Convert.ToDecimal(ObjetoTurma.Periodo);

                            SimpleRow dataAula = RN.PeriodoLetivo.ObterDataAula(ano, semestre);

                            if (dataAula != null)
                            {
                                if (!dataAula["dt_inicio_aula"].IsNull)
                                    dtIniAula.Date = Convert.ToDateTime(dataAula["dt_inicio_aula"]);
                                if (!dataAula["dt_fim_aula"].IsNull)
                                    dtFimAula.Date = Convert.ToDateTime(dataAula["dt_fim_aula"]);
                            }
                        }
                    }
                    CarregarDadosDrop(ddlDependencia.ID);
                }

                string turno = ddlTurno.SelectedValue;
                string dependencia = ddlDependencia.SelectedValue;
                string numMaxAlunos = txtNumMaxAluno.Text;
                string dataInicio = dtIniAula.Date.ToString("yyyy-MM-dd");
                string dataFim = dtFimAula.Date.ToString("yyyy-MM-dd");

                CarregarDadosDrop(ddlDisciplinaQuadroHorario.ID);
                CarregarDadosDrop(ddlDisciplinaQuadroHorario2.ID);
                ArmazenarDadosTurmaOriginaisEmHiddenField(turno, dependencia, numMaxAlunos, dataInicio, dataFim);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        /// <summary>
        /// Grava em campo hidden os valores originais do quadro no formato:
        /// AULA;DIA_SEMANA;NUM_FUNC;DISCIPLINA|AULA;DIA_SEMANA;NUM_FUNC;DISCIPLINA|...
        /// Utilizado em verificação de Javascript se existem dados alterados, ao selecionar outra turma na combo
        /// </summary>
        private void ArmazenarQuadroOriginalEmHiddenField(QueryTable qtAulas)
        {
            hdnOriginalQuadro.Value = string.Empty;
            StringBuilder str = new StringBuilder();

            if (qtAulas != null)
            {
                for (int i = 0; i < qtAulas.Rows.Count; i++)
                {
                    string aula = Convert.ToString(qtAulas.Rows[i]["AULA"]);
                    string dia_semana = Convert.ToString(qtAulas.Rows[i]["DIA_SEMANA"]);
                    string num_func = Convert.ToString(qtAulas.Rows[i]["NUM_FUNC"]);
                    string disciplina = Convert.ToString(qtAulas.Rows[i]["DISCIPLINA"]);

                    str.Append(String.Format("{0};{1};{2};{3}", aula, dia_semana, num_func, disciplina));
                    if (i != qtAulas.Rows.Count - 1)
                        str.Append("|");
                }
            }
            hdnOriginalQuadro.Value = str.ToString();
        }

        /// <summary>
        /// Grava em campo hidden os valores originais da turma no formato:
        /// TURNO|DEPENDENCIA|NUM_MAX_ALUNOS|DATA_INICIO-YYYY-MM-DD|DATA_FIM-YYYY-MM-DD
        /// Utilizado em verificação de Javascript se existem dados alterados, ao selecionar outra turma na combo
        /// </summary>        
        private void ArmazenarDadosTurmaOriginaisEmHiddenField(String turno, String dependencia, String numMaxAlunos, String dataInicio, String dataFim)
        {
            hdnOriginalDadosTurma.Value = String.Format("{0}|{1}|{2}|{3}|{4}",
                turno, dependencia, numMaxAlunos, dataInicio, dataFim);
        }

        protected void ddlTurmasUnidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Turma.DadosTurma turma = RN.Turma.ObterDadosTurma(ddlTurmasUnidadeDev.Value.ToString(), ddlAno.SelectedValue, ddlPeriodo.SelectedValue);
            string queryString = string.Empty;
            if (turma != null)
            {
                queryString += "tipoOperacao=ALTERAR";
                queryString += "&ano=" + turma.Ano;
                queryString += "&semestre=" + turma.Periodo;
                queryString += "&nucleo=" + turma.Nucleo;
                queryString += "&municipio=" + turma.Municipio;
                queryString += "&unidadeResponsavel=" + turma.UnidadeResponsavel;
                queryString += "&prefixoUnidadeResponsavel=" + turma.MnemonicoUnidadeResponsavel;
                queryString += "&grade=" + turma.Grade;
                queryString += "&faculdade=" + turma.Faculdade;
                queryString += "&turno=" + turma.Turno;
                queryString += "&curso=" + turma.Curso;
                queryString += "&serie=" + turma.Serie;
                queryString += "&gradeId=" + turma.Grade_ID;
                queryString += "&sufixo=" + turma.Sufixo;
                queryString += "&curriculo=" + turma.Curriculo;
                queryString += "&dependencia=" + turma.Dependencia;
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
                Response.Redirect("Turma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
        }

        private void MontarCelula(string idTextBoxDocente, string idTextBoxDisciplina, bool eventoBorda, string valor, TableRow tr)
        {
            TableCell td = new TableCell();
            //verifica se adiciona o evendo de click na célula para pintar de vermelho sua borda
            if (eventoBorda)
            {
                td.Width = Unit.Percentage(12.5);
                HtmlInputText txtDocente = new HtmlInputText();
                txtDocente.Attributes.Add("class", "txtInput");
                txtDocente.ID = idTextBoxDocente;
                txtDocente.Attributes.Add("readonly", "readonly");

                //TXTDOCENTE Control index 0
                td.Controls.Add(txtDocente);

                //LITERALCONTROL (<br>) Control index 1
                td.Controls.Add(new LiteralControl("<br>"));

                HtmlInputText txtDisciplina = new HtmlInputText();
                txtDisciplina.Attributes.Add("readonly", "readonly");
                txtDisciplina.Attributes.Add("class", "txtInput");
                txtDisciplina.ID = idTextBoxDisciplina;

                //TXTDISCIPLINA Control index 2
                td.Controls.Add(txtDisciplina);

                System.Web.UI.WebControls.TextBox txtCamposAula = new System.Web.UI.WebControls.TextBox();
                txtCamposAula.ID = "txtCamposAula_" + txtDocente.ID;
                txtCamposAula.Text = valor;
                txtCamposAula.Visible = false;
                //TXTCAMPOSAULA Control index 3
                td.Controls.Add(txtCamposAula);

                HtmlInputHidden hCodigoDocente = new HtmlInputHidden();
                hCodigoDocente.ID = "txtCodigoDocente_" + txtDocente.ID;
                //HTMLINPUTHIDDEN (hCodigoDocente) Control index 4
                td.Controls.Add(hCodigoDocente);

                HtmlInputHidden hCodigoDisciplina = new HtmlInputHidden();
                hCodigoDisciplina.ID = "txtCodigoDisciplina_" + txtDisciplina.ID;
                //HTMLINPUTHIDDEN (hCodigoDisciplina) Control index 5
                td.Controls.Add(hCodigoDisciplina);

                //adiciona célula na linha
                tr.Cells.Add(td);

                //Nova célula com o imagem que possibilita copiar/colar/recortar valores
                TableCell tdImagem = new TableCell();

                //adiciona o controle somente se o tipo de operação for diferente de consulta e exclusão, e o usuário possuir permissão de acesso de alteração do quadro                
                //String usuario = HttpContext.Current.User.Identity.Name.ToString();
                //DadosPadraoAcessoTurma padaces = RN.PadraoAcessoTurmas.ConsultarPadAcesTurma(ObjetoTurma.Curso, usuario);

                if (_tipoOperacao != TipoOperacao.ConsultarRetornaDados && _tipoOperacao != TipoOperacao.Excluir && padaces.PermissaoQHI)
                {
                    HtmlImage img = new HtmlImage();
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Attributes.Add("class", "PopUp");
                    div.Attributes.Add("style", "display:block;");

                    tdImagem.Controls.Add(div);

                    img.Attributes.Add("class", "Imagem");
                    img.Src = "~/Images/select_list.gif";
                    tdImagem.VerticalAlign = VerticalAlign.Top;
                    tdImagem.HorizontalAlign = HorizontalAlign.Left;
                    tdImagem.Controls.Add(img);
                }

                //adiciona célula na linha
                tr.Cells.Add(tdImagem);
            }
            else
            {
                td.Width = Unit.Percentage(10);
                string aula = valor.Split(' ')[0];

                HtmlInputHidden hAula = new HtmlInputHidden();
                hAula.ID = "hAula_" + aula;
                hAula.Value = aula;
                td.Controls.Add(hAula);

                td.Controls.Add(new LiteralControl(valor.Substring(aula.Length + 1)));

                tr.Cells.Add(td);
            }
        }

        private void ControlarCorControleCelula(HtmlInputText txtDocente, HtmlInputText txtDisciplina)
        {
            if (txtDocente != null && txtDisciplina != null)
            {
                //obtém valor do campo texto caso seja um postback
                string valorDocente = txtDocente.Value;

                if (!string.IsNullOrEmpty(valorDocente))
                {
                    //caso a cor esteja definida como GLP não entrará na condição pois já foi definido este valor quando foi preenchido seu valor
                    if (txtDocente.Style["background-color"] != COR_GLP_00000000
                        && txtDocente.Style["background-color"] != COR_GLP_99999999
                        && txtDocente.Style["background-color"] != COR_GLP
                        && txtDocente.Style["background-color"] != COR_CT_00000000
                        && txtDocente.Style["background-color"] != COR_CT_99999999
                        && txtDocente.Style["background-color"] != COR_CT)
                    {
                        //controle interno gerado pelo pagecontrole
                        string idControlePagina = pcTurma.UniqueID;
                        txtDocente.Style.Clear();
                        txtDisciplina.Style.Clear();
                    }

                    if (valorDocente.Contains("00000000"))
                    {
                        txtDocente.Style.Add("background-color", "#FFFD80");
                        txtDocente.Style.Add("color", "black");

                        txtDisciplina.Style.Add("background-color", "#FFFD80");
                        txtDisciplina.Style.Add("color", "black");
                    }
                    else if (valorDocente.Contains("99999999"))
                    {
                        txtDocente.Style.Add("background-color", "#EAEA00");
                        txtDocente.Style.Add("color", "black");

                        txtDisciplina.Style.Add("background-color", "#EAEA00");
                        txtDisciplina.Style.Add("color", "black");
                    }
                    else if (valorDocente.Contains("66666666"))
                    {
                        txtDocente.Style.Add("background-color", "#000080");
                        txtDocente.Style.Add("color", "white");

                        txtDisciplina.Style.Add("background-color", "#000080");
                        txtDisciplina.Style.Add("color", "white");
                    }
                    else if (valorDocente.Contains("88888888") || valorDocente.Contains("11111111")
                        || valorDocente.Contains("22222222") || valorDocente.Contains("44444444"))
                    {
                        txtDocente.Style.Add("background-color", "#D7D7D7");
                        txtDocente.Style.Add("color", "black");

                        txtDisciplina.Style.Add("background-color", "#D7D7D7");
                        txtDisciplina.Style.Add("color", "black");
                    }
                    else if (valorDocente.Contains("55555555") || valorDocente.Contains("77777777"))
                    {
                        txtDocente.Style.Add("background-color", "LightGray");
                        txtDocente.Style.Add("color", "black");

                        txtDisciplina.Style.Add("background-color", "LightGray");
                        txtDisciplina.Style.Add("color", "black");
                    }
                    else if (valorDocente.Contains("88888880"))
                    {
                        txtDocente.Style.Add("background-color", "#c8a2c8");
                        txtDocente.Style.Add("color", "black");

                        txtDisciplina.Style.Add("background-color", "#c8a2c8");
                        txtDisciplina.Style.Add("color", "black");
                    }
                    else if (valorDocente.Contains("88888881"))
                    {
                        txtDocente.Style.Add("background-color", "#ffa500");
                        txtDocente.Style.Add("color", "black");

                        txtDisciplina.Style.Add("background-color", "#ffa500");
                        txtDisciplina.Style.Add("color", "black");
                    }
                    else if (valorDocente.Contains("55555551"))
                    {// #ff0066 (ROSA)/ #ff6600(CORAL)/#262626(CINZA ESCURO)
                        txtDocente.Style.Add("background-color", "#ff0000");
                        txtDocente.Style.Add("color", "WHITE");

                        txtDisciplina.Style.Add("background-color", "#ff0000");
                        txtDisciplina.Style.Add("color", "WHITE");
                    }
                }
                else // se a célula estiver vazia será definido o estilo padrão para os controles
                {
                    txtDocente.Style.Clear();

                    txtDocente.Style.Add("background-color", "white");
                    txtDocente.Style.Add("color", "black");

                    txtDisciplina.Style.Clear();

                    txtDisciplina.Style.Add("background-color", "white");
                    txtDisciplina.Style.Add("color", "black");
                }
            }
        }

        private void LimparTable()
        {
            TableRow linhaPrincipal = tQuadroHorario.Rows[0];
            tQuadroHorario.Rows.Clear();
            tQuadroHorario.Rows.Add(linhaPrincipal);
        }

        private void PopularTable()
        {
            usuario = HttpContext.Current.User.Identity.Name.ToString();
            padaces = RN.PadraoAcessoTurmas.ConsultarPadAcesTurma(ObjetoTurma.Curso, usuario);
            try
            {
                //verifica se o objeto de turma não está nulo e se existe série no objeto de turma
                if (ObjetoTurma != null && !string.IsNullOrEmpty(ObjetoTurma.Serie))
                {
                    decimal serie = 0M;
                    //tenta converter o valor da série no objeto da turma para decimal
                    if (Decimal.TryParse(ObjetoTurma.Serie, out serie))
                    {
                        QueryTable qt = RN.HorarioOperacional.Consultar(ObjetoTurma.Faculdade, ObjetoTurma.Turno, ObjetoTurma.Curso, ObjetoTurma.Curriculo, serie);
                        QueryTable qtAulaDocente = null;

                        if (!_tipoOperacao.Equals(TipoOperacao.Novo))
                            qtAulaDocente = RN.Turma.ConsultarAulaDocente(ObjetoTurma.Turno, ObjetoTurma.Faculdade, ddlTurma.SelectedValue, Convert.ToDecimal(ObjetoTurma.Ano), Convert.ToDecimal(ObjetoTurma.Periodo));

                        TableRow tr = null;

                        if (qt != null)
                        {
                            foreach (SimpleRow linha in qt.Rows)
                            {
                                tr = new TableRow();
                                tr.Height = Unit.Pixel(7);
                                TableCell td = new TableCell();
                                td.CssClass = "bordaHorario";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.ColumnSpan = 7;
                                td.CssClass = string.Empty;
                                tr.Cells.Add(td);

                                tQuadroHorario.Rows.Add(tr);

                                tr = new TableRow();
                                string valorIdentificador = Convert.ToString(linha["aula"]) + "_" + String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horaini_aula"])).Replace(":", "_") + "_" + String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horafim_aula"])).Replace(":", "_");
                                TableCell tdHorario = new TableCell();

                                tdHorario.Width = Unit.Percentage(10);
                                tdHorario.CssClass = "bordaHorario";

                                HtmlInputHidden hAula = new HtmlInputHidden();
                                hAula.ID = "hAula_" + valorIdentificador;
                                hAula.Value = Convert.ToString(linha["aula"]);
                                tdHorario.Controls.Add(hAula);

                                tdHorario.Controls.Add(new LiteralControl(String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horaini_aula"])) + " / " + String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horafim_aula"]))));

                                tr.Cells.Add(tdHorario);

                                string valor = Convert.ToString(linha["aula"]) + "|" + String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horaini_aula"])) + "|" + String.Format("{0:HH:mm}", Convert.ToDateTime(linha["horafim_aula"]));

                                MontarCelula("txtDocenteSegunda_" + valorIdentificador, "txtDisciplinaSegunda_" + valorIdentificador, true, valor, tr);
                                MontarCelula("txtDocenteTerca_" + valorIdentificador, "txtDisciplinaTerca_" + valorIdentificador, true, valor, tr);
                                MontarCelula("txtDocenteQuarta_" + valorIdentificador, "txtDisciplinaQuarta_" + valorIdentificador, true, valor, tr);
                                MontarCelula("txtDocenteQuinta_" + valorIdentificador, "txtDisciplinaQuinta_" + valorIdentificador, true, valor, tr);
                                MontarCelula("txtDocenteSexta_" + valorIdentificador, "txtDisciplinaSexta_" + valorIdentificador, true, valor, tr);
                                MontarCelula("txtDocenteSabado_" + valorIdentificador, "txtDisciplinaSabado_" + valorIdentificador, true, valor, tr);

                                tQuadroHorario.Rows.Add(tr);

                                if (qtAulaDocente != null && !_tipoOperacao.Equals(TipoOperacao.Novo))
                                    MontarCelulaDados(qtAulaDocente, Convert.ToString(linha["aula"]), valorIdentificador, tr, padaces);

                                //THAIS
                                VerficaCelulasVazias(tr, padaces.PermissaoParcial);

                            }

                            ArmazenarQuadroOriginalEmHiddenField(qtAulaDocente);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void VerficaCelulasVazias(TableRow tr, Boolean permissaoParcial)
        {

            if (permissaoParcial)
            {
                var emptyCells = tr.Cells.Cast<TableCell>()
                                         .Where(x => x.Controls.Count > 2
                                                     && x.Controls.Cast<Control>()
                                                         .Where(y => y is HtmlInputHidden
                                                                     && string.IsNullOrEmpty(((HtmlInputHidden)y).Value)).Any())
                                         .ToList();

                foreach (var empty in emptyCells)
                {
                    empty.Attributes["somenteLeitura"] = "true";
                    empty.Attributes["onclick"] = empty.Attributes["oncontextmenu"] = @"javascript:alert('Célula bloqueada para alocação.\nQualquer dúvida entrar em contato com o QHI.'); return false;";
                }
            }
        }

        private void VerificarHorarioOperacional(QueryTable qt)
        {
            if (qt != null)
            {
                if (qt.Rows != null && qt.Rows.Count == 0)
                {
                    lblMensagemQHI.Visible = true;
                    pnlQuadroHorario.Visible = false;
                }
                else if (qt.Rows != null && qt.Rows.Count > 0)
                {
                    lblMensagemQHI.Visible = false;
                    pnlQuadroHorario.Visible = true;
                }
            }
            else
            {
                lblMensagemQHI.Visible = true;
                pnlQuadroHorario.Visible = false;
            }
        }

        private void MontarCelulaDados(QueryTable qtAulaDocente, string aula, string valorIdentificador, TableRow tr, DadosPadraoAcessoTurma padaces)
        {
            string sql = "aula = " + aula;
            SimpleRow[] dadosAula = qtAulaDocente.Select(sql);

            bool privilegiado = false;
            privilegiado = RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name);

            bool permite5555555555ou66666666ou77777777 = RN.PadraoAcessoTurmas.ConsultarPermissaoAlocacaoDocSemAula(HttpContext.Current.User.Identity.Name.ToString());

            //Caso usuario seja QHI (que permite5555555555ou66666666ou77777777) ou seja privilegiado permitir exclusao de 88888880 e 88888881
            bool permite88888880ou88888881 = false;
            if (privilegiado || permite5555555555ou66666666ou77777777)
            {
                permite88888880ou88888881 = true;
            }

            if (dadosAula != null && dadosAula.Length > 0)
            {
                foreach (SimpleRow linhaDadosAula in dadosAula)
                {
                    int diaSemana = Convert.ToInt32(linhaDadosAula["DIA_SEMANA"]);
                    string tipo_aula = string.Empty;

                    if (!linhaDadosAula["tipo_aula"].IsNull)
                        tipo_aula = Convert.ToString(linhaDadosAula["tipo_aula"]);

                    String nomeDocente = Convert.ToString(linhaDadosAula["NOME_DOCENTE"]);
                    String nomeDisciplina = linhaDadosAula["NOME_DISCIPLINA"].ToString();
                    String numFunc = Convert.ToString(linhaDadosAula["NUM_FUNC"]);
                    String disciplina = Convert.ToString(linhaDadosAula["DISCIPLINA"]);
                    String matricula = Convert.ToString(linhaDadosAula["MATRICULA"]);

                    String turno = Convert.ToString(linhaDadosAula["turno"]);
                    String faculdade = Convert.ToString(linhaDadosAula["faculdade"]);
                    String turma = Convert.ToString(linhaDadosAula["turma"]);
                    String ano = Convert.ToString(linhaDadosAula["ano"]);
                    String semestre = Convert.ToString(linhaDadosAula["semestre"]);
                    String matriculaAnterior = Convert.ToString(linhaDadosAula["tipo_docente"]);
                    int? regimeContratacao = linhaDadosAula["regimeContratacaoid"] != DBNull.Value ? Convert.ToInt32(linhaDadosAula["regimeContratacaoid"]) : (int?)null;
                    string tipoContrato = string.Empty;

                    if (tipo_aula == "GLP")
                    {
                        if (!String.IsNullOrEmpty(matriculaAnterior))
                            tipo_aula += "_" + matriculaAnterior;
                    }
                    if (regimeContratacao != null)
                    {
                        if (regimeContratacao == ((int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario))
                        {
                            tipoContrato = "CT";
                            if (!string.IsNullOrEmpty(matriculaAnterior))
                            {
                                tipoContrato += "_" + matriculaAnterior;
                            }
                        }
                    }

                    bool somenteLeitura = (matricula == "55555555" || matricula == "77777777" || matricula == "66666666") && !permite5555555555ou66666666ou77777777;

                    //THAIS
                    if ((matricula != "99999999" && matricula != "00000000") && padaces.PermissaoParcial && Convert.ToString(linhaDadosAula["tipo_aula"]) != "GLP")
                        somenteLeitura = true;

                    if ((matricula == "77777777" || matricula == "66666666") && padaces.PermissaoParcial && Convert.ToString(linhaDadosAula["tipo_aula"]) != "GLP")
                        somenteLeitura = false;

                    //Verificar se usuario pode alterar 88888880 e 88888881
                    if ((matricula == "88888880" || matricula == "88888881") && !permite88888880ou88888881)
                    {
                        somenteLeitura = true;
                    }

                    if (diaSemana == 2)
                        PreencherValorCelula(tr, "txtDocenteSegunda_" + valorIdentificador, nomeDocente,
                            "txtDisciplinaSegunda_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula, somenteLeitura, tipoContrato);
                    else if (diaSemana == 3)
                        PreencherValorCelula(tr, "txtDocenteTerca_" + valorIdentificador, nomeDocente,
                            "txtDisciplinaTerca_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula, somenteLeitura, tipoContrato);
                    else if (diaSemana == 4)
                        PreencherValorCelula(tr, "txtDocenteQuarta_" + valorIdentificador, nomeDocente,
                            "txtDisciplinaQuarta_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula, somenteLeitura, tipoContrato);
                    else if (diaSemana == 5)
                        PreencherValorCelula(tr, "txtDocenteQuinta_" + valorIdentificador, nomeDocente,
                            "txtDisciplinaQuinta_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula, somenteLeitura, tipoContrato);
                    else if (diaSemana == 6)
                        PreencherValorCelula(tr, "txtDocenteSexta_" + valorIdentificador, nomeDocente,
                            "txtDisciplinaSexta_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula, somenteLeitura, tipoContrato);
                    else if (diaSemana == 7)
                        PreencherValorCelula(tr, "txtDocenteSabado_" + valorIdentificador, nomeDocente,
                            "txtDisciplinaSabado_" + valorIdentificador, nomeDisciplina, numFunc, disciplina, tipo_aula, somenteLeitura, tipoContrato);
                }
            }
        }

        private void PreencherValorCelula(TableRow tr, string idTextBoxDocente, string valorTextBoxDocente, string idTextBoxDisciplina, string valorTextBoxDisciplina, string num_func, string disciplina, string tipo_aula, bool somenteLeitura, string tipoContrato)
        {
            if (tr != null && tr.Cells.Count > 0)
            {
                foreach (TableCell td in tr.Cells)
                {
                    //verifica se não é a primeira célula que contém somente um controle
                    if (td.Controls.Count > 1)
                    {
                        HtmlInputText txtDocente = null;
                        HtmlInputText txtDisciplina = null;
                        if (td.FindControl(idTextBoxDocente) != null && td.FindControl(idTextBoxDocente) is HtmlInputText)
                        {
                            txtDocente = (HtmlInputText)td.FindControl(idTextBoxDocente);
                            if (txtDocente != null)
                            {
                                txtDocente.Value = valorTextBoxDocente;
                                if (somenteLeitura && txtDocente.Parent is TableCell)
                                {
                                    TableCell parentTD = (TableCell)txtDocente.Parent;
                                    parentTD.Attributes["somenteLeitura"] = "true";

                                    //THAIS
                                    //if (txtDocente.Value == "55555555")
                                    //{
                                    parentTD.Attributes["onclick"] = parentTD.Attributes["oncontextmenu"] = @"javascript:alert('Célula bloqueada para alocação.\nQualquer dúvida entrar em contato com a Coordenadoria Regional.'); return false;";
                                    //}
                                    //else
                                    //{
                                    //    parentTD.Attributes["onclick"] = parentTD.Attributes["oncontextmenu"] = @"javascript:alert('Célula bloqueada para alocação.\nQualquer dúvida entrar em contato com o QHI.'); return false;";
                                    //}
                                    parentTD.TabIndex = -1;

                                    if (parentTD.Parent is TableRow)
                                    {
                                        TableRow parentTR = (TableRow)parentTD.Parent;

                                        TableCell[] temp = parentTR.Cells.Cast<TableCell>().ToArray();
                                        for (int i = 0; i < temp.Length; i++)
                                        {
                                            TableCell imageTD_temp = temp[i];
                                            if (imageTD_temp.ClientID == parentTD.ClientID)
                                            {
                                                TableCell imageTD = temp[i + 1];
                                                imageTD.Controls.Clear();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (td.FindControl(idTextBoxDisciplina) != null && td.FindControl(idTextBoxDisciplina) is HtmlInputText)
                        {
                            txtDisciplina = (HtmlInputText)td.FindControl(idTextBoxDisciplina);
                            if (txtDisciplina != null)
                            {
                                txtDisciplina.Value = valorTextBoxDisciplina;
                            }
                        }

                        if (txtDocente != null && txtDisciplina != null && !string.IsNullOrEmpty(tipo_aula))
                        {
                            if (tipo_aula.ToUpper() == "GLP")
                            {
                                txtDocente.Style.Clear();
                                txtDisciplina.Style.Clear();

                                txtDocente.Style.Add("background-color", COR_GLP);
                                txtDocente.Style.Add("color", "black");

                                txtDisciplina.Style.Add("background-color", COR_GLP);
                                txtDisciplina.Style.Add("color", "black");
                            }
                            else if (tipo_aula.ToUpper() == "GLP_00000000")
                            {
                                txtDocente.Style.Clear();
                                txtDisciplina.Style.Clear();

                                txtDocente.Style.Add("background-color", COR_GLP_00000000);
                                txtDocente.Style.Add("color", "black");

                                txtDisciplina.Style.Add("background-color", COR_GLP_00000000);
                                txtDisciplina.Style.Add("color", "black");
                            }
                            else if (tipo_aula.ToUpper() == "GLP_99999999")
                            {
                                txtDocente.Style.Clear();
                                txtDisciplina.Style.Clear();

                                txtDocente.Style.Add("background-color", COR_GLP_99999999);
                                txtDocente.Style.Add("color", "black");

                                txtDisciplina.Style.Add("background-color", COR_GLP_99999999);
                                txtDisciplina.Style.Add("color", "black");
                            }
                        }
                        //Tratamento Contrato Temporario
                        if (txtDocente != null && txtDisciplina != null && !string.IsNullOrEmpty(tipoContrato))
                        {
                            if (tipoContrato.ToUpper() == "CT")
                            {
                                txtDocente.Style.Clear();
                                txtDisciplina.Style.Clear();

                                txtDocente.Style.Add("background-color", COR_CT);
                                txtDocente.Style.Add("color", "black");

                                txtDisciplina.Style.Add("background-color", COR_CT);
                                txtDisciplina.Style.Add("color", "black");

                            }
                            else if (tipoContrato.ToUpper() == "CT_00000000")
                            {
                                txtDocente.Style.Clear();
                                txtDisciplina.Style.Clear();

                                txtDocente.Style.Add("background-color", COR_CT_00000000);
                                txtDocente.Style.Add("color", "black");

                                txtDisciplina.Style.Add("background-color", COR_CT_00000000);
                                txtDisciplina.Style.Add("color", "black");
                            }
                            else if (tipoContrato.ToUpper() == "CT_99999999")
                            {
                                txtDocente.Style.Clear();
                                txtDisciplina.Style.Clear();

                                txtDocente.Style.Add("background-color", COR_CT_99999999);
                                txtDocente.Style.Add("color", "black");

                                txtDisciplina.Style.Add("background-color", COR_CT_99999999);
                                txtDisciplina.Style.Add("color", "black");
                            }
                        }

                        if (td.FindControl("txtCodigoDocente_" + idTextBoxDocente) != null &&
                            td.FindControl("txtCodigoDocente_" + idTextBoxDocente) is HtmlInputHidden)
                        {
                            HtmlInputHidden codigoDocente = (HtmlInputHidden)td.FindControl("txtCodigoDocente_" + idTextBoxDocente);
                            if (codigoDocente != null)
                            {
                                codigoDocente.Value = num_func;
                            }
                        }

                        if (td.FindControl("txtCodigoDisciplina_" + idTextBoxDisciplina) != null &&
                            td.FindControl("txtCodigoDisciplina_" + idTextBoxDisciplina) is HtmlInputHidden)
                        {
                            HtmlInputHidden codigoDisciplina = (HtmlInputHidden)td.FindControl("txtCodigoDisciplina_" + idTextBoxDisciplina);
                            if (codigoDisciplina != null)
                            {
                                codigoDisciplina.Value = disciplina;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Popup Transferência

        protected void LimparPopupTransferencia()
        {
            grdTransferenciaSelecionada.DataSource = null;
            grdTransferenciaSelecionada.DataBind();
            grdTransferenciaSelecionada.Visible = true;

            grdTransferencia.Visible = false;
            grdTransferencia.DataSource = null;
            grdTransferencia.DataBind();

            tseDisciplinaDestino.Value = null;
            tseTurmaTransferencia.Value = null;

            tseDisciplinaDestino.Enabled = false;

            lblTransferenciaMensagem.Text = string.Empty;
            lblTransferenciaMensagem.Visible = false;

            btnRealizarTransferencia.Visible = false;
            btnTransferenciaFechar.Visible = false;

            rbtnCarencia.SelectedIndex = -1;

            AtualizadaEstadoPopupTransferencia(true);
        }

        protected void btnTransferencia_Click(object sender, EventArgs e)
        {
            puTransferencia.ShowOnPageLoad = true;

            LimparPopupTransferencia();

            tseTurmaTransferencia.SqlWhere += String.Format(" and tu.ano = {0} and tu.semestre = {1} and tu.turma <> '{2}'", ddlAno.Text, ddlPeriodo.Text, txtTurma.Text);

            CarregarGridSelecionadas();
        }

        private void CarregarGridSelecionadas()
        {
            List<RN.Turma.AulaSelecionada> aulasSelecionadas = ObterAulasSelecionadas();

            if (aulasSelecionadas.Count == 0)
            {
                lblTransferenciaMensagem.Text = "Nenhuma alocação válida foi selecionada.<br/><br/>Obs.: Não é permitida a transferência de alocação GLP.";
                lblTransferenciaMensagem.Visible = true;
                AtualizadaEstadoPopupTransferencia(false);
                btnTransferenciaFechar.Visible = false;
                puTransferencia.CloseAction = DevExpress.Web.ASPxClasses.CloseAction.CloseButton;
            }
            else
            {
                grdTransferenciaSelecionada.DataSource = aulasSelecionadas;
                grdTransferenciaSelecionada.DataBind();
            }
        }

        public List<RN.Turma.AulaSelecionada> ObterAulasSelecionadas()
        {
            String dadosSelecionados = hdnTransferenciaSelecionada.Value.ToString();

            List<RN.Turma.AulaSelecionada> aulasSelecionadas = new List<RN.Turma.AulaSelecionada>();

            String turma = txtTurma.Text;
            String ano = ddlAno.SelectedValue;
            String semestre = ddlPeriodo.SelectedValue;
            String turno = ddlTurno.SelectedValue;

            String[] splits = dadosSelecionados.Split(new char[] { '{', '}' }).Where(s => !String.IsNullOrEmpty(s)).ToArray();
            foreach (String split in splits)
            {
                string num_func = split.Split(';')[0];
                string disciplina = split.Split(';')[1];
                string id = split.Split(';')[2];

                if (String.IsNullOrEmpty(num_func) || String.IsNullOrEmpty(disciplina))
                    continue;

                string aula = id.Split('_')[1];
                decimal dia_semana = ObterDiaSemana(id);
                string horaIni = id.Split('_')[2] + ":" + id.Split('_')[3];
                string horaFim = id.Split('_')[4] + ":" + id.Split('_')[5];

                RN.Turma.AulaSelecionada aulaSelecionada = new RN.Turma.AulaSelecionada();

                aulaSelecionada.Turma = turma;
                aulaSelecionada.Ano = Convert.ToDecimal(ano);
                aulaSelecionada.Semestre = Convert.ToDecimal(semestre);
                aulaSelecionada.Turno = turno;

                aulaSelecionada.Aula = Convert.ToDecimal(aula);
                aulaSelecionada.DiaSemana = Convert.ToDecimal(dia_semana);
                aulaSelecionada.Disciplina = disciplina;
                aulaSelecionada.NumFunc = Convert.ToDecimal(num_func);

                aulaSelecionada.HoraIni = horaIni;
                aulaSelecionada.HoraFim = horaFim;

                aulasSelecionadas.Add(aulaSelecionada);
            }

            RN.Turma.AulaSelecionada.PreencherAulasSelecionadas(aulasSelecionadas);
            return aulasSelecionadas;
        }

        public void CarregarGridTransferencia()
        {
            if (!tseTurmaTransferencia.DBValue.IsNull && tseTurmaTransferencia.IsValidDBValue &&
                !tseDisciplinaDestino.DBValue.IsNull && tseDisciplinaDestino.IsValidDBValue)
            {
                grdTransferencia.DataBind();
                grdTransferencia.Visible = true;
                btnRealizarTransferencia.Visible = true;
                grdTransferencia.SettingsText.Title = "Alocações Disponíveis e Carências em " + tseDisciplinaDestino["descricao"];
            }
            else
            {
                grdTransferencia.Visible = false;
                btnRealizarTransferencia.Visible = false;
            }
        }

        protected void tseTurmaTransferencia_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (!tseTurmaTransferencia.DBValue.IsNull && tseTurmaTransferencia.IsValidDBValue)
            {

                tseDisciplinaDestino.Enabled = true;
                tseDisciplinaDestino.SqlWhere = String.Format(@"tu.ano = {0} and tu.semestre = {1} and tu.turma = '{2}'
                    and (ghdoc.provisorio = 'N' or (ghdoc.provisorio = 'S' and ghdoc.dt_limite >= convert(date, getdate())))", ddlAno.Text, ddlPeriodo.Text, tseTurmaTransferencia["grade"]);

                List<RN.Turma.AulaSelecionada> aulas = ObterAulasSelecionadas();
                if (aulas != null && aulas.Count > 0)
                    tseDisciplinaDestino.SqlWhere += String.Format(" and ghdoc.num_func = {0}", aulas[0].NumFunc);

                lblTransferenciaMensagem.Text = "";
            }
            else
            {
                tseDisciplinaDestino.Enabled = false;
            }

            tseDisciplinaDestino.ResetValue();

            CarregarGridTransferencia();
        }

        protected void tseDisciplinaDestino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            CarregarGridTransferencia();
            lblTransferenciaMensagem.Text = "";
        }

        protected void grdTransferencia_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.VisibleIndex < 0) return;

            int quantidadeAulasOrigem = grdTransferenciaSelecionada.VisibleRowCount;

            GridViewDataColumn colSelecionar = (GridViewDataColumn)grdTransferencia.Columns["selecionar"];
            ASPxCheckBox cbSelecionar = grdTransferencia.FindRowCellTemplateControl(e.VisibleIndex, colSelecionar, "cbSelecionarAlocacao") as ASPxCheckBox;
            RN.Turma.AulaSelecionada aulaSelecionada = (RN.Turma.AulaSelecionada)grdTransferencia.GetRow(e.VisibleIndex);

            if (cbSelecionar != null && aulaSelecionada != null)
            {
                if (aulaSelecionada.IsCarencia && aulaSelecionada.CarenciaOrderID <= quantidadeAulasOrigem - 1)
                {
                    cbSelecionar.Checked = true;
                    cbSelecionar.ReadOnly = true;
                }
            }
        }

        protected void grdTransferencia_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregarGridTransferencia();
        }

        protected void btnRealizarTransferencia_Click(object sender, ImageClickEventArgs e)
        {
            #region Obtém dados relacionados ao destino
            List<RN.Turma.AulaSelecionada> aulasOrigem = ObterAulasSelecionadas();
            decimal num_func_origem = aulasOrigem.Count > 0 ? aulasOrigem[0].NumFunc : 0M;
            string matricula_origem = aulasOrigem.Count > 0 ? aulasOrigem[0].Matricula : string.Empty;

            String disciplinaDestino = Convert.ToString(tseDisciplinaDestino.DBValue);
            String turmaDestino = Convert.ToString(tseTurmaTransferencia["grade"]);
            decimal ano = Convert.ToDecimal(tseTurmaTransferencia["ano"]);
            decimal semestre = Convert.ToDecimal(tseTurmaTransferencia["semestre"]);
            String turno = Convert.ToString(tseTurmaTransferencia["turno"]);
            #endregion

            List<int> indexes = new List<int>();
            #region Obtém os indices da grid que estão marcadas
            for (int index = 0; index <= grdTransferencia.VisibleRowCount; index++)
            {
                GridViewDataColumn col = (GridViewDataColumn)grdTransferencia.Columns["selecionar"];
                Control control = grdTransferencia.FindRowCellTemplateControl(index, col, "cbSelecionarAlocacao");

                if (!(control is ASPxCheckBox))
                    continue;

                ASPxCheckBox cb = ((ASPxCheckBox)control);
                if (cb != null && cb.Checked)
                    indexes.Add(index);
            }
            #endregion

            List<RN.Turma.AulaSelecionada> aulasDestino = new List<RN.Turma.AulaSelecionada>();
            #region Obtém as aulas correspondentes das linhas marcadas
            foreach (int index in indexes)
            {
                RN.Turma.AulaSelecionada aulaMarcada = grdTransferencia.GetRow(index) as RN.Turma.AulaSelecionada;
                aulasDestino.Add(aulaMarcada);
            }
            #endregion

            String codigoCarencia = string.Empty;
            #region Obtém o código da carência que irá substituir as alocações de origem
            codigoCarencia = rbtnCarencia.SelectedItem != null ? rbtnCarencia.SelectedItem.Value.ToString() : string.Empty;
            #endregion

            RetValue ret = RN.Turma.TransferirAlocacoes(aulasOrigem, aulasDestino, codigoCarencia, disciplinaDestino, turmaDestino, ano, semestre, turno);

            lblTransferenciaMensagem.Visible = true;
            if (ret.Ok)
            {
                lblTransferenciaMensagem.Text = ret.Message.Replace(',', ' ');
                AtualizadaEstadoPopupTransferencia(false);
            }
            else
            {
                lblTransferenciaMensagem.Text = ret.Errors.ToString() + "<br/>";
                AtualizadaEstadoPopupTransferencia(true);

                CarregarGridSelecionadas();
                CarregarGridTransferencia();
            }
        }

        protected void AtualizadaEstadoPopupTransferencia(Boolean inicio)
        {
            btnRealizarTransferencia.Visible = false;
            btnTransferenciaFechar.Visible = !inicio;
            pnlTransferenciaFiltro.Visible = inicio;
            grdTransferenciaSelecionada.Visible = inicio;
            puTransferencia.CloseAction = inicio ? DevExpress.Web.ASPxClasses.CloseAction.CloseButton : DevExpress.Web.ASPxClasses.CloseAction.None;
        }

        protected void btnTransferenciaFechar_Click(object sender, EventArgs e)
        {
            EscondePopupAtualizaPagina();
        }

        protected void EscondePopupAtualizaPagina()
        {
            puTransferencia.ShowOnPageLoad = false;
            Response.Redirect(Request.Url.ToString());
        }

        #endregion

        protected void chkEletiva_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RN.Turma rnTurma = new Techne.Lyceum.RN.Turma();
                trTurmaReferencia.Visible = false;
                ddlTurmaReferencia.Enabled = false;

                if (chkEletiva.Checked)
                {
                    if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel.DBValue.IsNull))
                    {
                        trTurmaReferencia.Visible = true;

                        ddlTurmaReferencia.DataSource = rnTurma.ListaTurmasParaReferenciaPor(ddlAno.SelectedValue, ddlPeriodo.SelectedValue, tseUnidadeResponsavel.DBValue.ToString());
                        ddlTurmaReferencia.DataBind();
                        ddlTurmaReferencia.Items.Insert(0, new ListItem("Selecione", string.Empty));
                        ddlTurmaReferencia.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkAmbienteExterno_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAmbienteExterno.Checked)
            {
                ddlDependencia.Enabled = false;
                txtNumMaxAluno.Enabled = false;
                txtNumMaxAluno.ReadOnly = true;

                ddlDependencia.ClearSelection();
                if (ddlDependencia.Items.Count > 0)
                {
                    ddlDependencia.Items.FindByValue("").Selected = true;
                }
            }
            else if (ddlCurso.Value.ToString() != "9999.92")
            {
                ddlDependencia.Enabled = true;
                txtNumMaxAluno.Enabled = true;
                txtNumMaxAluno.ReadOnly = false;
            }

            PopularNumeroAlunoDependencia();
            ControlarVisibilidadeDependencia();
        }

        private int RetornaNumeroMaximoAluno()
        {
            decimal numAluno = 0;
            int salaCapacidadeMunicipio = 0;
            RN.CapacidadeAlunoTurmaMunicipio rnCapacidade = new RN.CapacidadeAlunoTurmaMunicipio();

            if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
            {
                salaCapacidadeMunicipio = rnCapacidade.RetornaCapacidadePor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), tseUnidadeResponsavel["municipio"].ToString());

                if (salaCapacidadeMunicipio == -1)
                {
                    var capacidade = RN.CapacidaDeAlunoTurma.Carregar(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlCurso.SelectedItem.Value.ToString());

                    if (capacidade.CapacidaDeAlunoTurmaId > 0)
                    {
                        NumMaxAluno = capacidade.CapacidadeMaxima;

                        //Alimenta Label de Capacidade da turma pelo curso
                        txtCapacidadeTurma.Text = capacidade.CapacidadeMaxima.ToString();
                    }
                    else
                    {
                        if (chkAmbienteExterno.Checked)
                            NumMaxAluno = 45;
                    }
                }
                else
                {
                    NumMaxAluno = salaCapacidadeMunicipio;

                    //Alimenta Label de Capacidade da turma pelo municipio
                    txtCapacidadeTurma.Text = salaCapacidadeMunicipio.ToString();
                }
            }

            if (!string.IsNullOrEmpty(ddlDependencia.SelectedValue))
            {
                numAluno = RN.Dependencia.ObterNumeroAluno(Convert.ToString(tseUnidadeFisica.DBValue), ddlDependencia.SelectedValue);

                //Alimenta Label de Capacidade da sala:
                txtCapacidadeSala.Text = numAluno.ToString();

                if (NumMaxAluno == -1)
                {
                    NumMaxAluno = Convert.ToInt32(numAluno);
                }
            }

            if (ddlOptativaReforco.SelectedValue == "S")
            {
                //Busca maximo de alunos para turma reforço
                NumMaxAluno = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["MaxAlunoReforcoPorTurma"]);
            }

            if (NumMaxAluno > 0)
            {
                return NumMaxAluno;
            }
            else
            {
                return 0;
            }

        }

        protected void btnSimTurmaDependencia_Click(object sender, EventArgs e)
        {
            try
            {
                hdnValidaDependTurma.Value = "validado";
                btnSalvar_Click(sender, null);
                this.pucConfirmarTurma.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                hdnValidaDependTurma.Value = string.Empty;
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNaoTurmaDependencia_Click(object sender, EventArgs e)
        {
            hdnValidaDependTurma.Value = string.Empty;
            lblMensagemTurmaDependencia.Text = "Não";
            this.pucConfirmarTurma.ShowOnPageLoad = false;

        }
    }
}