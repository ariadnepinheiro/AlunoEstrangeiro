using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosTurma
    {
        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Turma { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public string Turno { get; set; }

        public string Censo { get; set; }

        public string Curriculo { get; set; }

        public DateTime DataFimTurma { get; set; }
    }
}
