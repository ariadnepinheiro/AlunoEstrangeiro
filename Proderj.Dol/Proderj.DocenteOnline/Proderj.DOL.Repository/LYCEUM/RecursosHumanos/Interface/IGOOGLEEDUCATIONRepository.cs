using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;
using System.Collections.Generic;

namespace Proderj.DOL.Repository
{
    public interface IGOOGLEEDUCATIONRepository : IRepository<GOOGLEEDUCATION>
    {
        IQueryable<GOOGLEEDUCATION> ListaQueryable();
    }
}
