using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
	public interface IHD_PAISRepository : IRepository<HD_PAIS>
	{
        IQueryable<HD_PAIS> ListaQueryable();
	}
}
