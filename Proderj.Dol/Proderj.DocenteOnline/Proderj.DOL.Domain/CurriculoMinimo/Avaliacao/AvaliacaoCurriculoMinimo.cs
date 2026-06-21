using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class AvaliacaoCurriculoMinimo
	{
		public AvaliacaoCurriculoMinimo()
		{
			AvaliacoesCurriculoMinimoDocente = new List<AvaliacaoCurriculoMinimoDocente>();
		}

		public virtual int Id { get; set; }

		public virtual IList<AvaliacaoCurriculoMinimoDocente> AvaliacoesCurriculoMinimoDocente { get; set; }
        
		public virtual short Ano { get; set; }
        
		public virtual short Periodo { get; set; }
        
		public virtual short SubPeriodo { get; set; }
        
		public virtual short Ordem { get; set; }
        
		public virtual string DescricaoAvaliacao { get; set; }
        
		public virtual bool Habilitado { get; set; }
        
		public virtual string Matricula { get; set; }
        
		public virtual DateTime DataCadastro { get; set; }
	}
}
