using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvRestricao
    {
        public int IdRestricao { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public string Censo { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }

        public bool Terminalidade { get; set; }
    }
}
