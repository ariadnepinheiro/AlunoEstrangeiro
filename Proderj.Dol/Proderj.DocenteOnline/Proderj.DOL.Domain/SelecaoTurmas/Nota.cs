using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class Nota
    {
        public Nota()
        { }

        public virtual string Aluno { get; set; }

        public virtual string Conceito { get; set; }

        public virtual string RecuperacaoParalela { get; set; }

        public virtual string SemAvaliacao { get; set; }

        public virtual string Justificativa { get; set; }

        public virtual string Prova { get; set; }

        public virtual double Ano { get; set; }

        public virtual double Semestre { get; set; }

        public virtual string Turma { get; set; }

        public virtual string Disciplina { get; set; }

        public virtual string Compareceu { get; set; }

        public virtual DateTime? DataProva { get; set; }

        public virtual double? Ordem { get; set; }

        public virtual double? Formulario { get; set; }
    }
}
