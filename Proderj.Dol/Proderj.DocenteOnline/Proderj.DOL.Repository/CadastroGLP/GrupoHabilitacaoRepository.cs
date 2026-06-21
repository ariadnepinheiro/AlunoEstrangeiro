using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate;

namespace Proderj.DOL.Repository
{
	public class GrupoHabilitacaoRepository : NHRepositoryBase<GrupoHabilitacao>, IGrupoHabilitacaoRepository
	{
		public bool ExistePor(string agrupamento)
		{
			var grupoHabilitacao = Sessao.CreateCriteria<GrupoHabilitacao>()
			 .Add(Restrictions.Eq("Agrupamento", agrupamento))
			 .SetProjection
			 (
				Projections.ProjectionList()
				   .Add(Projections.Property("Agrupamento"), "Agrupamento")

			 )
			 .SetResultTransformer(Transformers.AliasToBean(typeof(GrupoHabilitacao)))
			 .UniqueResult<GrupoHabilitacao>();

			return grupoHabilitacao != null;
		}

		public IEnumerable<GrupoHabilitacao> Enumera()
		{
			IEnumerable<GrupoHabilitacao> grupos = Sessao.CreateCriteria<GrupoHabilitacao>()
			.Add(Restrictions.Eq("DisponibilidadeGLPDocente", "S"))
			.SetProjection
			(
				Projections.Distinct(
				   Projections.ProjectionList()
						  .Add(Projections.Property("Agrupamento"), "Agrupamento")
						  .Add(Projections.Property("Descricao"), "Descricao")
						  .Add(Projections.Property("Tipo"), "Tipo")
				)
			)
			.SetResultTransformer(Transformers.AliasToBean(typeof(GrupoHabilitacao)))
			.List<GrupoHabilitacao>();

			return grupos;
		}

        public IEnumerable<GrupoHabilitacao> EnumeraPor(int num_func)
        {
            ISQLQuery qry = SessaoAuditada.CreateSQLQuery(@"
                SELECT  vw.DESCRICAO as [Descricao], vw.AGRUPAMENTO as [Agrupamento], vw.TIPO as [Tipo], vw.DISP_GLP_DOL as [DisponibilidadeGLPDocente]
                FROM VW_LY_GRUPO_HABILITACAO vw
                inner join LY_GRUPO_HABILITACAO_DOC A on vw.AGRUPAMENTO = a.AGRUPAMENTO
                INNER JOIN LY_GRUPO_HABILITACAO B ON vw.AGRUPAMENTO = A.AGRUPAMENTO and A.AGRUPAMENTO = B.AGRUPAMENTO
                WHERE A.NUM_FUNC = :NUM_FUNC
                AND vw.DISP_GLP_DOL = 'S'
            ");

            qry.SetParameter<int>("NUM_FUNC", num_func);

            qry.SetResultTransformer(Transformers.AliasToBean<GrupoHabilitacao>());

            return qry.List<GrupoHabilitacao>();
        }
	}
}
