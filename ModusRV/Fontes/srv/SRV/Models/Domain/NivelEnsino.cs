using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class NivelEnsino
    {
        [PrimaryKey]
        public int? IdNivelEnsino { get; set; }

        [PrimaryKey]
        [Display(Name = "Modalidade")]
        public Modalidade Modalidade { get; set; }

        [Display(Name = "Código Nível de Ensino")]
        [StringLength(50, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesNivelEnsino { get; set; }

        public override string ToString()
        {
            return IdNivelEnsino.ToString();
        }
    }
}