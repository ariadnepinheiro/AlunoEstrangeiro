using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface ICompetenciaHabilidadeGrupoRepository : IRepository<CompetenciaHabilidadeGrupo>
	{
		IEnumerable<CompetenciaHabilidadeGrupo> EnumeraPor(CompetenciaHabilidadeGrupo competencia);

		IEnumerable<VOCompetenciaHabilidadeGrupoComResposta> EnumeraComRespostaPor(TOCompetenciaHabilidadeGrupo_EnumeraPor competencia);

		int QuantidadeItensRespostaPor(CompetenciaHabilidadeGrupo competencia);
	}
}
