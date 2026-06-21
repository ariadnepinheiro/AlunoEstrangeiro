using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Turmas.DTOs
{
    public class DadosAluno
    {
        public int Ano { get; set; }

        public int Semestre { get; set; }

        public string Turma { get; set; }

        public string Censo { get; set; }

        public string Disciplina { get; set; }

        public string Aluno { get; set; }

        public DateTime Data { get; set; }

        public int TotalTempos { get; set; }

        public int TotalFaltas { get; set; }

        public bool TeveLancamento { get; set; }

        public bool Falta { get; set; }
    }
}
