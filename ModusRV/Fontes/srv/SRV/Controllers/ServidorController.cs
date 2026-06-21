using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Models.Domain;
using SRV.Common.Exceptions;
using System.Threading;
using SRV.Filters;
using SRV.Common;

namespace SRV.Controllers
{
    public class ServidorController : BaseController
    {
        //
        // GET: /Servidor/

        public ActionResult Index()
        {
            FiltroServidor filtro = new FiltroServidor();

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroServidor filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ServidorService servidorService = new ServidorService();
                    filtro.Servidores = servidorService.List(filtro, page ?? 1, Constants.gridPageSize);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(filtro);
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
                UploadFile(fileUpload, Models.Domain.TipoImportacao.Servidor);
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha no upload", ModelState);
            }
            IList<ArquivoImportacao> arquivos = InicializaTelaUpload();

            return View(arquivos);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Import(int id)
        {
            try
            {
                ServidorService servidorService = new ServidorService();

                ArquivoImportacao arquivoImportacao = servidorService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id};

                servidorService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    servidorService.Import(arquivoImportacao, UsuarioLogado);
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
        public ActionResult Delete(int id)
        {
            try
            {
                ServidorService servidorService = new ServidorService();

                servidorService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir servidor");
            }
        }

        public ActionResult FormatoArquivo()
        {
            return PartialView();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult DeleteImport(short id)
        {
            try
            {
                ServidorService servidorService = new ServidorService();

                servidorService.Delete(id);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir arquivo de importação");
            }
        }


        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            ServidorService servidorService = new ServidorService();

            IList<ArquivoImportacao> result = servidorService.ListByTipoImportacao(Models.Domain.TipoImportacao.Servidor);

            return result;
        }
    }
}
