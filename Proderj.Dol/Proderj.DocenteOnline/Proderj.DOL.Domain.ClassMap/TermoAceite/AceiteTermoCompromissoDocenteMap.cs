using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{   
    public class AceiteTermoCompromissoDocenteMap : ClassMap<AceiteTermoCompromissoDocente>
    {
        public AceiteTermoCompromissoDocenteMap()
        {
            Table("TCE_ACEITE_TERMO_COMPROMISSO_DOCENTE");
            
            LazyLoad();

        	Id(x => x.Id).GeneratedBy.Native()
        		.Column("ID_ACEITE");
					
						

            References(x => x.TermoCompromissoDocente)
                .Column("ID_TERMO_DOCENTE")
                    .Not.Nullable();
            
            Map(x => x.Num_func )
                .Column("NUM_FUNC")
                    .Not.Nullable()
                        .Length(15);
            
            Map(x => x.IP)
                .Column("IP")
                    .Not.Nullable()
                        .Length(20);
            
            Map(x => x.Ano)
                .Column("ANO")
                    .Not.Nullable();            
         
            
            Map(x => x.DataAceite)
                .Column("DT_ACEITE")
                    .Not.Nullable();
        }
    }
}
