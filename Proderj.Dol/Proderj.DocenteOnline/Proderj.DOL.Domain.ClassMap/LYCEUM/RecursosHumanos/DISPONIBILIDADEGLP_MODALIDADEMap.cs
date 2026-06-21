using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {
    
    
    public class DISPONIBILIDADEGLP_MODALIDADEMap : ClassMap<DISPONIBILIDADEGLP_MODALIDADE> {
        
        public DISPONIBILIDADEGLP_MODALIDADEMap() {
            Table("DISPONIBILIDADEGLP_MODALIDADE");
            Schema("RecursosHumanos");
			LazyLoad();
			CompositeId().KeyProperty(x => x.DISPONIBILIDADEGLPID, "DISPONIBILIDADEGLPID")
			             .KeyProperty(x => x.MODALIDADE, "MODALIDADE");
			References(x => x.DISPONIBILIDADEGLP).Column("DISPONIBILIDADEGLPID");
        }
    }
}
