using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.DTO
{
    public class UnidadeConsolidado
    {
        public int IdUnidade { get; set; }
        public string DesUnidade { get; set; }
        public bool Regional { get; set; }
        public bool Elegivel { get; set; }
    }
}