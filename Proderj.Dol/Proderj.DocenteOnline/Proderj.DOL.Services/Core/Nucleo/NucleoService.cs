using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using AutoMapper;

namespace Proderj.DOL.Service
{
	public class NucleoService : INucleoService
	{
		private INucleoRepository repositorioNucleo;

		public NucleoService(INucleoRepository repositorioNucleo)
		{
			this.repositorioNucleo = repositorioNucleo;
		}

		public List<DTOCoordenadoria> ListaCoordenadorias()
		{
			IEnumerable<Nucleo> listaNucleos = repositorioNucleo.Enumera();

			List<DTOCoordenadoria> listaCoordenadorias = new List<DTOCoordenadoria>();

			foreach (Nucleo nucleo in listaNucleos)
			{
                //Mapper.CreateMap<Nucleo, DTOCoordenadoria>();
                var dto = Mapper.Map<Nucleo, DTOCoordenadoria>(nucleo);
				listaCoordenadorias.Add(dto);
			}

			return listaCoordenadorias;
		}
	}
}
