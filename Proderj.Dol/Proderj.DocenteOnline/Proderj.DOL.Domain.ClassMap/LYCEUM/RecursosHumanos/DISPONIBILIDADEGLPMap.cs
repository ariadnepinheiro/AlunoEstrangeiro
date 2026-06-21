using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {


    public class DISPONIBILIDADEGLPMap : ClassMap<DISPONIBILIDADEGLP>
    {
        
        public DISPONIBILIDADEGLPMap() {
			Table("DISPONIBILIDADEGLP");
            Schema("RecursosHumanos");
			LazyLoad();
			Id(x => x.DISPONIBILIDADEGLPID).GeneratedBy.Identity().Column("DISPONIBILIDADEGLPID");
			References(x => x.LY_GRUPO_HABILITACAO).Column("AGRUPAMENTO");
			References(x => x.LY_DOCENTE).Column("NUM_FUNC");
			Map(x => x.ANO).Column("ANO").Not.Nullable().Precision(10);
			Map(x => x.USUARIOID).Column("USUARIOID").Not.Nullable().Length(15);
			Map(x => x.DATACADASTRO).Column("DATACADASTRO").Not.Nullable();
			Map(x => x.DATAALTERACAO).Column("DATAALTERACAO").Not.Nullable();
			HasMany(x => x.DISPONIBILIDADEGLP_DIASEMANA).KeyColumn("DISPONIBILIDADEGLPID");
			HasMany(x => x.DISPONIBILIDADEGLP_MODALIDADE).KeyColumn("DISPONIBILIDADEGLPID");
			HasMany(x => x.DISPONIBILIDADEGLP_TURNO).KeyColumn("DISPONIBILIDADEGLPID");
			HasMany(x => x.DISPONIBILIDADEGLP_UNIDADEENSINO).KeyColumn("DISPONIBILIDADEGLPID");
        }
    }
}
