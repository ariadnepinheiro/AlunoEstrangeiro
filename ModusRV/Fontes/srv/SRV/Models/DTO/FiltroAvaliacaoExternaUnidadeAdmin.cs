using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SRV.Models.DTO
{
    public class FiltroAvaliacaoExternaUnidadeAdmin
    {

        [Display(Name = "Avaliação Externa")]
        public int? IdAvaliacaoExterna { get; set; }
        public IEnumerable AvaliacoesExternas { get; set; }

        [Display(Name = "Unidade Administrativa")]
        public int? IdUnidadeAdministrativa { get; set; }
        public IEnumerable UnidadesAdministrativas { get; set; }

        public Paging<AvaliacaoExternaUnidadeAdmin> AvaliacoesUnidades { get; set; }

		public Paging<AvaliacaoExternaUnidadeAdminDetalhe> DetalhesAvaliacoesUnidades { get; set; }
    }
}