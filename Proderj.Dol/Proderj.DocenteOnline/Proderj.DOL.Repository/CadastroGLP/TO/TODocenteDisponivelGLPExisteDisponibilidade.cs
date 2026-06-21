using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Repository
{
	public class TODocenteDisponivelGLPExisteDisponibilidade
	{
		public short DiaSemana { get; set; }

		public long NumeroFuncionario { get; set; }

		public DateTime HoraInicio { get; set; }

		public DateTime HoraFinal { get; set; }

		public string CodigoMunicipio { get; set; }

        public int CodigoRegional { get; set; }
	}
}
