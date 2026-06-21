using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using Seeduc.Infra.Data;
using log4net;

namespace Seeduc.Infra.Aspects
{
    internal sealed class ExceptionsDataContext
    {
        private log4net.ILog logger;

        public void GravaError(Exception ex, string comando)
        {
            string pagina = string.Empty;
            if (System.Web.HttpContext.Current != null)
            {
                pagina = System.Web.HttpContext.Current.Request.Url.AbsolutePath ?? string.Empty;
            }

            logger = log4net.LogManager.GetLogger("LogInFile");
            StringBuilder buffer = new StringBuilder();

            buffer.AppendFormat("\n Page: {2} \n Error executing ContextQuery! {1} , \n {0}", ex, comando, pagina);

            if (ex.InnerException != null)
            {
                //var contextQuery = ex. args.Arguments.Single(x => x is ContextQuery) as ContextQuery;
                ex = ex.InnerException;

                buffer.AppendFormat("\n {0} \n {1}", ex,comando);
            }

            logger.Error(buffer.ToString());
        }

    }
}
