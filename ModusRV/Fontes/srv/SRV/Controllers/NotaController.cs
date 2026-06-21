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
    public class NotaController : BaseController
    {
        //
        // GET: /Nota/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            IList<Nota> notas = null;

            try
            {
                NotaService notaService = new NotaService(ModelState);
                notas = notaService.List(UsuarioLogado.Ciclo);

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(notas);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            Nota nota = new Nota();

            return View(nota);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(Nota nota)
        {
            try
            {
                NotaService notaService = new NotaService(ModelState);

                nota = notaService.Insert(nota, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Nota salva com sucesso");

                    return RedirectToAction("Edit", new { id = nota.IdNota });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar nota", ModelState);
            }

            return View(nota);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            Nota nota = new Nota();

            NotaService notaService = new NotaService(ModelState);
            nota = notaService.Find(id);

            return View(nota);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(Nota nota)
        {
            try
            {
                NotaService notaService = new NotaService(ModelState);

                notaService.Update(nota, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Nota alterada com sucesso");

                    return RedirectToAction("Edit", new { id = nota.IdNota });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar nota", ModelState);
            }

            return View(nota);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                NotaService notaService = new NotaService(ModelState);

                notaService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir nota");
            }
        }
    }
}
