using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOCadastroGlp_DocenteDisponivel
	{
		public int DocenteDisponivelGlpId { get; set; }

		public DateTime? HoraFinal { get; set; }

		public DateTime? HoraInicio { get; set; }

		public DiaDaSemanaEnum DiaSemana { get; set; }

		public string CodigoDisciplina { get; set; }

		public string DescricaoDisciplina { get; set; }

		public string CodigoMunicipio { get; set; }

		public string NomeMunicipio { get; set; }

		public int CodigoCoordenadoria { get; set; }

		public string DescricaoCoordenadoria { get; set; }

        public int CodigoRegional { get; set; }

        public string DescricaoRegional { get; set; }
	}
}
