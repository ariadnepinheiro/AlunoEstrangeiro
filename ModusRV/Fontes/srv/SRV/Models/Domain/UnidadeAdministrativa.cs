using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Common.Logging;

namespace SRV.Models.Domain
{
    public class UnidadeAdministrativa
    {
        [PrimaryKey]
        [Display(Name = "Código")]
        public int? IdUnidadeAdministrativa { get; set; }

        [Display(Name = "Unidade Administrativa")]
        [StringLength(200, ErrorMessage = "Campo não pode ter mais de {1} caracteres")]
        public string DesUnidadeAdministrativa { get; set; }

        [Display(Name = "Tipo de Unidade")]
        public TipoUnidadeAdministrativa TipoUnidadeAdministrativa { get; set; }

        [Display(Name = "Regional")]
        public UnidadeAdministrativa Regional { get; set; }

        public string IdCenso { get; set; }

        public override string ToString()
        {
            return IdUnidadeAdministrativa.ToString();
        }
    }
}