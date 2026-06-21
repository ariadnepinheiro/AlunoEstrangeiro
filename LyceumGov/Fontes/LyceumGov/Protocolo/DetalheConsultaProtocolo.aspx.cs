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

    public partial class DetalheConsultaProtocolo : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    CarregarDadosProtocolo();
                }
            }
        }
        
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAnalise, "Análise Protocolos de Recursos Recebidos");
        }

        public object ListarProtocoloAnalise(object idProtocolo)
        {
            RN.Protocolo.Analise rnAnalise = new Techne.Lyceum.RN.Protocolo.Analise();

            if (idProtocolo != null)
            {
                return rnAnalise.ListaAnalisePor(Convert.ToInt32(idProtocolo.ToString()));
            }

            return null;
        }

        private void LimparDados()
        {
            lblCNPJ.Text = string.Empty;
            lblAno.Text = string.Empty;
            lblSemestre.Text = string.Empty;
            lblNumeroProcesso.Text = string.Empty;
            lblDataProcesso.Text = string.Empty;
            lblFolha.Text = string.Empty;
            lblTipo.Text = string.Empty;
            lblProgramaProtocolo.Text = string.Empty;
            txtObservacao.Text = string.Empty;
        }

        private void CarregarDadosProtocolo()
        {
            try
            {
                RN.DTOs.DadosAnaliseProtocolo dadosProtocolo = new Techne.Lyceum.RN.DTOs.DadosAnaliseProtocolo();
                byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
                
                LimparDados();

                dadosProtocolo = ObterDadosQueryString(decodedText);

                if (dadosProtocolo.ProtocoloPrestacaoId > 0)
                {
                    lblIdProtocolo.Text = dadosProtocolo.ProtocoloPrestacaoId.ToString();
                    lblCNPJ.Text = (!dadosProtocolo.UnidadeEnsino.IsNullOrEmptyOrWhiteSpace() ? dadosProtocolo.Cnpj.AplicarMascaraCNPJ() : string.Empty);
                    lblCNPJ.Visible = !lblCNPJ.Text.IsNullOrEmptyOrWhiteSpace();
                    lblCNPJTexto.Visible = !lblCNPJ.Text.IsNullOrEmptyOrWhiteSpace();
                    lblAno.Text = dadosProtocolo.Ano.ToString();
                    lblSemestre.Text = dadosProtocolo.Semestre.ToString();
                    lblNumeroProcesso.Text = dadosProtocolo.Processo;
                    lblDataProcesso.Text = dadosProtocolo.DataProcesso.Date.ToShortDateString();
                    lblFolha.Text = dadosProtocolo.NumeroFolhas.ToString();
                    lblTipo.Text = dadosProtocolo.TipoProtocolo;
                    lblProgramaProtocolo.Text = dadosProtocolo.ProgramaProtocolo.ToString();
                    txtObservacao.Text = dadosProtocolo.Observacao;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private RN.DTOs.DadosAnaliseProtocolo ObterDadosQueryString(string queryString)
        {
            RN.DTOs.DadosAnaliseProtocolo dadosProtocolo = new RN.DTOs.DadosAnaliseProtocolo();
            RN.Protocolo.ProtocoloPrestacao rnProtocoloPrestacao = new Techne.Lyceum.RN.Protocolo.ProtocoloPrestacao();
            Session["tipoConsulta"] = null;
            Session["valor"] = null;

            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("ProtocoloPrestacaoId") >= 0)
                {
                    dadosProtocolo.ProtocoloPrestacaoId = Convert.ToInt32(dados.Substring(dados.LastIndexOf('=') + 1));
                }
                if (dados.IndexOf("tipoConsulta") >= 0)
                {
                    Session["tipoConsulta"] = dados.Substring(dados.LastIndexOf('=') + 1);
                }
                if (dados.IndexOf("valor") >= 0)
                {
                    Session["valor"] = dados.Substring(dados.LastIndexOf('=') + 1);
                }
            }

            if (dadosProtocolo.ProtocoloPrestacaoId > 0)
            {
                dadosProtocolo = rnProtocoloPrestacao.ObtemDadosAnaliseProtocoloPor(dadosProtocolo.ProtocoloPrestacaoId);

            }

            return dadosProtocolo;
        }

        protected void grdAnalise_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAnalise);
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdAnalise.PageIndex * grdAnalise.SettingsPager.PageSize;
            for (int i = 0; i < grdAnalise.VisibleRowCount; i++)
            {
                if (grdAnalise.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }
        private void LimparDadosAnalise()
        {
            lblAnaliseId.Text = string.Empty;
            lblSituacao.Text = string.Empty;
            lblDataSituacao.Text = string.Empty;
            txtDescricao.Text = string.Empty;
        }
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            string queryString = "voltar";
            queryString += "&tipoConsulta=" + (Session["tipoConsulta"] == null ? string.Empty : Session["tipoConsulta"] );
            queryString += "&valor=" + (Session["valor"] == null ? string.Empty : Session["valor"]);
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
            Response.Redirect("ConsultaProtocolo.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

        protected void grdAnalise_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            string analiseId = grdAnalise.GetRowValues(e.VisibleIndex, "ANALISEID").ToString();
            string programa = grdAnalise.GetRowValues(e.VisibleIndex, "PROGRAMA").ToString();
            string observacao = grdAnalise.GetRowValues(e.VisibleIndex, "OBSERVACAO").ToString();
            string situacao = grdAnalise.GetRowValues(e.VisibleIndex, "SITUACAO").ToString();
            string datasituacao = grdAnalise.GetRowValues(e.VisibleIndex, "DATASITUACAO").ToString();
            string descricao = grdAnalise.GetRowValues(e.VisibleIndex, "DESCRICAO").ToString();

            if (e.ButtonID == "btnSelecionar")
            {
                LimparDadosAnalise();
                lblAnaliseId.Text = analiseId;
                lblProgramaProtocolo.Text = !programa.IsNullOrEmptyOrWhiteSpace() ? programa : string.Empty;
                txtObservacao.Text = observacao;
                lblSituacao.Text = !situacao.IsNullOrEmptyOrWhiteSpace() ? situacao : string.Empty;
                txtDescricao.Text = descricao.Trim();

                if (!datasituacao.IsNullOrEmptyOrWhiteSpace())
                {
                    lblDataSituacao.Text = Convert.ToDateTime(datasituacao).ToShortDateString();
                }

            }
        }

    }
}
