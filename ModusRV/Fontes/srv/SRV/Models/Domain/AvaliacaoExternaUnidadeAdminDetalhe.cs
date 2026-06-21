using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Common.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SRV.Models.Domain
{
    public enum Periodo{
		   [DescriptionAttribute("Anual")]
		   Anual = 0,
		   [DescriptionAttribute("1º Semestre")]
		   Semestre1 = 1,
		   [DescriptionAttribute("2º Semestre")]
		   Semestre2 = 2
	}
	
	public class AvaliacaoExternaUnidadeAdminDetalhe
    {
        [PrimaryKey]
		public int IdAvaliacaoExternaUnidadeAdminDetalhe { get; set; }
		
        public UnidadeAdministrativa UnidadeAdministrativa { get; set; }
        
        public AnoReferencia AnoReferencia { get; set; }

		public string Turma { get; set;}
			
		public Periodo Periodo { get; set; }

		public Turno Turno { get; set; }

		public AvaliacaoExterna AvaliacaoExterna { get; set; }

		public int Previsto { get; set; }
		
		public int Realizado { get; set; }

		public decimal PercentualParticipacao
		{
			get
			{
				return Convert.ToDecimal(Realizado)/Convert.ToDecimal(Previsto);
			}
		}
    }
}