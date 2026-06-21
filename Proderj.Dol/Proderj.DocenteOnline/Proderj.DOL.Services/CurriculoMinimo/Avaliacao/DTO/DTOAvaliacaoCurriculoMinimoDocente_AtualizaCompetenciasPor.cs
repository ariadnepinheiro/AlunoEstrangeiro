using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOAvaliacaoCurriculoMinimoDocente_AtualizaCompetenciasPor
	{
		public DTOAvaliacaoCurriculoMinimoDocente_AtualizaCompetenciasPor()
		{
			AvaliacoesCurriculoMinimo = new List<DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao>();
		}

		public short Ano { get; set; }

		public short Periodo { get; set; }

		public short SubPeriodo { get; set; }

		public string Matricula { get; set; }

		public List<DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao> AvaliacoesCurriculoMinimo { get; set; }
	}
}
