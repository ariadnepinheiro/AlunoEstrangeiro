using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOLancamentoNotasSolicitacao
	{
		public string CodigoCurso { get; set; }
		public string TipoCurso { get; set; }
		public string CodigoUnidadeEnsino { get; set; }
		public short Ano { get; set; }
		public short Periodo { get; set; }
		public short Serie { get; set; }
		public string CodigoTurma { get; set; }
		public string CodigoDisciplina { get; set; }
		public string CodigoModalidade{ get; set; }
	}
}

