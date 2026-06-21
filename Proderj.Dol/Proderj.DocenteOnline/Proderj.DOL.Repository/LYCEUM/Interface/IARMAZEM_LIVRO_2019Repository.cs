using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public interface IARMAZEM_LIVRO_2019Repository : IRepository<ARMAZEM_LIVRO_2019>
    {
        string ObtemCodigoPor(string matricula);
	}
}
