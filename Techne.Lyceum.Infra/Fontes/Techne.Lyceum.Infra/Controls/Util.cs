using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Techne
{
    public class TUtil
    {
        private TUtil()
        {
        }

        /// <summary>
        ///   Converte uma string Base64 para coleção de cookies
        /// </summary>
        /// <param name = "base64">string Base64</param>
        /// <returns>Coleção de cookies</returns>
        public static HttpCookieCollection Base64ToCookies(string base64)
        {
            var cookies = new HttpCookieCollection();

            var list = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            var itens = list.Split(';');
            for (var i = 1; i < itens.Length; i++)
            {
                var elems = itens[i].Split('=');
                var c = new HttpCookie(elems[0].Trim(), elems[1].Trim());
                cookies.Add(c);
            }

            return cookies;
        }

        /// <summary>
        ///   Converte uma coleção de cookies para uma string Base64
        /// </summary>
        /// <param name = "cookies">lista de cookies</param>
        /// <returns>string Base64</returns>
        public static string CookiesToBase64(HttpCookieCollection cookies)
        {
            if (cookies == null)
            {
                return string.Empty;
            }

            var c = string.Empty;
            var keys = cookies.AllKeys;
            if (keys.Length > 0)
            {
                c = keys[0] + "=" + cookies[keys[0]].Value;
            }

            for (var i = 1; i < keys.Length; i++)
            {
                c += "; " + keys[i] + "=" + cookies[keys[0]].Value;
            }

            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(c));
        }

        public static bool IsDesignMode(Control control)
        {
            ISite site;

            // Sai de cara se não houver controle
            if (control == null)
            {
                return false;
            }

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                return false;
            }

            // Pega site
            if (control.Site != null)
            {
                site = control.Site;
            }
            else if (control.Page != null && control.Page.Site != null)
            {
                site = control.Page.Site;
            }
            else
            {
                site = null;
            }

            if (site != null && site.DesignMode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string StyleToString(Style style)
        {
            int i;
            var sb = new StringBuilder();
            if (!style.BackColor.IsEmpty)
            {
                sb.Append("background-color:" + ColorTranslator.ToHtml(style.BackColor) + ";");
            }

            if (!style.ForeColor.IsEmpty)
            {
                sb.Append("color:" + ColorTranslator.ToHtml(style.ForeColor) + ";");
            }

            if (!style.BorderColor.IsEmpty)
            {
                sb.Append("border-color:" + ColorTranslator.ToHtml(style.BorderColor) + ";");
            }

            if (style.BorderStyle != BorderStyle.NotSet)
            {
                sb.Append("border-style:" + style.BorderStyle.ToString().ToLower() + ";");
            }

            if (!style.BorderWidth.IsEmpty)
            {
                sb.Append("border-width:" + style.BorderWidth.ToString(CultureInfo.InvariantCulture) + ";");
            }

            if (style.Font.Bold)
            {
                sb.Append("font-weigth: bold;");
            }

            if (style.Font.Italic)
            {
                sb.Append("font-style: italic;");
            }

            if (!style.Font.Size.IsEmpty)
            {
                sb.Append("font-size:" + style.Font.Size.ToString(CultureInfo.InvariantCulture) + ";");
            }

            if (style.Font.Name != null || (style.Font.Names != null && style.Font.Names.Length > 0))
            {
                sb.Append("font-family:");
                sb.Append(style.Font.Name);
                for (i = 0; i < style.Font.Names.Length; i++)
                {
                    sb.Append(style.Font.Names[i].IndexOf(" ") > 0 ? ",'" + style.Font.Names[i] + "'" : "," + style.Font.Names[i]);
                }

                sb.Append(";");
            }

            if (style.Font.Overline || style.Font.Underline || style.Font.Strikeout)
            {
                sb.Append("text-decoration:" + (style.Font.Overline ? " overline" : string.Empty) + (style.Font.Underline ? " underline" : string.Empty) + (style.Font.Strikeout ? " line-through" : string.Empty) + ";");
            }

            if (!style.Height.IsEmpty)
            {
                sb.Append("height:" + style.Height.ToString(CultureInfo.InvariantCulture) + ";");
            }

            if (!style.Width.IsEmpty)
            {
                sb.Append("width:" + style.Width.ToString(CultureInfo.InvariantCulture) + ";");
            }

            return sb.ToString();
        }

        /// <summary>
        ///   Traduz URL começadas por "~/", trocando o til pela raiz da aplicação.
        ///   Funciona em designtime e runtime
        /// </summary>
        /// <param name = "url">URL a ser traduzida</param>
        /// <returns></returns>
        public static string TranslateRelativeUrl(string url)
        {
            return TranslateRelativeUrl(url, null);
        }

        /// <summary>
        ///   Traduz URL começadas por "~/", trocando o til pela raiz da aplicação.
        ///   Funciona em designtime e runtime
        /// </summary>
        /// <param name = "url">URL a ser traduzida</param>
        /// <param name = "control">Controle/página que usa a url</param>
        /// <returns></returns>
        public static string TranslateRelativeUrl(string url, Control control)
        {
            ISite site;
            var urltranslated = url;

            // Sai de cara se não tiver til, ou se url forem nulas
            if (url == null)
            {
                return string.Empty;
            }
            else if (url.Length < 2 || (url.Substring(0, 2) != "~/" && url.Substring(0, 2) != "~\\"))
            {
                return url;
            }

            // Pega site
            if (control == null)
            {
                site = null;
            }
            else if (control.Site != null)
            {
                site = control.Site;
            }
            else if (control.Page != null && control.Page.Site != null)
            {
                site = control.Page.Site;
            }
            else
            {
                site = null;
            }

            // Se for modo de Design
            if (site != null && site.DesignMode)
            {
                var app = (IWebApplication)site.GetService(typeof (IWebApplication));
                if (app != null && app.RootProjectItem != null)
                {
                    urltranslated = app.RootProjectItem.PhysicalPath + url.Substring(2);
                }

                // IUrlResolutionService service = (IUrlResolutionService)site.GetService(typeof(IUrlResolutionService));
                // if (service != null)
                // {
                // urltranslated=service.ResolveClientUrl(url);
                // }
                // else
                // {
                // object o=site.GetService(typeof(System.Web.UI.Design.IWebFormsDocumentService);
                // if(o!=null) {
                // pagepath=((System.Web.UI.Design.IWebFormsDocumentService)o).DocumentUrl;
                // posraiz=-1;
                // // Isto não funciona se a aplicação estiver na raiz do site!!!
                // for(c=0;c<4;c++) {
                // posraiz=pagepath.IndexOf("/",posraiz+1);
                // if(posraiz<0)
                // break;
                // }
                // if(posraiz>0)
                // apppath=pagepath.Substring(0,posraiz);
                // }
                // urltranslated=apppath+url.Substring(1);
                // }
                return urltranslated;
            }
            else
            {
                try
                {
                    return (HttpContext.Current.Request.ApplicationPath == "/" ? string.Empty : HttpContext.Current.Request.ApplicationPath) +
                           url.Substring(1);
                }
                catch
                {
                    return url;
                }
            }
        }

        internal static object CreateInstance(string className)
        {
            if (className == null || className.Trim().Length == 0)
            {
                return null;
            }

            var asms = AppDomain.CurrentDomain.GetAssemblies();
            object o = null;
            foreach (var asm in asms)
            {
                try
                {
                    o = asm.CreateInstance(className, true);
                }
                catch
                {
                    o = null;
                }

                if (o != null)
                {
                    break;
                }
            }

            return o;
        }

        internal static object CreateInstance(string className, ISite site)
        {
            object o = null;
            if (className == null || className.Trim().Length == 0)
            {
                return null;
            }

            ITypeResolutionService resolution = null;
            Type classType = null;
            if (site != null)
            {
                resolution = (ITypeResolutionService)site.GetService(typeof (ITypeResolutionService));
            }

            if (resolution != null)
            {
                classType = resolution.GetType(className);
            }

            if (classType != null)
            {
                try
                {
                    o = classType.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
                catch
                {
                }

                if (o == null)
                {
                    var asm = classType.Assembly;
                    try
                    {
                        o = asm.CreateInstance(className, true);
                    }
                    catch
                    {
                    }
                }
            }

            if (o == null)
            {
                o = CreateInstance(className);
            }

            return o;
        }

        internal static string GetRelativeUrl(string url)
        {
            var request = HttpContext.Current.Request;
            var applicationPath = request.ApplicationPath;
            var path = new UriBuilder(new Uri(request.Url, url)).Path;

            if (path.ToLower().StartsWith(applicationPath.ToLower()))
            {
                if (applicationPath == "/")
                {
                    return "~" + path;
                }
                else
                {
                    return "~" + path.Substring(applicationPath.Length);
                }
            }
            else
            {
                return path;
            }
        }
    }
}