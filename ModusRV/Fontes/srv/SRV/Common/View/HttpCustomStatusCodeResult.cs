using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRV.Common.View
{
    /// <summary>
    /// Customização da classe HttpStatusCodeResult que adiciona o campo StatusDescription
    /// no corpo do response para permitir captura e exibição no cliente, geralemente utilizado 
    /// em requesições ajax.
    /// </summary>
    public class HttpCustomStatusCodeResult : HttpStatusCodeResult
    {

        public HttpCustomStatusCodeResult(int statusCode)
            : this(statusCode, null)
        {
        }

        public HttpCustomStatusCodeResult(int statusCode, string statusDescription) 
            : base(statusCode, statusDescription)
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            //Configuração para o IIS 7 não trocar a mensagem de erro customizada
            context.HttpContext.Response.TrySkipIisCustomErrors = true;

            if (StatusDescription != null)
            {
                context.HttpContext.Response.Write(StatusDescription);
            }

            base.ExecuteResult(context);
        }
    
    }
}