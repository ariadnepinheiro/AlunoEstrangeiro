using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class UnidadeEnsinoTeste : TesteBaseRepositorio<UnidadeEnsinoRepository>
	{
		[TestMethod]
		public void EnumeraPor()
		{
			var resultado = Repositorio.EnumeraPor(1);
			Assert.IsNotNull(resultado);
		}
	}
}
