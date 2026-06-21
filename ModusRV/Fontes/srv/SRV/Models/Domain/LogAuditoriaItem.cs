using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.Domain
{
    public class LogAuditoriaItem
    {
        public int IdLogAuditoria { get; set; }

        public string DesAtributo { get; set; }
        public string VlrAnterior { get; set; }
        public string VlrAtual { get; set; }
    }
}