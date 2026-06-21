using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOFrequenciaTurma
	{
		public string CodigoFrequencia { get; set; }
		public string Descricao { get; set; }
		public short? AulasDadas { get; set; }
		public short? AulasPrevistas { get; set; }
	}
}
