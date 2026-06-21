using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.Service;

namespace SRV.Controllers
{
    public class LogImportacaoController : Controller
    {
        //
        // GET: /LogImportacao/

        public ActionResult Index(int id)
        {
            ArquivoImportacaoLogService arquivoImportacaoService = new ArquivoImportacaoLogService();

            IList<String> result = arquivoImportacaoService.List(id);
           
            return View(result);
        }

    }
}
