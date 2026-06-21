using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public interface IDadosCapacitacaoDocenteRepository: IRepository<DadosCapacitacaoDocente>
    {
        IList<DadosCapacitacaoDocente> ListaPor(string matricula);
    }
}
