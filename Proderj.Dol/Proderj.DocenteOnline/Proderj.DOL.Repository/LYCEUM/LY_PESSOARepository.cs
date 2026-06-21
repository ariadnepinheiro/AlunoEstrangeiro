using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.SqlCommand;
using NHibernate;

namespace Proderj.DOL.Repository
{
    public class LY_PESSOARepository : NHRepositoryBase<LY_PESSOA>, ILY_PESSOARepository
    {
        public override IQueryable<LY_PESSOA> ListaQueryable()
        {
            return base.ListaQueryable();
        }
        
        public string ObtemIdFuncional(string pessoa){
            ISQLQuery consulta =
         Sessao.CreateSQLQuery(
        @"select concat(a.IDFUNCIONAL,'/',b.VINCULO)
	                  FROM [LYCEUM].[dbo].[LY_PESSOA] a
	  	                inner join [LYCEUM].[dbo].[LY_DOCENTE] b on a.pessoa =b.pessoa
		                where a.pessoa = :pessoa  and b.VINCULO is not null");
            consulta.SetString("pessoa", pessoa);
            var result = consulta.List();
            return result[0].ToString();
        }
        public void AlteraAuditada(LY_PESSOA entidade)
        {
            try
            {
                SessaoAuditada.SaveOrUpdate(entidade);
                SessaoAuditada.Flush();
            }
            catch
            {
                TransacaoRollback();
                throw;
            }
        }
    }
}
