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
    public class IndicadorUnidadeAdministrativaController : BaseController
    {
        //
        // GET: /IndicadorUnidadeAdministrativa/


        public ActionResult Index()
        {
            FiltroIndicadorUnidadeAdministrativa filtro = new FiltroIndicadorUnidadeAdministrativa();

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroIndicadorUnidadeAdministrativa filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IndicadorUnidadeAdministrativaService indicadorUnidadeAdministrativaService = new IndicadorUnidadeAdministrativaService();
                    filtro.IndicadoresUnidades = indicadorUnidadeAdministrativaService.List(filtro, UsuarioLogado.Ciclo, page ?? 1, Constants.gridPageSize);
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
                UploadFile(fileUpload, Models.Domain.TipoImportacao.IndicadorUnidadeAdministrativa);
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
                IndicadorUnidadeAdministrativaService indicadorUnidadeAdministrativaService = new IndicadorUnidadeAdministrativaService();

                ArquivoImportacao arquivoImportacao = indicadorUnidadeAdministrativaService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };

                indicadorUnidadeAdministrativaService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    indicadorUnidadeAdministrativaService.Import(arquivoImportacao, UsuarioLogado);
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
        public ActionResult Delete(int idUnidadeAdministrativa, int idModalidade, int idNivelEnsino, int idIndicador)
        {
            try
            {
                IndicadorUnidadeAdministrativaService indicadorUnidadeAdministrativaService = new IndicadorUnidadeAdministrativaService();

                indicadorUnidadeAdministrativaService.Delete(idUnidadeAdministrativa, idModalidade, idNivelEnsino, idIndicador, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir indicador de unidade administrativa");
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult DeleteImport(short id)
        {
            try
            {
                IndicadorUnidadeAdministrativaService indicadorUnidadeAdministrativaService = new IndicadorUnidadeAdministrativaService();

                indicadorUnidadeAdministrativaService.Delete(id);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir arquivo de importação");
            }
        }

        private FiltroIndicadorUnidadeAdministrativa InicializaTelaFiltro(FiltroIndicadorUnidadeAdministrativa filtro)
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
            IndicadorUnidadeAdministrativaService indicadorUnidadeAdministrativaService = new IndicadorUnidadeAdministrativaService();

            IList<ArquivoImportacao> result = indicadorUnidadeAdministrativaService.ListByTipoImportacao(Models.Domain.TipoImportacao.IndicadorUnidadeAdministrativa);

            return result;
        }

        [HttpGet]
        public JsonResult GetNivelEnsino(int idModalidade)
        {
            NivelEnsinoService nivelEnsinoService = new NivelEnsinoService(ModelState);
            return Json(new { Result = nivelEnsinoService.ListByModalidade(idModalidade) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FormatoArquivo()
        {
            return PartialView();
        }


    }
}
