using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Techne.Lyceum.RN.CartaoEstudante.Service;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Util;
using CartaoEstudante.WebServices.Properties;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.CartaoEstudante.Enum;

namespace CartaoEstudante.WebServices
{
    /// <summary>
    /// Summary description for CartaoEstudanteRetorno
    /// </summary>
    [WebService(Namespace = "http://aplicacoes.educacao.rj.gov.br/cartaoestudante")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSRetorno : System.Web.Services.WebService
    {
        private const string nomeProcessoRetorno = "wsretorno";

        [WebMethod]
        public RetornoLoginDTO AtualizaLogin(string matricula, string idBeneficiario, string dataConfirmacaoAluno, string login, string codOperadora)
        {
            Auditoria.AuditaWebServicePor("WSRetorno - AtualizaLogin");

            DateTime dataInicioProcessamento, dataFimProcessamento;
            ProcessoService processoService = new ProcessoService();
            RetornoLoginDTO dtoRetorno = new RetornoLoginDTO();
            string tituloLog;

            try
            {
                if (processoService.PodeProcessar(nomeProcessoRetorno, DateTime.Now.TimeOfDay))
                {
                    dataInicioProcessamento = DateTime.Now;
                    RetornoLoginService retornoLoginService = RetornoLoginService.Instancia;
                    dtoRetorno = retornoLoginService.AtualizaLogin(matricula, idBeneficiario, dataConfirmacaoAluno, login, codOperadora);
                    dataFimProcessamento = DateTime.Now;

                    processoService.GravarExecucao(nomeProcessoRetorno, dataInicioProcessamento, dataFimProcessamento, StatusExecucaoEnum.OK, null, null);
                }
                else
                {
                    dtoRetorno.Criticas.Add(new CriticaDTO { Codigo = "09", Descricao = "Processamento fora do horário permitido." });
                    dataInicioProcessamento = dataFimProcessamento = DateTime.Now;
                    processoService.GravarExecucao(nomeProcessoRetorno, dataInicioProcessamento, dataFimProcessamento, StatusExecucaoEnum.ERRO, null, "Processamento fora do horário permitido.");
                }
            }
            catch
            {
                string erro = "Erro não identificado na Aplicação.";
                dataInicioProcessamento = dataFimProcessamento = DateTime.Now;                
                processoService.GravarExecucao(nomeProcessoRetorno, dataInicioProcessamento, dataFimProcessamento, StatusExecucaoEnum.ERRO, null, erro);
                dtoRetorno.Criticas.Add(new CriticaDTO { Codigo = "03", Descricao = erro });
            }
            finally
            {
                if (!dtoRetorno.Criticas.FirstOrDefault().Codigo.Equals("00"))
                {
                    tituloLog = String.Format(@"### Request: Matricula: {0}, Id do Beneficiario: {1}, Data de Confirmação: {2}, Login: {3}, Código da Operadora: {4} ###"
                                              , matricula
                                              , idBeneficiario
                                              , dataConfirmacaoAluno
                                              , login
                                              , codOperadora);
                    Log.Adicionar(Settings.Default.CaminhoLogAtualizaLogin, Settings.Default.ExtensaoLog, tituloLog, dtoRetorno.Criticas.Select(x => x.Codigo.ToString() + " - " + x.Descricao).ToArray());
                }
            }

            return dtoRetorno;            
        }
    }    
}
