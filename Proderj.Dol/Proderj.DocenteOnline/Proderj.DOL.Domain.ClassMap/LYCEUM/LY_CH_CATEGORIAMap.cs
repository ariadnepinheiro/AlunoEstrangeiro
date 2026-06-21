using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {
    
    
    public class LY_CH_CATEGORIAMap : ClassMap<LY_CH_CATEGORIA> {
        
        public LY_CH_CATEGORIAMap() {
			Table("LY_CH_CATEGORIA");
			LazyLoad();
            Id(x => x.ID_CH_FUNCAO_DOCENTE).GeneratedBy.Assigned().Column("ID_CH_FUNCAO_DOCENTE");
            Map(x => x.CATEGORIA_DOCENTE).Column("CATEGORIA_DOCENTE").Not.Nullable().Length(20);
            Map(x => x.FUNCAO).Column("FUNCAO").Not.Nullable().Length(20);
            Map(x => x.NR_MATRICULAS).Column("NR_MATRICULAS").Not.Nullable().Precision(10);
            Map(x => x.READAPTADO).Column("READAPTADO").Length(1);
            Map(x => x.GLP).Column("GLP").Length(1);
            Map(x => x.CH_SEMANAL_TOTAL).Column("CH_SEMANAL_TOTAL").Not.Nullable().Precision(10).Scale(2);
            Map(x => x.CH_SEMANAL_EFETIVA).Column("CH_SEMANAL_EFETIVA").Precision(10).Scale(2);
            Map(x => x.CH_GRUPO).Column("CH_GRUPO").Precision(10);
            Map(x => x.CATEGORIA_DOCENTE_2).Column("CATEGORIA_DOCENTE_2").Length(20);
            Map(x => x.CH_GRUPO_2).Column("CH_GRUPO_2").Precision(10);
            Map(x => x.FUNCAO_2).Column("FUNCAO_2").Length(20);
            Map(x => x.CH_GLP).Column("CH_GLP").Precision(10);
            Map(x => x.USUARIOID).Column("USUARIOID").Length(15);
            Map(x => x.DATACADASTRO).Column("DATACADASTRO");
            Map(x => x.DATAALTERACAO).Column("DATAALTERACAO");
        }
    }
}
