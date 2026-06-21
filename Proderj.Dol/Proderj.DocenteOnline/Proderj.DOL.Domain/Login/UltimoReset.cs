using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class UltimoReset
	{
		public UltimoReset()
		{
		}

		#region Propriedades

		public virtual string Matricula { get; set; }

		public virtual DateTime DataUltimoReset { get; set; }

		#endregion
	}

}
