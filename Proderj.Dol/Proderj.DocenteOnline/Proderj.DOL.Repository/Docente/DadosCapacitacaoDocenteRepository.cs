using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public class DadosCapacitacaoDocenteRepository: NHRepositoryBase<DadosCapacitacaoDocente>, IDadosCapacitacaoDocenteRepository
    {
        public IList<DadosCapacitacaoDocente> ListaPor(string matricula)
        {
            return Sessao.QueryOver<DadosCapacitacaoDocente>()
                .Where(dc => dc.Matricula == matricula)
                .List();
        }
    }
}
