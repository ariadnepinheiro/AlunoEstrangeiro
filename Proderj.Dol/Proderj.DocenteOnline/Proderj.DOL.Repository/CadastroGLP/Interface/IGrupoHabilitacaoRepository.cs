using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
	public interface IGrupoHabilitacaoRepository : IRepository<GrupoHabilitacao>
	{
		bool ExistePor(string agrupamento);

		IEnumerable<GrupoHabilitacao> Enumera();
        IEnumerable<GrupoHabilitacao> EnumeraPor(int num_func);
	}
}
