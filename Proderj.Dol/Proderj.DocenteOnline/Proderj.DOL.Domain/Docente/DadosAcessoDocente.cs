using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class DadosAcessoDocente
    {
        public virtual string Id { get; set; }
        public virtual string IdFuncional { get; set; }
        public virtual DateTime LoginTime { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode() + this.LoginTime.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
