using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public class DadosTurmaDocenteRepository: NHRepositoryBase<DadosTurmaDocente>, IDadosTurmaDocenteRepository
    {
        public IList<DadosTurmaDocente> ListaPor(string idFuncional)
        {
            return Sessao.QueryOver<DadosTurmaDocente>()
                .Where(dc => dc.IdFuncional == idFuncional)
                .OrderBy(x => x.Name).Asc
                .OrderBy(x => x.Section).Asc
                .List();
        }
    }
}
