using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using SRV.Models.Service;
using SRV.Models.Domain;
using SRV.Common;
using SRV.Common.Exceptions;
using SRV.Filters;
using System.Threading;
using SRV.Models.DTO;
using System.Data.SqlClient;

namespace SRV.Controllers
{
    public class DenunciaAvaliacaoExternaController : BaseController
    {
        public ActionResult Index()
        {
            FiltroDenunciaAvaliacaoExterna filtro = new FiltroDenunciaAvaliacaoExterna();

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroDenunciaAvaliacaoExterna filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DenunciaAvaliacaoExternaService denunciaAvaliacaoExternaService = new DenunciaAvaliacaoExternaService();
                    filtro.DenunciasAvaliacoesExternas = denunciaAvaliacaoExternaService.List(filtro, page ?? 1, Constants.gridPageSize);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return View(filtro);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int idServidor)
        {
            DenunciaAvaliacaoExternaService denunciaAvaliacaoExternaService = new DenunciaAvaliacaoExternaService();

            try
            {
                denunciaAvaliacaoExternaService.Delete(idServidor, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir a avaliação externa por denúncia");
            }
        }

        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            DenunciaAvaliacaoExternaService denunciaAvaliacaoExternaService = new DenunciaAvaliacaoExternaService();

            IList<ArquivoImportacao> list = denunciaAvaliacaoExternaService.ListByTipoImportacao(TipoImportacao.DenunciaAvaliacaoExterna);

            return list;
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
                UploadFile(fileUpload, TipoImportacao.DenunciaAvaliacaoExterna);
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
            DenunciaAvaliacaoExternaService denunciaAvaliacaoExternaService = new DenunciaAvaliacaoExternaService();
            ArquivoImportacao arquivoImportacao = null;

            try
            {
                arquivoImportacao = denunciaAvaliacaoExternaService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };
                arquivoImportacao.UsuarioUpload = arquivoImportacao.UsuarioImportacao;
                denunciaAvaliacaoExternaService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    denunciaAvaliacaoExternaService.Import(arquivoImportacao, UsuarioLogado);
                });

                RedirectToAction("Upload");
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
                DenunciaAvaliacaoExternaService denunciaAvaliacaoExternaService = new DenunciaAvaliacaoExternaService();
                denunciaAvaliacaoExternaService.Delete(id);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir arquivo de importação");
            }
        }

        public ActionResult FormatoArquivo()
        {
            return PartialView();
        }
    }
}