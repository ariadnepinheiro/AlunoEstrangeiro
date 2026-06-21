using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Repository
{
	public class VOCompetenciaHabilidadeGrupoComResposta
	{
		public string DescricaoGrupo { get; set; }

		public string DescricaoCompetenciaHabilidade { get; set; }

		public int IdCompetenciaHabilidadeGrupo { get; set; }

		public int IdCompetenciaHabilidadeItem { get; set; }

		public bool Resposta { get; set; }

		public short OrdemGrupo { get; set; }

		public short OrdemResposta { get; set; }
	}
}
