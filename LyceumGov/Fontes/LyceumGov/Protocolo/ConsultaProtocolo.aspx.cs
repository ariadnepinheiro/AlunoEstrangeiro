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
using DevExpress.Web.ASPxClasses;

namespace Techne.Lyceum.Net.Protocolo
{
    [NavUrl("~/Protocolo/ConsultaProtocolo.aspx")]
    [ControlText("Consulta Protocolo")]
    [Title("Consulta Protocolo")]

    public partial class ConsultaProtocolo : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                CarregarGrid();

                if (!IsPostBack)
                {
                    CarregaSituacao();
                    pnlGridCoordenadoria.Visible = false;
                    pnlGrid.Visible = false;

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                        string tipo = string.Empty;
                        string valor = string.Empty;

                        string[] consulta = decodedText.Split('&');

                        foreach (string dados in consulta)
                        {
                            if (dados.IndexOf("tipoConsulta") >= 0)
                            {
                                tipo = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            if (dados.IndexOf("valor") >= 0)
                            {
                                valor = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                        }
                        MontaTela(tipo, valor);
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
            TituloGrid(grdProtocolo, "Protocolos de Recursos Recebidos");
            TituloGrid(grdProtocoloCoordenadoria, "Protocolos de Recursos Recebidos");
        }
        private void MontaTela(string tipo, string valor)
        {
            LimparTela();
            RetiraVisibilidadePainel();

            if (!tipo.IsNullOrEmptyOrWhiteSpace())
            {
                rblTipoConsulta.SelectedValue = tipo;

                switch (tipo)
                {
                    case "C":
                        {
                            pnlCoordenadoria.Visible = true;
                            tseCoordenadoria.DBValue = valor;
                            tseCoordenadoria_Changed(null, null);
                            break;
                        }
                    case "R":
                        {
                            pnlRegional.Visible = true;
                            tseRegional.DBValue = valor;
                            tseRegional_Changed(null, null);
                            break;
                        }
                    case "U":
                        {
                            pnlUnidade.Visible = true;
                            tseUnidadeResponsavel.DBValue = valor;
                            tseUnidadeResponsavel_Changed(null, null);
                            break;
                        }
                    case "S":
                        {
                            pnlSituacao.Visible = true;
                            ddlSituacao.SelectedValue = valor;
                            ddlSituacao_SelectedIndexChanged(null, null);
                            break;
                        }
                }
            }
        }
        private void CarregaSituacao()
        {
            RN.Protocolo.SituacaoProtocolo rnSituacaoProtocolo = new Techne.Lyceum.RN.Protocolo.SituacaoProtocolo();
            try
            {
                ddlSituacao.Items.Clear();
                ddlSituacao.DataSource = rnSituacaoProtocolo.ListaSituacaoProtocoloAtiva();
                ddlSituacao.Items.Insert(0, new ListItem("Selecione", string.Empty));
                ddlSituacao.DataBind();

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
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                pnlGrid.Visible = false;
                pnlGridCoordenadoria.Visible = false;

                if (sessao != null)
                {
                    if (!this.tseRegional.DBValue.IsNull)
                    {
                        if (this.tseRegional.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            CarregarGrid();
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
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseCoordenadoria_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                pnlGrid.Visible = false;
                pnlGridCoordenadoria.Visible = false;

                if (sessao != null)
                {
                    if (!this.tseCoordenadoria.DBValue.IsNull)
                    {
                        if (this.tseCoordenadoria.IsValidDBValue)
                        {
                            sessao.Coordenadoria = Convert.ToString(tseCoordenadoria.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            CarregarGrid();
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
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
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
                pnlGridCoordenadoria.Visible = false;

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = tseUnidadeResponsavel["id_regional"].ToString();
                            sessao.Municipio = tseUnidadeResponsavel["municipio"].ToString();
                            CarregarGrid();
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

        protected void rblTipoConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RetiraVisibilidadePainel();
                LimparTela();

                if (!rblTipoConsulta.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    string tipo = rblTipoConsulta.SelectedValue;

                    switch (tipo)
                    {
                        case "C":
                            {
                                pnlCoordenadoria.Visible = true;
                                break;
                            }
                        case "R":
                            {
                                pnlRegional.Visible = true;
                                break;
                            }
                        case "U":
                            {
                                pnlUnidade.Visible = true;
                                break;
                            }
                        case "S":
                            {
                                pnlSituacao.Visible = true;
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregarGrid();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void RetiraVisibilidadePainel()
        {
            pnlCoordenadoria.Visible = false;
            pnlRegional.Visible = false;
            pnlUnidade.Visible = false;
            pnlSituacao.Visible = false;
            pnlGridCoordenadoria.Visible = false;
            pnlGrid.Visible = false;
        }

        private void LimparTela()
        {
            tseCoordenadoria.ResetValue();
            tseRegional.ResetValue();
            tseUnidadeResponsavel.ResetValue();
            ddlSituacao.ClearSelection();

        }

        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!rblTipoConsulta.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if ((this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) || (this.tseRegional.IsValidDBValue && !this.tseRegional.DBValue.IsNull) || (this.tseCoordenadoria.IsValidDBValue && !this.tseCoordenadoria.DBValue.IsNull) || !ddlSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        CarregarGrid();
                        pnlGrid.Visible = true;
                    }
                    else
                    {
                        lblMensagem.Text = "Para efetuar a busca é necessario selecionar uma Coordenadoria ou Regional ou Unidade de Ensino ou Situação.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Para efetuar a busca é necessario selecionar um Tipo de Consulta.";
                }
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


                string queryString = MontarQueryString("consulta", id);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                ASPxWebControl.RedirectOnCallback("DetalheConsultaProtocolo.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private string MontarQueryString(string tipoOperacao, int protocoloPrestacaoId)
        {
            string queryString = string.Empty;

            if (protocoloPrestacaoId > 0)
            {
                queryString += "tipoOperacao=" + tipoOperacao;
                queryString += "&tipoConsulta=" + rblTipoConsulta.SelectedValue;

                switch (rblTipoConsulta.SelectedValue)
                {
                    case "C":
                        {
                            queryString += "&valor=" + tseCoordenadoria.DBValue;
                            break;
                        }
                    case "R":
                        {
                            queryString += "&valor=" + tseRegional.DBValue;
                            break;
                        }
                    case "U":
                        {
                            queryString += "&valor=" + tseUnidadeResponsavel.DBValue;
                            break;
                        }
                    case "S":
                        {
                            queryString += "&valor=" + ddlSituacao.SelectedValue;
                            break;
                        }
                }

                queryString += "&ProtocoloPrestacaoId=" + protocoloPrestacaoId;
            }
            return queryString;
        }

        private void CarregarGrid()
        {
            RN.Protocolo.ProtocoloPrestacao rnProtocoloPrestacao = new Techne.Lyceum.RN.Protocolo.ProtocoloPrestacao();
            DataTable dtPrestacao = new DataTable();

            try
            {
                pnlGridCoordenadoria.Visible = false;
                pnlGrid.Visible = false;

                if (rblTipoConsulta.SelectedValue != "C")
                {
                    if ((this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) || (this.tseRegional.IsValidDBValue && !this.tseRegional.DBValue.IsNull) || !ddlSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        pnlGrid.Visible = true;
                        dtPrestacao = rnProtocoloPrestacao.ListaConsultaProtocoloPor(0, ((this.tseRegional.IsValidDBValue && !this.tseRegional.DBValue.IsNull) ? Convert.ToInt32(tseRegional.DBValue.ToString()) : 0), ((this.tseUnidadeResponsavel.IsValidDBValue && !this.tseUnidadeResponsavel.DBValue.IsNull) ? tseUnidadeResponsavel.DBValue.ToString() : string.Empty), (!ddlSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSituacao.SelectedValue) : 0));
                        grdProtocolo.DataSource = dtPrestacao;
                        grdProtocolo.DataBind();
                    }
                }

                if (rblTipoConsulta.SelectedValue == "C")
                {
                    if (this.tseCoordenadoria.IsValidDBValue && !this.tseCoordenadoria.DBValue.IsNull)
                    {
                        pnlGridCoordenadoria.Visible = true;
                        dtPrestacao = rnProtocoloPrestacao.ListaConsultaProtocoloPor((this.tseCoordenadoria.IsValidDBValue && !this.tseCoordenadoria.DBValue.IsNull) ? Convert.ToInt32(tseCoordenadoria.DBValue.ToString()) : 0, 0, string.Empty, 0);
                        if (dtPrestacao.Rows.Count > 0)
                        {
                            grdProtocoloCoordenadoria.DataSource = dtPrestacao;
                            grdProtocoloCoordenadoria.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
