using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using SRV.Models.DTO;
using SRV.Common.View;
using SRV.Models.Service;
using System.Configuration;
using SRV.Common;

namespace SRV.Filters
{
    [AttributeUsageAttribute(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private string _controller;
        private UserState _user;

        public CustomAuthorizeAttribute()
        { 

        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            //string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.Name;
            _controller = controllerName.Substring(0, controllerName.Length - "Controller".Length);

            AuthorizeControllerAttribute[] att = (AuthorizeControllerAttribute[])filterContext.Controller.GetType().GetCustomAttributes(typeof(AuthorizeControllerAttribute), false);

            if (att.Length > 0)
                _controller = att[0].Controller;

            //Faz a leitura do usuário na sessão e grava no ViewBag 
            //para ser utilizado nos demais filtros e controllers
            UserState user = (UserState)filterContext.HttpContext.Session["user"];


            //Auto login para ambiente de desenvolvimento, configurado no web.config
            string autoLogin = ConfigurationManager.AppSettings["AutoLogin"];

            if (user == null && autoLogin != null && Boolean.Parse(autoLogin))
            {
                Login login = new Login();
                login.Usuario = ConfigurationManager.AppSettings["AutoLoginUser"];
                login.Senha = ConfigurationManager.AppSettings["AutoLoginPass"];
                login.Ciclo = Convert.ToInt32(ConfigurationManager.AppSettings["AutoLoginCiclo"]);

                LoginService loginService = new LoginService();
                UserState userDesenv = loginService.Login(login, null);

                Menu menu = new Menu();
                userDesenv.Menu = menu.ReadMenu(userDesenv.Perfil.ToString());

                filterContext.HttpContext.Session["user"] = userDesenv;
                user = userDesenv;
            }

            _user = user;

            filterContext.Controller.ViewBag.UsuarioLogado = user;

            //Tratamento para requisições Ajax
            if (user == null && filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (filterContext.HttpContext.Request.FilePath.EndsWith("/Solicitacao"))
                {
                    return;
                }
                else
                {
                    //Não pode ser retornado erro 401 (Unauthorized) pois o ASP irá redirecinar para tela de login
                    filterContext.Result = new HttpStatusCodeResult(403, "Não autorizado");
                }

                return;
            }
            else if (user != null && !filterContext.HttpContext.Request.FilePath.EndsWith("/AlteracaoSenha/Index")
                    && (user.AlterarSenha))
            {
                filterContext.Result = new RedirectResult("~/AlteracaoSenha/Index");
                return;
            }
            else if (filterContext.HttpContext.Request.FilePath.EndsWith("/NovaSenha"))
            {
                return;
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (_user == null)
            {
                return false;
            }

            if (SplitString(Roles).Length > 0 && !SplitString(Roles).Contains(_user.Perfil.ToString()))
            {
                //httpContext.Response.Redirect("~/Error/Denied", true);
                return false;
            }

            return true;
        }


        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }


    }
}