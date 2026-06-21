using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class LY_FL_PESSOARepository : NHRepositoryBase<LY_FL_PESSOA>, ILY_FL_PESSOARepository
    {
        public override IQueryable<LY_FL_PESSOA> ListaQueryable()
        {
            return base.ListaQueryable();
        }
    }
}
