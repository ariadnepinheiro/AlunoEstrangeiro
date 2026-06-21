using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class NotaConsolidado
    {
        public NotaConsolidado() { }

        public virtual short? SubPeriodo { get; set; }
        public virtual decimal? Media { get; set; }
    }
}
