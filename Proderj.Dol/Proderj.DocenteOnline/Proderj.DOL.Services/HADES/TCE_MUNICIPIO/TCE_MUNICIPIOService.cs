using System.Collections.Generic;
using AutoMapper;
using Proderj.DOL.Domain;
using Proderj.DOL.Repository;
using System.Linq;
using System.Collections;

namespace Proderj.DOL.Service
{
	public class TCE_MUNICIPIOService : ITCE_MUNICIPIOService
	{
		private ITCE_MUNICIPIORepository repositorioMunicipio;

		public TCE_MUNICIPIOService(ITCE_MUNICIPIORepository repositorioMunicipio)
		{
			this.repositorioMunicipio = repositorioMunicipio;
		}

        public IList<string> ListaUF()
        {
            return repositorioMunicipio.ListaQueryable()
                .Select(s => s.UF)
                .Distinct()
                .ToList();
        }

        public string ObtemUFPor(string municipio)
        {
            var obj = repositorioMunicipio.ObtemPorChavePrimaria(municipio);

            return obj != null ? obj.UF : null;
        }

        public IList<DTOTCE_MUNICIPIO> ListaMunicipioPor(string uf)
        {
            var lista = repositorioMunicipio.ListaQueryable()
                .Where(q => q.UF == uf)
                .ToList();
            
            Mapper.CreateMap<TCE_MUNICIPIO, DTOTCE_MUNICIPIO>()
                .ForMember(d => d.Logradouros, opts => opts.Ignore());

            var listaDto = Mapper.Map<IList<DTOTCE_MUNICIPIO>>(lista);

            return listaDto;
        }
	}

}
