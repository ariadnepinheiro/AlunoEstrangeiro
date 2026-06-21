using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
	public interface ILancamentoNotasRepository : IRepository<LancamentoNotas>
	{
		IEnumerable<LancamentoNotas> EnumeraLancamentosPor(string disciplina, string turma, short ano, short periodo, short subperiodo);
		bool ExisteNotaPendenteParaLancamentoEmBimestreAnteriorAoAtualPor(short ano, short periodo, short subperiodoAtual, string disciplina, string turma);
		bool AtualizaFlagLancamentoCompletoPor(short ano, short periodo, string turma, string disciplina, string prova);
	}
}
