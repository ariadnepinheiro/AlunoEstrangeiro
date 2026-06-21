using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class CompetenciaHabilidadeGrupoMap : ClassMap<CompetenciaHabilidadeGrupo>
	{
		public CompetenciaHabilidadeGrupoMap()
		{
			Table("VW_TCE_COMPETENCIA_HABILIDADE_GRUPO");

			LazyLoad();

			Id(x => x.Id)
					.Column("ID_COMPETENCIA_HABILIDADE_GRUPO")
						.Not.Nullable();

			HasMany(x => x.CompetenciaHabilidadeItens)
				//.Inverse()
					.KeyColumn("ID_COMPETENCIA_HABILIDADE_GRUPO");
						
			Map(x => x.Disciplina)
					.Column("DISCIPLINA")
						.Not.Nullable()
							.Length(20);

			Map(x => x.Grupo)
					.Column("GRUPO")
						.Not.Nullable()
							.Length(200);

			Map(x => x.Curso)
					.Column("CURSO")
						.Not.Nullable()
							.Length(20);

			Map(x => x.Modalidade)
					.Column("MODALIDADE")
						.Not.Nullable()
							.Length(20);

			Map(x => x.TipoCurso)
					.Column("TIPO_CURSO")
						.Not.Nullable()
							.Length(20);

			Map(x => x.Ano)
					.Column("ANO")
						.Not.Nullable();

			Map(x => x.Serie)
					.Column("SERIE")
						.Not.Nullable();
						

			Map(x => x.Periodo)
					.Column("PERIODO")
						.Not.Nullable();

			Map(x => x.SubPeriodo)
					.Column("SUBPERIODO")
						.Not.Nullable();

			Map(x => x.Ordem)
					.Column("ORDEM")
						.Not.Nullable();


		}
	}
}
