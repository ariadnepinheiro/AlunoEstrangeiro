using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proderj.DOL.Service.Teste
{
	[TestClass]
	public class AvaliacaoCurriculoMinimoTeste : TesteBaseService<NinjectModuloServico, AvaliacaoCurriculoMinimoService>
	{
		private class CenarioCarregamento
		{
			public DTOAvaliacaoCurriculoMinimo_EnumeraAvaliacoesEJustificativaPor dtoCarregamento { get; set; }
		}

		private class CenarioSalvamento
		{
			public DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor dtoSalvamento { get; set; }
		}


		private List<CenarioCarregamento> listaDeCenariosCarregamento = new List<CenarioCarregamento>();
		private List<CenarioSalvamento> listaDeCenariosSalvamento = new List<CenarioSalvamento>();

		public AvaliacaoCurriculoMinimoTeste()
		{
			short ano = 2012;
			short periodo = 0;
			short subperiodo = 1;
			string matricula = "08376535";

			var avaliacao1 = new DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao
			{
				IdAvaliacaoCurriculoMinimo = 35,
				EhAvaliadoPositivamente = true,
			};

			var avaliacao2 = new DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao
			{
				IdAvaliacaoCurriculoMinimo = 36,
				EhAvaliadoPositivamente = true,
			};

			//Cenario 1 - OK
			listaDeCenariosCarregamento.Add(new CenarioCarregamento
			{
				dtoCarregamento = new DTOAvaliacaoCurriculoMinimo_EnumeraAvaliacoesEJustificativaPor
				{
					Ano = ano,
					Periodo = periodo,
					SubPeriodo = subperiodo,
					Matricula = matricula
				}
			});

			listaDeCenariosSalvamento.Add(new CenarioSalvamento
			{
				dtoSalvamento = new DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor
				{
					Ano = ano,
					Periodo = periodo,
					SubPeriodo = subperiodo,
					Matricula = matricula,
					AvaliacoesCurriculoMinimo = new List<DTOAvaliacaoCurriculoMinimo_RespostaAvaliacao> { avaliacao1, avaliacao2 },
					DescricaoJustificativa = "Justificativa do Teste Unitário!",
				}
			});
		}

		[TestMethod]
		public void EnumeraAvaliacoesEJustificativaPor()
		{
			var retorno = Servico.ObtemAvaliacoesEJustificativaPor(listaDeCenariosCarregamento[0].dtoCarregamento.Ano, listaDeCenariosCarregamento[0].dtoCarregamento.Periodo, listaDeCenariosCarregamento[0].dtoCarregamento.SubPeriodo, listaDeCenariosCarregamento[0].dtoCarregamento.Matricula);

			Assert.IsTrue(retorno.ListaAvaliacaoCurriculoMinimo.Count == 5);
			Assert.IsTrue(!retorno.DescricaoJustificativa.IsNullOrEmpty());
		}

		[TestMethod]
		public void SalvaAvaliacoesEJustificativaPor()
		{
			Servico.SalvaAvaliacoesEJustificativaPor(listaDeCenariosSalvamento[0].dtoSalvamento);
		}
	}
}
