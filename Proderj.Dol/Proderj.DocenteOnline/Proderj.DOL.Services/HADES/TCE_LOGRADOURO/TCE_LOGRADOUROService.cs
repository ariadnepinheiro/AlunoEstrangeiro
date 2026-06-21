using System.Collections.Generic;
using AutoMapper;
using Proderj.DOL.Domain;
using Proderj.DOL.Repository;
using System.Linq;

namespace Proderj.DOL.Service
{
	public class TCE_LOGRADOUROService : ITCE_LOGRADOUROService
	{
		private ITCE_LOGRADOURORepository repositorioLogradouro;

		public TCE_LOGRADOUROService(ITCE_LOGRADOURORepository repositorioLogradouro)
		{
			this.repositorioLogradouro = repositorioLogradouro;

            Mapper.CreateMap<TCE_LOGRADOURO, DTOTCE_LOGRADOURO>();
            Mapper.CreateMap<TCE_MUNICIPIO, DTOTCE_LOGRADOURO.DTOTCE_MUNICIPIO>();
		}

        public IList<DTOTCE_LOGRADOURO> Lista()
        {
            List<TCE_LOGRADOURO> lista = repositorioLogradouro.ListaQueryable().ToList();

            List<DTOTCE_LOGRADOURO> listaDto = Mapper.Map<List<DTOTCE_LOGRADOURO>>(lista);

            return listaDto;
        }

        public DTOTCE_LOGRADOURO ObtemLogradouroPor(string cep)
        {
            TCE_LOGRADOURO obj = repositorioLogradouro.ListaQueryable().FirstOrDefault(q => q.CEP == cep);

            DTOTCE_LOGRADOURO dto = Mapper.Map<DTOTCE_LOGRADOURO>(obj);

            return dto;
        }
	}

}
