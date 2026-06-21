using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class LoginMap : ClassMap<Login>
    {
        public LoginMap()
        {
            Table("VW_LOGACESSODOCENTE");

			LazyLoad();

            Id(x => x.Id)
                 .Column("LOGACESSODOCENTEID");

            Map(x => x.Sistema)
                .Column("SISTEMA")
                    .CustomType(typeof(int));                   

            Map(x => x.Usuario)
                .Column("USUARIO")
                    .Length(100);

            Map(x => x.DataAcesso)
                .Column("DATAACESSO");
        }
    }
}
