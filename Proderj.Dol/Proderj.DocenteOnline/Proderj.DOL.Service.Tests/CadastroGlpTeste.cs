using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service.Teste
{
	[TestClass]
	public class CadastroGlpTeste : TesteBaseService<NinjectModuloServico, CadastroGlpService>
	{
		private class Cenario
		{
			public string Matricula { get; set; }
			public DTOCadastroGlp_InsereDocenteDisponivel DtoSalvamento { get; set; }
		}

		private List<Cenario> listaDeCenariosSalvamento = new List<Cenario>();

		public CadastroGlpTeste()
		{
			listaDeCenariosSalvamento.Add(new Cenario
			{
				Matricula = "08376535",
				DtoSalvamento = new DTOCadastroGlp_InsereDocenteDisponivel
				{
					Agrupamento = "207",
					CodigoMunicipio = "00006871",
					CodigoRegional = 1,
					DiaSemana = 2,
					HoraInicio = DateTime.Now,
					HoraFinal = DateTime.Now.AddHours(0.8),
					NumeroFuncionario = 43879L
				}
			});
		}

		[TestMethod]
		public void ListaDisciplinasPor()
		{
			var retorno = Servico.ListaDisciplinasPor();

			Assert.IsTrue(retorno.Count == 24);
		}

		[TestMethod]
		public void ListaMunicipiosPor()
		{
			var fabricaServico = new NinjectFactoryBase<NinjectModuloServico>();
			UnidadeEnsinoService servicoDeMunicipio = fabricaServico.Obtem<UnidadeEnsinoService>();

			var retorno = servicoDeMunicipio.ListaPor(listaDeCenariosSalvamento.First().DtoSalvamento.CodigoRegional);

			Assert.IsTrue(retorno.Count == 4);
		}

		[TestMethod]
		public void ListaCoordenadorias()
		{
			var fabricaServico = new NinjectFactoryBase<NinjectModuloServico>();
			NucleoService servicoDeNucleo = fabricaServico.Obtem<NucleoService>();

			var retorno = servicoDeNucleo.ListaCoordenadorias();

			Assert.IsTrue(retorno.Count == 31);
		}

		[TestMethod]
		public void ListaDocentesDisponiveisPor()
		{
			var retorno = Servico.ListaDocentesDisponiveisPor(listaDeCenariosSalvamento.First().DtoSalvamento.NumeroFuncionario);

			Assert.IsTrue(retorno.Count == 7);
		}

		[TestMethod]
		public void ObtemDocenteComTelefonePor()
		{
			var retorno = Servico.ObtemDocenteComTelefonePor(listaDeCenariosSalvamento.First().Matricula);

			Assert.IsTrue(retorno.Telefone == "212255-6688");
		}

		[TestMethod]
		public void ObtemDocenteComTelefonePor2()
		{
			var repositorio = new PessoaRepository();

			repositorio.InicializaTransacao();
			
			string telefoneEntrada = "21225566" + new Random().Next(99).ToString().PadLeft(2, '0'); ;
			var telefoneAnterior = Servico.ObtemDocenteComTelefonePor(listaDeCenariosSalvamento.First().Matricula).Telefone;

			var fabricaServico = new NinjectFactoryBase<NinjectModuloServico>();
			PessoaService servicoDePessoa = fabricaServico.Obtem<PessoaService>();
			servicoDePessoa.AtualizaTelefone(telefoneEntrada, 15683);

			var telefonePosterior = Servico.ObtemDocenteComTelefonePor(listaDeCenariosSalvamento.First().Matricula).Telefone;

			Assert.IsTrue(telefoneAnterior != telefonePosterior);
			Assert.IsTrue(telefoneEntrada == telefonePosterior);
			
			repositorio.TransacaoRollback();
		}

		[TestMethod]
		public void InsereDocenteDisponivel()
		{
			var repositorio = new DocenteDisponivelGLPRepository();

			repositorio.InicializaTransacao();

			Servico.InsereDocenteDisponivel(listaDeCenariosSalvamento.First().DtoSalvamento);

			repositorio.TransacaoRollback();
		}

		[TestMethod]
		public void RemoveDocenteDisponivel()
		{
			var repositorio = new DocenteDisponivelGLPRepository();

			repositorio.InicializaTransacao();

			Servico.RemoveDocenteDisponivel(7054, 43879);

			repositorio.TransacaoRollback();
		}

	}
}
