using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class GrupoHabilitacao
	{
		public GrupoHabilitacao()
		{
		}

		public virtual string Agrupamento { get; set; }

		public virtual string Descricao { get; set; }

		public virtual string Tipo { get; set; }

		public virtual string DisponibilidadeGLPDocente { get; set; }
	}
}
