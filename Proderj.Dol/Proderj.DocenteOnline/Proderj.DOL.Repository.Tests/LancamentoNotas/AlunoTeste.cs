using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class AlunoTeste : TesteBaseRepositorio<AlunoRepository>
	{
		[TestMethod]
		public void ObtemNomePorMatricula()
		{
			var matricula = "2000000000000001";
			
			string nome = Repositorio.ObtemNomePor(matricula);

			Assert.IsTrue(nome == "Lula Inácio da Silva");

		}
	}
}
