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
	public class ItemTabelaRepository : NHRepositoryBase<ItemTabela>, IItemTabelaRepository
	{
		#region IItemTabelaRepository Members

		public IEnumerable<ItemTabela> EnumeraPor(string tab)
		{
			IEnumerable<ItemTabela> itens = Sessao.CreateCriteria<ItemTabela>()
			.Add(Restrictions.Eq("Tab", tab))
			.Add(Restrictions.Not(Restrictions.Eq("Item","Selecione")))
			.SetProjection
			(
				Projections.ProjectionList()
					.Add(Projections.Property("Item"), "Item")
					.Add(Projections.Property("Descricao"), "Descricao")
			)
			.AddOrder(Order.Asc("Descricao"))			 
			.SetResultTransformer(Transformers.AliasToBean(typeof(ItemTabela)))
			.List<ItemTabela>();

			return itens;
		}

		#endregion
	}
}
