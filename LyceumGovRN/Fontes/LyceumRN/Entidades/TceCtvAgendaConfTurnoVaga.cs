using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCtvAgendaConfTurnoVaga : IEntity
    {
        public int IdAgendaConfTurnoVaga { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public int AnoReferencia { get; set; }

        public int PeriodoReferencia { get; set; }

        public DateTime DtInicioConfTurno { get; set; }

        public DateTime DtFimConfTurno { get; set; }

        public DateTime DtInicioConfVagas { get; set; }

        public DateTime DtFimConfVagas { get; set; }

        public bool Encerrado { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }

        public DateTime DtEncerramento { get; set; }

        [AtributoCampo(Nome = "AGENDAID")]
        public int AgendaId { get; set; }
    }
}
