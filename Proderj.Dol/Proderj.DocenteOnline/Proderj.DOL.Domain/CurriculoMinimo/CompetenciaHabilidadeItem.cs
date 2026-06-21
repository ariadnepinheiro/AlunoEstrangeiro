using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class CompetenciaHabilidadeItem
    {
		public CompetenciaHabilidadeItem() 
		{
			CompetenciaHabilidadeGrupo = new CompetenciaHabilidadeGrupo();
		}
        
		public virtual int Id { get; set; }
		
		public virtual int Ordem { get; set; }

		public virtual string CompetenciaHabilidade { get; set; }

		public virtual DateTime DataCadastro { get; set; }
		
		public virtual CompetenciaHabilidadeGrupo CompetenciaHabilidadeGrupo { get; set; }
    }
}
