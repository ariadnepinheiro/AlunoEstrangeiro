using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class HD_PAISMap : ClassMap<HD_PAIS>
    {
        public HD_PAISMap()
        {
			Table("HADES..HD_PAIS");
			
            LazyLoad();

            Id(x => x.PAIS)
                .Column("PAIS")
                .Not.Nullable()
                .Length(10);

            Map(x => x.NOME)
                .Column("NOME")
                .Not.Nullable()
                .Length(50);
        }
    }
}
