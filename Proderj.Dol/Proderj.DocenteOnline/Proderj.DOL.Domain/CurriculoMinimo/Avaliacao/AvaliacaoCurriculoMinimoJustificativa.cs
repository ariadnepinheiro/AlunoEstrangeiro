using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class AvaliacaoCurriculoMinimoJustificativa
	{
		public AvaliacaoCurriculoMinimoJustificativa()
		{
		}

        public virtual int Id { get; set; }
        
		public virtual short Ano { get; set; }
        
		public virtual short Periodo { get; set; }
        
		public virtual short SubPeriodo { get; set; }
        
		public virtual string Justificativa { get; set; }
        
		public virtual string Matricula { get; set; }
	}
}
