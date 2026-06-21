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
    public class TermoCompromissoDocenteTeste : TesteBaseRepositorio<TermoCompromissoDocenteRepository>
    {
        [TestMethod]
        public void ObtemTermoCompromissoDocente()
        {
            var resultado = Repositorio.ObtemPorChavePrimaria(1);
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void ObtemMenorTermoDeCompromissoSemAceite()
        {
            var matricula = "55098784";
            var resultado = Repositorio.ObtemTermoNaoAceitoMaisRecentePor(matricula);

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Ano == 2012);
            Assert.IsNotNull(resultado.Arquivo == "docente2012.htm");
        }
    }
}
