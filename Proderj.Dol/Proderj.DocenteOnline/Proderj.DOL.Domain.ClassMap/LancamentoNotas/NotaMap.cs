using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class NotaMap : ClassMap<Nota>
    {
        public NotaMap()
        {
            Table("VW_LY_NOTA");

            LazyLoad();

			//Chave primária é ALUNO, DISCIPLINA, TURMA, ANO, SEMESTRE, PROVA
			Id(x => x.Id)
				.Column("NOTAID");
					//.GeneratedBy.Identity();					

            Map(x => x.Aluno)
                .Column("ALUNO")
                    .Not.Nullable()
                        .Length(20);

            Map(x => x.Conceito)
                .Column("CONCEITO")
                    .Length(15);

			Map(x => x.RecuperacaoParalela)
				.Column("RECUPERACAO_PARALELA")
					.Length(1);

			Map(x => x.SemAvaliacao)
				.Column("SEM_AVALIACAO")
					.Length(1);

            Map(x => x.Justificativa)
                .Column("JUSTIFICATIVA")
                    .Length(100);

            Map(x => x.TipoProva)
                .Column("PROVA")
                    .Not.Nullable()
                        .Length(10);

            Map(x => x.Ano)
                .Column("ANO")
                    .Not.Nullable();

            Map(x => x.Semestre)
                .Column("SEMESTRE")
                    .Not.Nullable();

            Map(x => x.Turma)
                .Column("TURMA")
                    .Not.Nullable()
                        .Length(20);

            Map(x => x.Disciplina)
                .Column("DISCIPLINA")
                    .Not.Nullable()
                        .Length(20);

			Map(x => x.Compareceu)
				.Column("COMPARECEU")
					.Length(1);

            Map(x => x.DataProva)
                .Column("DATA");

            Map(x => x.Ordem)
                .Column("ORDEM");

            Map(x => x.Formulario)
                .Column("FORMULARIO");
            
            Map(x => x.NotaProva)
                .Column("NOTAPROVA");

            Map(x => x.NotaRecuperacao)
                .Column("NOTARECUPERACAO");

            Map(x => x.MotivoSemNota)
                .Column("MOTIVOSEMNOTAID");
        }
    }

}
