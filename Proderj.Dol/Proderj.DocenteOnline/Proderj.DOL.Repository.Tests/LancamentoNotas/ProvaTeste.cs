using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class ProvaTeste : TesteBaseRepositorio<ProvaRepository>
	{
		[TestMethod]
		public void ObtemProvaPor()
		{
			var disciplina = "159-EJA-1-36";
			var turma = "CEJAS-MAT36-181578";
			short ano = 2012;
			short periodo = 0;
			short subperiodo = 1;

			var resultado = Repositorio.ObtemPor(disciplina, turma, ano, periodo, subperiodo);

			Assert.IsNotNull(resultado);
		}
	}
}
