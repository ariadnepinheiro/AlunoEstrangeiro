using System.Collections.Generic;
using NHibernate.Transform;
using Proderj.Foundation.Framework;
using NHibernate;

namespace Proderj.DOL.Repository
{
    public class StoredProcedures : NHRepositoryBase<object>, IStoredProcedures
    {
        public IList<DTOREL_CH_SERV_ANO> REL_CH_SERV_ANO(string idfuncional, string ano)
        {
            //foi necessário criar uma variável do tipo TABLE porque precisava de uma forma de
            //contornar o problema dos campos que possuem ESPAÇO em seus nomes (ex.: [UNIDADE ADMINISTRATIVA]),
            //pois o NHibernate não consegue mapear esses campos para a DTO com o transformador AliasToBean.
            string sql = @"
declare @temp table (
    SEGUNDO_IDVINCULO varchar(MAX),
	SEGUNDA_MATRICULA varchar(MAX),
    SEGUNDO_VINCULO int,
	MATRICULA varchar(MAX),
	IDFUNCIONAL int,
	VINCULO int,
    IDVINCULO  varchar(MAX),
	NOME_COMPL varchar(MAX),
    PRE_NOME_SOCIAL varchar(MAX),
	CPF varchar(MAX),
	FUNCAO varchar(MAX),
	CARGO varchar(MAX),
	CH_REGENCIA decimal,
	UNIDADE_ADMINISTRATIVA varchar(MAX),
	UA_DE_LOTACAO varchar(MAX),
	REGIONAL varchar(MAX),
	MUNICIPIO varchar(MAX),
	HOR_TUR int,
	TOL_NORMAL int,
	TOL_GLP int,
	DIS_INGRESS varchar(MAX),
	SITUACAO varchar(MAX),
    READAPTADO varchar(MAX)
)
insert into @temp (SEGUNDO_IDVINCULO, SEGUNDA_MATRICULA,SEGUNDO_VINCULO, MATRICULA,IDFUNCIONAL, VINCULO,IDVINCULO, NOME_COMPL, PRE_NOME_SOCIAL, CPF, FUNCAO, CARGO, CH_REGENCIA, UNIDADE_ADMINISTRATIVA, UA_DE_LOTACAO, REGIONAL, MUNICIPIO, HOR_TUR, TOL_NORMAL, TOL_GLP, DIS_INGRESS, SITUACAO, READAPTADO)
exec REL_CH_SERV_ANO_DOL :idfuncional, :ANO
select * from @temp
";

            return Sessao.CreateSQLQuery(sql)
                .SetParameter("idfuncional", idfuncional)
                .SetParameter("ANO", ano)
                .SetResultTransformer(Transformers.AliasToBean<DTOREL_CH_SERV_ANO>())
                .List<DTOREL_CH_SERV_ANO>();
        }
    }
}
