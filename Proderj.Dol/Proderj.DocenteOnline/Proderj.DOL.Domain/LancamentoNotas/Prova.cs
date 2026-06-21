using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class Prova
    {
        public Prova()
        { 
		}
        
        public virtual string Disciplina { get; set; }
        
        public virtual string Turma { get; set; }

		public virtual string TipoProva { get; set; }

		public virtual string Nome { get; set; }
        
        public virtual short Ano { get; set; }
        
        public virtual short Semestre { get; set; }

		public virtual short SubPeriodo { get; set; }

        public virtual short Ordem { get; set; }
        
        public virtual string NotaMaxima { get; set; }
    
	}
}
