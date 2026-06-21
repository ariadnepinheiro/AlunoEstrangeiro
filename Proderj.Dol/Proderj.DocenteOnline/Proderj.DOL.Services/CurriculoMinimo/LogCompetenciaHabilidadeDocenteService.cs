using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Exception;

namespace Proderj.DOL.Service
{
	public class LogCompetenciaHabilidadeDocenteService : ILogCompetenciaHabilidadeDocenteService
	{
		ILogCompetenciaHabilidadeDocenteRepository repositorioLogCompetenciaHabilidadeDocente;

		public LogCompetenciaHabilidadeDocenteService(ILogCompetenciaHabilidadeDocenteRepository repositorioLogCompetenciaHabilidadeDocente)
		{
			this.repositorioLogCompetenciaHabilidadeDocente = repositorioLogCompetenciaHabilidadeDocente;
		}

		public bool InserePorCompetenciaHabilidadeItemPor(DTOLogCompetenciaHabilidade dtoLogCompetenciaHabilidade)
		{
			return repositorioLogCompetenciaHabilidadeDocente.InserePorCompetenciaHabilidadeItemPor(
				dtoLogCompetenciaHabilidade.Matricula, dtoLogCompetenciaHabilidade.CodigoTurma, dtoLogCompetenciaHabilidade.CodigoDisciplina,
				dtoLogCompetenciaHabilidade.Ano, dtoLogCompetenciaHabilidade.Periodo, dtoLogCompetenciaHabilidade.Subperiodo) > 0 ;
		}
	}
}
