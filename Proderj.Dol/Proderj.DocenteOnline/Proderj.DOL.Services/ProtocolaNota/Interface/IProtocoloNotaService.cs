using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
	public interface IProtocoloNotaService : IService
	{
        List<DTOProtocoloNotaComData> ListaPor(string idFuncional);

        List<DTOProtocoloNotaComData> ListaPor(string idFuncional, String idVinculo, short ano, short periodo);
	}
}
