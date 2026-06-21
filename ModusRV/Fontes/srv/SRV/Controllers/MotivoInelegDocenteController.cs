using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.Domain;
using SRV.Models.Service;
using SRV.Models.DTO;

namespace SRV.Controllers
{
    public class MotivoInelegDocenteController: BaseController
    {
        public ActionResult Index(int idServidor, int idUnidade, int idAnoReferencia, int idMotivo)
        {
            FiltroMotivoInelegDocente filtro = new FiltroMotivoInelegDocente();
            ServidorService servidorService = new ServidorService();

            try
            {
                filtro.Servidor = servidorService.FindServidor(idServidor);
                filtro.TipoMotivo = (MotivoInelegibilidade.TipoMotivo)idMotivo;

                switch (filtro.TipoMotivo)
                {
                    case MotivoInelegibilidade.TipoMotivo.LancamentoNotaDocente:
                        LancamentoNotaDocenteService lancamentoNotaDocenteService = new LancamentoNotaDocenteService();
                        filtro.LancamentosNotasDocentes = lancamentoNotaDocenteService.List(idAnoReferencia, idServidor, idUnidade);
                        break;

                    case MotivoInelegibilidade.TipoMotivo.DenunciaAvaliacaoExterna:
                        DenunciaAvaliacaoExternaService denunciaAvaliacaoExternaService = new DenunciaAvaliacaoExternaService();
                        filtro.DenunciasAvaliacoesExternas = denunciaAvaliacaoExternaService.List(idAnoReferencia, idServidor, idUnidade);
                        break;

                    case MotivoInelegibilidade.TipoMotivo.AplicacaoProvaAvaliacaoExterna:
                        AplicacaoProvaAvaliacaoExternaService aplicacaoProvaAvaliacaoExternaService = new AplicacaoProvaAvaliacaoExternaService();
                        filtro.AplicacoesProvasAvaliacoesExternas = aplicacaoProvaAvaliacaoExternaService.List(idAnoReferencia, idServidor, idUnidade);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            

            return View(filtro);
        }
    }
}