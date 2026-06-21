using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using log4net;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Framework.Config;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class PessoaTeste : TesteBaseRepositorio<PessoaRepository>
	{
		[TestMethod]
		public void ObtemPessoa()
		{
			var resultado = Repositorio.ObtemPorChavePrimaria(1L);
			Assert.IsNotNull(resultado);
		}

		[TestMethod]
		public void AtualizaTelefone()
		{
			string telefone = "2122556677";
			long pessoaId = 15683L;

			Repositorio.InicializaTransacao();

			var resultado = Repositorio.AtualizaTelefone(telefone, pessoaId);
			Assert.IsTrue(resultado == 1);

			Repositorio.TransacaoRollback();
		}
	}
}
