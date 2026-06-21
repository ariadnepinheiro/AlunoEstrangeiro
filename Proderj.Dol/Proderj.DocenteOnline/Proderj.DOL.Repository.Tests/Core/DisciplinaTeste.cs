using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class DisciplinaTeste : TesteBaseRepositorio<DisciplinaRepository>
	{
		[TestMethod]
		public void ObtemConceitosDisciplina()
		{
			var disciplina = "159-EJA-1-36";
			
			var resultado = Repositorio.ObtemConceitosPor(disciplina);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado.NotaMaxima == "10");
			Assert.IsTrue(resultado.QuantCasasDecimais == 2);
		}
	}
}
