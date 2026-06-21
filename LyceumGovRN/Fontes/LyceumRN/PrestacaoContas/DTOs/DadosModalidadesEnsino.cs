using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosModalidadesEnsino
    {
        public bool FundamentalModalidade { get; set; }

        public int FundamentalNumeroAlunos { get; set; }

        public int FundamentalNumeroTurnos { get; set; }

        public bool FundamentalHorarioIntegral { get; set; }

        public bool MedioModalidade { get; set; }

        public int MedioNumeroAlunos { get; set; }

        public int MedioNumeroTurnos { get; set; }

        public bool MedioHorarioIntegral { get; set; }

        public bool EjaModalidade { get; set; }

        public int EjaNumeroAlunos { get; set; }

        public int EjaNumeroTurnos { get; set; }

        public bool EjaHorarioIntegral { get; set; }

        public bool EducacaoEspecialModalidade { get; set; }

        public int EducacaoEspecialNumeroAlunos { get; set; }

        public int EducacaoEspecialNumeroTurnos { get; set; }

        public bool EducacaoEspecialHorarioIntegral { get; set; }

    }
}
