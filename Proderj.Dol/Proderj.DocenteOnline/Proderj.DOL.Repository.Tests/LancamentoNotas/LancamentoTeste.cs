using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class LancamentoTeste : TesteBaseRepositorio<LancamentoNotasRepository>
	{
		[TestMethod]
		public void EnumeraLancamentosPorParametros()
		{
			var disciplina = "159-EJA-1-36";
			var turma = "CEJAS-MAT36-181578";
			short ano = 2012;
			short periodo = 0;
			short subperiodo = 1;

			var resultado = Repositorio.EnumeraLancamentosPor(disciplina, turma, ano, periodo, subperiodo);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado.Count() == 100);
			//resultado.ExecuteForEach(x => { Assert.IsTrue(!x.RecuperacaoPararela 
			//        && !x.SemAvaliacao 
			//        && (x.Faltas == default(short?))
			//        && (x.MediaNota == default(double?))); });

		}

		[TestMethod]
		public void ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPorTeste_Existente()
		{
			short ano = 2012;
			short semestre = 0;
			short subperiodoAtual = 2;
			string disciplina = "ED_FISICA_7_2";
			string turma = "702-180251";

			bool existe = Repositorio.ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPor(ano, semestre, subperiodoAtual, disciplina, turma);
			Assert.IsTrue(existe);
		}

		[TestMethod]
		public void ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPorTeste_Inexistente()
		{
			short ano = 2012;
			short semestre = 0;
			short subperiodoAtual = 2;
			string disciplina = "159-CNR-3-4";
			string turma = "CN-3002-180040";

			bool existe = Repositorio.ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPor(ano, semestre, subperiodoAtual, disciplina, turma);
			Assert.IsFalse(existe);
		}
	}
}
