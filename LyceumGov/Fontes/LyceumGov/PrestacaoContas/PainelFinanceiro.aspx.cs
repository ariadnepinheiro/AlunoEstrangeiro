using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/PainelFinanceiro.aspx"), ControlText("Painel Despesas Aprovadas"), Title("Painel Despesas Aprovadas")]
    public partial class PainelFinanceiro : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                lblMensagem.Text = string.Empty;
                txtData.Text = DateTime.Now.Date.ToShortDateString();

                if (!this.IsPostBack)
                {
                    tblPainel.Visible = false;
                    tsePeriodoReferencia.ResetValue();
                    tseUnidadeResponsavel.ResetValue();
                    LimpaControles();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }


        }


        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                decimal saldoAntMerenda = 0;
                decimal saldoAntManutencao = 0;
                decimal saldoAntOutrosProj = 0;
                decimal saldoFinalMerenda = 0;
                decimal saldoFinalManutencao = 0;
                decimal saldoFinalOutrosProj = 0;
                decimal saldoBancario = 0;
                
                List<DateTime> datas = new List<DateTime>();
                RN.PrestacaoContas.PainelFinanceiro rnPainelFinanceiro = new Techne.Lyceum.RN.PrestacaoContas.PainelFinanceiro();

                datas = Montadata(Convert.ToInt32(tsePeriodoReferencia.DBValue));

              //  if (lblSaldoAntMerenda.Text.Length !=0)
                      saldoAntMerenda = (!lblSaldoAntMerenda.Text.IsNullOrEmptyOrWhiteSpace() && lblSaldoAntMerenda.Text != "R$ 0,00") ? Convert.ToDecimal(lblSaldoAntMerenda.Text) : 0;
               // else
               //     saldoAntMerenda = rnPainelFinanceiro.ObtemValorSaldoAnterior(tseUnidadeResponsavel.DBValue.ToString(), datas[0], 2);

                saldoAntManutencao = (!lblSaldoAntManutencao.Text.IsNullOrEmptyOrWhiteSpace() && lblSaldoAntManutencao.Text != "R$ 0,00") ? Convert.ToDecimal(lblSaldoAntManutencao.Text) : 0;
                saldoAntOutrosProj = (!lblSaldoAntOutroProjeto.Text.IsNullOrEmptyOrWhiteSpace() && lblSaldoAntOutroProjeto.Text != "R$ 0,00") ? Convert.ToDecimal(lblSaldoAntOutroProjeto.Text) : 0;


                if (grdMerenda.VisibleRowCount > 0 || saldoAntMerenda != 0)
                {
                    saldoFinalMerenda = (ObtemTotalMerenda() + saldoAntMerenda);
                }

                if (grdManutencao.VisibleRowCount > 0 || saldoAntManutencao != 0)
                {
                    saldoFinalManutencao = (ObtemTotalManutencao() + saldoAntManutencao);
                }
                if (grdOutrosProjetos.VisibleRowCount > 0 || saldoAntOutrosProj != 0)
                {
                    saldoFinalOutrosProj = (ObtemTotalOutrosProjetos() + saldoAntOutrosProj);
                }

                saldoBancario = (saldoFinalMerenda + saldoFinalManutencao + saldoFinalOutrosProj);

                //lblSaldoAntMerenda.Text = saldoAntMerenda != 0 ? saldoAntMerenda.ToString("c") : string.Empty;
                //lblSaldoAntManutencao.Text = saldoAntManutencao != 0 ? saldoAntManutencao.ToString("c") : string.Empty;
                //lblSaldoAntOutroProjeto.Text =saldoAntOutrosProj != 0 ? saldoAntOutrosProj.ToString("c"): string.Empty;
                //lblSaldoFinalMerenda.Text = saldoFinalMerenda != 0 ? saldoFinalMerenda.ToString("c"): string.Empty;
                //lblSaldoFinalManutencao.Text =saldoFinalManutencao != 0 ? saldoFinalManutencao.ToString("c"): string.Empty;
                //lblSaldoFinalOutroProjeto.Text =saldoFinalOutrosProj != 0 ? saldoFinalOutrosProj.ToString("c"): string.Empty;

                //lblSaldoBancarioTeorico.Text =saldoBancario != 0 ? saldoBancario.ToString("c"): string.Empty;

                lblSaldoAntMerenda.Text = saldoAntMerenda.ToString("c") ;
                lblSaldoAntManutencao.Text =  saldoAntManutencao.ToString("c");
                lblSaldoAntOutroProjeto.Text =saldoAntOutrosProj.ToString("c") ;
                lblSaldoFinalMerenda.Text = saldoFinalMerenda.ToString("c") ;
                lblSaldoFinalManutencao.Text = saldoFinalManutencao.ToString("c");
                lblSaldoFinalOutroProjeto.Text = saldoFinalOutrosProj.ToString("c") ;

                lblSaldoBancarioTeorico.Text = saldoBancario.ToString("c") ;
                //FormataValores();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaControles()
        {

            lblSaldoAntMerenda.Text = string.Empty;
            lblSaldoAntManutencao.Text = string.Empty;
            lblSaldoAntOutroProjeto.Text = string.Empty;
            lblSaldoFinalMerenda.Text = string.Empty;
            lblSaldoFinalManutencao.Text = string.Empty;
            lblSaldoFinalOutroProjeto.Text = string.Empty;
            lblSaldoBancarioTeorico.Text = string.Empty;


        }

        private void FormataValores()
        {
            decimal finalMerenda = !lblSaldoFinalMerenda.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(lblSaldoFinalMerenda.Text) : 0;

            lblSaldoFinalMerenda.Text = finalMerenda.ToString("c");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                LimpaControles();
                tblPainel.Visible = false;
                List<DateTime> datas = new List<DateTime>();
                RN.PrestacaoContas.PainelFinanceiro rnPainelFinanceiro = new Techne.Lyceum.RN.PrestacaoContas.PainelFinanceiro();
                Techne.Lyceum.RN.PrestacaoContas.Entidades.PainelFinanceiro painelFinanceiro = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PainelFinanceiro();
                ValidacaoDados validacao = new ValidacaoDados();


                if (tseUnidadeResponsavel.DBValue.IsNull)
                {
                    painelFinanceiro.Censo = null;
                }
                else
                {
                    painelFinanceiro.Censo = Convert.ToString(tseUnidadeResponsavel.DBValue);
                }


                if (tsePeriodoReferencia.DBValue.IsNull)
                {
                    painelFinanceiro.PeriodoReferencia = null;
                }
                else
                {
                    painelFinanceiro.PeriodoReferencia = Convert.ToString(tsePeriodoReferencia.DBValue);
                }

                validacao = rnPainelFinanceiro.Valida(painelFinanceiro);

                if (!validacao.Valido)
                {
                    lblMensagem.Text += validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
                else
                {
                    tblPainel.Visible = true;

                    datas = Montadata(Convert.ToInt32(tsePeriodoReferencia.DBValue));
                    decimal saldoAntMerenda = 0;
                    decimal saldoAntManutencao = 0;
                    decimal saldoAntOutrosProj = 0;

                    odsMerenda.Select();
                    odsMerenda.DataBind();
                    grdMerenda.DataBind();
                        
                    odsManutencao.Select();
                    odsManutencao.DataBind();
                    grdManutencao.DataBind();

                    odsOutrosProjetos.Select();
                    odsOutrosProjetos.DataBind();
                    grdOutrosProjetos.DataBind();

                    saldoAntMerenda = rnPainelFinanceiro.ObtemValorSaldoAnterior(tseUnidadeResponsavel.DBValue.ToString(), datas[0], 2);
                    saldoAntManutencao = rnPainelFinanceiro.ObtemValorSaldoAnterior(tseUnidadeResponsavel.DBValue.ToString(), datas[0], 1);
                    saldoAntOutrosProj = rnPainelFinanceiro.ObtemValorSaldoAnterior(tseUnidadeResponsavel.DBValue.ToString(), datas[0], 3);
                    
                    lblSaldoAntMerenda.Text = saldoAntMerenda != 0 ? saldoAntMerenda.ToString() : string.Empty;
                    lblSaldoAntManutencao.Text = saldoAntManutencao != 0 ? saldoAntManutencao.ToString() : string.Empty;
                    lblSaldoAntOutroProjeto.Text = saldoAntOutrosProj != 0 ? saldoAntOutrosProj.ToString() : string.Empty;

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        public List<DateTime> Montadata(int periodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.PeriodoReferencia();
            Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferencia periodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferencia();
            DateTime dataInicio;
            DateTime dataFim;
            List<DateTime> datas = new List<DateTime>();

            //Dados do periodo referencia  
            periodoReferencia = rnPeriodoReferencia.ObtemPor(contexto, Convert.ToInt32(periodo));

            dataInicio = new DateTime(periodoReferencia.Ano, periodoReferencia.MesInicial, 1);
            dataFim = new DateTime(periodoReferencia.Ano, periodoReferencia.MesFinal, DateTime.DaysInMonth(periodoReferencia.Ano, periodoReferencia.MesFinal));
            datas.Add(dataInicio);
            datas.Add(dataFim);
            return datas;
        }


        public object ListaMerenda(object censo, object inicio, object fim, object referenciaid)
        {
            DataTable dtDados = new DataTable();
            List<DateTime> datas = new List<DateTime>();

            RN.PrestacaoContas.PainelFinanceiro painelFinanceiro = new Techne.Lyceum.RN.PrestacaoContas.PainelFinanceiro();

            if (!string.IsNullOrEmpty(censo.ToString()) != null && !string.IsNullOrEmpty(inicio.ToString()) != null && !string.IsNullOrEmpty(fim.ToString()) != null)
            {
                dtDados = painelFinanceiro.RetornaDadosMerenda(censo.ToString(), Convert.ToDateTime(inicio).Date, Convert.ToDateTime(fim).Date, Convert.ToInt32(referenciaid));

                return dtDados;
            }
            else
            {
                return null;
            }

        }

        public object ListaManutencao(object censo, object inicio, object fim, object referenciaid)
        {

            DataTable dtDados = new DataTable();
            List<DateTime> datas = new List<DateTime>();

            RN.PrestacaoContas.PainelFinanceiro painelFinanceiro = new Techne.Lyceum.RN.PrestacaoContas.PainelFinanceiro();

            if (!string.IsNullOrEmpty(censo.ToString()) != null && !string.IsNullOrEmpty(inicio.ToString()) != null && !string.IsNullOrEmpty(fim.ToString()) != null)
            {

                dtDados = painelFinanceiro.RetornaDadosManutencao(censo.ToString(), Convert.ToDateTime(inicio).Date, Convert.ToDateTime(fim).Date, Convert.ToInt32(referenciaid));

                return dtDados;
            }
            else
            {
                return null;
            }

        }

        public object ListaOutrosProjetos(object censo, object inicio, object fim, object referenciaid)
        {

            DataTable dtDados = new DataTable();
            List<DateTime> datas = new List<DateTime>();

            RN.PrestacaoContas.PainelFinanceiro painelFinanceiro = new Techne.Lyceum.RN.PrestacaoContas.PainelFinanceiro();

            if (!string.IsNullOrEmpty(censo.ToString()) != null && !string.IsNullOrEmpty(inicio.ToString()) != null && !string.IsNullOrEmpty(fim.ToString()) != null)
            {
                dtDados = painelFinanceiro.RetornaDadosOutrosProjetos(censo.ToString(), Convert.ToDateTime(inicio).Date, Convert.ToDateTime(fim).Date, Convert.ToInt32(referenciaid));

                return dtDados;
            }
            else
            {
                return null;
            }

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMerenda, "");
            TituloGrid(grdManutencao, "");
            TituloGrid(grdOutrosProjetos, "");
        }

        protected void tsePeriodoReferencia_Changed(object sender, EventArgs args)
        {
            try
            {
                List<DateTime> datas = new List<DateTime>();
                LimpaControles();
                tblPainel.Visible = false;
                if (Page.IsCallback)
                {
                    return;
                }
                hdnDataInicio.Value = string.Empty;
                hdnDataFim.Value = string.Empty;

                if (!tsePeriodoReferencia.DBValue.IsNull)
                {
                    if (tsePeriodoReferencia.IsValidDBValue)
                    {
                        datas = Montadata(Convert.ToInt32(tsePeriodoReferencia.DBValue));

                        hdnDataInicio.Value = datas[0].Date.ToShortDateString();
                        hdnDataFim.Value = datas[1].Date.ToShortDateString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseUnidadeResponsavel_Changed(object sender, EventArgs args)
        {
            try
            {
                tblPainel.Visible = false;
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (tseUnidadeResponsavel.IsValidDBValue)
                    {
                        LimpaControles();
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {


                        lblMensagem.Text = "";
                    }
                }
                else
                {
                    lblMensagem.Text = "";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private decimal ObtemTotalMerenda()
        {

            decimal totalMerenda = 0;

            for (var rowIndex = 0; rowIndex < this.grdMerenda.VisibleRowCount; rowIndex++)
            {
                var valorItem = Convert.ToDecimal(this.grdMerenda.GetRowValues(rowIndex, "VALORITEM"));

                decimal valorMerenda = Convert.ToString(valorItem).IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToDecimal(valorItem);

                totalMerenda = totalMerenda + valorMerenda;
            }

            return totalMerenda;
        }

        private decimal ObtemTotalManutencao()
        {

            decimal totalManutencao = 0;

            for (var rowIndex = 0; rowIndex < this.grdManutencao.VisibleRowCount; rowIndex++)
            {
                var valorItem = Convert.ToDecimal(this.grdManutencao.GetRowValues(rowIndex, "VALORITEM"));

                decimal valorManutencao = Convert.ToString(valorItem).IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToDecimal(valorItem);

                totalManutencao = totalManutencao + valorManutencao;
            }

            return totalManutencao;
        }

        private decimal ObtemTotalOutrosProjetos()
        {

            decimal totalOutrosProjetos = 0;

            for (var rowIndex = 0; rowIndex < this.grdOutrosProjetos.VisibleRowCount; rowIndex++)
            {
                var valorItem = Convert.ToDecimal(this.grdOutrosProjetos.GetRowValues(rowIndex, "VALORITEM"));

                decimal valorOutrosProjetos = Convert.ToString(valorItem).IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToDecimal(valorItem);

                totalOutrosProjetos = totalOutrosProjetos + valorOutrosProjetos;
            }

            return totalOutrosProjetos;
        }


    }
}
