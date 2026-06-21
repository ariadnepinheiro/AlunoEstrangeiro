using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class AvaliacaoCurriculoMinimoJustificativaTeste : TesteBaseRepositorio<AvaliacaoCurriculoMinimoJustificativaRepository>
	{
		[TestMethod]
		public void ObtemPor()
		{
			string matricula = "08376535";
			short subPeriodo = 1;
			short ano = 2012;
			short periodo = 0;

			var resultado = Repositorio.ObtemPor(ano, periodo, subPeriodo, matricula);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado.Id > 33770);
			Assert.IsTrue(!resultado.Justificativa.IsNullOrEmpty());
		}

		[TestMethod]
		public void RemoveCompetenciasAntigas()
		{
			string matricula = "08376535";
			short subPeriodo = 1;
			short ano = 2012;
			short periodo = 0;

			Repositorio.InicializaTransacao();
			
			int resultado = Repositorio.RemoveCompetenciasAntigas(matricula, ano, periodo, subPeriodo);
			Assert.IsTrue(resultado == 1);

			Repositorio.TransacaoRollback();
		}

		[TestMethod]
		public void InsereCom()
		{
			var competencia = new AvaliacaoCurriculoMinimoJustificativa
			{
				Matricula = "08376535",
				SubPeriodo = 1,
				Ano = 2012,
				Periodo = 0,
				Justificativa = "TESTE2!"
			};

			Repositorio.InicializaTransacao();

			int resultado = Repositorio.InsereCom(competencia);
			Assert.IsTrue(resultado == 1);

			Repositorio.TransacaoRollback();
		}
	}
}
