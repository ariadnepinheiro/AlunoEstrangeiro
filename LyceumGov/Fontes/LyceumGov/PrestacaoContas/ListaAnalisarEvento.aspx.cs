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
using Techne.Controls;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using Techne.Lyceum.RN.PrestacaoContas.DTOs;
using System.Data.SqlTypes;
using DevExpress.Web.ASPxClasses;
using System.Text;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/ListaAnalisarEvento.aspx"), ControlText("ListaAnaliseDespesa"), Title("Lista Analise Despesa")]
    public partial class ListaAnalisarEvento : TPage
    {
        private readonly Techne.Lyceum.RN.PrestacaoContas.Evento rnEvento = new Techne.Lyceum.RN.PrestacaoContas.Evento();
        private readonly Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia();

        protected void Page_Init()
        {
            if (IsPostBack)
                return;

            TituloGrid(grdEvento, "Eventos");
        }

        protected void Page_Load()
        {
            if (Request["__EVENTTARGET"] == "FiltraEvento")
                FiltraEvento();

            try
            {
                if (!IsPostBack)
                {
                    string planoTrabalhoId = string.Empty, censo = string.Empty, tipo = string.Empty, eventoId = string.Empty, periodoPrest = string.Empty, situacao = string.Empty;
                    string tpdespesa = string.Empty;
                    string unidade = string.Empty;
                    string projeto = string.Empty;

                    //verifica se existe alguma querystring
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        var decodedBytes = Convert.FromBase64String(this.Request.QueryString["Chave"]);
                        var decodedText = Encoding.UTF8.GetString(decodedBytes);
                        
                        var listaDados = decodedText.Split('&');

                        foreach (var dados in listaDados)
                        {                           
                            if (dados.IndexOf("periodo") >= 0)
                            {
                                tsePeriodoPrestacaoContas.DBValue = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            else if (dados.IndexOf("situacao") >= 0)
                            {
                                rblSituacaoEvento.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            else if (dados.IndexOf("tpdespesa") >= 0)
                            {
                                rblTipoEvento.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            else if (dados.IndexOf("unidade") >= 0)
                            {
                                tseUnidadeEnsino.DBValue = dados.Substring(dados.LastIndexOf('=') + 1);
                            }
                            else if (dados.IndexOf("projeto") >= 0)
                            {
                                tsePlanoTrabalho.DBValue = dados.Substring(dados.LastIndexOf('=') + 1);
                            }

                        }
                        btnBuscar_Click(null, null);
                    }
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
                plaVisibilidadeGrid.Visible = false;

                var mensagens = new List<string>();

                if (tsePeriodoPrestacaoContas.Value == null || !tsePeriodoPrestacaoContas.IsValidDBValue)
                    mensagens.Add("PERÍODO DE PRESTAÇÃO DE CONTAS: Seleção obrigatória");

                if (rblSituacaoEvento.SelectedIndex == -1)
                    mensagens.Add("SITUAÇÃO: Seleção obrigatória");

                if (mensagens.Any())
                {
                    lblMensagem.Text = mensagens.Distinct().Aggregate((x, y) => x + "<br />" + y);
                    return;
                }

                DateTime dataInicio, dataFim;
                ObtemDtPgtoInicioEFim(Convert.ToInt32(tsePeriodoPrestacaoContas.DBValue), out dataInicio, out dataFim);

                odsEvento.SelectParameters["dataInicio"].DefaultValue = dataInicio.ToString("yyyy-MM-dd");
                odsEvento.SelectParameters["dataFim"].DefaultValue = dataFim.ToString("yyyy-MM-dd");
                odsEvento.SelectParameters["censo"].DefaultValue = Convert.ToString(tseUnidadeEnsino.Value);
                odsEvento.SelectParameters["planoTrabalhoId"].DefaultValue = Convert.ToString(tsePlanoTrabalho.Value);
                odsEvento.SelectParameters["tipoDespesa"].DefaultValue = rblTipoEvento.SelectedValue;
                odsEvento.SelectParameters["eventoId"].DefaultValue = Convert.ToString(tseEvento.Value);
                odsEvento.SelectParameters["situacao"].DefaultValue = rblSituacaoEvento.SelectedValue;

                grdEvento.DataBind();

                plaVisibilidadeGrid.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnVisualizar_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var argument = e.CommandArgument.ToString().Split(',');

                var planoTrabalhoId = Convert.ToInt32(argument[0]);
                var censo = Convert.ToString(argument[1]);
                var tipo = Convert.ToInt32(argument[2]);
                var eventoId = Convert.ToInt32(argument[3]);
                var periodoPrest = tsePeriodoPrestacaoContas.DBValue.ToString();
                var situacao = rblSituacaoEvento.SelectedValue;
                var tpdespesa = rblTipoEvento.SelectedValue;
                var unidade = tseUnidadeEnsino.DBValue;
                var projeto = tsePlanoTrabalho.DBValue;


                string queryString = string.Format("planoTrabalhoId={0}&censo={1}&tipo={2}&eventoId={3}&periodo={4}&situacao={5}&tpdespesa={6}&unidade={7}&projeto={8}",
                    planoTrabalhoId,
                    censo,
                    tipo,
                    eventoId,
                    periodoPrest,
                    situacao,
                    tpdespesa,
                    unidade,
                    projeto
                );

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                Response.Redirect("~/PrestacaoContas/AnalisarEvento.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ObtemDtPgtoInicioEFim(int periodoReferenciaId, out DateTime dataInicio, out DateTime dataFim)
        {
            dataInicio = SqlDateTime.MinValue.Value;
            dataFim = SqlDateTime.MinValue.Value;

            var periodoPrestacaoContas = rnPeriodoReferencia.ObtemPor(periodoReferenciaId);
            if (periodoPrestacaoContas == null)
                return;

            dataInicio = new DateTime(periodoPrestacaoContas.Ano, periodoPrestacaoContas.MesInicial, 1);
            dataFim = new DateTime(periodoPrestacaoContas.Ano, periodoPrestacaoContas.MesFinal, DateTime.DaysInMonth(periodoPrestacaoContas.Ano, periodoPrestacaoContas.MesFinal));
        }

        private bool FiltraEvento()
        {
            if (tsePeriodoPrestacaoContas.DBValue.IsNull || !tsePeriodoPrestacaoContas.IsValidDBValue)
            {
                lblMensagem.Text = "PERÍODO DA PRESTAÇÃO DE CONTAS: Seleção obrigatória";
                return false;
            }

            DateTime dataInicio, dataFim;
            ObtemDtPgtoInicioEFim(Convert.ToInt32(tsePeriodoPrestacaoContas.DBValue), out dataInicio, out dataFim);

            tseEvento.SqlWhere = "";
            tseEvento.Value = null;

            var where = string.Format(" DATAPAGAMENTO BETWEEN '{0}' AND '{1}' AND NUMEROEVENTO IS NOT NULL ", dataInicio.ToString("yyyy-MM-dd"), dataFim.ToString("yyyy-MM-dd"));

            if (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue)
                where += string.Format(" AND CENSO = '{0}' ", tseUnidadeEnsino.Value);

            if (!tsePlanoTrabalho.DBValue.IsNull && tsePlanoTrabalho.IsValidDBValue)
                where += string.Format(" AND PLANOTRABALHOID = {0} ", tsePlanoTrabalho.Value);

            tseEvento.SqlWhere = where;
            tseEvento.Value = null;

            return true;
        }

        public object ListaEvento(DateTime dataInicio, DateTime dataFim, string censo, int? planoTrabalhoId, int? tipoDespesa, int? eventoId, string situacao)
        {
            if (dataInicio < SqlDateTime.MinValue.Value)
                dataInicio = SqlDateTime.MinValue.Value;

            if (dataFim < SqlDateTime.MinValue.Value)
                dataFim = SqlDateTime.MinValue.Value;

            return rnEvento.ListaEventoParaAnalisePor(dataInicio, dataFim, censo, planoTrabalhoId, tipoDespesa, eventoId, situacao,User.Identity.Name);
        }
    }
}