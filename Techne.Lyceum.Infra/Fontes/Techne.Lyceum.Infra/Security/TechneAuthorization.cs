using System;
using System.Web;

namespace Techne
{
    public class TechneAuthorization
    {
        private TechneAuthorization()
        {
        }

        /// <summary>
        ///   Verifica se o usuário corrente tem permissão de acesso um recurso do sistema.
        /// </summary>
        /// <param name = "resource">Nome do recurso</param>
        /// <param name = "resourcetype">Tipo do recurso ("PAGINA", "RELATORIO", etc)</param>
        /// <returns>True se o usuário tem permissão. False se o usuário não tem permissão ou se o recurso não existe na aplicação.</returns>
        public static bool IsAuthorized(string resource, string resourcetype)
        {
            var app = HttpContext.Current.ApplicationInstance as TechneHttpApplication;
            if (app == null)
            {
                throw new ApplicationException("Erro de configuração. TechneHttpApplication");
            }

            var perm = app.GetPermission(resource, resourcetype);
            if (perm != null)
            {
                return perm.Execute;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///   Verifica se o usuário corrente tem permissão para acessar uma URL
        /// </summary>
        /// <param name = "url">URL verificada</param>
        /// <returns>True se o usuário tem permissão. False se o usuário não tem permissão ou se a url não existe na aplicação.</returns>
        public static bool IsUrlAuthorized(string url)
        {
            // Sai se não houver contexto http
            if (HttpContext.Current == null || HttpContext.Current.User == null)
            {
                return false;
            }

            var app = HttpContext.Current.ApplicationInstance as TechneHttpApplication;

// Se a classe Global (Global.asax) não for derivada de TechneHttpApplication, abre a segurança.
            if (app == null)
            {
                return true;
            }

            // Sai se a url for vazia
            if (url == null || url.Trim().Length == 0)
            {
                return false;
            }

            var perm = app.GetPermission(TUtil.GetRelativeUrl(url), "PAGINA");
            if (perm != null)
            {
                return perm.Execute;
            }
            else
            {
                return false;
            }
        }
    }
}