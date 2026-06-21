using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Net;

namespace SRV.Common
{
    /// <summary>
    /// Disponibliza informações sobre a aplicação
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// Retorna a versão da aplicação (Assembly) que é configurada no arquivo AsseblyInfo.cs
        /// A versão fica armazenada no objeto Application inicializada no Global.asax
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            HttpApplicationState applicationState = HttpContext.Current.Application;
            return applicationState["AppVersion"] != null ? applicationState["AppVersion"].ToString() : "";
        }

        /// <summary>
        /// Retorna o nome do servidor que está atentendo a requisição atual
        /// </summary>
        /// <returns></returns>
        public static string GetServerName()
        {
            return Dns.GetHostName();
        }

    }
}