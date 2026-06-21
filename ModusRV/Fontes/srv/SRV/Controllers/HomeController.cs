using System.Configuration;
using System.Web.Mvc;
using SRV.Filters;
using SRV.Models.DTO;
using SRV.Models.Service;

namespace SRV.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            UserState usuario = ViewBag.Usuario;

            return View();
        }

        [HttpPost]
        public ActionResult Index(int? id)
        {
            return View();
        }

    }
}
