using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public interface ILancamentoNotasConsolidadoRepository: IRepository<LancamentoNotasConsolidado>
    {
        IEnumerable<LancamentoNotasConsolidado> EnumeraLancamentosNotasConsolidado(string disciplina, string turma, short ano, short periodo);
    }
}
