using System;
using System.Collections.Generic;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [NavUrl("~/ProcessoSeletivo/ReimpressaoFichaInscricao.aspx"),
    ControlText("ReimpressaoFichaInscricao"),
    Title("Reimpressão da Ficha de Inscrição")]

    public partial class ReimpressaoFichaInscricao : TPage
    {
        public ReimpressaoFichaInscricao()
        {
            VerificarCompatibilidadeComIE = true;
            MensagemCompatibilidadeIE = @"Prezado candidato, a inscrição para a contratação temporária poderá ser efetivada utilizando o seu navegador padrão. <br>
                                    ";
        }

        protected override void OnInit(EventArgs args)
        {
            base.OnInit(args);
            Master.AtribuirMensagemCompatibilidadeIE(MensagemCompatibilidadeIE);
            Master.AtribuirModalOKClick(ModalOK_Click);
            HabilitaDesabilitaControles(true);
        }

        protected void ModalOK_Click(object sender, EventArgs e)
        {
            OcultarPopupModal();
        }

        private void HabilitaDesabilitaControles(bool valor)
        {
            tseConcursoBusca.Enabled = valor;
            txtCpf.Enabled = valor;
            dtDataNasc.Enabled = valor;
            vsReimpressao.Visible = valor;
            rfvConcursoBusca.Visible = valor;
            rfvCpf.Visible = valor;
            rfvDtNasc.Visible = valor;
            btnImprimir.Enabled = valor;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string textoJavaScript = string.Empty;

                if (!IsPostBack)
                {
                    InicializaComponentes();
                }
                else
                {
                    textoJavaScript = string.Concat("function AbrePopupImpressao(queryString){",
                                    string.Concat("window.open('../Relatorio/Relatorios.aspx?report=rsconcursodocenteexterno&grp=processoseletivo&' + queryString,'','directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes');}"));

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "AbrePopupImpressao", textoJavaScript, true);
                    btnImprimir.Attributes.Add("onclick", "AbrePopupImpressao");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void InicializaComponentes()
        {
            tseConcursoBusca.ResetValue();
            tseConcursoBusca.Mode = Techne.Controls.ControlMode.Edit;

            txtCpf.Text = string.Empty;
        }

        protected void Imprimir_Click(object sender, EventArgs e)
        {
            RN.CandidatoDocente rnCandidatoDocente = new RN.CandidatoDocente();
            string erroMsg = string.Empty;
            string codigoCandidato = string.Empty;

            try
            {
                ValidaCampos();

                codigoCandidato = rnCandidatoDocente.ConsultarCandidatoPorCPFeNasc(
                    tseConcursoBusca.DBValue.ToString(), txtCpf.Text.RetirarMascaraCPF(), dtDataNasc.Date);

                if (string.IsNullOrEmpty(codigoCandidato))
                    throw new Exception("Inscrição não encontrada para os dados informados.");

                Imprime(codigoCandidato);
            }
            catch (SqlException sqlex)
            {
                lblMensagem.Text = sqlex.Message;
                AtribuiMensagensDeErro(erroMsg);
                             
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                AtribuiMensagensDeErro(ex.Message);
            }
            finally
            {
                rnCandidatoDocente = null;
            }
        }

        private void Imprime(string codigoCandidato)
        {
            IDictionary<string, string> parametros = new Dictionary<string, string>();
            string textoJavaScript = string.Empty;

            parametros.Add("concurso", tseConcursoBusca.DBValue.ToString());
            parametros.Add("candidato", codigoCandidato);

            textoJavaScript = string.Concat("<script>AbrePopupImpressao('",
                string.Concat(TPage.CodificaQueryString(parametros), "');</script>"));

            Page.ClientScript.RegisterStartupScript(this.GetType(), string.Empty, textoJavaScript);            
        }

        private void AtribuiMensagensDeErro(string erroMsg)
        {
            CustomValidator cv = new CustomValidator();
            cv.IsValid = false;
            cv.ErrorMessage = erroMsg;
            cv.EnableClientScript = false;
            cv.ValidationGroup = "ReimprimirForm";

            Page.Validators.Add(cv);
        }

        private void ValidaCampos()
        {
            IList<string> erros = new List<string>();
            string msg = string.Empty;

            try
            {
                if (!tseConcursoBusca.IsValidDBValue)
                    erros.Add("Processo seletivo não informado.");

                if (string.IsNullOrEmpty(txtCpf.Text))
                    erros.Add("CPF não informado.");
                else if (!RN.Validacao.ValidaCpf(txtCpf.Text.RetirarMascaraCPF()))
                    erros.Add("CPF inválido.");

                if (string.IsNullOrEmpty(dtDataNasc.Text))
                    erros.Add("Data de nascimento não informada.");

                if (erros.Count > 0)
                {
                    foreach (string erro in erros)
                        msg += erro + "<br />";

                    throw new Exception(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
