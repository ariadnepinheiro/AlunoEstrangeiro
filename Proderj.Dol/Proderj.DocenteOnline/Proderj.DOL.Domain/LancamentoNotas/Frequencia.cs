using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class Frequencia
	{
		public Frequencia()
		{ }

		public virtual short Ano { get; set; }

		public virtual short Semestre { get; set; }

		public virtual short SubPeriodo { get; set; }

		public virtual short AulasPrevistas { get; set; }

		public virtual short AulasDadas { get; set; }

		public virtual string Turma { get; set; }

		public virtual string Disciplina { get; set; }

		public virtual string Descricao { get; set; }

		public virtual string TipoFrequencia { get; set; }

	}
}
