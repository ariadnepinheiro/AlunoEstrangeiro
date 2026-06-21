using System;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.RN;
using System.Web;
using Techne.Lyceum.CR;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/LotacaoServidor.aspx"),
    ControlText("Lotação Funcionário"),
    Title("Lotação Funcionário"),]

    public partial class LotacaoServidor : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdLotacao, "Lotações do Funcionário");
            TituloGrid(grdLicencas, "Situações do Funcionário");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.PadroesDeAcessos rnPadrao = new PadroesDeAcessos();

                lblMensagem.Text = string.Empty;
                lblMensagemLicenca.Text = string.Empty;
                //Resposta para requisição que retorna POSSUI_DTFIM da situação.

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

        protected void tseServidor_Changed(object sender, EventArgs args)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (tseServidor.IsValidDBValue && !tseServidor.DBValue.IsNull)
                {
                    grdLotacao.Visible = true;
                    grdLicencas.Visible = true;
                    txtPessoaHidden.Text = tseServidor["pessoa"].ToString();
                    odsMatricula.DataBind();
                    
                    

                }
                else if (!tseServidor.DBValue.IsNull)
                {
                    grdLotacao.Visible = false;
                    grdLicencas.Visible = false;
                    lblMensagem.Text = "Funcionário não cadastrado.";
                    txtPessoaHidden.Text = string.Empty;
                }
                else
                {
                    grdLotacao.Visible = false;
                    grdLicencas.Visible = false;
                    lblMensagem.Text = "Favor consultar um funcionário.";
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

            e.NewValues["ordem"] = rnLotacao.ObtemProximaOrdemPor(tseServidor["matricula"].ToString());
            e.NewValues["pessoa"] = Convert.ToDecimal(tseServidor["pessoa"].ToString());
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
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Entidades.LyLotacao lotacao = new LyLotacao();
                RN.Entidades.LyLicencaPessoa licenca = new LyLicencaPessoa();
                RN.Lotacao rnLotacao = new Lotacao();
                RN.Entidades.LyLotacao lotacaoAnterior = new LyLotacao();
                RN.VinculoLy rnVinculoLy = new VinculoLy();
                bool possuiDataFim = true;

                TSearchBox tseSetor = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseSetor");

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
                lotacao.Pessoa = Convert.ToDecimal(tseServidor["pessoa"].ToString());
                lotacao.Readaptado = null;
                lotacao.RespDocumentacao = e.NewValues["RESP_DOCUMENTACAO"] != null ? Convert.ToString(e.NewValues["RESP_DOCUMENTACAO"]) : null;
                lotacao.Setor = !tseSetor.DBValue.IsNull && tseSetor.IsValidDBValue ? tseSetor["setor"].ToString() : null;
                lotacao.Categoria = rnVinculoLy.ObtemCategoriaLotacaoPor(Convert.ToString(e.NewValues["MATRICULA"])); 
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

                if (e.NewValues["MOTIVO"] != null)
                {
                    licenca.Pessoa = Convert.ToDecimal(tseServidor["pessoa"].ToString());
                    licenca.Motivo = Convert.ToString(e.NewValues["MOTIVO"]);
                    licenca.Dtini = e.NewValues["DTINI"] != null ? Convert.ToDateTime(e.NewValues["DTINI"]) : DateTime.MinValue;
                    licenca.Dtfim = e.NewValues["DTFIM"] != null ? Convert.ToDateTime(e.NewValues["DTFIM"]) : (DateTime?)null;
                }

                if (!string.IsNullOrEmpty(licenca.Motivo))
                {
                    possuiDataFim = RN.Licencas.PossuiDataFim(licenca.Motivo);
                    Session["possuiDataFimLotacao"] = possuiDataFim;
                }

                
                if (rnLotacao.PossuiLotacaoAtivaPor(lotacao.Matricula))
                {
                    //Alimenta linha
                    HttpContext.Current.Session["lotacao"] = new RN.Entidades.LyLotacao();
                    HttpContext.Current.Session["lotacao"] = lotacao;

                    //Alimenta linhalic
                    HttpContext.Current.Session["licencaPessoa"] = new RN.Entidades.LyLicencaPessoa();
                    HttpContext.Current.Session["licencaPessoa"] = licenca;

                    //Se existe chama pop-up
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                    e.Cancel = true;
                    return;
                }

                validacao = rnLotacao.ValidaInsercaoLotacaoFuncionario(lotacao, licenca,false,out lotacaoAnterior, possuiDataFim);

                if (validacao.Valido)
                {
                    rnLotacao.InsereLotacaoFuncionario(lotacao, licenca,false,lotacaoAnterior, possuiDataFim);
                    grdLotacao.DataBind();
                    grdLicencas.DataBind();
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

        protected void grdLotacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();
                RN.VinculoLy rnVinculoLy = new VinculoLy();
                RN.Entidades.LyLotacao lotacao = new LyLotacao();
                RN.Entidades.LyLotacao proximaLotacao = new LyLotacao();

                TSearchBox tseSetor = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseSetor");

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
                lotacao.Pessoa = Convert.ToDecimal(tseServidor["pessoa"].ToString());
                lotacao.RespDocumentacao = e.NewValues["RESP_DOCUMENTACAO"] != null ? Convert.ToString(e.NewValues["RESP_DOCUMENTACAO"]) : null;
                lotacao.Setor = !tseSetor.DBValue.IsNull && tseSetor.IsValidDBValue ? tseSetor["setor"].ToString() : null;
                lotacao.Categoria = rnVinculoLy.ObtemCategoriaPor(Convert.ToString(e.Keys["MATRICULA"])); 
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

                validacao = rnLotacao.ValidaAlteracaoLotacaoFuncionario(lotacao, out proximaLotacao);

                if (validacao.Valido)
                {
                    rnLotacao.AlteraLotacaoFuncionario(lotacao, proximaLotacao);
                    lblMensagem.Text = string.Empty;
                    grdLotacao.DataBind();
                    grdLicencas.DataBind();
                    tseServidor.Enabled = true;
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
                    tseServidor.Enabled = true;
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

        protected void grdLotacao_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            tseServidor.Enabled = true;
        }

        protected void grdLotacao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdLotacao.IsNewRowEditing)
            {
                tseServidor.Enabled = false;
                if ((e.Column.FieldName) == "MATRICULA")
                {
                    e.Editor.ReadOnly = false;
                }
            }

            else if (grdLotacao.IsEditing)
            {
                tseServidor.Enabled = false;
                if ((e.Column.FieldName) == "MATRICULA")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                }
                if ((e.Column.FieldName) == "FUNCAO")
                {
                    e.Editor.ReadOnly = true;
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

        protected void grdLotacao_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdLotacao);
            ControlaAcesso(grdLicencas);
        }
        protected void cmbMatricula_SelectedIndexChanged(object sender, EventArgs e)
        {
            RN.Lotacao rnLotacao = new Lotacao();
            RN.VinculoLy rnVinculo = new VinculoLy();
            ASPxDateEdit dataNomeacao = (ASPxDateEdit)grdLotacao.FindEditFormTemplateControl("DATA_NOMEACAO");
            ASPxComboBox cmbMatricula = (ASPxComboBox)grdLotacao.FindEditFormTemplateControl("cmbMatricula");
            DateTime? novaDataNomeacao = null;

            //Busca pessoa da matricula selecionada
            decimal pessoa = rnVinculo.ObtemPessoaPor(cmbMatricula.Value.ToString());
            if (pessoa != Convert.ToDecimal(tseServidor["pessoa"].ToString()))
            {
                grdLotacao.CancelEdit();
                tseServidor_Changed(null, null);
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

        //public void Insert(object MATRICULA, object DATA_NOMEACAO, object DATA_NOMEACAO_DO, object DATA_DESATIVACAO, object DATA_DESATIVACAO_DO, object ATO_OFICIAL, object READAPTADO, object RESP_DOCUMENTACAO, object MOTIVO, object DTINI, object DTFIM, object FUNCAO, object SETOR) { }
        public void Insert(object DATA_NOMEACAO_DO, object DATA_DESATIVACAO, object DATA_DESATIVACAO_DO, object ATO_OFICIAL, object RESP_DOCUMENTACAO, object MOTIVO, object DTINI, object DTFIM, object MATRICULA, object FUNCAO, object UA_ATUAL, object DATA_NOMEACAO, object READAPTADO) { }
        
        public void Update(object DATA_NOMEACAO, object DATA_NOMEACAO_DO, object DATA_DESATIVACAO, object DATA_DESATIVACAO_DO, object ATO_OFICIAL, object READAPTADO, object RESP_DOCUMENTACAO, object FUNCAO, object SETOR, object PESSOA, object MATRICULA, object ORDEM) { }

       // public void Update(object DATA_NOMEACAO, object DATA_NOMEACAO_DO, object DATA_DESATIVACAO, object DATA_DESATIVACAO_DO, object ATO_OFICIAL, object READAPTADO, object RESP_DOCUMENTACAO, object MOTIVO, object DTINI, object DTFIM, object FUNCAO, object SETOR, object PESSOA, object MATRICULA, object ORDEM) { }

        public void Update(object DATA_NOMEACAO_DO, object DATA_DESATIVACAO, object DATA_DESATIVACAO_DO, object ATO_OFICIAL, object RESP_DOCUMENTACAO, object MOTIVO, object DTINI, object DTFIM, object MATRICULA, object FUNCAO, object UA_ATUAL, object DATA_NOMEACAO, object READAPTADO, object PESSOA, object ORDEM) { }

        public object Listar(string pessoa)
        {
            RN.Lotacao rnLotacao = new Lotacao();

            if (!string.IsNullOrEmpty(pessoa))
            {
                return rnLotacao.ListaLotacaoFuncionarioPor(Convert.ToInt32(pessoa));
            }

            return null;
        }

        public object ListaMatricula(string pessoa)
        {
            RN.VinculoLy rnVinculo = new Techne.Lyceum.RN.VinculoLy();

            if (!string.IsNullOrEmpty(pessoa))
            {
                return rnVinculo.ObtemMatriculaIdVinculoPor(Convert.ToInt32(pessoa));
            }

            return null;
        }

        public void DeleteLic(object matricula, object pessoa, object ordem, object dtini) { }

        public void DeleteLic(object pessoa, object ordem, object dtini) { }

        public void InsertLic(object IDFUNCIONAL, object VINCULO, object MATRICULA, object MOTIVO, object DTINI, object DTFIM) { }

        public void UpdateLic(object IDFUNCIONAL, object VINCULO, object matricula, object motivo, object dtini, object dtfim, object pessoa, object ordem) { }

        public object ListarLic(string pessoa)
        {
            RN.LicencaPessoa rnLicencaPessoa = new LicencaPessoa();

            if (!string.IsNullOrEmpty(pessoa))
            {
                return rnLicencaPessoa.ListaPor(Convert.ToDecimal(pessoa));
            }

            return null;
        }


        #endregion

        #region Eventos Grid Licença
        protected void grdLicencas_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                RN.LicencaPessoa rnLicencaPessoa = new LicencaPessoa();
                RN.Entidades.LyLicencaPessoa licenca = new LyLicencaPessoa();
                ValidacaoDados validacao = new ValidacaoDados();
                bool possuiDataFim = true;
                string matricula = string.Empty;

                string[] dados = null;

                if (e.NewValues["MATRICULA"] != null)
                {
                    dados = e.NewValues["MATRICULA"].ToString().Split(';');
                    matricula = dados[0];
                }                

                licenca.Pessoa = Convert.ToDecimal(tseServidor["pessoa"]);
                licenca.Motivo = e.NewValues["MOTIVO"] != null ? Convert.ToString(e.NewValues["MOTIVO"]) : null;
                licenca.Dtini = e.NewValues["DTINI"] != null ? Convert.ToDateTime(e.NewValues["DTINI"]) : DateTime.MinValue;
                licenca.Dtfim = e.NewValues["DTFIM"] != null && e.NewValues["DTFIM"] != string.Empty ? Convert.ToDateTime(e.NewValues["DTFIM"]) : (DateTime?)null;


                validacao = rnLicencaPessoa.ValidaInsercao(licenca, matricula, out possuiDataFim, User.Identity.Name);


                if (validacao.Valido)
                {
                    rnLicencaPessoa.Insere(licenca, matricula, possuiDataFim, User.Identity.Name);
                    grdLicencas.DataBind();
                    grdLotacao.DataBind();
                    tseServidor.Enabled = true;
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
                RN.LicencaPessoa rnLicencaPessoa = new LicencaPessoa();
                RN.Entidades.LyLicencaPessoa licenca = new LyLicencaPessoa();
                ValidacaoDados validacao = new ValidacaoDados();
                bool possuiDataFim = true;

                string[] dados = null;

                if (e.NewValues["MATRICULA"] != null)
                {
                    dados = e.NewValues["MATRICULA"].ToString().Split(';');                    
                }      

                licenca.Pessoa = Convert.ToDecimal(tseServidor["pessoa"]);
                licenca.Ordem = Convert.ToDecimal(e.Keys["ORDEM"]);
                licenca.Motivo = e.NewValues["MOTIVO"] != null ? Convert.ToString(e.NewValues["MOTIVO"]) : null;
                licenca.Dtini = Convert.ToDateTime(e.Keys["DTINI"]);
                licenca.Dtfim = e.NewValues["DTFIM"] != null && e.NewValues["DTFIM"] != string.Empty ? Convert.ToDateTime(e.NewValues["DTFIM"]) : (DateTime?)null;

                validacao = rnLicencaPessoa.ValidaAlteracao(licenca, dados[0], out possuiDataFim, User.Identity.Name);


                if (validacao.Valido)
                {
                    rnLicencaPessoa.Altera(licenca);
                    grdLicencas.DataBind();
                    grdLotacao.DataBind();
                    tseServidor.Enabled = true;
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
                RN.LicencaPessoa rnLicencaPessoa = new LicencaPessoa();
                RN.Entidades.LyLicencaPessoa licenca = new LyLicencaPessoa();
                ValidacaoDados validacao = new ValidacaoDados();

                string[] dados = null;

                if (e.Values["MATRICULA"] != null)
                {
                    dados = e.Values["MATRICULA"].ToString().Split(';');
                }     


                validacao = rnLicencaPessoa.ValidaRemocao(Convert.ToDecimal(tseServidor["pessoa"]), Convert.ToDecimal(e.Keys["ORDEM"]), Convert.ToDateTime(e.Keys["DTINI"]), Convert.ToString(e.Values["MOTIVO"]), User.Identity.Name);

                if (validacao.Valido)
                {
                    rnLicencaPessoa.RemoveMatriculapor(Convert.ToString(dados[0]), Convert.ToDecimal(tseServidor["pessoa"]), Convert.ToDecimal(e.Keys["ORDEM"]), Convert.ToDateTime(e.Keys["DTINI"]));
                    grdLicencas.DataBind();
                    grdLotacao.DataBind();
                    tseServidor.Enabled = true;
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
            tseServidor.Enabled = true;
        }
        protected void grdLicencas_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdLicencas.IsNewRowEditing)
            {
                tseServidor.Enabled = false;
                if ((e.Column.FieldName) == "IDFUNCIONAL")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.ClientEnabled = false;
                    e.Editor.Value = tseServidor["idfuncional"].ToString();
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
                tseServidor.Enabled = false;
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
        #endregion



        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Entidades.LyLotacao lotacao = new LyLotacao();
                RN.Entidades.LyLotacao lotacaoAnterior = new LyLotacao();
                RN.Entidades.LyLicencaPessoa licencaPessoa = new LyLicencaPessoa();
                lblMensagem.Text = string.Empty;
                RN.Lotacao rnLotacao = new Lotacao();
                bool possuiDataFim = Session["possuiDataFimLotacao"] != null ? Convert.ToBoolean(Session["possuiDataFimLotacao"]) : true;

                string funcaoAtual = string.Empty;

                this.pucConfirmar.ShowOnPageLoad = false;

                lotacao = (RN.Entidades.LyLotacao)HttpContext.Current.Session["lotacao"];
                licencaPessoa = (RN.Entidades.LyLicencaPessoa)HttpContext.Current.Session["licencaPessoa"];

                validacao = rnLotacao.ValidaInsercaoLotacaoFuncionario(lotacao, licencaPessoa, true ,out lotacaoAnterior, possuiDataFim);

                if (validacao.Valido)
                {
                    rnLotacao.InsereLotacaoFuncionario(lotacao, licencaPessoa, true, lotacaoAnterior, possuiDataFim);
                    grdLotacao.DataBind();
                    grdLicencas.DataBind();
                    grdLotacao.CancelEdit();
                }
                else
                {
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
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

    }
}