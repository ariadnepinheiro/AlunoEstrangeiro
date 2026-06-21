using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface IAvaliacaoCurriculoMinimoRepository : IRepository<AvaliacaoCurriculoMinimo>
	{
		IEnumerable<TOAvaliacaoCurriculoMinimoListagem> EnumeraPor(string matricula, short ano, short periodo, short subperiodo);
	}
}
