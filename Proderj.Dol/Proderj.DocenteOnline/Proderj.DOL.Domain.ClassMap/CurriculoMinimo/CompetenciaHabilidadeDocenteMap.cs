using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class CompetenciaHabilidadeDocenteMap : ClassMap<CompetenciaHabilidadeDocente>
	{
		public CompetenciaHabilidadeDocenteMap()
		{
			Table("VW_TCE_COMPETENCIA_HABILIDADE_DOCENTE");

			LazyLoad();

			Id(x => x.Id)
				.Column("ID_COMPETENCIA_HABILIDADE_GRUPO")
						.Not.Nullable();

			References(x => x.CompetenciaHabilidadeItem)
				.Column("ID_COMPETENCIA_HABILIDADE_ITEM")
					.Cascade.All();

			Map(x => x.Matricula)
					.Column("MATRICULA")
						.Not.Nullable()
							.Length(20);

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
						.Not.Nullable()
							.Length(20);

			Map(x => x.Periodo)
					.Column("PERIODO")
						.Not.Nullable()
							.Length(20);

			Map(x => x.SubPeriodo)
					.Column("SUBPERIODO")
						.Not.Nullable()
							.Length(20);

			Map(x => x.DataCadastro)
					.Column("DT_CADASTRO")
						.Not.Nullable();			
			
		}
	}
}
