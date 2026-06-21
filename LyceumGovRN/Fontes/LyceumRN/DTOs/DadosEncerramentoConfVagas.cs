using System;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosEncerramentoConfVagas
    {
        public int IdAgendaConfTurnoVaga { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }
       
        public bool Encerrado { get; set; }

        public DateTime DtEncerramento { get; set; }

        public int IdConfVaga { get; set; }

        public string Curriculo { get; set; }

        public string Turno { get; set; }

        public string Sala { get; set; }

        public string Censo { get; set; }

        public string Turma { get; set; }

        public int VagasNovas { get; set; }

        public int VagasContinuidade { get; set; }
    }
}
