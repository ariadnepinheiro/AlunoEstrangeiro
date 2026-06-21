using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service.Teste
{
	[TestClass]
	public class ProtocoloNotaTeste : TesteBaseService<NinjectModuloServico, ProtocoloNotaService>
	{
		private class Cenario
		{
			public string Matricula { get; set; }
		}
		private List<Cenario> listaDeCenarios = new List<Cenario>();

		public ProtocoloNotaTeste()
		{
			//Cenario OK
			listaDeCenarios.Add(new Cenario
			{
				Matricula = "08376535"
			});
		}

		[TestMethod]
		public void ListaPor()
		{
			var repositorio = new ProtocoloNotaRepository();

			repositorio.InicializaTransacao();

			List<DTOProtocoloNotaComData> protocolos = Servico.ListaPor(listaDeCenarios[0].Matricula);

			repositorio.TransacaoRollback();
		}

	}
}
