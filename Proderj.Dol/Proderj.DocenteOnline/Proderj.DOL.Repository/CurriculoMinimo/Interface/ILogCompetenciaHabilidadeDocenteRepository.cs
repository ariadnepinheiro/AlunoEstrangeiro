using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface ILogCompetenciaHabilidadeDocenteRepository : IRepository<LogCompetenciaHabilidadeDocente>
	{
		int InserePorCompetenciaHabilidadeItemPor(string matricula, string turma, string disciplina, short ano, short periodo, short subperiodo);
	}
}
