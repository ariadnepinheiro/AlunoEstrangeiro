using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class CriterioUnidadeAdministrativa
    {
        [PrimaryKey]
        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }

        [PrimaryKey]
        public AnoReferencia AnoReferencia { get; set; }

        public decimal? PerCurriculoMinimo { get; set; }

        public decimal? PercLancamentoNota { get; set; }

        public decimal? NotaIge { get; set; }
    }
}