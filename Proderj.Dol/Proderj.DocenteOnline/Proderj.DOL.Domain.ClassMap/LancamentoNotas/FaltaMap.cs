using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class FaltaMap : ClassMap<Falta>
	{
		public FaltaMap()
		{
			Table("VW_LY_FALTA");

			LazyLoad();

			Id(x => x.Id)//.Access.ReadOnly()
				.Column("FALTAID");
					//.GeneratedBy.Native();
				//.GeneratedBy.Increment();
					//.GeneratedBy.Assigned();
					//.GeneratedBy.Identity();

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

			Map(x => x.Semestre)
				.Column("PERIODO")
					.Not.Nullable();
			
			Map(x => x.Aluno)
				.Column("ALUNO")
					.Not.Nullable()
						.Length(20);

			Map(x => x.Frequencia)
				.Column("FREQ")
					.Not.Nullable();

			Map(x => x.QuantFaltas)
				.Column("FALTAS")
					.Not.Nullable()
						.Length(20);
		}
	}
}
