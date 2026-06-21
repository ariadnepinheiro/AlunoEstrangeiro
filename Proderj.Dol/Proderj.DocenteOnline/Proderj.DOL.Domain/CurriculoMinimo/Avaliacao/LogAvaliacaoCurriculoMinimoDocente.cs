using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
	public class LogAvaliacaoCurriculoMinimoDocente
	{
		public LogAvaliacaoCurriculoMinimoDocente()
		{
		}

        public virtual int Id { get; set; }
        
		public virtual int IdAvaliacaoCurriculoMinimoDocente { get; set; }
        
		public virtual int IdAvaliacaoCurriculoMinimo { get; set; }
        
		public virtual bool Resposta { get; set; }
        
		public virtual string Matricula { get; set; }
        
		public virtual DateTime DataCadastro { get; set; }
	}
}
