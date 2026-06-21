using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Filters;
using SRV.Models.Domain;
using SRV.Models.Service;
using SRV.Common.Exceptions;

namespace SRV.Controllers
{
    public class AnoReferenciaController : BaseController
    {
        //
        // GET: /AnoReferencia/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            IList<AnoReferencia> anosReferencia = null;

            try
            {
                AnoReferenciaService anoReferenciaService = new AnoReferenciaService(ModelState);
                anosReferencia = anoReferenciaService.List();

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(anosReferencia);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            AnoReferencia anoReferencia = new AnoReferencia();

            ViewBag.IsEdit = false;
            return View(anoReferencia);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(AnoReferencia anoReferencia)
        {
            try
            {
                AnoReferenciaService anoReferenciaService = new AnoReferenciaService(ModelState);

                anoReferencia = anoReferenciaService.Insert(anoReferencia, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Ano de referência salvo com sucesso");

                    return RedirectToAction("Edit", new { id = anoReferencia.IdAnoReferencia });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar ano de referência", ModelState);
            }

            ViewBag.IsEdit = false;
            return View(anoReferencia);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            AnoReferencia anoReferencia = new AnoReferencia();

            AnoReferenciaService anoReferenciaService = new AnoReferenciaService(ModelState);
            anoReferencia = anoReferenciaService.Find(id);

            ViewBag.IsEdit = true;
            return View(anoReferencia);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(AnoReferencia anoReferencia)
        {
            try
            {
                AnoReferenciaService anoReferenciaService = new AnoReferenciaService(ModelState);

                anoReferenciaService.Update(anoReferencia, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Ano de referência alterado com sucesso");

                    return RedirectToAction("Edit", new { id = anoReferencia.IdAnoReferencia });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar ano de referência", ModelState);
            }

            ViewBag.IsEdit = true;
            return View(anoReferencia);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                AnoReferenciaService anoReferenciaService = new AnoReferenciaService(ModelState);

                anoReferenciaService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir ano de referência");
            }
        }
    }
}
