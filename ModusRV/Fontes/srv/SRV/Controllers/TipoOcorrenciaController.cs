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
    public class TipoOcorrenciaController : BaseController
    {
        //
        // GET: /TipoOcorrencia/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            IList<TipoOcorrencia> tiposOcorrencia = null;

            try
            {
                TipoOcorrenciaService ocorrenciaService = new TipoOcorrenciaService(ModelState);
                tiposOcorrencia = ocorrenciaService.List();

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(tiposOcorrencia);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            TipoOcorrencia tipoOcorrencia = new TipoOcorrencia();

            return View(tipoOcorrencia);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(TipoOcorrencia tipoOcorrencia)
        {
            try
            {
                TipoOcorrenciaService tipoOcorrenciaService = new TipoOcorrenciaService(ModelState);

                tipoOcorrencia = tipoOcorrenciaService.Insert(tipoOcorrencia, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Tipo de ocorrência salvo com sucesso");

                    return RedirectToAction("Edit", new { id = tipoOcorrencia.IdTipoOcorrencia });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar tipo de ocorrência", ModelState);
            }

            return View(tipoOcorrencia);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            TipoOcorrencia tipoOcorrencia = new TipoOcorrencia();

            TipoOcorrenciaService tipoOcorrenciaService = new TipoOcorrenciaService(ModelState);
            tipoOcorrencia = tipoOcorrenciaService.Find(id);

            return View(tipoOcorrencia);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(TipoOcorrencia tipoOcorrencia)
        {
            try
            {
                TipoOcorrenciaService tipoOcorrenciaService = new TipoOcorrenciaService(ModelState);

                tipoOcorrenciaService.Update(tipoOcorrencia, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Tipo de ocorrência alterado com sucesso");

                    return RedirectToAction("Edit", new { id = tipoOcorrencia.IdTipoOcorrencia });
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar tipo de ocorrência", ModelState);
            }

            return View(tipoOcorrencia);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                TipoOcorrenciaService tipoOcorrenciaService = new TipoOcorrenciaService(ModelState);

                tipoOcorrenciaService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir tipo de ocorrência");
            }
        }
    }
}
