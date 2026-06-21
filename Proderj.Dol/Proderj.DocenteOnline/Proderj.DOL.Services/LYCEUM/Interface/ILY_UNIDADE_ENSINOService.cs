using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
	public interface ILY_UNIDADE_ENSINOService
	{
        dynamic ObtemRegionalEMunicipioPor(string unidadeEnsino);
        IDictionary<string, string> DicionarioPor(int? id_regional, string municipio);
        IList<dynamic> ListaPor(int? id_regional, string municipio);
        IList<string> ListaCodigoPor(int? id_regional, string municipio);
        IList<DTOListaMUNICIPIOPorID_REGIONAL> ListaMunicipioPor(int id_regional);
	}
}
