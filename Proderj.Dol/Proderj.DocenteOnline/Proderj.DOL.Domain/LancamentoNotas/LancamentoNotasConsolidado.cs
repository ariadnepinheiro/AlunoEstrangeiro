using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Domain
{
    public class LancamentoNotasConsolidado
    {
        public LancamentoNotasConsolidado()
        {                       
        }

        public virtual string NomeCompleto { get; set; }
        public virtual string SituacaoMatricula { get; set; }

        public virtual decimal? Nota1 { get; set; }
        public virtual short? Falta1 { get; set; }

        public virtual decimal? Nota2 { get; set; }
        public virtual short? Falta2 { get; set; }

        public virtual decimal? Nota3 { get; set; }
        public virtual short? Falta3 { get; set; }

        public virtual decimal? Nota4 { get; set; }
        public virtual short? Falta4 { get; set; }

        public virtual decimal? NotasAcumuladas { get; set; }
        public virtual short? FaltasAcumuladas { get; set; }
        public virtual decimal? PercentualFrequenciaAcumulada { get; set; }
    }
}
