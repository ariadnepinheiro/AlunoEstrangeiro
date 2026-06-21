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
    public class UltimoResetTeste : TesteBaseRepositorio<UltimoResetRepository>
    {
        [TestMethod]
        public void ObtemUltimoReset()
        {
            var resultado = Repositorio.ObtemPorChavePrimaria("00019331");
            Assert.IsNotNull(resultado);
        }
    }
}
