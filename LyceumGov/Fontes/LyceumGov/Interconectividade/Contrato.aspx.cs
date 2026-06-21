using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using System.Data;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.Interconectividade
{
    [NavUrl("~/Interconectividade/Contrato.aspx")]
    [ControlText("Contrato")]
    [Title("Contrato")]


    public partial class Contrato : TPage
    {
        public object Lista(object setor)
        {
            RN.FiscalizacaoLink.Contrato rnContrato = new Techne.Lyceum.RN.FiscalizacaoLink.Contrato();

            if (!string.IsNullOrEmpty(setor.ToString()))
            {
                return rnContrato.ListaContratoUnidadePor(setor.ToString());
            }
            return null;
        }

        public object ListaCircuito(object contrato)
        {
            RN.FiscalizacaoLink.CircuitoSetor rnCircuitoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.CircuitoSetor();

            if (contrato != null)
            {
                return rnCircuitoSetor.ListaPor(Convert.ToInt32(contrato));
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dtContratacao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                   // pcContrato.TabPages[1].Enabled = false;
                    pnlContrato.Visible = true;

                    pnlDadosContrato.Visible = false;
                    LimparTela();
                    btnCancel.Visible = false;
                    btnNovo.Visible = false;

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                        tseUnidadeAdministrativa.DBValue = ObterDadosQueryString(decodedText);
                        tseUnidadeAdministrativa_Changed(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            
            TituloGrid(grdContrato, string.Empty);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {

            btnSalvar.Visible = true;
            ControlaAcesso(btnSalvar, AcaoControle.novo);
         
            ControlaAcesso(grdContrato);

            if (grdContrato.VisibleRowCount > 0)
            {
                //pcContrato.TabPages[1].Enabled = true;
            }

            if (!btnNovo.Visible && !this.tseUnidadeAdministrativa.DBValue.IsNull)
            {
               // pnlDadosContrato.Visible = true;
            }
        }


        protected void tseUnidadeAdministrativa_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
               // pnlDadosContrato.Visible = false;          
                LimparTela();
                btnCancel.Visible = false;
                btnNovo.Visible = false;
                //pcContrato.TabPages[1].Enabled = false;

                if (!this.tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    if (!this.tseUnidadeAdministrativa.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                    }
                    else
                    {                       
                        btnCancel.Visible = true;
                        btnNovo.Visible = true;
                        pnlContrato.Visible = true;                       
                        CarregaOperadora();
                        CarregaTipoLink();
                        tab.Visible = true;
                        tab.ActiveTabIndex = 0;
                        pntabDadosGerais.Visible = true;
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade administrativa.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaOperadora()
        {
            RN.FiscalizacaoLink.Operadora rnOperadora = new Techne.Lyceum.RN.FiscalizacaoLink.Operadora();
            ListEditItem item = new ListEditItem("Selecione", string.Empty);

            ddlOperadora.Items.Clear();
            ddlOperadora.DataSource = rnOperadora.ListaOperadoraAtiva();
            ddlOperadora.DataBind();
            ddlOperadora.Items.Insert(0, item);
        }

        private void CarregaTipoLink()
        {
            RN.FiscalizacaoLink.TipoLink rnTipoLink = new Techne.Lyceum.RN.FiscalizacaoLink.TipoLink();
            ListEditItem item = new ListEditItem("Selecione", string.Empty);

            ddlTipoLink.Items.Clear();
            ddlTipoLink.DataSource = rnTipoLink.ListaAtivo();
            ddlTipoLink.DataBind();
            ddlTipoLink.Items.Insert(0, item);
        }

        private void LimparTela()
        {
           
            txtNumero.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            dtImplantacao.Text = string.Empty;
            dtContratacao.Text = string.Empty;
            dtTermino.Text = string.Empty;
            ddlOperadora.Items.Clear();
            ddlTipoLink.Items.Clear();
        }

        protected void pcContrato_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;            

            string queryString = "setor=" + tseUnidadeAdministrativa.DBValue;

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

            if (Convert.ToInt16(e.Tab.Index) == 1)
            {
                Response.Redirect("Circuito.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                tseUnidadeAdministrativa.ResetValue();
                tab.Visible = false;                
                LimparTela();                
                grdContrato.DataBind();                
                btnCancel.Visible = false;
                btnNovo.Visible = false;
                pntabDadosGerais.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                LimparTela();
                pnlDadosContrato.Visible = true;
                CarregaOperadora();
                CarregaTipoLink();
                btnNovo.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.DTOs.DadosContratoEscola dados = new Techne.Lyceum.RN.DTOs.DadosContratoEscola();
                RN.FiscalizacaoLink.Contrato rnContrato = new Techne.Lyceum.RN.FiscalizacaoLink.Contrato();

                dados.ContratoSetorId = !hdnContratoSetorId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnContratoSetorId.Value) : -1;
                dados.DataImplantacao = !dtImplantacao.Text.IsNullOrEmptyOrWhiteSpace() ? dtImplantacao.Date : (DateTime?)null;
                dados.DataContratacao = !dtContratacao.Text.IsNullOrEmptyOrWhiteSpace() ? dtContratacao.Date : (DateTime?)null;
                dados.DataTermino = !dtTermino.Text.IsNullOrEmptyOrWhiteSpace() ? dtTermino.Date : (DateTime?)null;
                dados.Descricao = !txtDescricao.Text.IsNullOrEmptyOrWhiteSpace() ? txtDescricao.Text.Trim() : null;
                dados.OperadoraId = ddlOperadora.Text == "Selecione" ? -1 : Convert.ToInt32(ddlOperadora.Value);
                dados.TipoLinkId = ddlTipoLink.Text == "Selecione" ? -1 : Convert.ToInt32(ddlTipoLink.Value);
                dados.NumeroContrato = !txtNumero.Text.IsNullOrEmptyOrWhiteSpace() ? txtNumero.Text.Trim() : null;
                dados.UsuarioId = User.Identity.Name;
                dados.UnidadeAdministrativa = (!tseUnidadeAdministrativa.DBValue.IsNull && tseUnidadeAdministrativa.IsValidDBValue) ? tseUnidadeAdministrativa.DBValue.ToString() : null;
                

                validacao = rnContrato.ValidaContratoUnidade(dados, dados.ContratoSetorId == -1 ? true : false);

                if (validacao.Valido)
                {
                    rnContrato.InsereContratoUnidade(dados);
                   
                    pnlDadosContrato.Visible = false;
                    LimparTela();
                    btnNovo.Visible = true;
                    lblMensagem.Text = "Contrato atualizado com sucesso.";
                    grdContrato.DataBind();

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                //pnlDadosContrato.Visible = true;
            }
        }

        public void Update(object TIPOLINKID, object NUMERO, object DESCRICAO, object OPERADORAID, object DATACONTRATACAO, object DATAIMPLANTACAO, object DATATERMINO, object CONTRATOID, object CONTRATOSETORID, object CONTRATOOPERADORAID) { }
        
        
        protected void grdContrato_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdContrato);
        }

        protected void grdContrato_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdContrato.Settings.ShowFilterRow = false;
        }

        protected void grdContrato_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdContrato.Settings.ShowFilterRow = false;
        }

        protected void grdContrato_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (this.grdContrato.IsEditing)
            {
                if (e.Column.FieldName == "CNPJOPERADORA")
                {
                    e.Editor.Enabled = false;
                    e.Editor.Value = string.Empty;
                }
            }
        }

        protected void grdContrato_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            string numero = grdContrato.GetRowValues(e.VisibleIndex, "NUMERO").ToString();
            string descricao = grdContrato.GetRowValues(e.VisibleIndex, "DESCRICAO").ToString();
            string dataContratacao = grdContrato.GetRowValues(e.VisibleIndex, "DATACONTRATACAO").ToString();
            string dataImplantacao = grdContrato.GetRowValues(e.VisibleIndex, "DATAIMPLANTACAO").ToString();
            string dataTermino = grdContrato.GetRowValues(e.VisibleIndex, "DATATERMINO").ToString();
            string operadoraId = grdContrato.GetRowValues(e.VisibleIndex, "OPERADORAID").ToString();
            string tipoLinkId = grdContrato.GetRowValues(e.VisibleIndex, "TIPOLINKID").ToString();
            string contratoId = grdContrato.GetRowValues(e.VisibleIndex, "CONTRATOID").ToString();
            string contratoSetorId = grdContrato.GetRowValues(e.VisibleIndex, "CONTRATOSETORID").ToString();
            string contratoOperadoraId = grdContrato.GetRowValues(e.VisibleIndex, "CONTRATOOPERADORAID").ToString();

            if (e.ButtonID == "btnEditar")
            {
                txtNumero.Text = numero;
                txtDescricao.Text = descricao;
                if (!dataContratacao.IsNullOrEmptyOrWhiteSpace())
                {
                    dtContratacao.Date = Convert.ToDateTime(dataContratacao);
                }
                if (!dataImplantacao.IsNullOrEmptyOrWhiteSpace())
                {
                    dtImplantacao.Date = Convert.ToDateTime(dataImplantacao);
                }
                if (!dataTermino.IsNullOrEmptyOrWhiteSpace())
                {
                    dtTermino.Date = Convert.ToDateTime(dataTermino);
                }
                ddlOperadora.Value = !operadoraId.IsNullOrEmptyOrWhiteSpace() ? operadoraId.ToString() : string.Empty;
                ddlTipoLink.Value = !tipoLinkId.IsNullOrEmptyOrWhiteSpace() ? tipoLinkId.ToString() : string.Empty;
                pnlDadosContrato.Visible = true;
                
            }
            if (e.ButtonID == "btnDeletar")
            {

            }
        }

        protected void grdContrato_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "compositekey")
            {
                string contrato = Convert.ToString(e.GetListSourceFieldValue("CONTRATOID"));
                string setor = Convert.ToString(e.GetListSourceFieldValue("CONTRATOSETORID"));
                string operadora = Convert.ToString(e.GetListSourceFieldValue("CONTRATOOPERADORAID"));
                e.Value = contrato + "-" + setor + "-" + operadora;
            }

        }

        protected void grdContrato_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.DTOs.DadosContratoEscola dados = new Techne.Lyceum.RN.DTOs.DadosContratoEscola();
            RN.FiscalizacaoLink.Contrato rnContrato = new Techne.Lyceum.RN.FiscalizacaoLink.Contrato();

            string[] chaves = e.Keys["compositekey"].ToString().Split('-');
            e.Keys.Clear();
            e.Keys.Add("CONTRATOID", chaves[0]);
            e.Keys.Add("CONTRATOSETORID", chaves[1]);
            e.Keys.Add("CONTRATOOPERADORAID", chaves[2]);

            var contratoid = chaves[0];
            var contratosetorid = chaves[1];
            var contratooperadoraid = chaves[2];

            dados.ContratoSetorId = Convert.ToInt32(contratosetorid);
            dados.ContratoId = Convert.ToInt32(contratoid);
            dados.ContratoOperadoraId = Convert.ToInt32(contratooperadoraid);
            dados.DataImplantacao = e.NewValues["DATAIMPLANTACAO"] != null ? Convert.ToDateTime(e.NewValues["DATAIMPLANTACAO"]) : (DateTime?)null;
            dados.DataContratacao = e.NewValues["DATACONTRATACAO"] != null ? Convert.ToDateTime(e.NewValues["DATACONTRATACAO"]) : (DateTime?)null;
            dados.TipoLinkId = e.NewValues["TIPOLINKID"] != null ? Convert.ToInt32(e.NewValues["TIPOLINKID"]) : -1;
            dados.DataTermino = e.NewValues["DATATERMINO"] != null ? Convert.ToDateTime(e.NewValues["DATATERMINO"]) : (DateTime?)null;
            dados.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString() : null;
            dados.OperadoraId = e.NewValues["OPERADORAID"] != null ? Convert.ToInt32(e.NewValues["OPERADORAID"]) : -1;
            dados.NumeroContrato = e.NewValues["NUMERO"] != null ? e.NewValues["NUMERO"].ToString() : null;
            dados.UsuarioId = User.Identity.Name;
            dados.UnidadeAdministrativa = (!tseUnidadeAdministrativa.DBValue.IsNull && tseUnidadeAdministrativa.IsValidDBValue) ? tseUnidadeAdministrativa.DBValue.ToString() : null;


            validacao = rnContrato.ValidaContratoUnidade(dados, false);

            if (validacao.Valido)
            {
                rnContrato.AtualizaContratoUnidade(dados);              
                lblMensagem.Text = "Contrato atualizado com sucesso.";
                grdContrato.DataBind();

            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }           
           
        }
       
      
        private string ObterDadosQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');
            string setor = null;
          

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("setor=") >= 0)
                    setor = dados.Substring(dados.LastIndexOf('=') + 1);
               
            }          

            return setor;
        }

        protected void tab_TabClick(object source, DevExpress.Web.ASPxTabControl.TabControlCancelEventArgs e)
        {
            try
            {
                pntabDadosGerais.Visible = false;
                pntabCircuito.Visible = false;
                              

                if (e.Tab.Name == "tabDadosGerais")
                {
                    pntabDadosGerais.Visible = true;
                  
                }
                else if (e.Tab.Name == "tabCircuito")
                {
                    pntabCircuito.Visible = true;

                    string queryString = "setor=" + tseUnidadeAdministrativa.DBValue;

                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
                    
                    Response.Redirect("Circuito.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                    
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
