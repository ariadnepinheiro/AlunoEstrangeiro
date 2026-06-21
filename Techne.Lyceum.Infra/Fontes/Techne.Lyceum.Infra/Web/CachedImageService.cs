using System.Web;

namespace Techne.Web
{
    internal class CachedImageService : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["data"] != null)
            {
                var storageKey = context.Request["data"];
                var img = HttpContext.Current.Cache[storageKey] as DbObject;
                HttpContext.Current.Cache.Remove(storageKey);

                if (img != null && img.Type == DbType.Raw)
                {
                    var byteArray = (byte[])(Raw)img;
                    HttpContext.Current.Response.ContentType = "image/jpeg";
                    HttpContext.Current.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
                    return;
                }
            }

            HttpContext.Current.Response.Write("No image specified.");
        }
    }
}