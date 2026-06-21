using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class AvaliacaoCurriculoMinimoTeste : TesteBaseRepositorio<AvaliacaoCurriculoMinimoRepository>
	{
		[TestMethod]
		public void EnumeraPor()
		{
			string matricula = "08376535";
			short subPeriodo = 1;
			short ano = 2012;
			short periodo = 0;

			var resultado = Repositorio.EnumeraPor(matricula, ano, periodo, subPeriodo);
			Assert.IsTrue(resultado.Count() == 5);
			Assert.IsNull(resultado.Last().Resposta);
		}
	}
}
