using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class PeriodoLetivoMap : ClassMap<PeriodoLetivo>    
    {
         public PeriodoLetivoMap()
        {
			Table("VW_LY_PERIODO_LETIVO");
			
            LazyLoad();

            Id(x => x.Ano)
                .Column("ANO")
                    .Not.Nullable();

            Map(x => x.Periodo)
                 .Column("PERIODO")
                     .Not.Nullable();

            Map(x => x.DescricaoPeriodo)
              .Column("DESCRICAO")
                  .Not.Nullable()
                      .Length(100);
        }

    }
}
