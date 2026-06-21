using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;
using Proderj.DOL.WebApp.Models.ProtocoloNota;
using Resources;

namespace Proderj.DOL.WebApp.Controllers
{
    public class ProtocoloNotaController : ControllerPadrao
    {
        private readonly IProtocoloNotaService protocoloNotaServico;
        private readonly IPeriodoLetivoService periodoLetivoServico;

        public ProtocoloNotaController(IProtocoloNotaService protocoloNotaServico, IPeriodoLetivoService periodoLetivoServico)
        {
            this.protocoloNotaServico = protocoloNotaServico;
            this.periodoLetivoServico = periodoLetivoServico;
        }

        //
        // GET: /ProtocoloNota/

        public ActionResult Lista(DocenteLogadoBindModel modeloDocenteLogado)
        {
            var modelo = new ProtocoloNotaListaViewModel(modeloDocenteLogado);
            modelo.TituloDaPagina = Recurso.ProtocoloNotaLista_TituloPagina;
            modelo.CabecalhoModelo.TituloCabecalho = Recurso.ProtocoloNotaLista_TituloPagina;
            modelo.CabecalhoModelo.LinkAjuda = Url.Action("ProtocoloNota", "Ajuda");
            modelo.RegistrosPorPagina = 10;
            modelo.ListaAno = periodoLetivoServico.Lista();

            return View(modelo);
        }

        public JsonResult ListaPeriodo(short ano)
        {
            try
            {
                List<DTOPeriodoLetivo> listaPeriodo = periodoLetivoServico.ListaPor(ano);
                return Json(
                        new
                        {
                            Sucesso = true,
                            Combo = listaPeriodo.Select(periodoLetivo => new
                            {
                                Codigo = periodoLetivo.Periodo,
                                Descricao = periodoLetivo.DescricaoPeriodo
                            })
                        });


            }
            catch (System.Exception excecao)
            {
                return JsonErro(excecao.Message);
            }
        }

        public PartialViewResult ListaProtocolo(DocenteLogadoBindModel modeloDocenteLogado, short ano, short periodo)
        {
            return PartialView(protocoloNotaServico.ListaPor(modeloDocenteLogado.IdFuncional , modeloDocenteLogado.IdFuncional + "/" + modeloDocenteLogado.Vinculo, ano, periodo));
        }
    }
}
