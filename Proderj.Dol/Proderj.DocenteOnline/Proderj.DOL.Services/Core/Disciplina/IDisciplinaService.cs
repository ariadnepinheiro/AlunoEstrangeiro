using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
	public interface IDisciplinaService
	{
		DTOConfiguracaoNotaDisciplina ObtemConfiguracaoNotaPor(string codigoDisciplina);
		string ObtemDescricaoDisciplinaPor(string codigoDisciplina);
        Disciplina ObtemDisciplina(string codigoDisciplina);
	}
}
