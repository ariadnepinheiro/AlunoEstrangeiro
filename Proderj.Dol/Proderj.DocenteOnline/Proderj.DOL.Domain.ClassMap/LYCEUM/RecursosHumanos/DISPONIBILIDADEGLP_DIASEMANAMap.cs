using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {


    public class DISPONIBILIDADEGLP_DIASEMANAMap : ClassMap<DISPONIBILIDADEGLP_DIASEMANA>
    {
        
        public DISPONIBILIDADEGLP_DIASEMANAMap() {
            Table("DISPONIBILIDADEGLP_DIASEMANA");
            Schema("RecursosHumanos");
			LazyLoad();
			CompositeId().KeyProperty(x => x.DISPONIBILIDADEGLPID, "DISPONIBILIDADEGLPID")
			             .KeyProperty(x => x.DIASEMANA, "DIASEMANA");
			References(x => x.DISPONIBILIDADEGLP).Column("DISPONIBILIDADEGLPID");
        }
    }
}
