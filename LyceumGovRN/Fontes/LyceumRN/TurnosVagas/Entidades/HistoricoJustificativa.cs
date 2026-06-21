using System;

namespace Techne.Lyceum.RN.TurnosVagas.Entidades
{
    public class HistoricoJustificativa
    {
        public int HistoricoJustificativaId { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public int HistoricoTurnoVagaId { get; set; }

        public string Censo { get; set; }

        public string JustificativaContinuidade { get; set; }

        public string JustificativaNovo { get; set; }

        public bool Turno { get; set; }

        public bool Vaga { get; set; }

        public int VagasNovas { get; set; }

        public int VagasContinuidade { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string Matricula { get; set; }
    }
}
