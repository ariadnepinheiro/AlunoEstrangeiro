using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Architecture;

namespace Proderj.DOL.Service
{
	public interface ILogCompetenciaHabilidadeDocenteService : IService
	{
		bool InserePorCompetenciaHabilidadeItemPor(DTOLogCompetenciaHabilidade dtoLogCompetenciaHabilidade);
	}
}
