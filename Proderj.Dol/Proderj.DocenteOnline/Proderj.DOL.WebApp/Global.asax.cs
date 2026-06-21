using System;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using Proderj.DOL.Service;
using Proderj.DOL.Service.Profiles;
using Proderj.DOL.WebApp.Models;
using Proderj.DOL.WebApp.Profiles;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Framework.Config;
using log4net;
using log4net.Config;

namespace Proderj.DOL.WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegistrarRotas(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{resource}.gif/{*pathInfo}");
            routes.IgnoreRoute("{resource}.jpg/{*pathInfo}");
            routes.IgnoreRoute("{resource}.png/{*pathInfo}");

            routes.MapRoute(
                "StyleSheet", // Route name
                "CSS/{funcao}", // URL with parameters
                new { controller = "Css", action = "Carrega", funcao = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Javascript", // Route name
                "JS/{funcao}",// URL with parameters
                new { controller = "Js", action = "Carrega", funcao = UrlParameter.Optional } // Parameter defaults
            );            
            routes.MapRoute(
                name: "LancamentoFrequencia",
                url: "Api/LancamentoFrequencia",
                defaults: new { controller = "Api", action = "LancamentoFrequencia" }
            );
            routes.MapRoute(
                name: "LancamentoNotas",
                url: "Api/LancamentoNotas",
                defaults: new { controller = "Api", action = "GetLancamentoNotas" }
            );
            routes.MapRoute(
                name: "CadastroGLP",
                url: "Api/CadastroGLP",
                defaults: new { controller = "Api", action = "CadastroGLP" }
            );
            routes.MapRoute(
                name: "Relatorios",
                url: "Api/Relatorios",
                defaults: new { controller = "Api", action = "Relatorios" }
            );
            routes.MapRoute(
                name: "ProtocoloNota",
                url: "Api/ProtocoloNota",
                defaults: new { controller = "Api", action = "ProtocoloNota" }
            ); 
            routes.MapRoute(
                name: "DadosDocente",
                url: "Api/DadosDocente",
                defaults: new { controller = "Api", action = "DadosDocente" }
            );
            routes.MapRoute(
                name: "DadosPessoais",
                url: "Api/DadosPessoais",
                defaults: new { controller = "Api", action = "DadosPessoais" }
            );
            routes.MapRoute(
                name: "ResultadoAvaliacao",
                url: "Api/ResultadoAvaliacao",
                defaults: new { controller = "Api", action = "GetResultadoAvaliacao" }
            );
            routes.MapRoute(
                name: "AnaliseRendimento",
                url: "Api/AnaliseRendimento",
                defaults: new { controller = "Api", action = "GetAnaliseRendimento" }
            );
            routes.MapRoute(
                "SelecaoTurmas", // Route name
                "{controller}/{action}/{nomeController}", // URL with parameters
                new { controller = "Login", action = "Inicial" } // Parameter defaults
            );

            routes.MapRoute(
                name: "AlteraSenha",
                url: "Api/AlteraSenha",
                defaults: new { controller = "Api", action = "AlteraSenha" }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Login", action = "Inicial", id = UrlParameter.Optional } // Parameter defaults
            );

            log4net.Config.XmlConfigurator.Configure();
        }

        private void VerificaEConfiguraNHibernate()
        {
            var scriptExecucao = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
            if (scriptExecucao.IndexOf("/CSS", StringComparison.CurrentCultureIgnoreCase) == -1 &&
                scriptExecucao.IndexOf("/JS", StringComparison.CurrentCultureIgnoreCase) == -1 &&
                scriptExecucao.IndexOf("/Imagens", StringComparison.CurrentCultureIgnoreCase) == -1)
            {

                try
                {
                    ConfiguracaoDeAplicacao.Instance.Config(ContextClassEnum.Web);
                }
                catch (System.Exception excecao)
                {
                    HttpCookie cookieErro, cookieStack;
                    if (excecao.InnerException != null)
                    {
                        cookieErro = new HttpCookie("MensagemErroGeral", excecao.InnerException.Message);
                        cookieStack = new HttpCookie("StackErroGeral", excecao.InnerException.StackTrace);
                    }
                    else
                    {
                        cookieErro = new HttpCookie("MensagemErroGeral", excecao.Message);
                        cookieStack = new HttpCookie("StackErroGeral", excecao.StackTrace);
                    }

                    cookieErro.HttpOnly = true;
                    cookieStack.HttpOnly = true;

                    HttpContext.Current.Response.Cookies.Add(cookieErro);
                    HttpContext.Current.Response.Cookies.Add(cookieStack);

                    //Exibe erro na tela

                    HttpContext.Current.Response.Write(RenderizaViewErroGeral());
                    HttpContext.Current.Response.End();

                    //Aborta response
                }
            }
        }

        protected void Application_Start()
        {
            //Desativa envio no cabeçalho http da informação de versão do MVC da aplicação.
            //Este envio além de ser desnecessário, trata-se de uma vulnerabilidade.
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            //Binders

            DefaultModelBinder.ResourceClassKey = "MvcRecursos";

            ModelBinders.Binders.Add(typeof(DocenteLogadoBindModel), new DocenteLogadoModelBinder());
            AppDomain.CurrentDomain.SetData("ExecutingDir", System.IO.Directory.GetCurrentDirectory());
            RegistrarRotas(RouteTable.Routes);

            ConfiguraAutoMapper();
            //VerificaEConfiguraNHibernate();

            XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

        }

        private void ConfiguraAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new AvaliacaoCurriculoMinimoServiceProfile());
                cfg.AddProfile(new CadastroGLPServiceProfile());
                cfg.AddProfile(new DocenteServiceProfile());
                cfg.AddProfile(new NucleoServiceProfile());
                cfg.AddProfile(new RespostaCurriculoMinimoServiceProfile());
                cfg.AddProfile(new SelecaoTurmasServiceProfile());
                cfg.AddProfile(new SubPeriodoLetivoServiceProfile());

                cfg.AddProfile(new DocenteWebProfile());
                cfg.AddProfile(new LancamentoNotasWebProfile());
            });

            Mapper.AssertConfigurationIsValid();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            #region application.Context.Response.Headers.Remove("Server")
