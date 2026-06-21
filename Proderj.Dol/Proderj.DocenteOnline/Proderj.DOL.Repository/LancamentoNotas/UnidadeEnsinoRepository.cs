using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Domain;
using Proderj.Foundation.Framework;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Proderj.DOL.Repository
{
	public class UnidadeEnsinoRepository : NHRepositoryBase<UnidadeEnsino>, IUnidadeEnsinoRepository
	{
		public UnidadeEnsino ObtemDescricaoPor(string codigoUnidadeEnsino)
		{
			UnidadeEnsino unidadeEnsino = Sessao.CreateCriteria<UnidadeEnsino>()
			 .Add(Restrictions.Eq("Codigo", codigoUnidadeEnsino))
			 .SetProjection
			 (
				Projections.ProjectionList()
				   .Add(Projections.Property("DescricaoCompleta"), "DescricaoCompleta")
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(UnidadeEnsino)))
			 .UniqueResult<UnidadeEnsino>();

			return unidadeEnsino;
		}

        //TODO: assim que puder, renomear para "EnumeraMunicipioPor"
        public IEnumerable<UnidadeEnsino> EnumeraPor(int codigoRegional)
        {
            var unidadesEnsino = Sessao.CreateCriteria<UnidadeEnsino>()
             .CreateAlias("Municipio", "m")
             .CreateAlias("Regional", "r")
             .Add(Restrictions.Eq("r.Codigo", codigoRegional))
             .SetProjection
             (
                 Projections.Distinct(
                    Projections.ProjectionList()
                        .Add(Projections.Property("Municipio"), "Municipio")
                        .Add(Projections.Property("Regional"), "Regional")
                 )
             )
             .SetResultTransformer(Transformers.AliasToBean(typeof(UnidadeEnsino)))
             .List<UnidadeEnsino>();

            return unidadesEnsino;
        }

        public IEnumerable<UnidadeEnsino> EnumeraPor(string codigoMunicipio)
        {
            return base.ListaQueryable()
                .Where(q => q.Municipio.Codigo == codigoMunicipio)
                .AsEnumerable();
        }
	}
}
