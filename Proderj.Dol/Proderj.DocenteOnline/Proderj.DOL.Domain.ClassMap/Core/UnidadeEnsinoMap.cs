using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class UnidadeEnsinoMap : ClassMap<UnidadeEnsino>
    {
		public UnidadeEnsinoMap()
        {
            Table("VW_LY_UNIDADE_ENSINO_REGIONAL");

            LazyLoad();

			Id(x => x.Codigo)
				.Column("UNIDADE_ENS");

            Map(x => x.DescricaoCompleta)
                .Column("DESCRICAOCOMPLETA")
                    .Not.Nullable()
                        .Length(100);

			References(x => x.Municipio)
				.Column("MUNICIPIO");

			References(x => x.Nucleo)
				.Column("NUCLEO");

            References(x => x.Regional)
            .Column("REGIONAL");
        }
    }
}
