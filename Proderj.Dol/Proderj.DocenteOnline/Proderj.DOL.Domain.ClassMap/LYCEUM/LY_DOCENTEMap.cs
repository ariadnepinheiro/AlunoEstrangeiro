using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class LY_DOCENTEMap : ClassMap<LY_DOCENTE>
    {
        public LY_DOCENTEMap()
        {
			Table("LY_DOCENTE");
			
            LazyLoad();

            Id(x => x.NUM_FUNC).Column("NUM_FUNC").Not.Nullable();
            //Map(x => x.SENHA_DOL).Column("SENHA_DOL").Length(40);
            //Map(x => x.CATEGORIA).Column("CATEGORIA").Length(20);
            //Map(x => x.DT_ADMISSAO).Column("DT_ADMISSAO");
            //Map(x => x.PESSOA).Column("PESSOA").Precision(10);
            //Map(x => x.REGIME_TRABALHO).Column("REGIME_TRABALHO").Length(50);
            //Map(x => x.DT_DEMISSAO).Column("DT_DEMISSAO");
            //Map(x => x.STAMP_ATUALIZACAO).Column("STAMP_ATUALIZACAO");
            Map(x => x.MATRICULA).Column("MATRICULA").Not.Nullable().Length(100);
            //Map(x => x.SENHA_ALTERADA).Column("SENHA_ALTERADA").Length(1);
            //Map(x => x.ANO_INGRESSO).Column("ANO_INGRESSO").Precision(4);
            //Map(x => x.CONCURSO).Column("CONCURSO").Length(20);
            //Map(x => x.CANDIDATO).Column("CANDIDATO").Length(20);
            //Map(x => x.VOLUNTARIO).Column("VOLUNTARIO").Length(1);
            //Map(x => x.REGIMECONTRATACAOID).Column("REGIMECONTRATACAOID").Precision(5);
            //Map(x => x.ACUMULACAO).Column("ACUMULACAO").Not.Nullable().Precision(5);
            //Map(x => x.VINCULO).Column("VINCULO").Precision(10);
            //Map(x => x.USUARIO).Column("USUARIO").Length(15);
            //Map(x => x.DATACADASTRO).Column("DATACADASTRO");
            //Map(x => x.DATAALTERACAO).Column("DATAALTERACAO");

            //References(x => x.LY_PESSOA, "PESSOA")
            //    .Cascade.None();
        }
    }
}
