using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Models.Domain;

namespace SRV.Models.DTO
{
    public class FiltroLancamentoNotaDocente
    {
        [Display(Name = "Matrícula")]
        public int? IdServidor { get; set; }

        [Display(Name = "Nome")]
        public string DesNomeServidor { get; set; }

		public int IdAnoReferencia { get; set; }

        public Paging<LancamentoNotaDocente> LancamentosNotasDocentes { get; set; }
    }
}