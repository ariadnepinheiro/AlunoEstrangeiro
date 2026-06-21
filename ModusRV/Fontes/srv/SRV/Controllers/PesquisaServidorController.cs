using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Common.Exceptions;

namespace SRV.Controllers
{
    public class PesquisaServidorController : BaseController
    {
        //
        // GET: /PesquisaServidor/

        public ActionResult Index(int? idRegional, int? idUnidadeAdministrativa, int? idAnoReferencia)
        {
            PesquisaServidor pesquisa = new PesquisaServidor();

            pesquisa.IdRegionalDefault = idRegional;

            pesquisa.IdUnidadeAdministrativaDefault = idUnidadeAdministrativa;

            pesquisa.IdAnoReferencia = idAnoReferencia;

            return PartialView(pesquisa);
        }

        [HttpPost]
        public ActionResult Index(PesquisaServidor pesquisa, int? page)
        {
            try
            {
                ServidorService servidorService = new ServidorService();

                pesquisa = servidorService.ListPesquisa(pesquisa, page ?? 1, 5, UsuarioLogado);
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return PartialView(pesquisa);
        }

        [HttpGet]
        public JsonResult FindServidor(int idServidor, int? idRegional, int? idUnidadeAdministrativa, int idAnoReferencia)
        {
            ServidorService servidorService = new ServidorService();
            return Json(new { Result = servidorService.FindServidor(idServidor, idRegional, idUnidadeAdministrativa, idAnoReferencia, UsuarioLogado) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FindServidorById(int idServidor)
        {
            ServidorService servidorService = new ServidorService();
            return Json(new { Result = servidorService.FindServidor(idServidor) }, JsonRequestBehavior.AllowGet);
        }

    }
}
