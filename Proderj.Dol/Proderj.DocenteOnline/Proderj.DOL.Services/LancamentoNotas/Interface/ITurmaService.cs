using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
	public interface ITurmaService : IService
	{
		DTOProvaTurma ObtemProvaDaTurmaPor(string disciplina, string turma, short ano, short periodo, short subperiodo);
		DTOFrequenciaTurma ObtemFrequenciaDaTurmaPor(string disciplina, string turma, short ano, short periodo, short subperiodo);
		DTOProvaParaLancamento ObtemProvaDaTurmaParaLancamentoPor(string disciplina, string turma, short ano, short periodo, short subperiodo);
	}
}
