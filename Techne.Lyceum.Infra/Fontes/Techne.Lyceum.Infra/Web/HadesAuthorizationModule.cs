using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Security;
using Techne.Web.Hades;

namespace Techne.Web
{
    internal class HadesAuthorizationModule : IHttpModule
    {
        private readonly static List<string> BypassExtensions = new List<string>(new[] { ".js", ".css", ".gif", ".config", ".jpg", ".png", ".ico", ".html", ".htm", ".swf" });

        private static readonly List<string> httpHandlersPermitidos = new List<string>(new[] { ".techne.ashx", ".axd", ".png", "webservice.htc" });

        private static Dictionary<string, HdTransacao> Transacoes
        {
            get
            {
                Dictionary<string, HdTransacao> dic = null;
                if (HttpContext.Current != null && HttpContext.Current.Cache["ListaDeTransacoes"] as Dictionary<string, HdTransacao> != null)
                {
                    dic = HttpContext.Current.Cache["ListaDeTransacoes"] as Dictionary<string, HdTransacao>;
                }
                else
                {
                    dic = Transacao.GetTransacao(WebHelper.ApplicationName);
                    if (dic != null)
                    {
                        HttpContext.Current.Cache.Insert("ListaDeTransacoes", dic, null, DateTime.UtcNow.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }

                return dic;
            }
        }

        internal static bool VerificaPermissao(HttpContext context)
        {
            if (context == null)
            {
                return false;
            }

            var request = context.Request;
            if (request == null)
            {
                return false;
            }

            var rolesPermitidos = PadacesPorUrl(request.AppRelativeCurrentExecutionFilePath);

            // se o usuário não estiver autenticado, só permite acesso às páginas com acesso anônimo
            if (context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                if (rolesPermitidos.Contains("?"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Se for transação pública, permite acesso independentemente dos roles do usuário
                if (rolesPermitidos.Contains("*"))
                {
                    return true;
                }

// checa se a página permite acesso a algum dos roles do usuário
                foreach (var role in rolesPermitidos)
                {
                    // ignora roles público e anônimo
                    if (role == "*" || role == "?")
                    {
                        continue;
                    }

                    if (context.User.IsInRole(role))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private static IList<string> PadacesPorUrl(string url)
        {
            HdTransacao trans = null;
            if (url.Length < 2)
            {
                return new string[] { };
            }

            if (url[0] == '/')
            {
                url = "~" + url;
            }

            if (Transacoes.ContainsKey(url.Trim().ToLower()))
            {
                trans = Transacoes[url.Trim().ToLower()];
                return trans.Roles;
            }
            else
            {
                return new string[] { };
            }
        }

        void IHttpModule.Dispose()
        {
        }

        private void Init(HttpApplication context)
        {
            context.AuthorizeRequest += this.OnEnter;
        }

        void IHttpModule.Init(HttpApplication context)
        {
            this.Init(context);
        }

        private void OnEnter(object source, EventArgs eventArgs)
        {
            var application = (HttpApplication)source;
            var current = HttpContext.Current;
            var request = (current == null) ? null : current.Request;
            if (current != null)
            {
                var response = current.Response;
            }

            if ((((((current != null) && (request != null)) && !BypassExtensions.Contains(Path.GetExtension(request.PhysicalPath).ToLower())) && !httpHandlersPermitidos.Contains(Path.GetExtension(request.PhysicalPath).ToLower())) && !WebHelper.EqualUrl(request.AppRelativeCurrentExecutionFilePath, FormsAuthentication.LoginUrl)) && !VerificaPermissao(current))
            {
                if (((current.User == null) || (current.User.Identity == null)) || current.User.Identity.IsAuthenticated)
                {
                    throw new HttpException(0x193, "Acesso negado.");
                }

                FormsAuthentication.RedirectToLoginPage();
                application.CompleteRequest();
            }
        }
    }
}