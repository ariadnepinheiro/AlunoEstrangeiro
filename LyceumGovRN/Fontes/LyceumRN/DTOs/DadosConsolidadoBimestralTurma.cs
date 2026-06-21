using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosConsolidadoBimestralTurma
    {
        public string Turma { get; set; }

        public string Disciplina { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public int QuantidadeSubPeriodos { get; set; }

        public int? TotalAulasPrevistas { get; set; }

        public int? TotalAulasDadas { get; set; }

        public Decimal? MediaTurmaBimestre1 { get; set; }

        public Decimal? MediaTurmaBimestre2 { get; set; }

        public Decimal? MediaTurmaBimestre3 { get; set; }

        public Decimal? MediaTurmaBimestre4 { get; set; }
    }
}
