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
    public class LoginTeste : TesteBaseRepositorio<LoginRepository>
    {
        [TestMethod]
        public void ObterLogins()
        {
            var resultados = Repositorio.Lista();
            var login = resultados.FirstOrDefault();
            //Assert.IsNotNull(login);
        }
    }
}
