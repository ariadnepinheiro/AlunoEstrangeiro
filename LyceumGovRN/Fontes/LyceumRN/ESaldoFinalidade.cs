using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN
{
    public class ESaldoFinalidade
    {
        public decimal? ID                  { get; set; }
        public decimal? ID_CONTABANCO_SALDO { get; set; }
        public decimal? ID_FINALIDADE       { get; set; }
        public decimal? SALDO               { get; set; }

        public String   CODIGO_FINALIDADE   { get; set; }
    }
}
