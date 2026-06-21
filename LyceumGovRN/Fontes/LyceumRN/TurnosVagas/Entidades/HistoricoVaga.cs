using System;

namespace Techne.Lyceum.RN.TurnosVagas.Entidades
{
    public class HistoricoVaga
    {
        public int HistoricoVagaId { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public int HistoricoTurnoVagaId { get; set; }

        public string Curriculo { get; set; }

        public string Curso { get; set; }

        public string Turno { get; set; }

        public string Sala { get; set; }

        public string Censo { get; set; }

        public string Turma { get; set; }

        public int VagasNovas { get; set; }

        public int VagasContinuidade { get; set; }

        public int DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public DateTime Matricula { get; set; }
    }
}
