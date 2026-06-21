using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class LogNotaMap : ClassMap<LogNota>
	{
		public LogNotaMap()
		{
			Table("VW_LY_LOG_NOTA");
			
			LazyLoad();
			
			Id(x => x.Id)
				.Column("ID")
					.Not.Nullable();
			
			Map(x => x.Aluno)
				.Column("ALUNO")
					.Not.Nullable()
						.Length(20);
			
			Map(x => x.Disciplina)
				.Column("DISCIPLINA")
					.Not.Nullable()
						.Length(20);
			
			Map(x => x.Turma)
				.Column("TURMA")
					.Not.Nullable()
						.Length(20);
			
			Map(x => x.Ano)
				.Column("ANO")
					.Not.Nullable();
			
			Map(x => x.Semestre)
				.Column("SEMESTRE")
					.Not.Nullable();
			
			Map(x => x.Prova)
				.Column("PROVA")
					.Not.Nullable()
						.Length(10);
			
			Map(x => x.ValorAnterior)
				.Column("VALOR_ANTERIOR")
					.Length(15);
			
			Map(x => x.ValorAtual)
				.Column("VALOR_ATUAL")
					.Length(15);
			
			Map(x => x.RecuperacaoParalelaAnterior)
				.Column("RECUPERACAO_PARALELA_ANTERIOR")
					.Length(1);
			
			Map(x => x.RecuperacaoParalelaAtual)
				.Column("RECUPERACAO_PARALELA_ATUAL")
					.Length(1);
			
			Map(x => x.SemAvaliacaoAnterior)
				.Column("SEM_AVALIACAO_ANTERIOR")
					.Length(1);
			
			Map(x => x.SemAvaliacaoAtual)
				.Column("SEM_AVALIACAO_ATUAL")
					.Length(1);
			
			Map(x => x.JustificativaAnterior)
				.Column("JUSTIFICATIVA_ANTERIOR")
					.Length(100);
			
			Map(x => x.JustificativaAtual)
				.Column("JUSTIFICATIVA_ATUAL")
					.Length(100);
			
			Map(x => x.Usuario)
				.Column("USUARIO")
					.Length(100);
			
			Map(x => x.DataOperacao)
				.Column("DATA_OPERACAO");
			
			Map(x => x.Operacao)
				.Column("OPERACAO");
		}
	}
}
