using System.Web;

namespace Techne.Web
{
    public class TSearchHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.End();
            return;
        }
    }
}