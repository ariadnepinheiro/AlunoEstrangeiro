using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.DTOs;
using DevExpress.Web.ASPxGridView;
using DevExpress.Utils;
using Techne.Lyceum.RN.Util;
using System.Data;
using Techne.Lyceum.RN.CartaoEstudante.DTO.Filter;

namespace Techne.Lyceum.Net.Servico
{
    [NavUrl("~/Servico/ConsultarMatriculasDuplicadas.aspx"), ControlText("ConsultarMatriculasDuplicadas"), Title("Consulta Matrículas Duplicadas")]

    public partial class ConsultarMatriculasDuplicadas : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                grdDuplicidade.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDuplicidade, "Consulta Matrículas Duplicadas");
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                        tseRegional.ResetValue();
                        tseUnidadeEnsino.ResetValue();
                    }
                    else
                    {
                        lblMensagem.Text = "Matrícula inexistente.";
                    }
                }
                else
                {
                    lblMensagem.Text = string.Empty;
                    tseAluno.ResetValue();
                }

                grdDuplicidade.Visible = false;
                updPnl.Update();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDuplicidade_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGrid();
        }

        #region Filtros

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue)
                {
                    tseUnidadeEnsino.ResetValue();
                    tseAluno.ResetValue();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeEnsino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (!tseUnidadeEnsino.DBValue.IsNull && tseUnidadeEnsino.IsValidDBValue)
                {
                    tseRegional.ResetValue();
                    tseAluno.ResetValue();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            CarregaGrid();
        }

        #endregion

        private void CarregaGrid()
        {
            try
            {
                RN.CartaoEstudante.Service.DuplicidadeMatriculaService rnDuplicidadeService = RN.CartaoEstudante.Service.DuplicidadeMatriculaService.Instancia;

                MatriculaDuplicadaFilterDTO filtro = new MatriculaDuplicadaFilterDTO();

                if (!String.IsNullOrEmpty(tseAluno.Text))
                {
                    filtro.Aluno = rnDuplicidadeService.obtemIdBeneficiario(tseAluno.Text);
                }

                filtro.UnidadeEnsino = Convert.ToString(tseUnidadeEnsino.Value);
                filtro.Regional = Convert.ToString(tseRegional.Value);


                grdDuplicidade.DataSource = rnDuplicidadeService.ListaDuplicidades(filtro);
                grdDuplicidade.DataBind();

                if (grdDuplicidade.VisibleRowCount > 0)
                {
                    grdDuplicidade.Visible = true;
                    updPnl.Update();
                }
                else
                {
                    lblMensagem.Text = "Não existem matrículas duplicadas para os parâmetros de pesquisa informados.";
                    grdDuplicidade.Visible = false;
                    updPnl.Update();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDuplicidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregaGrid();
            grdDuplicidade.Visible = true;
        }

    }
}
