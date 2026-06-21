using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class DisciplinaMap : ClassMap<Disciplina>
	{
		public DisciplinaMap()
		{
			Table("VW_LY_DISCIPLINA");

			LazyLoad();

			Id(x => x.CodigoDisciplina)
					.Column("DISCIPLINAID")
						.Not.Nullable()
							.Length(20);

			Map(x => x.GrupoNota)
					.Column("GRUPO_NOTA")
							.Length(20);
			
			Map(x => x.DescricaoCompleta)
					.Column("DESCRICAOCOMPLETA")
							.Length(20);

			Map(x => x.NotaMaxima)
					.Column("NOTA_MAX")
							.Length(15);

			Map(x => x.QuantCasasDecimais)
					.Column("N_CASAS_DEC")
						.Length(5);

            Map(x => x.TemNota)
                    .Column("tem_nota")
                        .Length(1);

            Map(x => x.TemFrequencia)
                    .Column("tem_freq")
                        .Length(1);
		}
	}
}
