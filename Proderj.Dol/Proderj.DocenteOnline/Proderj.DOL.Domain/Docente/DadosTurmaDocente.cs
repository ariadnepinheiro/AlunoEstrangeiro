using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class DadosTurmaDocente
    {
        public virtual string Id { get; set; }
        public virtual string IdFuncional { get; set; }
        public virtual string Name { get; set; }
        public virtual string Section { get; set; }
    }
}
