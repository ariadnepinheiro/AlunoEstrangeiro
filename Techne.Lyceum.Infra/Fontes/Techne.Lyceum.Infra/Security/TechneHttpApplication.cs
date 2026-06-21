namespace Techne
{
    using System;
    using System.IO;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Security;
    using System.Web.SessionState;
    using System.Xml;
    using Techne.Auditing;
    using Techne.Data;

    public abstract class TechneHttpApplication : HttpApplication
    {
        public delegate void UserLogInHandler(object sender, UserAuthenticatedEventArgs e);

        private readonly static string[] BypassExtensions = new[] { ".js", ".css", ".gif", ".config", ".jpg", ".png", ".ico", ".swf", ".htm", ".html" };

        private static string sessionTimeoutUrl = string.Empty;

        private static string stLoginUrl = string.Empty;

        public TechneHttpApplication()
        {
            // Pega referência global ao ConnectionList
            ConnectionList.Current = this.GetConnectionList();
        }

        /// <summary>
        ///   Evento que dispara após um usuário se logar e a sessão ser carregada
        /// </summary>
        public event UserLogInHandler UserLogIn;

        public static string SessionTimeoutUrl
        {
            get
            {
                return sessionTimeoutUrl;
            }

            set
            {
                sessionTimeoutUrl = value == null ? string.Empty : value;
            }
        }

        /// <summary>
        ///   Nome da aplicação (no Hades).
        /// </summary>
        public abstract string ApplicationName { get; }

        public bool ForceConnectionWritableReadWrite { get; set; }

        private static string CorrectedSessionTimeoutUrl
        {
            get
            {
                string url;
                {
                    if (sessionTimeoutUrl.Length > 0)
                    {
                        // Trata o SessionTimeoutUrl
                        Uri u;

                        try
                        {
                            u = new Uri(sessionTimeoutUrl);
                        }
                        catch
                        {
                            var request = HttpContext.Current.Request;
                            var ub = new UriBuilder(request.Url.Scheme, request.Url.Host, request.Url.Port);
                            ub.Path = request.ApplicationPath;
                            if (!ub.Path.EndsWith("/"))
                            {
                                ub.Path += "/";
                            }

                            u = new Uri(ub.Uri, sessionTimeoutUrl);
                        }

                        url = u.ToString();
                    }
                    else
                    {
                        url = string.Empty;
                    }
                }

                return url;
            }
        }

        private static string LoginUrl
        {
            get
            {
                var request = HttpContext.Current.Request;



                if (stLoginUrl == string.Empty)
                {
                    var raiz = request.PhysicalApplicationPath;
                    var webconfigpath = raiz + "web.config";
                    var xreader = new XmlTextReader(webconfigpath);

                    // Lê o arquivo web.config da aplicação e pega o loginUrl do tag <forms>
                    var xdoc = new XmlDocument();
                    xdoc.Load(xreader);
                    xreader.Close();

                    // Alterações para VS2005

                    // XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);

                    // nsmgr.AddNamespace("c", "http://schemas.microsoft.com/.NetConfiguration/v2.0");

                    // XmlNode xnode = xdoc.SelectSingleNode("c:configuration/c:system.web/c:authentication/c:forms", nsmgr);
                    var xnode = xdoc.SelectSingleNode("configuration/system.web/authentication/forms");
                    if (xnode == null || xnode.Attributes["loginUrl"] == null)
                    {
                        stLoginUrl = "Default.aspx";
                    }
                    else
                    {
                        stLoginUrl = xnode.Attributes["loginUrl"].Value;
                    }
                }

                Uri u;
                try
                {
                    u = new Uri(stLoginUrl);
                }
                catch
                {
                    var ub = new UriBuilder(request.Url.Scheme, request.Url.Host, request.Url.Port);
                    ub.Path = request.ApplicationPath;
                    if (!ub.Path.EndsWith("/"))
                    {
                        ub.Path += "/";
                    }

                    u = new Uri(ub.Uri, stLoginUrl);
                }

                return u.ToString();
            }
        }

        public static void RefreshAuthorizationData()
        {
            if (HttpContext.Current != null && HttpContext.Current.ApplicationInstance != null &&
                HttpContext.Current.ApplicationInstance is TechneHttpApplication)
            {
                ((TechneHttpApplication)HttpContext.Current.ApplicationInstance).RefreshAuthorization();
            }
        }

        public virtual bool ChangePassword(string user, string oldpassword, string newpassword, out string message)
        {
            message = "A senha não pôde ser mudada.";
            return false;
        }

        public override sealed void Init()
        {
            base.Init();

            FormsAuthenticationModule formsAuth = null;
            DefaultAuthenticationModule defAuth = null;

            // Pega referências aos módulos
            for (var i = 0; i < this.Modules.Count; i++)
            {
                if (this.Modules[i] is FormsAuthenticationModule)
                {
                    formsAuth = (FormsAuthenticationModule)this.Modules[i];
                }

                if (this.Modules[i] is DefaultAuthenticationModule)
                {
                    defAuth = (DefaultAuthenticationModule)this.Modules[i];
                }
            }

            // Eventos dos módulos
            if (formsAuth != null)
            {
                formsAuth.Authenticate += this.Forms_Authenticate;
            }

            // Eventos da aplicação
            this.AuthorizeRequest += this.Application_AuthorizeRequest;
            this.Error += this.Application__Error;
            this.AcquireRequestState += this.Application_AcquireRequestState;
            this.BeginRequest += this.TechneHttpApplication_BeginRequest;
        }

        public TPermission GetPagePermission(HttpRequest request)
        {
            var requestPath = TUtil.GetRelativeUrl(request.Url.AbsolutePath);

            var permission = this.GetPermission(requestPath, "PAGINA");

            if (permission != null && this.ForceConnectionWritableReadWrite)
            {
                permission.SetReadOnly(false);
            }

            return permission;
        }

        public bool IsPageAudited(HttpRequest request)
        {
            var requestPath = TUtil.GetRelativeUrl(request.Url.AbsolutePath);
            return this.IsAudit(requestPath, "PAGINA");
        }

        internal static void RemoveCookie()
        {
            foreach (var cookie in HttpContext.Current.Request.Cookies.AllKeys)
            {
                HttpContext.Current.Request.Cookies.Remove(cookie);
            }

            foreach (var cookie in HttpContext.Current.Response.Cookies.AllKeys)
            {
                HttpContext.Current.Response.Cookies.Remove(cookie);
            }
        }

        internal static string[] ReadRolesFromCookie(string username)
        {
            if (username == null)
            {
                return new string[] { };
            }

            var cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cookie == null)
            {
                return new string[] { };
            }

            // Lê o cookie
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            if (ticket == null)
            {
                return new string[] { };
            }

            if (ticket.Expired
                || !ticket.Name.Equals(username))
            {
                return new string[] { };
            }

            var roles = ticket.UserData.Split(';');

            return roles;
        }

        internal static void CreateCookie(IPrincipal user, string[] roles)
        {
            if (user == null)
            {
                return;
            }

            var conn = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            var section = (AuthenticationSection)conn.SectionGroups.Get("system.web").Sections.Get("authentication");
            var cookieExpires = Convert.ToInt64(section.Forms.Timeout.TotalMinutes);
            var ticket = new FormsAuthenticationTicket(
                1,
                user.Identity.Name,
                DateTime.Now,
                DateTime.Now.AddMinutes(cookieExpires),
                false,
                roles != null ? string.Join(";", roles) : string.Empty,
                FormsAuthentication.FormsCookiePath);

            HttpContext.Current.Response.Cookies.Add(
                new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
        }

        internal static void UpdateCookie(string user)
        {
            if (user == null)
            {
                return;
            }

            // Salva no cookie
            var conn = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            var section = (AuthenticationSection)conn.SectionGroups.Get("system.web").Sections.Get("authentication");
            var cookieExpires = Convert.ToInt64(section.Forms.Timeout.TotalMinutes);
            var cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cookie == null)
            {
                return;
            }

            // Lê o cookie
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            if (ticket == null)
            {
                return;
            }

            if (ticket.Expired
                || !ticket.Name.Equals(user))
            {
                return;
            }

            ticket = new FormsAuthenticationTicket(
                ticket.Version,
                ticket.Name,
                DateTime.Now,
                DateTime.Now.AddMinutes(cookieExpires),
                false,
                ticket.UserData,
                FormsAuthentication.FormsCookiePath);

            HttpContext.Current.Response.Cookies.Add(
                new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
        }

        internal void OnSessionSignIn()
        {
            HttpSessionState session = null;

            // Tenta pegar a sessão corrente
            // Se ela não estiver no contexto, assume nula
            try
            {
                session = HttpContext.Current.Session;
            }
            catch
            {
            }

            if (HttpContext.Current.Session != null)
            {
                // Pendura usuário na sessão
                session["__TPrincipal"] = this.User;

                // Registra ocorrência no log
                if (Settings.AuditingSettings.AuditWebSession)
                {
                    TLog.LogWebSession();
                }

                // Dispara evento
                var e = new UserAuthenticatedEventArgs();
                e.setPrincipal(HttpContext.Current.User);
                this.OnUserLogIn(e);
            }
        }

        protected internal abstract ConnectionList GetConnectionList();

        /// <summary>
        ///   Retorna as permissões que o usuário corrente tem de acessar um objeto/recurso
        /// </summary>
        /// <param name = "resource">Nome do objeto/recurso, ou sua URL</param>
        /// <param name = "resourcetype">Tipo do objeto ("PAGINA","RELATORIO", etc...)</param>
        /// <returns>Objeto HadesPermission com os acessos permitidos. Se retornar null, o objeto não existe.</returns>
        protected internal abstract TPermission GetPermission(string resource, string resourcetype);

        /// <summary>
        ///   Retorna dados do usuário. Null se o usuário não existir ou se estiver desabilitado
        /// </summary>
        /// <param name = "user">Código do usuário</param>
        /// <returns></returns>
        protected internal abstract IPrincipal GetUserInfo(string user, out string[] roles);

        /// <summary>
        ///   Indica se um recurso é auditável
        /// </summary>
        /// <param name = "resource">Nome do objeto/recurso, ou sua URL</param>
        /// <param name = "resourcetype">Tipo do objeto ("PAGINA","RELATORIO", etc...)</param>
        /// <returns></returns>
        protected internal abstract bool IsAudit(string resource, string resourcetype);

        /// <summary>
        ///   Valida senha do usuário
        /// </summary>
        /// <param name = "user">código do usuário</param>
        /// <param name = "password">senha</param>
        /// <returns>Resultado da validação</returns>
        protected internal abstract TechneAuthenticationResult ValidateUser(string user, string password);

        /// <summary>
        ///   Valida um usuário do windows contra o banco de dados. Se
        ///   o usuário for válido e estiver habilitado, retorna o código do usuário
        ///   Caso contrário, retorna null.
        /// </summary>
        /// <param name = "windowsuser">Usuário do Windows</param>
        /// <param name = "user">Usuário correspondente no banco de dados</param>
        /// <returns>Resultado da validação</returns>
        protected internal abstract TechneAuthenticationResult ValidateWindowsUser(string windowsuser, out string user);

        /// <summary>
        ///   Cria um novo objeto de Log
        /// </summary>
        /// <returns>TLog criado</returns>
        protected internal virtual TLog GetTLog()
        {
            return new TLog();
        }

        protected static IPrincipal CreateTPrincipal(string name, string[] roles, bool privil)
        {
            var iden = new GenericIdentity(name);
            return new GenericPrincipal(iden, roles);
        }

        protected virtual void OnApplicationStart(EventArgs args)
        {
            // Criado para permitir o override nas classes derivadas,
            // já que o Application_Start() não está mais disponível.
        }

        protected virtual void OnSessionStart()
        {
            // Criado para permitir o override nas classes derivadas,
            // já que o Session_Start() não está mais disponível.
        }

        protected virtual void OnUserLogIn(UserAuthenticatedEventArgs args)
        {
            if (this.UserLogIn != null)
            {
                this.UserLogIn(this, args);
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            this.OnApplicationStart(e);
        }

        protected TPermission CreatePermission(string resource, string resourceType, bool execute, bool insert, bool update, bool delete)
        {
            return new TPermission(resource, resourceType, execute, insert, update, delete);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            this.OnSessionStart();
        }

        private void Application_AcquireRequestState(object sender, EventArgs e)
        {
            // Audita sessão
            if (Techne.Settings.AuditingSettings.AuditWebSession)
            {
                TLog.LogWebSession();
            }

            HttpSessionState session;

            // Verifica se a sessão está disponível neste contexto
            try
            {
                session = this.Session;
            }
            catch
            {
                session = null;
            }

            if (session == null)
            {
                return;
            }

            // Sai da sessão de ela era de outro usuário
            if (this.Session["__TPrincipal"] is IPrincipal)
            {
                if (!this.User.Identity.IsAuthenticated ||
                    this.User.Identity.Name != ((IPrincipal)this.Session["__TPrincipal"]).Identity.Name)
                {
                    this.Session.Abandon();

                    // Se houver alguma página de expiração, vai pra ela
                    if ((string.Empty + CorrectedSessionTimeoutUrl).Trim() != string.Empty)
                    {
                        this.Response.Redirect(CorrectedSessionTimeoutUrl);
                    }
                }
            }
            else if (this.User.Identity.IsAuthenticated)
            {
                // Põe o TPrincipal na sessão também
                this.OnSessionSignIn();
            }
        }

        private void Application_AuthorizeRequest(object sender, EventArgs e)
        {
            var current = HttpContext.Current;
            var request = current.Request;

            // Não verifica permissão para determinadas extensões (BypassExtensions)
            if (StrLib.IndexOfInsensitive(Path.GetExtension(request.PhysicalPath), BypassExtensions) >= 0)
            {
                return;
            }

            // Checa permissões
            var perm = this.GetPagePermission(request);

            // se a permissão é nula, a página não existe
            if (perm == null)
            {
                throw new HttpException(404, "Página inexistente.");
            }

            if (!perm.Execute)
            {
                // Se o usuário não for anônimo e o acesso foi negado, mostra página de acesso negado
                if (this.User.Identity.IsAuthenticated)
                {
                    throw new HttpException(403, "Acesso negado.");
                }

                // Força autenticação se usuário não autenticado e acesso anônimo negado
                var loginUrl = LoginUrl;
                var loginPage = loginUrl.ToLower();
                var requestpage = request.Url.AbsoluteUri.ToLower();

                //Pagina para criar imagem de captcha que não deve ser autenticada
                var captchaPage = "gerachaveseguranca.aspx";

                if (request.Url.Query.Length > 0)
                {
                    requestpage = requestpage.Substring(0, requestpage.IndexOf(request.Url.Query.ToLower()));
                }

                //Verifica se não são as paginas de Login e Imagem de Captcha, que não devem ser autenticadas
                if ((loginPage != requestpage) && (!requestpage.Contains(captchaPage)))
                {
                    this.Response.StatusCode = 401;
                    this.CompleteRequest();
                }
            }

            // Guarda informações para auditoria de banco
            var audit = this.IsPageAudited(request);
            if (this.User.Identity.IsAuthenticated && this.User.Identity.Name.Length > 0)
            {
                current.Items["__AuditingInfo"] = new AuditingInfo(this.User.Identity.Name, perm.Resource, audit);
            }
        }

        private void Application__Error(object sender, EventArgs e)
        {
            var message = new StringBuilder();

            var erro = HttpContext.Current.Server.GetLastError();
            if (erro is HttpUnhandledException)
            {
                erro = erro.InnerException;
            }

            // ignora erro do favicon.ico
            if (erro is HttpException &&
                HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null &&
                "/favicon.ico".Equals(HttpContext.Current.Request.Url.AbsolutePath, StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            message.Append(erro.Message);
            message.Append("\r\n\r\nFonte:\r\n");
            message.Append(erro.Source);
            message.Append("\r\n\r\nStackTrace:\r\n");
            message.Append(erro.StackTrace);
            message.Append("\r\n\r\n");

            TLog.WriteEntry(
                HttpContext.Current.Request.Url.AbsolutePath,
                TLogCategory.Runtime,
                Settings.AppName,
                message.ToString(),
                TLogEntryType.Error);
        }

        private void Forms_Authenticate(object sender, FormsAuthenticationEventArgs e)
        {
            // Pega usuário corrente do cookie
            // Se for um usuário válido, pendura no contexto e sai
            this.ObtainTPrincipalFromCookie(true);
        }

        /// <summary>
        ///   Tenta criar um TPrincipal a partir do cookie de autenticação.
        ///   Salva no contexto se for assim solicitado.
        /// </summary>
        /// <param name = "save">Salva o TPrincipal no contexto se ele tiver sido criado</param>
        /// <returns>Conseguiu criar o TPrincipal; False se não há cookie ou ele se expirou.</returns>
        private bool ObtainTPrincipalFromCookie(bool save)
        {
            // Checa se o cookie existe
            var cookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cookie == null)
            {
                return false;
            }

            // Lê o cookie
            string user;
            string[] roles = null;

            try
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);

                if (ticket == null)
                {
                    return false;
                }

                if (ticket.Expired)
                {
                    return false;
                }

                user = ticket.Name;
                roles = ticket.UserData.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch
            {
                user = null;
            }

            if (user == null)
            {
                return true;
            }

            if (save)
            {
                // Atualiza duração do cookie
                UpdateCookie(user);

                // Pendura usuário no contexto
                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(user), roles);
            }

            return true;
        }

        private void RefreshAuthorization()
        {
        }

        private void TechneHttpApplication_BeginRequest(object sender, EventArgs e)
        {
            // Os applets em linux não conseguem passar cookies nas chamadas http
            // Para contornar este problema, estou permitindo que eles passem os cookies
            // via parâmetro POST "cookies", codificados em formato Base64.
            // Este handler decodifica os cookies e põe eles na coleção Cookies da Request
            // Desta forma, o usuário pode ser autenticado normalmente, via cookie.
            // Só funciona para requisições a Httphandlers terminados em ".techne.ashx"
            var context = HttpContext.Current;

            if (context == null
                || context.Request == null)
            {
                return;
            }

            var request = context.Request;
            if (request.Cookies.Count != 0 ||
                !request.Url.AbsolutePath.ToLower().EndsWith(".techne.ashx") ||
                request.Form["cookies"] + string.Empty == string.Empty)
            {
                return;
            }

            HttpCookieCollection cookies = null;
            try
            {
                cookies = TUtil.Base64ToCookies(request.Form["cookies"]);
                for (var i = 0; i < cookies.Count; i++)
                {
                    context.Request.Cookies.Add(cookies[i]);
                }
            }
            catch
            {
            }
        }
    }

    public class UserAuthenticatedEventArgs : EventArgs
    {
        private IPrincipal userprincipal;

        public UserAuthenticatedEventArgs()
        {
            this.userprincipal = new GenericPrincipal(new GenericIdentity(string.Empty), new string[] { });
        }

        public IPrincipal Principal
        {
            get
            {
                return this.userprincipal;
            }
        }

        internal void setPrincipal(IPrincipal value)
        {
            this.userprincipal = value;
        }
    }
}