using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTONotaSalva
	{
		public string CodigoAluno { get; set; }

		public decimal? Nota { get; set; }
		
		public bool RecuperacaoParalela { get; set; }

		public bool SemAvaliacao { get; set; }

        public decimal? NotaProva { get; set; }

        public decimal? NotaRecuperacao { get; set; }

        public String CodigoJustificativa { get; set; }
	}
}
