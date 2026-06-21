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
    public class GrupoFuncaoController : BaseController
    {
        //
        // GET: /GrupoFuncao/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            IList<GrupoFuncao> gruposFuncao = null;

            try
            {
                GrupoFuncaoService grupoFuncaoService = new GrupoFuncaoService(ModelState);
                gruposFuncao = grupoFuncaoService.List();

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(gruposFuncao);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            GrupoFuncao grupoFuncao = new GrupoFuncao();

            return View(grupoFuncao);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(GrupoFuncao grupoFuncao)
        {
            try
            {
                GrupoFuncaoService grupoFuncaoService = new GrupoFuncaoService(ModelState);

                grupoFuncao = grupoFuncaoService.Insert(grupoFuncao, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Grupo de função salvo com sucesso");

                    return RedirectToAction("Edit", new { id = grupoFuncao.IdGrupoFuncao });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar grupo de função", ModelState);
            }

            return View(grupoFuncao);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            GrupoFuncao grupoFuncao = new GrupoFuncao();

            GrupoFuncaoService grupoFuncaoService = new GrupoFuncaoService(ModelState);
            grupoFuncao = grupoFuncaoService.Find(id);

            return View(grupoFuncao);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(GrupoFuncao grupoFuncao)
        {
            try
            {
                GrupoFuncaoService grupoFuncaoService = new GrupoFuncaoService(ModelState);

                grupoFuncaoService.Update(grupoFuncao, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Grupo de função alterado com sucesso");

                    return RedirectToAction("Edit", new { id = grupoFuncao.IdGrupoFuncao });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar grupo de função", ModelState);
            }

            return View(grupoFuncao);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                GrupoFuncaoService grupoFuncaoService = new GrupoFuncaoService(ModelState);

                grupoFuncaoService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir grupo de função");
            }
        }
    }
}
