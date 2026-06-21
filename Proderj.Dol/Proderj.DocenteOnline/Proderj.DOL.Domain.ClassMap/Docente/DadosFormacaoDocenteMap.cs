using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class DadosFormacaoDocenteMap: ClassMap<DadosFormacaoDocente>
    {
        public DadosFormacaoDocenteMap()
        {
            Table("VW_FORMACAO_DOCENTE");

            LazyLoad();

            Id(x => x.Row)
                .Column("Row");

            Map(x => x.Matricula)
                .Column("MATRICULA")
                .Not.Nullable()
                .Length(100);

            Map(x => x.Escolaridade)
                .Column("ESCOLARIDADE")
                .Not.Nullable()
                .Length(500);

            Map(x => x.SituacaoCurso)
                .Column("SITUACAO_CURSO")
                .Not.Nullable()
                .Length(100);

            Map(x => x.AreaCurso)
                .Column("AREA_CURSO")
                .Not.Nullable()
                .Length(100);

            Map(x => x.Curso)
                .Column("CURSO")
                .Not.Nullable()
                .Length(100);

            Map(x => x.FormacaoComplementacaoPedagogica)
                .Column("FORMACAO_COMPLEMENTACAO_PEDAGOGICA")
                .Not.Nullable()
                .Length(50);

            Map(x => x.AnoInicio)
                .Column("ANO_INICIO")
                .Not.Nullable();

            Map(x => x.AnoConclusao)
                .Column("ANO_CONCLUSAO");

            Map(x => x.CodInstituicao)
                .Column("COD_INSTITUICAO")
                .Not.Nullable()
                .Length(20);

            Map(x => x.Instituicao)
                .Column("INSTITUICAO")
                .Not.Nullable()
                .Length(100);

            Map(x => x.DocComprobatorio)
                .Column("DOC_COMPROBATORIO")
                .Length(50);
        }
    }
}
