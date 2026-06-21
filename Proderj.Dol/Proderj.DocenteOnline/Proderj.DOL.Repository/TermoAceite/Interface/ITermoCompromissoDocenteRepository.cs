using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public interface ITermoCompromissoDocenteRepository : IRepository<TermoCompromissoDocente>
    {
		TermoCompromissoDocente ObtemTermoNaoAceitoMaisRecentePor(string matricula);

        TermoCompromissoDocente ObtemTermoNaoAceitoMaisRecentePorIdFuncional(string idfuncional);
        

    }
}
