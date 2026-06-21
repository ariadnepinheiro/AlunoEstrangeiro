using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.DTO
{
    public class CoeficienteServidor
    {
        public int AnoReferencia { get; set; }
        public string IdServidor { get; set; }
        public string Coeficiente { get; set; }
		public string IdFuncional { get; set; }
		public int? Vinculo { get; set; }
    }
}