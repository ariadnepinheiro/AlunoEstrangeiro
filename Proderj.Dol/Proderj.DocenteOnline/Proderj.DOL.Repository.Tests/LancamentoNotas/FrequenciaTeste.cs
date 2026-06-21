using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class FrequenciaTeste : TesteBaseRepositorio<FrequenciaRepository>
	{
		[TestMethod]
		public void ObtemFrequenciaPor()
		{
			var disciplina = "159-EJA-1-36";
			var turma = "CEJAS-MAT36-181578";
			short ano = 2012;
			short periodo = 0;
			short subperiodo = 1;

			var resultado = Repositorio.ObtemFrequenciaPor(disciplina, turma, ano, periodo, subperiodo);

			Assert.IsNotNull(resultado);
			Assert.IsNotNull(resultado.TipoFrequencia = "FB1");
		}

		[TestMethod]
		public void AtualizaFrequenciaPor()
		{
			var disciplina = "159-EJA-1-36";
			var turma = "CEJAS-MAT36-181578";
			short ano = 2012;
			short periodo = 0;
			short subperiodo = 1;

			var resultado = Repositorio.ObtemFrequenciaPor(disciplina, turma, ano, periodo, subperiodo);

			Assert.IsNotNull(resultado);
			Assert.IsNotNull(resultado.TipoFrequencia = "FB1");
		}
	}
}
