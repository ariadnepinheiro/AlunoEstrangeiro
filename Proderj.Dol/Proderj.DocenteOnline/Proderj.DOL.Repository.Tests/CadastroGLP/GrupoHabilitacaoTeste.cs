using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class GrupoHabilitacaoTeste : TesteBaseRepositorio<GrupoHabilitacaoRepository>
	{
		[TestMethod]
		public void ExistePor()
		{
			string agrupamento = "207";
			bool existe = Repositorio.ExistePor(agrupamento);
			Assert.IsTrue(existe);

			agrupamento = "20008";
			existe = Repositorio.ExistePor(agrupamento);
			Assert.IsFalse(existe);
		}
	}
}
