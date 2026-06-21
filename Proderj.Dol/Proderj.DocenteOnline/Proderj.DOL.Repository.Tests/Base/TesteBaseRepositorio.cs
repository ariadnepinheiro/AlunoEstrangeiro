using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework.Config;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository.Teste
{
    [TestClass]
    public class TesteBaseRepositorio<T> where T: IRepository, new()
    {
        public static T Repositorio { get; set; }

        static TesteBaseRepositorio()
        {
            AppDomain.CurrentDomain.SetData("ExecutingDir", System.IO.Directory.GetCurrentDirectory());
            ConfiguracaoDeAplicacao.Instance.Config();
            FluentHelper.Instance.LoadSession();

            Repositorio = new T();
        }
    }
}
