using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface IFaltaRepository : IRepository<Falta>
	{
		IEnumerable<Falta> EnumeraPor(short ano, short periodo, string turma, string disciplina, string frequencia);

		void Atualiza(double faltas, string aluno, string disciplina, string turma, short ano, short periodo, string frequencia);

	}
}
