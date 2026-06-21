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
    public class FuncaoServidorController : BaseController
    {
        //
        // GET: /FuncaoServidor/

        public ActionResult Index()
        {
            FiltroFuncaoServidor filtro = new FiltroFuncaoServidor();

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroFuncaoServidor filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FuncaoServidorService funcaoServidorService = new FuncaoServidorService();

                    filtro.IdAnoReferencia = UsuarioLogado.Ciclo;

                    filtro.FuncoesServidores = funcaoServidorService.List(filtro, page ?? 1, Constants.gridPageSize);
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
                FuncaoServidorService funcaoServidorService = new FuncaoServidorService();

                funcaoServidorService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir função servidor");
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
                UploadFile(fileUpload, Models.Domain.TipoImportacao.FuncaoServidor);
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

        public ArquivoImportacao _arquivoImportacao;
        public UserState _UsuariosLogado;

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Import(int id)
        {
            try
            {
                FuncaoServidorService funcaoServidorService = new FuncaoServidorService();

                ArquivoImportacao arquivoImportacao = funcaoServidorService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };

                funcaoServidorService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                _arquivoImportacao = arquivoImportacao;
                _UsuariosLogado = UsuarioLogado;

                Thread funcaoServidorThread = new Thread(ImportFuncaoServidor);
                funcaoServidorThread.Start();

                //ThreadPool.QueueUserWorkItem(delegate
                //{
                //    funcaoServidorService.Import(arquivoImportacao, UsuarioLogado);
                //});

                return RedirectToAction("Upload");

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha no upload", ModelState);
            }

            IList<ArquivoImportacao> arquivos = InicializaTelaUpload();

            return View("upload", arquivos);
        }

        public void ImportFuncaoServidor()
        {
            FuncaoServidorService funcaoServidorService = new FuncaoServidorService();
            funcaoServidorService.Import(_arquivoImportacao, _UsuariosLogado);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult DeleteImport(short id)
        {
            try
            {
                FuncaoServidorService funcaoServidorService = new FuncaoServidorService();

                funcaoServidorService.Delete(id);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir arquivo de importação");
            }
        }

        private FiltroFuncaoServidor InicializaTelaFiltro(FiltroFuncaoServidor filtro)
        {
            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();
            filtro.UnidadesAdministrativas = unidadeAdministrativaService.List().ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);

            FuncaoService funcaoService = new FuncaoService();
            filtro.Funcoes = funcaoService.List().ToSelectList<Funcao>(o => o.IdFuncao, o => o.DesFuncao);

            return filtro;
        }

        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            FuncaoServidorService funcaoServidorService = new FuncaoServidorService();

            IList<ArquivoImportacao> result = funcaoServidorService.ListByTipoImportacao(Models.Domain.TipoImportacao.FuncaoServidor);

            return result;
        }

		[CustomAuthorize(Roles = "Administrador")]
		public ActionResult Create(int idServidor, string nomeServidor)
		{
			FuncaoServidor funcaoServidor = new FuncaoServidor();

			funcaoServidor.Servidor = new Servidor { IdServidor = idServidor, DesNomeServidor = nomeServidor };

			CadastroFuncaoServidor modelCadastro = new CadastroFuncaoServidor() { FuncaoServidor = funcaoServidor, UnidadeAdministrativa = new UnidadeAdministrativa() };
			InicializaTelaCadastro(modelCadastro);

			ViewBag.IsEdit = false;
			return View(modelCadastro);
		}

		[HttpPost]
		[CustomAuthorize(Roles = "Administrador")]
		public ActionResult Create(CadastroFuncaoServidor cadastroFuncaoServidor)
		{
			try
			{
				FuncaoServidorService funcaoServidorService = new FuncaoServidorService(ModelState);

				cadastroFuncaoServidor.FuncaoServidor = funcaoServidorService.Insert(cadastroFuncaoServidor.FuncaoServidor, UsuarioLogado);

				if (ModelState.IsValid)
				{
					TempData["message"] = String.Format("Operação realizada com sucesso. Obs.: É necessário calcular novamente a remuneração");

					return RedirectToAction("Edit", new { id = cadastroFuncaoServidor.FuncaoServidor.IdFuncaoServidor });
				}
			}
			catch (Exception e)
			{
				ExceptionHandler.Execute(e, "Falha ao salvar função servidor", ModelState);
			}

			InicializaTelaCadastro(cadastroFuncaoServidor);
			ViewBag.IsEdit = false;
			return View(cadastroFuncaoServidor);
		}

		private CadastroFuncaoServidor InicializaTelaCadastro(CadastroFuncaoServidor cadastro)
		{
			UserState user = (UserState)Session["user"];

			FuncaoService funcaoService = new FuncaoService();
			cadastro.Funcoes = funcaoService.List().ToSelectList<Funcao>(o => o.IdFuncao, o => o.DesFuncao);

			UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();
			cadastro.UnidadesAdministrativas = unidadeAdministrativaService.List().ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa + " - " + o.IdUnidadeAdministrativa);

			if (cadastro.FuncaoServidor.DataInicioFuncao == DateTime.MinValue)
				cadastro.FuncaoServidor.DataInicioFuncao = cadastro.FuncaoServidor.DataInicioFuncao.AddYears(user.Ciclo - 1);

			return cadastro;
		}

		[CustomAuthorize(Roles = "Administrador")]
		public ActionResult Edit(int id)
		{
			CadastroFuncaoServidor cadastro = new CadastroFuncaoServidor();

			try
			{
				FuncaoServidorService funcaoService = new FuncaoServidorService(ModelState);
				cadastro.FuncaoServidor = funcaoService.PesquisarPor(id);

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
		public ActionResult Edit(CadastroFuncaoServidor cadastro)
		{
			try
			{
				FuncaoServidorService funcaoService = new FuncaoServidorService(ModelState);

				funcaoService.Update(cadastro.FuncaoServidor, UsuarioLogado);

				if (ModelState.IsValid)
				{
					TempData["message"] = String.Format("Operação realizada com sucesso. Obs.: É necessário calcular novamente a remuneração");

					return RedirectToAction("Edit", new { id = cadastro.FuncaoServidor.IdFuncaoServidor });
				}

			}
			catch (Exception e)
			{
				ExceptionHandler.Execute(e, "Falha ao alterar função servidor", ModelState);
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
				FuncaoServidorService funcaoServidorService = new FuncaoServidorService();

				funcaoServidorService.Delete(id,UsuarioLogado,numeroRecurso);

				return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
			}
			catch (Exception e)
			{
				return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir registro de função servidor");
			}
		}
    }
}
