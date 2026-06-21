using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOProvaParaLancamento
	{
		public virtual string TipoProva { get; set; }

		public virtual string Nome { get; set; }

		public virtual short Ordem { get; set; }

		public virtual string NotaMaxima { get; set; }
	}
}
