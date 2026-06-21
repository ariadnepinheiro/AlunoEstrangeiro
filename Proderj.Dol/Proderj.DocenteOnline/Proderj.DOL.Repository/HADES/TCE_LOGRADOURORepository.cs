using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class TCE_LOGRADOURORepository : NHRepositoryBase<TCE_LOGRADOURO>, ITCE_LOGRADOURORepository
    {
        public override IQueryable<TCE_LOGRADOURO> ListaQueryable()
        {
            return base.ListaQueryable();
        }
    }
}
