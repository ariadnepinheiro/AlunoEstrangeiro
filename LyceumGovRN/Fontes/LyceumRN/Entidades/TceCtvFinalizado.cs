using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvFinalizado
    {
        public int IdFinalizado { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public string Censo { get; set; }

        public bool Turno { get; set; }

        public bool Vaga { get; set; }

        public string Matricula { get; set; }

        public DateTime DtFinalizacao { get; set; }
    }
}
