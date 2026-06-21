using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class DocenteMap : ClassMap<Docente>
    {
        public DocenteMap()
        {
			Table("VW_LY_DOCENTE");
			
            LazyLoad();

            Id(x => x.NumeroFuncionario)
             .Column("NUM_FUNC")
                 .Not.Nullable();
					
            Map(x => x.Matricula)
                .Column("MATRICULA")
                    .Length(100);
            
            Map(x => x.SenhaDocente)
                .Column("SENHA_DOL")
                    .Length(40);

            Map(x => x.SenhaAlterada)
                .Column("SENHA_ALTERADA")
                    .Length(1);

            References(x => x.Pessoa)
                .Column("PESSOA")
                    .Not.Nullable();

 


        }
    }
}
