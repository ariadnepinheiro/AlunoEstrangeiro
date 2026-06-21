using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class AvaliacaoExterna
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        public int? IdAvaliacaoExterna { get; set; }

        [Display(Name = "Nome da Avaliação")]
        [StringLength(20, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesAvaliacaoExterna { get; set; }

        [Display(Name = "Período")]
        [StringLength(20, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesPeriodoAvaliacao { get; set; }

        public override string ToString()
        {
            return IdAvaliacaoExterna.ToString();
        }
    }
}