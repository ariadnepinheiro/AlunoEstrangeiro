namespace Techne
{
    using System;
    using System.Resources;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Security;

    public sealed class TechneAuthentication
    {
        private static ResourceManager resource;

        private TechneAuthentication()
        {
        }

        private static ResourceManager Resource
        {
            get
            {
                if (resource == null)
                {
                    resource = new ResourceManager(typeof(TechneAuthentication));
                }

                return resource;
            }
        }

        /// <summary>
        ///   Autentica o usuário no site, criando o cookie de autenticação.
        /// </summary>
        /// <param name = "user">Código do usuário</param>
        /// <param name = "password">Senha do usuário</param>
        /// <returns>Código de erro. Se a autenticação foi bem sucedida retorna TechneAuthentication.ErrorCode.OK.</returns>
        public static TechneAuthenticationResult Authenticate(string user, string password)
        {
            string message;
            return Authenticate(user, password, out message);
        }

        /// <summary>
        ///   Autentica o usuário no site, criando o cookie de autenticação.
        /// </summary>
        /// <param name = "user">Código do usuário</param>
        /// <param name = "password">Senha do usuário</param>
        /// <param name = "message">Mensagem de erro</param>
        /// <returns>Código de erro. Se a autenticação foi bem sucedida retorna TechneAuthentication.ErrorCode.OK.</returns>
        public static TechneAuthenticationResult Authenticate(string user, string password, out string message)
        {
            IPrincipal huser = null;
            TechneAuthenticationResult errorcode;
            TechneHttpApplication app = null;

            // Pega a aplicação Techne
            if (HttpContext.Current != null)
            {
                app = HttpContext.Current.ApplicationInstance as TechneHttpApplication;
            }

            if (app == null)
            {
                throw new ApplicationException("Erro de configuração. A aplicação não herda de TechneHttpApplication.");
            }

            // Valida senha
            message = null;
            errorcode = app.ValidateUser(user, password);
            message = TechneAuthentication.GetMessage(errorcode);

            // Se ocorrer erro, registra no log e sai
            if (errorcode != TechneAuthenticationResult.OK)
            {
                if (Settings.AuditingSettings.AuditLoginFailures)
                {
                    TLog.WriteEntry("Login", TLogCategory.Security, "Hades", message, TLogEntryType.Warning, new[] { "USUARIO" }, new[] { user });
                }

                return errorcode;
            }

            // Pega TPrincipal do usuário
            string[] roles;

            huser = app.GetUserInfo(user, out roles);

            if (roles == null
                || roles.Length == 0)
            {
                errorcode = TechneAuthenticationResult.UserHasNoRoles;
                message = "Nenhum padrão de acesso cadastrado para esse usuário.";
                return errorcode;
            }

            // Põe dados no Current.User
            HttpContext.Current.User = huser;

            // Salva cookie
            TechneHttpApplication.CreateCookie(huser, roles);

            // Registra o login
            if (Settings.AuditingSettings.AuditLogin)
            {
                TLog.WriteEntry("Login", TLogCategory.Security, Settings.AppName, GetMessage(TechneAuthenticationResult.OK), TLogEntryType.Information, null, null);
            }

            // Dispara evento de autenticação da sessão
            app.OnSessionSignIn();

            // Sai com resultado OK
            return TechneAuthenticationResult.OK;
        }

        public static bool ChangePassword(string user, string oldpassword, string newpassword, out string message)
        {
            var context = HttpContext.Current;
            if (context != null
                && context.ApplicationInstance is TechneHttpApplication)
            {
                var app = (TechneHttpApplication)context.ApplicationInstance;
                string msg;
                bool ret;
                ret = app.ChangePassword(user, oldpassword, newpassword, out msg);
                message = msg;
                return ret;
            }
            else
            {
                message = "Senha não pôde ser mudada";
                return false;
            }
        }

        public static string GetMessage(string messagecode)
        {
            var message = Resource.GetString(messagecode);
            if (message != null)
            {
                return message;
            }

            return string.Empty;
        }

        public static void SignOut()
        {
            var context = HttpContext.Current;

            if (context == null)
            {
                return;
            }

            if (context.Session != null)
            {
                context.Session.Clear();
                context.Session.Abandon();
            }

            FormsAuthentication.SignOut();
            TechneHttpApplication.RemoveCookie();
            FormsAuthentication.RedirectToLoginPage();

            context.Response.End();
        }

        public static TechneAuthenticationResult WebAuthenticate(string usuario, string senha, string initialPage, out string msg)
        {
            string message;
            TechneAuthenticationResult result;
            try
            {
                result = Authenticate(usuario, senha, out message);
            }
            catch (AuthenticationException exc)
            {
                result = TechneAuthenticationResult.Undefined;
                message = exc.Message;
            }

            msg = string.Empty;

            if (result == TechneAuthenticationResult.OK)
            {
                var principal = HttpContext.Current.User;
                var identity = principal.Identity;
                var redirurl = GetRedirectUrl();
                if (redirurl != null)
                {
                    RedirectFromLoginPage();
                }
                else
                {
                    HttpContext.Current.Response.Redirect(TUtil.TranslateRelativeUrl(initialPage));
                }
            }
            else
            {
                msg = message;
            }

            return result;
        }

        internal static string GetMessage(TechneAuthenticationResult result)
        {
            var message = Resource.GetString("TechneAuthenticationResult." + result);
            if (message != null)
            {
                return message;
            }

            return string.Empty;
        }

        private static string GetRedirectUrl()
        {
            string s = null;
            if (HttpContext.Current != null
                && HttpContext.Current.Request != null)
            {
                s = HttpContext.Current.Request.QueryString["ReturnUrl"];
            }

            if ((string.Empty + s).Trim()
                == string.Empty)
            {
                s = null;
            }

            return s;
        }

        private static void RedirectFromLoginPage()
        {
            if (HttpContext.Current == null ||
                HttpContext.Current.Request == null
                ||
                HttpContext.Current.Response == null)
            {
                return;
            }

            var s = GetRedirectUrl();
            if (s == null)
            {
                s = HttpContext.Current.Request.ApplicationPath + "/Default.aspx";
            }

            HttpContext.Current.Response.Redirect(s);
        }
    }

    public class AuthenticationException : Exception
    {
        public AuthenticationException()
            : base("Erro na autenticação do usuário")
        {
        }

        public AuthenticationException(string message)
            : base(message)
        {
        }
    }

    public class PasswordExpiredException : AuthenticationException
    {
        public PasswordExpiredException()
            : base("Senha expirada")
        {
        }
    }

    public class ChangePasswordException : AuthenticationException
    {
        public ChangePasswordException()
            : base("Altere a senha")
        {
        }
    }

    public enum TechneAuthenticationResult
    {
        OK, 
        UserDisabled, 
        ChangePassword, 
        InvalidUserOrPassword, 
        UserHasNoRoles, 
        Undefined
    }
}