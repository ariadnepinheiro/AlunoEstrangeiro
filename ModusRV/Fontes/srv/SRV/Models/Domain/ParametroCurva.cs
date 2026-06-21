using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class ParametroCurva
    {
        [PrimaryKey]
        public AnoReferencia AnoReferencia { get; set; }

        [PrimaryKey]
        public Nota Nota { get; set; }

        public decimal QuantidadeVencimento { get; set; }

        [PrimaryKey]
        public GrupoFuncao GrupoFuncao { get; set; }

        [PrimaryKey]
        public TipoUnidadeAdministrativa TipoUnidadeAdministrativa { get; set; }
    }
}