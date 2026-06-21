using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;
using System.Collections;
using System.Dynamic;

namespace Proderj.DOL.Repository
{
    public interface ILY_UNIDADE_ENSINORepository : IRepository<LY_UNIDADE_ENSINO>
	{
        IQueryable<LY_UNIDADE_ENSINO> ListaQueryable();
        IList<TDTO> CreateSQLQuery<TDTO>(string queryString, IDictionary<string, object> parameters);
        IList<T> ListaMunicipioPor<T>(int id_regional);
	}
}
