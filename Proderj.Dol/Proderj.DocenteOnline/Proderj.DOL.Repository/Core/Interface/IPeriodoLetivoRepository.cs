using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public interface IPeriodoLetivoRepository : IRepository<PeriodoLetivo>
    {
        IEnumerable<PeriodoLetivo> Enumera();

        IEnumerable<PeriodoLetivo> EnumeraPor(short ano);
    }
}
