using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvConfVaga
    {
        public int IdConfVaga { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public string Curriculo { get; set; }

        public string Curso { get; set; }

        public string Turno { get; set; }

        public string Sala { get; set; }

        public string Censo { get; set; }

        public string Turma { get; set; }

        public int VagasNovas { get; set; }

        public int VagasContinuidade { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }

        public int SalaCapacidade { get; set; }

    }
}
