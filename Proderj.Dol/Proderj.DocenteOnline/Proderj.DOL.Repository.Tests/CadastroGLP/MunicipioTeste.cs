using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Proderj.DOL.Repository.Teste.CadastroGLP
{
	[TestClass]
	public class MunicipioTeste : TesteBaseRepositorio<MunicipioRepository>
	{
		[TestMethod]
		public void ExistePor()
		{
			string municipio = "00006871";
			bool existe = Repositorio.ExistePor(municipio);
			Assert.IsTrue(existe);

			municipio = "20008";
			existe = Repositorio.ExistePor(municipio);
			Assert.IsFalse(existe);
		}
	}
}
