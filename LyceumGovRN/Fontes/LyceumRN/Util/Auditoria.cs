using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Util
{
    public class Auditoria
    {
        public static void AuditaWebServicePor(string pagina)
        {
            var current = System.Web.HttpContext.Current;
            current.Items["__AuditingInfo"] = new Techne.Auditing.AuditingInfo("WebService", pagina, true);
        }
    }
}
