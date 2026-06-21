using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;
using RN = Techne.Lyceum.RN;
using System.Linq;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Curriculo
{
    [
        NavUrl("~/Curriculo/ListarTurma.aspx"),
        ControlText("ListarTurma"),
        Title("Turmas"),
    ]
    public partial class ListarTurma : TPage
    {
        #region Propriedades

        private string Curriculo
        {
            get { return (string)ViewState["Curriculo"]; }
            set { ViewState["Curriculo"] = value; }
        }

        private Techne.Lyceum.RN.Turma.DadosTurma ObjetoTurma
        {
            get { return (Techne.Lyceum.RN.Turma.DadosTurma)ViewState["ObjetoTurma"]; }
            set { ViewState["ObjetoTurma"] = value; }
        }

        private Techne.Lyceum.RN.Turma.DadosTurma ObjetoTurmaPesquisa
        {
            get { return (Techne.Lyceum.RN.Turma.DadosTurma)ViewState["ObjetoTurmaPesquisa"]; }
            set { ViewState["ObjetoTurmaPesquisa"] = value; }
        }

        private decimal AnoVigente
        {
            get
            {
                if (ViewState["ano_vigente"] == null)
                    ViewState["ano_vigente"] = RN.PeriodoLetivo.ConsultarAnoLetivoAtual();
                return (decimal)ViewState["ano_vigente"];
            }
        }

        #endregion

        #region Eventos

        protected void Page_Init(object sender, EventArgs e)
        {
            //TituloGrid(grdGradeSerie, "Turmas");
            ControlaAcessoGridTurmas();
        }

        protected void ControlaAcessoGridTurmas()
        {
            foreach (GridViewColumn col in grdGradeSerie.Columns)
            {
                if (col is GridViewCommandColumn)
                {
                    if (((GridViewCommandColumn)col).CustomButtons["btnDesativar"] != null)
                        ((GridViewCommandColumn)col).CustomButtons["btnDesativar"].Visibility = 
                            Permission.AllowDelete && ddlSitTurma.SelectedValue == "Aberta" ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;

                    if (((GridViewCommandColumn)col).CustomButtons["btnReativar"] != null)
                        ((GridViewCommandColumn)col).CustomButtons["btnReativar"].Visibility = 
                            Permission.AllowDelete && ddlSitTurma.SelectedValue == "Desativada" ? GridViewCustomButtonVisibility.AllDataRows : GridViewCustomButtonVisibility.Invisible;

                    ((GridViewCommandColumn)col).EditButton.Visible = ddlSitTurma.SelectedValue != "Desativada";
                    ((GridViewCommandColumn)col).DeleteButton.Visible = ddlSitTurma.SelectedValue != "Desativada";
                    ((GridViewCommandColumn)col).SelectButton.Visible = ddlSitTurma.SelectedValue != "Desativada";
                }
            }

            //verifica acesso do usuário para os controles da página
            ControlaAcesso(grdGradeSerie);
        }

        protected void ddlSitTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarExibirGrid();
        }

        protected void ddlTipoTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarExibirGrid();
        }

        protected void CarregaSituacaoFuncionamento()
        {
            ddlSituacaoFunc.Items.Clear();
            ddlSituacaoFunc.DataSource = Techne.Lyceum.RN.Basico.ConsultaItemTabelaValDescr("SitFuncionamentoUE");
            ddlSituacaoFunc.DataBind();
            ddlSituacaoFunc.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcessoGridTurmas();

            foreach (ListItem item in ddlAno.Items)
            {                
                if (item.Value == Convert.ToString(AnoVigente + 1))
                    item.Attributes.Add("style", "color:Red");
                else
                    item.Attributes.Add("style", "color:#0353AB");
            }
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDadosDrop(ddlAno.ID);
                CarregaSituacaoFuncionamento();
                PreencherDadosSession();

                ddlSituacaoFunc.SelectedValue = "EmAtividade";

                //verifica se existe alguma querystring
                if (Request.QueryString.Keys.Count > 0)
                    CarregarDadosTurma();
                else
                    grdGradeSerie.Enabled = false;
            }

            DefinirFiltroUnidadeEnsino();
            //Carrega grid de acordo com os filtros selecionados pelo usuário
            //         CarregarGrid();

            CarregarExibirGrid();
        }

        protected void CarregarExibirGrid()
        {
            lblMensagem.Text = string.Empty;
            ObjetoTurmaPesquisa = null;

            ObterDadosTelaPesquisa();
            CarregarGrid();

            grdGradeSerie.Enabled = true;


            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            if (sessao != null)
                sessao.Periodo = string.Empty;
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            //ignora callbacks causados controles
            if (Page.IsCallback)
                return;
          
            lblUAValor.Text = string.Empty;
         

            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

            if (sessao != null)
            {
                if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
                {
                    sessao.Municipio = tseMunicipio.DBValue.ToString();
                    tseUnidadeResponsavel.ResetValue();

                }

            }
            CarregarExibirGrid();
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            //ignora callbacks causados controles
            if (Page.IsCallback)
                return;

            lblUAValor.Text = string.Empty;
            
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            if (sessao != null)
            {
                if (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue)
                {
                    sessao.Regional = Convert.ToString(tseRegional.DBValue);
                    tseUnidadeResponsavel.ResetValue();
                }
            }
            CarregarExibirGrid();
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            //ignora callbacks causados controles
            if (Page.IsCallback)
                return;

            lblUAValor.Text = string.Empty;
            
            ObjetoTurmaPesquisa = null;

            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

            if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel["unidade_ens"].IsNull)
            {
                lblUAValor.Text = Convert.ToString(tseUnidadeResponsavel["UA_ATUAL"]);
              
                tseRegional.Value = tseUnidadeResponsavel["id_regional"];
                tseMunicipio.Value = tseUnidadeResponsavel["municipio"];

                    if (sessao != null)
                    {
                        sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);                       
                        sessao.Municipio = tseMunicipio.DBValue.ToString();
                        sessao.Regional = tseUnidadeResponsavel["ID_REGIONAL"].ToString();
                    }
            }
            CarregarExibirGrid();
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {

            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            if (sessao != null && !string.IsNullOrEmpty(ddlAno.SelectedValue))
            {
                sessao.Ano = ddlAno.SelectedValue;
            }
            CarregarExibirGrid();
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            CarregarExibirGrid();
        }

        /// <summary>
        /// Evento de Click do ImageButton de busca
        /// </summary>
        /// <param name="sender">objeto do sistema</param>
        /// <param name="e">argumento do sistema</param>
        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            CarregarExibirGrid();
        }

        private bool PodeExecutarOperacaoPorNomeRetornoGridGradeSerie(string nomeRetorno)
        {
            return nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.ADDNEWROW)
                || nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.STARTEDIT)
                || nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.SELECTION)
                || nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.REATIVAR);
        }

        private RN.Turma.DadosTurma PreencheTurmaComValoresObtidosDaGridGradeSerieParaAlteracao(ASPxGridViewAfterPerformCallbackEventArgs gridGradeSerie)
        {
            RN.Turma.DadosTurma turma = new RN.Turma.DadosTurma();

            turma.Grade = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "grade"));
            turma.Faculdade = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "faculdade"));
            turma.Turno = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "turno"));
            turma.Curso = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "curso"));
            turma.Ano = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "ano"));
            turma.Periodo = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "semestre"));
            turma.Serie = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "serie"));
            turma.Grade_ID = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "grade_id"));
            turma.Sufixo = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "sufixo"));
            turma.UnidadeResponsavel = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "unidade_responsavel"));
            turma.Tipogestao = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "tipo_gestao"));
            turma.Dependencia = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gridGradeSerie.Args[0], "dependencia"));

            turma.Municipio = ObjetoTurmaPesquisa.Municipio;
            turma.Nucleo = ObjetoTurmaPesquisa.Nucleo;

            return turma;
        }

        private RN.Turma.DadosTurma PreencheTurmaComValoresObtidosDaGridGradeSerieParaConsulta(ASPxGridViewAfterPerformCallbackEventArgs gridGradeSerie)
        {
            RN.Turma.DadosTurma turma = new RN.Turma.DadosTurma();

            turma.Grade = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "grade"));
            turma.Faculdade = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "faculdade"));
            turma.Turno = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "turno"));
            turma.Curso = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "curso"));
            turma.Ano = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "ano"));
            turma.Periodo = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "semestre"));
            turma.Serie = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "serie"));
            turma.Grade_ID = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "grade_id"));
            turma.Sufixo = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "sufixo"));
            turma.UnidadeResponsavel = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "unidade_responsavel"));
            Curriculo = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "curriculo"));
            turma.Tipogestao = Convert.ToString(grdGradeSerie.GetRowValues(GetSelectedRowOnTheCurrentPage(), "tipo_gestao"));

            turma.Municipio = ObjetoTurmaPesquisa.Municipio;
            turma.Nucleo = ObjetoTurmaPesquisa.Nucleo;

            return turma;
        }

        protected void grdGradeSerie_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            string tipoOperacao = string.Empty;

            //if ((e.CallbackName == "ADDNEWROW") || (e.CallbackName == "STARTEDIT" || e.CallbackName == "SELECTION"))
            if (PodeExecutarOperacaoPorNomeRetornoGridGradeSerie(e.CallbackName))
            {
                //string tipoOperacao = string.Empty;    

                RN.Turma.DadosTurma turma = new RN.Turma.DadosTurma();

                //if (e.CallbackName == "ADDNEWROW")
                if(e.CallbackName.Equals(Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.ADDNEWROW)))
                {
                    //tipoOperacao = "NOVO";
                    tipoOperacao = Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.NOVO);

                    if (string.IsNullOrEmpty(ddlAno.SelectedValue))
                    {
                        throw new Exception("O Campo ANO é de preenchimento obrigatório.");
                    }

                    if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel["unidade_ens"].IsNull)
                    {
                        turma = new RN.Turma.DadosTurma();
                        turma.Ano = ObjetoTurmaPesquisa.Ano;
                        turma.Periodo = ObjetoTurmaPesquisa.Periodo;
                        turma.Nucleo = ObjetoTurmaPesquisa.Nucleo;
                        turma.Municipio = ObjetoTurmaPesquisa.Municipio;
                        turma.UnidadeResponsavel = ObjetoTurmaPesquisa.UnidadeResponsavel;
                        turma.MnemonicoUnidadeResponsavel = ObjetoTurmaPesquisa.MnemonicoUnidadeResponsavel;
                    }
                    else
                    { 
                         throw new Exception("O Campo UNIDADE DE ENSINO é de preenchimento obrigatório.");
                    }
                }
                //else if (e.CallbackName == "STARTEDIT")
                else if (e.CallbackName.Equals(Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.STARTEDIT)))
                {
                    //tipoOperacao = "ALTERAR";
                    tipoOperacao = Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.ALTERAR);
                    turma = PreencheTurmaComValoresObtidosDaGridGradeSerieParaAlteracao(e);

                    Curriculo = grdGradeSerie.GetRowValuesByKeyValue(e.Args[0], "curriculo").ToString();

                    if (!RN.Turma.VerificaTurmaVigente(turma.Grade, turma.Ano, turma.Periodo))
                        throw new Exception("Não é possível alterar os dados pois a turma não está mais vigente.");
                }
                //else if (e.CallbackName == "SELECTION")
                else if (e.CallbackName.Equals(Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.SELECTION)))
                {
                    //tipoOperacao = "CONSULTAR";
                    tipoOperacao = Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.CONSULTAR);

                    turma = PreencheTurmaComValoresObtidosDaGridGradeSerieParaConsulta(e);
                }
              

                string queryString = MontarQueryString(tipoOperacao, turma);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                ASPxWebControl.RedirectOnCallback("Turma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }

            ControlaAcessoGridTurmas();
        }

        private RN.Turma.DadosTurma PreencheTurmaComValoresObtidosDaGridGradeSerieParaExclusao(ASPxDataDeletingEventArgs argumento)
        {
            RN.Turma.DadosTurma turma = new RN.Turma.DadosTurma();
            string gradeId = Convert.ToString(argumento.Keys["grade_id"]);

            if (!string.IsNullOrEmpty(gradeId))
            {
                turma.Grade = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "grade"));
                turma.Faculdade = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "faculdade"));
                turma.Turno = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "turno"));
                turma.Curso = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "curso"));
                turma.Ano = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "ano"));
                turma.Periodo = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "semestre"));
                turma.Serie = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "serie"));
                turma.Nucleo = ObjetoTurmaPesquisa.Nucleo;
                turma.Sufixo = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "sufixo"));
                turma.Dependencia = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "dependencia"));
                turma.UnidadeResponsavel = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "unidade_responsavel"));
                //ObterCurriculo(turma.Curso, turma.Turno, turma.Ano, turma.Periodo);
                Curriculo = Convert.ToString(grdGradeSerie.GetRowValuesByKeyValue(gradeId, "curriculo"));

                turma.Grade_ID = gradeId;
                turma.Municipio = ObjetoTurmaPesquisa.Municipio;
            }

            return turma;
        }

        protected void grdGradeSerie_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            //Techne.Lyceum.RN.Turma.DadosTurma turma = null;
            RN.Turma.DadosTurma turma = new RN.Turma.DadosTurma();

            //string grade_id = Convert.ToString(e.Keys["grade_id"]);

            //if (!string.IsNullOrEmpty(grade_id))
            //{

            turma = PreencheTurmaComValoresObtidosDaGridGradeSerieParaExclusao(e);
                
            //}



            string queryString = MontarQueryString("EXCLUIR", turma);

            String nometurma = e.Values["grade_token"] + "-" + lblUAValor.Text;
            if (!RN.Turma.PodeExcluirTurma(turma.Turno, turma.Faculdade, nometurma, turma.Ano, turma.Periodo))
            {
                e.Cancel = true;
                throw new ApplicationException("Não é possível excluir a turma.\nExistem aulas alocadas no Quadro de Horários.");
            }

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
            ASPxWebControl.RedirectOnCallback("Turma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        protected void grdGradeSerie_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "em_elaboracao")
            {
                String em_elaboracao = Convert.ToString(e.CellValue).Trim().ToUpper();
                if (em_elaboracao == "HORÁRIO INCOMPLETO" || em_elaboracao == "SEM ALOCAÇÃO")
                    e.Cell.ForeColor = System.Drawing.Color.Red;
            }
            else if (e.DataColumn.FieldName == "ano")
            {
                if (Convert.ToString(e.CellValue) == Convert.ToString(AnoVigente + 1))
                    e.Cell.ForeColor = System.Drawing.Color.Red;                
            }
        }

        protected void grdGradeSerie_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            RN.Turma rnTurma = new Techne.Lyceum.RN.Turma();
            RN.Turma.DadosTurma dadosTurma = new RN.Turma.DadosTurma();

            try
            {
                string turma = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "grade"));
                string ano = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "ano"));
                string semestre = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "semestre"));
                string turno = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "turno"));
                string faculdade = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "faculdade"));
                string dependencia = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "dependencia"));

                if (e.ButtonID == "btnDesativar")
                {
                    RN.RetValue retDesativacao = RN.Turma.DesativarTurma(turma, ano, semestre, turno, faculdade);
                    if (retDesativacao != null && !retDesativacao.Ok)
                        throw new Exception(retDesativacao.Errors.ToString());

                    CarregarGrid();
                }
                else if (e.ButtonID == "btnReativar")
                {
                    if (!rnTurma.PossuiTurmaAbertaMesmaSalaETurnoPor(Convert.ToDecimal(ano), Convert.ToInt32(semestre), dependencia, turno, faculdade))
                    {
                        rnTurma.ReativaTurmaPor(turma, ano, semestre);
                        CarregarGrid();
                    }
                    else
                    {
                        dadosTurma.Grade = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "grade"));
                        dadosTurma.Faculdade = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "faculdade"));
                        dadosTurma.Turno = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "turno"));
                        dadosTurma.Curso = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "curso"));
                        dadosTurma.Ano = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "ano"));
                        dadosTurma.Periodo = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "semestre"));
                        dadosTurma.Serie = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "serie"));
                        dadosTurma.Grade_ID = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "grade_id"));
                        dadosTurma.Sufixo = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "sufixo"));
                        dadosTurma.UnidadeResponsavel = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "unidade_responsavel"));
                        dadosTurma.Tipogestao = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "tipo_gestao"));
                        dadosTurma.Dependencia = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "dependencia"));

                        dadosTurma.Municipio = ObjetoTurmaPesquisa.Municipio;
                        dadosTurma.Nucleo = ObjetoTurmaPesquisa.Nucleo;                       

                        Curriculo = Convert.ToString(grdGradeSerie.GetRowValues(e.VisibleIndex, "curriculo"));
                        string queryString = MontarQueryString("REATIVAR", dadosTurma);
                        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                        ASPxWebControl.RedirectOnCallback("Turma.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                    }                   
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

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

        #region Métodos

        private void DefinirFiltroUnidadeEnsino()
        {
            string sqlWhere = string.Empty;
            string table = string.Empty;

            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();

            table = " VW_UNIDADE_ENSINO_SITUACAO ";

            coluna.Add("unidade_ens");
            coluna.Add("nome_comp");
            coluna.Add("setor");
            coluna.Add("cgc");
            coluna.Add("situacao");
            coluna.Add("nucleo");
            coluna.Add("municipio");
            coluna.Add("id_regional");
            coluna.Add("ua_atual");
            coluna.Add("ua_antiga");

            if (!tseRegional.DBValue.IsNull)
            {
                if (tseRegional.IsValidDBValue)
                    sqlWhere = " id_regional = " + Convert.ToString(tseRegional.DBValue);
            }

            if (!tseMunicipio.DBValue.IsNull)
            {
                if (tseMunicipio.IsValidDBValue)
                {
                    if (string.IsNullOrEmpty(sqlWhere))
                        sqlWhere = "municipio = " + Convert.ToString(tseMunicipio.DBValue);
                    else
                        sqlWhere += " AND municipio = " + Convert.ToString(tseMunicipio.DBValue);
                }
            }

            if (!ddlSituacaoFunc.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                 if (string.IsNullOrEmpty(sqlWhere))
                     sqlWhere = " SITUACAO_FUNCIONAMENTO = '" + ddlSituacaoFunc.SelectedValue + "'";
                  else
                     sqlWhere += " AND SITUACAO_FUNCIONAMENTO = '" + ddlSituacaoFunc.SelectedValue + "'";
            }

            if (coluna.Count > 0)
            {
                Techne.Library.Sql.Structure.SqlSelect sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

                tseUnidadeResponsavel.SqlSelect = sqlSelect;
                tseUnidadeResponsavel.SqlWhere = sqlWhere;

                tseUnidadeResponsavel.DataBind();
            }
        }

        private void PreencherDadosSession()
        {
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();


            if (sessao != null)
            {
                if (!string.IsNullOrEmpty(sessao.Ano))
                {
                    if (ddlAno.Items.FindByValue(sessao.Ano) != null)
                    {
                        ddlAno.SelectedValue = sessao.Ano;
                        if (ObjetoTurma == null)
                            ObjetoTurma = new Techne.Lyceum.RN.Turma.DadosTurma();

                        ObjetoTurma.Ano = sessao.Ano;
                    }

                }
                if (!string.IsNullOrEmpty(sessao.Coordenadoria))
                {
                    tseRegional.DBValue = sessao.Coordenadoria;
                    if (!tseRegional.IsValidDBValue)
                    {
                        tseRegional.Msg = string.Empty;
                        tseRegional.ResetValue();
                    }
                }
                if (!string.IsNullOrEmpty(sessao.Municipio))
                {
                    tseMunicipio.DBValue = sessao.Municipio;
                    if (!tseMunicipio.IsValidDBValue)
                    {
                        tseMunicipio.Msg = string.Empty;
                        tseMunicipio.ResetValue();
                    }
                }
                if (!string.IsNullOrEmpty(sessao.Escola))
                {
                    lblUAValor.Text = string.Empty;                   

                    tseUnidadeResponsavel.DBValue = sessao.Escola;

                    if (!tseUnidadeResponsavel.IsValidDBValue)
                    {
                        tseUnidadeResponsavel.Msg = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                    else
                    {
                        if (!tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            lblUAValor.Text = Convert.ToString(tseUnidadeResponsavel["UA_ATUAL"]);
                            
                        }
                    }
                }
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdGradeSerie.PageIndex * grdGradeSerie.SettingsPager.PageSize;
            for (int i = 0; i < grdGradeSerie.VisibleRowCount; i++)
            {
                if (grdGradeSerie.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }

        [Obsolete("Método incorreto: pode existir mais de um curriculo para ano/periodo/curso/turno")]
        private void ObterCurriculo(string curso, string turno, string ano, string periodo)
        {
            QueryTable qt = null;
            Curriculo = string.Empty;
            if (!string.IsNullOrEmpty(curso) && !string.IsNullOrEmpty(turno) && !string.IsNullOrEmpty(ano) && !string.IsNullOrEmpty(periodo))
            {
                qt = RN.Curriculo.Consultar(turno, curso, Convert.ToDecimal(ano), Convert.ToDecimal(periodo));

                if (qt != null)
                {
                    if (qt.Rows.Count > 0)
                        Curriculo = Convert.ToString(qt.Rows[0]["CURRICULO"]);
                }
            }
        }

        private void ObterDadosQueryString(string queryString)
        {
            ObjetoTurma = new Techne.Lyceum.RN.Turma.DadosTurma();
            lblMensagem.Text = string.Empty;
            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("ano") >= 0)
                    ObjetoTurma.Ano = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("semestre") >= 0)
                    ObjetoTurma.Periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("nucleo") >= 0)
                    ObjetoTurma.Nucleo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("municipio") >= 0)
                    ObjetoTurma.Municipio = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("grade") >= 0)
                    ObjetoTurma.Grade = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("mensagem") >= 0)
                    lblMensagem.Text = dados.Substring(dados.LastIndexOf('=') + 1);
            }
        }

        private void CarregarDadosTurma()
        {
            try
            {
                byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                ObterDadosQueryString(decodedText);

                CarregarDadosDrop(ddlAno.ID);

                grdGradeSerie.Enabled = true;

                ObterDadosTelaPesquisa();
                ObjetoTurma = null;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private DataTable DataTableEstrutura()
        {
            DataTable dtEstrutura = new DataTable();

            dtEstrutura.Columns.Add("grade_id", typeof(int));
            dtEstrutura.Columns.Add("ano", typeof(int));
            dtEstrutura.Columns.Add("semestre", typeof(int));
            dtEstrutura.Columns.Add("curso", typeof(string));
            dtEstrutura.Columns.Add("nomeCurso", typeof(string));
            dtEstrutura.Columns.Add("turno", typeof(string));
            dtEstrutura.Columns.Add("descricaoTurno", typeof(string));
            dtEstrutura.Columns.Add("serie", typeof(string));
            dtEstrutura.Columns.Add("descricaoSerie", typeof(string));
            dtEstrutura.Columns.Add("grade", typeof(string));
            dtEstrutura.Columns.Add("unidade_responsavel", typeof(string));
            dtEstrutura.Columns.Add("nomeUnidadeResponsavel", typeof(string));            
            dtEstrutura.Columns.Add("dependencia", typeof(string));
            dtEstrutura.Columns.Add("em_elaboracao", typeof(string));
            dtEstrutura.Columns.Add("grade_token", typeof(string));
            dtEstrutura.Columns.Add("tipo_gestao", typeof(string));
            dtEstrutura.Columns.Add("tipo_alternativa", typeof(string));
            dtEstrutura.Columns.Add("turmareferencia", typeof(string));
            dtEstrutura.Columns.Add("capacidadeSala", typeof(string));
            dtEstrutura.Columns.Add("num_alunos", typeof(string));
            dtEstrutura.Columns.Add("matriculadosprincipal", typeof(string));
            dtEstrutura.Columns.Add("matriculadoseletivas", typeof(string));

            return dtEstrutura;
        }

        private void ObterDadosTelaPesquisa()
        {
            if (ObjetoTurmaPesquisa == null)
                ObjetoTurmaPesquisa = new Techne.Lyceum.RN.Turma.DadosTurma();

            ObjetoTurmaPesquisa.Ano = ddlAno.SelectedValue;

            if (!tseUnidadeResponsavel.DBValue.IsNull)
                ObjetoTurmaPesquisa.UnidadeResponsavel = tseUnidadeResponsavel.DBValue.ToString();

            if (!tseRegional.DBValue.IsNull)
            {
                if (tseRegional.IsValidDBValue)
                    ObjetoTurmaPesquisa.Regional = tseRegional.DBValue.ToString();
            }

            if (!tseMunicipio.DBValue.IsNull)
            {
                if (tseMunicipio.IsValidDBValue)
                    ObjetoTurmaPesquisa.Municipio = tseMunicipio.DBValue.ToString();
            }

            Ly_unidade_ensino.Row rowUE = RN.UnidadeEnsino.Consultar(Convert.ToString(tseUnidadeResponsavel.DBValue));
            if (rowUE != null)
            {
                ObjetoTurmaPesquisa.Nucleo = rowUE.Nucleo;
                ObjetoTurmaPesquisa.Municipio = rowUE.Municipio;
            }
        }

        private void CarregarGrid()
        {
            QueryTable qtGradeSerie = null;

            try
            {
                if (ObjetoTurmaPesquisa != null)
                {
                    if (!string.IsNullOrEmpty(ObjetoTurmaPesquisa.Ano) && !string.IsNullOrEmpty(ObjetoTurmaPesquisa.UnidadeResponsavel))
                    {
                        string sit_turma = ddlSitTurma.SelectedValue;
                        string tipoTurma = ddlTipoTurma.SelectedValue;

                        qtGradeSerie = RN.GradeSerie.ConsultarTurma(ObjetoTurmaPesquisa.Ano, ObjetoTurmaPesquisa.Periodo, ObjetoTurmaPesquisa.Nucleo,
                                                               ObjetoTurmaPesquisa.Municipio, ObjetoTurmaPesquisa.UnidadeResponsavel, null, sit_turma, tipoTurma);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

            if (qtGradeSerie != null)
            {
                if (qtGradeSerie.Rows.Count > 0)
                    grdGradeSerie.DataSource = qtGradeSerie;
                else
                    grdGradeSerie.DataSource = DataTableEstrutura();
            }
            else
                grdGradeSerie.DataSource = DataTableEstrutura();

            grdGradeSerie.DataBind();

            TituloGridTurmas(ObjetoTurmaPesquisa.Ano);
        }

        private void TituloGridTurmas(string ano)
        {
            String titulo = "Turmas " + ano;
            bool corVermelha = Convert.ToString(ano) == Convert.ToString(AnoVigente + 1);

            if (corVermelha)
                titulo = String.Format("<span style=\"color:red\">{0}</span>", titulo);

            if (grdGradeSerie.SettingsText.Title.Contains("|Tabela:|"))
                grdGradeSerie.SettingsText.Title = grdGradeSerie.SettingsText.Title.Replace("|Tabela:|", titulo);
            else
            {
                int[] startIndex = new int[] { grdGradeSerie.SettingsText.Title.IndexOf("Turmas "),
                    grdGradeSerie.SettingsText.Title.IndexOf("<span style=") };
                grdGradeSerie.SettingsText.Title = grdGradeSerie.SettingsText.Title.Substring(0, startIndex.Where(s => s > 0).Min()) + titulo;
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

        /// <summary>
        /// Adiciona no dropdownlist passado como parametro um item vazio
        /// </summary>
        /// <param name="drop">Dropdownlist</param>
        /// <param name="selecionado">se o item vazio deve ser selecionado</param>
        private void CriarItemVazio(DropDownList drop, bool selecionado)
        {
            //ListItem itemVazio = new ListItem("<Selecione>", "-1");
            //if (!drop.Items.Contains(itemVazio))
            //    drop.Items.Add(itemVazio);

            //if (selecionado)
            //{
            //    drop.ClearSelection();
            //    //seleciona o item vazio
            //    drop.Items.FindByValue("-1").Selected = true;
            //}
        }

        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLANO":
                        {
                            dadosDrop = RN.PeriodoLetivo.ConsultarAno();

                            List<DropDownList> listaDrop = new List<DropDownList>();

                            string anoAtual = Convert.ToString(AnoVigente);

                            if ((ObjetoTurma == null) || (ObjetoTurma != null && string.IsNullOrEmpty(ObjetoTurma.Ano)))
                            {
                                CarregarDropDownList(ddlAno, dadosDrop, listaDrop, anoAtual);
                                CriarItemVazio(ddlAno, false);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(ObjetoTurma.Ano))
                                    CarregarDropDownList(ddlAno, dadosDrop, listaDrop, ObjetoTurma.Ano);
                                else
                                    CarregarDropDownList(ddlAno, dadosDrop, listaDrop, anoAtual);
                            }                       
                            break;
                        }
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

        private string MontarQueryString(string tipoOperacao, Techne.Lyceum.RN.Turma.DadosTurma turma)
        {
            string queryString = string.Empty;
                        
            if (turma != null)
            {
                queryString += "tipoOperacao=" + tipoOperacao;
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
                queryString += "&tipogestao=" + turma.Tipogestao;
                queryString += "&dependencia=" + turma.Dependencia;

                if (!string.IsNullOrEmpty(Curriculo))
                    queryString += "&curriculo=" + Curriculo;
            }
            return queryString;
        }
        #endregion
    }
}
