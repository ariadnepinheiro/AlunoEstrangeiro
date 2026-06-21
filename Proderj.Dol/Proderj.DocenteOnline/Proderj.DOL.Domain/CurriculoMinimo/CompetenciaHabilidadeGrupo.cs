using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class CompetenciaHabilidadeGrupo
    {
		public CompetenciaHabilidadeGrupo() 
		{
			CompetenciaHabilidadeItens = new List<CompetenciaHabilidadeItem>();
        }

        public virtual int Id { get; set; }

		public virtual string Grupo { get; set; }

        public virtual string Disciplina { get; set; }

        public virtual string Curso { get; set; }
        
		public virtual string Modalidade { get; set; }
        
		public virtual string TipoCurso { get; set; }

		public virtual short Ano { get; set; }

		public virtual short Serie { get; set; }

		public virtual short Periodo { get; set; }

		public virtual short SubPeriodo { get; set; }

		public virtual short Ordem { get; set; }

		public virtual IList<CompetenciaHabilidadeItem> CompetenciaHabilidadeItens { get; set; }
        
	}
}
