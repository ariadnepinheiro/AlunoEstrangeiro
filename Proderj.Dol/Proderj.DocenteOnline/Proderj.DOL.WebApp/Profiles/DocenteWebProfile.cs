using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.WebApp.Profiles
{
    public class DocenteWebProfile: MapperProfileBase
    {
        public DocenteWebProfile() : base("DocenteWebProfile") { }

        protected override void CreateMaps()
        {
            CreateMap<DadosFormacaoDocenteDTO, DadosFormacaoDocenteViewModel>()
                .ForMember(dest => dest.NomeDaPagina, opt => opt.Ignore())
                .ForMember(dest => dest.TituloDaPagina, opt => opt.Ignore());

            CreateMap<DadosCapacitacaoDocenteDTO, DadosCapacitacaoDocenteViewModel>()
                .ForMember(dest => dest.NomeDaPagina, opt => opt.Ignore())
                .ForMember(dest => dest.TituloDaPagina, opt => opt.Ignore());
        }
    }
}