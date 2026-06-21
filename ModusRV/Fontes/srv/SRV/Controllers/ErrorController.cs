using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRV.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound(string aspxErrorPath)
        {
            ViewBag.Path = aspxErrorPath;

            return View();
        }
    }
}
