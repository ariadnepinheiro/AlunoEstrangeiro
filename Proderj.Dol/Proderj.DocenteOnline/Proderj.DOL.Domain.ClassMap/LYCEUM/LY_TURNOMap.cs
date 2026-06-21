using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {
    
    
    public class LY_TURNOMap : ClassMap<LY_TURNO> {
        
        public LY_TURNOMap() {
			Table("LY_TURNO");
			LazyLoad();
            Id(x => x.TURNO).GeneratedBy.Assigned().Column("TURNO");
            Map(x => x.MNEMONICO).Column("MNEMONICO").Not.Nullable().Length(2);
            Map(x => x.DESCRICAO).Column("DESCRICAO").Length(100);
            Map(x => x.STAMP_ATUALIZACAO).Column("STAMP_ATUALIZACAO");
            Map(x => x.HORAINICIO).Column("HORAINICIO").Not.Nullable();
            Map(x => x.HORAFIM).Column("HORAFIM").Not.Nullable();
            //HasMany(x => x.LY_CENTROCUSTO_ASSOCIACAO).KeyColumn("TURNO");
            //HasMany(x => x.LY_CURRICULO).KeyColumn("TURNO");
            //HasMany(x => x.LY_FERIADO).KeyColumn("TURNO");
            //HasMany(x => x.LY_HOR_OPER).KeyColumn("TURNO");
            //HasMany(x => x.LY_JUR_EQUIPE).KeyColumn("TURNO");
            //HasMany(x => x.LY_LOTACAO).KeyColumn("TURNO");
            //HasMany(x => x.LY_SALA_SERIE).KeyColumn("TURNO");
            //HasMany(x => x.LY_TURMA).KeyColumn("TURNO");
            //HasMany(x => x.LY_UNIDADE_ENSINO_CURSOS).KeyColumn("TURNO");
            //HasMany(x => x.RENOVACAO).KeyColumn("TURNOID");
            //HasMany(x => x.TCE_CVT_CONFIRMACAO_INICIAL_ESCOLA).KeyColumn("TURNO");
            //HasMany(x => x.TCE_CVT_CONFIRMACAO_TURNO_VAGA).KeyColumn("TURNO");
        }
    }
}
