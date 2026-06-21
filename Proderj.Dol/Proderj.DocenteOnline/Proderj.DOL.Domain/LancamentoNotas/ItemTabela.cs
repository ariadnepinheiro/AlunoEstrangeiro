using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class ItemTabela
	{
		public ItemTabela()
		{ 
		}
		
		public virtual int Id { get; set; }

		public virtual string Tab { get; set; }

		public virtual string Item { get; set; }

		public virtual string Descricao { get; set; }
	}

	

}
