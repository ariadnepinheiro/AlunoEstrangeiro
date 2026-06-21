using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Service.Profiles
{
    public class DocenteServiceProfile: MapperProfileBase
    {
        public DocenteServiceProfile(): base("DocenteServiceProfile") { }

        protected override void CreateMaps()
        {
            CreateMap<DadosGeraisDocente, DadosGeraisDocenteDTO>();
            CreateMap<DadosFormacaoDocente, DadosFormacaoDocenteDTO>();
            CreateMap<DadosCapacitacaoDocente, DadosCapacitacaoDocenteDTO>();
        }
    }
}
