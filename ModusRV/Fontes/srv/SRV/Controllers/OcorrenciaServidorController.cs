using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Common.Extension;
using SRV.Models.Domain;
using SRV.Common.Exceptions;
using SRV.Common;
using SRV.Filters;
using System.Threading;

namespace SRV.Controllers
{
    public class OcorrenciaServidorController : BaseController
    {
        //
        // GET: /OcorrenciaServidor/

        public ActionResult Index()
        {
            FiltroOcorrenciaServidor filtro = new FiltroOcorrenciaServidor();

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroOcorrenciaServidor filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService();
                    filtro.OcorrenciasServidores = ocorrenciaServidorService.List(filtro, page ?? 1, Constants.gridPageSize);
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
        public ActionResult Delete(int id)
        {
            try
            {
                OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService();

                ocorrenciaServidorService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir ocorrência servidor");
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
                UploadFile(fileUpload, Models.Domain.TipoImportacao.OcorrenciaServidor);
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
                OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService();

                ArquivoImportacao arquivoImportacao = ocorrenciaServidorService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };

                ocorrenciaServidorService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    ocorrenciaServidorService.Import(arquivoImportacao, UsuarioLogado);
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
                OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService();

                ocorrenciaServidorService.Delete(id);

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

        private FiltroOcorrenciaServidor InicializaTelaFiltro(FiltroOcorrenciaServidor filtro)
        {
            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();
            filtro.UnidadesAdministrativas = unidadeAdministrativaService.List().ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);

            OcorrenciaService ocorrenciaService = new OcorrenciaService(ModelState);
            filtro.Ocorrencias = ocorrenciaService.List().ToSelectList<Ocorrencia>(o => o.IdOcorrencia, o => o.DesOcorrencia);

            return filtro;
        }

        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            FuncaoService funcaoService = new FuncaoService();

            IList<ArquivoImportacao> result = funcaoService.ListByTipoImportacao(Models.Domain.TipoImportacao.OcorrenciaServidor);

            return result;
        }

		[CustomAuthorize(Roles = "Administrador")]
		public ActionResult Create(int idServidor, string nomeServidor)
		{
			OcorrenciaServidor ocorrenciaServidor = new OcorrenciaServidor();

			ocorrenciaServidor.Servidor = new Servidor { IdServidor = idServidor, DesNomeServidor = nomeServidor };

			CadastroOcorrenciaServidor modelCadastro = new CadastroOcorrenciaServidor() { OcorrenciaServidor = ocorrenciaServidor };
			InicializaTelaCadastro(modelCadastro);

			ViewBag.IsEdit = false;
			return View(modelCadastro);
		}

		[HttpPost]
		[CustomAuthorize(Roles = "Administrador")]
		public ActionResult Create(CadastroOcorrenciaServidor cadastroOcorrenciaServidor)
		{
			try
			{
				OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService(ModelState);

				cadastroOcorrenciaServidor.OcorrenciaServidor = ocorrenciaServidorService.Insert(cadastroOcorrenciaServidor.OcorrenciaServidor, UsuarioLogado);

				if (ModelState.IsValid)
				{
					TempData["message"] = String.Format("Operação realizada com sucesso. Obs.: É necessário calcular novamente a remuneração");

					return RedirectToAction("Edit", new { id = cadastroOcorrenciaServidor.OcorrenciaServidor.IdOcorrenciaServidor });
				}
			}
			catch (Exception e)
			{
				ExceptionHandler.Execute(e, "Falha ao salvar ocorrência servidor", ModelState);
			}

			InicializaTelaCadastro(cadastroOcorrenciaServidor);
			ViewBag.IsEdit = false;
			return View(cadastroOcorrenciaServidor);
		}

		private CadastroOcorrenciaServidor InicializaTelaCadastro(CadastroOcorrenciaServidor cadastro)
		{
			UserState user = (UserState)Session["user"];

			OcorrenciaService ocorrenciaService = new OcorrenciaService();
			cadastro.Ocorrencias = ocorrenciaService.List().ToSelectList<Ocorrencia>(o => o.IdOcorrencia, o => o.DesOcorrencia);
			
			if (cadastro.OcorrenciaServidor.DataInicioOcorrencia == DateTime.MinValue)
				cadastro.OcorrenciaServidor.DataInicioOcorrencia = cadastro.OcorrenciaServidor.DataInicioOcorrencia.AddYears(user.Ciclo - 1);

			return cadastro;
		}

		[CustomAuthorize(Roles = "Administrador")]
		public ActionResult Edit(int id)
		{
			CadastroOcorrenciaServidor cadastro = new CadastroOcorrenciaServidor();

			try
			{
				OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService(ModelState);
				cadastro.OcorrenciaServidor = ocorrenciaServidorService.PesquisarPor(id);

			}
			catch (Exception e)
			{
				ExceptionHandler.Execute(e, "Falha ao carregar tela", ModelState);
			}

			cadastro = InicializaTelaCadastro(cadastro);

			ViewBag.IsEdit = true;

			return View(cadastro);
		}

		[HttpPost]
		[CustomAuthorize(Roles = "Administrador")]
		public ActionResult Edit(CadastroOcorrenciaServidor cadastro)
		{
			try
			{
				OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService(ModelState);

				ocorrenciaServidorService.Update(cadastro.OcorrenciaServidor, UsuarioLogado);

				if (ModelState.IsValid)
				{
					TempData["message"] = String.Format("Operação realizada com sucesso. Obs.: É necessário calcular novamente a remuneração");

					return RedirectToAction("Edit", new { id = cadastro.OcorrenciaServidor.IdOcorrenciaServidor });
				}

			}
			catch (Exception e)
			{
				ExceptionHandler.Execute(e, "Falha ao alterar ocorrência servidor", ModelState);
			}

			cadastro = InicializaTelaCadastro(cadastro);

			ViewBag.IsEdit = true;

			return View(cadastro);
		}

		[HttpPost]
		[CustomAuthorize(Roles = "Administrador")]
		public ActionResult DeleteRecurso(int id, string numeroRecurso)
		{
			try
			{
				OcorrenciaServidorService ocorrenciaServidorService = new OcorrenciaServidorService();

				ocorrenciaServidorService.Delete(id, UsuarioLogado, numeroRecurso);

				return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
			}
			catch (Exception e)
			{
				return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir registro de ocorrência servidor");
			}
		}
    }
}
