using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN;
using System.Configuration;

namespace Techne.Lyceum.Net.Academico
{
    public partial class ConsultaCartao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string matricula = User.Identity.Name;

                var dadosAluno = Aluno.Carregar(matricula);

                var validacao = Aluno.ValidarDadosCartao(dadosAluno);

                if (!validacao.Valido)
                {
                    lblSituacao.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                }
                else
                {
                    var dados = CartaoAluno.UltimaSituacao(matricula);

                    if (!string.IsNullOrEmpty(dados.Passo))
                    {
                        if (dados.Passo == "1" && dados.Status == "Executado")
                        {
                            lblSituacao.Text = "Seus dados já foram enviados para a emissão do cartão. Aguarde " + ConfigurationManager.AppSettings["QtdeDiasCartaoPasso1"] + " dias e volte a consultar o sistema para acompanhar o andamento da solicitação.";
                        }
                        if (dados.Passo == "1" && dados.Status == "Cancelado")
                        {
                            lblSituacao.Text = "Seus dados não foram enviados para emissão do cartão. Compareça à secretaria de sua unidade escolar para pedir a regularização da situação.";
                        }
                        if (dados.Passo == "2" && dados.Status == "Executado")
                        {
                            lblSituacao.Text = "Seu cartão foi emitido com sucesso e deverá chegar a sua unidade escolar em um prazo máximo de " + ConfigurationManager.AppSettings["QtdeDiasCartaoPasso2"] + " dias.";
                        }
                        if (dados.Passo == "2" && dados.Status == "Cancelado")
                        {
                            lblSituacao.Text = "Seu cartão não pôde ser emitido. Compareça à secretaria de sua unidade escolar para pedir a regularização da situação.";
                        }
                        if (dados.Passo == "3" && dados.Status == "Executado")
                        {
                            lblSituacao.Text = "Seu cartão foi emitido com sucesso e entregue em " + dados.Data.Value.ToString("dd/MM/yyyy") + ". Dirija-se à secretaria de sua unidade escolar para retirá-lo.";
                        }
                        if (dados.Passo == "3" && dados.Status == "Cancelado")
                        {
                            lblSituacao.Text = "Seu cartão foi emitido com sucesso, mas ocorreu um problema na entrega à unidade escolar. Compareça à secretaria de sua unidade escolar para pedir a regularização da situação.";
                        }
                    }
                    else
                    {
                        lblSituacao.Text = "Seus dados não foram enviados para emissão do cartão. Compareça à secretaria de sua unidade escolar para pedir a regularização da situação.";
                    }
                }
            }
            catch (Exception ex)
            {

                lblMensagem.Text = ex.Message;
            }

        }
    }
}
