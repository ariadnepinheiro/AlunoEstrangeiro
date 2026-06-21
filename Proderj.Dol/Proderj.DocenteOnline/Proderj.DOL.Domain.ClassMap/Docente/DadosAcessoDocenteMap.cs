using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class DadosAcessoDocenteMap: ClassMap<DadosAcessoDocente>
    {
        public DadosAcessoDocenteMap()
        {
            Table("VW_ACESSO_DOCENTE");

            LazyLoad();

            CompositeId()
                .KeyProperty(x => x.Id)
                .KeyProperty(x => x.LoginTime);

            Map(x => x.IdFuncional)
                .Column("ID_FUNCIONAL");

            Map(x => x.LoginTime)
                .Column("LOGINTIME");
        }
    }
}
