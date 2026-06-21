using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.DOL.Service;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Proderj.Foundation.Framework.Config;

namespace Proderj.DOL.Service.Teste
{
    [TestClass]
	public class SelecaoTurmasTeste : TesteBaseService<NinjectModuloServico, SelecaoTurmasService>
    {
        [TestMethod]
        public void EnumeraSelecaoPorNumFunc()
        {
			const string numeroFuncionario = "08376535";

            IEnumerable<DTOSelecaoTurmas> turmas = Servico.EnumeraSelecaoTurmasPor(numeroFuncionario);

            Assert.IsTrue(turmas.Count() == 3);
        }
    }
}
