using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class SubPeriodoLetivo
    {
        public SubPeriodoLetivo() 
        {
		}

        public virtual short SubPeriodo { get; set; }
        
        public virtual short Ano { get; set; }
        
        public virtual short Periodo { get; set; }

        public virtual string Descricao { get; set; }
        
        public virtual DateTime? DataInicio { get; set; }
        
        public virtual DateTime? DataLancamento { get; set; }

        public virtual DateTime? DataCurriculoMinimo { get; set; }
      
    }
}
