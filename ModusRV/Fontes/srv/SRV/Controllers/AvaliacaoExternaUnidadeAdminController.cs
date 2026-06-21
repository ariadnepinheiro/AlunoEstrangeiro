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
    public class AvaliacaoExternaUnidadeAdminController : BaseController
    {
        //
        // GET: /AvaliacaoExternaUnidadeAdmin/

        public ActionResult Index()
        {
            FiltroAvaliacaoExternaUnidadeAdmin filtro = new FiltroAvaliacaoExternaUnidadeAdmin();

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroAvaliacaoExternaUnidadeAdmin filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AvaliacaoExternaUnidadeAdminService avaliacaoExternaUnidadeAdminService = new AvaliacaoExternaUnidadeAdminService();
                    filtro.AvaliacoesUnidades = avaliacaoExternaUnidadeAdminService.List(filtro, UsuarioLogado.Ciclo, page ?? 1, Constants.gridPageSize);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            filtro = InicializaTelaFiltro(filtro);

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
                UploadFile(fileUpload, Models.Domain.TipoImportacao.AvaliacaoExternaUnidadeAdmin);
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
				AvaliacaoExternaUnidadeAdminDetalheService avaliacaoExternaUnidadeAdminDetalheService = new AvaliacaoExternaUnidadeAdminDetalheService();

				ArquivoImportacao arquivoImportacao = avaliacaoExternaUnidadeAdminDetalheService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };

				avaliacaoExternaUnidadeAdminDetalheService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
					avaliacaoExternaUnidadeAdminDetalheService.Import(arquivoImportacao, UsuarioLogado);
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
        public ActionResult Delete(int IdAvaliacaoExterna, int idUnidadeAdministrativa)
        {
            try
            {
                AvaliacaoExternaUnidadeAdminService avaliacaoExternaUnidadeAdminService = new AvaliacaoExternaUnidadeAdminService();

                avaliacaoExternaUnidadeAdminService.Delete(IdAvaliacaoExterna, idUnidadeAdministrativa, UsuarioLogado.Ciclo, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir nível de ensino unidade administrativa");
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult DeleteImport(short id)
        {
            try
            {
                AvaliacaoExternaUnidadeAdminService avaliacaoExternaUnidadeAdminService = new AvaliacaoExternaUnidadeAdminService();

                avaliacaoExternaUnidadeAdminService.Delete(id);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir arquivo de importação");
            }
        }

        private FiltroAvaliacaoExternaUnidadeAdmin InicializaTelaFiltro(FiltroAvaliacaoExternaUnidadeAdmin filtro)
        {
            AvaliacaoExternaService avaliacaoExternaService = new AvaliacaoExternaService(ModelState);

            filtro.AvaliacoesExternas = avaliacaoExternaService.List().ToSelectList<AvaliacaoExterna>(o => o.IdAvaliacaoExterna.Value, o => o.DesAvaliacaoExterna);

            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();

            filtro.UnidadesAdministrativas = unidadeAdministrativaService.List().ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);

            return filtro;
        }

        public ActionResult FormatoArquivo()
        {
            return PartialView();
        }

        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            AvaliacaoExternaUnidadeAdminService avaliacaoExternaUnidadeAdminService = new AvaliacaoExternaUnidadeAdminService();

            IList<ArquivoImportacao> result = avaliacaoExternaUnidadeAdminService.ListByTipoImportacao(Models.Domain.TipoImportacao.AvaliacaoExternaUnidadeAdmin);

            return result;
        }

		public ActionResult Detalhes(int idUnidade)
		{
			FiltroAvaliacaoExternaUnidadeAdmin filtro = new FiltroAvaliacaoExternaUnidadeAdmin { IdUnidadeAdministrativa = idUnidade };
			FiltroAvaliacaoExternaUnidadeAdminDetalhe filtroDetalhes = new FiltroAvaliacaoExternaUnidadeAdminDetalhe() { IdUnidadeAdministrativa = filtro.IdUnidadeAdministrativa, IdAnoReferencia = UsuarioLogado.Ciclo };

			try
			{
				AvaliacaoExternaUnidadeAdminService avaliacaoExternaUnidadeAdminService = new AvaliacaoExternaUnidadeAdminService();
				AvaliacaoExternaUnidadeAdminDetalheService avaliacaoExternaUnidadeAdminDetalheService = new AvaliacaoExternaUnidadeAdminDetalheService();
								
				filtro.AvaliacoesUnidades = avaliacaoExternaUnidadeAdminService.List(filtro, UsuarioLogado.Ciclo, 1, Constants.gridPageSize);
				filtro.AvaliacoesUnidades.Title = "Participação Geral";

				filtro.DetalhesAvaliacoesUnidades = avaliacaoExternaUnidadeAdminDetalheService.List(filtroDetalhes, 1, Constants.gridPageSize);
				filtro.DetalhesAvaliacoesUnidades.Title = "Participação Importada";

			}
			catch (Exception e)
			{
				ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
			}

			return PartialView(filtro);
		}

        [HttpPost]
        public ActionResult Detalhes(int idUnidade, int? pageAvaliacoesUnidades, int? pageDetalhesAvaliacoesUnidades)
        {
            FiltroAvaliacaoExternaUnidadeAdmin filtro = new FiltroAvaliacaoExternaUnidadeAdmin()
            {
                IdUnidadeAdministrativa = idUnidade
            };                
            FiltroAvaliacaoExternaUnidadeAdminDetalhe filtroDetalhes = new FiltroAvaliacaoExternaUnidadeAdminDetalhe() 
            { 
                IdUnidadeAdministrativa = filtro.IdUnidadeAdministrativa, 
                IdAnoReferencia = UsuarioLogado.Ciclo 
            };
            AvaliacaoExternaUnidadeAdminService avaliacaoExternaUnidadeAdminService = new AvaliacaoExternaUnidadeAdminService();
            AvaliacaoExternaUnidadeAdminDetalheService avaliacaoExternaUnidadeAdminDetalheService = new AvaliacaoExternaUnidadeAdminDetalheService();

            try
            {
                filtro.AvaliacoesUnidades = avaliacaoExternaUnidadeAdminService.List(filtro, UsuarioLogado.Ciclo, pageAvaliacoesUnidades ?? 1, Constants.gridPageSize);
                filtro.AvaliacoesUnidades.Title = "Participação Geral";

                filtro.DetalhesAvaliacoesUnidades = avaliacaoExternaUnidadeAdminDetalheService.List(filtroDetalhes, pageDetalhesAvaliacoesUnidades ?? 1, Constants.gridPageSize);
                filtro.DetalhesAvaliacoesUnidades.Title = "Participação Importada";
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return PartialView(filtro);
        }
    }
}
