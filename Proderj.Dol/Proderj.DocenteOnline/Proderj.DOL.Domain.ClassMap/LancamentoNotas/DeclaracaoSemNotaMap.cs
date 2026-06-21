using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Domain.ClassMap
{
    public class DeclaracaoSemNotaMap : ClassMap<DeclaracaoSemNota>
    {
        public DeclaracaoSemNotaMap()
        {
            Table("DeclaracaoSemNota");

            LazyLoad();

            Id(x => x.Id)
                .Column("DECLARACAOSEMNOTAID")
                .GeneratedBy
                .Identity();

            Map(x => x.NotaId)
                .Column("NOTAID");

            Map(x => x.TipoDeclaracaoSemNota)
                .Column("TIPODECLARACAOSEMNOTAID");

            Map(x => x.Matricula)
                .Column("MATRICULA")
                    .Length(8);

            Map(x => x.DataCadastro)
                .Column("DATACADASTRO");
        }
    }

}
