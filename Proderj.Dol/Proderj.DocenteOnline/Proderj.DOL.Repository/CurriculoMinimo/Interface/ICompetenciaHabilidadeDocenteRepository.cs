using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface ICompetenciaHabilidadeDocenteRepository : IRepository<CompetenciaHabilidadeDocente>
	{
		IEnumerable<CompetenciaHabilidadeDocente> EnumeraCompetencias(CompetenciaHabilidadeDocente competencia);

		int RemoveCompetenciasAntigas(TOCompetenciaHabilidadeDocente_RemoveCompetenciasAntigas voRemoveCompetenciasAntigas);

		int Insere(TOCompetenciaHabilidade_Insere voInsereCompetenciaHabilidade);

		bool Insere(IList<TOCompetenciaHabilidade_Insere> voInsereCompetenciaHabilidade, bool abrirTransacaoParaLista);
	}
}
