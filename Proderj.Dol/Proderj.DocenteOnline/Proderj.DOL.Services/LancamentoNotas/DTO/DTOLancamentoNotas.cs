using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOLancamentoNotas
	{
		public virtual string NumeroChamada { get; set; }

		public virtual string NomeCompleto { get; set; }

		public virtual string SituacaoMatricula { get; set; }

		public virtual string DescricaoSituacao { get; set; }

		public virtual Double MediaNota { get; set; }

		public virtual short Faltas { get; set; }

		public virtual bool RecuperacaoPararela { get; set; }

		public virtual bool ComAvaliacao { get; set; }

		public virtual string Justificativa { get; set; }
	}
}

