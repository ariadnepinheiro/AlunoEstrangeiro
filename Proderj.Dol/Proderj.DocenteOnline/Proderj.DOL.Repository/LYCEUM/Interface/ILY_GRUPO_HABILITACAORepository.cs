using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
    public interface ILY_GRUPO_HABILITACAORepository : IRepository<LY_GRUPO_HABILITACAO>
	{
        IQueryable<LY_GRUPO_HABILITACAO> ListaQueryable();
	}
}
