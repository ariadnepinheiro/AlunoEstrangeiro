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
    public class MetaIGEUnidadeAdministrativaController : BaseController
    {
        //
        // GET: /MetaIGEUnidadeAdministrativa/

        public ActionResult Index()
        {
            FiltroMetaIGEUnidadeAdministrativa filtro = new FiltroMetaIGEUnidadeAdministrativa();

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroMetaIGEUnidadeAdministrativa filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    filtro.IdAnoReferencia = UsuarioLogado.Ciclo;

                    MetaIGEUnidadeAdministrativaService metaIGEUnidadeAdministrativaService = new MetaIGEUnidadeAdministrativaService();
                    filtro.Metas = metaIGEUnidadeAdministrativaService.List(filtro, page ?? 1, Constants.gridPageSize);
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
                MetaIGEUnidadeAdministrativaService metaIGEUnidadeAdministrativaService = new MetaIGEUnidadeAdministrativaService();

                metaIGEUnidadeAdministrativaService.Delete(id, seq, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir meta");
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
                UploadFile(fileUpload, Models.Domain.TipoImportacao.MetaIGEUnidadeAdministrativa);
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
                MetaIGEUnidadeAdministrativaService metaIGEUnidadeAdministrativaService = new MetaIGEUnidadeAdministrativaService();

                ArquivoImportacao arquivoImportacao = metaIGEUnidadeAdministrativaService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };

                metaIGEUnidadeAdministrativaService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    metaIGEUnidadeAdministrativaService.Import(arquivoImportacao, UsuarioLogado);
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
                MetaIGEUnidadeAdministrativaService metaIGEUnidadeAdministrativaService = new MetaIGEUnidadeAdministrativaService();

                metaIGEUnidadeAdministrativaService.Delete(id);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir arquivo de importação");
            }
        }

        private FiltroMetaIGEUnidadeAdministrativa InicializaTelaFiltro(FiltroMetaIGEUnidadeAdministrativa filtro)
        {
            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();
            filtro.UnidadesAdministrativas = unidadeAdministrativaService.List().ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);
            
            return filtro;
        }

        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            MetaIGEUnidadeAdministrativaService metaIGEUnidadeAdministrativaService = new MetaIGEUnidadeAdministrativaService();

            IList<ArquivoImportacao> result = metaIGEUnidadeAdministrativaService.ListByTipoImportacao(Models.Domain.TipoImportacao.MetaIGEUnidadeAdministrativa);

            return result;
        }
    }
}
