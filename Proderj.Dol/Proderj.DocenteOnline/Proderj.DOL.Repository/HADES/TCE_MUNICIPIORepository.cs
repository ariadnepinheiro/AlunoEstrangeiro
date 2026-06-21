using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class TCE_MUNICIPIORepository : NHRepositoryBase<TCE_MUNICIPIO>, ITCE_MUNICIPIORepository
    {
        public override IQueryable<TCE_MUNICIPIO> ListaQueryable()
        {
            return base.ListaQueryable();
        }
    }
}
