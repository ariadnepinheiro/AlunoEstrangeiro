using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Exception
{
	public class LancamentoNotasExceptionList : ApplicationException
	{
		public List<LancamentoNotasException> ListaExcecoes { get; set; }

		public LancamentoNotasExceptionList(List<LancamentoNotasException> listaExcecoes)
		{
			this.ListaExcecoes = listaExcecoes;
		}
	}
}
