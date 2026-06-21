using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface IProvaRepository : IRepository<Prova>
	{
		Prova ObtemPor(string disciplina, string turma, short ano, short periodo, short subperiodo);
	}
}
