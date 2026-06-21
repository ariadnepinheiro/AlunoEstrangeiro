using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Repository
{
	public interface IFrequenciaRepository : IRepository<Frequencia>
	{
		Frequencia ObtemFrequenciaPor(string disciplina, string turma, short ano, short periodo, short subperiodo);

		void AtualizaFrequencia(string aulasDadas, string aulasPrevistas, string disciplina, string turma, short ano, short periodo, string frequencia);
        Frequencia ObtemTotalAulasConsolidado(string disciplina, string turma, short ano, short periodo);
	}
}
