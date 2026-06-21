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
	public class CurriculoMinimoTeste : TesteBaseService<NinjectModuloServico, RespostaCurriculoMinimoService>
	{
		private class Cenario
		{
			public DTORespostaCurriculoMinimo_SalvaPor DtoCurriculoMinimoSalvamento { get; set; }
		}
		private List<Cenario> listaDeCenarios = new List<Cenario>();

		public CurriculoMinimoTeste()
		{
			//Cenario OK
			listaDeCenarios.Add(new Cenario
			{
				DtoCurriculoMinimoSalvamento = new DTORespostaCurriculoMinimo_SalvaPor
				{
					CodigoDisciplina = "159-CNR-4-2",
					CodigoTurma = "CN-4001-180040",
					Ano = 2012,
					Periodo = 0,
					Subperiodo = 1,
					Matricula = "08376535",
					IdsCompetenciaHabilidadeItem = new List<int> { 37611, 37612 }
				}
			});
		}

		[TestMethod]
		public void SalvaRespostasCurriculoMinimoPor()
		{
			var rep = new LogCompetenciaHabilidadeDocenteRepository();

			//rep.InicializaTransacao();

			Servico.SalvaPor(listaDeCenarios[0].DtoCurriculoMinimoSalvamento);
			
			//rep.TransacaoRollback();
		}
	}
}