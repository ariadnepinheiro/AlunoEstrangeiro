using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using Proderj.DOL.Domain;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Proderj.DOL.Repository
{
	public class DisciplinaRepository : NHRepositoryBase<Disciplina>, IDisciplinaRepository
	{
		#region IDisciplinaRepository Members

		public Disciplina ObtemConceitosPor(string codigoDisciplina)
		{
			Disciplina disciplina = Sessao.CreateCriteria<Disciplina>()
			 .Add(Restrictions.Eq("CodigoDisciplina", codigoDisciplina))
			 .SetProjection
			 (
				Projections.ProjectionList()
				   .Add(Projections.Property("GrupoNota"), "GrupoNota")
				   .Add(Projections.Property("QuantCasasDecimais"), "QuantCasasDecimais")
				   .Add(Projections.Property("NotaMaxima"), "NotaMaxima")
                   .Add(Projections.Property("TemNota"), "TemNota")
                   .Add(Projections.Property("TemFrequencia"), "TemFrequencia")
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(Disciplina)))
			 .UniqueResult<Disciplina>();

			return disciplina;
		}

		public Disciplina ObtemDescricaoPor(string codigoDisciplina)
		{
			Disciplina disciplina = Sessao.CreateCriteria<Disciplina>()
			 .Add(Restrictions.Eq("CodigoDisciplina", codigoDisciplina))
			 .SetProjection
			 (
				Projections.ProjectionList()
				   .Add(Projections.Property("DescricaoCompleta"), "DescricaoCompleta")
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(Disciplina)))
			 .UniqueResult<Disciplina>();

			return disciplina;
		}

		#endregion
	}
}
