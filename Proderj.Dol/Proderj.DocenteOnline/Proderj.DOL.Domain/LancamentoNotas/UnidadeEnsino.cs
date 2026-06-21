using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class UnidadeEnsino
	{
		public UnidadeEnsino()
		{
			Municipio = new Municipio();
			Nucleo = new Nucleo();
            Regional = new Regional();
		}

		public virtual string Codigo { get; set; }

		public virtual string DescricaoCompleta { get; set; }

		public virtual Municipio Municipio { get; set; }

		public virtual Nucleo Nucleo { get; set; }

        public virtual Regional Regional { get; set; }
	}
}
