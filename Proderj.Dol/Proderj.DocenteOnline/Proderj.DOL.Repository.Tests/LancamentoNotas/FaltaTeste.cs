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
	public class FaltaTeste : TesteBaseRepositorio<FaltaRepository>
	{
		private List<ChavePrimariaFalta> cenarios;

		private class ChavePrimariaFalta
		{
			public string Disciplina { get; set; }
			public string Turma { get; set; }
			public short Ano { get; set; }
			public short Periodo { get; set; }
			public string Frequencia { get; set; }
			public string Aluno { get; set; }
		}

		[TestInitialize]
		public void Inicializa()
		{
			cenarios = new List<ChavePrimariaFalta>();

			//Cenario 1
			cenarios.Add
			(
				new ChavePrimariaFalta()
				{
					Disciplina = "594-EJA-9-4",
					Turma = "JA-901-180220",
					Ano = 2011,
					Periodo = 2,
					Frequencia = "FB1",
					Aluno = "200602200015135"
				}
			);

		}

		[TestMethod]
		public void Exclui()
		{
			int cenario = 0;

			Exclui(cenario);
		}

		private void Exclui(int cenario)
		{
			Falta falta = Repositorio.EnumeraPor(cenarios[cenario].Ano, cenarios[cenario].Periodo, cenarios[cenario].Turma, cenarios[cenario].Disciplina, cenarios[cenario].Frequencia)
				.Where(x => x.Aluno == cenarios[cenario].Aluno)
					.FirstOrDefault();
			if (falta != null)
				Repositorio.RemoveFalta(cenarios[cenario].Ano, cenarios[cenario].Periodo, cenarios[cenario].Turma, cenarios[cenario].Disciplina, cenarios[cenario].Frequencia, cenarios[cenario].Aluno);
		}

		[TestMethod]
		public void Insere()
		{
			int cenario = 0;
			double faltas = new Random().NextDouble() * 100;

			Exclui(cenario);

			var falta = new Falta
			{
				Aluno = cenarios[cenario].Aluno,
				Disciplina = cenarios[cenario].Disciplina,
				Turma = cenarios[cenario].Turma,
				Ano = cenarios[cenario].Ano,
				Semestre = cenarios[cenario].Periodo,
				Frequencia = cenarios[cenario].Frequencia,
				QuantFaltas = faltas
			};

			Repositorio.Inclui(falta);
		}

		[TestMethod]
		public void Atualiza()
		{
			double faltas = new Random().NextDouble() * 100;
			int cenario = 0;

			Repositorio.Atualiza(faltas, cenarios[cenario].Aluno, cenarios[cenario].Disciplina, cenarios[cenario].Turma, cenarios[cenario].Ano, cenarios[cenario].Periodo, cenarios[cenario].Frequencia);
		}
	}
}
