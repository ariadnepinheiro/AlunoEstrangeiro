using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class MetaUnidadeAdministrativa
    {
        [PrimaryKey]
        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }

        [PrimaryKey]
        public NivelEnsino NivelEnsino { get; set; }

        [PrimaryKey]
        public AnoReferencia AnoReferencia { get; set; }

        [PrimaryKey]
        public Indicador Indicador { get; set; }

        [PrimaryKey]
        public Modalidade Modalidade { get; set; }

        public decimal ValorMeta { get; set; }
    }
}