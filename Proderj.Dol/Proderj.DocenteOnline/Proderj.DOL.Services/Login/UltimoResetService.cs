using Proderj.DOL.Repository;
namespace Proderj.DOL.Service
{
    public class UltimoResetService : IUltimoResetService
    {
        private IUltimoResetRepository repositorio;

        public UltimoResetService(IUltimoResetRepository repositorio)
        {
            this.repositorio = repositorio;
        }
    }
}
