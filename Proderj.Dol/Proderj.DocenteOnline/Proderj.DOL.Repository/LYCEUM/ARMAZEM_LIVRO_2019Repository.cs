using NHibernate;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;

namespace Proderj.DOL.Repository
{
    public class ARMAZEM_LIVRO_2019Repository : NHRepositoryBase<ARMAZEM_LIVRO_2019>, IARMAZEM_LIVRO_2019Repository
    {
        public string ObtemCodigoPor(string matricula)
        {
            try
            {
                ISQLQuery consulta = Sessao.CreateSQLQuery(@"
                SELECT CODIGO_ACESSO 
                FROM ARMAZEM_LIVRO_2019 (NOLOCK)
                WHERE CATEGORIA = 'SERVIDOR' AND PESSOA = (select PESSOA from LY_DOCENTE where MATRICULA = :MATRICULA)
            ");

                consulta.SetParameter<string>("MATRICULA", matricula);

                return consulta.UniqueResult<string>();
            }
            catch
            {
                throw;
            }
        }
    }
}