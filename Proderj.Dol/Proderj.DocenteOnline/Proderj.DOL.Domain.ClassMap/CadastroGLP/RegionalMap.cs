using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class RegionalMap : ClassMap<Regional>
    {
        public RegionalMap()
        {
            Table("VW_TCE_REGIONAL");

            LazyLoad();

            Id(x => x.Codigo)
                .Column("REGIONAL");

            Map(x => x.Descricao)
                .Column("DESCRICAO")
                    .Not.Nullable()
                        .Length(100);
        }
    }
}
