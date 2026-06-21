using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
    public interface ILY_DOCENTERepository : IRepository<LY_DOCENTE>
	{
        IQueryable<LY_DOCENTE> ListaQueryable();
	}
}
