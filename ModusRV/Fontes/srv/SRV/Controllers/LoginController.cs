using System;
using System.Web.Mvc;
using SRV.Models.Service;
using SRV.Models.Domain;
using SRV.Common.Exceptions;
using SRV.Common.Extension;
using SRV.Models.DTO;
using SRV.Common;
using System.Xml;
using System.Linq;

namespace SRV.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class LoginController : Controller
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoginController));

        public ActionResult Error()
        {
            return View();
        }

        //
        // GET: /Login/

        public ActionResult Index()
        {
            Login login = new Login();

            try
            {
                ClearSession();

                AnoReferenciaService anoReferenciaService = new AnoReferenciaService(ModelState);

                login.Ciclos = anoReferenciaService.List().ToSelectList<AnoReferencia>(o => o.IdAnoReferencia.Value, o => o.IdAnoReferencia.ToString());
            }
            catch (Exception e)
            {
                ExceptionHandler.Execute(e, "Falha ao carregar login. Favor recarregar a tela", ModelState);
            }

            return View(login);
        }

        [HttpPost]
        public ActionResult Index(Login login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string clientIP = GetClientIP();

                    LoginService loginService = new LoginService();

                    UserState user = loginService.Login(login, clientIP);

                    user.IPCliente = clientIP;

                    Menu menu = new Menu();
                    user.Menu = menu.ReadMenu(user.Perfil.ToString());

                    Session["user"] = user;

                    if (user.Perfil == Perfil.Servidor)
                        return RedirectToAction("Index", "RptExtratoServidor");

                    if (user.AlterarSenha)
                        return RedirectToAction("Index", "AlteracaoSenha");

                    if (!String.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
                catch(Exception e)
                {
                    ExceptionHandler.Execute(e, "Falha ao efetuar login", ModelState);
                }
            }

            return Index();
        }

        public ActionResult Logout()
        {
            Session.Remove("user");

            return RedirectToAction("Index");
        }

        private void ClearSession()
        {
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-1);
            }
            Session.Abandon();
        }

        public string GetClientIP()
        {
            string ipAddress = String.IsNullOrEmpty(Request.Headers.Get("x-forwarded-for")) 
                ? Request.UserHostAddress : Request.Headers.Get("x-forwarded-for");

            if (ipAddress.Contains(","))
                ipAddress = ipAddress.Split(',')[0];

            return ipAddress;
        }


    }
}
