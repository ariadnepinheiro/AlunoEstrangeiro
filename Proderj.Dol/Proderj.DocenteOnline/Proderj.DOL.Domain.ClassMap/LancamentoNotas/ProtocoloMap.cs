using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class ProtocoloMap : ClassMap<Protocolo>
	{
		public ProtocoloMap()
		{
			Table("VW_TCE_PROTOCOLO_NOTA");

			LazyLoad();

			Id(x => x.Id)
				.Column("ID_PROTOCOLO_NOTA")
					.Not.Nullable();

			Map(x => x.Disciplina)
					.Column("DISCIPLINA")
						.Not.Nullable()
							.Length(100);

			Map(x => x.NomeDisciplina)
				.Column("NOME_DISCIPLINA")
					.Not.Nullable()
						.Length(200);

			Map(x => x.Turma)
				.Column("TURMA")
					.Not.Nullable()
						.Length(20);

			Map(x => x.Ano)
				.Column("ANO")
					.Not.Nullable();

			Map(x => x.Periodo)
				.Column("PERIODO")
					.Not.Nullable();

			Map(x => x.SubPeriodo)
				.Column("SUBPERIODO")
					.Not.Nullable();

            Map(x => x.IdFuncional)
            .Column("IDFUNCIONAL")
                .Not.Nullable()
                    .Length(20);

			Map(x => x.Tipo)
				.Column("TIPO")
					.Not.Nullable()
						.Length(1);

			Map(x => x.DataCadastro)
				.Column("DT_CADASTRO");

		}
	}
}
