using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class NucleoMap : ClassMap<Nucleo>
	{
		public NucleoMap()
		{
			Table("VW_LY_NUCLEO");
			
			LazyLoad();
			
			Id(x => x.Codigo)
				.Column("NUCLEO");
			
			Map(x => x.Descricao)
				.Column("DESCRICAO")
					.Not.Nullable()
						.Length(100);
		}
	}
}
