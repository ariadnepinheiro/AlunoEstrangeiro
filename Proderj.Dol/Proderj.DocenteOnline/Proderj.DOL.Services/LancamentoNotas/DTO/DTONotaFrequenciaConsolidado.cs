using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    public class DTONotaFrequenciaConsolidado
    {
        public string NomeAluno { get; set; }
        public bool SituacaoMatriculado { get; set; }

        public IList<decimal?> Notas { get; set; }
        public IList<short?> Faltas { get; set; }

        public decimal? NotasAcumuladas { get; set; }

        public short? FaltasAcumuladas { get; set; }

        public decimal? PercentualFrequenciaAcumulada { get; set; }
    }
}
