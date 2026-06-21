using System;
using System.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Text;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.DTOs.Agenda;
using DevExpress.Web.ASPxGridView;
using System.Collections.Generic;
using System.Linq;
using Techne.Data;
using Seeduc.Infra.Helpers;
using System.Web.UI.WebControls;
using System.Drawing;
using DevExpress.Web.ASPxClasses;
using System.Globalization;

namespace Techne.Lyceum.Net.Patrimonio
{
    [NavUrl("~/Patrimonio/ListarPatrimonio.aspx"), ControlText("Patrimônio"), Title("Patrimônio")]
    public partial class ListarPatrimonio : TPage
    {
        public object Lista(object setor, object classificacao)
        {
            RN.Patrimonio.Bem rnBem = new Techne.Lyceum.RN.Patrimonio.Bem();

            if (!string.IsNullOrEmpty(setor.ToString()))
            {              
                 return rnBem.ListaPor(setor.ToString(), classificacao.ToString());             
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!this.IsPostBack)
                {
                    pnlGrid.Visible = false;

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

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPatrimonio, "Patrimônio");
            ControlaAcesso(grdPatrimonio);
        }

        private void CarregaQueryString()
        {
            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

            string[] listaDados = decodedText.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("Setor") >= 0)
                {
                    tseUA.DBValue = dados.Substring(dados.LastIndexOf('=') + 1);
                    pnlGrid.Visible = true;
                }
            }
        }

        protected void tseUA_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }
            try
            {
                pnlGrid.Visible = false;
                if (!this.tseUA.DBValue.IsNull)
                {
                    if (!this.tseUA.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade Administrativa não cadastrada.";                       
                    }
                    else
                    {
                        pnlGrid.Visible = true;
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

        protected void grdPatrimonio_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            string tipoOperacao = string.Empty;
            var bemId = string.Empty;

            switch (e.CallbackName)
            {
                case "ADDNEWROW":
                    {
                        tipoOperacao = Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.NOVO);

                        break;
                    }
                case "STARTEDIT":
                    {
                        tipoOperacao = Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.ALTERAR);
                        bemId = Convert.ToString(grdPatrimonio.GetRowValuesByKeyValue(e.Args[0], "BEMID"));
                        break;
                    }
                case "SELECTION":
                    {
                        tipoOperacao = Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.CONSULTAR);
                        bemId = Convert.ToString(grdPatrimonio.GetRowValues(GetSelectedRowOnTheCurrentPage(), "BEMID"));
                        break;
                    }
            }
            if ((e.CallbackName == "ADDNEWROW") || (e.CallbackName == "STARTEDIT" || e.CallbackName == "SELECTION"))
            {
                string queryString = "tipoOperacao=" + tipoOperacao + "&Setor=" + tseUA.DBValue.ToString() + "&NomeUnidade=" + tseUA["nome"].ToString() + "&BemId=" + bemId;
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                ASPxWebControl.RedirectOnCallback("Patrimonio.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdPatrimonio.PageIndex * grdPatrimonio.SettingsPager.PageSize;
            for (int i = 0; i < grdPatrimonio.VisibleRowCount; i++)
            {
                if (grdPatrimonio.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }

        protected void grdPatrimonio_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "VALORATUALIZADOFORMAT")
            {
                var sigla = e.GetListSourceFieldValue("SIGLA");
                var valor = e.GetListSourceFieldValue("VALORATUALIZADO");

                var valorFormatado = string.Format("{0:N2}", valor);

                e.Value = sigla + " " + valorFormatado;
            }

            if (e.Column.FieldName == "ULTIMOVALORFORMATADO")
            {
                var sigla = e.GetListSourceFieldValue("SIGLA");
                var ultimovalor = e.GetListSourceFieldValue("ULTIMOVALOR");

                var ultimovalorFormatado = string.Format("{0:N2}", ultimovalor);

                e.Value = sigla + " " + ultimovalorFormatado;
            }
        }

        protected void grdPatrimonio_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!this.tseUA.DBValue.IsNull && this.tseUA.IsValidDBValue)
            {

                var baixa = grdPatrimonio.GetRowValues(e.VisibleIndex, "BAIXA") != DBNull.Value ? Convert.ToBoolean(grdPatrimonio.GetRowValues(e.VisibleIndex, "BAIXA")) : false;

                if (baixa)
                {
                    if (e.ButtonType == ColumnCommandButtonType.Edit)
                    {
                        e.Visible = false;
                    }
                }
            }
        }

        protected void grdPatrimonio_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableDataCellEventArgs e)
        {
            var data = grdPatrimonio.GetRowValues(e.VisibleIndex, "DATABAIXA");
            if (e.DataColumn.Name == "VALORATUALIZADOFORMAT" && e.CellValue != DBNull.Value)
            {
                var precisa = Convert.ToBoolean(grdPatrimonio.GetRowValues(e.VisibleIndex, "PRECISAREAVALIAR"));

                System.Drawing.Color cor = System.Drawing.Color.FromName("#BFD7F3");
                if (precisa)
                {
                    e.Cell.Attributes.Add("style", "color:red");
                }
            }

            if (data != DBNull.Value)
            {
                e.Cell.Attributes.Add("style", "color:#848484");                
            }
        }

        protected void tseClassificacao_Changed(object sender, ChangedEventArgs args)
        {
            try
            {             

                if (!this.tseClassificacao.DBValue.IsNull)
                {
                    if (!this.tseClassificacao.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Classificação não cadastrada.";
                    }                    
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma classificao.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
