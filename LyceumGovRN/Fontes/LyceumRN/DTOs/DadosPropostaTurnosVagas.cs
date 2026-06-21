using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosPropostaTurnosVagas
    {
        public int Ano { get; set; }

        public int Periodo { get; set; }

        public string Curso { get; set; }

        public int Serie { get; set; }

        public string Censo { get; set; }

        public int VagasContinuidade { get; set; }

        public int VagasNovas { get; set; }

        public decimal TaxaReprovacao { get; set; }

        public string Matricula { get; set; }

        public int IdPropostaSeeduc { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }
    }
}
