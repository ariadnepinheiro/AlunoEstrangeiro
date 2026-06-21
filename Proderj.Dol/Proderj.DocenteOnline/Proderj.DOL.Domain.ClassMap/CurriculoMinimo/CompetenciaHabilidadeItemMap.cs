using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class CompetenciaHabilidadeItemMap : ClassMap<CompetenciaHabilidadeItem>
	{
		public CompetenciaHabilidadeItemMap()
		{
			Table("VW_TCE_COMPETENCIA_HABILIDADE_ITEM");

			LazyLoad();

			Id(x => x.Id)
					.Column("ID_COMPETENCIA_HABILIDADE_ITEM")
						.Not.Nullable();

			Map(x => x.Ordem)				
					.Column("ORDEM")
						.Not.Nullable();

			Map(x => x.DataCadastro)
					.Column("DT_CADASTRO")
						.Not.Nullable();

			Map(x => x.CompetenciaHabilidade)
					.Column("COMPETENCIA_HABILIDADE")
						.Not.Nullable()
							.Length(1000);

			References(x => x.CompetenciaHabilidadeGrupo)
					.Column("ID_COMPETENCIA_HABILIDADE_GRUPO")
						.Cascade.All();
		}
	}
}
