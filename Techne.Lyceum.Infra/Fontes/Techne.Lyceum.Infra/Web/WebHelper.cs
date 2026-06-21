using System;
using System.Web;

namespace Techne.Web
{
    internal class WebHelper
    {
        public static string ApplicationName
        {
            get
            {
                if (HttpContext.Current.ApplicationInstance is Techne.TechneHttpApplication)
                {
                    return ((Techne.TechneHttpApplication)HttpContext.Current.ApplicationInstance).ApplicationName;
                }
                else
                {
                    return System.Web.Configuration.WebConfigurationManager.AppSettings["ApplicationName"];
                }
            }
        }

        public static bool EqualUrl(string url1, string url2)
        {
            if (url1.Length > 0 && url1[0] == '~')
            {
                url1 = url1.Substring(1);
            }

            if (url2.Length > 0 && url2[0] == '~')
            {
                url2 = url2.Substring(1);
            }

            if (url1.Equals(url2, StringComparison.InvariantCultureIgnoreCase))
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