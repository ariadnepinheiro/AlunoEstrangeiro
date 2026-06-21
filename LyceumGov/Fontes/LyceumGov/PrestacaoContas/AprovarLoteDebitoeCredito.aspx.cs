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
         NavUrl("~/PrestacaoContas/AprovarLoteDebitoeCredito.aspx"),
         ControlText("AprovarLoteDebitoeCredito"),
         Title("Aprovação em Lotes das Operações")
    ]
    public partial class AprovarLoteDebitoeCredito : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAprovarDebitoCredito);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAprovarDebitoCredito, "");
            btnAprovar.Visible = false;
            EsconderSaldo(1);
            EsconderSaldo(2);
        }
        public object ListaDadosGridPor(String UnidadeEnsino, String PlanoTrabalho, String PeriodoPrestacaoContas, int Status)
        {
            DataTable dados = new DataTable();

            RN.PrestacaoContas.Operacao rnOperacao = new RN.PrestacaoContas.Operacao();

            dados = rnOperacao.ListaDadosGridPor(UnidadeEnsino, PlanoTrabalho, PeriodoPrestacaoContas, Status);
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
        protected void tseUnidadeEnsino_Changed(object sender, EventArgs e)
        {
            try
            {
                hdnUnidadeEnsino.Value = tseUnidadeEnsino.DBValue.ToString();
                EsconderSaldo(1);
                EsconderSaldo(2);
                btnAprovar.Visible = false;
                pnlObrigacoesFiscais.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tsePeriodoPrestacaoContas_Changed(object sender, EventArgs args)
        {
            try
            {
                EsconderSaldo(1);
                EsconderSaldo(2);
                btnAprovar.Visible = false;
                pnlObrigacoesFiscais.Visible = false;
                hdnPeriodoPrestacaoContas.Value = tsePeriodoPrestacaoContas.DBValue.ToString();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tsePlanoTrabalho_Changed(object sender, EventArgs args)
        {
            try
            {
                EsconderSaldo(1);
                EsconderSaldo(2);
                btnAprovar.Visible = false;
                pnlObrigacoesFiscais.Visible = false;
                hdnPlanoTrabalho.Value = tsePlanoTrabalho.DBValue.ToString();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
       
        protected void btnAprovar_Click(object sender, EventArgs e)
        {
            popup.ShowOnPageLoad = true;
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            RN.PrestacaoContas.Operacao rnOperacao = new RN.PrestacaoContas.Operacao();
            try
            {

                rnOperacao.Aprovaemlote(hdnUnidadeEnsino.Value, hdnPlanoTrabalho.Value, hdnPeriodoPrestacaoContas.Value);
                
                pnlObrigacoesFiscais.Visible = false;
                btnAprovar.Visible = false;

                

                EsconderSaldo(1);
               // EsconderSaldo(2);
                btnAprovar.Visible = false;
                
                hdnUnidadeEnsino.Value = "";
                hdnPlanoTrabalho.Value = "";
                hdnPeriodoPrestacaoContas.Value = "";
                tsePeriodoPrestacaoContas.Value = null;
                tsePlanoTrabalho.Value = null;
                tseUnidadeEnsino.Value = null;

                tseUnidadeEnsino.Focus();

                lblMensagem.Text = "Lote Aprovado com sucesso";

         
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Lote Não foi Aprovado";
               // lblMensagem.Text = ex.Message;
            }
        }     

        protected void AtualizaSaldo(string status){
             try
            {
   
              RN.PrestacaoContas.Operacao rnOperacao = new RN.PrestacaoContas.Operacao();
              DataTable dtValorProduto = new DataTable();
              hdnStatus.Value = status;
              String TextoValor = "";
       
              lblTotaldeOperacoesCredito.Text = rnOperacao.TotaldeOperacoesCredito(hdnUnidadeEnsino.Value, hdnPlanoTrabalho.Value, hdnPeriodoPrestacaoContas.Value, hdnStatus.Value).ToString(); 
              if  (lblTotaldeOperacoesCredito.Text =="0")
                  btnAprovar.Visible = false;
          
              else
              {
                  dtValorProduto = rnOperacao.SomadordeOperacoesCredito(hdnUnidadeEnsino.Value, hdnPlanoTrabalho.Value, hdnPeriodoPrestacaoContas.Value, hdnStatus.Value);

                  foreach (DataRow valorproduto in dtValorProduto.Rows)
                  {
                      TextoValor = TextoValor + valorproduto["planotrabalho"].ToString() + " - <b>R$ " + valorproduto["total"].ToString() + "</b><br>";
                  }
                  lblSomaOperacoesCredito.Text = TextoValor;
              }
             }
             catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void EsconderSaldo(int tipo)
        {
            try
            {
                if (tipo == 1)
                {
                    lblTotaldeOperacoesCreditotext.Visible = false;
                    lblTotaldeOperacoesCredito.Visible = false;                    
                }
                else
                {
                    lblSomaOperacoesCreditotext.Visible = false;
                    lblSomaOperacoesCredito.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void ExibirSaldo(int tipo)
        {
            try
            {
                if (tipo == 1)
                {
                    lblTotaldeOperacoesCreditotext.Visible = true;
                    lblTotaldeOperacoesCredito.Visible = true;
                }
                else
                {
                    lblSomaOperacoesCreditotext.Visible = true;
                    lblSomaOperacoesCredito.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if ((!tsePlanoTrabalho.DBValue.IsNull) && (tsePlanoTrabalho.IsValidDBValue))
                {
                    odsAprovarLoteDebitoCredito.Select();
                    odsAprovarLoteDebitoCredito.DataBind();
                    grdAprovarDebitoCredito.DataBind();
                    pnlObrigacoesFiscais.Visible = true;

                    AtualizaSaldo("1");

                    if (lblTotaldeOperacoesCredito.Text == "0")
                    {
                        btnAprovar.Visible = false;
                        EsconderSaldo(2);
                    }
                    else
                    {
                        btnAprovar.Visible = true;
                        ExibirSaldo(2);
                        ExibirSaldo(1);   
                    }
                }
                else
                {
                    lblMensagem.Text = "Campo Projeto/Programa é de preenchimento obrigatório";
                    pnlObrigacoesFiscais.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
