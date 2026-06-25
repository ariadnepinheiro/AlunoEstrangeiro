using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using System.Data.SqlTypes;
using Techne.Web;
using Techne.Lyceum.RN;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
    NavUrl("~/PrestacaoContas/ControlarSaldo.aspx"),
    ControlText("ControlarSaldo"),
    Title("Controlar Saldo"),
    ]
    public partial class ControlarSaldo : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        protected void tsePeriodoReferencia_Changed(object sender, Controls.ChangedEventArgs args)
        {
            Limpa();

            if (!tseUnidadeResponsavel.DBValue.IsNull)
            {
                if (tseUnidadeResponsavel.IsValidDBValue)
                {

                    lblMensagem.Text = string.Empty;
                }
                else
                {

                    lblMensagem.Text = "Período Referência não cadastrado.";
                }
            }
            else
            {
                lblMensagem.Text = "Favor consultar um Período Referência.";
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Controls.ChangedEventArgs args)
        {
            try
            {
                Limpa();
                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);
                        }

                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Coordenadoria = string.Empty;
                        }

                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Coordenadoria = string.Empty;
                    }

                    lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsePlanoTrabalho_Changed(object sender, Controls.ChangedEventArgs args)
        {
            try
            {
                Limpa();

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (tseUnidadeResponsavel.IsValidDBValue)
                    {

                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {

                        lblMensagem.Text = "Projeto / Programa não cadastrado.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor consultar um Projeto / Programa.";
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
                Limpa();
                bool erro = false;

                if (tsePeriodoReferencia.DBValue.IsNull || !tsePeriodoReferencia.IsValidDBValue)
                {
                    lblMensagem.Text += "O campo obrigatorio Periodo Referência não foi preenchido </br>";
                    erro = true;
                }
                if (tseUnidadeResponsavel.DBValue.IsNull || !tseUnidadeResponsavel.IsValidDBValue)
                {
                    lblMensagem.Text += "O campo obrigatorio Unidade de Ensino não foi preenchido </br>";
                    erro = true;
                }
                if (tsePlanoTrabalho.DBValue.IsNull || !tsePlanoTrabalho.IsValidDBValue)
                {
                    lblMensagem.Text += "O campo obrigatorio Projeto / Programa não foi preenchido ";
                    erro = true;
                }

                if (!erro)
                {
                    ObtemDados();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ObtemDados()
        {
            RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new RN.PrestacaoContas.PeriodoReferencia();
            RN.PrestacaoContas.PainelFinanceiro rnPainelFinanceiro = new Techne.Lyceum.RN.PrestacaoContas.PainelFinanceiro();
            RN.PrestacaoContas.Entidades.PeriodoReferencia periodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferencia();
            RN.PrestacaoContas.DTOs.DadosReceitasRecebidas dadosReceitas = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasRecebidas();
            DateTime dataInicio;
            DateTime dataFim;
            string censo;
            int finalidadeId;
            int planoTrabalhoId;
            decimal saldo;
            try
            {
                pnlSaldo.Visible = true;
                censo = tseUnidadeResponsavel.DBValue.ToString();

                //Dados do periodo referencia  
                periodoReferencia = rnPeriodoReferencia.ObtemPor(Convert.ToInt32(tsePeriodoReferencia.DBValue));
                dataInicio = new DateTime(periodoReferencia.Ano, periodoReferencia.MesInicial, 1);
                dataFim = new DateTime(periodoReferencia.Ano, periodoReferencia.MesFinal, DateTime.DaysInMonth(periodoReferencia.Ano, periodoReferencia.MesFinal));

                //Finalidade pelo plano
                lblFinalidade.Text = tsePlanoTrabalho["FINALIDADE"].ToString();
                finalidadeId = Convert.ToInt32(tsePlanoTrabalho["FINALIDADEID"]);
                planoTrabalhoId = Convert.ToInt32(tsePlanoTrabalho.DBValue);

                //Busca Receitas
                dadosReceitas = rnPainelFinanceiro.ObtemReceitasPor(censo, dataInicio, dataFim, finalidadeId, planoTrabalhoId, Convert.ToInt32(tsePeriodoReferencia.DBValue));
                
                //Monta dados Receitas              
                lblSaldoInicial.Text = dadosReceitas.SaldoAnterior.ToString("c");
                lblRepasses.Text = dadosReceitas.Repasses.ToString("c");
                lblDespesas.Text = dadosReceitas.Despesas.ToString("c");

                //Busca Operacoes
                var CreditosAnalisados = rnPainelFinanceiro.ObtemCreditoDebitoPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tsePeriodoReferencia.DBValue), Convert.ToInt32(tsePlanoTrabalho.DBValue), "C");
                var DebitosAnalisados  = rnPainelFinanceiro.ObtemCreditoDebitoPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tsePeriodoReferencia.DBValue), Convert.ToInt32(tsePlanoTrabalho.DBValue), "D");
                
                lblCreditosAnalisados.Text = CreditosAnalisados.ToString("c");
                lblDebitosAnalisados.Text  = DebitosAnalisados.ToString("c");

                if (finalidadeId == 1)//Manutenção
                {
                    //Para manutenção soma os rendimentos da aplicacao finaneira
                    lblCreditos.Text = (dadosReceitas.Devolucoes + dadosReceitas.Rendimentos).ToString("c");
                    saldo = dadosReceitas.SaldoAnterior + dadosReceitas.Repasses + CreditosAnalisados + (dadosReceitas.Devolucoes + dadosReceitas.Rendimentos) - (dadosReceitas.Despesas + DebitosAnalisados);
                }
                else
                {
                    lblCreditos.Text = dadosReceitas.Devolucoes.ToString("c");
                    saldo = dadosReceitas.SaldoAnterior + dadosReceitas.Repasses + CreditosAnalisados + dadosReceitas.Devolucoes - (dadosReceitas.Despesas+ DebitosAnalisados);
                }

             
                lblSaldo.Text = saldo.ToString("c");

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void Limpa()
        {
            pnlSaldo.Visible = false;
            lblFinalidade.Text = string.Empty;
            lblSaldoInicial.Text = string.Empty;
            lblRepasses.Text = string.Empty;
            lblDespesas.Text = string.Empty;
            lblCreditos.Text = string.Empty;
            lblCreditosAnalisados.Text = string.Empty;
            lblDebitosAnalisados.Text = string.Empty;
            lblSaldo.Text = string.Empty;
        }
    }
}
