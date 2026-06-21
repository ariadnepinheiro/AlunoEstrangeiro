using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Common.Exceptions;
using SRV.Common.View;

namespace SRV.Controllers
{
    public class AlteracaoSenhaController : BaseController
    {
        //
        // GET: /AlteracaoSenha/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AlteracaoSenha alteracaoSenha)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioService usuarioService = new UsuarioService();

                    UserState usuario = usuarioService.AlterarSenha(alteracaoSenha, UsuarioLogado);

                    //Atualiza usuário na sessão
                    Session["user"] = usuario;

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao alterar senha", ModelState);
            }

            return View();
        }


        public ActionResult Modal()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Modal(AlteracaoSenha alteracaoSenha)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioService usuarioService = new UsuarioService();

                    UserState usuario = usuarioService.AlterarSenha(alteracaoSenha, UsuarioLogado);
                }
                else
                {
                    return Json(new
                    {
                        ValidationMessage = (from item in ModelState.Values
                                   from error in item.Errors
                                   select error.ErrorMessage).ToList()
                    });
                }
            }
            catch (Exception e)
            {
                return new HttpCustomStatusCodeResult(400, e.Message);
            }

            return Json(new { SuccessMessage = "Senha alterada com sucesso" });
        }

    }
}
