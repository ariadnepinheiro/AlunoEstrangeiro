using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivoAluno
{
    public partial class Identificacao : TPage
    {
        Sessao.CandidatoProcessoSeletivoSessao sessaoCandidato = new Sessao.CandidatoProcessoSeletivoSessao();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!sessaoCandidato.CandidatoAcordoEdital)
            {
                Response.Redirect("Edital.aspx");
            }
        }

        protected void BtProsseguir_Click(object sender, ImageClickEventArgs e)
        {
            DataTable ProcessoSeletivoAtivo = RN.Agenda.ProcessoSeletivo.ProcessoSeletivoAtivo();
            if (ProcessoSeletivoAtivo.Rows.Count == 0)
            {
                this.txtChave.Text = string.Empty;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Não há processo seletivo em período de inscrição.')", true);
                return;
            }

            //Limpa session 
            sessaoCandidato.LimpaSessaoCandidatoInscrito();

            List<string> validacaoDadosCandidato = ValidarDadosCandidato();

            if (validacaoDadosCandidato.Count == 0)
            {
                sessaoCandidato.CandidatoLogado = true;

                if (rbTipoDadosInscricao.SelectedValue == "0")
                {
                    sessaoCandidato.NomeCandidato = txtAluno.Text.ToUpper();
                }
                else
                {
                    sessaoCandidato.NumeroInscricao = Convert.ToInt64(txtAluno.Text);
                }

                sessaoCandidato.NomeMae = txtNomeMae.Text.ToUpper();
                sessaoCandidato.DataNascimento = Convert.ToDateTime(dtDataNasc.Text);

                Response.Redirect("CandidatoInscricao.aspx");
            }
            else
            {
                this.txtChave.Text = string.Empty;
                string MensagensErro = validacaoDadosCandidato.Aggregate((x, y) => x + "\\n" + y);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" + MensagensErro + "')", true);

            }
        }

        public List<string> ValidarDadosCandidato()
        {
            var mensagens = new List<string>();
            string captchaGerado = string.Empty;

            if (rbTipoDadosInscricao.SelectedValue == "0")
            {
                if (txtAluno.Text == string.Empty)
                {
                    mensagens.Add("Nome do candidato não informado.");
                }
                else
                {
                    RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaDadosNome(txtAluno.Text, "Nome do Candidato", txtAluno.Text, ref mensagens);
                }
            }
            else
            {
                if (txtAluno.Text == string.Empty)
                {
                    mensagens.Add("Número de inscrição não informado.");
                }
            }

            if (txtNomeMae.Text == string.Empty)
            {
                mensagens.Add("Nome da mãe não informado.");
            }
            else
            {
                RN.ProcessoSeletivoAluno.ValidacaoCandidato.ValidaDadosNome(txtNomeMae.Text, "Nome da Mãe", txtAluno.Text, ref mensagens);
            }

            if (dtDataNasc.Text == string.Empty)
            {
                mensagens.Add("Data de nascimento não informada.");
            }
            else
            {
                if (!Techne.Lyceum.RN.Agenda.ProcessoSeletivo.VerificaDentroIntervaloDataNascimentoProcessoSeletivo(Convert.ToDateTime(dtDataNasc.Text), sessaoCandidato.AgendaID))
                {
                    mensagens.Add("Idade não permitida para este processo seletivo.");
                }
            }

            if (string.IsNullOrEmpty(txtChave.Text))
            {
                mensagens.Add("Código da imagem não informado.");
            }
            else
            {
                if (HttpContext.Current.Response.Cookies["CaptchaValue"] != null)
                {
                    captchaGerado = HttpContext.Current.Request.Cookies["CaptchaValue"].Value;
                }

                // Valida Captcha
                if (this.txtChave.Text != captchaGerado)
                {
                    this.txtChave.Text = string.Empty;
                    mensagens.Add("Código digitado incorreto. Digite-o novamente.");
                }
            }

            if (mensagens.Count == 0)
            {
                if (rbTipoDadosInscricao.SelectedValue == "0")
                {
                    DataTable candidato = RN.ProcessoSeletivoAluno.Candidato.VerificaCandidatoExistente(txtAluno.Text, txtNomeMae.Text, Convert.ToDateTime(dtDataNasc.Text), sessaoCandidato.AgendaID);
                    if (candidato.Rows.Count > 0)
                    {
                        sessaoCandidato.CandidatoId = Convert.ToInt32(candidato.Rows[0]["CANDIDATOID"].ToString());
                        if (candidato.Rows[0]["INSCRICAOID"] != DBNull.Value)
                            sessaoCandidato.InscricaoId = Convert.ToInt32(candidato.Rows[0]["INSCRICAOID"]);
                        if (candidato.Rows[0]["NUMEROINSCRICAO"] != DBNull.Value)
                            sessaoCandidato.NumeroInscricao = Convert.ToInt64(candidato.Rows[0]["NUMEROINSCRICAO"]);
                    }
                }
                else
                {
                    DataTable candidato = RN.ProcessoSeletivoAluno.Candidato.VerificaCandidatoExistentePorNumeroInscricao(Convert.ToInt64(txtAluno.Text), txtNomeMae.Text, Convert.ToDateTime(dtDataNasc.Text), sessaoCandidato.AgendaID);
                    if (candidato.Rows.Count > 0)
                    {
                        sessaoCandidato.CandidatoId = Convert.ToInt32(candidato.Rows[0]["CANDIDATOID"]);
                        sessaoCandidato.InscricaoId = Convert.ToInt32(candidato.Rows[0]["INSCRICAOID"]);
                    }
                    else
                    {
                        mensagens.Add("Inscrição não encontrada para os dados informados.");
                    }
                }
            }
            else
            {
                //Caso exista qualquer erro limpar Captcha
                this.txtChave.Text = string.Empty;
            }

            return mensagens;
        }

        public bool ehAbreviacaoValida(string abreviacao)
        {
            switch (abreviacao)
            {
                case "DA":
                case "DE":
                case "DI":
                case "DO":
                case "DU":
                    return true;
                default:
                    break;
            }

            return false;
        }

        protected void rbTipoDadosInscricao_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAluno.Text = string.Empty;

            if (rbTipoDadosInscricao.SelectedValue == "1")
            {
                txtAluno.MaxLength = 19;
            }
            else
            {
                txtAluno.MaxLength = 100;
            }

            txtAluno.Focus();
        }
    }
}
