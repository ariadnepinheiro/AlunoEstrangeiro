using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class LY_GRUPO_HABILITACAORepository : NHRepositoryBase<LY_GRUPO_HABILITACAO>, ILY_GRUPO_HABILITACAORepository
    {
        public override IQueryable<LY_GRUPO_HABILITACAO> ListaQueryable()
        {
            return base.ListaQueryable();
        }
    }
}
