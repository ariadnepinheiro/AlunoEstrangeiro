using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class UltimoResetMap : ClassMap<UltimoReset>
    {
        public UltimoResetMap()
        {
			Table("VW_TCE_ULTIMO_RESET");
			
            LazyLoad();

            Id(x => x.Matricula)
                .Column("MATRICULA")
                    .Not.Nullable()
                        .Length(8);
			
            Map(x => x.DataUltimoReset)
                .Column("DT_ULTIMO_RESET")
                    .Not.Nullable();
        }
    }
}
