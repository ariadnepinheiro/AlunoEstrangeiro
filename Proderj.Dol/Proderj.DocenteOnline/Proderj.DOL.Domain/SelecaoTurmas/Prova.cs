using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class Prova
    {
        public Prova()
        { }
        
        public virtual string Disciplina { get; set; }
        
        public virtual string Turma { get; set; }
        
        public virtual double Ano { get; set; }
        
        public virtual double Semestre { get; set; }

        public virtual string MediaProva { get; set; }
        
        public virtual double Ordem { get; set; }
        
        public virtual double? Subperiodo { get; set; }
        
        public virtual string Nome { get; set; }
        
        public virtual string NotaMaxima { get; set; }
        
        public virtual string Complemento { get; set; }
    }
}
