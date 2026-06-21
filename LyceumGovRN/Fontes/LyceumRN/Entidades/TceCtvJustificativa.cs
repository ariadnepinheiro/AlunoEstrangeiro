using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvJustificativa
    {
        public int IdJustificativa { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public string Censo { get; set; }

        public string JustificativaContinuidade { get; set; }

        public string JustificativaNovo { get; set; }

        public bool Turno { get; set; }

        public bool Vaga { get; set; }

        public int? VagasContinuidade { get; set; }

        public int? VagasNovo { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
