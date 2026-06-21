using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.Data;
using Techne.Lyceum.RN.PrestacaoContas;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;
using System.Web.UI.HtmlControls;
using System.Data.SqlTypes;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/ExcluirCreditoeDebito.aspx"), ControlText("Excluir Operações"), Title("Excluir Operações")]
    public partial class ExcluirCreditoeDebito : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdRegistro, "");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {
                var tipo = Request.QueryString["tipo"];
                if (tipo == "1")
                {
                    lblMensagem.Text = "Operação Excluída com Sucesso!";
                }
            }
        }

        protected void tsePlanoTrabalho_Changed(object sender, EventArgs args)
        {
            RN.PrestacaoContas.PlanoTrabalho rnPlanoTrabalho = new RN.PrestacaoContas.PlanoTrabalho();

            try
            {
                if (!tsePlanoTrabalho.DBValue.IsNull)
                {
                    if (tsePlanoTrabalho.IsValidDBValue)
                    {
                        DataContext contexto = null;
                        contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                        string[] dadosidentificador = rnPlanoTrabalho.ObtemIdentificadorPor(contexto, Convert.ToInt32(tsePlanoTrabalho.DBValue));
                    }
                    else
                    {

                        lblMensagem.Text = "Plano de Trabalho não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Plano de Trabalho não ativo ou não cadastrado (favor verificar).";

                }
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
                lblMensagem.Text = "";
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseUnidadeEnsino_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseUnidadeEnsino.DBValue.IsNull)
                {
                    if (tseUnidadeEnsino.IsValidDBValue)
                    {

                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {

                        lblMensagem.Text = "Unidade de Ensino não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Unidade de Ensino não ativo ou não cadastrado (favor verificar).";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseOperacao_Changed(object sender, EventArgs e)
        {
            RN.PrestacaoContas.Operacao rnOperacao = new RN.PrestacaoContas.Operacao();

            try
            {
                if (!tseOperacao.IsValidDBValue)
                {
                    return;
                }

                var mensagens = new List<string>();

                if (tsePeriodoPrestacaoContas.Value == null || !tsePeriodoPrestacaoContas.IsValidDBValue)
                    mensagens.Add("PERÍODO DE PRESTAÇÃO DE CONTAS: Seleção obrigatória");

                if (tseUnidadeEnsino.Value == null || !tseUnidadeEnsino.IsValidDBValue)
                    mensagens.Add("UNIDADE DE ENSINO: Seleção obrigatória");

                if (tsePlanoTrabalho.Value == null || !tsePlanoTrabalho.IsValidDBValue)
                    mensagens.Add("PROJETO/PROGRAMA: Seleção obrigatória");

                if (mensagens.Any())
                {
                    lblMensagem.Text = mensagens.Distinct().Aggregate((x, y) => x + "<br />" + y);
                    return;
                }

                var operacao = rnOperacao.ObtemPor(Convert.ToInt32(tseOperacao.Value));
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
                var mensagens = new List<string>();

                if (tsePeriodoPrestacaoContas.Value == null)
                    mensagens.Add("Período da Prestação de Contas: Preenchimento obrigatório");
                else
                    if (tsePeriodoPrestacaoContas.Value.ToString() == "")
                        mensagens.Add("Período da Prestação de Contas: Preenchimento obrigatório");

                if (mensagens.Any())
                {
                    lblMensagem.Text = mensagens.Aggregate((i, j) => i + "<br />" + j);
                    return;
                }

                //CarregaGrid();
                pnlRegistro.Visible = true;
                grdRegistro.DataBind();
                grdRegistro.FocusedRowIndex = -1;

                if (grdRegistro.VisibleRowCount > 0)
                {
                    grdRegistro.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Não existem ocorrências para os filtros selecionados.";
                    grdRegistro.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdRegistro.PageIndex * grdRegistro.SettingsPager.PageSize;
            for (int i = 0; i < grdRegistro.VisibleRowCount; i++)
            {
                if (grdRegistro.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }

        protected void grdRegistro_PageIndexChanged(object sender, EventArgs e)
        {
            //CarregaGrid();
        }

        public object ListaOperacao(object periodo, object filtroOperacao, object unidade, object plano, object operacao)
        {
            RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();

            if (!Convert.ToString(periodo).IsNullOrEmptyOrWhiteSpace())
            {
                return rnArquivoOcorrencia.ListaExigenciasGridAnaliseAprRepPor(
                    Convert.ToInt32(periodo),
                    Convert.ToString(filtroOperacao),
                    Convert.ToString(unidade),
                    Convert.ToString(plano),
                    Convert.ToString(operacao));
            }
            else
            {
                return null;
            }
        }

        protected void grdRegistro_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (e.CallbackName.Equals("SELECTION"))
            {

                var operacaoId = Convert.ToInt32(grdRegistro.GetRowValues(GetSelectedRowOnTheCurrentPage(), "OPERACAOID")); ;

                ASPxWebControl.RedirectOnCallback("ExcluireVerificarCreditoeDebito.aspx?OperacaoID=" + operacaoId);

            }

        }

        private string MontarQueryString(Techne.Lyceum.RN.DTOs.DadosOcorrencia ocorrencia)
        {
            string queryString = string.Empty;

            if (ocorrencia != null)
            {
                queryString += "tela=" + "consulta";
                queryString += "&tipoOperacao=" + "CONSULTAR";
                queryString += "&codigo=" + ocorrencia.OcorrenciaId;
            }
            return queryString;
        }

        protected void ddlOperacao_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        public string ObterDescricaoStatus(string status)
        {
            switch (status)
            {
                case "":
                    return "Lançamento pela AAE";
                case "1":
                    return "Enviado para análise";
                case "2":
                    return "Devolvido para revisão";
                case "3":
                    return "Revisado pela AAE";
                case "4":
                    return "Aprovado";
                case "5":
                    return "Reprovado";
                default:
                    return "(Status desconhecido)";
            }
        }
    }
}
