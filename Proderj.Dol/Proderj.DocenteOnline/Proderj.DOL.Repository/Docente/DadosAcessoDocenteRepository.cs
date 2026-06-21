using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public class DadosAcessoDocenteRepository: NHRepositoryBase<DadosAcessoDocente>, IDadosAcessoDocenteRepository
    {
        public IList<DadosAcessoDocente> ListaPor(string idFuncional)
        {
            return Sessao.QueryOver<DadosAcessoDocente>()
                .Where(dc => dc.IdFuncional == idFuncional)
                .OrderBy(x => x.LoginTime).Asc
                .List();
        }
    }
}
