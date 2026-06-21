using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.Domain
{
    public class NivelEnsinoUnidadeAdministrativa
    {
        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }

        public Modalidade Modalidade { get; set; }

        public NivelEnsino NivelEnsino { get; set; }

        public AnoReferencia AnoReferencia { get; set; }
    }
}