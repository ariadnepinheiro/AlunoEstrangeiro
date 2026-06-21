using System;
using System.Collections.Generic ;
using System.Text;
using System.Linq;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosHorarios
    {
        public string Disciplina { get; set; }

        public string Turma { get; set; }

        public int Ano { get; set; }

        public int Semestre { get; set; }

        public DateTime DtInicioTurma { get; set; }

        public DateTime DtFimTurma { get; set; }

        public int DiaSemana { get; set; }

        public int Aula { get; set; }

        public DateTime HoraInicioAula { get; set; }

        public DateTime HoraFimAula { get; set; }

        public DateTime DtInicioAula { get; set; }

        public DateTime DtFimAula { get; set; }
    }
}
