using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface ILogNotaRepository : IRepository<LogNota>
	{
		void InsereLogNota(LogNota logNota);
	}
}
