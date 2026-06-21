using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTORespostaCurriculoMinimo_SalvaPor
	{
		public short Ano { get; set; }

		public short Periodo { get; set; }

		public short Subperiodo { get; set; }

		public string CodigoDisciplina { get; set; }

		public string CodigoTurma { get; set; }

		public string Matricula { get; set; }

		public List<int> IdsCompetenciaHabilidadeItem { get; set; }
	}	
}
