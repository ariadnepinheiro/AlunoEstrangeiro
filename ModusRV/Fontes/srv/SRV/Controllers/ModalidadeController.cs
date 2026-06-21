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
    public class ModalidadeController : BaseController
    {
        //
        // GET: /Modalidade/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            IList<Modalidade> modalidades = null;

            try
            {
                ModalidadeService modalidadeService = new ModalidadeService(ModelState);
                modalidades = modalidadeService.List();

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(modalidades);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            Modalidade modalidade = new Modalidade();

            return View(modalidade);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(Modalidade modalidade)
        {
            try
            {
                ModalidadeService modalidadeService = new ModalidadeService(ModelState);

                modalidade = modalidadeService.Insert(modalidade, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Modalidade salva com sucesso");

                    return RedirectToAction("Edit", new { id = modalidade.IdModalidade });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar modalidade", ModelState);
            }

            return View(modalidade);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            Modalidade modalidade = new Modalidade();

            ModalidadeService modalidadeService = new ModalidadeService(ModelState);
            modalidade = modalidadeService.Find(id);

            return View(modalidade);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(Modalidade modalidade)
        {
            try
            {
                ModalidadeService modalidadeService = new ModalidadeService(ModelState);

                modalidadeService.Update(modalidade, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Modalidade alterada com sucesso");

                    return RedirectToAction("Edit", new { id = modalidade.IdModalidade });
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar modalidade", ModelState);
            }

            return View(modalidade);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                ModalidadeService modalidadeService = new ModalidadeService(ModelState);

                modalidadeService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir modalidade");
            }
        }
    }
}
