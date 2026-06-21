using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class SolicitacaoAlteracaoNotas
	{
		public SolicitacaoAlteracaoNotas()
		{ 
		}

		public virtual int Id { get; set; }

		public virtual short Ano { get; set; }

		public virtual short SubPeriodo { get; set; }

		public virtual short Periodo { get; set; }

		public virtual long NumeroFuncionario { get; set; }

		public virtual string Turma { get; set; }

		public virtual string Disciplina { get; set; }

		public virtual string Status { get; set; }

		public virtual string Justificativa { get; set; }

		public virtual string UnidadeEnsino { get; set; }

		public virtual DateTime DataStatus { get; set; }

		public virtual DateTime? DataLimite { get; set; }

		public virtual DateTime DataSolicitacao { get; set; }

	}
}
