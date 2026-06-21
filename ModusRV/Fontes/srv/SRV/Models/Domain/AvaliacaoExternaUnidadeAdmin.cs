using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class AvaliacaoExternaUnidadeAdmin
    {
        [PrimaryKey]
        public AvaliacaoExterna AvaliacaoExterna { get; set; }

        [PrimaryKey]
        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }
        
        public Decimal PercParticipacao { get; set; }

        [PrimaryKey]
        public AnoReferencia AnoReferencia { get; set; }

		public Decimal? PercParticipacaoDiurno { get; set; }

		public Decimal? PercParticipacaoNoturno { get; set; }
    }
}