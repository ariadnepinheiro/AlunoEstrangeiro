using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
    public interface INotaConsolidadoRepository: IRepository<NotaConsolidado>
    {
        IEnumerable<NotaConsolidado> EnumeraPor(string codigoDisciplina, string codigoTurma, short ano, short periodo);
    }
}
