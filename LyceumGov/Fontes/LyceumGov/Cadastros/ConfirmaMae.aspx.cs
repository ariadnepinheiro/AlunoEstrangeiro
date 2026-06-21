using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;
namespace Techne.Lyceum.Net.Cadastros
{
    [NavUrl("~/Cadastros/ConfirmaMae.aspx")]
    [ControlText("Confirmação")]
    [Title("Confirmação")]

    public partial class ConfirmaMae : TPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;

                if (!this.IsPostBack)
                {
                    LimparTela();
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        if (Request.QueryString["Chave"] != null)
                        {
                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["chave"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                            ObterQueryString(decodedText);

                            if (!hdnMaeInscricaoId.Value.IsNullOrEmptyOrWhiteSpace())
                            {
                                CarregaMotivo();
                                divPrincipal.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Para visualizar os dados é necessario escolher uma candidata.";
                    }

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparTela()
        {
            hdnMaeInscricaoId.Value = string.Empty;
            txtNome.Text = string.Empty;           
            lblUnidade.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtNome.Text = string.Empty;
            rblConfirmacao.ClearSelection();
            ddlMotivo.ClearSelection();
            btnSalvar.Visible = false;
            dtDataInicio.Text = string.Empty;


        }

        private void CarregaMotivo()
        {
            RN.Cadastros.MaeMotivoNaoHabilitado rnMaeMotivoNaoHabilitado = new Techne.Lyceum.RN.Cadastros.MaeMotivoNaoHabilitado();
            try
            {
                ddlMotivo.Items.Clear();
                ddlMotivo.DataSource = rnMaeMotivoNaoHabilitado.ListaAtivo();
                ddlMotivo.Items.Insert(0, new ListItem("Selecione", string.Empty));
                ddlMotivo.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        private void ObterQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');
            string censo = string.Empty;
            string unidade = string.Empty;

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("MaeInscricaoId") >= 0)
                {
                    hdnMaeInscricaoId.Value = Convert.ToString(dados.Substring(dados.LastIndexOf('=') + 1));
                }
                else if (dados.IndexOf("CPF") >= 0)
                {
                    txtCPF.Text = Convert.ToString(dados.Substring(dados.LastIndexOf('=') + 1));
                }
                else if (dados.IndexOf("Nome") >= 0)
                {
                    txtNome.Text = Convert.ToString(dados.Substring(dados.LastIndexOf('=') + 1));
                }
                else if (dados.IndexOf("Censo") >= 0)
                {
                    censo = Convert.ToString(dados.Substring(dados.LastIndexOf('=') + 1));
                    hdnCenso.Value = censo;
                }
                else if (dados.IndexOf("Unidade") >= 0)
                {
                    unidade = Convert.ToString(dados.Substring(dados.LastIndexOf('=') + 1));
                }

            }

            lblUnidade.Text = censo + " - " + unidade;
        }

        protected void rblConfirmacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlMotivo.Visible = false;
                pnlData.Visible = false;
                ddlMotivo.ClearSelection();
                dtDataInicio.Text = string.Empty;

                if (!rblConfirmacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    btnSalvar.Visible = true;
                    pnlMotivo.Visible = (rblConfirmacao.SelectedValue == "Nao");
                    pnlData.Visible = (rblConfirmacao.SelectedValue == "Sim");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(hdnCenso.Value);

                Response.Redirect("MaeAnalise.aspx?ChaveConfirmacao=" + Convert.ToBase64String(bytesToEncode), false);

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
                RN.Cadastros.MaeInscricao rnMaeInscricao = new Techne.Lyceum.RN.Cadastros.MaeInscricao();

                RN.Cadastros.DTOs.MaeDadosAnalise dados = new Techne.Lyceum.RN.Cadastros.DTOs.MaeDadosAnalise();

                ValidacaoDados validacao = new ValidacaoDados();
                string matriculaAluno = string.Empty;

                dados.Censo = !hdnCenso.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCenso.Value : null;
                dados.DataInicio = !dtDataInicio.Text.IsNullOrEmptyOrWhiteSpace() && rblConfirmacao.SelectedValue == "Sim" ? dtDataInicio.Date : DateTime.MinValue;

                if ( !rblConfirmacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    dados.Habilitado = (rblConfirmacao.SelectedValue == "Sim" ? true : false);
                }
                dados.MaeInscricaoId = !hdnMaeInscricaoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnMaeInscricaoId.Value) : -1;
                dados.MaeMotivoNaoHabilitadoId = !ddlMotivo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivo.SelectedValue) : -1;
                dados.UsuarioId = User.Identity.Name;               

                validacao = rnMaeInscricao.ValidaAnalise(dados);

                if (validacao.Valido)
                {
                    rnMaeInscricao.Analise(dados);

                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(txtCPF.Text.RetirarMascaraCPF());

                    Response.Redirect("~/Cadastros/MaeCadastro.aspx?ChaveConfirmacao=" + Convert.ToBase64String(bytesToEncode), false);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
