using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOAvaliacaoCurriculoMinimo_AvaliacoesEJustificativa
	{
		public DTOAvaliacaoCurriculoMinimo_AvaliacoesEJustificativa()
		{
			ListaAvaliacaoCurriculoMinimo = new List<DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao>();
		}

		public List<DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao> ListaAvaliacaoCurriculoMinimo { get; set; }

		public string DescricaoJustificativa { get; set; }
	}
}
