using Proderj.Foundation.Framework;
using AutoMapper;

namespace Proderj.DOL.Service.Profiles
{
    public class CadastroGLPServiceProfile : MapperProfileBase
    {
        public CadastroGLPServiceProfile() : base("CadastroGLPServiceProfile") { }

        protected override void CreateMaps()
        {
            CreateMap<DTOCadastroGlp_InsereDocenteDisponivel, DTOCadastroGlp_VerificaPermissaoParaInsercaoDocenteDisponivel>()
                .ForMember(
                    dest => dest.DocenteDisponivelGlpId,
                    opt => opt.Ignore()
                );
        }
    }
}