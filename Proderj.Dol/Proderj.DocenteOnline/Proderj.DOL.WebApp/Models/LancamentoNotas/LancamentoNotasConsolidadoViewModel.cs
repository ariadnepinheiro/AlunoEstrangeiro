using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proderj.DOL.Service;

namespace Proderj.DOL.WebApp.Models
{
    public class LancamentoNotasConsolidadoViewModel
    {
        public decimal TotalAulasPrevistas { get; set; }
        public decimal TotalAulasDadas { get; set; }
        public decimal[] MediaBimestre { get; set; }

        public IList<DTONotaFrequenciaConsolidado> NotasFrequenciasConsolidadas { get; set; }
    }
}