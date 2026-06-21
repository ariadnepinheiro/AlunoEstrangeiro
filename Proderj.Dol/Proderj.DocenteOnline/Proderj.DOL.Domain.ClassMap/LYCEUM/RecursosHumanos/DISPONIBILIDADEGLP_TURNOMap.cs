using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {
    
    
    public class DISPONIBILIDADEGLP_TURNOMap : ClassMap<DISPONIBILIDADEGLP_TURNO> {
        
        public DISPONIBILIDADEGLP_TURNOMap() {
            Table("DISPONIBILIDADEGLP_TURNO");
            Schema("RecursosHumanos");
			LazyLoad();
			CompositeId().KeyProperty(x => x.DISPONIBILIDADEGLPID, "DISPONIBILIDADEGLPID")
			             .KeyProperty(x => x.TURNO, "TURNO");
			References(x => x.DISPONIBILIDADEGLP).Column("DISPONIBILIDADEGLPID");
			//References(x => x.LY_TURNO).Column("TURNO");
        }
    }
}
