using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class ParametroPeso
    {
        [PrimaryKey]
        public int? IdParametroPeso { get; set; }

        public AnoReferencia AnoReferencia { get; set; }

        public Modalidade Modalidade { get; set; }

        public Indicador Indicador { get; set; }

        public TipoUnidadeAdministrativa TipoUnidadeAdministrativa { get; set; }

        public bool TemIGE { get; set; }

        public decimal ValorPeso { get; set; }

        public GrupoFuncao GrupoFuncao { get; set; }
    }
}