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

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/RenovacaoAutomatica.aspx"),
  ControlText("Unidades com Renovação Automática"),
  Title("Unidades com Renovação Automática")]
    public partial class RenovacaoAutomatica : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    LimpaTela();
                    CarregaGrid();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdRenovacaoAutomatica, "Unidade(s) de Ensino com Renovação Automática");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            ControlaAcesso(btnExcluir, AcaoControle.excluir);
        }

        protected void rblTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlUnidade.Visible = false;
            pnlRegional.Visible = false;
            tseRegional.ResetValue();
            tseUnidade.ResetValue();
            btnSalvar.Visible = false;
            if (rblTipoFiltro.SelectedValue == "porRegional")
            {
                pnlRegional.Visible = true;
            }
            else if (rblTipoFiltro.SelectedValue == "porUnidade")
            {
                pnlUnidade.Visible = true;
            }
            CarregaGrid();
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
                if (!this.tseRegional.DBValue.IsNull)
                {
                    if (this.tseRegional.IsValidDBValue)
                    {
                        btnSalvar.Visible = true;                        
                    }
                    else
                    {
                        lblMensagem.Text = "Regional não cadastrada.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar uma regional.";
                }

                CarregaGrid();
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
                if (!this.tseUnidade.DBValue.IsNull)
                {
                    if (this.tseUnidade.IsValidDBValue)
                    {
                        btnSalvar.Visible = true;                      
                    }
                    else
                    {
                        lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar uma unidade de ensino.";
                }

                CarregaGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            RN.RenovacaoMatricula.UnidadeEnsinoRenovacaoAutomatica rnUERenovacaoAutomatica = new RN.RenovacaoMatricula.UnidadeEnsinoRenovacaoAutomatica();
            List<int> selectedItems = new List<int>();
            try
            {
                selectedItems = this.grdRenovacaoAutomatica
                    .GetSelectedFieldValues("UNIDADEENSINORENOVACAOAUTOMATICAID")
                     .Select(x => int.Parse(x.ToString()))
                   .ToList();

                if (selectedItems.Count() > 0)
                {
                    rnUERenovacaoAutomatica.Remove(selectedItems);

                    lblMensagem.Text = "Unidade(s) de ensino excluída(s) com sucesso.";

                    CarregaGrid();
                    if (grdRenovacaoAutomatica.VisibleRowCount > 0)
                    {
                        btnExcluir.Visible = true;
                    }

                    else
                    {
                        btnExcluir.Visible = false;
                    }
                    grdRenovacaoAutomatica.Selection.UnselectAll();
                }
                else
                {
                    lblMensagem.Text = "Para excluir é necessário selecionar pelo menos uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaTela()
        {
            rblTipoFiltro.ClearSelection();
            tseRegional.ResetValue();
            tseUnidade.ResetValue();
            pnlUnidade.Visible = false;
            pnlRegional.Visible = false;
            pnlRenovacaoAutomatica.Visible = false;
        }

        private void CarregaGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                RN.RenovacaoMatricula.UnidadeEnsinoRenovacaoAutomatica rnUERenovacaoAutomatica = new RN.RenovacaoMatricula.UnidadeEnsinoRenovacaoAutomatica();

                if (tseUnidade.DBValue.IsNull && tseRegional.DBValue.IsNull)
                { 
                    dt = rnUERenovacaoAutomatica.ObtemLista();
                }

                if ((!this.tseUnidade.DBValue.IsNull && this.tseUnidade.IsValidDBValue) && tseRegional.DBValue.IsNull)
                {
                    dt = rnUERenovacaoAutomatica.ObtemListaPor(tseUnidade.DBValue.ToString());                
                }
                if ((!this.tseRegional.DBValue.IsNull && this.tseRegional.IsValidDBValue) && tseUnidade.DBValue.IsNull)
                {
                    dt = rnUERenovacaoAutomatica.ObtemListaPor(Convert.ToInt32(tseRegional.DBValue));
                }

                grdRenovacaoAutomatica.DataSource = dt;
                grdRenovacaoAutomatica.DataBind();
                pnlRenovacaoAutomatica.Visible = true;
                if (grdRenovacaoAutomatica.VisibleRowCount > 0)
                {
                    btnExcluir.Visible = true;
                }

                else
                {
                    btnExcluir.Visible = false;
                }

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
                RN.RenovacaoMatricula.UnidadeEnsinoRenovacaoAutomatica rnUERenovacaoAutomatica = new RN.RenovacaoMatricula.UnidadeEnsinoRenovacaoAutomatica();
                DadosRenovacaoAutomatica dadosRenovacaoAutomatica = new DadosRenovacaoAutomatica();
                ValidacaoDados validacao = new ValidacaoDados();

                dadosRenovacaoAutomatica.UsuarioResponsavel = User.Identity.Name;
                if (rblTipoFiltro.SelectedValue == "porRegional")
                {
                    dadosRenovacaoAutomatica.PorRegional = true;
                    if (!this.tseRegional.DBValue.IsNull && this.tseRegional.IsValidDBValue)
                    {
                        dadosRenovacaoAutomatica.Regional = Convert.ToInt32(tseRegional.DBValue);
                    }
                }
                if (rblTipoFiltro.SelectedValue == "porUnidade")
                {
                    dadosRenovacaoAutomatica.PorUnidadeEnsino = true;
                    if (!this.tseUnidade.DBValue.IsNull && this.tseUnidade.IsValidDBValue)
                    {
                        dadosRenovacaoAutomatica.UnidadeEnsino = tseUnidade.DBValue.ToString();
                    }
                }

                validacao = rnUERenovacaoAutomatica.Valida(dadosRenovacaoAutomatica);

                if (validacao.Valido)
                {
                    rnUERenovacaoAutomatica.Insere(dadosRenovacaoAutomatica);
                    lblMensagem.Text = "Unidade de Ensino incluída com sucesso.";
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

        protected void grdRenovacaoAutomatica_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRenovacaoAutomatica);
            CarregaGrid();
        }

        protected void grdRenovacaoAutomatica_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGrid();
        }
    }
}
