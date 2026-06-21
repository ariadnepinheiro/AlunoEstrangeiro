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
    public class TipoCriterioElegibilidadeController : BaseController
    {
        //
        // GET: /TipoCriterioElegibilidade/

        [CustomAuthorize(Roles = "Administrador, Secretaria")]
        public ActionResult Index()
        {
            IList<TipoCriterioElegibilidade> tiposCriterioElegibilidade = null;

            try
            {
                TipoCriterioElegibilidadeService tipoCriterioElegibilidadeService = new TipoCriterioElegibilidadeService(ModelState);
                tiposCriterioElegibilidade = tipoCriterioElegibilidadeService.List(UsuarioLogado.Ciclo);

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(tiposCriterioElegibilidade);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            CadastroTipoCriterioElegibilidade cadastro = new CadastroTipoCriterioElegibilidade();
            cadastro.TipoCriterioElegibilidade = new TipoCriterioElegibilidade();

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = false;
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(CadastroTipoCriterioElegibilidade cadastro)
        {
            try
            {
                cadastro.TipoCriterioElegibilidade.AnoReferencia = new AnoReferencia() { IdAnoReferencia = UsuarioLogado.Ciclo };

                TipoCriterioElegibilidadeService tipoCriterioElegibilidadeService = new TipoCriterioElegibilidadeService(ModelState);

                cadastro.TipoCriterioElegibilidade = tipoCriterioElegibilidadeService.Insert(cadastro.TipoCriterioElegibilidade, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Tipo de critério de elegibilidade salvo com sucesso");

                    return RedirectToAction("Edit", new { id = cadastro.TipoCriterioElegibilidade.IdTipoCriterioElegibilidade, seq1 = cadastro.TipoCriterioElegibilidade.AnoReferencia.IdAnoReferencia, seq2 = cadastro.TipoCriterioElegibilidade.TipoUnidadeAdministrativa.IdTipoUnidAdm });
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao salvar tipo de critério de elegibilidade", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = false;
            return View(cadastro);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int id, int seq1, int seq2)
        {
            CadastroTipoCriterioElegibilidade cadastro = new CadastroTipoCriterioElegibilidade();

            TipoCriterioElegibilidadeService tipoCriterioElegibilidadeService = new TipoCriterioElegibilidadeService(ModelState);
            cadastro.TipoCriterioElegibilidade = tipoCriterioElegibilidadeService.Find(id, seq1, seq2);

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = true;
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(CadastroTipoCriterioElegibilidade cadastro)
        {
            try
            {
				cadastro.TipoCriterioElegibilidade.AnoReferencia = new AnoReferencia() { IdAnoReferencia = UsuarioLogado.Ciclo };

				TipoCriterioElegibilidadeService tipoCriterioElegibilidadeService = new TipoCriterioElegibilidadeService(ModelState);

                tipoCriterioElegibilidadeService.Update(cadastro.TipoCriterioElegibilidade, UsuarioLogado);

                if (ModelState.IsValid)
                {
                    TempData["message"] = String.Format("Tipo de critério de elegibilidade alterado com sucesso");

                    return RedirectToAction("Edit", new { id = cadastro.TipoCriterioElegibilidade.IdTipoCriterioElegibilidade, seq1 = cadastro.TipoCriterioElegibilidade.AnoReferencia.IdAnoReferencia, seq2 = cadastro.TipoCriterioElegibilidade.TipoUnidadeAdministrativa.IdTipoUnidAdm });
                }

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar tipo de critério de elegibilidade", ModelState);
            }

            cadastro = InicializaTelaCadastro(cadastro);

            ViewBag.IsEdit = true;
            return View(cadastro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id, int seq1, int seq2)
        {
            try
            {
                TipoCriterioElegibilidadeService tipoCriterioElegibilidadeService = new TipoCriterioElegibilidadeService(ModelState);

                tipoCriterioElegibilidadeService.Delete(id, seq1, seq2, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir tipo de critério de elegibilidade");
            }
        }

        private CadastroTipoCriterioElegibilidade InicializaTelaCadastro(CadastroTipoCriterioElegibilidade cadastro)
        {
            TipoUnidadeAdministrativaService tipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);
            cadastro.TiposUnidadeAdministrativa = tipoUnidadeAdministrativaService.List().ToSelectList<TipoUnidadeAdministrativa>(o => o.IdTipoUnidAdm.Value, o => o.DesTipoUnidAdm);

            return cadastro;
        }
    }
}
