using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Lyceum.RN.Util;
using Techne.Controls;
using System.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using Seeduc.Infra.Helpers;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
NavUrl("~/PrestacaoContas/ExigenciasEvento.aspx"),
ControlText("Relatório de Exigências de Despesas"),
Title("Relatório de Exigências de Despesas"),
]
    public partial class ExigenciasEvento : TPage
    {
        public enum TipoOperacao
        {
            Consultar,
            Inicial
        }

        private TipoOperacao _tipoOperacao
        {
            get
            {
                if (ViewState["_tipoOperacao"] != null)
                {
                    if (ViewState["_tipoOperacao"] is TipoOperacao)
                    {
                        return (TipoOperacao)ViewState["_tipoOperacao"];
                    }
                }

                return TipoOperacao.Inicial;
            }
            set
            {
                ViewState["_tipoOperacao"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdExigencias, "Exigências de Despesas");
        }

        private void CarregaFinalidade()
        {
            RN.PrestacaoContas.Finalidade rnFinalidade = new RN.PrestacaoContas.Finalidade();
            try
            {
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlFinalidade.Items.Clear();
                ddlFinalidade.DataSource = rnFinalidade.ListaAtivo();
                ddlFinalidade.DataBind();
                ddlFinalidade.Items.Insert(0, item);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia();
            ImageButton[] controles;
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        controles = new ImageButton[] { btnBuscar };
                        ControlarVisibilidadeControle(controles);
                        CarregaFinalidade();
                        LimpaFiltro();
                        pnlGrid.Visible = false;

                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        RN.PrestacaoContas.ExigenciaEvento rnExigenciaEvento = new Techne.Lyceum.RN.PrestacaoContas.ExigenciaEvento();
                        ControlarTSearchs();

                        pnlGrid.Visible = true;

                        AtualizarGrid();

                        break;
                    }
            }
        }

        protected void tsePeriodoReferencia_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                pnlGrid.Visible = false;
                if (!tsePeriodoReferencia.DBValue.IsNull)
                {
                    if (tsePeriodoReferencia.IsValidDBValue)
                    {
                        CarregarFiltroEvento();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsino_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                pnlGrid.Visible = false;
                if (!tseUnidadeEnsino.DBValue.IsNull)
                {
                    if (tseUnidadeEnsino.IsValidDBValue)
                    {
                        CarregarFiltroEvento();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlFinalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarFiltroEvento();
            pnlGrid.Visible = false;
        }

        protected void tseEvento_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                pnlGrid.Visible = false;

                if (!tseEvento.DBValue.IsNull)
                {
                    if (!tseEvento.IsValidDBValue)
                    {

                        lblMensagem.Text = "Despesa não cadastrada (favor verificar).";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tseUnidadeEnsino.Enabled = true;
                        tseUnidadeEnsino.Mode = ControlMode.Edit;

                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        tseUnidadeEnsino.Mode = ControlMode.Edit;
                        tseUnidadeEnsino.Mode = ControlMode.Edit;

                        break;
                    }
            }
        }

        private void LimpaFiltro()
        {
            tsePeriodoReferencia.ResetValue();
            tseUnidadeEnsino.ResetValue();
            ddlFinalidade.ClearSelection();
            tseEvento.ResetValue();
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            btnBuscar.Visible = false;

            if (botoes != null)
            {
                foreach (ImageButton botao in botoes)
                {
                    botao.Visible = true;
                }
            }
        }

        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!tsePeriodoReferencia.IsValidDBValue || tsePeriodoReferencia.DBValue.IsNull ||
                    !tseUnidadeEnsino.IsValidDBValue || tseUnidadeEnsino.DBValue.IsNull)
                {
                    lblMensagem.Text = "Para efetuar a busca é necessário selecionar as duas datas do intervalo de pagamento e a unidade de ensino.";
                    pnlGrid.Visible = false;
                }
                else
                {
                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            try
            {
                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });
                int eventoId = Convert.ToInt32(chave[0]);
                string tipoEvento = chave[1].ToString();

                string queryString = MontarQueryString(tipoEvento, eventoId);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                Response.Redirect("CadastrarEvento.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private string MontarQueryString(string tipoEvento, int eventoId)
        {
            RN.PrestacaoContas.Evento rnEvento = new Techne.Lyceum.RN.PrestacaoContas.Evento();
            string queryString = string.Empty;
            int codigoTipoEvento = -1;

            if (tipoEvento == "Despesa Comum")
            {
                codigoTipoEvento = (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum;
            }
            else if (tipoEvento == "Pequena Despesa Com comprovação")
            {
                codigoTipoEvento = (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao;
            }
            else if (tipoEvento == "Pequena Despesa Sem comprovação")
            {
                codigoTipoEvento = (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaSemComprovacao;
            }
            else if (tipoEvento == "Pequena Despesa com Translado de Servidores")
            {
                codigoTipoEvento = (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores;
            }
            

            int planoTrabalhoId = rnEvento.ObtemPlanoTrabalhoPor(eventoId);

            queryString += "tipoEvento=" + codigoTipoEvento;
            queryString += "&eventoId=" + eventoId.ToString();
            queryString += "&censo=" + tseUnidadeEnsino["unidade_ens"].ToString();
            queryString += "&planoTrabalhoId=" + planoTrabalhoId.ToString();
            queryString += "&referencia=" + tsePeriodoReferencia.DBValue.ToString();

            return queryString;
        }

        protected void dtFim_DateChanged(object sender, EventArgs e)
        {
            CarregarFiltroEvento();
            pnlGrid.Visible = false;
        }

        protected void dtInicio_DateChanged(object sender, EventArgs e)
        {
            CarregarFiltroEvento();
            pnlGrid.Visible = false;
        }

        private void CarregarFiltroEvento()
        {
            RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia();
            tseEvento.ResetValue();

            if (!tsePeriodoReferencia.IsValidDBValue || tsePeriodoReferencia.DBValue.IsNull ||
                    !tseUnidadeEnsino.IsValidDBValue || tseUnidadeEnsino.DBValue.IsNull)
            {
                //Caso os campos obrigatorios não estejam preenchidos nao trazer nada
                tseEvento.SqlWhere = " 1 <> 1";
            }
            else
            {
                //Dados do periodo referencia  
                var periodoReferencia = rnPeriodoReferencia.ObtemPor(Convert.ToInt32(tsePeriodoReferencia.DBValue));
                DateTime inicio = new DateTime(periodoReferencia.Ano, periodoReferencia.MesInicial, 1);
                DateTime fim = new DateTime(periodoReferencia.Ano, periodoReferencia.MesFinal, DateTime.DaysInMonth(periodoReferencia.Ano, periodoReferencia.MesFinal));
                
                //Verifca se tem finalidade
                if (ddlFinalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    tseEvento.SqlWhere = " NUMEROEVENTO IS NOT NULL AND CENSO = '" + tseUnidadeEnsino.DBValue + "' and E.DATAPAGAMENTO >= '" + inicio.ToString("yyyy-MM-dd") + "' and e.DATAPAGAMENTO <= '" + fim.ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    tseEvento.SqlWhere = " NUMEROEVENTO IS NOT NULL AND CENSO = '" + tseUnidadeEnsino.DBValue + "' and f.FINALIDADEID = " + ddlFinalidade.SelectedValue + " and E.DATAPAGAMENTO >= '" + inicio.ToString("yyyy-MM-dd") + "' and e.DATAPAGAMENTO <= '" + fim.ToString("yyyy-MM-dd") + "'";
                }
            }

            tseEvento.DataBind();
        }

        protected void grdExigencias_PageIndexChanged(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        private void AtualizarGrid()
        {
            RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia();
            RN.PrestacaoContas.ExigenciaEvento rnExigenciaEvento = new Techne.Lyceum.RN.PrestacaoContas.ExigenciaEvento();

            //Dados do periodo referencia  
            var periodoReferencia = rnPeriodoReferencia.ObtemPor(Convert.ToInt32(tsePeriodoReferencia.DBValue));
            DateTime inicio = new DateTime(periodoReferencia.Ano, periodoReferencia.MesInicial, 1);
            DateTime fim = new DateTime(periodoReferencia.Ano, periodoReferencia.MesFinal, DateTime.DaysInMonth(periodoReferencia.Ano, periodoReferencia.MesFinal));

            string unidade = tseUnidadeEnsino.DBValue.ToString();
            int? finalidadeId = ddlFinalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? null : (int?)Convert.ToInt32(ddlFinalidade.SelectedValue);
            int? eventoId = (tseEvento.IsValidDBValue && !tseEvento.DBValue.IsNull) ? (int?)Convert.ToInt32(tseEvento.DBValue) : null;

            grdExigencias.DataSource = rnExigenciaEvento.ListaTodosEventosExigenciasReprovadasPor(inicio, fim, unidade, finalidadeId, eventoId);
            grdExigencias.DataBind();
        }
    }
}
