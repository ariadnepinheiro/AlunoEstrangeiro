using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class DocenteDisponivelGLP
	{
		public DocenteDisponivelGLP()
		{
			GrupoHabilitacao = new GrupoHabilitacao();
			Municipio = new Municipio();
			Docente = new Docente();
			UnidadeEnsino = new UnidadeEnsino();
            Regional = new Regional();
		}	

		public virtual int Id { get; set; }

		public virtual DateTime? HoraInicio { get; set; }

		public virtual DateTime? HoraFinal { get; set; }

		public virtual GrupoHabilitacao GrupoHabilitacao { get; set; }

		public virtual Municipio Municipio { get; set; }

		public virtual short DiaSemana { get; set; }

		public virtual Docente Docente { get; set; }

		public virtual UnidadeEnsino UnidadeEnsino { get; set; }

        public virtual Regional Regional { get; set; }
	}
}
