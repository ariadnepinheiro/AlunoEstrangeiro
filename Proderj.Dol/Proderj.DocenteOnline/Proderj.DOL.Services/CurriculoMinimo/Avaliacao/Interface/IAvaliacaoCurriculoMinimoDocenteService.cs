using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
	public interface IAvaliacaoCurriculoMinimoDocenteService : IService
	{
		void AtualizaCompetenciasPor(DTOAvaliacaoCurriculoMinimoDocente_AtualizaCompetenciasPor dtoAtualizacao);
	}

	
}
