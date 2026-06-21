using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class CompetenciaHabilidadeDocenteTeste : TesteBaseRepositorio<CompetenciaHabilidadeDocenteRepository>
	{
		[TestMethod]
		public void EnumeraCompetencias()
		{
			var competencia = new CompetenciaHabilidadeDocente
			{
				Matricula = "09488826",
				Turma = "3002-181597",
				SubPeriodo = 2,
				Disciplina = "728-EMR-3-4",
				Ano = 2011,
				Periodo = 0,
			};

			var resultado = Repositorio.EnumeraCompetencias(competencia);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado.Count() == 6);
			Assert.IsNotNull(resultado.First().DataCadastro);
		}

		[TestMethod]
		public void RemoveCompetenciasAntigas()
		{
			var voRemoveCompetenciasAntigas = new TOCompetenciaHabilidadeDocente_RemoveCompetenciasAntigas
			{
				Matricula = "08376535",
				CodigoTurma = "CN-4001-180040",
				SubPeriodo = 1,
				CodigoDisciplina = "159-CNR-4-2",
				Ano = 2012,
				Periodo = 0
			};

			Repositorio.InicializaTransacao();

			int resultado = Repositorio.RemoveCompetenciasAntigas(voRemoveCompetenciasAntigas);

			Repositorio.TransacaoRollback();
		}

		[TestMethod]
		public void InsereCompetenciaHabilidadeDocente()
		{
			var competencia = new TOCompetenciaHabilidade_Insere
			{
				Matricula = "08376535",
				CodigoTurma = "CN-4001-180040",
				SubPeriodo = 1,
				CodigoDisciplina = "159-CNR-4-2",
				Ano = 2012,
				Periodo = 0,
				IdCompetenciaHabilidadeItem = 37605
			};

			var competencia2 = new TOCompetenciaHabilidade_Insere
			{
				Matricula = "08376535",
				CodigoTurma = "CN-4001-180040",
				SubPeriodo = 1,
				CodigoDisciplina = "159-CNR-4-2",
				Ano = 2012,
				Periodo = 0,
				IdCompetenciaHabilidadeItem = 37612
			};

			var competencia3 = new TOCompetenciaHabilidade_Insere
			{
				Matricula = "08376535",
				CodigoTurma = "CN-4001-180040",
				SubPeriodo = 1,
				CodigoDisciplina = "159-CNR-4-2",
				Ano = 2012,
				Periodo = 0,
				IdCompetenciaHabilidadeItem = 37611
			};

			var listaCompetenciaHabilidade = new List<TOCompetenciaHabilidade_Insere>
			{
				competencia,competencia2,competencia3
			};

			Repositorio.InicializaTransacao();

			bool retorno = Repositorio.Insere(listaCompetenciaHabilidade, false);
			Assert.IsTrue(retorno);

			Repositorio.TransacaoRollback();
		}

		public bool InsereCompetenciaHabilidadeDocente(string matricula, string turma, string disciplina, short ano, short periodo, short subperiodo)
		{
			var competencia = new TOCompetenciaHabilidade_Insere
			{
				Matricula = matricula,
				CodigoTurma = turma,
				SubPeriodo = subperiodo,
				CodigoDisciplina = disciplina,
				Ano = ano,
				Periodo = periodo,
				IdCompetenciaHabilidadeItem = 37605
			};

			return Repositorio.Insere(competencia) == 1;
		}
	}
}
