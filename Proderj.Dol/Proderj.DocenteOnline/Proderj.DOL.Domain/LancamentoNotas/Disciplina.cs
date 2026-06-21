using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class Disciplina
	{
		public Disciplina()
		{ }

		public virtual string CodigoDisciplina { get; set; }

		public virtual string DescricaoCompleta { get; set; }

		public virtual string GrupoNota { get; set; }

		public virtual short QuantCasasDecimais { get; set; }

		public virtual string NotaMaxima { get; set; }

        public virtual string TemNota { get; set; }

        public virtual string TemFrequencia { get; set; }
	}
}
