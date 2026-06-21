using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class FiltroUnidadeAdministrativa
    {
        [Display(Name = "Regional")]
        public int? IdRegional { get; set; }
        public IEnumerable Regionais { get; set; }

        [Display(Name = "Tipo Unidade Administrativa")]
        public int? IdTipoUnidadeAdministrativa { get; set; }
        public IEnumerable TiposUnidadesAdministrativa { get; set; }

        public Paging<UnidadeAdministrativa> Unidades { get; set; }
    }
}