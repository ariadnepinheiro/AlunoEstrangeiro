using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class MatriculaMap : ClassMap<Matricula>
    {
        public MatriculaMap()
        {
            Table("VW_LY_MATRICULA");

            LazyLoad();

            Id(x => x.Aluno)
                .Column("ALUNO")
                    .Not.Nullable()
                        .Length(20);

            //CompositeId()
            //    .KeyProperty(x => x.Aluno, "ALUNO")
            //    .KeyProperty(x => x.Disciplina, "DISCIPLINA")
            //    .KeyProperty(x => x.Turma, "TURMA")
            //    .KeyProperty(x => x.Ano, "ANO")
            //    .KeyProperty(x => x.Semestre, "SEMESTRE");

            Map(x => x.Situacao)
                .Column("SIT_MATRICULA")
                    .Not.Nullable()
                        .Length(20);

            Map(x => x.Disciplina).Column("DISCIPLINA").Not.Nullable().Length(20);

            Map(x => x.Turma).Column("TURMA").Not.Nullable().Length(20);

            Map(x => x.Ano).Column("ANO").Not.Nullable();

            Map(x => x.Semestre).Column("SEMESTRE").Not.Nullable();
        }
    }
}
