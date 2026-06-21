using Proderj.DOL.Repository;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Service.Profiles
{
    public class RespostaCurriculoMinimoServiceProfile: MapperProfileBase
    {
        public RespostaCurriculoMinimoServiceProfile() : base("RespostaCurriculoMinimoServiceProfile") { }

        protected override void CreateMaps()
        {
            CreateMap<DTORespostaCurriculoMinimo_EnumeraRespostasPorGrupoPor, TOCompetenciaHabilidadeGrupo_EnumeraPor>();
            CreateMap<DTOAvaliacaoCurriculoMinimo_SalvaAvaliacoesEJustificativaPor, DTOAvaliacaoCurriculoMinimoJustificativa_AtualizaCompetenciasPor>();
            CreateMap<DTORespostaCurriculoMinimo_SalvaPor, DTOLogCompetenciaHabilidade>();
            CreateMap<DTORespostaCurriculoMinimo_SalvaPor, TOCompetenciaHabilidadeDocente_RemoveCompetenciasAntigas>();            
        }
    }
}
