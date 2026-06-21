using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class Ocorrencia
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public int IdOcorrencia { get; set; }

        [Display(Name = "Ocorrência")]
        [StringLength(50, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesOcorrencia { get; set; }

        [Display(Name = "Tipo de Ocorrência")]
        public TipoOcorrencia TipoOcorrencia { get; set; }

        public override string ToString()
        {
            return IdOcorrencia.ToString();
        }
    }
}