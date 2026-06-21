using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {


    public class GOOGLEEDUCATIONMap : ClassMap<GOOGLEEDUCATION>
    {

        public GOOGLEEDUCATIONMap()
        {
            Table("GOOGLEEDUCATION");
            Schema("RecursosHumanos");
			LazyLoad();

            Id(x => x.GOOGLEEDUCATIONID).GeneratedBy.Identity().Column("GOOGLEEDUCATIONID");
			Map(x => x.PESSOA).Column("PESSOA").Not.Nullable().Precision(10);
            Map(x => x.ALUNO).Column("ALUNO").Nullable().Precision(20);
            Map(x => x.EMAIL).Column("EMAIL").Not.Nullable().Precision(200);
			Map(x => x.USUARIOID).Column("USUARIOID").Not.Nullable().Length(15);
			Map(x => x.DATACADASTRO).Column("DATACADASTRO").Not.Nullable();
			Map(x => x.DATAALTERACAO).Column("DATAALTERACAO").Not.Nullable();
        }
    }
}
