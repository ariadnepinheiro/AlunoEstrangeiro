using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao
	{
		public int IdAvaliacaoCurriculoMinimo { get; set; }

		public int Ordem { get; set; }

		public string DescricaoAvaliacao { get; set; }

		public bool? EhAvaliadoPositivamente { get; set; }

	}



}
