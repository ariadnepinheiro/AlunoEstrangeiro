using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvConfTurnoInicial
    {
        public int IdConfTurnoInicial { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public string Censo { get; set; }

        public char Turno { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }
    }
}
