using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proderj.DOL.WebApp.Controllers
{
    public class JsController : Controller
    {
        //
		// GET: /JS/
		public ActionResult Carrega(string funcao)
		{          
            //ControllerContext.RequestContext.HttpContext.Response.ContentType = "text/css";
			//Response.ContentType = "text/css";

			//É importante usar cast de object aqui senao a sobrecarga da função View() que recebe string vai tentar carregar 
			//outra View diferente da Carrega.aspx
			return View((object)funcao);
		}
    }
}
