using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
    public interface IPeriodoLetivoService : IService
    {
        List<DTOPeriodoLetivo> Lista();

        List<DTOPeriodoLetivo> ListaPor(short ano);
    }
}
