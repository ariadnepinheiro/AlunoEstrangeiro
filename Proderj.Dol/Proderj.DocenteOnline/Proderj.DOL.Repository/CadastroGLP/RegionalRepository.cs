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
	public class RegionalRepository : NHRepositoryBase<Regional>, IRegionalRepository
	{
		public IEnumerable<Regional> Enumera()
		{
            IEnumerable<Regional> regionais = Sessao.CreateCriteria<Regional>()
			.SetProjection
			(
			   Projections.ProjectionList()
					  .Add(Projections.Property("Codigo"), "Codigo")
					  .Add(Projections.Property("Descricao"), "Descricao")
			)
			.AddOrder(Order.Asc
				(
					Projections.Cast(NHibernateUtil.Int32, Projections.Property("Codigo"))
				)
			)
            .SetResultTransformer(Transformers.AliasToBean(typeof(Regional)))
            .List<Regional>();

            return regionais;
		}
	}
}

