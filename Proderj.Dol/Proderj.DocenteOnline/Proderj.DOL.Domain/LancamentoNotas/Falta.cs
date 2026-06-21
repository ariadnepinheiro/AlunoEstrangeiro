using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class Falta
	{
		public Falta()
		{ }

		public virtual short Ano { get; set; }

		public virtual short Semestre { get; set; }

		public virtual int Id { get; set; }

		public virtual double QuantFaltas { get; set; }
		
		public virtual string Disciplina { get; set; }

		public virtual string Turma { get; set; }

		public virtual string Aluno { get; set; }

		public virtual string Frequencia { get; set; }
	}
}
