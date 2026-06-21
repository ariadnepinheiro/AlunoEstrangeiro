using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [
     NavUrl("~/ProcessoSeletivo/LiberacaoInscricaoCandidato.aspx"),
      ControlText("ExcluirInscricaoCandidato"),
      Title("Excluir Inscrição Candidato"),
    ]
    public partial class LiberacaoInscricaoCandidato : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        protected void tseCandidato_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                pnCampos.Visible = false;
                this.LimpaDadosCandidato();

                if (!tseConcurso.DBValue.IsNull && tseConcurso.IsValidDBValue)
                {
                    if (!tseCandidato.DBValue.IsNull && tseCandidato.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                        pnCampos.Visible = true;
                        txtCPF.Text = tseCandidato["CPF"].ToString();
                        txtConcurso.Text = tseCandidato["processo_seletivo"].ToString();
                        txtDataInscrição.Text = tseCandidato["DHR_CADASTRO"].ToString();
                        txtInscrição.Text = tseCandidato["CANDIDATO"].ToString();
                        txtNome.Text = tseCandidato["NOME"].ToString();
                    }
                    else
                    {
                        lblMensagem.Text = "Por favor selecionar um candidato.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Por favor selecionar um processo seletivo ativo.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnLiberar_Click(object sender, EventArgs e)
        {
            RN.LiberacaoCandidatoDocente rnLiberacaoCandidatoDocente = new Techne.Lyceum.RN.LiberacaoCandidatoDocente();
            ValidacaoDados validacao = new ValidacaoDados();

            try
            {
                validacao = rnLiberacaoCandidatoDocente.Valida(txtConcurso.Text, txtInscrição.Text);

                if (validacao.Valido)
                {
                    RN.ProcessoSeletivo.Remover(txtConcurso.Text.Trim(), txtInscrição.Text.Trim(), txtNome.Text.Trim(), txtCPF.Text.Trim(), User.Identity.Name);
                    this.LimpaTela();
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Candidato removido com sucesso.');", true);
                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaTela()
        {
            tseCandidato.ResetValue();
            this.LimpaDadosCandidato();
        }

        private void LimpaDadosCandidato()
        {
            txtCPF.Text = string.Empty;
            txtInscrição.Text = string.Empty;
            txtConcurso.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtDataInscrição.Text = string.Empty;
        }
    }
}
