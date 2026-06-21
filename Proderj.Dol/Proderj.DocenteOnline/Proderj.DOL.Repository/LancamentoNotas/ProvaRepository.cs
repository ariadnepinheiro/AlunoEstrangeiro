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
	public class ProvaRepository : NHRepositoryBase<Prova>, IProvaRepository
	{
		#region IProvaRepository Members

		public Prova ObtemPor(string disciplina, string turma, short ano, short periodo, short subperiodo)
		{
			DateTime dataAtual = DateTime.Now;

			Prova prova = Sessao.CreateCriteria<Prova>()
			 .Add(Restrictions.Eq("Disciplina", disciplina))
			 .Add(Restrictions.Eq("Turma", turma))
			 .Add(Restrictions.Eq("Ano", ano))
			 .Add(Restrictions.Eq("Semestre", periodo))
			 .Add(Restrictions.Eq("SubPeriodo", subperiodo))
			 .SetProjection
			 (
				Projections.ProjectionList()
				   .Add(Projections.Property("TipoProva"), "TipoProva")
				   .Add(Projections.Property("Nome"), "Nome")
				   .Add(Projections.Property("Ordem"), "Ordem")
				   .Add(Projections.Property("NotaMaxima"), "NotaMaxima")
			 )
			 .SetMaxResults(1)
			 .SetResultTransformer(Transformers.AliasToBean(typeof(Prova)))
			 .UniqueResult<Prova>();

			return prova;
		}

		#endregion
	}
}
