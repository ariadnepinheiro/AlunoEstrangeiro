using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvLogEncerramento : IEntity
    {
        public int IdLogEncerramento { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public string Censo { get; set; }

        public string Turma { get; set; }

        public string Restricao { get; set; }

        public string Matricula { get; set; }

        public DateTime DtEncerramento { get; set; }
    }
}
