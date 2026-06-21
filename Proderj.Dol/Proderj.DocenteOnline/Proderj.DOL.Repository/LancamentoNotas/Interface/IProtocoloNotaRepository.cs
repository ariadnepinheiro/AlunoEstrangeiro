using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface IProtocoloNotaRepository : IRepository<Protocolo>
	{
		IEnumerable<Protocolo> EnumeraPor(string idFuncional);

        IEnumerable<Protocolo> EnumeraPor(string idFuncional, string idVinculo, short ano, short periodo);

	}
}
