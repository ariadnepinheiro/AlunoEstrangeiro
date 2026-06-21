using System;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosAgendaTurnos
    {
        public int IdAgendaConfTurnoVaga { get; set; }

        public string NomeModalidade { get; set; }

        public string Modalidade { get; set; }

        public string NomeCurso { get; set; }

        public int Serie { get; set; }

        public string Nivel { get; set; }

        public string Curso { get; set; }

        public DateTime DtInicioConfTurno { get; set; }

        public DateTime DtFimConfTurno { get; set; }
    }
}
