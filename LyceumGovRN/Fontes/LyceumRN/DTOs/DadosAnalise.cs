using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosAnalise
    {
        public int Periodo { get; set; }

        public bool AnaliseSuplanEditavel { get; set; }

        public bool AnaliseSupedEditavel { get; set; }

        public bool AnaliseDiespEditavel { get; set; }

        public string AnaliseSuplan { get; set; }

        public string AnaliseSuped { get; set; }

        public string AnaliseDiesp { get; set; }

        public string ResponsavelAnaliseSuplan { get; set; }

        public string ResponsavelAnaliseSuped { get; set; }

        public string ResponsavelAnaliseDiesp { get; set; }

        public DateTime DataAnaliseSuplan { get; set; }

        public DateTime DataAnaliseSuped { get; set; }

        public DateTime DataAnaliseDiesp { get; set; }        
    }
}
