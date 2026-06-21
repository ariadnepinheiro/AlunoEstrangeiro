using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class AnoReferencia
    {
        [PrimaryKey]
        [Display(Name = "Ano")]
        public int? IdAnoReferencia { get; set; }

        [Display(Name = "Início do Ano Letivo")]
        public DateTime? DtInicioPeriodoLetivo { get; set; }

        [Display(Name = "Fim do Ano Letivo")]
        public DateTime? DtFimPeriodoLetivo { get; set; }

        [Display(Name = "Procedure de Cálculo")]
        [StringLength(50, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string NomeProcCalculo { get; set; }

        public override string ToString()
        {
            return IdAnoReferencia.ToString();
        }
    }
}