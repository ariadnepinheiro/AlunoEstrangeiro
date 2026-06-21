using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface IAvaliacaoCurriculoMinimoJustificativaRepository : IRepository<AvaliacaoCurriculoMinimoJustificativa>
	{
		AvaliacaoCurriculoMinimoJustificativa ObtemPor(short ano, short periodo, short subperiodo, string matricula);

		int RemoveCompetenciasAntigas(string matricula, short ano, short periodo, short subperiodo);

		int InsereCom(AvaliacaoCurriculoMinimoJustificativa avaliacaoCurriculoMinimoJustificativa);
	}
}
