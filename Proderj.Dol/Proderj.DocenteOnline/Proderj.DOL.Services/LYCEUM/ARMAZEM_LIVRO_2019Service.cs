
using Proderj.DOL.Repository;
namespace Proderj.DOL.Service
{
    public class ARMAZEM_LIVRO_2019Service : IARMAZEM_LIVRO_2019Service
    {
        private readonly IARMAZEM_LIVRO_2019Repository repositorioARMAZEM_LIVRO_2019;

        public ARMAZEM_LIVRO_2019Service(IARMAZEM_LIVRO_2019Repository repositorioARMAZEM_LIVRO_2019)
        {
            this.repositorioARMAZEM_LIVRO_2019 = repositorioARMAZEM_LIVRO_2019;
        }

        public string ObtemCodigoPor(string matricula)
        {
            return repositorioARMAZEM_LIVRO_2019.ObtemCodigoPor(matricula);
        }
    }
}
