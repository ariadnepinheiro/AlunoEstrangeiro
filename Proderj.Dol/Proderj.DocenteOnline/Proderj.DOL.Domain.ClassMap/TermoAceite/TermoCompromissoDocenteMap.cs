using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class TermoCompromissoDocenteMap : ClassMap<TermoCompromissoDocente>
    {
        public TermoCompromissoDocenteMap()
        {
            Table("VW_TCE_TERMO_COMPROMISSO_DOCENTE");

            LazyLoad();

            Id(x => x.Id)
                .Column("ID_TERMO_DOCENTE")
                    .Not.Nullable();

            Map(x => x.Ano)
                .Column("ANO")
                    .Not.Nullable();

            Map(x => x.DataInicio)
                .Column("DT_INICIO")
                    .Not.Nullable();

            Map(x => x.DataFim)
                .Column("DT_FIM")
                    .Not.Nullable();

            Map(x => x.Arquivo)
                .Column("ARQUIVO")
                    .Not.Nullable().Length(500);

            Map(x => x.DataCadastro)
                .Column("DT_CADASTRO")
                    .Not.Nullable();

            Map(x => x.DataAlteracao)
                .Column("DT_ALTERACAO");

            Map(x => x.Num_func)
                .Column("NUM_FUNC")
                    .Not.Nullable()
                        .Length(15);

            HasMany(x => x.AceiteTermoCompromissoDocente)
                .Cascade.All();
        }
    }
}
