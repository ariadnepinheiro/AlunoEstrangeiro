using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class MunicipioMap : ClassMap<Municipio>
	{
		public MunicipioMap()
		{
			Table("MUNICIPIO");

			LazyLoad();

			Id(x => x.Codigo)
				.Not.Nullable()
					.Column("CODIGO");

			Map(x => x.Nome)
				.Column("NOME")
					.Not.Nullable()
						.Length(50);

			Map(x => x.SiglaUF)
				.Column("UF_SIGLA")
					.Not.Nullable()
						.Length(2);
		}
	}
}
