using System.Linq;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using NHibernate.Linq;
using NHibernate.Transform;
using NHibernate;

namespace Proderj.DOL.Repository
{
    public class LY_UNIDADE_ENSINORepository : NHRepositoryBase<LY_UNIDADE_ENSINO>, ILY_UNIDADE_ENSINORepository
    {
        public override IQueryable<LY_UNIDADE_ENSINO> ListaQueryable()
        {
            return base.ListaQueryable();
        }

        public IList<T> ListaMunicipioPor<T>(int id_regional)
        {
            ISQLQuery qry = SessaoAuditada.CreateSQLQuery(@"
                select distinct 
                reg.ID_REGIONAL, 
                reg.REGIONAL, 
                mun.CODIGO as ID_MUNICIPIO, 
                mun.UF_SIGLA as UF,
                mun.NOME as MUNICIPIO

                from LY_UNIDADE_ENSINO lyue
                inner join MUNICIPIO mun on mun.CODIGO = lyue.MUNICIPIO
                inner join TCE_REGIONAL reg on reg.ID_REGIONAL = lyue.ID_REGIONAL

                where
                ( 
	                SELECT TOP 1
	                SITUACAO
	                FROM      LY_UNIDADE_ENSINO_SITUACAO UES
	                WHERE     UES.UNIDADE_ENS = lyue.UNIDADE_ENS
	                ORDER BY  DT_SITUACAO DESC
                ) = 'ESTADUAL'
                and lyue.SIT_FUNCIONAMENTO = 'EmAtividade'
                and lyue.ID_REGIONAL = :ID_REGIONAL
            ");

            qry.SetParameter<int>("ID_REGIONAL", id_regional);

            qry.SetResultTransformer(Transformers.AliasToBean<T>());

            return qry.List<T>();
        }

        public IList<TDTO> CreateSQLQuery<TDTO>(string queryString, IDictionary<string, object> parameters)
        {
            var query = Sessao.CreateSQLQuery(queryString);
            foreach (var parameter in parameters)
                query.SetParameter(parameter.Key, parameter.Value);
            query.SetResultTransformer(Transformers.AliasToBean<TDTO>());
            return query.List<TDTO>();
        }
    }
}