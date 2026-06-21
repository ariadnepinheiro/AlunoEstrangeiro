using System;
using System.Collections;
using System.Web;
using System.Web.Configuration;

namespace Techne
{
    public class Settings
    {
        private static string appName;

        private static AuditingSettings auditingSettings;

        private Settings()
        {
        }

        public static string AppName
        {
            get
            {
                if (appName == null)
                {
                    if (HttpContext.Current != null)
                    {
                        var app = HttpContext.Current.ApplicationInstance as TechneHttpApplication;

                        if (app != null && app.ApplicationName != null && app.ApplicationName.Trim().Length > 0)
                        {
                            appName = app.ApplicationName;
                        }
                        else
                        {
                            var authorization = (Hashtable)HttpContext.Current.GetSection("techne/web/authorization");
                            appName = authorization["appname"] as string;
                        }
                    }

                    if (appName == null)
                    {
                        appName = string.Empty;
                    }
                }

                return appName;
            }
        }

        public static AuditingSettings AuditingSettings
        {
            get
            {
                if (auditingSettings == null)
                {
                    auditingSettings = new AuditingSettings();
                    auditingSettings.AuditWebSession = EvalBoolean(WebConfigurationManager.AppSettings["AuditWebSession"]);
                    auditingSettings.AuditLogin = EvalBoolean(WebConfigurationManager.AppSettings["AuditLogin"]);
                    auditingSettings.AuditLoginFailures = EvalBoolean(WebConfigurationManager.AppSettings["AuditLoginFailures"]);
                }

                return auditingSettings;
            }
        }

        private static bool EvalBoolean(string text)
        {
            if (text == null)
            {
                return false;
            }
            else if ("true".Equals(text, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else if ("S".Equals(text, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else if ("Y".Equals(text, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else if ("yes".Equals(text, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}