using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Lyceum.Net.Modulos;
using System.Linq;
using Techne.Controls;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;
using System.Web;
using System.Web.UI;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/ProrrogacaoPrazoConfirmacao.aspx"),
  ControlText("Prorrogação Prazo de Confirmação de Matrícula"),
  Title("Prorrogação Prazo de Confirmação de Matrícula")]
    public partial class ProrrogacaoPrazoConfirmacao : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    rblTipoFiltro.ClearSelection();
                    LimpaTela();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdProrrogacaoPrazoConfirmacao, "Datas com confirmação de matricula pendentes");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvar, AcaoControle.editar);
        }

        protected void rblTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimpaTela();

            if (rblTipoFiltro.SelectedValue == "porRegional")
            {
                pnlRegional.Visible = true;
            }
            else if (rblTipoFiltro.SelectedValue == "porUnidade")
            {
                pnlUnidade.Visible = true;
            }
            else if (rblTipoFiltro.SelectedValue == "porMunicipio")
            {
                pnlMunicipio.Visible = true;
            }
        }

        protected void tseRegional_Changed(object sender, ChangedEventArgs args)
        {
            string municipio = string.Empty;
            try
            {
                tseRegional.Visible = true;
                if (Page.IsCallback)
                {
                    return;
                }
               
                tseMunicipio.ResetValue();
                tseUnidade.ResetValue();               
                grdProrrogacaoPrazoConfirmacao.DataSource = null;
                grdProrrogacaoPrazoConfirmacao.DataBind();
                pnlProrrogacaoPrazoConfirmacao.Visible = false;
                txtDias.Text = string.Empty;

                if (!this.tseRegional.DBValue.IsNull)
                {
                    if (!this.tseRegional.IsValidDBValue)
                    {
                        lblMensagem.Text = "Regional não cadastrada.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar uma regional.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidade_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                tseMunicipio.ResetValue();
                tseRegional.ResetValue();
                grdProrrogacaoPrazoConfirmacao.DataSource = null;
                grdProrrogacaoPrazoConfirmacao.DataBind();
                pnlProrrogacaoPrazoConfirmacao.Visible = false;
                txtDias.Text = string.Empty;

                if (!this.tseUnidade.DBValue.IsNull)
                {
                    if (!this.tseUnidade.IsValidDBValue)
                    {
                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar uma unidade de ensino.";
                }               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                tseRegional.ResetValue();
                tseUnidade.ResetValue();                
                pnlProrrogacaoPrazoConfirmacao.Visible = false;
                grdProrrogacaoPrazoConfirmacao.DataSource = null;
                grdProrrogacaoPrazoConfirmacao.DataBind();
                txtDias.Text = string.Empty;

                if (!tseMunicipio.DBValue.IsNull)
                {
                    if (!tseMunicipio.IsValidDBValue)
                    {
                        lblMensagem.Text = "Município não cadastrado.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar um Município.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaTela()
        {
            tseRegional.ResetValue();
            tseMunicipio.ResetValue();
            tseUnidade.ResetValue();
            pnlUnidade.Visible = false;
            pnlMunicipio.Visible = false;
            pnlRegional.Visible = false;
            grdProrrogacaoPrazoConfirmacao.DataSource = null;
            grdProrrogacaoPrazoConfirmacao.DataBind();
            pnlProrrogacaoPrazoConfirmacao.Visible = false;
            txtDias.Text = string.Empty;
        }

        private void CarregaGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                RN.Matriculas.OpcaoInscricao rnOpcaoInscricao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
                bool carrega = false;

                //Verifica se o tipo foi preenchido
                if (rblTipoFiltro.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    LimpaTela();
                    lblMensagem.Text = "Selecione um tipo de pesquisa.";
                    return;
                }

                switch (rblTipoFiltro.SelectedValue)
                {
                    case "todos":
                        dt = rnOpcaoInscricao.ListaDatasConvocados();
                        carrega = true;
                        break;
                    case "porUnidade":
                        if (!this.tseUnidade.DBValue.IsNull && this.tseUnidade.IsValidDBValue)
                        {
                            dt = rnOpcaoInscricao.ListaDatasConvocadosPorEscola(tseUnidade.DBValue.ToString());
                            carrega = true;
                        }
                        else
                        {
                            lblMensagem.Text = "Selecione uma Unidade de Ensino.";
                        }
                        break;
                    case "porMunicipio":
                        if (!this.tseMunicipio.DBValue.IsNull && this.tseMunicipio.IsValidDBValue)
                        {
                            dt = rnOpcaoInscricao.ListaDatasConvocadosPorMunicipio(tseMunicipio.DBValue.ToString());
                            carrega = true;
                        }
                        else
                        {
                            lblMensagem.Text = "Selecione um Municipio.";
                        }
                        break;
                    case "porRegional":
                        if (!this.tseRegional.DBValue.IsNull && this.tseRegional.IsValidDBValue)
                        {
                            dt = rnOpcaoInscricao.ListaDatasConvocadosPorRegional(Convert.ToInt32(tseRegional.DBValue));
                            carrega = true;
                        }
                        else
                        {
                            lblMensagem.Text = "Selecione uma Regional.";
                        }
                        break;
                }

                if (carrega)
                {
                    grdProrrogacaoPrazoConfirmacao.DataSource = dt;
                    grdProrrogacaoPrazoConfirmacao.DataBind();
                    pnlProrrogacaoPrazoConfirmacao.Visible = true;

                    if (grdProrrogacaoPrazoConfirmacao.VisibleRowCount > 0)
                    {
                        btnSalvar.Visible = true;
                    }
                    else
                    {
                        lblMensagem.Text = "Não existem convocações pendentes para o filtro selecionado.";
                        btnSalvar.Visible = false;
                    }
                }
                else
                {
                    grdProrrogacaoPrazoConfirmacao.DataSource = null;
                    grdProrrogacaoPrazoConfirmacao.DataBind();
                    pnlProrrogacaoPrazoConfirmacao.Visible = false;
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
                CarregaGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Matriculas.OpcaoInscricao rnOpcaoInscricao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
                DadosProrrogacaoPrazoConfirmacao dadosProrrogacaoPrazoConfirmacao = new DadosProrrogacaoPrazoConfirmacao();
                ValidacaoDados validacao = new ValidacaoDados();

                if (rblTipoFiltro.SelectedValue == "porRegional")
                {
                    dadosProrrogacaoPrazoConfirmacao.PorRegional = true;
                    if (!this.tseRegional.DBValue.IsNull && this.tseRegional.IsValidDBValue)
                    {
                        dadosProrrogacaoPrazoConfirmacao.Regional = Convert.ToInt32(tseRegional.DBValue);
                    }
                }
                if (rblTipoFiltro.SelectedValue == "porMunicipio")
                {
                    dadosProrrogacaoPrazoConfirmacao.PorMunicipio = true;
                    if (!this.tseMunicipio.DBValue.IsNull && this.tseMunicipio.IsValidDBValue)
                    {
                        dadosProrrogacaoPrazoConfirmacao.Municipio = tseMunicipio.DBValue.ToString();
                    }
                }
                if (rblTipoFiltro.SelectedValue == "porUnidade")
                {
                    dadosProrrogacaoPrazoConfirmacao.PorUnidadeEnsino = true;
                    if (!this.tseUnidade.DBValue.IsNull && this.tseUnidade.IsValidDBValue)
                    {
                        dadosProrrogacaoPrazoConfirmacao.UnidadeEnsino = tseUnidade.DBValue.ToString();
                    }
                }
                if (rblTipoFiltro.SelectedValue == "todos")
                {
                    dadosProrrogacaoPrazoConfirmacao.Todos = true;
                }


                int numero;
                dadosProrrogacaoPrazoConfirmacao.Dias = txtDias.Text.IsNullOrEmptyOrWhiteSpace() || !Int32.TryParse(txtDias.Text.Trim(), out numero) ? -1 : Convert.ToInt32(txtDias.Text);
                dadosProrrogacaoPrazoConfirmacao.UsuarioResponsavel = User.Identity.Name;

                validacao = rnOpcaoInscricao.ValidaProrrogacaoPrazo(dadosProrrogacaoPrazoConfirmacao);
                if (validacao.Valido)
                {
                    rnOpcaoInscricao.ProrrogaPrazo(dadosProrrogacaoPrazoConfirmacao);
                    lblMensagem.Text = "Prazo Alterado com sucesso.";
                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

                CarregaGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdProrrogacaoPrazoConfirmacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregaGrid();
        }

        protected void grdProrrogacaoPrazoConfirmacao_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGrid();
        }
    }
}
