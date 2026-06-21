using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Common.Exceptions;
using SRV.Common.Extension;
using SRV.Models.Service;
using SRV.Models.Domain;
using SRV.Filters;

namespace SRV.Controllers
{
    public class RptConsolidadoController : BaseController
    {
        //
        // GET: /RptConsolidado/

        [CustomAuthorize(Roles = "Administrador, Secretaria, Regional, Escola")]
        public ActionResult Index()
        {
            FiltroRptConsolidado filtro = new FiltroRptConsolidado();

            try
            {
                filtro = InicializaTelaFiltro(filtro);
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao carregar tela", ModelState);
            }

            ViewBag.Consolidado = true;

            return View(filtro);
        }
        
        [HttpPost]
        [CustomAuthorize(Roles = "Administrador, Secretaria, Regional, Escola")]
        public ActionResult Index(FiltroRptConsolidado filtro)
        {

            try
            {
                RptConsolidadoService service = new RptConsolidadoService(ModelState);
                filtro.Resultado = service.List(filtro, UsuarioLogado);
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao executar a consulta", ModelState);
            }

            filtro = InicializaTelaFiltro(filtro);

            ViewBag.Consolidado = true;

            return View(filtro);
            
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Administrador, Secretaria, Regional, Escola")]
        public ActionResult GeraExcel()
        {
            return Index();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador, Secretaria, Regional, Escola")]
        public ActionResult GeraExcel(FiltroRptConsolidado filtro)
        {
            byte[] result = null;

            try
            {
                RptConsolidadoService rptConsolidadoService = new RptConsolidadoService(ModelState);

                result = rptConsolidadoService.GeraExcel(Server.MapPath("~/Content/RptConsolidado.xls"), filtro, UsuarioLogado);
                
                if (ModelState.IsValid)
                {
                    Response.AppendCookie(new HttpCookie("downloadToken", filtro.DownloadToken));

                    return File(result, "application/vnd.ms-excel", "ConsolidadoRV.xls");
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao exportar relatório", ModelState);
            }

            filtro = InicializaTelaFiltro(filtro);

            return View("Index", filtro);
        }

        private FiltroRptConsolidado InicializaTelaFiltro(FiltroRptConsolidado filtro)
        {
            int idAnoReferencia = UsuarioLogado.Ciclo;

            if (filtro.IdAnoReferencia != null)
                idAnoReferencia = filtro.IdAnoReferencia.Value;

            AnoReferenciaService anoReferenciaService = new AnoReferenciaService(ModelState);

            filtro.Referencias = anoReferenciaService.List().ToSelectList<AnoReferencia>(o => o.IdAnoReferencia.Value, o => o.IdAnoReferencia.Value.ToString(), idAnoReferencia);

            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();

            var listaRegional = unidadeAdministrativaService.ListRegional(idAnoReferencia, UsuarioLogado);

            filtro.Regionais = listaRegional.ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);

            if ((UsuarioLogado.Perfil == Perfil.Regional || UsuarioLogado.Perfil == Perfil.Escola) && listaRegional != null && listaRegional.Count > 0)
            {
                filtro.IdRegional = listaRegional.FirstOrDefault().IdUnidadeAdministrativa;
            }

            if (filtro.IdRegional != null)
            {
                var listaEscolas = unidadeAdministrativaService.List(idAnoReferencia, filtro.IdRegional.Value, UsuarioLogado);
                filtro.UnidadesAdministrativas = listaEscolas.ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);
                if ((UsuarioLogado.Perfil == Perfil.Escola) && listaEscolas != null && listaEscolas.Count > 0)
                {
                    filtro.IdUnidadeAdministrativa = listaRegional.FirstOrDefault().IdUnidadeAdministrativa;
                }
            }

            return filtro;
        }

        [HttpGet]
        public JsonResult GetRegionais(int idAnoReferencia)
        {
            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();

            return Json(new { Result = unidadeAdministrativaService.ListRegional(idAnoReferencia, UsuarioLogado) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUnidadeAdministrativa(int idAnoReferencia, int idRegional)
        {
            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();

            return Json(new { Result = unidadeAdministrativaService.List(idAnoReferencia, idRegional, UsuarioLogado) }, JsonRequestBehavior.AllowGet);
        }
    }
}

