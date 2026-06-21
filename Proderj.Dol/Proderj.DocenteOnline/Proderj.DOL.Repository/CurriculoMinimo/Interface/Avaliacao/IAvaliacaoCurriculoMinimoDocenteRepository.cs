using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface IAvaliacaoCurriculoMinimoDocenteRepository : IRepository<AvaliacaoCurriculoMinimoDocente>
	{
		int InserePor(AvaliacaoCurriculoMinimoDocente competenciaHabilidadeDocente);

		bool InserePor(IList<AvaliacaoCurriculoMinimoDocente> avaliacoesCurriculoMinimoDocente, bool abrirTransacaoParaLista);

		int RemoveCompetenciasAntigas(string matricula, short ano, short periodo, short subperiodo);
	}
}
