using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {


    public class LY_FUNCAOMap : ClassMap<LY_FUNCAO>
    {
        
        public LY_FUNCAOMap() {
			Table("LY_FUNCAO");
			LazyLoad();
            Id(x => x.FUNCAO).GeneratedBy.Assigned().Column("FUNCAO");
            Map(x => x.DESCRICAO).Column("DESCRICAO").Length(100);
            Map(x => x.FUNCAOBB).Column("FUNCAOBB").Length(50);
            Map(x => x.TIPO).Column("TIPO").Length(40);
            Map(x => x.CAMPO_01).Column("CAMPO_01").Length(20);
            Map(x => x.CAMPO_02).Column("CAMPO_02").Length(20);
            Map(x => x.CAMPO_03).Column("CAMPO_03").Length(20);
            Map(x => x.CAMPO_04).Column("CAMPO_04").Length(20);
            Map(x => x.CAMPO_05).Column("CAMPO_05").Length(20);
            Map(x => x.CAMPO_06).Column("CAMPO_06").Length(20);
            Map(x => x.CAMPO_07).Column("CAMPO_07").Length(20);
            Map(x => x.CAMPO_08).Column("CAMPO_08").Length(20);
            Map(x => x.CAMPO_09).Column("CAMPO_09").Length(20);
            Map(x => x.CAMPO_10).Column("CAMPO_10").Length(20);
            Map(x => x.SEMCARGAHORARIAEFETIVA).Column("SEMCARGAHORARIAEFETIVA").Not.Nullable().Length(1);
            //HasMany(x => x.FUNCAO).KeyColumn("FUNCAOID");
			//HasMany(x => x.LY_CATEGORIA_DOCENTE).KeyColumn("FUNCAO");
			//HasMany(x => x.LY_CH_CATEGORIA).KeyColumn("FUNCAO");
            //HasMany(x => x.LY_CONCURSO_DOCENTE).KeyColumn("FUNCAOID");
			//HasMany(x => x.LY_DOCENTE_FUNCAO_GLP).KeyColumn("FUNCAO_GLP");
            //HasMany(x => x.LY_ESTAGIO_EMPRESA).KeyColumn("FUNCAO");
            //HasMany(x => x.LY_EXTRA_CLASSE).KeyColumn("FUNCAO");
            //HasMany(x => x.LY_HISTESTAGIO_EMPRESA).KeyColumn("FUNCAO");
			HasMany(x => x.LY_LOTACAO).KeyColumn("FUNCAO");
            //HasMany(x => x.LY_PADACES_FUNCAO).KeyColumn("FUNCAO");
            //HasMany(x => x.LY_VALOR_GLP).KeyColumn("FUNCAO");
        }
    }
}
