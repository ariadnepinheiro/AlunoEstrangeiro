using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Domain;
using SRV.Models.Service;
using SRV.Common.Exceptions;
using SRV.Common.Extension;
using SRV.Common;
using SRV.Filters;
using System.Threading;

namespace SRV.Controllers
{
    public class UnidadeAdministrativaController : BaseController
    {
        //
        // GET: /UnidadeAdministrativa/


        public ActionResult Index()
        {
            FiltroUnidadeAdministrativa filtro = new FiltroUnidadeAdministrativa();

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroUnidadeAdministrativa filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();
                    filtro.Unidades = unidadeAdministrativaService.ListPesquisa(filtro, page ?? 1, Constants.gridPageSize);
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
            UploadUnidadeAdministrativa upload = new UploadUnidadeAdministrativa();
            upload = InicializaTelaUpload(upload);

            return View(upload);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Upload(HttpPostedFileBase fileUpload)
        {
            try
            {
                UploadFile(fileUpload, Models.Domain.TipoImportacao.UnidadeAdministrativa);
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha no upload", ModelState);
            }

            UploadUnidadeAdministrativa upload = new UploadUnidadeAdministrativa();
            upload = InicializaTelaUpload(upload);

            return View(upload);
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Import(int? idArquivoImportacao, TipoUnidadeAdministrativa.Tipos_Upload? tipoUpload)
        {
            try
            {
                UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();

                ArquivoImportacao arquivoImportacao = unidadeAdministrativaService.Find(idArquivoImportacao.Value);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };

                unidadeAdministrativaService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                if (tipoUpload == TipoUnidadeAdministrativa.Tipos_Upload.REGIONAL)
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        unidadeAdministrativaService.ImportRegional(arquivoImportacao, UsuarioLogado);
                    });
                }
                else if (tipoUpload == TipoUnidadeAdministrativa.Tipos_Upload.ESCOLAR)
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        unidadeAdministrativaService.ImportEscolar(arquivoImportacao, UsuarioLogado);
                    });
                }
                else if (tipoUpload == TipoUnidadeAdministrativa.Tipos_Upload.DEMAIS_UNIDADES)
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        unidadeAdministrativaService.ImportDemaisUnidades(arquivoImportacao, UsuarioLogado);
                    });
                }
                else
                    throw new BusinessException("Tipo de Unidade Administrativa é obrigatório");
                
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Tipo de Unidade Administrativa é obrigatório");
            }

            return Json(true);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();

                unidadeAdministrativaService.Delete(id, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir unidade administrativa");
            }
        }


        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult DeleteImport(short id)
        {
            try
            {
                UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();

                unidadeAdministrativaService.Delete(id);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir arquivo de importação");
            }
        }

        private FiltroUnidadeAdministrativa InicializaTelaFiltro(FiltroUnidadeAdministrativa filtro)
        {

            TipoUnidadeAdministrativaService tipoUnidadeAdministrativaService = new TipoUnidadeAdministrativaService(ModelState);

            filtro.TiposUnidadesAdministrativa = tipoUnidadeAdministrativaService.List().ToSelectList<TipoUnidadeAdministrativa>(o => o.IdTipoUnidAdm.Value, o => o.DesTipoUnidAdm);

            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();

            filtro.Regionais = unidadeAdministrativaService.ListRegional(UsuarioLogado.Ciclo, UsuarioLogado).ToSelectList<UnidadeAdministrativa>(o => o.IdUnidadeAdministrativa.Value, o => o.DesUnidadeAdministrativa);

            return filtro;
        }

        public ActionResult FormatoArquivo()
        {
            return PartialView();
        }

        private UploadUnidadeAdministrativa InicializaTelaUpload(UploadUnidadeAdministrativa upload)
        {
            UnidadeAdministrativaService unidadeAdministrativaService = new UnidadeAdministrativaService();

            upload.Arquivos = unidadeAdministrativaService.ListByTipoImportacao(Models.Domain.TipoImportacao.UnidadeAdministrativa);

            upload.TiposUpload = SRV.Models.Domain.TipoUnidadeAdministrativa.Tipos_Upload.REGIONAL.ToSelectList();

            return upload;
        }

    }
}
