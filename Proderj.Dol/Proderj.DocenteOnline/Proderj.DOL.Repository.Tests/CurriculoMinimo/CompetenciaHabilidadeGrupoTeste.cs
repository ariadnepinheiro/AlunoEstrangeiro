using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class CompetenciaHabilidadeGrupoTeste : TesteBaseRepositorio<CompetenciaHabilidadeGrupoRepository>
	{
		[TestMethod]
		public void Obtem()
		{
			var teste = Repositorio.ObtemPorChavePrimaria(4458);

			Assert.IsNotNull(teste);
		}

		[TestMethod]
		public void EnumeraPor()
		{
			var competencia = new CompetenciaHabilidadeGrupo
			{
				Serie = 4,
				SubPeriodo = 1,
				Disciplina = "159-CNR-4-2",
				Ano = 2012,
				Periodo = 0,
				Curso = "0003.31",
				Modalidade = "NO9",
				TipoCurso = "3"
			};

			var teste = Repositorio.EnumeraPor(competencia);

			Assert.IsNotNull(teste);

			int quant = Repositorio.QuantidadeItensRespostaPor(competencia);

			Assert.IsTrue(quant > 0);
		}

		[TestMethod]
		public void EnumeraComRespostaPor()
		{
			var competencia = new TOCompetenciaHabilidadeGrupo_EnumeraPor
			{			
				Serie = 4,
				SubPeriodo = 1,
				Disciplina = "159-CNR-4-2",
				Ano = 2012,
				Periodo = 0,
				Curso = "0003.31",
				Modalidade = "NO9",
				TipoCurso = "3",
				CodigoTurma = "CN-4001-180040",
				Matricula = "08376535",
			};

			IEnumerable<VOCompetenciaHabilidadeGrupoComResposta> resultado = Repositorio.EnumeraComRespostaPor(competencia);

			Assert.IsNotNull(resultado);

			resultado.ExecuteForEach
			(x=>
				{
					//Assert.IsTrue(!x.Resposta);
					Assert.IsTrue(!x.DescricaoGrupo.IsNullOrEmpty());
				}
			);

			Assert.IsTrue(resultado.ElementAt(0).IdCompetenciaHabilidadeGrupo == 6015);
			Assert.IsTrue(resultado.ElementAt(1).IdCompetenciaHabilidadeGrupo == 6015);
			Assert.IsTrue(resultado.ElementAt(2).IdCompetenciaHabilidadeGrupo == 6016);
			Assert.IsTrue(resultado.ElementAt(3).IdCompetenciaHabilidadeGrupo == 6016);
			Assert.IsTrue(resultado.ElementAt(4).IdCompetenciaHabilidadeGrupo == 6016);
		}
	}
}
