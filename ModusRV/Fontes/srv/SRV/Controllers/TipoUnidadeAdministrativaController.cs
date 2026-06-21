using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SRV.Models.Service;
using SRV.Models.Domain;
using SRV.Common.Exceptions;
using SRV.Filters;

namespace SRV.Controllers
{
    public class TipoUnidadeAdministrativaController : BaseController
    {
        //
        // GET: /TipoUnidadeAdministrativa/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index(int? page)
        {
            IList<TipoUnidadeAdministrativa> tiposUnidadeAdministrativa = null;

            try
            {
                TipoUnidadeAdministrativaService tipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);
                tiposUnidadeAdministrativa = tipoUnidadeAdministrativaService.List();

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(tiposUnidadeAdministrativa);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            TipoUnidadeAdministrativa tipoUnidadeAdministrativa = new TipoUnidadeAdministrativa();

            return View(tipoUnidadeAdministrativa);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(TipoUnidadeAdministrativa tipoUnidadeAdministrativa)
        {
            try
            {
                TipoUnidadeAdministrativaService tipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);

                tipoUnidadeAdministrativa = tipoUnidadeAdministrativaService.Insert(tipoUnidadeAdministrativa, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Tipo de unidade administrativa salvo com sucesso");

                    return RedirectToAction("Edit", new { id = tipoUnidadeAdministrativa.IdTipoUnidAdm });
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar tipo de unidade administrativa", ModelState);
            }

            return View(tipoUnidadeAdministrativa);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            TipoUnidadeAdministrativa tipoUnidadeAdministrativa = new TipoUnidadeAdministrativa();

            TipoUnidadeAdministrativaService tipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);
            tipoUnidadeAdministrativa = tipoUnidadeAdministrativaService.Find(id);

            return View(tipoUnidadeAdministrativa);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(TipoUnidadeAdministrativa tipoUnidadeAdministrativa)
        {
            try
            {
                TipoUnidadeAdministrativaService TipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);

                TipoUnidadeAdministrativaService.Update(tipoUnidadeAdministrativa, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Tipo de unidade administrativa alterado com sucesso");

                    return RedirectToAction("Edit", new { id = tipoUnidadeAdministrativa.IdTipoUnidAdm });
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar tipo de unidade administrativa", ModelState);
            }

            return View(tipoUnidadeAdministrativa);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                TipoUnidadeAdministrativaService tipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);

                tipoUnidadeAdministrativaService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir tipo de unidade administrativa");
            }
        }
    }
}
