using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using AutoMapper;

namespace Proderj.DOL.Service
{
	public class RegionalService : IRegionalService
	{
		private IRegionalRepository repositorioRegional;

		public RegionalService(IRegionalRepository repositorioRegional)
		{
			this.repositorioRegional = repositorioRegional;
		}

		public List<DTORegional> ListaRegionais()
		{
			IEnumerable<Regional> listaRegional = repositorioRegional.Enumera();

			List<DTORegional> listaRegionais = new List<DTORegional>();

			foreach (Regional regional in listaRegional)
			{
				Mapper.CreateMap<Regional, DTORegional>();
				listaRegionais.Add(Mapper.Map<Regional, DTORegional>(regional));
			}

			return listaRegionais;
		}
	}
}
