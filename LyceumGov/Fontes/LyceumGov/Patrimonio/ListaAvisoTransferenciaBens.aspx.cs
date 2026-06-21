using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using Techne.Controls;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/ListaAvisoTransferenciaBens.aspx"), ControlText("Aviso de Transferência de Bens Móveis"), Title("Aviso de Transferência de Bens Móveis")]
    public partial class ListaAvisoTransferenciaBens : TPage
    {
        public object Lista(object setorCedente, object setorDestino)
        {
            RN.Patrimonio.Transferencia rnTransferencia = new Techne.Lyceum.RN.Patrimonio.Transferencia();

            if (!string.IsNullOrEmpty(setorCedente.ToString()) && !string.IsNullOrEmpty(setorDestino.ToString()))
            {
                return rnTransferencia.ListaLoteAvisoTransferenciaBensMoveisPor(setorCedente.ToString(), setorDestino.ToString());
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;


                if (!IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        CarregaQueryString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void CarregaQueryString()
        {
            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
            string tipo = string.Empty;
            string valor = string.Empty;

            string[] consulta = decodedText.Split('&');

            foreach (string dados in consulta)
            {
                if (dados.IndexOf("Cedente") >= 0)
                {
                    tseUACedente.DBValue = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                if (dados.IndexOf("Destino") >= 0)
                {
                    tseUADestinataria.DBValue = dados.Substring(dados.LastIndexOf('=') + 1);
                }
            }

            pnlGrid.Visible = true;
            odsATBM.Select();
            odsATBM.DataBind();
            grdATBM.DataBind();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdATBM, string.Empty);
        }

        protected void grdATBM_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdATBM);
        }

        protected void grdATBM_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                if (e.ButtonID == "btnImprimir")
                {
                    var lote = Convert.ToString(grdATBM.GetRowValues(e.VisibleIndex, "LOTE"));
                    var setorDestino = Convert.ToString(grdATBM.GetRowValues(e.VisibleIndex, "SETORDESTINO"));
                    var setorDestinoDescricao = Convert.ToString(grdATBM.GetRowValues(e.VisibleIndex, "SETORDESTINODESCRICAO"));
                    var setorOrigem = Convert.ToString(grdATBM.GetRowValues(e.VisibleIndex, "SETORORIGEM"));
                    var setorOrigemDescricao = Convert.ToString(grdATBM.GetRowValues(e.VisibleIndex, "SETORORIGEMDESCRICAO"));
                    var ano = grdATBM.GetRowValues(e.VisibleIndex, "DATASOLICITACAO") != DBNull.Value ? Convert.ToDateTime(grdATBM.GetRowValues(e.VisibleIndex, "DATASOLICITACAO")).Year.ToString() : string.Empty;
                    var aceitos = Convert.ToInt32(grdATBM.GetRowValues(e.VisibleIndex, "QUANTIDADEITENSACEITOS"));

                    if (aceitos > 0)
                    {
                        string queryString = "Lote=" + lote + "&SetorDestino=" + setorDestino + "&Destinataria=" + setorDestinoDescricao + "&SetorOrigem=" + setorOrigem + "&Cedente=" + setorOrigemDescricao + "&Ano=" + ano;
                        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                        ASPxWebControl.RedirectOnCallback("AvisoTransferenciaBens.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                    }
                    else
                    {
                        throw new Exception(" Este Lote não possui itens aceitos.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void tseUACedente_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }
            try
            {
                tseUADestinataria.ResetValue();
                pnlGrid.Visible = false;                
                if (!this.tseUACedente.DBValue.IsNull)
                {
                    if (!this.tseUACedente.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade Administrativa Cedente não cadastrada.";
                    }
                    else
                    {
                        hidSetorOrigem.Value = tseUACedente["setor"].ToString();
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade administrativa cedente.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUADestinataria_Changed(object sender, EventArgs args)
        {
            try
            {
                pnlGrid.Visible = false;
                if (!this.tseUADestinataria.DBValue.IsNull)
                {
                    if (!this.tseUADestinataria.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade Administrativa Destinatária não cadastrada.";
                    }
                    else
                    {
                        hidSetorDestino.Value = tseUADestinataria["setor"].ToString();

                        pnlGrid.Visible = true;
                        odsATBM.Select();
                        odsATBM.DataBind();
                        grdATBM.DataBind();
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade administrativa destinatária.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
