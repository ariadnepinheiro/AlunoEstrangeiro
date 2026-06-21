using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class LY_DOCENTERepository : NHRepositoryBase<LY_DOCENTE>, ILY_DOCENTERepository
    {
        public override IQueryable<LY_DOCENTE> ListaQueryable()
        {
            return base.ListaQueryable();
        }
    }
}
