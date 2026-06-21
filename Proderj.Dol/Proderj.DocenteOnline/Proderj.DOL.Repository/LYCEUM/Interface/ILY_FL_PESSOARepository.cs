using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
    public interface ILY_FL_PESSOARepository : IRepository<LY_FL_PESSOA>
	{
        IQueryable<LY_FL_PESSOA> ListaQueryable();
	}
}
