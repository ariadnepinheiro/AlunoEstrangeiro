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

namespace SRV.Controllers
{
    public class AuditoriaController : BaseController
    {
        //
        // GET: /LogAuditoria/

        public ActionResult Index()
        {
            FiltroLogAuditoria filtro = new FiltroLogAuditoria();

            filtro = InicializaTelaFiltro(filtro);

            @ViewBag.IsFirstTime = true;

            return View(filtro);
        }

        [HttpPost]
        public ActionResult Index(FiltroLogAuditoria filtro, int? page)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LogAuditoriaService logAuditoriaService = new LogAuditoriaService();
                    filtro.Logs = logAuditoriaService.List(filtro, page ?? 1, Constants.gridPageSize);
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            filtro = InicializaTelaFiltro(filtro);

            @ViewBag.IsFirstTime = false;

            return View(filtro);
        }

		[CustomAuthorize(Roles = "Administrador")]
		public ActionResult ListarRecursos(FiltroLogAuditoria filtro, int? page, int idServidor, string nomeServidor)
		{
			if (filtro == null)
			{
				filtro = new FiltroLogAuditoria() { Logs = new Paging<LogAuditoria>() };
			}
			else
			{
				filtro.Logs = new Paging<LogAuditoria>();
			}

			filtro.IdUsuario = idServidor;
			filtro.LoginUsuario = idServidor.ToString();
			filtro.NomeUsuario = nomeServidor;

			LogAuditoriaService logAuditoriaService = new LogAuditoriaService();
			filtro.Logs.Items = logAuditoriaService.ListRecurso(filtro);

			filtro = InicializaTelaFiltro(filtro);

			ViewBag.IsFirstTime = true;
			ViewBag.Recurso = true;

			return View("Index",filtro);
		}

        public ActionResult Detalhes(int id)
        {
            LogAuditoriaItemService logAuditoriaItemService = new LogAuditoriaItemService();

            IList<LogAuditoriaItem> result = logAuditoriaItemService.List(id);

            return PartialView(result);
        }

        private FiltroLogAuditoria InicializaTelaFiltro(FiltroLogAuditoria filtro)
        {
            filtro.OperacoesAuditoria = OperacaoAuditoria.Inclusao.ToSelectList();

            filtro.ObjetosAuditoria = ObjetoAuditoria.AnoReferencia.ToSelectList();

            return filtro;
        }
    }
}
