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

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/SuspenderAluno.aspx"),
  ControlText("Suspender Aluno"),
  Title("Suspender Alunos")]

    public partial class SuspenderAluno : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    LimpaTela();
                    CarregaAno();
                    btnBuscar.Visible = false;
                    btnSuspender.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdAlunos, string.Empty);
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnBuscar, AcaoControle.novo);
            ControlaAcesso(btnSuspender, AcaoControle.novo);
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaPeriodo(Convert.ToInt32(ddlAno.SelectedValue));
                }               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaAno()
        {
            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = RN.PeriodoLetivo.ConsultarAnoSuspensao();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }


        private void CarregaPeriodo(int ano)
        {
            ddlPeriodo.Items.Clear();

            if (ano > 0)
            {
                ListItem item = new ListItem("Selecione", string.Empty);
                ddlPeriodo.DataSource = RN.PeriodoLetivo.ListarPeriodo(ano.ToString());
                ddlPeriodo.DataBind();
                ddlPeriodo.Items.Insert(0, item);
            }
        }


        protected void rblTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlUnidade.Visible = false;
            pnlRegional.Visible = false;
            tseRegional.ResetValue();
            tseUnidade.ResetValue();
            btnBuscar.Visible = false;
            btnSuspender.Visible = false;

            if (rblTipoFiltro.SelectedValue == "porRegional")
            {
                pnlRegional.Visible = true;
            }
            else if (rblTipoFiltro.SelectedValue == "porUnidade")
            {
                pnlUnidade.Visible = true;
            }
            else
            {
                btnBuscar.Visible = true;
            }
        }

        protected void tseRegional_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                tseUnidade.ResetValue();
                pnlUnidade.Visible = false;
                pnlAlunos.Visible = false;
                btnBuscar.Visible = false;
                btnSuspender.Visible = false;

                if (Page.IsCallback)
                {
                    return;
                }
                if (!this.tseRegional.DBValue.IsNull)
                {
                    if (this.tseRegional.IsValidDBValue)
                    {
                        btnBuscar.Visible = true;
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

                tseRegional.ResetValue();
                pnlRegional.Visible = false;
                pnlAlunos.Visible = false;
                btnBuscar.Visible = false;
                btnSuspender.Visible = false;

                if (Page.IsCallback)
                {
                    return;
                }
                if (!this.tseUnidade.DBValue.IsNull)
                {
                    if (this.tseUnidade.IsValidDBValue)
                    {
                        btnBuscar.Visible = true;
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
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                btnSuspender.Visible = false;

                if (!rblTipoFiltro.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaGrid();

                    lblTotalAlunos.Text = grdAlunos.VisibleRowCount.ToString();

                    if (grdAlunos.VisibleRowCount > 0)
                    {
                        btnSuspender.Visible = true;
                    }
                    else
                    {
                        lblMensagem.Text = "Não tem aluno para este filtro";
                    }
                }
                else
                {
                    lblMensagem.Text = "Para buscar é necessário escolher uma opção do filtro.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSuspender_Click(object sender, ImageClickEventArgs e)
        {
            RN.Turmas.HistoricoSuspensao rnHistoricoSuspensao = new Techne.Lyceum.RN.Turmas.HistoricoSuspensao();
            ValidacaoDados validacao = new ValidacaoDados(); 
            List<int> selectedItems = new List<int>();

            try
            {
                selectedItems = this.grdAlunos
                  .GetSelectedFieldValues("HISTORICOSUSPENSAOID")
                   .Select(x => int.Parse(x.ToString()))
                 .ToList();

                if (selectedItems.Count() > 0)
                {

                    validacao = rnHistoricoSuspensao.ValidaSuspendeAluno(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), selectedItems, User.Identity.Name);

                    if (validacao.Valido)
                    {

                        rnHistoricoSuspensao.SuspendeAluno(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), selectedItems, User.Identity.Name);

                        lblMensagem.Text = "Aluno(s) suspenso(s) com sucesso.";

                        CarregaGrid();
                        grdAlunos.Selection.UnselectAll();
                        lblTotalAlunos.Text = grdAlunos.VisibleRowCount.ToString();
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                   
                    if (grdAlunos.VisibleRowCount > 0)
                    {
                        btnSuspender.Visible = true;
                    }

                    else
                    {
                        btnSuspender.Visible = false;
                    }                  
                }
                else
                {
                    lblMensagem.Text = "Para suspender é necessário selecionar pelo menos um aluno.";
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
            pnlAlunos.Visible = false;
            lblTotalAlunos.Text = string.Empty;
        }

        private void CarregaGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                RN.Turmas.HistoricoSuspensao rnHistoricoSuspensao = new Techne.Lyceum.RN.Turmas.HistoricoSuspensao();

                pnlAlunos.Visible = false;

                if (!rblTipoFiltro.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {

                    if (tseUnidade.DBValue.IsNull && tseRegional.DBValue.IsNull)
                    {
                        dt = rnHistoricoSuspensao.ListaAlunoParaSuspenderPor(Convert.ToInt32(ddlAno.SelectedValue),Convert.ToInt32(ddlPeriodo.SelectedValue), string.Empty, 0);
                    }

                    if ((!this.tseUnidade.DBValue.IsNull && this.tseUnidade.IsValidDBValue) && tseRegional.DBValue.IsNull)
                    {
                        dt = rnHistoricoSuspensao.ListaAlunoParaSuspenderPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), tseUnidade.DBValue.ToString(), 0);
                    }
                    if ((!this.tseRegional.DBValue.IsNull && this.tseRegional.IsValidDBValue) && tseUnidade.DBValue.IsNull)
                    {
                        dt = rnHistoricoSuspensao.ListaAlunoParaSuspenderPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlPeriodo.SelectedValue), string.Empty, Convert.ToInt32(tseRegional.DBValue));
                    }
                }

                grdAlunos.DataSource = dt;
                grdAlunos.DataBind();


                if (grdAlunos.VisibleRowCount > 0)
                {
                    btnSuspender.Visible = true;
                    pnlAlunos.Visible = true;
                }
                else
                {
                    btnSuspender.Visible = false;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdAlunos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAlunos);
            CarregaGrid();
        }

        protected void grdAlunos_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGrid();
        }
    }
}
