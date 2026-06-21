using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Data;
using Seeduc.Infra.Helpers;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Lyceum.RN.PrestacaoContas.DTOs;
using System.Globalization;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
         NavUrl("~/PrestacaoContas/AprovarProgramacaoOrcamentaria.aspx"),
         ControlText("AprovarProgramacaoOrcamentaria"),
         Title("Aprovar Programação Orçamentária")
    ]
    public partial class AprovarProgramacaoOrcamentaria : TPage
    {
        public object ListaDadosGridPor(int ano, String processo)
        {
            DataTable dados = new DataTable();
            RN.PrestacaoContas.AnalisePlanilhaOrcamentaria rnAnalisePlanilhaOrcamentaria = new RN.PrestacaoContas.AnalisePlanilhaOrcamentaria();

            if (!String.IsNullOrEmpty(processo))
            {
                dados = rnAnalisePlanilhaOrcamentaria.ListaDadosGridPor(ano, processo);

            }
            else if (String.IsNullOrEmpty(processo) && ano > 0)
            {
                dados = rnAnalisePlanilhaOrcamentaria.ListaDadosGridPorAno(ano);
            }            

            return dados;
        }

        public object ListaDadosGridPopupPor(object planilhaOrcamentariaId)
        {
            RN.PrestacaoContas.ItemPlanilhaOrcamentaria rnItemPlanilhaOrcamentaria = new RN.PrestacaoContas.ItemPlanilhaOrcamentaria();

            if (planilhaOrcamentariaId == null)
            {
                return null;
            }           

            var dados = rnItemPlanilhaOrcamentaria.ListaDadosGridAprovarProOrcPor(Convert.ToInt32(planilhaOrcamentariaId));
            return dados;
        }

        protected void ddlAnoCadastro_SelectedIndexChanged(object sender, EventArgs e)
        {            
            RN.PrestacaoContas.PlanilhaOrcamentaria rnPlanilhaOrcamentaria = new RN.PrestacaoContas.PlanilhaOrcamentaria();
            if (!string.IsNullOrEmpty(ddlAnoCadastro.SelectedValue))
            {
                tseNumProcesso.DBValue = DBNull.Value;
                hdnNumProcesso.Value = null;
                tseNumProcesso.SqlWhere = "ano = " + ddlAnoCadastro.SelectedValue;
                tseNumProcesso.DataBind();                
                //  dpdNumProcesso.DataBind();
                //   dpdNumProcesso.Items.Insert(0, new ListItem("Selecione", string.Empty));
            }
        }

        protected void tseNumProcesso_Changed(object sender, EventArgs e)
        {
            try
            {
                if (!tseNumProcesso.DBValue.IsNull)
                {
                    if (tseNumProcesso.IsValidDBValue)
                    {
                        hdnNumProcesso.Value = tseNumProcesso.DBValue.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    CarregaAnoCadastro();
                    pnlObrigacoesFiscais.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdPlanilhaOrcamentaria);
        }       

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPlanilhaOrcamentaria, "");
            TituloGrid(grdItemPlanilha, "");
        }

        public void Update(object ANO, object PROCESSO, object DESCRICAO, object PT, object PROGRAMATRABALHO, object PLANOTRABALHOID, object NATUREZADESPESAID, object REGIAOFINANCEIRAID, object VALORTOTAL, object ACAO, object MOTIVOREPROVACAO, object PLANILHAORCAMENTARIAID) { }

        protected void grdPlanilhaOrcamentaria_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.AnalisePlanilhaOrcamentaria analisePlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.Entidades.AnalisePlanilhaOrcamentaria();
            RN.PrestacaoContas.AnalisePlanilhaOrcamentaria rnAnalisePlanilhaOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.AnalisePlanilhaOrcamentaria();
            var analisePlanilhaOrcamentariaId = grdPlanilhaOrcamentaria.GetRowValuesByKeyValue(e.Keys[0], "ANALISEPLANILHAORCAMENTARIAID");
            var planilhaOrcamentariaId = grdPlanilhaOrcamentaria.GetRowValuesByKeyValue(e.Keys[0], "PLANILHAORCAMENTARIAID");


            string acao = e.NewValues["ACAO"].ToString();
            string motivoreprovacao = e.NewValues["MOTIVOREPROVACAO"] != null ? e.NewValues["MOTIVOREPROVACAO"].ToString() : null;

            if (String.IsNullOrEmpty(acao) || acao == "SELECIONE")
            {
                lblMensagem.Text = "</br> Selecione ação ";
                return;
            }

            analisePlanilhaOrcamentaria.Ano = Convert.ToInt32(ddlAnoCadastro.SelectedValue);
            analisePlanilhaOrcamentaria.UsuarioId = User.Identity.Name;
            analisePlanilhaOrcamentaria.AnalisePlanilhaOrcamentariaId = analisePlanilhaOrcamentariaId != DBNull.Value ? Convert.ToInt32(analisePlanilhaOrcamentariaId) : 0;
            analisePlanilhaOrcamentaria.PlanilhaOrcamentariaId = planilhaOrcamentariaId != DBNull.Value ? Convert.ToInt32(planilhaOrcamentariaId) : 0;
            analisePlanilhaOrcamentaria.Aprovada = acao == "S" ? true : false;

            if (!analisePlanilhaOrcamentaria.Aprovada && !String.IsNullOrEmpty(motivoreprovacao))
            {
                analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId = Convert.ToInt32(motivoreprovacao);
            }
            else
            {
                analisePlanilhaOrcamentaria.MotivoReprovacaoPlanilhaOrcamentariaId = null;
            }

            bool cadastro = analisePlanilhaOrcamentaria.AnalisePlanilhaOrcamentariaId == 0;

            validacao = rnAnalisePlanilhaOrcamentaria.Valida(analisePlanilhaOrcamentaria, cadastro);

            if (validacao.Valido)
            {
                if (cadastro)
                {
                    rnAnalisePlanilhaOrcamentaria.Insere(analisePlanilhaOrcamentaria);
                }
                else
                {
                    rnAnalisePlanilhaOrcamentaria.Atualiza(analisePlanilhaOrcamentaria);
                }
            }
            else
            {
                e.Cancel = true;
                lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                //throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        public object ListaMotivo()
        {
            RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria rnMotivoReprovacaoPlanilhaOrcamentaria = new RN.PrestacaoContas.MotivoReprovacaoPlanilhaOrcamentaria();
            var dados = rnMotivoReprovacaoPlanilhaOrcamentaria.ListaAtivoPor();
            return dados;
        }       

        protected void grdPlanilhaOrcamentaria_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            string programaOrcamentariaID = grdPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "PLANILHAORCAMENTARIAID").ToString();
            string ValorTotal = grdPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "VALORTOTAL").ToString();

            if (e.ButtonID == "btnVizualizar")
            {
                hdnProgramaOrcamentarioId.Value = programaOrcamentariaID;
                              

                lblTotalParcelas.Text = ValorTotal.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Convert.ToDecimal(ValorTotal).ToString("c", CultureInfo.CurrentCulture);
                odsItemPlanilha.Select();
                odsItemPlanilha.DataBind();
                grdItemPlanilha.DataBind();

                popup.ShowOnPageLoad = true;
            }
        }

        protected void grdPlanilhaOrcamentaria_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPlanilhaOrcamentaria);
        }

        protected void grdPlanilhaOrcamentaria_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var acao = grdPlanilhaOrcamentaria.GetRowValues(e.VisibleIndex, "ACAO").ToString();

            if (acao == "S")
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlAnoCadastro.SelectedValue.IsNullOrEmptyOrWhiteSpace() || ddlAnoCadastro.SelectedValue == "0")
                {
                    lblMensagem.Text = "Campo ANO é de preenchimento obrigatório";
                    pnlObrigacoesFiscais.Visible = false;
                }
                else
                {

                    if (tseNumProcesso.IsValidDBValue)
                        hdnNumProcesso.Value = tseNumProcesso.DBValue.ToString();
                    else hdnNumProcesso.Value = null;


                    odsPlanilhaOrcamentaria.Select();
                    odsPlanilhaOrcamentaria.DataBind();
                    grdPlanilhaOrcamentaria.DataBind();
                    pnlObrigacoesFiscais.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaAnoCadastro()
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
            ddlAnoCadastro.Items.Clear();
            ddlAnoCadastro.DataSource = rnPeriodoLetivo.ListaAnos(false);
            ddlAnoCadastro.DataBind();
            ddlAnoCadastro.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void ddlAnoCadastro_Load(object sender, EventArgs e)
        {
            if (ddlAnoCadastro.SelectedValue == "")
            {
                tseNumProcesso.SqlWhere = "ano = 9999";
                tseNumProcesso.Value = null;
                tseNumProcesso.DataBind();
            }
        }
    }
}
