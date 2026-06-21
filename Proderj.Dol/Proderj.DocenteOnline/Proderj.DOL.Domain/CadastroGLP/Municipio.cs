using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class Municipio
	{
		public Municipio()
		{
		}

		public virtual string Codigo { get; set; }

		public virtual string Nome { get; set; }

		public virtual string SiglaUF { get; set; }
	}
}
