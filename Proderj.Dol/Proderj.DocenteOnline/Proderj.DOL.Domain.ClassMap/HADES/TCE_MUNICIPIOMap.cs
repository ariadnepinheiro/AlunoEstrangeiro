using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class TCE_MUNICIPIOMap : ClassMap<TCE_MUNICIPIO>
	{
        public TCE_MUNICIPIOMap()
		{
            Table("HADES..TCE_MUNICIPIO");

			LazyLoad();

            Id(x => x.ID_MUNICIPIO)
                .Column("ID_MUNICIPIO")
                .Not.Nullable()
                .Length(10);

            Map(x => x.ID_MUNICIPIO_PRODERJ)
                .Column("ID_MUNICIPIO_PRODERJ")
                .Not.Nullable()
                .Length(10);

            Map(x => x.UF)
                .Column("UF")
                .Not.Nullable()
                .Length(2);

            Map(x => x.ID_IBGE)
                .Column("ID_IBGE")
                .Nullable()
                .Length(50);

			Map(x => x.ID_IBGE_COM_DV)
                .Column("ID_IBGE_COM_DV")
				.Nullable()
                .Length(50);

			Map(x => x.NOME)
				.Column("NOME")
                .Not.Nullable()
				.Length(50);

            HasMany(x => x.Logradouros)
                .KeyColumn("ID_MUNICIPIO")
                .Inverse()
                .Cascade.None();
		}
	}
}
