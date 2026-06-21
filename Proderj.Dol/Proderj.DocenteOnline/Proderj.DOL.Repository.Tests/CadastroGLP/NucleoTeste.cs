using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.Foundation.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository.Teste
{
	[TestClass]
	public class NucleoTeste : TesteBaseRepositorio<NucleoRepository>
	{
		[TestMethod]
		public void Enumera()
		{
			IEnumerable<Nucleo> nucleos = Repositorio.Enumera();

			Assert.IsTrue(nucleos.Count() == 31);

			int i = 1;
			nucleos.ExecuteForEach(x=>
				{
					Assert.IsTrue(x.Codigo == i);
					i++;
				}
			);
			
		}
	}
}
