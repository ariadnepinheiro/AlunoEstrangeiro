using System.Web;

namespace Proderj.DOL.WebApp
{
    public static class StaticMethods
    {
        public static bool AlteracaoDeSenhaNecessaria
        {
            get 
            {
                try
                {
                    bool result;
                    bool.TryParse((HttpContext.Current.Session["AlteracaoDeSenhaNecessaria"] ?? "false").ToString(), out result);
                    return result;
                }
                catch
                {
                    return false;
                }
            }

            set
            {
                HttpContext.Current.Session["AlteracaoDeSenhaNecessaria"] = value;
            }
        }
    }
}