using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Techne.Web;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Techne.Lyceum.RN.CartaoEstudante.Enum;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Service;

namespace Techne.Lyceum.Net.Servico
{

    [NavUrl("~/Servico/SolicitacaoManual.aspx"),
    ControlText("SolicitacaoManual"),
    Title("Inclusão de Solicitação / Remessa"),]

    public partial class SolicitacaoManual : TPage
    {
        #region EVENTOS DE PÁGINA

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdSolicitacao, "Solicitações");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    CarregarControle(ddlOperadora.ID);
                    CarregarControle(ddlTipoSolicitacao.ID);
                    grdSolicitacao.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCriar_Click(object sender, EventArgs e)
        {
            SolicitacaoService solicitacaoService = SolicitacaoService.Instancia;
            SolicitacaoManualRequestDTO solicitacaoManual = PreencheSolicitacaoManual();

            try
            {

                SolicitacaoManualResponseDTO responseDto = solicitacaoService.GeraSolicitacaoManual(solicitacaoManual, false);

                if (!responseDto.Inseriu)
                {
                    if (responseDto.DadosValidos)
                    {
                        if (responseDto.PodeForcarGeracao)
                        {
                            lblMensagemPopup.Text = responseDto.MensagemErro;
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popup", "abrirPopupGeracaoForcada();", true);
                            updatePanelForcarGeracao.Update();
                        }
                        else
                        {
                            lblMensagem.Text = responseDto.MensagemErro;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = responseDto.MensagemErro;
                    }
                }
                else
                {                    
                    LimpaFormulario();
                    tseAluno.ResetValue();
                    grdSolicitacao.Visible = false;
                    lblMensagem.Text = "Dados enviados com sucesso.";

                }
            }
            catch(Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnForcarGeracao_Click(object sender, EventArgs e)
        {
            try
            {
                RN.CartaoEstudante.Service.SolicitacaoService solicitacaoService = RN.CartaoEstudante.Service.SolicitacaoService.Instancia;
                SolicitacaoManualRequestDTO solicitacaoManual = PreencheSolicitacaoManual();
                SolicitacaoManualResponseDTO responseDto = solicitacaoService.GeraSolicitacaoManual(solicitacaoManual, true);

                if (responseDto.Inseriu)
                {
                    LimpaFormulario();
                    tseAluno.ResetValue();
                    grdSolicitacao.Visible = false;
                    lblMensagem.Text = "Dados enviados com sucesso.";
                }
                else
                {
                    lblMensagem.Text = responseDto.MensagemErro;
                }
            }
            catch(Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdSolicitacao_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGrid();
        } 

        #endregion

        #region Filtros

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                RN.Aluno rnAluno = new Techne.Lyceum.RN.Aluno();
               
                pnlForm.Visible = false;
                LimpaFormulario();
                grdSolicitacao.Visible = false;

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                        CarregaGrid();

                        if (rnAluno.EhAlunoEmMunicipioBilhetagemEletronicaPor(tseAluno.DBValue.ToString()))
                        {
                            pnlForm.Visible = true;
                        }
                        else
                        {
                            lblMensagem.Text = "O munícipio a qual o aluno se encontra não consta na listagem de bilhetagem eletrônica";
                            return;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Matrícula inexistente.";
                        tseAluno.ResetValue();
                    }
                }
                else
                {
                    lblMensagem.Text = String.Empty;
                    tseAluno.ResetValue();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Métodos
        
        private void LimpaFormulario()
        {
            ddlOperadora.ClearSelection();
            ddlTipoSolicitacao.ClearSelection();
            txtMotivo.Text = String.Empty;
            lblMensagem.Text = string.Empty;
            pnlForm.Visible = false;
        }

        private SolicitacaoManualRequestDTO PreencheSolicitacaoManual()
        {
            SolicitacaoManualRequestDTO solicitacao = new SolicitacaoManualRequestDTO();
            solicitacao.Aluno = Convert.ToString(tseAluno.Value);
            solicitacao.Motivo = txtMotivo.Text;
            solicitacao.OperadoraId = ddlOperadora.SelectedIndex;
            solicitacao.TipoSolicitacaoId = ddlTipoSolicitacao.SelectedIndex;
            solicitacao.Usuario = User.Identity.Name;

            return solicitacao;
        }

        private void CarregarControle(string idDrop)
        {
            switch (idDrop.ToUpper())
            {
                case "DDLOPERADORA":
                    {
                        ddlOperadora.DataSource = RN.CartaoEstudante.Service.OperadoraService.Instancia.ListaTodos();
                        ddlOperadora.DataBind();
                        ddlOperadora.DataTextField = "NomeOperadora";
                        ddlOperadora.DataValueField = "OperadoraId";
                        ListItem item = new ListItem("<Selecione a operadora>", "");
                        ddlOperadora.Items.Insert(0, item);
                        break;
                    }

                case "DDLTIPOSOLICITACAO":
                    {
                        ddlTipoSolicitacao.DataSource = RN.CartaoEstudante.Service.TipoSolicitacaoService.Instancia.ListaTodos();
                        ddlTipoSolicitacao.DataBind();
                        ddlTipoSolicitacao.DataTextField = "Descricao";
                        ddlTipoSolicitacao.DataValueField = "TipoSolicitacaoId";
                        ListItem item = new ListItem("<Selecione o tipo de solicitação>", "");
                        ddlTipoSolicitacao.Items.Insert(0, item);
                        break;
                    }
                default:
                    break;
            }
        }

        private void CarregaGrid()
        {
            try
            {
                SolicitacaoService rnSolicitacaoService = SolicitacaoService.Instancia;
                grdSolicitacao.DataSource = rnSolicitacaoService.ListaSolicitacoesPor(tseAluno.Text);
                grdSolicitacao.DataBind();

                if (grdSolicitacao.VisibleRowCount > 0)
                {
                    grdSolicitacao.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Não existe solicitação para o aluno informado.";
                    grdSolicitacao.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion
    }
}
