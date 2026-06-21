using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service
{
	public class MunicipioService : IMunicipioService
	{
		private IMunicipioRepository repositorioMunicipio;

		public MunicipioService(IMunicipioRepository repositorioMunicipio)
		{
			this.repositorioMunicipio = repositorioMunicipio;
		}

		public bool ExistePor(string codigoMunicipio)
		{
			return repositorioMunicipio.ExistePor(codigoMunicipio);
		}
	}

}
