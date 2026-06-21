using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class LogCompetenciaHabilidadeDocente
	{
		public LogCompetenciaHabilidadeDocente()
		{
		}

		public virtual int Id { get; set; }
		
		public virtual int IdCompetenciaHabilidadeItem { get; set; }
		
		public virtual string Disciplina { get; set; }
		
		public virtual string Turma { get; set; }
		
		public virtual short Ano { get; set; }
		
		public virtual short Periodo { get; set; }
		
		public virtual short SubPeriodo { get; set; }
		
		public virtual string Matricula { get; set; }
		
		public virtual DateTime DataCadastro { get; set; }

	}
}
