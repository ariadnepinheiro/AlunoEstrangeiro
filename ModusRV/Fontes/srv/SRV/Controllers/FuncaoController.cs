using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Common;
using SRV.Common.Exceptions;
using SRV.Filters;
using SRV.Models.Domain;
using System.Threading;
using SRV.Common.Extension;

namespace SRV.Controllers
{
    public class FuncaoController : BaseController
    {
        //
        // GET: /Funcao/

        public ActionResult Index()
        {
            FiltroFuncao filtro = new FiltroFuncao();

            filtro = InicializaTelaFiltro(filtro);

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroFuncao filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FuncaoService funcaoService = new FuncaoService();
                    filtro.Funcoes = funcaoService.List(filtro, page ?? 1, Constants.gridPageSize);
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
                UploadFile(fileUpload, Models.Domain.TipoImportacao.Funcao);
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
                FuncaoService funcaoService = new FuncaoService();

                ArquivoImportacao arquivoImportacao = funcaoService.Find(id);

                if (arquivoImportacao == null)
                    throw new BusinessException("Arquivo inválido");

                arquivoImportacao.UsuarioImportacao = new Usuario() { Id = UsuarioLogado.Id };

                funcaoService.UpdateStatus(arquivoImportacao, StatusImportacao.EmExecucao);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    funcaoService.Import(arquivoImportacao, UsuarioLogado);
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
                FuncaoService funcaoService = new FuncaoService();

                funcaoService.Delete(id);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir arquivo de importação");
            }
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(string idFuncao)
        {
            try
            {
                FuncaoService funcaoService = new FuncaoService();

                funcaoService.Delete(idFuncao, UsuarioLogado);

                return Json(new { Result = "Registro excluído com sucesso" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception e)
            {
                return ExceptionHandler.ExecuteAjax(e, "Falha ao excluir função");
            }
        }

        private FiltroFuncao InicializaTelaFiltro(FiltroFuncao filtro)
        {
            GrupoFuncaoService grupoFuncaoService = new GrupoFuncaoService(ModelState);
            filtro.GruposFuncoes = grupoFuncaoService.List().ToSelectList<GrupoFuncao>(o => o.IdGrupoFuncao.Value, o => o.DesGrupoFuncao);

            return filtro;
        }

        private IList<ArquivoImportacao> InicializaTelaUpload()
        {
            FuncaoService funcaoService = new FuncaoService();

            IList<ArquivoImportacao> result = funcaoService.ListByTipoImportacao(Models.Domain.TipoImportacao.Funcao);

            return result;
        }
    }
}
