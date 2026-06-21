using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Servico
{
    [NavUrl("~/Servico/AcompanhamentoStatusFoto.aspx"), ControlText("AcompanhamentoStatusFoto"), Title("Acompanhamento Status Foto")]

    public partial class AcompanhamentoStatusFoto : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
               
                if (!IsPostBack)
                {
                    LimpaTela();
                    pnlFiltroAluno.Visible = false;
                    pnlFiltroEscola.Visible = false;
                    grdProcessamentoRemessa.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProcessamentoRemessa, "Situação de Processamento da Foto");
        }

        private void LimpaTela()
        {
            rblTipoFiltro.ClearSelection();
            tseAluno.ResetValue();
            tseRegional.ResetValue();
            tseMunicipio.ResetValue();
            tseUnidadeEnsino.ResetValue();
        }

        protected void rblTipoFiltro_IndexChanged(object sender, EventArgs e)
        {  
            grdProcessamentoRemessa.Visible = false;
            pnlFiltroAluno.Visible = false;
            pnlFiltroEscola.Visible = false;
            btnPesquisar.Visible = false;

            if (!rblTipoFiltro.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                btnPesquisar.Visible = true;
                if (rblTipoFiltro.SelectedValue == "Aluno")
                {
                    pnlFiltroAluno.Visible = true;
                    tseAluno.ResetValue();
                }
                else
                {
                    pnlFiltroEscola.Visible = true;
                    tseRegional.ResetValue();
                    tseMunicipio.ResetValue();
                    tseUnidadeEnsino.ResetValue();
                }
            }          
        }

        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblMensagem.Text = String.Empty;
                grdProcessamentoRemessa.Visible = false;

                if (EscolheuFiltro())
                {
                    grdProcessamentoRemessa.DataSource = null;
                    grdProcessamentoRemessa.PageIndex = 0;
                    grdProcessamentoRemessa.DataBind();                 
                    
                    CarregaGrid();
                }
                else
                {
                    lblMensagem.Text = "Escolha uma opção de filtro.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaGrid()
        {
            try
            {
                Techne.Lyceum.RN.CartaoEstudante.WsStatusFoto rnWsStatusFoto = new Techne.Lyceum.RN.CartaoEstudante.WsStatusFoto();

                string aluno = string.Empty;
                string censo;

                aluno = (!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue) ? tseAluno.DBValue.ToString() : null;
                censo = (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue) ? tseUnidadeEnsino.DBValue.ToString() : null;


                grdProcessamentoRemessa.DataSource = rnWsStatusFoto.ObtemListaPor(aluno, censo);
                grdProcessamentoRemessa.DataBind();

                if (grdProcessamentoRemessa.VisibleRowCount > 0)
                {
                    grdProcessamentoRemessa.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Não existem retorno para os parâmetros de pesquisa informados.";
                    grdProcessamentoRemessa.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdProcessamentoRemessa_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGrid();
        }  

        private bool EscolheuFiltro()
        {
            bool escolheuFiltro = false;

            if (!rblTipoFiltro.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                if ((!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue)
                    || (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue)
                    || (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
                    || (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue)
                    )
                    escolheuFiltro = true;
            }
            return escolheuFiltro;
        }


        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }

            grdProcessamentoRemessa.Visible = false;

            if (!tseAluno.DBValue.IsNull)
            {
                if (tseAluno.IsValidDBValue)
                {
                    lblMensagem.Text = string.Empty;
                    tseRegional.ResetValue();
                    tseMunicipio.ResetValue();
                    tseUnidadeEnsino.ResetValue();
                }
                else
                {
                    lblMensagem.Text = "Matrícula inexistente.";
                    tseAluno.ResetValue();
                }
            }
            else
            {
                lblMensagem.Text = "Matrícula inexistente.";
                tseAluno.ResetValue();
            }
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            grdProcessamentoRemessa.Visible = false;
            try
            {
                if (!tseRegional.DBValue.IsNull)
                {
                    if (tseRegional.IsValidDBValue)
                    {
                        tseUnidadeEnsino.ResetValue();
                        tseMunicipio.ResetValue();
                        tseAluno.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            grdProcessamentoRemessa.Visible = false;
            try
            {
                if (!tseMunicipio.DBValue.IsNull)
                {
                    if (tseMunicipio.IsValidDBValue)
                    {
                        tseUnidadeEnsino.ResetValue();
                        tseRegional.ResetValue();
                        tseAluno.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            grdProcessamentoRemessa.Visible = false;
            tseMunicipio.ResetValue();
            tseRegional.ResetValue();
            tseAluno.ResetValue();
            try
            {
                if (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue)
                {
                    tseRegional.Value = tseUnidadeEnsino["id_regional"];
                    tseMunicipio.Value = tseUnidadeEnsino["municipio"];
                                   
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
