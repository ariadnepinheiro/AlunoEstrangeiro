using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class LogAvaliacaoCurriculoMinimoDocenteTeste : TesteBaseRepositorio<LogAvaliacaoCurriculoMinimoDocenteRepository>
	{
		[TestMethod]
		public void InserePor()
		{
			string matricula = "08376535";
			short subPeriodo = 1;
			short ano = 2012;
			short periodo = 0;

			int resultado = Repositorio.InserePor(ano, periodo, subPeriodo, matricula);

			Assert.IsTrue(resultado > 0);
		}
	}
}
