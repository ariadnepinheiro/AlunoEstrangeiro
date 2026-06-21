using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Filters;
using SRV.Common.Exceptions;
using SRV.Models.Service;
using SRV.Models.Domain;
using SRV.Models.DTO;
using SRV.Common;
using System.Threading;

namespace SRV.Controllers 
{
	public class AplicacaoProvaAvaliacaoExternaController: BaseController 
	{
		public ActionResult Index()
		{
			FiltroAplicacaoProvaAvaliacaoExterna filtro = new FiltroAplicacaoProvaAvaliacaoExterna();
			
			return View(filtro);
		}

		[HttpPost]
		public ActionResult Index(FiltroAplicacaoProvaAvaliacaoExterna filtro, int? page)
		{
			try
			{
				if (ModelState.IsValid)
				{
					AplicacaoProvaAvaliacaoExternaService aplicacaoProvaAvaliacaoExternaService = new AplicacaoProvaAvaliacaoExternaService();
					filtro.AplicaoesProvas = aplicacaoProvaAvaliacaoExternaService.List(filtro, page ?? 1, Constants.gridPageSize);
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
			AplicacaoProvaAvaliacaoExternaService aplicacaoProvaAvaliacaoExternaService = new AplicacaoProvaAvaliacaoExternaService();

			try
			{
				aplicacaoProvaAvaliacaoExternaService.Delete(idServidor, UsuarioLogado);

				return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
			}
			catch (Exception e)
			{
				return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir a aplicação de prova para avaliação");
			}			
		}

        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            AplicacaoProvaAvaliacaoExternaService aplicacaoProvaAvaliacaoExternaService = new AplicacaoProvaAvaliacaoExternaService();

            IList<ArquivoImportacao> result = aplicacaoProvaAvaliacaoExternaService.ListByTipoImportacao(Models.Domain.TipoImportacao.AplicacaoProvaAvaliacaoExterna);

            return result;
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
                UploadFile(fileUpload, Models.Domain.TipoImportacao.AplicacaoProvaAvaliacaoExterna);
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
            AplicacaoProvaAvaliacaoExternaService aplicacaoProvaAvaliacaoExternaService = 
                new AplicacaoProvaAvaliacaoExternaService();
            ArquivoImportacao arquivoImportacao = null;

            try
            {
                arquivoImportacao = aplicacaoProvaAvaliacaoExternaService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };
                arquivoImportacao.UsuarioUpload = arquivoImportacao.UsuarioImportacao;
                aplicacaoProvaAvaliacaoExternaService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    aplicacaoProvaAvaliacaoExternaService.Import(arquivoImportacao, UsuarioLogado);
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
                AplicacaoProvaAvaliacaoExternaService aplicacaoProvaAvaliacaoExternaService = new AplicacaoProvaAvaliacaoExternaService();
                aplicacaoProvaAvaliacaoExternaService.Delete(id);

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