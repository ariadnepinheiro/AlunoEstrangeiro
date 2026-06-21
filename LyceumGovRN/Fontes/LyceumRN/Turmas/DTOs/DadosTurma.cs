using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Turmas.DTOs
{
    public class DadosTurma
    {
        public int Ano { get; set; }

        public int Semestre { get; set; }

        public string Turma { get; set; }

        public string Censo { get; set; }

        public DateTime MaiorData { get; set; }
    }
}
