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
	public class NucleoRepository : NHRepositoryBase<Nucleo>, INucleoRepository
	{
		public IEnumerable<Nucleo> Enumera()
		{
			IEnumerable<Nucleo> nucleos = Sessao.CreateCriteria<Nucleo>()
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
			.SetResultTransformer(Transformers.AliasToBean(typeof(Nucleo)))
			.List<Nucleo>();

			return nucleos;
		}
	}
}
