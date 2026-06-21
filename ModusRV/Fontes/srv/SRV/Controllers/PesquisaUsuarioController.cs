using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Common.Exceptions;

namespace SRV.Controllers
{
    public class PesquisaUsuarioController : BaseController
    {
        //
        // GET: /PesquisaUsuario/

        public ActionResult Index()
        {
            PesquisaUsuario pesquisa = new PesquisaUsuario();

            return PartialView(pesquisa);
        }

        [HttpPost]
        public ActionResult Index(PesquisaUsuario pesquisa, int? page)
        {
            try
            {
                UsuarioService usuarioService = new UsuarioService(ModelState);

                pesquisa.Usuarios = usuarioService.ListPesquisa(pesquisa, page ?? 1, 5);
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na consulta", ModelState);
            }

            return PartialView(pesquisa);
        }

        [HttpGet]
        public JsonResult FindUsuario(string login)
        {
            UsuarioService usuarioService = new UsuarioService();
            return Json(new { Result = usuarioService.FindByLogin(login) }, JsonRequestBehavior.AllowGet);
        }
    }
}
