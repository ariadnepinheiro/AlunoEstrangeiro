using Proderj.Foundation.Framework;
using AutoMapper;

namespace Proderj.DOL.Service.Profiles
{
    public class AvaliacaoCurriculoMinimoServiceProfile : MapperProfileBase
    {
        public AvaliacaoCurriculoMinimoServiceProfile() : base("AvaliacaoCurriculoMinimoServiceProfile") { }

        protected override void CreateMaps()
        {
            CreateMap<DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor, DTOAvaliacaoCurriculoMinimoDocente_AtualizaCompetenciasPor>();
            CreateMap<DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor, DTOAvaliacaoCurriculoMinimoJustificativa_AtualizaCompetenciasPor>();
        }
    }
}
