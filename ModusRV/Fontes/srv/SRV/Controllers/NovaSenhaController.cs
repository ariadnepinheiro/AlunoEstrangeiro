using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SRV.Common.Exceptions;
using SRV.Common.Mail;
using SRV.Common.View;
using SRV.Models.Domain;
using SRV.Models.DTO;
using SRV.Models.Service;
using SRV.Common;

namespace SRV.Controllers
{
    public class NovaSenhaController : BaseController
    {
        //
        // GET: /NovaSenha/

        public ActionResult Index(string token)
        {
            UserState usuario = new UserState();

            try
            {
                TokenAlteracaoSenhaService tokenService = new TokenAlteracaoSenhaService();
                TokenAlteracaoSenha tokenAlteracaoSenha = tokenService.Find(token);

                usuario.Id = tokenAlteracaoSenha.Usuario.Id.Value;
                usuario.IPCliente = GetClientIP();
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na solicitação", ModelState);
            }

            Menu menu = new Menu();
            usuario.Menu = menu.ReadMenu(usuario.Perfil.ToString());

            ViewBag.UsuarioLogado = usuario;

            ViewBag.CaminhoPao = new CaminhoPao();

            return View();
        }

        [HttpPost]
        public ActionResult Index(NovaSenha novaSenha)
        {
            UserState usuario = new UserState();

            try
            {
                TokenAlteracaoSenhaService tokenService = new TokenAlteracaoSenhaService();
                TokenAlteracaoSenha tokenAlteracaoSenha = tokenService.Find(novaSenha.Token);
                usuario.Id = tokenAlteracaoSenha.Usuario.Id.Value;
                usuario.IPCliente = GetClientIP();

                if (ModelState.IsValid)
                {
                    UsuarioService usuarioService = new UsuarioService(ModelState);
                    usuarioService.CriarNovaSenha(novaSenha, usuario);

                    if (ModelState.IsValid)
                    {
                        TempData["message"] = String.Format("Senha alterada com sucesso");

                        return RedirectToAction("Index", "Login");
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha na solicitação", ModelState);
            }

            Menu menu = new Menu();
            usuario.Menu = menu.ReadMenu(usuario.Perfil.ToString());

            ViewBag.UsuarioLogado = usuario;

            ViewBag.CaminhoPao = new CaminhoPao();

            return View(novaSenha);
        }


        public ActionResult Solicitacao()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Solicitacao(string login)
        {
            try
            {
                TokenAlteracaoSenhaService tokenAlteracaoSenhaService = new TokenAlteracaoSenhaService();

                TokenAlteracaoSenha tokenAlteracaoSenha = tokenAlteracaoSenhaService.Insert(login);

                if (ModelState.IsValid && tokenAlteracaoSenha != null)
                {
                    SendMail(tokenAlteracaoSenha);

                    //TODO: Redirect ou retornar mensagem se a requisição for ajax
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
            

            return Json(new { SuccessMessage = "Solicitação realizada com sucesso" });
        }

        private void SendMail(TokenAlteracaoSenha tokenAlteracaoSenha)
        {
            string url = Url.Action("Index", "NovaSenha", new RouteValueDictionary(new { token = tokenAlteracaoSenha.DesToken }), HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Host);

            StringBuilder message = new StringBuilder();

            message.Append("Olá " + tokenAlteracaoSenha.Usuario.Nome + ",\n\n");
            message.Append("Você solicitou a redefinição de sua senha de acesso ao sistema. Para concluir a solicitação, clique neste link:\n\n");
            message.Append(url + "\n\n");
            message.Append("Caso não tenha feito esta solicitação favor desconsiderar esta mensagem");

            Mail.SendMail(tokenAlteracaoSenha.Usuario.EmailUsuario, "Solicitação de nova senha", message.ToString());
        }
    }
}
