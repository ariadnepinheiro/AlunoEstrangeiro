using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
	public interface IUnidadeEnsinoService
	{
		string ObtemDescricaoPor(string codigoUnidadeEnsino);

		//List<DTOUnidadeEnsino> ListaPor(int codigoCoordenadoria);

        List<DTOUnidadeEnsino> ListaPor(int codigoRegional);

        List<UnidadeEnsino> ListaPor(string codigoMunicipio);
	}
}
