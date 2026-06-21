using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {
    
    
    public class DISPONIBILIDADEGLP_UNIDADEENSINOMap : ClassMap<DISPONIBILIDADEGLP_UNIDADEENSINO> {
        
        public DISPONIBILIDADEGLP_UNIDADEENSINOMap() {
            Table("DISPONIBILIDADEGLP_UNIDADEENSINO");
            Schema("RecursosHumanos");
			LazyLoad();
			CompositeId().KeyProperty(x => x.DISPONIBILIDADEGLPID, "DISPONIBILIDADEGLPID")
			             .KeyProperty(x => x.UNIDADE_ENS, "UNIDADE_ENS");
			References(x => x.DISPONIBILIDADEGLP).Column("DISPONIBILIDADEGLPID");
			//References(x => x.LY_UNIDADE_ENSINO).Column("UNIDADE_ENS");
        }
    }
}
