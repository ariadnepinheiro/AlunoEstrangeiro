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
	public class NotaTeste : TesteBaseRepositorio<NotaRepository>
	{
		private List<ChavePrimariaNota> cenarios;

		private class ChavePrimariaNota
		{
			public string Disciplina { get; set; }
			public string Turma { get; set; }
			public short Ano { get; set; }
			public short Periodo { get; set; }
			public string Prova { get; set; }
		}

		[TestInitialize]
		public void Inicializa()
		{
			cenarios = new List<ChavePrimariaNota>();

			//Cenario 1
			cenarios.Add
			(
				new ChavePrimariaNota()
				{
					Disciplina = "000-ADMI-1-1",
					Turma = "MI-ADM-1001-187000",
					Ano = 2012,
					Periodo = 0,
					Prova = "MEDIA1"
				}
			);

			//Cenario 2
			cenarios.Add
			(
				new ChavePrimariaNota()
				{
					Disciplina = "000-ADMI-1-1",
					Turma = "MI-ADM-1002-181407",
					Ano = 2011,
					Periodo = 0,
					Prova = "MEDIA2"
				}
			);

			//Cenario 3
			cenarios.Add
			(
				new ChavePrimariaNota()
				{
					Disciplina = "000-ADMI-1-1",
					Turma = "MI-ADM-1001-187000",
					Ano = 2012,
					Periodo = 0,
					Prova = "MEDIA2"
				}
			);
		}

		[TestMethod]
		public void EnumeraPor()
		{
			var resultado = Repositorio.EnumeraPor(cenarios[0].Ano, cenarios[0].Periodo, cenarios[0].Turma, cenarios[0].Disciplina, cenarios[0].Prova);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado.Count() == 40);

			resultado = Repositorio.EnumeraPor(cenarios[1].Ano, cenarios[1].Periodo, cenarios[1].Turma, cenarios[1].Disciplina, cenarios[1].Prova);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado.Count() == 4);
		}

		[TestMethod]
		public void TesteAtualiza()
		{
			int pk = 4146689;

			var notaAnterior = Repositorio.ObtemPorChavePrimaria(pk);

			short? ordemAnterior = notaAnterior.Ordem;
			notaAnterior.Ordem = new Random().Next(100).To<short>();

			Repositorio.AtualizaNota(notaAnterior);

			var notaPosterior = Repositorio.ObtemPorChavePrimaria(pk);
			short? ordemPosterior = notaPosterior.Ordem;

			Assert.IsFalse(ordemAnterior == ordemPosterior);

			notaPosterior.Ordem = ordemAnterior;

			Repositorio.AtualizaNota(notaPosterior);

			Assert.IsTrue(ordemAnterior == notaPosterior.Ordem);
		}

		[TestMethod]
		public void Insere()
		{
			Exclui();

			int pk = 4146689;

			var notaAnterior = Repositorio.ObtemPorChavePrimaria(pk);

			var notaNova = new Nota();

			notaNova.Aluno = notaAnterior.Aluno;
			notaNova.Ano = notaAnterior.Ano;
			notaNova.Disciplina = notaAnterior.Disciplina;
			notaNova.Turma = notaAnterior.Turma;
			notaNova.Semestre = notaAnterior.Semestre;
			notaNova.TipoProva = "MEDIA2";
			notaNova.Ordem = 10;

			Repositorio.Inclui(notaNova);
		}

		[TestMethod]
		public void Exclui()
		{
			var nota = Repositorio.EnumeraPor(cenarios[2].Ano, cenarios[2].Periodo, cenarios[2].Turma, cenarios[2].Disciplina, cenarios[2].Prova)
				.Where(x => x.Aluno == "200917970111303").FirstOrDefault();

			if (nota != null)
				Repositorio.RemoveNota(cenarios[2].Ano, cenarios[2].Periodo, cenarios[2].Turma, cenarios[2].Disciplina, cenarios[2].Prova, nota.Aluno);

		}
	}
}
