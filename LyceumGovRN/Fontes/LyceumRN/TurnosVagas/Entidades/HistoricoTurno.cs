using System;

namespace Techne.Lyceum.RN.TurnosVagas.Entidades
{
    public class HistoricoTurno
    {
        public int HistoricoTurnoId { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public int HistoricoTurnoVagaId { get; set; }

        public string Censo { get; set; }

        public string Turno { get; set; }

        public bool Continuidade { get; set; }

        public bool Novo { get; set; }

        public bool Confirmada { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string Matricula { get; set; }
    }
}
