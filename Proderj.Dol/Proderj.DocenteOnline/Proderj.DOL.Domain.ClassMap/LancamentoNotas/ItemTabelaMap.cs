using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{	
	public class ItemTabelaMap : ClassMap<ItemTabela>
	{
		public ItemTabelaMap()
		{
			Table("VW_ITEMTABELA");

			LazyLoad();

			Id(x => x.Id)
					.Column("ITEMTABELAID");

			Map(x => x.Tab)
					.Column("TAB")
							.Not.Nullable()
								.Length(20);

			Map(x => x.Item)
					.Column("ITEM")
							.Not.Nullable()
								.Length(40);

			Map(x => x.Descricao)
					.Column("DESCR")				
						.Length(180);
		}
	}
}
