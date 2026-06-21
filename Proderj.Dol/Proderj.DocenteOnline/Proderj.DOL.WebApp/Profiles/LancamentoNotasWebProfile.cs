using Proderj.DOL.Service;
using Proderj.DOL.WebApp.Models;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.WebApp.Profiles
{
    public class LancamentoNotasWebProfile: MapperProfileBase
    {
        public LancamentoNotasWebProfile() : base("LancamentoNotasWebProfile") { }

        protected override void CreateMaps()
        {
            CreateMap<LancamentoNotasListaRequestModel, DTOLancamentoNotasSolicitacao>();
            CreateMap<LancamentoNotasSolicitaReaberturaBimestreRequestModel, LancamentoNotasListaRequestModel>()
                .ForMember(dest => dest.ExibeConsolidado, opt => opt.Ignore());

            CreateMap<LancamentoNotasSolicitaReaberturaBimestreRequestModel, DTOSolicitacaoAlteracaoNotas_ConsultaTurma>()
                .ForMember(dest => dest.NumeroFuncionarioDocente, opt => opt.Ignore());

            CreateMap<LancamentoNotasSolicitaReaberturaBimestreRequestModel, DTOSolicitacaoReabertura>()
                .ForMember(dest => dest.NumeroFuncionario, opt => opt.Ignore());

            CreateMap<LancamentoNotasSalvaRequestModel, DTOLancamentoNotasSolicitacao>();
            CreateMap<LancamentoNotasSalvaRequestModel, DTOLancamentoNotasSalvamento>();
        }
    }
}