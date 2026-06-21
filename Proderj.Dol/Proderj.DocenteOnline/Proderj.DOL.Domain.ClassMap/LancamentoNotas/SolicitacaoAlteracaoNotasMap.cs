using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class SolicitacaoAlteracaoNotasMap : ClassMap<SolicitacaoAlteracaoNotas>
	{
		public SolicitacaoAlteracaoNotasMap()
		{
			Table("VW_TCE_SOLICITACAO_ALTERACAO_NOTA");

			LazyLoad();

			Id(x => x.Id)
				.Column("ID_SOLICITACAO_ALTERACAO_NOTA")
				.Not.Nullable();


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

			Map(x => x.SubPeriodo)
				.Column("SUBPERIODO")
					.Not.Nullable();

			Map(x => x.NumeroFuncionario)
				.Column("NUM_FUNC")
					.Not.Nullable();

			Map(x => x.UnidadeEnsino)
				.Column("UNIDADE_ENS")
					.Not.Nullable()
						.Length(20);

			Map(x => x.DataStatus)
				.Column("DT_STATUS")
					.Not.Nullable();

			Map(x => x.DataLimite)
				.Column("DT_LIMITE");

			Map(x => x.DataSolicitacao)
				.Column("DT_SOLICITACAO")
					.Not.Nullable(); ;

			Map(x => x.Status)
				.Column("STATUS")
					.Not.Nullable()
						.Length(40);

			Map(x => x.Periodo)
				.Column("SEMESTRE")
					.Not.Nullable();

			Map(x => x.Justificativa)
				.Column("JUSTIFICATIVA")
					.Not.Nullable()
						.Length(250);
				

		}

	}
}
