using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap()
        {
			Table("VW_LY_PESSOA");
			
            LazyLoad();

            Id(x => x.Id)
                .Column("PESSOA")
                    .Not.Nullable();

            Map(x => x.NomeCompleto)
                .Column("NOME_COMPL")
                    .Not.Nullable()
                        .Length(100);

            Map(x => x.PreNomeSocial)
                .Column("PRE_NOME_SOCIAL")
                    .Not.Nullable()
                        .Length(100);

            Map(x => x.Telefone)
                .Column("FONE")
                    .Length(30);

            Map(x => x.EmailInterno)
                .Column("E_MAIL_INTERNO")
                    .Length(100);
        }
    }
}
