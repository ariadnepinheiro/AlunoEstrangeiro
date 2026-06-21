using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Exception
{	
	public class CadastroGlpListaException : ApplicationException
	{
		public List<CadastroGlpException> ListaExcecoes { get; set; }

		public CadastroGlpListaException(List<CadastroGlpException> listaExcecoes)
		{
			this.ListaExcecoes = listaExcecoes;
		}
	}
}
