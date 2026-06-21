using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface INotaRepository : IRepository<Nota>
	{
		IEnumerable<Nota> EnumeraPor(short ano, short periodo, string turma, string disciplina, string prova);

		void RemoveNota(short ano, short periodo, string turma, string disciplina, string prova, string aluno);

		void AtualizaNota(Nota nota);

	}
}
