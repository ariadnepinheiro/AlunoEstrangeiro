using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOSolicitacaoAlteracaoNotas_ConsultaTurma
	{
		public short Ano { get; set; }
		public short SubPeriodo { get; set; }
		public long NumeroFuncionarioDocente { get; set; }
		public string CodigoTurma { get; set; }
		public string CodigoUnidadeEnsino { get; set; }
		public string CodigoDisciplina { get; set; }
	}
}
