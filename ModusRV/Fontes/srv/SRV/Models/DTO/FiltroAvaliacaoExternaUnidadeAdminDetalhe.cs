using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SRV.Models.DTO
{
    public class FiltroAvaliacaoExternaUnidadeAdminDetalhe
    {
        public int? IdAvaliacaoExterna { get; set; }

        public int? IdUnidadeAdministrativa { get; set; }

		public int IdAnoReferencia { get; set; }
    }
}