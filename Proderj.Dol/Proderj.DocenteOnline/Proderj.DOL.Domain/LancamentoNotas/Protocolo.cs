using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class Protocolo
	{
		public Protocolo()
		{
		}

		public virtual int Id { get; set; }

		public virtual short Ano { get; set; }

		public virtual short Periodo { get; set; }

		public virtual short SubPeriodo { get; set; }
		
		public virtual string Disciplina { get; set; }
		
		public virtual string NomeDisciplina { get; set; }
		
		public virtual string Turma { get; set; }
		
		public virtual string IdFuncional { get; set; }
		
		public virtual string Tipo { get; set; }

		public virtual DateTime DataCadastro { get; set; }
	}
}
