using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTORespostaCurriculoMinimo_StatusPreenchimentoPorTurma
	{
		public DateTime? DataUltimoPreenchimento { get; set; }
		public StatusPreenchimentoCurriculoMinimoPorTurmaEnum StatusPreenchimento { get; set; }
		public decimal PercentualPreenchimentoParcial { get; set; }
	}
}
