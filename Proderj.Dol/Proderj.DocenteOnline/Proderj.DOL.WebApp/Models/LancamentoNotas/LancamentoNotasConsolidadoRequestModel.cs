using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proderj.DOL.WebApp.Models
{
    public class LancamentoNotasConsolidadoRequestModel
    {
        [Required]
        public string CodigoDisciplina { get; set; }

        [Required]
        public string CodigoTurma { get; set; }

        [Required]
        public int Ano { get; set; }

        [Required]
        public short Periodo { get; set; }
    }
}