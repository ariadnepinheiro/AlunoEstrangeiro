using System;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosConfVaga
    {
        public int IdCtvConfVaga { get; set; }

        public int Periodo { get; set; }

        public string SalaCapacidade { get; set; }

        public string Turma { get; set; }

        public int VagasContinuidade { get; set; }

        public int VagasNova { get; set; }

        public int NumAlunos { get; set; }

        public string Turno { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public bool Editavel { get; set; }

        public DateTime DtInicioConfVaga { get; set; }

        public DateTime DtFimConfTurno { get; set; }

        public bool Finalizado { get; set; }

        public bool AgendaEncerrada { get; set; }

        public string PerfilResponsavel { get; set; }
    }
}
