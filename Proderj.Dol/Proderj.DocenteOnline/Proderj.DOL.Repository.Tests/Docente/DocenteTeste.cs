using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using log4net;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Framework.Config;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository.Teste
{
    //TODO: TESTE BASE para abrir sessão
    [TestClass]
    public class DocenteTeste : TesteBaseRepositorio<DocenteRepository>
    {
        [TestMethod]
        public void ObtemDocente()
        {
            var resultado = Repositorio.ObtemPorChavePrimaria(1L);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObtemDocentePorMatricula()
        {
            var matricula = "55098784";
            var resultado = Repositorio.ObtemPor(matricula);

            Assert.IsNotNull(resultado);
            Assert.IsNotNull(resultado.Pessoa);
            Assert.IsTrue(resultado.Pessoa.Id == 1L);
            Assert.IsTrue(resultado.SenhaAlterada == "S");
        }

        [TestMethod]
        public void ObtemEmailDeDocentePorMatricula()
        {
            var matricula = "55098784";

            var resultado = Repositorio.ObtemEmailPor(matricula);
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado == "amelegari@prof.educacao.rj.gov.br");
        }

        [TestMethod]
        public void ResetarSenhaDocente()
        {
            var numeroFunc = 100721L;
            var novaSenha = new Random().Next(0, 1000000).ToString();

            var docenteVelho = Repositorio.ObtemPorChavePrimaria(numeroFunc);

            Repositorio.RedefineSenha(novaSenha, docenteVelho.Matricula);

            var docenteNovo = Repositorio.ObtemPorChavePrimaria(numeroFunc);

            Assert.IsTrue(docenteNovo.SenhaDocente == novaSenha);
        }        

		[TestMethod]
		public void ObtemPorPessoaPor()
		{
			string matricula = "08376535";

			Docente docente = Repositorio.ObtemPorPessoaPor(matricula);
			
			Assert.IsNotNull(docente);
		}
    }
}
