using Proderj.DOL.Repository;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Service
{
    public class DocenteService : IDocenteService
    {
        private IDocenteRepository repositorio;

        public DocenteService(IDocenteRepository repositorio)
        {
            this.repositorio = repositorio;
        }
    }
}
