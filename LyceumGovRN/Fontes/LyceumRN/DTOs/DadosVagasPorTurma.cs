using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosVagasPorTurma
    {
        public int Periodo { get; set; }

        public string Turma { get; set; }

        public string Sala { get; set; }

        public int VagasNovas { get; set; }

        public int VagasContinuidade { get; set; }

        public string Turno { get; set; }

        public string Curso { get; set; }

        public string DescricaoCurso { get; set; }

        public int Serie { get; set; }
    }
}
