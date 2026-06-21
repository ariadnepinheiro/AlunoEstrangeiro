using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SRV.Filters
{
    public class HandleAndLogErrorAttribute : HandleErrorAttribute
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(HandleAndLogErrorAttribute));

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            log.Error(filterContext.Exception.Message, filterContext.Exception);
        }
    }
}