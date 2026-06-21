using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class DadosTurmaDocenteMap : ClassMap<DadosTurmaDocente>
    {
        public DadosTurmaDocenteMap()
        {
            Table("VW_TURMA_DOCENTE");

            LazyLoad();

            Id(x => x.Id)
                .Column("ID");

            Map(x => x.IdFuncional)
                .Column("ID_FUNCIONAL");

            Map(x => x.Name)
                .Column("NAME");

            Map(x => x.Section)
                .Column("SECTION");
        }
    }
}
