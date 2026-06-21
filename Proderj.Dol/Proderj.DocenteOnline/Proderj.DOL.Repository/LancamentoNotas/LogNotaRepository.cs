using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{	
	public class LogNotaRepository : NHRepositoryBase<LogNota>, ILogNotaRepository
	{
		public void InsereLogNota(LogNota logNota)
		{
			IncluiAuditada(logNota);
		}
	}
}
