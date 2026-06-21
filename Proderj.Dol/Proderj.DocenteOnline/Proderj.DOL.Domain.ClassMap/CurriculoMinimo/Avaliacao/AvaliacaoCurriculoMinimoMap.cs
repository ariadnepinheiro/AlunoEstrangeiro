using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class AvaliacaoCurriculoMinimoMap : ClassMap<AvaliacaoCurriculoMinimo>
	{
		public AvaliacaoCurriculoMinimoMap()
		{
			Table("VW_TCE_AVALIACAO_CM");

			Id(x => x.Id)
				.Column("ID_AVALIACAO_CM")
					.Not.Nullable();

			HasMany(x => x.AvaliacoesCurriculoMinimoDocente)
				.KeyColumn("ID_AVALIACAO_CM");
			
			Map(x => x.Ano)
				.Column("ANO")
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
			
			Map(x => x.DescricaoAvaliacao)
				.Column("AVALIACAO")
					.Not.Nullable()
						.Length(500);
			
			Map(x => x.Habilitado)
				.Column("HABILITADO")
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
