using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Techne.Lyceum.RN.CartaoEstudante.Entities;
using Techne.Lyceum.RN.CartaoEstudante.Repository;using Techne.Lyceum.RN.CartaoEstudante.Service;
using Techne.Lyceum.RN.CartaoEstudante.Enum;
using Techne.Lyceum.RN.Util;

namespace CartaoEstudante.WebServices
{
    /// <summary>
    /// Summary description for CartaoEstudanteEnvio
    /// </summary>
    [WebService(Namespace = "http://aplicacoes.educacao.rj.gov.br/cartaoestudante")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSEnvio : System.Web.Services.WebService
    {
        private const string nomeProcessoEnvio = "wsenvio";
       
        /// <summary>
        /// Serviço que busca o total de Lotes de remessas
        /// e os lotes de remessas
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public RetornoLoteRemessa lotesremessa(int operadoraID)
        {
            Auditoria.AuditaWebServicePor("WSEnvio - LotesRemessa");

            DateTime dataInicioProcessamento, dataFimProcessamento;
            RetornoLoteRemessa _retornoLoteRemessa = new RetornoLoteRemessa();
            RepositoryLoteRemessa _repository = new RepositoryLoteRemessa();
            ProcessoService processoService = new ProcessoService();

            ControleProcessamentoService controleProcessamentoService = new ControleProcessamentoService();

            try
            {
                if (processoService.PodeProcessar(nomeProcessoEnvio, DateTime.Now.TimeOfDay))
                {
                    dataInicioProcessamento = DateTime.Now;
                    _retornoLoteRemessa.listaLoteRemessa = _repository.ListaLoteremessa(operadoraID);
                    _retornoLoteRemessa.quantidadeLotes = _repository.TotalLoteremessa(operadoraID);
                    dataFimProcessamento = DateTime.Now;

                    processoService.GravarExecucao(nomeProcessoEnvio, dataInicioProcessamento, dataFimProcessamento, StatusExecucaoEnum.OK, null, null);
                }
                else
                {
                    dataInicioProcessamento = dataFimProcessamento = DateTime.Now;  
                    _retornoLoteRemessa.quantidadeLotes = -1; //Para identificar erro
                    processoService.GravarExecucao(nomeProcessoEnvio, dataInicioProcessamento, dataFimProcessamento, StatusExecucaoEnum.ERRO, null, "Processamento fora do horário permitido.");
                }
            }
            catch
            {
                dataInicioProcessamento = dataFimProcessamento = DateTime.Now;
                _retornoLoteRemessa.quantidadeLotes = -1; //Para identificar erro
                processoService.GravarExecucao(nomeProcessoEnvio, dataInicioProcessamento, dataFimProcessamento, StatusExecucaoEnum.ERRO, null, "Erro decorrente de exceção do sistema.");
            }

            return _retornoLoteRemessa;
        }

        /// <summary>
        /// Serviço que busca o total das remessas
        /// e as remessas de um determinado Lote
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public RetornoRemessa registrosremessa(string nomeLote)
        {
            Auditoria.AuditaWebServicePor("WSEnvio - RegistrosRemessa");

            RetornoRemessa _retornoRemessa = new RetornoRemessa();
            RepositoryRemessa _repository = new RepositoryRemessa();
            RepositoryLoteRemessa _repositoryLoteRemessa = new RepositoryLoteRemessa();


            Int32 idLoteRemessa = _repositoryLoteRemessa.BuscaPorID(nomeLote);

            _retornoRemessa = _repository.TotalRemessa(idLoteRemessa, nomeLote);
            _retornoRemessa.remessa = _repository.Listaremessas(idLoteRemessa, nomeLote);


            return _retornoRemessa;
        }
    }
}
