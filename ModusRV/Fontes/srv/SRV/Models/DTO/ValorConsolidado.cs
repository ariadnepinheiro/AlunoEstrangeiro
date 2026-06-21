using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.DTO
{
    public class ValorConsolidado
    {
        public int IdUnidade { get; set; }

        public decimal? Previsto { get; set; }
        public decimal? Realizado { get; set; }
        public decimal? Percentual { get; set; }
    }
}