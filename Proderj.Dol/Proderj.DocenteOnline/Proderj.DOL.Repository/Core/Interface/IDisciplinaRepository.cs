using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface IDisciplinaRepository : IRepository<Disciplina>
	{
		Disciplina ObtemConceitosPor(string codigoDisciplina);
		Disciplina ObtemDescricaoPor(string codigoDisciplina);
	}
}
