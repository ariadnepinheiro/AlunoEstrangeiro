using System;
using System.Web;



namespace Techne.Lyceum.Net
{
    public class Global : HadesHttpApplicationBase
    {
        public Global()
        {
            this.ForceConnectionWritableReadWrite = true;
        }

        public override sealed string ApplicationName { get { return "LyceumNet"; } }


        protected override void OnUserLogIn(UserAuthenticatedEventArgs args)
        {
            base.OnUserLogIn(args);
            LyceumUser.SetCurrent(LyceumUser.Get(User.Identity.Name));
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            //if (ex is ThreadAbortException)
            return;
            //Logger.Error(LoggerType.Global, ex, "Exception");
            //Response.Redirect("unexpectederror.htm");
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpResponse response = app.Response;
            HttpRequest request = app.Request;

            if (!string.IsNullOrEmpty(request.Url.PathAndQuery) )
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
    }
}

