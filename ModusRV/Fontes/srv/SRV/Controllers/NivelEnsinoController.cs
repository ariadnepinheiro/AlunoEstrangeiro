using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Filters;
using SRV.Models.Domain;
using SRV.Models.Service;
using SRV.Common.Exceptions;
using SRV.Common.Extension;
using SRV.Models.DTO;

namespace SRV.Controllers
{
    public class NivelEnsinoController : BaseController
    {
        //
        // GET: /NivelEnsino/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            IList<NivelEnsino> niveisEnsino = null;

            try
            {
                NivelEnsinoService nivelEnsinoService = new NivelEnsinoService(ModelState);
                niveisEnsino = nivelEnsinoService.List();

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(niveisEnsino);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            CadastroNivelEnsino cadastro = new CadastroNivelEnsino();
            cadastro.NivelEnsino = new NivelEnsino();

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = false;
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(CadastroNivelEnsino cadastro)
        {
            try
            {
                NivelEnsinoService nivelEnsinoService = new NivelEnsinoService(ModelState);

                cadastro.NivelEnsino = nivelEnsinoService.Insert(cadastro.NivelEnsino, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Nível de ensino salvo com sucesso");

                    return RedirectToAction("Edit", new { id = cadastro.NivelEnsino.IdNivelEnsino, seq = cadastro.NivelEnsino.Modalidade.IdModalidade });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar nível de ensino", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = false;
            return View(cadastro);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id, int seq)
        {
            CadastroNivelEnsino cadastro = new CadastroNivelEnsino();

            NivelEnsinoService nivelEnsinoService = new NivelEnsinoService(ModelState);
            cadastro.NivelEnsino = nivelEnsinoService.Find(id, seq);

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = true;
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(CadastroNivelEnsino cadastro)
        {
            try
            {
                NivelEnsinoService nivelEnsinoService = new NivelEnsinoService(ModelState);

                nivelEnsinoService.Update(cadastro.NivelEnsino, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Nível de ensino alterado com sucesso");

                    return RedirectToAction("Edit", new { id = cadastro.NivelEnsino.IdNivelEnsino, seq = cadastro.NivelEnsino.Modalidade.IdModalidade });
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar nível de ensino", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = true;
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id, int seq)
        {
            try
            {
                NivelEnsinoService nivelEnsinoService = new NivelEnsinoService(ModelState);

                nivelEnsinoService.Delete(id, seq, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir nível de ensino");
            }
        }

        private CadastroNivelEnsino InicializaTelaCadastro(CadastroNivelEnsino cadastro)
        {
            ModalidadeService modalidadeService = new ModalidadeService(ModelState);
            cadastro.Modalidades = modalidadeService.List().ToSelectList<Modalidade>(o => o.IdModalidade.Value, o => o.DesModalidade);

            return cadastro;
        }
    }
}
