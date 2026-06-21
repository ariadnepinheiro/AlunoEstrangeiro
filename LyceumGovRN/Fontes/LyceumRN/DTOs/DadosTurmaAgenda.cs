using System;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosTurmaAgenda
    {
        public int IdAgenda { get; set; }

        public string Modalidade { get; set; }

        public string NomeModalidade { get; set; }

        public string Curso { get; set; }

        public string NomeCurso { get; set; }

        public string Curriculo { get; set; }

        public int Serie { get; set; }

        public DateTime DtFimConfTurno { get; set; }

        public DateTime DtInicioConfVagas { get; set; }

        public DateTime DtFimConfVagas { get; set; }
    }
}
