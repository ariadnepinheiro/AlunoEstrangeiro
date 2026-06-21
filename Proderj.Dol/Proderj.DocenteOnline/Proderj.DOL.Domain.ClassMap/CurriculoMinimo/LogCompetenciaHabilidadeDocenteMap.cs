using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class LogCompetenciaHabilidadeDocenteMap : ClassMap<LogCompetenciaHabilidadeDocente>
	{
		public LogCompetenciaHabilidadeDocenteMap()
		{
			Table("VW_TCE_LOG_COMPETENCIA_HABILIDADE_DOCENTE");

			LazyLoad();
			
			Id(x => x.Id)
				.Column("ID_LOG_COMPETENCIA_HABILIDADE_DOCENTE");
			
			Map(x => x.IdCompetenciaHabilidadeItem)
				.Column("ID_COMPETENCIA_HABILIDADE_ITEM")
					.Not.Nullable();
			
			Map(x => x.Disciplina)
				.Column("DISCIPLINA")
					.Not.Nullable()
						.Length(20);
			
			Map(x => x.Turma)
				.Column("TURMA")
					.Not.Nullable()
						.Length(20);
			
			Map(x => x.Ano)
				.Column("ANO")
					.Not.Nullable();
			
			Map(x => x.Periodo)
				.Column("PERIODO")
					.Not.Nullable();
			
			Map(x => x.SubPeriodo)
				.Column("SUBPERIODO")
					.Not.Nullable();
			
			Map(x => x.Matricula)
				.Column("MATRICULA")
					.Not.Nullable()
						.Length(8);
			
			Map(x => x.DataCadastro)
				.Column("DT_CADASTRO")
					.Not.Nullable();
		}

	}
}
