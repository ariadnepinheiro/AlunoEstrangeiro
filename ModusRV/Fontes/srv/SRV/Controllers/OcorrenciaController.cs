using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.Service;
using SRV.Models.Domain;
using SRV.Common.Exceptions;
using SRV.Models.DTO;
using SRV.Common;
using SRV.Common.Extension;
using SRV.Filters;

namespace SRV.Controllers
{
    public class OcorrenciaController : BaseController
    {
        //
        // GET: /Ocorrencia/

        [CustomAuthorize(Roles="Administrador, Secretaria")]
        public ActionResult Index(int? page)
        {
            Paging<Ocorrencia> ocorrencias = null;

            try
            {
                OcorrenciaService ocorrenciaService = new OcorrenciaService(ModelState);
                ocorrencias = ocorrenciaService.List(page ?? 1, Constants.gridPageSize);

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(ocorrencias);
        }


        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            CadastroOcorrencia cadastro = new CadastroOcorrencia();
            cadastro.Ocorrencia = new Ocorrencia();
            cadastro.OperacaoInsert = true;

            cadastro = InicializaTelaCadastro(cadastro);

            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(CadastroOcorrencia cadastro)
        {
            try
            {
                OcorrenciaService ocorrenciaService = new OcorrenciaService(ModelState);

                cadastro.Ocorrencia = ocorrenciaService.Insert(cadastro.Ocorrencia, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Ocorrência salva com sucesso");

                    return RedirectToAction("Edit", new { id = cadastro.Ocorrencia.IdOcorrencia });
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar ocorrência", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);
            cadastro.OperacaoInsert = true;
            return View(cadastro);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            CadastroOcorrencia cadastro = new CadastroOcorrencia();

            OcorrenciaService ocorrenciaService = new OcorrenciaService(ModelState);
            cadastro.Ocorrencia = ocorrenciaService.Find(id);

            cadastro = InicializaTelaCadastro(cadastro);
            cadastro.OperacaoInsert = false;

            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(CadastroOcorrencia cadastro)
        {
            try
            {
                OcorrenciaService ocorrenciaService = new OcorrenciaService(ModelState);

                ocorrenciaService.Update(cadastro.Ocorrencia, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Ocorrência alterada com sucesso");

                    return RedirectToAction("Edit", new { id = cadastro.Ocorrencia.IdOcorrencia });
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar ocorrência", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);
            cadastro.OperacaoInsert = false;
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(short id)
        {
            try
            {
                OcorrenciaService ocorrenciaService = new OcorrenciaService(ModelState);

                ocorrenciaService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir ocorrência");
            }
        }

        private CadastroOcorrencia InicializaTelaCadastro(CadastroOcorrencia cadastro)
        {
            TipoOcorrenciaService tipoOcorrenciaService = new TipoOcorrenciaService(ModelState);
            cadastro.TiposOcorrencia = tipoOcorrenciaService.List().ToSelectList<TipoOcorrencia>(o => o.IdTipoOcorrencia.Value, o => o.DesTipoOcorrencia);

            return cadastro;
        }

    }
}
