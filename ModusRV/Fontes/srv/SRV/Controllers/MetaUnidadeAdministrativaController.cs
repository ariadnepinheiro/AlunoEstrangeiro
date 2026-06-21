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
    public class MetaUnidadeAdministrativaController : BaseController
    {
        //
        // GET: /MetaUnidadeAdministrativa/

        public ActionResult Index()
        {
            FiltroMetaUnidadeAdministrativa filtro = new FiltroMetaUnidadeAdministrativa();

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroMetaUnidadeAdministrativa filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    filtro.IdAnoReferencia = UsuarioLogado.Ciclo;

                    MetaUnidadeAdministrativaService metaUnidadeAdministrativaService = new MetaUnidadeAdministrativaService();
                    filtro.Metas = metaUnidadeAdministrativaService.List(filtro, page ?? 1, Constants.gridPageSize);
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
        public ActionResult Delete(int idUnidadeAdministrativa, int idNivelEnsino, int idIndicador, int idModalidade, int idAnoReferencia)
        {
            try
            {
                MetaUnidadeAdministrativaService metaUnidadeAdministrativaService = new MetaUnidadeAdministrativaService();

                metaUnidadeAdministrativaService.Delete(idUnidadeAdministrativa, idNivelEnsino, idIndicador, idModalidade, idAnoReferencia, UsuarioLogado);

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
                UploadFile(fileUpload, Models.Domain.TipoImportacao.MetaUnidadeAdministrativa);
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
                MetaUnidadeAdministrativaService metaUnidadeAdministrativaService = new MetaUnidadeAdministrativaService();

                ArquivoImportacao arquivoImportacao = metaUnidadeAdministrativaService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };

                metaUnidadeAdministrativaService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    metaUnidadeAdministrativaService.Import(arquivoImportacao, UsuarioLogado);
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
                MetaUnidadeAdministrativaService metaUnidadeAdministrativaService = new MetaUnidadeAdministrativaService();

                metaUnidadeAdministrativaService.Delete(id);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir arquivo de importação");
            }
        }

        private FiltroMetaUnidadeAdministrativa InicializaTelaFiltro(FiltroMetaUnidadeAdministrativa filtro)
        {
            ModalidadeService modalidadeService = new ModalidadeService(ModelState);
            filtro.Modalidades = modalidadeService.List().ToSelectList<Modalidade>(o => o.IdModalidade.Value, o => o.DesModalidade);

            if (filtro.IdModalidade != null)
            {
                NivelEnsinoService nivelEnsinoService = new NivelEnsinoService(ModelState);
                filtro.NiveisEnsino = nivelEnsinoService.ListByModalidade(filtro.IdModalidade.Value).ToSelectList<NivelEnsino>(o => o.IdNivelEnsino.Value, o => o.DesNivelEnsino);
            }

            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();
            filtro.UnidadesAdministrativas = unidadeAdministrativaService.List().ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);

            IndicadorService indicadorService = new IndicadorService(ModelState);
            filtro.Indicadores = indicadorService.List().ToSelectList<Indicador>(o => o.IdIndicador.Value, o => o.DesIndicador);

            return filtro;
        }

        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            MetaUnidadeAdministrativaService metaUnidadeAdministrativaService = new MetaUnidadeAdministrativaService();

            IList<ArquivoImportacao> result = metaUnidadeAdministrativaService.ListByTipoImportacao(Models.Domain.TipoImportacao.MetaUnidadeAdministrativa);

            return result;
        }

        [HttpGet]
        public JsonResult GetNivelEnsino(int idModalidade)
        {
            NivelEnsinoService nivelEnsinoService = new NivelEnsinoService(ModelState);
            return Json(new { Result = nivelEnsinoService.ListByModalidade(idModalidade) }, JsonRequestBehavior.AllowGet);
        }
    }
}
