using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class AvaliacaoCurriculoMinimoDocente
	{
		public AvaliacaoCurriculoMinimoDocente()
		{
			AvaliacaoCurriculoMinimo = new AvaliacaoCurriculoMinimo();
		}

		public virtual int Id { get; set; }

		public virtual AvaliacaoCurriculoMinimo AvaliacaoCurriculoMinimo { get; set; }

		public virtual bool Resposta { get; set; }

		public virtual string Matricula { get; set; }

		public virtual DateTime DataCadastro { get; set; }
	}
}
