using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOFalta
	{
		public string CodigoAluno { get; set; }

		public string CodigoDisciplina { get; set; }

		public string CodigoTurma { get; set; }

		public short Ano { get; set; }

		public short Periodo { get; set; }

		public short? Faltas { get; set; }

		public string CodigoFrequencia { get; set; }
	}
}