#if DEBUG
            //Evita exception do tipo System.PlatformNotSupportedException.
            //Mensagem de erro: This operation requires IIS integrated pipeline mode.
#else
            HttpApplication application = sender as HttpApplication;

            if (application != null && application.Context != null)
            {
                //Desativa envio no cabeçalho http da informação de versão servidor IIS.
                //Este envio além de ser desnecessário, trata-se de uma vulnerabilidade.
                application.Context.Response.Headers.Remove("Server");
            }
#endif
            #endregion

            
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            NHSession.EncerraSessao();

            HttpApplication app = (HttpApplication)sender;
            HttpResponse response = app.Response;
            HttpRequest request = app.Request;

            if (!string.IsNullOrEmpty(request.Url.PathAndQuery))
            {
                if (request.Url.PathAndQuery.Contains(".js"))
                {
                    response.ContentType = "application/javascript";
                }
                else if (request.Url.PathAndQuery.Contains(".css"))
                {
                    response.ContentType = "text/css";
                }
            }
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            VerificaEConfiguraNHibernate();

            var nomeCookieAutenticacaoForms = FormsAuthentication.FormsCookieName;
            var cookieAutenticacaoForms = Request.Cookies[nomeCookieAutenticacaoForms];

            if (cookieAutenticacaoForms != null)
            {
                IPrincipal docentePrincipal = LoginService.ObtemDocenteLogadoPrincipalPor(cookieAutenticacaoForms.Value);
                HttpContext.Current.User = docentePrincipal;
            }
        }

        public static string RenderizaViewErroGeral()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var fabricaController = ControllerBuilder.Current.GetControllerFactory();

            return
                RenderViewToString(
                    (Controller)fabricaController.CreateController(new RequestContext(httpContext, new RouteData()), "Login"),
                    "ErroGeral", new ViewDataDictionary());
        }

        /// <summary>Renders a view to string.</summary>
        public static string RenderViewToString(Controller controller,
                                                string viewName, object viewData)
        {
            //Create memory writer
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);

            //Create fake http context to render the view
            var fakeResponse = new HttpResponse(memWriter);
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Login");
            routeData.Values.Add("action", "ErroGeral");

            var fakeContext = new HttpContextWrapper(HttpContext.Current);
            var fakeControllerContext = new ControllerContext(
                fakeContext,
                routeData,
                controller);

            //var oldContext = HttpContext.Current;
            //HttpContext.Current = fakeContext;

            //Use HtmlHelper to render partial view to fake context
            var html = new HtmlHelper(new ViewContext(fakeControllerContext,
                new WebFormView("ErroGeral"), new ViewDataDictionary(), new TempDataDictionary(), fakeResponse.Output),
                new ViewPage());
            html.RenderPartial(viewName, viewData);

            //Restore context
            //HttpContext.Current = oldContext;

            //Flush memory and return output
            memWriter.Flush();
            return sb.ToString();
        }

        private void Application_Error(object sender, EventArgs e)
        {
            System.Exception ex = Server.GetLastError();

            log4net.ILog logger = log4net.LogManager.GetLogger("FileAppender");
            logger.Error("Erro - ", ex);

            // LIMPA O ERRO
            Server.ClearError();

            // EVITA PROBLEMAS DE RESPONSE JÁ INICIADO
            Response.Clear();

            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
    }
}