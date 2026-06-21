using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class Modalidade
    {
        [PrimaryKey]
        public int? IdModalidade { get; set; }

        [Display(Name = "Modalidade")]
        [StringLength(50, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesModalidade { get; set; }

        [Display(Name = "Código")]
        [StringLength(20, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string SiglaModalidade { get; set; }

        public override string ToString()
        {
            return IdModalidade.ToString();
        }
    }
}