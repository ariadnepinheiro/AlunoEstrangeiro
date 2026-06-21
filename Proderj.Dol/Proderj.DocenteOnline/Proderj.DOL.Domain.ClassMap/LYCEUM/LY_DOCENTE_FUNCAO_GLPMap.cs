using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain; 

namespace Proderj.DOL.Domain.ClassMap {
    
    
    public class LY_DOCENTE_FUNCAO_GLPMap : ClassMap<LY_DOCENTE_FUNCAO_GLP> {
        
        public LY_DOCENTE_FUNCAO_GLPMap() {
			Table("LY_DOCENTE_FUNCAO_GLP");
			LazyLoad();
			References(x => x.LY_FUNCAO).Column("FUNCAO_GLP");
			References(x => x.LY_UNIDADE_ENSINO).Column("UNIDADE_ENS");
			References(x => x.LY_GRUPO_HABILITACAO).Column("AGRUPAMENTO");
            Id(x => x.ID_DOCENTE_FUNCAO_GLP).GeneratedBy.Assigned().Column("ID_DOCENTE_FUNCAO_GLP");
            Map(x => x.MATRICULA).Column("MATRICULA").Not.Nullable().Length(100);
            Map(x => x.FUNCAO_GLP).Column("FUNCAO_GLP").Not.Nullable().Length(20);
            Map(x => x.ANO).Column("ANO").Precision(4);
            Map(x => x.MES).Column("MES").Precision(2);
            Map(x => x.STATUS).Column("STATUS").Length(40);
            Map(x => x.UNIDADE_ENS).Column("UNIDADE_ENS").Length(20);
            Map(x => x.DATA).Column("DATA");
            Map(x => x.GLP_SOLICITADA).Column("GLP_SOLICITADA").Precision(10).Scale(2);
            Map(x => x.GLP_USADA).Column("GLP_USADA").Precision(10).Scale(2);
            Map(x => x.GLP_CANCELADA).Column("GLP_CANCELADA").Precision(10).Scale(2);
            Map(x => x.AGRUPAMENTO).Column("AGRUPAMENTO").Length(50);
            Map(x => x.PRAZO).Column("PRAZO").Precision(3);
            Map(x => x.CHLIVRE).Column("CHLIVRE").Precision(10);
            Map(x => x.RESERV01).Column("RESERV01").Length(50);
            Map(x => x.USUARIOSOLICITACAOID).Column("USUARIOSOLICITACAOID").Length(15);
            Map(x => x.DATASOLICITACAO).Column("DATASOLICITACAO");
            Map(x => x.USUARIOANALISEID).Column("USUARIOANALISEID").Length(15);
            Map(x => x.DATAANALISE).Column("DATAANALISE");
            Map(x => x.CHLIVREMUNICIPIO).Column("CHLIVREMUNICIPIO").Precision(10);
            //HasMany(x => x.LY_AULA_DOCENTE_TIPO).KeyColumn("ID_DOCENTE_FUNCAO_GLP");
			//HasMany(x => x.LY_DOCENTE_FUNCAO_GLP_DETALHE).KeyColumn("ID_DOCENTE_FUNCAO_GLP");
        }
    }
}
