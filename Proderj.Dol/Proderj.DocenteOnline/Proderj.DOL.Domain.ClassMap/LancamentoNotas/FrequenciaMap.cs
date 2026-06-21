using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class FrequenciaMap : ClassMap<Frequencia>
	{
		public FrequenciaMap()
		{
			Table("VW_LY_FREQ");

			LazyLoad();

			//Chave primária é ANO/PERIODO/DISCIPLINA/TURMA/FREQ
			Id(x => x.TipoFrequencia)
				.Column("FREQ")
					.Not.Nullable()
						.Length(4);

			Map(x => x.Ano)
				.Column("ANO")
					.Not.Nullable();

			Map(x => x.Semestre)
				.Column("PERIODO")
					.Not.Nullable();

			Map(x => x.SubPeriodo)
				.Column("SUBPERIODO")
					.Not.Nullable();

			Map(x => x.Turma)
				.Column("TURMA")
					.Not.Nullable()
						.Length(20);

			Map(x => x.Disciplina)
				.Column("DISCIPLINA")
					.Not.Nullable()
						.Length(20);

			Map(x => x.AulasPrevistas)
				.Column("AULAS_PREVISTAS")
					.Length(15);

			Map(x => x.AulasDadas)
				.Column("AULAS_DADAS")
					.Length(1);

			Map(x => x.Descricao)
				.Column("DESCRICAO")
					.Length(200);
		}
	}
}
