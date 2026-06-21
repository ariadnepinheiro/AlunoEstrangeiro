using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Models.Domain;
using System.Collections;

namespace SRV.Models.DTO
{
    public class FiltroMetaIGEUnidadeAdministrativa
    {
        [Display(Name = "Unidade Administrativa")]
        public int? IdUnidadeAdministrativa { get; set; }
        public IEnumerable UnidadesAdministrativas { get; set; }

        public int IdAnoReferencia { get; set; }

        public Paging<MetaIGEUnidadeAdministrativa> Metas { get; set; }
    }
}