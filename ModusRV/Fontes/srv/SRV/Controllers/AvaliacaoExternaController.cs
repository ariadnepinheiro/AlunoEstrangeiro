using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Filters;
using SRV.Common.Exceptions;
using SRV.Models.Service;
using SRV.Models.Domain;

namespace SRV.Controllers
{
    public class AvaliacaoExternaController : BaseController
    {
        //
        // GET: /AvaliacaoExterna/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            IList<AvaliacaoExterna> avaliacoes = null;

            try
            {
                AvaliacaoExternaService avaliacaoExternaService = new AvaliacaoExternaService(ModelState);
                avaliacoes = avaliacaoExternaService.List();

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(avaliacoes);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            AvaliacaoExterna avaliacaoExterna = new AvaliacaoExterna();

            ViewBag.IsEdit = false;
            return View(avaliacaoExterna);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(AvaliacaoExterna avaliacaoExterna)
        {
            try
            {
                AvaliacaoExternaService avaliacaoExternaService = new AvaliacaoExternaService(ModelState);

                avaliacaoExterna = avaliacaoExternaService.Insert(avaliacaoExterna, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Avaliação externa salva com sucesso");

                    return RedirectToAction("Edit", new { id = avaliacaoExterna.IdAvaliacaoExterna });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar avaliação externa", ModelState);
            }

            ViewBag.IsEdit = false;
            return View(avaliacaoExterna);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            AvaliacaoExterna avaliacaoExterna = new AvaliacaoExterna();

            AvaliacaoExternaService avaliacaoExternaService = new AvaliacaoExternaService(ModelState);
            avaliacaoExterna = avaliacaoExternaService.Find(id);

            ViewBag.IsEdit = true;
            return View(avaliacaoExterna);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(AvaliacaoExterna avaliacaoExterna)
        {
            try
            {
                AvaliacaoExternaService avaliacaoExternaService = new AvaliacaoExternaService(ModelState);

                avaliacaoExternaService.Update(avaliacaoExterna, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Avaliação externa alterada com sucesso");

                    return RedirectToAction("Edit", new { id = avaliacaoExterna.IdAvaliacaoExterna });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar avaliação externa", ModelState);
            }

            ViewBag.IsEdit = true;
            return View(avaliacaoExterna);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                AvaliacaoExternaService avaliacaoExternaService = new AvaliacaoExternaService(ModelState);

                avaliacaoExternaService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir avaliação externa");
            }
        }
    }
}
