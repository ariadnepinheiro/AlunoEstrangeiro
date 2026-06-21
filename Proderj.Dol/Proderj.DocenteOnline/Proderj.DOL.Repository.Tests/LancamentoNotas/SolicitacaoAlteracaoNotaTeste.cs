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
	public class SolicitacaoAlteracaoNotaTeste : TesteBaseRepositorio<SolicitacaoAlteracaoNotasRepository>
	{
		public List<SolicitacaoAlteracaoNotas> Solicitacao { get; set; }

		[TestInitialize]
		public void InicializaSolicitacao()
		{
			var data = new DateTime(2011, 07, 25);
			Solicitacao = new List<SolicitacaoAlteracaoNotas>();

			//Cenário 1
			Solicitacao.Add
			(
				new SolicitacaoAlteracaoNotas
				{
					NumeroFuncionario = 85145L,
					Status = "Aprovado",
					Turma = "3002-181869",
					Disciplina = "728-EMR-3-4",
					Ano = 2011,
					SubPeriodo = 1,
					UnidadeEnsino = "33106690",
					DataStatus = data,
					DataLimite = data
				}
			);

			//Cenário 2
			Solicitacao.Add
			(
				new SolicitacaoAlteracaoNotas
				{
					NumeroFuncionario = 85145L,
					Status = "Aprovado",
					Turma = "3002-181869",
					Disciplina = "728-EMR-3-4",
					Ano = 2011,
					SubPeriodo = 10,
					UnidadeEnsino = "33106690",
					DataStatus = data,
					DataLimite = data
				}
			);
		}

		[TestMethod]
		public void ConferePeriodoAlteracaoNotaValido()
		{
			var resultado = Repositorio.ExisteSolicitacaoAlteracaoNotasValido(Solicitacao[0]);
			Assert.IsFalse(resultado);

			resultado = Repositorio.ExisteSolicitacaoAlteracaoNotasValido(Solicitacao[1]);
			Assert.IsFalse(resultado);
		}

		[TestMethod]
		public void ConfereSolicitacaoAlteracaoNota()
		{
			var resultado = Repositorio.ExisteSolicitacaoAlteracaoNotas(Solicitacao[0]);
			Assert.IsTrue(resultado);

			resultado = Repositorio.ExisteSolicitacaoAlteracaoNotasValido(Solicitacao[1]);
			Assert.IsFalse(resultado);
		}

		[TestMethod]
		public void ObtemDataPor()
		{
			var resultado = Repositorio.ObtemDataPor(Solicitacao[0]);
			Assert.IsNotNull(resultado);

			resultado = Repositorio.ObtemDataPor(Solicitacao[1]);
			Assert.IsNull(resultado);
		}

		[TestMethod]
		public void InsereTeste()
		{
			//Solicitacao[0].Id = 148647;
			Solicitacao[0].Justificativa = "Justificativa";
			Solicitacao[0].DataSolicitacao = DateTime.Now;

			//Repositorio.InicializaTransacao();
			Repositorio.Inclui(Solicitacao[0]);
			//Repositorio.FinalizaTransacao();
		}
	}
}
