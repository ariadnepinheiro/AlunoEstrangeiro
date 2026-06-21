using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Filters;
using SRV.Models.Service;
using SRV.Common.Exceptions;
using SRV.Models.DTO;
using System.IO;
using System.Text;
using SRV.Models.Domain;
using System.Threading;

namespace SRV.Controllers
{
    public class CalculoRVController : BaseController
    {
        //
        // GET: /CalculoRV/

        [CustomAuthorize(Roles="Administrador")]
        public ActionResult Index()
        {
            CalculoRVService calculoRVService = new CalculoRVService();

            ExecucaoCalculo execucaoCalculo = calculoRVService.FindUltimaExecucao();

            return View(execucaoCalculo);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Index(string param)
        {
            try
            {
                CalculoRVService calculoRVService = new CalculoRVService();

                calculoRVService.UpdateStatusEmExecucao(UsuarioLogado);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    calculoRVService.ExecutarCalculo(UsuarioLogado.Ciclo, UsuarioLogado);
                });

                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao executar cálculo", ModelState);
            }

            return View("Index");
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Export()
        {
            try
            {
                CalculoRVService calculoRVService = new CalculoRVService();

                string content = calculoRVService.ExportCoeficienteServidor(UsuarioLogado.Ciclo);

                Response.ContentType = "text/plain";
                Response.AddHeader("content-disposition", "attachment;filename=" + String.Format("rv_coeficiente_servidor_{0}.csv", String.Format("{0:dd_MM_yyyy}", DateTime.Today)));
                Response.Clear();

                if (!String.IsNullOrEmpty(content))
                {
                    using (StreamWriter writer = new StreamWriter(Response.OutputStream, Encoding.UTF8))
                    {
                        writer.Write(content);
                    }
                }

                Response.End();

            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao exportar coeficiente dos servidores", ModelState);
            }

            return View("Index");
        }


    }
}
