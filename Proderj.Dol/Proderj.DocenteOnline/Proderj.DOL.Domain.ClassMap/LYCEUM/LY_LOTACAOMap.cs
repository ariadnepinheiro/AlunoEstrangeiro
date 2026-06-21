using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {
    
    
    public class LY_LOTACAOMap : ClassMap<LY_LOTACAO> {
        
        public LY_LOTACAOMap() {
			Table("LY_LOTACAO");
			LazyLoad();
            CompositeId().KeyProperty(x => x.PESSOA, "PESSOA")
                         .KeyProperty(x => x.MATRICULA, "MATRICULA")
                         .KeyProperty(x => x.ORDEM, "ORDEM");
            //Map(x => x.FUNCAO).Column("FUNCAO").Not.Nullable().Length(20);
            //Map(x => x.TURNO).Column("TURNO").Length(20);
            //Map(x => x.DATA_DESATIVACAO).Column("DATA_DESATIVACAO");
            //Map(x => x.ATO_OFICIAL).Column("ATO_OFICIAL").Length(100);
            //Map(x => x.RESP_DOCUMENTACAO).Column("RESP_DOCUMENTACAO").Length(1);
            //Map(x => x.UNIDADE_FIS).Column("UNIDADE_FIS").Length(20);
            //Map(x => x.DATA_NOMEACAO).Column("DATA_NOMEACAO");
            //Map(x => x.DATA_NOMEACAO_DO).Column("DATA_NOMEACAO_DO");
            //Map(x => x.DATA_DESATIVACAO_DO).Column("DATA_DESATIVACAO_DO");
            //Map(x => x.TIPO_DESATIVACAO).Column("TIPO_DESATIVACAO").Length(100);
            //Map(x => x.UNIDADE_ENS).Column("UNIDADE_ENS").Length(20);
            //Map(x => x.NUCLEO).Column("NUCLEO").Length(20);
            //Map(x => x.SETOR).Column("SETOR").Length(15);
            //Map(x => x.READAPTADO).Column("READAPTADO").Length(1);
            //Map(x => x.DT_INICIO_READAPTACAO).Column("DT_INICIO_READAPTACAO");
            //Map(x => x.DT_FIM_READAPTACAO).Column("DT_FIM_READAPTACAO");
            //Map(x => x.MOTIVO_READAPTACAO).Column("MOTIVO_READAPTACAO").Length(100);
            //Map(x => x.USUARIO).Column("USUARIO").Length(15);
            //Map(x => x.DATA_ATUALIZACAO).Column("DATA_ATUALIZACAO");
        }
    }
}
