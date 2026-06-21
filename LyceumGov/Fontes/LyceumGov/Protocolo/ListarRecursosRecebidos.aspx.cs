using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using Techne.Web;

namespace Techne.Lyceum.Net.Protocolo
{
    [NavUrl("~/Protocolo/ListarAnaliseRecursoRecebido.aspx")]
    [ControlText("Análise Recursos Recebidos")]
    [Title("Análise Recursos Recebidos")]

    public partial class ListarAnaliseRecursoRecebido : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            CarregarGrid();
            if (!IsPostBack)
            {
                PreencherDadosSession();

                if (Request.QueryString.Keys.Count > 0)
                {
                    byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                    string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                    if (decodedText == "voltar")
                    {
                        CarregarGrid();
                        pnlGrid.Visible = true;
                    }
                }
            }
        }

        private void PreencherDadosSession()
        {
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

            if (sessao != null)
            {
                if (!string.IsNullOrEmpty(sessao.Regional))
                {
                    tseRegional.DBValue = sessao.Regional;
                    if (!tseRegional.IsValidDBValue)
                    {
                        tseRegional.Msg = string.Empty;
                        tseRegional.ResetValue();
                    }
                }
                if (!string.IsNullOrEmpty(sessao.Municipio))
                {
                    tseMunicipio.DBValue = sessao.Municipio;
                    if (!tseMunicipio.IsValidDBValue)
                    {
                        tseMunicipio.Msg = string.Empty;
                        tseMunicipio.ResetValue();
                    }
                }
                if (!string.IsNullOrEmpty(sessao.Escola))
                {
                    tseUnidadeResponsavel.DBValue = sessao.Escola;
                    if (!tseUnidadeResponsavel.IsValidDBValue)
                    {
                        tseUnidadeResponsavel.Msg = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProtocolo, "Protocolo de Recursos Recebidos");
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                pnlGrid.Visible = false;

                if (sessao != null)
                {
                    if (!this.tseRegional.DBValue.IsNull)
                    {
                        if (this.tseRegional.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;

                            tseMunicipio.ResetValue();
                            tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            sessao.Regional = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseMunicipio.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();
                pnlGrid.Visible = false;

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                            sessao.Escola = string.Empty;
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            tseUnidadeResponsavel.ResetValue();
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                var sessao = RN.SessaoUsuario.GetSessaoUsuario();
                pnlGrid.Visible = false;


                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            this.tseRegional.Value = this.tseUnidadeResponsavel["id_regional"];
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegional.DBValue);
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);
                        }

                    }
                    else
                    {
                        lblMensagem.Text = "Unidade de ensino não encontrada.";
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Regional = string.Empty;
                        }
                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if ((this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) || (this.tseRegional.IsValidDBValue && !this.tseRegional.DBValue.IsNull))
                {
                    CarregarGrid();
                    pnlGrid.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Para efetuar a busca é necessario selecionar uma Regional e/ou Unidade de Ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarGrid()
        {
            RN.Protocolo.ProtocoloPrestacao rnProtocoloPrestacao = new Techne.Lyceum.RN.Protocolo.ProtocoloPrestacao();
            DataTable dtPrestacao = new DataTable();

            try
            {
                if ((this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) || (this.tseRegional.IsValidDBValue && !this.tseRegional.DBValue.IsNull))
                {
                    dtPrestacao = rnProtocoloPrestacao.ListaProtocoloComUltimaSituacaoPor((this.tseRegional.IsValidDBValue && !this.tseRegional.DBValue.IsNull) ? Convert.ToInt32(tseRegional.DBValue.ToString()) : 0, (this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) ? tseUnidadeResponsavel.DBValue.ToString() : string.Empty);
                    grdProtocolo.DataSource = dtPrestacao;
                }

                grdProtocolo.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdProtocolo.PageIndex * grdProtocolo.SettingsPager.PageSize;
            for (int i = 0; i < grdProtocolo.VisibleRowCount; i++)
            {
                if (grdProtocolo.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }

        protected void grdProtocolo_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int curPageSelection = GetSelectedRowOnTheCurrentPage();

                int id = Convert.ToInt32(grdProtocolo.GetRowValues(curPageSelection, "PROTOCOLOPRESTACAOID"));

                if (id > 0)
                {

                    string queryString = MontarQueryString("consulta", id);
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                    ASPxWebControl.RedirectOnCallback("AnaliseRecursosRecebidos.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
                else
                {
                    lblMensagem.Text = "Protocolo não identificado.";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdProtocolo_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            string tipoOperacao = string.Empty;

        }

        private bool PodeExecutarOperacaoPorNomeRetornoGridGradeSerie(string nomeRetorno)
        {
            return nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.ADDNEWROW)
                || nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.STARTEDIT)
                || nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.SELECTION);
        }

        protected void ControlaAcessoGrid()
        {         

            //verifica acesso do usuário para os controles da página
            ControlaAcesso(grdProtocolo);
        }

        private string MontarQueryString(string tipoOperacao, int protocoloPrestacaoId)
        {
            string queryString = string.Empty;

            if (protocoloPrestacaoId > 0)
            {
                queryString += "tipoOperacao=" + tipoOperacao;
                queryString += "&ProtocoloPrestacaoId=" + protocoloPrestacaoId;
            }
            return queryString;
        }
    }
}
