using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
	public class DTOConfiguracaoNotaDisciplina
	{
		public string GrupoNota { get; set; }
		public string NotaMaxima { get; set; }
		public short CasasDecimais { get; set; }
        public string TemNota { get; set; }
        public string TemFrequencia { get; set; }
	}
}
