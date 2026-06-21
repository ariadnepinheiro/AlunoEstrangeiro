using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using SRV.Models.DTO;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Web.Configuration;

namespace SRV
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            log4net.Config.XmlConfigurator.Configure();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Application["AppVersion"] = String.Format("{0}.{1}", version.Major, version.Minor);

            DefaultModelBinder.ResourceClassKey = "CustomMvcResources";
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            if (exception is HttpException)
            {
                //Erro no upload de arquivo muito grande
                if ((Path.GetFileName(Request.Path).Equals("Upload") || Path.GetFileName(Request.Path).Equals("UploadCreate"))
                    && ((HttpException)exception).GetHttpCode() == 500
                    && ((HttpException)exception).ErrorCode == -2147467259)
                {
                    Server.ClearError();

                    HttpRuntimeSection section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
                    double maxFileSize = section.MaxRequestLength / 1024;

                    Dictionary<string, object> tempDataDictionary = Context.Session["__ControllerTempData"] as Dictionary<string, object>;
                    
                    if (tempDataDictionary == null)
                    {
                        tempDataDictionary = new Dictionary<string, object>();
                    }
                    tempDataDictionary["message"] = "Falha no Upload. Verifique se o arquivo não excede o tamanho máximo de " + maxFileSize.ToString() + "MB.";
                    
                    HttpContext.Current.Session["__ControllerTempData"] = tempDataDictionary;

                    Response.Redirect(HttpContext.Current.Request.Url.ToString());
                }
            }
        }
    }
}