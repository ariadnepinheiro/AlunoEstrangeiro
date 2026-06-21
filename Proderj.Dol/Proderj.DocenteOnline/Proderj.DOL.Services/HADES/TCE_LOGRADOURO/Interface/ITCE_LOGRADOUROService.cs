using System.Collections.Generic;

namespace Proderj.DOL.Service
{
	public interface ITCE_LOGRADOUROService
	{
        IList<DTOTCE_LOGRADOURO> Lista();
        DTOTCE_LOGRADOURO ObtemLogradouroPor(string cep);
	}
}
