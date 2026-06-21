using System.Collections.Generic;

namespace Proderj.DOL.Service
{
	public interface ITCE_MUNICIPIOService
	{
        IList<string> ListaUF();
        IList<DTOTCE_MUNICIPIO> ListaMunicipioPor(string uf);
        string ObtemUFPor(string municipio);
	}
}
