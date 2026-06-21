using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Common;
using SRV.Common.Exceptions;
using SRV.Common.Extension;
using SRV.Models.Domain;
using SRV.Filters;
using System.Threading;

namespace SRV.Controllers
{
    public class CriterioUnidadeAdministrativaController : BaseController
    {
        //
        // GET: /CriterioUnidadeAdministrativa/

        public ActionResult Index()
        {
            FiltroCriterioUnidadeAdministrativa filtro = new FiltroCriterioUnidadeAdministrativa();

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroCriterioUnidadeAdministrativa filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    filtro.IdAnoReferencia = UsuarioLogado.Ciclo;

                    CriterioUnidadeAdministrativaService criterioUnidadeAdministrativaService = new CriterioUnidadeAdministrativaService();
                    filtro.CriteriosUnidadeAdministrativa = criterioUnidadeAdministrativaService.List(filtro, page ?? 1, Constants.gridPageSize);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id, int seq)
        {
            try
            {
                CriterioUnidadeAdministrativaService criterioUnidadeAdministrativaService = new CriterioUnidadeAdministrativaService();

                criterioUnidadeAdministrativaService.Delete(id, seq, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir critério de unidade administrativa");
            }
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Upload()
        {
            IList<ArquivoImportacao> arquivos = InicializaTelaUpload();

            return View(arquivos);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Upload(HttpPostedFileBase fileUpload)
        {
            try
            {
                UploadFile(fileUpload, Models.Domain.TipoImportacao.CriterioUnidadeAdministrativa);
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha no upload", ModelState);
            }
            IList<ArquivoImportacao> arquivos = InicializaTelaUpload();

            return View(arquivos);
        }

        public ActionResult FormatoArquivo()
        {
            return PartialView();
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Import(int id)
        {
            try
            {
                CriterioUnidadeAdministrativaService criterioUnidadeAdministrativaService = new CriterioUnidadeAdministrativaService();

                ArquivoImportacao arquivoImportacao = criterioUnidadeAdministrativaService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };

                criterioUnidadeAdministrativaService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    criterioUnidadeAdministrativaService.Import(arquivoImportacao, UsuarioLogado);
                });

                return RedirectToAction("Upload");

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha no upload", ModelState);
            }

            IList<ArquivoImportacao> arquivos = InicializaTelaUpload();

            return View("upload", arquivos);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult DeleteImport(short id)
        {
            try
            {
                CriterioUnidadeAdministrativaService criterioUnidadeAdministrativaService = new CriterioUnidadeAdministrativaService();

                criterioUnidadeAdministrativaService.Delete(id);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir arquivo de importação");
            }
        }

        private FiltroCriterioUnidadeAdministrativa InicializaTelaFiltro(FiltroCriterioUnidadeAdministrativa filtro)
        {
            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();
            filtro.UnidadesAdministrativas = unidadeAdministrativaService.List().ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);

            return filtro;
        }

        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            CriterioUnidadeAdministrativaService criterioUnidadeAdministrativaService = new CriterioUnidadeAdministrativaService();

            IList<ArquivoImportacao> result = criterioUnidadeAdministrativaService.ListByTipoImportacao(Models.Domain.TipoImportacao.CriterioUnidadeAdministrativa);

            return result;
        }
    }
}
