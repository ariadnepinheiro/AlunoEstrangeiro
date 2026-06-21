using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class ItemTabelaTeste : TesteBaseRepositorio<ItemTabelaRepository>
	{
		[TestMethod]
		public void EnumeraPor()
		{
			var tab = "JustificativaNota";
			var resultado = Repositorio.EnumeraPor(tab);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado.Count() == 2);

			tab = "Moeda";
			resultado = Repositorio.EnumeraPor(tab);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado.Count() == 3);

			tab = "Moeda111";
			resultado = Repositorio.EnumeraPor(tab);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado.Count() == 0);
		}
	}
}
