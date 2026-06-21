using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using Proderj.DOL.Exception;

namespace Proderj.DOL.Service
{
	public class UnidadeEnsinoService : IUnidadeEnsinoService
	{
		IUnidadeEnsinoRepository repositorioUnidadeEnsino;

		public UnidadeEnsinoService(IUnidadeEnsinoRepository repositorioUnidadeEnsino)
		{
			this.repositorioUnidadeEnsino = repositorioUnidadeEnsino;
		}

		public string ObtemDescricaoPor(string codigoUnidadeEnsino)
		{
			UnidadeEnsino unidadeEnsino = repositorioUnidadeEnsino.ObtemDescricaoPor(codigoUnidadeEnsino);

			if (unidadeEnsino == null)
				throw new UnidadeEnsinoException(UnidadeEnsinoException.TipoEnum.UnidadeEnsinoInexistenteParaCodigoInformado);

			return unidadeEnsino.DescricaoCompleta;
		}

        //TODO: assim que puder, renomear para "ListaMunicipioPor"
        public List<DTOUnidadeEnsino> ListaPor(int codigoRegional)
        {
            List<UnidadeEnsino> listaUnidadesEnsino = repositorioUnidadeEnsino.EnumeraPor(codigoRegional).ToList();

            List<DTOUnidadeEnsino> listaDtoUnidadeEnsino = listaUnidadesEnsino.ConvertAll(unidadeEnsino => new DTOUnidadeEnsino
            {
                CodigoCoordenadoria = unidadeEnsino.Nucleo.Codigo,
                CodigoMunicipio = unidadeEnsino.Municipio.Codigo,
                NomeMunicipio = unidadeEnsino.Municipio.Nome,
                SiglaUF = unidadeEnsino.Municipio.SiglaUF,
                CodigoRegional = unidadeEnsino.Regional.Codigo
            });

            return listaDtoUnidadeEnsino;
        }

        public List<UnidadeEnsino> ListaPor(string codigoMunicipio)
        {
            return repositorioUnidadeEnsino.EnumeraPor(codigoMunicipio).ToList();
        }
	}
}
