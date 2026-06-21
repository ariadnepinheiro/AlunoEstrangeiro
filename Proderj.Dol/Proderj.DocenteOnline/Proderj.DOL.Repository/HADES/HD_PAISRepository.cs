using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class HD_PAISRepository : NHRepositoryBase<HD_PAIS>, IHD_PAISRepository
    {
        public override IQueryable<HD_PAIS> ListaQueryable()
        {
            return base.ListaQueryable();
        }
    }
}
