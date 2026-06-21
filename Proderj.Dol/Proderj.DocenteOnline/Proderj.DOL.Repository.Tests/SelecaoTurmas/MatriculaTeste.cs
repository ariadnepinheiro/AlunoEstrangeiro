using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.Foundation.Framework.Config;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository.Teste
{
    [TestClass]
    public class MatriculaTeste : TesteBaseRepositorio<MatriculaRepository>
    {
        [TestMethod]
        public void ObterMatriculas()
        {
            var matriculaAluno = "2000000000000001";

            var resultados = Repositorio.EnumeraMatriculaPor(matriculaAluno);

            Assert.IsTrue(resultados.Any());

            Logger.Log("ObterMatriculas com sucesso", LogLevel.Debug);
        }

        [TestMethod]
        public void ListarSelecaoTurmas()
        {
            var matriculaAluno = "2000000000000001";

            var resultados = Repositorio.EnumeraMatriculaPor(matriculaAluno);

            Assert.IsTrue(resultados.Any());

            Logger.Log("ListarSelecaoTurmas com sucesso", LogLevel.Debug);
        }

        [TestMethod]
        public void TestaHibernateProjection()
        {
            var resultados = Repositorio.ListaMatriculas();

            Assert.IsTrue(resultados.Any());

            Logger.Log("TestaHibernateProjection", LogLevel.Debug);
        }
    }
}
