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
	public class AlunoRepository : NHRepositoryBase<Aluno>, IAlunoRepository
	{
		public string ObtemNomePor(string matriculaAluno)
		{
			Aluno aluno = Sessao.CreateCriteria<Aluno>()
			.Add(Restrictions.Eq("MatriculaAluno", matriculaAluno))
			.SetProjection
			(
			   Projections.ProjectionList()
				  .Add(Projections.Property("NomeCompleto"), "NomeCompleto")
			)
			.SetMaxResults(1)
			.SetResultTransformer(Transformers.AliasToBean(typeof(Aluno)))
			.UniqueResult<Aluno>();

			return aluno.NomeCompleto;
		}
	}
}
