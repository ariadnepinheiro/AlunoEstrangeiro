using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
	public interface IUnidadeEnsinoRepository : IRepository<UnidadeEnsino>
	{
		UnidadeEnsino ObtemDescricaoPor(string codigoUnidadeEnsino);

        IEnumerable<UnidadeEnsino> EnumeraPor(int codigoRegional);

        IEnumerable<UnidadeEnsino> EnumeraPor(string codigoMunicipio);
	}
}
