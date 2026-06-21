using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class PeriodoLetivo
    {

        public PeriodoLetivo() 
        {
		}

        public virtual short Ano { get; set; }

        public virtual short Periodo { get; set; }

        public virtual string DescricaoPeriodo { get; set; }
    }
}
