using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {
    
    
    public class LY_GRUPO_HABILITACAOMap : ClassMap<LY_GRUPO_HABILITACAO> {
        
        public LY_GRUPO_HABILITACAOMap() {
			Table("LY_GRUPO_HABILITACAO");
			LazyLoad();
            Id(x => x.AGRUPAMENTO).GeneratedBy.Assigned().Column("AGRUPAMENTO");
            //Map(x => x.DESCRICAO).Column("DESCRICAO").Not.Nullable().Length(200);
            //Map(x => x.STAMP_ATUALIZACAO).Column("STAMP_ATUALIZACAO");
            //Map(x => x.TIPO).Column("TIPO").Length(20);
            //Map(x => x.DISP_GLP_DOL).Column("DISP_GLP_DOL").Length(1);
            //Map(x => x.INGRESSO).Column("INGRESSO").Not.Nullable().Length(1);
            //Map(x => x.ATIVO).Column("ATIVO").Length(1);
            //Map(x => x.USUARIO).Column("USUARIO").Length(15);
            //Map(x => x.DATACADASTRO).Column("DATACADASTRO");
            //Map(x => x.DATAALTERACAO).Column("DATAALTERACAO");
            //HasMany(x => x.LY_CONCURSO_DOC_HABILITACAO).KeyColumn("AGRUPAMENTO");
			//HasMany(x => x.LY_DOCENTE_FUNCAO_GLP).KeyColumn("AGRUPAMENTO");
            //HasMany(x => x.LY_GRUPO_HABILITACAO_DISC).KeyColumn("AGRUPAMENTO");
            //HasMany(x => x.LY_GRUPO_HABILITACAO_DOC).KeyColumn("AGRUPAMENTO");
            //HasMany(x => x.TCE_SOLICITACAO_HABILITACAO_DOCENTE).KeyColumn("AGRUPAMENTO");
        }
    }
}
