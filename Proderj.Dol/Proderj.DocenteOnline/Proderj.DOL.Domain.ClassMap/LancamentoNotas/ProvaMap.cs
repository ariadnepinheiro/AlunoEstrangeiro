using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class ProvaMap : ClassMap<Prova>
	{
		public ProvaMap()
		{
			Table("VW_LY_PROVA");

			LazyLoad();

			//Chave primária é ANO/PERIODO/DISCIPLINA/TURMA/PROVA
			Id(x => x.SubPeriodo)
				.Column("SUBPERIODO")
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

			Map(x => x.Semestre)
				.Column("SEMESTRE")
					.Not.Nullable();

			Map(x => x.TipoProva)
				.Column("PROVA")
					.Not.Nullable()
						.Length(10);

			Map(x => x.Ordem)
				.Column("ORDEM")
					.Not.Nullable();

			Map(x => x.Nome)
				.Column("NOME")
					.Not.Nullable()
						.Length(500);

			Map(x => x.NotaMaxima)
				.Column("NOTA_MAX")
					.Length(15);
		}
	}	
}
