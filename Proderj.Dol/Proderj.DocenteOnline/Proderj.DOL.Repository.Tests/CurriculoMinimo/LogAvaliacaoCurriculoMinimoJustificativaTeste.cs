using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class LogAvaliacaoCurriculoMinimoJustificativaTeste : TesteBaseRepositorio<LogAvaliacaoCurriculoMinimoJustificativaRepository>
	{
		[TestMethod]
		public void RemoveCompetenciasAntigas()
		{
			string matricula = "08376535";
			short subPeriodo = 1;
			short ano = 2012;
			short periodo = 0;

			Repositorio.InicializaTransacao();

			int resultado = Repositorio.InserePor(ano, periodo, subPeriodo, matricula);
			Assert.IsTrue(resultado == 1);

			Repositorio.TransacaoRollback();
		}
	}
}
