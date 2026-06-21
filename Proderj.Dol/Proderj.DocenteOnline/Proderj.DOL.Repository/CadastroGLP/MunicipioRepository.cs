using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using NHibernate.Transform;
using NHibernate.Criterion;

namespace Proderj.DOL.Repository
{
	public class MunicipioRepository : NHRepositoryBase<Municipio>, IMunicipioRepository
	{
		public bool ExistePor(string codigo)
		{
			var municipio = Sessao.CreateCriteria<Municipio>()
			 .Add(Restrictions.Eq("Codigo", codigo))
			 .SetProjection
			 (
				Projections.ProjectionList()
				   .Add(Projections.Property("Codigo"), "Codigo")

			 )
			 .SetResultTransformer(Transformers.AliasToBean(typeof(Municipio)))
			 .UniqueResult<Municipio>();

			return municipio != null;
		}
	}
}
