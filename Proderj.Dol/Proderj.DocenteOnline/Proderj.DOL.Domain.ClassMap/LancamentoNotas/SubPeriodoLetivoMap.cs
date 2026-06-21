using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class SubPeriodoLetivoMap : ClassMap<SubPeriodoLetivo>
    {
        public SubPeriodoLetivoMap()
        {
			Table("VW_LY_SUBPERIODO_LETIVO");
            			
            LazyLoad();

            //Chave primária é ANO/PERIODO/SUBPERIODO
            Id(x => x.SubPeriodo)
                .Column("SUBPERIODO")
                    .Not.Nullable();
			
            Map(x => x.Ano)
                .Column("ANO")
                    .Not.Nullable();
			            
            Map(x => x.Periodo)
                .Column("PERIODO")
                    .Not.Nullable();
			
            Map(x => x.DataInicio)
                .Column("DT_INICIO");
			
            Map(x => x.DataLancamento)
                .Column("DT_LANCAMENTO");
			
            Map(x => x.Descricao)
                .Column("DESCRICAO")
                    .Length(100);
			
            Map(x => x.DataCurriculoMinimo)
                .Column("DT_CURRICULO_MINIMO");
        }
    }
}
