using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRV.Models.DTO
{
    public class RptRegionalExtratoServidor
    {
        public int IdUnidadeAdministrativa { get; set; }

        public string Regional { get; set; }

        public string Unidade { get; set; }

        public string FuncaoAtividade { get; set; }

        public string FuncaoBonificacao { get; set; }

        public decimal Proporcionalidade { get; set; }

        public string Periodo { get; set; }

        public decimal CoeficienteBonificacao { get; set; }

        public decimal? Alocacao { get; set; }

        public bool Eligibilidade { get; set; }

        public decimal Resultado { get; set; }

        public bool EligibilidadeUnidadeServidor { get; set; }
    }
}