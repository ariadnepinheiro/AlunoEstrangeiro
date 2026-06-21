using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;
using System.Web.Mvc;
using SRV.Models.Service;
using SRV.Common;
using SRV.Common.Exceptions;
using SRV.Filters;
using SRV.Models.Domain;
using System.Threading;

namespace SRV.Controllers
{
    public class LancamentoNotaDocenteController: BaseController
    {
        public ActionResult Index()
        {
            FiltroLancamentoNotaDocente filtro = new FiltroLancamentoNotaDocente();

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroLancamentoNotaDocente filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
					filtro.IdAnoReferencia = UsuarioLogado.Ciclo;
					LancamentoNotaDocenteService lancamentoNotaDocenteService = new LancamentoNotaDocenteService();
                    filtro.LancamentosNotasDocentes = lancamentoNotaDocenteService.List(filtro, page ?? 1, Constants.gridPageSize);
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
		public ActionResult Delete(int IdLancamentoNotaDocente)
        {
            LancamentoNotaDocenteService lancamentoNotaDocenteService = new LancamentoNotaDocenteService();

            try
            {
				lancamentoNotaDocenteService.Delete(IdLancamentoNotaDocente, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir o lançamento de nota para o docente");
            }
        }

        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            LancamentoNotaDocenteService lancamentoNotaDocenteService = new LancamentoNotaDocenteService();

            IList<ArquivoImportacao> list = lancamentoNotaDocenteService.ListByTipoImportacao(TipoImportacao.LancamentoNotasDocente);

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
                UploadFile(fileUpload, TipoImportacao.LancamentoNotasDocente);
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
            LancamentoNotaDocenteService lancamentoNotaDocenteService = new LancamentoNotaDocenteService();
            ArquivoImportacao arquivoImportacao = null;

            try
            {
                arquivoImportacao = lancamentoNotaDocenteService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };
                arquivoImportacao.UsuarioUpload = arquivoImportacao.UsuarioImportacao;
                lancamentoNotaDocenteService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    lancamentoNotaDocenteService.Import(arquivoImportacao, UsuarioLogado);
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
                LancamentoNotaDocenteService lancamentoNotaDocenteService = new LancamentoNotaDocenteService();
                lancamentoNotaDocenteService.Delete(id);

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