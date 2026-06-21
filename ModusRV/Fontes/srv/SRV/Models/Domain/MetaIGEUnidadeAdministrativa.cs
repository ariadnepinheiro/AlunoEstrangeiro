using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class MetaIGEUnidadeAdministrativa
    {
        [PrimaryKey]
        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }

        [PrimaryKey]
        public AnoReferencia AnoReferencia { get; set; }

        public decimal MetaIge { get; set; }
    }
}