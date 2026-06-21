using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.DTO
{
    public class ConsolidadoUnidadeAdm
    {
        public UnidadeConsolidado Unidade { get; set; }

        public ValorConsolidado PercCurriculoMinimoRegional { get; set; }

        public ValorConsolidado IderjERFundamental1 { get; set; }
        public ValorConsolidado IderjERFundamental2 { get; set; }
        public ValorConsolidado IderjEREnsinoMedio { get; set; }
        public ValorConsolidado IdEjaFundamental2 { get; set; }
        public ValorConsolidado IdEjaEnsinoMedio { get; set; }

        public ValorConsolidado PercLancamentoNotas { get; set; }
        public ValorConsolidado PercCurriculoMinimo { get; set; }

        public ValorConsolidado Saerjinho1 { get; set; }
        public ValorConsolidado Saerjinho2 { get; set; }
        public ValorConsolidado Saerjinho3 { get; set; }
        public ValorConsolidado Saerj { get; set; }

        public ValorConsolidado IdERFundamental1 { get; set; }
        public ValorConsolidado IfERFundamental1 { get; set; }

        public ValorConsolidado IdERFundamental2 { get; set; }
        public ValorConsolidado IfERFundamental2 { get; set; }

        public ValorConsolidado IdEREnsinoMedio { get; set; }
        public ValorConsolidado IfEREnsinoMedio { get; set; }

        public ValorConsolidado Ige { get; set; }

    }
}