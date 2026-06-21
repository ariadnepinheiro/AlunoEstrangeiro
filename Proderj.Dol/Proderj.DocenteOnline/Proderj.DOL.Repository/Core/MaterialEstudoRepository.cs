using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using NHibernate;
using NHibernate.Transform;


namespace Proderj.DOL.Repository
{
    public class MaterialEstudoRepository : NHRepositoryBase<MaterialEstudo>, IMaterialEstudoRepository
    {
        public IList<MaterialEstudo> ObtemPor(string identificador)
        {
            
            ISQLQuery qry = SessaoAuditada.CreateSQLQuery(@"
                SELECT DESCRICAO as [Descricao], MATERIALESTUDOID as [MaterialEstudoId] from lancamentonotas.materialestudo 
                WHERE ATIVO = 1                             
            ");

            

            qry.SetResultTransformer(Transformers.AliasToBean<MaterialEstudo>());

            return qry.List<MaterialEstudo>();
            
        }

        public IList<MaterialEstudo> ObtemIds()
        {

            ISQLQuery qry = SessaoAuditada.CreateSQLQuery(@"
                SELECT MATERIALESTUDOID as [MaterialEstudoId] from lancamentonotas.materialestudo                              
            ");
            
            

            qry.SetResultTransformer(Transformers.AliasToBean<MaterialEstudo>());

            return qry.List<MaterialEstudo>();

        }

        #region IMaterialEstudoRepository Members

      

        #endregion
    }
}
