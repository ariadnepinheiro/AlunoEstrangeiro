using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOLancamentoAtualizacaoAulas
	{
		public string CodigoDisciplina { get; set; }

		public string CodigoTurma { get; set; }

		public short Ano { get; set; }

		public short Periodo { get; set; }

		public string CodigoFrequencia { get; set; }

		public string TipoProva { get; set; }

		public int? AulasPrevistas { get; set; }

		public int? AulasDadas { get; set; }
	}
}
