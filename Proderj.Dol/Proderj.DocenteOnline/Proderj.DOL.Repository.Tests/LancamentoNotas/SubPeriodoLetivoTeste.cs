using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using log4net;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Proderj.Foundation.Framework.Config;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class SubPeriodoLetivoTeste : TesteBaseRepositorio<SubPeriodoLetivoRepository>
	{
		[TestMethod]
		public void ObtemSubPeriodoAtualPorAnoPeriodo()
		{
			short ano = 2012;
			short periodo = 0;

            var resultado = Repositorio.ObtemAtualParaLancamentoDeNotasPor(ano, periodo);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado == 3);

            periodo = 12;
            resultado = Repositorio.ObtemAtualParaLancamentoDeNotasPor(ano, periodo);

			Assert.IsNull(resultado);
		}

		[TestMethod]
		public void EnumeraSubPeriodosLetivosPorAnoPeriodo()
		{
			short ano = 2012;
			short periodo = 0;

			var resultado = Repositorio.EnumeraPor(ano, periodo);

			Assert.IsTrue(resultado.Any());

			ano = 2012;
			periodo = 10;

			resultado = Repositorio.EnumeraPor(ano, periodo);

			Assert.IsTrue(!resultado.Any());
		}

		[TestMethod]
		public void ConfereSubPeriodoAnterior()
		{
			short ano = 2012;
			short periodo = 0;
			short subPeriodo = 0;

			var resultado = Repositorio.EhAntigoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);

			Assert.IsFalse(resultado);

			ano = 2012;
			periodo = 0;
			subPeriodo = 1;

			resultado = Repositorio.EhAntigoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);

			Assert.IsTrue(resultado);

			ano = 2012;
			periodo = 0;
			subPeriodo = 2;

			resultado = Repositorio.EhAntigoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);

			Assert.IsTrue(resultado);

			ano = 2012;
			periodo = 0;
			subPeriodo = 3;

			resultado = Repositorio.EhAntigoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);

			Assert.IsFalse(resultado);

			ano = 2012;
			periodo = 0;
			subPeriodo = 4;

			resultado = Repositorio.EhAntigoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);

			Assert.IsFalse(resultado);
		}

		[TestMethod]
		public void ConfereSubPeriodoAtivo()
		{
			short ano = 2012;
			short periodo = 0;
			short subPeriodo = 0;

			var resultado = Repositorio.EhAtivoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);

			Assert.IsFalse(resultado);

			ano = 2012;
			periodo = 0;
			subPeriodo = 1;

			resultado = Repositorio.EhAtivoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);

			Assert.IsFalse(resultado);

			ano = 2012;
			periodo = 0;
			subPeriodo = 2;

			resultado = Repositorio.EhAtivoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);

			Assert.IsFalse(resultado);

			ano = 2012;
			periodo = 0;
			subPeriodo = 3;

			resultado = Repositorio.EhAtivoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);

			Assert.IsTrue(resultado);

			ano = 2012;
			periodo = 0;
			subPeriodo = 4;

			resultado = Repositorio.EhAtivoParaLancamentoDeNotasPor(ano, periodo, subPeriodo);

			Assert.IsTrue(resultado);
		}

		[TestMethod]
		public void EnumeraAtivosParaLancamentoDeCurriculoMinimoPor()
		{
			short ano = 2012;
			short periodo = 0;

			var resultado = Repositorio.EnumeraAtivosParaLancamentoDeCurriculoMinimoPor(ano, periodo);

			Assert.IsTrue(resultado.Count() > 0);

		}

		[TestMethod]
		public void EnumeraParaAvaliacaoPor()
		{
			short ano = 2012;
			short periodo = 0;

			IEnumerable<SubPeriodoLetivo> resultado = Repositorio.EnumeraParaAvaliacaoPor(ano, periodo, true);
			Assert.IsTrue(resultado.Count() == 5);
						
			resultado = Repositorio.EnumeraParaAvaliacaoPor(ano, periodo);
			Assert.IsTrue(resultado.Count() == 4);

		}

		[TestMethod]
		public void ObtemAtualParaCurriculoMinimoPor()
		{
			short ano = 2012;
			short periodo = 0;
			short? resultado = Repositorio.ObtemAtualParaCurriculoMinimoPor(ano, periodo);

			Assert.IsNotNull(resultado);
			Assert.IsTrue(resultado == 1);

		}

		[TestMethod]
		public void EhAtivoParaLancamentoDeCurriculoMinimo()
		{
			short ano = 2012;
			short periodo = 0;
			short subperiodo = 1;

			bool resultado = Repositorio.EhAtivoParaLancamentoDeRespostaDeCurriculoMinimoPor(ano, periodo, subperiodo);

			Assert.IsTrue(resultado);

		}
	}
}
