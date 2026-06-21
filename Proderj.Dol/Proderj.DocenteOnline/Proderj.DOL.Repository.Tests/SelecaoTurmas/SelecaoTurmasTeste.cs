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
    public class SelecaoTurmasTeste : TesteBaseRepositorio<SelecaoTurmasRepository>
    {
        [TestMethod]
        public void EnumeraTurmasPorNumeroFuncionario()
        {
            var numeroFuncionario = 43879L;
            var resultado = Repositorio.EnumeraTurmasPor(numeroFuncionario);

            Assert.IsNotNull(resultado);
        }       
    }
}


