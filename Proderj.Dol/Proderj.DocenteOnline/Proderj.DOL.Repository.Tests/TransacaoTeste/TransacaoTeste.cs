using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service.Teste
{
	[TestClass]
	public class TransacaoTeste : TesteBaseRepositorio<ProtocoloNotaRepository>
	{
		[TestMethod]
		public void TestaRepositorio()
		{
			Repositorio.InicializaTransacao();

			Protocolo protocolo = new Protocolo
			{
				Disciplina = "159-CNR-3-4",
				NomeDisciplina = "Matemática",
				Turma = "CN-3002-180040",
				Ano = 2012,
				Periodo = 0,
				SubPeriodo = 0,
                IdFuncional = "leoenes1",
				Tipo = "T",
			};

			Repositorio.Inclui(protocolo);

			protocolo = new Protocolo
			{
				Disciplina = "159-CNR-3-4",
				NomeDisciplina = "Matemática",
				Turma = "CN-3002-180040",
				Ano = 2012,
				Periodo = 0,
				SubPeriodo = 0,
                IdFuncional = "leoenes2",
				Tipo = "T",
			};

			Repositorio.Inclui(protocolo);

			protocolo = new Protocolo
			{
				Disciplina = "159-CNR-3-4",
				NomeDisciplina = "Matemática",
				Turma = "CN-3002-180040",
				Ano = 2012,
				Periodo = 0,
				SubPeriodo = 0,
                IdFuncional = "leoenes3",
				Tipo = "T",
			};

			Repositorio.Inclui(protocolo);

			protocolo = new Protocolo
			{
				Disciplina = "159-CNR-3-4",
				NomeDisciplina = "Matemática",
				Turma = "CN-3002-180040",
				Ano = 2012,
				Periodo = 0,
				SubPeriodo = 0,
                IdFuncional = "leoenes4",
				Tipo = "T",
			};

			Repositorio.Inclui(protocolo);

			Repositorio.FinalizaTransacao();

			Repositorio.InicializaTransacao();

			protocolo = new Protocolo
			{
				Disciplina = "159-CNR-3-4",
				NomeDisciplina = "Matemática",
				Turma = "CN-3002-180040",
				Ano = 2012,
				Periodo = 0,
				SubPeriodo = 0,
                IdFuncional = "leoenes5",
				Tipo = "T",
			};

			Repositorio.Inclui(protocolo);

			Repositorio.TransacaoRollback();

			Repositorio.InicializaTransacao();

			protocolo = new Protocolo
			{
				Disciplina = "159-CNR-3-4",
				NomeDisciplina = "Matemática",
				Turma = "CN-3002-180040",
				Ano = 2012,
				Periodo = 0,
				SubPeriodo = 0,
                IdFuncional = "leoenes6",
				Tipo = "T",
			};

			Repositorio.Inclui(protocolo);

			Repositorio.TransacaoRollback();
		}

		[TestMethod]
		public void TestaRepositorio2()
		{
			Repositorio.InicializaTransacao();

			var lancamento = new LancamentoNotasRepository();

			short ano = 2012;
			short periodo = 0;
			string prova = "MEDIA3";
			string turma = "CN-3002-180040";
			string disciplina = "159-CNR-3-4";
			
			lancamento.AtualizaFlagLancamentoCompletoPor(ano, periodo, turma, disciplina, prova);

			Repositorio.TransacaoRollback();
		}
	}
}
