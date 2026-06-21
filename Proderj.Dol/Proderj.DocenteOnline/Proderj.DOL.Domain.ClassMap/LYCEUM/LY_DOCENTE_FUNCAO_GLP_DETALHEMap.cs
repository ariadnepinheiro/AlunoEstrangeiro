using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {
    
    
    public class LY_DOCENTE_FUNCAO_GLP_DETALHEMap : ClassMap<LY_DOCENTE_FUNCAO_GLP_DETALHE> {
        
        public LY_DOCENTE_FUNCAO_GLP_DETALHEMap() {
			Table("LY_DOCENTE_FUNCAO_GLP_DETALHE");
			LazyLoad();
            Id(x => x.ID_DOCENTE_FUNCAO_GLP_DETALHE).GeneratedBy.Assigned().Column("ID_DOCENTE_FUNCAO_GLP_DETALHE");
            Map(x => x.ID_DOCENTE_FUNCAO_GLP).Column("ID_DOCENTE_FUNCAO_GLP").Not.Nullable().Precision(10);
            Map(x => x.DATA).Column("DATA").Not.Nullable();
            Map(x => x.STATUS).Column("STATUS").Not.Nullable().Length(40);
            Map(x => x.QTD_GLP).Column("QTD_GLP").Precision(10).Scale(2);
            Map(x => x.USUARIO).Column("USUARIO").Length(15);
            Map(x => x.MOTIVO).Column("MOTIVO").Length(100);
        }
    }
}
