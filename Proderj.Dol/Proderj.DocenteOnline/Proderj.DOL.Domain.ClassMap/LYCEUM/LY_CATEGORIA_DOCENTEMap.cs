using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {
    
    
    public class LY_CATEGORIA_DOCENTEMap : ClassMap<LY_CATEGORIA_DOCENTE> {
        
        public LY_CATEGORIA_DOCENTEMap() {
			Table("LY_CATEGORIA_DOCENTE");
			LazyLoad();
			Id(x => x.CATEGORIA).GeneratedBy.Assigned().Column("CATEGORIA");
			//References(x => x.LY_FUNCAO).Column("FUNCAO");
            //References(x => x.AGRUPAMENTOCARGOS).Column("AGRUPAMENTOCARGOSID");
            Map(x => x.NOME).Column("NOME").Length(100);
            Map(x => x.FUNCAO).Column("FUNCAO").Length(20);
            Map(x => x.INGRESSO).Column("INGRESSO").Not.Nullable().Length(1);
            Map(x => x.NECESSITA_SUPERIOR).Column("NECESSITA_SUPERIOR").Length(1);
            Map(x => x.TIPO).Column("TIPO").Length(100);
            Map(x => x.USUARIOID).Column("USUARIOID").Length(15);
            Map(x => x.DATACADASTRO).Column("DATACADASTRO");
            Map(x => x.DATAALTERACAO).Column("DATAALTERACAO");
            Map(x => x.AGRUPAMENTOCARGOSID).Column("AGRUPAMENTOCARGOSID").Precision(10);
            Map(x => x.CARGAHORARIACOMPLEMENTACAO).Column("CARGAHORARIACOMPLEMENTACAO").Precision(10);
            Map(x => x.CARGAHORARIAREGENCIA).Column("CARGAHORARIAREGENCIA").Precision(10);
            Map(x => x.CARGAHORARIAPLANEJAMENTO).Column("CARGAHORARIAPLANEJAMENTO").Precision(10);
			HasMany(x => x.LY_DOCENTE).KeyColumn("CATEGORIA");
        }
    }
}
