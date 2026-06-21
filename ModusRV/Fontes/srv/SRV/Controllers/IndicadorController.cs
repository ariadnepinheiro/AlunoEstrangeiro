using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Filters;
using SRV.Common.Exceptions;
using SRV.Models.Domain;
using SRV.Models.Service;
using SRV.Models.DTO;
using SRV.Common.Extension;

namespace SRV.Controllers
{
    public class IndicadorController : BaseController
    {
        //
        // GET: /Indicador/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            IList<Indicador> indicadores = null;

            try
            {
                IndicadorService indicadorService = new IndicadorService(ModelState);
                indicadores = indicadorService.List();

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(indicadores);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            CadastroIndicador cadastro = new CadastroIndicador();
            cadastro.Indicador = new Indicador();

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = false;
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(CadastroIndicador cadastro)
        {
            try
            {
                IndicadorService indicadorService = new IndicadorService(ModelState);

                cadastro.Indicador = indicadorService.Insert(cadastro.Indicador, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Indicador salvo com sucesso");

                    return RedirectToAction("Edit", new { id = cadastro.Indicador.IdIndicador });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar indicador", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = false;
            return View(cadastro);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id)
        {
            CadastroIndicador cadastro = new CadastroIndicador();            

            IndicadorService indicadorService = new IndicadorService(ModelState);
            cadastro.Indicador = indicadorService.Find(id);

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = true;
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(CadastroIndicador cadastro)
        {
            try
            {
                IndicadorService indicadorService = new IndicadorService(ModelState);

                indicadorService.Update(cadastro.Indicador, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Indicador alterado com sucesso");

                    return RedirectToAction("Edit", new { id = cadastro.Indicador.IdIndicador });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar indicador", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = true;
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                IndicadorService indicadorService = new IndicadorService(ModelState);

                indicadorService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir indicador");
            }
        }

        private CadastroIndicador InicializaTelaCadastro(CadastroIndicador cadastro)
        {
            cadastro.TiposIndicador = TipoIndicador.Parametro.ToSelectList();

            // Verifica se existe algum relacionamnto com outras tabelas
            if (cadastro.Indicador.IdIndicador != null)
            {
                IndicadorService indicadorService = new IndicadorService(ModelState);
                ViewBag.BloqueiaTipoIndicador = indicadorService.VerificaRelacionamento(cadastro.Indicador.IdIndicador.Value);
            }
            else
            {
                ViewBag.BloqueiaTipoIndicador = false;
            }

            return cadastro;
        }
    }
}
