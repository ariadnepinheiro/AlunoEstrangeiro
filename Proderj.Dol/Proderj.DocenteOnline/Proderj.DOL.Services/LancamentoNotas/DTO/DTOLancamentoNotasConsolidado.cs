using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    public class DTOLancamentoNotasConsolidado
    {
        public decimal TotalAulasPrevistas { get; set; }
        public decimal TotalAulasDadas { get; set; }
        public IDictionary<short?, decimal?> MediaTurma { get; set; }

        public IList<DTONotaFrequenciaConsolidado> NotasFrequenciasConsolidadas { get; set; }

        public int TotalBimestresAtivos { get; set; }
       
        public DTOLancamentoNotasConsolidado()
        {
            NotasFrequenciasConsolidadas = new List<DTONotaFrequenciaConsolidado>();
            MediaTurma = new Dictionary<short?, decimal?>();
        }
    }
}
