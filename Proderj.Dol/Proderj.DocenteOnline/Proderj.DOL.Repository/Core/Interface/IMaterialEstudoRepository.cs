using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;
//using Proderj.DOL.Domain.LancamentoNotas;

namespace Proderj.DOL.Repository
{
    public interface IMaterialEstudoRepository : IRepository<MaterialEstudo>
    {
        IList<MaterialEstudo> ObtemPor(string identificador);

        IList<MaterialEstudo> ObtemIds();
                
    }
}
