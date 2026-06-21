using System;
using System.Web;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;
using Techne.Lyceum.RN;
using System.Web.UI;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Academico
{
    [
     NavUrl("~/Academico/ListarEncerramentoAluno.aspx"),
      ControlText("ListarEncerramentoAluno"),
      Title("Encerramento de Aluno"),
    ]

    public partial class ListarEncerramentoAluno : TPage
    {
        #region Propriedades e Enumeradores

        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            Retorno
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion

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

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdEncerramentos, "Encerramentos do Aluno");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        _tipoOperacao = TipoOperacao.Retorno;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        lblMensagem.Text = string.Empty;
                        btnEncerrar.Visible = false;
                        btnReabrir.Visible = false;
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdEncerramentos);
            ControlaAcesso(btnEncerrar, AcaoControle.novo);
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        _tipoOperacao = TipoOperacao.Consultar;
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        _tipoOperacao = TipoOperacao.Inicial;
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdEncerramentos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdEncerramentos);
        }

        protected void grdEncerramentos_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string curso = Convert.ToString(e.GetListSourceFieldValue("curso"));
                string turno = Convert.ToString(e.GetListSourceFieldValue("turno"));
                string curriculo = Convert.ToString(e.GetListSourceFieldValue("curriculo"));
                string aluno = Convert.ToString(e.GetListSourceFieldValue("aluno"));
                string dt_encerramento = Convert.ToString(e.GetListSourceFieldValue("dt_encerramento"));

                e.Value = curso + "|" + turno + "|" + curriculo + "|" + aluno + "|" + dt_encerramento;
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdEncerramentos.PageIndex * grdEncerramentos.SettingsPager.PageSize;
            for (int i = 0; i < grdEncerramentos.VisibleRowCount; i++)
            {
                if (grdEncerramentos.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    return startIndexOnPage + i;
                }
            }
            return -1;
        }

        private void RedirecionarPagina()
        {
            HttpContext.Current.Items.Add("chave", "1000");

            Server.Transfer("EncerramentoAluno.aspx");
        }

        private string MontarQueryString(string tipoOperacao, Ly_h_cursos_concl.Row dadosEncerramento)
        {
            string queryString = string.Empty;

            queryString += "Operacao=" + tipoOperacao;
            queryString += "&aluno=" + dadosEncerramento.Aluno;
            queryString += "&dt_encerramento=" + dadosEncerramento.Dt_encerramento;
            queryString += "&dt_reabertura=" + dadosEncerramento.Dt_reabertura;
            queryString += "&motivo=" + dadosEncerramento.Motivo;
            queryString += "&instituicao=" + dadosEncerramento.Outra_faculdade;
            queryString += "&dt_colacao=" + dadosEncerramento.Dt_colacao;
            queryString += "&dt_diploma=" + dadosEncerramento.Dt_diploma;
            queryString += "&ano_encerramento=" + dadosEncerramento.Ano_encerramento;
            queryString += "&periodo_encerramento=" + dadosEncerramento.Sem_encerramento;
            queryString += "&causa=" + dadosEncerramento.Causa_encerr;
            queryString += "&reabertura=" + dadosEncerramento.Motivoreabertura;
            queryString += "&pessoa=" + tseAluno["pessoa"].ToString();
            queryString += "&comp=" + hdnCompartilhada.Value;
            queryString += "&anocp=" + hdnAno.Value;
            queryString += "&periodocp=" + hdnPeriodo.Value;
            queryString += "&cursocp=" + hdnCurso.Value;
            queryString += "&seriecp=" + hdnSerie.Value;

            return queryString;
        }

        private string MontarQueryStringAluno(string tipoOperacao, Ly_aluno.Row dadosAluno)
        {
            string queryString = string.Empty;

            queryString += "Operacao=" + tipoOperacao;
            queryString += "&aluno=" + dadosAluno.Aluno;
            queryString += "&ano_ingresso=" + dadosAluno.Ano_ingresso;
            queryString += "&periodo_ingresso=" + dadosAluno.Sem_ingresso;
            queryString += "&pessoa=" + dadosAluno.Pessoa;

            return queryString;
        }

        private string ObterDadosQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');
            string aluno = null;
            string mensagem = null;
            string compartilhada = string.Empty;
            string ano = null;
            string periodo = null;
            string curso = null;
            string serie = null;
            hdnCompartilhada.Value = string.Empty;

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("aluno=") >= 0)
                    aluno = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("comp=") >= 0)
                    compartilhada = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("ano=") >= 0)
                    ano = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("periodo=") >= 0)
                    periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("curso=") >= 0)
                    curso = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("serie=") >= 0)
                    serie = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("mensagem=") >= 0)
                    mensagem = dados.Substring(dados.LastIndexOf('=') + 1);
            }

            lblMensagem.Text = mensagem;

            if (!compartilhada.IsNullOrEmptyOrWhiteSpace())
            {
                hdnCompartilhada.Value = compartilhada;
                hdnAno.Value = ano;
                hdnPeriodo.Value = periodo;
                hdnCurso.Value = curso;
                hdnSerie.Value = serie;                
            }

            return aluno;
        }

        private QueryTable CarregarDadosGrid(string idGrid, string aluno)
        {
            QueryTable dadosGrid = null;

            dadosGrid = RN.EncerramentoAluno.ConsultarEncerramentos(aluno);

            int count = dadosGrid.Rows.Count;

            if (count <= 0)
            {
                btnEncerrar.Visible = true;
            }
            else
            {
                if (dadosGrid.Rows[0]["dt_reabertura"] != null && dadosGrid.Rows[0]["dt_reabertura"].ToString() != String.Empty)
                {
                    btnEncerrar.Visible = true;
                }
                else
                {
                    btnEncerrar.Visible = false;
                }
            }

            grdEncerramentos.DataSource = dadosGrid;
            grdEncerramentos.DataBind();

            return dadosGrid;
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        pnDados.Visible = false;
                        grdEncerramentos.Visible = false;
                        tseAluno.ResetValue();

                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        lblMensagem.Text = string.Empty;

                        tseAluno.Enabled = true;

                        Ly_aluno dtAluno = new Ly_aluno();
                        Ly_aluno.Row dadosAluno = dtAluno.NewRow();
                        dadosAluno.Aluno = tseAluno.DBValue.ToString();

                        dadosAluno = RN.Aluno.ConsultarAluno(dadosAluno.Aluno);

                        if (dadosAluno != null)
                        {
                            PreencherDadosTela(dadosAluno);
                            CarregarDadosGrid("grdEncerramentos", dadosAluno.Aluno);
                            pnDados.Visible = true;
                            grdEncerramentos.Visible = true;
                        }
                        else
                        {
                            LimparTela();
                            pnDados.Visible = false;
                            grdEncerramentos.Visible = false;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        }
                        break;
                    }

                case TipoOperacao.Retorno:
                    {
                        string aluno = string.Empty;

                        if (!String.IsNullOrEmpty(Request.QueryString["Aluno"]))
                            aluno = Request.QueryString["Aluno"];
                        else
                        {
                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                            aluno = ObterDadosQueryString(decodedText);
                        }
                        tseAluno.DBValue = aluno;
                        tseAluno.DataBind();
                        tseAluno.Enabled = true;

                        Ly_aluno dtAluno = new Ly_aluno();
                        Ly_aluno.Row dadosAluno = dtAluno.NewRow();
                        dadosAluno.Aluno = aluno;

                        dadosAluno = RN.Aluno.ConsultarAluno(dadosAluno.Aluno);

                        if (dadosAluno != null)
                        {
                            PreencherDadosTela(dadosAluno);
                            CarregarDadosGrid("grdEncerramentos", dadosAluno.Aluno);
                            pnDados.Visible = true;
                            grdEncerramentos.Visible = true;
                        }
                        else
                        {
                            LimparTela();
                            pnDados.Visible = false;
                            grdEncerramentos.Visible = false;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Limpa todas as textbox e combobox.
        /// </summary>
        protected void LimparTela()
        {
            txtCurriculo.Text = string.Empty;
            txtCurso.Text = string.Empty;
            txtSerie.Text = string.Empty;
            txtSituacao.Text = string.Empty;
            txtTurno.Text = string.Empty;
            txtUniEnsino.Text = string.Empty;

        }

        /// <summary>
        /// Preenche os dados na tela de acordo com a linha passada como parâmetro
        /// </summary>
        /// <param name="dadosDocente">Linha com os dados do docente</param>
        private void PreencherDadosTela(Ly_aluno.Row dadosAluno)
        {
            txtCurriculo.Text = Convert.ToString(dadosAluno.Curriculo);
            txtCurso.Text = Convert.ToString(dadosAluno.Curso);
            txtNomeCurso.Text = Convert.ToString(dadosAluno.Nome_curso);
            txtSerie.Text = Convert.ToString(dadosAluno.Serie);
            txtNomeSerie.Text = Convert.ToString(dadosAluno.Descricao_serie);
            txtSituacao.Text = Convert.ToString(dadosAluno.Sit_aluno);
            txtTurno.Text = Convert.ToString(dadosAluno.Turno);
            txtNomeTurno.Text = Convert.ToString(dadosAluno.Descricao_turno);
            txtUniEnsino.Text = Convert.ToString(dadosAluno.Nome_comp04);

        }

        protected void btnEncerrar_Click(object sender, EventArgs e)
        {
            try
            {
                string tipoOperacao = string.Empty;
                RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
                RN.Matriculas.OpcaoInscricao rnOpcaoInscricao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
                RN.SituacaoFinalAluno rnSituacaoFinalAluno = new SituacaoFinalAluno();
                RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
                RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
                Ly_aluno dtAluno = new Ly_aluno();
                Ly_aluno.Row dadosAluno = dtAluno.NewRow();

                if (rnConfirmacaoMatricula.ExisteConfirmacaoPendenteConfirmadoSemPermissaoAcesso(tseAluno.DBValue.ToString(), User.Identity.Name))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('O aluno possui confirmação pendente ou confirmada em outra unidade escolar fora da sua restrição de acesso.');", true);
                }
                else if (rnOpcaoInscricao.PossuiConvocacaoPendentePor(tseAluno.DBValue.ToString()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('O aluno não pode ser encerrado pois possui convocação pendente de resposta no Matrícula Fácil.');", true);
                }
                //Chamado 47993 - Solicitação de retirada da regra
                //Verifica se o aluno não foi enturmado e reprovado em 2023 0 e 1 na 2ª e 3ª série, EJA III e EJA IV
                //else if (!rnMatricula.PossuiMatriculaAtivaPeriodoPor(tseAluno.DBValue.ToString(), 2024) //Não enturmado em 2024
                //        && rnRenovacao.PossuiRenovacaoAtivaConfirmadaPor(tseAluno.DBValue.ToString(), 2024, 0) //Verifica se possui renovação para 2024 ou seja de serie concluinte
                //        && (rnSituacaoFinalAluno.EhReprovadoPor(tseAluno.DBValue.ToString(), 2023, 0, "0002.31", 3) //verifica se é reprovado na 3ª série do ensino medio                                
                //                || rnSituacaoFinalAluno.EhReprovadoItinerarioFormativoTrihaPor(tseAluno.DBValue.ToString(), 2023, 0, 2, "RE1", 3) //Verifica se é reprovado em cursos do itinerario regular serie 2
                //                || rnSituacaoFinalAluno.EhReprovadoItinerarioFormativoTrihaPor(tseAluno.DBValue.ToString(), 2023, 0, 3, "ED2", 3) //Verifica se é reprovado em cursos do itinerario eja serie 3
                //                || rnSituacaoFinalAluno.EhReprovadoItinerarioFormativoTrihaPor(tseAluno.DBValue.ToString(), 2023, 0, 4, "ED2", 3)) //Verifica se é reprovado em cursos do itinerario eja serie 4
                //            )
                //    {
                //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Aluno(s) retido(s) em 2023 na 2ª / 3ª Série do Regular ou EJA III / IV ficarão em pendência de enturmação para futura escolha do curso/série de 2024, realize a enturmação antes do encerramento.');", true);

                //    }
                else
                {
                    tipoOperacao = "ENCERRARNOVO";
                    dadosAluno.Aluno = tseAluno.DBValue.ToString();
                    dadosAluno.Ano_ingresso = RN.Aluno.ConsultarAluno(tseAluno.DBValue.ToString()).Ano_ingresso;
                    dadosAluno.Sem_ingresso = RN.Aluno.ConsultarAluno(tseAluno.DBValue.ToString()).Sem_ingresso;
                    dadosAluno.Pessoa = Convert.ToDecimal(tseAluno["pessoa"].ToString());

                    string queryString = MontarQueryStringAluno(tipoOperacao, dadosAluno);

                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                    Response.Redirect("EncerramentoAluno.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdEncerramentos_SelectionChanged(object sender, EventArgs e)
        {
            string tipoOperacao = string.Empty;
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            Ly_h_cursos_concl dtEncerramento = new Ly_h_cursos_concl();
            Ly_h_cursos_concl.Row dadosEncerramento = dtEncerramento.NewRow();

            int curPageSelection = GetSelectedRowOnTheCurrentPage();

            dadosEncerramento.Aluno = tseAluno.DBValue.ToString();
            if (!string.IsNullOrEmpty(Convert.ToString(grdEncerramentos.GetRowValues(curPageSelection, "dt_encerramento"))))
            {
                dadosEncerramento.Dt_encerramento = Convert.ToDateTime(grdEncerramentos.GetRowValues(curPageSelection, "dt_encerramento"));
            }
            if (!string.IsNullOrEmpty(Convert.ToString(grdEncerramentos.GetRowValues(curPageSelection, "dt_reabertura"))))
            {
                dadosEncerramento.Dt_reabertura = Convert.ToDateTime(grdEncerramentos.GetRowValues(curPageSelection, "dt_reabertura"));
            }
            dadosEncerramento.Motivo = Convert.ToString(grdEncerramentos.GetRowValues(curPageSelection, "motivo"));
            dadosEncerramento.Outra_faculdade = Convert.ToString(grdEncerramentos.GetRowValues(curPageSelection, "instituicao"));
            if (!string.IsNullOrEmpty(Convert.ToString(grdEncerramentos.GetRowValues(curPageSelection, "dt_colacao"))))
            {
                dadosEncerramento.Dt_colacao = Convert.ToDateTime(grdEncerramentos.GetRowValues(curPageSelection, "dt_colacao"));
            }

            tipoOperacao = "ENCERRAR";


            if (!string.IsNullOrEmpty(Convert.ToString(grdEncerramentos.GetRowValues(0, "dt_reabertura"))) || curPageSelection != 0)
            {
                tipoOperacao = "VISUALIZAR";
            }

            if (!string.IsNullOrEmpty(Convert.ToString(grdEncerramentos.GetRowValues(curPageSelection, "dt_diploma"))))
            {
                dadosEncerramento.Dt_diploma = Convert.ToDateTime(grdEncerramentos.GetRowValues(curPageSelection, "dt_diploma"));
            }
            if (!string.IsNullOrEmpty(Convert.ToString(grdEncerramentos.GetRowValues(curPageSelection, "ano_encerramento"))))
            {
                dadosEncerramento.Ano_encerramento = Convert.ToInt16(grdEncerramentos.GetRowValues(curPageSelection, "ano_encerramento"));
            }
            if (!string.IsNullOrEmpty(Convert.ToString(grdEncerramentos.GetRowValues(curPageSelection, "periodo_encerramento"))))
            {
                dadosEncerramento.Sem_encerramento = Convert.ToInt16(grdEncerramentos.GetRowValues(curPageSelection, "periodo_encerramento"));
            }
            dadosEncerramento.Causa_encerr = Convert.ToString(grdEncerramentos.GetRowValues(curPageSelection, "causa"));

            dadosEncerramento.Motivoreabertura = Convert.ToString(grdEncerramentos.GetRowValues(curPageSelection, "motivoreabertura"));

            string queryString = MontarQueryString(tipoOperacao, dadosEncerramento);

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);


            ASPxWebControl.RedirectOnCallback("EncerramentoAluno.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

        }
    }
}
