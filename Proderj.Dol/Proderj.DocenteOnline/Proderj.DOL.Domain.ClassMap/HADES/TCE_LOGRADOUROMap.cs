using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
	public class TCE_LOGRADOUROMap : ClassMap<TCE_LOGRADOURO>
	{
        public TCE_LOGRADOUROMap()
		{
            Table("HADES..TCE_LOGRADOURO");

			LazyLoad();

            Id(x => x.ID_LOGRADOURO)
                .Column("ID_LOGRADOURO")
                .Not.Nullable();

            Map(x => x.ID_MUNICIPIO)
                .Column("ID_MUNICIPIO")
                .Not.Nullable()
                .Length(10);

			Map(x => x.NOME)
				.Column("NOME")
				.Not.Nullable()
                .Length(100);

			Map(x => x.CEP)
				.Column("CEP")
                .Not.Nullable()
				.Length(50);

            References(x => x.Municipio, "ID_MUNICIPIO")
                .Cascade.None();
		}
	}
}
