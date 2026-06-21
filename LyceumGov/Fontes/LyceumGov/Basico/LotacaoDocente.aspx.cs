using System;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.RN;
using System.Web;
using Techne.Lyceum.CR;
using System.Web.Services;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Helpers;
using System.Web.UI.WebControls;
using System.Web.UI;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/LotacaoDocente.aspx"),
    ControlText("Lotação Docente"),
    Title("Lotação de Docentes"),]

    public partial class LotacaoDocente : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdLotacao, "Lotações do Docente");
            TituloGrid(grdLicencas, "Situações do Docente");
        }

        protected void ddlMatriculaLic_SelectedIndexChanged(object sender, EventArgs e)
        {
            ASPxComboBox cmbSerie = sender as ASPxComboBox;
            int vinculo = Convert.ToInt32(grdLicencas.GetDataRow(grdLicencas.EditingRowVisibleIndex)["VINCULO"]);
            ASPxTextBox txtVinculo = (ASPxTextBox)grdLicencas.FindEditFormTemplateControl("txtVinculo");
            ASPxComboBox ddlMatriculaLic = (ASPxComboBox)grdLicencas.FindEditFormTemplateControl("ddlMatriculaLic");

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.PadroesDeAcessos rnPadrao = new PadroesDeAcessos();

                lblMensagem.Text = string.Empty;
                lblMensagemLicenca.Text = string.Empty;

                if (!IsPostBack)
                {
                    hdnPadraoQHI.Value = string.Empty;

                    if (rnPadrao.PossuiPadraoCoordQHIPor(User.Identity.Name))
                    {
                        hdnPadraoQHI.Value = "S";
                    }

                    if (rnPadrao.PossuiPadraoCOCACPor(User.Identity.Name))
                    {
                        hdnPadraoCOCAC.Value = "S";
                    }
                }

                //Resposta para requisição que retorna POSSUI_DTFIM da situação.
                if (!string.IsNullOrEmpty(Request["Motivo"]))
                {
                    Response.Write(RN.Licencas.PossuiDataFim(Request["Motivo"]) ? "S" : "N");
                    Response.End();
                }
                else
                {
                    txtUsuarioHidden.Text = HttpContext.Current.User.Identity.Name;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdLotacao);
            ControlaAcesso(grdLicencas);
        }

        protected void tseDocente_Changed(object sender, EventArgs args)
        {
            try
            {
                if (tseDocente.IsValidDBValue && !tseDocente.DBValue.IsNull)
                {
                    grdLotacao.Visible = true;
                    grdLotacao.CancelEdit();
                    grdLicencas.Visible = true;
                    grdLicencas.CancelEdit();
                    lblMensagem.Text = string.Empty;
                    txtPessoaHidden.Text = tseDocente["pessoa"].ToString();
                    odsMatricula.DataBind();
                }
                else if (!tseDocente.DBValue.IsNull)
                {
                    grdLotacao.Visible = false;
                    grdLicencas.Visible = false;
                    lblMensagem.Text = "Docente não cadastrado.";
                    txtPessoaHidden.Text = string.Empty;
                }
                else
                {
                    grdLotacao.Visible = false;
                    grdLicencas.Visible = false;
                    lblMensagem.Text = "Favor consultar um docente.";
                    txtPessoaHidden.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        #region Eventos Grid Lotação
        protected void grdLotacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            RN.Lotacao rnLotacao = new Lotacao();
            e.NewValues["ordem"] = rnLotacao.ObtemProximaOrdemPor(tseDocente["matricula"].ToString());
            e.NewValues["pessoa"] = Convert.ToDecimal(tseDocente["pessoa"].ToString());
            e.NewValues["unidade_fis"] = null;

            grdLotacao.Settings.ShowFilterRow = false;
        }

        protected void grdLotacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdLotacao.Settings.ShowFilterRow = false;
        }

        protected void grdLotacao_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            bool eFuncaoGRT = false;
            
            var podeExcluir = (string)grdLotacao.GetRowValues(e.VisibleIndex, "PODE_REMOVER");

            var tipoFuncao = (string)grdLotacao.GetRowValues(e.VisibleIndex, "TIPOFUNCAO");

            if (tipoFuncao == "COMISSIONADA" || tipoFuncao == "GRATIFICADA")
                eFuncaoGRT = true;
            
            if (hdnPadraoQHI.Value == "S")
                podeExcluir = "S";

            if (!string.IsNullOrEmpty(podeExcluir))
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    if (podeExcluir == "S")
                    {
                        e.Visible = true;
                    }
                    else
                    {
                        e.Visible = false;
                    }
                }
            }

            if (eFuncaoGRT && hdnPadraoCOCAC.Value != "S")
            {
                if (e.ButtonType == ColumnCommandButtonType.Delete)
                {
                    e.Visible = false;
                }

                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }
        }

        protected void grdLotacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {

                lblMensagem.Text = string.Empty;
                RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Entidades.LyLotacao lotacao = new LyLotacao();
                RN.Entidades.LyLicencaDocente licenca = new LyLicencaDocente();
                decimal numFunc = -1;
                RN.Entidades.LyLotacao lotacaoAnterior = new LyLotacao();
                RN.Lotacao rnLotacao = new Lotacao();
                RN.AulaDocente rnAulaDocente = new AulaDocente();
                RN.AulaDocenteTipo rnAulaDocenteTipo = new AulaDocenteTipo();
                string retorno = string.Empty;

                TSearchBox tseSetor = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseSetor");

                if (e.NewValues["MATRICULA"] != null)
                {
                    numFunc = rnDocentes.ObtemNumFuncPor(Convert.ToString(e.NewValues["MATRICULA"]));
                }

                lotacao.AtoOficial = e.NewValues["ATO_OFICIAL"] != null ? Convert.ToString(e.NewValues["ATO_OFICIAL"]) : null;
                lotacao.DataAtualizacao = DateTime.Now;
                lotacao.DataDesativacao = e.NewValues["DATA_DESATIVACAO"] != null ? Convert.ToDateTime(e.NewValues["DATA_DESATIVACAO"]) : (DateTime?)null;
                lotacao.DataDesativacaoDo = e.NewValues["DATA_DESATIVACAO_DO"] != null ? Convert.ToDateTime(e.NewValues["DATA_DESATIVACAO_DO"]) : (DateTime?)null;
                lotacao.DataNomeacao = e.NewValues["DATA_NOMEACAO"] != null ? Convert.ToDateTime(e.NewValues["DATA_NOMEACAO"]) : DateTime.MinValue;
                lotacao.DataNomeacaoDo = e.NewValues["DATA_NOMEACAO_DO"] != null ? Convert.ToDateTime(e.NewValues["DATA_NOMEACAO_DO"]) : (DateTime?)null;
                lotacao.DtFimReadaptacao = (DateTime?)null;
                lotacao.DtInicioReadaptacao = (DateTime?)null;
                lotacao.Funcao = e.NewValues["FUNCAO"] != null ? Convert.ToString(e.NewValues["FUNCAO"]) : null;
                lotacao.Matricula = e.NewValues["MATRICULA"] != null ? Convert.ToString(e.NewValues["MATRICULA"]) : null;
                lotacao.MotivoReadaptacao = null;
                lotacao.Ordem = e.NewValues["MATRICULA"] != null ? rnLotacao.ObtemProximaOrdemPor(Convert.ToString(e.NewValues["MATRICULA"])) : -1;
                lotacao.Pessoa = Convert.ToDecimal(tseDocente["pessoa"].ToString());
                lotacao.Readaptado = null;
                lotacao.RespDocumentacao = e.NewValues["RESP_DOCUMENTACAO"] != null ? Convert.ToString(e.NewValues["RESP_DOCUMENTACAO"]) : null;
                lotacao.Setor = !tseSetor.DBValue.IsNull && tseSetor.IsValidDBValue ? tseSetor["setor"].ToString() : null;
                lotacao.Categoria = rnDocentes.ObtemCategoriaPor(Convert.ToString(e.NewValues["MATRICULA"]));
                lotacao.TipoDesativacao = null;
                lotacao.Turno = null;
                lotacao.Usuario = User.Identity.Name;
                if (lotacao.Setor != null)
                {
                    lotacao.UnidadeEns = RN.Setores.ConsultaSetorUniEns(lotacao.Setor);
                    lotacao.UnidadeFis = lotacao.UnidadeEns;

                    if (!string.IsNullOrEmpty(lotacao.UnidadeEns))
                    {
                        lotacao.Nucleo = RN.Setores.ConsultaNucleoUniEns(lotacao.UnidadeEns);
                    }
                    else if (!string.IsNullOrEmpty(lotacao.Nucleo))
                    {
                        lotacao.UnidadeEns = null;
                        lotacao.UnidadeFis = null;
                    }
                    else
                    {
                        lotacao.UnidadeEns = null;
                        lotacao.UnidadeFis = null;
                        lotacao.Nucleo = null;
                    }
                }
                licenca.NumFunc = numFunc;

                if (e.NewValues["MOTIVO"] != null)
                {

                    licenca.Motivo = Convert.ToString(e.NewValues["MOTIVO"]);
                    if (e.NewValues["DTINI"] != null)
                    {
                        if (!RN.Util.Utils.IsValidSqlDatetime(e.NewValues["DTINI"].ToString()))
                        {
                            e.Cancel = true;
                            throw new Exception("Data Início da Situação Inválida.");
                        }
                        else
                        {
                            licenca.Dtini = Convert.ToDateTime(e.NewValues["DTINI"]);
                        }
                    }
                    else
                    {
                        licenca.Dtini = DateTime.MinValue;
                    }

                    if (e.NewValues["DTFIM"] != null)
                    {
                        if (!RN.Util.Utils.IsValidSqlDatetime(e.NewValues["DTFIM"].ToString()))
                        {
                            e.Cancel = true;
                            throw new Exception("Data Fim da Situação Inválida.");

                        }
                        else
                        {
                            licenca.Dtfim = Convert.ToDateTime(e.NewValues["DTFIM"]);
                        }
                    }
                    else
                    {
                        licenca.Dtfim = DateTime.MinValue;
                    }
                }

                if (Session["possuiDataFimLotacao"] == null && !string.IsNullOrEmpty(licenca.Motivo))
                {
                    Session["possuiDataFimLotacao"] = RN.Licencas.PossuiDataFim(licenca.Motivo);
                }

                if (rnAulaDocenteTipo.PossuiGLPAtiva(numFunc) && (lotacao.Funcao == "11" || lotacao.Funcao == "18"))
                {
                    //Alimenta linha
                    HttpContext.Current.Session["lotacao"] = new RN.Entidades.LyLotacao();
                    HttpContext.Current.Session["lotacao"] = lotacao;

                    //Alimenta linhalic
                    HttpContext.Current.Session["licencaDocente"] = new RN.Entidades.LyLicencaDocente();
                    HttpContext.Current.Session["licencaDocente"] = licenca;

                    //Se existe chama pop-up
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopupCPOE();", true);
                    e.Cancel = true;
                    return;
                }

                if (rnLotacao.PossuiLotacaoAtivaPor(lotacao.Matricula))
                {
                    //Alimenta linha
                    HttpContext.Current.Session["lotacao"] = new RN.Entidades.LyLotacao();
                    HttpContext.Current.Session["lotacao"] = lotacao;

                    //Alimenta linhalic
                    HttpContext.Current.Session["licencaDocente"] = new RN.Entidades.LyLicencaDocente();
                    HttpContext.Current.Session["licencaDocente"] = licenca;

                    //Se existe chama pop-up
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                    e.Cancel = true;
                    return;
                }

                retorno = InsereLotacao(lotacao, licenca, false, numFunc, false);
                if (string.IsNullOrEmpty(retorno))
                {
                    lblMensagem.Text = string.Empty;
                    grdLotacao.DataBind();
                    grdLicencas.DataBind();
                    tseDocente.Enabled = true;
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(retorno);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private string InsereLotacao(RN.Entidades.LyLotacao lotacao, RN.Entidades.LyLicencaDocente licencaDocente, bool finalizarAnterior, decimal numFunc, bool desalocaOECP)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();
                RN.Entidades.LyLotacao lotacaoAnterior = new LyLotacao();
                bool possuiDataFim = Session["possuiDataFimLotacao"] != null ? Convert.ToBoolean(Session["possuiDataFimLotacao"]) : true;

                validacao = rnLotacao.ValidaInsercaoLotacaoDocente(lotacao, licencaDocente, finalizarAnterior, out lotacaoAnterior, possuiDataFim, numFunc, desalocaOECP);

                if (validacao.Valido)
                {
                    rnLotacao.InsereLotacaoDocente(lotacao, licencaDocente, finalizarAnterior, lotacaoAnterior, possuiDataFim, numFunc, desalocaOECP, tseDocente.DBValue.ToString(), tseDocente["nome"].ToString());
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                }
            }
            catch (Exception e)
            {
                lblMensagem.Text = e.Message.ToString();
            }
            return lblMensagem.Text;

        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Entidades.LyLotacao lotacao = new LyLotacao();
                RN.Entidades.LyLicencaDocente licencaDocente = new LyLicencaDocente();
                lblMensagem.Text = string.Empty;

                string funcaoAtual = string.Empty;

                this.pucConfirmar.ShowOnPageLoad = false;

                lotacao = (RN.Entidades.LyLotacao)HttpContext.Current.Session["lotacao"];
                licencaDocente = (RN.Entidades.LyLicencaDocente)HttpContext.Current.Session["licencaDocente"];

                if (string.IsNullOrEmpty(this.InsereLotacao(lotacao, licencaDocente, true, licencaDocente.NumFunc, false)))
                {
                    grdLotacao.CancelEdit();
                    grdLotacao.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmar.ShowOnPageLoad = false;
            grdLotacao.CancelEdit();
        }

        protected void btnSimDesalocacao_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Entidades.LyLotacao lotacaoAtualizacao = new LyLotacao();
                RN.Entidades.LyLotacao proximaLotacao = new LyLotacao();
                RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();
                lblMensagem.Text = string.Empty;
                decimal numFunc = -1;

                this.pucConfirmarDesalocacao.ShowOnPageLoad = false;

                lotacaoAtualizacao = (RN.Entidades.LyLotacao)HttpContext.Current.Session["lotacaoAtualizacao"];
                proximaLotacao = (RN.Entidades.LyLotacao)HttpContext.Current.Session["proximaLotacao"];
                numFunc = (Decimal)HttpContext.Current.Session["numFunc"];

                rnLotacao.AlteraLotacaoDocente(lotacaoAtualizacao, numFunc, proximaLotacao, true);

                grdLotacao.CancelEdit();
                grdLotacao.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnNaoDesalocacao_Click(object sender, EventArgs e)
        {

            this.pucConfirmarDesalocacao.ShowOnPageLoad = false;
            grdLotacao.CancelEdit();
        }

        protected void btnNaoCPOE_Click(object sender, EventArgs e)
        {

            this.pucConfirmarCPOE.ShowOnPageLoad = false;
            grdLotacao.CancelEdit();
        }

        protected void btnSimCPOE_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Entidades.LyLotacao lotacao = new LyLotacao();
                RN.Entidades.LyLicencaDocente licencaDocente = new LyLicencaDocente();
                lblMensagem.Text = string.Empty;

                string funcaoAtual = string.Empty;

                this.pucConfirmarCPOE.ShowOnPageLoad = false;

                lotacao = (RN.Entidades.LyLotacao)HttpContext.Current.Session["lotacao"];
                licencaDocente = (RN.Entidades.LyLicencaDocente)HttpContext.Current.Session["licencaDocente"];

                if (string.IsNullOrEmpty(this.InsereLotacao(lotacao, licencaDocente, true, licencaDocente.NumFunc, true)))
                {
                    grdLotacao.CancelEdit();
                    grdLotacao.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdLotacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
                RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();
                RN.Entidades.LyLotacao lotacao = new LyLotacao();
                RN.Entidades.LyLotacao proximaLotacao = new LyLotacao();
                bool desalocaAula = false;

                TSearchBox tseSetor = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseSetor");

                decimal numFunc = rnDocentes.ObtemNumFuncPor(Convert.ToString(e.Keys["MATRICULA"]));

                lotacao.AtoOficial = e.NewValues["ATO_OFICIAL"] != null ? Convert.ToString(e.NewValues["ATO_OFICIAL"]) : null;
                lotacao.Readaptado = Convert.ToString(e.NewValues["READAPTADO"]) == "Sim" ? "S" : "N";
                lotacao.DataAtualizacao = DateTime.Now;
                lotacao.DataDesativacao = e.NewValues["DATA_DESATIVACAO"] != null ? Convert.ToDateTime(e.NewValues["DATA_DESATIVACAO"]) : (DateTime?)null;
                lotacao.DataDesativacaoDo = e.NewValues["DATA_DESATIVACAO_DO"] != null ? Convert.ToDateTime(e.NewValues["DATA_DESATIVACAO_DO"]) : (DateTime?)null;
                lotacao.DataNomeacao = e.NewValues["DATA_NOMEACAO"] != null ? Convert.ToDateTime(e.NewValues["DATA_NOMEACAO"]) : DateTime.MinValue;
                lotacao.DataNomeacaoDo = e.NewValues["DATA_NOMEACAO_DO"] != null ? Convert.ToDateTime(e.NewValues["DATA_NOMEACAO_DO"]) : (DateTime?)null;
                lotacao.Funcao = e.NewValues["FUNCAO"] != null ? Convert.ToString(e.NewValues["FUNCAO"]) : null;
                lotacao.Matricula = e.Keys["MATRICULA"].ToString();
                lotacao.Ordem = Convert.ToDecimal(e.Keys["ORDEM"].ToString());
                lotacao.Pessoa = Convert.ToDecimal(tseDocente["pessoa"].ToString());
                lotacao.RespDocumentacao = e.NewValues["RESP_DOCUMENTACAO"] != null ? Convert.ToString(e.NewValues["RESP_DOCUMENTACAO"]) : null;
                lotacao.Setor = !tseSetor.DBValue.IsNull && tseSetor.IsValidDBValue ? tseSetor["setor"].ToString() : null;
                lotacao.Categoria = rnDocentes.ObtemCategoriaPor(Convert.ToString(e.Keys["MATRICULA"]));
                lotacao.Usuario = User.Identity.Name;
                if (lotacao.Setor != null)
                {
                    lotacao.UnidadeEns = RN.Setores.ConsultaSetorUniEns(lotacao.Setor);
                    lotacao.UnidadeFis = lotacao.UnidadeEns;

                    if (!string.IsNullOrEmpty(lotacao.UnidadeEns))
                    {
                        lotacao.Nucleo = RN.Setores.ConsultaNucleoUniEns(lotacao.UnidadeEns);
                    }
                    else if (!string.IsNullOrEmpty(lotacao.Nucleo))
                    {
                        lotacao.UnidadeEns = null;
                        lotacao.UnidadeFis = null;
                    }
                    else
                    {
                        lotacao.UnidadeEns = null;
                        lotacao.UnidadeFis = null;
                        lotacao.Nucleo = null;
                    }
                }

                validacao = rnLotacao.ValidaAlteracaoLotacaoDocente(lotacao, numFunc, out proximaLotacao, out desalocaAula);

                if (validacao.Valido)
                {
                    if (desalocaAula)
                    {
                        //Alimenta lotacao
                        HttpContext.Current.Session["lotacaoAtualizacao"] = new RN.Entidades.LyLotacao();
                        HttpContext.Current.Session["lotacaoAtualizacao"] = lotacao;

                        //Alimenta proxima lotacao
                        HttpContext.Current.Session["proximaLotacao"] = new RN.Entidades.LyLotacao();
                        HttpContext.Current.Session["proximaLotacao"] = proximaLotacao;
                        HttpContext.Current.Session["numFunc"] = numFunc;

                        //Se existe chama pop-up
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopupDesalocacao();", true);
                        e.Cancel = true;
                        return;
                    }

                    rnLotacao.AlteraLotacaoDocente(lotacao, numFunc, proximaLotacao, desalocaAula);
                    lblMensagem.Text = string.Empty;
                    grdLotacao.DataBind();
                    tseDocente.Enabled = true;
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdLotacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();
                lblMensagem.Text = string.Empty;

                validacao = rnLotacao.ValidaRemocao(Convert.ToDecimal(e.Keys["PESSOA"]), e.Keys["MATRICULA"].ToString(), Convert.ToDecimal(e.Keys["ORDEM"]));

                if (validacao.Valido)
                {
                    rnLotacao.Remove(Convert.ToDecimal(e.Keys["PESSOA"]), e.Keys["MATRICULA"].ToString(), Convert.ToDecimal(e.Keys["ORDEM"]));
                    grdLotacao.DataBind();
                    tseDocente.Enabled = true;
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void grdLotacao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdLotacao.IsNewRowEditing)
            {
                tseDocente.Enabled = false;
                if ((e.Column.FieldName) == "MATRICULA")
                {
                    e.Editor.ReadOnly = false;
                }
            }
            else if (grdLotacao.IsEditing)
            {
                tseDocente.Enabled = false;
                if ((e.Column.FieldName) == "MATRICULA")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                if ((e.Column.FieldName) == "FUNCAO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "DATA_NOMEACAO")
                {
                    e.Editor.ClientEnabled = false;
                }
                if ((e.Column.FieldName) == "MOTIVO")
                {
                    e.Editor.ClientEnabled = false;
                }
                if ((e.Column.FieldName) == "DTINI")
                {
                    e.Editor.ClientEnabled = false;
                }
                if ((e.Column.FieldName) == "DTFIM")
                {
                    e.Editor.ClientEnabled = false;
                }
            }
        }

        protected void grdLotacao_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            tseDocente.Enabled = true;
        }

        protected void grdLotacao_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdLotacao);
            ControlaAcesso(grdLicencas);
        }

        protected void cmbMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Lotacao rnLotacao = new Lotacao();
            RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();

            ASPxDateEdit dataNomeacao = (ASPxDateEdit)grdLotacao.FindEditFormTemplateControl("DATA_NOMEACAO");
            ASPxComboBox cmbMatricula = (ASPxComboBox)grdLotacao.FindEditFormTemplateControl("cmbMatricula");
            DateTime? novaDataNomeacao = null;

            //Busca pessoa da matricula selecionada
            decimal pessoa = rnDocentes.ObtemPessoaPor(cmbMatricula.Value.ToString());
            if (pessoa != Convert.ToDecimal(tseDocente["pessoa"].ToString()))
            {
                grdLotacao.CancelEdit();
                tseDocente_Changed(null, null);
            }
            else
            {
                if (cmbMatricula.Value != null)
                {
                    dataNomeacao.ClientEnabled = true;
                    dataNomeacao.Text = string.Empty;
                    novaDataNomeacao = rnLotacao.ObtemUltimaDataDesativacaoPor(cmbMatricula.Value.ToString());

                    if (novaDataNomeacao != null && novaDataNomeacao != DateTime.MinValue)
                    {
                        dataNomeacao.Text = novaDataNomeacao.Value.Date.AddDays(1).ToShortDateString();
                        dataNomeacao.ClientEnabled = false;
                    }
                }
            }
        }

        protected void grdLotacao_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (!this.grdLotacao.Visible
             || this.grdLotacao.VisibleRowCount == 0
             )
            {
                return;
            }

            RN.PadroesDeAcessos rnPadrao = new PadroesDeAcessos();

            ASPxCheckBox chkReadaptado = (ASPxCheckBox)grdLotacao.FindEditFormTemplateControl("chkReadaptado");

            if (grdLotacao.IsEditing && !grdLotacao.IsNewRowEditing)
            {
                TSearchBox tseFuncao = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseFuncao");
                ASPxDateEdit dataNomeacao = (ASPxDateEdit)grdLotacao.FindEditFormTemplateControl("DATA_NOMEACAO");
                ASPxComboBox cmbMatricula = (ASPxComboBox)grdLotacao.FindEditFormTemplateControl("cmbMatricula");

                if (hdnPadraoQHI.Value == "S")
                {
                    if (tseFuncao != null)
                    {
                        tseFuncao.Mode = Techne.Controls.ControlMode.Edit;
                    }
                    if (dataNomeacao != null)
                    {
                        dataNomeacao.ClientEnabled = true;
                    }
                }
                else
                {

                    if (tseFuncao != null)
                    {
                        tseFuncao.Mode = Techne.Controls.ControlMode.View;
                    }
                    if (dataNomeacao != null)
                    {
                        dataNomeacao.ClientEnabled = false;
                    }
                }
                if (cmbMatricula != null)
                {
                    cmbMatricula.ClientEnabled = false;
                }
                if (chkReadaptado != null)
                {
                    chkReadaptado.Visible = true;
                }
            }
            else
            {
                if (chkReadaptado != null)
                {
                    chkReadaptado.Visible = false;
                }
            }
        }

        #endregion

        #region ODS da grid
        public void Delete(object PESSOA, object MATRICULA, object ORDEM) { }

        public void Insert(object MATRICULA, object DATA_NOMEACAO, object DATA_NOMEACAO_DO, object DATA_DESATIVACAO, object DATA_DESATIVACAO_DO, object ATO_OFICIAL, object READAPTADO, object RESP_DOCUMENTACAO, object MOTIVO, object DTINI, object DTFIM, object FUNCAO, object SETOR) { }

        public void Insert(object DATA_NOMEACAO_DO, object DATA_DESATIVACAO, object DATA_DESATIVACAO_DO, object ATO_OFICIAL, object RESP_DOCUMENTACAO, object MOTIVO, object DTINI, object DTFIM, object MATRICULA, object FUNCAO, object DATA_NOMEACAO, object READAPTADO) { }

        public void Update(object DATA_NOMEACAO, object DATA_NOMEACAO_DO, object DATA_DESATIVACAO, object DATA_DESATIVACAO_DO, object ATO_OFICIAL, object READAPTADO, object RESP_DOCUMENTACAO, object FUNCAO, object SETOR, object PESSOA, object MATRICULA, object ORDEM) { }

        public void Update(object DATA_NOMEACAO, object DATA_NOMEACAO_DO, object DATA_DESATIVACAO, object DATA_DESATIVACAO_DO, object ATO_OFICIAL, object READAPTADO, object RESP_DOCUMENTACAO, object MOTIVO, object DTINI, object DTFIM, object FUNCAO, object SETOR, object PESSOA, object MATRICULA, object ORDEM) { }
        public void Update(object DATA_NOMEACAO_DO, object DATA_DESATIVACAO, object DATA_DESATIVACAO_DO, object ATO_OFICIAL, object RESP_DOCUMENTACAO, object MOTIVO, object DTINI, object DTFIM, object MATRICULA, object FUNCAO, object DATA_NOMEACAO, object READAPTADO, object PESSOA, object ORDEM) { }

        public object Listar(string pessoa)
        {
            RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();

            if (!string.IsNullOrEmpty(pessoa))
            {
                return rnLotacao.ListaLotacaoDocentePor(Convert.ToInt32(pessoa));
            }

            return null;
        }

        public object ListaMatricula(string pessoa)
        {
            RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();

            if (!string.IsNullOrEmpty(pessoa))
            {
                return rnDocentes.ObtemMatriculaIdVinculoPor(Convert.ToInt32(pessoa));
            }

            return null;
        }


        public void DeleteLic(object num_func, object dtini) { }
        public void InsertLic(object IDFUNCIONAL, object VINCULO, object MATRICULA, object MOTIVO, object DTINI, object DTFIM) { }
        public void UpdateLic(object IDFUNCIONAL, object VINCULO, object matricula, object motivo, object dtini, object dtfim, object num_func) { }
        public void UpdateLic(object dtfim, object num_func, object dtini) { }

        public object ListarLic(string pessoa)
        {
            RN.LicencaDocente rnLicencaDocente = new LicencaDocente();
            if (!string.IsNullOrEmpty(pessoa))
            {
                return rnLicencaDocente.ListaPor(Convert.ToDecimal(pessoa));
            }
            return null;
        }


        #endregion

        #region Eventos Grid Licença

        protected void grdLicencas_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                RN.LicencaDocente rnLicencaDocente = new LicencaDocente();
                RN.Entidades.LyLicencaDocente licenca = new LyLicencaDocente();
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
                int numFunc = -1;
                bool possuiDataFim = true;
                string[] dados = null;

                if (e.NewValues["MATRICULA"] != null)
                {
                    dados = e.NewValues["MATRICULA"].ToString().Split(';');
                }

                if (e.NewValues["MATRICULA"] != null)
                {
                    numFunc = rnDocentes.ObtemNumFuncPor(Convert.ToString(dados[0]));
                }

                licenca.NumFunc = numFunc;
                licenca.Motivo = e.NewValues["MOTIVO"] != null ? Convert.ToString(e.NewValues["MOTIVO"]) : null;
                licenca.Dtini = e.NewValues["DTINI"] != null && e.NewValues["DTINI"] != string.Empty ? Convert.ToDateTime(e.NewValues["DTINI"]) : DateTime.MinValue;
                licenca.Dtfim = e.NewValues["DTFIM"] != null && e.NewValues["DTFIM"] != string.Empty ? Convert.ToDateTime(e.NewValues["DTFIM"]) : (DateTime?)null;

                validacao = rnLicencaDocente.ValidaInsercao(licenca, dados[0], Convert.ToDecimal(tseDocente["pessoa"].ToString()), out possuiDataFim, User.Identity.Name);


                if (validacao.Valido)
                {
                    rnLicencaDocente.Insere(licenca, dados[0], possuiDataFim, User.Identity.Name);
                    grdLicencas.DataBind();
                    grdLotacao.DataBind();
                    tseDocente.Enabled = true;
                    lblMensagem.Text = string.Empty;
                    lblMensagemLicenca.Text = string.Empty;
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemLicenca.Text = ex.Message;
            }
        }

        protected void grdLicencas_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                RN.LicencaDocente rnLicencaDocente = new LicencaDocente();
                RN.Entidades.LyLicencaDocente licenca = new LyLicencaDocente();
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
                bool possuiDataFim = true;

                string[] dados = null;

                if (e.NewValues["MATRICULA"] != null)
                {
                    dados = e.NewValues["MATRICULA"].ToString().Split(';');
                }

                licenca.NumFunc = Convert.ToDecimal(e.Keys["NUM_FUNC"].ToString());
                licenca.Motivo = e.NewValues["MOTIVO"] != null ? Convert.ToString(e.NewValues["MOTIVO"]) : null;
                licenca.Dtini = Convert.ToDateTime(e.Keys["DTINI"]);
                licenca.Dtfim = e.NewValues["DTFIM"] != null && e.NewValues["DTFIM"] != string.Empty ? Convert.ToDateTime(e.NewValues["DTFIM"]) : (DateTime?)null;

                validacao = rnLicencaDocente.ValidaAlteracao(licenca, dados[0], Convert.ToDecimal(tseDocente["pessoa"].ToString()), out possuiDataFim, User.Identity.Name);


                if (validacao.Valido)
                {
                    rnLicencaDocente.Altera(licenca);
                    grdLicencas.DataBind();
                    grdLotacao.DataBind();
                    tseDocente.Enabled = true;
                    lblMensagem.Text = string.Empty;
                    lblMensagemLicenca.Text = string.Empty;
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemLicenca.Text = ex.Message;
            }
        }

        protected void grdLicencas_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                RN.LicencaDocente rnLicencaDocente = new LicencaDocente();
                RN.Entidades.LyLicencaDocente licenca = new LyLicencaDocente();
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();


                validacao = rnLicencaDocente.ValidaRemocao(Convert.ToDecimal(e.Keys["NUM_FUNC"]), Convert.ToDateTime(e.Keys["DTINI"]), Convert.ToString(e.Values["MOTIVO"]), User.Identity.Name);

                if (validacao.Valido)
                {
                    rnLicencaDocente.Remove(Convert.ToDecimal(e.Keys["NUM_FUNC"]), Convert.ToDateTime(e.Keys["DTINI"]), Convert.ToString(e.Values["MOTIVO"]));
                    grdLicencas.DataBind();
                    grdLotacao.DataBind();
                    tseDocente.Enabled = true;
                    lblMensagem.Text = string.Empty;
                    lblMensagemLicenca.Text = string.Empty;
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemLicenca.Text = ex.Message;
            }
        }
        protected void grdLicencas_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            tseDocente.Enabled = true;
        }

        protected void grdLicencas_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdLicencas.IsNewRowEditing)
            {
                tseDocente.Enabled = false;

                if ((e.Column.FieldName) == "IDFUNCIONAL")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                    e.Editor.Value = tseDocente["idfuncional"].ToString();
                }
                else if ((e.Column.FieldName) == "VINCULO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "MATRICULA")
                {
                    e.Editor.ReadOnly = false;
                }
                else if ((e.Column.FieldName) == "DTINI")
                {
                    e.Editor.ReadOnly = false;
                }
                else if ((e.Column.FieldName) == "MOTIVO")
                {
                    e.Editor.ReadOnly = false;
                }
            }
            else if (grdLicencas.IsEditing)
            {
                tseDocente.Enabled = false;
                if ((e.Column.FieldName) == "IDFUNCIONAL")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "VINCULO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "MATRICULA")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "DTINI")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                else if ((e.Column.FieldName) == "MOTIVO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }

                else if ((e.Column.FieldName) == "DTFIM")
                {
                    string motivo = (string)grdLicencas.GetRowValues(e.VisibleIndex, "MOTIVO");
                    if (RN.Licencas.PossuiDataFim(motivo))
                    {
                        e.Editor.Enabled = true;
                    }
                    else
                    {
                        e.Editor.Enabled = false;
                    }
                }
            }
        }

        protected void dtIni_Load(object sender, EventArgs e)
        {
            ASPxTextBox txtDataInicio = sender as ASPxTextBox;

            GridViewEditItemTemplateContainer cont = txtDataInicio.NamingContainer as GridViewEditItemTemplateContainer;
            if (cont == null || string.IsNullOrEmpty(cont.Text.Replace("&nbsp;", string.Empty)))
            {
                txtDataInicio.Enabled = true;
            }
            else
            {
                txtDataInicio.Enabled = false;
            }
        }

        protected void dtFim_Load(object sender, EventArgs e)
        {
            ASPxTextBox txtDataFim = sender as ASPxTextBox;

            GridViewEditItemTemplateContainer cont = txtDataFim.NamingContainer as GridViewEditItemTemplateContainer;
            if (string.IsNullOrEmpty(cont.Text.Replace("&nbsp;", string.Empty)))
            {
                txtDataFim.Enabled = false;
            }
            else
            {
                txtDataFim.Enabled = true;
            }
        }

        protected void grdLicencas_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            // e.NewValues["NUM_FUNC"] = tseDocente["num_func"].ToString();
            grdLicencas.Settings.ShowFilterRow = false;
        }

        protected void grdLicencas_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdLicencas.Settings.ShowFilterRow = false;
        }

        protected void grdLicencas_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdLotacao);
            ControlaAcesso(grdLicencas);
        }
        protected int retornaIndiceLinha(string numFunc, string dtInicio)
        {
            int indiceLinha = -1;

            for (var rowIndex = 0; rowIndex < this.grdLicencas.VisibleRowCount; rowIndex++)
            {
                var numFuncRow = this.grdLicencas.GetRowValues(rowIndex, "NUM_FUNC").ToString();
                var dtInicioRow = this.grdLicencas.GetRowValues(rowIndex, "DTINI").ToString();
                if (numFuncRow == numFunc && dtInicioRow == dtInicio)
                {
                    indiceLinha = rowIndex;
                    break;
                }
            }

            return indiceLinha;
        }


        #endregion


    }
}
