using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using AutoMapper;

namespace Proderj.DOL.Service
{
	public class HD_PAISService : IHD_PAISService
	{
		private IHD_PAISRepository repositorioPais;

		public HD_PAISService(IHD_PAISRepository repositorioPais)
		{
			this.repositorioPais = repositorioPais;
		}

        public IList<DTOHD_PAIS> Lista()
        {
            List<HD_PAIS> lista = repositorioPais.ListaQueryable().ToList();

            Mapper.CreateMap<HD_PAIS, DTOHD_PAIS>().ForMember(d => d.Pais, opt => opt.MapFrom(s => s.PAIS));
            List<DTOHD_PAIS> listaDto = Mapper.Map<List<DTOHD_PAIS>>(lista);

            return listaDto;
        }
	}

}
