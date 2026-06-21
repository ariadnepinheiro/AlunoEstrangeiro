using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Lyceum.Net.Modulos;
using System.Linq;
using Techne.Controls;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;
using System.Web;
using System.Web.UI;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.Net.RecursosHumanos
{
    [NavUrl("~/RecursosHumanos/MigracaoCHAlocacao.aspx"),
    ControlText("Migração de CH e Alocação"),
    Title("Migração de CH e Alocação"),]


    public partial class MigracaoCHAlocacao : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                lblMensagemMigracao.Text = string.Empty;

                if (!IsPostBack)
                {
                    Session["grid"] = null;
                    pnlDados.Visible = !tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue;
                    InicializaGrid();
                }
                else
                {
                    grdSelecionado.DataSource = Session["grid"];
                    grdSelecionado.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string anoAtual = DateTime.Today.Year.ToString();
                    if (ddlAno.Items.FindByText(anoAtual) != null)
                        ddlAno.Items.FindByText(anoAtual).Selected = true;
                }
                tseFuncaoLotacao.Mode = ControlMode.View;

                var pares = new Dictionary<string, string>
                        {
                          
                            { "idvinculo", (!tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue) ? tseDocente.DBValue.ToString() : string.Empty },
                            { "usuario", this.User.Identity.Name }, 
                        };



                this.btnRelDesalocaProfessor.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=DesalocacaoMigracaoPorProfessor&grp=qhi&" + CodificaQueryString(pares) + "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");


                if (grdSelecionado.VisibleRowCount < 8)
                {
                    dvMigrar.Visible = false;
                }
                else
                {
                    dvMigrar.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAulas, "Alocações do Docente");
            TituloGrid(grdTurmaCarenciaContratoGLP, string.Empty);
            TituloGrid(grdSelecionado, "Turmas/disciplinas selecionadas para alocação");
        }

        protected void tseDocente_Changed(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDocente = new DataTable();
                RN.Docentes rnDocente = new Docentes();
                RN.LicencaDocente rnLicencaDocente = new LicencaDocente();
                LimparTela();
                lblMensagem.Text = string.Empty;
                pnlDados.Visible = false;
                btnIncluir.Visible = false;
                dvMigrar.Visible = false;
                btnAlterarCargo.Visible = false;
                
                pnlAulas.Visible = false;
                pnPrincipal.Visible = false;
                //pnlRelatorio.Visible = false;

                pnlGridTurmas.Visible = false;
                grdTurmaCarenciaContratoGLP.Selection.UnselectAll();
                grdTurmaCarenciaContratoGLP.DataBind();
                Session["grid"] = null;
                InicializaGrid();


                if (!tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue)
                {
                    String matricula = Convert.ToString(tseDocente["num_func"]);
                    hdnNumFunc.Value = Convert.ToString(tseDocente["num_func"]);

                    if (!hdnNumFunc.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        //REL_CH_SERV_ANO_RDL
                        dtDocente = rnDocente.ObtemDadosDocenteMigracaoPor(Convert.ToInt32(ddlAno.SelectedValue), tseDocente.DBValue.ToString());

                        if (dtDocente.Rows.Count == 0)
                        {
                            lblMensagem.Text = "Informações do docente não encontrado ou não ativo.";

                        }
                        else
                        {
                            pnlFuncao.Visible = true;

                            txtNome.Text = Convert.ToString(dtDocente.Rows[0]["nome_compl"]);
                            txtMatricula.Text = Convert.ToString(dtDocente.Rows[0]["matricula"]);
                            txtIDVinculo.Text = Convert.ToString(dtDocente.Rows[0]["idvinculo"]);

                            Int64 cpf;
                            Int64.TryParse(Convert.ToString(dtDocente.Rows[0]["cpf"]), out cpf);
                            txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", cpf);

                            txtCargo.Text = Convert.ToString(dtDocente.Rows[0]["cargo"]);
                            txtFuncao.Text = Convert.ToString(dtDocente.Rows[0]["funcao"]);
                            txtSituacao.Text = Convert.ToString(dtDocente.Rows[0]["situacao"]);
                            txtReadaptado.Text = Convert.ToString(dtDocente.Rows[0]["readaptado"]);

                            txtSegundaMatricula.Text = Convert.ToString(dtDocente.Rows[0]["segunda_matricula"]);
                            txtDisciplinaIngresso.Text = Convert.ToString(dtDocente.Rows[0]["dis_ingress"]);

                            txtCHIngresso.Text = Convert.ToString(dtDocente.Rows[0]["ch_regencia"]);
                            txtCHTurma.Text = Convert.ToString(dtDocente.Rows[0]["hor_tur"]);
                            txtCHNormal.Text = Convert.ToString(dtDocente.Rows[0]["tol_normal"]);
                            txtCHGLP.Text = Convert.ToString(dtDocente.Rows[0]["tol_glp"]);

                            txtRegional.Text = Convert.ToString(dtDocente.Rows[0]["regional"]);
                            txtMunicipio.Text = Convert.ToString(dtDocente.Rows[0]["municipio"]);
                            txtUALotacao.Text = Convert.ToString(dtDocente.Rows[0]["unidade administrativa"]);
                            txtUANome.Text = Convert.ToString(dtDocente.Rows[0]["ua de lotacao"]);

                            

                            hdnCenso.Value = Convert.ToString(dtDocente.Rows[0]["UNIDADE_ENS"]);

                            pnlDados.Visible = !tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue;

                            if (Convert.ToString(dtDocente.Rows[0]["REGENTE"]) == "S" || Convert.ToString(dtDocente.Rows[0]["FUNC_PERMITE_GLP"]) == "S")
                            {

                                int chRegencia = Convert.ToInt32(dtDocente.Rows[0]["ch_regencia"]);
                                int chAula = Convert.ToInt32(dtDocente.Rows[0]["tol_normal"]);
                                bool reduzida = rnLicencaDocente.PossuiCargaHorariaReduzida(Convert.ToInt32(tseDocente["num_Func"]));


                                if ((reduzida && (chAula < (chRegencia / 2))) || (!reduzida && chAula < chRegencia))
                                {
                                    lblMensagem.Text = "Para efetuar a migração o professor não pode estar em CH Livre total ou parcial. Favor verificar";
                                    pnlFuncao.Visible = false;
                                    pnlAulas.Visible = false;
                                    btnIncluir.Visible = false;
                                    dvMigrar.Visible = false;
                                    btnAlterarCargo.Visible = false;
                                    return;
                                }                                
                                pnPrincipal.Visible = true;
                                pnlAulas.Visible = true; 
                                tseFuncaoLotacao.DBValue = "10090";

                                if (Convert.ToString(dtDocente.Rows[0]["REGENTE"]) == "N" && Convert.ToString(dtDocente.Rows[0]["FUNC_PERMITE_GLP"]) == "S")
                                {
                                    hdnFuncaoNaoRegenteComGLP.Value = "S";
                                    pnPrincipal.Visible = false;
                                    pnlAulas.Visible = false;
                                    btnAlterarCargo.Visible = true;
                                }
                            }
                            else
                            {
                                tseFuncaoLotacao.DBValue = Convert.ToString(dtDocente.Rows[0]["CODFUNCAO"]);
                                btnAlterarCargo.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Docente não encontrado.";

                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        public object ListarTurmaCarenciaContratoGLP(object ano, object unidadeEnsino, object numFunc, object agrupamentoDisciplina)
        {
            RN.AulaDocente rnAulaDocente = new AulaDocente();

            var anoFiltro = ano != null ? ano.ToString() : null;
            var docente = numFunc != null ? numFunc.ToString() : null;
            var unidade = unidadeEnsino != null ? unidadeEnsino.ToString() : null;
            var agrupamento = agrupamentoDisciplina != null ? agrupamentoDisciplina.ToString() : null;

            if (!anoFiltro.IsNullOrEmptyOrWhiteSpace() && !docente.IsNullOrEmptyOrWhiteSpace() && !unidade.IsNullOrEmptyOrWhiteSpace() && !agrupamento.IsNullOrEmptyOrWhiteSpace())
            {
                return rnAulaDocente.ObtemTurmaCarenciaGlpContratoPor(Convert.ToInt32(anoFiltro), unidade.ToString(), Convert.ToInt32(docente), agrupamento == "TODAS" ? null : agrupamento.ToString());
            }
            return null;
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                btnIncluir.Visible = false;
                if (sessao != null)
                {
                    if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
                    {
                        sessao.Municipio = tseMunicipio.DBValue.ToString();
                        tseUnidadeEnsino.ResetValue();

                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                btnIncluir.Visible = false;
                if (sessao != null)
                {
                    if (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue)
                    {
                        sessao.Escola = tseUnidadeEnsino.DBValue.ToString();
                        CarregaDisciplina();
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {

                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue)
                    {
                        sessao.Regional = Convert.ToString(tseRegional.DBValue);
                        tseUnidadeEnsino.ResetValue();
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }


        }

        private void CarregaDisciplina()
        {
            RN.AulaDocente rnAulaDocente = new Techne.Lyceum.RN.AulaDocente();
            DataTable dtAulaDocente = new DataTable();
            ddlDisciplina.Items.Clear();


            if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue))
            {
                dtAulaDocente = rnAulaDocente.ObtemAgrupamentoCarenciaGlpContratoPor(Convert.ToInt32(ddlAno.SelectedValue), tseUnidadeEnsino.DBValue.ToString(), Convert.ToInt32(tseDocente["num_func"].ToString()));

                if (dtAulaDocente.Rows.Count > 0)
                {            
                    ddlDisciplina.DataSource = dtAulaDocente;
                    ListItem item = new ListItem("Selecione", string.Empty);
                    ListItem item1 = new ListItem("TODAS", "TODAS");
                    ddlDisciplina.DataBind();
                    ddlDisciplina.Items.Insert(0, item);
                    ddlDisciplina.Items.Insert(1, item1);
                   
                }
                else
                {
                    lblMensagem.Text = "Não há disciplina(s) com carência.";
                }
            }
            else
            {
                lblMensagem.Text = "Para listar as disciplinas é necessário preencher o Ano e a Unidade Escolar.";
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void LimparTela()
        {
            hdnFuncaoNaoRegenteComGLP.Value = string.Empty;
            hdnNumFunc.Value = string.Empty;
            txtNome.Text = String.Empty;
            txtMatricula.Text = String.Empty;
            txtIDVinculo.Text = string.Empty;

            txtCPF.Text = String.Empty;
            txtCargo.Text = String.Empty;
            txtFuncao.Text = String.Empty;
            txtDisciplinaIngresso.Text = String.Empty;
            txtSituacao.Text = String.Empty;
            txtReadaptado.Text = string.Empty;

            txtCHIngresso.Text = String.Empty;
            txtCHTurma.Text = String.Empty;
            txtCHNormal.Text = string.Empty;
            txtCHGLP.Text = string.Empty;

            txtRegional.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtUANome.Text = string.Empty;
            txtUALotacao.Text = string.Empty;

            txtSegundaMatricula.Text = String.Empty;
            tseFuncaoLotacao.ResetValue();
            tseCargo.ResetValue();
            dtConvocacao.Text = string.Empty;
            tseMunicipio.ResetValue();
            tseUnidadeEnsino.ResetValue();
            ddlDisciplina.Items.Clear();
            hdnCenso.Value = string.Empty;
            txtObservacao.Text = string.Empty;
        }

        protected void tseFuncaoLotacao_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
                if (!this.tseFuncaoLotacao.DBValue.IsNull)
                {
                    if (!this.tseFuncaoLotacao.IsValidDBValue)
                    {
                        lblMensagem.Text = "Função não cadastrada.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar uma função.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void ddlDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlGridTurmas.Visible = false;
                grdTurmaCarenciaContratoGLP.Selection.UnselectAll();
                grdTurmaCarenciaContratoGLP.DataBind();
                btnIncluir.Visible = false;

                if (!ddlDisciplina.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!this.tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue))
                {
                    pnlGridTurmas.Visible = true;
                    btnIncluir.Visible = true;
                }
            }
            catch (Exception EX)
            {
                lblMensagem.Text = EX.Message;
            }

        }
        protected void grdTurmaCarenciaContratoGLP_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTurmaCarenciaContratoGLP);
        }

        protected void grdTurmaCarenciaContratoGLP_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string ano = Convert.ToString(e.GetListSourceFieldValue("ANO"));
                string semestre = Convert.ToString(e.GetListSourceFieldValue("SEMESTRE"));
                string turma = Convert.ToString(e.GetListSourceFieldValue("TURMA"));
                string disciplina = Convert.ToString(e.GetListSourceFieldValue("DISCIPLINA"));
                string numFuncCarencia = Convert.ToString(e.GetListSourceFieldValue("NUM_FUNC_AULA"));
                string turno = Convert.ToString(e.GetListSourceFieldValue("TURNO"));
                string censo = Convert.ToString(e.GetListSourceFieldValue("CENSO"));
                string diaSemana = Convert.ToString(e.GetListSourceFieldValue("DIA_SEMANA"));
                string aula = Convert.ToString(e.GetListSourceFieldValue("AULA"));
                string data = Convert.ToDateTime(e.GetListSourceFieldValue("DATA_INICIO")).ToString("dd/MM/yyyy");

                e.Value = ano + "|" + semestre + "|" + turma + "|" + disciplina + "|" + numFuncCarencia + "|" + turno + "|" + censo + "|" + diaSemana + "|" + aula + "|" + data;
            }
        }

        protected void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> mensagens = new List<string>();
                DataTable dtCurrentTable = (DataTable)Session["grid"];

                List<object> fieldValues = grdTurmaCarenciaContratoGLP.GetSelectedFieldValues(new string[] { "CompositeKey", "REGIONAL", "MUNICIPIO", "DESCRICAOMUNICIPIO", "CENSO", "UA_ATUAL", 
                                "ESCOLA", "ANO", "SEMESTRE", "TURMA","TURNO", "DISCIPLINA", "NOMEDISCIPLINA", 
                                "DIA_SEMANA", "DIA_SEMANA_DESCRICAO", "HORA_INICIO", "HORA_FIM", "AULA", 
                                "TIPO", "DATA_INICIO", "DATA_FIM", "NUM_FUNC_AULA", "TIPO_GESTAO", "DEPENDENCIA", "CURSO", 
                                "SERIE", "OPTATIVAREFORCO", "CURRICULO", "DISCIPLINA_MULTIPLA","TIPODOCENTE"});

                if (fieldValues.Count() == 0)
                {
                    lblMensagem.Text = "Para adicionar é necessário selecionar pelo menos uma aula.";
                    lblMensagemMigracao.Text = lblMensagem.Text;
                    return;
                }
                //else
                //{ 
                //        btnMigrar.Visible = true;
                //}

                List<string> listaCompositeKey = new List<string>();

                for (var rowIndex = 0; rowIndex < this.grdSelecionado.VisibleRowCount; rowIndex++)
                {
                    string id = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "CompositeKey"));

                    if (!listaCompositeKey.Contains(id))
                    {
                        listaCompositeKey.Add(id);
                    }

                }


                foreach (object[] item in fieldValues)
                {
                    if (!listaCompositeKey.Contains(Convert.ToString(item[0])))
                    {
                        dtCurrentTable.Rows.Add(item[0].ToString(), item[1].ToString(), item[2].ToString(), item[3].ToString(), item[4].ToString(), item[5].ToString(),
                            item[6].ToString(), item[7].ToString(), item[8].ToString(), item[9].ToString(), item[10].ToString(), item[11].ToString(), item[12].ToString(),
                            item[13].ToString(), item[14].ToString(), item[15].ToString(), item[16].ToString(), item[17].ToString(), item[18].ToString(), item[19].ToString(),
                            item[20].ToString(), item[21].ToString(), item[22].ToString(), item[23].ToString(), item[24].ToString(), item[25].ToString(), item[26].ToString(),
                            item[27].ToString(), Convert.ToString(item[28]), Convert.ToString(item[29]));

                        Session["grid"] = dtCurrentTable;
                    }
                    //else
                    //{
                    //    mensagens.Add("A disciplina " + item[12] + " da turma " + item[9] + " e no horário de " + item[15] + " até " + item[16] + " já foi adicionado anteriormente. Verifique.");

                    //}
                }

                DataView view = new DataView(dtCurrentTable);
                DataTable distinctValues = view.ToTable(true, "CompositeKey", "REGIONAL", "MUNICIPIO", "DESCRICAOMUNICIPIO", "CENSO", "UA_ATUAL",
"ESCOLA", "ANO", "SEMESTRE", "TURMA", "TURNO", "DISCIPLINA", "NOMEDISCIPLINA",
 "DIA_SEMANA", "DIA_SEMANA_DESCRICAO", "HORA_INICIO", "HORA_FIM", "AULA",
"TIPO", "DATA_INICIO", "DATA_FIM", "NUM_FUNC_AULA", "TIPO_GESTAO", "DEPENDENCIA", "CURSO",
"SERIE", "OPTATIVAREFORCO", "CURRICULO", "DISCIPLINA_MULTIPLA", "TIPODOCENTE");

                grdSelecionado.DataSource = distinctValues;
                grdSelecionado.DataBind();


                if (mensagens.Count() > 0)
                {
                    lblMensagem.Text = mensagens.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                    lblMensagemMigracao.Text = lblMensagem.Text;
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemMigracao.Text = lblMensagem.Text;
            }
        }

        protected void grdSelecionado_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            DataTable dtCurrentTable = (DataTable)Session["grid"];
            var CompositeKey = Convert.ToString(grdSelecionado.GetRowValues(e.VisibleIndex, "CompositeKey"));

            if (e.ButtonID == "btnExcluir")
            {
                try
                {
                    dtCurrentTable.AcceptChanges();

                    DataRow[] dadosLinha = dtCurrentTable.Select("CompositeKey = '" + RN.RNBase.MudarAspas(CompositeKey) + "'");


                    dtCurrentTable.Rows.Remove(dadosLinha[0]);

                    dtCurrentTable.AcceptChanges();

                    Session["grid"] = dtCurrentTable;

                    grdSelecionado.DataSource = dtCurrentTable;
                    grdSelecionado.DataBind();


                    if (grdSelecionado.VisibleRowCount < 8)
                    {
                        dvMigrar.Visible = false;
                    }
                    else
                    {
                        dvMigrar.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    Session["Mensagem"] = ex.Message;

                }
            }
        }


        protected void btnMigrar_Click(object sender, EventArgs e)
        {
            try
            {
                Ly_turma dtTurma = new Ly_turma();
                Ly_hor_aula dtHoraAula = new Ly_hor_aula();
                RN.GradeSerie rnGradeSerie = new GradeSerie();
                RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();
                RN.RecursosHumanos.DTO.DadosAlocacaoMigracao dados = new Techne.Lyceum.RN.RecursosHumanos.DTO.DadosAlocacaoMigracao();
                List<RN.RecursosHumanos.DTO.DadosTurmaAlocacao> dadosAlocacao = new List<Techne.Lyceum.RN.RecursosHumanos.DTO.DadosTurmaAlocacao>();
                RN.RecursosHumanos.DTO.DadosTurmaAlocacao alocacao = new Techne.Lyceum.RN.RecursosHumanos.DTO.DadosTurmaAlocacao();
                ValidacaoDados validacao = new ValidacaoDados();
                DataTable dtDocente = new DataTable();
                RN.Docentes rnDocente = new Docentes();

                if (grdSelecionado.VisibleRowCount != 8)
                {
                    lblMensagem.Text = "Para efetuar a migração é necessário ter 8 tempos selecionados.";
                    lblMensagemMigracao.Text = lblMensagem.Text;
                    return;
                }


                for (var rowIndex = 0; rowIndex < this.grdSelecionado.VisibleRowCount; rowIndex++)
                {
                    alocacao = new Techne.Lyceum.RN.RecursosHumanos.DTO.DadosTurmaAlocacao();

                    alocacao.Ano = Convert.ToInt32(ddlAno.SelectedValue);
                    alocacao.Aula = Convert.ToInt32(grdSelecionado.GetRowValues(rowIndex, "AULA"));
                    alocacao.DataInicio = Convert.ToDateTime(grdSelecionado.GetRowValues(rowIndex, "DATA_INICIO"));
                    alocacao.DataFim = Convert.ToDateTime(grdSelecionado.GetRowValues(rowIndex, "DATA_FIM"));
                    alocacao.DiaSemana = Convert.ToInt32(grdSelecionado.GetRowValues(rowIndex, "DIA_SEMANA"));
                    alocacao.DiaSemanaDescricao = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "DIA_SEMANA_DESCRICAO"));
                    alocacao.Disciplina = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "DISCIPLINA"));
                    alocacao.DisciplinaMultipla = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "DISCIPLINA_MULTIPLA"));
                    alocacao.Faculdade = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "CENSO"));
                    alocacao.NomeDisciplina = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "NOMEDISCIPLINA"));
                    alocacao.NumFuncAnterior = Convert.ToInt32(grdSelecionado.GetRowValues(rowIndex, "NUM_FUNC_AULA"));
                    alocacao.Semestre = Convert.ToInt32(grdSelecionado.GetRowValues(rowIndex, "SEMESTRE"));
                    alocacao.TipoAula = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "TIPO"));
                    alocacao.Turma = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "TURMA"));
                    alocacao.Turno = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "TURNO"));
                    alocacao.TipoDocente = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "TIPODOCENTE"));

                    DateTime horaIni = Convert.ToDateTime(grdSelecionado.GetRowValues(rowIndex, "HORA_INICIO"));
                    DateTime horaFim = Convert.ToDateTime(grdSelecionado.GetRowValues(rowIndex, "HORA_FIM"));

                    alocacao.HoraInicio = new DateTime(1899, 12, 30, horaIni.Hour, horaIni.Minute, horaIni.Second, horaIni.Millisecond);
                    alocacao.HoraFim = new DateTime(1899, 12, 30, horaFim.Hour, horaFim.Minute, horaFim.Second, horaFim.Millisecond);


                    CR.Ly_turma.Row dadosTurma = dtTurma.NewRow();

                    dadosTurma.Tipo_gestao = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "TIPO_GESTAO"));
                    dadosTurma.Ano = Convert.ToDecimal(ddlAno.SelectedValue);
                    dadosTurma.Semestre = Convert.ToDecimal(grdSelecionado.GetRowValues(rowIndex, "SEMESTRE"));
                    dadosTurma.Turno = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "TURNO"));
                    dadosTurma.Curso = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "CURSO"));
                    dadosTurma.OptativaReforco = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "OPTATIVAREFORCO"));
                    dadosTurma.Serie = Convert.ToDecimal(grdSelecionado.GetRowValues(rowIndex, "SERIE")); ;
                    dadosTurma.Faculdade = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "CENSO"));
                    dadosTurma.Turma = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "TURMA"));
                    dadosTurma.Dt_inicio = Convert.ToDateTime(grdSelecionado.GetRowValues(rowIndex, "DATA_INICIO"));
                    dadosTurma.Dt_fim = Convert.ToDateTime(grdSelecionado.GetRowValues(rowIndex, "DATA_FIM"));
                    dadosTurma.Curriculo = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "CURRICULO"));

                    Ly_hor_aula.Row linhaHoraAula = dtHoraAula.NewRow();

                    linhaHoraAula.Ano = Convert.ToInt32(grdSelecionado.GetRowValues(rowIndex, "ANO"));
                    linhaHoraAula.Aula = Convert.ToInt32(grdSelecionado.GetRowValues(rowIndex, "AULA"));
                    linhaHoraAula.Dependencia = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "DEPENDENCIA"));
                    linhaHoraAula.Dia_semana = Convert.ToDecimal(grdSelecionado.GetRowValues(rowIndex, "DIA_SEMANA"));
                    linhaHoraAula.Disciplina = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "DISCIPLINA"));
                    linhaHoraAula.Faculdade = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "CENSO"));
                    linhaHoraAula.Horaini_aula = new DateTime(1899, 12, 30, horaIni.Hour, horaIni.Minute, horaIni.Second, horaIni.Millisecond);
                    linhaHoraAula.Horafim_aula = new DateTime(1899, 12, 30, horaFim.Hour, horaFim.Minute, horaFim.Second, horaFim.Millisecond);
                    linhaHoraAula.Semestre = Convert.ToDecimal(grdSelecionado.GetRowValues(rowIndex, "SEMESTRE"));
                    linhaHoraAula.Turma = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "TURMA"));
                    linhaHoraAula.Turno = Convert.ToString(grdSelecionado.GetRowValues(rowIndex, "TURNO"));
                    linhaHoraAula.Qtde_aula = 1;

                    dtHoraAula.Rows.Add(linhaHoraAula);

                    alocacao.DadosHoraAula = linhaHoraAula;
                    alocacao.DadosTurma = dadosTurma;

                    dadosAlocacao.Add(alocacao);
                }

                dados.Pessoa = (!tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue) ? Convert.ToInt32(tseDocente["pessoa"]) : -1;
                dados.Aulas = dadosAlocacao;
                dados.Categoria = (!tseCargo.DBValue.IsNull && tseCargo.IsValidDBValue) ? tseCargo.DBValue.ToString() : null;
                dados.CategoriaAnterior = (!tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue) ? Convert.ToString(tseDocente["categoria"]) : null;
                dados.DocenteCandidatoId = (!tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue) ? Convert.ToInt32(tseDocente["DOCENTECANDIDATOID"]) : -1;
                dados.Funcao = (!tseFuncaoLotacao.DBValue.IsNull && tseFuncaoLotacao.IsValidDBValue) ? tseFuncaoLotacao.DBValue.ToString() : null;
                dados.MatriculaDocente = (!tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue) ? tseDocente["matricula"].ToString() : null;
                dados.NumFunc = (!tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue) ? Convert.ToInt32(tseDocente["num_func"]) : -1;
                dados.UsuarioId = User.Identity.Name;
                dados.Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                dados.DataConvocacaoDO = !dtConvocacao.Text.IsNullOrEmptyOrWhiteSpace() ? dtConvocacao.Date : DateTime.MinValue;
                dados.Observacao = !txtObservacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtObservacao.Text : null;

                validacao = rnDocenteCandidato.ValidaAlocacao(dados,tseConcursoBusca.DBValue.ToString());

                if (validacao.Valido)
                {
                    rnDocenteCandidato.Aloca(dados);
                    lblMensagem.Text = "Migração concluida com sucesso.";
                    lblMensagemMigracao.Text = lblMensagem.Text;

                    grdSelecionado.DataBind();
                    grdAulas.DataBind();
                    grdTurmaCarenciaContratoGLP.DataBind();
                    pnPrincipal.Visible = false;
                    dvMigrar.Visible = false;
                    dvRelatorio.Visible = true;

                    dtDocente = rnDocente.ObtemDadosDocenteMigracaoPor(Convert.ToInt32(ddlAno.SelectedValue), tseDocente.DBValue.ToString());

                    if (dtDocente.Rows.Count == 0)
                    {
                        lblMensagem.Text = "Informações do docente não encontrado ou não ativo.";

                    }
                    else
                    {
                        txtCargo.Text = Convert.ToString(dtDocente.Rows[0]["cargo"]);
                        txtFuncao.Text = Convert.ToString(dtDocente.Rows[0]["funcao"]);
                        txtCHIngresso.Text = Convert.ToString(dtDocente.Rows[0]["ch_regencia"]);
                        txtCHTurma.Text = Convert.ToString(dtDocente.Rows[0]["hor_tur"]);
                        txtCHNormal.Text = Convert.ToString(dtDocente.Rows[0]["tol_normal"]);
                        txtCHGLP.Text = Convert.ToString(dtDocente.Rows[0]["tol_glp"]);
                    }

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    lblMensagemMigracao.Text = lblMensagem.Text;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemMigracao.Text = lblMensagem.Text;

                grdSelecionado.DataSource = null;
                grdSelecionado.DataBind();

                grdAulas.DataBind();
                grdTurmaCarenciaContratoGLP.DataBind();
            }
        }

        private void InicializaGrid()
        {

            DataTable dt = new DataTable();

            DataRow dr = null;


            dt.Columns.Add(new DataColumn("CompositeKey", typeof(string)));
            dt.Columns.Add(new DataColumn("REGIONAL", typeof(string)));
            dt.Columns.Add(new DataColumn("MUNICIPIO", typeof(string)));
            dt.Columns.Add(new DataColumn("DESCRICAOMUNICIPIO", typeof(string)));
            dt.Columns.Add(new DataColumn("CENSO", typeof(string)));
            dt.Columns.Add(new DataColumn("UA_ATUAL", typeof(string)));
            dt.Columns.Add(new DataColumn("ESCOLA", typeof(string)));
            dt.Columns.Add(new DataColumn("ANO", typeof(string)));
            dt.Columns.Add(new DataColumn("SEMESTRE", typeof(string)));
            dt.Columns.Add(new DataColumn("TURMA", typeof(string)));
            dt.Columns.Add(new DataColumn("TURNO", typeof(string)));
            dt.Columns.Add(new DataColumn("DISCIPLINA", typeof(string)));
            dt.Columns.Add(new DataColumn("NOMEDISCIPLINA", typeof(string)));
            dt.Columns.Add(new DataColumn("DIA_SEMANA", typeof(string)));
            dt.Columns.Add(new DataColumn("DIA_SEMANA_DESCRICAO", typeof(string)));
            dt.Columns.Add(new DataColumn("HORA_INICIO", typeof(string)));
            dt.Columns.Add(new DataColumn("HORA_FIM", typeof(string)));
            dt.Columns.Add(new DataColumn("AULA", typeof(string)));
            dt.Columns.Add(new DataColumn("TIPO", typeof(string)));
            dt.Columns.Add(new DataColumn("DATA_INICIO", typeof(string)));
            dt.Columns.Add(new DataColumn("DATA_FIM", typeof(string)));
            dt.Columns.Add(new DataColumn("NUM_FUNC_AULA", typeof(string)));
            dt.Columns.Add(new DataColumn("TIPO_GESTAO", typeof(string)));
            dt.Columns.Add(new DataColumn("DEPENDENCIA", typeof(string)));
            dt.Columns.Add(new DataColumn("CURSO", typeof(string)));
            dt.Columns.Add(new DataColumn("SERIE", typeof(string)));
            dt.Columns.Add(new DataColumn("OPTATIVAREFORCO", typeof(string)));
            dt.Columns.Add(new DataColumn("CURRICULO", typeof(string)));
            dt.Columns.Add(new DataColumn("DISCIPLINA_MULTIPLA", typeof(string)));
            dt.Columns.Add(new DataColumn("TIPODOCENTE", typeof(string)));

            dr = dt.NewRow();

            Session["grid"] = dt;

            grdSelecionado.DataSource = dt;
            grdSelecionado.DataBind();

        }

        protected void btnAlterarCargo_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new Techne.Lyceum.RN.Util.ValidacaoDados();
                RN.RecursosHumanos.DocenteCandidato rnDocenteCandidato = new Techne.Lyceum.RN.RecursosHumanos.DocenteCandidato();

                if (!tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue)
                {
                    if (!tseCargo.DBValue.IsNull && tseCargo.IsValidDBValue)
                    {

                        validacao = rnDocenteCandidato.ValidaAtualizaCargo(Convert.ToInt32(tseDocente["docentecandidatoid"]), Convert.ToInt32(tseDocente["num_func"]), tseCargo.DBValue.ToString(), User.Identity.Name, hdnFuncaoNaoRegenteComGLP.Value == "S" ? true : false, tseDocente["matricula"].ToString(), (dtConvocacao.Text.IsNullOrEmptyOrWhiteSpace() ? DateTime.MinValue : dtConvocacao.Date), tseConcursoBusca.DBValue.ToString(), Convert.ToInt32(tseDocente["pessoa"]));

                        if (validacao.Valido)
                        {
                            rnDocenteCandidato.AtualizaCargo(Convert.ToInt32(tseDocente["docentecandidatoid"]), Convert.ToInt32(tseDocente["num_func"]), tseCargo.DBValue.ToString(), User.Identity.Name, tseDocente["matricula"].ToString(),dtConvocacao.Date);

                            lblMensagem.Text = "Troca de cargo realizada. Migração concluida com sucesso.";
                            lblMensagem.Visible = true;
                            txtCargo.Text = tseCargo["nome"].ToString();
                            tseCargo.ResetValue();
                            dtConvocacao.Enabled = false;

                            btnAlterarCargo.Visible = false;
                        }
                        else
                        {
                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Para efetuar a troca é necessário escolher um Cargo.";
                    }
                    lblMensagem.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseConcursoBusca_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (!tseConcursoBusca.DBValue.IsNull && tseConcursoBusca.IsValidDBValue)
                {
                    // lblMensagem.Text = string.Empty;
                   
                }
                else
                {
                    lblMensagem.Text = "Favor informar um processo seletivo.";

                }               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


    }
}
