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
	public class AvaliacaoCurriculoMinimoDocenteTeste : TesteBaseRepositorio<AvaliacaoCurriculoMinimoDocenteRepository>
	{
		[TestMethod]
		public void RemoveCompetenciasAntigas()
		{
			string matricula = "08376535";
			short subPeriodo = 1;
			short ano = 2012;
			short periodo = 0;

			Repositorio.InicializaTransacao();

			int resultado = Repositorio.RemoveCompetenciasAntigas(matricula, ano, periodo, subPeriodo);

			//Assert.IsTrue(resultado > 0);
			//Assert.IsTrue(resultado == 3);

			Repositorio.TransacaoRollback();
		}

		[TestMethod]
		public void InserePor()
		{
			Repositorio.InicializaTransacao();
			var competencia = new AvaliacaoCurriculoMinimoDocente
			{
				Matricula = "08376535",
				AvaliacaoCurriculoMinimo = new AvaliacaoCurriculoMinimo { Id = 35 },
				Resposta = true
			};

			var resultado = Repositorio.InserePor(competencia);

			Repositorio.TransacaoRollback();

		}

		[TestMethod]
		public void InsereListaPor()
		{
			var competencia = new AvaliacaoCurriculoMinimoDocente
			{
				Matricula = "08376535",
				AvaliacaoCurriculoMinimo = new AvaliacaoCurriculoMinimo { Id = 35 },
				Resposta = false
			};

			var competencia2 = new AvaliacaoCurriculoMinimoDocente
			{
				Matricula = "08376535",
				AvaliacaoCurriculoMinimo = new AvaliacaoCurriculoMinimo { Id = 36 },
				Resposta = false
			};

			var competencia3 = new AvaliacaoCurriculoMinimoDocente
			{
				Matricula = "08376535",
				AvaliacaoCurriculoMinimo = new AvaliacaoCurriculoMinimo { Id = 37 },
				Resposta = false
			};

			var listaCompetenciaHabilidade = new List<AvaliacaoCurriculoMinimoDocente>
			{
				competencia,competencia2,competencia3
			};

			Repositorio.InicializaTransacao();

			bool retorno = Repositorio.InserePor(listaCompetenciaHabilidade, false);
			Assert.IsTrue(retorno);

			Repositorio.TransacaoRollback();
		}
	}
}
