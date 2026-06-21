using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Lyceum.Net.Modulos;
using Techne.Lyceum.RN.Util;


namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/CadHistoricoMatricula.aspx"),
     ControlText("HistoricoMatriculas"),
     Title("Histórico de Matrículas"),]

    public partial class CadHistoricoMatricula : TPage
    {
        private string situacaoHistOriginal;
        #region Evento da Pagina
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            #region Relatorio
            //if (!string.IsNullOrEmpty(tseAluno.DBValue.ToString()) && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
            //{
            //    var pares = new Dictionary<string, string>
            //                    {
            //                        {"aluno", tseAluno.DBValue.ToString()},
            //                      };
            //this.btnImprimirHistorico.Attributes.Add("onclick",
            //                                         @"javascript:window.open('../Relatorio/Relatorios.aspx?report=Filipeta&grp=dol&" +
            //                                         CodificaQueryString(pares) +
            //                                         "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
            //}
            #endregion
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdHistorico);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdHistorico, "Histórico de Matrículas");

            LyceumMaster mp = (LyceumMaster)Master;
            mp.habilitaLoading = true;
        }
        #endregion

        #region Logica

        public object ListarHist(object aluno, object ano, object periodo, object turma)
        {
            if (aluno != null && (ano != null && ano.ToString() != "Selecione") && (periodo != null && periodo.ToString() != "Selecione"))
            {
                decimal numero;

                //Verifica se aluno possui 15 digitos numericos
                if ((aluno.ToString().Trim().Length != 15) || !Decimal.TryParse(aluno.ToString().Trim(), out numero))
                {
                    return null;
                }

                return HistMatricula.Listar(aluno.ToString().Trim(), int.Parse(ano.ToString()), int.Parse(periodo.ToString()), turma.ToString());
            }

            return null;
        }

        private void CarregarAno()
        {
            QueryTable qt = HistMatricula.RetornaAnosDeHistoricoPor(tseAluno.DBValue.ToString());
            ddlAno.DataSource = qt;
            ddlAno.DataBind();
            ListItem ls = new ListItem("Selecione", string.Empty);
            ddlAno.Items.Insert(0, ls);          
        }

        private void CarregaPeriodo()
        {
            if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
            {
                ddlPeriodo.DataSource = HistMatricula.RetornaPeriodosDeHistoricoPor(ddlAno.SelectedValue, tseAluno.DBValue.ToString());
                ddlPeriodo.DataBind();
                ListItem ls = new ListItem("Selecione", string.Empty);
                ddlPeriodo.Items.Insert(0, ls);
            }
        }

        protected void AtualizarStatusAluno()
        {
            if (!tseAluno.DBValue.IsNull && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlTurma.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                string situacaoFinal = string.Empty;
                decimal percentual = RN.HistMatricula.VerificaFrequenciaGlobal(tseAluno.DBValue.ToString(),
                                                                               int.Parse(ddlAno.SelectedValue),
                                                                               int.Parse(ddlPeriodo.SelectedValue),
                                                                               ddlTurma.SelectedValue);

                situacaoFinal = RN.HistMatricula.VerificaSituacaoFinal(tseAluno.DBValue.ToString(),
                                                                                   int.Parse(ddlAno.SelectedValue),
                                                                                   int.Parse(ddlPeriodo.SelectedValue),
                                                                            ddlTurma.SelectedValue);

                lblFrequenciaGlobal.Text = percentual.ToString("0.00") + " %";

                if (!string.IsNullOrEmpty(situacaoFinal) && situacaoFinal != "Sem Disciplina")
                {
                    lblSituacaoFinal.Text = situacaoFinal;
                }
                else
                {
                    lblSituacaoFinal.Text = string.Empty;
                }
            }
            else
            {
                throw new Exception("Os campos Aluno/Ano/Periodo/Turma são de preenchimento obrigatório.");
            }
        }

        protected void CarregaSituacaoFinalAluno()
        {
            TceSituacaoFinalAluno situacaoFinalAluno = new TceSituacaoFinalAluno();
            SituacaoFinalAluno rnSituacaoFinalAluno = new SituacaoFinalAluno();
            decimal frequenciaGlobal = 0;
            string situacaoFinal = string.Empty;

            situacaoFinalAluno = rnSituacaoFinalAluno.ObtemPor(tseAluno.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), ddlTurma.SelectedValue);
            if (situacaoFinalAluno.IdSituacaoFinalAluno > 0)
            {
                frequenciaGlobal = situacaoFinalAluno.FrequenciaGlobal;
                situacaoFinal = situacaoFinalAluno.SituacaoFinal;
            }

            if (ddlAno.SelectedValue == "2020" || ddlAno.SelectedValue == "2021" )
            {
                if (ddlSituacao.Items.FindByValue(situacaoFinal) != null)
                {
                    ddlSituacao.SelectedValue = situacaoFinal;
                }
            }
            else
            {
                lblSituacaoFinal.Text = situacaoFinal;
                lblFrequenciaGlobal.Text = frequenciaGlobal.ToString("0.00") + " %";
            }           
        }

        private bool PossuiNota(DevExpress.Web.ASPxGridView.ASPxGridView grid, ASPxGridViewEditorEventArgs e)
        {
            string codigoDisciplina = grid.GetRowValues(e.VisibleIndex, "disciplina").ToString();
            System.Data.DataTable dt = Lyceum.RN.Disciplina.ConsultarPorDisciplina(codigoDisciplina);
            return dt.Rows[0]["tem_nota"].ToString().Equals("S");
        }

        private bool PossuiFrequencia(DevExpress.Web.ASPxGridView.ASPxGridView grid, ASPxGridViewEditorEventArgs e)
        {
            string codigoDisciplina = grid.GetRowValues(e.VisibleIndex, "disciplina").ToString();
            System.Data.DataTable dt = Lyceum.RN.Disciplina.ConsultarPorDisciplina(codigoDisciplina);
            return dt.Rows[0]["tem_freq"].ToString().Equals("S");
        }

        #endregion

        #region Eventos de Controles

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                ddlAno.Enabled = false;
                ddlPeriodo.Enabled = false;
                ddlTurma.Enabled = false;
                ddlAno.ClearSelection();
                ddlPeriodo.Items.Clear();
                ddlTurma.Items.Clear();
                grdHistorico.Visible = false;
                lblFrequenciaGlobal.Visible = false;
                lblSituacaoFinal.Visible = false;
                lblTextoFreq.Visible = false;
                lblTextoSitFinal.Visible = false;
                hdnSerieConcluinte.Value = string.Empty;
                lblInformativo.Visible = false;
                ddlSituacao.Visible = false;
                btnSalvarSituacao.Visible = false;

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                        ddlAno.Enabled = true;
                        CarregarAno();
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor preencher o campo Aluno.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                ddlPeriodo.Enabled = false;
                ddlTurma.Enabled = false;
                ddlPeriodo.Items.Clear();
                ddlTurma.Items.Clear();
                lblFrequenciaGlobal.Visible = false;
                lblSituacaoFinal.Visible = false;
                lblTextoFreq.Visible = false;
                lblTextoSitFinal.Visible = false;
                grdHistorico.Visible = false;
                lblInformativo.Visible = false;
                ddlSituacao.Visible = false;
                btnSalvarSituacao.Visible = false;
                lblSituacaoFinal.Visible = false;
                hdnSerieConcluinte.Value = string.Empty;

                if (!string.IsNullOrEmpty(ddlAno.SelectedValue))
                {
                    CarregaPeriodo();
                    ddlPeriodo.Enabled = true;

                }
                else
                {
                    ddlPeriodo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlTurma.Enabled = false;
                ddlTurma.Items.Clear();
                lblFrequenciaGlobal.Visible = false;
                lblSituacaoFinal.Visible = false;
                lblTextoFreq.Visible = false;
                lblTextoSitFinal.Visible = false;
                grdHistorico.Visible = false;
                hdnSerieConcluinte.Value = string.Empty;

                if (tseAluno.DBValue.IsNull)
                {
                    lblMensagem.Text = "O campo Aluno é de preenchimento obrigatório.";
                    return;
                }

                if (!string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
                {
                    ddlTurma.DataSource = RN.HistMatricula.ListarTurmasHistoricoPor(tseAluno.DBValue.ToString(),
                                                                                    int.Parse(ddlAno.SelectedValue),
                                                                                    int.Parse(ddlPeriodo.SelectedValue));
                    ddlTurma.DataBind();
                    ListItem ls = new ListItem("Selecione", string.Empty);
                    ddlTurma.Items.Insert(0, ls);
                    ddlTurma.Enabled = true;

                    if (ddlAno.SelectedValue == "2020" || ddlAno.SelectedValue == "2021" )
                    {
                        if (ddlAno.SelectedValue == "2020")
                        {
                            lblInformativo.Text = "Amparado pela Resolução SEEDUC/SUGEN nº 5.879/2020 publicada no D.O. de 14/10/2020";
                        }
                        else if ( ddlAno.SelectedValue == "2021" && ddlPeriodo.SelectedValue == "1")
                        {
                            lblInformativo.Text = "Amparado pela Resolução SEEDUC/SUGEN nº 5.595/2021 publicada no D.O. de 05/07/2021";
                        }
                        else if (ddlAno.SelectedValue == "2021" && (ddlPeriodo.SelectedValue == "2" || ddlPeriodo.SelectedValue == "0"))
                        {
                            lblInformativo.Text = "Amparado pela Resolução SEEDUC/SUGEN nº 6015/2021 publicada no D.O. de 14/12/2021";
                        }

                        lblInformativo.Visible = true;

                    }
                }
                else
                {
                    ddlTurma.Enabled = false;
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
                RN.GradeSerie rnGradeSerie = new GradeSerie();
                hdnSerieConcluinte.Value = "N";
                ddlSituacao.SelectedValue = string.Empty;

                if (!tseAluno.DBValue.IsNull && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue) && !string.IsNullOrEmpty(ddlTurma.SelectedValue))
                {
                    //Busca a situação final do aluno na turma
                    CarregaSituacaoFinalAluno();

                    grdHistorico.Visible = true;
                    this.odsCadHistMatricula.Select();
                    this.odsCadHistMatricula.DataBind();
                    this.grdHistorico.DataBind();

                    if (ddlAno.SelectedValue == "2020" || ddlAno.SelectedValue == "2021" )
                    {
                        lblTextoFreq.Visible = false;
                        lblTextoSitFinal.Visible = true;
                        lblFrequenciaGlobal.Visible = false;
                        lblSituacaoFinal.Visible = false;
                        ddlSituacao.Visible = true;

                        btnSalvarSituacao.Visible = true;
                        if (rnGradeSerie.EhSerieConcluinte(Convert.ToDecimal(ddlAno.SelectedValue), Convert.ToDecimal(ddlPeriodo.SelectedValue), ddlTurma.SelectedValue))
                        {
                            hdnSerieConcluinte.Value = "S";
                        }
                    }
                    else
                    {
                        lblTextoFreq.Visible = true;
                        lblTextoSitFinal.Visible = true;
                        lblFrequenciaGlobal.Visible = true;
                        lblSituacaoFinal.Visible = true;
                        ddlSituacao.Visible = false;
                    }
                                
                }
                else
                {
                    lblSituacaoFinal.Text = string.Empty;
                    lblFrequenciaGlobal.Text = string.Empty;
                    grdHistorico.Visible = false;
                    lblTextoFreq.Visible = false;
                    lblTextoSitFinal.Visible = false;
                    lblFrequenciaGlobal.Visible = false;
                    lblSituacaoFinal.Visible = false;
                    ddlSituacao.Visible = false;
                    btnSalvarSituacao.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdHistorico_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdHistorico.Settings.ShowFilterRow = false;
        }

        protected void grdHistorico_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdHistorico.Settings.ShowFilterRow = false;
        }

        protected void grdHistorico_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "aluno")
                e.Editor.ReadOnly = !grdHistorico.IsNewRowEditing;
            if (e.Column.FieldName == "ordem")
                e.Editor.ReadOnly = !grdHistorico.IsNewRowEditing;
            if (e.Column.FieldName == "disciplina")
                e.Editor.ReadOnly = !grdHistorico.IsNewRowEditing;

            if (ddlAno.SelectedValue == "2021")
            {
                if (e.Column.FieldName == "situacao_hist")
                {
                    e.Editor.Enabled = false;
                }
            }

            if (e.Column.FieldName == "nota_final")
            {
                if (PossuiNota((DevExpress.Web.ASPxGridView.ASPxGridView)sender, e))
                {
                    e.Editor.ToolTip = String.Empty;
                    e.Editor.ClientEnabled = true;
                }
                else
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ToolTip = "Não é possível alterar este campo, pois a disciplina não possui critério de avaliação por nota.";
                }
            }

            if (e.Column.FieldName == "falta_final")
            {
                if (PossuiFrequencia((DevExpress.Web.ASPxGridView.ASPxGridView)sender, e))
                {
                    e.Editor.ToolTip = String.Empty;
                    e.Editor.ClientEnabled = true;
                }
                else
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ToolTip = "Não é possível alterar este campo, pois a disciplina não possui critério de avaliação por frequência.";
                }
            }

            if (e.Column.FieldName == "aulas_dadas")
            {
                if (PossuiFrequencia((DevExpress.Web.ASPxGridView.ASPxGridView)sender, e))
                {
                    e.Editor.ToolTip = String.Empty;
                    e.Editor.ClientEnabled = true;
                }
                else
                {
                    e.Editor.ClientEnabled = false;
                    e.Editor.ToolTip = "Não é possível alterar este campo, pois a disciplina não possui critério de avaliação por frequência.";
                }
            }
        }

        protected void grdHistorico_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string disciplina = e.NewValues["disciplina"].ToString();
            e.NewValues["creditos"] = RN.Historicos.ConsultaCreditos(disciplina);

            e.NewValues["nivel_presenca"] = "Presencial";

            if (ddlAno.SelectedValue == "2021")
            {
                situacaoHistOriginal = Convert.ToString(grdHistorico.GetRowValues(grdHistorico.EditingRowVisibleIndex, "situacao_hist"));
            }
            else
            {
                situacaoHistOriginal = e.OldValues["situacao_hist"].ToString();
            }
        }

        public void Update(object nome_comp03, object outras, object disciplina, object nomedisciplina, object turma, object serie, object nota_final, object situacao_hist, object DEPENDENCIA, object falta_final, object aulas_dadas, object observacao, object creditos, object nivel_presenca, object aluno, object ordem, object ano, object semestre)
        { }

        public void Update(object nome_comp03, object outras, object disciplina, object nomedisciplina, object turma, object serie, object nota_final, object situacao_hist, object observacao, object falta_final, object aulas_dadas, object DEPENDENCIA, object SERIE_REFERENCIA, object DISCIPLINA_REFERENCIA, object creditos, object nivel_presenca, object aluno, object ordem, object ano, object semestre)
        { }

        public void Update(object nome_comp03, object outras, object disciplina, object nomedisciplina, object turma, object serie, object nota_final, object observacao, object falta_final, object aulas_dadas, object DEPENDENCIA, object SERIE_REFERENCIA, object DISCIPLINA_REFERENCIA, object creditos, object nivel_presenca, object aluno, object ordem, object ano, object semestre)
        { }

        protected void odsHistMatricula_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var dt = PeriodoLetivo.ConsultarDatas(ddlAno.SelectedValue, ddlPeriodo.SelectedValue);
            DateTime? dtInicio = null;
            DateTime? dtFim = null;
            bool situacaoNaoAlterada;

            if (dt.Rows.Count > 0)
            {
                dtInicio = Convert.ToDateTime(dt.Rows[0]["DT_INICIO"]);
                dtFim = Convert.ToDateTime(dt.Rows[0]["DT_FIM"]);
            }

            LyHistMatricula lyHistMatricula = new LyHistMatricula
            {
                Aluno = tseAluno.DBValue.ToString(),
                Ano = Convert.ToDecimal(ddlAno.SelectedValue),
                Semestre = Convert.ToDecimal(ddlPeriodo.SelectedValue),
                Ordem = Convert.ToDecimal(e.InputParameters["ordem"]),
                Disciplina = e.InputParameters["disciplina"].ToString(),
                NotaFinal = e.InputParameters["nota_final"].ToString().Replace('.', ','),
                Turma = e.InputParameters["turma"].ToString(),               
                FaltaFinal = (e.InputParameters["falta_final"] != null) ? Convert.ToInt32(e.InputParameters["falta_final"]) : (int?)null,
                AulasDadas = (e.InputParameters["aulas_dadas"] != null) ? Convert.ToDecimal(e.InputParameters["aulas_dadas"]) : (decimal?)null,
                Observacao = (e.InputParameters["observacao"] != null) ? Convert.ToString(e.InputParameters["observacao"]) : (string)null,
                UnidadeEnsino = (e.InputParameters["nome_comp03"] != null) ? "valido" : null,
                Creditos = (e.InputParameters["creditos"] != null) ? Convert.ToDecimal(e.InputParameters["creditos"]) : (decimal?)null,
                NivelPresenca = e.InputParameters["nivel_presenca"].ToString(),
                Dependencia = (e.InputParameters["DEPENDENCIA"] != null && Convert.ToString(e.InputParameters["DEPENDENCIA"]).Trim() != string.Empty) ? "S" : "N",
                DtUltalt = DateTime.Now,
                DtInicio = dtInicio,
                DtFim = dtFim,
                Matricula = User.Identity.Name
            };

            if (ddlAno.SelectedValue == "2021" )
            {
                lyHistMatricula.SituacaoHist = Convert.ToString(grdHistorico.GetRowValues(grdHistorico.EditingRowVisibleIndex, "situacao_hist"));
            }
            else
            {
                lyHistMatricula.SituacaoHist = e.InputParameters["situacao_hist"].ToString();
            }

            situacaoNaoAlterada = situacaoHistOriginal.Equals(lyHistMatricula.SituacaoHist);

            var validacao = HistMatricula.ValidarAlteracao(lyHistMatricula, situacaoNaoAlterada);

            if (validacao.Valido)
            {
                HistMatricula.AtualizarSituacaoFinalEFrequenciaGlobal(lyHistMatricula);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdHistorico_OnRowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            CarregaSituacaoFinalAluno();

            if (!string.IsNullOrEmpty(lblSituacaoFinal.Text) && lblSituacaoFinal.Text != "Sem Disciplina")
            {
                ((ASPxGridView)sender).JSProperties["cpAtualizar"] = lblSituacaoFinal.Text;
            }
            else
            {
                ((ASPxGridView)sender).JSProperties["cpAtualizar"] = string.Empty;
            }

            if (!string.IsNullOrEmpty(lblFrequenciaGlobal.Text))
            {
                ((ASPxGridView)sender).JSProperties["cpAtualizarFreq"] = lblFrequenciaGlobal.Text;
            }
            else
            {
                ((ASPxGridView)sender).JSProperties["cpAtualizarFreq"] = string.Empty;
            }
        }

        protected void grdHistorico_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["disciplina"] == null)
            {
                e.RowError = "Favor informar a Disciplina.";
            }
        }

        protected void grdHistorico_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdHistorico);
        }
        protected void grdHistorico_CustomErrorText(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomErrorTextEventArgs e)
        {
            Exception ex = e.Exception;
            //process the exception
            e.ErrorText = ex.Message;
        }

        #endregion

        protected void btnSalvarSituacao_Click(object sender, EventArgs e)
        {
            RN.HistMatricula rnHistMatricula = new HistMatricula();
            try
            {
                if (!tseAluno.DBValue.IsNull && !string.IsNullOrEmpty(ddlAno.SelectedValue) && !string.IsNullOrEmpty(ddlPeriodo.SelectedValue) && !string.IsNullOrEmpty(ddlTurma.SelectedValue) && !string.IsNullOrEmpty(ddlSituacao.SelectedValue))
                {
                    if (ddlAno.SelectedValue == "2020" || ddlAno.SelectedValue == "2021" )
                    {                       
                        //CASO A SERIE NÃO SEJA CONCLUINTE, A OPÇAO APROVADO NÃO PODERÁ SER ESCOLHIDA
                        if (hdnSerieConcluinte.Value == "N" && ddlSituacao.SelectedValue == "Aprovado")
                        {
                            lblMensagem.Text = "EM " + ddlAno.SelectedValue + ", EXCEPCIONALMENTE, NÃO PODERÁ SER INFORMADA A SITUAÇÃO";
                            return;
                        }

                        //CASO A SERIE SEJA CONCLUINTE, A OPÇAO PROMOVIDO NÃO PODERÁ SER ESCOLHIDA
                        if (hdnSerieConcluinte.Value == "S" && ddlSituacao.SelectedValue == "Promovido")
                        {
                            lblMensagem.Text = "Esta situação é apenas para alunos das séries/fases/anos/módulos qua não sejam terminalidades.";
                            return;
                        }                     

                    }


                    rnHistMatricula.AtualizaSituacaoFinal(tseAluno.DBValue.ToString(), Convert.ToDecimal(ddlAno.SelectedValue), Convert.ToDecimal(ddlPeriodo.SelectedValue), ddlTurma.SelectedValue, ddlSituacao.SelectedValue, User.Identity.Name);

                    this.odsCadHistMatricula.Select();
                    this.odsCadHistMatricula.DataBind();
                    this.grdHistorico.DataBind();
                }
                else
                {
                    lblMensagem.Text = "Para atualizar a situação final é necessário preencher os campos ALUNO/ANO/PERÍODO/TURMA/SITUAÇÃO.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdHistorico_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {

            if (e.VisibleIndex == -1) return;

            var dependencia = grdHistorico.GetRowValues(e.VisibleIndex, "DEPENDENCIA")  != DBNull.Value ? (string)grdHistorico.GetRowValues(e.VisibleIndex, "DEPENDENCIA") : string.Empty;

            if (ddlAno.SelectedValue == "2020")
            {
                if ((dependencia == "Dependência" || dependencia == "Dependência/Optativa/Reforço"))
                {
                    e.Visible = true;
                }
                else
                {
                    e.Visible = false;
                }
            }

        }

    }
}

