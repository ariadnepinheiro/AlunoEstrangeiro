using Proderj.DOL.Domain;
using Proderj.DOL.Repository;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Service.Profiles
{
    public class SubPeriodoLetivoServiceProfile: MapperProfileBase
    {
        public SubPeriodoLetivoServiceProfile(): base("SubPeriodoLetivoServiceProfile") { }

        protected override void CreateMaps()
        {
            CreateMap<SubPeriodoLetivo, DTOSubPeriodoLetivoAtivo>();
            CreateMap<DTOSubPeriodoLetivo_EnumeraParaExibicaoDeCurriculoMinimoPor, TOSubPeriodoLetivo_EnumeraAtivosParaLancamentoDeRespostaDeCurriculoMinimoPor>();
        }
    }
}
