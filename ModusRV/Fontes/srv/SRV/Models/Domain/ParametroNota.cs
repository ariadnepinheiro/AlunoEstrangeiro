using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class ParametroNota
    {
        [PrimaryKey]
        public AnoReferencia AnoReferencia { get; set; }

        [PrimaryKey]
        public Nota Nota { get; set; }

        public decimal Percentual { get; set; }

        [PrimaryKey]
        public Indicador Indicador { get; set; }

        [PrimaryKey]
        public Modalidade Modalidade { get; set; }

        public bool ValorMeta { get; set; }
    }
}