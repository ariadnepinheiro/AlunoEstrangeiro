using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
    public interface ILY_PESSOARepository : IRepository<LY_PESSOA>
	{
        IQueryable<LY_PESSOA> ListaQueryable();
        void AlteraAuditada(LY_PESSOA entidade);
        string ObtemIdFuncional(string pessoa);
	}

}
