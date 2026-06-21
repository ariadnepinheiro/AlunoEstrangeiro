using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class TipoOcorrencia
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        public int? IdTipoOcorrencia { get; set; }

        [Display(Name = "Tipo de Ocorrência")]
        [StringLength(50, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesTipoOcorrencia { get; set; }

        public override string ToString()
        {
            return IdTipoOcorrencia.ToString();
        }
    }
}