using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Common.Exceptions;
using SRV.Filters;

namespace SRV.Controllers
{
    public class RptExtratoServidorController : BaseController
    {
        //
        // GET: /RptExtratoServidor/
        [CustomAuthorize(Roles = "Regional, Escola, Servidor")]
        public ActionResult Index()
        {
            RptExtratoServidor rptExtratoServidor = new RptExtratoServidor();

            try
            {
                RptExtratoServidorService rptExtratoServidorService = new RptExtratoServidorService();

                rptExtratoServidor = rptExtratoServidorService.Find(null, Convert.ToInt32(UsuarioLogado.Login), UsuarioLogado.Ciclo);

                if (rptExtratoServidor.Unidades == null)
                    throw new BusinessException("Servidor Inelegível - SOMA dos períodos de lotação inferior a meta definida para o ano de referência.");

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao carregar tela", ModelState);
            }

            return View(rptExtratoServidor);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador, Secretaria, Regional, Escola")]
        public ActionResult Index(int? idRegional, int matriculaServidor, int idAnoReferencia)
        {
            RptExtratoServidor rptExtratoServidor = new RptExtratoServidor();

            try
            {
                RptExtratoServidorService rptExtratoServidorService = new RptExtratoServidorService();

                rptExtratoServidor.Retorno = true;

                rptExtratoServidor = rptExtratoServidorService.Find(idRegional, matriculaServidor, idAnoReferencia);

                rptExtratoServidor.Retorno = true;

                if (rptExtratoServidor.Unidades == null)
                    throw new BusinessException("Servidor Inelegível - SOMA dos períodos de lotação inferior a meta definida para o ano de referência.");

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao carregar tela", ModelState);
            }

            return View("Index", rptExtratoServidor);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador, Secretaria, Regional, Escola, Servidor")]
        public ActionResult GeraExcel(int? idRegional, int matriculaServidor, int idAnoReferencia, string downloadToken)
        {
            byte[] result = null;

            RptExtratoServidor rptExtratoServidor = new RptExtratoServidor(downloadToken);

            try
            {
                RptExtratoServidorService rptExtratoServidorService = new RptExtratoServidorService();

                result = rptExtratoServidorService.GeraExcel(Server.MapPath("~/Content/RptExtratoServidor.xls"), idRegional, matriculaServidor, idAnoReferencia);

                if (ModelState.IsValid)
                {
                    Response.AppendCookie(new HttpCookie("downloadToken", rptExtratoServidor.DownloadToken));

                    return File(result, "application/vnd.ms-excel", "ExtratoRV.xls");
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao exportar relatório", ModelState);
            }

            return View("Index", rptExtratoServidor);
        }

    }
}
