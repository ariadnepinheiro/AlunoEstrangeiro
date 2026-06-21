using System;
using Proderj.Foundation.Architecture;
using System.Collections.Generic;
namespace Proderj.DOL.Repository
{
    public interface IStoredProcedures : IRepository<object>
    {
        IList<DTOREL_CH_SERV_ANO> REL_CH_SERV_ANO(string idfuncional, string ano);
    }
}
