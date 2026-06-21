using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvConfTurno
    {
        public int IdConfTurno { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public string Censo { get; set; }

        public string Turno { get; set; }

        public bool Continuidade { get; set; }

        public bool Novo { get; set; }

        public bool Confirmada { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}
