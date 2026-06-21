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
    //TODO: TESTE BASE para abrir sessão
    [TestClass]
    public class AceiteTermoCompromissoDocenteTeste
    {
        [TestInitialize]
        public void Start()
        {
            AppDomain.CurrentDomain.SetData("ExecutingDir", System.IO.Directory.GetCurrentDirectory());
            ConfiguracaoDeAplicacao.Instance.Config(ContextClassEnum.ThreadStatic);
            FluentHelper.Instance.LoadSession();
        }

        [TestMethod]
        public void ObtemAceiteTermoCompromissoDocente()
        {
            var resultado = new AceiteTermoCompromissoDocenteRepository().ObtemPorChavePrimaria(1);
            Assert.IsNotNull(resultado);
        }
    }
}
